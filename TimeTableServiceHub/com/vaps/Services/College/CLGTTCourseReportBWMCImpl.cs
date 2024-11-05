using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class CLGTTCourseReportBWMCImpl : Interfaces.CLGTTCourseReportBWMCInterface
    {

        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public CLGTTCourseReportBWMCImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public CLGTTCourseReportBWMCDTO getdetails(CLGTTCourseReportBWMCDTO data)
        {
            try
            {

                data.acayear = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(y=>y.ASMAY_Order).ToArray();

                data.categorylist = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToArray();

                data.sectionlist = _ttcontext.Adm_College_Master_SectionDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToArray();

                data.periodslst = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.TTMP_ActiveFlag == true).ToArray();
              

  

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public CLGTTCourseReportBWMCDTO getreport(CLGTTCourseReportBWMCDTO data)
        {
            //CLGTTCourseReportBWMCDTO objpge = Mapper.Map<CLGTTCourseReportBWMCDTO>(_category);
            //List<CLGTTCourseReportBWMCDTO> list = new List<CLGTTCourseReportBWMCDTO>();

            try
            {

                string semids = "0";
                string secids = "0";
                string crsids = "0";
                string brnids = "0";
                

                for (int d = 0; d < data.classarray.Count(); d++)
                {
                    semids = semids + ',' + data.classarray[d].AMSE_Id;
                }
                for (int d = 0; d < data.sectionarray.Count(); d++)
                {
                    secids = secids + ',' + data.sectionarray[d].ACMS_Id;
                }
                for (int d = 0; d < data.crids.Count(); d++)
                {
                    crsids = crsids + ',' + data.crids[d].AMCO_Id;
                }
                for (int d = 0; d < data.brnchds.Count(); d++)
                {
                    brnids = brnids + ',' + data.brnchds[d].AMB_Id;
                }

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_COURSEWISE_BWMC_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                SqlDbType.VarChar)
                    {
                        Value = crsids
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                SqlDbType.VarChar)
                    {
                        Value = brnids
                    });


                    cmd.Parameters.Add(new SqlParameter("@AMSE_IDs",
                      SqlDbType.NVarChar)
                    {
                        Value = semids
                    });

                    cmd.Parameters.Add(new SqlParameter("@ACMS_IDs",
                      SqlDbType.NVarChar)
                    {
                        Value = secids
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
                        data.Time_Table = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                List<long> semm = new List<long>();
                List<long> crs = new List<long>();
                List<long> bran = new List<long>();
                for (int i = 0; i < data.classarray.Length; i++)
                {
                    semm.Add(data.classarray[i].AMSE_Id);
                }

                for (int i = 0; i < data.crids.Length; i++)
                {
                    crs.Add(data.crids[i].AMCO_Id);
                }

                for (int i = 0; i < data.brnchds.Length; i++)
                {
                    bran.Add(data.brnchds[i].AMB_Id);
                }

                data.TT_Break_list_all = (from a in _ttcontext.CLGTT_Master_BreakDMO
                                          .Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTMC_Id == data.TTMC_Id  && t.TTMBC_ActiveFlag == true  && crs.Contains(t.AMCO_Id) && bran.Contains(t.AMB_Id))            
                   select new CLGTTCourseReportBWMCDTO
                {
                      TTMD_Id=a.TTMD_Id,
                       TTMBC_AfterPeriod = a.TTMBC_AfterPeriod,
                       TTMBC_BreakName = a.TTMBC_BreakName,
                       TTMBC_BreakStartTime = a.TTMBC_BreakStartTime,
                       TTMBC_BreakEndTime = a.TTMBC_BreakEndTime,
                }).Distinct().ToArray();

                data.allday = ( from a in _ttcontext.TT_Master_DayDMO
                               where  a.MI_Id == data.MI_Id && a.TTMD_ActiveFlag==true      
                   select new CLGTTCourseReportBWMCDTO
                  {
                       TTMD_Id = a.TTMD_Id,
                       TTMD_DayName = a.TTMD_DayName,
                }).Distinct().ToArray();


                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_COURSEWISE_DAY_PERIODTIME";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                    cmd.Parameters.Add(new SqlParameter("@TTMC_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.TTMC_Id
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
                        data.periodtimelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_COURSEWISE_DAY_DISTINCT_PERIODTIME";
                    cmd.CommandType = CommandType.StoredProcedure;

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
                    cmd.Parameters.Add(new SqlParameter("@TTMC_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.TTMC_Id
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
                        data.periodtimelist_distinct = retObject.ToArray();
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

      

        public CLGTTCourseReportBWMCDTO getclass_catg(CLGTTCourseReportBWMCDTO data)
        {
            try
            {
                data.classlist = (from a in _ttcontext.TTMasterCategoryDMO
                                  from b in _ttcontext.TT_Category_Class_DMO
                                  from c in _ttcontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag==true
                                  select new CLGTTCourseReportBWMCDTO
                                  {
                                      ASMCL_Id = c.ASMCL_Id,
                                      ASMCL_ClassName = c.ASMCL_ClassName,
                                      TTMC_Id = a.TTMC_Id,
                                      TTMC_CategoryName = a.TTMC_CategoryName,
                                  }
      ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;

        }



    }
}
