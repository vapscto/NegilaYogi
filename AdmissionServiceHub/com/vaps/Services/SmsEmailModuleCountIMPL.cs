using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class SmsEmailModuleCountIMPL : Interfaces.SmsEmailModuleCountInterface
    {
        private static ConcurrentDictionary<string, SmsEmailModuleCountDTO> _login =
          new ConcurrentDictionary<string, SmsEmailModuleCountDTO>();

        public DomainModelMsSqlServerContext _db;

        public SmsEmailModuleCountIMPL(DomainModelMsSqlServerContext DomainModelMsSqlServerContext)
        {
            _db = DomainModelMsSqlServerContext;
        }
        public SmsEmailModuleCountDTO getdetails(SmsEmailModuleCountDTO data)
        {
            try
            {
                data.acayear = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).Distinct().ToArray();
                data.fillmonth = _db.month.Where(t => t.Is_Active == true).Distinct().ToArray();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ClgEmailSmsCount";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                        data.Modulelist = retObject.ToArray();

                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public SmsEmailModuleCountDTO Getreportdetails(SmsEmailModuleCountDTO data)
        {
            try
            {
                //List<long> Month = new List<long>();

                //if (data.rptmonth != null)
                //{
                //    foreach (var i in data.rptmonth)
                //    {
                //        Month.Add(i.ivrM_Month_Id);
                //    }
                //}
                string Month = "0";
                if (data.rptmonth.Length > 0)
                {
                    foreach (var ue in data.rptmonth)
                    {
                        Month = Month + "," + ue.ivrM_Month_Id;
                    }
                }
                if (data.radioption == "Consolited")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMS_EMAIL_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Month",
                  SqlDbType.VarChar)
                        {
                            Value = Month
                        });
                //        cmd.Parameters.Add(new SqlParameter("@optionflag",
                //SqlDbType.VarChar)
                //        {
                //            Value = data.radioption
                //        });
                        cmd.Parameters.Add(new SqlParameter("@year",
               SqlDbType.BigInt)
                        {
                            Value = data.year
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
                            data.Emailcount = retObject.ToArray();

                        }


                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "EmailSmsCountConstwo";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Month",
                  SqlDbType.VarChar)
                        {
                            Value = Month
                        });
                        cmd.Parameters.Add(new SqlParameter("@optionflag",
                SqlDbType.VarChar)
                        {
                            Value = data.radioption
                        });
                        cmd.Parameters.Add(new SqlParameter("@year",
               SqlDbType.VarChar)
                        {
                            Value = data.year
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
                            data.Smscount = retObject.ToArray();

                        }


                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                }
                else if (data.radioption == "Detailed")
                {

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
