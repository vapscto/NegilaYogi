using DataAccessMsSqlServerProvider.com.vapstech.TT;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.TT;
using DomainModel.Model.com.vapstech.TT;
using DataAccessMsSqlServerProvider;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Dynamic;

namespace PortalHub.com.vaps.Principal.Services
{
    public class SmsEmailDetailsImpl : Interfaces.SmsEmailDetailsInterface
    {
        public TTContext _tt;
        public DomainModelMsSqlServerContext _db;

        public SmsEmailDetailsImpl(TTContext tt, DomainModelMsSqlServerContext db)
        {
            _tt = tt;
            _db = db;
        }
        public SmsEmailDetailsDTO getdata(SmsEmailDetailsDTO data)
        {
            try
            {
                data.monthlist = _db.month.Where(t => t.Is_Active == true).ToArray();



                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_SMS_EMAIL_MODULE_LIST";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.smsmodulelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public SmsEmailDetailsDTO Getreportdetails(SmsEmailDetailsDTO data)
        {
               try
                {

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ALL_SMS_EMAIL_DETAILS";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 8000000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@frmdate",
                                    SqlDbType.Date)
                        {
                            Value = data.frmdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                                    SqlDbType.Date)
                        {
                            Value = data.todate
                        });

                        cmd.Parameters.Add(new SqlParameter("@type",
                                    SqlDbType.VarChar)
                        {
                            Value = data.type
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                                    SqlDbType.VarChar)
                        {
                            Value = data.templete
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
                            data.griddata = retObject.ToArray();
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
        public SmsEmailDetailsDTO Getreportdetails1(SmsEmailDetailsDTO data)
        {
            try
            {

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "MODULEWISESMS_EMAIL_SENT_DETAILS";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@frmdate",
                                SqlDbType.Date)
                    {
                        Value = data.frmdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                                SqlDbType.Date)
                    {
                        Value = data.todate
                    });

                    cmd.Parameters.Add(new SqlParameter("@type",
                                SqlDbType.VarChar)
                    {
                        Value = data.type
                    });
                    cmd.Parameters.Add(new SqlParameter("@template",
                                SqlDbType.VarChar)
                    {
                        Value = data.templete
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
                        data.griddata = retObject.ToArray();
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
