namespace HotelBooking.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}
