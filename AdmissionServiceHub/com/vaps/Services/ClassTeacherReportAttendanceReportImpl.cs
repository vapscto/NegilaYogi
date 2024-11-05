using DataAccessMsSqlServerProvider.com.vapstech.admission;
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
    public class ClassTeacherReportAttendanceReportImpl : Interfaces.ClassTeacherReportAttendanceInterface
    {
        public ClassTeacherMappingContext _db;

        public ClassTeacherReportAttendanceReportImpl(ClassTeacherMappingContext db)
        {
            _db = db;
        }
        public ClassTeacherReportAttendanceDTO getdata(ClassTeacherReportAttendanceDTO data)
        {
            try
            {
                data.getyear = _db.year.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToArray();
                data.getyear1 = _db.year.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true && t.ASMAY_Id == data.ASMAY_Id).ToArray();

                var cat = _db.GenConfig.Where(g => g.MI_Id == data.MI_Id && g.IVRMGC_CatLogoFlg == true).ToList();
                if (cat.Count > 0)
                {
                    data.category_list = _db.category.Where(f => f.MI_Id == data.MI_Id && f.AMC_ActiveFlag == 1).ToArray();

                    //stu.category_list = (from m in _ActivateDeactivateContext.category
                    //                     from n in _ActivateDeactivateContext.masterclasscategory
                    //                                        where m.AMC_Id == n.AMC_Id && n.MI_Id == stu.MI_Id &&
                    //                                        n.Is_Active == true
                    //                                        select new MasterClassCategoryDTO
                    //                                        {
                    //                                            ASMCC_Id = n.ASMCC_Id,
                    //                                            className = m.AMC_Name
                    //                                        }).ToArray();
                    data.categoryflag = true;
                }
                else
                {
                    data.categoryflag = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ClassTeacherReportAttendanceDTO> getreport(ClassTeacherReportAttendanceDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                   // Class_Teacher_Attendance_Report
                    
                    cmd.CommandText = "Class_Teacher_Attendance_Report_category";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@year",
                    SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.Flag)
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
               SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMC_Id",
               SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt32(data.AMC_Id)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

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
                        data.SearchstudentDetails = retObject.ToArray();
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
