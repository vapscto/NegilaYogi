using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTFixingImpl : Interfaces.FixingInterface
    {
        private static ConcurrentDictionary<string, TTMasterCategoryDTO> _login =
         new ConcurrentDictionary<string, TTMasterCategoryDTO>();


        public TTContext _ttcontext;
        readonly ILogger<TTFixingImpl> _logger;
        public TTFixingImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public TTFixingDTO savedetail1(TTFixingDTO objday)
        {
            TT_Fixing_Day_PeriodDMO objpge = Mapper.Map<TT_Fixing_Day_PeriodDMO>(objday);
            try
            {
                var restrict_count = _ttcontext.TT_Restricting_Day_PeriodDMO.AsNoTracking().Where(r => r.MI_Id == objpge.MI_Id && r.ASMAY_Id == objpge.ASMAY_Id && r.TTMC_Id == objpge.TTMC_Id && r.ASMCL_Id == objpge.ASMCL_Id && r.ASMS_Id == objpge.ASMS_Id && r.TTMD_Id == objpge.TTMD_Id && r.TTMP_Id == objpge.TTMP_Id && r.HRME_Id == objpge.HRME_Id && r.ISMS_Id == objpge.ISMS_Id && r.TTRDP_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                    if (objpge.TTFDP_Id > 0)
                    {
                     var resultCount = _ttcontext.TT_Fixing_Day_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.ASMCL_Id == objpge.ASMCL_Id && t.ASMS_Id == objpge.ASMS_Id && t.TTMC_Id == objpge.TTMC_Id && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id && t.HRME_Id==objpge.HRME_Id  && t.TTFDP_Id != objpge.TTFDP_Id).Count();
                        if (resultCount == 0)
                        {
                            var result = _ttcontext.TT_Fixing_Day_PeriodDMO.Single(t => t.TTFDP_Id == objpge.TTFDP_Id && t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id);
                            result.MI_Id = objpge.MI_Id;
                            result.ASMAY_Id = objpge.ASMAY_Id;
                            result.TTMC_Id = objpge.TTMC_Id;
                            result.ASMCL_Id = objpge.ASMCL_Id;
                            result.ASMS_Id = objpge.ASMS_Id;
                            result.HRME_Id = objpge.HRME_Id;
                            result.ISMS_Id = objpge.ISMS_Id;
                            result.TTMD_Id = objpge.TTMD_Id;
                            result.TTMP_Id = objpge.TTMP_Id;
                            objpge.UpdatedDate = DateTime.Now;                      
                            _ttcontext.Update(result);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                objday.returnval = true;
                            }
                            else
                            {
                                objday.returnval = false;
                            }
                        }
                        else
                        {
                            objday.returnduplicatestatus = "Duplicate";
                            return objday;
                        }

                    }
                    else
                    {
                        var resultCount = _ttcontext.TT_Fixing_Day_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.ASMCL_Id == objpge.ASMCL_Id && t.ASMS_Id == objpge.ASMS_Id && t.TTMC_Id == objpge.TTMC_Id && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id && t.HRME_Id == objpge.HRME_Id).Count();
                        if (resultCount > 0)
                        {
                            objday.returnduplicatestatus = "Duplicate";
                            return objday;
                        }
                        else
                        {
                            objpge.TTFDP_AllotedFlag = "No";
                            objpge.TTFDP_ActiveFlag = true;
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            _ttcontext.Add(objpge);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                objday.returnval = true;
                            }
                            else
                            {
                                objday.returnval = false;
                            }
                        }
                    }
                }
                else
                {
                    objday.returnrestrictstatus = "Restricted";
                    return objday;
                }
                objday.fix_day_period_list = (from a in _ttcontext.AcademicYear
                                            from b in _ttcontext.TTMasterCategoryDMO
                                            from c in _ttcontext.School_M_Class
                                            from d in _ttcontext.School_M_Section
                                            from e in _ttcontext.HR_Master_Employee_DMO                                          
                                            from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                            from g in _ttcontext.TT_Master_PeriodDMO
                                            from h in _ttcontext.TT_Fixing_Day_PeriodDMO
                                            from i in _ttcontext.TT_Master_DayDMO
                                            where (a.MI_Id == h.MI_Id && a.ASMAY_Id == h.ASMAY_Id && b.MI_Id == h.MI_Id && b.TTMC_Id == h.TTMC_Id && c.MI_Id == h.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == h.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == h.MI_Id && e.HRME_Id == h.HRME_Id && f.MI_Id == h.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == h.MI_Id && g.TTMP_Id == h.TTMP_Id && i.MI_Id == h.MI_Id && i.TTMD_Id == h.TTMD_Id && h.MI_Id == objday.MI_Id)
                                            select new TTFixingDTO
                                            {
                                                TTFDP_Id = h.TTFDP_Id,
                                                ASMAY_Year = a.ASMAY_Year,
                                                TTMC_CategoryName = b.TTMC_CategoryName,
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                ISMS_SubjectName = f.ISMS_SubjectName,
                                                TTMP_PeriodName = g.TTMP_PeriodName,
                                                TTMD_DayName = i.TTMD_DayName,
                                                TTFDP_ActiveFlag = h.TTFDP_ActiveFlag
                                            }
  ).Distinct().OrderBy(x => x.ASMCL_ClassName).ToArray();


            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return objday;
        }

        public TTFixingDTO savedetail2(TTFixingDTO _category)
        {
            TT_Fixing_Day_StaffDMO objpge = Mapper.Map<TT_Fixing_Day_StaffDMO>(_category);
            try
            {
                var restrict_count = _ttcontext.TT_Restricting_Day_StaffDMO.AsNoTracking().Where(r => r.MI_Id == objpge.MI_Id && r.ASMAY_Id == objpge.ASMAY_Id && r.HRME_Id == objpge.HRME_Id && r.TTMD_Id == objpge.TTMD_Id && r.TTRDS_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                  if (_category.TTFDS_SUbSelcFlag == false)
                  {
                    if (objpge.TTFDS_Id > 0)
                    {

                        var result0 = _ttcontext.TT_Fixing_Day_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id) && t.TTFDS_Id != objpge.TTFDS_Id);
                        if (result0.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result = _ttcontext.TT_Fixing_Day_StaffDMO.Single(t => t.TTFDS_Id.Equals(objpge.TTFDS_Id) && t.MI_Id.Equals(objpge.MI_Id));
                            result.ASMAY_Id = objpge.ASMAY_Id;
                            result.HRME_Id = objpge.HRME_Id;
                            result.TTMD_Id = objpge.TTMD_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.TTFDS_AllotedFlag = "No";
                            result.TTFDS_ActiveFlag = true;
                            result.TTFDS_SUbSelcFlag = false;                        
                            _ttcontext.Update(result);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                List<TT_Fixing_Day_Staff_ClassSectionDMO> deactivelist = new List<TT_Fixing_Day_Staff_ClassSectionDMO>();
                                deactivelist = _ttcontext.TT_Fixing_Day_Staff_ClassSectionDMO.AsNoTracking().Where(s => s.TTFDS_Id == objpge.TTFDS_Id).ToList();
                                if (deactivelist.Count() > 0)
                                {
                                    List<TT_Fixing_Day_Staff_ClassSectionDMO> lorg = new List<TT_Fixing_Day_Staff_ClassSectionDMO>();
                                    lorg = _ttcontext.TT_Fixing_Day_Staff_ClassSectionDMO.Where(t => t.TTFDS_Id.Equals(objpge.TTFDS_Id)).ToList();
                                    if (lorg.Any())
                                    {
                                        for (int i = 0; lorg.Count > i; i++)
                                        {
                                            lorg.ElementAt(i).TTFDSCC_ActiveFlag = false;
                                            _ttcontext.Update(lorg.ElementAt(i));
                                            var contactExists1 = _ttcontext.SaveChanges();
                                            if (contactExists1 == 1)
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
                        var result = _ttcontext.TT_Fixing_Day_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id));
                        if (result.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.TTFDS_AllotedFlag = "No";
                            objpge.TTFDS_ActiveFlag = true;
                            objpge.TTFDS_SUbSelcFlag = false;
                            _ttcontext.Add(objpge);
                            var contactExists = _ttcontext.SaveChanges();

                            var result123 = _ttcontext.TT_Fixing_Day_StaffDMO.Max(t => t.TTFDS_Id);
                            if (contactExists == 1)
                            {
                                List<TT_Fixing_Day_Staff_ClassSectionDMO> deactivelist = new List<TT_Fixing_Day_Staff_ClassSectionDMO>();
                                deactivelist = _ttcontext.TT_Fixing_Day_Staff_ClassSectionDMO.AsNoTracking().Where(s => s.TTFDS_Id == result123).ToList();
                                if (deactivelist.Count() > 0)
                                {
                                    List<TT_Fixing_Day_Staff_ClassSectionDMO> lorg = new List<TT_Fixing_Day_Staff_ClassSectionDMO>();
                                    lorg = _ttcontext.TT_Fixing_Day_Staff_ClassSectionDMO.Where(t => t.TTFDS_Id.Equals(result123)).ToList();
                                    if (lorg.Any())
                                    {
                                        for (int i = 0; lorg.Count > i; i++)
                                        {
                                            lorg.ElementAt(i).TTFDSCC_ActiveFlag = false;
                                            _ttcontext.Update(lorg.ElementAt(i));
                                            var contactExists1 = _ttcontext.SaveChanges();
                                            if (contactExists1 == 1)
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
                                _category.returnval = true;
                            }
                            else
                            {
                                _category.returnval = false;
                            }
                        }
                    }

                }
                else if (_category.TTFDS_SUbSelcFlag == true)
                {
                    if (objpge.TTFDS_Id > 0)
                    {

                        var result0 = _ttcontext.TT_Fixing_Day_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id) && t.TTFDS_Id != objpge.TTFDS_Id);
                        if (result0.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {                           
                            var result = _ttcontext.TT_Fixing_Day_StaffDMO.Single(t => t.TTFDS_Id.Equals(objpge.TTFDS_Id) && t.MI_Id.Equals(objpge.MI_Id));
                            result.ASMAY_Id = objpge.ASMAY_Id;
                            result.HRME_Id = objpge.HRME_Id;
                            result.TTMD_Id = objpge.TTMD_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.TTFDS_AllotedFlag = "No";
                            result.TTFDS_ActiveFlag = true;
                            result.TTFDS_SUbSelcFlag = true;
                           
                            _ttcontext.Update(result);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {                              
                                deletepagesRightgrid2(objpge.TTFDS_Id);
                                for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                {
                                    TTFixingDTO ttclass = new TTFixingDTO();
                                    TT_Fixing_Day_Staff_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Fixing_Day_Staff_ClassSectionDMO>(ttclass);
                                    tt_dis_det.TTFDS_Id = objpge.TTFDS_Id;
                                    tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                        tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);                                 
                                    tt_dis_det.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                    tt_dis_det.TTFDSCC_Periods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                    tt_dis_det.TTFDSCC_ActiveFlag = true;
                                    tt_dis_det.CreatedDate = DateTime.Now;
                                    tt_dis_det.UpdatedDate = DateTime.Now;
                                    _ttcontext.Add(tt_dis_det);
                                    var contactExists1 = _ttcontext.SaveChanges();
                                    if (contactExists1 == 1)
                                    {
                                        _category.returnval = true;
                                    }
                                    else
                                    {
                                        _category.returnval = false;
                                    }
                                }
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
                        var result = _ttcontext.TT_Fixing_Day_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id));
                        if (result.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.TTFDS_AllotedFlag = "No";
                            objpge.TTFDS_ActiveFlag = true;
                            objpge.TTFDS_SUbSelcFlag = true;
                            _ttcontext.Add(objpge);
                            var contactExists = _ttcontext.SaveChanges();

                            var result123 = _ttcontext.TT_Fixing_Day_StaffDMO.Max(t => t.TTFDS_Id);
                            if (contactExists == 1)
                            {
                                for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                {
                                    TTFixingDTO ttclass = new TTFixingDTO();
                                    TT_Fixing_Day_Staff_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Fixing_Day_Staff_ClassSectionDMO>(ttclass);
                                    tt_dis_det.TTFDS_Id = result123;
                                    tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                    tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                    tt_dis_det.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                    tt_dis_det.TTFDSCC_Periods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                    tt_dis_det.TTFDSCC_ActiveFlag = true;
                                    tt_dis_det.CreatedDate = DateTime.Now;
                                    tt_dis_det.UpdatedDate = DateTime.Now;
                                    _ttcontext.Add(tt_dis_det);
                                    var contactExists1 = _ttcontext.SaveChanges();
                                    if (contactExists1 == 1)
                                    {
                                        _category.returnval = true;
                                    }
                                    else
                                    {
                                        _ttcontext.Remove(_ttcontext.TT_Fixing_Day_Staff_ClassSectionDMO.Where(t => t.TTFDS_Id.Equals(result123)));
                                        _ttcontext.Remove(_ttcontext.TT_Fixing_Day_StaffDMO.Where(t => t.TTFDS_Id.Equals(result123)));
                                        _category.returnval = false;
                                    }
                                }
                            }
                            else
                            {
                                _category.returnval = false;
                            }
                        }
                    }
                }
                }
                else
                {
                    _category.returnrestrictstatus = "Restricted";
                    return _category;
                }
                _category.all_fix_day_staff_list = (from a in _ttcontext.AcademicYear
                                               from e in _ttcontext.HR_Master_Employee_DMO
                                               from g in _ttcontext.TT_Fixing_Day_StaffDMO
                                                   from i in _ttcontext.TT_Master_DayDMO
                                               where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == _category.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                               select new TTFixingDTO
                                               {
                                                 TTFDS_Id = g.TTFDS_Id,
                                                   ASMAY_Year = a.ASMAY_Year,
                                                   staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                   TTMD_Id = g.TTMD_Id,
                                                   TTMD_DayName = i.TTMD_DayName,
                                                   TTFDS_SUbSelcFlag = g.TTFDS_SUbSelcFlag,
                                                   TTFDS_ActiveFlag = g.TTFDS_ActiveFlag,
                                               }
     ).Distinct().OrderBy(x => x.staffName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TTFixingDTO savedetail3(TTFixingDTO _category)
        {
            TT_Fixing_Day_SubjectDMO objpge = Mapper.Map<TT_Fixing_Day_SubjectDMO>(_category);
            try
            {
                var restrict_count = _ttcontext.TT_Restricting_Day_SubjectDMO.AsNoTracking().Where(r => r.MI_Id == objpge.MI_Id && r.ASMAY_Id == objpge.ASMAY_Id && r.ISMS_Id == objpge.ISMS_Id && r.TTMD_Id == objpge.TTMD_Id && r.TTRDSU_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                    if (_category.TTFDSU_SUbSelcFlag == false)
                    {
                        if (objpge.TTFDSU_Id > 0)
                        {

                            var result0 = _ttcontext.TT_Fixing_Day_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id) && t.TTFDSU_Id != objpge.TTFDSU_Id);
                            if (result0.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _ttcontext.TT_Fixing_Day_SubjectDMO.Single(t => t.TTFDSU_Id.Equals(objpge.TTFDSU_Id) && t.MI_Id.Equals(objpge.MI_Id));
                                result.ASMAY_Id = objpge.ASMAY_Id;
                                result.ISMS_Id = objpge.ISMS_Id;
                                result.TTMD_Id = objpge.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFDSU_AllotedFlag = "No";
                                result.TTFDSU_ActiveFlag = true;
                                result.TTFDSU_SUbSelcFlag = false;
                                _ttcontext.Update(result);
                                var contactExists = _ttcontext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    List<TT_Fixing_Day_Subject_ClassSectionDMO> deactivelist = new List<TT_Fixing_Day_Subject_ClassSectionDMO>();
                                    deactivelist = _ttcontext.TT_Fixing_Day_Subject_ClassSectionDMO.AsNoTracking().Where(s => s.TTFDSU_Id == objpge.TTFDSU_Id).ToList();
                                    if (deactivelist.Count() > 0)
                                    {
                                        List<TT_Fixing_Day_Subject_ClassSectionDMO> lorg = new List<TT_Fixing_Day_Subject_ClassSectionDMO>();
                                        lorg = _ttcontext.TT_Fixing_Day_Subject_ClassSectionDMO.Where(t => t.TTFDSU_Id.Equals(objpge.TTFDSU_Id)).ToList();
                                        if (lorg.Any())
                                        {
                                            for (int i = 0; lorg.Count > i; i++)
                                            {
                                                lorg.ElementAt(i).TTFDSUCC_ActiveFlag = false;
                                                _ttcontext.Update(lorg.ElementAt(i));
                                                var contactExists1 = _ttcontext.SaveChanges();
                                                if (contactExists1 == 1)
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
                            var result = _ttcontext.TT_Fixing_Day_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id));
                            if (result.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.TTFDSU_AllotedFlag = "No";
                                objpge.TTFDSU_ActiveFlag = true;
                                objpge.TTFDSU_SUbSelcFlag = false;
                                _ttcontext.Add(objpge);
                                var contactExists = _ttcontext.SaveChanges();

                                var result123 = _ttcontext.TT_Fixing_Day_SubjectDMO.Max(t => t.TTFDSU_Id);
                                if (contactExists == 1)
                                {
                                    List<TT_Fixing_Day_Subject_ClassSectionDMO> deactivelist = new List<TT_Fixing_Day_Subject_ClassSectionDMO>();
                                    deactivelist = _ttcontext.TT_Fixing_Day_Subject_ClassSectionDMO.AsNoTracking().Where(s => s.TTFDSU_Id == result123).ToList();
                                    if (deactivelist.Count() > 0)
                                    {
                                        List<TT_Fixing_Day_Subject_ClassSectionDMO> lorg = new List<TT_Fixing_Day_Subject_ClassSectionDMO>();
                                        lorg = _ttcontext.TT_Fixing_Day_Subject_ClassSectionDMO.Where(t => t.TTFDSU_Id.Equals(result123)).ToList();
                                        if (lorg.Any())
                                        {
                                            for (int i = 0; lorg.Count > i; i++)
                                            {
                                                lorg.ElementAt(i).TTFDSUCC_ActiveFlag = false;
                                                _ttcontext.Update(lorg.ElementAt(i));
                                                var contactExists1 = _ttcontext.SaveChanges();
                                                if (contactExists1 == 1)
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
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                        }

                    }
                    else if (_category.TTFDSU_SUbSelcFlag == true)
                    {
                        if (objpge.TTFDSU_Id > 0)
                        {

                            var result0 = _ttcontext.TT_Fixing_Day_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id) && t.TTFDSU_Id != objpge.TTFDSU_Id);
                            if (result0.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _ttcontext.TT_Fixing_Day_SubjectDMO.Single(t => t.TTFDSU_Id.Equals(objpge.TTFDSU_Id) && t.MI_Id.Equals(objpge.MI_Id));
                                result.ASMAY_Id = objpge.ASMAY_Id;
                                result.ISMS_Id = objpge.ISMS_Id;
                                result.TTMD_Id = objpge.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFDSU_AllotedFlag = "No";
                                result.TTFDSU_ActiveFlag = true;
                                result.TTFDSU_SUbSelcFlag = true;
                                _ttcontext.Update(result);
                                var contactExists = _ttcontext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    deletepagesRightgrid3(objpge.TTFDSU_Id);
                                    for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                    {
                                        TTFixingDTO ttclass = new TTFixingDTO();
                                        TT_Fixing_Day_Subject_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Fixing_Day_Subject_ClassSectionDMO>(ttclass);
                                        tt_dis_det.TTFDSU_Id = objpge.TTFDSU_Id;
                                        tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                        tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                        tt_dis_det.HRME_Id = Convert.ToInt64(_category.TempararyArrayList[i].HRME_Id);
                                        tt_dis_det.TTFDSUCC_Periods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                        tt_dis_det.TTFDSUCC_ActiveFlag = true;
                                        tt_dis_det.CreatedDate = DateTime.Now;
                                        tt_dis_det.UpdatedDate = DateTime.Now;
                                        _ttcontext.Add(tt_dis_det);
                                        var contactExists1 = _ttcontext.SaveChanges();
                                        if (contactExists1 == 1)
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _category.returnval = false;
                                        }
                                    }
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
                            var result = _ttcontext.TT_Fixing_Day_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id));
                            if (result.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.TTFDSU_AllotedFlag = "No";
                                objpge.TTFDSU_ActiveFlag = true;
                                objpge.TTFDSU_SUbSelcFlag = true;
                                _ttcontext.Add(objpge);
                                var contactExists = _ttcontext.SaveChanges();

                                var result123 = _ttcontext.TT_Fixing_Day_SubjectDMO.Max(t => t.TTFDSU_Id);
                                if (contactExists == 1)
                                {
                                    for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                    {
                                        TTFixingDTO ttclass = new TTFixingDTO();
                                        TT_Fixing_Day_Subject_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Fixing_Day_Subject_ClassSectionDMO>(ttclass);
                                        tt_dis_det.TTFDSU_Id = result123;
                                        tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                        tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                        tt_dis_det.HRME_Id = Convert.ToInt64(_category.TempararyArrayList[i].HRME_Id);
                                        tt_dis_det.TTFDSUCC_Periods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                        tt_dis_det.TTFDSUCC_ActiveFlag = true;
                                        tt_dis_det.CreatedDate = DateTime.Now;
                                        tt_dis_det.UpdatedDate = DateTime.Now;
                                        _ttcontext.Add(tt_dis_det);
                                        var contactExists1 = _ttcontext.SaveChanges();
                                        if (contactExists1 == 1)
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _ttcontext.Remove(_ttcontext.TT_Fixing_Day_Subject_ClassSectionDMO.Where(t => t.TTFDSU_Id.Equals(result123)));
                                            _ttcontext.Remove(_ttcontext.TT_Fixing_Day_SubjectDMO.Where(t => t.TTFDSU_Id.Equals(result123)));
                                            _category.returnval = false;
                                        }
                                    }
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _category.returnrestrictstatus = "Restricted";
                    return _category;
                }
                _category.all_fix_day_subject_list = (from a in _ttcontext.AcademicYear
                                                    from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                                    from g in _ttcontext.TT_Fixing_Day_SubjectDMO                                                  
                                                    from i in _ttcontext.TT_Master_DayDMO
                                                    where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == _category.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                                    select new TTFixingDTO
                                                    {
                                                        TTFDSU_Id = g.TTFDSU_Id,
                                                        ASMAY_Year = a.ASMAY_Year,
                                                        ISMS_SubjectName=e.ISMS_SubjectName,
                                                        TTMD_Id = g.TTMD_Id,
                                                        TTMD_DayName = i.TTMD_DayName,
                                                        TTFDSU_SUbSelcFlag = g.TTFDSU_SUbSelcFlag,
                                                        TTFDSU_ActiveFlag = g.TTFDSU_ActiveFlag,
                                                    }
     ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }
        public TTFixingDTO savedetail4(TTFixingDTO _category)
        {
            TT_Fixing_Period_StaffDMO objpge = Mapper.Map<TT_Fixing_Period_StaffDMO>(_category);
            try
            {
                var restrict_count = _ttcontext.TT_Restricting_Period_StaffDMO.AsNoTracking().Where(r => r.MI_Id == objpge.MI_Id && r.ASMAY_Id == objpge.ASMAY_Id && r.HRME_Id == objpge.HRME_Id && r.TTMP_Id == objpge.TTMP_Id && r.TTRPS_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                    if (_category.TTFPS_SUbSelcFlag == false)
                    {
                        if (objpge.TTFPS_Id > 0)
                        {

                            var result0 = _ttcontext.TT_Fixing_Period_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id) && t.TTFPS_Id != objpge.TTFPS_Id);
                            if (result0.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _ttcontext.TT_Fixing_Period_StaffDMO.Single(t => t.TTFPS_Id.Equals(objpge.TTFPS_Id) && t.MI_Id.Equals(objpge.MI_Id));
                                result.ASMAY_Id = objpge.ASMAY_Id;
                                result.HRME_Id = objpge.HRME_Id;
                                result.TTMP_Id = objpge.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFPS_AllotedFlag = "No";
                                result.TTFPS_ActiveFlag = true;
                                result.TTFPS_SUbSelcFlag = false;
                                _ttcontext.Update(result);
                                var contactExists = _ttcontext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    List<TT_Fixing_Period_Staff_ClassSectionDMO> deactivelist = new List<TT_Fixing_Period_Staff_ClassSectionDMO>();
                                    deactivelist = _ttcontext.TT_Fixing_Period_Staff_ClassSectionDMO.AsNoTracking().Where(s => s.TTFPS_Id == objpge.TTFPS_Id).ToList();
                                    if (deactivelist.Count() > 0)
                                    {
                                        List<TT_Fixing_Period_Staff_ClassSectionDMO> lorg = new List<TT_Fixing_Period_Staff_ClassSectionDMO>();
                                        lorg = _ttcontext.TT_Fixing_Period_Staff_ClassSectionDMO.Where(t => t.TTFPS_Id.Equals(objpge.TTFPS_Id)).ToList();
                                        if (lorg.Any())
                                        {
                                            for (int i = 0; lorg.Count > i; i++)
                                            {
                                                lorg.ElementAt(i).TTFPSCC_ActiveFlag = false;
                                                _ttcontext.Update(lorg.ElementAt(i));
                                                var contactExists1 = _ttcontext.SaveChanges();
                                                if (contactExists1 == 1)
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
                            var result = _ttcontext.TT_Fixing_Period_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id));
                            if (result.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.TTFPS_AllotedFlag = "No";
                                objpge.TTFPS_ActiveFlag = true;
                                objpge.TTFPS_SUbSelcFlag = false;
                                _ttcontext.Add(objpge);
                                var contactExists = _ttcontext.SaveChanges();

                                var result123 = _ttcontext.TT_Fixing_Period_StaffDMO.Max(t => t.TTFPS_Id);
                                if (contactExists == 1)
                                {
                                    List<TT_Fixing_Period_Staff_ClassSectionDMO> deactivelist = new List<TT_Fixing_Period_Staff_ClassSectionDMO>();
                                    deactivelist = _ttcontext.TT_Fixing_Period_Staff_ClassSectionDMO.AsNoTracking().Where(s => s.TTFPS_Id == result123).ToList();
                                    if (deactivelist.Count() > 0)
                                    {
                                        List<TT_Fixing_Period_Staff_ClassSectionDMO> lorg = new List<TT_Fixing_Period_Staff_ClassSectionDMO>();
                                        lorg = _ttcontext.TT_Fixing_Period_Staff_ClassSectionDMO.Where(t => t.TTFPS_Id.Equals(result123)).ToList();
                                        if (lorg.Any())
                                        {
                                            for (int i = 0; lorg.Count > i; i++)
                                            {
                                                lorg.ElementAt(i).TTFPSCC_ActiveFlag = false;
                                                _ttcontext.Update(lorg.ElementAt(i));
                                                var contactExists1 = _ttcontext.SaveChanges();
                                                if (contactExists1 == 1)
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
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                        }

                    }
                    else if (_category.TTFPS_SUbSelcFlag == true)
                    {
                        if (objpge.TTFPS_Id > 0)
                        {

                            var result0 = _ttcontext.TT_Fixing_Period_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id) && t.TTFPS_Id != objpge.TTFPS_Id);
                            if (result0.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _ttcontext.TT_Fixing_Period_StaffDMO.Single(t => t.TTFPS_Id.Equals(objpge.TTFPS_Id) && t.MI_Id.Equals(objpge.MI_Id));
                                result.ASMAY_Id = objpge.ASMAY_Id;
                                result.HRME_Id = objpge.HRME_Id;
                                result.TTMP_Id = objpge.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFPS_AllotedFlag = "No";
                                result.TTFPS_ActiveFlag = true;
                                result.TTFPS_SUbSelcFlag = true;
                                _ttcontext.Update(result);
                                var contactExists = _ttcontext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    deletepagesRightgrid4(objpge.TTFPS_Id);
                                    for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                    {
                                        TTFixingDTO ttclass = new TTFixingDTO();
                                        TT_Fixing_Period_Staff_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Fixing_Period_Staff_ClassSectionDMO>(ttclass);
                                        tt_dis_det.TTFPS_Id = objpge.TTFPS_Id;
                                        tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                        tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                        tt_dis_det.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                        tt_dis_det.TTFPSCC_Days = Convert.ToInt32(_category.TempararyArrayList[i].NOD);
                                        tt_dis_det.TTFPSCC_ActiveFlag = true;
                                        tt_dis_det.CreatedDate = DateTime.Now;
                                        tt_dis_det.UpdatedDate = DateTime.Now;
                                        _ttcontext.Add(tt_dis_det);
                                        var contactExists1 = _ttcontext.SaveChanges();
                                        if (contactExists1 == 1)
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _category.returnval = false;
                                        }
                                    }
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
                            var result = _ttcontext.TT_Fixing_Period_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id));
                            if (result.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.TTFPS_AllotedFlag = "No";
                                objpge.TTFPS_ActiveFlag = true;
                                objpge.TTFPS_SUbSelcFlag = true;
                                _ttcontext.Add(objpge);
                                var contactExists = _ttcontext.SaveChanges();

                                var result123 = _ttcontext.TT_Fixing_Period_StaffDMO.Max(t => t.TTFPS_Id);
                                if (contactExists == 1)
                                {
                                    for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                    {
                                        TTFixingDTO ttclass = new TTFixingDTO();
                                        TT_Fixing_Period_Staff_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Fixing_Period_Staff_ClassSectionDMO>(ttclass);
                                        tt_dis_det.TTFPS_Id = result123;
                                        tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                        tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                        tt_dis_det.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                        tt_dis_det.TTFPSCC_Days = Convert.ToInt32(_category.TempararyArrayList[i].NOD);
                                        tt_dis_det.TTFPSCC_ActiveFlag = true;
                                        tt_dis_det.CreatedDate = DateTime.Now;
                                        tt_dis_det.UpdatedDate = DateTime.Now;
                                        _ttcontext.Add(tt_dis_det);
                                        var contactExists1 = _ttcontext.SaveChanges();
                                        if (contactExists1 == 1)
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _ttcontext.Remove(_ttcontext.TT_Fixing_Period_Staff_ClassSectionDMO.Where(t => t.TTFPS_Id.Equals(result123)));
                                            _ttcontext.Remove(_ttcontext.TT_Fixing_Period_StaffDMO.Where(t => t.TTFPS_Id.Equals(result123)));
                                            _category.returnval = false;
                                        }
                                    }
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _category.returnrestrictstatus = "Restricted";
                    return _category;
                }
                _category.all_fix_period_staff_list = (from a in _ttcontext.AcademicYear
                                                    from e in _ttcontext.HR_Master_Employee_DMO
                                                    from g in _ttcontext.TT_Fixing_Period_StaffDMO
                                                         from b in _ttcontext.TT_Master_PeriodDMO
                                                    where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == _category.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                    select new TTFixingDTO
                                                    {
                                                        TTFPS_Id = g.TTFPS_Id,
                                                        ASMAY_Year = a.ASMAY_Year,
                                                        staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                        TTMP_Id = g.TTMP_Id,
                                                       TTMP_PeriodName=b.TTMP_PeriodName,
                                                        TTFPS_SUbSelcFlag = g.TTFPS_SUbSelcFlag,
                                                        TTFPS_ActiveFlag = g.TTFPS_ActiveFlag,
                                                    }
      ).Distinct().OrderBy(x => x.staffName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TTFixingDTO savedetail5(TTFixingDTO _category)
        {
            TT_Fixing_Period_SubjectDMO objpge = Mapper.Map<TT_Fixing_Period_SubjectDMO>(_category);
            try
            {
                var restrict_count = _ttcontext.TT_Restricting_Period_SubjectDMO.AsNoTracking().Where(r => r.MI_Id == objpge.MI_Id && r.ASMAY_Id == objpge.ASMAY_Id && r.ISMS_Id == objpge.ISMS_Id && r.TTMP_Id == objpge.TTMP_Id && r.TTRPSU_ActiveFlag==true).Count();
                if (restrict_count == 0)
                {
                    if (_category.TTFPSU_SUbSelcFlag == false)
                    {
                        if (objpge.TTFPSU_Id > 0)
                        {

                            var result0 = _ttcontext.TT_Fixing_Period_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id) && t.TTFPSU_Id != objpge.TTFPSU_Id);
                            if (result0.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _ttcontext.TT_Fixing_Period_SubjectDMO.Single(t => t.TTFPSU_Id.Equals(objpge.TTFPSU_Id) && t.MI_Id.Equals(objpge.MI_Id));
                                result.ASMAY_Id = objpge.ASMAY_Id;
                                result.ISMS_Id = objpge.ISMS_Id;
                                result.TTMP_Id = objpge.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFPSU_AllotedFlag = "No";
                                result.TTFPSU_ActiveFlag = true;
                                result.TTFPSU_SUbSelcFlag = false;
                                _ttcontext.Update(result);
                                var contactExists = _ttcontext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    List<TT_Fixing_Period_Subject_ClassSectionDMO> deactivelist = new List<TT_Fixing_Period_Subject_ClassSectionDMO>();
                                    deactivelist = _ttcontext.TT_Fixing_Period_Subject_ClassSectionDMO.AsNoTracking().Where(s => s.TTFPSU_Id == objpge.TTFPSU_Id).ToList();
                                    if (deactivelist.Count() > 0)
                                    {
                                        List<TT_Fixing_Period_Subject_ClassSectionDMO> lorg = new List<TT_Fixing_Period_Subject_ClassSectionDMO>();
                                        lorg = _ttcontext.TT_Fixing_Period_Subject_ClassSectionDMO.Where(t => t.TTFPSU_Id.Equals(objpge.TTFPSU_Id)).ToList();
                                        if (lorg.Any())
                                        {
                                            for (int i = 0; lorg.Count > i; i++)
                                            {
                                                lorg.ElementAt(i).TTFPSUCC_ActiveFlag = false;
                                                _ttcontext.Update(lorg.ElementAt(i));
                                                var contactExists1 = _ttcontext.SaveChanges();
                                                if (contactExists1 == 1)
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
                            var result = _ttcontext.TT_Fixing_Period_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id));
                            if (result.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.TTFPSU_AllotedFlag = "No";
                                objpge.TTFPSU_ActiveFlag = true;
                                objpge.TTFPSU_SUbSelcFlag = false;
                                _ttcontext.Add(objpge);
                                var contactExists = _ttcontext.SaveChanges();

                                var result123 = _ttcontext.TT_Fixing_Period_SubjectDMO.Max(t => t.TTFPSU_Id);
                                if (contactExists == 1)
                                {
                                    List<TT_Fixing_Period_Subject_ClassSectionDMO> deactivelist = new List<TT_Fixing_Period_Subject_ClassSectionDMO>();
                                    deactivelist = _ttcontext.TT_Fixing_Period_Subject_ClassSectionDMO.AsNoTracking().Where(s => s.TTFPSU_Id == result123).ToList();
                                    if (deactivelist.Count() > 0)
                                    {
                                        List<TT_Fixing_Period_Subject_ClassSectionDMO> lorg = new List<TT_Fixing_Period_Subject_ClassSectionDMO>();
                                        lorg = _ttcontext.TT_Fixing_Period_Subject_ClassSectionDMO.Where(t => t.TTFPSU_Id.Equals(result123)).ToList();
                                        if (lorg.Any())
                                        {
                                            for (int i = 0; lorg.Count > i; i++)
                                            {
                                                lorg.ElementAt(i).TTFPSUCC_ActiveFlag = false;
                                                _ttcontext.Update(lorg.ElementAt(i));
                                                var contactExists1 = _ttcontext.SaveChanges();
                                                if (contactExists1 == 1)
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
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                        }

                    }
                    else if (_category.TTFPSU_SUbSelcFlag == true)
                    {
                        if (objpge.TTFPSU_Id > 0)
                        {

                            var result0 = _ttcontext.TT_Fixing_Period_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id) && t.TTFPSU_Id != objpge.TTFPSU_Id);
                            if (result0.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _ttcontext.TT_Fixing_Period_SubjectDMO.Single(t => t.TTFPSU_Id.Equals(objpge.TTFPSU_Id) && t.MI_Id.Equals(objpge.MI_Id));
                                result.ASMAY_Id = objpge.ASMAY_Id;
                                result.ISMS_Id = objpge.ISMS_Id;
                                result.TTMP_Id = objpge.TTMP_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTFPSU_AllotedFlag = "No";
                                result.TTFPSU_ActiveFlag = true;
                                result.TTFPSU_SUbSelcFlag = true;
                                _ttcontext.Update(result);
                                var contactExists = _ttcontext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    deletepagesRightgrid5(objpge.TTFPSU_Id);
                                    for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                    {
                                        TTFixingDTO ttclass = new TTFixingDTO();
                                        TT_Fixing_Period_Subject_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Fixing_Period_Subject_ClassSectionDMO>(ttclass);
                                        tt_dis_det.TTFPSU_Id = objpge.TTFPSU_Id;
                                        tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                        tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                        tt_dis_det.HRME_Id = Convert.ToInt64(_category.TempararyArrayList[i].HRME_Id);
                                        tt_dis_det.TTFPSUCC_Days = Convert.ToInt32(_category.TempararyArrayList[i].NOD);
                                        tt_dis_det.TTFPSUCC_ActiveFlag = true;
                                        tt_dis_det.CreatedDate = DateTime.Now;
                                        tt_dis_det.UpdatedDate = DateTime.Now;
                                        _ttcontext.Add(tt_dis_det);
                                        var contactExists1 = _ttcontext.SaveChanges();
                                        if (contactExists1 == 1)
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _category.returnval = false;
                                        }
                                    }
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
                            var result = _ttcontext.TT_Fixing_Period_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id));
                            if (result.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.TTFPSU_AllotedFlag = "No";
                                objpge.TTFPSU_ActiveFlag = true;
                                objpge.TTFPSU_SUbSelcFlag = true;
                                _ttcontext.Add(objpge);
                                var contactExists = _ttcontext.SaveChanges();

                                var result123 = _ttcontext.TT_Fixing_Period_SubjectDMO.Max(t => t.TTFPSU_Id);
                                if (contactExists == 1)
                                {
                                    for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                    {
                                        TTFixingDTO ttclass = new TTFixingDTO();
                                        TT_Fixing_Period_Subject_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Fixing_Period_Subject_ClassSectionDMO>(ttclass);
                                        tt_dis_det.TTFPSU_Id = result123;
                                        tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                        tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                        tt_dis_det.HRME_Id = Convert.ToInt64(_category.TempararyArrayList[i].HRME_Id);
                                        tt_dis_det.TTFPSUCC_Days = Convert.ToInt32(_category.TempararyArrayList[i].NOD);
                                        tt_dis_det.TTFPSUCC_ActiveFlag = true;
                                        tt_dis_det.CreatedDate = DateTime.Now;
                                        tt_dis_det.UpdatedDate = DateTime.Now;
                                        _ttcontext.Add(tt_dis_det);
                                        var contactExists1 = _ttcontext.SaveChanges();
                                        if (contactExists1 == 1)
                                        {
                                            _category.returnval = true;
                                        }
                                        else
                                        {
                                            _ttcontext.Remove(_ttcontext.TT_Fixing_Period_Subject_ClassSectionDMO.Where(t => t.TTFPSU_Id.Equals(result123)));
                                            _ttcontext.Remove(_ttcontext.TT_Fixing_Period_SubjectDMO.Where(t => t.TTFPSU_Id.Equals(result123)));
                                            _category.returnval = false;
                                        }
                                    }
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    _category.returnrestrictstatus = "Restricted";
                    return _category;
                }
                _category.all_fix_period_subject_list = (from a in _ttcontext.AcademicYear
                                                       from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                                       from g in _ttcontext.TT_Fixing_Period_SubjectDMO
                                                       from b in _ttcontext.TT_Master_PeriodDMO
                                                       where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == _category.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                       select new TTFixingDTO
                                                       {
                                                           TTFPSU_Id = g.TTFPSU_Id,
                                                           ASMAY_Year = a.ASMAY_Year,
                                                           ISMS_SubjectName=e.ISMS_SubjectName,
                                                           TTMP_Id = g.TTMP_Id,
                                                           TTMP_PeriodName = b.TTMP_PeriodName,
                                                           TTFPSU_SUbSelcFlag = g.TTFPSU_SUbSelcFlag,
                                                           TTFPSU_ActiveFlag = g.TTFPSU_ActiveFlag,
                                                       }
     ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TTFixingDTO savedetail(TTFixingDTO _category)
        {
            TT_Final_Period_DistributionDMO objpge = Mapper.Map<TT_Final_Period_DistributionDMO>(_category);
            try
            {
                if (objpge.TTFPD_Id > 0)
                {

                    var result0 = _ttcontext.TT_Final_Period_DistributionDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTFPD_Id!=objpge.TTFPD_Id);
                    if (result0.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ttcontext.TT_Final_Period_DistributionDMO.Single(t => t.TTFPD_Id.Equals(objpge.TTFPD_Id) && t.MI_Id.Equals(objpge.MI_Id));
                         result.ASMAY_Id = objpge.ASMAY_Id;
                         result.HRME_Id = objpge.HRME_Id;
                         result.TTFPD_TotWeekPeriods = objpge.TTFPD_TotWeekPeriods;
                         result.TTFPD_ActiveFlag = true;
                        result.UpdatedDate = DateTime.Now;
                        _ttcontext.Update(result);
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                            {
                                TTFixingDTO ttclass = new TTFixingDTO();
                                TT_Final_Period_Distribution_DetailedDMO tt_dis_det = Mapper.Map<TT_Final_Period_Distribution_DetailedDMO>(ttclass);

                                tt_dis_det.TTFPD_Id = objpge.TTFPD_Id;
                                tt_dis_det.TTMC_Id = _category.TTMC_Id;
                                tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                tt_dis_det.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                tt_dis_det.TTFPD_TotalPeriods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                tt_dis_det.TTFPD_AllotedPeriods = 0;
                                tt_dis_det.TTFPD_AvailablePeriods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                tt_dis_det.TTFPDD_ActiveFlag = true;
                                tt_dis_det.CreatedDate = DateTime.Now;
                                tt_dis_det.UpdatedDate = DateTime.Now;
                                _ttcontext.Add(tt_dis_det);
                                var contactExists1 = _ttcontext.SaveChanges();
                                if (contactExists1 == 1)
                                {
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _category.returnval = false;
                                }
                            }
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
                    var result = _ttcontext.TT_Final_Period_DistributionDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id));
                    if (result.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                       objpge.CreatedDate = DateTime.Now;
                       objpge.UpdatedDate = DateTime.Now;
                        objpge.TTFPD_ActiveFlag = true;
                        _ttcontext.Add(objpge);
                        var contactExists = _ttcontext.SaveChanges();

                        var result123 = _ttcontext.TT_Final_Period_DistributionDMO.Max(t => t.TTFPD_Id);
                        if (contactExists == 1)
                        {
                            for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                            {
                                TTFixingDTO ttclass = new TTFixingDTO();
                                TT_Final_Period_Distribution_DetailedDMO tt_dis_det = Mapper.Map<TT_Final_Period_Distribution_DetailedDMO>(ttclass);
                                tt_dis_det.TTFPD_Id = result123;
                                tt_dis_det.TTMC_Id = _category.TTMC_Id;
                                tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                tt_dis_det.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                tt_dis_det.TTFPD_TotalPeriods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                tt_dis_det.TTFPD_AllotedPeriods = 0;
                                tt_dis_det.TTFPD_AvailablePeriods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                tt_dis_det.TTFPDD_ActiveFlag = true;
                                tt_dis_det.CreatedDate = DateTime.Now;
                                tt_dis_det.UpdatedDate = DateTime.Now;
                                _ttcontext.Add(tt_dis_det);
                                var contactExists1 = _ttcontext.SaveChanges();
                                if (contactExists1 == 1)
                                {
                                    _category.returnval = true;
                                }
                                else
                                {
                                    _ttcontext.Remove(_ttcontext.TT_Final_Period_Distribution_DetailedDMO.Where(t => t.TTFPD_Id.Equals(result123)));
                                    _ttcontext.Remove(_ttcontext.TT_Final_Period_DistributionDMO.Where(t => t.TTFPD_Id.Equals(result123)));                            
                                    _category.returnval = false;
                                }
                            }
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
         public TTFixingDTO deletepagesRightgrid2(long id)
        {
            TTFixingDTO pagert = new TTFixingDTO();     
            try
            {
                
                List<TT_Fixing_Day_Staff_ClassSectionDMO> lorg = new List<TT_Fixing_Day_Staff_ClassSectionDMO>();
                lorg = _ttcontext.TT_Fixing_Day_Staff_ClassSectionDMO.Where(t => t.TTFDS_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _ttcontext.Remove(lorg.ElementAt(i));
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public TTFixingDTO deletepagesRightgrid3(long id)
        {
            TTFixingDTO pagert = new TTFixingDTO();        
            try
            {

                List<TT_Fixing_Day_Subject_ClassSectionDMO> lorg = new List<TT_Fixing_Day_Subject_ClassSectionDMO>();
                lorg = _ttcontext.TT_Fixing_Day_Subject_ClassSectionDMO.Where(t => t.TTFDSU_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _ttcontext.Remove(lorg.ElementAt(i));
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public TTFixingDTO deletepagesRightgrid4(long id)
        {
            TTFixingDTO pagert = new TTFixingDTO();
            try
            {

                List<TT_Fixing_Period_Staff_ClassSectionDMO> lorg = new List<TT_Fixing_Period_Staff_ClassSectionDMO>();
                lorg = _ttcontext.TT_Fixing_Period_Staff_ClassSectionDMO.Where(t => t.TTFPS_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _ttcontext.Remove(lorg.ElementAt(i));
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public TTFixingDTO deletepagesRightgrid5(long id)
        {
            TTFixingDTO pagert = new TTFixingDTO();
            try
            {

                List<TT_Fixing_Period_Subject_ClassSectionDMO> lorg = new List<TT_Fixing_Period_Subject_ClassSectionDMO>();
                lorg = _ttcontext.TT_Fixing_Period_Subject_ClassSectionDMO.Where(t => t.TTFPSU_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    for (int i = 0; lorg.Count > i; i++)
                    {
                        _ttcontext.Remove(lorg.ElementAt(i));
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            pagert.returnval = true;
                        }
                        else
                        {
                            pagert.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }

        public TTFixingDTO getalldetailsviewrecords(int id)
        {
            TTFixingDTO TTMC = new TTFixingDTO();
            try
            {
                TTMC.detailspopuparray2 = (from a in _ttcontext.AcademicYear
                                          from b in _ttcontext.TTMasterCategoryDMO
                                           from c in _ttcontext.School_M_Class
                                          from d in _ttcontext.School_M_Section
                                          from e in _ttcontext.HR_Master_Employee_DMO
                                          from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                          from g in _ttcontext.TT_Final_Period_DistributionDMO
                                          from h in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                          where (g.TTFPD_Id == h.TTFPD_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && b.MI_Id == g.MI_Id && b.TTMC_Id == h.TTMC_Id && c.MI_Id == g.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == g.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && f.MI_Id == g.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == g.MI_Id && h.TTFPD_Id==id)
                                          select new TTFixingDTO
                                          {
                                              TTFPDD_Id = h.TTFPDD_Id,
                                              ASMAY_Year = a.ASMAY_Year,
                                              TTMC_CategoryName = b.TTMC_CategoryName,
                                              ASMCL_ClassName = c.ASMCL_ClassName,
                                              ASMC_SectionName = d.ASMC_SectionName,
                                              staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                              ISMS_SubjectName = f.ISMS_SubjectName,
                                              TTFPD_TotalPeriods = h.TTFPD_TotalPeriods,
                                              TTFPDD_ActiveFlag = h.TTFPDD_ActiveFlag,
                                          }
                ).Distinct().OrderBy(x => x.staffName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TTFixingDTO getdetails(TTFixingDTO data)
       {
            try
            {
                List<AcademicYear> year = new List<AcademicYear>();
                year = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.acayear = year.Distinct().OrderByDescending(ii=>ii.ASMAY_Order).ToArray();

                List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                mcat = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList();
                data.categorylist = mcat.Distinct().ToArray();

                data.classlist = (from a in _ttcontext.TTMasterCategoryDMO
                                  from b in _ttcontext.TT_Category_Class_DMO
                                  from c in _ttcontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && b.TTCC_ActiveFlag == true )
                                  select new TTFixingDTO
                                  {
                                      ASMCL_Id = c.ASMCL_Id,
                                      ASMCL_ClassName = c.ASMCL_ClassName,
                                      TTMC_Id = a.TTMC_Id,
                                      TTMC_CategoryName = a.TTMC_CategoryName,
                                  }
      ).Distinct().GroupBy(c => c.ASMCL_Id).Select(c => c.First()).ToArray();

                List<School_M_Section> allsetion = new List<School_M_Section>();
                allsetion = _ttcontext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.sectionlist = allsetion.Distinct().ToArray();

                data.stafflist = (from e in _ttcontext.HR_Master_Employee_DMO
                                  from staffAbbr in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                  where (e.MI_Id.Equals(data.MI_Id) && e.HRME_ActiveFlag.Equals(true) && staffAbbr.HRME_Id == e.HRME_Id && staffAbbr.MI_Id== data.MI_Id)
                                  select new TTFixingDTO
                                  {
                                      HRME_Id = e.HRME_Id,
                                      staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                  }
                                  ).Distinct().ToArray();

                List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                subjects = _ttcontext.IVRM_School_Master_SubjectsDMO.AsNoTracking().Where(s => s.MI_Id == data.MI_Id && s.ISMS_ActiveFlag == 1).ToList();
                data.subjectlist = subjects.Distinct().ToArray();

                List<TT_Master_PeriodDMO> periods = new List<TT_Master_PeriodDMO>();
                periods = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(p => p.MI_Id == data.MI_Id && p.TTMP_ActiveFlag == true).ToList();
                data.periodlist = periods.Distinct().ToArray();

                data.period_count = periods.Count();

                List<TT_Master_DayDMO> day = new List<TT_Master_DayDMO>();
                day = _ttcontext.TT_Master_DayDMO.AsNoTracking().Where(d => d.MI_Id == data.MI_Id && d.TTMD_ActiveFlag == true).ToList();
                data.daylist = day.Distinct().ToArray();

                data.day_count = day.Count();

                data.fix_day_period_list = (from a in _ttcontext.AcademicYear
                                            from b in _ttcontext.TTMasterCategoryDMO
                                            from c in _ttcontext.School_M_Class
                                            from d in _ttcontext.School_M_Section
                                            from e in _ttcontext.HR_Master_Employee_DMO
                                            from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                            from g in _ttcontext.TT_Master_PeriodDMO
                                            from h in _ttcontext.TT_Fixing_Day_PeriodDMO
                                            from i in _ttcontext.TT_Master_DayDMO
                                            where (a.MI_Id == h.MI_Id && a.ASMAY_Id == h.ASMAY_Id && b.MI_Id == h.MI_Id && b.TTMC_Id == h.TTMC_Id && c.MI_Id == h.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == h.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == h.MI_Id && e.HRME_Id == h.HRME_Id && f.MI_Id == h.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == h.MI_Id && g.TTMP_Id == h.TTMP_Id && i.MI_Id == h.MI_Id && i.TTMD_Id == h.TTMD_Id && h.MI_Id == data.MI_Id)
                                            select new TTFixingDTO
                                            {
                                                TTFDP_Id = h.TTFDP_Id,
                                                ASMAY_Year = a.ASMAY_Year,
                                                TTMC_CategoryName = b.TTMC_CategoryName,
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                ISMS_SubjectName = f.ISMS_SubjectName,
                                                TTMP_PeriodName = g.TTMP_PeriodName,
                                                TTMD_DayName = i.TTMD_DayName,
                                                TTFDP_ActiveFlag = h.TTFDP_ActiveFlag
                                            }
    ).Distinct().OrderBy(x => x.ASMCL_ClassName).ToArray();

                data.all_fix_day_staff_list = (from a in _ttcontext.AcademicYear
                                                    from e in _ttcontext.HR_Master_Employee_DMO
                                                    from g in _ttcontext.TT_Fixing_Day_StaffDMO
                                                        from i in _ttcontext.TT_Master_DayDMO
                                                    where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && i.MI_Id==g.MI_Id && i.TTMD_Id==g.TTMD_Id)
                                                    select new TTFixingDTO
                                                    {
                                                        TTFDS_Id = g.TTFDS_Id,
                                                        ASMAY_Year = a.ASMAY_Year,
                                                        staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                        TTMD_Id =g.TTMD_Id,
                                                        TTMD_DayName=i.TTMD_DayName,
                                                        TTFDS_SUbSelcFlag = g.TTFDS_SUbSelcFlag,
                                                        TTFDS_ActiveFlag = g.TTFDS_ActiveFlag,
                                                    }
     ).Distinct().OrderBy(x => x.staffName).ToArray();

                data.all_fix_day_subject_list = (from a in _ttcontext.AcademicYear
                                                      from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                                      from g in _ttcontext.TT_Fixing_Day_SubjectDMO
                                                      from i in _ttcontext.TT_Master_DayDMO
                                                      where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                                      select new TTFixingDTO
                                                      {
                                                          TTFDSU_Id = g.TTFDSU_Id,
                                                          ASMAY_Year = a.ASMAY_Year,                                                        
                                                          ISMS_SubjectName = e.ISMS_SubjectName,
                                                          TTMD_Id = g.TTMD_Id,
                                                          TTMD_DayName = i.TTMD_DayName,
                                                          TTFDSU_SUbSelcFlag = g.TTFDSU_SUbSelcFlag,
                                                          TTFDSU_ActiveFlag = g.TTFDSU_ActiveFlag,
                                                      }
     ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();

                data.all_fix_period_staff_list = (from a in _ttcontext.AcademicYear
                                                       from e in _ttcontext.HR_Master_Employee_DMO
                                                       from g in _ttcontext.TT_Fixing_Period_StaffDMO
                                                       from b in _ttcontext.TT_Master_PeriodDMO
                                                       where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                       select new TTFixingDTO
                                                       {
                                                           TTFPS_Id = g.TTFPS_Id,
                                                           ASMAY_Year = a.ASMAY_Year,
                                                           staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                           TTMP_Id = g.TTMP_Id,
                                                           TTMP_PeriodName = b.TTMP_PeriodName,
                                                           TTFPS_SUbSelcFlag = g.TTFPS_SUbSelcFlag,
                                                           TTFPS_ActiveFlag = g.TTFPS_ActiveFlag,
                                                       }
    ).Distinct().OrderBy(x => x.staffName).ToArray();

                data.all_fix_period_subject_list = (from a in _ttcontext.AcademicYear
                                                         from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                                         from g in _ttcontext.TT_Fixing_Period_SubjectDMO
                                                         from b in _ttcontext.TT_Master_PeriodDMO
                                                         where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                         select new TTFixingDTO
                                                         {
                                                             TTFPSU_Id = g.TTFPSU_Id,
                                                             ASMAY_Year = a.ASMAY_Year,
                                                             ISMS_SubjectName = e.ISMS_SubjectName,
                                                             TTMP_Id = g.TTMP_Id,
                                                             TTMP_PeriodName = b.TTMP_PeriodName,
                                                             TTFPSU_SUbSelcFlag = g.TTFPSU_SUbSelcFlag,
                                                             TTFPSU_ActiveFlag = g.TTFPSU_ActiveFlag,
                                                         }
     ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TTFixingDTO getpageedit1(int id)
        {
            TTFixingDTO page = new TTFixingDTO();
            try
            {
                List<TT_Fixing_Day_PeriodDMO> lorg = new List<TT_Fixing_Day_PeriodDMO>();
                lorg = _ttcontext.TT_Fixing_Day_PeriodDMO.AsNoTracking().Where(t => t.TTFDP_Id.Equals(id)).ToList();
                page.fix_day_period_edit = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }


        public TTFixingDTO getpageedit2(int id)
        {
            TTFixingDTO page = new TTFixingDTO();
            try
            {
                List<TT_Fixing_Day_StaffDMO> lorg = new List<TT_Fixing_Day_StaffDMO>();
                lorg = _ttcontext.TT_Fixing_Day_StaffDMO.AsNoTracking().Where(t => t.TTFDS_Id.Equals(id)).ToList();
                page.fix_day_staff_edit = lorg.ToArray();

                List<TT_Fixing_Day_Staff_ClassSectionDMO> lorgdetails = new List<TT_Fixing_Day_Staff_ClassSectionDMO>();
                lorgdetails = _ttcontext.TT_Fixing_Day_Staff_ClassSectionDMO.AsNoTracking().Where(t => t.TTFDS_Id.Equals(id) && t.TTFDSCC_ActiveFlag==true).ToList();
                page.fix_day_staff__classecedit = lorgdetails.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public TTFixingDTO getpageedit3(int id)
        {
            TTFixingDTO page = new TTFixingDTO();
            try
            {
                List<TT_Fixing_Day_SubjectDMO> lorg = new List<TT_Fixing_Day_SubjectDMO>();
                lorg = _ttcontext.TT_Fixing_Day_SubjectDMO.AsNoTracking().Where(t => t.TTFDSU_Id.Equals(id)).ToList();
                page.fix_day_subject_edit = lorg.ToArray();

                List<TT_Fixing_Day_Subject_ClassSectionDMO> lorgdetails = new List<TT_Fixing_Day_Subject_ClassSectionDMO>();
                lorgdetails = _ttcontext.TT_Fixing_Day_Subject_ClassSectionDMO.AsNoTracking().Where(t => t.TTFDSU_Id.Equals(id) && t.TTFDSUCC_ActiveFlag == true).ToList();
                page.fix_day_subject__classecedit = lorgdetails.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public TTFixingDTO getpageedit4(int id)
        {
            TTFixingDTO page = new TTFixingDTO();
            try
            {
                List<TT_Fixing_Period_StaffDMO> lorg = new List<TT_Fixing_Period_StaffDMO>();
                lorg = _ttcontext.TT_Fixing_Period_StaffDMO.AsNoTracking().Where(t => t.TTFPS_Id.Equals(id)).ToList();
                page.fix_period_staff_edit = lorg.ToArray();

                List<TT_Fixing_Period_Staff_ClassSectionDMO> lorgdetails = new List<TT_Fixing_Period_Staff_ClassSectionDMO>();
                lorgdetails = _ttcontext.TT_Fixing_Period_Staff_ClassSectionDMO.AsNoTracking().Where(t => t.TTFPS_Id.Equals(id) && t.TTFPSCC_ActiveFlag == true).ToList();
                page.fix_period_staff__classecedit = lorgdetails.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public TTFixingDTO getpageedit5(int id)
        {
            TTFixingDTO page = new TTFixingDTO();
            try
            {
                List<TT_Fixing_Period_SubjectDMO> lorg = new List<TT_Fixing_Period_SubjectDMO>();
                lorg = _ttcontext.TT_Fixing_Period_SubjectDMO.AsNoTracking().Where(t => t.TTFPSU_Id.Equals(id)).ToList();
                page.fix_period_subject_edit = lorg.ToArray();

                List<TT_Fixing_Period_Subject_ClassSectionDMO> lorgdetails = new List<TT_Fixing_Period_Subject_ClassSectionDMO>();
                lorgdetails = _ttcontext.TT_Fixing_Period_Subject_ClassSectionDMO.AsNoTracking().Where(t => t.TTFPSU_Id.Equals(id) && t.TTFPSUCC_ActiveFlag == true).ToList();
                page.fix_period_subject__classecedit = lorgdetails.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public TTFixingDTO getcategories(TTFixingDTO data)
        {
            try
            {
                List<TTMasterCategoryDMO> mcats = new List<TTMasterCategoryDMO>();
                mcats = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true ).ToList();
                data.categorybyyear = mcats.Distinct().ToArray();



            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
              }
            return data;

        }


        public TTFixingDTO getclasses(TTFixingDTO data)
        {
            try
            {
                data.classbycategory = (from a in _ttcontext.TTMasterCategoryDMO                                  
                                        from c in _ttcontext.School_M_Class
                                        from d in _ttcontext.TT_Final_Period_DistributionDMO
                                        from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                        where (e.TTMC_Id==a.TTMC_Id && d.ASMAY_Id==data.ASMAY_Id && c.ASMCL_Id==e.ASMCL_Id && e.TTFPD_Id==d.TTFPD_Id && a.MI_Id == c.MI_Id && d.MI_Id==a.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == data.TTMC_Id &&     d.TTFPD_ActiveFlag==true && e.TTFPDD_ActiveFlag==true)
                                        select new TTFixingDTO
                                        {
                                            ASMCL_Id = e.ASMCL_Id,
                                            ASMCL_ClassName = c.ASMCL_ClassName,
                                            TTMC_Id = e.TTMC_Id,
                                            TTMC_CategoryName = a.TTMC_CategoryName,
                                        }
                                      ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }                       
            return data;

        }

        public TTFixingDTO getperiods(TTFixingDTO data)
        {

            try
            {

                data.sectionbyclass = (from a in _ttcontext.TTMasterCategoryDMO
                                        from b in _ttcontext.School_M_Section
                                        from d in _ttcontext.TT_Final_Period_DistributionDMO
                                        from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                        where (e.TTMC_Id == a.TTMC_Id && d.ASMAY_Id == data.ASMAY_Id && b.ASMS_Id == e.ASMS_Id && e.TTFPD_Id == d.TTFPD_Id && a.MI_Id == b.MI_Id && d.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == data.TTMC_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true && e.ASMCL_Id==data.ASMCL_Id)
                                        select new TTFixingDTO
                                        {
                                            ASMS_Id = e.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            TTMC_Id = e.TTMC_Id,
                                            TTMC_CategoryName = a.TTMC_CategoryName,
                                        }
                                       ).Distinct().ToArray();

                data.periodbyclass = (from a in _ttcontext.TT_Master_PeriodDMO
                                      from b in _ttcontext.TT_Master_Period_ClasswiseDMO
                                      where (a.MI_Id == b.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && a.TTMP_Id == b.TTMP_Id && b.TTMPC_ActiveFlag == true)
                                      select new TTFixingDTO
                                      {
                                          TTMP_Id = b.TTMP_Id,
                                          TTMP_PeriodName = a.TTMP_PeriodName,
                                      }
    ).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return data;

        }


        public TTFixingDTO getstaff(TTFixingDTO data)
        {
            try
            {
                data.staffbyall = (from a in _ttcontext.TTMasterCategoryDMO
                                        from c in _ttcontext.HR_Master_Employee_DMO
                                        from d in _ttcontext.TT_Final_Period_DistributionDMO
                                        from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                        where (e.TTMC_Id == a.TTMC_Id && d.ASMAY_Id == data.ASMAY_Id && c.HRME_Id == d.HRME_Id  && e.ASMCL_Id==data.ASMCL_Id && e.TTMC_Id == data.TTMC_Id &&  e.ASMS_Id==data.ASMS_Id && e.TTFPD_Id == d.TTFPD_Id && a.MI_Id == c.MI_Id && d.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id &&  d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                        select new TTFixingDTO
                                        {
                                            HRME_Id = d.HRME_Id,
                                            staffName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == " " || c.HRME_EmployeeMiddleName == "0" ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == " " || c.HRME_EmployeeLastName == "0" ? " " : c.HRME_EmployeeLastName),
                                            TTMC_Id = e.TTMC_Id,
                                            TTMC_CategoryName = a.TTMC_CategoryName,
                                        }
                                      ).Distinct().ToArray();
                //Praveen commented
                //var asmay_id = _ttcontext.TT_Final_Period_DistributionDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFPD_ActiveFlag == true).Select(r => r.ASMAY_Id).FirstOrDefault();

                //New code added
                var asmay_id = _ttcontext.TT_Final_Period_DistributionDMO.Where(t => t.MI_Id == data.MI_Id && t.TTFPD_ActiveFlag == true && t.ASMAY_Id== data.ASMAY_Id).Select(r => r.ASMAY_Id).FirstOrDefault();


                data.maxvalue = (from a in _ttcontext.TT_Master_Period_ClasswiseDMO
                                 from b in _ttcontext.TT_Master_PeriodDMO
                                 where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.TTMPC_ActiveFlag == true && a.ASMAY_Id == asmay_id)
                                 select new TTFixingDTO
                                 {
                                     ttperiod = Convert.ToInt32(b.TTMP_PeriodName),
                                     ttmpid = b.TTMP_Id
                                 }).Max(f => f.ttperiod);


            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return data;

        }

        public TTFixingDTO getsubjects(TTFixingDTO data)
        {
            try
            {
                data.subjectbystaff = (
                                   from c in _ttcontext.IVRM_School_Master_SubjectsDMO
                                   from d in _ttcontext.TT_Final_Period_DistributionDMO
                                   from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                   where (  d.ASMAY_Id == data.ASMAY_Id && c.ISMS_Id == e.ISMS_Id && d.HRME_Id==data.HRME_Id && e.ASMCL_Id == data.ASMCL_Id && e.TTMC_Id == data.TTMC_Id && e.ASMS_Id == data.ASMS_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == c.MI_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                   select new TTFixingDTO
                                   {
                                       ISMS_Id = e.ISMS_Id,
                                       ISMS_SubjectName=c.ISMS_SubjectName,
                                   }
                                      ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return data;

        }
        public TTFixingDTO get_cls_sec_subs(TTFixingDTO data)
        {
            try
            {
                data.clssbystaff = (from c in _ttcontext.School_M_Class
                                        from d in _ttcontext.TT_Final_Period_DistributionDMO
                                        from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                        where (d.HRME_Id==data.HRME_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id == d.MI_Id && c.ASMCL_Id == e.ASMCL_Id && e.TTFPD_Id == d.TTFPD_Id &&  d.MI_Id == data.MI_Id &&  d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                        select new TTFixingDTO
                                        {
                                            ASMCL_Id = e.ASMCL_Id,
                                            ASMCL_ClassName = c.ASMCL_ClassName,
                                           
                                        }
                                      ).Distinct().ToArray();
                data.secsbystaff = (from c in _ttcontext.School_M_Section
                                     from d in _ttcontext.TT_Final_Period_DistributionDMO
                                     from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                     where (d.HRME_Id == data.HRME_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id == d.MI_Id && c.ASMS_Id == e.ASMS_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                     select new TTFixingDTO
                                     {
                                         ASMS_Id=e.ASMS_Id,
                                         ASMC_SectionName=c.ASMC_SectionName,

                                     }
                                      ).Distinct().ToArray();
                data.subsbystaff = (from c in _ttcontext.IVRM_School_Master_SubjectsDMO
                                     from d in _ttcontext.TT_Final_Period_DistributionDMO
                                     from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                     where (d.HRME_Id == data.HRME_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id == d.MI_Id && c.ISMS_Id == e.ISMS_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                     select new TTFixingDTO
                                     {
                                         ISMS_Id=e.ISMS_Id,
                                         ISMS_SubjectName=c.ISMS_SubjectName,

                                     }
                                      ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return data;

        }

        public TTFixingDTO get_cls_sec_staffs(TTFixingDTO data)
        {
            try
            {
                data.clssbysub = (from c in _ttcontext.School_M_Class
                                    from d in _ttcontext.TT_Final_Period_DistributionDMO
                                    from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                    where (e.ISMS_Id == data.ISMS_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id==d.MI_Id && c.ASMCL_Id == e.ASMCL_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                    select new TTFixingDTO
                                    {
                                        ASMCL_Id = e.ASMCL_Id,
                                        ASMCL_ClassName = c.ASMCL_ClassName,

                                    }
                                      ).Distinct().ToArray();
                data.secsbysub = (from c in _ttcontext.School_M_Section
                                    from d in _ttcontext.TT_Final_Period_DistributionDMO
                                    from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                    where (e.ISMS_Id == data.ISMS_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id==d.MI_Id && c.ASMS_Id == e.ASMS_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                    select new TTFixingDTO
                                    {
                                        ASMS_Id = e.ASMS_Id,
                                        ASMC_SectionName = c.ASMC_SectionName,

                                    }
                                      ).Distinct().ToArray();
                data.staffbysub = (from c in _ttcontext.HR_Master_Employee_DMO
                                    from d in _ttcontext.TT_Final_Period_DistributionDMO
                                    from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                    where (e.ISMS_Id == data.ISMS_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id==d.MI_Id && c.HRME_Id == d.HRME_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                    select new TTFixingDTO
                                    {
                                        HRME_Id = d.HRME_Id,
                                        staffName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == " " || c.HRME_EmployeeMiddleName == "0" ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == " " || c.HRME_EmployeeLastName == "0" ? " " : c.HRME_EmployeeLastName),

                                    }
                                      ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return data;

        }

        public TTFixingDTO getalldetailsviewrecords2(int id)
        {
            TTFixingDTO TTMC = new TTFixingDTO();
            try
            {
                TTMC.detailspopuparray2 = (from a in _ttcontext.AcademicYear
                                          from c in _ttcontext.School_M_Class
                                          from d in _ttcontext.School_M_Section
                                          from e in _ttcontext.HR_Master_Employee_DMO
                                          from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                          from g in _ttcontext.TT_Fixing_Day_StaffDMO
                                          from h in _ttcontext.TT_Fixing_Day_Staff_ClassSectionDMO
                                              from i in _ttcontext.TT_Master_DayDMO
                                          where (g.TTFDS_Id == h.TTFDS_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id &&  c.MI_Id == g.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == g.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && f.MI_Id == g.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == g.MI_Id && h.TTFDS_Id == id  && i.MI_Id==g.MI_Id && g.TTMD_Id==i.TTMD_Id)
                                          select new TTFixingDTO
                                          {
                                              TTFDSCC_Id = h.TTFDSCC_Id,
                                              ASMAY_Year = a.ASMAY_Year,                                            
                                              ASMCL_ClassName = c.ASMCL_ClassName,
                                              ASMC_SectionName = d.ASMC_SectionName,
                                              staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                              TTMD_DayName =i.TTMD_DayName,
                                              ISMS_SubjectName = f.ISMS_SubjectName,
                                              TTFDSCC_Periods = h.TTFDSCC_Periods,
                                              TTFDSCC_ActiveFlag = h.TTFDSCC_ActiveFlag,
                                          }
                ).Distinct().OrderBy(x => x.staffName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TTFixingDTO getalldetailsviewrecords3(int id)
        {
            TTFixingDTO TTMC = new TTFixingDTO();
            try
            {
                TTMC.detailspopuparray3 = (from a in _ttcontext.AcademicYear
                                           from c in _ttcontext.School_M_Class
                                           from d in _ttcontext.School_M_Section
                                           from e in _ttcontext.HR_Master_Employee_DMO
                                           from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                           from g in _ttcontext.TT_Fixing_Day_SubjectDMO
                                           from h in _ttcontext.TT_Fixing_Day_Subject_ClassSectionDMO
                                           from i in _ttcontext.TT_Master_DayDMO
                                           where (g.TTFDSU_Id == h.TTFDSU_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && c.MI_Id == g.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == g.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == g.MI_Id && e.HRME_Id == h.HRME_Id && f.MI_Id == g.MI_Id && f.ISMS_Id == g.ISMS_Id && g.MI_Id == g.MI_Id && h.TTFDSU_Id == id && i.MI_Id == g.MI_Id && g.TTMD_Id == i.TTMD_Id)
                                           select new TTFixingDTO
                                           {
                                               TTFDSUCC_Id = h.TTFDSUCC_Id,
                                               ASMAY_Year = a.ASMAY_Year,
                                               ASMCL_ClassName = c.ASMCL_ClassName,
                                               ASMC_SectionName = d.ASMC_SectionName,
                                               staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                               TTMD_DayName = i.TTMD_DayName,
                                               ISMS_SubjectName = f.ISMS_SubjectName,
                                               TTFDSUCC_Periods = h.TTFDSUCC_Periods,
                                               TTFDSUCC_ActiveFlag = h.TTFDSUCC_ActiveFlag,
                                           }
                ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TTFixingDTO getalldetailsviewrecords4(int id)
        {
            TTFixingDTO TTMC = new TTFixingDTO();
            try
            {
                TTMC.detailspopuparray4 = (from a in _ttcontext.AcademicYear
                                           from c in _ttcontext.School_M_Class
                                           from d in _ttcontext.School_M_Section
                                           from e in _ttcontext.HR_Master_Employee_DMO
                                           from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                           from g in _ttcontext.TT_Fixing_Period_StaffDMO
                                           from h in _ttcontext.TT_Fixing_Period_Staff_ClassSectionDMO
                                               from b in _ttcontext.TT_Master_PeriodDMO
                                           where (g.TTFPS_Id == h.TTFPS_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && c.MI_Id == g.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == g.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && f.MI_Id == g.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == g.MI_Id && h.TTFPS_Id == id && b.MI_Id == g.MI_Id && g.TTMP_Id == b.TTMP_Id)
                                           select new TTFixingDTO
                                           {
                                               TTFPSCC_Id = h.TTFPSCC_Id,
                                               ASMAY_Year = a.ASMAY_Year,
                                               ASMCL_ClassName = c.ASMCL_ClassName,
                                               ASMC_SectionName = d.ASMC_SectionName,
                                               staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                               TTMP_PeriodName =b.TTMP_PeriodName,
                                               ISMS_SubjectName = f.ISMS_SubjectName,
                                               TTFPSCC_Days = h.TTFPSCC_Days,
                                               TTFPSCC_ActiveFlag = h.TTFPSCC_ActiveFlag,
                                           }
                ).Distinct().OrderBy(x => x.staffName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TTFixingDTO getalldetailsviewrecords5(int id)
        {
            TTFixingDTO TTMC = new TTFixingDTO();
            try
            {
                TTMC.detailspopuparray5 = (from a in _ttcontext.AcademicYear
                                           from c in _ttcontext.School_M_Class
                                           from d in _ttcontext.School_M_Section
                                           from e in _ttcontext.HR_Master_Employee_DMO
                                           from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                           from g in _ttcontext.TT_Fixing_Period_SubjectDMO
                                           from h in _ttcontext.TT_Fixing_Period_Subject_ClassSectionDMO
                                           from b in _ttcontext.TT_Master_PeriodDMO
                                           where (g.TTFPSU_Id == h.TTFPSU_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && c.MI_Id == g.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == g.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == g.MI_Id && e.HRME_Id == h.HRME_Id && f.MI_Id == g.MI_Id && f.ISMS_Id == g.ISMS_Id && g.MI_Id == g.MI_Id && h.TTFPSU_Id == id && b.MI_Id == g.MI_Id && g.TTMP_Id == b.TTMP_Id)
                                           select new TTFixingDTO
                                           {
                                               TTFPSUCC_Id = h.TTFPSUCC_Id,
                                               ASMAY_Year = a.ASMAY_Year,
                                               ASMCL_ClassName = c.ASMCL_ClassName,
                                               ASMC_SectionName = d.ASMC_SectionName,
                                               staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                               TTMP_PeriodName = b.TTMP_PeriodName,
                                               ISMS_SubjectName = f.ISMS_SubjectName,
                                               TTFPSUCC_Days = h.TTFPSUCC_Days,
                                               TTFPSUCC_ActiveFlag = h.TTFPSUCC_ActiveFlag,
                                           }
                ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;
        }

        public TTFixingDTO deactivate1(TTFixingDTO data)
        {
            TT_Fixing_Day_PeriodDMO pge = Mapper.Map<TT_Fixing_Day_PeriodDMO>(data);
            if (pge.TTFDP_Id > 0)
            {
                var result = _ttcontext.TT_Fixing_Day_PeriodDMO.AsNoTracking().Single(t => t.TTFDP_Id == pge.TTFDP_Id);
                if (result.TTFDP_ActiveFlag == true)
                {
                    result.TTFDP_ActiveFlag = false;
                }
                else
                {
                    result.TTFDP_ActiveFlag = true;
                }
                _ttcontext.Update(result);
                var flag = _ttcontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            return data;
        }


        public TTFixingDTO deactivate2(TTFixingDTO data)
        {
            TT_Fixing_Day_StaffDMO pge = Mapper.Map<TT_Fixing_Day_StaffDMO>(data);
            if (pge.TTFDS_Id > 0)
            {
                var result = _ttcontext.TT_Fixing_Day_StaffDMO.AsNoTracking().Single(t => t.TTFDS_Id == pge.TTFDS_Id);
                if (result.TTFDS_ActiveFlag == true)
                {
                    result.TTFDS_ActiveFlag = false;
                }
                else
                {
                    result.TTFDS_ActiveFlag = true;
                }              
                _ttcontext.Update(result);
                var flag = _ttcontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }
        public TTFixingDTO deactivate3(TTFixingDTO data)
        {
            TT_Fixing_Day_SubjectDMO pge = Mapper.Map<TT_Fixing_Day_SubjectDMO>(data);
            if (pge.TTFDSU_Id > 0)
            {
                var result = _ttcontext.TT_Fixing_Day_SubjectDMO.AsNoTracking().Single(t => t.TTFDSU_Id == pge.TTFDSU_Id);
                if (result.TTFDSU_ActiveFlag == true)
                {
                    result.TTFDSU_ActiveFlag = false;
                }
                else
                {
                    result.TTFDSU_ActiveFlag = true;
                }
                _ttcontext.Update(result);
                var flag = _ttcontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }

        public TTFixingDTO deactivate4(TTFixingDTO data)
        {
            TT_Fixing_Period_StaffDMO pge = Mapper.Map<TT_Fixing_Period_StaffDMO>(data);
            if (pge.TTFPS_Id > 0)
            {
                var result = _ttcontext.TT_Fixing_Period_StaffDMO.AsNoTracking().Single(t => t.TTFPS_Id == pge.TTFPS_Id);
                if (result.TTFPS_ActiveFlag == true)
                {
                    result.TTFPS_ActiveFlag = false;
                }
                else
                {
                    result.TTFPS_ActiveFlag = true;
                }
                _ttcontext.Update(result);
                var flag = _ttcontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }

        public TTFixingDTO deactivate5(TTFixingDTO data)
        {
            TT_Fixing_Period_SubjectDMO pge = Mapper.Map<TT_Fixing_Period_SubjectDMO>(data);
            if (pge.TTFPSU_Id > 0)
            {
                var result = _ttcontext.TT_Fixing_Period_SubjectDMO.AsNoTracking().Single(t => t.TTFPSU_Id == pge.TTFPSU_Id);
                if (result.TTFPSU_ActiveFlag == true)
                {
                    result.TTFPSU_ActiveFlag = false;
                }
                else
                {
                    result.TTFPSU_ActiveFlag = true;
                }
                _ttcontext.Update(result);
                var flag = _ttcontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }

        public TTFixingDTO deactivate(TTFixingDTO data)
        {
            TT_Final_Period_DistributionDMO pge = Mapper.Map<TT_Final_Period_DistributionDMO>(data);
            if (pge.TTFPD_Id > 0)
            {
                var result = _ttcontext.TT_Final_Period_DistributionDMO.AsNoTracking().Single(t => t.TTFPD_Id == pge.TTFPD_Id);
                if (result.TTFPD_ActiveFlag == true)
                {
                    result.TTFPD_ActiveFlag = false;
                }
                else
                {
                    result.TTFPD_ActiveFlag = true;
                }
                _ttcontext.Update(result);
                var flag = _ttcontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }
        public TTFixingDTO getpageedit(int id)
        {
            TTFixingDTO page = new TTFixingDTO();
            try
            {
                List<TT_Final_Period_DistributionDMO> lorg = new List<TT_Final_Period_DistributionDMO>();
                lorg = _ttcontext.TT_Final_Period_DistributionDMO.AsNoTracking().Where(t => t.TTFPD_Id.Equals(id)).ToList();

                List<TT_Final_Period_Distribution_DetailedDMO> lorgdetails = new List<TT_Final_Period_Distribution_DetailedDMO>();
                lorgdetails = _ttcontext.TT_Final_Period_Distribution_DetailedDMO.AsNoTracking().Where(t => t.TTFPD_Id.Equals(id)).ToList();
              

                var count = _ttcontext.TT_Final_Period_Distribution_DetailedDMO.Where(x=>x.TTFPD_Id.Equals(id) && x.TTFPDD_ActiveFlag==true).Sum(t => t.TTFPD_TotalPeriods);
               
               

                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TTFixingDTO deleterec(int id)
        {
            TTFixingDTO period = new TTFixingDTO();
            try
            {
                var result1 = _ttcontext.TT_Master_Day_Period_TimeDMO.Single(s => s.TTMDPT_Id == id);
                _ttcontext.TT_Master_Day_Period_TimeDMO.Remove(result1);
                var contactExists = _ttcontext.SaveChanges();
                if (contactExists == 1)
                {
                    period.returnval = true;
                }
                else
                {
                    period.returnval = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return period;
        }


    }
}
