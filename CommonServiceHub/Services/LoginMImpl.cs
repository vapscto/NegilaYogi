using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.mobile;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using DomainModel.Model.com.vapstech.TT;

using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs;
using CommonLibrary;
using DomainModel.Model;
using Newtonsoft.Json;
using DomainModel.Model.com.vaps.Fee;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Fees;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider.com.vapstech.LeaveManagement;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.LeaveManagement;

namespace CommonServiceHub.Services
{
    public class LoginMImpl : Interfaces.LoginMinterface
    {
        public DomainModelMsSqlServerContext _db;
        public FeeGroupContext _dbf;
        public COEContext _coecontext;
        public ExamContext _examcontext;
        public TTContext _ttcontext;
        public HRMSContext _hrmscontext;
        public StudentAttendanceReportContext _admission;
        public FOContext _FOContext;
        private PortalContext _ChairmanDashboardContext;
        private LMContext _leave;

        public LoginMImpl(DomainModelMsSqlServerContext db, FeeGroupContext dbf, COEContext coecontext, ExamContext examcontext, TTContext ttcontext, HRMSContext hrmscontext, StudentAttendanceReportContext admission, FOContext FOContext, PortalContext ChairmanDashboardContext, LMContext leave)
        {
            _db = db;
            _dbf = dbf;
            _coecontext = coecontext;
            _examcontext = examcontext;
            _ttcontext = ttcontext;
            _hrmscontext = hrmscontext;
            _admission = admission;
            _FOContext = FOContext;
            _ChairmanDashboardContext = ChairmanDashboardContext;
            _leave = leave;
        }
        public StudentdetDTO getdetails(StudentdetDTO.input data)
        {
            StudentdetDTO ldt = new StudentdetDTO();
            try
            {
                //var s = (from a in _db.Adm_M_Student
                //         from b in _db.SchoolYearWiseStudent
                //         from c in _db.admissioncls
                //         from d in _db.Section
                //         where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                //         select new StudentdetDTO
                //         {
                //             AMST_Id = a.AMST_Id,
                //             AMST_RegistrationNo = a.AMST_RegistrationNo,
                //             AMST_FirstName = a.AMST_FirstName,
                //             AMST_MiddleName = a.AMST_MiddleName,
                //             AMST_LastName = a.AMST_LastName,
                //             AMST_DOB = a.AMST_DOB,
                //             AMST_emailId = a.AMST_emailId,
                //             AMST_Photoname = a.AMST_Photoname,
                //             AMST_MobileNo = a.AMST_MobileNo,
                //             AMST_Date = a.AMST_Date,
                //             ASMCL_ClassName = c.ASMCL_ClassName,
                //             ASMC_SectionName = d.ASMC_SectionName
                //         }).Distinct().ToList();

                //foreach (var a in s)
                //{
                //    ldt.AMST_Id = a.AMST_Id;
                //    ldt.AMST_RegistrationNo = a.AMST_RegistrationNo;
                //    ldt.AMST_FirstName = a.AMST_FirstName;
                //    ldt.AMST_MiddleName = a.AMST_MiddleName;
                //    ldt.AMST_LastName = a.AMST_LastName;
                //    ldt.AMST_DOB = a.AMST_DOB;
                //    ldt.AMST_emailId = a.AMST_emailId;
                //    ldt.AMST_Photoname = a.AMST_Photoname;
                //    ldt.AMST_MobileNo = a.AMST_MobileNo;
                //    ldt.AMST_Date = a.AMST_Date;
                //    ldt.ASMCL_ClassName = a.ASMCL_ClassName;
                //    ldt.ASMC_SectionName = a.ASMC_SectionName;
                //    break;
                //}
                var s = (from a in _db.Adm_M_Student
                         where (a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                         select new StudentdetDTO
                         {
                             AMST_Id = a.AMST_Id,
                             AMST_RegistrationNo = a.AMST_RegistrationNo,
                             AMST_FirstName = a.AMST_FirstName,
                             AMST_MiddleName = a.AMST_MiddleName,
                             AMST_LastName = a.AMST_LastName,
                             AMST_DOB = a.AMST_DOB,
                             AMST_emailId = a.AMST_emailId,
                             AMST_Photoname = a.AMST_Photoname,
                             AMST_MobileNo = a.AMST_MobileNo,
                             AMST_Date = a.AMST_Date
                         }).Distinct().ToList();

                var dd = (from b in _db.SchoolYearWiseStudent
                          from c in _db.admissioncls
                          from d in _db.Section
                          where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && c.MI_Id == data.MI_Id && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id)
                          select new StudentdetDTO
                          {
                              ASMCL_ClassName = c.ASMCL_ClassName,
                              ASMC_SectionName = d.ASMC_SectionName
                          }).Distinct().ToList();

                foreach (var a in s)
                {
                    ldt.AMST_Id = a.AMST_Id;
                    ldt.AMST_RegistrationNo = a.AMST_RegistrationNo;
                    ldt.AMST_FirstName = a.AMST_FirstName;
                    ldt.AMST_MiddleName = a.AMST_MiddleName;
                    ldt.AMST_LastName = a.AMST_LastName;
                    ldt.AMST_DOB = a.AMST_DOB;
                    ldt.AMST_emailId = a.AMST_emailId;
                    ldt.AMST_Photoname = a.AMST_Photoname;
                    ldt.AMST_MobileNo = a.AMST_MobileNo;
                    ldt.AMST_Date = a.AMST_Date;
                    break;
                }
                foreach (var a in dd)
                {
                    ldt.ASMCL_ClassName = a.ASMCL_ClassName;
                    ldt.ASMC_SectionName = a.ASMC_SectionName;
                    break;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }

        public async Task<StudentAttendanceDTO> monthlydata(long AMST_Id, long MI_Id, long ASMAY_Id, int monthid)
        {
            StudentAttendanceDTO lt = new StudentAttendanceDTO();
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Mobile_Attendance_monthwise";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                {
                    Value = ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                {
                    Value = AMST_Id
                });
                cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.Int)
                {
                    Value = monthid
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                {
                    Value = MI_Id
                });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                var retObject = new List<dynamic>();
                List<string> present = new List<string>();
                List<string> absent = new List<string>();
                List<string> firsthalf = new List<string>();
                List<string> secondhalf = new List<string>();
                try
                {
                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
                        {
                            present.Add(dataReader["presentdays"].ToString());
                        }
                        if (dataReader.NextResult())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                absent.Add(dataReader["absentdays"].ToString());
                            }
                        }
                        if (dataReader.NextResult())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                firsthalf.Add(dataReader["firsthalfpresent"].ToString());
                            }
                        }
                        if (dataReader.NextResult())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                secondhalf.Add(dataReader["secondhalfpresent"].ToString());
                            }
                        }
                        if (dataReader.NextResult())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                lt.ClassHeld = Convert.ToInt64(dataReader["ClassHeld"]);
                                lt.Class_Attended = Convert.ToInt64(dataReader["Class_Attended"]);
                                lt.Percentage = Convert.ToInt64(dataReader["Percentage"]);
                                break;
                            }
                        }
                        lt.monthid = monthid;

                        lt.Presentdays = present.ToArray();
                        lt.Absentdays = absent.ToArray();
                        lt.FHPdays = firsthalf.ToArray();
                        lt.SHPdays = secondhalf.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return lt;
            }
        }
        public async Task<StudentAttendanceDTO> getAttend(StudentAttendanceDTO.input data)
        {

            StudentAttendanceDTO lt = new StudentAttendanceDTO();
            try
            {
                lt = await monthlydata(data.AMST_Id, data.MI_Id, data.ASMAY_Id, data.monthid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lt;

        }
        public async Task<StudentYAttendanceDTO> getYAttend(StudentYAttendanceDTO.input data)
        {

            StudentAttendanceDTO lt = new StudentAttendanceDTO();
            List<StudentAttendanceDTO> lst = new List<StudentAttendanceDTO>();
            StudentYAttendanceDTO lty = new StudentYAttendanceDTO();
            try
            {
                var result = _db.AcademicYear.Single(y => y.Is_Active == true && y.MI_Id == data.MI_Id && y.ASMAY_Id == data.ASMAY_Id);
                int[] mon1 = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
                foreach (var x in mon1)
                {
                    var date = Convert.ToDateTime(result.ASMAY_From_Date).AddMonths(x);
                    if (date <= DateTime.Now)
                    {
                        lt = await monthlydata(data.AMST_Id, data.MI_Id, data.ASMAY_Id, date.Month);
                        lst.Add(lt);
                    }

                }
                lty.YearlyArray = lst.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lty;

        }
        public StudentFeeDetailsDTO stufeedetails(StudentFeeDetailsDTO.input data)
        {
            StudentFeeDetailsDTO ldt = new StudentFeeDetailsDTO();
            try
            {
                //total tobe paid for this year
                ldt.totalReceipt = (from a in _dbf.FeeStudentTransactionDMO
                                    where (a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select a.FSS_TotalToBePaid).Sum();
                //total concession for this year
                ldt.totalconcession = (from a in _dbf.FeeStudentTransactionDMO
                                       where (a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                       select a.FSS_ConcessionAmount).Sum();

                // miscellaneous once in a carrier
                ldt.totalonceinacarrier = (from a in _dbf.FeeStudentTransactionDMO
                                           from b in _dbf.feeMIY
                                           where (a.FTI_Id == b.FTI_Id && b.FTI_Name == "Once in a Carrier" && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                           select a.FSS_ToBePaid).Sum();

                //miscellaneous Anytime inthis year
                ldt.totalanytime = (from a in _dbf.FeeStudentTransactionDMO
                                    from b in _dbf.feeMIY
                                    where (a.FTI_Id == b.FTI_Id && b.FTI_Name == "Anytime" && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select a.FSS_ToBePaid).Sum();


                //total tobe paid pending for this year
                ldt.totalpending = (from a in _dbf.FeeStudentTransactionDMO
                                    where (a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select a.FSS_ToBePaid).Sum();
                //Payment history
                var paymenthistorynew = (from a in _dbf.Fee_Y_Payment_School_StudentDMO
                                         from b in _dbf.FeePaymentDetailsDMO
                                         where a.FYP_Id == b.FYP_Id && a.AMST_Id == data.AMST_Id && b.ASMAY_ID == data.ASMAY_Id
                                         select new StudentFeeDetailsDTO.PaymenthistoryDTO
                                         {
                                             FYP_Id = a.FYP_Id,
                                             // FTP_Paid_Amt = b.FYP_TotalPaidAmount,
                                             // FYP_Date = b.FYP_ReceiptDate,
                                             // FYP_Bank_Or_Cash = b.FYP_TransactionTypeFlag
                                         }
                 ).OrderByDescending(t => t.FYP_Id).Distinct().ToArray();

                ldt.paymenthistory = paymenthistorynew;

                //var feeterm = (from d in _dbf.FeeStudentTransactionDMO
                //               from e in _dbf.feeMTH
                //               where (d.FMH_Id == e.FMH_Id && d.FTI_Id == e.FTI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.AMST_Id == data.AMST_Id)
                //               select new StudentArray
                //               {
                //                   fmt_id = e.FMT_Id
                //               }).Distinct().ToArray();
                //long fmt_id_new = 0;
                //fmt_id_new = Convert.ToInt64(feeterm[0].fmt_id) + 1;

                List<StudentFeeDetailsDTO.input> temp_group_head = new List<StudentFeeDetailsDTO.input>();
                List<StudentFeeDetailsDTO.input> fordate = new List<StudentFeeDetailsDTO.input>();
                List<StudentFeeDetailsDTO.input> fordateinfyp = new List<StudentFeeDetailsDTO.input>();

                //next due amount and date
                if (paymenthistorynew.Length > 0)
                {
                    long fyi_id_new = 0;
                    fyi_id_new = Convert.ToInt64(paymenthistorynew[0].FYP_Id);

                    temp_group_head = (from a in _dbf.Fee_Y_Payment_School_StudentDMO
                                       from b in _dbf.Fee_Payment
                                       from c in _dbf.FeeTransactionPaymentDMO
                                       from d in _dbf.FeeAmountEntryDMO

                                       where (a.AMST_Id == data.AMST_Id && a.FYP_Id == b.FYP_Id && a.FYP_Id == c.FYP_Id && c.FMA_Id == d.FMA_Id && a.FYP_Id <= fyi_id_new && b.MI_Id == d.MI_Id && d.MI_Id == data.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.ASMAY_Id == data.ASMAY_Id)
                                       select new StudentFeeDetailsDTO.input
                                       {
                                           FMG_Id = d.FMG_Id,
                                           FMH_Id = d.FMH_Id,
                                           FTI_Id = d.FTI_Id
                                       }).Distinct().ToList();
                    List<long> grp_ids = new List<long>();
                    List<long> head_ids = new List<long>();
                    List<long> inst_ids = new List<long>();
                    foreach (var item in temp_group_head)
                    {
                        grp_ids.Add(item.FMG_Id);
                        head_ids.Add(item.FMH_Id);
                        inst_ids.Add(item.FTI_Id);
                    }
                    fordate = (from d in _dbf.FeeStudentTransactionDMO
                               from f in _dbf.feeTDueDateRegularDMO
                               where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.AMST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && !inst_ids.Contains(d.FTI_Id))
                               select new StudentFeeDetailsDTO.input
                               {
                                   date = f.FTDD_Day,
                                   month = f.FTDD_Month,
                               }

                          ).Distinct().ToList();
                    fordateinfyp = (from d in _dbf.FeeStudentTransactionDMO
                                    from f in _dbf.feeTDueDateRegularDMO
                                    where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.AMST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                    select new StudentFeeDetailsDTO.input
                                    {
                                        month = f.FTDD_Month,
                                    }

                       ).Distinct().ToList();
                    List<int> months = new List<int>();
                    List<int> dates = new List<int>();
                    List<int> months1 = new List<int>();
                    List<int> months2 = new List<int>();

                    List<int> startperiod = new List<int>();

                    foreach (StudentFeeDetailsDTO.input item in fordate)
                    {
                        dates.Add(Convert.ToInt32(item.date));
                        months.Add(Convert.ToInt32(item.month));
                    }

                    foreach (StudentFeeDetailsDTO.input itemperiod in fordateinfyp)
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

                    ldt.nextdueamount = (from a in _dbf.FeeStudentTransactionDMO
                                         from b in _dbf.feeTDueDateRegularDMO
                                         where (b.FTDD_Day == data.date && b.FTDD_Month == data.month && a.FMA_Id == b.FMA_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && grp_ids.Contains(a.FMG_Id) && head_ids.Contains(a.FMH_Id) && !inst_ids.Contains(a.FTI_Id))
                                         select a.FSS_ToBePaid).Sum();
                }
                else
                {
                    temp_group_head = (from a in _dbf.FeeStudentTransactionDMO
                                       from b in _dbf.FeeAmountEntryDMO
                                       where (a.AMST_Id == data.AMST_Id && a.FMA_Id == b.FMA_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id)
                                       select new StudentFeeDetailsDTO.input
                                       {
                                           FMG_Id = b.FMG_Id,
                                           FMH_Id = b.FMH_Id,
                                           FTI_Id = b.FTI_Id
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
                    fordate = (from d in _dbf.FeeStudentTransactionDMO
                               from f in _dbf.feeTDueDateRegularDMO
                               where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.AMST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                               select new StudentFeeDetailsDTO.input
                               {
                                   date = f.FTDD_Day,
                                   month = f.FTDD_Month,
                               }

                          ).Distinct().ToList();
                    fordateinfyp = (from d in _dbf.FeeStudentTransactionDMO
                                    from f in _dbf.feeTDueDateRegularDMO
                                    where (d.FMA_Id == f.FMA_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.AMST_Id && grp_ids.Contains(d.FMG_Id) && head_ids.Contains(d.FMH_Id) && inst_ids.Contains(d.FTI_Id))
                                    select new StudentFeeDetailsDTO.input
                                    {
                                        month = f.FTDD_Month,
                                    }

                       ).Distinct().ToList();
                    List<int> months = new List<int>();
                    List<int> dates = new List<int>();
                    List<int> months1 = new List<int>();
                    List<int> months2 = new List<int>();

                    List<int> startperiod = new List<int>();

                    foreach (StudentFeeDetailsDTO.input item in fordate)
                    {
                        dates.Add(Convert.ToInt32(item.date));
                        months.Add(Convert.ToInt32(item.month));
                    }

                    foreach (StudentFeeDetailsDTO.input itemperiod in fordateinfyp)
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

                    ldt.nextdueamount = (from a in _dbf.FeeStudentTransactionDMO
                                         from b in _dbf.feeTDueDateRegularDMO
                                         where (b.FTDD_Day == data.date && b.FTDD_Month == data.month && a.FMA_Id == b.FMA_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && grp_ids.Contains(a.FMG_Id) && head_ids.Contains(a.FMH_Id) && inst_ids.Contains(a.FTI_Id))
                                         select a.FSS_ToBePaid).Sum();
                }





                if (ldt.nextdueamount > 0)
                {
                    ldt.nextduedate = data.date + "-" + data.month + "-" + data.year;
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }
        public async Task<StudentFeeTermDTO> StudentFeeTerm(StudentFeeTermDTO.input data)
        {
            StudentFeeTermDTO lt = new StudentFeeTermDTO();
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Mobile_Feeterm_Paymenthistory";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                 SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
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
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        lt.paymenthistory = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var customgrplist = (from a in _dbf.feegm
                                     from b in _dbf.feeGGG
                                     from c in _dbf.FeeStudentTransactionDMO
                                     where (a.FMGG_Id == b.FMGG_Id && b.FMG_Id == c.FMG_Id && a.MI_Id == data.MI_Id && a.FMGG_ActiveFlag == true && c.AMST_Id == data.AMST_Id && c.ASMAY_Id == data.ASMAY_Id)
                                     select new StudentFeeTermDTO.customgroup
                                     {
                                         FMGG_GroupName = a.FMGG_GroupName,
                                         FMGG_Id = a.FMGG_Id
                                     }).Distinct().OrderBy(t => t.FMGG_Id).ToArray();

                var feeconfig = _db.FeeMasterConfigurationDMO.FirstOrDefault(t => t.MI_Id == data.MI_Id).FMC_GroupOrTermFlg;

                List<StudentFeeTermDTO.customgroup> resultcustomgroup = new List<StudentFeeTermDTO.customgroup>();

                foreach (StudentFeeTermDTO.customgroup cgrp in customgrplist)
                {

                    using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "mobile_getcustomgeegroups";
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
                        SqlDbType.BigInt)
                        {
                            Value = data.AMST_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@FMGG_Id",
                         SqlDbType.BigInt)
                        {
                            Value = cgrp.FMGG_Id
                        });
                        cmd1.Parameters.Add(new SqlParameter("@config",
                       SqlDbType.VarChar)
                        {
                            Value = feeconfig
                        });
                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();
                        List<StudentFeeTermDTO.TermorGroup> result1 = new List<StudentFeeTermDTO.TermorGroup>();
                        try
                        {

                            using (var dataReader = await cmd1.ExecuteReaderAsync())
                            {

                                while (await dataReader.ReadAsync())
                                {
                                    result1.Add(new StudentFeeTermDTO.TermorGroup
                                    {
                                        FMT_Name = dataReader["FMT_Name"].ToString(),
                                        FSS_TotalToBePaid = Convert.ToInt64(dataReader["FSS_TotalToBePaid"]),
                                        FSS_ToBePaid = Convert.ToInt64(dataReader["FSS_ToBePaid"].ToString()),
                                        FSS_PaidAmount = Convert.ToInt64(dataReader["FSS_PaidAmount"].ToString()),
                                        FSS_ConcessionAmount = Convert.ToInt64(dataReader["FSS_ConcessionAmount"]),
                                        FSS_FineAmount = Convert.ToInt64(dataReader["FSS_FineAmount"])
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                        cgrp.feetermorgroup = result1.ToArray();

                        cgrp.C_FSS_TotalToBePaid = result1.ToArray().Sum(t => t.FSS_TotalToBePaid);
                        cgrp.C_FSS_ToBePaid = result1.ToArray().Sum(t => t.FSS_ToBePaid);
                        cgrp.C_FSS_PaidAmount = result1.ToArray().Sum(t => t.FSS_PaidAmount);
                        cgrp.C_FSS_ConcessionAmount = result1.ToArray().Sum(t => t.FSS_ConcessionAmount);
                        cgrp.C_FSS_FineAmount = result1.ToArray().Sum(t => t.FSS_FineAmount);


                        resultcustomgroup.Add(cgrp);
                    }

                    lt.customgroup_array = resultcustomgroup.ToArray();

                    lt.Total_FSS_TotalToBePaid = resultcustomgroup.ToArray().Sum(t => t.C_FSS_TotalToBePaid);
                    lt.Total_FSS_ToBePaid = resultcustomgroup.ToArray().Sum(t => t.C_FSS_ToBePaid);
                    lt.Total_FSS_PaidAmount = resultcustomgroup.ToArray().Sum(t => t.C_FSS_PaidAmount);
                    lt.Total_FSS_ConcessionAmount = resultcustomgroup.ToArray().Sum(t => t.C_FSS_ConcessionAmount);
                    lt.Total_FSS_FineAmount = resultcustomgroup.ToArray().Sum(t => t.C_FSS_FineAmount);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return lt;
        }


        public COEDTO CalenderofEvents(COEDTO.input data)
        {
            COEDTO ldt = new COEDTO();
            try
            {
                var list = new List<COEDTO.temp>();
                var mon = (from b in _coecontext.COE_EventsDMO
                           where (b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id)
                           select Convert.ToDateTime(b.COEE_EStartDate).ToString("MM/yyyy")).Distinct().ToArray();
                var mon1 = mon.Distinct().ToArray();
                foreach (var x in mon1)
                {
                    var monthid = Convert.ToDateTime(x).Month;
                    var yearid = Convert.ToDateTime(x).Year;
                    var eventlist = (from a in _coecontext.COE_Master_EventsDMO
                                     from b in _coecontext.COE_EventsDMO
                                     from c in _coecontext.AcademicYear
                                     where (a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == b.MI_Id && a.COEME_Id == b.COEME_Id && b.ASMAY_Id == c.ASMAY_Id && b.MI_Id == c.MI_Id && b.COEE_EStartDate.Value.Month == monthid)
                                     select new COEDTO.eventl
                                     {

                                         //ASMAY_Year = c.ASMAY_Year,
                                         //COEE_Id = b.COEE_Id,
                                         //COEME_Id = a.COEME_Id,
                                         COEME_EventName = a.COEME_EventName,
                                         COEE_EStartDate = Convert.ToDateTime(b.COEE_EStartDate).ToString("dd/MM/yyyy"),
                                         COEE_EEndDate = Convert.ToDateTime(b.COEE_EEndDate).ToString("dd/MM/yyyy"),
                                         //COEE_ActiveFlag = b.COEE_ActiveFlag,
                                     }
                                      ).ToArray();
                    list.Add(new COEDTO.temp { month_id = monthid, year = yearid, event_list = eventlist });
                }



                //for (int i = 1; i < 12; i++)
                //{

                //}

                // List<Array> monthlst = new List<Array>();
                //  foreach(var x in ldt.eventlist)
                //   {

                //   }
                ldt.monthlist = list.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }
        public ExamDTO.examid Examid(ExamDTO.input data)
        {
            ExamDTO.examid ldt = new ExamDTO.examid();
            try
            {
                var catid1 = (from a in _examcontext.School_Adm_Y_Student
                              from b in _examcontext.Exm_Category_ClassDMO
                              where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id)
                              select new ExamDTO.temp1
                              {
                                  EMCA_Id = b.EMCA_Id
                              }).ToArray();
                List<long> catid = new List<long>();

                foreach (var item in catid1)
                {
                    catid.Add(item.EMCA_Id);
                }

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                var emeid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)).Select(t => t.EME_Id).ToArray();
                ldt.examlist = (from t in _examcontext.exammasterDMO
                                where (t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeid.Contains(t.EME_Id))
                                select new ExamDTO.temp2
                                {
                                    EME_Id = t.EME_Id,
                                    EME_ExamName = t.EME_ExamName
                                }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }
        public ExamDTO Examdetails(ExamDTO.input data)
        {
            ExamDTO ldt = new ExamDTO();
            try
            {
                var catid1 = (from a in _examcontext.School_Adm_Y_Student
                              from b in _examcontext.Exm_Category_ClassDMO
                              where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && b.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id)
                              select new ExamDTO.temp1
                              {
                                  EMCA_Id = b.EMCA_Id
                              }).ToArray();
                List<long> catid = new List<long>();

                foreach (var item in catid1)
                {
                    catid.Add(item.EMCA_Id);
                }

                var eycid = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                var eyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id)).Select(t => t.EYCE_Id).ToArray();

                var subid = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).Select(t => t.ISMS_Id).ToArray();

                var subMorGFlag = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => eyceid.Contains(t.EYCE_Id)).ToArray();

                ldt.subMorGFlag = subMorGFlag[0].EYCES_MarksGradeEntryFlg;

                var EMGR_Id = subMorGFlag[0].EMGR_Id;

                if (ldt.subMorGFlag == "G")
                {
                    ldt.gradname = (from a in _examcontext.Exm_Master_GradeDMO
                                    from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                    where (a.MI_Id == data.MI_Id && a.EMGR_Id == EMGR_Id && b.EMGR_Id == EMGR_Id)
                                    select new ExamDTO.temp3
                                    {
                                        grade = b.EMGD_Name
                                    }).Select(b => b.grade).ToArray();
                }
                ldt.Marklist = (from a in _examcontext.Adm_M_Student
                                from b in _examcontext.ExamMarksDMO
                                from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                from d in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                where (a.AMST_Id == b.AMST_Id && b.ISMS_Id == c.ISMS_Id && c.ISMS_Id == d.ISMS_Id && a.AMST_ActiveFlag == 1
                                && a.AMST_SOL == "S" && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1 && subid.Contains(c.ISMS_Id)
                                && eyceid.Contains(d.EYCE_Id) && b.EME_Id == data.EME_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id
                                && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                select new ExamDTO.ExamMarks
                                {
                                    SubjectName = c.ISMS_SubjectName,
                                    TotalMarks = d.EYCES_MaxMarks,
                                    MinMarks = d.EYCES_MinMarks,
                                    obtainmarks = b.ESTM_Marks
                                }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }

        public PaymentDetails generatehashsequence(OnlinePaymentDTO.input enq)
        {
            Payment pay = new Payment(_db);
            // ProspectusDTO data = new ProspectusDTO();
            int autoinc = 1, totpayableamount = 0;


            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
            try
            {
                string ids = enq.ftiidss;

                using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "calculatefine_OnlinePayment1";
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
                    SqlDbType.BigInt)
                    {
                        Value = enq.AMST_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@fmt_id",
                     SqlDbType.VarChar)
                    {
                        Value = ids
                    });

                    cmd1.Parameters.Add(new SqlParameter("@fmgid",
                  SqlDbType.VarChar)
                    {
                        Value = enq.grpidss
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
                                    merchantId = dataReader["FPGD_MerchantId"].ToString(),
                                    value = dataReader["balance"].ToString(),
                                    name = "splitId" + autoinc.ToString(),
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


            //string list_values = "";
            //for (int q=0;q<enq.disableterms.Length;q++)
            //{
            //    list_values= list_values
            //}


            List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
            paymentdetails = _dbf.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();

            //List<FeePaymentDetailsDMO> receipt = new List<FeePaymentDetailsDMO>();
            var receipt = _dbf.FeePaymentDetailsDMO.Where(t => t.MI_Id == enq.MI_Id).Max(t => t.FYP_Receipt_No);

            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
            //PaymentDetailsDto.amount = totamount.ToString();


            Master_NumberingDTO transnumbconfigurationsettingsss = new Master_NumberingDTO();
            List<Master_Numbering> masnum = new List<Master_Numbering>();
            masnum = _db.Master_Numbering.Where(t => t.MI_Id == enq.MI_Id && t.IMN_Flag == "online").OrderByDescending(t => t.IMN_Id).ToList();
            // Master_NumberingDTO transnumbconfigurationsettingsss = Mapper.Map<Master_Numbering>(masnum);
            foreach (var a in masnum)
            {

                transnumbconfigurationsettingsss.IMN_AutoManualFlag = a.IMN_AutoManualFlag;
                transnumbconfigurationsettingsss.IMN_DuplicatesFlag = a.IMN_DuplicatesFlag;
                transnumbconfigurationsettingsss.IMN_Flag = a.IMN_Flag;
                transnumbconfigurationsettingsss.IMN_Id = a.IMN_Id;
                transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = a.IMN_PrefixAcadYearCode;
                transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = a.IMN_PrefixCalYearCode;
                transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = a.IMN_PrefixFinYearCode;
                transnumbconfigurationsettingsss.IMN_PrefixParticular = a.IMN_PrefixParticular;
                transnumbconfigurationsettingsss.IMN_RestartNumFlag = a.IMN_RestartNumFlag;
                transnumbconfigurationsettingsss.IMN_StartingNo = a.IMN_StartingNo;
                transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = a.IMN_SuffixAcadYearCode;
                transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = a.IMN_SuffixCalYearCode;
                transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = a.IMN_SuffixFinYearCode;
                transnumbconfigurationsettingsss.IMN_SuffixParticular = a.IMN_SuffixParticular;
                transnumbconfigurationsettingsss.IMN_WidthNumeric = a.IMN_WidthNumeric;
                transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = a.IMN_ZeroPrefixFlag;
            }

            if (transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
            {
                GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                transnumbconfigurationsettingsss.MI_Id = enq.MI_Id;
                transnumbconfigurationsettingsss.ASMAY_Id = enq.ASMAY_Id;
                PaymentDetailsDto.trans_id = a.GenerateNumber(transnumbconfigurationsettingsss);
            }

            PaymentDetailsDto.trans_id = PaymentDetailsDto.trans_id.ToString();

            PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

            //PaymentDetailsDto.productinfo = "FeeOnlinePayment";

            //Radha
            //string payinfo = "{\\\"Employee\\\":[";

            //foreach(FeeSlplitOnlinePayment x in result)
            //{
            //    payinfo = payinfo + "{\\\"name\\\":\\" + x.name + "\\\",\\\"merchantId\\\":\\" + x.merchantId + "\\\",\\\"value\\\":\\" + x.value + "\\\",\\\\\"commission\\\":\\" + x.commission + "\\\",\\\"description\\\":\\" + x.description + "}\\\",";

            //    totpayableamount = totpayableamount + Convert.ToInt32(x.value);
            //}
            //payinfo = payinfo.Substring(0, (payinfo.Length - 1));
            //payinfo += "]}";
            //Radha


            foreach (FeeSlplitOnlinePayment x in result)
            {
                totpayableamount = totpayableamount + Convert.ToInt32(x.value);
            }

            var item = new
            {
                FeeOninePaymentDoubleIntegration = result
            };
            // PaymentDetailsDto.productinfoObj = item;
            //PaymentDetailsDto.productinfoObj = result.ToArray();
            //PaymentDetailsDto.productinfoObj = (FeeSlplitOnlinePayment)result.ToArray().GetValue(0);
            PaymentDetailsDto.productinfoObj = result.ToArray();
            string payinfo = JsonConvert.SerializeObject(item);

            //  string payinfo = "{\"Employee\": [{\"name\":\"splitId1\",\"merchantId\":\"4510911\",\"value\":\"10\",\"commission\":\"5\",\"description\":\"splitId1 summary\"},{\"name\":\"splitId2\",\"merchantId\":\"4510912\",\"value\":\"10\",\"commission\":\"5\",\"description\":\"splitId2 summary\"}]}";

            PaymentDetailsDto.productinfo = payinfo;
            PaymentDetailsDto.amount = totpayableamount;
            //PaymentDetailsDto.amount =Convert.ToString(enq.amount);
            var fillstudent = (from a in _dbf.AdmissionStudentDMO
                               from b in _dbf.School_Adm_Y_StudentDMO
                               from c in _dbf.admissioncls
                               from d in _dbf.school_M_Section
                               where (c.ASMCL_Id == b.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == enq.MI_Id && a.AMST_Id == enq.AMST_Id && b.ASMAY_Id == enq.ASMAY_Id)
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
              ).ToList();
            foreach (var a in fillstudent)
            {
                PaymentDetailsDto.firstname = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName;
                PaymentDetailsDto.email = a.amst_email_id;
                PaymentDetailsDto.phone = a.amst_mobile;
                PaymentDetailsDto.udf5 = a.ASMCL_ID.ToString();
            }
            PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT;
            PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
            PaymentDetailsDto.udf1 = "Rs.";
            PaymentDetailsDto.udf2 = Convert.ToString(enq.AMST_Id);
            PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
            //PaymentDetailsDto.udf4 = enq.ASMAY_Id.ToString();
            //PaymentDetailsDto.udf4 = enq.fmt_id.ToString();
            PaymentDetailsDto.udf4 = enq.grpidss.ToString();


            PaymentDetailsDto.udf6 = enq.ASMAY_Id.ToString();
            // PaymentDetailsDto.udf6 = Convert.ToString(enq.fmt_id);
            //  PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/" + PaymentDetailsDto.productinfo + "/paymentresponse/";
            PaymentDetailsDto.transaction_response_url = "http://stagingcampusux.azurewebsites.net/api/" + PaymentDetailsDto.productinfo + "/paymentresponse/";

            PaymentDetailsDto.status = "success";
            PaymentDetailsDto.service_provider = "payu_paisa";

            PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

            //var confirmstatus = _FeeGroupHeadContext.Database.ExecuteSqlCommand("Insert_fee_tables_Online_Full_payment @p0,@p1,@p2,@p3,@p4", enq.MI_Id, enq.ftiidss,enq.Amst_Id, totpayableamount, PaymentDetailsDto.trans_id);

            FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
            feepaydet.MI_Id = enq.MI_Id;
            feepaydet.ASMAY_ID = enq.ASMAY_Id;
            //feepaydet.FYP_Currency = 1;
            //feepaydet.FYP_DOE = DateTime.Now;
            //feepaydet.FYP_ReceiptDate = DateTime.Now;
            //feepaydet.FYP_ReceiptNo = "";
            //feepaydet.FYP_PayModeType = "Single";
            //feepaydet.FYP_TransactionTypeFlag = "O";
            //feepaydet.FYP_BankName = "";
            //feepaydet.FYP_DDChequeNo = 0;
            //feepaydet.FYP_DDChequeDate = DateTime.Now;
            //feepaydet.FYP_TotalPaidAmount = totpayableamount;
            //feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
            //feepaydet.FYP_ChallanNo = "0";
            //feepaydet.FYP_Transaction_Id = PaymentDetailsDto.trans_id;
            //feepaydet.FYP_PaymentReference_Id = 0;
            //feepaydet.FYP_TotalFineAmount = 0;
            //feepaydet.FYP_TotalRebateAmount = 0;
            //feepaydet.FYP_Remarks = "Online Payment";
            //feepaydet.FYP_ChequeBounceFlag = "CL";
            //feepaydet.FYP_ActiveFlag = true;
            //feepaydet.CreatedDate = DateTime.Now;
            //feepaydet.UpdatedDate = DateTime.Now;
            //feepaydet.user_id = enq.userId;

            //_dbf.FeePaymentDetailsDMO.Add(feepaydet);
            //_dbf.SaveChanges();
            feepaydet.FTCU_Id = 1;
            feepaydet.FYP_Receipt_No = PaymentDetailsDto.trans_id;
            feepaydet.FYP_Bank_Name = "";
            feepaydet.FYP_Bank_Or_Cash = "O";
            feepaydet.FYP_DD_Cheque_No = "";
            feepaydet.FYP_DD_Cheque_Date = DateTime.Now;
            feepaydet.FYP_Date = DateTime.Now;
            feepaydet.FYP_Tot_Amount = totpayableamount;
            feepaydet.FYP_Tot_Waived_Amt = 0;
            feepaydet.FYP_Tot_Fine_Amt = 0;
            feepaydet.FYP_Tot_Concession_Amt = 0;
            feepaydet.FYP_Remarks = "Online Payment";
            feepaydet.FYP_Chq_Bounce = "CL";
            feepaydet.DOE = DateTime.Now;
            feepaydet.CreatedDate = DateTime.Now;
            feepaydet.UpdatedDate = DateTime.Now;
            feepaydet.user_id = enq.userId;
            feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
            feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
            feepaydet.FYP_PaymentReference_Id = "";

            _dbf.FeePaymentDetailsDMO.Add(feepaydet);
            _dbf.SaveChanges();
            return PaymentDetailsDto;

        }
        public TTDTO Timetable(TTDTO.input data)
        {
            TTDTO ldt = new TTDTO();
            try
            {
                var daynames = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();
                var Category = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMC_ActiveFlag == true).Select(s => s.TTMC_CategoryName).FirstOrDefault();
                var calss = _ttcontext.School_M_Class.Where(t => t.MI_Id.Equals(data.MI_Id) && t.ASMCL_Id == data.ASMCL_Id).Select(s => s.ASMCL_ClassName).FirstOrDefault();
                var section = _ttcontext.School_M_Section.Where(t => t.MI_Id.Equals(data.MI_Id) && t.ASMS_Id == data.ASMS_Id).Select(s => s.ASMC_SectionName).FirstOrDefault();

                var list1 = new List<TTDTO.temp>();
                var list = new List<TTDTO.events_list>();

                list1.Add(new TTDTO.temp { CategoryName = Category, ClassName = calss, SectionName = section });

                ldt.TT_common = list1.ToArray();

                for (int y = 0; y < daynames.Count(); y++)
                {

                    var dddd = (from a in _ttcontext.TT_Master_DayDMO
                                from b in _ttcontext.TT_Master_PeriodDMO
                                from c in _ttcontext.School_M_Class
                                from d in _ttcontext.School_M_Section
                                from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                from f in _ttcontext.HR_Master_Employee_DMO
                                from g in _ttcontext.TT_Final_GenerationDMO
                                from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                from ii in _ttcontext.TTMasterCategoryDMO
                                where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && ii.MI_Id == g.MI_Id && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.ASMCL_Id == data.ASMCL_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMS_Id == data.ASMS_Id && g.ASMAY_Id == data.ASMAY_Id && h.TTMD_Id == daynames[y].TTMD_Id)
                                select new TTDTO.eventl
                                {
                                    PeriodName = b.TTMP_PeriodName,
                                    staffName = f.HRME_EmployeeFirstName,
                                    SubjectName = e.ISMS_SubjectName,

                                }
                                   ).Distinct().ToArray();

                    list.Add(new TTDTO.events_list { DayName = daynames[y].TTMD_DayName, TTData = dddd });

                }
                ldt.TimeTable = list.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }

        //Deepak for Employee portal

        public EmpPortalDTO EmployeePortal_SalaryD(EmpPortalDTO.Input data)
        {
            EmpPortalDTO ldt = new EmpPortalDTO();
            try
            {
                //var ylist = _hrmscontext.HR_MasterLeaveYear.Single(t => t.MI_Id == data.MI_Id);
                //var mlist=_db.month.Single(t => t.MI_Id == data.MI_Id);

                //var HRME_Id = _hrmscontext.MasterEmployee.Single(c => c.HRME_Id == data.HRME_ID && c.MI_Id == data.MI_Id);

                //var salarylist = (from m in _hrmscontext.HR_Employee_Salary
                //                  from n in _hrmscontext.HR_Employee_Salary_Details
                //                  from o in _hrmscontext.HR_Master_EarningsDeductions
                //                  where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_ID && o.HRMED_EarnDedFlag.Equals("Earning") && m.HRES_Year == data.hres_year
                //                  group new { m, n, o }
                //                    by new { m.HRES_Month } into g
                //                  select new EmpPortalDTO.Output 
                //                  {
                //                      salary = g.Sum(d => d.n.HRESD_Amount),
                //                      monthName = g.FirstOrDefault().m.HRES_Month,
                //                      hres_id = g.FirstOrDefault().m.HRES_Id
                //                  }
                //                ).ToArray();


                //var EQuery = _hrmscontext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == data.MI_Id && t.HRMED_EarnDedFlag.Equals("Earning")).Select(d => d.HRMED_Id).ToList();
                //var TotalEarning = (from m in _hrmscontext.HR_Employee_Salary
                //                    from n in _hrmscontext.HR_Employee_Salary_Details

                //                    where m.HRES_Id == n.HRES_Id && EQuery.Contains(n.HRMED_Id) && m.MI_Id == data.MI_Id 
                //                    group new { m, n }
                //                     by new { m.HRES_Month } into g
                //                    select new EmpPortalDTO.Output
                //                    {
                //                        salary = g.Sum(d => d.n.HRESD_Amount),

                //                    }
                //                 ).ToArray();
                //var DQuery = _hrmscontext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == data.MI_Id && t.HRMED_EarnDedFlag.Equals("Deduction")).Select(d => d.HRMED_Id).ToList();
                //var totalDeduction = (from m in _hrmscontext.HR_Employee_Salary
                //                      from n in _hrmscontext.HR_Employee_Salary_Details

                //                      where m.HRES_Id == n.HRES_Id && DQuery.Contains(n.HRMED_Id) && m.MI_Id == data.MI_Id 
                //                      group new { m, n }
                //                       by new { m.HRES_Month } into g
                //                      select new EmpPortalDTO.Output
                //                      {
                //                          salary = g.Sum(d => d.n.HRESD_Amount),

                //                      }
                //                 ).ToArray();
                //var salarylistE = (from m in _hrmscontext.HR_Employee_Salary
                //                   from n in _hrmscontext.HR_Employee_Salary_Details
                //                   from o in _hrmscontext.HR_Master_EarningsDeductions
                //                   where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == data.MI_Id   && o.HRMED_EarnDedFlag.Equals("Earning")

                //                   select new EmpPortalDTO.Output_E_D
                //                   {

                //                       hrmed_Name = o.HRMED_Name,
                //                       hrmed_Amount = n.HRESD_Amount,
                //                   }
                //                 ).ToArray();
                //var salarylistD = (from m in _hrmscontext.HR_Employee_Salary
                //                   from n in _hrmscontext.HR_Employee_Salary_Details
                //                   from o in _hrmscontext.HR_Master_EarningsDeductions
                //                   where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == data.MI_Id  && o.HRMED_EarnDedFlag.Equals("Deduction")

                //                   select new EmpPortalDTO.Output_E_D
                //                   {

                //                       hrmed_Name = o.HRMED_Name,
                //                       hrmed_Amount = n.HRESD_Amount,
                //                   }
                //                ).ToArray();




                //var list = new List<EmpPortalDTO.Output>();


                //list.Add(new EmpPortalDTO.Output { HRMLY_LeaveYear = Convert.ToString(ylist), salarylist = salarylist, TotalEarning= TotalEarning, totalDeduction= totalDeduction, salarylistE= salarylistE, salarylistD= salarylistD });





                //ldt.EmployeePortal_SalaryD = list.ToArray();






                //List<HR_Employee_SalaryDTO> main_list = new List<HR_Employee_SalaryDTO>();
                //for (int i = 0; i < data.HRME_Id[i].Count(); i++)
                //{
                //    HR_Employee_SalaryDTO obj = new HR_Employee_SalaryDTO();
                //    obj.HRME_Id = data.HRME_Id[i];
                //    obj.MI_Id = data.MI_Id;
                //    obj.HRES_Year = data.HRES_Year;
                //    obj.HRES_Month = data.HRES_Month;

                //    Institution institute = new Institution();
                //    institute = _db.Institution.FirstOrDefault(t => t.MI_Id.Equals(data.MI_Id));

                //    InstitutionDTO dmoObj = Mapper.Map<InstitutionDTO>(institute);
                //    obj.institutionDetails = dmoObj;

                //    MasterEmployee employe = _hrmscontext.MasterEmployee.FirstOrDefault(t => t.MI_Id.Equals(data.MI_Id) && t.HRME_Id.Equals(obj.HRME_Id));

                //    var DepartmentName = _hrmscontext.HR_Master_Department.FirstOrDefault(t => t.MI_Id.Equals(data.MI_Id) && t.HRMD_Id.Equals(employe.HRMD_Id)).HRMD_DepartmentName;
                //    var DesignationName = _hrmscontext.HR_Master_Designation.FirstOrDefault(t => t.MI_Id.Equals(data.MI_Id) && t.HRMDES_Id.Equals(employe.HRMDES_Id)).HRMDES_DesignationName;

                //    Employee Basic Details
                //    MasterEmployeeDTO employeObj = Mapper.Map<MasterEmployeeDTO>(employe);
                //    obj.currentemployeeDetails = employeObj;

                //    obj.DesignationName = DesignationName;
                //    obj.DepartmentName = DepartmentName;


                //    Configuration details
                //    HR_Configuration PayrollStandard = _hrmscontext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(data.MI_Id));
                //    HR_ConfigurationDTO HR_ConfigurationDTO = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                //    obj.PayrollStandard = HR_ConfigurationDTO;

                //    Employee Earning / Deduction heads

                //     obj = await getEmployeeSalarySlip(obj);


                //    Double? Lopdays = 0;
                //    decimal LopAmount = 0;
                //    LOP Calculation

                //    var employeSalary = _hrmscontext.HR_Employee_Salary.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRES_Year.Equals(data.HRES_Year) && t.HRES_Month.Equals(data.HRES_Month) && t.HRME_Id == obj.HRME_Id).FirstOrDefault();
                //    if (employeSalary != null)
                //    {
                //        HR_Employee_SalaryDTO employeSalObj = Mapper.Map<HR_Employee_SalaryDTO>(employeSalary);

                //        obj.empsaldetail = employeSalObj;

                //        var LOPcal = (from A in _hrmscontext.HR_Emp_Leave_Trans
                //                      from B in _hrmscontext.HR_Master_Leave
                //                      where (B.HRML_Id == A.HRELT_LeaveId &&
                //                      A.MI_Id.Equals(data.MI_Id) && A.HRME_Id == obj.HRME_Id &&
                //                      B.HRML_LeaveType.Equals("LWP") && A.HRELT_ActiveFlag == true
                //                    && ((A.HRELT_FromDate >= employeSalary.HRES_FromDate && A.HRELT_FromDate <= employeSalary.HRES_ToDate)
                //                        || (A.HRELT_ToDate >= employeSalary.HRES_FromDate && A.HRELT_ToDate <= employeSalary.HRES_ToDate))
                //                      )
                //                      select A
                //                   ).ToList();
                //        if (LOPcal.Count() > 0)
                //        {
                //            Lopdays = LOPcal.Sum(t => t.HRELT_TotDays);

                //            LopAmount = Convert.ToDecimal(Lopdays) * Convert.ToDecimal(employeSalary.HRES_DailyRates);
                //        }

                //        obj.empsaldetail.Lopdays = Lopdays;
                //        obj.empsaldetail.LopAmount = LopAmount;

                //        Leave Details

                //        var LeayearId = _hrmscontext.HR_MasterLeaveYear.Where(t => t.MI_Id.Equals(data.MI_Id) && t.HRMLY_LeaveYear.Equals(data.HRES_Year)).FirstOrDefault().HRMLY_Id;

                //        if (LeayearId > 0)
                //        {
                //            var LeaveDetails = (from A in _hrmscontext.HR_Emp_Leave_StatusDMO
                //                                from B in _hrmscontext.HR_Master_Leave
                //                                where (B.HRML_Id == A.HRML_Id &&
                //                                A.MI_Id.Equals(data.MI_Id) && A.HRME_Id == data.HRME_Id &&
                //                                A.HRMLY_Id == LeayearId)
                //                                select new HR_Emp_Leave_StatusDTO
                //                                {
                //                                    HRELS_Id = A.HRELS_Id,
                //                                    HRML_LeaveName = B.HRML_LeaveName,
                //                                    HRELS_TotalLeaves = A.HRELS_TotalLeaves,
                //                                    HRELS_TransLeaves = A.HRELS_TransLeaves,
                //                                    HRELS_CBLeaves = A.HRELS_CBLeaves

                //                                }).ToList();

                //            obj.employeeLeaveDetails = LeaveDetails.ToArray();
                //        }



                //    }
                //    main_list.Add(obj);
                //    //---To Delete The Generated File Start ---

                //    /////////
                //    ////var Empdetails = _HRMSContext.MasterEmployee.Where(t => t.HRME_Id == obj.HRME_Id && t.HRME_ActiveFlag == true).ToList();
                //    ////string slaryslip = "Salary Slip Of Employee " + Empdetails.FirstOrDefault().HRME_EmployeeCode + ".pdf";
                //    ////string GetDownloadFolderPath =
                //    ////      Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();

                //    ////IFileProvider provider = new PhysicalFileProvider(GetDownloadFolderPath);
                //    ////IFileInfo fileInfo = provider.GetFileInfo(slaryslip);

                //    ////bool fileexists = File.Exists(fileInfo.PhysicalPath);
                //    ////if (fileexists == true)
                //    ////{
                //    ////    File.Delete(fileInfo.PhysicalPath);
                //    ////}
                //    ---To Delete The Generated File Start-- -
                //    /////////////////////////

                //}
                //data.main_list = main_list.ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }

        public async Task<HR_Employee_SalaryDTO> getEmployeeSalarySlip(HR_Employee_SalaryDTO dto)
        {

            try
            {
                using (var cmd = _hrmscontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmployeeSalarySlipGeneration";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMLY_LeaveYear",
                  SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.HRES_Year)
                    });

                    cmd.Parameters.Add(new SqlParameter("@IVRM_Month_Name",
                  SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(dto.HRES_Month)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        }
                        dto.employeeSalaryslipDetails = retObject.ToArray();


                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }


            return dto;
        }

        public EmployeePunchDTO EmployeePortal_PunchD(EmployeePunchDTO.Input data)
        {
            EmployeePunchDTO ldt = new EmployeePunchDTO();
            try
            {
                //data.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == data.userid && c.MI_Id == data.MI_Id).Emp_Code;


                var filldepartment = (from a in _FOContext.HR_Master_Employee_DMO
                                      from b in _FOContext.HR_Master_Department_DMO
                                      from c in _FOContext.HR_Master_Designation_DMO
                                      where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true
                                          && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id)
                                      select new EmployeePunchDTO.Output
                                      {
                                          empFname = a.HRME_EmployeeFirstName,
                                          empMname = a.HRME_EmployeeMiddleName,
                                          empLname = a.HRME_EmployeeLastName,
                                          HRME_DOJ = a.HRME_DOJ,
                                          HRMD_DepartmentName = b.HRMD_DepartmentName,
                                          HRMDES_DesignationName = c.HRMDES_DesignationName,

                                      }
                   ).Distinct().ToArray();

                var Emp_punchDetails = (from a in _FOContext.FO_Emp_Punch
                                        from b in _FOContext.FO_Emp_Punch_Details
                                        where (a.FOEP_Id == b.FOEP_Id && b.FOEPD_Flag == "1" && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_Id && (a.FOEP_PunchDate >= data.fromdate && a.FOEP_PunchDate <= data.todate))
                                        select new EmployeePunchDTO.OutputD
                                        {
                                            punchdate = a.FOEP_PunchDate,
                                            punchtime = b.FOEPD_PunchTime,
                                            InOutFlg = b.FOEPD_InOutFlg,


                                        }
                ).Distinct().ToArray();


                var list = new List<EmployeePunchDTO.Output>();


                list.Add(new EmployeePunchDTO.Output { filldepartment = filldepartment, Emp_punchDetails = Emp_punchDetails });





                ldt.EmployeePortal_PunchD = list.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }

        public EmployeePortal_StudentAttrndenceDTO EmployeePortal_StudentAttrndence(EmployeePortal_StudentAttrndenceDTO.Input data)
        {
            EmployeePortal_StudentAttrndenceDTO.OutputAllStudent[] allstudent = null;
            EmployeePortal_StudentAttrndenceDTO.Output[] attendencelist = null;

            EmployeePortal_StudentAttrndenceDTO ldt = new EmployeePortal_StudentAttrndenceDTO();
            try
            {

                var studentList = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   from c in _db.Adm_M_Student
                                   from d in _db.Section
                                   where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMS_Id == data.ASMS_Id)
                                   select new EmployeePortal_StudentAttrndenceDTO.Output_StudentList
                                   {
                                       AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                       AMST_Id = a.AMST_Id
                                   }).Distinct().OrderBy(t => t.AMST_Id).ToArray();
                var classlist = (from a in _db.School_Adm_Y_StudentDMO
                                 from b in _db.admissioncls
                                 where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)
                                 select new EmployeePortal_StudentAttrndenceDTO.Output_ClassList
                                 {
                                     classname = b.ASMCL_ClassName,
                                     ASMCL_Id = b.ASMCL_Id
                                 }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                var SectionList = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   from c in _db.Adm_M_Student
                                   from d in _db.Section
                                   where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == a.ASMS_Id)
                                   select new EmployeePortal_StudentAttrndenceDTO.Output_SectionList
                                   {
                                       sectionname = d.ASMC_SectionName,
                                       ASMS_Id = d.ASMS_Id
                                   }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

                List<MasterAcademic> listtemp = new List<MasterAcademic>();
                listtemp = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id).ToList();
                var yearlist = listtemp.ToArray();

                var fillmonths = _ChairmanDashboardContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToArray();

                List<EmployeePortal_StudentAttrndenceDTO.OutputAllStudent> result2 = new List<EmployeePortal_StudentAttrndenceDTO.OutputAllStudent>();
                List<EmployeePortal_StudentAttrndenceDTO.Output> result1 = new List<EmployeePortal_StudentAttrndenceDTO.Output>();
                if (data.type == 2)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_STUDENT_MONTHLY_ATTENDANCE";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@amst_id",
                        SqlDbType.BigInt)
                        {
                            Value = data.AMST_Id
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
                                    result1.Add(new EmployeePortal_StudentAttrndenceDTO.Output
                                    {
                                        monthName = dataReader["MONTH_NAME"].ToString(),
                                        present = Convert.ToDecimal(dataReader["TOTAL_PRESENT"].ToString()),
                                        classheld = Convert.ToDecimal(dataReader["CLASS_HELD"].ToString()),

                                        perc = (Convert.ToDecimal(dataReader["TOTAL_PRESENT"].ToString()) / Convert.ToDecimal(dataReader["CLASS_HELD"].ToString())) * 100


                                    });

                                }
                                attendencelist = result1.ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                if (data.type == 1)
                {


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ALLSTUDENT_MONTHLY_ATTENDANCE_PORTAL";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@asmay_id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.ASMS_Id
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
                                    result2.Add(new EmployeePortal_StudentAttrndenceDTO.OutputAllStudent
                                    {
                                        AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                        studentname = dataReader["name"].ToString(),
                                        monthid = Convert.ToInt64(dataReader["month_id"].ToString()),
                                        monthName = dataReader["MONTH_NAME"].ToString(),
                                        present = Convert.ToDecimal(dataReader["TOTAL_PRESENT"].ToString()),
                                        classheld = Convert.ToDecimal(dataReader["CLASS_HELD"].ToString()),
                                        perc = Convert.ToDecimal(dataReader["per"].ToString())


                                    });

                                }
                                allstudent = result2.ToArray();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }


                var list = new List<EmployeePortal_StudentAttrndenceDTO.Output>();


                list.Add(new EmployeePortal_StudentAttrndenceDTO.Output { attendencelist = attendencelist, allstudent = allstudent, fillmonths = fillmonths, studentList = studentList, classlist = classlist, sectionList = SectionList, yearlist = yearlist });

                ldt.EmployeePortal_StudentAttrndence = list.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }

        public EmployeePortalTimeTableDTO EmployeePortalTimeTableD(EmployeePortalTimeTableDTO.Input dto)
        {
            EmployeePortalTimeTableDTO ldt = new EmployeePortalTimeTableDTO();
            try
            {
                // var HRME_Id = _examcontext.Staff_User_Login.Single(c => c.Id == dto.userid && c.MI_Id == dto.MI_Id).Emp_Code;

                var TT_final_generation = (from a in _ttcontext.TT_Final_GenerationDMO
                                           from b in _ttcontext.TT_Final_Generation_DetailedDMO
                                           from c in _ttcontext.School_M_Class
                                           from d in _ttcontext.School_M_Section
                                           from e in _ttcontext.TT_Master_PeriodDMO
                                           from f in _ttcontext.TT_Master_DayDMO
                                           where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id && a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                           select new
                                           {
                                               DayName = f.TTMD_DayName,
                                               PeriodCount = e.TTMP_PeriodName.Count()


                                           }).Distinct().GroupBy(f => f.DayName).Select(g => new EmployeePortalTimeTableDTO.Output_TTGenration { DayName = g.Key, PeriodCount = g.Count() }).ToArray();



                // var allperiods = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag == true && c.MI_Id == dto.MI_Id).ToArray();

                var allperiods = (from a in _ttcontext.TT_Master_DayDMO
                                  where (a.TTMD_ActiveFlag == true && a.MI_Id == dto.MI_Id)
                                  select new EmployeePortalTimeTableDTO.OutputAllPeriods
                                  {
                                      TTMD_Id = a.TTMD_Id,
                                      TTMD_DayName = a.TTMD_DayName
                                  }).Distinct().ToArray();

                //var periods = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(dto.MI_Id)).ToArray();

                var periods = (from a in _ttcontext.TT_Master_PeriodDMO
                               where (a.TTMP_ActiveFlag == true && a.MI_Id == dto.MI_Id)
                               select new EmployeePortalTimeTableDTO.OutputPeriods
                               {
                                   TTMP_Id = a.TTMP_Id,
                                   TTMP_PeriodName = a.TTMP_PeriodName
                               }).Distinct().ToArray();


                var class_sectons = (from a in _ttcontext.TT_Final_GenerationDMO
                                     from b in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from c in _ttcontext.School_M_Class
                                     from d in _ttcontext.School_M_Section
                                     from e in _ttcontext.TT_Master_PeriodDMO
                                     from f in _ttcontext.TT_Master_DayDMO
                                     where (a.TTFG_Id == b.TTFG_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && a.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                     select new EmployeePortalTimeTableDTO.OutputClass_section
                                     {
                                         P_Days = f.TTMD_DayName,
                                         Period = e.TTMP_PeriodName,
                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                         ASMC_SectionName = d.ASMC_SectionName

                                     }).Distinct().ToArray();


                var list = new List<EmployeePortalTimeTableDTO.Output>();


                list.Add(new EmployeePortalTimeTableDTO.Output { TT_final_generation = TT_final_generation, class_sectons = class_sectons, allperiods = allperiods, periods = periods });





                ldt.EmployeePortalTimeTableD = list.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }

        public EmployeePortalStudentReportCardDTO EmployeePortalStudentReportCard(EmployeePortalStudentReportCardDTO.Input data)
        {
            EmployeePortalStudentReportCardDTO ldt = new EmployeePortalStudentReportCardDTO();
            try
            {
                var clstchname = (from a in _examcontext.ClassTeacherMappingDMO
                                  from b in _examcontext.HR_Master_Employee_DMO
                                  where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                  select new EmployeePortalStudentReportCardDTO.OutputClassTeacherName
                                  {
                                      HRME_Id = a.HRME_Id,
                                      HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim()
                                  }).Distinct().ToArray();

                var classlist = (from a in _db.School_Adm_Y_StudentDMO
                                 from b in _db.admissioncls
                                 where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)
                                 select new EmployeePortal_StudentAttrndenceDTO.Output_ClassList
                                 {
                                     classname = b.ASMCL_ClassName,
                                     ASMCL_Id = b.ASMCL_Id
                                 }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                var SectionList = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   from c in _db.Adm_M_Student
                                   from d in _db.Section
                                   where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == a.ASMS_Id)
                                   select new EmployeePortal_StudentAttrndenceDTO.Output_SectionList
                                   {
                                       sectionname = d.ASMC_SectionName,
                                       ASMS_Id = d.ASMS_Id
                                   }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
                var studentList = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   from c in _db.Adm_M_Student
                                   from d in _db.Section
                                   where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMS_Id == data.ASMS_Id)
                                   select new EmployeePortal_StudentAttrndenceDTO.Output_StudentList
                                   {
                                       AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                       AMST_Id = a.AMST_Id
                                   }).Distinct().OrderBy(t => t.AMST_Id).ToArray();

                var savelist = (from a in _examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                from b in _examcontext.AdmissionClass
                                from c in _examcontext.exammasterDMO
                                from d in _examcontext.IVRM_School_Master_SubjectsDMO
                                from e in _examcontext.School_M_Section
                                from f in _examcontext.Adm_M_Student
                                from h in _examcontext.School_Adm_Y_Student
                                where (a.ASMCL_Id == b.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == c.EME_Id && a.ISMS_Id == d.ISMS_Id && a.ASMS_Id == e.ASMS_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == f.AMST_Id && a.MI_Id == data.MI_Id && a.EME_Id == data.EME_Id && h.ASMAY_Id == data.ASMAY_Id && h.ASMCL_Id == data.ASMCL_Id && h.ASMS_Id == a.ASMS_Id && h.AMST_Id == a.AMST_Id && a.AMST_Id == data.AMST_Id)
                                select new EmployeePortalStudentReportCardDTO.OutputSaveList
                                {
                                    ESTMPS_ObtainedMarks = a.ESTMPS_ObtainedMarks,
                                    ESTMPS_ObtainedGrade = a.ESTMPS_ObtainedGrade,
                                    ESTMPS_PassFailFlg = a.ESTMPS_PassFailFlg,
                                    EME_ExamName = c.EME_ExamName,
                                    ASMCL_ClassName = b.ASMCL_ClassName,
                                    ASMC_SectionName = e.ASMC_SectionName,
                                    AMST_Id = f.AMST_Id,
                                    AMST_FirstName = ((f.AMST_FirstName == null ? " " : f.AMST_FirstName) + (f.AMST_MiddleName == null ? " " : f.AMST_MiddleName) + (f.AMST_LastName == null ? " " : f.AMST_LastName)).Trim(),
                                    AMST_DOB = f.AMST_DOB,
                                    AMAY_RollNo = h.AMAY_RollNo,
                                    AMST_AdmNo = f.AMST_AdmNo,
                                    ISMS_Id = d.ISMS_Id,
                                    ISMS_SubjectName = d.ISMS_SubjectName,
                                    ESTMPS_MaxMarks = a.ESTMPS_MaxMarks,
                                }).Distinct().ToArray();

                var savelisttot = _examcontext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.AMST_Id == data.AMST_Id).Distinct().ToArray();





                var list = new List<EmployeePortalStudentReportCardDTO.Output>();


                list.Add(new EmployeePortalStudentReportCardDTO.Output { clstchname = clstchname, savelist = savelist });





                ldt.EmployeePortalStudentReportCard = list.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }

        public EmployeePortalStudentSearchDTO EmployeePortalStudentSearchD(EmployeePortalStudentSearchDTO.Input data)
        {
            EmployeePortalStudentSearchDTO ldt = new EmployeePortalStudentSearchDTO();
            try
            {

                var studentList = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   from c in _db.Adm_M_Student
                                   from d in _db.Section
                                   where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.ASMS_Id == data.ASMS_Id)
                                   select new EmployeePortalStudentSearchDTO.Output_StudentList
                                   {
                                       AMST_FirstName = ((c.AMST_FirstName == null ? " " : c.AMST_FirstName) + (c.AMST_MiddleName == null ? " " : c.AMST_MiddleName) + (c.AMST_LastName == null ? " " : c.AMST_LastName)).Trim(),
                                       AMST_Id = a.AMST_Id
                                   }).Distinct().OrderBy(t => t.AMST_Id).ToArray();
                var classlist = (from a in _db.School_Adm_Y_StudentDMO
                                 from b in _db.admissioncls
                                 where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)
                                 select new EmployeePortalStudentSearchDTO.Output_ClassList
                                 {
                                     classname = b.ASMCL_ClassName,
                                     ASMCL_Id = b.ASMCL_Id
                                 }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                var SectionList = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.admissioncls
                                   from c in _db.Adm_M_Student
                                   from d in _db.Section
                                   where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == a.ASMS_Id)
                                   select new EmployeePortalStudentSearchDTO.Output_SectionList
                                   {
                                       sectionname = d.ASMC_SectionName,
                                       ASMS_Id = d.ASMS_Id
                                   }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

                var fillstudentalldetails = (from a in _dbf.AdmissionStudentDMO
                                             from b in _dbf.School_Adm_Y_StudentDMO
                                             from c in _dbf.admissioncls
                                             from d in _dbf.school_M_Section
                                             from e in _dbf.AcademicYear
                                             where (b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && e.ASMAY_Id == data.ASMAY_Id)
                                             select new EmployeePortalStudentSearchDTO.Output_StudentD
                                             {
                                                 // amst_Id = a.AMST_Id,
                                                 amst_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),

                                                 amst_RegistrationNo = a.AMST_RegistrationNo,
                                                 amst_AdmNo = a.AMST_AdmNo,
                                                 amay_RollNo = b.AMAY_RollNo,
                                                 classname = c.ASMCL_ClassName,
                                                 sectionname = d.ASMC_SectionName,
                                                 fathername = a.AMST_FatherName,
                                                 mothername = a.AMST_MotherName,
                                                 bloodgroup = a.AMST_BloodGroup,
                                                 address1 = a.AMST_PerStreet,
                                                 address2 = a.AMST_PerArea,
                                                 address3 = a.AMST_PerCity,
                                                 fathermobileno = a.AMST_FatherMobleNo,
                                                 studentdob = a.AMST_DOB,
                                                 amst_mobile = a.AMST_MobileNo,
                                                 amst_sex = a.AMST_Sex,
                                                 amst_dob = a.AMST_DOB,
                                                 amst_emailid = a.AMST_emailId,
                                                 asma_year = e.ASMAY_Year

                                             }
                   ).Distinct().ToArray();

                var examlist = (from a in _examcontext.ExmStudentMarksProcessDMO
                                from b in _examcontext.exammasterDMO
                                where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id && a.EME_Id == b.EME_Id && b.EME_ActiveFlag == true)
                                select new EmployeePortalStudentSearchDTO.Output_StudentExamD
                                {
                                    exam_name = b.EME_ExamName,
                                    EME_Id = b.EME_Id,
                                }
                               ).Distinct().ToArray();


                var list = new List<EmployeePortalStudentSearchDTO.Output>();


                list.Add(new EmployeePortalStudentSearchDTO.Output { studentList = studentList, classlist = classlist, sectionList = SectionList, fillstudentalldetails = fillstudentalldetails, examlist = examlist });

                ldt.EmployeePortalStudentSearchD = list.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
        }

        public EmployeePortalLeaveDTO EmployeePortalLeaveD(EmployeePortalLeaveDTO.Input data)
        {
            EmployeePortalLeaveDTO ldt = new EmployeePortalLeaveDTO();
            try
            {

                var leaveList = (from a in _hrmscontext.MasterEmployee
                                 from c in _leave.HR_Emp_Leave_StatusDMO
                                 from d in _leave.HR_Master_Leave_DMO
                                 where (a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id && d.HRML_Id == c.HRML_Id && c.HRMLY_Id == data.HRMLY_Id)
                                 select new EmployeePortalLeaveDTO.Output
                                 {

                                     HRML_LeaveName = d.HRML_LeaveName,
                                     HRELS_TotalLeaves = c.HRELS_TotalLeaves,
                                     HRELS_TransLeaves = c.HRELS_TransLeaves,
                                     HRELS_CBLeaves = c.HRELS_CBLeaves,


                                 }).Distinct().ToArray();
                var list = new List<EmployeePortalLeaveDTO.OutputA>();


                list.Add(new EmployeePortalLeaveDTO.OutputA { leaveList = leaveList });

                ldt.EmployeePortal_LeaveD = list.ToArray();

            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
            //  return Emp;


        }

        public EmployeeDTO EmployeePortalDetails(EmployeeDTO.input data)
        {
            EmployeeDTO ldt = new EmployeeDTO();
            try
            {

                ldt.Employee = (from a in _FOContext.HR_Master_Employee_DMO
                                from b in _FOContext.HR_Master_Department_DMO
                                from c in _FOContext.HR_Master_Designation_DMO
                                where (a.HRMD_Id == b.HRMD_Id && a.HRMDES_Id == c.HRMDES_Id && a.HRME_ActiveFlag == true
                                    && b.HRMD_ActiveFlag == true && c.HRMDES_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRME_Id == data.HRME_ID)
                                select new EmployeeDTO.output
                                {
                                    HRME_Id = a.HRME_Id,
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                    HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                    HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                    HRME_EmployeeCode = a.HRME_EmployeeCode,
                                    HRME_DOB = a.HRME_DOB,
                                    HRME_DOJ = a.HRME_DOJ,
                                    HRMEM_EmailId = a.HRME_EmailId,
                                    Emp_MobileNo = a.HRME_MobileNo,
                                    HRME_Photo = a.HRME_Photo,
                                    Hrme_designation = c.HRMDES_DesignationName,
                                    hrme_deptname = b.HRMD_DepartmentName,

                                }).Distinct().ToArray();


                //var list = new List<EmployeeDTO>();


                //list.Add(new EmployeeDTO { EmployeeDetails = EmpDetails });

                //ldt.Employee = list.ToArray();
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;
            //  return Emp;


        }
        //IVRM_User_Login_Faculty
        public EmployeeloginDTO EmployeeDetails(EmployeeloginDTO.input data)
        {
            EmployeeloginDTO ldt = new EmployeeloginDTO();
            try
            {

                ldt.Employee = (from a in _hrmscontext.MasterEmployee
                                from b in _hrmscontext.Emp_Email_Id
                                from c in _hrmscontext.Emp_MobileNo
                                from d in _hrmscontext.Staff_User_Login
                                from e in _hrmscontext.HR_Master_Department
                                from f in _hrmscontext.HR_Master_Designation
                                where (a.MI_Id == data.MI_Id && b.HRME_Id == a.HRME_Id && c.HRME_Id == a.HRME_Id && a.HRME_Id == d.Emp_Code && d.IVRMSTAUL_UserName == data.IVRMSTAUL_UserName && d.IVRMSTAUL_Password == data.IVRMSTAUL_Password && e.HRMD_Id == a.HRMD_Id && f.HRMDES_Id == a.HRMDES_Id && e.MI_Id == data.MI_Id && f.MI_Id == data.MI_Id)
                                select new EmployeeloginDTO.output
                                {
                                    HRME_Id = a.HRME_Id,
                                    MI_Id = a.MI_Id,
                                    HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                    HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                                    HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                                    HRME_EmployeeCode = a.HRME_EmployeeCode,
                                    HRME_DOB = a.HRME_DOB,
                                    HRME_DOJ = a.HRME_DOJ,
                                    HRMEM_EmailId = b.HRMEM_EmailId,
                                    Emp_MobileNo = c.HRMEMNO_MobileNo,
                                    HRME_Photo = a.HRME_Photo,
                                    Hrme_designation = f.HRMDES_DesignationName,
                                    hrme_deptname = e.HRMD_DepartmentName,

                                }).Distinct().ToArray();

                //var list = new List<EmployeeloginDTO>();


                //list.Add(new EmployeeloginDTO { Employee = s });

                //ldt.EmployeeDetails = list.ToArray();
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;



        }

        public EmployeeSalaryDTO EmployeesalaryDetails(EmployeeSalaryDTO.input data)
        {
            EmployeeSalaryDTO ldt = new EmployeeSalaryDTO();
            try
            {

                var logo = _db.Institution.Single(t => t.MI_Id == data.MI_Id);
                // var mlist = _db.month.Single(t => t.MI_Id == data.MI_Id);

                // var HRME_Id = _hrmscontext.MasterEmployee.Single(c => c.HRME_Id == data.HRME_ID && c.MI_Id == data.MI_Id);

                ldt.salarylist = (from m in _hrmscontext.HR_Employee_Salary
                                  from a in _hrmscontext.MasterEmployee
                                  from n in _hrmscontext.HR_Employee_Salary_Details
                                  from o in _hrmscontext.HR_Master_EarningsDeductions
                                  from b in _hrmscontext.HR_Master_Department
                                  from c in _hrmscontext.HR_Master_Designation
                                  where m.HRES_Id == n.HRES_Id && n.HRMED_Id == o.HRMED_Id && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_ID && o.HRMED_EarnDedFlag.Equals("Earning") && m.HRES_Year == data.HRES_YEAR && m.HRES_Month == data.HRES_MONTH && a.HRME_Id == data.HRME_ID && b.HRMD_Id == a.HRMD_Id && c.HRMDES_Id == a.HRMDES_Id
                                  group new { m, n, o, a, b, c }
                                    by new { m.HRES_Month, m.HRES_Year } into g
                                  select new EmployeeSalaryDTO.Output
                                  {
                                      //salary = g.Sum(d => d.n.HRESD_Amount),
                                      // monthName = g.FirstOrDefault().m.HRES_Month,
                                      HRME_ID = g.FirstOrDefault().a.HRME_Id,
                                      HRME_EmployeeFirstName = g.FirstOrDefault().a.HRME_EmployeeFirstName,
                                      HRME_EmployeeMiddleName = g.FirstOrDefault().a.HRME_EmployeeMiddleName,
                                      HRME_EmployeeLastName = g.FirstOrDefault().a.HRME_EmployeeLastName,
                                      HRME_EmployeeCode = g.FirstOrDefault().a.HRME_EmployeeCode,
                                      HRME_DOJ = g.FirstOrDefault().a.HRME_DOJ,
                                      HRME_PFAccNo = g.FirstOrDefault().a.HRME_PFAccNo,
                                      HRES_WorkingDays = g.FirstOrDefault().m.HRES_WorkingDays,
                                      Hrme_designation = g.FirstOrDefault().c.HRMDES_DesignationName,
                                      hrme_deptname = g.FirstOrDefault().b.HRMD_DepartmentName

                                  }
                                ).ToArray();


                var EQuery = _hrmscontext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == data.MI_Id && t.HRMED_EarnDedFlag.Equals("Earning")).Select(d => d.HRMED_Id).ToList();
                ldt.TotalEarning = (from m in _hrmscontext.HR_Employee_Salary
                                    from n in _hrmscontext.HR_Employee_Salary_Details
                                    from o in _hrmscontext.HR_Master_EarningsDeductions

                                    where m.HRES_Id == n.HRES_Id && m.MI_Id == data.MI_Id && o.HRMED_Id == n.HRMED_Id && m.HRES_Month == data.HRES_MONTH && m.HRES_Year == data.HRES_YEAR && o.HRMED_EarnDedFlag.Equals("Earning") && m.HRME_Id == data.HRME_ID

                                    select new EmployeeSalaryDTO.EOutput
                                    {
                                        Amount = n.HRESD_Amount,
                                        earningname = o.HRMED_Name,
                                      
                                    }
                                 ).ToArray();
                var DQuery = _hrmscontext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == data.MI_Id && t.HRMED_EarnDedFlag.Equals("Deduction")).Select(d => d.HRMED_Id).ToList();
                ldt.totalDeduction = (from m in _hrmscontext.HR_Employee_Salary
                                      from n in _hrmscontext.HR_Employee_Salary_Details
                                      from o in _hrmscontext.HR_Master_EarningsDeductions

                                      where m.HRES_Id == n.HRES_Id && m.MI_Id == data.MI_Id && o.HRMED_Id == n.HRMED_Id && m.HRES_Month == data.HRES_MONTH && m.HRES_Year == data.HRES_YEAR && o.HRMED_EarnDedFlag.Equals("Deduction") && m.HRME_Id == data.HRME_ID

                                      select new EmployeeSalaryDTO.DOutput
                                      {
                                          Amount = n.HRESD_Amount,
                                          Deductionname = o.HRMED_Name,

                                      }
                                 ).ToArray();


                //ldt.TotalSumEarning = (from m in _hrmscontext.HR_Employee_Salary
                //                       from n in _hrmscontext.HR_Employee_Salary_Details

                //                       where m.HRES_Id == n.HRES_Id && EQuery.Contains(n.HRMED_Id) && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_ID && m.HRES_Year == data.HRES_YEAR && m.HRES_Month == data.HRES_MONTH
                //                       group new { m, n }
                //                        by new { m.HRES_Month, m.HRES_Year } into g
                //                       select new EmployeeSalaryDTO.AddEarning
                //                       {
                //                           E1 = g.Sum(d => d.n.HRESD_Amount),

                //                       }
                //                 ).ToArray();

                //ldt.TotalSumDeduction = (from m in _hrmscontext.HR_Employee_Salary
                //                         from n in _hrmscontext.HR_Employee_Salary_Details

                //                         where m.HRES_Id == n.HRES_Id && DQuery.Contains(n.HRMED_Id) && m.MI_Id == data.MI_Id && m.HRME_Id == data.HRME_ID && m.HRES_Year == data.HRES_YEAR && m.HRES_Month == data.HRES_MONTH
                //                         group new { m, n }
                //                          by new { m.HRES_Month, m.HRES_Year } into g
                //                         select new EmployeeSalaryDTO.AddDeduction
                //                         {
                //                             D1 = g.Sum(d => d.n.HRESD_Amount),

                //                         }
                //                ).ToArray();




          
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ldt;


        }
    }

}
