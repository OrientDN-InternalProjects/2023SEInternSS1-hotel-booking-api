using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Common.Base;

namespace HotelBooking.Model.Entities
{
    public class BlackList : BaseEntity
    {
        public string Token { get; set; }
    }
}
