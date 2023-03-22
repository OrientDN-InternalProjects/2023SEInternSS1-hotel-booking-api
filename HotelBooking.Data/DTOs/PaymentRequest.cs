using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking.Data.DTOs
{
    public class PaymentRequest
    {
        public string appid { get; set; }
        public string appuser { get; set; }
        public string apptime { get; set; }
        public string amount { get; set; }
        public string apptransid { get; set; }
        public string embeddata { get; set; }
        public string item { get; set; }
        public string  mac { get; set; }
        public string bankcode { get; set; } = string.Empty;
        public string discountamount { get; set; }
        public string checksum { get; set; }
        public string pmcid { get; set; }
        public string status { get; set; }
        public string id { get; set; }
    }
}
