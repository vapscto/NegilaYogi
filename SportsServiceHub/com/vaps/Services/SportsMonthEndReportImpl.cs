using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

namespace SportsServiceHub.com.vaps.Services
{
    public class SportsMonthEndReportImpl : Interfaces.SportsMonthEndReportInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;

        public SportsMonthEndReportImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }


        public SportsMonthEndReport_DTO getdeatils(SportsMonthEndReport_DTO data)
        {
            try
            {
                var q = (from a in _db.month
                         where (a.Is_Active == true)
                         select new
                         {
                             monthid = a.IVRM_Month_Id,
                             monthname = a.IVRM_Month_Name,
                         }).Distinct().ToArray();

                var query = q.Distinct().ToArray();
                data.fillmonth = (from a in query
                                  select new SportsMonthEndReport_DTO
                                  {
                                      month = Convert.ToInt32(a.monthid),
                                      monthname = a.monthname
                                  }).Distinct().OrderBy(t => t.month).ToArray();


                data.fillyear = (from a in _db.AcademicYear
                                 where (a.MI_Id == data.MI_Id && a.Is_Active == true)
                                 select new SportsMonthEndReport_DTO
                                 {
                                     ASMAY_Id = Convert.ToInt32(a.ASMAY_Id),
                                     ASMAY_Year = a.ASMAY_Year

                                 }).Distinct().OrderByDescending(s => s.ASMAY_Order).ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

        public SportsMonthEndReport_DTO GetReport(SportsMonthEndReport_DTO data)
        {
            try
            {
                var total_stud = (from a in _context.SportStudentHouseDivisionDMO
                                  from b in _context.Adm_M_Student
                                  where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMST_Id==a.AMST_Id && b.AMST_SOL=="S" && a.SPCCMH_ActiveFlag == true)
                                  select a).ToList();
                if (total_stud.Count > 0)
                {
                    data.total_count_student = total_stud.Count();
                    data.count = data.total_count_student;
                }
                else
                {
                    data.count = 0;
                }
                var total_event_of_month = (from a in _context.EventsMappingDMO
                                                //from b in _context.SPCC_Events_Students_DMO
                                            from c in _context.MasterEventsDMO
                                            where (a.SPCCME_Id == c.SPCCME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.SPCCE_StartDate.Value.Month == data.month && a.SPCCE_ActiveFlag == true)
                                            select c.SPCCME_Id).ToList();

                var participate_stud = (from a in _context.EventsStudentRecordDMO
                                        from b in _context.SPCC_Events_Students_DMO
                                        from c in _context.Adm_M_Student
                                        where (a.SPCCEST_Id == b.SPCCEST_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == a.AMST_Id && c.AMST_SOL == "S" && total_event_of_month.Contains(b.SPCCME_Id) && a.SPCCESTR_ActiveFlag == true)
                                        select a.AMST_Id).ToList();
                data.total_partic_student = participate_stud.Count();

                var winner_record_month = (from a in _context.EventsStudentRecordDMO
                                           from b in _context.SPCC_Events_Students_DMO
                                           from c in _context.Adm_M_Student
                                           where (a.SPCCEST_Id == b.SPCCEST_Id && a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == a.AMST_Id && c.AMST_SOL == "S" && total_event_of_month.Contains(b.SPCCME_Id) && a.SPCCESTR_ActiveFlag == true && Convert.ToInt32(a.SPCCESTR_Rank) <= 3)
                                           select a.AMST_Id).ToList();
                data.total_winner_student = winner_record_month.Count();

                data.not_patr_std = total_stud.Count() - participate_stud.Count();


            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

    }
}
