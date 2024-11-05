using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class LibraryMonthEndReportImpl : Interfaces.LibraryMonthEndReportInterface
    {
        public LibraryContext _LibraryContext;
        public LibraryMonthEndReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public LibraryMonthEndReportDTO Savedata(LibraryMonthEndReportDTO data)
        {
            try
            {
                if (data.LMRA_Id > 0)
                {
                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_LIB_MONTH_END_REPORT_New";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 8000000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                            SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@frmdate",
                                    SqlDbType.VarChar)
                        {
                            Value = data.year
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                                    SqlDbType.VarChar)
                        {
                            Value = data.IVRM_Month_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMRA_Id",
                                   SqlDbType.BigInt)
                        {
                            Value = data.LMRA_Id
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
                else
                {
                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GET_LIB_MONTH_END_REPORT";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 8000000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                            SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@frmdate",
                                    SqlDbType.VarChar)
                        {
                            Value = data.year
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                                    SqlDbType.VarChar)
                        {
                            Value = data.IVRM_Month_Id
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

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LibraryMonthEndReportDTO getdetails(int id)
        {
            LibraryMonthEndReportDTO data = new LibraryMonthEndReportDTO();
            try
            {
                var list = _LibraryContext.AcademicYearDMO.Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.monthlist = _LibraryContext.MonthDMO.Where(a => a.Is_Active == true).ToArray();
                data.alldata = _LibraryContext.LIB_Master_Library_DMO.Where(R => R.MI_Id == id && R.LMAL_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



    }
}
