using HotelBooking.Data;
using HotelBooking.Model.Entities;
using System.Text.Json;

namespace HotelBooking.Test
{
    public static class Extensions
    {
        private static object _customerContextLock = new object();
        public static BookingDbContext InitializeTestDatabase(this BookingDbContext context)
        {
            lock (_customerContextLock)
            {
                if (!context.Addresses.Any())
                {
                    context.Addresses.Add(new Address
                    {
                        Id = new Guid("ce146157-b904-48f8-ad08-7d2202dddca1"),
                        City = "Da Nang",
                        PinCode = "12345",
                        StreetNumber = "54 NLB",
                        District = "LC",
                        Building = "SB"
                    });

                    context.Addresses.Add(new Address
                    {
                        Id = new Guid("12a57dcf-1927-48a3-8871-8efc50e1e4e9"),
                        City = "HCM city",
                        PinCode = "56789",
                        StreetNumber = "138 HVT",
                        District = "LC",
                        Building = "SB"
                    });

                    context.SaveChanges();
                }
                return context;
            }
        }

        public static async Task<T> DeserializeContent<T>(this HttpResponseMessage message) =>
           await JsonSerializer.DeserializeAsync<T>(await message.Content.ReadAsStreamAsync().ConfigureAwait(false),
           new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).ConfigureAwait(false);
    }
}