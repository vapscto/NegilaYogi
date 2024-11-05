using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.admission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SwimmingAttendanceImpl : Interfaces.SwimmingAttendanceInterface
    {
        private DomainModelMsSqlServerContext _context;

        ILogger<SwimmingAttendanceImpl> _acdimpl;

        public SwimmingAttendanceImpl(DomainModelMsSqlServerContext _conte, ILogger<SwimmingAttendanceImpl> _acdim)
        {
            _context = _conte;
            _acdimpl = _acdim;
        }
        public SwimmingAttendanceDTO loaddata(SwimmingAttendanceDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SwimmingAttendanceDTO onchnageyear(SwimmingAttendanceDTO data)
        {
            try
            {
                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new SwimmingAttendanceDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                int UserId = GetUserId(data);


                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(UserId))
                                     select new SwimmingAttendanceDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var checkclassteacher = _context.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (checkclassteacher.Count() > 0)
                    {
                        data.getclass = (from a in _context.ClassTeacherMappingDMO
                                         from b in _context.AcademicYear
                                         from c in _context.School_M_Class
                                         where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.IMCT_ActiveFlag == true
                                         && c.ASMCL_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                         && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code)
                                         select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                    else
                    {
                        data.getclass = (from a in _context.Masterclasscategory
                                         from b in _context.AcademicYear
                                         from c in _context.School_M_Class
                                         where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && c.ASMCL_ActiveFlag == true
                                         && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                         select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                }
                else
                {
                    data.getclass = (from a in _context.Masterclasscategory
                                     from b in _context.AcademicYear
                                     from c in _context.School_M_Class
                                     where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && c.ASMCL_ActiveFlag == true
                                     && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                     select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SwimmingAttendanceDTO onchangeclass(SwimmingAttendanceDTO data)
        {
            try
            {
                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new SwimmingAttendanceDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                int UserId = GetUserId(data);

                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(UserId))
                                     select new SwimmingAttendanceDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var checkclassteacher = _context.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (checkclassteacher.Count() > 0)
                    {
                        data.getsection = (from a in _context.ClassTeacherMappingDMO
                                           from b in _context.AcademicYear
                                           from c in _context.School_M_Class
                                           from d in _context.Section
                                           where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && a.IMCT_ActiveFlag == true
                                           && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1 && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ASMCL_Id == data.ASMCL_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code)
                                           select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                    }
                    else
                    {
                        data.getsection = (from a in _context.Masterclasscategory
                                           from b in _context.AcademicYear
                                           from c in _context.School_M_Class
                                           from d in _context.Section
                                           from e in _context.AdmSchoolMasterClassCatSec
                                           where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && d.ASMC_ActiveFlag == 1
                                           && c.ASMCL_ActiveFlag == true && a.ASMCC_Id == e.ASMCC_Id && d.ASMS_Id == e.ASMS_Id
                                           && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && e.ASMCCS_ActiveFlg == true)
                                           select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                    }
                }
                else
                {
                    data.getsection = (from a in _context.Masterclasscategory
                                       from b in _context.AcademicYear
                                       from c in _context.School_M_Class
                                       from d in _context.Section
                                       from e in _context.AdmSchoolMasterClassCatSec
                                       where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.Is_Active == true && d.ASMC_ActiveFlag == 1
                                       && c.ASMCL_ActiveFlag == true && a.ASMCC_Id == e.ASMCC_Id && d.ASMS_Id == e.ASMS_Id && e.ASMCCS_ActiveFlg == true
                                       && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                       select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SwimmingAttendanceDTO search(SwimmingAttendanceDTO data)
        {
            try
            {
                data.getstandarad = _context.GenConfig.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.admissionstandarad = _context.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(data.ASSC_AttendanceDate.Value.Date.ToString("yyyy-MM-dd"));

                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_Get_Student_Data_Activity_Attendance";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@DATE", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASSC_EntryForFlg", SqlDbType.VarChar)
                    {
                        Value = data.ASSC_EntryForFlg
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
                        data.getstudent = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Error In swimming attendance entry 1 :" + ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_Get_Student_Data_Activity_Attendance";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = 2
                    });
                    cmd.Parameters.Add(new SqlParameter("@DATE", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASSC_EntryForFlg", SqlDbType.VarChar)
                    {
                        Value = data.ASSC_EntryForFlg
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
                        data.getsavedsstudent = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Error In swimming attendance entry 2 :" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SwimmingAttendanceDTO save(SwimmingAttendanceDTO data)
        {
            try
            {
                if (data.Tempstudent.Count() > 0)
                {
                    TimeSpan ts = new TimeSpan();
                    DateTime fromdatecon = DateTime.Now;
                    string confromdate = "";
                    try
                    {
                        fromdatecon = Convert.ToDateTime(DateTime.UtcNow.ToString("HH:mm"));
                        confromdate = fromdatecon.ToString("HH:mm");
                        ts = TimeSpan.Parse(confromdate);
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    foreach (var x in data.Tempstudent)
                    {
                        var checkresult = _context.Attendance_Lunch_Students_SmartCardDMO.Where(a => a.MI_Id == data.MI_Id
                        && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == x.AMST_Id
                        && a.ASSC_AttendanceDate == data.ASSC_AttendanceDate && a.ASSC_EntryForFlg == data.ASSC_EntryForFlg).ToList();
                        if (checkresult.Count() > 0)
                        {
                            var result = _context.Attendance_Lunch_Students_SmartCardDMO.Single(a => a.MI_Id == data.MI_Id
                        && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == x.AMST_Id
                        && a.ASSC_AttendanceDate == data.ASSC_AttendanceDate && a.ASSC_EntryForFlg == data.ASSC_EntryForFlg);

                            result.ALSSC_AttendanceCount = x.ALSSC_AttendanceCount;
                            result.UpdatedDate = DateTime.UtcNow;
                            result.ASSC_SystemIP = data.ASA_Network_IP;
                            result.ASSC_NetworkIP = data.ASA_Network_IP;
                            result.ASSC_PunchTime = ts;
                            _context.Update(result);
                        }
                        else
                        {
                            Attendance_Lunch_Students_SmartCardDMO dmo = new Attendance_Lunch_Students_SmartCardDMO();
                            dmo.MI_Id = data.MI_Id;
                            dmo.AMST_Id = x.AMST_Id;
                            dmo.ASMAY_Id = data.ASMAY_Id;
                            dmo.ASMCL_Id = data.ASMCL_Id;
                            dmo.ASMS_Id = data.ASMS_Id;
                            dmo.ASSC_AttendanceDate = data.ASSC_AttendanceDate;
                            dmo.ASSC_PunchDate = data.ASSC_AttendanceDate;
                            dmo.ASSC_PunchTime = ts;
                            dmo.ASSC_EntryForFlg = data.ASSC_EntryForFlg;
                            dmo.ALSSC_AttendanceCount = x.ALSSC_AttendanceCount;
                            dmo.CreatedDate = DateTime.UtcNow;
                            dmo.UpdatedDate = DateTime.UtcNow;
                            dmo.ASSC_SystemIP = data.ASA_Network_IP;
                            dmo.ASSC_NetworkIP = data.ASA_Network_IP;

                            _context.Add(dmo);
                        }
                    }

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                _acdimpl.LogInformation("Swimming Attendance Save : " + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public int GetUserId(SwimmingAttendanceDTO mas)
        {
            var Get_UserId = _context.ApplicationUser.Where(a => a.UserName == mas.username).Select(a => a.Id).FirstOrDefault();
            return Get_UserId;
        }
    }
}
