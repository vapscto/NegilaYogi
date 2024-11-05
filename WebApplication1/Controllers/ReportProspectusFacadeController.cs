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
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using CommonLibrary;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]

    public class ReportProspectusFacadeController : Controller
    {

        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;

        string Is_middlename_null;
        string Is_lastname_null;


        public ReportProspectusFacadeController(DomainModelMsSqlServerContext db, ApplicationDBContext ApplicationDBContext)
        {

            _db = db;
            _ApplicationDBContext = ApplicationDBContext;
        }

        [Route("Get_Intial_data/")]
        public WrittenTestMarksBindDataDTO Get_Intial_data([FromBody] WrittenTestMarksBindDataDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        // POST api/values
        [HttpPost]
        [Route("Getdetails/")]

        public async Task<WrittenTestMarksBindDataDTO> Getdetails([FromBody] WrittenTestMarksBindDataDTO reg)
        {
            List<WrittenTestMarksBindDataDTO> main_result = new List<WrittenTestMarksBindDataDTO>();

            // Student Roles
            string studentRole = "OnlinePreadmissionUser";
            var id = _db.applicationRole.Single(d => d.Name == studentRole);
            //

            // Student Role Type
            string studentRoleType = "OnlinePreadmissionUser";
            var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
            //


            if(reg.From_Date==null || reg.From_Date=="")
            {
                reg.From_Date = DateTime.Now.ToString();
            }
            if (reg.To_Date == null || reg.To_Date == "")
            {
                reg.To_Date = DateTime.Now.ToString();
            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ReportProspectus_modified";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@year",
                             SqlDbType.VarChar)
                {
                    Value = reg.ASMAY_Id
                });

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
                cmd.Parameters.Add(new SqlParameter("@miid",
          SqlDbType.VarChar)
                {
                    Value = reg.MI_Id
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
                            if (dataReader["PASP_Middle_Name"] != System.DBNull.Value)
                            {
                                Is_middlename_null = dataReader["PASP_Middle_Name"].ToString();

                            }
                            else
                            {
                                Is_middlename_null = "Not Available";
                            }


                            if (dataReader["PASP_Last_Name"] != System.DBNull.Value)
                            {
                                Is_lastname_null = dataReader["PASP_Last_Name"].ToString();

                            }
                            else
                            {
                                Is_lastname_null = "Not Available";
                            }


                            main_result.Add(new WrittenTestMarksBindDataDTO
                            {
                                PASP_FirstName = dataReader["PASP_First_Name"].ToString(),
                                PASP_MiddleName = Is_middlename_null,
                                PASP_LastName = Is_lastname_null,
                                PASP_emailid = dataReader["PASP_EmailId"].ToString(),
                                PASP_MobileNo = Convert.ToInt64(dataReader["PASP_MobileNo"].ToString()),
                                PASP_Phone = Convert.ToInt64(dataReader["PASP_PhoneNo"].ToString()),
                                PASP_Date = Convert.ToDateTime(dataReader["PASP_Date"]),
                                state = dataReader["IVRMMS_Name"].ToString(),
                                PASP_ProspectusNo = dataReader["PASP_ProspectusNo"].ToString(),
                            });
                            reg.SearchstudentDetails = main_result.ToArray();
                        }

                    }

                }



                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return reg;
            }
        }




        [Route("SendSms/")]

        public async Task<string> SendSms([FromBody] WrittenTestMarksBindDataDTO reg)
        {
            string str = "false";

            for (int i = 0; i < reg.SmsMailStudentDetails.Length; i++)
            {
                int Id = Convert.ToInt16(reg.SmsMailStudentDetails[i].Id);

                long mobileNumber;
                mobileNumber = reg.PASP_MobileNo;
                //var allusers = _ApplicationDBContext.applicationUser.Where(d => d.Id.Equals(Id)).ToArray();


                //try
                //{
                //     mobileNumber = allusers[0].PhoneNumber.ToString();
                //}
                //catch (Exception ex)
                //{

                //    mobileNumber = "9026034321";
                //}

                //Array SearchstudentDetails = allusers.Where(d => d.Id.Equals(Id)).ToArray();

                //string s = await sendSms("Af7660f900c9df85ac5dfae4b055d75bd", "HHSJCP", 8884747430, reg.SmsMailText);

                SMS sms = new SMS(_db);

                string smsdet = await sms.sendSms(reg.MI_Id, Convert.ToInt64(mobileNumber), "ProspectusReport", reg.Id);

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

                //var allusers = _ApplicationDBContext.applicationUser.Where(d => d.Id.Equals(Id)).ToArray();

                string MailText = reg.SmsMailText.ToString();

                string MailId = reg.PASP_emailid;

                Email Email = new Email(_db);

                string m = Email.sendmail(reg.MI_Id, MailId, "ProspectusReport", reg.Id);

                //try
                //{
                //    MailId = allusers[0].NormalizedEmail.ToString();
                //}
                //catch (Exception ex)
                //{
                //    //Console.WriteLine(ex.Message);

                //    MailId = "vapsmail@gmail.com";
                //}

                //Array SearchstudentDetails = allusers.Where(d => d.Id.Equals(Id)).ToArray();

                //Mail(MailText, MailId);

            }

            return str;
        }

        protected void Mail(string MailText, string MailId)
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
            var message = new SendGridMessage();
            message.From = new EmailAddress("vapstech123@gmail.com", "Vapstech");
            message.Subject = "Prospectus Report";
            message.AddTo(recepientEmail);
            message.HtmlContent = body;
            var client = new SendGridClient("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");
            client.SendEmailAsync(message).Wait();




            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Vapstech", "vapstech123@gmail.com"));
            //message.To.Add(new MailboxAddress("", recepientEmail));
            //message.Subject = "subject";
            //var bodyBuilder = new BodyBuilder();
            //bodyBuilder.HtmlBody = body;
            //message.Body = bodyBuilder.ToMessageBody();
            //using (var client = new SmtpClient())
            //{
            //    client.Connect("smtp.gmail.com", 587);

            //    client.AuthenticationMechanisms.Remove("XOAUTH2");

            //    client.Authenticate("vapstech123@gmail.com", "vaps@123");
            //    client.Send(message);
            //    client.Disconnect(true);
            //}
        }

        [Route("ExportToExcle/")]

        public string ExportToExcle([FromBody] WrittenTestMarksBindDataDTO reg)
        {
            string str = "";



            return str;
        }

        [Route("searchprospectus/")]
        public async Task<WrittenTestMarksBindDataDTO> searchprospectus([FromBody] WrittenTestMarksBindDataDTO acd)
        {
            List<WrittenTestMarksBindDataDTO> result = new List<WrittenTestMarksBindDataDTO>();

            string studentRole = "OnlinePreadmissionUser";
            var id = _db.applicationRole.Single(d => d.Name == studentRole);
            //

            // Student Role Type
            string studentRoleType = "OnlinePreadmissionUser";
            var id2 = _db.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
            //

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ReportProspectus_modified";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@year",
                             SqlDbType.VarChar)
                {
                    Value = acd.ASMAY_Id
                });
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
               SqlDbType.VarChar)
                {
                    Value = acd.From_Date
                });
                cmd.Parameters.Add(new SqlParameter("@To_Date",
               SqlDbType.VarChar)
                {
                    Value = acd.To_Date
                });


                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();



                try
                {


                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            if (dataReader["PASP_Middle_Name"] != System.DBNull.Value)
                            {
                                Is_middlename_null = dataReader["PASP_Middle_Name"].ToString();

                            }
                            else
                            {
                                Is_middlename_null = "Not Available";
                            }


                            if (dataReader["PASP_Last_Name"] != System.DBNull.Value)
                            {
                                Is_lastname_null = dataReader["PASP_Last_Name"].ToString();

                            }
                            else
                            {
                                Is_lastname_null = "Not Available";
                            }





                            result.Add(new WrittenTestMarksBindDataDTO
                            {
                                PASP_FirstName = dataReader["PASP_First_Name"].ToString(),
                                PASP_MiddleName = Is_middlename_null,
                                PASP_LastName = Is_lastname_null,
                                PASP_emailid = dataReader["PASP_EmailId"].ToString(),
                                PASP_MobileNo = Convert.ToInt64(dataReader["PASP_MobileNo"].ToString()),
                                PASP_Phone = Convert.ToInt64(dataReader["PASP_PhoneNo"].ToString()),
                                PASP_Date = Convert.ToDateTime(dataReader["PASP_Date"]),

                            });
                            acd.SearchstudentDetails = result.ToArray();
                        }
                        switch (acd.searchType)
                        {

                            case "all":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).ToArray();

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }








                                else
                                {
                                    acd.count = 0;
                                }
                                break;






                            case "first_name":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_FirstName.Contains(acd.searchString)).ToArray();

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "middle_name":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_MiddleName == acd.searchString).ToArray();

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "last_name":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_LastName == acd.searchString).ToArray();

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "email_id":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_emailid == acd.searchString).ToArray();


                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "mobile_no":

                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => Convert.ToString(w.PASP_MobileNo) == acd.searchString).ToArray();

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "phone_no":
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => Convert.ToString(w.PASP_Phone) == acd.searchString).ToArray();

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;
                            case "date_search":
                                DateTime date = DateTime.ParseExact(acd.searchString, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                                acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w => w.PASP_Date.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd"))).ToArray();
                                // acd.SearchstudentDetails = result.OrderByDescending(d => d.UpdatedDate).Where(w =>Convert.ToString( w.PASP_Date) == acd.searchString).ToArray();

                                if (acd.SearchstudentDetails.Length > 0)
                                {
                                    acd.count = acd.SearchstudentDetails.Length;
                                }
                                else
                                {
                                    acd.count = 0;
                                }
                                break;


                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return acd;
            }
        }
    }
}
