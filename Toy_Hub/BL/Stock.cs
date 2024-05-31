using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyHub.BL
{
    internal class Stock
    {
        public int StockID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public DateTime StockDate { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }
    }
}
