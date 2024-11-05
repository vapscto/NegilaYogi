using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeStudentAttendenceDetailsImpl : Interfaces.EmployeeStudentAttendenceDetailsInterface
    {
        private static ConcurrentDictionary<string, EmployeeDashboardDTO> _login =
        new ConcurrentDictionary<string, EmployeeDashboardDTO>();

        private readonly ExamContext _PCReportContext;
        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<EmployeeStudentAttendenceDetailsImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public EmployeeStudentAttendenceDetailsImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, ExamContext PCReportContext)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
            _PCReportContext = PCReportContext;
        }

        public EmployeeDashboardDTO Getdetails(EmployeeDashboardDTO data)//int IVRMM_Id
        {
            List<MasterAcademic> list = new List<MasterAcademic>();
            list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1).ToList();
            data.yearlist = list.ToArray();
            return data;

        }
        public EmployeeDashboardDTO getclass(EmployeeDashboardDTO data)//int IVRMM_Id
        {
            try
            {
                data.HRME_Id = _PCReportContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                //var EQuery = (from a in _PCReportContext.HR_Master_Employee_DMO
                //              from b in _PCReportContext.Staff_User_Login
                //              from c in _PCReportContext.Exm_Login_PrivilegeDMO
                //              from d in _PCReportContext.Exm_Login_Privilege_SubjectsDMO
                //              where (a.HRME_Id == b.Emp_Code && b.IVRMSTAUL_Id == c.Login_Id && c.ELP_Id == d.ELP_Id && a.HRME_Id == data.HRME_Id && c.MI_Id == data.MI_Id)
                //              select d.ASMCL_Id).Distinct().ToList();

                //data.classlist = (from a in _db.School_Adm_Y_StudentDMO
                //                  from b in _db.admissioncls
                //                  where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1 && EQuery.Contains(b.ASMCL_Id))
                //                  select new EmployeeDashboardDTO
                //                  {
                //                      classname = b.ASMCL_ClassName,
                //                      ASMCL_Id = b.ASMCL_Id
                //                  }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                //data.classlist = (from a in _ChairmanDashboardContext.Adm_SchoolAttendanceLoginUserClass
                //                  from b in _ChairmanDashboardContext.Adm_SchoolAttendanceLoginUser
                //                  from c in _ChairmanDashboardContext.School_M_Class
                //                  where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.HRME_Id == data.HRME_Id && c.ASMCL_ActiveFlag == true)
                //                  select new EmployeeDashboardDTO
                //                  {
                //                      classname = c.ASMCL_ClassName,
                //                      ASMCL_Id = c.ASMCL_Id
                //                  }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                data.classlist = (from a in _ChairmanDashboardContext.ClassTeacherMappingDMO
                                  from b in _ChairmanDashboardContext.School_M_Class
                                  from c in _ChairmanDashboardContext.AcademicYearDMO
                                  where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                  && a.HRME_Id == data.HRME_Id && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                  select new EmployeeDashboardDTO
                                  {
                                      ASMCL_Id = b.ASMCL_Id,
                                      classname = b.ASMCL_ClassName,
                                  }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public EmployeeDashboardDTO Getsection(EmployeeDashboardDTO data)//int IVRMM_Id
        {
            try
            {
                data.HRME_Id = _PCReportContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                using (var cmd = _ChairmanDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRM_Interaction_Section";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@roleflg",
                   SqlDbType.VarChar)
                    {
                        Value = data.roleflg
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
                        data.SectionList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }


            //    {


            //        data.SectionList = (from a in _ChairmanDashboardContext.ClassTeacherMappingDMO
            //                        from b in _ChairmanDashboardContext.School_M_Section
            //                        from c in _ChairmanDashboardContext.AcademicYearDMO
            //                        from d in _ChairmanDashboardContext.School_M_Class
            //                        where (a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
            //                        && a.ASMCL_Id == data.ASMCL_Id && a.HRME_Id == data.HRME_Id && a.IMCT_ActiveFlag == true && b.ASMC_ActiveFlag == 1)
            //                        select new EmployeeDashboardDTO
            //                        {
            //                            ASMS_Id = b.ASMS_Id,
            //                            ASMC_SectionName = b.ASMC_SectionName,

            //                        }).Distinct().ToArray();

            //    //var EQuery1 = (from a in _PCReportContext.HR_Master_Employee_DMO
            //    //               from b in _PCReportContext.Staff_User_Login
            //    //               from c in _PCReportContext.Exm_Login_PrivilegeDMO
            //    //               from d in _PCReportContext.Exm_Login_Privilege_SubjectsDMO
            //    //               where (a.HRME_Id == b.Emp_Code && b.IVRMSTAUL_Id == c.Login_Id && c.ELP_Id == d.ELP_Id && a.HRME_Id == data.HRME_Id && c.MI_Id == data.MI_Id && d.ASMCL_Id == data.ASMCL_Id)
            //    //               select d.ASMS_Id).Distinct().ToList();

            //    //data.SectionList = (from a in _db.School_Adm_Y_StudentDMO
            //    //                    from b in _db.admissioncls
            //    //                    from c in _db.Adm_M_Student
            //    //                    from d in _db.Section
            //    //                    where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && EQuery1.Contains(d.ASMS_Id))
            //    //                    select new EmployeeDashboardDTO
            //    //                    {
            //    //                        sectionname = d.ASMC_SectionName,
            //    //                        ASMS_Id = d.ASMS_Id
            //    //                    }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

            //    //var classid = _ChairmanDashboardContext.Masterclasscategory.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id).Select(t => t.ASMCC_Id).ToArray();

            //    //var secid = _ChairmanDashboardContext.AdmSchoolMasterClassCatSec.Where(t => classid.Contains(t.ASMCC_Id)).Select(t => t.ASMS_Id).ToArray();

            //    //var sectionexamid = (from e in _ChairmanDashboardContext.Staff_User_Login
            //    //                     from f in _ChairmanDashboardContext.Exm_Login_PrivilegeDMO
            //    //                     from i in _ChairmanDashboardContext.Exm_Login_Privilege_SubjectsDMO
            //    //                     where (e.Id == data.UserId && //data.MI_Id == data.MI_Id &&
            //    //                       f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && i.ASMCL_Id == data.ASMCL_Id && secid.Contains(i.ASMS_Id)
            //    //                       && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
            //    //                     select new IVRM_Homework_DTO
            //    //                     {
            //    //                         ASMS_Id = i.ASMS_Id
            //    //                     }).Distinct().Select(t => t.ASMS_Id).ToArray();

            //    //data.SectionList= (from b in _ChairmanDashboardContext.Adm_SchoolAttendanceLoginUser
            //    //                   from c in _ChairmanDashboardContext.Adm_SchoolAttendanceLoginUserClass
            //    //                   from d in _ChairmanDashboardContext.School_M_Class
            //    //                   from e in _ChairmanDashboardContext.School_M_Section
            //    //                   where(b.ASALU_Id==b.ASALU_Id ))


            //    //List<School_M_Section> seclist = new List<School_M_Section>();
            //    //seclist = _ChairmanDashboardContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1 && sectionexamid.Contains(t.ASMS_Id)).ToList();
            //    //data.SectionList = seclist.Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            //}


            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public EmployeeDashboardDTO GetAttendence(EmployeeDashboardDTO data)
        {
            try
            {
                data.studentList = (from a in _ChairmanDashboardContext.Adm_M_Student
                                    from b in _ChairmanDashboardContext.School_Adm_Y_StudentDMO
                                    from c in _ChairmanDashboardContext.AcademicYearDMO
                                    from d in _ChairmanDashboardContext.School_M_Class
                                    from e in _ChairmanDashboardContext.School_M_Section
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S")
                                    select new EmployeeDashboardDTO
                                    {
                                        AMST_FirstName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) || a.AMST_MiddleName == "0" ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) || a.AMST_LastName == "0" ? "" : ' ' + a.AMST_LastName),
                                        amst_AdmNo = a.AMST_AdmNo,
                                        Amst_Id = a.AMST_Id
                                    }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public async Task<EmployeeDashboardDTO> GetIndividualAttendence(EmployeeDashboardDTO data)
        {
            try
            {
                string amst_ids = "0";
                if (data.studentArray != null)
                {
                    foreach (var s in data.studentArray)
                    {
                        amst_ids = amst_ids + "," + s.AMST_Id;
                    }
                }
                using (var cmd = _ChairmanDashboardContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_STUDENT_ATTENDANCE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_ids",
                   SqlDbType.VarChar)
                    {
                        Value = amst_ids
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
                        data.attendencelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
