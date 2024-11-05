using DataAccessMsSqlServerProvider.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class NonBookTransactionReportImpl:Interfaces.NonBookTransactionReportInterface
    {

        private static ConcurrentDictionary<string, NonBookReport_DTO> _login =
        new ConcurrentDictionary<string, NonBookReport_DTO>();

        private LibraryContext _LibraryContext;
        public NonBookTransactionReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

      
        public NonBookReport_DTO getdetails(NonBookReport_DTO id)
        {
            
            try
            {
                
                id.lib_list = (from a in _LibraryContext.LIB_Master_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO
                                   // from c in _LibraryContext.LIB_Library_Class_DMO
                               where a.MI_Id == b.MI_Id && a.MI_Id == id.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == id.IVRMUL_Id
                               select a).ToArray();
                var lib_list1 = (from a in _LibraryContext.LIB_Master_Library_DMO
                                 from b in _LibraryContext.LIB_User_Library_DMO
                                     // from c in _LibraryContext.LIB_Library_Class_DMO
                                 where a.MI_Id == b.MI_Id && a.MI_Id == id.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == id.IVRMUL_Id
                                 select a).ToList();

                if (lib_list1.Count > 0)
                {
                    id.classList = (from a in _LibraryContext.School_M_Class
                                    from c in _LibraryContext.LIB_Library_Class_DMO
                                    where (a.MI_Id == c.MI_Id && a.ASMCL_Id == c.ASMCL_Id && c.MI_Id == id.MI_Id && a.ASMCL_ActiveFlag == true && c.LLC_ActiveFlg == true && c.LMAL_Id == lib_list1.FirstOrDefault().LMAL_Id)
                                    select a).Distinct().ToArray();
                    id.LMAL_Id = lib_list1.FirstOrDefault().LMAL_Id;
                }
                
                id.sectionList = _LibraryContext.school_M_Section.Where(d => d.MI_Id == id.MI_Id && d.ASMC_ActiveFlag == 1).Select(d => new NonBookReport_DTO { ASMS_Id = d.ASMS_Id, sectionName = d.ASMC_SectionName }).ToArray();

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return id;
        }

        public async Task<NonBookReport_DTO> get_report(NonBookReport_DTO data)
        {
            try
            {
                string classs_ids = "0";
                string section_idss = "0";


                List<long> clss_ids = new List<long>();
                List<long> section_ids = new List<long>();


                if (data.AGType == "Student")
                {
                    foreach (var item in data.selectedClasslist)
                    {
                        clss_ids.Add(item.ASMCL_Id);
                    }
                    for (int s = 0; s < clss_ids.Count(); s++)
                    {
                        classs_ids = classs_ids + ',' + clss_ids[s].ToString();

                    }
                    foreach (var item in data.selectedSectionlist)
                    {
                        section_ids.Add(item.ASMS_Id);
                    }
                    for (int s = 0; s < section_ids.Count(); s++)
                    {
                        section_idss = section_idss + ',' + section_ids[s].ToString();
                    }

                }


                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lib_Non_Book_Transaction_Report";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });                   
                    cmd.Parameters.Add(new SqlParameter("@AGType",
                   SqlDbType.VarChar)
                    {
                        Value = data.AGType
                    });
                    cmd.Parameters.Add(new SqlParameter("@TrnType",
                   SqlDbType.VarChar)
                    {
                        Value = data.TrnType
                    });

                    cmd.Parameters.Add(new SqlParameter("@IssueFromDate",
               SqlDbType.VarChar)
                    {
                        Value = data.IssueFromDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@IssueToDate",
               SqlDbType.VarChar)
                    {
                        Value = data.IssueToDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@DueFromdate",
            SqlDbType.VarChar)
                    {
                        Value = data.DueFromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@DueTodate",
               SqlDbType.VarChar)
                    {
                        Value = data.DueTodate
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.LMAL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
         SqlDbType.VarChar)
                    {
                        Value = classs_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
            SqlDbType.VarChar)
                    {
                        Value = section_idss
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }


    }
}
