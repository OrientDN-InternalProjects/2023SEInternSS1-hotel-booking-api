using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Data.ViewModel;
using HotelBooking.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data.Repositories
{
    public class IBookedRoomRepository : GenericRepository<BookedRoom>, IBookedRoom
    {
        public IBookedRoomRepository(BookingDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckValidation(DurationVM newDuration, Guid roomId)
        {
            var returnObjects = await GetAll().Where(x => x.RoomId.Equals(roomId)).Select(x => new DurationVM { From = x.From, To = x.To }).ToListAsync();
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
            if (times.ContainsKey(newDuration.To))
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

        public void CreateAsync(BookedRoom model)
        {
            Add(model);
        }

        public void DeleteAsync(BookedRoom model)
        {
            Delete(model);
        }

        public async Task<IEnumerable<BookedRoom>> GetAllAsync()
        {
            return await GetAll().OrderBy(br => br.Id).Where(br => br.IsDeleted == false).ToListAsync();
        }
        public async Task<BookedRoom> GetByIdAsync(Guid id)
        {
            return await GetByCondition(br => br.Id.Equals(id) && br.IsDeleted == false).FirstOrDefaultAsync();
        }

        public void UpdateAsync(BookedRoom model)
        {
            Update(model);
        }

    }
}
