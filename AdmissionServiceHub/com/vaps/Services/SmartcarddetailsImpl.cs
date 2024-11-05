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
    public class SmartcarddetailsImpl : Interfaces.SmartcarddetailsInterface
    {
        public StudentAttendanceReportContext _db;


        public SmartcarddetailsImpl(StudentAttendanceReportContext db)
        {
            _db = db;
        }

        public async Task<Adm_M_StudentDTO> getInitailData(int mi_id)
        {
            Adm_M_StudentDTO ctdo = new Adm_M_StudentDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _db.academicYear.Where(t => t.MI_Id == mi_id && t.Is_Active == true).ToListAsync();
                ctdo.academicList = allyear.OrderByDescending(a => a.ASMAY_Order).ToArray();

                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = await _db.admissionClass.Where(t => t.MI_Id == mi_id && t.ASMCL_ActiveFlag == true).ToListAsync();
                ctdo.classlist = allclass.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> allsection = new List<School_M_Section>();
                allsection = await _db.masterSection.Where(t => t.MI_Id == mi_id && t.ASMC_ActiveFlag == 1).ToListAsync();
                ctdo.SectionList = allsection.OrderBy(s => s.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }

        public async Task<Adm_M_StudentDTO> getserdata(Adm_M_StudentDTO data)
        {
            using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd1.CommandText = "Smart_Card_Details";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
                 SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.ASMAY_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@ASMCL_ID",
                SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.ASMCL_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@ASMS_Id",
                SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.ASMC_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@mi_id",
               SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.MI_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@stdmobilenumber",
               SqlDbType.VarChar)
                {
                    Value = data.stdmobilenumber
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
                    data.getcarddetails = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return data;
            }
        }

        public async Task<Adm_M_StudentDTO> getstudentdetails(Adm_M_StudentDTO data)
        {
            using (var cmd1 = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd1.CommandText = "Classwise_Students";
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add(new SqlParameter("@YEAR",
                 SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.ASMAY_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@CLASS",
                SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.ASMCL_Id)
                });
                cmd1.Parameters.Add(new SqlParameter("@MI_ID",
               SqlDbType.BigInt)
                {
                    Value = Convert.ToInt64(data.MI_Id)
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
                    data.StudentList1 = retObject.ToArray();
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
