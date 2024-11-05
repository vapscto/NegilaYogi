using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class DatewiseAttendanceReportImpl : Interfaces.DatewiseAttendanceReportInterface
    {
        private readonly AdmissionFormContext _AdmissionFormContext;

        private readonly DomainModelMsSqlServerContext _db;
        private readonly ILogger<StudentAdmissionImp> _log;


        public DatewiseAttendanceReportImpl(AdmissionFormContext AdmissionFormContext, DomainModelMsSqlServerContext db, ILogger<StudentAdmissionImp> loggerFactor)
        {
            _AdmissionFormContext = AdmissionFormContext;
            _db = db;
            _log = loggerFactor;
        }


        public DatewiseAttendanceReportDTO getdata(DatewiseAttendanceReportDTO data)
        {
            try
            {
                data.yearlist = _AdmissionFormContext.year.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public DatewiseAttendanceReportDTO onchangeyear(DatewiseAttendanceReportDTO data)
        {
            try
            {
                data.classlist = _AdmissionFormContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).ToArray();

                //var check_rolename = (from a in _db.MasterRoleType
                //                      where (a.IVRMRT_Id == data.roleId)
                //                      select new DatewiseAttendanceReportDTO
                //                      {
                //                          rolename = a.IVRMRT_Role,
                //                      }
                //                 ).ToList();

                //var empcode_check = (from a in _db.Staff_User_Login
                //                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                //                     select new DatewiseAttendanceReportDTO
                //                     {
                //                         Emp_Code = a.Emp_Code,
                //                     }).ToList();

                //if (empcode_check.Count > 0)
                //{
                //    data.classlist = (from a in _db.Adm_SchAttLoginUserClass
                //                      from b in _db.Adm_SchAttLoginUser
                //                      from c in _db.School_M_Class
                //                      where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                //                      && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                //                      && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                //                      && c.ASMCL_ActiveFlag == true)
                //                      select new StudentAttendanceReportDTO
                //                      {
                //                          ASMCL_Id = c.ASMCL_Id,
                //                          asmcL_ClassName = c.ASMCL_ClassName,
                //                      }).Distinct().ToArray();
                //}
                //else
                //{
                //    data.classlist = _AdmissionFormContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).ToArray();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public DatewiseAttendanceReportDTO onchangeclass(DatewiseAttendanceReportDTO data)
        {
            try
            {
                data.sectionlist = _AdmissionFormContext.AdmSection.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1).ToArray();

                //var check_rolename = (from a in _db.MasterRoleType
                //                      where (a.IVRMRT_Id == data.roleId)
                //                      select new DatewiseAttendanceReportDTO
                //                      {
                //                          rolename = a.IVRMRT_Role,
                //                      }
                //               ).ToList();

                //var empcode_check = (from a in _db.Staff_User_Login
                //                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                //                     select new DatewiseAttendanceReportDTO
                //                     {
                //                         Emp_Code = a.Emp_Code,
                //                     }).ToList();

                //if (empcode_check.Count > 0)
                //{
                //    data.sectionlist = (from a in _db.Adm_SchAttLoginUserClass
                //                        from b in _db.Adm_SchAttLoginUser
                //                        from c in _db.School_M_Class
                //                        from d in _db.School_M_Section
                //                        where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                //                        && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                //                        && d.ASMC_ActiveFlag == 1 )
                //                        select new StudentAttendanceReportDTO
                //                        {
                //                            ASMS_Id = d.ASMS_Id,
                //                            ASMC_SectionName = d.ASMC_SectionName,

                //                        }).Distinct().ToArray();
                //}

                //else
                //{
                //    data.sectionlist = _AdmissionFormContext.AdmSection.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1).ToArray();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public DatewiseAttendanceReportDTO getreport(DatewiseAttendanceReportDTO data)
        {
            try
            {
                var asmsid = "0";

                if (data.ASMS_Id == 0)
                {
                    var sectonid = _AdmissionFormContext.AdmSection.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1).ToList();

                    for (int k = 0; k < sectonid.Count; k++)
                    {
                        asmsid = asmsid + "," + sectonid[k].ASMS_Id.ToString();
                    }
                }
                else
                {
                    asmsid = data.ASMS_Id.ToString();
                }

                string confromdate = "";
                DateTime fromdate = DateTime.Now;

                fromdate = Convert.ToDateTime(data.fromdate.Date.ToString("yyyy-MM-dd"));

                confromdate = fromdate.ToString("yyyy-MM-dd");

                if (data.type == "f1")
                {
                    using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Student_Attendance_Report_Datewise";
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
                            Value = asmsid
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                        {
                            Value = data.flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                        {
                            Value = confromdate
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
                            data.reportdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            _log.LogInformation("Error In Datewise attendance report     :" + ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Student_Attendance_Report_Datewise_Count";
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
                            Value = asmsid
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                        {
                            Value = data.flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar)
                        {
                            Value = confromdate
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
                            data.reportdata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            _log.LogInformation("Error In Datewise attendance report     :" + ex.Message);
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

        public DatewiseAttendanceReportDTO getcountreport(DatewiseAttendanceReportDTO data)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public DatewiseAttendanceReportDTO Reportnew(DatewiseAttendanceReportDTO data)
        {
            try
            {
                var FromDate = data.fromdate.Date.ToString("yyyy-MM-dd");
                var ToDate = data.todate.Date.ToString("yyyy-MM-dd");

                using (var cmd = _AdmissionFormContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ADM_Student_LongAbsenteeslist_proc";  //Admission_Get_Continuous_Absent_Attendance_Report
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
                    cmd.Parameters.Add(new SqlParameter("@FROMDATE", SqlDbType.VarChar)
                    {
                        Value = FromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE", SqlDbType.VarChar)
                    {
                        Value = ToDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@NUM", SqlDbType.BigInt)
                    {
                        Value = data.num
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
                        data.reportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _log.LogInformation("Error In Datewise attendance report     :" + ex.Message);
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
