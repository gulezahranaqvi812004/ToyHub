using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyHub.BL
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public int StaffID { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentStatus { get; set; }
        public int PaymentMethod { get; set; }

        // Parameterized constructor
        public Order( DateTime orderDate, int staffID, decimal totalAmount, int paymentStatus, int paymentMethod)
        {
           
            OrderDate = orderDate;
            StaffID = staffID;
            TotalAmount = totalAmount;
            PaymentStatus = paymentStatus;
            PaymentMethod = paymentMethod;
        }
        public Order()   {
           
        }
    }
}
