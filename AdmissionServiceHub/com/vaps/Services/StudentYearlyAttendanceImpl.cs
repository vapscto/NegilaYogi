using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
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
    public class StudentYearlyAttendanceImpl : Interfaces.StudentYearlyAttendanceInterface
    {
        public StudentAttendanceReportContext _db;


        public StudentYearlyAttendanceImpl(StudentAttendanceReportContext db)
        {
            _db = db;
        }
        //public async Task<StudentAttendanceReportDTO> getmonthtotalclassheld(StudentAttendanceReportDTO data)
        //{
        //    StudentAttendanceReportDTO monthheld = new StudentAttendanceReportDTO();
        //    try { 

        //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "ATTENDANCE_MONTH_NAME_FROM_START_AND_END_DATE";
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            if (cmd.Connection.State != ConnectionState.Open)
        //                cmd.Connection.Open();

        //            var retObject = new List<dynamic>();


        //            using (var dataReader = await cmd.ExecuteReaderAsync())
        //            {
        //                while (await dataReader.ReadAsync())
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
        //            monthheld.monthList = retObject.ToArray();
        //        }
        //    }

        //    catch (Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //        }

        //    return monthheld;
        //}
        public async Task<StudentAttendanceReportDTO> getInitailData(int mi_id)
        {
            StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(t => t.MI_Id == mi_id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToListAsync();
                ctdo.academicList = allyear.ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = await _db.admissionClass.Where(t => t.MI_Id == mi_id && t.ASMCL_ActiveFlag == true).OrderBy(c => c.ASMCL_Order).ToListAsync();
                ctdo.classlist = allclass.ToArray();

                List<School_M_Section> allsection = new List<School_M_Section>();
                allsection = await _db.masterSection.Where(t => t.MI_Id == mi_id && t.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToListAsync();
                ctdo.SectionList = allsection.ToArray();

                var cat = _db.GenConfig.Where(g => g.MI_Id == mi_id && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {


                    ctdo.category_list = _db.category.Where(f => f.MI_Id == mi_id && f.AMC_ActiveFlag == 1).ToArray();
                    ctdo.categoryflag = true;
                }
                else
                {
                    ctdo.categoryflag = false;
                }
            }

            //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            //    {
            //        cmd.CommandText = "ATTENDANCE_MONTH_NAME_FROM_START_AND_END_DATE";
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        if (cmd.Connection.State != ConnectionState.Open)
            //            cmd.Connection.Open();

            //        var retObject = new List<dynamic>();


            //        using (var dataReader = await cmd.ExecuteReaderAsync())
            //        {
            //            while (await dataReader.ReadAsync())
            //            {
            //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
            //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
            //                {
            //                    dataRow.Add(
            //                        dataReader.GetName(iFiled),
            //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
            //                    );
            //                }

            //                retObject.Add((ExpandoObject)dataRow);
            //            }
            //        }
            //        ctdo.monthList = retObject.ToArray();
            //    }
            //}

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public async Task<StudentAttendanceReportDTO> getserdata(StudentAttendanceReportDTO data)
        {
            var amcid = "0";
            
            if(data.AMC_Id>0)
            {
                 amcid = data.AMC_Id.ToString();

                data.AMC_logo = _db.category.Where(p => p.AMC_Id == data.AMC_Id && p.MI_Id == data.miid && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();

            }

            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "ADM_STUDENT_YEARLY_ATTENDANCE_NEW_HEADBINDING";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@asmay_id",
                 SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMAY_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@asmcl_id",
                SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMCL_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@asms_id",
                SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMC_Id)
                });
                cmd.Parameters.Add(new SqlParameter("@mi_id",
               SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.miid)
                });
                cmd.Parameters.Add(new SqlParameter("@flag",
                    SqlDbType.VarChar)
                {
                    Value = data.allorindiflag
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
                    data.monthList = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
            {
                //ADM_STUDENT_YEARLY_ATTENDANCE_NEW
                
                cmd1.CommandText = "ADM_STUDENT_YEARLY_ATTENDANCE_NEW_CATEGORY";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add(new SqlParameter("@asmay_id",
                 SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMAY_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@asmcl_id",
                SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMCL_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@asms_id",
                SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.ASMC_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@mi_id",
             SqlDbType.VarChar)
                {
                    Value = Convert.ToInt64(data.miid)
                });
                cmd1.Parameters.Add(new SqlParameter("@flag",
                    SqlDbType.VarChar)
                {
                    Value = data.allorindiflag
                });

                cmd1.Parameters.Add(new SqlParameter("@AMC_Id",
                   SqlDbType.VarChar)
                {
                    Value = data.AMC_Id
                });


                if (cmd1.Connection.State != ConnectionState.Open)
                    cmd1.Connection.Open();

                var retObject = new List<dynamic>();

                try
                {
                    using (var dataReader = await cmd1.ExecuteReaderAsync())
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
                    Console.WriteLine(ex.Message);
                }
                return data;
            }
        }


        public StudentAttendanceReportDTO getclass(StudentAttendanceReportDTO data)
        {
            try
            {

                //var check_rolename = (from a in _db.MasterRoleType
                //                      where (a.IVRMRT_Id == data.roleId)
                //                      select new StudentAttendanceReportDTO
                //                      {
                //                          rolename = a.IVRMRT_Role,
                //                      }).ToList();

                //int UserId = _db.ApplicationUser.Where(a => a.UserName == data.username).Select(a => a.Id).FirstOrDefault();

                //var empcode_check = (from a in _db.Staff_User_Login
                //                     where (a.MI_Id == data.miid && a.Id.Equals(UserId))
                //                     select new StudentAttendanceReportDTO
                //                     {
                //                         Emp_Code = a.Emp_Code,
                //                     }).ToList();
                //long? emp_id = 0;
                //if (check_rolename.FirstOrDefault().rolename.ToUpper().Equals("STAFF"))
                //{
                //    if (empcode_check.Count > 0)
                //    {
                //        emp_id = empcode_check.FirstOrDefault().Emp_Code;
                //    }

                //}

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    //AlumnistudentsearchReport_new
                    //AlumnistudentsearchReport
                    cmd.CommandText = "Total_strength_class";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.BigInt) { Value = data.AMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.miid });
                    cmd.Parameters.Add(new SqlParameter("@Emp_Id", SqlDbType.BigInt) { Value = 0 });

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
    }
}
