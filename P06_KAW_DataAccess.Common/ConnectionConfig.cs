using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace P06_KAW_DataAccess.Common
{
    public class ConnectionConfig
    {
        public ConnectionConfig()
        {
            NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();
            builder.Host = "localhost";
            builder.Port = 5432;
            builder.Username = "postgres";
            builder.Database = "postgres";
            builder.ApplicationName = "RainbowTableMD5";

            ConnectionString = builder.ToString();
        }

        public string ConnectionString { get; set; }

    }
}
