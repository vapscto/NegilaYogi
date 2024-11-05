using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeTTImpl : Interfaces.EmployeeTTInterface
    {
        public TTContext _tt;
        public ExamContext _exm;
        public EmployeeTTImpl(TTContext tt, ExamContext exm)
        {
            _tt = tt;
            _exm = exm;
        }
        public EmployeeDashboardDTO getdata(EmployeeDashboardDTO dto)
        {
            try
            {
                dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;

                dto.TT_final_generation = (from a in _tt.TT_Final_GenerationDMO
                                           from b in _tt.TT_Final_Generation_DetailedDMO
                                           from c in _tt.School_M_Class
                                           from d in _tt.School_M_Section
                                           from e in _tt.TT_Master_PeriodDMO
                                           from f in _tt.TT_Master_DayDMO
                                           where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id && a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                           select new
                                           {
                                               DayName = f.TTMD_DayName,
                                               PeriodCount = e.TTMP_PeriodName.Count()
                                           }).Distinct().GroupBy(f => f.DayName).Select(g => new EmployeeDashboardDTO { DayName = g.Key, PeriodCount = g.Count() }).ToArray();


                dto.allperiods = _tt.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag == true && c.MI_Id == dto.MI_Id).ToArray();

                dto.periods = _tt.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(dto.MI_Id)).ToArray();


                //dto.periods = (from a in _tt.TT_Final_GenerationDMO
                //                     from b in _tt.TT_Final_Generation_DetailedDMO
                //                     from c in _tt.School_M_Class
                //                     from d in _tt.School_M_Section
                //                     from e in _tt.TT_Master_PeriodDMO
                //                     from h in _tt.TTBreakTimeSettingsDMO
                //                     where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id &&
                //                     a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id
                //                     && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id &&
                //                     d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id
                //                     && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id && 
                //                     h.ASMAY_Id == dto.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id)
                //                     select new EmployeeDashboardDTO
                //                     {
                //                         TTMP_PeriodName = e.TTMP_PeriodName,                            
                //                         TTMB_BreakName = h.TTMB_BreakName,
                //                         TTMB_AfterPeriod = h.TTMB_AfterPeriod
                //                     }).Distinct().ToArray();


                using (var cmd = _tt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_class_sections";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
               SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                 SqlDbType.VarChar)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                   

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
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
                        dto.class_sectons = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //dto.breatketime = (from a in _tt.TT_Final_GenerationDMO
                //                     from b in _tt.TT_Final_Generation_DetailedDMO
                //                     from c in _tt.School_M_Class
                //                     from d in _tt.School_M_Section
                //                     from g in _tt.IVRM_School_Master_SubjectsDMO
                //                     from h in _tt.TTBreakTimeSettingsDMO
                //                     where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id &&
                //                     a.MI_Id == dto.MI_Id &&  b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id &&
                //                     d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id
                //                   && b.HRME_Id == dto.HRME_Id &&
                //                     a.ASMAY_Id == dto.ASMAY_Id && a.MI_Id == g.MI_Id &&
                //                     b.ISMS_Id == g.ISMS_Id && h.ASMAY_Id == dto.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id)
                //                     select new EmployeeDashboardDTO
                //                     {

                //                         ASMCL_ClassName = c.ASMCL_ClassName,
                //                         ASMC_SectionName = d.ASMC_SectionName,
                //                         ISMS_SubjectName = g.ISMS_SubjectName,
                //                         TTMB_BreakName = h.TTMB_BreakName,
                //                         TTMB_AfterPeriod = h.TTMB_AfterPeriod
                //                     }).Distinct().ToArray();


                //dto.class_sectons = (from a in _tt.TT_Final_GenerationDMO
                //                     from b in _tt.TT_Final_Generation_DetailedDMO
                //                     from c in _tt.School_M_Class
                //                     from d in _tt.School_M_Section
                //                     from e in _tt.TT_Master_PeriodDMO
                //                     from f in _tt.TT_Master_DayDMO
                //                     from g in _tt.IVRM_School_Master_SubjectsDMO
                //                     from h in _tt.TTBreakTimeSettingsDMO
                //                     where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id &&
                //                     a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id
                //                     && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id &&
                //                     d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id
                //                     && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id &&
                //                     a.ASMAY_Id == dto.ASMAY_Id && a.MI_Id == g.MI_Id &&
                //                     b.ISMS_Id == g.ISMS_Id && h.ASMAY_Id == dto.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id)
                //                     select new EmployeeDashboardDTO
                //                     {
                //                         P_Days = f.TTMD_DayName,
                //                         Period = e.TTMP_PeriodName,
                //                         ASMCL_ClassName = c.ASMCL_ClassName,
                //                         ASMC_SectionName = d.ASMC_SectionName,
                //                         ISMS_SubjectName = g.ISMS_SubjectName,
                //                         TTMB_BreakName = h.TTMB_BreakName,
                //                         TTMB_AfterPeriod=h.TTMB_AfterPeriod
                //                     }).Distinct().ToArray();








            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public EmployeeDashboardDTO getdaily_data(EmployeeDashboardDTO dto)
        {
            try
            {
                dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;

                dto.TT_final_generation = (from a in _tt.TT_Final_GenerationDMO
                                           from b in _tt.TT_Final_Generation_DetailedDMO
                                           from c in _tt.School_M_Class
                                           from d in _tt.School_M_Section
                                           from e in _tt.TT_Master_PeriodDMO
                                           from f in _tt.TT_Master_DayDMO

                                           where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id && a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id && f.TTMD_Id == dto.TTMD_Id)
                                           select new
                                           {
                                               ASMCL_ClassName = c.ASMCL_ClassName,
                                               ASMC_SectionName = d.ASMC_SectionName,
                                               //DayName = f.TTMD_DayName,
                                               PeriodCount = e.TTMP_PeriodName.Count()
                                           }).Distinct().GroupBy(c => new { c.ASMCL_ClassName, c.ASMC_SectionName }).Select(g => new EmployeeDashboardDTO { ASMCL_ClassName = g.Key.ASMCL_ClassName, ASMC_SectionName = g.Key.ASMC_SectionName, PeriodCount = g.Count() }).ToArray();


                using (var cmd = _tt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TTEmployee_class_sections";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
               SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                 SqlDbType.VarChar)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TTMD_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dto.TTMD_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
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
                        dto.class_sectons = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //dto.class_sectons = (from a in _tt.TT_Final_GenerationDMO
                //                     from b in _tt.TT_Final_Generation_DetailedDMO
                //                     from c in _tt.School_M_Class
                //                     from d in _tt.School_M_Section
                //                     from e in _tt.TT_Master_PeriodDMO
                //                     from f in _tt.TT_Master_DayDMO
                //                     from g in _tt.TT_Master_Day_Period_TimeDMO
                //                     where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id && g.TTMC_Id == a.TTMC_Id && g.MI_Id == a.MI_Id && g.TTMP_Id == e.TTMP_Id && g.TTMD_Id == f.TTMD_Id && a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id && f.TTMD_Id == dto.TTMD_Id && g.TTMDPT_ActiveFlag == true && g.ASMAY_Id == dto.ASMAY_Id)
                //                     select new EmployeeDashboardDTO
                //                     {
                //                         //P_Days = f.TTMD_DayName,
                //                         Period = e.TTMP_PeriodName,
                //                         ASMCL_ClassName = c.ASMCL_ClassName,
                //                         ASMC_SectionName = d.ASMC_SectionName,
                //                         TTMDPT_StartTime = g.TTMDPT_StartTime,
                //                         TTMDPT_EndTime = g.TTMDPT_EndTime,

                //                     }).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
