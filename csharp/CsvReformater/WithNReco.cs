using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using NReco.Csv;

namespace CsvReformater
{
    public static class WithNReco
    {
        public static void UsingGenericDictionary()
        {
            Console.WriteLine("Starting");
            Stopwatch sw = Stopwatch.StartNew();
            Stopwatch allsw = Stopwatch.StartNew();

            using var reader = File.OpenText("test.txt");
            var csvReader = new CsvReader(reader, ";");
            csvReader.Read();
            var headers = new string[csvReader.FieldsCount];
            for (int i = 0; i < csvReader.FieldsCount; i++)
            {
                headers[i] = csvReader[i];
            }

            var records = new List<Dictionary<string, string>>(1_000_000);

            while (csvReader.Read())
            {
                //var item = new string[csvReader.FieldsCount];
                var item = new Dictionary<string, string>(csvReader.FieldsCount);
                for (int i = 0; i < csvReader.FieldsCount; i++)
                {
                    item.Add(headers[i], csvReader[i]);
                    //item[i] = csvReader[i];
                }
                records.Add(item);
                //records.Add(item.Zip(headers).ToDictionary(kvp => kvp.Second, kvp => kvp.First));

            }

            sw.Stop();
            Console.WriteLine($"Reading CSV: {sw.ElapsedMilliseconds}ms for {records.Count} records");

            sw = Stopwatch.StartNew();
            //StringBuilder sb = new StringBuilder();
            //sb.AppendLine("[");
            //foreach (var item in records)
            //{
            //    sb.Append("{");
            //    for (int i = 0; i < headers.Length; i++)
            //    {
            //        sb.AppendFormat("\"{0}\":\"{1}\",", headers[i], item[i]);
            //    }
            //    sb.Remove(sb.Length - 1, 1);
            //    sb.AppendLine("},");
            //}
            //sb.Remove(sb.Length - 1, 1);
            //sb.AppendLine("]");
            //var result = sb.ToString();


            var serializer = new JsonSerializer();
            using var writer = new StreamWriter("test.json");
            using var jsonWriter = new JsonTextWriter(writer);
            serializer.Serialize(jsonWriter, records.Take(10));

            //var result = JsonConvert.SerializeObject(records);
            //sw.Stop();
            //Console.WriteLine($"Generating JSON: {sw.ElapsedMilliseconds}ms for {result.Length} bytes");

            //sw = Stopwatch.StartNew();
            //File.WriteAllText("test.json", result);
            sw.Stop();
            Console.WriteLine($"Wrote file in {sw.ElapsedMilliseconds}ms");

            allsw.Stop();
            Console.WriteLine($"Total: {allsw.ElapsedMilliseconds}ms");

        }

        public static void UsingClass()
        {
            Console.WriteLine("Starting");
            Stopwatch sw = Stopwatch.StartNew();
            Stopwatch allsw = Stopwatch.StartNew();

            using var reader = File.OpenText("test.txt");
            var csvReader = new CsvReader(reader, ";");
            csvReader.Read();
            var headers = new string[csvReader.FieldsCount];
            for (int i = 0; i < csvReader.FieldsCount; i++)
            {
                headers[i] = csvReader[i];
            }

            var records = new List<Earning>(1_000_000);

            while (csvReader.Read())
            {
                var item = new Earning
                {
                    IdEarning = csvReader[0],
                    CreatedAt = csvReader[1],
                    UpdatedAt = csvReader[2],
                    UpdatedBy = csvReader[3],
                    CreatedBy = csvReader[4],
                    Account_id = csvReader[5],
                    PeriodEndYear = csvReader[6],
                    TaxYear = csvReader[7],
                    PensionEarningsRaw = csvReader[8],
                    PensionEarningsAdjustment = csvReader[9],
                    HoursRaw = csvReader[10],
                    HoursAdjustment = csvReader[11],
                    ServiceRaw = csvReader[12],
                    ServiceAdjustment = csvReader[13],
                    EarningNotes = csvReader[14],
                    Plan_id = csvReader[15],
                    ImportTransaction_id = csvReader[16],
                    CustomOne = csvReader[17],
                    CustomTwo = csvReader[18],
                    CustomThree = csvReader[19],
                    CustomFour = csvReader[20],
                    HoursAdjustmentNote = csvReader[21],
                    ServiceAdjustmentNote = csvReader[22],
                    StartDate = csvReader[23],
                    EndDate = csvReader[24],
                    Employer_id = csvReader[25],
                    IsDeleted = csvReader[26],
                    JobGroup = csvReader[27]
                };
                records.Add(item);
            }

            sw.Stop();
            Console.WriteLine($"Reading CSV: {sw.ElapsedMilliseconds}ms for {records.Count} records");

            sw = Stopwatch.StartNew();
            var serializer = new JsonSerializer();
            using var writer = new StreamWriter("test.json");
            using var jsonWriter = new JsonTextWriter(writer);
            serializer.Serialize(jsonWriter, records);

            sw.Stop();
            Console.WriteLine($"Wrote file in {sw.ElapsedMilliseconds}ms");

            allsw.Stop();
            Console.WriteLine($"Total: {allsw.ElapsedMilliseconds}ms");
        }


    }
}
