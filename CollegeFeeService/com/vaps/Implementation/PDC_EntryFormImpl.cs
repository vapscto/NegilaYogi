using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Admission;
using Microsoft.Extensions.Logging;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using System.Collections.Concurrent;
using DomainModel.Model.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Fees;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Net;
using paytm.security;
using paytm.util;
using paytm.exception;
using Razorpay.Api;
using System.Text;
using PreadmissionDTOs.com.vaps.College.Fee;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using PreadmissionDTOs.com.vaps.College.Fees;
using DomainModel.Model.com.vapstech.College.Fees;
using System.Net.Mail;
using System.Text.RegularExpressions;
using DomainModel.Model;
using SendGrid.Helpers.Mail;
using SendGrid;
using AutoMapper;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class PDC_EntryFormImpl : Interfaces.PDC_EntryFormInterface
    {
        private static ConcurrentDictionary<string, FeeGroupClgDTO> _login =
 new ConcurrentDictionary<string, FeeGroupClgDTO>();

        private static ConcurrentDictionary<string, FeeYearlyGroupClgDTO> _login1 =
             new ConcurrentDictionary<string, FeeYearlyGroupClgDTO>();

        public CollFeeGroupContext _FeeGroupContext;
        readonly ILogger<PDC_EntryFormImpl> _logger;
        public PDC_EntryFormImpl(CollFeeGroupContext frgContext, ILogger<PDC_EntryFormImpl> log)
        {
            _FeeGroupContext = frgContext;
            _logger = log;
        }

        public PDC_EntryFormDTO SaveGroupData(PDC_EntryFormDTO FGpage)
        {

            // var exists = 0;
            bool returnresult = false;
            string retval = "";



            if (FGpage.savetmpdata.Length > 0)
            {
                // Fee_College_Studentwise_PDCDMO feepge = Mapper.Map<Fee_College_Studentwise_PDCDMO>(FGpage);

                // Fee_College_Studentwise_PDCDMO feepge = new Fee_College_Studentwise_PDCDMO();

                foreach (var std in FGpage.savetmpdata)
                {




                    var feegrplist = _FeeGroupContext.Fee_College_Studentwise_PDCDMO.Where(t => t.FCSPDC_Id == std.FCSPDC_Id).ToList();
                    if (feegrplist.Count > 0)
                    {
                        foreach (var stdfee in feegrplist)
                        {
                            _FeeGroupContext.Remove(stdfee);
                        }
                    }



                    Fee_College_Studentwise_PDCDMO feepge = new Fee_College_Studentwise_PDCDMO();
                    feepge.MI_Id = FGpage.MI_Id;
                    feepge.AMSE_Id = std.AMSE_Id;
                    feepge.AMCO_Id = FGpage.AMCO_Id;
                    feepge.AMB_Id = FGpage.AMB_Id;
                    feepge.FCSPDC_ActiveFlg = true;
                    feepge.FCSPDC_ChequeDate = std.FCSPDC_ChequeDate;
                    feepge.FCSPDC_CreatedBy = FGpage.user_id;
                    feepge.FCSPDC_Narration = std.FCSPDC_Narration;

                    feepge.FCSPDC_CreatedDate = DateTime.Now;
                    feepge.FCSPDC_UpdatedDate = DateTime.Now;
                    feepge.AMCST_Id = std.AMCST_Id;
                    feepge.FCSPDC_Currency = FGpage.FCSPDC_Currency;
                    feepge.FCSPDC_Updatedby = std.user_id;
                    feepge.FCSPDC_ChequeNo = std.FCSPDC_ChequeNo;
                    feepge.FCSPDC_Amount = std.FCSPDC_Amount;
                    feepge.FMBANK_Id = std.FMBANK_Id;
                    feepge.FCSPDC_Status = "Pending";
                    _FeeGroupContext.Add(feepge);







                }
            }

            var exists = _FeeGroupContext.SaveChanges();


            if (exists >= 1)
            {
                FGpage.returnduplicatestatus = "Save";
            }
            else
            {
                FGpage.returnduplicatestatus = "NotSave";
            }

            return FGpage;
        }
        public PDC_EntryFormDTO getdetails(PDC_EntryFormDTO FGRDT)
        {


            try
            {

                try
                {


                    FGRDT.Fillcourse = _FeeGroupContext.MasterCourseDMO.Where(t => t.MI_Id == FGRDT.MI_Id && t.AMCO_ActiveFlag == true).ToArray();
                    FGRDT.fillbranch = _FeeGroupContext.ClgMasterBranchDMO.Where(t => t.MI_Id == FGRDT.MI_Id && t.AMB_ActiveFlag == true).ToArray();
                    FGRDT.FillSemester = _FeeGroupContext.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == FGRDT.MI_Id && t.AMSE_ActiveFlg == true).ToArray();
                    FGRDT.FillCategory = _FeeGroupContext.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == FGRDT.MI_Id && t.AMCST_ActiveFlag == true).ToArray();

                    FGRDT.FillBank = _FeeGroupContext.Fee_Master_BankDMO.Where(t => t.MI_Id == FGRDT.MI_Id && t.FMBANK_ActiveFlg == true).ToArray();


                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SP_PDCDETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
            SqlDbType.VarChar)
                        {
                            Value = FGRDT.MI_Id

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
                            FGRDT.pdcrecord = retObject.ToArray();
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




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }

        public PDC_EntryFormDTO showdata(PDC_EntryFormDTO FGRDT)
        {


            try
            {

                try
                {


                    FGRDT.FillBank = _FeeGroupContext.Fee_Master_BankDMO.Where(t => t.MI_Id == FGRDT.MI_Id && t.FMBANK_ActiveFlg == true).ToArray();

                    var semid = _FeeGroupContext.Fee_College_Studentwise_PDCDMO.Where(t => t.AMCST_Id == FGRDT.AMCST_Id && t.FCSPDC_ActiveFlg == true).ToList();

                    //var amse_ids = "";
                    //for (var i = 0; i < semid.Count; i++)
                    //{
                    //    amse_ids += semid[i].AMSE_Id + ",";
                    //}
                    List<long> amse_ids = new List<long>();
                    if (semid.Count > 0)
                    {
                        foreach (var i in semid)
                        {
                            amse_ids.Add(i.AMSE_Id);
                        }
                    }
                    else
                    {
                        amse_ids.Add(0);
                    }

                    //for (var i=0;i<semid.Count;i++)
                    //    {
                    //        amse_ids += semid[i].AMSE_Id + ",";
                    //    }

                    // amse_ids = amse_ids.Substring(0, (amse_ids.Length - 1));

                    FGRDT.admsudentslist = (from a in _FeeGroupContext.Adm_College_Yearly_StudentDMO
                                            from b in _FeeGroupContext.Adm_Master_College_StudentDMO
                                            from c in _FeeGroupContext.CLG_Adm_Master_SemesterDMO
                                            where (a.ASMAY_Id == FGRDT.ASMAY_Id && b.MI_Id == FGRDT.MI_Id && b.AMCST_ActiveFlag == true &&
                                            a.AMCO_Id == FGRDT.AMCO_Id && a.AMB_Id == FGRDT.AMB_Id && a.ACYST_ActiveFlag == 1
                                            && b.AMCST_SOL == "S" && a.AMCST_Id == b.AMCST_Id && a.AMCST_Id == FGRDT.AMCST_Id && !amse_ids.Contains(c.AMSE_Id))
                                            select new PDC_EntryFormDTO
                                            {
                                                AMCST_Id = b.AMCST_Id,
                                                AMCST_FirstName = ((b.AMCST_FirstName == null ? " " : b.AMCST_FirstName) + " " + (b.AMCST_MiddleName == null ? " " : b.AMCST_MiddleName) + " " + (b.AMCST_LastName == null ? " " : b.AMCST_LastName)).Trim(),
                                                AMSE_Id = c.AMSE_Id,
                                                AMSE_SEMName = c.AMSE_SEMName,
                                                FCSPDC_ChequeDate = DateTime.Now,
                                            }).Distinct().OrderBy(d => d.AMCST_FirstName).ToArray();


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public PDC_EntryFormDTO EditgroupDetails(int id)
        {
            PDC_EntryFormDTO FMG = new PDC_EntryFormDTO();
            try
            {
                List<Fee_College_Studentwise_PDCDMO> masterfeegroup = new List<Fee_College_Studentwise_PDCDMO>();
                masterfeegroup = _FeeGroupContext.Fee_College_Studentwise_PDCDMO.AsNoTracking().Where(t => t.FCSPDC_Id.Equals(id)).ToList();

                FMG.GroupData = masterfeegroup.ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public PDC_EntryFormDTO GetGroupSearchData(PDC_EntryFormDTO mas)
        {

            PDC_EntryFormDTO FGRDT = new PDC_EntryFormDTO();
            try
            {
                List<Fee_College_Studentwise_PDCDMO> feegrp = new List<Fee_College_Studentwise_PDCDMO>();
                feegrp = _FeeGroupContext.Fee_College_Studentwise_PDCDMO.ToList();
                FGRDT.GroupData = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }
        public PDC_EntryFormDTO getpageedit(int id)
        {
            PDC_EntryFormDTO page = new PDC_EntryFormDTO();
            try
            {
                List<Fee_College_Studentwise_PDCDMO> lorg = new List<Fee_College_Studentwise_PDCDMO>();
                page.GroupData = (from a in _FeeGroupContext.Fee_College_Studentwise_PDCDMO
                                  from b in _FeeGroupContext.Adm_Master_College_StudentDMO
                                  from c in _FeeGroupContext.CLG_Adm_Master_SemesterDMO
                                  where (a.AMCST_Id == b.AMCST_Id && a.FCSPDC_ActiveFlg == true && b.AMCST_ActiveFlag == true && a.FCSPDC_Id.Equals(id) && a.AMSE_Id == c.AMSE_Id)
                                  select new PDC_EntryFormDTO
                                  {
                                      AMCST_Id = b.AMCST_Id,
                                      AMCST_FirstName = ((b.AMCST_FirstName == null ? " " : b.AMCST_FirstName) + " " + (b.AMCST_MiddleName == null ? " " : b.AMCST_MiddleName) + " " + (b.AMCST_LastName == null ? " " : b.AMCST_LastName)).Trim(),
                                      FCSPDC_Id = a.FCSPDC_Id,
                                      FCSPDC_Amount = a.FCSPDC_Amount,
                                      FCSPDC_ChequeDate = a.FCSPDC_ChequeDate,
                                      FCSPDC_Narration = a.FCSPDC_Narration,
                                      FCSPDC_Currency = a.FCSPDC_Currency,
                                      FCSPDC_ChequeNo = a.FCSPDC_ChequeNo,
                                      FCSPDC_Status = a.FCSPDC_Status,
                                      FCSPDC_ActiveFlg = a.FCSPDC_ActiveFlg,
                                      MI_Id = a.MI_Id,
                                      FMBANK_Id = a.FMBANK_Id,
                                      AMCO_Id = a.AMCO_Id,
                                      AMB_Id = a.AMB_Id,
                                      AMSE_Id = a.AMSE_Id,
                                      AMSE_SEMName = c.AMSE_SEMName,

                                  }).ToArray();


                //  page.GroupData = lorg.ToArray();

                page.FillBank = _FeeGroupContext.Fee_Master_BankDMO.Where(t => t.FMBANK_ActiveFlg == true).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public PDC_EntryFormDTO deactivate(PDC_EntryFormDTO acd)
        {
            try
            {
                Fee_College_Studentwise_PDCDMO feepge = Mapper.Map<Fee_College_Studentwise_PDCDMO>(acd);
                if (feepge.FCSPDC_Id > 0)
                {



                    var result = _FeeGroupContext.Fee_College_Studentwise_PDCDMO.Single(t => t.FCSPDC_Id == feepge.FCSPDC_Id);
                    result.FCSPDC_Updatedby = acd.user_id;

                    if (result.FCSPDC_ActiveFlg == true)
                    {
                        result.FCSPDC_ActiveFlg = false;
                        acd.confirmmgs = "Deactivated";


                    }
                    else
                    {
                        result.FCSPDC_ActiveFlg = true;
                        acd.confirmmgs = "Activated";
                    }
                    _FeeGroupContext.Update(result);

                    var flag = _FeeGroupContext.SaveChanges();
                    acd.returnval = "true";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);

            }
            return acd;
        }


        public PDC_EntryFormDTO PDCRemainder(PDC_EntryFormDTO data)
        {
            List<PDC_EntryFormDTO> devicelist = new List<PDC_EntryFormDTO>();
            List<PDC_EntryFormDTO> deviceidsnew = new List<PDC_EntryFormDTO>();
            try
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PDCRemainderemail";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                                devicelist.Add(new PDC_EntryFormDTO
                                {

                                    AMCST_MobileNo = Convert.ToInt64(dataReader["AMCST_MobileNo"]),
                                    AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                    AMCST_emailId = Convert.ToString(dataReader["AMCST_emailId"]),
                                    //AMCST_FirstName = Convert.ToString(dataReader["AMCST_FirstName"]),
                                    //FCSPDC_ChequeDate = Convert.ToDateTime(dataReader["FCSPDC_ChequeDate"]),
                                    //FCSPDC_Amount = Convert.ToDecimal(dataReader["FCSPDC_Amount"]),
                                    //AMSE_Id = Convert.ToInt64(dataReader["AMSE_Id"]),
                                    //FCSPDC_ChequeNo = Convert.ToString(dataReader["FCSPDC_ChequeNo"]),

                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                }

                int k = 0;
                foreach (var deviceid in devicelist)
                {
                    string m = sendmail(deviceid.MI_Id, deviceid.AMCST_emailId, "PDCRemainder", deviceid.AMCST_Id);
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }




        public string sendmail(long MI_Id, string Email, string Template, long AMCST_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _FeeGroupContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _FeeGroupContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _FeeGroupContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _FeeGroupContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

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
                    // result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    //Mailmsg = result;
                    //Mailcontent = result;
                }
                else
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "PDCRemainders";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                     SqlDbType.VarChar)
                        {
                            Value = AMCST_Id
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
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _FeeGroupContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _FeeGroupContext.GenConfiguration.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "SMTP")
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
                            var img = _FeeGroupContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                                    var img = _FeeGroupContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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


                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _FeeGroupContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _FeeGroupContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _FeeGroupContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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


        public College_Student_SettlementDTO Settlement_data(College_Student_SettlementDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                //DateTime indianTime = TimeZoneInfo.ConvertTime(data.Selected_Date, INDIAN_ZONE);

                List<College_Student_SettlementDTO> PAYMENTPARAMDETAILS = new List<College_Student_SettlementDTO>();
                PAYMENTPARAMDETAILS = (from a in _FeeGroupContext.PAYUDETAILS
                                       where (a.IMPG_Id == data.IMPG_Id)

                                       select new College_Student_SettlementDTO
                                       {
                                           IMPG_PGFlag = a.IMPG_PGFlag,
                                           IMPG_SettlementURL = a.IMPG_SettlementURL
                                       }
           ).ToList();


                //var SubMerchantKey = _FeeGroupContext.Fee_PaymentGateway_Details.Single(t => t.MI_Id == data.MI_Id && t.FPGD_Id == data.FPGD_Id).FPGD_SubMerchantKey;
                List<College_Student_SettlementDTO> paymentdet = new List<College_Student_SettlementDTO>();
                paymentdet = (from a in _FeeGroupContext.Fee_PaymentGateway_Details
                              where (a.MI_Id == data.MI_Id && a.FPGD_Id == data.FPGD_Id)
                              select new College_Student_SettlementDTO
                              {
                                  MID = a.FPGD_MerchantId,
                                  FPGD_SubMerchantKey = a.FPGD_SubMerchantKey,
                                  FPGD_AuthorizationHeader = a.FPGD_AuthorizationHeader,
                                  FPGD_AuthorisationKey = a.FPGD_AuthorisationKey,
                                  FPGD_MerchantId = a.FPGD_MerchantId,
                              }
           ).ToList();

                //var AuthorizationHeader = _FeeGroupContext.Fee_PaymentGateway_Details.Single(t => t.MI_Id == data.MI_Id && t.FPGD_Id == data.FPGD_Id).FPGD_AuthorizationHeader;

                var date = ("-MM-dd");

                List<College_Student_SettlementDTO> list = new List<College_Student_SettlementDTO>();

                var list123 = (from a in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                               where (a.MI_ID == data.MI_Id)
                               select new College_Student_SettlementDTO
                               {
                                   FMG_Id = a.FMG_ID
                               }
      ).Distinct().Select(t => t.FMG_Id).ToList();

                list = (from a in _FeeGroupContext.FeeGroupClgDMO
                        where (a.MI_Id == data.MI_Id && list123.Contains(a.FMG_Id))
                        select new College_Student_SettlementDTO
                        {
                            user_id = a.user_id
                        }
        ).Distinct().ToList();

                long useridss = 0;

                foreach (var x in list)
                {
                    useridss = x.user_id;
                }

                var values = new Dictionary<string, string>
                        {
                        { "Authorization", paymentdet.FirstOrDefault().FPGD_AuthorisationKey },
                        };

                string url = "";

                if (PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_PGFlag.Equals("PAYU"))
                {
                    //url = "https://www.payumoney.com/treasury/op/getSettlementDetailsByDate?merchantKey=SubMerchantKey&settlementDate=date";
                    url = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_SettlementURL;

                    url = url.Replace("date", date);
                    url = url.Replace("SubMerchantKey", paymentdet.FirstOrDefault().FPGD_SubMerchantKey);

                    System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                    req.Method = "GET";
                    req.Headers["Authorization"] = paymentdet.FirstOrDefault().FPGD_AuthorizationHeader;
                    // req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
                    System.Net.WebResponse resp = req.GetResponseAsync().Result;
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    string s = sr.ReadToEnd().Trim();
                    JObject joResponse1 = JObject.Parse(s);
                    JArray array1 = (JArray)joResponse1["result"];
                    if (array1.Count == 0)
                    {
                        data.settled_flag = false;
                    }
                    else if (array1.Count > 0)
                    {
                        data.settled_flag = true;
                    }

                    foreach (JObject root1 in array1)
                    {
                        JArray array2 = (JArray)root1["transaction"];

                        Fee_Payment_Overall_Settlement_Details_CollegeDMO obj1 = new Fee_Payment_Overall_Settlement_Details_CollegeDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.ASMAY_Id = data.ASMAY_Id;
                        obj1.FYPPSTC_Settlement_Id = (String)root1["settlementId"];
                        obj1.FYPPSTC_Settlement_Date = DateTime.ParseExact((String)root1["settlementCompletedDate"], "yyyy-MM-dd", null);
                        obj1.FYPPSTC_Settlement_Amount = (Int64)root1["settlementAmount"];
                        obj1.FYPPSTC_UTR_No = (String)root1["utrnumber"];
                        //obj1.User_id = data.user_id;
                        obj1.User_Id = Convert.ToInt32(useridss);

                        _FeeGroupContext.Add(obj1);

                        foreach (JObject root2 in array2)
                        {
                            Fee_Payment_Settlement_Details_CollegeDMO obj2 = new Fee_Payment_Settlement_Details_CollegeDMO();
                            obj2.FYPPSDC_PAYU_Id = (String)root2["payuId"];
                            obj2.FYPPSDC_Transaction_amount = (Int64)root2["transactionAmount"];
                            obj2.FYPPSDC_Payment_Id = (string)root2["paymentId"];
                            obj2.FYPPSDC_Payment_Mode = (String)root2["paymentMode"];
                            obj2.FYPPSDC_Payment_Status = (String)root2["paymentStatus"];
                            obj2.FYPPSDC_Transaction_Date = DateTime.ParseExact((String)root2["paymentAddedOn"], "yyyy-MM-dd", null);
                            obj2.FYPPSDC_Payment_Amount = (Int64)root2["paymentAmount"];
                            obj2.FYPPSTC_Id = obj1.FYPPSTC_Id;

                            _FeeGroupContext.Add(obj2);
                        }
                    }
                    var exists = _FeeGroupContext.SaveChanges();
                    if (exists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                if (PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_PGFlag.Equals("PAYTM"))
                {

                    string utrrno = "";
                    //url = "https://securegw.paytm.in/merchant-settlement-services/search/settlement";
                    url = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_SettlementURL;

                    Dictionary<String, String> paytmParams = new Dictionary<String, String>();
                    paytmParams.Add("MID", paymentdet.FirstOrDefault().MID);
                    paytmParams.Add("date", date);
                    paytmParams.Add("pageNum", "1");
                    paytmParams.Add("pageSize", "100");

                    string paytmChecksum = generateCheckSum(paymentdet.FirstOrDefault().FPGD_AuthorisationKey, paytmParams);
                    paytmParams.Add("checksumHash", paytmChecksum);


                    var myContent = JsonConvert.SerializeObject(paytmParams);

                    //String postData = "JsonData=" + new JavaScriptSerializer().Serialize(paytmParams);
                    String postData = myContent;
                    HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                    connection.ContentType = "application/json";
                    connection.MediaType = "application/json";
                    connection.Accept = "application/json";

                    connection.Method = "POST";

                    using (StreamWriter requestWriter = new StreamWriter(connection.GetRequestStream()))
                    {
                        requestWriter.Write(postData);
                    }
                    string responseData = string.Empty;

                    using (StreamReader responseReader = new StreamReader(connection.GetResponse().GetResponseStream()))
                    {
                        responseData = responseReader.ReadToEnd();

                        //var obj = JArray.Parse(responseData);
                        JObject joResponse1 = JObject.Parse(responseData);
                        JArray array1 = (JArray)joResponse1["settlementSearchResponseVO"]["settlementDetailList"];

                        string totalcount = joResponse1["settlementSearchResponseVO"]["totalCount"].ToString();
                        string settledAmount = joResponse1["settlementSearchResponseVO"]["settledAmount"].ToString();

                        settledAmount = settledAmount.Replace(".00", "");

                        if (array1.Count == 0)
                        {
                            data.settled_flag = false;
                        }
                        else if (array1.Count > 0)
                        {
                            data.settled_flag = true;
                        }

                        //foreach (JObject root1 in array1)
                        //{
                        Fee_Payment_Overall_Settlement_Details_CollegeDMO obj1 = new Fee_Payment_Overall_Settlement_Details_CollegeDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.ASMAY_Id = data.ASMAY_Id;


                        obj1.FYPPSTC_Settlement_Id = indianTime.ToString("yyyyMMddHHmmss");

                        obj1.FYPPSTC_Settlement_Date = indianTime;

                        obj1.FYPPSTC_Settlement_Amount = Convert.ToInt64(settledAmount);

                        obj1.User_Id = Convert.ToInt32(useridss);

                        utrrno = (String)(array1[0]["UTR"]);
                        obj1.FYPPSTC_UTR_No = utrrno;

                        _FeeGroupContext.Add(obj1);

                        foreach (JObject root2 in array1)
                        {
                            Fee_Payment_Settlement_Details_CollegeDMO obj2 = new Fee_Payment_Settlement_Details_CollegeDMO();
                            obj2.FYPPSDC_PAYU_Id = (String)root2["TXNID"];
                            string FYPPSD_Transaction_amount = (String)(root2["TXNAMOUNT"]);

                            FYPPSD_Transaction_amount = FYPPSD_Transaction_amount.Replace(".00", "");

                            obj2.FYPPSDC_Transaction_amount = Convert.ToInt64(FYPPSD_Transaction_amount);

                            obj2.FYPPSDC_Payment_Id = "0";

                            obj2.FYPPSDC_Payment_Mode = (String)root2["PAYMENTMODE"];
                            obj2.FYPPSDC_Payment_Status = "Sucess";

                            string FYPPSD_Transaction_Date = (String)(root2["TXNDATE"]);
                            obj2.FYPPSDC_Transaction_Date = Convert.ToDateTime(FYPPSD_Transaction_Date);

                            string FYPPSD_Settled_amount = (String)(root2["SETTLEDAMOUNT"]);
                            FYPPSD_Settled_amount = FYPPSD_Settled_amount.Replace(".00", "");

                            obj2.FYPPSDC_Payment_Amount = Convert.ToInt64(FYPPSD_Settled_amount);
                            obj2.FYPPSTC_Id = obj1.FYPPSTC_Id;

                            utrrno = (String)(root2["UTR"]);

                            _FeeGroupContext.Add(obj2);
                        }

                        //obj1.User_id = utrrno;

                        //}


                        var exists = _FeeGroupContext.SaveChanges();
                        if (exists >= 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }

                if (PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_PGFlag.Equals("RAZORPAY"))
                {
                    //single account added on 17/12/2019

                    var accountvalidation = (from a in _FeeGroupContext.Fee_PaymentGateway_Details
                                             where (a.MI_Id == data.MI_Id && a.FPGD_PGName == "RAZORPAY")
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                             }).Distinct().ToArray();

                    //single account added on 17/12/2019
                    if (accountvalidation.Count() > 1)
                    {
                        var FYPPSD_Idmax = (from a in _FeeGroupContext.Fee_Payment_Settlement_Details_CollegeDMO
                                            where (a.MI_Id == data.MI_Id)
                                            select new College_Student_SettlementDTO
                                            {
                                                FYPPSDC_Id = a.FYPPSDC_Id,
                                            }
                       ).Distinct().ToList();

                        string utrrno = "";
                        //url = "https://api.razorpay.com/v1/settlements/?count=10&skip=0&from=frmdate&to=todate";
                        url = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_SettlementURL;

                        //CURRENT DAY - 1 
                        //Int32 unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddDays(-18).AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        //Int32 unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddDays(-18).AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        //CURRENT DAY - 1 

                        //CURRENT DAY

                        Int32 unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        Int32 unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        //CURRENT DAY

                        url = url.Replace("frmdate", unixTimestampstart.ToString());
                        url = url.Replace("todate", unixTimestampend.ToString());

                        List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                        paymentdetails = _FeeGroupContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                        list = (from a in _FeeGroupContext.FEE_RAZOR_TRANSFER_API_DETAILS
                                from b in _FeeGroupContext.Fee_Y_PaymentDMO
                                where (a.ORDER_ID == b.FYP_PaymentReference_Id && a.MI_ID == data.MI_Id && a.SETTLEMENT_FLAG == "0"
                                && b.FYP_ChallanStatusFlag == "Sucessfull" && b.FYP_PayGatewayType == "RAZORPAY")
                                select new College_Student_SettlementDTO
                                {
                                    ORDER_ID = a.ORDER_ID,
                                    TRANSFER_ID = a.TRANSFER_ID,
                                    PAYMENT_ID = a.PAYMENT_ID,
                                }
         ).Distinct().ToList();

                        for (int r = 0; r < list.Count(); r++)
                        {
                            //url = "https://api.razorpay.com/v1/payments/paymentid/transfers";
                            //url = url.Replace("paymentid", list[r].PAYMENT_ID);
                            url = "https://api.razorpay.com/v1/transfers/TRANSFERID";
                            url = url.Replace("TRANSFERID", list[r].TRANSFER_ID);

                            string method1 = "GET";

                            HttpWebRequest requestsett = (HttpWebRequest)WebRequest.Create(url);
                            requestsett.Method = method1.ToString();
                            requestsett.ContentLength = 0;
                            requestsett.ContentType = "application/json";

                            string userAgent1 = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                            requestsett.UserAgent = "razorpay-dot-net/" + userAgent1;

                            string authString1 = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                            requestsett.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                                Encoding.UTF8.GetBytes(authString1));

                            System.Net.WebResponse resp1 = requestsett.GetResponseAsync().Result;
                            System.IO.StreamReader sr1 = new System.IO.StreamReader(resp1.GetResponseStream());
                            string s1 = sr1.ReadToEnd().Trim();
                            JObject root2 = JObject.Parse(s1);
                            // JArray array2 = (JArray)joResponse2[""];
                            Fee_Payment_Settlement_Details_CollegeDMO obj2 = new Fee_Payment_Settlement_Details_CollegeDMO();
                            if ((String)root2["recipient_settlement_id"] != null)
                            {
                                var getconnid = (from a in _FeeGroupContext.FEE_RAZOR_TRANSFER_API_DETAILS
                                                 where (a.MI_ID == data.MI_Id && a.TRANSFER_ID == (String)root2["id"])
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     order_id = a.ORDER_ID,
                                                     payment_id = a.PAYMENT_ID
                                                 }
                             ).ToList();

                                //obj2.FYPPSD_PAYU_Id = getconnid[0].order_id.ToString();
                                obj2.FYPPSDC_PAYU_Id = getconnid[0].payment_id.ToString();
                                string FYPPSDC_Transaction_amount = (String)(root2["amount"]);
                                obj2.FYPPSDC_Transaction_amount = Convert.ToInt32(FYPPSDC_Transaction_amount) / 100;
                                //obj2.FYPPSD_Payment_Id = getconnid[0].payment_id.ToString();
                                obj2.FYPPSDC_Payment_Id = getconnid[0].order_id.ToString();
                                obj2.FYPPSDC_Payment_Mode = (String)root2["recipient_settlement_id"];
                                obj2.FYPPSDC_Payment_Status = "Sucess";

                                // Format our new DateTime object to start at the UNIX Epoch
                                System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                                // Add the timestamp (number of seconds since the Epoch) to be converted
                                dateTime = dateTime.AddSeconds((double)root2["created_at"]);

                                obj2.FYPPSDC_Transaction_Date = dateTime;
                                obj2.FYPPSDC_Payment_Amount = Convert.ToInt32(FYPPSDC_Transaction_amount) / 100; ;
                                obj2.MI_Id = data.MI_Id;
                                obj2.FYPPSTC_Id = 0;

                                _FeeGroupContext.Add(obj2);
                                //var existsRAZORPAY = _FeeGroupContext.SaveChanges();

                                var obj_status_stf = _FeeGroupContext.FEE_RAZOR_TRANSFER_API_DETAILS.Where(t => t.MI_ID == data.MI_Id && t.TRANSFER_ID == (String)root2["id"]).FirstOrDefault();

                                obj_status_stf.SETTLEMENT_FLAG = "1";
                                _FeeGroupContext.Update(obj_status_stf);

                                _FeeGroupContext.SaveChanges();
                            }


                        }

                        try
                        {
                            List<College_Student_SettlementDTO> ovrsett = new List<College_Student_SettlementDTO>();
                            using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())

                            {
                                cmd1.CommandText = "getsettlementsummation";
                                cmd1.CommandType = CommandType.StoredProcedure;

                                cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });

                                cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                if (FYPPSD_Idmax == null)
                                {
                                    cmd1.Parameters.Add(new SqlParameter("@FYPPSD_Idmax", SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                }
                                else
                                {
                                    cmd1.Parameters.Add(new SqlParameter("@FYPPSD_Idmax", SqlDbType.BigInt)
                                    {
                                        Value = FYPPSD_Idmax.Max(x => x.FYPPSDC_Id)
                                    });
                                }

                                cmd1.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                                {
                                    Value = data.user_id
                                });

                                if (cmd1.Connection.State != ConnectionState.Open)
                                    cmd1.Connection.Open();

                                var retObject1 = new List<dynamic>();

                                try
                                {
                                    using (var dataReader1 = cmd1.ExecuteReader())
                                    {
                                        while (dataReader1.Read())
                                        {
                                            ovrsett.Add(new College_Student_SettlementDTO
                                            {
                                                FYPPSDC_Payment_Mode = Convert.ToString(dataReader1["FYPPSD_Payment_Mode"]),
                                                overallsettamt = Convert.ToInt32(dataReader1["SettmentAmount"]),
                                            });
                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }



                            foreach (var value in ovrsett)
                            //for (int q = 0; q < data.ovrsett.Length; q++)
                            {
                                Fee_Payment_Overall_Settlement_Details_CollegeDMO overallsett = new Fee_Payment_Overall_Settlement_Details_CollegeDMO();

                                url = "https://api.razorpay.com/v1/transfers?expand[]=recipient_settlement&recipient_settlement_id=receipient_sett_id";
                                url = url.Replace("receipient_sett_id", value.FYPPSDC_Payment_Mode);

                                string method1 = "GET";

                                HttpWebRequest requestsett = (HttpWebRequest)WebRequest.Create(url);
                                requestsett.Method = method1.ToString();
                                requestsett.ContentLength = 0;
                                requestsett.ContentType = "application/json";

                                string userAgent1 = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                                requestsett.UserAgent = "razorpay-dot-net/" + userAgent1;

                                string authString1 = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                                requestsett.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                                    Encoding.UTF8.GetBytes(authString1));

                                System.Net.WebResponse resp1 = requestsett.GetResponseAsync().Result;
                                System.IO.StreamReader sr1 = new System.IO.StreamReader(resp1.GetResponseStream());
                                string s1 = sr1.ReadToEnd().Trim();
                                JObject root3 = JObject.Parse(s1);

                                Fee_Payment_Overall_Settlement_Details_CollegeDMO obj3 = new Fee_Payment_Overall_Settlement_Details_CollegeDMO();

                                obj3.MI_Id = data.MI_Id;
                                obj3.ASMAY_Id = data.ASMAY_Id;
                                obj3.FYPPSTC_Settlement_Id = value.FYPPSDC_Payment_Mode;

                                JArray array2 = (JArray)root3["items"];
                                //JArray array3 = (JArray)array2["recipient_settlement"];

                                obj3.FYPPSTC_UTR_No = (array2[0]["recipient_settlement"]["utr"].ToString());
                                obj3.FYPPSTC_Settlement_Amount = value.overallsettamt;

                                // Format our new DateTime object to start at the UNIX Epoch
                                System.DateTime dateTime1 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                                // Add the timestamp (number of seconds since the Epoch) to be converted
                                obj3.FYPPSTC_Settlement_Date = dateTime1.AddSeconds((double)(array2[0]["recipient_settlement"]["created_at"]));
                                obj3.User_Id = 0;

                                _FeeGroupContext.Add(obj3);
                                _FeeGroupContext.SaveChanges();

                                _FeeGroupContext.Database.ExecuteSqlCommand("updatesettlementidcollege @p0,@p1,@p2", data.MI_Id, data.ASMAY_Id, value.FYPPSDC_Payment_Mode);

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        Int32 unixTimestampstart = 0;
                        Int32 unixTimestampend = 0;


                        unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddHours(-62).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        //url = "https://api.razorpay.com/v1/settlements/?count=10&skip=0&from=1576434600&to=1576607340";

                        string urlupdated = "https://api.razorpay.com/v1/settlements/?count=10&skip=0&from=frmdate&to=todate";
                        urlupdated = urlupdated.Replace("frmdate", unixTimestampstart.ToString());
                        urlupdated = urlupdated.Replace("todate", unixTimestampend.ToString());

                        string method1 = "GET";

                        HttpWebRequest requestsett = (HttpWebRequest)WebRequest.Create(urlupdated);
                        requestsett.Method = method1.ToString();
                        requestsett.ContentLength = 0;
                        requestsett.ContentType = "application/json";

                        string userAgent1 = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                        requestsett.UserAgent = "razorpay-dot-net/" + userAgent1;

                        List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                        paymentdetails = _FeeGroupContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                        string authString1 = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                        requestsett.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                            Encoding.UTF8.GetBytes(authString1));

                        System.Net.WebResponse resp1 = requestsett.GetResponseAsync().Result;
                        System.IO.StreamReader sr1 = new System.IO.StreamReader(resp1.GetResponseStream());
                        string s1 = sr1.ReadToEnd().Trim();
                        JObject root3 = JObject.Parse(s1);

                        Fee_Payment_Overall_Settlement_Details_CollegeDMO obj3 = new Fee_Payment_Overall_Settlement_Details_CollegeDMO();

                        JArray array2 = (JArray)root3["items"];

                        obj3.MI_Id = data.MI_Id;
                        obj3.ASMAY_Id = data.ASMAY_Id;
                        obj3.FYPPSTC_Settlement_Id = (array2[0]["id"].ToString());
                        obj3.FYPPSTC_UTR_No = (array2[0]["utr"].ToString());
                        obj3.FYPPSTC_Settlement_Amount = (Int32)(array2[0]["amount"]) / 100;

                        // Format our new DateTime object to start at the UNIX Epoch
                        System.DateTime dateTime1 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                        // Add the timestamp (number of seconds since the Epoch) to be converted
                        obj3.FYPPSTC_Settlement_Date = dateTime1.AddSeconds((double)(array2[0]["created_at"]));
                        obj3.User_Id = 0;

                        _FeeGroupContext.Add(obj3);
                        _FeeGroupContext.SaveChanges();

                        //second API

                        string unixTimestampstartDAY = DateTime.UtcNow.Day.ToString();
                        string unixTimestampstartMONTH = DateTime.UtcNow.Month.ToString();
                        string unixTimestampstartYEAR = DateTime.UtcNow.Year.ToString();

                        //url = "https://api.razorpay.com/v1/settlements/recon/combined?year=2019&month=12&day=16";

                        string secondurl = "https://api.razorpay.com/v1/settlements/recon/combined?year=YEARID&month=MONTHID&day=DAYID";
                        secondurl = secondurl.Replace("YEARID", unixTimestampstartYEAR.ToString());
                        secondurl = secondurl.Replace("MONTHID", unixTimestampstartMONTH.ToString());
                        secondurl = secondurl.Replace("DAYID", unixTimestampstartDAY.ToString());

                        string method2 = "GET";

                        HttpWebRequest requestsettI = (HttpWebRequest)WebRequest.Create(secondurl);
                        requestsettI.Method = method1.ToString();
                        requestsettI.ContentLength = 0;
                        requestsettI.ContentType = "application/json";

                        string userAgent2 = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                        requestsettI.UserAgent = "razorpay-dot-net/" + userAgent2;

                        requestsettI.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(authString1));

                        System.Net.WebResponse resp2 = requestsettI.GetResponseAsync().Result;
                        System.IO.StreamReader sr2 = new System.IO.StreamReader(resp2.GetResponseStream());
                        string s2 = sr2.ReadToEnd().Trim();
                        JObject root4 = JObject.Parse(s2);

                        JArray array3 = (JArray)root4["items"];

                        for (int q = 0; q < array3.Count(); q++)
                        {
                            Fee_Payment_Settlement_Details_CollegeDMO obj4 = new Fee_Payment_Settlement_Details_CollegeDMO();
                            if (array3[q]["type"].ToString() == "payment")
                            {
                                obj4.FYPPSDC_PAYU_Id = array3[q]["entity_id"].ToString();
                                obj4.FYPPSDC_Transaction_amount = (Int32)array3[q]["credit"] / 100;
                                obj4.FYPPSDC_Payment_Id = array3[q]["order_id"].ToString();
                                obj4.FYPPSDC_Payment_Mode = array3[q]["settlement_id"].ToString();
                                obj4.FYPPSDC_Payment_Status = "Completed";

                                // Format our new DateTime object to start at the UNIX Epoch
                                System.DateTime dateTime2 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                                // Add the timestamp (number of seconds since the Epoch) to be converted
                                obj4.FYPPSDC_Transaction_Date = dateTime1.AddSeconds((double)(array2[0]["created_at"]));
                                obj4.FYPPSDC_Payment_Amount = (Int32)array3[q]["credit"];
                                obj4.FYPPSTC_Id = obj3.FYPPSTC_Id;
                                obj4.MI_Id = data.MI_Id;

                                _FeeGroupContext.Add(obj4);



                            }
                        }

                        _FeeGroupContext.SaveChanges();

                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public static String generateCheckSum(String masterKey, Dictionary<String, String> parameters)
        {
            // Validate Input
            validateGenerateCheckSumInput(masterKey);
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            try
            {
                foreach (string key in parameters.Keys)
                {
                    if (parameters[key].Trim().ToUpper().Contains("REFUND") || parameters[key].Trim().Contains("|"))
                    {
                        parameter.Add(key.Trim(), "");
                    }

                    else
                    {
                        parameter.Add(key.Trim(), parameters[key].Trim());
                    }
                }

                String checkSumParams = SecurityUtils.createCheckSumString(parameter);
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);

                checkSumParams += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("Final CheckSum String:::: " + checkSumParams);
                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String:::: " + hashedCheckSum);
                hashedCheckSum += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String with Salt:::: " + hashedCheckSum);

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);

                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }
        }

        private static void validateGenerateCheckSumInput(String masterKey)
        {
            if (masterKey == null)
            {
                //throw new ArgumentNullException("masterKey");
                throw new ArgumentNullException("Parameter cannot be null", "masterKey");
            }

        }
        private static string getAppDetailsUa()
        {
            List<Dictionary<string, string>> appsDetails = RazorpayClient.AppsDetails;

            string appsDetailsUa = string.Empty;

            foreach (Dictionary<string, string> appsDetail in appsDetails)
            {
                string appUa = string.Empty;

                if (appsDetail.ContainsKey("title"))
                {
                    appUa = appsDetail["title"];

                    if (appsDetail.ContainsKey("version"))
                    {
                        appUa += appsDetail["version"];
                    }
                }

                appsDetailsUa += appUa;
            }

            return appsDetailsUa;
        }


        public PDC_EntryFormDTO getbranchdetails(PDC_EntryFormDTO data)
        {

            try
            {

                var branchlist = (from a in _FeeGroupContext.ClgMasterBranchDMO
                                  from b in _FeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _FeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag && b.AMCO_Id == data.AMCO_Id)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToList();
                data.fillbranch = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PDC_EntryFormDTO getsemesterdetails(PDC_EntryFormDTO data)
        {

            try
            {

                var semisterlist = (from a in _FeeGroupContext.CLG_Adm_Master_SemesterDMO
                                    from b in _FeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _FeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _FeeGroupContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag
                                    && b.AMCO_Id == data.AMCO_Id && c.AMB_Id == data.AMB_Id)
                                    select new CLG_Adm_Master_SemesterDMO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();
                data.FillSemester = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public PDC_EntryFormDTO selectstudent(PDC_EntryFormDTO data)
        {
            try
            {

                var fillstudent = (from b in _FeeGroupContext.CLG_Adm_College_AY_CourseDMO
                                   from c in _FeeGroupContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from d in _FeeGroupContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO

                                   from f in _FeeGroupContext.Adm_Master_College_StudentDMO
                                   from g in _FeeGroupContext.Adm_College_Yearly_StudentDMO
                                   where (f.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.ACAYCBS_ActiveFlag
                                   && b.AMCO_Id == data.AMCO_Id && c.AMB_Id == data.AMB_Id && f.AMCST_Id == g.AMCST_Id && g.AMSE_Id == data.AMSE_Id)
                                   select new CollegeConcessionDTO
                                   {
                                       AMCST_FirstName = f.AMCST_FirstName,
                                       AMCST_MiddleName = f.AMCST_MiddleName,
                                       AMCST_LastName = f.AMCST_LastName,

                                       AMCST_AdmNo = f.AMCST_AdmNo,
                                       AMCST_Id = f.AMCST_Id,
                                       AMCST_RegistrationNo = f.AMCST_RegistrationNo,
                                   }).Distinct().ToList();
                data.FillCategory = fillstudent.ToList().Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
