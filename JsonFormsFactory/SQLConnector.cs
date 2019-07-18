using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace JsonFormsFactory
{
    class SQLConnector
    {
        public SQLConnector(string host, string user, string pwd)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = host;
            builder.UserID = user;
            builder.Password = pwd;
            builder.InitialCatalog = "master";

            var SQLConnection = new SqlConnection(builder.ConnectionString);
            SQLConnection.Open();
        }

        public bool CreateTable(string name, string[] options)
        {
            return false;
        }

        public bool Insert()
        {
            return false;
        }

        public bool Update()
        {
            return false;
        }

        public bool Remove()
        {
            return false;
        }
    }
}
