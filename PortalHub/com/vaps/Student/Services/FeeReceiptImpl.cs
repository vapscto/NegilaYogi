using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using PreadmissionDTOs.com.vaps.Fees;
using CommonLibrary;
using System.Globalization;
using DataAccessMsSqlServerProvider;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;

namespace PortalHub.com.vaps.Student.Services
{
    public class FeeReceiptImpl : Interfaces.FeeReceiptInterface
    {
        private static ConcurrentDictionary<string, StudentDashboardDTO> _login =
           new ConcurrentDictionary<string, StudentDashboardDTO>();
        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        private PortalContext _Feecontext;
        public FeeReceiptImpl(PortalContext Feecontext, FeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context)
        {
            _Feecontext = Feecontext;
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;
        }

        public StudentDashboardDTO getloaddata(StudentDashboardDTO data)
         {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _YearlyFeeGroupMappingContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();

                //var allyear = (from A in _YearlyFeeGroupMappingContext.AcademicYear
                //               from B in _context.StudentApplication
                //               where (A.ASMAY_Id == B.ASMAY_Id && B.Id == data.User_Id && A.MI_Id == data.MI_Id)
                //               select new StudentDashboardDTO
                //               {
                //                   ASMAY_Id = A.ASMAY_Id,
                //                   ASMAY_Year = A.ASMAY_Year,
                //                   ASMAY_Order = A.ASMAY_Order
                //               }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();


                data.yearlist = allyear.Distinct().ToArray();

                data.specialheaddetails = (from a in _YearlyFeeGroupMappingContext.feespecialHead
                                           from b in _YearlyFeeGroupMappingContext.feeSGGG
                                           from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                           where (a.MI_Id == data.MI_Id && a.FMSFH_ActiceFlag == true && a.FMSFH_Id == b.FMSFH_Id && b.FMSFHFH_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == b.FMH_Id && c.FMH_SpecialFeeFlag == true)//&& a.IVRMSTAUL_Id==data.User_Id
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentDashboardDTO getrecdetails(StudentDashboardDTO data)
        {
            try
            {

                var clssec = (from a in _Feecontext.Adm_M_Student
                              from b in _Feecontext.School_Adm_Y_StudentDMO
                              from c in _Feecontext.School_M_Class
                              from s in _Feecontext.School_M_Section
                              where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id)
                              select new StudentDashboardDTO
                              {
                                  ASMCL_Id = c.ASMCL_Id,
                                  ASMCL_ClassName = c.ASMCL_ClassName,
                                  ASMS_Id = s.ASMS_Id,
                                  ASMC_SectionName = s.ASMC_SectionName

                              }).Distinct().ToArray();

                data.recnolist = (from a in _Feecontext.FeePaymentDetailsDMO
                                  from b in _Feecontext.Fee_Y_Payment_School_StudentDMO
                                  where (a.FYP_Id == b.FYP_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag == "Sucessfull" && b.AMST_Id == data.AMST_Id)
                                  orderby a.FYP_Receipt_No

                                  select new StudentDashboardDTO
                                  {
                                      FYP_Id = a.FYP_Id,
                                      FYP_Receipt_No = a.FYP_Receipt_No
                                  }
                 ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeeStudentTransactionDTO Printterm(FeeStudentTransactionDTO data)
        {
            try
            {
                data.srkvsdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                     from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                     from c in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                     where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == b.FYP_Id && b.FMA_Id == c.FMA_Id && a.ASMAY_ID == c.ASMAY_Id && c.AMST_Id == data.Amst_Id
                                     select new FeeStudentTransactionDTO
                                     {
                                         FTP_Paid_Amt = b.FTP_Paid_Amt,
                                         FTP_Concession_Amt = Convert.ToDecimal(c.FSS_ConcessionAmount),
                                         FSS_ToBePaid = c.FSS_ToBePaid,
                                         FSS_TotalToBePaid = c.FSS_TotalToBePaid,
                                         FSS_CurrentYrCharges = c.FSS_CurrentYrCharges
                                     }
       ).ToArray();

                data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                          from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                          from f in _YearlyFeeGroupMappingContext.feespecialHead
                                          from g in _YearlyFeeGroupMappingContext.feeSGGG
                                          where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMH_FeeName = f.FMSFH_Name,
                                              FMH_Id = e.FMH_Id,
                                              FTP_Paid_Amt = c.FTP_Paid_Amt,
                                              FTP_Concession_Amt = c.FTP_Concession_Amt,
                                              FTP_Fine_Amt = c.FTP_Fine_Amt,
                                              FTI_Id = d.FTI_Id,
                                              totalcharges = d.FMA_Amount
                                          }
              ).ToArray();

                var findfmgid = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMG_Id = d.FMG_Id,
                                     FMH_Id = d.FMH_Id
                                 }).Distinct().ToArray();

                data.termremarks = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    from c in _YearlyFeeGroupMappingContext.feeMTH
                                    from d in _YearlyFeeGroupMappingContext.feeTr
                                    where c.FMT_Id == d.FMT_Id && a.FMA_Id == b.FMA_Id && b.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id
                                    select new FeeStudentTransactionDTO
                                    {
                                        termname = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id
                                           select new FeeStudentTransactionDTO
                                           {
                                               FTP_Paid_Amt = a.FYP_Tot_Amount,
                                               FTP_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                               FYP_Date = a.FYP_Date,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,
                                           }).Distinct().ToArray();


                using (var cmd = _YearlyFeeGroupMappingContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_fillstudentviewdetails";
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
                //                               from l in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                               from m in _YearlyFeeGroupMappingContext.feeTr
                //                               from n in _YearlyFeeGroupMappingContext.FeeMasterTermHeadsDMO
                //                               where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && l.FMH_Id == n.FMH_Id && n.FMT_Id == m.FMT_Id && l.FTI_Id == n.FTI_Id && b.ASMAY_ID == data.ASMAY_Id
                //                             && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id && b.FYP_Id == data.FYP_Id && l.AMST_Id == f.AMST_Id && l.FMA_Id == c.FMA_Id && l.FMH_Id == c.FMH_Id && l.ASMAY_Id == f.ASMAY_Id && l.FTI_Id == c.FTI_Id && l.MI_Id == i.MI_Id && l.FMA_Id == a.FMA_Id)
                //                               select new FeeStudentTransactionDTO
                //                               {
                //                                   Amst_Id = f.AMST_Id,
                //                                   AMST_FirstName = i.AMST_FirstName,
                //                                   AMST_MiddleName = i.AMST_MiddleName,
                //                                   AMST_LastName = i.AMST_LastName,
                //                                   FMH_Id = d.FMH_Id,
                //                                   FMH_FeeName = d.FMH_FeeName,
                //                                   FTI_Name = e.FTI_Name,
                //                                   FTI_Id = e.FTI_Id,
                //                                   FYP_Receipt_No = b.FYP_Receipt_No,
                //                                   FTP_Paid_Amt = a.FTP_Paid_Amt,
                //                                   FTP_Concession_Amt = l.FSS_ConcessionAmount,
                //                                   FTP_Fine_Amt = a.FTP_Fine_Amt,
                //                                   FSS_ToBePaid = l.FSS_ToBePaid,
                //                                   FYP_Date = b.FYP_Date,
                //                                   classname = g.ASMCL_ClassName,
                //                                   sectionname = h.ASMC_SectionName,
                //                                   rollno = f.AMAY_RollNo,
                //                                   admno = i.AMST_AdmNo,
                //                                   fathername = i.AMST_FatherName,
                //                                   mothername = i.AMST_MotherName,
                //                                   FYP_Bank_Or_Cash = b.FYP_Bank_Or_Cash,
                //                                   FYP_DD_Cheque_No = b.FYP_DD_Cheque_No,
                //                                   FYP_DD_Cheque_Date = b.FYP_DD_Cheque_Date,
                //                                   FYP_Bank_Name = b.FYP_Bank_Name,
                //                                   FYP_Remarks = b.FYP_Remarks,
                //                                   AMST_RegistrationNo = i.AMST_RegistrationNo,
                //                                   FMCC_ConcessionName = k.FMCC_ConcessionName,
                //                                   totalcharges = c.FMA_Amount,
                //                                   FSS_AdjustedAmount = l.FSS_AdjustedAmount,
                //                                   amst_mobile = i.AMST_MobileNo,
                //                                   FYP_ChallanNo = b.FYP_ChallanNo,
                //                                   FMH_Order = d.FMH_Order,
                //                                   fyp_transaction_id = b.fyp_transaction_id,
                //                                   FMT_Name = m.FMT_Name
                //                               }).Distinct().OrderBy(t => t.FMH_Order).ToArray();
                // data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                //                                from b in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                                from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                //                                from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                                from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                //                                from f in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                                from g in _YearlyFeeGroupMappingContext.admissioncls
                //                                from h in _YearlyFeeGroupMappingContext.school_M_Section
                //                                from i in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                                from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                //                                from k in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                //                                from l in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                                where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && b.ASMAY_ID == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id && b.FYP_Id == data.FYP_Id && l.AMST_Id == f.AMST_Id && l.FMA_Id == c.FMA_Id && l.FMH_Id == c.FMH_Id && l.ASMAY_Id == f.ASMAY_Id && l.FTI_Id == c.FTI_Id && l.MI_Id == i.MI_Id && l.FMA_Id == a.FMA_Id)
                //                                select new FeeStudentTransactionDTO
                //                                {
                //                                    Amst_Id = f.AMST_Id,
                //                                    AMST_FirstName = i.AMST_FirstName,
                //                                    AMST_MiddleName = i.AMST_MiddleName,
                //                                    AMST_LastName = i.AMST_LastName,
                //                                    FMH_Id = d.FMH_Id,
                //                                    FMH_FeeName = d.FMH_FeeName,
                //                                    FTI_Name = e.FTI_Name,
                //                                    FTI_Id = e.FTI_Id,
                //                                    FYP_Receipt_No = b.FYP_Receipt_No,
                //                                    FTP_Paid_Amt = a.FTP_Paid_Amt,
                //                                    FTP_Concession_Amt = l.FSS_ConcessionAmount,
                //                                    FTP_Fine_Amt = a.FTP_Fine_Amt,
                //                                    FYP_Date = b.FYP_Date,
                //                                    classname = g.ASMCL_ClassName,
                //                                    sectionname = h.ASMC_SectionName,
                //                                    rollno = f.AMAY_RollNo,
                //                                    admno = i.AMST_AdmNo,
                //                                    fathername = i.AMST_FatherName,
                //                                    mothername = i.AMST_MotherName,
                //                                    FYP_Bank_Or_Cash = b.FYP_Bank_Or_Cash,
                //                                    FYP_DD_Cheque_No = b.FYP_DD_Cheque_No,
                //                                    FYP_DD_Cheque_Date = b.FYP_DD_Cheque_Date,
                //                                    FYP_Bank_Name = b.FYP_Bank_Name,
                //                                    FYP_Remarks = b.FYP_Remarks,
                //                                    AMST_RegistrationNo = i.AMST_RegistrationNo,
                //                                    FMCC_ConcessionName = k.FMCC_ConcessionName,
                //                                    totalcharges = c.FMA_Amount,
                //                                    FSS_AdjustedAmount = l.FSS_AdjustedAmount,
                //                                    FSS_ToBePaid = l.FSS_ToBePaid,
                //                                    amst_mobile = i.AMST_MobileNo,
                //                                    FYP_ChallanNo = b.FYP_ChallanNo,
                //                                    FMH_Order = d.FMH_Order,
                //                                    fyp_transaction_id = b.fyp_transaction_id
                //                                }
                //).Distinct().OrderBy(t => t.FMH_Order).ToArray();


                // data.fillstudentviewdetails = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                //                                from b in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //                                from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                //                                from d in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                                from e in _YearlyFeeGroupMappingContext.FeeInstallmentsyearlyDMO
                //                                from f in _YearlyFeeGroupMappingContext.School_Adm_Y_StudentDMO
                //                                from g in _YearlyFeeGroupMappingContext.admissioncls
                //                                from h in _YearlyFeeGroupMappingContext.school_M_Section
                //                                from i in _YearlyFeeGroupMappingContext.AdmissionStudentDMO
                //                                from j in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                //                                from k in _YearlyFeeGroupMappingContext.Fee_Master_ConcessionDMO
                //                                where (i.AMST_Concession_Type == k.FMCC_Id && a.FYP_Id == b.FYP_Id && a.FMA_Id == c.FMA_Id && c.FMH_Id == d.FMH_Id && c.FTI_Id == e.FTI_Id && j.FYP_Id == b.FYP_Id && f.AMST_Id == j.AMST_Id && f.ASMCL_Id == g.ASMCL_Id && f.ASMS_Id == h.ASMS_Id && i.AMST_Id == j.AMST_Id && f.ASMAY_Id == data.ASMAY_Id && i.MI_Id == data.MI_Id && j.AMST_Id == data.Amst_Id && b.FYP_Id == data.FYP_Id)
                //                                select new FeeStudentTransactionDTO
                //                                {
                //                                    Amst_Id = f.AMST_Id,
                //                                    AMST_FirstName = i.AMST_FirstName,
                //                                    AMST_MiddleName = i.AMST_MiddleName,
                //                                    AMST_LastName = i.AMST_LastName,
                //                                    FMH_Id = d.FMH_Id,
                //                                    FMH_FeeName = d.FMH_FeeName,
                //                                    FTI_Name = e.FTI_Name,
                //                                    FTI_Id = e.FTI_Id,
                //                                    FYP_Receipt_No = b.FYP_Receipt_No,
                //                                    FTP_Paid_Amt = a.FTP_Paid_Amt,
                //                                    FTP_Concession_Amt = a.FTP_Concession_Amt,
                //                                    FTP_Fine_Amt = a.FTP_Fine_Amt,
                //                                    FYP_Date = b.FYP_Date,
                //                                    classname = g.ASMCL_ClassName,
                //                                    sectionname = h.ASMC_SectionName,
                //                                    rollno = f.AMAY_RollNo,
                //                                    admno = i.AMST_AdmNo,
                //                                    fathername = i.AMST_FatherName,
                //                                    mothername = i.AMST_MotherName,
                //                                    FYP_Bank_Or_Cash = b.FYP_Bank_Or_Cash,
                //                                    FYP_DD_Cheque_No = b.FYP_DD_Cheque_No,
                //                                    FYP_DD_Cheque_Date = b.FYP_DD_Cheque_Date,
                //                                    FYP_Bank_Name = b.FYP_Bank_Name,
                //                                    FYP_Remarks = b.FYP_Remarks,
                //                                    AMST_RegistrationNo = i.AMST_RegistrationNo,
                //                                    FMCC_ConcessionName = k.FMCC_ConcessionName,
                //                                    totalcharges = c.FMA_Amount,
                //                                }
                //).Distinct().ToArray();

                data.filltotaldetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                         where (a.ASMAY_ID == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id)
                                         select new FeeStudentTransactionDTO
                                         {
                                             FYP_Tot_Amount = a.FYP_Tot_Amount,
                                             FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                             FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                         }
              ).Distinct().ToArray();


                data.pendingamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                      where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FSS_ToBePaid = a.FSS_ToBePaid
                                      }
   ).Sum(t => t.FSS_ToBePaid);


