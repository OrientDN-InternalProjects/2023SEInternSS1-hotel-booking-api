using HotelBooking.Data.Crypto;
using HotelBooking.Data.DTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HotelBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        public PaymentController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            string appid = "2554";
            string key1 = "sdngKKJmqEMzvh5QQcdD2A9XBSKUNaYn";
            string createOrderUrl = "https://sandbox.zalopay.com.vn/v001/tpe/createorder";
            var transid = Guid.NewGuid().ToString();
            var embeddata = new
            {
                merchantinfo = "embeddata123",
                redirecturl = "https://localhost:7137/api/Payment/return-url"
            };
            var items = new[]{
                new { itemid = "knb", itemname = "kim nguyen bao", itemprice = 198400, itemquantity = 1 }
                };

            var param = new Dictionary<string, string>();

            param.Add("appid", appid);
            param.Add("appuser", "demo");
            param.Add("apptime", Utils.GetTimeStamp().ToString());
            param.Add("amount", "10000");
            param.Add("apptransid", DateTime.Now.ToString("yyMMdd") + "_" + transid); // mã giao dich có định dạng yyMMdd_xxxx
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
        public IActionResult PostZaloPay([FromQuery] PaymentRequest data)
        {
            string key2 = "trMrHtvjo6myautxDUiAcYsVtaeQ8nhf";
            var result = new Dictionary<string, object>();

            try
            {
                //var checksumData = data.appid + "|" + data.apptransid + "|" + data["pmcid"] + "|" +
                //data["bankcode"] + "|" + data["amount"] + "|" + data["discountamount"] + "|" + data["status"];
                //var checksum = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key2, checksumData);

                //var dataStr = Convert.ToString(cbdata["data"]);
                //var reqMac = Convert.ToString(cbdata["mac"]);

                //var mac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, key2, dataStr);

                //Console.WriteLine("mac = {0}", mac);

                //// kiểm tra callback hợp lệ (đến từ ZaloPay server)
                //if (!reqMac.Equals(mac))
                //{
                //    // callback không hợp lệ
                //    result["returncode"] = -1;
                //    result["returnmessage"] = "mac not equal";
                //}
                //else
                //{
                //    // thanh toán thành công
                //    // merchant cập nhật trạng thái cho đơn hàng
                //    var dataJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(dataStr);
                //    Console.WriteLine("update order's status = success where apptransid = {0}", dataJson["apptransid"]);

                //    result["returncode"] = 1;
                //    result["returnmessage"] = "success";
                //}
            }
            catch (Exception ex)
            {
                result["returncode"] = 0; // ZaloPay server sẽ callback lại (tối đa 3 lần)
                result["returnmessage"] = ex.Message;
            }

            // thông báo kết quả cho ZaloPay server
            return Ok(result);
        }
    }
}
