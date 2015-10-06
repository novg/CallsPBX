using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallsPBX.Models
{
    class CallsDataBase
    {
        private string connectionString = string.Empty;

        public CallsDataBase(ConnectData connectData)
        {
            NpgsqlConnectionStringBuilder connectBuilder = new NpgsqlConnectionStringBuilder();
            connectBuilder.Host = connectData.Host;
            connectBuilder.Database = connectData.Database;
            connectBuilder.Username = connectData.Username;
            connectBuilder.Password = connectData.Password;
            connectionString = connectBuilder.ConnectionString;
        }

        internal DataTable GetCalls()
        {
            string sqlString = "SELECT datetime, duration, numin, numout FROM calls " +
                         "WHERE datetime " +
                         "BETWEEN '2015-08-17 16:06' AND '2015-08-24 16:11';";
            NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(sqlString, connectionString);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);

            var calls = from call in dt.AsEnumerable()
                        where new string[] {"77080", "73407", "73442", "77346"}.Contains(call.Field<string>("numin"))
                        select call;

            return calls.CopyToDataTable();
        }
    }
}
