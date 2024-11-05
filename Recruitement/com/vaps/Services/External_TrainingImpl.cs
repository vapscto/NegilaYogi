using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.Training;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VMS.Training;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Services
{
    public class External_TrainingImpl : Interfaces.External_TrainingInterface
    {
        public VMSContext _context;
        public DomainModelMsSqlServerContext _db;
        public External_TrainingImpl(VMSContext _con, DomainModelMsSqlServerContext Context)
        {
            _context = _con;
            _db = Context;
        }

        public External_TrainingDTO onloaddata(External_TrainingDTO data)
        {
            try
            {
                data.trainingcentername = _context.Master_External_TrainingCentersDMO.Where(a => a.HRMETRCEN_ActiveFlag == true && a.MI_Id == data.MI_Id).ToArray();
                data.externaltrainingtype = _context.Master_External_TrainingTypeDMO.Where(a => a.HRMETRTY_ActiveFlag == true && a.MI_Id == data.MI_Id).ToArray();
                data.participates_Employee_list = _context.HR_Master_Employee_DMO.Where(a => a.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id).ToArray();


                data.hrmeid = (from a in _context.ApplicationUserDMO
                               from b in _context.IVRM_Staff_User_Login
                               where (a.Id == b.Id && a.Id == data.Userid)
                               select b).Select(t => t.Emp_Code).FirstOrDefault();

                data.getloaddetails = (from a in _context.External_TrainingDMO
                                       from b in _context.Master_External_TrainingTypeDMO
                                       from c in _context.Master_External_TrainingCentersDMO
                                       from d in _context.HR_Master_Employee_DMO
                                       where (a.MI_Id == data.MI_Id && a.HRMETRTY_Id == b.HRMETRTY_Id && a.HRMETRCEN_Id == c.HRMETRCEN_Id && a.HRME_Id == d.HRME_Id)
                                       select new External_TrainingDTO
                                       {
                                           HREXTTRN_TrainingTopic = a.HREXTTRN_TrainingTopic,
                                           HRMETRTY_ExternalTrainingType = b.HRMETRTY_ExternalTrainingType,
                                           HRMETRCEN_TrainingCenterName = c.HRMETRCEN_TrainingCenterName,
                                           HREXTTRN_StartDate = a.HREXTTRN_StartDate,
                                           HREXTTRN_EndDate = a.HREXTTRN_EndDate,
                                           HREXTTRN_StartTime = a.HREXTTRN_StartTime,
                                           HREXTTRN_EndTime = a.HREXTTRN_EndTime,
                                           HREXTTRN_ActiveFlag = a.HREXTTRN_ActiveFlag,
                                           HREXTTRN_ApprovedFlg = a.HREXTTRN_ApprovedFlg,
                                           HRME_Id=a.HRME_Id,
                                           HRME_EmployeeFirstName = d.HRME_EmployeeFirstName,
                                           HRME_EmployeeMiddleName  = d.HRME_EmployeeMiddleName,
                                           HREXTTRN_Id = a.HREXTTRN_Id
                                       }).Distinct().OrderByDescending(t=>t.HREXTTRN_StartDate).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public External_TrainingDTO saverecord(External_TrainingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);


                List<long> employeeids = new List<long>();
                if (data.HREXTTRN_Id > 0)
                {

                    var checkduplicate = _context.External_TrainingDMO.Where(a => a.MI_Id == data.MI_Id && a.HREXTTRN_Id != data.HREXTTRN_Id && a.HRMETRCEN_Id == data.HRMETRCEN_Id 
                    && a.HRMETRTY_Id == data.HRMETRTY_Id && a.HREXTTRN_StartDate == data.HREXTTRN_StartDate && a.HREXTTRN_StartTime == data.HREXTTRN_StartTime && a.HREXTTRN_EndTime == data.HREXTTRN_EndTime 
                    && a.HREXTTRN_TotalHrs == data.HREXTTRN_TotalHrs).ToList();

                    data.hrmeid = (from a in _context.ApplicationUserDMO
                                   from b in _context.IVRM_Staff_User_Login
                                   where (a.Id == b.Id && a.Id == data.Userid)
                                   select b).Select(t => t.Emp_Code).FirstOrDefault();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        if (data.FilePath_Array != null)
                        {
                            for (int j = 0; j < data.FilePath_Array.Length; j++)
                            {

                                var checkresult = _context.External_TrainingDMO.Single(a => a.MI_Id == data.MI_Id && a.HREXTTRN_Id == data.HREXTTRN_Id);
                                checkresult.HREXTTRN_Id = data.HREXTTRN_Id;
                                checkresult.HRMETRCEN_Id = data.HRMETRCEN_Id;
                                checkresult.HRMETRTY_Id = data.HRMETRTY_Id;
                                checkresult.HRME_Id = data.hrmeid;
                                checkresult.HREXTTRN_TrainingTopic = data.FilePath_Array[j].HREXTTRN_TrainingTopic;
                                checkresult.HREXTTRN_CertificateFileName = data.FilePath_Array[j].HREXTTRN_CertificateFileName;
                                checkresult.HREXTTRN_CertificateFilePath = data.FilePath_Array[j].HREXTTRN_CertificateFilePath;
                                checkresult.HREXTTRN_StartDate = data.HREXTTRN_StartDate;
                                checkresult.HREXTTRN_StartTime = data.HREXTTRN_StartTime;
                                checkresult.HREXTTRN_EndTime = data.HREXTTRN_EndTime;
                                checkresult.HREXTTRN_TotalHrs = data.HREXTTRN_TotalHrs;
                                checkresult.HREXTTRN_UpdatedBy = data.Userid;
                                checkresult.HREXTTRN_UpdatedDate = indiantime0;
                                _context.Update(checkresult);
                            }

                        }


                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }
                }
                else
                {


                    var checkduplicate = _context.External_TrainingDMO.Where(a => a.MI_Id == data.MI_Id && a.HREXTTRN_Id == data.HREXTTRN_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        if (data.FilePath_Array != null)
                        {
                            data.hrmeid = (from a in _context.ApplicationUserDMO
                                           from b in _context.IVRM_Staff_User_Login
                                           where (a.Id == b.Id && a.Id == data.Userid)
                                           select b).Select(t => t.Emp_Code).FirstOrDefault();
                            for (int j = 0; j < data.FilePath_Array.Length; j++)
                            {
                                External_TrainingDMO obj = new External_TrainingDMO();

                                obj.MI_Id = data.MI_Id;
                                obj.HRMETRCEN_Id = data.HRMETRCEN_Id;
                                obj.HRMETRTY_Id = data.HRMETRTY_Id;
                                obj.HREXTTRN_StartDate = data.HREXTTRN_StartDate;
                                obj.HREXTTRN_StartTime = data.HREXTTRN_StartTime;
                                obj.HREXTTRN_EndDate = data.HREXTTRN_EndDate;
                                obj.HREXTTRN_EndTime = data.HREXTTRN_EndTime;
                                obj.HREXTTRN_TotalHrs = data.HREXTTRN_TotalHrs;
                                obj.HREXTTRN_TrainingTopic = data.FilePath_Array[j].HREXTTRN_TrainingTopic;
                                obj.HREXTTRN_CertificateFileName = data.FilePath_Array[j].HREXTTRN_CertificateFileName;
                                obj.HREXTTRN_CertificateFilePath = data.FilePath_Array[j].HREXTTRN_CertificateFilePath;
                                obj.HREXTTRN_ActiveFlag = true;
                                obj.HREXTTRN_ApprovedFlg = "Pending";
                                obj.HREXTTRN_CreatedBy = data.Userid;
                                obj.HREXTTRN_UpdatedBy = data.Userid;
                                obj.HREXTTRN_CreatedDate = indiantime0;
                                obj.HREXTTRN_UpdatedDate = indiantime0;
                                obj.HRME_Id = data.HRME_Id;
                                _context.Add(obj);
                            }
                        }
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }


               
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public External_TrainingDTO Edit(External_TrainingDTO dto)
        {
            try
            {
                dto.editdata = _context.External_TrainingDMO.Where(t => t.MI_Id == dto.MI_Id && t.HREXTTRN_Id == dto.HREXTTRN_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public External_TrainingDTO deactiveY(External_TrainingDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
           
                    var resultdeactive = _context.External_TrainingDMO.Single(a => a.HREXTTRN_Id == data.HREXTTRN_Id);

                    if (resultdeactive.HREXTTRN_ActiveFlag == true)
                    {
                        resultdeactive.HREXTTRN_ActiveFlag = false;
                    }
                    else
                    {
                        resultdeactive.HREXTTRN_ActiveFlag = true;
                    }

                    resultdeactive.HREXTTRN_UpdatedDate = indiantime0;
                    resultdeactive.HREXTTRN_UpdatedBy = data.Userid;
                    _context.Update(resultdeactive);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public string sendEmail(long MI_Id, string Template, long HRME_ID, string email, string ccemail)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _context.SMSEmailSetting.Where(e => e.ISES_Template_Name == Template
                && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _context.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;


                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HR_Email_Prametars_Replace";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.VarChar)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_IDE",
                SqlDbType.VarChar)
                    {
                        Value = HRME_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
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

                                    else if (datatype.Name == "Decimal")
                                    {
                                        var dateval = (Convert.ToDecimal(dataReader[iFiled])).ToString();
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

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    for (int p = 0; p < val.Count; p++)
                    {
                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        {


                            Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            Mailcontent = Resultsms;
                        }
                    }
                }
                Mailcontent = Resultsms;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _context.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == 17).ToList();

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
                    smstpdetails = _context.GeneralConfigDMO.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                    string mailcc = "";
                    string mailbcc = "";

                    //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    //{
                    //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                    //    mailcc = ccmail[0].ToString();

                    //    if (ccmail.Length > 1)
                    //    {
                    //        if (ccmail[1] != null || ccmail[1] != "")
                    //        {
                    //            mailbcc = ccmail[1].ToString();
                    //        }
                    //    }

                    //}
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }



                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(email);

                 

                  
                    //****************** EMAIL CC DETAILS ***************//

                    if (ccemail != null && ccemail != "")
                    {
                        string[] ccmaildetails = ccemail.Split(',');

                        foreach (var c in ccmaildetails)
                        {
                            message.AddCc(c);
                        }
                    }


                    //if (mailcc != null && mailcc != "")
                    //{
                    //    string[] ccmaildetails = mailcc.Split(',');

                    //    foreach (var c in ccmaildetails)
                    //    {
                    //        message.AddCc(c);
                    //    }
                    //}

                    //if (ccemail != null && ccemail != "")
                    //{
                    //    message.AddBcc(ccemail);
                    //}

                    //****************** EMAIL BCC DETAILS ***************//




                    if (mailcc != null && mailcc != "")
                    {
                        //message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }

                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    try
                    {
                        client.SendEmailAsync(message).Wait();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }



                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _context.SMSEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).Select(d => d.IVRMIM_Id).ToArray();

                        var moduleid = _context.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = email
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