                //to find next due amount
                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from g in _YearlyFeeGroupMappingContext.feeTr
                               from h in _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO
                               where (h.ASMAY_ID == data.ASMAY_Id && h.FMT_Id == e.FMT_Id && e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id && d.FSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                               select new FeeStudentTransactionDTO
                               {
                                   fmt_id = e.FMT_Id,
                                   fmt_order = g.FMT_Order,
                                   FMT_Year = h.FMTP_Year
                               }

                 ).Distinct().OrderBy(t => t.fmt_order).ToArray();

                //long fmt_id_new = 0;
                //long initialfmtids = Convert.ToInt64(feeterm[0].fmt_id);
                //fmt_id_new = Convert.ToInt64(feeterm[0].fmt_id) + 1;

                int fmtorder_end = 0;
                string fmt_id_int = "0", fmt_id_end = "0", fmt_id_end_year = "0";
                fmt_id_int = feeterm[0].fmt_id.ToString();
                fmt_id_end = feeterm[feeterm.Length - 1].fmt_id.ToString();

                fmtorder_end = Convert.ToInt32(feeterm[feeterm.Length - 1].fmt_order);
                fmt_id_end_year = (feeterm[feeterm.Length - 1].FMT_Year).ToString();

                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order == fmtorder_end + 1).Select(t => t.FMT_Id);

                List<long> termmidsnew = new List<long>();
                foreach (var item in feeterm)
                {
                    termmidsnew.Add(item.fmt_id);
                }

                List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (b.FYP_Id == c.FYP_Id && a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id == data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new FeeStudentTransactionDTO
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

                List<FeeStudentTransactionDTO> fordate = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> fordateinfyp = new List<FeeStudentTransactionDTO>();

                var nextduedate = "0";

                if (term_ids.Count() > 0)
                {
                    nextduedate = (Convert.ToInt32(fmt_id_end) + 1).ToString();

                    fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                               where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && (d.FSS_ToBePaid > 0))
                               select new FeeStudentTransactionDTO
                               {
                                   date = f.FTDD_Day,
                                   month = f.FTDD_Month,
                               }
                        ).Distinct().ToList();

                }
                else
                {
                    nextduedate = (Convert.ToInt32(fmt_id_end)).ToString();

                    fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                               where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id) && (d.FSS_ToBePaid > 0))
                               select new FeeStudentTransactionDTO
                               {
                                   date = f.FTDD_Day,
                                   month = f.FTDD_Month,
                               }
                        ).Distinct().ToList();
                }

                termmidsnew.Add(Convert.ToInt32(nextduedate));

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new FeeStudentTransactionDTO
                                {
                                    month = f.FTDD_Month,
                                }
                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (FeeStudentTransactionDTO item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (FeeStudentTransactionDTO itemperiod in fordateinfyp)
                {
                    startperiod.Add(Convert.ToInt32(itemperiod.month));
                }

                foreach (var item in months)
                {
                    if (Convert.ToInt32(item) <= 12 && Convert.ToInt32(item) > 3)
                    {
                        months1.Add(Convert.ToInt32(item));
                        var curyear = DateTime.Now;
                        var nextyr = curyear.Year;
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
                            var nextyr = curyear.Year;
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

                var compusoryflag = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(d => d.MI_Id == data.MI_Id && grp_ids.Contains(d.FMG_Id)).Select(t => t.FMG_CompulsoryFlag).Distinct().ToArray();

                if (compusoryflag[0].ToString() == "T")
                {
                    var termperiodlistint = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                    var termperiodlistend = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                    monthnameinitial = termperiodlistint[0].Transport_FromMonth.ToString();
                    monthnameend = termperiodlistend[0].Transport_ToMonth.ToString();

                    string yeardisplay = "0";
                    yeardisplay = fmt_id_end_year;

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;
                }
                else
                {
                    var termperiodlistint = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_int)).ToArray();

                    var termperiodlistend = _YearlyFeeGroupMappingContext.feeTr.Where(d => d.MI_Id == data.MI_Id && d.FMT_Id == Convert.ToInt32(fmt_id_end)).ToArray();

                    monthnameinitial = termperiodlistint[0].FromMonth.ToString();
                    monthnameend = termperiodlistend[0].ToMonth.ToString();

                    string yeardisplay = "0";
                    yeardisplay = fmt_id_end_year;

                    data.duration = monthnameinitial + '-' + monthnameend + '-' + yeardisplay;
                }



                //new one
                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && termmidsnew.Contains(b.FMT_Id) && c.FMH_Flag != "E")
                                  select new FeeStudentTransactionDTO
                                  {
                                      FSS_ToBePaid = a.FSS_ToBePaid
                                  }
          ).Sum(t => t.FSS_ToBePaid);



                //old one
                //   data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                     from b in _YearlyFeeGroupMappingContext.feeMTH
                //                     from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                     where (b.FMH_Id==c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag!="E")
                //                     select new FeeStudentTransactionDTO
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
        public FeeStudentTransactionDTO Printinstallment(FeeStudentTransactionDTO data)
        {
            try
            {
                string yeardisplay = "0";


                data.receiptformathead = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                          from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                          from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                          from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                          from e in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                          from f in _YearlyFeeGroupMappingContext.feespecialHead
                                          from g in _YearlyFeeGroupMappingContext.feeSGGG
                                          from h in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                          where (g.FMH_Id == e.FMH_Id && f.FMSFH_Id == g.FMSFH_Id && a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMH_Id == e.FMH_Id && b.AMST_Id == data.Amst_Id && a.MI_Id == data.MI_Id && a.FYP_Id == data.FYP_Id && f.MI_Id == data.MI_Id && h.MI_Id == f.MI_Id && h.FMH_Id == g.FMH_Id && h.FMA_Id == c.FMA_Id && h.FTI_Id == d.FTI_Id && h.ASMAY_Id == data.ASMAY_Id)
                                          select new FeeStudentTransactionDTO
                                          {
                                              FMH_FeeName = f.FMSFH_Name,
                                              FMH_Id = e.FMH_Id,
                                              FTP_Paid_Amt = c.FTP_Paid_Amt,
                                              FTP_Concession_Amt = c.FTP_Concession_Amt,
                                              FTP_Fine_Amt = c.FTP_Fine_Amt,
                                              FTI_Id = d.FTI_Id,
                                              // totalcharges = d.FMA_Amount,
                                              FSS_AdjustedAmount = h.FSS_AdjustedAmount
                                          }
             ).Distinct().ToArray();


                var findfmgid = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                 from b in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                 where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_Id == data.FYP_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMG_Id = d.FMG_Id,
                                     FMH_Id = d.FMH_Id
                                 }).Distinct().ToArray();




                data.termremarks = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                    from b in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                    from c in _YearlyFeeGroupMappingContext.feeMTH
                                    from d in _YearlyFeeGroupMappingContext.feeTr
                                    where c.FMT_Id == d.FMT_Id && a.FMA_Id == b.FMA_Id && b.FMH_Id == c.FMH_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FYP_Id == data.FYP_Id
                                    select new FeeStudentTransactionDTO
                                    {
                                        termname = d.FMT_Name
                                    }
                                    ).Distinct().ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id
                                           select new FeeStudentTransactionDTO
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



                List<FeeStudentTransactionDTO> result = new List<FeeStudentTransactionDTO>();

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
                            //    result.Add(new FeeStudentTransactionDTO
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
                                         select new FeeStudentTransactionDTO
                                         {
                                             FYP_Tot_Amount = a.FYP_Tot_Amount,
                                             FYP_Tot_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                             FYP_Tot_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                         }
              ).Distinct().ToArray();



                data.pendingamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                      where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id)
                                      select new FeeStudentTransactionDTO
                                      {
                                          FSS_ToBePaid = a.FSS_ToBePaid
                                      }
).Sum(t => t.FSS_ToBePaid);



                //to find next due amount

                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from g in _YearlyFeeGroupMappingContext.feeTr
                               from h in _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO
                               where (h.ASMAY_ID == data.ASMAY_Id && h.FMT_Id == e.FMT_Id && e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id && d.FSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                               select new FeeStudentTransactionDTO
                               {
                                   fmt_id = e.FMT_Id,
                                   fmt_order = g.FMT_Order,
                                   FMT_Year = h.FMTP_Year
                               }

               ).Distinct().OrderBy(t => t.fmt_order).ToArray();

                //var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                //               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                //               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                //               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //               from e in _YearlyFeeGroupMappingContext.feeMTH
                //               from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //               where ( e.FMH_Id==f.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id && d.FSS_PaidAmount>0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                //               select new FeeStudentTransactionDTO
                //               {
                //                   fmt_id = e.FMT_Id,
                //                   fmt_order = g.FMT_Order,
                //                   FMT_Year = g.FMT_Year
                //               }

                // ).Distinct().ToArray();

                //long fmt_id_new = 0;

                //long initialfmtids = Convert.ToInt64(feeterm[0].fmt_id);

                //fmt_id_new = Convert.ToInt64(feeterm[0].fmt_id) + 1;


                int fmtorder_end = 0;
                string fmt_id_int = "0", fmt_id_end = "0", fmt_id_end_year = "0"; ;
                fmt_id_int = feeterm[0].fmt_id.ToString();
                fmt_id_end = feeterm[feeterm.Length - 1].fmt_id.ToString();

                fmtorder_end = Convert.ToInt32(feeterm[feeterm.Length - 1].fmt_order);
                fmt_id_end_year = (feeterm[feeterm.Length - 1].FMT_Year).ToString();

                var term_ids = _YearlyFeeGroupMappingContext.feeTr.Where(t => t.MI_Id == data.MI_Id && t.FMT_Order == fmtorder_end + 1).Select(t => t.FMT_Id);

                List<long> termmidsnew = new List<long>();
                foreach (var item in feeterm)
                {
                    termmidsnew.Add(item.fmt_id);
                }


                List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new FeeStudentTransactionDTO
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
                var userids = _YearlyFeeGroupMappingContext.FeeGroupDMO.Where(d => grp_ids.Contains(d.FMG_Id)).Distinct().Select(t => t.user_id).ToList();
                data.userid = userids.FirstOrDefault();

                List<FeeStudentTransactionDTO> fordate = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> fordateinfyp = new List<FeeStudentTransactionDTO>();

                fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                           from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                           where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && d.User_Id == data.userid)
                           select new FeeStudentTransactionDTO
                           {
                               date = f.FTDD_Day,
                               month = f.FTDD_Month,
                           }

                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new FeeStudentTransactionDTO
                                {
                                    month = f.FTDD_Month,
                                }

                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (FeeStudentTransactionDTO item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (FeeStudentTransactionDTO itemperiod in fordateinfyp)
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
                            var nextyr = curyear.Year + 1;
                            data.year = nextyr.ToString();
                        }
                        else
                        {
                            months2.Add(Convert.ToInt32(item));
                            var curyear = DateTime.Now;
                            var nextyr = curyear.Year - 1;
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
                //      data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                //                        from b in _YearlyFeeGroupMappingContext.feeMTH
                //                        from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                //                        where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag != "E")
                //                        select new FeeStudentTransactionDTO
                //                        {
                //                            FSS_ToBePaid = a.FSS_ToBePaid
                //                        }
                //).Sum(t => t.FSS_ToBePaid);

                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && termmidsnew.Contains(b.FMT_Id) && c.FMH_Flag != "E" && a.User_Id == data.userid)
                                  select new FeeStudentTransactionDTO
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
        public async Task<FeeStudentTransactionDTO> printreceipt([FromBody] FeeStudentTransactionDTO data)
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


                data.specialheaddetails = (from a in _YearlyFeeGroupMappingContext.feespecialHead
                                           from b in _YearlyFeeGroupMappingContext.feeSGGG
                                           from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                           where (a.MI_Id == data.MI_Id && a.FMSFH_ActiceFlag == true && a.FMSFH_Id == b.FMSFH_Id && b.FMSFHFH_ActiceFlag == true && c.MI_Id == data.MI_Id && c.FMH_ActiveFlag == true && c.FMH_Id == b.FMH_Id && c.FMH_SpecialFeeFlag == true)//&& a.IVRMSTAUL_Id==data.User_Id
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

                try
                {
                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "feereceipt/" + data.MI_Id, "PortalFeeReceipt.html", 0);
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
                        Printterm(data);
                    }
                }
                else { printcommon(data); }

                var periodnme = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                 from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                 from c in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO
                                 from d in _YearlyFeeGroupMappingContext.FeeGroupDMO
                                 where (a.FMG_Id == c.FMG_Id && a.FMG_Id == d.FMG_Id && b.FMA_Id == c.FMA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.FYP_Id == data.FYP_Id && a.AMST_Id == data.Amst_Id)
                                 select new FeeStudentTransactionDTO
                                 {
                                     FMG_CompulsoryFlag = d.FMG_CompulsoryFlag,
                                 }
               ).Distinct().OrderBy(t => t.fmt_order).ToArray();

                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               from f in _YearlyFeeGroupMappingContext.FeeHeadDMO
                               from g in _YearlyFeeGroupMappingContext.feeTr
                               from h in _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO
                               where (h.ASMAY_ID == data.ASMAY_Id && h.FMT_Id == e.FMT_Id && e.FMT_Id == g.FMT_Id && f.FMH_Id == e.FMH_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id && d.FSS_PaidAmount > 0 && (f.FMH_Flag != "F" && f.FMH_Flag != "E"))
                               select new FeeStudentTransactionDTO
                               {
                                   fmt_id = e.FMT_Id,
                                   fmt_order = g.FMT_Order,
                                   FMT_Year = h.FMTP_Year
                               }

                ).Distinct().OrderBy(t => t.fmt_order).ToArray();

                string startmonth = "";
                string endmonth = "";
                string year1 = "";
                string year2 = "";
                string fmt_id_int = "0", fmt_id_end = "0";

                for (int i = 0; i < feeterm.Length; i++)
                {
                    fmt_id_int = feeterm[0].fmt_id.ToString();
                    fmt_id_end = feeterm[feeterm.Length - 1].fmt_id.ToString();
                }


                //startmonth = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_int) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_FROM_MONTH;

                //endmonth = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_end) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_TO_MONTH;

                //year1 = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_int) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_Year;

                //year2 = _YearlyFeeGroupMappingContext.FEE_MASTER_TERMWISE_PERIOD_DMO.Where(a => a.ASMAY_ID == data.ASMAY_Id && a.USER_ID == data.userid && a.FMT_Id == Convert.ToInt32(fmt_id_end) && a.FeeFlag == periodnme[0].FMG_CompulsoryFlag).ToList().FirstOrDefault().FMTP_Year;

                //data.duration = startmonth + '-' + year1 + '/' + endmonth + '-' + year2;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;

        }
        public async Task<FeeStudentTransactionDTO> printreceiptnew([FromBody] FeeStudentTransactionDTO data)
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

        private FeeStudentTransactionDTO printcommon(FeeStudentTransactionDTO data)
        {
            try
            {

                var fillstudent = (from a in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
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
                                   select new FeeStudentTransactionDTO
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
                                       FTP_Waived_Amt = a.FTP_Waived_Amt,
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
                                       totalcharges = c.FMA_Amount,
                                       fyp_transaction_id = b.fyp_transaction_id
                                   }
               ).Distinct().ToList();

                data.fillstudentviewdetails = fillstudent.ToArray();

                List<long> lstheadid = new List<long>();
                foreach (var lst in fillstudent)
                {
                    lstheadid.Add(lst.FMH_Id);
                }
                FeeStudentTransactionDTO fst = new FeeStudentTransactionDTO();
                List<FeeStudentTransactionDTO> lstfst = new List<FeeStudentTransactionDTO>();
                foreach (var lst1 in lstheadid.Distinct())
                {
                    decimal totalcharges = 0, Fine_Amt = 0, Concession_Amt = 0, Waived_Amt = 0, paidamt = 0;
                    foreach (var lst in fillstudent)
                    {
                        if (lst1.Equals(lst.FMH_Id))
                        {
                            fst.FMH_FeeName = lst.FMH_FeeName;
                            totalcharges = totalcharges + lst.totalcharges;
                            Fine_Amt = Fine_Amt + lst.FTP_Fine_Amt;
                            Concession_Amt = Concession_Amt + lst.FTP_Concession_Amt;
                            Waived_Amt = Waived_Amt + lst.FTP_Waived_Amt;
                            paidamt = paidamt + lst.FTP_Paid_Amt;
                            fst.totalcharges = totalcharges;
                            fst.FTP_Fine_Amt = Fine_Amt;
                            fst.FTP_Concession_Amt = Concession_Amt;
                            fst.FTP_Waived_Amt = Waived_Amt;
                            fst.FTP_Paid_Amt = paidamt;

                        }
                    }
                    lstfst.Add(fst);
                }
                data.filltotaldetails = lstfst.ToArray();

                data.currpaymentdetails = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                                           where a.FYP_Id == data.FYP_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id
                                           select new FeeStudentTransactionDTO
                                           {
                                               FTP_Paid_Amt = a.FYP_Tot_Amount,
                                               FTP_Concession_Amt = a.FYP_Tot_Concession_Amt,
                                               FTP_Fine_Amt = a.FYP_Tot_Fine_Amt,
                                               FTP_Waived_Amt = a.FYP_Tot_Waived_Amt,
                                               FYP_Date = a.FYP_Date,
                                               FYP_Bank_Or_Cash = a.FYP_Bank_Or_Cash,
                                               FYP_DD_Cheque_No = a.FYP_DD_Cheque_No,
                                               FYP_DD_Cheque_Date = a.FYP_DD_Cheque_Date,
                                               FYP_Bank_Name = a.FYP_Bank_Name,
                                               FYP_Remarks = a.FYP_Remarks,
                                           }
              ).Distinct().ToArray();


                List<Institution> masins = new List<Institution>();
                masins = _Feecontext.Institute.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.masterinstitution = masins.ToArray();

                //to find next due amount
                var feeterm = (from a in _YearlyFeeGroupMappingContext.FeePaymentDetailsDMO
                               from b in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                               from c in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                               from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                               from e in _YearlyFeeGroupMappingContext.feeMTH
                               where (a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && b.FMA_Id == d.FMA_Id && d.AMST_Id == c.AMST_Id && d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && d.AMST_Id == data.Amst_Id && a.FYP_Id == data.FYP_Id)
                               select new FeeStudentTransactionDTO
                               {
                                   fmt_id = e.FMT_Id
                               }

                 ).Distinct().OrderByDescending(t => t.fmt_id).ToArray();

                long fmt_id_new = 0;

                long initialfmtids = Convert.ToInt64(feeterm[0].fmt_id);

                fmt_id_new = Convert.ToInt64(feeterm[0].fmt_id) + 1;

                List<FeeStudentTransactionDTO> temp_group_head = new List<FeeStudentTransactionDTO>();
                temp_group_head = (from a in _YearlyFeeGroupMappingContext.Fee_Y_Payment_School_StudentDMO
                                   from b in _YearlyFeeGroupMappingContext.Fee_Payment
                                   from c in _YearlyFeeGroupMappingContext.FeeTransactionPaymentDMO
                                   from d in _YearlyFeeGroupMappingContext.FeeAmountEntryDMO

                                   where (a.AMST_Id == data.Amst_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= data.FYP_Id && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                   select new FeeStudentTransactionDTO
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

                List<FeeStudentTransactionDTO> fordate = new List<FeeStudentTransactionDTO>();
                List<FeeStudentTransactionDTO> fordateinfyp = new List<FeeStudentTransactionDTO>();

                fordate = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                           from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                           where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id) && (d.FSS_ToBePaid > 0))
                           select new FeeStudentTransactionDTO
                           {
                               date = f.FTDD_Day,
                               month = f.FTDD_Month,
                           }
                          ).Distinct().ToList();

                fordateinfyp = (from d in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                from f in _YearlyFeeGroupMappingContext.feeTDueDateRegularDMO
                                where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.Amst_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                select new FeeStudentTransactionDTO
                                {
                                    month = f.FTDD_Month,
                                }
                         ).Distinct().ToList();

                List<int> months = new List<int>();
                List<int> dates = new List<int>();
                List<int> months1 = new List<int>();
                List<int> months2 = new List<int>();

                List<int> startperiod = new List<int>();

                foreach (FeeStudentTransactionDTO item in fordate)
                {
                    dates.Add(Convert.ToInt32(item.date));
                    months.Add(Convert.ToInt32(item.month));
                }

                foreach (FeeStudentTransactionDTO itemperiod in fordateinfyp)
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

                data.dueamount = (from a in _YearlyFeeGroupMappingContext.FeeStudentTransactionDMO
                                  from b in _YearlyFeeGroupMappingContext.feeMTH
                                  from c in _YearlyFeeGroupMappingContext.FeeHeadDMO
                                  where (b.FMH_Id == c.FMH_Id && a.FMH_Id == b.FMH_Id && a.FTI_Id == b.FTI_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_Id && grp_ids.Contains(a.FMG_Id) && b.FMT_Id == fmt_id_new && c.FMH_Flag != "E")
                                  select new FeeStudentTransactionDTO
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


        public string duadatecollect(long asmayid, long userid, long asmclid)
        {
            string date = "";
            List<FeetransactionSMS> Due_Date_array = new List<FeetransactionSMS>();
            List<FeetransactionSMS> result_duadate = new List<FeetransactionSMS>();
            using (var cmdnew = _Feecontext.Database.GetDbConnection().CreateCommand())
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

        public StudentDashboardDTO preadmissiongetrecdetails(StudentDashboardDTO data)
        {
            try
            {

                var studetailslist = (from a in _Feecontext.stuapp
                                      where (a.Id == data.User_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                      select new StudentDashboardDTO
                                      {
                                          AMST_Id = a.pasr_id,
                                          //AMST_FirstName = a.PASR_FirstName + "" + a.PASR_RegistrationNo,
                                          AMST_FirstName = ((a.PASR_FirstName == null || a.PASR_FirstName == "0" ? "" : a.PASR_FirstName) + " " + (a.PASR_MiddleName == null || a.PASR_MiddleName == "0" ? "" : a.PASR_MiddleName) + " " + (a.PASR_LastName == null || a.PASR_LastName == "0" ? "" : a.PASR_LastName)).Trim()
                                      });

                data.studetailslist = studetailslist.ToArray();

                List<long> stuid = new List<long>();
                foreach (var item in studetailslist)
                {
                    stuid.Add(item.AMST_Id);
                }

                if (data.studetailslist.Length == 1)
                {
                    data.recnolist = (from a in _Feecontext.FeePaymentDetailsDMO
                                      from b in _Feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                      where (a.FYP_Id == b.FYP_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag == "Sucessfull" && stuid.Contains(b.PASA_Id))
                                      orderby a.FYP_Receipt_No

                                      select new StudentDashboardDTO
                                      {
                                          FYP_Id = a.FYP_Id,
                                          FYP_Receipt_No = a.FYP_Receipt_No
                                      }
                ).Distinct().ToArray();
                }

                //data.recnolist = (from a in _Feecontext.FeePaymentDetailsDMO
                //                  from b in _Feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO
                //                  where (a.FYP_Id == b.FYP_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag == "Sucessfull" && b.PASA_Id == data.AMST_Id)
                //                  orderby a.FYP_Receipt_No

                //                  select new StudentDashboardDTO
                //                  {
                //                      FYP_Id = a.FYP_Id,
                //                      FYP_Receipt_No = a.FYP_Receipt_No
                //                  }
                //).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentDashboardDTO preadmissiongetdetails(StudentDashboardDTO data)
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
                try
                {
                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "feereceipt/" + data.MI_Id, "PreadmissionFeeReceipt.html", 0);
                }
                catch (Exception ex)
                { html = ""; }

                data.htmldata = html;

                if (html != "")
                {

                    var studetailslist = (from a in _Feecontext.stuapp
                                          where (a.Id == data.User_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                          select new StudentDashboardDTO
                                          {
                                              AMST_Id = a.pasr_id,
                                              AMST_FirstName = ((a.PASR_FirstName == null || a.PASR_FirstName == "0" ? "" : a.PASR_FirstName) + " " + (a.PASR_MiddleName == null || a.PASR_MiddleName == "0" ? "" : a.PASR_MiddleName) + " " + (a.PASR_LastName == null || a.PASR_LastName == "0" ? "" : a.PASR_LastName)).Trim()
                                          });

                    data.studetailslist = studetailslist.ToArray();

                    List<long> stuid = new List<long>();
                    foreach (var item in studetailslist)
                    {
                        stuid.Add(item.AMST_Id);
                    }

                    data.studentfeedetails = (from a in _Feecontext.stuapp
                                              from b in _Feecontext.School_M_Class
                                              from c in _Feecontext.AcademicYearDMO
                                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && stuid.Contains(a.pasr_id) && a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id)
                                              select new StudentDashboardDTO
                                              {
                                                  AMST_FirstName = ((a.PASR_FirstName == null || a.PASR_FirstName == "0" ? "" : a.PASR_FirstName) + " " + (a.PASR_MiddleName == null || a.PASR_MiddleName == "0" ? "" : a.PASR_MiddleName) + " " + (a.PASR_LastName == null || a.PASR_LastName == "0" ? "" : a.PASR_LastName)).Trim(),
                                                  ASMCL_ClassName = b.ASMCL_ClassName,
                                                  ASMAY_Year = c.ASMAY_Year,
                                                  AMST_AdmNo = a.PASR_RegistrationNo,
                                                  AMST_FatherName = a.PASR_FatherName,
                                                  AMST_MotherName = a.PASR_MotherName,
                                                  AMST_MobileNo = a.PASR_MobileNo
                                              }
               ).Distinct().ToArray();

                    data.recnolist = (from a in _Feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                      from b in _Feecontext.FeePaymentDetailsDMO
                                      from c in _Feecontext.FeeTransactionPaymentDMO
                                      from d in _Feecontext.FeeAmountEntryDMO
                                      from e in _Feecontext.FeeGroupDMO
                                      from f in _Feecontext.FeeHeadDMO
                                      from g in _Feecontext.FeeInstallmentsyearlyDMO
                                      where (a.FYP_Id == b.FYP_Id && b.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && d.FMG_Id == e.FMG_Id && d.FMH_Id == f.FMH_Id && d.FTI_Id == g.FTI_Id && stuid.Contains(a.PASA_Id) && b.ASMAY_ID == d.ASMAY_Id && b.ASMAY_ID == data.ASMAY_Id && b.FYP_Id == data.FYP_Id)
                                      select new StudentDashboardDTO
                                      {
                                          FMH_FeeName = f.FMH_FeeName,
                                          FSS_NetAmount = d.FMA_Amount,
                                          FSS_ToBePaid = Convert.ToInt32((d.FMA_Amount - c.FTP_Paid_Amt)),
                                          FSS_PaidAmount = Convert.ToInt32(c.FTP_Paid_Amt),
                                          FYP_Bank_Name = b.FYP_Bank_Name,
                                          FYP_DD_Cheque_Date = b.FYP_DD_Cheque_Date,
                                          FYP_DD_Cheque_No = b.FYP_DD_Cheque_No,
                                          transactionid = b.FYP_PaymentReference_Id,
                                          FYP_Bank_Or_Cash = b.FYP_Bank_Or_Cash,
                                          FTI_Name = g.FTI_Name,
                                          FYP_Receipt_No = b.FYP_Receipt_No,
                                          FYP_Date = b.FYP_Date
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

        public StudentDashboardDTO getstudetails(StudentDashboardDTO data)
        {
            try
            {
                data.recnolist = (from a in _Feecontext.FeePaymentDetailsDMO
                                  from b in _Feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO
                                  where (a.FYP_Id == b.FYP_Id && a.ASMAY_ID == data.ASMAY_Id && a.FYP_OnlineChallanStatusFlag == "Sucessfull" && b.PASA_Id == data.AMST_Id)
                                  orderby a.FYP_Receipt_No

                                  select new StudentDashboardDTO
                                  {
                                      FYP_Id = a.FYP_Id,
                                      FYP_Receipt_No = a.FYP_Receipt_No
                                  }
            ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}