using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ToyHub.BL;
using Microsoft.IdentityModel.Tokens;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Windows.Forms;
namespace ToyHub.DL
{
    internal class SenderDL
    {
        public static List<Sender> GetAllSenders()
        {
            List<Sender> senders = new List<Sender>();

            using (SqlConnection connection = Configuration.getInstance().getConnection())
            {
                string query = "SELECT * FROM Sender Join Person ON Sender.SenderID = Person.PersonID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Close();
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            Sender sender = new Sender(
     personID: Convert.ToInt32(reader["PersonID"]),
     firstName: reader["FirstName"].ToString(),
     lastName: reader["LastName"].ToString(),
     password: reader["Password"].ToString(),
     email: reader["Email"].ToString(),
     address: reader["Address"].ToString(),
     contactNumber: reader["ContactNumber"].ToString(),
     genderID: Convert.ToInt32(reader["GenderID"]),
     dob: Convert.ToDateTime(reader["DOB"]),
     joiningDate: Convert.ToDateTime(reader["JoiningDate"]),
     companyName: reader["CompanyName"].ToString(),
     companyAddress: reader["CompanyAddress"].ToString(),
     companyContact: reader["CompanyContact"].ToString(),
     status: Convert.ToInt32(reader["Status"])
 );

                            senders.Add(sender);
                        }
                    }
                }
            }

            return senders;
        }
        public static void UpdateSenderWithPerson(Sender sender)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(sender.FirstName) || string.IsNullOrEmpty(sender.LastName) || string.IsNullOrEmpty(sender.Address) || string.IsNullOrEmpty(sender.Email))
                {
                    MessageBox.Show("Please provide valid values for all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validations.IsValidEmail(sender.Email))
                {
                    MessageBox.Show("Invalid email format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validations.IsValidContactNumber(sender.ContactNumber))
                {
                    MessageBox.Show("Invalid Contact Number format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validations.ValidateGenderID(sender.GenderID))
                {
                    MessageBox.Show("Invalid gender ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check password format
                if (string.IsNullOrEmpty(sender.Password) || sender.Password.Length < 8)
                {
                    MessageBox.Show("Invalid password format. Password must be at least 8 characters long and contain at least one letter, one digit, and one special character.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SqlConnection connection = Configuration.getInstance().getConnection();

                using (connection)
                {
                    connection.Close();
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UpdateSender", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.AddWithValue("@SenderID", sender.SenderID);
                        command.Parameters.AddWithValue("@FirstName", sender.FirstName);
                        command.Parameters.AddWithValue("@LastName", sender.LastName);
                        command.Parameters.AddWithValue("@Email", sender.Email);
                        command.Parameters.AddWithValue("@Address", sender.Address);
                        command.Parameters.AddWithValue("@ContactNumber", sender.ContactNumber);
                        command.Parameters.AddWithValue("@GenderID", sender.GenderID);
                        command.Parameters.AddWithValue("@DOB", sender.DOB);
                        command.Parameters.AddWithValue("@CompanyName", sender.CompanyName);
                        command.Parameters.AddWithValue("@CompanyAddress", sender.CompanyAddress);
                        command.Parameters.AddWithValue("@CompanyContact", sender.CompanyContact);
                        command.Parameters.AddWithValue("@JoiningDate", sender.JoiningDate);
                        command.Parameters.AddWithValue("@Status", sender.statusID);
                        command.Parameters.AddWithValue("@Password", sender.Password); // Add password parameter

                        // Execute the stored procedure
                        string result = (string)command.ExecuteScalar();

                        Console.WriteLine(result); // Display the result
                        MessageBox.Show("Successful!");
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("CompanyContact")) // Check if the error message contains the column name
                    {
                        MessageBox.Show("A sender with the same phone number already exists. Please use a different phone number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (ex.Message.Contains("Email"))
                    {
                        MessageBox.Show("A sender with the same email already exists. Please use a different email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("A unique constraint violation occurred. Please check your input and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                else
                {
                    MessageBox.Show("An error occurred while updating the sender. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

        public static void AddSender(string firstName, string lastName, string email, string address, string contactNumber,
      int genderID, DateTime dob, DateTime joiningDate, string companyName, string companyAddress, string companyContact,
      int status, string password)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Please provide valid values for all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validations.IsValidEmail(email))
                {
                    MessageBox.Show("Invalid email format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validations.IsValidContactNumber(contactNumber))
                {
                    MessageBox.Show("Invalid Contact Number format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Validations.ValidateGenderID(genderID))
                {
                    MessageBox.Show("Invalid gender ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please provide a password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }



                // Create SQL connection
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open();

                    // Create SQL command
                    using (SqlCommand command = new SqlCommand("AddSender", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Address", address);
                        command.Parameters.AddWithValue("@ContactNumber", contactNumber);
                        command.Parameters.AddWithValue("@GenderID", genderID);
                        command.Parameters.AddWithValue("@DOB", dob);
                        command.Parameters.AddWithValue("@JoiningDate", joiningDate);
                        command.Parameters.AddWithValue("@CompanyName", companyName);
                        command.Parameters.AddWithValue("@CompanyAddress", companyAddress);
                        command.Parameters.AddWithValue("@CompanyContact", companyContact);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@Password", password);

                        // Execute the stored procedure
                        string result = (string)command.ExecuteScalar();
                        MessageBox.Show(result);
                        Console.WriteLine("Successfull"); // Display the result
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle SQL exceptions
                if (ex.Number == 2627)
                {
                    if (ex.Message.Contains("CompanyContact")) // Check if the error message contains the column name
                    {
                        MessageBox.Show("A sender with the same phone number already exists. Please use a different phone number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (ex.Message.Contains("Email"))
                    {
                        MessageBox.Show("A sender with the same email already exists. Please use a different email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("A unique constraint violation occurred. Please check your input and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("An error occurred while adding the sender. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

    }
}
