using HotelBooking.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Data
{
    public class BookingDbContext : IdentityDbContext<User>
    {
        public BookingDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Image> ImgUrls { get; set; }
        public DbSet<PriceQuotation> PricesQuotation { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoomDiscount> RoomsDiscount { get; set; }
        public DbSet<RoomFacility> RoomsFacility { get; set; }
        public DbSet<RoomService> RoomsService { get; set; }
        public DbSet<Duration> RoomsDuration { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
