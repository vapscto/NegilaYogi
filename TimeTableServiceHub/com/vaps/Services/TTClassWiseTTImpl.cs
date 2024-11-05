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
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTClassWiseTTImpl : Interfaces.ClassWiseTTInterface
    {

        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public TTClassWiseTTImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public TTClassWiseTTDTO getreport(TTClassWiseTTDTO _category)
        {
            TTClassWiseTTDTO objpge = Mapper.Map<TTClassWiseTTDTO>(_category);
            List<TTClassWiseTTDTO> list = new List<TTClassWiseTTDTO>();

            try
            {
                List<TT_Master_PeriodDMO> allperiods = new List<TT_Master_PeriodDMO>();
                allperiods = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(objpge.MI_Id)).ToList();
                objpge.periodslst = allperiods.ToArray();

                objpge.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();
                List<TTClassWiseTTDTO> breaks = new List<TTClassWiseTTDTO>();
                List<TTBreakTimeSettingsDMO> breaks_all = new List<TTBreakTimeSettingsDMO>();
                for (int i = 0; i < objpge.classarray.Count(); i++)
                {
                    var temp_class = objpge.classarray[i].ASMCL_Id;
                    for (int j = 0; j < objpge.sectionarray.Count(); j++)
                    {
                        var temp_section = objpge.sectionarray[j].ASMS_Id;
                        objpge.Time_Table = (from a in _ttcontext.TT_Master_DayDMO
                                             from b in _ttcontext.TT_Master_PeriodDMO
                                             from c in _ttcontext.School_M_Class
                                             from d in _ttcontext.School_M_Section
                                             from e in _ttcontext.TT_Master_Subject_AbbreviationDMO
                                             from f in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                             from g in _ttcontext.TT_Final_GenerationDMO
                                             from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                             from ii in _ttcontext.TTMasterCategoryDMO
                                             where (g.MI_Id == objpge.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && ii.MI_Id == g.MI_Id && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.ASMCL_Id == temp_class && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMS_Id == temp_section && g.ASMAY_Id == objpge.ASMAY_Id && g.TTMC_Id == objpge.TTMC_Id && e.TTMSUAB_ActiveFlag==true && f.TTMSAB_ActiveFlag==true)
                                             select new TTClassWiseTTDTO
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


                        foreach (TTClassWiseTTDTO dto in objpge.Time_Table)
                        {
                            list.Add(dto);
                        }
                    }
                    objpge.Break_list = (from a in _ttcontext.TTBreakTimeSettingsDMO
                                         where (a.MI_Id == objpge.MI_Id && a.ASMAY_Id == objpge.ASMAY_Id && a.ASMCL_Id == temp_class && a.TTMB_ActiveFlag == true && a.TTMC_Id == objpge.TTMC_Id)
                                         select new TTClassWiseTTDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             TTMB_AfterPeriod = a.TTMB_AfterPeriod,
                                             TTMB_BreakName = a.TTMB_BreakName
                                         }).Distinct().OrderBy(x => x.TTMB_AfterPeriod).ToArray();

                    List<TTBreakTimeSettingsDMO> break_cls = new List<TTBreakTimeSettingsDMO>();
                    break_cls = _ttcontext.TTBreakTimeSettingsDMO.AsNoTracking().Where(b => b.ASMCL_Id == temp_class && b.ASMAY_Id == objpge.ASMAY_Id && b.TTMC_Id == objpge.TTMC_Id && b.MI_Id == objpge.MI_Id && b.TTMB_ActiveFlag == true).OrderBy(x => x.TTMB_AfterPeriod).ToList();
                    objpge.Break_list_all = break_cls.ToArray();


                    foreach (TTClassWiseTTDTO dto in objpge.Break_list)
                    {
                        breaks.Add(dto);
                    }
                    foreach (TTBreakTimeSettingsDMO dmo in objpge.Break_list_all)
                    {
                        breaks_all.Add(dmo);
                    }

                }
                objpge.TT = list.ToArray();
                objpge.TT_Break_list = breaks.ToArray();
                objpge.TT_Break_list_all = breaks_all.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return _category;
        }

        public TTClassWiseTTDTO getdetails(int id)
        {
            TTClassWiseTTDTO data = new TTClassWiseTTDTO();
            try
            {

                List<AcademicYear> year = new List<AcademicYear>();
                year = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == id && t.Is_Active == true).ToList();
                data.acayear = year.Distinct().OrderByDescending(l=>l.ASMAY_Order).ToArray();

                List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                mcat = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == id && t.TTMC_ActiveFlag == true).ToList();
                data.categorylist = mcat.Distinct().ToArray();

                List<School_M_Section> allsetion = new List<School_M_Section>();
                allsetion = _ttcontext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).ToList();
                data.sectionlist = allsetion.Distinct().ToArray();

                List<School_M_Class> admcls = new List<School_M_Class>();
                admcls = _ttcontext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList();
                data.classlist = admcls.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }

        public TTClassWiseTTDTO getclass_catg(TTClassWiseTTDTO data)
        {
            try
            {
                data.classlist = (from a in _ttcontext.TTMasterCategoryDMO
                                  from b in _ttcontext.TT_Category_Class_DMO
                                  from c in _ttcontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag==true
                                  select new TTClassWiseTTDTO
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
