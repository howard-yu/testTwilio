using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Twilio.AspNet.Core;
using Twilio.TwiML;
using static Twilio.TwiML.Voice.Say;
using System.Text;
using Twilio.TwiML.Voice;
using Microsoft.AspNetCore.Mvc.Routing;
using static Twilio.TwiML.Voice.Gather;

using Twilio.Http;
using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using Twilio.AspNet.Common;

namespace ConversationTemplate.Controllers
{
    public class STTController : TwilioController
    {
        [HttpPost]
        public IActionResult Index([FromBody] string SpeechResult)
        {
            var voiceResponse = new VoiceResponse();

            var language = Say.LanguageEnum.ZhTw;
            var voice = Say.VoiceEnum.Alice;

            Trace.Listeners.Add(new TextWriterTraceListener("SpeechResultOutput.log", "SpeechListener"));
            Trace.TraceInformation("SpeechResult: " + SpeechResult);
            Trace.Flush();
            //Trace.TraceInformation("SequenceNumber = " + SequenceNumber + ", UnstableSpeechResult = " + UnstableSpeechResult　+ ", SpeechResult = " + SpeechResult);
            voiceResponse.Say(SpeechResult, voice, null, language);

            return TwiML(voiceResponse);
        }
    }
}