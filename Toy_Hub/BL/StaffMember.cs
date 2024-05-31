using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyHub.BL
{
    public class StaffMember:Person
    {
        private decimal salary;

        public int StaffID { get; set; }
        public string Qualification { get; set; }
        public DateTime JoiningDate { get; set; }
        public int TypeOfMember { get; set; }
        public int StatusOfMember { get; set; }
        public int Salary { get; set; }
        public StaffMember(int personID, string firstName, string lastName,string password, string email, string address, string contactNumber, int genderID, DateTime dob, string qualification, DateTime joiningDate, int type, int status)
        : base(personID, firstName, lastName,password, email, address, contactNumber, genderID, dob)
       
        {
            StaffID = personID;
            Qualification = qualification;
            JoiningDate = joiningDate;
            TypeOfMember = type;
            StatusOfMember = status;
        }
        public StaffMember (string firstName, string lastName,string password, string email, string address, string contactNumber, int genderID, DateTime dob, string qualification, DateTime joiningDate, int type, int status, int salary)
      : base(firstName, lastName,password, email, address, contactNumber, genderID, dob)

        {
            this.Salary=salary;
            Qualification = qualification;
            JoiningDate = joiningDate;
            TypeOfMember = type;
            StatusOfMember = status;
        }
  

        public StaffMember(string firstName, string lastName, string password,string email, string address, string contactNumber, int genderID, DateTime dob, string qualification, DateTime joiningDate, int typeOfMember, int statusOfMember, decimal salary) : base(firstName, lastName,password, email, address, contactNumber, genderID, dob)
        {
            Qualification = qualification;
            JoiningDate = joiningDate;
            TypeOfMember = typeOfMember;
            StatusOfMember = statusOfMember;
            this.salary = salary;
        }

        public static int GetStaffStatusID(string status)
        {
            int statusID = -1;
            if (status == "On Leave")
            {
                statusID = 5;
            }
            else if (status == "Expelled")
            {
                statusID = 4;
            }
            else if (status == "Active")
            {
                statusID = 3;
            }
            return statusID;
        }
        public static int GetStaffTypeID(string type)
        {
            int typeOfMember = -1;

            if (type == "Cashier")
            {
                typeOfMember = 6;
            }
            else if (type == "Salesperson")
            {
                typeOfMember = 7;

            }
            return typeOfMember;
        }
       
    }

}
