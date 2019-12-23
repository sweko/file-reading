using ExcelDataReader;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ExcelLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var sw = Stopwatch.StartNew();
            int rowCount = 0;
            double minYear = double.MaxValue;
            using var stream = File.OpenRead("toread.xlsx");
            using var reader = ExcelReaderFactory.CreateReader(stream);
            //skip headers
            reader.Read();
            while (reader.Read())
            {
                rowCount += 1;
                //int.TryParse(reader.GetValue(6)?.ToString()??"", out int year);
                //if (year < minYear) minYear = year;
                //if (rowCount % 10000 == 0)
                //{
                //    Console.WriteLine($"Processed {rowCount} rows, minimal year is {minYear}. Continuing.");
                //}
            }

            sw.Stop();
            Console.WriteLine($"Processed {rowCount} rows. Finished.");
            Console.WriteLine($"{sw.ElapsedMilliseconds}ms elapsed");
        }
    }
}
