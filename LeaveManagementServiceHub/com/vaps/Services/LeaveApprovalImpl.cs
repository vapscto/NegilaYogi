using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.LeaveManagement;
using DomainModel.Model.com.vapstech.TT;
using LeaveManagementServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.LeaveManagement;
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


namespace LeaveManagementServiceHub.com.vaps.Services
{
    public class LeaveApprovalImpl : LeaveApprovalInterface
    {
        public DomainModelMsSqlServerContext _db;
        private readonly ILogger<LeaveReportImpl> _log;
        public LMContext _lmContext;
        public LeaveApprovalImpl(LMContext ttcategory, DomainModelMsSqlServerContext abc)
        {
            _lmContext = ttcategory;
            _db = abc;
        }

        public LeaveCreditDTO getApprovalStatus(LeaveCreditDTO data)
        {
            List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
            leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_LeaveCreditFlg == true).ToList();
            data.leave_name = leave_name.Distinct().ToArray();

            data.get_emp = (from x in _lmContext.HR_Master_Employee_DMO
                            where (x.MI_Id == data.MI_Id && x.HRME_ActiveFlag == true)
                            select new HR_Master_Employee_DMO
                            {
                                HRME_Id = x.HRME_Id,
                                HRME_EmployeeFirstName = x.HRME_EmployeeFirstName
                            }).Distinct().ToArray();

            leaveApprovalStatus(data);

            data.activityIds = (from a in _lmContext.HR_Emp_Leave_Trans_Details_DMO
                                from b in _lmContext.HR_Master_Leave_DMO
                                from c in _lmContext.HR_Master_Employee_DMO
                                from d in _lmContext.HR_Emp_Leave_Trans_DMO
                                from e in _lmContext.HR_Emp_Leave_ApplicationDMO
                                from f in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO
                                where (a.HRML_Id == b.HRML_Id && a.HRME_Id == c.HRME_Id && e.HRELAP_ActiveFlag==true && a.MI_Id == data.MI_Id && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false && a.HRELT_Id == d.HRELT_Id && d.HRELT_ActiveFlag == true && e.MI_Id == data.MI_Id && e.HRME_Id == c.HRME_Id && f.HRELAP_Id == e.HRELAP_Id && f.HRME_Id == c.HRME_Id && e.HRELAP_FromDate == a.HRELTD_FromDate && e.HRELAP_ToDate == a.HRELTD_ToDate)
                                select new LeaveCreditDTO
                                {
                                    HRELTD_Id = a.HRELTD_Id,
                                    HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                    HRML_LeaveName = b.HRML_LeaveName,
                                    HRELTD_FromDate = a.HRELTD_FromDate,
                                    HRELTD_ToDate = a.HRELTD_ToDate,
                                    HRELTD_TotDays = a.HRELTD_TotDays,
                                    HRME_EmployeeCode = c.HRME_EmployeeCode,
                                    HRELT_Status = d.HRELT_Status,
                                    HRELAPA_Remarks = f.HRELAPA_Remarks
                                }).Distinct().OrderByDescending(a => a.HRELTD_FromDate).ToArray();
            return data;
        }

