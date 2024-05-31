using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace ToyHub.DL
{
    internal class LoginDL
    {
        public static int isValidLogin(string email, string password)
        {
            try
            {
                // Check if email exists in the database and verify password
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT PersonID FROM Person JOIN StaffMember ON Person.PersonID = StaffMember.StaffID WHERE Email = @Email AND Password=@Password AND StaffMember.TypeOfMember = 6 AND StaffMember.StatusOfMember = 3", connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            // Login successful, return StaffID
                            int staffID = Convert.ToInt32(result);
                            Console.WriteLine($"Staff with ID {staffID} logged in successfully.");
                           /* MessageBox.Show($"Staff with ID {staffID} logged in successfully.");*/
                            return staffID;
                        }
                        else
                        {
                            // Email or password is incorrect
                            Console.WriteLine("Incorrect email or password.");
                          /*  MessageBox.Show("Incorrect email or password.");*/
                            return -1; // Indicate invalid login
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
                return -1; // Indicate invalid login
            }
        }
        public static int ManagerLogin(string email, string password)
        {
            try
            {
                // Check if email exists in the database and verify password
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT PersonID FROM Person JOIN Manager ON Person.PersonID = Manager.ManagerID WHERE Email = @Email AND Manager.Password=@Password", connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            // Login successful, return StaffID
                            int managerID = Convert.ToInt32(result);
                             return managerID;
                        }
                        else
                        {
                            // Email or password is incorrect
                            Console.WriteLine("Incorrect email or password.");
                           /* MessageBox.Show("Incorrect email or password.");*/
                            return -1; // Indicate invalid login
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("An error occurred: " + ex.Message);
                return -1; // Indicate invalid login
            }
        }
    }
}
