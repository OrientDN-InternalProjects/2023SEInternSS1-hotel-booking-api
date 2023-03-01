using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Common.Enums;

namespace HotelBooking.Data.ViewModel
{
    public class InforBookingResponse
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public IEnumerable<RoomVM> Rooms { get; set; }

        public string InformationSender()
        {
            string room_infor = "";
            foreach (var room in Rooms)
            {
                room_infor += $"{room.RoomType}"+ $"; {room.Description} \n";
            }

            return  $"Id booking: {Id.ToString()}\n" 
                +  $"From: {From.ToString()} -" + $"To: {To.ToString()}\n"
                + room_infor 
                +  $"Amount: {Amount.ToString()}";
        }
    }
    
}
