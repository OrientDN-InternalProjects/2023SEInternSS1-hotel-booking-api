using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Data.DTOs
{
    public class PaymentRequest
    {
        public int apiid { get; set; }
        public string appuser { get; set; }
        public long apptime { get; set; }
        public long  amount { get; set; }
        public string apptransid { get; set; }
        public string embeddata { get; set; }
        public string item { get; set; }
        public string  mac { get; set; }
        public string bankcode { get; set; }
        public string discountamount { get; set; }
        public string checksum { get; set; }
        public string pmcid { get; set; }
    }
}
