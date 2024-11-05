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
    public class AvailableBooksReportImpl:Interfaces.AvailableBooksReportInterface
    {
        private LibraryContext _LibraryContext;

        public AvailableBooksReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }


        public AvailableBooksReport_DTO getdetails(AvailableBooksReport_DTO data)
        {
          
            try
            {
                data.deptlist = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_ActiveFlg==true).Distinct().ToArray();
                data.msterliblist1 = (from a in _LibraryContext.LIB_Master_Library_DMO
                                      from b in _LibraryContext.LIB_User_Library_DMO
                                          // from c in _LibraryContext.LIB_Library_Class_DMO
                                      where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                                      select a).ToArray();
                data.griddata = _LibraryContext.MasterSubject_DMO.Where(R => R.MI_Id == data.MI_Id).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AvailableBooksReport_DTO get_report(AvailableBooksReport_DTO data)
        {
            try
            {
                if (data.statuscount==false)
                {
                    data.Fromdate = DateTime.Now;
                        data.ToDate= DateTime.Now;
                }

                List<AvailableBooksReport_DTO> result1 = new List<AvailableBooksReport_DTO>();
                if(data.subjectsthomos ==true)
                {
                    string LMS_Id = "0";
                    if(data.stthomosSubject !=null && data.stthomosSubject.Length > 0)
                    {
                        foreach(var d in data.stthomosSubject)
                        {
                            LMS_Id = LMS_Id + ',' + d.LMS_Id;
                        }
                    }
                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LIB_AVAILABLE_BOOK_REPORT_STthomos";

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
                      SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMD_Id",
                      SqlDbType.BigInt)
                        {
                            Value = data.LMD_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate",
                      SqlDbType.Date)
                        {
                            Value = data.Fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                     SqlDbType.Date)
                        {
                            Value = data.ToDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@statuscount",
                    SqlDbType.Bit)
                        {
                            Value = data.statuscount
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                     SqlDbType.BigInt)
                        {
                            Value = data.LMAL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMS_Id",
                     SqlDbType.VarChar)
                        {
                            Value = LMS_Id
                        });
                        //LMS_Id
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
                        cmd.CommandText = "LIB_AVAILABLE_BOOK_REPORT";

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
                      SqlDbType.Date)
                        {
                            Value = data.Fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                     SqlDbType.Date)
                        {
                            Value = data.ToDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@statuscount",
                    SqlDbType.Bit)
                        {
                            Value = data.statuscount
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                     SqlDbType.VarChar)
                        {
                            Value = data.LMAL_Id
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
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
    }
}