        public LeaveCreditDTO leaveApprovalStatus(LeaveCreditDTO data)
        {
            //LeaveCreditDTO load = new LeaveCreditDTO();
            try
            {

                long Userid = 0;

                if (data.LoginId > 0)
                {
                    Userid = data.LoginId;
                }
                else
                {
                    Userid = data.UserId;
                }


                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_LeaveApproval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt)
                    {
                        Value = Userid
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
                        {
                            while (dataReader1.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader1.GetName(iFiled),
                                       dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_leavestatus = retObject.ToArray();
                        if (data.get_leavestatus.Length > 0)
                        {
                            data.message = "True";
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception ex)
            {
                _log.LogInformation("Leave Approval  error");
                _log.LogDebug(ex.Message);
            }
            return data;
        }

        public async Task<LeaveCreditDTO> get_status(LeaveCreditDTO data)
        {
            string status = "Approved";
            string sectionlevel = "1";
            var deviceidsnew = "";
            var devicenew = "";
            var redirecturl = "";
            var revieveduserid = 0;
            var TransactionID = 0;
            var leave_year = _lmContext.HR_Master_LeaveYear_DMO.Where(e => e.MI_Id == data.MI_Id && DateTime.Now.Date >= e.HRMLY_FromDate.Date && DateTime.Now.Date <= e.HRMLY_ToDate.Date).ToList();
            try
            {
                if (data.get_leave_status != null)
                {
                    for (var i = 0; i < data.get_leave_status.Count(); i++)
                    {

                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "leave_approval_status_NEW";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRELAP_Id", SqlDbType.BigInt)
                            {
                                Value = data.get_leave_status[i].HRELAP_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar)
                            {
                                Value = data.HRELT_Status
                            });
                            cmd.Parameters.Add(new SqlParameter("@LoginID", SqlDbType.BigInt)
                            {
                                Value = data.LoginId
                            });
                            cmd.Parameters.Add(new SqlParameter("@Remarks", SqlDbType.VarChar)
                            {
                                Value = data.HRELAPA_Remarks
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
                                                dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow);

                                        deviceidsnew = dataReader["HRME_AppDownloadedDeviceId"].ToString();
                                        revieveduserid = Convert.ToInt32(dataReader["Id"].ToString());
                                        TransactionID = Convert.ToInt32(dataReader["HRELAP_Id"].ToString());
                                    }
                                }
                                data.activityIds = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }




                        if (deviceidsnew!=null && deviceidsnew.Length > 0)
                        {

                            PushNotification push_noti = new PushNotification(_db);
                            push_noti.Call_PushNotificationGeneral(deviceidsnew, data.MI_Id, data.LoginId, revieveduserid, TransactionID, data.HRELAPA_Remarks, "LeaveStatus", "LeaveApproval");

                        }

                    }
                }
            }

            catch (Exception ex)
            {
                _log.LogInformation("Leave Approval  error");
                _log.LogDebug(ex.Message);
            }
            return data;
        }

        public async Task<LeaveCreditDTO> reject_status(LeaveCreditDTO data)
        {
            string status = "Rejected";
            string sectionlevel = "1";
            long hrmeId = 0;
            try
            {
                if (data.get_leave_status != null)
                {
                    for (var i = 0; i < data.get_leave_status.Count(); i++)
                    {
                        var temp_id = data.get_leave_status[i].HRELAP_ApplicationID;
                        var temp_leave_res = data.get_leave_status[i].HRELAP_LeaveReason;
                        using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "leave_approval_status_NEW";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@HRELAP_ApplicationID", SqlDbType.VarChar)
                            {
                                Value = temp_id
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar)
                            {
                                Value = status
                            });
                            //cmd.Parameters.Add(new SqlParameter("@HRELAP_SanctioningLevel",SqlDbType.Int)
                            //{
                            //    Value = sectionlevel
                            //});
                            cmd.Parameters.Add(new SqlParameter("@LoginID", SqlDbType.Int)
                            {
                                Value = data.LoginId
                            });
                            cmd.Parameters.Add(new SqlParameter("@Remarks", SqlDbType.VarChar)
                            {
                                Value = data.HRELAPA_Remarks
                            });
                            cmd.Parameters.Add(new SqlParameter("@Reason", SqlDbType.VarChar)
                            {
                                Value = temp_leave_res
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
                                                dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled] // use null instead of {}
                                            );
                                        }

                                        retObject.Add((ExpandoObject)dataRow);
                                    }
                                    data.activityIds = retObject.ToArray();

                                }
                                if (retObject.Count > 0)
                                {
                                    hrmeId = 0;
                                    long hrmL_Id = 0;
                                    decimal hrelaP_TotalDays = 0;
                                    foreach (LeaveCreditDTO item in data.get_leave_status)
                                    {
                                        hrmeId = item.HRME_Id;
                                        hrmL_Id = item.HRML_Id;
                                        hrelaP_TotalDays = item.HRELAP_TotalDays;
                                    }
                                    var updatleave = _lmContext.HR_Emp_Leave_StatusDMO.Single(d => d.MI_Id == data.MI_Id && d.HRME_Id == hrmeId && d.HRML_Id == hrmL_Id);
                                    //updatleave.HRELS_CBLeaves = updatleave.HRELS_CBLeaves + hrelaP_TotalDays;
                                    _lmContext.Update(updatleave);
                                    _lmContext.SaveChanges();
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            // leaveApprovalStatus(data);
                        }

