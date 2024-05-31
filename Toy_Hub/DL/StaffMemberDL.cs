using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyHub.BL;
using System.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Windows.Forms;
namespace ToyHub.DL
{
    internal class StaffMemberDL
    {
        public static int GetLastInsertedStaffID()
        {
            int lastInsertedStaffID = -1;

            try
            {
                using (var con = Configuration.getInstance().getConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Close();
                        con.Open();
                    }

                    string query = "SELECT TOP 1 StaffID FROM StaffMember ORDER BY StaffID DESC";

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        object result = command.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            lastInsertedStaffID = Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving last inserted staff ID: " + ex.Message);
                // Handle exception as needed
            }

            return lastInsertedStaffID;
        }
        public static void AddStaffMember(StaffMember staffMember)
        {
            try
            {
                // Check if all required fields are provided
                if (string.IsNullOrEmpty(staffMember.FirstName) || string.IsNullOrEmpty(staffMember.LastName) || string.IsNullOrEmpty(staffMember.Qualification) || string.IsNullOrEmpty(staffMember.Address) || string.IsNullOrEmpty(staffMember.Email) || string.IsNullOrEmpty(staffMember.Password))
                {
                    MessageBox.Show("Please provide valid values for all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate email format
                if (!Validations.IsValidEmail(staffMember.Email))
                {
                    MessageBox.Show("Invalid email format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate contact number format
                if (!Validations.IsValidContactNumber(staffMember.ContactNumber))
                {
                    MessageBox.Show("Invalid Contact Number format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate gender ID
                if (!Validations.ValidateGenderID(staffMember.GenderID))
                {
                    MessageBox.Show("Invalid gender ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate type of member ID
                if (!Validations.ValidatestaffID(staffMember.TypeOfMember))
                {
                    MessageBox.Show("Invalid type of member ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Additional password format validation can be added here

                using (var con = Configuration.getInstance().getConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Close();
                        con.Open();
                    }

                    using (SqlCommand command = new SqlCommand("AddStaffMember", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.AddWithValue("@FirstName", staffMember.FirstName);
                        command.Parameters.AddWithValue("@LastName", staffMember.LastName);
                        command.Parameters.AddWithValue("@Email", staffMember.Email);
                        command.Parameters.AddWithValue("@Address", staffMember.Address);
                        command.Parameters.AddWithValue("@ContactNumber", staffMember.ContactNumber);
                        command.Parameters.AddWithValue("@GenderID", staffMember.GenderID);
                        command.Parameters.AddWithValue("@DOB", staffMember.DOB);
                        command.Parameters.AddWithValue("@Qualification", staffMember.Qualification);
                        command.Parameters.AddWithValue("@JoiningDate", staffMember.JoiningDate);
                        command.Parameters.AddWithValue("@TypeOfMember", staffMember.TypeOfMember);
                        command.Parameters.AddWithValue("@StatusOfMember", staffMember.StatusOfMember);
                        command.Parameters.AddWithValue("@Salary", staffMember.Salary);
                        command.Parameters.AddWithValue("@Password", staffMember.Password); // Add password parameter

                        // Execute the stored procedure
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Staff member added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                if (ex.Number == 2627) // Error number for unique constraint violation
                {
                    if (ex.Message.Contains("UQ_ContactNumber")) // Check if the error message contains the column name
                    {
                        MessageBox.Show("A staff member with the same phone number already exists. Please use a different phone number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (ex.Message.Contains("UQ_Person"))
                    {
                        MessageBox.Show("A staff member with the same email already exists. Please use a different email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("A unique constraint violation occurred. Please check your input and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("An error occurred while adding the staff member. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                MessageBox.Show("An unexpected error occurred. Please try again.");
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public static void UpdateStaffMember(StaffMember staffMember) // Add password parameter
        {
            try
            {
                // Validate input parameters including password format
                if (string.IsNullOrEmpty(staffMember.FirstName) || string.IsNullOrEmpty(staffMember.LastName) || string.IsNullOrEmpty(staffMember.Qualification) || string.IsNullOrEmpty(staffMember.Address) || string.IsNullOrEmpty(staffMember.Email) || string.IsNullOrEmpty(staffMember.Password))
                {
                    MessageBox.Show("Please provide valid values for all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validations.IsValidEmail(staffMember.Email))
                {
                    MessageBox.Show("Invalid email format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validations.IsValidContactNumber(staffMember.ContactNumber))
                {
                    MessageBox.Show("Invalid Contact Number format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validations.ValidateGenderID(staffMember.GenderID))
                {
                    MessageBox.Show("Invalid gender ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validations.ValidatestaffID(staffMember.TypeOfMember))
                {
                    MessageBox.Show("Invalid type of member ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Get database connection
                SqlConnection connection = Configuration.getInstance().getConnection();
                using (connection)
                {
                    connection.Close();
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UpdateStaffMember", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.AddWithValue("@StaffID", staffMember.StaffID);
                        command.Parameters.AddWithValue("@FirstName", staffMember.FirstName);
                        command.Parameters.AddWithValue("@LastName", staffMember.LastName);
                        command.Parameters.AddWithValue("@Email", staffMember.Email);
                        command.Parameters.AddWithValue("@Address", staffMember.Address);
                        command.Parameters.AddWithValue("@ContactNumber", staffMember.ContactNumber);
                        command.Parameters.AddWithValue("@GenderID", staffMember.GenderID);
                        command.Parameters.AddWithValue("@DOB", staffMember.DOB);
                        command.Parameters.AddWithValue("@Qualification", staffMember.Qualification);
                        command.Parameters.AddWithValue("@JoiningDate", staffMember.JoiningDate);
                        command.Parameters.AddWithValue("@TypeOfMember", staffMember.TypeOfMember);
                        command.Parameters.AddWithValue("@StatusOfMember", staffMember.StatusOfMember);
                        command.Parameters.AddWithValue("@Password", staffMember.Password); // Add password parameter

                        // Execute the stored procedure
                        string result = (string)command.ExecuteScalar();

                        MessageBox.Show(result, "Update Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    connection.Close();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("UQ_ContactNumber"))
                    {
                        MessageBox.Show("A staff member with the same phone number already exists. Please use a different phone number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (ex.Message.Contains("UQ_Person"))
                    {
                        MessageBox.Show("A staff member with the same email already exists. Please use a different email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("A unique constraint violation occurred. Please check your input and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("An error occurred while updating the staff member. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred. Please try again.");
                Console.WriteLine("Error: " + ex.Message);
            }
        }



    }
}
