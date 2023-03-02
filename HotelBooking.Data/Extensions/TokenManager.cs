using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Model.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace HotelBooking.Data.Extensions
{
    public class TokenManager : ITokenManager
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly BookingDbContext bookingDbContext;
        public TokenManager(IHttpContextAccessor httpContextAccessor,
            BookingDbContext bookingDbContext)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.bookingDbContext = bookingDbContext;
        }

        public async Task<bool> IsCurrentActiveToken()
        => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentAsync()
        => await DeactivateAsync(GetCurrentAsync());
        
        public async Task DeactivateAsync(string token)
        {
            var item = new BlackList { Token = token };
            bookingDbContext.BlackLists.Add(item);
            await bookingDbContext.SaveChangesAsync();
        }

        public async Task<bool> IsActiveAsync(string token)
        {
            var item = await bookingDbContext.BlackLists.FirstOrDefaultAsync(x=>x.Token.Equals(token));
            if(item == null) { return true; }
            return false;
        }

        private string GetCurrentAsync()
        {
            var authorizationHeader = httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }
    }
}

