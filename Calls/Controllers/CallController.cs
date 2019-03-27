using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.AspNet.Core;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

using Microsoft.Extensions.Configuration;

namespace Calls.Controllers
{
    public class CallController : TwilioController
    {
        private readonly IConfiguration _config;

        public CallController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            var accountSid = _config["TwilioSetting:accountSid"];
            var authToken = _config["TwilioSetting:authToken"];

            var toPhoneNumber = _config["TwilioSetting:to"];
            var fromPhoneNumber = _config["TwilioSetting:from"];

            TwilioClient.Init(accountSid, authToken);
            // TwilioClient.Init(accountSidTest, authTokenTest);

            // 請填入正確的電話號碼
            var to = new PhoneNumber(toPhoneNumber);

            var from = new PhoneNumber(fromPhoneNumber);

            var url = $"https://conversationtemplate.azurewebsites.net/VoiceTest";
            // var url = $"https://almond-vulture-8773.twil.io/facts";

            var call = CallResource.Create(
                to: to,
                from: from,
                url: new Uri(url)
               );

            //var call = IncomingPhoneNumberResource.Create(
            //    phoneNumber: from,
            //    voiceUrl: new Uri("http://demo.twilio.com/docs/voice.xml")
            //    );
            return Content(call.Sid);
        }
    }
}