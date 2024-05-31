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
    public class SalaryDL
    {
        public static void UpdateSalaryStatusAndPaymentMethod(int staffID, int paymentMethod)
        {
            try
            {
                string message = "";

                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open(); // Open the connection

                    using (SqlCommand command = new SqlCommand("UpdateSalaryStatusAndPaymentMethod", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StaffID", staffID);
                        command.Parameters.AddWithValue("@PaymentMethod", paymentMethod);

                        // Add output parameter for message
                        SqlParameter messageParameter = new SqlParameter("@Message", SqlDbType.NVarChar, 100);
                        messageParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(messageParameter);

                        command.ExecuteNonQuery();

                        // Retrieve the message from the output parameter
                        if (messageParameter.Value != DBNull.Value)
                            message = messageParameter.Value.ToString();
                    }
                }
                Console.WriteLine(message);
                MessageBox.Show(message); // If needed to show message box
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating salary status: " + ex.Message);
                MessageBox.Show("Error updating salary status: " + ex.Message);
            }
        }


        public static bool UpdateSalaryAmountWithBonus(int staffID)
        {
            bool isSuccessful = false;
            try
            {
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open(); // Open the connection

                    using (SqlCommand command = new SqlCommand("UpdateSalaryAmountWithBonus", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for StaffID
                        command.Parameters.AddWithValue("@StaffID", staffID);

                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Salary amount updated successfully.");
               /* MessageBox.Show("Salary amount with bonus updated successfully.");*/
                isSuccessful = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating salary amount: " + ex.Message);
                MessageBox.Show("Error updating salary amount: " + ex.Message);
            }
            return isSuccessful;
        }



        public static void UpdateStaffSalary(int staffID, int salary)
        {
            try
            {
                SqlConnection connection = Configuration.getInstance().getConnection();
                using (connection)
                {
                    connection.Close();
                    connection.Open(); // Open the connection before using it
                    using (SqlCommand command = new SqlCommand("UpdateStaffSalary", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parameters
                        command.Parameters.AddWithValue("@StaffID", staffID);
                        command.Parameters.AddWithValue("@Salary", salary);

                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Staff salary updated successfully.");
                MessageBox.Show("Staff salary updated successfully.");
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error updating staff salary: " + ex.Message);
                MessageBox.Show("Error updating staff salary: " + ex.Message);
            }
        }

        public static void AddSalary(int staffID)
        {
            try
            {
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close(); 
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("AddSalary", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@StaffID", staffID);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Salary added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error adding salary: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
