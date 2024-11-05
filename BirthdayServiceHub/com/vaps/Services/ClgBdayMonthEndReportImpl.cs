using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.College.Birthday;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.BirthDay;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayServiceHub.com.vaps.Services
{
    public class ClgBdayMonthEndReportImpl : Interfaces.ClgBdayMonthEndReportInterface
    {
        private static ConcurrentDictionary<string, ClgBirthDayDTO> _login =
         new ConcurrentDictionary<string, ClgBirthDayDTO>();

        public DomainModelMsSqlServerContext _db;
        public ClgBirthdayContext _ClgBirthdayContext;
        public ClgAdmissionContext _ClgAdmissionContext;

        public ClgBdayMonthEndReportImpl(DomainModelMsSqlServerContext db, ClgBirthdayContext clgbdContext, ClgAdmissionContext abc)
        {
            _db = db;
            _ClgBirthdayContext = clgbdContext;
            _ClgAdmissionContext = abc;
        }

        public ClgBirthDayDTO getloaddata(ClgBirthDayDTO data)
        {
            try
            {
                //data.fillyear = (from a in _db.AcademicYear
                //                where (a.MI_Id == data.MI_Id && a.ASMAY_ActiveFlag == 1)
                //                select new ClgBirthDayDTO
                //                {
                //                    ASMAY_Id = a.ASMAY_Id,
                //                    ASMAY_Year = a.ASMAY_Year
                //                }
                //             ).Distinct().ToArray();

                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _ClgAdmissionContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.acayear = year.ToArray();
                List<Month> mnth = new List<Month>();
                mnth = _ClgAdmissionContext.mnth.Where(t => t.Is_Active == true).ToList();
                data.Month_array = mnth.Distinct().OrderBy(t => t.IVRM_Month_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ClgBirthDayDTO> getmonthreport(ClgBirthDayDTO data)
        {
            try
            {
                #region
                //var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                ////data.studentlist = (from a in _db.Adm_M_Student
                ////                   from b in _db.School_Adm_Y_StudentDMO
                ////                   from c in _db.School_M_Class
                ////                   from d in _db.School_M_Section

                ////                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL.Equals("S") && b.AMAY_ActiveFlag == 1 && a.AMST_DOB.Month == data.month && b.ASMAY_Id == data.ASMAY_Id)

                ////                   select new ClgBirthDayDTO
                ////                   {
                ////                       AMST_Id = a.AMST_Id,
                ////                       studentname = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                ////                       AMST_emailId = a.AMST_emailId == null || a.AMST_emailId == "" ? "" : a.AMST_emailId,
                ////                       AMST_MobileNo = a.AMST_MobileNo == 0 || a.AMST_MobileNo == null ? 0 : a.AMST_MobileNo,
                ////                       AMST_DOB = a.AMST_DOB,
                ////                       ASMCL_ClassName = c.ASMCL_ClassName,
                ////                       ASMC_SectionName = d.ASMC_SectionName
                ////                   }
                ////             ).ToArray();


                //data.studentlist = (from y in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                //                     from s in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                //                     from c in _ClgAdmissionContext.MasterCourseDMO
                //                     from b in _ClgAdmissionContext.ClgMasterBranchDMO
                //                     from sm in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                //                     from yc in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                //                     from ycb in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                //                     from ycbs in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO

                //                     where (y.AMCST_Id == s.AMCST_Id && y.AMCO_Id==c.AMCO_Id && y.AMB_Id==b.AMB_Id && y.AMSE_Id==sm.AMSE_Id && b.AMB_Id==ycb.AMB_Id && sm.AMSE_Id==ycbs.AMSE_Id && y.ASMAY_Id == data.ASMAY_Id && s.MI_Id==data.MI_Id  && y.ACYST_ActiveFlag==true && s.AMCST_SOL=="S" && s.AMCST_DOB.Value.Month == data.month)

                //                    select new ClgBirthDayDTO
                //                    {
                //                        AMST_Id = s.AMCST_Id,
                //                        studentname = s.AMCST_FirstName + (string.IsNullOrEmpty(s.AMCST_MiddleName) ? "" : ' ' + s.AMCST_MiddleName) + (string.IsNullOrEmpty(s.AMCST_LastName) ? "" : ' ' + s.AMCST_LastName),
                //                        AMST_emailId = s.AMCST_emailId == null || s.AMCST_emailId == "" ? "" : s.AMCST_emailId,
                //                        //AMST_MobileNo = s.AMCST_MobileNo,
                //                        AMST_DOB = s.AMCST_DOB,
                //                        ASMCL_ClassName = c.AMCO_CourseName,
                //                        ASMC_SectionName = b.AMB_BranchName,
                //                    }
                //             ).ToArray();



                //if (data.studentlist.Length > 0)
                //{
                //    data.count = data.studentlist.Length;
                //}
                //else
                //{
                //    data.count = 0;
                //}

                //data.staffList = (from a in _db.HR_Master_Employee_DMO
                //                 from b in _db.Multiple_Email_DMO
                //                 from c in _db.Multiple_Mobile_DMO
                //                 where (a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && b.HRMEM_DeFaultFlag.Equals("default") && c.HRMEMNO_DeFaultFlag.Equals("default") && a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_DOB.Value.Month == data.month)

                //                 select new ClgBirthDayDTO
                //                 {
                //                     HRME_Id = a.HRME_Id,
                //                     employeename = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                //                     HRME_EmailId = b.HRMEM_EmailId == null || b.HRMEM_EmailId == "" ? "" : b.HRMEM_EmailId,
                //                     HRME_MobileNo = c.HRMEMNO_MobileNo == 0 || c.HRMEMNO_MobileNo == null ? 0 : c.HRMEMNO_MobileNo,
                //                     HRME_DOB = a.HRME_DOB
                //                 }
                //             ).ToArray();
                //if (data.staffList.Length > 0)
                //{
                //    data.count += data.staffList.Length;
                //}
                //else
                //{
                //    data.count += 0;
                //}

                //var quee1 = (from a in _db.AcademicYear
                //             where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                //             select new ClgBirthDayDTO
                //             {
                //                 date11 = a.ASMAY_From_Date
                //             }
                //             ).ToArray();
                //var quee2 = (from a in _db.AcademicYear
                //             where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                //             select new ClgBirthDayDTO
                //             {
                //                 date12 = a.ASMAY_To_Date
                //             }
                //            ).ToArray();

                //data.smsStatus = (from aw in _db.IVRM_sms_sentBoxDMO
                //                 where (aw.MI_Id == data.MI_Id && aw.Module_Name == "College Birthday" && aw.Datetime.Date >= quee1.FirstOrDefault().date11.Value.Date && aw.Datetime.Date <= quee2.FirstOrDefault().date12.Value.Date && aw.Datetime.Month == data.month)
                //                 select new ClgBirthDayDTO
                //                 {
                //                     ssb = aw.IVRM_SSB_ID
                //                 }).Count().ToString();

                //data.AMST_emailId = (from aw2 in _db.ivrm_email_sentbox
                //                    where (aw2.MI_Id == data.MI_Id && aw2.Module_Name == "College Birthday" && aw2.Datetime.Value.Date >= quee1.FirstOrDefault().date11.Value.Date && aw2.Datetime.Value.Date <= quee2.FirstOrDefault().date12.Value.Date && aw2.Datetime.Value.Month == data.month)
                //                    select new ClgBirthDayDTO
                //                    {
                //                        esb = aw2.IVRMESB_ID
                //                    }).Count().ToString();
                #endregion


                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Birthday_Month_End_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                  SqlDbType.VarChar)
                    {
                        Value = data.month
                    });

                    cmd.Parameters.Add(new SqlParameter("@year",
                  SqlDbType.VarChar)
                    {
                        Value = data.year
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
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
                        data.studentlist = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }




            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
