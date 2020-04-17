using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace P06_KAW_DataAccess.App1
{
    public class Hashes
    {
        public int Number { get; set; }
        public string Hash { get; set; }
    }

    class RainbowTableGenerator
    {
        /* #### STATS ####
         * One Thread: 04:09.92
         * Ten Threads: 00:56.90
         * 20 Threads: 00:52.30
         * 25 Threads: 00:51.41
         * 50 Threads: 00:52.92
         * 100 Threads: 00:55.95
         * 
         * 1 Mio. PostgreSQL INSERTS: 00:00.05
         * 1 Mio. Modulo Operations: 00:00.00
         * Multiple Threads need 0.1 sec additional initialisation time each in order to function properly
         * 
         * Tests ran in VM, don't hate on my specs
         */

        static void Main(string[] args)
        {
            // Config
            int hashCount = 1000000;
            int threadCount = 25;

            Stopwatch stopwatch = new Stopwatch();

            // Create Connection String
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            builder.Host = "localhost";
            builder.Port = 5432;
            builder.Username = "postgres";
            builder.Database = "postgres";
            builder.ApplicationName = "RainbowTableMD5";

            // Connect to DB
            using NpgsqlConnection connection = new NpgsqlConnection(builder.ToString());
            connection.Open();

            // Create Table
            connection.Execute("CREATE TABLE IF NOT EXISTS hashes (number int, hash varchar(100))");
            //connection.Execute("CREATE TABLE IF NOT EXISTS test (number varchar(100))");

            // Generate 1 Mio MD5 Strings
            connection.Execute("DELETE FROM hashes"); // Ensure that Table is empty
            //connection.Execute("DELETE FROM test"); // Ensure that Table is empty
            stopwatch.Start();
            // Begin Test

            List<Thread> threads = new List<Thread>();
            for (int i = 1; i < hashCount; i += hashCount/threadCount)
            {
                // i is from, hashCount/threadCount is to
                int to = i + hashCount / threadCount;
                Thread thread = new Thread(() => MainPerThread(i, to, builder.ToString()));
                thread.Start();
                threads.Add(thread);
                Console.WriteLine($"Form {i} to {to-1}");
                Thread.Sleep(100); // Without this I get a random number of Threads not executing code.
            }

            foreach (Thread thread1 in threads)
            {
                thread1.Join();
            }

            // End Test
            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        static void MainPerThread(int from, int to, string cstring)
        {
            var mD5 = MD5.Create();
            using NpgsqlConnection tconnection = new NpgsqlConnection(cstring);
            tconnection.Open();
            for (int i = from; i < to; i++)
            {
                //tconnection.Execute("INSERT INTO test (number) VALUES (@p)", new { p = GetMd5Hash(mD5, i.ToString()) });

                //Console.WriteLine($"# {i}");
                tconnection.Execute("INSERT INTO hashes (number, hash) VALUES (@p,@q)", new { p = i, q = GetMd5Hash(mD5, i.ToString()) });
                /*
                if (i % 100000 == 0)
                {
                    Console.WriteLine("Reached " + i);
                }
                */
                //break;
            }
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}
