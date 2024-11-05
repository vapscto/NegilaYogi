using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DataAccessMsSqlServerProvider;
using PreadmissionDTOs.com.vaps.Sports;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace SportsServiceHub.com.vaps.Services
{
    public class YearEndReportImpl : Interfaces.YearEndReportInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;
        public YearEndReportImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }
        public YearEndReportDTO loadDrpDwn(YearEndReportDTO data)
        {
            try
            {
                data.academicYear = _db.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.Is_Active == true).Distinct().OrderByDescending(s => s.ASMAY_Order).ToArray();

                data.classList = _db.School_M_Class.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).Select(d => new YearEndReportDTO { ASMCL_Id = d.ASMCL_Id, className = d.ASMCL_ClassName }).ToArray();

                data.sectionList = _db.School_M_Section.Where(d => d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1).Select(d => new YearEndReportDTO { ASMS_Id = d.ASMS_Id, sectionName = d.ASMC_SectionName }).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<YearEndReportDTO> getReport(YearEndReportDTO report)
        {
            try
            {

                string classs_ids = "0";
                string section_idss = "0";


                List<long> clss_ids = new List<long>();
                List<long> section_ids = new List<long>();


                if (report.Type == "CS")
                {
                    foreach (var item in report.selectedClasslist)
                    {
                        clss_ids.Add(item.ASMCL_Id);
                    }
                    for (int s = 0; s < clss_ids.Count(); s++)
                    {
                        classs_ids = classs_ids + ',' + clss_ids[s].ToString();

                    }
                    foreach (var item in report.selectedSectionlist)
                    {
                        section_ids.Add(item.ASMS_Id);
                    }
                    for (int s = 0; s < section_ids.Count(); s++)
                    {
                        section_idss = section_idss + ',' + section_ids[s].ToString();
                    }


                }



                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Spc_Year_End_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = report.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = report.ASMAY_Id
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
                        Value = report.Type
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
                        report.yearEndReport = retObject.ToArray();

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
            return report;
        }
        public YearEndReportDTO getReportGraph(YearEndReportDTO report1)
        {
            try
            {
                List<YearEndReportDTO> list = new List<YearEndReportDTO>();
                if (report1.radioVal.Equals("hsvts"))
                {
                    list = (from m in _context.SportStudentHouseDivisionDMO
                            from n in _context.SportMasterHouseDMO
                            from o in _context.Adm_M_Student
                            from p in _context.admissionyearstudent

                            where (m.SPCCMH_Id == n.SPCCMH_Id && p.AMST_Id == m.AMST_Id && p.AMST_Id == o.AMST_Id && m.MI_Id == o.MI_Id && p.ASMAY_Id == m.ASMAY_Id && p.ASMCL_Id == m.ASMCL_Id && p.ASMS_Id == m.ASMS_Id && m.MI_Id == report1.MI_Id && p.ASMAY_Id == report1.ASMAY_Id && o.AMST_SOL.Equals("S") && o.AMST_ActiveFlag == 1 && p.AMAY_ActiveFlag == 1)
                            group new { m, n } by m.SPCCMH_Id into g
                            select new YearEndReportDTO
                            {
                                totalNo = g.Count(),
                                houseName = g.FirstOrDefault().n.SPCCMH_HouseName
                            }
                                 ).ToList();

                }
                else if (report1.radioVal.Equals("hsvwn"))
                {
                    list = (from m in _context.SportStudentHouseDivisionDMO
                            from n in _context.SportMasterHouseDMO
                            from o in _context.EventsStudentRecordDMO
                            from newtable in _context.SPCC_Events_Students_DMO
                            from p in _context.Adm_M_Student
                            from q in _context.admissionyearstudent
                            from e in _context.EventsMappingDMO
                            from ev in _context.MasterEventVenueDMO

                            where (m.SPCCMH_Id == n.SPCCMH_Id
                            && newtable.ASMAY_Id == m.ASMAY_Id /*&& newtable.ASMCL_Id == m.ASMCL_Id*/ /*&& newtable.ASMS_Id == m.ASMS_Id*/
                            && newtable.SPCCEST_Id == o.SPCCEST_Id && q.AMST_Id == o.AMST_Id && m.AMST_Id == o.AMST_Id && q.AMST_Id == p.AMST_Id
                            && q.ASMAY_Id == m.ASMAY_Id && q.ASMCL_Id == m.ASMCL_Id && q.ASMS_Id == m.ASMS_Id
                            && e.ASMAY_Id == newtable.ASMAY_Id && e.SPCCME_Id == newtable.SPCCME_Id
                            && m.ASMAY_Id == newtable.ASMAY_Id /*&& m.ASMCL_Id == newtable.ASMCL_Id*/
                            && ev.SPCCMEV_Id == e.SPCCMEV_Id && ev.SPCCMEV_ActiveFlag == true
                            && m.MI_Id == report1.MI_Id && q.ASMAY_Id == report1.ASMAY_Id && o.SPCCESTR_ActiveFlag == true && e.SPCCE_ActiveFlag == true
                            && p.AMST_SOL.Equals("S") && p.AMST_ActiveFlag == 1 && q.AMAY_ActiveFlag == 1
                            && Convert.ToInt32(o.SPCCESTR_Rank) <= 3)
                            group new { m, n } by m.SPCCMH_Id into g
                            select new YearEndReportDTO
                            {
                                totalNo = g.Count(),
                                houseName = g.FirstOrDefault().n.SPCCMH_HouseName
                            }).ToList();
                }
                else if (report1.radioVal.Equals("hsvtp"))
                {
                    list = (from m in _context.SportStudentHouseDivisionDMO
                            from n in _context.SportMasterHouseDMO
                            from o in _context.EventsStudentRecordDMO
                            from newtable in _context.SPCC_Events_Students_DMO
                            from p in _context.Adm_M_Student
                            from q in _context.admissionyearstudent
                            from e in _context.EventsMappingDMO
                            from ev in _context.MasterEventVenueDMO

                            where (m.SPCCMH_Id == n.SPCCMH_Id
                            && newtable.ASMAY_Id == m.ASMAY_Id /*&& newtable.ASMCL_Id == m.ASMCL_Id*/ /*&& newtable.ASMS_Id == m.ASMS_Id */
                            && newtable.SPCCEST_Id == o.SPCCEST_Id && q.AMST_Id == o.AMST_Id && m.AMST_Id == o.AMST_Id && q.AMST_Id == p.AMST_Id
                            && q.ASMAY_Id == m.ASMAY_Id && q.ASMCL_Id == m.ASMCL_Id && q.ASMS_Id == m.ASMS_Id
                            && e.ASMAY_Id == newtable.ASMAY_Id && e.SPCCME_Id == newtable.SPCCME_Id
                            && m.ASMAY_Id == newtable.ASMAY_Id /*&& m.ASMCL_Id==newtable.ASMCL_Id*/
                            && ev.SPCCMEV_Id == e.SPCCMEV_Id && ev.SPCCMEV_ActiveFlag == true
                            && m.MI_Id == report1.MI_Id && q.ASMAY_Id == report1.ASMAY_Id && o.SPCCESTR_ActiveFlag == true && e.SPCCE_ActiveFlag == true
                            && p.AMST_SOL.Equals("S") && p.AMST_ActiveFlag == 1 && q.AMAY_ActiveFlag == 1)
                            group new { m, n } by m.SPCCMH_Id into g
                            select new YearEndReportDTO
                            {
                                totalNo = g.Count(),
                                houseName = g.FirstOrDefault().n.SPCCMH_HouseName
                            }).ToList();
                }
                else if (report1.radioVal.Equals("divts"))
                {
                    //list = (from m in _context.SportStudentHouseDivisionDMO
                    //        from n in _context.SportMasterHouseDMO
                    //        from o in _context.Adm_M_Student
                    //        from p in _context.admissionyearstudent
                    //        where m.SPCCMH_Id == n.SPCCMH_Id && m.AMST_Id == o.AMST_Id && o.AMST_Id == p.AMST_Id && m.MI_Id == report1.MI_Id && m.ASMAY_Id == report1.ASMAY_Id && o.AMST_SOL.Equals("S") && o.AMST_ActiveFlag == 1 && p.AMAY_ActiveFlag == 1
                    //        group new { m } by m.SPCCMH_Id into g
                    //        select new YearEndReportDTO
                    //        {
                    //            totalNo = g.Count(),
                    //            //houseName = g.FirstOrDefault().n.SPCCMH_HouseName
                    //        }).ToList();
                }
                if (list.Count() > 0)
                {
                    report1.yearEndReport = list.ToArray();
                    report1.count = list.Count;
                }
                else
                {
                    report1.count = 0;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return report1;
        }
    }
}
