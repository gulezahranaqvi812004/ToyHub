using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ToyHub.BL;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
namespace ToyHub.DL
{
    public  class AttendanceDL
    {
        public static void MarkLeavingTime(int attendanceID)
        {
            try
            {
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("MarkLeavingTime", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.AddWithValue("@AttendanceID", attendanceID);

                    // Output parameter for indicating if arrival time is null
                    SqlParameter arrivalTimeNullParam = new SqlParameter("@ArrivalTimeNull", SqlDbType.Bit);
                    arrivalTimeNullParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(arrivalTimeNullParam);

                    // Output parameter for indicating if leaving time is already marked
                    SqlParameter leavingTimeMarkedParam = new SqlParameter("@LeavingTimeMarked", SqlDbType.Bit);
                    leavingTimeMarkedParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(leavingTimeMarkedParam);

                    // Execute the command
                    cmd.ExecuteNonQuery();

                    bool arrivalTimeNull = Convert.ToBoolean(arrivalTimeNullParam.Value);
                    bool leavingTimeMarked = Convert.ToBoolean(leavingTimeMarkedParam.Value);

                    if (arrivalTimeNull)
                    {
                        MessageBox.Show("Cannot update attendance because arrival time is null.");
                    }
                    else if (leavingTimeMarked)
                    {
                        MessageBox.Show("Leaving time is already marked.");
                    }
                    else
                    {
                        Console.WriteLine("Attendance record updated successfully.");
                        MessageBox.Show("Leaving time marked");
                    }
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


        }


        public static TimeSpan? GetArrivalTime(int attendanceID)
        {
            TimeSpan? arrivalTime = null;

            try
            {
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT ArrivalTime FROM Attendance WHERE AttendanceID = @AttendanceID", connection);
                    cmd.Parameters.AddWithValue("@AttendanceID", attendanceID);

                    // Execute the command and retrieve the arrival time
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        arrivalTime = (TimeSpan)result;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }

            return arrivalTime;
        }

        public static int GetAttendanceID(int staffID)
        {
            int attendanceID = -1; // Default value if no record found

            try
            {
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("SELECT AttendanceID FROM Attendance WHERE StaffID = @StaffID AND LeavingTime IS NULL", connection);
                    cmd.Parameters.AddWithValue("@StaffID", staffID);

                    // Execute the command and retrieve the AttendanceID
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        attendanceID = Convert.ToInt32(result);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }

            return attendanceID;
        }

        public static void UpdateAttendance(Attendance attendance)
        {
            try
            {
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("UpdateAttendance", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.AddWithValue("@AttendanceID", attendance.AttendanceID);
                    cmd.Parameters.AddWithValue("@StaffID", attendance.StaffID);
                    cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate);
                    cmd.Parameters.AddWithValue("@ArrivalTime", attendance.ArrivalTime);
                    cmd.Parameters.AddWithValue("@LeavingTime", attendance.LeavingTime);

                    // Execute the command
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Attendance record updated successfully.");
                    MessageBox.Show("Attendance record updated successfully.");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public static List<StaffMember> GetStaffDetails(List<int> staffIDs)
        {
            List<StaffMember> staffMembers = new List<StaffMember>();

            try
            {
                // Create connection
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open();
                    // Create command
                    using (SqlCommand command = new SqlCommand())
                    {
                        // Set command text to a query that retrieves staff details based on their IDs
                        command.CommandText = "SELECT p.FirstName, p.LastName, p.Email,p.Password, p.Address, p.ContactNumber, p.GenderID, p.DOB, s.Qualification, s.JoiningDate, s.TypeOfMember, s.StatusOfMember, s.Salary FROM [ToyHub].[dbo].[Person] p INNER JOIN [ToyHub].[dbo].[StaffMember] s ON p.PersonID = s.StaffID WHERE p.PersonID IN (" + string.Join(",", staffIDs) + ")";
                        command.Connection = connection;

                        // Execute command
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if there are rows returned
                            while (reader.Read())
                            {
                                // Get staff details from the result
                                string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                                string email = reader.GetString(reader.GetOrdinal("Email"));
                                string address = reader.GetString(reader.GetOrdinal("Address"));
                                string contactNumber = reader.GetString(reader.GetOrdinal("ContactNumber"));
                                int genderID = reader.GetInt32(reader.GetOrdinal("GenderID"));
                                DateTime dob = reader.GetDateTime(reader.GetOrdinal("DOB"));
                                string qualification = reader.GetString(reader.GetOrdinal("Qualification"));
                                DateTime joiningDate = reader.GetDateTime(reader.GetOrdinal("JoiningDate"));
                                int typeOfMember = reader.GetInt32(reader.GetOrdinal("TypeOfMember"));
                                int statusOfMember = reader.GetInt32(reader.GetOrdinal("StatusOfMember"));
                                decimal salary = Convert.ToDecimal(reader.GetInt32(11));
                                string password = reader.GetString(reader.GetOrdinal("Password"));
                                // Create a new StaffMember object and add it to the list
                                StaffMember staffMember = new StaffMember(firstName, lastName,password, email, address, contactNumber, genderID, dob, qualification, joiningDate, typeOfMember, statusOfMember, salary);

                                staffMembers.Add(staffMember);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }

            return staffMembers;
        }

        public static List<int> GetStaffIDsWithFullAttendance()
        {
            List<int> staffIDs = new List<int>();

            try
            {
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open();
                    // Create command
                    using (SqlCommand command = new SqlCommand("GetStaffIDsForFullAttendance", connection))
                    {
                        // Set command type
                        command.CommandType = CommandType.StoredProcedure;

                        // Execute command
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Check if there are rows returned
                            if (reader.HasRows)
                            {
                                // Read each row
                                while (reader.Read())
                                {
                                    // Get StaffID from the result
                                    int staffID = reader.GetInt32(0);
                                    // Add StaffID to the list
                                    staffIDs.Add(staffID);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error: " + ex.Message);
            }

            return staffIDs;
        }
        public static void AddAttendance(Attendance attendance)
        {
            try
            {
               
                using (SqlConnection connection = Configuration.getInstance().getConnection())
                {
                    connection.Close();
                    connection.Open();

                    SqlCommand cmd = new SqlCommand("AddAttendance", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Input parameters
                    cmd.Parameters.AddWithValue("@StaffID", attendance.StaffID);
                    cmd.Parameters.AddWithValue("@AttendanceDate", attendance.AttendanceDate);
                    cmd.Parameters.AddWithValue("@ArrivalTime", attendance.ArrivalTime ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LeavingTime", attendance.LeavingTime ?? (object)DBNull.Value);

                    // Output parameter
                    SqlParameter resultParam = new SqlParameter("@Result", SqlDbType.VarChar, 100);
                    resultParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(resultParam);

                    cmd.ExecuteNonQuery();

                    // Retrieve the result message
                    string result = Convert.ToString(cmd.Parameters["@Result"].Value);

                    // Display the result message in a message box
                    MessageBox.Show(result);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message);
            }
        }


    }
}
