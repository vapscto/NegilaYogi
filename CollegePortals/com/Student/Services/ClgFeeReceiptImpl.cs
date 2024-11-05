using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Fees;
using CommonLibrary;
using System.Globalization;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using PreadmissionDTOs.com.vaps.College.Fees;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vaps.Fee;

namespace CollegePortals.com.vaps.Student.Services
{
    public class ClgFeeReceiptImpl : Interfaces.ClgFeeReceiptInterface
    {
        private static ConcurrentDictionary<string, ClgPortalFeeDTO> _login =
           new ConcurrentDictionary<string, ClgPortalFeeDTO>();
        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        private CollegeportalContext _ClgPortalContext;
        public ClgFeeReceiptImpl(CollegeportalContext ClgPortalContext, CollFeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context)
        {
            _ClgPortalContext = ClgPortalContext;
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
        }

        public ClgPortalFeeDTO getloaddata(ClgPortalFeeDTO data)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = allyear.Distinct().ToArray();

                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.feeconfiglist = feemasnum.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClgPortalFeeDTO getrecdetails(ClgPortalFeeDTO data)
        {
            try
            {
                data.recnolist = (from a in _ClgPortalContext.Fee_Y_PaymentDMO
                                  from b in _ClgPortalContext.Fee_Y_Payment_College_StudentDMO
                                  where (a.FYP_Id == b.FYP_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMCST_Id == data.AMCST_Id)
                                  orderby a.FYP_ReceiptNo
                                  select new ClgPortalFeeDTO
                                  {
                                      FYP_Id = a.FYP_Id,
                                      FYP_ReceiptNo = a.FYP_ReceiptNo
                                  }
                 ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeFeeTransactionDTO Printterm(CollegeFeeTransactionDTO data)
        {
            try
            {

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

                var findfmgid = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                 from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                 from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == e.FCMAS_Id && e.FCMA_Id == d.FCMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                 select new CollegeFeeTransactionDTO
                                 {
                                     FMG_Id = d.FMG_Id,
                                     FMH_Id = d.FMH_Id
                                 }).Distinct().ToArray();

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
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FYP_Id == a.FYP_Id
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FTCP_PaidAmount = a.FYP_TotalPaidAmount,
                                               FTCP_ConcessionAmount = b.FYPCS_TotalConcessionAmount,
                                               FYP_ReceiptDate = a.FYP_ReceiptDate,
                                               // FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                               // FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               //  FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               //   FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,
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
                //                                from i in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                //                                from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                //                                    // from k in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                //                                where (a.FYP_Id == b.FYP_Id && a.FCMAS_Id == l.FCMAS_Id && c.FCMA_Id == l.FCMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMCST_Id == j.AMCST_Id && f.AMCO_Id == g.AMCO_Id && f.AMB_Id == h.AMB_Id && i.AMCST_Id == j.AMCST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMCST_Id == data.AMCST_Id && b.FYP_Id == data.FYP_Id && f.ACYST_ActiveFlag == 1)//i.AMST_Concession_Type == k.FMCC_Id &&
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
                //                                    ACYST_RollNo = f.ACYST_RollNo,
                //                                    AMCST_AdmNo = i.AMCST_AdmNo,
                //                                    AMCST_FatherName = i.AMCST_FatherName,
                //                                    AMCST_MotherName = i.AMCST_MotherName,
                //                                    //FYP_Bank_Or_Cash = b.FYP_Bank_Or_Cash,
                //                                    //FYP_DD_Cheque_No = b.FYP_DD_Cheque_No,
                //                                    //FYP_DD_Cheque_Date = b.FYP_DD_Cheque_Date,
                //                                    //FYP_Bank_Name = b.FYP_Bank_Name,
                //                                    FYP_Remarks = b.FYP_Remarks,
                //                                    AMCST_RegistrationNo = i.AMCST_RegistrationNo,
                //                                    // FMCC_ConcessionName = k.FMCC_ConcessionName,
                //                                    FCSS_TotalCharges = Convert.ToInt64(l.FCMAS_Amount),
                //                                }
                //).Distinct().ToArray();

                data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                               from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                               from l in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                               from d in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                               from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                               from f in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                               from g in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                               from h in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                               from i in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                               from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                                    from k in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                               where (a.FYP_Id == b.FYP_Id && a.FCMAS_Id == l.FCMAS_Id && c.FCMA_Id == l.FCMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMCST_Id == j.AMCST_Id && f.AMCO_Id == g.AMCO_Id && f.AMB_Id == h.AMB_Id && i.AMCST_Id == j.AMCST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMCST_Id == data.AMCST_Id && b.FYP_Id == data.FYP_Id && f.ACYST_ActiveFlag == 1)//i.AMST_Concession_Type == k.FMCC_Id &&
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
                                                   FYP_Bank_Or_Cash = b.FYP_TransactionTypeFlag,
                                                   FYP_DD_Cheque_No = k.FYPPM_DDChequeNo,
                                                   FYP_DD_Cheque_Date = k.FYPPM_DDChequeDate,
                                                   //FYP_Bank_Name = b.FYP_Bank_Name,
                                                   FYP_Remarks = b.FYP_Remarks,
                                                   AMCST_RegistrationNo = i.AMCST_RegistrationNo,
                                                   // FMCC_ConcessionName = k.FMCC_ConcessionName,
                                                   FCSS_TotalCharges = Convert.ToInt64(l.FCMAS_Amount),
                                               }
               ).Distinct().ToArray();


                data.filltotaldetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                         from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                         where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id)
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
                //long fmt_id_new = 0;
                //long initialfmtids = Convert.ToInt64(feeterm[0].FMT_Id);
                //fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;

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
                                   where (a.AMCST_Id == data.AMCST_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == e.FCMAS_Id && e.FCMA_Id == d.FCMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
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
                        // var nextyr = curyear.Year - 1;
                        data.year = curyear.Year.ToString();
                        // data.year = nextyr.ToString();
                    }
                }

