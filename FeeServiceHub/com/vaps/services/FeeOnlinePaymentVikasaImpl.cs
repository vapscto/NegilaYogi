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
using System.Security.Cryptography;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeOnlinePaymentVikasaImpl : interfaces.FeeOnlinePaymentVikasaInterface
    {
        private static ConcurrentDictionary<string, FeeStudentTransactionDTO> _login =
      new ConcurrentDictionary<string, FeeStudentTransactionDTO>();

        public DomainModelMsSqlServerContext _context;
        public FeeGroupContext _FeeGroupHeadContext;
        readonly ILogger<FeeOnlinePaymentVikasaImpl> _logger;

        public FeeOnlinePaymentVikasaImpl(FeeGroupContext frgContext, ILogger<FeeOnlinePaymentVikasaImpl> log, DomainModelMsSqlServerContext context)
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

                data.ASMAY_Id = getacademicyearcongig(data);

                long clsid = getcurrentclass(data);

                data.ASMCL_ID = clsid;



                //data.ASMCL_ID = 1;

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
                                    FSS_OBArrearAmount = b.FSS_OBArrearAmount
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
                                    FSS_OBArrearAmount = b.FSS_OBArrearAmount
                                }
             ).OrderBy(t => t.FTI_Id).ToList();
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


                //                data.fillpaymentgateway = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                //                                           where (a.MI_Id == data.MI_Id && a.FPGD_PGActiveFlag == "1")
                //                                           select new FeeStudentTransactionDTO
                //                                           {
                //                                               FPGD_PGName = a.FPGD_PGName,
                //                                           }
                //).Distinct().ToArray();

                data.fillpaymentgateway = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                           from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }
                                             ).Distinct().ToArray();

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

            // enq.ASMCL_ID = clsid;
            //enq.ASMCL_ID = 1;

            List<long> HeadId1 = new List<long>();

            foreach (var item in enq.temarray)
            {
                HeadId1.Add(item.FMH_Id);

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
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && HeadId1.Contains(a.FMH_Id))
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
                                        where (a.fpgd_id == b.FPGD_Id && grp_ids.Contains(a.fmg_id) && a.MI_Id == enq.MI_Id && a.ASMCL_Id == enq.ASMCL_ID && trm_ids.Contains(a.fmt_id) && HeadId1.Contains(a.FMH_Id))
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
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && HeadId1.Contains(c.FMH_Id))
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
                                         where (a.fpgd_id == e.FPGD_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && c.FMH_Flag == "F" && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID && HeadId1.Contains(c.FMH_Id))
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
                                    where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && HeadId1.Contains(c.FMH_Id))
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
                                    where (a.ASMCL_Id == enq.ASMCL_ID && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && grp_ids.Contains(a.fmg_id) && b.FMG_Id == f.FMG_Id && h.ASMCL_Id == Convert.ToInt32(enq.ASMCL_ID) && trm_ids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.FMA_Id == b.FMA_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.AMST_Id == enq.Amst_Id && i.ASMAY_Id == enq.ASMAY_Id && HeadId1.Contains(c.FMH_Id))
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



                    DateTime currdt = DateTime.Now;

                    foreach (var x in list123)
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

                            //fineamt = fineamt + cmd.Parameters["@amt"].Value.ToString();
                            //fineamt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                            if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
                            {
                                fineamt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
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

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        enq.pendingamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                             from b in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && HeadId1.Contains(a.FMH_Id))
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
                                             where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMST_Id == enq.Amst_Id && trm_ids.Contains(a.fmt_id) && b.FSS_ToBePaid > 0 && grp_ids.Contains(a.fmg_id) && a.ASMCL_Id == enq.ASMCL_ID && HeadId1.Contains(a.FMH_Id))
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
                        value = (Convert.ToInt32(enq.pendingamount) + totalfine).ToString(),
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
                //PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().FPGD_SaltKey.Trim();
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
                PaymentDetailsDto.transaction_response_url = "http://localhost:49540/api/" + PaymentDetailsDto.productinfo + "/paymentresponse/";
                PaymentDetailsDto.status = "success";
                PaymentDetailsDto.service_provider = "payu_paisa";

                PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

                get_grp_reptno(enq);



                //FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
                //feepaydet.MI_Id = enq.MI_Id;
                //feepaydet.ASMAY_ID = enq.ASMAY_Id;

                //feepaydet.FTCU_Id = 1;
                //feepaydet.FYP_Receipt_No = PaymentDetailsDto.trans_id;
                //feepaydet.FYP_Bank_Name = "";
                //feepaydet.FYP_Bank_Or_Cash = "O";
                //feepaydet.FYP_DD_Cheque_No = "";
                //feepaydet.FYP_DD_Cheque_Date = DateTime.Now;
                //feepaydet.FYP_Date = DateTime.Now;
                //feepaydet.FYP_Tot_Amount = PaymentDetailsDto.amount;
                //feepaydet.FYP_Tot_Waived_Amt = 0;
                //feepaydet.FYP_Tot_Fine_Amt = 0;
                //feepaydet.FYP_Tot_Concession_Amt = 0;
                //feepaydet.FYP_Remarks = "Online Payment";
                //feepaydet.FYP_Chq_Bounce = "CL";
                //feepaydet.DOE = DateTime.Now;
                //feepaydet.CreatedDate = DateTime.Now;
                //feepaydet.UpdatedDate = DateTime.Now;
                //feepaydet.user_id = enq.userid;
                //feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
                //feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
                //feepaydet.FYP_PaymentReference_Id = "";

                //_FeeGroupHeadContext.FeePaymentDetailsDMO.Add(feepaydet);

                //for (int q=0;q< amount_list.Count();q++)
                //{
                //    FeeTransactionPaymentDMO tra = new FeeTransactionPaymentDMO();
                //    tra.FMA_Id = amount_list[q].FMA_Id;
                //    tra.FYP_Id = feepaydet.FYP_Id;
                //    tra.FTP_Paid_Amt= amount_list[q].FSS_ToBePaid;
                //    tra.FTP_Fine_Amt = amount_list[q].FSS_FineAmount;
                //    tra.FTP_Concession_Amt = amount_list[q].FSS_ConcessionAmount;
                //    tra.FTP_Waived_Amt = amount_list[q].FSS_WaivedAmount;
                //    tra.ftp_remarks = "Online Payment";

                //    _FeeGroupHeadContext.FeeTransactionPaymentDMO.Add(tra);
                //}

                //for (int b = 0; b < finelst.Count(); b++)
                //{
                //    FeeTransactionPaymentDMO tra = new FeeTransactionPaymentDMO();
                //    tra.FMA_Id = finelst[b].FMA_Id;
                //    tra.FYP_Id = finelst[b].FYP_Id;
                //    tra.FTP_Paid_Amt = finelst[b].FSS_ToBePaid;
                //    tra.FTP_Fine_Amt = 0;
                //    tra.FTP_Concession_Amt = 0;
                //    tra.FTP_Waived_Amt = 0;
                //    tra.ftp_remarks = "Online Payment";

                //    _FeeGroupHeadContext.FeeTransactionPaymentDMO.Add(tra);
                //}

                //var contactexisttransaction = 0;
                //using (var dbCtxTxn = _FeeGroupHeadContext.Database.BeginTransaction())
                //{
                //    try
                //    {
                //        contactexisttransaction = _FeeGroupHeadContext.SaveChanges();
                //        dbCtxTxn.Commit();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //        dbCtxTxn.Rollback();
                //    }
                //}

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
                                where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id && c.fmt_id == d.FMT_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && d.FMT_ActiveFlag == true && grp_ids.Contains(b.FMG_Id))
                                select new temotermDTO
                                {
                                    FMGG_GroupName = a.FMGG_GroupName,
                                    FMGG_Id = a.FMGG_Id,
                                    FMT_Id = d.FMT_Id,
                                    FMT_Name = d.FMT_Name
                                }
            ).Distinct().ToArray();


                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _FeeGroupHeadContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Online").ToList();
                data.transnumconfig = masnum.ToArray();

                List<MasterAcademic> acade = new List<MasterAcademic>();
                acade = _FeeGroupHeadContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1).ToList();
                data.academicyrlist = acade.ToArray();

                List<FeeMasterConfigurationDMO> configg = new List<FeeMasterConfigurationDMO>();
                configg = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.readterms = configg.ToArray();

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

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                DateTime curdt = DateTime.Now;
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
                List<long> termids = new List<long>();
                foreach (var item in data.temarray)
                {
                    HeadId.Add(item.FMH_Id);
                    termids.Add(item.FMT_Id);
                }

                //foreach (var item in data.selected_list[0].trm_list)
                //{
                //    termids.Add(item.FMT_Id);
                //}

                //foreach (var item in data.selected_list)
                //{
                //    foreach (var item1 in item.trm_list)
                //    {
                //        termids.Add(item1.FMT_Id);
                //    }

                //}


                List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
                        from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                        from d in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                        where (c.FGAR_Id == d.FGAR_Id && d.ASMAY_Id == b.ASMAY_Id && b.FMG_Id == c.FMG_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))
                        select new FeeStudentTransactionDTO
                        {
                            FGAR_Id = c.FGAR_Id
                        }
                       ).Distinct().ToList();




                for (int r = 0; r < grps.Count(); r++)
                {
                    foreach (var item10 in data.temarray)
                    {

                        termids.Add(item10.FMT_Id);
                    }

                    onlineheadmapid = grps[r].FGAR_Id.ToString();

                    List<FeeStudentTransactionDTO> grps1 = new List<FeeStudentTransactionDTO>();
                    grps1 = (from a in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                             from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                             from c in _FeeGroupHeadContext.FeeStudentTransactionDMO
                             where (b.FGAR_Id == a.FGAR_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FGAR_Id == Convert.ToInt64(onlineheadmapid) && c.FMG_Id == a.FMG_Id && c.AMST_Id == data.Amst_Id && HeadId.Contains(c.FMH_Id))
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

                    //              var groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                    //                                     from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                    //                                     from c in _FeeGroupHeadContext.FeeHeadDMO
                    //                                     from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                    //                                     from f in _FeeGroupHeadContext.FeeGroupDMO
                    //                                     from g in _FeeGroupHeadContext.feeYCC
                    //                                     from h in _FeeGroupHeadContext.feeYCCC
                    //                                     from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                    //                                     where (i.FMA_Id==b.FMA_Id && a.FMH_Id==i.FMH_Id && a.fmg_id==i.FMG_Id && i.FTI_Id==a.fti_id && g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && i.AMST_Id==data.Amst_Id)
                    //                                     select new FeeStudentTransactionDTO
                    //                                     {
                    //                                         FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid)
                    //                                     }
                    //).Sum(t => t.FSS_ToBePaid);

                    //List<FeeStudentTransactionDTO> groupwiseamount = new List<FeeStudentTransactionDTO>();
                    decimal groupwiseamount = 0;
                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                           from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                           where (a.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && i.FSS_ToBePaid > 0 && termids.Contains(a.fmt_id) && i.AMST_Id == data.Amst_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && HeadId.Contains(a.FMH_Id))
                                           select new FeeStudentTransactionDTO
                                           {
                                               FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid)
                                           }
  ).Sum(t => t.FSS_ToBePaid);

                        //                      groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                        //                                             from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                        //                                             from c in _FeeGroupHeadContext.FeeHeadDMO
                        //                                             from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                        //                                             from f in _FeeGroupHeadContext.FeeGroupDMO
                        //                                             from g in _FeeGroupHeadContext.feeYCC
                        //                                             from h in _FeeGroupHeadContext.feeYCCC
                        //                                             from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                        //                                             where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMA_Amount > 0 && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && i.AMST_Id == data.Amst_Id && a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && b.FMH_Id == c.FMH_Id && b.FMG_Id == f.FMG_Id && d.FTI_Id == b.FTI_Id && g.FMCC_Id == b.FMCC_Id && g.FYCC_Id == h.FYCC_Id && i.FMG_Id == b.FMG_Id && i.FMH_Id == b.FMH_Id && i.FTI_Id == b.FTI_Id && g.ASMAY_Id == b.ASMAY_Id)
                        //                                             select new FeeStudentTransactionDTO
                        //                                             {
                        //                                                 FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid)
                        //                                             }
                        //).Sum(t => t.FSS_ToBePaid);

                    }
                    else
                    {
                        groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                           from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                           where (a.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && i.FSS_ToBePaid > 0 && termids.Contains(a.fmt_id) && i.AMST_Id == data.Amst_Id && a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && a.ASMCL_Id == data.ASMCL_ID && HeadId.Contains(a.FMH_Id))
                                           select new FeeStudentTransactionDTO
                                           {
                                               FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid)
                                           }
  ).Sum(t => t.FSS_ToBePaid);

                        //                     groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                        //                                            from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                        //                                            from c in _FeeGroupHeadContext.FeeHeadDMO
                        //                                            from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                        //                                            from f in _FeeGroupHeadContext.FeeGroupDMO
                        //                                            from g in _FeeGroupHeadContext.feeYCC
                        //                                            from h in _FeeGroupHeadContext.feeYCCC
                        //                                            from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                        //                                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMA_Amount > 0 && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && i.AMST_Id == data.Amst_Id && a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && b.FMH_Id == c.FMH_Id && b.FMG_Id == f.FMG_Id && d.FTI_Id == b.FTI_Id && g.FMCC_Id == b.FMCC_Id && g.FYCC_Id == h.FYCC_Id && i.FMG_Id == b.FMG_Id && i.FMH_Id == b.FMH_Id && i.FTI_Id == b.FTI_Id && g.ASMAY_Id == b.ASMAY_Id && h.ASMCL_Id == data.ASMCL_ID && h.ASMCL_Id == a.ASMCL_Id)
                        //                                            select new FeeStudentTransactionDTO
                        //                                            {
                        //                                                FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid)
                        //                                            }
                        //).Sum(t => t.FSS_ToBePaid);
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
                                               from c in _FeeGroupHeadContext.FeeHeadDMO
                                               from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                               where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && termids.Contains(a.fmt_id) && ((i.FSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMST_Id == data.Amst_Id && HeadId.Contains(a.FMH_Id))
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FMA_Id = i.FMA_Id,
                                                   FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                                                   FMH_Flag = c.FMH_Flag
                                               }
                            ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        }
                        else
                        {
                            groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                                               from c in _FeeGroupHeadContext.FeeHeadDMO
                                               from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                                               where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && termids.Contains(a.fmt_id) && ((i.FSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMST_Id == data.Amst_Id && a.ASMCL_Id == data.ASMCL_ID && HeadId.Contains(a.FMH_Id))
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FMA_Id = i.FMA_Id,
                                                   FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                                                   FMH_Flag = c.FMH_Flag
                                               }
                            ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                        }

                        //var groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                        //                       from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                        //                       from c in _FeeGroupHeadContext.FeeHeadDMO
                        //                       from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                        //                       from f in _FeeGroupHeadContext.FeeGroupDMO
                        //                       from g in _FeeGroupHeadContext.feeYCC
                        //                       from h in _FeeGroupHeadContext.feeYCCC
                        //                       from i in _FeeGroupHeadContext.FeeStudentTransactionDMO
                        //                       where ( b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id) && ((b.FMA_Amount > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F"))  && i.AMST_Id==data.Amst_Id && a.fmg_id==b.FMG_Id && a.FMH_Id==b.FMH_Id && a.fti_id==b.FTI_Id && b.FMH_Id==c.FMH_Id && a.fti_id==d.FTI_Id && a.fmg_id==f.FMG_Id && b.FMCC_Id ==g.FMCC_Id && g.FYCC_Id==h.FYCC_Id && i.FMG_Id==b.FMG_Id &&i.FMH_Id==b.FMH_Id && i.FTI_Id==d.FTI_Id && b.ASMAY_Id==i.ASMAY_Id && g.ASMAY_Id==b.ASMAY_Id)
                        //                       select new FeeStudentTransactionDTO
                        //                       {
                        //                           FMA_Id = b.FMA_Id,
                        //                           FSS_ToBePaid = Convert.ToInt64(i.FSS_ToBePaid),
                        //                           FMH_Flag=c.FMH_Flag
                        //                       }
                        //     ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToArray();


                        Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                        onlinemtrans.FMOT_Trans_Id = data.trans_id;
                        //onlinemtrans.FMOT_Amount = data.topayamount;
                        onlinemtrans.FMOT_Amount = rece_amt.amt;
                        onlinemtrans.FMOT_Date = indianTime;
                        onlinemtrans.FMOT_Flag = "P";
                        onlinemtrans.PASR_Id = 0;
                        onlinemtrans.AMST_Id = data.Amst_Id;
                        onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;

                        onlinemtrans.MI_Id = data.MI_Id;
                        onlinemtrans.ASMAY_ID = data.ASMAY_Id;

                        _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                        for (int s = 0; s < groupwisefmaids.Count(); s++)
                        {
                            if (groupwisefmaids[s].FMH_Flag == "F")
                            {
                                fethchfmaidsfine = groupwisefmaids[s].FMA_Id;
                            }
                            else
                            {
                                Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                onlinettrans.FMA_Id = groupwisefmaids[s].FMA_Id;
                                onlinettrans.FTOT_Amount = groupwisefmaids[s].FSS_ToBePaid;
                                onlinettrans.FTOT_Created_date = indianTime;
                                onlinettrans.FTOT_Updated_date = indianTime;
                                onlinettrans.FTOT_Concession = 0;
                                onlinettrans.FTOT_Fine = 0;

                                using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd1.CommandText = "Sp_Calculate_Fine";
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@On_Date",
                                        SqlDbType.DateTime, 100)
                                    {
                                        Value = curdt
                                    });

                                    cmd1.Parameters.Add(new SqlParameter("@fma_id",
                                       SqlDbType.NVarChar, 100)
                                    {
                                        Value = groupwisefmaids[s].FMA_Id
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

                                    if (Convert.ToInt32(cmd1.Parameters["@amt"].Value) > 0)
                                    {
                                        fineamountcal = fineamountcal + Convert.ToInt32(cmd1.Parameters["@amt"].Value);
                                    }
                                }

                                _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                            }

                        }

                        for (int s = 0; s < groupwisefmaids.Count(); s++)
                        {
                            if (groupwisefmaids[s].FMH_Flag == "F")
                            {
                                if (finecount < 1)
                                {
                                    finecount = finecount + 1;
                                    fethchfmaidsfine = groupwisefmaids[s].FMA_Id;
                                    Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                                    onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                                    onlinettrans.FMA_Id = groupwisefmaids[s].FMA_Id;
                                    onlinettrans.FTOT_Amount = fineamountcal;
                                    onlinettrans.FTOT_Created_date = indianTime;
                                    onlinettrans.FTOT_Updated_date = indianTime;
                                    onlinettrans.FTOT_Concession = 0;
                                    onlinettrans.FTOT_Fine = 0;
                                    _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                                }
                            }
                        }

                        //commented on 19012018
                        onlinemtrans.FMOT_Amount = Convert.ToInt32(rece_amt.amt) + Convert.ToInt32(fineamountcal);



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
                //}

                //      else if (data.automanualreceiptno == "Auto")
                //      {
                //          GenerateTransactionNumbering ab = new GenerateTransactionNumbering(_context);
                //          data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                //          data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                //          data.FYP_Receipt_No = ab.GenerateNumber(data.transnumbconfigurationsettingsss);

                //          List<long> HeadId = new List<long>();
                //          List<long> termids = new List<long>();
                //          foreach (var item in data.temarray)
                //          {
                //              HeadId.Add(item.FMH_Id);
                //              termids.Add(item.FMT_Id);
                //          }

                //          foreach (var item in data.selected_list[0].trm_list)
                //          {
                //              termids.Add(item.FMT_Id);
                //          }

                //          List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                //          grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO

                //                  where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

                //                  select new FeeStudentTransactionDTO
                //                  {
                //                      FMG_Id = b.FMG_Id
                //                  }
                //                 ).Distinct().ToList();

                //          List<long> grpid = new List<long>();
                //          foreach (var item in grps)
                //          {
                //              grpid.Add(item.FMG_Id);
                //          }

                //          var groupwiseamount = (from a in _FeeGroupHeadContext.Fee_OnlinePayment_Mapping
                //                                 from b in _FeeGroupHeadContext.FeeAmountEntryDMO
                //                                 from c in _FeeGroupHeadContext.FeeHeadDMO
                //                                 from d in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                //                                 from f in _FeeGroupHeadContext.FeeGroupDMO
                //                                 from g in _FeeGroupHeadContext.feeYCC
                //                                 from h in _FeeGroupHeadContext.feeYCCC
                //                                 where (g.ASMAY_Id == b.ASMAY_Id && b.FMCC_Id == g.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMH_Id == a.FMH_Id && b.FTI_Id == a.fti_id && b.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && b.FMG_Id == f.FMG_Id && b.FMA_Amount > 0 && h.ASMCL_Id == Convert.ToInt32(data.ASMCL_ID) && termids.Contains(a.fmt_id))
                //                                 select new FeeStudentTransactionDTO
                //                                 {
                //                                     FSS_ToBePaid = Convert.ToInt64(b.FMA_Amount)
                //                                 }
                //).Sum(t => t.FSS_ToBePaid);

                //          Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                //          onlinemtrans.FMOT_Trans_Id = data.trans_id;
                //          onlinemtrans.FMOT_Amount = groupwiseamount;
                //          onlinemtrans.FMOT_Date = indianTime;
                //          onlinemtrans.FMOT_Flag = "P";
                //          onlinemtrans.PASR_Id = 0;
                //          onlinemtrans.AMST_Id = data.Amst_Id;
                //          onlinemtrans.FMOT_Receipt_no = data.FYP_Receipt_No;

                //          onlinemtrans.MI_Id = data.MI_Id;
                //          onlinemtrans.ASMAY_ID = data.ASMAY_Id;

                //          _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                //          for (int s = 0; s < data.temarray.Count(); s++)
                //          {
                //              Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                //              onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                //              onlinettrans.FMA_Id = data.temarray[s].FMA_Id;
                //              onlinettrans.FTOT_Amount = data.temarray[s].FSS_ToBePaid;
                //              onlinettrans.FTOT_Created_date = indianTime;
                //              onlinettrans.FTOT_Updated_date = indianTime;
                //              onlinettrans.FTOT_Concession = 0;
                //              onlinettrans.FTOT_Fine = 0;

                //              _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                //          }
                //      }

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
        //        if (data.auto_receipt_flag == 1)
        //        {
        //            List<long> HeadId = new List<long>();
        //            foreach (var item in data.temp_head_list)
        //            {
        //                HeadId.Add(item.FMH_Id);
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
        //                groupidss = groupidss + ',' + grpid[r];
        //            }

        //            var final_rept_no = "";
        //            List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
        //            List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

        //            list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
        //                        from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
        //                        where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

        //                        select new FeeStudentTransactionDTO
        //                        {
        //                            FGAR_PrefixName = b.FGAR_PrefixName,
        //                            FGAR_SuffixName = b.FGAR_SuffixName,
        //                            //FGAR_Name = b.FGAR_Name,
        //                            //FMG_Id = c.FMG_Id
        //                        }
        //                 ).Distinct().ToList();

        //            data.grp_count = list_all.Count();

        //            if (data.grp_count == 1)
        //            {
        //                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //                {
        //                    cmd.CommandText = "receiptnogeneration";
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.Add(new SqlParameter("@mi_id",
        //                        SqlDbType.VarChar, 100)
        //                    {
        //                        Value = data.MI_Id
        //                    });

        //                    cmd.Parameters.Add(new SqlParameter("@asmayid",
        //                       SqlDbType.NVarChar, 100)
        //                    {
        //                        Value = data.ASMAY_Id
        //                    });
        //                    cmd.Parameters.Add(new SqlParameter("@fmgid",
        //                   SqlDbType.NVarChar, 100)
        //                    {
        //                        Value = groupidss
        //                    });

        //                    cmd.Parameters.Add(new SqlParameter("@receiptno",
        //        SqlDbType.NVarChar, 500)
        //                    {
        //                        Direction = ParameterDirection.Output
        //                    });

        //                    if (cmd.Connection.State != ConnectionState.Open)
        //                        cmd.Connection.Open();

        //                    var data1 = cmd.ExecuteNonQuery();

        //                    data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

        //                }

        //                //data.auto_FYP_Receipt_No = final_rept_no;

        //                //data.FYP_Receipt_No = final_rept_no;
        //            }
        //        }

        //        else if (data.automanualreceiptno == "Auto")
        //        {
        //            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
        //            data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
        //            data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
        //            data.FYP_Receipt_No = a.GenerateNumber(data.transnumbconfigurationsettingsss);
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return data;
        //}

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
                    var confirmstatus = insertdatainfeetables(response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid.ToString(), response.udf1, indianTime);

                    if (Convert.ToInt32(confirmstatus) > 0)
                    {
                        pgmod.MI_Id = Convert.ToInt64(response.udf3);
                        pgmod.amst_mobile = response.phone;
                        pgmod.Amst_Id = Convert.ToInt64(response.udf2);
                        pgmod.amst_email_id = response.email;

                        SMS sms = new SMS(_context);

                        //  sms.sendSms(Convert.ToInt32(response.udf3), Convert.ToInt64(response.phone), "FEEONLINEPAYMENT", Convert.ToInt32(response.udf2));

                        Email Email = new Email(_context);

                        // Email.sendmail(Convert.ToInt32(response.udf3), response.email, "FEEONLINEPAYMENT", Convert.ToInt32(response.udf2));

                    }
                }

            }
            else
            {
                SMS sms = new SMS(_context);

                // sms.sendSms(Convert.ToInt32(response.udf3), Convert.ToInt64(response.phone), "FEEONLINEPAYMENTFAIL", Convert.ToInt32(response.udf2));

                Email Email = new Email(_context);

                //  Email.sendmail(Convert.ToInt32(response.udf3), response.email, "FEEONLINEPAYMENTFAIL", Convert.ToInt32(response.udf2));

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
                    //           data.fillinstallment = (from a in _FeeGroupHeadContext.FeeGroupDMO
                    //                                   from b in _FeeGroupHeadContext.FeeStudentGroupMappingDMO
                    //                                   from c in _FeeGroupHeadContext.FEeGroupLoginPreviledgeDMO
                    //                                   from d in _FeeGroupHeadContext.FeeHeadDMO
                    //                                   where (c.FMH_Id == d.FMH_Id && d.FMH_Flag == "T" && a.FMG_Id == b.FMG_Id && b.AMST_Id == data.Amst_Id && a.FMG_ActiceFlag == true && c.FMG_ID == b.FMG_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                    //                                   select new FeeStudentTransactionDTO
                    //                                   {
                    //                                       FMG_GroupName = a.FMG_GroupName,
                    //                                       FMG_Id = a.FMG_Id,
                    //                                   }
                    //).Distinct().ToArray();


                    //     data.transportopted = (from a in _FeeGroupHeadContext.FeeStudentTransactionDMO
                    //                            from b in _FeeGroupHeadContext.FeeHeadDMO
                    //                            from c in _FeeGroupHeadContext.FeeInstallmentsyearlyDMO
                    //                            where (a.FMH_Id==b.FMH_Id && a.FTI_Id==c.FTI_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "T")
                    //                            select new FeeStudentTransactionDTO
                    //                            {
                    //                                FMG_GroupName = c.FTI_Name,
                    //                                FMG_Id = a.FMG_Id,
                    //                            }
                    //).Distinct().OrderBy(t => t.FMH_Id).ToArray();

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
                                        ASYST_Id = b.ASYST_Id
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
                   // data = paymentPartbilldesk(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "EBS")
                {
                    //data.paydet = paymentPartebs(data, data.topayamount);
                }
                else if (data.onlinepaygteway == "PAYTM")
                {
                    //data.paydet = paymentPartccavenue(data, data.topayamount);
                }
                //else
                //{
                //    data.paydet = paymentPart(data, data.topayamount);
                //}

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

        public string insertdatainfeetables(string miid, string termid, string studentid, string classid, decimal amount, string transid, string refid, string yearid, DateTime indianTime)
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
                                        FMA_Amount = a.FMOT_Amount
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
                                                   FSS_ToBePaid = Convert.ToInt64(a.FTOT_Amount)
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

                            _FeeGroupHeadContext.FeeTransactionPaymentDMO.Add(onlinettrans);

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
                                obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - groupwisefmaids[s].FSS_ToBePaid;
                            }
                            else
                            {
                                obj_status_stf.FSS_ToBePaid = 0;
                            }

                            _FeeGroupHeadContext.Update(obj_status_stf);

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

            return contactexisttransaction.ToString();
        }

        public long getacademicyearcongig(FeeStudentTransactionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                var currasmayid = (from a in _FeeGroupHeadContext.AcademicYear
                                   where (a.MI_Id == data.MI_Id && indianTime > a.ASMAY_From_Date && indianTime < a.ASMAY_To_Date)
                                   select new FeeStudentTransactionDTO
                                   {
                                       ASMAY_Id = a.ASMAY_Id,
                                   }
                                ).FirstOrDefault();

                data.ASMAY_Id = Convert.ToInt32(currasmayid.ASMAY_Id);

                var readterms = (from a in _FeeGroupHeadContext.feemastersettings
                                 where (a.MI_Id == data.MI_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMC_Online_Payment_Aca_Yr_Flag = a.FMC_Online_Payment_Aca_Yr_Flag,
                                 }
                                 ).FirstOrDefault();

                if (readterms.FMC_Online_Payment_Aca_Yr_Flag == "C")
                {
                    data.ASMAY_Id = data.ASMAY_Id;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data.ASMCL_ID;
        }
    }
}
