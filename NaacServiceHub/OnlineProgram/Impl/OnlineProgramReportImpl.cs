using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.NAAC;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.OnlineProgram;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.OnlineProgram.Impl
{
    public class OnlineProgramReportImpl:Interface.OnlineProgramReportInterface
    {

        public DomainModelMsSqlServerContext _context;
       public OnlineProgramReportImpl(DomainModelMsSqlServerContext b)
        {
            _context = b;
        }

        public OnlineProgramReport_DTO getyearlyprogram(OnlineProgramReport_DTO data)
        {
            try
            {
                var res1 = _context.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.departmentlist = res1.Distinct().ToArray();
                data.yearlist = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().ToArray();
                data.typelist = _context.ProgramsMasterTypeDMO.Where(t => t.MI_Id == data.MI_Id &&t.PRMTY_ActiveFlg==true).Distinct().ToArray();
                data.activitylist = _context.ProgramsYearlyActivitiesDMO.Where(t => t.PRYRA_ActiveFlag == true&&t.PRYRA_Duration==null ).Distinct().ToArray();
                data.levellist = _context.ProgramsMasterLevelDMO.Where(t =>t.MI_Id==data.MI_Id && t.PRMTLE_ActiveFlg == true).Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }

        public async Task<OnlineProgramReport_DTO> getYearlyProgramReport(OnlineProgramReport_DTO data)
        {
            try
            {
                 string year_ids = "0";
                List<long> year1_ids = new List<long>();
                foreach (var item in data.selectedyearlist)
                {
                    year1_ids.Add(item.ASMAY_Id);
                }
                for (int s = 0; s < year1_ids.Count(); s++)
                {
                    year_ids = year_ids + ',' + year1_ids[s].ToString();
                }
                // type
                string type_ids = "0";
                List<long> type1_ids = new List<long>();
                foreach (var item in data.selectedtypelist)
                {
                    type1_ids.Add(item.PRMTY_Id);
                }
                for (int s = 0; s < type1_ids.Count(); s++)
                {
                    type_ids = type_ids + ',' + type1_ids[s].ToString();
                }
                //activity
                string activity_ids = "0";
                List<long> activity1_ids = new List<long>();
                foreach(var item in data.selectedactivitylist)
                {
                    activity1_ids.Add(item.PRYRA_Id);
                }
                for(int s = 0; s < activity1_ids.Count(); s++)
                {
                    activity_ids = activity_ids + ',' + activity1_ids[s].ToString();
                }
                //level
                string level_ids = "0";
                List<long> level1_ids = new List<long>();
                foreach(var item in data.selectedlevellist)
                {
                    level1_ids.Add(item.PRMTLE_Id);
                }
                for(int s = 0; s < level1_ids.Count(); s++)
                {
                    level_ids = level_ids + ',' + level1_ids[s].ToString();
                }
                int r = 0;

                
                string fromdatecon = "";
                fromdatecon = data.PRYR_StartDate.ToString("dd/MM/yyyy");

                string todatecon = "";
                todatecon = data.PRYR_EndDate.ToString("dd/MM/yyyy");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "YearlyProgram_Reports";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = year_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@PRMTY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = type_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@PRYRA_Id",
                    SqlDbType.VarChar)
                    {
                        Value = activity_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@PRMTLE_Id",
                    SqlDbType.VarChar)
                    {
                        Value = level_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@PRYR_StartDate",
                    SqlDbType.VarChar)
                    {
                        Value = fromdatecon
                    });
                    cmd.Parameters.Add(new SqlParameter("@PRYR_EndDate",
                    SqlDbType.VarChar)
                    {
                        Value = todatecon
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

                //data.reportlist = (from a in _context.ProgramsYearlyDMO
                //                   from b in _context.ProgramsYearlyActivitiesDMO
                  //                from c in _context.ProgramsMasterLevelDMO
                //                   from d in _context.ProgramsMasterTypeDMO
                  //                from e in _context.AcademicYear
                //                   where (a.PRYR_Id == b.PRYR_Id&&a.ASMAY_Id==e.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == d.MI_Id && a.PRYR_StartDate == data.date1 && a.PRYR_EndDate == data.date2 && a.ASMAY_Id.Equals(data.selectedyearlist) && b.PRYRA_Id.Equals(data.selectedactivitylist) && c.PRMTLE_Id.Equals(data.selectedlevellist) && d.PRMTY_Id.Equals(data.selectedtypelist))
                //                   select new OnlineProgramReport_DTO
                //                   {
                //                    ASMAY_Year=e.ASMAY_Year,
                //                       ASMAY_Id = e.ASMAY_Id,

                //                       PRYR_ProgramName = a.PRYR_ProgramName,
                //                       PRYRA_ActivityName = b.PRYRA_ActivityName,
                //                       PRMTLE_ProgramLevel = c.PRMTLE_ProgramLevel,
                //                       PRMTY_ProgramType = d.PRMTY_ProgramType,
                //                       date1 = a.PRYR_StartDate,
                //                       date2 = a.PRYR_EndDate.Value,
                //                       PRYR_StartTime = a.PRYR_StartTime,
                //                       PRYR_EndTime = a.PRYR_EndTime,
                //                       PRYR_TotalParticipants = a.PRYR_TotalParticipants.Value,


                //                   }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }




        public async Task<OnlineProgramReport_DTO> ConferenceDetailsReport(OnlineProgramReport_DTO data)
        {
            try
            {
               
                // type
                string type_ids = "0";
                List<long> type1_ids = new List<long>();
                foreach (var item in data.selectedtypelist)
                {
                    type1_ids.Add(item.PRMTY_Id);
                }
                for (int s = 0; s < type1_ids.Count(); s++)
                {
                    type_ids = type_ids + ',' + type1_ids[s].ToString();
                }
                //dept
                string dept_ids = "0";
                List<long> dept1_ids = new List<long>();
                foreach (var item in data.selecteddepartmentlist)
                {
                    dept1_ids.Add(item.HRMD_Id);
                }
                for (int s = 0; s < dept1_ids.Count(); s++)
                {
                    dept_ids = dept_ids + ',' + dept1_ids[s].ToString();
                }
                //level
                string level_ids = "0";
                List<long> level1_ids = new List<long>();
                foreach (var item in data.selectedlevellist)
                {
                    level1_ids.Add(item.PRMTLE_Id);
                }
                for (int s = 0; s < level1_ids.Count(); s++)
                {
                    level_ids = level_ids + ',' + level1_ids[s].ToString();
                }
                int r = 0;


                string fromdatecon = "";
                fromdatecon = data.PRYR_StartDate.ToString("dd/MM/yyyy");

                string todatecon = "";
                todatecon = data.PRYR_EndDate.ToString("dd/MM/yyyy");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ConferenceDetails_Reports";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                   
                    cmd.Parameters.Add(new SqlParameter("@PRMTY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = type_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                    SqlDbType.VarChar)
                    {
                        Value = dept_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@PRMTLE_Id",
                    SqlDbType.VarChar)
                    {
                        Value = level_ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@PRYR_StartDate",
                    SqlDbType.VarChar)
                    {
                        Value = fromdatecon
                    });
                    cmd.Parameters.Add(new SqlParameter("@PRYR_EndDate",
                    SqlDbType.VarChar)
                    {
                        Value = todatecon
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

                //data.reportlist = (from a in _context.ProgramsYearlyDMO
                //                   from b in _context.ProgramsYearlyActivitiesDMO
                //                from c in _context.ProgramsMasterLevelDMO
                //                   from d in _context.ProgramsMasterTypeDMO
                //                from e in _context.AcademicYear
                //                   where (a.PRYR_Id == b.PRYR_Id&&a.ASMAY_Id==e.ASMAY_Id && a.MI_Id == data.MI_Id && c.MI_Id == d.MI_Id && a.PRYR_StartDate == data.date1 && a.PRYR_EndDate == data.date2 && a.ASMAY_Id.Equals(data.selectedyearlist) && b.PRYRA_Id.Equals(data.selectedactivitylist) && c.PRMTLE_Id.Equals(data.selectedlevellist) && d.PRMTY_Id.Equals(data.selectedtypelist))
                //                   select new OnlineProgramReport_DTO
                //                   {
                //                    ASMAY_Year=e.ASMAY_Year,
                //                       ASMAY_Id = e.ASMAY_Id,

                //                       PRYR_ProgramName = a.PRYR_ProgramName,
                //                       PRYRA_ActivityName = b.PRYRA_ActivityName,
                //                       PRMTLE_ProgramLevel = c.PRMTLE_ProgramLevel,
                //                       PRMTY_ProgramType = d.PRMTY_ProgramType,
                //                       date1 = a.PRYR_StartDate,
                //                       date2 = a.PRYR_EndDate.Value,
                //                       PRYR_StartTime = a.PRYR_StartTime,
                //                       PRYR_EndTime = a.PRYR_EndTime,
                //                       PRYR_TotalParticipants = a.PRYR_TotalParticipants.Value,


                //                   }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
    }
}