                string maxmonth = "", monthnameinitial = "", monthnameend = "";
                if (months1.Count() > 0)
                {
                    data.month = months1.Min().ToString();
                    // maxmonth = (Convert.ToInt32(data.month) - 1).ToString();
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
                            // data.year = curyear.Year.ToString();
                        }
                        else
                        {
                            maxmonth = startperiod.Max().ToString();
                            var curyear = DateTime.Now;
                            data.year = curyear.Year.ToString();
                            // var nextyr = curyear.Year - 1;
                            //data.year = nextyr.ToString();
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

                //var termperiodlist = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == initialfmtids).ToArray();

                //monthnameinitial = termperiodlist[0].FromMonth.ToString();
                //monthnameend = termperiodlist[0].ToMonth.ToString();

                var termperiodlistint = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                var termperiodlistend = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                monthnameinitial = termperiodlistint[0].FromMonth.ToString();
                monthnameend = termperiodlistend[0].ToMonth.ToString();


                string yeardisplay = "0";
                //if (monthnameend == "Jan" || monthnameend == "Feb" || monthnameend == "Mar")
                //{
                //    var curyear = DateTime.Now;
                //    var nextyr = curyear.Year ;
                //    yeardisplay = nextyr.ToString();
                //}
                //else
                //{
                //    var curyear = DateTime.Now;
                //    var nextyr = curyear.Year - 1;
                //    yeardisplay = nextyr.ToString();

                //    //var curyear = DateTime.Now;
                //    //yeardisplay = curyear.Year.ToString();
                //}

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

                //old one
                //   data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                //                     from b in _YearlyFeeGroupMappingContext.feeMTH
                //                     from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                     where (b.FMH_Id==c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMCST_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag!="E")
                //                     select new CollegeFeeTransactionDTO
                //                     {
                //                         FSS_ToBePaid = a.FSS_ToBePaid
                //                     }
                //).Sum(t => t.FSS_ToBePaid);

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
                                          where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == h.FCMAS_Id && h.FCMA_Id == d.FCMA_Id && d.FMH_Id == e.FMH_Id && b.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id)
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
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == e.FCMAS_Id && d.FCMA_Id == e.FCMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
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
                                    where c.FMT_Id == d.FMT_Id && a.FCMAS_Id == e.FCMAS_Id && e.FCMA_Id == b.FCMA_Id && b.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id
                                    select new CollegeFeeTransactionDTO
                                    {
                                        FMT_Name = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FYP_Id == a.FYP_Id
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FTCP_PaidAmount = a.FYP_TotalPaidAmount,
                                               FTCP_ConcessionAmount = b.FYPCS_TotalConcessionAmount,
                                               FYP_ReceiptDate = a.FYP_ReceiptDate,
                                               FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                               //FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               //FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               //FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,
                                           }
               ).Distinct().ToArray();

