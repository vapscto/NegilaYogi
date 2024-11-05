using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;

namespace CollegeServiceHub.Impl
{
    public class CollegeTpinGenerationImpl :Interface.CollegeTpinGenerationInterface
    {
        public ClgAdmissionContext _context;
        ILogger<CollegeTpinGenerationImpl> _log;
        public CollegeTpinGenerationImpl(ClgAdmissionContext _cont, ILogger<CollegeTpinGenerationImpl> _logg)
        {
            _context = _cont;
            _log = _logg;
        }

        public CollegeTpinGenerationDTO loaddata(CollegeTpinGenerationDTO data)
        {
            try
            {
                data.getyearlist = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Tpin Load Data  :" + ex.Message);
            }
            return data;
        }
        public CollegeTpinGenerationDTO search(CollegeTpinGenerationDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Admission_Get_Tpin_Student_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = 1
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
                        data.gettpinnotgeneratedliststudent = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _log.LogInformation("Error In Search Tpin List Not Generate  :" + ex.Message);
                    }
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Admission_Get_Tpin_Student_List";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.BigInt)
                    {
                        Value = 2
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
                        data.gettpingeneratedliststudent = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _log.LogInformation("Error In Search Tpin List Generated  :" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("search Tpin Load Data  :" + ex.Message);
            }
            return data;
        }
        public CollegeTpinGenerationDTO generatetpin(CollegeTpinGenerationDTO data)
        {
            try
            {
                var outputval = _context.Database.ExecuteSqlCommand("College_Admission_Generate_Tpin  @p0,@p1", data.MI_Id, 0);
                if (outputval > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("generatetpin Tpin Load Data  :" + ex.Message);
                data.returnval = false;
            }
            return data;
        }
    }
}
