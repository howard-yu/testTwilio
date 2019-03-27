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
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace ConversationTemplate.Controllers
{
    public class VoiceTestController : TwilioController
    {
        private readonly ILogger _logger;

        public VoiceTestController(ILogger<VoiceTestController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public IActionResult Index()
        {
            var voiceResponse = new VoiceResponse();
            voiceResponse.Say("您好這邊是裕融企業", VoiceEnum.Alice, null, Say.LanguageEnum.ZhTw);

            var message = $"請問您是溫家宏先生嗎 請回答是或否謝謝";
            voiceResponse.Say(message, VoiceEnum.Alice, null, Say.LanguageEnum.ZhTw);

            Gather gather = new Gather(
                numDigits: 1,
                input: new List<InputEnum> { InputEnum.Speech },
                language: Twilio.TwiML.Voice.Gather.LanguageEnum.CmnHantTw,
                hints: "是,否",
                speechTimeout: "1",
                partialResultCallback: new Uri("https://conversationtemplate.azurewebsites.net/STT"),
                partialResultCallbackMethod: HttpMethod.Post,
                action: new Uri("https://conversationtemplate.azurewebsites.net/VoiceTest/Speechmessage"),
                method: HttpMethod.Post);

            voiceResponse.Append(gather);

            voiceResponse
                .Say("這是Gather的句子", VoiceEnum.Alice, null, Say.LanguageEnum.ZhTw);

            return TwiML(voiceResponse);
        }

        [HttpPost]
        public IActionResult Gather(string digits)
        {
            var response = new VoiceResponse();

            // If the user entered digits, process their request
            if (!string.IsNullOrEmpty(digits))
            {
                switch (digits)
                {
                    case "1":
                        response.Say("You selected sales. Good for you!");
                        break;
                    case "2":
                        response.Say("You need support. We will help!");
                        break;
                    default:
                        response.Say("Sorry, I don't understand that choice.").Pause();
                        response.Redirect(new Uri("https://conversationtemplate.azurewebsites.net/VoiceTest"));
                        break;
                }
            }
            else
            {
                // If no input was sent, redirect to the /voice route
                response.Redirect(new Uri("https://conversationtemplate.azurewebsites.net/VoiceTest"));
            }

            return TwiML(response);
        }

        public IActionResult RouteStep(string From, string To, string CallSid, string Digits)
        {
            var response = new VoiceResponse();

            switch (Digits)
            {
                case "1":
                    response.Say("5秒大聲說話", VoiceEnum.Alice, null, Say.LanguageEnum.ZhTw);
                    break;

                case "10":
                    response.Say("Digits is ten.");
                    break;
                default:
                    break;
            }
            return TwiML(response);
        }

        [HttpPost]
        public string Test(int SequenceNumber, string UnstableSpeechResult, string SpeechResult)
        {
            Trace.Listeners.Add(new TextWriterTraceListener("SpeechResultOutput.log", "SpeechListener"));
            Trace.TraceInformation(SpeechResult);
            Trace.Flush();
            return "OK";
        }

        public IActionResult Speechmessage(string SpeechResult)
        {
            var response = new VoiceResponse();
            var language = Say.LanguageEnum.ZhTw;

            //_logger.LogTrace(SpeechResult);
            //_logger.LogDebug(SpeechResult);
            _logger.LogInformation("Information  是否是本人 log SpeechResult ==========================================> " + SpeechResult);
            //_logger.LogWarning(SpeechResult);
            //_logger.LogError(SpeechResult);
            //_logger.LogCritical(SpeechResult);


            //string pattern = ("是");
            //Match match = Regex.Match(SpeechResult, pattern);
            //if (match.Success)

            switch (SpeechResult)
            {
                case "是":
                    response.Say("溫家宏先生您好", VoiceEnum.Alice, null, language);

                    Gather gather = new Gather(
                    input: new List<InputEnum> { InputEnum.Speech },
                    language: Twilio.TwiML.Voice.Gather.LanguageEnum.CmnHantTw,
                    hints: "是,否",
                    speechTimeout: "1",
                    partialResultCallback: new Uri("https://conversationtemplate.azurewebsites.net/STT"),
                    partialResultCallbackMethod: HttpMethod.Post,
                    action: new Uri("https://conversationtemplate.azurewebsites.net/VoiceTest/SpeechSecond"),
                    method: HttpMethod.Post).Say("請問您本期因繳款想以繳清了嗎 請回答是或否謝謝", VoiceEnum.Alice, null, language);

                    response.Append(gather);

                    break;

                case "否":
                    response.Say("很抱歉打擾您了", VoiceEnum.Alice, null, language);
                    break;

                default:
                    break;
            }


            response.Say("您好，這邊是裕融企業資訊中心，您聽到的是中文音訊測試", VoiceEnum.Alice, null, language);

            //Trace.Listeners.Add(new TextWriterTraceListener("SpeechResultOutput.log", "SpeechListener"));
            //Trace.TraceInformation(SpeechResult);
            //Trace.Flush();

            return TwiML(response);
        }

        public IActionResult SpeechSecond(string SpeechResult)
        {

            //Trace.TraceInformation("SpeechResult = " + SpeechResult);

            //_logger.LogTrace(SpeechResult);
            //_logger.LogDebug(SpeechResult);
            _logger.LogInformation("Information 是否繳款回覆 log SpeechResult ==========================================> " + SpeechResult);
            //_logger.LogWarning(SpeechResult);
            //_logger.LogError(SpeechResult);
            //_logger.LogCritical(SpeechResult);


            var response = new VoiceResponse();
            var language = Say.LanguageEnum.ZhTw;

            switch (SpeechResult)
            {
                case "是":
                    response.Say("謝謝您祝您順心", VoiceEnum.Alice, null, language);
                    break;

                case "否":
                    response.Say("請您競速繳款感謝", VoiceEnum.Alice, null, language);
                    break;

                default:
                    break;
            }

            response.Say("您好，這邊是裕融企業資訊中心，您聽到的是中文音訊測試", VoiceEnum.Alice, null, language);

            //Trace.Listeners.Add(new TextWriterTraceListener("SpeechResultOutput.log", "SpeechListener"));
            //Trace.TraceInformation(SpeechResult);
            //Trace.Flush();

            return TwiML(response);
        }
    }
}