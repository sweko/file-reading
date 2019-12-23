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
            //WithCsvHelper.BruteForceIt();

            //WithCsvHelper.GoRowByRow();

            //WithNReco.UsingGenericDictionary();

            WithNReco.UsingClass();
        }

    }
}
