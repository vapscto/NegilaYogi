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
    public class StaffReplacementFromExistingToNewImpl : Interfaces.StaffReplacementFromExistingToNewInterface
    {
        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public StaffReplacementFromExistingToNewImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public StaffReplacementFromExistingToNewDTO savedetail(StaffReplacementFromExistingToNewDTO _category)
        {
            StaffReplacementFromExistingToNewDTO objpge = Mapper.Map<StaffReplacementFromExistingToNewDTO>(_category);
            var id = _ttcontext.TT_Final_GenerationDMO.Single(t => t.TTMC_Id == objpge.TTMC_Id && t.ASMAY_Id == objpge.ASMAY_Id);
            var result = _ttcontext.TT_Final_Generation_DetailedDMO.Where(t => t.ASMCL_Id == objpge.ASMCL_Id && t.ASMS_Id == objpge.ASMS_Id && t.ISMS_Id == objpge.ISMS_Id && t.TTFG_Id == id.TTFG_Id && t.HRME_Id==objpge.staf_from).ToList();
            if (result.Count() == 0)
            {
                _category.returnduplicatestatus = "Duplicate";
            }
            else
            {
                var res = _ttcontext.TT_Final_Generation_DetailedDMO.Where(t => t.ASMCL_Id == objpge.ASMCL_Id && t.ASMS_Id == objpge.ASMS_Id && t.ISMS_Id == objpge.ISMS_Id && t.HRME_Id == objpge.staf_from).ToList();
                foreach(TT_Final_Generation_DetailedDMO dmo in res)
                {
                    dmo.HRME_Id = objpge.staf_to;
                    dmo.UpdatedDate = DateTime.Now;
                    _ttcontext.Update(dmo);
                    var contactExists = _ttcontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        _category.returnval = true;
                    }
                    else
                    {
                        _category.returnval = false;
                    }

                }
              
               

            }

            return _category;
        }

        public StaffReplacementFromExistingToNewDTO getdetails(int id)
           {
            StaffReplacementFromExistingToNewDTO TTMB = new StaffReplacementFromExistingToNewDTO();
            try
            {

                List<TTMasterCategoryDMO> category = new List<TTMasterCategoryDMO>();
                category = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id == id && t.TTMC_ActiveFlag == true).OrderBy(t => t.TTMC_Id).ToList();
                TTMB.categorylist = category.ToArray();

                List<School_M_Class> admcls = new List<School_M_Class>();
                admcls = _ttcontext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList();
                TTMB.clsdrp = admcls.ToArray();

                List<School_M_Section> section = new List<School_M_Section>();
                section = _ttcontext.School_M_Section.Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).ToList();
                TTMB.secdrp = section.ToArray();

                List<IVRM_School_Master_SubjectsDMO> sub = new List<IVRM_School_Master_SubjectsDMO>();
                sub = _ttcontext.IVRM_School_Master_SubjectsDMO.Where(t => t.MI_Id == id && t.ISMS_ActiveFlag == 1).ToList();
                TTMB.subdrp = sub.ToArray();

                TTMB.staffDrpDwn = (from e in _ttcontext.HR_Master_Employee_DMO
                                    from TT_Master_Staff_AbbreviationDMO in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                    where (e.MI_Id.Equals(id) && e.HRME_ActiveFlag.Equals(true) && TT_Master_Staff_AbbreviationDMO.HRME_Id == e.HRME_Id)
                                    select new StaffReplacementFromExistingToNewDTO
                                    {
                                        HRME_Id = e.HRME_Id,
                                        staffNamelst = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                    }
                                    ).ToArray();


                TTMB.view = (from a in _ttcontext.TT_Master_DayDMO
                             from b in _ttcontext.TT_Master_PeriodDMO
                             from c in _ttcontext.School_M_Class
                             from d in _ttcontext.School_M_Section
                             from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                             from f in _ttcontext.HR_Master_Employee_DMO
                             from g in _ttcontext.TT_Final_GenerationDMO
                             from h in _ttcontext.TT_Final_Generation_DetailedDMO
                             from ii in _ttcontext.TTMasterCategoryDMO
                             where (g.MI_Id == id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && ii.MI_Id == g.MI_Id && ii.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.ISMS_Id ==e.ISMS_Id  && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMCL_Id == c.ASMCL_Id )
                             select new StaffReplacementFromExistingToNewDTO
                             {
                                 TTMC_CategoryName = ii.TTMC_CategoryName,
                                 ASMCL_ClassName = c.ASMCL_ClassName,
                                 ASMC_SectionName = d.ASMC_SectionName,
                                 HRME_EmployeeFirstName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " :f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                 ISMS_SubjectName = e.ISMS_SubjectName,
                                 TTMP_PeriodName = b.TTMP_PeriodName
                             }).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMB;

        }

    }
}
