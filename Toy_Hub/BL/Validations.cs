using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToyHub.BL
{
    internal class Validations
    {
        public static bool ValidateInt(string text, out int integer)
        {
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Please enter a salary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                integer = 0; // Or any other default value
                return false;
            }

            if (!int.TryParse(text, out integer))
            {
                MessageBox.Show("Please enter a valid integer for the salary.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
        public static bool IsValidEmail(string email)
        {
            // Regular expression pattern for email validation
            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

            // Check if the email matches the pattern
            return Regex.IsMatch(email, emailPattern);
        }

        public static bool IsValidContactNumber(string contactNumber)
        {
            // Regular expression pattern for Pakistani mobile numbers starting with 03 and 11 digits long
            string contactPattern = @"^03\d{9}$";

            // Check if the contact number matches the pattern
            return Regex.IsMatch(contactNumber, contactPattern);
        }
        public static bool ValidateNull(string inputText)
        {
            // Check if the address is not null or empty
            if (string.IsNullOrEmpty(inputText))
            {
                return false;
            }

            // Additional validation rules can be added here

            return true;
        }
        public static bool ValidateGenderID(int genderID)
        {
            if (genderID != 1 && genderID != 2)
            {
                return false ;
            }
            return true;    
        }
        public static bool ValidatestaffID(int typeOfMember)
        {
            int maxTypeID = 7;
            int minTypeID = 6;
            if (typeOfMember < minTypeID || typeOfMember > maxTypeID)
            {
                  return false;
            }
            return true;
        }

    }
}
