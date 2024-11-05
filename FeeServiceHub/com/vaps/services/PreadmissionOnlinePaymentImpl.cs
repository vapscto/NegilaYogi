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
using Newtonsoft.Json;
using DomainModel.Model.com.vapstech.Fee;
using paytm.security;
using paytm.util;
using paytm.exception;
using Razorpay.Api;
using Payment = CommonLibrary.Payment;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Text;
using System.IO;
using easebuzz_.net;

namespace FeeServiceHub.com.vaps.services
{
    public class PreadmissionOnlinePaymentImpl : interfaces.PreadmissionOnlinePaymentInterface
    {

        private static ConcurrentDictionary<string, FeeStudentTransactionDTO> _login =
       new ConcurrentDictionary<string, FeeStudentTransactionDTO>();

        public DomainModelMsSqlServerContext _context;
        public FeeGroupContext _FeeGroupHeadContext;
        readonly ILogger<PreadmissionOnlinePaymentImpl> _logger;

        public PreadmissionOnlinePaymentImpl(FeeGroupContext frgContext, ILogger<PreadmissionOnlinePaymentImpl> log, DomainModelMsSqlServerContext context)
        {
            _logger = log;
            _FeeGroupHeadContext = frgContext;
            _context = context;
        }

        public FeeStudentTransactionDTO getamountdetails(FeeStudentTransactionDTO data)
        {
            try
            {

                List<feefinefmaDTO> fineheadfmaid = new List<feefinefmaDTO>();
                List<feefinefmaDTO> fineheadfmaidall = new List<feefinefmaDTO>();
                List<feefinefmaDTO> fineheadfmaidsaved = new List<feefinefmaDTO>();

                List<long> trm_ids = new List<long>();
                List<long> trm_idsforspecialfee = new List<long>();

                List<long> savedfma = new List<long>();

                int grpset = 0;
                var feemasnum = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToArray();
                for (int s = 0; s < feemasnum.Count(); s++)
                {
                    grpset = Convert.ToInt16(feemasnum[s].fee_group_setting);
                    //grpset = 1;
                }

                long fineamt = 0, totalfine = 0;

                var Acdemic_preadmission = _FeeGroupHeadContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                var fetchclass = (from a in _FeeGroupHeadContext.stuapp
                                  where (a.MI_Id == data.MI_Id && a.pasr_id == data.PASR_Id)
                                  select new FeeStudentTransactionDTO
                                  {
                                      ASMCL_ID = a.ASMCL_Id,
                                      ASMAY_Id = a.ASMAY_Id,
                                      ASMST_Id = a.ASMST_Id
                                      //pasl_id=a.PASL_ID
                                  }
  ).Distinct().ToArray();

                string classid = "0", academicyearid = "0", streamid = "0";
                for (int s = 0; s < fetchclass.Count(); s++)
                {
                    classid = fetchclass[s].ASMCL_ID.ToString();
                    //streamid = fetchclass[s].pasl_id.ToString();
                    streamid = fetchclass[s].ASMST_Id.ToString();
                }

                data.fillpaymentgateway = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                           from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id && b.FPGD_PGActiveFlag == "1")
                                           select new FeeStudentTransactionDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }
).Distinct().ToArray();

                List<FeeStudentTransactionDTO> finelst = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> finelstfinal = new List<FeeStudentTransactionDTO>();

                List<FeeStudentTransactionDTO> amount_list = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> gropidssss = new List<FeeStudentTransactionDTO>();

                List<long> groupids = new List<long>();

