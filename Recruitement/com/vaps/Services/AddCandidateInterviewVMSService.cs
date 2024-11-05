using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary;
using DomainModel.Model;
using SendGrid.Helpers.Mail;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using SendGrid;
using System.Dynamic;
using System.IO;
using System.Net.Mail;
using System.Net;

namespace Recruitment.com.vaps.Services
{
    public class AddCandidateInterviewVMSService : Interfaces.AddCandidateInterviewVMSInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public AddCandidateInterviewVMSService(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        public HR_CandidateInterviewScheduleDTO getBasicData(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }
        public HR_CandidateInterviewScheduleDTO getallgrade(HR_CandidateInterviewScheduleDTO dto)
        {           
            try
            {
                dto.gradelist = _VMSContext.HR_Candidate_Master_GradeDMO.Where(t => t.MI_Id == dto.MI_Id && t.HRCMG_ActiveFlag == true).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_CandidateInterviewScheduleDTO SaveUpdate(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_CandidateInterviewScheduleDMO dmoObj = Mapper.Map<HR_CandidateInterviewScheduleDMO>(dto);

                var duplicatecountresult = _VMSContext.HR_CandidateInterviewScheduleDMO.Where(t => t.HRCD_Id == dto.HRCD_Id && t.HRCISC_InterviewRounds == dto.HRCISC_InterviewRounds && t.HRCISC_InterviewDateTime == dto.HRCISC_InterviewDateTime && t.HRCISC_InterviewVenue == dto.HRCISC_InterviewVenue && t.HRCISC_Interviewer == dto.HRCISC_Interviewer).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRCISC_Id > 0)
                    {
                        var result = _VMSContext.HR_CandidateInterviewScheduleDMO.Single(t => t.HRCISC_Id == dmoObj.HRCISC_Id);
                        dto.UpdatedDate = DateTime.Now;
                        dto.HRCISC_ActiveFlg = true;
                        Mapper.Map(dto, result);
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dmoObj.HRCISC_ActiveFlg = true;
                        dmoObj.HRCISC_CreatedBy = dto.HRCISC_UpdatedBy;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _VMSContext.Add(dmoObj);
                        var flag = _VMSContext.SaveChanges();
                        dto.HRCISC_Id = dmoObj.HRCISC_Id;
                        if (flag == 1)
                        {
                            if (dmoObj.HRCISC_NotifyEmail == true)
                            {
                                string candidatemail = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).Select(t => t.HRCD_EmailId).FirstOrDefault();
                                if (candidatemail != null && candidatemail != "")
                                {
                                    SendEmail(dto, "Interview Call Letter", "Interview", dto.MI_Id);
                                }
                            }
                            if (dmoObj.HRCISC_NotifySMS == true)
                            {
                                long candidatemobile = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == dto.HRCD_Id).Select(t => t.HRCD_MobileNo).FirstOrDefault();
                                if (candidatemobile != null && candidatemobile !=0)
                                {
                                    SMS sms = new SMS(_Context);
                                    sms.sendSms(dto.MI_Id, candidatemobile, "Interview_Notification", dto.HRCISC_Id);
                                }
                            }
                            dto.retrunMsg = "Add";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                }
                else
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }
                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO editData(int id)
        {
            HR_CandidateInterviewScheduleDTO dto = new HR_CandidateInterviewScheduleDTO();
            dto.retrunMsg = "";
            try
            {
                dto.VMSEditValue = (from a in _VMSContext.HR_CandidateInterviewScheduleDMO
                                    from b in _VMSContext.HR_Candidate_DetailsDMO
                                    where (a.HRCD_Id == b.HRCD_Id)
                                    select new HR_CandidateInterviewScheduleDTO
                                    {
                                        HRCISC_Id = a.HRCISC_Id,
                                        HRCD_Id = a.HRCD_Id,
                                        HRCD_FullName = b.HRCD_FirstName + " " + b.HRCD_MiddleName + " " + b.HRCD_LastName,
                                        HRCISC_InterviewRounds = a.HRCISC_InterviewRounds,
                                        HRCISC_InterviewDateTime = a.HRCISC_InterviewDateTime,
                                        HRCISC_InterviewVenue = a.HRCISC_InterviewVenue
                                    }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO deactivate(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRCISC_Id > 0)
                {
                    var result = _VMSContext.HR_CandidateInterviewScheduleDMO.Single(t => t.HRCISC_Id == dto.HRCISC_Id);

                    if (result.HRCISC_ActiveFlg == true)
                    {
                        result.HRCISC_ActiveFlg = false;
                    }
                    else if (result.HRCISC_ActiveFlg == false)
                    {
                        result.HRCISC_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _VMSContext.Update(result);
                    var flag = _VMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRCISC_ActiveFlg == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO GetAllDropdownAndDatatableDetails(HR_CandidateInterviewScheduleDTO dto)
        {
            List<HR_CandidateInterviewScheduleDMO> datalist = new List<HR_CandidateInterviewScheduleDMO>();
            try
            {
                var interviewlist = _VMSContext.HR_CandidateInterviewScheduleDMO.Where(p => (p.HRCISC_Status == "Upcomming" || p.HRCISC_Status == "InProgress") && p.HRCISC_ActiveFlg == true).Select(p => p.HRCD_Id).Distinct().ToList();

                dto.CandidateDetailsList = (from emp in _VMSContext.HR_Candidate_DetailsDMO
                                            where emp.HRCD_ActiveFlg == true && emp.MI_Id == dto.MI_Id && !interviewlist.Contains(emp.HRCD_Id)
                                            select new HR_CandidateInterviewScheduleDTO
                                            {
                                                HRCD_Id = emp.HRCD_Id,
                                                HRCD_FirstName = emp.HRCD_FirstName,
                                                HRCD_MiddleName = (emp.HRCD_MiddleName == null ? "" : emp.HRCD_MiddleName),
                                                HRCD_LastName = (emp.HRCD_LastName == null ? "" : emp.HRCD_LastName)
                                                //HRCD_FullName = emp.HRCD_FirstName.ToString() + " " + emp.HRCD_MiddleName.ToString() + " " + emp.HRCD_LastName.ToString(),
                                            }).Distinct().ToArray();

                dto.InterviewerList = (from emp in _VMSContext.IVRM_Staff_User_Login
                                       from empl in _VMSContext.HR_Master_Employee_DMO
                                       from empi in _VMSContext.Institution
                                       where (emp.Emp_Code == empl.HRME_Id && empl.HRME_ActiveFlag == true && empl.HRME_LeftFlag == false && empl.MI_Id == empi.MI_Id && empi.MI_ActiveFlag == 1)
                                       select new HR_CandidateInterviewScheduleDTO
                                       {
                                           Id = emp.Id,
                                           HRME_Id = empl.HRME_Id,
                                           HRME_EmployeeFirstName = empl.HRME_EmployeeFirstName,
                                           HRME_EmployeeMiddleName = (empl.HRME_EmployeeMiddleName == null ? "" : empl.HRME_EmployeeMiddleName),
                                           HRME_EmployeeLastName = (empl.HRME_EmployeeLastName == null ? "" : empl.HRME_EmployeeLastName)
                                       }).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public HR_CandidateInterviewScheduleDTO getrpt(HR_CandidateInterviewScheduleDTO dto)
        {
            using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "CANDIDATE_INTREVIEW_REPORT";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@type",
                  SqlDbType.VarChar)
                {
                    Value = dto.rdotype
                });

                cmd.Parameters.Add(new SqlParameter("@round",
                  SqlDbType.VarChar)
                {
                    Value = dto.HRCISC_InterviewRounds
                });
                cmd.Parameters.Add(new SqlParameter("@grade",
                 SqlDbType.BigInt)
                {
                    Value = dto.HRCMG_Id
                });
                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                {
                    Value = dto.MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate",
                SqlDbType.Date)
                {
                    Value = dto.fromdate.Date.ToString("yyyy-MM-dd")
                });
                cmd.Parameters.Add(new SqlParameter("@todate",
                SqlDbType.Date)
                {
                    Value = dto.todate.Date.ToString("yyyy-MM-dd")
                });

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
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                );
                            }
                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    dto.upcomingintvw = retObject.ToArray();
                }

                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return dto;
        }

        public HR_CandidateInterviewScheduleDTO savefeedback(HR_CandidateInterviewScheduleDTO dto)
        {
            dto.retrunMsg = "";
            dto.HRCIS_Datetime = DateTime.Now;
            try
            {
                HR_Candidate_InterviewStatusDMO dmoObj = Mapper.Map<HR_Candidate_InterviewStatusDMO>(dto);
                HR_CandidateInterviewScheduleDMO OBJDMO = Mapper.Map<HR_CandidateInterviewScheduleDMO>(dto);

                var duplicatecountresult = _VMSContext.HR_Candidate_InterviewStatusDMO.Where(t => t.HRCD_Id == dto.HRCD_Id && t.IVRMUL_Id == dto.IVRMUL_Id && t.HRCIS_Datetime == dto.HRCIS_Datetime).Count();
                if (duplicatecountresult == 0)
                {
                    if (dmoObj.HRCIS_Id > 0)
                    {
                        var result = _VMSContext.HR_Candidate_InterviewStatusDMO.Single(t => t.HRCIS_Id == dmoObj.HRCIS_Id);
                        dto.UpdatedDate = DateTime.Now;

                        Mapper.Map(dto, result);
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                    else
                    {
                        dmoObj.HRCIS_ActiveFlg = true;
                        dmoObj.HRCD_Id = dto.HRCD_Id;
                        dmoObj.IVRMUL_Id = dto.HRCISC_UpdatedBy;
                        dmoObj.HRCIS_InterviewFeedBack = dto.HRCIS_InterviewFeedBack;
                        dmoObj.HRCIS_Datetime = dto.HRCIS_Datetime;
                        dmoObj.HRCIS_Status = dto.HRCISC_Status;
                        dmoObj.HRCISC_CreatedBy = dto.HRCISC_UpdatedBy;
                        dmoObj.HRCISC_UpdatedBy = dto.HRCISC_UpdatedBy;
                        dmoObj.HRCMG_Id = dto.HRCMG_Id;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
                        _VMSContext.Add(dmoObj);
                        var flag = _VMSContext.SaveChanges();
                        if (flag == 1)
                        {
                            dto.retrunMsg = "Add";
                            SendEmailFeedback(dto, "Interview Feedback", "Interview Feedback", dto.MI_Id);
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }

                    if (OBJDMO.HRCISC_Id > 0)
                    {
                        var result = _VMSContext.HR_CandidateInterviewScheduleDMO.Single(t => t.HRCISC_Id == OBJDMO.HRCISC_Id);
                        dto.UpdatedDate = DateTime.Now;
                        dto.HRCISC_ActiveFlg = true;
                        dto.HRCISC_Status = dto.HRCISC_Status;
                        dto.HRCMG_Id = dto.HRCMG_Id;
                        Mapper.Map(dto, result);
                        _VMSContext.Update(result);
                        var flag = _VMSContext.SaveChanges();
                        if (flag > 0)
                        {
                            dto.retrunMsg = "Update";
                        }
                        else
                        {
                            dto.retrunMsg = "false";
                        }
                    }
                }
                else
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public string sendmail(long MI_Id, string Email, string Template, long UserID, string mailcc)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _Context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _Context.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _Context.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "SMSMAILPARAMETER";
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
                            //result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailmsg = result;
                        }
                    }
                }
                Mailmsg = result;

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {
                            //Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _Context.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {

                        string mailbcc = "";

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
                            var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                            string[] ccmaildetails = mailcc.Split(',');

                            foreach (var c in ccmaildetails)
                            {
                                if (c != Email)
                                {
                                    message.AddCc(c);
                                }
                            }
                        }

                        if (mailbcc != null && mailbcc != "")
                        {
                            string[] bccmaildetails = mailbcc.Split(',');

                            foreach (var c in bccmaildetails)
                            {
                                if (c != Email)
                                {
                                    message.AddBcc(c);
                                }
                            }
                        }

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
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
                                    var img = _Context.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

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


                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
                            Value = Mailcontent
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

        public void SendEmail(HR_CandidateInterviewScheduleDTO obj, string subject, string body, long id)
        {
            try
            {
                var candidatedetails = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == obj.HRCD_Id).FirstOrDefault();
                string mailid = candidatedetails.HRCD_EmailId;

                string mailcc = "", mailcc1 = "", mailcc2 = "";

                mailcc1 = (from a in _VMSContext.IVRM_Staff_User_Login
                           from c in _VMSContext.Multiple_Email_DMO
                           where (a.Id == obj.HRCISC_UpdatedBy && a.Emp_Code == c.HRME_Id && c.HRMEM_DeFaultFlag == "default")
                           select c.HRMEM_EmailId).FirstOrDefault();

                mailcc2 = (from a in _VMSContext.IVRM_Staff_User_Login
                           from c in _VMSContext.Multiple_Email_DMO
                           where (a.Id == obj.HRCISC_Interviewer && a.Emp_Code == c.HRME_Id && c.HRMEM_DeFaultFlag == "default")
                           select c.HRMEM_EmailId).FirstOrDefault();
                if(mailcc1!="" && mailcc1 != null)
                {
                    mailcc = mailcc1;
                }
                if (mailcc2 != "" && mailcc2 != null && mailcc!="")
                {
                    mailcc = mailcc + "," + mailcc2;
                }
                else if (mailcc2 != "" && mailcc2 != null && mailcc == "")
                {
                    mailcc =mailcc2;
                }
               


                sendmail(id, mailid, "Interview_Notification", obj.HRCISC_Id, mailcc);

                //Dictionary<string, string> val = new Dictionary<string, string>();
                //var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Interview_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                //var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                //string Mailmsg = body;

                //List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                //alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                //if (alldetails.Count > 0)
                //{
                //    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                //    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                //    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                //    //string mailcc = alldetails[0].IVRM_mailcc;
                //    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                //    string Subject = subject;
                //    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                //    {
                //        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                //    }

                //    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                //    var message = new SendGridMessage();
                //    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                //    message.Subject = Subject;

                //    if (mailcc != null && mailcc != "")
                //    {
                //        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                //        if (mail_id.Length > 0)
                //        {
                //            for (int i = 0; i < mail_id.Length; i++)
                //            {
                //                message.AddBcc(mail_id[i]);
                //            }
                //            if(mailcc1 != null) { message.AddBcc(mailcc1); }
                //            if (mailcc2 != null) { message.AddBcc(mailcc2); }
                //        }
                //    }

                //    message.AddTo(mailid);

                //    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                //    {
                //        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                //        message.HtmlContent = message.HtmlContent.Replace("[NAME]", candidatedetails.HRCD_FirstName + " " + (candidatedetails.HRCD_MiddleName == null ? "" : candidatedetails.HRCD_MiddleName) + " " + (candidatedetails.HRCD_LastName == null ? "" : candidatedetails.HRCD_LastName));
                //        message.HtmlContent = message.HtmlContent.Replace("[VENUE]", obj.HRCISC_InterviewVenue);
                //        message.HtmlContent = message.HtmlContent.Replace("[DATE]", obj.HRCISC_InterviewDateTime.ToString("dd/MM/yyyy"));
                //    }
                //    else
                //    {
                //        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                //    }

                //    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                //    {
                //        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                //        client.SendEmailAsync(message).Wait();
                //    }
                //    else
                //    {
                //        // return "Sendgrid key is not available";
                //    }

                //    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                //    {
                //        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("Interview_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                //        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                //        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                //        cmd.CommandText = "IVRM_Email_Outgoing_1";
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add(new SqlParameter("@EmailId",SqlDbType.NVarChar)
                //        {
                //            Value = mailid
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@Message",SqlDbType.NVarChar)
                //        {
                //            Value = Mailmsg
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@module",SqlDbType.VarChar)
                //        {
                //            Value = subject
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                //        {
                //            Value = id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@type",SqlDbType.VarChar)
                //        {
                //            Value = "Staff"
                //        });

                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();
                //        try
                //        {
                //            using (var dataReader = cmd.ExecuteReader())
                //            {
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            //return ex.Message;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendEmailFeedback(HR_CandidateInterviewScheduleDTO obj, string subject, string body, long id)
        {
            try
            {
                var candidatedetails = _VMSContext.HR_Candidate_DetailsDMO.Where(t => t.HRCD_Id == obj.HRCD_Id).FirstOrDefault();

                var interviewername = (from a in _VMSContext.IVRM_Staff_User_Login
                                       from c in _VMSContext.HR_Master_Employee_DMO
                                       where (a.Id == obj.HRCISC_UpdatedBy && a.Emp_Code == c.HRME_Id)
                                       select new { c.HRME_EmployeeFirstName, c.HRME_EmployeeMiddleName, c.HRME_EmployeeLastName }).FirstOrDefault();

                string mailid = "hr@vapstech.com";
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("InterviewFeedback_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _Context.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _Context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = "hr@vapsknowledge.com";
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

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

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);
                        message.HtmlContent = message.HtmlContent.Replace("[NAME]", candidatedetails.HRCD_FirstName + " " + (candidatedetails.HRCD_MiddleName == null ? "" : candidatedetails.HRCD_MiddleName) + " " + (candidatedetails.HRCD_LastName == null ? "" : candidatedetails.HRCD_LastName));
                        message.HtmlContent = message.HtmlContent.Replace("[ROUND]", obj.HRCISC_InterviewRounds);
                        message.HtmlContent = message.HtmlContent.Replace("[STATUS]", obj.HRCISC_Status);
                        message.HtmlContent = message.HtmlContent.Replace("[FEEDBACK]", obj.HRCIS_InterviewFeedBack);
                        message.HtmlContent = message.HtmlContent.Replace("[INTERVIEWER]", interviewername.HRME_EmployeeFirstName + " " + interviewername.HRME_EmployeeMiddleName + " " + interviewername.HRME_EmployeeLastName);
                        message.HtmlContent = message.HtmlContent.Replace("[CSTATUS]", obj.HRCIS_CandidateStatus);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }

                    using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _Context.smsEmailSetting.Where(e => e.ISES_Template_Name.Equals("InterviewFeedback_Notification", StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _Context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _Context.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = "Staff"
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
                            //return ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
