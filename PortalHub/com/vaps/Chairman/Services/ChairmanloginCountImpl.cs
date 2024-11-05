
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DataAccessMsSqlServerProvider.com.vapstech.COE;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class ChairmanloginCountImpl : Interfaces.ChairmanloginCountInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public ChairmanloginCountImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public ChairmanloginCountDTO Getdetails(ChairmanloginCountDTO data)//int IVRMM_Id
        {
           
            try
            {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CHAIRMAN_LOGIN_COUNT";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@USERId",
                          SqlDbType.BigInt)
                        {
                            Value = data.user_id
                        });
                       
                        cmd.Parameters.Add(new SqlParameter("@FRMDATE",
                         SqlDbType.DateTime)
                        {
                            Value = data.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@TODATE",
                         SqlDbType.DateTime)
                        {
                            Value = data.todate
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
                            data.fillgroupfee = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }

                       
                    }
               
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }



        public ChairmanloginCountDTO getstdappcount(ChairmanloginCountDTO data)
        {
         
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_CHAIRMAN_INSTITUTION";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERId",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
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
                        data.instarray = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }


                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHAIRMAN_LOGIN_COUNT_STUDENT_MOBILE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERId",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FRMDATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                    SqlDbType.Char)
                    {
                        Value = "L"
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
                        data.fillgroupfee = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }


                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHAIRMAN_LOGIN_COUNT_STUDENT_MOBILE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERId",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FRMDATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                    SqlDbType.Char)
                    {
                        Value = "D"
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
                        data.fillnewadmstd = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                    
                }

            }
            
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
          public ChairmanloginCountDTO getstaffappcount(ChairmanloginCountDTO data)
        {
         
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_CHAIRMAN_INSTITUTION";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERId",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
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
                        data.instarray = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }


                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHAIRMAN_LOGIN_COUNT_STAFF_MOBILE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERId",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FRMDATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                    SqlDbType.Char)
                    {
                        Value = "L"
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
                        data.fillgroupfee = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }


                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHAIRMAN_LOGIN_COUNT_STAFF_MOBILE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERId",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FRMDATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                    SqlDbType.Char)
                    {
                        Value = "D"
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
                        data.fillnewadmstd = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                    
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHAIRMAN_LOGIN_COUNT_STAFF_MOBILE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERId",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FRMDATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                    SqlDbType.Char)
                    {
                        Value = "R"
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
                        data.fillhead = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }

                }

            }
            
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }


        public ChairmanloginCountDTO GetpopupDetails(ChairmanloginCountDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CHAIRMAN_LOGIN_DETAILS_STAFF_MOBILE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@USERId",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FRMDATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TODATE",
                     SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                    SqlDbType.Char)
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
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.fillgroupfee = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }


                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }



    }
}
