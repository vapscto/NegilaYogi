using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
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
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class SMSReportImpl : Interfaces.SMSReportInterface
    {
        public ScheduleReportContext _SReportContext;
        public DomainModelMsSqlServerContext _SSReportContext;
        public SMSReportImpl(ScheduleReportContext DomainModelContext, DomainModelMsSqlServerContext DomainModelContext1)
        {
            _SReportContext = DomainModelContext; _SSReportContext = DomainModelContext1;
        }

        public SMSReportDTO getdetails(SMSReportDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _SReportContext.AcademicYear.Where(t => t.MI_Id == data.mid && t.Is_Active == true).ToList();
                data.yeardropDown = year.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<SMSReportDTO> Getreportdetails(SMSReportDTO data)
        {
            List<SMSReportDTO> AllInOne = new List<SMSReportDTO>();
            List<SMSReportDTO> AllInOne1 = new List<SMSReportDTO>();
            List<SMSReportDTO> stulist = new List<SMSReportDTO>();
          
            SMSReportDTO temp1 = new SMSReportDTO();
            try
            {

                List<SMSReportDTO> result = new List<SMSReportDTO>();
                List<SMSReportDTO> result1 = new List<SMSReportDTO>();
                // for sms  count
                using (var cmd = _SReportContext.Database.GetDbConnection().CreateCommand())
                {
                    //changed by suryan
                    cmd.CommandText = "GET_SMS_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.Int) { Value = data.asmayid });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = data.from_date });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime) { Value = data.to_date });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = data.mid });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new SMSReportDTO
                                {
                                    Name = (dataReader["Name"]).ToString(),
                                    smscount = (dataReader["SMSCount"]).ToString(),
                                    emailcount = (dataReader["EMAILCount"]).ToString()
                                });
                                data.sms_listarray = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // for email  count
                using (var cmd = _SReportContext.Database.GetDbConnection().CreateCommand())
                {
                    //changed by suryan
                    cmd.CommandText = "GET_EMAIL_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.Int) { Value = data.asmayid });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = data.from_date });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime) { Value = data.to_date });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = data.mid });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result1.Add(new SMSReportDTO
                                {
                                    Name = (dataReader["Name"]).ToString(),
                                    smscount = (dataReader["SMSCount"]).ToString(),
                                    emailcount = (dataReader["EMAILCount"]).ToString()
                                });
                                data.mial_listarray = result1.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                if (data.sms_listarray.Count() > 0)
                {
                  
                    for (int i = 0; i < data.sms_listarray.Count(); i++)
                    {
                        SMSReportDTO temp = new SMSReportDTO();
                        int where = 0;
                        temp.Name = data.sms_listarray[i].Name;
                        temp.smscount = data.sms_listarray[i].smscount;
                        if (data.mial_listarray.Count() > 0)
                        {
                            for (int j = 0; j < data.mial_listarray.Count(); j++)
                            {
                                if (where == 0)
                                {
                                    if (data.sms_listarray[i].Name == data.mial_listarray[j].Name)
                                    {
                                        temp.emailcount = data.mial_listarray[j].emailcount;
                                        where = 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            temp.emailcount = "0";

                        }
                        if(where==0)
                        {
                            temp.emailcount = "0";
                        }                       
                        AllInOne.Add(temp);
                    }

                    for (int i = 0; i < data.mial_listarray.Count(); i++)
                    {
                        SMSReportDTO temp = new SMSReportDTO();
                        int where = 0;
                        temp.Name = data.mial_listarray[i].Name;
                        temp.emailcount = data.mial_listarray[i].emailcount;
                        if (data.sms_listarray.Count() > 0)
                        {
                            for (int j = 0; j < data.sms_listarray.Count(); j++)
                            {
                                if (where == 0)
                                {
                                    if (data.mial_listarray[i].Name == data.sms_listarray[j].Name)
                                    {
                                        temp.smscount = data.sms_listarray[j].smscount;
                                        where = 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            temp.smscount = "0";

                        }
                        if (where == 0)
                        {
                            temp.smscount = "0";
                            AllInOne.Add(temp);
                        }
                       
                    }
                }
                else
                {
                    for (int i = 0; i < data.mial_listarray.Count(); i++)
                    {
                        SMSReportDTO temp = new SMSReportDTO();
                        temp.Name = data.mial_listarray[i].Name;
                        temp.smscount = data.mial_listarray[i].smscount;
                        temp.emailcount = data.mial_listarray[i].emailcount;
                        AllInOne.Add(temp);
                    }
                }

                data.sms_mial_listarray = AllInOne.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;

        }


        public async Task<SMSReportDTO>  smscreditschedular(SMSReportDTO data)
        {
            try
            {
                long credit = 0;
                long Rcredit = 0;
                string WORKINGKEY = "";
                var instiutionlist = _SReportContext.institution.Where(e => e.MI_Id == data.MI_Id).Distinct().ToList();

                if (instiutionlist.Count>0)
                {
                    if (instiutionlist.FirstOrDefault().MI_SMSAlertToemailids!=null && instiutionlist.FirstOrDefault().MI_SMSAlertToemailids !="")
                    {

                        if (instiutionlist.FirstOrDefault().MI_SMSCountAlert==null)
                        {
                            credit = 0;
                        }
                        else
                        {
                            credit = Convert.ToInt64 (instiutionlist.FirstOrDefault().MI_SMSCountAlert);
                        }
                     var   alldetails = _SSReportContext.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                        if (alldetails.Count>0)
                        {

                            if (alldetails.FirstOrDefault().IVRMSD_WORKINGKEY!=null && alldetails.FirstOrDefault().IVRMSD_WORKINGKEY !="")
                            {
                                WORKINGKEY = alldetails.FirstOrDefault().IVRMSD_WORKINGKEY;

                            
                                var url1 = "https://api-alerts.kaleyra.com/v4/?api_key=" + WORKINGKEY + "&method=account.credits";
                                System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                                System.Net.HttpWebResponse response1 = await request1.GetResponseAsync() as System.Net.HttpWebResponse;
                                Stream stream1 = response1.GetResponseStream();

                                StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                                string responseparameters1 = readStream1.ReadToEnd();
                                var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                                dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                                var statusdetails = JsonConvert.DeserializeObject<creditstausdata>(responsedata1);

                                if (statusdetails.status=="OK")
                                {
                                   var Rcredit1 = statusdetails.data.credits;

                                    Rcredit = Convert.ToInt64(Rcredit1);
                                    if (Rcredit<=credit)
                                    {

                                        List<string> emailidss = new List<string>(instiutionlist.FirstOrDefault().MI_SMSAlertToemailids.Split(','));
                                        emailidss.Reverse();

                                        if (emailidss.Count > 0)
                                        {
                                            foreach (var emailid in emailidss)
                                            {

                                                string s = sendmail(data.MI_Id, emailid, "SMSCREDITALERT", data.user_id, Rcredit);
                                            }

                                        }

                                    }

                                }
                                
                            }
                            
                        }

                    }
                }
            }
            catch (Exception ex )
            {

                throw ex;
            }

            return data;

        }
        
         public string sendmail(long MI_Id, string Email, string Template, long UserID, long credit)
        {
        try
        {
            Dictionary<string, string> val = new Dictionary<string, string>();

            var template = _SSReportContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

            if (template.Count == 0)
            {
                return "";
            }


            var institutionName = _SSReportContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

            var Paramaeters = _SSReportContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

            var ParamaetersName = _SSReportContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

            string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
            string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

            string Resultsms = Mailcontent;
            string result = Mailmsg;

            List<Match> variables = new List<Match>();

            foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
            {
                variables.Add(match);
            }

            if (Template == "EMAILOTP")
            {
                result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                Mailmsg = result;
                Mailcontent = result;
            }
            else
            {
                using (var cmd = _SSReportContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "SMSMAILPARAMETER_SMSCREDIT_ALERT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                    {
                        Value = UserID
                    });

                   cmd.Parameters.Add(new SqlParameter("@Credit",
                        SqlDbType.BigInt)
                    {
                        Value = credit
                   });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = MI_Id
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
            }

            List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
            alldetails = _SSReportContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                smstpdetails = _SSReportContext.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

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


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);

                    if (Attechement.Equals("1"))
                    {
                        var img = _SSReportContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                    // var client = new Web("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");

                    //if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    //{
                    //    message.AddAttachment(template.FirstOrDefault().ISES_MailHTMLTemplate);

                    //}
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
                                var img = _SSReportContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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


                                        //var attachment = new MimePart("image", "gif")
                                        //{
                                        //    ContentObject = new ContentObject(File.OpenRead(img[i].IVRM_Att_Path), ContentEncoding.Default),
                                        //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                        //    ContentTransferEncoding = ContentEncoding.Base64,
                                        //    FileName = Path.GetFileName(img[i].IVRM_Att_Path)
                                        //};
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


                using (var cmd = _SSReportContext.Database.GetDbConnection().CreateCommand())
                {
                    var template1010 = _SSReportContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template ).Select(d => d.IVRMIM_Id).ToList();

                    var moduleid = _SSReportContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                    var modulename = _SSReportContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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
    }

   
    public class creditstausdata
    {
        public string status { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public string credits { get; set; }

       // public string data { get; set; }
       public credit data { get; set; }
    }

    public class credit
    {
        public string credits { get; set; }
      
    }

}
