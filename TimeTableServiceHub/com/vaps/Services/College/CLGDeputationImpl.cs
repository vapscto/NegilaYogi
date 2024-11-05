using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;
using SendGrid;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using System.IO;
//using CommonLibrary;
//using System.Net.Mail;

using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net.Mail;
using SendGrid.Helpers.Mail;
using CommonLibrary;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using Newtonsoft.Json.Linq;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class CLGDeputationImpl : Interfaces.CLGDeputationInterface
    {
        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _dbcontext;
        public DomainModelMsSqlServerContext _db;
        public CLGDeputationImpl(TTContext ttcntx, DomainModelMsSqlServerContext _db123, DomainModelMsSqlServerContext _db1234)
        {
            _ttcontext = ttcntx;
            _dbcontext = _db123;
            _db = _db1234;
        }

        public CLGDeputationDTO savedetails(CLGDeputationDTO data)
        {
            try
            {

                //for edit and add start here.............
                var result = _ttcontext.CLGDeputationDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTSDC_Date == data.TTSD_Date && t.TTSDC_AbsentStaff == data.TTSD_AbsentStaff && t.TTMD_Id == data.TTMD_Id && t.TTMP_Id == data.TTMP_Id).Count();
                if (result > 0)
                {
                    var result1 = _ttcontext.CLGDeputationDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTSDC_Date == data.TTSD_Date && t.TTSDC_AbsentStaff == data.TTSD_AbsentStaff && t.TTMD_Id == data.TTMD_Id && t.TTMP_Id == data.TTMP_Id);
                    result1.ASMAY_Id = data.ASMAY_Id;
                    result1.TTSDC_Date = data.TTSD_Date;
                    result1.TTSDC_AbsentStaff = data.TTSD_AbsentStaff;
                    result1.TTSDC_DeputedStaff = data.TTSD_DeputedStaff;
                    result1.AMCO_Id = data.AMCO_Id;
                    result1.AMB_Id = data.AMB_Id;
                    result1.AMSE_Id = data.AMSE_Id;
                    result1.ACMS_Id = data.ACMS_Id;
                    result1.TTMD_Id = data.TTMD_Id;
                    result1.TTMP_Id = data.TTMP_Id;
                    result1.TTSDC_Remarks = data.TTSD_Remarks;
                    result1.UpdatedDate = DateTime.Now;
                    _ttcontext.Update(result1);
                    var contactExists = _ttcontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        var Template = "";
                        data.returnval = true;
                        if (data.mailflag == true)
                        {
                            var result123 = _ttcontext.CLGDeputationDMO.Max(t => t.TTSDC_Id);
                            for (int z = 0; z <= 1; z++)
                            {
                                string mailId = "";
                                // string mailId = "suryan989@gmail.com";
                                if (z == 0)
                                {
                                    Template = "CLGTTAbsentStaff";
                                    mailId = _ttcontext.Emp_Email_Id.Where(h => h.HRME_Id.Equals(data.TTSD_AbsentStaff) && h.HRMEM_DeFaultFlag == "default").Select(h => h.HRMEM_EmailId).FirstOrDefault();
                                    //  mailId = "suryan989@gmail.com";
                                }
                                else
                                {
                                    Template = "CLGTTTakingStaff";
                                    mailId = _ttcontext.Emp_Email_Id.Where(h => h.HRME_Id.Equals(data.TTSD_DeputedStaff) && h.HRMEM_DeFaultFlag == "default").Select(h => h.HRMEM_EmailId).FirstOrDefault();
                                    //    mailId = "suryan989@gmail.com";
                                }
                                sendmail(data.MI_Id, mailId, Template, result123);
                            }
                        }
                        //Mails Sending Start
                        if (data.smsflag == true)
                        {
                            var result123 = _ttcontext.CLGDeputationDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTSDC_Date == data.TTSD_Date && t.TTSDC_AbsentStaff == data.TTSD_AbsentStaff && t.TTMD_Id == data.TTMD_Id && t.TTMP_Id == data.TTMP_Id);
                            for (int z = 0; z <= 1; z++)
                            {
                                long contactno = 0;
                                //long contactno = 8095652032;
                                if (z == 0)
                                {
                                    Template = "CLGTTAbsentStaff";
                                    contactno = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(data.TTSD_AbsentStaff) && h.HRMEMNO_DeFaultFlag == "default").Select(h => h.HRMEMNO_MobileNo).FirstOrDefault();
                                    //  contactno = 9686358569;
                                }
                                else
                                {
                                    Template = "TTTakingStaff";
                                    contactno = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(data.TTSD_DeputedStaff) && h.HRMEMNO_DeFaultFlag == "default").Select(h => h.HRMEMNO_MobileNo).FirstOrDefault();
                                    //   contactno = 9686358569;
                                }

                                if (contactno != 0)
                                {
                                    sendSms(data.MI_Id, contactno, Template, result123.TTSDC_Id);
                                }
                            }
                        }

                        if (data.NOT_Flag == true)
                        {

                            var keylist = _ttcontext.MobileApplAuthenticationDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToList();

                            if (keylist.Count > 0)
                            {



                                if (data.deviceid != null && data.deviceid != "")
                                {
                                    long mobno = 0;
                                    var contactnolist = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(data.TTSD_DeputedStaff) && h.HRMEMNO_DeFaultFlag == "default").ToList();
                                    if (contactnolist.Count > 0)
                                    {
                                        mobno = contactnolist[0].HRMEMNO_MobileNo;
                                    }
                                    var result123 = _ttcontext.CLGDeputationDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTSDC_Date == data.TTSD_Date && t.TTSDC_DeputedStaff == data.TTSD_DeputedStaff && t.TTMD_Id == data.TTMD_Id && t.TTMP_Id == data.TTMP_Id);

                                    int e = callnotification(data.TTSD_DeputedStaff, data.deviceid, mobno, data.MI_Id, "", "", keylist[0].MAAN_AuthenticationKey, "Staff", result123.TTSDC_Id, "P");
                                }

                                var empmobid = _ttcontext.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.TTSD_AbsentStaff).ToList();

                                if (empmobid.Count > 0)
                                {


                                    if (empmobid[0].HRME_AppDownloadedDeviceId != null && empmobid[0].HRME_AppDownloadedDeviceId != "")
                                    {

                                        long mobno = 0;
                                        var contactnolist = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(data.TTSD_AbsentStaff) && h.HRMEMNO_DeFaultFlag == "default").ToList();
                                        if (contactnolist.Count > 0)
                                        {
                                            mobno = contactnolist[0].HRMEMNO_MobileNo;
                                        }
                                        var result123 = _ttcontext.CLGDeputationDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTSDC_Date == data.TTSD_Date && t.TTSDC_AbsentStaff == data.TTSD_AbsentStaff && t.TTMD_Id == data.TTMD_Id && t.TTMP_Id == data.TTMP_Id);

                                        int e = callnotification(data.TTSD_AbsentStaff, empmobid[0].HRME_AppDownloadedDeviceId, mobno, data.MI_Id, "", "", keylist[0].MAAN_AuthenticationKey, "Staff", result123.TTSDC_Id, "A");



                                    }
                                }




                            }


                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else if (result == 0)
                {

                    CLGDeputationDMO res = new CLGDeputationDMO();
                    res.ASMAY_Id = data.ASMAY_Id;
                    res.MI_Id = data.MI_Id;
                    res.TTSDC_Date = data.TTSD_Date;
                    res.TTSDC_AbsentStaff = data.TTSD_AbsentStaff;
                    res.TTSDC_DeputedStaff = data.TTSD_DeputedStaff;
                    res.AMCO_Id = data.AMCO_Id;
                    res.AMB_Id = data.AMB_Id;
                    res.AMSE_Id = data.AMSE_Id;
                    res.ACMS_Id = data.ACMS_Id;
                    res.TTMD_Id = data.TTMD_Id;
                    res.TTMP_Id = data.TTMP_Id;
                    res.TTSDC_Remarks = data.TTSD_Remarks;
                    res.UpdatedDate = DateTime.Now;
                    res.CreatedDate = DateTime.Now;
                    _ttcontext.Add(res);
                    var contactExists = _ttcontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        var Template = "";
                        data.returnval = true;
                        // mail_for_deputation(_category);
                        if (data.mailflag == true)
                        {
                            var result123 = _ttcontext.CLGDeputationDMO.Max(t => t.TTSDC_Id);
                            for (int z = 0; z <= 1; z++)
                            {
                                string mailId = "";
                                //  string mailId = "suryan989@gmail.com";
                                if (z == 0)
                                {
                                    Template = "CLGTTAbsentStaff";
                                    mailId = _ttcontext.Emp_Email_Id.Where(h => h.HRME_Id.Equals(data.TTSD_AbsentStaff) && h.HRMEM_DeFaultFlag == "default").Select(h => h.HRMEM_EmailId).FirstOrDefault();
                                    // mailId = "suryan989@gmail.com";
                                }
                                else
                                {
                                    Template = "CLGTTTakingStaff";
                                    mailId = _ttcontext.Emp_Email_Id.Where(h => h.HRME_Id.Equals(data.TTSD_DeputedStaff) && h.HRMEM_DeFaultFlag == "default").Select(h => h.HRMEM_EmailId).FirstOrDefault();
                                    //mailId = "suryan.netdeveloper@gmail.com";
                                }
                                sendmail(data.MI_Id, mailId, Template, result123);
                            }
                        }
                        //Mails Sending Start
                        if (data.smsflag == true)
                        {
                            var result123 = _ttcontext.CLGDeputationDMO.Max(t => t.TTSDC_Id);
                            for (int z = 0; z <= 1; z++)
                            {
                                long contactno = 0;
                                //long contactno = 8095652032;
                                if (z == 0)
                                {
                                    Template = "CLGTTAbsentStaff";
                                    contactno = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(data.TTSD_AbsentStaff) && h.HRMEMNO_DeFaultFlag == "default").Select(h => h.HRMEMNO_MobileNo).FirstOrDefault();
                                    //  contactno = 9686358569;
                                }
                                else
                                {
                                    Template = "CLGTTTakingStaff";
                                    contactno = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(data.TTSD_DeputedStaff) && h.HRMEMNO_DeFaultFlag == "default").Select(h => h.HRMEMNO_MobileNo).FirstOrDefault();
                                    // contactno = 7892679744;
                                }
                                if (contactno != 0)
                                {
                                    sendSms(data.MI_Id, contactno, Template, result123);
                                }
                            }
                        }
                        if (data.NOT_Flag == true)
                        {

                            var keylist = _ttcontext.MobileApplAuthenticationDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToList();

                            if (keylist.Count > 0)
                            {



                                if (data.deviceid != null && data.deviceid != "")
                                {
                                    long mobno = 0;
                                    var contactnolist = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(data.TTSD_DeputedStaff) && h.HRMEMNO_DeFaultFlag == "default").ToList();
                                    if (contactnolist.Count > 0)
                                    {
                                        mobno = contactnolist[0].HRMEMNO_MobileNo;
                                    }
                                    var result123 = _ttcontext.CLGDeputationDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTSDC_Date == data.TTSD_Date && t.TTSDC_DeputedStaff == data.TTSD_DeputedStaff && t.TTMD_Id == data.TTMD_Id && t.TTMP_Id == data.TTMP_Id);

                                    int e = callnotification(data.TTSD_DeputedStaff, data.deviceid, mobno, data.MI_Id, "", "", keylist[0].MAAN_AuthenticationKey, "Staff", result123.TTSDC_Id, "P");
                                }

                                var empmobid = _ttcontext.HR_Master_Employee_DMO.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == data.TTSD_AbsentStaff).ToList();

                                if (empmobid.Count > 0)
                                {


                                    if (empmobid[0].HRME_AppDownloadedDeviceId != null && empmobid[0].HRME_AppDownloadedDeviceId != "")
                                    {

                                        long mobno = 0;
                                        var contactnolist = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(data.TTSD_AbsentStaff) && h.HRMEMNO_DeFaultFlag == "default").ToList();
                                        if (contactnolist.Count > 0)
                                        {
                                            mobno = contactnolist[0].HRMEMNO_MobileNo;
                                        }
                                        var result123 = _ttcontext.CLGDeputationDMO.Single(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTSDC_Date == data.TTSD_Date && t.TTSDC_AbsentStaff == data.TTSD_AbsentStaff && t.TTMD_Id == data.TTMD_Id && t.TTMP_Id == data.TTMP_Id);

                                        int e = callnotification(data.TTSD_AbsentStaff, empmobid[0].HRME_AppDownloadedDeviceId, mobno, data.MI_Id, "", "", keylist[0].MAAN_AuthenticationKey, "Staff", result123.TTSDC_Id, "A");



                                    }
                                }




                            }

                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                //end ...........
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID)
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

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
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
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
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

        public string sendmail(long MI_Id, string Email, string Template, long UserID)
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

                //     string Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

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
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
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
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                }

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
                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

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
        public CLGDeputationDTO get_period_alloted(CLGDeputationDTO data)
        {

            try
            {

                data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();


                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_STAFF_DEPUTATION_PERIOD_DAYWISE";
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
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.TTMD_Id
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
                        data.Time_Table = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_ABSENTSTAFF_DEPUTATION_COUNT";
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
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = data.TTSD_Date
                    });


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = data.HRME_Id
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
                        data.absentdpcount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return data;
        }

        public CLGDeputationDTO get_free_stfdets(CLGDeputationDTO data)
        {
            
            try
            {

                data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();





                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_FREESTAFF_FOR_DEPUTATION";
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
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.TTMD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTMP_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.TTMP_Id
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
                        data.Time_Table = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_STAFF_DEPUTATION_COUNT";
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
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = data.TTSD_Date
                    });


                    cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = data.TTMD_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTMP_Id",
                   SqlDbType.NVarChar)
                    {
                        Value = data.TTMP_Id
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
                        data.dpcount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_ABSENTSTAFF_DEPUTATION_COUNT_ALL";
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
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = data.TTSD_Date
                    });


                    //cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    //SqlDbType.NVarChar)
                    //{
                    //    Value = data.HRME_Id
                    //});




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
                        data.absentdpcountall = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_DEPUTATION_WEEKLY_PERIOD_COUNT_COLLAGE";
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
                        data.weeklycntlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return data;
        }

        public CLGDeputationDTO getalldetailsviewrecords2(CLGDeputationDTO data)
        {
            try
            {
                data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

               
                if (data.ttftype=="F")
                {
                    data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_VIEW_STAFF_TT_FOR_DEPUTATION";
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
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
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
                            data.Time_Table = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                if (data.ttftype=="D")
                {

                    data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true && t.TTMD_Id==data.TTMD_Id).ToArray();

                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_VIEW_STAFF_TT_FOR_DEPUTATION_DAY";
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
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.TTMD_Id
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
                            data.Time_Table = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }



               

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public CLGDeputationDTO getdetails(CLGDeputationDTO data)
        {
            
            try
            {
                data.acayear = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(z=>z.ASMAY_Order).ToList().Distinct().ToArray();

                data.categorylist = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().Distinct().ToArray();

             
                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_STAFFS_NAMES";
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
                        data.stafflist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //to get data according to search criteria.
                //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "TT_CLG_GET_STAFFS_NAMES";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                result.Add(new CLGDeputationDTO
                //                {
                //                    HRME_Id =Convert.ToInt64(dataReader["HRME_Id"].ToString()),
                //                    HRME_EmployeeCode = dataReader["HRME_EmployeeCode"].ToString(),
                //                    staffName = dataReader["staffName"].ToString(),  
                //                });
                //                data.stafflist = result.ToArray();
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.Write(ex.Message);
                //    }
                //}
               

                data.classlist = _ttcontext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList().ToArray();

                data.daylist = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public CLGDeputationDTO viewdeputation(CLGDeputationDTO data)
        {
            try
            {
                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_STAFF_DEPUTATION_DETAILS";
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
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = data.TTSD_Date
                    });


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.NVarChar)
                    {
                        Value = data.FLAG
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
                        data.dptdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public CLGDeputationDTO viewabsent(CLGDeputationDTO data)
        {
            try
            {
                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_VIEW_ABESTSTAFF_DEPUTATION_DETAILS";
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
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = data.TTSD_Date
                    });


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.NVarChar)
                    {
                        Value = data.FLAG
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
                        data.dptdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public CLGDeputationDTO getabsentstaff(CLGDeputationDTO data)
        {
            try
            {
                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_ABSENTSTAFF_FOR_DEPUTATION";
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
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = data.TTSD_Date
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                   SqlDbType.Bit)
                    {
                        Value = data.absflag
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
                        data.stafflist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public int callnotification(long AMST_Id, string devicenew, long mobileno, long mi_id, string headername, string text, string key, string type, long TTSD_Id, string type1)
        {

            string transid = "";
            string result = "";
            int cnt = 0;
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_DEPUTATION_NOTIFICATION_MSG_COLLAGE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = mi_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTSD_Id",
                      SqlDbType.BigInt)
                    {
                        Value = TTSD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                     SqlDbType.Char)
                    {
                        Value = type1
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
                        var viewdata = retObject.ToList();

                        if (viewdata.Count > 0)
                        {
                            text = viewdata[0].msg;
                        }
                        else
                        {
                            text = "";
                        }
                    }


                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }


                }







                var head = "TIMETABLE DEPUTATION";


                string devicenew1 = '"' + devicenew + '"';

                devicenew1 = "[" + devicenew1 + "]";

                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                // "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Topic + '"' + " , " + '"' + "body" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Content + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";


                string sound = "default";
                //string notId = "4";
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                // "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Topic + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' +
                // +'"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , "
                // + '"' + "body" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Content + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew1 + "," +
                "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + head + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "body" + '"' + ":" + '"' + text + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                //connection.Headers["authorization"] = "key=AAAAvDDD0Rs:APA91bEFpdVjbc7oDFoFR2LIagSffKZmmu17NowfggiE752rEo45Hgl1kNX2_AWVxHqBcAUJOTvo5CApdlHNwNFHKBjIFqhVEwiQC9PVYdba_SRCAHC2tMVTVzV2WBaWcKIGGwAOGo4I";

                connection.Headers["authorization"] = "key=" + key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }


                string responsedata = string.Empty;



                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);

                    transid = (string)joresponse1["multicast_id"];
                    result = (string)joresponse1["success"];


                }



                Insert_PushNotification(mi_id, mobileno, devicenew, head, text, type, AMST_Id, transid, result);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }

            if (result == "0")
            {
                cnt = 0;
            }
            else if (result == "1")
            {
                cnt = 1;
            }
            else
            {
                cnt = 0;
            }

            return cnt;

        }

        public string Insert_PushNotification(long mi_id, long mobileno, string devicenew, string header, string msgdetails, string type, long appdownloaderid, string transid, string result)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
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


                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = header;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = type;
                sdd.PNSD_SMSMessage = msgdetails;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                sdd.PNSD_SystemIP = remoteIpAddress;
                sdd.PNSD_NetworkIP = myIP1;
                sdd.PNSD_MAACAddress = sMacAddress;
                sdd.PNSD_TransactionId = transid;
                sdd.PNSD_ApprovalLevel = "0";

                _ttcontext.Add(sdd);


                PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                ss.PNSD_Id = sdd.PNSD_Id;
                ss.PNSDDE_MobileNo = mobileno;
                ss.PNSDDE_DeviceId = devicenew;
                ss.PNSDDE_ReadStatus = result;
                ss.PNSDDE_DeliveryDate = indianTime;
                ss.PNSDDE_DeliveryTime = time;
                ss.PNSDDE_MakeUnreadFlg = false;
                ss.PNSDDE_ApprovalLevel = "0";
                _ttcontext.Add(ss);


                if (type == "Student")
                {
                    PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                    sds.PNSD_Id = sdd.PNSD_Id;
                    sds.AMST_Id = appdownloaderid;
                    _ttcontext.Add(sds);
                }


                if (type == "Staff")
                {
                    PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                    sds.PNSD_Id = sdd.PNSD_Id;
                    sds.HRME_Id = appdownloaderid;
                    _ttcontext.Add(sds);
                }



                _ttcontext.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "err";

            }
            return "success";
        }

    }
}
