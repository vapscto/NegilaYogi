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
using DataAccessMsSqlServerProvider.com.vapstech.Exam;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.TT;
using CommonLibrary;
using SendGrid.Helpers.Mail;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using SendGrid;
using System.Net;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Web;
using System.Dynamic;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SMSApprovalTransactionImpl : Interfaces.SMSApprovalTransactionInterface
    {
        private readonly AdmissionFormContext _SMSAPLContext;

        public DomainModelMsSqlServerContext _db;
        public SMSApprovalTransactionImpl(AdmissionFormContext cpContext, DomainModelMsSqlServerContext db)
        {
            _SMSAPLContext = cpContext;
            _db = db;
        }
        public SMSMasterApprovalDTO Getdetails(SMSMasterApprovalDTO data)//int IVRMM_Id
        {
            try
            {
                var templatelist = _SMSAPLContext.SMSEmailSetting.Where(t => t.MI_Id == data.MI_Id).Distinct().ToList();
                data.headernamelist = templatelist.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public SMSMasterApprovalDTO deactivate(SMSMasterApprovalDTO data)
        {
            try
            {
                if (data.SMA_Id > 0)
                {
                    var result = _SMSAPLContext.SMSMasterApprovalDMO.Single(t => t.SMA_Id == data.SMA_Id);
                    if (result.SMA_ActiveFlag == true)
                    {
                        result.SMA_ActiveFlag = false;
                        _SMSAPLContext.Update(result);
                    }
                    else
                    {
                        result.SMA_ActiveFlag = true;
                        _SMSAPLContext.Update(result);
                    }
                    var flag = _SMSAPLContext.SaveChanges();
                    if (flag == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public SMSMasterApprovalDTO editdata(SMSMasterApprovalDTO data)//int IVRMM_Id
        {
            try
            {
                if (data.SMA_Id > 0)
                {
                    data.editdata = _SMSAPLContext.SMSMasterApprovalDMO.Where(t => t.MI_Id == data.MI_Id && t.SMA_Id == data.SMA_Id).ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public SMSMasterApprovalDTO GetAttendence(SMSMasterApprovalDTO data)//int IVRMM_Id
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public SMSMasterApprovalDTO savedetails(SMSMasterApprovalDTO data)
        {
            try
            {
                var aprovallevallist = _SMSAPLContext.SMSMasterApprovalDMO.Where(e => e.MI_Id == data.MI_Id && e.SMA_HeaderName.ToLower() == data.headername.ToLower() && e.IVRMUL_Id == data.user_id).ToList();
                if (aprovallevallist.Count > 0)
                {
                    var level = aprovallevallist[0].SMA_Level;

                    var checklevel = _SMSAPLContext.SMSMasterApprovalDMO.Where(e => e.MI_Id == data.MI_Id && e.SMA_HeaderName.ToLower() == data.headername.ToLower() && e.SMA_Level < level).ToList();

                    if (checklevel.Count == 0)
                    {
                        data.mainlist = (from a in _SMSAPLContext.SMS_Sent_Details
                                         from b in _SMSAPLContext.SMS_Sent_Details_NowiseDMO
                                         from c in _SMSAPLContext.SMSApprovalStatusDMO
                                         where a.MI_ID == data.MI_Id && a.SSD_Id == b.SSD_Id && a.SSD_HeaderName.Trim().ToLower() == data.headername.Trim().ToLower() && a.SSD_TransactionId == c.STAD_TransNo && c.STAD_ApprStatus == "PENDING" && c.IRMUL_Id == data.user_id
                                         select new SMSMasterApprovalDTO
                                         {
                                             SSD_Id = a.SSD_Id,

                                             headername = a.SSD_HeaderName,
                                             SSD_TransactionId = a.SSD_TransactionId,
                                             SSD_SentDate = a.SSD_SentDate
                                         }).Distinct().ToArray();

                        data.smsdetailslist = (from a in _SMSAPLContext.SMS_Sent_Details
                                               from b in _SMSAPLContext.SMS_Sent_Details_NowiseDMO
                                               from c in _SMSAPLContext.SMSApprovalStatusDMO
                                               where a.MI_ID == data.MI_Id && a.SSD_Id == b.SSD_Id && a.SSD_HeaderName.Trim().ToLower() == data.headername.Trim().ToLower() && a.SSD_TransactionId == c.STAD_TransNo && c.STAD_ApprStatus == "PENDING" && c.IRMUL_Id == data.user_id
                                               select new SMSMasterApprovalDTO
                                               {
                                                   SSD_Id = a.SSD_Id,
                                                   SSDNO_Id = b.SSDNO_Id,
                                                   SMA_Id = c.SMA_Id,
                                                   headername = a.SSD_HeaderName,
                                                   SSD_TransactionId = a.SSD_TransactionId,
                                                   SSDNO_MobileNo = b.SSDNO_MobileNo,
                                                   SSDN_SMSMessage = b.SSDN_SMSMessage,
                                                   STAD_ApprStatus = c.STAD_ApprStatus,
                                                   SSD_SentDate = a.SSD_SentDate
                                               }).Distinct().ToArray();

                    }
                    else
                    {
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GET_SMS_APPROVAL_NEXTLEVEL_DETAILS";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@SSD_HeaderName",
                              SqlDbType.VarChar)
                            {
                                Value = data.headername.Trim()
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                              SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@USER_Id",
                              SqlDbType.BigInt)
                            {
                                Value = data.user_id
                            });
                            cmd.Parameters.Add(new SqlParameter("@SMA_Level",
                            SqlDbType.Int)
                            {
                                Value = level
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
                                data.smsdetailslist = retObject.ToArray();
                            }

                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }


                        }
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GET_SMS_APPROVAL_NEXTLEVEL";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@SSD_HeaderName",
                              SqlDbType.VarChar)
                            {
                                Value = data.headername.Trim()
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                              SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@USER_Id",
                              SqlDbType.BigInt)
                            {
                                Value = data.user_id
                            });
                            cmd.Parameters.Add(new SqlParameter("@SMA_Level",
                            SqlDbType.Int)
                            {
                                Value = level
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
                                data.mainlist = retObject.ToArray();
                            }

                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
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
        public SMSMasterApprovalDTO saveapprove(SMSMasterApprovalDTO data)
        {
            try
            {
                SMS sms1 = new SMS(_db);

                if (data.listdata.Length > 0)
                {
                    foreach (var item in data.listdata)
                    {

                        var levelid = _SMSAPLContext.SMSMasterApprovalDMO.Single(e => e.MI_Id == data.MI_Id && e.SMA_HeaderName.ToLower() == item.headername.ToLower() && e.IVRMUL_Id == data.user_id && e.SMA_SMSMailCallFlag == "SMS").SMA_Level;

                        var aprlevlist = _SMSAPLContext.SMSMasterApprovalDMO.Where(e => e.MI_Id == data.MI_Id && e.SMA_HeaderName.ToLower() == item.headername.ToLower() && e.SMA_Level > levelid && e.SMA_SMSMailCallFlag == "SMS").ToList();

                        if (aprlevlist.Count > 0)
                        {
                            var aprres = _SMSAPLContext.SMSApprovalStatusDMO.Single(s => s.STAD_TransNo == item.SSD_TransactionId && s.IRMUL_Id == data.user_id && s.MI_Id == data.MI_Id);

                            aprres.STAD_ApprStatus = "APPROVED";
                            aprres.UpdatedDate = DateTime.Now;
                            _SMSAPLContext.Update(aprres);

                            int a = _SMSAPLContext.SaveChanges();

                            if (a > 0)
                            {
                                string y = sms1.saveapproval_nextlevel(data.MI_Id, item.headername.ToLower(), data.user_id, item.SSD_TransactionId,
                                    data.otpapproveflag, true, levelid).Result;
                                data.retmsg = "APR";
                            }
                        }
                        else
                        {
                            var aprres = _SMSAPLContext.SMSApprovalStatusDMO.Single(s => s.STAD_TransNo == item.SSD_TransactionId && s.IRMUL_Id == data.user_id && s.MI_Id == data.MI_Id);

                            aprres.STAD_ApprStatus = "APPROVED";
                            aprres.UpdatedDate = DateTime.Now;
                            _SMSAPLContext.Update(aprres);

                            int v = _SMSAPLContext.SaveChanges();

                            if (v > 0)
                            {

                                data.retmsg = "APR";
                                var smsdetailslist = (from a in _SMSAPLContext.SMS_Sent_Details
                                                      from b in _SMSAPLContext.SMS_Sent_Details_NowiseDMO

                                                      where a.MI_ID == data.MI_Id && a.SSD_Id == b.SSD_Id && a.SSD_HeaderName.Trim().ToLower() == item.headername.Trim().ToLower() && a.SSD_TransactionId == item.SSD_TransactionId && a.SSD_Id == item.SSD_Id
                                                      select new SMSMasterApprovalDTO
                                                      {
                                                          SSD_Id = a.SSD_Id,
                                                          SSDNO_Id = b.SSDNO_Id,
                                                          headername = a.SSD_HeaderName,
                                                          SSD_TransactionId = a.SSD_TransactionId,
                                                          SSDNO_MobileNo = b.SSDNO_MobileNo,
                                                          SSDN_SMSMessage = b.SSDN_SMSMessage,

                                                          SSD_SentDate = a.SSD_SentDate
                                                      }
                                           ).Distinct().ToList();


                                if (smsdetailslist.Count > 0)
                                {
                                    foreach (var dd in smsdetailslist)
                                    {
                                        string s = sms1.sendSmsapproval_update_newtable(data.MI_Id, Convert.ToInt64(dd.SSDNO_MobileNo), dd.headername, data.user_id, dd.SSDN_SMSMessage, dd.SSD_TransactionId, "test", data.user_id, dd.SSDNO_Id, dd.SSD_Id).Result;
                                        if (s.Equals("Success"))
                                        {
                                            data.smsStatus = "sent";
                                        }
                                        else
                                        {
                                            data.smsStatus = "failed";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public SMSMasterApprovalDTO rejectsms(SMSMasterApprovalDTO data)
        {
            try
            {
                SMS sms1 = new SMS(_db);

                if (data.listdata.Length > 0)
                {
                    foreach (var item in data.listdata)
                    {

                        var aprres = _SMSAPLContext.SMSApprovalStatusDMO.Single(s => s.STAD_TransNo == item.SSD_TransactionId && s.IRMUL_Id == data.user_id && s.MI_Id == data.MI_Id);

                        aprres.STAD_ApprStatus = "REJECTED";
                        aprres.UpdatedDate = DateTime.Now;
                        _SMSAPLContext.Update(aprres);

                        int a = _SMSAPLContext.SaveChanges();

                        if (a > 0)
                        {

                            var rejlist = (from x in _SMSAPLContext.SMS_Sent_Details
                                           from y in _SMSAPLContext.SMS_Sent_Details_NowiseDMO
                                           where x.SSD_Id == y.SSD_Id && x.MI_ID == data.MI_Id && x.SSD_TransactionId == item.SSD_TransactionId && x.SSD_HeaderName.Trim().ToLower() == item.headername.Trim().ToLower()
                                           select new SMSMasterApprovalDTO
                                           {
                                               SSD_Id = x.SSD_Id,
                                               SSDNO_Id = y.SSDNO_Id,

                                               headername = x.SSD_HeaderName,
                                               SSD_TransactionId = x.SSD_TransactionId,
                                               SSDNO_MobileNo = y.SSDNO_MobileNo,
                                               SSDN_SMSMessage = y.SSDN_SMSMessage,
                                               SSD_SentDate = x.SSD_SentDate

                                           }).ToList();

                            if (rejlist.Count > 0)
                            {
                                foreach (var rj in rejlist)
                                {
                                    string y = sms1.sendSmsnewTemplet_new(data.MI_Id, rj.SSDNO_MobileNo, "RJAPRLSMS", data.user_id, rj.SSD_TransactionId, "test", data.user_id).Result;
                                }
                            }

                            //     string y = sms1.saveapproval_nextlevel(data.MI_Id, item.headername.ToLower(), data.user_id, item.SSD_TransactionId, data.otpapproveflag, true, levelid).Result;
                            //data.retmsg = "APR";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
    }
}