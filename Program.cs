using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace console_app_csv_postcodes
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Users\A236978\dev\console-app-csv-postcodes\resources\pp-monthly-update-new-version.csv";
            string[] fileContents = File.ReadAllLines(filePath);
            IEnumerable<PostCode> postCodes =
                fileContents.Select((line) => {
                    double price = 0;
                    bool check = double.TryParse(line.Split(",")[1].Trim('"'), out price);
                    return new PostCode{ FullCode = line.Split(",")[3].Trim('"'), Price = price };
                })
                .ToArray();
            var prefixCodeAverages =
                postCodes
                    .GroupBy((postCode) => postCode.PrefixCode)
                    .Select((grouped) => new {
                        PrefixCode = grouped.First().PrefixCode,
                        PriceAverage = grouped.Sum((pca) => pca.Price)
                    })
                    .OrderByDescending((groupedOrderBy) => groupedOrderBy.PriceAverage);

                foreach(var postCode in prefixCodeAverages){
                    Console.WriteLine($"{postCode.PrefixCode} - {postCode.PriceAverage}");
                }
        }

        private class PostCode
        {
            public string FullCode;
            public string PrefixCode => FullCode.Split(" ")[0].Trim();
            public double Price;
        }
    }
}
