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

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationReportFacadeController : Controller
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;
        string Is_middlename_null;
        string Is_lastname_null;
        public RegistrationReportFacadeController(DomainModelMsSqlServerContext db, ApplicationDBContext ApplicationDBContext)
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


                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _db.admissioncls.ToList();
                data.fillclass = classname.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToArray();

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
                cmd.CommandText = "RegistrationReport";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@From_Date", SqlDbType.DateTime) { Value = reg.From_Date });
                cmd.Parameters.Add(new SqlParameter("@To_Date", SqlDbType.DateTime) { Value = reg.To_Date });
                cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt) { Value = reg.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = reg.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@psdate", SqlDbType.DateTime) { Value = reg.prestartdate });
                cmd.Parameters.Add(new SqlParameter("@prenddate", SqlDbType.DateTime) { Value = reg.presenddate });
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                var retObject = new List<dynamic>();

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
                    Console.WriteLine(ex.Message);
                }
                return reg;
            }
        }
        [Route("Getdetailsforpre/")]
        public async Task<WrittenTestMarksBindDataDTO> Getdetailsforpre([FromBody] WrittenTestMarksBindDataDTO reg)
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

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "RegistrationReportPreuser";
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.BigInt) { Value = reg.ASMAY_Id });
                cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = reg.MI_Id });
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                var retObject = new List<dynamic>();

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

                //string mobileNumber ="";
                //try
                //{
                //     mobileNumber = allusers[0].PhoneNumber.ToString();
                //}
                //catch (Exception ex)
                //{

                //   mobileNumber = "9026034321";
                //}


                // SMS sms = new SMS(_db);

                //string smsdet = await sms.sendSms(reg.MI_Id, Convert.ToInt64(mobileNumber), "RegistrationReport", reg.Id);

                //Array SearchstudentDetails = allusers.Where(d => d.Id.Equals(Id)).ToArray();

                string s = await sendSms("A038a673a697f74890c57b94e7b6e9552", "VAPSTE", mobileNumber, reg.SmsMailText);

            }

            return str;
        }
        [Route("avtivedeactive/")]
        public async Task<string> avtivedeactive([FromBody] WrittenTestMarksBindDataDTO reg)
        {          

            for (int i = 0; i < reg.data_array.Length; i++)
            {
                int Id = Convert.ToInt32(reg.data_array[i].Id);

                var result2 = _db.UserRoleWithInstituteDMO.Single(t => t.Id == Id);
                if (reg.count == 1)
                {
                    result2.Activeflag = 1;
                    result2.UpdatedDate = DateTime.Now;
                    _db.Update(result2);
                    _db.SaveChanges();
                    reg.success = "success";
                }
                else
                {
                    result2.Activeflag = 0;
                    result2.UpdatedDate = DateTime.Now;
                    _db.Update(result2);
                    _db.SaveChanges();
                    reg.success = "success";
                }
            }
            return reg.success;
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

                string MailId = reg.PASP_emailid;

                // Email Email = new Email(_db);

                // string m = Email.sendmail(reg.MI_Id, MailId, "RegistrationReport", reg.Id);

                //string MailId ="";

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

                Mail(MailText, MailId);

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
        [Route("smssend")]
        public async Task<WrittenTestMarksBindDataDTO> smssend([FromBody]WrittenTestMarksBindDataDTO data)
        {
            SMS sms = new SMS(_db);

            for (var i = 0; i < data.data_array.Length; i++)
            {
                if (data.data_array[i].Mob_No != 0)
                {
                    data.success = sms.sendSmsreg(data.MI_Id, data.data_array[i].Mob_No, "PREUSER", data.data_array[i].Id, "Password@123").Result;
                        // data.success =  sms.sendSms(data.MI_Id, data.data_array[i].mobilenumber, "ADMINISTRATOR", data.data_array[i].);
                }
            }
            return data;
        }
        [Route("emailsend")]
        public WrittenTestMarksBindDataDTO emailsend([FromBody]WrittenTestMarksBindDataDTO data)
        {
            Email Email = new Email(_db);
            for (var i = 0; i < data.data_array.Length; i++)
            {
                if (data.data_array[i].Email_Id != "null" && data.data_array[i].Email_Id != null && data.data_array[i].Email_Id != "")
                {
                    data.success = Email.sendmailreg(data.MI_Id, data.data_array[i].Email_Id, "PREUSER", data.data_array[i].Id, "Password@123");
                }
            }
            return data;
        }
    }
}
