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
    public class LibTransactionReportImpl : Interfaces.LibTransactionReportInterface
    {
        private LibraryContext _LibraryContext;
        public LibTransactionReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }



        public LibTransactionReportDTO getdetails(LibTransactionReportDTO data)
        {
            try
            {
                //data.donorlist = _LibraryContext.MasterDonorDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
                data.yearlist = _LibraryContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active==true ).Distinct().OrderByDescending(t=>t.ASMAY_Order).ToArray();
                data.msterliblist1 = (from a in _LibraryContext.LIB_Master_Library_DMO
                                      from b in _LibraryContext.LIB_User_Library_DMO
                                          // from c in _LibraryContext.LIB_Library_Class_DMO
                                      where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                                      select a).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LibTransactionReportDTO get_report(LibTransactionReportDTO data)
        {
            try
            {
                if (data.statuscount==true)
                {
                    data.Type = "1";

                }
                else
                {

                    data.Type = "0";

                    data.classlist = _LibraryContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }

                List<LibTransactionReportDTO> result1 = new List<LibTransactionReportDTO>();

                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_SECTIONWISE_BOOK_ISSUE_COUNT";

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
                    cmd.Parameters.Add(new SqlParameter("@FROM_DATE",
                               SqlDbType.Date)
                    {
                        Value = data.Issue_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@TO_DATE",
                               SqlDbType.Date)
                    {
                        Value = data.IssueToDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                              SqlDbType.VarChar)
                    {
                        Value = data.Type
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
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
            return data;
        }


        public LibTransactionReportDTO CLGget_report(LibTransactionReportDTO data)
        {
            try
            {
                if (data.statuscount == true)
                {
                    data.Type = "1";

                }
                else
                {

                    data.Type = "0";

                    data.classlist = _LibraryContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                }

                List<LibTransactionReportDTO> result1 = new List<LibTransactionReportDTO>();

                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_SECTIONWISE_BOOK_ISSUE_COUNT_COLLEGE";

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
                    cmd.Parameters.Add(new SqlParameter("@FROM_DATE",
                               SqlDbType.Date)
                    {
                        Value = data.Issue_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@TO_DATE",
                               SqlDbType.Date)
                    {
                        Value = data.IssueToDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                              SqlDbType.VarChar)
                    {
                        Value = data.Type
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
                        data.alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
