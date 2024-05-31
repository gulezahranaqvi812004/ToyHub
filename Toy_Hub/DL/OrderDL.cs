using System;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace ToyHub.DL
{
    internal class OrderDL
    {
        private static string connectionString = @"Data Source=(local);Initial Catalog=ToyHub;Integrated Security=True";


        public static void AddPunchOrder(int orderID, int productID, int quantity)
        {
            string punchQuery = "INSERT INTO [ToyHub].[dbo].[PunchOrders] ([OrderID], [ProductID], [Quantity], [PunchDate]) " +
                                "VALUES (@OrderID, @ProductID, @Quantity, GETDATE())";

            string updateStockQuery = "UPDATE [ToyHub].[dbo].[Stock] SET [Quantity] = [Quantity] - @Quantity WHERE [ProductID] = @ProductID";

            string checkStockQuery = "SELECT [Quantity] FROM [ToyHub].[dbo].[Stock] WHERE [ProductID] = @ProductID";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    connection.Close();
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    // Check Stock
                    int currentStockQuantity;
                    using (SqlCommand checkStockCommand = new SqlCommand(checkStockQuery, connection, transaction))
                    {
                        checkStockCommand.Parameters.AddWithValue("@ProductID", productID);
                        currentStockQuantity = (int)checkStockCommand.ExecuteScalar();
                    }

                    if (currentStockQuantity < quantity)
                    {
                        throw new Exception("Error: Input quantity is greater than available stock quantity.Minum available quantity is: " + currentStockQuantity.ToString());
                        return; // Exit the function   
                    }

                    // Add PunchOrder
                    using (SqlCommand punchCommand = new SqlCommand(punchQuery, connection, transaction))
                    {
                        punchCommand.Parameters.AddWithValue("@OrderID", orderID);
                        punchCommand.Parameters.AddWithValue("@ProductID", productID);
                        punchCommand.Parameters.AddWithValue("@Quantity", quantity);

                        int rowsAffected = punchCommand.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected <= 0)
                        {
                            throw new Exception("Failed to add PunchOrder.");
                        }
                    }

                    // Update Stock
                    using (SqlCommand updateStockCommand = new SqlCommand(updateStockQuery, connection, transaction))
                    {
                        updateStockCommand.Parameters.AddWithValue("@ProductID", productID);
                        updateStockCommand.Parameters.AddWithValue("@Quantity", quantity);

                        int rowsAffected = updateStockCommand.ExecuteNonQuery();

                        // Check if any rows were affected
                        if (rowsAffected <= 0)
                        {
                            throw new Exception("Failed to update Stock.");
                        }
                    }

                    // Commit the transaction
                    transaction.Commit();

                    Console.WriteLine("PunchOrder added successfully and Stock updated.");
                    MessageBox.Show("PunchOrder added successfully and Stock updated.");
                }
                catch (SqlException ex)
                {
                    transaction?.Rollback();

                    // Check if the error is due to a foreign key constraint violation
                    if (ex.Number == 547)
                    {
                        Console.WriteLine("Error: Invalid ProductID. Make sure the ProductID exists in the Product table.");
                        MessageBox.Show("Error: Invalid ProductID. Make sure the ProductID exists in the Product table.");
                    }
                    else
                    {
                        Console.WriteLine($"SQL Error: {ex.Message}");
                        MessageBox.Show($"SQL Error: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();

                    Console.WriteLine($"Error: {ex.Message}");
                    MessageBox.Show($"Error: {ex.Message}");
                }
                finally
                {
                    transaction?.Dispose();
                }
            }
        }






        /*  public static void AddPunchOrder(int orderID, int productID, int quantity)
          {
              string query = "INSERT INTO [ToyHub].[dbo].[PunchOrders] ([OrderID], [ProductID], [Quantity], [PunchDate]) " +
                             "VALUES (@OrderID, @ProductID, @Quantity, GETDATE())";

              using (SqlConnection connection = new SqlConnection(connectionString))
              {
                  using (SqlCommand command = new SqlCommand(query, connection))
                  {
                      // Add parameters
                      command.Parameters.AddWithValue("@OrderID", orderID);
                      command.Parameters.AddWithValue("@ProductID", productID);
                      command.Parameters.AddWithValue("@Quantity", quantity);

                      try
                      {
                          connection.Close();
                          connection.Open();
                          int rowsAffected = command.ExecuteNonQuery();
                          Console.WriteLine($"Rows affected: {rowsAffected}");
                          MessageBox.Show($"Rows affected: {rowsAffected}");
                      }
                      catch (SqlException ex)
                      {
                          // Check if the error is due to a foreign key constraint violation
                          if (ex.Number == 547)
                          {
                              Console.WriteLine("Error: Invalid ProductID. Make sure the ProductID exists in the Product table.");
                              MessageBox.Show("Error: Invalid ProductID. Make sure the ProductID exists in the Product table.");
                          }
                          else
                          {
                              Console.WriteLine($"SQL Error: {ex.Message}");
                              MessageBox.Show($"SQL Error: {ex.Message}");
                          }
                      }
                      catch (Exception ex)
                      {
                          Console.WriteLine($"Error: {ex.Message}");
                          MessageBox.Show($"Error: {ex.Message}");
                      }
                  }
              }
          }
  */
        public static int GetNewOrderID(int staffID, decimal totalAmount, int paymentStatus, int paymentMethod)
        {
            try
            {
                int newOrderID = 0;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Close();
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    command.CommandText = "INSERT INTO [ToyHub].[dbo].[Orders] ([OrderDate], [StaffID], [TotalAmount], [PaymentStatus], [PaymentMethod]) " +
                                          "VALUES (GETDATE(), @StaffID, @TotalAmount, @PaymentStatus, @PaymentMethod); " +
                                          "SELECT SCOPE_IDENTITY();";

                    command.Parameters.AddWithValue("@StaffID", staffID);
                    command.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    command.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
                    command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                    newOrderID = Convert.ToInt32(command.ExecuteScalar());
                }

                return newOrderID;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 0;
            }
        }
        public static string AddOrder(int staffID, decimal totalAmount, int paymentStatus, int paymentMethod)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "AddOrder";

                    command.Parameters.AddWithValue("@StaffID", staffID);
                    command.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    command.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
                    command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                    // Execute the stored procedure
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return "Order added successfully";
                    }
                    else
                    {
                        return "Failed to add order";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public static string AddAndPunchOrder(int staffID, decimal totalAmount, int paymentStatus, int paymentMethod, int productID, int quantity)
        {
           
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Close();
                    connection.Open();

                    // Begin transaction
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Add order
                        string addOrderQuery = "INSERT INTO [ToyHub].[dbo].[Orders] ([OrderDate], [StaffID], [TotalAmount], [PaymentStatus], [PaymentMethod]) VALUES (@OrderDate, @StaffID, @TotalAmount, @PaymentStatus, @PaymentMethod); SELECT SCOPE_IDENTITY();";
                        SqlCommand addOrderCommand = new SqlCommand(addOrderQuery, connection, transaction);
                        addOrderCommand.Parameters.AddWithValue("@OrderDate", DateTime.Now);
                        addOrderCommand.Parameters.AddWithValue("@StaffID", staffID);
                        addOrderCommand.Parameters.AddWithValue("@TotalAmount", totalAmount);
                        addOrderCommand.Parameters.AddWithValue("@PaymentStatus", paymentStatus);
                        addOrderCommand.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                        int orderID = Convert.ToInt32(addOrderCommand.ExecuteScalar());

                        // Punch the order
                        string punchOrderQuery = "INSERT INTO [ToyHub].[dbo].[PunchOrders] ([OrderID], [ProductID], [Quantity], [PunchDate]) VALUES (@OrderID, @ProductID, @Quantity, @PunchDate);";
                        SqlCommand punchOrderCommand = new SqlCommand(punchOrderQuery, connection, transaction);
                        punchOrderCommand.Parameters.AddWithValue("@OrderID", orderID);
                        punchOrderCommand.Parameters.AddWithValue("@ProductID", productID);
                        punchOrderCommand.Parameters.AddWithValue("@Quantity", quantity);
                        punchOrderCommand.Parameters.AddWithValue("@PunchDate", DateTime.Now);

                        punchOrderCommand.ExecuteNonQuery();

                        // Commit the transaction
                        transaction.Commit();

                        return "Order added and punched successfully";
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction if an error occurs
                        transaction.Rollback();
                        return "Error: " + ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        }
}
