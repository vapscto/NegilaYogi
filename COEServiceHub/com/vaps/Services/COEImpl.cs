using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Net;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using DomainModel.Model.com.vapstech.Birthday;
using System.Net.Mail;
using PreadmissionDTOs.com.vaps.BirthDay;
using DomainModel.Model.com.vaps.admission;
using SendGrid;
using CommonLibrary;
using DomainModel.Model.com.vapstech.HRMS;
using SendGrid.Helpers.Mail;

namespace CoeServiceHub.com.vaps.Services
{
    public class COEImpl : Interfaces.COEInterface
    {
        COEContext _context;
        DomainModelMsSqlServerContext _db;
        public COEImpl(COEContext context, DomainModelMsSqlServerContext db)
        {
            _context = context;
            _db = db;
        }
        public COEDTO getdata(COEDTO dto)
        {
            try
            {
                var Events = (from m in _context.COE_EventsDMO
                              from n in _context.COE_Master_EventsDMO
                              where m.COEME_Id == n.COEME_Id && m.MI_Id == n.MI_Id && m.MI_Id == dto.MI_Id && m.ASMAY_Id == dto.ASMAY_Id
                              && m.COEE_ActiveFlag == true && n.COEME_ActiveFlag == true && m.COEE_EStartDate.Value.Date >= DateTime.Now.Date
                              select new COEDTO
                              {
                                  COEE_Id = m.COEE_Id,
                                  COEME_Id = n.COEME_Id,
                                  COEME_EventName = n.COEME_EventName
                              }
                              ).ToList();
                if (Events.Count > 0)
                {
                    var config = _db.GenConfig.Where(d => d.MI_Id == dto.MI_Id).Select(d => d.IVRMGC_OTPMobileNo).ToList();
                    dto.configuration = config.ToArray();
                    dto.EventsList = Events;
                    dto.eventCount = Events.Count;
                }
                else
                {
                    dto.eventCount = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public COEDTO getEvents(COEDTO dto)
        {
            try
            {
                var Events = (from m in _context.COE_EventsDMO
                              from n in _context.COE_Master_EventsDMO
                              where m.COEME_Id == n.COEME_Id && m.MI_Id == n.MI_Id && m.MI_Id == dto.MI_Id
                              && m.COEE_ActiveFlag == true && n.COEME_ActiveFlag == true && m.COEME_Id == dto.COEME_Id
                              select new COEDTO
                              {
                                  COEE_Id = m.COEE_Id,
                                  COEME_Id = n.COEME_Id,
                                  COEME_EventName = n.COEME_EventName,
                                  COEE_EStartDate = m.COEE_EStartDate,
                                  COEE_EEndDate = m.COEE_EEndDate,
                                  COEE_SMSMessage = m.COEE_SMSMessage,
                                  COEE_Mail_Message = m.COEE_Mail_Message,
                                  COEE_ReminderDate = m.COEE_ReminderDate,
                                  COEE_SMSActiveFlag = m.COEE_SMSActiveFlag,
                                  COEE_MailActiveFlag = m.COEE_MailActiveFlag,
                                  COEE_StudentFlag = m.COEE_StudentFlag,
                                  COEE_EmployeeFlag = m.COEE_EmployeeFlag,
                                  COEE_AlumniFlag = m.COEE_AlumniFlag,
                                  COEE_OtherFlag = m.COEE_OtherFlag
                              }
                            ).ToList();

                dto.EventsDetails = Events;
                dto.ClassList = _context.AdmissionClass.Where(d => d.MI_Id == dto.MI_Id && d.ASMCL_ActiveFlag == true).
                Select(d => new AdmissionClass
                {
                    ASMCL_Id = d.ASMCL_Id,
                    ASMCL_ClassName = d.ASMCL_ClassName
                }).ToArray();

                dto.GroupTypeList = _context.HR_Master_Department.Where(d => d.MI_Id == dto.MI_Id && d.HRMD_ActiveFlag == true).
                    Select(d => new HR_Master_Department { HRMD_Id = d.HRMD_Id, HRMD_DepartmentName = d.HRMD_DepartmentName }).ToArray();

                dto.CoeEventsClassesList = _context.COE_Events_ClassesDMO.Where(d => d.COEE_Id == Events.FirstOrDefault().COEE_Id).Select(d=>d.ASMCL_Id).ToArray();
                dto.CoeEventsEmployees = _context.COE_Events_EmployeesDMO.Where(d => d.COEE_Id == Events.FirstOrDefault().COEE_Id).Select(d => d.HRMGT_Id).ToArray();
                dto.CoeEventsOthers = _context.COE_Events_OthersDMO.Where(d => d.COEE_Id == Events.FirstOrDefault().COEE_Id).Select(d => new COE_Events_OthersDMO {COEEO_Name=d.COEEO_Name,COEEO_Emailid=d.COEEO_Emailid,COEEO_MobileNo=d.COEEO_MobileNo }).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public COEDTO sendmsg(COEDTO data)
        {
            long trnsno = 0;
            List<COE_EventsDMO> arr = new List<COE_EventsDMO>();
            try
            {
                //  SMS NEW TABLES CODE START


                SMS smstransno = new SMS(_db);
                //  public async Task<string> sendSmsnew(long MI_Id, long mobileNo, string Template, long UserID, string sms)
               // trnsno = smstransno.getsmsno(data.MI_Id);

                //string studentempflag = "STUDENT";

                // SMS NEW TABLES CODE END
                var EventsDet = _context.COE_EventsDMO.Where(d => d.COEME_Id == data.COEME_Id).Select(d => new COE_EventsDMO
                {
                    COEE_Id = d.COEE_Id,
                    COEE_ActiveFlag = d.COEE_ActiveFlag,
                    MI_Id = d.MI_Id,
                    ASMAY_Id = d.ASMAY_Id,
                    COEME_Id = d.COEME_Id,
                    COEE_RepeatFlag = d.COEE_RepeatFlag,
                    COEE_ReminderSchedule = d.COEE_ReminderSchedule,
                    COEE_SMSMessage=d.COEE_SMSMessage,
                    COEE_Mail_Message=d.COEE_Mail_Message,
                    COEE_MailHeader=d.COEE_MailHeader,
                    COEE_MailSubject=d.COEE_MailSubject,
                    COEE_MailFooter=d.COEE_MailFooter

                }).ToList();

                if (data.COEE_StudentFlag == true)
                {
                    if (data.selectedClasses.Length > 0)
                    {
                        for (int i = 0; i < data.selectedClasses.Length; i++)
                        {
                            var stud = (from m in _db.Adm_M_Student
                                        from n in _db.School_Adm_Y_StudentDMO
                                        where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && m.AMST_SOL.Equals("S") && n.ASMCL_Id == data.selectedClasses[i].asmcL_Id
                                        && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                        select new Adm_M_StudentDTO { Name = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName) ? "" : ' ' + m.AMST_MiddleName) + (string.IsNullOrEmpty(m.AMST_LastName) ? "" : ' ' + m.AMST_LastName), AMST_MobileNo = m.AMST_MobileNo, AMST_Id = n.AMST_Id, AMST_emailId = m.AMST_emailId }).ToList();
                            if (stud.Count > 0)
                            {
                                if (data.SMS_Flag == true)
                                {
                                    for (int j = 0; j < stud.Count; j++)
                                    {
                                      string s = Send_SMS(stud[j].Name, stud[j].AMST_MobileNo.ToString(), stud[j].AMST_Id, EventsDet.FirstOrDefault().COEE_SMSMessage, Convert.ToInt32(data.MI_Id), "Student", "COE", EventsDet.FirstOrDefault().COEE_Id);

                                       // string s = Send_SMS(stud[j].Name, stud[j].AMST_MobileNo.ToString(), stud[j].AMST_Id, EventsDet.FirstOrDefault().COEE_SMSMessage, Convert.ToInt32(data.MI_Id), "Student", "COE", EventsDet.FirstOrDefault().COEE_Id, trnsno);
                                        if (s.Equals("success"))
                                        {
                                            data.msgStatus = "success";
                                        }
                                    }
                                }
                                if (data.Email_Flag== true)
                                {
                                    for (int j = 0; j < stud.Count; j++)
                                    {
                                      string s= Send_Email(stud[j].Name, stud[j].AMST_Id, EventsDet.FirstOrDefault().COEE_MailHeader, EventsDet.FirstOrDefault().COEE_MailSubject, EventsDet.FirstOrDefault().COEE_Mail_Message, stud[j].AMST_emailId, EventsDet.FirstOrDefault().COEE_MailFooter, Convert.ToInt32(data.MI_Id), "Student", "COE", EventsDet.FirstOrDefault().COEE_Id);
                                        // string s = Send_Email(stud[j].Name, stud[j].AMST_Id, EventsDet.FirstOrDefault().COEE_MailHeader, EventsDet.FirstOrDefault().COEE_MailSubject, EventsDet.FirstOrDefault().COEE_Mail_Message, stud[j].AMST_emailId, EventsDet.FirstOrDefault().COEE_MailFooter, Convert.ToInt32(data.MI_Id), "Student", "COE", EventsDet.FirstOrDefault().COEE_Id,trnsno);
                                        if (s.Equals("success"))
                                        {
                                            data.mailStatus = "success";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (data.COEE_EmployeeFlag == true)
                {
                    if(data.selectedGroupTypeList.Length > 0)
                    {
                        for(int k=0;k<data.selectedGroupTypeList.Length; k++)
                        {
                            var staff = (from d in _db.HR_Master_Employee_DMO
                                        from b in _db.Multiple_Email_DMO
                                        from c in _db.Multiple_Mobile_DMO
                                        where (d.HRME_Id == b.HRME_Id && d.HRME_Id == c.HRME_Id && b.HRMEM_DeFaultFlag.Equals("default") && c.HRMEMNO_DeFaultFlag.Equals("default") && d.MI_Id == data.MI_Id && d.HRME_ActiveFlag == true && d.HRME_LeftFlag == false && d.HRMD_Id == data.selectedGroupTypeList[k].hrmgT_Id)
                                        select new BirthDayDTO
                                        {
                                            HRME_Id = d.HRME_Id,
                                            employeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName),
                                            HRME_MobileNo = c.HRMEMNO_MobileNo == null ? 0 : c.HRMEMNO_MobileNo,
                                            HRME_EmailId = (string.IsNullOrEmpty(b.HRMEM_EmailId) ? "" : b.HRMEM_EmailId),
                                        }).Distinct().ToList();
                            if(staff.Count > 0)
                            {
                                if(data.SMS_Flag == true)
                                {
                                    for (int l = 0; l < staff.Count; l++)
                                    {
                                      string s= Send_SMS(staff[l].employeeName, staff[l].HRME_MobileNo.ToString(), staff[l].HRME_Id, EventsDet.FirstOrDefault().COEE_SMSMessage, Convert.ToInt32(data.MI_Id), "Employee", "COE", EventsDet.FirstOrDefault().COEE_Id);
                                        //string s = Send_SMS(staff[l].employeeName, staff[l].HRME_MobileNo.ToString(), staff[l].HRME_Id, EventsDet.FirstOrDefault().COEE_SMSMessage, Convert.ToInt32(data.MI_Id), "Employee", "COE", EventsDet.FirstOrDefault().COEE_Id, trnsno);
                                        if (s.Equals("success"))
                                        {
                                            data.msgStatus = "success";
                                        }
                                    }
                                }
                                if(data.Email_Flag == true)
                                {
                                    for (int l = 0; l < staff.Count; l++)
                                    {
                                      string s =Send_Email(staff[l].employeeName, staff[l].HRME_Id, EventsDet.FirstOrDefault().COEE_MailHeader, EventsDet.FirstOrDefault().COEE_MailSubject, EventsDet.FirstOrDefault().COEE_Mail_Message, staff[l].HRME_EmailId, EventsDet.FirstOrDefault().COEE_MailFooter, Convert.ToInt32(data.MI_Id), "Employee", "COE", EventsDet.FirstOrDefault().COEE_Id);
                                        //string s = Send_Email(staff[l].employeeName, staff[l].HRME_Id, EventsDet.FirstOrDefault().COEE_MailHeader, EventsDet.FirstOrDefault().COEE_MailSubject, EventsDet.FirstOrDefault().COEE_Mail_Message, staff[l].HRME_EmailId, EventsDet.FirstOrDefault().COEE_MailFooter, Convert.ToInt32(data.MI_Id), "Employee", "COE", EventsDet.FirstOrDefault().COEE_Id, trnsno);
                                        if (s.Equals("success"))
                                        {
                                            data.mailStatus = "success";
                                        }
                                    }
                                }
                            }
                        }
                    }
                   
                }
                if (data.COEE_OtherFlag == true)
                {
                    if (data.Others_list.Length > 0)
                    {
                        for (int k = 0; k < data.Others_list.Length; k++)
                        {
                            if (data.SMS_Flag == true)
                            {
                                if (data.Others_list[k].COEEO_MobileNo != null && data.Others_list[k].COEEO_MobileNo != "")
                                {
                                   string s = Send_SMS1(data.Others_list[k].COEEO_Name, data.Others_list[k].COEEO_MobileNo.ToString(), EventsDet.FirstOrDefault().COEE_SMSMessage, Convert.ToInt32(data.MI_Id), "COE", EventsDet.FirstOrDefault().COEE_Id);
                                    //string s = Send_SMS1(data.Others_list[k].COEEO_Name, data.Others_list[k].COEEO_MobileNo.ToString(), EventsDet.FirstOrDefault().COEE_SMSMessage, Convert.ToInt32(data.MI_Id), "COE", EventsDet.FirstOrDefault().COEE_Id, trnsno);
                                    if (s.Equals("success"))
                                    {
                                        data.msgStatus = "success";
                                    }
                                }
                            }
                            if (data.Email_Flag == true)
                            {
                                if (data.Others_list[k].COEEO_Emailid != null && data.Others_list[k].COEEO_Emailid != "")
                                {
                                  string s=  Send_Email1(data.Others_list[k].COEEO_Name, EventsDet.FirstOrDefault().COEE_Mail_Message, data.Others_list[k].COEEO_Emailid, Convert.ToInt32(data.MI_Id), "COE", EventsDet.FirstOrDefault().COEE_Id, EventsDet.FirstOrDefault().COEE_MailSubject);
                                    //string s = Send_Email1(data.Others_list[k].COEEO_Name, EventsDet.FirstOrDefault().COEE_Mail_Message, data.Others_list[k].COEEO_Emailid, Convert.ToInt32(data.MI_Id), "COE", EventsDet.FirstOrDefault().COEE_Id, EventsDet.FirstOrDefault().COEE_MailSubject, trnsno);
                                    if (s.Equals("success"))
                                    {
                                        data.mailStatus = "success";
                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public string Send_SMS(string amst_FName, string amst_mobile, long amst_Admno, string sms_message, int MID, string type, string Template, int coeid)
        {
            try
            {
                string sms = sms_message;
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count > 0)
                {
                   
                    string result = "";
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "BIRTHDAY_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = amst_Admno
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

                    for (int j = 0; j < variables.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (variables[j].Value.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(variables[j].Value, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = amst_mobile.ToString();
                    //PHNO = "966061628";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    //  string initialstatusss = responsedata.status;
                    //   string responsemessage = responsedata.message;
                    string messageid = responsedata;

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MID;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Calendar of Event";
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
        
        public string Send_Email(string amst_FName, long amst_Admno, string Email_header, string Email_subject, string Email_message, string amst_Email, string Email_footer, int MID, string type, string Template, int coeid)
        {
            try
            {
                string Mailmsg = Email_message;

                string date1 = "";

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MID && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count > 0)
                {
                   
                    string result = "";
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "BIRTHDAY_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = amst_Admno
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = "COE"
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


                    for (int j = 0; j < variables.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (variables[j].Value.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(variables[j].Value, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }

                    Mailmsg = result;
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = Email_subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    // message.AddTo("rakeshrd307@gmail.com");
                    message.AddTo(amst_Email);

                    var img = _context.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (img.Count > 0)
                    {
                        for (int i = 0; i < img.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].COEEI_Images) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.jpg");
                        }
                    }

                    var vido = _context.COE_Events_VideosDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (vido.Count > 0)
                    {

                        for (int i = 0; i < vido.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(vido[i].COEEV_Videos) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.mp4");
                        }
                    }

                    string name = "";
                    if (type.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                    {
                        //var query1 = _db.HR_Master_Employee_DMO.Single(d => d.HRME_Id == amst_Admno);
                        var query1 = (from d in _db.HR_Master_Employee_DMO
                                      where (d.HRME_LeftFlag == false && d.HRME_Id == amst_Admno)
                                      select new BirthDayDTO
                                      {
                                          HRME_Id = d.HRME_Id,
                                          employeeName = d.HRME_EmployeeFirstName + (string.IsNullOrEmpty(d.HRME_EmployeeMiddleName) ? "" : ' ' + d.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(d.HRME_EmployeeLastName) ? "" : ' ' + d.HRME_EmployeeLastName)
  
                                      }).Distinct().ToList();
                    
                        name = query1.FirstOrDefault().employeeName;

                        date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    }
                    else if (type.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {
                        var query1 = _db.Adm_M_Student.Where(d => d.AMST_Id == amst_Admno).Select(i => new Adm_M_Student { AMST_FirstName = i.AMST_FirstName, AMST_MiddleName = i.AMST_MiddleName, AMST_LastName = i.AMST_LastName }).ToList();
                        name = query1.FirstOrDefault().AMST_FirstName + ' ' + query1.FirstOrDefault().AMST_MiddleName ?? " " + ' ' + query1.FirstOrDefault().AMST_LastName;

                        date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");


                    }
                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);


                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);



                    }
                    else
                    {

                        // message.Html = "HAPPY BIRTHDAY DEAR" + " " + name + "<br/> No Template Found";
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    else
                    {
                        return "Sendgrid key is not available";
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = amst_Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Calendar of Event"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MID
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                       SqlDbType.BigInt)
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
        public string Send_SMS1(string amst_FName, string amst_mobile, string sms_message, int MID, string Template, int coeid)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();

              
                string sms = sms_message;

                string result = "";

                List<Match> variables = new List<Match>();
                List<SMS_MAIL_PARAMETER_DMO> ParamaetersName = new List<SMS_MAIL_PARAMETER_DMO>();


                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    SMS_MAIL_PARAMETER_DMO param = new SMS_MAIL_PARAMETER_DMO();
                    variables.Add(match);
                    param.ISMP_NAME = match.ToString();
                    ParamaetersName.Add(param);
                }

                if(ParamaetersName.Count > 0)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "COE_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@amst_FName",
                            SqlDbType.VarChar)
                        {
                            Value = amst_FName
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = sms_message
                        });
                        cmd.Parameters.Add(new SqlParameter("@miid",
                          SqlDbType.BigInt)
                        {
                            Value = MID
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
                }
             
                sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = amst_mobile.ToString();
                    // PHNO = "9686061628";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    //  string initialstatusss = responsedata.status;
                    //   string responsemessage = responsedata.message;
                    string messageid = responsedata;

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MID;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Calendar of Event";
                    dmo2.To_FLag = "Other";
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

        public string Send_Email1(string amst_FName, string Email_message, string amst_Email, int MID, string Template, int coeid, string Email_subject)
        {
            try
            {

                string date1 = "";

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }

                string Mailmsg = Email_message;
                string result = "";
                var institutionName = _db.Institution.Where(i => i.MI_Id == MID).ToList();
                List<Match> variables = new List<Match>();
                List<SMS_MAIL_PARAMETER_DMO> ParamaetersName = new List<SMS_MAIL_PARAMETER_DMO>();


                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    SMS_MAIL_PARAMETER_DMO param = new SMS_MAIL_PARAMETER_DMO();
                    variables.Add(match);
                    param.ISMP_NAME = match.ToString();
                    ParamaetersName.Add(param);
                }

                if(ParamaetersName.Count > 0)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "COE_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@amst_FName",
                            SqlDbType.VarChar)
                        {
                            Value = amst_FName
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Email_message
                        });
                        cmd.Parameters.Add(new SqlParameter("@miid",
                           SqlDbType.BigInt)
                        {
                            Value = MID
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
                }



                date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MID)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = Email_subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    //message.AddTo("rakeshrd307@gmail.com");
                    message.AddTo(amst_Email);

                    var img = _context.COE_Events_ImagesDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (img.Count > 0)
                    {
                        for (int i = 0; i < img.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].COEEI_Images) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.jpg");
                        }
                    }

                    var vido = _context.COE_Events_VideosDMO.Where(i => i.COEE_Id == coeid).ToList();
                    if (vido.Count > 0)
                    {

                        for (int i = 0; i < vido.Count; i++)
                        {
                            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(vido[i].COEEV_Videos) as HttpWebRequest;
                            System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            message.AddAttachment(stream.ToString(), "Calander_Event.mp4");
                        }
                    }



                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);


                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);


                    }
                    else
                    {

                        // message.Html = "HAPPY BIRTHDAY DEAR" + " " + name + "<br/> No Template Found";
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    else
                    {
                        return "Sendgrid key is not available";
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MID && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = amst_Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Calendar of Event"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MID
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                      SqlDbType.BigInt)
                        {
                            Value = "Other"
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
    }
}
