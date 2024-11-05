using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using FeeServiceHub.com.vaps.interfaces;
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
using DomainModel.Model.com.vapstech.Fee;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeStaffOthersTransactionImpl : interfaces.FeeStaffOthersTransactionInterface
    {
        private static ConcurrentDictionary<string, FeeStaffOthersTransactionDTO> _login =
        new ConcurrentDictionary<string, FeeStaffOthersTransactionDTO>();

        private static readonly Object obj = new Object();

        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        readonly ILogger<FeeStaffOthersTransactionImpl> _logger;
        public FeeStaffOthersTransactionImpl(FeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context, ILogger<FeeStaffOthersTransactionImpl> log)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
            _logger = log;
        }

        public FeeStaffOthersTransactionDTO getdata(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _YearlyFeeGroupMappingContext.feemastersettings.Where(t => t.MI_Id == data.MI_Id && t.userid == data.userid).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                List<FeeGroupDMO> group = new List<FeeGroupDMO>();
                group = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(t => t.FMG_ActiceFlag == true && t.MI_Id == data.MI_Id).ToList();
                data.fillmastergroup = group.ToArray();

                List<Master_Numbering> masnum = new List<Master_Numbering>();
                masnum = _YearlyFeeGroupMappingContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag== "Transaction").ToList();
                data.transnumconfig = masnum.ToArray();

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.fillyear = allyear.Distinct().ToArray();

                var rolename= _YearlyFeeGroupMappingContext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.roleid).IVRMRT_Role;

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
                
                var fetchmaxfypid = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ID == data.ASMAY_Id && t.user_id == data.userid).OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                          from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                          from e in _YearlyFeeGroupMappingContext.admissioncls
                                          from f in _YearlyFeeGroupMappingContext.school_M_Section
                                          where ( f.ASMS_Id==d.ASMS_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                          select new FeeStaffOthersTransactionDTO
                                          {
                                              Amst_Id = c.AMST_Id,
                                              AMST_FirstName = c.AMST_FirstName,
                                              AMST_MiddleName = c.AMST_MiddleName,
                                              AMST_LastName = c.AMST_LastName,
                                              FYP_Receipt_No = a.FYP_Receipt_No,
                                              FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                              FYP_Tot_Amount = a.FYP_Tot_Amount,
                                              classname = e.ASMCL_ClassName,
                                              sectionname = f.ASMC_SectionName,
                                              FYP_Id = a.FYP_Id,
                                              AMST_AdmNo = c.AMST_AdmNo,
                                              FYP_Date = a.FYP_Date
                                          }
     ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

                data.stafflist = (from a in _YearlyFeeGroupMappingContext.MasterEmployee
                                  where ( a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                  select new Temp_Staff_DTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName)).Trim(),
                                  }
             ).OrderBy(t => t.HRME_EmployeeFirstName).ToArray();

                data.oth_studentlist = _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO.Where(s => s.MI_Id == data.MI_Id && s.FMOST_ActiveFlag == true).ToList().Distinct().OrderBy(t => t.FMOST_Id).ToArray();

                data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                           from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                           from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                           from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                           from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                          where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && d.HRME_ActiveFlag==true && e.HRMD_Id==d.HRMD_Id && e.HRMD_ActiveFlag==true && f.HRMDES_Id==d.HRMDES_Id && f.HRMDES_ActiveFlag==true && a.user_id==data.userid && a.ASMAY_ID==data.ASMAY_Id)
                                          select new FeeStaffOthersTransactionDTO
                                          {
                                              FYP_Id = a.FYP_Id,
                                              HRME_Id = d.HRME_Id,
                                              HRME_EmployeeCode = d.HRME_EmployeeCode,
                                              HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                              HRMD_DepartmentName=e.HRMD_DepartmentName,
                                              HRMDES_DesignationName=f.HRMDES_DesignationName,
                                              FYP_Receipt_No = a.FYP_Receipt_No,
                                              FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                              FYP_Tot_Amount = a.FYP_Tot_Amount,
                                              FYP_Date = a.FYP_Date
                                          }).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                           from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                           from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                            where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id &&  a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMOST_Id == d.FMOST_Id && d.FMOST_ActiveFlag == true && a.user_id == data.userid && a.ASMAY_ID==data.ASMAY_Id)
                                           select new FeeStaffOthersTransactionDTO
                                           {
                                               FYP_Id = a.FYP_Id,
                                               FMOST_Id = d.FMOST_Id,
                                               FMOST_StudentName = d.FMOST_StudentName,
                                               FMOST_StudentMobileNo=d.FMOST_StudentMobileNo,
                                               FMOST_StudentEmailId=d.FMOST_StudentEmailId,
                                               FYP_Receipt_No = a.FYP_Receipt_No,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_Tot_Amount = a.FYP_Tot_Amount,
                                               FYP_Date = a.FYP_Date
                                           }).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public FeeStaffOthersTransactionDTO Printterm(FeeStaffOthersTransactionDTO data)
        {
            try
            {

                data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                          from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                          from f in _YearlyFeeGroupMappingContext.feespecialHead
                                          from g in _YearlyFeeGroupMappingContext.feeSGGG
                                          where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id)
                                          select new FeeStaffOthersTransactionDTO
                                          {
                                              FMH_FeeName = f.FMSFH_Name,
                                              FMH_Id = e.FMH_Id,
                                              FTP_Paid_Amt = c.FTP_Paid_Amt,
                                              FTP_Concession_Amt = c.FTP_Concession_Amt,
                                              FTP_Fine_Amt = c.FTP_Fine_Amt,
                                              FTI_Id = d.FTI_Id,
                                              totalcharges=d.FMA_Amount
                                          }
              ).ToArray();

                var findfmgid = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                 select new FeeStaffOthersTransactionDTO
                                 {
                                     FMG_Id = d.FMG_Id,
                                     FMH_Id = d.FMH_Id
                                 }).Distinct().ToArray();

                data.termremarks = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    from c in _YearlyFeeGroupMappingContext.feeMTH
                                    from d in _YearlyFeeGroupMappingContext.feeTr
                                    where c.FMT_Id == d.FMT_Id && a.FMA_Id == b.FMA_Id && b.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id
                                    select new FeeStaffOthersTransactionDTO
                                    {
                                        termname = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id
                                           select new FeeStaffOthersTransactionDTO
                                           {
                                               FTP_Paid_Amt = a.FYP_Tot_Amount,
                                               FTP_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                               FYP_Date = a.FYP_Date,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,
                                           }
               ).Distinct().ToArray();

                data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                               from b in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                               from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                               from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                               from f in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                               from g in _YearlyFeeGroupMappingContext.admissioncls
                                               from h in _YearlyFeeGroupMappingContext.school_M_Section
                                               from i in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                               from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                               from k in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                                               where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id && b.FYP_Id == data.FYP_Id)
                                               select new FeeStaffOthersTransactionDTO
                                               {
                                                   Amst_Id = f.AMST_Id,
                                                   AMST_FirstName = i.AMST_FirstName,
                                                   AMST_MiddleName = i.AMST_MiddleName,
                                                   AMST_LastName = i.AMST_LastName,
                                                   FMH_Id = d.FMH_Id,
                                                   FMH_FeeName = d.FMH_FeeName,
                                                   FTI_Name = e.FTI_Name,
                                                   FTI_Id = e.FTI_Id,
                                                   FYP_Receipt_No = b.FYP_Receipt_No,
                                                   FTP_Paid_Amt = a.FTP_Paid_Amt,
                                                   FTP_Concession_Amt = a.FTP_Concession_Amt,
                                                   FTP_Fine_Amt = a.FTP_Fine_Amt,
                                                   FYP_Date = b.FYP_Date,
                                                   classname = g.ASMCL_ClassName,
                                                   sectionname = h.ASMC_SectionName,
                                                   rollno = f.AMAY_RollNo,
                                                   admno = i.AMST_AdmNo,
                                                   fathername = i.AMST_FatherName,
                                                   mothername = i.AMST_MotherName,
                                                   FYP_Bank_Or_Cash = b.FYP_Bank_Or_Cash,
                                                   FYP_DD_Cheque_No = b.FYP_DD_Cheque_No,
                                                   FYP_DD_Cheque_Date = b.FYP_DD_Cheque_Date,
                                                   FYP_Bank_Name = b.FYP_Bank_Name,
                                                   FYP_Remarks = b.FYP_Remarks,
                                                   AMST_RegistrationNo = i.AMST_RegistrationNo,
                                                   FMCC_ConcessionName = k.FMCC_ConcessionName,
                                                   totalcharges = c.FMA_Amount
                                               }
               ).Distinct().ToArray();

                data.filltotaldetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                         where (a.ASMAY_ID == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id)
                                         select new FeeStaffOthersTransactionDTO
                                         {
                                             FYP_Tot_Amount = a.FYP_Tot_Amount,
                                             FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                             FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                         }
              ).Distinct().ToArray();



                //to find next due amount
                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id==data.FYP_Id)
                               select new FeeStaffOthersTransactionDTO
                               {
                                   //fmt_id = e.FMT_Id
                                   FMT_Id = e.FMT_Id
                               }

                 ).Distinct().OrderByDescending(t=>t.FMT_Id).ToArray();

                long fmt_id_new = 0;
                //string fmt_id_new = '0';

                //for(int r=0;r<feeterm.Length;r++)
                //{
                //    fmt_id_new= fmt_id_new + ',' + feeterm[0].fmt_id
                //}

                long initialfmtids = Convert.ToInt64(feeterm[0].FMT_Id);

                fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;
                //fmt_id_new = 10;


                List<FeeStaffOthersTransactionDTO> temp_group_head = new List<FeeStaffOthersTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new FeeStaffOthersTransactionDTO
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

                List<FeeStaffOthersTransactionDTO> fordate = new List<FeeStaffOthersTransactionDTO>();
                List<FeeStaffOthersTransactionDTO> fordateinfyp = new List<FeeStaffOthersTransactionDTO>();

                fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                           from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                           where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && (d.FSS_ToBePaid>0))
                           select new FeeStaffOthersTransactionDTO
                           {
                               date = f.FTDD_Day,
                               month = f.FTDD_Month,
                           }
                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new FeeStaffOthersTransactionDTO
                                {
                                    month = f.FTDD_Month,
                                }
                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (FeeStaffOthersTransactionDTO item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (FeeStaffOthersTransactionDTO itemperiod in fordateinfyp)
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

                // data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                   from b in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                //                   where (b.FTDD_Day == data.date && b.FTDD_Month == data.month && a.FMA_Id == b.FMA_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && head_ids.Contains(a.FMH_Id) && !inst_ids.Contains(a.FTI_Id))
                //                   select new FeeStaffOthersTransactionDTO
                //                   {
                //                       FSS_ToBePaid = a.FSS_ToBePaid
                //                   }
                //).Sum(t => t.FSS_ToBePaid);


                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                  where (b.FMH_Id==c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag!="E")
                                  select new FeeStaffOthersTransactionDTO
                                  {
                                      FSS_ToBePaid = a.FSS_ToBePaid
                                  }
             ).Sum(t => t.FSS_ToBePaid);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeStaffOthersTransactionDTO Printinstallment(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                          from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                          from f in _YearlyFeeGroupMappingContext.feespecialHead
                                          from g in _YearlyFeeGroupMappingContext.feeSGGG
                                          where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id)
                                          select new FeeStaffOthersTransactionDTO
                                          {
                                              FMH_FeeName = f.FMSFH_Name,
                                              FMH_Id = e.FMH_Id,
                                              FTP_Paid_Amt = c.FTP_Paid_Amt,
                                              FTP_Concession_Amt = c.FTP_Concession_Amt,
                                              FTP_Fine_Amt = c.FTP_Fine_Amt,
                                              FTI_Id = d.FTI_Id
                                          }
              ).ToArray();

                var findfmgid = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                 select new FeeStaffOthersTransactionDTO
                                 {
                                     FMG_Id = d.FMG_Id,
                                     FMH_Id = d.FMH_Id
                                 }).Distinct().ToArray();

                data.termremarks = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    from c in _YearlyFeeGroupMappingContext.feeMTH
                                    from d in _YearlyFeeGroupMappingContext.feeTr
                                    where c.FMT_Id == d.FMT_Id && a.FMA_Id == b.FMA_Id && b.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id
                                    select new FeeStaffOthersTransactionDTO
                                    {
                                        termname = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id
                                           select new FeeStaffOthersTransactionDTO
                                           {
                                               FTP_Paid_Amt = a.FYP_Tot_Amount,
                                               FTP_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                               FYP_Date = a.FYP_Date,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,
                                           }
               ).Distinct().ToArray();




                //data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                //                               from b in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                               from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                //                               from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                               from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                //                               from f in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                               from g in _YearlyFeeGroupMappingContext.admissioncls
                //                               from h in _YearlyFeeGroupMappingContext.school_M_Section
                //                               from i in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                               from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                //                               from k in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                //                               from l in _YearlyFeeGroupMappingContext.feeMI 
                //                               where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && e.FMI_Id==l.FMI_Id  && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id && b.FYP_Id == data.FYP_Id)
                //                               group new { a,b, c,d,f,g,h,i,j,k,l } by new {  a.FTP_Paid_Amt, a.FTP_Concession_Amt, a.FTP_Fine_Amt, b.FYP_Receipt_No, b.FYP_Date, b.FYP_Bank_Or_Cash, b.FYP_DD_Cheque_No, b.FYP_DD_Cheque_Date, b.FYP_Bank_Name, b.FYP_Remarks, d.FMH_Id, d.FMH_FeeName, f.AMST_Id, f.AMAY_RollNo, g.ASMCL_ClassName, h.ASMC_SectionName, i.AMST_FirstName, i.AMST_MiddleName, i.AMST_LastName, i.AMST_AdmNo, i.AMST_FatherName, i.AMST_MotherName, i.AMST_RegistrationNo, k.FMCC_ConcessionName,l.FMI_Id,l.FMI_Installment_Type } into grp
                //                               select new FeeStaffOthersTransactionDTO
                //                               {
                //                                   Amst_Id = grp.Key.AMST_Id,
                //                                   AMST_FirstName =grp.Key.AMST_FirstName,
                //                                   AMST_MiddleName=grp.Key.AMST_MiddleName,
                //                                   AMST_LastName=grp.Key.AMST_LastName,
                //                                   FMH_Id=grp.Key.FMH_Id,
                //                                   FMH_FeeName=grp.Key.FMH_FeeName,
                //                                   FYP_Receipt_No=grp.Key.FYP_Receipt_No,
                //                                   FTP_Paid_Amt = grp.Sum(x => x.a.FTP_Paid_Amt),
                //                                   FTP_Concession_Amt = grp.Sum(y => y.a.FTP_Concession_Amt),
                //                                   FTP_Fine_Amt = grp.Sum(z => z.a.FTP_Fine_Amt),
                //                                   FYP_Date=grp.Key.FYP_Date,
                //                                   classname=grp.Key.ASMCL_ClassName,
                //                                   sectionname = grp.Key.ASMC_SectionName,
                //                                   rollno = grp.Key.AMAY_RollNo,
                //                                   admno = grp.Key.AMST_AdmNo,
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

                List<FeeStaffOthersTransactionDTO> result = new List<FeeStaffOthersTransactionDTO>();

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
                            //while (dataReader.Read())
                            //{
                            //    result.Add(new FeeStaffOthersTransactionDTO
                            //    {
                            //        //FMCC_ClassCategoryName = dataReader["FMCC_ClassCategoryName"].ToString(),
                            //        //ASMCL_ClassName = Convert.ToString(dataReader["ASMCL_ClassName"]),
                            //        //ASMC_SectionName = Convert.ToString(dataReader["ASMC_SectionName"]),

                            //        Amst_Id = Convert.ToInt16(dataReader["Amst_Id"]),
                            //        AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                            //        AMST_MiddleName = dataReader["AMST_MiddleName"].ToString(),
                            //        AMST_LastName = dataReader["AMST_LastName"].ToString(),
                            //        FMH_Id = Convert.ToInt16(dataReader["FMH_Id"]),
                            //        FMH_FeeName = dataReader["FMH_FeeName"].ToString(),
                            //        //FTI_Name = dataReader["FTI_Name"].ToString(),
                            //        //FTI_Id = Convert.ToInt16(dataReader["FTI_Id"]),
                            //        FYP_Receipt_No = dataReader["FYP_Receipt_No"].ToString(),
                            //        FTP_Paid_Amt =Convert.ToDecimal(dataReader["FTP_Paid_Amt"]),
                            //        FTP_Concession_Amt = Convert.ToDecimal(dataReader["FTP_Concession_Amt"]),
                            //        FTP_Fine_Amt = Convert.ToDecimal(dataReader["FTP_Fine_Amt"]),
                            //        FYP_Date = Convert.ToDateTime(dataReader["FYP_Date"]),
                            //        classname = dataReader["ASMCL_ClassName"].ToString(),
                            //        sectionname = dataReader["ASMC_SectionName"].ToString(),
                            //        rollno = Convert.ToInt16(dataReader["AMAY_RollNo"]),
                            //        admno = dataReader["AMST_AdmNo"].ToString(),
                            //        fathername = dataReader["AMST_FatherName"].ToString(),
                            //        mothername = dataReader["AMST_MotherName"].ToString(),
                            //        FYP_Bank_Or_Cash = dataReader["FYP_Bank_Or_Cash"].ToString(),
                            //        FYP_DD_Cheque_No = dataReader["FYP_DD_Cheque_No"].ToString(),
                            //        FYP_DD_Cheque_Date = Convert.ToDateTime(dataReader["FYP_DD_Cheque_Date"]),
                            //        FYP_Bank_Name = dataReader["FYP_Bank_Name"].ToString(),
                            //        FYP_Remarks = dataReader["FYP_Remarks"].ToString(),
                            //        AMST_RegistrationNo = dataReader["AMST_RegistrationNo"].ToString(),
                            //        FMCC_ConcessionName = dataReader["AMST_LastName"].ToString(),

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




                data.filltotaldetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                         where (a.ASMAY_ID == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id)
                                         select new FeeStaffOthersTransactionDTO
                                         {
                                             FYP_Tot_Amount = a.FYP_Tot_Amount,
                                             FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                             FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                         }
              ).Distinct().ToArray();



                //to find next due amount
                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id)
                               select new FeeStaffOthersTransactionDTO
                               {
                                   // fmt_id = e.FMT_Id
                                   FMT_Id = e.FMT_Id
                               }

                 ).Distinct().ToArray();


                long fmt_id_new = 0;

                long initialfmtids = Convert.ToInt64(feeterm[0].FMT_Id);

                fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;


                List<FeeStaffOthersTransactionDTO> temp_group_head = new List<FeeStaffOthersTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new FeeStaffOthersTransactionDTO
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

                List<FeeStaffOthersTransactionDTO> fordate = new List<FeeStaffOthersTransactionDTO>();
                List<FeeStaffOthersTransactionDTO> fordateinfyp = new List<FeeStaffOthersTransactionDTO>();

                fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                           from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                           where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id))
                           select new FeeStaffOthersTransactionDTO
                           {
                               date = f.FTDD_Day,
                               month = f.FTDD_Month,
                           }

                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new FeeStaffOthersTransactionDTO
                                {
                                    month = f.FTDD_Month,
                                }

                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (FeeStaffOthersTransactionDTO item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (FeeStaffOthersTransactionDTO itemperiod in fordateinfyp)
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

                // data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                   from b in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                //                   where (b.FTDD_Day == data.date && b.FTDD_Month == data.month && a.FMA_Id == b.FMA_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && head_ids.Contains(a.FMH_Id) && !inst_ids.Contains(a.FTI_Id))
                //                   select new FeeStaffOthersTransactionDTO
                //                   {
                //                       FSS_ToBePaid = a.FSS_ToBePaid
                //                   }
                //).Sum(t => t.FSS_ToBePaid);


                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag != "E")
                                  select new FeeStaffOthersTransactionDTO
                                  {
                                      FSS_ToBePaid = a.FSS_ToBePaid
                                  }
          ).Sum(t => t.FSS_ToBePaid);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
      
        public FeeStaffOthersTransactionDTO duplicaterecept(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                data.duplicatereceipt = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                         where (a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Receipt_No.Trim().Equals(data.FYP_Receipt_No.Trim()))
                                         select new FeeStaffOthersTransactionDTO
                                         {
                                             FYP_Receipt_No = a.FYP_Receipt_No
                                         }
              ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeStaffOthersTransactionDTO get_grp_reptno(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                if (data.auto_receipt_flag == 1)
                {
                    List<long> HeadId = new List<long>();
                    foreach (var item in data.head_installments)
                    {
                        HeadId.Add(item.FMH_Id);
                    }

                    List<FeeStaffOthersTransactionDTO> grps = new List<FeeStaffOthersTransactionDTO>();
                    grps = (from b in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO

                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

                            select new FeeStaffOthersTransactionDTO
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
                    List<FeeStaffOthersTransactionDTO> list_all = new List<FeeStaffOthersTransactionDTO>();
                    List<FeeStaffOthersTransactionDTO> list_repts = new List<FeeStaffOthersTransactionDTO>();

                    list_all = (from b in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceiptDMO
                                from c in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new FeeStaffOthersTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    //FGAR_Name = b.FGAR_Name,
                                    //FMG_Id = c.FMG_Id
                                }
                         ).Distinct().ToList();

                    data.grp_count = list_all.Count();

                    if (data.grp_count == 1)
                    {
                        //list_repts = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                        //              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                        //              from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                        //              where (a.MI_Id == c.MI_Id && c.ASMAY_Id == a.ASMAY_ID && a.ASMAY_ID == b.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && grpid.Contains(c.FMG_Id) && a.FYP_Receipt_No.StartsWith(list_all[0].FGAR_PrefixName) && a.FYP_Receipt_No.EndsWith(list_all[0].FGAR_SuffixName))
                        //              select new FeeStaffOthersTransactionDTO
                        //              {
                        //                  FYP_Receipt_No = a.FYP_Receipt_No
                        //              }
                        //           ).Distinct().ToList();

                        //var prefix = list_all[0].FGAR_PrefixName;
                        //var prefix_len = prefix.Length;
                        //var suffix = list_all[0].FGAR_SuffixName;
                        //var suffix_len = suffix.Length;

                        //var max_num = "";
                        //var rept_no = "";
                        //var rept_prefix = "";
                        //var rept_suffix = "";
                        //var max_num_int = 0;
                        //List<int> number_max = new List<int>();
                        //for (int z = 0; z < list_repts.Count(); z++)
                        //{
                        //    rept_no = list_repts[z].FYP_Receipt_No;
                        //    var num_1 = rept_no.Substring(0, 1);
                        //    if (num_1 != "0" && num_1 != "1" && num_1 != "2" && num_1 != "3" && num_1 != "4" && num_1 != "5" && num_1 != "6" && num_1 != "7" && num_1 != "8" && num_1 != "9")
                        //    {
                        //        rept_prefix = rept_no.Substring(0, prefix_len);
                        //        rept_suffix = rept_no.Substring((rept_no.Length - suffix_len), suffix_len);
                        //        max_num = rept_no.Substring(0, (rept_no.Length - suffix_len));
                        //        max_num = max_num.Substring(prefix_len, (max_num.Length - prefix_len));
                        //    }
                        //    if (max_num != "" && rept_prefix == prefix && rept_suffix == suffix)
                        //    {
                        //        number_max.Add(Convert.ToInt32(max_num));
                        //    }

                        //}
                        //if (number_max.Count() > 0)
                        //{
                        //    max_num_int = number_max.Max();
                        //}
                        //max_num_int += 1;

                        //final_rept_no = prefix + max_num_int.ToString().Trim() + suffix;


                        //var receiptno = _YearlyFeeGroupMappingContext.Database.ExecuteSqlCommand("receiptnogeneration @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, grpid, final_rept_no);



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

                        //data.FYP_Receipt_No = final_rept_no;
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

        //public FeeStaffOthersTransactionDTO get_grp_reptno(FeeStaffOthersTransactionDTO data)
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

        //            List<FeeStaffOthersTransactionDTO> grps = new List<FeeStaffOthersTransactionDTO>();
        //            grps = (from b in _YearlyFeeGroupMappingContext.FeeYearlygroupHeadMappingDMO

        //                    where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

        //                    select new FeeStaffOthersTransactionDTO
        //                    {
        //                        FMG_Id = b.FMG_Id

        //                    }
        //                   ).Distinct().ToList();

        //            List<long> grpid = new List<long>();
        //            foreach (var item in grps)
        //            {
        //                grpid.Add(item.FMG_Id);
        //            }

        //            var final_rept_no = "";
        //            List<FeeStaffOthersTransactionDTO> list_all = new List<FeeStaffOthersTransactionDTO>();
        //            List<FeeStaffOthersTransactionDTO> list_repts = new List<FeeStaffOthersTransactionDTO>();

        //            list_all = (from b in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceiptDMO
        //                        from c in _YearlyFeeGroupMappingContext.Fee_Groupwise_AutoReceipt_GroupsDMO
        //                        where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

        //                        select new FeeStaffOthersTransactionDTO
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
        //                list_repts = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
        //                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
        //                              from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
        //                              where (a.MI_Id == c.MI_Id && c.ASMAY_Id == a.ASMAY_ID && a.ASMAY_ID == b.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && grpid.Contains(c.FMG_Id) && a.FYP_Receipt_No.StartsWith(list_all[0].FGAR_PrefixName) && a.FYP_Receipt_No.EndsWith(list_all[0].FGAR_SuffixName))
        //                              select new FeeStaffOthersTransactionDTO
        //                              {
        //                                  FYP_Receipt_No = a.FYP_Receipt_No
        //                              }
        //                           ).Distinct().ToList();

        //                var prefix = list_all[0].FGAR_PrefixName;
        //                var prefix_len = prefix.Length;
        //                var suffix = list_all[0].FGAR_SuffixName;
        //                var suffix_len = suffix.Length;

        //                var max_num = "";
        //                var rept_no = "";
        //                var rept_prefix = "";
        //                var rept_suffix = "";
        //                var max_num_int = 0;
        //                List<int> number_max = new List<int>();
        //                for (int z = 0; z < list_repts.Count(); z++)
        //                {
        //                    rept_no = list_repts[z].FYP_Receipt_No;
        //                    var num_1 = rept_no.Substring(0, 1);
        //                    if (num_1 != "0" && num_1 != "1" && num_1 != "2" && num_1 != "3" && num_1 != "4" && num_1 != "5" && num_1 != "6" && num_1 != "7" && num_1 != "8" && num_1 != "9")
        //                    {
        //                        rept_prefix = rept_no.Substring(0, prefix_len);
        //                        rept_suffix = rept_no.Substring((rept_no.Length - suffix_len), suffix_len);
        //                        max_num = rept_no.Substring(0, (rept_no.Length - suffix_len));
        //                        max_num = max_num.Substring(prefix_len, (max_num.Length - prefix_len));
        //                    }
        //                    if (max_num != "" && rept_prefix == prefix && rept_suffix == suffix)
        //                    {
        //                        number_max.Add(Convert.ToInt32(max_num));
        //                    }

        //                }
        //                if (number_max.Count() > 0)
        //                {
        //                    max_num_int = number_max.Max();
        //                }
        //                max_num_int += 1;

        //                final_rept_no = prefix + max_num_int.ToString().Trim() + suffix;

        //            }

        //            data.auto_FYP_Receipt_No = final_rept_no;

        //            data.FYP_Receipt_No = final_rept_no;
        //        }

        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return data;
        //}

       
     

        //public FeeStaffOthersTransactionDTO searching(FeeStaffOthersTransactionDTO data)
        //{

        //    try
        //    {

        //        switch (data.searchType)
        //        {

        //            case "0":
        //                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
        //                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
        //                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
        //                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
        //                                    from e in _YearlyFeeGroupMappingContext.admissioncls
        //                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id  && ((c.AMST_FirstName.Trim() + ' ' + c.AMST_MiddleName.Trim() + ' ' + c.AMST_LastName.Trim()).Contains(data.searchtext) || c.AMST_FirstName.StartsWith(data.searchtext) || c.AMST_MiddleName.StartsWith(data.searchtext) || c.AMST_LastName.StartsWith(data.searchtext)) && d.ASMAY_Id == data.ASMAY_Id)
        //                                    select new FeeStaffOthersTransactionDTO
        //                                    {
        //                                        Amst_Id = c.AMST_Id,
        //                                        AMST_FirstName = c.AMST_FirstName,
        //                                        AMST_MiddleName = c.AMST_MiddleName,
        //                                        AMST_LastName = c.AMST_LastName,
        //                                        FYP_Receipt_No = a.FYP_Receipt_No,
        //                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                        classname = e.ASMCL_ClassName,
        //                                        FYP_Id = a.FYP_Id,
        //                                        AMST_AdmNo = c.AMST_AdmNo,
        //                                        FYP_Date = a.FYP_Date
        //                                    }
        //      ).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
        //                break;
        //            case "1":
        //                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
        //                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
        //                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
        //                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
        //                                    from e in _YearlyFeeGroupMappingContext.admissioncls
        //                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && e.ASMCL_ClassName.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
        //                                    select new FeeStaffOthersTransactionDTO
        //                                    {
        //                                        Amst_Id = c.AMST_Id,
        //                                        AMST_FirstName = c.AMST_FirstName,
        //                                        AMST_MiddleName = c.AMST_MiddleName,
        //                                        AMST_LastName = c.AMST_LastName,
        //                                        FYP_Receipt_No = a.FYP_Receipt_No,
        //                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                        classname = e.ASMCL_ClassName,
        //                                        FYP_Id = a.FYP_Id,
        //                                        AMST_AdmNo = c.AMST_AdmNo,
        //                                        FYP_Date = a.FYP_Date
        //                                    }
        //      ).Distinct().OrderBy(t => t.classname).ToArray();
        //                break;
        //            case "2":
        //                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
        //                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
        //                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
        //                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
        //                                    from e in _YearlyFeeGroupMappingContext.admissioncls
        //                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && c.AMST_AdmNo.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
        //                                    select new FeeStaffOthersTransactionDTO
        //                                    {
        //                                        Amst_Id = c.AMST_Id,
        //                                        AMST_FirstName = c.AMST_FirstName,
        //                                        AMST_MiddleName = c.AMST_MiddleName,
        //                                        AMST_LastName = c.AMST_LastName,
        //                                        FYP_Receipt_No = a.FYP_Receipt_No,
        //                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                        classname = e.ASMCL_ClassName,
        //                                        FYP_Id = a.FYP_Id,
        //                                        AMST_AdmNo = c.AMST_AdmNo,
        //                                        FYP_Date = a.FYP_Date
        //                                    }
        //      ).Distinct().OrderBy(t => t.AMST_AdmNo).ToArray();
        //                break;
        //            case "3":
        //                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
        //                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
        //                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
        //                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
        //                                    from e in _YearlyFeeGroupMappingContext.admissioncls
        //                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Receipt_No.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
        //                                    select new FeeStaffOthersTransactionDTO
        //                                    {
        //                                        Amst_Id = c.AMST_Id,
        //                                        AMST_FirstName = c.AMST_FirstName,
        //                                        AMST_MiddleName = c.AMST_MiddleName,
        //                                        AMST_LastName = c.AMST_LastName,
        //                                        FYP_Receipt_No = a.FYP_Receipt_No,
        //                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                        classname = e.ASMCL_ClassName,
        //                                        FYP_Id = a.FYP_Id,
        //                                        AMST_AdmNo = c.AMST_AdmNo,
        //                                        FYP_Date = a.FYP_Date
        //                                    }
        //      ).Distinct().OrderBy(t => t.FYP_Receipt_No).ToArray();
        //                break;
        //            case "4":
        //                var date_format = data.searchdate.ToString("dd/MM/yyyy");
        //                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
        //                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
        //                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
        //                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
        //                                    from e in _YearlyFeeGroupMappingContext.admissioncls
        //                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id  && a.FYP_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy") && d.ASMAY_Id == data.ASMAY_Id)
        //                                    select new FeeStaffOthersTransactionDTO
        //                                    {
        //                                        Amst_Id = c.AMST_Id,
        //                                        AMST_FirstName = c.AMST_FirstName,
        //                                        AMST_MiddleName = c.AMST_MiddleName,
        //                                        AMST_LastName = c.AMST_LastName,
        //                                        FYP_Receipt_No = a.FYP_Receipt_No,
        //                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                        classname = e.ASMCL_ClassName,
        //                                        FYP_Id = a.FYP_Id,
        //                                        AMST_AdmNo = c.AMST_AdmNo,
        //                                        FYP_Date = a.FYP_Date
        //                                    }
        //      ).Distinct().OrderBy(t => t.FYP_Date).ToArray();
        //                break;
        //            case "5":
        //                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
        //                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
        //                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
        //                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
        //                                    from e in _YearlyFeeGroupMappingContext.admissioncls
        //                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Tot_Amount.ToString().Contains(data.searchnumber) && d.ASMAY_Id == data.ASMAY_Id)
        //                                    select new FeeStaffOthersTransactionDTO
        //                                    {
        //                                        Amst_Id = c.AMST_Id,
        //                                        AMST_FirstName = c.AMST_FirstName,
        //                                        AMST_MiddleName = c.AMST_MiddleName,
        //                                        AMST_LastName = c.AMST_LastName,
        //                                        FYP_Receipt_No = a.FYP_Receipt_No,
        //                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                        classname = e.ASMCL_ClassName,
        //                                        FYP_Id = a.FYP_Id,
        //                                        AMST_AdmNo = c.AMST_AdmNo,
        //                                        FYP_Date = a.FYP_Date
        //                                    }
        //      ).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();
        //                break;
        //            case "6":
        //                data.searcharray = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
        //                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
        //                                    from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
        //                                    from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
        //                                    from e in _YearlyFeeGroupMappingContext.admissioncls
        //                                    where (a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id  && a.FYP_Bank_Or_Cash.Contains(data.searchtext) && d.ASMAY_Id == data.ASMAY_Id)
        //                                    select new FeeStaffOthersTransactionDTO
        //                                    {
        //                                        Amst_Id = c.AMST_Id,
        //                                        AMST_FirstName = c.AMST_FirstName,
        //                                        AMST_MiddleName = c.AMST_MiddleName,
        //                                        AMST_LastName = c.AMST_LastName,
        //                                        FYP_Receipt_No = a.FYP_Receipt_No,
        //                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
        //                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
        //                                        classname = e.ASMCL_ClassName,
        //                                        FYP_Id = a.FYP_Id,
        //                                        AMST_AdmNo = c.AMST_AdmNo,
        //                                        FYP_Date = a.FYP_Date
        //                                    }
        //      ).Distinct().OrderBy(t => t.FYP_Bank_Or_Cash).ToArray();

        //                break;
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return data;
        //}

        public FeeStaffOthersTransactionDTO edittra(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                if (data.Grp_Term_flg == "T" && data.Stf_Others_flg=="Staff")
                {
                    data.disableterms = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                         from b in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                         from d in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                         where (a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && b.HRME_Id == c.HRME_Id && d.FYP_Id == c.FYP_Id && d.FMAOST_Id == b.FMA_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.HRME_Id == data.HRME_Id && d.FYP_Id == data.FYP_Id)
                                         select new FeeStaffOthersTransactionDTO
                                         {
                                             FMG_Id = a.FMT_Id,
                                         }
                                        ).Distinct().ToArray();

                    data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.feeTr
                                            where (a.FMT_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                            select new FeeStaffOthersTransactionDTO
                                            {
                                                FMT_Id = a.FMT_Id,
                                                FMT_Name = a.FMT_Name,
                                            }
             ).OrderBy(t => t.FMH_Id).ToArray();
                }
                else if(data.Grp_Term_flg == "G" && data.Stf_Others_flg == "Staff")
                {
                    data.disableterms = (from b in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                         from d in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                         where (b.HRME_Id == c.HRME_Id && d.FYP_Id == c.FYP_Id && d.FMAOST_Id == b.FMA_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.HRME_Id == data.HRME_Id && d.FYP_Id == data.FYP_Id)
                                         select new FeeStaffOthersTransactionDTO
                                         {
                                             FMG_Id = b.FMG_Id,
                                         }
                                       ).Distinct().ToArray();

                    data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                            where (a.FMG_ActiceFlag == true && a.MI_Id == data.MI_Id && a.user_id==data.userid)
                                            select new FeeStaffOthersTransactionDTO
                                            {
                                                FMG_Id = a.FMG_Id,
                                                FMG_GroupName = a.FMG_GroupName,
                                            }
             ).OrderBy(t => t.FMH_Id).ToArray();
                }

                if (data.Grp_Term_flg == "T" && data.Stf_Others_flg == "Others")
                {
                    data.disableterms = (from a in _YearlyFeeGroupMappingContext.feeMTH
                                         from b in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                         from d in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                         where (a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && b.FMOST_Id == c.FMOST_Id && d.FYP_Id == c.FYP_Id && d.FMAOST_Id == b.FMA_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMOST_Id == data.HRME_Id && d.FYP_Id == data.FYP_Id)
                                         select new FeeStaffOthersTransactionDTO
                                         {
                                             FMG_Id = a.FMT_Id,
                                         }
                                        ).Distinct().ToArray();

                    data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.feeTr
                                            where (a.FMT_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                            select new FeeStaffOthersTransactionDTO
                                            {
                                                FMT_Id = a.FMT_Id,
                                                FMT_Name = a.FMT_Name,
                                            }
             ).OrderBy(t => t.FMH_Id).ToArray();
                }
                else if (data.Grp_Term_flg == "G" && data.Stf_Others_flg == "Others")
                {
                    data.disableterms = (from b in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                         from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                         from d in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                         where (b.FMOST_Id == c.FMOST_Id && d.FYP_Id == c.FYP_Id && d.FMAOST_Id == b.FMA_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FMOST_Id == data.HRME_Id && d.FYP_Id == data.FYP_Id)
                                         select new FeeStaffOthersTransactionDTO
                                         {
                                             FMG_Id = b.FMG_Id,
                                         }
                                       ).Distinct().ToArray();

                    data.fillmastergroup = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                            where (a.FMG_ActiceFlag == true && a.MI_Id == data.MI_Id && a.user_id == data.userid)
                                            select new FeeStaffOthersTransactionDTO
                                            {
                                                FMG_Id = a.FMG_Id,
                                                FMG_GroupName = a.FMG_GroupName,
                                            }
             ).OrderBy(t => t.FMH_Id).ToArray();
                }

                if(data.Stf_Others_flg == "Staff")
                {
                    data.receiparraydeleteall = (from a in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                 from c in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                                 from d in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                                 from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                 where (d.FMG_Id == e.FMG_Id && a.FMAOST_Id == d.FMA_Id && b.FMH_Id == d.FMH_Id && c.FTI_Id == d.FTI_Id && a.FYP_Id == data.FYP_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.HRME_Id == data.HRME_Id)
                                                 select new FeeStaffOthersTransactionDTO
                                                 {
                                                     FMG_GroupName = e.FMG_GroupName,
                                                     FMH_FeeName = b.FMH_FeeName,
                                                     FTI_Name = c.FTI_Name,
                                                     FMH_Id = b.FMH_Id,
                                                     FTI_Id = c.FTI_Id,
                                                     FMA_Id = a.FMAOST_Id,

                                                     totalamount = d.FSSST_NetAmount,
                                                     ToBePaid = a.FTPOST_PaidAmount,
                                                     ConcessionAmount = d.FSSST_ConcessionAmount,
                                                     FSS_FineAmount = d.FSSST_FineAmount,
                                                     CurrentYrCharges = d.FSSST_CurrentYrCharges,
                                                     totalcharges = d.FSSST_TotalCharges,
                                                     FYP_Id = a.FYP_Id

                                                 }).ToArray();

                    data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                              from c in _YearlyFeeGroupMappingContext.MasterEmployee
                                              from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                              from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                              where (a.FYP_Id == b.FYP_Id && b.HRME_Id == c.HRME_Id && c.HRMD_Id == e.HRMD_Id && c.HRMDES_Id == f.HRMDES_Id && c.HRME_Id == data.HRME_Id && a.ASMAY_ID == data.ASMAY_Id)
                                              select new FeeStaffOthersTransactionDTO
                                              {
                                                  HRME_Id = c.HRME_Id,
                                                  HRME_EmployeeFirstName = c.HRME_EmployeeFirstName,
                                                  HRME_EmployeeMiddleName = c.HRME_EmployeeMiddleName,
                                                  HRME_EmployeeLastName = c.HRME_EmployeeLastName,
                                                  FYP_Receipt_No = a.FYP_Receipt_No,
                                                  FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                  FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                  HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                  HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                  FYP_Id = a.FYP_Id,
                                                  HRME_EmployeeCode = c.HRME_EmployeeCode,
                                                  FYP_Date = a.FYP_Date,
                                                  FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                                  FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                                  FYP_Bank_Name = a.FYP_Bank_Name,
                                                  amst_mobile = c.HRME_MobileNo,
                                                  fathername = c.HRME_FatherName,
                                                  studentdob = c.HRME_DOB,
                                                  FYP_Remarks=a.FYP_Remarks
                                              }

        ).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                }

                if (data.Stf_Others_flg == "Others")
                {
                    data.receiparraydeleteall = (from a in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                                 from c in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                                                 from d in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                                 from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                 where (d.FMG_Id == e.FMG_Id && a.FMAOST_Id == d.FMA_Id && b.FMH_Id == d.FMH_Id && c.FTI_Id == d.FTI_Id && a.FYP_Id == data.FYP_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.FMOST_Id == data.HRME_Id)
                                                 select new FeeStaffOthersTransactionDTO
                                                 {
                                                     FMG_GroupName = e.FMG_GroupName,
                                                     FMH_FeeName = b.FMH_FeeName,
                                                     FTI_Name = c.FTI_Name,
                                                     FMH_Id = b.FMH_Id,
                                                     FTI_Id = c.FTI_Id,
                                                     FMA_Id = a.FMAOST_Id,

                                                     totalamount = d.FSSOST_NetAmount,
                                                     ToBePaid = a.FTPOST_PaidAmount,
                                                     ConcessionAmount = d.FSSOST_ConcessionAmount,
                                                     FSS_FineAmount = d.FSSOST_FineAmount,
                                                     CurrentYrCharges = d.FSSOST_CurrentYrCharges,
                                                     totalcharges = d.FSSOST_TotalCharges,
                                                     FYP_Id = a.FYP_Id

                                                 }).ToArray();

                    data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                              from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                              from c in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                              where (a.FYP_Id == b.FYP_Id && b.FMOST_Id == c.FMOST_Id  && c.FMOST_Id == data.HRME_Id && a.ASMAY_ID == data.ASMAY_Id)
                                              select new FeeStaffOthersTransactionDTO
                                              {
                                                  HRME_Id = c.FMOST_Id,
                                                  HRME_EmployeeFirstName = c.FMOST_StudentName,
                                                  HRME_EmployeeMiddleName = "",
                                                  HRME_EmployeeLastName = "",
                                                  FYP_Receipt_No = a.FYP_Receipt_No,
                                                  FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                  FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                  //HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                  //HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                  FYP_Id = a.FYP_Id,
                                                  //HRME_EmployeeCode = c.HRME_EmployeeCode,
                                                  FYP_Date = a.FYP_Date,
                                                  FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                                  FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                                  FYP_Bank_Name = a.FYP_Bank_Name,
                                                  amst_mobile = c.FMOST_StudentMobileNo,
                                                  //fathername = c.HRME_FatherName,
                                                  //studentdob = c.HRME_DOB
                                                  FMOST_StudentEmailId=c.FMOST_StudentEmailId,
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
        public FeeStaffOthersTransactionDTO select_emp(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                if(data.Grp_Term_flg=="T")
                {
                    data.termlist = (from a in _YearlyFeeGroupMappingContext.feeTr
                                     from b in _YearlyFeeGroupMappingContext.feeMTH
                                     from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                     where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && c.FSSST_ActiveFlag == true && c.HRME_Id == data.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id  && c.ASMAY_Id==data.ASMAY_Id)//&& c.FSSST_PaidAmount < c.FSSST_NetAmount
                                     select new FeeStaffOthersTransactionDTO
                                     {
                                         FMT_Id = a.FMT_Id,
                                         FMT_Name = a.FMT_Name,
                                         FMT_Order = a.FMT_Order,
                                         NetAmount = c.FSSST_NetAmount,
                                         PaidAmount = c.FSSST_PaidAmount
                                     }).Distinct().ToList().ToArray();

                   

                }
                else if (data.Grp_Term_flg == "G")
                {
                    data.grouplist = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                   //  from b in _YearlyFeeGroupMappingContext.feeMTH
                                     from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                     where ( c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true &&  c.FSSST_ActiveFlag == true && c.HRME_Id == data.HRME_Id && a.FMG_Id == c.FMG_Id  && c.ASMAY_Id==data.ASMAY_Id)//&& c.FSSST_PaidAmount < c.FSSST_NetAmount
                                      select new FeeStaffOthersTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName,
                                          NetAmount = c.FSSST_NetAmount,
                                          PaidAmount = c.FSSST_PaidAmount
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


                if(data.Stf_Others_flg== "Staff")
                {
                    var showstaticticsdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                 where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.HRME_Id == data.HRME_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_CurrentYrCharges = a.FSSST_CurrentYrCharges,
                                                     FSS_ToBePaid = a.FSSST_ToBePaid,
                                                     FSS_PaidAmount = a.FSSST_PaidAmount,
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
        public FeeStaffOthersTransactionDTO select_student(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                if (data.Grp_Term_flg == "T")
                {
                    data.termlist = (from a in _YearlyFeeGroupMappingContext.feeTr
                                     from b in _YearlyFeeGroupMappingContext.feeMTH
                                     from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                     where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && c.FSSOST_ActiveFlag == true && c.FMOST_Id == data.FMOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.ASMAY_Id == data.ASMAY_Id)//&& c.FSSOST_PaidAmount < c.FSSOST_NetAmount
                                     select new FeeStaffOthersTransactionDTO
                                     {
                                         FMT_Id = a.FMT_Id,
                                         FMT_Name = a.FMT_Name,
                                         FMT_Order = a.FMT_Order,
                                         NetAmount = c.FSSOST_NetAmount,
                                         PaidAmount = c.FSSOST_PaidAmount
                                     }).Distinct().ToList().ToArray();




                }
                else if (data.Grp_Term_flg == "G")
                {
                    data.grouplist = (from a in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                          //  from b in _YearlyFeeGroupMappingContext.feeMTH
                                      from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                      where (c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true && c.FSSOST_ActiveFlag == true && c.FMOST_Id == data.FMOST_Id && a.FMG_Id == c.FMG_Id  && c.ASMAY_Id==data.ASMAY_Id) //&& c.FSSOST_PaidAmount < c.FSSOST_NetAmount
                                      select new FeeStaffOthersTransactionDTO
                                      {
                                          FMG_Id = a.FMG_Id,
                                          FMG_GroupName = a.FMG_GroupName,
                                          NetAmount = c.FSSOST_NetAmount,
                                          PaidAmount = c.FSSOST_PaidAmount
                                      }).Distinct().ToList().ToArray();
                }
                data.oth_studentlist = _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO.Where(s => s.MI_Id == data.MI_Id && s.FMOST_ActiveFlag == true && s.FMOST_Id==data.FMOST_Id).Distinct().OrderBy(t => t.FMOST_Id).ToList().ToArray();


                 
                    var showstaticticsdetails = (from a in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                                 from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                                 where (a.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.FMOST_Id == data.FMOST_Id) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     FSS_CurrentYrCharges = a.FSSOST_CurrentYrCharges,
                                                     FSS_ToBePaid = a.FSSOST_ToBePaid,
                                                     FSS_PaidAmount = a.FSSOST_PaidAmount,
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
        public FeeStaffOthersTransactionDTO getgroupmappedheadsnew_st(FeeStaffOthersTransactionDTO data)
        {
            try
            {

                if (data.Stf_Others_flg == "Staff")
                {
                    if (data.Grp_Term_flg.Equals("G"))
                    {
                        //data.mapped_hds_ins = _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.HRME_Id == data.HRME_Id && data.terms_groups.Contains(s.FMG_Id) && s.FSSST_ActiveFlag == true).ToList().ToArray();
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO 
                                               from b in _YearlyFeeGroupMappingContext.feeMIY
                                               from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                               where (a.MI_Id==b.MI_ID && a.FMH_Id==c.FMH_Id && a.FMH_ActiveFlag==true &&  b.FTI_Id==c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FSSST_ActiveFlag == true && c.HRME_Id == data.HRME_Id  && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id==b.MI_ID && d.FMG_Id==c.FMG_Id && d.FMG_ActiceFlag==true)
                                               select new Head_Installments_DTO
                                               {
                                                   FMA_Id = c.FMA_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = b.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = c.FSSST_CurrentYrCharges,
                                                   TotalCharges = c.FSSST_TotalCharges,
                                                   ConcessionAmount = c.FSSST_ConcessionAmount,
                                                   FineAmount = c.FSSST_FineAmount,
                                                   ToBePaid = c.FSSST_ToBePaid,
                                                   NetAmount = c.FSSST_NetAmount,
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = d.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();
                    }
                    else if (data.Grp_Term_flg.Equals("T"))
                    {
                        //data.mapped_hds_ins = (from b in _YearlyFeeGroupMappingContext.feeMTH
                        //                       from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                        //                       where (c.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FSSST_ActiveFlag == true && c.HRME_Id == data.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && data.terms_groups.Contains(b.FMT_Id))
                        //                       select c).ToList().ToArray();
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                               from d in _YearlyFeeGroupMappingContext.feeMIY
                                               from b in _YearlyFeeGroupMappingContext.feeMTH
                                               from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                               from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                               where (a.MI_Id==d.MI_ID && a.MI_Id==c.MI_Id && a.FMH_Id==c.FMH_Id && a.FMH_ActiveFlag==true &&  d.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && c.ASMAY_Id==data.ASMAY_Id && c.FSSST_ActiveFlag == true && c.HRME_Id == data.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && data.terms_groups.Contains(b.FMT_Id) && e.MI_Id==b.MI_Id && e.FMG_Id==c.FMG_Id && e.FMG_ActiceFlag==true)
                                               select new Head_Installments_DTO
                                               {
                                                   FMA_Id = c.FMA_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = d.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = c.FSSST_CurrentYrCharges,
                                                   TotalCharges = c.FSSST_TotalCharges,
                                                   ConcessionAmount = c.FSSST_ConcessionAmount,
                                                   FineAmount = c.FSSST_FineAmount,
                                                   ToBePaid = c.FSSST_ToBePaid,
                                                   NetAmount = c.FSSST_NetAmount,
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = e.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();
                    }
                }
                else if (data.Stf_Others_flg == "Others")
                {
                    if (data.Grp_Term_flg.Equals("G"))
                    {
                        //  data.mapped_hds_ins = _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO.Where(s => s.MI_Id == data.MI_Id && s.ASMAY_Id == data.ASMAY_Id && s.FMOST_Id == data.FMOST_Id && data.terms_groups.Contains(s.FMG_Id) && s.FSSOST_ActiveFlag == true).ToList().ToArray();
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                               from b in _YearlyFeeGroupMappingContext.feeMIY
                                               from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                               from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                               where (a.MI_Id == b.MI_ID && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && b.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_ID && b.MI_ID == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FSSOST_ActiveFlag == true && c.FMOST_Id == data.FMOST_Id && data.terms_groups.Contains(c.FMG_Id) && d.MI_Id == b.MI_ID && d.FMG_Id == c.FMG_Id && d.FMG_ActiceFlag == true)
                                               select new Head_Installments_DTO
                                               {
                                                   FMA_Id = c.FMA_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = b.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = c.FSSOST_CurrentYrCharges,
                                                   TotalCharges = c.FSSOST_TotalCharges,
                                                   ConcessionAmount = c.FSSOST_ConcessionAmount,
                                                   FineAmount = c.FSSOST_FineAmount,
                                                   ToBePaid = c.FSSOST_ToBePaid,
                                                   NetAmount = c.FSSOST_NetAmount,
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = d.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();

                    }
                    else if (data.Grp_Term_flg.Equals("T"))
                    {

                        //data.mapped_hds_ins = (from b in _YearlyFeeGroupMappingContext.feeMTH
                        //                       from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                        //                       where (c.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FSSOST_ActiveFlag == true && c.FMOST_Id == data.FMOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && data.terms_groups.Contains(b.FMT_Id))
                        //                       select c).ToList().ToArray();
                        data.mapped_hds_ins = (from a in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                               from d in _YearlyFeeGroupMappingContext.feeMIY
                                               from b in _YearlyFeeGroupMappingContext.feeMTH
                                               from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                               from e in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                               where (a.MI_Id == d.MI_ID && a.MI_Id == c.MI_Id && a.FMH_Id == c.FMH_Id && a.FMH_ActiveFlag == true && d.FTI_Id == c.FTI_Id && c.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FSSOST_ActiveFlag == true && c.FMOST_Id == data.FMOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && data.terms_groups.Contains(b.FMT_Id) && e.MI_Id == b.MI_Id && e.FMG_Id == c.FMG_Id && e.FMG_ActiceFlag == true)
                                               select new Head_Installments_DTO
                                               {
                                                   FMA_Id = c.FMA_Id,
                                                   FMH_FeeName = a.FMH_FeeName,
                                                   FTI_Name = d.FTI_Name,
                                                   FMH_Id = c.FMH_Id,
                                                   FTI_Id = c.FTI_Id,
                                                   CurrentYrCharges = c.FSSOST_CurrentYrCharges,
                                                   TotalCharges = c.FSSOST_TotalCharges,
                                                   ConcessionAmount = c.FSSOST_ConcessionAmount,
                                                   FineAmount = c.FSSOST_FineAmount,
                                                   ToBePaid = c.FSSOST_ToBePaid,
                                                   NetAmount = c.FSSOST_NetAmount,
                                                   FMG_Id = c.FMG_Id,
                                                   FMG_GroupName = e.FMG_GroupName,
                                               }).Distinct().ToList().ToArray();

                    }
                }
                   
              
          //      data.alldata = (from a in _YearlyFeeGroupMappingContext.V_StudentPendingDMO
          //                      from b in _YearlyFeeGroupMappingContext.FeeGroupDMO
          //                      where (a.mi_id == data.MI_Id && a.fmg_id == b.FMG_Id)
          //                      select new FeeStaffOthersTransactionDTO
          //                      {
          //                          FMA_Id = a.fma_id,
          //                          FMH_FeeName = a.FMH_FeeName,
          //                          FTI_Name = a.FTI_Name,
          //                          FMH_Id = a.fmh_id,
          //                          FTI_Id = a.fti_id,
          //                          totalamount = a.FSS_NetAmount,
          //                          FSS_ToBePaid = a.FSS_ToBePaid,
          //                          FSS_ConcessionAmount = a.FSS_ConcessionAmount,
          //                          FSS_FineAmount = a.FSS_FineAmount,
          //                          FSS_CurrentYrCharges = a.FSS_CurrentYrCharges,
          //                          FSS_TotalToBePaid = a.FSS_TotalToBePaid,
          //                          FMG_Id = a.fmg_id,
          //                          FMG_GroupName = b.FMG_GroupName,
          //                      }

          //).OrderBy(t => t.FMH_Id).ToArray();

          //      if (data.alldata.Length > 0)
          //      {
          //          var count_res = _YearlyFeeGroupMappingContext.V_StudentPendingDMO.Where(r => r.mi_id == data.MI_Id).Select(r => r.fmg_id).GroupBy(id => id).OrderByDescending(id => id.Count()).Select(g => new FeeStaffOthersTransactionDTO { FMG_Id = g.Key, grp_count = g.Count() }).First();

          //          data.validationgroupid = count_res.FMG_Id;
          //          data.validationgrougidcount = count_res.grp_count;
          //      }


            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                data.validationvalue = "Contact Administrator";
            }
            return data;
        }

        public FeeStaffOthersTransactionDTO savedata_st(FeeStaffOthersTransactionDTO pgmod)
        {
            // FeeStudentGroupMappingDTO feestumap = new FeeStudentGroupMappingDTO();
            try
            {

                if (pgmod.FYP_Id > 0)
                {
                    var result = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Single(t => t.MI_Id == pgmod.MI_Id && t.FYP_Id == pgmod.FYP_Id && t.ASMAY_ID == pgmod.ASMAY_Id);

                    FeePaymentDetailsDMO pmm = new FeePaymentDetailsDMO();

                    if (pgmod.automanualreceiptno != "Auto" || pgmod.auto_receipt_flag != 1)
                    {
                        result.FYP_Receipt_No = pgmod.FYP_Receipt_No;
                    }

                    result.FYP_Date = Convert.ToDateTime(pgmod.FYP_Date);

                    if (pgmod.FYP_Remarks != "")
                    {
                        result.FYP_Remarks = pgmod.FYP_Remarks;
                    }

                    if (pgmod.FYP_Bank_Or_Cash != "C")
                    {
                        result.FYP_DD_Cheque_Date = pgmod.FYP_DD_Cheque_Date;
                        result.FYP_DD_Cheque_No = pgmod.FYP_DD_Cheque_No;
                        result.FYP_Bank_Name = pgmod.FYP_Bank_Name;
                    }

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
                    FeePaymentDetailsDMO obj_pay = Mapper.Map<FeePaymentDetailsDMO>(pgmod);
                    obj_pay.ASMAY_ID = pgmod.ASMAY_Id;
                    obj_pay.FTCU_Id = 1;
                    obj_pay.FYP_Tot_Waived_Amt = 0;
                    obj_pay.FYP_Chq_Bounce = "CL";
                    obj_pay.DOE = DateTime.Now;
                    obj_pay.CreatedDate = DateTime.Now;
                    obj_pay.UpdatedDate = DateTime.Now;
                    obj_pay.user_id = pgmod.userid;

                    get_grp_reptno(pgmod);

                    obj_pay.FYP_Receipt_No = pgmod.FYP_Receipt_No;

                    if (obj_pay.FYP_Receipt_No == "" || obj_pay.FYP_Receipt_No == null)
                    {
                        pgmod.returnval = "Record Not Saved because Receipt No is not Generated Automatic.Settings are missing";
                        return pgmod;
                    }
                    else
                    {
                        _YearlyFeeGroupMappingContext.Add(obj_pay);
                        if (pgmod.Stf_Others_flg == "Staff")
                        {
                            Fee_Y_Payment_StaffDMO obj_pay_stf = new Fee_Y_Payment_StaffDMO();
                            obj_pay_stf.FYP_Id = obj_pay.FYP_Id;
                            obj_pay_stf.HRME_Id = pgmod.HRME_Id;
                            obj_pay_stf.ASMAY_Id = pgmod.ASMAY_Id;
                            obj_pay_stf.FYPS_TotalPaidAmount = Convert.ToInt64(obj_pay.FYP_Tot_Amount);
                            obj_pay_stf.FYPS_TotalWaivedAmount = Convert.ToInt64(obj_pay.FYP_Tot_Waived_Amt);
                            obj_pay_stf.FYPS_TotalConcessionAmount = Convert.ToInt64(obj_pay.FYP_Tot_Concession_Amt);
                            obj_pay_stf.FYPS_TotalFineAmount = obj_pay.FYP_Tot_Fine_Amt;
                            obj_pay_stf.CreatedDate = DateTime.Now;
                            obj_pay_stf.UpdatedDate = DateTime.Now;
                            _YearlyFeeGroupMappingContext.Add(obj_pay_stf);

                            for (int l = 0; l < pgmod.head_installments.Length; l++)
                            {
                                Fee_T_Payment_OthStaffDMO obj_trans_st = new Fee_T_Payment_OthStaffDMO();
                                obj_trans_st.FYP_Id = obj_pay.FYP_Id;
                                obj_trans_st.FMAOST_Id = pgmod.head_installments[l].FMA_Id;
                                obj_trans_st.FTPOST_PaidAmount = pgmod.head_installments[l].ToBePaid;
                                obj_trans_st.FTPOST_WaivedAmount = 0;
                                obj_trans_st.FTPOST_ConcessionAmount = pgmod.head_installments[l].ConcessionAmount;
                                obj_trans_st.FTPOST_FineAmount = pgmod.head_installments[l].FineAmount;
                                obj_trans_st.FTPOST_RebateAmount = 0;
                                obj_trans_st.FTPOST_Remarks = obj_pay.FYP_Remarks;
                                obj_trans_st.CreatedDate = DateTime.Now;
                                obj_trans_st.UpdatedDate = DateTime.Now;
                                _YearlyFeeGroupMappingContext.Add(obj_trans_st);

                                var obj_status_stf = _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO.Single(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.HRME_Id == pgmod.HRME_Id && t.FMH_Id == pgmod.head_installments[l].FMH_Id && t.FTI_Id == pgmod.head_installments[l].FTI_Id && t.FMG_Id == pgmod.head_installments[l].FMG_Id && t.FMA_Id == pgmod.head_installments[l].FMA_Id && t.FSSST_ActiveFlag == true);

                                obj_status_stf.FSSST_PaidAmount = pgmod.head_installments[l].ToBePaid;

                                if (obj_status_stf.FSSST_NetAmount != 0)
                                {
                                    obj_status_stf.FSSST_ToBePaid = obj_status_stf.FSSST_ToBePaid - obj_status_stf.FSSST_PaidAmount;
                                }
                                else
                                {
                                    obj_status_stf.FSSST_ToBePaid = 0;
                                }

                                //obj_status_stf.FSSST_ToBePaid = obj_status_stf.FSSST_ToBePaid - obj_status_stf.FSSST_PaidAmount;

                                obj_status_stf.UpdatedDate = DateTime.Now;
                                _YearlyFeeGroupMappingContext.Update(obj_status_stf);
                            }

                        }
                        else if (pgmod.Stf_Others_flg == "Others")
                        {
                            Fee_Y_Payment_OthStuDMO obj_pay_stf = new Fee_Y_Payment_OthStuDMO();
                            obj_pay_stf.FYP_Id = obj_pay.FYP_Id;
                            obj_pay_stf.FMOST_Id = pgmod.FMOST_Id;
                            obj_pay_stf.ASMAY_Id = pgmod.ASMAY_Id;
                            obj_pay_stf.FYPOST_TotalPaidAmount = Convert.ToInt64(obj_pay.FYP_Tot_Amount);
                            obj_pay_stf.FYPOST_TotalWaivedAmount = Convert.ToInt64(obj_pay.FYP_Tot_Waived_Amt);
                            obj_pay_stf.FYPOST_TotalConcessionAmount = Convert.ToInt64(obj_pay.FYP_Tot_Concession_Amt);
                            obj_pay_stf.FYPOST_TotalFineAmount = obj_pay.FYP_Tot_Fine_Amt;
                            obj_pay_stf.CreatedDate = DateTime.Now;
                            obj_pay_stf.UpdatedDate = DateTime.Now;
                            _YearlyFeeGroupMappingContext.Add(obj_pay_stf);
                            for (int l = 0; l < pgmod.head_installments.Length; l++)
                            {
                                Fee_T_Payment_OthStaffDMO obj_trans_st = new Fee_T_Payment_OthStaffDMO();
                                obj_trans_st.FYP_Id = obj_pay.FYP_Id;
                                obj_trans_st.FMAOST_Id = pgmod.head_installments[l].FMA_Id;
                                obj_trans_st.FTPOST_PaidAmount = pgmod.head_installments[l].ToBePaid;
                                obj_trans_st.FTPOST_WaivedAmount = 0;
                                obj_trans_st.FTPOST_ConcessionAmount = pgmod.head_installments[l].ConcessionAmount;
                                obj_trans_st.FTPOST_FineAmount = pgmod.head_installments[l].FineAmount;
                                obj_trans_st.FTPOST_RebateAmount = 0;
                                obj_trans_st.FTPOST_Remarks = obj_pay.FYP_Remarks;
                                obj_trans_st.CreatedDate = DateTime.Now;
                                obj_trans_st.UpdatedDate = DateTime.Now;
                                _YearlyFeeGroupMappingContext.Add(obj_trans_st);

                                var obj_status_otrs = _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO.Single(t => t.MI_Id == pgmod.MI_Id && t.ASMAY_Id == pgmod.ASMAY_Id && t.FMOST_Id == pgmod.FMOST_Id && t.FMH_Id == pgmod.head_installments[l].FMH_Id && t.FTI_Id == pgmod.head_installments[l].FTI_Id && t.FMG_Id == pgmod.head_installments[l].FMG_Id && t.FMA_Id == pgmod.head_installments[l].FMA_Id && t.FSSOST_ActiveFlag == true);

                                obj_status_otrs.FSSOST_PaidAmount = pgmod.head_installments[l].ToBePaid;

                                if (obj_status_otrs.FSSOST_NetAmount != 0)
                                {
                                    obj_status_otrs.FSSOST_ToBePaid = obj_status_otrs.FSSOST_ToBePaid - obj_status_otrs.FSSOST_PaidAmount;
                                }
                                else
                                {
                                    obj_status_otrs.FSSOST_ToBePaid = 0;
                                }


                                //obj_status_otrs.FSSOST_ToBePaid = obj_status_otrs.FSSOST_ToBePaid - obj_status_otrs.FSSOST_PaidAmount;

                                obj_status_otrs.UpdatedDate = DateTime.Now;
                                _YearlyFeeGroupMappingContext.Update(obj_status_otrs);

                            }

                        }
                        var ResultCount = _YearlyFeeGroupMappingContext.SaveChanges();
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

        public FeeStaffOthersTransactionDTO searching_s(FeeStaffOthersTransactionDTO data)
        {
            try
            {

                switch (data.searchType)
                {

                    case "0":

                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where ( d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.user_id == data.userid &&  (((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim().Contains(data.searchtext) || ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName)).Trim().Contains(data.searchtext) || ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim().Contains(data.searchtext) || d.HRME_EmployeeFirstName.StartsWith(data.searchtext) || d.HRME_EmployeeMiddleName.StartsWith(data.searchtext) || d.HRME_EmployeeLastName.StartsWith(data.searchtext)))
                                                   select new FeeStaffOthersTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_Receipt_No,
                                                       FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                       FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                       FYP_Date = a.FYP_Date
                                                   }).Distinct().OrderBy(t => t.HRME_EmployeeFirstName).ToArray();
                        break;
                    case "1":
                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where ( d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.user_id == data.userid && d.HRME_EmployeeCode.Contains(data.searchtext))
                                                   select new FeeStaffOthersTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_Receipt_No,
                                                       FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                       FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                       FYP_Date = a.FYP_Date
                                                   }).Distinct().OrderBy(t => t.HRME_EmployeeCode).ToArray();
                        break;
                    case "7":
                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where ( d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.user_id == data.userid && e.HRMD_DepartmentName.Contains(data.searchtext))
                                                   select new FeeStaffOthersTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_Receipt_No,
                                                       FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                       FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                       FYP_Date = a.FYP_Date
                                                   }).Distinct().OrderBy(t => t.HRMD_DepartmentName).ToArray();
                        break;
                    case "2":
                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where ( d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.user_id == data.userid && f.HRMDES_DesignationName.Contains(data.searchtext))
                                                   select new FeeStaffOthersTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_Receipt_No,
                                                       FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                       FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                       FYP_Date = a.FYP_Date
                                                   }).Distinct().OrderBy(t => t.HRMDES_DesignationName).ToArray();
                        break;
                    case "3":
                        
                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where ( d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.user_id == data.userid && a.FYP_Receipt_No.Contains(data.searchtext))
                                                   select new FeeStaffOthersTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_Receipt_No,
                                                       FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                       FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                       FYP_Date = a.FYP_Date
                                                   }).Distinct().OrderBy(t => t.FYP_Receipt_No).ToArray();
                        break;
                    case "4":
                        var date_format = data.searchdate.ToString("dd/MM/yyyy");
                        
                         data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                    from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                    from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                    from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                    where ( d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id &&  d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.user_id == data.userid && a.FYP_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy"))
                                                    select new FeeStaffOthersTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        HRME_Id = d.HRME_Id,
                                                        HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                        HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                        HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                        HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        FYP_Date = a.FYP_Date
                                                    }).Distinct().OrderBy(t => t.FYP_Date).ToArray();
                        break;
                    case "5":
                       
                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where ( d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.user_id == data.userid && a.FYP_Tot_Amount.ToString().Contains(data.searchnumber))
                                                   select new FeeStaffOthersTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_Receipt_No,
                                                       FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                       FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                       FYP_Date = a.FYP_Date
                                                   }).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();
                        break;
                    case "6":
                        
                        data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                   from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                                   from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                                   from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                                   where ( d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.user_id == data.userid && a.FYP_Bank_Or_Cash.Contains(data.searchtext))
                                                   select new FeeStaffOthersTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       HRME_Id = d.HRME_Id,
                                                       HRME_EmployeeCode = d.HRME_EmployeeCode,
                                                       HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                                       HRMD_DepartmentName = e.HRMD_DepartmentName,
                                                       HRMDES_DesignationName = f.HRMDES_DesignationName,
                                                       FYP_Receipt_No = a.FYP_Receipt_No,
                                                       FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                       FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                       FYP_Date = a.FYP_Date
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

        public FeeStaffOthersTransactionDTO searching_o(FeeStaffOthersTransactionDTO data)
        {
            try
            {

                switch (data.searchType)
                {

                    case "0":

                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where ( d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.user_id == data.userid && d.FMOST_StudentName.Contains(data.searchtext))
                                                    select new FeeStaffOthersTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        FYP_Date = a.FYP_Date
                                                    }).Distinct().OrderBy(t => t.FMOST_StudentName).ToArray();
                        break;
                    case "1":
                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where ( d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.user_id == data.userid && d.FMOST_StudentMobileNo.ToString().Contains(data.searchtext))
                                                    select new FeeStaffOthersTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        FYP_Date = a.FYP_Date
                                                    }).Distinct().OrderBy(t => t.FMOST_StudentMobileNo).ToArray();
                        break;                  
                    case "2":
                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where ( d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.user_id == data.userid && d.FMOST_StudentEmailId.Contains(data.searchtext))
                                                    select new FeeStaffOthersTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        FYP_Date = a.FYP_Date
                                                    }).Distinct().OrderBy(t => t.FMOST_StudentEmailId).ToArray();
                        break;
                    case "3":                        
                        data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                    from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                                    from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                    from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                    where ( d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.user_id == data.userid && a.FYP_Receipt_No.Contains(data.searchtext))
                                                    select new FeeStaffOthersTransactionDTO
                                                    {
                                                        FYP_Id = a.FYP_Id,
                                                        FMOST_Id = d.FMOST_Id,
                                                        FMOST_StudentName = d.FMOST_StudentName,
                                                        FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                        FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                        FYP_Receipt_No = a.FYP_Receipt_No,
                                                        FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                        FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                        FYP_Date = a.FYP_Date
                                                    }).Distinct().OrderBy(t => t.FYP_Receipt_No).ToArray();
                        break;
                    case "4":
                        var date_format = data.searchdate.ToString("dd/MM/yyyy");                        
                         data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                                     from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                     from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                     where ( d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.user_id == data.userid && a.FYP_Date.ToString("dd/MM/yyyy") == data.searchdate.ToString("dd/MM/yyyy"))
                                                     select new FeeStaffOthersTransactionDTO
                                                     {
                                                         FYP_Id = a.FYP_Id,
                                                         FMOST_Id = d.FMOST_Id,
                                                         FMOST_StudentName = d.FMOST_StudentName,
                                                         FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                         FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                         FYP_Receipt_No = a.FYP_Receipt_No,
                                                         FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                         FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                         FYP_Date = a.FYP_Date
                                                     }).Distinct().OrderBy(t => t.FYP_Date).ToArray();
                        break;
                    case "5":                        
                         data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                                     from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                     from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                     where ( d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.user_id == data.userid && a.FYP_Tot_Amount.ToString().Contains(data.searchnumber))
                                                     select new FeeStaffOthersTransactionDTO
                                                     {
                                                         FYP_Id = a.FYP_Id,
                                                         FMOST_Id = d.FMOST_Id,
                                                         FMOST_StudentName = d.FMOST_StudentName,
                                                         FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                         FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                         FYP_Receipt_No = a.FYP_Receipt_No,
                                                         FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                         FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                         FYP_Date = a.FYP_Date
                                                     }).Distinct().OrderBy(t => t.FYP_Tot_Amount).ToArray();
                        break;
                    case "6":
                       data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                                   from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                                   where ( d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMOST_Id == d.FMOST_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.FMOST_ActiveFlag == true && a.user_id == data.userid && a.FYP_Bank_Or_Cash.Contains(data.searchtext))
                                                   select new FeeStaffOthersTransactionDTO
                                                   {
                                                       FYP_Id = a.FYP_Id,
                                                       FMOST_Id = d.FMOST_Id,
                                                       FMOST_StudentName = d.FMOST_StudentName,
                                                       FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                       FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                       FYP_Receipt_No = a.FYP_Receipt_No,
                                                       FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                       FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                       FYP_Date = a.FYP_Date
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
        public FeeStaffOthersTransactionDTO printreceipt_s(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                data.receiptdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                       from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                       from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                       from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                       from e in _YearlyFeeGroupMappingContext.MasterEmployee
                                       from f in _YearlyFeeGroupMappingContext.HR_Master_Department
                                       from g in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                       from h in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                       from i in _YearlyFeeGroupMappingContext.feeMIY
                                       from j in _YearlyFeeGroupMappingContext.AcademicYear
                                       where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && g.MI_Id == a.MI_Id && h.MI_Id == a.MI_Id && i.MI_ID == a.MI_Id && j.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == e.HRME_Id && f.HRMD_Id == e.HRMD_Id && g.HRMDES_Id == e.HRMDES_Id && c.FMAOST_Id == d.FMAOST_Id && d.FMH_Id == h.FMH_Id && d.FTI_Id == i.FTI_Id && a.ASMAY_ID == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && j.ASMAY_Id == a.ASMAY_ID && d.FMAOST_OthStaffFlag=="S")
                                       select new FeeStaffOthersTransactionDTO
                                       {
                                           FYP_Id = a.FYP_Id,
                                           FYP_Receipt_No = a.FYP_Receipt_No,
                                           FYP_Tot_Amount = a.FYP_Tot_Amount,
                                           FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                           FYP_Bank_Name = a.FYP_Bank_Name,
                                           FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                           FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                           FYP_Remarks = a.FYP_Remarks,
                                           FYP_Date = a.FYP_Date,
                                           ToBePaid = Convert.ToInt64(d.FMAOST_Amount),
                                           FTPOST_PaidAmount = c.FTPOST_PaidAmount,
                                           FTPOST_ConcessionAmount = c.FTPOST_ConcessionAmount,
                                           HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                           HRME_EmployeeCode = e.HRME_EmployeeCode,
                                           HRMD_DepartmentName = f.HRMD_DepartmentName,
                                           HRMDES_DesignationName = g.HRMDES_DesignationName,
                                           FMH_Id = d.FMH_Id,
                                           FTI_Id = d.FTI_Id,
                                           ASMAY_Id = a.ASMAY_ID,
                                           HRME_Id = e.HRME_Id,
                                           FMH_FeeName = h.FMH_FeeName,
                                           FTI_Name = i.FTI_Name,
                                           ASMAY_Year = j.ASMAY_Year
                                       }).Distinct().ToArray();

                //List<FeeStaffOthersTransactionDTO> temp_group_head = new List<FeeStaffOthersTransactionDTO>();
                //temp_group_head = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                //                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                //                   from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                //                   from e in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                //                   where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id==a.MI_Id &&  a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && c.FMAOST_Id == d.FMAOST_Id   && a.ASMAY_ID == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && e.HRME_Id==b.HRME_Id && e.FMA_Id==c.FMAOST_Id && e.FMG_Id==d.FMG_Id && e.FMH_Id==d.FMH_Id && e.FTI_Id==d.FTI_Id)
                //                   //a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id
                //                   select new FeeStaffOthersTransactionDTO
                //                   {

                //                       FMG_Id = d.FMG_Id,
                //                       FMH_Id = d.FMH_Id,
                //                       FTI_Id = d.FTI_Id

                //                   }

                //        ).Distinct().ToList();
                //List<long> grp_ids = new List<long>();
                //List<long> head_ids = new List<long>();
                //List<long> inst_ids = new List<long>();
                //foreach (var item in temp_group_head)
                //{
                //    grp_ids.Add(item.FMG_Id);
                //    head_ids.Add(item.FMH_Id);
                //    inst_ids.Add(item.FTI_Id);
                //}
                
                var feeterm = (from a in _YearlyFeeGroupMappingContext.feeTr
                                 from b in _YearlyFeeGroupMappingContext.feeMTH
                                 from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                 from d in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                 from e in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                 from f in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                 from g in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                 where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && c.FSSST_ActiveFlag == true && c.HRME_Id == d.HRME_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.ASMAY_Id == d.ASMAY_Id && e.MI_Id==a.MI_Id && d.ASMAY_Id==e.ASMAY_ID && d.FYP_Id==data.FYP_Id && e.FYP_Id==d.FYP_Id && f.FYP_Id==d.FYP_Id && g.FMAOST_Id==f.FMAOST_Id && g.FTI_Id==b.FTI_Id && g.FMAOST_OthStaffFlag == "S")//&& c.FSSST_PaidAmount < c.FSSST_NetAmount
                                 select new FeeStaffOthersTransactionDTO
                                 {
                                     FMT_Id = a.FMT_Id,
                                     FMT_Order=a.FMT_Order,
                                     FromMonth=a.FromMonth,
                                     ToMonth=a.ToMonth
                                     //FMT_Name = a.FMT_Name,
                                     //NetAmount = c.FSSST_NetAmount,
                                     //PaidAmount = c.FSSST_PaidAmount
                                 }).Distinct().OrderByDescending(t => t.FMT_Order).ToArray();
                
                //var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //               from b in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                //               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                //               from d in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                //               from e in _YearlyFeeGroupMappingContext.feeMTH
                //               where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMAOST_Id == d.FMA_Id && d.HRME_Id == c.HRME_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id &&  a.FYP_Id == data.FYP_Id)
                //               select new FeeStaffOthersTransactionDTO
                //               {
                //                   //fmt_id = e.FMT_Id
                //                   FMT_Id = e.FMT_Id
                //               }

         // ).Distinct().OrderByDescending(t => t.FMT_Id).ToArray();
                long fmt_id_new = 0;
              
               // long initialfmtids = Convert.ToInt64(feeterm[0].FMT_Id);

                fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;
                var fmt_by_order = feeterm[0].FMT_Order + 1;
                //var fmt_id_byord = _YearlyFeeGroupMappingContext.feeTr.Single(t => t.MI_Id == data.MI_Id && t.FMT_Order == fmt_by_order).FMT_Id;
                //if(fmt_id_new!=fmt_id_byord)
                //{
                //    fmt_id_new = fmt_id_byord;
                //}
                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order <= fmt_by_order).Select(t => t.FMT_Id);

                //     data.dueamount = (from a in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                //                       from b in _YearlyFeeGroupMappingContext.feeMTH
                //                       from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                       where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag != "E")
                //                       select new FeeStaffOthersTransactionDTO
                //                       {
                //                           ToBePaid = a.FSSST_ToBePaid
                //                       }
                //).Sum(t => t.ToBePaid);

                //data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                   from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                //                   from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                //                   from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                //                   from e in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                //                   from f in _YearlyFeeGroupMappingContext.feeMTH
                //                   where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && c.FMAOST_Id == d.FMAOST_Id && a.ASMAY_ID == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && e.HRME_Id == b.HRME_Id && e.FMA_Id == c.FMAOST_Id && e.FMG_Id == d.FMG_Id && e.FMH_Id == d.FMH_Id && e.FTI_Id == d.FTI_Id  &&  f.FMH_Id==e.FMH_Id && f.FTI_Id==e.FTI_Id && term_ids.Contains(f.FMT_Id))
                //                  //a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id
                //                  select e.FSSST_ToBePaid).Sum();

                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                  from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                 // from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                //  from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                  from e in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                  from f in _YearlyFeeGroupMappingContext.feeMTH
                                  where (a.MI_Id == data.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id &&  a.ASMAY_ID == b.ASMAY_Id &&  e.HRME_Id == b.HRME_Id &&  f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id))
                                  select e.FSSST_ToBePaid).Sum();
                var monthnameinitial = feeterm[(feeterm.Length - 1)].FromMonth.ToString();
              var  monthnameend = feeterm[0].ToMonth.ToString();

                data.duration = monthnameinitial + '-' + monthnameend;// +'-' + data.year

               var duedates_list = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                  from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                 // from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                  from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                  from e in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                  from f in _YearlyFeeGroupMappingContext.feeMTH
                                  from g in _YearlyFeeGroupMappingContext.Fee_T_Due_Date_OthStaffs
                                  where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && a.ASMAY_ID == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && e.HRME_Id == b.HRME_Id  && e.FMG_Id == d.FMG_Id && e.FMH_Id == d.FMH_Id && e.FTI_Id == d.FTI_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id) && d.FMAOST_Id==g.FMAOST_Id && d.FMAOST_OthStaffFlag == "S")// && f.FMT_Id== fmt_id_new  && c.FYP_Id == a.FYP_Id && c.FMAOST_Id == d.FMAOST_Id && e.FMA_Id == c.FMAOST_Id
                                    //a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id
                                    select g).Distinct().ToList();
               
                var year = duedates_list.Max(t => t.FTDD_Year);
                var month = duedates_list.Where(t => t.FTDD_Year == year).Select(t=>Convert.ToInt32(t.FTDD_Month)).Max();
                var date = duedates_list.Where(t => Convert.ToInt32(t.FTDD_Month) == month && t.FTDD_Year == year).Select(t => Convert.ToInt32(t.FTDD_Day)).Max();

                data.Due_Date = date + "/" + month + "/" + year;
                //List<int> d_months = new List<int>();
                //List<int> d_dates = new List<int>();
                //List<int> d_years = new List<int>();
                //foreach (var due_dates in duedates_list)
                //{
                //    d_months.Add(Convert.ToInt32(due_dates.FTDD_Month));
                //    d_dates.Add(Convert.ToInt32(due_dates.FTDD_Day));
                //    d_years.Add(due_dates.FTDD_Year);
                //}


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public FeeStaffOthersTransactionDTO printreceipt_o(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                data.receiptdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                       from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                       from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                       from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                       from e in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                       //from f in _YearlyFeeGroupMappingContext.HR_Master_Department
                                       //from g in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                       from h in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                       from i in _YearlyFeeGroupMappingContext.feeMIY
                                       from j in _YearlyFeeGroupMappingContext.AcademicYear
                                       where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id &&  h.MI_Id == a.MI_Id && i.MI_ID == a.MI_Id && j.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMOST_Id == e.FMOST_Id &&   c.FMAOST_Id == d.FMAOST_Id && d.FMH_Id == h.FMH_Id && d.FTI_Id == i.FTI_Id && a.ASMAY_ID == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && j.ASMAY_Id == a.ASMAY_ID && d.FMAOST_OthStaffFlag == "O")
                                       select new FeeStaffOthersTransactionDTO
                                       {
                                           FYP_Id = a.FYP_Id,
                                           FYP_Receipt_No = a.FYP_Receipt_No,
                                           FYP_Bank_Or_Cash=a.FYP_Bank_Or_Cash,
                                           FYP_Tot_Amount = a.FYP_Tot_Amount,
                                           FYP_Bank_Name = a.FYP_Bank_Name,
                                           FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                           FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                           FYP_Remarks = a.FYP_Remarks,
                                           FYP_Date = a.FYP_Date,
                                           ToBePaid = Convert.ToInt64(d.FMAOST_Amount),
                                           FTPOST_PaidAmount = c.FTPOST_PaidAmount,
                                           FTPOST_ConcessionAmount = c.FTPOST_ConcessionAmount,
                                           FMOST_Id = e.FMOST_Id,
                                           FMOST_StudentName = e.FMOST_StudentName,
                                           FMOST_StudentMobileNo = e.FMOST_StudentMobileNo,
                                           FMOST_StudentEmailId = e.FMOST_StudentEmailId,
                                           FMH_Id = d.FMH_Id,
                                           FTI_Id = d.FTI_Id,
                                           ASMAY_Id = a.ASMAY_ID,
                                           FMH_FeeName = h.FMH_FeeName,
                                           FTI_Name = i.FTI_Name,
                                           ASMAY_Year = j.ASMAY_Year
                                       }).Distinct().ToArray();

                //var feeterm = (from a in _YearlyFeeGroupMappingContext.feeTr
                //               from b in _YearlyFeeGroupMappingContext.feeMTH
                //               from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                //               from d in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                //               from e in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //               from f in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                //               where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && c.FSSOST_ActiveFlag == true && c.FMOST_Id == d.FMOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.ASMAY_Id == d.ASMAY_Id && e.MI_Id == a.MI_Id && d.ASMAY_Id == e.ASMAY_ID && d.FYP_Id == data.FYP_Id && e.FYP_Id == d.FYP_Id && f.FYP_Id == d.FYP_Id)//&& c.FSSST_PaidAmount < c.FSSST_NetAmount
                //               select new FeeStaffOthersTransactionDTO
                //               {
                //                   FMT_Id = a.FMT_Id,
                //                   FMT_Order = a.FMT_Order,
                //                   FromMonth=a.FromMonth,
                //                   ToMonth=a.ToMonth                              
                //               }).Distinct().OrderByDescending(t => t.FMT_Id).ToArray();
                var feeterm = (from a in _YearlyFeeGroupMappingContext.feeTr
                               from b in _YearlyFeeGroupMappingContext.feeMTH
                               from c in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                               from d in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                               from e in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from f in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                               from g in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                               where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMT_ActiveFlag == true && a.FMT_Id == b.FMT_Id && c.FSSOST_ActiveFlag == true && c.FMOST_Id == d.FMOST_Id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.FTI_Id && c.ASMAY_Id == d.ASMAY_Id && e.MI_Id == a.MI_Id && d.ASMAY_Id == e.ASMAY_ID && d.FYP_Id == data.FYP_Id && e.FYP_Id == d.FYP_Id && f.FYP_Id == d.FYP_Id && g.FMAOST_Id == f.FMAOST_Id && g.FTI_Id == b.FTI_Id && g.FMAOST_OthStaffFlag == "O")
                               select new FeeStaffOthersTransactionDTO
                               {
                                   FMT_Id = a.FMT_Id,
                                   FMT_Order = a.FMT_Order,
                                   FromMonth = a.FromMonth,
                                   ToMonth = a.ToMonth
                               }).Distinct().OrderByDescending(t => t.FMT_Order).ToArray();

                long fmt_id_new = 0;
             
                fmt_id_new = Convert.ToInt64(feeterm[0].FMT_Id) + 1;
                var fmt_by_order = feeterm[0].FMT_Order + 1;
                //var fmt_id_byord = _YearlyFeeGroupMappingContext.feeTr.Single(t => t.MI_Id == data.MI_Id && t.FMT_Order == fmt_by_order).FMT_Id;
                //if (fmt_id_new != fmt_id_byord)
                //{
                //    fmt_id_new = fmt_id_byord;
                //}

                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order <= fmt_by_order).Select(t => t.FMT_Id);

                //data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                  from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                //                  from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                //                  from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                //                  from e in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                //                  from f in _YearlyFeeGroupMappingContext.feeMTH
                //                  where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && c.FMAOST_Id == d.FMAOST_Id && a.ASMAY_ID == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && e.FMOST_Id == b.FMOST_Id && e.FMA_Id == c.FMAOST_Id && e.FMG_Id == d.FMG_Id && e.FMH_Id == d.FMH_Id && e.FTI_Id == d.FTI_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id))                              
                //                  select e.FSSOST_ToBePaid).Sum();
                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                  from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                  from e in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                  from f in _YearlyFeeGroupMappingContext.feeMTH
                                  where (a.MI_Id == data.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && a.ASMAY_ID == b.ASMAY_Id && e.FMOST_Id == b.FMOST_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id))
                                  select e.FSSOST_ToBePaid).Sum();
                var monthnameinitial = feeterm[(feeterm.Length - 1)].FromMonth.ToString();
                var monthnameend = feeterm[0].ToMonth.ToString();

                data.duration = monthnameinitial + '-' + monthnameend;

                //var duedates_list = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                //                     from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                //                     from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                //                     from e in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                //                     from f in _YearlyFeeGroupMappingContext.feeMTH
                //                     from g in _YearlyFeeGroupMappingContext.Fee_T_Due_Date_OthStaffs
                //                     where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && c.FYP_Id == a.FYP_Id && c.FMAOST_Id == d.FMAOST_Id && a.ASMAY_ID == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && e.FMOST_Id == b.FMOST_Id && e.FMA_Id == c.FMAOST_Id && e.FMG_Id == d.FMG_Id && e.FMH_Id == d.FMH_Id && e.FTI_Id == d.FTI_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id) && d.FMAOST_Id == g.FMAOST_Id)// && f.FMT_Id== fmt_id_new                                        
                //                     select g).Distinct().ToList();
                //var year = duedates_list.Max(t => t.FTDD_Year);
                //var month = duedates_list.Where(t => t.FTDD_Year == year).Max(t => t.FTDD_Month);
                //var date = duedates_list.Where(t => t.FTDD_Month == month && t.FTDD_Year == year).Max(t => t.FTDD_Day);

                var duedates_list = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                     from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                     from d in _YearlyFeeGroupMappingContext.Fee_Master_Amount_OthStaffs
                                     from e in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                     from f in _YearlyFeeGroupMappingContext.feeMTH
                                     from g in _YearlyFeeGroupMappingContext.Fee_T_Due_Date_OthStaffs
                                     where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && a.FYP_Id == data.FYP_Id && b.FYP_Id == a.FYP_Id && a.ASMAY_ID == b.ASMAY_Id && d.ASMAY_Id == a.ASMAY_ID && e.FMOST_Id == b.FMOST_Id && e.FMG_Id == d.FMG_Id && e.FMH_Id == d.FMH_Id && e.FTI_Id == d.FTI_Id && f.FMH_Id == e.FMH_Id && f.FTI_Id == e.FTI_Id && term_ids.Contains(f.FMT_Id) && d.FMAOST_Id == g.FMAOST_Id && d.FMAOST_OthStaffFlag == "O")
                                     select g).Distinct().ToList();

                var year = duedates_list.Max(t => t.FTDD_Year);
                var month = duedates_list.Where(t => t.FTDD_Year == year).Select(t => Convert.ToInt32(t.FTDD_Month)).Max();
                var date = duedates_list.Where(t => Convert.ToInt32(t.FTDD_Month) == month && t.FTDD_Year == year).Select(t => Convert.ToInt32(t.FTDD_Day)).Max();

                data.Due_Date = date + "/" + month + "/" + year;
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
         
            return data;
        }
        public FeeStaffOthersTransactionDTO deletereceipt_s(FeeStaffOthersTransactionDTO data)
        {
            try
            {
              
               var lorg1 = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Single(t => t.FYP_Id == data.FYP_Id);
             
               var lorg2 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg3 = _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                var lorg34 = _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO.Where(t => t.FYP_Id == data.FYP_Id).Select(t => t.FMAOST_Id).ToList();

                var delcandel = (from a in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                 from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                 where (a.FMA_Id == b.FMAOST_Id && c.HRME_Id == a.HRME_Id && c.ASMAY_Id == a.ASMAY_Id && c.FYP_Id == b.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && lorg34.Contains(a.FMA_Id) && a.HRME_Id == lorg2.HRME_Id && (a.FSSST_WaivedAmount > 0 || a.FSSST_AdjustedAmount > 0 || a.FSSST_RunningExcessAmount > 0))
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMH_Id = a.FMH_Id
                                 }
                      ).ToArray();

                if(delcandel.Length==0)
                {
                    foreach (var x in lorg3)
                    {
                        var FMA_Id = x.FMAOST_Id;
                        var FSSST_PaidAmount = x.FTPOST_PaidAmount;
                        var lorg4 = _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO.Single(t => t.FMA_Id == FMA_Id && t.MI_Id == lorg1.MI_Id && t.ASMAY_Id == lorg1.ASMAY_ID && t.HRME_Id == lorg2.HRME_Id);

                        lorg4.FSSST_PaidAmount = lorg4.FSSST_PaidAmount - x.FTPOST_PaidAmount;


                        if (lorg4.FSSST_ExcessPaidAmount > 0)
                        {
                            lorg4.FSSST_ExcessPaidAmount = (lorg4.FSSST_ExcessPaidAmount) - Convert.ToInt64(x.FTPOST_PaidAmount);
                        }
                        if (lorg4.FSSST_RunningExcessAmount > 0)
                        {
                            lorg4.FSSST_RunningExcessAmount = (lorg4.FSSST_RunningExcessAmount) - Convert.ToInt64(x.FTPOST_PaidAmount);
                        }

                        //added on 11-07-2018
                        var fineheadss = (from a in _YearlyFeeGroupMappingContext.Fee_Student_Status_StaffDMO
                                          from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                          where (a.MI_Id == data.MI_Id && a.FMA_Id == FMA_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.HRME_Id == lorg2.HRME_Id && a.FMH_Id == b.FMH_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMH_Id = a.FMH_Id,
                                          }
                           ).Distinct().Take(1);

                        if (fineheadss.Count() > 0)
                        {
                            lorg4.FSSST_FineAmount = lorg4.FSSST_FineAmount - Convert.ToInt64(x.FTPOST_PaidAmount);
                        }
                        //added on 11-07-2018


                        if (lorg4.FSSST_NetAmount != 0)
                        {
                            lorg4.FSSST_ToBePaid = lorg4.FSSST_ToBePaid + x.FTPOST_PaidAmount;
                        }
                        else
                        {
                            lorg4.FSSST_ToBePaid = 0;
                        }

                        lorg4.UpdatedDate = DateTime.Now;

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

        public FeeStaffOthersTransactionDTO deletereceipt_o(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                var lorg1 = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg2 = _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO.Single(t => t.FYP_Id == data.FYP_Id);

                var lorg3 = _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();

                //List<long> FMA_Ids = new List<long>();
                //foreach (var x in lorg3)
                //{
                //    FMA_Ids.Add(x.FMAOST_Id);
                //}
                foreach (var x in lorg3)
                {
                    var FMA_Id = x.FMAOST_Id;
                    var FSSST_PaidAmount = x.FTPOST_PaidAmount;
                    var lorg4 = _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO.Single(t => t.FMA_Id == FMA_Id && t.MI_Id == lorg1.MI_Id && t.ASMAY_Id == lorg1.ASMAY_ID && t.FMOST_Id == lorg2.FMOST_Id);
                    lorg4.FSSOST_PaidAmount = lorg4.FSSOST_PaidAmount - x.FTPOST_PaidAmount;

                    if (lorg4.FSSOST_ExcessPaidAmount > 0)
                    {
                        lorg4.FSSOST_ExcessPaidAmount = (lorg4.FSSOST_ExcessPaidAmount) - Convert.ToInt64(x.FTPOST_PaidAmount);
                    }
                    if (lorg4.FSSOST_RunningExcessAmount > 0)
                    {
                        lorg4.FSSOST_RunningExcessAmount = (lorg4.FSSOST_RunningExcessAmount) - Convert.ToInt64(x.FTPOST_PaidAmount);
                    }

                    //added on 11-07-2018
                    var fineheadss = (from a in _YearlyFeeGroupMappingContext.Fee_Student_Status_OthStuDMO
                                      from b in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                      where (a.MI_Id == data.MI_Id && a.FMA_Id == FMA_Id && a.ASMAY_Id == data.ASMAY_Id && b.FMH_Flag == "F" && a.FMOST_Id == lorg2.FMOST_Id && a.FMH_Id == b.FMH_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FMH_Id = a.FMH_Id,
                                      }
                       ).Distinct().Take(1);

                    if (fineheadss.Count() > 0)
                    {
                        lorg4.FSSOST_FineAmount = lorg4.FSSOST_FineAmount - Convert.ToInt64(x.FTPOST_PaidAmount);
                    }
                    //added on 11-07-2018


                    if (lorg4.FSSOST_NetAmount != 0)
                    {
                        lorg4.FSSOST_ToBePaid = lorg4.FSSOST_ToBePaid + x.FTPOST_PaidAmount;
                    }
                    else
                    {
                        lorg4.FSSOST_ToBePaid = 0;
                    }

                    //lorg4.FSSOST_ToBePaid = lorg4.FSSOST_ToBePaid + x.FTPOST_PaidAmount;
                    lorg4.UpdatedDate = DateTime.Now;
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

        public FeeStaffOthersTransactionDTO getacademicyear(FeeStaffOthersTransactionDTO data)
        {
            try
            {
                var fetchmaxfypid = _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ID == data.ASMAY_Id && t.user_id == data.userid).OrderByDescending(t => t.FYP_Id).Take(5).Select(t => t.FYP_Id).ToList();

                data.receiparraydelete = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                                          from d in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                                          from e in _YearlyFeeGroupMappingContext.admissioncls
                                          from f in _YearlyFeeGroupMappingContext.school_M_Section
                                          where (f.ASMS_Id == d.ASMS_Id && fetchmaxfypid.Contains(a.FYP_Id) && a.ASMAY_ID == d.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && d.ASMCL_Id == e.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id)
                                          select new FeeStaffOthersTransactionDTO
                                          {
                                              Amst_Id = c.AMST_Id,
                                              AMST_FirstName = c.AMST_FirstName,
                                              AMST_MiddleName = c.AMST_MiddleName,
                                              AMST_LastName = c.AMST_LastName,
                                              FYP_Receipt_No = a.FYP_Receipt_No,
                                              FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                              FYP_Tot_Amount = a.FYP_Tot_Amount,
                                              classname = e.ASMCL_ClassName,
                                              sectionname = f.ASMC_SectionName,
                                              FYP_Id = a.FYP_Id,
                                              AMST_AdmNo = c.AMST_AdmNo,
                                              FYP_Date = a.FYP_Date
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

                data.staff_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_StaffDMO
                                           from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                           from d in _YearlyFeeGroupMappingContext.MasterEmployee
                                           from e in _YearlyFeeGroupMappingContext.HR_Master_Department
                                           from f in _YearlyFeeGroupMappingContext.HR_Master_Designation
                                           where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && e.MI_Id == a.MI_Id && f.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.HRME_Id == d.HRME_Id && d.HRME_ActiveFlag == true && e.HRMD_Id == d.HRMD_Id && e.HRMD_ActiveFlag == true && f.HRMDES_Id == d.HRMDES_Id && f.HRMDES_ActiveFlag == true && a.user_id == data.userid && a.ASMAY_ID == data.ASMAY_Id)
                                           select new FeeStaffOthersTransactionDTO
                                           {
                                               FYP_Id = a.FYP_Id,
                                               HRME_Id = d.HRME_Id,
                                               HRME_EmployeeCode = d.HRME_EmployeeCode,
                                               HRME_EmployeeFirstName = ((d.HRME_EmployeeFirstName == null ? " " : d.HRME_EmployeeFirstName) + " " + (d.HRME_EmployeeMiddleName == null ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null ? " " : d.HRME_EmployeeLastName)).Trim(),
                                               HRMD_DepartmentName = e.HRMD_DepartmentName,
                                               HRMDES_DesignationName = f.HRMDES_DesignationName,
                                               FYP_Receipt_No = a.FYP_Receipt_No,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_Tot_Amount = a.FYP_Tot_Amount,
                                               FYP_Date = a.FYP_Date
                                           }).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
                data.others_paid_details = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                            from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_OthStuDMO
                                            from c in _YearlyFeeGroupMappingContext.Fee_T_Payment_OthStaffDMO
                                            from d in _YearlyFeeGroupMappingContext.FeeMasterOtherStudentDMO
                                            where (a.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && a.FYP_Id == b.FYP_Id && c.FYP_Id == a.FYP_Id && b.FMOST_Id == d.FMOST_Id && d.FMOST_ActiveFlag == true && a.user_id == data.userid && a.ASMAY_ID == data.ASMAY_Id)
                                            select new FeeStaffOthersTransactionDTO
                                            {
                                                FYP_Id = a.FYP_Id,
                                                FMOST_Id = d.FMOST_Id,
                                                FMOST_StudentName = d.FMOST_StudentName,
                                                FMOST_StudentMobileNo = d.FMOST_StudentMobileNo,
                                                FMOST_StudentEmailId = d.FMOST_StudentEmailId,
                                                FYP_Receipt_No = a.FYP_Receipt_No,
                                                FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                                FYP_Tot_Amount = a.FYP_Tot_Amount,
                                                FYP_Date = a.FYP_Date
                                            }).Distinct().OrderByDescending(t => t.FYP_Id).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}



