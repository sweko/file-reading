using CsvHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CsvReformater
{
    public static class WithCsvHelper
    {
        public static void BruteForceIt()
        {
            Console.WriteLine("Starting");
            Stopwatch sw = Stopwatch.StartNew();
            using var reader = File.OpenText("test.txt");
            using var csv = new CsvReader(reader);
            csv.Configuration.Delimiter = ";";
            csv.Configuration.HasHeaderRecord = true;

            var records = csv.GetRecords<dynamic>().ToArray();
            sw.Stop();
            Console.WriteLine($"Reading CSV: {sw.ElapsedMilliseconds}ms for {records.Count()} records");

            sw = Stopwatch.StartNew();
            var result = JsonConvert.SerializeObject(records);
            sw.Stop();
            Console.WriteLine($"Generating JSON: {sw.ElapsedMilliseconds}ms for {result.Length} bytes");

            sw = Stopwatch.StartNew();
            File.WriteAllText("test.json", result);
            sw.Stop();
            Console.WriteLine($"Wrote file in {sw.ElapsedMilliseconds}ms");
        }

        public static void GoRowByRow()
        {
            Console.WriteLine("Starting");
            Stopwatch[] sws = { Stopwatch.StartNew(), new Stopwatch(), new Stopwatch(), new Stopwatch() };
            long[] totals = { 0, 0, 0, 0 };

            using var reader = File.OpenText("test.txt");
            using var writer = new StreamWriter("test.json");
            using var csv = new CsvReader(reader);
            csv.Configuration.Delimiter = ";";
            csv.Configuration.HasHeaderRecord = true;
            sws[1].Restart();
            csv.Read();
            csv.ReadHeader();
            sws[1].Stop();
            totals[1] += sws[1].ElapsedTicks;
            var index = 0;
            writer.WriteLine("[");

            sws[1].Restart();
            var read = csv.Read();
            sws[1].Stop();
            totals[1] += sws[1].ElapsedTicks;

            while (read)
            {
                sws[1].Restart();
                var record = csv.GetRecord<dynamic>();
                sws[1].Stop();
                totals[1] += sws[1].ElapsedTicks;

                sws[2].Restart();
                var json = JsonConvert.SerializeObject(record);
                sws[2].Stop();
                totals[2] += sws[2].ElapsedTicks;


                sws[3].Restart();
                writer.Write(json);
                writer.WriteLine(",");
                sws[3].Stop();
                totals[3] += sws[3].ElapsedTicks;
                index++;
                if (index % 12347 == 0)
                {
                    Console.WriteLine(index);
                    sws[3].Restart();
                    writer.Flush();
                    sws[3].Stop();
                    totals[3] += sws[3].ElapsedTicks;
                }

                sws[1].Restart();
                read = csv.Read();
                sws[1].Stop();
                totals[1] += sws[1].ElapsedTicks;
            }
            sws[3].Restart();
            writer.WriteLine("]");
            writer.Flush();
            sws[3].Stop();
            totals[3] += sws[3].ElapsedTicks;
            sws[0].Stop();
            Console.WriteLine($"Processed {index} values in {sws[0].ElapsedMilliseconds}ms");
            Console.WriteLine($"Time spent reading csv is {totals[1]}ms");
            Console.WriteLine($"Time spent generating json is {totals[2]}ms");
            Console.WriteLine($"Time spent writing file is {totals[3]}ms");
        }
    }
}
