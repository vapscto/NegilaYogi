using DataAccessMsSqlServerProvider.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class BookArrivalReportImpl:Interfaces.BookArrivalReportInterface
    {
        private LibraryContext _LibraryContext;
        public BookArrivalReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }
        public BookArrivalReportDTO getdetails(BookArrivalReportDTO id)
        {
            
            try
            {
                //data.donorlist = _LibraryContext.MasterDonorDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
                id.deptlist = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == id.MI_Id && t.LMD_ActiveFlg==true).Distinct().ToArray();

                id.lib_list = (from a in _LibraryContext.LIB_Master_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO
                                   // from c in _LibraryContext.LIB_Library_Class_DMO
                               where a.MI_Id == b.MI_Id && a.MI_Id == id.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == id.IVRMUL_Id
                               select a).ToArray();
                id.griddata = _LibraryContext.MasterSubject_DMO.Where(R => R.MI_Id == id.MI_Id).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return id;
        }
        public async Task<BookArrivalReportDTO> get_report(BookArrivalReportDTO data)
        {
            try
            {
                if (data.LMD_Id == "0")
                {
                    data.LMD_Id = "ALL";
                }

                List<BookArrivalReportDTO> result1 = new List<BookArrivalReportDTO>();
                if(data.BookArrival==true)
                {
                    string LMS_Id = "0";
                    if(data.BookSummary !=null && data.BookSummary.Length > 0)
                    {
                        foreach(var d  in data.BookSummary)
                        {
                            LMS_Id = LMS_Id + ',' + d.LMS_Id;

                        }
                    }
                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Lib_Book_Arrival_Report_Stthomos";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 8000000;

                        cmd.Parameters.Add(new SqlParameter("@Type",
                        SqlDbType.VarChar)
                        {
                            Value = data.Type
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type2",
                      SqlDbType.VarChar)
                        {
                            Value = data.Type2
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMD_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.LMD_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate",
                      SqlDbType.VarChar)
                        {
                            Value = data.Fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                     SqlDbType.VarChar)
                        {
                            Value = data.ToDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.LMAL_Id
                        });
                        //LMS_Id
                        cmd.Parameters.Add(new SqlParameter("@LMS_Id",
                      SqlDbType.VarChar)
                        {
                            Value = LMS_Id
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
                            data.reportlist = retObject.ToArray();
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
                        cmd.CommandText = "Lib_Book_Arrival_Report_NewOne";

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 8000000;

                        cmd.Parameters.Add(new SqlParameter("@Type",
                        SqlDbType.VarChar)
                        {
                            Value = data.Type
                        });
                        cmd.Parameters.Add(new SqlParameter("@Type2",
                      SqlDbType.VarChar)
                        {
                            Value = data.Type2
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMD_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.LMD_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate",
                      SqlDbType.VarChar)
                        {
                            Value = data.Fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                     SqlDbType.VarChar)
                        {
                            Value = data.ToDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.LMAL_Id
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
                            data.reportlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                
            }
            catch(Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
    }
}
