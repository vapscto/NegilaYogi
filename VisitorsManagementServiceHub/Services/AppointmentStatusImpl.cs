using AutoMapper;
using CommonLibrary;
//using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
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
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisitorsManagementServiceHub.Interfaces;


namespace VisitorsManagementServiceHub.Services
{
    public class AppointmentStatusImpl : AppointmentStatusInterface
    {
        public VisitorsManagementContext visctxt;
        public DomainModelMsSqlServerContext _db;
        public AppointmentStatusImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext ctxt)
        {
            visctxt = context;
            _db = ctxt;
        }

        public AppointmentStatusDTO getDetails(AppointmentStatusDTO data)
        {
            try
            {
                data.FloreList = visctxt.HR_Master_Floor_DMO.Where(b => b.MI_Id == Convert.ToInt64(data.MI_Id)).Distinct().ToArray();

                data.visitorlist = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                    from b in visctxt.Visitor_Management_Visitor_Appointment_DMO
                                        /* from c in visctxt.HR_Master_Floor_DMO*/
                                    where (a.MI_Id == b.MI_Id && a.VMMV_Id == b.VMMV_Id && a.MI_Id == data.MI_Id
                                    /*&& a.MI_Id==c.MI_Id&&b.MI_Id==c.MI_Id */
                                    && a.MI_Id == data.MI_Id)
                                    select new AppointmentStatusDTO
                                    {
                                        VMMV_Id = a.VMMV_Id,
                                        VMMV_VisitorName = a.VMMV_VisitorName,
                                        //HLMF_Id = c.HLMH_Id,
                                        //HRMF_FloorName = c.HRMF_FloorName
                                    }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public AppointmentStatusDTO EditDetails(AppointmentStatusDTO id)
        {
            AppointmentStatusDTO resp = new AppointmentStatusDTO();
            try
            {

                var editData = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                from b in visctxt.Visitor_Management_Visitor_Appointment_DMO
                                from e in visctxt.MasterEmployee
                                where (a.MI_Id == b.MI_Id && a.VMMV_Id == b.VMMV_Id && e.HRME_Id == a.VMMV_ToMeet && a.MI_Id == id.MI_Id && a.VMMV_Id == id.VMMV_Id)
                                select new AppointmentStatusDTO
                                {
                                    VMMV_Id = a.VMMV_Id,
                                    VMMV_VisitorName = a.VMMV_VisitorName,
                                    empname = e.HRME_EmployeeFirstName + (string.IsNullOrEmpty(e.HRME_EmployeeMiddleName) ? "" : ' ' + e.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(e.HRME_EmployeeLastName) ? "" : ' ' + e.HRME_EmployeeLastName),
                                    VMMV_MeetingDateTime = a.VMMV_MeetingDateTime,
                                    VMMV_Remarks = a.VMMV_Remarks,
                                    VMMV_EntryDateTime = a.VMMV_EntryDateTime,

                                }).Distinct().ToList();
                resp.editDetails = editData.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return resp;
        }
        public async Task<AppointmentStatusDTO> saveDataAsync(AppointmentStatusDTO data)
        {
            try
            {
                if (data.VMMV_Id > 0)
                {
                    var update = visctxt.Visitor_Management_MasterVisitor_DMO.Single(d => d.VMMV_Id == data.VMMV_Id && d.MI_Id == data.MI_Id);
                    update.UpdatedDate = DateTime.Now;
                    update.VMMV_ExitDate = data.VMMV_ExitDate;
                    update.VMMV_ExitDateTime = data.VMMV_ExitDateTime;
                    update.VMMV_CkeckedInOutStatus = data.VMMV_CkeckedInOutStatus;

                    update.VMMV_UpdatedBy = data.UserId;
                    visctxt.Update(update);
                    int s = visctxt.SaveChanges();
                    if (s > 0)
                    {
                        data.returnVal = "updated";
                        string visitorname = "";
                        var contactno = visctxt.Visitor_Management_MasterVisitor_DMO.Single(t => t.VMMV_Id == data.VMMV_Id).VMMV_VisitorContactNo;
                        var mailId = visctxt.Visitor_Management_MasterVisitor_DMO.Single(t => t.VMMV_Id == data.VMMV_Id).VMMV_VisitorEmailid;

                        var name = (from a in visctxt.Visitor_Management_MasterVisitor_DMO
                                    where (a.VMMV_Id == data.VMMV_Id)
                                    select new AppointmentStatusDTO { VMMV_VisitorName = a.VMMV_VisitorName }).Distinct().SingleOrDefault();

                        visitorname = name.VMMV_VisitorName.ToString();

                        var Template = "VisitorStatus";

                        if (contactno != 0)
                        {
                            //SMS sms = new SMS(_db);
                            //sendSms(data.MI_Id, contactno, Template, data.VMMV_Id);
                            if (update.VMMV_CkeckedInOutStatus == "Checked Out")
                            {
                                string smsval = await sendSms(data.MI_Id, contactno, Template, visitorname);
                            }
                        }
                        else
                        {

                        }
                        if (mailId != "" && mailId != null)
                        {
                            if (update.VMMV_CkeckedInOutStatus == "Checked Out")
                            {
                                sendmail(data.MI_Id, mailId, Template, visitorname);
                            }
                        }
                    }
                    else
                    {
                        data.returnVal = "updateFailed";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, string visitorname)
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

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, visitorname);
                        sms = result;
                    }
                }
                sms = result;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    //PHNO = "9581586484";
                    url = url.Replace("PHNO", PHNO);
                    //PHNO = "9581586484";
                    url = url.Replace("MESSAGE", sms);
                    url = url.Replace("entityid", institutionName[0].MI_EntityId.ToString());
                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    var remoteIpAddress = "";
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;
                    remoteIpAddress = addr[addr.Length - 1].ToString();

                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                    string myIP1 = addr[addr.Length - 2].ToString();

                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    String sMacAddress = string.Empty;
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (sMacAddress == String.Empty)// only return MAC Address from first card
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                        }
                    }

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Visitors Management";
                    dmo2.To_FLag = "VISITOR";
                    dmo2.UpdatedDate = DateTime.Now;
                    dmo2.System_Ip = remoteIpAddress;
                    dmo2.MacAddress_Ip = sMacAddress;
                    dmo2.network_Ip = myIP1;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = "Delivered";
                    }

                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return ex.Message;
            }
            return "success";
        }
        public string sendmail(long MI_Id, string Email, string Template, string visitorname)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string result_value = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, visitorname);
                        Mailmsg = result_value;
                    }
                }
                Mailmsg = result_value;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        mailcc = alldetails[0].IVRM_mailcc.ToString();
                    }
                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.
                    try
                    {
                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        //VMMV_VisitorEmailid = "amanrce@gmail.com";
                        message.AddTo(Email);

                        message.HtmlContent = Mailmsg;

                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();

                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "Visitor" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "Visitor_IVRM_Email_Outgoing";
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
                        cmd.Parameters.Add(new SqlParameter("@To_FLag",
                        SqlDbType.VarChar)
                        {
                            Value = "VISITOR"
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
                //Mails Sending end

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return "success";
        }


    }
}
