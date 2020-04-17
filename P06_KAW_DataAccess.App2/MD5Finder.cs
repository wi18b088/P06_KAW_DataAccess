using Dapper;
using Npgsql;
using P06_KAW_DataAccess.Common;
using System;

namespace P06_KAW_DataAccess.App2
{
    class MD5Finder
    {
        static void Main(string[] args)
        {
            // Buid Connection String
            var cConf = new ConnectionConfig();

            // Connect to DB
            using NpgsqlConnection connection = new NpgsqlConnection(cConf.ConnectionString);
            connection.Open();

            // Create Table
            connection.Execute("CREATE TABLE IF NOT EXISTS stats_table (input int, result varchar(100), first_test timestamp, last_test timestamp, best_time int, test_count int)");

            // Create Statistic Object
            Stat stat = new Stat();

            // Ask for number
            Console.WriteLine("Please enter a number between 1 and 1.000.000: ");
            stat.Input = int.Parse(Console.ReadLine());

            // hash number
            // lookup number & Stopwatch
        }
    }
}
