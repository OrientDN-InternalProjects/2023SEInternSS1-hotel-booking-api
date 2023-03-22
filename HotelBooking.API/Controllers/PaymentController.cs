using HotelBooking.Common.Models;
using HotelBooking.Data.Crypto;
using HotelBooking.Data.DTOs;
using HotelBooking.Data.Extensions;
using HotelBooking.Service.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IBookingService bookingService;
        private readonly ILogger<PaymentController> logger;
        private readonly IOptions<ZaloOptions> zaloOptions;
        private readonly ICurrentUser currentUser;
        public PaymentController(IBookingService bookingService, ILogger<PaymentController> logger, 
            ICurrentUser currentUser, IOptions<ZaloOptions> zaloOptions)
        {
            this.bookingService = bookingService;
            this.logger = logger;
            this.currentUser = currentUser;
            this.zaloOptions = zaloOptions;
        }

        [HttpGet]
        public async Task<IActionResult> Payment(Guid id)
        {
            var booking = await bookingService.GetBookingById(id);
            var appid = "2554";
            var key1 = zaloOptions.Value.Key1;
            var createOrderUrl = "https://sandbox.zalopay.com.vn/v001/tpe/createorder";
            var transid = Guid.NewGuid().ToString();
            var embeddata = new
            {
                merchantinfo = "haiyencute123@",
                redirecturl = $"https://localhost:7137/api/Payment/return-url?id={id}"
            };
            var items = new[]{new { id = booking.Id.ToString()}};
            var param = new Dictionary<string, string>();

            param.Add("appid", appid);
            param.Add("appuser", currentUser.UserEmail);
            param.Add("apptime", Utils.GetTimeStamp().ToString());
            param.Add("amount", booking.Amount.ToString());
            param.Add("apptransid", DateTime.Now.ToString("yyMMdd") + "_" + transid);
            param.Add("embeddata", JsonConvert.SerializeObject(embeddata));
            param.Add("item", JsonConvert.SerializeObject(items));
            param.Add("description", "ZaloPay demo");
            param.Add("bankcode", "zalopayapp");

            var data = appid + "|" + param["apptransid"] + "|" + param["appuser"] + "|" + param["amount"] + "|"
                + param["apptime"] + "|" + param["embeddata"] + "|" + param["item"];
            param.Add("mac", HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key1, data));

            var result = await HttpHelper.PostFormAsync(createOrderUrl, param);

            foreach (var entry in result)
            {
                Console.WriteLine("{0} = {1}", entry.Key, entry.Value);
            }
            return Ok(result);
        }

        [HttpGet("return-url")]
        public async Task<IActionResult> ReturnFromZaloPay([FromQuery] PaymentRequest data)
        {
            string key2 = zaloOptions.Value.Key2;
            var result = new Dictionary<string, object>();
            var checksumData = data.appid + "|" + data.apptransid + "|" + data.pmcid + "|" +
                data.bankcode + "|" + data.amount + "|" + data.discountamount + "|" + data.status;
            var checksum = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key2, checksumData);
            if (!checksum.Equals(data.checksum))
            {
                return BadRequest("Request is not valid");
            }
            else
            {
                if (int.Parse(data.status) != 1) return BadRequest("Payment process failed!");
                await bookingService.UpdatePaymentStatus(new Guid(data.id));
                return Ok(data);
            }
        }
    }
}
