using System.Net;

namespace HotelBooking.Common.Base
{
    public class ResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
        public string Errors { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