                //data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                //                               from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                //                               from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                //                               from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                               from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                //                               from f in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                //                               from g in _YearlyFeeGroupMappingContext.admissioncls
                //                               from h in _YearlyFeeGroupMappingContext.school_M_Section
                //                               from i in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                //                               from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                //                               from k in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                //                               from l in _YearlyFeeGroupMappingContext.feeMI 
                //                               where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && e.FMI_Id==l.FMI_Id  && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.AMCST_Id && b.FYP_Id == data.FYP_Id)
                //                               group new { a,b, c,d,f,g,h,i,j,k,l } by new {  a.FTCP_PaidAmount, a.FTCP_ConcessionAmount, a.FTCP_FineAmount, b.FYP_ReceiptNo, b.FYP_ReceiptDate, b.FYP_Bank_Or_Cash, b.FYP_DD_Cheque_No, b.FYP_DD_Cheque_Date, b.FYP_Bank_Name, b.FYP_Remarks, d.FMH_Id, d.FMH_FeeName, f.AMST_Id, f.AMAY_RollNo, g.ASMCL_ClassName, h.ASMC_SectionName, i.AMCST_FirstName, i.AMCST_MiddleName, i.AMCST_LastName, i.AMCST_AdmNo, i.AMST_FatherName, i.AMST_MotherName, i.AMST_RegistrationNo, k.FMCC_ConcessionName,l.FMI_Id,l.FMI_Installment_Type } into grp
                //                               select new CollegeFeeTransactionDTO
                //                               {
                //                                   AMCST_Id = grp.Key.AMST_Id,
                //                                   AMCST_FirstName =grp.Key.AMCST_FirstName,
                //                                   AMCST_MiddleName=grp.Key.AMCST_MiddleName,
                //                                   AMCST_LastName=grp.Key.AMCST_LastName,
                //                                   FMH_Id=grp.Key.FMH_Id,
                //                                   FMH_FeeName=grp.Key.FMH_FeeName,
                //                                   FYP_ReceiptNo=grp.Key.FYP_ReceiptNo,
                //                                   FTCP_PaidAmount = grp.Sum(x => x.a.FTCP_PaidAmount),
                //                                   FTCP_ConcessionAmount = grp.Sum(y => y.a.FTCP_ConcessionAmount),
                //                                   FTCP_FineAmount = grp.Sum(z => z.a.FTCP_FineAmount),
                //                                   FYP_ReceiptDate=grp.Key.FYP_ReceiptDate,
                //                                   classname=grp.Key.ASMCL_ClassName,
                //                                   sectionname = grp.Key.ASMC_SectionName,
                //                                   rollno = grp.Key.AMAY_RollNo,
                //                                   admno = grp.Key.AMCST_AdmNo,
                //                                   fathername = grp.Key.AMST_FatherName,
                //                                   mothername = grp.Key.AMST_MotherName,
                //                                   FYP_Bank_Or_Cash = grp.Key.FYP_Bank_Or_Cash,
                //                                   FYP_DD_Cheque_Date = grp.Key.FYP_DD_Cheque_Date,
                //                                   FYP_Bank_Name = grp.Key.FYP_Bank_Name,
                //                                   FYP_Remarks = grp.Key.FYP_Remarks,
                //                                   AMST_RegistrationNo = grp.Key.AMST_RegistrationNo,
                //                                   FMCC_ConcessionName = grp.Key.FMCC_ConcessionName,
                //                                   FMI_ID = grp.Key.FMI_Id,
                //                                   FMI_Installment_type = grp.Key.FMI_Installment_Type
                //                               }
                //            ).Distinct().ToArray();

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
                            //while (dataReader.Read())
                            //{
                            //    result.Add(new CollegeFeeTransactionDTO
                            //    {
                            //        //FMCC_ClassCategoryName = dataReader["FMCC_ClassCategoryName"].ToString(),
                            //        //ASMCL_ClassName = Convert.ToString(dataReader["ASMCL_ClassName"]),
                            //        //ASMC_SectionName = Convert.ToString(dataReader["ASMC_SectionName"]),

