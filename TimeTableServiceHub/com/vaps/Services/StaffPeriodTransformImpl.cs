using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
namespace TimeTableServiceHub.com.vaps.Services
{
    public class StaffPeriodTransformImpl : Interfaces.StaffPeriodTransformInterface
    {

        private static ConcurrentDictionary<string, StaffPeriodTransformDTO> _login =
               new ConcurrentDictionary<string, StaffPeriodTransformDTO>();


        public TTContext _ttcategorycontext;
        public StaffPeriodTransformImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public StaffPeriodTransformDTO getdetails(int id)
        {
            StaffPeriodTransformDTO TTMC = new StaffPeriodTransformDTO();
            try
            {
                TTMC.academiclist = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(id) && t.Is_Active == true).OrderByDescending(u=>u.ASMAY_Order).ToList().ToArray();

                TTMC.catelist = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(id) && t.TTMC_ActiveFlag.Equals(true)).ToList().ToArray();
                TTMC.staffDrpDwn = (from b in _ttcategorycontext.HR_Master_Employee_DMO
                                    from a in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    from c in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                    from d in _ttcategorycontext.TT_Final_GenerationDMO
                                    where (b.MI_Id.Equals(id) //&& b.HRME_ActiveFlag.Equals(true)
                                    && a.HRME_Id == b.HRME_Id && a.TTMSAB_ActiveFlag == true && c.HRME_Id == a.HRME_Id && c.TTFG_Id == d.TTFG_Id && d.TTFG_ActiveFlag == true)
                                    select new StaffReplacementUnalocatedPeriodDTO
                                    {
                                        HRME_Id = b.HRME_Id,
                                        staffNamelst = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "  " || b.HRME_EmployeeMiddleName == "0" ? "  " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "  " || b.HRME_EmployeeLastName == "0" ? "  " : b.HRME_EmployeeLastName)
                                    }).Distinct().OrderBy(r=>r.staffNamelst).ToArray();

