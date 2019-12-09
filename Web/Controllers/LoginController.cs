using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Web.Proxy;


namespace Web.Controllers
{
    public class LoginController : Controller
    {
       // GET: Login
        public ActionResult Index()
        {
            return View();
        }

   
        [HttpPost]
        public ActionResult Login(string userName, string password)
        {

            var token = TokenDetails(userName, password);

            return Json(token);
        }

        public Dictionary<string, string> TokenDetails(string userName, string password)
        {
            Dictionary<string, string> tokenDetails = null;
            try
            {
                using (var client = new HttpClient())
                {
                    var login = new Dictionary<string, string>
                   {
                       {"grant_type", "password"},
                       {"username", userName},
                       {"password", password},
                   };

                    var resp = client.PostAsync("http://localhost:52663/token", new FormUrlEncodedContent(login));
                    resp.Wait(TimeSpan.FromSeconds(10));

                    if (resp.IsCompleted)
                    {
                        if (resp.Result.Content.ReadAsStringAsync().Result.Contains("access_token"))
                        {
                            tokenDetails = JsonConvert.DeserializeObject<Dictionary<string, string>>(resp.Result.Content.ReadAsStringAsync().Result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return tokenDetails;
        }
    }
}
