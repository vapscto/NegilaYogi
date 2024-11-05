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
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TT_ConsecutiveImpl : Interfaces.TT_ConsecutiveInterface
    {
        private static ConcurrentDictionary<string, TT_ConsecutiveDTO> _login =
                 new ConcurrentDictionary<string, TT_ConsecutiveDTO>();


        public TTContext _ttcategorycontext;
        public TT_ConsecutiveImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }
        public TT_ConsecutiveDTO savedetail(TT_ConsecutiveDTO _category)
        {
            TT_ConsecutiveDMO objpge = Mapper.Map<TT_ConsecutiveDMO>(_category);
            try
            {
                if (objpge.TTC_Id > 0)
                {
                    var resultw = _ttcategorycontext.TT_ConsecutiveDMO.Where(t => t.ASMCL_Id.Equals(objpge.ASMCL_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.ASMS_Id.Equals(objpge.ASMS_Id)
                    && t.HRME_Id.Equals(objpge.HRME_Id) && t.ISMS_Id.Equals(objpge.ISMS_Id) && t.TTC_NoOfPeriods.Equals(objpge.TTC_NoOfPeriods) && t.TTC_RemPeriods.Equals(objpge.TTC_RemPeriods)
                    && t.TTC_NoOfConPeriods.Equals(objpge.TTC_NoOfConPeriods) && t.TTC_NoOfConDays.Equals(objpge.TTC_NoOfConDays)
                    && t.TTC_BefAftApplFlag.Equals(objpge.TTC_BefAftApplFlag) && t.TTC_BefAftFalg.Equals(objpge.TTC_BefAftFalg) && t.TTC_BefAftPeriod.Equals(objpge.TTC_BefAftPeriod)
                    && t.TTC_AllotedFlag.Equals(objpge.TTC_AllotedFlag));
                    if (resultw.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ttcategorycontext.TT_ConsecutiveDMO.Single(t => t.TTC_Id.Equals(objpge.TTC_Id) && t.MI_Id.Equals(objpge.MI_Id));
                        result.ASMCL_Id = objpge.ASMCL_Id;
                        result.ASMAY_Id = objpge.ASMAY_Id;
                        result.TTMC_Id = objpge.TTMC_Id;
                        result.ASMS_Id = objpge.ASMS_Id;
                        result.HRME_Id = objpge.HRME_Id;
                        result.ISMS_Id = objpge.ISMS_Id;
                        result.TTC_NoOfPeriods = objpge.TTC_NoOfPeriods;
                        result.TTC_AllotPeriods = objpge.TTC_AllotPeriods;
                        result.TTC_RemPeriods = objpge.TTC_RemPeriods;
                        result.TTC_NoOfConPeriods = objpge.TTC_NoOfConPeriods;
                        result.TTC_NoOfConDays = objpge.TTC_NoOfConDays;
                        result.TTC_BefAftApplFlag = objpge.TTC_BefAftApplFlag;
                        result.TTC_BefAftFalg = objpge.TTC_BefAftFalg;
                        result.TTC_BefAftPeriod = objpge.TTC_BefAftPeriod;
                        result.TTC_AllotedFlag = objpge.TTC_AllotedFlag;
                        result.TTC_ActiveFlag = true;
                        result.UpdatedDate = DateTime.Now;
                        _ttcategorycontext.Update(result);
                        var contactExists = _ttcategorycontext.SaveChanges();
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
                else
                {
                    var result = _ttcategorycontext.TT_ConsecutiveDMO.Where(t => t.ASMCL_Id.Equals(objpge.ASMCL_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.ASMS_Id.Equals(objpge.ASMS_Id) && t.ISMS_Id.Equals(objpge.ISMS_Id) && t.HRME_Id.Equals(objpge.HRME_Id));
                    if (result.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        objpge.TTC_ActiveFlag = true;
                        _ttcategorycontext.Add(objpge);
                        var contactExists = _ttcategorycontext.SaveChanges();
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
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TT_ConsecutiveDTO getdetails(int id)
        {
            TT_ConsecutiveDTO TTMC = new TT_ConsecutiveDTO();
            try
            {
                List<AcademicYear> acad = new List<AcademicYear>();
                acad = _ttcategorycontext.AcademicYear.Where(t => t.MI_Id.Equals(id) && t.Is_Active == true).OrderByDescending(p=>p.ASMAY_Order).ToList();
                TTMC.academiclist = acad.ToArray();

                List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                mcat = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.MI_Id.Equals(id) && t.TTMC_ActiveFlag.Equals(true)).ToList();
                TTMC.catelist = mcat.ToArray();

                List<School_M_Class> allClass = new List<School_M_Class>();
                allClass = _ttcategorycontext.School_M_Class.Where(c => c.ASMCL_ActiveFlag.Equals(true) && c.MI_Id.Equals(id)).ToList();
                TTMC.classDrpDwn = allClass.ToArray();

                List<School_M_Section> allsect = new List<School_M_Section>();
                allsect = _ttcategorycontext.School_M_Section.Where(c => c.ASMC_ActiveFlag.Equals(1) && c.MI_Id.Equals(id)).ToList();
                TTMC.sectDrpDwn = allsect.ToArray();

                TTMC.staffDrpDwn = (from e in _ttcategorycontext.HR_Master_Employee_DMO
                                    from g in _ttcategorycontext.TT_Final_Period_DistributionDMO
                                    where (e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == id && e.HRME_ActiveFlag.Equals(true))
                                    select new TT_ConsecutiveDTO
                                    {
                                        HRME_Id = e.HRME_Id,
                                        staffNamelst = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                    }
                 ).Distinct().OrderBy(x => x.staffName).ToArray();



                List<IVRM_School_Master_SubjectsDMO> allsub = new List<IVRM_School_Master_SubjectsDMO>();
                allsub = _ttcategorycontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id.Equals(id)).ToList(); //&& c.TimeTable_flag.Equals("Y")
                TTMC.subjDrpDwn = allsub.ToArray();

                TTMC.consecutivelst = (from TT_Master_Category in _ttcategorycontext.TTMasterCategoryDMO
                                       from TT_Consecutive in _ttcategorycontext.TT_ConsecutiveDMO
                                       from Adm_School_M_Class in _ttcategorycontext.School_M_Class
                                       from Adm_School_M_Section in _ttcategorycontext.School_M_Section
                                       from f in _ttcategorycontext.HR_Master_Employee_DMO
                                       from IVRM_School_Master_SubjectsDMO in _ttcategorycontext.IVRM_School_Master_SubjectsDMO
                                       from Adm_School_M_Academic_Year in _ttcategorycontext.AcademicYear
                                       where (TT_Consecutive.TTMC_Id.Equals(TT_Master_Category.TTMC_Id) && Adm_School_M_Class.ASMCL_Id.Equals(TT_Consecutive.ASMCL_Id) &&
                                             Adm_School_M_Section.ASMS_Id.Equals(TT_Consecutive.ASMS_Id) && f.HRME_Id.Equals(TT_Consecutive.HRME_Id) &&
                                             IVRM_School_Master_SubjectsDMO.ISMS_Id.Equals(TT_Consecutive.ISMS_Id) && Adm_School_M_Academic_Year.ASMAY_Id.Equals(TT_Consecutive.ASMAY_Id) && TT_Consecutive.MI_Id.Equals(id))
                                       select new TT_ConsecutiveDTO
                                       {
                                           TTC_Id = TT_Consecutive.TTC_Id,
                                           ASMAYYear = Adm_School_M_Academic_Year.ASMAY_Year,
                                           CategoryName = TT_Master_Category.TTMC_CategoryName,
                                           NoOfPeriods = TT_Consecutive.TTC_NoOfPeriods,
                                           ClassName = Adm_School_M_Class.ASMCL_ClassName,
                                           SectionName = Adm_School_M_Section.ASMC_SectionName,
                                           staffName = f.HRME_EmployeeFirstName + " " + (f.HRME_EmployeeMiddleName == null || f.HRME_EmployeeMiddleName == " " || f.HRME_EmployeeMiddleName == "0" ? " " :f.HRME_EmployeeMiddleName) + " " + (f.HRME_EmployeeLastName == null || f.HRME_EmployeeLastName == " " || f.HRME_EmployeeLastName == "0" ? " " : f.HRME_EmployeeLastName),
                                           SubjectName = IVRM_School_Master_SubjectsDMO.ISMS_SubjectName,
                                           TTC_ActiveFlag = TT_Consecutive.TTC_ActiveFlag
                                       }
                                      ).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TT_ConsecutiveDTO getpageedit(int id)
        {
            TT_ConsecutiveDTO page = new TT_ConsecutiveDTO();
            try
            {
                List<TT_ConsecutiveDMO> lorg = new List<TT_ConsecutiveDMO>();
                lorg = _ttcategorycontext.TT_ConsecutiveDMO.AsNoTracking().Where(t => t.TTC_Id.Equals(id)).ToList();
                page.consecutivelstedit = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TT_ConsecutiveDTO deleterec(int id)
        {
            TT_ConsecutiveDTO page = new TT_ConsecutiveDTO();
            try
            {
                List<TT_ConsecutiveDMO> lorg = new List<TT_ConsecutiveDMO>();
                lorg = _ttcategorycontext.TT_ConsecutiveDMO.Where(t => t.TTC_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    _ttcategorycontext.Remove(lorg.ElementAt(0));
                    var contactExists = _ttcategorycontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        page.returnval = true;
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TT_ConsecutiveDTO getclass_catg(TT_ConsecutiveDTO data)
        {

            try
            {
                data.classbycategory = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                        from b in _ttcategorycontext.TT_Category_Class_DMO
                                        from c in _ttcategorycontext.School_M_Class
                                        where (a.MI_Id == b.MI_Id && a.MI_Id.Equals(c.MI_Id) && a.MI_Id.Equals(data.MI_Id) && a.TTMC_Id.Equals(b.TTMC_Id) && b.ASMCL_Id.Equals(c.ASMCL_Id) && a.TTMC_Id.Equals(data.TTMC_Id))
                                        select new TT_ConsecutiveDTO
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
            }
            return data;

        }

        public TT_ConsecutiveDTO getstaff(TT_ConsecutiveDTO data)
        {
            try
            {
                data.staffDrpDwn = (from e in _ttcategorycontext.HR_Master_Employee_DMO
                                    from g in _ttcategorycontext.TT_Final_Period_DistributionDMO
                                    from d in _ttcategorycontext.TT_Final_Period_Distribution_DetailedDMO
                                    where (e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && e.HRME_ActiveFlag.Equals(true) && d.TTFPD_Id == g.TTFPD_Id && d.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == data.ASMS_Id)
                                    select new TT_ConsecutiveDTO
                                    {
                                        HRME_Id = g.HRME_Id,
                                        staffNamelst = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                    }
                 ).Distinct().OrderBy(x => x.staffName).ToArray();



            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }

        public TT_ConsecutiveDTO deactivate(TT_ConsecutiveDTO acd)
        {
            try
            {
                TT_ConsecutiveDMO pge = Mapper.Map<TT_ConsecutiveDMO>(acd);
                if (pge.TTC_Id > 0)
                {
                    var result = _ttcategorycontext.TT_ConsecutiveDMO.Single(t => t.TTC_Id.Equals(pge.TTC_Id));
                    if (result.TTC_ActiveFlag.Equals(true))
                    {
                        result.TTC_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTC_ActiveFlag = true;
                    }
                    _ttcategorycontext.Update(result);
                    var flag = _ttcategorycontext.SaveChanges();
                    if (flag.Equals(1))
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                acd.returnval = false;
            }
            return acd;
        }
        public TT_ConsecutiveDTO get_catg(TT_ConsecutiveDTO data)
        {
            try
            {
                data.catelist = (from a in _ttcategorycontext.TTMasterCategoryDMO
                                 where (a.MI_Id.Equals(data.MI_Id) && a.TTMC_ActiveFlag.Equals(true))
                                 select new TT_ConsecutiveDTO
                                 {
                                     TTMC_Id = a.TTMC_Id,
                                     TTMC_CategoryName = a.TTMC_CategoryName,
                                 }
          ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                data.returnval = false;
            }
            return data;

        }




    }
}