                        string emailid = _lmContext.Emp_Email_Id.Where(p => p.HRME_Id == hrmeId && p.HRMEM_DeFaultFlag == "default").Select(p => p.HRMEM_EmailId).FirstOrDefault();
                        long mobileno = _lmContext.Emp_MobileNo.Where(p => p.HRME_Id == hrmeId && p.HRMEMNO_DeFaultFlag == "default").Select(p => p.HRMEMNO_MobileNo).FirstOrDefault();

                        sendmail(data.MI_Id, emailid, "Leave_Rejected", hrmeId, data.get_leave_status[i].HRELAP_FromDate);
                        sendSms(data.MI_Id, mobileno, "Leave_Rejected", hrmeId, data.get_leave_status[i].HRELAP_FromDate, data.get_leave_status[i].HRML_Id);
                    }
                }
            }

            catch (Exception ex)
            {
                _log.LogInformation("Leave Approval  error");
                _log.LogDebug(ex.Message);
            }
            return data;
        }

        public string sendmail(long MI_Id, string Email, string Template, long UserID, DateTime? LeaveDate)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                var employeename = _lmContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == UserID && p.MI_Id == MI_Id).Select(p => new { p.HRME_EmployeeFirstName, p.HRME_EmployeeMiddleName, p.HRME_EmployeeLastName }).FirstOrDefault();
                string Empname = employeename.HRME_EmployeeFirstName + " " + employeename.HRME_EmployeeMiddleName + " " + employeename.HRME_EmployeeLastName;
                Mailmsg = Mailmsg.Replace("[NAME]", Empname);
                DateTime date = DateTime.Parse(LeaveDate.ToString());
                Mailmsg = Mailmsg.Replace("[DATE]", date.ToString("dd/MM/yyyy"));

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
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
                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
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

        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID, DateTime? LeaveDate, long leaveid)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string sms = template.FirstOrDefault().ISES_SMSMessage;

                var employeename = _lmContext.HR_Master_Employee_DMO.Where(p => p.HRME_Id == UserID && p.MI_Id == MI_Id).Select(p => new { p.HRME_EmployeeFirstName, p.HRME_EmployeeMiddleName, p.HRME_EmployeeLastName }).FirstOrDefault();
                string Empname = employeename.HRME_EmployeeFirstName + " " + employeename.HRME_EmployeeMiddleName + " " + employeename.HRME_EmployeeLastName;
                sms = sms.Replace("[NAME]", Empname);
                DateTime date = DateTime.Parse(LeaveDate.ToString());
                sms = sms.Replace("[DATE]", date.ToString("dd/MM/yyyy"));
                string leavename = _lmContext.HR_Master_Leave_DMO.Where(p => p.MI_Id == MI_Id && p.HRML_Id == leaveid).Select(p => p.HRML_LeaveName).FirstOrDefault();
                sms = sms.Replace("[LEAVENAME]", leavename);

                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();
                    string PHNO = mobileNo.ToString();
                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    #region
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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
                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
                    #endregion
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }

        public LeaveCreditDTO getApprovedLeave(LeaveCreditDTO data)
        {
            List<HR_Master_Leave_DMO> leave_name = new List<HR_Master_Leave_DMO>();
            leave_name = _lmContext.HR_Master_Leave_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRML_LeaveCreditFlg == true).ToList();
            data.leave_name = leave_name.Distinct().ToArray();

            data.get_leavestatus = (from a in _lmContext.HR_Emp_Leave_ApplicationDMO
                                    from b in _lmContext.HR_Emp_Leave_Trans_DMO
                                    from c in _lmContext.HR_Master_Employee_DMO
                                    from d in _lmContext.HR_Master_Leave_DMO
                                    from e in _lmContext.HR_Leave_Auth_OrderNo_DMO
                                    from f in _lmContext.Staff_User_Login
                                    from g in _lmContext.HR_Leave_Authorisation_DMO
                                    where (a.HRELAP_FromDate == b.HRELT_FromDate && a.HRELAP_ToDate == b.HRELT_ToDate && a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id && a.HRME_Id == c.HRME_Id && c.HRME_ActiveFlag == true && c.MI_Id == data.MI_Id && b.HRELT_LeaveId == d.HRML_Id && d.MI_Id == data.MI_Id && e.HRLA_Id == g.HRLA_Id && g.HRMDES_Id == c.HRMDES_Id && g.HRMD_Id == c.HRMD_Id && g.HRMGT_Id == c.HRMGT_Id && g.HRMG_Id == c.HRMG_Id && g.HRML_Id == b.HRELT_LeaveId && f.Id == data.LoginId && f.Emp_Code == e.HRME_Id && f.MI_Id == data.MI_Id && a.HRELAP_ApplicationStatus == "Approved" && a.HRELAP_ActiveFlag == true && b.HRELT_ActiveFlag == true && b.HRELT_FromDate > DateTime.Today)
                                    select new LeaveCreditDTO
                                    {
                                        HRELAP_Id = a.HRELAP_Id,
                                        HRME_EmployeeFirstName = c.HRME_EmployeeFirstName,
                                        HRML_LeaveType = d.HRML_LeaveName,
                                        HRELAP_ApplicationID = a.HRELAP_ApplicationID,
                                        HRELAP_FromDate = a.HRELAP_FromDate,
                                        HRELAP_ToDate = a.HRELAP_ToDate,
                                        HRELAP_TotalDays = a.HRELAP_TotalDays,
                                        HRELAP_ReportingDate = a.HRELAP_ReportingDate,
                                        HRELAP_LeaveReason = a.HRELAP_LeaveReason,
                                        HRME_Id = c.HRME_Id,
                                        HRML_Id = d.HRML_Id,
                                        CreatedDate = b.CreatedDate
                                    }).Distinct().OrderBy(t => t.CreatedDate).ToArray();

            data.activityIds = (from a in _lmContext.HR_Emp_Leave_Trans_Details_DMO
                                from b in _lmContext.HR_Master_Leave_DMO
                                from c in _lmContext.HR_Master_Employee_DMO
                                from d in _lmContext.HR_Emp_Leave_Trans_DMO
                                from e in _lmContext.HR_Emp_Leave_ApplicationDMO
                                from f in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO
                                where (a.HRML_Id == b.HRML_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == data.MI_Id && c.HRME_ActiveFlag == true && c.HRME_LeftFlag == false && a.HRELT_Id == d.HRELT_Id && d.HRELT_ActiveFlag == true && e.MI_Id == data.MI_Id && e.HRME_Id == c.HRME_Id && f.HRELAP_Id == e.HRELAP_Id && f.HRME_Id == c.HRME_Id && e.HRELAP_FromDate == a.HRELTD_FromDate && e.HRELAP_ToDate == a.HRELTD_ToDate)
                                select new LeaveCreditDTO
                                {
                                    HRELTD_Id = a.HRELTD_Id,
                                    HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),
                                    HRML_LeaveName = b.HRML_LeaveName,
                                    HRELTD_FromDate = a.HRELTD_FromDate,
                                    HRELTD_ToDate = a.HRELTD_ToDate,
                                    HRELTD_TotDays = a.HRELTD_TotDays,
                                    HRME_EmployeeCode = c.HRME_EmployeeCode,
                                    HRELT_Status = d.HRELT_Status,
                                    HRELAPA_Remarks = f.HRELAPA_Remarks
                                }).Distinct().ToArray();
            return data;
        }


        public LeaveCreditDTO Viewleavebalancehistory(LeaveCreditDTO data)
        {
            try
            {
                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmpWiseLeavesDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = data.HRME_Id });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getemployeeleavedetails = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogDebug(ex.Message);
                data.message = "False";
            }
            return data;
        }

        //onduty approval
        public LeaveCreditDTO getRequestStatus(LeaveCreditDTO dto)
        {
            try
            {
                string[] leavecodes = new string[2];
                leavecodes[0] = "OD";
                leavecodes[1] = "COMPOFF";




                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Compoffondutyleave_APPROVALEMPLIST";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.VarChar) { Value = dto.LoginId });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        dto.get_leavestatus = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                dto.activityIds = (from c in _lmContext.HR_Emp_Leave_ApplicationDMO
                                   from d in _lmContext.HR_Emp_Leave_Appl_AuthorisationDMO
                                   from e in _lmContext.HR_Master_Employee_DMO
                                   from f in _lmContext.HR_Master_Leave_DMO
                                   from g in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                   where (c.HRELAP_Id == d.HRELAP_Id && c.HRELAP_Id == g.HRELAP_Id && f.HRML_Id == g.HRML_Id && leavecodes.Contains(f.HRML_LeaveCode) && f.MI_Id == dto.MI_Id && c.HRME_Id == e.HRME_Id && c.HRELAP_ActiveFlag == true && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && e.MI_Id == dto.MI_Id)
                                   select new LeaveCreditDTO
                                   {
                                       HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                       HRML_LeaveName = f.HRML_LeaveName,
                                       HRELAP_FromDate = c.HRELAP_FromDate,
                                       HRELAP_ToDate = c.HRELAP_ToDate,
                                       HRELAP_TotalDays = c.HRELAP_TotalDays,
                                       HRME_EmployeeCode = e.HRME_EmployeeCode,
                                       HRELAP_ApplicationStatus = c.HRELAP_ApplicationStatus,
                                       HRELAPA_Remarks = d.HRELAPA_Remarks,
                                       HRELAPA_InTime = d.HRELAPA_InTime,
                                       HRELAPA_OutTime = d.HRELAPA_OutTime
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return dto;
        }
        public async Task<LeaveCreditDTO> get_approvestatusAsync(LeaveCreditDTO data)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            string status = "Approved";
            string sectionlevel = "1";

            string[] leavecodes = new string[2];
            leavecodes[0] = "OD";
            leavecodes[1] = "COMPOFF";

            try
            {

                var HRME_ID = _lmContext.Staff_User_Login.Where(t => t.Id == data.LoginId).Select(t => t.Emp_Code).FirstOrDefault();

                if (data.doc_list2.Length > 0)
                {
                    foreach (var p in data.doc_list2)
                    {
                        var contactExistsP = _lmContext.Database.ExecuteSqlCommand("CompoffLeaveApproval @p0,@p1,@p2,@p3,@p4,@p5", p.HRELAP_Id, HRME_ID, p.HRELAPA_Remarks, p.HRELAPD_InTime, p.HRELAPD_OutTime, p.Status);
                        if (contactExistsP > 0)
                        {
                            data.message = "Add";
                        }
                        else
                        {
                            data.message = "notUpdated";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("CompOff/OD Approval error");
                _log.LogDebug(ex.Message);
                data.message = "False";
            }
            return data;
        }
        //END
        //periodwiseapproval/////////////////////////////////////////////////////////////
        public LeaveCreditDTO getperiodApprovalStatus(LeaveCreditDTO data)
        {
            try
            {
                data.periodwiseApproval = (from a in _lmContext.HR_Emp_Leave_ApplicationDMO
                                           from b in _lmContext.HR_Emp_Leave_Appl_DetailsDMO
                                           from c in _lmContext.HR_Emp_Leave_Application_DeputationDMO
                                           from d in _lmContext.HR_Master_Employee_DMO
                                           from e in _lmContext.HR_Master_Employee_DMO
                                           from f in _lmContext.HR_Master_Leave_DMO
                                           from g in _lmContext.Staff_User_Login
                                           where (g.Emp_Code == c.HRME_Id && g.Id == data.LoginId &&a.HRELAP_ActiveFlag==true && b.HRELAPD_ActiveFlag==true && d.HRME_Id == a.HRME_Id && e.HRME_Id == c.HRME_Id && c.HRELAPD_Id == b.HRELAPD_Id && b.HRELAP_Id == a.HRELAP_Id && f.HRML_Id == b.HRML_Id && c.HRELAPDD_ApprovalFlg == "Applied")
                                           select new LeaveCreditDTO
                                           {
                                               HRML_LeaveName = f.HRML_LeaveName,
                                               HRELAPDD_Id = c.HRELAPDD_Id,
                                               HRELAPD_Id = c.HRELAPD_Id,
                                               HRME_Id = c.HRME_Id,
                                               HRELAP_TotalDays = a.HRELAP_TotalDays,
                                               HRELAP_FromDate = a.HRELAP_FromDate,
                                               HRELAP_ToDate = a.HRELAP_ToDate,
                                               HRELAP_LeaveReason = a.HRELAP_LeaveReason,
                                               HRELAPDD_Date = c.HRELAPDD_Date,
                                               HRELAPDD_Period = c.HRELAPDD_Period,
                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                               ApprovalEmployee = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),

                                           }).Distinct().ToArray();

                using (var cmd = _lmContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_PeriodWiseLeaveApproval";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.VarChar) { Value = data.LoginId });

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
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.approvalstatus = retObject.ToArray();
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
        public LeaveCreditDTO periodleavestatus(LeaveCreditDTO data)
        {
            try
            {
                if (data.HRELAPDD_ApprovalFlg == "Approved")
                {
                    if (data.get_leave_status != null && data.get_leave_status.Length > 0)
                    {
                        foreach (var item in data.get_leave_status)
                        {
                            if (item.HRELAPDD_Id > 0)
                            {
                                HR_Emp_Leave_Application_DeputationDMO rlt = new HR_Emp_Leave_Application_DeputationDMO();
                                rlt.HRELAPDD_Id = item.HRELAPDD_Id;
                                rlt.HRELAPD_Id = item.HRELAPD_Id;
                                rlt.HRME_Id = item.HRME_Id;
                                rlt.HRELAPDD_Remarks = data.HRELAPDD_Remarks;
                                rlt.HRELAPDD_Period = item.HRELAPDD_Period;
                                rlt.HRELAPDD_ApprovalFlg = data.HRELAPDD_ApprovalFlg;
                                rlt.HRELAPDD_Date = item.HRELAPDD_Date;
                                rlt.HRELAPDD_UpdatedBy = data.LoginId;
                                rlt.HRELAPDD_UpdatedDate = DateTime.Now;
                                rlt.HRELAPDD_CreatedDate = DateTime.Now;
                                rlt.HRELAPDD_ActiveFlag = true;                             
                                _lmContext.Update(rlt);
                                var contactExists = _lmContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnmsg = "Approved";
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }

                        }
                    }
                }
                else
                {
                    if (data.get_leave_status != null && data.get_leave_status.Length > 0)
                    {
                        foreach (var item in data.get_leave_status)
                        {
                            if (item.HRELAPDD_Id > 0)
                            {
                                HR_Emp_Leave_Application_DeputationDMO result = new HR_Emp_Leave_Application_DeputationDMO();
                                result.HRELAPDD_Id = item.HRELAPDD_Id;
                                result.HRELAPD_Id = item.HRELAPD_Id;
                                result.HRME_Id = item.HRME_Id;
                                result.HRELAPDD_Remarks = data.HRELAPDD_Remarks;
                                result.HRELAPDD_Period = item.HRELAPDD_Period;
                                result.HRELAPDD_Date = item.HRELAPDD_Date;
                                result.HRELAPDD_ApprovalFlg = data.HRELAPDD_ApprovalFlg;
                                result.HRELAPDD_UpdatedBy = data.LoginId;
                                result.HRELAPDD_ActiveFlag = true;
                                result.HRELAPDD_UpdatedDate = DateTime.Now;
                                result.HRELAPDD_CreatedDate = DateTime.Now;
                                _lmContext.Update(result);
                                var contactExists = _lmContext.SaveChanges();
                                if (contactExists > 0)
                                {
                                    data.returnmsg = "Rejected";
                                }
                                else
                                {
                                    data.returnval = false;
                                }
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
    }
}