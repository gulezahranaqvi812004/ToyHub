using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ToyHub.BL;
using System.Windows.Forms;
namespace ToyHub.DL
{
    internal class ProductDL
    {

        public static List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            try
            {
               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Close();
                    connection.Open();

                    // SQL query to retrieve all products
                    string query = "SELECT ProductID, Name, Price,Description FROM Product";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product product = new Product
                                {
                                    ProductID = Convert.ToInt32(reader["ProductID"]),
                                    Name = reader["Name"].ToString(),
                                   
                                    Price = Convert.ToDecimal(reader["Price"]),
                                  
                                    Description = reader["Description"].ToString()
                                };

                                products.Add(product);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("An error occurred while retrieving products: " + ex.Message);
                MessageBox.Show("An error occurred while retrieving products: " + ex.Message);
            }

            return products;
        }
        private static string connectionString = @"Data Source=(local);Initial Catalog=ToyHub;Integrated Security=True";
        public static decimal GetTotalAmount(int productId, int quantity)
        {
            decimal totalAmount = 0;

            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Close();
                    con.Open();

                    string query = "SELECT [Price] * @Quantity AS TotalAmount FROM [ToyHub].[dbo].[Product] WHERE [ProductID] = @ProductID";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.Parameters.AddWithValue("@ProductID", productId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        totalAmount = Convert.ToDecimal(reader["TotalAmount"]);
                    }

                    reader.Close();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return totalAmount;
        }


        public static int GetProductIDByName(string productName)
        {
            int productID = -1; // Default value if no product found

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Close();
                    con.Open();

                    SqlCommand cmd = new SqlCommand("GetProductIDByName", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters
                    cmd.Parameters.AddWithValue("@ProductName", productName);

                    // Execute the command
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        productID = (int)result;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL errors
                Console.WriteLine("SQL Error: " + ex.Message);
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other errors
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }

            return productID;
        }
        public static void AddProduct(string productName, decimal price, string description)
        {
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Close();
                    con.Open();

                    using (SqlCommand command = new SqlCommand("AddProduct", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Name", productName);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Description", description);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL Server exceptions
                if (ex.Number == 2627) // Error number for unique constraint violation
                {
                    throw new Exception("A product with the same name already exists. Please use a different name.", ex);
                }
                else
                {
                    throw new Exception("An error occurred while adding the product. Please try again.", ex);
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                throw new Exception("An unexpected error occurred. Please try again.", ex);
            }
        }



        public static string UpdateProduct(string connectionString, int productId, string productName, decimal price, string description)
        {
            // Validation for product name (allow only text)
            if (!IsTextOnly(productName))
            {
                MessageBox.Show("Product name can only contain letters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null; // or throw an exception depending on your preference
            }

            // Validation for price (integer only)
            if (price % 1 != 0)
            {
                MessageBox.Show("Price must be a whole number without decimal points.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null; // or throw an exception depending on your preference
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create command for stored procedure
                    using (SqlCommand command = new SqlCommand("UpdateProduct", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@ProductID", productId);
                        command.Parameters.AddWithValue("@Name", productName);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Description", description);

                        // Execute the stored procedure and retrieve the result
                        string result = (string)command.ExecuteScalar();

                        // Return the result
                        return result;
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL Server exceptions
                MessageBox.Show("An error occurred while updating the product. Please try again.");
                Console.WriteLine("SQL Error: " + ex.Message);
                return null; // or throw ex; depending on how you want to handle it
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                MessageBox.Show("An unexpected error occurred. Please try again.");
                Console.WriteLine("Error: " + ex.Message);
                return null; // or throw ex; depending on how you want to handle it
            }
        }

        // Method to check if a string contains only text (letters)
        private static bool IsTextOnly(string input)
        {
            return input.All(char.IsLetter);
        }




        public static int GetProductIdByName(string productName)
        {
            int productId = -1; // Initialize productId to -1, indicating not found

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT ProductID FROM Product WHERE Name = @ProductName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);

                        object result = command.ExecuteScalar();

                        // Check if a product ID is found
                        if (result != null && result != DBNull.Value)
                        {
                            productId = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error: " + ex.Message);
            }

            return productId;
        }
    }
}
