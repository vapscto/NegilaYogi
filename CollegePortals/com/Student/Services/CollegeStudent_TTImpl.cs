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
using AutoMapper;
using DomainModel.Model.com.vapstech.TT;
using PreadmissionDTOs.com.vaps.TT;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DataAccessMsSqlServerProvider;
using CollegePortals.com.Student.Interfaces;
using PreadmissionDTOs.com.vaps.College.Student;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;

namespace CollegePortals.com.Student.Services
{
    public class CollegeStudent_TTImpl : Interfaces.CollegeStudent_TTInterface
    {
        private static ConcurrentDictionary<string, CollegeStudent_TTDTO> _login =
           new ConcurrentDictionary<string, CollegeStudent_TTDTO>();
        private CollegeportalContext _ClgPortalContext;
        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;
        public CollegeStudent_TTImpl(CollegeportalContext ClgPortalContext, TTContext ttcntx)
        {
            _ClgPortalContext = ClgPortalContext;
            _ttcontext = ttcntx;
        }

        public CollegeStudent_TTDTO getloaddata(CollegeStudent_TTDTO data)
        {
            try
            {

                data.getyear = (from d in _ClgPortalContext.academicYearDMO
                                from a in _ClgPortalContext.MasterCourseDMO
                                from e in _ClgPortalContext.ClgMasterBranchDMO
                                from f in _ClgPortalContext.CLG_Adm_Master_SemesterDMO
                                from b in _ClgPortalContext.Adm_College_Master_SectionDMO
                                from c in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                where ( a.AMCO_Id == c.AMCO_Id && e.AMB_Id == c.AMB_Id && f.AMSE_Id == c.AMSE_Id && b.ACMS_Id == c.ACMS_Id && d.ASMAY_Id == c.ASMAY_Id && c.AMCST_Id == data.AMCST_Id &&
                                a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                select new ExamDTO
                                {
                                    ASMCL_Id = e.AMB_Id,
                                    ASMCL_ClassName = e.AMB_BranchName,
                                    ASMS_Id = b.ACMS_Id,
                                    ASMC_SectionName = b.ACMS_SectionName,
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



        public async Task<CollegeStudent_TTDTO> getStudentTT(CollegeStudent_TTDTO data)
        {
            // TTClassWiseTTDTO objpge = Mapper.Map<TTClassWiseTTDTO>(data);
            List<CollegeStudent_TTDTO> list = new List<CollegeStudent_TTDTO>();

            try
            {
                var clssec1 = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                               from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                               from c in _ClgPortalContext.MasterCourseDMO
                               from d in _ClgPortalContext.ClgMasterBranchDMO
                               from e in _ClgPortalContext.CLG_Adm_Master_SemesterDMO
                               from s in _ClgPortalContext.Adm_College_Master_SectionDMO
                               where (b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.ACMS_Id == s.ACMS_Id && e.AMSE_Id == b.AMSE_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.AMCST_Id == b.AMCST_Id 
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id == data.AMCST_Id && b.AMCST_Id == data.AMCST_Id)
                               select new CollegeStudent_TTDTO
                               {   
                                   AMCO_Id = c.AMCO_Id,
                                   ASMCL_Id = d.AMB_Id,
                                   ASMCL_ClassName = d.AMB_BranchName,
                                   ASMS_Id = s.ACMS_Id,
                                   ASMC_SectionName = s.ACMS_SectionName,
                                   AMSE_Id = e.AMSE_Id
                               }).Distinct().ToList();

                data.alldata = clssec1.ToArray();

                var temp_course = clssec1.FirstOrDefault().AMCO_Id;
                var temp_class = clssec1.FirstOrDefault().ASMCL_Id;
                var temp_section = clssec1.FirstOrDefault().ASMS_Id;
                var temp_Sem = clssec1.FirstOrDefault().AMSE_Id;


                var cateid = _ttcontext.CLGTT_Category_CourseBranchDMO.Where(c => c.AMCO_Id == temp_course && c.AMB_Id == temp_class && c.ASMAY_Id == data.ASMAY_Id && c.TTCC_ActiveFlag == true).Distinct().ToList();

                long temp_cate = cateid.FirstOrDefault().TTMC_Id;

                List<TT_Master_PeriodDMO> allperiods = new List<TT_Master_PeriodDMO>();
                //allperiods = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList();

                allperiods = (from a in _ttcontext.TT_Master_PeriodDMO
                              from b in _ttcontext.ClgPeriodAllocation_Course_DMO
                              where a.MI_Id == data.MI_Id && a.TTMP_Id == b.TTMP_Id && a.TTMP_ActiveFlag == true && b.TTMPC_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == temp_course && b.AMB_Id == temp_class && b.AMSE_Id == temp_Sem
                              select a).Distinct().ToList();


                data.periodslst = allperiods.ToArray();

                data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();
                List<CollegeStudent_TTDTO> breaks = new List<CollegeStudent_TTDTO>();
                List<CLGTT_Master_BreakDMO> breaks_all = new List<CLGTT_Master_BreakDMO>();


                data.getStudentTT = (from a in _ttcontext.TT_Master_DayDMO
                                     from b in _ttcontext.TT_Master_PeriodDMO
                                     from c in _ttcontext.MasterCourseDMO
                                     from br in _ttcontext.ClgMasterBranchDMO
                                     from sem in _ttcontext.CLG_Adm_Master_SemesterDMO
                                     from d in _ttcontext.Adm_College_Master_SectionDMO
                                     from e in _ttcontext.TT_Master_Subject_AbbreviationDMO
                                     from f in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                     from g in _ttcontext.TT_Final_GenerationDMO
                                     from h in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                     from ii in _ttcontext.TTMasterCategoryDMO
                                     where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && br.MI_Id == g.MI_Id && sem.MI_Id == g.MI_Id && 
                                     d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && ii.MI_Id == g.MI_Id && c.AMCO_Id == h.AMCO_Id && br.AMB_Id == h.AMB_Id && 
                                     d.ACMS_Id == h.ACMS_Id && sem.AMSE_Id == h.AMSE_Id && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.AMCO_Id == h.AMCO_Id && 
                                     d.ACMS_Id == h.ACMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.AMB_Id == temp_class && a.TTMD_Id == h.TTMD_Id && 
                                     b.TTMP_Id == h.TTMP_Id && c.AMCO_Id == temp_course && br.AMB_Id == temp_class && sem.AMSE_Id == temp_Sem && h.ACMS_Id == temp_section && g.ASMAY_Id == data.ASMAY_Id && g.TTMC_Id == temp_cate && e.TTMSUAB_ActiveFlag == true &&
                                     f.TTMSAB_ActiveFlag == true)
                                     select new CollegeStudent_TTDTO
                                     {
                                         TTFGD_Id = h.TTFGDC_Id,
                                         TTFG_Id = g.TTFG_Id,
                                         ASMCL_Id = h.AMB_Id,
                                         ASMS_Id = h.ACMS_Id,
                                         HRME_Id = h.HRME_Id,
                                         ISMS_Id = h.ISMS_Id,
                                         TTMD_Id = h.TTMD_Id,
                                         TTMP_Id = h.TTMP_Id,
                                         TTMC_Id = g.TTMC_Id,
                                         TTMD_DayName = a.TTMD_DayName,
                                         TTMP_PeriodName = b.TTMP_PeriodName,
                                         ASMCL_ClassName = c.AMCO_CourseName,
                                         ASMC_SectionName = d.ACMS_SectionName,
                                         staffName = f.TTMSAB_Abbreviation,
                                         ISMS_SubjectName = e.TTMSUAB_Abbreviation,
                                         TTMC_CategoryName = ii.TTMC_CategoryName,
                                     }
                              ).Distinct().ToArray();


                foreach (CollegeStudent_TTDTO dto in data.getStudentTT)
                {
                    list.Add(dto);
                }

                data.Break_list = (from a in _ttcontext.CLGTT_Master_BreakDMO
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == temp_course && a.AMB_Id == temp_class && a.AMSE_Id == temp_Sem && a.TTMBC_ActiveFlag == true && a.TTMC_Id == temp_cate)
                                   select new CollegeStudent_TTDTO
                                   {
                                       ASMCL_Id = a.AMB_Id,
                                       TTMBC_AfterPeriod = a.TTMBC_AfterPeriod,
                                       type = a.TTMBC_BreakName
                                   }).Distinct().OrderBy(x => x.TTMBC_AfterPeriod).ToArray();

                List<CLGTT_Master_BreakDMO> break_cls = new List<CLGTT_Master_BreakDMO>();
                break_cls = _ttcontext.CLGTT_Master_BreakDMO.AsNoTracking().Where(b => b.AMCO_Id == temp_course && b.AMB_Id == temp_class && b.AMSE_Id == temp_Sem && b.ASMAY_Id == data.ASMAY_Id && b.TTMC_Id == temp_cate && b.MI_Id == data.MI_Id && b.TTMBC_ActiveFlag == true).OrderBy(x => x.TTMBC_AfterPeriod).ToList();
                data.Break_list_all = break_cls.ToArray();


                foreach (CollegeStudent_TTDTO dto in data.Break_list)
                {
                    breaks.Add(dto);
                }
                foreach (CLGTT_Master_BreakDMO dmo in data.Break_list_all)
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
