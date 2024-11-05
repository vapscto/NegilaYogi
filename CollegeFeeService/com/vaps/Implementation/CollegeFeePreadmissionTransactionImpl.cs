using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Fees;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DomainModel.Model.com.vaps.Fee;
using CollegeFeeService.com.vaps.Interfaces;
using System.Dynamic;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegeFeePreadmissionTransactionImpl : CollegeFeePreadmissionTransactionInterface
    {
        private static ConcurrentDictionary<string, CollegeFeeTransactionDTO> _login =
      new ConcurrentDictionary<string, CollegeFeeTransactionDTO>();

        private static readonly Object obj = new Object();

        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        readonly ILogger<CollegeFeeTransactionImpl> _logger;
        public CollegeFeePreadmissionTransactionImpl(CollFeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context, ILogger<CollegeFeeTransactionImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
            _logger = log;
        }

        public CollegeFeeTransactionDTO getdata(CollegeFeeTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _context.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id && t.userid == data.UserId).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                List<FeeGroupClgDMO> group = new List<FeeGroupClgDMO>();
                group = _YearlyFeeGroupMappingContext.FeeGroupClgDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == data.MI_Id).ToList();
                data.fillmastergroup = group.ToArray();

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _YearlyFeeGroupMappingContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Transaction").ToList();
                data.transnumconfig = masnum.ToArray();

                bool recsettingval = false;
                var getcurrsett = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id && t.userid == data.UserId).ToList();
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

                if (data.filterinitialdata == "Preadmission")
                {
                    data.preadmissionstudentlist = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                                    from b in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                                    from c in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                                    from d in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                                    where (a.AMCO_Id == b.AMCO_Id && a.AMB_Id == c.AMB_Id && a.AMSE_Id==d.AMSE_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PACA_FinalpaymentFlag == false)
                                                    select new CollegeFeeTransactionDTO
                                                    {
                                                        PACA_Id = a.PACA_Id,
                                                        PACA_FirstName = ((a.PACA_FirstName == null || a.PACA_FirstName == "" ? "" : " " + a.PACA_FirstName) + (a.PACA_MiddleName == null || a.PACA_MiddleName == "" || a.PACA_MiddleName == "0" ? "" : " " + a.PACA_MiddleName) + (a.PACA_LastName == null || a.PACA_LastName == "" || a.PACA_LastName == "0" ? "" : " " + a.PACA_LastName)).Trim(),
                                                        PACA_MiddleName = a.PACA_MiddleName,
                                                        PACA_LastName = a.PACA_LastName,
                                                        AMCO_CourseName = b.AMCO_CourseName,
                                                        AMB_BranchName = c.AMB_BranchName,
                                                        AMSE_SEMName = d.AMSE_SEMName,
                                                        PACA_RegistrationNo = a.PACA_RegistrationNo,
                                                    }
  ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

                    var fetchmaxfypidabc = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                            where (a.PACA_Id == b.PACA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                            select new CollegeFeeTransactionDTO
                                            {
                                                FYP_Id = b.FYP_Id
                                            }
 ).Distinct().OrderByDescending(t => t.FYP_Id).ToList();

                    List<long> fetchmaxfypid = new List<long>();
                    foreach (var item in fetchmaxfypidabc)
                    {
                        fetchmaxfypid.Add(item.FYP_Id);
                    }

                    data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                              from c in _YearlyFeeGroupMappingContext.PA_College_Application
                                              from d in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                              from e in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                              from f in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                              where (a.FYP_Id == b.FYP_Id && b.PACA_Id == c.PACA_Id && c.AMCO_Id == d.AMCO_Id && c.AMB_Id == e.AMB_Id && c.AMSE_Id == f.AMSE_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                              select new CollegeFeeTransactionDTO
                                              {
                                                  PACA_Id = c.PACA_Id,
                                                  PACA_FirstName = ((c.PACA_FirstName == null ||c.PACA_FirstName == "" ? "" : " " + c.PACA_FirstName) + (c.PACA_MiddleName == null || c.PACA_MiddleName == "" || c.PACA_MiddleName == "0" ? "" : " " + c.PACA_MiddleName) + (c.PACA_LastName == null || c.PACA_LastName == "" || c.PACA_LastName == "0" ? "" : " " + c.PACA_LastName)).Trim(),
                                                  PACA_MiddleName = c.PACA_MiddleName,
                                                  PACA_LastName = c.PACA_LastName,
                                                  AMCO_CourseName = d.AMCO_CourseName,
                                                  AMB_BranchName = e.AMB_BranchName,
                                                  AMSE_SEMName = f.AMSE_SEMName,
                                                  FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                  FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                  FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                  FYP_Id = a.FYP_Id,
                                                  PACA_MobileNo = c.PACA_MobileNo,
                                                  FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                  PACA_ApplicationNo = c.PACA_ApplicationNo,
                                                  PACA_emailId = c.PACA_emailId,
                                                  UserId = a.User_Id,
                                                  FYP_TransactionTypeFlag=a.FYP_TransactionTypeFlag,
                                                  FYP_ApprovedFlg=a.FYP_ApprovedFlg,
                                                  PACA_RegistrationNo=c.PACA_RegistrationNo,
                                              }
         ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }
                else if (data.filterinitialdata == "Prospectus")
                {
  //                  data.preadmissionstudentlist = (from a in _YearlyFeeGroupMappingContext.Prospectus
  //                                                  from b in _YearlyFeeGroupMappingContext.MasterCourseDMO
  //                                                  from c in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
  //                                                  from d in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
  //                                                  where (a.AMCO_Id == b.AMCO_Id && a.AMB_Id == c.AMB_Id && a.AMSE_Id == d.AMSE_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
  //                                                  select new CollegeFeeTransactionDTO
  //                                                  {
  //                                                      PACA_Id = a.PACA_Id,
  //                                                      PACA_FirstName = ((a.PACA_FirstName == null || a.PACA_FirstName == "" ? "" : " " + a.PACA_FirstName) + (a.PACA_MiddleName == null || a.PACA_MiddleName == "" || a.PACA_MiddleName == "0" ? "" : " " + a.PACA_MiddleName) + (a.PACA_LastName == null || a.PACA_LastName == "" || a.PACA_LastName == "0" ? "" : " " + a.PACA_LastName)).Trim(),
  //                                                      PACA_MiddleName = a.PACA_MiddleName,
  //                                                      PACA_LastName = a.PACA_LastName,
  //                                                      AMCO_CourseName = b.AMCO_CourseName,
  //                                                      AMB_BranchName = c.AMB_BranchName,
  //                                                      AMSE_SEMName = d.AMSE_SEMName,
  //                                                  }
  //).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();


         //           var fetchmaxfypid = _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO.OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

         //           data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
         //                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ProspectusDMO
         //                                     from c in _YearlyFeeGroupMappingContext.Prospectus
         //                                     from d in _YearlyFeeGroupMappingContext.MasterCourseDMO
         //                                     from e in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
         //                                     from f in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
         //                                     where (a.FYP_Id == b.FYP_Id && b.PACA_Id == c.PACA_Id && c.AMCO_Id == d.AMCO_Id && c.AMB_Id == e.AMB_Id && c.AMSE_Id == f.AMSE_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
         //                                     select new CollegeFeeTransactionDTO
         //                                     {
         //                                         PACA_Id = c.PACA_Id,
         //                                         PACA_FirstName = ((c.PACA_FirstName == null || c.PACA_FirstName == "" ? "" : " " + c.PACA_FirstName) + (c.PACA_MiddleName == null || c.PACA_MiddleName == "" || c.PACA_MiddleName == "0" ? "" : " " + c.PACA_MiddleName) + (c.PACA_LastName == null || c.PACA_LastName == "" || c.PACA_LastName == "0" ? "" : " " + c.PACA_LastName)).Trim(),
         //                                         PACA_MiddleName = c.PACA_MiddleName,
         //                                         PACA_LastName = c.PACA_LastName,
         //                                         AMCO_CourseName = d.AMCO_CourseName,
         //                                         AMB_BranchName = e.AMB_BranchName,
         //                                         AMSE_SEMName = f.AMSE_SEMName,
         //                                         FYP_Receipt_No = a.FYP_ReceiptNo,
         //                                         FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
         //                                         FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
         //                                         FYP_Id = a.FYP_Id,
         //                                         PACA_MobileNo = c.PACA_MobileNo,
         //                                         FYP_ReceiptDate = a.FYP_ReceiptDate,
         //                                         PACA_ApplicationNo = c.PACA_ApplicationNo,
         //                                         PACA_emailId = c.PACA_emailId,
         //                                         UserId = a.User_Id
         //                                     }
         //).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }


                data.academicyrlist = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).ToArray();

                //data.specialheaddetails = (from a in _YearlyFeeGroupMappingContext.feespecialHead
                //                           from b in _YearlyFeeGroupMappingContext.feeSGGG
                //                           from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                //                           where (a.MI_Id == data.MI_Id && a.FMSFH_ActiceFlag == true && a.FMSFH_Id == b.FMSFH_Id && b.FMSFHFH_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == b.FMH_Id)//&& a.IVRMSTAUL_Id==data.User_Id
                //                           select new FeeSpecialFeeGroupDTO
                //                           {
                //                               FMSFH_Id = a.FMSFH_Id,
                //                               FMSFH_Name = a.FMSFH_Name,
                //                               FMSFHFH_Id = b.FMSFHFH_Id,
                //                               FMH_ID = b.FMH_Id,
                //                               FMH_Name = c.FMH_FeeName
                //                           }).Distinct().ToArray();

                //var specialheadlist = _YearlyFeeGroupMappingContext.feespecialHead.Where(t => t.MI_Id == data.MI_Id && t.FMSFH_ActiceFlag == true).Distinct().ToList();
                //data.specialheadlist = specialheadlist.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public CollegeFeeTransactionDTO selectstu(CollegeFeeTransactionDTO data)
        {
            try
            {
                string alltrids = "0";
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                if (data.Grp_Term_flg == "T")
                {
                    data.termlist = (from a in _YearlyFeeGroupMappingContext.feeTr
                                     where (a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true)
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FMT_Id = a.FMT_Id,
                                         FMT_Name = a.FMT_Name,
                                         FMT_Order = a.FMT_Order
                                     }).Distinct().OrderBy(T => T.FMT_Order).ToList().ToArray();

                    var readterms = (from a in _YearlyFeeGroupMappingContext.feeTr
                                     where (a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true)
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FMT_Id = a.FMT_Id,
                                         FMT_Name = a.FMT_Name,
                                         FMT_Order = a.FMT_Order
                                     }).Distinct().ToArray();

                    for (int s = 0; s < readterms.Count(); s++)
                    {
                        alltrids = alltrids + ',' + readterms[s].FMT_Id.ToString();
                    }

                }

                if (data.Grp_Term_flg == "G")
                {
                    var readterms = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeYearGroupDMO
                                     where (a.MI_Id == data.MI_Id && a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && (a.FMG_CompulsoryFlag == "1" && a.FMG_RegNewFlg == "N"))
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FMG_Id = a.FMG_Id,
                                         FMG_GroupName = a.FMG_GroupName,

                                     }).Distinct().ToArray();

                    data.termlist = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeYearGroupDMO
                                     where (a.MI_Id == data.MI_Id && a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true && (a.FMG_CompulsoryFlag == "1" && a.FMG_RegNewFlg == "N"))
                                     select new CollegeFeeTransactionDTO
                                     {
                                         FMG_Id = a.FMG_Id,
                                         FMG_GroupName = a.FMG_GroupName,

                                     }).Distinct().ToList().ToArray();

                    for (int s = 0; s < readterms.Count(); s++)
                    {
                        alltrids = alltrids + ',' + readterms[s].FMG_Id.ToString();
                    }
                }

                if (data.filterinitialdata == "Preadmission")
                {
                    data.preadmissionstudentlist = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                                    from b in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                                    from c in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                                    from d in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                                    where (a.AMCO_Id == b.AMCO_Id && a.AMB_Id == c.AMB_Id && a.AMSE_Id == d.AMSE_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id  && a.PACA_Id == data.PACA_Id) /*&& a.PASR_Adm_Confirm_Flag == false*/
                                                    select new CollegeFeeTransactionDTO
                                                    {
                                                        PACA_Id = a.PACA_Id,
                                                        PACA_FirstName = ((a.PACA_FirstName == null || a.PACA_FirstName == "" ? "" : " " + a.PACA_FirstName) + (a.PACA_MiddleName == null || a.PACA_MiddleName == "" || a.PACA_MiddleName == "0" ? "" : " " + a.PACA_MiddleName) + (a.PACA_LastName == null || a.PACA_LastName == "" || a.PACA_LastName == "0" ? "" : " " + a.PACA_LastName)).Trim(),
                                                        AMCO_CourseName = b.AMCO_CourseName,
                                                        AMB_BranchName = c.AMB_BranchName,
                                                        AMSE_SEMName = d.AMSE_SEMName,
                                                        PACA_ApplicationNo = a.PACA_ApplicationNo,
                                                        PACA_MobileNo = a.PACA_MobileNo,
                                                        PACA_emailId = a.PACA_emailId,
                                                        fathername = (a.PACA_FatherName + ' ' + (a.PACA_FatherSurname == null || a.PACA_FatherSurname == "0" ? "" : a.PACA_FatherSurname)),
                                                        PACA_RegistrationNo = a.PACA_RegistrationNo,
                                                    }
 ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }
                try
                {
                    using (var cmd1 = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "gettermsstatisticPreadmissiondetails_College";
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
                            Value = data.PACA_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@fmtids",
                         SqlDbType.VarChar)
                        {
                            Value = alltrids
                        });

                        cmd1.Parameters.Add(new SqlParameter("@flag",
                       SqlDbType.VarChar)
                        {
                            Value = data.Grp_Term_flg
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

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public CollegeFeeTransactionDTO selectgrouppterm(CollegeFeeTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                data.Bankname = (from a in _YearlyFeeGroupMappingContext.Fee_Master_BankDMO
                                 where (a.MI_Id == data.MI_Id && a.FMBANK_ActiveFlg == true)
                                 select new CollegeFeeTransactionDTO
                                 {
                                     FMBANK_Id = a.FMBANK_Id,
                                     FMBANK_BankName = a.FMBANK_BankName,
                                 }
  ).Distinct().ToArray();

                if (data.filterinitialdata == "Preadmission")
                {
                    var saved_fma = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                     from b in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                     from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                     from d in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                     where (a.FYP_Id == b.FYP_Id && a.PACA_Id == data.PACA_Id && c.FCMA_Id==d.FCMA_Id && d.FCMAS_Id == b.FCMAS_Id && b.FTCP_PaidAmount >= d.FCMAS_Amount)
                                     select b.FCMAS_Id
).Distinct().ToList();


                    data.feepaiddetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                           from b in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                           where (a.FYP_Id == b.FYP_Id && a.PACA_Id == data.PACA_Id)
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FCMAS_Id = b.FCMAS_Id,
                                               FTCP_PaidAmount = b.FTCP_PaidAmount
                                           }
    ).Distinct().ToArray();

                    var fetchclass = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                      where (a.MI_Id == data.MI_Id && a.PACA_Id == data.PACA_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new CollegeFeeTransactionDTO
                                      {
                                          AMCO_Id = a.AMCO_Id,
                                          ASMAY_Id = a.ASMAY_Id,
                                          AMSE_Id=a.AMSE_Id,
                                          PACA_Id = a.PACA_Id
                                      }
    ).Distinct().ToArray();

                    long semid = 0;
                    for (int s = 0; s < fetchclass.Count(); s++)
                    {
                        semid = fetchclass[s].AMSE_Id;
                    }


                    int grpset = 0;
                    var feemasnum = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToArray();
                    for (int s = 0; s < feemasnum.Count(); s++)
                    {
                        grpset = Convert.ToInt16(feemasnum[s].fee_group_setting);
                    }

                    if (data.Grp_Term_flg.Equals("G"))
                    {
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                               from b in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                               from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                               from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                               where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && c.FCMA_Id==e.FCMA_Id && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(c.FMG_Id) && e.AMSE_Id== semid && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && !saved_fma.Contains(e.FCMAS_Id) && ((e.FCMAS_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) && (d.FMG_CompulsoryFlag == "1" || d.FMG_RegNewFlg == "N"))
                                               select new Head_Installments_College_DTO
                                               {
                                                   FCMAS_Id = e.FCMAS_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = b.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = Convert.ToInt64(e.FCMAS_Amount),
                                                   TotalCharges = Convert.ToInt64(e.FCMAS_Amount),
                                                   ConcessionAmount = 0,
                                                   FineAmount = 0,
                                                   ToBePaid = Convert.ToInt64(e.FCMAS_Amount),
                                                   NetAmount = Convert.ToDecimal(e.FCMAS_Amount),
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = d.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();


                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                              from b in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                              from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                              from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                              from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                              where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && c.FCMA_Id==e.FCMA_Id && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true && !saved_fma.Contains(e.FCMAS_Id) && ((e.FCMAS_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")))
                                              select new Head_Installments_College_DTO
                                              {
                                                  FTI_Name = b.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();
                    }
                    else if (data.Grp_Term_flg.Equals("T"))
                    {
                            data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                                   from b in _YearlyFeeGroupMappingContext.feeMTH
                                                   from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                                   from e in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                   from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                   where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && c.FCMA_Id == f.FCMA_Id && c.AMB_Id == data.AMB_Id && c.AMCO_Id == Convert.ToInt16(data.AMCO_Id) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(f.FCMAS_Id) && ((f.FCMAS_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) && (e.FMG_CompulsoryFlag == "1" || e.FMG_RegNewFlg == "N") && c.ASMAY_Id == c.ASMAY_Id)
                                                   select new Head_Installments_College_DTO
                                                   {
                                                       FCMAS_Id = f.FCMAS_Id,
                                                       FMH_FeeName = a.FMH_FeeName,
                                                       FTI_Name = d.FTI_Name,
                                                       FMH_Id = c.FMH_Id,
                                                       FTI_Id = c.FTI_Id,
                                                       CurrentYrCharges = Convert.ToInt64(f.FCMAS_Amount),
                                                       TotalCharges = Convert.ToInt64(f.FCMAS_Amount),
                                                       ConcessionAmount = 0,
                                                       FineAmount = 0,
                                                       ToBePaid = Convert.ToInt64(f.FCMAS_Amount),
                                                       NetAmount = Convert.ToDecimal(f.FCMAS_Amount),
                                                       FMG_Id = c.FMG_Id,
                                                       FMG_GroupName = e.FMG_GroupName,
                                                       FMH_Order = a.FMH_Order
                                                   }).Distinct().OrderBy(t => t.FMH_Order).ToList().ToArray();

                        data.instalspecial = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                              from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                              from b in _YearlyFeeGroupMappingContext.feeMTH
                                              from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                              from e in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                              from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                              where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && c.FCMA_Id == f.FCMA_Id && c.AMB_Id == data.AMB_Id && c.AMCO_Id == Convert.ToInt16(data.AMCO_Id) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(f.FCMAS_Id) && ((f.FCMAS_Amount > 0 && (a.FMH_Flag != "F" || a.FMH_Flag != "E")) || (a.FMH_Flag == "F" || a.FMH_Flag == "E")) && c.ASMAY_Id == c.ASMAY_Id)
                                              select new Head_Installments_College_DTO
                                              {
                                                  FTI_Name = d.FTI_Name,
                                                  FTI_Id = c.FTI_Id
                                              }).Distinct().ToList().ToArray();
                    }

                    if (data.mapped_hds_ins.Length > 0)
                    {

                        var count_res = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                         from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                         from b in _YearlyFeeGroupMappingContext.feeMTH
                                         from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                         from e in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                         from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                         where (a.FMH_Id == c.FMH_Id && e.FMG_Id == c.FMG_Id && c.FTI_Id == b.FTI_Id && d.FTI_Id == c.FTI_Id && b.FMH_Id == a.FMH_Id && e.FMG_ActiceFlag == true && c.FCMA_Id == f.FCMA_Id && c.AMB_Id == data.AMB_Id && c.AMCO_Id == Convert.ToInt16(data.AMCO_Id) && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && data.terms_groups.Contains(b.FMT_Id) && !saved_fma.Contains(f.FCMAS_Id) && (e.FMG_CompulsoryFlag == "1") && c.ASMAY_Id == c.ASMAY_Id)
                                         select new
                                         {
                                             FMG_Id = e.FMG_Id,
                                             FMH_Id = a.FMH_Id

                                         }).Distinct().GroupBy(id => id.FMG_Id).Select(g => new CollegeFeeTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).OrderByDescending(t => t.grp_count).ToList();

                        //var count_res1 = _YearlyFeeGroupMappingContext.V_StudentPendingDMO.Where(r => r.mi_id == data.MI_Id).Select(r => r.fmg_id).GroupBy(id => id).OrderByDescending(id => id.Count()).Select(g => new FeeStudentTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).First();

                        //var count_res = _YearlyFeeGroupMappingContext.V_StudentPendingDMO.Where(r => r.mi_id == data.MI_Id).Select(r => r.fmg_id).GroupBy(id => id).OrderByDescending(id => id.Count()).Select(g => new FeeStudentTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).First();

                        if(count_res.Count>0)
                        {
                            data.validationgroupid = count_res[0].FMG_Id;
                            data.validationgrougidcount = count_res[0].grp_count;
                        }
                    }
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
                    foreach (var item in data.head_installments)
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

        public CollegeFeeTransactionDTO savedata(CollegeFeeTransactionDTO data)
        {
            try
            {
                string headflg = "";
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                var makerchecker = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).Select(d => d.FMC_MakerCheckerReqdFlg).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                if (data.auto_receipt_flag == 1)
                {
                    if (data.FYP_ReceiptNo == null || data.FYP_ReceiptNo == "")
                    {
                        data.temp_head_list = data.savetmpdata;
                        CollegeFeeTransactionDTO new_obj = get_grp_reptno(data);
                        data.FYP_ReceiptNo = new_obj.FYP_ReceiptNo;
                    }
                }

                if (data.FYP_ReceiptNo == "" || data.FYP_ReceiptNo == null)
                {
                    data.returnval = "Record Not Saved because Receipt No is not Generated Automatic.Settings are missing";
                    return data;
                }
                else
                {
                    Fee_Y_PaymentDMO obj1 = new Fee_Y_PaymentDMO();
                    obj1.MI_Id = data.MI_Id;
                    obj1.ASMAY_Id = data.ASMAY_Id;
                    obj1.FYP_Currency = "1";
                    obj1.FYP_DOE = DateTime.Now;
                    obj1.FYP_ReceiptDate = data.FYP_ReceiptDate;
                    obj1.FYP_ReceiptNo = data.FYP_ReceiptNo;
                    obj1.FYP_PayModeType = "Single";
                    obj1.FYP_TransactionTypeFlag = data.FYP_TransactionTypeFlag;
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

                    if(makerchecker==true)
                    {
                        obj1.FYP_ApprovedFlg = false;
                    }
                    else if (makerchecker == false)
                    {
                        obj1.FYP_ApprovedFlg = true;
                    }

                    _YearlyFeeGroupMappingContext.Add(obj1);

                    Fee_Y_Payment_PaymentModeDMO obj2 = new Fee_Y_Payment_PaymentModeDMO();
                    obj2.FYP_Id = obj1.FYP_Id;
                    obj2.FYPPM_TransactionTypeFlag = data.FYP_TransactionTypeFlag;
                    obj2.FYPPM_TotalPaidAmount = data.FYP_TotalPaidAmount;
                    obj2.FYPPM_LedgerId = 0;
                    obj2.FYPPM_BankName = data.FYP_TransactionTypeFlag == "C" ? "" : data.FYPPM_BankName;
                    obj2.FYPPM_DDChequeNo = data.FYP_TransactionTypeFlag == "C" ? "" : data.FYPPM_DDChequeNo;
                    obj2.FYPPM_DDChequeDate = data.FYP_TransactionTypeFlag == "C" ? data.FYP_ReceiptDate : data.FYPPM_DDChequeDate;
                    obj2.FYPPM_Transaction_Id = "";
                    obj2.FYPPM_PaymentReference_Id = "";
                    obj2.FYPPM_ClearanceStatusFlag = (data.FYP_TransactionTypeFlag == "C" || data.FYP_TransactionTypeFlag == "S" || data.FYP_TransactionTypeFlag == "R" || data.FYP_TransactionTypeFlag == "U") ? "1" : "0";
                    obj2.FYPPM_ClearanceDate = (data.FYP_TransactionTypeFlag == "C" || data.FYP_TransactionTypeFlag == "S" || data.FYP_TransactionTypeFlag == "R" || data.FYP_TransactionTypeFlag == "U") ? data.FYP_ReceiptDate : data.FYPPM_ClearanceDate;
                    _YearlyFeeGroupMappingContext.Add(obj2);

                    //added on 02-07-2018

                    List<long> HeadId = new List<long>();
                    foreach (var item in data.head_installments)
                    {
                        HeadId.Add(item.FMH_Id);
                    }

                    List<CollegeFeeTransactionDTO> grps = new List<CollegeFeeTransactionDTO>();
                    grps = (from b in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping
                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))
                            select new CollegeFeeTransactionDTO
                            {
                                FMG_Id = b.FMG_Id
                            }
                            ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    string groupidss = "0";
                    foreach (var item in grps)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    Fee_Y_Payment_PA_Application obj_pay_stf = new Fee_Y_Payment_PA_Application();

                    var getdetails1 = (from a in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                       from b in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                       from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        where (a.FMH_Id == c.FMH_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FCMA_Id==b.FCMA_Id && b.FCMAS_Id == data.head_installments[0].FCMAS_Id)
                                        select new CollegeFeeTransactionDTO
                                        {
                                            headflag = c.FMH_Flag,
                                            headorder = c.FMH_Order
                                        }
        ).Distinct().OrderBy(t => t.headorder).ToList();

                    if (getdetails1.Count > 0)
                    {
                        headflg = getdetails1.FirstOrDefault().headflag;
                        if (headflg != "R")
                        {
                            headflg = "A";
                        }
                        else
                        {
                            headflg = "R";
                        }
                    }
                    else if (getdetails1.Count == 0)
                    {
                        data.returnval = "Kindly contact Administrator!!";
                        return data;
                    }

                    obj_pay_stf.FYP_Id = obj1.FYP_Id;
                    obj_pay_stf.PACA_Id = data.PACA_Id;
                    obj_pay_stf.FYPPA_Type = headflg;
                    obj_pay_stf.FYPPA_TotalPaidAmount = Convert.ToInt64(obj1.FYP_TotalPaidAmount);

                    _YearlyFeeGroupMappingContext.Add(obj_pay_stf);

                    for (int l = 0; l < data.head_installments.Length; l++)
                    {
                        if (data.head_installments[l].ToBePaid > 0)
                        {
                            Fee_T_College_PaymentDMO obj_trans_st = new Fee_T_College_PaymentDMO();
                            obj_trans_st.FYP_Id = obj1.FYP_Id;
                            obj_trans_st.FCMAS_Id = data.head_installments[l].FCMAS_Id;
                            obj_trans_st.FTCP_PaidAmount = data.head_installments[l].ToBePaid;
                            obj_trans_st.FTCP_WaivedAmount = 0;
                            obj_trans_st.FTCP_ConcessionAmount = data.head_installments[l].ConcessionAmount;
                            obj_trans_st.FTCP_FineAmount = data.head_installments[l].FineAmount;
                            obj_trans_st.FTCP_Remarks = obj1.FYP_Remarks;

                            _YearlyFeeGroupMappingContext.Add(obj_trans_st);
                        }
                    }

                    var feepartialpaymentflag = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).Select(d => d.FMC_Partial_Pre_Payment_flag).FirstOrDefault();

                    if (headflg != "R" && feepartialpaymentflag != "P")
                    {
                        var resultupda = _YearlyFeeGroupMappingContext.PA_College_Application.Where(t => t.PACA_Id == Convert.ToInt64(data.PACA_Id)).ToArray();

                        resultupda[0].PACA_FinalpaymentFlag = true;
                        _YearlyFeeGroupMappingContext.Update(resultupda[0]);
                    }

                    var contactexisttransaction = 0;
                    using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    {
                        try
                        {
                            contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                            dbCtxTxn.Commit();
                            data.returnval = "Save";

                            string MailId = "";
                            long mobileno = 0;
                            var getdetails = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                              where (a.PACA_Id == data.PACA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                                select new CollegeFeeTransactionDTO
                                                {
                                                    PACA_MobileNo = a.PACA_MobileNo,
                                                    PACA_emailId = a.PACA_emailId
                                                }
        ).ToList();

                            mobileno = getdetails.FirstOrDefault().PACA_MobileNo;
                            MailId = getdetails.FirstOrDefault().PACA_emailId;

                            if (mobileno != 0)
                            {
                                SMS sms = new SMS(_context);
                                //string s = sms.sendSms(data.MI_Id, mobileno, "FEEPREADMISSION", data.PACA_Id);
                            }

                            if (MailId != "" || MailId != null)
                            {
                                Email Email = new Email(_context);
                                //string m = Email.sendmail(data.MI_Id, MailId, "FEEPREADMISSION", data.PACA_Id);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            dbCtxTxn.Rollback();
                            data.returnval = "Cancel";
                        }
                    }

                }
                
            }

            catch (Exception ee)
            {
                data.returnval = "Error";
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return data;
        }

        public CollegeFeeTransactionDTO Printterm(CollegeFeeTransactionDTO data)
        {
            try
            {


                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                if (data.filterinitialdata == "Preadmission")
                {

                    data.username = _YearlyFeeGroupMappingContext.applicationUser.Where(d => d.Id == data.UserId).Select(t => t.NormalizedUserName).FirstOrDefault();

                    using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fetch_Final_Level_Approval_Name_Preadmission";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@PACA_Id", SqlDbType.VarChar) { Value = data.PACA_Id });
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
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                              from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                              from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                              from h in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                              from e in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                              //from f in _YearlyFeeGroupMappingContext.feespecialHead
                                              //from g in _YearlyFeeGroupMappingContext.feeSGGG
                                              where (d.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == h.FCMAS_Id && d.FCMA_Id==h.FCMA_Id && d.FMH_Id == e.FMH_Id && b.PACA_Id == data.PACA_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id)
                                              select new CollegeFeeTransactionDTO
                                              {
                                                  FMH_FeeName = e.FMH_FeeName,
                                                  FMH_Id = e.FMH_Id,
                                                  FTCP_PaidAmount = c.FTCP_PaidAmount,
                                                  FTCP_ConcessionAmount = c.FTCP_ConcessionAmount,
                                                  FTCP_FineAmount = c.FTCP_FineAmount,
                                                  FTI_Id = d.FTI_Id,
                                                  FCMAS_Amount = h.FCMAS_Amount,
                                                  FYP_TransactionTypeFlag=a.FYP_TransactionTypeFlag
                                              }
          ).ToArray();

                    data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                   from n in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                                   from e in _YearlyFeeGroupMappingContext.PA_College_Application
                                                   from f in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                                   from h in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                   from i in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                                   from j in _YearlyFeeGroupMappingContext.AcademicYear
                                                   from k in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                                   from l in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                                   from m in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                                   from o in _YearlyFeeGroupMappingContext.PA_College_Student_CEMarksClgDMO
                                                   where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && i.MI_ID == a.MI_Id && j.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && a.FYP_Id==n.FYP_Id && b.PACA_Id == e.PACA_Id && f.AMCO_Id == e.AMCO_Id && e.AMB_Id==l.AMB_Id && e.AMSE_Id==m.AMSE_Id && d.FCMA_Id==k.FCMA_Id && c.FCMAS_Id == k.FCMAS_Id && d.FMH_Id == h.FMH_Id && d.FTI_Id == i.FTI_Id && a.ASMAY_Id == j.ASMAY_Id && d.ASMAY_Id == a.ASMAY_Id && j.ASMAY_Id == a.ASMAY_Id && e.PACA_Id==o.PACA_Id)
                                                   select new CollegeFeeTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       FYP_Receipt_No = a.FYP_ReceiptNo,
                                                       FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                       FYP_TransactionTypeFlag = a.FYP_TransactionTypeFlag,
                                                       FYP_Bank_Name = n.FYPPM_BankName,
                                                       FYP_DD_Cheque_No = n.FYPPM_DDChequeNo,
                                                       FYP_DD_Cheque_Date = n.FYPPM_DDChequeDate,
                                                       FYP_Remarks = a.FYP_Remarks,
                                                       FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                       FCMAS_Amount = Convert.ToInt64(k.FCMAS_Amount),
                                                       FTCP_PaidAmount = Convert.ToDecimal(c.FTCP_PaidAmount),
                                                       FTCP_ConcessionAmount = Convert.ToDecimal(c.FTCP_ConcessionAmount),
                                                       PACA_FirstName = ((e.PACA_FirstName == null ? " " : e.PACA_FirstName) + " " + (e.PACA_MiddleName == null ? " " : e.PACA_MiddleName) + " " + (e.PACA_LastName == null ? " " : e.PACA_LastName)).Trim(),
                                                       PACA_ApplicationNo = e.PACA_ApplicationNo,
                                                       AMCO_CourseName = f.AMCO_CourseName,
                                                       AMB_BranchName = l.AMB_BranchName,
                                                       AMSE_SEMName = m.AMSE_SEMName,
                                                       FMH_Id = d.FMH_Id,
                                                       FTI_Id = d.FTI_Id,
                                                       ASMAY_Id = a.ASMAY_Id,
                                                       PACA_Id = e.PACA_Id,
                                                       FMH_FeeName = h.FMH_FeeName,
                                                       FTI_Name = i.FTI_Name,
                                                       ASMAY_Year = j.ASMAY_Year,
                                                       fathername = (e.PACA_FatherName + ' ' + (e.PACA_FatherSurname == null || e.PACA_FatherSurname == "0" ? "" : e.PACA_FatherSurname)),
                                                       PACA_MotherName = e.PACA_MotherName,
                                                       FYPPM_Transaction_Id=n.FYPPM_Transaction_Id,
                                                       FYPPM_PaymentReference_Id=n.FYPPM_PaymentReference_Id,
                                                       PACA_RegistrationNo=e.PACA_RegistrationNo,
                                                       FYPPM_ClearanceDate=n.FYPPM_ClearanceDate,
                                                       AMCST_NEETRN=o.PACSTCEM_RollNo,
                                                   }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeFeeTransactionDTO printrec(CollegeFeeTransactionDTO data)
        {
            try
            {
                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                string accountname = "", accesskey = "", html = "";
                var datatstu = _context.IVRM_Storage_path_Details.ToList();
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }
                //try
                //{
                //    html = obj.getHtmlContentFromAzure(accountname, accesskey, "feereceipt/" + data.MI_Id, "PreadmissionRec.html", 0);
                //}
                //catch (Exception ex)
                //{ html = ""; }

                List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                   from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                   from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                   from f in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                   where (a.PACA_Id == data.PACA_Id && a.FYP_Id == c.FYP_Id && d.FCMA_Id==e.FCMA_Id && c.FCMAS_Id == e.FCMAS_Id && f.FMG_Id==d.FMG_Id && f.FMG_CompulsoryFlag=="1" && f.FMG_RegNewFlg=="N" && a.FYP_Id == data.FYP_Id && d.MI_Id == data.MI_Id)
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
                            html = obj.getHtmlContentFromAzure(accountname, accesskey, "feereceipt/" + data.MI_Id, "PreadmissionRec.html", 0);
                        }
                    }
                }
                catch (Exception ex)
                { html = ""; }


                data.htmldata = html;

                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                if (data.minstall == "0")
                {
                    Printterm(data);
                }
                else
                {
                    if (data.filterinitialdata == "Preadmission")
                    {

                        data.username = _YearlyFeeGroupMappingContext.applicationUser.Where(d => d.Id == data.UserId).Select(t => t.NormalizedUserName).FirstOrDefault();

                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Fetch_Final_Level_Approval_Name_Preadmission";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@PACA_Id", SqlDbType.VarChar) { Value = data.PACA_Id });
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

                        data.receiptdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                               from n in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                               from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                               from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                               from e in _YearlyFeeGroupMappingContext.PA_College_Application
                                               from f in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                               from h in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                               from i in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                               from j in _YearlyFeeGroupMappingContext.AcademicYear
                                               from k in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                               from l in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                               from m in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                               from o in _YearlyFeeGroupMappingContext.PA_College_Student_CEMarksClgDMO
                                               where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && i.MI_ID == a.MI_Id && j.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && a.FYP_Id==n.FYP_Id && c.FYP_Id == a.FYP_Id && b.PACA_Id == e.PACA_Id && f.AMCO_Id == e.AMCO_Id && e.AMB_Id==l.AMB_Id && e.AMSE_Id==m.AMSE_Id && c.FCMAS_Id == k.FCMAS_Id && d.FMH_Id == h.FMH_Id && d.FTI_Id == i.FTI_Id && a.ASMAY_Id == j.ASMAY_Id && d.ASMAY_Id == a.ASMAY_Id && j.ASMAY_Id == a.ASMAY_Id && e.PACA_Id==o.PACA_Id)
                                               select new CollegeFeeTransactionDTO
                                               {
                                                   FYP_Id = a.FYP_Id,
                                                   FYP_ReceiptNo = a.FYP_ReceiptNo,
                                                   FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                   FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                   FYPPM_BankName = n.FYPPM_BankName,
                                                   FYP_DD_Cheque_No = n.FYPPM_DDChequeNo,
                                                   FYP_DD_Cheque_Date = n.FYPPM_DDChequeDate,
                                                   FYP_Remarks = a.FYP_Remarks,
                                                   FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                   FCMAS_Amount = Convert.ToInt64(k.FCMAS_Amount),
                                                   FTCP_PaidAmount = Convert.ToDecimal(c.FTCP_PaidAmount),
                                                   FTCP_ConcessionAmount = Convert.ToDecimal(c.FTCP_ConcessionAmount),
                                                   PACA_FirstName = ((e.PACA_FirstName == null ? " " : e.PACA_FirstName) + " " + (e.PACA_MiddleName == null ? " " : e.PACA_MiddleName) + " " + (e.PACA_LastName == null ? " " : e.PACA_LastName)).Trim(),
                                                   PACA_ApplicationNo = e.PACA_ApplicationNo,
                                                   AMCO_CourseName = f.AMCO_CourseName,
                                                   AMB_BranchName = l.AMB_BranchName,
                                                   AMSE_SEMName = m.AMSE_SEMName,
                                                   FMH_Id = d.FMH_Id,
                                                   FTI_Id = d.FTI_Id,
                                                   ASMAY_Id = a.ASMAY_Id,
                                                   PACA_Id = e.PACA_Id,
                                                   FMH_FeeName = h.FMH_FeeName,
                                                   FTI_Name = i.FTI_Name,
                                                   ASMAY_Year = j.ASMAY_Year,
                                                   PACA_RegistrationNo=e.PACA_RegistrationNo,
                                                   AMCST_FatherName = e.PACA_FatherName,
                                                   AMCST_NEETRN = o.PACSTCEM_RollNo
                                               }).Distinct().ToArray();
                    }
                }
            } 
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public CollegeFeeTransactionDTO search(CollegeFeeTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.ASMAY_Id = Acdemic_preadmission;

                if (data.filterinitialdata == "Preadmission")
                {
                    data.searchfilter = data.searchfilter.ToUpper();
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && ((a.PACA_FirstName.Trim().ToUpper() + ' ' + a.PACA_MiddleName.Trim().ToUpper() + ' ' + a.PACA_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || (a.PACA_FirstName.Trim().ToUpper() + a.PACA_MiddleName.Trim().ToUpper() + ' ' + a.PACA_LastName.Trim().ToUpper()).StartsWith(data.searchfilter) || a.PACA_FirstName.ToUpper().StartsWith(data.searchfilter) || a.PACA_MiddleName.ToUpper().StartsWith(data.searchfilter) || a.PACA_LastName.ToUpper().StartsWith(data.searchfilter))) /*&& a.PASR_Adm_Confirm_Flag == false*/
                                        select new CollegeFeeTransactionDTO
                                        {
                                            PACA_Id = a.PACA_Id,
                                            PACA_MiddleName = a.PACA_MiddleName,
                                            PACA_LastName = a.PACA_LastName,

                                            PACA_FirstName = ((a.PACA_FirstName == null || a.PACA_FirstName == "" ? "" : " " + a.PACA_FirstName) + (a.PACA_MiddleName == null || a.PACA_MiddleName == "" || a.PACA_MiddleName == "0" ? "" : " " +   a.PACA_MiddleName) + (a.PACA_LastName == null || a.PACA_LastName == "" || a.PACA_LastName == "0" ? "" : " " + a.PACA_LastName)).Trim(),
                                        }
           ).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeFeeTransactionDTO searchfilter(CollegeFeeTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                data.ASMAY_Id = Acdemic_preadmission;

                if (data.filterinitialdata == "Preadmission")
                {
                    switch (data.searchType)
                    {

                        case "0":
                            string str = "";
                            data.searchtext = data.searchtext.ToUpper();
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                                from c in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                from d in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                                from e in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                                from f in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                                where (a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id && a.AMSE_Id == f.AMSE_Id && b.FYP_Id == c.FYP_Id && b.PACA_Id == a.PACA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && (((a.PACA_FirstName.Trim().ToUpper() + ' ' + (string.IsNullOrEmpty(a.PACA_MiddleName.Trim().ToUpper()) == true ? str : a.PACA_MiddleName.Trim().ToUpper())).Trim() + ' ' + (string.IsNullOrEmpty(a.PACA_LastName.Trim().ToUpper()) == true ? str : a.PACA_LastName.Trim().ToUpper())).Trim().Contains(data.searchtext) || a.PACA_FirstName.ToUpper().StartsWith(data.searchtext) || a.PACA_MiddleName.ToUpper().StartsWith(data.searchtext) || a.PACA_LastName.ToUpper().StartsWith(data.searchtext)))
                                                select new CollegeFeeTransactionDTO
                                                {
                                                    PACA_Id = a.PACA_Id,
                                                    PACA_FirstName = ((a.PACA_FirstName == null || a.PACA_FirstName == "" ? "" : " " + a.PACA_FirstName) + (a.PACA_MiddleName == null || a.PACA_MiddleName == "" || a.PACA_MiddleName == "0" ? "" : " " + a.PACA_MiddleName) + (a.PACA_LastName == null || a.PACA_LastName == "" || a.PACA_LastName == "0" ? "" : " " + a.PACA_LastName)).Trim(),
                                                    PACA_MiddleName = a.PACA_MiddleName,
                                                    PACA_LastName = a.PACA_LastName,
                                                    AMCO_CourseName = d.AMCO_CourseName,
                                                    AMB_BranchName = e.AMB_BranchName,
                                                    AMSE_SEMName = f.AMSE_SEMName,
                                                    FYP_Receipt_No = c.FYP_ReceiptNo,
                                                    FYP_Bank_Or_Cash = c.FYP_TransactionTypeFlag,
                                                    FYP_TotalPaidAmount = c.FYP_TotalPaidAmount,
                                                    FYP_Id = c.FYP_Id,
                                                    PACA_MobileNo = a.PACA_MobileNo,
                                                    FYP_ReceiptDate = c.FYP_ReceiptDate,
                                                    PACA_ApplicationNo = a.PACA_ApplicationNo,
                                                    PACA_emailId = a.PACA_emailId
                                                }
                  ).Distinct().OrderBy(t => t.PACA_FirstName).ToArray();
                            break;

                        case "1":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                                from c in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                from d in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                                from e in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                                from f in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                                where (a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id && a.AMSE_Id == f.AMSE_Id && b.FYP_Id == c.FYP_Id && b.PACA_Id == a.PACA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PACA_ApplicationNo.Contains(data.searchtext))
                                                select new CollegeFeeTransactionDTO
                                                {
                                                    PACA_Id = a.PACA_Id,
                                                    PACA_FirstName = ((a.PACA_FirstName == null || a.PACA_FirstName == "" ? "" : " " + a.PACA_FirstName) + (a.PACA_MiddleName == null || a.PACA_MiddleName == "" || a.PACA_MiddleName == "0" ? "" : " " + a.PACA_MiddleName) + (a.PACA_LastName == null || a.PACA_LastName == "" || a.PACA_LastName == "0" ? "" : " " + a.PACA_LastName)).Trim(),
                                                    PACA_MiddleName = a.PACA_MiddleName,
                                                    PACA_LastName = a.PACA_LastName,
                                                    AMCO_CourseName = d.AMCO_CourseName,
                                                    AMB_BranchName = e.AMB_BranchName,
                                                    AMSE_SEMName = f.AMSE_SEMName,
                                                    FYP_Receipt_No = c.FYP_ReceiptNo,
                                                    FYP_Bank_Or_Cash = c.FYP_TransactionTypeFlag,
                                                    FYP_TotalPaidAmount = c.FYP_TotalPaidAmount,
                                                    FYP_Id = c.FYP_Id,
                                                    PACA_MobileNo = a.PACA_MobileNo,
                                                    FYP_ReceiptDate = c.FYP_ReceiptDate,
                                                    PACA_ApplicationNo = a.PACA_ApplicationNo,
                                                    PACA_emailId = a.PACA_emailId
                                                }
                ).Distinct().OrderBy(t => t.PACA_ApplicationNo).ToArray();
                            break;

                        case "2":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                                from c in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                from d in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                                from e in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                                from f in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                                where (a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id && a.AMSE_Id == f.AMSE_Id && b.FYP_Id == c.FYP_Id && b.PACA_Id == a.PACA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.PACA_emailId.Contains(data.searchtext))
                                                select new CollegeFeeTransactionDTO
                                                {
                                                    PACA_Id = a.PACA_Id,
                                                    PACA_FirstName = ((a.PACA_FirstName == null || a.PACA_FirstName == "" ? "" : " " + a.PACA_FirstName) + (a.PACA_MiddleName == null || a.PACA_MiddleName == "" || a.PACA_MiddleName == "0" ? "" : " " + a.PACA_MiddleName) + (a.PACA_LastName == null || a.PACA_LastName == "" || a.PACA_LastName == "0" ? "" : " " + a.PACA_LastName)).Trim(),
                                                    PACA_MiddleName = a.PACA_MiddleName,
                                                    PACA_LastName = a.PACA_LastName,
                                                    AMCO_CourseName = d.AMCO_CourseName,
                                                    AMB_BranchName = e.AMB_BranchName,
                                                    AMSE_SEMName = f.AMSE_SEMName,
                                                    FYP_Receipt_No = c.FYP_ReceiptNo,
                                                    FYP_Bank_Or_Cash = c.FYP_TransactionTypeFlag,
                                                    FYP_TotalPaidAmount = c.FYP_TotalPaidAmount,
                                                    FYP_Id = c.FYP_Id,
                                                    PACA_MobileNo = a.PACA_MobileNo,
                                                    FYP_ReceiptDate = c.FYP_ReceiptDate,
                                                    PACA_ApplicationNo = a.PACA_ApplicationNo,
                                                    PACA_emailId = a.PACA_emailId
                                                }
                ).Distinct().ToArray();
                            break;

                        case "3":
                            data.searcharray = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                                from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                                from c in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                from d in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                                from e in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                                from f in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                                where (a.AMCO_Id == d.AMCO_Id && a.AMB_Id == e.AMB_Id && a.AMSE_Id == f.AMSE_Id && b.FYP_Id == c.FYP_Id && b.PACA_Id == a.PACA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.FYP_ReceiptNo.Contains(data.searchtext))
                                                select new CollegeFeeTransactionDTO
                                                {
                                                    PACA_Id = a.PACA_Id,
                                                    PACA_FirstName = ((a.PACA_FirstName == null || a.PACA_FirstName == "" ? "" : " " + a.PACA_FirstName) + (a.PACA_MiddleName == null || a.PACA_MiddleName == "" || a.PACA_MiddleName == "0" ? "" : " " + a.PACA_MiddleName) + (a.PACA_LastName == null || a.PACA_LastName == "" || a.PACA_LastName == "0" ? "" : " " + a.PACA_LastName)).Trim(),
                                                    PACA_MiddleName = a.PACA_MiddleName,
                                                    PACA_LastName = a.PACA_LastName,
                                                    AMCO_CourseName = d.AMCO_CourseName,
                                                    AMB_BranchName = e.AMB_BranchName,
                                                    AMSE_SEMName = f.AMSE_SEMName,
                                                    FYP_Receipt_No = c.FYP_ReceiptNo,
                                                    FYP_Bank_Or_Cash = c.FYP_TransactionTypeFlag,
                                                    FYP_TotalPaidAmount = c.FYP_TotalPaidAmount,
                                                    FYP_Id = c.FYP_Id,
                                                    PACA_MobileNo = a.PACA_MobileNo,
                                                    FYP_ReceiptDate = c.FYP_ReceiptDate,
                                                    PACA_ApplicationNo = a.PACA_ApplicationNo,
                                                    PACA_emailId = a.PACA_emailId
                                                }
                ).Distinct().ToArray();
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public CollegeFeeTransactionDTO recnogen(CollegeFeeTransactionDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeFeeTransactionDTO delrec(CollegeFeeTransactionDTO data)
        {
            try
            {
                if (data.filterinitialdata == "Preadmission")
                {
                    var lorg1 = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    var lorg2 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    var lorg3 = _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    var lorg5 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                    if (lorg1.Count() > 0 && lorg2.Count() > 0 && lorg3.Count() > 0)
                    {
                        _YearlyFeeGroupMappingContext.Remove(lorg2[0]);
                        _YearlyFeeGroupMappingContext.Remove(lorg1[0]);

                        for (int i = 0; i < lorg3.Count(); i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(lorg3[i]);
                        }

                        foreach (var c in lorg5)
                        {
                            var checkresult = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO.Single(a => a.FYPPM_Id == c.FYPPM_Id);
                            _YearlyFeeGroupMappingContext.Remove(checkresult);
                        }

                        var contactexisttransaction = 0;
                        using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                        {
                            try
                            {
                                contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                                dbCtxTxn.Commit();
                                data.returnval = "Delete";
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                dbCtxTxn.Rollback();
                                data.returnval = "Cancel";
                            }
                        }
                    }

                    else
                    {
                        data.returnval = "Error";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeFeeTransactionDTO filstude(CollegeFeeTransactionDTO data)
        {
            try
            {
                var Acdemic_preadmission = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                data.ASMAY_Id = Acdemic_preadmission;
                if (data.filterinitialdata == "Preadmission")
                {
                    data.fillstudent = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id  && ((a.PACA_FirstName.Trim() + ' ' + a.PACA_MiddleName.Trim() + ' ' + a.PACA_LastName.Trim()).StartsWith(data.searchfilter) || (a.PACA_FirstName.Trim() + a.PACA_MiddleName.Trim() + ' ' + a.PACA_LastName.Trim()).StartsWith(data.searchfilter) || a.PACA_FirstName.StartsWith(data.searchfilter) || a.PACA_MiddleName.StartsWith(data.searchfilter) || a.PACA_LastName.StartsWith(data.searchfilter)))
                                        select new CollegeFeeTransactionDTO
                                        {
                                            PACA_Id = a.PACA_Id,
                                            PACA_MiddleName = a.PACA_MiddleName,
                                            PACA_LastName = a.PACA_LastName,
                                            PACA_FirstName = ((a.PACA_FirstName == null || a.PACA_FirstName == "" ? "" : " " + a.PACA_FirstName) + (a.PACA_MiddleName == null || a.PACA_MiddleName == "" || a.PACA_MiddleName == "0" ? "" : " " + a.PACA_MiddleName) + (a.PACA_LastName == null || a.PACA_LastName == "" || a.PACA_LastName == "0" ? "" : " " + a.PACA_LastName)).Trim()
                                        }
           ).ToArray();
                    //&& a.PASR_Adm_Confirm_Flag == false

                    data.preadmissionstudentlist = (from a in _YearlyFeeGroupMappingContext.PA_College_Application
                                                    from b in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                                    from c in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                                    from d in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                                    where (a.AMCO_Id == b.AMCO_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id) 
                                                    //&& a.PASR_Adm_Confirm_Flag == false
                                                    select new CollegeFeeTransactionDTO
                                                    {
                                                        PACA_Id = a.PACA_Id,
                                                        PACA_MiddleName = a.PACA_MiddleName,
                                                        PACA_LastName = a.PACA_LastName,
                                                        PACA_FirstName = a.PACA_FirstName,
                                                        AMCO_CourseName = b.AMCO_CourseName,
                                                        AMB_BranchName = c.AMB_BranchName,

                                                        AMSE_SEMName = d.AMSE_SEMName,
                                                        PACA_RegistrationNo = a.PACA_RegistrationNo,
                                                    }
  ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();


                    var fetchmaxfypid = _YearlyFeeGroupMappingContext.Fee_Y_Payment_Preadmission_ApplicationDMO.OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                    data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PA_Application
                                              from c in _YearlyFeeGroupMappingContext.PA_College_Application
                                              from d in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                              from e in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                              from f in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                              where (a.FYP_Id == b.FYP_Id && b.PACA_Id == c.PACA_Id && c.AMCO_Id == d.AMCO_Id && c.AMB_Id == e.AMB_Id && c.AMSE_Id == f.AMSE_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                              select new CollegeFeeTransactionDTO
                                              {
                                                  PACA_Id = c.PACA_Id,
                                                  PACA_FirstName = ((c.PACA_FirstName == null || c.PACA_FirstName == "" ? "" : " " + c.PACA_FirstName) + (c.PACA_MiddleName == null || c.PACA_MiddleName == "" || c.PACA_MiddleName == "0" ? "" : " " + c.PACA_MiddleName) + (c.PACA_LastName == null || c.PACA_LastName == "" || c.PACA_LastName == "0" ? "" : " " + c.PACA_LastName)).Trim(),
                                                  PACA_MiddleName = c.PACA_MiddleName,
                                                  PACA_LastName = c.PACA_LastName,
                                                  AMCO_CourseName = d.AMCO_CourseName,
                                                  AMB_BranchName = e.AMB_BranchName,
                                                  AMSE_SEMName = f.AMSE_SEMName,
                                                  FYP_Receipt_No = a.FYP_ReceiptNo,
                                                  FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                  FYP_TotalPaidAmount = a.FYP_TotalPaidAmount,
                                                  FYP_Id = a.FYP_Id,
                                                  PACA_MobileNo = c.PACA_MobileNo,
                                                  FYP_ReceiptDate = a.FYP_ReceiptDate,
                                                  PACA_ApplicationNo = c.PACA_ApplicationNo,
                                                  PACA_emailId = c.PACA_emailId
                                              }
         ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

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
