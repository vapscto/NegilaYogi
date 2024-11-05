using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Data.SqlClient;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.Dynamic;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Fees;
using CollegeFeeService.com.vaps.Interfaces;
using DomainModel.Model.com.vaps.admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;
using Newtonsoft.Json;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegeDefaultersReportImpl : CollegeDefaultersReportInterface
    {

        public CollFeeGroupContext _ClgAdmissionContext;
        readonly ILogger<CollegeDefaultersReportImpl> _logger;
        public CollegeDefaultersReportImpl(CollFeeGroupContext _ClgAdmissionCon, ILogger<CollegeDefaultersReportImpl> log)
        {
            _logger = log;
            _ClgAdmissionContext = _ClgAdmissionCon;

        }


        public CollegeConcessionDTO getdetails(CollegeConcessionDTO dt)
        {
            // CollegeConcessionDTO data = new CollegeConcessionDTO();
            try
            {


                var year = _ClgAdmissionContext.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == dt.MI_Id).ToList();
                dt.yearlst = year.Distinct().GroupBy(y => y.ASMAY_Year).Select(y => y.First()).ToArray();

                dt.grouplist = (from a in _ClgAdmissionContext.FeeGroupClgDMO
                                from b in _ClgAdmissionContext.FeeYearGroupDMO
                                where (a.FMG_Id == b.FMG_Id && a.MI_Id == dt.MI_Id && b.ASMAY_Id == dt.ASMAY_Id)
                                select new FeeGroupDMO
                                {
                                    FMG_Id = a.FMG_Id,
                                    FMG_GroupName = a.FMG_GroupName
                                }
                    ).Distinct().ToArray();


                List<FeeHeadClgDMO> headlist = new List<FeeHeadClgDMO>();
                headlist = _ClgAdmissionContext.FeeHeadClgDMO.Where(h => h.FMH_ActiveFlag == true && h.MI_Id == dt.MI_Id).ToList();
                dt.fillfeehead = headlist.Distinct().ToArray();

                List<School_M_Section> section = new List<School_M_Section>();
                dt.fillsection = (
                                  from b in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                  where ( b.MI_Id == dt.MI_Id)
                                  select new CollegeConcessionDTO
                                  {
                                      AMSC_Id = b.ACMS_Id,
                                      ASMC_SectionName = b.ACMS_SectionName
                                  }
                          ).Distinct().ToArray();

                // string m = sendmailDuedateRemainder(5, "kavita@vapstech.com", "DUEDATEREMAINDER", 55);
         //   string n = duedatesmstrigger(5, 8147132010, "OverDueRemainder", 51);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dt;
        }



        public CollegeConcessionDTO get_courses(CollegeConcessionDTO data)
        {
            try
            {

                data.courselist = (from a in _ClgAdmissionContext.MasterCourseDMO
                                   from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_courses :" + ex.Message);
            }
            return data;
        }
        public CollegeConcessionDTO get_branches(CollegeConcessionDTO data)
        {

            try
            {
                var branchlist = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                  from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag && data.AMCO_Ids.Contains(b.AMCO_Id))
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
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_branches :" + ex.Message);
            }
            return data;
        }
        public CollegeConcessionDTO get_semisters(CollegeConcessionDTO data)
        {
            try
            {
                var amco_ids = "";
                foreach (var x in data.AMCO_Ids)
                {
                    amco_ids += x + ",";
                }
                amco_ids = amco_ids.Substring(0, (amco_ids.Length - 1));


                var amb_ids = "";
                foreach (var x in data.AMB_Ids)
                {
                    amb_ids += x + ",";
                }
                amb_ids = amb_ids.Substring(0, (amb_ids.Length - 1));

                using (var cmd1 = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "College_Semester_Select";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@amco_ids",
                        SqlDbType.VarChar)
                    {
                        Value = amco_ids

                    });
                    cmd1.Parameters.Add(new SqlParameter("@amb_ids",
                        SqlDbType.VarChar)
                    {
                        Value = amb_ids

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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader1.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader1.GetName(iFiled1),
                                        dataReader1.IsDBNull(iFiled1) ? 0 : dataReader1[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.semisterlist = retObject1.ToArray();
                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Atten_Batch_Mapping  get_semisters :" + ex.Message);
            }
            return data;
        }



        public async Task<CollegeConcessionDTO> radiobtndata(CollegeConcessionDTO temp)
        {
            long fmgg_id = 0;
            var amco_ids = "";
            foreach (var x in temp.AMCO_Ids)
            {
                amco_ids += x + ",";
            }
            amco_ids = amco_ids.Substring(0, (amco_ids.Length - 1));


            var amb_ids = "";
            foreach (var x in temp.AMB_Ids)
            {
                amb_ids += x + ",";
            }
            amb_ids = amb_ids.Substring(0, (amb_ids.Length - 1));

            var fmg_id = "";
            foreach (var x in temp.FMG_Ids)
            {
                fmg_id += x + ",";
            }
            fmg_id = fmg_id.Substring(0, (fmg_id.Length - 1));

            var amse_ids = "";
            foreach (var x in temp.AMSE_Ids)
            {
                amse_ids += x + ",";
            }
            amse_ids = amse_ids.Substring(0, (amse_ids.Length - 1));

            if (temp.reporttype == "year")
            {
                temp.Fromdate = Convert.ToDateTime("01 / 01 / 2017");
                //temp.To_Date = Convert.ToDateTime("01 / 01 / 2017");
                //  temp.duedate = "null";
            }

            //  temp.date = duadatecollect(temp.ASMAY_Id, temp.userid, temp.AMSE_Id);


            using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "College_defaulters_report";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@fmg_id",
                    SqlDbType.VarChar)
                {
                    // Value = coloumns
                    Value = fmg_id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                   SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(temp.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@amco_ids",
               SqlDbType.VarChar)
                {
                    Value = amco_ids
                });

                cmd.Parameters.Add(new SqlParameter("@option",
                   SqlDbType.VarChar)
                {
                    Value = temp.radioval
                });
                cmd.Parameters.Add(new SqlParameter("@active",
              SqlDbType.VarChar)
                {
                    Value = temp.active
                });
                cmd.Parameters.Add(new SqlParameter("@deactive",
              SqlDbType.VarChar)
                {
                    Value = temp.deactive
                });
                cmd.Parameters.Add(new SqlParameter("@left",
              SqlDbType.VarChar)
                {
                    Value = temp.left
                });

                cmd.Parameters.Add(new SqlParameter("@section",
          SqlDbType.VarChar)
                {
                    Value = temp.AMSC_Id
                });

                cmd.Parameters.Add(new SqlParameter("@userid",
         SqlDbType.VarChar)
                {
                    Value = temp.userid
                });

                cmd.Parameters.Add(new SqlParameter("@amb_ids",
                  SqlDbType.VarChar)
                {
                    // Value = coloumns
                    Value = amb_ids
                });
                cmd.Parameters.Add(new SqlParameter("@amse_ids",
            SqlDbType.VarChar)
                {
                    Value = amse_ids
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {

                    // var data = cmd.ExecuteNonQuery();

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
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

                    temp.alldata = retObject.ToArray();

                    List<SMSEmailSetting> smsemailset = new List<SMSEmailSetting>();
                    smsemailset = _ClgAdmissionContext.smsEmailSetting.Where(y => y.MI_Id == temp.MI_Id && y.ISES_Template_Name == "FEEDEFAULTREPORT").ToList();
                    temp.smsemailsettings = smsemailset.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return temp;
            }


        }

        public string duadatecollect(long asmay_id, long userid, long fillclasflg)
        {
            string date = "";
            string v = "";
            List<FeetransactionSMS> Due_Date_array = new List<FeetransactionSMS>();
            List<FeetransactionSMS> result_duadate = new List<FeetransactionSMS>();
            using (var cmdnew = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
            {
                cmdnew.CommandText = "DUE_DATE_CALCULATION_test";
                cmdnew.CommandType = CommandType.StoredProcedure;
                cmdnew.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = asmay_id });
                cmdnew.Parameters.Add(new SqlParameter("@FMT_Id", SqlDbType.VarChar) { Value = userid });
                cmdnew.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = fillclasflg });
                if (cmdnew.Connection.State != ConnectionState.Open)
                    cmdnew.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReadernew = cmdnew.ExecuteReader())
                    {
                        while (dataReadernew.Read())
                        {
                            result_duadate.Add(new FeetransactionSMS
                            {
                                Due_Date = dataReadernew["duedate"].ToString(),
                            });
                            Due_Date_array = result_duadate.ToList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            if (Due_Date_array.Count == 0)
            {
                v = "";

            }
            else
            {
                date = Due_Date_array[0].Due_Date;
                v = date.Substring(0, 10);

            }

            return v;
        }
        public CollegeConcessionDTO DuedateRemainder(CollegeConcessionDTO data)
        {
            List<CollegeConcessionDTO> devicelist = new List<CollegeConcessionDTO>();
            List<CollegeConcessionDTO> deviceidsnew = new List<CollegeConcessionDTO>();
            try
            {
                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SP_DUEDATE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                 SqlDbType.VarChar)
                    {
                        Value = "DUEDATE"
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
                                devicelist.Add(new CollegeConcessionDTO
                                {

                                    AMCST_MobileNo = Convert.ToInt64(dataReader["AMCST_MobileNo"]),
                                    AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                    AMCST_emailId = Convert.ToString(dataReader["AMCST_emailId"]),


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
                    string m = sendmailDuedateRemainder(deviceid.MI_Id, deviceid.AMCST_emailId, "DUEDATEREMAINDER", deviceid.AMCST_Id);
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }



        public CollegeConcessionDTO duedateExcededRemainder(CollegeConcessionDTO data)
        {
            List<CollegeConcessionDTO> devicelist = new List<CollegeConcessionDTO>();
            List<CollegeConcessionDTO> deviceidsnew = new List<CollegeConcessionDTO>();
            try
            {
                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SP_DUEDATE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                 SqlDbType.VarChar)
                    {
                        Value = "OverDue"
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
                                devicelist.Add(new CollegeConcessionDTO
                                {

                                    AMCST_MobileNo = Convert.ToInt64(dataReader["AMCST_MobileNo"]),
                                    AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                    AMCST_emailId = Convert.ToString(dataReader["AMCST_emailId"]),


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
                    string m = sendmailDuedated(deviceid.MI_Id, deviceid.AMCST_emailId, "OverDueRemainder", deviceid.AMCST_Id);
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public string sendmailDuedateRemainder(long MI_Id, string Email, string Template, long AMCST_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _ClgAdmissionContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _ClgAdmissionContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _ClgAdmissionContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _ClgAdmissionContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

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


                    //add
                    using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SP_DUEDATERemainder";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                 SqlDbType.VarChar)
                        {
                            Value = "Duedate"
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
                                        //var datatype = dataReader.GetFieldType(iFiled);
                                        //if (datatype.Name == "DateTime")
                                        //{
                                        //    var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                        //    val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        //}
                                        //else
                                        //{
                                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        //}
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
                alldetails = _ClgAdmissionContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _ClgAdmissionContext.GenConfiguration.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

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
                            var img = _ClgAdmissionContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                                    var img = _ClgAdmissionContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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


                    using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _ClgAdmissionContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _ClgAdmissionContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _ClgAdmissionContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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

        public string sendmailDuedated(long MI_Id, string Email, string Template, long AMCST_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _ClgAdmissionContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _ClgAdmissionContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _ClgAdmissionContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _ClgAdmissionContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

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
                    using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SP_DUEDATERemainder";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                 SqlDbType.VarChar)
                        {
                            Value = "OverDue"
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
                alldetails = _ClgAdmissionContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

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
                    smstpdetails = _ClgAdmissionContext.GenConfiguration.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

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
                            var img = _ClgAdmissionContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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
                                    var img = _ClgAdmissionContext.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

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


                    using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _ClgAdmissionContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _ClgAdmissionContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _ClgAdmissionContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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



        public async Task<FeetransactionSMS> sendsms(FeetransactionSMS data)
        {
            try
            {
                foreach (FeetransactionSMS y in data.selectedStudentList)
                {
                    string PHNO = y.AMCST_MobileNo.ToString();
                    // string PHNO = "7899248169";
                    List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                    alldetails = _ClgAdmissionContext.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_ID)).ToList();

                    if (alldetails.Count > 0)
                    {
                        try
                        {
                            Dictionary<string, string> val = new Dictionary<string, string>();

                            var template = _ClgAdmissionContext.smsEmailSetting.Where(e => e.MI_Id == data.MI_ID && e.ISES_Template_Name == "FEEDEFAULTREPORT" && e.ISES_SMSActiveFlag == true).ToList();

                            if (template.Count == 0)
                            {

                            }
                            var institutionName = _ClgAdmissionContext.Institution.Where(i => i.MI_Id == data.MI_ID).ToList();

                            var Paramaeters = _ClgAdmissionContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_ID && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(i => i.ISMP_ID).ToList();

                            var ParamaetersName = _ClgAdmissionContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                            string sms = template.FirstOrDefault().ISES_SMSMessage;

                            string result = sms;

                            List<Match> variables = new List<Match>();

                            foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                            {
                                variables.Add(match);
                            }
                            //dua date calculation 

                            //end duadatec calculation
                            using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "SMSMAILPARAMETER_TERMS";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@template",
                                   SqlDbType.VarChar)
                                {
                                    Value = "FEEDEFAULTREPORT"
                                });
                                cmd.Parameters.Add(new SqlParameter("@termName",
                                      SqlDbType.VarChar)
                                {
                                    Value = " "
                                });
                                cmd.Parameters.Add(new SqlParameter("@duedate",
                                      SqlDbType.VarChar)
                                {
                                    Value = DateTime.Now
                                });
                                cmd.Parameters.Add(new SqlParameter("@amount",
                                     SqlDbType.VarChar)
                                {
                                    Value = y.totalbalance
                                });
                                cmd.Parameters.Add(new SqlParameter("@insname",
                                   SqlDbType.VarChar)
                                {
                                    Value = institutionName[0].MI_Name
                                });

                                cmd.Parameters.Add(new SqlParameter("@email",
                                   SqlDbType.VarChar)
                                {
                                    Value = (y.paid == null || y.paid == "") ? "0" : y.paid
                                });
                                cmd.Parameters.Add(new SqlParameter("@type",
                             SqlDbType.VarChar)
                                {
                                    Value = "SMS"
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
                                    Console.WriteLine(ex.Message);
                                }

                            }




                            for (int j = 0; j < ParamaetersName.Count; j++)
                            {
                                for (int p = 0; p < val.Count; p++)
                                {
                                    if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                                    {

                                        //result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                        sms = result;
                                    }
                                }
                            }


                            sms = result;



                            List<SMS_DETAILS_DMO> alldetails123 = new List<SMS_DETAILS_DMO>();
                            alldetails123 = _ClgAdmissionContext.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_ID)).ToList();

                            if (alldetails123.Count > 0)
                            {
                                string url = alldetails123[0].IVRMSD_URL.ToString();



                                url = url.Replace("PHNO", PHNO);

                                url = url.Replace("MESSAGE", sms);

                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();

                                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                                string responseparameters = readStream.ReadToEnd();

                                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                                {
                                    var template1010 = _ClgAdmissionContext.smsEmailSetting.Where(e => e.MI_Id == data.MI_ID && e.ISES_Template_Name == "FEEDEFAULTREPORT" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                                    var moduleid = _ClgAdmissionContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                                    var modulename = _ClgAdmissionContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

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
                                        Value = "FEES"
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                   SqlDbType.BigInt)
                                    {
                                        Value = data.MI_ID
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@status",
                               SqlDbType.VarChar)
                                    {
                                        Value = "Delivered"
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@Message_id",
                            SqlDbType.VarChar)
                                    {
                                        Value = responseparameters
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

                    }
                }
                data.msg = "successSMS";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.msg = "failedSMS";
            }
            return data;
        }


        public FeetransactionSMS sendemail(FeetransactionSMS data)
        {
            try
            {
                foreach (FeetransactionSMS d in data.selectedStudentList)
                {
                    string textBody = " ";

                    // var termname = _ClgAdmissionContext.feeTr.Where(t => t.FMT_Id == d.FMT_ID).ToString(); /*data.fmtdeatls.FMT_Name*/
                    try
                    {
                        int T = 0;
                        string name = "";
                        var fmg_ids = "";
                        long fmt_id_new = 0;
                        string terminstall = "";

                        Dictionary<string, string> val = new Dictionary<string, string>();

                        var template = _ClgAdmissionContext.smsEmailSetting.Where(e => e.MI_Id == data.MI_ID && e.ISES_Template_Name == "FEEDEFAULTREPORT" && e.ISES_MailActiveFlag == true).ToList();



                        var institutionName = _ClgAdmissionContext.Institution.Where(i => i.MI_Id == data.MI_ID).ToList();
                        var institutionName_email = _ClgAdmissionContext.Institution_EmailId.Where(i => i.MI_Id == data.MI_ID).ToList();
                        var Paramaeters = _ClgAdmissionContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_ID && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

                        var ParamaetersName = _ClgAdmissionContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                        //     string Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                        string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                        string result = Mailmsg;

                        List<Match> variables = new List<Match>();

                        foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                        {
                            variables.Add(match);
                        }

                        //var query = _ClgAdmissionContext.feeTr.Where(n => n.FMT_Id == fmt_id_new).ToList();

                        //end duadatec calculation
                        using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "SMSMAILPARAMETER_TERMS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@template",
                               SqlDbType.VarChar)
                            {
                                Value = "FEEDEFAULTREPORT"
                            });
                            cmd.Parameters.Add(new SqlParameter("@termName",
                                  SqlDbType.VarChar)
                            {
                                Value = (d.paid == null || d.paid == "") ? "0" : d.paid
                            });
                            cmd.Parameters.Add(new SqlParameter("@duedate",
                                  SqlDbType.VarChar)
                            {
                                Value = DateTime.Now
                            });
                            cmd.Parameters.Add(new SqlParameter("@amount",
                                 SqlDbType.VarChar)
                            {
                                Value = d.totalbalance
                            });
                            cmd.Parameters.Add(new SqlParameter("@insname",
                                  SqlDbType.VarChar)
                            {
                                Value = institutionName[0].MI_Name
                            });

                            cmd.Parameters.Add(new SqlParameter("@email",
                               SqlDbType.VarChar)
                            {
                                Value = institutionName_email[0].MIE_EmailId
                            });
                            cmd.Parameters.Add(new SqlParameter("@type",
                              SqlDbType.VarChar)
                            {
                                Value = "MAIL"
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
                                Console.WriteLine(ex.Message);
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


                        List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                        alldetails = _ClgAdmissionContext.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_ID)).ToList();

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
                            if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                            {
                                string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                            }



                            var message = new SendGridMessage();
                            message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                            message.Subject = Subject;

                            message.AddTo(d.AMCST_emailId);


                            message.HtmlContent = Mailmsg;

                            var client = new SendGridClient(sengridkey);

                            client.SendEmailAsync(message).Wait();


                            using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                            {
                                var template1010 = _ClgAdmissionContext.smsEmailSetting.Where(e => e.MI_Id == data.MI_ID && e.ISES_Template_Name == "FEEDEFAULTREPORT" && e.ISES_SMSActiveFlag == true).Select(e => e.IVRMIM_Id).ToList();

                                var moduleid = _ClgAdmissionContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(i => i.IVRMM_Id).ToList();

                                var modulename = _ClgAdmissionContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(i => i.IVRMM_ModuleName).ToList();

                                cmd.CommandText = "IVRM_Email_Outgoing";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@EmailId",
                                    SqlDbType.NVarChar)
                                {
                                    Value = d.AMCST_emailId
                                    //Value= "kiransoravi@gmail.com"
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
                                    Value = data.MI_ID
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
                }
                data.msg = "success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<CollegeConcessionDTO> duedatesms(CollegeConcessionDTO data)
        {
            List<CollegeConcessionDTO> devicelist = new List<CollegeConcessionDTO>();
            List<CollegeConcessionDTO> deviceidsnew = new List<CollegeConcessionDTO>();
            try
            {
                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SP_DUEDATE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Type",
                 SqlDbType.VarChar)
                    {
                        Value = "OverDue"
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
                                devicelist.Add(new CollegeConcessionDTO
                                {

                                    AMCST_MobileNo = Convert.ToInt64(dataReader["AMCST_MobileNo"]),
                                    AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"]),
                                    AMCST_emailId = Convert.ToString(dataReader["AMCST_emailId"]),


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
                    string m = duedatesmstrigger(deviceid.MI_Id, deviceid.AMCST_MobileNo, "OverDueRemainder", deviceid.AMCST_Id);
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    

        public string duedatesmstrigger(long MI_Id, long Mobile, string Template, long AMCST_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _ClgAdmissionContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _ClgAdmissionContext.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _ClgAdmissionContext.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _ClgAdmissionContext.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                   // result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    //sms = result;
                }
                else
                {
                    using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SP_DUEDATERemainder";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                 SqlDbType.VarChar)
                        {
                            Value = "OverDue"
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
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _ClgAdmissionContext.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _ClgAdmissionContext.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    //URL http://alerts.kaleyra.com/api/v4/?method=sms&api_key={{api_key}}&message=MESSAGE&sender={{sender}}&to=PHNO&entity_id=entityid&template_id=templateid

                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = Mobile.ToString();

                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);
                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response =request.GetResponse() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    
                    using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _ClgAdmissionContext.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _ClgAdmissionContext.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _ClgAdmissionContext.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

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

    }
}
