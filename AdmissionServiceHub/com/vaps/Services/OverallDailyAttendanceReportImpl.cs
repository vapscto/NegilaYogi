using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Threading.Tasks;
using System.Linq;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class OverallDailyAttendanceReportImpl : Interfaces.OverallDailyAttendanceInterface
    {
        public StudentAttendanceReportContext _db;
        ILogger<OverallDailyAttendanceReportImpl> _log;


        public OverallDailyAttendanceReportImpl(StudentAttendanceReportContext db, ILogger<OverallDailyAttendanceReportImpl> loggerFactor)
        {
            _db = db;
            _log = loggerFactor;
        }
        public async Task<StudentAttendanceReportDTO> getInitailData(StudentAttendanceReportDTO ctdo)
        {
            // StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(d => d.MI_Id == ctdo.miid && d.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToListAsync();
                ctdo.academicList = allyear.ToArray();

                List<MasterAcademic> allyear1 = new List<MasterAcademic>();
                allyear1 = await _db.academicYear.Where(d => d.MI_Id == ctdo.miid && d.Is_Active == true && d.ASMAY_Id == ctdo.ASMAY_Id).ToListAsync();
                ctdo.academicListdefault = allyear1.ToArray();


                // logo
                var cat = _db.GenConfig.Where(g => g.MI_Id == ctdo.miid && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {


                    ctdo.category_list = _db.category.Where(f => f.MI_Id == ctdo.miid && f.AMC_ActiveFlag == 1).ToArray();
                    ctdo.categoryflag = true;
                }
                else
                {
                    ctdo.categoryflag = false;
                }

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public async Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data)
        {
            int k = 0;
            var asmclid = "";
            var asmsid = "";
            var check_rolename = (from a in _db.MasterRoleType
                                  where (a.IVRMRT_Id == data.roleId)
                                  select new StudentAttendanceEntryDTO
                                  {
                                      rolename = a.IVRMRT_Role,
                                  }).ToList();

            var empcode_check = (from a in _db.Staff_User_Login
                                 where (a.MI_Id == data.miid && a.Id.Equals(data.userId))
                                 select new StudentAttendanceEntryDTO
                                 {
                                     Emp_Code = a.Emp_Code,
                                 }).ToList();


            if (check_rolename.FirstOrDefault().rolename.Equals("STAFF") || check_rolename.FirstOrDefault().rolename.Equals("Staff"))
            {
                k = 1;

                if (empcode_check.Count > 0)
                {
                    var classlist12 = (from a in _db.Adm_SchAttLoginUserClass
                                       from b in _db.Adm_SchAttLoginUser
                                       from c in _db.admissionClass
                                       where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                       && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                       && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                       && c.ASMCL_ActiveFlag == true)
                                       select new StudentAttendanceEntryDTO
                                       {
                                           ASMCL_Id = c.ASMCL_Id,
                                           asmcL_ClassName = c.ASMCL_ClassName,
                                       }
                               ).Distinct().ToList();


                    var SectionList12 = (from a in _db.Adm_SchAttLoginUserClass
                                         from b in _db.Adm_SchAttLoginUser
                                         from c in _db.masterSection
                                         where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                         && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                         && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                         && c.ASMC_ActiveFlag == 1)
                                         select new StudentAttendanceEntryDTO
                                         {
                                             ASMS_Id = c.ASMS_Id,
                                             asmC_SectionName = c.ASMC_SectionName,
                                         }
                                        ).Distinct().ToList();

                    for (int i = 0; i < classlist12.Count; i++)
                    {

                        if (i == 0)
                        {
                            asmclid = classlist12[i].ASMCL_Id.ToString();
                        }
                        else
                        {
                            asmclid = asmclid + ',' + classlist12[i].ASMCL_Id.ToString();
                        }
                    }

                    for (int i = 0; i < SectionList12.Count; i++)
                    {

                        if (i == 0)
                        {
                            asmsid = SectionList12[i].ASMS_Id.ToString();
                        }
                        else
                        {
                            asmsid = asmsid + ',' + SectionList12[i].ASMS_Id.ToString();
                        }
                    }

                }
                else
                {
                    //   data.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                }
            }
            else
            {
                asmclid = "0";
                asmsid = "0";
            }


            var amcid = "0";
            if (data.AMC_Id>0)
            {
                amcid = data.AMC_Id.ToString();

                data.AMC_logo = _db.category.Where(p => p.AMC_Id == data.AMC_Id && p.MI_Id == data.miid && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();


            }


            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "OverallDailyAttendance_NEW1";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                {
                    Value = data.fromdate
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                {
                    Value = data.miid
                });

                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt)
                {
                    Value = k
                });
                cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.NVarChar)
                {
                    Value = asmclid
                });
                cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.NVarChar)
                {
                    Value = asmsid
                });
                cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.NVarChar)
                {
                    Value = data.AMC_Id
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
                    data.studentAttendanceList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    _log.LogInformation("Error in Over All Daily Attendance :" + ex.Message);
                }
                return data;
            }
        }
        public StudentAttendanceReportDTO getStudentDetails(StudentAttendanceReportDTO data)
        {
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "OverallDailyAttendance_by_Class_Section_New1";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                {
                    Value = data.fromdate
                });
                cmd.Parameters.Add(new SqlParameter("@className", SqlDbType.VarChar)
                {
                    Value = data.asmcL_ClassName
                });
                cmd.Parameters.Add(new SqlParameter("@sectionName", SqlDbType.VarChar)
                {
                    Value = data.ASMC_SectionName
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                {
                    Value = data.miid
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
                    data.student_teacherList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return data;
            }
        }
        public async Task<StudentAttendanceReportDTO> getoveallattendance(StudentAttendanceReportDTO data)
        {
            try
            {
                int k = 0;
                var asmclid = "";
                var asmsid = "";
                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new StudentAttendanceEntryDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == data.miid && a.Id.Equals(data.userId))
                                     select new StudentAttendanceEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();


                if (check_rolename.FirstOrDefault().rolename.Equals("STAFF") || check_rolename.FirstOrDefault().rolename.Equals("Staff"))
                {
                    k = 1;

                    if (empcode_check.Count > 0)
                    {
                        var classlist12 = (from a in _db.Adm_SchAttLoginUserClass
                                           from b in _db.Adm_SchAttLoginUser
                                           from c in _db.admissionClass
                                           where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                           && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                           && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                           && c.ASMCL_ActiveFlag == true)
                                           select new StudentAttendanceEntryDTO
                                           {
                                               ASMCL_Id = c.ASMCL_Id,
                                               asmcL_ClassName = c.ASMCL_ClassName,
                                           }).Distinct().ToList();


                        var SectionList12 = (from a in _db.Adm_SchAttLoginUserClass
                                             from b in _db.Adm_SchAttLoginUser
                                             from c in _db.masterSection
                                             where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                             && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                             && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                             && c.ASMC_ActiveFlag == 1)
                                             select new StudentAttendanceEntryDTO
                                             {
                                                 ASMS_Id = c.ASMS_Id,
                                                 asmC_SectionName = c.ASMC_SectionName,
                                             }).Distinct().ToList();

                        for (int i = 0; i < classlist12.Count; i++)
                        {

                            if (i == 0)
                            {
                                asmclid = classlist12[i].ASMCL_Id.ToString();
                            }
                            else
                            {
                                asmclid = asmclid + ',' + classlist12[i].ASMCL_Id.ToString();
                            }
                        }

                        for (int i = 0; i < SectionList12.Count; i++)
                        {

                            if (i == 0)
                            {
                                asmsid = SectionList12[i].ASMS_Id.ToString();
                            }
                            else
                            {
                                asmsid = asmsid + ',' + SectionList12[i].ASMS_Id.ToString();
                            }
                        }

                    }
                    else
                    {
                        //   data.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }
                else
                {
                    asmclid = "0";
                    asmsid = "0";
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "overalldailyattendancereport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = data.miid
                    });

                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt)
                    {
                        Value = k
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.NVarChar)
                    {
                        Value = asmclid
                    });
                    cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.NVarChar)
                    {
                        Value = asmsid
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
                        data.studentAttendanceList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _log.LogInformation("Error in Over All Daily Attendance :" + ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "overalldailyattendancereportstudentlist";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                    {
                        Value = data.miid
                    });

                    cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.BigInt)
                    {
                        Value = k
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmcl_id", SqlDbType.NVarChar)
                    {
                        Value = asmclid
                    });
                    cmd.Parameters.Add(new SqlParameter("@asms_id", SqlDbType.NVarChar)
                    {
                        Value = asmsid
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
                        data.studentAttendanceListnew = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _log.LogInformation("Error in Over All Daily Attendance :" + ex.Message);
                    }
                }

                data.institutiondetails = _db.Institution.Where(a => a.MI_Id == data.miid && a.MI_ActiveFlag == 1).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }


        //

        public StudentAttendanceReportDTO getStudentAllDetails(StudentAttendanceReportDTO data)
        {
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "OverallDailyAttendance_by_Class_Section_All_Details";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_ID", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime)
                {
                    Value = data.fromdate
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt)
                {
                    Value = data.miid
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
                    data.student_teacherList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return data;
            }
        }


    }
}