                            //        AMCST_Id = Convert.ToInt16(dataReader["AMCST_Id"]),
                            //        AMCST_FirstName = dataReader["AMCST_FirstName"].ToString(),
                            //        AMCST_MiddleName = dataReader["AMCST_MiddleName"].ToString(),
                            //        AMCST_LastName = dataReader["AMCST_LastName"].ToString(),
                            //        FMH_Id = Convert.ToInt16(dataReader["FMH_Id"]),
                            //        FMH_FeeName = dataReader["FMH_FeeName"].ToString(),
                            //        //FTI_Name = dataReader["FTI_Name"].ToString(),
                            //        //FTI_Id = Convert.ToInt16(dataReader["FTI_Id"]),
                            //        FYP_ReceiptNo = dataReader["FYP_ReceiptNo"].ToString(),
                            //        FTCP_PaidAmount =Convert.ToDecimal(dataReader["FTP_Paid_Amt"]),
                            //        FTCP_ConcessionAmount = Convert.ToDecimal(dataReader["FTP_Concession_Amt"]),
                            //        FTCP_FineAmount = Convert.ToDecimal(dataReader["FTP_Fine_Amt"]),
                            //        FYP_ReceiptDate = Convert.ToDateTime(dataReader["FYP_ReceiptDate"]),
                            //        classname = dataReader["ASMCL_ClassName"].ToString(),
                            //        sectionname = dataReader["ASMC_SectionName"].ToString(),
                            //        rollno = Convert.ToInt16(dataReader["AMAY_RollNo"]),
                            //        admno = dataReader["AMCST_AdmNo"].ToString(),
                            //        fathername = dataReader["AMST_FatherName"].ToString(),
                            //        mothername = dataReader["AMST_MotherName"].ToString(),
                            //        FYP_Bank_Or_Cash = dataReader["FYP_Bank_Or_Cash"].ToString(),
                            //        FYP_DD_Cheque_No = dataReader["FYP_DD_Cheque_No"].ToString(),
                            //        FYP_DD_Cheque_Date = Convert.ToDateTime(dataReader["FYP_DD_Cheque_Date"]),
                            //        FYP_Bank_Name = dataReader["FYP_Bank_Name"].ToString(),
                            //        FYP_Remarks = dataReader["FYP_Remarks"].ToString(),
                            //        AMST_RegistrationNo = dataReader["AMST_RegistrationNo"].ToString(),
                            //        FMCC_ConcessionName = dataReader["AMCST_LastName"].ToString(),

