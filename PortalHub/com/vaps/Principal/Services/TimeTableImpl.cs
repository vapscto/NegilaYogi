using DataAccessMsSqlServerProvider.com.vapstech.TT;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.TT;
using DomainModel.Model.com.vapstech.TT;

namespace PortalHub.com.vaps.Principal.Services
{
    public class TimeTableImpl : Interfaces.TimeTableInterface
    {
        public TTContext _tt;

        public TimeTableImpl(TTContext tt)
        {
            _tt = tt;
        }
        public TimeTableDTO getdata(TimeTableDTO dto)
        {
            try
            {
                dto.TT_final_generation = (from a in _tt.TT_Final_GenerationDMO
                                           from b in _tt.TT_Final_Generation_DetailedDMO
                                           from c in _tt.School_M_Class
                                           from d in _tt.School_M_Section
                                           from e in _tt.TT_Master_PeriodDMO
                                           from f in _tt.TT_Master_DayDMO

                                           where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id && a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                           select new TimeTableDTO
                                           {
                                               DayName = f.TTMD_DayName,
                                               PeriodCount = e.TTMP_PeriodName.Count()

                                           }).Distinct().GroupBy(f => f.DayName).Select(g => new TimeTableDTO { DayName = g.Key, PeriodCount = g.Count() }).ToArray();

                dto.stafflist = (from HR_Master_Employee_DMO in _tt.HR_Master_Employee_DMO
                                 from staffAbbr in _tt.TT_Master_Staff_AbbreviationDMO
                                 where (HR_Master_Employee_DMO.MI_Id.Equals(dto.MI_Id) && HR_Master_Employee_DMO.HRME_ActiveFlag.Equals(true) && staffAbbr.HRME_Id == HR_Master_Employee_DMO.HRME_Id)
                                 select new TimeTableDTO
                                 {
                                     HRME_Id = HR_Master_Employee_DMO.HRME_Id,
                                     FirstName = HR_Master_Employee_DMO.HRME_EmployeeFirstName,
                                     MiddleName = HR_Master_Employee_DMO.HRME_EmployeeMiddleName,
                                     LastName = HR_Master_Employee_DMO.HRME_EmployeeLastName
                                     //  staffName = HR_Master_Employee_DMO.HRME_EmployeeFirstName + HR_Master_Employee_DMO.HRME_EmployeeMiddleName + HR_Master_Employee_DMO.HRME_EmployeeLastName
                                 }
                                  ).Distinct().OrderBy(r=> r.FirstName).ToArray();


                dto.allperiods = _tt.TT_Master_DayDMO.Where(c => c.TTMD_ActiveFlag == true && c.MI_Id == dto.MI_Id).ToArray();

                dto.periods = _tt.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(dto.MI_Id)).ToArray();

                dto.class_sectons = (from a in _tt.TT_Final_GenerationDMO
                                     from b in _tt.TT_Final_Generation_DetailedDMO
                                     from c in _tt.School_M_Class
                                     from d in _tt.School_M_Section
                                     from e in _tt.TT_Master_PeriodDMO
                                     from f in _tt.TT_Master_DayDMO
                                     where (a.ASMAY_Id == dto.ASMAY_Id && a.TTFG_Id == b.TTFG_Id && a.MI_Id == dto.MI_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && c.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id)
                                     select new TimeTableDTO
                                     {
                                         P_Days = f.TTMD_DayName,
                                         Period = e.TTMP_PeriodName,
                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                         ASMC_SectionName = d.ASMC_SectionName

                                     }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public TimeTableDTO getdaily_data(TimeTableDTO dto)
        {
            try
            {
                if(dto.type== "daily")
                {
                    dto.TT_final_generation = (from a in _tt.TT_Final_GenerationDMO
                                               from b in _tt.TT_Final_Generation_DetailedDMO
                                               from c in _tt.School_M_Class
                                               from d in _tt.School_M_Section
                                               from e in _tt.TT_Master_PeriodDMO
                                               from f in _tt.TT_Master_DayDMO

                                               where (a.TTFG_Id == b.TTFG_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && a.ASMAY_Id == dto.ASMAY_Id && a.MI_Id == dto.MI_Id && d.ASMC_ActiveFlag == 1 && b.HRME_Id == dto.HRME_Id)//&& f.TTMD_Id == dto.TTMD_Id
                                               select new TimeTableDTO
                                               {
                                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                                   ASMC_SectionName = d.ASMC_SectionName,
                                                   DayName = f.TTMD_DayName,
                                                   Period = e.TTMP_PeriodName,
                                                   PeriodCount = e.TTMP_PeriodName.Count()


                                               }).Distinct().GroupBy(c => new { c.ASMCL_ClassName, c.ASMC_SectionName }).Select(g => new TimeTableDTO { ASMCL_ClassName = g.Key.ASMCL_ClassName, ASMC_SectionName = g.Key.ASMC_SectionName, PeriodCount = g.Count() }).ToArray();


                    dto.class_sectons = (from a in _tt.TT_Final_GenerationDMO
                                         from b in _tt.TT_Final_Generation_DetailedDMO
                                         from c in _tt.School_M_Class
                                         from d in _tt.School_M_Section
                                         from e in _tt.TT_Master_PeriodDMO
                                         from f in _tt.TT_Master_DayDMO
                                         where (a.TTFG_Id == b.TTFG_Id && b.TTMP_Id == e.TTMP_Id && b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && b.TTMD_Id == f.TTMD_Id && d.ASMC_ActiveFlag == 1 && b.HRME_Id == dto.HRME_Id && a.ASMAY_Id == dto.ASMAY_Id && f.TTMD_Id == dto.TTMD_Id && a.MI_Id == dto.MI_Id)
                                         select new TimeTableDTO
                                         {
                                             //P_Days = f.TTMD_DayName,
                                             Period = e.TTMP_PeriodName,
                                             ASMCL_ClassName = c.ASMCL_ClassName,
                                             ASMC_SectionName = d.ASMC_SectionName

                                         }).Distinct().ToArray();
                }
                else if(dto.type== "weekly")
                {
                    
                    List<TTStaffWiseTTDTO> list = new List<TTStaffWiseTTDTO>();
                  
                        List<TT_Master_PeriodDMO> allperiods = new List<TT_Master_PeriodDMO>();
                        allperiods = _tt.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(dto.MI_Id)).ToList();
                        dto.periodslst = allperiods.ToArray();

                        dto.gridweeks = _tt.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

                       // for (int i = 0; i < objpge.staffarray.Count(); i++)
                      //  {
                            var temp_stfid = dto.HRME_Id;
                       var Time_Table = (from a in _tt.TT_Master_DayDMO
                                                 from b in _tt.TT_Master_PeriodDMO
                                                 from c in _tt.School_M_Class
                                                 from d in _tt.School_M_Section
                                                 from e in _tt.IVRM_School_Master_SubjectsDMO
                                                 from f in _tt.HR_Master_Employee_DMO
                                                 from g in _tt.TT_Final_GenerationDMO
                                                 from h in _tt.TT_Final_Generation_DetailedDMO
                                                 from ii in _tt.TTMasterCategoryDMO
                                                 where (g.MI_Id == dto.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && ii.MI_Id == g.MI_Id && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.HRME_Id == temp_stfid && g.ASMAY_Id == dto.ASMAY_Id)
                                                 // && g.TTMC_Id == objpge.TTMC_Id
                                                 select new TTStaffWiseTTDTO
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
                                                     staffName = f.HRME_EmployeeFirstName,
                                                     ISMS_SubjectName = e.ISMS_SubjectName,
                                                     TTMC_CategoryName = ii.TTMC_CategoryName,

                                                 }
                                          ).Distinct().ToArray();

                            foreach (TTStaffWiseTTDTO za in Time_Table)
                            {
                                list.Add(za);
                            }
                    //  }
                    dto.TT = list.ToArray();

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
