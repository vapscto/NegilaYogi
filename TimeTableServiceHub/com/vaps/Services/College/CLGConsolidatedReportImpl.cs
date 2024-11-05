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
    public class CLGConsolidatedReportImpl : Interfaces.CLGConsolidatedReportInterface
    {
        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public CLGConsolidatedReportImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public CLGConsolidatedReportDTO getalldetails(CLGConsolidatedReportDTO data)
        {
            try
            {
                data.acayear = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList().Distinct().OrderByDescending(r=>r.ASMAY_Order).ToArray();
                data.dayslst_fix = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id) ).ToList().ToArray();
                data.categorylist = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().ToArray();
                data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                 from b in _ttcontext.TT_Final_GenerationDMO
                                 from c in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                 from d in _ttcontext.HR_Master_Employee_DMO
                                 where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                 )
                                 select new CLGConsolidatedReportDTO
                                 {
                                     empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                     HRME_Id = c.HRME_Id
                                 }).Distinct().OrderBy(g=>g.empName).ToArray();

                data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(a => a.MI_Id == data.MI_Id && a.TTMP_ActiveFlag == true).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public CLGConsolidatedReportDTO getreport(CLGConsolidatedReportDTO data)
        {

            try
            {
                if (data.TYPE=="SWD")
                {

                    string sidss = "0";
                   
                    foreach (var item in data.stfidss)
                    {
                        sidss = sidss + "," + item.HRME_Id;
                    }

                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_CONSOLIDATED_REPORT_STAFF_WORKLOAD_DETAILS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        //cmd.Parameters.Add(new SqlParameter("@TYPE",
                        //   SqlDbType.VarChar)
                        //{
                        //    Value = data.TYPE
                        //});
                        cmd.Parameters.Add(new SqlParameter("@Staffids",
                               SqlDbType.VarChar)
                        {
                            Value = sidss
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.subjectlist = (from b in _ttcontext.TT_Final_GenerationDMO
                                        from c in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                        from d in _ttcontext.IVRM_School_Master_SubjectsDMO
                                        where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1 && c.ISMS_Id == d.ISMS_Id && d.MI_Id == b.MI_Id)
                                        select new CLGConsolidatedReportDTO
                                        {
                                            ISMS_Id = d.ISMS_Id,
                                            ISMS_SubjectName = d.ISMS_SubjectName
                                        }).Distinct().ToArray();

                    data.class_sectons = (from a in _ttcontext.TT_Final_GenerationDMO
                                          from b in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                          from c in _ttcontext.MasterCourseDMO
                                          from d in _ttcontext.ClgMasterBranchDMO
                                          from e in _ttcontext.CLG_Adm_Master_SemesterDMO
                                          from f in _ttcontext.Adm_College_Master_SectionDMO
                                          where (a.TTFG_Id == b.TTFG_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.TTFG_ActiveFlag == true && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ACMS_Id == f.ACMS_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id)
                                          select new CLGConsolidatedReportDTO
                                          {
                                              AMCO_Id = c.AMCO_Id,
                                              AMB_Id = d.AMB_Id,
                                              AMSE_Id = e.AMSE_Id,
                                              ACMS_Id = f.ACMS_Id,
                                              ASMCL_ClassName = c.AMCO_CourseName + " : " + d.AMB_BranchName + " : " + e.AMSE_SEMName + " : " + f.ACMS_SectionName,

                                          }).Distinct().ToArray();
                }
                else
                if (data.TYPE == "RMR")
                {
                    data.roomlst = _ttcontext.TT_Master_RoomDMO.Where(c => c.TTMRM_ActiveFlg.Equals(true) && c.MI_Id.Equals(data.MI_Id)).Distinct().ToList().ToArray();

                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    if (data.TTMD_Id>0)
                    {
                        data.dayslst = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id) && c.TTMD_Id==data.TTMD_Id).ToList().ToArray();
                    }
                    else
                    {
                        data.dayslst = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();
                    }
                   

                    data.TT_Break_list_all = (from a in _ttcontext.CLGTT_Master_BreakDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id   && t.TTMBC_ActiveFlag == true && t.TTMC_Id==data.TTMC_Id)
                                              select new CLGTTCourseWiseReportDTO
                                              {
                                                  ASMAY_Id = a.ASMAY_Id,
                                                  TTMD_Id=a.TTMD_Id,
                                                  TTMBC_AfterPeriod = a.TTMBC_AfterPeriod,
                                                  TTMBC_BreakName = a.TTMBC_BreakName,
                                                  TTMBC_BreakStartTime = a.TTMBC_BreakStartTime,
                                                  TTMBC_BreakEndTime = a.TTMBC_BreakEndTime,
                                              }).Distinct().ToArray();


                    data.periodtimelist = (from a in _ttcontext.TT_Master_Day_Period_TimeDMO.Where(e => e.ASMAY_Id == data.ASMAY_Id && e.MI_Id == data.MI_Id && e.TTMDPT_ActiveFlag == true)
                                              select new CLGTTCourseWiseReportDTO
                                              {
                                                  ASMAY_Id = a.ASMAY_Id,
                                                  TTMD_Id = a.TTMD_Id,
                                                  TTMP_Id = a.TTMP_Id,
                                                  TTMDPT_EndTime = a.TTMDPT_EndTime,
                                                  TTMDPT_StartTime = a.TTMDPT_StartTime,
                                              }
                                            ).Distinct().ToArray();



                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_CONSOLIDATED_REPORT_ROOMWISE_NEW";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.rpttyp
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTMC_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.TTMC_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.TTMD_Id
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }


                else  if (data.TYPE == "ST")
                {
                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();


                    data.dayslst = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new CLGConsolidatedReportDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().OrderBy(r=>r.empName).ToArray();


                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_CONSOLIDATED_REPORT_STAFF";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }


                else if (data.TYPE == "STD")
                {



                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_CONSOLIDATED_REPORT_COUNT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.dayslst = _ttcontext.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new CLGConsolidatedReportDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().OrderBy(r=>r.empName).ToArray();

                }
                else if (data.TYPE == "CSUB" || data.TYPE == "STSUB")
                {

                    string sidss = "0";
                    string didss = "0";
                    string pidss = "0";
                    foreach (var item in data.stfidss)
                    {
                        sidss = sidss + "," + item.HRME_Id;
                    }
                    foreach (var item in data.dayidss)
                    {
                        didss = didss + "," + item.TTMD_Id;
                    }
                    foreach (var item in data.periodidss)
                    {
                        pidss = pidss + "," + item.TTMP_Id;
                    }


                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_CONSOLIDATED_REPORT_COURSE_SUBJECT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
                        });
                        cmd.Parameters.Add(new SqlParameter("@STFIDs",
                               SqlDbType.VarChar)
                        {
                            Value = sidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@DAYIDs",
                               SqlDbType.VarChar)
                        {
                            Value = didss
                        });
                        cmd.Parameters.Add(new SqlParameter("@PERIODIDs",
                               SqlDbType.VarChar)
                        {
                            Value = pidss
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    data.subjectlist = (from b in _ttcontext.TT_Final_GenerationDMO
                                        from c in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                        from d in _ttcontext.IVRM_School_Master_SubjectsDMO
                                        where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && d.ISMS_ActiveFlag == 1 && c.ISMS_Id == d.ISMS_Id && d.MI_Id == b.MI_Id)
                                        select new CLGConsolidatedReportDTO
                                        {
                                            ISMS_Id = d.ISMS_Id,
                                            ISMS_SubjectName = d.ISMS_SubjectName
                                        }).Distinct().ToArray();

                    data.class_sectons = (from a in _ttcontext.TT_Final_GenerationDMO
                                          from b in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                          from c in _ttcontext.MasterCourseDMO
                                          from d in _ttcontext.ClgMasterBranchDMO
                                          from e in _ttcontext.CLG_Adm_Master_SemesterDMO
                                          from f in _ttcontext.Adm_College_Master_SectionDMO
                                          where (a.TTFG_Id == b.TTFG_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.TTFG_ActiveFlag == true && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id&& b.AMSE_Id==e.AMSE_Id && b.ACMS_Id==f.ACMS_Id && a.MI_Id==e.MI_Id && a.MI_Id==f.MI_Id)
                                          select new CLGConsolidatedReportDTO
                                          {
                                              AMCO_Id = c.AMCO_Id,
                                              AMB_Id = d.AMB_Id,
                                              AMSE_Id = e.AMSE_Id,
                                              ACMS_Id = f.ACMS_Id,
                                              ASMCL_ClassName = c.AMCO_CourseName + " : " + d.AMB_BranchName + " : " + e.AMSE_SEMName + " : " + f.ACMS_SectionName,
                                            
                                          }).Distinct().ToArray();


                }
                else if (data.TYPE == "SSW")
                {
                    string sidss = "0";
                    string didss = "0";
                    string pidss = "0";

                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_SCHOOL_CONSOLIDATED_REPORT_CLASS_SUBJECT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
                        });
                        cmd.Parameters.Add(new SqlParameter("@STFIDs",
                               SqlDbType.VarChar)
                        {
                            Value = sidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@DAYIDs",
                               SqlDbType.VarChar)
                        {
                            Value = didss
                        });
                        cmd.Parameters.Add(new SqlParameter("@PERIODIDs",
                               SqlDbType.VarChar)
                        {
                            Value = pidss
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new CLGConsolidatedReportDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().OrderBy(e => e.empName).ToArray();
                }
                else if (data.TYPE == "SPW")
                {
                    string sidss = "0";
                    string didss = "0";
                    string pidss = "0";

                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_CONSOLIDATED_REPORT_COURSE_SUBJECT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
                        });
                        cmd.Parameters.Add(new SqlParameter("@STFIDs",
                               SqlDbType.VarChar)
                        {
                            Value = sidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@DAYIDs",
                               SqlDbType.VarChar)
                        {
                            Value = didss
                        });
                        cmd.Parameters.Add(new SqlParameter("@PERIODIDs",
                               SqlDbType.VarChar)
                        {
                            Value = pidss
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new CLGConsolidatedReportDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().OrderBy(e => e.empName).ToArray();
                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();
                }
                else if (data.TYPE == "STFP")
                {
                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_DAYWISE_STAFFFREEPERIODS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.TTMD_Id
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new CLGConsolidatedReportDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().OrderBy(e => e.empName).ToArray();
                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();


                }
                else if (data.TYPE == "PSTF")
                {
                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_DAY_PERIOD_FREE_STAFF";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.TTMD_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TTMP_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.TTMP_Id
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.TYPE == "SPC")
                {
                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    data.stafflst = (from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from b in _ttcontext.TT_Final_GenerationDMO
                                     from c in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                     from d in _ttcontext.HR_Master_Employee_DMO
                                     where (b.TTFG_Id == c.TTFG_Id && b.MI_Id == data.MI_Id && a.HRME_Id == c.HRME_Id && b.TTFG_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == d.HRME_Id && d.MI_Id == b.MI_Id
                                     //&&( a.TTMSAB_Abbreviation== "SCH" || a.TTMSAB_Abbreviation == "RS")
                                     )
                                     select new TTConsolidatedDTO
                                     {
                                         empName = d.HRME_EmployeeFirstName + " " + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == " " || d.HRME_EmployeeMiddleName == "0" ? " " : d.HRME_EmployeeMiddleName) + " " + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == " " || d.HRME_EmployeeLastName == "0" ? " " : d.HRME_EmployeeLastName),
                                         HRME_Id = c.HRME_Id,
                                         TTMSAB_Abbreviation = a.TTMSAB_Abbreviation
                                     }).Distinct().ToArray();


                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_STAFF_CLASS_PERIODWISE_CONSOLIDATED_REPORT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
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
                            data.finaltable = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }


                }
                else if (data.TYPE == "CSPC")
                {
                    data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                    data.class_sectons = (from a in _ttcontext.TT_Final_GenerationDMO
                                          from b in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                          from c in _ttcontext.MasterCourseDMO
                                          from d in _ttcontext.ClgMasterBranchDMO
                                          from e in _ttcontext.CLG_Adm_Master_SemesterDMO
                                          from f in _ttcontext.Adm_College_Master_SectionDMO
                                          where (a.TTFG_Id == b.TTFG_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.TTFG_ActiveFlag == true && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id && b.ACMS_Id == f.ACMS_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id)
                                          select new CLGConsolidatedReportDTO
                                          {
                                              AMCO_Id = c.AMCO_Id,
                                              AMB_Id = d.AMB_Id,
                                              AMSE_Id = e.AMSE_Id,
                                              ACMS_Id = f.ACMS_Id,
                                              ASMCL_ClassName = c.AMCO_CourseName + " : " + d.AMB_BranchName + " : " + e.AMSE_SEMName + " : " + f.ACMS_SectionName,
                                          }).Distinct().ToArray();


                    using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TT_CLG_COURSE_SECTION_PERIODWISE_CONSOLIDATED_REPORT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                           SqlDbType.VarChar)
                        {
                            Value = data.TYPE
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
                            data.finaltable = retObject.ToArray();
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

                throw ex;
            }
            
        
            return data;







        }
    }
}
