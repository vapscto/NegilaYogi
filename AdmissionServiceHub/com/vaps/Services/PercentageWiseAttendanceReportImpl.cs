using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class PercentageWiseAttendanceReportImpl : Interfaces.PercentageWiseAttendanceReportInterface
    {
        public StudentAttendanceReportContext _context;
        public DomainModelMsSqlServerContext _db;
        public PercentageWiseAttendanceReportImpl(StudentAttendanceReportContext _contex, DomainModelMsSqlServerContext _dbd)
        {
            _context = _contex;
            _db = _dbd;
        }
        public PercentageWiseAttendanceReportDTO getloaddata(PercentageWiseAttendanceReportDTO data)
        {
            try
            {
                data.getyear = _context.academicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PercentageWiseAttendanceReportDTO getclass(PercentageWiseAttendanceReportDTO data)
        {
            try
            {
                var checkempcode = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.IVRMSTAUL_ActiveFlag == 1).ToList();
                if (checkempcode.Count() > 0)
                {
                    data.HRME_Id = checkempcode.FirstOrDefault().Emp_Code;

                    data.getclass = (from a in _context.Adm_SchAttLoginUser
                                     from b in _context.Adm_SchAttLoginUserClass
                                     from c in _context.admissionClass
                                     where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.HRME_Id == data.HRME_Id
                                     && a.MI_Id == data.MI_Id)
                                     select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                }
                else
                {
                    data.getclass = (from a in _context.Masterclasscategory
                                     from b in _context.admissionClass
                                     where (a.ASMCL_Id == b.ASMCL_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                     select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PercentageWiseAttendanceReportDTO getsection(PercentageWiseAttendanceReportDTO data)
        {
            try
            {
                var checkempcode = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.IVRMSTAUL_ActiveFlag == 1).ToList();
                if (checkempcode.Count() > 0)
                {
                    data.HRME_Id = checkempcode.FirstOrDefault().Emp_Code;

                    data.getsection = (from a in _context.Adm_SchAttLoginUser
                                       from b in _context.Adm_SchAttLoginUserClass
                                       from c in _context.admissionClass
                                       from d in _context.masterSection
                                       where (a.ASALU_Id == b.ASALU_Id && b.ASMS_Id == d.ASMS_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id
                                       && a.HRME_Id == data.HRME_Id && b.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id)
                                       select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();

                }
                else
                {
                    data.getsection = (from a in _context.Masterclasscategory
                                       from b in _context.admissionClass
                                       from c in _context.AdmSchoolMasterClassCatSec
                                       from d in _context.masterSection
                                       where (a.ASMCL_Id == b.ASMCL_Id && a.ASMCC_Id == c.ASMCC_Id && c.ASMS_Id == d.ASMS_Id && a.Is_Active == true
                                       && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMCCS_ActiveFlg == true && a.ASMCL_Id == data.ASMCL_Id)
                                       select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PercentageWiseAttendanceReportDTO showreport(PercentageWiseAttendanceReportDTO data)
        {
            try
            {
                string confromdate = "";
                DateTime fromdatecon = Convert.ToDateTime(data.fromdate.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                string todate = "";
                DateTime todatecon = Convert.ToDateTime(data.todate.Date.ToString("yyyy-MM-dd"));
                todate = todatecon.ToString("yyyy-MM-dd");

                var asms_id = "0";

                List<School_M_Section> sectionlist = new List<School_M_Section>();


                var checkempcode = _context.Staff_User_Login.Where(a => a.Id == data.userid && a.IVRMSTAUL_ActiveFlag == 1).ToList();
                if (checkempcode.Count() > 0)
                {
                    data.HRME_Id = checkempcode.FirstOrDefault().Emp_Code;

                    if (data.ASMS_Id == 0)
                    {
                        sectionlist = (from a in _context.Adm_SchAttLoginUser
                                       from b in _context.Adm_SchAttLoginUserClass
                                       from c in _context.admissionClass
                                       from d in _context.masterSection
                                       where (a.ASALU_Id == b.ASALU_Id && b.ASMS_Id == d.ASMS_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id
                                       && a.HRME_Id == data.HRME_Id && b.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id)
                                       select d).Distinct().OrderBy(a => a.ASMC_Order).ToList();
                    }
                    else
                    {
                        sectionlist = (from a in _context.Adm_SchAttLoginUser
                                       from b in _context.Adm_SchAttLoginUserClass
                                       from c in _context.admissionClass
                                       from d in _context.masterSection
                                       where (a.ASALU_Id == b.ASALU_Id && b.ASMS_Id == d.ASMS_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id
                                       && a.HRME_Id == data.HRME_Id && b.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id && b.ASMS_Id == data.ASMS_Id)
                                       select d).Distinct().OrderBy(a => a.ASMC_Order).ToList();
                    }
                }
                else
                {
                    if (data.ASMS_Id == 0)
                    {
                        sectionlist = (from a in _context.Masterclasscategory
                                       from b in _context.admissionClass
                                       from c in _context.AdmSchoolMasterClassCatSec
                                       from d in _context.masterSection
                                       where (a.ASMCL_Id == b.ASMCL_Id && a.ASMCC_Id == c.ASMCC_Id && c.ASMS_Id == d.ASMS_Id && a.Is_Active == true
                                       && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMCCS_ActiveFlg == true && a.ASMCL_Id == data.ASMCL_Id)
                                       select d).Distinct().OrderBy(a => a.ASMC_Order).ToList();
                    }
                    else
                    {
                        sectionlist = (from a in _context.Masterclasscategory
                                       from b in _context.admissionClass
                                       from c in _context.AdmSchoolMasterClassCatSec
                                       from d in _context.masterSection
                                       where (a.ASMCL_Id == b.ASMCL_Id && a.ASMCC_Id == c.ASMCC_Id && c.ASMS_Id == d.ASMS_Id && a.Is_Active == true
                                       && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMCCS_ActiveFlg == true && a.ASMCL_Id == data.ASMCL_Id
                                       && c.ASMS_Id == data.ASMS_Id)
                                       select d).Distinct().OrderBy(a => a.ASMC_Order).ToList();
                    }
                }

                foreach (var c in sectionlist)
                {
                    asms_id = asms_id + "," + c.ASMS_Id;
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Attendance_Percentage_Report_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_ID", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = asms_id });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar) { Value = todate });
                    cmd.Parameters.Add(new SqlParameter("@allorindi", SqlDbType.VarChar) { Value = data.allorindi });
                    cmd.Parameters.Add(new SqlParameter("@percentage", SqlDbType.VarChar) { Value = data.percentage });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = data.flag });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;

                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }

                        data.getreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                    }
                }

                data.getinstituion = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<PercentageWiseAttendanceReportDTO> SendAttendanceSMS(PercentageWiseAttendanceReportDTO data)
        {
            try
            {
                var template = _context.SMSEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "Attendance_Shortage_Percentage").ToList();

                string result = template.FirstOrDefault().ISES_SMSMessage;

                if (data.Temp_studentlist != null && data.Temp_studentlist.Length > 0)
                {
                    foreach (var d in data.Temp_studentlist)
                    {
                        string studentname = d.studentname;
                        string studentemailid = d.AMST_emailId;
                        string studentclassname = d.classname;
                        string studentsectionname = d.sectionname;
                        long studentmobileno = d.AMST_MobileNo;
                        long AMST_Id = d.AMST_Id;
                        decimal? percentage = d.percentage;
                        decimal? totalworkingdays = d.totalworkingdays;
                        decimal? totalpresentdays = d.totalpresentdays;

                        result = result.Replace("[NAME]", studentname);
                        result = result.Replace("[TOTAL_WORKING_DAYS]", Convert.ToString(totalworkingdays));
                        result = result.Replace("[TOTAL_PRESENT_DAYS]", Convert.ToString(totalpresentdays));
                        result = result.Replace("[TOTAL_PERCENTAGE]", Convert.ToString(percentage));
                        result = result.Replace("[CLASS]", studentclassname);
                        result = result.Replace("[SECTION]", studentsectionname);

                        if (data.smschecked == true)
                        {
                            try
                            {
                                var s = SendSms(data.MI_Id, studentmobileno, "Attendance_Shortage_Percentage", AMST_Id, studentclassname, studentsectionname, studentname, percentage, totalworkingdays, totalpresentdays);
                            }

                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (data.emailchecked == true)
                        {
                            try
                            {
                                var s = SendEmail(data.MI_Id, studentemailid, "Attendance_Shortage_Percentage", AMST_Id, studentclassname, studentsectionname, studentname,percentage, totalworkingdays, totalpresentdays);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        if (data.whatsappchecked == true)
                        {
                            try
                            {
                                SmsWithoutTemplate smsWithoutTemplate = new SmsWithoutTemplate(_db);
                                var s = await smsWithoutTemplate.SendwhatsappfromPortal(data.MI_Id, studentmobileno, result, "", "");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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

        public async Task<string> SendSms(long MI_Id, long mobileNo, string Template, long AMST_Id, string studentclassname, string studentsectionname,
           string studentname, decimal? percentage, decimal? totalworkingdays, decimal? totalpresentdays)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _context.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    sms = result;
                }
                else
                {
                    result = result.Replace("[NAME]", studentname);
                    result = result.Replace("[TOTAL_WORKING_DAYS]", Convert.ToString(totalworkingdays));
                    result = result.Replace("[TOTAL_PRESENT_DAYS]", Convert.ToString(totalpresentdays));
                    result = result.Replace("[TOTAL_PERCENTAGE]", Convert.ToString(percentage));
                    result = result.Replace("[CLASS]", studentclassname);
                    result = result.Replace("[SECTION]", studentsectionname);
                    sms = result;
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _context.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entity_id", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("template_id", template.FirstOrDefault().ISES_TemplateId);
                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo", SqlDbType.NVarChar) { Value = PHNO });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar) { Value = sms });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar) { Value = modulename[0] });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = "Delivered" });
                        cmd.Parameters.Add(new SqlParameter("@Message_id", SqlDbType.VarChar) { Value = messageid });

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
            }
            return "success";
        }

        public async Task<string> SendEmail(long MI_Id, string Email, string Template, long AMST_Id, string studentclassname, string studentsectionname,
          string studentname, decimal? percentage, decimal? totalworkingdays, decimal? totalpresentdays)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _context.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;

                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    result = result.Replace("[NAME]", studentname);
                    result = result.Replace("[TOTAL_WORKING_DAYS]", Convert.ToString(totalworkingdays));
                    result = result.Replace("[TOTAL_PRESENT_DAYS]", Convert.ToString(totalpresentdays));
                    result = result.Replace("[TOTAL_PERCENTAGE]", Convert.ToString(percentage));
                    result = result.Replace("[CLASS]", studentclassname);
                    result = result.Replace("[SECTION]", studentsectionname);
                    Mailmsg = result;
                    Mailcontent = result;
                }
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();

                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }
                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);

                        if (Attechement.Equals("1"))
                        {
                            var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }

                        if (mailcc != null && mailcc != "")
                        {
                            message.AddCc(mailcc);
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }

                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;

                                if (Attechement.Equals("1"))
                                {
                                    var img = _context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {
                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }
                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar) { Value = Email });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar) { Value = Mailcontent });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar) { Value = modulename[0] });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });

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
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            return "success";
        }
    }
}
