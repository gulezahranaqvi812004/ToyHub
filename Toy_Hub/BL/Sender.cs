using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyHub.BL
{
    public class Sender:Person
    {
        public int SenderID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyContact { get; set; }
        public DateTime JoiningDate { get; set; }

      

        public int statusID { get; set; }
        public Sender(int personID, string firstName, string lastName, string password, string email, string address, string contactNumber, int genderID, DateTime dob, DateTime joiningDate, string companyName, string companyAddress, string companyContact, int status)
        : base(personID, firstName, lastName, password,email, address, contactNumber, genderID, dob)
        {
            SenderID = personID;
            CompanyName = companyName;
            CompanyAddress = companyAddress;
            CompanyContact = companyContact;
            JoiningDate=joiningDate;
            statusID=status;
        }
    }
}