                for (int l = 0; l < data.selected_list.Length; l++)
                {
                    trm_ids.Clear();
                    foreach (var x in data.selected_list[l].trm_list)
                    {
                        trm_ids.Add(x.FMT_Id);
                        trm_idsforspecialfee.Add(x.FMT_Id);
                    }

                    groupids.Clear();
                    if (grpset.Equals(0))
                    {
                        //groupids = _FeeGroupHeadContext.feeGGG.Where(t => t.FMGG_Id == data.selected_list[l].grp.FMGG_Id).Select(t => t.FMG_Id).ToList();
                        //added on 03122018
                        gropidssss = (from a in _FeeGroupHeadContext.feeGGG
                                      from b in _FeeGroupHeadContext.FeeGroupDMO
                                      where (b.MI_Id == data.MI_Id && a.FMGG_Id == data.selected_list[l].grp.FMGG_Id && a.FMG_Id == b.FMG_Id && b.FMG_PRE_FLAG == "1")
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                      }
  ).Distinct().ToList();
                        //added on 03122018
                    }
                    else
                    {
                        List<FeeStudentTransactionDTO> gropidsssstemp = new List<FeeStudentTransactionDTO>();
                        List<long> customfmgids = new List<long>();
                        gropidsssstemp = (from a in _FeeGroupHeadContext.feeGGG
                                          from b in _FeeGroupHeadContext.FeeGroupDMO
                                          where (b.MI_Id == data.MI_Id && a.FMGG_Id == data.selected_list[l].grp.FMGG_Id && a.FMG_Id == b.FMG_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMG_Id = a.FMG_Id,
                                          }
).Distinct().ToList();

                        if (gropidsssstemp.Count() > 0)
                        {
                            foreach (var sav1 in gropidsssstemp)
                            {
                                customfmgids.Add(sav1.FMG_Id);
                            }
                        }

                        //groupids = _FeeGroupHeadContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.PASL_ID==Convert.ToInt64(streamid) && t.ASMCL_ID== Convert.ToInt64(classid) && customfmgids.Contains(t.FMG_Id)).Select(t => t.FMG_Id).ToList();

                        groupids = _FeeGroupHeadContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PASL_ID == Convert.ToInt64(streamid) && t.ASMCL_ID == Convert.ToInt64(classid) && customfmgids.Contains(t.FMG_Id)).Select(t => t.FMG_Id).ToList();
                    }

                    //added on 03122018
                    foreach (var x in gropidssss)
                    {
                        groupids.Add(x.FMG_Id);
                    }
                    //added on 03122018



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
                                         from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from f in _FeeGroupHeadContext.FeeGroupDMO
                                         from g in _FeeGroupHeadContext.feeYCC
                                         from h in _FeeGroupHeadContext.feeYCCC
                                         where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && groupids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(classid) && b.ASMAY_Id == g.ASMAY_Id)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName
                                         }
               ).Distinct().ToList();

                        fineheadfmaidsaved = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                              from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                              from c in _FeeGroupHeadContext.FeeHeadDMO
                                              from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                              from f in _FeeGroupHeadContext.FeeGroupDMO
                                              from g in _FeeGroupHeadContext.feeYCC
                                              from h in _FeeGroupHeadContext.feeYCCC
                                              from i in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                                              from j in _FeeGroupHeadContext.feeGGG
                                              from k in _FeeGroupHeadContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                              where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && groupids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && trm_ids.Contains(a.fmt_id) && h.ASMCL_Id == Convert.ToInt32(classid) && i.FMA_Id == b.FMA_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PASA_Id == data.PASR_Id && b.ASMAY_Id == g.ASMAY_Id)
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
                                         from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                         from c in _FeeGroupHeadContext.FeeHeadDMO
                                         from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                         from f in _FeeGroupHeadContext.FeeGroupDMO
                                         from g in _FeeGroupHeadContext.feeYCC
                                         from h in _FeeGroupHeadContext.feeYCCC
                                         where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && groupids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(classid) && b.ASMAY_Id == g.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id)
                                         select new feefinefmaDTO
                                         {
                                             FMA_Id = b.FMA_Id,
                                             FMH_FeeName = c.FMH_FeeName
                                         }
             ).Distinct().ToList();

                        fineheadfmaidsaved = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                              from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                              from c in _FeeGroupHeadContext.FeeHeadDMO
                                              from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                              from f in _FeeGroupHeadContext.FeeGroupDMO
                                              from g in _FeeGroupHeadContext.feeYCC
                                              from h in _FeeGroupHeadContext.feeYCCC
                                              from i in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                                              from j in _FeeGroupHeadContext.feeGGG
                                              from k in _FeeGroupHeadContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                              where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && groupids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && trm_ids.Contains(a.fmt_id) && h.ASMCL_Id == Convert.ToInt32(classid) && i.FMA_Id == b.FMA_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PASA_Id == data.PASR_Id && b.ASMAY_Id == g.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id)
                                              select new feefinefmaDTO
                                              {
                                                  FMA_Id = b.FMA_Id,
                                                  FMH_FeeName = c.FMH_FeeName
                                              }
               ).Distinct().ToList();
                    }





                    if (fineheadfmaidsaved.Count() > 0)
                    {
                        foreach (var sav in fineheadfmaidsaved)
                        {
                            savedfma.Add(sav.FMA_Id);
                        }
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
                                from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                from c in _FeeGroupHeadContext.FeeHeadDMO
                                from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                from e in _FeeGroupHeadContext.FeeGroupDMO
                                from g in _FeeGroupHeadContext.feeYCC
                                from h in _FeeGroupHeadContext.feeYCCC
                                where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && trm_ids.Contains(a.fmt_id) && b.FMA_Amount > 0 && groupids.Contains(a.fmg_id) && (e.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(classid) && !savedfma.Contains(b.FMA_Id) && g.ASMAY_Id == b.ASMAY_Id)
                                select new FeeStudentTransactionDTO
                                {
                                    FMA_Id = b.FMA_Id,
                                    FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount),
                                    FSS_FineAmount = 0,
                                    FSS_ConcessionAmount = 0,
                                    FSS_WaivedAmount = 0,
                                    FMG_Id = b.FMG_Id,
                                    FMH_Id = b.FMH_Id,
                                    FTI_Id = b.FTI_Id,
                                    FSS_PaidAmount = 0,
                                    FSS_NetAmount = 0,
                                    FSS_RefundAmount = 0,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FSS_CurrentYrCharges = Convert.ToInt64(b.FMA_Amount),
                                    FSS_TotalToBePaid = Convert.ToInt64(b.FMA_Amount),
                                }
             ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                    }
                    else
                    {
                        list = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                from c in _FeeGroupHeadContext.FeeHeadDMO
                                from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                from e in _FeeGroupHeadContext.FeeGroupDMO
                                from g in _FeeGroupHeadContext.feeYCC
                                from h in _FeeGroupHeadContext.feeYCCC
                                where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && trm_ids.Contains(a.fmt_id) && b.FMA_Amount > 0 && groupids.Contains(a.fmg_id) && (e.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(classid) && !savedfma.Contains(b.FMA_Id) && g.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id)
                                select new FeeStudentTransactionDTO
                                {
                                    FMA_Id = b.FMA_Id,
                                    FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount),
                                    FSS_FineAmount = 0,
                                    FSS_ConcessionAmount = 0,
                                    FSS_WaivedAmount = 0,
                                    FMG_Id = b.FMG_Id,
                                    FMH_Id = b.FMH_Id,
                                    FTI_Id = b.FTI_Id,
                                    FSS_PaidAmount = 0,
                                    FSS_NetAmount = 0,
                                    FSS_RefundAmount = 0,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FSS_CurrentYrCharges = Convert.ToInt64(b.FMA_Amount),
                                    FSS_TotalToBePaid = Convert.ToInt64(b.FMA_Amount),
                                }
             ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                    }


                    DateTime currdt = DateTime.Now;
                    foreach (var x in list)
                    {
                        amount_list.Add(x);

                        using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Sp_Calculate_Fine";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime, 100)
                            {
                                Value = currdt
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

                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                            {
                                fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                totalfine = totalfine + fineamt;
                            }

                            //if (Convert.ToInt32(fineamt)>0)
                            //{
                            //    finelst.Add(new FeeStudentTransactionDTO
                            //    {
                            //        FMA_Id = fineheadfmaid[0].FMA_Id,
                            //        FSS_ToBePaid = Convert.ToInt32(fineamt),
                            //        FSS_FineAmount = 0,
                            //        FSS_ConcessionAmount = 0,
                            //        FSS_WaivedAmount = 0,
                            //        FMG_Id = 0,
                            //        FMH_Id = 0,
                            //        FTI_Id = 0,
                            //        FSS_PaidAmount = 0,
                            //        FSS_NetAmount = 0,
                            //        FSS_RefundAmount = 0,
                            //        FMH_FeeName = fineheadfmaid[0].FMH_FeeName,
                            //        FTI_Name = "Anytime",
                            //        FSS_CurrentYrCharges = 0,
                            //        FSS_TotalToBePaid = 0,
                            //    });
                            //}
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
                                FMH_FeeName = fineheadfmaid[0].FMH_FeeName,
                                FTI_Name = "Anytime",
                                FSS_CurrentYrCharges = 0,
                                FSS_TotalToBePaid = 0,
                            });
                        }
                    }
                    trm_ids.Clear();
                }

                data.fillstudentviewdetails = amount_list.ToArray();


                data.instalspecial = (from a in _FeeGroupHeadContext.FeeHeadDMO
                                      from d in _FeeGroupHeadContext.feeMIY
                                      from b in _FeeGroupHeadContext.feeMTH
                                      from c in _FeeGroupHeadContext.FeeAmountEntryDMO
                                      from e in _FeeGroupHeadContext.FeeGroupDMO
                                      from f in _FeeGroupHeadContext.feeYCCC
                                      from g in _FeeGroupHeadContext.feeYCC
                                      where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.FYCC_Id == g.FYCC_Id && g.FMCC_Id == c.FMCC_Id && f.ASMCL_Id == Convert.ToInt16(classid) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && trm_idsforspecialfee.Contains(b.FMT_Id) && !savedfma.Contains(c.FMA_Id) && ((c.FMA_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                      select new Head_Installments_DTO
                                      {
                                          FTI_Name = d.FTI_Name,
                                          FTI_Id = c.FTI_Id
                                      }).Distinct().ToList().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public FeeStudentTransactionDTO getdetails(FeeStudentTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _FeeGroupHeadContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                data.ASMAY_Id = Acdemic_preadmission;

                var AdmissionStatus = _FeeGroupHeadContext.AdmissionStatus.FirstOrDefault(d => d.MI_Id == data.MI_Id && d.PAMST_StatusFlag.Contains("CNF")).PAMST_Id;

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _FeeGroupHeadContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Online").ToList();
                data.transnumconfig = masnum.ToArray();

                List<StudentApplication> allRegStudent2 = new List<StudentApplication>();
                allRegStudent2 = _FeeGroupHeadContext.stuapp.Where(t => t.Id == data.userid && t.MI_Id == data.MI_Id && t.PAMS_Id != AdmissionStatus && t.PASR_FinalpaymentFlag == 0).ToList();
                //data.registrationList = allRegStudent.ToArray();
                if (allRegStudent2.Count() > 0)
                {
                    data.dismessage = "Your Seat is not yet confirmed to make Payment";
                }

                List<StudentApplication> allRegStudent1 = new List<StudentApplication>();
                allRegStudent1 = _FeeGroupHeadContext.stuapp.Where(t => t.Id == data.userid && t.MI_Id==data.MI_Id && t.PAMS_Id == AdmissionStatus && (t.PASR_FinalpaymentFlag == 1 || t.PASR_Adm_Confirm_Flag == true)).ToList();
                //data.registrationList = allRegStudent1.ToArray();
                if (allRegStudent1.Count() > 0)
                {
                    var displaymessage = _FeeGroupHeadContext.sMSEmailSetting.Where(t => t.MI_Id == data.MI_Id && t.ISES_Template_Name == "PreadmissionParPayDisMgs").ToList();
                    if (displaymessage.Count > 0)
                    {
                        data.dismessage = displaymessage[0].ISES_SMSMessage;
                    }
                    else
                    {
                        data.dismessage =  "Your Payment is Successful!!!";
                    }
                }

                List<StudentApplication> allRegStudent = new List<StudentApplication>();
                allRegStudent = _FeeGroupHeadContext.stuapp.Where(t => t.Id == data.userid  && t.PAMS_Id == AdmissionStatus && t.PASR_FinalpaymentFlag == 0 && t.PASR_Adm_Confirm_Flag == false).ToList();
                data.registrationList = allRegStudent.ToArray();

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

                List<MOBILE_INSTITUTION> instidet = new List<MOBILE_INSTITUTION>();
                instidet = _FeeGroupHeadContext.MOBILE_INSTITUTION.Where(t => t.MI_ID == data.MI_Id).ToList();
                data.institutiondet = instidet.ToArray();

                //            data.fillinstallment = (from a in _FeeGroupHeadContext.feeTr
                //                                    where (a.MI_Id == data.MI_Id && a.FMT_Order==1) /*&& a.fmg_id.Contains(data.fmg_id)*/
                //                                    select new FeeStudentTransactionDTO
                //                                    {
                //                                        FMT_Name = a.FMT_Name,
                //                                        FMT_Id = a.FMT_Id,
                //                                        feetermorder = a.FMT_Order
                //                                    }
                //).OrderBy(t=>t.feetermorder).Distinct().ToArray();

                //             data.paidstudents = (from a in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                //                                  from b in _FeeGroupHeadContext.Fee_Y_Payment_PA_RegistrationDMO
                //                                  from c in _FeeGroupHeadContext.FeePaymentDetailsDMO
                //                                  where (a.FYP_Id==b.FYP_Id && a.FYP_Id==c.FYP_Id && b.PASR_Id==data.PASR_Id && c.MI_Id==data.MI_Id)
                //                                     select new FeeStudentTransactionDTO
                //                                     {
                //                                       PASR_Id=b.PASR_Id
                //                                     }
                //).Distinct().ToArray();

                //      data.customgrplist = (from a in _FeeGroupHeadContext.feegm
                //                            from b in _FeeGroupHeadContext.feeGGG
                //                            from c in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
                //                            from d in _FeeGroupHeadContext.FeeHeadDMO
                //                            from e in _FeeGroupHeadContext.FeeGroupDMO
                //                            where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.FMG_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && c.FMH_Id==d.FMH_Id && c.FMG_Id==e.FMG_Id && b.FMG_Id==e.FMG_Id && (e.FMG_CompulsoryFlag=="1" || d.FMH_Flag=="N"))
                //                            select new tempgroupDTO
                //                            {
                //                                FMGG_GroupName = a.FMGG_GroupName,
                //                                FMGG_Id = a.FMGG_Id
                //                            }
                //).Distinct().OrderBy(t => t.FMGG_Id).ToArray();

                //      data.termlst = (from a in _FeeGroupHeadContext.feegm
                //                      from b in _FeeGroupHeadContext.feeGGG
                //                      from c in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                //                      from d in _FeeGroupHeadContext.feeTr
                //                      where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id && c.fmt_id == d.FMT_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && d.FMT_ActiveFlag == true)
                //                      select new temotermDTO
                //                      {
                //                          FMGG_GroupName = a.FMGG_GroupName,
                //                          FMGG_Id = a.FMGG_Id,
                //                          FMT_Id = d.FMT_Id,
                //                          FMT_Name = d.FMT_Name
                //                      }
                //  ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO getstudentdetails(FeeStudentTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _FeeGroupHeadContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                var fetchclass = (from a in _FeeGroupHeadContext.stuapp
                                  where (a.MI_Id == data.MI_Id && a.pasr_id == data.PASR_Id)
                                  select new FeeStudentTransactionDTO
                                  {
                                      ASMCL_ID = a.ASMCL_Id,
                                      ASMAY_Id = a.ASMAY_Id,
                                      //pasl_id=a.PASL_ID
                                      ASMST_Id = a.ASMST_Id
                                  }
 ).Distinct().ToArray();

                string classid = "0", academicyearid = "0", streamid = "0";
                for (int s = 0; s < fetchclass.Count(); s++)
                {
                    classid = fetchclass[s].ASMCL_ID.ToString();
                    //streamid = fetchclass[s].pasl_id.ToString();
                    streamid = fetchclass[s].ASMST_Id.ToString();
                }

                var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                 where (a.MI_Id == data.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                        ).FirstOrDefault();

                data.partialorfullpayment = (from a in _FeeGroupHeadContext.MasterConfiguration
                                             where (a.MI_Id == data.MI_Id)
                                             select new FeeStudentTransactionDTO
                                             {
                                                 ISPAC_FullPaymentCompFlg = a.ISPAC_FullPaymentCompFlg
                                             }
                    ).ToArray();

                data.fillpaymentgateway = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                           from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id && b.FPGD_PGActiveFlag == "1")
                                           select new FeeStudentTransactionDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }
).Distinct().ToArray();


                decimal fineheadfmaidsaved = 0;
                if (readterms.IVRMGC_Classwise_Payment != "1")
                {
                    fineheadfmaidsaved = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                          from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                          from c in _FeeGroupHeadContext.FeeHeadDMO
                                          from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                          from f in _FeeGroupHeadContext.FeeGroupDMO
                                          from g in _FeeGroupHeadContext.feeYCC
                                          from h in _FeeGroupHeadContext.feeYCCC
                                          from i in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                                          from j in _FeeGroupHeadContext.feeGGG
                                          from k in _FeeGroupHeadContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                          where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(classid) && i.FMA_Id == b.FMA_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PASA_Id == data.PASR_Id && g.ASMAY_Id == b.ASMAY_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMA_Amount = b.FMA_Amount,
                                              FTI_Id = b.FTI_Id
                                          }
             ).Sum(t => t.FMA_Amount);
                }
                else
                {
                    fineheadfmaidsaved = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                          from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                          from c in _FeeGroupHeadContext.FeeHeadDMO
                                          from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                          from f in _FeeGroupHeadContext.FeeGroupDMO
                                          from g in _FeeGroupHeadContext.feeYCC
                                          from h in _FeeGroupHeadContext.feeYCCC
                                          from i in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                                          from j in _FeeGroupHeadContext.feeGGG
                                          from k in _FeeGroupHeadContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                          where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(classid) && i.FMA_Id == b.FMA_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PASA_Id == data.PASR_Id && g.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMA_Amount = b.FMA_Amount,
                                              FTI_Id = b.FTI_Id
                                          }
             ).Sum(t => t.FMA_Amount);
                }


                data.fineheadfmaidsaved = Convert.ToInt64(fineheadfmaidsaved);

                //List<long> savedfma = new List<long>();
                //if (fineheadfmaidsaved.Count() > 0)
                //{
                //    foreach (var sav in fineheadfmaidsaved)
                //    {
                //        savedfma.Add(sav.FMA_Id);
                //    }
                //}

                int grpset = 0;
                var feemasnum = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToArray();
                for (int s = 0; s < feemasnum.Count(); s++)
                {
                    grpset = Convert.ToInt16(feemasnum[s].fee_group_setting);
                    //grpset = 1;
                }

                if (grpset.Equals(0))
                {
                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        data.filonlinepaymentgrid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                                     from g in _FeeGroupHeadContext.feeYCC
                                                     from h in _FeeGroupHeadContext.feeYCCC
                                                     from i in _FeeGroupHeadContext.Yearlygroups
                                                     where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(classid) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id && b.ASMAY_Id == i.ASMAY_Id)
                                                     select new FeeStudentTransactionDTO
                                                     {
                                                         FSS_NetAmount = Convert.ToDecimal(b.FMA_Amount),
                                                         FSS_ConcessionAmount = 0,
                                                         FSS_FineAmount = 0,
                                                         FSS_ToBePaid = 0,
                                                         FSS_PaidAmount = 0,
                                                         FSS_TotalToBePaid = Convert.ToInt64(b.FMA_Amount),
                                                         FMA_Id = b.FMA_Id,
                                                     }
           ).Distinct().ToArray();
                    }

                    else
                    {
                        data.filonlinepaymentgrid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                                     from g in _FeeGroupHeadContext.feeYCC
                                                     from h in _FeeGroupHeadContext.feeYCCC
                                                     from i in _FeeGroupHeadContext.Yearlygroups
                                                     where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(classid) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id && b.ASMAY_Id == i.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id)
                                                     select new FeeStudentTransactionDTO
                                                     {
                                                         FSS_NetAmount = Convert.ToDecimal(b.FMA_Amount),
                                                         FSS_ConcessionAmount = 0,
                                                         FSS_FineAmount = 0,
                                                         FSS_ToBePaid = 0,
                                                         FSS_PaidAmount = 0,
                                                         FSS_TotalToBePaid = Convert.ToInt64(b.FMA_Amount),
                                                         FMA_Id = b.FMA_Id,
                                                     }
           ).Distinct().ToArray();
                    }

                }
                else
                {
                    //var groupids = _FeeGroupHeadContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PASL_ID == Convert.ToInt64(streamid) && t.ASMCL_ID == Convert.ToInt64(classid)).Select(t => t.FMG_Id).ToList();

                    var groupids = _FeeGroupHeadContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PASL_ID == Convert.ToInt64(streamid) && t.ASMCL_ID == Convert.ToInt64(classid)).Select(t => t.FMG_Id).ToList();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        data.filonlinepaymentgrid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                                     from g in _FeeGroupHeadContext.feeYCC
                                                     from h in _FeeGroupHeadContext.feeYCCC
                                                     from i in _FeeGroupHeadContext.Yearlygroups
                                                     where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(classid) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id && b.ASMAY_Id == i.ASMAY_Id && groupids.Contains(f.FMG_Id) && g.ASMAY_Id == b.ASMAY_Id)
                                                     select new FeeStudentTransactionDTO
                                                     {
                                                         FSS_NetAmount = Convert.ToDecimal(b.FMA_Amount),
                                                         FSS_ConcessionAmount = 0,
                                                         FSS_FineAmount = 0,
                                                         FSS_ToBePaid = 0,
                                                         FSS_PaidAmount = 0,
                                                         FSS_TotalToBePaid = Convert.ToInt64(b.FMA_Amount),
                                                         FMA_Id = b.FMA_Id,
                                                     }
             ).Distinct().ToArray();
                    }
                    else
                    {
                        data.filonlinepaymentgrid = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                                     from g in _FeeGroupHeadContext.feeYCC
                                                     from h in _FeeGroupHeadContext.feeYCCC
                                                     from i in _FeeGroupHeadContext.Yearlygroups
                                                     where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(classid) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id && b.ASMAY_Id == i.ASMAY_Id && groupids.Contains(f.FMG_Id) && g.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id)
                                                     select new FeeStudentTransactionDTO
                                                     {
                                                         FSS_NetAmount = Convert.ToDecimal(b.FMA_Amount),
                                                         FSS_ConcessionAmount = 0,
                                                         FSS_FineAmount = 0,
                                                         FSS_ToBePaid = 0,
                                                         FSS_PaidAmount = 0,
                                                         FSS_TotalToBePaid = Convert.ToInt64(b.FMA_Amount),
                                                         FMA_Id = b.FMA_Id,
                                                     }
             ).Distinct().ToArray();
                    }

                }

                data.customgrplist = (from a in _FeeGroupHeadContext.feegm
                                      from b in _FeeGroupHeadContext.feeGGG
                                      from c in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
                                      from d in _FeeGroupHeadContext.FeeHeadDMO
                                      from e in _FeeGroupHeadContext.FeeGroupDMO
                                      where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.FMG_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && c.FMH_Id == d.FMH_Id && c.FMG_Id == e.FMG_Id && b.FMG_Id == e.FMG_Id && (e.FMG_PRE_FLAG == "1" || d.FMH_Flag == "N") && c.ASMAY_Id == data.ASMAY_Id)
                                      select new tempgroupDTO
                                      {
                                          FMGG_GroupName = a.FMGG_GroupName,
                                          FMGG_Id = a.FMGG_Id
                                      }
        ).Distinct().OrderBy(t => t.FMGG_Id).ToArray();


                var customgrplistt = (from a in _FeeGroupHeadContext.feegm
                                      from b in _FeeGroupHeadContext.feeGGG
                                      from c in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
                                      from d in _FeeGroupHeadContext.FeeHeadDMO
                                      from e in _FeeGroupHeadContext.FeeGroupDMO
                                      where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.FMG_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && c.FMH_Id == d.FMH_Id && c.FMG_Id == e.FMG_Id && b.FMG_Id == e.FMG_Id && (e.FMG_PRE_FLAG == "1" || d.FMH_Flag == "N") && c.ASMAY_Id == data.ASMAY_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMGG_GroupName = a.FMGG_GroupName,
                                          FMGG_Id = a.FMGG_Id,
                                           FMG_Id=e.FMG_Id
                                      }
     ).Distinct().OrderBy(t => t.FMGG_Id).ToArray();

                List<long> cusgroupids = new List<long>();
                 List<long> fmgidss = new List<long>();
                foreach (var x in customgrplistt)
                {
                    cusgroupids.Add(x.FMGG_Id);
                    fmgidss.Add(x.FMG_Id);
                }

                //    data.termlst = (from a in _FeeGroupHeadContext.feegm
                //                    from b in _FeeGroupHeadContext.feeGGG
                //                    from c in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                //                    from d in _FeeGroupHeadContext.feeTr
                //                    from e in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                //                    from f in _FeeGroupHeadContext.PAYUDETAILS
                //                    where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id && c.fmt_id == d.FMT_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && d.FMT_ActiveFlag == true && c.fpgd_id==e.FPGD_Id && e.IMPG_Id==f.IMPG_Id && f.IMPG_ActiveFlg==true)
                //                    select new temotermDTO
                //                    {
                //                        FMGG_GroupName = a.FMGG_GroupName,
                //                        FMGG_Id = a.FMGG_Id,
                //                        FMT_Id = d.FMT_Id,
                //                        FMT_Name = d.FMT_Name,
                //                        PreAdmFlag=c.PreAdmFlag
                //                    }
                //).Distinct().ToArray();

                var getblockedata = (from a in _FeeGroupHeadContext.feegm
                                     from b in _FeeGroupHeadContext.feeGGG
                                     from c in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                     from d in _FeeGroupHeadContext.feeTr
                                     where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id && c.fmt_id == d.FMT_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && d.FMT_ActiveFlag == true && c.PreAdmFlag == true && cusgroupids.Contains(b.FMGG_Id)) /*&& c.ASMCL_Id == Convert.ToInt32(classid)*/
                                     select new FeeStudentTransactionDTO
                                     {
                                         FMH_Id = c.FMH_Id,
                                     }
        ).Distinct().ToList();

                List<long> head_idss = new List<long>();
                foreach (var x in getblockedata)
                {
                    head_idss.Add(x.FMH_Id);
                }

                if (readterms.IVRMGC_Classwise_Payment != "1")
                {
                    data.termlst = (from a in _FeeGroupHeadContext.feegm
                                    from b in _FeeGroupHeadContext.feeGGG
                                    from c in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from d in _FeeGroupHeadContext.feeTr
                                    where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id && c.fmt_id == d.FMT_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && d.FMT_ActiveFlag == true && head_idss.Contains(c.FMH_Id) && cusgroupids.Contains(b.FMGG_Id) && fmgidss.Contains(c.fmg_id)) /*&& c.ASMCL_Id == Convert.ToInt32(classid)*/
                                    select new temotermDTO
                                    {
                                        FMGG_GroupName = a.FMGG_GroupName,
                                        FMGG_Id = a.FMGG_Id,
                                        FMT_Id = d.FMT_Id,
                                        FMT_Name = d.FMT_Name,
                                        PreAdmFlag = c.PreAdmFlag,
                                        FMT_Order = d.FMT_Order
                                    }
         ).Distinct().OrderBy(T => T.FMT_Order).ToArray();
                }
                else
                {
                    data.termlst = (from a in _FeeGroupHeadContext.feegm
                                    from b in _FeeGroupHeadContext.feeGGG
                                    from c in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                    from d in _FeeGroupHeadContext.feeTr
                                    where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id && c.fmt_id == d.FMT_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && d.FMT_ActiveFlag == true && c.ASMCL_Id == Convert.ToInt32(classid) && head_idss.Contains(c.FMH_Id))
                                    select new temotermDTO
                                    {
                                        FMGG_GroupName = a.FMGG_GroupName,
                                        FMGG_Id = a.FMGG_Id,
                                        FMT_Id = d.FMT_Id,
                                        FMT_Name = d.FMT_Name,
                                        PreAdmFlag = c.PreAdmFlag
                                    }
         ).Distinct().ToArray();
                }



                data.fillstudent = (from a in _FeeGroupHeadContext.stuapp
                                    from b in _FeeGroupHeadContext.admissioncls
                                    where (a.MI_Id == data.MI_Id && a.pasr_id == data.PASR_Id && a.ASMCL_Id == b.ASMCL_Id)
                                    select new FeeStudentTransactionDTO
                                    {
                                        PASR_FirstName = a.PASR_FirstName,
                                        PASR_MiddleName = a.PASR_MiddleName,
                                        PASR_LastName = a.PASR_LastName,
                                        PASR_RegistrationNo = a.PASR_RegistrationNo,
                                        PASR_Id = a.pasr_id,
                                        classname = b.ASMCL_ClassName,
                                        pasR_MobileNo = a.PASR_MobileNo,
                                        pasR_emailId = a.PASR_emailId,
                                        ASMCL_ID = a.ASMCL_Id
                                    }
                                       ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO hashgeneration(FeeStudentTransactionDTO data)
        {
            try
            {
                data.fillstudent = (from a in _FeeGroupHeadContext.stuapp
                                    from b in _FeeGroupHeadContext.admissioncls
                                    where (a.MI_Id == data.MI_Id && a.pasr_id == data.PASR_Id && a.ASMCL_Id == b.ASMCL_Id)
                                    select new FeeStudentTransactionDTO
                                    {
                                        PASR_FirstName = a.PASR_FirstName,
                                        PASR_MiddleName = a.PASR_MiddleName,
                                        PASR_LastName = a.PASR_LastName,
                                        PASR_RegistrationNo = a.PASR_RegistrationNo,
                                        PASR_Id = a.pasr_id,
                                        classname = b.ASMCL_ClassName,
                                        pasR_MobileNo = a.PASR_MobileNo,
                                        pasR_emailId = a.PASR_emailId,
                                        ASMCL_ID = a.ASMCL_Id,
                                        AMST_PerCity=a.PASR_PerCity
                                    }
                                      ).Distinct().ToArray();

                if (data.onlinepaygteway.Equals("PAYU"))
                {
                    data.paydet = paymentPart(data, data.topayamount);
                }
                else if (data.onlinepaygteway.Equals("PAYTM"))
                {
                    data.paydet = PAYTMpaymentPart(data, data.topayamount);
                }
                else if (data.onlinepaygteway.Equals("RAZORPAY"))
                {
                    data.paydet = paymentPartrazorpay(data, data.topayamount);
                }
                else if (data.onlinepaygteway.Equals("EASEBUZZ"))
                {
                    data.paydet = paymentParteasebuzz(data, data.topayamount);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Array paymentPart(FeeStudentTransactionDTO enq, long totamount)
        {
            Payment pay = new Payment(_context);
            ProspectusDTO data = new ProspectusDTO();
            int autoinc = 1, totpayableamount = 0;

            var fetchclass = (from a in _FeeGroupHeadContext.stuapp
                              where (a.MI_Id == enq.MI_Id && a.pasr_id == enq.PASR_Id)
                              select new FeeStudentTransactionDTO
                              {
                                  ASMCL_ID = a.ASMCL_Id,
                                  ASMAY_Id = a.ASMAY_Id,
                                  //pasl_id=a.PASL_ID
                                  ASMST_Id = a.ASMST_Id
                              }
   ).Distinct().ToArray();

            string classid = "0", academicyearid = "0";
            for (int s = 0; s < fetchclass.Count(); s++)
            {
                enq.ASMCL_ID = Convert.ToInt64(fetchclass[s].ASMCL_ID);
                enq.ASMAY_Id = Convert.ToInt64(fetchclass[s].ASMAY_Id);
                //enq.pasl_id = Convert.ToInt64(fetchclass[s].pasl_id);
                enq.ASMST_Id = Convert.ToInt64(fetchclass[s].ASMST_Id);
            }

            string multigrp = "0";
            for (int l = 0; l < enq.selected_list.Length; l++)
            {
                List<long> trm_ids = new List<long>();
                foreach (var x in enq.selected_list[l].trm_list)
                {
                    multigrp = multigrp + ',' + x.FMT_Id;
                    trm_ids.Add(x.FMT_Id);
                }
            }

            enq.multiplegroups = multigrp;

            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
            try
            {
                string ids = enq.ftiidss;

                using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "Preadmission_Split_Payment";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                    {
                        Value = enq.MI_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                    SqlDbType.BigInt)
                    {
                        Value = enq.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                    SqlDbType.VarChar)
                    {
                        Value = enq.PASR_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@fmt_id",
                     SqlDbType.VarChar)
                    {
                        Value = enq.multiplegroups
                    });

                    cmd1.Parameters.Add(new SqlParameter("@pgname",
                    SqlDbType.VarChar)
                    {
                        Value = enq.onlinepaygteway
                    });

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    try
                    {
                        using (var dataReader = cmd1.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FeeSlplitOnlinePayment
                                {
                                    name = "splitId" + autoinc.ToString(),
                                    merchantId = dataReader["FPGD_MerchantId"].ToString(),
                                    value = dataReader["balance"].ToString(),
                                    commission = "0",
                                    description = "Online Payment",
                                });

                                autoinc = autoinc + 1;
                            }
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


            List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
            paymentdetails = _FeeGroupHeadContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();

            var receipt = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_Receipt_No);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY.Trim();
            PaymentDetailsDto.amount = Convert.ToDecimal(totamount.ToString());

            if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
            {
                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
            }

            PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
            enq.trans_id = PaymentDetailsDto.trans_id;

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

            PaymentDetailsDto.firstname = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).PASR_FirstName + ' ' + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).PASR_MiddleName + ' ' + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).PASR_LastName;


            PaymentDetailsDto.email = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_emailId;

            PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT.Trim();
            PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
            PaymentDetailsDto.phone = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_MobileNo;
            PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
            PaymentDetailsDto.udf2 = Convert.ToString(enq.PASR_Id);
            PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
            PaymentDetailsDto.udf4 = enq.multiplegroups.ToString();
            PaymentDetailsDto.udf5 = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString();
            PaymentDetailsDto.udf6 = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString();
            PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/PreadmissionOnlinePayment/paymentresponse/";
            PaymentDetailsDto.status = "success";
            PaymentDetailsDto.service_provider = "payu_paisa";

            PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

            var fetchuserid = (from a in _FeeGroupHeadContext.applicationUser
                               from b in _FeeGroupHeadContext.ApplicationUserRole
                               from c in _FeeGroupHeadContext.IVRM_Role_Type
                               where (a.Id == b.UserId && b.RoleTypeId == c.IVRMRT_Id && c.IVRMRT_RoleFlag == "ADMIN")
                               select new FeeStudentTransactionDTO
                               {
                                   userid = Convert.ToInt64(a.Id),
                               }
       ).Distinct().ToArray().Take(1);

            string useridss = "0";
            for (int s = 0; s < fetchuserid.Count(); s++)
            {
                useridss = fetchuserid.FirstOrDefault().userid.ToString();
            }

            int grpset = 0;
            var feemasnum = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == enq.MI_Id).ToArray();
            for (int s = 0; s < feemasnum.Count(); s++)
            {
                grpset = Convert.ToInt16(feemasnum[s].fee_group_setting);
            }

            if (grpset.Equals(0))
            {
                get_grp_reptno(enq);
            }
            else
            {
                get_grp_reptnogroupwise(enq);
            }


            //List<dynamic> rec = new List<dynamic>();

            //foreach(var x in enq.recenocol)
            //{
            //    rec.Add(x);
            //}

            //for (int q = 0; q < rec.Count();q++)
            //{
            //    FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
            //    feepaydet.MI_Id = enq.MI_Id;
            //    feepaydet.ASMAY_ID = Convert.ToInt32(enq.ASMAY_Id);
            //    feepaydet.FTCU_Id = 1;
            //    feepaydet.FYP_Receipt_No = rec[q].receiptno;
            //    feepaydet.FYP_Bank_Name = "";
            //    feepaydet.FYP_Bank_Or_Cash = "O";
            //    feepaydet.FYP_DD_Cheque_No = "";
            //    feepaydet.FYP_DD_Cheque_Date = DateTime.Now;
            //    feepaydet.FYP_Date = DateTime.Now;
            //    feepaydet.FYP_Tot_Amount = rec[q].amt;
            //    feepaydet.FYP_Tot_Waived_Amt = 0;
            //    feepaydet.FYP_Tot_Fine_Amt = 0;
            //    feepaydet.FYP_Tot_Concession_Amt = 0;
            //    feepaydet.FYP_Remarks = "Preadmission Online Payment";
            //    feepaydet.FYP_Chq_Bounce = "CL";
            //    feepaydet.DOE = DateTime.Now;
            //    feepaydet.CreatedDate = DateTime.Now;
            //    feepaydet.UpdatedDate = DateTime.Now;
            //    feepaydet.user_id = Convert.ToInt32(useridss);
            //    feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
            //    feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
            //    feepaydet.FYP_PaymentReference_Id = "";

            //    _FeeGroupHeadContext.FeePaymentDetailsDMO.Add(feepaydet);

            //}

            //_FeeGroupHeadContext.SaveChanges();

            if (enq.recenocol != null)
            {
                Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }

        public string insertdatainfeetables(string miid, string termid, string studentid, string classid, decimal amount, string transid, string refid, string yearid)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            var contactexisttransaction = 0;
            try
            {
                string recnoen = "";
                var fetchfmhotid = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                    where (a.PASR_Id == Convert.ToInt64(studentid) && a.FMOT_Trans_Id == transid.ToString() && a.FMOT_Amount > 0)
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMHOT_Id = a.FMOT_Id,
                                        FMA_Amount = a.FMOT_Amount,
                                        FYP_PayModeType = a.FYP_PayModeType,
                                        FMOT_PayGatewayType=a.FMOT_PayGatewayType

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

                        //added on 12-10-2019
                        var Euserid = (from a in _FeeGroupHeadContext.FeeGroupDMO
                                       where (a.MI_Id == Convert.ToInt64(miid) && grpid.Contains(a.FMG_Id))
                                       select new FeeStudentTransactionDTO
                                       {
                                           enduserid = a.user_id,
                                       }
                           ).Distinct().Take(1).ToArray();
                        //added on 12-10-2019

                        var groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_T_Online_TransactionDMO
                                               from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                               from c in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                               where (a.FMOT_Id == c.FMOT_Id && a.FMA_Id == b.FMA_Id && b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid) && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FMA_Id = b.FMA_Id,
                                                   FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                               }
                             ).ToArray();

                        FeePaymentDetailsDMO onlinemtrans = new FeePaymentDetailsDMO();

                        onlinemtrans.ASMAY_ID = Convert.ToInt64(yearid);
                        onlinemtrans.FTCU_Id = 1;
                        onlinemtrans.FYP_Receipt_No = recnoen;
                        onlinemtrans.FYP_Bank_Name = "";
                        onlinemtrans.FYP_Bank_Or_Cash = "O";
                        onlinemtrans.FYP_DD_Cheque_No = "";
                        onlinemtrans.FYP_DD_Cheque_Date = DateTime.Now;

                        onlinemtrans.FYP_Date = DateTime.Now;
                        onlinemtrans.FYP_Tot_Amount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinemtrans.FYP_Tot_Waived_Amt = 0;
                        onlinemtrans.FYP_Tot_Fine_Amt = 0;
                        onlinemtrans.FYP_Tot_Concession_Amt = 0;
                        onlinemtrans.FYP_Remarks = "Preadmission Full Payment";
                        onlinemtrans.FYP_PayModeType = fetchfmhotid[r].FYP_PayModeType;
                        onlinemtrans.FYP_PayGatewayType = fetchfmhotid[r].FMOT_PayGatewayType;

                        // onlinemtrans.IVRMSTAUL_ID = Convert.ToInt64(studentid);

                        onlinemtrans.FYP_Chq_Bounce = "CL";
                        onlinemtrans.MI_Id = Convert.ToInt64(miid);
                        onlinemtrans.DOE = DateTime.Now;
                        onlinemtrans.CreatedDate = DateTime.Now;
                        onlinemtrans.UpdatedDate = DateTime.Now;
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

                        Fee_Y_Payment_Preadmission_ApplicationDMO onlinestuapp = new Fee_Y_Payment_Preadmission_ApplicationDMO();

                        onlinestuapp.FYP_Id = onlinemtrans.FYP_Id;
                        onlinestuapp.PASA_Id = Convert.ToInt64(studentid);
                        onlinestuapp.FYPPA_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinestuapp.FYPPA_ActiveFlag = 1;
                        onlinestuapp.FYPPA_Type = "A";

                        _FeeGroupHeadContext.Fee_Y_Payment_Preadmission_ApplicationDMO.Add(onlinestuapp);

                        for (int s = 0; s < groupwisefmaids.Count(); s++)
                        {
                            FeeTransactionPaymentDMO onlinettrans = new FeeTransactionPaymentDMO();
                            onlinettrans.FYP_Id = onlinemtrans.FYP_Id;
                            onlinettrans.FMA_Id = groupwisefmaids[s].FMA_Id;
                            onlinettrans.FTP_Paid_Amt = groupwisefmaids[s].FSS_ToBePaid;
                            onlinettrans.FTP_Fine_Amt = 0;
                            onlinettrans.FTP_Concession_Amt = 0;
                            onlinettrans.FTP_Waived_Amt = 0;
                            onlinettrans.ftp_remarks = "Preadmission Full Payment";

                            _FeeGroupHeadContext.FeeTransactionPaymentDMO.Add(onlinettrans);

                        }

                        groupidss = "0";
                    }

                }

                var result = _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Where(t => t.PASR_Id == Convert.ToInt64(studentid) && t.FMOT_Trans_Id == transid).ToArray();

                for (int g = 0; g < result.Count(); g++)
                {
                    result[g].FMOT_Flag = "S";
                    _FeeGroupHeadContext.Update(result[g]);
                }

                string paystatusflag = "Pay";

                var feepartialpaymentflag = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == Convert.ToInt64(miid)).Select(d => d.FMC_Partial_Pre_Payment_flag).FirstOrDefault();

                if (paystatusflag == "Pay" && feepartialpaymentflag != "P")
                {
                    var resultupda = _FeeGroupHeadContext.stuapp.Where(t => t.pasr_id == Convert.ToInt64(studentid)).ToArray();

                    resultupda[0].PASR_FinalpaymentFlag = 1;
                    _FeeGroupHeadContext.Update(resultupda[0]);
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


                if (feepartialpaymentflag == "P")
                {
                    var feepartialpaymentflagamst = _FeeGroupHeadContext.stuapp.Where(t => t.pasr_id == Convert.ToInt32(studentid)).Select(d => d.ASMCL_Id).FirstOrDefault();

                    var termidssss = _FeeGroupHeadContext.feeTr.Where(t => t.MI_Id == Convert.ToInt32(miid) && t.FMT_Order == 1).Select(d => d.FMT_Id).FirstOrDefault();

                    var netamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                                     from g in _FeeGroupHeadContext.feeYCC
                                     from h in _FeeGroupHeadContext.feeYCCC
                                     from i in _FeeGroupHeadContext.Yearlygroups
                                     where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(feepartialpaymentflagamst) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id && b.ASMAY_Id == i.ASMAY_Id && a.fmt_id == termidssss && g.ASMAY_Id == b.ASMAY_Id)
                                     select new FeeStudentTransactionDTO
                                     {
                                         FSS_NetAmount = Convert.ToDecimal(b.FMA_Amount),
                                     }
    ).Sum(t => t.FSS_NetAmount);



                    var paidamount = (from a in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                                      from b in _FeeGroupHeadContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                      where (a.FYP_Id == b.FYP_Id && b.PASA_Id == Convert.ToInt64(studentid))
                                      select new FeeStudentTransactionDTO
                                      {
                                          FSS_PaidAmount = Convert.ToInt64(a.FTP_Paid_Amt),
                                      }
   ).Sum(t => t.FSS_PaidAmount);

                    if (Convert.ToInt64(paidamount) >= Convert.ToInt64(netamount))
                    {
                        paystatusflag = "Pay";
                    }
                    else
                    {
                        paystatusflag = "pending";
                    }


                    if (paystatusflag == "Pay")
                    {
                        var resultupda = _FeeGroupHeadContext.stuapp.Where(t => t.pasr_id == Convert.ToInt64(studentid)).ToArray();

                        resultupda[0].PASR_FinalpaymentFlag = 1;
                        _FeeGroupHeadContext.Update(resultupda[0]);
                        _FeeGroupHeadContext.SaveChanges();
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return contactexisttransaction.ToString();
        }

        public PaymentDetails payuresponse(PaymentDetails response)
        {
            FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
            PaymentDetails dto = new PaymentDetails();

            var emailmobile = (from a in _FeeGroupHeadContext.stuapp
                               where (a.MI_Id == Convert.ToInt32(response.udf3) && a.ASMAY_Id == Convert.ToInt32(response.udf1) && a.pasr_id == Convert.ToInt64(response.udf2))
                               select new FeeStudentTransactionDTO
                               {
                                   pasR_MobileNo = a.PASR_MobileNo,
                                   pasR_emailId = a.PASR_emailId

                               }
                                ).ToArray();

            if (emailmobile.Length > 0)
            {
                for (int i = 0; i < emailmobile.Length; i++)
                {
                    pgmod.amst_email_id = emailmobile[i].pasR_emailId;
                    pgmod.amst_mobile = emailmobile[i].pasR_MobileNo;
                }
            }

            var confirmstatusadmission = 0;
            if (response.status == "success")
            {
                string txnid = response.txnid.ToString();

                //get_grp_reptno(pgmod);

                //var confirmstatus = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Insert_fee_tables_Online_Full_payment_Preadmission @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid, response.udf1);


                var confirmstatus = insertdatainfeetables(response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid.ToString(), response.udf1);

                FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
                var transfersett = (from a in _FeeGroupHeadContext.mstConfig
                                    where (a.MI_Id == Convert.ToInt32(response.udf3) && a.ASMAY_Id == Convert.ToInt32(response.udf1))
                                    select new FeeStudentTransactionDTO
                                    {
                                        transfersettings = a.ISPAC_Transfer_Settings_after_payment_Flag
                                    }
                                        ).ToArray();

                if (transfersett.Length > 0)
                {
                    for (int i = 0; i < transfersett.Length; i++)
                    {
                        data.transfersettings = transfersett[i].transfersettings;
                    }
                }

                if (data.transfersettings == 1)
                {
                    confirmstatusadmission = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission @p0,@p1,@p2", response.udf2, response.udf1, response.udf3);
                }

                if (Convert.ToInt64(confirmstatus) > 0 || confirmstatusadmission > 0)
                {
                    pgmod.MI_Id = Convert.ToInt64(response.udf3);
                    // pgmod.amst_mobile = response.phone;
                    pgmod.Amst_Id = Convert.ToInt64(response.udf2);
                    //  pgmod.amst_email_id = response.email;

                    Email Email = new Email(_context);

                    Email.sendmail(pgmod.MI_Id, pgmod.amst_email_id, "PREADMISSIONFEEONLINEPAYMENT", pgmod.Amst_Id);

                    SMS sms = new SMS(_context);

                    sms.sendSms(pgmod.MI_Id, pgmod.amst_mobile, "PREADMISSIONFEEONLINEPAYMENT", pgmod.Amst_Id);



                    //createusername(pgmod.Amst_Id,);
                }
            }
            else
            {
                pgmod.MI_Id = Convert.ToInt64(response.udf3);
                // pgmod.amst_mobile = response.phone;
                pgmod.Amst_Id = Convert.ToInt64(response.udf2);
                //  pgmod.amst_email_id = response.email;

                Email Email = new Email(_context);

                Email.sendmail(pgmod.MI_Id, pgmod.amst_email_id, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.Amst_Id);

                SMS sms = new SMS(_context);

                sms.sendSms(pgmod.MI_Id, pgmod.amst_mobile, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.Amst_Id);

            }

            return response;
        }

        public FeeStudentTransactionDTO get_grp_reptno(FeeStudentTransactionDTO data)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();

                string onlineheadmapid = "0", groupidss = "0";

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();

                if (data.auto_receipt_flag == 1 || data.auto_receipt_flag == 0)
                {
                    List<long> HeadId = new List<long>();
                    List<long> termids = new List<long>();
                    foreach (var item in data.temarray)
                    {
                        HeadId.Add(item.FMH_Id);
                        termids.Add(item.FMT_Id);
                    }

                    foreach (var item in data.selected_list[0].trm_list)
                    {
                        termids.Add(item.FMT_Id);
                    }

                    List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                    //grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
                    //        from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                    //        where (b.FMG_Id==c.FMG_Id &&  b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))
                    //        select new FeeStudentTransactionDTO
                    //        {
                    //            FGAR_Id = c.FGAR_Id
                    //        }
                    //       ).Distinct().ToList();

                    grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
                            from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                            from d in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                            where (b.FMG_Id == c.FMG_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id) && c.FGAR_Id == d.FGAR_Id && b.ASMAY_Id == d.ASMAY_Id)
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
                                 where (b.FGAR_Id == a.FGAR_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FGAR_Id == Convert.ToInt64(onlineheadmapid))
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
                                        //FGAR_Name = b.FGAR_Name,
                                        //FGAR_Id = c.FGAR_Id
                                    }
                             ).Distinct().ToList();

                        List<feefinefmaDTO> fineheadfmaidsaved = new List<feefinefmaDTO>();
                        var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                         where (a.MI_Id == data.MI_Id)
                                         select new FeeStudentTransactionDTO
                                         {
                                             IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                         }
                        ).FirstOrDefault();
                        if (readterms.IVRMGC_Classwise_Payment != "1")
                        {
                            fineheadfmaidsaved = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                  from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                  from c in _FeeGroupHeadContext.FeeHeadDMO
                                                  from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                  from f in _FeeGroupHeadContext.FeeGroupDMO
                                                  from g in _FeeGroupHeadContext.feeYCC
                                                  from h in _FeeGroupHeadContext.feeYCCC
                                                  from i in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                                                  from j in _FeeGroupHeadContext.feeGGG
                                                  from k in _FeeGroupHeadContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                  from l in _FeeGroupHeadContext.PAYUDETAILS
                                                  from m in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                  where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && l.IMPG_Id == m.IMPG_Id && m.FPGD_Id == a.fpgd_id && l.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && termids.Contains(a.fmt_id) && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && i.FMA_Id == b.FMA_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PASA_Id == data.PASR_Id && b.ASMAY_Id == g.ASMAY_Id && l.IMPG_ActiveFlg == true)
                                                  select new feefinefmaDTO
                                                  {
                                                      FMA_Id = b.FMA_Id,
                                                      FMH_FeeName = c.FMH_FeeName
                                                  }
           ).Distinct().ToList();
                        }
                        else
                        {
                            fineheadfmaidsaved = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                  from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                  from c in _FeeGroupHeadContext.FeeHeadDMO
                                                  from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                  from f in _FeeGroupHeadContext.FeeGroupDMO
                                                  from g in _FeeGroupHeadContext.feeYCC
                                                  from h in _FeeGroupHeadContext.feeYCCC
                                                  from i in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                                                  from j in _FeeGroupHeadContext.feeGGG
                                                  from k in _FeeGroupHeadContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                  from l in _FeeGroupHeadContext.PAYUDETAILS
                                                  from m in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                  where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && l.IMPG_Id == m.IMPG_Id && m.FPGD_Id == a.fpgd_id && l.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && termids.Contains(a.fmt_id) && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && i.FMA_Id == b.FMA_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PASA_Id == data.PASR_Id && b.ASMAY_Id == g.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id && l.IMPG_ActiveFlg == true)
                                                  select new feefinefmaDTO
                                                  {
                                                      FMA_Id = b.FMA_Id,
                                                      FMH_FeeName = c.FMH_FeeName
                                                  }
           ).Distinct().ToList();
                        }


                        List<long> savedfma = new List<long>();
                        if (fineheadfmaidsaved.Count() > 0)
                        {
                            foreach (var sav in fineheadfmaidsaved)
                            {
                                savedfma.Add(sav.FMA_Id);
                            }
                        }

                        long groupwiseamount = 0;
                        if (readterms.IVRMGC_Classwise_Payment != "1")
                        {
                            groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                               from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                               from c in _FeeGroupHeadContext.FeeHeadDMO
                                               from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                               from f in _FeeGroupHeadContext.FeeGroupDMO
                                               from g in _FeeGroupHeadContext.feeYCC
                                               from h in _FeeGroupHeadContext.feeYCCC
                                               from i in _FeeGroupHeadContext.PAYUDETAILS
                                               from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && !savedfma.Contains(b.FMA_Id) && g.ASMAY_Id == b.ASMAY_Id && i.IMPG_ActiveFlg == true)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                               }
        ).Sum(t => t.FSS_ToBePaid);
                        }
                        else
                        {
                            groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                               from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                               from c in _FeeGroupHeadContext.FeeHeadDMO
                                               from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                               from f in _FeeGroupHeadContext.FeeGroupDMO
                                               from g in _FeeGroupHeadContext.feeYCC
                                               from h in _FeeGroupHeadContext.feeYCCC
                                               from i in _FeeGroupHeadContext.PAYUDETAILS
                                               from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && !savedfma.Contains(b.FMA_Id) && g.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id && i.IMPG_ActiveFlg == true)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
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

                            List<FeeStudentTransactionDTO> groupwisefmaids = new List<FeeStudentTransactionDTO>();
                            if (readterms.IVRMGC_Classwise_Payment != "1")
                            {
                                groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                   from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                   from c in _FeeGroupHeadContext.FeeHeadDMO
                                                   from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                   from f in _FeeGroupHeadContext.FeeGroupDMO
                                                   from g in _FeeGroupHeadContext.feeYCC
                                                   from h in _FeeGroupHeadContext.feeYCCC
                                                   from i in _FeeGroupHeadContext.PAYUDETAILS
                                                   from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                   where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && !savedfma.Contains(b.FMA_Id) && g.ASMAY_Id == b.ASMAY_Id && i.IMPG_ActiveFlg == true)
                                                   select new FeeStudentTransactionDTO
                                                   {
                                                       FMA_Id = b.FMA_Id,
                                                       FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                                   }
                               ).ToList();
                            }
                            else
                            {
                                groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                   from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                   from c in _FeeGroupHeadContext.FeeHeadDMO
                                                   from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                   from f in _FeeGroupHeadContext.FeeGroupDMO
                                                   from g in _FeeGroupHeadContext.feeYCC
                                                   from h in _FeeGroupHeadContext.feeYCCC
                                                   from i in _FeeGroupHeadContext.PAYUDETAILS
                                                   from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                   where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && !savedfma.Contains(b.FMA_Id) && g.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id && i.IMPG_ActiveFlg == true)
                                                   select new FeeStudentTransactionDTO
                                                   {
                                                       FMA_Id = b.FMA_Id,
                                                       FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                                   }
                             ).ToList();

                            }


                            Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                            onlinemtrans.FMOT_Trans_Id = data.trans_id;
                            onlinemtrans.FMOT_Amount = rece_amt.amt;
                            onlinemtrans.FMOT_Date = DateTime.Now;
                            onlinemtrans.FMOT_Flag = "P";
                            onlinemtrans.AMST_Id = 0;
                            onlinemtrans.PASR_Id = data.PASR_Id;
                            onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;

                            onlinemtrans.MI_Id = data.MI_Id;
                            onlinemtrans.ASMAY_ID = data.ASMAY_Id;
                            onlinemtrans.FYP_PayModeType = "APP";
                            onlinemtrans.FMOT_PayGatewayType = data.onlinepaygteway;


                            _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                            for (int s = 0; s < groupwisefmaids.Count(); s++)
                            {
                                Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                onlinettrans.FMA_Id = groupwisefmaids[s].FMA_Id;
                                onlinettrans.FTOT_Amount = groupwisefmaids[s].FSS_ToBePaid;
                                onlinettrans.FTOT_Created_date = DateTime.Now;
                                onlinettrans.FTOT_Updated_date = DateTime.Now;
                                onlinettrans.FTOT_Concession = 0;
                                onlinettrans.FTOT_Fine = 0;

                                _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                            }

                            groupidss = "0";
                        }
                    }
                }
                else if (data.automanualreceiptno == "Auto")
                {
                    GenerateTransactionNumbering ab = new GenerateTransactionNumbering(_context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.FYP_Receipt_No = ab.GenerateNumber(data.transnumbconfigurationsettingsss);

                    List<long> HeadId = new List<long>();
                    List<long> termids = new List<long>();
                    foreach (var item in data.temarray)
                    {
                        HeadId.Add(item.FMH_Id);
                        termids.Add(item.FMT_Id);
                    }

                    foreach (var item in data.selected_list[0].trm_list)
                    {
                        termids.Add(item.FMT_Id);
                    }

                    List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                    grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO

                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

                            select new FeeStudentTransactionDTO
                            {
                                FMG_Id = b.FMG_Id
                            }
                           ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    foreach (var item in grps)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    long groupwiseamount = 0;
                    List<FeeStudentTransactionDTO> groupwisefmaids = new List<FeeStudentTransactionDTO>();
                    var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                     where (a.MI_Id == data.MI_Id)
                                     select new FeeStudentTransactionDTO
                                     {
                                         IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                     }
                       ).FirstOrDefault();
                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                           from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                           from c in _FeeGroupHeadContext.FeeHeadDMO
                                           from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                           from f in _FeeGroupHeadContext.FeeGroupDMO
                                           from g in _FeeGroupHeadContext.feeYCC
                                           from h in _FeeGroupHeadContext.feeYCCC
                                           where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && g.ASMAY_Id == b.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                           }
         ).Sum(t => t.FSS_ToBePaid);
                        groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                           from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                           from c in _FeeGroupHeadContext.FeeHeadDMO
                                           from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                           from f in _FeeGroupHeadContext.FeeGroupDMO
                                           from g in _FeeGroupHeadContext.feeYCC
                                           from h in _FeeGroupHeadContext.feeYCCC
                                           where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && g.ASMAY_Id == b.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMA_Id = b.FMA_Id,
                                               FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                           }
                           ).ToList();
                    }
                    else
                    {
                        groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                           from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                           from c in _FeeGroupHeadContext.FeeHeadDMO
                                           from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                           from f in _FeeGroupHeadContext.FeeGroupDMO
                                           from g in _FeeGroupHeadContext.feeYCC
                                           from h in _FeeGroupHeadContext.feeYCCC
                                           where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && g.ASMAY_Id == b.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                           }
        ).Sum(t => t.FSS_ToBePaid);
                        groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                           from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                           from c in _FeeGroupHeadContext.FeeHeadDMO
                                           from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                           from f in _FeeGroupHeadContext.FeeGroupDMO
                                           from g in _FeeGroupHeadContext.feeYCC
                                           from h in _FeeGroupHeadContext.feeYCCC
                                           where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && g.ASMAY_Id == b.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMA_Id = b.FMA_Id,
                                               FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                           }
                            ).ToList();
                    }





                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = data.trans_id;
                    onlinemtrans.FMOT_Amount = groupwiseamount;
                    onlinemtrans.FMOT_Date = DateTime.Now;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.AMST_Id = 0;
                    onlinemtrans.PASR_Id = data.PASR_Id;
                    onlinemtrans.FMOT_Receipt_no = data.FYP_Receipt_No;

                    _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                    for (int s = 0; s < groupwisefmaids.Count(); s++)
                    {
                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                        onlinettrans.FMA_Id = groupwisefmaids[s].FMA_Id;
                        onlinettrans.FTOT_Amount = groupwisefmaids[s].FSS_ToBePaid;
                        onlinettrans.FTOT_Created_date = DateTime.Now;
                        onlinettrans.FTOT_Updated_date = DateTime.Now;
                        onlinettrans.FTOT_Concession = 0;
                        onlinettrans.FTOT_Fine = 0;

                        _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                    }

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
            catch (Exception ex)
            {

            }

            return data;
        }

        //public FeeStudentTransactionDTO get_grp_reptno(FeeStudentTransactionDTO data)
        //{
        //    try
        //    {
        //        var dynamicrecgen = new List<dynamic>();

        //        List<string> mulreceiptno = new List<string>();
        //        List<string> fmgidrec = new List<string>();
        //        if (data.auto_receipt_flag == 1)
        //        {
        //            List<long> HeadId = new List<long>();
        //            List<long> termids = new List<long>();
        //            foreach (var item in data.temarray)
        //            {
        //                HeadId.Add(item.FMH_Id);
        //                termids.Add(item.FMT_Id);
        //            }

        //            foreach (var item in data.selected_list[0].trm_list)
        //            {
        //                termids.Add(item.FMT_Id);
        //            }

        //            List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
        //            grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
        //                    where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))
        //                    select new FeeStudentTransactionDTO
        //                    {
        //                        FMG_Id = b.FMG_Id
        //                    }
        //                   ).Distinct().ToList();

        //            List<long> grpid = new List<long>();
        //            string groupidss = "0";
        //            foreach (var item in grps)
        //            {
        //                grpid.Add(item.FMG_Id);
        //            }

        //            for (int r = 0; r < grpid.Count(); r++)
        //            {
        //                groupidss =  grpid[r].ToString();

        //                List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
        //                List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

        //                list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
        //                            from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
        //                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && c.FMG_Id == Convert.ToInt64(groupidss)  && b.FGAR_Id == c.FGAR_Id)

        //                            select new FeeStudentTransactionDTO
        //                            {
        //                                FGAR_PrefixName = b.FGAR_PrefixName,
        //                                FGAR_SuffixName = b.FGAR_SuffixName,
        //                                FGAR_Id = c.FGAR_Id,
        //                                //FGAR_Name = b.FGAR_Name,
        //                                //FGAR_Id = c.FGAR_Id
        //                            }
        //                     ).Distinct().ToList();

        //                var groupoffmgids = _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO.Where(t => t.FGAR_Id == list_all[0].FGAR_Id).Select(t => t.FMG_Id).ToList();

        //                var groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
        //                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
        //                                     from c in _FeeGroupHeadContext.FeeHeadDMO
        //                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
        //                                     from f in _FeeGroupHeadContext.FeeGroupDMO
        //                                     from g in _FeeGroupHeadContext.feeYCC
        //                                     from h in _FeeGroupHeadContext.feeYCCC
        //                                     where (g.ASMAY_Id==b.ASMAY_Id &&  b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && groupoffmgids.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_CompulsoryFlag == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id))
        //                                     select new FeeStudentTransactionDTO
        //                                     {
        //                                         FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
        //                                     }
        //   ).Sum(t => t.FSS_ToBePaid);

        //                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //                    {
        //                        cmd.CommandText = "receiptnogeneration";
        //                        cmd.CommandType = CommandType.StoredProcedure;
        //                        cmd.Parameters.Add(new SqlParameter("@mi_id",
        //                            SqlDbType.VarChar, 100)
        //                        {
        //                            Value = data.MI_Id
        //                        });

        //                        cmd.Parameters.Add(new SqlParameter("@asmayid",
        //                           SqlDbType.NVarChar, 100)
        //                        {
        //                            Value = data.ASMAY_Id
        //                        });
        //                        cmd.Parameters.Add(new SqlParameter("@fmgid",
        //                       SqlDbType.NVarChar, 100)
        //                        {
        //                            Value = groupidss
        //                        });

        //                        cmd.Parameters.Add(new SqlParameter("@receiptno",
        //            SqlDbType.NVarChar, 500)
        //                        {
        //                            Direction = ParameterDirection.Output
        //                        });

        //                        if (cmd.Connection.State != ConnectionState.Open)
        //                            cmd.Connection.Open();

        //                        var data1 = cmd.ExecuteNonQuery();

        //                        data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();


        //                        var rece_amt = new {  receiptno = data.FYP_Receipt_No, amt = groupwiseamount };

        //                        dynamicrecgen.Add(rece_amt);

        //                        mulreceiptno.Add(data.FYP_Receipt_No);
        //                        fmgidrec.Add(groupwiseamount.ToString());
        //                }
        //            }
        //        }

        //        data.recenocol = dynamicrecgen.ToArray();
        //        //data.recnogrpids = fmgidrec.ToArray();

        //        //else if (data.automanualreceiptno == "Auto")
        //        //{
        //        //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
        //        //    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
        //        //    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
        //        //    data.FYP_Receipt_No = a.GenerateNumber(data.transnumbconfigurationsettingsss);
        //        //}

        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return data;
        //}

        public FeeStudentTransactionDTO get_grp_reptnogroupwise(FeeStudentTransactionDTO data)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();

                string onlineheadmapid = "0", groupidss = "0";

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();

                var groupids = _FeeGroupHeadContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PASL_ID == Convert.ToInt64(data.ASMST_Id) && t.ASMCL_ID == Convert.ToInt64(data.ASMCL_ID)).Select(t => t.FMG_Id).ToList();

                if (data.auto_receipt_flag == 1)
                {
                    List<long> HeadId = new List<long>();
                    List<long> termids = new List<long>();
                    foreach (var item in data.temarray)
                    {
                        HeadId.Add(item.FMH_Id);
                        termids.Add(item.FMT_Id);
                    }

                    foreach (var item in data.selected_list[0].trm_list)
                    {
                        termids.Add(item.FMT_Id);
                    }

                    List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                    grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
                            from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                            from d in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                            where (b.FMG_Id == c.FMG_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id) && c.FGAR_Id == d.FGAR_Id && b.ASMAY_Id == d.ASMAY_Id)
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
                                 where (b.FGAR_Id == a.FGAR_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FGAR_Id == Convert.ToInt64(onlineheadmapid) && groupids.Contains(a.FMG_Id))
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
                                        //FGAR_Name = b.FGAR_Name,
                                        //FGAR_Id = c.FGAR_Id
                                    }
                             ).Distinct().ToList();

                        var fineheadfmaidsaved = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                  from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                  from c in _FeeGroupHeadContext.FeeHeadDMO
                                                  from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                  from f in _FeeGroupHeadContext.FeeGroupDMO
                                                  from g in _FeeGroupHeadContext.feeYCC
                                                  from h in _FeeGroupHeadContext.feeYCCC
                                                  from i in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                                                  from j in _FeeGroupHeadContext.feeGGG
                                                  from k in _FeeGroupHeadContext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                                  where (b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && termids.Contains(a.fmt_id) && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && i.FMA_Id == b.FMA_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PASA_Id == data.PASR_Id && b.ASMAY_Id == g.ASMAY_Id)
                                                  select new feefinefmaDTO
                                                  {
                                                      FMA_Id = b.FMA_Id,
                                                      FMH_FeeName = c.FMH_FeeName
                                                  }
            ).Distinct().ToList();

                        List<long> savedfma = new List<long>();
                        if (fineheadfmaidsaved.Count() > 0)
                        {
                            foreach (var sav in fineheadfmaidsaved)
                            {
                                savedfma.Add(sav.FMA_Id);
                            }
                        }

                        var readterms = (from a in _FeeGroupHeadContext.GenConfig
                                         where (a.MI_Id == data.MI_Id)
                                         select new FeeStudentTransactionDTO
                                         {
                                             IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                         }
                        ).FirstOrDefault();

                        long groupwiseamount = 0;
                        if (readterms.IVRMGC_Classwise_Payment != "1")
                        {
                            groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                               from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                               from c in _FeeGroupHeadContext.FeeHeadDMO
                                               from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                               from f in _FeeGroupHeadContext.FeeGroupDMO
                                               from g in _FeeGroupHeadContext.feeYCC
                                               from h in _FeeGroupHeadContext.feeYCCC
                                               from i in _FeeGroupHeadContext.PAYUDETAILS
                                               from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id 
                                               && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id
                                               && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway &&
                                               b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && !savedfma.Contains(b.FMA_Id) && g.ASMAY_Id == b.ASMAY_Id)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                               }
          ).Sum(t => t.FSS_ToBePaid);
                        }
                        else
                        {
                            groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                               from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                               from c in _FeeGroupHeadContext.FeeHeadDMO
                                               from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                               from f in _FeeGroupHeadContext.FeeGroupDMO
                                               from g in _FeeGroupHeadContext.feeYCC
                                               from h in _FeeGroupHeadContext.feeYCCC
                                               from i in _FeeGroupHeadContext.PAYUDETAILS
                                               from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id 
                                               && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id
                                                && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway &&
                                                b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && !savedfma.Contains(b.FMA_Id) && g.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id)
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
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

                            List<FeeStudentTransactionDTO> groupwisefmaids = new List<FeeStudentTransactionDTO>();
                            if (readterms.IVRMGC_Classwise_Payment != "1")
                            {
                                groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                   from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                   from c in _FeeGroupHeadContext.FeeHeadDMO
                                                   from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                   from f in _FeeGroupHeadContext.FeeGroupDMO
                                                   from g in _FeeGroupHeadContext.feeYCC
                                                   from h in _FeeGroupHeadContext.feeYCCC
                                                   from i in _FeeGroupHeadContext.PAYUDETAILS
                                                   from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                   where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && !savedfma.Contains(b.FMA_Id) && g.ASMAY_Id == b.ASMAY_Id)
                                                   select new FeeStudentTransactionDTO
                                                   {
                                                       FMA_Id = b.FMA_Id,
                                                       FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                                   }
                                 ).Distinct().ToList();
                            }
                            else
                            {
                                groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                                   from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                   from c in _FeeGroupHeadContext.FeeHeadDMO
                                                   from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                                   from f in _FeeGroupHeadContext.FeeGroupDMO
                                                   from g in _FeeGroupHeadContext.feeYCC
                                                   from h in _FeeGroupHeadContext.feeYCCC
                                                   from i in _FeeGroupHeadContext.PAYUDETAILS
                                                   from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                   where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && !savedfma.Contains(b.FMA_Id) && g.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == h.ASMCL_Id)
                                                   select new FeeStudentTransactionDTO
                                                   {
                                                       FMA_Id = b.FMA_Id,
                                                       FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                                   }
                                 ).Distinct().ToList();
                            }


                            Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                            onlinemtrans.FMOT_Trans_Id = data.trans_id;
                            onlinemtrans.FMOT_Amount = rece_amt.amt;
                            onlinemtrans.FMOT_Date = DateTime.Now;
                            onlinemtrans.FMOT_Flag = "P";
                            onlinemtrans.AMST_Id = 0;
                            onlinemtrans.PASR_Id = data.PASR_Id;
                            onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;

                            onlinemtrans.MI_Id = data.MI_Id;
                            onlinemtrans.ASMAY_ID = data.ASMAY_Id;
                            onlinemtrans.FYP_PayModeType = "APP";
                            onlinemtrans.FMOT_PayGatewayType  = data.onlinepaygteway;

                            _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                            for (int s = 0; s < groupwisefmaids.Count(); s++)
                            {
                                Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                onlinettrans.FMA_Id = groupwisefmaids[s].FMA_Id;
                                onlinettrans.FTOT_Amount = groupwisefmaids[s].FSS_ToBePaid;
                                onlinettrans.FTOT_Created_date = DateTime.Now;
                                onlinettrans.FTOT_Updated_date = DateTime.Now;
                                onlinettrans.FTOT_Concession = 0;
                                onlinettrans.FTOT_Fine = 0;

                                _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                            }

                            groupidss = "0";
                        }
                    }
                }
                else if (data.automanualreceiptno == "Auto")
                {
                    GenerateTransactionNumbering ab = new GenerateTransactionNumbering(_context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.FYP_Receipt_No = ab.GenerateNumber(data.transnumbconfigurationsettingsss);

                    List<long> HeadId = new List<long>();
                    List<long> termids = new List<long>();
                    foreach (var item in data.temarray)
                    {
                        HeadId.Add(item.FMH_Id);
                        termids.Add(item.FMT_Id);
                    }

                    foreach (var item in data.selected_list[0].trm_list)
                    {
                        termids.Add(item.FMT_Id);
                    }

                    List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                    grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO

                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

                            select new FeeStudentTransactionDTO
                            {
                                FMG_Id = b.FMG_Id
                            }
                           ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    foreach (var item in grps)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    var groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                           from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                           from c in _FeeGroupHeadContext.FeeHeadDMO
                                           from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                           from f in _FeeGroupHeadContext.FeeGroupDMO
                                           from g in _FeeGroupHeadContext.feeYCC
                                           from h in _FeeGroupHeadContext.feeYCCC
                                           where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && g.ASMAY_Id == b.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                           }
          ).Sum(t => t.FSS_ToBePaid);

                    var groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                           from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                                           from c in _FeeGroupHeadContext.FeeHeadDMO
                                           from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                                           from f in _FeeGroupHeadContext.FeeGroupDMO
                                           from g in _FeeGroupHeadContext.feeYCC
                                           from h in _FeeGroupHeadContext.feeYCCC
                                           where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && (f.FMG_PRE_FLAG == "1" || c.FMH_Flag == "N") && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && g.ASMAY_Id == b.ASMAY_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMA_Id = b.FMA_Id,
                                               FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                                           }
                               ).ToArray();


                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = data.trans_id;
                    onlinemtrans.FMOT_Amount = groupwiseamount;
                    onlinemtrans.FMOT_Date = DateTime.Now;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.AMST_Id = 0;
                    onlinemtrans.PASR_Id = data.PASR_Id;
                    onlinemtrans.FMOT_Receipt_no = data.FYP_Receipt_No;

                    _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                    for (int s = 0; s < groupwisefmaids.Count(); s++)
                    {
                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                        onlinettrans.FMA_Id = groupwisefmaids[s].FMA_Id;
                        onlinettrans.FTOT_Amount = groupwisefmaids[s].FSS_ToBePaid;
                        onlinettrans.FTOT_Created_date = DateTime.Now;
                        onlinettrans.FTOT_Updated_date = DateTime.Now;
                        onlinettrans.FTOT_Concession = 0;
                        onlinettrans.FTOT_Fine = 0;

                        _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                    }

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
            catch (Exception ex)
            {

            }

            return data;
        }

        public Array PAYTMpaymentPart(FeeStudentTransactionDTO enq, long totamount)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            Payment pay = new Payment(_context);
            ProspectusDTO data = new ProspectusDTO();
            int autoinc = 1, totpayableamount = 0;
            var fetchclass = (from a in _FeeGroupHeadContext.stuapp
                              where (a.MI_Id == enq.MI_Id && a.pasr_id == enq.PASR_Id)
                              select new FeeStudentTransactionDTO
                              {
                                  ASMCL_ID = a.ASMCL_Id,
                                  ASMAY_Id = a.ASMAY_Id,
                                  ASMST_Id = a.ASMST_Id
                                  //pasl_id = a.PASL_ID
                              }
   ).Distinct().ToArray();

            string classid = "0", academicyearid = "0";
            for (int s = 0; s < fetchclass.Count(); s++)
            {
                enq.ASMCL_ID = Convert.ToInt64(fetchclass[s].ASMCL_ID);
                enq.ASMAY_Id = Convert.ToInt64(fetchclass[s].ASMAY_Id);
                //enq.pasl_id = Convert.ToInt64(fetchclass[s].pasl_id);
                enq.ASMST_Id = Convert.ToInt64(fetchclass[s].ASMST_Id);
            }

            string multigrp = "0";
            for (int l = 0; l < enq.selected_list.Length; l++)
            {
                List<long> trm_ids = new List<long>();
                foreach (var x in enq.selected_list[l].trm_list)
                {
                    multigrp = multigrp + ',' + x.FMT_Id;
                    trm_ids.Add(x.FMT_Id);
                }
            }

            enq.multiplegroups = multigrp;

            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
            try
            {
                string ids = enq.ftiidss;

                using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "Preadmission_Split_Payment";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                    {
                        Value = enq.MI_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                    SqlDbType.BigInt)
                    {
                        Value = enq.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                    SqlDbType.VarChar)
                    {
                        Value = enq.PASR_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@fmt_id",
                     SqlDbType.VarChar)
                    {
                        Value = enq.multiplegroups
                    });

                    cmd1.Parameters.Add(new SqlParameter("@pgname",
                    SqlDbType.VarChar)
                    {
                        Value = enq.onlinepaygteway
                    });

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    try
                    {
                        using (var dataReader = cmd1.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FeeSlplitOnlinePayment
                                {
                                    name = "splitId" + autoinc.ToString(),
                                    merchantId = dataReader["FPGD_MerchantId"].ToString(),
                                    value = dataReader["balance"].ToString(),
                                    commission = "0",
                                    description = "Online Payment",
                                });

                                autoinc = autoinc + 1;
                            }
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


            List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
            paymentdetails = _FeeGroupHeadContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();

            var receipt = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_Receipt_No);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY.Trim();
            PaymentDetailsDto.amount = Convert.ToDecimal(totamount.ToString());

            if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
            {
                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                //PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);

                PaymentDetailsDto.trans_id = "PrePartial" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();

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


            PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
            enq.trans_id = PaymentDetailsDto.trans_id;

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

            //PaymentDetailsDto.productinfo = payinfo;
            //PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount.ToString());

            //PaymentDetailsDto.firstname = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).PASR_FirstName + ' ' + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).PASR_MiddleName + ' ' + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).PASR_LastName;


            //PaymentDetailsDto.email = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_emailId;

            //PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT.Trim();
            //PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
            //PaymentDetailsDto.phone = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_MobileNo;
            //PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
            //PaymentDetailsDto.udf2 = Convert.ToString(enq.PASR_Id);
            //PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
            //PaymentDetailsDto.udf4 = enq.multiplegroups.ToString();
            //PaymentDetailsDto.udf5 = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString();
            //PaymentDetailsDto.udf6 = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString();
            //PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/PreadmissionOnlinePayment/paymentresponse/";
            //PaymentDetailsDto.status = "success";
            //PaymentDetailsDto.service_provider = "payu_paisa";

            //PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

            var fetchuserid = (from a in _FeeGroupHeadContext.applicationUser
                               from b in _FeeGroupHeadContext.ApplicationUserRole
                               from c in _FeeGroupHeadContext.IVRM_Role_Type
                               where (a.Id == b.UserId && b.RoleTypeId == c.IVRMRT_Id && c.IVRMRT_RoleFlag == "ADMIN")
                               select new FeeStudentTransactionDTO
                               {
                                   userid = Convert.ToInt64(a.Id),
                               }
       ).Distinct().ToArray().Take(1);

            string useridss = "0";
            for (int s = 0; s < fetchuserid.Count(); s++)
            {
                useridss = fetchuserid.FirstOrDefault().userid.ToString();
            }

            int grpset = 0;
            var feemasnum = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == enq.MI_Id).ToArray();
            for (int s = 0; s < feemasnum.Count(); s++)
            {
                grpset = Convert.ToInt16(feemasnum[s].fee_group_setting);
            }

            if (grpset.Equals(0))
            {
                get_grp_reptno(enq);
            }
            else
            {
                get_grp_reptnogroupwise(enq);
            }


            PaymentDetails.PAYTM aa = new PaymentDetails.PAYTM();

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("MID", paymentdet.FirstOrDefault().merchantid);
            parameters.Add("ORDER_ID", PaymentDetailsDto.trans_id);
            parameters.Add("CUST_ID", enq.MI_Id.ToString());
            parameters.Add("TXN_AMOUNT", totpayableamount.ToString());
            parameters.Add("CHANNEL_ID", "WEB");

            //for production
            parameters.Add("INDUSTRY_TYPE_ID", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType);
            parameters.Add("WEBSITE", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website);

            //parameters.Add("INDUSTRY_TYPE_ID", "Retail");
            //parameters.Add("WEBSITE", "WEBSTAGING ");

            parameters.Add("MOBILE_NO", ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_MobileNo.ToString());
            parameters.Add("EMAIL", ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_emailId.Trim());
            parameters.Add("MERC_UNQ_REF", enq.ASMAY_Id.ToString().Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString() + "_" + enq.multiplegroups.ToString().Trim() + "_" + enq.MI_Id.ToString() + "_" + Convert.ToString(enq.PASR_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_emailId.Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_MobileNo + "_" + totpayableamount.ToString());
            string url = paymentdet.FirstOrDefault().merchanturl;
            //parameters.Add("REQUEST_TYPE", "DEFAULT");
            parameters.Add("CALLBACK_URL", "http://localhost:57606/api/PreadmissionOnlinePayment/paymentresponsePAYTM/");

            string checksum = generateCheckSum(paymentdet.FirstOrDefault().merchantkey, parameters);

            aa.MID = paymentdet.FirstOrDefault().merchantid;
            aa.ORDER_ID = PaymentDetailsDto.trans_id;
            aa.CUST_ID = enq.MI_Id.ToString();
            aa.TXN_AMOUNT = Convert.ToDecimal(totpayableamount);
            aa.CHANNEL_ID = "WEB";

            //for production
            aa.INDUSTRY_TYPE_ID = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType;
            aa.WEBSITE = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website;

            //aa.INDUSTRY_TYPE_ID = "Retail";
            //aa.WEBSITE = "WEBSTAGING";

            aa.MOBILE_NO = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_MobileNo;
            aa.EMAIL = ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_emailId.Trim();
            aa.payu_URL = url;

            aa.CHECKSUMHASH = checksum;
            aa.MERC_UNQ_REF = enq.ASMAY_Id.ToString().Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).ASMCL_ID.ToString() + "_" + enq.multiplegroups.ToString().Trim() + "_" + enq.MI_Id.ToString() + "_" + Convert.ToString(enq.PASR_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_emailId.Trim() + "_" + ((FeeStudentTransactionDTO)enq.fillstudent.GetValue(0)).pasR_MobileNo + "_" + totpayableamount.ToString();

            List<PaymentDetails.PAYTM> paydet = new List<PaymentDetails.PAYTM>();
            paydet.Add(aa);

            PaymentDetailsDto.PaymentDetailsList = paydet.ToArray();


            if (enq.recenocol != null)
            {
                Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
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

        public PaymentDetails.PAYTM payuresponsepaytm(PaymentDetails.PAYTM response)
        {
            FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
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

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("MID", paymentdet[0].merchantid);
            parameters.Add("ORDER_ID", tokens[5]);
            parameters.Add("CUST_ID", tokens[6]);
            parameters.Add("TXN_AMOUNT", tokens[9]);
            parameters.Add("CHANNEL_ID", "WEB");

            //for statging
            //parameters.Add("INDUSTRY_TYPE_ID", "Retail");
            //parameters.Add("WEBSITE", "WEBSTAGING ");

            //for production
            parameters.Add("INDUSTRY_TYPE_ID", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType);
            parameters.Add("WEBSITE", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website);

            parameters.Add("MOBILE_NO", tokens[8]);
            parameters.Add("EMAIL", tokens[7]);
            parameters.Add("MERC_UNQ_REF", tokens[0].ToString().Trim() + "_" + tokens[1].ToString() + "_" + tokens[2].ToString() + "_" + tokens[3].ToString() + "_" + Convert.ToString(tokens[4]) + "_" + tokens[5] + "_" + tokens[6].ToString() + "_" + tokens[7].Trim() + "_" + tokens[8] + "_" + tokens[9].ToString());

            string url = paymentdet.FirstOrDefault().merchanturl;
            parameters.Add("CALLBACK_URL", "http://localhost:57606/api/PreadmissionOnlinePayment/paymentresponsePAYTM/");

            string checksum = generateCheckSum(paymentdet[0].merchantkey, parameters);

            // CODE SNIPPET FOR CHECKSUM VALIDATION
            Boolean res = false;
            res = verifyCheckSum(paymentdet[0].merchantkey, parameters, checksum);

            var emailmobile = (from a in _FeeGroupHeadContext.stuapp
                               where (a.MI_Id == Convert.ToInt32(tokens[3].ToString()) && a.ASMAY_Id == Convert.ToInt32(tokens[0].ToString()) && a.pasr_id == Convert.ToInt64(tokens[4]))
                               select new FeeStudentTransactionDTO
                               {
                                   pasR_MobileNo = a.PASR_MobileNo,
                                   pasR_emailId = a.PASR_emailId

                               }
                            ).ToArray();

            if (emailmobile.Length > 0)
            {
                for (int i = 0; i < emailmobile.Length; i++)
                {
                    pgmod.amst_email_id = emailmobile[i].pasR_emailId;
                    pgmod.amst_mobile = emailmobile[i].pasR_MobileNo;
                }
            }

            var confirmstatusadmission = 0;
            if (response.status == "TXN_SUCCESS")
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

                dto.txnid = response.txnid;
                dto.BANKTXNID = response.BANKTXNID;

                //get_grp_reptno(pgmod);

                //var confirmstatus = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Insert_fee_tables_Online_Full_payment_Preadmission @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid, response.udf1);

                var confirmstatus = insertdatainfeetables(dto.udf3, dto.udf4, dto.udf2, dto.udf5, dto.amount, dto.trans_id, response.txnid.ToString(), dto.udf1);

                FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
                var transfersett = (from a in _FeeGroupHeadContext.mstConfig
                                    where (a.MI_Id == Convert.ToInt32(dto.udf3) && a.ASMAY_Id == Convert.ToInt32(dto.udf1))
                                    select new FeeStudentTransactionDTO
                                    {
                                        transfersettings = a.ISPAC_Transfer_Settings_after_payment_Flag
                                    }
                                        ).ToArray();

                if (transfersett.Length > 0)
                {
                    for (int i = 0; i < transfersett.Length; i++)
                    {
                        data.transfersettings = transfersett[i].transfersettings;
                    }
                }

                if (data.transfersettings == 1)
                {
                    confirmstatusadmission = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission @p0,@p1,@p2", dto.udf2, dto.udf1, dto.udf3);
                }

                if (Convert.ToInt64(confirmstatus) > 0 || confirmstatusadmission > 0)
                {
                    pgmod.MI_Id = Convert.ToInt64(dto.udf3);
                    // pgmod.amst_mobile = response.phone;
                    pgmod.Amst_Id = Convert.ToInt64(dto.udf2);
                    //  pgmod.amst_email_id = response.email;

                    Email Email = new Email(_context);

                    Email.sendmail(pgmod.MI_Id, pgmod.amst_email_id, "PREADMISSIONFEEONLINEPAYMENT", pgmod.Amst_Id);

                    SMS sms = new SMS(_context);

                    sms.sendSms(pgmod.MI_Id, pgmod.amst_mobile, "PREADMISSIONFEEONLINEPAYMENT", pgmod.Amst_Id);



                    //createusername(pgmod.Amst_Id,);
                }
            }
            else
            {
                pgmod.MI_Id = Convert.ToInt64(dto.udf3);
                // pgmod.amst_mobile = response.phone;
                pgmod.Amst_Id = Convert.ToInt64(dto.udf2);
                //  pgmod.amst_email_id = response.email;

                Email Email = new Email(_context);

                Email.sendmail(pgmod.MI_Id, pgmod.amst_email_id, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.Amst_Id);

                SMS sms = new SMS(_context);

                sms.sendSms(pgmod.MI_Id, pgmod.amst_mobile, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.Amst_Id);

            }

            return response;
        }

        public Array paymentPartrazorpay(FeeStudentTransactionDTO enq, long totamount)
        {

            Payment pay = new Payment(_context);
            ProspectusDTO data = new ProspectusDTO();
            int autoinc = 1, totpayableamount = 0;

            var fetchclass = (from a in _FeeGroupHeadContext.stuapp
                              where (a.MI_Id == enq.MI_Id && a.pasr_id == enq.PASR_Id)
                              select new FeeStudentTransactionDTO
                              {
                                  ASMCL_ID = a.ASMCL_Id,
                                  ASMAY_Id = a.ASMAY_Id,
                                  //pasl_id=a.PASL_ID
                                  ASMST_Id = a.ASMST_Id
                              }
   ).Distinct().ToArray();

            string classid = "0", academicyearid = "0";
            for (int s = 0; s < fetchclass.Count(); s++)
            {
                enq.ASMCL_ID = Convert.ToInt64(fetchclass[s].ASMCL_ID);
                enq.ASMAY_Id = Convert.ToInt64(fetchclass[s].ASMAY_Id);
                //enq.pasl_id = Convert.ToInt64(fetchclass[s].pasl_id);
                enq.ASMST_Id = Convert.ToInt64(fetchclass[s].ASMST_Id);
            }

            string multigrp = "0";
            for (int l = 0; l < enq.selected_list.Length; l++)
            {
                List<long> trm_ids = new List<long>();
                foreach (var x in enq.selected_list[l].trm_list)
                {
                    multigrp = multigrp + ',' + x.FMT_Id;
                    trm_ids.Add(x.FMT_Id);
                }
            }

            enq.multiplegroups = multigrp;

            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
            try
            {
                string ids = enq.ftiidss;

                using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "Preadmission_Split_Payment";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                    {
                        Value = enq.MI_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                    SqlDbType.BigInt)
                    {
                        Value = enq.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                    SqlDbType.VarChar)
                    {
                        Value = enq.PASR_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@fmt_id",
                     SqlDbType.VarChar)
                    {
                        Value = enq.multiplegroups
                    });

                    cmd1.Parameters.Add(new SqlParameter("@pgname",
                    SqlDbType.VarChar)
                    {
                        Value = enq.onlinepaygteway
                    });

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    try
                    {
                        using (var dataReader = cmd1.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FeeSlplitOnlinePayment
                                {
                                    name = "splitId" + autoinc.ToString(),
                                    merchantId = dataReader["FPGD_MerchantId"].ToString(),
                                    value = dataReader["balance"].ToString(),
                                    commission = "0",
                                    description = "Online Payment",
                                });

                                autoinc = autoinc + 1;
                            }
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


            //List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
            //paymentdetails = _FeeGroupHeadContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();

            //var receipt = _FeeGroupHeadContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_Receipt_No);

            //PaymentDetails PaymentDetailsDto = new PaymentDetails();
            //PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY.Trim();
            //PaymentDetailsDto.amount = Convert.ToDecimal(totamount.ToString());

            //if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
            //{
            //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
            //    enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
            //    enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
            //    PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
            //}

            //PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();
            //enq.trans_id = PaymentDetailsDto.trans_id;

            //PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

            foreach (FeeSlplitOnlinePayment x in result)
            {
                totpayableamount = totpayableamount + Convert.ToInt32(x.value);
            }

            var item = new
            {
                paymentParts = result
            };

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            string payinfo = JsonConvert.SerializeObject(item);

            List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
            paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

            string orderId;

            Dictionary<string, object> input = new Dictionary<string, object>();
            //input.Add("amount", 1 * 100);
            input.Add("amount", totpayableamount * 100); // this amount should be same as transaction amount
            input.Add("currency", "INR");
            input.Add("receipt", PaymentDetailsDto.trans_id);
            input.Add("payment_capture", 1);

            string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
            string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

            RazorpayClient client = new RazorpayClient(key, secret);

            Razorpay.Api.Order order = client.Order.Create(input);
            orderId = order["id"].ToString();

            enq.trans_id = orderId;
            enq.FPGD_AuthorisationKey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
            enq.FMA_Amount = totpayableamount;
            enq.payinfo = payinfo;

            var fetchuserid = (from a in _FeeGroupHeadContext.applicationUser
                               from b in _FeeGroupHeadContext.ApplicationUserRole
                               from c in _FeeGroupHeadContext.IVRM_Role_Type
                               where (a.Id == b.UserId && b.RoleTypeId == c.IVRMRT_Id && c.IVRMRT_RoleFlag == "ADMIN")
                               select new FeeStudentTransactionDTO
                               {
                                   userid = Convert.ToInt64(a.Id),
                               }
       ).Distinct().ToArray().Take(1);

            string useridss = "0";
            for (int s = 0; s < fetchuserid.Count(); s++)
            {
                useridss = fetchuserid.FirstOrDefault().userid.ToString();
            }

            int grpset = 0;
            var feemasnum = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == enq.MI_Id).ToArray();
            for (int s = 0; s < feemasnum.Count(); s++)
            {
                grpset = Convert.ToInt16(feemasnum[s].fee_group_setting);
            }

            if (grpset.Equals(0))
            {
                get_grp_reptno(enq);
            }
            else
            {
                get_grp_reptnogroupwise(enq);
            }

            if (enq.recenocol != null)
            {
                Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }

        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {

            FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
            PaymentDetails dto = new PaymentDetails();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            int r = 0;
            
             //single account added on 17/12/2019

            var accountvalidation = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                     where (a.MI_Id == response.IVRMOP_MIID && a.FPGD_PGName == "RAZORPAY")
                                     select new FeeStudentTransactionDTO
                                     {
                                         FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                     }).Distinct().ToArray();

            //single account added on 17/12/2019
            
            //TRANSFER API

            string url = "https://api.razorpay.com/v1/payments/" + response.razorpay_payment_id + "/transfers";

            List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

            paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.IVRMOP_MIID && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

            RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);
            Razorpay.Api.Payment payment = client.Payment.Fetch(response.razorpay_payment_id);

            response.order_id = payment.Attributes["order_id"];

            var fetchfmhotid = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                where (a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Amount > 0)
                                select new FeeStudentTransactionDTO
                                {
                                    FMHOT_Id = a.FMOT_Id,
                                    FMA_Amount = a.FMOT_Amount,
                                    MI_Id = a.MI_Id,
                                    ASMAY_Id = a.ASMAY_ID,
                                    Amst_Id = a.PASR_Id,
                                }).ToArray();

            var fetchstudentdeatils = (from a in _FeeGroupHeadContext.stuapp
                                       from b in _FeeGroupHeadContext.School_M_Class
                                       where (a.ASMCL_Id==b.ASMCL_Id && a.pasr_id == Convert.ToInt64(fetchfmhotid[0].Amst_Id))
                                       select new FeeStudentTransactionDTO
                                       {
                                           amst_mobile = a.PASR_MobileNo,
                                           amst_email_id = a.PASR_emailId,
                                           ASMCL_ID = b.ASMCL_Id,
                                           AMST_FirstName = a.PASR_FirstName + ' ' + a.PASR_MiddleName + ' ' + a.PASR_LastName,
                                           AMST_AdmNo = a.PASR_RegistrationNo,
                                           Amst_Id = a.pasr_id
                                           //amst_mobile = a.PASR_MobileNo,
                                           //amst_email_id = a.PASR_emailId,
                                           //ASMCL_ID=b.ASMCL_Id
                                       }).ToArray();

            var readterms = (from a in _FeeGroupHeadContext.GenConfig
                             where (a.MI_Id == Convert.ToInt64(fetchfmhotid[0].MI_Id))
                             select new FeeStudentTransactionDTO
                             {
                                 IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                             }
                            ).FirstOrDefault();

            Dictionary<String, object> transfers = new Dictionary<String, object>();
            Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

            List<FeeStudentTransactionDTO> fetchaccountid = new List<FeeStudentTransactionDTO>();
            for (r = 0; r < fetchfmhotid.Count(); r++)
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
                                      where (a.FMA_Id == b.FMA_Id && b.FMG_Id == c.fmg_id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.fti_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && e.IMPG_PGFlag == "RAZORPAY" && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(fetchfmhotid[r].MI_Id) && b.ASMAY_Id == Convert.ToInt64(fetchfmhotid[r].ASMAY_Id) && c.ASMCL_Id== Convert.ToInt64(fetchstudentdeatils[0].ASMCL_ID))
                                      select new FeeStudentTransactionDTO
                                      {
                                          FPGD_SubMerchantId = d.FPGD_SubMerchantId
                                      }).Distinct().ToList();
                }

                var fetchamount = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                   where (a.PASR_Id == Convert.ToInt64(fetchfmhotid[0].Amst_Id) && a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id)
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
                    fet.AMOUNT = fetchamount.FirstOrDefault().FMA_Amount.ToString();
                    fet.CREATED_AT = "";
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

            var confirmstatusadmission = 0;
            if (response.status == "success")
            {

                var confirmstatus = insertdatainfeetables(fetchfmhotid[0].MI_Id.ToString(), "0", fetchfmhotid[0].Amst_Id.ToString(), fetchfmhotid[0].ASMCL_ID.ToString(), Convert.ToInt64(fetchfmhotid[0].FMA_Amount), response.order_id, response.razorpay_payment_id, fetchfmhotid[0].ASMAY_Id.ToString());

                FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
                var transfersett = (from a in _FeeGroupHeadContext.mstConfig
                                    where (a.MI_Id == Convert.ToInt32(fetchfmhotid[0].MI_Id) && a.ASMAY_Id == Convert.ToInt32(fetchfmhotid[0].ASMAY_Id))
                                    select new FeeStudentTransactionDTO
                                    {
                                        transfersettings = a.ISPAC_Transfer_Settings_after_payment_Flag
                                    }
                                        ).ToArray();

                if (transfersett.Length > 0)
                {
                    for (int i = 0; i < transfersett.Length; i++)
                    {
                        data.transfersettings = transfersett[i].transfersettings;
                    }
                }

                if (data.transfersettings == 1)
                {
                    confirmstatusadmission = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission @p0,@p1,@p2", fetchfmhotid[0].Amst_Id, fetchfmhotid[0].ASMAY_Id, fetchfmhotid[0].MI_Id);
                }

                if (Convert.ToInt64(confirmstatus) > 0 || confirmstatusadmission > 0)
                {
                    pgmod.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                    pgmod.Amst_Id = Convert.ToInt64(fetchfmhotid[0].Amst_Id);
                    Email Email = new Email(_context);
                    Email.sendmail(pgmod.MI_Id, fetchstudentdeatils[0].amst_email_id, "PREADMISSIONFEEONLINEPAYMENT", pgmod.Amst_Id);
                    SMS sms = new SMS(_context);
                    sms.sendSms(pgmod.MI_Id, fetchstudentdeatils[0].amst_mobile, "PREADMISSIONFEEONLINEPAYMENT", pgmod.Amst_Id);
                }
            }
            else
            {
                pgmod.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                pgmod.Amst_Id = Convert.ToInt64(fetchfmhotid[0].Amst_Id);
                Email Email = new Email(_context);
                Email.sendmail(pgmod.MI_Id, pgmod.amst_email_id, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.Amst_Id);
                SMS sms = new SMS(_context);
                sms.sendSms(pgmod.MI_Id, pgmod.amst_mobile, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.Amst_Id);

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
                        id = x.Attributes["id"];
                        if (x.Attributes["status"] == "paid")
                        {
                            var registrationpayment = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                                       where (a.MI_Id == response.MI_Id  && a.FMOT_Trans_Id == id && a.AMST_Id == 0)
                                                       select new FeeStudentTransactionDTO
                                                       {
                                                           order_id = a.FMOT_Trans_Id
                                                       }
                         ).ToList();

                            if (registrationpayment.Count > 0)
                            {
                                var pendingpayments = (from a in _FeeGroupHeadContext.FeePaymentDetailsDMO
                                                       from b in _FeeGroupHeadContext.FEE_RAZOR_TRANSFER_API_DETAILS
                                                       from c in _FeeGroupHeadContext.FeeTransactionPaymentDMO
                                                       from d in _FeeGroupHeadContext.FeeAmountEntryDMO
                                                       from e in _FeeGroupHeadContext.FeeGroupDMO
                                                       from f in _FeeGroupHeadContext.FeeHeadDMO
                                                       where (a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMG_Id == e.FMG_Id && d.FMH_Id == f.FMH_Id && a.MI_Id == response.MI_Id && b.ORDER_ID == id && a.fyp_transaction_id == id && a.FYP_OnlineChallanStatusFlag == "Sucessfull" && a.FYP_PayGatewayType == "RAZORPAY" && (e.FMG_CompulsoryFlag == "1" || f.FMH_Flag == "N"))
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

                                    request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));

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
                                            razorgetpaymentresponse(response1);
                                        }
                                    }
                                    //FETCH PAYMENT BASED ON ORDER-ID
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

        public PaymentDetails paytmresponsse(PaymentDetails data)
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

                pendingtransactions = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                       where (a.MI_Id == data.MI_Id && a.FMOT_PayGatewayType == "PAYTM" && a.AMST_Id == 0 && Convert.ToDateTime(a.FMOT_Date.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd")))
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
                        }

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
                                payuresponsepaytm(response);
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

        //Easebuzz 

        public Array paymentParteasebuzz(FeeStudentTransactionDTO enq, long totamount)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            Payment pay = new Payment(_context);
            ProspectusDTO data = new ProspectusDTO();
            int autoinc = 1, totpayableamount = 0;

            var fetchclass = (from a in _FeeGroupHeadContext.stuapp
                              where (a.MI_Id == enq.MI_Id && a.pasr_id == enq.PASR_Id)
                              select new FeeStudentTransactionDTO
                              {
                                  ASMCL_ID = a.ASMCL_Id,
                                  ASMAY_Id = a.ASMAY_Id,
                                  //pasl_id=a.PASL_ID
                                  ASMST_Id = a.ASMST_Id,
                                  AMST_FirstName=a.PASR_FirstName,
                                  amst_mobile=a.PASR_MobileNo,
                                  amst_email_id=a.PASR_emailId

                              }
   ).Distinct().ToArray();

            string classid = "0", academicyearid = "0";
            for (int s = 0; s < fetchclass.Count(); s++)
            {
                enq.ASMCL_ID = Convert.ToInt64(fetchclass[s].ASMCL_ID);
                enq.ASMAY_Id = Convert.ToInt64(fetchclass[s].ASMAY_Id);
                //enq.pasl_id = Convert.ToInt64(fetchclass[s].pasl_id);
                enq.ASMST_Id = Convert.ToInt64(fetchclass[s].ASMST_Id);
            }

            string multigrp = "0";
            for (int l = 0; l < enq.selected_list.Length; l++)
            {
                List<long> trm_ids = new List<long>();
                foreach (var x in enq.selected_list[l].trm_list)
                {
                    multigrp = multigrp + ',' + x.FMT_Id;
                    trm_ids.Add(x.FMT_Id);
                }
            }

            enq.multiplegroups = multigrp;

            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
            try
            {
                string ids = enq.ftiidss;

                using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "Preadmission_Split_Payment";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                    {
                        Value = enq.MI_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                    SqlDbType.BigInt)
                    {
                        Value = enq.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                    SqlDbType.VarChar)
                    {
                        Value = enq.PASR_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@fmt_id",
                     SqlDbType.VarChar)
                    {
                        Value = enq.multiplegroups
                    });

                    cmd1.Parameters.Add(new SqlParameter("@pgname",
                    SqlDbType.VarChar)
                    {
                        Value = enq.onlinepaygteway
                    });

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    try
                    {
                        using (var dataReader = cmd1.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FeeSlplitOnlinePayment
                                {
                                    name = "splitId" + autoinc.ToString(),
                                    merchantId = dataReader["FPGD_MerchantId"].ToString(),
                                    value = dataReader["balance"].ToString(),
                                    commission = "0",
                                    description = "Online Payment",
                                });

                                autoinc = autoinc + 1;
                            }
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

            foreach (FeeSlplitOnlinePayment x in result)
            {
                totpayableamount = totpayableamount + Convert.ToInt32(x.value);
            }

            var item = new
            {
                paymentParts = result
            };

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            string payinfo = JsonConvert.SerializeObject(item);

            List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
            //paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

            var readterms = (from a in _FeeGroupHeadContext.GenConfig
                             where (a.MI_Id == enq.MI_Id)
                             select new FeeStudentTransactionDTO
                             {
                                 IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                             }
                       ).FirstOrDefault();
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

                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway && t.FPGD_SubMerchantId == onlinepayment[0].FPGD_SubMerchantId).Distinct().ToList();

    
            }


            string amount = (totpayableamount).ToString();
            string firstname = fetchclass[0].AMST_FirstName;
            string email = fetchclass[0].amst_email_id;
            string phone = (fetchclass[0].amst_mobile).ToString();

            string surl = "http://localhost:57606/api/PreadmissionOnlinePayment/paymentresponseeasybuzz/";
            string furl = "http://localhost:57606/api/PreadmissionOnlinePayment/paymentresponseeasybuzz/";



           
                enq.trans_id =  enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();

          
            string Txnid = (enq.trans_id).ToString();
            string UDF1 = (enq.PASR_Id).ToString();

            string UDF2 = (enq.userid).ToString();

            string UDF3 = (enq.MI_Id).ToString();

            string UDF4 = (enq.ASMAY_Id).ToString();

            string UDF5 ="";

            string UDF6 = "";

            string UDF7 = "";
            string UDF8 = "";
            string UDF9 = "";
            string UDF10 = "";
            string productinfo = (fetchclass[0].ASMCL_ID).ToString();
            string Show_payment_mode = "";

            Dictionary<string, long> transfersnotescash = new Dictionary<string, long>();
        
            if (result.Count > 0)
            {
               
                for (var j = 0; j < result.Count; j++)
                {
                   
                    transfersnotescash.Add(result[j].merchantId.ToString(), Convert.ToInt64(result[j].value));

                }

                
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

            string strForm = t1.initiatePaymentAPI(amount, firstname, email, phone, productinfo, surl, furl, Txnid, UDF1, UDF2, UDF3, UDF4, UDF5, UDF6, UDF7, UDF8, UDF9, UDF10, Show_payment_mode, split_payments, sub_merchant_id);

            enq.FYP_Tot_Amount = totpayableamount;
            enq.FYP_Tot_Amount = totpayableamount;
            enq.strdatanew = strForm;

            //string orderId;

            //Dictionary<string, object> input = new Dictionary<string, object>();
            ////input.Add("amount", 1 * 100);
            //input.Add("amount", totpayableamount * 100); 
            //input.Add("currency", "INR");
            //input.Add("receipt", PaymentDetailsDto.trans_id);
            //input.Add("payment_capture", 1);

            //string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
            //string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

            //RazorpayClient client = new RazorpayClient(key, secret);

            //Razorpay.Api.Order order = client.Order.Create(input);
            //orderId = order["id"].ToString();

            //enq.trans_id = orderId;
            //enq.FPGD_AuthorisationKey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
            //enq.FMA_Amount = totpayableamount;
            //enq.payinfo = payinfo;

            var fetchuserid = (from a in _FeeGroupHeadContext.applicationUser
                               from b in _FeeGroupHeadContext.ApplicationUserRole
                               from c in _FeeGroupHeadContext.IVRM_Role_Type
                               where (a.Id == b.UserId && b.RoleTypeId == c.IVRMRT_Id && c.IVRMRT_RoleFlag == "ADMIN")
                               select new FeeStudentTransactionDTO
                               {
                                   userid = Convert.ToInt64(a.Id),
                               }
       ).Distinct().ToArray().Take(1);

            string useridss = "0";
            for (int s = 0; s < fetchuserid.Count(); s++)
            {
                useridss = fetchuserid.FirstOrDefault().userid.ToString();
            }

            int grpset = 0;
            var feemasnum = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == enq.MI_Id).ToArray();
            for (int s = 0; s < feemasnum.Count(); s++)
            {
                grpset = Convert.ToInt16(feemasnum[s].fee_group_setting);
            }

            if (grpset.Equals(0))
            {
                get_grp_reptno(enq);
            }
            else
            {
                get_grp_reptnogroupwise(enq);
            }

            if (enq.recenocol != null)
            {
                Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
            }

            return PaymentDetailsDto.PaymentDetailsList;

        }

        public PaymentDetails.easybuzz getpaymentresponseeasybuzz(PaymentDetails.easybuzz response)
        {

            FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
            PaymentDetails dto = new PaymentDetails();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            int r = 0;

            //single account added on 17/12/2019

            var accountvalidation = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                     where (a.MI_Id == Convert.ToInt64(response.UDF3) && a.FPGD_PGName == "EASEBUZZ")
                                     select new FeeStudentTransactionDTO
                                     {
                                         FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                     }).Distinct().ToArray();

            //single account added on 17/12/2019


            var readterms = (from a in _FeeGroupHeadContext.GenConfig
                             where (a.MI_Id == Convert.ToInt64(response.UDF3))
                             select new FeeStudentTransactionDTO
                             {
                                 IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                             }
                            ).FirstOrDefault();


            List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

            if (readterms.IVRMGC_Classwise_Payment != "1")
            {
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(q => q.MI_Id == Convert.ToInt64(response.UDF3) && q.FPGD_PGName == "EASEBUZZ").Distinct().ToList();

            }
            else
            {
                var onlinepayment = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping

                                     from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                     where (a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == "EASEBUZZ" && a.MI_Id == Convert.ToInt64(response.UDF3) && a.ASMCL_Id == Convert.ToInt64(response.productinfo))
                                     select new FeeStudentTransactionDTO
                                     {
                                         FPGD_SubMerchantId = c.FPGD_SubMerchantId
                                     }
).ToList();

                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(w => w.MI_Id == Convert.ToInt64(response.UDF3) && w.FPGD_PGName == "EASEBUZZ" && w.FPGD_SubMerchantId == onlinepayment[0].FPGD_SubMerchantId).Distinct().ToList();


            }

            //    paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(z => z.MI_Id == Convert.ToInt64(response.UDF3) && z.FPGD_PGName == "EASEBUZZ").Distinct().ToList();


            string txnid = response.txnid.ToString();
            string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
            string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;

            string env = "prod";
            string paymentref = "";
            Easebuzz t = new Easebuzz(secret, key, env);

            var res = t.transactionAPI(txnid, response.amount, response.email, response.phone);

            JObject joResponse1 = JObject.Parse(res);

            paymentref = joResponse1["msg"]["easepayid"].ToString();
           // paymentref = "";



            var fetchfmhotid = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                where (a.FMOT_Trans_Id == response.txnid.ToString() && a.FMOT_Amount > 0)
                                select new FeeStudentTransactionDTO
                                {
                                    FMHOT_Id = a.FMOT_Id,
                                    FMA_Amount = a.FMOT_Amount,
                                    MI_Id = a.MI_Id,
                                    ASMAY_Id = a.ASMAY_ID,
                                    Amst_Id = a.PASR_Id,
                                }).ToArray();

            var fetchstudentdeatils = (from a in _FeeGroupHeadContext.stuapp
                                       from b in _FeeGroupHeadContext.School_M_Class
                                       where (a.ASMCL_Id == b.ASMCL_Id && a.pasr_id == Convert.ToInt64(fetchfmhotid[0].Amst_Id))
                                       select new FeeStudentTransactionDTO
                                       {
                                           amst_mobile = a.PASR_MobileNo,
                                           amst_email_id = a.PASR_emailId,
                                           ASMCL_ID = b.ASMCL_Id,
                                           AMST_FirstName = a.PASR_FirstName + ' ' + a.PASR_MiddleName + ' ' + a.PASR_LastName,
                                           AMST_AdmNo = a.PASR_RegistrationNo,
                                           Amst_Id = a.pasr_id
                                    
                                       }).ToArray();




            var confirmstatusadmission = 0;
            if (response.status == "success")
            {

                var confirmstatus = insertdatainfeetables(fetchfmhotid[0].MI_Id.ToString(), "0", fetchfmhotid[0].Amst_Id.ToString(), fetchfmhotid[0].ASMCL_ID.ToString(), Convert.ToInt64(fetchfmhotid[0].FMA_Amount), response.txnid, paymentref, fetchfmhotid[0].ASMAY_Id.ToString());

                FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
                var transfersett = (from a in _FeeGroupHeadContext.mstConfig
                                    where (a.MI_Id == Convert.ToInt32(fetchfmhotid[0].MI_Id) && a.ASMAY_Id == Convert.ToInt32(fetchfmhotid[0].ASMAY_Id))
                                    select new FeeStudentTransactionDTO
                                    {
                                        transfersettings = a.ISPAC_Transfer_Settings_after_payment_Flag
                                    }
                                        ).ToArray();

                if (transfersett.Length > 0)
                {
                    for (int i = 0; i < transfersett.Length; i++)
                    {
                        data.transfersettings = transfersett[i].transfersettings;
                    }
                }

                if (data.transfersettings == 1)
                {
                    confirmstatusadmission = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission @p0,@p1,@p2", fetchfmhotid[0].Amst_Id, fetchfmhotid[0].ASMAY_Id, fetchfmhotid[0].MI_Id);
                }

                if (Convert.ToInt64(confirmstatus) > 0 || confirmstatusadmission > 0)
                {
                    pgmod.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                    pgmod.Amst_Id = Convert.ToInt64(fetchfmhotid[0].Amst_Id);
                    Email Email = new Email(_context);
                    Email.sendmail(pgmod.MI_Id, fetchstudentdeatils[0].amst_email_id, "PREADMISSIONFEEONLINEPAYMENT", pgmod.Amst_Id);
                    SMS sms = new SMS(_context);
                    sms.sendSms(pgmod.MI_Id, fetchstudentdeatils[0].amst_mobile, "PREADMISSIONFEEONLINEPAYMENT", pgmod.Amst_Id);
                }
            }
            else
            {
                pgmod.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                pgmod.Amst_Id = Convert.ToInt64(fetchfmhotid[0].Amst_Id);
                Email Email = new Email(_context);
                Email.sendmail(pgmod.MI_Id, pgmod.amst_email_id, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.Amst_Id);
                SMS sms = new SMS(_context);
                sms.sendSms(pgmod.MI_Id, pgmod.amst_mobile, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.Amst_Id);

            }

            return response;
        }





    }
}
















