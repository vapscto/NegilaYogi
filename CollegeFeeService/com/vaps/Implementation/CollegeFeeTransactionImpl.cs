using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using CollegeFeeService.com.vaps.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using System.Collections.Concurrent;
using CommonLibrary;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.College.Fees;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs;
using Razorpay.Api;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using easebuzz_.net;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegeFeeTransactionImpl : CollegeFeeTransactionInterface
    {
        private static ConcurrentDictionary<string, CollegeFeeTransactionDTO> _login =
        new ConcurrentDictionary<string, CollegeFeeTransactionDTO>();

        private static readonly Object obj = new Object();

        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        readonly ILogger<CollegeFeeTransactionImpl> _logger;
        public CollegeFeeTransactionImpl(CollFeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context, ILogger<CollegeFeeTransactionImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
            _logger = log;
        }
        public CollegeFeeTransactionDTO getdata(CollegeFeeTransactionDTO data)
        {
            try
            {
                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();//&& t.userid == data.UserId
                data.feeconfiglist = feemasnum.ToArray();

                List<FeeGroupClgDMO> group = new List<FeeGroupClgDMO>();
                group = _YearlyFeeGroupMappingContext.FeeGroupClgDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == data.MI_Id).OrderBy(y => y.FMG_Order).ToList();
                data.fillmastergroup = group.ToArray();

                List<MasterAcademic> yearlist = new List<MasterAcademic>();
                yearlist = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.Is_Active == true && t.MI_Id == data.MI_Id).OrderBy(y => y.ASMAY_Order).ToList();
                data.fillyear = yearlist.ToArray();

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _YearlyFeeGroupMappingContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Transaction").ToList();
                data.transnumconfig = masnum.ToArray();

                var rolename = _YearlyFeeGroupMappingContext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.RoleId).IVRMRT_Role;

                data.rolename = rolename;

                bool recsettingval = false;
                string maxval = "";
                var getcurrsett = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.ASMAY_ID.Equals(data.ASMAY_Id) && t.MI_Id == data.MI_Id).ToList();
                foreach (var value in getcurrsett)
                {
                    if (value.FMC_AutoReceiptFeeGroupFlag == 1)
                    {
                        recsettingval = true;
                    }
                    else
                    {
                        recsettingval = false;
                    }
                }

                var fetchmaxfypid = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                     where (a.FYP_Id == b.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_ChequeBounceFlag=="CL")
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FYP_Id = a.FYP_Id,
                                     }
               ).OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                          from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                          from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                          from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                          from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                          from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                          where (f.AMB_Id == d.AMB_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_Id == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && g.AMSE_Id == d.AMSE_Id && d.ACYST_ActiveFlag==1 && h.ACMS_Id == d.ACMS_Id && (string.IsNullOrEmpty(a.FYP_ChallanNo)))
                                          select new CollegeFeeTransactionDTO
                                          {
                                              AMCST_Id = c.AMCST_Id,
                                              AMCST_FirstName = c.AMCST_FirstName,
                                              AMCST_MiddleName = c.AMCST_MiddleName,
                                              AMCST_LastName = c.AMCST_LastName,
                                              FYP_ReceiptNo = a.FYP_ReceiptNo,
                                              FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                              FYP_PayModeType = a.FYP_PayModeType,
                                              FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                              AMCO_CourseName = e.AMCO_CourseName,
                                              AMB_BranchName = f.AMB_BranchName,
                                              AMSE_SEMName = g.AMSE_SEMName,
                                              ACMS_SectionName = h.ACMS_SectionName,
                                              FYP_Id = a.FYP_Id,
                                              AMCST_AdmNo = c.AMCST_AdmNo,
                                              FYP_ReceiptDate = a.FYP_ReceiptDate,
                                              FYP_ApprovedFlg=a.FYP_ApprovedFlg,
                                              UserId=a.User_Id
                                          }
     ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

                data.specialheaddetails = (from a in _YearlyFeeGroupMappingContext.feespecialHead
                                           from b in _YearlyFeeGroupMappingContext.feeSGGG
                                           from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                           where (a.MI_Id == data.MI_Id && a.FMSFH_ActiceFlag == true && a.FMSFH_Id == b.FMSFH_Id && b.FMSFHFH_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == b.FMH_Id)
                                           select new FeeSpecialFeeGroupDTO
                                           {
                                               FMSFH_Id = a.FMSFH_Id,
                                               FMSFH_Name = a.FMSFH_Name,
                                               FMSFHFH_Id = b.FMSFHFH_Id,
                                               FMH_ID = b.FMH_Id,
                                               FMH_Name = c.FMH_FeeName
                                           }).Distinct().ToArray();

                var specialheadlist = _YearlyFeeGroupMappingContext.feespecialHead.Where(t => t.MI_Id == data.MI_Id && t.FMSFH_ActiceFlag == true).Distinct().ToList();
                data.specialheadlist = specialheadlist.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO getdatastuacad(CollegeFeeTransactionDTO data)
        {
            try
            {
                if (data.filterinitialdata == "regular")
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                        }
             ).OrderBy(t => t.AMCST_FirstName).ToArray();
                }

                else if (data.filterinitialdata.Equals("InActive"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "D" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("regno"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_RegistrationNo,
                                            AMCST_MiddleName = "",
                                            AMCST_LastName = ""

                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("AdmNo"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_AdmNo = a.AMCST_AdmNo,
                                            AMCST_MiddleName = "",
                                            AMCST_LastName = ""

                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameAdmno"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName + "-" + a.AMCST_AdmNo,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName + "-" + a.AMCST_AdmNo
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("Admnoname"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_AdmNo + "-" + a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameRegNo"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName + "-" + a.AMCST_RegistrationNo,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName + "-" + a.AMCST_RegistrationNo,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("RegNoName"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_RegistrationNo + "-" + a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                        }
             ).ToArray();
                }

                //added now

                var fetchmaxfypid = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                     where (a.FYP_Id == b.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.User_Id == data.UserId && a.FYP_ChequeBounceFlag == "CL")
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FYP_Id = a.FYP_Id,
                                     }
              ).OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                          from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                          from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                          from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                          from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                          from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                          where (f.AMB_Id == d.AMB_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_Id == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && g.AMSE_Id == d.AMSE_Id && d.ACYST_ActiveFlag == 1 && h.ACMS_Id == d.ACMS_Id && (string.IsNullOrEmpty(a.FYP_ChallanNo)))
                                          select new CollegeFeeTransactionDTO
                                          {
                                              AMCST_Id = c.AMCST_Id,
                                              AMCST_FirstName = c.AMCST_FirstName,
                                              AMCST_MiddleName = c.AMCST_MiddleName,
                                              AMCST_LastName = c.AMCST_LastName,
                                              FYP_ReceiptNo = a.FYP_ReceiptNo,
                                              FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                              FYP_PayModeType = a.FYP_PayModeType,
                                              FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                              AMCO_CourseName = e.AMCO_CourseName,
                                              AMB_BranchName = f.AMB_BranchName,
                                              AMSE_SEMName = g.AMSE_SEMName,
                                              ACMS_SectionName = h.ACMS_SectionName,
                                              FYP_Id = a.FYP_Id,
                                              AMCST_AdmNo = c.AMCST_AdmNo,
                                              FYP_ReceiptDate = a.FYP_ReceiptDate,
                                              UserId = a.User_Id
                                          }
     ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO getdatastuacadgrp(CollegeFeeTransactionDTO data)
        {
            try
            {
                if (data.configset.Equals("T"))
                {
                    var readterms = (from a in _YearlyFeeGroupMappingContext.feeTr
                                     where (a.MI_Id == data.MI_Id)
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FMT_Name = a.FMT_Name,
                                         FMT_Id = a.FMT_Id,
                                     }
    ).Distinct().ToArray();

                    string alltrids = "0";
                    List<long> FMT_Ids = new List<long>();
                    for (int s = 0; s < readterms.Count(); s++)
                    {
                        alltrids = alltrids + ',' + readterms[s].FMT_Id.ToString();
                        FMT_Ids.Add(readterms[s].FMT_Id);
                    }
                    data.disableterms = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                         from b in _YearlyFeeGroupMappingContext.feeMTH
                                         where (a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.AMCST_Id == data.AMCST_Id && a.User_Id == data.UserId && a.FCSS_NetAmount > 0 && FMT_Ids.Contains(b.FMT_Id) && a.ASMAY_Id==data.ASMAY_Id)
                                         group new { a, b } by new { b.FMT_Id } into g
                                         select new CollegeFeeTransactionDTO
                                         {
                                             FCSS_NetAmount = g.Sum(t => t.a.FCSS_NetAmount) + g.Sum(t => t.a.FCSS_OBArrearAmount),
                                             FCSS_ToBePaid = g.Sum(t => t.a.FCSS_ToBePaid) + g.Sum(t => t.a.FCSS_OBArrearAmount),
                                             FCSS_PaidAmount = g.Sum(t => t.a.FCSS_PaidAmount) + g.Sum(t => t.a.FCSS_ConcessionAmount),
                                             FMT_Id = g.Key.FMT_Id
                                         }).Distinct().ToArray();
                }
                else if (data.configset.Equals("G"))
                {
                    data.disableterms = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                         from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                         where (a.AMCST_Id == data.AMCST_Id &&  a.FMG_Id == b.FMG_Id && a.ASMAY_Id==data.ASMAY_Id)
                                         group a by new { a.FMG_Id, b.FMG_Order } into g
                                         select new CollegeFeeTransactionDTO
                                         {
                                             FCSS_NetAmount = g.Sum(t => t.FCSS_NetAmount) + g.Sum(t => t.FCSS_OBArrearAmount),
                                             FCSS_ToBePaid = g.Sum(t => t.FCSS_ToBePaid) + g.Sum(t => t.FCSS_OBArrearAmount),
                                             FCSS_PaidAmount = g.Sum(t => t.FCSS_PaidAmount) + g.Sum(t => t.FCSS_ConcessionAmount),
                                             FMT_Id = g.Key.FMG_Id,
                                             FMG_Order = g.Key.FMG_Order,

                                         }).Distinct().OrderBy(t => t.FMG_Order).ToArray();
                }


                if (data.filterinitialdata != "Preadmission")
                {
                    if (data.configset.Equals("T"))
                    {
                        data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                                from b in _YearlyFeeGroupMappingContext.feeTr
                                                from c in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                                where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == c.FTI_Id && c.AMCST_Id == data.AMCST_Id && c.MI_Id == b.MI_Id && c.ASMAY_Id==data.ASMAY_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                                select new CollegeFeeTransactionDTO
                                                {
                                                    FMG_GroupName = b.FMT_Name,
                                                    FMG_Id = a.FMT_Id,
                                                }
         ).Distinct().ToArray();

                    }
                    else
                    {
                        data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                //from b in _YearlyFeeGroupMappingContext.Fee_College_Master_Student_GroupHeadDMO
                                                from c in _YearlyFeeGroupMappingContext.FEeGroupLoginPreviledgeDMO
                                                from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                                where (a.MI_Id == d.MI_Id && d.AMCST_Id == data.AMCST_Id && a.FMG_Id == d.FMG_Id && a.FMG_ActiceFlag == true && c.User_Id == data.UserId && d.ASMAY_Id==data.ASMAY_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                                select new CollegeFeeTransactionDTO
                                                {
                                                    FMG_GroupName = a.FMG_GroupName,
                                                    FMG_Id = a.FMG_Id,
                                                    FMG_Order = a.FMG_Order
                                                }
          ).Distinct().OrderBy(t => t.FMG_Order).ToArray();

                    }


                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        from c in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                        from d in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                        from e in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                        from f in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                        where (b.AMCO_Id == c.AMCO_Id && d.AMB_Id == b.AMB_Id && a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && e.AMSE_Id == b.AMSE_Id && f.ACMS_Id == b.ACMS_Id && b.ACYST_ActiveFlag==1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                            AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                            AMCST_AdmNo = a.AMCST_AdmNo,
                                            ACYST_RollNo = b.ACYST_RollNo,
                                            AMCO_CourseName = c.AMCO_CourseName,
                                            AMB_BranchName = d.AMB_BranchName,
                                            AMSE_SEMName = e.AMSE_SEMName,
                                            ACMS_SectionName = f.ACMS_SectionName,
                                            AMCST_FatherName = a.AMCST_FatherName,
                                            AMCST_DOB = a.AMCST_DOB,
                                            AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo),
                                            fathername=a.AMCST_FatherName
                                        }
                   ).Distinct().ToArray();

                    data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                   from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                                   from d in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                   from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                                   from f in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                                   from g in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                                   from k in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                                   from h in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                                   from i in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                                   from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                                   from l in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                   from m in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                                   where (a.FYP_Id == b.FYP_Id && a.FCMAS_Id == l.FCMAS_Id && l.FCMA_Id == c.FCMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMCST_Id == j.AMCST_Id && f.AMCO_Id == g.AMCO_Id && f.AMB_Id == k.AMB_Id && f.AMSE_Id == h.AMSE_Id && i.AMCST_Id == j.AMCST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMCST_Id == data.AMCST_Id && m.ACMS_Id == f.ACMS_Id && f.ACYST_ActiveFlag==1)
                                                   select new CollegeFeeTransactionDTO
                                                   {
                                                       AMCST_Id = f.AMCST_Id,
                                                       AMCST_FirstName = i.AMCST_FirstName,
                                                       AMCST_MiddleName = i.AMCST_MiddleName,
                                                       AMCST_LastName = i.AMCST_LastName,
                                                       FMH_FeeName = d.FMH_FeeName,
                                                       FTI_Name = e.FTI_Name,
                                                       FTCP_PaidAmount = a.FTCP_PaidAmount,
                                                       FTCP_ConcessionAmount = a.FTCP_ConcessionAmount,
                                                       FTCP_FineAmount = a.FTCP_FineAmount,
                                                       AMCO_CourseName = g.AMCO_CourseName,
                                                       AMB_BranchName = k.AMB_BranchName,
                                                       AMSE_SEMName = h.AMSE_SEMName,
                                                       ACYST_RollNo = f.ACYST_RollNo,
                                                       AMCST_AdmNo = i.AMCST_AdmNo,
                                                       AMCST_FatherName = i.AMCST_FatherName,
                                                       FYP_Remarks = b.FYP_Remarks,
                                                       AMCST_RegistrationNo = i.AMCST_RegistrationNo
                                                   }
                   ).Distinct().ToArray();
                }

                data.showstudetails = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                       from b in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from c in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                       from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                       from e in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                       from f in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                       from g in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                       from i in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                       from h in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                       where (c.AMCST_Id == h.AMCST_Id && h.FYP_Id == f.FYP_Id && g.FYP_Id == h.FYP_Id && f.FCMAS_Id == i.FCMAS_Id && i.FCMA_Id == e.FCMA_Id && e.FMH_Id == b.FMH_Id && e.FTI_Id == d.FTI_Id && e.FMG_Id == a.FMG_Id && g.MI_Id == data.MI_Id && h.AMCST_Id == data.AMCST_Id && c.ACYST_ActiveFlag==1)
                                       select new CollegeFeeTransactionDTO
                                       {
                                           FMG_GroupName = a.FMG_GroupName,
                                           FMH_FeeName = b.FMH_FeeName,
                                           FTI_Name = d.FTI_Name,
                                           FTCP_PaidAmount = f.FTCP_PaidAmount,
                                           FTCP_ConcessionAmount = f.FTCP_ConcessionAmount,
                                       }
         ).OrderBy(t => t.FYP_ReceiptDate).Distinct().ToArray();

                var showstaticticsdetails = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                             where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.User_Id == data.UserId && a.AMCST_Id == data.AMCST_Id) 
                                             select new CollegeFeeTransactionDTO
                                             {
                                                 FCSS_CurrentYrCharges = a.FCSS_CurrentYrCharges,
                                                 FCSS_ToBePaid = a.FCSS_ToBePaid,
                                                 FCSS_PaidAmount = a.FCSS_PaidAmount,
                                                 FCSS_ConcessionAmount = a.FCSS_ConcessionAmount,
                                                 FCSS_OBArrearAmount = a.FCSS_OBArrearAmount,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMG_Id = b.FMG_Id
                                             }
        ).ToList();

                data.showstaticticsdetails = (from i in showstaticticsdetails
                                              group i by new { i.FMG_Id, i.FMG_GroupName } into g
                                              select new CollegeFeeTransactionDTO
                                              {
                                                  FCSS_CurrentYrCharges = g.Sum(t => t.FCSS_CurrentYrCharges),
                                                  FCSS_ToBePaid = g.Sum(t => t.FCSS_ToBePaid),
                                                  FCSS_PaidAmount = g.Sum(t => t.FCSS_PaidAmount),
                                                  FCSS_ConcessionAmount = g.Sum(t => t.FCSS_ConcessionAmount),
                                                  FCSS_OBArrearAmount = g.Sum(t => t.FCSS_OBArrearAmount),
                                                  FMG_GroupName = g.Key.FMG_GroupName,
                                                  FMG_Id = g.Key.FMG_Id
                                              }).Distinct().ToArray();

                DateTime date = DateTime.Now;

                data.getpdcdetails = (from a in _YearlyFeeGroupMappingContext.Fee_College_Studentwise_PDCDMO
                                      from b in _YearlyFeeGroupMappingContext.Fee_Master_BankDMO
                                      where (a.MI_Id == b.MI_Id && a.FMBANK_Id == b.FMBANK_Id && a.MI_Id == data.MI_Id && a.FCSPDC_ActiveFlg == true && a.AMCST_Id == data.AMCST_Id && b.FMBANK_ActiveFlg == true)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FCSPDC_Id = a.FCSPDC_Id,
                                          FCSPDC_ChequeNo = a.FCSPDC_ChequeNo,
                                          FCSPDC_ChequeDate = a.FCSPDC_ChequeDate,
                                          FMBANK_BankName = b.FMBANK_BankName,
                                          FCSPDC_Amount = a.FCSPDC_Amount,
                                          FCSPDC_Status = a.FCSPDC_Status,
                                          FCSPDC_Narration = a.FCSPDC_Narration,
                                          FMBANK_Id = b.FMBANK_Id
                                      }
).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO getstuddet(CollegeFeeTransactionDTO data)
        {
            try
            {
                if (data.filterinitialdata.Equals("regular"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("InActive"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "D" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("regno"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_RegistrationNo,
                                            AMCST_MiddleName = "",
                                            AMCST_LastName = ""

                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("AdmNo"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_AdmNo,
                                            AMCST_MiddleName = "",
                                            AMCST_LastName = ""

                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("Admnoname"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_AdmNo + "-" + a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameAdmno"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName + "-" + a.AMCST_AdmNo,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName + "-" + a.AMCST_AdmNo,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameRegNo"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName + "-" + a.AMCST_RegistrationNo,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName + "-" + a.AMCST_RegistrationNo,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("RegNoName"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_RegistrationNo + "-" + a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                        }
             ).ToArray();
                }

            }
            catch (Exception e)
            {
                data.validationvalue = "Contact Administrator";
            }
            return data;
        }
        public CollegeFeeTransactionDTO getstuddetnew(CollegeFeeTransactionDTO data)
        {
            try
            {
                List<CollegeFeeTransactionDTO> V_StudentPendinglist = new List<CollegeFeeTransactionDTO>();
                var retObject = new List<dynamic>();
                var retObject1 = new List<dynamic>();

                data.Bankname = (from a in _YearlyFeeGroupMappingContext.Fee_Master_BankDMO
                                 where (a.MI_Id == data.MI_Id && a.FMBANK_ActiveFlg==true)
                                 select new CollegeFeeTransactionDTO
                                 {
                                     FMBANK_Id = a.FMBANK_Id,
                                     FMBANK_BankName = a.FMBANK_BankName,
                                 }
    ).Distinct().ToArray();

                if (data.filterinitialdata != "Preadmission")
                {

                    var saved_fma = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                     from b in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                     from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                     where (a.FCMAS_Id == b.FCMAS_Id && b.FYP_Id == c.FYP_Id && a.AMCST_Id == c.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                     select b.FCMAS_Id
).Distinct().ToList();
                    var myArray = data.multiplegroups.Split(',');
                    List<long> terms_groups = new List<long>();
                    for (int i = 0; i < myArray.Length; i++)
                    {
                        terms_groups.Add(Convert.ToInt64(myArray[i]));
                    }
                    data.terms_groups = terms_groups.ToArray();

                    if (data.configset.Equals("G"))
                    {
                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Get_Student_Status_Details";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                              SqlDbType.VarChar, 10)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                         SqlDbType.VarChar, 10)
                            {
                                Value = data.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@User_Id",
                                SqlDbType.VarChar, 10)
                            {
                                Value = data.UserId
                            });

                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                                SqlDbType.VarChar, 10)
                            {
                                Value = data.AMCST_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                                SqlDbType.VarChar, 100)
                            {
                                Value = data.multiplegroups
                            });
                            cmd.Parameters.Add(new SqlParameter("@FYP_ReceiptDate",
                            SqlDbType.DateTime, 100)
                            {
                                Value = data.FYP_ReceiptDate
                            });


                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }

                                        retObject.Add((ExpandoObject)dataRow1);
                                    }

                                }
                                data.alldata = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message);
                                _logger.LogDebug(ex.Message);
                            }
                        }

                        using (var cmd1 = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd1.CommandText = "Get_Student_Status_AdvanceDetails";
                            cmd1.CommandType = CommandType.StoredProcedure;

                            cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                              SqlDbType.VarChar, 10)
                            {
                                Value = data.MI_Id
                            });

                            cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                         SqlDbType.VarChar, 10)
                            {
                                Value = data.ASMAY_Id
                            });

                            cmd1.Parameters.Add(new SqlParameter("@User_Id",
                                SqlDbType.VarChar, 10)
                            {
                                Value = data.UserId
                            });

                            cmd1.Parameters.Add(new SqlParameter("@AMCST_Id",
                                SqlDbType.VarChar, 10)
                            {
                                Value = data.AMCST_Id
                            });
                            cmd1.Parameters.Add(new SqlParameter("@FMG_Id",
                                SqlDbType.VarChar, 100)
                            {
                                Value = data.multiplegroups
                            });
                            if (cmd1.Connection.State != ConnectionState.Open)
                                cmd1.Connection.Open();
                            try
                            {
                                using (var dataReader = cmd1.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }

                                        retObject1.Add((ExpandoObject)dataRow1);
                                    }

                                }
                                data.advancefee = retObject1.ToArray();
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex.Message);
                                _logger.LogDebug(ex.Message);
                            }
                        }

                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                              from b in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                              from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                              from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                              from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                              where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && !saved_fma.Contains(e.FCMAS_Id) && e.FCMA_Id == c.FCMA_Id && ((e.FCMAS_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                              select new PreadmissionDTOs.com.vaps.College.Fees.Head_Installments_DTO
                                              {
                                                  FTI_Name = b.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();

                    }
                    else if (data.configset.Equals("T"))
                    {
                        _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Student_Fee_Det_term_all @p0,@p1,@p2,@p3,@p4,@p5,@p6", data.AMCST_Id, data.ASMAY_Id, data.FYP_ReceiptDate, data.MI_Id, data.configset, data.multiplegroups, data.UserId);

                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                              from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                              from b in _YearlyFeeGroupMappingContext.feeMTH
                                              from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                              from e in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                              from h in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                              from i in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                              where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(h.FCMAS_Id) && h.FCMA_Id == c.FCMA_Id && i.AMCST_Id == data.AMCST_Id && i.ASMAY_Id == data.ASMAY_Id && i.ACYST_ActiveFlag == 1 && c.AMCO_Id == i.AMCO_Id && c.AMB_Id == i.AMB_Id && c.FCMA_ActiveFlg == true && h.AMSE_Id == i.AMSE_Id && ((h.FCMAS_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                              select new PreadmissionDTOs.com.vaps.College.Fees.Head_Installments_DTO
                                              {
                                                  FTI_Name = d.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();

                    }
                }
                else if (data.filterinitialdata.Equals("Preadmission"))
                {
                    _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("Student_Fee_Det_Preadmission @p0,@p1,@p2,@p3,@p5", data.AMCST_Id, data.ASMAY_Id, data.FYP_ReceiptDate, data.MI_Id);
                }
                if (data.alldata.Length > 0)
                {
                    var count_res = retObject.Select(r => r.FMG_Id).GroupBy(id => id).OrderByDescending(id => id.Count()).Select(g => new CollegeFeeTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).First();

                    data.validationgroupid = count_res.FMG_Id;
                    data.validationgrougidcount = count_res.grp_count;
                    //MB
                    List<CollegeFeeTransactionDTO> fines_fma_ids = new List<CollegeFeeTransactionDTO>();
                    List<CollegeFeeTransactionDTO> fines_fma_idsadvan = new List<CollegeFeeTransactionDTO>();
                    var fine_amount = 0;
                    var fine_amountadv = 0;
                    //retObject.AddRange(retObject);
                    foreach (var x in retObject)
                    {
                        CollegeFeeTransactionDTO sew = new CollegeFeeTransactionDTO();
                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Sp_Calculate_Fine_CLG";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime)
                            {
                                Value = data.FYP_ReceiptDate
                            });

                            cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                               SqlDbType.BigInt)
                            {
                                Value = x.FCMAS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.AMCST_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Float)
                            {
                                Direction = ParameterDirection.Output
                            });
                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();
                            fine_amount += Convert.ToInt32(cmd.Parameters["@amt"].Value);

                            sew.FCMAS_Id = x.FCMAS_Id;
                            sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                            fines_fma_ids.Add(sew);
                        }
                    }
                    data.Fine_FCMAS_Ids = fines_fma_ids.Distinct().ToArray();

                    foreach (var x in retObject1)
                    {
                        CollegeFeeTransactionDTO sew1 = new CollegeFeeTransactionDTO();
                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Sp_Calculate_Fine_CLG";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@On_Date",
                                SqlDbType.DateTime)
                            {
                                Value = data.FYP_ReceiptDate
                            });

                            cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                               SqlDbType.BigInt)
                            {
                                Value = x.FCMAS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.AMCST_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@amt",
                SqlDbType.Float)
                            {
                                Direction = ParameterDirection.Output
                            });
                            cmd.Parameters.Add(new SqlParameter("@flgArr",
               SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();
                            fine_amountadv += Convert.ToInt32(cmd.Parameters["@amt"].Value);

                            sew1.FCMAS_Id = x.FCMAS_Id;
                            sew1.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                            fines_fma_idsadvan.Add(sew1);

                        }
                    }
                    data.Fine_FCMAS_IdsAdvance = fines_fma_idsadvan.Distinct().ToArray();
                    //MB
                }

                data.fetchmodeofpayment = (from a in _YearlyFeeGroupMappingContext.IVRM_ModeOfPayment
                                           where (a.MI_Id == data.MI_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               IVRMMOD_Id = a.IVRMMOD_Id,
                                               IVRMMOD_ModeOfPayment = a.IVRMMOD_ModeOfPayment,
                                               IVRMMOD_Flag = a.IVRMMOD_Flag,
                                               IVRMMOD_ModeOfPayment_Code = a.IVRMMOD_ModeOfPayment_Code
                                           }
).Distinct().ToArray();

                data.getpdcdetails = (from a in _YearlyFeeGroupMappingContext.Fee_College_Studentwise_PDCDMO
                                      from b in _YearlyFeeGroupMappingContext.Fee_Master_BankDMO
                                      where (a.MI_Id==b.MI_Id && a.FMBANK_Id==b.FMBANK_Id && a.MI_Id == data.MI_Id && a.FCSPDC_ActiveFlg==true && a.AMCST_Id == data.AMCST_Id && b.FMBANK_ActiveFlg == true)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FCSPDC_Id = a.FCSPDC_Id,
                                               FCSPDC_ChequeNo = a.FCSPDC_ChequeNo,
                                               FCSPDC_ChequeDate = a.FCSPDC_ChequeDate,
                                               FMBANK_BankName = b.FMBANK_BankName,
                                               FCSPDC_Amount = a.FCSPDC_Amount,
                                               FCSPDC_Status = a.FCSPDC_Status,
                                               FCSPDC_Narration = a.FCSPDC_Narration,
                                               FMBANK_Id=b.FMBANK_Id
                                           }
).Distinct().ToArray();

            }
            catch (Exception e)
            {
                data.validationvalue = "Contact Administrator";
            }
            return data;
        }
        public async Task<CollegeFeeTransactionDTO> savedetails(CollegeFeeTransactionDTO data)
        {
            try
            {
                var makerchecker = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).Select(d => d.FMC_MakerCheckerReqdFlg).FirstOrDefault();

                if (data.FYP_Id > 0)
                {
                    var result_obj = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Single(t => t.FYP_Id == data.FYP_Id);
                    result_obj.FYP_TransactionTypeFlag = data.FYP_PayModeType == "Single" ? data.FYP_Bank_Or_Cash : "";
                    result_obj.FYP_DOE = DateTime.Now;
                    result_obj.FYP_ReceiptDate = data.FYP_ReceiptDate;
                    result_obj.FYP_ReceiptNo = data.FYP_ReceiptNo;
                    result_obj.FYP_Remarks = data.FYP_Remarks;
                    result_obj.UpdatedDate = DateTime.Now;
                    _YearlyFeeGroupMappingContext.Update(result_obj);
                    if (data.FYP_PayModeType == "Single")
                    {
                        var result_obj1 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO.Single(t=>t.FYP_Id==data.FYP_Id);
                        result_obj1.FYPPM_TransactionTypeFlag = data.FYP_Bank_Or_Cash;
                        result_obj1.FYPPM_BankName = data.FYP_Bank_Or_Cash == "C" ? "" : data.FYP_Bank_Name;
                        result_obj1.FYPPM_DDChequeNo = data.FYP_Bank_Or_Cash == "C" ? "" : data.FYP_DD_Cheque_No;
                        result_obj1.FYPPM_DDChequeDate = data.FYP_Bank_Or_Cash == "C" ? data.FYP_ReceiptDate : data.FYP_DD_Cheque_Date;
                        result_obj1.FYPPM_ClearanceStatusFlag = (data.FYP_Bank_Or_Cash == "C" || data.FYP_Bank_Or_Cash == "S" || data.FYP_Bank_Or_Cash == "R") ? "1" : "0";
                        result_obj1.FYPPM_ClearanceDate = (data.FYP_Bank_Or_Cash == "C" || data.FYP_Bank_Or_Cash == "S"  || data.FYP_Bank_Or_Cash == "R") ? data.FYP_ReceiptDate : data.FYP_DD_Cheque_Date;
                        _YearlyFeeGroupMappingContext.Update(result_obj1);
                    }
                    else if (data.FYP_PayModeType == "Multiple")
                    {
                        foreach (var mode in data.Modes)
                        {
                            var result_obj2 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO.Single(t => t.FYP_Id == data.FYP_Id && t.FYPPM_TransactionTypeFlag== mode.FYPPM_TransactionTypeFlag);

                            result_obj2.FYPPM_BankName = mode.FYPPM_TransactionTypeFlag == "C" ? "" : mode.FYPPM_BankName;
                            result_obj2.FYPPM_DDChequeNo = mode.FYPPM_TransactionTypeFlag == "C" ? "" : mode.FYPPM_DDChequeNo;
                            result_obj2.FYPPM_DDChequeDate = mode.FYPPM_TransactionTypeFlag == "C" ? data.FYP_ReceiptDate : mode.FYPPM_DDChequeDate;
                            result_obj2.FYPPM_ClearanceStatusFlag = (mode.FYPPM_TransactionTypeFlag == "C" || mode.FYPPM_TransactionTypeFlag == "S" || mode.FYPPM_TransactionTypeFlag == "R") ? "1" : "0";
                            result_obj2.FYPPM_ClearanceDate = (mode.FYPPM_TransactionTypeFlag == "C" || mode.FYPPM_TransactionTypeFlag == "S" || mode.FYPPM_TransactionTypeFlag == "R") ? data.FYP_ReceiptDate : mode.FYPPM_DDChequeDate;
                            _YearlyFeeGroupMappingContext.Update(result_obj2);
                        }
                    }
                    var ResultCount = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (ResultCount >= 1)
                    {
                        data.returnval = "true";
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }
                else
                {
                    if (data.auto_receipt_flag == 1)
                    {
                        if (data.FYP_ReceiptNo == null || data.FYP_ReceiptNo == "")
                        {
                            data.temp_head_list = data.savetmpdata;
                            CollegeFeeTransactionDTO new_obj = get_grp_reptno(data);
                            data.FYP_ReceiptNo = new_obj.FYP_ReceiptNo;
                        }
                    }
                    Fee_Y_PaymentDMO obj1 = new Fee_Y_PaymentDMO();
                    obj1.MI_Id = data.MI_Id;
                    obj1.ASMAY_Id = data.ASMAY_Id;
                    obj1.FYP_Currency = "1";
                    obj1.FYP_DOE = DateTime.Now;
                    obj1.FYP_ReceiptDate = data.FYP_ReceiptDate;
                    obj1.FYP_ReceiptNo = data.FYP_ReceiptNo;
                    obj1.FYP_PayModeType = data.FYP_PayModeType;
                    obj1.FYP_TransactionTypeFlag = data.FYP_PayModeType == "Single" ? data.FYP_Bank_Or_Cash : "";
                    obj1.FYP_TotalPaidAmount = data.FYP_TotalPaidAmount;
                    obj1.FYP_ChallanStatusFlag = "";
                    obj1.FYP_ChallanNo = "";
                    obj1.FYP_Transaction_Id = "";
                    obj1.FYP_TotalFineAmount = data.FYP_TotalFineAmount;
                    obj1.FYP_TotalRebateAmount = 0;
                    obj1.FYP_Remarks = data.FYP_Remarks;
                    obj1.FYP_ChequeBounceFlag = "CL";
                    obj1.FYP_ActiveFlag = true;
                    obj1.User_Id = data.UserId;
                    obj1.CreatedDate = DateTime.Now;
                    obj1.UpdatedDate = DateTime.Now;
                    if (makerchecker == true)
                    {
                        obj1.FYP_ApprovedFlg = false;
                    }
                    else if (makerchecker == false)
                    {
                        obj1.FYP_ApprovedFlg = true;
                    }
                    _YearlyFeeGroupMappingContext.Add(obj1);

                    if (data.FYP_PayModeType == "Single")
                    {
                        Fee_Y_Payment_PaymentModeDMO obj2 = new Fee_Y_Payment_PaymentModeDMO();
                        obj2.FYP_Id = obj1.FYP_Id;
                        obj2.FYPPM_TransactionTypeFlag = data.FYP_Bank_Or_Cash;
                        obj2.FYPPM_TotalPaidAmount = data.FYP_TotalPaidAmount;
                        obj2.FYPPM_LedgerId = 0;
                        obj2.FYPPM_BankName = data.FYP_Bank_Or_Cash == "C" ? "" : data.FYP_Bank_Name;
                        obj2.FYPPM_DDChequeNo = data.FYP_Bank_Or_Cash == "C" ? "" : data.FYP_DD_Cheque_No;
                        obj2.FYPPM_DDChequeDate = data.FYP_Bank_Or_Cash == "C" ? data.FYP_ReceiptDate : data.FYP_DD_Cheque_Date;
                        obj2.FYPPM_Transaction_Id = "";
                        obj2.FYPPM_PaymentReference_Id = "";
                        obj2.FYPPM_ClearanceStatusFlag = (data.FYP_Bank_Or_Cash == "C" || data.FYP_Bank_Or_Cash == "S" || data.FYP_Bank_Or_Cash == "R" || data.FYP_Bank_Or_Cash == "U") ? "1" : "0";
                        obj2.FYPPM_ClearanceDate = (data.FYP_Bank_Or_Cash == "C" || data.FYP_Bank_Or_Cash == "S"|| data.FYP_Bank_Or_Cash == "R" || data.FYP_Bank_Or_Cash == "U") ? data.FYP_ReceiptDate : data.FYPPM_ClearanceDate;
                        _YearlyFeeGroupMappingContext.Add(obj2);
                    }
                    else if (data.FYP_PayModeType == "Multiple")
                    {
                        foreach (var mode in data.Modes)
                        {
                            Fee_Y_Payment_PaymentModeDMO obj2 = new Fee_Y_Payment_PaymentModeDMO();
                            obj2.FYP_Id = obj1.FYP_Id;
                            obj2.FYPPM_TransactionTypeFlag = mode.FYPPM_TransactionTypeFlag;
                            obj2.FYPPM_TotalPaidAmount = mode.FYPPM_TotalPaidAmount;
                            obj2.FYPPM_LedgerId = 0;
                            obj2.FYPPM_BankName = mode.FYPPM_TransactionTypeFlag == "C" ? "" : mode.FYPPM_BankName;
                            obj2.FYPPM_DDChequeNo = mode.FYPPM_TransactionTypeFlag == "C" ? "" : mode.FYPPM_DDChequeNo;
                            obj2.FYPPM_DDChequeDate = mode.FYPPM_TransactionTypeFlag == "C" ? data.FYP_ReceiptDate : mode.FYPPM_DDChequeDate;
                            obj2.FYPPM_Transaction_Id = "";
                            obj2.FYPPM_PaymentReference_Id = "";
                            obj2.FYPPM_ClearanceStatusFlag = (mode.FYPPM_TransactionTypeFlag == "C" || mode.FYPPM_TransactionTypeFlag == "S" || mode.FYPPM_TransactionTypeFlag == "R" || mode.FYPPM_TransactionTypeFlag == "U") ? "1" : "0";
                            obj2.FYPPM_ClearanceDate = (mode.FYPPM_TransactionTypeFlag == "C" || mode.FYPPM_TransactionTypeFlag == "S" || mode.FYPPM_TransactionTypeFlag == "R" || mode.FYPPM_TransactionTypeFlag == "U") ? data.FYP_ReceiptDate : mode.FYPPM_DDChequeDate;
                            _YearlyFeeGroupMappingContext.Add(obj2);
                        }
                    }
                    Fee_Y_Payment_College_StudentDMO obj3 = new Fee_Y_Payment_College_StudentDMO();
                    obj3.FYP_Id = obj1.FYP_Id;
                    obj3.AMCST_Id = data.AMCST_Id;
                    obj3.ASMAY_Id = data.ASMAY_Id;
                    obj3.FYPCS_TotalPaidAmount = data.FYP_TotalPaidAmount;
                    obj3.FYPCS_TotalWaivedAmount = data.FYP_Tot_Waived_Amt;
                    obj3.FYPCS_TotalConcessionAmount = data.FYP_Tot_Concession_Amt;
                    obj3.FYPCS_TotalFineAmount = data.FYP_TotalFineAmount;
                    _YearlyFeeGroupMappingContext.Add(obj3);

                    foreach (var x in data.savetmpdata)
                    {
                        Fee_T_College_PaymentDMO obj4 = new Fee_T_College_PaymentDMO();
                        obj4.FYP_Id = obj1.FYP_Id;
                        obj4.FCMAS_Id = x.FCMAS_Id;
                        obj4.FTCP_PaidAmount = x.FCSS_ToBePaid;
                        obj4.FTCP_WaivedAmount = x.FCSS_WaivedAmount;
                        obj4.FTCP_ConcessionAmount = x.FCSS_ConcessionAmount;
                        obj4.FTCP_FineAmount = x.FCSS_FineAmount;
                        obj4.FTCP_RebateAmount = 0;
                        obj4.FTCP_Remarks = data.FYP_Remarks;
                        _YearlyFeeGroupMappingContext.Add(obj4);


                        var obj_status = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.AMCST_Id && t.FMH_Id == x.FMH_Id && t.FTI_Id == x.FTI_Id && t.FMG_Id == x.FMG_Id && t.FCMAS_Id == x.FCMAS_Id && t.FCSS_ActiveFlag == true);

                        if (obj_status.FCSS_PaidAmount == 0)
                        {
                            obj_status.FCSS_PaidAmount = x.FCSS_ToBePaid;
                            if(obj_status.FCSS_PaidAmount<= obj_status.FCSS_ToBePaid)
                            {
                                obj_status.FCSS_ToBePaid = obj_status.FCSS_ToBePaid - obj_status.FCSS_PaidAmount;
                            }
                            else
                            {
                                obj_status.FCSS_ToBePaid = 0;
                            }
                            
                        }
                        else if (obj_status.FCSS_PaidAmount > 0)
                        {
                            obj_status.FCSS_PaidAmount = obj_status.FCSS_PaidAmount + x.FCSS_ToBePaid;
                            if(obj_status.FCSS_ToBePaid>0)
                            {
                                obj_status.FCSS_ToBePaid = obj_status.FCSS_ToBePaid - x.FCSS_ToBePaid;
                            }
                        }
                        
                         //added on 14-12-2020
                        var fineheadss = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                          from b in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                          where (a.MI_Id == data.MI_Id && a.FCMAS_Id == x.FCMAS_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.AMCST_Id == data.AMCST_Id && a.FMH_Id == b.FMH_Id)
                                          select new CollegeFeeTransactionDTO
                                          {
                                              FMH_Id = a.FMH_Id,
                                          }
                           ).Distinct().Take(1);

                        if (fineheadss.Count() > 0)
                        {
                            //obj_status.FCSS_FineAmount = obj_status.FCSS_FineAmount + obj_status.FCSS_PaidAmount;
                        }
                        //added on 14-12-2020
                        
                        _YearlyFeeGroupMappingContext.Update(obj_status);

                    }

                    foreach (var x in data.saveadvancedata)
                    {
                        Fee_T_College_PaymentDMO obj4 = new Fee_T_College_PaymentDMO();
                        obj4.FYP_Id = obj1.FYP_Id;
                        obj4.FCMAS_Id = x.FCMAS_Id;
                        obj4.FTCP_PaidAmount = x.FCSS_ToBePaid;
                        obj4.FTCP_WaivedAmount = x.FCSS_WaivedAmount;
                        obj4.FTCP_ConcessionAmount = x.FCSS_ConcessionAmount;
                        obj4.FTCP_FineAmount = x.FCSS_FineAmount;
                        obj4.FTCP_RebateAmount = 0;
                        obj4.FTCP_Remarks = data.FYP_Remarks;

                        _YearlyFeeGroupMappingContext.Add(obj4);
                    }


                    data.returnval = "false";
                    var contactexisttransaction = 0;
                    using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    {
                        try
                        {
                            contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                            dbCtxTxn.Commit();
                              data.returnval = "true";
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            dbCtxTxn.Rollback();
                            data.returnval = "false";
                            data.displaymessage = "Not Saved";
                        }
                    }

                    var template = _context.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "FeeReceiptPreviousDate" && e.ISES_MailActiveFlag == true).ToList();

                    Email Email = new Email(_context);
                    if (template.Count > 0 && data.FYP_ReceiptDate.ToString("dd-MM-yyyy")!=DateTime.Now.ToString("dd-MM-yyyy"))
                    {
                        string m = Email.sendmail(data.MI_Id, template[0].ISES_MailCCId, "FeeReceiptPreviousDate", obj1.FYP_Id);
                    }
                }
            }
            catch (Exception ee)
            {
                data.returnval = "false";
                data.displaymessage = "Not Saved";
                Console.WriteLine(ee.Message);
                _logger.LogError("Error in " + data.MI_Id + " - " + ee.InnerException);
            }

            return data;
        }
        public CollegeFeeTransactionDTO delrec(CollegeFeeTransactionDTO data)
        {
            try
            {
                var obj_status_stf = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).FirstOrDefault();
                obj_status_stf.FYP_Remarks = obj_status_stf.FYP_Remarks + " - " + data.FYP_Remarks;
                _YearlyFeeGroupMappingContext.Update(obj_status_stf);
                _YearlyFeeGroupMappingContext.SaveChanges();

                var lorg1 = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Single(t => t.FYP_Id == data.FYP_Id);
                var lorg = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();
                var lorg2 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg3 = _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                foreach (var x in lorg3)
                {
                    var FCMAS_Id = x.FCMAS_Id;
                    var FSSST_PaidAmount = x.FTCP_PaidAmount;

                    var checkadvance = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Where(t => t.MI_Id == data.MI_Id && t.FCMAS_Id == FCMAS_Id).FirstOrDefault();

                    if (checkadvance != null){

                        var lorg4 = _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO.Single(t => t.FCMAS_Id == FCMAS_Id && t.MI_Id == lorg1.MI_Id && t.ASMAY_Id == lorg1.ASMAY_Id && t.AMCST_Id == lorg2.AMCST_Id);

                        lorg4.FCSS_PaidAmount = (lorg4.FCSS_PaidAmount) - Convert.ToInt64(x.FTCP_PaidAmount);

                        if (lorg4.FCSS_NetAmount != 0)
                        {
                            lorg4.FCSS_ToBePaid = lorg4.FCSS_ToBePaid + (Convert.ToInt64(x.FTCP_PaidAmount) - lorg4.FCSS_OBArrearAmount);
                        }
                        else
                        {
                            lorg4.FCSS_ToBePaid = 0;
                        }

                        _YearlyFeeGroupMappingContext.Update(lorg4);
                    }
                   
                }

                if (lorg3.Any())
                {
                    for (int i = 0; lorg3.Count > i; i++)
                    {
                        _YearlyFeeGroupMappingContext.Remove(lorg3.ElementAt(i));
                    }
                }
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _YearlyFeeGroupMappingContext.Remove(lorg.ElementAt(i));
                    }
                }

                _YearlyFeeGroupMappingContext.Remove(lorg2);
                _YearlyFeeGroupMappingContext.Remove(lorg1);


                var contactexisttransaction = 0;
                using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                        dbCtxTxn.Commit();
                        data.returnval = "true";
                    }
                    catch (Exception ex)
                    {
                        dbCtxTxn.Rollback();
                        data.returnval = "Transaction is not Processed Correctly.Kindly contact Administrator!!!!!";
                    }
                }
            }
            catch (Exception ee)
            {
                data.returnval = "Error";
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO Printterm(CollegeFeeTransactionDTO data)
        {
            try
            {
                data.username = _YearlyFeeGroupMappingContext.applicationUser.Where(d => d.Id == data.UserId).Select(t=>t.NormalizedUserName).FirstOrDefault();

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fetch_Final_Level_Approval_Name";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.VarChar) { Value = data.AMCST_Id });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.FYP_Id });
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
                            data.Approvedbyname = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                          from h in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                          from e in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                          from f in _YearlyFeeGroupMappingContext.feespecialHead
                                          from g in _YearlyFeeGroupMappingContext.feeSGGG
                                          where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == h.FCMAS_Id && d.FCMA_Id == h.FCMA_Id && d.FMH_Id == e.FMH_Id && b.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id)
                                          select new CollegeFeeTransactionDTO
                                          {
                                              FMH_FeeName = f.FMSFH_Name,
                                              FMH_Id = e.FMH_Id,
                                              FTCP_PaidAmount = c.FTCP_PaidAmount,
                                              FTCP_ConcessionAmount = c.FTCP_ConcessionAmount,
                                              FTCP_FineAmount = c.FTCP_FineAmount,
                                              FTI_Id = d.FTI_Id,
                                              FCSS_TotalCharges = Convert.ToInt64(h.FCMAS_Amount)
                                          }
              ).ToArray();

                List<CollegeFeeTransactionDTO> findfmgid = new List<CollegeFeeTransactionDTO>();
                findfmgid = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                             from b in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                 where (a.FCMAS_Id==b.FCMAS_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                 select new CollegeFeeTransactionDTO
                                 {
                                     FMG_Id = b.FMG_Id,
                                     FMH_Id = b.FMH_Id,
                                     FCMAS_Id= b.FCMAS_Id
                                 }).Distinct().ToList();

                List<long> fcmasids = new List<long>();
                foreach (var item in findfmgid)
                {
                    fcmasids.Add(item.FCMAS_Id);
                }

                data.termremarks = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                    from b in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                    from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                    from c in _YearlyFeeGroupMappingContext.feeMTH
                                    from d in _YearlyFeeGroupMappingContext.feeTr
                                    where c.FMT_Id == d.FMT_Id && a.FCMAS_Id == e.FCMAS_Id && e.FCMA_Id == b.FCMA_Id && b.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id
                                    select new CollegeFeeTransactionDTO
                                    {
                                        FMT_Name = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                           from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FYP_Id==a.FYP_Id && a.FYP_Id==c.FYP_Id && b.FYP_Id==c.FYP_Id
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FTCP_PaidAmount = a.FYP_TotalPaidAmount,
                                               FTCP_ConcessionAmount = b.FYPCS_TotalConcessionAmount,
                                               FYP_ReceiptDate = a.FYP_ReceiptDate,
                                               FYP_Remarks = a.FYP_Remarks,

                                               FYPPM_BankName=c.FYPPM_BankName,
                                               FYPPM_DDChequeDate=c.FYPPM_DDChequeDate,
                                               FYPPM_DDChequeNo=c.FYPPM_DDChequeNo,
                                               FYP_TransactionTypeFlag=c.FYPPM_TransactionTypeFlag,
                                               FYPPM_Transaction_Id=c.FYPPM_Transaction_Id,
                                               FYPPM_PaymentReference_Id=c.FYPPM_PaymentReference_Id,
                                               FYPPM_ClearanceDate=c.FYPPM_ClearanceDate
                                           }
               ).Distinct().ToArray();


                // data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                //                                from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                //                                from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                //                                from l in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                //                                from d in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                //                                from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                //                                from f in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                //                                from g in _YearlyFeeGroupMappingContext.MasterCourseDMO
                //                                from h in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                //                                from k in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                //                                from i in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                //                                from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                //                                from m in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                //                                where ( a.FYP_Id == b.FYP_Id && a.FCMAS_Id == l.FCMAS_Id && c.FCMA_Id==l.FCMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMCST_Id == j.AMCST_Id && f.AMCO_Id == g.AMCO_Id && f.AMB_Id == h.AMB_Id && f.AMSE_Id==k.AMSE_Id && f.ASMAY_Id==b.ASMAY_Id && i.AMCST_Id == j.AMCST_Id && m.AMCST_Id==f.AMCST_Id && f.ASMAY_Id==m.ASMAY_Id && a.FCMAS_Id==m.FCMAS_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMCST_Id == data.AMCST_Id && b.FYP_Id == data.FYP_Id && f.ACYST_ActiveFlag==1)
                //                                select new CollegeFeeTransactionDTO
                //                                {
                //                                    AMCST_Id = f.AMCST_Id,
                //                                    AMCST_FirstName = i.AMCST_FirstName,
                //                                    AMCST_MiddleName = i.AMCST_MiddleName,
                //                                    AMCST_LastName = i.AMCST_LastName,
                //                                    FMH_Id = d.FMH_Id,
                //                                    FMH_FeeName = d.FMH_FeeName,
                //                                    FTI_Name = e.FTI_Name,
                //                                    FTI_Id = e.FTI_Id,
                //                                    FYP_ReceiptNo = b.FYP_ReceiptNo,
                //                                    FTCP_PaidAmount = a.FTCP_PaidAmount,
                //                                    FTCP_ConcessionAmount = a.FTCP_ConcessionAmount,
                //                                    FTCP_FineAmount = a.FTCP_FineAmount,
                //                                    FYP_ReceiptDate = b.FYP_ReceiptDate,
                //                                    AMCO_CourseName = g.AMCO_CourseName,
                //                                    AMB_BranchName = h.AMB_BranchName,
                //                                    AMSE_SEMName = k.AMSE_SEMName,
                //                                    ACYST_RollNo = f.ACYST_RollNo,
                //                                    AMCST_AdmNo = i.AMCST_AdmNo,
                //                                    AMCST_FatherName = i.AMCST_FatherName,
                //                                    AMCST_MotherName = i.AMCST_MotherName,
                //                                    FYP_Remarks = b.FYP_Remarks,
                //                                    AMCST_RegistrationNo = i.AMCST_RegistrationNo,
                //                                    FCSS_TotalCharges = Convert.ToInt64(l.FCMAS_Amount),
                //                                }
                //).Distinct().ToArray(); 

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Student_DueDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.BigInt) { Value = data.AMCST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FYP_Id", SqlDbType.BigInt) { Value = data.FYP_Id });
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
                            data.fillstudentviewdetails = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.fillstudentviewdetailsadvance = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                               from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                               from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                               from e in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                               from f in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                               from g in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                where (a.FYP_Id==b.FYP_Id && c.FMH_Id==f.FMH_Id && d.FTI_Id==f.FTI_Id && e.AMCST_Id==data.AMCST_Id && e.FYP_Id==a.FYP_Id && a.FCMAS_Id==g.FCMAS_Id && f.FCMA_Id==g.FCMAS_Id && b.ASMAY_Id==data.ASMAY_Id && b.FYP_Id == data.FYP_Id && !fcmasids.Contains(a.FCMAS_Id))
                                               select new CollegeFeeTransactionDTO
                                               {
                                                   AMCST_Id = e.AMCST_Id,
                                                   FMH_Id = c.FMH_Id,
                                                   FMH_FeeName = c.FMH_FeeName,
                                                   FTI_Name = d.FTI_Name,
                                                   FTI_Id = d.FTI_Id,
                                                   FYP_ReceiptNo = b.FYP_ReceiptNo,
                                                   FTCP_PaidAmount = a.FTCP_PaidAmount,
                                                   FTCP_ConcessionAmount = a.FTCP_ConcessionAmount,
                                                   FTCP_FineAmount = a.FTCP_FineAmount,
                                                   FYP_ReceiptDate = b.FYP_ReceiptDate,
                                                   FYP_Remarks = b.FYP_Remarks,
                                                   FCSS_TotalCharges = Convert.ToInt64(g.FCMAS_Amount),
                                               }
              ).Distinct().ToArray();

                data.filltotaldetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                         from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                         where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id==a.FYP_Id)
                                         select new CollegeFeeTransactionDTO
                                         {
                                             FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                             FYP_Tot_Concession_Amt = b.FYPCS_TotalConcessionAmount,
                                             FYP_TotalFineAmount = a.FYP_TotalFineAmount,
                                         }
              ).Distinct().ToArray();



                //to find next due amount
                var feeterm1 = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                               from b in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               from f in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                               from g in _YearlyFeeGroupMappingContext.feeTr
                               where (e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FCMAS_Id == d.FCMAS_Id && d.AMCST_Id == c.AMCST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.AMCST_Id == data.AMCST_Id && a.FYP_Id == data.FYP_Id && d.FCSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                               select new FeeTermDMO
                               {
                                   FMT_Id = e.FMT_Id,
                                   FMT_Order = g.FMT_Order,
                                   FMT_Year = g.FMT_Year
                               }

                 ).Distinct().ToArray();
                var feeterm = feeterm1.OrderBy(t => t.FMT_Order).ToArray();
                int fmtorder_end = 0;
                string fmt_id_int = "0", fmt_id_end = "0", fmt_id_end_year = "0";
                fmt_id_int = feeterm[0].FMT_Id.ToString();
                fmt_id_end = feeterm[feeterm.Length - 1].FMT_Id.ToString();

                fmtorder_end = Convert.ToInt32(feeterm[feeterm.Length - 1].FMT_Order);
                fmt_id_end_year = (feeterm[feeterm.Length - 1].FMT_Year).ToString();

                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order == fmtorder_end + 1).Select(t => t.FMT_Id);

                List<long> termmidsnew = new List<long>();
                foreach (var item in feeterm)
                {
                    termmidsnew.Add(item.FMT_Id);
                }

                List<CollegeFeeTransactionDTO> temp_group_head = new List<CollegeFeeTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                   from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                   from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                   where (a.AMCST_Id == data.AMCST_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == e.FCMAS_Id && e.FCMA_Id==d.FCMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new CollegeFeeTransactionDTO
                                   {
                                       FMG_Id = d.FMG_Id,
                                       FMH_Id = d.FMH_Id,
                                       FTI_Id = d.FTI_Id
                                   }

                           ).Distinct().ToList();

                List<long> grp_ids = new List<long>();
                List<long> head_ids = new List<long>();
                List<long> inst_ids = new List<long>();
                foreach (var item in temp_group_head)
                {
                    grp_ids.Add(item.FMG_Id);
                    head_ids.Add(item.FMH_Id);
                    inst_ids.Add(item.FTI_Id);
                }

                List<CollegeFeeTransactionDTO> fordate = new List<CollegeFeeTransactionDTO>();
                List<CollegeFeeTransactionDTO> fordateinfyp = new List<CollegeFeeTransactionDTO>();

                var nextduedate = "0";

                if (term_ids.Count() > 0)
                {
                    nextduedate = (Convert.ToInt32(fmt_id_end) + 1).ToString();

                    fordate = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                               from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                               where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.AMCST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && (d.FCSS_ToBePaid > 0))
                               select new CollegeFeeTransactionDTO
                               {
                                   date = f.FCTDD_Day,
                                   month = f.FCTDD_Month,
                               }
                        ).Distinct().ToList();

                }
                else
                {
                    nextduedate = (Convert.ToInt32(fmt_id_end)).ToString();

                    fordate = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                               from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                               where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.AMCST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id) && (d.FCSS_ToBePaid > 0))
                               select new CollegeFeeTransactionDTO
                               {
                                   date = f.FCTDD_Day,
                                   month = f.FCTDD_Month,
                               }
                        ).Distinct().ToList();
                }

                termmidsnew.Add(Convert.ToInt32(nextduedate));

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                                where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.AMCST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new CollegeFeeTransactionDTO
                                {
                                    month = f.FCTDD_Month,
                                }
                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (CollegeFeeTransactionDTO item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (CollegeFeeTransactionDTO itemperiod in fordateinfyp)
                {
                    startperiod.Add(Convert.ToInt32(itemperiod.month));
                }

                foreach (var item in months)
                {
                    if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                    {
                        months1.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year - 1;
                        data.year = nextyr.ToString();
                    }
                    else
                    {
                        months2.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        data.year = curyear.Year.ToString();
                    }
                }

                string maxmonth = "", monthnameinitial = "", monthnameend = "";
                if (months1.Count() > 0)
                {
                    data.month = months1.Min().ToString();
                    maxmonth = months1.Max().ToString();
                    if (startperiod.Count >= 4)
                    {
                        monthnameinitial = startperiod.Min().ToString();
                        maxmonth = startperiod.Max().ToString();
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                    else
                    {
                        monthnameinitial = startperiod.Max().ToString();
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;
                }
                else if (months2.Count() > 0)
                {
                    data.month = months2.Min().ToString();
                    maxmonth = months2.Max().ToString();

                    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(data.month));
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));


                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;

                }
                for (int i = 0; i < months.Count(); i++)
                {
                    if (Convert.ToInt32(data.month) == months[i])
                    {
                        data.date = dates[i].ToString();
                    }
                }

                if (months.Count == 0)
                {
                    foreach (var item in startperiod)
                    {
                        if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                        {
                            monthnameinitial = startperiod.Min().ToString();
                            var curyear = DateTime.Now;
                            var nextyr = curyear.Year - 1;
                            data.year = curyear.Year.ToString();
                        }
                        else
                        {
                            maxmonth = startperiod.Max().ToString();
                            var curyear = DateTime.Now;
                            data.year = curyear.Year.ToString();
                        }
                    }
                    if (monthnameinitial != "")
                    {
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                    }

                    if (monthnameend != "")
                    {
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                    else
                    {
                        maxmonth = startperiod.Max().ToString();
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                }

                var termperiodlistint = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                var termperiodlistend = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                monthnameinitial = termperiodlistint[0].FromMonth.ToString();
                monthnameend = termperiodlistend[0].ToMonth.ToString();

                string yeardisplay = "0";
                yeardisplay = fmt_id_end_year;

                data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;

                //new one
                data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && grp_ids.Contains(a.FMG_Id) && termmidsnew.Contains(b.FMT_Id) && c.FMH_Flag != "E")
                                  select new CollegeFeeTransactionDTO
                                  {
                                      FCSS_ToBePaid = a.FCSS_ToBePaid
                                  }
          ).Sum(t => t.FCSS_ToBePaid);

             

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO Printinstallment(CollegeFeeTransactionDTO data)
        {
            try
            {
                string yeardisplay = "0";

                data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                          from e in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                          from f in _YearlyFeeGroupMappingContext.feespecialHead
                                          from g in _YearlyFeeGroupMappingContext.feeSGGG
                                          from h in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                          where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == h.FCMAS_Id && h.FCMA_Id==d.FCMA_Id && d.FMH_Id == e.FMH_Id && b.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id)
                                          select new CollegeFeeTransactionDTO
                                          {
                                              FMH_FeeName = f.FMSFH_Name,
                                              FMH_Id = e.FMH_Id,
                                              FTCP_PaidAmount = c.FTCP_PaidAmount,
                                              FTCP_ConcessionAmount = c.FTCP_ConcessionAmount,
                                              FTCP_FineAmount = c.FTCP_FineAmount,
                                              FTI_Id = d.FTI_Id
                                          }
              ).ToArray();

                var findfmgid = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                 from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                 from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == e.FCMAS_Id && d.FCMA_Id==e.FCMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                 select new CollegeFeeTransactionDTO
                                 {
                                     FMG_Id = d.FMG_Id,
                                     FMH_Id = d.FMH_Id
                                 }).Distinct().ToArray();

                data.termremarks = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                    from b in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                    from c in _YearlyFeeGroupMappingContext.feeMTH
                                    from d in _YearlyFeeGroupMappingContext.feeTr
                                    from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                    where c.FMT_Id == d.FMT_Id && a.FCMAS_Id == e.FCMAS_Id && e.FCMA_Id==b.FCMA_Id && b.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id  
                                    select new CollegeFeeTransactionDTO
                                    {
                                        FMT_Name = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FYP_Id==a.FYP_Id
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FTCP_PaidAmount = a.FYP_TotalPaidAmount,
                                               FTCP_ConcessionAmount = b.FYPCS_TotalConcessionAmount,
                                               FYP_ReceiptDate = a.FYP_ReceiptDate,
                                               FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                               FYP_Remarks = a.FYP_Remarks,
                                           }
               ).Distinct().ToArray();

                List<CollegeFeeTransactionDTO> result = new List<CollegeFeeTransactionDTO>();

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "installment_transaction_details_CLG";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Amcst_id", SqlDbType.VarChar) { Value = data.AMCST_Id });
                    cmd.Parameters.Add(new SqlParameter("@fyp_id", SqlDbType.VarChar) { Value = data.FYP_Id });
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
                            data.fillstudentviewdetails = retObject.Distinct().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                data.filltotaldetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                         from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                         where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id==a.FYP_Id)
                                         select new CollegeFeeTransactionDTO
                                         {
                                             FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                             FYP_Tot_Concession_Amt = b.FYPCS_TotalConcessionAmount,
                                             FYP_TotalFineAmount = a.FYP_TotalFineAmount,
                                         }
              ).Distinct().ToArray();

                //to find next due amount

                var feeterm1 = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                               from b in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               from f in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                               from g in _YearlyFeeGroupMappingContext.feeTr
                               where (e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FCMAS_Id == d.FCMAS_Id && d.AMCST_Id == c.AMCST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.AMCST_Id == data.AMCST_Id && a.FYP_Id == data.FYP_Id && d.FCSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                               select new FeeTermDMO
                               {
                                   FMT_Id = g.FMT_Id,
                                   FMT_Order = g.FMT_Order,
                                   FMT_Year = g.FMT_Year
                               }

               ).Distinct().ToArray();
                var feeterm = feeterm1.OrderBy(t => t.FMT_Order).ToArray();

                int fmtorder_end = 0;
                string fmt_id_int = "0", fmt_id_end = "0", fmt_id_end_year = "0"; ;
                fmt_id_int = feeterm[0].FMT_Id.ToString();
                fmt_id_end = feeterm[feeterm.Length - 1].FMT_Id.ToString();

                fmtorder_end = Convert.ToInt32(feeterm[feeterm.Length - 1].FMT_Order);
                fmt_id_end_year = (feeterm[feeterm.Length - 1].FMT_Year).ToString();

                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order == fmtorder_end + 1).Select(t => t.FMT_Id);

                List<long> termmidsnew = new List<long>();
                foreach (var item in feeterm)
                {
                    termmidsnew.Add(item.FMT_Id);
                }


                List<CollegeFeeTransactionDTO> temp_group_head = new List<CollegeFeeTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                   from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                   from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                   where (a.AMCST_Id == data.AMCST_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == e.FCMAS_Id && e.FCMA_Id==d.FCMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new CollegeFeeTransactionDTO
                                   {

                                       FMG_Id = d.FMG_Id,
                                       FMH_Id = d.FMH_Id,
                                       FTI_Id = d.FTI_Id

                                   }

                           ).Distinct().ToList();
                List<long> grp_ids = new List<long>();
                List<long> head_ids = new List<long>();
                List<long> inst_ids = new List<long>();
                foreach (var item in temp_group_head)
                {
                    grp_ids.Add(item.FMG_Id);
                    head_ids.Add(item.FMH_Id);
                    inst_ids.Add(item.FTI_Id);
                }

                List<CollegeFeeTransactionDTO> fordate = new List<CollegeFeeTransactionDTO>();
                List<CollegeFeeTransactionDTO> fordateinfyp = new List<CollegeFeeTransactionDTO>();

                fordate = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                           from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                           where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.AMCST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && d.User_Id == data.UserId)
                           select new CollegeFeeTransactionDTO
                           {
                               date = f.FCTDD_Day,
                               month = f.FCTDD_Month,
                           }

                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                                where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.AMCST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new CollegeFeeTransactionDTO
                                {
                                    month = f.FCTDD_Month,
                                }

                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (CollegeFeeTransactionDTO item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (CollegeFeeTransactionDTO itemperiod in fordateinfyp)
                {
                    startperiod.Add(Convert.ToInt32(itemperiod.month));
                }


                foreach (var item in months)
                {
                    if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                    {
                        months1.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year - 1;
                        data.year = nextyr.ToString();
                    }
                    else
                    {
                        months2.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        // var nextyr = curyear.Year - 1;
                        data.year = curyear.Year.ToString();
                    }
                }

                string maxmonth = "", monthnameinitial = "", monthnameend = "";
                if (months1.Count() > 0)
                {
                    data.month = months1.Min().ToString();
                    maxmonth = (Convert.ToInt32(data.month) - 1).ToString();

                    if (startperiod.Count >= 4)
                    {
                        monthnameinitial = startperiod.Min().ToString();
                        maxmonth = startperiod.Max().ToString();
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                    else
                    {
                        monthnameinitial = startperiod.Max().ToString();
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;
                }
                else if (months2.Count() > 0)
                {
                    data.month = months2.Min().ToString();
                    maxmonth = months2.Max().ToString();

                    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(data.month));
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));


                    if (monthnameend == "Jan" || monthnameend == "Feb" || monthnameend == "Mar")
                    {
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year;
                        yeardisplay = nextyr.ToString();
                    }
                    else
                    {
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year - 1;
                        yeardisplay = nextyr.ToString();
                    }

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;

                }
                for (int i = 0; i < months.Count(); i++)
                {
                    if (Convert.ToInt32(data.month) == months[i])
                    {
                        data.date = dates[i].ToString();
                    }
                }

                if (months.Count == 0)
                {
                    foreach (var item in startperiod)
                    {
                        if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                        {
                            months1.Add(Convert.ToInt32(item));
                            var curyear = DateTime.Now;
                            var nextyr = curyear.Year - 1;
                            data.year = nextyr.ToString();
                        }
                        else
                        {
                            months2.Add(Convert.ToInt32(item));
                            var curyear = DateTime.Now;
                            // var nextyr = curyear.Year - 1;
                            data.year = curyear.ToString();
                        }
                    }
                    if (monthnameinitial != "")
                    {
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                    }

                    if (monthnameend != "")
                    {
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                    else
                    {
                        maxmonth = startperiod.Max().ToString();
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }

                    if (monthnameend == "Jan" || monthnameend == "Feb" || monthnameend == "Mar")
                    {
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year;
                        yeardisplay = nextyr.ToString();
                    }
                    else
                    {
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year - 1;
                        yeardisplay = nextyr.ToString();


                    }

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;
                }


                var termperiodlistint = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                var termperiodlistend = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                monthnameinitial = termperiodlistint[0].FromMonth.ToString();
                monthnameend = termperiodlistend[0].ToMonth.ToString();

                if (monthnameend == "Jan" || monthnameend == "Feb" || monthnameend == "Mar")
                {
                    var curyear = DateTime.Now;
                    var nextyr = curyear.Year;
                    yeardisplay = nextyr.ToString();
                }
                else
                {
                    var curyear = DateTime.Now;
                    var nextyr = curyear.Year - 1;
                    yeardisplay = nextyr.ToString();
                }

                yeardisplay = fmt_id_end_year;
                data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;

                data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && grp_ids.Contains(a.FMG_Id) && termmidsnew.Contains(b.FMT_Id) && c.FMH_Flag != "E")
                                  select new CollegeFeeTransactionDTO
                                  {
                                      FCSS_ToBePaid = a.FCSS_ToBePaid
                                  }
        ).Sum(t => t.FCSS_ToBePaid);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<CollegeFeeTransactionDTO> printreceipt([FromBody] CollegeFeeTransactionDTO data)
        {
            try
            {
                var payment_modes = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO.Where(t => t.FYP_Id == data.FYP_Id).Distinct().ToList();
                data.paymentmode_details = payment_modes.ToArray();

                List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                   from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                   from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                   where (a.AMCST_Id == data.AMCST_Id  && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == d.FCMA_Id && a.FYP_Id == data.FYP_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id )
                                   select new FeeStudentTransactionDTO
                                   {
                                       FMG_Id = d.FMG_Id,
                                       //FMH_Id = d.FMH_Id,
                                       //FTI_Id = d.FTI_Id
                                   }
                           ).Distinct().ToList();

                List<long> grp_ids = new List<long>();
                List<long> head_ids = new List<long>();
                List<long> inst_ids = new List<long>();
                foreach (var item in temp_group_head)
                {
                    grp_ids.Add(item.FMG_Id);
                    head_ids.Add(item.FMH_Id);
                    inst_ids.Add(item.FTI_Id);
                }

                var asmay_id = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Where(a => a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id).ToList().FirstOrDefault().ASMAY_Id;

                data.ASMAY_Id = asmay_id;

                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                string accountname = "", accesskey = "", html = "";
                var datatstu = _context.IVRM_Storage_path_Details.ToList();
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }

                try
                {
                    var checktemplate = (from a in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceiptDMO
                                         from b in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                         where (a.FGAR_Id == b.FGAR_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && grp_ids.Contains(b.FMG_Id))
                                         select new FeeStudentTransactionDTO
                                         {
                                             FMG_Id = b.FMG_Id,
                                             receipttemplate = a.FGAR_Template_Name,
                                         }
             ).Distinct().ToArray();

                    if (checktemplate.Count() > 0)
                    {
                        if (checktemplate[0].receipttemplate != "")
                        {
                            html = checktemplate[0].receipttemplate;
                        }
                        else
                        {
                            html = obj.getHtmlContentFromAzure(accountname, accesskey, "feereceipt/" + data.MI_Id, "FeeReceipt.html", 0);
                        }
                    }
                }
                catch (Exception ex)
                { html = ""; }

                data.htmldata = html;

                if (html != "")
                {
                    if (data.minstall == "0")
                    {
                        Printterm(data);
                    }
                    else
                    {
                        Printinstallment(data);
                    }
                }
                else { printcommon(data); }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public CollegeFeeTransactionDTO duplicaterecept(CollegeFeeTransactionDTO data)
        {
            try
            {

                data.duplicatereceipt = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                         where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_ReceiptNo.Equals(data.FYP_ReceiptNo.Trim()) && a.FYP_Id!=data.FYP_Id)
                                         select new CollegeFeeTransactionDTO
                                         {
                                             FYP_ReceiptNo = a.FYP_ReceiptNo
                                         }
              ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO get_grp_reptno(CollegeFeeTransactionDTO data)
        {
            try
            {
                if (data.auto_receipt_flag == 1)
                {
                    List<long> HeadId = new List<long>();
                    List<long> grpid = new List<long>();
                    foreach (var item in data.temp_head_list)
                    {
                        HeadId.Add(item.FMH_Id);
                        grpid.Add(item.FMG_Id);
                    }
                    string groupidss = "0";
                    for (int r = 0; r < grpid.Count(); r++)
                    {
                        groupidss = groupidss + ',' + grpid[r];
                    }
                    var final_rept_no = "";
                    List<CollegeFeeTransactionDTO> list_all = new List<CollegeFeeTransactionDTO>();
                    List<CollegeFeeTransactionDTO> list_repts = new List<CollegeFeeTransactionDTO>();

                    list_all = (from b in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceiptDMO
                                from c in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new CollegeFeeTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                }
                         ).Distinct().ToList();

                    data.grp_count = list_all.Count();

                    if (data.grp_count == 1)
                    {
                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
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

                            data.FYP_ReceiptNo = cmd.Parameters["@receiptno"].Value.ToString();

                        }
                    }
                }

                else if (data.automanualreceiptno == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.FYP_ReceiptNo = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO getsearchfilter(CollegeFeeTransactionDTO data)
        {
            try
           {

                if (data.filterinitialdata.Equals("regular"))
                {
                    data.searchfilter = data.searchfilter.ToUpper();
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1 && a.AMCST_ActiveFlag == true && ((a.AMCST_FirstName.ToUpper().Trim() + ' ' + a.AMCST_MiddleName.ToUpper().Trim() + ' ' + a.AMCST_LastName.ToUpper().Trim()).StartsWith(data.searchfilter) || (a.AMCST_FirstName.ToUpper().Trim() + a.AMCST_MiddleName.ToUpper().Trim() + ' ' + a.AMCST_LastName.ToUpper().Trim()).StartsWith(data.searchfilter) || a.AMCST_FirstName.ToUpper().StartsWith(data.searchfilter) || a.AMCST_MiddleName.ToUpper().StartsWith(data.searchfilter) || a.AMCST_LastName.ToUpper().StartsWith(data.searchfilter)))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("InActive"))
                {
                    data.searchfilter = data.searchfilter.ToUpper();
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "D" && a.AMCST_FirstName.ToUpper().StartsWith(data.searchfilter))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("regno"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1 && a.AMCST_ActiveFlag == true && a.AMCST_RegistrationNo.StartsWith(data.searchfilter))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_RegistrationNo,
                                            AMCST_MiddleName = "",
                                            AMCST_LastName = ""

                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("AdmNo"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1 && a.AMCST_ActiveFlag == true && a.AMCST_AdmNo.StartsWith(data.searchfilter))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_AdmNo,
                                            AMCST_MiddleName = "",
                                            AMCST_LastName = ""

                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("Admnoname"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1 && a.AMCST_ActiveFlag == true && a.AMCST_AdmNo.StartsWith(data.searchfilter))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_AdmNo + "-" + a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameAdmno"))
                {
                    data.searchfilter = data.searchfilter.ToUpper();
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1 && a.AMCST_ActiveFlag == true && a.AMCST_FirstName.ToUpper().StartsWith(data.searchfilter))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName + "-" + a.AMCST_AdmNo,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName + "-" + a.AMCST_AdmNo,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("NameRegNo"))
                {
                    data.searchfilter = data.searchfilter.ToUpper();
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1 && a.AMCST_ActiveFlag == true && a.AMCST_FirstName.ToUpper().StartsWith(data.searchfilter))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName + "-" + a.AMCST_RegistrationNo,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName + "-" + a.AMCST_RegistrationNo,
                                        }
             ).ToArray();
                }

                else if (data.filterinitialdata.Equals("RegNoName"))
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                        from b in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                        where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_SOL == "S" && b.ACYST_ActiveFlag == 1 && a.AMCST_ActiveFlag == true && a.AMCST_RegistrationNo.StartsWith(data.searchfilter))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            AMCST_Id = a.AMCST_Id,
                                            AMCST_FirstName = a.AMCST_RegistrationNo + "-" + a.AMCST_FirstName + ' ' + a.AMCST_MiddleName + ' ' + a.AMCST_LastName,
                                            AMCST_MiddleName = a.AMCST_MiddleName,
                                            AMCST_LastName = a.AMCST_LastName,
                                        }
             ).ToArray();
                }
                else if (data.filterinitialdata.Equals("Preadmission"))
                {

                }

            }
            catch (Exception e)
            {
                data.validationvalue = "Contact Administrator";
            }
            return data;
        }
        public CollegeFeeTransactionDTO searching(CollegeFeeTransactionDTO data)
        {
            try
            {

                switch (data.searchType)
                {

                    case "0":
                        string str = "";
                        data.searchtext = data.searchtext.ToUpper();
                        data.searcharray = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                            from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                            from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                            from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                            where (a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && a.MI_Id == data.MI_Id && d.AMB_Id == f.AMB_Id && a.ASMAY_Id == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id  && c.AMCST_SOL == "S" && c.AMCST_ActiveFlag && d.ACYST_ActiveFlag==1 && g.AMSE_Id == d.AMSE_Id && h.ACMS_Id == d.ACMS_Id && a.FYP_ChequeBounceFlag == "CL" && (((c.AMCST_FirstName.Trim() + ' ' + (string.IsNullOrEmpty(c.AMCST_MiddleName.Trim()) == true ? str : c.AMCST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(c.AMCST_LastName.ToUpper().Trim()) == true ? str : c.AMCST_LastName.ToUpper().Trim())).Trim().Contains(data.searchtext) || c.AMCST_FirstName.ToUpper().StartsWith(data.searchtext) || c.AMCST_MiddleName.ToUpper().StartsWith(data.searchtext) || c.AMCST_LastName.ToUpper().StartsWith(data.searchtext)))
                                            select new CollegeFeeTransactionDTO
                                            {
                                                AMCST_Id = c.AMCST_Id,
                                                AMCST_FirstName = c.AMCST_FirstName,
                                                AMCST_MiddleName = c.AMCST_MiddleName,
                                                AMCST_LastName = c.AMCST_LastName,
                                                FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                                FYP_PayModeType = a.FYP_PayModeType,
                                                FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                AMCO_CourseName = e.AMCO_CourseName,
                                                AMB_BranchName = f.AMB_BranchName,
                                                AMSE_SEMName = g.AMSE_SEMName,
                                                ACMS_SectionName = h.ACMS_SectionName,
                                                FYP_Id = a.FYP_Id,
                                                AMCST_AdmNo = c.AMCST_AdmNo,
                                                FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                UserId=a.User_Id
                                            }
      ).Distinct().OrderBy(t => t.AMCST_FirstName).ToArray();
                        break;
                    case "1":
                        data.searchtext = data.searchtext.ToUpper();
                        data.searcharray = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                            from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                            from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                            from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                            where (a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && d.AMB_Id == f.AMB_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.AMCO_CourseName.ToUpper().Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id  && c.AMCST_SOL == "S" && c.AMCST_ActiveFlag && d.ACYST_ActiveFlag==1 && g.AMSE_Id == d.AMSE_Id && h.ACMS_Id == d.ACMS_Id && a.FYP_ChequeBounceFlag == "CL")
                                            select new CollegeFeeTransactionDTO
                                            {
                                                AMCST_Id = c.AMCST_Id,
                                                AMCST_FirstName = c.AMCST_FirstName,
                                                AMCST_MiddleName = c.AMCST_MiddleName,
                                                AMCST_LastName = c.AMCST_LastName,
                                                FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                                FYP_PayModeType = a.FYP_PayModeType,
                                                FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                AMCO_CourseName = e.AMCO_CourseName,
                                                AMB_BranchName = f.AMB_BranchName,
                                                AMSE_SEMName = g.AMSE_SEMName,
                                                ACMS_SectionName = h.ACMS_SectionName,
                                                FYP_Id = a.FYP_Id,
                                                AMCST_AdmNo = c.AMCST_AdmNo,
                                                FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                UserId = a.User_Id
                                            }
              ).Distinct().OrderBy(t => t.AMCO_CourseName).ToArray();
                        break;
                    case "2":
                        data.searcharray = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                            from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                            from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                            from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                            where (a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && d.AMB_Id == f.AMB_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMCST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id  && c.AMCST_SOL == "S" && c.AMCST_ActiveFlag && d.ACYST_ActiveFlag==1 && g.AMSE_Id == d.AMSE_Id && h.ACMS_Id == d.ACMS_Id && a.FYP_ChequeBounceFlag == "CL")
                                            select new CollegeFeeTransactionDTO
                                            {
                                                AMCST_Id = c.AMCST_Id,
                                                AMCST_FirstName = c.AMCST_FirstName,
                                                AMCST_MiddleName = c.AMCST_MiddleName,
                                                AMCST_LastName = c.AMCST_LastName,
                                                FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                                FYP_PayModeType = a.FYP_PayModeType,
                                                FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                AMCO_CourseName = e.AMCO_CourseName,
                                                AMB_BranchName = f.AMB_BranchName,
                                                AMSE_SEMName = g.AMSE_SEMName,
                                                ACMS_SectionName = h.ACMS_SectionName,
                                                FYP_Id = a.FYP_Id,
                                                AMCST_AdmNo = c.AMCST_AdmNo,
                                                FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                UserId = a.User_Id
                                            }
              ).Distinct().OrderBy(t => t.AMCST_AdmNo).ToArray();
                        break;
                    case "3":
                        data.searcharray = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                            from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                            from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                            from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                            where (a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && d.AMB_Id == f.AMB_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_ReceiptNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id  && c.AMCST_SOL == "S" && c.AMCST_ActiveFlag && d.ACYST_ActiveFlag==1 && g.AMSE_Id == d.AMSE_Id && h.ACMS_Id == d.ACMS_Id && a.FYP_ChequeBounceFlag == "CL")
                                            select new CollegeFeeTransactionDTO
                                            {
                                                AMCST_Id = c.AMCST_Id,
                                                AMCST_FirstName = c.AMCST_FirstName,
                                                AMCST_MiddleName = c.AMCST_MiddleName,
                                                AMCST_LastName = c.AMCST_LastName,
                                                FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                                FYP_PayModeType = a.FYP_PayModeType,
                                                FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                AMCO_CourseName = e.AMCO_CourseName,
                                                AMB_BranchName = f.AMB_BranchName,
                                                AMSE_SEMName = g.AMSE_SEMName,
                                                ACMS_SectionName = h.ACMS_SectionName,
                                                FYP_Id = a.FYP_Id,
                                                AMCST_AdmNo = c.AMCST_AdmNo,
                                                FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                UserId = a.User_Id
                                            }
              ).Distinct().OrderBy(t => t.FYP_ReceiptNo).ToArray();
                        break;
                    case "4":
                        var date_format = data.searchdate.ToString("dd/MM/yyyy");
                        data.searcharray = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                            from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                            from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                            from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                                // from g in list
                                            where (a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && d.AMB_Id == f.AMB_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id && c.AMCST_SOL == "S" && c.AMCST_ActiveFlag && d.ACYST_ActiveFlag==1 && g.AMSE_Id == d.AMSE_Id && h.ACMS_Id == d.ACMS_Id && a.FYP_ReceiptDate.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && a.FYP_ChequeBounceFlag == "CL")
                                            select new CollegeFeeTransactionDTO
                                            {
                                                AMCST_Id = c.AMCST_Id,
                                                AMCST_FirstName = c.AMCST_FirstName,
                                                AMCST_MiddleName = c.AMCST_MiddleName,
                                                AMCST_LastName = c.AMCST_LastName,
                                                FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                                FYP_PayModeType = a.FYP_PayModeType,
                                                FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                AMCO_CourseName = e.AMCO_CourseName,
                                                AMB_BranchName = f.AMB_BranchName,
                                                AMSE_SEMName = g.AMSE_SEMName,
                                                ACMS_SectionName = h.ACMS_SectionName,
                                                FYP_Id = a.FYP_Id,
                                                AMCST_AdmNo = c.AMCST_AdmNo,
                                                FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                UserId = a.User_Id
                                            }
           ).Distinct().ToArray();

                        break;
                    case "5":
                        data.searcharray = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                            from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                            from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                            from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                            where (a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && d.AMB_Id == f.AMB_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_TotalPaidAmount.ToString().Contains(data.searchnumber) && d.ASMAY_Id == data.ASMAY_Id  && c.AMCST_SOL == "S" && c.AMCST_ActiveFlag && d.ACYST_ActiveFlag==1 && g.AMSE_Id == d.AMSE_Id && h.ACMS_Id == d.ACMS_Id && a.FYP_ChequeBounceFlag == "CL")
                                            select new CollegeFeeTransactionDTO
                                            {
                                                AMCST_Id = c.AMCST_Id,
                                                AMCST_FirstName = c.AMCST_FirstName,
                                                AMCST_MiddleName = c.AMCST_MiddleName,
                                                AMCST_LastName = c.AMCST_LastName,
                                                FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                                FYP_PayModeType = a.FYP_PayModeType,
                                                FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                AMCO_CourseName = e.AMCO_CourseName,
                                                AMB_BranchName = f.AMB_BranchName,
                                                AMSE_SEMName = g.AMSE_SEMName,
                                                ACMS_SectionName = h.ACMS_SectionName,
                                                FYP_Id = a.FYP_Id,
                                                AMCST_AdmNo = c.AMCST_AdmNo,
                                                FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                UserId = a.User_Id
                                            }
              ).Distinct().OrderBy(t => t.FYP_TotalPaidAmount).ToArray();
                        break;
                    case "6":
                        data.searcharray = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                            from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                            from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                            from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                            where (a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && d.AMB_Id == f.AMB_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_TransactionTypeFlag.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id  && c.AMCST_SOL == "S" && c.AMCST_ActiveFlag && d.ACYST_ActiveFlag==1 && g.AMSE_Id == d.AMSE_Id && h.ACMS_Id == d.ACMS_Id && a.FYP_ChequeBounceFlag == "CL")
                                            select new CollegeFeeTransactionDTO
                                            {
                                                AMCST_Id = c.AMCST_Id,
                                                AMCST_FirstName = c.AMCST_FirstName,
                                                AMCST_MiddleName = c.AMCST_MiddleName,
                                                AMCST_LastName = c.AMCST_LastName,
                                                FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                FYP_PayModeType = a.FYP_PayModeType,
                                                FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                AMCO_CourseName = e.AMCO_CourseName,
                                                AMB_BranchName = f.AMB_BranchName,
                                                AMSE_SEMName = g.AMSE_SEMName,
                                                ACMS_SectionName = h.ACMS_SectionName,
                                                FYP_Id = a.FYP_Id,
                                                AMCST_AdmNo = c.AMCST_AdmNo,
                                                FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                UserId = a.User_Id
                                            }
              ).Distinct().OrderBy(t => t.FYP_Bank_Or_Cash).ToArray();

                        break;
                    case "7":
                        data.searchtext = data.searchtext.ToUpper();
                        data.searcharray = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                            from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                            from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                            from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                            where (a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && d.AMB_Id == f.AMB_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && f.AMB_BranchName.ToUpper().Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id  && c.AMCST_SOL == "S" && c.AMCST_ActiveFlag && d.ACYST_ActiveFlag==1 && g.AMSE_Id == d.AMSE_Id && h.ACMS_Id == d.ACMS_Id && a.FYP_ChequeBounceFlag == "CL")
                                            select new CollegeFeeTransactionDTO
                                            {
                                                AMCST_Id = c.AMCST_Id,
                                                AMCST_FirstName = c.AMCST_FirstName,
                                                AMCST_MiddleName = c.AMCST_MiddleName,
                                                AMCST_LastName = c.AMCST_LastName,
                                                FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                                FYP_PayModeType = a.FYP_PayModeType,
                                                FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                AMCO_CourseName = e.AMCO_CourseName,
                                                AMB_BranchName = f.AMB_BranchName,
                                                AMSE_SEMName = g.AMSE_SEMName,
                                                ACMS_SectionName = h.ACMS_SectionName,
                                                FYP_Id = a.FYP_Id,
                                                AMCST_AdmNo = c.AMCST_AdmNo,
                                                FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                UserId = a.User_Id
                                            }
              ).Distinct().OrderBy(t => t.AMB_BranchName).ToArray();
                        break;
                    case "8":
                        data.searchtext = data.searchtext.ToUpper();
                        data.searcharray = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                            from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                            from d in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                            from e in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                            from f in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                            from g in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                            from h in _YearlyFeeGroupMappingContext.Adm_College_Master_SectionDMO
                                            where (a.FYP_Id == b.FYP_Id && b.AMCST_Id == c.AMCST_Id && c.AMCST_Id == d.AMCST_Id && d.AMCO_Id == e.AMCO_Id && a.MI_Id == data.MI_Id && d.AMB_Id == f.AMB_Id && a.ASMAY_Id == data.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id  && c.AMCST_SOL == "S" && c.AMCST_ActiveFlag && d.ACYST_ActiveFlag == 1 && g.AMSE_Id == d.AMSE_Id && h.ACMS_Id == d.ACMS_Id && a.FYP_ChequeBounceFlag == "CL" && c.AMCST_MobileNo== Convert.ToInt64(data.searchtext))
                                            select new CollegeFeeTransactionDTO
                                            {
                                                AMCST_Id = c.AMCST_Id,
                                                AMCST_FirstName = c.AMCST_FirstName,
                                                AMCST_MiddleName = c.AMCST_MiddleName,
                                                AMCST_LastName = c.AMCST_LastName,
                                                FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                                FYP_PayModeType = a.FYP_PayModeType,
                                                FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                AMCO_CourseName = e.AMCO_CourseName,
                                                AMB_BranchName = f.AMB_BranchName,
                                                AMSE_SEMName = g.AMSE_SEMName,
                                                ACMS_SectionName = h.ACMS_SectionName,
                                                FYP_Id = a.FYP_Id,
                                                AMCST_AdmNo = c.AMCST_AdmNo,
                                                FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                UserId = a.User_Id
                                            }
       ).Distinct().OrderBy(t => t.AMCST_FirstName).ToArray();
                        break;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO edittra(CollegeFeeTransactionDTO data)
        {
            try
            {
                        var result_obj = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Single(t => t.FYP_Id == data.FYP_Id);
                        data.FYP_PayModeType = result_obj.FYP_PayModeType;
                        data.FYP_ReceiptDate = result_obj.FYP_ReceiptDate;
                        data.FYP_ReceiptNo = result_obj.FYP_ReceiptNo;
                        data.FYP_Remarks = result_obj.FYP_Remarks;
                        data.currpaymentdetails = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO.Where(t => t.FYP_Id == data.FYP_Id).Distinct().ToArray();

                data.fetchmodeofpayment = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                           from g in _YearlyFeeGroupMappingContext.IVRM_ModeOfPayment
                                           from h in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                           where (a.FYP_Id == b.FYP_Id && a.ASMAY_Id == b.ASMAY_Id && b.FYP_Id == h.FYP_Id && a.FYP_Id == h.FYP_Id && g.IVRMMOD_ModeOfPayment_Code == a.FYP_TransactionTypeFlag && a.MI_Id == data.MI_Id && g.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               Bank_Date = a.FYP_ReceiptDate,
                                               Bank_No = h.FYPPM_DDChequeNo,
                                               Bank_Amount = a.FYP_TotalPaidAmount,
                                               Bank_Name = h.FYPPM_BankName,
                                               fyppM_TransactionTypeFlag = a.FYP_TransactionTypeFlag,
                                               IVRMMOD_Id = g.IVRMMOD_Id,
                                               IVRMMOD_ModeOfPayment = g.IVRMMOD_ModeOfPayment,
                                               IVRMMOD_Flag = g.IVRMMOD_Flag,
                                               IVRMMOD_ModeOfPayment_Code = g.IVRMMOD_ModeOfPayment_Code,
                                               FYP_Id = a.FYP_Id,
                                               FYPPM_Id = h.FYPPM_Id,
                                               FYPPM_ClearanceDate=h.FYPPM_ClearanceDate
                                           }
).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<CollegeFeeTransactionDTO> printreceiptnew([FromBody] CollegeFeeTransactionDTO data)
        {
            try
            {
                printcommon(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        private CollegeFeeTransactionDTO printcommon(CollegeFeeTransactionDTO data)
        {
            try
            {
                var fillstudent = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                               from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                               from o in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                               from d in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                               from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                               from f in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                               from g in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                               from h in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                               from i in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                               from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                               where (a.FYP_Id == b.FYP_Id && a.FCMAS_Id == o.FCMAS_Id && o.FCMA_Id == c.FCMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMCST_Id == j.AMCST_Id && f.AMCO_Id == g.AMCO_Id && f.AMB_Id == h.AMB_Id && i.AMCST_Id == j.AMCST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMCST_Id == data.AMCST_Id && b.FYP_Id == data.FYP_Id && f.ACYST_ActiveFlag==1)
                                               select new CollegeFeeTransactionDTO
                                               {
                                                   AMCST_Id = f.AMCST_Id,
                                                   AMCST_FirstName = i.AMCST_FirstName,
                                                   AMCST_MiddleName = i.AMCST_MiddleName,
                                                   AMCST_LastName = i.AMCST_LastName,
                                                   FMH_Id = d.FMH_Id,
                                                   FMH_FeeName = d.FMH_FeeName,
                                                   FTI_Name = e.FTI_Name,
                                                   FTI_Id = e.FTI_Id,
                                                   FYP_ReceiptNo = b.FYP_ReceiptNo,
                                                   FTCP_PaidAmount = a.FTCP_PaidAmount,
                                                   FTCP_ConcessionAmount = a.FTCP_ConcessionAmount,
                                                   FTCP_FineAmount = a.FTCP_FineAmount,
                                                   FYP_ReceiptDate = b.FYP_ReceiptDate,
                                                   AMCO_CourseName = g.AMCO_CourseName,
                                                   AMB_BranchName = h.AMB_BranchName,
                                                   ACYST_RollNo = f.ACYST_RollNo,
                                                   AMCST_AdmNo = i.AMCST_AdmNo,
                                                   AMCST_FatherName = i.AMCST_FatherName,
                                                   AMCST_MotherName = i.AMCST_MotherName,
                                                   FYP_Bank_Or_Cash = b.FYP_PayModeType,
                                                   FYP_Remarks = b.FYP_Remarks,
                                                   AMCST_RegistrationNo = i.AMCST_RegistrationNo,
                                               }
              ).Distinct().ToList();

               data.fillstudentviewdetails = fillstudent.ToArray();

                List<long> lstheadid = new List<long>();
                foreach (var lst in fillstudent)
                {
                    lstheadid.Add(lst.FMH_Id);
                }
                CollegeFeeTransactionDTO fst = new CollegeFeeTransactionDTO();
                List<CollegeFeeTransactionDTO> lstfst = new List<CollegeFeeTransactionDTO>();
                foreach (var lst1 in lstheadid.Distinct())
                {
                    decimal totalcharges = 0, Fine_Amt = 0, Concession_Amt = 0, Waived_Amt = 0, paidamt = 0;
                    foreach (var lst in fillstudent)
                    {
                        if (lst1.Equals(lst.FMH_Id))
                        {
                            fst.FMH_FeeName = lst.FMH_FeeName;
                            totalcharges = totalcharges + lst.FCSS_TotalCharges;
                            Fine_Amt = Fine_Amt + lst.FTCP_FineAmount;
                            Concession_Amt = Concession_Amt + lst.FTCP_ConcessionAmount;
                            Waived_Amt = Waived_Amt + lst.FTCP_WaivedAmount;
                            paidamt = paidamt + lst.FTCP_PaidAmount;
                            fst.FCSS_TotalCharges = Convert.ToInt64(totalcharges);
                            fst.FTCP_FineAmount = Fine_Amt;
                            fst.FTCP_ConcessionAmount = Concession_Amt;
                            fst.FTCP_WaivedAmount = Waived_Amt;
                            fst.FTCP_PaidAmount = paidamt;

                        }
                    }
                    lstfst.Add(fst);
                }
                data.filltotaldetails = lstfst.ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FYP_Id==a.FYP_Id
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FTCP_PaidAmount = a.FYP_TotalPaidAmount,
                                               FTCP_ConcessionAmount = b.FYPCS_TotalConcessionAmount,
                                               FTCP_FineAmount = a.FYP_TotalFineAmount,
                                               FTCP_WaivedAmount = b.FYPCS_TotalWaivedAmount,
                                               FYP_ReceiptDate = a.FYP_ReceiptDate,
                                               FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                               FYP_Remarks = a.FYP_Remarks,
                                           }
              ).Distinct().ToArray();


                List<Institution> masins = new List<Institution>();
                masins = _context.Institute.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.masterinstitution = masins.ToArray();

                //to find next due amount
                var feeterm = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                               from b in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FCMAS_Id == d.FCMAS_Id && d.AMCST_Id == c.AMCST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.AMCST_Id == data.AMCST_Id && a.FYP_Id == data.FYP_Id)
                               select new CollegeFeeTransactionDTO
                               {
                                   FMT_Id = e.FMT_Id
                               }

                 ).Distinct().OrderByDescending(t => t.FMT_Id).ToArray();

                long fmt_id_new = 0;
                long initialfmtids = Convert.ToInt64(feeterm[0].FMT_Id);

                fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;

                List<CollegeFeeTransactionDTO> temp_group_head = new List<CollegeFeeTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                   from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                   from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                   where (a.AMCST_Id == data.AMCST_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == e.FCMAS_Id && e.FCMA_Id==d.FCMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new CollegeFeeTransactionDTO
                                   {

                                       FMG_Id = d.FMG_Id,
                                       FMH_Id = d.FMH_Id,
                                       FTI_Id = d.FTI_Id

                                   }

                           ).Distinct().ToList();
                List<long> grp_ids = new List<long>();
                List<long> head_ids = new List<long>();
                List<long> inst_ids = new List<long>();
                foreach (var item in temp_group_head)
                {
                    grp_ids.Add(item.FMG_Id);
                    head_ids.Add(item.FMH_Id);
                    inst_ids.Add(item.FTI_Id);
                }

                List<CollegeFeeTransactionDTO> fordate = new List<CollegeFeeTransactionDTO>();
                List<CollegeFeeTransactionDTO> fordateinfyp = new List<CollegeFeeTransactionDTO>();

                fordate = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                           from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                           where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.AMCST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && (d.FCSS_ToBePaid > 0))
                           select new CollegeFeeTransactionDTO
                           {
                               date = f.FCTDD_Day,
                               month = f.FCTDD_Month,
                           }
                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                                where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.AMCST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new CollegeFeeTransactionDTO
                                {
                                    month = f.FCTDD_Month,
                                }
                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (CollegeFeeTransactionDTO item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (CollegeFeeTransactionDTO itemperiod in fordateinfyp)
                {
                    startperiod.Add(Convert.ToInt32(itemperiod.month));
                }

                foreach (var item in months)
                {
                    if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                    {
                        months1.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        data.year = curyear.Year.ToString();
                    }
                    else
                    {
                        months2.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year + 1;
                        data.year = nextyr.ToString();
                    }
                }

                string maxmonth = "", monthnameinitial = "", monthnameend = "";
                if (months1.Count() > 0)
                {
                    data.month = months1.Min().ToString();
                    maxmonth = months1.Max().ToString();
                    if (startperiod.Count >= 4)
                    {
                        monthnameinitial = startperiod.Min().ToString();
                        maxmonth = startperiod.Max().ToString();
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                    else
                    {
                        monthnameinitial = startperiod.Max().ToString();
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;
                }
                else if (months2.Count() > 0)
                {
                    data.month = months2.Min().ToString();
                    maxmonth = months2.Max().ToString();

                    monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(data.month));
                    monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));


                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;

                }
                for (int i = 0; i < months.Count(); i++)
                {
                    if (Convert.ToInt32(data.month) == months[i])
                    {
                        data.date = dates[i].ToString();
                    }
                }

                if (months.Count == 0)
                {
                    foreach (var item in startperiod)
                    {
                        if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                        {
                            monthnameinitial = startperiod.Min().ToString();
                            var curyear = DateTime.Now;
                            data.year = curyear.Year.ToString();
                        }
                        else
                        {
                            maxmonth = startperiod.Max().ToString();
                            var curyear = DateTime.Now;
                            var nextyr = curyear.Year + 1;
                            data.year = nextyr.ToString();
                        }
                    }
                    if (monthnameinitial != "")
                    {
                        monthnameinitial = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(monthnameinitial));
                    }

                    if (monthnameend != "")
                    {
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                    else
                    {
                        maxmonth = startperiod.Max().ToString();
                        monthnameend = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(Convert.ToInt32(maxmonth));
                    }
                }

                var termperiodlist = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == initialfmtids).ToArray();

                monthnameinitial = termperiodlist[0].FromMonth.ToString();
                monthnameend = termperiodlist[0].ToMonth.ToString();

                data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;

                data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag != "E")
                                  select new CollegeFeeTransactionDTO
                                  {
                                      FCSS_ToBePaid = a.FCSS_ToBePaid
                                  }
             ).Sum(t => t.FCSS_ToBePaid);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO Search_Chaln_No(CollegeFeeTransactionDTO data)
        {
            try
            {
                var flag = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.FYP_ChallanNo == data.FYP_ChallanNo && !string.IsNullOrEmpty(t.FYP_ChallanNo)).Select(t => t.FYP_Id).Distinct().ToList();
                if (flag.Count > 0)
                {
                    data.Challan_Flag = true;
                    data.FYP_Id = flag[0];
                    data.AMCST_Id = _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO.Single(t => t.FYP_Id == data.FYP_Id).AMCST_Id;

                    var ToBePaid_Amount = (from b in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                            from c in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                            where (b.FYP_Id == data.FYP_Id && b.FCMAS_Id == c.FCMAS_Id && c.AMCST_Id == data.AMCST_Id && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                                            select c.FCSS_ToBePaid).Sum();

                    if (ToBePaid_Amount > 0)
                    {
                        data.returnval = "Pay";
                        data.AMCST_AdmNo = _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO.Single(t => t.MI_Id == data.MI_Id && t.AMCST_Id == data.AMCST_Id).AMCST_AdmNo;
                        data.AMCST_FirstName = _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO.Single(t => t.MI_Id == data.MI_Id && t.AMCST_Id == data.AMCST_Id).AMCST_FirstName;

                        if (data.configset.Equals("T"))
                        {
                            var fmh_ids = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                            from c in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                            where (a.FYP_Id == data.FYP_Id && a.FCMAS_Id == c.FCMAS_Id && c.FCMA_Id == b.FCMA_Id)
                                            select b.FMH_Id).Distinct().ToList();

                            var fti_ids = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                            from c in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                            where (a.FYP_Id == data.FYP_Id && a.FCMAS_Id == c.FCMAS_Id && c.FCMA_Id == b.FCMA_Id)
                                            select b.FTI_Id).Distinct().ToList();

                            data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                                    from b in _YearlyFeeGroupMappingContext.feeTr
                                                    where (a.FMT_Id == b.FMT_Id && a.MI_Id == data.MI_Id && fmh_ids.Contains(a.FMH_Id) && fti_ids.Contains(a.FTI_Id)) 
                                                    select new CollegeFeeTransactionDTO
                                                    {
                                                        FMG_GroupName = b.FMT_Name,
                                                        FMG_Id = a.FMT_Id,
                                                    }).Distinct().ToArray();

                        }
                        else
                        {
                            var groups = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                            from c in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                            where (a.FYP_Id == data.FYP_Id && a.FCMAS_Id == c.FCMAS_Id && c.FCMA_Id == b.FCMA_Id)
                                            select b.FMG_Id).Distinct().ToList();

                            data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                    where (a.FMG_ActiceFlag == true && groups.Contains(a.FMG_Id))
                                                    select new CollegeFeeTransactionDTO
                                                    {
                                                        FMG_GroupName = a.FMG_GroupName,
                                                        FMG_Id = a.FMG_Id,
                                                    }
                ).Distinct().ToArray();

                        }
                        data.alldata = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                        from b in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                        from c in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                        from d in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                        from f in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                        from g in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                        where (a.FYP_Id == data.FYP_Id && a.FCMAS_Id == g.FCMAS_Id && g.FCMA_Id == b.FCMA_Id && b.FMG_Id == c.FMG_Id && b.FMH_Id == d.FMH_Id && b.FTI_Id == e.FTI_Id && f.MI_Id == data.MI_Id && f.AMCST_Id == data.AMCST_Id && f.ASMAY_Id == data.ASMAY_Id && f.FCMAS_Id == a.FCMAS_Id)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            FCMAS_Id = a.FCMAS_Id,
                                            FMH_FeeName = d.FMH_FeeName,
                                            FMH_Flag = d.FMH_Flag,
                                            FTI_Name = e.FTI_Name,
                                            FMH_Id = b.FMH_Id,
                                            FTI_Id = b.FTI_Id,
                                            FCSS_NetAmount = f.FCSS_NetAmount,
                                            FCSS_ToBePaid = f.FCSS_ToBePaid,
                                            FCSS_ConcessionAmount = f.FCSS_ConcessionAmount,
                                            FCSS_FineAmount = f.FCSS_FineAmount,
                                            FCSS_CurrentYrCharges = f.FCSS_CurrentYrCharges,
                                            FCSS_TotalCharges = f.FCSS_TotalCharges,
                                            FMG_Id = b.FMG_Id,
                                            FMG_GroupName = c.FMG_GroupName,
                                        }).OrderBy(t => t.FMH_Id).ToArray();

                        if (data.alldata.Length > 0)
                        {
                            List<CollegeFeeTransactionDTO> fines_fma_ids = new List<CollegeFeeTransactionDTO>();
                            var fine_amount = 0;
                            foreach (CollegeFeeTransactionDTO x in data.alldata)
                            {
                                CollegeFeeTransactionDTO sew = new CollegeFeeTransactionDTO();
                                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "Sp_Calculate_Fine_CLG";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@On_Date",
                                        SqlDbType.DateTime)
                                    {
                                        Value = data.FYP_ReceiptDate
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                                        SqlDbType.BigInt)
                                    {
                                        Value = x.FCMAS_Id
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                    SqlDbType.BigInt)
                                    {
                                        Value = data.ASMAY_Id
                                    });

                                    cmd.Parameters.Add(new SqlParameter("@amt",
                        SqlDbType.Float)
                                    {
                                        Direction = ParameterDirection.Output
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@flgArr",
                        SqlDbType.Int)
                                    {
                                        Direction = ParameterDirection.Output
                                    });
                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var data1 = cmd.ExecuteNonQuery();
                                    fine_amount += Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    sew.FCMAS_Id = x.FCMAS_Id;
                                    sew.Fine_Amt = Convert.ToInt32(cmd.Parameters["@amt"].Value);
                                    fines_fma_ids.Add(sew);
                                }
                            }
                            data.Fine_FCMAS_Ids = fines_fma_ids.Distinct().ToArray();
                        }

                    }
                    else
                    {
                        data.returnval = "Paid";
                    }
                }
                else
                {
                    data.Challan_Flag = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO Save_Chaln_No(CollegeFeeTransactionDTO data)
        {
            try
            {
      
            }
            catch (Exception ee)
            {
                data.returnval = "Error";
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeFeeTransactionDTO dynamicfinecalculation(CollegeFeeTransactionDTO data)
        {
           try
            {
                var retObject = new List<dynamic>();
                var retObject1 = new List<dynamic>();
                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_Student_Status_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar, 10)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar, 10)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                        SqlDbType.VarChar, 10)
                    {
                        Value = data.UserId
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                        SqlDbType.VarChar, 10)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                        SqlDbType.VarChar, 100)
                    {
                        Value = data.multiplegroups
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow1);
                            }

                        }
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogDebug(ex.Message);
                    }
                }

                var fine_amount = 0;
                CollegeFeeTransactionDTO sew = new CollegeFeeTransactionDTO();
                foreach (var x in retObject)
                {
                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Sp_Calculate_Fine_CLG";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@On_Date",
                            SqlDbType.DateTime)
                        {
                            Value = data.FYP_ReceiptDate
                        });

                        cmd.Parameters.Add(new SqlParameter("@FCMAS_Id",
                           SqlDbType.BigInt)
                        {
                            Value = data.FCMAS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@FCSS_ToBePaid",
                     SqlDbType.BigInt)
                        {
                            Value = data.FCSS_ToBePaid
                        });

                        cmd.Parameters.Add(new SqlParameter("@amt",
            SqlDbType.Float)
                        {
                            Direction = ParameterDirection.Output
                        });
                        cmd.Parameters.Add(new SqlParameter("@flgArr",
           SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();
                        fine_amount += Convert.ToInt32(cmd.Parameters["@amt"].Value);
                        data.FCSS_FineAmount = fine_amount;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public CollegeFeeTransactionDTO viewstatus(CollegeFeeTransactionDTO data)
        {
            try
            {
                RazorpayTransactionLogs(data);



                //data.razorpaytransactionlogs = (from a in _YearlyFeeGroupMappingContext.Fee_Payment_Settlement_Details_CollegeDMO
                //                                from b in _YearlyFeeGroupMappingContext.overall
                //                                from c in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                //                                where (a.FYPPST_Id == b.FYPPST_Id && c.fyp_transaction_id == a.FYPPSD_Payment_Id && c.ASMAY_ID == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.FYP_Id == data.FYP_Id)
                //                                select new FeeStudentTransactionDTO
                //                                {
                //                                    FYPPST_Settlement_Date = b.FYPPST_Settlement_Date,
                //                                }
                //                           ).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
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


        public CollegeFeeTransactionDTO RazorpayTransactionLogs(CollegeFeeTransactionDTO data)
        {
            try
            {
                List<Translogsresults> razorpayparam = new List<Translogsresults>();
                List<FeeStudentTransactionDTO> razorpaytransactionlogs = new List<FeeStudentTransactionDTO>();
                razorpaytransactionlogs = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                           from c in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                           where (a.FYP_Id == b.FYP_Id && a.FYP_Id == data.FYP_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == c.AMCST_Id)
                                           select new FeeStudentTransactionDTO
                                           {
                                               trans_id = b.FYP_Transaction_Id,
                                               FYP_PayModeType = b.FYP_PayModeType,
                                               FYP_Date = b.FYP_ReceiptDate,
                                               FMOT_PayGatewayType = b.FYP_PayGatewayType,
                                               Amst_Id = a.AMCST_Id,
                                               amst_email_id = c.AMCST_emailId,
                                               amst_mobile = c.AMCST_MobileNo,
                                               FYP_Tot_Amount = b.FYP_TotalPaidAmount,
                                               FYP_PaymentReference_Id = b.FYP_PaymentReference_Id
                                           }
     ).ToList();

                if (razorpaytransactionlogs.Count > 0)
                {
                    if (razorpaytransactionlogs[0].FMOT_PayGatewayType == "RAZORPAY")
                    {
                        for (int i = 0; i < razorpaytransactionlogs.Count(); i++)
                        {
                            string PaymentStatusurl = "https://api.razorpay.com/v1/orders/ID/payments";

                            PaymentDetails response1 = new PaymentDetails();
                            List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                            paymentdetails = _YearlyFeeGroupMappingContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();
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
                                if ((String)root1["status"] != "failed")
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
                        }
                        data.translogresults = razorpayparam.ToArray();
                    }
                    else if (razorpaytransactionlogs[0].FMOT_PayGatewayType == "EASEBUZZ")
                    {
                        for (int i = 0; i < razorpaytransactionlogs.Count(); i++)
                        {
                            List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                            paymentdetails = _YearlyFeeGroupMappingContext.Fee_PaymentGateway_Details.Where(q => q.MI_Id == data.MI_Id && q.FPGD_PGName == "EASEBUZZ").Distinct().ToList();
                            var masterinstitution = _YearlyFeeGroupMappingContext.master_institution.Where(z => z.MI_ActiveFlag == 1 && z.MI_Id == data.MI_Id).ToList();

                            string txnid = razorpaytransactionlogs[i].trans_id.ToString();
                            string key = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
                            string secret = paymentdetails.FirstOrDefault().FPGD_SaltKey;
                            string merchant_email = masterinstitution[0].MI_PGRegisteredEmailId;
                            string env = "prod";

                            Easebuzz t = new Easebuzz(secret, key, env);
                            var transdate = razorpaytransactionlogs[i].FYP_Date.ToString("dd-MM-yyyy");

                            var res = t.payoutAPI(merchant_email, transdate);

                            JObject joResponse1 = JObject.Parse(res);
                            JArray array1 = (JArray)joResponse1["payouts_history_data"];

                            var payment_id = razorpaytransactionlogs[i].FYP_PaymentReference_Id.ToString();


                            var FMA_Amountnew = "";
                            DateTime date = DateTime.Now;
                            foreach (JObject root1 in array1)
                            {
                                if ((bool)joResponse1["status"] == true)
                                {
                                    JArray array2 = (JArray)root1["peb_transactions"];
                                    JArray array3 = (JArray)root1["split_payouts"];

                                    foreach (JObject root2 in array2)
                                    {
                                        if ((String)root2["txnid"] == txnid)
                                        {


                                            payment_id = payment_id;
                                            txnid = txnid;
                                            FMA_Amountnew = (String)root2["peb_settlement_amount"];
                                            date = (DateTime)root1["payout_actual_date"];
                                        }

                                    }


                                }
                            }
                            if ((bool)joResponse1["status"] == true)
                            {
                                razorpayparam.Add(new Translogsresults
                                {
                                    payment_id = payment_id,
                                    responsestatuslogs = ((bool)joResponse1["status"]).ToString(),
                                    error_description = "",
                                    order_id = txnid,
                                    FYP_Date = Convert.ToDateTime(date),
                                    //FMA_Amount = Convert.ToInt64(FMA_Amountnew),
                                    FYP_PayModeType = razorpaytransactionlogs[i].FYP_PayModeType,
                                    FMOT_PayGatewayType = razorpaytransactionlogs[i].FMOT_PayGatewayType
                                });
                            }

                        }

                        data.translogresults = razorpayparam.ToArray();
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





