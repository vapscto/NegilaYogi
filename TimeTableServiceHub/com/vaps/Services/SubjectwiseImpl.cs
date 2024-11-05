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
    public class SubjectwiseImpl : Interfaces.SubjectwiseInterface
    {

        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public SubjectwiseImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public TT_SubjectwiseDTO savedetail(TT_SubjectwiseDTO _category)
        {
            TT_SubjectwiseDTO objpge = Mapper.Map<TT_SubjectwiseDTO>(_category);
            List<TT_SubjectwiseDTO> list = new List<TT_SubjectwiseDTO>();

            try
            {
                List<TT_Master_PeriodDMO> allperiods = new List<TT_Master_PeriodDMO>();
                allperiods = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(objpge.MI_Id)).ToList();
                objpge.periodslst = allperiods.ToArray();

                objpge.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(objpge.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();

                for (int i = 0; i < objpge.subarray.Count(); i++)
                {
                    var temp_isms = objpge.subarray[i].ISMS_Id;
                    for (int j = 0; j < objpge.classarray.Count(); j++)
                    {
                        var temp_clsid = objpge.classarray[j].ASMCL_Id;
                        objpge.Time_Table = (from a in _ttcontext.TT_Master_DayDMO
                                             from b in _ttcontext.TT_Master_PeriodDMO
                                             from c in _ttcontext.School_M_Class
                                             from d in _ttcontext.School_M_Section
                                             from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                             from f in _ttcontext.HR_Master_Employee_DMO
                                             from g in _ttcontext.TT_Final_GenerationDMO
                                             from h in _ttcontext.TT_Final_Generation_DetailedDMO
                                             from ii in _ttcontext.TTMasterCategoryDMO
                                             where (g.MI_Id == objpge.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && ii.MI_Id == g.MI_Id && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.ISMS_Id == temp_isms && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMCL_Id == temp_clsid && g.ASMAY_Id == objpge.ASMAY_Id && g.TTFG_ActiveFlag==true)
                                             select new TT_SubjectwiseDTO
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
                                                 staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                                 ISMS_SubjectName = e.ISMS_SubjectName,
                                                 TTMC_CategoryName = ii.TTMC_CategoryName,

                                             }
                                      ).Distinct().ToArray();



                        foreach (TT_SubjectwiseDTO dto in objpge.Time_Table)
                        {
                            list.Add(dto);
                        }
                    }
                }
                objpge.TT = list.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return _category;
        }

        public TT_SubjectwiseDTO getdetails(int id)
        {
            TT_SubjectwiseDTO TTMB = new TT_SubjectwiseDTO();
            try
            {
                List<AcademicYear> Admyear = new List<AcademicYear>();
                Admyear = _ttcontext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(p=>p.ASMAY_Order).ToList();
                TTMB.year = Admyear.ToArray();

                List<School_M_Class> admcls = new List<School_M_Class>();
                admcls = _ttcontext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList();
                TTMB.clsdrp = admcls.ToArray();

                List<IVRM_School_Master_SubjectsDMO> sub = new List<IVRM_School_Master_SubjectsDMO>();
                sub = _ttcontext.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == id && t.ISMS_ActiveFlag == 1).ToList();
                TTMB.subdrp = sub.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMB;

        }



    }
}
