using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyHub.BL
{
    public class Person
    {
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public int GenderID { get; set; }
        public DateTime DOB { get; set; }

        public Person(int personID, string firstName, string lastName, string password,string email, string address, string contactNumber, int genderID, DateTime dob)
        {
            PersonID = personID;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
            ContactNumber = contactNumber;
            GenderID = genderID;
            DOB = dob;
        }

        public Person( string firstName, string lastName, string password,string email, string address, string contactNumber, int genderID, DateTime dob)
        {
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
            ContactNumber = contactNumber;
            GenderID = genderID;
            DOB = dob;
        }



        public static int GetGenderId(string gender)
        {
            int genderID = -1;
            if (gender == "Male" || gender == "Female")
            {

                /*genderID = Person.GetGenderId(gender);*/
                if (gender == "Male")
                {
                    genderID = 1;
                }
                else if (gender == "Female")
                {
                    genderID = 2;
                }
            }
            return genderID;
            /* SqlConnection con = null;
             SqlDataReader reader = null;

             try
             {
                 con = Configuration.getInstance().getConnection();
                 con.Open();

                 string sql = "SELECT id FROM Lookup WHERE Value = @Enter AND Category = 'GENDER'";
                 SqlCommand command = new SqlCommand(sql, con);
                 command.Parameters.AddWithValue("@Enter", selectedGender);

                 reader = command.ExecuteReader();

                 if (reader.Read())
                 {
                     int intValue = reader.GetInt32(0);
                     return intValue;
                 }
                 else
                 {
                     // Handle case where no data is retrieved
                     return -1; // Or throw an exception, depending on your requirement
                 }
             }
             catch (Exception ex)
             {
                 Console.WriteLine("An error occurred: " + ex.Message);
                 // Handle the exception appropriately, e.g., log the error or throw it
                 return -1; // Or throw an exception, depending on your requirement
             }
             finally
             {
                 // Close the reader and connection if they are open
                 if (reader != null && !reader.IsClosed)
                 {
                     reader.Close();
                 }
                 if (con != null && con.State != ConnectionState.Closed)
                 {
                     con.Close();
                 }
             }*/
        }


    }
}
