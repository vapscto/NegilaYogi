using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.College.Exam.LessonPlanner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
namespace NaacServiceHub.LessonPlanner.Services
{
    public class CollegeStaffperiodtransactionreportImpl : Interface.CollegeStaffperiodtransactionreportInterface
    {
        public LessonplannerContext _context;
        ILogger<CollegeStaffperiodtransactionreportImpl> _logg;


        public CollegeStaffperiodtransactionreportImpl(LessonplannerContext context, ILogger<CollegeStaffperiodtransactionreportImpl> logg)
        {
            _context = context;
            _logg = logg;
        }
        public CollegeStaffperiodtransactionreportDTO Getdetailstransaction(CollegeStaffperiodtransactionreportDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffperiodtransactionreportDTO onselectAcdYear(CollegeStaffperiodtransactionreportDTO data)
        {
            try
            {
                data.getcourse = (from a in _context.MasterCourseDMO
                                  from b in _context.CLG_Adm_College_AY_CourseDMO
                                  where (a.AMCO_Id == b.AMCO_Id && a.AMCO_ActiveFlag == true && b.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id
                                  && b.ASMAY_Id == data.ASMAY_Id)
                                  select a).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffperiodtransactionreportDTO onselectCourse(CollegeStaffperiodtransactionreportDTO data)
        {
            try
            {
                data.getbranch = (from a in _context.MasterCourseDMO
                                  from b in _context.CLG_Adm_College_AY_CourseDMO
                                  from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                  from d in _context.ClgMasterBranchDMO
                                  where (a.AMCO_Id == b.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && d.AMB_Id == c.AMB_Id
                                  && c.ACAYCB_ActiveFlag == true && d.AMB_ActiveFlag == true
                                  && a.AMCO_ActiveFlag == true && b.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id
                                  && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id)
                                  select d).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffperiodtransactionreportDTO onselectBranch(CollegeStaffperiodtransactionreportDTO data)
        {
            try
            {
                data.getsemester = (from a in _context.MasterCourseDMO
                                    from b in _context.CLG_Adm_College_AY_CourseDMO
                                    from c in _context.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _context.ClgMasterBranchDMO
                                    from e in _context.CLG_Adm_Master_SemesterDMO
                                    from f in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.AMCO_Id == b.AMCO_Id && b.ACAYC_Id == c.ACAYC_Id && d.AMB_Id == c.AMB_Id
                                    && e.AMSE_Id == f.AMSE_Id && f.ACAYCB_Id == c.ACAYCB_Id && f.ACAYCBS_ActiveFlag == true
                                    && e.AMSE_ActiveFlg == true && c.ACAYCB_ActiveFlag == true && d.AMB_ActiveFlag == true
                                    && a.AMCO_ActiveFlag == true && b.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id
                                    && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && c.AMB_Id == data.AMB_Id)
                                    select e).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffperiodtransactionreportDTO getsection(CollegeStaffperiodtransactionreportDTO data)
        {
            try
            {
                data.getsection = _context.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffperiodtransactionreportDTO onchangesection(CollegeStaffperiodtransactionreportDTO data)
        {
            try
            {
                data.getemployee = (from a in _context.HR_Master_Employee_DMO
                                    from b in _context.Adm_College_Atten_Login_DetailsDMO
                                    from c in _context.Adm_College_Atten_Login_UserDMO
                                    from d in _context.AcademicYear
                                    from e in _context.MasterCourseDMO
                                    from f in _context.ClgMasterBranchDMO
                                    from g in _context.CLG_Adm_Master_SemesterDMO
                                    from h in _context.Adm_College_Master_SectionDMO
                                    where (a.HRME_Id == c.HRME_Id && b.ACALU_Id == c.ACALU_Id && c.ASMAY_Id == d.ASMAY_Id && e.AMCO_Id == b.AMCO_Id
                                    && b.AMB_Id == f.AMB_Id && b.AMSE_Id == g.AMSE_Id && b.ACMS_Id == h.ACMS_Id && a.HRME_ActiveFlag == true
                                    && a.HRME_LeftFlag == false && b.ACALD_ActiveFlag == true && d.Is_Active == true && a.MI_Id == data.MI_Id
                                    && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && b.ACMS_Id == data.ACMS_Id
                                    && c.ASMAY_Id == data.ASMAY_Id)
                                    select new CollegeStaffperiodtransactionreportDTO
                                    {
                                        HRME_Id = a.HRME_Id,
                                        employeename = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) +
                                             (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName) +
                                             (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " : " + a.HRME_EmployeeCode)).Trim()
                                    }).Distinct().OrderBy(a => a.employeename).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffperiodtransactionreportDTO getreport(CollegeStaffperiodtransactionreportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Lesson_Planner_Staff_Transaction_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 1
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
                        data.getreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Lesson_Planner_Staff_Transaction_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 2
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
                        data.getreportemployee = retObject.ToArray();
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
            return data;
        }
        public CollegeStaffperiodtransactionreportDTO getdevationreport(CollegeStaffperiodtransactionreportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Lesson_Planner_Staff_Transaction_Devation_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 1
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
                        data.getdevationreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Lesson_Planner_Staff_Transaction_Devation_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 2
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
                        data.getdevationreportemployee = retObject.ToArray();
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
            return data;
        }

    }
}
