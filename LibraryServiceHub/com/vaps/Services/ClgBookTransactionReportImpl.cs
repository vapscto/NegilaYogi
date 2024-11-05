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
    public class ClgBookTransactionReportImpl : Interfaces.ClgBookTransactionReportInterface
    {
        private static ConcurrentDictionary<string, CLGBookTransactionDTO> _login = new ConcurrentDictionary<string, CLGBookTransactionDTO>();

        private LibraryContext _LibraryContext;
        public ClgBookTransactionReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }


        public CLGBookTransactionDTO getdetails(CLGBookTransactionDTO id)
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
                                 where a.MI_Id == b.MI_Id && a.MI_Id == id.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == id.IVRMUL_Id
                                 select a).ToList();

                if (lib_list1.Count > 0)
                {
                    id.courselist = _LibraryContext.MasterCourseDMO.Where(t => t.MI_Id == id.MI_Id && t.AMCO_ActiveFlag == true).Distinct().OrderBy(a => a.AMCO_Order).ToArray();

                    id.branchlist = _LibraryContext.ClgMasterBranchDMO.Where(a => a.MI_Id == id.MI_Id && a.AMB_ActiveFlag == true).Distinct().OrderBy(a => a.AMB_Order).ToArray();

                    id.semisterlist = _LibraryContext.CLG_Adm_Master_SemesterDMO.Where(s => s.MI_Id == id.MI_Id && s.AMSE_ActiveFlg == true).Distinct().OrderBy(S => S.AMSE_SEMOrder).ToArray();


                    id.LMAL_Id = lib_list1.FirstOrDefault().LMAL_Id;
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return id;
        }

        public async Task<CLGBookTransactionDTO> get_report(CLGBookTransactionDTO data)
        {
            try
            {
                string cousr_ids = "0";
                string brnch_idss = "0";
                string sem_idss = "0";


                List<long> cours_long = new List<long>();
                List<long> brnchs_long = new List<long>();
                List<long> semss_long = new List<long>();


                if (data.AGType == "Student")
                {
                    //------------------Course_ids
                    foreach (var item in data.selectedcourse)
                    {
                        cours_long.Add(item.AMCO_Id);
                    }
                    for (int s = 0; s < cours_long.Count(); s++)
                    {
                        cousr_ids = cousr_ids + ',' + cours_long[s].ToString();
                    }

                    //------------------Branch_ids
                    foreach (var item in data.selectedbrnchlst)
                    {
                        brnchs_long.Add(item.AMB_Id);
                    }
                    for (int s = 0; s < brnchs_long.Count(); s++)
                    {
                        brnch_idss = brnch_idss + ',' + brnchs_long[s].ToString();
                    }

                    //------------------Semester_ids
                    foreach (var item in data.selectedsemlst)
                    {
                        semss_long.Add(item.AMSE_Id);
                    }
                    for (int i = 0; i < semss_long.Count; i++)
                    {
                        sem_idss= sem_idss +','+ semss_long[i].ToString();
                    }
                }


                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_COLLEGE_TRANSACTION_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
                     SqlDbType.VarChar)
                    {
                        Value = data.Type
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
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
          SqlDbType.BigInt)
                    {
                        Value = data.LMAL_Id
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
                  
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
         SqlDbType.VarChar)
                    {
                        Value = cousr_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
            SqlDbType.VarChar)
                    {
                        Value = brnch_idss
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
            SqlDbType.VarChar)
                    {
                        Value = sem_idss
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