                            //    });
                            //    data.fillstudentviewdetails = retObject.ToArray();
                            //}
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
                                         where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id)
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

                //var feeterm = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                //               from b in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                //               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                //               from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                //               from e in _YearlyFeeGroupMappingContext.feeMTH
                //               from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //               where ( e.FMH_Id==f.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.AMCST_Id && a.FYP_Id == data.FYP_Id && d.FSS_PaidAmount>0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                //               select new CollegeFeeTransactionDTO
                //               {
                //                   FMT_Id = e.FMT_Id,
                //                   fmt_order = g.FMT_Order,
                //                   FMT_Year = g.FMT_Year
                //               }

                // ).Distinct().ToArray();

                //long fmt_id_new = 0;

                //long initialfmtids = Convert.ToInt64(feeterm[0].FMT_Id);

                //fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;


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
                                   where (a.AMCST_Id == data.AMCST_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == e.FCMAS_Id && e.FCMA_Id == d.FCMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
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


                //var termperiodlist = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == initialfmtids).ToArray();

                //monthnameinitial = termperiodlist[0].FromMonth.ToString();
                //monthnameend = termperiodlist[0].ToMonth.ToString();

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


                //commented on 04-01-2018
                //      data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                //                        from b in _YearlyFeeGroupMappingContext.feeMTH
                //                        from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                        where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMCST_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag != "E")
                //                        select new CollegeFeeTransactionDTO
                //                        {
                //                            FSS_ToBePaid = a.FSS_ToBePaid
                //                        }
                //).Sum(t => t.FSS_ToBePaid);

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
                //MB for clg

                var payment_modes = _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO.Where(t => t.FYP_Id == data.FYP_Id).Distinct().ToList();
                data.paymentmode_details = payment_modes.ToArray();
                data.ASMAY_Year = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).Select(t => t.ASMAY_Year).FirstOrDefault();
                //MB
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
                    //html = obj.getHtmlContentFromAzure(accountname, accesskey, "feereceipt/" + data.MI_Id, "FeeReceipt.html", 0);
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
                //else { printcommon(data); }
                printcommon(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;


            //try
            //{
            //    ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
            //    string accountname = "", accesskey = "", html = "";
            //    var datatstu = _context.IVRM_Storage_path_Details.ToList();
            //    if (datatstu.Count() > 0)
            //    {
            //        accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
            //        accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
            //    }


            //    data.specialheaddetails = (from a in _YearlyFeeGroupMappingContext.feespecialHead
            //                               from b in _YearlyFeeGroupMappingContext.feeSGGG
            //                               from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
            //                               where (a.MI_Id == data.MI_Id && a.FMSFH_ActiceFlag == true && a.FMSFH_Id == b.FMSFH_Id && b.FMSFHFH_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == b.FMH_Id && c.FMH_SpecialFeeFlag == true)//&& a.IVRMSTAUL_Id==data.User_Id
            //                               select new FeeSpecialFeeGroupDTO
            //                               {
            //                                   FMSFH_Id = a.FMSFH_Id,
            //                                   FMSFH_Name = a.FMSFH_Name,
            //                                   FMSFHFH_Id = b.FMSFHFH_Id,
            //                                   FMH_ID = b.FMH_Id,
            //                                   FMH_Name = c.FMH_FeeName
            //                               }).Distinct().ToArray();

            //    var specialheadlist = _YearlyFeeGroupMappingContext.feespecialHead.Where(t => t.MI_Id == data.MI_Id && t.FMSFH_ActiceFlag == true).Distinct().ToList();
            //    data.specialheadlist = specialheadlist.ToArray();

            //    try
            //    {
            //        html = obj.getHtmlContentFromAzure(accountname, accesskey, "feereceipt/" + data.MI_Id, "PortalFeeReceipt.html", 0);
            //    }
            //    catch (Exception ex)
            //    { html = ""; }

            //    data.htmldata = html;

            //    if (html != "")
            //    {
            //        if (data.minstall == "0")
            //        {
            //            Printterm(data);
            //        }
            //        else
            //        {
            //            Printterm(data);
            //        }
            //    }
            //    else { printcommon(data); }

            //    var periodnme = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
            //                     from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
            //                     from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
            //                     from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
            //                     where (a.FMG_Id == c.FMG_Id && a.FMG_Id == d.FMG_Id && b.FMA_Id == c.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FYP_Id == data.FYP_Id && a.AMST_Id == data.Amst_Id)
            //                     select new CollegeFeeTransactionDTO
            //                     {
            //                         FMG_CompulsoryFlag = d.FMG_CompulsoryFlag,
            //                     }
            //   ).Distinct().OrderBy(t => t.fmt_order).ToArray();

            //    var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
            //                   from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
            //                   from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
            //                   from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
            //                   from e in _YearlyFeeGroupMappingContext.feeMTH
            //                   from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
            //                   from g in _YearlyFeeGroupMappingContext.feeTr
            //                   where (e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id && d.FSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
            //                   select new CollegeFeeTransactionDTO
            //                   {
            //                       fmt_id = e.FMT_Id,
            //                       fmt_order = g.FMT_Order,
            //                       FMT_Year = g.FMT_Year
            //                   }

            //    ).Distinct().OrderBy(t => t.fmt_order).ToArray();

            //    string startmonth = "";
            //    string endmonth = "";
            //    string year1 = "";
            //    string year2 = "";
            //    string fmt_id_int = "0", fmt_id_end = "0";

            //    for (int i = 0; i < feeterm.Length; i++)
            //    {
            //        fmt_id_int = feeterm[0].fmt_id.ToString();
            //        fmt_id_end = feeterm[feeterm.Length - 1].fmt_id.ToString();
            //    }


            //    //startmonth = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_int) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_FROM_MONTH;

            //    //endmonth = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_end) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_TO_MONTH;

            //    //year1 = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_int) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_Year;

            //    //year2 = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_end) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_Year;

            //    //data.duration = startmonth + '-' + year1 + '/' + endmonth + '-' + year2;

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //return data;

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
                                   from k in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                              where (a.FYP_Id == b.FYP_Id && a.FCMAS_Id == o.FCMAS_Id && o.FCMA_Id == c.FCMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMCST_Id == j.AMCST_Id && f.AMCO_Id == g.AMCO_Id && f.AMB_Id == h.AMB_Id && i.AMCST_Id == j.AMCST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMCST_Id == data.AMCST_Id && b.FYP_Id == data.FYP_Id && f.ACYST_ActiveFlag == 1 && b.FYP_Id==k.FYP_Id)
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
                                       FYP_Bank_Or_Cash = b.FYP_TransactionTypeFlag,
                                       FYP_DD_Cheque_No = k.FYPPM_DDChequeNo,
                                      FYP_DD_Cheque_Date = k.FYPPM_DDChequeDate,
                                       //FYP_Bank_Name = b.FYP_Bank_Name,
                                       FYP_Remarks = b.FYP_Remarks,
                                       AMCST_RegistrationNo = i.AMCST_RegistrationNo,
                                       // FMCC_ConcessionName = k.FMCC_ConcessionName,

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
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FYP_Id == a.FYP_Id
                                           select new CollegeFeeTransactionDTO
                                           {
                                               FTCP_PaidAmount = a.FYP_TotalPaidAmount,
                                               FTCP_ConcessionAmount = b.FYPCS_TotalConcessionAmount,
                                               FTCP_FineAmount = a.FYP_TotalFineAmount,
                                               FTCP_WaivedAmount = b.FYPCS_TotalWaivedAmount,
                                               FYP_ReceiptDate = a.FYP_ReceiptDate,
                                               FYP_Bank_Or_Cash = a.FYP_PayModeType,
                                               //FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               //FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               //FYP_Bank_Name = a.FYP_Bank_Name,
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
                //string fmt_id_new = '0';

                //for(int r=0;r<feeterm.Length;r++)
                //{
                //    fmt_id_new= fmt_id_new + ',' + feeterm[0].fmt_id
                //}

                long initialfmtids = Convert.ToInt64(feeterm[0].FMT_Id);

                fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;
                //fmt_id_new = 10;


                List<CollegeFeeTransactionDTO> temp_group_head = new List<CollegeFeeTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                   from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                   from e in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                   where (a.AMCST_Id == data.AMCST_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == e.FCMAS_Id && e.FCMA_Id == d.FCMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
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
                    // maxmonth = (Convert.ToInt32(data.month) - 1).ToString();
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

                // data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                //                   from b in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                //                   where (b.FTDD_Day == data.date && b.FTDD_Month == data.month && a.FMA_Id == b.FMA_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMCST_Id && grp_ids.Contains(a.FMG_Id) && head_ids.Contains(a.FMH_Id) && !inst_ids.Contains(a.FTI_Id))
                //                   select new CollegeFeeTransactionDTO
                //                   {
                //                       FSS_ToBePaid = a.FSS_ToBePaid
                //                   }
                //).Sum(t => t.FSS_ToBePaid);


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


        public string duadatecollect(long asmayid, long userid, long asmclid)
        {
            string date = "";
            List<FeetransactionSMS> Due_Date_array = new List<FeetransactionSMS>();
            List<FeetransactionSMS> result_duadate = new List<FeetransactionSMS>();
            using (var cmdnew = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
            {
                cmdnew.CommandText = "DUE_DATE_CALCULATION";
                cmdnew.CommandType = CommandType.StoredProcedure;
                cmdnew.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = asmayid });
                cmdnew.Parameters.Add(new SqlParameter("@FMT_Id", SqlDbType.VarChar) { Value = userid });
                cmdnew.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = asmclid });
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
            date = Due_Date_array[0].Due_Date;
            string v = date.Substring(0, 10);
            return v;
        }


    }
}