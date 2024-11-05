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
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTDeputationImpl : Interfaces.DeputationInterface
    {
        public TTContext _ttcontext;
      
        public DomainModelMsSqlServerContext _db;
        public TTDeputationImpl(TTContext ttcntx, DomainModelMsSqlServerContext _db1234)
        {
            _ttcontext = ttcntx;
         
            _db = _db1234;
        }

        public TTDeputationDTO savedetails(TTDeputationDTO _category)
        {
            TT_Staff_DeputationDMO objpge = Mapper.Map<TT_Staff_DeputationDMO>(_category);
            try
            {

                List<TT_Staff_DeputationDMO> deputs = new List<TT_Staff_DeputationDMO>();
                deputs = _ttcontext.TT_Staff_DeputationDMO.Where(x => x.MI_Id == objpge.MI_Id).ToList();

                //for edit and add start here.............
                var result = _ttcontext.TT_Staff_DeputationDMO.Where(t => t.ASMAY_Id == objpge.ASMAY_Id && t.MI_Id == objpge.MI_Id && t.TTSD_Date == objpge.TTSD_Date && t.TTSD_AbsentStaff == objpge.TTSD_AbsentStaff && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id).Count();
                if (result > 0)
                {
                    var result1 = _ttcontext.TT_Staff_DeputationDMO.Single(t => t.ASMAY_Id == objpge.ASMAY_Id && t.MI_Id == objpge.MI_Id && t.TTSD_Date == objpge.TTSD_Date && t.TTSD_AbsentStaff == objpge.TTSD_AbsentStaff && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id);
                    result1.ASMAY_Id = objpge.ASMAY_Id;
                    result1.TTSD_Date = objpge.TTSD_Date;
                    result1.TTSD_AbsentStaff = objpge.TTSD_AbsentStaff;
                    result1.TTSD_DeputedStaff = objpge.TTSD_DeputedStaff;
                    result1.ASMCL_Id = objpge.ASMCL_Id;
                    result1.ASMS_Id = objpge.ASMS_Id;
                    result1.TTMD_Id = objpge.TTMD_Id;
                    result1.TTMP_Id = objpge.TTMP_Id;
                    result1.TTSD_Remarks = objpge.TTSD_Remarks;
                    result1.UpdatedDate = DateTime.Now;
                    _ttcontext.Update(result1);
                    var contactExists = _ttcontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        var Template = "";
                        _category.returnval = true;
                        if (_category.mailflag == true)
                        {
                            var result123 = _ttcontext.TT_Staff_DeputationDMO.Max(t => t.TTSD_Id);
                            for (int z = 0; z <= 1; z++)
                            {
                                string mailId = "";
                                // string mailId = "suryan989@gmail.com";
                                if (z == 0)
                                {
                                    Template = "TTAbsentStaff";
                                    mailId = _ttcontext.Emp_Email_Id.Where(h => h.HRME_Id.Equals(_category.TTSD_AbsentStaff) && h.HRMEM_DeFaultFlag == "default").Select(h => h.HRMEM_EmailId).FirstOrDefault();
                                    //  mailId = "suryan989@gmail.com";
                                }
                                else
                                {
                                    Template = "TTTakingStaff";
                                    mailId = _ttcontext.Emp_Email_Id.Where(h => h.HRME_Id.Equals(_category.TTSD_DeputedStaff) && h.HRMEM_DeFaultFlag == "default").Select(h => h.HRMEM_EmailId).FirstOrDefault();
                                    //    mailId = "suryan989@gmail.com";
                                }
                                sendmail(_category.MI_Id, mailId, Template, result123);
                            }
                        }
                        //Mails Sending Start
                        if (_category.smsflag == true)
                        {
                            var result123 = _ttcontext.TT_Staff_DeputationDMO.Single(t => t.ASMAY_Id == objpge.ASMAY_Id && t.MI_Id == objpge.MI_Id && t.TTSD_Date == objpge.TTSD_Date && t.TTSD_AbsentStaff == objpge.TTSD_AbsentStaff && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id);
                            for (int z = 0; z <= 1; z++)
                            {
                                long contactno = 0;
                                //long contactno = 8095652032;
                                if (z == 0)
                                {
                                    Template = "TTAbsentStaff";
                                    contactno = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(_category.TTSD_AbsentStaff) && h.HRMEMNO_DeFaultFlag == "default").Select(h => h.HRMEMNO_MobileNo).FirstOrDefault();
                                    //  contactno = 9686358569;
                                }
                                else
                                {
                                    Template = "TTTakingStaff";
                                    contactno = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(_category.TTSD_DeputedStaff) && h.HRMEMNO_DeFaultFlag == "default").Select(h => h.HRMEMNO_MobileNo).FirstOrDefault();
                                    //   contactno = 9686358569;
                                }

                                if (contactno != 0)
                                {
                                   // SMS sms = new SMS(_db);
                                    string s = sendSms_TT(_category.MI_Id, contactno, Template, result123.TTSD_Id).Result;
                                    //a.sendSms(_category.MI_Id, contactno, Template, result123.TTSD_Id);
                                }
                            }
                        }


                        if (_category.NOT_Flag == true)
                        {

                            var keylist = _ttcontext.MobileApplAuthenticationDMO.Where(t => t.MI_Id == objpge.MI_Id).Distinct().ToList();

                            if (keylist.Count > 0)
                            {

                           

                                if (_category.deviceid != null && _category.deviceid != "")
                                {
                                    long mobno = 0;
                                    var contactnolist = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(_category.TTSD_DeputedStaff) && h.HRMEMNO_DeFaultFlag == "default").ToList();
                                    if (contactnolist.Count > 0)
                                    {
                                        mobno = contactnolist[0].HRMEMNO_MobileNo;
                                    }
                                    var result123 = _ttcontext.TT_Staff_DeputationDMO.Single(t => t.ASMAY_Id == objpge.ASMAY_Id && t.MI_Id == objpge.MI_Id && t.TTSD_Date == objpge.TTSD_Date && t.TTSD_DeputedStaff == objpge.TTSD_DeputedStaff && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id);

                                    int e = callnotification(objpge.TTSD_DeputedStaff, _category.deviceid, mobno, objpge.MI_Id, "", "", keylist[0].MAAN_AuthenticationKey, "Staff", result123.TTSD_Id, "P");
                                }

                                var empmobid = _ttcontext.HR_Master_Employee_DMO.Where(t => t.MI_Id == objpge.MI_Id && t.HRME_Id == objpge.TTSD_AbsentStaff).ToList();

                                if (empmobid.Count > 0)
                                {
                                    

                                    if (empmobid[0].HRME_AppDownloadedDeviceId != null && empmobid[0].HRME_AppDownloadedDeviceId != "")
                                    {

                                        long mobno = 0;
                                        var contactnolist = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(_category.TTSD_AbsentStaff) && h.HRMEMNO_DeFaultFlag == "default").ToList();
                                        if (contactnolist.Count > 0)
                                        {
                                            mobno = contactnolist[0].HRMEMNO_MobileNo;
                                        }
                                        var result123 = _ttcontext.TT_Staff_DeputationDMO.Single(t => t.ASMAY_Id == objpge.ASMAY_Id && t.MI_Id == objpge.MI_Id && t.TTSD_Date == objpge.TTSD_Date && t.TTSD_AbsentStaff == objpge.TTSD_AbsentStaff && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id);

                                        int e = callnotification(objpge.TTSD_AbsentStaff, empmobid[0].HRME_AppDownloadedDeviceId, mobno, objpge.MI_Id, "", "", keylist[0].MAAN_AuthenticationKey, "Staff", result123.TTSD_Id, "A");



                                    }
                                }




                            }

                    
                        }

                    }
                    else
                    {
                        _category.returnval = false;
                    }
                }
                else if (result == 0)
                {
                    objpge.CreatedDate = DateTime.Now;
                    objpge.UpdatedDate = DateTime.Now;
                    _ttcontext.Add(objpge);
                    var contactExists = _ttcontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        var Template = "";
                        _category.returnval = true;
                        // mail_for_deputation(_category);
                        if (_category.mailflag == true)
                        {
                            var result123 = _ttcontext.TT_Staff_DeputationDMO.Max(t => t.TTSD_Id);
                            for (int z = 0; z <= 1; z++)
                            {
                                string mailId = "";
                                 // mailId = "praveend114@gmail.com";
                                if (z == 0)
                                {
                                    Template = "TTAbsentStaff";
                                    mailId = _ttcontext.Emp_Email_Id.Where(h => h.HRME_Id.Equals(_category.TTSD_AbsentStaff) && h.HRMEM_DeFaultFlag == "default").Select(h => h.HRMEM_EmailId).FirstOrDefault();
                                    // mailId = "praveend114@gmail.com";
                                }
                                else
                                {
                                    Template = "TTTakingStaff";
                                    mailId = _ttcontext.Emp_Email_Id.Where(h => h.HRME_Id.Equals(_category.TTSD_DeputedStaff) && h.HRMEM_DeFaultFlag == "default").Select(h => h.HRMEM_EmailId).FirstOrDefault();
                               // mailId = "praveend114@gmail.com";
                                }
                                sendmail(_category.MI_Id, mailId, Template, result123);
                            }
                        }
                        //Mails Sending Start
                        if (_category.smsflag == true)
                        {
                            var result123 = _ttcontext.TT_Staff_DeputationDMO.Max(t => t.TTSD_Id);
                            for (int z = 0; z <= 1; z++)
                            {
                                long contactno = 0;
                             // contactno = 9591081840;
                                if (z == 0)
                                {
                                    Template = "TTAbsentStaff";
                                    contactno = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(_category.TTSD_AbsentStaff) && h.HRMEMNO_DeFaultFlag == "default").Select(h => h.HRMEMNO_MobileNo).FirstOrDefault();
                                     // contactno = 9591081840;
                                }
                                else
                                {
                                    Template = "TTTakingStaff";
                                    contactno = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(_category.TTSD_DeputedStaff) && h.HRMEMNO_DeFaultFlag == "default").Select(h => h.HRMEMNO_MobileNo).FirstOrDefault();
                                   // contactno = 9591081840;
                                }
                                if (contactno != 0)
                                {
                                   // SMS sms = new SMS(_db);

                                    string s = sendSms_TT(_category.MI_Id, contactno, Template, result123).Result;
                                  //  a.sendSms(_category.MI_Id, contactno, Template, result123);
                                }
                            }
                        }

                        if (_category.NOT_Flag == true)
                        {

                            var keylist = _ttcontext.MobileApplAuthenticationDMO.Where(t => t.MI_Id == objpge.MI_Id).Distinct().ToList();

                            if (keylist.Count > 0)
                            {

                                

                                if (_category.deviceid !=null && _category.deviceid !="")
                                {
                                    long mobno = 0;
                                  var  contactnolist = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(_category.TTSD_DeputedStaff) && h.HRMEMNO_DeFaultFlag == "default").ToList();
                                    if (contactnolist.Count > 0)
                                    {
                                        mobno = contactnolist[0].HRMEMNO_MobileNo;
                                    }
                                    var result123 = _ttcontext.TT_Staff_DeputationDMO.Single(t => t.ASMAY_Id == objpge.ASMAY_Id && t.MI_Id == objpge.MI_Id && t.TTSD_Date == objpge.TTSD_Date && t.TTSD_DeputedStaff == objpge.TTSD_DeputedStaff && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id);

                                    int e = callnotification(objpge.TTSD_DeputedStaff, _category.deviceid, mobno, objpge.MI_Id, "", "", keylist[0].MAAN_AuthenticationKey, "Staff", result123.TTSD_Id,"P");
                                }

                                var empmobid = _ttcontext.HR_Master_Employee_DMO.Where(t => t.MI_Id == objpge.MI_Id && t.HRME_Id == objpge.TTSD_AbsentStaff).ToList();

                                if (empmobid.Count>0)
                                {
                                   

                                    if (empmobid[0].HRME_AppDownloadedDeviceId !=null && empmobid[0].HRME_AppDownloadedDeviceId !="")
                                    {

                                        long mobno = 0;
                                        var contactnolist = _ttcontext.Emp_MobileNo.Where(h => h.HRME_Id.Equals(_category.TTSD_AbsentStaff) && h.HRMEMNO_DeFaultFlag == "default").ToList();
                                        if (contactnolist.Count > 0)
                                        {
                                            mobno = contactnolist[0].HRMEMNO_MobileNo;
                                        }
                                        var result123 = _ttcontext.TT_Staff_DeputationDMO.Single(t => t.ASMAY_Id == objpge.ASMAY_Id && t.MI_Id == objpge.MI_Id && t.TTSD_Date == objpge.TTSD_Date && t.TTSD_AbsentStaff == objpge.TTSD_AbsentStaff && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id);

                                        int e = callnotification(objpge.TTSD_AbsentStaff, empmobid[0].HRME_AppDownloadedDeviceId, mobno, objpge.MI_Id, "", "", keylist[0].MAAN_AuthenticationKey, "Staff", result123.TTSD_Id, "A");



                                    }
                                }




                            }

                        }
                    }
                    else
                    {
                        _category.returnval = false;
                    }
                }
                //end ...........
            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return _category;
        }
        //public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID)
        //{
        //    try
        //    {

        //        mobileNo = 9591081840;
        //        Dictionary<string, string> val = new Dictionary<string, string>();

        //        var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

        //        if (template.Count == 0)
        //        {
        //            return "SMS Template not Mapped to the selected Institution";
        //        }


        //        var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

        //        var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

        //        var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

        //        string sms = template.FirstOrDefault().ISES_SMSMessage;

        //        string result = sms;

        //        List<Match> variables = new List<Match>();

        //        foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
        //        {
        //            variables.Add(match);
        //        }

        //        if (Template == "EMAILOTP")
        //        {
        //            result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
        //            sms = result;
        //        }
        //        else
        //        {
        //            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
        //            {


        //                cmd.CommandText = "SMSMAILPARAMETER";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@UserID",
        //                    SqlDbType.BigInt)
        //                {
        //                    Value = UserID
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@template",
        //                   SqlDbType.VarChar)
        //                {
        //                    Value = Template
        //                });


        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                try
        //                {
        //                    using (var dataReader = cmd.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            var dataRow = new ExpandoObject() as IDictionary<string, object>;
        //                            for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
        //                            {
        //                                dataRow.Add(
        //                                    dataReader.GetName(iFiled),
        //                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
        //                                );
        //                                var datatype = dataReader.GetFieldType(iFiled);
        //                                if (datatype.Name == "DateTime")
        //                                {
        //                                    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
        //                                    val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
        //                                }
        //                                else
        //                                {
        //                                    val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
        //                                }
        //                            }
        //                        }

        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    return ex.Message;
        //                }

        //            }

        //            for (int j = 0; j < ParamaetersName.Count; j++)
        //            {
        //                for (int p = 0; p < val.Count; p++)
        //                {
        //                    if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
        //                    {
        //                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
        //                        sms = result;
        //                    }
        //                }
        //            }

        //            sms = result;
        //        }


        //        List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
        //        alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

        //        if (alldetails.Count > 0)
        //        {
        //            string url = alldetails[0].IVRMSD_URL.ToString();

        //            string PHNO = mobileNo.ToString();

        //            url = url.Replace("PHNO", PHNO);

        //            url = url.Replace("MESSAGE", sms);

        //            System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
        //            System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
        //            Stream stream = response.GetResponseStream();

        //            StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
        //            string responseparameters = readStream.ReadToEnd();
        //            var myContent = JsonConvert.SerializeObject(responseparameters);

        //            dynamic responsedata = JsonConvert.DeserializeObject(myContent);
        //            string messageid = responsedata;

        //        //    #region
        //        //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
        //        //    {
        //        //        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
        //        //        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
        //        //        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();
        //        //        cmd.CommandText = "IVRM_SMS_Outgoing";
        //        //        cmd.CommandType = CommandType.StoredProcedure;
        //        //        cmd.Parameters.Add(new SqlParameter("@MobileNo",
        //        //            SqlDbType.NVarChar)
        //        //        {
        //        //            Value = PHNO
        //        //        });
        //        //        cmd.Parameters.Add(new SqlParameter("@Message",
        //        //           SqlDbType.NVarChar)
        //        //        {
        //        //            Value = sms
        //        //        });
        //        //        cmd.Parameters.Add(new SqlParameter("@module",
        //        //        SqlDbType.VarChar)
        //        //        {
        //        //            Value = modulename[0]
        //        //        });
        //        //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
        //        //       SqlDbType.BigInt)
        //        //        {
        //        //            Value = MI_Id
        //        //        });
        //        //        cmd.Parameters.Add(new SqlParameter("@status",
        //        //   SqlDbType.VarChar)
        //        //        {
        //        //            Value = "Delivered"
        //        //        });
        //        //        cmd.Parameters.Add(new SqlParameter("@Message_id",
        //        //SqlDbType.VarChar)
        //        //        {
        //        //            Value = messageid
        //        //        });
        //        //        if (cmd.Connection.State != ConnectionState.Open)
        //        //            cmd.Connection.Open();
        //        //        try
        //        //        {
        //        //            using (var dataReader = cmd.ExecuteReader())
        //        //            {
        //        //            }
        //        //        }
        //        //        catch (Exception ex)
        //        //        {
        //        //            return ex.Message;
        //        //        }
        //        //    }
        //        //    #endregion
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return ex.Message;
        //    }
        //    return "success";
        //}

        public string sendmail(long MI_Id, string Email, string Template, long UserID)
        {
            try
            {

               // Email = "praveend114@gmail.com";

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
                    //for (int j = 0; j < ParamaetersName.Count; j++)
                    //{
                    //    for (int p = 0; p < val.Count; p++)
                    //    {
                    //        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                    //        {
                    //            result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                    //            Mailmsg = result;
                    //        }
                    //    }
                    //}

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                //result = sms.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
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
        public TTDeputationDTO get_period_alloted(TTDeputationDTO _category)
        {
            TTDeputationDTO objpge = Mapper.Map<TTDeputationDTO>(_category);
            List<TTDeputationDTO> list = new List<TTDeputationDTO>();

            try
            {

                objpge.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(objpge.MI_Id)).ToList().ToArray();

                objpge.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

                objpge.Time_Table = (from a in _ttcontext.TT_Master_DayDMO
                                     from b in _ttcontext.TT_Master_PeriodDMO
                                     from c in _ttcontext.School_M_Class
                                     from d in _ttcontext.School_M_Section
                                     from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                     from f in _ttcontext.HR_Master_Employee_DMO
                                     from g in _ttcontext.TT_Final_GenerationDMO
                                     from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from ii in _ttcontext.TTMasterCategoryDMO
                                     where (g.MI_Id == objpge.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && ii.MI_Id == g.MI_Id && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.HRME_Id == objpge.HRME_Id && g.ASMAY_Id == objpge.ASMAY_Id && h.TTMD_Id == objpge.TTMD_Id)
                                     // && g.TTMC_Id == objpge.TTMC_Id
                                     select new TTDeputationDTO
                                     {
                                         TTFGD_Id = h.TTFGD_Id,
                                         TTFG_Id = g.TTFG_Id,
                                         ASMCL_Id = h.ASMCL_Id,
                                         ASMS_Id = h.ASMS_Id,
                                         HRME_Id = h.HRME_Id,
                                         ISMS_Id = h.ISMS_Id,
                                         TTMD_Id = h.TTMD_Id,
                                         TTMP_Id = h.TTMP_Id,
                                         TTMC_Id = g.TTMC_Id,
                                         ASMAY_Id = g.ASMAY_Id,
                                         TTMD_DayName = a.TTMD_DayName,
                                         TTMP_PeriodName = b.TTMP_PeriodName,
                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                         ASMC_SectionName = d.ASMC_SectionName,
                                         staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),                      
                                         ISMS_SubjectName = e.ISMS_SubjectName,
                                         TTMC_CategoryName = ii.TTMC_CategoryName,

                                     }
                                  ).Distinct().OrderBy(f => f.TTMP_Id).ToArray();



                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_AbsentStaffDeputationCount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = objpge.TTSD_Date
                    });


                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = objpge.HRME_Id
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
                        objpge.absentdpcount = retObject.ToArray();
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


            return _category;
        }

        public TTDeputationDTO get_free_stfdets(TTDeputationDTO _category)
        {

            TTDeputationDTO objpge = Mapper.Map<TTDeputationDTO>(_category);
            List<TTDeputationDTO> list = new List<TTDeputationDTO>();
            try
            {

                objpge.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(objpge.MI_Id)).ToList().ToArray();

                objpge.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();


                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_SCHOOL_GET_DEPUTATION_LIST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.ASMAY_Id
                    });
                    
                    cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = objpge.TTMD_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTMP_Id",
                   SqlDbType.NVarChar)
                    {
                        Value = objpge.TTMP_Id
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
                        objpge.Time_Table = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_StaffDeputationCount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = objpge.TTSD_Date
                    });


                    cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = objpge.TTMD_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TTMP_Id",
                   SqlDbType.NVarChar)
                    {
                        Value = objpge.TTMP_Id
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
                        objpge.dpcount = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }





                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_AbsentStaffDeputationCount_ALL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StaffDate",
                    SqlDbType.Date)
                    {
                        Value = objpge.TTSD_Date
                    });


                   // cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                   // SqlDbType.NVarChar)
                   // {
                   //     Value = objpge.TTMD_Id
                   // });

                   // cmd.Parameters.Add(new SqlParameter("@TTMP_Id",
                   //SqlDbType.NVarChar)
                   // {
                   //     Value = objpge.TTMP_Id
                   // });


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
                        objpge.absentstfcnt = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_DEPUTATION_WEEKLY_PERIOD_COUNT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = objpge.ASMAY_Id
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
                        objpge.weeklycntlist = retObject.ToArray();
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


            return _category;
        }

        public TTDeputationDTO getalldetailsviewrecords2(TTDeputationDTO data)
        {
            try
            {
                data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

                data.Time_Table = (from a in _ttcontext.TT_Master_DayDMO
                                   from b in _ttcontext.TT_Master_PeriodDMO
                                   from c in _ttcontext.School_M_Class
                                   from d in _ttcontext.School_M_Section
                                   from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                   from f in _ttcontext.HR_Master_Employee_DMO
                                   from g in _ttcontext.TT_Final_GenerationDMO
                                   from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                   from i in _ttcontext.TTMasterCategoryDMO
                                   where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id)
                                   //&& g.TTMC_Id==data.TTMC_Id
                                   select new TTDeputationDTO
                                   {
                                       TTFGD_Id = h.TTFGD_Id,
                                       TTFG_Id = g.TTFG_Id,
                                       ASMCL_Id = h.ASMCL_Id,
                                       ASMS_Id = h.ASMS_Id,
                                       HRME_Id = h.HRME_Id,
                                       ISMS_Id = h.ISMS_Id,
                                       TTMD_Id = h.TTMD_Id,
                                       TTMP_Id = h.TTMP_Id,
                                       TTMC_Id = g.TTMC_Id,
                                       TTMD_DayName = a.TTMD_DayName,
                                       TTMP_PeriodName = b.TTMP_PeriodName,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMC_SectionName = d.ASMC_SectionName,
                                       ISMS_SubjectName = e.ISMS_SubjectName,
                                       staffName = f.HRME_EmployeeFirstName + " " + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + " " + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                       TTMC_CategoryName = i.TTMC_CategoryName,

                                   }
                              ).Distinct().ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public TTDeputationDTO viewrecordspopup9(TTDeputationDTO data)
        {
            try
            {
                data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true && t.TTMD_Id==data.TTMD_Id).ToArray();

                data.Time_Table = (from a in _ttcontext.TT_Master_DayDMO
                                   from b in _ttcontext.TT_Master_PeriodDMO
                                   from c in _ttcontext.School_M_Class
                                   from d in _ttcontext.School_M_Section
                                   from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                   from f in _ttcontext.HR_Master_Employee_DMO
                                   from g in _ttcontext.TT_Final_GenerationDMO
                                   from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                   from i in _ttcontext.TTMasterCategoryDMO
                                   where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id)
                                  && h.TTMD_Id == data.TTMD_Id
                                   select new TTDeputationDTO
                                   {
                                       TTFGD_Id = h.TTFGD_Id,
                                       TTFG_Id = g.TTFG_Id,
                                       ASMCL_Id = h.ASMCL_Id,
                                       ASMS_Id = h.ASMS_Id,
                                       HRME_Id = h.HRME_Id,
                                       ISMS_Id = h.ISMS_Id,
                                       TTMD_Id = h.TTMD_Id,
                                       TTMP_Id = h.TTMP_Id,
                                       TTMC_Id = g.TTMC_Id,
                                       TTMD_DayName = a.TTMD_DayName,
                                       TTMP_PeriodName = b.TTMP_PeriodName,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMC_SectionName = d.ASMC_SectionName,
                                       ISMS_SubjectName = e.ISMS_SubjectName,
                                       staffName = f.HRME_EmployeeFirstName + " " + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + " " + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                       TTMC_CategoryName = i.TTMC_CategoryName,

                                   }
                              ).Distinct().ToArray();

            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public TTDeputationDTO viewdeputation(TTDeputationDTO data)
        {
            try
            {
                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_SCHOOL_VIEW_STAFF_DEPUTATION_DETAILS";
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
        public TTDeputationDTO viewabsent(TTDeputationDTO data)
        {
            try
            {
                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_SCHOOL_VIEW_ABSENTSTAFF_DEPUTE_DETAILS";
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


        public TTDeputationDTO getabsentstaff(TTDeputationDTO data)
        {
            try
            {
                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_SCHOOL_GET_ABSENTSTAFF_FOR_DEPUTATION";
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
        public TTDeputationDTO getdetails(int id)
        {
            TTDeputationDTO data = new TTDeputationDTO();
            try
            {
                data.acayear = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(z=>z.ASMAY_Order).ToList().Distinct().ToArray();

                data.categorylist = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == id && t.TTMC_ActiveFlag == true).ToList().Distinct().ToArray();

                List<TTDeputationDTO> result = new List<TTDeputationDTO>();

                //to get data according to search criteria.
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_Staffs_Names_get";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = id });
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
                        Console.Write(ex.Message);
                    }
                }
             

                data.classlist = _ttcontext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList().ToArray();

                data.daylist = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(id) && t.TTMD_ActiveFlag == true).ToList().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public async Task<string> sendSms_TT(long MI_Id, long mobileNo, string Template, long UserID)
        {

            try
            {
                //mobileNo = 9591081840;
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

                    //for (int j = 0; j < ParamaetersName.Count; j++)
                    //{
                    //    for (int p = 0; p < val.Count; p++)
                    //    {
                    //        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                    //        {
                    //            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                    //            sms = result;
                    //        }
                    //    }
                    //}

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                //result = sms.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
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
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public int callnotification(long AMST_Id, string devicenew, long mobileno, long mi_id, string headername, string text, string key, string type,long TTSD_Id, string type1)
        {

            string transid = "";
            string result = "";
            int cnt = 0;
            try
            {

                

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_DEPUTATION_NOTIFICATION_MSG";
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

                        if (viewdata.Count>0)
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
