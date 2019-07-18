using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace JsonFormsFactory
{
    class SQLConnector
    {
        SqlConnection SQLConnection;
        public SQLConnector(string host, string user, string pwd)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = host;
            builder.UserID = user;
            builder.Password = pwd;
            builder.InitialCatalog = "master";

            SQLConnection = new SqlConnection(builder.ConnectionString);
            SQLConnection.Open();
            /*string query = "CREATE DATABASE formFactoryDB;\r\n" +
                "USE formFactoryDB;" +
                "CREATE TABLE JsonStorage(" +
                "ID INT NOT NULL PRIMARY KEY AUTOINCREMENT(1,1), " +
                "Json NVARCHAR);" +
                "CREATE TABLE formData(" +
                "PropertyID INT NOT NULL PRIMARY KEY AUTOINCREMENT(1,1)," +
                "PropertyName VARCHAR(50), " +
                "PropertyValue VARCHAR(50));";*/
            string query = "If(db_id(N'formFactoryDB') IS NULL)\r\n" +
                "BEGIN\r\n" +
                "CREATE DATABASE [formFactoryDB];\r\n" +
                "END";

            string query1 = "USE formFactoryDB;" +
                "if not exists (select name from sys.tables where name = 'JsonStorage')" +
                "BEGIN " +
                "CREATE TABLE dbo.JsonStorage" +
                "(" +
                "ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY, " +
                "Json NVARCHAR(50)" +
                "); " +
                "end";
            string query2 = "USE formFactoryDB;" +
                "if not exists (select name from sys.tables where name = 'formData')" +
                "CREATE TABLE formData(" +
                "PropertyID INT NOT NULL PRIMARY KEY IDENTITY(1,1)," +
                "PropertyName VARCHAR(50), " +
                "PropertyValue VARCHAR(50))";
            SqlCommand comand = new SqlCommand(query, SQLConnection);
            int rowsAffected = comand.ExecuteNonQuery();
            //MessageBox.Show(rowsAffected.ToString(), "INFO");
            comand = new SqlCommand(query1, SQLConnection);
            rowsAffected = comand.ExecuteNonQuery();
            //MessageBox.Show(rowsAffected.ToString(), "INFO");
            comand = new SqlCommand(query2, SQLConnection);
            rowsAffected = comand.ExecuteNonQuery();
            //MessageBox.Show(rowsAffected.ToString(), "INFO");
        }

        public bool CreateTable(string name, string[] fields)
        {
            string query = "if not exists (select name from sys.tables where name = @tableName) CREATE TABLE @tableName(@fields)";
            string _fields = "";
            foreach (var field in fields)
            {
                _fields += field;
            }
            SqlCommand command = new SqlCommand(query, SQLConnection);
            command.Parameters.AddWithValue("@tableName", name);
            command.Parameters.AddWithValue("@fields", _fields);
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected == -1)
            {
                return true;
            } else return false;

        }

        public bool Insert(string table, string[] values)
        {
            string query = "insert into @tableName(@fields) values (@values)";
            return false;
        }

        public bool Update()
        {
            string query = "update @tableName set (@expression)";
            return false;
        }

        public bool Remove(string table, string where = "")
        {
            string query = "delete from @tableName";
            if (where != "")
            {
                query += where;
            }

            return false;
        }
    }
}
