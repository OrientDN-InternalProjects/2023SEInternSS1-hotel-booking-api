using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelBooking.Data.Authenication;
using HotelBooking.Data.DTOs.Account;
using Microsoft.AspNetCore.Identity;

namespace HotelBooking.Data.Interfaces
{
    public interface IAccountRepository
    {
        Task<AuthenicationModel> RegisterAsync(RegisterDTO model);
        Task<AuthenicationModel> LoginAsync(LoginDTO model);
    }
}
