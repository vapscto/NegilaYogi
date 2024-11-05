using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VisitorsManagement;

namespace VisitorsManagementServiceHub.Services
{
    public class StudentLateInReportImpl:Interfaces.StudentLateInReportInterface
    {
        DomainModelMsSqlServerContext _db;
        public VisitorsManagementContext visctxt;
        public StudentLateInReportImpl(DomainModelMsSqlServerContext apera, VisitorsManagementContext pare)
        {
            _db = apera;
            visctxt = pare;
        }

        public LateInStudent_DTO loaddata(LateInStudent_DTO data)
        {
            try
            {
                data.academicYear = _db.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_ActiveFlag == 1).Distinct().ToArray();
                data.classList = _db.School_M_Class.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).Distinct().ToArray();
                data.sectionList = _db.School_M_Section.Where(d => d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1).Distinct().ToArray();
                var q = (from a in visctxt.month
                         where (a.Is_Active == true)
                         select new
                         {
                             monthid = a.IVRM_Month_Id,
                             monthname = a.IVRM_Month_Name,
                         }).Distinct().ToArray();

                var query = q.Distinct().ToArray();
                data.month_list = (from a in query
                                   select new LateInStudent_DTO
                                   {
                                       monthid = Convert.ToInt32(a.monthid),
                                       monthname = a.monthname
                                   }).Distinct().OrderBy(t => t.monthid).ToArray();

                //var q = (from a in visctxt.holidaydate
                //         where (a.MI_Id == data.MI_Id && a.FOMHWD_ActiveFlg == true)
                //         select new
                //         {
                //             monthid = a.FOMHWDD_FromDate.Value.Month,
                //             monthname = Convert.ToDateTime(a.FOMHWDD_FromDate).ToString("MMMMM").ToString()
                //         }).Distinct().ToArray();

                //var query = q.Distinct().ToArray();
                //data.month_list = (from a in query
                //                  select new LateInStudent_DTO
                //                  {
                //                      monthid = a.monthid,
                //                      monthname = a.monthname
                //                  }).Distinct().OrderBy(t => t.monthid).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }

        public async Task<LateInStudent_DTO> getReport(LateInStudent_DTO data)
        {
            try
            {
                try
                {

                    string classs_ids = "0";
                    string section_idss = "0";


                    List<long> clss_ids = new List<long>();
                    List<long> section_ids = new List<long>();


                    if (data.Type == "CS")
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

                    if (data.all1 == "1")
                    {
                        data.month_id = "";
                    }
                    else
                    {
                        data.AttendanceFromDate = "";
                        data.AttendanceToDate = "";
                    }

                    using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Student_LateIn_Report_New";
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
                        cmd.Parameters.Add(new SqlParameter("@Type",
                       SqlDbType.VarChar)
                        {
                            Value = data.Type
                        });
                        cmd.Parameters.Add(new SqlParameter("@AttendanceFromDate",
                      SqlDbType.VarChar)
                        {
                            Value = data.AttendanceFromDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@AttendanceToDate",
                      SqlDbType.VarChar)
                        {
                            Value = data.AttendanceToDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@months",
                     SqlDbType.VarChar)
                        {
                            Value = data.month_id
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
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

 
    }
}
