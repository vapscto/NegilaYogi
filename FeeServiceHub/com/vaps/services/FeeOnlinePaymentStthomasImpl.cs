using System;
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using CommonLibrary;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Newtonsoft.Json;
using DomainModel.Model.com.vapstech.Fee;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using paytm.security;
using paytm.util;
using paytm.exception;
using System.Security.Cryptography;
using System.Net.Http;
using System.Xml.Linq;
using Razorpay.Api;
using Payment = CommonLibrary.Payment;
using easebuzz_.net;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeOnlinePaymentStthomasImpl : interfaces.FeeOnlinePaymentStthomasInterface
    {
        private static ConcurrentDictionary<string, FeeStudentTransactionDTO> _login =
       new ConcurrentDictionary<string, FeeStudentTransactionDTO>();

        public DomainModelMsSqlServerContext _context;
        public FeeGroupContext _FeeGroupHeadContext;
        readonly ILogger<FeeOnlinePaymentStthomasImpl> _logger;

        public FeeOnlinePaymentStthomasImpl(FeeGroupContext frgContext, ILogger<FeeOnlinePaymentStthomasImpl> log, DomainModelMsSqlServerContext context)
        {
            _logger = log;
            _FeeGroupHeadContext = frgContext;
            _context = context;
        }

        public FeeStudentTransactionDTO getamountdet(FeeStudentTransactionDTO data)
        {
            try
            {
                List<long> trm_ids = new List<long>();
                List<FeeStudentTransactionDTO> list3 = new List<FeeStudentTransactionDTO>();
                string termlst = "0";
                long fmttotal = 0;
                data.ASMAY_Id = getacademicyearcongig(data);

                long clsid = getcurrentclass(data);

                data.ASMCL_ID = clsid;

                var feemasnum = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                data.studwisepartialpayment = _FeeGroupHeadContext.FeeStudentEnablePartialPaymentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.Amst_Id && t.ASMAY_Id == data.ASMAY_Id).ToArray();


                var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                from b in _FeeGroupHeadContext.AdmissionStudentDMO
                                where (a.AMST_Id == b.AMST_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                select new FeeStudentTransactionDTO
                                {
                                    AMST_ECSFlag = b.AMST_ECSFlag
                                }
).Distinct().ToArray();

                int ecsflag = 0;
                for (int s = 0; s < fetchecs.Count(); s++)
                {
                    if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                    {
                        ecsflag = 0;
                    }
                    else
                    {
                        ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                    }
                }

                long fineamt = 0, totalfine = 0, amountids = 0;
                List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> finelstfinal = new List<FeeStudentTransactionDTO>();

                data.fillstudent = (from a in _FeeGroupHeadContext.AdmissionStudentDMO
                                    from b in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                    from c in _FeeGroupHeadContext.admissioncls
                                    from d in _FeeGroupHeadContext.school_M_Section
                                    where (c.ASMCL_Id == b.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && b.ASMAY_Id == data.ASMAY_Id)
                                    select new FeeStudentTransactionDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        classname = c.ASMCL_ClassName,
                                        sectionname = d.ASMC_SectionName,
                                        ASMCL_ID = b.ASMCL_Id,
                                        amst_mobile = a.AMST_MobileNo,
                                        amst_email_id = a.AMST_emailId
                                    }
               ).ToArray();


                //online payment
                data.filonlinepaymentgrid = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             where (a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_NetAmount = a.FSS_NetAmount,
                                                 FSS_ConcessionAmount = a.FSS_ConcessionAmount,
                                                 FSS_FineAmount = a.FSS_FineAmount,
                                                 FSS_ToBePaid = a.FSS_ToBePaid,
                                                 FSS_PaidAmount = a.FSS_PaidAmount,
                                                 FSS_TotalToBePaid = a.FSS_TotalToBePaid,
                                                 FSS_OBArrearAmount = a.FSS_OBArrearAmount
                                             }
                ).OrderBy(t => t.FMH_Id).ToArray();

                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                for (int l = 0; l < data.selected_list.Length; l++)
                {
                    trm_ids.Clear();
                    foreach (var x in data.selected_list[l].trm_list)
                    {
                        trm_ids.Add(x.FMT_Id);
                        termlst = termlst + "," + x.FMT_Id;
                    }

                    var grp_ids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == data.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id);
                    fineamt = 0;

                    List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                    List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();

                    var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                     where (a.MI_Id == data.MI_Id)
                                     select new FeeStudentTransactionDTO
                                     {
                                         IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                     }
                          ).FirstOrDefault();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         where (b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName
                                         }
              ).Distinct().ToList();
                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         where (a.ASMCL_Id == data.ASMCL_ID && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && trm_ids.Contains(a.fmt_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName
                                         }
              ).Distinct().ToList();

                    }

                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }

                    List<FeeStudentTransactionDTO> list = new List<FeeStudentTransactionDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                from c in _FeeGroupHeadContext.FeeHeadDMO
                                from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                from e in _FeeGroupHeadContext.FeeGroupDMO
                                    // from e in _FeeGroupHeadContext.feeGGG
                                where (b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && b.FMG_Id == e.FMG_Id && a.fmg_id == e.FMG_Id)
                                select new FeeStudentTransactionDTO
                                {
                                    FMA_Id = b.FMA_Id,
                                    FSS_ToBePaid = b.FSS_ToBePaid,
                                    FSS_FineAmount = b.FSS_FineAmount,
                                    FSS_ConcessionAmount = b.FSS_ConcessionAmount,
                                    FSS_WaivedAmount = b.FSS_WaivedAmount,
                                    FMG_Id = b.FMG_Id,
                                    FMH_Id = b.FMH_Id,
                                    FTI_Id = b.FTI_Id,
                                    FSS_PaidAmount = b.FSS_PaidAmount,
                                    FSS_NetAmount = b.FSS_NetAmount,
                                    FSS_RefundAmount = b.FSS_RebateAmount,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FSS_CurrentYrCharges = b.FSS_CurrentYrCharges,
                                    FSS_TotalToBePaid = b.FSS_TotalToBePaid,
                                    FMH_Order = c.FMH_Order,
                                    headflag = c.FMH_Flag,
                                    FMG_GroupName = e.FMG_GroupName,
                                    FSS_OBArrearAmount = b.FSS_OBArrearAmount,
                                    FMT_Id = a.fmt_id,
                                    FMGG_Id = data.selected_list[l].grp.FMGG_Id,
                                    FMH_Flag = c.FMH_Flag

                                }
              ).Distinct().OrderBy(t => t.FTI_Id).ToList();

                        list3 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                 from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                 from c in _FeeGroupHeadContext.FeeHeadDMO
                                 from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                 from e in _FeeGroupHeadContext.FeeGroupDMO
                                 from f in _FeeGroupHeadContext.Yearlygroups
                                     // from e in _FeeGroupHeadContext.feeGGG
                                 where (b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && b.FMG_Id == e.FMG_Id && a.fmg_id == e.FMG_Id && e.FMG_Id == f.FMG_Id && f.ASMAY_Id == data.ASMAY_Id && f.FYG_RebateApplicableFlg == true)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMA_Id = b.FMA_Id,
                                     FSS_ToBePaid = b.FSS_ToBePaid,
                                     FSS_FineAmount = b.FSS_FineAmount,
                                     FSS_ConcessionAmount = b.FSS_ConcessionAmount,
                                     FSS_WaivedAmount = b.FSS_WaivedAmount,
                                     FMG_Id = b.FMG_Id,
                                     FMH_Id = b.FMH_Id,
                                     FTI_Id = b.FTI_Id,
                                     FSS_PaidAmount = b.FSS_PaidAmount,
                                     FSS_NetAmount = b.FSS_NetAmount,
                                     FSS_RefundAmount = b.FSS_RebateAmount,
                                     FMH_FeeName = c.FMH_FeeName,
                                     FTI_Name = d.FTI_Name,
                                     FSS_CurrentYrCharges = b.FSS_CurrentYrCharges,
                                     FSS_TotalToBePaid = b.FSS_TotalToBePaid,
                                     FMH_Order = c.FMH_Order,
                                     headflag = c.FMH_Flag,
                                     FMG_GroupName = e.FMG_GroupName,
                                     FSS_OBArrearAmount = b.FSS_OBArrearAmount,
                                     FMT_Id = a.fmt_id,
                                     FMGG_Id = data.selected_list[l].grp.FMGG_Id,
                                     FMH_Flag = c.FMH_Flag
                                 }
).Distinct().OrderBy(t => t.FTI_Id).ToList();

                    }
                    else
                    {
                        list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                from c in _FeeGroupHeadContext.FeeHeadDMO
                                from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                from e in _FeeGroupHeadContext.FeeGroupDMO
                                    // from e in _FeeGroupHeadContext.feeGGG
                                where (b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && b.FMG_Id == e.FMG_Id && a.ASMCL_Id == data.ASMCL_ID)
                                select new FeeStudentTransactionDTO
                                {
                                    FMA_Id = b.FMA_Id,
                                    FSS_ToBePaid = b.FSS_ToBePaid,
                                    FSS_FineAmount = b.FSS_FineAmount,
                                    FSS_ConcessionAmount = b.FSS_ConcessionAmount,
                                    FSS_WaivedAmount = b.FSS_WaivedAmount,
                                    FMG_Id = b.FMG_Id,
                                    FMH_Id = b.FMH_Id,
                                    FTI_Id = b.FTI_Id,
                                    FSS_PaidAmount = b.FSS_PaidAmount,
                                    FSS_NetAmount = b.FSS_NetAmount,
                                    FSS_RefundAmount = b.FSS_RebateAmount,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FSS_CurrentYrCharges = b.FSS_CurrentYrCharges,
                                    FSS_TotalToBePaid = b.FSS_TotalToBePaid,
                                    FMH_Order = c.FMH_Order,
                                    headflag = c.FMH_Flag,
                                    FMG_GroupName = e.FMG_GroupName,
                                    FSS_OBArrearAmount = b.FSS_OBArrearAmount,
                                    FMT_Id = a.fmt_id,
                                    FMGG_Id = data.selected_list[l].grp.FMGG_Id,
                                    FMH_Flag = c.FMH_Flag
                                }
             ).Distinct().OrderBy(t => t.FTI_Id).ToList();

                        list3 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                 from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                 from c in _FeeGroupHeadContext.FeeHeadDMO
                                 from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                 from e in _FeeGroupHeadContext.FeeGroupDMO
                                 from f in _FeeGroupHeadContext.Yearlygroups
                                     // from e in _FeeGroupHeadContext.feeGGG
                                 where (b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && b.FMG_Id == e.FMG_Id && a.ASMCL_Id == data.ASMCL_ID && e.FMG_Id == f.FMG_Id && f.ASMAY_Id == data.ASMAY_Id && f.FYG_RebateApplicableFlg == true)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMA_Id = b.FMA_Id,
                                     FSS_ToBePaid = b.FSS_ToBePaid,
                                     FSS_FineAmount = b.FSS_FineAmount,
                                     FSS_ConcessionAmount = b.FSS_ConcessionAmount,
                                     FSS_WaivedAmount = b.FSS_WaivedAmount,
                                     FMG_Id = b.FMG_Id,
                                     FMH_Id = b.FMH_Id,
                                     FTI_Id = b.FTI_Id,
                                     FSS_PaidAmount = b.FSS_PaidAmount,
                                     FSS_NetAmount = b.FSS_NetAmount,
                                     FSS_RefundAmount = b.FSS_RebateAmount,
                                     FMH_FeeName = c.FMH_FeeName,
                                     FTI_Name = d.FTI_Name,
                                     FSS_CurrentYrCharges = b.FSS_CurrentYrCharges,
                                     FSS_TotalToBePaid = b.FSS_TotalToBePaid,
                                     FMH_Order = c.FMH_Order,
                                     headflag = c.FMH_Flag,
                                     FMG_GroupName = e.FMG_GroupName,
                                     FSS_OBArrearAmount = b.FSS_OBArrearAmount,
                                     FMT_Id = a.fmt_id,
                                     FMGG_Id = data.selected_list[l].grp.FMGG_Id,
                                     FMH_Flag = c.FMH_Flag
                                 }
        ).Distinct().OrderBy(t => t.FTI_Id).ToList();
                    }

                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                    foreach (var x in list3)
                    {
                        fmttotal = fmttotal + x.FSS_CurrentYrCharges;

                    }

                    //DateTime currdt = DateTime.Now;

                    try
                    {
                        using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd1.CommandText = "Readmissionfeeschecking";
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });

                            cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                             SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });


                            cmd1.Parameters.Add(new SqlParameter("@AMST_Id",
                            SqlDbType.VarChar)
                            {
                                Value = data.Amst_Id
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
                                data.Readmissionfeeschecking = retObject1.ToArray();
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
                    if (data.Readmissionfeeschecking.Length > 0) {
                        var readmssionflg= (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                            from b in _FeeGroupHeadContext.FeeHeadDMO
                                            
                                            where (a.AMST_Id == data.Amst_Id 
                                            && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMH_Id==b.FMH_Id && b.FMH_Flag=="RA" && a.FSS_ToBePaid>0)
                                            select a.AMST_Id
).Distinct().ToList();

                       
                            foreach (var x in list)
                            {
                            amount_list.Add(x);
                            if ((data.Readmissionfeeschecking == null || data.Readmissionfeeschecking.Length == 0) || (readmssionflg.Count == 0))
                            {
                                

                                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                {
                                    if (ecsflag.Equals(0))
                                    {
                                        cmd.CommandText = "Sp_Calculate_Fine";
                                    }
                                    else
                                    {
                                        cmd.CommandText = "Sp_Calculate_Fine_ECS";
                                    }

                                    //cmd.CommandText = "Sp_Calculate_Fine";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@On_Date",
                                        SqlDbType.DateTime, 100)
                                    {
                                        //Value = currdt
                                        Value = indianTime
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@fma_id",
                                       SqlDbType.NVarChar, 100)
                                    {
                                        Value = x.FMA_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                                   SqlDbType.NVarChar, 100)
                                    {
                                        Value = data.ASMAY_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@amt",
                        SqlDbType.Decimal, 500)
                                    {
                                        Direction = ParameterDirection.Output
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@flgArr",
                       SqlDbType.Int, 500)
                                    {
                                        Direction = ParameterDirection.Output
                                    });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var data1 = cmd.ExecuteNonQuery();

                                    var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                         where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == data.Amst_Id)
                                                         select new FeeStudentTransactionDTO
                                                         {
                                                             FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                             FSWO_FineFlg = a.FSWO_FineFlg,
                                                             FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                         }
                    ).Distinct().ToList();

                                    if (waivedofffine.Count() > 0)
                                    {
                                        if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                        {
                                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                                totalfine = totalfine + fineamt;
                                            }
                                        }
                                        else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                        {
                                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamt = 0;
                                                totalfine = totalfine + fineamt;
                                            }
                                        }

                                        else
                                        {
                                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                                totalfine = totalfine + fineamt;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                        {
                                            fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                            totalfine = totalfine + fineamt;
                                        }
                                    }

                                    if (Convert.ToInt32(fineamt) > 0)
                                    {
                                        if (fineheadfmaid.Count() > 0)
                                        {
                                            finelst.Add(new FeeStudentTransactionDTO
                                            {
                                                FMA_Id = x.FMA_Id,
                                                FSS_ToBePaid = Convert.ToInt32(fineamt),
                                                FSS_FineAmount = 0,
                                                FSS_ConcessionAmount = 0,
                                                FSS_WaivedAmount = 0,
                                                FMG_Id = 0,
                                                FMH_Id = 0,
                                                FTI_Id = 0,
                                                FSS_PaidAmount = 0,
                                                FSS_NetAmount = 0,
                                                FSS_RefundAmount = 0,
                                                FMH_FeeName = fineheadfmaid[0].FMH_FeeName,
                                                FTI_Name = "Anytime",
                                                FSS_CurrentYrCharges = 0,
                                                FSS_TotalToBePaid = 0,
                                            });
                                        }
                                    }
                                    fineamt = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var x in list)
                        {
                            amount_list.Add(x);
                          

                                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                {
                                    if (ecsflag.Equals(0))
                                    {
                                        cmd.CommandText = "Sp_Calculate_Fine";
                                    }
                                    else
                                    {
                                        cmd.CommandText = "Sp_Calculate_Fine_ECS";
                                    }

                                    //cmd.CommandText = "Sp_Calculate_Fine";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@On_Date",
                                        SqlDbType.DateTime, 100)
                                    {
                                        //Value = currdt
                                        Value = indianTime
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@fma_id",
                                       SqlDbType.NVarChar, 100)
                                    {
                                        Value = x.FMA_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                                   SqlDbType.NVarChar, 100)
                                    {
                                        Value = data.ASMAY_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@amt",
                        SqlDbType.Decimal, 500)
                                    {
                                        Direction = ParameterDirection.Output
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@flgArr",
                       SqlDbType.Int, 500)
                                    {
                                        Direction = ParameterDirection.Output
                                    });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var data1 = cmd.ExecuteNonQuery();

                                    var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                         where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == data.Amst_Id)
                                                         select new FeeStudentTransactionDTO
                                                         {
                                                             FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                             FSWO_FineFlg = a.FSWO_FineFlg,
                                                             FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                         }
                    ).Distinct().ToList();

                                    if (waivedofffine.Count() > 0)
                                    {
                                        if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                        {
                                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                                totalfine = totalfine + fineamt;
                                            }
                                        }
                                        else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                        {
                                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamt = 0;
                                                totalfine = totalfine + fineamt;
                                            }
                                        }

                                        else
                                        {
                                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                                totalfine = totalfine + fineamt;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                        {
                                            fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                            totalfine = totalfine + fineamt;
                                        }
                                    }

                                    if (Convert.ToInt32(fineamt) > 0)
                                    {
                                        if (fineheadfmaid.Count() > 0)
                                        {
                                            finelst.Add(new FeeStudentTransactionDTO
                                            {
                                                FMA_Id = x.FMA_Id,
                                                FSS_ToBePaid = Convert.ToInt32(fineamt),
                                                FSS_FineAmount = 0,
                                                FSS_ConcessionAmount = 0,
                                                FSS_WaivedAmount = 0,
                                                FMG_Id = 0,
                                                FMH_Id = 0,
                                                FTI_Id = 0,
                                                FSS_PaidAmount = 0,
                                                FSS_NetAmount = 0,
                                                FSS_RefundAmount = 0,
                                                FMH_FeeName = fineheadfmaid[0].FMH_FeeName,
                                                FTI_Name = "Anytime",
                                                FSS_CurrentYrCharges = 0,
                                                FSS_TotalToBePaid = 0,
                                            });
                                        }
                                    }
                                    fineamt = 0;
                                }
                        }
                     }
              
                    var Euserid = (from a in _FeeGroupHeadContext.FeeGroupDMO
                                   where (a.MI_Id == Convert.ToInt64(data.MI_Id) && grp_ids.Contains(a.FMG_Id))
                                   select new FeeStudentTransactionDTO
                                   {
                                       enduserid = a.user_id,
                                   }
              ).Distinct().Take(1).ToArray();
                    //added on 02-07-2018

                    data.userid = Convert.ToInt64(Euserid[0].enduserid);

                }


                data.fillstudentviewdetails = amount_list.ToArray();
                data.finearray = finelst.ToArray();

                string ids = data.fetchtermlst;

                var saved_fma = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                 from b in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                                 from c in _FeeGroupHeadContext.Fee_Y_Payment_School_StudentDMO
                                 where (a.FMA_Id == b.FMA_Id && b.FYP_Id == c.FYP_Id && a.AMST_Id == c.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                 select b.FMA_Id
).Distinct().ToList();

                var fetchclass = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                  where (a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                  select new FeeStudentTransactionDTO
                                  {
                                      ASMCL_ID = a.ASMCL_Id,
                                      ASMAY_Id = a.ASMAY_Id
                                  }
).Distinct().ToArray();

                string classid = "0";
                for (int s = 0; s < fetchclass.Count(); s++)
                {
                    classid = fetchclass[s].ASMCL_ID.ToString();
                }

                data.instalspecial = (from a in _FeeGroupHeadContext.FeeHeadDMO
                                      from d in _FeeGroupHeadContext.feeMIY
                                      from b in _FeeGroupHeadContext.feeMTH
                                      from c in _FeeGroupHeadContext.FeeAmountEntryDMO
                                      from e in _FeeGroupHeadContext.FeeGroupDMO
                                      from f in _FeeGroupHeadContext.feeYCCC
                                      from g in _FeeGroupHeadContext.feeYCC
                                      where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && trm_ids.Contains(b.FMT_Id) && !saved_fma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                      select new Head_Installments_DTO
                                      {
                                          FTI_Name = d.FTI_Name,
                                          FTI_Id = c.FTI_Id
                                      }).Distinct().ToList().ToArray();

                

                if (data.FYP_PayModeType != "MOBILE")
                {
                    data.fillpaymentgateway = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                               from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (a.IMPG_ActiveFlg == true && b.FPGD_PGActiveFlag ==
                                               "1" && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FPGD_Id = a.IMPG_Id,
                                                   FPGD_PGName = a.IMPG_PGFlag,
                                                   FPGD_Image = b.FPGD_Image
                                               }
).Distinct().ToArray();
                }
                else
                {
                    data.fillpaymentgateway = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                               from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (a.IMPG_ActiveFlg == true && b.FPGD_PGActiveFlag ==
                                               "1" && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id && b.FPGD_MobileActiveFlag == true)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FPGD_Id = a.IMPG_Id,
                                                   FPGD_PGName = a.IMPG_PGFlag,
                                                   FPGD_Image = b.FPGD_Image
                                               }
).Distinct().ToArray();
                }


               

                var cnt1 = _context.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id && t.FMC_RebateAplicableFlg == true && t.FMC_RebateAgainstFullPaymentFlg == true && t.FMC_RebateAgainstPartialPaymentFlg == true).ToList();



                string rebateamount;

                if (cnt1.Count > 0)
                {
                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fee_RebateTermWise_calculation_BOTH";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                       SqlDbType.VarChar)
                        {
                            Value = data.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMT_ID",
                      SqlDbType.VarChar)
                        {
                            Value = termlst
                        });
                        cmd.Parameters.Add(new SqlParameter("@paiddate",
                      SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
                        });
                        cmd.Parameters.Add(new SqlParameter("@paidamount",
                       SqlDbType.VarChar)
                        {
                            Value = fmttotal
                        });

                        cmd.Parameters.Add(new SqlParameter("@USERID",
                       SqlDbType.VarChar)
                        {
                            Value = data.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
            SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                    }



                }
                else
                {
                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fee_RebateTermWise_calculation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                       SqlDbType.VarChar)
                        {
                            Value = data.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMT_ID",
                      SqlDbType.VarChar)
                        {
                            Value = termlst
                        });
                        cmd.Parameters.Add(new SqlParameter("@paiddate",
                      SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
                        });
                        cmd.Parameters.Add(new SqlParameter("@paidamount",
                       SqlDbType.VarChar)
                        {
                            Value = fmttotal
                        });

                        cmd.Parameters.Add(new SqlParameter("@USERID",
                       SqlDbType.VarChar)
                        {
                            Value = data.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
            SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                    }

                }

                data.FSS_RebateAmount = Convert.ToInt64(rebateamount);





            }
            catch (Exception e)

            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public Array paymentPart(FeeStudentTransactionDTO enq, long totamount)
        {
            long yrid = getacademicyearcongig(enq);

            enq.ASMAY_Id = yrid;

            long clsid = getcurrentclass(enq);

            var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                            from b in _FeeGroupHeadContext.AdmissionStudentDMO
                            where (a.AMST_Id == b.AMST_Id && a.AMST_Id == enq.Amst_Id && a.ASMAY_Id == enq.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                            select new FeeStudentTransactionDTO
                            {
                                AMST_ECSFlag = b.AMST_ECSFlag
                            }
).Distinct().ToArray();

            int ecsflag = 0;
            for (int s = 0; s < fetchecs.Count(); s++)
            {
                if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                {
                    ecsflag = 0;
                }
                else
                {
                    ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                }
            }


            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();

            long fineamt = 0, totalfine = 0;
            try
            {
                Payment pay = new Payment(_context);
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;

                string termids = "0", grpidss = "0";

                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == enq.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                             ).FirstOrDefault();

                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                for (int l = 0; l < enq.selected_list.Length; l++)
                {
                    totalfine = 0;
                    string fpgdids = "";
                    List<long> trm_ids = new List<long>();
                    foreach (var x in enq.selected_list[l].trm_list)
                    {
                        termids = termids + ',' + x.FMT_Id;
                        trm_ids.Add(x.FMT_Id);
                    }

                    enq.ftiidss = termids;

                    var grp_ids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id);
                    var grp_ids1 = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id).ToArray();
                    fineamt = 0;
                    for (int r = 0; r < grp_ids1.Count(); r++)
                    {
                        grpidss = grpidss + ',' + grp_ids1[r];
                    }

                    enq.grpidss = grpidss;

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();
                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }
                    else
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID && trm_ids.Contains(a.fmt_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();

                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }

                    List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                    List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }


                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }


                    List<FeeStudentTransactionDTO> list123 = new List<FeeStudentTransactionDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list123 = list;
                    }
                    else
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list123 = list;

                    }



                    //DateTime currdt = DateTime.Now;
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    foreach (var x in list123)
                    {
                        amount_list.Add(x);

                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            if (ecsflag.Equals(0))
                            {
                                cmd.CommandText = "Sp_Calculate_Fine";
                            }
                            else
                            {
                                cmd.CommandText = "Sp_Calculate_Fine_ECS";
                            }

                            //cmd.CommandText = "Sp_Calculate_Fine";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime, 100)
                            {
                                //Value = currdt
                                Value = indianTime
                            });

                            cmd.Parameters.Add(new SqlParameter("@fma_id",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = x.FMA_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = enq.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Decimal, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                 where (a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == enq.Amst_Id)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                     FSWO_FineFlg = a.FSWO_FineFlg,
                                                     FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                 }
            ).Distinct().ToList();

                            if (waivedofffine.Count() > 0)
                            {
                                if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = 0;
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                {
                                    fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    totalfine = totalfine + fineamt;
                                }
                            }

                        }

                    }

                    if (Convert.ToInt32(fineamt) > 0)
                    {
                        if (fineheadfmaid.Count() > 0)
                        {
                            finelst.Add(new FeeStudentTransactionDTO
                            {
                                FMA_Id = fineheadfmaid[0].FMA_Id,
                                FSS_ToBePaid = Convert.ToInt32(fineamt),
                                FSS_FineAmount = 0,
                                FSS_ConcessionAmount = 0,
                                FSS_WaivedAmount = 0,
                                FMG_Id = 0,
                                FMH_Id = 0,
                                FTI_Id = 0,
                                FSS_PaidAmount = 0,
                                FSS_NetAmount = 0,
                                FSS_RefundAmount = 0,
                                FMH_FeeName = "Fine",
                                FTI_Name = "Anytime",
                                FSS_CurrentYrCharges = 0,
                                FSS_TotalToBePaid = 0,
                                merchantid = fineheadfmaid[0].merchantid,
                                FYP_Id = 0
                            });
                        }

                    }

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }
                    else
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }


                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (Convert.ToInt32(enq.pendingamount) + fineamt).ToString(),
                        //value = (Convert.ToInt32(enq.pendingamount) + totalfine).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });
                }

                enq.amount_list = amount_list.ToArray();

                if (finelst.Count() > 0)
                {
                    foreach (var y in finelst)
                    {
                        amount_list.Add(y);
                    }
                }


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                // paymentdetails = _FeeGroupHeadContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();
                //multiple gateway
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

                //var receipt = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_Receipt_No);

                //multiple gateway
                //PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                    enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                    PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
                }

                PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
                enq.trans_id = PaymentDetailsDto.trans_id.ToString();

                PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }

                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                //payinfo = "FeeOnlinePayment";

                PaymentDetailsDto.productinfo = payinfo;
                //PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount.ToString()) + Convert.ToInt32(totalfine);
                PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount.ToString());
                // PaymentDetailsDto.amount = Convert.ToDecimal(enq.topayamount);

                PaymentDetailsDto.firstname = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).AMST_FirstName + ' ' + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).AMST_MiddleName + ' ' + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).AMST_LastName.Trim();
                PaymentDetailsDto.email = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim();

                //PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT.Trim();
                //PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL.Trim();
                //multiple gateway
                PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().FPGD_SaltKey.Trim();
                PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().FPGD_URL.Trim();

                PaymentDetailsDto.phone = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile;
                PaymentDetailsDto.udf1 = enq.ASMAY_Id.ToString().Trim();
                PaymentDetailsDto.udf2 = Convert.ToString(enq.Amst_Id);
                PaymentDetailsDto.udf3 = enq.MI_Id.ToString();

                PaymentDetailsDto.udf4 = enq.ftiidss.ToString();
                // PaymentDetailsDto.udf4 = enq.ftiidss.ToString();

                PaymentDetailsDto.udf6 = enq.grpidss.ToString().Trim();

                PaymentDetailsDto.udf5 = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString();
                PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/" + PaymentDetailsDto.productinfo + "/paymentresponse/";
                PaymentDetailsDto.status = "success";
                PaymentDetailsDto.service_provider = "payu_paisa";

                PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

                get_grp_reptno(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }

        public FeeStudentTransactionDTO getdetails(FeeStudentTransactionDTO data)
        {
            try
            {
                string groupudss = "0";
                data.ASMAY_Id = getacademicyearcongig(data);

                data.pagelist = (from a in _context.Institution_Module_Page
                                 from b in _context.Institution_Module
                                 from c in _context.masterPage
                                 where a.IVRMIM_Id == b.IVRMIM_Id && a.IVRMP_Id == c.IVRMP_Id && b.MI_Id == data.MI_Id && a.IVRMIMP_Flag == 1 && c.IVRMP_PageURL.Trim() == "app.FeeOnlinePayment"
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMIMP_DisplayContent = a.IVRMIMP_DisplayContent,
                                 }
                               ).Distinct().ToArray();

                List<FeeStudentTransactionDTO> customgrp = new List<FeeStudentTransactionDTO>();
                customgrp = (from a in _FeeGroupHeadContext.feegm
                             from b in _FeeGroupHeadContext.feeGGG
                             from c in _FeeGroupHeadContext.FeeStudentTransactionDMO
                             where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.FMG_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && c.AMST_Id == data.Amst_Id && c.ASMAY_Id == data.ASMAY_Id)
                             select new FeeStudentTransactionDTO
                             {
                                 FMG_Id = b.FMG_Id
                             }
         ).Distinct().ToList();

                List<long> grp_ids = new List<long>();
                foreach (var item in customgrp)
                {
                    grp_ids.Add(item.FMG_Id);
                    groupudss = groupudss + ',' + item.FMG_Id;
                }

                List<FeeTermDMO> allpages = new List<FeeTermDMO>();
                List<FeeGroupDMO> allgroup = new List<FeeGroupDMO>();
                if (data.configset.Equals("T"))
                {
                    data.fillinstallment = (from a in _FeeGroupHeadContext.feeTr
                                            where (a.MI_Id == data.MI_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                            select new FeeStudentTransactionDTO
                                            {
                                                FMG_GroupName = a.FMT_Name,
                                                FMG_Id = a.FMT_Id,
                                            }
     ).Distinct().ToArray();

                    //disable terms

                    var readterms = (from a in _FeeGroupHeadContext.feeTr
                                     where (a.MI_Id == data.MI_Id)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMT_Name = a.FMT_Name,
                                         FMT_Id = a.FMT_Id,
                                     }
        ).Distinct().ToArray();

                    string alltrids = "0";
                    for (int s = 0; s < readterms.Count(); s++)
                    {
                        alltrids = alltrids + ',' + readterms[s].FMT_Id.ToString();
                    }

                    try
                    {
                        using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd1.CommandText = "gettermsstatisticdetails_online";
                            cmd1.CommandType = CommandType.StoredProcedure;

                            cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                             SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });

                            cmd1.Parameters.Add(new SqlParameter("@Mi_Id",
                            SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });

                            cmd1.Parameters.Add(new SqlParameter("@amst_id",
                            SqlDbType.VarChar)
                            {
                                Value = data.Amst_Id
                            });

                            cmd1.Parameters.Add(new SqlParameter("@fmtids",
                             SqlDbType.VarChar)
                            {
                                Value = alltrids
                            });

                            cmd1.Parameters.Add(new SqlParameter("@fmgid",
                            SqlDbType.VarChar)
                            {
                                Value = groupudss
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
                                data.disableterms = retObject1.ToArray();
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

                    // disable terms

                }
                else if (data.configset.Equals("G"))
                {
                    data.fillinstallment = (from a in _FeeGroupHeadContext.FeeGroupDMO
                                            from b in _FeeGroupHeadContext.FeeStudentGroupMappingDMO
                                            from c in _FeeGroupHeadContext.FEeGroupLoginPreviledgeDMO
                                            where (a.FMG_Id == b.FMG_Id && b.AMST_Id == data.Amst_Id && a.FMG_ActiceFlag == true && c.FMG_ID == b.FMG_Id)
                                            select new FeeStudentTransactionDTO
                                            {
                                                FMG_GroupName = a.FMG_GroupName,
                                                FMG_Id = a.FMG_Id,
                                            }
         ).Distinct().ToArray();
                }

                data.fillstudent = (from a in _FeeGroupHeadContext.AdmissionStudentDMO
                                    from b in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                    from c in _FeeGroupHeadContext.admissioncls
                                    from d in _FeeGroupHeadContext.school_M_Section
                                    where (c.ASMCL_Id == b.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id)
                                    select new FeeStudentTransactionDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        classname = c.ASMCL_ClassName,
                                        sectionname = d.ASMC_SectionName,
                                        amst_mobile = a.AMST_MobileNo,
                                        amst_email_id = a.AMST_emailId,
                                        ASMCL_ID = c.ASMCL_Id,
                                        ASYST_Id = b.ASYST_Id
                                    }
                  ).OrderByDescending(t => t.ASYST_Id).ToArray();

                data.customgrplist = (from a in _FeeGroupHeadContext.feegm
                                      from b in _FeeGroupHeadContext.feeGGG
                                      from c in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                      where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.FMG_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && c.AMST_Id == data.Amst_Id && c.ASMAY_Id == data.ASMAY_Id)
                                      select new tempgroupDTO
                                      {
                                          FMGG_GroupName = a.FMGG_GroupName,
                                          FMGG_Id = a.FMGG_Id
                                      }
          ).Distinct().OrderBy(t => t.FMGG_Id).ToArray();

                data.filonlinepaymentgrid = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             where (a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && grp_ids.Contains(a.FMG_Id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_NetAmount = a.FSS_NetAmount,
                                                 FSS_ConcessionAmount = a.FSS_ConcessionAmount,
                                                 FSS_FineAmount = a.FSS_FineAmount,
                                                 FSS_ToBePaid = a.FSS_ToBePaid,
                                                 FSS_PaidAmount = a.FSS_PaidAmount
                                             }
             ).ToArray();


                data.termlst = (from a in _FeeGroupHeadContext.feegm
                                from b in _FeeGroupHeadContext.feeGGG
                                from c in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                from d in _FeeGroupHeadContext.feeTr
                                from e in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    //where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id && c.fmt_id == d.FMT_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && d.FMT_ActiveFlag == true && grp_ids.Contains(b.FMG_Id))
                                where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id && c.fmt_id == d.FMT_Id && e.FMG_Id == b.FMG_Id && e.FMH_Id == c.FMH_Id && e.FTI_Id == c.fti_id && c.MI_Id == e.MI_Id && e.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && e.AMST_Id == data.Amst_Id && a.FMGG_ActiveFlag == true && d.FMT_ActiveFlag == true && grp_ids.Contains(b.FMG_Id))
                                select new temotermDTO
                                {
                                    FMGG_GroupName = a.FMGG_GroupName,
                                    FMGG_Id = a.FMGG_Id,
                                    FMT_Id = d.FMT_Id,
                                    FMT_Name = d.FMT_Name,
                                    FMT_Order = d.FMT_Order
                                }
     ).Distinct().OrderBy(t => t.FMT_Order).ToArray();


                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _FeeGroupHeadContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Online").ToList();
                data.transnumconfig = masnum.ToArray();

                var acadeorder = _FeeGroupHeadContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id).Select(t => t.ASMAY_Order).ToList();

                List<MasterAcademic> acade = new List<MasterAcademic>();
                acade = _FeeGroupHeadContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Order >= acadeorder.FirstOrDefault()).ToList();
                data.academicyrlist = acade.ToArray();

                List<FeeMasterConfigurationDMO> configg = new List<FeeMasterConfigurationDMO>();
                configg = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.readterms = configg.ToArray();

                if (data.readterms.Length > 0)
                {
                    bool staffcheck = configg[0].FMC_StaffConcessionCheck;
                    if (staffcheck == true)
                    {
                        try
                        {
                            using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd1.CommandText = "staffcategoryblocking";
                                cmd1.CommandType = CommandType.StoredProcedure;

                                cmd1.Parameters.Add(new SqlParameter("@mi_id",
                                 SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });

                                cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                                SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });

                                cmd1.Parameters.Add(new SqlParameter("@amst_id",
                                SqlDbType.VarChar)
                                {
                                    Value = data.Amst_Id
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
                                    data.staffdisableterms = retObject1.ToArray();
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
                }

                List<MOBILE_INSTITUTION> instidet = new List<MOBILE_INSTITUTION>();
                instidet = _FeeGroupHeadContext.MOBILE_INSTITUTION.Where(t => t.MI_ID == data.MI_Id).ToList();
                data.institutiondet = instidet.ToArray();

                data.specialheaddetails = (from a in _FeeGroupHeadContext.feespecialHead
                                           from b in _FeeGroupHeadContext.feeSGGG
                                           from c in _FeeGroupHeadContext.FeeHeadDMO
                                           where (a.MI_Id == data.MI_Id && a.FMSFH_ActiceFlag == true && a.FMSFH_Id == b.FMSFH_Id && b.FMSFHFH_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == b.FMH_Id)//&& a.IVRMSTAUL_Id==data.User_Id
                                           select new FeeSpecialFeeGroupDTO
                                           {
                                               FMSFH_Id = a.FMSFH_Id,
                                               FMSFH_Name = a.FMSFH_Name,
                                               FMSFHFH_Id = b.FMSFHFH_Id,
                                               FMH_ID = b.FMH_Id,
                                               FMH_Name = c.FMH_FeeName
                                           }).Distinct().ToArray();

                var specialheadlist = _FeeGroupHeadContext.feespecialHead.Where(t => t.MI_Id == data.MI_Id && t.FMSFH_ActiceFlag == true).Distinct().ToList();
                data.specialheadlist = specialheadlist.ToArray();


                data.fillpaymentgateway = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                           from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && b.FPGD_PGActiveFlag ==
                                           "1" && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id && b.FPGD_MobileActiveFlag == true)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }
).Distinct().ToArray();

                try
                {
                    using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "Readmissionfeeschecking";
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });


                        cmd1.Parameters.Add(new SqlParameter("@AMST_Id",
                        SqlDbType.VarChar)
                        {
                            Value = data.Amst_Id
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
                            data.Readmissionfeeschecking = retObject1.ToArray();
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

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO get_grp_reptno(FeeStudentTransactionDTO data)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();
                string termlst = "0";
                long fmttotal = 0;

                var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                from b in _FeeGroupHeadContext.AdmissionStudentDMO
                                where (a.AMST_Id == b.AMST_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                select new FeeStudentTransactionDTO
                                {
                                    AMST_ECSFlag = b.AMST_ECSFlag
                                }
               ).Distinct().ToArray();

                int ecsflag = 0;
                for (int s = 0; s < fetchecs.Count(); s++)
                {
                    if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                    {
                        ecsflag = 0;
                    }
                    else
                    {
                        ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                    }
                }


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //DateTime curdt = DateTime.Now;
                string onlineheadmapid = "0", groupidss = "0";

                long fineamountcal = 0, fethchfmaidsfine = 0, finecount = 0;

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == data.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                            ).FirstOrDefault();


                //if (data.auto_receipt_flag == 1)
                //{
                List<long> HeadId = new List<long>();
                List<long> groupidss1 = new List<long>();
                List<long> termids = new List<long>();
                foreach (var item in data.temarray)
                {
                    HeadId.Add(item.FMH_Id);
                    termids.Add(item.FMT_Id);
                    groupidss1.Add(item.FMG_Id);
                }

                foreach (var item in data.selected_list[0].trm_list)
                {
                    termids.Add(item.FMT_Id);
                    termlst = termlst + "," + item.FMT_Id;
                }

                List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
                        from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                        from d in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                        from e in _FeeGroupHeadContext.FeeStudentTransactionDMO
                        where (c.FGAR_Id == d.FGAR_Id && d.ASMAY_Id == b.ASMAY_Id && b.FMG_Id == c.FMG_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id) && c.FMG_Id == e.FMG_Id && b.FMG_Id == e.FMG_Id && e.AMST_Id == data.Amst_Id && b.FMH_Id == e.FMH_Id && e.ASMAY_Id == data.ASMAY_Id && groupidss1.Contains(e.FMG_Id))
                        select new FeeStudentTransactionDTO
                        {
                            FGAR_Id = c.FGAR_Id
                        }
                       ).Distinct().ToList();

                for (int r = 0; r < grps.Count(); r++)
                {

                    //praveen
                    foreach (var item10 in data.temarray)
                    {
                        termids.Add(item10.FMT_Id);
                    }
                    //praveen

                    onlineheadmapid = grps[r].FGAR_Id.ToString();

                    List<FeeStudentTransactionDTO> grps1 = new List<FeeStudentTransactionDTO>();
                    grps1 = (from a in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                             from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                             from c in _FeeGroupHeadContext.FeeStudentTransactionDMO
                             where (b.FGAR_Id == a.FGAR_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FGAR_Id == Convert.ToInt64(onlineheadmapid) && c.FMG_Id == a.FMG_Id && c.AMST_Id == data.Amst_Id)
                             select new FeeStudentTransactionDTO
                             {
                                 FMG_Id = a.FMG_Id
                             }
                     ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    foreach (var item in grps1)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    for (int d = 0; d < grps1.Count(); d++)
                    {
                        groupidss = groupidss + ',' + grps1[d].FMG_Id;
                    }

                    termids.Clear();


                    //praveen
                    foreach (var item in data.selected_list)
                    {
                        foreach (var item1 in item.trm_list)
                        {

                            var newgrplst = (from a in _FeeGroupHeadContext.feeGGG.Where(y => y.FMGG_Id == item1.FMGG_Id)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FMG_Id = a.FMG_Id
                                             }).Distinct().ToList();


                            foreach (var fmg in newgrplst)
                            {
                                if (grpid.Contains(fmg.FMG_Id))
                                {
                                    termids.Add(item1.FMT_Id);
                                }
                            }

                        }

                    }
                    if (termids.Count > 0)
                    {
                        List<long> temptermids = new List<long>();
                        temptermids = termids.Distinct().ToList();
                        termids = temptermids;
                    }

                    //praveen

                    List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();
                    list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                                from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new FeeStudentTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    FGAR_Id = c.FGAR_Id,
                                }
                         ).Distinct().ToList();

                    //List<FeeStudentTransactionDTO> groupwiseamount = new List<FeeStudentTransactionDTO>();
                    decimal groupwiseamount = 0;
                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                           from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                           from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                           where (a.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && i.FSS_ToBePaid > 0 && termids.Contains(a.fmt_id) && i.AMST_Id == data.Amst_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid)
                                           }
  ).Sum(t => t.FSS_ToBePaid);

                    }
                    else
                    {
                        groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                           from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                           from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                           where (a.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && i.FSS_ToBePaid > 0 && termids.Contains(a.fmt_id) && i.AMST_Id == data.Amst_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && a.ASMCL_Id == data.ASMCL_ID && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid)
                                           }
  ).Sum(t => t.FSS_ToBePaid);

                    }
                    var Euserid = (from a in _FeeGroupHeadContext.FeeGroupDMO
                                   where (a.MI_Id == Convert.ToInt64(data.MI_Id) && grpid.Contains(a.FMG_Id))
                                   select new FeeStudentTransactionDTO
                                   {
                                       enduserid = a.user_id,
                                   }
             ).Distinct().Take(1).ToArray();
                    //added on 02-07-2018

                    data.userid = Convert.ToInt64(Euserid[0].enduserid);


                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "receiptnogeneration";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                            SqlDbType.VarChar, 100)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@asmayid",
                           SqlDbType.NVarChar, 100)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmgid",
                       SqlDbType.NVarChar, 100)
                        {
                            Value = groupidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@receiptno",
            SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

                        var rece_amt = new { receiptno = data.FYP_Receipt_No, amt = groupwiseamount };

                        dynamicrecgen.Add(rece_amt);
                        List<FeeStudentTransactionDTO> groupwisefmaidsnew = new List<FeeStudentTransactionDTO>();

                        List<FeeStudentTransactionDTO> groupwisefmaids = new List<FeeStudentTransactionDTO>();
                        if (readterms.IVRMGC_Classwise_Payment != "1")
                        {
                            groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                               from c in _FeeGroupHeadContext.FeeHeadDMO
                                               from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                               from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && termids.Contains(a.fmt_id) && ((i.FSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMST_Id == data.Amst_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FMA_Id = i.FMA_Id,
                                                   FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                                                   FMH_Flag = c.FMH_Flag,
                                                   FSS_RebateAmount = Convert.ToInt64(i.FSS_CurrentYrCharges)
                                               }
                            ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                            groupwisefmaidsnew = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                  from c in _FeeGroupHeadContext.FeeHeadDMO
                                                  from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                  from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                  from e in _FeeGroupHeadContext.Yearlygroups
                                                  where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && termids.Contains(a.fmt_id) && ((i.FSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMST_Id == data.Amst_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway && i.FMG_Id == e.FMG_Id && e.ASMAY_Id == data.ASMAY_Id && e.FYG_RebateApplicableFlg == true)
                                                  select new FeeStudentTransactionDTO
                                                  {
                                                      FMA_Id = i.FMA_Id,
                                                      FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                                                      FMH_Flag = c.FMH_Flag,
                                                      FSS_RebateAmount = Convert.ToInt64(i.FSS_CurrentYrCharges)

                                                  }

              ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        }
                        else
                        {
                          
                            groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                               from c in _FeeGroupHeadContext.FeeHeadDMO
                                               from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                               from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && termids.Contains(a.fmt_id) && ((i.FSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMST_Id == data.Amst_Id && a.ASMCL_Id == data.ASMCL_ID && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FMA_Id = i.FMA_Id,
                                                   FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                                                   FMH_Flag = c.FMH_Flag,
                                                   FSS_RebateAmount = Convert.ToInt64(i.FSS_CurrentYrCharges)
                                               }
                            ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                            groupwisefmaidsnew = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                  from c in _FeeGroupHeadContext.FeeHeadDMO
                                                  from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                  from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                  from e in _FeeGroupHeadContext.Yearlygroups
                                                  where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && termids.Contains(a.fmt_id) && ((i.FSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMST_Id == data.Amst_Id && a.ASMCL_Id == data.ASMCL_ID && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway && b.FPGD_PGName == data.onlinepaygteway && i.FMG_Id == e.FMG_Id && e.ASMAY_Id == data.ASMAY_Id && e.FYG_RebateApplicableFlg == true)
                                                  select new FeeStudentTransactionDTO
                                                  {
                                                      FMA_Id = i.FMA_Id,
                                                      FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                                                      FMH_Flag = c.FMH_Flag,
                                                      FSS_RebateAmount = Convert.ToInt64(i.FSS_CurrentYrCharges)
                                                  }
                            ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        }
                        var cnt1 = _context.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id && t.FMC_RebateAplicableFlg == true && t.FMC_RebateAgainstFullPaymentFlg == true && t.FMC_RebateAgainstPartialPaymentFlg == true).ToList();

                        for (var i = 0; i < groupwisefmaidsnew.Count; i++)
                        {
                            fmttotal = fmttotal + groupwisefmaidsnew[i].FSS_RebateAmount;
                        }

                        data.FSS_RebateAmount = 0;
                        string rebateamount;
                        if (fmttotal > 0)
                        {
                            if (cnt1.Count > 0)
                            {
                                using (var cmd4 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd4.CommandText = "Fee_RebateTermWise_calculation_BOTH";
                                    cmd4.CommandType = CommandType.StoredProcedure;
                                    cmd4.Parameters.Add(new SqlParameter("@MI_Id",
                                             SqlDbType.VarChar)
                                    {
                                        Value = data.MI_Id
                                    });

                                    cmd4.Parameters.Add(new SqlParameter("@ASMAY_ID",
                                       SqlDbType.VarChar)
                                    {
                                        Value = data.ASMAY_Id
                                    });
                                    cmd4.Parameters.Add(new SqlParameter("@AMST_ID",
                                   SqlDbType.VarChar)
                                    {
                                        Value = data.Amst_Id
                                    });
                                    cmd4.Parameters.Add(new SqlParameter("@FMT_ID",
                                  SqlDbType.VarChar)
                                    {
                                        Value = termlst
                                    });
                                    cmd4.Parameters.Add(new SqlParameter("@paiddate",
                                  SqlDbType.DateTime)
                                    {
                                        Value = DateTime.Now
                                    });
                                    cmd4.Parameters.Add(new SqlParameter("@paidamount",
                                   SqlDbType.VarChar)
                                    {
                                        Value = fmttotal
                                    });

                                    cmd4.Parameters.Add(new SqlParameter("@USERID",
                                   SqlDbType.VarChar)
                                    {
                                        Value = data.userid
                                    });

                                    cmd4.Parameters.Add(new SqlParameter("@totalrebateamount",
                        SqlDbType.BigInt)
                                    {
                                        Direction = ParameterDirection.Output
                                    });
                                    if (cmd4.Connection.State != ConnectionState.Open)
                                        cmd4.Connection.Open();
                                    var data4 = cmd4.ExecuteNonQuery();
                                    rebateamount = cmd4.Parameters["@totalrebateamount"].Value.ToString();
                                }



                            }
                            else
                            {
                                using (var cmd5 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd5.CommandText = "Fee_RebateTermWise_calculation";
                                    cmd5.CommandType = CommandType.StoredProcedure;
                                    cmd5.Parameters.Add(new SqlParameter("@MI_Id",
                                             SqlDbType.VarChar)
                                    {
                                        Value = data.MI_Id
                                    });

                                    cmd5.Parameters.Add(new SqlParameter("@ASMAY_ID",
                                       SqlDbType.VarChar)
                                    {
                                        Value = data.ASMAY_Id
                                    });
                                    cmd5.Parameters.Add(new SqlParameter("@AMST_ID",
                                   SqlDbType.VarChar)
                                    {
                                        Value = data.Amst_Id
                                    });
                                    cmd5.Parameters.Add(new SqlParameter("@FMT_ID",
                                  SqlDbType.VarChar)
                                    {
                                        Value = termlst
                                    });
                                    cmd5.Parameters.Add(new SqlParameter("@paiddate",
                                  SqlDbType.DateTime)
                                    {
                                        Value = DateTime.Now
                                    });
                                    cmd5.Parameters.Add(new SqlParameter("@paidamount",
                                   SqlDbType.VarChar)
                                    {
                                        Value = fmttotal
                                    });

                                    cmd5.Parameters.Add(new SqlParameter("@USERID",
                                   SqlDbType.VarChar)
                                    {
                                        Value = data.userid
                                    });

                                    cmd5.Parameters.Add(new SqlParameter("@totalrebateamount",
                        SqlDbType.BigInt)
                                    {
                                        Direction = ParameterDirection.Output
                                    });
                                    if (cmd5.Connection.State != ConnectionState.Open)
                                        cmd5.Connection.Open();
                                    var data3 = cmd5.ExecuteNonQuery();
                                    rebateamount = cmd5.Parameters["@totalrebateamount"].Value.ToString();
                                }

                            }

                            data.FSS_RebateAmount = Convert.ToInt64(rebateamount);

                            fmttotal = 0;
                            rebateamount = "";
                        }
                        Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                        onlinemtrans.FMOT_Trans_Id = data.trans_id;
                        //onlinemtrans.FMOT_Amount = data.topayamount;
                        onlinemtrans.FMOT_Amount = rece_amt.amt;
                        onlinemtrans.FMOT_Date = indianTime;
                        onlinemtrans.FMOT_Flag = "P";
                        onlinemtrans.PASR_Id = 0;
                        onlinemtrans.AMST_Id = data.Amst_Id;
                        onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;
                        onlinemtrans.FYP_PayModeType = data.FYP_PayModeType;
                        onlinemtrans.FMOT_PayGatewayType = data.onlinepaygteway;

                        onlinemtrans.MI_Id = data.MI_Id;
                        onlinemtrans.ASMAY_ID = data.ASMAY_Id;

                        _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);
                        var count = 0;
                        List<long> ftiidfinecal = new List<long>();

                        for (var j = 0; j < groupwisefmaids.Count; j++)
                        {
                            ftiidfinecal.Add(groupwisefmaids[j].FMA_Id);
                            if (groupwisefmaids[j].FMH_Flag == "F")
                            {
                                count = count + 1;
                            }
                        }
                        var ftiids = _FeeGroupHeadContext.FeeAmountEntryDMO.Where(z => z.MI_Id == data.MI_Id && ftiidfinecal.Contains(z.FMA_Id)).Select(z => z.FTI_Id).Distinct().ToList();
                       

                        

                        if (count == 0)
                        {
                            fineamountcal = 0;

                        }


                        if (count > 1)
                        {
                            foreach( var x in ftiids)
                            {
                                long totalfinestthomas = 0;
                                for (var j = 0; j < groupwisefmaids.Count; j++)
                                {


                                    var fmaidcnt = _FeeGroupHeadContext.FeeAmountEntryDMO.Where(z => z.MI_Id == data.MI_Id && z.FMA_Id== groupwisefmaids[j].FMA_Id && z.FTI_Id==x).ToList();

                                    if (fmaidcnt.Count > 0)
                                    {
                                       

                                        if (groupwisefmaids[j].FMH_Flag == "F")
                                        {
                                            fethchfmaidsfine = groupwisefmaids[j].FMA_Id;
                                        }
                                        else
                                        {
                                            Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                            onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                            onlinettrans.FMA_Id = groupwisefmaids[j].FMA_Id;
                                            onlinettrans.FTOT_Amount = groupwisefmaids[j].FSS_ToBePaid;
                                            onlinettrans.FTOT_Created_date = indianTime;
                                            onlinettrans.FTOT_Updated_date = indianTime;
                                            onlinettrans.FTOT_Concession = 0;
                                            onlinettrans.FTOT_Fine = 0;
                                            onlinettrans.FTOT_RebateAmount = 0;

                                            try
                                            {
                                                using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd1.CommandText = "Readmissionfeeschecking";
                                                    cmd1.CommandType = CommandType.StoredProcedure;
                                                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                                                   SqlDbType.BigInt)
                                                    {
                                                        Value = data.MI_Id
                                                    });

                                                    cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                                                     SqlDbType.BigInt)
                                                    {
                                                        Value = data.ASMAY_Id
                                                    });


                                                    cmd1.Parameters.Add(new SqlParameter("@AMST_Id",
                                                    SqlDbType.VarChar)
                                                    {
                                                        Value = data.Amst_Id
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
                                                        data.Readmissionfeeschecking = retObject1.ToArray();
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
                                            if (data.Readmissionfeeschecking.Length > 0)
                                            {
                                                var readmssionflg = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                                     from b in _FeeGroupHeadContext.FeeHeadDMO

                                                                     where (a.AMST_Id == data.Amst_Id
                                                                     && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FMH_Id == b.FMH_Id && b.FMH_Flag == "RA" && a.FSS_ToBePaid > 0)
                                                                     select a.AMST_Id
                        ).Distinct().ToList();

                                                if ((data.Readmissionfeeschecking == null || data.Readmissionfeeschecking.Length == 0) || (readmssionflg.Count == 0))
                                                {

                                                    using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        if (ecsflag.Equals(0))
                                                        {
                                                            cmd1.CommandText = "Sp_Calculate_Fine";
                                                        }
                                                        else
                                                        {
                                                            cmd1.CommandText = "Sp_Calculate_Fine_ECS";
                                                        }

                                                        //cmd1.CommandText = "Sp_Calculate_Fine";
                                                        cmd1.CommandType = CommandType.StoredProcedure;
                                                        cmd1.Parameters.Add(new SqlParameter("@On_Date",
                                                            SqlDbType.DateTime, 100)
                                                        {
                                                            //Value = curdt
                                                            Value = indianTime
                                                        });

                                                        cmd1.Parameters.Add(new SqlParameter("@fma_id",
                                                           SqlDbType.NVarChar, 100)
                                                        {
                                                            Value = groupwisefmaids[j].FMA_Id
                                                        });
                                                        cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                                                       SqlDbType.NVarChar, 100)
                                                        {
                                                            Value = data.ASMAY_Id
                                                        });

                                                        cmd1.Parameters.Add(new SqlParameter("@amt",
                                            SqlDbType.Decimal, 500)
                                                        {
                                                            Direction = ParameterDirection.Output
                                                        });

                                                        cmd1.Parameters.Add(new SqlParameter("@flgArr",
                                           SqlDbType.Int, 500)
                                                        {
                                                            Direction = ParameterDirection.Output
                                                        });

                                                        if (cmd1.Connection.State != ConnectionState.Open)
                                                            cmd1.Connection.Open();

                                                        var execfine = cmd1.ExecuteNonQuery();

                                                        //finewaiedoff

                                                        var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                                             where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMA_Id == groupwisefmaids[j].FMA_Id && a.AMST_Id == data.Amst_Id)
                                                                             select new FeeStudentTransactionDTO
                                                                             {
                                                                                 FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                                                 FSWO_FineFlg = a.FSWO_FineFlg,
                                                                                 FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                                             }
                               ).Distinct().ToList();

                                                        if (waivedofffine.Count() > 0)
                                                        {
                                                            if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                                            {
                                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                                {
                                                                    totalfinestthomas = totalfinestthomas + (Convert.ToInt32(cmd1.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount));
                                                                }
                                                            }
                                                            else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                                            {
                                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                                {
                                                                    totalfinestthomas = totalfinestthomas + 0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                                {
                                                                    totalfinestthomas = totalfinestthomas + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                            {
                                                                totalfinestthomas = totalfinestthomas + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                            }
                                                        }


                                                    }
                                                }
                                            }
                                            else
                                            {
                                                using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                                {
                                                    if (ecsflag.Equals(0))
                                                    {
                                                        cmd1.CommandText = "Sp_Calculate_Fine";
                                                    }
                                                    else
                                                    {
                                                        cmd1.CommandText = "Sp_Calculate_Fine_ECS";
                                                    }

                                                    //cmd1.CommandText = "Sp_Calculate_Fine";
                                                    cmd1.CommandType = CommandType.StoredProcedure;
                                                    cmd1.Parameters.Add(new SqlParameter("@On_Date",
                                                        SqlDbType.DateTime, 100)
                                                    {
                                                        //Value = curdt
                                                        Value = indianTime
                                                    });

                                                    cmd1.Parameters.Add(new SqlParameter("@fma_id",
                                                       SqlDbType.NVarChar, 100)
                                                    {
                                                        Value = groupwisefmaids[j].FMA_Id
                                                    });
                                                    cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                                                   SqlDbType.NVarChar, 100)
                                                    {
                                                        Value = data.ASMAY_Id
                                                    });

                                                    cmd1.Parameters.Add(new SqlParameter("@amt",
                                        SqlDbType.Decimal, 500)
                                                    {
                                                        Direction = ParameterDirection.Output
                                                    });

                                                    cmd1.Parameters.Add(new SqlParameter("@flgArr",
                                       SqlDbType.Int, 500)
                                                    {
                                                        Direction = ParameterDirection.Output
                                                    });

                                                    if (cmd1.Connection.State != ConnectionState.Open)
                                                        cmd1.Connection.Open();

                                                    var execfine = cmd1.ExecuteNonQuery();

                                                    //finewaiedoff

                                                    var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                                         where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMA_Id == groupwisefmaids[j].FMA_Id && a.AMST_Id == data.Amst_Id)
                                                                         select new FeeStudentTransactionDTO
                                                                         {
                                                                             FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                                             FSWO_FineFlg = a.FSWO_FineFlg,
                                                                             FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                                         }
                           ).Distinct().ToList();

                                                    if (waivedofffine.Count() > 0)
                                                    {
                                                        if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                                        {
                                                            if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                            {
                                                                totalfinestthomas = totalfinestthomas + (Convert.ToInt32(cmd1.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount));
                                                            }
                                                        }
                                                        else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                                        {
                                                            if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                            {
                                                                totalfinestthomas = totalfinestthomas + 0;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                            {
                                                                totalfinestthomas = totalfinestthomas + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                        {
                                                            totalfinestthomas = totalfinestthomas + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                        }
                                                    }


                                                }
                                            }

                                            _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                        }

                                        if (groupwisefmaids[j].FMH_Flag == "F")
                                        {
                                            

                                                fineamountcal += totalfinestthomas;
                                            
                                                fethchfmaidsfine = groupwisefmaids[j].FMA_Id;
                                                Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                                onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                                onlinettrans.FMA_Id = groupwisefmaids[j].FMA_Id;
                                                onlinettrans.FTOT_Amount = totalfinestthomas;
                                                onlinettrans.FTOT_Created_date = indianTime;
                                                onlinettrans.FTOT_Updated_date = indianTime;
                                                onlinettrans.FTOT_Concession = 0;
                                                onlinettrans.FTOT_Fine = 0;
                                                onlinettrans.FTOT_RebateAmount = 0;
                                                _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                           
                                        }



                                    }





                                }
                            }
                        }
                        else if (count >= 1)
                        {
                            long totalfinestthomas = 0;
                            for (var j = 0; j < groupwisefmaids.Count; j++)
                            {





                                if (groupwisefmaids[j].FMH_Flag == "F")
                                {
                                    fethchfmaidsfine = groupwisefmaids[j].FMA_Id;
                                }
                                else
                                {
                                    Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                    onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                    onlinettrans.FMA_Id = groupwisefmaids[j].FMA_Id;
                                    onlinettrans.FTOT_Amount = groupwisefmaids[j].FSS_ToBePaid;
                                    onlinettrans.FTOT_Created_date = indianTime;
                                    onlinettrans.FTOT_Updated_date = indianTime;
                                    onlinettrans.FTOT_Concession = 0;
                                    onlinettrans.FTOT_Fine = 0;
                                    onlinettrans.FTOT_RebateAmount = 0;

                                    using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        if (ecsflag.Equals(0))
                                        {
                                            cmd1.CommandText = "Sp_Calculate_Fine";
                                        }
                                        else
                                        {
                                            cmd1.CommandText = "Sp_Calculate_Fine_ECS";
                                        }

                                        //cmd1.CommandText = "Sp_Calculate_Fine";
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add(new SqlParameter("@On_Date",
                                            SqlDbType.DateTime, 100)
                                        {
                                            //Value = curdt
                                            Value = indianTime
                                        });

                                        cmd1.Parameters.Add(new SqlParameter("@fma_id",
                                           SqlDbType.NVarChar, 100)
                                        {
                                            Value = groupwisefmaids[j].FMA_Id
                                        });
                                        cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                                       SqlDbType.NVarChar, 100)
                                        {
                                            Value = data.ASMAY_Id
                                        });

                                        cmd1.Parameters.Add(new SqlParameter("@amt",
                            SqlDbType.Decimal, 500)
                                        {
                                            Direction = ParameterDirection.Output
                                        });

                                        cmd1.Parameters.Add(new SqlParameter("@flgArr",
                           SqlDbType.Int, 500)
                                        {
                                            Direction = ParameterDirection.Output
                                        });

                                        if (cmd1.Connection.State != ConnectionState.Open)
                                            cmd1.Connection.Open();

                                        var execfine = cmd1.ExecuteNonQuery();

                                        //finewaiedoff

                                        var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                             where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMA_Id == groupwisefmaids[j].FMA_Id && a.AMST_Id == data.Amst_Id)
                                                             select new FeeStudentTransactionDTO
                                                             {
                                                                 FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                                 FSWO_FineFlg = a.FSWO_FineFlg,
                                                                 FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                             }
               ).Distinct().ToList();

                                        if (waivedofffine.Count() > 0)
                                        {
                                            if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    totalfinestthomas = totalfinestthomas + (Convert.ToInt32(cmd1.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount));
                                                }
                                            }
                                            else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    totalfinestthomas = totalfinestthomas + 0;
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    totalfinestthomas = totalfinestthomas + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                            {
                                                totalfinestthomas = totalfinestthomas + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                            }
                                        }

                                       
                                    }
                                    fineamountcal += totalfinestthomas;

                                    _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                }


                                if (groupwisefmaids[j].FMH_Flag == "F")
                                {
                                    if (finecount < 1)
                                    {
                                        finecount = finecount + 1;
                                        fethchfmaidsfine = groupwisefmaids[j].FMA_Id;
                                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                        onlinettrans.FMA_Id = groupwisefmaids[j].FMA_Id;
                                        onlinettrans.FTOT_Amount = totalfinestthomas;
                                        onlinettrans.FTOT_Created_date = indianTime;
                                        onlinettrans.FTOT_Updated_date = indianTime;
                                        onlinettrans.FTOT_Concession = 0;
                                        onlinettrans.FTOT_Fine = 0;
                                        onlinettrans.FTOT_RebateAmount = 0;
                                        _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                    }
                                }









                            }
                        }
                      
                          


                        onlinemtrans.FMOT_Amount = Convert.ToInt32(rece_amt.amt) + Convert.ToInt32(fineamountcal) - data.FSS_RebateAmount;



                        //if(fineamountcal>0)
                        //{
                        //    var obj = _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Single(t => t.FMA_Id == fethchfmaidsfine && t.FMOT_Id == onlinemtrans.FMOT_Id);

                        //    obj.FTOT_Amount = fineamountcal;
                        //}

                        groupidss = "0";
                        fineamountcal = 0;
                        finecount = 0;
                    }

                    termids.Clear();
                }

                var contactexisttransaction = 0;

                using (var dbCtxTxn = _FeeGroupHeadContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _FeeGroupHeadContext.SaveChanges();
                        dbCtxTxn.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

                //if (dynamicrecgen.Count() > 0)
                //{
                //    data.recenocol = dynamicrecgen.ToArray();
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        //=============end 
        public PaymentDetails payuresponse(PaymentDetails response)
        {

            FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
            PaymentDetails dto = new PaymentDetails();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            if (response.status == "success")
            {
                string txnid = response.txnid.ToString();

                var groups = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                              where (a.MI_Id == Convert.ToInt32(response.udf3) && a.ASMAY_ID == Convert.ToInt32(response.udf1) && a.fyp_transaction_id == response.txnid)
                              select new FeeStudentTransactionDTO
                              {
                                  FYP_Receipt_No = a.FYP_Receipt_No
                              }
                      ).Distinct().ToList();

                if (groups.Count == 0)
                {
                    var confirmstatus = insertdatainfeetables(response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid.ToString(), response.udf1, indianTime, "0");

                    if (Convert.ToInt32(confirmstatus) > 0)
                    {
                        pgmod.MI_Id = Convert.ToInt64(response.udf3);
                        pgmod.amst_mobile = response.phone;
                        pgmod.Amst_Id = Convert.ToInt64(response.udf2);
                        pgmod.amst_email_id = response.email;

                        SMS sms = new SMS(_context);

                        sms.sendSms(Convert.ToInt32(response.udf3), Convert.ToInt64(response.phone), "FEEONLINEPAYMENT", Convert.ToInt32(response.udf2));

                        Email Email = new Email(_context);

                        Email.sendmail(Convert.ToInt32(response.udf3), response.email, "FEEONLINEPAYMENT", Convert.ToInt32(response.udf2));

                    }
                }

            }
            else
            {
                SMS sms = new SMS(_context);

                sms.sendSms(Convert.ToInt32(response.udf3), Convert.ToInt64(response.phone), "FEEONLINEPAYMENTFAIL", Convert.ToInt32(response.udf2));

                Email Email = new Email(_context);

                Email.sendmail(Convert.ToInt32(response.udf3), response.email, "FEEONLINEPAYMENTFAIL", Convert.ToInt32(response.udf2));

                dto.status = response.status;
            }

            return response;
        }

        public FeeStudentTransactionDTO getgrouportermdeta(FeeStudentTransactionDTO data)
        {
            try
            {

                data.ASMAY_Id = getacademicyearcongig(data);

                if (data.Paymenttype == "R" || data.Paymenttype == "N")
                {
                    data.fillinstallment = (from a in _FeeGroupHeadContext.feeTr
                                            where (a.MI_Id == data.MI_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                            select new FeeStudentTransactionDTO
                                            {
                                                FMG_GroupName = a.FMT_Name,
                                                FMG_Id = a.FMT_Id,
                                            }
    ).Distinct().ToArray();

                }
                else if (data.Paymenttype == "T")
                {
                }

                else if (data.Paymenttype == "H")
                {
                    data.fillinstallment = (from a in _FeeGroupHeadContext.FeeGroupDMO
                                            from b in _FeeGroupHeadContext.FeeStudentGroupMappingDMO
                                            from c in _FeeGroupHeadContext.FEeGroupLoginPreviledgeDMO
                                            from d in _FeeGroupHeadContext.FeeHeadDMO
                                            where (c.FMH_Id == d.FMH_Id && d.FMH_Flag == "H" && a.FMG_Id == b.FMG_Id && b.AMST_Id == data.Amst_Id && a.FMG_ActiceFlag == true && c.FMG_ID == b.FMG_Id && c.User_Id == data.userid) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                            select new FeeStudentTransactionDTO
                                            {
                                                FMG_GroupName = a.FMG_GroupName,
                                                FMG_Id = a.FMG_Id,

                                            }
         ).Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO generatehashsequence(FeeStudentTransactionDTO data)
        {
            try
            {
                //data.ASMAY_Id = getacademicyearcongig(data);
                var config = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();

                data.fillstudent = (from a in _FeeGroupHeadContext.AdmissionStudentDMO
                                    from b in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                    from c in _FeeGroupHeadContext.admissioncls
                                    from d in _FeeGroupHeadContext.school_M_Section
                                    where (c.ASMCL_Id == b.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id)
                                    select new FeeStudentTransactionDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        classname = c.ASMCL_ClassName,
                                        sectionname = d.ASMC_SectionName,
                                        ASMCL_ID = b.ASMCL_Id,
                                        amst_mobile = a.AMST_MobileNo,
                                        amst_email_id = a.AMST_emailId,
                                        ASYST_Id = b.ASYST_Id,
                                        AMST_PerCity = a.AMST_PerCity
                                    }
               ).OrderByDescending(t => t.ASYST_Id).ToArray();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == data.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_OnlinePaymentCompany = a.IVRMGC_OnlinePaymentCompany,
                                     IVRMGC_Classwise_Payment = a.IVRMGC_OnlinePaymentCompany
                                 }
                                 ).FirstOrDefault();

                if (data.onlinepaygteway == "PAYU")
                {
                    data.paydet = paymentPart(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "BILLDESK")
                {
                    data = paymentPartbilldesk(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "RAZORPAY")
                {
                    data.paydet = paymentPartRAZORPAY(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "PAYTM")
                {
                    data.paydet = paymentPartpaytm(data, data.topayamount);
                    //data = statusapii(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "EASEBUZZ")
                {
                    if ((config[0].FMC_EnablePartialPaymentFlg == 1 || config[0].FMC_EnablePartialPaymentFlg == 2)
                        && config[0].FMC_Fine_Column == 0
                        && config[0].FMC_FineEnableDisable == false && config[0].FMC_RebateAplicableFlg == false)
                    {
                        data = paymentPartEasebuzzPartial(data);
                    }

                    else
                    {
                        //data = paymentPartEasyBuzz(data);
                        data = paymentPartEasyBuzzSplit(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO getcusgrp(FeeStudentTransactionDTO data)
        {
            data.ASMAY_Id = getacademicyearcongig(data);
            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

            try
            {
                using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "getcustomgeegroups";
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

                    cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.Amst_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@fmt_id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ftiidss
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
                        data.customgroup = retObject1.ToArray();
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
            return data;
        }

        public string insertdatainfeetables(string miid, string termid, string studentid, string classid, decimal amount, string transid, string refid, string yearid, DateTime indianTime, string transactionid)
        {
            var contactexisttransaction = 0;

            try
            {
                string recnoen = "";
                var fetchfmhotid = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                    where (a.AMST_Id == Convert.ToInt64(studentid) && a.FMOT_Trans_Id == transid.ToString())
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMHOT_Id = a.FMOT_Id,
                                        FMA_Amount = a.FMOT_Amount,
                                        FYP_PayModeType = a.FYP_PayModeType,
                                        FMOT_PayGatewayType = a.FMOT_PayGatewayType
                                    }).ToArray();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    var fethchfmgids = (from a in _FeeGroupHeadContext.Fee_T_Online_TransactionDMO
                                        from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                        from c in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        where (c.FMH_Id == b.FMH_Id && c.fti_id == b.FTI_Id && c.fmg_id == b.FMG_Id && a.FMA_Id == b.FMA_Id && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid))
                                        select new FeeStudentTransactionDTO
                                        {
                                            FMG_Id = b.FMG_Id
                                        }).Distinct().ToArray();

                    List<long> grpid = new List<long>();
                    string groupidss = "0";
                    foreach (var item in fethchfmgids)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    for (int d = 0; d < fethchfmgids.Count(); d++)
                    {
                        groupidss = groupidss + ',' + fethchfmgids[d].FMG_Id;
                    }


                    List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();
                    list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                                from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid) && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new FeeStudentTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    FGAR_Id = c.FGAR_Id,
                                }
                         ).Distinct().ToList();

                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "receiptnogeneration";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                            SqlDbType.VarChar, 100)
                        {
                            Value = Convert.ToInt32(miid)
                        });

                        cmd.Parameters.Add(new SqlParameter("@asmayid",
                           SqlDbType.NVarChar, 100)
                        {
                            Value = Convert.ToInt32(yearid)
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmgid",
                       SqlDbType.NVarChar, 100)
                        {
                            Value = groupidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@receiptno",
            SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        recnoen = cmd.Parameters["@receiptno"].Value.ToString();

                        var groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_T_Online_TransactionDMO
                                               from c in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                               where (a.FMOT_Id == c.FMOT_Id && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && c.AMST_Id == Convert.ToInt64(studentid))
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FMA_Id = a.FMA_Id,
                                                   FSS_ToBePaid = Convert.ToInt64(a.FTOT_Amount),
                                                   FTOT_RebateAmount = a.FTOT_RebateAmount

                                               }
                             ).ToArray();

                        FeePaymentDetailsDMO onlinemtrans = new FeePaymentDetailsDMO();

                        onlinemtrans.ASMAY_ID = Convert.ToInt64(yearid);
                        onlinemtrans.FTCU_Id = 1;
                        onlinemtrans.FYP_Receipt_No = recnoen;
                        onlinemtrans.FYP_Bank_Name = "";
                        onlinemtrans.FYP_Bank_Or_Cash = "O";
                        onlinemtrans.FYP_DD_Cheque_No = "";
                        onlinemtrans.FYP_DD_Cheque_Date = indianTime;

                        onlinemtrans.FYP_Date = indianTime;
                        onlinemtrans.FYP_Tot_Amount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinemtrans.FYP_Tot_Waived_Amt = 0;
                        onlinemtrans.FYP_Tot_Fine_Amt = 0;
                        onlinemtrans.FYP_Tot_Concession_Amt = 0;
                        onlinemtrans.FYP_Remarks = "Online Regular Payment";
                        // onlinemtrans.IVRMSTAUL_ID = Convert.ToInt64(studentid);

                        onlinemtrans.FYP_Chq_Bounce = "CL";
                        onlinemtrans.MI_Id = Convert.ToInt64(miid);
                        onlinemtrans.DOE = indianTime;
                        onlinemtrans.CreatedDate = indianTime;
                        onlinemtrans.UpdatedDate = indianTime;

                        onlinemtrans.FYP_PayModeType = fetchfmhotid[r].FYP_PayModeType;
                        onlinemtrans.FYP_PayGatewayType = fetchfmhotid[r].FMOT_PayGatewayType;

                        //added on 02-07-2018
                        var Euserid = (from a in _FeeGroupHeadContext.FeeGroupDMO
                                       where (a.MI_Id == Convert.ToInt64(miid) && grpid.Contains(a.FMG_Id))
                                       select new FeeStudentTransactionDTO
                                       {
                                           enduserid = a.user_id,
                                       }
                           ).Distinct().Take(1).ToArray();
                        //added on 02-07-2018

                        onlinemtrans.user_id = Convert.ToInt64(Euserid[0].enduserid);
                        onlinemtrans.fyp_transaction_id = transid;

                        onlinemtrans.FYP_OnlineChallanStatusFlag = "Sucessfull";
                        onlinemtrans.FYP_PaymentReference_Id = refid.ToString();
                        onlinemtrans.FYP_ChallanNo = "";

                        _FeeGroupHeadContext.FeePaymentDetailsDMO.Add(onlinemtrans);

                        //multimode of payment
                        Fee_Y_Payment_PaymentModeSchool onlinemulti = new Fee_Y_Payment_PaymentModeSchool();
                        onlinemulti.FYP_Id = onlinemtrans.FYP_Id;
                        onlinemulti.FYP_TransactionTypeFlag = "O";
                        onlinemulti.FYPPM_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinemulti.FYPPM_LedgerId = 0;
                        onlinemulti.FYPPM_BankName = "";
                        onlinemulti.FYPPM_DDChequeNo = "";
                        onlinemulti.FYPPM_DDChequeDate = indianTime;
                        onlinemulti.FYPPM_TransactionId = transid;
                        onlinemulti.FYPPM_PaymentReferenceId = refid.ToString();
                        onlinemulti.FYPPM_ClearanceStatusFlag = "0";
                        onlinemulti.FYPPM_ClearanceDate = indianTime;
                        _FeeGroupHeadContext.Add(onlinemulti);
                        //multimode of payment

                        Fee_Y_Payment_School_StudentDMO onlinestuapp = new Fee_Y_Payment_School_StudentDMO();

                        onlinestuapp.FYP_Id = onlinemtrans.FYP_Id;
                        onlinestuapp.AMST_Id = Convert.ToInt64(studentid);
                        onlinestuapp.ASMAY_Id = Convert.ToInt64(yearid);
                        onlinestuapp.FTP_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinestuapp.FTP_TotalConcessionAmount = 0;
                        onlinestuapp.FTP_TotalFineAmount = 0;
                        onlinestuapp.FTP_TotalWaivedAmount = 0;

                        _FeeGroupHeadContext.Fee_Y_Payment_School_StudentDMO.Add(onlinestuapp);

                        for (int s = 0; s < groupwisefmaids.Count(); s++)
                        {
                            FeeTransactionPaymentDMO onlinettrans = new FeeTransactionPaymentDMO();
                            onlinettrans.FYP_Id = onlinemtrans.FYP_Id;
                            onlinettrans.FMA_Id = groupwisefmaids[s].FMA_Id;
                            onlinettrans.FTP_Paid_Amt = groupwisefmaids[s].FSS_ToBePaid;
                            onlinettrans.FTP_Fine_Amt = 0;
                            onlinettrans.FTP_Concession_Amt = 0;
                            onlinettrans.FTP_Waived_Amt = 0;
                            onlinettrans.ftp_remarks = "Online Regular Payment";
                            // onlinettrans.FTP_RebateAmount = groupwisefmaids[s].FTOT_RebateAmount;
                            _FeeGroupHeadContext.FeeTransactionPaymentDMO.Add(onlinettrans);

                            if (groupwisefmaids[s].FTOT_RebateAmount > 0)
                            {

                                var rebateamt = Convert.ToInt64(groupwisefmaids[s].FTOT_RebateAmount);

                                var obj_status_stf = _FeeGroupHeadContext.FeeStudentTransactionDMO.Where(t => t.MI_Id == Convert.ToInt64(miid) && t.ASMAY_Id == Convert.ToInt64(yearid) && t.AMST_Id == Convert.ToInt64(studentid) && t.FMA_Id == groupwisefmaids[s].FMA_Id && t.FSS_ActiveFlag == true).FirstOrDefault();
                                obj_status_stf.FSS_PaidAmount = (obj_status_stf.FSS_PaidAmount + groupwisefmaids[s].FSS_ToBePaid);
                                //obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount-rebateamt; 

                                //added on 11-07-2018
                                var fineheadss = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                  from b in _FeeGroupHeadContext.FeeHeadDMO
                                                  where (a.MI_Id == Convert.ToInt64(miid) && a.FMA_Id == groupwisefmaids[s].FMA_Id && a.ASMAY_Id == Convert.ToInt64(yearid) && b.FMH_Flag == "F" && a.AMST_Id == Convert.ToInt64(studentid) && a.FMH_Id == b.FMH_Id)
                                                  select new FeeStudentTransactionDTO
                                                  {
                                                      FMH_Id = a.FMH_Id,
                                                  }
                                   ).Distinct().Take(1);

                                if (fineheadss.Count() > 0)
                                {
                                    obj_status_stf.FSS_FineAmount = obj_status_stf.FSS_FineAmount + groupwisefmaids[s].FSS_ToBePaid;
                                }
                                //added on 11-07-2018


                                if (obj_status_stf.FSS_NetAmount != 0)
                                {
                                    //obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - groupwisefmaids[s].FSS_ToBePaid;
                                    if (obj_status_stf.FSS_ToBePaid - groupwisefmaids[s].FSS_ToBePaid >= 0)
                                    {
                                        obj_status_stf.FSS_ToBePaid = (obj_status_stf.FSS_ToBePaid - groupwisefmaids[s].FSS_ToBePaid) - rebateamt;
                                    }
                                    else
                                    {
                                        obj_status_stf.FSS_ToBePaid = 0;
                                        obj_status_stf.FSS_ExcessPaidAmount = groupwisefmaids[s].FSS_ToBePaid - obj_status_stf.FSS_ToBePaid;
                                        obj_status_stf.FSS_RunningExcessAmount = groupwisefmaids[s].FSS_ToBePaid - obj_status_stf.FSS_ToBePaid;
                                    }
                                }
                                else
                                {
                                    obj_status_stf.FSS_ToBePaid = 0;
                                }
                                obj_status_stf.FSS_RebateAmount = rebateamt;
                                _FeeGroupHeadContext.Update(obj_status_stf);

                                FeeStudentRebate feestureb = new FeeStudentRebate();

                                feestureb.FSREB_Id = 0;
                                feestureb.FMA_Id = groupwisefmaids[s].FMA_Id;
                                feestureb.MI_Id = Convert.ToInt64(miid);
                                feestureb.AMST_Id = Convert.ToInt64(studentid);
                                feestureb.ASMAY_Id = Convert.ToInt64(yearid);
                                feestureb.FSREB_Date = DateTime.Now;
                                feestureb.FMH_Id = obj_status_stf.FMH_Id;
                                feestureb.FMG_Id = obj_status_stf.FMG_Id;
                                feestureb.FTI_Id = obj_status_stf.FTI_Id;
                                feestureb.FSREB_RebateAmount = rebateamt;
                                feestureb.FSREB_ActiveFlag = true;
                                feestureb.FSREB_Remarks = "Online Regular Payment";



                                feestureb.FYP_Id = onlinemtrans.FYP_Id;

                                _FeeGroupHeadContext.Add(feestureb);


                            }
                            else
                            {

                                var obj_status_stf = _FeeGroupHeadContext.FeeStudentTransactionDMO.Where(t => t.MI_Id == Convert.ToInt64(miid) && t.ASMAY_Id == Convert.ToInt64(yearid) && t.AMST_Id == Convert.ToInt64(studentid) && t.FMA_Id == groupwisefmaids[s].FMA_Id && t.FSS_ActiveFlag == true).FirstOrDefault();


                                obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + groupwisefmaids[s].FSS_ToBePaid;

                                //added on 11-07-2018
                                var fineheadss = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                  from b in _FeeGroupHeadContext.FeeHeadDMO
                                                  where (a.MI_Id == Convert.ToInt64(miid) && a.FMA_Id == groupwisefmaids[s].FMA_Id && a.ASMAY_Id == Convert.ToInt64(yearid) && b.FMH_Flag == "F" && a.AMST_Id == Convert.ToInt64(studentid) && a.FMH_Id == b.FMH_Id)
                                                  select new FeeStudentTransactionDTO
                                                  {
                                                      FMH_Id = a.FMH_Id,
                                                  }
                                   ).Distinct().Take(1);

                                if (fineheadss.Count() > 0)
                                {
                                    obj_status_stf.FSS_FineAmount = obj_status_stf.FSS_FineAmount + groupwisefmaids[s].FSS_ToBePaid;
                                }
                                //added on 11-07-2018


                                if (obj_status_stf.FSS_NetAmount != 0)
                                {
                                    //obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - groupwisefmaids[s].FSS_ToBePaid;
                                    if (obj_status_stf.FSS_ToBePaid - groupwisefmaids[s].FSS_ToBePaid >= 0)
                                    {
                                        obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - groupwisefmaids[s].FSS_ToBePaid;
                                    }
                                    else
                                    {
                                        obj_status_stf.FSS_ToBePaid = 0;
                                        obj_status_stf.FSS_ExcessPaidAmount = groupwisefmaids[s].FSS_ToBePaid - obj_status_stf.FSS_ToBePaid;
                                        obj_status_stf.FSS_RunningExcessAmount = groupwisefmaids[s].FSS_ToBePaid - obj_status_stf.FSS_ToBePaid;
                                    }
                                }
                                else
                                {
                                    obj_status_stf.FSS_ToBePaid = 0;
                                }

                                _FeeGroupHeadContext.Update(obj_status_stf);

                            }
                        }

                        groupidss = "0";
                    }

                }

                using (var dbCtxTxn = _FeeGroupHeadContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _FeeGroupHeadContext.SaveChanges();
                        dbCtxTxn.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            try
            {
                var config = _FeeGroupHeadContext.FeeMasterConfiguration.Where(t => t.MI_Id == Convert.ToInt64(miid) && t.FMC_ReadmitFineCalculationFlg == 1).ToList();

                if (config.Count > 0)
                {
                    _FeeGroupHeadContext.Database.ExecuteSqlCommand("ReadmissionFeeInsertStthomasStudentwise @p0,@p1", Convert.ToInt64(miid), Convert.ToInt64(studentid));
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return contactexisttransaction.ToString();
        }

        public long getacademicyearcongig(FeeStudentTransactionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //var currasmayid = (from a in _FeeGroupHeadContext.AcademicYear
                //                   where (a.MI_Id == data.MI_Id && indianTime > a.ASMAY_From_Date && indianTime < a.ASMAY_To_Date)
                //                   select new FeeStudentTransactionDTO
                //                   {
                //                       ASMAY_Id = a.ASMAY_Id,
                //                   }
                //       ).FirstOrDefault();

                var currasmayid = _FeeGroupHeadContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(indianTime) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(indianTime)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var readterms = (from a in _FeeGroupHeadContext.feemastersettings
                                 where (a.MI_Id == data.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMC_Online_Payment_Aca_Yr_Flag = a.FMC_Online_Payment_Aca_Yr_Flag,
                                 }
                                ).FirstOrDefault();

                if (readterms.FMC_Online_Payment_Aca_Yr_Flag == "C")
                {
                    data.ASMAY_Id = Convert.ToInt32(currasmayid);
                    //data.ASMAY_Id = data.ASMAY_Id;
                }
                else if (readterms.FMC_Online_Payment_Aca_Yr_Flag == "N")
                {
                    var newASMAY_Id = (from a in _FeeGroupHeadContext.AcademicYear
                                       where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           ASMAY_Order = a.ASMAY_Order,
                                       }
                                 ).FirstOrDefault();
                    var asmayorder = Convert.ToInt64(newASMAY_Id.ASMAY_Order) + 1;

                    var nextASMAY_Id = (from a in _FeeGroupHeadContext.AcademicYear
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Order == asmayorder)
                                        select new FeeStudentTransactionDTO
                                        {
                                            ASMAY_Id = a.ASMAY_Id,
                                        }
                                 ).FirstOrDefault();

                    data.ASMAY_Id = nextASMAY_Id.ASMAY_Id;
                }
                else if (readterms.FMC_Online_Payment_Aca_Yr_Flag == "B")
                {
                    data.ASMAY_Id = data.ASMAY_Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.ASMAY_Id;
        }

        public long getcurrentclass(FeeStudentTransactionDTO data)
        {
            try
            {

                var readterms = (from a in _FeeGroupHeadContext.feemastersettings
                                 where (a.MI_Id == data.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMC_Online_Payment_Aca_Yr_Flag = a.FMC_Online_Payment_Aca_Yr_Flag,
                                 }
                                 ).FirstOrDefault();

                if (readterms.FMC_Online_Payment_Aca_Yr_Flag == "N")
                {
                    var newclassorder = (from a in _FeeGroupHeadContext.School_M_Class
                                         where (a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && a.ASMCL_Id == data.ASMCL_ID)
                                         select new FeeStudentTransactionDTO
                                         {
                                             ASMCL_Order = a.ASMCL_Order
                                         }
                                 ).FirstOrDefault();
                    var clasorder = Convert.ToInt64(newclassorder.ASMCL_Order) + 1;

                    var nextasmclid = (from a in _FeeGroupHeadContext.School_M_Class
                                       where (a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && a.ASMCL_Order == clasorder)
                                       select new FeeStudentTransactionDTO
                                       {
                                           ASMCL_ID = a.ASMCL_Id,
                                       }
                                 ).FirstOrDefault();

                    data.ASMCL_ID = nextasmclid.ASMCL_ID;
                }
                else if (readterms.FMC_Online_Payment_Aca_Yr_Flag == "C")
                {
                    data.ASMCL_ID = data.ASMCL_ID;
                }
                else if (readterms.FMC_Online_Payment_Aca_Yr_Flag == "B")
                {
                    var checkacayr = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                      where (a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                      select new FeeStudentTransactionDTO
                                      {
                                          ASMCL_ID = a.ASMCL_Id,
                                      }
                       ).Distinct().ToArray();
                    if (checkacayr.Length == 0)
                    {
                        var newclassorder = (from a in _FeeGroupHeadContext.School_M_Class
                                             where (a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && a.ASMCL_Id == data.ASMCL_ID)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 ASMCL_Order = a.ASMCL_Order
                                             }
                                ).FirstOrDefault();
                        var clasorder1 = Convert.ToInt64(newclassorder.ASMCL_Order) + 1;

                        var nextasmclid = (from a in _FeeGroupHeadContext.School_M_Class
                                           where (a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && a.ASMCL_Order == clasorder1)
                                           select new FeeStudentTransactionDTO
                                           {
                                               ASMCL_ID = a.ASMCL_Id,
                                           }
                                     ).FirstOrDefault();

                        data.ASMCL_ID = nextasmclid.ASMCL_ID;
                    }
                    else if (checkacayr.Length > 0)
                    {
                        data.ASMCL_ID = checkacayr[0].ASMCL_ID;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.ASMCL_ID;
        }

        public void OnlineTransactionupdate(FeeStudentTransactionDTO data)
        {
            //string result1 = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");
            //string result2 = DateTime.Now.Date.AddDays(-1).ToString("yyyy-MM-dd");


            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            indianTime = indianTime.AddDays(-1);

            //string result1 = DateTime.Now.Date.ToString("yyyy-MM-dd");

            try
            {

                List<FeeStudentTransactionDTO> transupdate = new List<FeeStudentTransactionDTO>();

                List<FeeStudentTransactionDTO> transdetails = new List<FeeStudentTransactionDTO>();

                transupdate = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                               where (a.MI_Id == data.MI_Id && (a.FMOT_Date.ToShortDateString() == indianTime.Date.ToShortDateString()) && a.AMST_Id > 0)
                               select new FeeStudentTransactionDTO
                               {
                                   fyp_transaction_id = a.FMOT_Trans_Id,
                                   ASMAY_Id = a.ASMAY_ID
                               }
                             ).OrderBy(t => t.Amst_Id).ToList();

                long ASMAY_ID = transupdate.FirstOrDefault().ASMAY_Id;

                transdetails = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                where (a.MI_Id == data.MI_Id)
                                select new FeeStudentTransactionDTO
                                {
                                    FPGD_AuthorisationKey = a.FPGD_AuthorisationKey,
                                    FPGD_AuthorizationHeader = a.FPGD_AuthorizationHeader
                                }
                               ).Distinct().ToList();

                // string header = transdetails.FirstOrDefault().FPGD_AuthorizationHeader;
                string header = transdetails.FirstOrDefault().FPGD_AuthorizationHeader;
                string id = transdetails.FirstOrDefault().FPGD_AuthorisationKey;

                string transid = "";
                string description = "";
                string value = "";
                string amt = "";



                foreach (var x in transupdate)
                {
                    transid = x.fyp_transaction_id.ToString();

                    List<FeeStudentTransactionDTO> fyp_id = new List<FeeStudentTransactionDTO>();

                    transupdate = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                                   where (a.MI_Id == data.MI_Id && a.fyp_transaction_id == transid && a.ASMAY_ID == ASMAY_ID)
                                   select new FeeStudentTransactionDTO
                                   {
                                       FYP_Id = a.FYP_Id,
                                   }).Distinct().ToList();

                    if (transupdate.Count == 0)

                    {

                        try
                        {

                            var values = new Dictionary<string, string>
                        {
                        { "authorization", header },
                        };

                            string url = "https://www.payumoney.com/payment/payment/chkMerchantTxnStatus?merchantKey=id&merchantTransactionIds=TRANS_ID";
                            url = url.Replace("TRANS_ID", transid);
                            url = url.Replace("id", id);
                            WebRequest request1 = (HttpWebRequest)WebRequest.Create(url);
                            string postData = "This is a test that posts this string to a Web server.";
                            request1.Method = "POST";
                            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                            request1.Headers["authorization"] = header;
                            var response = request1.GetResponseAsync().Result;
                            //if (response.ContentLength > 0)
                            //{
                            string contentType = response.ContentType;
                            Stream content = response.GetResponseStream();
                            if (content != null)
                            {
                                StreamReader readStream = new StreamReader(content, Encoding.UTF8);
                                string responseparameters = readStream.ReadToEnd();
                                var myContent = JsonConvert.SerializeObject(responseparameters);

                                //StreamReader contentReader = new StreamReader(content, Encoding.UTF8);
                                //string responseparameters = contentReader.ReadToEnd();

                                JObject joResponse = JObject.Parse(responseparameters);
                                JArray array = (JArray)joResponse["result"];

                                foreach (JObject root in array)
                                {
                                    description = (String)root["status"];
                                    value = (String)root["paymentId"];
                                    amt = (String)root["amount"];
                                }
                            }
                            //  }

                            if ((description == "Money Settled" || description == "Settlement in process"))
                            {

                                string fmt_id = "0";

                                List<FeeStudentTransactionDTO> feeconfiglist = new List<FeeStudentTransactionDTO>();

                                List<FeeStudentTransactionDTO> feeterm = new List<FeeStudentTransactionDTO>();


                                feeconfiglist = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                                 from b in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                                 where (a.MI_Id == data.MI_Id && a.AMST_Id == b.AMST_Id && a.FMOT_Trans_Id == transid && b.ASMAY_Id == ASMAY_ID && a.ASMAY_ID == ASMAY_ID)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     Amst_Id = b.AMST_Id,
                                                     ASMCL_ID = b.ASMCL_Id
                                                 }
                                  ).Distinct().ToList();

                                long Amst_Id = feeconfiglist.FirstOrDefault().Amst_Id;
                                long Asmcl_Id = feeconfiglist.FirstOrDefault().ASMCL_ID;


                                feeterm = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                           from b in _FeeGroupHeadContext.Fee_T_Online_TransactionDMO
                                           from c in _FeeGroupHeadContext.FeeAmountEntryDMO
                                           from d in _FeeGroupHeadContext.feeMTH
                                           where (a.FMOT_Id == b.FMOT_Id && b.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == d.FTI_Id && c.ASMAY_Id == ASMAY_ID && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.FMOT_Trans_Id == transid && a.AMST_Id == Amst_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMT_Id = d.FMT_Id
                                           }
                                 ).Distinct().ToList();

                                foreach (var f in feeterm)
                                {
                                    fmt_id = fmt_id + "," + f.FMT_Id;
                                }

                                //fmt_id = fmt_id.Substring(0, (fmt_id.Length - 1));

                                insertdatainfeetables(data.MI_Id.ToString(), fmt_id, Amst_Id.ToString(), Asmcl_Id.ToString(), Convert.ToDecimal(amt), transid, value, ASMAY_ID.ToString(), indianTime, "0");
                            }
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

        }

        public FeeStudentTransactionDTO paymentPartbilldesk(FeeStudentTransactionDTO enq, long totamount)
        {

            long yrid = getacademicyearcongig(enq);

            enq.ASMAY_Id = yrid;

            long clsid = getcurrentclass(enq);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();

            long fineamt = 0, totalfine = 0;
            try
            {
                Payment pay = new Payment(_context);
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;

                string termids = "0", grpidss = "0";

                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == enq.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                             ).FirstOrDefault();

                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                for (int l = 0; l < enq.selected_list.Length; l++)
                {
                    totalfine = 0;
                    string fpgdids = "";
                    List<long> trm_ids = new List<long>();
                    foreach (var x in enq.selected_list[l].trm_list)
                    {
                        termids = termids + ',' + x.FMT_Id;
                        trm_ids.Add(x.FMT_Id);
                    }

                    enq.ftiidss = termids;

                    var grp_ids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id);
                    var grp_ids1 = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id).ToArray();
                    fineamt = 0;
                    for (int r = 0; r < grp_ids1.Count(); r++)
                    {
                        grpidss = grpidss + ',' + grp_ids1[r];
                    }

                    enq.grpidss = grpidss;

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();
                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }
                    else
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID && trm_ids.Contains(a.fmt_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();

                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }

                    List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                    List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }


                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }


                    List<FeeStudentTransactionDTO> list123 = new List<FeeStudentTransactionDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list123 = list;
                    }
                    else
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list123 = list;

                    }



                    //DateTime currdt = DateTime.Now;
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupHeadContext.AdmissionStudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.AMST_Id == enq.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                    select new FeeStudentTransactionDTO
                                    {
                                        AMST_ECSFlag = b.AMST_ECSFlag
                                    }
).Distinct().ToArray();

                    int ecsflag = 0;
                    for (int s = 0; s < fetchecs.Count(); s++)
                    {
                        if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                        {
                            ecsflag = 0;
                        }
                        else
                        {
                            ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                        }
                    }


                    foreach (var x in list123)
                    {
                        amount_list.Add(x);

                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            //cmd.CommandText = "Sp_Calculate_Fine";
                            if (ecsflag.Equals(0))
                            {
                                cmd.CommandText = "Sp_Calculate_Fine";
                            }
                            else
                            {
                                cmd.CommandText = "Sp_Calculate_Fine_ECS";
                            }

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime, 100)
                            {
                                //Value = currdt
                                Value = indianTime
                            });

                            cmd.Parameters.Add(new SqlParameter("@fma_id",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = x.FMA_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = enq.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Decimal, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                 where (a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == enq.Amst_Id)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                     FSWO_FineFlg = a.FSWO_FineFlg,
                                                     FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                 }
            ).Distinct().ToList();

                            if (waivedofffine.Count() > 0)
                            {
                                if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = 0;
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                {
                                    fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    totalfine = totalfine + fineamt;
                                }
                            }

                        }

                    }

                    if (Convert.ToInt32(fineamt) > 0)
                    {
                        if (fineheadfmaid.Count() > 0)
                        {
                            finelst.Add(new FeeStudentTransactionDTO
                            {
                                FMA_Id = fineheadfmaid[0].FMA_Id,
                                FSS_ToBePaid = Convert.ToInt32(fineamt),
                                FSS_FineAmount = 0,
                                FSS_ConcessionAmount = 0,
                                FSS_WaivedAmount = 0,
                                FMG_Id = 0,
                                FMH_Id = 0,
                                FTI_Id = 0,
                                FSS_PaidAmount = 0,
                                FSS_NetAmount = 0,
                                FSS_RefundAmount = 0,
                                FMH_FeeName = "Fine",
                                FTI_Name = "Anytime",
                                FSS_CurrentYrCharges = 0,
                                FSS_TotalToBePaid = 0,
                                merchantid = fineheadfmaid[0].merchantid,
                                FYP_Id = 0
                            });
                        }

                    }

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }
                    else
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }


                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (Convert.ToInt32(enq.pendingamount) + fineamt).ToString(),
                        //value = (Convert.ToInt32(enq.pendingamount) + totalfine).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });
                }

                enq.amount_list = amount_list.ToArray();

                if (finelst.Count() > 0)
                {
                    foreach (var y in finelst)
                    {
                        amount_list.Add(y);
                    }
                }


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                //var receipt = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_Receipt_No);

                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                //if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                //{
                //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                //    enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                //    enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                //    PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
                //}

                //PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
                //enq.trans_id = PaymentDetailsDto.trans_id.ToString();

                PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }

                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                PaymentDetailsDto.productinfo = payinfo;
                PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount.ToString());

                PaymentDetailsDto.firstname = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).AMST_FirstName + ' ' + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).AMST_MiddleName + ' ' + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).AMST_LastName.Trim();
                PaymentDetailsDto.email = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim();

                PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().FPGD_SaltKey.Trim();
                PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().FPGD_URL.Trim();
                PaymentDetailsDto.phone = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile;
                PaymentDetailsDto.udf1 = enq.ASMAY_Id.ToString().Trim();
                PaymentDetailsDto.udf2 = Convert.ToString(enq.Amst_Id);
                PaymentDetailsDto.udf3 = enq.MI_Id.ToString();

                PaymentDetailsDto.udf4 = enq.ftiidss.ToString();

                PaymentDetailsDto.udf6 = enq.grpidss.ToString().Trim();

                PaymentDetailsDto.udf5 = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString();


                //Creating XML Split info for Bill Desk Payment Gateway code starts here. 
                try
                {
                    string msg = "";
                    string totalPayable = "";
                    var tdyDate = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string PGCUSTOMERID = "PGCUST" + DateTime.Now.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (DateTime.Now.Millisecond));  //Merchant’s MAIN Unique Txn Reference Number/ Order Number [Alphanumeric; min length 1; max length 50; no special characters].
                    enq.trans_id = PGCUSTOMERID;

                    var decimalPoint = totpayableamount.ToString().Contains(".");
                    if (decimalPoint == true)
                    {
                        totalPayable = Math.Round(Convert.ToDecimal(totpayableamount), 2).ToString();
                    }
                    else
                    {
                        totalPayable = totpayableamount.ToString("0.00");
                    }


                    string xmlRequest = "<TXNDATA>" +
                    "<TXNSUMMARY>" +
                    "<REQID>PGECOM201</REQID>" +
                    "<PGMERCID>" + result.FirstOrDefault().Merchant.Trim() + "</PGMERCID>" +
                    "<RECORDS>" + result.Count + "</RECORDS>" +
                    "<PGCUSTOMERID>" + PGCUSTOMERID.Trim() + "</PGCUSTOMERID>" +
                    "<AMOUNT>" + totalPayable.Trim() + "</AMOUNT>" +
                    "<TXNDATE>" + tdyDate.Trim() + "</TXNDATE>" +
                    "</TXNSUMMARY>";
                    //CUSTOMERID :-  Txn ID – must be  unique for each child transaction.(can never be repeated for any other transaction).

                    for (int i = 0; i < result.Count; i++)
                    {
                        xmlRequest += "<RECORD ID=\"" + (i + 1) + "\">" +
                       "<MERCID>" + result[i].merchantId.Trim() + "</MERCID>" +
                       "<AMOUNT>" + result[i].splitAmount.Trim() + "</AMOUNT>" +
                       "<CUSTOMERID>" + "CUST" + i + DateTime.Now.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (DateTime.Now.Millisecond)).Trim() + "</CUSTOMERID>" +
                       "<ADDITIONALINFO1>" + PaymentDetailsDto.phone.ToString().Trim() + "</ADDITIONALINFO1>" +
                       "<ADDITIONALINFO2>" + PaymentDetailsDto.email.Trim() + "</ADDITIONALINFO2>" +
                       "<ADDITIONALINFO3>" + PaymentDetailsDto.firstname.Trim() + "</ADDITIONALINFO3>" +
                       "<ADDITIONALINFO4>" + PaymentDetailsDto.amount.ToString("0.00").Trim() + "</ADDITIONALINFO4>" +
                       "<ADDITIONALINFO5>" + PaymentDetailsDto.udf1.Trim() + "</ADDITIONALINFO5>" +
                       "<ADDITIONALINFO6>" + PaymentDetailsDto.udf2.Trim() + "</ADDITIONALINFO6>" +
                       "<ADDITIONALINFO7>" + PaymentDetailsDto.udf3.Trim() + "</ADDITIONALINFO7>" +
                       "<FILLER1>" + PaymentDetailsDto.udf4.Trim() + "</FILLER1>" +
                       "<FILLER2>" + PaymentDetailsDto.udf5.Trim() + "</FILLER2>" +
                       "<FILLER3>" + PaymentDetailsDto.udf6.Trim() + "</FILLER3>" +
                       "</RECORD>";
                    }

                    msg = xmlRequest + "</TXNDATA>";

                    string hash = string.Empty;
                    try
                    {
                        UTF8Encoding encoder = new UTF8Encoding();

                        byte[] hashValue;
                        byte[] keybyt = encoder.GetBytes(result.FirstOrDefault().CommonKey);
                        byte[] message = encoder.GetBytes(msg);

                        HMACSHA256 hashString = new HMACSHA256(keybyt);
                        string hex = "";

                        hashValue = hashString.ComputeHash(message);
                        foreach (byte x in hashValue)
                        {
                            hex += string.Format("{0:x2}", x);
                        }
                        hash = hex.ToUpper();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                    //XML Request.
                    string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" +
                        "<REQUEST>" + msg.Trim() + "<CHECKSUM>" + hash.Trim() + "</CHECKSUM>" +
                        "</REQUEST>";

                    try
                    {

                        var values = new Dictionary<string, string>();
                        values.Add("msg", xml);
                        var content = new FormUrlEncodedContent(values);
                        using (var client = new HttpClient())
                        {
                            try
                            {
                                //Posting Constructed XML Request to BillDesk Server in S2S mode and getting the XML response from BillDesk.
                                var httpResponseMessage = client.PostAsync("https://payments.billdesk.com/ecom/ECOM2ReqHandler", content).Result;

                                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                                {
                                    enq.xmlResponse = httpResponseMessage.Content.ReadAsStringAsync().Result;

                                }
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
                    XDocument doc = XDocument.Parse(enq.xmlResponse);
                    var status = doc.Descendants().Where(d => d.Name == "STATUSDESC").FirstOrDefault();

                    if (status.Value.Equals("SUCCESS"))
                    {
                        //Constructing pipe separated message.Which can be posted to BillDesk URL in B2B mode.

                        //string str = result.FirstOrDefault().merchantId.Trim() + "|" + PGCUSTOMERID.Trim() + "|NA|" + totalPayable.Trim() + "|NA|NA|NA|INR|NA|R|" + result.FirstOrDefault().Merchant.ToLower().Trim() + "|NA|NA|F|" + PaymentDetailsDto.phone.ToString().Trim() + "|" + PaymentDetailsDto.email.Trim() + "|" + PaymentDetailsDto.firstname.Trim() + "|" + PaymentDetailsDto.udf1.Trim() + "|" + PaymentDetailsDto.udf2.Trim() + "|" + PaymentDetailsDto.udf3.Trim() + "|" + PaymentDetailsDto.udf4.Trim() + "|" + PaymentDetailsDto.udf5.Trim() + "|" + PaymentDetailsDto.udf6.Trim() + "|http://localhost:57606/api/FeeOnlinePaymentStthomas/BillDeskpaymentresponse";


                        string str = result.FirstOrDefault().merchantId.Trim() + "|" + PGCUSTOMERID.Trim() + "|NA|" + totalPayable.Trim() + "|NA|NA|NA|INR|NA|R|" + result.FirstOrDefault().Merchant.ToLower().Trim() + "|NA|NA|F|" + PaymentDetailsDto.phone.ToString().Trim() + "|" + PaymentDetailsDto.email.Trim() + "|" + PaymentDetailsDto.firstname.Trim() + "|" + PaymentDetailsDto.udf1.Trim() + "|" + PaymentDetailsDto.udf2.Trim() + "|" + PaymentDetailsDto.udf3.Trim() + "|" + PaymentDetailsDto.udf4.Trim() + "|" + PaymentDetailsDto.udf5.Trim() + "|" + PaymentDetailsDto.udf6.Trim() + "http://localhost:57606/api/FeeOnlinePaymentStthomas/BillDeskpaymentresponse";


                        string hashh = string.Empty;
                        try
                        {
                            UTF8Encoding encoder = new UTF8Encoding();

                            byte[] hashValuee;
                            byte[] keybytt = encoder.GetBytes(result.FirstOrDefault().CommonKey);
                            byte[] messagee = encoder.GetBytes(str);

                            HMACSHA256 hashString = new HMACSHA256(keybytt);
                            string hexx = "";

                            hashValuee = hashString.ComputeHash(messagee);
                            foreach (byte x in hashValuee)
                            {
                                hexx += string.Format("{0:x2}", x);
                            }
                            hashh = hexx.ToUpper();
                            var pipemsg = str + "|" + hashh;
                            enq.pipeSepMsg = pipemsg;
                            enq.URL = result.FirstOrDefault().URL;
                        }

                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        //get_grp_reptno(enq);
                        get_grp_reptno_Mobile(enq);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                //XML Split info for Bill Desk code ends here. 

                // get_grp_reptno(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return enq;
        }

        public PaymentDetails.BillDeskPayment billdeskPayment(PaymentDetails.BillDeskPayment obj)
        {
            try
            {
                FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
                PaymentDetails.BillDeskPayment dto = new PaymentDetails.BillDeskPayment();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string geneval = "";
                string str = obj.responseparam;

                var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                where (a.MI_Id == Convert.ToInt32(obj.MI_Id) && a.FPGD_PGName == "BILLDESK")
                                select new FeeStudentTransactionDTO
                                {
                                    CommonKey = a.FPGD_SaltKey,
                                }
         ).Distinct().ToList();

                string hashh = string.Empty;
                try
                {
                    UTF8Encoding encoder = new UTF8Encoding();

                    byte[] hashValuee;
                    byte[] keybytt = encoder.GetBytes(fpgdids1.FirstOrDefault().CommonKey);
                    byte[] messagee = encoder.GetBytes(str);

                    HMACSHA256 hashString = new HMACSHA256(keybytt);
                    string hexx = "";

                    hashValuee = hashString.ComputeHash(messagee);
                    foreach (byte x in hashValuee)
                    {
                        hexx += string.Format("{0:x2}", x);
                    }
                    hashh = hexx.ToUpper();

                    if (hashh.Equals(obj.responsechecksumparam))
                    {
                        geneval = "Sucess";
                    }
                    else
                    {
                        geneval = "Fail";
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                if (obj.AuthStatus.Equals("0300") && geneval.Equals("Sucess"))
                {
                    var groups = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                                  where (a.MI_Id == Convert.ToInt32(obj.MI_Id) && a.ASMAY_ID == Convert.ToInt32(obj.ASMAY_Id) && a.fyp_transaction_id == obj.TxnReferenceNo)
                                  select new FeeStudentTransactionDTO
                                  {
                                      FYP_Receipt_No = a.FYP_Receipt_No
                                  }
                     ).Distinct().ToList();

                    if (groups.Count == 0)
                    {
                        var confirmstatus = insertdatainfeetables(obj.MI_Id, obj.termid, obj.AMST_Id, obj.ASMCL_ID, Convert.ToDecimal(obj.TxnAmount), obj.CustomerID, obj.TxnReferenceNo, obj.ASMAY_Id, indianTime, "0");

                        if (Convert.ToInt32(confirmstatus) > 0)
                        {
                            pgmod.MI_Id = Convert.ToInt64(obj.MI_Id);
                            pgmod.amst_mobile = Convert.ToInt64(obj.PhoneNo);
                            pgmod.Amst_Id = Convert.ToInt64(obj.AMST_Id);
                            pgmod.amst_email_id = obj.emailId;

                            SMS sms = new SMS(_context);

                            sms.sendSms(Convert.ToInt32(obj.MI_Id), Convert.ToInt64(obj.PhoneNo), "FEEONLINEPAYMENT", Convert.ToInt32(obj.AMST_Id));

                            Email Email = new Email(_context);

                            Email.sendmail(Convert.ToInt32(obj.MI_Id), obj.emailId, "FEEONLINEPAYMENT", Convert.ToInt32(obj.AMST_Id));

                        }
                    }
                }
                else
                {
                    SMS sms = new SMS(_context);

                    sms.sendSms(Convert.ToInt32(obj.MI_Id), Convert.ToInt64(obj.PhoneNo), "FEEONLINEPAYMENTFAIL", Convert.ToInt32(obj.AMST_Id));

                    Email Email = new Email(_context);

                    Email.sendmail(Convert.ToInt32(obj.MI_Id), obj.emailId, "FEEONLINEPAYMENTFAIL", Convert.ToInt32(obj.AMST_Id));

                    dto.status = obj.AuthStatus;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        public async Task<FeeStudentTransactionDTO> mobilepayuconnect(FeeStudentTransactionDTO data)
        {
            try
            {
                data.ASMAY_Id = getacademicyearcongig(data);

                data.fillstudent = (from a in _FeeGroupHeadContext.AdmissionStudentDMO
                                    from b in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                    from c in _FeeGroupHeadContext.admissioncls
                                    from d in _FeeGroupHeadContext.school_M_Section
                                    where (c.ASMCL_Id == b.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id)
                                    select new FeeStudentTransactionDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName,
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                        AMST_RegistrationNo = a.AMST_RegistrationNo,
                                        AMST_AdmNo = a.AMST_AdmNo,
                                        AMAY_RollNo = b.AMAY_RollNo,
                                        classname = c.ASMCL_ClassName,
                                        sectionname = d.ASMC_SectionName,
                                        ASMCL_ID = b.ASMCL_Id,
                                        amst_mobile = a.AMST_MobileNo,
                                        amst_email_id = a.AMST_emailId,
                                        ASYST_Id = b.ASYST_Id,
                                        AMST_PerCity = a.AMST_PerCity
                                    }
               ).OrderByDescending(t => t.ASYST_Id).ToArray();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == data.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_OnlinePaymentCompany = a.IVRMGC_OnlinePaymentCompany,
                                     IVRMGC_Classwise_Payment = a.IVRMGC_OnlinePaymentCompany
                                 }
                                 ).FirstOrDefault();

                if (data.onlinepaygteway == "PAYU")
                {
                    data.paydet = mobilepaymentPart(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "BILLDESK")
                {
                    data = paymentPartbilldesk(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "RAZORPAY")
                {
                    data.paydet = paymentPartRAZORPAY(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "PAYTM")
                {
                    data.paydet = paymentPartpaytmmobile(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "EASEBUZZ" && data.FYP_PayModeType == "Portal")
                {
                    //data = paymentPartEasyBuzz(data);
                    data = paymentPartEasyBuzzSplit(data);
                }
                else if (data.onlinepaygteway == "EASEBUZZ" && data.FYP_PayModeType == "MOBILE")
                {
                    //data = paymentPartEasyBuzz(data);
                    data = await paymentPartEasyBuzzMobileAsync(data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

      
        public Array mobilepaymentPart(FeeStudentTransactionDTO enq, long totamount)
        {
            long yrid = getacademicyearcongig(enq);

            enq.ASMAY_Id = yrid;

            long clsid = getcurrentclass(enq);

          

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();

            long fineamt = 0, totalfine = 0;
            try
            {
                Payment pay = new Payment(_context);
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;

                string termids = "0", grpidss = "0";

                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == enq.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                             ).FirstOrDefault();

                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                for (int l = 0; l < enq.selected_list.Length; l++)
                {
                    totalfine = 0;
                    string fpgdids = "";
                    List<long> trm_ids = new List<long>();
                    foreach (var x in enq.selected_list[l].trm_list)
                    {
                        termids = termids + ',' + x.FMT_Id;
                        trm_ids.Add(x.FMT_Id);
                    }

                    enq.ftiidss = termids;

                    var grp_ids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id);
                    var grp_ids1 = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id).ToArray();
                    fineamt = 0;
                    for (int r = 0; r < grp_ids1.Count(); r++)
                    {
                        grpidss = grpidss + ',' + grp_ids1[r];
                    }

                    enq.grpidss = grpidss;

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();
                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }
                    else
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID && trm_ids.Contains(a.fmt_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();

                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }

                    List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                    List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }


                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }


                    List<FeeStudentTransactionDTO> list123 = new List<FeeStudentTransactionDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list123 = list;
                    }
                    else
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list123 = list;

                    }


                    var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupHeadContext.AdmissionStudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.AMST_Id == enq.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                    select new FeeStudentTransactionDTO
                                    {
                                        AMST_ECSFlag = b.AMST_ECSFlag
                                    }
                   ).Distinct().ToArray();

                    int ecsflag = 0;
                    for (int s = 0; s < fetchecs.Count(); s++)
                    {
                        if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                        {
                            ecsflag = 0;
                        }
                        else
                        {
                            ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                        }
                    }

                    //DateTime currdt = DateTime.Now;
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    foreach (var x in list123)
                    {
                        amount_list.Add(x);

                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            if (ecsflag.Equals(0))
                            {
                                cmd.CommandText = "Sp_Calculate_Fine";
                            }
                            else
                            {
                                cmd.CommandText = "Sp_Calculate_Fine_ECS";
                            }

                            //cmd.CommandText = "Sp_Calculate_Fine";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime, 100)
                            {
                                //Value = currdt
                                Value = indianTime
                            });

                            cmd.Parameters.Add(new SqlParameter("@fma_id",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = x.FMA_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = enq.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Decimal, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                 where (a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == enq.Amst_Id)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                     FSWO_FineFlg = a.FSWO_FineFlg,
                                                     FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                 }
            ).Distinct().ToList();

                            if (waivedofffine.Count() > 0)
                            {
                                if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = 0;
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                {
                                    fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    totalfine = totalfine + fineamt;
                                }
                            }

                        }

                    }

                    if (Convert.ToInt32(fineamt) > 0)
                    {
                        if (fineheadfmaid.Count() > 0)
                        {
                            finelst.Add(new FeeStudentTransactionDTO
                            {
                                FMA_Id = fineheadfmaid[0].FMA_Id,
                                FSS_ToBePaid = Convert.ToInt32(fineamt),
                                FSS_FineAmount = 0,
                                FSS_ConcessionAmount = 0,
                                FSS_WaivedAmount = 0,
                                FMG_Id = 0,
                                FMH_Id = 0,
                                FTI_Id = 0,
                                FSS_PaidAmount = 0,
                                FSS_NetAmount = 0,
                                FSS_RefundAmount = 0,
                                FMH_FeeName = "Fine",
                                FTI_Name = "Anytime",
                                FSS_CurrentYrCharges = 0,
                                FSS_TotalToBePaid = 0,
                                merchantid = fineheadfmaid[0].merchantid,
                                FYP_Id = 0
                            });
                        }

                    }

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }
                    else
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }


                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (Convert.ToInt32(enq.pendingamount) + fineamt).ToString(),
                        //value = (Convert.ToInt32(enq.pendingamount) + totalfine).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });
                }

                enq.amount_list = amount_list.ToArray();

                if (finelst.Count() > 0)
                {
                    foreach (var y in finelst)
                    {
                        amount_list.Add(y);
                    }
                }


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                // paymentdetails = _FeeGroupHeadContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();
                //multiple gateway
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

                var receipt = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_Receipt_No);

                //multiple gateway
                //PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                if (enq.automanualreceiptno == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);

                    List<Master_Numbering> masnum = new List<Master_Numbering>();
                    masnum = _FeeGroupHeadContext.Master_Numbering.Where(t => t.MI_Id == enq.MI_Id && t.IMN_Flag == "OnlineRegular").ToList();

                    enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                    enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;

                    enq.transnumbconfigurationsettingsss.MI_Id = Convert.ToInt32(masnum.FirstOrDefault().MI_Id);
                    enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag = masnum.FirstOrDefault().IMN_AutoManualFlag;
                    enq.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = masnum.FirstOrDefault().IMN_DuplicatesFlag;
                    enq.transnumbconfigurationsettingsss.IMN_StartingNo = masnum.FirstOrDefault().IMN_StartingNo;
                    enq.transnumbconfigurationsettingsss.IMN_WidthNumeric = masnum.FirstOrDefault().IMN_WidthNumeric;
                    enq.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = masnum.FirstOrDefault().IMN_ZeroPrefixFlag;
                    enq.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = masnum.FirstOrDefault().IMN_PrefixAcadYearCode;
                    enq.transnumbconfigurationsettingsss.IMN_PrefixParticular = masnum.FirstOrDefault().IMN_PrefixParticular;
                    enq.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = masnum.FirstOrDefault().IMN_SuffixAcadYearCode;
                    enq.transnumbconfigurationsettingsss.IMN_SuffixParticular = masnum.FirstOrDefault().IMN_SuffixParticular;
                    enq.transnumbconfigurationsettingsss.IMN_RestartNumFlag = masnum.FirstOrDefault().IMN_RestartNumFlag;
                    enq.transnumbconfigurationsettingsss.IMN_Flag = masnum.FirstOrDefault().IMN_Flag;

                    enq.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = masnum.FirstOrDefault().IMN_PrefixFinYearCode;
                    enq.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = masnum.FirstOrDefault().IMN_PrefixCalYearCode;
                    enq.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = masnum.FirstOrDefault().IMN_SuffixFinYearCode;
                    enq.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = masnum.FirstOrDefault().IMN_SuffixCalYearCode;

                    PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
                }

                PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
                enq.trans_id = PaymentDetailsDto.trans_id.ToString();

                PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }

                //fine amount

                //        var fpgdidsfine = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                //                       from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                //                       from c in _FeeGroupHeadContext.FeeHeadDMO
                //                       where (a.fpgd_id == b.FPGD_Id && a.FMH_Id==c.FMH_Id && c.FMH_Flag=="F")
                //                       select new FeeStudentTransactionDTO
                //                       {
                //                           merchantid = b.FPGD_SubMerchantId
                //                       }
                //).Distinct().ToList();

                //        if (totalfine>0)
                //        {
                //            for(int g=0;g< finelst.Count();g++)
                //            {
                //                autoinc = autoinc + 1;
                //                result.Add(new FeeSlplitOnlinePayment
                //                {
                //                    name = "splitId" + autoinc.ToString(),
                //                    merchantId = finelst[g].merchantid.ToString(),
                //                    value = finelst[g].FSS_ToBePaid.ToString(),
                //                    commission = "0",
                //                    description = "Online Payment",
                //                });
                //            }
                //        }

                //fine amount

                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                //payinfo = "FeeOnlinePayment";

                PaymentDetailsDto.productinfo = payinfo;
                //PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount.ToString()) + Convert.ToInt32(totalfine);
                PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount.ToString());
                // PaymentDetailsDto.amount = Convert.ToDecimal(enq.topayamount);

                PaymentDetailsDto.firstname = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).AMST_FirstName + ' ' + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).AMST_MiddleName + ' ' + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).AMST_LastName.Trim();
                PaymentDetailsDto.email = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim();

                //PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT.Trim();
                //PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL.Trim();
                //multiple gateway
                PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().FPGD_SaltKey.Trim();
                PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().FPGD_URL.Trim();

                PaymentDetailsDto.phone = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile;
                PaymentDetailsDto.udf1 = enq.ASMAY_Id.ToString().Trim();
                PaymentDetailsDto.udf2 = Convert.ToString(enq.Amst_Id);
                PaymentDetailsDto.udf3 = enq.MI_Id.ToString();

                PaymentDetailsDto.udf4 = enq.ftiidss.ToString();
                // PaymentDetailsDto.udf4 = enq.ftiidss.ToString();

                PaymentDetailsDto.udf6 = enq.grpidss.ToString().Trim();

                PaymentDetailsDto.udf5 = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString();
                PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponse/";

                PaymentDetailsDto.status = "success";
                PaymentDetailsDto.service_provider = "payu_paisa";

                PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

                get_grp_reptno_Mobile(enq);

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return PaymentDetailsDto.PaymentDetailsList;
        }

        public FeeStudentTransactionDTO get_grp_reptno_Mobile(FeeStudentTransactionDTO data)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();
                string termlst = "0";
                long fmttotal = 0;

                var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                from b in _FeeGroupHeadContext.AdmissionStudentDMO
                                where (a.AMST_Id == b.AMST_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                select new FeeStudentTransactionDTO
                                {
                                    AMST_ECSFlag = b.AMST_ECSFlag
                                }
               ).Distinct().ToArray();

                int ecsflag = 0;
                for (int s = 0; s < fetchecs.Count(); s++)
                {
                    if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                    {
                        ecsflag = 0;
                    }
                    else
                    {
                        ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                    }
                }

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string onlineheadmapid = "0", groupidss = "0";
                long fineamountcal = 0, fethchfmaidsfine = 0, finecount = 0;

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();
                List<long> HeadId = new List<long>();
                List<long> termids = new List<long>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == data.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                            ).FirstOrDefault();

                List<long> fmggids = new List<long>();
                for (int l = 0; l < data.selected_list.Length; l++)
                {
                    fmggids.Clear();
                    HeadId.Clear();
                    foreach (var x in data.selected_list[l].trm_list)
                    {
                        fmggids.Add(x.FMGG_Id);
                        termids.Add(x.FMT_Id);
                        termlst = termlst + "," + x.FMT_Id;
                    }

                    var groupidsss = (from a in _FeeGroupHeadContext.feeGGG
                                      where (fmggids.Contains(a.FMGG_Id))
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id
                                      }
        ).Distinct().Select(t => t.FMG_Id).ToList();

                    List<FeeStudentTransactionDTO> list = new List<FeeStudentTransactionDTO>();
                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                from c in _FeeGroupHeadContext.FeeHeadDMO
                                from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                from e in _FeeGroupHeadContext.FeeGroupDMO
                                where (b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id && termids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && groupidsss.Contains(a.fmg_id) && b.FMG_Id == e.FMG_Id && a.fmg_id == e.FMG_Id)
                                select new FeeStudentTransactionDTO
                                {
                                    FMT_Id = a.fmt_id,
                                    FMH_Id = b.FMH_Id,
                                }
              ).OrderBy(t => t.FTI_Id).ToList();
                    }
                    else
                    {
                        list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                from c in _FeeGroupHeadContext.FeeHeadDMO
                                from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                from e in _FeeGroupHeadContext.FeeGroupDMO
                                    // from e in _FeeGroupHeadContext.feeGGG
                                where (b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id && termids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && groupidsss.Contains(a.fmg_id) && b.FMG_Id == e.FMG_Id && a.ASMCL_Id == data.ASMCL_ID)
                                select new FeeStudentTransactionDTO
                                {
                                    FMH_Id = b.FMH_Id,
                                    FMT_Id = a.fmt_id
                                }
             ).OrderBy(t => t.FTI_Id).ToList();
                    }

                    foreach (var item in list)
                    {
                        HeadId.Add(item.FMH_Id);
                        termids.Add(item.FMT_Id);
                    }

                    List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                    grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
                            from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                            from d in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                            from e in _FeeGroupHeadContext.FeeStudentTransactionDMO
                            where (c.FGAR_Id == d.FGAR_Id && d.ASMAY_Id == b.ASMAY_Id && b.FMG_Id == c.FMG_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id) && b.FMG_Id == e.FMG_Id && e.AMST_Id == data.Amst_Id && b.FMH_Id == e.FMH_Id && e.ASMAY_Id == data.ASMAY_Id && groupidsss.Contains(e.FMG_Id))
                            select new FeeStudentTransactionDTO
                            {
                                FGAR_Id = c.FGAR_Id
                            }
                           ).Distinct().ToList();

                    for (int r = 0; r < grps.Count(); r++)
                    {
                        onlineheadmapid = grps[r].FGAR_Id.ToString();

                        List<FeeStudentTransactionDTO> grps1 = new List<FeeStudentTransactionDTO>();
                        grps1 = (from a in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                 from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                                 from c in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                 where (b.FGAR_Id == a.FGAR_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FGAR_Id == Convert.ToInt64(onlineheadmapid) && c.FMG_Id == a.FMG_Id && c.AMST_Id == data.Amst_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMG_Id = a.FMG_Id
                                 }
                         ).Distinct().ToList();

                        List<long> grpid = new List<long>();
                        foreach (var item in grps1)
                        {
                            grpid.Add(item.FMG_Id);
                        }

                        for (int d = 0; d < grps1.Count(); d++)
                        {
                            groupidss = groupidss + ',' + grps1[d].FMG_Id;
                        }

                        termids.Clear();


                        //praveen
                        foreach (var item in data.selected_list)
                        {
                            foreach (var item1 in item.trm_list)
                            {

                                var newgrplst = (from a in _FeeGroupHeadContext.feeGGG.Where(y => y.FMGG_Id == item1.FMGG_Id)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FMG_Id = a.FMG_Id
                                                 }).Distinct().ToList();


                                foreach (var fmg in newgrplst)
                                {
                                    if (grpid.Contains(fmg.FMG_Id))
                                    {
                                        termids.Add(item1.FMT_Id);
                                    }
                                }

                            }

                        }
                        if (termids.Count > 0)
                        {
                            List<long> temptermids = new List<long>();
                            temptermids = termids.Distinct().ToList();
                            termids = temptermids;
                        }

                        //praveen

                        List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                        List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();
                        list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                                    from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                    where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                    select new FeeStudentTransactionDTO
                                    {
                                        FGAR_PrefixName = b.FGAR_PrefixName,
                                        FGAR_SuffixName = b.FGAR_SuffixName,
                                        FGAR_Id = c.FGAR_Id,
                                    }
                             ).Distinct().ToList();

                        //List<FeeStudentTransactionDTO> groupwiseamount = new List<FeeStudentTransactionDTO>();
                        decimal groupwiseamount = 0;
                        if (readterms.IVRMGC_Classwise_Payment != "1")
                        {
                            groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                               from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                               from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (a.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && i.FSS_ToBePaid > 0 && termids.Contains(a.fmt_id) && i.AMST_Id == data.Amst_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid)
                                               }
      ).Sum(t => t.FSS_ToBePaid);

                        }
                        else
                        {
                            groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                               from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                               from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (a.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && i.FSS_ToBePaid > 0 && termids.Contains(a.fmt_id) && i.AMST_Id == data.Amst_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && a.ASMCL_Id == data.ASMCL_ID && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid)
                                               }
      ).Sum(t => t.FSS_ToBePaid);

                        }

                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "receiptnogeneration";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mi_id",
                                SqlDbType.VarChar, 100)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@asmayid",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@fmgid",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = groupidss
                            });

                            cmd.Parameters.Add(new SqlParameter("@receiptno",
                SqlDbType.NVarChar, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

                            var rece_amt = new { receiptno = data.FYP_Receipt_No, amt = groupwiseamount };

                            dynamicrecgen.Add(rece_amt);
                            List<FeeStudentTransactionDTO> groupwisefmaidsnew = new List<FeeStudentTransactionDTO>();


                            List<FeeStudentTransactionDTO> groupwisefmaids = new List<FeeStudentTransactionDTO>();
                            if (readterms.IVRMGC_Classwise_Payment != "1")
                            {
                                groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                   from c in _FeeGroupHeadContext.FeeHeadDMO
                                                   from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                   from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                   where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && termids.Contains(a.fmt_id) && ((i.FSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMST_Id == data.Amst_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway)
                                                   select new FeeStudentTransactionDTO
                                                   {
                                                       FMA_Id = i.FMA_Id,
                                                       FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                                                       FMH_Flag = c.FMH_Flag,
                                                       FSS_RebateAmount = Convert.ToInt64(i.FSS_CurrentYrCharges)
                                                   }
                                ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                                groupwisefmaidsnew = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                      from c in _FeeGroupHeadContext.FeeHeadDMO
                                                      from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                      from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                      from e in _FeeGroupHeadContext.FeeGroupDMO
                                                      where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && termids.Contains(a.fmt_id) && ((i.FSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMST_Id == data.Amst_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway && i.FMG_Id == e.FMG_Id && e.FMG_CompulsoryFlag != "T")
                                                      select new FeeStudentTransactionDTO
                                                      {
                                                          FMA_Id = i.FMA_Id,
                                                          FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                                                          FMH_Flag = c.FMH_Flag,
                                                          FSS_RebateAmount = Convert.ToInt64(i.FSS_CurrentYrCharges)
                                                      }
        ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                            }
                            else
                            {
                                groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                   from c in _FeeGroupHeadContext.FeeHeadDMO
                                                   from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                   from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                   where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && termids.Contains(a.fmt_id) && ((i.FSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMST_Id == data.Amst_Id && a.ASMCL_Id == data.ASMCL_ID && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway)
                                                   select new FeeStudentTransactionDTO
                                                   {
                                                       FMA_Id = i.FMA_Id,
                                                       FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                                                       FMH_Flag = c.FMH_Flag,
                                                       FSS_RebateAmount = Convert.ToInt64(i.FSS_CurrentYrCharges)
                                                   }
                                ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                                groupwisefmaidsnew = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                      from c in _FeeGroupHeadContext.FeeHeadDMO
                                                      from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                                      from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                      from e in _FeeGroupHeadContext.FeeGroupDMO
                                                      where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && termids.Contains(a.fmt_id) && ((i.FSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMST_Id == data.Amst_Id && a.ASMCL_Id == data.ASMCL_ID && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway && i.FMG_Id == e.FMG_Id && e.FMG_CompulsoryFlag != "T")
                                                      select new FeeStudentTransactionDTO
                                                      {
                                                          FMA_Id = i.FMA_Id,
                                                          FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                                                          FMH_Flag = c.FMH_Flag,
                                                          FSS_RebateAmount = Convert.ToInt64(i.FSS_CurrentYrCharges)
                                                      }
                               ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                            }

                            var cnt1 = _context.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id && t.FMC_RebateAplicableFlg == true && t.FMC_RebateAgainstFullPaymentFlg == true && t.FMC_RebateAgainstPartialPaymentFlg == true).ToList();

                            for (var i = 0; i < groupwisefmaidsnew.Count; i++)
                            {
                                fmttotal = fmttotal + groupwisefmaidsnew[i].FSS_RebateAmount;
                            }
                            data.FSS_RebateAmount = 0;

                            string rebateamount;
                            if (groupwisefmaidsnew.Count > 0)
                            {
                                if (cnt1.Count > 0)
                                {
                                    using (var cmd4 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd4.CommandText = "Fee_RebateTermWise_calculation_BOTH";
                                        cmd4.CommandType = CommandType.StoredProcedure;
                                        cmd4.Parameters.Add(new SqlParameter("@MI_Id",
                                                 SqlDbType.VarChar)
                                        {
                                            Value = data.MI_Id
                                        });

                                        cmd4.Parameters.Add(new SqlParameter("@ASMAY_ID",
                                           SqlDbType.VarChar)
                                        {
                                            Value = data.ASMAY_Id
                                        });
                                        cmd4.Parameters.Add(new SqlParameter("@AMST_ID",
                                       SqlDbType.VarChar)
                                        {
                                            Value = data.Amst_Id
                                        });
                                        cmd4.Parameters.Add(new SqlParameter("@FMT_ID",
                                      SqlDbType.VarChar)
                                        {
                                            Value = termlst
                                        });
                                        cmd4.Parameters.Add(new SqlParameter("@paiddate",
                                      SqlDbType.DateTime)
                                        {
                                            Value = DateTime.Now
                                        });
                                        cmd4.Parameters.Add(new SqlParameter("@paidamount",
                                       SqlDbType.VarChar)
                                        {
                                            Value = fmttotal
                                        });

                                        cmd4.Parameters.Add(new SqlParameter("@USERID",
                                       SqlDbType.VarChar)
                                        {
                                            Value = data.userid
                                        });

                                        cmd4.Parameters.Add(new SqlParameter("@totalrebateamount",
                            SqlDbType.BigInt)
                                        {
                                            Direction = ParameterDirection.Output
                                        });
                                        if (cmd4.Connection.State != ConnectionState.Open)
                                            cmd4.Connection.Open();
                                        var data4 = cmd4.ExecuteNonQuery();
                                        rebateamount = cmd4.Parameters["@totalrebateamount"].Value.ToString();
                                    }



                                }
                                else
                                {
                                    using (var cmd5 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd5.CommandText = "Fee_RebateTermWise_calculation";
                                        cmd5.CommandType = CommandType.StoredProcedure;
                                        cmd5.Parameters.Add(new SqlParameter("@MI_Id",
                                                 SqlDbType.VarChar)
                                        {
                                            Value = data.MI_Id
                                        });

                                        cmd5.Parameters.Add(new SqlParameter("@ASMAY_ID",
                                           SqlDbType.VarChar)
                                        {
                                            Value = data.ASMAY_Id
                                        });
                                        cmd5.Parameters.Add(new SqlParameter("@AMST_ID",
                                       SqlDbType.VarChar)
                                        {
                                            Value = data.Amst_Id
                                        });
                                        cmd5.Parameters.Add(new SqlParameter("@FMT_ID",
                                      SqlDbType.VarChar)
                                        {
                                            Value = termlst
                                        });
                                        cmd5.Parameters.Add(new SqlParameter("@paiddate",
                                      SqlDbType.DateTime)
                                        {
                                            Value = DateTime.Now
                                        });
                                        cmd5.Parameters.Add(new SqlParameter("@paidamount",
                                       SqlDbType.VarChar)
                                        {
                                            Value = fmttotal
                                        });

                                        cmd5.Parameters.Add(new SqlParameter("@USERID",
                                       SqlDbType.VarChar)
                                        {
                                            Value = data.userid
                                        });

                                        cmd5.Parameters.Add(new SqlParameter("@totalrebateamount",
                            SqlDbType.BigInt)
                                        {
                                            Direction = ParameterDirection.Output
                                        });
                                        if (cmd5.Connection.State != ConnectionState.Open)
                                            cmd5.Connection.Open();
                                        var data3 = cmd5.ExecuteNonQuery();
                                        rebateamount = cmd5.Parameters["@totalrebateamount"].Value.ToString();
                                    }

                                }

                                data.FSS_RebateAmount = Convert.ToInt64(rebateamount);

                                fmttotal = 0;
                                rebateamount = "";
                            }
                            Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                            onlinemtrans.FMOT_Trans_Id = data.trans_id;
                            //onlinemtrans.FMOT_Amount = data.topayamount;
                            onlinemtrans.FMOT_Amount = rece_amt.amt;
                            onlinemtrans.FMOT_Date = indianTime;
                            onlinemtrans.FMOT_Flag = "P";
                            onlinemtrans.PASR_Id = 0;
                            onlinemtrans.AMST_Id = data.Amst_Id;
                            onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;
                            onlinemtrans.FYP_PayModeType = data.FYP_PayModeType;
                            onlinemtrans.FMOT_PayGatewayType = data.onlinepaygteway;

                            onlinemtrans.MI_Id = data.MI_Id;
                            onlinemtrans.ASMAY_ID = data.ASMAY_Id;

                            _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                            

                            for (var i = 0; i < groupwisefmaids.Count; i++)
                            {
                                for (var k = 0; k < groupwisefmaids.Count - 1; k++)
                                {
                                    if (groupwisefmaids[k].FSS_ToBePaid < groupwisefmaids[k + 1].FSS_ToBePaid)
                                    {
                                        var temp = groupwisefmaids[k];
                                        groupwisefmaids[k] = groupwisefmaids[k + 1];
                                        groupwisefmaids[k + 1] = temp;
                                    }
                                }
                            }



                            for (var j = 0; j < groupwisefmaids.Count; j++)
                            {
                                //FeeTransactionPaymentDMO feetrapay = new FeeTransactionPaymentDMO();


                                if (j == 0)
                                {

                                    if (groupwisefmaids[j].FMH_Flag == "F")
                                    {
                                        fethchfmaidsfine = groupwisefmaids[j].FMA_Id;
                                    }
                                    else
                                    {
                                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                        onlinettrans.FMA_Id = groupwisefmaids[j].FMA_Id;
                                        onlinettrans.FTOT_Amount = groupwisefmaids[j].FSS_ToBePaid - data.FSS_RebateAmount;
                                        onlinettrans.FTOT_Created_date = indianTime;
                                        onlinettrans.FTOT_Updated_date = indianTime;
                                        onlinettrans.FTOT_Concession = 0;
                                        onlinettrans.FTOT_Fine = 0;
                                        onlinettrans.FTOT_RebateAmount = data.FSS_RebateAmount;

                                        using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                        {
                                            if (ecsflag.Equals(0))
                                            {
                                                cmd1.CommandText = "Sp_Calculate_Fine";
                                            }
                                            else
                                            {
                                                cmd1.CommandText = "Sp_Calculate_Fine_ECS";
                                            }

                                            //cmd1.CommandText = "Sp_Calculate_Fine";
                                            cmd1.CommandType = CommandType.StoredProcedure;
                                            cmd1.Parameters.Add(new SqlParameter("@On_Date",
                                                SqlDbType.DateTime, 100)
                                            {
                                                //Value = curdt
                                                Value = indianTime
                                            });

                                            cmd1.Parameters.Add(new SqlParameter("@fma_id",
                                               SqlDbType.NVarChar, 100)
                                            {
                                                Value = groupwisefmaids[j].FMA_Id
                                            });
                                            cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                                           SqlDbType.NVarChar, 100)
                                            {
                                                Value = data.ASMAY_Id
                                            });

                                            cmd1.Parameters.Add(new SqlParameter("@amt",
                                SqlDbType.Decimal, 500)
                                            {
                                                Direction = ParameterDirection.Output
                                            });

                                            cmd1.Parameters.Add(new SqlParameter("@flgArr",
                               SqlDbType.Int, 500)
                                            {
                                                Direction = ParameterDirection.Output
                                            });

                                            if (cmd1.Connection.State != ConnectionState.Open)
                                                cmd1.Connection.Open();

                                            var execfine = cmd1.ExecuteNonQuery();

                                            //finewaiedoff

                                            var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                                 where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMA_Id == groupwisefmaids[j].FMA_Id && a.AMST_Id == data.Amst_Id)
                                                                 select new FeeStudentTransactionDTO
                                                                 {
                                                                     FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                                     FSWO_FineFlg = a.FSWO_FineFlg,
                                                                     FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                                 }
                   ).Distinct().ToList();

                                            if (waivedofffine.Count() > 0)
                                            {
                                                if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                                {
                                                    if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                    {
                                                        fineamountcal = fineamountcal + (Convert.ToInt32(cmd1.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount));
                                                    }
                                                }
                                                else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                                {
                                                    if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                    {
                                                        fineamountcal = fineamountcal + 0;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                    {
                                                        fineamountcal = fineamountcal + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    fineamountcal = fineamountcal + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                }
                                            }

                                          
                                        }

                                        _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                    }




                                    if (groupwisefmaids[j].FMH_Flag == "F")
                                    {
                                        if (finecount < 1)
                                        {
                                            finecount = finecount + 1;
                                            fethchfmaidsfine = groupwisefmaids[j].FMA_Id;
                                            Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                            onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                            onlinettrans.FMA_Id = groupwisefmaids[j].FMA_Id;
                                            onlinettrans.FTOT_Amount = fineamountcal - data.FSS_RebateAmount;
                                            onlinettrans.FTOT_Created_date = indianTime;
                                            onlinettrans.FTOT_Updated_date = indianTime;
                                            onlinettrans.FTOT_Concession = 0;
                                            onlinettrans.FTOT_Fine = 0;
                                            onlinettrans.FTOT_RebateAmount = data.FSS_RebateAmount;

                                            _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                        }
                                    }


                                }
                                else
                                {


                                    if (groupwisefmaids[j].FMH_Flag == "F")
                                    {
                                        fethchfmaidsfine = groupwisefmaids[j].FMA_Id;
                                    }
                                    else
                                    {
                                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                        onlinettrans.FMA_Id = groupwisefmaids[j].FMA_Id;
                                        onlinettrans.FTOT_Amount = groupwisefmaids[j].FSS_ToBePaid;
                                        onlinettrans.FTOT_Created_date = indianTime;
                                        onlinettrans.FTOT_Updated_date = indianTime;
                                        onlinettrans.FTOT_Concession = 0;
                                        onlinettrans.FTOT_Fine = 0;
                                        onlinettrans.FTOT_RebateAmount = 0;

                                        using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                        {
                                            if (ecsflag.Equals(0))
                                            {
                                                cmd1.CommandText = "Sp_Calculate_Fine";
                                            }
                                            else
                                            {
                                                cmd1.CommandText = "Sp_Calculate_Fine_ECS";
                                            }

                                            //cmd1.CommandText = "Sp_Calculate_Fine";
                                            cmd1.CommandType = CommandType.StoredProcedure;
                                            cmd1.Parameters.Add(new SqlParameter("@On_Date",
                                                SqlDbType.DateTime, 100)
                                            {
                                                //Value = curdt
                                                Value = indianTime
                                            });

                                            cmd1.Parameters.Add(new SqlParameter("@fma_id",
                                               SqlDbType.NVarChar, 100)
                                            {
                                                Value = groupwisefmaids[j].FMA_Id
                                            });
                                            cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                                           SqlDbType.NVarChar, 100)
                                            {
                                                Value = data.ASMAY_Id
                                            });

                                            cmd1.Parameters.Add(new SqlParameter("@amt",
                                SqlDbType.Decimal, 500)
                                            {
                                                Direction = ParameterDirection.Output
                                            });

                                            cmd1.Parameters.Add(new SqlParameter("@flgArr",
                               SqlDbType.Int, 500)
                                            {
                                                Direction = ParameterDirection.Output
                                            });

                                            if (cmd1.Connection.State != ConnectionState.Open)
                                                cmd1.Connection.Open();

                                            var execfine = cmd1.ExecuteNonQuery();

                                            //finewaiedoff

                                            var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                                 where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMA_Id == groupwisefmaids[j].FMA_Id && a.AMST_Id == data.Amst_Id)
                                                                 select new FeeStudentTransactionDTO
                                                                 {
                                                                     FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                                     FSWO_FineFlg = a.FSWO_FineFlg,
                                                                     FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                                 }
                   ).Distinct().ToList();

                                            if (waivedofffine.Count() > 0)
                                            {
                                                if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                                {
                                                    if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                    {
                                                        fineamountcal = fineamountcal + (Convert.ToInt32(cmd1.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount));
                                                    }
                                                }
                                                else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                                {
                                                    if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                    {
                                                        fineamountcal = fineamountcal + 0;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                    {
                                                        fineamountcal = fineamountcal + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    fineamountcal = fineamountcal + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                }
                                            }

                                         
                                        }

                                        _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                    }


                                    if (groupwisefmaids[j].FMH_Flag == "F")
                                    {
                                        if (finecount < 1)
                                        {
                                            finecount = finecount + 1;
                                            fethchfmaidsfine = groupwisefmaids[j].FMA_Id;
                                            Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                            onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                            onlinettrans.FMA_Id = groupwisefmaids[j].FMA_Id;
                                            onlinettrans.FTOT_Amount = fineamountcal;
                                            onlinettrans.FTOT_Created_date = indianTime;
                                            onlinettrans.FTOT_Updated_date = indianTime;
                                            onlinettrans.FTOT_Concession = 0;
                                            onlinettrans.FTOT_Fine = 0;
                                            onlinettrans.FTOT_RebateAmount = 0;
                                            _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                        }
                                    }


                                }







                            }

                            onlinemtrans.FMOT_Amount = Convert.ToInt32(rece_amt.amt) + Convert.ToInt32(fineamountcal) - data.FSS_RebateAmount;




                            groupidss = "0";
                            fineamountcal = 0;
                            finecount = 0;
                        }

                        termids.Clear();
                    }

                    var contactexisttransaction = 0;

                    using (var dbCtxTxn = _FeeGroupHeadContext.Database.BeginTransaction())
                    {
                        try
                        {
                            contactexisttransaction = _FeeGroupHeadContext.SaveChanges();
                            dbCtxTxn.Commit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            dbCtxTxn.Rollback();
                        }
                    }

                    if (dynamicrecgen.Count() > 0)
                    {
                        data.recenocol = dynamicrecgen.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Array paymentPartpaytmmobile(FeeStudentTransactionDTO enq, long totamount)
        {
            long yrid = getacademicyearcongig(enq);

            enq.ASMAY_Id = yrid;

            long clsid = getcurrentclass(enq);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();

            long fineamt = 0, totalfine = 0;
            try
            {
                Payment pay = new Payment(_context);
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;

                string termids = "0", grpidss = "0";

                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == enq.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                             ).FirstOrDefault();

                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                for (int l = 0; l < enq.selected_list.Length; l++)
                {
                    totalfine = 0;
                    string fpgdids = "";
                    List<long> trm_ids = new List<long>();
                    foreach (var x in enq.selected_list[l].trm_list)
                    {
                        termids = termids + ',' + x.FMT_Id;
                        trm_ids.Add(x.FMT_Id);
                    }

                    enq.ftiidss = termids;

                    var grp_ids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id);
                    var grp_ids1 = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id).ToArray();
                    fineamt = 0;
                    for (int r = 0; r < grp_ids1.Count(); r++)
                    {
                        grpidss = grpidss + ',' + grp_ids1[r];
                    }

                    enq.grpidss = grpidss;

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();
                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }
                    else
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID && trm_ids.Contains(a.fmt_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();

                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }

                    List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                    List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }


                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }


                    List<FeeStudentTransactionDTO> list123 = new List<FeeStudentTransactionDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list123 = list;
                    }
                    else
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list123 = list;

                    }

                    var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupHeadContext.AdmissionStudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.AMST_Id == enq.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                    select new FeeStudentTransactionDTO
                                    {
                                        AMST_ECSFlag = b.AMST_ECSFlag
                                    }
).Distinct().ToArray();

                    int ecsflag = 0;
                    for (int s = 0; s < fetchecs.Count(); s++)
                    {
                        if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                        {
                            ecsflag = 0;
                        }
                        else
                        {
                            ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                        }
                    }


                    //DateTime currdt = DateTime.Now;
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    foreach (var x in list123)
                    {
                        amount_list.Add(x);

                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                          
                            if (ecsflag.Equals(0))
                            {
                                cmd.CommandText = "Sp_Calculate_Fine";
                            }
                            else
                            {
                                cmd.CommandText = "Sp_Calculate_Fine_ECS";
                            }

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime, 100)
                            {
                                //Value = currdt
                                Value = indianTime
                            });

                            cmd.Parameters.Add(new SqlParameter("@fma_id",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = x.FMA_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = enq.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Decimal, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                 where (a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == enq.Amst_Id)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                     FSWO_FineFlg = a.FSWO_FineFlg,
                                                     FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                 }
            ).Distinct().ToList();

                            if (waivedofffine.Count() > 0)
                            {
                                if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = 0;
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                {
                                    fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    totalfine = totalfine + fineamt;
                                }
                            }

                        }

                    }

                    if (Convert.ToInt32(fineamt) > 0)
                    {
                        if (fineheadfmaid.Count() > 0)
                        {
                            finelst.Add(new FeeStudentTransactionDTO
                            {
                                FMA_Id = fineheadfmaid[0].FMA_Id,
                                FSS_ToBePaid = Convert.ToInt32(fineamt),
                                FSS_FineAmount = 0,
                                FSS_ConcessionAmount = 0,
                                FSS_WaivedAmount = 0,
                                FMG_Id = 0,
                                FMH_Id = 0,
                                FTI_Id = 0,
                                FSS_PaidAmount = 0,
                                FSS_NetAmount = 0,
                                FSS_RefundAmount = 0,
                                FMH_FeeName = "Fine",
                                FTI_Name = "Anytime",
                                FSS_CurrentYrCharges = 0,
                                FSS_TotalToBePaid = 0,
                                merchantid = fineheadfmaid[0].merchantid,
                                FYP_Id = 0
                            });
                        }

                    }

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }
                    else
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }


                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (Convert.ToInt32(enq.pendingamount) + fineamt).ToString(),
                        //value = (Convert.ToInt32(enq.pendingamount) + totalfine).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });
                }

                enq.amount_list = amount_list.ToArray();

                if (finelst.Count() > 0)
                {
                    foreach (var y in finelst)
                    {
                        amount_list.Add(y);
                    }
                }



                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                              where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

             
                var receipt = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_Receipt_No);

                PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                if (enq.automanualreceiptno == "Auto")
                {
                    PaymentDetailsDto.trans_id = "CUST" + enq.MI_Id + DateTime.Now.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (DateTime.Now.Millisecond)).Trim();
                }

                PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
                enq.trans_id = PaymentDetailsDto.trans_id.ToString();

                PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }

                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                get_grp_reptno_Mobile(enq);

                // For Testing Purpose
                totpayableamount = 5000;

                PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("MID", paymentdet.FirstOrDefault().merchantid);
                parameters.Add("ORDER_ID", PaymentDetailsDto.trans_id);
                parameters.Add("CUST_ID", enq.MI_Id.ToString());
                parameters.Add("TXN_AMOUNT", totpayableamount.ToString());
                parameters.Add("CHANNEL_ID", "WEB");
                parameters.Add("INDUSTRY_TYPE_ID", "PrivateEducation");
                parameters.Add("WEBSITE", "WEBPROD ");
                parameters.Add("MOBILE_NO", ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile.ToString());
                parameters.Add("EMAIL", ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim());
                parameters.Add("MERC_UNQ_REF", enq.ASMAY_Id.ToString().Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString() + "_" + enq.grpidss.ToString().Trim() + "_" + enq.ftiidss.ToString() + "_" + Convert.ToString(enq.Amst_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile + "_" + totpayableamount.ToString());
                string url = paymentdet.FirstOrDefault().merchanturl;
                //parameters.Add("REQUEST_TYPE", "DEFAULT");
                parameters.Add("CALLBACK_URL", "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponsePAYTM/");
                //parameters.Add("CALLBACK_URL", "https://secure.paytm.in/oltp-web/invoiceResponse");

                string checksum = generateCheckSum(paymentdet.FirstOrDefault().merchantkey, parameters);

                aa.MID = paymentdet.FirstOrDefault().merchantid;
                aa.ORDER_ID = PaymentDetailsDto.trans_id;
                aa.CUST_ID = enq.MI_Id.ToString();
                aa.TXN_AMOUNT = Convert.ToDecimal(totpayableamount);
                aa.CHANNEL_ID = "WEB";
                aa.INDUSTRY_TYPE_ID = "PrivateEducation";
                aa.WEBSITE = "WEBPROD";
                aa.MOBILE_NO = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile;
                aa.EMAIL = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim();
                aa.payu_URL = url;
                aa.CALLBACK_URL = "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponsePAYTM/";
                aa.CHECKSUMHASH = checksum;
                aa.MERC_UNQ_REF = enq.ASMAY_Id.ToString().Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString() + "_" + enq.grpidss.ToString().Trim() + "_" + enq.ftiidss.ToString() + "_" + Convert.ToString(enq.Amst_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile + "_" + totpayableamount.ToString();

                List<PaymentDetails.PAYTM> paydet = new List<PaymentDetails.PAYTM>();
                paydet.Add(aa);
                PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }

        public Array paymentPartpaytm(FeeStudentTransactionDTO enq, long totamount)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            long yrid = getacademicyearcongig(enq);

            enq.ASMAY_Id = yrid;

            long clsid = getcurrentclass(enq);



            var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                            from b in _FeeGroupHeadContext.AdmissionStudentDMO
                            where (a.AMST_Id == b.AMST_Id && a.AMST_Id == enq.Amst_Id && a.ASMAY_Id == enq.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                            select new FeeStudentTransactionDTO
                            {
                                AMST_ECSFlag = b.AMST_ECSFlag
                            }
           ).Distinct().ToArray();

            int ecsflag = 0;
            for (int s = 0; s < fetchecs.Count(); s++)
            {
                if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                {
                    ecsflag = 0;
                }
                else
                {
                    ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                }
            }

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();

            long fineamt = 0, totalfine = 0;
            try
            {
                Payment pay = new Payment(_context);
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;

                string termids = "0", grpidss = "0";

                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == enq.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                             ).FirstOrDefault();

                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                for (int l = 0; l < enq.selected_list.Length; l++)
                {
                    totalfine = 0;
                    string fpgdids = "";
                    List<long> trm_ids = new List<long>();
                    foreach (var x in enq.selected_list[l].trm_list)
                    {
                        termids = termids + ',' + x.FMT_Id;
                        trm_ids.Add(x.FMT_Id);
                    }

                    enq.ftiidss = termids;

                    var grp_ids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id);
                    var grp_ids1 = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id).ToArray();
                    fineamt = 0;
                    for (int r = 0; r < grp_ids1.Count(); r++)
                    {
                        grpidss = grpidss + ',' + grp_ids1[r];
                    }

                    enq.grpidss = grpidss;

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();
                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }
                    else
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID && trm_ids.Contains(a.fmt_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();

                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }

                    List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                    List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }


                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }


                    List<FeeStudentTransactionDTO> list123 = new List<FeeStudentTransactionDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list123 = list;
                    }
                    else
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list123 = list;

                    }

                    foreach (var x in list123)
                    {
                        amount_list.Add(x);

                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            //cmd.CommandText = "Sp_Calculate_Fine";
                            if (ecsflag.Equals(0))
                            {
                                cmd.CommandText = "Sp_Calculate_Fine";
                            }
                            else
                            {
                                cmd.CommandText = "Sp_Calculate_Fine_ECS";
                            }

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime, 100)
                            {
                                //Value = currdt
                                Value = indianTime
                            });

                            cmd.Parameters.Add(new SqlParameter("@fma_id",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = x.FMA_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = enq.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Decimal, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                 where (a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == enq.Amst_Id)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                     FSWO_FineFlg = a.FSWO_FineFlg,
                                                     FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                 }
            ).Distinct().ToList();

                            if (waivedofffine.Count() > 0)
                            {
                                if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = 0;
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                {
                                    fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    totalfine = totalfine + fineamt;
                                }
                            }

                        }

                    }

                    if (Convert.ToInt32(fineamt) > 0)
                    {
                        if (fineheadfmaid.Count() > 0)
                        {
                            finelst.Add(new FeeStudentTransactionDTO
                            {
                                FMA_Id = fineheadfmaid[0].FMA_Id,
                                FSS_ToBePaid = Convert.ToInt32(fineamt),
                                FSS_FineAmount = 0,
                                FSS_ConcessionAmount = 0,
                                FSS_WaivedAmount = 0,
                                FMG_Id = 0,
                                FMH_Id = 0,
                                FTI_Id = 0,
                                FSS_PaidAmount = 0,
                                FSS_NetAmount = 0,
                                FSS_RefundAmount = 0,
                                FMH_FeeName = "Fine",
                                FTI_Name = "Anytime",
                                FSS_CurrentYrCharges = 0,
                                FSS_TotalToBePaid = 0,
                                merchantid = fineheadfmaid[0].merchantid,
                                FYP_Id = 0
                            });
                        }

                    }

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }
                    else
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }


                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (Convert.ToInt32(enq.pendingamount) + fineamt).ToString(),
                        //value = (Convert.ToInt32(enq.pendingamount) + totalfine).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });
                }

                enq.amount_list = amount_list.ToArray();

                if (finelst.Count() > 0)
                {
                    foreach (var y in finelst)
                    {
                        amount_list.Add(y);
                    }
                }


                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                              where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
                PAYMENTPARAMDETAILS = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                       where (a.IMPG_PGFlag == enq.onlinepaygteway)
                                       select new FeeStudentTransactionDTO
                                       {
                                           IMPG_IndustryType = a.IMPG_IndustryType,
                                           IMPG_Website = a.IMPG_Website,
                                           //IMPG_CallBackURL = a.IMPG_CallBackURL
                                       }
           ).ToList();

                var receipt = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_Receipt_No);

                PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    PaymentDetailsDto.trans_id = "RegularOnlinePAYTM" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();
                }

                PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
                enq.trans_id = PaymentDetailsDto.trans_id.ToString();

                PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }

                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                get_grp_reptno(enq);

                // For Testing Purpose
                //totpayableamount = 2;

                PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                parameters.Add("MID", paymentdet.FirstOrDefault().merchantid);
                parameters.Add("ORDER_ID", PaymentDetailsDto.trans_id);
                parameters.Add("CUST_ID", enq.MI_Id.ToString());
                parameters.Add("TXN_AMOUNT", totpayableamount.ToString());
                parameters.Add("CHANNEL_ID", "WEB");

                //for staging
                //parameters.Add("INDUSTRY_TYPE_ID", "Retail");
                //parameters.Add("WEBSITE", "WEBSTAGING");

                //for production
                parameters.Add("INDUSTRY_TYPE_ID", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType);
                parameters.Add("WEBSITE", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website);

                parameters.Add("MOBILE_NO", ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile.ToString());
                parameters.Add("EMAIL", ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim());
                parameters.Add("MERC_UNQ_REF", enq.ASMAY_Id.ToString().Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString() + "_" + enq.grpidss.ToString().Trim() + "_" + enq.ftiidss.ToString() + "_" + Convert.ToString(enq.Amst_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile + "_" + totpayableamount.ToString());
                string url = paymentdet.FirstOrDefault().merchanturl;
                //parameters.Add("REQUEST_TYPE", "DEFAULT");
                parameters.Add("CALLBACK_URL", "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponsePAYTM/");

                string checksum = generateCheckSum(paymentdet.FirstOrDefault().merchantkey, parameters);

                aa.MID = paymentdet.FirstOrDefault().merchantid;
                aa.ORDER_ID = PaymentDetailsDto.trans_id;
                aa.CUST_ID = enq.MI_Id.ToString();
                aa.TXN_AMOUNT = Convert.ToDecimal(totpayableamount);
                aa.CHANNEL_ID = "WEB";

                //for production
                aa.INDUSTRY_TYPE_ID = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType;
                aa.WEBSITE = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website;

                //for staging
                //aa.INDUSTRY_TYPE_ID = "Retail";
                //aa.WEBSITE = "WEBSTAGING";

                aa.MOBILE_NO = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile;
                aa.EMAIL = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim();
                aa.payu_URL = url;

                aa.CHECKSUMHASH = checksum;
                aa.MERC_UNQ_REF = enq.ASMAY_Id.ToString().Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString() + "_" + enq.grpidss.ToString().Trim() + "_" + enq.ftiidss.ToString() + "_" + Convert.ToString(enq.Amst_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_email_id.Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).amst_mobile + "_" + totpayableamount.ToString();

                List<PaymentDetails.PAYTM> paydet = new List<PaymentDetails.PAYTM>();
                paydet.Add(aa);

                PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return PaymentDetailsDto.PaymentDetailsList;

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
                    // below code snippet is mandatory, so that no one can use your checksumgeneration url for other purpose .
                    if (parameters[key].Trim().ToUpper().Contains("REFUND") || parameters[key].Trim().Contains("|"))
                    {
                        parameter.Add(key.Trim(), "");
                    }

                    //if (parameters.ContainsKey(key.Trim()))
                    //{
                    //    parameters[key.Trim()] = parameters[key].Trim();
                    //}
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

        public static String generateCheckSumForRefund(String masterKey, Dictionary<String, String> parameters)
        {
            // Validate Input
            validateGenerateCheckSumInput(masterKey);

            try
            {
                String checkSumParams = SecurityUtils.createCheckSumString(parameters);
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

        public static String generateCheckSumByJson(String masterKey, string json)
        {
            validateGenerateCheckSumInput(masterKey);

            try
            {
                String checkSumParams = json;
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);
                checkSumParams += Constants.VALUE_SEPARATOR_TOKEN;
                checkSumParams += salt;

                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                hashedCheckSum += salt;

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);
                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }

        }

        public static Boolean verifyCheckSumByjson(String masterKey, string json, String checkSum)
        {
            // Validate Input
            validateVerifyCheckSumInput(masterKey, checkSum);

            try
            {
                String hashedCheckSum = Crypto.Decrypt(checkSum, masterKey);

                if (hashedCheckSum == null || hashedCheckSum.Length < Constants.SALT_LENGTH)
                {
                    return false;
                }

                String salt = hashedCheckSum.Substring(hashedCheckSum.Length - Constants.SALT_LENGTH, Constants.SALT_LENGTH);
                MessageConsole.WriteLine("Salt:::: " + salt);


                MessageConsole.WriteLine(); MessageConsole.WriteLine("Input CheckSum:::: " + checkSum);

                // Now creating hashed checkSum string from given checkSum string
                String checkSumParams = json;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String:::: " + checkSumParams);
                checkSumParams += Constants.VALUE_SEPARATOR_TOKEN;
                checkSumParams += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String with Salt:::: " + checkSumParams);

                String hashedInputCheckSum = SecurityUtils.getHashedString(checkSumParams);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String:::: " + hashedInputCheckSum);
                hashedInputCheckSum += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String with Salt:::: " + hashedInputCheckSum);

                return hashedInputCheckSum.Equals(hashedCheckSum);
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while verifying CheckSum. " + e.Message);
            }

        }

        public static Boolean verifyCheckSum(String masterKey, Dictionary<String, String> parameters, String checkSum)
        {
            // Validate Input
            validateVerifyCheckSumInput(masterKey, checkSum);

            try
            {
                String hashedCheckSum = Crypto.Decrypt(checkSum, masterKey);

                if (hashedCheckSum == null || hashedCheckSum.Length < Constants.SALT_LENGTH)
                {
                    return false;
                }

                String salt = hashedCheckSum.Substring(hashedCheckSum.Length - Constants.SALT_LENGTH, Constants.SALT_LENGTH);
                MessageConsole.WriteLine("Salt:::: " + salt);


                MessageConsole.WriteLine(); MessageConsole.WriteLine("Input CheckSum:::: " + checkSum);

                // Now creating hashed checkSum string from given checkSum string
                String checkSumParams = SecurityUtils.createCheckSumString(parameters);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String:::: " + checkSumParams);
                checkSumParams += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("GeneratedCheckSum String with Salt:::: " + checkSumParams);

                String hashedInputCheckSum = SecurityUtils.getHashedString(checkSumParams);
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String:::: " + hashedInputCheckSum);
                hashedInputCheckSum += salt;
                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedGeneratedCheckSum String with Salt:::: " + hashedInputCheckSum);

                return hashedInputCheckSum.Equals(hashedCheckSum);
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while verifying CheckSum. " + e.Message);
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

        private static void validateVerifyCheckSumInput(String masterKey, String checkSum)
        {
            if (masterKey == null)
            {
                //throw new ArgumentNullException("masterKey");
                throw new ArgumentNullException("Parameter cannot be null", "masterKey");
            }

            if (checkSum == null)
            {
                throw new ArgumentNullException("Parameter cannot be null", "checkSum");
            }
        }

        public static string Encrypt(String CardDetails, String masterKey)
        {
            return Crypto.Encrypt(CardDetails, masterKey);
        }

        public static String Decrypt(String carddetails, String masterKey)
        {
            return Crypto.Decrypt(carddetails, masterKey);
        }

        public PaymentDetails.PAYTM paytmresponse(PaymentDetails.PAYTM response)
        {
            FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            PaymentDetails dto = new PaymentDetails();

            string[] tokens = response.MERC_UNQ_REF.Split('_');

            // CODE SNIPPET FOR CHECKSUM GENERATION

            List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
            paymentdet = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                          where (a.MI_Id == Convert.ToInt32(tokens[6]) && a.FPGD_PGName == "PAYTM")
                          select new FeeStudentTransactionDTO
                          {
                              merchantid = a.FPGD_MerchantId,
                              merchantkey = a.FPGD_AuthorisationKey,
                              merchanturl = a.FPGD_URL
                          }
       ).ToList();

            List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
            PAYMENTPARAMDETAILS = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                   where (a.IMPG_PGFlag == "PAYTM")
                                   select new FeeStudentTransactionDTO
                                   {
                                       IMPG_IndustryType = a.IMPG_IndustryType,
                                       IMPG_Website = a.IMPG_Website,
                                       //IMPG_CallBackURL = a.IMPG_CallBackURL
                                   }
       ).ToList();

            PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();

            //commeneted
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("MID", paymentdet[0].merchantid);
            parameters.Add("ORDER_ID", tokens[5]);
            parameters.Add("CUST_ID", tokens[6]);
            parameters.Add("TXN_AMOUNT", tokens[9]);
            parameters.Add("CHANNEL_ID", "WEB");

            //for production
            parameters.Add("INDUSTRY_TYPE_ID", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType);
            parameters.Add("WEBSITE", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website);

            parameters.Add("MOBILE_NO", tokens[8]);
            parameters.Add("EMAIL", tokens[7]);
            parameters.Add("MERC_UNQ_REF", tokens[0].ToString().Trim() + "_" + tokens[1].ToString() + "_" + tokens[2].ToString() + "_" + tokens[3].ToString() + "_" + Convert.ToString(tokens[4]) + "_" + tokens[5] + "_" + tokens[6].ToString() + "_" + tokens[7].Trim() + "_" + tokens[8] + "_" + tokens[9].ToString());

            string url = paymentdet.FirstOrDefault().merchanturl;
            parameters.Add("CALLBACK_URL", "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponsePAYTM/");

            string checksum = generateCheckSum(paymentdet[0].merchantkey, parameters);

            // CODE SNIPPET FOR CHECKSUM VALIDATION
            Boolean res = false;
            res = verifyCheckSum(paymentdet[0].merchantkey, parameters, checksum);

            //commeneted

            if (res == true && response.status == "TXN_SUCCESS")
            {
                dto.udf1 = tokens[0];

                dto.udf3 = tokens[6];
                dto.udf4 = tokens[3];
                dto.udf2 = tokens[4];
                dto.udf5 = tokens[1];
                dto.trans_id = tokens[5];
                dto.email = tokens[7];
                dto.phone = Convert.ToInt64(tokens[8]);
                dto.amount = Convert.ToInt64(tokens[9]);
                dto.IVRMOP_MIID = Convert.ToInt64(tokens[6]);

                dto.txnid = response.txnid;
                dto.BANKTXNID = response.BANKTXNID;

                var groups = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                              where (a.MI_Id == dto.IVRMOP_MIID && a.ASMAY_ID == Convert.ToInt32(dto.udf1) && a.fyp_transaction_id == dto.trans_id)
                              select new FeeStudentTransactionDTO
                              {
                                  FYP_Receipt_No = a.FYP_Receipt_No
                              }
                      ).Distinct().ToList();

                if (groups.Count == 0)
                {
                    var confirmstatus = insertdatainfeetables(dto.udf3, dto.udf4, dto.udf2, dto.udf5, dto.amount, dto.trans_id, dto.txnid, dto.udf1, indianTime, dto.BANKTXNID);

                    if (Convert.ToInt32(confirmstatus) > 0)
                    {
                        pgmod.MI_Id = Convert.ToInt64(dto.udf3);
                        pgmod.amst_mobile = dto.phone;
                        pgmod.Amst_Id = Convert.ToInt64(dto.udf2);
                        pgmod.amst_email_id = dto.email;

                        SMS sms = new SMS(_context);

                        sms.sendSms(Convert.ToInt32(dto.udf3), Convert.ToInt64(dto.phone), "FEEONLINEPAYMENT", Convert.ToInt32(dto.udf2));

                        Email Email = new Email(_context);

                        Email.sendmail(Convert.ToInt32(dto.udf3), dto.email, "FEEONLINEPAYMENT", Convert.ToInt32(dto.udf2));

                    }
                }

            }
            else
            {
                SMS sms = new SMS(_context);

                sms.sendSms(Convert.ToInt32(dto.udf3), Convert.ToInt64(dto.phone), "FEEONLINEPAYMENTFAIL", Convert.ToInt32(dto.udf2));

                Email Email = new Email(_context);

                Email.sendmail(Convert.ToInt32(dto.udf3), dto.email, "FEEONLINEPAYMENTFAIL", Convert.ToInt32(dto.udf2));

                dto.status = response.status;
            }

            return response;
        }

        public FeeStudentTransactionDTO statusapii(FeeStudentTransactionDTO data, long totalamount)
        {
            List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
            paymentdet = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                          where (a.MI_Id == data.MI_Id && a.FPGD_PGName == data.onlinepaygteway)
                          select new FeeStudentTransactionDTO
                          {
                              merchantid = a.FPGD_MerchantId,
                              merchantkey = a.FPGD_AuthorisationKey,
                              merchanturl = a.FPGD_URL
                          }
       ).ToList();

            List<FeeStudentTransactionDTO> PAYMENTPARAMDETAILS = new List<FeeStudentTransactionDTO>();
            PAYMENTPARAMDETAILS = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                   where (a.IMPG_PGFlag == data.onlinepaygteway)
                                   select new FeeStudentTransactionDTO
                                   {
                                       IMPG_TransactionStatusURL = a.IMPG_TransactionStatusURL,
                                   }
       ).ToList();

            //for staging
            //String transactionURL = "https://securegw-stage.paytm.in/merchant-status/getTxnStatus";

            //for production

            //String transactionURL = "https://securegw.paytm.in/merchant-status/getTxnStatus";

            String transactionURL = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_TransactionStatusURL;
            String merchantKey = paymentdet.FirstOrDefault().merchantkey;
            String merchantMid = paymentdet.FirstOrDefault().merchantid;
            //String orderId = data.trans_id;

            String orderId = "RegularOnlinePAYTM42019020817422700341";
            Dictionary<String, String> paytmParams = new Dictionary<String, String>();
            paytmParams.Add("MID", merchantMid);
            paytmParams.Add("ORDERID", orderId);
            try
            {
                string paytmChecksum = generateCheckSum(merchantKey, paytmParams);
                paytmParams.Add("CHECKSUMHASH", paytmChecksum);

                var myContent = JsonConvert.SerializeObject(paytmParams);

                //String postData = "JsonData=" + new JavaScriptSerializer().Serialize(paytmParams);
                String postData = myContent;
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(transactionURL);
                connection.Headers.Add("ContentType", "application/json");
                connection.Method = "POST";
                using (StreamWriter requestWriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestWriter.Write(postData);
                }
                string responseData = string.Empty;

                using (StreamReader responseReader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();
                    JObject json = JObject.Parse(responseData);
                    data.abc = JsonConvert.DeserializeObject<transactionstatuss>(responseData, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });

                    //Response.Write(responseData);
                    //Response.Write("Requested Json= " + postData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Response.Write("Exception message: " + ex.Message.ToString());
            }

            return data;
        }

        public Array paymentPartRAZORPAY(FeeStudentTransactionDTO enq, long totamount)
        {
            long totconclist = 0;
            long yrid = getacademicyearcongig(enq);
            string termlst = "0";
            long fmttotal = 0;

            enq.ASMAY_Id = yrid;

            long clsid = getcurrentclass(enq);

            var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                            from b in _FeeGroupHeadContext.AdmissionStudentDMO
                            where (a.AMST_Id == b.AMST_Id && a.AMST_Id == enq.Amst_Id && a.AMAY_ActiveFlag == 1)
                            select new FeeStudentTransactionDTO
                            {
                                AMST_ECSFlag = b.AMST_ECSFlag,
                                AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) +
                                        (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) +
                                        (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim(),
                                AMST_AdmNo = b.AMST_AdmNo,
                                amst_mobile = b.AMST_MobileNo,
                                amst_email_id = b.AMST_emailId
                            }
).Distinct().ToArray();

            int ecsflag = 0;
            for (int s = 0; s < fetchecs.Count(); s++)
            {
                if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                {
                    ecsflag = 0;
                }
                else
                {
                    ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                }
            }


            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();

            long fineamt = 0, totalfine = 0;
            try
            {
                Payment pay = new Payment(_context);
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;

                string termids = "0", grpidss = "0";

                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == enq.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                             ).FirstOrDefault();

                enq.FSS_FineAmount = 0;
                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                for (int l = 0; l < enq.selected_list.Length; l++)
                {
                    totalfine = 0;
                    string fpgdids = "";
                    List<long> trm_ids = new List<long>();
                    foreach (var x in enq.selected_list[l].trm_list)
                    {
                        termids = termids + ',' + x.FMT_Id;
                        trm_ids.Add(x.FMT_Id);
                        termlst = termlst + "," + x.FMT_Id;
                    }

                    enq.ftiidss = termids;

                    var grp_ids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id);
                    var grp_ids1 = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id).ToArray();
                    fineamt = 0;
                    for (int r = 0; r < grp_ids1.Count(); r++)
                    {
                        grpidss = grpidss + ',' + grp_ids1[r];
                    }

                    enq.grpidss = grpidss;

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();
                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }
                    else
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID && trm_ids.Contains(a.fmt_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();

                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }

                    List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                    List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }


                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }


                    List<FeeStudentTransactionDTO> list123 = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list456 = new List<FeeStudentTransactionDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list123 = list;

                        var list1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                     from g in _FeeGroupHeadContext.feeYCC
                                     from h in _FeeGroupHeadContext.feeYCCC
                                     from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                     from j in _FeeGroupHeadContext.Yearlygroups
                                     where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0 && j.FMG_Id == f.FMG_Id && j.ASMAY_Id == enq.ASMAY_Id && j.FYG_RebateApplicableFlg == true)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMA_Id = b.FMA_Id,
                                         FSS_ToBePaid = i.FSS_ToBePaid,
                                         FSS_FineAmount = i.FSS_FineAmount,
                                         FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                         FSS_WaivedAmount = i.FSS_WaivedAmount,
                                         FMG_Id = b.FMG_Id,
                                         FMH_Id = b.FMH_Id,
                                         FTI_Id = b.FTI_Id,
                                         FSS_PaidAmount = i.FSS_PaidAmount,
                                         FSS_NetAmount = i.FSS_NetAmount,
                                         FSS_RefundAmount = i.FSS_RebateAmount,
                                         FMH_FeeName = c.FMH_FeeName,
                                         FTI_Name = d.FTI_Name,
                                         FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                         FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                     }
                    ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list456 = list1;
                    }
                    else
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list123 = list;

                        var list1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                     from g in _FeeGroupHeadContext.feeYCC
                                     from h in _FeeGroupHeadContext.feeYCCC
                                     from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                     from j in _FeeGroupHeadContext.Yearlygroups
                                     where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0 && j.FMG_Id == f.FMG_Id && j.ASMAY_Id == enq.ASMAY_Id && j.FYG_RebateApplicableFlg == true)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMA_Id = b.FMA_Id,
                                         FSS_ToBePaid = i.FSS_ToBePaid,
                                         FSS_FineAmount = i.FSS_FineAmount,
                                         FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                         FSS_WaivedAmount = i.FSS_WaivedAmount,
                                         FMG_Id = b.FMG_Id,
                                         FMH_Id = b.FMH_Id,
                                         FTI_Id = b.FTI_Id,
                                         FSS_PaidAmount = i.FSS_PaidAmount,
                                         FSS_NetAmount = i.FSS_NetAmount,
                                         FSS_RefundAmount = i.FSS_RebateAmount,
                                         FMH_FeeName = c.FMH_FeeName,
                                         FTI_Name = d.FTI_Name,
                                         FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                         FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                     }
                      ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list456 = list1;

                    }



                    //DateTime currdt = DateTime.Now;
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    foreach (var x in list456)
                    {
                        fmttotal = fmttotal + x.FSS_CurrentYrCharges;
                    }
                    foreach (var x in list123)
                    {
                        amount_list.Add(x);

                        totconclist = totconclist + x.FSS_ConcessionAmount;

                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            if (ecsflag.Equals(0))
                            {
                                cmd.CommandText = "Sp_Calculate_Fine";
                            }
                            else
                            {
                                cmd.CommandText = "Sp_Calculate_Fine_ECS";
                            }

                            //cmd.CommandText = "Sp_Calculate_Fine";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime, 100)
                            {
                                //Value = currdt
                                Value = indianTime
                            });

                            cmd.Parameters.Add(new SqlParameter("@fma_id",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = x.FMA_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = enq.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Decimal, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                 where (a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == enq.Amst_Id)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                     FSWO_FineFlg = a.FSWO_FineFlg,
                                                     FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                 }
            ).Distinct().ToList();

                            if (waivedofffine.Count() > 0)
                            {
                                if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = 0;
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                {
                                    fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    totalfine = totalfine + fineamt;
                                }
                            }

                        }

                    }

                    if (Convert.ToInt32(fineamt) > 0)
                    {
                        if (fineheadfmaid.Count() > 0)
                        {
                            finelst.Add(new FeeStudentTransactionDTO
                            {
                                FMA_Id = fineheadfmaid[0].FMA_Id,
                                FSS_ToBePaid = Convert.ToInt32(fineamt),
                                FSS_FineAmount = 0,
                                FSS_ConcessionAmount = 0,
                                FSS_WaivedAmount = 0,
                                FMG_Id = 0,
                                FMH_Id = 0,
                                FTI_Id = 0,
                                FSS_PaidAmount = 0,
                                FSS_NetAmount = 0,
                                FSS_RefundAmount = 0,
                                FMH_FeeName = "Fine",
                                FTI_Name = "Anytime",
                                FSS_CurrentYrCharges = 0,
                                FSS_TotalToBePaid = 0,
                                merchantid = fineheadfmaid[0].merchantid,
                                FYP_Id = 0
                            });
                        }
                        else
                        {
                            fineamt = 0;
                        }

                    }

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }
                    else
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }


                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (Convert.ToInt32(enq.pendingamount) + fineamt).ToString(),
                        //value = (Convert.ToInt32(enq.pendingamount) + totalfine).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });


                    enq.FSS_FineAmount = enq.FSS_FineAmount + fineamt;
                    var Euserid = (from a in _FeeGroupHeadContext.FeeGroupDMO
                                   where (a.MI_Id == Convert.ToInt64(enq.MI_Id) && grp_ids.Contains(a.FMG_Id))
                                   select new FeeStudentTransactionDTO
                                   {
                                       enduserid = a.user_id,
                                   }
).Distinct().Take(1).ToArray();

                    enq.userid = Convert.ToInt64(Euserid[0].enduserid);
                }

                enq.amount_list = amount_list.ToArray();

                if (finelst.Count() > 0)
                {
                    foreach (var y in finelst)
                    {
                        amount_list.Add(y);
                    }
                }

                //enq.FSS_FineAmount= fineamt;
                enq.Totalconcession = totconclist;

                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

                //multiple gateway
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();


                //PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                //rebate


                var cnt2 = _context.FeeMasterConfigurationDMO.Where(t => t.MI_Id == enq.MI_Id && t.FMC_RebateAplicableFlg == true && t.FMC_RebateAgainstFullPaymentFlg == true && t.FMC_RebateAgainstPartialPaymentFlg == true).ToList();



                string rebateamount;

                if (cnt2.Count > 0)
                {
                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fee_RebateTermWise_calculation_BOTH";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.VarChar)
                        {
                            Value = enq.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.VarChar)
                        {
                            Value = enq.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMT_ID",
                      SqlDbType.VarChar)
                        {
                            Value = termlst
                        });
                        cmd.Parameters.Add(new SqlParameter("@paiddate",
                      SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
                        });
                        cmd.Parameters.Add(new SqlParameter("@paidamount",
                       SqlDbType.VarChar)
                        {
                            Value = fmttotal
                        });

                        cmd.Parameters.Add(new SqlParameter("@USERID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
            SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                    }



                }
                else
                {
                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fee_RebateTermWise_calculation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.VarChar)
                        {
                            Value = enq.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.VarChar)
                        {
                            Value = enq.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMT_ID",
                      SqlDbType.VarChar)
                        {
                            Value = termlst
                        });
                        cmd.Parameters.Add(new SqlParameter("@paiddate",
                      SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
                        });
                        cmd.Parameters.Add(new SqlParameter("@paidamount",
                       SqlDbType.VarChar)
                        {
                            Value = fmttotal
                        });

                        cmd.Parameters.Add(new SqlParameter("@USERID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
            SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                    }

                }

                enq.FSS_RebateAmount = Convert.ToInt64(rebateamount);


                //rebate


                PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }

                var item = new
                {
                    paymentParts = result
                };

                string payinfo = JsonConvert.SerializeObject(item);

                string orderId;
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                transfersnotes.Add("notes_1", fetchecs[0].AMST_FirstName);
                transfersnotes.Add("notes_2", fetchecs[0].AMST_AdmNo);
                transfersnotes.Add("notes_3", fetchecs[0].Amst_Id);
                transfersnotes.Add("notes_4", fetchecs[0].amst_mobile);
                transfersnotes.Add("notes_5", fetchecs[0].amst_email_id);

                Dictionary<string, object> input = new Dictionary<string, object>();
                //input.Add("amount", 1 * 100);
                input.Add("amount", (totpayableamount - enq.FSS_RebateAmount) * 100); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("receipt", PaymentDetailsDto.trans_id);
                input.Add("payment_capture", 1);
                input.Add("notes", transfersnotes);

                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                RazorpayClient client = new RazorpayClient(key, secret);

                //Dictionary<string, object>[] matrix2 = new Dictionary<string, object>[result.Count];
                //Dictionary<string, object> matrix1 = new Dictionary<string, object>();

                //for (int q = 0; q < result.Count; q++)
                //{
                //    matrix2[q] = new Dictionary<string, object>();
                //    matrix2[q].Add("account", result[q].merchantId);
                //    matrix2[q].Add("amount", Convert.ToInt32(result[q].value) * 100);
                //    matrix2[q].Add("currency", "INR");

                //    matrix1.Add("roll_no", fetchecs[0].AMST_AdmNo);
                //    matrix1.Add("name", fetchecs[q].AMST_FirstName);

                //    matrix2[q].Add("notes", matrix1);
                //}

                //input.Add("transfers", matrix2);


                Razorpay.Api.Order order = client.Order.Create(input);
                orderId = order["id"].ToString();

                enq.trans_id = orderId;
                enq.merchantkey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                enq.FMA_Amount = totpayableamount;
                enq.splitpayinformation = payinfo;

                //PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

                //get_grp_reptno_Mobile(enq);

                if (enq.FYP_PayModeType == "Portal")
                {
                    get_grp_reptno(enq);
                }
                else if (enq.FYP_PayModeType == "MOBILE")
                {
                    get_grp_reptno_Mobile(enq);
                }

                if (enq.recenocol != null)
                {
                    Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }

        public PaymentDetails razorresponse(PaymentDetails response)
        {
            try
            {
                FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
                PaymentDetails dto = new PaymentDetails();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //single account added on 17/12/2019

                var accountvalidation = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.MI_Id == response.IVRMOP_MIID && a.FPGD_PGName == "RAZORPAY")
                                         select new FeeStudentTransactionDTO
                                         {
                                             FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                         }).Distinct().ToArray();

                //single account added on 17/12/2019

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == response.IVRMOP_MIID)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                          ).FirstOrDefault();

                //TRANSFER API

                string url = "https://api.razorpay.com/v1/payments/" + response.razorpay_payment_id + "/transfers";

                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.IVRMOP_MIID && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);
                Razorpay.Api.Payment payment = client.Payment.Fetch(response.razorpay_payment_id);
                response.order_id = payment.Attributes["order_id"];

                //response.order_id = response.razorpay_order_id;

                //Generate Signature
                //string hashh = string.Empty;
                //try
                //{
                //    string str = response.order_id + "|" + response.razorpay_payment_id;

                //    UTF8Encoding encoder = new UTF8Encoding();

                //    byte[] hashValue;
                //    byte[] keybyt = encoder.GetBytes(paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);
                //    byte[] message = encoder.GetBytes(str);

                //    HMACSHA256 hashString = new HMACSHA256(keybyt);
                //    string hex = "";

                //    hashValue = hashString.ComputeHash(message);
                //    foreach (byte x in hashValue)
                //    {
                //        hex += string.Format("{0:x2}", x);
                //    }
                //    hashh = hex;
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //}

                //Generate Signature

                //FETCH SUBMERCHANT IDS

                //if (hashh == response.razorpay_signature)
                //{
                var fetchfmhotid = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                    where (a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Amount > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMHOT_Id = a.FMOT_Id,
                                        FMA_Amount = a.FMOT_Amount,
                                        MI_Id = a.MI_Id,
                                        ASMAY_Id = a.ASMAY_ID,
                                        Amst_Id = a.AMST_Id,

                                    }).ToArray();

                var fetchstudentdeatils = (from a in _FeeGroupHeadContext.AdmissionStudentDMO
                                           from b in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                           where (a.AMST_Id == b.AMST_Id && a.AMST_Id == Convert.ToInt64(fetchfmhotid[0].Amst_Id) && b.ASMAY_Id == Convert.ToInt64(fetchfmhotid[0].ASMAY_Id) && a.MI_Id == Convert.ToInt64(fetchfmhotid[0].MI_Id))
                                           select new FeeStudentTransactionDTO
                                           {
                                               amst_mobile = a.AMST_MobileNo,
                                               amst_email_id = a.AMST_emailId,
                                               ASMCL_ID = b.ASMCL_Id,
                                               AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                               AMST_AdmNo = a.AMST_AdmNo,
                                               Amst_Id = a.AMST_Id
                                           }).ToArray();

                if (fetchstudentdeatils.Length == 0)
                {

                    var acayr = _FeeGroupHeadContext.AcademicYear.Where(r => r.MI_Id == Convert.ToInt64(fetchfmhotid[0].MI_Id) && r.Is_Active == true && r.ASMAY_Id == Convert.ToInt64(fetchfmhotid[0].ASMAY_Id)).ToList();

                    var acaorder = acayr.OrderByDescending(r => r.ASMAY_Order).FirstOrDefault().ASMAY_Order;

                    var acayrid = _FeeGroupHeadContext.AcademicYear.Where(r => r.MI_Id == Convert.ToInt64(fetchfmhotid[0].MI_Id) && r.Is_Active == true && r.ASMAY_Order == Convert.ToInt32(acaorder) - 1).ToList();

                    if (acayrid.Count() > 0)
                    {
                        fetchstudentdeatils = (from a in _FeeGroupHeadContext.AdmissionStudentDMO
                                               from b in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                               where (a.AMST_Id == b.AMST_Id && a.AMST_Id == Convert.ToInt64(fetchfmhotid[0].Amst_Id) && b.ASMAY_Id == Convert.ToInt64(acayrid.FirstOrDefault().ASMAY_Id) && a.MI_Id == Convert.ToInt64(fetchfmhotid[0].MI_Id))
                                               select new FeeStudentTransactionDTO
                                               {
                                                   amst_mobile = a.AMST_MobileNo,
                                                   amst_email_id = a.AMST_emailId,
                                                   ASMCL_ID = b.ASMCL_Id,
                                                   AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                                   AMST_AdmNo = a.AMST_AdmNo,
                                                   Amst_Id = a.AMST_Id
                                               }).ToArray();
                    }

                    pgmod.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                    pgmod.ASMAY_Id = Convert.ToInt64(fetchfmhotid[0].ASMAY_Id);
                    pgmod.ASMCL_ID = Convert.ToInt64(fetchstudentdeatils[0].ASMCL_ID);
                    pgmod.Amst_Id = Convert.ToInt64(fetchfmhotid[0].Amst_Id);
                    getcurrentclass(pgmod);
                    fetchstudentdeatils[0].ASMCL_ID = pgmod.ASMCL_ID;

                }

                Dictionary<String, object> transfers = new Dictionary<String, object>();
                Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

                List<FeeStudentTransactionDTO> fetchaccountid = new List<FeeStudentTransactionDTO>();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    transfers.Clear();
                    transfersnotes.Clear();
                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fetchaccountid = (from a in _FeeGroupHeadContext.Fee_T_Online_TransactionDMO
                                          from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                          from c in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                          from d in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                          from e in _FeeGroupHeadContext.PAYUDETAILS
                                          where (a.FMA_Id == b.FMA_Id && b.FMG_Id == c.fmg_id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.fti_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && e.IMPG_PGFlag == "RAZORPAY" && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(fetchfmhotid[r].MI_Id) && b.ASMAY_Id == Convert.ToInt64(fetchfmhotid[r].ASMAY_Id))
                                          select new FeeStudentTransactionDTO
                                          {
                                              FPGD_SubMerchantId = d.FPGD_SubMerchantId
                                          }).Distinct().ToList();
                    }
                    else
                    {
                        fetchaccountid = (from a in _FeeGroupHeadContext.Fee_T_Online_TransactionDMO
                                          from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                          from c in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                          from d in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                          from e in _FeeGroupHeadContext.PAYUDETAILS
                                          where (a.FMA_Id == b.FMA_Id && b.FMG_Id == c.fmg_id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.fti_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && e.IMPG_PGFlag == "RAZORPAY" && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(fetchfmhotid[r].MI_Id) && b.ASMAY_Id == Convert.ToInt64(fetchfmhotid[r].ASMAY_Id) && c.ASMCL_Id == Convert.ToInt64(fetchstudentdeatils[0].ASMCL_ID))
                                          select new FeeStudentTransactionDTO
                                          {
                                              FPGD_SubMerchantId = d.FPGD_SubMerchantId
                                          }).Distinct().ToList();
                    }

                    var fetchamount = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                       where (a.AMST_Id == Convert.ToInt64(fetchfmhotid[0].Amst_Id) && a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id)
                                       select new FeeStudentTransactionDTO
                                       {
                                           FMA_Amount = a.FMOT_Amount
                                       }).ToArray();

                    transfersnotes.Add("notes_1", fetchstudentdeatils[0].AMST_FirstName);
                    transfersnotes.Add("notes_2", fetchstudentdeatils[0].AMST_AdmNo);
                    transfersnotes.Add("notes_3", fetchstudentdeatils[0].Amst_Id);
                    transfersnotes.Add("notes_4", fetchstudentdeatils[0].amst_mobile);
                    transfersnotes.Add("notes_5", fetchstudentdeatils[0].amst_email_id);

                    transfers.Add("account", (fetchaccountid.FirstOrDefault().FPGD_SubMerchantId));
                    transfers.Add("amount", (Convert.ToInt32(fetchamount.FirstOrDefault().FMA_Amount.ToString()) * 100).ToString());
                    transfers.Add("currency", "INR");
                    transfers.Add("notes", transfersnotes);

                    if (accountvalidation.Count() > 1)
                    {

                        Razorpay.Api.Transfer payment1 = client.Transfer.Create(transfers);

                        transferAPI trapay = new transferAPI();

                        if (payment1.Attributes["id"] != "")
                        {
                            trapay.transfer_id = payment1.Attributes["id"];
                            trapay.entity = payment1.Attributes["entity"];
                            trapay.source = payment1.Attributes["source"];
                            trapay.recipient = payment1.Attributes["recipient"];
                            trapay.amount = payment1.Attributes["amount"];
                            trapay.created_at = payment1.Attributes["created_at"];

                            FEE_RAZOR_TRANSFER_API_DETAILS fet = new FEE_RAZOR_TRANSFER_API_DETAILS();
                            fet.TRANSFER_ID = trapay.transfer_id;
                            fet.ENTITY = trapay.entity;
                            fet.SOURCE = trapay.source;
                            fet.RECIPIENT = trapay.recipient;
                            fet.AMOUNT = (Convert.ToInt32(trapay.amount) / 100).ToString();
                            fet.CREATED_AT = trapay.created_at;
                            fet.ORDER_ID = response.order_id;

                            fet.PAYMENT_ID = response.razorpay_payment_id;
                            fet.MI_ID = Convert.ToInt32(fetchfmhotid[0].MI_Id);
                            fet.SETTLEMENT_FLAG = "0";

                            fet.CREATED_BY = indianTime;
                            fet.UPDATED_BY = indianTime;
                            _FeeGroupHeadContext.Add(fet);
                            var contactExists = _FeeGroupHeadContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                response.status = "success";
                            }
                            else
                            {
                                response.status = "Failure";
                            }
                        }
                    }
                    else
                    {
                        FEE_RAZOR_TRANSFER_API_DETAILS fet = new FEE_RAZOR_TRANSFER_API_DETAILS();
                        fet.TRANSFER_ID = "";
                        fet.ENTITY = "";
                        fet.SOURCE = "";
                        fet.RECIPIENT = "";
                        fet.AMOUNT = (Convert.ToInt32(fetchamount.FirstOrDefault().FMA_Amount.ToString())).ToString();
                        fet.CREATED_AT = indianTime.ToString();
                        fet.ORDER_ID = response.order_id;

                        fet.PAYMENT_ID = response.razorpay_payment_id;
                        fet.MI_ID = Convert.ToInt32(fetchfmhotid[0].MI_Id);
                        fet.SETTLEMENT_FLAG = "0";

                        fet.CREATED_BY = indianTime;
                        fet.UPDATED_BY = indianTime;
                        _FeeGroupHeadContext.Add(fet);
                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            response.status = "success";
                        }
                        else
                        {
                            response.status = "Failure";
                        }
                    }
                }

                if (response.status == "success")
                {
                    var groups = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                                  where (a.MI_Id == Convert.ToInt32(fetchfmhotid[0].MI_Id) && a.ASMAY_ID == Convert.ToInt32(fetchfmhotid[0].ASMAY_Id) && a.fyp_transaction_id == response.order_id)
                                  select new FeeStudentTransactionDTO
                                  {
                                      FYP_Receipt_No = a.FYP_Receipt_No
                                  }
                          ).Distinct().ToList();

                    if (groups.Count == 0)
                    {
                        var confirmstatus = insertdatainfeetables(fetchfmhotid[0].MI_Id.ToString(), "0", fetchfmhotid[0].Amst_Id.ToString(), "0", fetchfmhotid[0].FMA_Amount, response.order_id, response.razorpay_payment_id, fetchfmhotid[0].ASMAY_Id.ToString(), indianTime, "0");

                        if (Convert.ToInt32(confirmstatus) > 0)
                        {
                            SMS sms = new SMS(_context);

                            sms.sendSms(Convert.ToInt32(fetchfmhotid[0].MI_Id), Convert.ToInt64(fetchstudentdeatils[0].amst_mobile), "FEEONLINEPAYMENT", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                            Email Email = new Email(_context);

                            Email.sendmail(Convert.ToInt32(fetchfmhotid[0].MI_Id), fetchstudentdeatils[0].amst_email_id, "FEEONLINEPAYMENT", Convert.ToInt32(fetchfmhotid[0].Amst_Id));
                        }
                    }
                }
                else
                {
                    SMS sms = new SMS(_context);

                    sms.sendSms(Convert.ToInt32(fetchfmhotid[0].MI_Id), Convert.ToInt64(fetchstudentdeatils[0].amst_mobile), "FEEONLINEPAYMENTFAIL", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                    Email Email = new Email(_context);

                    Email.sendmail(Convert.ToInt32(fetchfmhotid[0].MI_Id), fetchstudentdeatils[0].amst_email_id, "FEEONLINEPAYMENTFAIL", Convert.ToInt32(fetchfmhotid[0].Amst_Id));

                    dto.status = response.status;
                }
                //}
                //else
                //{
                //response.status = "Failure";
                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
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

        public PaymentDetails RefundAPI(PaymentDetails response)
        {
            try
            {
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == 4 && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                // payment to be refunded, payment must be a captured payment
                Razorpay.Api.Payment payment = client.Payment.Fetch(response.razorpay_payment_id);

                //Full Refund
                Refund refund = payment.Refund();

                //Partial Refund
                Dictionary<string, object> data = new Dictionary<string, object>();
                data.Add("amount", "100");
                Refund Partialrefund = payment.Refund(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public PaymentDetails OrdersAPI(PaymentDetails response)
        {
            try
            {
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == 4 && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                try
                {
                    JObject orderRequest = new JObject();

                    // supported option filters (from, to, count, skip)
                    orderRequest.Add("count", 2);
                    orderRequest.Add("skip", 1);

                    //List<Order> orders = razorpay.Orders.fetchAll(orderRequest);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public FeeStudentTransactionDTO Razorpaypaymentsettlementresponse(FeeStudentTransactionDTO response)
        {
            try
            {
                PaymentDetails response1 = new PaymentDetails();
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                try
                {
                    Dictionary<string, object> input = new Dictionary<string, object>();
                    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                    Int32 unixTimestampstart = 0;
                    Int32 unixTimestampend = 0;
                    if (response.date != null && response.date != "" && response.date != "0" && response.date != "1")
                    {
                        //CURRENT DAY - 1 
                        unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddDays(-(Convert.ToInt32(response.date))).AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddDays(-(Convert.ToInt32(response.date) - 1)).AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    }
                    else
                    {
                        //CURRENT DAY
                        unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    }

                    // supported option filters (from, to, count, skip)

                    input.Add("from", unixTimestampstart.ToString());
                    input.Add("to", unixTimestampend.ToString());
                    input.Add("count", 100);
                    //input.Add("skip", 0);

                    List<Order> orders = client.Order.All(input);
                    string id = "";
                    foreach (var x in orders)
                    {

                        id = "order_LlqC2kunIHWHkR";
                        // id = x.Attributes["id"];
                        if (x.Attributes["status"] == "paid")
                        {
                            var pendingpayments = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                                                   from b in _FeeGroupHeadContext.FEE_RAZOR_TRANSFER_API_DETAILS
                                                   where (a.MI_Id == response.MI_Id && b.ORDER_ID == id && a.fyp_transaction_id == id && a.FYP_OnlineChallanStatusFlag == "Sucessfull" && a.FYP_PayGatewayType == "RAZORPAY")
                                                   select new FeeStudentTransactionDTO
                                                   {
                                                       order_id = a.fyp_transaction_id
                                                   }
                         ).ToList();

                            if (pendingpayments.Count == 0)
                            {
                                response1.order_id = id;
                                response1.IVRMOP_MIID = response.MI_Id;

                                //FETCH PAYMENT BASED ON ORDER-ID

                                string method = "GET";
                                string url = "https://api.razorpay.com/v1/orders/ID/payments";
                                url = url.Replace("ID", id);
                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                request.Method = method.ToString();
                                request.ContentLength = 0;
                                request.ContentType = "application/json";

                                string userAgent = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                                request.UserAgent = "razorpay-dot-net/" + userAgent;

                                string authString = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                                    Encoding.UTF8.GetBytes(authString));

                                System.Net.WebResponse resp = request.GetResponseAsync().Result;
                                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                                string s = sr.ReadToEnd().Trim();
                                JObject joResponse1 = JObject.Parse(s);
                                JArray array1 = (JArray)joResponse1["items"];
                                foreach (JObject root1 in array1)
                                {
                                    if ((String)root1["status"] == "captured")
                                    {
                                        response1.razorpay_payment_id = (String)root1["id"];
                                        razorresponse(response1);
                                    }
                                }
                                //FETCH PAYMENT BASED ON ORDER-ID
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        public PaymentDetails transactionstatuspaytm(PaymentDetails data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                indianTime = indianTime.AddDays(-data.date);

                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                              where (a.MI_Id == data.MI_Id && a.FPGD_PGName == "PAYTM")
                              select new FeeStudentTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                List<FeeStudentTransactionDTO> pendingtransactions = new List<FeeStudentTransactionDTO>();
                //       pendingtransactions = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                //                     where (a.MI_Id == data.MI_Id && a.FMOT_PayGatewayType=="PAYTM" && a.AMST_Id!=0 && Convert.ToDateTime(a.FMOT_Date.ToString("yyyy-MM-dd")) <= Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd")) && Convert.ToDateTime(a.FMOT_Date.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd")))
                //                     select new FeeStudentTransactionDTO
                //                     {
                //                         trans_id = a.FMOT_Trans_Id,
                //                     }
                //).ToList();

                pendingtransactions = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                       where (a.MI_Id == data.MI_Id && a.FMOT_PayGatewayType == "PAYTM" && a.AMST_Id != 0 && Convert.ToDateTime(a.FMOT_Date.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd")))
                                       select new FeeStudentTransactionDTO
                                       {
                                           trans_id = a.FMOT_Trans_Id,
                                       }
     ).Distinct().ToList();

                if (pendingtransactions.Count > 0)
                {
                    for (int p = 0; p < pendingtransactions.Count; p++)
                    {
                        PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();
                        Dictionary<string, string> parameters = new Dictionary<string, string>();

                        parameters.Add("MID", paymentdet.FirstOrDefault().merchantid);
                        parameters.Add("ORDER_ID", pendingtransactions[p].trans_id);

                        string checksum = generateCheckSum(paymentdet.FirstOrDefault().merchantkey, parameters);
                        string url = "https://securegw.paytm.in/order/status";

                        Dictionary<String, String> paytmParams = new Dictionary<String, String>();
                        paytmParams.Add("MID", paymentdet.FirstOrDefault().merchantid);
                        paytmParams.Add("ORDER_ID", pendingtransactions[p].trans_id);
                        paytmParams.Add("CHECKSUMHASH", checksum);

                        var myContent = JsonConvert.SerializeObject(paytmParams);

                        String postData = myContent;
                        HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                        connection.ContentType = "application/json";

                        connection.Method = "POST";

                        using (StreamWriter requestWriter = new StreamWriter(connection.GetRequestStream()))
                        {
                            requestWriter.Write(postData);
                        }
                        string responseData = string.Empty;

                        using (StreamReader responseReader = new StreamReader(connection.GetResponse().GetResponseStream()))
                        {
                            responseData = responseReader.ReadToEnd();


                            JObject joResponse1 = JObject.Parse(responseData);
                            string txnstatus = (string)joResponse1["STATUS"];

                            if (txnstatus == "TXN_SUCCESS")
                            {
                                var txnstatusexists = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                                                       where (a.MI_Id == data.MI_Id && a.fyp_transaction_id == (string)joResponse1["ORDERID"])
                                                       select new FeeStudentTransactionDTO
                                                       {
                                                           fyp_transaction_id = a.fyp_transaction_id,
                                                       }
                                           ).ToList();
                                if (txnstatusexists.Count == 0)
                                {
                                    PaymentDetails.PAYTM response = new PaymentDetails.PAYTM();
                                    response.MERC_UNQ_REF = (string)joResponse1["MERC_UNQ_REF"];
                                    response.txnid = (string)joResponse1["TXNID"];
                                    response.BANKTXNID = (string)joResponse1["BANKTXNID"];
                                    response.status = txnstatus;
                                    paytmresponse(response);
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO RazorpayTransactionLogs(FeeStudentTransactionDTO data)
        {
            try
            {
                List<Translogsresults> razorpayparam = new List<Translogsresults>();
                List<FeeStudentTransactionDTO> razorpaytransactionlogs = new List<FeeStudentTransactionDTO>();
                razorpaytransactionlogs = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                           where (a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_ID == data.ASMAY_Id && a.FMOT_PayGatewayType == "RAZORPAY")
                                           select new FeeStudentTransactionDTO
                                           {
                                               trans_id = a.FMOT_Trans_Id,
                                               FYP_PayModeType = a.FYP_PayModeType,
                                               FYP_Date = a.FMOT_Date,
                                               FMOT_PayGatewayType = a.FMOT_PayGatewayType
                                           }
     ).ToList();

                if (razorpaytransactionlogs.Count > 0)
                {
                    for (int i = 0; i < razorpaytransactionlogs.Count(); i++)
                    {
                        //Dictionary<string, string> razorpayparam = new Dictionary<string, string>();

                        string PaymentStatusurl = "https://api.razorpay.com/v1/orders/ID/payments";

                        PaymentDetails response1 = new PaymentDetails();
                        List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                        paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();
                        RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                        string method = "GET";
                        PaymentStatusurl = PaymentStatusurl.Replace("ID", razorpaytransactionlogs[i].trans_id);
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PaymentStatusurl);
                        request.Method = method.ToString();
                        request.ContentLength = 0;
                        request.ContentType = "application/json";

                        string userAgent = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                        request.UserAgent = "razorpay-dot-net/" + userAgent;

                        string authString = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                        request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                            Encoding.UTF8.GetBytes(authString));

                        System.Net.WebResponse resp = request.GetResponseAsync().Result;
                        System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                        string s = sr.ReadToEnd().Trim();
                        JObject joResponse1 = JObject.Parse(s);
                        JArray array1 = (JArray)joResponse1["items"];

                        foreach (JObject root1 in array1)
                        {
                            razorpayparam.Add(new Translogsresults
                            {
                                payment_id = (String)root1["id"],
                                responsestatuslogs = (String)root1["status"],
                                error_description = (String)root1["error_description"],
                                order_id = (String)root1["order_id"],
                                FYP_Date = razorpaytransactionlogs[i].FYP_Date,
                                FMA_Amount = (Int32)root1["amount"],
                                FYP_PayModeType = razorpaytransactionlogs[i].FYP_PayModeType,
                                FMOT_PayGatewayType = razorpaytransactionlogs[i].FMOT_PayGatewayType
                            });
                        }
                    }
                    data.translogresults = razorpayparam.ToArray();
                }

                List<FeeStudentTransactionDTO> paytmtransactionlogs = new List<FeeStudentTransactionDTO>();
                paytmtransactionlogs = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                        where (a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_ID == data.ASMAY_Id && a.FMOT_PayGatewayType == "PAYTM")
                                        select new FeeStudentTransactionDTO
                                        {
                                            trans_id = a.FMOT_Trans_Id,
                                            FYP_PayModeType = a.FYP_PayModeType,
                                            FYP_Date = a.FMOT_Date,
                                            FMOT_PayGatewayType = a.FMOT_PayGatewayType
                                        }
    ).ToList();
                List<Student_SettlementDTO> paymentdet = new List<Student_SettlementDTO>();
                paymentdet = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                              where (a.MI_Id == data.MI_Id && a.FPGD_PGName == "PAYTM")
                              select new Student_SettlementDTO
                              {
                                  MID = a.FPGD_MerchantId,
                                  FPGD_SubMerchantKey = a.FPGD_SubMerchantKey,
                                  FPGD_AuthorizationHeader = a.FPGD_AuthorizationHeader,
                                  FPGD_AuthorisationKey = a.FPGD_AuthorisationKey,
                              }
           ).ToList();



                if (paytmtransactionlogs.Count() > 0)
                {
                    for (int j = 0; j < paytmtransactionlogs.Count(); j++)
                    {
                        string paytmurl = "https://securegw.paytm.in/merchant-status/api/v1/getPaymentStatus";

                        Dictionary<String, String> paytmParams = new Dictionary<String, String>();
                        paytmParams.Add("mid", paymentdet.FirstOrDefault().MID);
                        paytmParams.Add("orderId", paytmtransactionlogs[j].trans_id);

                        string paytmChecksum = generateCheckSum(paymentdet.FirstOrDefault().FPGD_AuthorisationKey, paytmParams);
                        paytmParams.Add("checksumHash", paytmChecksum);

                        var myContent = JsonConvert.SerializeObject(paytmParams);

                        String postData = myContent;
                        HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(paytmurl);
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

                            JObject joResponse1 = JObject.Parse(responseData);
                            JArray array1 = (JArray)joResponse1["settlementSearchResponseVO"]["settlementDetailList"];
                            JArray array2 = (JArray)joResponse1["body"];
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //EASEBUZZ

        public FeeStudentTransactionDTO paymentPartEasyBuzz(FeeStudentTransactionDTO enq)
        {
            long totconclist = 0;
            enq.ASMAY_Id = enq.ASMAY_Id;

            long clsid = enq.ASMCL_ID;
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


            var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                            from b in _FeeGroupHeadContext.AdmissionStudentDMO
                            where (a.AMST_Id == b.AMST_Id && a.AMST_Id == enq.Amst_Id && a.AMAY_ActiveFlag == 1)
                            select new FeeStudentTransactionDTO
                            {
                                AMST_ECSFlag = b.AMST_ECSFlag,
                                AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) +
                                        (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) +
                                        (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim(),
                                AMST_AdmNo = b.AMST_AdmNo,
                                amst_mobile = b.AMST_MobileNo,
                                amst_email_id = b.AMST_emailId
                            }
).Distinct().ToArray();

            int ecsflag = 0;
            for (int s = 0; s < fetchecs.Count(); s++)
            {
                if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                {
                    ecsflag = 0;
                }
                else
                {
                    ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                }
            }


            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();

            long fineamt = 0, totalfine = 0;
            try
            {
                Payment pay = new Payment(_context);
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;

                string termids = "0", grpidss = "0";

                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == enq.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                             ).FirstOrDefault();

                enq.FSS_FineAmount = 0;
                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                for (int l = 0; l < enq.selected_list.Length; l++)
                {
                    totalfine = 0;
                    string fpgdids = "";
                    List<long> trm_ids = new List<long>();
                    foreach (var x in enq.selected_list[l].trm_list)
                    {
                        termids = termids + ',' + x.FMT_Id;
                        trm_ids.Add(x.FMT_Id);
                    }

                    enq.ftiidss = termids;

                    var grp_ids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id);
                    var grp_ids1 = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id).ToArray();
                    fineamt = 0;
                    for (int r = 0; r < grp_ids1.Count(); r++)
                    {
                        grpidss = grpidss + ',' + grp_ids1[r];
                    }

                    enq.grpidss = grpidss;

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();
                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }
                    else
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID && trm_ids.Contains(a.fmt_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();

                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }

                    List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                    List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }


                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }


                    List<FeeStudentTransactionDTO> list123 = new List<FeeStudentTransactionDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list123 = list;
                    }
                    else
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list123 = list;

                    }




                    foreach (var x in list123)
                    {
                        amount_list.Add(x);

                        totconclist = totconclist + x.FSS_ConcessionAmount;

                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            if (ecsflag.Equals(0))
                            {
                                cmd.CommandText = "Sp_Calculate_Fine";
                            }
                            else
                            {
                                cmd.CommandText = "Sp_Calculate_Fine_ECS";
                            }
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime, 100)
                            {
                                Value = indianTime
                            });

                            cmd.Parameters.Add(new SqlParameter("@fma_id",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = x.FMA_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = enq.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Decimal, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                 where (a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == enq.Amst_Id)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                     FSWO_FineFlg = a.FSWO_FineFlg,
                                                     FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                 }
            ).Distinct().ToList();

                            if (waivedofffine.Count() > 0)
                            {
                                if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = 0;
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                {
                                    fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    totalfine = totalfine + fineamt;
                                }
                            }

                        }

                    }

                    if (Convert.ToInt32(fineamt) > 0)
                    {
                        if (fineheadfmaid.Count() > 0)
                        {
                            finelst.Add(new FeeStudentTransactionDTO
                            {
                                FMA_Id = fineheadfmaid[0].FMA_Id,
                                FSS_ToBePaid = Convert.ToInt32(fineamt),
                                FSS_FineAmount = 0,
                                FSS_ConcessionAmount = 0,
                                FSS_WaivedAmount = 0,
                                FMG_Id = 0,
                                FMH_Id = 0,
                                FTI_Id = 0,
                                FSS_PaidAmount = 0,
                                FSS_NetAmount = 0,
                                FSS_RefundAmount = 0,
                                FMH_FeeName = "Fine",
                                FTI_Name = "Anytime",
                                FSS_CurrentYrCharges = 0,
                                FSS_TotalToBePaid = 0,
                                merchantid = fineheadfmaid[0].merchantid,
                                FYP_Id = 0
                            });
                        }

                    }

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }
                    else
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }


                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (Convert.ToInt32(enq.pendingamount) + fineamt).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });

                    enq.FSS_FineAmount = enq.FSS_FineAmount + fineamt;
                }

                enq.amount_list = amount_list.ToArray();

                if (finelst.Count() > 0)
                {
                    foreach (var y in finelst)
                    {
                        amount_list.Add(y);
                    }
                }
                enq.Totalconcession = totconclist;
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }

                var item = new
                {
                    paymentParts = result
                };
                string hashkey = "key|txnid|Amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10|salt";
                string payinfo = JsonConvert.SerializeObject(item);
                string orderId;
                string amount = (Convert.ToDecimal(totpayableamount)).ToString();
                string firstname = fetchecs[0].AMST_FirstName;
                string email = fetchecs[0].amst_email_id;
                string phone = (fetchecs[0].amst_mobile).ToString();

                string surl = "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponseeasybuzz/";
                string furl = "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponseeasybuzz/";

                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    //enq.trans_id = enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();
                    enq.trans_id = enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();
                }
                string Txnid = (enq.trans_id).ToString();
                string UDF1 = (enq.Amst_Id).ToString();

                string UDF2 = (enq.userid).ToString();

                string UDF3 = (enq.MI_Id).ToString();

                string UDF4 = (enq.ASMAY_Id).ToString();

                string UDF5 = (enq.ftiidss).ToString();

                string UDF6 = "";

                string UDF7 = "";
                string UDF8 = "";
                string UDF9 = "";
                string UDF10 = "";
                string productinfo = (enq.ASMCL_ID).ToString();
                string Show_payment_mode = "";
                string split_payments = "";
                string sub_merchant_id = "";

                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SubMerchantId;

                string env = "test";
                Easebuzz t1 = new Easebuzz(secret, key, env);

                string strForm = t1.initiatePaymentAPI(amount, firstname, email, phone, productinfo, surl, furl, Txnid, UDF1, UDF2, UDF3, UDF4, UDF5, UDF6, UDF7, UDF8, UDF9, UDF10, Show_payment_mode, split_payments, sub_merchant_id);

                enq.FYP_Tot_Amount = totpayableamount;
                enq.FYP_Tot_Amount = totpayableamount;
                //enq.strdatanew = strForm;

                if (enq.FYP_PayModeType == "Portal")
                {
                    get_grp_reptno(enq);
                }
                else if (enq.FYP_PayModeType == "MOBILE")
                {
                    get_grp_reptno_Mobile(enq);
                }
                enq.strdatanew = strForm;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return enq;
        }
        public PaymentDetails.easybuzz getpaymentresponseeasybuzz(PaymentDetails.easybuzz response)
        {

            FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
            PaymentDetails dto = new PaymentDetails();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            if (response.status == "success")
            {
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(q => q.MI_Id == Convert.ToInt64(response.UDF3) && q.FPGD_PGName == "EASEBUZZ").Distinct().ToList();

                string txnid = response.txnid.ToString();
                string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string env = "prod";
                string paymentref = "";
                Easebuzz t = new Easebuzz(secret, key, env);
                // Easebuzz t = new Easebuzz();
                var res = t.transactionAPI(txnid, response.amount, response.email, response.phone);

                JObject joResponse1 = JObject.Parse(res);
                // JArray array1 = (JArray)joResponse1["msg"];
                //JArray array1=(JArray)joResponse1["msg"];


                //foreach (JObject root1 in array1)
                //{
                paymentref = joResponse1["msg"]["easepayid"].ToString();

                // }



                var groups = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                              where (a.MI_Id == Convert.ToInt32(response.UDF3) && a.ASMAY_ID == Convert.ToInt32(response.UDF4) && a.fyp_transaction_id == response.txnid)
                              select new FeeStudentTransactionDTO
                              {
                                  FYP_Receipt_No = a.FYP_Receipt_No
                              }
                      ).Distinct().ToList();

                if (groups.Count == 0)
                {
                    // public string insertdatainfeetables(string miid, string termid, string studentid, string classid, decimal amount, string transid, string refid, string yearid, DateTime indianTime, string transactionid)

                    var confirmstatus = insertdatainfeetables(response.UDF3, response.UDF5.ToString(), response.UDF1.ToString(), response.productinfo.ToString(), Convert.ToDecimal(response.amount), response.txnid, paymentref.ToString(), response.UDF4, indianTime, "0");

                    if (Convert.ToInt32(confirmstatus) > 0)
                    {
                        pgmod.MI_Id = Convert.ToInt64(response.UDF3);
                        pgmod.amst_mobile = Convert.ToInt64(response.phone);
                        pgmod.Amst_Id = Convert.ToInt64(response.UDF3);
                        pgmod.amst_email_id = response.email;

                        SMS sms = new SMS(_context);

                        sms.sendSms(Convert.ToInt32(response.UDF3), Convert.ToInt64(response.phone), "FEEONLINEPAYMENT", Convert.ToInt32(response.UDF2));

                        Email Email = new Email(_context);

                        Email.sendmail(Convert.ToInt32(response.UDF3), response.email, "FEEONLINEPAYMENT", Convert.ToInt32(response.UDF2));

                    }
                }
            }
            else
            {
                SMS sms = new SMS(_context);

                sms.sendSms(Convert.ToInt64(response.UDF4), Convert.ToInt64(response.phone), "FEEONLINEPAYMENTFAIL", Convert.ToInt64(response.UDF2));

                Email Email = new Email(_context);

                Email.sendmail(Convert.ToInt64(response.UDF4), response.email, "FEEONLINEPAYMENTFAIL", Convert.ToInt64(response.UDF2));

                dto.status = response.status;
            }

            return response;
        }

        //easebuzz settlement
        public FeeStudentTransactionDTO Easebuzzsettlementresponse(FeeStudentTransactionDTO data)
        {
            try
            {
                var masterinstitution = _FeeGroupHeadContext.master_institution.Where(z => z.MI_ActiveFlag == 1 && z.MI_SchoolCollegeFlag == "S").ToList();

                if (masterinstitution.Count > 0)
                {
                    for (int z = 0; z < masterinstitution.Count; z++)
                    {
                        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                        List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                        var paymentdet = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(w => w.MI_Id == masterinstitution[z].MI_Id && w.FYP_Bank_Or_Cash == "O" && (w.FYP_Date.ToString("dd-MM-yyyy") == DateTime.Now.ToString("dd-MM-yyyy")) && w.FYP_PayGatewayType == "EASEBUZZ").ToList();
                        if (paymentdet.Count > 0)
                        {
                            // paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(k => k.MI_Id == masterinstitution[z].MI_Id && k.FPGD_PGName == "EASEBUZZ").Distinct().ToList();
                            paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(k => k.MI_Id == masterinstitution[z].MI_Id && k.FPGD_PGName == "EASEBUZZ").Distinct().ToList();
                            if (paymentdetails.Count > 0)
                            {
                                var ASMAY_Id = _FeeGroupHeadContext.AcademicYear.Where(k => k.MI_Id.Equals(masterinstitution[z].MI_Id) && k.Is_Active == true && Convert.ToDateTime(k.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(k.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();


                                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                                string env = "prod";
                                Easebuzz t = new Easebuzz(key, secret, env);

                                // Required parameters in transactionDateAPI function
                                var transdate = DateTime.Now.ToString("17-06-2023");
                                //var transdate = DateTime.Now.ToString("dd-MM-yyyy");
                                // string merchant_email = "Rameshbabu@vapstech.com";
                                string merchant_email = masterinstitution[z].MI_PGRegisteredEmailId;

                                string transaction_date = transdate;


                                //var myContent = t.transactionDateAPI(merchant_email, transaction_date);
                                var myContent = t.payoutAPI(merchant_email, transdate);

                                JObject joResponse1 = JObject.Parse(myContent);
                                JArray array1 = (JArray)joResponse1["payouts_history_data"];

                                if (paymentdet.Count > 0)
                                {
                                    for (var u = 0; u < paymentdet.Count; u++)
                                    {

                                        foreach (JObject root1 in array1)
                                        {

                                            JArray array2 = (JArray)root1["peb_transactions"];
                                            JArray array3 = (JArray)root1["split_payouts"];

                                            Fee_Payment_Overall_Settlement_DetailsDMO obj1 = new Fee_Payment_Overall_Settlement_DetailsDMO();
                                            obj1.MI_Id = masterinstitution[z].MI_Id;
                                            obj1.ASMAY_Id = ASMAY_Id;


                                            obj1.FYPPST_Settlement_Id = (String)(root1["payout_id"]);


                                            obj1.FYPPST_Settlement_Date = indianTime;

                                            obj1.FYPPST_Settlement_Amount = (long)(root1["payout_amount"]);

                                            // obj1.User_id = Convert.ToInt32(useridss);


                                            obj1.FYPPST_UTR_No = (String)(root1["payout_id"]);


                                            string txnid = "";
                                            string transaction_type = "";


                                            foreach (JObject root2 in array2)
                                            {
                                                txnid = (String)root2["txnid"];
                                                transaction_type = (String)root2["transaction_type"];


                                                if (txnid == paymentdet[u].fyp_transaction_id)
                                                {

                                                    _FeeGroupHeadContext.Add(obj1);
                                                    foreach (JObject root3 in array3)
                                                    {
                                                        Fee_Payment_Settlement_DetailsDMO obj2 = new Fee_Payment_Settlement_DetailsDMO();
                                                        obj2.FYPPSD_PAYU_Id = (String)(root2["peb_transaction_id"]);
                                                        string FYPPSD_Transaction_amount = (String)(root3["payout_amount"]);

                                                        FYPPSD_Transaction_amount = FYPPSD_Transaction_amount.Replace(".00", "");

                                                        obj2.FYPPSD_Transaction_amount = Convert.ToInt64(FYPPSD_Transaction_amount);

                                                        obj2.FYPPSD_Payment_Id = txnid;

                                                        obj2.FYPPSD_Payment_Mode = transaction_type;
                                                        obj2.FYPPSD_Payment_Status = "Sucess";
                                                        System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
                                                        // string FYPPSD_Transaction_Date = (String)(root2["payout_date"]);
                                                        obj2.FYPPSD_Transaction_Date = (DateTime)root3["payout_date"];


                                                        string FYPPSD_Settled_amount = (String)(root3["payout_amount"]);
                                                        FYPPSD_Settled_amount = FYPPSD_Settled_amount.Replace(".00", "");

                                                        obj2.FYPPSD_Payment_Amount = Convert.ToInt64(FYPPSD_Settled_amount);
                                                        obj2.FYPPST_Id = obj1.FYPPST_Id;

                                                        // utrrno = (String)(root2["split_payout_id"]);

                                                        _FeeGroupHeadContext.Add(obj2);
                                                    }
                                                }
                                            }



                                        }





                                    }

                                }
                                var contactExists = _FeeGroupHeadContext.SaveChanges();
                                if (contactExists >= 1)
                                {
                                    // data.status = "success";
                                }
                                else
                                {
                                    // data.status = "Failure";
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //easebuzz split payment 
        public FeeStudentTransactionDTO paymentPartEasyBuzzSplit(FeeStudentTransactionDTO enq)
        {
            long totconclist = 0;
            long yrid = getacademicyearcongig(enq);
            string termlst = "0";
            long fmttotal = 0;
            enq.ASMAY_Id = yrid;

            List<multipleaccountscashfree> mulacc = new List<multipleaccountscashfree>();
            long clsid = enq.ASMCL_ID;
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


            var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                            from b in _FeeGroupHeadContext.AdmissionStudentDMO
                            where (a.AMST_Id == b.AMST_Id && a.AMST_Id == enq.Amst_Id && a.AMAY_ActiveFlag == 1)
                            select new FeeStudentTransactionDTO
                            {
                                AMST_ECSFlag = b.AMST_ECSFlag,
                                AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) +
                                        (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) +
                                        (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim(),
                                AMST_AdmNo = b.AMST_AdmNo,
                                amst_mobile = b.AMST_MobileNo,
                                amst_email_id = b.AMST_emailId
                            }
).Distinct().ToArray();

            int ecsflag = 0;
            for (int s = 0; s < fetchecs.Count(); s++)
            {
                if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                {
                    ecsflag = 0;
                }
                else
                {
                    ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                }
            }


            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();

            long fineamt = 0, totalfine = 0;
            try
            {
                Payment pay = new Payment(_context);
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;
                long totalfineamt = 0;
                string termids = "0", grpidss = "0";

                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == enq.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                             ).FirstOrDefault();

                enq.FSS_FineAmount = 0;
                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                for (int l = 0; l < enq.selected_list.Length; l++)
                {
                    totalfine = 0;
                    string fpgdids = "";
                    List<long> trm_ids = new List<long>();
                    foreach (var x in enq.selected_list[l].trm_list)
                    {
                        termids = termids + ',' + x.FMT_Id;
                        trm_ids.Add(x.FMT_Id);
                        termlst = termlst + "," + x.FMT_Id;

                    }

                    enq.ftiidss = termids;

                    var grp_ids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id);
                    var grp_ids1 = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id).ToArray();
                    fineamt = 0;
                    for (int r = 0; r < grp_ids1.Count(); r++)
                    {
                        grpidss = grpidss + ',' + grp_ids1[r];
                    }

                    enq.grpidss = grpidss;

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();
                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }
                    else
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID && trm_ids.Contains(a.fmt_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();

                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }

                    List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                    List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }


                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }


                    List<FeeStudentTransactionDTO> list123 = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list456 = new List<FeeStudentTransactionDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list123 = list;


                        var list1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                     from g in _FeeGroupHeadContext.feeYCC
                                     from h in _FeeGroupHeadContext.feeYCCC
                                     from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                     from j in _FeeGroupHeadContext.Yearlygroups
                                     where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0 && j.FMG_Id == f.FMG_Id && j.ASMAY_Id == enq.ASMAY_Id && j.FYG_RebateApplicableFlg == true)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMA_Id = b.FMA_Id,
                                         FSS_ToBePaid = i.FSS_ToBePaid,
                                         FSS_FineAmount = i.FSS_FineAmount,
                                         FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                         FSS_WaivedAmount = i.FSS_WaivedAmount,
                                         FMG_Id = b.FMG_Id,
                                         FMH_Id = b.FMH_Id,
                                         FTI_Id = b.FTI_Id,
                                         FSS_PaidAmount = i.FSS_PaidAmount,
                                         FSS_NetAmount = i.FSS_NetAmount,
                                         FSS_RefundAmount = i.FSS_RebateAmount,
                                         FMH_FeeName = c.FMH_FeeName,
                                         FTI_Name = d.FTI_Name,
                                         FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                         FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                     }
                    ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list456 = list1;
                    }
                    else
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list123 = list;
                        var list1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                     from g in _FeeGroupHeadContext.feeYCC
                                     from h in _FeeGroupHeadContext.feeYCCC
                                     from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                     from j in _FeeGroupHeadContext.Yearlygroups
                                     where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0 && j.FMG_Id == f.FMG_Id && j.ASMAY_Id == enq.ASMAY_Id && j.FYG_RebateApplicableFlg == true)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMA_Id = b.FMA_Id,
                                         FSS_ToBePaid = i.FSS_ToBePaid,
                                         FSS_FineAmount = i.FSS_FineAmount,
                                         FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                         FSS_WaivedAmount = i.FSS_WaivedAmount,
                                         FMG_Id = b.FMG_Id,
                                         FMH_Id = b.FMH_Id,
                                         FTI_Id = b.FTI_Id,
                                         FSS_PaidAmount = i.FSS_PaidAmount,
                                         FSS_NetAmount = i.FSS_NetAmount,
                                         FSS_RefundAmount = i.FSS_RebateAmount,
                                         FMH_FeeName = c.FMH_FeeName,
                                         FTI_Name = d.FTI_Name,
                                         FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                         FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                     }
       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list456 = list1;

                    }


                    foreach (var x in list456)
                    {
                        fmttotal = fmttotal + x.FSS_CurrentYrCharges;
                    }

                    try
                    {
                        using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd1.CommandText = "Readmissionfeeschecking";
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.BigInt)
                            {
                                Value = enq.MI_Id
                            });

                            cmd1.Parameters.Add(new SqlParameter("@Asmay_id",
                             SqlDbType.BigInt)
                            {
                                Value = enq.ASMAY_Id
                            });


                            cmd1.Parameters.Add(new SqlParameter("@AMST_Id",
                            SqlDbType.VarChar)
                            {
                                Value = enq.Amst_Id
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
                                enq.Readmissionfeeschecking = retObject1.ToArray();
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
                    if (enq.Readmissionfeeschecking.Length > 0)
                    {
                        var readmssionflg = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from b in _FeeGroupHeadContext.FeeHeadDMO

                                             where (a.AMST_Id == enq.Amst_Id
                                             && a.ASMAY_Id == enq.ASMAY_Id && a.MI_Id == enq.MI_Id && a.FMH_Id == b.FMH_Id && b.FMH_Flag == "RA" && a.FSS_ToBePaid > 0)
                                             select a.AMST_Id
).Distinct().ToList();

                            foreach (var x in list123)
                            {
                                amount_list.Add(x);
                            totconclist = totconclist + x.FSS_ConcessionAmount;

                            if ((enq.Readmissionfeeschecking == null || enq.Readmissionfeeschecking.Length == 0) || (readmssionflg.Count == 0))
                            {
                                

                                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                {
                                    if (ecsflag.Equals(0))
                                    {
                                        cmd.CommandText = "Sp_Calculate_Fine";
                                    }
                                    else
                                    {
                                        cmd.CommandText = "Sp_Calculate_Fine_ECS";
                                    }
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@On_Date",
                                        SqlDbType.DateTime, 100)
                                    {
                                        Value = indianTime
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@fma_id",
                                       SqlDbType.NVarChar, 100)
                                    {
                                        Value = x.FMA_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                                   SqlDbType.NVarChar, 100)
                                    {
                                        Value = enq.ASMAY_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@amt",
                        SqlDbType.Decimal, 500)
                                    {
                                        Direction = ParameterDirection.Output
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@flgArr",
                       SqlDbType.Int, 500)
                                    {
                                        Direction = ParameterDirection.Output
                                    });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var data1 = cmd.ExecuteNonQuery();

                                    var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                         where (a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == enq.Amst_Id)
                                                         select new FeeStudentTransactionDTO
                                                         {
                                                             FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                             FSWO_FineFlg = a.FSWO_FineFlg,
                                                             FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                         }
                    ).Distinct().ToList();

                                    if (waivedofffine.Count() > 0)
                                    {
                                        if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                        {
                                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                                totalfine = totalfine + fineamt;
                                            }
                                        }
                                        else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                        {
                                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamt = 0;
                                                totalfine = totalfine + fineamt;
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                                totalfine = totalfine + fineamt;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                        {
                                            fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                            totalfine = totalfine + fineamt;
                                        }
                                    }

                                }

                                if (Convert.ToInt32(fineamt) > 0)
                                {
                                    if (fineheadfmaid.Count() > 0)
                                    {
                                        finelst.Add(new FeeStudentTransactionDTO
                                        {
                                            FMA_Id = fineheadfmaid[0].FMA_Id,
                                            FSS_ToBePaid = Convert.ToInt32(fineamt),
                                            FSS_FineAmount = 0,
                                            FSS_ConcessionAmount = 0,
                                            FSS_WaivedAmount = 0,
                                            FMG_Id = 0,
                                            FMH_Id = 0,
                                            FTI_Id = 0,
                                            FSS_PaidAmount = 0,
                                            FSS_NetAmount = 0,
                                            FSS_RefundAmount = 0,
                                            FMH_FeeName = "Fine",
                                            FTI_Name = "Anytime",
                                            FSS_CurrentYrCharges = 0,
                                            FSS_TotalToBePaid = 0,
                                            merchantid = fineheadfmaid[0].merchantid,
                                            FYP_Id = 0
                                        });
                                    }

                                }

                                totalfineamt += fineamt;
                                fineamt = 0;

                            }


                        }
                    }
                    else
                    {
                        foreach (var x in list123)
                        {
                            amount_list.Add(x);
                            totconclist = totconclist + x.FSS_ConcessionAmount;
                            amount_list.Add(x);
                            totconclist = totconclist + x.FSS_ConcessionAmount;
                            using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                            {
                                if (ecsflag.Equals(0))
                                {
                                    cmd.CommandText = "Sp_Calculate_Fine";
                                }
                                else
                                {
                                    cmd.CommandText = "Sp_Calculate_Fine_ECS";
                                }
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@On_Date",
                                    SqlDbType.DateTime, 100)
                                {
                                    Value = indianTime
                                });

                                cmd.Parameters.Add(new SqlParameter("@fma_id",
                                   SqlDbType.NVarChar, 100)
                                {
                                    Value = x.FMA_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@asmay_id",
                               SqlDbType.NVarChar, 100)
                                {
                                    Value = enq.ASMAY_Id
                                });

                                cmd.Parameters.Add(new SqlParameter("@amt",
                    SqlDbType.Decimal, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                cmd.Parameters.Add(new SqlParameter("@flgArr",
                   SqlDbType.Int, 500)
                                {
                                    Direction = ParameterDirection.Output
                                });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var data1 = cmd.ExecuteNonQuery();

                                var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                     where (a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == enq.Amst_Id)
                                                     select new FeeStudentTransactionDTO
                                                     {
                                                         FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                         FSWO_FineFlg = a.FSWO_FineFlg,
                                                         FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                     }
                ).Distinct().ToList();

                                if (waivedofffine.Count() > 0)
                                {
                                    if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                    {
                                        if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                        {
                                            fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                            totalfine = totalfine + fineamt;
                                        }
                                    }
                                    else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                    {
                                        if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                        {
                                            fineamt = 0;
                                            totalfine = totalfine + fineamt;
                                        }
                                    }
                                    else
                                    {
                                        if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                        {
                                            fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                            totalfine = totalfine + fineamt;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                        totalfine = totalfine + fineamt;
                                    }
                                }

                            }

                            if (Convert.ToInt32(fineamt) > 0)
                            {
                                if (fineheadfmaid.Count() > 0)
                                {
                                    finelst.Add(new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = fineheadfmaid[0].FMA_Id,
                                        FSS_ToBePaid = Convert.ToInt32(fineamt),
                                        FSS_FineAmount = 0,
                                        FSS_ConcessionAmount = 0,
                                        FSS_WaivedAmount = 0,
                                        FMG_Id = 0,
                                        FMH_Id = 0,
                                        FTI_Id = 0,
                                        FSS_PaidAmount = 0,
                                        FSS_NetAmount = 0,
                                        FSS_RefundAmount = 0,
                                        FMH_FeeName = "Fine",
                                        FTI_Name = "Anytime",
                                        FSS_CurrentYrCharges = 0,
                                        FSS_TotalToBePaid = 0,
                                        merchantid = fineheadfmaid[0].merchantid,
                                        FYP_Id = 0
                                    });
                                }

                            }

                            totalfineamt += fineamt;
                            fineamt = 0;
                        }
                    }

                    string grp = "";
                    grp = "0";
                    if (grp_ids != null)
                    {
                        foreach (var i in grp_ids)
                        {
                            grp = grp + "," + i;
                        }
                    }
                    string term = "";
                    term = "0";
                    if (trm_ids != null)
                    {
                        foreach (var i in trm_ids)
                        {
                            term = term + "," + i;
                        }
                    }
                    List<multipleaccountscashfree> employeedetails = new List<multipleaccountscashfree>();
                    
                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Feeaccountdetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@AMST_id",
                                SqlDbType.VarChar)
                        {
                            Value = enq.Amst_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAYID",
                                SqlDbType.VarChar)
                        {
                            Value = enq.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FMTID",
                                SqlDbType.VarChar)
                        {
                            Value = term
                        });

                        cmd.Parameters.Add(new SqlParameter("@FMGGID",
                                SqlDbType.VarChar)
                        {
                            Value = grp
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.VarChar)
                        {
                            Value = enq.MI_Id
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
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                        );
                                    }



                                    employeedetails.Add(new multipleaccountscashfree
                                    {
                                        amount = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),
                                        vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                    });

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    //}

                    if (employeedetails.Count() > 0)
                    {
                        foreach (var q in employeedetails)
                        {
                            mulacc.Add(q);
                        }
                    }

                    //

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }
                    else
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }
                    var Euserid = (from a in _FeeGroupHeadContext.FeeGroupDMO
                                   where (a.MI_Id == Convert.ToInt64(enq.MI_Id) && grp_ids.Contains(a.FMG_Id))
                                   select new FeeStudentTransactionDTO
                                   {
                                       enduserid = a.user_id,
                                   }
                                   
).Distinct().Take(1).ToArray();

                    enq.userid = Convert.ToInt64(Euserid[0].enduserid);


                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (Convert.ToInt32(enq.pendingamount) + totalfineamt).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });

                    enq.FSS_FineAmount = enq.FSS_FineAmount + totalfineamt;
                }

                enq.amount_list = amount_list.ToArray();

                if (finelst.Count() > 0)
                {
                    foreach (var y in finelst)
                    {
                        amount_list.Add(y);
                    }
                }
                enq.Totalconcession = totconclist;
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();



                //added by kavita//
                if (readterms.IVRMGC_Classwise_Payment != "1")
                {
                    paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

                }
                else
                {
                    var onlinepayment = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping

                                         from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new FeeStudentTransactionDTO
                                         {
                                             FPGD_SubMerchantId = c.FPGD_SubMerchantId
                                         }
 ).ToList();

                    if (onlinepayment.Count > 0)
                    {

                        paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway && t.FPGD_SubMerchantId == onlinepayment[0].FPGD_SubMerchantId).Distinct().ToList();

                    }


                }

                //added by kavita//

                //    paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }

                var item = new
                {
                    paymentParts = result
                };



                var cnt2 = _context.FeeMasterConfigurationDMO.Where(t => t.MI_Id == enq.MI_Id && t.FMC_RebateAplicableFlg == true && t.FMC_RebateAgainstFullPaymentFlg == true && t.FMC_RebateAgainstPartialPaymentFlg == true).ToList();



                string rebateamount;

                if (cnt2.Count > 0)
                {
                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fee_RebateTermWise_calculation_BOTH";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.VarChar)
                        {
                            Value = enq.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.VarChar)
                        {
                            Value = enq.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMT_ID",
                      SqlDbType.VarChar)
                        {
                            Value = termlst
                        });
                        cmd.Parameters.Add(new SqlParameter("@paiddate",
                      SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
                        });
                        cmd.Parameters.Add(new SqlParameter("@paidamount",
                       SqlDbType.VarChar)
                        {
                            Value = fmttotal
                        });

                        cmd.Parameters.Add(new SqlParameter("@USERID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
            SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                    }



                }
                else
                {
                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fee_RebateTermWise_calculation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.VarChar)
                        {
                            Value = enq.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.VarChar)
                        {
                            Value = enq.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMT_ID",
                      SqlDbType.VarChar)
                        {
                            Value = termlst
                        });
                        cmd.Parameters.Add(new SqlParameter("@paiddate",
                      SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
                        });
                        cmd.Parameters.Add(new SqlParameter("@paidamount",
                       SqlDbType.VarChar)
                        {
                            Value = fmttotal
                        });

                        cmd.Parameters.Add(new SqlParameter("@USERID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
            SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                    }

                }

                enq.FSS_RebateAmount = Convert.ToInt64(rebateamount);


                //rebate






                decimal amt = Convert.ToDecimal(totpayableamount - enq.FSS_RebateAmount);
                string hashkey = "key|txnid|Amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10|salt";
                string payinfo = JsonConvert.SerializeObject(item);
                string orderId;
                string amount = (amt + ".0").ToString();
                string firstname = fetchecs[0].AMST_FirstName;
                string email = fetchecs[0].amst_email_id;
                string phone = (fetchecs[0].amst_mobile).ToString();

                string surl = "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponseeasybuzz/";
                string furl = "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponseeasybuzz/";



                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    enq.trans_id = "EASEBUZZ" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();

                }
                string Txnid = (enq.trans_id).ToString();
                string UDF1 = (enq.Amst_Id).ToString();

                string UDF2 = (enq.userid).ToString();

                string UDF3 = (enq.MI_Id).ToString();

                string UDF4 = (enq.ASMAY_Id).ToString();

                string UDF5 = (enq.ftiidss).ToString();

                string UDF6 = "";

                string UDF7 = "";
                string UDF8 = "";
                string UDF9 = "";
                string UDF10 = "";
                string productinfo = (enq.ASMCL_ID).ToString();
                string Show_payment_mode = "";

                Dictionary<string, decimal> transfersnotescash = new Dictionary<string, decimal>();
                string split = "";
                var z = 0;
                if (mulacc.Count > 0)
                {
                    split += "{";

                    for (var j = 0; j < mulacc.Count; j++)
                    {
                        if (j == 0)
                        {
                            if (totalfineamt > 0)
                            {
                                split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                                transfersnotescash.Add(mulacc[j].vendor_id.ToString(), Convert.ToDecimal((mulacc[j].amount + totalfineamt) - enq.FSS_RebateAmount));
                            }
                            else
                            {
                                split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                                transfersnotescash.Add(mulacc[j].vendor_id.ToString(), Convert.ToDecimal((mulacc[j].amount - enq.FSS_RebateAmount)));
                            }
                        }
                        else
                        {
                            split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                            transfersnotescash.Add(mulacc[j].vendor_id.ToString(), Convert.ToDecimal(mulacc[j].amount));
                        }



                    }

                    split = split.Substring(0, (split.Length - 1));




                    split += "}";
                }
                var myContent = JsonConvert.SerializeObject(transfersnotescash);
                String postData = myContent;
                //String postData = res;
                string split_payments = postData;
                //string split_payments = split.ToString();
                string sub_merchant_id = paymentdetails.FirstOrDefault().FPGD_AccNo;

                string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string env = "prod";
                Easebuzz t1 = new Easebuzz(secret, key, env);
                //split_payments = "";
                // sub_merchant_id = "";


                string strForm = t1.initiatePaymentAPI(amount, firstname, email, phone, productinfo, surl, furl, Txnid, UDF1, UDF2, UDF3, UDF4, UDF5, UDF6, UDF7, UDF8, UDF9, UDF10, Show_payment_mode, split_payments, sub_merchant_id);

                enq.FYP_Tot_Amount = totpayableamount;
                enq.FYP_Tot_Amount = totpayableamount;
                enq.strdatanew = strForm;

                if (enq.FYP_PayModeType == "Portal")
                {
                    get_grp_reptno(enq);
                }
                else if (enq.FYP_PayModeType == "MOBILE")
                {
                    get_grp_reptno_Mobile(enq);
                }
                enq.strdatanew = strForm;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return enq;
        }

        //EASEBUZZ


        //Razorpay API

        public FeeStudentTransactionDTO RazorpayApi(FeeStudentTransactionDTO response)
        {
            try
            {
                PaymentDetails response1 = new PaymentDetails();
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();




                string id = response.searchtext;



                string method = "GET";
                string url = "https://api.razorpay.com/v1/orders/" + id;

                // string url = "https://api.razorpay.com/v1/orders/ID/payments";
                url = url.Replace("ID", id);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToString();
                request.ContentLength = 0;
                request.ContentType = "application/json";

                string userAgent = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                request.UserAgent = "razorpay-dot-net/" + userAgent;

                string authString = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(authString));

                System.Net.WebResponse resp = request.GetResponseAsync().Result;
                System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                string s = sr.ReadToEnd().Trim();
                JObject joResponse1 = JObject.Parse(s);
                var payout = joResponse1["status"].ToString();


                if (payout == "paid")
                {



                    string method1 = "GET";
                    string url1 = "https://api.razorpay.com/v1/orders/ID/payments";
                    url1 = url1.Replace("ID", id);
                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url1);
                    request1.Method = method.ToString();
                    request1.ContentLength = 0;
                    request1.ContentType = "application/json";

                    string userAgent1 = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                    request.UserAgent = "razorpay-dot-net/" + userAgent1;

                    string authString1 = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                    request1.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(authString1));

                    System.Net.WebResponse resp1 = request1.GetResponseAsync().Result;
                    System.IO.StreamReader sr1 = new System.IO.StreamReader(resp1.GetResponseStream());
                    string s1 = sr1.ReadToEnd().Trim();
                    JObject joResponse2 = JObject.Parse(s1);
                    JArray array2 = (JArray)joResponse2["items"];
                    foreach (JObject root2 in array2)
                    {
                        if ((String)root2["status"] == "captured")
                        {
                            response.FYPPM_PaymentReferenceId = (String)root2["id"];
                            response.status = (String)root2["status"];
                            //  razorresponse(response1);
                        }
                    }

                }
                else if (payout != "Paid")
                {
                    string method1 = "GET";
                    string url1 = "https://api.razorpay.com/v1/orders/ID/payments";
                    url1 = url1.Replace("ID", id);
                    HttpWebRequest request1 = (HttpWebRequest)WebRequest.Create(url1);
                    request1.Method = method.ToString();
                    request1.ContentLength = 0;
                    request1.ContentType = "application/json";

                    string userAgent1 = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                    request.UserAgent = "razorpay-dot-net/" + userAgent1;

                    string authString1 = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                    request1.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                        Encoding.UTF8.GetBytes(authString1));

                    System.Net.WebResponse resp1 = request1.GetResponseAsync().Result;
                    System.IO.StreamReader sr1 = new System.IO.StreamReader(resp1.GetResponseStream());
                    string s1 = sr1.ReadToEnd().Trim();
                    JObject joResponse2 = JObject.Parse(s1);
                    JArray array2 = (JArray)joResponse2["items"];
                    foreach (JObject root2 in array2)
                    {
                        //if ((String)root2["status"] == "captured")
                        //{
                        response.FYPPM_PaymentReferenceId = (String)root2["id"];
                        response.status = (String)root2["status"];
                        //  razorresponse(response1);
                        //}
                    }

                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }



        public FeeStudentTransactionDTO Easebuzzpaymentsplitresponse(FeeStudentTransactionDTO data)
        {
            try
            {


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(k => k.MI_Id == data.MI_Id && k.FPGD_PGName == data.FMOT_PayGatewayType).Distinct().ToList();

                data.ASMAY_Id = _FeeGroupHeadContext.AcademicYear.Where(k => k.MI_Id.Equals(data.MI_Id) && k.Is_Active == true && Convert.ToDateTime(k.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(k.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var masterinstitution = _FeeGroupHeadContext.master_institution.Where(z => z.MI_ActiveFlag == 1 && z.MI_Id == data.MI_Id).ToList();



                if (data.FMOT_PayGatewayType == "EASEBUZZ")
                {
                    string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                    string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                    string env = "prod";
                    Easebuzz t = new Easebuzz(key, secret, env);


                    var transdate = DateTime.Now.ToString("dd-MM-yyyy");


                    string merchant_email = "";
                    string transaction_date = transdate;


                    var myContent = t.transactionDateAPI(merchant_email, transaction_date);
                    //var myContent = t.payoutAPI("rameshbabu@vapstech.com", "06-03-2023");

                    JObject joResponse1 = JObject.Parse(myContent);
                    JArray array1 = (JArray)joResponse1["payouts_history_data"];


                    foreach (JObject root1 in array1)
                    {

                        JArray array2 = (JArray)root1["peb_transactions"];
                        JArray array3 = (JArray)root1["split_payouts"];

                        foreach (JObject root2 in array2)
                        {
                            string transid = (String)(root2["txnid"]);
                            string type = (String)(root2["transaction_type"]);
                            JArray array4 = (JArray)root2["split_transactions"];


                            var paymentgateway = _FeeGroupHeadContext.PAYUDETAILS.Where(p => p.IMPG_PGFlag == "EASEBUZZ" && p.IMPG_ActiveFlg == true).ToList();
                            var paymentgatewayrate = _FeeGroupHeadContext.Fee_PaymentGateway_RateDMO.Where(q => q.MI_Id == data.MI_Id && q.FPGR_CardType == type).ToList();

                            var paymentlist = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(z => z.MI_Id == data.MI_Id && z.fyp_transaction_id == transid).ToList();

                            foreach (JObject root4 in array4)
                            {

                                if (paymentlist.Count == 1)
                                {

                                    Fee_T_Payment_OnlineDMO obj1 = new Fee_T_Payment_OnlineDMO();
                                    obj1.FYP_Id = paymentlist[0].FYP_Id;
                                    obj1.FTPONLINE_FYP_Id = 0;

                                    obj1.IMPG_Id = paymentgateway[0].IMPG_Id;
                                    obj1.FTPONLINE_OnlineTransactionId = transid;
                                    obj1.FTPONLINE_TotalFeeAmount = (decimal)(root1["payout_amount"]);
                                    obj1.FTPONLINE_Remarks = "Easebuzz Payment";
                                    obj1.FTPONLINE_CardType = type;

                                    obj1.FTPONLINE_TDRFee = (decimal)(root1["peb_service_charge"]);
                                    obj1.FTPONLINE_TDRTax = (decimal)(root1["peb_service_tax"]);

                                    obj1.FTPONLINE_TotalTDRAmount = ((decimal)(root1["peb_service_charge"]) + (decimal)(root1["peb_service_tax"]));

                                    if (type == "UPI" || type == "upi")
                                    {
                                        obj1.FTPONLINE_CardNetworkType = type;
                                        obj1.FTPONLINE_VAPSAmount = 0;
                                    }
                                    else if (type == "card")
                                    {
                                        obj1.FTPONLINE_CardNetworkType = (string)root1["card"]["network"];
                                        obj1.FTPONLINE_CardType = (string)root1["card"]["type"];
                                        obj1.FTPONLINE_VAPSAmount = 0;
                                    }
                                    else if (type == "CreditCard" || type == "creditcard")
                                    {
                                        obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                        obj1.FTPONLINE_VAPSAmount = 0;

                                    }
                                    else if (type == "DebitCard" || type == "debitcard")
                                    {
                                        obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                        obj1.FTPONLINE_VAPSAmount = 0;

                                    }
                                    else if (type == "Netbanking" || type == "netbanking")
                                    {
                                        obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                        obj1.FTPONLINE_VAPSAmount = 0;

                                    }
                                    else
                                    {
                                        obj1.FTPONLINE_CardNetworkType = type;
                                        obj1.FTPONLINE_VAPSAmount = 0;
                                    }
                                    _FeeGroupHeadContext.Fee_T_Payment_OnlineDMO.Add(obj1);
                                    var contactExists1 = _FeeGroupHeadContext.SaveChanges();
                                    if (contactExists1 >= 1)
                                    {
                                        data.status = "success";
                                    }
                                    else
                                    {
                                        data.status = "Failure";
                                    }


                                }
                                else if (paymentlist.Count > 1)
                                {
                                    long fypidnew = 0;
                                    for (int i = 0; i < paymentlist.Count; i++)
                                    {

                                        if (i == 0)
                                        {
                                            var feedet = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(k => k.MI_Id == data.MI_Id && k.fyp_transaction_id == transid && k.FYP_Id == paymentlist[i].FYP_Id).ToList();

                                            fypidnew = feedet[0].FYP_Id;



                                            Fee_T_Payment_OnlineDMO obj1 = new Fee_T_Payment_OnlineDMO();
                                            obj1.FYP_Id = paymentlist[0].FYP_Id;
                                            obj1.FTPONLINE_FYP_Id = 0;

                                            obj1.IMPG_Id = paymentgateway[0].IMPG_Id;
                                            obj1.FTPONLINE_OnlineTransactionId = transid;
                                            obj1.FTPONLINE_TotalFeeAmount = (decimal)(root4["amount"]);
                                            obj1.FTPONLINE_Remarks = "Easebuzz Payment";
                                            obj1.FTPONLINE_CardType = type;

                                            obj1.FTPONLINE_TDRFee = (decimal)(root4["service_charge"]);
                                            obj1.FTPONLINE_TDRTax = (decimal)(root4["service_tax"]);

                                            obj1.FTPONLINE_TotalTDRAmount = ((decimal)(root4["service_charge"]) + (decimal)(root4["service_tax"]));

                                            if (type == "UPI" || type == "upi")
                                            {
                                                obj1.FTPONLINE_CardNetworkType = type;
                                                obj1.FTPONLINE_VAPSAmount = 0;
                                            }
                                            else if (type == "card")
                                            {
                                                obj1.FTPONLINE_CardNetworkType = (string)root2["card"]["network"];
                                                obj1.FTPONLINE_CardType = (string)root2["card"]["type"];
                                                obj1.FTPONLINE_VAPSAmount = 0;
                                            }
                                            else if (type == "CreditCard" || type == "creditcard" || type == "Credit Card")
                                            {
                                                obj1.FTPONLINE_CardNetworkType = (String)(root2["bank"]);
                                                obj1.FTPONLINE_VAPSAmount = 0;

                                            }
                                            else if (type == "DebitCard" || type == "debitcard" || type == "Debit Card")
                                            {
                                                obj1.FTPONLINE_CardNetworkType = (String)(root2["bank"]);
                                                obj1.FTPONLINE_VAPSAmount = 0;

                                            }
                                            else if (type == "Netbanking" || type == "netbanking")
                                            {
                                                obj1.FTPONLINE_CardNetworkType = (String)(root2["bank"]);
                                                obj1.FTPONLINE_VAPSAmount = 0;

                                            }
                                            else
                                            {
                                                obj1.FTPONLINE_CardNetworkType = type;
                                                obj1.FTPONLINE_VAPSAmount = 0;
                                            }

                                            _FeeGroupHeadContext.Fee_T_Payment_OnlineDMO.Add(obj1);


                                        }
                                        else
                                        {
                                            Fee_T_Payment_OnlineDMO obj1 = new Fee_T_Payment_OnlineDMO();
                                            obj1.FYP_Id = paymentlist[i].FYP_Id;
                                            obj1.FTPONLINE_FYP_Id = fypidnew;

                                            obj1.IMPG_Id = paymentgateway[0].IMPG_Id;
                                            obj1.FTPONLINE_OnlineTransactionId = transid;
                                            obj1.FTPONLINE_TotalFeeAmount = (decimal)(root4["amount"]);
                                            obj1.FTPONLINE_Remarks = "Easebuzz Payment";
                                            obj1.FTPONLINE_CardType = type;

                                            obj1.FTPONLINE_TDRFee = (decimal)(root4["service_charge"]);
                                            obj1.FTPONLINE_TDRTax = (decimal)(root4["service_tax"]);

                                            obj1.FTPONLINE_TotalTDRAmount = ((decimal)(root4["service_charge"]) + (decimal)(root4["service_tax"]));

                                            if (type == "UPI" || type == "upi")
                                            {
                                                obj1.FTPONLINE_CardNetworkType = type;
                                                obj1.FTPONLINE_VAPSAmount = 0;
                                            }
                                            else if (type == "card")
                                            {
                                                obj1.FTPONLINE_CardNetworkType = (string)root2["card"]["network"];
                                                obj1.FTPONLINE_CardType = (string)root2["card"]["type"];
                                                obj1.FTPONLINE_VAPSAmount = 0;
                                            }
                                            else if (type == "CreditCard" || type == "creditcard")
                                            {
                                                obj1.FTPONLINE_CardNetworkType = (String)(root2["bank"]);
                                                obj1.FTPONLINE_VAPSAmount = 0;

                                            }
                                            else if (type == "DebitCard" || type == "debitcard")
                                            {
                                                obj1.FTPONLINE_CardNetworkType = (String)(root2["bank"]);
                                                obj1.FTPONLINE_VAPSAmount = 0;

                                            }
                                            else if (type == "Netbanking" || type == "netbanking")
                                            {
                                                obj1.FTPONLINE_CardNetworkType = (String)(root2["bank"]);
                                                obj1.FTPONLINE_VAPSAmount = 0;

                                            }
                                            else
                                            {
                                                obj1.FTPONLINE_CardNetworkType = type;
                                                obj1.FTPONLINE_VAPSAmount = 0;
                                            }


                                            _FeeGroupHeadContext.Fee_T_Payment_OnlineDMO.Add(obj1);
                                        }



                                    }
                                    var contactExists1 = _FeeGroupHeadContext.SaveChanges();
                                    if (contactExists1 >= 1)
                                    {
                                        data.status = "success";
                                    }
                                    else
                                    {
                                        data.status = "Failure";
                                    }


                                }
                            }

                        }




                    }




                    var contactExists = _FeeGroupHeadContext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        data.status = "success";
                    }
                    else
                    {
                        data.status = "Failure";
                    }
                }
                else if (data.FMOT_PayGatewayType == "RAZORPAY")
                {
                    PaymentDetails response1 = new PaymentDetails();
                    RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);


                    Dictionary<string, object> input = new Dictionary<string, object>();

                    Int32 unixTimestampstart = 0;
                    Int32 unixTimestampend = 0;
                    if (data.date != null && data.date != "" && data.date != "0" && data.date != "1")
                    {
                        //CURRENT DAY - 1 
                        unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddDays(-(Convert.ToInt32(data.date))).AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddDays(-(Convert.ToInt32(data.date) - 1)).AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    }
                    else
                    {

                        unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    }



                    input.Add("from", unixTimestampstart.ToString());
                    input.Add("to", unixTimestampend.ToString());
                    input.Add("count", 100);
                    //input.Add("skip", 0);

                    List<Order> orders = client.Order.All(input);
                    string id = "";
                    foreach (var x in orders)
                    {
                        id = x.Attributes["id"];



                        if (x.Attributes["status"] == "paid")
                        {


                            response1.order_id = id;
                            response1.IVRMOP_MIID = data.MI_Id;

                            //FETCH PAYMENT BASED ON ORDER-ID

                            string method = "GET";
                            string url = "https://api.razorpay.com/v1/orders/ID/payments";
                            url = url.Replace("ID", id);
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                            request.Method = method.ToString();
                            request.ContentLength = 0;
                            request.ContentType = "application/json";

                            string userAgent = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                            request.UserAgent = "razorpay-dot-net/" + userAgent;

                            string authString = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                                Encoding.UTF8.GetBytes(authString));

                            System.Net.WebResponse resp = request.GetResponseAsync().Result;
                            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                            string s = sr.ReadToEnd().Trim();
                            JObject joResponse1 = JObject.Parse(s);
                            JArray array1 = (JArray)joResponse1["items"];
                            foreach (JObject root1 in array1)
                            {
                                if ((String)root1["status"] == "captured")
                                {



                                    string transid = (String)(root1["order_id"]);
                                    string type = (String)(root1["method"]);

                                    var paymentgateway = _FeeGroupHeadContext.PAYUDETAILS.Where(p => p.IMPG_PGFlag == data.FMOT_PayGatewayType && p.IMPG_ActiveFlg == true).ToList();
                                    var paymentgatewayrate = _FeeGroupHeadContext.Fee_PaymentGateway_RateDMO.Where(q => q.MI_Id == data.MI_Id && q.FPGR_CardType == type).ToList();

                                    var paymentlist = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(z => z.MI_Id == data.MI_Id && z.fyp_transaction_id == transid).ToList();

                                    if (paymentlist.Count == 1)
                                    {

                                        Fee_T_Payment_OnlineDMO obj1 = new Fee_T_Payment_OnlineDMO();
                                        obj1.FYP_Id = paymentlist[0].FYP_Id;
                                        obj1.FTPONLINE_FYP_Id = 0;

                                        obj1.IMPG_Id = paymentgateway[0].IMPG_Id;
                                        obj1.FTPONLINE_OnlineTransactionId = transid;
                                        obj1.FTPONLINE_TotalFeeAmount = ((decimal)(root1["amount"])) / 100;
                                        obj1.FTPONLINE_Remarks = "Razorpay Payment";
                                        obj1.FTPONLINE_CardType = type;

                                        obj1.FTPONLINE_TDRFee = ((decimal)(root1["fee"])) / 100;
                                        obj1.FTPONLINE_TDRTax = ((decimal)(root1["tax"])) / 100;

                                        obj1.FTPONLINE_TotalTDRAmount = (((decimal)(root1["fee"])) + ((decimal)(root1["tax"]))) / 100;

                                        if (type == "UPI" || type == "upi")
                                        {
                                            obj1.FTPONLINE_CardNetworkType = type;
                                            obj1.FTPONLINE_VAPSAmount = 0;
                                        }
                                        else if (type == "card")
                                        {
                                            obj1.FTPONLINE_CardNetworkType = (string)root1["card"]["network"];
                                            obj1.FTPONLINE_CardType = (string)root1["card"]["type"];
                                            obj1.FTPONLINE_VAPSAmount = 0;
                                        }
                                        else if (type == "CreditCard" || type == "creditcard")
                                        {
                                            obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                            obj1.FTPONLINE_VAPSAmount = 0;

                                        }
                                        else if (type == "DebitCard" || type == "debitcard")
                                        {
                                            obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                            obj1.FTPONLINE_VAPSAmount = 0;

                                        }
                                        else if (type == "Netbanking" || type == "netbanking")
                                        {
                                            obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                            obj1.FTPONLINE_VAPSAmount = 0;

                                        }
                                        else
                                        {
                                            obj1.FTPONLINE_CardNetworkType = type;
                                            obj1.FTPONLINE_VAPSAmount = 0;
                                        }
                                        _FeeGroupHeadContext.Fee_T_Payment_OnlineDMO.Add(obj1);
                                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                                        if (contactExists >= 1)
                                        {
                                            data.status = "success";
                                        }
                                        else
                                        {
                                            data.status = "Failure";
                                        }


                                    }
                                    else if (paymentlist.Count > 1)
                                    {
                                        long fypidnew = 0;
                                        for (int i = 0; i < paymentlist.Count; i++)
                                        {

                                            if (i == 0)
                                            {
                                                var feedet = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(k => k.MI_Id == data.MI_Id && k.fyp_transaction_id == transid && k.FYP_Id == paymentlist[i].FYP_Id).ToList();

                                                fypidnew = feedet[0].FYP_Id;



                                                Fee_T_Payment_OnlineDMO obj1 = new Fee_T_Payment_OnlineDMO();
                                                obj1.FYP_Id = paymentlist[0].FYP_Id;
                                                obj1.FTPONLINE_FYP_Id = 0;

                                                obj1.IMPG_Id = paymentgateway[0].IMPG_Id;
                                                obj1.FTPONLINE_OnlineTransactionId = transid;
                                                obj1.FTPONLINE_TotalFeeAmount = ((decimal)(root1["amount"])) / 100;
                                                obj1.FTPONLINE_Remarks = "Razorpay Payment";
                                                obj1.FTPONLINE_CardType = type;

                                                obj1.FTPONLINE_TDRFee = ((decimal)(root1["fee"])) / 100;
                                                obj1.FTPONLINE_TDRTax = ((decimal)(root1["tax"])) / 100;

                                                obj1.FTPONLINE_TotalTDRAmount = (((decimal)(root1["fee"])) + ((decimal)(root1["tax"]))) / 100;

                                                if (type == "UPI" || type == "upi")
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = type;
                                                    obj1.FTPONLINE_VAPSAmount = 0;
                                                }
                                                else if (type == "card")
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = (string)root1["card"]["network"];
                                                    obj1.FTPONLINE_CardType = (string)root1["card"]["type"];
                                                    obj1.FTPONLINE_VAPSAmount = 0;
                                                }
                                                else if (type == "CreditCard" || type == "creditcard")
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                                    obj1.FTPONLINE_VAPSAmount = 0;

                                                }
                                                else if (type == "DebitCard" || type == "debitcard")
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                                    obj1.FTPONLINE_VAPSAmount = 0;

                                                }
                                                else if (type == "Netbanking" || type == "netbanking")
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                                    obj1.FTPONLINE_VAPSAmount = 0;

                                                }
                                                else
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = type;
                                                    obj1.FTPONLINE_VAPSAmount = 0;
                                                }

                                                _FeeGroupHeadContext.Fee_T_Payment_OnlineDMO.Add(obj1);


                                            }
                                            else
                                            {
                                                Fee_T_Payment_OnlineDMO obj1 = new Fee_T_Payment_OnlineDMO();
                                                obj1.FYP_Id = paymentlist[i].FYP_Id;
                                                obj1.FTPONLINE_FYP_Id = fypidnew;

                                                obj1.IMPG_Id = paymentgateway[0].IMPG_Id;
                                                obj1.FTPONLINE_OnlineTransactionId = transid;
                                                obj1.FTPONLINE_TotalFeeAmount = ((decimal)(root1["amount"])) / 100;
                                                obj1.FTPONLINE_Remarks = "Razorpay Payment";
                                                obj1.FTPONLINE_CardType = type;
                                                obj1.FTPONLINE_CardNetworkType = type;
                                                obj1.FTPONLINE_TDRFee = 0;
                                                obj1.FTPONLINE_TDRTax = 0;

                                                obj1.FTPONLINE_TotalTDRAmount = 0;

                                                if (type == "UPI" || type == "upi")
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = type;
                                                    obj1.FTPONLINE_VAPSAmount = 0;
                                                }
                                                else if (type == "card")
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = (string)root1["card"]["network"];
                                                    obj1.FTPONLINE_CardType = (string)root1["card"]["type"];
                                                    obj1.FTPONLINE_VAPSAmount = 0;
                                                }
                                                else if (type == "CreditCard" || type == "creditcard")
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                                    obj1.FTPONLINE_VAPSAmount = 0;

                                                }
                                                else if (type == "DebitCard" || type == "debitcard")
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                                    obj1.FTPONLINE_VAPSAmount = 0;

                                                }
                                                else if (type == "Netbanking" || type == "netbanking")
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = (String)(root1["bank"]);
                                                    obj1.FTPONLINE_VAPSAmount = 0;

                                                }
                                                else
                                                {
                                                    obj1.FTPONLINE_CardNetworkType = type;
                                                    obj1.FTPONLINE_VAPSAmount = 0;
                                                }


                                                _FeeGroupHeadContext.Fee_T_Payment_OnlineDMO.Add(obj1);
                                            }



                                        }
                                        var contactExists = _FeeGroupHeadContext.SaveChanges();
                                        if (contactExists >= 1)
                                        {
                                            data.status = "success";
                                        }
                                        else
                                        {
                                            data.status = "Failure";
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public FeeStudentTransactionDTO getFeetotalamount(FeeStudentTransactionDTO data)
        {
            try
            {
                List<FeeStudentTransactionDTO> customgrp = new List<FeeStudentTransactionDTO>();
                customgrp = (from a in _FeeGroupHeadContext.feegm
                             from b in _FeeGroupHeadContext.feeGGG
                             from c in _FeeGroupHeadContext.FeeStudentTransactionDMO
                             where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.FMG_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && c.AMST_Id == data.Amst_Id && c.ASMAY_Id == data.ASMAY_Id)
                             select new FeeStudentTransactionDTO
                             {
                                 FMG_Id = b.FMG_Id
                             }
         ).Distinct().ToList();

                string groupudss = "0";
                List<long> grp_ids = new List<long>();
                foreach (var item in customgrp)
                {
                    grp_ids.Add(item.FMG_Id);
                    groupudss = groupudss + ',' + item.FMG_Id;
                }



                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_Fees_totalPortal";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = data.Amst_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@GROUPID", SqlDbType.VarChar)
                    {
                        Value = groupudss
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader1 = cmd.ExecuteReader())
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
                        data.filonlinepaymentgrid = retObject1.ToArray();
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


            return data;
        }

  
        public async System.Threading.Tasks.Task<FeeStudentTransactionDTO> paymentPartEasyBuzzMobileAsync(FeeStudentTransactionDTO enq)
        {
            long totconclist = 0;
            long yrid = getacademicyearcongig(enq);
            string termlst = "0";
            long fmttotal = 0;
            enq.ASMAY_Id = yrid;
            List<multipleaccountscashfree> mulacc = new List<multipleaccountscashfree>();
            long clsid = enq.ASMCL_ID;
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


            var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                            from b in _FeeGroupHeadContext.AdmissionStudentDMO
                            where (a.AMST_Id == b.AMST_Id && a.AMST_Id == enq.Amst_Id && a.AMAY_ActiveFlag == 1)
                            select new FeeStudentTransactionDTO
                            {
                                AMST_ECSFlag = b.AMST_ECSFlag,
                                AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) +
                                        (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) +
                                        (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim(),
                                AMST_AdmNo = b.AMST_AdmNo,
                                amst_mobile = b.AMST_MobileNo,
                                amst_email_id = b.AMST_emailId
                            }
).Distinct().ToArray();

            int ecsflag = 0;
            for (int s = 0; s < fetchecs.Count(); s++)
            {
                if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                {
                    ecsflag = 0;
                }
                else
                {
                    ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                }
            }


            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();

            long fineamt = 0, totalfine = 0;
            try
            {
                Payment pay = new Payment(_context);
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;

                string termids = "0", grpidss = "0";

                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == enq.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                             ).FirstOrDefault();

                enq.FSS_FineAmount = 0;
                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                for (int l = 0; l < enq.selected_list.Length; l++)
                {
                    totalfine = 0;
                    string fpgdids = "";
                    List<long> trm_ids = new List<long>();
                    foreach (var x in enq.selected_list[l].trm_list)
                    {
                        termids = termids + ',' + x.FMT_Id;
                        trm_ids.Add(x.FMT_Id);
                        termlst = termlst + "," + x.FMT_Id;

                    }

                    enq.ftiidss = termids;

                    var grp_ids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id);
                    var grp_ids1 = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == enq.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id).ToArray();
                    fineamt = 0;
                    for (int r = 0; r < grp_ids1.Count(); r++)
                    {
                        grpidss = grpidss + ',' + grp_ids1[r];
                    }

                    enq.grpidss = grpidss;

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();
                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }
                    else
                    {
                        var fpgdids1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                        from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID && trm_ids.Contains(a.fmt_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                        select new FeeStudentTransactionDTO
                                        {
                                            merchantid = b.FPGD_SubMerchantId
                                        }
          ).Distinct().ToList();

                        fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();
                    }

                    List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                    List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id))
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                         from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName,
                                             merchantid = e.FPGD_SubMerchantId
                                         }
               ).Distinct().ToList();
                    }


                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }


                    List<FeeStudentTransactionDTO> list123 = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list456 = new List<FeeStudentTransactionDTO>();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list123 = list;
                        var list1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                     from g in _FeeGroupHeadContext.feeYCC
                                     from h in _FeeGroupHeadContext.feeYCCC
                                     from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                     from j in _FeeGroupHeadContext.Yearlygroups
                                     where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0 && j.FMG_Id == f.FMG_Id && j.ASMAY_Id == enq.ASMAY_Id && j.FYG_RebateApplicableFlg == true)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMA_Id = b.FMA_Id,
                                         FSS_ToBePaid = i.FSS_ToBePaid,
                                         FSS_FineAmount = i.FSS_FineAmount,
                                         FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                         FSS_WaivedAmount = i.FSS_WaivedAmount,
                                         FMG_Id = b.FMG_Id,
                                         FMH_Id = b.FMH_Id,
                                         FTI_Id = b.FTI_Id,
                                         FSS_PaidAmount = i.FSS_PaidAmount,
                                         FSS_NetAmount = i.FSS_NetAmount,
                                         FSS_RefundAmount = i.FSS_RebateAmount,
                                         FMH_FeeName = c.FMH_FeeName,
                                         FTI_Name = d.FTI_Name,
                                         FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                         FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                     }
                 ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                        list456 = list1;
                    }
                    else
                    {
                        var list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                    from c in _FeeGroupHeadContext.FeeHeadDMO
                                    from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                    from f in _FeeGroupHeadContext.FeeGroupDMO
                                    from g in _FeeGroupHeadContext.feeYCC
                                    from h in _FeeGroupHeadContext.feeYCCC
                                    from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                    where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMA_Id = b.FMA_Id,
                                        FSS_ToBePaid = i.FSS_ToBePaid,
                                        FSS_FineAmount = i.FSS_FineAmount,
                                        FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                        FSS_WaivedAmount = i.FSS_WaivedAmount,
                                        FMG_Id = b.FMG_Id,
                                        FMH_Id = b.FMH_Id,
                                        FTI_Id = b.FTI_Id,
                                        FSS_PaidAmount = i.FSS_PaidAmount,
                                        FSS_NetAmount = i.FSS_NetAmount,
                                        FSS_RefundAmount = i.FSS_RebateAmount,
                                        FMH_FeeName = c.FMH_FeeName,
                                        FTI_Name = d.FTI_Name,
                                        FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                        FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                    }
                       ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list123 = list;
                        var list1 = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                     from g in _FeeGroupHeadContext.feeYCC
                                     from h in _FeeGroupHeadContext.feeYCCC
                                     from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                     from j in _FeeGroupHeadContext.Yearlygroups
                                     where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && i.FSS_ToBePaid > 0 && j.FMG_Id == f.FMG_Id && j.ASMAY_Id == enq.ASMAY_Id && j.FYG_RebateApplicableFlg == true)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMA_Id = b.FMA_Id,
                                         FSS_ToBePaid = i.FSS_ToBePaid,
                                         FSS_FineAmount = i.FSS_FineAmount,
                                         FSS_ConcessionAmount = i.FSS_ConcessionAmount,
                                         FSS_WaivedAmount = i.FSS_WaivedAmount,
                                         FMG_Id = b.FMG_Id,
                                         FMH_Id = b.FMH_Id,
                                         FTI_Id = b.FTI_Id,
                                         FSS_PaidAmount = i.FSS_PaidAmount,
                                         FSS_NetAmount = i.FSS_NetAmount,
                                         FSS_RefundAmount = i.FSS_RebateAmount,
                                         FMH_FeeName = c.FMH_FeeName,
                                         FTI_Name = d.FTI_Name,
                                         FSS_CurrentYrCharges = i.FSS_CurrentYrCharges,
                                         FSS_TotalToBePaid = i.FSS_TotalToBePaid
                                     }
                  ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        list456 = list1;

                    }


                    foreach (var x in list456)
                    {
                        fmttotal = fmttotal + x.FSS_CurrentYrCharges;
                    }




                    foreach (var x in list123)
                    {
                        amount_list.Add(x);

                        totconclist = totconclist + x.FSS_ConcessionAmount;

                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            if (ecsflag.Equals(0))
                            {
                                cmd.CommandText = "Sp_Calculate_Fine";
                            }
                            else
                            {
                                cmd.CommandText = "Sp_Calculate_Fine_ECS";
                            }
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime, 100)
                            {
                                Value = indianTime
                            });

                            cmd.Parameters.Add(new SqlParameter("@fma_id",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = x.FMA_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = enq.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Decimal, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                 where (a.MI_Id == enq.MI_Id && a.ASMAY_Id == enq.ASMAY_Id && a.FMA_Id == x.FMA_Id && a.AMST_Id == enq.Amst_Id)
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                     FSWO_FineFlg = a.FSWO_FineFlg,
                                                     FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                 }
            ).Distinct().ToList();

                            if (waivedofffine.Count() > 0)
                            {
                                if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = 0;
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                        totalfine = totalfine + fineamt;
                                    }
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                                {
                                    fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    totalfine = totalfine + fineamt;
                                }
                            }

                        }

                    }

                    if (Convert.ToInt32(fineamt) > 0)
                    {
                        if (fineheadfmaid.Count() > 0)
                        {
                            finelst.Add(new FeeStudentTransactionDTO
                            {
                                FMA_Id = fineheadfmaid[0].FMA_Id,
                                FSS_ToBePaid = Convert.ToInt32(fineamt),
                                FSS_FineAmount = 0,
                                FSS_ConcessionAmount = 0,
                                FSS_WaivedAmount = 0,
                                FMG_Id = 0,
                                FMH_Id = 0,
                                FTI_Id = 0,
                                FSS_PaidAmount = 0,
                                FSS_NetAmount = 0,
                                FSS_RefundAmount = 0,
                                FMH_FeeName = "Fine",
                                FTI_Name = "Anytime",
                                FSS_CurrentYrCharges = 0,
                                FSS_TotalToBePaid = 0,
                                merchantid = fineheadfmaid[0].merchantid,
                                FYP_Id = 0
                            });
                        }

                    }


                    string grp = "";
                    grp = "0";
                    if (grp_ids != null)
                    {
                        foreach (var i in grp_ids)
                        {
                            grp = grp + "," + i;
                        }
                    }
                    string term = "";
                    term = "0";
                    if (trm_ids != null)
                    {
                        foreach (var i in trm_ids)
                        {
                            term = term + "," + i;
                        }
                    }
                    List<multipleaccountscashfree> employeedetails = new List<multipleaccountscashfree>();

                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Feeaccountdetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@AMST_id",
                                SqlDbType.VarChar)
                        {
                            Value = enq.Amst_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAYID",
                                SqlDbType.VarChar)
                        {
                            Value = enq.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FMTID",
                                SqlDbType.VarChar)
                        {
                            Value = term
                        });

                        cmd.Parameters.Add(new SqlParameter("@FMGGID",
                                SqlDbType.VarChar)
                        {
                            Value = grp
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                SqlDbType.VarChar)
                        {
                            Value = enq.MI_Id
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
                                            dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                        );
                                    }



                                    employeedetails.Add(new multipleaccountscashfree
                                    {
                                        amount = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),
                                        vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                    });

                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    if (employeedetails.Count() > 0)
                    {
                        foreach (var q in employeedetails)
                        {
                            mulacc.Add(q);
                        }
                    }



                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id))
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }
                    else
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_ToBePaid = b.FSS_ToBePaid,
                                             }
           ).Sum(t => t.FSS_ToBePaid);
                    }

                    enq.FMA_Amount = Convert.ToInt64(enq.pendingamount);

                    result.Add(new FeeSlplitOnlinePayment
                    {
                        name = "splitId" + autoinc.ToString(),
                        merchantId = fpgdids,
                        value = (Convert.ToInt32(enq.pendingamount) + fineamt).ToString(),
                        commission = "0",
                        description = "Online Payment",
                    });

                    var Euserid = (from a in _FeeGroupHeadContext.FeeGroupDMO
                                   where (a.MI_Id == Convert.ToInt64(enq.MI_Id) && grp_ids.Contains(a.FMG_Id))
                                   select new FeeStudentTransactionDTO
                                   {
                                       enduserid = a.user_id,
                                   }
).Distinct().Take(1).ToArray();

                    enq.userid = Convert.ToInt64(Euserid[0].enduserid);

                    enq.FSS_FineAmount = enq.FSS_FineAmount + fineamt;
                }

                enq.amount_list = amount_list.ToArray();

                if (finelst.Count() > 0)
                {
                    foreach (var y in finelst)
                    {
                        amount_list.Add(y);
                    }
                }
                enq.Totalconcession = totconclist;
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();


                if (readterms.IVRMGC_Classwise_Payment != "1")
                {
                    paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

                }
                else
                {
                    var onlinepayment = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping

                                         from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new FeeStudentTransactionDTO
                                         {
                                             FPGD_SubMerchantId = c.FPGD_SubMerchantId
                                         }
 ).ToList();

                    if (onlinepayment.Count > 0)
                    {

                        paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway && t.FPGD_SubMerchantId == onlinepayment[0].FPGD_SubMerchantId).Distinct().ToList();

                    }


                }

                //rebate


                var cnt2 = _context.FeeMasterConfigurationDMO.Where(t => t.MI_Id == enq.MI_Id && t.FMC_RebateAplicableFlg == true && t.FMC_RebateAgainstFullPaymentFlg == true && t.FMC_RebateAgainstPartialPaymentFlg == true).ToList();



                string rebateamount;

                if (cnt2.Count > 0)
                {
                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fee_RebateTermWise_calculation_BOTH";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.VarChar)
                        {
                            Value = enq.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.VarChar)
                        {
                            Value = enq.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMT_ID",
                      SqlDbType.VarChar)
                        {
                            Value = termlst
                        });
                        cmd.Parameters.Add(new SqlParameter("@paiddate",
                      SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
                        });
                        cmd.Parameters.Add(new SqlParameter("@paidamount",
                       SqlDbType.VarChar)
                        {
                            Value = fmttotal
                        });

                        cmd.Parameters.Add(new SqlParameter("@USERID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
            SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                    }



                }
                else
                {
                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fee_RebateTermWise_calculation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.VarChar)
                        {
                            Value = enq.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                           SqlDbType.VarChar)
                        {
                            Value = enq.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FMT_ID",
                      SqlDbType.VarChar)
                        {
                            Value = termlst
                        });
                        cmd.Parameters.Add(new SqlParameter("@paiddate",
                      SqlDbType.DateTime)
                        {
                            Value = DateTime.Now
                        });
                        cmd.Parameters.Add(new SqlParameter("@paidamount",
                       SqlDbType.VarChar)
                        {
                            Value = fmttotal
                        });

                        cmd.Parameters.Add(new SqlParameter("@USERID",
                       SqlDbType.VarChar)
                        {
                            Value = enq.userid
                        });

                        cmd.Parameters.Add(new SqlParameter("@totalrebateamount",
            SqlDbType.BigInt)
                        {
                            Direction = ParameterDirection.Output
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var data1 = cmd.ExecuteNonQuery();
                        rebateamount = cmd.Parameters["@totalrebateamount"].Value.ToString();
                    }

                }

                enq.FSS_RebateAmount = Convert.ToInt64(rebateamount);


                //rebate


                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                foreach (FeeSlplitOnlinePayment x in result)
                {
                    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                }

                var item = new
                {
                    paymentParts = result
                };




                enq.FMA_Amount = Convert.ToInt64(totpayableamount);
                string amount = (totpayableamount - enq.FSS_RebateAmount).ToString();
                string firstname = fetchecs[0].AMST_FirstName;
                string email = fetchecs[0].amst_email_id;
                string phone = (fetchecs[0].amst_mobile).ToString();


                string surl = "http://localhost:57606/api/FeeOnlinePaymentStthomas/getpaymentresponseeasybuzzmobile/";
                string furl = "http://localhost:57606/api/FeeOnlinePaymentStthomas/getpaymentresponseeasybuzzmobile/";



                enq.trans_id = enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();


                string Txnid = (enq.trans_id).ToString();
                string UDF1 = (enq.Amst_Id).ToString();

                string UDF2 = (enq.userid).ToString();

                string UDF3 = (enq.MI_Id).ToString();

                string UDF4 = (enq.ASMAY_Id).ToString();

                string UDF5 = (enq.ftiidss).ToString();

                string UDF6 = "";

                string UDF7 = "";
                string UDF8 = "";
                string UDF9 = "";
                string UDF10 = "";
                string productinfo = (enq.ASMCL_ID).ToString();
                string Show_payment_mode = "";

                Dictionary<string, long> transfersnotescash = new Dictionary<string, long>();
                string split = "";


                var z = 0;
                if (mulacc.Count > 0)
                {
                    split += "{";

                    for (var j = 0; j < mulacc.Count; j++)
                    {
                        if (j == 0)
                        {
                            if (fineamt > 0)
                            {
                                split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                                transfersnotescash.Add(mulacc[j].vendor_id.ToString(), (mulacc[j].amount + fineamt) - enq.FSS_RebateAmount);
                            }
                            else
                            {
                                split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                                transfersnotescash.Add(mulacc[j].vendor_id.ToString(), mulacc[j].amount - enq.FSS_RebateAmount);
                            }
                        }
                        else
                        {
                            split += "'" + mulacc[j].vendor_id.ToString() + "':" + mulacc[j].amount + ",";
                            transfersnotescash.Add(mulacc[j].vendor_id.ToString(), mulacc[j].amount - enq.FSS_RebateAmount);
                        }



                    }

                    split = split.Substring(0, (split.Length - 1));




                    split += "}";
                }
                var myContent = JsonConvert.SerializeObject(transfersnotescash);
                String postData = myContent;

                string split_payments = postData;

                string sub_merchant_id = paymentdetails.FirstOrDefault().FPGD_AccNo;

                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string url = "https://pay.easebuzz.in/payment/initiateLink";

                string strForm = secret + "|" + Txnid + "|" + amount + "|" + productinfo + "|" + firstname + "|" + email + "|" + UDF1 + "|" + UDF2 + "|" + UDF3 + "|" + UDF4 + "|" + UDF5 + "|" + UDF6 + "|" + UDF7 + "|" + UDF8 + "|" + UDF9 + "|" + UDF10 + "|" + key;
                string hash = SHA512Alg(strForm);


                string hash1 = hash.ToLower();





                var client = new HttpClient();

                var requestnew = new HttpRequestMessage
                {
                    Method = System.Net.Http.HttpMethod.Post,
                    RequestUri = new Uri("https://pay.easebuzz.in/payment/initiateLink"),
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
    {
        { "key", secret },
        { "txnid", Txnid },
        { "amount", amount },
        { "productinfo", productinfo },
        { "firstname", firstname },
        { "phone", phone },
        { "email", email },
        { "surl", surl },
        { "furl", furl },
        { "hash", hash1 },
        { "udf1", UDF1 },
        { "udf2", UDF2 },
        { "udf3", UDF3 },
        { "udf4", UDF4 },
        { "udf5", UDF5 },
        { "udf6", UDF6 },
        { "udf7", UDF7 },
        { "address1", "" },
        { "address2", "" },
        { "city", "" },
        { "state", "" },
        { "country", "" },
        { "zipcode", "" },

         { "split_payments", split_payments },
         { "sub_merchant_id", sub_merchant_id }


    }),
                };
                using (var response123 = await client.SendAsync(requestnew))
                {
                    response123.EnsureSuccessStatusCode();
                    var body = await response123.Content.ReadAsStringAsync();

                    Console.WriteLine(body);
                    JObject joResponse1 = JObject.Parse(body);
                    enq.easebuzzstatus = (Int64)joResponse1["status"];
                    enq.easebuzzdata = (String)joResponse1["data"];

                    enq.easebuzzenv = "production";



                }




                if (enq.FYP_PayModeType == "Portal")
                {
                    get_grp_reptno(enq);
                }
                else if (enq.FYP_PayModeType == "MOBILE")
                {
                    get_grp_reptno_Mobile(enq);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return enq;
        }


        public PaymentDetails.easybuzzmobile getpaymentresponseeasybuzzmobile(PaymentDetails.easybuzzmobile response)
        {

            FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
            PaymentDetails dto = new PaymentDetails();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            if (response.payment_response.status == "success")
            {
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(q => q.MI_Id == Convert.ToInt64(response.payment_response.udf3) && q.FPGD_PGName == "EASEBUZZ").Distinct().ToList();

                string txnid = response.payment_response.txnid.ToString();
                string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string env = "prod";
                string paymentref = response.payment_response.easepayid;


                var groups = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                              where (a.MI_Id == Convert.ToInt32(response.payment_response.udf3) && a.ASMAY_ID == Convert.ToInt32(response.payment_response.udf4) && a.fyp_transaction_id == response.payment_response.txnid)
                              select new FeeStudentTransactionDTO
                              {
                                  FYP_Receipt_No = a.FYP_Receipt_No
                              }
                      ).Distinct().ToList();

                if (groups.Count == 0)
                {
                    // public string insertdatainfeetables(string miid, string termid, string studentid, string classid, decimal amount, string transid, string refid, string yearid, DateTime indianTime, string transactionid)

                    var confirmstatus = insertdatainfeetables(response.payment_response.udf3, response.payment_response.udf5.ToString(), response.payment_response.udf1.ToString(), response.payment_response.productinfo.ToString(), Convert.ToDecimal(response.payment_response.amount), response.payment_response.txnid, paymentref.ToString(), response.payment_response.udf4, indianTime, "0");

                    if (Convert.ToInt32(confirmstatus) > 0)
                    {
                        pgmod.MI_Id = Convert.ToInt64(response.payment_response.udf3);
                        pgmod.amst_mobile = Convert.ToInt64(response.payment_response.phone);
                        pgmod.Amst_Id = Convert.ToInt64(response.payment_response.udf3);
                        pgmod.amst_email_id = response.payment_response.email;
                        response.status = "true";
                        SMS sms = new SMS(_context);

                        sms.sendSms(Convert.ToInt32(response.payment_response.udf3), Convert.ToInt64(response.payment_response.phone), "FEEONLINEPAYMENT", Convert.ToInt32(response.payment_response.udf2));

                        Email Email = new Email(_context);

                        Email.sendmail(Convert.ToInt32(response.payment_response.udf3), response.payment_response.email, "FEEONLINEPAYMENT", Convert.ToInt32(response.payment_response.udf2));

                    }
                }
            }
            else
            {
                response.status = "failed";
                SMS sms = new SMS(_context);

                sms.sendSms(Convert.ToInt64(response.payment_response.udf4), Convert.ToInt64(response.payment_response.phone), "FEEONLINEPAYMENTFAIL", Convert.ToInt64(response.payment_response.udf2));

                Email Email = new Email(_context);

                Email.sendmail(Convert.ToInt64(response.payment_response.udf4), response.payment_response.email, "FEEONLINEPAYMENTFAIL", Convert.ToInt64(response.payment_response.udf2));

                dto.status = response.payment_response.status;
            }

            return response;
        }

        public static string SHA512Alg(string inputString)
        {
            SHA512 sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }


        public FeeStudentTransactionDTO paymentPartEasebuzzPartial(FeeStudentTransactionDTO enq)
        {
            long totconclist = 0;
            long yrid = getacademicyearcongig(enq);
            string termlst = "0";
            long fmttotal = 0;
            enq.ASMAY_Id = yrid;

            List<multipleaccountscashfree> mulacc = new List<multipleaccountscashfree>();
            long clsid = enq.ASMCL_ID;
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


            var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                            from b in _FeeGroupHeadContext.AdmissionStudentDMO
                            where (a.AMST_Id == b.AMST_Id && a.AMST_Id == enq.Amst_Id && a.AMAY_ActiveFlag == 1)
                            select new FeeStudentTransactionDTO
                            {
                                AMST_ECSFlag = b.AMST_ECSFlag,
                                AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "0" ? "" : b.AMST_FirstName) +
                                        (b.AMST_MiddleName == null || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) +
                                        (b.AMST_LastName == null || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim(),
                                AMST_AdmNo = b.AMST_AdmNo,
                                amst_mobile = b.AMST_MobileNo,
                                amst_email_id = b.AMST_emailId
                            }
).Distinct().ToArray();

            int ecsflag = 0;
            for (int s = 0; s < fetchecs.Count(); s++)
            {
                if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                {
                    ecsflag = 0;
                }
                else
                {
                    ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                }
            }


            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();

            long fineamt = 0, totalfine = 0;
            try
            {
                Payment pay = new Payment(_context);
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;

                string termids = "0", grpidss = "0";

                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == enq.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                             ).FirstOrDefault();

                enq.FSS_FineAmount = 0;
                string fpgdids = "";
                List<long> trm_ids = new List<long>();

                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();

                string ftiidss = "0";
                string term = "0";
                List<multipleaccountscashfree> accountdetails = new List<multipleaccountscashfree>();

                Dictionary<long, long> groupdetailids = new Dictionary<long, long>();
                foreach (var x in enq.selectedlist_partialpayment)
                {

                    if (groupdetailids.Count == 0 || groupdetailids == null)
                    {
                        groupdetailids.Add(x.FMG_Id, x.FSS_ToBePaid);
                    }
                    else
                    {

                        if (groupdetailids.ContainsKey(x.FMG_Id))
                        {
                            groupdetailids[x.FMG_Id] += x.FSS_ToBePaid;
                        }
                        else
                        {
                            groupdetailids.Add(x.FMG_Id, x.FSS_ToBePaid);
                        }

                    }
                    term = term + "," + x.FMT_Id;

                    ftiidss = ftiidss + "," + x.FTI_Id;

                }

                if (groupdetailids.Count > 0)
                {
                    foreach (KeyValuePair<long, long> entry in groupdetailids)
                    {
                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Feeaccountdetailspartial";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@AMST_id",
                                    SqlDbType.VarChar)
                            {
                                Value = enq.Amst_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAYID",
                                    SqlDbType.VarChar)
                            {
                                Value = enq.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@FMTID",
                                    SqlDbType.VarChar)
                            {
                                Value = term
                            });

                            cmd.Parameters.Add(new SqlParameter("@FMGGID",
                                    SqlDbType.VarChar)
                            {
                                Value = entry.Key
                            });
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                    SqlDbType.VarChar)
                            {
                                Value = enq.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Amount",
                                    SqlDbType.VarChar)
                            {
                                Value = entry.Value
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            var retObject1 = new List<dynamic>();
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
                                                dataReader.IsDBNull(iFiled) ? 0 : dataReader[iFiled]
                                            );
                                        }



                                        accountdetails.Add(new multipleaccountscashfree
                                        {
                                            amount = Convert.ToInt64(dataReader["FSS_Tobepaid"].ToString()),
                                            vendor_id = dataReader["FPGD_SubMerchantId"].ToString()



                                        });

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


                // List<multipleaccountscashfree> Accountdetailsfinal = new List<multipleaccountscashfree>();
                Dictionary<string, decimal> Accountdetailsfinal = new Dictionary<string, decimal>();

                long totalamtnew = 0;

                foreach (var entry in accountdetails)
                {
                    if (Accountdetailsfinal.Count == 0 || Accountdetailsfinal == null)
                    {
                        Accountdetailsfinal.Add(entry.vendor_id, Convert.ToDecimal(entry.amount));
                        totalamtnew += entry.amount;
                    }
                    else
                    {

                        if (Accountdetailsfinal.ContainsKey(entry.vendor_id))
                        {
                            Accountdetailsfinal[entry.vendor_id] += Convert.ToDecimal(entry.amount);
                            totalamtnew += entry.amount;
                        }
                        else
                        {
                            Accountdetailsfinal.Add(entry.vendor_id, Convert.ToDecimal(entry.amount));
                            totalamtnew += entry.amount;
                        }

                    }

                }




                enq.amount_list = amount_list.ToArray();

                if (finelst.Count() > 0)
                {
                    foreach (var y in finelst)
                    {
                        amount_list.Add(y);
                    }
                }
                enq.Totalconcession = totconclist;
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();



                //added by kavita//
                if (readterms.IVRMGC_Classwise_Payment != "1")
                {
                    paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

                }
                else
                {
                    var onlinepayment = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping

                                         from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                         where (a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID)
                                         select new FeeStudentTransactionDTO
                                         {
                                             FPGD_SubMerchantId = c.FPGD_SubMerchantId
                                         }
 ).ToList();

                    if (onlinepayment.Count > 0)
                    {

                        paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway && t.FPGD_SubMerchantId == onlinepayment[0].FPGD_SubMerchantId).Distinct().ToList();

                    }


                }

                //added by kavita//

                //    paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

                //foreach (FeeSlplitOnlinePayment x in result)
                //{
                //    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                //}

                //var item = new
                //{
                //    paymentParts = result
                //};










                decimal amt = Convert.ToDecimal(totalamtnew);
                enq.FMA_Amount = totalamtnew;









                string amount = (amt + ".0").ToString();
                string firstname = fetchecs[0].AMST_FirstName;
                string email = fetchecs[0].amst_email_id;
                string phone = (fetchecs[0].amst_mobile).ToString();

                string surl = "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponseeasybuzz/";
                string furl = "http://localhost:57606/api/FeeOnlinePaymentStthomas/paymentresponseeasybuzz/";



                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    enq.trans_id = "EASEBUZZ" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();

                }
                string Txnid = (enq.trans_id).ToString();
                string UDF1 = (enq.Amst_Id).ToString();

                string UDF2 = (enq.userid).ToString();

                string UDF3 = (enq.MI_Id).ToString();

                string UDF4 = (enq.ASMAY_Id).ToString();

                string UDF5 = (ftiidss).ToString();

                string UDF6 = "";

                string UDF7 = "";
                string UDF8 = "";
                string UDF9 = "";
                string UDF10 = "";
                string productinfo = (enq.ASMCL_ID).ToString();
                string Show_payment_mode = "";


                var myContent = JsonConvert.SerializeObject(Accountdetailsfinal);
                String postData = myContent;

                string split_payments = postData;
                string sub_merchant_id = paymentdetails.FirstOrDefault().FPGD_AccNo;

                string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

                string env = "prod";
                Easebuzz t1 = new Easebuzz(secret, key, env);



                string strForm = t1.initiatePaymentAPI(amount, firstname, email, phone, productinfo, surl, furl, Txnid, UDF1, UDF2, UDF3, UDF4, UDF5, UDF6, UDF7, UDF8, UDF9, UDF10, Show_payment_mode, split_payments, sub_merchant_id);

                enq.FYP_Tot_Amount = totpayableamount;
                enq.FYP_Tot_Amount = totpayableamount;
                enq.strdatanew = strForm;

                if (enq.FYP_PayModeType == "Portal")
                {
                    get_grp_reptnopartial(enq);
                }

                enq.strdatanew = strForm;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return enq;
        }

        public FeeStudentTransactionDTO get_grp_reptnopartial(FeeStudentTransactionDTO data)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();
                string termlst = "0";
                long fmttotal = 0;

                var fetchecs = (from a in _FeeGroupHeadContext.School_Adm_Y_StudentDMO
                                from b in _FeeGroupHeadContext.AdmissionStudentDMO
                                where (a.AMST_Id == b.AMST_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1)
                                select new FeeStudentTransactionDTO
                                {
                                    AMST_ECSFlag = b.AMST_ECSFlag
                                }
               ).Distinct().ToArray();

                int ecsflag = 0;
                for (int s = 0; s < fetchecs.Count(); s++)
                {
                    if (fetchecs[s].AMST_ECSFlag.Equals(null) || fetchecs[s].AMST_ECSFlag.Equals(""))
                    {
                        ecsflag = 0;
                    }
                    else
                    {
                        ecsflag = Convert.ToInt32(fetchecs[s].AMST_ECSFlag);
                    }
                }


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //DateTime curdt = DateTime.Now;
                string onlineheadmapid = "0", groupidss = "0";

                long fineamountcal = 0, fethchfmaidsfine = 0, finecount = 0;

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == data.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                            ).FirstOrDefault();


                //if (data.auto_receipt_flag == 1)
                //{
                List<long> HeadId = new List<long>();
                List<long> groupidss1 = new List<long>();
                List<long> termids = new List<long>();
                foreach (var item in data.temarray)
                {
                    HeadId.Add(item.FMH_Id);
                    termids.Add(item.FMT_Id);
                    groupidss1.Add(item.FMG_Id);
                }

                foreach (var item in data.selected_list[0].trm_list)
                {
                    termids.Add(item.FMT_Id);
                    termlst = termlst + "," + item.FMT_Id;
                }

                List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
                        from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                        from d in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                        from e in _FeeGroupHeadContext.FeeStudentTransactionDMO
                        where (c.FGAR_Id == d.FGAR_Id && d.ASMAY_Id == b.ASMAY_Id && b.FMG_Id == c.FMG_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id) && c.FMG_Id == e.FMG_Id && b.FMG_Id == e.FMG_Id && e.AMST_Id == data.Amst_Id && b.FMH_Id == e.FMH_Id && e.ASMAY_Id == data.ASMAY_Id && groupidss1.Contains(e.FMG_Id))
                        select new FeeStudentTransactionDTO
                        {
                            FGAR_Id = c.FGAR_Id
                        }
                       ).Distinct().ToList();

                for (int r = 0; r < grps.Count(); r++)
                {

                    //praveen
                    foreach (var item10 in data.temarray)
                    {
                        termids.Add(item10.FMT_Id);
                    }
                    //praveen

                    onlineheadmapid = grps[r].FGAR_Id.ToString();

                    List<FeeStudentTransactionDTO> grps1 = new List<FeeStudentTransactionDTO>();
                    grps1 = (from a in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                             from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                             from c in _FeeGroupHeadContext.FeeStudentTransactionDMO
                             where (b.FGAR_Id == a.FGAR_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FGAR_Id == Convert.ToInt64(onlineheadmapid) && c.FMG_Id == a.FMG_Id && c.AMST_Id == data.Amst_Id)
                             select new FeeStudentTransactionDTO
                             {
                                 FMG_Id = a.FMG_Id
                             }
                     ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    foreach (var item in grps1)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    for (int d = 0; d < grps1.Count(); d++)
                    {
                        groupidss = groupidss + ',' + grps1[d].FMG_Id;
                    }

                    termids.Clear();


                    //praveen
                    foreach (var item in data.selected_list)
                    {
                        foreach (var item1 in item.trm_list)
                        {

                            var newgrplst = (from a in _FeeGroupHeadContext.feeGGG.Where(y => y.FMGG_Id == item1.FMGG_Id)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FMG_Id = a.FMG_Id
                                             }).Distinct().ToList();


                            foreach (var fmg in newgrplst)
                            {
                                if (grpid.Contains(fmg.FMG_Id))
                                {
                                    termids.Add(item1.FMT_Id);
                                }
                            }

                        }

                    }
                    if (termids.Count > 0)
                    {
                        List<long> temptermids = new List<long>();
                        temptermids = termids.Distinct().ToList();
                        termids = temptermids;
                    }

                    //praveen

                    List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();
                    list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                                from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new FeeStudentTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    FGAR_Id = c.FGAR_Id,
                                }
                         ).Distinct().ToList();

                    //List<FeeStudentTransactionDTO> groupwiseamount = new List<FeeStudentTransactionDTO>();
                    decimal groupwiseamount = 0;

                    var Euserid = (from a in _FeeGroupHeadContext.FeeGroupDMO
                                   where (a.MI_Id == Convert.ToInt64(data.MI_Id) && grpid.Contains(a.FMG_Id))
                                   select new FeeStudentTransactionDTO
                                   {
                                       enduserid = a.user_id,
                                   }
             ).Distinct().Take(1).ToArray();
                    //added on 02-07-2018

                    data.userid = Convert.ToInt64(Euserid[0].enduserid);


                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "receiptnogeneration";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                            SqlDbType.VarChar, 100)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@asmayid",
                           SqlDbType.NVarChar, 100)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmgid",
                       SqlDbType.NVarChar, 100)
                        {
                            Value = groupidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@receiptno",
            SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

                        var rece_amt = new { receiptno = data.FYP_Receipt_No, amt = data.FMA_Amount };

                        dynamicrecgen.Add(rece_amt);




                        Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                        onlinemtrans.FMOT_Trans_Id = data.trans_id;
                        //onlinemtrans.FMOT_Amount = data.topayamount;
                        onlinemtrans.FMOT_Amount = rece_amt.amt;
                        onlinemtrans.FMOT_Date = indianTime;
                        onlinemtrans.FMOT_Flag = "P";
                        onlinemtrans.PASR_Id = 0;
                        onlinemtrans.AMST_Id = data.Amst_Id;
                        onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;
                        onlinemtrans.FYP_PayModeType = data.FYP_PayModeType;
                        onlinemtrans.FMOT_PayGatewayType = data.onlinepaygteway;

                        onlinemtrans.MI_Id = data.MI_Id;
                        onlinemtrans.ASMAY_ID = data.ASMAY_Id;

                        _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);


                        for (var i = 0; i < data.selectedlist_partialpayment.Length; i++)
                        {
                            for (var l = 0; l < data.selectedlist_partialpayment.Length - 1; l++)
                            {
                                if (data.selectedlist_partialpayment[l].FSS_ToBePaid < data.selectedlist_partialpayment[l + 1].FSS_ToBePaid)
                                {
                                    var temp = data.selectedlist_partialpayment[l];
                                    data.selectedlist_partialpayment[l] = data.selectedlist_partialpayment[l + 1];
                                    data.selectedlist_partialpayment[l + 1] = temp;
                                }
                            }
                        }



                        for (var j = 0; j < data.selectedlist_partialpayment.Length; j++)
                        {



                            if (j == 0)
                            {

                                if (data.selectedlist_partialpayment[j].FMH_Flag == "F")
                                {
                                    fethchfmaidsfine = data.selectedlist_partialpayment[j].FMA_Id;
                                }
                                else
                                {
                                    Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                    onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                    onlinettrans.FMA_Id = data.selectedlist_partialpayment[j].FMA_Id;
                                    onlinettrans.FTOT_Amount = data.selectedlist_partialpayment[j].FSS_ToBePaid - data.FSS_RebateAmount;
                                    onlinettrans.FTOT_Created_date = indianTime;
                                    onlinettrans.FTOT_Updated_date = indianTime;
                                    onlinettrans.FTOT_Concession = 0;
                                    onlinettrans.FTOT_Fine = 0;
                                    onlinettrans.FTOT_RebateAmount = data.FSS_RebateAmount;

                                    using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        if (ecsflag.Equals(0))
                                        {
                                            cmd1.CommandText = "Sp_Calculate_Fine";
                                        }
                                        else
                                        {
                                            cmd1.CommandText = "Sp_Calculate_Fine_ECS";
                                        }

                                        //cmd1.CommandText = "Sp_Calculate_Fine";
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add(new SqlParameter("@On_Date",
                                            SqlDbType.DateTime, 100)
                                        {
                                            //Value = curdt
                                            Value = indianTime
                                        });

                                        cmd1.Parameters.Add(new SqlParameter("@fma_id",
                                           SqlDbType.NVarChar, 100)
                                        {
                                            Value = data.selectedlist_partialpayment[j].FMA_Id
                                        });
                                        cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                                       SqlDbType.NVarChar, 100)
                                        {
                                            Value = data.ASMAY_Id
                                        });

                                        cmd1.Parameters.Add(new SqlParameter("@amt",
                            SqlDbType.Decimal, 500)
                                        {
                                            Direction = ParameterDirection.Output
                                        });

                                        cmd1.Parameters.Add(new SqlParameter("@flgArr",
                           SqlDbType.Int, 500)
                                        {
                                            Direction = ParameterDirection.Output
                                        });

                                        if (cmd1.Connection.State != ConnectionState.Open)
                                            cmd1.Connection.Open();

                                        var execfine = cmd1.ExecuteNonQuery();

                                        //finewaiedoff

                                        var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                             where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMA_Id == data.selectedlist_partialpayment[j].FMA_Id && a.AMST_Id == data.Amst_Id)
                                                             select new FeeStudentTransactionDTO
                                                             {
                                                                 FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                                 FSWO_FineFlg = a.FSWO_FineFlg,
                                                                 FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                             }
               ).Distinct().ToList();

                                        if (waivedofffine.Count() > 0)
                                        {
                                            if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    fineamountcal = fineamountcal + (Convert.ToInt32(cmd1.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount));
                                                }
                                            }
                                            else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    fineamountcal = fineamountcal + 0;
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    fineamountcal = fineamountcal + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamountcal = fineamountcal + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                            }
                                        }


                                    }

                                    _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                }




                                if (data.selectedlist_partialpayment[j].FMH_Flag == "F")
                                {
                                    if (finecount < 1)
                                    {
                                        finecount = finecount + 1;
                                        fethchfmaidsfine = data.selectedlist_partialpayment[j].FMA_Id;
                                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                        onlinettrans.FMA_Id = data.selectedlist_partialpayment[j].FMA_Id;
                                        onlinettrans.FTOT_Amount = fineamountcal - data.FSS_RebateAmount;
                                        onlinettrans.FTOT_Created_date = indianTime;
                                        onlinettrans.FTOT_Updated_date = indianTime;
                                        onlinettrans.FTOT_Concession = 0;
                                        onlinettrans.FTOT_Fine = 0;
                                        onlinettrans.FTOT_RebateAmount = data.FSS_RebateAmount;

                                        _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                    }
                                }


                            }
                            else
                            {


                                if (data.selectedlist_partialpayment[j].FMH_Flag == "F")
                                {
                                    fethchfmaidsfine = data.selectedlist_partialpayment[j].FMA_Id;
                                }
                                else
                                {
                                    Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                    onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                    onlinettrans.FMA_Id = data.selectedlist_partialpayment[j].FMA_Id;
                                    onlinettrans.FTOT_Amount = data.selectedlist_partialpayment[j].FSS_ToBePaid;
                                    onlinettrans.FTOT_Created_date = indianTime;
                                    onlinettrans.FTOT_Updated_date = indianTime;
                                    onlinettrans.FTOT_Concession = 0;
                                    onlinettrans.FTOT_Fine = 0;
                                    onlinettrans.FTOT_RebateAmount = 0;

                                    using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        if (ecsflag.Equals(0))
                                        {
                                            cmd1.CommandText = "Sp_Calculate_Fine";
                                        }
                                        else
                                        {
                                            cmd1.CommandText = "Sp_Calculate_Fine_ECS";
                                        }

                                        //cmd1.CommandText = "Sp_Calculate_Fine";
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add(new SqlParameter("@On_Date",
                                            SqlDbType.DateTime, 100)
                                        {
                                            //Value = curdt
                                            Value = indianTime
                                        });

                                        cmd1.Parameters.Add(new SqlParameter("@fma_id",
                                           SqlDbType.NVarChar, 100)
                                        {
                                            Value = data.selectedlist_partialpayment[j].FMA_Id
                                        });
                                        cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                                       SqlDbType.NVarChar, 100)
                                        {
                                            Value = data.ASMAY_Id
                                        });

                                        cmd1.Parameters.Add(new SqlParameter("@amt",
                            SqlDbType.Decimal, 500)
                                        {
                                            Direction = ParameterDirection.Output
                                        });

                                        cmd1.Parameters.Add(new SqlParameter("@flgArr",
                           SqlDbType.Int, 500)
                                        {
                                            Direction = ParameterDirection.Output
                                        });

                                        if (cmd1.Connection.State != ConnectionState.Open)
                                            cmd1.Connection.Open();

                                        var execfine = cmd1.ExecuteNonQuery();

                                        //finewaiedoff

                                        var waivedofffine = (from a in _FeeGroupHeadContext.feeStudentWaivedOff
                                                             where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMA_Id == data.selectedlist_partialpayment[j].FMA_Id && a.AMST_Id == data.Amst_Id)
                                                             select new FeeStudentTransactionDTO
                                                             {
                                                                 FSS_WaivedAmount = a.FSWO_WaivedOffAmount,
                                                                 FSWO_FineFlg = a.FSWO_FineFlg,
                                                                 FSWO_FullFineWaiveOffFlg = a.FSWO_FullFineWaiveOffFlg
                                                             }
               ).Distinct().ToList();

                                        if (waivedofffine.Count() > 0)
                                        {
                                            if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 0)
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    fineamountcal = fineamountcal + (Convert.ToInt32(cmd1.Parameters["@amt"].Value) - Convert.ToInt32(waivedofffine.FirstOrDefault().FSS_WaivedAmount));
                                                }
                                            }
                                            else if (waivedofffine[0].FSWO_FineFlg == 1 && waivedofffine[0].FSWO_FullFineWaiveOffFlg == 1)
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    fineamountcal = fineamountcal + 0;
                                                }
                                            }
                                            else
                                            {
                                                if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                                {
                                                    fineamountcal = fineamountcal + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                            {
                                                fineamountcal = fineamountcal + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                            }
                                        }


                                    }

                                    _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                }


                                if (data.selectedlist_partialpayment[j].FMH_Flag == "F")
                                {
                                    if (finecount < 1)
                                    {
                                        finecount = finecount + 1;
                                        fethchfmaidsfine = data.selectedlist_partialpayment[j].FMA_Id;
                                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                        onlinettrans.FMA_Id = data.selectedlist_partialpayment[j].FMA_Id;
                                        onlinettrans.FTOT_Amount = fineamountcal;
                                        onlinettrans.FTOT_Created_date = indianTime;
                                        onlinettrans.FTOT_Updated_date = indianTime;
                                        onlinettrans.FTOT_Concession = 0;
                                        onlinettrans.FTOT_Fine = 0;
                                        onlinettrans.FTOT_RebateAmount = 0;
                                        _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                    }
                                }


                            }







                        }
                        var count = 0;

                        for (int s = 0; s < data.selectedlist_partialpayment.Length; s++)
                        {
                            if (data.selectedlist_partialpayment[s].FMH_Flag == "F")
                            {
                                count = count + 1;
                            }
                        }

                        if (count == 0)
                        {
                            fineamountcal = 0;

                        }

                        //commented on 19012018
                        onlinemtrans.FMOT_Amount = Convert.ToInt32(rece_amt.amt) + Convert.ToInt32(fineamountcal) - data.FSS_RebateAmount;



                        groupidss = "0";
                        fineamountcal = 0;
                        finecount = 0;
                    }

                    termids.Clear();
                }

                var contactexisttransaction = 0;

                using (var dbCtxTxn = _FeeGroupHeadContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _FeeGroupHeadContext.SaveChanges();
                        dbCtxTxn.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

                //if (dynamicrecgen.Count() > 0)
                //{
                //    data.recenocol = dynamicrecgen.ToArray();
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }



    }
}
