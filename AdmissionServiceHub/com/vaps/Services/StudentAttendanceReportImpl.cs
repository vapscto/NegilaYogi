using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using iTextSharp.text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentAttendanceReportImpl : Interfaces.StudentAtttendanceReportInterface
    {
        public StudentAttendanceReportContext _db;
        private DomainModelMsSqlServerContext _admdb;
        ILogger<StudentAttendanceReportImpl> _acdimpl;

        public StudentAttendanceReportImpl(StudentAttendanceReportContext db, ILogger<StudentAttendanceReportImpl> acdimpl, DomainModelMsSqlServerContext admdb)
        {
            _db = db;
            _acdimpl = acdimpl;
            _admdb = admdb;
        }
        public StudentAttendanceReportDTO getInitailData(StudentAttendanceReportDTO data)
        {
            //StudentAttendanceReportDTO data = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.academicYear.Where(t => t.MI_Id == data.miid && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.academicList = allyear.ToArray();

                List<MasterAcademic> defaultyear = new List<MasterAcademic>();
                defaultyear = _db.academicYear.Where(t => t.MI_Id == data.miid && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id).OrderBy(a => a.ASMAY_Order).ToList();
                data.academicListdefault = defaultyear.ToArray();

                List<MasterMonthDMO> allmonth = new List<MasterMonthDMO>();
                allmonth = _db.masterMonth.ToList();
                data.monthList = allmonth.ToArray();

                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                int UserId = GetUserId(data);

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == data.miid && a.Id.Equals(UserId))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();


                if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
                {
                    if (empcode_check.Count > 0)
                    {
                        data.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }).Distinct().ToArray();


                        data.SectionList = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.masterSection
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                            && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                            && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                            && c.ASMC_ActiveFlag == 1)
                                            select new StudentAttendanceReportDTO
                                            {
                                                ASMS_Id = c.ASMS_Id,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                            }).Distinct().ToArray();


                        var cat = _db.GenConfig.Where(g => g.MI_Id == data.miid && g.IVRMGC_CatLogoFlg == true).ToList();
                        if (cat.Count > 0)
                        {


                            data.category_list = _db.category.Where(f => f.MI_Id == data.miid && f.AMC_ActiveFlag == 1).ToArray();
                            data.categoryflag = true;
                        }
                        else
                        {
                            data.categoryflag = false;
                        }
                    }
                    else
                    {
                        //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }
                else
                {
                    List<School_M_Class> allclass = new List<School_M_Class>();
                    allclass = _db.admissionClass.Where(s => s.MI_Id == data.miid && s.ASMCL_ActiveFlag == true).ToList();
                    data.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> allsection = new List<School_M_Section>();
                    allsection = _db.masterSection.Where(y => y.MI_Id == data.miid && y.ASMC_ActiveFlag == 1).ToList();
                    data.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();


                    var cat = _db.GenConfig.Where(g => g.MI_Id == data.miid && g.IVRMGC_CatLogoFlg == true).ToList();
                    if (cat.Count > 0)
                    {


                        data.category_list = _db.category.Where(f => f.MI_Id == data.miid && f.AMC_ActiveFlag == 1).ToArray();
                        data.categoryflag = true;
                    }
                    else
                    {
                        data.categoryflag = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO getDataByTypeSelected(int id)
        {
            StudentAttendanceReportDTO data = new StudentAttendanceReportDTO();

            data.studentList = (from a in _db.admissionStduent
                                from b in _db.admissionyearstudent
                                where a.AMST_Id == b.AMST_Id && a.AMST_SOL == "S"
                                select new StudentAttendanceReportDTO
                                {
                                    AMST_Id = a.AMST_Id,
                                    AMST_FirstName = a.AMST_FirstName,
                                    AMST_MiddleName = a.AMST_MiddleName,
                                    AMST_LastName = a.AMST_LastName,
                                    AMST_AdmNo = a.AMST_AdmNo

                                }).ToArray();
            return data;
        }
        public async Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data)
        {
            var amcid = "0";
            if (data.AMC_Id == null || data.AMC_Id == 0)
            {
                data.AMC_Id = 0;

            }
            if(data.AMC_Id>0)
            {
                amcid = data.AMC_Id.ToString();

                data.AMC_logo = _db.category.Where(p => p.AMC_Id == data.AMC_Id && p.MI_Id == data.miid && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();
            }
          

            _acdimpl.LogInformation("entered fnction block");
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Attendance_monthwise_bkp";
                //Attendance_monthwise
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                 SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMCL_ID", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMCL_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMC_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                {
                    Value = Convert.ToDateTime(data.fromdate).ToString("dd/MM/yyyy")
                });
                cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                {
                    Value = Convert.ToDateTime(data.todate).ToString("dd/MM/yyyy")
                });
                cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.Int)
                {
                    Value = Convert.ToInt64(data.type)
                });
                cmd.Parameters.Add(new SqlParameter("@radiotype", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.radiotype)
                });
                cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.AMST_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.AMM_ID)
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.miid)
                });
                cmd.Parameters.Add(new SqlParameter("@datewise", SqlDbType.VarChar)
                {
                    Value = data.datewise
                });
                cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.VarChar)
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
                                dataRow.Add(dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.studentAttendanceList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    //  Console.WriteLine(ex.Message);
                    _acdimpl.LogInformation("error : '" + ex.Message + "'");
                }
            }
            if (data.datewise == 1)
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Student_Attendance_Month_Datewise_Namebinding";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@month",
                     SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMM_ID)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                 SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });
                    //cmd.Parameters.Add(new SqlParameter("@AMC_Id",
                    //    SqlDbType.VarChar)
                    //{
                    //    Value = data.AMC_Id
                    //});

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
                                    dataRow.Add(dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.monthList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _acdimpl.LogInformation("error : '" + ex.Message + "'");
                    }
                }
                List<StudentAttendanceReportDTO> result = new List<StudentAttendanceReportDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Attendance_Monthwise_TotalClassHeld";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_ID", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMCL_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMC_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMM_ID)
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                 SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });
                    //cmd.Parameters.Add(new SqlParameter("@AMC_Id", 
                    //    SqlDbType.VarChar)
                    //{
                    //    Value = data.AMC_Id
                    //});

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new StudentAttendanceReportDTO
                                {
                                    countclass = Convert.ToDecimal(dataReader["classheld"]),
                                });
                            }
                        }

                        if (result.Count > 0)
                        {
                            data.countclass = result.FirstOrDefault().countclass;
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _acdimpl.LogInformation("error : '" + ex.Message + "'");
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    //Adm_Get_Attendance_percentage_Student_Attendancereport
                    cmd.CommandText = "Adm_Get_Attendance_percentage_Student_Attendancereport_category";
                   // cmd.CommandText = "Adm_Get_Attendance_percentage_Student_Attendancereport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMCL_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.ASMC_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MONTHId", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMM_ID)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.VarChar)
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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.totalpercentageatt = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _acdimpl.LogInformation("error : '" + ex.Message + "'");
                    }
                }
            }

            data.institutiondetails = _db.Institution.Where(a => a.MI_Id == data.miid && a.MI_ActiveFlag == 1).ToArray();


            return data;
        }
        public async Task<StudentAttendanceReportDTO> getdatatype(StudentAttendanceReportDTO data)
        {
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Adm_namebinding";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMCL_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@secid", SqlDbType.VarChar)
                {
                    Value = Convert.ToInt32(data.ASMC_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar)
                {
                    Value = data.type1
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.miid)
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
                                    dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                );
                            }

                            retObject.Add((ExpandoObject)dataRow);
                        }
                    }
                    data.studentAttendanceList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return data;
            }
        }
        public StudentAttendanceReportDTO getreportdiv(StudentAttendanceReportDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Attendance_Deviation_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@year", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@secid", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMC_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                  SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(data.fromdate).ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                SqlDbType.VarChar)
                    {
                        Value = Convert.ToDateTime(data.todate).ToString("yyyy-MM-dd")
                    });

                    cmd.Parameters.Add(new SqlParameter("@flag",
             SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.flag)
                    });
                    cmd.Parameters.Add(new SqlParameter("@hrmeid",
          SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.hrmE_Id)
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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.studentAttendanceList = retObject.ToArray();
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
        public StudentAttendanceReportDTO savetmpldatanew(StudentAttendanceReportDTO data)
        {
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Admission_Attendance_Total_present_absent_classwise";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });
                    cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMM_ID)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMC_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = data.type
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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.newarray = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Admission_Get_MonthDates_Attendance";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });

                    cmd.Parameters.Add(new SqlParameter("@monthid", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.AMM_ID)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.newarray_date = retObject.ToArray();
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

            if (data.type == 1)
            {
                data.classsecdetails = (from a in _db.admissionClass
                                        from b in _db.masterSection
                                        from c in _db.AdmSchoolMasterClassCatSec
                                        from d in _db.Masterclasscategory
                                        from e in _db.academicYear
                                        where (a.ASMCL_Id == d.ASMCL_Id && c.ASMCC_Id == d.ASMCC_Id && d.ASMAY_Id == e.ASMAY_Id && c.ASMS_Id == b.ASMS_Id
                                        && a.MI_Id == data.miid && d.MI_Id == data.miid && d.ASMAY_Id == data.ASMAY_Id && d.Is_Active == true && d.Is_Active == true)
                                        select new StudentAttendanceReportDTO
                                        {
                                            asmcL_ClassName = a.ASMCL_ClassName,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMCL_Id = a.ASMCL_Id,
                                            ASMS_Id = b.ASMS_Id,
                                            classorder = a.ASMCL_Order,
                                            secorder = b.ASMC_Order
                                        }).OrderBy(f => f.classorder).ThenBy(g => g.secorder).ToArray();

                data.classdetails = _db.admissionClass.Where(a => a.MI_Id == data.miid && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();
            }
            else
            {
                data.classsecdetails = (from a in _db.admissionClass
                                        from b in _db.masterSection
                                        from c in _db.AdmSchoolMasterClassCatSec
                                        from d in _db.Masterclasscategory
                                        from e in _db.academicYear
                                        where (a.ASMCL_Id == d.ASMCL_Id && c.ASMCC_Id == d.ASMCC_Id && d.ASMAY_Id == e.ASMAY_Id && c.ASMS_Id == b.ASMS_Id && a.MI_Id == data.miid && d.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMC_Id
                                        && d.MI_Id == data.miid && d.ASMAY_Id == data.ASMAY_Id && d.Is_Active == true && d.Is_Active == true)
                                        select new StudentAttendanceReportDTO
                                        {
                                            asmcL_ClassName = a.ASMCL_ClassName,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMCL_Id = a.ASMCL_Id,
                                            ASMS_Id = b.ASMS_Id,
                                            classorder = a.ASMCL_Order,
                                            secorder = b.ASMC_Order
                                        }).OrderBy(f => f.classorder).ThenBy(g => g.secorder).ToArray();




                data.classdetails = _db.admissionClass.Where(a => a.MI_Id == data.miid && a.ASMCL_ActiveFlag == true && a.ASMCL_Id == data.ASMCL_Id).OrderBy(a => a.ASMCL_Order).ToArray();


            }

            return data;
        }
        public StudentAttendanceReportDTO onchangeyear(StudentAttendanceReportDTO data)
        {
            try
            {
                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                int UserId = GetUserId(data);

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == data.miid && a.Id.Equals(UserId))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();
                if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
                {
                    if (empcode_check.Count > 0)
                    {
                        data.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }).Distinct().ToArray();
                    }
                    else
                    {
                        //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }
                else
                {
                    data.classlist = (from a in _db.admissionClass
                                      from b in _db.Masterclasscategory
                                      from c in _db.academicYear
                                      where (a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.Is_Active == true && c.Is_Active == true
                                      && b.ASMAY_Id == data.ASMAY_Id)
                                      select a).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }


                data.stafflist = (from a in _db.Adm_SchAttLoginUser
                                  from b in _db.Adm_SchAttLoginUserClass
                                  from c in _db.admissionClass
                                  from d in _db.masterSection
                                  from e in _db.masteremployee
                                  from f in _db.academicYear
                                  where (a.ASALU_Id == b.ASALU_Id && a.HRME_Id == e.HRME_Id && a.ASMAY_Id == f.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id
                                  && b.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && c.ASMCL_ActiveFlag == true
                                  && d.ASMC_ActiveFlag == 1 && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && a.MI_Id == data.miid)
                                  select new StudentAttendanceReportDTO
                                  {
                                      employeename = ((e.HRME_EmployeeFirstName == null ? "" : e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null ? "" : " " + e.HRME_EmployeeLastName) + (e.HRME_EmployeeCode == null ? "" : " : " + e.HRME_EmployeeCode)).Trim(),
                                      hrmE_Id = e.HRME_Id
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO onclasschange(StudentAttendanceReportDTO data)
        {
            try
            {
                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                int UserId = GetUserId(data);

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == data.miid && a.Id.Equals(UserId))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
                {
                    if (empcode_check.Count > 0)
                    {
                        data.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                          from b in _db.Adm_SchAttLoginUser
                                          from c in _db.admissionClass
                                          where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                          && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                          && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                          && c.ASMCL_ActiveFlag == true)
                                          select new StudentAttendanceReportDTO
                                          {
                                              ASMCL_Id = c.ASMCL_Id,
                                              asmcL_ClassName = c.ASMCL_ClassName,
                                          }).Distinct().ToArray();


                        data.SectionList = (from a in _db.Adm_SchAttLoginUserClass
                                            from b in _db.Adm_SchAttLoginUser
                                            from c in _db.masterSection
                                            where (a.ASALU_Id == b.ASALU_Id && c.ASMS_Id == a.ASMS_Id
                                            && b.MI_Id == data.miid && b.ASMAY_Id == data.ASMAY_Id
                                            && b.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                            && c.ASMC_ActiveFlag == 1)
                                            select new StudentAttendanceReportDTO
                                            {
                                                ASMS_Id = c.ASMS_Id,
                                                ASMC_SectionName = c.ASMC_SectionName,
                                            }).Distinct().ToArray();
                    }
                    else
                    {
                        //   mas.message = "For This Staff There Is No Previlages To Enter Attendance.. Please Contact Administrator";
                    }
                }
                else
                {
                    data.SectionList = (from a in _db.AdmSchoolMasterClassCatSec
                                        from b in _db.Masterclasscategory
                                        from c in _db.admissionClass
                                        from d in _db.masterSection
                                        where (a.ASMCC_Id == b.ASMCC_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ASMCCS_ActiveFlg == true
                                        && b.Is_Active == true && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1 && b.MI_Id == data.miid
                                        && b.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id)
                                        select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO onsectionchange(StudentAttendanceReportDTO data)
        {
            try
            {
                if (data.ASMS_Id == 0)
                {
                    data.stafflist = (from a in _db.Adm_SchAttLoginUser
                                      from b in _db.Adm_SchAttLoginUserClass
                                      from c in _db.admissionClass
                                      from d in _db.masterSection
                                      from e in _db.masteremployee
                                      from f in _db.academicYear
                                      where (a.ASALU_Id == b.ASALU_Id && a.HRME_Id == e.HRME_Id && a.ASMAY_Id == f.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id
                                      && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && c.ASMCL_ActiveFlag == true
                                      && d.ASMC_ActiveFlag == 1 && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && a.MI_Id == data.miid)
                                      select new StudentAttendanceReportDTO
                                      {
                                          employeename = ((e.HRME_EmployeeFirstName == null ? "" : e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null ? "" : " " + e.HRME_EmployeeLastName) + (e.HRME_EmployeeCode == null ? "" : " : " + e.HRME_EmployeeCode)).Trim(),
                                          hrmE_Id = e.HRME_Id
                                      }).Distinct().ToArray();
                }
                else
                {
                    data.stafflist = (from a in _db.Adm_SchAttLoginUser
                                      from b in _db.Adm_SchAttLoginUserClass
                                      from c in _db.admissionClass
                                      from d in _db.masterSection
                                      from e in _db.masteremployee
                                      from f in _db.academicYear
                                      where (a.ASALU_Id == b.ASALU_Id && a.HRME_Id == e.HRME_Id && a.ASMAY_Id == f.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id
                                      && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && c.ASMCL_ActiveFlag == true
                                      && d.ASMC_ActiveFlag == 1 && e.HRME_ActiveFlag == true && e.HRME_LeftFlag == false && a.MI_Id == data.miid)
                                      select new StudentAttendanceReportDTO
                                      {
                                          employeename = ((e.HRME_EmployeeFirstName == null ? "" : e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null ? "" : " " + e.HRME_EmployeeLastName) + (e.HRME_EmployeeCode == null ? "" : " : " + e.HRME_EmployeeCode)).Trim(),
                                          hrmE_Id = e.HRME_Id
                                      }).Distinct().ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Active Deactive Report
        public StudentAttendanceReportDTO getreport(StudentAttendanceReportDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Active_Deactive_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.miid)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMC_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = data.type
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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.newarray = retObject.ToArray();
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

        // Subject wise attednace report
        public StudentAttendanceReportDTO LoadData(StudentAttendanceReportDTO data)
        {
            try
            {
                data.academicList = _db.academicYear.Where(a => a.MI_Id == data.miid && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                var getuserid = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();

                if (getuserid.Count > 0)
                {
                    data.classlist = (from a in _db.Adm_SchAttLoginUser
                                      from b in _db.Adm_SchAttLoginUserClass
                                      from c in _db.admissionClass
                                      from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                      where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASALUC_Id == d.ASALUC_Id && a.ASMAY_Id == data.ASMAY_Id
                                      && a.MI_Id == data.miid && a.HRME_Id == getuserid.FirstOrDefault().Emp_Code)
                                      select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
                else
                {
                    data.classlist = (from a in _db.Adm_SchAttLoginUser
                                      from b in _db.Adm_SchAttLoginUserClass
                                      from c in _db.admissionClass
                                      from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                      where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASALUC_Id == d.ASALUC_Id && a.ASMAY_Id == data.ASMAY_Id
                                      && a.MI_Id == data.miid)
                                      select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }

                data.monthList = _db.masterMonth.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO OnChangeAcademicYear(StudentAttendanceReportDTO data)
        {
            try
            {
                var getuserid = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();

                if (getuserid.Count > 0)
                {
                    data.classlist = (from a in _db.Adm_SchAttLoginUser
                                      from b in _db.Adm_SchAttLoginUserClass
                                      from c in _db.admissionClass
                                      from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                      where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASALUC_Id == d.ASALUC_Id && a.ASMAY_Id == data.ASMAY_Id
                                      && a.MI_Id == data.miid && a.HRME_Id == getuserid.FirstOrDefault().Emp_Code)
                                      select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
                else
                {
                    data.classlist = (from a in _db.Adm_SchAttLoginUser
                                      from b in _db.Adm_SchAttLoginUserClass
                                      from c in _db.admissionClass
                                      from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                      where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASALUC_Id == d.ASALUC_Id && a.ASMAY_Id == data.ASMAY_Id
                                      && a.MI_Id == data.miid)
                                      select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO OnChangeClass(StudentAttendanceReportDTO data)
        {
            try
            {
                var getuserid = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();

                if (getuserid.Count > 0)
                {
                    data.SectionList = (from a in _db.Adm_SchAttLoginUser
                                        from b in _db.Adm_SchAttLoginUserClass
                                        from c in _db.admissionClass
                                        from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                        from e in _db.masterSection
                                        where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && b.ASALUC_Id == d.ASALUC_Id
                                        && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.miid && b.ASMCL_Id == data.ASMCL_Id
                                        && a.HRME_Id == getuserid.FirstOrDefault().Emp_Code)
                                        select e).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
                else
                {
                    data.SectionList = (from a in _db.Adm_SchAttLoginUser
                                        from b in _db.Adm_SchAttLoginUserClass
                                        from c in _db.admissionClass
                                        from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                        from e in _db.masterSection
                                        where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && b.ASALUC_Id == d.ASALUC_Id
                                        && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.miid && b.ASMCL_Id == data.ASMCL_Id)
                                        select e).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO OnChangeSection(StudentAttendanceReportDTO data)
        {
            try
            {
                var getuserid = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();

               

                if (getuserid.Count > 0)
                {
                    data.subjectlist = (from a in _db.Adm_SchAttLoginUser
                                        from b in _db.Adm_SchAttLoginUserClass
                                        from c in _db.admissionClass
                                        from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                        from e in _db.masterSection
                                        from f in _db.IVRM_Master_SubjectsDMO
                                        where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && b.ASALUC_Id == d.ASALUC_Id
                                        && d.ISMS_Id == f.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.miid && b.ASMCL_Id == data.ASMCL_Id
                                        && a.HRME_Id == getuserid.FirstOrDefault().Emp_Code && b.ASMS_Id == data.ASMS_Id)
                                        select f).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();


                }
                else
                {
                    data.subjectlist = (from a in _db.Adm_SchAttLoginUser
                                        from b in _db.Adm_SchAttLoginUserClass
                                        from c in _db.admissionClass
                                        from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                        from e in _db.masterSection
                                        from f in _db.IVRM_Master_SubjectsDMO
                                        where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && b.ASALUC_Id == d.ASALUC_Id
                                        && d.ISMS_Id == f.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.miid && b.ASMCL_Id == data.ASMCL_Id
                                         && b.ASMS_Id == data.ASMS_Id)
                                        select f).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

                    
                }

                data.getstudentlist = (from e in _db.admissionStduent
                                       from f in _db.admissionyearstudent
                                       where (e.AMST_Id == f.AMST_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && f.ASMAY_Id == data.ASMAY_Id
                                       && e.MI_Id == data.miid && (e.AMST_SOL != "Del" || e.AMST_SOL != "WD"))
                                       select new StudentAttendanceReportDTO
                                       {
                                           AMST_Id = e.AMST_Id,
                                           AMST_FirstName = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + " " + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + " " + (e.AMST_LastName == null ? " " : e.AMST_LastName) +
                                           (e.AMST_AdmNo == null || e.AMST_AdmNo == "" ? "" : " : " + e.AMST_AdmNo)).Trim(),
                                       }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

                //data.getstudentlist = (from e in _db.admissionStduent
                //                       from f in _db.admissionyearstudent
                //                       where (e.AMST_Id == f.AMST_Id && asmclids.Contains(f.ASMCL_Id) && f.ASMS_Id == data.ASMS_Id && f.ASMAY_Id == data.ASMAY_Id
                //                       && e.MI_Id == data.miid && (e.AMST_SOL != "Del" || e.AMST_SOL != "WD"))
                //                       select new StudentAttendanceReportDTO
                //                       {
                //                           AMST_Id = e.AMST_Id,
                //                           AMST_FirstName = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + " " + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + " " + (e.AMST_LastName == null ? " " : e.AMST_LastName) +
                //                           (e.AMST_AdmNo == null || e.AMST_AdmNo == "" ? "" : " : " + e.AMST_AdmNo)).Trim(),
                //                       }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO OnReport(StudentAttendanceReportDTO data)
        {
            try
            {
                if (data.reportflag == "Date")
                {
                    DateTime fromdatecon = DateTime.Now;
                    string confromdate = "";
                    fromdatecon = Convert.ToDateTime(data.fromdate.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    DateTime todatecon = DateTime.Now;
                    string tofromdate = "";
                    todatecon = Convert.ToDateTime(data.todate.Date.ToString("yyyy-MM-dd"));
                    tofromdate = todatecon.ToString("yyyy-MM-dd");

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_School_SubjectwiseAttendance";
                        //Adm_School_SubjectwiseAttendance
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.newarray = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_School_SubjectwiseAttendance_Total";
                        //Adm_School_SubjectwiseAttendance_Total
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.newarray_total = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if (data.reportflag == "Year")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_School_SubjectWise_YearlyAttendance";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.newarray = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    var getyeardates = _db.academicYear.Where(a => a.MI_Id == data.miid && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true).ToArray();

                    var start = getyeardates.FirstOrDefault().ASMAY_From_Date.Value.Date;
                    var end = getyeardates.FirstOrDefault().ASMAY_To_Date.Value.Date;
                    List<StudentAttendanceReportDTO> FilesPaths = new List<StudentAttendanceReportDTO>();
                    for (DateTime dt = start; dt <= end; dt = dt.AddMonths(1))
                    {
                        StudentAttendanceReportDTO emp = new StudentAttendanceReportDTO();
                        emp.columnname = dt.ToString("MMMyyyy");
                        FilesPaths.Add(emp);
                    }
                    data.getbtwn_monthsname = FilesPaths.ToArray();
                }

                else if (data.reportflag == "StudentWise")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_School_MultipleSubjectsWise_YearlyAttendance";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = 0 });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.newarray = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    var getsubjectids = _db.StudentMappingDMO.Where(a => a.MI_Id == data.miid && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id && a.ESTSU_ActiveFlg == true).Select(a => a.ISMS_Id).Distinct().ToList();

                    data.getsubjectlist = _db.IVRM_Master_SubjectsDMO.Where(a => a.MI_Id == data.miid && getsubjectids.Contains(a.ISMS_Id)).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }

                else if (data.reportflag == "MonthDateWise")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Attendance_Periods_daywise_report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@MonthId", SqlDbType.VarChar) { Value = data.monthid });
                        cmd.Parameters.Add(new SqlParameter("@YearId", SqlDbType.VarChar) { Value = data.yearid });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "1" });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.newarray = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Attendance_Periods_daywise_report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@MonthId", SqlDbType.VarChar) { Value = data.monthid });
                        cmd.Parameters.Add(new SqlParameter("@YearId", SqlDbType.VarChar) { Value = data.yearid });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "2" });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.newarray_date = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    List<DateTime> daysOfMonth = Enumerable.Range(1, DateTime.DaysInMonth(data.yearid, data.monthid))
                             .Select(day => new DateTime(data.yearid, data.monthid, day)).ToList();

                    List<StudentAttendanceReportDTO> FilesPaths = new List<StudentAttendanceReportDTO>();

                    foreach (var c in daysOfMonth)
                    {
                        StudentAttendanceReportDTO emp = new StudentAttendanceReportDTO();

                        DateTime _temp = DateTime.Now;
                        string tofromdate = "";
                        _temp = Convert.ToDateTime(c.ToString("dd/MM/yyyy"));
                        tofromdate = _temp.ToString("dd/MM/yyyy");
                        emp.columnname = tofromdate;
                        FilesPaths.Add(emp);
                    }
                    data.daysOfMonth = FilesPaths.ToArray();
                }

                /* This Condition Will Come From Another Page (StudentAttendancePerdiodWiseAbsentReport.html) */
                else if (data.reportflag == "StudentAbsentPeriodWise")
                {
                    DateTime fromdatecon = DateTime.Now;
                    string confromdate = "";
                    fromdatecon = Convert.ToDateTime(data.fromdate.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    DateTime todatecon = DateTime.Now;
                    string tofromdate = "";
                    todatecon = Convert.ToDateTime(data.todate.Date.ToString("yyyy-MM-dd"));
                    tofromdate = todatecon.ToString("yyyy-MM-dd");

                    string amstid = "0";
                    if (data.Get_Selected_Student_List != null && data.Get_Selected_Student_List.Length > 0)
                    {
                        foreach (var c in data.Get_Selected_Student_List)
                        {
                            amstid = amstid + "," + c.AMST_Id;
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Students_DayPeriodWisePresentAbsent_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstid });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.newarray = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else if (data.reportflag == "SubjectWiseAttEntry")
                {
                    DateTime fromdatecon = DateTime.Now;
                    string confromdate = "";
                    fromdatecon = Convert.ToDateTime(data.fromdate.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    DateTime todatecon = DateTime.Now;
                    string tofromdate = "";
                    todatecon = Convert.ToDateTime(data.todate.Date.ToString("yyyy-MM-dd"));
                    tofromdate = todatecon.ToString("yyyy-MM-dd");

                    string ismsids = "0";
                    if (data.Get_Selected_Subject_List != null && data.Get_Selected_Subject_List.Length > 0)
                    {
                        foreach (var c in data.Get_Selected_Subject_List)
                        {
                            ismsids = ismsids + "," + c.ISMS_Id;
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Staffwise_AttendanceEntered_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = ismsids });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "1" });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.newarray = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_Staffwise_AttendanceEntered_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = ismsids });
                        cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "2" });

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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.daysOfMonth = retObject.ToArray();
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
        public StudentAttendanceReportDTO PeriodWiseReportOverAll(StudentAttendanceReportDTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                fromdatecon = Convert.ToDateTime(data.fromdate.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                DateTime todatecon = DateTime.Now;
                string tofromdate = "";
                todatecon = Convert.ToDateTime(data.todate.Date.ToString("yyyy-MM-dd"));
                tofromdate = todatecon.ToString("yyyy-MM-dd");

                var get_Staffdetails = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();
                long HRME_Id = 0;
                if (get_Staffdetails.Count > 0)
                {
                    HRME_Id = get_Staffdetails.FirstOrDefault().Emp_Code;
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_SchoolPeriodWiseAttendance_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = HRME_Id });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.newarray = retObject.ToArray();
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
        public StudentAttendanceReportDTO OnAttendanceLoadData(StudentAttendanceReportDTO data)
        {
            try
            {
                data.academicList = _db.academicYear.Where(a => a.MI_Id == data.miid && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.classlist = (from a in _db.Masterclasscategory
                                  from b in _db.admissionClass
                                  from c in _db.academicYear
                                  where (a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.Is_Active == true && a.MI_Id == data.miid
                                  && a.ASMAY_Id == data.ASMAY_Id)
                                  select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO OnAttendanceChangeYear(StudentAttendanceReportDTO data)
        {
            try
            {
                var get_Staffdetails = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();
                long HRME_Id = 0;
                if (get_Staffdetails.Count > 0)
                {
                    HRME_Id = get_Staffdetails.FirstOrDefault().Emp_Code;
                }

                data.classlist = (from a in _db.Masterclasscategory
                                  from b in _db.admissionClass
                                  from c in _db.academicYear
                                  where (a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.Is_Active == true && a.MI_Id == data.miid
                                  && a.ASMAY_Id == data.ASMAY_Id)
                                  select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO OnAttendanceChangeClass(StudentAttendanceReportDTO data)
        {
            try
            {
                var get_Staffdetails = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();
                long HRME_Id = 0;
                if (get_Staffdetails.Count > 0)
                {
                    HRME_Id = get_Staffdetails.FirstOrDefault().Emp_Code;
                }

                data.SectionList = (from a in _db.Masterclasscategory
                                    from b in _db.admissionClass
                                    from c in _db.academicYear
                                    from d in _db.AdmSchoolMasterClassCatSec
                                    from e in _db.masterSection
                                    where (a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMCC_Id == d.ASMCC_Id && d.ASMS_Id == e.ASMS_Id
                                    && a.Is_Active == true && a.MI_Id == data.miid && a.ASMCL_Id == data.ASMCL_Id
                                    && a.ASMAY_Id == data.ASMAY_Id)
                                    select e).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO OnAttendanceChangeSection(StudentAttendanceReportDTO data)
        {
            try
            {
                var get_Staffdetails = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();
                long HRME_Id = 0;
                if (get_Staffdetails.Count > 0)
                {
                    HRME_Id = get_Staffdetails.FirstOrDefault().Emp_Code;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentAttendanceReportDTO GetAttendanceDeletedReport(StudentAttendanceReportDTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                fromdatecon = Convert.ToDateTime(data.fromdate.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                DateTime todatecon = DateTime.Now;
                string tofromdate = "";
                todatecon = Convert.ToDateTime(data.todate.Date.ToString("yyyy-MM-dd"));
                tofromdate = todatecon.ToString("yyyy-MM-dd");

                var get_Staffdetails = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();
                long HRME_Id = 0;
                if (get_Staffdetails.Count > 0)
                {
                    HRME_Id = get_Staffdetails.FirstOrDefault().Emp_Code;
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_School_Attendance_DeletionRecords_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.sectionids });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.VarChar) { Value = HRME_Id });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = data.reportflag });

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
                                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.newarray = retObject.ToArray();
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
        public int GetUserId(StudentAttendanceReportDTO mas)
        {
            var Get_UserId = _db.ApplicationUser.Where(a => a.UserName == mas.username).Select(a => a.Id).FirstOrDefault();
            return Get_UserId;
        }


        public StudentAttendanceReportDTO getclass(StudentAttendanceReportDTO data)
        {
            //try
            //{

            //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            //    {
            //        //AlumnistudentsearchReport_new
            //        //AlumnistudentsearchReport
            //        cmd.CommandText = "Total_strength_class";
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
            //        cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.BigInt) { Value = data.AMC_Id });
            //        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.miid });

            //        if (cmd.Connection.State != ConnectionState.Open)
            //            cmd.Connection.Open();

            //        var retObject = new List<dynamic>();
            //        try
            //        {
            //            using (var dataReader = cmd.ExecuteReader())
            //            {
            //                while (dataReader.Read())
            //                {
            //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                    {
            //                        dataRow.Add(
            //                            dataReader.GetName(iFiled),
            //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                        );
            //                    }

            //                    retObject.Add((ExpandoObject)dataRow);
            //                }
            //            }
            //            data.fillclass = retObject.ToArray();
            //            //if (data.fillclass.Length > 0)
            //            //{
            //            //    data.count = data.fillclass.Length;
            //            //}
            //            //else
            //            //{
            //            //    data.count = 0;
            //            //}
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.Write(ex.Message);
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}



            try
            {
                var check_rolename = (from a in _db.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new StudentAttendanceReportDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                int UserId = GetUserId(data);

                var empcode_check = (from a in _db.Staff_User_Login
                                     where (a.MI_Id == data.miid && a.Id.Equals(UserId))
                                     select new StudentAttendanceReportDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();
                long? emp_id = 0;
                if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
                {
                    if (empcode_check.Count > 0)
                    {
                        emp_id = empcode_check.FirstOrDefault().Emp_Code;
                    }

                }
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    //AlumnistudentsearchReport_new
                    //AlumnistudentsearchReport
                    cmd.CommandText = "Total_strength_class";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.BigInt) { Value = data.AMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.miid });
                    cmd.Parameters.Add(new SqlParameter("@Emp_Id", SqlDbType.BigInt) { Value = emp_id });

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
                        data.fillclass = retObject.ToArray();
                        //if (data.fillclass.Length > 0)
                        //{
                        //    data.count = data.fillclass.Length;
                        //}
                        //else
                        //{
                        //    data.count = 0;
                        //}
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
         
        }

        //subjectwise student sms
        public StudentAttendanceReportDTO getstudetails(StudentAttendanceReportDTO data)
        {
            try
            {
                if (data.reportflag == "Date")
                {
                    DateTime fromdatecon = DateTime.Now;
                    string confromdate = "";
                    fromdatecon = Convert.ToDateTime(data.fromdate.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    DateTime todatecon = DateTime.Now;
                    string tofromdate = "";
                    todatecon = Convert.ToDateTime(data.todate.Date.ToString("yyyy-MM-dd"));
                    tofromdate = todatecon.ToString("yyyy-MM-dd");


                    //List<long> asmclids = new List<long>();
                    //foreach (var item in data.classlsttwo)
                    //{
                       
                    //    asmclids.Add(item.ASMCL_Id);
                    //}

                    //List<long> asmsids = new List<long>();
                    //foreach (var item in data.sectionlistarray)
                    //{
                       
                    //    asmsids.Add(item.ASMS_Id);
                    //}

                    //List<long> ismsids = new List<long>();
                    //foreach (var item in data.subjectlistarray)
                    //{
                      
                    //    ismsids.Add(item.ISMS_Id);
                    //}


                    string asmcl_ids = "0";
                    if (data.classlsttwo != null && data.classlsttwo.Length > 0)
                    {
                        if (data.classlsttwo.Length > 0)
                        {
                            foreach (var ue in data.classlsttwo)
                            {
                                asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;

                            }

                        }


                    }


                    string asms_ids = "0";
                    if (data.sectionlistarray != null && data.sectionlistarray.Length > 0)
                    {
                        if (data.sectionlistarray.Length > 0)
                        {
                            foreach (var ue in data.sectionlistarray)
                            {
                                asms_ids = asms_ids + "," + ue.ASMS_Id;

                            }

                        }
                    }

                    string isms_ids = "0";
                    if (data.subjectlistarray != null && data.subjectlistarray.Length > 0)
                    {
                        if (data.subjectlistarray.Length > 0)
                        {
                            foreach (var ue in data.subjectlistarray)
                            {
                                isms_ids = isms_ids + "," + ue.ISMS_Id;

                            }

                        }
                    }

                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "Adm_SchoolSubjectwiseAbsentAttendanceSMS";

                    //    //Students_DaySubjectWiseAbsent_Details
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                    //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    //  cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                    //    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });

                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

                    //    var retObject = new List<dynamic>();

                    //    try
                    //    {
                    //        using (var dataReader = cmd.ExecuteReader())
                    //        {
                    //            while (dataReader.Read())
                    //            {
                    //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    //                {
                    //                    dataRow.Add(
                    //                        dataReader.GetName(iFiled),
                    //                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                }
                    //                retObject.Add((ExpandoObject)dataRow);
                    //            }
                    //        }
                    //        data.newarray = retObject.ToArray();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }
                    //}

                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{
                    //    cmd.CommandText = "Adm_School_AbsentSubjectwiseAttendance_Total";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                    //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    //    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                    //    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });

                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();

                    //    var retObject = new List<dynamic>();

                    //    try
                    //    {
                    //        using (var dataReader = cmd.ExecuteReader())
                    //        {
                    //            while (dataReader.Read())
                    //            {
                    //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    //                {
                    //                    dataRow.Add(
                    //                        dataReader.GetName(iFiled),
                    //                        dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                }
                    //                retObject.Add((ExpandoObject)dataRow);
                    //            }
                    //        }
                    //        data.newarray_total = retObject.ToArray();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //    }
                    //}



                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Adm_School_AbsentSubjectwiseAttendance_multiple";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = asmcl_ids });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = asms_ids });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = isms_ids });

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
                                            dataReader.IsDBNull(iFiled) ? "Not Available" : dataReader[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.newarray_total = retObject.ToArray();
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

        public StudentAttendanceReportDTO OnsendSMS(StudentAttendanceReportDTO data)
        {
            try
            {
                var acd_Id = _db.academicYear.Where(t => t.MI_Id.Equals(data.miid) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var acd_name = _db.academicYear.Where(t => t.ASMAY_Id == acd_Id).Select(d => d.ASMAY_Year).FirstOrDefault();

                data.ASMAY_Id = acd_Id;


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                //DateTime fromdatecon = DateTime.Now;
                string confromdate = "";
                //try
                //{
                //indianTime = Convert.ToDateTime(indianTime.Date.ToString("yyyy-MM-dd"));
                //confromdate = indianTime.ToString("yyyy-MM-dd");
                DateTime fromdatecon = DateTime.Now;

                fromdatecon = Convert.ToDateTime(data.fromdate.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdatecon.ToString("yyyy-MM-dd");

                DateTime todatecon = DateTime.Now;
                string tofromdate = "";
                todatecon = Convert.ToDateTime(data.todate.Date.ToString("yyyy-MM-dd"));
                tofromdate = todatecon.ToString("yyyy-MM-dd");
                //}

                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //}

                string amst_ids = "0";
                if (data.AbsentSMS != null && data.AbsentSMS.Length > 0)
                {
                    if (data.AbsentSMS.Length > 0)
                    {
                        foreach (var ue in data.AbsentSMS)
                        {
                            amst_ids = amst_ids + "," + ue.AMST_Id;
                            // asmsid = asmsid + "," + ue.ASMS_Id;
                        }

                    }
                }


                string asmcl_ids = "0";
                if (data.classlsttwo != null && data.classlsttwo.Length > 0)
                {
                    if (data.classlsttwo.Length > 0)
                    {
                        foreach (var ue in data.classlsttwo)
                        {
                            asmcl_ids = asmcl_ids + "," + ue.ASMCL_Id;
                         
                        }

                    }


                }


                string asms_ids = "0";
                if (data.sectionlistarray != null && data.sectionlistarray.Length > 0)
                {
                    if (data.sectionlistarray.Length > 0)
                    {
                        foreach (var ue in data.sectionlistarray)
                        {
                            asms_ids = asms_ids + "," + ue.ASMS_Id;
                           
                        }

                    }
                }

                string isms_ids = "0";
                if (data.subjectlistarray != null && data.subjectlistarray.Length > 0)
                {
                    if (data.subjectlistarray.Length > 0)
                    {
                        foreach (var ue in data.subjectlistarray)
                        {
                            isms_ids = isms_ids + "," + ue.ISMS_Id;
                            
                        }

                    }
                }


                List<Absent_Student_AbsentList> absentlist = new List<Absent_Student_AbsentList>();

                //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //{
                //  //  _acdimpl.LogInformation("entered cmd getdbconnection");
                //    cmd.CommandText = "Adm_School_SubjectwiseAttendanceSMS_multiple";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = asmcl_ids });
                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = asms_ids });
                //    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                //    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                //    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = isms_ids });
                //   cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amst_ids });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();
                //   // _acdimpl.LogInformation("entered if block");

                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            _acdimpl.LogInformation("entered in dataReader block");
                //            while (dataReader.Read())
                //            {
                //                absentlist.Add(new Absent_Student_List
                //                {
                //                    Amst_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                //                    MobileNo = Convert.ToInt64(dataReader["AMST_MobileNo"]),
                //                });
                //            }
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        _acdimpl.LogInformation("error:'" + ex.Message + "'");
                //        Console.Write(ex.Message);
                //    }
                //}

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    //  _acdimpl.LogInformation("entered cmd getdbconnection");
                    cmd.CommandText = "Adm_School_SubjectwiseAttendanceSMS_bkp";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.miid });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = asmcl_ids });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = asms_ids });
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = tofromdate });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = isms_ids });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amst_ids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    // _acdimpl.LogInformation("entered if block");

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            _acdimpl.LogInformation("entered in dataReader block");
                            while (dataReader.Read())
                            {
                                absentlist.Add(new Absent_Student_AbsentList
                                {
                                    Amst_Id = Convert.ToInt64(dataReader["AMST_Id"]),
                                    MobileNo = Convert.ToInt64(dataReader["AMST_MobileNo"]),
                                    ISMS_Id= Convert.ToInt64(dataReader["ISMS_Id"]),
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


                // Dictionary<List, decimal?> amount = new Dictionary<List, decimal?>();

                for (int k = 0; k < absentlist.Count; k++)
                {
                    try
                    {
                        var admConfig = _db.AdmissionStandardDMO.Single(t => t.MI_Id == data.miid);
                        var studDet = _db.admissionStduent.Where(t => t.MI_Id == data.miid && t.AMST_Id == absentlist[k].Amst_Id).ToList();


                        var template = _db.SMSEmailSetting.Where(e => e.MI_Id == data.miid && e.ISES_Template_Name == "StudentAbsentSMS").ToList();

                        // ----- SMS ------//
                        if (template.FirstOrDefault().ISES_SMSActiveFlag == true)
                        {
                            if (admConfig.ASC_DefaultSMS_Flag == "M" && studDet.FirstOrDefault().AMST_MotherMobileNo != null)
                            {
                                try
                                {
                                    SMS sms = new SMS(_admdb);
                                    string s = sms.sendAbsentSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_MotherMobileNo), "StudentAbsentSMS", absentlist[k].Amst_Id, absentlist[k].ISMS_Id, asmcl_ids, asms_ids, data.ASMAY_Id, data.todate,data.fromdate,data.miid).Result;
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
                                   
                                    SMS sms = new SMS(_admdb);
                                    string s = sms.sendAbsentSms(data.miid, Convert.ToInt64(studDet.FirstOrDefault().AMST_FatherMobleNo), "StudentAbsentSMS", absentlist[k].Amst_Id, absentlist[k].ISMS_Id, asmcl_ids, asms_ids, data.ASMAY_Id, data.todate, data.fromdate, data.miid).Result;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    _acdimpl.LogInformation("Sendsmsabsent attendance entry mobile F" + ex.Message);
                                    data.return_msg = "admin";
                                }
                            }
                            else
                            {
                                try
                                {
                                    if (studDet.FirstOrDefault().AMST_MobileNo.ToString() != null)
                                    {
                                        SMS sms = new SMS(_admdb);
                                        string s = sms.sendAbsentSms(data.miid, studDet.FirstOrDefault().AMST_MobileNo, "StudentAbsentSMS", absentlist[k].Amst_Id, absentlist[k].ISMS_Id, asmcl_ids,asms_ids, data.ASMAY_Id, data.todate, data.fromdate, data.miid).Result;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    _acdimpl.LogInformation("Sendsmsabsent attendance entry mobile S" + ex.Message);
                                    data.return_msg = "admin";
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
                                    Email Email = new Email(_admdb);
                                    string s = Email.sendmail(data.miid, studDet.FirstOrDefault().AMST_MotherEmailId, "Student_Absent_SMS", absentlist[k].Amst_Id);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    _acdimpl.LogInformation("Sendsmsabsent attendance entry email M" + ex.Message);
                                    data.return_msg = "admin";
                                }
                            }
                            else if (admConfig.ASC_DefaultSMS_Flag == "F" && studDet.FirstOrDefault().AMST_FatheremailId != null
                                && studDet.FirstOrDefault().AMST_FatheremailId != "")
                            {
                                try
                                {
                                    Email Email = new Email(_admdb);
                                    string s = Email.sendmail(data.miid, studDet.FirstOrDefault().AMST_FatheremailId, "Student_Absent_SMS", absentlist[k].Amst_Id);
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
                                        Email Email = new Email(_admdb);
                                        string s = Email.sendmail(data.miid, studDet.FirstOrDefault().AMST_emailId, "Student_Absent_SMS", absentlist[k].Amst_Id);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    _acdimpl.LogInformation("Sendsmsabsent attendance entry email S" + ex.Message);
                                    data.return_msg = "admin";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _acdimpl.LogInformation("Sendsmsabsent attendance entry New" + ex.Message);
                        data.return_msg = "admin";
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogInformation("Sendsmsabsent attendance entry" + ex.Message);
                data.return_msg = "admin";
            }
            return data;
        }


        public StudentAttendanceReportDTO OnChangeSectionAbsent(StudentAttendanceReportDTO data)
        {
            try
            {
                var getuserid = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();

               

                List<long> asmclids = new List<long>();
                foreach (var item in data.classlsttwo)
                {
                    // INTBC_Ids.Add(item.INTBC_Id);
                    asmclids.Add(item.ASMCL_Id);
                }

                List<long> asmsids = new List<long>();
                foreach (var item in data.sectionlistarray)
                {
                    // INTBC_Ids.Add(item.INTBC_Id);
                    asmsids.Add(item.ASMS_Id);
                }

                if (getuserid.Count > 0)
                {
                    //data.subjectlist = (from a in _db.Adm_SchAttLoginUser
                    //                    from b in _db.Adm_SchAttLoginUserClass
                    //                    from c in _db.admissionClass
                    //                    from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                    //                    from e in _db.masterSection
                    //                    from f in _db.IVRM_Master_SubjectsDMO
                    //                    where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && b.ASALUC_Id == d.ASALUC_Id
                    //                    && d.ISMS_Id == f.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.miid && b.ASMCL_Id == data.ASMCL_Id
                    //                    && a.HRME_Id == getuserid.FirstOrDefault().Emp_Code && b.ASMS_Id == data.ASMS_Id)
                    //                    select f).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

                    data.subjectlist = (from a in _db.Adm_SchAttLoginUser
                                        from b in _db.Adm_SchAttLoginUserClass
                                        from c in _db.admissionClass
                                        from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                        from e in _db.masterSection
                                        from f in _db.IVRM_Master_SubjectsDMO
                                        where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && b.ASALUC_Id == d.ASALUC_Id
                                        && d.ISMS_Id == f.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.miid && asmclids.Contains(b.ASMCL_Id)
                                        && a.HRME_Id == getuserid.FirstOrDefault().Emp_Code && asmsids.Contains(b.ASMS_Id))
                                        select new StudentAttendanceReportDTO
                                        {
                                            //ASMCL_Id=b.ASMCL_Id,
                                            //ASMS_Id=b.ASMS_Id,
                                            ISMS_Id=d.ISMS_Id,
                                            ISMS_SubjectName=f.ISMS_SubjectName
                                        }).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }
                else
                {
                    //data.subjectlist = (from a in _db.Adm_SchAttLoginUser
                    //                    from b in _db.Adm_SchAttLoginUserClass
                    //                    from c in _db.admissionClass
                    //                    from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                    //                    from e in _db.masterSection
                    //                    from f in _db.IVRM_Master_SubjectsDMO
                    //                    where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && b.ASALUC_Id == d.ASALUC_Id
                    //                    && d.ISMS_Id == f.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.miid && b.ASMCL_Id == data.ASMCL_Id
                    //                     && b.ASMS_Id == data.ASMS_Id)
                    //                    select f).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();


                    data.subjectlist = (from a in _db.Adm_SchAttLoginUser
                                        from b in _db.Adm_SchAttLoginUserClass
                                        from c in _db.admissionClass
                                        from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                        from e in _db.masterSection
                                        from f in _db.IVRM_Master_SubjectsDMO
                                        where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && b.ASALUC_Id == d.ASALUC_Id
                                        && d.ISMS_Id == f.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.miid && asmclids.Contains(b.ASMCL_Id)
                                         && asmsids.Contains(b.ASMS_Id))
                                        select new StudentAttendanceReportDTO
                                        {
                                            //ASMCL_Id = b.ASMCL_Id,
                                            //ASMS_Id = b.ASMS_Id,
                                            ISMS_Id = d.ISMS_Id,
                                            ISMS_SubjectName = f.ISMS_SubjectName
                                        }).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }

                //data.getstudentlist = (from e in _db.admissionStduent
                //                       from f in _db.admissionyearstudent
                //                       where (e.AMST_Id == f.AMST_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && f.ASMAY_Id == data.ASMAY_Id
                //                       && e.MI_Id == data.miid && (e.AMST_SOL != "Del" || e.AMST_SOL != "WD"))
                //                       select new StudentAttendanceReportDTO
                //                       {
                //                           AMST_Id = e.AMST_Id,
                //                           AMST_FirstName = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + " " + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + " " + (e.AMST_LastName == null ? " " : e.AMST_LastName) +
                //                           (e.AMST_AdmNo == null || e.AMST_AdmNo == "" ? "" : " : " + e.AMST_AdmNo)).Trim(),
                //                       }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();

                data.getstudentlist = (from e in _db.admissionStduent
                                       from f in _db.admissionyearstudent
                                       where (e.AMST_Id == f.AMST_Id && asmclids.Contains(f.ASMCL_Id) && asmsids.Contains(f.ASMS_Id) && f.ASMAY_Id == data.ASMAY_Id
                                       && e.MI_Id == data.miid && (e.AMST_SOL != "Del" || e.AMST_SOL != "WD"))
                                       select new StudentAttendanceReportDTO
                                       {
                                           AMST_Id = e.AMST_Id,
                                           AMST_FirstName = ((e.AMST_FirstName == null ? " " : e.AMST_FirstName) + " " + (e.AMST_MiddleName == null ? " " : e.AMST_MiddleName) + " " + (e.AMST_LastName == null ? " " : e.AMST_LastName) +
                                           (e.AMST_AdmNo == null || e.AMST_AdmNo == "" ? "" : " : " + e.AMST_AdmNo)).Trim(),
                                       }).Distinct().OrderBy(t => t.AMST_FirstName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentAttendanceReportDTO OnChangeClassAbsent(StudentAttendanceReportDTO data)
        {
            try
            {
                var getuserid = _db.Staff_User_Login.Where(a => a.Id == data.userId).ToList();

                List<long> asmclids = new List<long>();
                foreach (var item in data.classlsttwo)
                {
                    // INTBC_Ids.Add(item.INTBC_Id);
                    asmclids.Add(item.ASMCL_Id);
                }

                if (getuserid.Count > 0)
                {
                    data.SectionList = (from a in _db.Adm_SchAttLoginUser
                                        from b in _db.Adm_SchAttLoginUserClass
                                        from c in _db.admissionClass
                                        from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                        from e in _db.masterSection
                                        where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && b.ASALUC_Id == d.ASALUC_Id
                                        && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.miid && asmclids.Contains(b.ASMCL_Id) 
                                        && a.HRME_Id == getuserid.FirstOrDefault().Emp_Code)
                                        select e).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
                else
                {
                    data.SectionList = (from a in _db.Adm_SchAttLoginUser
                                        from b in _db.Adm_SchAttLoginUserClass
                                        from c in _db.admissionClass
                                        from d in _db.Adm_SchoolAttendanceLoginUserClassSubject
                                        from e in _db.masterSection
                                        where (a.ASALU_Id == b.ASALU_Id && b.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == b.ASMS_Id && b.ASALUC_Id == d.ASALUC_Id
                                        && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.miid && asmclids.Contains(b.ASMCL_Id))
                                        select new StudentAttendanceReportDTO
                                        {
                                            ASMC_SectionName=e.ASMC_SectionName,
                                           // ASMCL_className=c.ASMCL_ClassName,
                                            //ASMCL_Id=c.ASMCL_Id,
                                            ASMS_Id=e.ASMS_Id
                                        }).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
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