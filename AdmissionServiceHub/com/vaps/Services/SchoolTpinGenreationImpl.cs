using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SchoolTpinGenreationImpl :Interfaces.SchoolTpinGenreationInterface
    {
        public AdmissionFormContext _context;
        ILogger<SchoolTpinGenreationImpl> _log;
        public SchoolTpinGenreationImpl(AdmissionFormContext _cont, ILogger<SchoolTpinGenreationImpl> _logg)
        {
            _context = _cont;
            _log = _logg;
        }

        public SchoolTpinGenreationDTO loaddata(SchoolTpinGenreationDTO data)
        {
            try
            {
                data.getyearlist = _context.year.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch(Exception ex)
            {
                _log.LogInformation("Tpin Load Data  :" + ex.Message);
            }
            return data;
        }
        public SchoolTpinGenreationDTO search(SchoolTpinGenreationDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_Get_Tpin_Student_List";
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
                    cmd.CommandText = "Admission_Get_Tpin_Student_List";
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
        public SchoolTpinGenreationDTO generatetpin(SchoolTpinGenreationDTO data)
        {
            try
            {
                var outputval = _context.Database.ExecuteSqlCommand("Admission_Generate_Tpin  @p0,@p1", data.MI_Id, 0);
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
