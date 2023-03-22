using HotelBooking.Common.Models;
using HotelBooking.Data;
using HotelBooking.Data.DataSeeder;
using HotelBooking.Data.Extensions;
using HotelBooking.Data.Infrastructure;
using HotelBooking.Data.Interfaces;
using HotelBooking.Data.Repositories;
using HotelBooking.Data.Repository;
using HotelBooking.Model.Entities;
using HotelBooking.Service.IServices;
using HotelBooking.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var mailKitOptions = builder.Configuration.GetSection("MailSettings").Get<NETCore.MailKit.Infrastructure.Internal.MailKitOptions>();
builder.Services.Configure<ZaloOptions>(builder.Configuration.GetSection("Zalopay"));
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", policy =>
    {
        policy.AllowAnyHeader().AllowAnyOrigin();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Booking Hotel API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<TokenManagerMiddleware>();
builder.Services.AddScoped<ITokenManager,TokenManager>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Indentity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<BookingDbContext>().AddDefaultTokenProviders();

//Add Jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateIssuerSigningKey = true
    };
});

// Config MailKit

builder.Services.AddMailKit(optionBuilder =>
{
    optionBuilder.UseMailKit(new MailKitOptions()
    {
        Server = mailKitOptions.Server,
        Port = mailKitOptions.Port,
        SenderName = mailKitOptions.SenderName,
        SenderEmail = mailKitOptions.SenderEmail,
        Account = mailKitOptions.Account,
        Password = mailKitOptions.Password,
        Security = true
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IDataSeeder, DataSeeder>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IMailSender, MailSender>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IPictureRepository, PictureRepository>();
builder.Services.AddScoped<IPictureService, PictureService>();
builder.Services.AddScoped<IPriceQuotationRepository, PriceQuotationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomServiceRepository>();
builder.Services.AddScoped<IRoomFacility, RoomFacilityRepository>();
builder.Services.AddScoped<IServiceHotelRepository, ServiceHotelRepository>();
builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookedRoom, IBookedRoomRepository>();
builder.Services.AddScoped<ICheckDurationValidationService, CheckDurationValidationService>();
builder.Services.AddScoped<IRoomPicture, RoomPicture>();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<BookingDbContext>(options =>
    options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("cors");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseMiddleware<TokenManagerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.InitDb();

app.Run();