                TTMC.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(id)).ToList().ToArray();

            }
            catch (Exception ee)
            {
                TTMC.returnval = false;
            }
            return TTMC;

        }

        public StaffPeriodTransformDTO get_catg(StaffPeriodTransformDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new StaffPeriodTransformDTO
                                 {
                                     TTMC_Id = a.TTMC_Id,
                                     TTMC_CategoryName = a.TTMC_CategoryName,
                                 }
          ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public StaffPeriodTransformDTO getreport(StaffPeriodTransformDTO data)
        {
            try
            {
                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                data.gridweeks = _ttcategorycontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();
              

                data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                   from b in _ttcategorycontext.TT_Master_PeriodDMO
                                   from c in _ttcategorycontext.School_M_Class
                                   from d in _ttcategorycontext.School_M_Section
                                   from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                   from f in _ttcategorycontext.HR_Master_Employee_DMO
                                   from g in _ttcategorycontext.TT_Final_GenerationDMO
                                   from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                   from i in _ttcategorycontext.TTMasterCategoryDMO
                                   where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id)
                                   //&& g.TTMC_Id==data.TTMC_Id
                                   select new StaffPeriodTransformDTO
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
                                       ISMS_SubjectName = e.ISMS_SubjectName,
                                       staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == "  " || f.HRME_EmployeeMiddleName == "0" ? "  " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? "  " : f.HRME_EmployeeLastName),
                                       TTMC_CategoryName = i.TTMC_CategoryName,

                                   }
                              ).Distinct().ToArray();


                data.classdetails = (from a in _ttcategorycontext.TT_Final_GenerationDMO
                                    from b in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                    from c in _ttcategorycontext.School_M_Class
                                    from d in _ttcategorycontext.School_M_Section
                                    where a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.HRME_Id == data.HRME_Id && a.TTFG_ActiveFlag == true
                                    select c).Distinct().ToArray();
                data.secdetails = (from a in _ttcategorycontext.TT_Final_GenerationDMO
                                    from b in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                    from c in _ttcategorycontext.School_M_Class
                                    from d in _ttcategorycontext.School_M_Section
                                    where a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id==data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.HRME_Id == data.HRME_Id && a.TTFG_ActiveFlag == true
                                    select d).Distinct().ToArray();

                data.subjectdet = (from a in _ttcategorycontext.TT_Final_GenerationDMO
                                    from b in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                    from c in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                    where a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ISMS_Id == c.ISMS_Id  && b.HRME_Id == data.HRME_Id && a.TTFG_ActiveFlag == true
                                    select c).Distinct().ToArray();





                data.staffDrpDwnto = (from b in _ttcategorycontext.HR_Master_Employee_DMO
                                    from a in _ttcategorycontext.TT_Master_Staff_AbbreviationDMO
                                    where (b.MI_Id.Equals(data.MI_Id) && b.HRME_ActiveFlag.Equals(true) && a.HRME_Id == b.HRME_Id && a.TTMSAB_ActiveFlag == true && a.HRME_Id == a.HRME_Id && b.HRME_Id!=data.HRME_Id)
                                    select new StaffReplacementUnalocatedPeriodDTO
                                    {
                                        HRME_Id = b.HRME_Id,
                                        staffNamelst = b.HRME_EmployeeFirstName +" "+ (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "  " || b.HRME_EmployeeMiddleName == "0" ? "  " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "  " || b.HRME_EmployeeLastName == "0" ? "   " : b.HRME_EmployeeLastName)
                                    }).Distinct().OrderBy(t=>t.staffNamelst).ToArray();

            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }



            return data;

        }
        public StaffPeriodTransformDTO gettimetable(StaffPeriodTransformDTO data)
        {
            try
            {
                data.periodslst = _ttcategorycontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id.Equals(data.MI_Id)).ToList().ToArray();

                data.gridweeks = _ttcategorycontext.TT_Master_DayDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TTMD_ActiveFlag == true).ToArray();
               

                if (data.ASMCL_Id == 0 && data.ASMS_Id == 0 && data.ISMS_Id == 0)
                {
                    data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                       from b in _ttcategorycontext.TT_Master_PeriodDMO
                                       from c in _ttcategorycontext.School_M_Class
                                       from d in _ttcategorycontext.School_M_Section
                                       from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                       from f in _ttcategorycontext.HR_Master_Employee_DMO
                                       from g in _ttcategorycontext.TT_Final_GenerationDMO
                                       from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                       from i in _ttcategorycontext.TTMasterCategoryDMO
                                       where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id)
                                       //&& g.TTMC_Id==data.TTMC_Id
                                       select new StaffPeriodTransformDTO
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
                                           ISMS_SubjectName = e.ISMS_SubjectName,
                                           staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                           TTMC_CategoryName = i.TTMC_CategoryName,

                                       }
                              ).Distinct().ToArray();
                }
                else if (data.ASMCL_Id > 0 && data.ASMS_Id == 0 && data.ISMS_Id == 0)
                {
                    data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                       from b in _ttcategorycontext.TT_Master_PeriodDMO
                                       from c in _ttcategorycontext.School_M_Class
                                       from d in _ttcategorycontext.School_M_Section
                                       from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                       from f in _ttcategorycontext.HR_Master_Employee_DMO
                                       from g in _ttcategorycontext.TT_Final_GenerationDMO
                                       from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                       from i in _ttcategorycontext.TTMasterCategoryDMO
                                       where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMCL_Id==data.ASMCL_Id)
                                       //&& g.TTMC_Id==data.TTMC_Id
                                       select new StaffPeriodTransformDTO
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
                                           ISMS_SubjectName = e.ISMS_SubjectName,
                                           staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                           TTMC_CategoryName = i.TTMC_CategoryName,

                                       }
                             ).Distinct().ToArray();
                }
                else if (data.ASMCL_Id > 0 && data.ASMS_Id >0 && data.ISMS_Id == 0)
                {
                    data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                       from b in _ttcategorycontext.TT_Master_PeriodDMO
                                       from c in _ttcategorycontext.School_M_Class
                                       from d in _ttcategorycontext.School_M_Section
                                       from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                       from f in _ttcategorycontext.HR_Master_Employee_DMO
                                       from g in _ttcategorycontext.TT_Final_GenerationDMO
                                       from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                       from i in _ttcategorycontext.TTMasterCategoryDMO
                                       where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMCL_Id == data.ASMCL_Id &&  h.ASMS_Id == data.ASMS_Id)
                                       //&& g.TTMC_Id==data.TTMC_Id
                                       select new StaffPeriodTransformDTO
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
                                           ISMS_SubjectName = e.ISMS_SubjectName,
                                           staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                           TTMC_CategoryName = i.TTMC_CategoryName,

                                       }
                             ).Distinct().ToArray();
                }
                else if (data.ASMCL_Id > 0 && data.ASMS_Id == 0 && data.ISMS_Id > 0)
                {
                    data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                       from b in _ttcategorycontext.TT_Master_PeriodDMO
                                       from c in _ttcategorycontext.School_M_Class
                                       from d in _ttcategorycontext.School_M_Section
                                       from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                       from f in _ttcategorycontext.HR_Master_Employee_DMO
                                       from g in _ttcategorycontext.TT_Final_GenerationDMO
                                       from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                       from i in _ttcategorycontext.TTMasterCategoryDMO
                                       where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMCL_Id == data.ASMCL_Id && h.ISMS_Id == data.ISMS_Id)
                                       //&& g.TTMC_Id==data.TTMC_Id
                                       select new StaffPeriodTransformDTO
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
                                           ISMS_SubjectName = e.ISMS_SubjectName,
                                           staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                           TTMC_CategoryName = i.TTMC_CategoryName,

                                       }
                             ).Distinct().ToArray();
                }

                else if (data.ASMCL_Id > 0 && data.ASMS_Id > 0 && data.ISMS_Id > 0)
                {
                    data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                       from b in _ttcategorycontext.TT_Master_PeriodDMO
                                       from c in _ttcategorycontext.School_M_Class
                                       from d in _ttcategorycontext.School_M_Section
                                       from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                       from f in _ttcategorycontext.HR_Master_Employee_DMO
                                       from g in _ttcategorycontext.TT_Final_GenerationDMO
                                       from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                       from i in _ttcategorycontext.TTMasterCategoryDMO
                                       where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMCL_Id == data.ASMCL_Id && h.ASMS_Id == data.ASMS_Id && h.ISMS_Id == data.ISMS_Id)
                                       //&& g.TTMC_Id==data.TTMC_Id
                                       select new StaffPeriodTransformDTO
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
                                           ISMS_SubjectName = e.ISMS_SubjectName,
                                           staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                           TTMC_CategoryName = i.TTMC_CategoryName,

                                       }
                             ).Distinct().ToArray();
                }
                else if (data.ASMCL_Id == 0 && data.ASMS_Id > 0 && data.ISMS_Id ==  0)
                {
                    data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                       from b in _ttcategorycontext.TT_Master_PeriodDMO
                                       from c in _ttcategorycontext.School_M_Class
                                       from d in _ttcategorycontext.School_M_Section
                                       from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                       from f in _ttcategorycontext.HR_Master_Employee_DMO
                                       from g in _ttcategorycontext.TT_Final_GenerationDMO
                                       from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                       from i in _ttcategorycontext.TTMasterCategoryDMO
                                       where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id  && h.ASMS_Id == data.ASMS_Id )
                                       //&& g.TTMC_Id==data.TTMC_Id
                                       select new StaffPeriodTransformDTO
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
                                           ISMS_SubjectName = e.ISMS_SubjectName,
                                           staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                           TTMC_CategoryName = i.TTMC_CategoryName,

                                       }
                             ).Distinct().ToArray();
                }
                else if (data.ASMCL_Id == 0 && data.ASMS_Id > 0 && data.ISMS_Id > 0)
                {
                    data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                       from b in _ttcategorycontext.TT_Master_PeriodDMO
                                       from c in _ttcategorycontext.School_M_Class
                                       from d in _ttcategorycontext.School_M_Section
                                       from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                       from f in _ttcategorycontext.HR_Master_Employee_DMO
                                       from g in _ttcategorycontext.TT_Final_GenerationDMO
                                       from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                       from i in _ttcategorycontext.TTMasterCategoryDMO
                                       where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id && h.ASMS_Id == data.ASMS_Id && h.ISMS_Id == data.ISMS_Id)
                                       //&& g.TTMC_Id==data.TTMC_Id
                                       select new StaffPeriodTransformDTO
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
                                           ISMS_SubjectName = e.ISMS_SubjectName,
                                           staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                           TTMC_CategoryName = i.TTMC_CategoryName,

                                       }
                             ).Distinct().ToArray();
                }
                else if (data.ASMCL_Id == 0 && data.ASMS_Id == 0 && data.ISMS_Id > 0)
                {
                    data.Time_Table = (from a in _ttcategorycontext.TT_Master_DayDMO
                                       from b in _ttcategorycontext.TT_Master_PeriodDMO
                                       from c in _ttcategorycontext.School_M_Class
                                       from d in _ttcategorycontext.School_M_Section
                                       from e in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                       from f in _ttcategorycontext.HR_Master_Employee_DMO
                                       from g in _ttcategorycontext.TT_Final_GenerationDMO
                                       from h in _ttcategorycontext.TT_Final_Generation_DetailedDMO
                                       from i in _ttcategorycontext.TTMasterCategoryDMO
                                       where (g.MI_Id == data.MI_Id && a.MI_Id == g.MI_Id && b.MI_Id == g.MI_Id && c.MI_Id == g.MI_Id && d.MI_Id == g.MI_Id && e.MI_Id == g.MI_Id && f.MI_Id == g.MI_Id && i.MI_Id == g.MI_Id && i.TTMC_Id == g.TTMC_Id && h.TTFG_Id == g.TTFG_Id && g.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == h.ASMCL_Id && d.ASMS_Id == h.ASMS_Id && e.ISMS_Id == h.ISMS_Id && f.HRME_Id == h.HRME_Id && h.HRME_Id == data.HRME_Id && a.TTMD_Id == h.TTMD_Id && b.TTMP_Id == h.TTMP_Id  && h.ISMS_Id == data.ISMS_Id)
                                       //&& g.TTMC_Id==data.TTMC_Id
                                       select new StaffPeriodTransformDTO
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
                                           ISMS_SubjectName = e.ISMS_SubjectName,
                                           staffName = f.HRME_EmployeeFirstName + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " : f.HRME_EmployeeMiddleName) + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                           TTMC_CategoryName = i.TTMC_CategoryName,

                                       }
                             ).Distinct().ToArray();
                }

            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }



            return data;

        }      
        public StaffPeriodTransformDTO getpossiblePeriod(StaffPeriodTransformDTO data)
        {
            var asmay_id = _ttcategorycontext.TT_Final_GenerationDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFG_ActiveFlag == true).Select(r => r.ASMAY_Id).OrderByDescending(t=>t).FirstOrDefault();
                    
            List<StaffPeriodTransformDTO> result = new List<StaffPeriodTransformDTO>();

            using (var cmd = _ttcategorycontext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "TT_Get_Allpossibilities_for_StaffTheir_TT_replacement";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = asmay_id });             
                cmd.Parameters.Add(new SqlParameter("@ttmd_id", SqlDbType.BigInt) { Value = data.TTMD_Id });
                cmd.Parameters.Add(new SqlParameter("@ttmp_id", SqlDbType.BigInt) { Value = data.TTMP_Id });
                cmd.Parameters.Add(new SqlParameter("@hrme_id", SqlDbType.BigInt) { Value = data.HRME_Id });
                cmd.Parameters.Add(new SqlParameter("@staffwiseflag", SqlDbType.BigInt) { Value = data.staffSDK });
                cmd.Parameters.Add(new SqlParameter("@subjectwiseflag", SqlDbType.BigInt) { Value = data.subSDK });
                cmd.Parameters.Add(new SqlParameter("@Consecutivewiseflag", SqlDbType.BigInt) { Value = data.conSDK });

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                var retObject = new List<dynamic>();
                try
                {
                    using (var dataReader = cmd.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            result.Add(new StaffPeriodTransformDTO
                            {
                                TTMD_Id = Convert.ToInt64(dataReader["TTMD_Id"].ToString()),
                                TTMP_Id = Convert.ToInt32(dataReader["TTMP_Id"].ToString()),
                            });
                            data.Data_lst = result.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            return data;
        }
        public StaffPeriodTransformDTO savedetail(StaffPeriodTransformDTO data)
        {
            try
            {

                if (data.Time_Table.Length>0)
                {


                    foreach (var item in data.Time_Table)
                    {
                        var ressult = _ttcategorycontext.TT_Final_Generation_DetailedDMO.Single(f => f.TTFGD_Id == item.TTFGD_Id);
                        ressult.HRME_Id = data.HRME_IdTO;
                        ressult.UpdatedDate = DateTime.Now;

                        _ttcategorycontext.Update(ressult);
                        int res = _ttcategorycontext.SaveChanges();
                        if (res>0)
                        {
                            data.returnval = true;
                            data.sscnt += 1;
                        }
                        else
                        {
                            data.returnval = false;
                            data.ffcnt += 1;
                        }
                    }

                    



                }

               

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public StaffPeriodTransformDTO deleteperiod(StaffPeriodTransformDTO data)
        {
            try
            {

                if (data.Time_Table.Length>0)
                {


                    foreach (var item in data.Time_Table)
                    {
                        var ressult = _ttcategorycontext.TT_Final_Generation_DetailedDMO.Single(f => f.TTFGD_Id == item.TTFGD_Id);
                        _ttcategorycontext.Remove(ressult);
                        int res = _ttcategorycontext.SaveChanges();
                        if (res>0)
                        {
                            data.returnval = true;
                            data.sscnt += 1;
                        }
                        else
                        {
                            data.returnval = false;
                            data.ffcnt += 1;
                        }
                    }

                    



                }

               

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
