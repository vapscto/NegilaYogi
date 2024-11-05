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
using CommonLibrary;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]

    public class TotalCountReportFacadeController : Controller
    {

        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;

        public DomainModelMsSqlServerContext _context;

        public TotalCountReportFacadeController(DomainModelMsSqlServerContext db, ApplicationDBContext ApplicationDBContext, DomainModelMsSqlServerContext context)
        {

            _db = db;
            _ApplicationDBContext = ApplicationDBContext;
            _context = context;
        }

        [Route("Get_Intial_data/")]
        public async Task<WrittenTestMarksBindDataDTO> Get_Intial_data([FromBody] WrittenTestMarksBindDataDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();


                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _db.admissioncls.ToList();
                data.fillclass = classname.Where(t => t.MI_Id == data.MI_Id).ToArray();


                var Acdemic_preadmission = _db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                data.ASMAY_Id = Acdemic_preadmission;

                DateTime startdate = Convert.ToDateTime(_db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_PreAdm_F_Date).FirstOrDefault());
                data.prestartdate = startdate;
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
                    cmd.CommandText = "totalcountReport";
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
                    cmd.Parameters.Add(new SqlParameter("@year",
                SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@predate",
         SqlDbType.DateTime)
                    {
                        Value = data.prestartdate
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //  var data = cmd.ExecuteNonQuery();

                    try
                    {
                        //   var data = cmd.ExecuteNonQuery();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                //for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                //{
                                //    dataRow.Add(
                                //        dataReader.GetName(iFiled),
                                //        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                //    );
                                //}

                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? " " : dateval  // use null instead of {}
                                    );
                                    }
                                    else
                                    {
                                        dataRow.Add(
                                       dataReader.GetName(iFiled),
                                       dataReader.IsDBNull(iFiled) ? " " : dataReader[iFiled] // use null instead of {}
                                   );
                                    }
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.totalcountDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }

                }

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

            DateTime enddate = DateTime.Now;
            if (reg.ASMAY_Id!=0)
            {
                DateTime startdate = Convert.ToDateTime(_db.AcademicYear.Where(t => t.ASMAY_Id == reg.ASMAY_Id && t.MI_Id == reg.MI_Id).Select(d => d.ASMAY_PreAdm_F_Date).FirstOrDefault());
                reg.prestartdate = startdate;
                var ASMAY_orderid = (from a in _db.AcademicYear
                                     where (a.ASMAY_Id == reg.ASMAY_Id && a.MI_Id == reg.MI_Id)
                                     select new WrittenTestMarksBindDataDTO
                                     {
                                         academicorder = a.ASMAY_Order + 1
                                     }
          ).ToList();
                var academicorderid = (from a in _db.AcademicYear
                                       where (a.ASMAY_Order == ASMAY_orderid.FirstOrDefault().academicorder && a.MI_Id== reg.MI_Id)
                                       select new WrittenTestMarksBindDataDTO
                                       {
                                           academicyearstratdate = a.ASMAY_PreAdm_F_Date,
                                           acedemicyear = a.ASMAY_Id

                                       }
          ).ToList();


                if (academicorderid.Count > 0)
                {
                    enddate = Convert.ToDateTime(_db.AcademicYear.Where(t => t.ASMAY_Id == academicorderid.FirstOrDefault().acedemicyear && t.MI_Id == reg.MI_Id).Select(d => d.ASMAY_PreAdm_T_Date).FirstOrDefault());
                    reg.presenddate = enddate.AddDays(-1);
                }
                else
                {
                    reg.presenddate = enddate;
                }

            }
            else
            {
                reg.prestartdate = enddate;
                reg.presenddate = enddate;
            }



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
                cmd.CommandText = "AllInOneReport";
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
                cmd.Parameters.Add(new SqlParameter("@option",
              SqlDbType.TinyInt)
                {
                    Value = reg.ReportType
                });
                cmd.Parameters.Add(new SqlParameter("@year",
            SqlDbType.BigInt)
                {
                    Value = reg.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@type",
          SqlDbType.VarChar)
                {
                    Value = reg.type
                });
                cmd.Parameters.Add(new SqlParameter("@miid",
         SqlDbType.VarChar)
                {
                    Value = reg.MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@predate",
         SqlDbType.DateTime)
                {
                    Value = reg.prestartdate
                });
                cmd.Parameters.Add(new SqlParameter("@prenddate",
          SqlDbType.DateTime)
                {
                    Value = reg.presenddate
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
          SqlDbType.BigInt)
                {
                    Value = reg.ASMCL_Id
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
                            //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            //for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            //{
                            //    dataRow.Add(
                            //        dataReader.GetName(iFiled),
                            //        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                            //    );
                            //}

                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                var datatype = dataReader.GetFieldType(iFiled);
                                if (datatype.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? " " : dateval  // use null instead of {}
                                );
                                }
                                else
                                {
                                    dataRow.Add(
                                   dataReader.GetName(iFiled),
                                   dataReader.IsDBNull(iFiled) ? " " : dataReader[iFiled] // use null instead of {}
                               );
                                }
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    reg.SearchstudentDetails = retObject.ToArray();
                    if (reg.SearchstudentDetails.Length > 0)
                    {
                        reg.count = reg.SearchstudentDetails.Length;
                    }
                    else
                    {
                        reg.count = 0;
                    }
                }
                catch (Exception ex)
                {

                }

            }
            return reg;
        }

        [Route("Getdetailsmobiles/")]

        public async Task<WrittenTestMarksBindDataDTO> Getdetailsmobile([FromBody] WrittenTestMarksBindDataDTO reg)
        {

            DateTime enddate = DateTime.Now;
            if (reg.ASMAY_Id != 0)
            {

                var Acdemic_preadmission = _db.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == reg.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                reg.ASMAY_Id = Acdemic_preadmission;



                DateTime startdate = Convert.ToDateTime(_db.AcademicYear.Where(t => t.ASMAY_Id == reg.ASMAY_Id && t.MI_Id == reg.MI_Id).Select(d => d.ASMAY_PreAdm_F_Date).FirstOrDefault());
                reg.prestartdate = startdate;
                var ASMAY_orderid = (from a in _db.AcademicYear
                                     where (a.ASMAY_Id == reg.ASMAY_Id && a.MI_Id == reg.MI_Id)
                                     select new WrittenTestMarksBindDataDTO
                                     {
                                         academicorder = a.ASMAY_Order + 1
                                     }
          ).ToList();
                var academicorderid = (from a in _db.AcademicYear
                                       where (a.ASMAY_Order == ASMAY_orderid.FirstOrDefault().academicorder && a.MI_Id == reg.MI_Id)
                                       select new WrittenTestMarksBindDataDTO
                                       {
                                           academicyearstratdate = a.ASMAY_PreAdm_F_Date,
                                           acedemicyear = a.ASMAY_Id

                                       }
          ).ToList();


                if (academicorderid.Count > 0)
                {
                    enddate = Convert.ToDateTime(_db.AcademicYear.Where(t => t.ASMAY_Id == academicorderid.FirstOrDefault().acedemicyear && t.MI_Id == reg.MI_Id).Select(d => d.ASMAY_PreAdm_F_Date).FirstOrDefault());
                    reg.presenddate = enddate.AddDays(-1);
                }
                else
                {
                    reg.presenddate = enddate;
                }

            }
            else
            {
                reg.prestartdate = enddate;
                reg.presenddate = enddate;
            }



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
                cmd.CommandText = "AllInOneReport";
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
                cmd.Parameters.Add(new SqlParameter("@option",
              SqlDbType.TinyInt)
                {
                    Value = reg.ReportType
                });
                cmd.Parameters.Add(new SqlParameter("@year",
            SqlDbType.BigInt)
                {
                    Value = reg.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@type",
          SqlDbType.VarChar)
                {
                    Value = reg.type
                });
                cmd.Parameters.Add(new SqlParameter("@miid",
         SqlDbType.VarChar)
                {
                    Value = reg.MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@predate",
         SqlDbType.DateTime)
                {
                    Value = reg.prestartdate
                });
                cmd.Parameters.Add(new SqlParameter("@prenddate",
          SqlDbType.DateTime)
                {
                    Value = reg.presenddate
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
          SqlDbType.BigInt)
                {
                    Value = reg.ASMCL_Id
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
                            //var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            //for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            //{
                            //    dataRow.Add(
                            //        dataReader.GetName(iFiled),
                            //        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                            //    );
                            //}

                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                            {
                                var datatype = dataReader.GetFieldType(iFiled);
                                if (datatype.Name == "DateTime")
                                {
                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? " " : dateval  // use null instead of {}
                                );
                                }
                                else
                                {
                                    dataRow.Add(
                                   dataReader.GetName(iFiled),
                                   dataReader.IsDBNull(iFiled) ? " " : dataReader[iFiled] // use null instead of {}
                               );
                                }
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    reg.SearchstudentDetails = retObject.ToArray();
                    if (reg.SearchstudentDetails.Length > 0)
                    {
                        reg.count = reg.SearchstudentDetails.Length;
                    }
                    else
                    {
                        reg.count = 0;
                    }
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

            for (int i = 0; i < reg.SmsMailStudentDetailst.Length; i++)
            {
                int Id = Convert.ToInt16(reg.SmsMailStudentDetailst[i].Id);


                var allusers = _ApplicationDBContext.applicationUser.Where(d => d.Id.Equals(Id)).ToArray();

                string smsText = reg.SmsMailText.ToString();

                long mobile = Convert.ToInt64(reg.SmsMailStudentDetailst[i].PhoneNumber);





                string s = await sendSms("A038a673a697f74890c57b94e7b6e9552", "VAPSTE", mobile, smsText);

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

            for (int i = 0; i < reg.SmsMailStudentDetailst.Length; i++)
            {
                int Id = Convert.ToInt16(reg.SmsMailStudentDetailst[i].Id);


                var allusers = _ApplicationDBContext.applicationUser.Where(d => d.Id.Equals(Id)).ToArray();

                string MailText = reg.SmsMailText.ToString();

                string MailId = reg.SmsMailStudentDetailst[i].Email;

                // Email Email = new Email(_context);

                //string m = Email.sendmail(reg.MI_Id, MailId, "StudentCount", reg.Id);

                //Array SearchstudentDetails = allusers.Where(d => d.Id.Equals(Id)).ToArray();

                long mid = reg.MI_Id;
                Mail(MailText, MailId, mid);

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
            alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(mid) && t.IVRMMD_NAME == "Total count").ToList();

            var institute = _db.Institute.Where(t => t.MI_Id == mid).ToList();

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
