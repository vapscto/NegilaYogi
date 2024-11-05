using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using System.IO;
using System.Text;
using PreadmissionDTOs;
using System.Net;
using corewebapi18072016.Delegates.com.vapstech.admission;

namespace corewebapi18072016.com.vaps.admission.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SendingSMSandMAILSController : Controller
    {
        SendingSMSandMailsDelegate _SendingSMSandMails = new SendingSMSandMailsDelegate();

        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public CommonDTO Get([FromQuery] int id)
        {
            return _SendingSMSandMails.getdetails(id);
        }
        [HttpPost]
        [Route("getdetailsstudentorstaff/")]
        public CommonDTO getdetailsstudentorstaff([FromBody] CommonDTO data)
        {

            data.IVRM_MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _SendingSMSandMails.getdetailsstudentorstaff(data);
        }
        [HttpPost]
        [Route("msgsend/")]
        public async Task<string> msgsend([FromBody] CommonDTO reg)
        {
            string str = "false";
            try
            {

                if (reg.msgtype == "sms")
                {
                    for (int i = 0; i < reg.bulkmailsNmobilenos.Length; i++)
                    {
                        long mobileno = Convert.ToInt64(reg.bulkmailsNmobilenos[i].smsmobileno);
                        string messageText = reg.msgcontent.ToString();
                        string s = await sendSms("A038a673a697f74890c57b94e7b6e9552", "VAPSTE", mobileno, messageText);
                    }
                    str = "true";
                }
                else if (reg.msgtype == "mail")
                {
                    for (int i = 0; i < reg.bulkmailsNmobilenos.Length; i++)
                    {
                        string mailid = reg.bulkmailsNmobilenos[i].sendmailid.ToString();
                        string messageText = reg.msgcontent.ToString();
                        string mailsubject = reg.mailsubject.ToString();
                        Mail(messageText, mailid, mailsubject);
                    }
                    str = "true";
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return str;
        }

        public async Task<string> sendSms(string workingKey, string sender, long mobileNo, string message)
        {
            try
            {
                System.Net.HttpWebRequest request = System.Net.WebRequest.Create("http://trans.kapsystem.com/api/web2sms.php?&workingkey=" + workingKey + "&sender=" + sender + "&to=" + mobileNo + "&message=" + message) as HttpWebRequest;
                //optional
                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                Stream stream = response.GetResponseStream();
                return "success";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void Mail(string MailText, string MailId, string subject)
        {
            string body = this.PopulateBody(MailText);
            this.SendHtmlFormattedEmail(MailId, body, subject);
        }

        private string PopulateBody(string MailText)
        {
            string body = string.Empty;
            string[] res = System.IO.File.ReadAllLines(@"..\corewebapi18072016\wwwroot\EmailTemplate\ManualReportEmail.html");
            string str = string.Join("", res);
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            System.IO.MemoryStream stream = new MemoryStream(byteArray);
            using (StreamReader reader = new StreamReader(stream))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{MailContent}", MailText);
            return body;
        }

        private void SendHtmlFormattedEmail(string recepientEmail, string body, string subject)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Vapstech", "vapstech123@gmail.com"));
            message.To.Add(new MailboxAddress("", recepientEmail));
            message.Subject = subject;
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("vapstech123@gmail.com", "vaps@123");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
