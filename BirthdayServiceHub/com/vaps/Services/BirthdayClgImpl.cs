using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Birthday;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.BirthDay;
using PreadmissionDTOs.com.vaps.College.BirthDay;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BirthdayServiceHub.com.vaps.Services
{
    public class BirthdayClgImpl : Interfaces.BirthdayClgInterface
    {
        int MI_ID = 0;
        private static ConcurrentDictionary<string, BirthDayDTO> _login =
          new ConcurrentDictionary<string, BirthDayDTO>();

        public DomainModelMsSqlServerContext _db;
        public ClgBirthdayContext _ClgBirthdayContext;
        public ClgAdmissionContext _ClgadmContext;

        public BirthdayClgImpl(DomainModelMsSqlServerContext db, ClgBirthdayContext clgbdContext, ClgAdmissionContext ClgadmContext)
        {
            _db = db;
            _ClgBirthdayContext = clgbdContext;
            _ClgadmContext = ClgadmContext;
        }


        public ClgBirthDayDTO getloaddata(ClgBirthDayDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ClgBirthDayDTO> radiochange(ClgBirthDayDTO data)
        {
            try
            {
                using (var cmd = _ClgBirthdayContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_BirthdayList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@typeflag",
             SqlDbType.VarChar)
                    {
                        Value = data.typeflag
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
                        data.birthdaylist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ClgBirthDayDTO> sendmsg(ClgBirthDayDTO data)
        {
            try
            {
                string s = "";
                string m = "";
                if (data.typeflag == "Student" || data.typeflag == "Alumni")
                {
                    if (data.SMSFlag == true && data.EmailFlag == false)
                    {
                        foreach (var x in data.selectedarray)
                        {
                            SMS sms = new SMS(_db);
                            s = await sms.sendSms(data.MI_Id, x.AMCST_MobileNo, "CLGBIRTHDAY", x.AMCST_Id);
                        }
                        if (s == "success")
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else if (data.EmailFlag == true && data.SMSFlag == false)
                    {
                        foreach (var x in data.selectedarray)
                        {
                            Email Email = new Email(_db);
                            m = Email.sendmail(data.MI_Id, x.AMCST_emailId, "CLGBIRTHDAY", x.AMCST_Id);
                        }
                        if (m == "success")
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else if (data.SMSFlag == true && data.EmailFlag == true)
                    {
                        foreach (var x in data.selectedarray)
                        {
                            SMS sms = new SMS(_db);
                            s = await sms.sendSms(data.MI_Id, x.AMCST_MobileNo, "CLGBIRTHDAY", x.AMCST_Id);

                            Email Email = new Email(_db);
                            m = Email.sendmail(data.MI_Id, x.AMCST_emailId, "CLGBIRTHDAY", x.AMCST_Id);
                        }
                        if (s == "success" && m == "success")
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.typeflag == "Staff")
                {
                    if (data.SMSFlag == true && data.EmailFlag == false)
                    {
                        foreach (var x in data.selectedarray)
                        {
                            SMS sms = new SMS(_db);
                            s = await sms.sendSms(data.MI_Id, x.HRME_MobileNo, "CLGBIRTHDAYSTAFF", x.HRME_Id);
                        }
                        if (s == "success")
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else if (data.EmailFlag == true && data.SMSFlag == false)
                    {
                        foreach (var x in data.selectedarray)
                        {
                            Email Email = new Email(_db);
                            m = Email.sendmail(data.MI_Id, x.HRME_EmailId, "CLGBIRTHDAYSTAFF", x.HRME_Id);
                        }
                        if (m == "success")
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else if (data.SMSFlag == true && data.EmailFlag == true)
                    {
                        foreach (var x in data.selectedarray)
                        {
                            SMS sms = new SMS(_db);
                            s = await sms.sendSms(data.MI_Id, x.HRME_MobileNo, "CLGBIRTHDAYSTAFF", x.HRME_Id);

                            Email Email = new Email(_db);
                            m = Email.sendmail(data.MI_Id, x.HRME_EmailId, "CLGBIRTHDAYSTAFF", x.HRME_Id);
                        }
                        if (s == "success" && m == "success")
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgBirthDayDTO staflist(ClgBirthDayDTO data)
        {
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                if (data.typeflag.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    data.studentlist = (from a in _ClgadmContext.Adm_Master_College_StudentDMO
                                        from b in _ClgadmContext.Adm_College_Yearly_StudentDMO
                                        from c in _ClgadmContext.MasterCourseDMO
                                        from d in _ClgadmContext.ClgMasterBranchDMO
                                        from e in _ClgadmContext.CLG_Adm_Master_SemesterDMO
                                        from f in _ClgadmContext.Adm_College_Master_SectionDMO
                                        where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.ASMAY_Id == acd_Id && b.AMB_Id == d.AMB_Id && a.MI_Id == data.MI_Id && b.AMSE_Id == e.AMSE_Id && b.ACMS_Id == f.ACMS_Id && a.AMCST_ActiveFlag == true && a.AMCST_SOL.Equals("S") && b.ACYST_ActiveFlag == 1 && a.AMCST_DOB.Value.Date.Day == DateTime.Now.Day && a.AMCST_DOB.Value.Date.Month == DateTime.Now.Month)

                                        //group a by a.HRME_Id into g
                                        select new ClgBirthDayDTO
                                        {
                                            AMST_Id = a.AMCST_Id,
                                            studentname = a.AMCST_FirstName,
                                            AMST_emailId = a.AMCST_emailId,
                                            ASMCL_ClassName = c.AMCO_CourseName,
                                            ASMC_SectionName = d.AMB_BranchName,
                                            monthname = (string.IsNullOrEmpty(a.AMCST_StudentPhoto) ? "" : a.AMCST_StudentPhoto)

                                        }
                                 ).ToArray();
                    if (data.studentlist.Length > 0)
                    {
                        data.count = data.studentlist.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                }
                else if (data.typeflag.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.staffList = (from a in _db.HR_Master_Employee_DMO
                                      from b in _db.Multiple_Email_DMO
                                      from c in _db.Multiple_Mobile_DMO
                                      where (a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && b.HRMEM_DeFaultFlag.Equals("default") && c.HRMEMNO_DeFaultFlag.Equals("default") && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_DOB.Value.Date.Day == DateTime.Now.Day && a.HRME_DOB.Value.Date.Month == DateTime.Now.Month && a.HRME_LeftFlag == false)
                                      //group a by a.HRME_Id into g
                                      select new BirthDayDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                          HRME_EmailId = b.HRMEM_EmailId == null || b.HRMEM_EmailId == "" ? "" : b.HRMEM_EmailId,
                                          HRME_MobileNo = c.HRMEMNO_MobileNo == 0 || c.HRMEMNO_MobileNo == null ? 0 : c.HRMEMNO_MobileNo,
                                          HRME_DOB = a.HRME_DOB,
                                          PhotoPath = (string.IsNullOrEmpty(a.HRME_Photo) ? "" : a.HRME_Photo)
                                      }
                                 ).ToArray();
                    if (data.staffList.Length > 0)
                    {
                        data.count = data.staffList.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public void clg_getBirthday(int id)
        {
            try
            {
                MI_ID = id;

                SendEmail(MI_ID);
                Check_Mail_Staff(MI_ID);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

        }

        public string SendEmail(int MI_ID)
        {
            string re = "";
            long trnsno = 0;
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(MI_ID) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var que1 = (from a in _ClgadmContext.Adm_Master_College_StudentDMO
                            from b in _ClgadmContext.Adm_College_Yearly_StudentDMO
                            from c in _ClgadmContext.MasterCourseDMO
                            from d in _ClgadmContext.ClgMasterBranchDMO
                            from e in _ClgadmContext.CLG_Adm_Master_SemesterDMO
                            from f in _ClgadmContext.Adm_College_Master_SectionDMO
                            where (a.AMCST_Id == b.AMCST_Id && b.AMCO_Id == c.AMCO_Id && b.ASMAY_Id == acd_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ACMS_Id == f.ACMS_Id && a.MI_Id == MI_ID && a.AMCST_ActiveFlag == true && a.AMCST_SOL.Equals("S") && b.ACYST_ActiveFlag == 1 && a.AMCST_DOB.Value.Date.Day == DateTime.Now.Day && a.AMCST_DOB.Value.Date.Month == DateTime.Now.Month)
                            select new { AMST_Id = a.AMCST_Id, AMST_emailId = a.AMCST_emailId, AMST_MobileNo = a.AMCST_MobileNo }).ToList();

                if (que1.Count > 0)
                {

                    // SMS NEW TABLES CODE END
                    for (int i = 0; i < que1.Count; i++)
                    {
                        try
                        {
                            long id = que1[i].AMST_Id;
                            string email = que1[i].AMST_emailId.ToString();
                            string mobileNo = que1[i].AMST_MobileNo.ToString();
                            // string email = "amanullah@vapstech.com";
                            // string mobileNo = "9771237044";
                            string Template = "CLGBIRTHDAY";
                            string type = "Student";

                            string val = sendmail(MI_ID, email, Template, id, type);
                            string x = sendSms(MI_ID, mobileNo, Template, id, type);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return re;
        }

        public string Check_Mail_Staff(int MI_ID)
        {
            string ret = "";
            long trnsno = 0;
            try
            {


                var que2 = (from a in _db.HR_Master_Employee_DMO
                            from b in _db.Multiple_Email_DMO
                            from c in _db.Multiple_Mobile_DMO
                            where (a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && b.HRMEM_DeFaultFlag.Equals("default") && c.HRMEMNO_DeFaultFlag.Equals("default") && a.MI_Id == MI_ID && a.HRME_ActiveFlag == true && a.HRME_DOB.Value.Date.Day == DateTime.Now.Day && a.HRME_DOB.Value.Date.Month == DateTime.Now.Month && a.HRME_LeftFlag == false)
                            //group a by a.HRME_Id into g
                            select new BirthDayDTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmailId = b.HRMEM_EmailId,
                                HRME_MobileNo = c.HRMEMNO_MobileNo
                            }).ToList();

                if (que2.Count > 0)
                {
                    //  SMS NEW TABLES CODE START


                    SMS smstransno = new SMS(_db);
                    //  public async Task<string> sendSmsnew(long MI_Id, long mobileNo, string Template, long UserID, string sms)
                    //  trnsno = smstransno.getsmsno(MI_ID);

                    //string studentempflag = "STUDENT";

                    // SMS NEW TABLES CODE END
                    for (int i = 0; i < que2.Count; i++)
                    {
                        try
                        {
                            long id = que2[i].HRME_Id;
                            string email = que2[i].HRME_EmailId.ToString();
                            string mob = que2[i].HRME_MobileNo.ToString();
                            string Template = "CLGBIRTHDAY";
                            string type = "Employee";
                            // string val = sendmail(MI_ID, email, Template, id, type, trnsno);
                            // string x = sendSms(MI_ID, mob, Template, id, type, trnsno);
                            string val = sendmail(MI_ID, email, Template, id, type);
                            string x = sendSms(MI_ID, mob, Template, id, type);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ret;
        }

        public string sendmail(long MI_Id, string Email, string Template, long UserID, string type)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                string result = "";
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "COLLEGE_BIRTHDAY_PARAMETER";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                    {
                        Value = type
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }
                                    else
                                    {
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                }


                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))

                        {
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailmsg = result;
                        }
                    }
                }

                Mailmsg = result;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string mailcc = alldetails[0].IVRM_mailcc;

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }

                        }
                    }

                    //Email = "amanrce@gmail.com";
                    message.AddTo(Email);

                    string name = "";
                    string date1 = "";
                    if (type.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                    {
                        var query1 = _db.HR_Master_Employee_DMO.Where(d => d.HRME_Id == UserID).Select(d => new HR_Master_Employee_DMO { HRME_EmployeeFirstName = d.HRME_EmployeeFirstName, HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName, HRME_EmployeeLastName = d.HRME_EmployeeLastName }).ToList();
                        name = query1.FirstOrDefault().HRME_EmployeeFirstName + (string.IsNullOrEmpty(query1.FirstOrDefault().HRME_EmployeeMiddleName) ? "" : ' ' + query1.FirstOrDefault().HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(query1.FirstOrDefault().HRME_EmployeeLastName) ? "" : ' ' + query1.FirstOrDefault().HRME_EmployeeLastName);
                        date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    }
                    else if (type.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {
                        var query1 = _ClgadmContext.Adm_Master_College_StudentDMO.Single(d => d.AMCST_Id == UserID);
                        name = query1.AMCST_FirstName + (string.IsNullOrEmpty(query1.AMCST_MiddleName) ? "" : ' ' + query1.AMCST_MiddleName) + (string.IsNullOrEmpty(query1.AMCST_LastName) ? "" : ' ' + query1.AMCST_LastName);
                        date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    }
                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {

                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bname\b", name,
                            RegexOptions.IgnoreCase);

                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);


                    }
                    else
                    {

                        message.HtmlContent = "HAPPY BIRTHDAY DEAR" + " " + name + "<br/> No Template Found";
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                        //string status=client.DeliverAsync(message).Status.ToString();
                    }
                    else
                    {
                        return "Sendgrid key is not available";
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "CLG_IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                        {
                            Value = type
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public string sendSms(long MI_Id, string mobileNo, string Template, long UserID, string type)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = "";

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {


                    cmd.CommandText = "COLLEGE_BIRTHDAY_PARAMETER";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });
                    cmd.Parameters.Add(new SqlParameter("@template",
                       SqlDbType.VarChar)
                    {
                        Value = Template
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                      SqlDbType.VarChar)
                    {
                        Value = type
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                    var datatype = dataReader.GetFieldType(iFiled);
                                    if (datatype.Name == "DateTime")
                                    {
                                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                    }
                                    else
                                    {
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }

                }

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            sms = result;
                        }
                    }
                }

                sms = result;
                sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();
                    // PHNO = "9771237044";
                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);
                    url = url.Replace("entity_id", insdeta[0].MI_EntityId.ToString());
                    url = url.Replace("template_id", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "College Birthday";
                    dmo2.To_FLag = type;
                    dmo2.UpdatedDate = DateTime.Now;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = messageid;
                    }

                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
}
