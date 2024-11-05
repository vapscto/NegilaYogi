using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Net;



namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]

    public class enquiryreportController : Controller
    {

        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;


        public enquiryreportController(DomainModelMsSqlServerContext db, ApplicationDBContext ApplicationDBContext)
        {

            _db = db;
            _ApplicationDBContext = ApplicationDBContext;
        }


        // POST api/values
        [HttpPost]
        [Route("Getdetails/")]

        public async Task<WrittenTestMarksBindDataDTO> Getdetails([FromBody] WrittenTestMarksBindDataDTO reg)
        {


            // Student Roles
            string studentRole = "OnlinePreadmissionUser";
            var id = _db.applicationRole.Single(d => d.Name == studentRole);
            //

            // Student Role Type
            string studentRoleType = "OnlinePreadmissionUser";
            var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
            //

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "EnquiryReport";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@RoleId",
                    SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt32(id.Id)
                });
                cmd.Parameters.Add(new SqlParameter("@RoleTypeId",
                   SqlDbType.TinyInt)
                {
                    Value = Convert.ToInt64(id2.IVRMRT_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@From_Date",
               SqlDbType.DateTime)
                {
                    Value = reg.From_Date
                });
                cmd.Parameters.Add(new SqlParameter("@To_Date",
               SqlDbType.DateTime)
                {
                    Value = reg.To_Date
                });


                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                //var data = cmd.ExecuteNonQuery();

                try
                {
                    // var data = cmd.ExecuteNonQuery();

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    reg.SearchstudentDetails = retObject.ToArray();
                }
                catch (Exception ex)
                {

                }
            
                }

            return reg;
        }

        [Route("SendSms/")]

        public async Task<string> SendSms([FromBody] WrittenTestMarksBindDataDTO reg)
        {
            string str = "false";

            for (int i = 0; i < reg.SmsMailStudentDetails.Length; i++)
            {
                int Id = Convert.ToInt16(reg.SmsMailStudentDetails[i].Id);


                var allusers = _ApplicationDBContext.applicationUser.Where(d => d.Id.Equals(Id)).ToArray();

                string mobileNumber = allusers[0].PhoneNumber.ToString();

                //Array SearchstudentDetails = allusers.Where(d => d.Id.Equals(Id)).ToArray();

                string s = await sendSms("Af7660f900c9df85ac5dfae4b055d75bd", "HHSJCP", 8884747430, reg.SmsMailText);

            }

            return str;
        }

        public async Task<string> sendSms(string workingKey, string sender, long mobileNo, string message)
        {
            System.Net.HttpWebRequest request = System.Net.WebRequest.Create("http://trans.kapsystem.com/api/web2sms.php?&workingkey=" + workingKey + "&sender=" + sender + "&to=" + mobileNo + "&message=" + message) as HttpWebRequest;
            //optional
            System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
            Stream stream = response.GetResponseStream();
            return "success";
        }

        [Route("SendMail/")]

        public string SendMail([FromBody] WrittenTestMarksBindDataDTO reg)
        {
            string str = "false";

            for (int i = 0; i < reg.SmsMailStudentDetails.Length; i++)
            {
                int Id = Convert.ToInt16(reg.SmsMailStudentDetails[i].Id);


                var allusers = _ApplicationDBContext.applicationUser.Where(d => d.Id.Equals(Id)).ToArray();

                string MailText = reg.SmsMailText.ToString();

                string MailId = allusers[0].NormalizedEmail.ToString();

                //Array SearchstudentDetails = allusers.Where(d => d.Id.Equals(Id)).ToArray();

                Mail(MailText, MailId);

            }

            return str;
        }

        protected void Mail(string MailText,string MailId)
        {
            string body = this.PopulateBody(MailText);
            this.SendHtmlFormattedEmail(MailId, body);
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

        private void SendHtmlFormattedEmail(string recepientEmail, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Vapstech", "vapstech123@gmail.com"));
            message.To.Add(new MailboxAddress("", recepientEmail));
            message.Subject = "subject";
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

        [Route("ExportToExcle/")]

        public string ExportToExcle([FromBody] WrittenTestMarksBindDataDTO reg)
        {
            string str = "";



            return str;
        }
    }
}
