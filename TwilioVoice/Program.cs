using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace TwilioVoice
{
    class Program
    {
        static void Main(string[] args)
        {
            // Find your Account Sid and Auth Token at twilio.com/console
            const string accountSid = "AC181a0c8fa874b60bfd55fd4f52858e47";
            const string authToken = "74e0b7b6e82373b48845dab946169b5a";
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber("+886914020765");
            var from = new PhoneNumber("+886255941177");
            var call = CallResource.Create(to, from,
                url: new Uri("http://demo.twilio.com/docs/voice.xml"));
            Console.WriteLine(call.Sid);



        }
    }
}
