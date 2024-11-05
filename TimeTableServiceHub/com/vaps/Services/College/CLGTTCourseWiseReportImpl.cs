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
    public class CLGTTCourseWiseReportImpl : Interfaces.CLGTTCourseWiseReportInterface
    {

        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public CLGTTCourseWiseReportImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public CLGTTCourseWiseReportDTO getdetails(CLGTTCourseWiseReportDTO data)
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
        public CLGTTCourseWiseReportDTO getreport(CLGTTCourseWiseReportDTO data)
        {
            //CLGTTCourseWiseReportDTO objpge = Mapper.Map<CLGTTCourseWiseReportDTO>(_category);
            //List<CLGTTCourseWiseReportDTO> list = new List<CLGTTCourseWiseReportDTO>();

            try
            {

                string semids = "0";
                string secids = "0";

                for (int d = 0; d < data.classarray.Count(); d++)
                {
                    semids = semids + ',' + data.classarray[d].AMSE_Id;
                }
                for (int d = 0; d < data.sectionarray.Count(); d++)
                {
                    secids = secids + ',' + data.sectionarray[d].ACMS_Id;
                }

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_COURSEWISE_REPORT";
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
                SqlDbType.BigInt)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.AMB_Id
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
                List<long> secc = new List<long>();
                for (int i = 0; i < data.classarray.Length; i++)
                {
                    semm.Add(data.classarray[i].AMSE_Id);
                }

                //for (int i = 0; i < data.sectionarray.Length; i++)
                //{
                //    secc.Add(data.sectionarray[i].ACMS_Id);
                //}

                data.TT_Break_list_all = (from a in _ttcontext.CLGTT_Master_BreakDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && semm.Contains(t.AMSE_Id) && t.TTMBC_ActiveFlag == true )
                   select new CLGTTCourseWiseReportDTO
                {
                       ASMAY_Id=a.ASMAY_Id,
                       AMCO_Id=a.AMCO_Id,
                       AMB_Id=a.AMB_Id,
                       AMSE_Id = a.AMSE_Id,
                       TTMBC_AfterPeriod = a.TTMBC_AfterPeriod,
                       TTMBC_BreakName = a.TTMBC_BreakName,
                }).Distinct().ToArray();


                data.dayBreak_list_all = (from a in _ttcontext.CLGTT_Master_BreakDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && semm.Contains(t.AMSE_Id) && t.TTMBC_ActiveFlag == true)
                                          select new CLGTTCourseWiseReportDTO
                                          {
                                              ASMAY_Id = a.ASMAY_Id,
                                              AMCO_Id = a.AMCO_Id,
                                              AMB_Id = a.AMB_Id,
                                              AMSE_Id = a.AMSE_Id,
                                              TTMD_Id = a.TTMD_Id,
                                              TTMBC_AfterPeriod = a.TTMBC_AfterPeriod,
                                              TTMBC_BreakName = a.TTMBC_BreakName,
                                          }).Distinct().ToArray();

                data.allday = (from t in _ttcontext.CLGTT_Master_Day_CourseBranchDMO
                               from a in _ttcontext.TT_Master_DayDMO
                               where t.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && semm.Contains(t.AMSE_Id) && t.TTMDC_ActiveFlag == true && a.TTMD_Id==t.TTMD_Id && a.TTMD_ActiveFlag==true      
                   select new CLGTTCourseWiseReportDTO
                {
                       ASMAY_Id=t.ASMAY_Id,
                       AMCO_Id=t.AMCO_Id,
                       AMB_Id=t.AMB_Id,
                       AMSE_Id = t.AMSE_Id,
                       TTMD_Id = t.TTMD_Id,
                       TTMD_DayName = a.TTMD_DayName,
                }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public CLGTTCourseWiseReportDTO getclass_catg(CLGTTCourseWiseReportDTO data)
        {
            try
            {
                data.classlist = (from a in _ttcontext.TTMasterCategoryDMO
                                  from b in _ttcontext.TT_Category_Class_DMO
                                  from c in _ttcontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag==true
                                  select new CLGTTCourseWiseReportDTO
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
        public async Task<CLGTTCourseWiseReportDTO> GetStudentDetails(CLGTTCourseWiseReportDTO data)
        {
            try
            {
              
                

                #region Course Branch & Semester Id
                var ids = (from a in _ttcontext.Adm_College_Yearly_StudentDMO
                           where (a.ACYST_ActiveFlag == 1 && a.AMCST_Id == data.AMCST_Id)
                           select new CLGTTCourseWiseReportDTO
                           {
                               AMCST_Id = a.AMCST_Id,
                               AMCO_Id = a.AMCO_Id,
                               AMB_Id = a.AMB_Id,
                               AMSE_Id = a.AMSE_Id,
                               ACMS_Id = a.ACMS_Id
                           }).Distinct().FirstOrDefault();
                data.periodslst = (from a in _ttcontext.TT_Master_PeriodDMO
                                   from b in _ttcontext.ClgPeriodAllocation_Course_DMO
                                   where (a.MI_Id == data.MI_Id && a.TTMP_Id == b.TTMP_Id && a.TTMP_ActiveFlag == true && b.TTMPC_ActiveFlag == true && b.AMCO_Id == ids.AMCO_Id && b.AMB_Id == ids.AMB_Id && b.AMSE_Id == ids.AMSE_Id)
                                   select a).Distinct().ToArray();

                _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.TTMP_ActiveFlag == true).ToArray();
                #endregion

                #region Student Details
                using (var cmd1 = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "CLG_PORTAL_STUDENTDETAILS";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@ASMAY_Id",
               SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd1.Parameters.Add(new SqlParameter("@AMCST_Id",
              SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd1.ExecuteReaderAsync())
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
                        data.studentdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion

                #region TimeTable
                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_COURSEWISE_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",SqlDbType.BigInt)
                    {
                        Value = ids.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",SqlDbType.BigInt)
                    {
                        Value = ids.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_IDs", SqlDbType.NVarChar)
                    {
                        Value = ids.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_IDs",SqlDbType.NVarChar)
                    {
                        Value = ids.ACMS_Id
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
                #endregion

                data.TT_Break_list_all = (from a in _ttcontext.CLGTT_Master_BreakDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.AMCO_Id == ids.AMCO_Id && t.AMB_Id == ids.AMB_Id && t.AMSE_Id == ids.AMSE_Id && t.TTMBC_ActiveFlag == true)
                    select new CLGTTCourseWiseReportDTO
                    {
                        ASMAY_Id = a.ASMAY_Id,
                        AMCO_Id = a.AMCO_Id,
                        AMB_Id = a.AMB_Id,
                        AMSE_Id = a.AMSE_Id,
                        TTMBC_AfterPeriod = a.TTMBC_AfterPeriod,
                        TTMBC_BreakName = a.TTMBC_BreakName,
                    }).Distinct().ToArray();

                data.allday = (from t in _ttcontext.CLGTT_Master_Day_CourseBranchDMO
                               from a in _ttcontext.TT_Master_DayDMO
                               where t.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && t.AMCO_Id == ids.AMCO_Id && t.AMB_Id == ids.AMB_Id && t.AMSE_Id == ids.AMSE_Id && t.TTMDC_ActiveFlag == true && a.TTMD_Id == t.TTMD_Id && a.TTMD_ActiveFlag == true
                               select new CLGTTCourseWiseReportDTO
                               {
                                   ASMAY_Id = t.ASMAY_Id,
                                   AMCO_Id = t.AMCO_Id,
                                   AMB_Id = t.AMB_Id,
                                   AMSE_Id = t.AMSE_Id,
                                   TTMD_Id = t.TTMD_Id,
                                   TTMD_DayName = a.TTMD_DayName,
                               }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
