using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class Chatgptimple : Interfaces.Chatgptinterface
    {
        public PortalContext _SReportContext;


        public Chatgptimple(PortalContext DomainModelContext)
        {
            _SReportContext = DomainModelContext;

        }


        public async Task<ChatgptDTO> ChatGPT([FromBody]ChatgptDTO data)
        {
            string apiKey = "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";

            string endpoint = "https://api.openai.com/v1/chat/completions";

            string prompt = data.message;
            int maxTokens = 500; // Set the maximum number of tokens you want in the response

            string response = await CallChatGPTApi(apiKey, endpoint, prompt, maxTokens);

            data.ChatCompletion = response;

            return data;
        }


        static async Task<string> CallChatGPTApi(string apiKey, string endpoint, string prompt, int maxTokens)
        {

            float temperature = 0.5f;
            //string message = "";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                var requestContent = new StringContent($"{{\"messages\": [{{" +
                    $"\"role\": \"user\"," +
                    $"\"content\": \"{prompt}\"" +
                    $"}}], \"max_tokens\": {maxTokens},\"model\": \"gpt-3.5-turbo\",\"temperature\": {temperature}}}",
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync(endpoint, requestContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    JObject statusresult = JObject.Parse(responseContent.ToString());

                    string responseContent1 = statusresult.GetValue("choices").ToString();

                    string[] message = responseContent1.Split("content");
                    string smessage = message[1].Trim();

                    string[] messages = smessage.Split("finish_reason");

                    smessage = messages[0].Trim();

                    smessage = smessage.Replace("}", "");

                    smessage = smessage.Replace("\"", "");

                    smessage = smessage.Replace(":", "");

                    smessage = smessage.Replace("    ,", "");

                    smessage = smessage.Replace("\\n", "\x0A");

                    return smessage;
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
                }

            }

        }


        public class ApiParm
        {
            public string messaging_product { get; set; }
            public string to { get; set; }
            public string type { get; set; }
            public templete template { get; set; }
        }

        public class templete
        {
            public string name { get; set; }
            public language language { get; set; }
            public List<component> components { get; set; }
        }
        public class component
        {
            public string type { get; set; }
            public List<parameter> parameters { get; set; }
        }
        public class parameter
        {
            public string type { get; set; }
            public string text { get; set; }
        }
        public class language
        {
            public string code { get; set; }
        }

        public async Task<string> WhatsAppCall(ChatgptDTO data)
        {
            ApiParm parm = new ApiParm();

            parm.messaging_product = "whatsapp";
            parm.to = data.MobileNumber;
            parm.type = "template";

            //Welcome board

            if (data.Typedropdown == "Board")
            {


                //    List<parameter> Board = new List<parameter>
                //{

                //    new parameter
                //    {
                //        type = "text",
                //        text = data.Board
                //    }
                //};

                //    List<component> BoardBody = new List<component>
                //{
                //    new component
                //    {
                //        type = "body",
                //        parameters = Board
                //    },
                //};

                language a = new language();
                a.code = "en";

                templete BoardTemplate = new templete();
                BoardTemplate.name = "welcomevaps";
                BoardTemplate.language = a;
                //BoardTemplate.components = BoardBody;

                parm.template = BoardTemplate;
            }





            //Birthday
            else if (data.Typedropdown == "Birthday")
            {
                List<parameter> Birthday = new List<parameter>
            {

                new parameter
                {
                    type = "text",
                    text = data.BirthdayName
                },
                new parameter
                {
                    type = "text",
                    text = data.InstituteNameB
                },

            };

                List<component> BirthdayBody = new List<component>
            {
                new component
                {
                    type = "body",
                    parameters = Birthday
                },
            };

                language LanguageB = new language();
                LanguageB.code = "en";

                templete templateBirthday = new templete();
                templateBirthday.name = "birthdaynew";
                templateBirthday.language = LanguageB;
                templateBirthday.components = BirthdayBody;

                parm.template = templateBirthday;

            }

            else if (data.Typedropdown == "Attendance")
            {

                //Attendance


                List<parameter> Attendance = new List<parameter> {
                 new parameter
                {
                    type = "text",
                    text = data.Attendance1
                },
                new parameter
                {
                    type = "text",
                    text = data.Attendance2
                },
                    new parameter
                {
                    type = "text",
                    text = data.Attendance3
                },
                new parameter
                {
                    type = "text",
                    text = data.Attendance4
                },
                 new parameter
                 {
                    type = "text",
                    text = data.Attendance5
                }
            };

                List<component> AttendanceBody = new List<component>
            {
                new component
                {
                    type = "body",
                    parameters = Attendance
                },
            };

                language LanguageA = new language();
                LanguageA.code = "en";

                templete AttendanceTemplate = new templete();
                AttendanceTemplate.name = "attendance";
                AttendanceTemplate.language = LanguageA;
                AttendanceTemplate.components = AttendanceBody;

                parm.template = AttendanceTemplate;
            }
            else if (data.Typedropdown == "FeeDue")
            {
                //feeDue

                List<parameter> FeeDue = new List<parameter>
            {

                new parameter
                {
                    type = "text",
                    text = data.FeeDue
                }
            };

                List<component> feeDueBody = new List<component>
            {
                new component
                {
                    type = "body",
                    parameters = FeeDue
                },
            };

                language LanguageFee = new language();
                LanguageFee.code = "en";

                templete FeeDueTemplate = new templete();
                FeeDueTemplate.name = "feedefault";
                FeeDueTemplate.language = LanguageFee;
                FeeDueTemplate.components = feeDueBody;

                parm.template = FeeDueTemplate;
            }
            else if (data.Typedropdown == "FeeTransaction")
            {
                //feeTransaction

                List<parameter> feeTransaction = new List<parameter> {
                 new parameter
                {
                    type = "text",
                    text = data.FeeTransaction1
                },
                new parameter
                {
                    type = "text",
                    text = data.FeeTransaction2
                },
                    new parameter
                {
                    type = "text",
                    text = data.FeeTransaction3
                },
                new parameter
                {
                    type = "text",
                    text = data.FeeTransaction4
                },
                 new parameter
                 {
                    type = "text",
                    text = data.FeeTransaction5
                }
            };


                List<component> feeTransactionBody = new List<component>
            {
                new component
                {
                    type = "body",
                    parameters = feeTransaction
                },
            };
                language languageFee = new language();
                languageFee.code = "en";

                templete feetransactionTemplate = new templete();
                feetransactionTemplate.name = "feetransaction";
                feetransactionTemplate.language = languageFee;
                feetransactionTemplate.components = feeTransactionBody;

                parm.template = feetransactionTemplate;
            }
            else
            {
                //LoginCredentials

                //list<parameter> logincredentials = new list<parameter> {
                List<parameter> logincredentials = new List<parameter> {
                 new parameter
                {
                    type = "text",
                    text = data.LoginCredentials1
                },
                new parameter
                {
                    type = "text",
                    text = data.LoginCredentials2
                },
                    new parameter
                {
                    type = "text",
                    text = data.LoginCredentials3
                },
                new parameter
                {
                    type = "text",
                    text = data.LoginCredentials4
                }
            };


                //     list<component> logincredentialsbody = new list<component>
                List<component> logincredentialsbody = new List<component>
            {
                new component
                {
                    type = "body",
                    parameters = logincredentials
                },
            };


                language languagel = new language();
                languagel.code = "en";

                templete administratorbody = new templete();
                administratorbody.name = "administrator";
                administratorbody.language = languagel;
                administratorbody.components = logincredentialsbody;
                parm.template = administratorbody;
            }

            var client = new HttpClient();
            var jsonContent = JsonConvert.SerializeObject(parm);
            var requestnew = new HttpRequestMessage
            {
                Method = System.Net.Http.HttpMethod.Post,
                RequestUri = new Uri("https://graph.facebook.com/v17.0/177948302058854/messages?access_token=EAALIaEGktRgBOxmK05MMqsi9jKwo4ZBSzps3ZAWckqVLvDMe0LZCSPZCDHEUkrsGjuKkR43wKzvBVZC4XnPNrESHgZC9ZADLVZAHLAAZCDZCY7mhy6hdqwt2FRTTAdjUUqrqNUrwasJwdOBEeXzFGDonc5ri5KZCts64szUfrhQ1kc0RLIdeQWjYZAx4Gi2SupDgg3aP2e8xsSfMt4QVVj7u"),
                Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
            };
            using (var response123 = await client.SendAsync(requestnew))
            {
                response123.EnsureSuccessStatusCode();
                var body = await response123.Content.ReadAsStringAsync();
                Console.WriteLine(body);
                JObject joResponse1 = JObject.Parse(body);
            }
            return "success";
        }

       

    }

   


}
