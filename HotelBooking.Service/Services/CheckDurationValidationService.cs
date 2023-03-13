using HotelBooking.Data.Interfaces;
using HotelBooking.Data.ViewModel;
using HotelBooking.Service.IServices;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Service.Services
{
    public class CheckDurationValidationService : ICheckDurationValidationService
    {
        private readonly IBookedRoom bookedRoomRepository;

        public CheckDurationValidationService(IBookedRoom bookedRoomRepository)
        {
            this.bookedRoomRepository = bookedRoomRepository;
        }

        public async Task<bool> CheckValidationDurationForRoom(DurationVM newDuration, Guid roomId)
        {
            var returnObjects = await bookedRoomRepository.GetAll().Where(x => x.RoomId.Equals(roomId)).Select(x => new DurationVM { From = x.From.Date, To = x.To.Date }).ToListAsync();
            if (!returnObjects.Any())
            {
                return true;
            }
            SortedList<DateTime, bool> times = new SortedList<DateTime, bool>();
            foreach (var item in returnObjects)
            {
                if (times.ContainsKey(item.From))
                {
                    times.Remove(item.From);
                }
                else
                {
                    times.Add(item.From, true);
                }
                if (times.ContainsKey(item.To))
                {
                    times.Remove(item.To);
                }
                else
                {
                    times.Add(item.To, false);
                }
            }
            if (DateTime.Compare(newDuration.From.Date, times.ElementAt(times.Count - 1).Key.Date) >= 0)
                return true;
            if (times.ContainsKey(newDuration.From))
            {
                if (times[newDuration.From].Equals(false))
                {
                    var next_checkIn_Index = times.IndexOfKey(newDuration.From) + 1;
                    if (next_checkIn_Index > times.Count) { return true; }
                    if (DateTime.Compare(newDuration.To.Date, times.ElementAt(next_checkIn_Index).Key.Date) <= 0)
                        return true;
                }
                return false;
            }
            if (times.ContainsKey(newDuration.To) && times[newDuration.To] == true)
            {
                //return true;
                times.Remove(newDuration.To);
            }
            if (times.ContainsKey(newDuration.To) && times[newDuration.To] == false)
            {
                return false;
            }

            times.Add(newDuration.From, true);
            times.Add(newDuration.To, false);
            int from_index = times.IndexOfKey(newDuration.From);
            int to_index = times.IndexOfKey(newDuration.To);
            if (to_index - from_index != 1)
            {
                return false;
            }
            return true;
        }
    }
}
