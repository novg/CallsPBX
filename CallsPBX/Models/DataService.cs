using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using System.Data;

namespace CallsPBX.Models
{
    class DataService : IDisposable
    {
        private NpgsqlConnection connect = null;

        internal bool OpenConnection(ConnectData connectData)
        {
            NpgsqlConnectionStringBuilder connectBuilder = new NpgsqlConnectionStringBuilder();
            connectBuilder.Host = connectData.Host;
            connectBuilder.Database = connectData.Database;
            connectBuilder.Username = connectData.Username;
            connectBuilder.Password = connectData.Password;

            connect = new NpgsqlConnection(connectBuilder);
            connect.Open();

            if (connect.FullState == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void CloseConnect()
        {
            connect.Close();
        }

        internal DataTable GetDataCalls(string beginPeriod, string endPeriod, string numin, string numout)
        {
            string sql = string.Format("SELECT datetime, duration, numin, numout FROM calls WHERE datetime " +
                             "BETWEEN '{0}' AND '{1}'", beginPeriod, endPeriod);
            string addition = string.Empty;
            if (numin == string.Empty && numout == string.Empty)
            {
                addition = ";";
            }
            else if (numout == string.Empty)
            {
                addition = string.Format(" AND numin IN ({0});", AddQuotationMarks(numin));
            }
            else if (numin == string.Empty)
            {
                addition = string.Format(" AND numout IN ({0});", AddQuotationMarks(numout));
            }
            else
            {
                addition = string.Format(" AND numin IN ({0}) AND numout IN ({1});",
                    AddQuotationMarks(numin), AddQuotationMarks(numout));
            }

            return GetCalls(sql + addition);
        }

        internal DataTable GetDataCalls(DateTime? begin, DateTime? end, string numin, string numout)
        {
            DateTime dateTime = begin ?? DateTime.Now.AddMinutes(-5);
            string beginPeriod = dateTime.ToString("yyyy-MM-dd HH:mm");
            dateTime = end ?? DateTime.Now;
            string endPeriod = dateTime.ToString("yyyy-MM-dd HH:mm");
            return GetDataCalls(beginPeriod, endPeriod, numin, numout);
        }

        string AddQuotationMarks(string numbers)
        {
            StringBuilder quotationNumbers = new StringBuilder();
            foreach (string number in numbers.Split(new char[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries))
            {
                string s = string.Format("'{0}'", number);
                quotationNumbers.Append(s + ",");
            }
            quotationNumbers.Remove(quotationNumbers.Length - 1, 1);

            return quotationNumbers.ToString();
        }

        DataTable GetCalls(string sql)
        {
            DataTable calls = new DataTable();
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = connect;
                cmd.CommandText = sql;
                NpgsqlDataReader reader = cmd.ExecuteReader();
                calls.Load(reader);
                reader.Close();
            }

            return calls;
        }

        public void Dispose()
        {
            connect.Close();
            connect.Dispose();
        }
    }
}
