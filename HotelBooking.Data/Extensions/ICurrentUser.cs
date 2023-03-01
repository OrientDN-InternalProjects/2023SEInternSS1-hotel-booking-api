namespace HotelBooking.Data.Extensions
{
    public interface ICurrentUser
    {
        string UserId { get; }
        string UserEmail { get; }
        bool IsAuthenicated { get; }
    }
}
