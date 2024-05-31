using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace ToyHub.DL
{
    internal class StockDL
    {
        private static string connectionString = @"Data Source=(local);Initial Catalog=ToyHub;Integrated Security=True";


        public static string UpdateStock(int stockID, int productID, int quantity, DateTime stockDate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UpdateStock", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@StockID", stockID);
                        command.Parameters.AddWithValue("@ProductID", productID); // Updated parameter name
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@StockDate", stockDate);

                        // Execute the stored procedure and retrieve the result
                        string result = (string)command.ExecuteScalar();

                        // Return the result
                        return result;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Log the error or throw it for the calling code to handle
                Console.WriteLine("SQL Error: " + ex.Message);
                throw; // Re-throw the exception for the calling code to handle
            }
            catch (Exception ex)
            {
                // Log the error or throw it for the calling code to handle
                Console.WriteLine("Error: " + ex.Message);
                throw; // Re-throw the exception for the calling code to handle
            }
        }



        // Method to get the ID of a receiver or sender by name
        private static int GetIDByName(SqlConnection connection, string tableName, string name)
        {
            string query = $"SELECT {tableName}ID FROM {tableName} WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                object result = command.ExecuteScalar();
                return result != null ? (int)result : -1; // Return -1 if not found
            }
        }
    }
}
