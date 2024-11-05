using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CommonLibrary;
using System.Threading.Tasks;
using System.Dynamic;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SMSResendImpl : Interfaces.SMSResendInterface
    {
        private static ConcurrentDictionary<string, SMSResendDTO> _login =
        new ConcurrentDictionary<string, SMSResendDTO>();

        private readonly AcademicContext _mastercasteContext;
        private readonly ILogger<SMSResendImpl> _log;
        private readonly DomainModelMsSqlServerContext _db;
        public SMSResendImpl(AcademicContext mastercasteContext, ILogger<SMSResendImpl> log, DomainModelMsSqlServerContext db)
        {
            _mastercasteContext = mastercasteContext;
            _log = log;
            _db = db;
        }

        public SMSResendDTO checksmsstatus(SMSResendDTO data)
        {
            try
            {
                var checkstatusdata = (from a in _db.SMS_Sent_Details
                                       from b in _db.SMS_Sent_Details_LoginUserDMO
                                       from c in _db.SMS_Sent_Details_NowiseDMO
                                       where a.MI_ID == data.MI_ID
                                       && a.SSD_Id == b.SSD_Id
                                       // && b.IVRMUL_Id == data.IVRMUL_Id 
                                       // && a.SSD_HeaderName.Contains(data.SSD_HeaderName)
                                       && a.SSD_Id == c.SSD_Id
                                       //&& a.SSD_TransactionId == data.SSD_TransactionId && a.SSD_SentDate >= data.FromDate && a.SSD_SentDate <= data.ToDate 
                                       && c.SSDNO_Status != "DELIVRD"
                                       select new SMSResendDTO
                                       {
                                           SSD_TransactionId = a.SSD_TransactionId,
                                           SSD_HeaderName = a.SSD_HeaderName,
                                           SSD_SentDate = a.SSD_SentDate,
                                           SSD_Senttime = a.SSD_Senttime,
                                           SSDNO_MobileNo = c.SSDNO_MobileNo,
                                           SSDNO_Status = c.SSDNO_Status,
                                           SSDN_SMSMessage = c.SSDN_SMSMessage,
                                           SSDNO_SMSStatusId = c.SSDNO_SMSStatusId,
                                           SSDNO_DeliveryDate = c.SSDNO_DeliveryDate,
                                           SSDNO_Id = c.SSDNO_Id,
                                           SSDNO_NoOfAttempt = c.SSDNO_NoOfAttempt
                                       }).ToList();
                if (checkstatusdata.Count > 0)
                {
                    var msgid = string.Empty;
                    var mainstatus = string.Empty;
                    var status = string.Empty;
                    DateTime? delitime = null;
                    string msg = string.Empty;
                    var message = string.Empty;
                    //  var query = string.Empty;
                    var workingkey = string.Empty;
                    List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                    alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_ID)).ToList();

                    if (alldetails.Count > 0)
                    {
                        string url = alldetails[0].IVRMSD_URL.ToString();

                        var query = HttpUtility.ParseQueryString(url);

                        workingkey = query.Get("workingkey");
                        for (int i = 0; i < checkstatusdata.Count; i++)
                        {
                            if (checkstatusdata[i].SSDNO_Status != "DELIVRD")
                            {
                                if (checkstatusdata[i].SSDNO_SMSStatusId != "" && checkstatusdata[i].SSDNO_SMSStatusId != null)
                                {
                                    var url1 = "https://api-alerts.solutionsinfini.com/v4/?api_key=" + workingkey + "&method=sms.status&id=" + checkstatusdata[i].SSDNO_SMSStatusId + "";
                                    System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                                    System.Net.HttpWebResponse response1 = request1.GetResponse() as System.Net.HttpWebResponse;
                                    Stream stream1 = response1.GetResponseStream();

                                    StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                                    string responseparameters1 = readStream1.ReadToEnd();
                                    var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                                    dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                                    var statusdetails = JsonConvert.DeserializeObject<CommonLibrary.stausdata>(responsedata1);

                                    if (statusdetails.data.Length > 0)
                                    {
                                        status = statusdetails.data[0].status;
                                        if (statusdetails.data[0].dlrtime != null)
                                        {
                                            delitime = Convert.ToDateTime(statusdetails.data[0].dlrtime);
                                        }
                                        mainstatus = statusdetails.status;
                                        message = statusdetails.message;
                                    }
                                    else
                                    {
                                        status = message = checkstatusdata[i].SSDNO_Status;
                                    }

                                    //var updatestatus = _db.SMS_Sent_Details_NowiseDMO.Single(t => t.SSDNO_Id == data.resenddata[i].SSDNO_Id && t.SSDNO_SMSStatusId.Contains(data.resenddata[i].SSDNO_SMSStatusId));
                                    //updatestatus.

                                    if (status != "AWAITED-DLR")
                                    {
                                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                        {
                                            cmd.CommandText = "IVRM_SMS_Outgoing_Table_Update";
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.Add(new SqlParameter("@SSDNO_Id", SqlDbType.BigInt)
                                            {
                                                Value = checkstatusdata[i].SSDNO_Id
                                            });
                                            cmd.Parameters.Add(new SqlParameter("@MessStatusId", SqlDbType.NVarChar)
                                            {
                                                Value = checkstatusdata[i].SSDNO_SMSStatusId
                                            });
                                            if (delitime == null)
                                            {
                                                cmd.Parameters.Add(new SqlParameter("@deliverytime", SqlDbType.DateTime)
                                                {
                                                    Value = DBNull.Value
                                                });
                                            }
                                            else
                                            {
                                                cmd.Parameters.Add(new SqlParameter("@deliverytime", SqlDbType.DateTime)
                                                {
                                                    Value = delitime
                                                });
                                            }
                                            cmd.Parameters.Add(new SqlParameter("@Status", SqlDbType.NVarChar)
                                            {
                                                Value = status
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
                                                data.retmsg = "error";
                                                return data;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return data;
        }
        public SMSResendDTO Getdetailsstatus(SMSResendDTO data)//int IVRMM_Id
        {
            try
            {
                var headerlist = (from a in _db.SMS_Sent_Details
                                  from b in _db.SMS_Sent_Details_LoginUserDMO
                                  where a.MI_ID == data.MI_ID && a.SSD_Id == b.SSD_Id
                                  select new SMSResendDTO
                                  {
                                      SSD_HeaderName = a.SSD_HeaderName.Trim().ToUpper()
                                  }).Distinct().ToList();

                if (headerlist.Count > 0)
                {
                    data.headerlist = headerlist.Distinct().ToArray();
                }

                var statuslist = (from a in _db.SMS_Sent_Details_NowiseDMO
                                  where a.SSDNO_Status != null && a.SSDNO_Status != "" && a.SSDNO_Status != string.Empty
                                  select new SMSResendDTO
                                  {
                                      SSDNO_Status = a.SSDNO_Status.Trim().ToUpper()
                                  }).Distinct().ToList();


                if (statuslist.Count > 0)
                {
                    data.statuslist = statuslist.Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
            return data;
        }
        public SMSResendDTO Gettransnostatus(SMSResendDTO data)//int IVRMM_Id
        {
            try
            {
                List<string> hname = new List<string>();

                if (data.headsname != null)
                {
                    foreach (var item in data.headsname)
                    {
                        hname.Add(item.SSD_HeaderName);
                    }
                }

                var transnolist = (from a in _db.SMS_Sent_Details
                                   from b in _db.SMS_Sent_Details_LoginUserDMO
                                   where a.MI_ID == data.MI_ID && a.SSD_Id == b.SSD_Id && hname.Contains(a.SSD_HeaderName.Trim().ToUpper())
                                   select new SMSResendDTO
                                   {
                                       SSD_TransactionId = a.SSD_TransactionId,
                                       transname = a.SSD_TransactionId.ToString(),
                                   }).ToList();

                if (transnolist.Count > 0)
                {
                    data.transnolist = transnolist.Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public SMSResendDTO getstatusreport(SMSResendDTO data)//int IVRMM_Id
        {
            try
            {
                string hname = "";
                string transid = "";
                string stname = "";
                if (data.headsname != null)
                {
                    var cn = 0;
                    foreach (var item in data.headsname)
                    {
                        if (cn == 0)
                        {
                            hname = item.SSD_HeaderName;
                        }
                        else
                        {
                            hname = hname + "," + item.SSD_HeaderName;
                        }
                        cn += 1;
                    }
                }

                if (data.transnameid != null)
                {
                    var cn = 0;
                    foreach (var item in data.transnameid)
                    {
                        if (cn == 0)
                        {
                            transid = item.SSD_TransactionId.ToString();
                        }
                        else
                        {
                            transid = transid + "," + item.SSD_TransactionId.ToString();
                        }
                        cn += 1;
                    }
                }
                if (data.statusname != null)
                {
                    var cn = 0;
                    foreach (var item in data.statusname)
                    {
                        if (cn == 0)
                        {
                            stname = item.SSDNO_Status;
                        }
                        else
                        {
                            stname = stname + "," + item.SSDNO_Status;
                        }
                        cn += 1;
                    }
                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SMS_Status_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@transid",SqlDbType.VarChar)
                    {
                        Value = transid
                    });

                    cmd.Parameters.Add(new SqlParameter("@headername",SqlDbType.VarChar)
                    {
                        Value = hname
                    });
                    cmd.Parameters.Add(new SqlParameter("@status",SqlDbType.VarChar)
                    {
                        Value = stname
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",SqlDbType.VarChar)
                    {
                        Value = data.FromDate.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",SqlDbType.VarChar)
                    {
                        Value = data.ToDate.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
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
                        data.fillgriddata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        /// <summary>
        /// sms resend 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>

        public SMSResendDTO Getdetails(SMSResendDTO data)//int IVRMM_Id
        {
            var headerlist = (from a in _db.SMS_Sent_Details
                              from b in _db.SMS_Sent_Details_LoginUserDMO
                              where a.MI_ID == data.MI_ID && a.SSD_Id == b.SSD_Id && b.IVRMUL_Id == data.IVRMUL_Id
                              select new SMSResendDTO
                              {
                                  SSD_HeaderName = a.SSD_HeaderName
                              }).Distinct().ToList();


            if (headerlist.Count > 0)
            {
                data.headerlist = headerlist.Distinct().ToArray();
            }

            return data;
        }

        public SMSResendDTO Gettransno(SMSResendDTO data)//int IVRMM_Id
        {
            var transnolist = (from a in _db.SMS_Sent_Details
                               from b in _db.SMS_Sent_Details_LoginUserDMO
                               where a.MI_ID == data.MI_ID && a.SSD_Id == b.SSD_Id && b.IVRMUL_Id == data.IVRMUL_Id && a.SSD_HeaderName.Contains(data.SSD_HeaderName)
                               select new SMSResendDTO
                               {
                                   SSD_TransactionId = a.SSD_TransactionId
                               }).ToList();

            if (transnolist.Count > 0)
            {
                data.transnolist = transnolist.Distinct().ToArray();
            }

            return data;
        }
        public SMSResendDTO showdata(SMSResendDTO data)//int IVRMM_Id
        {
            try
            {
                if (data.checkst == "status")
                {
                    var checkstatusdata = (from a in _db.SMS_Sent_Details
                                           from b in _db.SMS_Sent_Details_LoginUserDMO
                                           from c in _db.SMS_Sent_Details_NowiseDMO
                                           where a.MI_ID == data.MI_ID && a.SSD_Id == b.SSD_Id && b.IVRMUL_Id == data.IVRMUL_Id && a.SSD_HeaderName.Contains(data.SSD_HeaderName) && a.SSD_Id == c.SSD_Id && a.SSD_TransactionId == data.SSD_TransactionId && a.SSD_SentDate >= data.FromDate && a.SSD_SentDate <= data.ToDate && c.SSDNO_Status != "DELIVRD"
                                           select new SMSResendDTO
                                           {
                                               SSD_TransactionId = a.SSD_TransactionId,
                                               SSD_HeaderName = a.SSD_HeaderName,
                                               SSD_SentDate = a.SSD_SentDate,
                                               SSD_Senttime = a.SSD_Senttime,
                                               SSDNO_MobileNo = c.SSDNO_MobileNo,
                                               SSDNO_Status = c.SSDNO_Status,
                                               SSDN_SMSMessage = c.SSDN_SMSMessage,
                                               SSDNO_SMSStatusId = c.SSDNO_SMSStatusId,
                                               SSDNO_DeliveryDate = c.SSDNO_DeliveryDate,
                                               SSDNO_Id = c.SSDNO_Id,
                                               SSDNO_NoOfAttempt = c.SSDNO_NoOfAttempt
                                           }).ToList();

                    if (checkstatusdata.Count > 0)
                    {
                        var msgid = string.Empty;
                        var mainstatus = string.Empty;
                        var status = string.Empty;
                        DateTime? delitime = null;
                        string msg = string.Empty;
                        var message = string.Empty;
                        //  var query = string.Empty;
                        var workingkey = string.Empty;

                        List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                        alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_ID)).ToList();

                        if (alldetails.Count > 0)
                        {
                            string url = alldetails[0].IVRMSD_URL.ToString();

                            var query = HttpUtility.ParseQueryString(url);

                            workingkey = query.Get("workingkey");
                            for (int i = 0; i < checkstatusdata.Count; i++)
                            {
                                if (checkstatusdata[i].SSDNO_Status != "DELIVRD")
                                {
                                    if (checkstatusdata[i].SSDNO_SMSStatusId != "" && checkstatusdata[i].SSDNO_SMSStatusId != null)
                                    {
                                        var url1 = "https://api-alerts.solutionsinfini.com/v4/?api_key=" + workingkey + "&method=sms.status&id=" + checkstatusdata[i].SSDNO_SMSStatusId + "";
                                        System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                                        System.Net.HttpWebResponse response1 = request1.GetResponse() as System.Net.HttpWebResponse;
                                        Stream stream1 = response1.GetResponseStream();

                                        StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                                        string responseparameters1 = readStream1.ReadToEnd();
                                        var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                                        dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                                        var statusdetails = JsonConvert.DeserializeObject<CommonLibrary.stausdata>(responsedata1);

                                        if (statusdetails.data.Length > 0)
                                        {
                                            status = statusdetails.data[0].status;
                                            if (statusdetails.data[0].dlrtime != null)
                                            {
                                                delitime = Convert.ToDateTime(statusdetails.data[0].dlrtime);
                                            }
                                            mainstatus = statusdetails.status;
                                            message = statusdetails.message;
                                        }
                                        else
                                        {
                                            status = message = checkstatusdata[i].SSDNO_Status;
                                        }

                                        //var updatestatus = _db.SMS_Sent_Details_NowiseDMO.Single(t => t.SSDNO_Id == data.resenddata[i].SSDNO_Id && t.SSDNO_SMSStatusId.Contains(data.resenddata[i].SSDNO_SMSStatusId));
                                        //updatestatus.

                                        if (status != "AWAITED-DLR")
                                        {
                                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                            {
                                                cmd.CommandText = "IVRM_SMS_Outgoing_Table_Update";
                                                cmd.CommandType = CommandType.StoredProcedure;

                                                cmd.Parameters.Add(new SqlParameter("@SSDNO_Id",SqlDbType.BigInt)
                                                {
                                                    Value = checkstatusdata[i].SSDNO_Id
                                                });
                                                cmd.Parameters.Add(new SqlParameter("@MessStatusId",SqlDbType.NVarChar)
                                                {
                                                    Value = checkstatusdata[i].SSDNO_SMSStatusId
                                                });

                                                if (delitime == null)
                                                {
                                                    cmd.Parameters.Add(new SqlParameter("@deliverytime",SqlDbType.DateTime)
                                                    {
                                                        Value = DBNull.Value
                                                    });
                                                }
                                                else
                                                {
                                                    cmd.Parameters.Add(new SqlParameter("@deliverytime",SqlDbType.DateTime)
                                                    {
                                                        Value = delitime
                                                    });
                                                }

                                                cmd.Parameters.Add(new SqlParameter("@Status",SqlDbType.NVarChar)
                                                {
                                                    Value = status
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
                                                    data.retmsg = "error";
                                                    return data;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (data.msgstatus == "ALL")
                    {
                        var fillgriddata = (from a in _db.SMS_Sent_Details
                                            from b in _db.SMS_Sent_Details_LoginUserDMO
                                            from c in _db.SMS_Sent_Details_NowiseDMO
                                            where a.MI_ID == data.MI_ID && a.SSD_Id == b.SSD_Id && b.IVRMUL_Id == data.IVRMUL_Id && a.SSD_HeaderName.Contains(data.SSD_HeaderName) && a.SSD_Id == c.SSD_Id && a.SSD_TransactionId == data.SSD_TransactionId && a.SSD_SentDate >= data.FromDate && a.SSD_SentDate <= data.ToDate
                                            select new SMSResendDTO
                                            {
                                                SSD_TransactionId = a.SSD_TransactionId,
                                                SSD_HeaderName = a.SSD_HeaderName,
                                                SSD_SentDate = a.SSD_SentDate,
                                                SSDNO_MobileNo = c.SSDNO_MobileNo,
                                                SSDNO_Status = c.SSDNO_Status,
                                                SSDN_SMSMessage = c.SSDN_SMSMessage,
                                                SSDNO_SMSStatusId = c.SSDNO_SMSStatusId,
                                                SSDNO_DeliveryDate = c.SSDNO_DeliveryDate,
                                                SSDNO_Id = c.SSDNO_Id,
                                                SSDNO_NoOfAttempt = c.SSDNO_NoOfAttempt
                                            }).ToList();
                        if (fillgriddata.Count > 0)
                        {
                            data.fillgriddata = fillgriddata.Distinct().ToArray();
                        }
                    }
                    else
                    {
                        var fillgriddata = (from a in _db.SMS_Sent_Details
                                            from b in _db.SMS_Sent_Details_LoginUserDMO
                                            from c in _db.SMS_Sent_Details_NowiseDMO
                                            where a.MI_ID == data.MI_ID && a.SSD_Id == b.SSD_Id && b.IVRMUL_Id == data.IVRMUL_Id && a.SSD_HeaderName.Contains(data.SSD_HeaderName) && a.SSD_Id == c.SSD_Id && a.SSD_TransactionId == data.SSD_TransactionId && a.SSD_SentDate >= data.FromDate && a.SSD_SentDate <= data.ToDate && c.SSDNO_Status.Trim() == data.msgstatus.Trim()
                                            select new SMSResendDTO
                                            {
                                                SSD_TransactionId = a.SSD_TransactionId,
                                                SSD_HeaderName = a.SSD_HeaderName,
                                                SSD_SentDate = a.SSD_SentDate,
                                                SSD_Senttime = a.SSD_Senttime,
                                                SSDNO_MobileNo = c.SSDNO_MobileNo,
                                                SSDNO_Status = c.SSDNO_Status,
                                                SSDN_SMSMessage = c.SSDN_SMSMessage,
                                                SSDNO_SMSStatusId = c.SSDNO_SMSStatusId,
                                                SSDNO_DeliveryDate = c.SSDNO_DeliveryDate,
                                                SSDNO_Id = c.SSDNO_Id,
                                                SSDNO_NoOfAttempt = c.SSDNO_NoOfAttempt
                                            }).ToList();
                        if (fillgriddata.Count > 0)
                        {
                            data.fillgriddata = fillgriddata.Distinct().ToArray();
                        }
                    }

                    var fillsentdata = (from a in _db.SMS_Sent_Details
                                        from b in _db.SMS_Sent_Details_LoginUserDMO
                                        from c in _db.SMS_Sent_Details_NowiseDMO
                                        where a.MI_ID == data.MI_ID && a.SSD_Id == b.SSD_Id && b.IVRMUL_Id == data.IVRMUL_Id && a.SSD_HeaderName.Contains(data.SSD_HeaderName) && a.SSD_Id == c.SSD_Id && a.SSD_TransactionId == data.SSD_TransactionId && a.SSD_SentDate >= data.FromDate && a.SSD_SentDate <= data.ToDate
                                        select new SMSResendDTO
                                        {
                                            SSD_TransactionId = a.SSD_TransactionId,
                                            SSD_HeaderName = a.SSD_HeaderName,
                                            SSD_SentDate = a.SSD_SentDate,
                                            SSD_Senttime = a.SSD_Senttime,
                                            SSDNO_MobileNo = c.SSDNO_MobileNo,
                                            SSDNO_Status = c.SSDNO_Status,
                                            SSDN_SMSMessage = c.SSDN_SMSMessage,
                                            SSDNO_SMSStatusId = c.SSDNO_SMSStatusId,
                                            SSDNO_DeliveryDate = c.SSDNO_DeliveryDate,
                                            SSDNO_Id = c.SSDNO_Id,
                                            SSDNO_NoOfAttempt = c.SSDNO_NoOfAttempt
                                        }).ToList();
                    if (fillsentdata.Count > 0)
                    {
                        data.fillsentdata = fillsentdata.Distinct().ToArray();
                    }
                }
            }
            catch (Exception status)
            {
                throw status;
            }
            return data;
        }
        public async Task<SMSResendDTO> resendMsg(SMSResendDTO data)//int IVRMM_Id
        {
            try
            {
                SMS sms1 = new SMS(_db);
                if (data.resenddata.Length > 0)
                {
                    var msgid = string.Empty;
                    var mainstatus = string.Empty;
                    var status = string.Empty;
                    DateTime? delitime = null;
                    string msg = string.Empty;
                    var message = string.Empty;
                    //  var query = string.Empty;
                    var workingkey = string.Empty;

                    List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                    alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_ID)).ToList();

                    if (alldetails.Count > 0)
                    {
                        string url = alldetails[0].IVRMSD_URL.ToString();

                        var query = HttpUtility.ParseQueryString(url);

                        workingkey = query.Get("workingkey");
                        for (int i = 0; i < data.resenddata.Length; i++)
                        {
                            if (data.resenddata[i].SSDNO_SMSStatusId != "" && data.resenddata[i].SSDNO_SMSStatusId != null)
                            {
                                var url1 = "https://api-alerts.solutionsinfini.com/v4/?api_key=" + workingkey + "&method=sms.status&id=" + data.resenddata[i].SSDNO_SMSStatusId + "";
                                System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                                System.Net.HttpWebResponse response1 = request1.GetResponse() as System.Net.HttpWebResponse;
                                Stream stream1 = response1.GetResponseStream();

                                StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                                string responseparameters1 = readStream1.ReadToEnd();
                                var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                                dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                                var statusdetails = JsonConvert.DeserializeObject<CommonLibrary.stausdata>(responsedata1);

                                if (statusdetails.data.Length > 0)
                                {
                                    status = statusdetails.data[0].status;
                                    if (statusdetails.data[0].dlrtime != null)
                                    {
                                        delitime = Convert.ToDateTime(statusdetails.data[0].dlrtime);
                                    }
                                    mainstatus = statusdetails.status;
                                    message = statusdetails.message;
                                }
                                else
                                {
                                    status = message = statusdetails.message;
                                }

                                //var updatestatus = _db.SMS_Sent_Details_NowiseDMO.Single(t => t.SSDNO_Id == data.resenddata[i].SSDNO_Id && t.SSDNO_SMSStatusId.Contains(data.resenddata[i].SSDNO_SMSStatusId));
                                //updatestatus.

                                if (status == "DELIVRD")
                                {
                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "IVRM_SMS_Outgoing_Table_Update";
                                        cmd.CommandType = CommandType.StoredProcedure;

                                        cmd.Parameters.Add(new SqlParameter("@SSDNO_Id",SqlDbType.BigInt)
                                        {
                                            Value = data.resenddata[i].SSDNO_Id
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@MessStatusId",SqlDbType.NVarChar)
                                        {
                                            Value = data.resenddata[i].SSDNO_SMSStatusId
                                        });

                                        if (delitime == null)
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@deliverytime",SqlDbType.DateTime)
                                            {
                                                Value = DBNull.Value
                                            });
                                        }
                                        else
                                        {
                                            cmd.Parameters.Add(new SqlParameter("@deliverytime",SqlDbType.DateTime)
                                            {
                                                Value = delitime
                                            });
                                        }

                                        cmd.Parameters.Add(new SqlParameter("@Status",SqlDbType.NVarChar)
                                        {
                                            Value = status
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
                                            data.retmsg = "error";
                                            return data;
                                        }
                                    }
                                }
                                else
                                {
                                    string d = await sms1.ResendsendSmsnewwithouttemplete_newtable(data.MI_ID, data.resenddata[i].SSDNO_MobileNo, data.resenddata[i].SSDN_SMSMessage, 0, data.IVRMUL_Id, data.resenddata[i].SSDNO_Id);
                                    if (d == "Success")
                                    {
                                        data.retmsg = "Success";
                                    }
                                    else
                                    {
                                        data.retmsg = "error";
                                        return data;
                                    }
                                }
                            }
                            else
                            {
                                string d = await sms1.ResendsendSmsnewwithouttemplete_newtable(data.MI_ID, data.resenddata[i].SSDNO_MobileNo, data.resenddata[i].SSDN_SMSMessage, 0, data.IVRMUL_Id, data.resenddata[i].SSDNO_Id);
                                if (d == "Success")
                                {
                                    data.retmsg = "Success";
                                }
                                else
                                {
                                    data.retmsg = "error";
                                    return data;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.retmsg = "error";
                throw ex;
            }
            if (data.retmsg == "error")
            {
                return data;
            }
            return data;
        }
        public SMSResendDTO GetSelectedRowDetails(int ID)
        {
            SMSResendDTO SMSResendDTO = new SMSResendDTO();
            return SMSResendDTO;
        }
    }
}