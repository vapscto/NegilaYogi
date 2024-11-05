using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class ClientWise_Module_Feature_Impl : Interfaces.ClientWise_Module_Feature_Interface
    {
        public DomainModelMsSqlServerContext _db;
        public ClientWise_Module_Feature_Impl(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        public ClientWise_Module_Feature_DTO getmodule(ClientWise_Module_Feature_DTO dto)
        {
            try
            {
                dto.modulelist = (from a in _db.Institution_Module
                                  from b in _db.masterModule
                                  where a.IVRMM_Id == b.IVRMM_Id
                                  && a.MI_Id == dto.MI_Id
                                  && b.Module_ActiveFlag == 1 && a.IVRMIM_Flag == 1
                                  select new ClientWise_Module_Feature_DTO
                                  {
                                      IVRMM_Id = a.IVRMM_Id,
                                      IVRMM_ModuleName = b.IVRMM_ModuleName,
                                      IVRMM_Flag = b.IVRMM_Flag
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

        public ClientWise_Module_Feature_DTO getreport(ClientWise_Module_Feature_DTO dto)
        {
            try
            {
                //var sts = _db.masterModule.Where(a => a.IVRMM_Flag == dto.moduleflage).ToList();
                //dto.getmodulename = sts.ToArray();
                if (dto.moduleflage == "Birthday")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_BirthDay_YearlyMonthlyCount";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                SqlDbType.VarChar)
                        {
                            Value = dto.Type
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                SqlDbType.VarChar)
                        {
                            Value = dto.Flag
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
                            dto.getreportdetails = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else if (dto.moduleflage == "COE")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_COE_YearlyMonthlyCount";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                SqlDbType.VarChar)
                        {
                            Value = dto.Type
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                SqlDbType.VarChar)
                        {
                            Value = dto.Flag
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
                            dto.getreportdetails = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else if (dto.moduleflage == "VM")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_VM_YearlyMonthlyCount";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                 SqlDbType.VarChar)
                        {
                            Value = dto.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                SqlDbType.VarChar)
                        {
                            Value = dto.todate
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
                            dto.getreportdetails = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else if (dto.moduleflage == "Admission")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "School_Admission_CountDetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                SqlDbType.VarChar)
                        {
                            Value = dto.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                SqlDbType.VarChar)
                        {
                            Value = dto.todate
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
                            dto.getreportdetails = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else if (dto.moduleflage == "Library")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_LIB_YearlyMonthlyCount";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                SqlDbType.VarChar)
                        {
                            Value = dto.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                SqlDbType.VarChar)
                        {
                            Value = dto.todate
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
                            dto.getreportdetails = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else if (dto.moduleflage == "FO")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_FO_YearlyMonthlyCount";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type",
                SqlDbType.VarChar)
                        {
                            Value = dto.Type
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                SqlDbType.VarChar)
                        {
                            Value = dto.Flag
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
                            dto.getreportdetails = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else if (dto.moduleflage == "Preadmission")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "School_PreAdmission_CountDetails";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                SqlDbType.VarChar)
                        {
                            Value = dto.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                SqlDbType.VarChar)
                        {
                            Value = dto.todate
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
                            dto.getreportdetails = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                else if (dto.moduleflage == "Exam")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_Exam_YearlyReport";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                        {
                            Value = dto.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                SqlDbType.VarChar)
                        {
                            Value = dto.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                SqlDbType.VarChar)
                        {
                            Value = dto.todate
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
                            dto.getreportdetails = retObject.ToArray();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
    }
}
