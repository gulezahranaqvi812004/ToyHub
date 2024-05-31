using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyHub.BL
{
     public class PunchOrder
    {
        public int PunchOrderID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime PunchDate { get; set; }

        public PunchOrder()
        {

        }

        public PunchOrder( int orderID, int productID, int quantity, decimal price)
        {   
            OrderID = orderID;
            ProductID = productID;
            Quantity = quantity;
            Price = price;
        
        }
    }
}
