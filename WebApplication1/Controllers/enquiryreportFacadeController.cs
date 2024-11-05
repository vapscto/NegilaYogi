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
using WebApplication1.Interfaces;
using CommonLibrary;
using System.Collections;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]

    public class enquiryreportFacadeController : Controller
    {

        //private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;
        enquiryreportInterface _enq;

        public DomainModelMsSqlServerContext _context;

        string Is_middlename_null;
        string Is_lastname_null;
        string Is_address2_null;
        string Is_address3_null;
        string Is_enquiry_details_null;
        string Is_enquiry_number_null;
        int Is_Id_null = 0;
        //public enquiryreportController(enquiryreportInterface enqui)
        //{
        //    _enq = enqui;
        //}

        public enquiryreportFacadeController(DomainModelMsSqlServerContext context, ApplicationDBContext ApplicationDBContext, enquiryreportInterface enqui)
        {
            _context = context;
            _ApplicationDBContext = ApplicationDBContext;
            _enq = enqui;
           
        }


        [Route("getenqyearlist")]
        public WrittenTestMarksBindDataDTO getenqyearlist([FromBody]WrittenTestMarksBindDataDTO data)
        {
            return _enq.getenqyearlist(data);
        }

        // POST api/values
        //[HttpPost]
        //[Route("getEnquirySearchedDetails")]
        //public async Task<Enq> getEnquirySearchedDetails([FromBody] SortingPagingInfoDTO Ins)
        //{
        //    return await _enq.getenqSearchedDetails(Ins);
        //}
        [Route("Getdetails/")]

        public async Task<WrittenTestMarksBindDataDTO> Getdetails([FromBody] WrittenTestMarksBindDataDTO reg)
        {
            List<Enq> result = new List<Enq>();
            string studentRole = "OnlinePreadmissionUser";
            var id = _context.applicationRole.Single(d => d.Name == studentRole);
            //

            // Student Role Type
            string studentRoleType = "OnlinePreadmissionUser";
            var id2 = _context.MasterRoleType.Single(d => d.IVRMRT_Role == studentRoleType);
            //

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "EnquiryReport_modified";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@year",
                             SqlDbType.Int)
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
               SqlDbType.VarChar)
                {
                    Value = reg.From_Date
                });
                cmd.Parameters.Add(new SqlParameter("@To_Date",
               SqlDbType.VarChar)
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

                            if (dataReader["PASE_MiddleName"] != System.DBNull.Value)
                            {
                                Is_middlename_null = dataReader["PASE_MiddleName"].ToString();

                            }
                            else
                            {
                                Is_middlename_null = "Not Available";
                            }


                            if (dataReader["PASE_LastName"] != System.DBNull.Value)
                            {
                                Is_lastname_null = dataReader["PASE_LastName"].ToString();

                            }
                            else
                            {
                                Is_lastname_null = "Not Available";
                            }



                            if (dataReader["PASE_Address2"] != System.DBNull.Value)
                            {
                                Is_address2_null = dataReader["PASE_Address2"].ToString();

                            }
                            else
                            {
                                Is_address2_null = "Not Available";
                            }


                            if (dataReader["PASE_Address3"] != System.DBNull.Value)
                            {
                                Is_address3_null = dataReader["PASE_Address3"].ToString();

                            }
                            else
                            {
                                Is_address3_null = "Not Available";
                            }

                            if (dataReader["PASE_EnquiryDetails"] != System.DBNull.Value)
                            {
                                Is_enquiry_details_null = dataReader["PASE_EnquiryDetails"].ToString();

                            }
                            else
                            {
                                Is_enquiry_details_null = "Not Available";
                            }

                            if (dataReader["PASE_EnquiryNo"] != System.DBNull.Value)
                            {
                                Is_enquiry_number_null = dataReader["PASE_EnquiryNo"].ToString();

                            }
                            else
                            {
                                Is_enquiry_number_null = "Not Available";
                            }

                            if (dataReader["Id"] != System.DBNull.Value)
                            {
                                Is_Id_null = Convert.ToInt32(dataReader["Id"]);

                            }
                            else
                            {
                                Is_Id_null = 0;
                            }



                            result.Add(new Enq
                            {

                                UserName = dataReader["UserName"].ToString(),
                                PASE_emailid = dataReader["PASE_emailid"].ToString(),
                                PASE_MobileNo = Convert.ToInt64(dataReader["PASE_MobileNo"]),
                                ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                                PASE_Date = Convert.ToDateTime(dataReader["PASE_Date"]),
                                PASE_EnquiryNo = Is_enquiry_number_null,
                                PASE_FirstName = dataReader["PASE_FirstName"].ToString(),
                                PASE_MiddleName = Is_middlename_null,
                                PASE_LastName = Is_lastname_null,
                                PASE_Address1 = dataReader["PASE_Address1"].ToString(),
                                PASE_Address2 = Is_address2_null,
                                PASE_Address3 = Is_address3_null,
                                PASE_EnquiryDetails = Is_enquiry_details_null,
                                Id = Is_Id_null,

                            });
                            reg.SearchstudentDetails = result.ToArray();
                        }
                    }


                }
                catch (Exception ex)
                {

                }

            }

            return reg;
        }



        //Added on 13-feb-2016 by sudeep

        [Route("searchenquiry")]
        public async Task<WrittenTestMarksBindDataDTO> searchenquiry([FromBody] WrittenTestMarksBindDataDTO dto)
        {
            return await _enq.searchenquiry(dto);
        }

        [Route("SendSms/")]

        public async Task<string> SendSms([FromBody] WrittenTestMarksBindDataDTO reg)
        {
            string str = "false";

            for (int i = 0; i < reg.SmsMailStudentDetails.Length; i++)
            {
                int Id = Convert.ToInt32(reg.SmsMailStudentDetails[i].Id);

                // var allusers = _ApplicationDBContext.applicationUser.Where(d => d.Id.Equals(Id)).ToArray();

                long mobileNumber = reg.SmsMailStudentDetails[i].PASE_MobileNo;

                //Array SearchstudentDetails = allusers.Where(d => d.Id.Equals(Id)).ToArray();

              //  string s = await sendSms("A038a673a697f74890c57b94e7b6e9552", "VAPSTE", mobileNumber, reg.SmsMailText);


                SMS sms = new SMS(_context);
                string Smsmessage = reg.SmsMailText;
                await sms.sendSms(reg.MI_Id, mobileNumber, reg.SmsMailStudentDetails[i].Id, "Enquery", Smsmessage);
             
           




                // SMS sms = new SMS(_context);
                // string smsdet = await sms.sendSms(reg.MI_Id, Convert.ToInt64(mobileNumber), "EnquiryReport", reg.Id);
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
                int Id = Convert.ToInt32(reg.SmsMailStudentDetails[i].Id);


                // var allusers = _ApplicationDBContext.applicationUser.Where(d => d.Id.Equals(Id)).ToArray();

                string MailText = reg.SmsMailText.ToString();

                string MailId = reg.SmsMailStudentDetails[i].PASE_emailid;

                //Array SearchstudentDetails = allusers.Where(d => d.Id.Equals(Id)).ToArray();
                long mid = reg.MI_Id;
                Mail(MailText, MailId, mid);

                //Email Email = new Email(_context);

                //string m = Email.sendmail(reg.MI_Id, MailId, "EnquiryReport", reg.Id);
            }

            return str;
        }

        protected void Mail(string MailText, string MailId, long mid)
        {
            //  string body = this.PopulateBody(MailText);
            this.SendHtmlFormattedEmail(MailId, MailText, mid);
        }

        //private string PopulateBody(string MailText)
        //{
        //    string body = string.Empty;
        //    string[] res = System.IO.File.ReadAllLines(@"..\corewebapi18072016\wwwroot\EmailTemplate\ManualReportEmail.html");
        //    string str = string.Join("", res);
        //    byte[] byteArray = Encoding.UTF8.GetBytes(str);
        //    System.IO.MemoryStream stream = new MemoryStream(byteArray);

        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        body = reader.ReadToEnd();
        //    }
        //    body = body.Replace("{MailContent}", MailText);
        //    return body;
        //}

        private void SendHtmlFormattedEmail(string recepientEmail, string MailText, long mid)
        {
            string SendingEmail = "";
            string SendingEmailPassword = "";
            List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
            alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(mid) && t.IVRMMD_NAME == "Enquiry").ToList();
            var institute = _context.Institute.Where(t => t.MI_Id == mid).ToList();
            if (alldetails.Count > 0)
            {
                SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
            }



            var message = new SendGridMessage();
            message.From = new EmailAddress(SendingEmail, institute.FirstOrDefault().MI_Name);
            message.Subject = alldetails.FirstOrDefault().IVRMMD_SUBJECT;
            message.AddTo(recepientEmail);
            message.HtmlContent = MailText;
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

    }
}
