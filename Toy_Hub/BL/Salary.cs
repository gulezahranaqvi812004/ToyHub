using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyHub.BL
{
    public class Salary
    {
        public int SalaryID { get; set; }
        public int StaffID { get; set; }
        public decimal Bonus { get; set; }
        public int PaymentMethod { get; set; }
        public DateTime DateOfSalaryReview { get; set; }
        public decimal Amount { get; set; }
        public int SalaryStatus { get; set; }

        // Constructor
        public Salary( int staffID, decimal bonus, int paymentMethod, DateTime dateOfSalaryReview, decimal amount, int salaryStatus)
        {
           
            StaffID = staffID;
            Bonus = bonus;
            PaymentMethod = paymentMethod;
            DateOfSalaryReview = dateOfSalaryReview;
            Amount = amount;
            SalaryStatus = salaryStatus;
        }

        public static int GetSalaryStatus(string status)
        {
            if (status== "Payed")
            {
                return 11;
            }
            else if (status== "Pending")
            {
                return 12;
            }
            return 0;
        }
        public static int GetPaymentMethod(string method)
        {
            if (method == "In Cash")
            {
                return 13;
            }
            else if (method == "Bank Transfer")
            {
                return 14;
            }
            return 0;
        }
    }
}
