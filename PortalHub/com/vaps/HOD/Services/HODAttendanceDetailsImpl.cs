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
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DomainModel.Model;

namespace PortalHub.com.vaps.HOD.Services
{
    public class HODAttendanceDetailsImpl : Interfaces.HODAttendanceDetailsInterface
    {
        private static ConcurrentDictionary<string, StudentDashboardDTO> _login =
           new ConcurrentDictionary<string, StudentDashboardDTO>();
        private PortalContext _ChairmanDashboardContext;

        public DomainModelMsSqlServerContext _db;
        public HODAttendanceDetailsImpl(PortalContext Attcontext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = Attcontext;
            _db = db;
        }

        public ADMAttendenceDTO Getdetails(ADMAttendenceDTO data)//int IVRMM_Id
        {
            List<MasterAcademic> list = new List<MasterAcademic>();
            list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1).OrderByDescending(t => t.ASMAY_Order).ToList();
            data.yearlist = list.ToArray();

            //data.ASMAY_Id = data.ASMAY_Id;
            return data;
        }

        public ADMAttendenceDTO Getsection(ADMAttendenceDTO data)//int IVRMM_Id
        {


            try
            {
                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();


                //data.fillsection = (from d in _db.School_M_Section
                //                    from e in _db.Masterclasscategory
                //                    from f in _db.AdmSchoolMasterClassCatSec
                //                    where (d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == f.ASMS_Id && f.ASMCC_Id == e.ASMCC_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.asmcL_Id)
                //                    select new ADMAttendenceDTO
                //                    {
                //                        sectionname = d.ASMC_SectionName,
                //                        asmS_Id = d.ASMS_Id
                //                    }).Distinct().OrderBy(t => t.asmS_Id).ToArray();

                data.fillsection = (from a in _db.School_M_Class
                                    from b in _db.School_Adm_Y_StudentDMO
                                    from c in _db.School_M_Section
                                    where (a.ASMCL_Id == b.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.asmcL_Id && a.ASMCL_ActiveFlag == true && b.AMAY_ActiveFlag == 1 && c.ASMC_ActiveFlag == 1)
                                    select new ADMAttendenceDTO
                                    {
                                        sectionname = c.ASMC_SectionName,
                                        asmS_Id = c.ASMS_Id
                                    }).Distinct().OrderBy(t => t.asmS_Id).ToArray();


            }
            catch (Exception ex)
            {

            }
            return data;

        }

        public ADMAttendenceDTO getclass(ADMAttendenceDTO data)//int IVRMM_Id
        {
            try
            {
                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();
                data.fillclass = (from c in _ChairmanDashboardContext.School_M_Class
                                      //from e in _db.Masterclasscategory
                                      //from e in _ChairmanDashboardContext.Masterclasscategory
                                  from f in _ChairmanDashboardContext.IVRM_HOD_Class_DMO
                                      // from g in _ChairmanDashboardContext.IVRM_HOD_Staff_DMO
                                  from h in _ChairmanDashboardContext.HOD_DMO
                                  from z in _ChairmanDashboardContext.School_Adm_Y_StudentDMO
                                  from y in _ChairmanDashboardContext.AcademicYearDMO

                                  where (h.IHOD_Id == f.IHOD_Id && f.ASMCL_Id == c.ASMCL_Id && z.ASMCL_Id == c.ASMCL_Id && z.ASMAY_Id == y.ASMAY_Id && z.ASMAY_Id == data.ASMAY_Id && h.IHOD_Flg == "HOD" && h.IHOD_ActiveFlag == true && h.MI_Id == data.MI_Id && h.HRME_Id == loginData.FirstOrDefault().Emp_Code)
                                  select new ExamDTO
                                  {
                                      ASMCL_Id = c.ASMCL_Id,
                                      ASMCL_ClassName = c.ASMCL_ClassName
                                  }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                //_acdimpl.LogError(ex.Message);
                //_acdimpl.LogDebug(ex.Message);
            }
            return data;

        }

        public ADMAttendenceDTO GetAttendence(ADMAttendenceDTO data)//int IVRMM_Id
        {


            try
            {
                //if (data.type==2)
                //{
                data.Fillstudents = (from a in _db.Adm_M_Student
                                     from b in _db.School_Adm_Y_StudentDMO
                                     where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.asmcL_Id && b.ASMS_Id == data.asmS_Id && a.AMST_SOL == "S")
                                     select new ADMAttendenceDTO
                                     {
                                         //studentname = c.AMST_FirstName,

                                         studentname = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(), // studentname = ((e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName)).Trim(),
                                         amstid = a.AMST_Id
                                     }).Distinct().OrderBy(t => t.amstid).ToArray();
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public async Task<ADMAttendenceDTO> GetIndividualAttendenceAsync(ADMAttendenceDTO data)
        {
            try
            {
                List<ADMAttendenceDTO> result2 = new List<ADMAttendenceDTO>();
                List<ADMAttendenceDTO> result1 = new List<ADMAttendenceDTO>();
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
                            Value = data.amstid
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    result1.Add(new ADMAttendenceDTO
                                    {
                                        monthname = dataReader["MONTH_NAME"].ToString(),
                                        present = Convert.ToDecimal(dataReader["TOTAL_PRESENT"].ToString()),
                                        classheld = Convert.ToDecimal(dataReader["CLASS_HELD"].ToString()),

                                        perc = (Convert.ToDecimal(dataReader["TOTAL_PRESENT"].ToString()) / Convert.ToDecimal(dataReader["CLASS_HELD"].ToString())) * 100
                                    });
                                    data.attendencelist = result1.ToArray();
                                }
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
                    data.fillmonths = _ChairmanDashboardContext.IVRM_Month_DMO.Where(t => t.Is_Active == true).ToArray();

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
                            Value = data.asmcL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.asmS_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader.ReadAsync())
                                {
                                    result2.Add(new ADMAttendenceDTO
                                    {
                                        amstid = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                        studentname = dataReader["name"].ToString(),
                                        monthid = Convert.ToInt64(dataReader["month_id"].ToString()),
                                        monthname = dataReader["MONTH_NAME"].ToString(),
                                        present = Convert.ToDecimal(dataReader["TOTAL_PRESENT"].ToString()),
                                        classheld = Convert.ToDecimal(dataReader["CLASS_HELD"].ToString()),
                                        perc = Convert.ToDecimal(dataReader["per"].ToString())
                                    });
                                    data.allstudent = result2.ToArray();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return data;
        }
    }
}
