using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HotelBooking.Data.Extensions
{
    public class TokenManagerMiddleware : IMiddleware
    {
        private readonly ITokenManager tokenManager;
        public TokenManagerMiddleware(ITokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await tokenManager.IsCurrentActiveToken())
            {
                await next(context);
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
