using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;

namespace PortalHub.com.vaps.Student.Services
{
    public class FeeDetailsImpl : Interfaces.FeeDetailsInterface
    {
        private static ConcurrentDictionary<string, StudentDashboardDTO> _login =
           new ConcurrentDictionary<string, StudentDashboardDTO>();
        private PortalContext _Feecontext;
        public FeeDetailsImpl(PortalContext Feecontext)
        {
            _Feecontext = Feecontext;
        }

        public async Task<StudentDashboardDTO> getloaddata(StudentDashboardDTO data)
        {            
            try
            {
                using (var cmd = _Feecontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_FEE_ACADEMICYEAR_CLASS_SECTION";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                     SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                        data.yearclsList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.feecurrentyear = (from a in _Feecontext.School_Adm_Y_StudentDMO
                                    from b in _Feecontext.AcademicYearDMO
                                    where (b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.MI_Id == data.MI_Id)
                                    select new StudentDashboardDTO
                                    {
                                        ASMAY_Id = b.ASMAY_Id,
                                        ASMAY_Year = b.ASMAY_Year
                                    }
                            ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }         
            return data;
        }

        public async Task<StudentDashboardDTO> Getdetails(StudentDashboardDTO fddto)
        {         
            try
            {
                using (var cmd = _Feecontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_FEE_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                  SqlDbType.BigInt)
                    {
                        Value = fddto.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                    {
                        Value = fddto.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                    SqlDbType.BigInt)
                    {
                        Value = fddto.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = fddto.type
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
                        fddto.getfeedetails = retObject.ToArray();
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
            return fddto;
        }
    }
}
