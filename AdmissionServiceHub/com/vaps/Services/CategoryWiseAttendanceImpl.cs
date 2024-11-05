using DataAccessMsSqlServerProvider;
using DomainModel.Model;
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
    public class CategoryWiseAttendanceImpl : Interfaces.CategoryWiseAttendanceInterface
    {
        public StudentAttendanceReportContext _db;

        public CategoryWiseAttendanceImpl(StudentAttendanceReportContext db)
        {
            _db = db;
        }
        public async Task<StudentAttendanceReportDTO> getInitailData(int mi_id)
        {
            StudentAttendanceReportDTO ctdo = new StudentAttendanceReportDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(d=>d.MI_Id==mi_id && d.Is_Active==true).OrderByDescending(t => t.ASMAY_Order).ToListAsync();
                ctdo.academicList = allyear.ToArray();

                var cat = _db.GenConfig.Where(g => g.MI_Id == mi_id && g.IVRMGC_CatLogoFlg == true).ToList();
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

            if (data.categorylistarray == null || data.categorylistarray.Length > 0)
            {
                data.AMC_Id = 0;

            }
            var amcid = data.AMC_Id.ToString();

            data.AMC_logo = _db.category.Where(p => p.AMC_Id == data.AMC_Id && p.MI_Id == data.miid && p.AMC_ActiveFlag == 1).Select(p => p.AMC_FilePath).ToArray();


            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "CategoryWise_Attendance";
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
                //cmd.Parameters.Add(new SqlParameter("@AMC_Id", SqlDbType.VarChar)
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
    }
}
