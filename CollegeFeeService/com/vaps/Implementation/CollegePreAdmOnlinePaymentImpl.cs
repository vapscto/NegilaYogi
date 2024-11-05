using CollegeFeeService.com.vaps.Interfaces;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model.com.vapstech.Fee;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using PreadmissionDTOs.com.vaps.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using System.Linq;
using paytm.security;
using paytm.util;
using paytm.exception;
using Razorpay.Api;
using Payment = CommonLibrary.Payment;

using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using PreadmissionDTOs;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegePreAdmOnlinePaymentImpl:Interfaces.CollegePreAdmOnlinePaymentInterface
    {
        private static ConcurrentDictionary<string, CollegeFeeTransactionDTO> _login =
new ConcurrentDictionary<string, CollegeFeeTransactionDTO>();

        public DomainModelMsSqlServerContext _context;
        public  CollFeeGroupContext _FeeGroupHeadContext;
   

        public CollegePreAdmOnlinePaymentImpl(CollFeeGroupContext frgContext,  DomainModelMsSqlServerContext context)
        {
           
            _FeeGroupHeadContext = frgContext;
            _context = context;
        }

        public CollegeFeeTransactionDTO getamountdetails(CollegeFeeTransactionDTO data)
        {
            try
            {

                List<feeheadDTO> fineheadfmaid = new List<feeheadDTO>();
                List<feeheadDTO> fineheadfmaidall = new List<feeheadDTO>();
                List<feeheadDTO> fineheadfmaidsaved = new List<feeheadDTO>();

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

                var fetchclass = (from a in _FeeGroupHeadContext.PA_College_Application
                                  where (a.MI_Id == data.MI_Id && a.PACA_Id == data.PACA_Id)
                                  select new CollegeFeeTransactionDTO
                                  {
                                      AMCO_Id = a.AMCO_Id,
                                      ASMAY_Id = a.ASMAY_Id,
                                      AMB_Id=a.AMB_Id,
                                      AMSE_Id=a.AMSE_Id
                                     // AMSE_Id = a.AMSE_Id
                                      //pasl_id=a.PASL_ID
                                  }
  ).Distinct().ToArray();

                string courseid = "0", academicyearid = "0", branchid = "0", semid="0";
                for (int s = 0; s < fetchclass.Count(); s++)
                {
                    courseid = fetchclass[s].AMCO_Id.ToString();
                    //semid = fetchclass[s].pasl_id.ToString();
                    branchid = fetchclass[s].AMB_Id.ToString();
                    semid = fetchclass[s].AMSE_Id.ToString();
                }

                data.fillpaymentgateway = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                           from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id && b.FPGD_PGActiveFlag == "1")
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }
).Distinct().ToArray();

                List<CollegeFeeTransactionDTO> finelst = new List<CollegeFeeTransactionDTO>();
                List<CollegeFeeTransactionDTO> finelstfinal = new List<CollegeFeeTransactionDTO>();

                List<CollegeFeeTransactionDTO> amount_list = new List<CollegeFeeTransactionDTO>();
                List<CollegeFeeTransactionDTO> gropidssss = new List<CollegeFeeTransactionDTO>();

                List<long> groupids = new List<long>();

                for (int l = 0; l < data.grouplist.Length; l++)
                {
                    trm_ids.Clear();
                    foreach (var x in data.grouplist[l].trm_list)
                    {
                        trm_ids.Add(x.FMT_Id);
                        trm_idsforspecialfee.Add(x.FMT_Id);
                    }

                    groupids.Clear();
                    if (grpset.Equals(0))
                    {
                      
                        gropidssss = (from a in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                      from b in _FeeGroupHeadContext.FeeGroupClgDMO
                                      where (b.MI_Id == data.MI_Id && a.FMGG_Id == data.grouplist[l].grp.FMGG_Id && a.FMG_Id == b.FMG_Id )
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                      }
  ).Distinct().ToList();
                        //added on 03122018
                    }
                    else
                    {
                        List<CollegeFeeTransactionDTO> gropidsssstemp = new List<CollegeFeeTransactionDTO>();
                        List<long> customfmgids = new List<long>();
                        gropidsssstemp = (from a in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                          from b in _FeeGroupHeadContext.FeeGroupClgDMO
                                          where (b.MI_Id == data.MI_Id && a.FMGG_Id == data.grouplist[l].grp.FMGG_Id && a.FMG_Id == b.FMG_Id)
                                          select new CollegeFeeTransactionDTO
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

                        //groupids = _FeeGroupHeadContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id==data.ASMAY_Id && t.PASL_ID==Convert.ToInt64(semid) && t.AMCO_Id== Convert.ToInt64(courseid) && customfmgids.Contains(t.FMG_Id)).Select(t => t.FMG_Id).ToList();

                        //groupids = _FeeGroupHeadContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PASL_ID == Convert.ToInt64(semid) && t.AMCO_Id == Convert.ToInt64(courseid) && customfmgids.Contains(t.FMG_Id)).Select(t => t.FMG_Id).ToList();
                    }

                    //added on 03122018
                    foreach (var x in gropidssss)
                    {
                        groupids.Add(x.FMG_Id);
                    }
                    //added on 03122018



                    var readterms = (from a in _FeeGroupHeadContext.GenConfiguration
                                     where (a.MI_Id == data.MI_Id)
                                     select new CollegeFeeTransactionDTO
                                     {
                                         IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                     }
                          ).FirstOrDefault();
                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                         from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                         from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                         from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                         from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                     
                                         where (g.FCMA_Id==b.FCMA_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && groupids.Contains(a.fmg_id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0  && a.AMCO_Id == Convert.ToInt32(courseid) && g.ASMAY_Id == g.ASMAY_Id)
                                         select new feeheadDTO
                                         {
                                             FCMAS_Id = b.FCMAS_Id,
                                             FMH_FeeName = c.FMH_FeeName
                                         }
               ).Distinct().ToList();

                        fineheadfmaidsaved = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                              from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                              from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                              from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                              from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                              from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                       
                                              from i in _FeeGroupHeadContext.Fee_T_College_PaymentDMO
                                              from j in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                              from k in _FeeGroupHeadContext.Fee_Y_Payment_PA_Application
                                              where (g.FCMA_Id==b.FCMA_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && groupids.Contains(a.fmg_id) 
                                              && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 &&  a.AMCO_Id == Convert.ToInt32(courseid) && i.FCMAS_Id == b.FCMAS_Id
                                              && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PACA_Id == data.PACA_Id)
                                              select new feeheadDTO
                                              {
                                                  FCMAS_Id = b.FCMAS_Id,
                                                  FMH_FeeName = c.FMH_FeeName
                                              }
               ).Distinct().ToList();

                    }
                    else
                    {
                        fineheadfmaid = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                         from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                         from e in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                         from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                         from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                         from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                        
                                         where ( e.FCMA_Id==b.FCMA_Id && e.FMH_Id == a.FMH_Id && e.FTI_Id == a.fti_id && e.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && groupids.Contains(a.fmg_id) && e.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0  && b.AMSE_Id == Convert.ToInt32(semid)  && a.AMCO_Id==Convert.ToInt32(courseid))
                                         select new feeheadDTO
                                         {
                                             FCMAS_Id = b.FCMAS_Id,
                                             FMH_FeeName = c.FMH_FeeName
                                         }
             ).Distinct().ToList();

                        fineheadfmaidsaved = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                              from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                              from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                              from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                              from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                              from e in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                          
                                              from i in _FeeGroupHeadContext.Fee_T_College_PaymentDMO
                                              from j in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                              from k in _FeeGroupHeadContext.Fee_Y_Payment_PA_Application
                                              where (e.FCMA_Id == b.FCMA_Id && e.FMH_Id == a.FMH_Id && e.FTI_Id == a.fti_id && e.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && groupids.Contains(a.fmg_id) && e.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0  && b.AMSE_Id == Convert.ToInt32(semid) && i.FCMAS_Id == b.FCMAS_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PACA_Id == data.PACA_Id  && a.AMCO_Id == Convert.ToInt32(courseid))
                                              select new feeheadDTO
                                              {
                                                  FCMAS_Id = b.FCMAS_Id,
                                                  FMH_FeeName = c.FMH_FeeName
                                              }
               ).Distinct().ToList();
                    }





                    if (fineheadfmaidsaved.Count() > 0)
                    {
                        foreach (var sav in fineheadfmaidsaved)
                        {
                            savedfma.Add(sav.FCMAS_Id);
                        }
                    }

                    if (fineheadfmaid.Count() > 0)
                    {
                        foreach (var q in fineheadfmaid)
                        {
                            fineheadfmaidall.Add(q);
                        }
                    }

                    List<CollegeFeeTransactionDTO> list = new List<CollegeFeeTransactionDTO>();
                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        list = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                from e in _FeeGroupHeadContext.FeeGroupClgDMO
                                from f in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                //from g in _FeeGroupHeadContext.feeYCC
                                //from h in _FeeGroupHeadContext.feeYCCC
                                where ( f.FMH_Id == a.FMH_Id && f.FTI_Id == a.fti_id && f.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id  && b.FCMAS_Amount > 0 && groupids.Contains(a.fmg_id) && b.FCMA_Id==f.FCMA_Id && a.AMCO_Id == Convert.ToInt32(courseid))
                                select new CollegeFeeTransactionDTO
                                {
                                    FCMAS_Id = b.FCMAS_Id,
                                    FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount),
                                    FCSS_FineAmount = 0,
                                    FCSS_ConcessionAmount = 0,
                                    FCSS_WaivedAmount = 0,
                                    FMG_Id = f.FMG_Id,
                                    FMH_Id = f.FMH_Id,
                                    FTI_Id = f.FTI_Id,
                                    FCSS_PaidAmount = 0,
                                    FCSS_NetAmount = 0,
                                    FCSS_RefundAmount = 0,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FCSS_CurrentYrCharges = Convert.ToInt64(b.FCMAS_Amount),
                                   FCSS_TotalToBePaid = Convert.ToInt64(b.FCMAS_Amount),
                                }
             ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                    }
                    else
                    {
                        list = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                from e in _FeeGroupHeadContext.FeeGroupClgDMO
                                from f in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                where ( f.FMH_Id == a.FMH_Id && f.FTI_Id == a.fti_id && f.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id  && b.FCMAS_Amount > 0 && groupids.Contains(a.fmg_id)  && a.AMCO_Id == Convert.ToInt32(courseid) &&  a.AMCO_Id==Convert.ToInt32(courseid))
                                select new CollegeFeeTransactionDTO
                                {
                                    FCMAS_Id = b.FCMAS_Id,
                                    FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount),
                                    FCSS_FineAmount = 0,
                                    FCSS_ConcessionAmount = 0,
                                    FCSS_WaivedAmount = 0,
                                    FMG_Id =f.FMG_Id,
                                    FMH_Id = f.FMH_Id,
                                    FTI_Id = f.FTI_Id,
                                    FCSS_PaidAmount = 0,
                                    FCSS_NetAmount = 0,
                                    FCSS_RefundAmount = 0,
                                    FMH_FeeName = c.FMH_FeeName,
                                    FTI_Name = d.FTI_Name,
                                    FCSS_CurrentYrCharges = Convert.ToInt64(b.FCMAS_Amount),
                                     FCSS_TotalToBePaid = Convert.ToInt64(b.FCMAS_Amount),
                                }
             ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();
                    }


                    DateTime currdt = DateTime.Now;
                    foreach (var x in list)
                    {
                        amount_list.Add(x);

               //         using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
               //         {
               //             cmd.CommandText = "Sp_Calculate_Fine_CLG";
               //             cmd.CommandType = CommandType.StoredProcedure;
               //             cmd.Parameters.Add(new SqlParameter("@On_Date",
               //                 SqlDbType.DateTime, 100)
               //             {
               //                 Value = currdt
               //             });

               //             cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
               //                SqlDbType.NVarChar, 100)
               //             {
               //                 Value = x.FCMAS_Id
               //             });
               //             cmd.Parameters.Add(new SqlParameter("@asmay_id",
               //            SqlDbType.NVarChar, 100)
               //             {
               //                 Value = data.ASMAY_Id
               //             });

               //             cmd.Parameters.Add(new SqlParameter("@amt",
               // SqlDbType.Decimal, 500)
               //             {
               //                 Direction = ParameterDirection.Output
               //             });

               //             cmd.Parameters.Add(new SqlParameter("@flgArr",
               //SqlDbType.Int, 500)
               //             {
               //                 Direction = ParameterDirection.Output
               //             });

               //             if (cmd.Connection.State != ConnectionState.Open)
               //                 cmd.Connection.Open();

               //             var data1 = cmd.ExecuteNonQuery();

               //             if (Convert.ToInt32(cmd.Parameters["@amt"].Value) > 0)
               //             {
               //                 fineamt = fineamt + Convert.ToInt32(cmd.Parameters["@amt"].Value);
               //                 totalfine = totalfine + fineamt;
               //             }

                            
               //         }
                    }


                    if (Convert.ToInt32(fineamt) > 0)
                    {
                        if (fineheadfmaid.Count() > 0)
                        {
                            finelst.Add(new CollegeFeeTransactionDTO
                            {
                                FCMAS_Id = fineheadfmaid[0].FCMAS_Id,
                                FCSS_ToBePaid = Convert.ToInt32(fineamt),
                                FCSS_FineAmount = 0,
                                FCSS_ConcessionAmount = 0,
                                FCSS_WaivedAmount = 0,
                                FMG_Id = 0,
                                FMH_Id = 0,
                                FTI_Id = 0,
                                FCSS_PaidAmount = 0,
                                FCSS_NetAmount = 0,
                                FCSS_RefundAmount = 0,
                                FMH_FeeName = fineheadfmaid[0].FMH_FeeName,
                                FTI_Name = "Anytime",
                                FCSS_CurrentYrCharges = 0,
                               // FCSS_TotalToBePaid = 0,
                            });
                        }
                    }
                    trm_ids.Clear();
                }

                data.fillstudentviewdetails = amount_list.ToArray();


                data.instalspecial = (from a in _FeeGroupHeadContext.FeeHeadClgDMO
                                      from d in _FeeGroupHeadContext.Clg_Fee_Installment_DMO
                                      from b in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                      from c in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                      from e in _FeeGroupHeadContext.FeeGroupClgDMO
                                      from f in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                      //from f in _FeeGroupHeadContext.feeYCCC
                                      //from g in _FeeGroupHeadContext.feeYCC
                                      where (f.FCMA_Id==c.FCMA_Id && a.FMH_Id == f.FMH_Id && e.FMG_Id == f.FMG_Id && f.FTI_Id == b.FTI_Id  && f.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && f.AMCO_Id == Convert.ToInt16(courseid) && c.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id  && !savedfma.Contains(c.FCMAS_Id) && ((c.FCMAS_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                      select new PreadmissionDTOs.com.vaps.College.Fees.Head_Installments_DTO
                                      {
                                          FTI_Name = b.FTI_Name,
                                          FTI_Id = b.FTI_Id
                                      }).Distinct().ToList().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public CollegeFeeTransactionDTO getdetails(CollegeFeeTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _FeeGroupHeadContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                data.ASMAY_Id = Acdemic_preadmission;

                var AdmissionStatus = _FeeGroupHeadContext.AdmissionStatus.FirstOrDefault(d => d.MI_Id == data.MI_Id && d.PAMST_StatusFlag.Contains("CNF")).PAMST_Id;

              
                var masnum = _FeeGroupHeadContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Online").ToList();
                data.transnumconfig = masnum.ToArray();

               
                var allRegStudent2 = _FeeGroupHeadContext.PA_College_Application.Where(t => t.ID == data.UserId && t.MI_Id == data.MI_Id && t.PACA_AdmStatus != AdmissionStatus && t.PACA_FinalpaymentFlag == false).ToList();
                data.registrationList = allRegStudent2.ToArray();
                if (allRegStudent2.Count() > 0)
                {
                    data.dismessage = "Your Seat is not yet confirmed to make Payment";
                }

              
                var allRegStudent1 = _FeeGroupHeadContext.PA_College_Application.Where(t => t.ID == data.UserId && t.MI_Id == data.MI_Id && t.PACA_AdmStatus == AdmissionStatus && (t.PACA_FinalpaymentFlag == true || t.PACA_ActiveFlag == true)).ToList();
                data.registrationList = allRegStudent1.ToArray();
                if (allRegStudent1.Count() > 0)
                {
                    var displaymessage = _FeeGroupHeadContext.smsEmailSetting.Where(t => t.MI_Id == data.MI_Id && t.ISES_Template_Name == "PreadmissionParPayDisMgs").ToList();
                    if (displaymessage.Count > 0)
                    {
                        data.dismessage = displaymessage[0].ISES_SMSMessage;
                    }
                    else
                    {
                        data.dismessage = "Your Payment is Successful!!!";
                    }
                }

            
             var allRegStudent = _FeeGroupHeadContext.PA_College_Application.Where(t => t.ID == data.UserId && t.PACA_AdmStatus == AdmissionStatus && t.PACA_FinalpaymentFlag == false).ToList();
                data.registrationList = allRegStudent.ToArray();

                data.specialheaddetails = (from a in _FeeGroupHeadContext.feespecialHead
                                           from b in _FeeGroupHeadContext.feeSGGG
                                           from c in _FeeGroupHeadContext.FeeHeadClgDMO
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

            
                var instidet = _FeeGroupHeadContext.MOBILE_INSTITUTION.Where(t => t.MI_ID == data.MI_Id).ToList();
                data.institutiondet = instidet.ToArray();

               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeFeeTransactionDTO getstudentdetails(CollegeFeeTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _FeeGroupHeadContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                var fetchclass = (from a in _FeeGroupHeadContext.PA_College_Application
                                  where (a.MI_Id == data.MI_Id && a.PACA_Id == data.PACA_Id)
                                  select new CollegeFeeTransactionDTO
                                  {
                                      AMCO_Id = a.AMCO_Id,
                                      ASMAY_Id = a.ASMAY_Id,
                                      //pasl_id=a.PASL_ID
                                      AMB_Id = a.AMB_Id,
                                      AMSE_Id=a.AMSE_Id
                                  }
 ).Distinct().ToArray();

                string courseid = "0", academicyearid = "0",branchid="0",semid="0";
                for (int s = 0; s < fetchclass.Count(); s++)
                {
                    courseid = fetchclass[s].AMCO_Id.ToString();
                    branchid = fetchclass[s].AMB_Id.ToString();
                    semid = fetchclass[s].AMSE_Id.ToString();
                }

                var readterms = (from a in _FeeGroupHeadContext.GenConfiguration
                                 where (a.MI_Id == data.MI_Id)
                                 select new CollegeFeeTransactionDTO
                                 {
                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                 }
                        ).FirstOrDefault();

                data.partialorfullpayment = (from a in _FeeGroupHeadContext.MasterConfiguration
                                             where (a.MI_Id == data.MI_Id)
                                             select new CollegeFeeTransactionDTO
                                             {
                                                 ISPAC_FullPaymentCompFlg = a.ISPAC_FullPaymentCompFlg
                                             }
                    ).ToArray();

                data.fillpaymentgateway = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                           from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                           where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == data.MI_Id && b.FPGD_PGActiveFlag == "1")
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FPGD_Id = a.IMPG_Id,
                                               FPGD_PGName = a.IMPG_PGFlag,
                                               FPGD_Image = b.FPGD_Image
                                           }
).Distinct().ToArray();


                decimal fineheadfmaidsaved = 0;
                if (readterms.IVRMGC_Classwise_Payment != "1")
                {
                    fineheadfmaidsaved = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                          from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                          from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                          from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                          from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                          from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                
                                          from i in _FeeGroupHeadContext.Fee_T_College_PaymentDMO
                                          from j in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                          from k in _FeeGroupHeadContext.Fee_Y_Payment_PA_Application
                                          where (g.FCMA_Id==b.FCMA_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (c.FMH_Flag == "N") && g.AMCO_Id == Convert.ToInt32(courseid) && g.AMB_Id == Convert.ToInt32(branchid) && i.FCMAS_Id == b.FCMAS_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PACA_Id == data.PACA_Id)
                                          select new CollegeFeeTransactionDTO
                                          {
                                              FCMAS_Amount = b.FCMAS_Amount,
                                              FTI_Id = g.FTI_Id
                                          }
             ).Sum(t => t.FCMAS_Amount);
                }
                else
                {
                    fineheadfmaidsaved = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                          from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                          from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                          from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                          from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                        
                                          from i in _FeeGroupHeadContext.Fee_T_College_PaymentDMO
                                          from j in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                          from k in _FeeGroupHeadContext.Fee_Y_Payment_PA_Application
                                          from l in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                          where (b.FCMA_Id==l.FCMA_Id && l.FMH_Id == a.FMH_Id && l.FTI_Id == a.fti_id && l.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && l.ASMAY_Id == data.ASMAY_Id && l.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(courseid) && b.AMSE_Id==Convert.ToInt32(semid) && i.FCMAS_Id == b.FCMAS_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PACA_Id == data.PACA_Id )
                                          select new CollegeFeeTransactionDTO
                                          {
                                              FCMAS_Amount = b.FCMAS_Amount,
                                              FTI_Id = l.FTI_Id
                                          }
             ).Sum(t => t.FCMAS_Amount);
                }


                data.fineheadfmaidsaved = Convert.ToInt64(fineheadfmaidsaved);

                //List<long> savedfma = new List<long>();
                //if (fineheadfmaidsaved.Count() > 0)
                //{
                //    foreach (var sav in fineheadfmaidsaved)
                //    {
                //        savedfma.Add(sav.FCMAS_Id);
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
                        data.filonlinepaymentgrid = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                                     from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                     from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                                     from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                                     from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                                    
                                                     from i in _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping
                                                     from j in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                                     where (b.FCMA_Id==j.FCMA_Id && j.FMH_Id == a.FMH_Id && j.FTI_Id == a.fti_id && j.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.AMSE_Id == Convert.ToInt32(semid) &&  j.ASMAY_Id == data.ASMAY_Id && j.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "G" ) && a.AMCO_Id == Convert.ToInt32(courseid) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id )
                                                     select new CollegeFeeTransactionDTO
                                                     {
                                                         FCSS_NetAmount= Convert.ToInt64(b.FCMAS_Amount),
                                                         FCSS_ConcessionAmount = 0,
                                                         FCSS_FineAmount = 0,
                                                         FCSS_ToBePaid = 0,
                                                         FCSS_PaidAmount = 0,
                                                      //   FCSS_TotalToBePaid = Convert.ToInt64(b.FCMAS_Amount),
                                                         FCMAS_Id = b.FCMAS_Id,
                                                     }
           ).Distinct().ToArray();
                    }

                    else
                    {
                        data.filonlinepaymentgrid = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                                     from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                     from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                                     from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                                     from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                       
                                                     from i in _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping
                                                     from j in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                                     where (j.FCMA_Id==b.FCMA_Id && j.FMH_Id == a.FMH_Id && j.FTI_Id == a.fti_id && j.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && b.AMSE_Id == Convert.ToInt32(semid) && j.ASMAY_Id == data.ASMAY_Id && j.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(courseid) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id && j.ASMAY_Id == i.ASMAY_Id)
                                                     select new CollegeFeeTransactionDTO
                                                     {
                                                         FCSS_NetAmount = Convert.ToInt64(b.FCMAS_Amount),
                                                         FCSS_ConcessionAmount = 0,
                                                         FCSS_FineAmount = 0,
                                                         FCSS_ToBePaid = 0,
                                                         FCSS_PaidAmount = 0,
                                                      //  FCSS_TotalToBePaid = Convert.ToInt64(b.FCMAS_Amount),
                                                         FCMAS_Id = b.FCMAS_Id,
                                                     }
           ).Distinct().ToArray();
                    }

                }
                else
                {
                    //var groupids = _FeeGroupHeadContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PASL_ID == Convert.ToInt64(semid) && t.AMCO_Id == Convert.ToInt64(courseid)).Select(t => t.FMG_Id).ToList();

                 //   var groupids = _FeeGroupHeadContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PASL_ID == Convert.ToInt64(semid) && t.AMCO_Id == Convert.ToInt64(courseid)).Select(t => t.FMG_Id).ToList();

                    if (readterms.IVRMGC_Classwise_Payment != "1")
                    {
                        data.filonlinepaymentgrid = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                                     from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                     from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                                     from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                                     from f in _FeeGroupHeadContext.FeeGroupClgDMO

                                                     from i in _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping
                                                     from j in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                                     where (j.FCMA_Id == b.FCMA_Id && j.FMH_Id == a.FMH_Id && j.FTI_Id == a.fti_id && j.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && j.ASMAY_Id == data.ASMAY_Id && b.AMSE_Id == Convert.ToInt32(semid) && j.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(courseid) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id && j.ASMAY_Id == i.ASMAY_Id)
                                                     select new CollegeFeeTransactionDTO
                                                     {
                                                         FCSS_NetAmount = Convert.ToInt64(b.FCMAS_Amount),
                                                         FCSS_ConcessionAmount = 0,
                                                         FCSS_FineAmount = 0,
                                                         FCSS_ToBePaid = 0,
                                                         FCSS_PaidAmount = 0,
                                                      //   FCSS_TotalToBePaid = Convert.ToInt64(b.FCMAS_Amount),
                                                         FCMAS_Id = b.FCMAS_Id,
                                                     }
             ).Distinct().ToArray();
                    }
                    else
                    {
                        data.filonlinepaymentgrid = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                                     from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                     from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                                     from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                                     from f in _FeeGroupHeadContext.FeeGroupClgDMO

                                                     from i in _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping
                                                     from j in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                                     where (j.FCMA_Id == b.FCMA_Id && j.FMH_Id == a.FMH_Id && j.FTI_Id == a.fti_id && j.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && j.ASMAY_Id == data.ASMAY_Id && b.AMSE_Id == Convert.ToInt32(semid) && j.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(courseid) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id && j.ASMAY_Id == i.ASMAY_Id)
                                                     select new CollegeFeeTransactionDTO
                                                     {
                                                         FCSS_NetAmount = Convert.ToInt64(b.FCMAS_Amount),
                                                         FCSS_ConcessionAmount = 0,
                                                         FCSS_FineAmount = 0,
                                                         FCSS_ToBePaid = 0,
                                                         FCSS_PaidAmount = 0,
                                                        // FCSS_TotalToBePaid = Convert.ToInt64(b.FCMAS_Amount),
                                                         FCMAS_Id = b.FCMAS_Id,
                                                     }
             ).Distinct().ToArray();
                    }

                }

                data.customgrplist = (from a in _FeeGroupHeadContext.FeeGroupMappingDMO
                                      from b in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                      from c in _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping
                                      from d in _FeeGroupHeadContext.FeeHeadClgDMO
                                      from e in _FeeGroupHeadContext.FeeGroupClgDMO
                                      where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.FMG_Id && c.FMH_Id == d.FMH_Id && c.FMG_Id == e.FMG_Id && b.FMG_Id == e.FMG_Id && (d.FMH_Flag == "G") && c.ASMAY_Id == data.ASMAY_Id  )
                                      select new tempgroupDTO
                                      {
                                        FMGG_GroupName = a.FMGG_GroupName,
                                          FMGG_Id = a.FMGG_Id
                                      }
        ).Distinct().OrderBy(t => t.FMGG_Id).ToArray();


                var customgrplistt = (from a in _FeeGroupHeadContext.FeeGroupMappingDMO
                                      from b in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                      from c in _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping
                                      from d in _FeeGroupHeadContext.FeeHeadClgDMO
                                      from e in _FeeGroupHeadContext.FeeGroupClgDMO
                                      where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.FMG_Id && c.FMH_Id == d.FMH_Id && c.FMG_Id == e.FMG_Id && b.FMG_Id == e.FMG_Id && ( d.FMH_Flag == "N") && c.ASMAY_Id == data.ASMAY_Id)
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FMGG_GroupName =a.FMGG_GroupName,
                                         FMGG_Id= a.FMGG_Id,
                                          FMG_Id = e.FMG_Id
                                      }
     ).Distinct().OrderBy(t => t.FMGG_Id).ToArray();

                List<long> cusgroupids = new List<long>();
                List<long> fmgidss = new List<long>();
                foreach (var x in customgrplistt)
                {
                   cusgroupids.Add(x.FMGG_Id);
                    fmgidss.Add(x.FMG_Id);
                }



                var getblockedata = (from a in _FeeGroupHeadContext.FeeGroupMappingDMO
                                     from b in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                     from c in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                     from d in _FeeGroupHeadContext.feeTr
                                     where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id  && c.MI_Id == data.MI_Id  && d.FMT_ActiveFlag == true && c.PreAdmFlag == true && cusgroupids.Contains(b.FMGG_Id)) /*&& c.AMCO_Id == Convert.ToInt32(courseid)*/
                                     select new CollegeFeeTransactionDTO
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
                    data.termlst = (from a in _FeeGroupHeadContext.FeeGroupMappingDMO
                                    from b in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                    from c in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                    from d in _FeeGroupHeadContext.feeTr
                                    where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id  && c.MI_Id == data.MI_Id  && d.FMT_ActiveFlag == true && head_idss.Contains(c.FMH_Id) && cusgroupids.Contains(b.FMGG_Id) && fmgidss.Contains(c.fmg_id)) /*&& c.AMCO_Id == Convert.ToInt32(courseid)*/
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
                    data.termlst = (from a in _FeeGroupHeadContext.FeeGroupMappingDMO
                                    from b in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                    from c in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                    from d in _FeeGroupHeadContext.feeTr
                                    where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.fmg_id && c.MI_Id == data.MI_Id  && d.FMT_ActiveFlag == true && c.AMCO_Id == Convert.ToInt32(courseid) && head_idss.Contains(c.FMH_Id))
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



                data.fillstudent = (from a in _FeeGroupHeadContext.PA_College_Application
                                    from b in _FeeGroupHeadContext.MasterCourseDMO
                                    from c in _FeeGroupHeadContext.ClgMasterBranchDMO
                                    from d in _FeeGroupHeadContext.CLG_Adm_Master_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.PACA_Id == data.PACA_Id && a.AMCO_Id == b.AMCO_Id && a.AMSE_Id==d.AMSE_Id && a.AMB_Id==c.AMB_Id)
                                    select new CollegeFeeTransactionDTO
                                    {
                                        PACA_FirstName = a.PACA_FirstName,
                                        PACA_MiddleName = a.PACA_MiddleName,
                                        PACA_LastName = a.PACA_LastName,
                                        PACA_RegistrationNo = a.PACA_RegistrationNo,
                                        PACA_Id = a.PACA_Id,
                                        AMCO_CourseName = b.AMCO_CourseName,
                                        PACA_MobileNo = a.PACA_MobileNo,
                                        PACA_emailId = a.PACA_emailId,
                                        AMCO_Id = a.AMCO_Id,
                                        AMSE_SEMName=d.AMSE_SEMName,
                                        AMB_BranchName=c.AMB_BranchName,
                                        PACA_StudentPhoto=a.PACA_StudentPhoto,
                                    }
                                       ).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeFeeTransactionDTO hashgeneration(CollegeFeeTransactionDTO data)
        {
            try
            {
                data.fillstudent = (from a in _FeeGroupHeadContext.PA_College_Application
                                    from b in _FeeGroupHeadContext.MasterCourseDMO
                                    from c in _FeeGroupHeadContext.ClgMasterBranchDMO
                                    from d in _FeeGroupHeadContext.CLG_Adm_Master_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.PACA_Id == data.PACA_Id && a.AMCO_Id == b.AMCO_Id)
                                    select new CollegeFeeTransactionDTO
                                    {
                                        PACA_FirstName = a.PACA_FirstName,
                                        PACA_MiddleName = a.PACA_MiddleName,
                                        PACA_LastName = a.PACA_LastName,
                                        PACA_RegistrationNo = a.PACA_RegistrationNo,
                                        PACA_Id = a.PACA_Id,
                                        AMCO_CourseName = b.AMCO_CourseName,
                                        PACA_MobileNo = a.PACA_MobileNo,
                                        PACA_emailId = a.PACA_emailId,
                                        AMCO_Id = a.AMCO_Id,

                                        //AMCST_PerCity = a.PACA_PerCity
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Array paymentPart(CollegeFeeTransactionDTO enq, long totamount)
        {
            Payment pay = new Payment(_context);
            //ProspectusDTO data = new ProspectusDTO();
            int autoinc = 1, totpayableamount = 0;

            var fetchclass = (from a in _FeeGroupHeadContext.PA_College_Application
                              where (a.MI_Id == enq.MI_Id && a.PACA_Id == enq.PACA_Id)
                              select new CollegeFeeTransactionDTO
                              {
                                  AMCO_Id = a.AMCO_Id,
                                  ASMAY_Id = a.ASMAY_Id,
                                  //pasl_id=a.PASL_ID
                                  AMSE_Id = a.AMSE_Id
                              }
   ).Distinct().ToArray();

            string courseid = "0", academicyearid = "0";
            for (int s = 0; s < fetchclass.Count(); s++)
            {
                enq.AMCO_Id = Convert.ToInt64(fetchclass[s].AMCO_Id);
                enq.ASMAY_Id = Convert.ToInt64(fetchclass[s].ASMAY_Id);
                //enq.pasl_id = Convert.ToInt64(fetchclass[s].pasl_id);
                enq.AMSE_Id = Convert.ToInt64(fetchclass[s].AMSE_Id);
            }

            string multigrp = "0";
            for (int l = 0; l < enq.grouplist.Length; l++)
            {
                List<long> trm_ids = new List<long>();
                foreach (var x in enq.grouplist[l].trm_list)
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
                        Value = enq.PACA_Id
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


          //  List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
           var paymentdetails = _FeeGroupHeadContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();

            var receipt = _FeeGroupHeadContext.Fee_Y_PaymentDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_ReceiptNo);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
           // PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY.Trim();
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
            PaymentDetailsDto.amount = Convert.ToDecimal(totamount.ToString());

            PaymentDetailsDto.firstname = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_FirstName + ' ' + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_MiddleName + ' ' + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_LastName;


            PaymentDetailsDto.email = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_emailId;

           // PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT.Trim();
          //  PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
            PaymentDetailsDto.phone = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_MobileNo;
            PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
            PaymentDetailsDto.udf2 = Convert.ToString(enq.PACA_Id);
            PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
            PaymentDetailsDto.udf4 = enq.multiplegroups.ToString();
            PaymentDetailsDto.udf5 = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCO_Id.ToString();
            PaymentDetailsDto.udf6 = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMSE_Id.ToString();
         //   PaymentDetailsDto.udf7 = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMB_Id.ToString();
            PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/CollegePreAdmOnlinePayment/paymentresponse/";
            PaymentDetailsDto.status = "success";
            PaymentDetailsDto.service_provider = "payu_paisa";

            PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

            var fetchuserid = (from a in _FeeGroupHeadContext.applicationUser
                               from b in _FeeGroupHeadContext.ApplicationUserRole
                               from c in _FeeGroupHeadContext.IVRM_Role_Type
                               where (a.Id == b.UserId && b.RoleTypeId == c.IVRMRT_Id && c.IVRMRT_RoleFlag == "ADMIN")
                               select new CollegeFeeTransactionDTO
                               {
                                   UserId = Convert.ToInt64(a.Id),
                               }
       ).Distinct().ToArray().Take(1);

            string useridss = "0";
            for (int s = 0; s < fetchuserid.Count(); s++)
            {
                useridss = fetchuserid.FirstOrDefault().UserId.ToString();
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

        public string insertdatainfeetables(string miid, string studentid, string courseid, decimal amount, string transid, string refid, string yearid)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            var contactexisttransaction = 0;
            try
            {
                string recnoen = "";
                var fetchfmhotid = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                    where (a.PASR_Id == Convert.ToInt64(studentid) && a.FMOT_Trans_Id == transid.ToString() && a.FMOT_Amount > 0)
                                    select new CollegeFeeTransactionDTO
                                    {
                                        FMHOT_Id = a.FMOT_Id,
                                        FCMAS_Amount = a.FMOT_Amount,
                                        FYP_PayModeType = a.FYP_PayModeType,
                                        FMOT_PayGatewayType = a.FMOT_PayGatewayType

                                    }).ToArray();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    var fethchfmgids = (from a in _FeeGroupHeadContext.Fee_T_Online_TransactionDMO
                                        from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                        from c in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                        from d in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                        where (d.FCMA_Id==b.FCMA_Id && c.FMH_Id == d.FMH_Id && c.fti_id == d.FTI_Id && c.fmg_id == d.FMG_Id && a.FMA_Id == b.FCMAS_Id && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(miid) && d.ASMAY_Id == Convert.ToInt64(yearid))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            FMG_Id = d.FMG_Id
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

                    List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();
                    List<CollegeFeeTransactionDTO> list_repts = new List<CollegeFeeTransactionDTO>();
                    list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                                from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid) && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new CollegeFeeTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    FGAR_Id = c.FGAR_Id,
                                }
                         ).Distinct().ToList();

                    using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "receiptnogeneration_CLG";
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
                        var Euserid = (from a in _FeeGroupHeadContext.FeeGroupClgDMO
                                       where (a.MI_Id == Convert.ToInt64(miid) && grpid.Contains(a.FMG_Id))
                                       select new CollegeFeeTransactionDTO
                                       {
                                           enduserid = a.user_id,
                                       }
                           ).Distinct().Take(1).ToArray();
                        //added on 12-10-2019

                        var groupwisefmaids = (from a in _FeeGroupHeadContext.Fee_T_Online_TransactionDMO
                                               from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                               from c in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                               from d in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                               where (b.FCMA_Id==d.FCMA_Id && a.FMOT_Id == c.FMOT_Id && a.FMA_Id == b.FCMAS_Id && b.MI_Id == Convert.ToInt64(miid) && d.ASMAY_Id == Convert.ToInt64(yearid) && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id)
                                               select new CollegeFeeTransactionDTO
                                               {
                                                   FCMAS_Id = b.FCMAS_Id,
                                                   FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
                                               }
                             ).ToArray();

                        Fee_Y_PaymentDMO onlinemtrans = new Fee_Y_PaymentDMO();

                        onlinemtrans.ASMAY_Id = Convert.ToInt64(yearid);
                      //  onlinemtrans.FTCU_Id = 1;
                        onlinemtrans.FYP_ReceiptNo = recnoen;
    
                        onlinemtrans.FYP_PayModeType = "Single";
                        onlinemtrans.FYP_TransactionTypeFlag = "O";
                        onlinemtrans.FYP_DOE = DateTime.Now;
                        onlinemtrans.FYP_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FCMAS_Amount);
                        onlinemtrans.FYP_ReceiptDate = DateTime.Now;
                        onlinemtrans.FYP_TotalFineAmount = 0;
                       
                        onlinemtrans.FYP_Remarks = "Preadmission Full Payment";
                        onlinemtrans.FYP_PayModeType = fetchfmhotid[r].FYP_PayModeType;
                        onlinemtrans.FYP_PayGatewayType = fetchfmhotid[r].FMOT_PayGatewayType;

                        // onlinemtrans.IVRMSTAUL_ID = Convert.ToInt64(studentid);

                        onlinemtrans.FYP_ChequeBounceFlag = "CL";
                        onlinemtrans.MI_Id = Convert.ToInt64(miid);
                       
                        onlinemtrans.CreatedDate = DateTime.Now;
                        onlinemtrans.UpdatedDate = DateTime.Now;
                        onlinemtrans.User_Id = Convert.ToInt64(Euserid[0].enduserid);
                        onlinemtrans.FYP_Transaction_Id = transid;

                        onlinemtrans.FYP_ChallanStatusFlag= "Sucessfull";
                        onlinemtrans.FYP_PaymentReference_Id = refid.ToString();
                        onlinemtrans.FYP_ChallanNo = "";

                        _FeeGroupHeadContext.Fee_Y_PaymentDMO.Add(onlinemtrans);

                        //multimode of payment
                        Fee_Y_Payment_PaymentModeDMO onlinemulti = new Fee_Y_Payment_PaymentModeDMO();
                        onlinemulti.FYP_Id = onlinemtrans.FYP_Id;
                        onlinemulti.FYPPM_TransactionTypeFlag = "O";
                        onlinemulti.FYPPM_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FCMAS_Amount);
                        onlinemulti.FYPPM_LedgerId = 0;
                        onlinemulti.FYPPM_BankName = "";
                        onlinemulti.FYPPM_DDChequeNo = "";
                        onlinemulti.FYPPM_DDChequeDate = indianTime;
                        onlinemulti.FYPPM_Transaction_Id = transid;
                        onlinemulti.FYPPM_PaymentReference_Id = refid.ToString();
                        onlinemulti.FYPPM_ClearanceStatusFlag = "0";
                        onlinemulti.FYPPM_ClearanceDate = indianTime;
                        _FeeGroupHeadContext.Add(onlinemulti);
                        //multimode of payment

                        Fee_Y_Payment_PA_Application onlinePA_College_Application = new Fee_Y_Payment_PA_Application();

                        onlinePA_College_Application.FYP_Id = onlinemtrans.FYP_Id;
                        onlinePA_College_Application.PACA_Id = Convert.ToInt64(studentid);
                        onlinePA_College_Application.FYPPA_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FCMAS_Amount);
                        onlinePA_College_Application.FYPPA_ActiveFlag = 1;
                        onlinePA_College_Application.FYPPA_Type = "A";

                        _FeeGroupHeadContext.Fee_Y_Payment_PA_Application.Add(onlinePA_College_Application);

                        for (int s = 0; s < groupwisefmaids.Count(); s++)
                        {
                            Fee_T_College_PaymentDMO onlinettranst = new Fee_T_College_PaymentDMO();
                            onlinettranst.FYP_Id = onlinemtrans.FYP_Id;
                            onlinettranst.FCMAS_Id = groupwisefmaids[s].FCMAS_Id;
                            onlinettranst.FTCP_PaidAmount = groupwisefmaids[s].FCSS_ToBePaid;
                            onlinettranst.FTCP_FineAmount = 0;
                            onlinettranst.FTCP_ConcessionAmount = 0;
                            onlinettranst.FTCP_WaivedAmount = 0;
                            onlinettranst.FTCP_Remarks = "Preadmission Full Payment";
                            onlinettranst.FTCP_UpdatedDate = DateTime.Now;
                            _FeeGroupHeadContext.Fee_T_College_PaymentDMO.Add(onlinettranst);

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
                    var resultupda = _FeeGroupHeadContext.PA_College_Application.Where(t => t.PACA_Id == Convert.ToInt64(studentid)).ToArray();

                    resultupda[0].PACA_FinalpaymentFlag = true;
                    resultupda[0].UpdatedDate = DateTime.Now;
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
                    var feepartialpaymentflagamst = _FeeGroupHeadContext.PA_College_Application.Where(t => t.PACA_Id == Convert.ToInt32(studentid)).Select(d => d.AMCO_Id).FirstOrDefault();

                    var termidssss = _FeeGroupHeadContext.feeTr.Where(t => t.MI_Id == Convert.ToInt32(miid) && t.FMT_Order == 1).Select(d => d.FMT_Id).FirstOrDefault();

                    var netamount = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                     from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                     from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                     from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                     from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                     //from g in _FeeGroupHeadContext.feeYCC
                                     //from h in _FeeGroupHeadContext.feeYCCC
                                     from i in _FeeGroupHeadContext.FeeYearGroupDMO
                                     from j in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                     where ( b.FCMA_Id==j.FCMA_Id && j.FMH_Id == a.FMH_Id && j.FTI_Id == a.fti_id && j.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == Convert.ToInt64(miid) &&j.ASMAY_Id == Convert.ToInt64(yearid) && j.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && ( c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(feepartialpaymentflagamst) && f.FMG_Id == i.FMG_Id && i.FMG_Id == a.fmg_id && j.ASMAY_Id == i.ASMAY_Id )
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FCSS_NetAmount = Convert.ToInt64(b.FCMAS_Amount),
                                     }
    ).Sum(t => t.FCSS_NetAmount);



                    var paidamount = (from a in _FeeGroupHeadContext.Fee_T_College_PaymentDMO
                                      from b in _FeeGroupHeadContext.Fee_Y_Payment_PA_Application
                                      where (a.FYP_Id == b.FYP_Id && b.PACA_Id == Convert.ToInt64(studentid))
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FCSS_PaidAmount = Convert.ToInt64(a.FTCP_PaidAmount),
                                      }
   ).Sum(t => t.FCSS_PaidAmount);

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
                        var resultupda = _FeeGroupHeadContext.PA_College_Application.Where(t => t.PACA_Id == Convert.ToInt64(studentid)).ToArray();

                        resultupda[0].PACA_FinalpaymentFlag = true;
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
            CollegeFeeTransactionDTO pgmod = new CollegeFeeTransactionDTO();
            PaymentDetails dto = new PaymentDetails();

            var emailmobile = (from a in _FeeGroupHeadContext.PA_College_Application
                               where (a.MI_Id == Convert.ToInt32(response.udf3) && a.ASMAY_Id == Convert.ToInt32(response.udf1) && a.PACA_Id == Convert.ToInt64(response.udf2))
                               select new CollegeFeeTransactionDTO
                               {
                                   PACA_MobileNo = a.PACA_MobileNo,
                                   PACA_emailId = a.PACA_emailId

                               }
                                ).ToArray();

            if (emailmobile.Length > 0)
            {
                for (int i = 0; i < emailmobile.Length; i++)
                {
                    pgmod.AMCST_emailId = emailmobile[i].PACA_emailId;
                    pgmod.AMCST_MobileNo = emailmobile[i].PACA_MobileNo;
                }
            }

            var confirmstatusadmission = 0;
            if (response.status == "success")
            {
                string txnid = response.txnid.ToString();

                //get_grp_reptno(pgmod);

                //var confirmstatus = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Insert_fee_tables_Online_Full_payment_Preadmission @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7", response.udf3, response.udf4, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid, response.udf1);


                var confirmstatus = insertdatainfeetables(response.udf3, response.udf2, response.udf5, response.amount, response.txnid, response.mihpayid.ToString(), response.udf1);

                CollegeFeeTransactionDTO data = new CollegeFeeTransactionDTO();
                var transfersett = (from a in _FeeGroupHeadContext.MasterConfiguration
                                    where (a.MI_Id == Convert.ToInt32(response.udf3) && a.ASMAY_Id == Convert.ToInt32(response.udf1))
                                    select new CollegeFeeTransactionDTO
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
                    confirmstatusadmission = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission_college @p0,@p1,@p2", response.udf2, response.udf1, response.udf3);
                }

                if (Convert.ToInt64(confirmstatus) > 0 || confirmstatusadmission > 0)
                {
                    pgmod.MI_Id = Convert.ToInt64(response.udf3);
                    // pgmod.AMCST_MobileNo = response.phone;
                    pgmod.AMCST_Id = Convert.ToInt64(response.udf2);
                    //  pgmod.AMCST_emailId = response.email;

                    Email Email = new Email(_context);

                    Email.sendmail(pgmod.MI_Id, pgmod.AMCST_emailId, "PREADMISSIONFEEONLINEPAYMENT", pgmod.AMCST_Id);

                    SMS sms = new SMS(_context);

                    sms.sendSms(pgmod.MI_Id, pgmod.AMCST_MobileNo, "PREADMISSIONFEEONLINEPAYMENT", pgmod.AMCST_Id);



                    //createusername(pgmod.Amst_Id,);
                }
            }
            else
            {
                pgmod.MI_Id = Convert.ToInt64(response.udf3);
                // pgmod.AMCST_MobileNo = response.phone;
                pgmod.AMCST_Id = Convert.ToInt64(response.udf2);
                //  pgmod.AMCST_emailId = response.email;

                Email Email = new Email(_context);

                Email.sendmail(pgmod.MI_Id, pgmod.AMCST_emailId, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.AMCST_Id);

                SMS sms = new SMS(_context);

                sms.sendSms(pgmod.MI_Id, pgmod.AMCST_MobileNo, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.AMCST_Id);

            }

            return response;
        }

        //public CollegeFeeTransactionDTO get_grp_reptno(CollegeFeeTransactionDTO data)
        //{
        //    try
        //    {
        //        var dynamicrecgen = new List<dynamic>();

        //        string onlineheadmapid = "0", groupidss = "0";

        //        List<string> mulreceiptno = new List<string>();
        //        List<string> fmgidrec = new List<string>();

        //        if (data.auto_receipt_flag == 1 || data.auto_receipt_flag == 0)
        //        {
        //            List<long> HeadId = new List<long>();
        //            List<long> termids = new List<long>();
        //            foreach (var item in data.temarray)
        //            {
        //                HeadId.Add(item.FMH_Id);
        //                termids.Add(item.FMT_Id);
        //            }

        //            foreach (var item in data.grouplist[0].trm_list)
        //            {
        //                termids.Add(item.FMT_Id);
        //            }

        //            List<CollegeFeeTransactionDTO> grps = new List<CollegeFeeTransactionDTO>();
        //            //grps = (from b in _FeeGroupHeadContext.FeeYearlygroupHeadMappingDMO
        //            //        from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
        //            //        where (b.FMG_Id==c.FMG_Id &&  b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))
        //            //        select new CollegeFeeTransactionDTO
        //            //        {
        //            //            FGAR_Id = c.FGAR_Id
        //            //        }
        //            //       ).Distinct().ToList();

        //            grps = (from b in _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping
        //                    from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
        //                    from d in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
        //                    where (b.FMG_Id == c.FMG_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id) && c.FGAR_Id == d.FGAR_Id && b.ASMAY_Id == d.ASMAY_Id)
        //                    select new CollegeFeeTransactionDTO
        //                    {
        //                        FGAR_Id = c.FGAR_Id
        //                    }
        //                  ).Distinct().ToList();


        //            for (int r = 0; r < grps.Count(); r++)
        //            {
        //                onlineheadmapid = grps[r].FGAR_Id.ToString();

        //                List<CollegeFeeTransactionDTO> grps1 = new List<CollegeFeeTransactionDTO>();
        //                grps1 = (from a in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
        //                         from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
        //                         where (b.FGAR_Id == a.FGAR_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FGAR_Id == Convert.ToInt64(onlineheadmapid))
        //                         select new CollegeFeeTransactionDTO
        //                         {
        //                             FMG_Id = a.FMG_Id
        //                         }
        //                 ).Distinct().ToList();

        //                List<long> grpid = new List<long>();
        //                foreach (var item in grps1)
        //                {
        //                    grpid.Add(item.FMG_Id);
        //                }

        //                for (int d = 0; d < grps1.Count(); d++)
        //                {
        //                    groupidss = groupidss + ',' + grps1[d].FMG_Id;
        //                }

        //                List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();
        //                List<CollegeFeeTransactionDTO> list_repts = new List<CollegeFeeTransactionDTO>();
        //                list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
        //                            from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
        //                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

        //                            select new CollegeFeeTransactionDTO
        //                            {
        //                                FGAR_PrefixName = b.FGAR_PrefixName,
        //                                FGAR_SuffixName = b.FGAR_SuffixName,
        //                                FGAR_Id = c.FGAR_Id,
        //                                //FGAR_Name = b.FGAR_Name,
        //                                //FGAR_Id = c.FGAR_Id
        //                            }
        //                     ).Distinct().ToList();

        //                List<feeheadDTO> fineheadfmaidsaved = new List<feeheadDTO>();
        //                var readterms = (from a in _FeeGroupHeadContext.GenConfiguration
        //                                 where (a.MI_Id == data.MI_Id)
        //                                 select new CollegeFeeTransactionDTO
        //                                 {
        //                                     IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
        //                                 }
        //                ).FirstOrDefault();
        //                if (readterms.IVRMGC_Classwise_Payment != "1")
        //                {
        //                    fineheadfmaidsaved = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
        //                                          from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
        //                                          from c in _FeeGroupHeadContext.FeeHeadClgDMO
        //                                          from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
        //                                          from f in _FeeGroupHeadContext.FeeGroupClgDMO
        //                                          //from g in _FeeGroupHeadContext.feeYCC
        //                                          //from h in _FeeGroupHeadContext.feeYCCC
        //                                          from i in _FeeGroupHeadContext.Fee_T_College_PaymentDMO
        //                                          from j in _FeeGroupHeadContext.FeeGroupGroupingDMO
        //                                          from k in _FeeGroupHeadContext.Fee_Y_Payment_PA_Application
        //                                          from l in _FeeGroupHeadContext.PAYUDETAILS
        //                                          from m in _FeeGroupHeadContext.Fee_PaymentGateway_Details
        //                                           from n in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
        //                                          where ( n.FCMA_Id==b.FCMA_Id && n.FMH_Id == a.FMH_Id && n.FTI_Id == a.fti_id && n.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && l.IMPG_Id == m.IMPG_Id && m.FPGD_Id == a.fpgd_id && l.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && grpid.Contains(a.fmg_id) && n.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N")  && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id) && i.FCMAS_Id == b.FCMAS_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PACA_Id == data.PACA_Id &&  l.IMPG_ActiveFlg == true)
        //                                          select new feeheadDTO
        //                                          {
        //                                              FCMAS_Id = b.FCMAS_Id,
        //                                              FMH_FeeName = c.FMH_FeeName
        //                                          }
        //   ).Distinct().ToList();
        //                }
        //                else
        //                {
        //                    fineheadfmaidsaved = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
        //                                          from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
        //                                          from c in _FeeGroupHeadContext.FeeHeadClgDMO
        //                                          from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
        //                                          from f in _FeeGroupHeadContext.FeeGroupClgDMO

        //                                          from i in _FeeGroupHeadContext.Fee_T_College_PaymentDMO
        //                                          from j in _FeeGroupHeadContext.FeeGroupGroupingDMO
        //                                          from k in _FeeGroupHeadContext.Fee_Y_Payment_PA_Application
        //                                          from l in _FeeGroupHeadContext.PAYUDETAILS
        //                                          from m in _FeeGroupHeadContext.Fee_PaymentGateway_Details
        //                                          from n in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
        //                                          where (b.FCMA_Id==n.FCMA_Id && n.FMH_Id == a.FMH_Id && n.FTI_Id == a.fti_id && n.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && l.IMPG_Id == m.IMPG_Id && m.FPGD_Id == a.fpgd_id && l.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && grpid.Contains(a.fmg_id) && n.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N")  && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id) && i.FCMAS_Id == b.FCMAS_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PACA_Id == data.PACA_Id  && a.AMCO_Id == a.AMCO_Id && l.IMPG_ActiveFlg == true)
        //                                          select new feeheadDTO
        //                                          {
        //                                              FCMAS_Id = b.FCMAS_Id,
        //                                              FMH_FeeName = c.FMH_FeeName
        //                                          }
        //   ).Distinct().ToList();
        //                }


        //                List<long> savedfma = new List<long>();
        //                if (fineheadfmaidsaved.Count() > 0)
        //                {
        //                    foreach (var sav in fineheadfmaidsaved)
        //                    {
        //                        savedfma.Add(sav.FCMAS_Id);
        //                    }
        //                }

        //                long groupwiseamount = 0;
        //                if (readterms.IVRMGC_Classwise_Payment != "1")
        //                {
        //                    groupwiseamount = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
        //                                       from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
        //                                       from c in _FeeGroupHeadContext.FeeHeadClgDMO
        //                                       from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
        //                                       from f in _FeeGroupHeadContext.FeeGroupClgDMO
        //                                       //from g in _FeeGroupHeadContext.feeYCC
        //                                       //from h in _FeeGroupHeadContext.feeYCCC
        //                                       from i in _FeeGroupHeadContext.PAYUDETAILS
        //                                       from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
        //                                       from k in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
        //                                       where (b.FCMA_Id==k.FCMA_Id &&  k.FMH_Id == a.FMH_Id && k.FTI_Id == a.fti_id && k.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && k.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id)  && !savedfma.Contains(b.FCMAS_Id) &&  i.IMPG_ActiveFlg == true)
        //                                       select new CollegeFeeTransactionDTO
        //                                       {
        //                                           FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
        //                                       }
        //).Sum(t => t.FCSS_ToBePaid);
        //                }
        //                else
        //                {
        //                    groupwiseamount = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
        //                                       from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
        //                                       from c in _FeeGroupHeadContext.FeeHeadClgDMO
        //                                       from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
        //                                       from f in _FeeGroupHeadContext.FeeGroupClgDMO

        //                                       from i in _FeeGroupHeadContext.PAYUDETAILS
        //                                       from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
        //                                       from k in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
        //                                       where ( k.FCMA_Id==b.FCMA_Id &&  k.FMH_Id == a.FMH_Id && k.FTI_Id == a.fti_id && k.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && k.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id)  && !savedfma.Contains(b.FCMAS_Id)  && a.AMCO_Id == a.AMCO_Id && i.IMPG_ActiveFlg == true)
        //                                       select new CollegeFeeTransactionDTO
        //                                       {
        //                                           FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
        //                                       }
        //).Sum(t => t.FCSS_ToBePaid);
        //                }


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

        //                    data.FYP_ReceiptNo = cmd.Parameters["@receiptno"].Value.ToString();

        //                    var rece_amt = new { receiptno = data.FYP_ReceiptNo, amt = groupwiseamount };

        //                    dynamicrecgen.Add(rece_amt);

        //                    List<CollegeFeeTransactionDTO> groupwisefmaids = new List<CollegeFeeTransactionDTO>();
        //                    if (readterms.IVRMGC_Classwise_Payment != "1")
        //                    {
        //                        groupwisefmaids = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
        //                                           from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
        //                                           from c in _FeeGroupHeadContext.FeeHeadClgDMO
        //                                           from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
        //                                           from f in _FeeGroupHeadContext.FeeGroupClgDMO

        //                                           from i in _FeeGroupHeadContext.PAYUDETAILS
        //                                           from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
        //                                           from k in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
        //                                           where (b.FCMA_Id==k.FCMA_Id  && k.FMH_Id == a.FMH_Id && k.FTI_Id == a.fti_id && k.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && k.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id)  && !savedfma.Contains(b.FCMAS_Id)  && i.IMPG_ActiveFlg == true)
        //                                           select new CollegeFeeTransactionDTO
        //                                           {
        //                                               FCMAS_Id = b.FCMAS_Id,
        //                                               FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
        //                                           }
        //                       ).ToList();
        //                    }
        //                    else
        //                    {
        //                        groupwisefmaids = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
        //                                           from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
        //                                           from c in _FeeGroupHeadContext.FeeHeadClgDMO
        //                                           from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
        //                                           from f in _FeeGroupHeadContext.FeeGroupClgDMO

        //                                           from i in _FeeGroupHeadContext.PAYUDETAILS
        //                                           from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
        //                                           from k in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
        //                                           where (b.FCMA_Id==k.FCMA_Id  && k.FMH_Id == a.FMH_Id && k.FTI_Id == a.fti_id && k.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && k.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && k.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id)  && !savedfma.Contains(b.FCMAS_Id) && a.AMCO_Id == a.AMCO_Id && i.IMPG_ActiveFlg == true)
        //                                           select new CollegeFeeTransactionDTO
        //                                           {
        //                                               FCMAS_Id = b.FCMAS_Id,
        //                                               FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
        //                                           }
        //                     ).ToList();

        //                    }


        //                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

        //                    onlinemtrans.FMOT_Trans_Id = data.trans_id;
        //                    onlinemtrans.FMOT_Amount = data.topayamount;
        //                    onlinemtrans.FMOT_Date = DateTime.Now;
        //                    onlinemtrans.FMOT_Flag = "P";
        //                    onlinemtrans.AMST_Id = 0;
        //                    onlinemtrans.PASR_Id = data.PACA_Id;
        //                    onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;

        //                    onlinemtrans.MI_Id = data.MI_Id;
        //                    onlinemtrans.ASMAY_ID = data.ASMAY_Id;
        //                    onlinemtrans.FYP_PayModeType = "APP";
        //                    onlinemtrans.FMOT_PayGatewayType = data.onlinepaygteway;


        //                    _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

        //                    for (int s = 0; s < data.selected_list.Count(); s++)
        //                    {
        //                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
        //                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
        //                        onlinettrans.FMA_Id = data.selected_list[s].FCMAS_Id;
        //                        onlinettrans.FTOT_Amount = data.selected_list[s].FCSS_ToBePaid;
        //                        onlinettrans.FTOT_Created_date = DateTime.Now;
        //                        onlinettrans.FTOT_Updated_date = DateTime.Now;
        //                        onlinettrans.FTOT_Concession = 0;
        //                        onlinettrans.FTOT_Fine = 0;

        //                        _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
        //                    }

        //                    groupidss = "0";
        //                }
        //            }
        //        }
        //        else if (data.automanualreceiptno == "Auto")
        //        {
        //            GenerateTransactionNumbering ab = new GenerateTransactionNumbering(_context);
        //            data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
        //            data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
        //            data.FYP_ReceiptNo = ab.GenerateNumber(data.transnumbconfigurationsettingsss);

        //            List<long> HeadId = new List<long>();
        //            List<long> termids = new List<long>();
        //            foreach (var item in data.temarray)
        //            {
        //                HeadId.Add(item.FMH_Id);
        //                termids.Add(item.FMT_Id);
        //            }

        //            foreach (var item in data.grouplist[0].trm_list)
        //            {
        //                termids.Add(item.FMT_Id);
        //            }

        //            List<CollegeFeeTransactionDTO> grps = new List<CollegeFeeTransactionDTO>();
        //            grps = (from b in _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping

        //                    where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

        //                    select new CollegeFeeTransactionDTO
        //                    {
        //                        FMG_Id = b.FMG_Id
        //                    }
        //                   ).Distinct().ToList();

        //            List<long> grpid = new List<long>();
        //            foreach (var item in grps)
        //            {
        //                grpid.Add(item.FMG_Id);
        //            }

        //            long groupwiseamount = 0;
        //            List<CollegeFeeTransactionDTO> groupwisefmaids = new List<CollegeFeeTransactionDTO>();
        //            var readterms = (from a in _FeeGroupHeadContext.GenConfiguration
        //                             where (a.MI_Id == data.MI_Id)
        //                             select new CollegeFeeTransactionDTO
        //                             {
        //                                 IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
        //                             }
        //               ).FirstOrDefault();
        //            if (readterms.IVRMGC_Classwise_Payment != "1")
        //            {
        //                groupwiseamount = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
        //                                   from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
        //                                   from c in _FeeGroupHeadContext.FeeHeadClgDMO
        //                                   from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
        //                                   from f in _FeeGroupHeadContext.FeeGroupClgDMO
        //                                   from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
        //                                   //from g in _FeeGroupHeadContext.feeYCC
        //                                   //from h in _FeeGroupHeadContext.feeYCCC
        //                                   where (g.FCMA_Id==g.FCMA_Id &&  g.FMH_Id == a.FMH_Id && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id))
        //                                   select new CollegeFeeTransactionDTO
        //                                   {
        //                                       FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
        //                                   }
        // ).Sum(t => t.FCSS_ToBePaid);
        //                groupwisefmaids = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
        //                                   from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
        //                                   from c in _FeeGroupHeadContext.FeeHeadClgDMO
        //                                   from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
        //                                   from f in _FeeGroupHeadContext.FeeGroupClgDMO
        //                                   from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO

        //                                   where (g.FCMA_Id==b.FCMA_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id))
        //                                   select new CollegeFeeTransactionDTO
        //                                   {
        //                                       FCMAS_Id = b.FCMAS_Id,
        //                                       FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
        //                                   }
        //                   ).ToList();
        //            }
        //            else
        //            {
        //                groupwiseamount = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
        //                                   from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
        //                                   from c in _FeeGroupHeadContext.FeeHeadClgDMO
        //                                   from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
        //                                   from f in _FeeGroupHeadContext.FeeGroupClgDMO
        //                                from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
        //                                   where (g.FCMA_Id==b.FCMA_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id))
        //                                   select new CollegeFeeTransactionDTO
        //                                   {
        //                                       FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
        //                                   }
        //).Sum(t => t.FCSS_ToBePaid);
        //                groupwisefmaids = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
        //                                   from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
        //                                   from c in _FeeGroupHeadContext.FeeHeadClgDMO
        //                                   from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
        //                                   from f in _FeeGroupHeadContext.FeeGroupClgDMO
        //                                   from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
        //                                   where (g.FCMA_Id == b.FCMA_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id))
        //                                   select new CollegeFeeTransactionDTO
        //                                   {
        //                                       FCMAS_Id = b.FCMAS_Id,
        //                                       FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
        //                                   }
        //                    ).ToList();
        //            }





        //            Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

        //            onlinemtrans.FMOT_Trans_Id = data.trans_id;
        //            onlinemtrans.FMOT_Amount = data.topayamount;
        //            onlinemtrans.FMOT_Date = DateTime.Now;
        //            onlinemtrans.FMOT_Flag = "P";
        //            onlinemtrans.AMST_Id = 0;
        //            onlinemtrans.PASR_Id = data.PACA_Id;
        //            onlinemtrans.FMOT_Receipt_no = data.FYP_ReceiptNo;

        //            _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

        //            for (int s = 0; s < data.selected_list.Count(); s++)
        //            {
        //                Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
        //                onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
        //                onlinettrans.FMA_Id = data.selected_list[s].FCMAS_Id;
        //                onlinettrans.FTOT_Amount = data.selected_list[s].FCSS_ToBePaid;
        //                onlinettrans.FTOT_Created_date = DateTime.Now;
        //                onlinettrans.FTOT_Updated_date = DateTime.Now;
        //                onlinettrans.FTOT_Concession = 0;
        //                onlinettrans.FTOT_Fine = 0;

        //                _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
        //            }

        //        }

        //        var contactexisttransaction = 0;

        //        using (var dbCtxTxn = _FeeGroupHeadContext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                contactexisttransaction = _FeeGroupHeadContext.SaveChanges();
        //                dbCtxTxn.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //                dbCtxTxn.Rollback();
        //            }
        //        }

        //        if (dynamicrecgen.Count() > 0)
        //        {
        //            data.recenocol = dynamicrecgen.ToArray();
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //    return data;
        //}


        public CollegeFeeTransactionDTO get_grp_reptno(CollegeFeeTransactionDTO data)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                string onlineheadmapid = "0", groupidss = "0";

                long fineamountcal = 0, fethchfmaidsfine = 0, finecount = 0;

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();

                List<long> HeadId = new List<long>();
                List<long> grpid = new List<long>();

                foreach (var item in data.selected_list)
                {
                    HeadId.Add(item.FMH_Id);
                    grpid.Add(item.FMG_Id);
                    groupidss = groupidss + ',' + item.FMG_Id;
                }

                List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();


                list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                            from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                            select new CollegeFeeTransactionDTO
                            {
                                FGAR_PrefixName = b.FGAR_PrefixName,
                                FGAR_SuffixName = b.FGAR_SuffixName,
                                FGAR_Id = c.FGAR_Id,
                            }
                     ).Distinct().ToList();



                decimal groupwiseamount = 0;

                groupwiseamount = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                   from b in _FeeGroupHeadContext.Fee_College_Student_StatusDMO
                                   from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                   where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == data.onlinepaygteway && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCST_Id == data.AMCST_Id && b.FCSS_ToBePaid > 0 && grpid.Contains(a.fmg_id) && HeadId.Contains(b.FMH_Id))
                                   select new CollegeFeeTransactionDTO
                                   {
                                       FCSS_ToBePaid = b.FCSS_ToBePaid,
                                   }
        ).Sum(t => t.FCSS_ToBePaid);

                using (var cmd = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "receiptnogeneration_CLG";
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

                    List<CollegeFeeTransactionDTO> groupwisefmaids = new List<CollegeFeeTransactionDTO>();

                    groupwisefmaids = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                       from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                       from i in _FeeGroupHeadContext.Fee_College_Student_StatusDMO
                                       from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                       where (a.fmg_id == i.FMG_Id && a.FMH_Id == i.FMH_Id && a.fti_id == i.FTI_Id && i.FMH_Id == c.FMH_Id && a.FMH_Id == c.FMH_Id && i.MI_Id == data.MI_Id && i.ASMAY_Id == data.ASMAY_Id && grpid.Contains(i.FMG_Id) && grpid.Contains(a.fmg_id) && ((i.FCSS_ToBePaid > 0 && (c.FMH_Flag != "F")) || (c.FMH_Flag == "F")) && i.AMCST_Id == data.AMCST_Id && b.FPGD_Id == a.fpgd_id && b.FPGD_PGName == data.onlinepaygteway && HeadId.Contains(i.FMH_Id))
                                       select new CollegeFeeTransactionDTO
                                       {
                                           FCMAS_Id = i.FCMAS_Id,
                                           FCSS_ToBePaid = Convert.ToInt64(i.FCSS_ToBePaid),
                                           FMH_Flag = c.FMH_Flag
                                       }
                    ).Distinct().OrderBy(t => t.FMH_Id).ThenBy(t => t.FTI_Id).ToList();

                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = data.trans_id;
                    onlinemtrans.FMOT_Amount = data.topayamount;
                    onlinemtrans.FMOT_Date = indianTime;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.PASR_Id = data.PACA_Id;
                    onlinemtrans.AMST_Id = 0;
                    onlinemtrans.FMOT_Receipt_no = rece_amt.receiptno;

                    onlinemtrans.FYP_PayModeType = "APP";
                    onlinemtrans.FMOT_PayGatewayType = data.onlinepaygteway;

                    onlinemtrans.MI_Id = data.MI_Id;
                    onlinemtrans.ASMAY_ID = data.ASMAY_Id;

                    _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                    for (int s = 0; s < data.selected_list.Count(); s++)
                    {
                        
                        
                            Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                            onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                            onlinettrans.FMA_Id = data.selected_list[s].FCMAS_Id;
                            onlinettrans.FTOT_Amount = data.selected_list[s].FCSS_ToBePaid;
                            onlinettrans.FTOT_Created_date = indianTime;
                            onlinettrans.FTOT_Updated_date = indianTime;
                            onlinettrans.FTOT_Concession = 0;
                            onlinettrans.FTOT_Fine = 0;

                      
                            _FeeGroupHeadContext.Fee_T_Online_TransactionDMO.Add(onlinettrans);
                       

                    }

                 //   onlinemtrans.FMOT_Amount = Convert.ToInt32(rece_amt.amt) + Convert.ToInt32(fineamountcal);

                    groupidss = "0";
                    fineamountcal = 0;
                    finecount = 0;
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
        public CollegeFeeTransactionDTO get_grp_reptnogroupwise(CollegeFeeTransactionDTO data)
        {
            try
            {
                var dynamicrecgen = new List<dynamic>();

                string onlineheadmapid = "0", groupidss = "0";

                List<string> mulreceiptno = new List<string>();
                List<string> fmgidrec = new List<string>();

               // var groupids = _FeeGroupHeadContext.Fee_Master_Stream_Group_MappingDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.PASL_ID == Convert.ToInt64(data.AMSE_Id) && t.AMCO_Id == Convert.ToInt64(data.AMCO_Id)).Select(t => t.FMG_Id).ToList();

                if (data.auto_receipt_flag == 1)
                {
                    List<long> HeadId = new List<long>();
                    List<long> termids = new List<long>();
                    foreach (var item in data.temarray)
                    {
                        HeadId.Add(item.FMH_Id);
                        termids.Add(item.FMT_Id);
                    }

                    foreach (var item in data.grouplist[0].trm_list)
                    {
                        termids.Add(item.FMT_Id);
                    }

                    List<CollegeFeeTransactionDTO> grps = new List<CollegeFeeTransactionDTO>();
                    grps = (from b in _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping
                            from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                            from d in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                            where (b.FMG_Id == c.FMG_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id) && c.FGAR_Id == d.FGAR_Id && b.ASMAY_Id == d.ASMAY_Id)
                            select new CollegeFeeTransactionDTO
                            {
                                FGAR_Id = c.FGAR_Id
                            }
                            ).Distinct().ToList();

                    for (int r = 0; r < grps.Count(); r++)
                    {
                        onlineheadmapid = grps[r].FGAR_Id.ToString();

                        List<CollegeFeeTransactionDTO> grps1 = new List<CollegeFeeTransactionDTO>();
                        grps1 = (from a in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                 from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                                 where (b.FGAR_Id == a.FGAR_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FGAR_Id == Convert.ToInt64(onlineheadmapid) )
                                 select new CollegeFeeTransactionDTO
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

                        List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();
                        List<CollegeFeeTransactionDTO> list_repts = new List<CollegeFeeTransactionDTO>();
                        list_all = (from b in _FeeGroupHeadContext.Fee_Groupwise_AutoReceiptDMO
                                    from c in _FeeGroupHeadContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                    where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                    select new CollegeFeeTransactionDTO
                                    {
                                        FGAR_PrefixName = b.FGAR_PrefixName,
                                        FGAR_SuffixName = b.FGAR_SuffixName,
                                        FGAR_Id = c.FGAR_Id,
                                        //FGAR_Name = b.FGAR_Name,
                                        //FGAR_Id = c.FGAR_Id
                                    }
                             ).Distinct().ToList();

                        var fineheadfmaidsaved = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                                  from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                  from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                                  from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                                  from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                               
                                                  from i in _FeeGroupHeadContext.Fee_T_College_PaymentDMO
                                                  from j in _FeeGroupHeadContext.FeeGroupGroupingDMO
                                                  from k in _FeeGroupHeadContext.Fee_Y_Payment_PA_Application
                                                  from l in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                                  where (l.FCMA_Id==b.FCMA_Id  && l.FMH_Id == a.FMH_Id && l.FTI_Id == a.fti_id && l.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && l.ASMAY_Id == data.ASMAY_Id && grpid.Contains(a.fmg_id) && l.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N")  && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id) && i.FCMAS_Id == b.FCMAS_Id && j.FMG_Id == a.fmg_id && k.FYP_Id == i.FYP_Id && k.PACA_Id == data.PACA_Id )
                                                  select new feeheadDTO
                                                  {
                                                      FCMAS_Id = b.FCMAS_Id,
                                                      FMH_FeeName = c.FMH_FeeName
                                                  }
            ).Distinct().ToList();

                        List<long> savedfma = new List<long>();
                        if (fineheadfmaidsaved.Count() > 0)
                        {
                            foreach (var sav in fineheadfmaidsaved)
                            {
                                savedfma.Add(sav.FCMAS_Id);
                            }
                        }

                        var readterms = (from a in _FeeGroupHeadContext.GenConfiguration
                                         where (a.MI_Id == data.MI_Id)
                                         select new CollegeFeeTransactionDTO
                                         {
                                             IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                                         }
                        ).FirstOrDefault();

                        long groupwiseamount = 0;
                        if (readterms.IVRMGC_Classwise_Payment != "1")
                        {
                            groupwiseamount = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                               from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                               from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                               from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                               from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                               from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                        
                                               from i in _FeeGroupHeadContext.PAYUDETAILS
                                               from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               where (g.FCMA_Id == b.FCMA_Id  && g.FMH_Id == a.FMH_Id

                                               && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id
                                               && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway &&
                                               b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id)  && !savedfma.Contains(b.FCMAS_Id))
                                               select new CollegeFeeTransactionDTO
                                               {
                                                   FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
                                               }
          ).Sum(t => t.FCSS_ToBePaid);
                        }
                        else
                        {
                            groupwiseamount = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                               from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                               from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                               from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                               from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                               from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO

                                               from i in _FeeGroupHeadContext.PAYUDETAILS
                                               from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                               
                                               where (g.FCMA_Id == b.FCMA_Id && g.FMH_Id == a.FMH_Id
                                               && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id
                                                && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway &&
                                                b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id)
                                                 && !savedfma.Contains(b.FCMAS_Id) && a.AMCO_Id == a.AMCO_Id)
                                               select new CollegeFeeTransactionDTO
                                               {
                                                   FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
                                               }
          ).Sum(t => t.FCSS_ToBePaid);
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

                            data.FYP_ReceiptNo = cmd.Parameters["@receiptno"].Value.ToString();

                            var rece_amt = new { receiptno = data.FYP_ReceiptNo, amt = groupwiseamount };

                            dynamicrecgen.Add(rece_amt);

                            List<CollegeFeeTransactionDTO> groupwisefmaids = new List<CollegeFeeTransactionDTO>();
                            if (readterms.IVRMGC_Classwise_Payment != "1")
                            {
                                groupwisefmaids = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                                   from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                   from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                                   from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                                   from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                                   from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                             
                                                   from i in _FeeGroupHeadContext.PAYUDETAILS
                                                   from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                   where (g.FCMA_Id==b.FCMA_Id &&  g.FMH_Id == a.FMH_Id && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id) && !savedfma.Contains(b.FCMAS_Id) )
                                                   select new CollegeFeeTransactionDTO
                                                   {
                                                       FCMAS_Id = b.FCMAS_Id,
                                                       FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
                                                   }
                                 ).Distinct().ToList();
                            }
                            else
                            {
                                groupwisefmaids = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                                   from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                   from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                                   from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                                   from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                                   from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                                   from i in _FeeGroupHeadContext.PAYUDETAILS
                                                   from j in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                                   where (g.FCMA_Id==b.FCMA_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && i.IMPG_Id == j.IMPG_Id && j.FPGD_Id == a.fpgd_id && i.IMPG_PGFlag == data.onlinepaygteway && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id)  && !savedfma.Contains(b.FCMAS_Id)  && a.AMCO_Id == a.AMCO_Id)
                                                   select new CollegeFeeTransactionDTO
                                                   {
                                                       FCMAS_Id = b.FCMAS_Id,
                                                       FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
                                                   }
                                 ).Distinct().ToList();
                            }


                            Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                            onlinemtrans.FMOT_Trans_Id = data.trans_id;
                            onlinemtrans.FMOT_Amount = rece_amt.amt;
                            onlinemtrans.FMOT_Date = DateTime.Now;
                            onlinemtrans.FMOT_Flag = "P";
                            onlinemtrans.AMST_Id = 0;
                            onlinemtrans.PASR_Id = data.PACA_Id;
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
                                onlinettrans.FMA_Id = groupwisefmaids[s].FCMAS_Id;
                                onlinettrans.FTOT_Amount = groupwisefmaids[s].FCSS_ToBePaid;
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
                    data.FYP_ReceiptNo = ab.GenerateNumber(data.transnumbconfigurationsettingsss);

                    List<long> HeadId = new List<long>();
                    List<long> termids = new List<long>();
                    foreach (var item in data.temarray)
                    {
                        HeadId.Add(item.FMH_Id);
                        termids.Add(item.FMT_Id);
                    }

                    foreach (var item in data.grouplist[0].trm_list)
                    {
                        termids.Add(item.FMT_Id);
                    }

                    List<CollegeFeeTransactionDTO> grps = new List<CollegeFeeTransactionDTO>();
                    grps = (from b in _FeeGroupHeadContext.CLG_Fee_Yearly_Group_Head_Mapping

                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

                            select new CollegeFeeTransactionDTO
                            {
                                FMG_Id = b.FMG_Id
                            }
                           ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    foreach (var item in grps)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    var groupwiseamount = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                           from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                           from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                           from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                           from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                           from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                           //from g in _FeeGroupHeadContext.feeYCC
                                           //from h in _FeeGroupHeadContext.feeYCCC
                                           where (g.FCMA_Id==b.FCMA_Id && g.FMH_Id == a.FMH_Id && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id))
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
                                           }
          ).Sum(t => t.FCSS_ToBePaid);

                    var groupwisefmaids = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                           from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                           from c in _FeeGroupHeadContext.FeeHeadClgDMO
                                           from d in _FeeGroupHeadContext.Clg_Fee_Installments_Yearly_DMO
                                           from f in _FeeGroupHeadContext.FeeGroupClgDMO
                                           from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                           where (g.FCMA_Id == b.FCMA_Id  && g.FTI_Id == a.fti_id && g.FMG_Id == a.fmg_id && c.FMH_Id == a.FMH_Id && d.FTI_Id == a.fti_id && b.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id && grpid.Contains(f.FMG_Id) && g.FMG_Id == f.FMG_Id && b.FCMAS_Amount > 0 && (   c.FMH_Flag == "N") && a.AMCO_Id == Convert.ToInt32(data.AMCO_Id))
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FCMAS_Id = b.FCMAS_Id,
                                               FCSS_ToBePaid = Convert.ToInt64(b.FCMAS_Amount)
                                           }
                               ).ToArray();


                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = data.trans_id;
                    onlinemtrans.FMOT_Amount = groupwiseamount;
                    onlinemtrans.FMOT_Date = DateTime.Now;
                    onlinemtrans.FMOT_Flag = "P";
                    onlinemtrans.AMST_Id = 0;
                    onlinemtrans.PASR_Id = data.PACA_Id;
                    onlinemtrans.FMOT_Receipt_no = data.FYP_ReceiptNo;

                    _FeeGroupHeadContext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);

                    for (int s = 0; s < groupwisefmaids.Count(); s++)
                    {
                        Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                        onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                        onlinettrans.FMA_Id = groupwisefmaids[s].FCMAS_Id;
                        onlinettrans.FTOT_Amount = groupwisefmaids[s].FCSS_ToBePaid;
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

        public Array PAYTMpaymentPart(CollegeFeeTransactionDTO enq, long totamount)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            Payment pay = new Payment(_context);
            ProspectusDTO data = new ProspectusDTO();
            int autoinc = 1, totpayableamount = 0;
            var fetchclass = (from a in _FeeGroupHeadContext.PA_College_Application
                              where (a.MI_Id == enq.MI_Id && a.PACA_Id == enq.PACA_Id)
                              select new CollegeFeeTransactionDTO
                              {
                                  AMCO_Id = a.AMCO_Id,
                                  ASMAY_Id = a.ASMAY_Id,
                                  AMSE_Id = a.AMSE_Id
                                  //pasl_id = a.PASL_ID
                              }
   ).Distinct().ToArray();

            string courseid = "0", academicyearid = "0";
            for (int s = 0; s < fetchclass.Count(); s++)
            {
                enq.AMCO_Id = Convert.ToInt64(fetchclass[s].AMCO_Id);
                enq.ASMAY_Id = Convert.ToInt64(fetchclass[s].ASMAY_Id);
                //enq.pasl_id = Convert.ToInt64(fetchclass[s].pasl_id);
                enq.AMSE_Id = Convert.ToInt64(fetchclass[s].AMSE_Id);
            }

            string multigrp = "0";
            for (int l = 0; l < enq.grouplist.Length; l++)
            {
                List<long> trm_ids = new List<long>();
                foreach (var x in enq.grouplist[l].trm_list)
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
                        Value = enq.PACA_Id
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


         
          // var paymentdetails = _FeeGroupHeadContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();

            var receipt = _FeeGroupHeadContext.Fee_Y_PaymentDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_ReceiptNo);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
           // PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY.Trim();


            PaymentDetailsDto.amount = Convert.ToDecimal(totamount.ToString());

            if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
            {
                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                enq.transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                enq.transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                //PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);

                PaymentDetailsDto.trans_id = "PrePartial" + enq.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();

            }

            List<CollegeFeeTransactionDTO> paymentdet = new List<CollegeFeeTransactionDTO>();
            paymentdet = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                          where (a.MI_Id == enq.MI_Id && a.FPGD_PGName == enq.onlinepaygteway)
                          select new CollegeFeeTransactionDTO
                          {
                              merchantid = a.FPGD_MerchantId,
                              merchantkey = a.FPGD_AuthorisationKey,
                              merchanturl = a.FPGD_URL
                          }
       ).ToList();

            List<CollegeFeeTransactionDTO> PAYMENTPARAMDETAILS = new List<CollegeFeeTransactionDTO>();
            PAYMENTPARAMDETAILS = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                   where (a.IMPG_PGFlag == enq.onlinepaygteway)
                                   select new CollegeFeeTransactionDTO
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

            //PaymentDetailsDto.firstname = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PASR_FirstName + ' ' + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PASR_MiddleName + ' ' + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PASR_LastName;


            //PaymentDetailsDto.email = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_emailId;

            //PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT.Trim();
            //PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
            //PaymentDetailsDto.phone = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_MobileNo;
            //PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
            //PaymentDetailsDto.udf2 = Convert.ToString(enq.PACA_Id);
            //PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
            //PaymentDetailsDto.udf4 = enq.multiplegroups.ToString();
            //PaymentDetailsDto.udf5 = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCO_Id.ToString();
            //PaymentDetailsDto.udf6 = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCO_Id.ToString();
            //PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/CollegePreAdmOnlinePayment/paymentresponse/";
            //PaymentDetailsDto.status = "success";
            //PaymentDetailsDto.service_provider = "payu_paisa";

            //PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

            var fetchuserid = (from a in _FeeGroupHeadContext.applicationUser
                               from b in _FeeGroupHeadContext.ApplicationUserRole
                               from c in _FeeGroupHeadContext.IVRM_Role_Type
                               where (a.Id == b.UserId && b.RoleTypeId == c.IVRMRT_Id && c.IVRMRT_RoleFlag == "ADMIN")
                               select new CollegeFeeTransactionDTO
                               {
                                   UserId = Convert.ToInt64(a.Id),
                               }
       ).Distinct().ToArray().Take(1);

            string useridss = "0";
            for (int s = 0; s < fetchuserid.Count(); s++)
            {
                useridss = fetchuserid.FirstOrDefault().UserId.ToString();
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
            parameters.Add("TXN_AMOUNT", totamount.ToString());
            parameters.Add("CHANNEL_ID", "WEB");

            //for production
            parameters.Add("INDUSTRY_TYPE_ID", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType);
            parameters.Add("WEBSITE", PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website);

            //parameters.Add("INDUSTRY_TYPE_ID", "Retail");
            //parameters.Add("WEBSITE", "WEBSTAGING ");

            parameters.Add("MOBILE_NO", ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_MobileNo.ToString());
            parameters.Add("EMAIL", ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_emailId.Trim());
            parameters.Add("MERC_UNQ_REF", enq.ASMAY_Id.ToString().Trim() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCO_Id.ToString() + "_" + enq.multiplegroups.ToString().Trim() + "_" + enq.MI_Id.ToString() + "_" + Convert.ToString(enq.PACA_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_emailId.Trim() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_MobileNo + "_" + totpayableamount.ToString());
            string url = paymentdet.FirstOrDefault().merchanturl;
            //parameters.Add("REQUEST_TYPE", "DEFAULT");
            parameters.Add("CALLBACK_URL", "http://localhost:57606/api/CollegePreAdmOnlinePayment/paymentresponsePAYTM/");

            string checksum = generateCheckSum(paymentdet.FirstOrDefault().merchantkey, parameters);

            aa.MID = paymentdet.FirstOrDefault().merchantid;
            aa.ORDER_ID = PaymentDetailsDto.trans_id;
            aa.CUST_ID = enq.MI_Id.ToString();
            aa.TXN_AMOUNT = Convert.ToDecimal(totamount);
            aa.CHANNEL_ID = "WEB";

            //for production
            aa.INDUSTRY_TYPE_ID = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_IndustryType;
            aa.WEBSITE = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_Website;

            //aa.INDUSTRY_TYPE_ID = "Retail";
            //aa.WEBSITE = "WEBSTAGING";

            aa.MOBILE_NO = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_MobileNo;
            aa.EMAIL = ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_emailId.Trim();
            aa.payu_URL = url;

            aa.CHECKSUMHASH = checksum;
            aa.MERC_UNQ_REF = enq.ASMAY_Id.ToString().Trim() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).AMCO_Id.ToString() + "_" + enq.multiplegroups.ToString().Trim() + "_" + enq.MI_Id.ToString() + "_" + Convert.ToString(enq.PACA_Id) + "_" + PaymentDetailsDto.trans_id + "_" + enq.MI_Id.ToString() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_emailId.Trim() + "_" + ((CollegeFeeTransactionDTO)enq.fillstudent.GetValue(0)).PACA_MobileNo + "_" + totpayableamount.ToString();

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
            CollegeFeeTransactionDTO pgmod = new CollegeFeeTransactionDTO();
            PaymentDetails dto = new PaymentDetails();

            string[] tokens = response.MERC_UNQ_REF.Split('_');

            // CODE SNIPPET FOR CHECKSUM GENERATION

            List<CollegeFeeTransactionDTO> paymentdet = new List<CollegeFeeTransactionDTO>();
            paymentdet = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                          where (a.MI_Id == Convert.ToInt32(tokens[6]) && a.FPGD_PGName == "PAYTM")
                          select new CollegeFeeTransactionDTO
                          {
                              merchantid = a.FPGD_MerchantId,
                              merchantkey = a.FPGD_AuthorisationKey,
                              merchanturl = a.FPGD_URL
                          }
       ).ToList();

            List<CollegeFeeTransactionDTO> PAYMENTPARAMDETAILS = new List<CollegeFeeTransactionDTO>();
            PAYMENTPARAMDETAILS = (from a in _FeeGroupHeadContext.PAYUDETAILS
                                   where (a.IMPG_PGFlag == "PAYTM")
                                   select new CollegeFeeTransactionDTO
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
            parameters.Add("CALLBACK_URL", "http://localhost:57606/api/CollegePreAdmOnlinePayment/paymentresponsePAYTM/");

            string checksum = generateCheckSum(paymentdet[0].merchantkey, parameters);

            // CODE SNIPPET FOR CHECKSUM VALIDATION
            Boolean res = false;
            res = verifyCheckSum(paymentdet[0].merchantkey, parameters, checksum);

            var emailmobile = (from a in _FeeGroupHeadContext.PA_College_Application
                               where (a.MI_Id == Convert.ToInt32(tokens[3].ToString()) && a.ASMAY_Id == Convert.ToInt32(tokens[0].ToString()) && a.PACA_Id == Convert.ToInt64(tokens[4]))
                               select new CollegeFeeTransactionDTO
                               {
                                   PACA_MobileNo = a.PACA_MobileNo,
                                   PACA_emailId = a.PACA_emailId

                               }
                            ).ToArray();

            if (emailmobile.Length > 0)
            {
                for (int i = 0; i < emailmobile.Length; i++)
                {
                    pgmod.AMCST_emailId = emailmobile[i].PACA_emailId;
                    pgmod.AMCST_MobileNo = emailmobile[i].PACA_MobileNo;
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

                var confirmstatus = insertdatainfeetables(dto.udf3, dto.udf2, dto.udf5, dto.amount, dto.trans_id, response.txnid.ToString(), dto.udf1);

                CollegeFeeTransactionDTO data = new CollegeFeeTransactionDTO();
                var transfersett = (from a in _FeeGroupHeadContext.MasterConfiguration
                                    where (a.MI_Id == Convert.ToInt32(dto.udf3) && a.ASMAY_Id == Convert.ToInt32(dto.udf1))
                                    select new CollegeFeeTransactionDTO
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
                    confirmstatusadmission = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission_college @p0,@p1,@p2", dto.udf2, dto.udf1, dto.udf3);
                }

                if (Convert.ToInt64(confirmstatus) > 0 || confirmstatusadmission > 0)
                {
                    pgmod.MI_Id = Convert.ToInt64(dto.udf3);
                    // pgmod.AMCST_MobileNo = response.phone;
                    pgmod.AMCST_Id = Convert.ToInt64(dto.udf2);
                    //  pgmod.AMCST_emailId = response.email;

                    Email Email = new Email(_context);

                    Email.sendmail(pgmod.MI_Id, pgmod.AMCST_emailId, "PREADMISSIONFEEONLINEPAYMENT", pgmod.AMCST_Id);

                    SMS sms = new SMS(_context);

                    sms.sendSms(pgmod.MI_Id, pgmod.AMCST_MobileNo, "PREADMISSIONFEEONLINEPAYMENT", pgmod.AMCST_Id);



                    //createusername(pgmod.Amst_Id,);
                }
            }
            else
            {
                pgmod.MI_Id = Convert.ToInt64(dto.udf3);
                // pgmod.AMCST_MobileNo = response.phone;
                pgmod.AMCST_Id = Convert.ToInt64(dto.udf2);
                //  pgmod.AMCST_emailId = response.email;

                Email Email = new Email(_context);

                Email.sendmail(pgmod.MI_Id, pgmod.AMCST_emailId, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.AMCST_Id);

                SMS sms = new SMS(_context);

                sms.sendSms(pgmod.MI_Id, pgmod.AMCST_MobileNo, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.AMCST_Id);

            }

            return response;
        }
        public Array paymentPartrazorpay(CollegeFeeTransactionDTO enq, long totamount)
        {
            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            Payment pay = new Payment(_context);
    
            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            try
            {
                ProspectusDTO data = new ProspectusDTO();
                int autoinc = 1, totpayableamount = 0;

                var fetchclass = (from a in _FeeGroupHeadContext.PA_College_Application
                                  where (a.MI_Id == enq.MI_Id && a.PACA_Id == enq.PACA_Id)
                                  select new CollegeFeeTransactionDTO
                                  {
                                      AMCO_Id = a.AMCO_Id,
                                      ASMAY_Id = a.ASMAY_Id,
                                      //pasl_id=a.PASL_ID
                                      AMSE_Id = a.AMSE_Id
                                  }
       ).Distinct().ToArray();

                string courseid = "0", academicyearid = "0";
                for (int s = 0; s < fetchclass.Count(); s++)
                {
                    enq.AMCO_Id = Convert.ToInt64(fetchclass[s].AMCO_Id);
                    enq.ASMAY_Id = Convert.ToInt64(fetchclass[s].ASMAY_Id);
                    //enq.pasl_id = Convert.ToInt64(fetchclass[s].pasl_id);
                    enq.AMSE_Id = Convert.ToInt64(fetchclass[s].AMSE_Id);
                }


                enq.ASMAY_Id = enq.ASMAY_Id;
                enq.grpidss = "0";
                string headidss = "0";

                List<long> grpids = new List<long>();
                List<long> headids = new List<long>();
                foreach (var x in enq.selected_list)
                {
                    grpids.Add(x.FMG_Id);
                    headids.Add(x.FMH_Id);
                    enq.grpidss = enq.grpidss + "," + x.FMG_Id;
                    headidss = headidss + "," + x.FMH_Id;
                }

                string fpgdids = "";
                var fpgdids1 = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                from b in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                where (a.fpgd_id == b.FPGD_Id && a.MI_Id == enq.MI_Id && grpids.Contains(a.fmg_id) && b.FPGD_PGName == enq.onlinepaygteway)
                                select new CollegeFeeTransactionDTO
                                {
                                    merchantid = b.FPGD_SubMerchantId
                                }
         ).Distinct().ToList();

                fpgdids = fpgdids1.FirstOrDefault().merchantid.ToString();

    

                //Fine Calculation

                var showstudetails = (from a in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                      from b in _FeeGroupHeadContext.Fee_College_Student_StatusDMO
                                      from c in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                      where (a.fmg_id == b.FMG_Id && a.FMH_Id == b.FMH_Id && a.fti_id == b.FTI_Id && a.fpgd_id == c.FPGD_Id && c.FPGD_PGName == enq.onlinepaygteway && b.MI_Id == enq.MI_Id && b.ASMAY_Id == enq.ASMAY_Id && b.AMCST_Id == enq.AMCST_Id && b.FCSS_ToBePaid > 0 && grpids.Contains(a.fmg_id) && headids.Contains(b.FMH_Id))
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FCMAS_Id = b.FCMAS_Id,
                                      }
          ).ToList();

               
          
                //Fine Calculation

               // int autoinc = 1, totpayableamount = 0;
                List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
                result.Add(new FeeSlplitOnlinePayment
                {
                    name = "splitId" + autoinc.ToString(),
                    merchantId = fpgdids,
                    value = ((Convert.ToInt32(enq.pendingamount))).ToString(),
                    commission = "0",
                    description = "Online Payment",
                });


                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();
                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey.ToString();

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

                PaymentDetailsDto.MARCHANT_ID = paymentdet.FirstOrDefault().merchantid;

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

                Dictionary<string, object> input = new Dictionary<string, object>();
                input.Add("amount", 1 * 100);
                //input.Add("amount", totamount * 100); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("receipt", PaymentDetailsDto.trans_id);
                input.Add("payment_capture", 1);

                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

                RazorpayClient client = new RazorpayClient(key, secret);

                Razorpay.Api.Order order = client.Order.Create(input);
                orderId = order["id"].ToString();

                enq.trans_id = orderId;
                enq.merchantkey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                enq.FMA_Amount = 100;
                enq.splitpayinformation = payinfo;

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
        //     public Array paymentPartrazorpay(CollegeFeeTransactionDTO enq, long totamount)
        //     {

        //         Payment pay = new Payment(_context);
        //         ProspectusDTO data = new ProspectusDTO();
        //         int autoinc = 1, totpayableamount = 0;

        //         var fetchclass = (from a in _FeeGroupHeadContext.PA_College_Application
        //                           where (a.MI_Id == enq.MI_Id && a.PACA_Id == enq.PACA_Id)
        //                           select new CollegeFeeTransactionDTO
        //                           {
        //                               AMCO_Id = a.AMCO_Id,
        //                               ASMAY_Id = a.ASMAY_Id,
        //                               //pasl_id=a.PASL_ID
        //                               AMSE_Id = a.AMSE_Id
        //                           }
        //).Distinct().ToArray();

        //         string courseid = "0", academicyearid = "0";
        //         for (int s = 0; s < fetchclass.Count(); s++)
        //         {
        //             enq.AMCO_Id = Convert.ToInt64(fetchclass[s].AMCO_Id);
        //             enq.ASMAY_Id = Convert.ToInt64(fetchclass[s].ASMAY_Id);
        //             //enq.pasl_id = Convert.ToInt64(fetchclass[s].pasl_id);
        //             enq.AMSE_Id = Convert.ToInt64(fetchclass[s].AMSE_Id);
        //         }

        //         string multigrp = "0";
        //         for (int l = 0; l < enq.grouplist.Length; l++)
        //         {
        //             List<long> trm_ids = new List<long>();
        //             foreach (var x in enq.grouplist[l].trm_list)
        //             {
        //                 multigrp = multigrp + ',' + x.FMT_Id;
        //                 trm_ids.Add(x.FMT_Id);
        //             }
        //         }

        //         enq.multiplegroups = multigrp;

        //         List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
        //         try
        //         {
        //             string ids = enq.ftiidss;

        //             using (var cmd1 = _FeeGroupHeadContext.Database.GetDbConnection().CreateCommand())
        //             {
        //                 cmd1.CommandText = "Preadmission_Split_Payment";
        //                 cmd1.CommandType = CommandType.StoredProcedure;

        //                 cmd1.Parameters.Add(new SqlParameter("@MI_Id",
        //                  SqlDbType.BigInt)
        //                 {
        //                     Value = enq.MI_Id
        //                 });

        //                 cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
        //                 SqlDbType.BigInt)
        //                 {
        //                     Value = enq.ASMAY_Id
        //                 });

        //                 cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
        //                 SqlDbType.VarChar)
        //                 {
        //                     Value = enq.PACA_Id
        //                 });

        //                 cmd1.Parameters.Add(new SqlParameter("@fmt_id",
        //                  SqlDbType.VarChar)
        //                 {
        //                     Value = enq.multiplegroups
        //                 });

        //                 cmd1.Parameters.Add(new SqlParameter("@pgname",
        //                 SqlDbType.VarChar)
        //                 {
        //                     Value = enq.onlinepaygteway
        //                 });

        //                 if (cmd1.Connection.State != ConnectionState.Open)
        //                     cmd1.Connection.Open();

        //                 try
        //                 {
        //                     using (var dataReader = cmd1.ExecuteReader())
        //                     {
        //                         while (dataReader.Read())
        //                         {
        //                             result.Add(new FeeSlplitOnlinePayment
        //                             {
        //                                 name = "splitId" + autoinc.ToString(),
        //                                 merchantId = dataReader["FPGD_MerchantId"].ToString(),
        //                                 value = dataReader["balance"].ToString(),
        //                                 commission = "0",
        //                                 description = "Online Payment",
        //                             });

        //                             autoinc = autoinc + 1;
        //                         }
        //                     }
        //                 }

        //                 catch (Exception ex)
        //                 {
        //                     Console.WriteLine(ex.Message);
        //                 }
        //             }
        //         }
        //         catch (Exception ex)
        //         {
        //             Console.WriteLine(ex.Message);
        //         }



        //         foreach (FeeSlplitOnlinePayment x in result)
        //         {
        //             totpayableamount = totpayableamount + Convert.ToInt32(x.value);
        //         }

        //         var item = new
        //         {
        //             paymentParts = result
        //         };

        //         PaymentDetails PaymentDetailsDto = new PaymentDetails();
        //         string payinfo = JsonConvert.SerializeObject(item);

        //         var  paymentdetails = _FeeGroupHeadContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.onlinepaygteway).Distinct().ToList();

        //         string orderId;

        //         Dictionary<string, object> input = new Dictionary<string, object>();
        //           input.Add("amount", totamount * 100);
        //         input.Add("currency", "INR");
        //         input.Add("receipt", PaymentDetailsDto.trans_id);
        //         input.Add("payment_capture", 1);

        //         string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
        //         string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

        //         RazorpayClient client = new RazorpayClient(key, secret);

        //         Razorpay.Api.Order order = client.Order.Create(input);
        //         orderId = order["id"].ToString();

        //         enq.trans_id = orderId;
        //         enq.FPGD_AuthorisationKey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
        //         enq.FCMAS_Amount = totpayableamount;
        //         enq.payinfo = payinfo;

        //         var fetchuserid = (from a in _FeeGroupHeadContext.applicationUser
        //                            from b in _FeeGroupHeadContext.ApplicationUserRole
        //                            from c in _FeeGroupHeadContext.IVRM_Role_Type
        //                            where (a.Id == b.UserId && b.RoleTypeId == c.IVRMRT_Id && c.IVRMRT_RoleFlag == "ADMIN")
        //                            select new CollegeFeeTransactionDTO
        //                            {
        //                                UserId = Convert.ToInt64(a.Id),
        //                            }
        //    ).Distinct().ToArray().Take(1);

        //         string useridss = "0";
        //         for (int s = 0; s < fetchuserid.Count(); s++)
        //         {
        //             useridss = fetchuserid.FirstOrDefault().UserId.ToString();
        //         }

        //         int grpset = 0;
        //         var feemasnum = _FeeGroupHeadContext.feemastersettings.Where(t => t.MI_Id == enq.MI_Id).ToArray();
        //         for (int s = 0; s < feemasnum.Count(); s++)
        //         {
        //             grpset = Convert.ToInt16(feemasnum[s].fee_group_setting);
        //         }

        //         if (grpset.Equals(0))
        //         {
        //             get_grp_reptno(enq);
        //         }
        //         else
        //         {
        //             get_grp_reptnogroupwise(enq);
        //         }

        //         if (enq.recenocol != null)
        //         {
        //             Array.Clear(enq.recenocol, 0, enq.recenocol.Length);
        //         }

        //         return PaymentDetailsDto.PaymentDetailsList;

        //     }

        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {

            CollegeFeeTransactionDTO pgmod = new CollegeFeeTransactionDTO();
            PaymentDetails dto = new PaymentDetails();

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            int r = 0;

            //single account added on 17/12/2019

            var accountvalidation = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                     where (a.MI_Id == response.IVRMOP_MIID && a.FPGD_PGName == "RAZORPAY")
                                     select new CollegeFeeTransactionDTO
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
                                select new CollegeFeeTransactionDTO
                                {
                                    FMHOT_Id = a.FMOT_Id,
                                    FCMAS_Amount = a.FMOT_Amount,
                                    MI_Id = a.MI_Id,
                                    ASMAY_Id = a.ASMAY_ID,
                                    AMCST_Id = a.PASR_Id,
                                }).ToArray();

            var fetchstudentdeatils = (from a in _FeeGroupHeadContext.PA_College_Application
                                       from b in _FeeGroupHeadContext.MasterCourseDMO
                                       from c in _FeeGroupHeadContext.ClgMasterBranchDMO
                                       from d in _FeeGroupHeadContext.CLG_Adm_Master_SemesterDMO
                                       where (a.AMCO_Id == b.AMCO_Id && a.PACA_Id == Convert.ToInt64(fetchfmhotid[0].AMCST_Id) && a.AMCO_Id == b.AMCO_Id && a.AMB_Id == c.AMB_Id && d.AMSE_Id == a.AMSE_Id)
                                       select new CollegeFeeTransactionDTO
                                       {
                                           AMCST_MobileNo = a.PACA_MobileNo,
                                           AMCST_emailId = a.PACA_emailId,
                                           AMCO_Id = b.AMCO_Id,
                                           AMCST_FirstName = a.PACA_FirstName + ' ' + a.PACA_MiddleName + ' ' + a.PACA_LastName,
                                           AMCST_AdmNo = a.PACA_RegistrationNo,
                                           AMCST_Id = a.PACA_Id,
                                           Mobile_No = a.PACA_MobileNo,
                                           PACA_emailId = a.PACA_emailId,
                                           AMCO_CourseName=b.AMCO_CourseName,
                                           AMB_BranchName=c.AMB_BranchName,
                                           AMSE_SEMName=d.AMSE_SEMName
                                       }).ToArray();

            var readterms = (from a in _FeeGroupHeadContext.GenConfiguration
                             where (a.MI_Id == Convert.ToInt64(fetchfmhotid[0].MI_Id))
                             select new CollegeFeeTransactionDTO
                             {
                                 IVRMGC_Classwise_Payment = a.IVRMGC_Classwise_Payment
                             }
                            ).FirstOrDefault();

            Dictionary<String, object> transfers = new Dictionary<String, object>();
            Dictionary<String, object> transfersnotes = new Dictionary<String, object>();

            List<CollegeFeeTransactionDTO> fetchaccountid = new List<CollegeFeeTransactionDTO>();
            for (r = 0; r < fetchfmhotid.Count(); r++)
            {
                transfers.Clear();
                transfersnotes.Clear();
                if (readterms.IVRMGC_Classwise_Payment != "1")
                {
                    fetchaccountid = (from a in _FeeGroupHeadContext.Fee_T_Online_TransactionDMO
                                      from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                      from c in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                      from d in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                      from e in _FeeGroupHeadContext.PAYUDETAILS
                                      from f in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                      where (a.FMA_Id == b.FCMAS_Id && b.FCMA_Id==f.FCMA_Id &&  f.FMG_Id == c.fmg_id && f.FMH_Id == c.FMH_Id && f.FTI_Id == c.fti_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && e.IMPG_PGFlag == "RAZORPAY" && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(fetchfmhotid[r].MI_Id) && f.ASMAY_Id == Convert.ToInt64(fetchfmhotid[r].ASMAY_Id))
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FPGD_SubMerchantId = d.FPGD_SubMerchantId
                                      }).Distinct().ToList();
                }
                else
                {
                    fetchaccountid = (from a in _FeeGroupHeadContext.Fee_T_Online_TransactionDMO
                                      from b in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                      from c in _FeeGroupHeadContext.CLG_Fee_OnlinePayment_MappingDMO
                                      from d in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                                      from e in _FeeGroupHeadContext.PAYUDETAILS
                                      from f in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                      
                                      where (a.FMA_Id == b.FCMAS_Id && b.FCMA_Id==f.FCMA_Id && f.FMG_Id == c.fmg_id && f.FMH_Id == c.FMH_Id && f.FTI_Id == c.fti_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && e.IMPG_PGFlag == "RAZORPAY" && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(fetchfmhotid[r].MI_Id) && f.ASMAY_Id == Convert.ToInt64(fetchfmhotid[r].ASMAY_Id) && f.AMCO_Id == Convert.ToInt64(fetchstudentdeatils[0].AMCO_Id))
                                      select new CollegeFeeTransactionDTO
                                      {
                                          FPGD_SubMerchantId = d.FPGD_SubMerchantId
                                      }).Distinct().ToList();
                }

                var fetchamount = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                   where (a.PASR_Id == Convert.ToInt64(fetchfmhotid[0].AMCST_Id) && a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id)
                                   select new CollegeFeeTransactionDTO
                                   {
                                       FCMAS_Amount = a.FMOT_Amount
                                   }).ToArray();

                transfersnotes.Add("notes_1", fetchstudentdeatils[0].AMCST_FirstName);
                transfersnotes.Add("notes_2", fetchstudentdeatils[0].AMCST_AdmNo);
                transfersnotes.Add("notes_3", fetchstudentdeatils[0].AMCST_Id);
                transfersnotes.Add("notes_4", fetchstudentdeatils[0].AMCST_MobileNo);
                transfersnotes.Add("notes_5", fetchstudentdeatils[0].AMCST_emailId);

                transfers.Add("account", (fetchaccountid.FirstOrDefault().FPGD_SubMerchantId));
                transfers.Add("amount", (Convert.ToInt32(fetchamount.FirstOrDefault().FCMAS_Amount.ToString()) * 100).ToString());
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
                    fet.AMOUNT = fetchamount.FirstOrDefault().FCMAS_Amount.ToString();
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
            var status ="";
            var confirmstatusadmission = 0;
            if (response.status == "success")
            {
              
                      status = insertdatainfeetables(fetchfmhotid[0].MI_Id.ToString(), fetchfmhotid[0].AMCST_Id.ToString(), fetchfmhotid[0].AMCO_Id.ToString(), Convert.ToInt64(fetchfmhotid[0].FCMAS_Amount), response.order_id, response.razorpay_payment_id, fetchfmhotid[0].ASMAY_Id.ToString());

                CollegeFeeTransactionDTO data = new CollegeFeeTransactionDTO();
                var transfersett = (from a in _FeeGroupHeadContext.MasterConfiguration
                                    where (a.MI_Id == Convert.ToInt32(fetchfmhotid[0].MI_Id) && a.ASMAY_Id == Convert.ToInt32(fetchfmhotid[0].ASMAY_Id))
                                    select new CollegeFeeTransactionDTO
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
                  confirmstatusadmission = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Export_from_preadmission_to_admission_college @p0,@p1,@p2,@p3", fetchfmhotid[0].AMCST_Id, fetchfmhotid[0].ASMAY_Id, fetchfmhotid[0].MI_Id,response.UserId );
                }

                if (Convert.ToInt64(status) > 0 || confirmstatusadmission > 0)
                {
                    pgmod.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                    pgmod.AMCST_Id = Convert.ToInt64(fetchfmhotid[0].AMCST_Id);
                    Email Email = new Email(_context);
                    Email.sendmail(pgmod.MI_Id, fetchstudentdeatils[0].AMCST_emailId, "PREADMISSIONFEEONLINEPAYMENT", pgmod.AMCST_Id);
                    SMS sms = new SMS(_context);
                    sms.sendSms(pgmod.MI_Id, fetchstudentdeatils[0].AMCST_MobileNo, "PREADMISSIONFEEONLINEPAYMENT", pgmod.AMCST_Id);
                }
            }
            else
            {
                pgmod.MI_Id = Convert.ToInt64(fetchfmhotid[0].MI_Id);
                pgmod.AMCST_Id = Convert.ToInt64(fetchfmhotid[0].AMCST_Id);
                Email Email = new Email(_context);
                Email.sendmail(pgmod.MI_Id, pgmod.AMCST_emailId, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.AMCST_Id);
                SMS sms = new SMS(_context);
                sms.sendSms(pgmod.MI_Id, pgmod.AMCST_MobileNo, "PREADMISSIONFEEONLINEPAYMENTFAIL", pgmod.AMCST_Id);

            }

            return response;
        }

        public CollegeFeeTransactionDTO Razorpaypaymentsettlementresponse(CollegeFeeTransactionDTO response)
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
                                                       where (a.MI_Id == response.MI_Id && a.FMOT_Trans_Id == id && a.AMST_Id == 0)
                                                       select new CollegeFeeTransactionDTO
                                                       {
                                                           order_id = a.FMOT_Trans_Id
                                                       }
                         ).ToList();

                            if (registrationpayment.Count > 0)
                            {
                                var pendingpayments = (from a in _FeeGroupHeadContext.Fee_Y_PaymentDMO
                                                       from b in _FeeGroupHeadContext.FEE_RAZOR_TRANSFER_API_DETAILS
                                                       from c in _FeeGroupHeadContext.Fee_T_College_PaymentDMO
                                                       from d in _FeeGroupHeadContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                       from e in _FeeGroupHeadContext.FeeGroupClgDMO
                                                       from f in _FeeGroupHeadContext.FeeHeadClgDMO
                                                       from g in _FeeGroupHeadContext.Clg_Fee_AmountEntry_DMO
                                                       where (d.FCMA_Id==g.FCMA_Id && a.ASMAY_Id ==g.ASMAY_Id && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == d.FCMAS_Id && g.FMG_Id == e.FMG_Id && g.FMH_Id == f.FMH_Id && a.MI_Id == response.MI_Id && b.ORDER_ID == id && a.FYP_Transaction_Id == id && a.FYP_ChallanStatusFlag == "Sucessfull" && a.FYP_PayGatewayType == "RAZORPAY" && (e.FMG_CompulsoryFlag == "1" || f.FMH_Flag == "N"))
                                                       select new CollegeFeeTransactionDTO
                                                       {
                                                           order_id = a.FYP_Transaction_Id
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

                List<CollegeFeeTransactionDTO> paymentdet = new List<CollegeFeeTransactionDTO>();
                paymentdet = (from a in _FeeGroupHeadContext.Fee_PaymentGateway_Details
                              where (a.MI_Id == data.MI_Id && a.FPGD_PGName == "PAYTM")
                              select new CollegeFeeTransactionDTO
                              {
                                  merchantid = a.FPGD_MerchantId,
                                  merchantkey = a.FPGD_AuthorisationKey,
                                  merchanturl = a.FPGD_URL
                              }
           ).ToList();

                List<CollegeFeeTransactionDTO> pendingtransactions = new List<CollegeFeeTransactionDTO>();

                pendingtransactions = (from a in _FeeGroupHeadContext.Fee_M_Online_TransactionDMO
                                       where (a.MI_Id == data.MI_Id && a.FMOT_PayGatewayType == "PAYTM" && a.AMST_Id == 0 && Convert.ToDateTime(a.FMOT_Date.ToString("yyyy-MM-dd")) >= Convert.ToDateTime(indianTime.ToString("yyyy-MM-dd")))
                                       select new CollegeFeeTransactionDTO
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
                            var txnstatusexists = (from a in _FeeGroupHeadContext.Fee_Y_PaymentDMO
                                                   where (a.MI_Id == data.MI_Id && a.FYP_Transaction_Id == (string)joResponse1["ORDERID"])
                                                   select new CollegeFeeTransactionDTO
                                                   {
                                                      FYPPM_Transaction_Id = a.FYP_Transaction_Id,
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
    }
}
