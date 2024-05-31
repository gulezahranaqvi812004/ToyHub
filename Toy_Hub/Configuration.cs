using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace ToyHub
{
    internal class Configuration
    {
        private string ConnectionStr = @"Data Source=(local);Initial Catalog=ToyHub;Integrated Security=True";
        private SqlConnection con;

        private static Configuration _instance;

        public static Configuration getInstance()
        {
            if (_instance == null)
                _instance = new Configuration();
            return _instance;
        }

        private Configuration()
        {
            con = new SqlConnection(ConnectionStr);
        }

        public SqlConnection getConnection()
        {
            if (con.State != System.Data.ConnectionState.Open)
            {
                con.ConnectionString = ConnectionStr; // Set the connection string if not already set
                con.Open();
            }
            else
            {
                // Reset the reader's position to the beginning, in case it's already been read
                con.Close();
                con.Open();
            }
            return con;
        }

        public void CloseConnection()
        {
            if (con.State != System.Data.ConnectionState.Closed)
            {
                con.Close();
            }
        }
    }
}
