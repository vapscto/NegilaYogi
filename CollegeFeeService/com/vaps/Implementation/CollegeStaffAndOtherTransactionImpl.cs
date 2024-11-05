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
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using System.IO;
using System.Net;
using System.Text;
using MimeKit;
using MailKit.Net.Smtp;
using DomainModel.Model.com.vaps.admission;
using CommonLibrary;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SqlServer.Server;
using System.Globalization;
using System.Dynamic;
using Microsoft.AspNetCore.Http.Features;
using PreadmissionDTOs.com.vaps.College.Fees;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CollegeStaffAndOtherTransactionImpl:Interfaces.CollegeStaffAndOtherTransactionInterface
    {
        private static ConcurrentDictionary<string, CollegeStaffAndOtherTransactionDTO> _login =
   new ConcurrentDictionary<string, CollegeStaffAndOtherTransactionDTO>();

        private static readonly Object obj = new Object();

        public CollFeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        readonly ILogger<CollegeStaffAndOtherTransactionImpl> _logger;
        public CollegeStaffAndOtherTransactionImpl(CollFeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context, ILogger<CollegeStaffAndOtherTransactionImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
            _logger = log;
        }
       

      


        public CollegeStaffAndOtherTransactionDTO getdata(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id && t.userid == data.userid).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                List<FeeGroupClgDMO> group = new List<FeeGroupClgDMO>();
                group = _YearlyFeeGroupMappingContext.FeeGroupClgDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == data.MI_Id).ToList();
                data.fillmastergroup = group.ToArray();

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _YearlyFeeGroupMappingContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Transaction").ToList();
                data.transnumconfig = masnum.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.fillyear = allyear.Distinct().ToArray();

                var rolename = _YearlyFeeGroupMappingContext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.roleid).IVRMRT_Role;

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

                var fetchmaxfypid = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.User_Id == data.userid).OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

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
                                              FYP_ApprovedFlg = a.FYP_ApprovedFlg,
                                              UserId = a.User_Id
                                          }
).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

                data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                  where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                  select new Temp_Staff_DTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                  }
             ).OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

                data.oth_studentlist = _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO.Where(s => s.MI_Id == data.MI_Id && s.FMOST_ActiveFlag == true).ToList().Distinct().OrderBy(t => t.FMOST_Id).ToArray();

                data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                           from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                           from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                           from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                           from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                           where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.User_Id == data.userid && a.ASMAY_Id == data.ASMAY_Id)
                                           select new CollegeStaffAndOtherTransactionDTO
                                           {
                                               FYP_Id = a.FYP_Id,
                                               HRME_Id = d.HRME_Id,
                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                               HRMD_DepartmentName = e.HRMD_DepartmentName,
                                               HRMDES_DesignationName = f.HRMDES_DesignationName,
                                               FYP_Receipt_No = a.FYP_ReceiptNo,
                                               FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                               FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                               FYP_Date = a.FYP_ReceiptDate
                                           }).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                            from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                            from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                            where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMCOST_Id == d.FMOST_Id && d.FMOST_ActiveFlag == true && a.User_Id == data.userid && a.ASMAY_Id == data.ASMAY_Id)
                                            select new CollegeStaffAndOtherTransactionDTO
                                            {
                                                FYP_Id = a.FYP_Id,
                                                FMOST_Id = d.FMOST_Id,
                                                FMOST_StudentName = d.FMOST_StudentName,
                                                FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                FYP_Receipt_No = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                FYP_Date = a.FYP_ReceiptDate
                                            }).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeStaffAndOtherTransactionDTO Printterm(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {

                data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                          from e in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO

                                          from f in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                        
                                          where (d.FCMA_Id == e.FCMA_Id  && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == d.FCMAS_Id && e.FMH_Id == e.FMH_Id && b.AMCST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && e.MI_Id == data.MI_Id)
                                          select new CollegeStaffAndOtherTransactionDTO
                                          {
                                              FMH_FeeName = f.FMH_FeeName,
                                              FMH_Id = e.FMH_Id,
                                              FTP_Paid_Amt = c.FTCP_PaidAmount,
                                              FTP_Concession_Amt = c.FTCP_ConcessionAmount,
                                              FTP_Fine_Amt = c.FTCP_FineAmount,
                                              FTI_Id = e.FTI_Id,
                                              totalcharges = d.FCMAS_Amount
                                          }
              ).ToArray();

                var findfmgid = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                 from d in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                 from e in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == d.FCMAS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id && e.FCMA_Id == d.FCMA_Id)
                                 select new CollegeStaffAndOtherTransactionDTO
                                 {
                                     FMG_Id = e.FMG_Id,
                                     FMH_Id = e.FMH_Id
                                 }).Distinct().ToArray();

                data.termremarks = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                    from b in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                    from c in _YearlyFeeGroupMappingContext.feeMTH
                                    from d in _YearlyFeeGroupMappingContext.feeTr
                                    from e in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                    where b.FCMA_Id == e.FCMA_Id && c.FMT_Id == d.FMT_Id && a.FCMAS_Id == b.FCMAS_Id && e.FCMA_Id == b.FCMA_Id && e.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id
                                    select new CollegeStaffAndOtherTransactionDTO
                                    {
                                        termname = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                           where a.FYP_Id == data.FYP_Id && a.FYP_Id == b.FYP_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                           select new CollegeStaffAndOtherTransactionDTO
                                           {
                                               FTP_Paid_Amt = a.FYPPM_TotalPaidAmount,
                                               // FTP_Concession_Amt = a,
                                               FYP_Date = b.FYP_ReceiptDate,
                                               FYP_Bank_Or_Cash = a.FYPPM_TransactionTypeFlag,
                                               FYP_DD_Cheque_No = a.FYPPM_DDChequeNo,
                                               FYP_DD_Cheque_Date = a.FYPPM_DDChequeDate,
                                               FYP_Bank_Name = a.FYPPM_BankName,
                                               FYP_Remarks = b.FYP_Remarks,
                                           }
               ).Distinct().ToArray();


                data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                               from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                               from c in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                               from l in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                               from d in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                               from e in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                               from f in _YearlyFeeGroupMappingContext.Adm_College_Yearly_StudentDMO
                                               from g in _YearlyFeeGroupMappingContext.MasterCourseDMO
                                               from h in _YearlyFeeGroupMappingContext.ClgMasterBranchDMO
                                               from k in _YearlyFeeGroupMappingContext.CLG_Adm_Master_SemesterDMO
                                               from i in _YearlyFeeGroupMappingContext.Adm_Master_College_StudentDMO
                                               from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                               from m in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                               where (a.FYP_Id == b.FYP_Id && a.FCMAS_Id == l.FCMAS_Id && c.FCMA_Id == l.FCMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMCST_Id == j.AMCST_Id && f.AMCO_Id == g.AMCO_Id && f.AMB_Id == h.AMB_Id && f.AMSE_Id == k.AMSE_Id && f.ASMAY_Id == b.ASMAY_Id && i.AMCST_Id == j.AMCST_Id && m.AMCST_Id == f.AMCST_Id && f.ASMAY_Id == m.ASMAY_Id && a.FCMAS_Id == m.FCMAS_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMCST_Id == data.Amst_Id && b.FYP_Id == data.FYP_Id && f.ACYST_ActiveFlag == 1)
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
                                                   AMSE_SEMName = k.AMSE_SEMName,
                                                   ACYST_RollNo = f.ACYST_RollNo,
                                                   AMCST_AdmNo = i.AMCST_AdmNo,
                                                   AMCST_FatherName = i.AMCST_FatherName,
                                                   AMCST_MotherName = i.AMCST_MotherName,
                                                   FYP_Remarks = b.FYP_Remarks,
                                                   AMCST_RegistrationNo = i.AMCST_RegistrationNo,
                                                   FCSS_TotalCharges = Convert.ToInt64(l.FCMAS_Amount),
                                               }
               ).Distinct().ToArray();




                data.filltotaldetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                         where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id)
                                         select new CollegeStaffAndOtherTransactionDTO
                                         {
                                             FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                             // FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                             FYP_Tot_Fine_Amt = a.FYP_TotalFineAmount,
                                         }
              ).Distinct().ToArray();



                //to find next due amount
                var feeterm = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                               from b in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FCMAS_Id == d.FCMAS_Id && d.AMCST_Id == c.AMCST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.AMCST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id)
                               select new CollegeStaffAndOtherTransactionDTO
                               {
                                   //fmt_id = e.FMT_Id
                                   FMT_Id = e.FMT_Id
                               }

                 ).Distinct().OrderByDescending(t => t.FMT_Id).ToArray();

                long fmt_id_new = 0;


                long initialfmtids = Convert.ToInt64(feeterm[0].FMT_Id);

                fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;



                List<CollegeStaffAndOtherTransactionDTO> temp_group_head = new List<CollegeStaffAndOtherTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                   from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                   from e in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO

                                   where (d.FCMA_Id == e.FCMA_Id && a.AMCST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == d.FCMAS_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == e.ASMAY_Id && e.ASMAY_Id == data.ASMAY_Id)
                                   select new CollegeStaffAndOtherTransactionDTO
                                   {

                                       FMG_Id = e.FMG_Id,
                                       FMH_Id = e.FMH_Id,
                                       FTI_Id = e.FTI_Id

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

                List<CollegeStaffAndOtherTransactionDTO> fordate = new List<CollegeStaffAndOtherTransactionDTO>();
                List<CollegeStaffAndOtherTransactionDTO> fordateinfyp = new List<CollegeStaffAndOtherTransactionDTO>();

                fordate = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                           from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                           where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && (d.FCSS_ToBePaid > 0))
                           select new CollegeStaffAndOtherTransactionDTO
                           {
                               date = f.FCTDD_Day,
                               month = f.FCTDD_Month,
                           }
                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                                where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new CollegeStaffAndOtherTransactionDTO
                                {
                                    month = f.FCTDD_Month,
                                }
                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (CollegeStaffAndOtherTransactionDTO item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (CollegeStaffAndOtherTransactionDTO itemperiod in fordateinfyp)
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
                //                   from b in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                //                   where (b.FTDD_Day == data.date && b.FTDD_Month == data.month && a.FCMAS_Id == b.FCMAS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && head_ids.Contains(a.FMH_Id) && !inst_ids.Contains(a.FTI_Id))
                //                   select new CollegeStaffAndOtherTransactionDTO
                //                   {
                //                       FSS_ToBePaid = a.FSS_ToBePaid
                //                   }
                //).Sum(t => t.FSS_ToBePaid);


                data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag != "E")
                                  select new CollegeStaffAndOtherTransactionDTO
                                  {
                                      FSS_ToBePaid = a.FCSS_ToBePaid
                                  }
             ).Sum(t => t.FSS_ToBePaid);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffAndOtherTransactionDTO Printinstallment(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                          from e in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                          from i in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                              // from f in _YearlyFeeGroupMappingContext.feespecialHead
                                              // from g in _YearlyFeeGroupMappingContext.feeSGGG
                                          where (d.FCMA_Id == i.FCMA_Id  && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == d.FCMAS_Id && i.FMH_Id == e.FMH_Id && b.AMCST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && i.MI_Id == data.MI_Id)
                                          select new CollegeStaffAndOtherTransactionDTO
                                          {
                                              FMH_FeeName = e.FMH_FeeName,
                                              FMH_Id = e.FMH_Id,
                                              FTP_Paid_Amt = c.FTCP_PaidAmount,
                                              FTP_Concession_Amt = c.FTCP_ConcessionAmount,
                                              FTP_Fine_Amt = c.FTCP_FineAmount,
                                              FTI_Id = i.FTI_Id
                                          }
              ).ToArray();

                var findfmgid = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                 from d in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                 from e in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FCMAS_Id == d.FCMAS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id && e.FCMA_Id == d.FCMA_Id)
                                 select new CollegeStaffAndOtherTransactionDTO
                                 {
                                     FMG_Id = e.FMG_Id,
                                     FMH_Id = e.FMH_Id
                                 }).Distinct().ToArray();

                data.termremarks = (from a in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                    from b in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                    from e in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                    from c in _YearlyFeeGroupMappingContext.feeMTH
                                    from d in _YearlyFeeGroupMappingContext.feeTr
                                    where c.FMT_Id == d.FMT_Id && a.FCMAS_Id == b.FCMAS_Id && b.FCMA_Id == e.FCMA_Id && e.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id
                                    select new CollegeStaffAndOtherTransactionDTO
                                    {
                                        termname = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           select new CollegeStaffAndOtherTransactionDTO
                                           {
                                               FTP_Paid_Amt = a.FYP_TotalPaidAmount,
                                               // FTP_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                               FYP_Date = a.FYP_ReceiptDate,
                                               FYP_Bank_Or_Cash = b.FYPPM_TransactionTypeFlag,
                                               FYP_DD_Cheque_No = b.FYPPM_DDChequeNo,
                                               FYP_DD_Cheque_Date = b.FYPPM_DDChequeDate,
                                               FYP_Bank_Name = b.FYPPM_BankName,
                                               FYP_Remarks = a.FYP_Remarks,
                                           }
               ).Distinct().ToArray();






                List<CollegeStaffAndOtherTransactionDTO> result = new List<CollegeStaffAndOtherTransactionDTO>();

                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "installment_transaction_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@Amst_id", SqlDbType.VarChar) { Value = data.Amst_Id });
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
                                         where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id)
                                         select new CollegeStaffAndOtherTransactionDTO
                                         {
                                             FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                             //  FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                             FYP_Tot_Fine_Amt = a.FYP_TotalFineAmount,
                                         }
              ).Distinct().ToArray();




                var feeterm = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                               from b in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FCMAS_Id == d.FCMAS_Id && d.AMCST_Id == c.AMCST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.AMCST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id)
                               select new CollegeStaffAndOtherTransactionDTO
                               {

                                   FMT_Id = e.FMT_Id
                               }

                 ).Distinct().ToArray();


                long fmt_id_new = 0;

                long initialfmtids = Convert.ToInt64(feeterm[0].FMT_Id);

                fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;


                List<CollegeStaffAndOtherTransactionDTO> temp_group_head = new List<CollegeStaffAndOtherTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                   from c in _YearlyFeeGroupMappingContext.Fee_T_College_PaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.CLG_Fee_College_Master_Amount_Semesterwise
                                   from e in _YearlyFeeGroupMappingContext.Clg_Fee_AmountEntry_DMO
                                   where (d.FCMA_Id == e.FCMA_Id && a.AMCST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == d.FCMAS_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id)
                                   select new CollegeStaffAndOtherTransactionDTO
                                   {

                                       FMG_Id = e.FMG_Id,
                                       FMH_Id = e.FMH_Id,
                                       FTI_Id = e.FTI_Id

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

                List<CollegeStaffAndOtherTransactionDTO> fordate = new List<CollegeStaffAndOtherTransactionDTO>();
                List<CollegeStaffAndOtherTransactionDTO> fordateinfyp = new List<CollegeStaffAndOtherTransactionDTO>();

                fordate = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                           from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                           where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id))
                           select new CollegeStaffAndOtherTransactionDTO
                           {
                               date = f.FCTDD_Day,
                               month = f.FCTDD_Month,
                           }

                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                from f in _YearlyFeeGroupMappingContext.CLG_Fee_College_T_Due_DateDMO
                                where (d.FCMAS_Id == f.FCMAS_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMCST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new CollegeStaffAndOtherTransactionDTO
                                {
                                    month = f.FCTDD_Month,
                                }

                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (CollegeStaffAndOtherTransactionDTO item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (CollegeStaffAndOtherTransactionDTO itemperiod in fordateinfyp)
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

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;
                }


                var termperiodlist = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == initialfmtids).ToArray();

                monthnameinitial = termperiodlist[0].FromMonth.ToString();
                monthnameend = termperiodlist[0].ToMonth.ToString();

                data.duration = monthnameinitial + '-' + monthnameend + '-' + data.year;




                data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_StatusDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag != "E")
                                  select new CollegeStaffAndOtherTransactionDTO
                                  {
                                      FSS_ToBePaid = a.FCSS_ToBePaid
                                  }
          ).Sum(t => t.FSS_ToBePaid);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeStaffAndOtherTransactionDTO duplicaterecept(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                data.duplicatereceipt = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                         where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_ReceiptNo.Trim().Equals(data.FYP_Receipt_No.Trim()))
                                         select new CollegeStaffAndOtherTransactionDTO
                                         {
                                             FYP_Receipt_No = a.FYP_ReceiptNo
                                         }
              ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffAndOtherTransactionDTO get_grp_reptno(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                if (data.auto_receipt_flag == 1)
                {
                    List<long> HeadId = new List<long>();
                    List<long> groupid = new List<long>();
                    foreach (var item in data.head_installments)
                    {
                        HeadId.Add(item.FMH_Id);
                        groupid.Add(item.FMG_Id);
                    }

                    List<CollegeStaffAndOtherTransactionDTO> grps = new List<CollegeStaffAndOtherTransactionDTO>();
                    grps = (from b in _YearlyFeeGroupMappingContext.CLG_Fee_Yearly_Group_Head_Mapping

                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id) && groupid.Contains(b.FMG_Id))

                            select new CollegeStaffAndOtherTransactionDTO
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

                    for (int r = 0; r < grpid.Count(); r++)
                    {
                        groupidss = groupidss + ',' + grpid[r];
                    }

                    var final_rept_no = "";
                    List<CollegeStaffAndOtherTransactionDTO> list_all = new List<CollegeStaffAndOtherTransactionDTO>();
                    List<CollegeStaffAndOtherTransactionDTO> list_repts = new List<CollegeStaffAndOtherTransactionDTO>();

                    list_all = (from b in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceiptDMO
                                from c in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new CollegeStaffAndOtherTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    //FGAR_Name = b.FGAR_Name,
                                    //FMG_Id = c.FMG_Id
                                }
                         ).Distinct().ToList();

                    data.grp_count = list_all.Count();

                    if (data.grp_count >= 1)
                    {


                        using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
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

                        }

                        //data.auto_FYP_Receipt_No = final_rept_no;

                        //data.FYP_ReceiptNo = final_rept_no;
                    }
                }

                else if (data.automanualreceiptno == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.FYP_Receipt_No = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

      

        public CollegeStaffAndOtherTransactionDTO edittra(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                if (data.Grp_Term_flg == "T" && data.Stf_Others_flg == "Staff")
                {
                    data.disableterms = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                         from b in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                         from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                         from d in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                         where (a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && b.HRME_Id == c.HRME_Id && d.FYP_Id == c.FYP_Id && d.FMCAOST_Id == b.FMCAOST_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.HRME_Id == data.HRME_Id && d.FYP_Id == data.FYP_Id)
                                         select new CollegeStaffAndOtherTransactionDTO
                                         {
                                             FMG_Id = a.FMT_Id,
                                         }
                                        ).Distinct().ToArray();

                    data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.feeTr
                                            where (a.FMT_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                            select new CollegeStaffAndOtherTransactionDTO
                                            {
                                                FMT_Id = a.FMT_Id,
                                                FMT_Name = a.FMT_Name,
                                            }
             ).OrderBy(t => t.FMH_Id).ToArray();
                }
                else if (data.Grp_Term_flg == "G" && data.Stf_Others_flg == "Staff")
                {
                    data.disableterms = (from b in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                         from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                         from d in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                         where (b.HRME_Id == c.HRME_Id && d.FYP_Id == c.FYP_Id && d.FMCAOST_Id == b.FMCAOST_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.HRME_Id == data.HRME_Id && d.FYP_Id == data.FYP_Id)
                                         select new CollegeStaffAndOtherTransactionDTO
                                         {
                                             FMG_Id = b.FMG_Id,
                                         }
                                       ).Distinct().ToArray();

                    data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                            where (a.FMG_ActiceFlag == true && a.MI_Id == data.MI_Id && a.user_id == data.userid)
                                            select new CollegeStaffAndOtherTransactionDTO
                                            {
                                                FMG_Id = a.FMG_Id,
                                                FMG_GroupName = a.FMG_GroupName,
                                            }
             ).OrderBy(t => t.FMH_Id).ToArray();
                }

                if (data.Grp_Term_flg == "T" && data.Stf_Others_flg == "Others")
                {
                    data.disableterms = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                         from b in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                         from d in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                         where (a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && b.FMCOST_Id == c.FMCOST_Id && d.FYP_Id == c.FYP_Id && d.FMCAOST_Id == b.FMCAOST_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMCOST_Id == data.HRME_Id && d.FYP_Id == data.FYP_Id)
                                         select new CollegeStaffAndOtherTransactionDTO
                                         {
                                             FMG_Id = a.FMT_Id,
                                         }
                                        ).Distinct().ToArray();

                    data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.feeTr
                                            where (a.FMT_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                            select new CollegeStaffAndOtherTransactionDTO
                                            {
                                                FMT_Id = a.FMT_Id,
                                                FMT_Name = a.FMT_Name,
                                            }
             ).OrderBy(t => t.FMH_Id).ToArray();
                }
                else if (data.Grp_Term_flg == "G" && data.Stf_Others_flg == "Others")
                {
                    data.disableterms = (from b in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                         from d in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                         where (b.FMCOST_Id == c.FMCOST_Id && d.FYP_Id == c.FYP_Id && d.FMCAOST_Id == b.FMCAOST_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMCOST_Id == data.HRME_Id && d.FYP_Id == data.FYP_Id)
                                         select new CollegeStaffAndOtherTransactionDTO
                                         {
                                             FMG_Id = b.FMG_Id,
                                         }
                                       ).Distinct().ToArray();

                    data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                            where (a.FMG_ActiceFlag == true && a.MI_Id == data.MI_Id && a.user_id == data.userid)
                                            select new CollegeStaffAndOtherTransactionDTO
                                            {
                                                FMG_Id = a.FMG_Id,
                                                FMG_GroupName = a.FMG_GroupName,
                                            }
             ).OrderBy(t => t.FMH_Id).ToArray();
                }

                if (data.Stf_Others_flg == "Staff")
                {
                    data.receiparraydeleteall = (from a in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                 from c in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                                 from d in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                                 from e in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                 where (d.FMG_Id == e.FMG_Id && a.FMCAOST_Id == d.FMCAOST_Id && b.FMH_Id == d.FMH_Id && c.FTI_Id == d.FTI_Id && a.FYP_Id == data.FYP_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.HRME_Id == data.HRME_Id)
                                                 select new CollegeStaffAndOtherTransactionDTO
                                                 {
                                                     FMG_GroupName = e.FMG_GroupName,
                                                     FMH_FeeName = b.FMH_FeeName,
                                                     FTI_Name = c.FTI_Name,
                                                     FMH_Id = b.FMH_Id,
                                                     FTI_Id = c.FTI_Id,
                                                     FCMAS_Id = a.FMCAOST_Id,

                                                     totalamount = d.FCSSST_NetAmount,
                                                     ToBePaid = a.FTPOSTC_PaidAmount,
                                                     ConcessionAmount = d.FCSSST_ConcessionAmount,
                                                     FSS_FineAmount = d.FCSSST_FineAmount,
                                                     CurrentYrCharges = d.FCSSST_CurrentYrCharges,
                                                     totalcharges = d.FCSSST_TotalCharges,
                                                     FYP_Id = a.FYP_Id

                                                 }).ToArray();

                    data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                              from c in _YearlyFeeGroupMappingContext.MasterEmployee
                                              from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                              from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                              from g in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                              where (a.FYP_Id == g.FYP_Id && a.FYP_Id == b.FYP_Id && b.HRME_Id == c.HRME_Id && c.HRMD_Id == e.HRMD_Id && c.HRMDES_Id == f.HRMDES_Id && c.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id)
                                              select new CollegeStaffAndOtherTransactionDTO
                                              {
                                                  HRME_Id = c.HRME_Id,
                                                  HRME_EmployeeFirstName = c.HRME_EmployeeFirstName,
                                                  HRME_EmployeeMiddleName = c.HRME_EmployeeMiddleName,
                                                  HRME_EmployeeLastName = c.HRME_EmployeeLastName,
                                                  FYP_Receipt_No = a.FYP_ReceiptNo,
                                                  FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                  FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                  HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                  HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                  FYP_Id = a.FYP_Id,
                                                  HRME_EmployeeCode = c.HRME_EmployeeCode,
                                                  FYP_Date = a.FYP_ReceiptDate,
                                                  FYP_DD_Cheque_Date = g.FYPPM_DDChequeDate,
                                                  FYP_DD_Cheque_No = g.FYPPM_DDChequeNo,
                                                  FYP_Bank_Name = g.FYPPM_BankName,
                                                  amst_mobile = c.HRME_MobileNo,
                                                  fathername = c.HRME_FatherName,
                                                  studentdob = c.HRME_DOB,
                                                  FYP_Remarks = a.FYP_Remarks
                                              }

        ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }

                if (data.Stf_Others_flg == "Others")
                {
                    data.receiparraydeleteall = (from a in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                                 from c in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                                 from d in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                                 from e in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                 where (d.FMG_Id == e.FMG_Id && a.FMCAOST_Id == d.FMCAOST_Id && b.FMH_Id == d.FMH_Id && c.FTI_Id == d.FTI_Id && a.FYP_Id == data.FYP_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.FMCOST_Id == data.HRME_Id)
                                                 select new CollegeStaffAndOtherTransactionDTO
                                                 {
                                                     FMG_GroupName = e.FMG_GroupName,
                                                     FMH_FeeName = b.FMH_FeeName,
                                                     FTI_Name = c.FTI_Name,
                                                     FMH_Id = b.FMH_Id,
                                                     FTI_Id = c.FTI_Id,
                                                     FCMAS_Id = a.FMCAOST_Id,

                                                     totalamount = d.FCSSOST_NetAmount,
                                                     ToBePaid = a.FTPOSTC_PaidAmount,
                                                     ConcessionAmount = d.FCSSOST_ConcessionAmount,
                                                     FSS_FineAmount = d.FCSSOST_FineAmount,
                                                     CurrentYrCharges = d.FCSSOST_CurrentYrCharges,
                                                     totalcharges = d.FCSSOST_TotalCharges,
                                                     FYP_Id = a.FYP_Id

                                                 }).ToArray();

                    data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                              from c in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                              from d in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                              where (a.FYP_Id == d.FYP_Id && a.FYP_Id == b.FYP_Id && b.FMCOST_Id == c.FMOST_Id && c.FMOST_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id)
                                              select new CollegeStaffAndOtherTransactionDTO
                                              {
                                                  HRME_Id = c.FMOST_Id,
                                                  HRME_EmployeeFirstName = c.FMOST_StudentName,
                                                  HRME_EmployeeMiddleName = "",
                                                  HRME_EmployeeLastName = "",
                                                  FYP_Receipt_No = a.FYP_ReceiptNo,
                                                  FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                  FYP_Tot_Amount = a.FYP_TotalPaidAmount,

                                                  FYP_Id = a.FYP_Id,

                                                  FYP_Date = a.FYP_ReceiptDate,
                                                  FYP_DD_Cheque_Date = d.FYPPM_DDChequeDate,
                                                  FYP_DD_Cheque_No = d.FYPPM_DDChequeNo,
                                                  FYP_Bank_Name = d.FYPPM_BankName,
                                                  amst_mobile = c.FMOST_StudentMobileNo,

                                                  FMOST_StudentEmailId = c.FMOST_StudentEmailId,
                                                  FYP_Remarks = a.FYP_Remarks
                                              }

        ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }




                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _YearlyFeeGroupMappingContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Transaction").ToList();
                data.transnumconfig = masnum.ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        //for staff_others
        public CollegeStaffAndOtherTransactionDTO select_emp(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                if (data.Grp_Term_flg == "T")
                {
                    data.termlist = (from a in _YearlyFeeGroupMappingContext.feeTr
                                     from b in _YearlyFeeGroupMappingContext.feeMTH
                                     from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                     where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && c.FCSSST_ActiveFlag == true && c.HRME_Id == data.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.ASMAY_Id == data.ASMAY_Id)//&& c.FSSST_PaidAmount < c.FSSST_NetAmount
                                     select new CollegeStaffAndOtherTransactionDTO
                                     {
                                         FMT_Id = a.FMT_Id,
                                         FMT_Name = a.FMT_Name,
                                         FMT_Order = a.FMT_Order,
                                         NetAmount = c.FCSSST_NetAmount,
                                         PaidAmount = c.FCSSST_PaidAmount
                                     }).Distinct().ToList().ToArray();



                }
                else if (data.Grp_Term_flg == "G")
                {
                    data.grouplist = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO

                                      from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                      where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true && c.FCSSST_ActiveFlag == true && c.HRME_Id == data.HRME_Id && a.FMG_Id == c.FMG_Id && c.ASMAY_Id == data.ASMAY_Id)//&& c.FSSST_PaidAmount < c.FSSST_NetAmount
                                      select new CollegeStaffAndOtherTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName,
                                          NetAmount = c.FCSSST_NetAmount,
                                          PaidAmount = c.FCSSST_PaidAmount
                                      }).Distinct().ToList().ToArray();
                }
                data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                  from b in _YearlyFeeGroupMappingContext.HR_Master_Department
                                  from c in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.HRME_Id == data.HRME_Id)
                                  select new Temp_Staff_DTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                      HRMD_Id = a.HRMD_Id,
                                      HRMDES_Id = c.HRMDES_Id,
                                      HRMD_DepartmentName = b.HRMD_DepartmentName,
                                      HRMDES_DesignationName = c.HRMDES_DesignationName
                                  }).Distinct().OrderBy(t => t.HRME_Id).ToList().ToArray();


                if (data.Stf_Others_flg == "Staff")
                {
                    var showstaticticsdetails = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                                 from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                                 where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.HRME_Id == data.HRME_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_CurrentYrCharges = a.FCSSST_CurrentYrCharges,
                                                     FSS_ToBePaid = a.FCSSST_ToBePaid,
                                                     FSS_PaidAmount = a.FCSSST_PaidAmount,
                                                     FMG_GroupName = b.FMG_GroupName,
                                                     FMG_Id = b.FMG_Id
                                                 }
      ).ToList();

                    data.showstaticticsdetails = (from i in showstaticticsdetails
                                                  group i by new { i.FMG_Id, i.FMG_GroupName } into g
                                                  select new FeeStudentTransactionDTO
                                                  {
                                                      FSS_CurrentYrCharges = g.Sum(t => t.FSS_CurrentYrCharges),
                                                      FSS_ToBePaid = g.Sum(t => t.FSS_ToBePaid),
                                                      FSS_PaidAmount = g.Sum(t => t.FSS_PaidAmount),
                                                      FMG_GroupName = g.Key.FMG_GroupName,
                                                      FMG_Id = g.Key.FMG_Id

                                                  }).Distinct().ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeStaffAndOtherTransactionDTO select_student(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                if (data.Grp_Term_flg == "T")
                {
                    data.termlist = (from a in _YearlyFeeGroupMappingContext.feeTr
                                     from b in _YearlyFeeGroupMappingContext.feeMTH
                                     from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                     where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && c.FCSSOST_ActiveFlag == true && c.FMCOST_Id == data.FMOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.ASMAY_Id == data.ASMAY_Id)//&& c.FCSSOST_PaidAmount < c.FSSOST_NetAmount
                                     select new CollegeStaffAndOtherTransactionDTO
                                     {
                                         FMT_Id = a.FMT_Id,
                                         FMT_Name = a.FMT_Name,
                                         FMT_Order = a.FMT_Order,
                                         NetAmount = c.FCSSOST_NetAmount,
                                         PaidAmount = c.FCSSOST_PaidAmount
                                     }).Distinct().ToList().ToArray();




                }
                else if (data.Grp_Term_flg == "G")
                {
                    data.grouplist = (from a in _YearlyFeeGroupMappingContext.FeeGroupClgDMO

                                      from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                      where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true && c.FCSSOST_ActiveFlag == true && c.FMCOST_Id == data.FMOST_Id && a.FMG_Id == c.FMG_Id && c.ASMAY_Id == data.ASMAY_Id) //&& c.FCSSOST_PaidAmount < c.FSSOST_NetAmount
                                      select new CollegeStaffAndOtherTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName,
                                          NetAmount = c.FCSSOST_NetAmount,
                                          PaidAmount = c.FCSSOST_PaidAmount
                                      }).Distinct().ToList().ToArray();
                }
                data.oth_studentlist = _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO.Where(s => s.MI_Id == data.MI_Id && s.FMOST_ActiveFlag == true && s.FMOST_Id == data.FMOST_Id).Distinct().OrderBy(t => t.FMOST_Id).ToList().ToArray();



                var showstaticticsdetails = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                             from b in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                             where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMCOST_Id == data.FMOST_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FSS_CurrentYrCharges = a.FCSSOST_CurrentYrCharges,
                                                 FSS_ToBePaid = a.FCSSOST_ToBePaid,
                                                 FSS_PaidAmount = a.FCSSOST_PaidAmount,
                                                 FMG_GroupName = b.FMG_GroupName,
                                                 FMG_Id = b.FMG_Id
                                             }
  ).ToList();

                data.showstaticticsdetails = (from i in showstaticticsdetails
                                              group i by new { i.FMG_Id, i.FMG_GroupName } into g
                                              select new FeeStudentTransactionDTO
                                              {
                                                  FSS_CurrentYrCharges = g.Sum(t => t.FSS_CurrentYrCharges),
                                                  FSS_ToBePaid = g.Sum(t => t.FSS_ToBePaid),
                                                  FSS_PaidAmount = g.Sum(t => t.FSS_PaidAmount),
                                                  FMG_GroupName = g.Key.FMG_GroupName,
                                                  FMG_Id = g.Key.FMG_Id

                                              }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeStaffAndOtherTransactionDTO getgroupmappedheadsnew_st(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {

                if (data.Stf_Others_flg == "Staff")
                {
                    if (data.Grp_Term_flg.Equals("G"))
                    {

                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                               from b in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                               from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                               where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FCSSST_ActiveFlag == true && c.HRME_Id == data.HRME_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true)
                                               select new PreadmissionDTOs.com.vaps.College.Fees.Head_Installments_DTO
                                               {
                                                   FCMAS_Id = c.FMCAOST_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = b.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = c.FCSSST_CurrentYrCharges,
                                                   TotalCharges  = c.FCSSST_TotalCharges,
                                                   ConcessionAmount = c.FCSSST_ConcessionAmount,
                                                   FineAmount = c.FCSSST_FineAmount,
                                                   ToBePaid = c.FCSSST_ToBePaid,
                                                   NetAmount = c.FCSSST_NetAmount,
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = d.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();
                    }
                    else if (data.Grp_Term_flg.Equals("T"))
                    {
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                               from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                               from b in _YearlyFeeGroupMappingContext.feeMTH
                                               from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                               from e in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                               where ( a.MI_Id == c.MI_Id && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && d.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FCSSST_ActiveFlag == true && c.HRME_Id == data.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && data.terms_groups.Contains(b.FMT_Id) && e.MI_Id == b.MI_Id && e.FMG_Id == c.FMG_Id && e.FMG_ActiceFlag == true)
                                               select new PreadmissionDTOs.com.vaps.College.Fees.Head_Installments_DTO
                                               {
                                                   FCMAS_Id = c.FMCAOST_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = d.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = c.FCSSST_CurrentYrCharges,
                                                   TotalCharges = c.FCSSST_TotalCharges,
                                                   ConcessionAmount = c.FCSSST_ConcessionAmount,
                                                   FineAmount = c.FCSSST_FineAmount,
                                                   ToBePaid = c.FCSSST_ToBePaid,
                                                   NetAmount = c.FCSSST_NetAmount,
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = e.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();
                    }
                }
                else if (data.Stf_Others_flg == "Others")
                {
                    if (data.Grp_Term_flg.Equals("G"))
                    {
                        //  data.mapped_hds_ins = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.FMOST_Id == data.FMOST_Id && data.terms_groups.Contains(s.FMG_Id) && s.FSSOST_ActiveFlag == true).ToList().ToArray();
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                               from b in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                               from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                               where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FCSSOST_ActiveFlag == true && c.FMCOST_Id == data.FMOST_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true)
                                               select new PreadmissionDTOs.com.vaps.College.Fees.Head_Installments_DTO
                                               {
                                                   FCMAS_Id = c.FMCAOST_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = b.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = c.FCSSOST_CurrentYrCharges,
                                                   TotalCharges = c.FCSSOST_TotalCharges,
                                                   ConcessionAmount = c.FCSSOST_ConcessionAmount,
                                                   FineAmount = c.FCSSOST_FineAmount,
                                                   ToBePaid = c.FCSSOST_ToBePaid,
                                                   NetAmount = c.FCSSOST_NetAmount,
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = d.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();

                    }
                    else if (data.Grp_Term_flg.Equals("T"))
                    {

                        //data.mapped_hds_ins = (from b in _YearlyFeeGroupMappingContext.feeMTH
                        //                       from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                        //                       where (c.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FSSOST_ActiveFlag == true && c.FMOST_Id == data.FMOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && data.terms_groups.Contains(b.FMT_Id))
                        //                       select c).ToList().ToArray();
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                               from d in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                               from b in _YearlyFeeGroupMappingContext.feeMTH
                                               from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                               from e in _YearlyFeeGroupMappingContext.FeeGroupClgDMO
                                               where (a.MI_Id == d.MI_ID && a.MI_Id == c.MI_Id && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && d.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FCSSOST_ActiveFlag == true && c.FMCOST_Id == data.FMOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && data.terms_groups.Contains(b.FMT_Id) && e.MI_Id == b.MI_Id && e.FMG_Id == c.FMG_Id && e.FMG_ActiceFlag == true)
                                               select new PreadmissionDTOs.com.vaps.College.Fees.Head_Installments_DTO
                                               {
                                                   FCMAS_Id = c.FMCAOST_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = d.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = c.FCSSOST_CurrentYrCharges,
                                                   TotalCharges = c.FCSSOST_TotalCharges,
                                                   ConcessionAmount = c.FCSSOST_ConcessionAmount,
                                                   FineAmount = c.FCSSOST_FineAmount,
                                                   ToBePaid = c.FCSSOST_ToBePaid,
                                                   NetAmount = c.FCSSOST_NetAmount,
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = e.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();

                    }
                }



            }
            catch (Exception e)
            {

                data.validationvalue = "Contact Administrator";
            }
            return data;
        }

        public CollegeStaffAndOtherTransactionDTO savedata_st(CollegeStaffAndOtherTransactionDTO pgmod)
        {

            try
            {
                var ResultCount = 0;

                if (pgmod.FYP_Id > 0)
                {
                    var result = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Single(t => t.MI_Id == pgmod.MI_Id && t.FYP_Id == pgmod.FYP_Id && t.ASMAY_Id == pgmod.ASMAY_Id);

                    Fee_Y_PaymentDMO pmm = new Fee_Y_PaymentDMO();

                    if (pgmod.automanualreceiptno != "Auto" || pgmod.auto_receipt_flag != 1)
                    {
                        result.FYP_ReceiptNo = pgmod.FYP_Receipt_No;
                    }

                    result.FYP_ReceiptDate = Convert.ToDateTime(pgmod.FYP_Date);

                    if (pgmod.FYP_Remarks != "")
                    {
                        result.FYP_Remarks = pgmod.FYP_Remarks;
                    }

                    //if (pgmod.FYP_Bank_Or_Cash != "C")
                    //{
                    //    result.ch = pgmod.FYP_DD_Cheque_Date;
                    //    result.FYP_DD_Cheque_No = pgmod.FYP_DD_Cheque_No;
                    //    result. = pgmod.FYP_Bank_Name;
                    //}

                    _YearlyFeeGroupMappingContext.Update(result);

                    var contactexisttransaction = 0;
                    using (var dbCtxTxn = _YearlyFeeGroupMappingContext.Database.BeginTransaction())
                    {
                        try
                        {
                            contactexisttransaction = _YearlyFeeGroupMappingContext.SaveChanges();
                            dbCtxTxn.Commit();
                            pgmod.returnval = "Update";
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            dbCtxTxn.Rollback();
                            pgmod.returnval = "Failed";
                        }
                    }
                }
                else
                {
                    // Fee_Y_PaymentDMO obj_pay = Fee_Y_PaymentDMO(pgmod);
                    Fee_Y_PaymentDMO obj_pay = new Fee_Y_PaymentDMO();
                    obj_pay.ASMAY_Id = pgmod.ASMAY_Id;
                    obj_pay.MI_Id = pgmod.MI_Id;
                    //obj_pay.FTCU_Id = 1;
                    //obj_pay.FYP_Tot_Waived_Amt = 0;
                    obj_pay.FYP_ChequeBounceFlag = "CL";
                    obj_pay.FYP_DOE = DateTime.Now;
                    obj_pay.FYP_ReceiptDate = pgmod.FYP_Date;

                    obj_pay.CreatedDate = DateTime.Now;
                    obj_pay.UpdatedDate = DateTime.Now;
                    obj_pay.User_Id = pgmod.userid;
                    obj_pay.FYP_TransactionTypeFlag =  "";
                    obj_pay.FYP_Remarks = pgmod.FYP_Remarks;
                    obj_pay.FYP_ChallanStatusFlag = "";
                    obj_pay.FYP_ChallanNo = "";
                    obj_pay.FYP_Transaction_Id = "";
                    obj_pay.FYP_TotalFineAmount = pgmod.FineAmount;
                    obj_pay.FYP_TotalRebateAmount = 0;
                    obj_pay.FYP_TotalPaidAmount = pgmod.FYP_Tot_Amount;
                    obj_pay.FYP_ChequeBounceFlag = "CL";
                    obj_pay.FYP_ActiveFlag = true;
                    obj_pay.FYP_Currency = "1";
                    get_grp_reptno(pgmod);

                    obj_pay.FYP_ReceiptNo = pgmod.FYP_Receipt_No;

                    if (obj_pay.FYP_ReceiptNo == "" || obj_pay.FYP_ReceiptNo == null)
                    {
                        pgmod.returnval = "Record Not Saved because Receipt No is not Generated Automatic.Settings are missing";
                        return pgmod;
                    }
                    else
                    {
                        _YearlyFeeGroupMappingContext.Add(obj_pay);
                        if (pgmod.Stf_Others_flg == "Staff")
                        {
                            Fee_Y_Payment_College_StaffDMO obj_pay_stf = new Fee_Y_Payment_College_StaffDMO();
                            obj_pay_stf.FYP_Id = obj_pay.FYP_Id;
                            obj_pay_stf.HRME_Id = pgmod.HRME_Id;
                            obj_pay_stf.ASMAY_Id = pgmod.ASMAY_Id;
                            obj_pay_stf.FYPCS_TotalPaidAmount = Convert.ToInt64(obj_pay.FYP_TotalPaidAmount);
                           obj_pay_stf.FYPCS_TotalWaivedAmount = 0;
                            obj_pay_stf.FYPCS_TotalConcessionAmount =pgmod.ConcessionAmount ;
                            obj_pay_stf.FYPCS_TotalFineAmount = obj_pay.FYP_TotalFineAmount;
                            obj_pay_stf.FYPCS_CreatedDate = DateTime.Now;
                            obj_pay_stf.FYPCS_UpdatedDate = DateTime.Now;
                            _YearlyFeeGroupMappingContext.Add(obj_pay_stf);

                            for (int l = 0; l < pgmod.head_installments.Length; l++)
                            {
                                Fee_T_Payment_OthStaff_College_CollegeDMO obj_trans_st = new Fee_T_Payment_OthStaff_College_CollegeDMO();
                                obj_trans_st.FYP_Id = obj_pay.FYP_Id;
                                obj_trans_st.FMCAOST_Id = pgmod.head_installments[l].FCMAS_Id;
                                obj_trans_st.FTPOSTC_PaidAmount = pgmod.head_installments[l].ToBePaid;
                                obj_trans_st.FTPOSTC_WaivedAmount = 0;
                                obj_trans_st.FTPOSTC_ConcessionAmount = pgmod.head_installments[l].ConcessionAmount;
                                obj_trans_st.FTPOSTC_FineAmount = pgmod.head_installments[l].FineAmount;
                                obj_trans_st.FTPOSTC_RebateAmount = 0;
                                obj_trans_st.FTPOSTC_Remarks = obj_pay.FYP_Remarks;
                                obj_trans_st.FTPOSTC_CreatedDate = DateTime.Now;
                                obj_trans_st.FTPOSTC_UpdatedDate = DateTime.Now;
                                _YearlyFeeGroupMappingContext.Add(obj_trans_st);

                                var obj_status_stf = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff.Single(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.HRME_Id == pgmod.HRME_Id && t.FMH_Id == pgmod.head_installments[l].FMH_Id && t.FTI_Id == pgmod.head_installments[l].FTI_Id && t.FMG_Id == pgmod.head_installments[l].FMG_Id && t.FMCAOST_Id == pgmod.head_installments[l].FCMAS_Id && t.FCSSST_ActiveFlag == true);

                                obj_status_stf.FCSSST_PaidAmount = pgmod.head_installments[l].ToBePaid;

                                if (obj_status_stf.FCSSST_NetAmount != 0)
                                {
                                    obj_status_stf.FCSSST_ToBePaid = obj_status_stf.FCSSST_ToBePaid - obj_status_stf.FCSSST_ToBePaid;
                                }
                                else
                                {
                                    obj_status_stf.FCSSST_ToBePaid = 0;
                                }

                                //obj_status_stf.FSSST_ToBePaid = obj_status_stf.FSSST_ToBePaid - obj_status_stf.FSSST_PaidAmount;

                                obj_status_stf.FCSSST_UpdatedDate = DateTime.Now;
                                _YearlyFeeGroupMappingContext.Update(obj_status_stf);
                                ResultCount = _YearlyFeeGroupMappingContext.SaveChanges();
                            }

                        }
                        else if (pgmod.Stf_Others_flg == "Others")
                        {
                            Fee_Y_Payment_OthStu_CollegeDMO obj_pay_stf = new Fee_Y_Payment_OthStu_CollegeDMO();
                            obj_pay_stf.FYP_Id = obj_pay.FYP_Id;
                            obj_pay_stf.FMCOST_Id = pgmod.FMOST_Id;
                            obj_pay_stf.ASMAY_Id = pgmod.ASMAY_Id;
                            obj_pay_stf.FYPOSTC_TotalPaidAmount = Convert.ToInt64(obj_pay.FYP_TotalPaidAmount);
                            obj_pay_stf.FYPOSTC_TotalWaivedAmount =0;
                          obj_pay_stf.FYPOSTC_TotalConcessionAmount = pgmod.ConcessionAmount;
                            obj_pay_stf.FYPOSTC_TotalFineAmount = pgmod.FYP_Tot_Fine_Amt;
                            obj_pay_stf.FYPOSTC_CreatedDate = DateTime.Now;
                            obj_pay_stf.FYPOSTC_UpdatedDate = DateTime.Now;
                            _YearlyFeeGroupMappingContext.Add(obj_pay_stf);
                            for (int l = 0; l < pgmod.head_installments.Length; l++)
                            {
                                Fee_T_Payment_OthStaff_College_CollegeDMO obj_trans_st = new Fee_T_Payment_OthStaff_College_CollegeDMO();
                                obj_trans_st.FYP_Id = obj_pay.FYP_Id;
                                obj_trans_st.FMCAOST_Id = pgmod.head_installments[l].FCMAS_Id;
                                obj_trans_st.FTPOSTC_PaidAmount = pgmod.head_installments[l].ToBePaid;
                                obj_trans_st.FTPOSTC_WaivedAmount = 0;
                                obj_trans_st.FTPOSTC_ConcessionAmount = pgmod.head_installments[l].ConcessionAmount;
                                obj_trans_st.FTPOSTC_FineAmount = pgmod.head_installments[l].FineAmount;
                                obj_trans_st.FTPOSTC_RebateAmount = 0;
                                obj_trans_st.FTPOSTC_Remarks = obj_pay.FYP_Remarks;
                                obj_trans_st.FTPOSTC_CreatedDate = DateTime.Now;
                                obj_trans_st.FTPOSTC_UpdatedDate = DateTime.Now;

                                obj_trans_st.FTPOSTC_CreatedBy = pgmod.userid;
                                obj_trans_st.FTPOSTC_UpdatedBy = pgmod.userid;

                                _YearlyFeeGroupMappingContext.Add(obj_trans_st);

                                var obj_status_otrs = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO.Single(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.FMCOST_Id == pgmod.FMOST_Id && t.FMH_Id == pgmod.head_installments[l].FMH_Id && t.FTI_Id == pgmod.head_installments[l].FTI_Id && t.FMG_Id == pgmod.head_installments[l].FMG_Id && t.FMCAOST_Id == pgmod.head_installments[l].FCMAS_Id && t.FCSSOST_ActiveFlag == true);

                                obj_status_otrs.FCSSOST_PaidAmount = pgmod.head_installments[l].ToBePaid;

                                if (obj_status_otrs.FCSSOST_NetAmount != 0)
                                {
                                    obj_status_otrs.FCSSOST_ToBePaid = obj_status_otrs.FCSSOST_ToBePaid - obj_status_otrs.FCSSOST_PaidAmount;
                                }
                                else
                                {
                                    obj_status_otrs.FCSSOST_ToBePaid = 0;
                                }


                                //obj_status_otrs.FCSSOST_ToBePaid = obj_status_otrs.FCSSOST_ToBePaid - obj_status_otrs.FCSSOST_PaidAmount;

                                obj_status_otrs.FCSSOST_UpdatedDate = DateTime.Now;
                                _YearlyFeeGroupMappingContext.Update(obj_status_otrs);
                                ResultCount = _YearlyFeeGroupMappingContext.SaveChanges();

                            }

                        }
                        if (ResultCount >= 1)
                        {
                            pgmod.returnval = "Save";
                        }
                        else
                        {
                            pgmod.returnval = "Cancel";
                        }
                    }
                }

            }

            catch (Exception ee)
            {
                pgmod.returnval = "Error";
                Console.WriteLine(ee.Message);
                _logger.LogError(ee.Message);
            }

            return pgmod;
        }

        public CollegeStaffAndOtherTransactionDTO searching_s(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {

                switch (data.searchType)
                {

                    case "0":

                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where (d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.User_Id == data.userid && (((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim().Contains(data.searchtext) || ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName)).Trim().Contains(data.searchtext) || ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim().Contains(data.searchtext) || d.HRME_EmployeeFirstName.StartsWith(data.searchtext) || d.HRME_EmployeeMiddleName.StartsWith(data.searchtext) || d.HRME_EmployeeLastName.StartsWith(data.searchtext)))
                                                   select new CollegeStaffAndOtherTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_ReceiptNo,
                                                       FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                       FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                       FYP_Date = a.FYP_ReceiptDate
                                                   }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();
                        break;
                    case "1":
                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where (d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.User_Id == data.userid && d.HRME_EmployeeCode.Contains(data.searchtext))
                                                   select new CollegeStaffAndOtherTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_ReceiptNo,
                                                       FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                       FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                       FYP_Date = a.FYP_ReceiptDate
                                                   }).Distinct().OrderBy(t => t.HRME_EmployeeCode).ToArray();
                        break;
                    case "7":
                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where (d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.User_Id == data.userid && e.HRMD_DepartmentName.Contains(data.searchtext))
                                                   select new CollegeStaffAndOtherTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_ReceiptNo,
                                                       FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                       FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                       FYP_Date = a.FYP_ReceiptDate
                                                   }).Distinct().OrderBy(t => t.HRMD_DepartmentName).ToArray();
                        break;
                    case "2":
                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where (d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.User_Id == data.userid && f.HRMDES_DesignationName.Contains(data.searchtext))
                                                   select new CollegeStaffAndOtherTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_ReceiptNo,
                                                       FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                       FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                       FYP_Date = a.FYP_ReceiptDate
                                                   }).Distinct().OrderBy(t => t.HRMDES_DesignationName).ToArray();
                        break;
                    case "3":

                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where (d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.User_Id == data.userid && a.FYP_ReceiptNo.Contains(data.searchtext))
                                                   select new CollegeStaffAndOtherTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_ReceiptNo,
                                                       FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                       FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                       FYP_Date = a.FYP_ReceiptDate
                                                   }).Distinct().OrderBy(t => t.FYP_Receipt_No).ToArray();
                        break;
                    case "4":
                        var date_format = data.searchdate.ToString("dd/MM/yyyy");

                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where (d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.User_Id == data.userid && a.FYP_ReceiptDate.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy"))
                                                   select new CollegeStaffAndOtherTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_ReceiptNo,
                                                       FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                       FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                       FYP_Date = a.FYP_ReceiptDate
                                                   }).Distinct().OrderBy(t => t.FYP_Receipt_No).ToArray();
                        break;
                    case "5":

                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where (d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.User_Id == data.userid && a.FYP_TotalPaidAmount.ToString().Contains(data.searchnumber))
                                                   select new CollegeStaffAndOtherTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_ReceiptNo,
                                                       FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                       FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                       FYP_Date = a.FYP_ReceiptDate
                                                   }).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();
                        break;
                    case "6":

                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where (d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.User_Id == data.userid && a.FYP_TransactionTypeFlag.Contains(data.searchtext))
                                                   select new CollegeStaffAndOtherTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_ReceiptNo,
                                                       FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                       FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                       FYP_Date = a.FYP_ReceiptDate
                                                   }).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();

                        break;

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public CollegeStaffAndOtherTransactionDTO searching_o(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {

                switch (data.searchType)
                {

                    case "0":

                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where (d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMCOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.User_Id == data.userid && d.FMOST_StudentName.Contains(data.searchtext))
                                                    select new CollegeStaffAndOtherTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_ReceiptNo,
                                                        FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                        FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                        FYP_Date = a.FYP_ReceiptDate
                                                    }).Distinct().OrderBy(t => t.FMOST_StudentName).ToArray();
                        break;
                    case "1":
                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where (d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMCOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.User_Id == data.userid && d.FMOST_StudentMobileNo.ToString().Contains(data.searchtext))
                                                    select new CollegeStaffAndOtherTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_ReceiptNo,
                                                        FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                        FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                        FYP_Date = a.FYP_ReceiptDate
                                                    }).Distinct().OrderBy(t => t.FMOST_StudentMobileNo).ToArray();
                        break;
                    case "2":
                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where (d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMCOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.User_Id == data.userid && d.FMOST_StudentEmailId.Contains(data.searchtext))
                                                    select new CollegeStaffAndOtherTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_ReceiptNo,
                                                        FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                        FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                        FYP_Date = a.FYP_ReceiptDate
                                                    }).Distinct().OrderBy(t => t.FMOST_StudentEmailId).ToArray();
                        break;
                    case "3":
                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where (d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMCOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.User_Id == data.userid && a.FYP_ReceiptNo.Contains(data.searchtext))
                                                    select new CollegeStaffAndOtherTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_ReceiptNo,
                                                        FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                        FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                        FYP_Date = a.FYP_ReceiptDate
                                                    }).Distinct().OrderBy(t => t.FYP_Receipt_No).ToArray();
                        break;
                    case "4":
                        var date_format = data.searchdate.ToString("dd/MM/yyyy");
                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where (d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMCOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.User_Id == data.userid && a.FYP_ReceiptDate.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy"))
                                                    select new CollegeStaffAndOtherTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_ReceiptNo,
                                                        FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                        FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                        FYP_Date = a.FYP_ReceiptDate
                                                    }).Distinct().OrderBy(t => t.FYP_Date).ToArray();
                        break;
                    case "5":
                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where (d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMCOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.User_Id == data.userid && a.FYP_TotalPaidAmount.ToString().Contains(data.searchnumber))
                                                    select new CollegeStaffAndOtherTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_ReceiptNo,
                                                        FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                        FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                        FYP_Date = a.FYP_ReceiptDate
                                                    }).Distinct().ToArray();
                        break;
                    case "6":
                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where (d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMCOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.User_Id == data.userid && a.FYP_TransactionTypeFlag.Contains(data.searchtext))
                                                    select new CollegeStaffAndOtherTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_ReceiptNo,
                                                        FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                        FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                        FYP_Date = a.FYP_ReceiptDate
                                                    }).Distinct().OrderBy(t => t.FYP_Bank_Or_Cash).ToArray();

                        break;

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public CollegeStaffAndOtherTransactionDTO printreceipt_s(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                data.receiptdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                       from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                       from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                       from d in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                       from e in _YearlyFeeGroupMappingContext.MasterEmployee
                                       from f in _YearlyFeeGroupMappingContext.HR_Master_Department
                                       from g in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                       from h in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from i in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                       from j in _YearlyFeeGroupMappingContext.AcademicYear
                                       from k in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                       where (a.FYP_Id==k.FYP_Id && a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && g.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && i.MI_ID == a.MI_Id && j.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == e.HRME_Id && f.HRMD_Id == e.HRMD_Id && g.HRMDES_Id == e.HRMDES_Id && c.FMCAOST_Id == d.FMCAOST_Id && d.FMH_Id == h.FMH_Id && d.FTI_Id == i.FTI_Id && a.ASMAY_Id == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_Id && j.ASMAY_Id == a.ASMAY_Id && d.FMCAOST_OthStaffFlag == "S")
                                       select new CollegeStaffAndOtherTransactionDTO
                                       {
                                           FYP_Id = a.FYP_Id,
                                           FYP_Receipt_No = a.FYP_ReceiptNo,
                                           FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                           FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                           FYP_Bank_Name = k.FYPPM_BankName,
                                           FYP_DD_Cheque_No = k.FYPPM_DDChequeNo,
                                           FYP_DD_Cheque_Date = k.FYPPM_DDChequeDate,
                                           FYP_Remarks = a.FYP_Remarks,
                                           FYP_Date = a.FYP_ReceiptDate,
                                           ToBePaid = Convert.ToInt64(d.FMCAOST_Amount),
                                           FTPOST_PaidAmount = c.FTPOSTC_PaidAmount,
                                           FTPOST_ConcessionAmount = c.FTPOSTC_ConcessionAmount,
                                           HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                           HRME_EmployeeCode = e.HRME_EmployeeCode,
                                           HRMD_DepartmentName = f.HRMD_DepartmentName,
                                           HRMDES_DesignationName = g.HRMDES_DesignationName,
                                           FMH_Id = d.FMH_Id,
                                           FTI_Id = d.FTI_Id,
                                           ASMAY_Id = a.ASMAY_Id,
                                           HRME_Id = e.HRME_Id,
                                           FMH_FeeName = h.FMH_FeeName,
                                           FTI_Name = i.FTI_Name,
                                           ASMAY_Year = j.ASMAY_Year
                                       }).Distinct().ToArray();



                var feeterm = (from a in _YearlyFeeGroupMappingContext.feeTr
                               from b in _YearlyFeeGroupMappingContext.feeMTH
                               from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                               from d in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                               from e in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                               from f in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                               from g in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                               where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && c.FCSSST_ActiveFlag == true && c.HRME_Id == d.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.ASMAY_Id == d.ASMAY_Id && e.MI_Id == a.MI_Id && d.ASMAY_Id == e.ASMAY_Id && d.FYP_Id == data.FYP_Id && e.FYP_Id == d.FYP_Id && f.FYP_Id == d.FYP_Id && g.FMCAOST_Id == f.FMCAOST_Id && g.FTI_Id == b.FTI_Id && g.FMCAOST_OthStaffFlag == "S")//&& c.FSSST_PaidAmount < c.FSSST_NetAmount
                               select new CollegeStaffAndOtherTransactionDTO
                               {
                                   FMT_Id = a.FMT_Id,
                                   FMT_Order = a.FMT_Order,
                                   FromMonth = a.FromMonth,
                                   ToMonth = a.ToMonth

                               }).Distinct().OrderByDescending(t => t.FMT_Order).ToArray();


                long fmt_id_new = 0;



                fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;
                var fmt_by_order = feeterm[0].FMT_Order + 1;

                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order <= fmt_by_order).Select(t => t.FMT_Id);


                data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                  from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                  from e in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                  from f in _YearlyFeeGroupMappingContext.feeMTH
                                  where (a.MI_Id == data.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && a.ASMAY_Id == b.ASMAY_Id && e.HRME_Id == b.HRME_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id))
                                  select e.FCSSST_ToBePaid).Sum();
                var monthnameinitial = feeterm[(feeterm.Length - 1)].FromMonth.ToString();
                var monthnameend = feeterm[0].ToMonth.ToString();

                data.duration = monthnameinitial + '-' + monthnameend;// +'-' + data.year

                var duedates_list = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                         // from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                     from d in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                     from e in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                     from f in _YearlyFeeGroupMappingContext.feeMTH
                                     from g in _YearlyFeeGroupMappingContext.Fee_T_Due_Date_OthStaffs
                                     where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && a.ASMAY_Id == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_Id && e.HRME_Id == b.HRME_Id && e.FMG_Id == d.FMG_Id && e.FMH_Id == d.FMH_Id && e.FTI_Id == d.FTI_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id) && d.FMCAOST_Id == g.FMAOST_Id && d.FMCAOST_OthStaffFlag == "S")// && f.FMT_Id== fmt_id_new  && c.FYP_Id == a.FYP_Id && c.FMAOST_Id == d.FMAOST_Id && e.FCMAS_Id == c.FMAOST_Id
                                                                                                                                                                                                                                                                                                                                                                                                                                                                        //a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FCMAS_Id == d.FCMAS_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id
                                     select g).Distinct().ToList();

                var year = duedates_list.Max(t => t.FTDD_Year);
                var month = duedates_list.Where(t => t.FTDD_Year == year).Select(t => Convert.ToInt32(t.FTDD_Month)).Max();
                var date = duedates_list.Where(t => Convert.ToInt32(t.FTDD_Month) == month && t.FTDD_Year == year).Select(t => Convert.ToInt32(t.FTDD_Day)).Max();

                data.Due_Date = date + "/" + month + "/" + year;


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public CollegeStaffAndOtherTransactionDTO printreceipt_o(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                data.receiptdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                       from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                       from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                       from d in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                       from e in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                       from h in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                       from i in _YearlyFeeGroupMappingContext.Clg_Fee_Installments_Yearly_DMO
                                       from j in _YearlyFeeGroupMappingContext.AcademicYear
                                       from k in _YearlyFeeGroupMappingContext.Fee_Y_Payment_PaymentModeDMO
                                       where (a.FYP_Id==k.FYP_Id && a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && i.MI_ID == a.MI_Id && j.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMCOST_Id == e.FMOST_Id && c.FMCAOST_Id == d.FMCAOST_Id && d.FMH_Id == h.FMH_Id && d.FTI_Id == i.FTI_Id && a.ASMAY_Id == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_Id && j.ASMAY_Id == a.ASMAY_Id && d.FMCAOST_OthStaffFlag == "O")
                                       select new CollegeStaffAndOtherTransactionDTO
                                       {
                                           FYP_Id = a.FYP_Id,
                                           FYP_Receipt_No = a.FYP_ReceiptNo,
                                           FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                           FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                           FYP_Bank_Name = k.FYPPM_BankName,
                                           FYP_DD_Cheque_No = k.FYPPM_DDChequeNo,
                                           FYP_DD_Cheque_Date = k.FYPPM_DDChequeDate,
                                           FYP_Remarks = a.FYP_Remarks,
                                           FYP_Date = a.FYP_ReceiptDate,
                                           ToBePaid = Convert.ToInt64(d.FMCAOST_Amount),
                                           FTPOST_PaidAmount = c.FTPOSTC_PaidAmount,
                                           FTPOST_ConcessionAmount = c.FTPOSTC_ConcessionAmount,
                                           FMOST_Id = e.FMOST_Id,
                                           FMOST_StudentName = e.FMOST_StudentName,
                                           FMOST_StudentMobileNo = e.FMOST_StudentMobileNo,
                                           FMOST_StudentEmailId = e.FMOST_StudentEmailId,
                                           FMH_Id = d.FMH_Id,
                                           FTI_Id = d.FTI_Id,
                                           ASMAY_Id = a.ASMAY_Id,
                                           FMH_FeeName = h.FMH_FeeName,
                                           FTI_Name = i.FTI_Name,
                                           ASMAY_Year = j.ASMAY_Year
                                       }).Distinct().ToArray();


                var feeterm = (from a in _YearlyFeeGroupMappingContext.feeTr
                               from b in _YearlyFeeGroupMappingContext.feeMTH
                               from c in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                               from d in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                               from e in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                               from f in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                               from g in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                               where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && c.FCSSOST_ActiveFlag == true && c.FMCOST_Id == d.FMCOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.ASMAY_Id == d.ASMAY_Id && e.MI_Id == a.MI_Id && d.ASMAY_Id == e.ASMAY_Id && d.FYP_Id == data.FYP_Id && e.FYP_Id == d.FYP_Id && f.FYP_Id == d.FYP_Id && g.FMCAOST_Id == f.FMCAOST_Id && g.FTI_Id == b.FTI_Id && g.FMCAOST_OthStaffFlag == "O")
                               select new CollegeStaffAndOtherTransactionDTO
                               {
                                   FMT_Id = a.FMT_Id,
                                   FMT_Order = a.FMT_Order,
                                   FromMonth = a.FromMonth,
                                   ToMonth = a.ToMonth
                               }).Distinct().OrderByDescending(t => t.FMT_Order).ToArray();

                long fmt_id_new = 0;

                fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;
                var fmt_by_order = feeterm[0].FMT_Order + 1;

                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order <= fmt_by_order).Select(t => t.FMT_Id);
                data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                  from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                  from e in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                  from f in _YearlyFeeGroupMappingContext.feeMTH
                                  where (a.MI_Id == data.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && a.ASMAY_Id == b.ASMAY_Id && e.FMCOST_Id == b.FMCOST_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id))
                                  select e.FCSSOST_ToBePaid).Sum();
                var monthnameinitial = feeterm[(feeterm.Length - 1)].FromMonth.ToString();
                var monthnameend = feeterm[0].ToMonth.ToString();

                data.duration = monthnameinitial + '-' + monthnameend;


                var duedates_list = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                     from d in _YearlyFeeGroupMappingContext.Fee_Master_College_Amount_OthStaffs
                                     from e in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                     from f in _YearlyFeeGroupMappingContext.feeMTH
                                     from g in _YearlyFeeGroupMappingContext.Fee_T_Due_Date_OthStaffs
                                     where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && a.ASMAY_Id == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_Id && e.FMCOST_Id == b.FMCOST_Id && e.FMG_Id == d.FMG_Id && e.FMH_Id == d.FMH_Id && e.FTI_Id == d.FTI_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id) && d.FMCAOST_Id == g.FMAOST_Id && d.FMCAOST_OthStaffFlag == "O")
                                     select g).Distinct().ToList();

                var year = duedates_list.Max(t => t.FTDD_Year);
                var month = duedates_list.Where(t => t.FTDD_Year == year).Select(t => Convert.ToInt32(t.FTDD_Month)).Max();
                var date = duedates_list.Where(t => Convert.ToInt32(t.FTDD_Month) == month && t.FTDD_Year == year).Select(t => Convert.ToInt32(t.FTDD_Day)).Max();

                data.Due_Date = date + "/" + month + "/" + year;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public CollegeStaffAndOtherTransactionDTO deletereceipt_s(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {

                var lorg1 = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg2 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg3 = _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                var lorg34 = _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO.Where(t => t.FYP_Id == data.FYP_Id).Select(t => t.FMCAOST_Id).ToList();

                var delcandel = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                 from b in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                 from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                 where (a.FMCAOST_Id == b.FMCAOST_Id && c.HRME_Id == a.HRME_Id && c.ASMAY_Id == a.ASMAY_Id && c.FYP_Id == b.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && lorg34.Contains(a.FMCAOST_Id) && a.HRME_Id == lorg2.HRME_Id && (a.FCSSST_WaivedAmount > 0 || a.FCSSST_AdjustedAmount > 0 || a.FCSSST_RunningExcessAmount > 0))
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMH_Id = a.FMH_Id
                                 }
                      ).ToArray();

                if (delcandel.Length == 0)
                {
                    foreach (var x in lorg3)
                    {
                        var FCMAS_Id = x.FMCAOST_Id;
                        var FSSST_PaidAmount = x.FTPOSTC_PaidAmount;
                        var lorg4 = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff.Single(t => t.FMCAOST_Id == FCMAS_Id && t.MI_Id == lorg1.MI_Id && t.ASMAY_Id == lorg1.ASMAY_Id && t.HRME_Id == lorg2.HRME_Id);

                        lorg4.FCSSST_PaidAmount= lorg4.FCSSST_PaidAmount - x.FTPOSTC_PaidAmount;


                        if (lorg4.FCSSST_ExcessPaidAmount > 0)
                        {
                            lorg4.FCSSST_ExcessPaidAmount = (lorg4.FCSSST_ExcessPaidAmount) - Convert.ToInt64(x.FTPOSTC_PaidAmount);
                        }
                        if (lorg4.FCSSST_RunningExcessAmount > 0)
                        {
                            lorg4.FCSSST_RunningExcessAmount = (lorg4.FCSSST_RunningExcessAmount) - Convert.ToInt64(x.FTPOSTC_PaidAmount);
                        }

                        //added on 11-07-2018
                        var fineheadss = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_Staff
                                          from b in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                          where (a.MI_Id == data.MI_Id && a.FMCAOST_Id == FCMAS_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.HRME_Id == lorg2.HRME_Id && a.FMH_Id == b.FMH_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMH_Id = a.FMH_Id,
                                          }
                           ).Distinct().Take(1);

                        if (fineheadss.Count() > 0)
                        {
                            lorg4.FCSSST_FineAmount = lorg4.FCSSST_FineAmount - Convert.ToInt64(x.FTPOSTC_PaidAmount);
                        }
                        //added on 11-07-2018


                        if (lorg4.FCSSST_NetAmount != 0)
                        {
                            lorg4.FCSSST_ToBePaid = lorg4.FCSSST_ToBePaid + x.FTPOSTC_PaidAmount;
                        }
                        else
                        {
                            lorg4.FCSSST_ToBePaid = 0;
                        }

                        lorg4.FCSSST_UpdatedDate = DateTime.Now;

                        _YearlyFeeGroupMappingContext.Update(lorg4);
                    }

                    if (lorg3.Any())
                    {
                        for (int i = 0; lorg3.Count > i; i++)
                        {
                            _YearlyFeeGroupMappingContext.Remove(lorg3.ElementAt(i));
                        }
                    }
                    _YearlyFeeGroupMappingContext.Remove(lorg2);
                    _YearlyFeeGroupMappingContext.Remove(lorg1);


                    var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        data.returnval = "Delete";
                    }
                    else
                    {
                        data.returnval = "Cancel";
                    }

                }

                else
                {
                    data.returnval = "Transaction is not Processed Correctly.Kindly contact Administrator!!!!!";
                }

            }
            catch (Exception ee)
            {
                data.returnval = "Error";
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public CollegeStaffAndOtherTransactionDTO deletereceipt_o(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                var lorg1 = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg2 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg3 = _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                //List<long> FMA_Ids = new List<long>();
                //foreach (var x in lorg3)
                //{
                //    FMA_Ids.Add(x.FMAOST_Id);
                //}
                foreach (var x in lorg3)
                {
                    var FCMAS_Id = x.FMCAOST_Id;
                    var FSSST_PaidAmount = x.FTPOSTC_PaidAmount;
                    var lorg4 = _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO.Single(t => t.FMCAOST_Id == FCMAS_Id && t.MI_Id == lorg1.MI_Id && t.ASMAY_Id == lorg1.ASMAY_Id && t.FMCOST_Id == lorg2.FMCOST_Id);
                    lorg4.FCSSOST_PaidAmount = lorg4.FCSSOST_PaidAmount - x.FTPOSTC_PaidAmount;

                    if (lorg4.FCSSOST_ExcessPaidAmount > 0)
                    {
                        lorg4.FCSSOST_ExcessPaidAmount = (lorg4.FCSSOST_ExcessPaidAmount) - Convert.ToInt64(x.FTPOSTC_PaidAmount);
                    }
                    if (lorg4.FCSSOST_RunningExcessAmount > 0)
                    {
                        lorg4.FCSSOST_RunningExcessAmount = (lorg4.FCSSOST_RunningExcessAmount) - Convert.ToInt64(x.FTPOSTC_PaidAmount);
                    }

                    //added on 11-07-2018
                    var fineheadss = (from a in _YearlyFeeGroupMappingContext.Fee_College_Student_Status_OthStuDMO
                                      from b in _YearlyFeeGroupMappingContext.FeeHeadClgDMO
                                      where (a.MI_Id == data.MI_Id && a.FMCAOST_Id == FCMAS_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.FMCOST_Id == lorg2.FMCOST_Id && a.FMH_Id == b.FMH_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMH_Id = a.FMH_Id,
                                      }
                       ).Distinct().Take(1);

                    if (fineheadss.Count() > 0)
                    {
                        lorg4.FCSSOST_FineAmount = lorg4.FCSSOST_FineAmount - Convert.ToInt64(x.FTPOSTC_PaidAmount);
                    }
                    //added on 11-07-2018


                    if (lorg4.FCSSOST_NetAmount != 0)
                    {
                        lorg4.FCSSOST_ToBePaid = lorg4.FCSSOST_ToBePaid + x.FTPOSTC_PaidAmount;
                    }
                    else
                    {
                        lorg4.FCSSOST_ToBePaid = 0;
                    }

                    //lorg4.FCSSOST_ToBePaid = lorg4.FCSSOST_ToBePaid + x.FTPOST_PaidAmount;
                    lorg4.FCSSOST_UpdatedDate = DateTime.Now;
                    _YearlyFeeGroupMappingContext.Update(lorg4);
                }

                if (lorg3.Any())
                {
                    for (int i = 0; lorg3.Count > i; i++)
                    {
                        _YearlyFeeGroupMappingContext.Remove(lorg3.ElementAt(i));
                    }
                }
                _YearlyFeeGroupMappingContext.Remove(lorg2);
                _YearlyFeeGroupMappingContext.Remove(lorg1);


                var contactExists = _YearlyFeeGroupMappingContext.SaveChanges();
                if (contactExists >= 1)
                {
                    data.returnval = "Delete";
                }
                else
                {
                    data.returnval = "Cancel";
                }


            }
            catch (Exception ee)
            {
                data.returnval = "Error";
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public CollegeStaffAndOtherTransactionDTO getacademicyear(CollegeStaffAndOtherTransactionDTO data)
        {
            try
            {
                var fetchmaxfypid = _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.User_Id == data.userid).OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                //           data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                //                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StudentDMO
                //                                     from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                                     from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                                     from e in _YearlyFeeGroupMappingContext.admissioncls
                //                                     from f in _YearlyFeeGroupMappingContext.school_M_Section
                //                                     where (f.ASMS_Id == d.ASMS_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_Id == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                //                                     select new CollegeStaffAndOtherTransactionDTO
                //                                     {
                //                                         Amst_Id = c.AMST_Id,
                //                                         AMST_FirstName = c.AMST_FirstName,
                //                                         AMST_MiddleName = c.AMST_MiddleName,
                //                                         AMST_LastName = c.AMST_LastName,
                //                                         FYP_Receipt_No = a.FYP_ReceiptNo,
                //                                         FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                //                                         FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                //                                         classname = e.ASMCL_ClassName,
                //                                         sectionname = f.ASMC_SectionName,
                //                                         FYP_Id = a.FYP_Id,
                //                                         AMST_AdmNo = c.AMST_AdmNo,
                //                                         FYP_Date = a.FYP_ReceiptDate
                //                                     }
                //).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
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
                                              FYP_ApprovedFlg = a.FYP_ApprovedFlg,
                                              UserId = a.User_Id
                                          }
       ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

                data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                  where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                  select new Temp_Staff_DTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                  }
             ).OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

                data.oth_studentlist = _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO.Where(s => s.MI_Id == data.MI_Id && s.FMOST_ActiveFlag == true).ToList().Distinct().OrderBy(t => t.FMOST_Id).ToArray();

                data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_College_StaffDMO
                                           from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                           from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                           from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                           from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                           where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.User_Id == data.userid && a.ASMAY_Id == data.ASMAY_Id)
                                           select new CollegeStaffAndOtherTransactionDTO
                                           {
                                               FYP_Id = a.FYP_Id,
                                               HRME_Id = d.HRME_Id,
                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                               HRMD_DepartmentName = e.HRMD_DepartmentName,
                                               HRMDES_DesignationName = f.HRMDES_DesignationName,
                                               FYP_Receipt_No = a.FYP_ReceiptNo,
                                               FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                               FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                               FYP_Date = a.FYP_ReceiptDate
                                           }).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.Fee_Y_PaymentDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStu_CollegeDMO
                                            from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaff_College_CollegeDMO
                                            from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                            where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMCOST_Id == d.FMOST_Id && d.FMOST_ActiveFlag == true && a.User_Id == data.userid && a.ASMAY_Id == data.ASMAY_Id)
                                            select new CollegeStaffAndOtherTransactionDTO
                                            {
                                                FYP_Id = a.FYP_Id,
                                                FMOST_Id = d.FMOST_Id,
                                                FMOST_StudentName = d.FMOST_StudentName,
                                                FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                FYP_Receipt_No = a.FYP_ReceiptNo,
                                                FYP_Bank_Or_Cash = a.FYP_TransactionTypeFlag,
                                                FYP_Tot_Amount = a.FYP_TotalPaidAmount,
                                                FYP_Date = a.FYP_ReceiptDate
                                            }).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
