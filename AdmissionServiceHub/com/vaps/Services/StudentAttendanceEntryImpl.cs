using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DataAccessMsSqlServerProvider;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Dynamic;
using AutoMapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using CommonLibrary;
using Microsoft.AspNetCore.Identity;
using DomainModel.Model.com.vapstech.MobileApp;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentAttendanceEntryImpl : Interfaces.StudentAttendanceEntryInterface
    {
        string Student_name_null;
        string AMST_ADM_null;
        string amsT_RegistrationNo;

        private readonly UserManager<ApplicationUser> _userManager;

        private DomainModelMsSqlServerContext _db;
        private MasterSubjectContext _subject;       
        ILogger<StudentAttendanceEntryImpl> _acdimpl;       
        public StudentAttendanceEntryImpl() { }
        // parameterized constructor
        public StudentAttendanceEntryImpl(ILogger<StudentAttendanceEntryImpl> acdimpl, DomainModelMsSqlServerContext db, MasterSubjectContext subject,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _acdimpl = acdimpl;
            _subject = subject;
            _userManager = userManager;
        }

        public StudentAttendanceEntryDTO GetInitialData(StudentAttendanceEntryDTO stddto)
        {

            try
            {
                var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == stddto.MI_Id).ToList();
                if (attendance_entrytype.Count == 0)
                {
                    stddto.message = "Please Map The Attendance Entry Type  i.e., Absent / Present Type In Admission Standard";
                    return stddto;
                }
                else
                {
                    var check_rolename = (from a in _db.MasterRoleType
                                          where (a.IVRMRT_Id == stddto.roleId)
                                          select new StudentAttendanceEntryDTO
                                          {
                                              rolename = a.IVRMRT_Role,
                                          }).ToList();

                    var empcode_check = (from a in _db.Staff_User_Login
                                         where (a.MI_Id == stddto.MI_Id && a.Id.Equals(stddto.userId))
                                         select new StudentAttendanceEntryDTO
                                         {
                                             Emp_Code = a.Emp_Code,
                                         }).ToList();

                    if (empcode_check.Count > 0)
                    {
                        stddto.classList = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.School_M_Class
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                            && b.MI_Id == stddto.MI_Id && b.ASMAY_Id == stddto.ASMAY_Id
                                            && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                            && c.ASMCL_ActiveFlag == true)
                                            select new StudentAttendanceEntryDTO
                                            {
                                                ASMCL_Id = c.ASMCL_Id,
                                                asmcL_ClassName = c.ASMCL_ClassName,
                                            }).Distinct().ToArray();


                        stddto.sectionList = (from a in _db.Adm_SchAttLoginUserClass
                                              from b in _db.Adm_SchAttLoginUser
                                              from c in _db.School_M_Section
                                              where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                              && b.MI_Id == stddto.MI_Id && b.ASMAY_Id == stddto.ASMAY_Id
                                              && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                              && c.ASMC_ActiveFlag == 1)
                                              select new StudentAttendanceEntryDTO
                                              {
                                                  ASMS_Id = c.ASMS_Id,
                                                  asmC_SectionName = c.ASMC_SectionName,
                                              }).Distinct().ToArray();

                        if (stddto.classList.Length == 0)
                        {
                            stddto.message = "For This Staff There Is No Privileges To Enter Attendance.. Please Contact Administrator";
                        }
                    }
                    else
                    {
                        stddto.message = "For This Staff There Is No Privileges To Enter Attendance.. Please Contact Administrator";
                    }

                    stddto.academicYearList = _db.AcademicYear.Where(r => r.Is_Active == true && r.MI_Id == stddto.MI_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                    stddto.CurrentYear = _db.AcademicYear.Where(d => d.MI_Id == stddto.MI_Id && d.Is_Active == true && d.ASMAY_Id == stddto.ASMAY_Id).ToArray();
                    stddto.subjectList = _subject.IVRM_Master_SubjectsDMO.Where(d => d.MI_Id == stddto.MI_Id && d.ISMS_ActiveFlag == 1
                    && d.ISMS_AttendanceFlag == true).ToArray();

                    stddto.monthList = _db.month.ToArray();

                    stddto.batchList = _db.AdmSchoolAttendanceSubjectBatch.Where(d => d.MI_Id == stddto.MI_Id).ToArray();

                    stddto.periodlist = _db.TT_Master_PeriodDMO.Where(d => d.MI_Id == stddto.MI_Id && d.TTMP_ActiveFlag == true).ToArray();

                    if (stddto.stringmobileorportal == "Mobile")
                    {
                        List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                        Staffmobileappprivileges = _db.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == stddto.userId && t.MI_Id == stddto.MI_Id).ToList();

                        if (Staffmobileappprivileges.Count() > 0)
                        {
                            stddto.Staffmobileappprivileges = (from Mobilepage in _db.IVRM_MobileApp_Page
                                                               from MobileRolePrivileges in _db.IVRM_Role_MobileApp_Privileges
                                                               from UserRolePrivileges in _db.IVRM_User_MobileApp_Login_Privileges
                                                               where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                               && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                               && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id
                                                               && MobileRolePrivileges.IVRMRT_Id == stddto.roleId && MobileRolePrivileges.MI_ID == stddto.MI_Id
                                                               && UserRolePrivileges.IVRMUL_Id == stddto.userId)
                                                               select new StudentAttendanceEntryDTO
                                                               {
                                                                   Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                                   Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                                   Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                                   IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                                   IVRMMAP_AddFlg = UserRolePrivileges.IVRMUMALP_AddFlg,
                                                                   IVRMMAP_UpdateFlg = UserRolePrivileges.IVRMUMALP_UpdateFlg,
                                                                   IVRMMAP_DeleteFlg = UserRolePrivileges.IVRMUMALP_DeleteFlg
                                                               }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                            stddto.mobileprivileges = "true";
                        }
                        else
                        {
                            stddto.mobileprivileges = "false";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Attendance Entry loadpage :'" + ex.Message + "'");
                //Console.Write(ex.Message);
            }
            return stddto;
        }
        public async Task<StudentAttendanceEntryDTO> getmonthclassheld(StudentAttendanceEntryDTO data)
        {
            var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
            data.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;

            data.getstandarad = _db.GenConfig.Where(a => a.MI_Id == data.MI_Id).ToArray();

            data.admissionstandarad = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";
            if (data.ASA_FromDate != null)
            {
                try
                {
                    fromdatecon = Convert.ToDateTime(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd"));
                    //confromdate = fromdatecon.ToString();
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    fromdatecon = Convert.ToDateTime(DateTime.UtcNow.Date.ToString("yyyy-MM-dd"));
                    //confromdate = fromdatecon.ToString();
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            string ordderby = "";
            if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 1)
            {
                ordderby = "Order By AMST_Sex";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 2)
            {
                ordderby = "Order By AMST_Sex desc";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 3)
            {
                ordderby = "Order By AMAY_RollNo ";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 4)
            {
                ordderby = "Order By studentname";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 5)
            {
                ordderby = "Order By studentname desc";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 6)
            {
                ordderby = "Order By amsT_RegistrationNo";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 7)
            {
                ordderby = "Order By AMST_Admno";
            }
            else
            {
                ordderby = "Order By studentname";
            }

            try
            {
                //getting no of class helds
                List<StudentAttendanceEntryDTO> result = new List<StudentAttendanceEntryDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "adm_no_class_held_month";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMCL_Id) });
                    cmd.Parameters.Add(new SqlParameter("@secid", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMS_Id) });
                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = data.monthflag });
                    cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(data.monthid) });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new StudentAttendanceEntryDTO
                                {
                                    countclass = Convert.ToDecimal(dataReader["classheld"]),
                                });

                            }
                        }
                        if (result.Count > 0)
                        {
                            data.countclass = result.FirstOrDefault().countclass;
                        }
                        else
                        {
                            data.message = "Please Enter The Number Of Class Held For Particular Month In Master Class Held";
                            return data;
                        }

                        if (data.countclass == Convert.ToDecimal(0.00))
                        {
                            data.message = "Please Enter The Number Of Class Held For Particular Month In Master Class Held";
                            return data;
                        }

                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Attendance Entry month class held sp error :'" + ex.Message + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Attendance Entry month class held :'" + ex.Message + "'");
            }

            long yearname = 0;
            try
            {
                _acdimpl.LogInformation("entered try block");
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    _acdimpl.LogInformation("entered cmd getdbconnection");
                    cmd.CommandText = "adm_attendance_get_year_from_month";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMCL_Id) });
                    cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMS_Id) });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = Convert.ToString(data.monthflag) });
                    cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(data.monthid) });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    _acdimpl.LogInformation("entered if block");
                    _acdimpl.LogInformation("Fromdate :'" + data.ASA_FromDate + "");
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            _acdimpl.LogInformation("entered in dataReader block");
                            while (dataReader.Read())
                            {
                                _acdimpl.LogInformation("entered in while block");
                                yearname = Convert.ToInt64(dataReader["AYear"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("error:'" + ex.Message + "'");
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                Console.Write(ex.Message);
            }

            //getting whether saved or not 
            var vdata = (from a in _db.Adm_studentAttendanceStudents
                         from b in _db.Adm_studentAttendance
                         where (a.ASA_Id == b.ASA_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                         && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ASA_Att_Type == data.monthflag1
                         && b.ASA_FromDate.Value.Month == data.monthid && b.ASA_FromDate.Value.Year == yearname && b.ASA_Activeflag == true)
                         select new StudentAttendanceEntryDTO
                         {
                             ASA_Id = b.ASA_Id
                         }).ToList();

            data.countclass1 = vdata.Count;

            //getting the save details for display 
            try
            {
                if (data.countclass1 > 0)
                {
                    List<StudentAttTempDTO> result1 = new List<StudentAttTempDTO>();
                    List<StudentAttTempDTO> studentList1 = new List<StudentAttTempDTO>();
                    List<StudentAttTempDTO> obj = new List<StudentAttTempDTO>();

                    try
                    {
                        _acdimpl.LogInformation("entered try block");
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            _acdimpl.LogInformation("entered cmd getdbconnection");
                            cmd.CommandText = "adm_student_list_not_in_att";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMCL_Id) });
                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMS_Id) });
                            cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = confromdate });
                            cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                            cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = Convert.ToString(data.monthflag) });
                            cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(data.monthid) });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            _acdimpl.LogInformation("entered if block");
                            _acdimpl.LogInformation("Fromdate :'" + data.ASA_FromDate + "");
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    _acdimpl.LogInformation("entered in dataReader block");
                                    while (dataReader.Read())
                                    {
                                        _acdimpl.LogInformation("entered in while block");

                                        result1.Add(new StudentAttTempDTO
                                        {
                                            amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                            studentname = (dataReader["studentname"]).ToString(),
                                            amsT_AdmNo = (dataReader["AMST_AdmNo"]).ToString(),
                                            amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                            amsT_RegistrationNo = (dataReader["amsT_RegistrationNo"]).ToString(),
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                Console.Write(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("error:'" + ex.Message + "'");
                        Console.Write(ex.Message);
                    }

                    using (var command = _db.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = " Select b.ASA_Class_Attended  as ASA_Class_Attended, b.ASAS_Id as ASAS_Id, a.ASA_Id as ASA_Id, (d.AMST_Id) AMST_Id, (CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+" +
                    "CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END + " +
                    "CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END) AS studentname," +
                    "(d.AMST_AdmNo)AMST_AdmNo, (c.AMAY_RollNo) AMAY_RollNo, AMST_Sex, (amsT_RegistrationNo) as amsT_RegistrationNo from Adm_Student_Attendance a " +
                    "INNER JOIN Adm_Student_Attendance_Students b on  a.asa_id=b.asa_id inner join  Adm_School_Y_Student c on c.amst_id=b.amst_id " +
                    " Inner Join  Adm_M_student d on d.amst_id=c.amst_id where ASA_Activeflag=1 and c.asmcl_id=" + data.ASMCL_Id + "  and c.ASMS_Id =" + data.ASMS_Id + " and c.ASMAY_Id =" + data.ASMAY_Id + " and a.asmcl_id=" + data.ASMCL_Id + "  and a.ASMS_Id =" + data.ASMS_Id + " and a.ASMAY_Id =" + data.ASMAY_Id + " " +
                    "and a.MI_Id =" + data.MI_Id + " and month(a.ASA_FromDate) =" + data.monthid + " and year(a.ASA_FromDate)=" + yearname + "  and a.ASMAY_Id =" + data.ASMAY_Id + "  and  a.ASA_Att_Type ='" + data.monthflag1 + "' " +
                    " and d.AMST_ActiveFlag = 1 and d.AMST_SOL = 'S' and c.AMAY_ActiveFlag = 1  " + ordderby + " ";
                        _db.Database.OpenConnection();
                        using (var result12 = command.ExecuteReader())
                        {
                            while (result12.Read())
                            {
                                obj.Add(new StudentAttTempDTO
                                {
                                    amsT_Id = Convert.ToInt64(result12["AMST_Id"]),
                                    studentname = result12["studentname"].ToString(),
                                    amsT_AdmNo = result12["AMST_AdmNo"].ToString(),
                                    amaY_RollNo = Convert.ToInt64(result12["AMAY_RollNo"]),
                                    amsT_RegistrationNo = result12["amsT_RegistrationNo"].ToString(),
                                    pdays = Convert.ToDecimal(result12["ASA_Class_Attended"]),
                                    ASAS_Id = Convert.ToInt64(result12["ASAS_Id"]),
                                    asA_Id = Convert.ToInt64(result12["ASA_Id"]),
                                });
                            }

                            studentList1 = obj.ToList();
                        }
                    }

                    for (int i = 0; i < result1.Count; i++)
                    {
                        studentList1.Add(result1[i]);
                    }

                    data.studentList = studentList1.ToArray();
                }
                else
                {
                    List<StudentAttTempDTO> arrAttdto = new List<StudentAttTempDTO>();
                    //string mywhere = null;
                    try
                    {
                        if (data.ASMAY_Id != 0 && data.ASMCL_Id != 0)
                        {
                            var type = _db.AttendanceEntryTypeDMO.Where(t => t.ASMCL_Id.Equals(data.ASMCL_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id)).ToArray();

                            data.ASA_Att_EntryType = type[0].ASAET_Att_Type;
                        }

                        if (data.ASMAY_Id != 0 && data.ASMCL_Id != 0)
                        {
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "GetStudentDataByAdecmicYearClassSection_New_St";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag });
                                cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar) { Value = confromdate });
                                cmd.Parameters.Add(new SqlParameter("@monthflag1", SqlDbType.VarChar) { Value = data.monthflag1 });
                                cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = data.monthid });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            if (dataReader["studentname"] != System.DBNull.Value)
                                            {
                                                Student_name_null = Convert.ToString(dataReader["studentname"]);
                                            }
                                            else
                                            {
                                                Student_name_null = "NOT AVAILABLE";
                                            }


                                            if (dataReader["AMST_AdmNo"] != System.DBNull.Value)
                                            {
                                                AMST_ADM_null = Convert.ToString(dataReader["AMST_AdmNo"]);
                                            }
                                            else
                                            {
                                                AMST_ADM_null = "NOT AVAILABLE";
                                            }
                                            if (dataReader["amsT_RegistrationNo"] != System.DBNull.Value)
                                            {
                                                amsT_RegistrationNo = Convert.ToString(dataReader["amsT_RegistrationNo"]);
                                            }
                                            else
                                            {
                                                amsT_RegistrationNo = "NOT AVAILABLE";
                                            }

                                            arrAttdto.Add(new StudentAttTempDTO
                                            {
                                                amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                studentname = Student_name_null,
                                                amsT_AdmNo = AMST_ADM_null,
                                                amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                                amsT_RegistrationNo = amsT_RegistrationNo,
                                            });
                                            data.studentList = arrAttdto.ToArray();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                    Console.Write(ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //start date and enddate based on month ids 
            try
            {
                List<StudentAttendanceEntryDTO> result = new List<StudentAttendanceEntryDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "get_startdate_enddate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(data.monthid) });
                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@asmayid", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new StudentAttendanceEntryDTO
                                {
                                    startdate = Convert.ToDateTime(dataReader["startdate"]),
                                    enddate = Convert.ToDateTime(dataReader["enddate"]),
                                });

                            }
                        }
                        data.startdate = result.FirstOrDefault().startdate;
                        data.enddate = result.FirstOrDefault().enddate;
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Attendance Entry get_startdate_enddate sp error :'" + ex.Message + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Attendance Entry get_startdate_enddate sp error :'" + ex.Message + "'");
            }

            return data;
        }
        public async Task<StudentAttendanceEntryDTO> GetStudentData(StudentAttendanceEntryDTO attdto)
        {
            List<StudentAttTempDTO> arrAttdto = new List<StudentAttTempDTO>();
            // attdto.getstandarad = _db.AdmissionStandardDMO.Where(a => a.MI_Id == attdto.MI_Id).ToArray();
            attdto.admissionstandarad = _db.AdmissionStandardDMO.Where(a => a.MI_Id == attdto.MI_Id).ToArray();
            attdto.getstandarad = _db.GenConfig.Where(a => a.MI_Id == attdto.MI_Id).ToArray();

            //string mywhere = null;
            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";
            if (attdto.ASA_FromDate != null)
            {
                try
                {
                    fromdatecon = Convert.ToDateTime(attdto.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd"));
                    //confromdate = fromdatecon.ToString();
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    fromdatecon = Convert.ToDateTime(DateTime.UtcNow.Date.ToString("yyyy-MM-dd"));
                    //confromdate = fromdatecon.ToString();
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == attdto.MI_Id).ToList();

            attdto.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;

            attdto.attdefaultdisplay = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag;

            string ordderby = "";
            if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 1)
            {
                ordderby = "Order By AMST_Sex";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 2)
            {
                ordderby = "Order By AMST_Sex desc";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 3)
            {
                ordderby = "Order By AMAY_RollNo ";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 4)
            {
                ordderby = "Order By studentname";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 5)
            {
                ordderby = "Order By studentname desc";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 6)
            {
                ordderby = "Order By amsT_RegistrationNo";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 7)
            {
                ordderby = "Order By AMST_Admno";
            }
            else
            {
                ordderby = "Order By studentname";
            }




            List<StudentAttendanceEntryDTO> vdata = new List<StudentAttendanceEntryDTO>();
            if (attdto.monthflag1 == "monthly")
            {
                //vdata = (from a in _db.Adm_studentAttendanceStudents
                //         from b in _db.Adm_studentAttendance
                //         where (a.ASA_Id == b.ASA_Id && b.MI_Id == attdto.MI_Id && b.ASMAY_Id == attdto.ASMAY_Id
                //         && b.ASMCL_Id == attdto.ASMCL_Id && b.ASMS_Id == attdto.ASMS_Id && b.ASA_Att_Type == attdto.monthflag1
                //         && b.ASA_FromDate <= attdto.ASA_FromDate && b.ASA_ToDate >= attdto.ASA_FromDate)
                //         select new StudentAttendanceEntryDTO
                //         {
                //             ASA_Id = b.ASA_Id
                //         }
                //               ).ToList();
            }
            else if (attdto.monthflag1 == "period")
            {
                vdata = (from a in _db.Adm_studentAttendanceStudents
                         from b in _db.Adm_studentAttendance
                         from c in _db.Adm_studentAttendanceSubjects
                         from d in _db.Adm_StudentAttendancePeriodwiseDMO
                         where (a.ASA_Id == b.ASA_Id && b.MI_Id == attdto.MI_Id && b.ASMAY_Id == attdto.ASMAY_Id
                         && b.ASMCL_Id == attdto.ASMCL_Id && b.ASMS_Id == attdto.ASMS_Id && b.ASA_Att_Type == attdto.monthflag1
                         && b.ASA_FromDate == attdto.ASA_FromDate && c.ASA_Id == b.ASA_Id && d.ASA_Id == b.ASA_Id && b.ASA_Activeflag == true)
                         select new StudentAttendanceEntryDTO
                         {
                             ASA_Id = b.ASA_Id
                         }).ToList();
            }
            else
            {
                vdata = (from a in _db.Adm_studentAttendanceStudents
                         from b in _db.Adm_studentAttendance
                         where (a.ASA_Id == b.ASA_Id && b.MI_Id == attdto.MI_Id && b.ASMAY_Id == attdto.ASMAY_Id
                         && b.ASMCL_Id == attdto.ASMCL_Id && b.ASMS_Id == attdto.ASMS_Id && b.ASA_Att_Type == attdto.monthflag1
                         && b.ASA_FromDate == attdto.ASA_FromDate && b.ASA_Activeflag == true)
                         select new StudentAttendanceEntryDTO
                         {
                             ASA_Id = b.ASA_Id
                         }).ToList();
            }

            attdto.countclass1 = vdata.Count;

            if (attdto.countclass1 >= 1)
            {
                try
                {
                    attdto.monthid = 0;
                    List<StudentAttTempDTO> studentList1 = new List<StudentAttTempDTO>();
                    List<StudentAttTempDTO> result = new List<StudentAttTempDTO>();
                    List<StudentAttTempDTO> obj = new List<StudentAttTempDTO>();

                    var type = _db.AttendanceEntryTypeDMO.Where(t => t.ASMCL_Id.Equals(attdto.ASMCL_Id) && t.ASMAY_Id.Equals(attdto.ASMAY_Id) && t.MI_Id.Equals(attdto.MI_Id)).ToArray();
                    attdto.ASA_Att_EntryType = type[0].ASAET_Att_Type;

                    try
                    {
                        _acdimpl.LogInformation("entered try block");
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            _acdimpl.LogInformation("entered cmd getdbconnection");
                            cmd.CommandText = "adm_student_list_not_in_att";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = Convert.ToString(attdto.ASMCL_Id) });
                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = Convert.ToString(attdto.ASMS_Id) });
                            cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = confromdate });
                            cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = Convert.ToString(attdto.MI_Id) });
                            cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = Convert.ToString(attdto.ASMAY_Id) });
                            cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = Convert.ToString(attdto.monthflag) });
                            cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(attdto.monthid) });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();
                            _acdimpl.LogInformation("entered if block");
                            _acdimpl.LogInformation("Fromdate :'" + attdto.ASA_FromDate + "");
                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    _acdimpl.LogInformation("entered in dataReader block");
                                    while (dataReader.Read())
                                    {
                                        _acdimpl.LogInformation("entered in while block");

                                        result.Add(new StudentAttTempDTO
                                        {
                                            amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                            studentname = (dataReader["studentname"]).ToString(),
                                            amsT_AdmNo = (dataReader["AMST_AdmNo"]).ToString(),
                                            amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                            amsT_RegistrationNo = (dataReader["amsT_RegistrationNo"]).ToString()
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                Console.Write(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("error:'" + ex.Message + "'");
                        Console.Write(ex.Message);
                    }
                    attdto.ASA_Mac_Add = "";

                    if (attdto.monthflag == "D")
                    {
                        using (var command = _db.Database.GetDbConnection().CreateCommand())
                        {
                            command.CommandText = " Select b.ASA_Class_Attended  as ASA_Class_Attended, b.ASAS_Id as ASAS_Id, a.ASA_Id as ASA_Id, (d.AMST_Id) AMST_Id, (CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+" +
                        "CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END + " +
                        "CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END) AS studentname," +
                        "(d.AMST_AdmNo)AMST_AdmNo, (c.AMAY_RollNo) AMAY_RollNo, AMST_Sex, (amsT_RegistrationNo) as amsT_RegistrationNo from Adm_Student_Attendance a " +
                        "INNER JOIN Adm_Student_Attendance_Students b on  a.asa_id=b.asa_id inner join  Adm_School_Y_Student c on c.amst_id=b.amst_id " +
                        " Inner Join  Adm_M_student d on d.amst_id=c.amst_id  inner join Adm_School_M_Academic_Year dd on dd.asmay_id=c.ASMAY_Id  where ASA_Activeflag=1 and   c.asmcl_id=" + attdto.ASMCL_Id + "  and c.ASMS_Id =" + attdto.ASMS_Id + " and c.ASMAY_Id =" + attdto.ASMAY_Id + " and a.asmcl_id=" + attdto.ASMCL_Id + "  and a.ASMS_Id =" + attdto.ASMS_Id + " and a.ASMAY_Id =" + attdto.ASMAY_Id + " " +
                        "and a.MI_Id =" + attdto.MI_Id + " and a.ASA_FromDate ='" + confromdate + "' and  a.ASA_Att_Type ='" + attdto.monthflag1 + "' " +
                        " and d.AMST_ActiveFlag = 1 and d.AMST_SOL = 'S' and c.AMAY_ActiveFlag = 1  " + ordderby + " ";
                            _db.Database.OpenConnection();
                            using (var result1 = command.ExecuteReader())
                            {
                                while (result1.Read())
                                {
                                    obj.Add(new StudentAttTempDTO
                                    {
                                        amsT_Id = Convert.ToInt64(result1["AMST_Id"]),
                                        studentname = result1["studentname"].ToString(),
                                        amsT_AdmNo = result1["AMST_AdmNo"].ToString(),
                                        amaY_RollNo = Convert.ToInt64(result1["AMAY_RollNo"]),
                                        amsT_RegistrationNo = result1["amsT_RegistrationNo"].ToString(),
                                        pdays = Convert.ToDecimal(result1["ASA_Class_Attended"]),
                                        ASAS_Id = Convert.ToInt64(result1["ASAS_Id"]),
                                        asA_Id = Convert.ToInt64(result1["ASA_Id"]),
                                    });
                                }

                                studentList1 = obj.ToList();
                            }
                        }
                    }


                    else if (attdto.monthflag == "H")
                    {

                        using (var command = _db.Database.GetDbConnection().CreateCommand())
                        {
                            command.CommandText = " Select b.ASA_Class_Attended  as ASA_Class_Attended, b.ASAS_Id as ASAS_Id, a.ASA_Id as ASA_Id, (d.AMST_Id) AMST_Id, (CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+" +
                        "CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END + " +
                        "CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END) AS studentname," +
                        "(d.AMST_AdmNo)AMST_AdmNo, (c.AMAY_RollNo) AMAY_RollNo, AMST_Sex, (amsT_RegistrationNo) as amsT_RegistrationNo ,asA_Dailytwice_Flag as asA_Dailytwice_Flag from Adm_Student_Attendance a " +
                        "INNER JOIN Adm_Student_Attendance_Students b on  a.asa_id=b.asa_id inner join  Adm_School_Y_Student c on c.amst_id=b.amst_id " +
                        " Inner Join  Adm_M_student d on d.amst_id=c.amst_id  inner join Adm_School_M_Academic_Year dd on dd.asmay_id=c.ASMAY_Id where  ASA_Activeflag=1 and  c.asmcl_id=" + attdto.ASMCL_Id + "  and c.ASMS_Id =" + attdto.ASMS_Id + " and c.ASMAY_Id =" + attdto.ASMAY_Id + " and a.asmcl_id=" + attdto.ASMCL_Id + "  and a.ASMS_Id =" + attdto.ASMS_Id + " and a.ASMAY_Id =" + attdto.ASMAY_Id + " " +
                        "and a.MI_Id =" + attdto.MI_Id + " and a.ASA_FromDate ='" + confromdate + "' and  a.ASA_Att_Type ='" + attdto.monthflag1 + "' " +
                        " and d.AMST_ActiveFlag = 1 and d.AMST_SOL = 'S' and c.AMAY_ActiveFlag = 1  " + ordderby + " ";
                            _db.Database.OpenConnection();
                            using (var result1 = command.ExecuteReader())
                            {
                                while (result1.Read())
                                {
                                    obj.Add(new StudentAttTempDTO
                                    {
                                        amsT_Id = Convert.ToInt64(result1["AMST_Id"]),
                                        studentname = result1["studentname"].ToString(),
                                        amsT_AdmNo = result1["AMST_AdmNo"].ToString(),
                                        amaY_RollNo = Convert.ToInt64(result1["AMAY_RollNo"]),
                                        amsT_RegistrationNo = result1["amsT_RegistrationNo"].ToString(),
                                        pdays = Convert.ToDecimal(result1["ASA_Class_Attended"]),
                                        ASAS_Id = Convert.ToInt64(result1["ASAS_Id"]),
                                        asA_Id = Convert.ToInt64(result1["ASA_Id"]),
                                        asA_Dailytwice_Flag = result1["asA_Dailytwice_Flag"].ToString(),
                                    });
                                }
                                studentList1 = obj.ToList();
                            }
                        }
                    }

                    else if (attdto.monthflag == "P")
                    {

                        using (var command = _db.Database.GetDbConnection().CreateCommand())
                        {
                            command.CommandText = " Select b.ASA_Class_Attended  as ASA_Class_Attended, b.ASAS_Id as ASAS_Id, a.ASA_Id as ASA_Id, (d.AMST_Id) AMST_Id, (CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+" +
                        "CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END + " +
                        "CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END) AS studentname," +
                        "(d.AMST_AdmNo)AMST_AdmNo, (c.AMAY_RollNo) AMAY_RollNo, AMST_Sex, (amsT_RegistrationNo) as amsT_RegistrationNo from Adm_Student_Attendance a " +
                        "INNER JOIN Adm_Student_Attendance_Students b on  a.asa_id=b.asa_id inner join  Adm_School_Y_Student c on c.amst_id=b.amst_id " +
                        " Inner Join  Adm_M_student d on d.amst_id=c.amst_id  inner join Adm_School_M_Academic_Year dd on dd.asmay_id=c.ASMAY_Id  inner join Adm_Student_Attendance_Periodwise k on k.asa_id=a.asa_id " +
                        " inner join adm_student_attendance_subjects l on l.asa_id=a.asa_id where  ASA_Activeflag=1 and  c.asmcl_id=" + attdto.ASMCL_Id + "  and c.ASMS_Id =" + attdto.ASMS_Id + " and c.ASMAY_Id =" + attdto.ASMAY_Id + " " +
                        "and a.MI_Id =" + attdto.MI_Id + " and a.ASA_FromDate ='" + confromdate + "' and  a.ASA_Att_Type ='" + attdto.monthflag1 + "' " +
                        " and d.AMST_ActiveFlag = 1 and d.AMST_SOL = 'S' and c.AMAY_ActiveFlag = 1  " + ordderby + " ";
                            _db.Database.OpenConnection();
                            using (var result1 = command.ExecuteReader())
                            {
                                while (result1.Read())
                                {
                                    obj.Add(new StudentAttTempDTO
                                    {
                                        amsT_Id = Convert.ToInt64(result1["AMST_Id"]),
                                        studentname = result1["studentname"].ToString(),
                                        amsT_AdmNo = result1["AMST_AdmNo"].ToString(),
                                        amaY_RollNo = Convert.ToInt64(result1["AMAY_RollNo"]),
                                        amsT_RegistrationNo = result1["amsT_RegistrationNo"].ToString(),
                                        pdays = Convert.ToDecimal(result1["ASA_Class_Attended"]),
                                        ASAS_Id = Convert.ToInt64(result1["ASAS_Id"]),
                                        asA_Id = Convert.ToInt64(result1["ASA_Id"]),
                                    });
                                }

                                studentList1 = obj.ToList();
                            }
                        }
                    }

                    for (int i = 0; i < result.Count; i++)
                    {
                        studentList1.Add(result[i]);
                    }

                    attdto.studentList = studentList1.ToArray();
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                    Console.Write(ex.Message);
                }
            }
            else
            {
                try
                {

                    if (attdto.ASMAY_Id != 0 && attdto.ASMCL_Id != 0)
                    {
                        var type = _db.AttendanceEntryTypeDMO.Where(t => t.ASMCL_Id.Equals(attdto.ASMCL_Id) && t.ASMAY_Id.Equals(attdto.ASMAY_Id) && t.MI_Id.Equals(attdto.MI_Id)).ToList();
                        if (type.Count == 0)
                        {
                            attdto.message = "Map The Attendance Entry Type For This Class i.e., Daily /Daily Twice/ Monthly";
                            return attdto;
                        }

                        attdto.ASA_Att_EntryType = type.FirstOrDefault().ASAET_Att_Type;
                    }

                    if (attdto.ASMAY_Id != 0 && attdto.ASMCL_Id != 0)
                    {
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GetStudentDataByAdecmicYearClassSection_New";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = attdto.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = attdto.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag });
                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = attdto.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = attdto.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar) { Value = confromdate });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        if (dataReader["studentname"] != System.DBNull.Value)
                                        {
                                            Student_name_null = Convert.ToString(dataReader["studentname"]);
                                        }
                                        else
                                        {
                                            Student_name_null = "NOT AVAILABLE";
                                        }


                                        if (dataReader["AMST_AdmNo"] != System.DBNull.Value)
                                        {
                                            AMST_ADM_null = Convert.ToString(dataReader["AMST_AdmNo"]);
                                        }
                                        else
                                        {
                                            AMST_ADM_null = "NOT AVAILABLE";
                                        }
                                        if (dataReader["amsT_RegistrationNo"] != System.DBNull.Value)
                                        {
                                            amsT_RegistrationNo = Convert.ToString(dataReader["amsT_RegistrationNo"]);
                                        }
                                        else
                                        {
                                            amsT_RegistrationNo = "NOT AVAILABLE";
                                        }

                                        arrAttdto.Add(new StudentAttTempDTO
                                        {
                                            amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                            studentname = Student_name_null,
                                            amsT_AdmNo = AMST_ADM_null,
                                            amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                            amsT_RegistrationNo = amsT_RegistrationNo
                                        });
                                        attdto.studentList = arrAttdto.ToArray();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                Console.Write(ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
                try
                {
                    if (attdto.classsecflag == "1")
                    {
                        var check_rolename1 = (from a in _db.MasterRoleType
                                               where (a.IVRMRT_Id == attdto.roleId)
                                               select new StudentAttendanceEntryDTO
                                               {
                                                   rolename = a.IVRMRT_Role,
                                               }).ToList();

                        var empcode_check1 = (from a in _db.Staff_User_Login
                                              where (a.MI_Id == attdto.MI_Id && a.Id.Equals(attdto.userId))
                                              select new StudentAttendanceEntryDTO
                                              {
                                                  Emp_Code = a.Emp_Code,
                                              }).ToList();


                        if (empcode_check1.Count() > 0)
                        {
                            attdto.sectionList = (from a in _db.Adm_SchAttLoginUserClass
                                                  from b in _db.Adm_SchAttLoginUser
                                                  from c in _db.School_M_Section
                                                  where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                                  && b.MI_Id == attdto.MI_Id && b.ASMAY_Id == attdto.ASMAY_Id
                                                  && b.HRME_Id == empcode_check1.FirstOrDefault().Emp_Code
                                                  && c.ASMC_ActiveFlag == 1 && a.ASMCL_Id == attdto.ASMCL_Id)
                                                  select new StudentAttendanceEntryDTO
                                                  {
                                                      ASMS_Id = c.ASMS_Id,
                                                      asmC_SectionName = c.ASMC_SectionName,
                                                  }).Distinct().ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("Student Attendance entry Get student data:" + ex.Message);
                }
            }
            if (attdto.checksubject == "1")
            {
                try
                {
                    var check_rolename1 = (from a in _db.MasterRoleType
                                           where (a.IVRMRT_Id == attdto.roleId)
                                           select new StudentAttendanceEntryDTO
                                           {
                                               rolename = a.IVRMRT_Role,
                                           }).ToList();

                    var empcode_check1 = (from a in _db.Staff_User_Login
                                          where (a.MI_Id == attdto.MI_Id && a.Id.Equals(attdto.userId))
                                          select new StudentAttendanceEntryDTO
                                          {
                                              Emp_Code = a.Emp_Code,
                                          }).ToList();

                    if (check_rolename1.FirstOrDefault().rolename.Equals("STAFF") || check_rolename1.FirstOrDefault().rolename.Equals("Staff"))
                    {
                        attdto.subjectList = (from a in _db.Adm_SchAttLoginUser
                                              from b in _db.Adm_SchAttLoginUserClass
                                              from c in _db.Adm_schAttLoginUserClassSubjects
                                              from d in _db.MasterSubjectList
                                              where d.ISMS_AttendanceFlag == true && a.ASALU_Id == b.ASALU_Id && b.ASALUC_Id == c.ASALUC_Id && d.ISMS_Id == c.ISMS_Id && a.HRME_Id == empcode_check1.FirstOrDefault().Emp_Code && a.MI_Id == attdto.MI_Id && a.ASMAY_Id == attdto.ASMAY_Id && b.ASMCL_Id == attdto.ASMCL_Id && b.ASMS_Id == attdto.ASMS_Id
                                              select new StudentAttendanceEntryDTO
                                              {
                                                  ismS_Id = c.ISMS_Id,
                                                  ismS_SubjectName = d.ISMS_SubjectName
                                              }).ToArray();
                    }
                    else
                    {
                        attdto.subjectList = (from a in _db.Adm_SchAttLoginUser
                                              from b in _db.Adm_SchAttLoginUserClass
                                              from c in _db.Adm_schAttLoginUserClassSubjects
                                              from d in _db.MasterSubjectList
                                              where d.ISMS_AttendanceFlag == true && a.ASALU_Id == b.ASALU_Id && b.ASALUC_Id == c.ASALUC_Id && d.ISMS_Id == c.ISMS_Id && a.MI_Id == attdto.MI_Id && a.ASMAY_Id == attdto.ASMAY_Id && b.ASMCL_Id == attdto.ASMCL_Id && b.ASMS_Id == attdto.ASMS_Id
                                              select new StudentAttendanceEntryDTO
                                              {
                                                  ismS_Id = c.ISMS_Id,
                                                  ismS_SubjectName = d.ISMS_SubjectName
                                              }).ToArray();
                    }
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("Student Attendance entry Get student with subjectlist data:" + ex.Message);

                }
            }

            return attdto;
        }
        public async Task<StudentAttendanceEntryDTO> SaveStudentAttendance(StudentAttendanceEntryDTO attdto)
        {
            try
            {
                if (attdto.userId > 0)
                {
                    var useridnew = attdto.userId;
                }
                else
                {
                    string id = "";
                    ApplicationUser user = new ApplicationUser();
                    user = await _userManager.FindByNameAsync(attdto.username);
                    if (user != null)
                    {
                        id = user.Id.ToString();
                        attdto.userId = Convert.ToInt64(id);
                    }
                }

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == attdto.MI_Id).ToList();

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == attdto.MI_Id && a.Id.Equals(attdto.userId))
                                     select new StudentAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                var emp_att_login_check = (from a in _db.attloginuser
                                           from c in _db.Adm_SchAttLoginUserClass
                                           from d in _db.Adm_schAttLoginUserClassSubjects
                                           where (a.ASALU_Id == c.ASALU_Id && c.ASALUC_Id == d.ASALUC_Id && a.MI_Id == attdto.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && c.ASMCL_Id == attdto.ASMCL_Id && c.ASMS_Id == attdto.ASMS_Id && d.ISMS_Id == attdto.ismS_Id)
                                           select new StudentAttendanceEntryDTO
                                           {
                                               ASALU_Id = a.ASALU_Id,
                                           }).ToList();
                if (emp_att_login_check.Count == 0)
                {
                    emp_att_login_check = (from a in _db.attloginuser
                                           from c in _db.Adm_SchAttLoginUserClass
                                           where (a.ASALU_Id == c.ASALU_Id && a.MI_Id == attdto.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && c.ASMCL_Id == attdto.ASMCL_Id && c.ASMS_Id == attdto.ASMS_Id)
                                           select new StudentAttendanceEntryDTO
                                           {
                                               ASALU_Id = a.ASALU_Id,
                                           }).ToList();
                }

                if (emp_att_login_check.Count == 0)
                {
                    attdto.message = "For This Staff There Is No Privileges To Enter Attendance.. Please Map The Staff In Attendance Privileges";
                    return attdto;
                }
                ////-------------------------Attendance Entry For Period Wise Start ---------------------/////
                if (attdto.ASA_Att_Type == "period")
                {
                    if (attdto.asasB_Id == 0)
                    {
                        //-------------------------When  Attendance Entry for not batchwise------------------//
                        _acdimpl.LogInformation("Attendance Entry Type Is Period");
                        if (attendance_entrytype.Count > 0 && attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type != null)
                        {
                            attdto.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;
                            //--------------Attendance Entry Type Is Absent------------//
                            if (attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "A")
                            {
                                _acdimpl.LogInformation("Attendance Entry Type Is Period and Attendance Entry type is Absent");
                                var isDuplicate = (from d in _db.Adm_studentAttendance
                                                   from b in _db.Adm_studentAttendanceStudents
                                                   from c in _db.Adm_studentAttendanceSubjects
                                                   from a in _db.Adm_StudentAttendancePeriodwiseDMO
                                                   where (a.ASA_Id == d.ASA_Id && b.ASA_Id == d.ASA_Id && c.ASA_Id == d.ASA_Id && d.MI_Id == attdto.MI_Id
                                                   && d.ASMAY_Id == attdto.ASMAY_Id && d.ASMCL_Id == attdto.ASMCL_Id && d.ASMS_Id == attdto.ASMS_Id
                                                   && d.ASA_FromDate == attdto.ASA_FromDate && d.ASA_Att_Type == attdto.ASA_Att_Type && c.ISMS_Id == attdto.ismS_Id
                                                   && a.TTMP_Id == Convert.ToInt64(attdto.TTMP_Id) && d.ASA_Activeflag == true)
                                                   select new StudentAttendanceEntryDTO
                                                   {
                                                       ASA_Id = d.ASA_Id

                                                   }).Distinct().ToList();
                                if (isDuplicate.Count > 0)
                                {
                                    //attendance update code for absent Period Wise
                                    for (int i = 0; i < attdto.stdList.Count(); i++)
                                    {
                                        if (i == 0) //**updating the adm_student_attendance when entry type change
                                        {
                                            var result = _db.Adm_studentAttendance.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id);
                                            result.ASA_Att_EntryType = "Absent";
                                            result.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                            result.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                            result.UpdatedDate = indianTime;
                                            result.ASA_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendance.Update(result);
                                        }
                                        //**start** if condition is for updating the attendance once it given not for new student who join that date with out any attendance
                                        if (attdto.stdList[i].ASAS_Id != null)
                                        {
                                            if (i == 0)
                                            {
                                                var result1 = _db.Adm_StudentAttendancePeriodwiseDMO.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id
                                                && d.TTMP_Id == Convert.ToInt32(attdto.TTMP_Id));
                                                result1.UpdatedDate = indianTime;
                                                result1.ASAP_UpdatedBy = attdto.userId;
                                                _db.Adm_StudentAttendancePeriodwiseDMO.Update(result1);

                                                var result2 = _db.Adm_studentAttendanceSubjects.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id
                                                && d.ISMS_Id == attdto.ismS_Id);
                                                result2.UpdatedDate = indianTime;
                                                result2.ASASU_UpdatedBy = attdto.userId;
                                                _db.Adm_studentAttendanceSubjects.Update(result2);
                                            }

                                            var result3 = _db.Adm_studentAttendanceStudents.Single(a => a.ASAS_Id == attdto.stdList[i].ASAS_Id && a.AMST_Id == attdto.stdList[i].amsT_Id);
                                            Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.stdList[i]);

                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == false)
                                                {
                                                    result3.ASA_AttendanceFlag = "Present";
                                                    result3.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                                }
                                                else if (attdto.stdList[i].selected == true)
                                                {
                                                    result3.ASA_AttendanceFlag = "Absent";
                                                    result3.ASA_Class_Attended = 0;
                                                }
                                            }                                            
                                            result3.UpdatedDate = indianTime;
                                            result3.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Update(result3);
                                        }
                                        //------Inserting New Student Details If Already Attendance Entry Is Happend--//
                                        else
                                        {
                                            Adm_studentAttendanceStudents stdperiod = new Adm_studentAttendanceStudents();

                                            stdperiod.ASA_Id = isDuplicate.FirstOrDefault().ASA_Id;
                                            stdperiod.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    stdperiod.ASA_AttendanceFlag = "Absent";
                                                    stdperiod.ASA_Class_Attended = 0;
                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    stdperiod.ASA_AttendanceFlag = "Present";
                                                    stdperiod.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                                }
                                            }
                                            stdperiod.CreatedDate = indianTime;
                                            stdperiod.UpdatedDate = indianTime;
                                            stdperiod.ASAS_UpdatedBy = attdto.userId;
                                            stdperiod.ASAS_CreatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(stdperiod);
                                        }
                                    }
                                    int n = _db.SaveChanges();
                                    if (n > 0)
                                    {
                                        attdto.returnval = true;
                                    }
                                    else
                                    {
                                        attdto.returnval = false;
                                    }
                                }
                                else
                                {
                                    //attendance insert code for absent Period Wise
                                    Adm_studentAttendance enq = new Adm_studentAttendance();
                                    enq.MI_Id = attdto.MI_Id;
                                    enq.ASMAY_Id = attdto.ASMAY_Id;
                                    enq.ASA_Att_Type = attdto.ASA_Att_Type;
                                    enq.ASA_Att_EntryType = "Absent";
                                    enq.ASMCL_Id = attdto.ASMCL_Id;
                                    enq.ASMS_Id = attdto.ASMS_Id;
                                    enq.IMP_Id = Convert.ToInt64(attdto.ISMP_Id);
                                    enq.ASA_Entry_DateTime = attdto.ASA_Entry_DateTime;
                                    enq.ASA_FromDate = attdto.ASA_FromDate;
                                    enq.ASA_ToDate = attdto.ASA_ToDate;
                                    enq.ASA_ClassHeld = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                    enq.ASA_Network_IP = attdto.ASA_Network_IP;
                                    enq.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                    enq.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                    enq.ASA_Regular_Extra = attdto.ASA_Regular_Extra;
                                    enq.CreatedDate = indianTime;
                                    enq.UpdatedDate = indianTime;
                                    enq.ASA_CreatedBy = attdto.userId;
                                    enq.ASA_UpdatedBy = attdto.userId;
                                    enq.ASA_Activeflag = true;
                                    _db.Adm_studentAttendance.Add(enq);
                                    if (attdto.stdList != null && attdto.stdList.Count() > 0)
                                    {
                                        Adm_studentAttendanceSubjects attsubjects = new Adm_studentAttendanceSubjects();
                                        attsubjects.ASA_Id = enq.ASA_Id;
                                        attsubjects.ISMS_Id = attdto.ismS_Id;
                                        attsubjects.CreatedDate = indianTime;
                                        attsubjects.UpdatedDate = indianTime;
                                        attsubjects.ASASU_UpdatedBy = attdto.userId;
                                        attsubjects.ASASU_CreatedBy = attdto.userId;
                                        _db.Adm_studentAttendanceSubjects.Add(attsubjects);


                                        Adm_StudentAttendancePeriodwiseDMO attperiodwise = new Adm_StudentAttendancePeriodwiseDMO();
                                        attperiodwise.ASA_Id = enq.ASA_Id;
                                        attperiodwise.TTMP_Id = attdto.TTMP_Id;
                                        attperiodwise.CreatedDate = indianTime;
                                        attperiodwise.UpdatedDate = indianTime;
                                        attperiodwise.ASAP_UpdatedBy = attdto.userId;
                                        attperiodwise.ASAP_CreatedBy = attdto.userId;
                                        try
                                        {
                                            _db.Adm_StudentAttendancePeriodwiseDMO.Add(attperiodwise);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }

                                        for (int i = 0; i < attdto.stdList.Count(); i++)
                                        {
                                            Adm_studentAttendanceStudents std = new Adm_studentAttendanceStudents();
                                            std.ASA_Id = enq.ASA_Id;

                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == false)
                                                {
                                                    std.ASA_AttendanceFlag = "Present";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);

                                                }
                                                else if (attdto.stdList[i].selected == true)
                                                {
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    std.ASA_Class_Attended = 0;
                                                }
                                            }
                                            std.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            std.CreatedDate = indianTime;
                                            std.UpdatedDate = indianTime;
                                            std.ASAS_UpdatedBy = attdto.userId;
                                            std.ASAS_CreatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(std);
                                        }
                                        var contactExists = _db.SaveChanges();
                                        if (contactExists >= 1)
                                        {
                                            attdto.returnval = true;
                                        }
                                        else
                                        {
                                            attdto.returnval = false;
                                        }
                                    }
                                }
                            }
                            //--------------Attendance Entry Type Is Present------------//
                            else if (attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "P")
                            {
                                _acdimpl.LogInformation("Attendance Entry Type Is Period and Attendance Entry type is Present");
                                var isDuplicate = (from d in _db.Adm_studentAttendance
                                                   from b in _db.Adm_studentAttendanceStudents
                                                   from c in _db.Adm_studentAttendanceSubjects
                                                   from a in _db.Adm_StudentAttendancePeriodwiseDMO
                                                   where (a.ASA_Id == d.ASA_Id && b.ASA_Id == d.ASA_Id && c.ASA_Id == d.ASA_Id && d.MI_Id == attdto.MI_Id && d.ASMAY_Id == attdto.ASMAY_Id && d.ASMCL_Id == attdto.ASMCL_Id && d.ASMS_Id == attdto.ASMS_Id && d.ASA_FromDate == attdto.ASA_FromDate && d.ASA_Att_Type == attdto.ASA_Att_Type && c.ISMS_Id == attdto.ismS_Id && a.TTMP_Id == attdto.TTMP_Id && d.ASA_Activeflag == true)
                                                   select new StudentAttendanceEntryDTO
                                                   {
                                                       ASA_Id = d.ASA_Id

                                                   }).Distinct().ToList();
                                if (isDuplicate.Count > 0)
                                {
                                    //attendance update code for Present Period Wise
                                    for (int i = 0; i < attdto.stdList.Count(); i++)
                                    {
                                        if (i == 0) //**updating the adm_student_attendance when entry type change
                                        {
                                            var result = _db.Adm_studentAttendance.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id);
                                            result.ASA_Att_EntryType = "Present";
                                            result.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                            result.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                            result.UpdatedDate = indianTime;
                                            result.ASA_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendance.Update(result);
                                        }
                                        //**start** if condition is for updating the attendance once it given not for new student who join that date with out any attendance
                                        if (attdto.stdList[i].ASAS_Id != null)
                                        {
                                            if (i == 0)
                                            {
                                                var ttttmp = Convert.ToInt32(attdto.TTMP_Id);

                                                var result1 = _db.Adm_StudentAttendancePeriodwiseDMO.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id 
                                                && d.TTMP_Id == ttttmp);
                                                result1.UpdatedDate = indianTime;
                                                result1.ASAP_UpdatedBy = attdto.userId;
                                                _db.Adm_StudentAttendancePeriodwiseDMO.Update(result1);

                                                var result2 = _db.Adm_studentAttendanceSubjects.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id 
                                                && d.ISMS_Id == attdto.ismS_Id);
                                                result2.UpdatedDate = indianTime;
                                                result2.ASASU_UpdatedBy = attdto.userId;
                                                _db.Adm_studentAttendanceSubjects.Update(result2);
                                            }

                                            var result3 = _db.Adm_studentAttendanceStudents.Single(a => a.ASAS_Id == attdto.stdList[i].ASAS_Id && a.AMST_Id == attdto.stdList[i].amsT_Id);
                                            Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.stdList[i]);

                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    result3.ASA_AttendanceFlag = "Present";
                                                    result3.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);

                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    result3.ASA_AttendanceFlag = "Absent";
                                                    result3.ASA_Class_Attended = 0;
                                                }
                                            }
                                            result3.CreatedDate = result3.CreatedDate;
                                            result3.UpdatedDate = indianTime;
                                            result3.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Update(result3);
                                        }
                                        //------Inserting New Student Details If Already Attendance Entry Is Happend--//
                                        else
                                        {
                                            Adm_studentAttendanceStudents stdperiod = new Adm_studentAttendanceStudents();

                                            stdperiod.ASA_Id = isDuplicate.FirstOrDefault().ASA_Id;
                                            stdperiod.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == false)
                                                {
                                                    stdperiod.ASA_AttendanceFlag = "Absent";
                                                    stdperiod.ASA_Class_Attended = 0;
                                                }
                                                else if (attdto.stdList[i].selected == true)
                                                {
                                                    stdperiod.ASA_AttendanceFlag = "Present";
                                                    stdperiod.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                                }
                                            }
                                            stdperiod.CreatedDate = indianTime;
                                            stdperiod.UpdatedDate = indianTime;
                                            stdperiod.ASAS_CreatedBy = attdto.userId;
                                            stdperiod.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(stdperiod);
                                        }
                                    }
                                    int n = _db.SaveChanges();
                                    if (n > 0)
                                    {
                                        attdto.returnval = true;
                                    }
                                    else
                                    {
                                        attdto.returnval = false;
                                    }
                                }
                                else
                                {
                                    //attendance insert code for Present Period Wise
                                    Adm_studentAttendance enq = new Adm_studentAttendance();
                                    enq.MI_Id = attdto.MI_Id;
                                    enq.ASMAY_Id = attdto.ASMAY_Id;
                                    enq.ASA_Att_Type = attdto.ASA_Att_Type;
                                    enq.ASA_Att_EntryType = "Present";
                                    enq.ASMCL_Id = attdto.ASMCL_Id;
                                    enq.ASMS_Id = attdto.ASMS_Id;
                                    enq.IMP_Id = Convert.ToInt64(attdto.ISMP_Id);
                                    enq.ASA_Entry_DateTime = attdto.ASA_Entry_DateTime;
                                    enq.ASA_FromDate = attdto.ASA_FromDate;
                                    enq.ASA_ToDate = attdto.ASA_ToDate;
                                    enq.ASA_ClassHeld = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                    enq.ASA_Network_IP = attdto.ASA_Network_IP;
                                    enq.ASA_Regular_Extra = attdto.ASA_Regular_Extra;
                                    enq.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                    enq.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                    enq.CreatedDate = indianTime;
                                    enq.UpdatedDate = indianTime;
                                    enq.ASA_CreatedBy = attdto.userId;
                                    enq.ASA_UpdatedBy = attdto.userId;
                                    enq.ASA_Activeflag = true;

                                    _db.Adm_studentAttendance.Add(enq);
                                    if (attdto.stdList != null && attdto.stdList.Count() > 0)
                                    {
                                        Adm_studentAttendanceSubjects attsubjects = new Adm_studentAttendanceSubjects();
                                        attsubjects.ASA_Id = enq.ASA_Id;
                                        attsubjects.ISMS_Id = attdto.ismS_Id;
                                        attsubjects.CreatedDate = indianTime;
                                        attsubjects.UpdatedDate = indianTime;
                                        attsubjects.ASASU_CreatedBy = attdto.userId;
                                        attsubjects.ASASU_UpdatedBy = attdto.userId;
                                        _db.Adm_studentAttendanceSubjects.Add(attsubjects);

                                        Adm_StudentAttendancePeriodwiseDMO attperiodwise = new Adm_StudentAttendancePeriodwiseDMO();
                                        attperiodwise.ASA_Id = enq.ASA_Id;
                                        attperiodwise.TTMP_Id = attdto.TTMP_Id;
                                        attperiodwise.CreatedDate = indianTime;
                                        attperiodwise.UpdatedDate = indianTime;
                                        attperiodwise.ASAP_CreatedBy = attdto.userId;
                                        attperiodwise.ASAP_CreatedBy = attdto.userId;
                                        try
                                        {
                                            _db.Adm_StudentAttendancePeriodwiseDMO.Add(attperiodwise);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }


                                        for (int i = 0; i < attdto.stdList.Count(); i++)
                                        {
                                            Adm_studentAttendanceStudents std = new Adm_studentAttendanceStudents();
                                            std.ASA_Id = enq.ASA_Id;

                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    std.ASA_AttendanceFlag = "Present";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);

                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    std.ASA_Class_Attended = 0;
                                                }
                                            }

                                            std.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            std.CreatedDate = indianTime;
                                            std.UpdatedDate = indianTime;
                                            std.ASAS_CreatedBy = attdto.userId;
                                            std.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(std);
                                        }
                                        var contactExists = _db.SaveChanges();
                                        if (contactExists >= 1)
                                        {
                                            attdto.returnval = true;
                                        }
                                        else
                                        {
                                            attdto.returnval = false;
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            attdto.message = "Attendance Entry Type Flag Is Not Mapped Please Contact Administrator...";
                            return attdto;
                        }
                    }

                    //--------------When Batch Is Selected----------------//

                    else
                    {
                        _acdimpl.LogInformation("Attendance Entry Type Is Period");
                        if (attendance_entrytype.Count > 0 && attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type != null)
                        {
                            attdto.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;
                            //--------------Attendance Entry Type Is Absent------------//
                            if (attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "A")
                            {
                                _acdimpl.LogInformation("Attendance Entry Type Is Period and Attendance Entry type is Absent Batchwise");
                                var isDuplicate = (from d in _db.Adm_studentAttendance
                                                   from b in _db.Adm_studentAttendanceStudents
                                                   from c in _db.Adm_studentAttendanceSubjects
                                                   from a in _db.Adm_StudentAttendancePeriodwiseDMO
                                                   from e in _db.AdmSchoolAttendanceSubjectBatch
                                                   from f in _db.AdmSchoolAttendanceSubjectBatchStudents
                                                   where (a.ASA_Id == d.ASA_Id && b.ASA_Id == d.ASA_Id && c.ASA_Id == d.ASA_Id && d.MI_Id == attdto.MI_Id &&
                                                   f.AMST_Id == b.AMST_Id && f.ASASB_Id == e.ASASB_Id && d.ASMAY_Id == attdto.ASMAY_Id
                                                   && d.ASMCL_Id == attdto.ASMCL_Id && d.ASMS_Id == attdto.ASMS_Id && d.ASA_FromDate == attdto.ASA_FromDate &&
                                                   d.ASA_Att_Type == attdto.ASA_Att_Type && c.ISMS_Id == attdto.ismS_Id && a.TTMP_Id == Convert.ToInt64(attdto.TTMP_Id) && e.ASASB_Id == attdto.asasB_Id && d.ASA_Activeflag == true)
                                                   select new StudentAttendanceEntryDTO
                                                   {
                                                       ASA_Id = d.ASA_Id

                                                   }).Distinct().ToList();
                                if (isDuplicate.Count > 0)
                                {
                                    for (int i = 0; i < attdto.stdList.Count(); i++)
                                    {
                                        if (i == 0) //**updating the adm_student_attendance when entry type change
                                        {
                                            var result = _db.Adm_studentAttendance.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id);
                                            result.ASA_Att_EntryType = "Absent";
                                            result.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                            result.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                            result.UpdatedDate = indianTime;
                                            result.ASA_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendance.Update(result);
                                        }
                                        //**start** if condition is for updating the attendance once it given not for new student who join that date with out any attendance
                                        if (attdto.stdList[i].ASAS_Id != null)
                                        {
                                            if (i == 0)
                                            {
                                                var result1 = _db.Adm_StudentAttendancePeriodwiseDMO.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id && d.TTMP_Id == Convert.ToInt32(attdto.TTMP_Id));
                                                result1.UpdatedDate = indianTime;
                                                result1.ASAP_UpdatedBy = attdto.userId;
                                                _db.Adm_StudentAttendancePeriodwiseDMO.Update(result1);

                                                var result2 = _db.Adm_studentAttendanceSubjects.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id && d.ISMS_Id == attdto.ismS_Id);
                                                result2.UpdatedDate = indianTime;
                                                result2.ASASU_UpdatedBy = attdto.userId;
                                                _db.Adm_studentAttendanceSubjects.Update(result2);
                                            }

                                            var result3 = _db.Adm_studentAttendanceStudents.Single(a => a.ASAS_Id == attdto.stdList[i].ASAS_Id && a.AMST_Id == attdto.stdList[i].amsT_Id);
                                            Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.stdList[i]);

                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == false)
                                                {
                                                    result3.ASA_AttendanceFlag = "Present";
                                                    result3.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);

                                                }
                                                else if (attdto.stdList[i].selected == true)
                                                {
                                                    result3.ASA_AttendanceFlag = "Absent";
                                                    result3.ASA_Class_Attended = 0;
                                                }
                                            }
                                            result3.UpdatedDate = indianTime;
                                            result3.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Update(result3);
                                        }
                                        //------Inserting New Student Details If Already Attendance Entry Is Happend--//
                                        else
                                        {
                                            Adm_studentAttendanceStudents stdperiod = new Adm_studentAttendanceStudents();

                                            stdperiod.ASA_Id = isDuplicate.FirstOrDefault().ASA_Id;
                                            stdperiod.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    stdperiod.ASA_AttendanceFlag = "Absent";
                                                    stdperiod.ASA_Class_Attended = 0;
                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    stdperiod.ASA_AttendanceFlag = "Present";
                                                    stdperiod.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                                }
                                            }
                                            stdperiod.CreatedDate = indianTime;
                                            stdperiod.UpdatedDate = indianTime;
                                            stdperiod.ASAS_CreatedBy = attdto.userId;
                                            stdperiod.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(stdperiod);
                                        }
                                    }
                                    int n = _db.SaveChanges();
                                    if (n > 0)
                                    {
                                        attdto.returnval = true;
                                    }
                                    else
                                    {
                                        attdto.returnval = false;
                                    }
                                }
                                else
                                {
                                    //attendance insert code for absent Period Wise
                                    Adm_studentAttendance enq = new Adm_studentAttendance();
                                    enq.MI_Id = attdto.MI_Id;
                                    enq.ASMAY_Id = attdto.ASMAY_Id;
                                    enq.ASA_Att_Type = attdto.ASA_Att_Type;
                                    enq.ASA_Att_EntryType = "Absent";
                                    enq.ASMCL_Id = attdto.ASMCL_Id;
                                    enq.ASMS_Id = attdto.ASMS_Id;
                                    enq.IMP_Id = Convert.ToInt64(attdto.ISMP_Id);
                                    enq.ASA_Entry_DateTime = attdto.ASA_Entry_DateTime;
                                    enq.ASA_FromDate = attdto.ASA_FromDate;
                                    enq.ASA_ToDate = attdto.ASA_ToDate;
                                    enq.ASA_ClassHeld = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                    enq.ASA_Network_IP = attdto.ASA_Network_IP;
                                    enq.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                    enq.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                    enq.ASA_Regular_Extra = attdto.ASA_Regular_Extra;
                                    enq.CreatedDate = indianTime;
                                    enq.UpdatedDate = indianTime;
                                    enq.ASA_CreatedBy = attdto.userId;
                                    enq.ASA_UpdatedBy = attdto.userId;
                                    enq.ASA_Activeflag = true;
                                    _db.Adm_studentAttendance.Add(enq);
                                    if (attdto.stdList != null && attdto.stdList.Count() > 0)
                                    {
                                        Adm_studentAttendanceSubjects attsubjects = new Adm_studentAttendanceSubjects();
                                        attsubjects.ASA_Id = enq.ASA_Id;
                                        attsubjects.ISMS_Id = attdto.ismS_Id;
                                        attsubjects.CreatedDate = indianTime;
                                        attsubjects.UpdatedDate = indianTime;
                                        attsubjects.ASASU_CreatedBy = attdto.userId;
                                        attsubjects.ASASU_UpdatedBy = attdto.userId;
                                        _db.Adm_studentAttendanceSubjects.Add(attsubjects);


                                        Adm_StudentAttendancePeriodwiseDMO attperiodwise = new Adm_StudentAttendancePeriodwiseDMO();
                                        attperiodwise.ASA_Id = enq.ASA_Id;
                                        attperiodwise.TTMP_Id = attdto.TTMP_Id;
                                        attperiodwise.CreatedDate = indianTime;
                                        attperiodwise.UpdatedDate = indianTime;
                                        attperiodwise.ASAP_CreatedBy = attdto.userId;
                                        attperiodwise.ASAP_UpdatedBy = attdto.userId;
                                        try
                                        {
                                            _db.Adm_StudentAttendancePeriodwiseDMO.Add(attperiodwise);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }

                                        for (int i = 0; i < attdto.stdList.Count(); i++)
                                        {
                                            Adm_studentAttendanceStudents std = new Adm_studentAttendanceStudents();
                                            std.ASA_Id = enq.ASA_Id;

                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == false)
                                                {
                                                    std.ASA_AttendanceFlag = "Present";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);

                                                }
                                                else if (attdto.stdList[i].selected == true)
                                                {
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    std.ASA_Class_Attended = 0;
                                                }
                                            }
                                            std.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            std.CreatedDate = indianTime;
                                            std.UpdatedDate = indianTime;
                                            std.ASAS_CreatedBy = attdto.userId;
                                            std.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(std);
                                        }
                                        var contactExists = _db.SaveChanges();
                                        if (contactExists >= 1)
                                        {
                                            attdto.returnval = true;
                                        }
                                        else
                                        {
                                            attdto.returnval = false;
                                        }
                                    }
                                }
                            }
                            //-----------Attendance Entry Type Is Present--------//
                            else if (attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "P")
                            {
                                _acdimpl.LogInformation("Attendance Entry Type Is Period and Attendance Entry type is Present Batchwise");
                                var isDuplicate = (from d in _db.Adm_studentAttendance
                                                   from b in _db.Adm_studentAttendanceStudents
                                                   from c in _db.Adm_studentAttendanceSubjects
                                                   from a in _db.Adm_StudentAttendancePeriodwiseDMO
                                                   from e in _db.AdmSchoolAttendanceSubjectBatch
                                                   from f in _db.AdmSchoolAttendanceSubjectBatchStudents
                                                   where (a.ASA_Id == d.ASA_Id && b.ASA_Id == d.ASA_Id && c.ASA_Id == d.ASA_Id && d.MI_Id == attdto.MI_Id &&
                                                   e.ASASB_Id == f.ASASB_Id && f.AMST_Id == b.AMST_Id && d.ASMAY_Id == attdto.ASMAY_Id
                                                   && d.ASMCL_Id == attdto.ASMCL_Id && d.ASMS_Id == attdto.ASMS_Id && d.ASA_FromDate == attdto.ASA_FromDate &&
                                                   d.ASA_Att_Type == attdto.ASA_Att_Type && c.ISMS_Id == attdto.ismS_Id && a.TTMP_Id == Convert.ToInt64(attdto.TTMP_Id) && f.ASASB_Id == attdto.asasB_Id && d.ASA_Activeflag == true)
                                                   select new StudentAttendanceEntryDTO
                                                   {
                                                       ASA_Id = d.ASA_Id

                                                   }).Distinct().ToList();
                                if (isDuplicate.Count > 0)
                                {
                                    for (int i = 0; i < attdto.stdList.Count(); i++)
                                    {
                                        if (i == 0) //**updating the adm_student_attendance when entry type change
                                        {
                                            var result = _db.Adm_studentAttendance.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id);
                                            result.ASA_Att_EntryType = "Present";
                                            result.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                            result.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                            result.UpdatedDate = indianTime;
                                            result.ASA_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendance.Update(result);
                                        }
                                        //**start** if condition is for updating the attendance once it given not for new student who join that date with out any attendance
                                        if (attdto.stdList[i].ASAS_Id != null)
                                        {
                                            if (i == 0)
                                            {
                                                var result1 = _db.Adm_StudentAttendancePeriodwiseDMO.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id && d.TTMP_Id == Convert.ToInt32(attdto.TTMP_Id));
                                                result1.UpdatedDate = indianTime;
                                                result1.ASAP_UpdatedBy = attdto.userId;
                                                _db.Adm_StudentAttendancePeriodwiseDMO.Update(result1);

                                                var result2 = _db.Adm_studentAttendanceSubjects.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id && d.ISMS_Id == attdto.ismS_Id);
                                                result2.UpdatedDate = indianTime;
                                                result2.ASASU_UpdatedBy = attdto.userId;
                                                _db.Adm_studentAttendanceSubjects.Update(result2);
                                            }

                                            var result3 = _db.Adm_studentAttendanceStudents.Single(a => a.ASAS_Id == attdto.stdList[i].ASAS_Id && a.AMST_Id == attdto.stdList[i].amsT_Id);
                                            Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.stdList[i]);

                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    result3.ASA_AttendanceFlag = "Present";
                                                    result3.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);

                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    result3.ASA_AttendanceFlag = "Absent";
                                                    result3.ASA_Class_Attended = 0;
                                                }
                                            }                                           
                                            result3.UpdatedDate = indianTime;
                                            result3.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Update(result3);
                                        }
                                        //------Inserting New Student Details If Already Attendance Entry Is Happend--//
                                        else
                                        {
                                            Adm_studentAttendanceStudents stdperiod = new Adm_studentAttendanceStudents();

                                            stdperiod.ASA_Id = isDuplicate.FirstOrDefault().ASA_Id;
                                            stdperiod.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == false)
                                                {
                                                    stdperiod.ASA_AttendanceFlag = "Absent";
                                                    stdperiod.ASA_Class_Attended = 0;
                                                }
                                                else if (attdto.stdList[i].selected == true)
                                                {
                                                    stdperiod.ASA_AttendanceFlag = "Present";
                                                    stdperiod.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                                }
                                            }
                                            stdperiod.CreatedDate = indianTime;
                                            stdperiod.UpdatedDate = indianTime;
                                            stdperiod.ASAS_CreatedBy = attdto.userId;
                                            stdperiod.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(stdperiod);
                                        }
                                    }
                                    int n = _db.SaveChanges();
                                    if (n > 0)
                                    {
                                        attdto.returnval = true;
                                    }
                                    else
                                    {
                                        attdto.returnval = false;
                                    }
                                }
                                else
                                {
                                    //attendance insert code for absent Period Wise
                                    Adm_studentAttendance enq = new Adm_studentAttendance();
                                    enq.MI_Id = attdto.MI_Id;
                                    enq.ASMAY_Id = attdto.ASMAY_Id;
                                    enq.ASA_Att_Type = attdto.ASA_Att_Type;
                                    enq.ASA_Att_EntryType = "Present";
                                    enq.ASMCL_Id = attdto.ASMCL_Id;
                                    enq.ASMS_Id = attdto.ASMS_Id;
                                    enq.IMP_Id = Convert.ToInt64(attdto.ISMP_Id);
                                    enq.ASA_Entry_DateTime = attdto.ASA_Entry_DateTime;
                                    enq.ASA_FromDate = attdto.ASA_FromDate;
                                    enq.ASA_ToDate = attdto.ASA_ToDate;
                                    enq.ASA_ClassHeld = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                    enq.ASA_Network_IP = attdto.ASA_Network_IP;
                                    enq.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                    enq.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                    enq.ASA_Regular_Extra = attdto.ASA_Regular_Extra;
                                    enq.CreatedDate = indianTime;
                                    enq.UpdatedDate = indianTime;
                                    enq.ASA_CreatedBy = attdto.userId;
                                    enq.ASA_UpdatedBy = attdto.userId;
                                    enq.ASA_Activeflag = true;
                                    _db.Adm_studentAttendance.Add(enq);
                                    if (attdto.stdList != null && attdto.stdList.Count() > 0)
                                    {
                                        Adm_studentAttendanceSubjects attsubjects = new Adm_studentAttendanceSubjects();
                                        attsubjects.ASA_Id = enq.ASA_Id;
                                        attsubjects.ISMS_Id = attdto.ismS_Id;
                                        attsubjects.CreatedDate = indianTime;
                                        attsubjects.UpdatedDate = indianTime;
                                        attsubjects.ASASU_CreatedBy = attdto.userId;
                                        attsubjects.ASASU_UpdatedBy = attdto.userId;
                                        _db.Adm_studentAttendanceSubjects.Add(attsubjects);


                                        Adm_StudentAttendancePeriodwiseDMO attperiodwise = new Adm_StudentAttendancePeriodwiseDMO();
                                        attperiodwise.ASA_Id = enq.ASA_Id;
                                        attperiodwise.TTMP_Id = attdto.TTMP_Id;
                                        attperiodwise.CreatedDate = indianTime;
                                        attperiodwise.UpdatedDate = indianTime;
                                        attperiodwise.ASAP_CreatedBy = attdto.userId;
                                        attperiodwise.ASAP_UpdatedBy = attdto.userId;
                                        try
                                        {
                                            _db.Adm_StudentAttendancePeriodwiseDMO.Add(attperiodwise);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }

                                        for (int i = 0; i < attdto.stdList.Count(); i++)
                                        {
                                            Adm_studentAttendanceStudents std = new Adm_studentAttendanceStudents();
                                            std.ASA_Id = enq.ASA_Id;

                                            if (attdto.ASA_Att_Type == "period")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    std.ASA_AttendanceFlag = "Present";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);

                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    std.ASA_Class_Attended = 0;
                                                }
                                            }
                                            std.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            std.CreatedDate = indianTime;
                                            std.UpdatedDate = indianTime;
                                            std.ASAS_UpdatedBy = attdto.userId;
                                            std.ASAS_CreatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(std);
                                        }
                                        var contactExists = _db.SaveChanges();
                                        if (contactExists >= 1)
                                        {
                                            attdto.returnval = true;
                                        }
                                        else
                                        {
                                            attdto.returnval = false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            attdto.message = "Attendance Entry Type Flag Is Not Mapped Please Contact Administrator...";
                            return attdto;
                        }
                    }

                }
                ////------------------------------------End--------------------///////

                ////-------------------------Attendance Entry For Daily Once , Daily Twice And Monthly---------------------/////
                else
                {
                    _acdimpl.LogInformation("Attendance Entry Type Is Daily , Daily Twice , Monthly");
                    //attendance entry type is  absent 
                    if (attendance_entrytype.Count > 0 && attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type != null)
                    {
                        attdto.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;
                        if (attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "A")
                        {
                            if (attdto.ASA_Id == 0)
                            {
                                var isDuplicate = _db.Adm_studentAttendance.Where(d => d.MI_Id == attdto.MI_Id && d.ASMAY_Id == attdto.ASMAY_Id && d.ASMCL_Id == attdto.ASMCL_Id && d.ASMS_Id == attdto.ASMS_Id && d.ASA_FromDate == attdto.ASA_FromDate && d.ASA_Att_Type == attdto.ASA_Att_Type && d.ASA_Activeflag == true).ToList();
                                if (isDuplicate.Count > 0)
                                {
                                    //attendance update code for absent
                                    for (int i = 0; i < attdto.stdList.Count(); i++)
                                    {
                                        if (i == 0) //**updating the adm_student_attendance when entry type change
                                        {
                                            var result = _db.Adm_studentAttendance.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id);
                                            result.ASA_Att_EntryType = "Absent";
                                            result.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                            result.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                            result.ASA_ClassHeld = Convert.ToDecimal(attdto.ASA_ClassHeld);                                            
                                            result.UpdatedDate = indianTime;
                                            result.ASA_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendance.Update(result);
                                        }
                                        //**start** if condition is for updating the attendance once it given not for new student who join that date with out any attendance
                                        if (attdto.stdList[i].ASAS_Id != null)
                                        {
                                            var result = _db.Adm_studentAttendanceStudents.Single(a => a.ASAS_Id == attdto.stdList[i].ASAS_Id && a.AMST_Id == attdto.stdList[i].amsT_Id);
                                            Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.stdList[i]);

                                            if (attdto.ASA_Att_Type == "Dailyonce")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    result.ASA_AttendanceFlag = "Absent";
                                                    result.ASA_Class_Attended = 0;
                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    result.ASA_AttendanceFlag = "Present";
                                                    result.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);

                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "Dailytwice")
                                            {
                                                string classheld = "";
                                                if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    result.ASA_Dailytwice_Flag = "Secondhalf";
                                                    result.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    result.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].SecondHalfflag == true && attdto.stdList[i].FirstHalfflag == false)
                                                {
                                                    result.ASA_Dailytwice_Flag = "firsthalf";
                                                    result.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    result.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == true)
                                                {
                                                    result.ASA_Dailytwice_Flag = "Absent";
                                                    result.ASA_AttendanceFlag = "Absent";
                                                    classheld = "0.0";
                                                    result.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == false && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    result.ASA_Dailytwice_Flag = "Present";
                                                    result.ASA_AttendanceFlag = "Present";
                                                    classheld = "1.0";
                                                    result.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "monthly")
                                            {
                                                result.ASA_AttendanceFlag = "Present";
                                                result.ASA_Class_Attended = Convert.ToDecimal(attdto.stdList[i].pdays);
                                            }                                                                                   
                                            result.UpdatedDate = indianTime;
                                            result.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Update(result);
                                            var contactExists = _db.SaveChanges();

                                            if (contactExists >= 1)
                                            {
                                                attdto.returnval = true;
                                            }
                                            else
                                            {
                                                attdto.returnval = false;
                                            }
                                        }
                                        //**--end--** if condition is for updating the attendance once it given not for new student who join that date with out any attendance
                                        // ** start ** updating the attendance of the new join student on that date once attendance entry is already done it for new student again inserting the details
                                        else
                                        {
                                            //  Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.stdList[i]);
                                            Adm_studentAttendanceStudents std = new Adm_studentAttendanceStudents();

                                            std.ASA_Id = isDuplicate.FirstOrDefault().ASA_Id;
                                            std.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            if (attdto.ASA_Att_Type == "Dailyonce")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    std.ASA_Class_Attended = 0;

                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    std.ASA_AttendanceFlag = "Present";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "Dailytwice")
                                            {
                                                string classheld = "";
                                                if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Secondhalf";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].SecondHalfflag == true && attdto.stdList[i].FirstHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "firsthalf";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == true)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Absent";
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    classheld = "0.0";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == false && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Present";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "1.0";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "monthly")
                                            {
                                                std.ASA_AttendanceFlag = "Present";
                                                std.ASA_Class_Attended = Convert.ToDecimal(attdto.stdList[i].pdays);
                                            }
                                            std.CreatedDate = indianTime;
                                            std.UpdatedDate = indianTime;
                                            std.ASAS_CreatedBy = attdto.userId;
                                            std.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(std);

                                            var contactExists = _db.SaveChanges();

                                            if (contactExists >= 1)
                                            {
                                                attdto.returnval = true;
                                            }
                                            else
                                            {
                                                attdto.returnval = false;
                                            }
                                        }
                                    }
                                }

                                ////attendance new  entry  code for absent

                                else
                                {
                                    ////attendance new  entry  code for absent                                    
                                    Adm_studentAttendance enq = new Adm_studentAttendance();
                                    enq.MI_Id = attdto.MI_Id;
                                    enq.ASMAY_Id = attdto.ASMAY_Id;
                                    enq.ASA_Att_Type = attdto.ASA_Att_Type;
                                    enq.ASA_Att_EntryType = "Absent";
                                    enq.ASMCL_Id = attdto.ASMCL_Id;
                                    enq.ASMS_Id = attdto.ASMS_Id;
                                    enq.IMP_Id = Convert.ToInt64(attdto.ISMP_Id);
                                    enq.ASA_Entry_DateTime = attdto.ASA_Entry_DateTime;
                                    enq.ASA_FromDate = attdto.ASA_FromDate;
                                    enq.ASA_ToDate = attdto.ASA_ToDate;
                                    enq.ASA_ClassHeld = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                    enq.ASA_Network_IP = attdto.ASA_Network_IP;
                                    enq.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                    enq.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                    enq.CreatedDate = indianTime;
                                    enq.UpdatedDate = indianTime;
                                    enq.ASA_CreatedBy = attdto.userId;
                                    enq.ASA_UpdatedBy = attdto.userId;
                                    enq.ASA_Activeflag = true;
                                    _db.Adm_studentAttendance.Add(enq);
                                    // _db.SaveChanges();
                                    if (attdto.stdList != null && attdto.stdList.Count() > 0)
                                    {
                                        for (int i = 0; i < attdto.stdList.Count(); i++)
                                        {
                                            //Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.stdList[i]);
                                            Adm_studentAttendanceStudents std = new Adm_studentAttendanceStudents();
                                            std.ASA_Id = enq.ASA_Id;

                                            if (attdto.ASA_Att_Type == "Dailyonce")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    std.ASA_Class_Attended = 0;

                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    std.ASA_AttendanceFlag = "Present";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "Dailytwice")
                                            {
                                                string classheld = "";
                                                if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Secondhalf";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].SecondHalfflag == true && attdto.stdList[i].FirstHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "firsthalf";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == true)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Absent";
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    classheld = "0.0";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == false && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Present";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "1.0";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "monthly")
                                            {
                                                std.ASA_AttendanceFlag = "Present";
                                                std.ASA_Class_Attended = Convert.ToDecimal(attdto.stdList[i].pdays);
                                            }
                                            std.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            std.CreatedDate = indianTime;
                                            std.UpdatedDate = indianTime;
                                            std.ASAS_CreatedBy = attdto.userId;
                                            std.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(std);

                                        }
                                        var contactExists = _db.SaveChanges();

                                        if (contactExists >= 1)
                                        {
                                            attdto.returnval = true;
                                        }
                                        else
                                        {
                                            attdto.returnval = false;
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < attdto.apsentStdList.Count(); i++)
                                        {
                                            Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.apsentStdList[i]);
                                            std.ASA_Id = enq.ASA_Id;
                                            std.ASA_AttendanceFlag = "Absent";
                                            std.ASA_Class_Attended = 0;                                         
                                            std.CreatedDate = indianTime;
                                            std.UpdatedDate = indianTime;
                                            std.ASAS_CreatedBy = attdto.userId;
                                            std.ASAS_UpdatedBy = attdto.userId;
                                            _db.Add(std);

                                            var contactExists = _db.SaveChanges();

                                            if (contactExists >= 1)
                                            {
                                                attdto.returnval = true;
                                            }
                                            else
                                            {
                                                attdto.returnval = false;
                                            }
                                        }
                                    }
                                    if (enq.ASA_Id > 0 && attdto.PAMS_Id > 0)
                                    {
                                        Adm_studentAttendanceSubjects sbj = Mapper.Map<Adm_studentAttendanceSubjects>(attdto);
                                        sbj.ASA_Id = enq.ASA_Id;
                                        sbj.CreatedDate = indianTime;
                                        sbj.UpdatedDate = indianTime;
                                        sbj.ASASU_CreatedBy = attdto.userId;
                                        sbj.ASASU_UpdatedBy = attdto.userId;
                                        _db.Add(sbj);
                                        var contactExists = _db.SaveChanges();

                                        if (contactExists >= 1)
                                        {
                                            attdto.returnval = true;
                                        }
                                        else
                                        {
                                            attdto.returnval = false;
                                        }
                                    }

                                    //  transaction.Commit();
                                }
                            }
                            else if (attdto.ASA_Id > 0)
                            {

                            }
                            else
                            {
                                attdto.studentList = (from a in _db.Adm_studentAttendance
                                                      from b in _db.Adm_studentAttendanceStudents
                                                      from c in _db.Adm_M_Student
                                                      from d in _db.School_Adm_Y_StudentDMO
                                                      where a.ASA_Id == b.ASA_Id && a.ASMAY_Id == attdto.ASMAY_Id && a.ASMCL_Id == attdto.ASMAY_Id 
                                                      && a.MI_Id == attdto.MI_Id && a.ASMS_Id == attdto.ASMS_Id && b.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id 
                                                      && a.ASA_Activeflag == true
                                                      select new StudentAttendanceEntryDTO
                                                      {
                                                          studentname = c.AMST_FirstName + c.AMST_MiddleName + c.AMST_LastName,
                                                          AMST_AdmNo = c.AMST_AdmNo,
                                                          AMAY_RollNo = d.AMAY_RollNo,
                                                          ASA_Class_Attended = b.ASA_Class_Attended
                                                      }).ToArray();
                            }

                        } //end of attendance entry type absent

                        //attendance entry type present

                        else if (attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "P")
                        {
                            if (attdto.ASA_Id == 0)
                            {
                                var isDuplicate = _db.Adm_studentAttendance.Where(d => d.MI_Id == attdto.MI_Id && d.ASMAY_Id == attdto.ASMAY_Id
                                && d.ASMCL_Id == attdto.ASMCL_Id && d.ASMS_Id == attdto.ASMS_Id && d.ASA_FromDate == attdto.ASA_FromDate
                                && d.ASA_Att_Type == attdto.ASA_Att_Type && d.ASA_Activeflag == true).ToList();
                                if (isDuplicate.Count > 0)
                                {
                                    ////attendance update  entry  code for present
                                    for (int i = 0; i < attdto.stdList.Count(); i++)
                                    {
                                        if (i == 0) //**updating the adm_student_attendance when entry type change
                                        {
                                            var result = _db.Adm_studentAttendance.Single(d => d.ASA_Id == attdto.stdList[i].asA_Id);
                                            result.ASA_Att_EntryType = "Present";
                                            result.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                            result.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                            result.ASA_ClassHeld = Convert.ToDecimal(attdto.ASA_ClassHeld);                                            
                                            result.UpdatedDate = indianTime;
                                            result.ASA_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendance.Update(result);
                                        }
                                        //**start** if condition is for updating the attendance once  attendnace given
                                        if (attdto.stdList[i].ASAS_Id != null)
                                        {
                                            var result = _db.Adm_studentAttendanceStudents.Single(a => a.ASAS_Id == attdto.stdList[i].ASAS_Id && a.AMST_Id == attdto.stdList[i].amsT_Id);
                                            //Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.stdList[i]);
                                            Adm_studentAttendanceStudents std = new Adm_studentAttendanceStudents();
                                            if (attdto.ASA_Att_Type == "Dailyonce")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    result.ASA_AttendanceFlag = "Present";
                                                    result.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    result.ASA_AttendanceFlag = "Absent";
                                                    result.ASA_Class_Attended = 0;

                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "Dailytwice")
                                            {
                                                string classheld = "";
                                                if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    result.ASA_Dailytwice_Flag = "firsthalf";
                                                    result.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    result.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].SecondHalfflag == true && attdto.stdList[i].FirstHalfflag == false)
                                                {
                                                    result.ASA_Dailytwice_Flag = "Secondhalf";
                                                    result.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    result.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == true)
                                                {
                                                    result.ASA_Dailytwice_Flag = "Present";
                                                    result.ASA_AttendanceFlag = "Present";
                                                    classheld = "1.0";
                                                    result.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == false && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    result.ASA_Dailytwice_Flag = "Absent";
                                                    result.ASA_AttendanceFlag = "Absent";
                                                    classheld = "0.0";
                                                    result.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "monthly")
                                            {
                                                result.ASA_AttendanceFlag = "Present";
                                                result.ASA_Class_Attended = Convert.ToDecimal(attdto.stdList[i].pdays);
                                            }
                                            result.UpdatedDate = indianTime;
                                            result.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Update(result);
                                            var contactExists = _db.SaveChanges();

                                            if (contactExists > 0)
                                            {
                                                attdto.returnval = true;
                                            }
                                            else
                                            {
                                                attdto.returnval = false;
                                            }
                                        }
                                        //**end ** if condition is for updating the attendance once  attendnace given

                                        // **start *** else part is for saving the new record when the new student is added on particulare date after entring the attendance
                                        else
                                        {
                                            Adm_studentAttendanceStudents std = new Adm_studentAttendanceStudents();
                                            std.ASA_Id = isDuplicate.FirstOrDefault().ASA_Id;
                                            std.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);
                                            if (attdto.ASA_Att_Type == "Dailyonce")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    std.ASA_AttendanceFlag = "Present";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);

                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    std.ASA_Class_Attended = 0;
                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "Dailytwice")
                                            {
                                                string classheld = "";
                                                if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "firsthalf";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].SecondHalfflag == true && attdto.stdList[i].FirstHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Secondhalf";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == true)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Present";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "1.0";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == false && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Absent";
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    classheld = "0.0";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "monthly")
                                            {
                                                std.ASA_AttendanceFlag = "Present";
                                                std.ASA_Class_Attended = Convert.ToDecimal(attdto.stdList[i].pdays);
                                            }                                            
                                            std.CreatedDate = indianTime;
                                            std.UpdatedDate = indianTime;
                                            std.ASAS_CreatedBy = attdto.userId;
                                            std.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(std);

                                            var contactExists = _db.SaveChanges();

                                            if (contactExists >= 1)
                                            {
                                                attdto.returnval = true;
                                            }
                                            else
                                            {
                                                attdto.returnval = false;
                                            }
                                        }
                                        // **end *** else part is for saving the new record when the new student is added on particulare date after entring the attendance
                                    }

                                }
                                else
                                {
                                    ////attendance new  entry  code for present
                                    Adm_studentAttendance enq = new Adm_studentAttendance();

                                    enq.MI_Id = attdto.MI_Id;
                                    enq.ASMAY_Id = attdto.ASMAY_Id;
                                    enq.ASA_Att_Type = attdto.ASA_Att_Type;
                                    enq.ASA_Att_EntryType = "Present";
                                    enq.ASMCL_Id = attdto.ASMCL_Id;
                                    enq.ASMS_Id = attdto.ASMS_Id;
                                    enq.IMP_Id = Convert.ToInt64(attdto.ISMP_Id);
                                    enq.ASA_Entry_DateTime = attdto.ASA_Entry_DateTime;
                                    enq.ASA_FromDate = attdto.ASA_FromDate;
                                    enq.ASA_ToDate = attdto.ASA_ToDate;
                                    enq.ASA_ClassHeld = Convert.ToDecimal(attdto.ASA_ClassHeld);
                                    enq.ASA_Network_IP = attdto.ASA_Network_IP;
                                    enq.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                                    enq.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                                    enq.CreatedDate = indianTime;
                                    enq.UpdatedDate = indianTime;
                                    enq.ASA_CreatedBy = attdto.userId;
                                    enq.ASA_UpdatedBy = attdto.userId;
                                    enq.ASA_Activeflag = true;
                                    _db.Adm_studentAttendance.Add(enq);

                                    if (attdto.stdList != null && attdto.stdList.Count() > 0)
                                    {
                                        for (int i = 0; i < attdto.stdList.Count(); i++)
                                        {
                                            //  Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.stdList[i]);
                                            Adm_studentAttendanceStudents std = new Adm_studentAttendanceStudents();
                                            std.ASA_Id = enq.ASA_Id;
                                            std.AMST_Id = Convert.ToInt64(attdto.stdList[i].amsT_Id);

                                            if (attdto.ASA_Att_Type == "Dailyonce")
                                            {
                                                if (attdto.stdList[i].selected == true)
                                                {
                                                    std.ASA_AttendanceFlag = "Present";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(attdto.ASA_ClassHeld);

                                                }
                                                else if (attdto.stdList[i].selected == false)
                                                {
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    std.ASA_Class_Attended = 0;
                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "Dailytwice")
                                            {
                                                string classheld = "";
                                                if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "firsthalf";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].SecondHalfflag == true && attdto.stdList[i].FirstHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Secondhalf";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "0.5";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == true && attdto.stdList[i].SecondHalfflag == true)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Present";
                                                    std.ASA_AttendanceFlag = "Present";
                                                    classheld = "1.0";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                                else if (attdto.stdList[i].FirstHalfflag == false && attdto.stdList[i].SecondHalfflag == false)
                                                {
                                                    std.ASA_Dailytwice_Flag = "Absent";
                                                    std.ASA_AttendanceFlag = "Absent";
                                                    classheld = "0.0";
                                                    std.ASA_Class_Attended = Convert.ToDecimal(classheld);
                                                }
                                            }
                                            else if (attdto.ASA_Att_Type == "monthly")
                                            {
                                                std.ASA_AttendanceFlag = "Present";
                                                std.ASA_Class_Attended = Convert.ToDecimal(attdto.stdList[i].pdays);
                                            }
                                            else if (attdto.ASA_Att_Type == "period")
                                            {
                                                string classheldperiod = "";
                                                std.ASA_AttendanceFlag = "Present";
                                                classheldperiod = "1.0";
                                                std.ASA_Class_Attended = Convert.ToDecimal(classheldperiod);
                                            }                                           
                                            std.CreatedDate = indianTime;
                                            std.UpdatedDate = indianTime;
                                            std.ASAS_CreatedBy = attdto.userId;
                                            std.ASAS_UpdatedBy = attdto.userId;
                                            _db.Adm_studentAttendanceStudents.Add(std);

                                        }
                                        var contactExists = _db.SaveChanges();

                                        if (contactExists >= 1)
                                        {
                                            attdto.returnval = true;
                                        }
                                        else
                                        {
                                            attdto.returnval = false;
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; i < attdto.apsentStdList.Count(); i++)
                                        {
                                            Adm_studentAttendanceStudents std = Mapper.Map<Adm_studentAttendanceStudents>(attdto.apsentStdList[i]);
                                            std.ASA_Id = enq.ASA_Id;
                                            std.ASA_AttendanceFlag = "Absent";
                                            std.ASA_Class_Attended = 0;                                           
                                            std.CreatedDate = indianTime;
                                            std.UpdatedDate = indianTime;
                                            std.ASAS_CreatedBy = attdto.userId;
                                            std.ASAS_UpdatedBy = attdto.userId;
                                            _db.Add(std);

                                            var contactExists = _db.SaveChanges();

                                            if (contactExists >= 1)
                                            {
                                                attdto.returnval = true;
                                            }
                                            else
                                            {
                                                attdto.returnval = false;
                                            }
                                        }
                                    }
                                    if (enq.ASA_Id > 0 && attdto.PAMS_Id > 0)
                                    {
                                        Adm_studentAttendanceSubjects sbj = Mapper.Map<Adm_studentAttendanceSubjects>(attdto);
                                        sbj.ASA_Id = enq.ASA_Id;                                        
                                        sbj.CreatedDate = indianTime;
                                        sbj.UpdatedDate = indianTime;
                                        sbj.ASASU_CreatedBy = attdto.userId;
                                        sbj.ASASU_UpdatedBy = attdto.userId;
                                        _db.Add(sbj);
                                        var contactExists = _db.SaveChanges();

                                        if (contactExists >= 1)
                                        {
                                            attdto.returnval = true;
                                        }
                                        else
                                        {
                                            attdto.returnval = false;
                                        }
                                    }

                                    // transaction.Commit();
                                } ////attendance new  entry  code for present
                            }
                            else if (attdto.ASA_Id > 0)
                            {

                            }
                            else
                            {
                                attdto.studentList = (from a in _db.Adm_studentAttendance
                                                      from b in _db.Adm_studentAttendanceStudents
                                                      from c in _db.Adm_M_Student
                                                      from d in _db.School_Adm_Y_StudentDMO
                                                      where a.ASA_Id == b.ASA_Id && a.ASMAY_Id == attdto.ASMAY_Id && a.ASMCL_Id == attdto.ASMAY_Id && a.MI_Id == attdto.MI_Id && a.ASMS_Id == attdto.ASMS_Id && b.AMST_Id == d.AMST_Id && c.AMST_Id == d.AMST_Id && a.ASA_Activeflag == true
                                                      select new StudentAttendanceEntryDTO
                                                      {
                                                          studentname = c.AMST_FirstName + c.AMST_MiddleName + c.AMST_LastName,
                                                          AMST_AdmNo = c.AMST_AdmNo,
                                                          AMAY_RollNo = d.AMAY_RollNo,
                                                          ASA_Class_Attended = b.ASA_Class_Attended
                                                      }).ToArray();
                            }

                        }//end of the attendance entry type present
                    }
                    else
                    {
                        attdto.message = "Attendance Entry Type Flag Is Not Mapped Please Contact Administrator...";
                        return attdto;
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error save attendance :'" + ex.Message + "'");
            }
            return attdto;
        }
        public async Task<StudentAttendanceEntryDTO> Deleteattendance(StudentAttendanceEntryDTO attdto)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime2 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var result = _db.Adm_studentAttendance.Single(d => d.ASA_Id == attdto.stdList[0].asA_Id);

                result.ASA_Activeflag = false;
                result.ASA_UpdatedBy = attdto.userId;
                result.UpdatedDate = indianTime2;
                _db.Update(result);
                int i = _db.SaveChanges();
                if (i > 0)
                {
                    attdto.returnval = true;
                }
                else
                {
                    attdto.returnval = false;
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Attendance Entry Delete : " + ex.Message);
                Console.WriteLine(ex.Message);
                attdto.returnval = false;
            }
            return attdto;
        }
        public async Task<StudentAttendanceEntryDTO> getdatewiseatt(StudentAttendanceEntryDTO data)
        {
            List<StudentAttTempDTO> studentList1 = new List<StudentAttTempDTO>();
            List<StudentAttTempDTO> result = new List<StudentAttTempDTO>();
            List<StudentAttTempDTO> obj = new List<StudentAttTempDTO>();

            data.admissionstandarad = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            data.getstandarad = _db.GenConfig.Where(a => a.MI_Id == data.MI_Id).ToArray();

            var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
            data.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;
            //  string fromdatecon = Convert.ToString(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd"));

            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";
            if (data.ASA_FromDate != null)
            {
                try
                {
                    fromdatecon = Convert.ToDateTime(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd"));
                    //confromdate = fromdatecon.ToString();
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            string ordderby = "";
            if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 1)
            {
                ordderby = "Order By AMST_Sex";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 2)
            {
                ordderby = "Order By AMST_Sex desc";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 3)
            {
                ordderby = "Order By AMAY_RollNo ";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 4)
            {
                ordderby = "Order By studentname";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 5)
            {
                ordderby = "Order By studentname desc";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 6)
            {
                ordderby = "Order By amsT_RegistrationNo";
            }
            else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 7)
            {
                ordderby = "Order By AMST_Admno";
            }
            else
            {
                ordderby = "Order By studentname";
            }

            var vdata = (from a in _db.Adm_studentAttendanceStudents
                         from b in _db.Adm_studentAttendance
                         where (a.ASA_Id == b.ASA_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                         && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ASA_Att_Type == data.monthflag1
                         && b.ASA_FromDate == data.ASA_FromDate && b.ASA_Activeflag == true)
                         select new StudentAttendanceEntryDTO
                         {
                             ASA_Id = b.ASA_Id
                         }).ToList();

            data.countclass1 = vdata.Count;

            if (data.countclass1 >= 1)
            {
                try
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        _acdimpl.LogInformation("enter in getdbconncetion");
                        cmd.CommandText = "adm_student_list_not_in_att";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMCL_Id) });
                        cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMS_Id) });
                        //cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = Convert.ToString(data.ASA_FromDate) });
                        //cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = Convert.ToString(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd")) });
                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = Convert.ToString(data.monthflag) });
                        cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(data.monthid) });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        _acdimpl.LogInformation("enter in if condtion");
                        _acdimpl.LogInformation("Fromdate :'" + data.ASA_FromDate + "");
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                _acdimpl.LogInformation("enter in datareader condition");
                                while (dataReader.Read())
                                {
                                    _acdimpl.LogInformation("enter in while condition");
                                    result.Add(new StudentAttTempDTO
                                    {
                                        amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                        studentname = (dataReader["studentname"]).ToString(),
                                        amsT_AdmNo = (dataReader["AMST_AdmNo"]).ToString(),
                                        amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                        amsT_RegistrationNo = (dataReader["amsT_RegistrationNo"]).ToString(),
                                    });
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _acdimpl.LogInformation("error:'" + ex.Message + "'");
                            Console.Write(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                    Console.Write(ex.Message);
                }

                try
                {
                    data.monthid = 0;

                    var type = _db.AttendanceEntryTypeDMO.Where(t => t.ASMCL_Id.Equals(data.ASMCL_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id)).ToArray();
                    data.ASA_Att_EntryType = type[0].ASAET_Att_Type;
                    if (data.monthflag == "D")
                    {
                        using (var command = _db.Database.GetDbConnection().CreateCommand())
                        {
                            command.CommandText = " Select b.ASA_Class_Attended  as ASA_Class_Attended, b.ASAS_Id as ASAS_Id, a.ASA_Id as ASA_Id, (d.AMST_Id) AMST_Id, (CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+" +
                        "CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END + " +
                        "CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END) AS studentname," +
                        "(d.AMST_AdmNo)AMST_AdmNo, (c.AMAY_RollNo) AMAY_RollNo, AMST_Sex, (amsT_RegistrationNo) as amsT_RegistrationNo from Adm_Student_Attendance a " +
                        "INNER JOIN Adm_Student_Attendance_Students b on  a.asa_id=b.asa_id inner join  Adm_School_Y_Student c on c.amst_id=b.amst_id " +
                        " Inner Join  Adm_M_student d on d.amst_id=c.amst_id  inner join Adm_School_M_Academic_Year dd on dd.asmay_id=c.ASMAY_Id  where ASA_Activeflag=1 and  c.asmcl_id=" + data.ASMCL_Id + "  and c.ASMS_Id =" + data.ASMS_Id + " and c.ASMAY_Id =" + data.ASMAY_Id + " and  a.asmcl_id=" + data.ASMCL_Id + "  and a.ASMS_Id =" + data.ASMS_Id + " and a.ASMAY_Id =" + data.ASMAY_Id + " " +
                        "and a.MI_Id =" + data.MI_Id + " and a.ASA_FromDate ='" + confromdate + "' and  a.ASA_Att_Type ='" + data.monthflag1 + "' " +
                        " and d.AMST_ActiveFlag = 1 and d.AMST_SOL = 'S' and c.AMAY_ActiveFlag = 1  " + ordderby + " ";
                            _db.Database.OpenConnection();
                            using (var result1 = command.ExecuteReader())
                            {
                                while (result1.Read())
                                {
                                    obj.Add(new StudentAttTempDTO
                                    {
                                        amsT_Id = Convert.ToInt64(result1["AMST_Id"]),
                                        studentname = result1["studentname"].ToString(),
                                        amsT_AdmNo = result1["AMST_AdmNo"].ToString(),
                                        amaY_RollNo = Convert.ToInt64(result1["AMAY_RollNo"]),
                                        amsT_RegistrationNo = result1["amsT_RegistrationNo"].ToString(),
                                        pdays = Convert.ToDecimal(result1["ASA_Class_Attended"]),
                                        ASAS_Id = Convert.ToInt64(result1["ASAS_Id"]),
                                        asA_Id = Convert.ToInt64(result1["ASA_Id"]),
                                    });
                                }

                                studentList1 = obj.ToList();
                            }
                        }
                    }


                    else if (data.monthflag == "H")
                    {
                        using (var command = _db.Database.GetDbConnection().CreateCommand())
                        {
                            command.CommandText = " Select b.ASA_Class_Attended  as ASA_Class_Attended, b.ASAS_Id as ASAS_Id, a.ASA_Id as ASA_Id, (d.AMST_Id) AMST_Id, (CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+" +
                        "CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END + " +
                        "CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END) AS studentname," +
                        "(d.AMST_AdmNo)AMST_AdmNo, (c.AMAY_RollNo) AMAY_RollNo, AMST_Sex, (amsT_RegistrationNo) as amsT_RegistrationNo ,asA_Dailytwice_Flag as asA_Dailytwice_Flag from Adm_Student_Attendance a " +
                        "INNER JOIN Adm_Student_Attendance_Students b on  a.asa_id=b.asa_id inner join  Adm_School_Y_Student c on c.amst_id=b.amst_id " +
                        " Inner Join  Adm_M_student d on d.amst_id=c.amst_id  inner join Adm_School_M_Academic_Year dd on dd.asmay_id=c.ASMAY_Id where ASA_Activeflag=1 and c.asmcl_id=" + data.ASMCL_Id + "  and c.ASMS_Id =" + data.ASMS_Id + " and c.ASMAY_Id =" + data.ASMAY_Id + " and a.asmcl_id=" + data.ASMCL_Id + "  and a.ASMS_Id =" + data.ASMS_Id + " and a.ASMAY_Id =" + data.ASMAY_Id + " " +
                        "and a.MI_Id =" + data.MI_Id + " and a.ASA_FromDate ='" + confromdate + "'  and  a.ASA_Att_Type ='" + data.monthflag1 + "' " +
                        " and d.AMST_ActiveFlag = 1 and d.AMST_SOL = 'S' and c.AMAY_ActiveFlag = 1  " + ordderby + " ";
                            _db.Database.OpenConnection();
                            using (var result1 = command.ExecuteReader())
                            {
                                while (result1.Read())
                                {
                                    obj.Add(new StudentAttTempDTO
                                    {
                                        amsT_Id = Convert.ToInt64(result1["AMST_Id"]),
                                        studentname = result1["studentname"].ToString(),
                                        amsT_AdmNo = result1["AMST_AdmNo"].ToString(),
                                        amaY_RollNo = Convert.ToInt64(result1["AMAY_RollNo"]),
                                        amsT_RegistrationNo = result1["amsT_RegistrationNo"].ToString(),
                                        pdays = Convert.ToDecimal(result1["ASA_Class_Attended"]),
                                        ASAS_Id = Convert.ToInt64(result1["ASAS_Id"]),
                                        asA_Id = Convert.ToInt64(result1["ASA_Id"]),
                                        asA_Dailytwice_Flag = result1["asA_Dailytwice_Flag"].ToString(),
                                    });
                                }
                                studentList1 = obj.ToList();
                            }
                        }
                    }


                    else if (data.monthflag == "M")
                    {
                        studentList1 = (from a in _db.Adm_studentAttendance
                                        from b in _db.Adm_studentAttendanceStudents
                                        from c in _db.masterclassHeld
                                        from d in _db.AttendanceEntryTypeDMO
                                        from e in _db.Adm_M_Student
                                        from f in _db.School_Adm_Y_StudentDMO
                                        where (c.ASMCL_Id == d.ASMCL_Id && a.ASA_Id == b.ASA_Id && a.ASMCL_Id == c.ASMCL_Id && b.AMST_Id == f.AMST_Id && e.AMST_Id == f.AMST_Id
                                        && c.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && f.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASA_FromDate.Value.Month == data.monthid && a.ASA_FromDate.Value.Year == DateTime.Now.Year
                                        && d.ASAET_Att_Type == data.monthflag && e.AMST_ActiveFlag == 1 && e.AMST_SOL == "S" && f.AMAY_ActiveFlag == 1 && a.ASA_Activeflag == true)
                                        select new StudentAttTempDTO
                                        {
                                            amsT_Id = e.AMST_Id,
                                            studentname = ((e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName)).Trim(),
                                            amsT_AdmNo = e.AMST_AdmNo == null ? "" : e.AMST_AdmNo,
                                            amaY_RollNo = f.AMAY_RollNo,
                                            amsT_RegistrationNo = e.AMST_RegistrationNo == null ? "" : e.AMST_RegistrationNo,
                                            pdays = b.ASA_Class_Attended,
                                            ASAS_Id = b.ASAS_Id,
                                            asA_Id = a.ASA_Id,
                                        }
                                             ).Distinct().ToList();
                    }
                    for (int i = 0; i < result.Count; i++)
                    {
                        studentList1.Add(result[i]);
                    }

                    data.studentList = studentList1.ToArray();
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                    Console.Write(ex.Message);
                }
            }
            else
            {
                List<StudentAttendanceEntryDTO> arrAttdto = new List<StudentAttendanceEntryDTO>();
                //string mywhere = null;
                try
                {
                    if (data.ASMAY_Id != 0 && data.ASMCL_Id != 0)
                    {
                        var type = _db.AttendanceEntryTypeDMO.Where(t => t.ASMCL_Id.Equals(data.ASMCL_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id)).ToArray();

                        data.ASA_Att_EntryType = type[0].ASAET_Att_Type;
                    }

                    if (data.ASMAY_Id != 0 && data.ASMCL_Id != 0)
                    {
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GetStudentDataByAdecmicYearClassSection_New";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag });
                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar) { Value = confromdate });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        if (dataReader["studentname"] != System.DBNull.Value)
                                        {
                                            Student_name_null = Convert.ToString(dataReader["studentname"]);
                                        }
                                        else
                                        {
                                            Student_name_null = "NOT AVAILABLE";
                                        }

                                        if (dataReader["AMST_AdmNo"] != System.DBNull.Value)
                                        {
                                            AMST_ADM_null = Convert.ToString(dataReader["AMST_AdmNo"]);
                                        }
                                        else
                                        {
                                            AMST_ADM_null = "NOT AVAILABLE";
                                        }
                                        if (dataReader["amsT_RegistrationNo"] != System.DBNull.Value)
                                        {
                                            amsT_RegistrationNo = Convert.ToString(dataReader["amsT_RegistrationNo"]);
                                        }
                                        else
                                        {
                                            amsT_RegistrationNo = "NOT AVAILABLE";
                                        }

                                        arrAttdto.Add(new StudentAttendanceEntryDTO
                                        {
                                            AMST_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                            studentname = Student_name_null,
                                            AMST_AdmNo = AMST_ADM_null,
                                            AMAY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                            amsT_RegistrationNo = amsT_RegistrationNo,
                                        });
                                        data.studentList = arrAttdto.ToArray();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                Console.Write(ex.Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                    Console.Write(ex.Message);
                }
            }
            return data;
        }
        public StudentAttendanceEntryDTO year(StudentAttendanceEntryDTO data)
        {
            try
            {
                data.academicYearList = _db.AcademicYear.Where(r => r.Is_Active == true && r.MI_Id == data.MI_Id && r.ASMAY_Id == data.ASMAY_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public StudentAttendanceEntryDTO getbatchlist(StudentAttendanceEntryDTO data)
        {
            try
            {
                var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;
                //  var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.batchList = _db.AdmSchoolAttendanceSubjectBatch.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ISMS_Id == data.ismS_Id).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error At Batch List In Attendance Entry:" + ex.Message);
            }
            return data;
        }
        public StudentAttendanceEntryDTO getstdlistperiod(StudentAttendanceEntryDTO data)
        {
            try
            {
                var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.admissionstandarad = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                data.getstandarad = _db.GenConfig.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;

                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                if (data.ASA_FromDate != null)
                {
                    try
                    {
                        fromdatecon = Convert.ToDateTime(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd"));
                        //confromdate = fromdatecon.ToString();
                        confromdate = fromdatecon.ToString("yyyy-MM-dd");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                string ordderby = "";
                if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 1)
                {
                    ordderby = "Order By AMST_Sex";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 2)
                {
                    ordderby = "Order By AMST_Sex desc";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 3)
                {
                    ordderby = "Order By AMAY_RollNo ";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 4)
                {
                    ordderby = "Order By studentname";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 5)
                {
                    ordderby = "Order By studentname desc";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 6)
                {
                    ordderby = "Order By amsT_RegistrationNo";
                }
                else if (attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag == 7)
                {
                    ordderby = "Order By AMST_Admno";
                }
                else
                {
                    ordderby = "Order By studentname";
                }

                if (data.asasB_Id == 0)
                {
                    var check_subject_elective = _db.StudentMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_Id == data.ismS_Id && a.ASMAY_Id == data.ASMAY_Id
                      && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ESTSU_ElecetiveFlag == true).ToList();
                    if (check_subject_elective.Count == 0)
                    {
                        //---Checking Whether Selected Period Attendance Is Done Or Not For That Class , Section , Year, Date And Period---//
                        var checkperiodattentry = (from a in _db.Adm_studentAttendance
                                                   from b in _db.Adm_studentAttendanceStudents
                                                   from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                                   from d in _db.Adm_studentAttendanceSubjects
                                                   where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id
                                                   && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                                   && c.TTMP_Id == data.TTMP_Id
                                                   && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate && a.ASA_Activeflag == true)
                                                   select new StudentAttendanceEntryDTO
                                                   {
                                                       ASA_Id = c.ASA_Id,
                                                       TTMP_Id = c.TTMP_Id
                                                   }).Distinct().ToList();

                        if (checkperiodattentry.Count > 0)
                        {
                            //--------If Period Attendance Happend For That Period Then Checking Period With Subject--------//
                            var checkperiodsubjectatt = (from a in _db.Adm_studentAttendance
                                                         from b in _db.Adm_studentAttendanceStudents
                                                         from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                                         from d in _db.Adm_studentAttendanceSubjects
                                                         where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id
                                                         && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && d.ISMS_Id == data.ismS_Id && c.TTMP_Id == data.TTMP_Id
                                                         && a.ASMS_Id == data.ASMS_Id && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate
                                                         && a.ASA_Activeflag == true)
                                                         select new StudentAttendanceEntryDTO
                                                         {
                                                             ASA_Id = c.ASA_Id,
                                                             TTMP_Id = c.TTMP_Id,
                                                             ismS_Id = d.ISMS_Id
                                                         }).Distinct().ToList();

                            if (checkperiodsubjectatt.Count > 0)
                            {
                                //--Retrieving The Saved Data For That Date , Class , Section , Subject, Period--//
                                try
                                {
                                    data.monthid = 0;
                                    List<StudentAttTempDTO> studentList1 = new List<StudentAttTempDTO>();
                                    List<StudentAttTempDTO> result = new List<StudentAttTempDTO>();
                                    List<StudentAttTempDTO> obj = new List<StudentAttTempDTO>();

                                    var type = _db.AttendanceEntryTypeDMO.Where(t => t.ASMCL_Id.Equals(data.ASMCL_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id) && t.MI_Id.Equals(data.MI_Id)).ToArray();
                                    data.ASA_Att_EntryType = type[0].ASAET_Att_Type;
                                    var getregularorextra = _db.Adm_studentAttendance.Where(t => t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASA_Att_Type.Equals(data.monthflag1) && t.ASA_FromDate == data.ASA_FromDate && t.MI_Id == data.MI_Id).ToList();
                                    data.ASA_Regular_Extra = getregularorextra.FirstOrDefault().ASA_Regular_Extra;
                                    try
                                    {
                                        _acdimpl.LogInformation("entered try block");
                                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                        {
                                            _acdimpl.LogInformation("entered cmd getdbconnection");
                                            cmd.CommandText = "adm_student_list_not_in_att";
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMCL_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMS_Id) });
                                            // cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = Convert.ToString(data.ASA_FromDate) });
                                            //cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = Convert.ToString(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd")) });
                                            cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = confromdate });
                                            cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = Convert.ToString(data.monthflag) });
                                            cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(data.monthid) });
                                            if (cmd.Connection.State != ConnectionState.Open)
                                                cmd.Connection.Open();
                                            _acdimpl.LogInformation("entered if block");
                                            _acdimpl.LogInformation("Fromdate :'" + data.ASA_FromDate + "");
                                            var retObject = new List<dynamic>();
                                            try
                                            {
                                                using (var dataReader = cmd.ExecuteReader())
                                                {
                                                    _acdimpl.LogInformation("entered in dataReader block");
                                                    while (dataReader.Read())
                                                    {
                                                        _acdimpl.LogInformation("entered in while block");

                                                        result.Add(new StudentAttTempDTO
                                                        {
                                                            amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                            studentname = (dataReader["studentname"]).ToString(),
                                                            amsT_AdmNo = (dataReader["AMST_AdmNo"]).ToString(),
                                                            amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                                            amsT_RegistrationNo = (dataReader["amsT_RegistrationNo"]).ToString(),
                                                        });
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                                Console.Write(ex.Message);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                        Console.Write(ex.Message);
                                    }


                                    if (data.monthflag == "P")
                                    {
                                        using (var command = _db.Database.GetDbConnection().CreateCommand())
                                        {
                                            command.CommandText = " Select Distinct  b.ASA_Class_Attended  as ASA_Class_Attended, b.ASAS_Id as ASAS_Id, a.ASA_Id as ASA_Id, (d.AMST_Id) AMST_Id, (CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+" +
                                        "CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END + " +
                                        "CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END) AS studentname," +
                                        "(d.AMST_AdmNo)AMST_AdmNo, (c.AMAY_RollNo) AMAY_RollNo, AMST_Sex, (amsT_RegistrationNo) as amsT_RegistrationNo from Adm_Student_Attendance a " +
                                        "INNER JOIN Adm_Student_Attendance_Students b on  a.asa_id=b.asa_id inner join  Adm_School_Y_Student c on c.amst_id=b.amst_id " +
                                        " Inner Join  Adm_M_student d on d.amst_id=c.amst_id  inner join Adm_School_M_Academic_Year dd on dd.asmay_id=c.ASMAY_Id inner join Adm_Student_Attendance_Periodwise k on k.asa_id=a.asa_id " +
                                        " inner join adm_student_attendance_subjects l on l.asa_id=a.asa_id inner join Exm.Exm_Studentwise_Subjects m on m.amst_id=c.amst_id and m.ASMAY_Id =" + data.ASMAY_Id + "  where  ASA_Activeflag=1 and c.asmcl_id=" + data.ASMCL_Id + "  and c.ASMS_Id =" + data.ASMS_Id + " and c.ASMAY_Id =" + data.ASMAY_Id + " " +
                                        "and a.MI_Id =" + data.MI_Id + " and a.ASA_FromDate = '" + confromdate + "' and  a.ASA_Att_Type ='" + data.monthflag1 + "' " +
                                        " and d.AMST_ActiveFlag = 1 and d.AMST_SOL = 'S' and c.AMAY_ActiveFlag = 1 and  l.ISMS_Id =" + data.ismS_Id + " and m.ISMS_Id =" + data.ismS_Id + " and k.TTMP_Id =" + data.TTMP_Id + " " + ordderby + " ";
                                            _db.Database.OpenConnection();
                                            using (var result1 = command.ExecuteReader())
                                            {
                                                while (result1.Read())
                                                {
                                                    obj.Add(new StudentAttTempDTO
                                                    {
                                                        amsT_Id = Convert.ToInt64(result1["AMST_Id"]),
                                                        studentname = result1["studentname"].ToString(),
                                                        amsT_AdmNo = result1["AMST_AdmNo"].ToString(),
                                                        amaY_RollNo = Convert.ToInt64(result1["AMAY_RollNo"]),
                                                        amsT_RegistrationNo = result1["amsT_RegistrationNo"].ToString(),
                                                        pdays = Convert.ToDecimal(result1["ASA_Class_Attended"]),
                                                        ASAS_Id = Convert.ToInt64(result1["ASAS_Id"]),
                                                        asA_Id = Convert.ToInt64(result1["ASA_Id"]),
                                                    });
                                                }
                                                studentList1 = obj.ToList();
                                            }
                                        }
                                    }

                                    for (int i = 0; i < result.Count; i++)
                                    {
                                        studentList1.Add(result[i]);
                                    }

                                    data.studentList = studentList1.ToArray();
                                    data.attcount = 1;
                                }
                                catch (Exception ex)
                                {
                                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                    Console.Write(ex.Message);
                                }
                            }
                            else
                            {
                                data.message = "Already Attendance Is Enter For This Period , Class And Section So You Can Not Entre Again For This Period";
                            }
                        }
                        else
                        {
                            List<StudentAttTempDTO> arrAttdto = new List<StudentAttTempDTO>();
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Getstudentdetailsforsubjectwise";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag });
                                cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@isms_id", SqlDbType.VarChar) { Value = data.ismS_Id });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            if (dataReader["studentname"] != System.DBNull.Value)
                                            {
                                                Student_name_null = Convert.ToString(dataReader["studentname"]);
                                            }
                                            else
                                            {
                                                Student_name_null = "NOT AVAILABLE";
                                            }


                                            if (dataReader["AMST_AdmNo"] != System.DBNull.Value)
                                            {
                                                AMST_ADM_null = Convert.ToString(dataReader["AMST_AdmNo"]);
                                            }
                                            else
                                            {
                                                AMST_ADM_null = "NOT AVAILABLE";
                                            }

                                            if (dataReader["amsT_RegistrationNo"] != System.DBNull.Value)
                                            {
                                                amsT_RegistrationNo = Convert.ToString(dataReader["amsT_RegistrationNo"]);
                                            }
                                            else
                                            {
                                                amsT_RegistrationNo = "NOT AVAILABLE";
                                            }
                                            arrAttdto.Add(new StudentAttTempDTO
                                            {
                                                amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                studentname = Student_name_null,
                                                amsT_AdmNo = AMST_ADM_null,
                                                amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                                amsT_RegistrationNo = amsT_RegistrationNo,
                                            });
                                            data.studentList = arrAttdto.ToArray();
                                        }
                                    }
                                    data.attcount = 0;
                                }
                                catch (Exception ex)
                                {
                                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                    Console.Write(ex.Message);
                                }
                            }

                        }
                    }
                    //----IF elective means----//
                    else
                    {
                        var checkperiodattentry = (from a in _db.Adm_studentAttendance
                                                   from b in _db.Adm_studentAttendanceStudents
                                                   from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                                   from d in _db.Adm_studentAttendanceSubjects
                                                   where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id
                                                   && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && c.TTMP_Id == data.TTMP_Id
                                                   && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate && a.ASA_Activeflag == true)
                                                   select new StudentAttendanceEntryDTO
                                                   {
                                                       ASA_Id = c.ASA_Id,
                                                       TTMP_Id = c.TTMP_Id
                                                   }).Distinct().ToList();



                        if (checkperiodattentry.Count > 0)
                        {
                            var checkperiodsubjectatt = (from a in _db.Adm_studentAttendance
                                                         from b in _db.Adm_studentAttendanceStudents
                                                         from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                                         from d in _db.Adm_studentAttendanceSubjects
                                                         where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id
                                                         && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                                         && d.ISMS_Id == data.ismS_Id && c.TTMP_Id == data.TTMP_Id && a.ASMS_Id == data.ASMS_Id
                                                         && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate && a.ASA_Activeflag == true)
                                                         select new StudentAttendanceEntryDTO
                                                         {
                                                             ASA_Id = c.ASA_Id,
                                                             TTMP_Id = c.TTMP_Id,
                                                             ismS_Id = d.ISMS_Id
                                                         }).Distinct().ToList();
                            if (checkperiodsubjectatt.Count > 0)
                            {
                                try
                                {
                                    data.monthid = 0;
                                    List<StudentAttTempDTO> studentList1 = new List<StudentAttTempDTO>();
                                    List<StudentAttTempDTO> result = new List<StudentAttTempDTO>();
                                    List<StudentAttTempDTO> obj = new List<StudentAttTempDTO>();

                                    var type = _db.AttendanceEntryTypeDMO.Where(t => t.ASMCL_Id.Equals(data.ASMCL_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id) 
                                    && t.MI_Id.Equals(data.MI_Id)).ToArray();
                                    data.ASA_Att_EntryType = type[0].ASAET_Att_Type;
                                    var getregularorextra = _db.Adm_studentAttendance.Where(t => t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id 
                                    && t.ASMAY_Id == data.ASMAY_Id && t.ASA_Att_Type.Equals(data.monthflag1) && t.ASA_FromDate == data.ASA_FromDate 
                                    && t.MI_Id == data.MI_Id).ToList();
                                    data.ASA_Regular_Extra = getregularorextra.FirstOrDefault().ASA_Regular_Extra;
                                    try
                                    {
                                        _acdimpl.LogInformation("entered try block");
                                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                        {
                                            _acdimpl.LogInformation("entered cmd getdbconnection");
                                            cmd.CommandText = "adm_student_list_not_in_att_subject";
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMCL_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMS_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = confromdate });
                                            cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = Convert.ToString(data.monthflag) });
                                            cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(data.monthid) });
                                            cmd.Parameters.Add(new SqlParameter("@isms_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ismS_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@ttmp_id", SqlDbType.VarChar) { Value = Convert.ToString(data.TTMP_Id) });
                                            if (cmd.Connection.State != ConnectionState.Open)
                                                cmd.Connection.Open();
                                            _acdimpl.LogInformation("entered if block");
                                            _acdimpl.LogInformation("Fromdate :'" + data.ASA_FromDate + "");
                                            var retObject = new List<dynamic>();
                                            try
                                            {
                                                using (var dataReader = cmd.ExecuteReader())
                                                {
                                                    _acdimpl.LogInformation("entered in dataReader block");
                                                    while (dataReader.Read())
                                                    {
                                                        _acdimpl.LogInformation("entered in while block");

                                                        result.Add(new StudentAttTempDTO
                                                        {
                                                            amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                            studentname = (dataReader["studentname"]).ToString(),
                                                            amsT_AdmNo = (dataReader["AMST_AdmNo"]).ToString(),
                                                            amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                                            amsT_RegistrationNo = (dataReader["amsT_RegistrationNo"]).ToString(),
                                                        });
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                                Console.Write(ex.Message);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                        Console.Write(ex.Message);
                                    }


                                    if (data.monthflag == "P")
                                    {
                                        using (var command = _db.Database.GetDbConnection().CreateCommand())
                                        {
                                            command.CommandText = " Select Distinct b.ASA_Class_Attended  as ASA_Class_Attended, b.ASAS_Id as ASAS_Id, a.ASA_Id as ASA_Id, (d.AMST_Id) AMST_Id, (CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+" +
                                        "CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END + " +
                                        "CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END) AS studentname," +
                                        "(d.AMST_AdmNo)AMST_AdmNo, (c.AMAY_RollNo) AMAY_RollNo, AMST_Sex, (amsT_RegistrationNo) as amsT_RegistrationNo from Adm_Student_Attendance a " +
                                        "INNER JOIN Adm_Student_Attendance_Students b on  a.asa_id=b.asa_id inner join  Adm_School_Y_Student c on c.amst_id=b.amst_id " +
                                        " Inner Join  Adm_M_student d on d.amst_id=c.amst_id  inner join Adm_School_M_Academic_Year dd on dd.asmay_id=c.ASMAY_Id inner join Adm_Student_Attendance_Periodwise k on k.asa_id=a.asa_id " +
                                        " inner join adm_student_attendance_subjects l on l.asa_id=a.asa_id inner join Exm.Exm_Studentwise_Subjects m on m.amst_id=c.amst_id   and m.asmay_id=" + data.ASMAY_Id + " where  ASA_Activeflag=1 and c.asmcl_id=" + data.ASMCL_Id + "  and c.ASMS_Id =" + data.ASMS_Id + " and c.ASMAY_Id =" + data.ASMAY_Id + " " +
                                        "and a.MI_Id =" + data.MI_Id + " and a.ASA_FromDate = '" + confromdate + "' and  a.ASA_Att_Type ='" + data.monthflag1 + "' " +
                                        " and d.AMST_ActiveFlag = 1 and d.AMST_SOL = 'S' and c.AMAY_ActiveFlag = 1 and  l.ISMS_Id =" + data.ismS_Id + " and m.ISMS_Id =" + data.ismS_Id + " and k.TTMP_Id =" + data.TTMP_Id + " " + ordderby + " ";
                                            _db.Database.OpenConnection();
                                            using (var result1 = command.ExecuteReader())
                                            {
                                                while (result1.Read())
                                                {
                                                    obj.Add(new StudentAttTempDTO
                                                    {
                                                        amsT_Id = Convert.ToInt64(result1["AMST_Id"]),
                                                        studentname = result1["studentname"].ToString(),
                                                        amsT_AdmNo = result1["AMST_AdmNo"].ToString(),
                                                        amaY_RollNo = Convert.ToInt64(result1["AMAY_RollNo"]),
                                                        amsT_RegistrationNo = result1["amsT_RegistrationNo"].ToString(),
                                                        pdays = Convert.ToDecimal(result1["ASA_Class_Attended"]),
                                                        ASAS_Id = Convert.ToInt64(result1["ASAS_Id"]),
                                                        asA_Id = Convert.ToInt64(result1["ASA_Id"]),
                                                    });
                                                }

                                                studentList1 = obj.ToList();
                                            }
                                        }

                                    }

                                    for (int i = 0; i < result.Count; i++)
                                    {
                                        studentList1.Add(result[i]);
                                    }

                                    data.studentList = studentList1.ToArray();
                                    data.attcount = 1;
                                }
                                catch (Exception ex)
                                {
                                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                    Console.Write(ex.Message);
                                }
                            }
                            else
                            {
                                var check_entired_subjectelectiveornot = (from a in _db.Adm_studentAttendance
                                                                          from b in _db.Adm_studentAttendanceSubjects
                                                                          from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                                                          from d in _db.Adm_studentAttendanceStudents
                                                                          where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id
                                                                          && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                                                          && c.TTMP_Id == data.TTMP_Id && a.ASA_Activeflag == true
                                                                          && a.ASA_FromDate == data.ASA_FromDate)
                                                                          select new StudentAttendanceEntryDTO
                                                                          {
                                                                              ismS_Id = b.ISMS_Id
                                                                          }).Distinct().ToArray();

                                var check_entired_subjectelectiveornot1 = _db.StudentMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_Id == check_entired_subjectelectiveornot.FirstOrDefault().ismS_Id
                                  && a.ESTSU_ElecetiveFlag == true).ToList();
                                if (check_entired_subjectelectiveornot1.Count > 0)
                                {
                                    List<StudentAttTempDTO> arrAttdto = new List<StudentAttTempDTO>();
                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandText = "Getstudentdetailsforsubjectwise";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                        cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = data.MI_Id });
                                        cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag });
                                        cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                        cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                        cmd.Parameters.Add(new SqlParameter("@isms_id", SqlDbType.VarChar) { Value = data.ismS_Id });
                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();

                                        var retObject = new List<dynamic>();
                                        try
                                        {
                                            using (var dataReader = cmd.ExecuteReader())
                                            {
                                                while (dataReader.Read())
                                                {
                                                    if (dataReader["studentname"] != System.DBNull.Value)
                                                    {
                                                        Student_name_null = Convert.ToString(dataReader["studentname"]);
                                                    }
                                                    else
                                                    {
                                                        Student_name_null = "NOT AVAILABLE";
                                                    }


                                                    if (dataReader["AMST_AdmNo"] != System.DBNull.Value)
                                                    {
                                                        AMST_ADM_null = Convert.ToString(dataReader["AMST_AdmNo"]);
                                                    }
                                                    else
                                                    {
                                                        AMST_ADM_null = "NOT AVAILABLE";
                                                    }

                                                    if (dataReader["amsT_RegistrationNo"] != System.DBNull.Value)
                                                    {
                                                        amsT_RegistrationNo = Convert.ToString(dataReader["amsT_RegistrationNo"]);
                                                    }
                                                    else
                                                    {
                                                        amsT_RegistrationNo = "NOT AVAILABLE";
                                                    }
                                                    arrAttdto.Add(new StudentAttTempDTO
                                                    {
                                                        amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                        studentname = Student_name_null,
                                                        amsT_AdmNo = AMST_ADM_null,
                                                        amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                                        amsT_RegistrationNo = amsT_RegistrationNo,
                                                    });
                                                    data.studentList = arrAttdto.ToArray();
                                                }
                                            }
                                            data.attcount = 0;
                                        }
                                        catch (Exception ex)
                                        {
                                            _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                            Console.Write(ex.Message);
                                        }
                                    }
                                }
                                else
                                {
                                    data.message = "Already Attendance Is Enter For This Period , Class And Section So You Can Not Entre Again For This Period";
                                }
                            }

                        }
                        else
                        {
                            List<StudentAttTempDTO> arrAttdto = new List<StudentAttTempDTO>();
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Getstudentdetailsforsubjectwise";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag });
                                cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@isms_id", SqlDbType.VarChar) { Value = data.ismS_Id });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var retObject = new List<dynamic>();
                                try
                                {
                                    using (var dataReader = cmd.ExecuteReader())
                                    {
                                        while (dataReader.Read())
                                        {
                                            if (dataReader["studentname"] != System.DBNull.Value)
                                            {
                                                Student_name_null = Convert.ToString(dataReader["studentname"]);
                                            }
                                            else
                                            {
                                                Student_name_null = "NOT AVAILABLE";
                                            }


                                            if (dataReader["AMST_AdmNo"] != System.DBNull.Value)
                                            {
                                                AMST_ADM_null = Convert.ToString(dataReader["AMST_AdmNo"]);
                                            }
                                            else
                                            {
                                                AMST_ADM_null = "NOT AVAILABLE";
                                            }

                                            if (dataReader["amsT_RegistrationNo"] != System.DBNull.Value)
                                            {
                                                amsT_RegistrationNo = Convert.ToString(dataReader["amsT_RegistrationNo"]);
                                            }
                                            else
                                            {
                                                amsT_RegistrationNo = "NOT AVAILABLE";
                                            }
                                            arrAttdto.Add(new StudentAttTempDTO
                                            {
                                                amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                studentname = Student_name_null,
                                                amsT_AdmNo = AMST_ADM_null,
                                                amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                                amsT_RegistrationNo = amsT_RegistrationNo,
                                            });
                                            data.studentList = arrAttdto.ToArray();
                                        }
                                    }
                                    data.attcount = 0;
                                }
                                catch (Exception ex)
                                {
                                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                    Console.Write(ex.Message);
                                }
                            }
                        }

                    }
                }
                //----------Conditions For The Batch Wise Attendance --------------//
                else
                {
                    //---Checking Whether Selected Period Attendance Is Done Or Not For That Class , Section , Year, Date And Period---//
                    var checkperiodattentry = (from a in _db.Adm_studentAttendance
                                               from b in _db.Adm_studentAttendanceStudents
                                               from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                               from d in _db.Adm_studentAttendanceSubjects
                                               where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                               && a.ASMCL_Id == data.ASMCL_Id && a.ASA_Activeflag == true
                                               && a.ASMS_Id == data.ASMS_Id && c.TTMP_Id == data.TTMP_Id && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate)
                                               select new StudentAttendanceEntryDTO
                                               {
                                                   ASA_Id = c.ASA_Id,
                                                   TTMP_Id = c.TTMP_Id
                                               }).Distinct().ToList();

                    if (checkperiodattentry.Count > 0)
                    {
                        //--------If Period Attendance Happend For That Period Then Checking Period With Subject--------//
                        var checkperiodsubjectatt = (from a in _db.Adm_studentAttendance
                                                     from b in _db.Adm_studentAttendanceStudents
                                                     from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                                     from d in _db.Adm_studentAttendanceSubjects
                                                     where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id
                                                     && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASA_Activeflag == true
                                                     && d.ISMS_Id == data.ismS_Id && c.TTMP_Id == data.TTMP_Id && a.ASMS_Id == data.ASMS_Id && a.ASA_Att_Type == data.monthflag1
                                                     && a.ASA_FromDate == data.ASA_FromDate)
                                                     select new StudentAttendanceEntryDTO
                                                     {
                                                         ASA_Id = c.ASA_Id,
                                                         TTMP_Id = c.TTMP_Id,
                                                         ismS_Id = d.ISMS_Id
                                                     }).Distinct().ToList();

                        if (checkperiodsubjectatt.Count > 0)
                        {
                            //--------If Period Attendance Happend For That Period Then Checking Period With Subject With Respect To Batch--------//
                            var checkperiodsubjectbatchatt = (from a in _db.Adm_studentAttendance
                                                              from b in _db.Adm_studentAttendanceStudents
                                                              from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                                              from d in _db.Adm_studentAttendanceSubjects
                                                              from e in _db.AdmSchoolAttendanceSubjectBatch
                                                              from f in _db.AdmSchoolAttendanceSubjectBatchStudents
                                                              where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && e.ASASB_Id == f.ASASB_Id
                                                              && f.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                                              && d.ISMS_Id == data.ismS_Id && c.TTMP_Id == data.TTMP_Id && a.ASMS_Id == data.ASMS_Id
                                                              && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate && e.ASASB_Id == data.asasB_Id
                                                              && a.ASA_Activeflag == true)
                                                              select new StudentAttendanceEntryDTO
                                                              {
                                                                  ASA_Id = c.ASA_Id,
                                                                  TTMP_Id = c.TTMP_Id,
                                                                  ismS_Id = d.ISMS_Id
                                                              }).Distinct().ToList();
                            if (checkperiodsubjectbatchatt.Count > 0)
                            {
                                //--Retrieving The Saved Data For That Date , Class , Section , Subject, Period--//
                                try
                                {
                                    data.monthid = 0;
                                    List<StudentAttTempDTO> studentList1 = new List<StudentAttTempDTO>();
                                    List<StudentAttTempDTO> result = new List<StudentAttTempDTO>();
                                    List<StudentAttTempDTO> obj = new List<StudentAttTempDTO>();

                                    var type = _db.AttendanceEntryTypeDMO.Where(t => t.ASMCL_Id.Equals(data.ASMCL_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id) && t.MI_Id.Equals(data.MI_Id)).ToArray();
                                    data.ASA_Att_EntryType = type[0].ASAET_Att_Type;
                                    var getregularorextra = _db.Adm_studentAttendance.Where(t => t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASA_Att_Type.Equals(data.monthflag1) && t.ASA_FromDate == data.ASA_FromDate && t.MI_Id == data.MI_Id).ToList();
                                    data.ASA_Regular_Extra = getregularorextra.FirstOrDefault().ASA_Regular_Extra;
                                    try
                                    {
                                        _acdimpl.LogInformation("entered try block");
                                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                        {
                                            _acdimpl.LogInformation("entered cmd getdbconnection");
                                            cmd.CommandText = "adm_student_list_not_in_att_batch";
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMCL_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMS_Id) });
                                            // cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = Convert.ToString(data.ASA_FromDate) });
                                            //cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = Convert.ToString(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd")) });
                                            cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = confromdate });
                                            cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = Convert.ToString(data.monthflag) });
                                            cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(data.monthid) });
                                            cmd.Parameters.Add(new SqlParameter("@asasb_id", SqlDbType.VarChar) { Value = Convert.ToString(data.asasB_Id) });
                                            if (cmd.Connection.State != ConnectionState.Open)
                                                cmd.Connection.Open();
                                            _acdimpl.LogInformation("entered if block");
                                            _acdimpl.LogInformation("Fromdate :'" + data.ASA_FromDate + "");
                                            var retObject = new List<dynamic>();
                                            try
                                            {
                                                using (var dataReader = cmd.ExecuteReader())
                                                {
                                                    _acdimpl.LogInformation("entered in dataReader block");
                                                    while (dataReader.Read())
                                                    {
                                                        _acdimpl.LogInformation("entered in while block");

                                                        result.Add(new StudentAttTempDTO
                                                        {
                                                            amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                            studentname = (dataReader["studentname"]).ToString(),
                                                            amsT_AdmNo = (dataReader["AMST_AdmNo"]).ToString(),
                                                            amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                                            amsT_RegistrationNo = (dataReader["amsT_RegistrationNo"]).ToString(),
                                                        });
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                                Console.Write(ex.Message);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                        Console.Write(ex.Message);
                                    }


                                    if (data.monthflag == "P")
                                    {
                                        using (var command = _db.Database.GetDbConnection().CreateCommand())
                                        {
                                            command.CommandText = " Select Distinct b.ASA_Class_Attended  as ASA_Class_Attended, b.ASAS_Id as ASAS_Id, a.ASA_Id as ASA_Id, (d.AMST_Id) AMST_Id, (CASE WHEN  AMST_FirstName is null or AMST_FirstName=''  then '' else AMST_FirstName end+" +
                                        "CASE WHEN  AMST_MiddleName is null or AMST_MiddleName = '' or AMST_MiddleName = '0' then ''  ELSE ' ' + AMST_MiddleName END + " +
                                        "CASE WHEN AMST_LastName is null or AMST_LastName = '' or AMST_LastName = '0' then ''  ELSE ' ' + AMST_LastName END) AS studentname," +
                                        "(d.AMST_AdmNo)AMST_AdmNo, (c.AMAY_RollNo) AMAY_RollNo, AMST_Sex, (amsT_RegistrationNo) as amsT_RegistrationNo from Adm_Student_Attendance a " +
                                        "INNER JOIN Adm_Student_Attendance_Students b on  a.asa_id=b.asa_id inner join  Adm_School_Y_Student c on c.amst_id=b.amst_id " +
                                        " Inner Join  Adm_M_student d on d.amst_id=c.amst_id  inner join Adm_School_M_Academic_Year dd on dd.asmay_id=c.ASMAY_Id  inner join Adm_Student_Attendance_Periodwise k on k.asa_id=a.asa_id " +
                                        " inner join adm_student_attendance_subjects l on l.asa_id=a.asa_id inner join Adm_School_Attendance_Subject_Batch_Students m on m.amst_id=c.amst_id " +
                                        "inner join Adm_School_Attendance_Subject_Batch n on n.asasb_id=m.asasb_id where ASA_Activeflag=1 and c.asmcl_id=" + data.ASMCL_Id + "  and c.ASMS_Id =" + data.ASMS_Id + " and c.ASMAY_Id =" + data.ASMAY_Id + " " +
                                        "and a.MI_Id =" + data.MI_Id + " and a.ASA_FromDate = '" + confromdate + "' and  a.ASA_Att_Type ='" + data.monthflag1 + "' " +
                                        " and d.AMST_ActiveFlag = 1 and d.AMST_SOL = 'S' and c.AMAY_ActiveFlag = 1 and  l.ISMS_Id =" + data.ismS_Id + " and n.ISMS_Id =" + data.ismS_Id + " and k.TTMP_Id =" + data.TTMP_Id + " and n.ASASB_Id = " + data.asasB_Id + " " + ordderby + " ";
                                            _db.Database.OpenConnection();
                                            using (var result1 = command.ExecuteReader())
                                            {
                                                while (result1.Read())
                                                {
                                                    obj.Add(new StudentAttTempDTO
                                                    {
                                                        amsT_Id = Convert.ToInt64(result1["AMST_Id"]),
                                                        studentname = result1["studentname"].ToString(),
                                                        amsT_AdmNo = result1["AMST_AdmNo"].ToString(),
                                                        amaY_RollNo = Convert.ToInt64(result1["AMAY_RollNo"]),
                                                        amsT_RegistrationNo = result1["amsT_RegistrationNo"].ToString(),
                                                        pdays = Convert.ToDecimal(result1["ASA_Class_Attended"]),
                                                        ASAS_Id = Convert.ToInt64(result1["ASAS_Id"]),
                                                        asA_Id = Convert.ToInt64(result1["ASA_Id"]),
                                                    });
                                                }

                                                studentList1 = obj.ToList();
                                            }
                                        }
                                    }

                                    for (int i = 0; i < result.Count; i++)
                                    {
                                        studentList1.Add(result[i]);
                                    }

                                    data.studentList = studentList1.ToArray();
                                    data.attcount = 1;
                                }
                                catch (Exception ex)
                                {
                                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                    Console.Write(ex.Message);
                                }
                            }
                            //--Getting Student Details Based On the Batch for that subject--//
                            else
                            {
                                List<StudentAttTempDTO> arrAttdto = new List<StudentAttTempDTO>();
                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandText = "GetStudentDataByAdecmicYearClassSectionBatchwise";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = data.MI_Id });
                                    cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag });
                                    cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                                    cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                                    cmd.Parameters.Add(new SqlParameter("@asab_id", SqlDbType.VarChar) { Value = data.asasB_Id });
                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();

                                    var retObject = new List<dynamic>();
                                    try
                                    {
                                        using (var dataReader = cmd.ExecuteReader())
                                        {
                                            while (dataReader.Read())
                                            {
                                                if (dataReader["studentname"] != System.DBNull.Value)
                                                {
                                                    Student_name_null = Convert.ToString(dataReader["studentname"]);
                                                }
                                                else
                                                {
                                                    Student_name_null = "NOT AVAILABLE";
                                                }


                                                if (dataReader["AMST_AdmNo"] != System.DBNull.Value)
                                                {
                                                    AMST_ADM_null = Convert.ToString(dataReader["AMST_AdmNo"]);
                                                }
                                                else
                                                {
                                                    AMST_ADM_null = "NOT AVAILABLE";
                                                }

                                                if (dataReader["amsT_RegistrationNo"] != System.DBNull.Value)
                                                {
                                                    amsT_RegistrationNo = Convert.ToString(dataReader["amsT_RegistrationNo"]);
                                                }
                                                else
                                                {
                                                    amsT_RegistrationNo = "NOT AVAILABLE";
                                                }

                                                arrAttdto.Add(new StudentAttTempDTO
                                                {
                                                    amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                    studentname = Student_name_null,
                                                    amsT_AdmNo = AMST_ADM_null,
                                                    amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                                    amsT_RegistrationNo = amsT_RegistrationNo,
                                                });
                                                data.studentList = arrAttdto.ToArray();
                                            }
                                        }
                                        data.attcount = 0;
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                        Console.Write(ex.Message);
                                    }
                                }
                            }
                        }
                        else
                        {
                            data.message = "Already Attendance Is Enter For This Period , Class And Section So You Can Not Entre Again For This Period";
                        }
                    }
                    else
                    {
                        List<StudentAttTempDTO> arrAttdto = new List<StudentAttTempDTO>();
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GetStudentDataByAdecmicYearClassSectionBatchwise";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag });
                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@asab_id", SqlDbType.VarChar) { Value = data.asasB_Id });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        if (dataReader["studentname"] != System.DBNull.Value)
                                        {
                                            Student_name_null = Convert.ToString(dataReader["studentname"]);
                                        }
                                        else
                                        {
                                            Student_name_null = "NOT AVAILABLE";
                                        }


                                        if (dataReader["AMST_AdmNo"] != System.DBNull.Value)
                                        {
                                            AMST_ADM_null = Convert.ToString(dataReader["AMST_AdmNo"]);
                                        }
                                        else
                                        {
                                            AMST_ADM_null = "NOT AVAILABLE";
                                        }

                                        if (dataReader["amsT_RegistrationNo"] != System.DBNull.Value)
                                        {
                                            amsT_RegistrationNo = Convert.ToString(dataReader["amsT_RegistrationNo"]);
                                        }
                                        else
                                        {
                                            amsT_RegistrationNo = "NOT AVAILABLE";
                                        }

                                        arrAttdto.Add(new StudentAttTempDTO
                                        {
                                            amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                            studentname = Student_name_null,
                                            amsT_AdmNo = AMST_ADM_null,
                                            amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                            amsT_RegistrationNo = amsT_RegistrationNo,
                                        });
                                        data.studentList = arrAttdto.ToArray();
                                    }
                                }
                                data.attcount = 0;
                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                Console.Write(ex.Message);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error At getstdlistperiod List In Attendance Entry:" + ex.Message);
            }
            return data;
        }
        public StudentAttendanceEntryDTO ViewAttendanceDetailsStaffWise(StudentAttendanceEntryDTO data)
        {
            try
            {
                var get_Staffdetails = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();
                long HRME_Id = 0;
                if (get_Staffdetails.Count > 0)
                {
                    HRME_Id = get_Staffdetails.FirstOrDefault().Emp_Code;
                }
                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                fromdatecon = Convert.ToDateTime(DateTime.UtcNow.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                if (data.att_entry_type == "period")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_View_Periodwise_Attendance_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@att_entry_type", SqlDbType.VarChar) { Value = data.att_entry_type });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }

                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.ViewStudentPeriodWiseAttDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_View_Staffwise_AttendanceEntry_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = HRME_Id });
                        cmd.Parameters.Add(new SqlParameter("@Date", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@att_entry_type", SqlDbType.VarChar) { Value = data.att_entry_type });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.ViewStudentPeriodWiseAttDetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceEntryDTO AttendanceDeleteRecordWise(StudentAttendanceEntryDTO data)
        {
            try
            {
                data.returnval = false;
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime2 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var result = _db.Adm_studentAttendance.Single(d => d.ASA_Id == data.ASA_Id);
                result.ASA_Activeflag = false;
                result.ASA_UpdatedBy = data.userId;
                result.UpdatedDate = indianTime2;
                _db.Update(result);
                int i = _db.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }

                StudentAttendanceEntryDTO data_temp = new StudentAttendanceEntryDTO();
                data_temp = ViewAttendanceDetailsStaffWise(data);
                data.ViewStudentPeriodWiseAttDetails = data_temp.ViewStudentPeriodWiseAttDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<string> getuserid(string data)
        {
            string username = data.ToString();
            string id = "";
            ApplicationUser user = new ApplicationUser();
            user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                id = user.Id.ToString(); ;
            }
            return id;
        }

        //getting the smart card details Onload function
        public class_section_list getSmartCardData(class_section_list data)
        {
            try
            {
                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new class_section_list
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var userid = getuserid(data.username);

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new class_section_list
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (check_rolename.FirstOrDefault().rolename.Equals("ADMIN") || check_rolename.FirstOrDefault().rolename.Equals("admin"))
                {
                    data.classlist = _db.School_M_Class.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).ToArray();
                    data.sectionlist = _db.Section.Where(d => d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1).ToArray();
                }
                else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User"))
                {
                    data.classlist = _db.School_M_Class.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).ToArray();
                    data.sectionlist = _db.Section.Where(d => d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1).ToArray();
                }
                else if (check_rolename.FirstOrDefault().rolename.Equals("Super Admin"))
                {
                    data.classlist = _db.School_M_Class.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).ToArray();
                    data.sectionlist = _db.Section.Where(d => d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1).ToArray();
                }
                else if (check_rolename.FirstOrDefault().rolename.Equals("Admission End User") || check_rolename.FirstOrDefault().rolename.Equals("Admission End User"))
                {
                    data.classlist = _db.School_M_Class.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).ToArray();
                    data.sectionlist = _db.Section.Where(d => d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1).ToArray();
                }
                else
                {
                    if (empcode_check.Count > 0)
                    {
                        data.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.School_M_Class
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new class_section_list
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }
                                      ).Distinct().ToArray();


                        data.sectionlist = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.School_M_Section
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                            && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                            && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                            && c.ASMC_ActiveFlag == 1)
                                            select new class_section_list
                                            {
                                                ASMS_Id = c.ASMS_Id,
                                                asmC_SectionName = c.ASMC_SectionName,
                                            }
                                            ).Distinct().ToArray();
                    }
                    else
                    {
                        data.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Attendance Entry loadpage :'" + ex.Message + "'");
                //Console.Write(ex.Message);
            }
            return data;
        }

        //saving the smart card attendnace while reading the card punch
        public async Task<class_section_list> SaveSmartCardData(class_section_list data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime2 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                //checking data is pushed to main table or not
                var check = (from a in _db.Adm_studentAttendance
                             from b in _db.Adm_studentAttendanceStudents
                             where (a.ASA_Id == b.ASA_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                             && a.ASA_FromDate == data.ASA_FromDate)
                             select new class_section_list
                             {
                                 AMST_Id = b.AMST_Id
                             }).ToList();

                if (check.Count() > 0)
                {
                    data.message = "Sorry Todays Attendance Already Collected No Need Of Punch Again";
                }
                else
                {

                    var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                    var userid = getuserid(data.username);

                    long? user_id = Convert.ToInt64(userid);

                    var empcode_check = (from a in _db.Staff_User_Login
                                         where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                         select new class_section_list
                                         {
                                             Emp_Code = a.Emp_Code,
                                         }).ToList();

                    var get_class_section = (from a in _db.Adm_M_Student
                                             from b in _db.School_Adm_Y_StudentDMO
                                             from c in _db.School_M_Class
                                             from d in _db.School_M_Section
                                             from e in _db.AcademicYear
                                             where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.ASMAY_Id == e.ASMAY_Id && a.MI_Id == data.MI_Id
                                             && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id)
                                             select new class_section_list
                                             {
                                                 ASMCL_Id = b.ASMCL_Id,
                                                 ASMS_Id = b.ASMS_Id,
                                                 AMST_AdmNo = a.AMST_AdmNo,
                                                 studentname = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                                 asmcL_ClassName = c.ASMCL_ClassName,
                                                 asmC_SectionName = d.ASMC_SectionName
                                             }).ToList();

                    data.get_cls_section = get_class_section.ToArray();

                    var emp_att_login_check = (from a in _db.attloginuser
                                               from c in _db.Adm_SchAttLoginUserClass
                                               where (a.ASALU_Id == c.ASALU_Id && a.MI_Id == data.MI_Id &&
                                               a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                               && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id
                                               && a.ASMAY_Id == data.ASMAY_Id
                                             )
                                               select new class_section_list
                                               {
                                                   ASALU_Id = a.ASALU_Id,
                                               }).Distinct().ToList();


                    if (emp_att_login_check.Count == 0)
                    {
                        data.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Map The Saff Attendance Privileges";
                        return data;
                    }

                    var adm = (from a in _db.SchoolYearWiseStudent
                               from b in _db.Adm_M_Student
                               from c in _db.attloginuser
                               from d in _db.Adm_SchAttLoginUserClass
                               where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                               && c.ASALU_Id == d.ASALU_Id && d.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == data.ASMS_Id
                               && c.ASMAY_Id == data.ASMAY_Id && c.ASALU_Id == emp_att_login_check.FirstOrDefault().ASALU_Id
                               && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                               select new class_section_list
                               {
                                   ASMCL_Id = a.ASMCL_Id,
                                   ASMS_Id = a.ASMS_Id,

                               }).ToList();


                    if (adm.Count == 0)
                    {
                        data.message = "Sorry Other Class Student";
                        return data;
                    }
                    Attendance_Students_SmartCard objpge = new Attendance_Students_SmartCard();
                    try
                    {
                        var checkduplicate = _db.Attendance_Students_SmartCard.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                          && a.ASSC_AttendanceDate.Value.Date == data.ASA_FromDate.Value.Date).Select(d => new { d.ASSC_Id, d.AMST_Id }).ToList();
                        if (checkduplicate.Count > 0)
                        {
                            var result = _db.Attendance_Students_SmartCard.Single(a => a.ASSC_Id == checkduplicate.FirstOrDefault().ASSC_Id);
                            result.ASSC_PunchDate = data.ASA_FromDate;
                            result.ASSC_PunchTime = data.ASSC_PunchTime;
                            _db.Update(result);
                        }
                        else
                        {
                            objpge.AMST_Id = data.AMST_Id;
                            objpge.ASMCL_Id = data.ASMCL_Id;
                            objpge.ASMS_Id = data.ASMS_Id;
                            objpge.ASMAY_Id = data.ASMAY_Id;
                            objpge.MI_Id = data.MI_Id;
                            objpge.ASSC_AttendanceDate = data.ASA_FromDate;
                            objpge.ASSC_PunchDate = data.ASA_FromDate;
                            objpge.ASSC_SystemIP = data.ASA_Network_IP;
                            objpge.ASSC_NetworkIP = data.ASA_Network_IP;
                            objpge.ASSC_PunchTime = data.ASSC_PunchTime;
                            objpge.CreatedDate = indianTime2;
                            objpge.UpdatedDate = indianTime2;
                            objpge.ASSC_CreatedBy = user_id;
                            objpge.ASSC_UpdatedBy = user_id;
                            objpge.ASALU_Id = emp_att_login_check.FirstOrDefault().ASALU_Id;
                            objpge.HRME_Id = empcode_check.FirstOrDefault().Emp_Code;
                            _db.Add(objpge);
                        }
                        var i = _db.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Record Saved / Updated";
                        }
                        else
                        {
                            data.message = "Record Not Saved / Updated";
                        }
                        if (data.offline == "ofline")
                        {
                            List<long> GrpId = new List<long>();

                            data.Studentattsmartcardabsent = (from a in _db.Attendance_Students_SmartCard
                                                              where a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id 
                                                              && a.MI_Id == data.MI_Id
                                                              && a.ASSC_AttendanceDate == data.ASA_FromDate
                                                              select new Studentattsmartcardabsent
                                                              {
                                                                  AMST_Id = a.AMST_Id
                                                              }).ToArray();
                            data.total_punch = Convert.ToString(data.Studentattsmartcardabsent.Count());

                            foreach (var item in data.Studentattsmartcardabsent)
                            {
                                GrpId.Add(item.AMST_Id);
                            }

                            data.studentlist = (from a in _db.School_Adm_Y_StudentDMO
                                                from b in _db.Adm_M_Student
                                                where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                                && b.MI_Id == data.MI_Id && b.AMST_SOL == "S" && a.AMAY_ActiveFlag == 1 && b.AMST_ActiveFlag == 1 && !GrpId.Contains(a.AMST_Id))
                                                select new class_section_list
                                                {
                                                    AMST_Id = b.AMST_Id,
                                                    studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName) + ":" + (b.AMST_AdmNo == null ? " " : b.AMST_AdmNo)).Trim()

                                                }).ToArray();

                            data.not_punch = Convert.ToString(data.studentlist.Length);

                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Error save attendance smart card :'" + ex.Message + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error save attendance smart card :'" + ex.Message + "'");
            }
            return data;

        }

        //sending the sms for absent list through scheduler
        public StudentAttendanceEntryDTO sendsmsabsent(StudentAttendanceEntryDTO data)
        {
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var acd_name = _db.AcademicYear.Where(t => t.ASMAY_Id == acd_Id).Select(d => d.ASMAY_Year).FirstOrDefault();

                data.ASMAY_Id = acd_Id;


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                //DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    indianTime = Convert.ToDateTime(indianTime.Date.ToString("yyyy-MM-dd"));
                    confromdate = indianTime.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                List<Absent_Student_List> absentlist = new List<Absent_Student_List>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    _acdimpl.LogInformation("entered cmd getdbconnection");
                    cmd.CommandText = "Adm_Get_Today_Absent_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Mi_id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    _acdimpl.LogInformation("entered if block");

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            _acdimpl.LogInformation("entered in dataReader block");
                            while (dataReader.Read())
                            {
                                absentlist.Add(new Absent_Student_List
                                {
                                    Amst_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("error:'" + ex.Message + "'");
                        Console.Write(ex.Message);
                    }
                }

                for (int k = 0; k < absentlist.Count; k++)
                {
                    try
                    {
                        var admConfig = _db.AdmissionStandardDMO.Single(t => t.MI_Id == data.MI_Id);
                        var studDet = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == absentlist[k].Amst_Id).ToList();


                        var template = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "Student_Absent_SMS").ToList();

                        // ----- SMS ------//
                        if (template.FirstOrDefault().ISES_SMSActiveFlag == true)
                        {
                            if (admConfig.ASC_DefaultSMS_Flag == "M" && studDet.FirstOrDefault().AMST_MotherMobileNo != null)
                            {
                                try
                                {
                                    SMS sms = new SMS(_db);
                                    string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "Student_Absent_SMS", absentlist[k].Amst_Id).Result;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    _acdimpl.LogInformation("Sendsmsabsent attendance entry mobile M" + ex.Message);
                                }
                            }
                            else if (admConfig.ASC_DefaultSMS_Flag == "F" && studDet.FirstOrDefault().AMST_FatherMobleNo != null)
                            {
                                try
                                {
                                    SMS sms = new SMS(_db);
                                    string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "Student_Absent_SMS", absentlist[k].Amst_Id).Result;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    _acdimpl.LogInformation("Sendsmsabsent attendance entry mobile F" + ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (studDet.FirstOrDefault().AMST_MobileNo.ToString() != null)
                                    {
                                        SMS sms = new SMS(_db);
                                        string s = sms.sendSms(data.MI_Id, studDet.FirstOrDefault().AMST_MobileNo, "Student_Absent_SMS", absentlist[k].Amst_Id).Result;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    _acdimpl.LogInformation("Sendsmsabsent attendance entry mobile S" + ex.Message);
                                }
                            }
                        }

                        // ------------- EMAIL -------------- //

                        if (template.FirstOrDefault().ISES_MailActiveFlag == true)
                        {
                            if (admConfig.ASC_DefaultSMS_Flag == "M" && studDet.FirstOrDefault().AMST_MotherEmailId != null
                                && studDet.FirstOrDefault().AMST_MotherEmailId != "")
                            {
                                try
                                {
                                    Email Email = new Email(_db);
                                    string s = Email.sendmail(data.MI_Id, studDet.FirstOrDefault().AMST_MotherEmailId, "Student_Absent_SMS", absentlist[k].Amst_Id);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    _acdimpl.LogInformation("Sendsmsabsent attendance entry email M" + ex.Message);
                                }
                            }
                            else if (admConfig.ASC_DefaultSMS_Flag == "F" && studDet.FirstOrDefault().AMST_FatheremailId != null
                                && studDet.FirstOrDefault().AMST_FatheremailId != "")
                            {
                                try
                                {
                                    Email Email = new Email(_db);
                                    string s = Email.sendmail(data.MI_Id, studDet.FirstOrDefault().AMST_FatheremailId, "Student_Absent_SMS", absentlist[k].Amst_Id);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    _acdimpl.LogInformation("Sendsmsabsent attendance entry email F " + ex.Message);
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (studDet.FirstOrDefault().AMST_emailId != null && studDet.FirstOrDefault().AMST_emailId != "")
                                    {
                                        Email Email = new Email(_db);
                                        string s = Email.sendmail(data.MI_Id, studDet.FirstOrDefault().AMST_emailId, "Student_Absent_SMS", absentlist[k].Amst_Id);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    _acdimpl.LogInformation("Sendsmsabsent attendance entry email S" + ex.Message);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _acdimpl.LogInformation("Sendsmsabsent attendance entry New" + ex.Message);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogInformation("Sendsmsabsent attendance entry" + ex.Message);
            }
            return data;
        }

        //saving  the student attendance details from smart card attendance to main attendance        
        public StudentAttendanceEntryDTO saveattendancesmartcard(StudentAttendanceEntryDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime2 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var attendanceentrytype = "";
                var attendanceentry = "";
                var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                data.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;

                if (attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "P")
                {
                    attendanceentrytype = "Present";
                    attendanceentry = "P";
                }
                else
                {
                    attendanceentrytype = "Absent";
                    attendanceentry = "A";
                }

                _acdimpl.LogInformation("Enter in smart card attendance savee option for export ");
                Adm_studentAttendance objpge = Mapper.Map<Adm_studentAttendance>(data);

                DateTime today = DateTime.Today;
                List<Studentattsmartcardabsent_class_sectionlist> classid = new List<Studentattsmartcardabsent_class_sectionlist>();
                List<Studentattsmartcardabsent_class_sectionlist> sectionid = new List<Studentattsmartcardabsent_class_sectionlist>();

                string confromdate = "";
                DateTime fromdatecon = Convert.ToDateTime(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd"));
                //confromdate = fromdatecon.ToString();
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                var check_config = _db.Attendance_Students_SmartCard_Timings.Where(a => a.MI_Id == data.MI_Id && a.ASSCT_Activeflag == true).ToArray();

                var check_attendanceentrytype = _db.AttendanceEntryTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id).ToList();


                if (check_attendanceentrytype.FirstOrDefault().ASAET_Att_Type == "D")
                {
                    // ***** For Daily Once *****//

                    if (data.rolename == "Staff")
                    {
                        classid = (from a in _db.Attendance_Students_SmartCard
                                   from b in _db.School_Adm_Y_StudentDMO
                                   from c in _db.Adm_M_Student
                                   where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                                   && a.ASSC_AttendanceDate == today && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                   && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.ASCC_Flag == 1)
                                   select new Studentattsmartcardabsent_class_sectionlist
                                   {
                                       ASMCL_Id = a.ASMCL_Id,
                                   }).Distinct().ToList();

                        //section id
                        sectionid = (from a in _db.Attendance_Students_SmartCard
                                     from b in _db.School_Adm_Y_StudentDMO
                                     from c in _db.Adm_M_Student
                                     where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                                     && a.ASSC_AttendanceDate == today && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                     && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && b.ASMS_Id == data.ASMS_Id && a.ASCC_Flag == 1)
                                     select new Studentattsmartcardabsent_class_sectionlist
                                     {
                                         ASMS_Id = a.ASMS_Id,
                                     }).Distinct().ToList();
                    }
                    else
                    {
                        classid = (from a in _db.Attendance_Students_SmartCard
                                   where (a.MI_Id == data.MI_Id && a.ASSC_AttendanceDate == today && a.ASCC_Flag == 1)
                                   select new Studentattsmartcardabsent_class_sectionlist
                                   {
                                       ASMCL_Id = a.ASMCL_Id,
                                   }).Distinct().ToList();

                        sectionid = (from b in _db.Attendance_Students_SmartCard
                                     where (b.MI_Id == data.MI_Id && b.ASSC_AttendanceDate == today && b.ASCC_Flag == 1)
                                     select new Studentattsmartcardabsent_class_sectionlist
                                     {
                                         ASMS_Id = b.ASMS_Id,
                                     }).Distinct().ToList();
                    }

                    // class id

                    _acdimpl.LogInformation("Enter in class array ");
                    if (classid.Count > 0 && sectionid.Count > 0)
                    {
                        for (int i = 0; i < classid.Count(); i++)
                        {
                            _acdimpl.LogInformation("Enter in class array for loop ");
                            var classid1 = classid[i].ASMCL_Id;

                            for (int j = 0; j < sectionid.Count(); j++)
                            {
                                _acdimpl.LogInformation("Enter in Section array for loop ");
                                var secid = sectionid[j].ASMS_Id;

                                var smartcard = (from a in _db.Attendance_Students_SmartCard
                                                 from b in _db.School_Adm_Y_StudentDMO
                                                 from c in _db.Adm_M_Student
                                                 where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == classid1
                                                 && b.ASMS_Id == secid && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == today
                                                 && a.ASMCL_Id == classid1 && a.ASMS_Id == secid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1
                                                 && b.AMAY_ActiveFlag == 1 && a.ASCC_Flag == 1)
                                                 select new Attendance_Students_SmartCard
                                                 {
                                                     AMST_Id = a.AMST_Id
                                                 }).ToList();

                                var smartcard1 = (from a in _db.Attendance_Students_SmartCard
                                                  where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == today
                                                  && a.ASMCL_Id == classid1 && a.ASMS_Id == secid && a.ASCC_Flag == 1)
                                                  select new Attendance_Students_SmartCard
                                                  {
                                                      HRME_Id = a.HRME_Id,
                                                      ASALU_Id = a.ASALU_Id,
                                                      ASSC_NetworkIP = a.ASSC_NetworkIP,
                                                      ASSC_SystemIP = a.ASSC_SystemIP, 
                                                      ASSC_CreatedBy=a.ASSC_CreatedBy,
                                                      ASSC_UpdatedBy=a.ASSC_UpdatedBy
                                                  }).Distinct().ToList();

                                Adm_studentAttendance obj1 = new Adm_studentAttendance();

                                List<Studentattsmartcardabsent> studentList12 = new List<Studentattsmartcardabsent>();

                                if (smartcard.Count > 0)
                                {
                                    // ***********   Updating  When Smart card attendance entered After Collecting The Attendance ***************** //

                                    var checkattendance = _db.Adm_studentAttendance.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id &&
                                     d.ASMCL_Id == classid1 && d.ASMS_Id == secid && d.ASA_FromDate == data.ASA_FromDate && d.ASA_Activeflag == true).ToList();
                                    if (checkattendance.Count() > 0)
                                    {
                                        var ASA_Id = checkattendance.FirstOrDefault().ASA_Id;

                                        for (int k = 0; k < smartcard.Count(); k++)
                                        {
                                            var checkcount = _db.Adm_studentAttendanceStudents.Single(a => a.ASA_Id == ASA_Id && a.AMST_Id == smartcard[k].AMST_Id);
                                            checkcount.ASA_AttendanceFlag = "Present";
                                            checkcount.ASA_Class_Attended = Convert.ToDecimal("1.00");
                                            checkcount.UpdatedDate = indianTime2;
                                            checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                            _db.Adm_studentAttendanceStudents.Update(checkcount);
                                        }
                                        var result = _db.SaveChanges();
                                        if (result >= 1)
                                        {
                                            data.returnval = true;
                                            data.message = "Record Updated Successfully";
                                        }
                                        else
                                        {
                                            data.returnval = false;
                                            data.message = "Failed To Update Record";
                                        }
                                    }

                                    // ***********   Saving First Time When Smart card attendance entered ***************** //
                                    else
                                    {
                                        _acdimpl.LogInformation("Enter in smart card student detais array  loop ");
                                        obj1.MI_Id = data.MI_Id;
                                        obj1.ASMAY_Id = data.ASMAY_Id;
                                        obj1.ASA_Att_Type = "Dailyonce";
                                        obj1.ASA_Att_EntryType = attendanceentrytype;
                                        obj1.ASMCL_Id = Convert.ToInt64(classid1);
                                        obj1.ASMS_Id = Convert.ToInt64(secid);
                                        obj1.ASA_ClassHeld = Convert.ToDecimal("1.00");
                                        obj1.HRME_Id = smartcard1.FirstOrDefault().HRME_Id;
                                        obj1.ASALU_Id = smartcard1.FirstOrDefault().ASALU_Id;
                                        obj1.ASA_Network_IP = smartcard1.FirstOrDefault().ASSC_SystemIP;
                                        obj1.ASA_Mac_Add = smartcard1.FirstOrDefault().ASSC_NetworkIP;

                                        obj1.IMP_Id = 0;
                                        obj1.ASA_Entry_DateTime = Convert.ToDateTime(confromdate);
                                        obj1.ASA_FromDate = Convert.ToDateTime(confromdate);
                                        obj1.ASA_ToDate = Convert.ToDateTime(confromdate);
                                        obj1.CreatedDate = indianTime2;
                                        obj1.UpdatedDate = indianTime2;                                         
                                        obj1.ASA_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                        obj1.ASA_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                        obj1.ASA_Activeflag = true;
                                        _db.Adm_studentAttendance.Add(obj1);
                                        _acdimpl.LogInformation("obj1 is added to attendance table ");

                                        List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                        for (int k = 0; k < smartcard.Count(); k++)
                                        {
                                            Adm_studentAttendanceStudents obj2 = new Adm_studentAttendanceStudents();
                                            _acdimpl.LogInformation("obj2 for loop enter ");
                                            obj2.AMST_Id = smartcard[k].AMST_Id;
                                            obj2.ASA_Id = obj1.ASA_Id;
                                            obj2.ASA_Class_Attended = Convert.ToDecimal("1.00");
                                            obj2.ASA_AttendanceFlag = "Present";
                                            obj2.CreatedDate = indianTime2;
                                            obj2.UpdatedDate = indianTime2;
                                            obj2.ASAS_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                            obj2.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                            _db.Adm_studentAttendanceStudents.Add(obj2);
                                            _acdimpl.LogInformation("obj2 is added to attendance student table ");
                                        }

                                        //absent list 
                                        //new        
                                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                        {
                                            _acdimpl.LogInformation("smart card stored procedure absent details ");
                                            cmd.CommandText = "Attendence_Student_SmartCard";
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.BigInt) { Value = classid1 });
                                            cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.BigInt) { Value = secid });
                                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                            cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.DateTime) { Value = data.ASA_FromDate });

                                            if (cmd.Connection.State != ConnectionState.Open)
                                                cmd.Connection.Open();
                                            List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                                    retObject.Add(new Studentattsmartcardabsent
                                                    {
                                                        AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                                    });
                                                }
                                            }
                                            _acdimpl.LogInformation("retobject is created  ");
                                            studentList1 = retObject.ToList();
                                        }

                                        for (int k = 0; k < studentList1.Count; k++)
                                        {
                                            Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                            _acdimpl.LogInformation("obj3 for absent details ");
                                            obj3.AMST_Id = studentList1[k].AMST_Id;
                                            obj3.ASA_Id = obj1.ASA_Id;
                                            obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                            obj3.ASA_AttendanceFlag = "Absent";
                                            obj3.CreatedDate = indianTime2;
                                            obj3.UpdatedDate = indianTime2;
                                            obj3.ASAS_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                            obj3.ASAS_UpdatedBy = smartcard.FirstOrDefault().ASSC_UpdatedBy;
                                            _db.Adm_studentAttendanceStudents.Add(obj3);
                                            _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                        }

                                        var result = _db.SaveChanges();
                                        if (result >= 1)
                                        {
                                            data.returnval = true;
                                            data.message = "Record Saved Successfully";
                                        }
                                        else
                                        {
                                            data.returnval = false;
                                            data.message = "Failed To Save Record";
                                        }
                                    }
                                }
                                // ************ If any smart card attendance not entered and click on save ************ //
                                else
                                {
                                    List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                    {
                                        _acdimpl.LogInformation("smart card stored procedure absent details ");
                                        cmd.CommandText = "Attendence_Student_SmartCard";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.BigInt) { Value = classid1 });
                                        cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.BigInt) { Value = secid });
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                        cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.DateTime) { Value = data.ASA_FromDate });

                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();
                                        List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                                retObject.Add(new Studentattsmartcardabsent
                                                {
                                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                                });
                                            }
                                        }
                                        _acdimpl.LogInformation("retobject is created  ");
                                        studentList1 = retObject.ToList();
                                    }

                                    for (int k = 0; k < studentList1.Count; k++)
                                    {
                                        Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                        _acdimpl.LogInformation("obj3 for absent details ");
                                        obj3.AMST_Id = studentList1[k].AMST_Id;
                                        obj3.ASA_Id = obj1.ASA_Id;
                                        obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                        obj3.ASA_AttendanceFlag = "Absent";
                                        obj3.CreatedDate = indianTime2;
                                        obj3.UpdatedDate = indianTime2;
                                        obj3.ASAS_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                        obj3.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                        _db.Adm_studentAttendanceStudents.Add(obj3);
                                        _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                    }

                                    var result = _db.SaveChanges();
                                    if (result >= 1)
                                    {
                                        data.returnval = true;
                                        data.message = "Record Saved Successfully";
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                        data.message = "Failed To Save Record";
                                    }
                                }
                            }
                        }
                    }
                    // ********  When not data in smart card attendance ********** //
                    else
                    {
                        if (data.rolename == "Staff")
                        {
                            classid = (from a in _db.Masterclasscategory
                                       from b in _db.AcademicYear
                                       from c in _db.School_M_Class
                                       where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                       && a.Is_Active == true && a.ASMCL_Id == data.ASMCL_Id)
                                       select new Studentattsmartcardabsent_class_sectionlist
                                       {
                                           ASMCL_Id = c.ASMCL_Id,
                                       }).Distinct().ToList();

                            sectionid = (from a in _db.Masterclasscategory
                                         from d in _db.AdmSchoolMasterClassCatSec
                                         from b in _db.AcademicYear
                                         from c in _db.School_M_Class
                                         from f in _db.Section
                                         where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCC_Id == d.ASMCC_Id && f.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == data.ASMS_Id)
                                         select new Studentattsmartcardabsent_class_sectionlist
                                         {
                                             ASMS_Id = f.ASMS_Id,
                                         }).Distinct().ToList();
                        }
                        else
                        {
                            classid = (from a in _db.Masterclasscategory
                                       from b in _db.AcademicYear
                                       from c in _db.School_M_Class
                                       where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                       && a.Is_Active == true)
                                       select new Studentattsmartcardabsent_class_sectionlist
                                       {
                                           ASMCL_Id = c.ASMCL_Id,
                                       }).Distinct().ToList();

                            sectionid = (from a in _db.Masterclasscategory
                                         from d in _db.AdmSchoolMasterClassCatSec
                                         from b in _db.AcademicYear
                                         from c in _db.School_M_Class
                                         from f in _db.Section
                                         where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCC_Id == d.ASMCC_Id && f.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true && a.ASMCL_Id == data.ASMCL_Id)
                                         select new Studentattsmartcardabsent_class_sectionlist
                                         {
                                             ASMS_Id = f.ASMS_Id,
                                         }).Distinct().ToList();
                        }

                        for (int k = 0; k < classid.Count; k++)
                        {
                            var classid1 = classid[k].ASMCL_Id;

                            for (int j = 0; j < sectionid.Count; j++)
                            {
                                var secid = sectionid[j].ASMS_Id;

                                var smartcard1 = (from a in _db.Attendance_Students_SmartCard
                                                  where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == classid1
                                                  && a.ASMS_Id == secid)
                                                  select new Attendance_Students_SmartCard
                                                  {
                                                      HRME_Id = a.HRME_Id,
                                                      ASALU_Id = a.ASALU_Id,
                                                      ASSC_NetworkIP = a.ASSC_NetworkIP,
                                                      ASSC_SystemIP = a.ASSC_SystemIP,
                                                      ASSC_CreatedBy = a.ASSC_CreatedBy,
                                                      ASSC_UpdatedBy = a.ASSC_UpdatedBy
                                                  }).Distinct().ToList();

                                Adm_studentAttendance obj1 = new Adm_studentAttendance();

                                long hrmeid = 0;
                                long ASALU_Id = 0;
                                long? ASSC_CreatedBy = 0;
                                long? ASSC_UpdatedBy = 0;
                                string ASSC_SystemIP = "";
                                string ASSC_NetworkIP = "";

                                if (smartcard1.Count() > 0)
                                {
                                    hrmeid = smartcard1.FirstOrDefault().HRME_Id;
                                    ASALU_Id = smartcard1.FirstOrDefault().ASALU_Id;
                                    ASSC_SystemIP = smartcard1.FirstOrDefault().ASSC_SystemIP;
                                    ASSC_NetworkIP = smartcard1.FirstOrDefault().ASSC_NetworkIP;
                                    ASSC_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                    ASSC_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                }
                                else
                                {
                                    var getdetails = _db.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userId && a.IVRMSTAUL_ActiveFlag == 1).ToList();
                                    hrmeid = getdetails.FirstOrDefault().Emp_Code;
                                    var getdetailss = _db.Adm_SchAttLoginUser.Where(a => a.MI_Id == data.MI_Id && a.HRME_Id == hrmeid && a.ASMAY_Id == data.ASMAY_Id).ToList();
                                    ASALU_Id = getdetailss.FirstOrDefault().ASALU_Id;
                                    ASSC_SystemIP = "::1";
                                    ASSC_NetworkIP = "::1";
                                }


                                _acdimpl.LogInformation("Enter in smart card student detais array  loop ");
                                obj1.MI_Id = data.MI_Id;
                                obj1.ASMAY_Id = data.ASMAY_Id;
                                obj1.ASA_Att_Type = "Dailyonce";
                                obj1.ASA_Att_EntryType = attendanceentrytype;
                                obj1.ASMCL_Id = classid1;
                                obj1.ASMS_Id = secid;
                                obj1.ASA_ClassHeld = Convert.ToDecimal("1.00");
                                obj1.HRME_Id = hrmeid;
                                obj1.ASALU_Id = ASALU_Id;
                                obj1.ASA_Network_IP = ASSC_SystemIP;
                                obj1.ASA_Mac_Add = ASSC_NetworkIP;
                                obj1.ASA_UpdatedBy = ASSC_UpdatedBy;
                                obj1.ASA_CreatedBy = ASSC_CreatedBy;
                                obj1.IMP_Id = 0;
                                obj1.ASA_Entry_DateTime = Convert.ToDateTime(confromdate);
                                obj1.ASA_FromDate = Convert.ToDateTime(confromdate);
                                obj1.ASA_ToDate = Convert.ToDateTime(confromdate);
                                obj1.CreatedDate = indianTime2;
                                obj1.UpdatedDate = indianTime2;
                                obj1.ASA_Activeflag = true;
                                _db.Adm_studentAttendance.Add(obj1);
                                _acdimpl.LogInformation("obj1 is added to attendance table ");

                                List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                {
                                    _acdimpl.LogInformation("smart card stored procedure absent details ");
                                    cmd.CommandText = "Attendence_Student_SmartCard";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.BigInt) { Value = classid1 });
                                    cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.BigInt) { Value = secid });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.DateTime) { Value = data.ASA_FromDate });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();
                                    List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                            retObject.Add(new Studentattsmartcardabsent
                                            {
                                                AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                            });
                                        }
                                    }
                                    _acdimpl.LogInformation("retobject is created  ");
                                    studentList1 = retObject.ToList();
                                }

                                for (int k1 = 0; k1 < studentList1.Count; k1++)
                                {
                                    Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                    _acdimpl.LogInformation("obj3 for absent details ");
                                    obj3.AMST_Id = studentList1[k1].AMST_Id;
                                    obj3.ASA_Id = obj1.ASA_Id;
                                    obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                    obj3.ASA_AttendanceFlag = "Absent";
                                    obj3.CreatedDate = indianTime2;
                                    obj3.UpdatedDate = indianTime2;
                                    obj3.ASAS_CreatedBy = ASSC_CreatedBy;
                                    obj3.ASAS_UpdatedBy = ASSC_UpdatedBy;
                                    _db.Adm_studentAttendanceStudents.Add(obj3);
                                    _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                }

                                var result = _db.SaveChanges();
                                if (result >= 1)
                                {
                                    data.returnval = true;
                                    data.message = "Record Saved Successfully";
                                }
                                else
                                {
                                    data.returnval = false;
                                    data.message = "Failed To Save Record";
                                }
                            }
                        }
                    }


                }

                else if (check_attendanceentrytype.FirstOrDefault().ASAET_Att_Type == "H")
                {
                    // ***** For Daily Twice **** //

                    if (check_config.Length > 0)
                    {
                        var check_firsthalf = _db.Attendance_Students_SmartCard_Timings.Where(a => a.MI_Id == data.MI_Id && data.ASSC_PunchTime >= a.ASSCT_FH_TimeFrom && data.ASSC_PunchTime <= a.ASSCT_FH_TimeTo).ToList();

                        if (check_firsthalf.Count() > 0)
                        {
                            //*** check the first half attendance taken or not ***//

                            if (data.rolename == "Staff")
                            {
                                classid = (from a in _db.Attendance_Students_SmartCard
                                           from b in _db.School_Adm_Y_StudentDMO
                                           from c in _db.Adm_M_Student
                                           where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                                           && a.ASSC_AttendanceDate == today && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                           && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.ASCC_Flag == 1 && a.ASSC_PunchTime >= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeFrom && a.ASSC_PunchTime <= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeTo)
                                           select new Studentattsmartcardabsent_class_sectionlist
                                           {
                                               ASMCL_Id = a.ASMCL_Id,
                                           }).Distinct().ToList();

                                //section id
                                sectionid = (from a in _db.Attendance_Students_SmartCard
                                             from b in _db.School_Adm_Y_StudentDMO
                                             from c in _db.Adm_M_Student
                                             where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                                             && a.ASSC_AttendanceDate == today && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                             && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && b.ASMS_Id == data.ASMS_Id && a.ASCC_Flag == 1 && a.ASSC_PunchTime >= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeFrom && a.ASSC_PunchTime <= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeTo)
                                             select new Studentattsmartcardabsent_class_sectionlist
                                             {
                                                 ASMS_Id = a.ASMS_Id,
                                             }).Distinct().ToList();
                            }
                            else
                            {
                                classid = (from a in _db.Attendance_Students_SmartCard
                                           where (a.MI_Id == data.MI_Id && a.ASSC_AttendanceDate == today && a.ASCC_Flag == 1 && a.ASSC_PunchTime >= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeFrom && a.ASSC_PunchTime <= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeTo)
                                           select new Studentattsmartcardabsent_class_sectionlist
                                           {
                                               ASMCL_Id = a.ASMCL_Id,
                                           }).Distinct().ToList();

                                sectionid = (from b in _db.Attendance_Students_SmartCard
                                             where (b.MI_Id == data.MI_Id && b.ASSC_AttendanceDate == today && b.ASCC_Flag == 1 && b.ASSC_PunchTime >= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeFrom && b.ASSC_PunchTime <= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeTo)
                                             select new Studentattsmartcardabsent_class_sectionlist
                                             {
                                                 ASMS_Id = b.ASMS_Id,
                                             }).Distinct().ToList();
                            }

                            if (classid.Count > 0 && sectionid.Count > 0)
                            {
                                for (int i = 0; i < classid.Count; i++)
                                {
                                    _acdimpl.LogInformation("FH Enter in class array for loop ");
                                    var classid1 = classid[i].ASMCL_Id;

                                    for (int j = 0; j < sectionid.Count(); j++)
                                    {
                                        _acdimpl.LogInformation("FH Enter in Section array for loop ");
                                        var secid = sectionid[j].ASMS_Id;

                                        var smartcard = (from a in _db.Attendance_Students_SmartCard
                                                         from b in _db.School_Adm_Y_StudentDMO
                                                         from c in _db.Adm_M_Student
                                                         where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == classid1
                                                         && b.ASMS_Id == secid && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == today
                                                         && a.ASMCL_Id == classid1 && a.ASMS_Id == secid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1
                                                         && b.AMAY_ActiveFlag == 1 && a.ASCC_Flag == 1 && a.ASSC_PunchTime >= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeFrom && a.ASSC_PunchTime <= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeTo)
                                                         select new Attendance_Students_SmartCard
                                                         {
                                                             AMST_Id = a.AMST_Id
                                                         }).Distinct().ToList();

                                        var smartcard1 = (from a in _db.Attendance_Students_SmartCard
                                                          where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == today
                                                          && a.ASMCL_Id == classid1 && a.ASMS_Id == secid && a.ASCC_Flag == 1 && a.ASSC_PunchTime >= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeFrom && a.ASSC_PunchTime <= check_firsthalf.FirstOrDefault().ASSCT_FH_TimeTo)
                                                          select new Attendance_Students_SmartCard
                                                          {
                                                              HRME_Id = a.HRME_Id,
                                                              ASALU_Id = a.ASALU_Id,
                                                              ASSC_NetworkIP = a.ASSC_NetworkIP,
                                                              ASSC_SystemIP = a.ASSC_SystemIP,
                                                              ASSC_CreatedBy = a.ASSC_CreatedBy,
                                                              ASSC_UpdatedBy = a.ASSC_UpdatedBy
                                                          }).Distinct().ToList();

                                        Adm_studentAttendance obj1 = new Adm_studentAttendance();

                                        List<Studentattsmartcardabsent> studentList12 = new List<Studentattsmartcardabsent>();

                                        if (smartcard.Count > 0)
                                        {
                                            // ***********   Updating  When Smart card attendance entered After Collecting The Attendance ***************** //

                                            var checkattendance = _db.Adm_studentAttendance.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id &&
                                             d.ASMCL_Id == classid1 && d.ASMS_Id == secid && d.ASA_FromDate == data.ASA_FromDate && d.ASA_Activeflag == true).ToList();

                                            if (checkattendance.Count() > 0)
                                            {
                                                var ASA_Id = checkattendance.FirstOrDefault().ASA_Id;

                                                for (int k = 0; k < smartcard.Count(); k++)
                                                {
                                                    var checkcount = _db.Adm_studentAttendanceStudents.Single(a => a.ASA_Id == ASA_Id && a.AMST_Id == smartcard[k].AMST_Id);
                                                    if (checkcount.ASA_Class_Attended == Convert.ToDecimal("0.50") && checkcount.ASA_Dailytwice_Flag == "Secondhalf")
                                                    {
                                                        if (attendanceentry == "P")
                                                        {
                                                            checkcount.ASA_AttendanceFlag = "Present";
                                                            checkcount.ASA_Dailytwice_Flag = "Present";
                                                            checkcount.ASA_Class_Attended = Convert.ToDecimal("1.0");
                                                            checkcount.UpdatedDate = indianTime2;
                                                            checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                        }
                                                        else
                                                        {
                                                            checkcount.ASA_AttendanceFlag = "Present";
                                                            checkcount.ASA_Dailytwice_Flag = "Present";
                                                            checkcount.ASA_Class_Attended = Convert.ToDecimal("1.0");
                                                            checkcount.UpdatedDate = indianTime2;
                                                            checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                        }
                                                    }
                                                    else if (checkcount.ASA_Class_Attended == Convert.ToDecimal("1.00") && checkcount.ASA_Dailytwice_Flag == "Present")
                                                    {
                                                        if (attendanceentry == "P")
                                                        {
                                                            checkcount.ASA_AttendanceFlag = "Present";
                                                            checkcount.ASA_Dailytwice_Flag = "Present";
                                                            checkcount.ASA_Class_Attended = Convert.ToDecimal("1.0");
                                                            checkcount.UpdatedDate = indianTime2;
                                                            checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                        }
                                                        else
                                                        {
                                                            checkcount.ASA_AttendanceFlag = "Present";
                                                            checkcount.ASA_Dailytwice_Flag = "Present";
                                                            checkcount.ASA_Class_Attended = Convert.ToDecimal("1.0");
                                                            checkcount.UpdatedDate = indianTime2;
                                                            checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (attendanceentry == "P")
                                                        {
                                                            checkcount.ASA_AttendanceFlag = "Present";
                                                            checkcount.ASA_Dailytwice_Flag = "firsthalf";
                                                            checkcount.ASA_Class_Attended = Convert.ToDecimal("0.5");
                                                            checkcount.UpdatedDate = indianTime2;
                                                            checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                        }
                                                        else
                                                        {
                                                            checkcount.ASA_AttendanceFlag = "Present";
                                                            checkcount.ASA_Dailytwice_Flag = "firsthalf";
                                                            checkcount.ASA_Class_Attended = Convert.ToDecimal("0.5");
                                                            checkcount.UpdatedDate = indianTime2;
                                                            checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                        }
                                                    }

                                                    _db.Adm_studentAttendanceStudents.Update(checkcount);
                                                }
                                                var result = _db.SaveChanges();
                                                if (result >= 1)
                                                {
                                                    data.returnval = true;
                                                    data.message = "Record Updated Successfully";
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                    data.message = "Failed To Update Record";
                                                }
                                            }

                                            // ***********   Saving First Time When Smart card attendance entered ***************** //
                                            else
                                            {
                                                _acdimpl.LogInformation("Enter in smart card student detais array  loop ");
                                                obj1.MI_Id = data.MI_Id;
                                                obj1.ASMAY_Id = data.ASMAY_Id;
                                                obj1.ASA_Att_Type = "Dailytwice";
                                                obj1.ASA_Att_EntryType = attendanceentrytype;
                                                obj1.ASMCL_Id = Convert.ToInt64(classid1);
                                                obj1.ASMS_Id = Convert.ToInt64(secid);
                                                obj1.ASA_ClassHeld = Convert.ToDecimal("1.00");
                                                obj1.HRME_Id = smartcard1.FirstOrDefault().HRME_Id;
                                                obj1.ASALU_Id = smartcard1.FirstOrDefault().ASALU_Id;
                                                obj1.ASA_Network_IP = smartcard1.FirstOrDefault().ASSC_SystemIP;
                                                obj1.ASA_Mac_Add = smartcard1.FirstOrDefault().ASSC_NetworkIP;
                                                obj1.ASA_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                obj1.ASA_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;

                                                obj1.IMP_Id = 0;
                                                obj1.ASA_Entry_DateTime = Convert.ToDateTime(confromdate);
                                                obj1.ASA_FromDate = Convert.ToDateTime(confromdate);
                                                obj1.ASA_ToDate = Convert.ToDateTime(confromdate);
                                                obj1.CreatedDate = indianTime2;
                                                obj1.UpdatedDate = indianTime2;
                                                obj1.ASA_Activeflag = true;
                                                _db.Adm_studentAttendance.Add(obj1);
                                                _acdimpl.LogInformation("obj1 is added to attendance table ");

                                                List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                                for (int k = 0; k < smartcard.Count(); k++)
                                                {
                                                    Adm_studentAttendanceStudents obj2 = new Adm_studentAttendanceStudents();
                                                    _acdimpl.LogInformation("obj2 for loop enter ");
                                                    obj2.AMST_Id = smartcard[k].AMST_Id;
                                                    obj2.ASA_Id = obj1.ASA_Id;
                                                    if (attendanceentry == "P")
                                                    {
                                                        obj2.ASA_AttendanceFlag = "Present";
                                                        obj2.ASA_Dailytwice_Flag = "firsthalf";
                                                        obj2.ASA_Class_Attended = Convert.ToDecimal("0.50");
                                                    }
                                                    else
                                                    {
                                                        obj2.ASA_AttendanceFlag = "Present";
                                                        obj2.ASA_Dailytwice_Flag = "firsthalf";
                                                        obj2.ASA_Class_Attended = Convert.ToDecimal("0.50");
                                                    }
                                                    obj2.CreatedDate = indianTime2;
                                                    obj2.UpdatedDate = indianTime2;
                                                    obj2.ASAS_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                                    obj2.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                    _db.Adm_studentAttendanceStudents.Add(obj2);
                                                    _acdimpl.LogInformation("obj2 is added to attendance student table ");
                                                }

                                                //absent list 
                                                //new        
                                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                {
                                                    _acdimpl.LogInformation("smart card stored procedure absent details ");
                                                    cmd.CommandText = "Attendence_Student_SmartCard_Timings";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.VarChar) { Value = classid1 });
                                                    cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.VarChar) { Value = secid });
                                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                                    cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.VarChar) { Value = confromdate });
                                                    cmd.Parameters.Add(new SqlParameter("@flagFHSH", SqlDbType.VarChar) { Value = "FH" });

                                                    if (cmd.Connection.State != ConnectionState.Open)
                                                        cmd.Connection.Open();
                                                    List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                                            retObject.Add(new Studentattsmartcardabsent
                                                            {
                                                                AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                                            });
                                                        }
                                                    }
                                                    _acdimpl.LogInformation("retobject is created  ");
                                                    studentList1 = retObject.ToList();
                                                }

                                                for (int k = 0; k < studentList1.Count; k++)
                                                {
                                                    Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                                    _acdimpl.LogInformation("obj3 for absent details ");
                                                    obj3.AMST_Id = studentList1[k].AMST_Id;
                                                    obj3.ASA_Id = obj1.ASA_Id;
                                                    obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                                    if (attendanceentry == "P")
                                                    {
                                                        obj3.ASA_Dailytwice_Flag = "Absent";
                                                    }
                                                    else
                                                    {
                                                        obj3.ASA_Dailytwice_Flag = "Absent";
                                                    }

                                                    obj3.ASA_AttendanceFlag = "Absent";
                                                    obj3.CreatedDate = indianTime2;
                                                    obj3.UpdatedDate = indianTime2;
                                                    obj3.ASAS_CreatedBy =  smartcard1.FirstOrDefault().ASSC_CreatedBy ;
                                                    obj3.ASAS_UpdatedBy =  smartcard1.FirstOrDefault().ASSC_UpdatedBy ;
                                                    _db.Adm_studentAttendanceStudents.Add(obj3);
                                                    _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                                }

                                                var result = _db.SaveChanges();
                                                if (result >= 1)
                                                {
                                                    data.returnval = true;
                                                    data.message = "Record Saved Successfully";
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                    data.message = "Failed To Save Record";
                                                }
                                            }
                                        }
                                        // ************ If any smart card attendance not entered and click on save ************ //
                                        else
                                        {
                                            List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                            {
                                                _acdimpl.LogInformation("smart card stored procedure absent details ");
                                                cmd.CommandText = "Attendence_Student_SmartCard_Timings";
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.VarChar) { Value = classid1 });
                                                cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.VarChar) { Value = secid });
                                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                                cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.VarChar) { Value = confromdate });
                                                cmd.Parameters.Add(new SqlParameter("@flagFHSH", SqlDbType.VarChar) { Value = "FH" });

                                                if (cmd.Connection.State != ConnectionState.Open)
                                                    cmd.Connection.Open();
                                                List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                                        retObject.Add(new Studentattsmartcardabsent
                                                        {
                                                            AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                                        });
                                                    }
                                                }
                                                _acdimpl.LogInformation("retobject is created  ");
                                                studentList1 = retObject.ToList();
                                            }

                                            for (int k = 0; k < studentList1.Count; k++)
                                            {
                                                Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                                _acdimpl.LogInformation("obj3 for absent details ");
                                                obj3.AMST_Id = studentList1[k].AMST_Id;
                                                obj3.ASA_Id = obj1.ASA_Id;
                                                obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                                if (attendanceentry == "P")
                                                {
                                                    obj3.ASA_Dailytwice_Flag = "firsthalf";
                                                }
                                                else
                                                {
                                                    obj3.ASA_Dailytwice_Flag = "firsthalf";
                                                }
                                                obj3.ASA_AttendanceFlag = "Absent";
                                                obj3.CreatedDate = indianTime2;
                                                obj3.UpdatedDate = indianTime2;
                                                obj3.ASAS_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                                obj3.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                _db.Adm_studentAttendanceStudents.Add(obj3);
                                                _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                            }

                                            var result = _db.SaveChanges();
                                            if (result >= 1)
                                            {
                                                data.returnval = true;
                                                data.message = "Record Saved Successfully";
                                            }
                                            else
                                            {
                                                data.returnval = false;
                                                data.message = "Failed To Save Record";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {

                            }
                        }

                        else
                        {
                            var check_secondhalf = _db.Attendance_Students_SmartCard_Timings.Where(a => a.MI_Id == data.MI_Id && data.ASSC_PunchTime >= a.ASSCT_SH_TimeFrom && data.ASSC_PunchTime <= a.ASSCT_SH_TimeTo).ToList();

                            if (check_secondhalf.Count() > 0)
                            {
                                if (data.rolename == "Staff")
                                {
                                    classid = (from a in _db.Attendance_Students_SmartCard
                                               from b in _db.School_Adm_Y_StudentDMO
                                               from c in _db.Adm_M_Student
                                               where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                                               && a.ASSC_AttendanceDate == today && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                               && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.ASCC_Flag == 1 && a.ASSC_PunchTime >= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeFrom && a.ASSC_PunchTime <= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeTo)
                                               select new Studentattsmartcardabsent_class_sectionlist
                                               {
                                                   ASMCL_Id = a.ASMCL_Id,
                                               }).Distinct().ToList();

                                    //section id
                                    sectionid = (from a in _db.Attendance_Students_SmartCard
                                                 from b in _db.School_Adm_Y_StudentDMO
                                                 from c in _db.Adm_M_Student
                                                 where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                                                 && a.ASSC_AttendanceDate == today && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                                 && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && b.ASMS_Id == data.ASMS_Id && a.ASCC_Flag == 1 && a.ASSC_PunchTime >= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeFrom && a.ASSC_PunchTime <= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeTo)
                                                 select new Studentattsmartcardabsent_class_sectionlist
                                                 {
                                                     ASMS_Id = a.ASMS_Id,
                                                 }).Distinct().ToList();
                                }
                                else
                                {
                                    classid = (from a in _db.Attendance_Students_SmartCard
                                               where (a.MI_Id == data.MI_Id && a.ASSC_AttendanceDate == today && a.ASCC_Flag == 1 && a.ASSC_PunchTime >= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeFrom && a.ASSC_PunchTime <= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeTo)
                                               select new Studentattsmartcardabsent_class_sectionlist
                                               {
                                                   ASMCL_Id = a.ASMCL_Id,
                                               }).Distinct().ToList();

                                    sectionid = (from b in _db.Attendance_Students_SmartCard
                                                 where (b.MI_Id == data.MI_Id && b.ASSC_AttendanceDate == today && b.ASCC_Flag == 1 && b.ASSC_PunchTime >= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeFrom && b.ASSC_PunchTime <= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeTo)
                                                 select new Studentattsmartcardabsent_class_sectionlist
                                                 {
                                                     ASMS_Id = b.ASMS_Id,
                                                 }).Distinct().ToList();
                                }

                                if (classid.Count > 0 && sectionid.Count > 0)
                                {
                                    for (int i = 0; i < classid.Count; i++)
                                    {
                                        _acdimpl.LogInformation("FH Enter in class array for loop ");
                                        var classid1 = classid[i].ASMCL_Id;

                                        for (int j = 0; j < sectionid.Count(); j++)
                                        {
                                            _acdimpl.LogInformation("FH Enter in Section array for loop ");
                                            var secid = sectionid[j].ASMS_Id;

                                            var smartcard = (from a in _db.Attendance_Students_SmartCard
                                                             from b in _db.School_Adm_Y_StudentDMO
                                                             from c in _db.Adm_M_Student
                                                             where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == classid1
                                                             && b.ASMS_Id == secid && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == today
                                                             && a.ASMCL_Id == classid1 && a.ASMS_Id == secid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1
                                                             && b.AMAY_ActiveFlag == 1 && a.ASCC_Flag == 1 && a.ASSC_PunchTime >= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeFrom && a.ASSC_PunchTime <= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeTo)
                                                             select new Attendance_Students_SmartCard
                                                             {
                                                                 AMST_Id = a.AMST_Id
                                                             }).Distinct().ToList();

                                            var smartcard1 = (from a in _db.Attendance_Students_SmartCard
                                                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == today
                                                              && a.ASMCL_Id == classid1 && a.ASMS_Id == secid && a.ASCC_Flag == 1 && a.ASSC_PunchTime >= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeFrom && a.ASSC_PunchTime <= check_secondhalf.FirstOrDefault().ASSCT_SH_TimeTo)
                                                              select new Attendance_Students_SmartCard
                                                              {
                                                                  HRME_Id = a.HRME_Id,
                                                                  ASALU_Id = a.ASALU_Id,
                                                                  ASSC_NetworkIP = a.ASSC_NetworkIP,
                                                                  ASSC_SystemIP = a.ASSC_SystemIP,
                                                                  ASSC_CreatedBy = a.ASSC_CreatedBy,
                                                                  ASSC_UpdatedBy = a.ASSC_UpdatedBy
                                                              }).Distinct().ToList();

                                            Adm_studentAttendance obj1 = new Adm_studentAttendance();

                                            List<Studentattsmartcardabsent> studentList12 = new List<Studentattsmartcardabsent>();

                                            if (smartcard.Count > 0)
                                            {
                                                // ***********   Updating  When Smart card attendance entered After Collecting The Attendance ***************** //

                                                var checkattendance = _db.Adm_studentAttendance.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id &&
                                                 d.ASMCL_Id == classid1 && d.ASMS_Id == secid && d.ASA_FromDate == data.ASA_FromDate && d.ASA_Activeflag == true).ToList();

                                                if (checkattendance.Count() > 0)
                                                {
                                                    var ASA_Id = checkattendance.FirstOrDefault().ASA_Id;

                                                    for (int k = 0; k < smartcard.Count(); k++)
                                                    {
                                                        var checkcount = _db.Adm_studentAttendanceStudents.Single(a => a.ASA_Id == ASA_Id
                                                        && a.AMST_Id == smartcard[k].AMST_Id);

                                                        if (checkcount.ASA_Class_Attended == Convert.ToDecimal("0.50") && checkcount.ASA_Dailytwice_Flag == "firsthalf")
                                                        {
                                                            if (attendanceentry == "P")
                                                            {
                                                                checkcount.ASA_AttendanceFlag = "Present";
                                                                checkcount.ASA_Dailytwice_Flag = "Present";
                                                                checkcount.ASA_Class_Attended = Convert.ToDecimal("1.0");
                                                                checkcount.UpdatedDate = indianTime2;
                                                                checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                            }
                                                            else
                                                            {
                                                                checkcount.ASA_AttendanceFlag = "Present";
                                                                checkcount.ASA_Dailytwice_Flag = "Present";
                                                                checkcount.ASA_Class_Attended = Convert.ToDecimal("1.0");
                                                                checkcount.UpdatedDate = indianTime2;
                                                                checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                            }
                                                        }
                                                        else if (checkcount.ASA_Class_Attended == Convert.ToDecimal("1.00") && checkcount.ASA_Dailytwice_Flag == "Present")
                                                        {
                                                            if (attendanceentry == "P")
                                                            {
                                                                checkcount.ASA_AttendanceFlag = "Present";
                                                                checkcount.ASA_Dailytwice_Flag = "Present";
                                                                checkcount.ASA_Class_Attended = Convert.ToDecimal("1.0");
                                                                checkcount.UpdatedDate = indianTime2;
                                                                checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                            }
                                                            else
                                                            {
                                                                checkcount.ASA_AttendanceFlag = "Present";
                                                                checkcount.ASA_Dailytwice_Flag = "Present";
                                                                checkcount.ASA_Class_Attended = Convert.ToDecimal("1.0");
                                                                checkcount.UpdatedDate = indianTime2;
                                                                checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (attendanceentry == "P")
                                                            {
                                                                checkcount.ASA_AttendanceFlag = "Present";
                                                                checkcount.ASA_Dailytwice_Flag = "Secondhalf";
                                                                checkcount.ASA_Class_Attended = Convert.ToDecimal("0.50");
                                                                checkcount.UpdatedDate = indianTime2;
                                                                checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                            }
                                                            else
                                                            {
                                                                checkcount.ASA_AttendanceFlag = "Present";
                                                                checkcount.ASA_Dailytwice_Flag = "Secondhalf";
                                                                checkcount.ASA_Class_Attended = Convert.ToDecimal("0.50");
                                                                checkcount.UpdatedDate = indianTime2;
                                                                checkcount.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                            }
                                                        }

                                                        _db.Adm_studentAttendanceStudents.Update(checkcount);
                                                    }

                                                    var result = _db.SaveChanges();
                                                    if (result >= 1)
                                                    {
                                                        data.returnval = true;
                                                        data.message = "Record Updated Successfully";
                                                    }
                                                    else
                                                    {
                                                        data.returnval = false;
                                                        data.message = "Failed To Update Record";
                                                    }
                                                }

                                                // ***********   Saving First Time When Smart card attendance entered ***************** //
                                                else
                                                {
                                                    _acdimpl.LogInformation("Enter in smart card student detais array  loop ");
                                                    obj1.MI_Id = data.MI_Id;
                                                    obj1.ASMAY_Id = data.ASMAY_Id;
                                                    obj1.ASA_Att_Type = "Dailytwice";
                                                    obj1.ASA_Att_EntryType = attendanceentrytype;
                                                    obj1.ASMCL_Id = Convert.ToInt64(classid1);
                                                    obj1.ASMS_Id = Convert.ToInt64(secid);
                                                    obj1.ASA_ClassHeld = Convert.ToDecimal("1.00");
                                                    obj1.HRME_Id = smartcard1.FirstOrDefault().HRME_Id;
                                                    obj1.ASALU_Id = smartcard1.FirstOrDefault().ASALU_Id;
                                                    obj1.ASA_Network_IP = smartcard1.FirstOrDefault().ASSC_SystemIP;
                                                    obj1.ASA_Mac_Add = smartcard1.FirstOrDefault().ASSC_NetworkIP;
                                                    obj1.ASA_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                                    obj1.ASA_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;

                                                    obj1.IMP_Id = 0;
                                                    obj1.ASA_Entry_DateTime = Convert.ToDateTime(confromdate);
                                                    obj1.ASA_FromDate = Convert.ToDateTime(confromdate);
                                                    obj1.ASA_ToDate = Convert.ToDateTime(confromdate);
                                                    obj1.CreatedDate = indianTime2;
                                                    obj1.UpdatedDate = indianTime2;
                                                    obj1.ASA_Activeflag = true;
                                                    _db.Adm_studentAttendance.Add(obj1);
                                                    _acdimpl.LogInformation("obj1 is added to attendance table ");

                                                    List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                                    for (int k = 0; k < smartcard.Count(); k++)
                                                    {
                                                        Adm_studentAttendanceStudents obj2 = new Adm_studentAttendanceStudents();
                                                        _acdimpl.LogInformation("obj2 for loop enter ");
                                                        obj2.AMST_Id = smartcard[k].AMST_Id;
                                                        obj2.ASA_Id = obj1.ASA_Id;
                                                        if (attendanceentry == "P")
                                                        {
                                                            obj2.ASA_AttendanceFlag = "Present";
                                                            obj2.ASA_Dailytwice_Flag = "Secondhalf";
                                                            obj2.ASA_Class_Attended = Convert.ToDecimal("0.50");
                                                        }
                                                        else
                                                        {
                                                            obj2.ASA_AttendanceFlag = "Present";
                                                            obj2.ASA_Dailytwice_Flag = "Secondhalf";
                                                            obj2.ASA_Class_Attended = Convert.ToDecimal("0.50");
                                                        }
                                                        obj2.CreatedDate = indianTime2;
                                                        obj2.UpdatedDate = indianTime2;
                                                        obj2.ASAS_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                                        obj2.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                        _db.Adm_studentAttendanceStudents.Add(obj2);
                                                        _acdimpl.LogInformation("obj2 is added to attendance student table ");
                                                    }

                                                    //absent list 
                                                    //new        
                                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        _acdimpl.LogInformation("smart card stored procedure absent details ");
                                                        cmd.CommandText = "Attendence_Student_SmartCard_Timings";
                                                        cmd.CommandType = CommandType.StoredProcedure;
                                                        cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.VarChar) { Value = classid1 });
                                                        cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.VarChar) { Value = secid });
                                                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                                        cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.VarChar) { Value = confromdate });
                                                        cmd.Parameters.Add(new SqlParameter("@flagFHSH", SqlDbType.VarChar) { Value = "SH" });

                                                        if (cmd.Connection.State != ConnectionState.Open)
                                                            cmd.Connection.Open();
                                                        List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                                                retObject.Add(new Studentattsmartcardabsent
                                                                {
                                                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                                                });
                                                            }
                                                        }
                                                        _acdimpl.LogInformation("retobject is created  ");
                                                        studentList1 = retObject.ToList();
                                                    }

                                                    for (int k = 0; k < studentList1.Count; k++)
                                                    {
                                                        Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                                        _acdimpl.LogInformation("obj3 for absent details ");
                                                        obj3.AMST_Id = studentList1[k].AMST_Id;
                                                        obj3.ASA_Id = obj1.ASA_Id;
                                                        obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                                        if (attendanceentry == "P")
                                                        {
                                                            obj3.ASA_Dailytwice_Flag = "Absent";
                                                        }
                                                        else
                                                        {
                                                            obj3.ASA_Dailytwice_Flag = "Absent";
                                                        }

                                                        obj3.ASA_AttendanceFlag = "Absent";
                                                        obj3.CreatedDate = indianTime2;
                                                        obj3.UpdatedDate = indianTime2;
                                                        obj3.ASAS_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                                        obj3.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                        _db.Adm_studentAttendanceStudents.Add(obj3);
                                                        _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                                    }

                                                    var result = _db.SaveChanges();
                                                    if (result >= 1)
                                                    {
                                                        data.returnval = true;
                                                        data.message = "Record Saved Successfully";
                                                    }
                                                    else
                                                    {
                                                        data.returnval = false;
                                                        data.message = "Failed To Save Record";
                                                    }
                                                }
                                            }
                                            // ************ If any smart card attendance not entered and click on save ************ //
                                            else
                                            {
                                                List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                {
                                                    _acdimpl.LogInformation("smart card stored procedure absent details ");
                                                    cmd.CommandText = "Attendence_Student_SmartCard_Timings";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.VarChar) { Value = classid1 });
                                                    cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.VarChar) { Value = secid });
                                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                                                    cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.VarChar) { Value = confromdate });
                                                    cmd.Parameters.Add(new SqlParameter("@flagFHSH", SqlDbType.VarChar) { Value = "SH" });

                                                    if (cmd.Connection.State != ConnectionState.Open)
                                                        cmd.Connection.Open();
                                                    List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                                            retObject.Add(new Studentattsmartcardabsent
                                                            {
                                                                AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                                            });
                                                        }
                                                    }
                                                    _acdimpl.LogInformation("retobject is created  ");
                                                    studentList1 = retObject.ToList();
                                                }

                                                for (int k = 0; k < studentList1.Count; k++)
                                                {
                                                    Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                                    _acdimpl.LogInformation("obj3 for absent details ");
                                                    obj3.AMST_Id = studentList1[k].AMST_Id;
                                                    obj3.ASA_Id = obj1.ASA_Id;
                                                    obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                                    if (attendanceentry == "P")
                                                    {
                                                        obj3.ASA_Dailytwice_Flag = "firsthalf";
                                                    }
                                                    else
                                                    {
                                                        obj3.ASA_Dailytwice_Flag = "firsthalf";
                                                    }
                                                    obj3.ASA_AttendanceFlag = "Absent";
                                                    obj3.CreatedDate = indianTime2;
                                                    obj3.UpdatedDate = indianTime2;
                                                    obj3.ASAS_CreatedBy = smartcard1.FirstOrDefault().ASSC_CreatedBy;
                                                    obj3.ASAS_UpdatedBy = smartcard1.FirstOrDefault().ASSC_UpdatedBy;
                                                    _db.Adm_studentAttendanceStudents.Add(obj3);
                                                    _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                                }

                                                var result = _db.SaveChanges();
                                                if (result >= 1)
                                                {
                                                    data.returnval = true;
                                                    data.message = "Record Saved Successfully";
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                    data.message = "Failed To Save Record";
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {

                                }
                            }
                            else
                            {

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error in Smartcard Attendance Download From smart card to maintable:'" + ex.Message + "'");
                data.message = "Failed To Save Record";
            }
            return data;
        }       

        //getting the student list for smart card attendance
        public class_section_list getstudentdetailssmart(class_section_list data)
        {
            try
            {
                List<long> GrpId = new List<long>();

                data.Studentattsmartcardabsent = (from a in _db.Attendance_Students_SmartCard
                                                  where a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                                  && a.ASSC_AttendanceDate == data.ASA_FromDate
                                                  select new Studentattsmartcardabsent
                                                  {
                                                      AMST_Id = a.AMST_Id
                                                  }).ToArray();
                data.total_punch = Convert.ToString(data.Studentattsmartcardabsent.Count());

                foreach (var item in data.Studentattsmartcardabsent)
                {
                    GrpId.Add(item.AMST_Id);
                }

                data.studentlist = (from a in _db.School_Adm_Y_StudentDMO
                                    from b in _db.Adm_M_Student
                                    where (a.AMST_Id == b.AMST_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                    && b.MI_Id == data.MI_Id && b.AMST_SOL == "S" && a.AMAY_ActiveFlag == 1 && b.AMST_ActiveFlag == 1 && !GrpId.Contains(a.AMST_Id))
                                    select new class_section_list
                                    {
                                        AMST_Id = b.AMST_Id,
                                        studentname = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName) + ":" + (b.AMST_AdmNo == null ? " " : b.AMST_AdmNo)).Trim()

                                    }).ToArray();

                data.not_punch = Convert.ToString(data.studentlist.Length);


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogInformation("getstudentdetailssmart attendance entry" + ex.Message);
            }
            return data;
        }

        public RFIDDATA GETRFIDDATA(RFIDDATA data)
        {
            try
            {
                using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "RFID_Data";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add(new SqlParameter("@RFID",
                        SqlDbType.NVarChar, 100)
                    {
                        //Value = curdt
                        Value = data.RFID
                    });

                    cmd1.Parameters.Add(new SqlParameter("@RFIDDatetime",
                       SqlDbType.DateTime, 100)
                    {
                        Value = data.RFIDDatetime
                    });
                    cmd1.Parameters.Add(new SqlParameter("@RFIDReaderIP",
                   SqlDbType.NVarChar, 100)
                    {
                        Value = data.RFIDReaderIP
                    });

                    cmd1.Parameters.Add(new SqlParameter("@RFIDAntenna",
                 SqlDbType.NVarChar, 100)
                    {
                        Value = data.RFIDAntenna
                    });

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var RFIDSTA = cmd1.ExecuteNonQuery();

                    if (RFIDSTA == 1)
                    {
                        data.RFIDstatus = "Sucess";
                    }
                    else
                    {
                        data.RFIDstatus = "Failure";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentAttendanceEntryDTO getstdlistperiod1(StudentAttendanceEntryDTO data)
        {
            try
            {
                var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                data.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;

                if (data.asasB_Id == 0)
                {
                    //---Checking Whether Selected Period Attendance Is Done Or Not For That Class , Section , Year, Date And Period---//
                    var checkperiodattentry = (from a in _db.Adm_studentAttendance
                                               from b in _db.Adm_studentAttendanceStudents
                                               from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                               from d in _db.Adm_studentAttendanceSubjects
                                               where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                               && a.ASMS_Id == data.ASMS_Id && c.TTMP_Id == data.TTMP_Id && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate)
                                               select new StudentAttendanceEntryDTO
                                               {
                                                   ASA_Id = c.ASA_Id,
                                                   TTMP_Id = c.TTMP_Id
                                               }).Distinct().ToList();

                    if (checkperiodattentry.Count > 0)
                    {
                        //--------If Period Attendance Happend For That Period Then Checking Period With Subject--------//
                        var checkperiodsubjectatt = (from a in _db.Adm_studentAttendance
                                                     from b in _db.Adm_studentAttendanceStudents
                                                     from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                                     from d in _db.Adm_studentAttendanceSubjects
                                                     where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                                     && d.ISMS_Id == data.ismS_Id && c.TTMP_Id == data.TTMP_Id && a.ASMS_Id == data.ASMS_Id && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate)
                                                     select new StudentAttendanceEntryDTO
                                                     {
                                                         ASA_Id = c.ASA_Id,
                                                         TTMP_Id = c.TTMP_Id,
                                                         ismS_Id = d.ISMS_Id
                                                     }).Distinct().ToList();

                        if (checkperiodsubjectatt.Count > 0)
                        {
                            //--Retrieving The Saved Data For That Date , Class , Section , Subject, Period--//
                            try
                            {
                                data.monthid = 0;
                                List<StudentAttTempDTO> studentList1 = new List<StudentAttTempDTO>();
                                List<StudentAttTempDTO> result = new List<StudentAttTempDTO>();
                                var type = _db.AttendanceEntryTypeDMO.Where(t => t.ASMCL_Id.Equals(data.ASMCL_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id) && t.MI_Id.Equals(data.MI_Id)).ToArray();
                                data.ASA_Att_EntryType = type[0].ASAET_Att_Type;
                                var getregularorextra = _db.Adm_studentAttendance.Where(t => t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASA_Att_Type.Equals(data.monthflag1) && t.ASA_FromDate == data.ASA_FromDate && t.MI_Id == data.MI_Id).ToList();
                                data.ASA_Regular_Extra = getregularorextra.FirstOrDefault().ASA_Regular_Extra;
                                try
                                {
                                    _acdimpl.LogInformation("entered try block");
                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                    {
                                        _acdimpl.LogInformation("entered cmd getdbconnection");
                                        cmd.CommandText = "adm_student_list_not_in_att";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMCL_Id) });
                                        cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMS_Id) });
                                        //cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = Convert.ToString(data.ASA_FromDate) });
                                        cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = Convert.ToString(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd")) });
                                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                        cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                        cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = Convert.ToString(data.monthflag) });
                                        cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(data.monthid) });
                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();
                                        _acdimpl.LogInformation("entered if block");
                                        _acdimpl.LogInformation("Fromdate :'" + data.ASA_FromDate + "");
                                        var retObject = new List<dynamic>();
                                        try
                                        {
                                            using (var dataReader = cmd.ExecuteReader())
                                            {
                                                _acdimpl.LogInformation("entered in dataReader block");
                                                while (dataReader.Read())
                                                {
                                                    _acdimpl.LogInformation("entered in while block");

                                                    result.Add(new StudentAttTempDTO
                                                    {
                                                        amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                        studentname = (dataReader["studentname"]).ToString(),
                                                        amsT_AdmNo = (dataReader["AMST_AdmNo"]).ToString(),
                                                        amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                                    });
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                            Console.Write(ex.Message);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                    Console.Write(ex.Message);
                                }


                                if (data.monthflag == "P")
                                {
                                    studentList1 = (from a in _db.Adm_studentAttendance
                                                    from b in _db.Adm_studentAttendanceStudents
                                                    from c in _db.Adm_studentAttendanceSubjects
                                                    from d in _db.Adm_StudentAttendancePeriodwiseDMO
                                                    from e in _db.Adm_M_Student
                                                    from f in _db.School_Adm_Y_StudentDMO
                                                    where (a.ASA_Id == b.ASA_Id && b.AMST_Id == f.AMST_Id && e.AMST_Id == f.AMST_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id
                                                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && f.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASA_FromDate == data.ASA_FromDate
                                                    && a.ASA_Att_Type == data.monthflag1 && e.AMST_ActiveFlag == 1 && e.AMST_SOL == "S" && f.AMAY_ActiveFlag == 1 && c.ISMS_Id == data.ismS_Id && d.TTMP_Id == data.TTMP_Id)
                                                    select new StudentAttTempDTO
                                                    {
                                                        amsT_Id = e.AMST_Id,
                                                        //studentname = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + (e.AMST_LastName == null ? " " : e.AMST_LastName)).Trim(),
                                                        studentname = ((e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName)).Trim(),
                                                        amsT_AdmNo = e.AMST_AdmNo == null ? "" : e.AMST_AdmNo,
                                                        amaY_RollNo = f.AMAY_RollNo,
                                                        pdays = (b.ASA_Class_Attended),
                                                        ASAS_Id = b.ASAS_Id,
                                                        asA_Id = a.ASA_Id,
                                                    }).Distinct().ToList();

                                }

                                for (int i = 0; i < result.Count; i++)
                                {
                                    studentList1.Add(result[i]);
                                }

                                data.studentList = studentList1.ToArray();
                                data.attcount = 1;
                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                Console.Write(ex.Message);
                            }
                        }
                        else
                        {
                            data.message = "Already Attendance Is Enter For This Period , Class And Section So You Can Not Entre Again For This Period";
                        }
                    }
                    else
                    {
                        List<StudentAttTempDTO> arrAttdto = new List<StudentAttTempDTO>();
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GetStudentDataByAdecmicYearClassSection";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag });
                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        if (dataReader["studentname"] != System.DBNull.Value)
                                        {
                                            Student_name_null = Convert.ToString(dataReader["studentname"]);
                                        }
                                        else
                                        {
                                            Student_name_null = "NOT AVAILABLE";
                                        }


                                        if (dataReader["AMST_AdmNo"] != System.DBNull.Value)
                                        {
                                            AMST_ADM_null = Convert.ToString(dataReader["AMST_AdmNo"]);
                                        }
                                        else
                                        {
                                            AMST_ADM_null = "NOT AVAILABLE";
                                        }

                                        arrAttdto.Add(new StudentAttTempDTO
                                        {
                                            amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                            studentname = Student_name_null,
                                            amsT_AdmNo = AMST_ADM_null,
                                            amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"])
                                        });
                                        data.studentList = arrAttdto.ToArray();
                                    }
                                }
                                data.attcount = 0;
                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                Console.Write(ex.Message);
                            }
                        }

                    }
                }
                //----------Conditions For The Batch Wise Attendance --------------//
                else
                {
                    //---Checking Whether Selected Period Attendance Is Done Or Not For That Class , Section , Year, Date And Period---//
                    var checkperiodattentry = (from a in _db.Adm_studentAttendance
                                               from b in _db.Adm_studentAttendanceStudents
                                               from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                               from d in _db.Adm_studentAttendanceSubjects
                                               where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                               && a.ASMS_Id == data.ASMS_Id && c.TTMP_Id == data.TTMP_Id && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate)
                                               select new StudentAttendanceEntryDTO
                                               {
                                                   ASA_Id = c.ASA_Id,
                                                   TTMP_Id = c.TTMP_Id
                                               }).Distinct().ToList();

                    if (checkperiodattentry.Count > 0)
                    {
                        //--------If Period Attendance Happend For That Period Then Checking Period With Subject--------//
                        var checkperiodsubjectatt = (from a in _db.Adm_studentAttendance
                                                     from b in _db.Adm_studentAttendanceStudents
                                                     from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                                     from d in _db.Adm_studentAttendanceSubjects
                                                     where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                                     && d.ISMS_Id == data.ismS_Id && c.TTMP_Id == data.TTMP_Id && a.ASMS_Id == data.ASMS_Id && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate)
                                                     select new StudentAttendanceEntryDTO
                                                     {
                                                         ASA_Id = c.ASA_Id,
                                                         TTMP_Id = c.TTMP_Id,
                                                         ismS_Id = d.ISMS_Id
                                                     }).Distinct().ToList();

                        if (checkperiodsubjectatt.Count > 0)
                        {
                            //--------If Period Attendance Happend For That Period Then Checking Period With Subject With Respect To Batch--------//
                            var checkperiodsubjectbatchatt = (from a in _db.Adm_studentAttendance
                                                              from b in _db.Adm_studentAttendanceStudents
                                                              from c in _db.Adm_StudentAttendancePeriodwiseDMO
                                                              from d in _db.Adm_studentAttendanceSubjects
                                                              from e in _db.AdmSchoolAttendanceSubjectBatch
                                                              from f in _db.AdmSchoolAttendanceSubjectBatchStudents
                                                              where (a.ASA_Id == b.ASA_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && e.ASASB_Id == f.ASASB_Id && f.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                                              && d.ISMS_Id == data.ismS_Id && c.TTMP_Id == data.TTMP_Id && a.ASMS_Id == data.ASMS_Id && a.ASA_Att_Type == data.monthflag1 && a.ASA_FromDate == data.ASA_FromDate && e.ASASB_Id == data.asasB_Id)
                                                              select new StudentAttendanceEntryDTO
                                                              {
                                                                  ASA_Id = c.ASA_Id,
                                                                  TTMP_Id = c.TTMP_Id,
                                                                  ismS_Id = d.ISMS_Id
                                                              }).Distinct().ToList();
                            if (checkperiodsubjectbatchatt.Count > 0)
                            {
                                //--Retrieving The Saved Data For That Date , Class , Section , Subject, Period--//
                                try
                                {
                                    data.monthid = 0;
                                    List<StudentAttTempDTO> studentList1 = new List<StudentAttTempDTO>();
                                    List<StudentAttTempDTO> result = new List<StudentAttTempDTO>();
                                    var type = _db.AttendanceEntryTypeDMO.Where(t => t.ASMCL_Id.Equals(data.ASMCL_Id) && t.ASMAY_Id.Equals(data.ASMAY_Id) && t.MI_Id.Equals(data.MI_Id)).ToArray();
                                    data.ASA_Att_EntryType = type[0].ASAET_Att_Type;
                                    var getregularorextra = _db.Adm_studentAttendance.Where(t => t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASA_Att_Type.Equals(data.monthflag1) && t.ASA_FromDate == data.ASA_FromDate && t.MI_Id == data.MI_Id).ToList();
                                    data.ASA_Regular_Extra = getregularorextra.FirstOrDefault().ASA_Regular_Extra;
                                    try
                                    {
                                        _acdimpl.LogInformation("entered try block");
                                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                        {
                                            _acdimpl.LogInformation("entered cmd getdbconnection");
                                            cmd.CommandText = "adm_student_list_not_in_att";
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMCL_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMS_Id) });
                                            //cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = Convert.ToString(data.ASA_FromDate) });
                                            cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar) { Value = Convert.ToString(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd")) });
                                            cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar) { Value = Convert.ToString(data.MI_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.VarChar) { Value = Convert.ToString(data.ASMAY_Id) });
                                            cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.VarChar) { Value = Convert.ToString(data.monthflag) });
                                            cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar) { Value = Convert.ToString(data.monthid) });
                                            if (cmd.Connection.State != ConnectionState.Open)
                                                cmd.Connection.Open();
                                            _acdimpl.LogInformation("entered if block");
                                            _acdimpl.LogInformation("Fromdate :'" + data.ASA_FromDate + "");
                                            var retObject = new List<dynamic>();
                                            try
                                            {
                                                using (var dataReader = cmd.ExecuteReader())
                                                {
                                                    _acdimpl.LogInformation("entered in dataReader block");
                                                    while (dataReader.Read())
                                                    {
                                                        _acdimpl.LogInformation("entered in while block");

                                                        result.Add(new StudentAttTempDTO
                                                        {
                                                            amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                                            studentname = (dataReader["studentname"]).ToString(),
                                                            amsT_AdmNo = (dataReader["AMST_AdmNo"]).ToString(),
                                                            amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"]),
                                                        });
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                                Console.Write(ex.Message);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                        Console.Write(ex.Message);
                                    }


                                    if (data.monthflag == "P")
                                    {
                                        studentList1 = (from a in _db.Adm_studentAttendance
                                                        from b in _db.Adm_studentAttendanceStudents
                                                        from c in _db.Adm_studentAttendanceSubjects
                                                        from d in _db.Adm_StudentAttendancePeriodwiseDMO
                                                        from e in _db.Adm_M_Student
                                                        from f in _db.School_Adm_Y_StudentDMO
                                                        from g in _db.AdmSchoolAttendanceSubjectBatch
                                                        from h in _db.AdmSchoolAttendanceSubjectBatchStudents
                                                        where (a.ASA_Id == b.ASA_Id && b.AMST_Id == f.AMST_Id && e.AMST_Id == f.AMST_Id && a.ASA_Id == c.ASA_Id && a.ASA_Id == d.ASA_Id && g.ASASB_Id == h.ASASB_Id && h.AMST_Id == b.AMST_Id
                                                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && f.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASA_FromDate == data.ASA_FromDate
                                                        && a.ASA_Att_Type == data.monthflag1 && e.AMST_ActiveFlag == 1 && e.AMST_SOL == "S" && f.AMAY_ActiveFlag == 1 && c.ISMS_Id == data.ismS_Id && d.TTMP_Id == data.TTMP_Id && g.ASASB_Id == data.asasB_Id)
                                                        select new StudentAttTempDTO
                                                        {
                                                            amsT_Id = e.AMST_Id,
                                                            //studentname = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + (e.AMST_LastName == null ? " " : e.AMST_LastName)).Trim(),
                                                            studentname = ((e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName)).Trim(),
                                                            amsT_AdmNo = e.AMST_AdmNo == null ? "" : e.AMST_AdmNo,
                                                            amaY_RollNo = f.AMAY_RollNo,
                                                            pdays = (b.ASA_Class_Attended),
                                                            ASAS_Id = b.ASAS_Id,
                                                            asA_Id = a.ASA_Id,
                                                        }).Distinct().ToList();

                                    }

                                    for (int i = 0; i < result.Count; i++)
                                    {
                                        studentList1.Add(result[i]);
                                    }

                                    data.studentList = studentList1.ToArray();
                                    data.attcount = 1;
                                }
                                catch (Exception ex)
                                {
                                    _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                    Console.Write(ex.Message);
                                }
                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            data.message = "Already Attendance Is Enter For This Period , Class And Section So You Can Not Entre Again For This Period";
                        }
                    }
                    else
                    {
                        List<StudentAttTempDTO> arrAttdto = new List<StudentAttTempDTO>();
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "GetStudentDataByAdecmicYearClassSection";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@yearid", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.VarChar) { Value = attendance_entrytype.FirstOrDefault().ASC_Att_Default_OrderFlag });
                            cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        if (dataReader["studentname"] != System.DBNull.Value)
                                        {
                                            Student_name_null = Convert.ToString(dataReader["studentname"]);
                                        }
                                        else
                                        {
                                            Student_name_null = "NOT AVAILABLE";
                                        }


                                        if (dataReader["AMST_AdmNo"] != System.DBNull.Value)
                                        {
                                            AMST_ADM_null = Convert.ToString(dataReader["AMST_AdmNo"]);
                                        }
                                        else
                                        {
                                            AMST_ADM_null = "NOT AVAILABLE";
                                        }

                                        arrAttdto.Add(new StudentAttTempDTO
                                        {
                                            amsT_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                            studentname = Student_name_null,
                                            amsT_AdmNo = AMST_ADM_null,
                                            amaY_RollNo = Convert.ToInt64(dataReader["AMAY_RollNo"])
                                        });
                                        data.studentList = arrAttdto.ToArray();
                                    }
                                }
                                data.attcount = 0;
                            }
                            catch (Exception ex)
                            {
                                _acdimpl.LogInformation("error:'" + ex.Message + "'");
                                Console.Write(ex.Message);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error At getstdlistperiod List In Attendance Entry:" + ex.Message);
            }
            return data;
        }
        public StudentAttendanceEntryDTO saveattendancesmartcardworking(StudentAttendanceEntryDTO data)
        {
            try
            {
                var attendanceentrytype = "";
                var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                data.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;

                if (attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "P")
                {
                    attendanceentrytype = "Present";
                }
                else
                {
                    attendanceentrytype = "Absent";
                }

                _acdimpl.LogInformation("Enter in smart card attendance savee option for export ");
                Adm_studentAttendance objpge = Mapper.Map<Adm_studentAttendance>(data);

                DateTime today = DateTime.Today;
                List<Studentattsmartcardabsent_class_sectionlist> classid = new List<Studentattsmartcardabsent_class_sectionlist>();
                List<Studentattsmartcardabsent_class_sectionlist> sectionid = new List<Studentattsmartcardabsent_class_sectionlist>();

                string confromdate = "";
                DateTime fromdatecon = Convert.ToDateTime(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd"));
                //confromdate = fromdatecon.ToString();
                confromdate = fromdatecon.ToString("yyyy-MM-dd");



                if (data.rolename == "Staff")
                {
                    classid = (from a in _db.Attendance_Students_SmartCard
                               from b in _db.School_Adm_Y_StudentDMO
                               from c in _db.Adm_M_Student
                               where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                               && a.ASSC_AttendanceDate == today && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                               && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.ASCC_Flag == 1)
                               select new Studentattsmartcardabsent_class_sectionlist
                               {
                                   ASMCL_Id = a.ASMCL_Id,
                               }).Distinct().ToList();

                    //section id
                    sectionid = (from a in _db.Attendance_Students_SmartCard
                                 from b in _db.School_Adm_Y_StudentDMO
                                 from c in _db.Adm_M_Student
                                 where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                                 && a.ASSC_AttendanceDate == today && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                 && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && b.ASMS_Id == data.ASMS_Id && a.ASCC_Flag == 1)
                                 select new Studentattsmartcardabsent_class_sectionlist
                                 {
                                     ASMS_Id = a.ASMS_Id,
                                 }).Distinct().ToList();
                }
                else
                {
                    classid = (from a in _db.Attendance_Students_SmartCard
                               where (a.MI_Id == data.MI_Id && a.ASSC_AttendanceDate == today && a.ASCC_Flag == 1)
                               select new Studentattsmartcardabsent_class_sectionlist
                               {
                                   ASMCL_Id = a.ASMCL_Id,
                               }).Distinct().ToList();

                    sectionid = (from b in _db.Attendance_Students_SmartCard
                                 where (b.MI_Id == data.MI_Id && b.ASSC_AttendanceDate == today && b.ASCC_Flag == 1)
                                 select new Studentattsmartcardabsent_class_sectionlist
                                 {
                                     ASMS_Id = b.ASMS_Id,
                                 }).Distinct().ToList();
                }

                // class id

                _acdimpl.LogInformation("Enter in class array ");
                if (classid.Count > 0 && sectionid.Count > 0)
                {
                    for (int i = 0; i < classid.Count(); i++)
                    {
                        _acdimpl.LogInformation("Enter in class array for loop ");
                        var classid1 = classid[i].ASMCL_Id;

                        for (int j = 0; j < sectionid.Count(); j++)
                        {
                            _acdimpl.LogInformation("Enter in Section array for loop ");
                            var secid = sectionid[j].ASMS_Id;

                            var smartcard = (from a in _db.Attendance_Students_SmartCard
                                             from b in _db.School_Adm_Y_StudentDMO
                                             from c in _db.Adm_M_Student
                                             where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == classid1 && b.ASMS_Id == secid && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == today
                                             && a.ASMCL_Id == classid1 && a.ASMS_Id == secid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1
                                              && a.ASCC_Flag == 1)
                                             select new Attendance_Students_SmartCard
                                             {
                                                 AMST_Id = a.AMST_Id
                                             }).ToList();

                            var smartcard1 = (from a in _db.Attendance_Students_SmartCard
                                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == today && a.ASMCL_Id == classid1
                                              && a.ASMS_Id == secid && a.ASCC_Flag == 1)
                                              select new Attendance_Students_SmartCard
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  ASALU_Id = a.ASALU_Id,
                                                  ASSC_NetworkIP = a.ASSC_NetworkIP,
                                                  ASSC_SystemIP = a.ASSC_SystemIP
                                              }).Distinct().ToList();

                            Adm_studentAttendance obj1 = new Adm_studentAttendance();

                            List<Studentattsmartcardabsent> studentList12 = new List<Studentattsmartcardabsent>();

                            if (smartcard.Count > 0)
                            {
                                // ***********   Updating  When Smart card attendance entered After Collecting The Attendance ***************** //

                                var checkattendance = _db.Adm_studentAttendance.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id &&
                                 d.ASMCL_Id == classid1 && d.ASMS_Id == secid && d.ASA_FromDate == data.ASA_FromDate && d.ASA_Activeflag == true).ToList();
                                if (checkattendance.Count() > 0)
                                {
                                    var ASA_Id = checkattendance.FirstOrDefault().ASA_Id;

                                    for (int k = 0; k < smartcard.Count(); k++)
                                    {
                                        var checkcount = _db.Adm_studentAttendanceStudents.Single(a => a.ASA_Id == ASA_Id && a.AMST_Id == smartcard[k].AMST_Id);
                                        checkcount.ASA_AttendanceFlag = "Present";
                                        checkcount.ASA_Class_Attended = Convert.ToDecimal("1.00");
                                        checkcount.UpdatedDate = DateTime.Now;
                                        _db.Adm_studentAttendanceStudents.Update(checkcount);
                                    }
                                    var result = _db.SaveChanges();
                                    if (result >= 1)
                                    {
                                        data.returnval = true;
                                        data.message = "Record Updated Successfully";
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                        data.message = "Failed To Update Record";
                                    }
                                }

                                // ***********   Saving First Time When Smart card attendance entered ***************** //
                                else
                                {
                                    _acdimpl.LogInformation("Enter in smart card student detais array  loop ");
                                    obj1.MI_Id = data.MI_Id;
                                    obj1.ASMAY_Id = data.ASMAY_Id;
                                    obj1.ASA_Att_Type = "Dailyonce";
                                    obj1.ASA_Att_EntryType = attendanceentrytype;
                                    obj1.ASMCL_Id = Convert.ToInt64(classid1);
                                    obj1.ASMS_Id = Convert.ToInt64(secid);
                                    obj1.ASA_ClassHeld = Convert.ToDecimal("1.00");
                                    obj1.HRME_Id = smartcard1.FirstOrDefault().HRME_Id;
                                    obj1.ASALU_Id = smartcard1.FirstOrDefault().ASALU_Id;
                                    obj1.ASA_Network_IP = smartcard1.FirstOrDefault().ASSC_SystemIP;
                                    obj1.ASA_Mac_Add = smartcard1.FirstOrDefault().ASSC_NetworkIP;

                                    obj1.IMP_Id = 0;
                                    obj1.ASA_Entry_DateTime = Convert.ToDateTime(confromdate);
                                    obj1.ASA_FromDate = Convert.ToDateTime(confromdate);
                                    obj1.ASA_ToDate = Convert.ToDateTime(confromdate);
                                    obj1.CreatedDate = DateTime.Now;
                                    obj1.UpdatedDate = DateTime.Now;
                                    obj1.ASA_Activeflag = true;
                                    _db.Adm_studentAttendance.Add(obj1);
                                    _acdimpl.LogInformation("obj1 is added to attendance table ");

                                    List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                    for (int k = 0; k < smartcard.Count(); k++)
                                    {
                                        Adm_studentAttendanceStudents obj2 = new Adm_studentAttendanceStudents();
                                        _acdimpl.LogInformation("obj2 for loop enter ");
                                        obj2.AMST_Id = smartcard[k].AMST_Id;
                                        obj2.ASA_Id = obj1.ASA_Id;
                                        obj2.ASA_Class_Attended = Convert.ToDecimal("1.00");
                                        obj2.ASA_AttendanceFlag = "Present";
                                        obj2.CreatedDate = DateTime.Now;
                                        obj2.UpdatedDate = DateTime.Now;
                                        _db.Adm_studentAttendanceStudents.Add(obj2);
                                        _acdimpl.LogInformation("obj2 is added to attendance student table ");
                                    }

                                    //absent list 
                                    //new        
                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                    {
                                        _acdimpl.LogInformation("smart card stored procedure absent details ");
                                        cmd.CommandText = "Attendence_Student_SmartCard";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.BigInt) { Value = classid1 });
                                        cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.BigInt) { Value = secid });
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                        cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.DateTime) { Value = data.ASA_FromDate });

                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();
                                        List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                                retObject.Add(new Studentattsmartcardabsent
                                                {
                                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                                });
                                            }
                                        }
                                        _acdimpl.LogInformation("retobject is created  ");
                                        studentList1 = retObject.ToList();
                                    }

                                    for (int k = 0; k < studentList1.Count; k++)
                                    {
                                        Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                        _acdimpl.LogInformation("obj3 for absent details ");
                                        obj3.AMST_Id = studentList1[k].AMST_Id;
                                        obj3.ASA_Id = obj1.ASA_Id;
                                        obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                        obj3.ASA_AttendanceFlag = "Absent";
                                        obj3.CreatedDate = DateTime.Now;
                                        obj3.UpdatedDate = DateTime.Now;
                                        _db.Adm_studentAttendanceStudents.Add(obj3);
                                        _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                    }

                                    var result = _db.SaveChanges();
                                    if (result >= 1)
                                    {
                                        data.returnval = true;
                                        data.message = "Record Saved Successfully";
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                        data.message = "Failed To Save Record";
                                    }
                                }
                            }
                            // ************ If any smart card attendance not entered and click on save ************ //
                            else
                            {
                                List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                {
                                    _acdimpl.LogInformation("smart card stored procedure absent details ");
                                    cmd.CommandText = "Attendence_Student_SmartCard";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.BigInt) { Value = classid1 });
                                    cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.BigInt) { Value = secid });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.DateTime) { Value = data.ASA_FromDate });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();
                                    List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                            retObject.Add(new Studentattsmartcardabsent
                                            {
                                                AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                            });
                                        }
                                    }
                                    _acdimpl.LogInformation("retobject is created  ");
                                    studentList1 = retObject.ToList();
                                }

                                for (int k = 0; k < studentList1.Count; k++)
                                {
                                    Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                    _acdimpl.LogInformation("obj3 for absent details ");
                                    obj3.AMST_Id = studentList1[k].AMST_Id;
                                    obj3.ASA_Id = obj1.ASA_Id;
                                    obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                    obj3.ASA_AttendanceFlag = "Absent";
                                    obj3.CreatedDate = DateTime.Now;
                                    obj3.UpdatedDate = DateTime.Now;
                                    _db.Adm_studentAttendanceStudents.Add(obj3);
                                    _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                }

                                var result = _db.SaveChanges();
                                if (result >= 1)
                                {
                                    data.returnval = true;
                                    data.message = "Record Saved Successfully";
                                }
                                else
                                {
                                    data.returnval = false;
                                    data.message = "Failed To Save Record";
                                }
                            }
                        }
                    }
                }
                // ********  When not data in smart card attendance ********** //
                else
                {
                    if (data.rolename == "Staff")
                    {
                        classid = (from a in _db.Masterclasscategory
                                   from b in _db.AcademicYear
                                   from c in _db.School_M_Class
                                   where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.Is_Active == true && a.ASMCL_Id == data.ASMCL_Id)
                                   select new Studentattsmartcardabsent_class_sectionlist
                                   {
                                       ASMCL_Id = c.ASMCL_Id,
                                   }).Distinct().ToList();

                        sectionid = (from a in _db.Masterclasscategory
                                     from d in _db.AdmSchoolMasterClassCatSec
                                     from b in _db.AcademicYear
                                     from c in _db.School_M_Class
                                     from f in _db.Section
                                     where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCC_Id == d.ASMCC_Id && f.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == data.ASMS_Id)
                                     select new Studentattsmartcardabsent_class_sectionlist
                                     {
                                         ASMS_Id = f.ASMS_Id,
                                     }).Distinct().ToList();
                    }
                    else
                    {
                        classid = (from a in _db.Masterclasscategory
                                   from b in _db.AcademicYear
                                   from c in _db.School_M_Class
                                   where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.Is_Active == true)
                                   select new Studentattsmartcardabsent_class_sectionlist
                                   {
                                       ASMCL_Id = c.ASMCL_Id,
                                   }).Distinct().ToList();

                        sectionid = (from a in _db.Masterclasscategory
                                     from d in _db.AdmSchoolMasterClassCatSec
                                     from b in _db.AcademicYear
                                     from c in _db.School_M_Class
                                     from f in _db.Section
                                     where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCC_Id == d.ASMCC_Id && f.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true && a.ASMCL_Id == data.ASMCL_Id)
                                     select new Studentattsmartcardabsent_class_sectionlist
                                     {
                                         ASMS_Id = f.ASMS_Id,
                                     }).Distinct().ToList();
                    }

                    for (int k = 0; k < classid.Count; k++)
                    {
                        var classid1 = classid[k].ASMCL_Id;

                        for (int j = 0; j < sectionid.Count; j++)
                        {
                            var secid = sectionid[j].ASMS_Id;

                            var smartcard1 = (from a in _db.Attendance_Students_SmartCard
                                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == classid1
                                              && a.ASMS_Id == secid)
                                              select new Attendance_Students_SmartCard
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  ASALU_Id = a.ASALU_Id,
                                                  ASSC_NetworkIP = a.ASSC_NetworkIP,
                                                  ASSC_SystemIP = a.ASSC_SystemIP
                                              }).Distinct().ToList();

                            Adm_studentAttendance obj1 = new Adm_studentAttendance();

                            _acdimpl.LogInformation("Enter in smart card student detais array  loop ");
                            obj1.MI_Id = data.MI_Id;
                            obj1.ASMAY_Id = data.ASMAY_Id;
                            obj1.ASA_Att_Type = "Dailyonce";
                            obj1.ASA_Att_EntryType = attendanceentrytype;
                            obj1.ASMCL_Id = classid1;
                            obj1.ASMS_Id = secid;
                            obj1.ASA_ClassHeld = Convert.ToDecimal("1.00");
                            obj1.HRME_Id = smartcard1.FirstOrDefault().HRME_Id;
                            obj1.ASALU_Id = smartcard1.FirstOrDefault().ASALU_Id;
                            obj1.ASA_Network_IP = smartcard1.FirstOrDefault().ASSC_SystemIP;
                            obj1.ASA_Mac_Add = smartcard1.FirstOrDefault().ASSC_NetworkIP;
                            obj1.IMP_Id = 0;
                            obj1.ASA_Entry_DateTime = Convert.ToDateTime(confromdate);
                            obj1.ASA_FromDate = Convert.ToDateTime(confromdate);
                            obj1.ASA_ToDate = Convert.ToDateTime(confromdate);
                            obj1.CreatedDate = DateTime.Now;
                            obj1.UpdatedDate = DateTime.Now;
                            obj1.ASA_Activeflag = true;
                            _db.Adm_studentAttendance.Add(obj1);
                            _acdimpl.LogInformation("obj1 is added to attendance table ");

                            List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                _acdimpl.LogInformation("smart card stored procedure absent details ");
                                cmd.CommandText = "Attendence_Student_SmartCard";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.BigInt) { Value = classid1 });
                                cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.BigInt) { Value = secid });
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.DateTime) { Value = data.ASA_FromDate });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                        retObject.Add(new Studentattsmartcardabsent
                                        {
                                            AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                        });
                                    }
                                }
                                _acdimpl.LogInformation("retobject is created  ");
                                studentList1 = retObject.ToList();
                            }

                            for (int k1 = 0; k1 < studentList1.Count; k1++)
                            {
                                Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                _acdimpl.LogInformation("obj3 for absent details ");
                                obj3.AMST_Id = studentList1[k1].AMST_Id;
                                obj3.ASA_Id = obj1.ASA_Id;
                                obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                obj3.ASA_AttendanceFlag = "Absent";
                                obj3.CreatedDate = DateTime.Now;
                                obj3.UpdatedDate = DateTime.Now;
                                _db.Adm_studentAttendanceStudents.Add(obj3);
                                _acdimpl.LogInformation("obj3 is added to attendance student table ");
                            }

                            var result = _db.SaveChanges();
                            if (result >= 1)
                            {
                                data.returnval = true;
                                data.message = "Record Saved Successfully";
                            }
                            else
                            {
                                data.returnval = false;
                                data.message = "Failed To Save Record";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error in Smartcard Attendance Download From smart card to maintable:'" + ex.Message + "'");
                data.message = "Failed To Save Record";
            }
            return data;
        }
        public StudentAttendanceEntryDTO saveattendancesmartcard1(StudentAttendanceEntryDTO data)
        {
            try
            {
                var attendanceentrytype = "";
                var attendance_entrytype = _db.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                data.attendanceentryflag = attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type;

                if (attendance_entrytype.FirstOrDefault().ASC_Att_DefaultEntry_Type == "P")
                {
                    attendanceentrytype = "Present";
                }
                else
                {
                    attendanceentrytype = "Absent";
                }

                _acdimpl.LogInformation("Enter in smart card attendance savee option for export ");
                Adm_studentAttendance objpge = Mapper.Map<Adm_studentAttendance>(data);

                DateTime today = DateTime.Today;
                List<Studentattsmartcardabsent_class_sectionlist> classid = new List<Studentattsmartcardabsent_class_sectionlist>();
                List<Studentattsmartcardabsent_class_sectionlist> sectionid = new List<Studentattsmartcardabsent_class_sectionlist>();

                string confromdate = "";
                DateTime fromdatecon = Convert.ToDateTime(data.ASA_FromDate.Value.Date.ToString("yyyy-MM-dd"));
                //confromdate = fromdatecon.ToString();
                confromdate = fromdatecon.ToString("yyyy-MM-dd");



                if (data.rolename == "Staff")
                {
                    classid = (from a in _db.Attendance_Students_SmartCard
                               from b in _db.School_Adm_Y_StudentDMO
                               from c in _db.Adm_M_Student
                               where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                               && a.ASSC_AttendanceDate == today && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                               && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                               select new Studentattsmartcardabsent_class_sectionlist
                               {
                                   ASMCL_Id = a.ASMCL_Id,
                               }).Distinct().ToList();

                    //section id
                    sectionid = (from a in _db.Attendance_Students_SmartCard
                                 from b in _db.School_Adm_Y_StudentDMO
                                 from c in _db.Adm_M_Student
                                 where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                                 && a.ASSC_AttendanceDate == today && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id
                                 && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id && b.ASMS_Id == data.ASMS_Id)
                                 select new Studentattsmartcardabsent_class_sectionlist
                                 {
                                     ASMS_Id = a.ASMS_Id,
                                 }).Distinct().ToList();
                }
                else
                {
                    classid = (from a in _db.Attendance_Students_SmartCard
                               where (a.MI_Id == data.MI_Id && a.ASSC_AttendanceDate == today)
                               select new Studentattsmartcardabsent_class_sectionlist
                               {
                                   ASMCL_Id = a.ASMCL_Id,
                               }).Distinct().ToList();

                    sectionid = (from b in _db.Attendance_Students_SmartCard
                                 where (b.MI_Id == data.MI_Id && b.ASSC_AttendanceDate == today)
                                 select new Studentattsmartcardabsent_class_sectionlist
                                 {
                                     ASMS_Id = b.ASMS_Id,
                                 }).Distinct().ToList();
                }

                // class id

                _acdimpl.LogInformation("Enter in class array ");
                if (classid.Count > 0 && sectionid.Count > 0)
                {
                    for (int i = 0; i < classid.Count(); i++)
                    {
                        _acdimpl.LogInformation("Enter in class array for loop ");
                        var classid1 = classid[i].ASMCL_Id;

                        for (int j = 0; j < sectionid.Count(); j++)
                        {
                            _acdimpl.LogInformation("Enter in Section array for loop ");
                            var secid = sectionid[j].ASMS_Id;

                            var smartcard = (from a in _db.Attendance_Students_SmartCard
                                             from b in _db.School_Adm_Y_StudentDMO
                                             from c in _db.Adm_M_Student
                                             where (a.AMST_Id == b.AMST_Id && b.AMST_Id == c.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == classid1 && b.ASMS_Id == secid && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == today
                                             && a.ASMCL_Id == classid1 && a.ASMS_Id == secid && c.AMST_SOL == "S" && c.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                             select new Attendance_Students_SmartCard
                                             {
                                                 AMST_Id = a.AMST_Id
                                             }).ToList();

                            var smartcard1 = (from a in _db.Attendance_Students_SmartCard
                                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASSC_AttendanceDate == today && a.ASMCL_Id == classid1
                                              && a.ASMS_Id == secid)
                                              select new Attendance_Students_SmartCard
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  ASALU_Id = a.ASALU_Id,
                                                  ASSC_NetworkIP = a.ASSC_NetworkIP,
                                                  ASSC_SystemIP = a.ASSC_SystemIP
                                              }).Distinct().ToList();

                            Adm_studentAttendance obj1 = new Adm_studentAttendance();

                            List<Studentattsmartcardabsent> studentList12 = new List<Studentattsmartcardabsent>();
                            if (smartcard.Count > 0)
                            {
                                //---------Check already  data is insert or not----------//
                                var checkattendance = _db.Adm_studentAttendance.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id &&
                                 d.ASMCL_Id == classid1 && d.ASMS_Id == secid && d.ASA_FromDate == data.ASA_FromDate).ToList();
                                if (checkattendance.Count() > 0)
                                {
                                    data.message = "Attendance Is Already collected For This Date";
                                }
                                else
                                {
                                    _acdimpl.LogInformation("Enter in smart card student detais array  loop ");
                                    obj1.MI_Id = data.MI_Id;
                                    obj1.ASMAY_Id = data.ASMAY_Id;
                                    obj1.ASA_Att_Type = "Dailyonce";
                                    obj1.ASA_Att_EntryType = attendanceentrytype;
                                    obj1.ASMCL_Id = Convert.ToInt64(classid1);
                                    obj1.ASMS_Id = Convert.ToInt64(secid);
                                    obj1.ASA_ClassHeld = Convert.ToDecimal("1.00");
                                    obj1.HRME_Id = smartcard1.FirstOrDefault().HRME_Id;
                                    obj1.ASALU_Id = smartcard1.FirstOrDefault().ASALU_Id;
                                    obj1.ASA_Network_IP = smartcard1.FirstOrDefault().ASSC_SystemIP;
                                    obj1.ASA_Mac_Add = smartcard1.FirstOrDefault().ASSC_NetworkIP;

                                    obj1.IMP_Id = 0;
                                    obj1.ASA_Entry_DateTime = Convert.ToDateTime(confromdate);
                                    obj1.ASA_FromDate = Convert.ToDateTime(confromdate);
                                    obj1.ASA_ToDate = Convert.ToDateTime(confromdate);
                                    obj1.CreatedDate = DateTime.Now;
                                    obj1.UpdatedDate = DateTime.Now;
                                    _db.Adm_studentAttendance.Add(obj1);
                                    _acdimpl.LogInformation("obj1 is added to attendance table ");



                                    List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                    for (int k = 0; k < smartcard.Count(); k++)
                                    {
                                        Adm_studentAttendanceStudents obj2 = new Adm_studentAttendanceStudents();
                                        _acdimpl.LogInformation("obj2 for loop enter ");
                                        obj2.AMST_Id = smartcard[k].AMST_Id;
                                        obj2.ASA_Id = obj1.ASA_Id;
                                        obj2.ASA_Class_Attended = Convert.ToDecimal("1.00");
                                        obj2.ASA_AttendanceFlag = "Present";
                                        obj2.CreatedDate = DateTime.Now;
                                        obj2.UpdatedDate = DateTime.Now;
                                        _db.Adm_studentAttendanceStudents.Add(obj2);
                                        _acdimpl.LogInformation("obj2 is added to attendance student table ");
                                    }

                                    //absent list 
                                    //new        
                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                    {
                                        _acdimpl.LogInformation("smart card stored procedure absent details ");
                                        cmd.CommandText = "Attendence_Student_SmartCard";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.BigInt) { Value = classid1 });
                                        cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.BigInt) { Value = secid });
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                        cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.DateTime) { Value = data.ASA_FromDate });

                                        if (cmd.Connection.State != ConnectionState.Open)
                                            cmd.Connection.Open();
                                        List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                                retObject.Add(new Studentattsmartcardabsent
                                                {
                                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                                });
                                            }
                                        }
                                        _acdimpl.LogInformation("retobject is created  ");
                                        studentList1 = retObject.ToList();
                                    }

                                    for (int k = 0; k < studentList1.Count; k++)
                                    {
                                        Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                        _acdimpl.LogInformation("obj3 for absent details ");
                                        obj3.AMST_Id = studentList1[k].AMST_Id;
                                        obj3.ASA_Id = obj1.ASA_Id;
                                        obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                        obj3.ASA_AttendanceFlag = "Absent";
                                        obj3.CreatedDate = DateTime.Now;
                                        obj3.UpdatedDate = DateTime.Now;
                                        _db.Adm_studentAttendanceStudents.Add(obj3);
                                        _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                    }

                                    var result = _db.SaveChanges();
                                    if (result >= 1)
                                    {
                                        data.returnval = true;
                                        data.message = "Record Saved Successfully";
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                        data.message = "Failed To Save Record";
                                    }
                                }
                            }
                            else
                            {
                                List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                {
                                    _acdimpl.LogInformation("smart card stored procedure absent details ");
                                    cmd.CommandText = "Attendence_Student_SmartCard";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.BigInt) { Value = classid1 });
                                    cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.BigInt) { Value = secid });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                    cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.DateTime) { Value = data.ASA_FromDate });

                                    if (cmd.Connection.State != ConnectionState.Open)
                                        cmd.Connection.Open();
                                    List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                            retObject.Add(new Studentattsmartcardabsent
                                            {
                                                AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                            });
                                        }
                                    }
                                    _acdimpl.LogInformation("retobject is created  ");
                                    studentList1 = retObject.ToList();
                                }

                                for (int k = 0; k < studentList1.Count; k++)
                                {
                                    Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                    _acdimpl.LogInformation("obj3 for absent details ");
                                    obj3.AMST_Id = studentList1[k].AMST_Id;
                                    obj3.ASA_Id = obj1.ASA_Id;
                                    obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                    obj3.ASA_AttendanceFlag = "Absent";
                                    obj3.CreatedDate = DateTime.Now;
                                    obj3.UpdatedDate = DateTime.Now;
                                    _db.Adm_studentAttendanceStudents.Add(obj3);
                                    _acdimpl.LogInformation("obj3 is added to attendance student table ");
                                }

                                var result = _db.SaveChanges();
                                if (result >= 1)
                                {
                                    data.returnval = true;
                                    data.message = "Record Saved Successfully";
                                }
                                else
                                {
                                    data.returnval = false;
                                    data.message = "Failed To Save Record";
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (data.rolename == "Staff")
                    {
                        classid = (from a in _db.Masterclasscategory
                                   from b in _db.AcademicYear
                                   from c in _db.School_M_Class
                                   where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.Is_Active == true && a.ASMCL_Id == data.ASMCL_Id)
                                   select new Studentattsmartcardabsent_class_sectionlist
                                   {
                                       ASMCL_Id = c.ASMCL_Id,
                                   }).Distinct().ToList();

                        sectionid = (from a in _db.Masterclasscategory
                                     from d in _db.AdmSchoolMasterClassCatSec
                                     from b in _db.AcademicYear
                                     from c in _db.School_M_Class
                                     from f in _db.Section
                                     where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCC_Id == d.ASMCC_Id && f.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == data.ASMS_Id)
                                     select new Studentattsmartcardabsent_class_sectionlist
                                     {
                                         ASMS_Id = f.ASMS_Id,
                                     }).Distinct().ToList();
                    }
                    else
                    {
                        classid = (from a in _db.Masterclasscategory
                                   from b in _db.AcademicYear
                                   from c in _db.School_M_Class
                                   where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.Is_Active == true)
                                   select new Studentattsmartcardabsent_class_sectionlist
                                   {
                                       ASMCL_Id = c.ASMCL_Id,
                                   }).Distinct().ToList();

                        sectionid = (from a in _db.Masterclasscategory
                                     from d in _db.AdmSchoolMasterClassCatSec
                                     from b in _db.AcademicYear
                                     from c in _db.School_M_Class
                                     from f in _db.Section
                                     where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCC_Id == d.ASMCC_Id && f.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true && a.ASMCL_Id == data.ASMCL_Id)
                                     select new Studentattsmartcardabsent_class_sectionlist
                                     {
                                         ASMS_Id = f.ASMS_Id,
                                     }).Distinct().ToList();
                    }

                    for (int k = 0; k < classid.Count; k++)
                    {
                        var classid1 = classid[k].ASMCL_Id;

                        for (int j = 0; j < sectionid.Count; j++)
                        {
                            var secid = sectionid[j].ASMS_Id;

                            var smartcard1 = (from a in _db.Attendance_Students_SmartCard
                                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == classid1
                                              && a.ASMS_Id == secid)
                                              select new Attendance_Students_SmartCard
                                              {
                                                  HRME_Id = a.HRME_Id,
                                                  ASALU_Id = a.ASALU_Id,
                                                  ASSC_NetworkIP = a.ASSC_NetworkIP,
                                                  ASSC_SystemIP = a.ASSC_SystemIP
                                              }).Distinct().ToList();

                            Adm_studentAttendance obj1 = new Adm_studentAttendance();

                            _acdimpl.LogInformation("Enter in smart card student detais array  loop ");
                            obj1.MI_Id = data.MI_Id;
                            obj1.ASMAY_Id = data.ASMAY_Id;
                            obj1.ASA_Att_Type = "Dailyonce";
                            obj1.ASA_Att_EntryType = attendanceentrytype;
                            obj1.ASMCL_Id = classid1;
                            obj1.ASMS_Id = secid;
                            obj1.ASA_ClassHeld = Convert.ToDecimal("1.00");
                            obj1.HRME_Id = smartcard1.FirstOrDefault().HRME_Id;
                            obj1.ASALU_Id = smartcard1.FirstOrDefault().ASALU_Id;
                            obj1.ASA_Network_IP = smartcard1.FirstOrDefault().ASSC_SystemIP;
                            obj1.ASA_Mac_Add = smartcard1.FirstOrDefault().ASSC_NetworkIP;
                            obj1.IMP_Id = 0;
                            obj1.ASA_Entry_DateTime = Convert.ToDateTime(confromdate);
                            obj1.ASA_FromDate = Convert.ToDateTime(confromdate);
                            obj1.ASA_ToDate = Convert.ToDateTime(confromdate);
                            obj1.CreatedDate = DateTime.Now;
                            obj1.UpdatedDate = DateTime.Now;
                            _db.Adm_studentAttendance.Add(obj1);
                            _acdimpl.LogInformation("obj1 is added to attendance table ");

                            List<Studentattsmartcardabsent> studentList1 = new List<Studentattsmartcardabsent>();

                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                _acdimpl.LogInformation("smart card stored procedure absent details ");
                                cmd.CommandText = "Attendence_Student_SmartCard";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@AYST_Id", SqlDbType.BigInt) { Value = classid1 });
                                cmd.Parameters.Add(new SqlParameter("@Section_Id", SqlDbType.BigInt) { Value = secid });
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASSC_AttendanceDate", SqlDbType.DateTime) { Value = data.ASA_FromDate });

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                List<Studentattsmartcardabsent> retObject = new List<Studentattsmartcardabsent>();

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
                                        retObject.Add(new Studentattsmartcardabsent
                                        {
                                            AMST_Id = Convert.ToInt32(dataReader["AMST_Id"]),
                                        });
                                    }
                                }
                                _acdimpl.LogInformation("retobject is created  ");
                                studentList1 = retObject.ToList();
                            }

                            for (int k1 = 0; k1 < studentList1.Count; k1++)
                            {
                                Adm_studentAttendanceStudents obj3 = new Adm_studentAttendanceStudents();
                                _acdimpl.LogInformation("obj3 for absent details ");
                                obj3.AMST_Id = studentList1[k1].AMST_Id;
                                obj3.ASA_Id = obj1.ASA_Id;
                                obj3.ASA_Class_Attended = Convert.ToDecimal("0.00");
                                obj3.ASA_AttendanceFlag = "Absent";
                                obj3.CreatedDate = DateTime.Now;
                                obj3.UpdatedDate = DateTime.Now;
                                _db.Adm_studentAttendanceStudents.Add(obj3);
                                _acdimpl.LogInformation("obj3 is added to attendance student table ");
                            }

                            var result = _db.SaveChanges();
                            if (result >= 1)
                            {
                                data.returnval = true;
                                data.message = "Record Saved Successfully";
                            }
                            else
                            {
                                data.returnval = false;
                                data.message = "Failed To Save Record";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogInformation("Error in Smartcard Attendance Download From smart card to maintable:'" + ex.Message + "'");
                data.message = "Failed To Save Record";
            }
            return data;
        }



        //update attendance tables from the scheduler
        public StudentAttendanceEntryDTO studentattendanceinsert(StudentAttendanceEntryDTO data)
        {
            try
            {
           
                List<StudentAttendanceEntryDTO> result = new List<StudentAttendanceEntryDTO>();

                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(data.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Student_Attendance_Insert";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    }); 
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.BigInt)
                    {
                        Value = acd_Id
                    });
                   

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
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
                        //data.getinboxmsg_readflg = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                //_acdimpl.LogInformation("Attendance Entry month class held :'" + ex.Message + "'");
            }
            return data;
        }
    }
}
