namespace HotelBooking.Data.Authenication
{
    public class AuthenicationModel
    {
        public string Message { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public bool IsSuccess { get; set; }
    }
}
