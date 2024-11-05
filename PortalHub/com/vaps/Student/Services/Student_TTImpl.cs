using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using AutoMapper;
using DomainModel.Model.com.vapstech.TT;
using PreadmissionDTOs.com.vaps.TT;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DataAccessMsSqlServerProvider;

namespace PortalHub.com.vaps.Student.Services
{
    public class Student_TTImpl : Interfaces.Student_TTInterface
    {
        private static ConcurrentDictionary<string, StudentDashboardDTO> _login =
           new ConcurrentDictionary<string, StudentDashboardDTO>();
        private PortalContext _Poralcontext;
        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;
        public Student_TTImpl(PortalContext Portalcontext, TTContext ttcntx)
        {
            _Poralcontext = Portalcontext;
            _ttcontext = ttcntx;
        }

        public StudentDashboardDTO getloaddata(StudentDashboardDTO data)
        {
            try
            {
                //  data.getyear = _Poralcontext.AcademicYearDMO.Where(y => y.ASMAY_Id == data.ASMAY_Id && y.MI_Id == data.MI_Id).Distinct().ToArray();

                data.getyear = (from d in _Poralcontext.AcademicYearDMO
                                from a in _Poralcontext.School_M_Class
                                from b in _Poralcontext.School_M_Section
                                from c in _Poralcontext.School_Adm_Y_StudentDMO
                                where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                select new ExamDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id,
                                    ASMCL_ClassName = a.ASMCL_ClassName,
                                    ASMS_Id = c.ASMS_Id,
                                    ASMC_SectionName = b.ASMC_SectionName,
                                    ASMAY_Id = c.ASMAY_Id,
                                    ASMAY_Year = d.ASMAY_Year
                                }
                           ).OrderBy(y => y.ASMAY_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public async Task<StudentDashboardDTO> getStudentTT(StudentDashboardDTO data)
        {
            // TTClassWiseTTDTO objpge = Mapper.Map<TTClassWiseTTDTO>(data);
            List<StudentDashboardDTO> list = new List<StudentDashboardDTO>();

            try
            {
                var clssec1 = (from a in _Poralcontext.Adm_M_Student
                               from b in _Poralcontext.School_Adm_Y_StudentDMO
                               from c in _Poralcontext.School_M_Class
                               from s in _Poralcontext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new StudentDashboardDTO
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();

                data.alldata = clssec1.ToArray();

                var temp_class = clssec1.FirstOrDefault().ASMCL_Id;
                var temp_section = clssec1.FirstOrDefault().ASMS_Id;


                var cateid = _ttcontext.TT_Category_Class_DMO.Where(c => c.ASMCL_Id == temp_class && c.ASMAY_Id == data.ASMAY_Id && c.TTCC_ActiveFlag == true).Distinct().ToList();

                long temp_cate = cateid.FirstOrDefault().TTMC_Id;

                List<TT_Master_PeriodDMO> allperiods = new List<TT_Master_PeriodDMO>();
                //allperiods = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList();

                allperiods = (from a in _ttcontext.TT_Master_PeriodDMO
                              from b in _ttcontext.TT_Master_Period_ClasswiseDMO
                              where a.MI_Id == data.MI_Id && a.TTMP_Id == b.TTMP_Id && a.TTMP_ActiveFlag == true && b.TTMPC_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == temp_class
                              select a).Distinct().ToList();


                data.periodslst = allperiods.ToArray();

                data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();
                List<StudentDashboardDTO> breaks = new List<StudentDashboardDTO>();
                List<TTBreakTimeSettingsDMO> breaks_all = new List<TTBreakTimeSettingsDMO>();


                data.getStudentTT = (from a in _ttcontext.TT_Master_DayDMO
                                     from b in _ttcontext.TT_Master_PeriodDMO
                                     from c in _ttcontext.School_M_Class
                                     from d in _ttcontext.School_M_Section
                                     from e in _ttcontext.TT_Master_Subject_AbbreviationDMO
                                     from f in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from g in _ttcontext.TT_Final_GenerationDMO
                                     from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                     from ii in _ttcontext.TTMasterCategoryDMO
                                     where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && ii.MI_Id == g.MI_Id && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.ASMCL_Id == temp_class && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMS_Id == temp_section && g.ASMAY_Id == data.ASMAY_Id && g.TTMC_Id == temp_cate && e.TTMSUAB_ActiveFlag == true && f.TTMSAB_ActiveFlag == true)
                                     select new StudentDashboardDTO
                                     {
                                         TTFGD_Id = h.TTFGD_Id,
                                         TTFG_Id = g.TTFG_Id,
                                         ASMCL_Id = h.ASMCL_Id,
                                         ASMS_Id = h.ASMS_Id,
                                         HRME_Id = h.HRME_Id,
                                         ISMS_Id = h.ISMS_Id,
                                         TTMD_Id = h.TTMD_Id,
                                         TTMP_Id = h.TTMP_Id,
                                         TTMC_Id = g.TTMC_Id,
                                         TTMD_DayName = a.TTMD_DayName,
                                         TTMP_PeriodName = b.TTMP_PeriodName,
                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                         ASMC_SectionName = d.ASMC_SectionName,
                                         staffName = f.TTMSAB_Abbreviation,
                                         ISMS_SubjectName = e.TTMSUAB_Abbreviation,
                                         TTMC_CategoryName = ii.TTMC_CategoryName,
                                     }
                              ).Distinct().ToArray();


                foreach (StudentDashboardDTO dto in data.getStudentTT)
                {
                    list.Add(dto);
                }

                data.Break_list = (from a in _ttcontext.TTBreakTimeSettingsDMO
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == temp_class && a.TTMB_ActiveFlag == true && a.TTMC_Id == temp_cate)
                                   select new StudentDashboardDTO
                                   {
                                       ASMCL_Id = a.ASMCL_Id,
                                       TTMB_AfterPeriod = a.TTMB_AfterPeriod,
                                       type = a.TTMB_BreakName
                                   }).Distinct().OrderBy(x => x.TTMB_AfterPeriod).ToArray();

                List<TTBreakTimeSettingsDMO> break_cls = new List<TTBreakTimeSettingsDMO>();
                break_cls = _ttcontext.TTBreakTimeSettingsDMO.AsNoTracking().Where(b => b.ASMCL_Id == temp_class && b.ASMAY_Id == data.ASMAY_Id && b.TTMC_Id == temp_cate && b.MI_Id == data.MI_Id && b.TTMB_ActiveFlag == true).OrderBy(x => x.TTMB_AfterPeriod).ToList();
                data.Break_list_all = break_cls.ToArray();


                foreach (StudentDashboardDTO dto in data.Break_list)
                {
                    breaks.Add(dto);
                }
                foreach (TTBreakTimeSettingsDMO dmo in data.Break_list_all)
                {
                    breaks_all.Add(dmo);
                }


                data.TT = list.ToArray();
                data.TT_Break_list = breaks.ToArray();
                data.TT_Break_list_all = breaks_all.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
