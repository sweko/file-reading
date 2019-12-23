using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace CsvReformater
{
    class Program
    {
        static void Main(string[] args)
        {
            // Brute forcing it 
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

            //using var reader = File.OpenText("test.txt");
            //using var writer = new StreamWriter("test.json");
            //using var csv = new CsvReader(reader);
            //csv.Configuration.Delimiter = ";";
            //csv.Configuration.HasHeaderRecord = true;
            //csv.Read();
            //csv.ReadHeader();
            //var index = 0;
            //while (csv.Read())
            //{
            //    var json = JsonConvert.SerializeObject(csv.GetRecord<dynamic>());
            //    writer.WriteLine(json);
            //    index++;
            //    if (index % 10000 == 0)
            //    {
            //        Console.WriteLine(index);
            //    }
            //}

            //sw.Stop();
            //Console.WriteLine(sw.ElapsedMilliseconds / 1000);
        }
    }
}
