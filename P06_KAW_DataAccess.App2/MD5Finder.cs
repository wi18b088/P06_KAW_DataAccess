using Dapper;
using Npgsql;
using P06_KAW_DataAccess.Common;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;

namespace P06_KAW_DataAccess.App2
{
    class MD5Finder
    {
        static void Main(string[] args)
        {
            // Config
            Stopwatch stopwatch = new Stopwatch();

            // Buid Connection String
            var cConf = new ConnectionConfig();

            // Connect to DB
            using NpgsqlConnection connection = new NpgsqlConnection(cConf.ConnectionString);
            connection.Open();

            // Create Table
            connection.Execute("CREATE TABLE IF NOT EXISTS stats_table (input int, result varchar(100), first_test timestamp, last_test timestamp, best_time int, test_count int)");

            // Create Statistic Object
            StatLocal stat = new StatLocal();

            // Ask for number
            Console.WriteLine("Please enter a number between 1 and 1.000.000: ");
            do
            {
                try
                {
                    stat.Input = int.Parse(Console.ReadLine());
                    break;
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Please provide valid input!");
                }
            } while (true);

            // hash number
            GenerateMD5 MD5Gen = new GenerateMD5();
            stat.Result = MD5Gen.GetMd5Hash(MD5.Create(), stat.Input.ToString());

            // lookup number & Stopwatch
            stat.LastTest = DateTime.Now;
            stopwatch.Start();
            var dataList = connection.Query<HashData>("SELECT * FROM hashes WHERE hash = @p", new { p = stat.Result }).ToList();
            if (dataList.Count > 0)
            {
                // Found Match in Rainbow Table
                stopwatch.Stop();
                stat.Found = true;
                Console.WriteLine("Found value in Lookup Table!");
                UpdateStatsTable(stat, connection);
            }
            else
            {
                stat.Found = false;
                Console.WriteLine("Sorry, no match.");
            }
            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("Lookup took this long: " + elapsedTime);
        }

        private static void UpdateStatsTable(StatLocal stat, NpgsqlConnection connection)
        {

        }
    }
}
