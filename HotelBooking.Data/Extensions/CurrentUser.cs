using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HotelBooking.Data.Extensions
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        private HttpContext HttpContext => httpContextAccessor.HttpContext;
        public string UserId => HttpContext?.User?.FindFirstValue("Id");
        public string UserEmail => HttpContext?.User?.FindFirstValue("Email");
    }
}
