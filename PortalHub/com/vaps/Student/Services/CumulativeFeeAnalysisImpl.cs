using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using System.Linq;

namespace PortalHub.com.vaps.Student.Services
{
    public class CumulativeFeeAnalysisImpl : Interfaces.CumulativeFeeAnalysisInterface
    {
        public PortalContext _Feecontext;
        public CumulativeFeeAnalysisImpl(PortalContext Feecontext)
        {
            _Feecontext = Feecontext;
        }

        public async Task<StudentDashboardDTO> getloaddata(StudentDashboardDTO data)

        {
            try
            {
                //var ASMAY_Id = _Feecontext.AcademicYearDMO.Where(y => y.Is_Active == true && y.MI_Id == data.MI_Id).OrderByDescending(y => y.ASMAY_Order).FirstOrDefault().ASMAY_Id;
                //   data.acdlist = _Feecontext.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.acdlist = (from a in _Feecontext.School_Adm_Y_StudentDMO
                                from b in _Feecontext.AdmissionStudentDMO
                                from c in _Feecontext.AcademicYearDMO
                                where (a.ASMAY_Id == c.ASMAY_Id && b.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id)
                                select new StudentDashboardDTO
                                {
                                    ASMAY_Year = c.ASMAY_Year,
                                    ASMAY_Id = a.ASMAY_Id

                                }).Distinct().ToArray();
                var studentfeedetails = (from c in _Feecontext.FeeStudentTransactionDMO
                                         from a in _Feecontext.feeMTH

                                         from b in _Feecontext.feeTr
                                         from d in _Feecontext.FeeHeadDMO
                                         where (a.FMT_Id == b.FMT_Id && c.FMH_Id == d.FMH_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == c.FTI_Id && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id && c.FSS_NetAmount > 0) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                         select new StudentDashboardDTO
                                         {
                                             FSS_CurrentYrCharges = c.FSS_CurrentYrCharges,
                                             FSS_ToBePaid = c.FSS_ToBePaid,
                                             FSS_PaidAmount = c.FSS_PaidAmount,
                                             FSS_ConcessionAmount = c.FSS_ConcessionAmount,
                                             FTI_Name = b.FMT_Name,
                                             FMH_FeeName = d.FMH_FeeName

                                         }
        ).ToList();

                data.studentfeedetails = (from i in studentfeedetails
                                          group i by new { i.FTI_Name, i.FMH_FeeName } into g
                                          select new StudentDashboardDTO
                                          {
                                              FSS_CurrentYrCharges = g.Sum(t => t.FSS_CurrentYrCharges),
                                              FSS_ToBePaid = g.Sum(t => t.FSS_ToBePaid),
                                              FSS_PaidAmount = g.Sum(t => t.FSS_PaidAmount),
                                              FSS_ConcessionAmount = g.Sum(t => t.FSS_ConcessionAmount),
                                              FTI_Name = g.Key.FTI_Name,
                                              FMH_FeeName = g.Key.FMH_FeeName
                                          }).Distinct().ToArray();



             //   using (var cmd = _Feecontext.Database.GetDbConnection().CreateCommand())
             //   {
             //       cmd.CommandText = "PORTAL_Cumulative_Fee_Analysis";
             //       cmd.CommandType = CommandType.StoredProcedure;

             //       cmd.Parameters.Add(new SqlParameter("@mi_id",
             //SqlDbType.VarChar)
             //       {
             //           Value = data.MI_Id
             //       });
             //       cmd.Parameters.Add(new SqlParameter("@amst_id",
             //        SqlDbType.VarChar)
             //       {
             //           Value = data.AMST_Id
             //       });

             //       if (cmd.Connection.State != ConnectionState.Open)
             //           cmd.Connection.Open();

             //       var retObject = new List<dynamic>();
             //       try
             //       {
             //           using (var dataReader = await cmd.ExecuteReaderAsync())
             //           {
             //               while (await dataReader.ReadAsync())
             //               {
             //                   var dataRow = new ExpandoObject() as IDictionary<string, object>;
             //                   for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
             //                   {
             //                       dataRow.Add(
             //                           dataReader.GetName(iFiled),
             //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
             //                       );
             //                   }
             //                   retObject.Add((ExpandoObject)dataRow);
             //               }
             //           }
             //           data.feeAnalysisList = retObject.ToArray();
             //       }
             //       catch (Exception ex)
             //       {
             //           Console.WriteLine(ex.Message);
             //       }
             //   }


                using (var cmd = _Feecontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_Cumulative_Fee_Analysis";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                     SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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
                        data.feeAnalysisList = retObject.ToArray();
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

        public async Task<StudentDashboardDTO> onreport(StudentDashboardDTO data)
        {
            try
            {
                // getloaddata(data);

                var studentfeedetails = (from c in _Feecontext.FeeStudentTransactionDMO
                                         from a in _Feecontext.feeMTH

                                         from b in _Feecontext.feeTr
                                         from d in _Feecontext.FeeHeadDMO
                                         where (a.FMT_Id == b.FMT_Id && c.FMH_Id == d.FMH_Id && a.FMH_Id == c.FMH_Id && a.FTI_Id == c.FTI_Id && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id && c.FSS_NetAmount > 0) /*&& a.fmg_id.Contains(data.fmg_id)*/
                                         select new StudentDashboardDTO
                                         {
                                             FSS_CurrentYrCharges = c.FSS_CurrentYrCharges,
                                             FSS_ToBePaid = c.FSS_ToBePaid,
                                             FSS_PaidAmount = c.FSS_PaidAmount,
                                             FSS_ConcessionAmount = c.FSS_ConcessionAmount,
                                             FTI_Name = b.FMT_Name,
                                             FMH_FeeName = d.FMH_FeeName

                                         }
       ).ToList();

                data.studentfeedetails = (from i in studentfeedetails
                                          group i by new { i.FTI_Name, i.FMH_FeeName } into g
                                          select new StudentDashboardDTO
                                          {
                                              FSS_CurrentYrCharges = g.Sum(t => t.FSS_CurrentYrCharges),
                                              FSS_ToBePaid = g.Sum(t => t.FSS_ToBePaid),
                                              FSS_PaidAmount = g.Sum(t => t.FSS_PaidAmount),
                                              FSS_ConcessionAmount = g.Sum(t => t.FSS_ConcessionAmount),
                                              FTI_Name = g.Key.FTI_Name,
                                              FMH_FeeName = g.Key.FMH_FeeName
                                          }).Distinct().ToArray();



                //   using (var cmd = _Feecontext.Database.GetDbConnection().CreateCommand())
                //   {
                //       cmd.CommandText = "PORTAL_Cumulative_Fee_Analysis";
                //       cmd.CommandType = CommandType.StoredProcedure;

                //       cmd.Parameters.Add(new SqlParameter("@mi_id",
                //SqlDbType.VarChar)
                //       {
                //           Value = data.MI_Id
                //       });
                //       cmd.Parameters.Add(new SqlParameter("@amst_id",
                //        SqlDbType.VarChar)
                //       {
                //           Value = data.AMST_Id
                //       });

                //       if (cmd.Connection.State != ConnectionState.Open)
                //           cmd.Connection.Open();

                //       var retObject = new List<dynamic>();
                //       try
                //       {
                //           using (var dataReader = await cmd.ExecuteReaderAsync())
                //           {
                //               while (await dataReader.ReadAsync())
                //               {
                //                   var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                   for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                   {
                //                       dataRow.Add(
                //                           dataReader.GetName(iFiled),
                //                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                       );
                //                   }
                //                   retObject.Add((ExpandoObject)dataRow);
                //               }
                //           }
                //           data.feeAnalysisList = retObject.ToArray();
                //       }
                //       catch (Exception ex)
                //       {
                //           Console.WriteLine(ex.Message);
                //       }
                //   }


                using (var cmd = _Feecontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_Cumulative_Fee_Analysis";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                     SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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
                        data.feeAnalysisList = retObject.ToArray();
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
