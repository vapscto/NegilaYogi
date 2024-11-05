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
    public class TTRestrictionImpl : Interfaces.RestrictionInterface
    {
        private static ConcurrentDictionary<string, TTMasterCategoryDTO> _login =
         new ConcurrentDictionary<string, TTMasterCategoryDTO>();


        public TTContext _ttcontext;
        readonly ILogger<TTRestrictionImpl> _logger;
        public TTRestrictionImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public TTRestrictionDTO savedetail1(TTRestrictionDTO objday)
        {
            TT_Restricting_Day_PeriodDMO objpge = Mapper.Map<TT_Restricting_Day_PeriodDMO>(objday);
            try
            {

                var fix_count = _ttcontext.TT_Fixing_Day_PeriodDMO.AsNoTracking().Where(r => r.MI_Id == objpge.MI_Id && r.ASMAY_Id == objpge.ASMAY_Id && r.TTMC_Id == objpge.TTMC_Id && r.ASMCL_Id == objpge.ASMCL_Id && r.ASMS_Id == objpge.ASMS_Id && r.TTMD_Id == objpge.TTMD_Id && r.TTMP_Id == objpge.TTMP_Id && r.HRME_Id == objpge.HRME_Id && r.ISMS_Id == objpge.ISMS_Id && r.TTFDP_ActiveFlag==true).Count();
               if (fix_count == 0)
                {
                    if (objpge.TTRDP_Id > 0)
                    {
  var resultCount = _ttcontext.TT_Restricting_Day_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.ASMCL_Id == objpge.ASMCL_Id && t.ASMS_Id == objpge.ASMS_Id && t.TTMC_Id == objpge.TTMC_Id && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id && t.HRME_Id==objpge.HRME_Id  && t.TTRDP_Id != objpge.TTRDP_Id).Count();
                        if (resultCount == 0)
                        {
                            var result = _ttcontext.TT_Restricting_Day_PeriodDMO.Single(t => t.TTRDP_Id == objpge.TTRDP_Id && t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id);

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
                        var resultCount = _ttcontext.TT_Restricting_Day_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.ASMCL_Id == objpge.ASMCL_Id && t.ASMS_Id == objpge.ASMS_Id && t.TTMC_Id == objpge.TTMC_Id && t.TTMD_Id == objpge.TTMD_Id && t.TTMP_Id == objpge.TTMP_Id && t.HRME_Id == objpge.HRME_Id).Count();
                        if (resultCount > 0)
                        {
                            objday.returnduplicatestatus = "Duplicate";
                            return objday;
                        }
                        else
                        {                          
                            objpge.TTRDP_AllotedFlag = "No";
                            objpge.TTRDP_ActiveFlag = true;
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
                    objday.returnfixstatus = "Fixed";
                    return objday;
                }
                objday.restrict_day_period_list = (from a in _ttcontext.AcademicYear
                                            from b in _ttcontext.TTMasterCategoryDMO
                                            from c in _ttcontext.School_M_Class
                                            from d in _ttcontext.School_M_Section
                                            from e in _ttcontext.HR_Master_Employee_DMO
                                            from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                            from g in _ttcontext.TT_Master_PeriodDMO
                                            from h in _ttcontext.TT_Restricting_Day_PeriodDMO
                                            from i in _ttcontext.TT_Master_DayDMO
                                            where (a.MI_Id == h.MI_Id && a.ASMAY_Id == h.ASMAY_Id && b.MI_Id == h.MI_Id && b.TTMC_Id == h.TTMC_Id && c.MI_Id == h.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == h.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == h.MI_Id && e.HRME_Id == h.HRME_Id && f.MI_Id == h.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == h.MI_Id && g.TTMP_Id == h.TTMP_Id && i.MI_Id == h.MI_Id && i.TTMD_Id == h.TTMD_Id && h.MI_Id == objday.MI_Id)
                                            select new TTRestrictionDTO
                                            {
                                                TTRDP_Id = h.TTRDP_Id,
                                                ASMAY_Year = a.ASMAY_Year,
                                                TTMC_CategoryName = b.TTMC_CategoryName,
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                ISMS_SubjectName = f.ISMS_SubjectName,
                                                TTMP_PeriodName = g.TTMP_PeriodName,
                                                TTMD_DayName = i.TTMD_DayName,
                                                TTRDP_ActiveFlag = h.TTRDP_ActiveFlag
                                            }
  ).Distinct().OrderBy(x => x.ASMCL_ClassName).ToArray();


            }
            catch (Exception ee)
            {

                Console.WriteLine(ee.Message);
            }
            return objday;
        }

        public TTRestrictionDTO savedetail2(TTRestrictionDTO _category)
        {
            TT_Restricting_Day_StaffDMO objpge = Mapper.Map<TT_Restricting_Day_StaffDMO>(_category);
            try
            {
                var fix_count = _ttcontext.TT_Fixing_Day_StaffDMO.AsNoTracking().Where(r => r.MI_Id == objpge.MI_Id && r.ASMAY_Id == objpge.ASMAY_Id && r.HRME_Id == objpge.HRME_Id && r.TTMD_Id == objpge.TTMD_Id && r.TTFDS_ActiveFlag==true).Count();
                if (fix_count == 0)
                {
                    if (_category.TTRDS_SUbSelcFlag == false)
                    {
                        if (objpge.TTRDS_Id > 0)
                        {

                            var result0 = _ttcontext.TT_Restricting_Day_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id) && t.TTRDS_Id != objpge.TTRDS_Id);
                            if (result0.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                var result = _ttcontext.TT_Restricting_Day_StaffDMO.Single(t => t.TTRDS_Id.Equals(objpge.TTRDS_Id) && t.MI_Id.Equals(objpge.MI_Id));
                                result.ASMAY_Id = objpge.ASMAY_Id;
                                result.HRME_Id = objpge.HRME_Id;
                                result.TTMD_Id = objpge.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTRDS_AllotedFlag = "No";
                                result.TTRDS_ActiveFlag = true;
                                result.TTRDS_SUbSelcFlag = false;
                                _ttcontext.Update(result);
                                var contactExists = _ttcontext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    List<TT_Restricting_Day_Staff_ClassSectionDMO> deactivelist = new List<TT_Restricting_Day_Staff_ClassSectionDMO>();
                                    deactivelist = _ttcontext.TT_Restricting_Day_Staff_ClassSectionDMO.AsNoTracking().Where(s => s.TTRDS_Id == objpge.TTRDS_Id).ToList();
                                    if (deactivelist.Count() > 0)
                                    {
                                        List<TT_Restricting_Day_Staff_ClassSectionDMO> lorg = new List<TT_Restricting_Day_Staff_ClassSectionDMO>();
                                        lorg = _ttcontext.TT_Restricting_Day_Staff_ClassSectionDMO.Where(t => t.TTRDS_Id.Equals(objpge.TTRDS_Id)).ToList();
                                        if (lorg.Any())
                                        {
                                            for (int i = 0; lorg.Count > i; i++)
                                            {
                                                lorg.ElementAt(i).TTRDSCC_ActiveFlag = false;
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
                            var result = _ttcontext.TT_Restricting_Day_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id));
                            if (result.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.TTRDS_AllotedFlag = "No";
                                objpge.TTRDS_ActiveFlag = true;
                                objpge.TTRDS_SUbSelcFlag = false;
                                _ttcontext.Add(objpge);
                                var contactExists = _ttcontext.SaveChanges();

                                var result123 = _ttcontext.TT_Restricting_Day_StaffDMO.Max(t => t.TTRDS_Id);
                                if (contactExists == 1)
                                {
                                    List<TT_Restricting_Day_Staff_ClassSectionDMO> deactivelist = new List<TT_Restricting_Day_Staff_ClassSectionDMO>();
                                    deactivelist = _ttcontext.TT_Restricting_Day_Staff_ClassSectionDMO.AsNoTracking().Where(s => s.TTRDS_Id == result123).ToList();
                                    if (deactivelist.Count() > 0)
                                    {
                                        List<TT_Restricting_Day_Staff_ClassSectionDMO> lorg = new List<TT_Restricting_Day_Staff_ClassSectionDMO>();
                                        lorg = _ttcontext.TT_Restricting_Day_Staff_ClassSectionDMO.Where(t => t.TTRDS_Id.Equals(result123)).ToList();
                                        if (lorg.Any())
                                        {
                                            for (int i = 0; lorg.Count > i; i++)
                                            {
                                                lorg.ElementAt(i).TTRDSCC_ActiveFlag = false;
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
                    else if (_category.TTRDS_SUbSelcFlag == true)
                    {
                        if (objpge.TTRDS_Id > 0)
                        {

                            var result0 = _ttcontext.TT_Restricting_Day_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id) && t.TTRDS_Id != objpge.TTRDS_Id);
                            if (result0.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {

                                var result = _ttcontext.TT_Restricting_Day_StaffDMO.Single(t => t.TTRDS_Id.Equals(objpge.TTRDS_Id) && t.MI_Id.Equals(objpge.MI_Id));
                                result.ASMAY_Id = objpge.ASMAY_Id;
                                result.HRME_Id = objpge.HRME_Id;
                                result.TTMD_Id = objpge.TTMD_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TTRDS_AllotedFlag = "No";
                                result.TTRDS_ActiveFlag = true;
                                result.TTRDS_SUbSelcFlag = true;
                                _ttcontext.Update(result);
                                var contactExists = _ttcontext.SaveChanges();
                                if (contactExists == 1)
                                {
                                    deletepagesRightgrid2(objpge.TTRDS_Id);
                                    for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                    {
                                        TTRestrictionDTO ttclass = new TTRestrictionDTO();
                                        TT_Restricting_Day_Staff_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Restricting_Day_Staff_ClassSectionDMO>(ttclass);
                                        tt_dis_det.TTRDS_Id = objpge.TTRDS_Id;
                                        tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                        tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                        tt_dis_det.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                        tt_dis_det.TTRDSCC_Periods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                        tt_dis_det.TTRDSCC_ActiveFlag = true;
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
                            var result = _ttcontext.TT_Restricting_Day_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id));
                            if (result.Count() > 0)
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                            else
                            {
                                objpge.CreatedDate = DateTime.Now;
                                objpge.UpdatedDate = DateTime.Now;
                                objpge.TTRDS_AllotedFlag = "No";
                                objpge.TTRDS_ActiveFlag = true;
                                objpge.TTRDS_SUbSelcFlag = true;
                                _ttcontext.Add(objpge);
                                var contactExists = _ttcontext.SaveChanges();

                                var result123 = _ttcontext.TT_Restricting_Day_StaffDMO.Max(t => t.TTRDS_Id);
                                if (contactExists == 1)
                                {
                                    for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                    {
                                        TTRestrictionDTO ttclass = new TTRestrictionDTO();
                                        TT_Restricting_Day_Staff_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Restricting_Day_Staff_ClassSectionDMO>(ttclass);
                                        tt_dis_det.TTRDS_Id = result123;
                                        tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                        tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                        tt_dis_det.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                        tt_dis_det.TTRDSCC_Periods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                        tt_dis_det.TTRDSCC_ActiveFlag = true;
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
                                            _ttcontext.Remove(_ttcontext.TT_Restricting_Day_Staff_ClassSectionDMO.Where(t => t.TTRDS_Id.Equals(result123)));
                                            _ttcontext.Remove(_ttcontext.TT_Restricting_Day_StaffDMO.Where(t => t.TTRDS_Id.Equals(result123)));
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
                    _category.returnfixstatus = "Fixed";
                    return _category;
                }
                    _category.all_restrict_day_staff_list = (from a in _ttcontext.AcademicYear
                                               from e in _ttcontext.HR_Master_Employee_DMO
                                               from g in _ttcontext.TT_Restricting_Day_StaffDMO
                                                   from i in _ttcontext.TT_Master_DayDMO
                                               where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == _category.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                               select new TTRestrictionDTO
                                               {
                                                 TTRDS_Id = g.TTRDS_Id,
                                                   ASMAY_Year = a.ASMAY_Year,
                                                   staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                   TTMD_Id = g.TTMD_Id,
                                                   TTMD_DayName = i.TTMD_DayName,
                                                   TTRDS_SUbSelcFlag = g.TTRDS_SUbSelcFlag,
                                                   TTRDS_ActiveFlag = g.TTRDS_ActiveFlag,
                                               }
     ).Distinct().OrderBy(x => x.staffName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TTRestrictionDTO savedetail3(TTRestrictionDTO _category)
        {
            TT_Restricting_Day_SubjectDMO objpge = Mapper.Map<TT_Restricting_Day_SubjectDMO>(_category);
            try
            {
                var fix_count = _ttcontext.TT_Fixing_Day_SubjectDMO.AsNoTracking().Where(r => r.MI_Id == objpge.MI_Id && r.ASMAY_Id == objpge.ASMAY_Id && r.ISMS_Id == objpge.ISMS_Id && r.TTMD_Id == objpge.TTMD_Id && r.TTFDSU_ActiveFlag==true).Count();
                if (fix_count == 0)
                {
                    if (_category.TTRDSU_SUbSelcFlag == false)
                {
                    if (objpge.TTRDSU_Id > 0)
                    {

                        var result0 = _ttcontext.TT_Restricting_Day_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id) && t.TTRDSU_Id != objpge.TTRDSU_Id);
                        if (result0.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result = _ttcontext.TT_Restricting_Day_SubjectDMO.Single(t => t.TTRDSU_Id.Equals(objpge.TTRDSU_Id) && t.MI_Id.Equals(objpge.MI_Id));
                            result.ASMAY_Id = objpge.ASMAY_Id;
                            result.ISMS_Id = objpge.ISMS_Id;
                            result.TTMD_Id = objpge.TTMD_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.TTRDSU_AllotedFlag = "No";
                            result.TTRDSU_ActiveFlag = true;
                            result.TTRDSU_SUbSelcFlag = false;
                            _ttcontext.Update(result);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                List<TT_Restricting_Day_Subject_ClassSectionDMO> deactivelist = new List<TT_Restricting_Day_Subject_ClassSectionDMO>();
                                deactivelist = _ttcontext.TT_Restricting_Day_Subject_ClassSectionDMO.AsNoTracking().Where(s => s.TTRDSU_Id == objpge.TTRDSU_Id).ToList();
                                if (deactivelist.Count() > 0)
                                {
                                    List<TT_Restricting_Day_Subject_ClassSectionDMO> lorg = new List<TT_Restricting_Day_Subject_ClassSectionDMO>();
                                    lorg = _ttcontext.TT_Restricting_Day_Subject_ClassSectionDMO.Where(t => t.TTRDSU_Id.Equals(objpge.TTRDSU_Id)).ToList();
                                    if (lorg.Any())
                                    {
                                        for (int i = 0; lorg.Count > i; i++)
                                        {
                                            lorg.ElementAt(i).TTRDSUCC_ActiveFlag = false;
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
                        var result = _ttcontext.TT_Restricting_Day_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id));
                        if (result.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.TTRDSU_AllotedFlag = "No";
                            objpge.TTRDSU_ActiveFlag = true;
                            objpge.TTRDSU_SUbSelcFlag = false;
                            _ttcontext.Add(objpge);
                            var contactExists = _ttcontext.SaveChanges();

                            var result123 = _ttcontext.TT_Restricting_Day_SubjectDMO.Max(t => t.TTRDSU_Id);
                            if (contactExists == 1)
                            {
                                List<TT_Restricting_Day_Subject_ClassSectionDMO> deactivelist = new List<TT_Restricting_Day_Subject_ClassSectionDMO>();
                                deactivelist = _ttcontext.TT_Restricting_Day_Subject_ClassSectionDMO.AsNoTracking().Where(s => s.TTRDSU_Id == result123).ToList();
                                if (deactivelist.Count() > 0)
                                {
                                    List<TT_Restricting_Day_Subject_ClassSectionDMO> lorg = new List<TT_Restricting_Day_Subject_ClassSectionDMO>();
                                    lorg = _ttcontext.TT_Restricting_Day_Subject_ClassSectionDMO.Where(t => t.TTRDSU_Id.Equals(result123)).ToList();
                                    if (lorg.Any())
                                    {
                                        for (int i = 0; lorg.Count > i; i++)
                                        {
                                            lorg.ElementAt(i).TTRDSUCC_ActiveFlag = false;
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
                else if (_category.TTRDSU_SUbSelcFlag == true)
                {
                    if (objpge.TTRDSU_Id > 0)
                    {

                        var result0 = _ttcontext.TT_Restricting_Day_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id) && t.TTRDSU_Id != objpge.TTRDSU_Id);
                        if (result0.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result = _ttcontext.TT_Restricting_Day_SubjectDMO.Single(t => t.TTRDSU_Id.Equals(objpge.TTRDSU_Id) && t.MI_Id.Equals(objpge.MI_Id));
                            result.ASMAY_Id = objpge.ASMAY_Id;
                            result.ISMS_Id = objpge.ISMS_Id;
                            result.TTMD_Id = objpge.TTMD_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.TTRDSU_AllotedFlag = "No";
                            result.TTRDSU_ActiveFlag = true;
                            result.TTRDSU_SUbSelcFlag = true;
                            _ttcontext.Update(result);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                deletepagesRightgrid3(objpge.TTRDSU_Id);
                                for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                {
                                    TTRestrictionDTO ttclass = new TTRestrictionDTO();
                                    TT_Restricting_Day_Subject_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Restricting_Day_Subject_ClassSectionDMO>(ttclass);
                                    tt_dis_det.TTRDSU_Id = objpge.TTRDSU_Id;
                                    tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                    tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);                                
                                    tt_dis_det.HRME_Id = Convert.ToInt64(_category.TempararyArrayList[i].HRME_Id);
                                    tt_dis_det.TTRDSUCC_Periods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                    tt_dis_det.TTRDSUCC_ActiveFlag = true;
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
                        var result = _ttcontext.TT_Restricting_Day_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMD_Id.Equals(objpge.TTMD_Id));
                        if (result.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.TTRDSU_AllotedFlag = "No";
                            objpge.TTRDSU_ActiveFlag = true;
                            objpge.TTRDSU_SUbSelcFlag = true;
                            _ttcontext.Add(objpge);
                            var contactExists = _ttcontext.SaveChanges();

                            var result123 = _ttcontext.TT_Restricting_Day_SubjectDMO.Max(t => t.TTRDSU_Id);
                            if (contactExists == 1)
                            {
                                for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                {
                                    TTRestrictionDTO ttclass = new TTRestrictionDTO();
                                    TT_Restricting_Day_Subject_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Restricting_Day_Subject_ClassSectionDMO>(ttclass);
                                    tt_dis_det.TTRDSU_Id = result123;
                                    tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                    tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                    tt_dis_det.HRME_Id = Convert.ToInt64(_category.TempararyArrayList[i].HRME_Id);
                                    tt_dis_det.TTRDSUCC_Periods = Convert.ToInt32(_category.TempararyArrayList[i].NOP);
                                    tt_dis_det.TTRDSUCC_ActiveFlag = true;
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
                                        _ttcontext.Remove(_ttcontext.TT_Restricting_Day_Subject_ClassSectionDMO.Where(t => t.TTRDSU_Id.Equals(result123)));
                                        _ttcontext.Remove(_ttcontext.TT_Restricting_Day_SubjectDMO.Where(t => t.TTRDSU_Id.Equals(result123)));
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
                    _category.returnfixstatus = "Fixed";
                    return _category;
                }
                _category.all_restrict_day_subject_list = (from a in _ttcontext.AcademicYear
                                                    from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                                    from g in _ttcontext.TT_Restricting_Day_SubjectDMO                                              
                                                    from i in _ttcontext.TT_Master_DayDMO
                                                    where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == _category.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                                    select new TTRestrictionDTO
                                                    {
                                                        TTRDSU_Id = g.TTRDSU_Id,
                                                        ASMAY_Year = a.ASMAY_Year,
                                                        ISMS_SubjectName=e.ISMS_SubjectName,
                                                        TTMD_Id = g.TTMD_Id,
                                                        TTMD_DayName = i.TTMD_DayName,
                                                        TTRDSU_SUbSelcFlag = g.TTRDSU_SUbSelcFlag,
                                                        TTRDSU_ActiveFlag = g.TTRDSU_ActiveFlag,
                                                    }
     ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }
        public TTRestrictionDTO savedetail4(TTRestrictionDTO _category)
        {
            TT_Restricting_Period_StaffDMO objpge = Mapper.Map<TT_Restricting_Period_StaffDMO>(_category);
            try
            {
                var fix_count = _ttcontext.TT_Fixing_Period_StaffDMO.AsNoTracking().Where(r => r.MI_Id == objpge.MI_Id && r.ASMAY_Id == objpge.ASMAY_Id && r.HRME_Id == objpge.HRME_Id && r.TTMP_Id == objpge.TTMP_Id && r.TTFPS_ActiveFlag==true).Count();
                if (fix_count == 0)
                {
                    if (_category.TTRPS_SUbSelcFlag == false)
                {
                    if (objpge.TTRPS_Id > 0)
                    {

                        var result0 = _ttcontext.TT_Restricting_Period_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id) && t.TTRPS_Id != objpge.TTRPS_Id);
                        if (result0.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result = _ttcontext.TT_Restricting_Period_StaffDMO.Single(t => t.TTRPS_Id.Equals(objpge.TTRPS_Id) && t.MI_Id.Equals(objpge.MI_Id));
                            result.ASMAY_Id = objpge.ASMAY_Id;
                            result.HRME_Id = objpge.HRME_Id;
                            result.TTMP_Id = objpge.TTMP_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.TTRPS_AllotedFlag = "No";
                            result.TTRPS_ActiveFlag = true;
                            result.TTRPS_SUbSelcFlag = false;
                            _ttcontext.Update(result);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                List<TT_Restricting_Period_Staff_ClassSectionDMO> deactivelist = new List<TT_Restricting_Period_Staff_ClassSectionDMO>();
                                deactivelist = _ttcontext.TT_Restricting_Period_Staff_ClassSectionDMO.AsNoTracking().Where(s => s.TTRPS_Id == objpge.TTRPS_Id).ToList();
                                if (deactivelist.Count() > 0)
                                {
                                    List<TT_Restricting_Period_Staff_ClassSectionDMO> lorg = new List<TT_Restricting_Period_Staff_ClassSectionDMO>();
                                    lorg = _ttcontext.TT_Restricting_Period_Staff_ClassSectionDMO.Where(t => t.TTRPS_Id.Equals(objpge.TTRPS_Id)).ToList();
                                    if (lorg.Any())
                                    {
                                        for (int i = 0; lorg.Count > i; i++)
                                        {
                                            lorg.ElementAt(i).TTRPSCC_ActiveFlag = false;
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
                        var result = _ttcontext.TT_Restricting_Period_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id));
                        if (result.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.TTRPS_AllotedFlag = "No";
                            objpge.TTRPS_ActiveFlag = true;
                            objpge.TTRPS_SUbSelcFlag = false;
                            _ttcontext.Add(objpge);
                            var contactExists = _ttcontext.SaveChanges();

                            var result123 = _ttcontext.TT_Restricting_Period_StaffDMO.Max(t => t.TTRPS_Id);
                            if (contactExists == 1)
                            {
                                List<TT_Restricting_Period_Staff_ClassSectionDMO> deactivelist = new List<TT_Restricting_Period_Staff_ClassSectionDMO>();
                                deactivelist = _ttcontext.TT_Restricting_Period_Staff_ClassSectionDMO.AsNoTracking().Where(s => s.TTRPS_Id == result123).ToList();
                                if (deactivelist.Count() > 0)
                                {
                                    List<TT_Restricting_Period_Staff_ClassSectionDMO> lorg = new List<TT_Restricting_Period_Staff_ClassSectionDMO>();
                                    lorg = _ttcontext.TT_Restricting_Period_Staff_ClassSectionDMO.Where(t => t.TTRPS_Id.Equals(result123)).ToList();
                                    if (lorg.Any())
                                    {
                                        for (int i = 0; lorg.Count > i; i++)
                                        {
                                            lorg.ElementAt(i).TTRPSCC_ActiveFlag = false;
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
                else if (_category.TTRPS_SUbSelcFlag == true)
                {
                    if (objpge.TTRPS_Id > 0)
                    {

                        var result0 = _ttcontext.TT_Restricting_Period_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id) && t.TTRPS_Id != objpge.TTRPS_Id);
                        if (result0.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result = _ttcontext.TT_Restricting_Period_StaffDMO.Single(t => t.TTRPS_Id.Equals(objpge.TTRPS_Id) && t.MI_Id.Equals(objpge.MI_Id));
                            result.ASMAY_Id = objpge.ASMAY_Id;
                            result.HRME_Id = objpge.HRME_Id;
                            result.TTMP_Id = objpge.TTMP_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.TTRPS_AllotedFlag = "No";
                            result.TTRPS_ActiveFlag = true;
                            result.TTRPS_SUbSelcFlag = true;
                            _ttcontext.Update(result);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                deletepagesRightgrid4(objpge.TTRPS_Id);
                                for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                {
                                    TTRestrictionDTO ttclass = new TTRestrictionDTO();
                                    TT_Restricting_Period_Staff_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Restricting_Period_Staff_ClassSectionDMO>(ttclass);
                                    tt_dis_det.TTRPS_Id = objpge.TTRPS_Id;
                                    tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                    tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                    tt_dis_det.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                    tt_dis_det.TTRPSCC_Days = Convert.ToInt32(_category.TempararyArrayList[i].NOD);
                                    tt_dis_det.TTRPSCC_ActiveFlag = true;
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
                        var result = _ttcontext.TT_Restricting_Period_StaffDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id));
                        if (result.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.TTRPS_AllotedFlag = "No";
                            objpge.TTRPS_ActiveFlag = true;
                            objpge.TTRPS_SUbSelcFlag = true;
                            _ttcontext.Add(objpge);
                            var contactExists = _ttcontext.SaveChanges();

                            var result123 = _ttcontext.TT_Restricting_Period_StaffDMO.Max(t => t.TTRPS_Id);
                            if (contactExists == 1)
                            {
                                for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                {
                                    TTRestrictionDTO ttclass = new TTRestrictionDTO();
                                    TT_Restricting_Period_Staff_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Restricting_Period_Staff_ClassSectionDMO>(ttclass);
                                    tt_dis_det.TTRPS_Id = result123;
                                    tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                    tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                    tt_dis_det.ISMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ISMS_Id);
                                    tt_dis_det.TTRPSCC_Days = Convert.ToInt32(_category.TempararyArrayList[i].NOD);
                                    tt_dis_det.TTRPSCC_ActiveFlag = true;
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
                                        _ttcontext.Remove(_ttcontext.TT_Restricting_Period_Staff_ClassSectionDMO.Where(t => t.TTRPS_Id.Equals(result123)));
                                        _ttcontext.Remove(_ttcontext.TT_Restricting_Period_StaffDMO.Where(t => t.TTRPS_Id.Equals(result123)));
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
                    _category.returnfixstatus = "Fixed";
                    return _category;
                }
                _category.all_restrict_period_staff_list = (from a in _ttcontext.AcademicYear
                                                    from e in _ttcontext.HR_Master_Employee_DMO
                                                    from g in _ttcontext.TT_Restricting_Period_StaffDMO
                                                         from b in _ttcontext.TT_Master_PeriodDMO
                                                    where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == _category.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                    select new TTRestrictionDTO
                                                    {
                                                        TTRPS_Id = g.TTRPS_Id,
                                                        ASMAY_Year = a.ASMAY_Year,
                                                        staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                        TTMP_Id = g.TTMP_Id,
                                                       TTMP_PeriodName=b.TTMP_PeriodName,
                                                        TTRPS_SUbSelcFlag = g.TTRPS_SUbSelcFlag,
                                                        TTRPS_ActiveFlag = g.TTRPS_ActiveFlag,
                                                    }
     ).Distinct().OrderBy(x => x.staffName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TTRestrictionDTO savedetail5(TTRestrictionDTO _category)
        {
            TT_Restricting_Period_SubjectDMO objpge = Mapper.Map<TT_Restricting_Period_SubjectDMO>(_category);
            try
            {
                var fix_count = _ttcontext.TT_Fixing_Period_SubjectDMO.AsNoTracking().Where(r => r.MI_Id == objpge.MI_Id && r.ASMAY_Id == objpge.ASMAY_Id && r.ISMS_Id == objpge.ISMS_Id && r.TTMP_Id == objpge.TTMP_Id && r.TTFPSU_ActiveFlag==true).Count();
                if (fix_count == 0)
                {
                    if (_category.TTRPSU_SUbSelcFlag == false)
                {
                    if (objpge.TTRPSU_Id > 0)
                    {

                        var result0 = _ttcontext.TT_Restricting_Period_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id) && t.TTRPSU_Id != objpge.TTRPSU_Id);
                        if (result0.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result = _ttcontext.TT_Restricting_Period_SubjectDMO.Single(t => t.TTRPSU_Id.Equals(objpge.TTRPSU_Id) && t.MI_Id.Equals(objpge.MI_Id));
                            result.ASMAY_Id = objpge.ASMAY_Id;
                            result.ISMS_Id = objpge.ISMS_Id;
                            result.TTMP_Id = objpge.TTMP_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.TTRPSU_AllotedFlag = "No";
                            result.TTRPSU_ActiveFlag = true;
                            result.TTRPSU_SUbSelcFlag = false;
                            _ttcontext.Update(result);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                List<TT_Restricting_Period_Subject_ClassSectionDMO> deactivelist = new List<TT_Restricting_Period_Subject_ClassSectionDMO>();
                                deactivelist = _ttcontext.TT_Restricting_Period_Subject_ClassSectionDMO.AsNoTracking().Where(s => s.TTRPSU_Id == objpge.TTRPSU_Id).ToList();
                                if (deactivelist.Count() > 0)
                                {
                                    List<TT_Restricting_Period_Subject_ClassSectionDMO> lorg = new List<TT_Restricting_Period_Subject_ClassSectionDMO>();
                                    lorg = _ttcontext.TT_Restricting_Period_Subject_ClassSectionDMO.Where(t => t.TTRPSU_Id.Equals(objpge.TTRPSU_Id)).ToList();
                                    if (lorg.Any())
                                    {
                                        for (int i = 0; lorg.Count > i; i++)
                                        {
                                            lorg.ElementAt(i).TTRPSUCC_ActiveFlag = false;
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
                        var result = _ttcontext.TT_Restricting_Period_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id));
                        if (result.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.TTRPSU_AllotedFlag = "No";
                            objpge.TTRPSU_ActiveFlag = true;
                            objpge.TTRPSU_SUbSelcFlag = false;
                            _ttcontext.Add(objpge);
                            var contactExists = _ttcontext.SaveChanges();

                            var result123 = _ttcontext.TT_Restricting_Period_SubjectDMO.Max(t => t.TTRPSU_Id);
                            if (contactExists == 1)
                            {
                                List<TT_Restricting_Period_Subject_ClassSectionDMO> deactivelist = new List<TT_Restricting_Period_Subject_ClassSectionDMO>();
                                deactivelist = _ttcontext.TT_Restricting_Period_Subject_ClassSectionDMO.AsNoTracking().Where(s => s.TTRPSU_Id == result123).ToList();
                                if (deactivelist.Count() > 0)
                                {
                                    List<TT_Restricting_Period_Subject_ClassSectionDMO> lorg = new List<TT_Restricting_Period_Subject_ClassSectionDMO>();
                                    lorg = _ttcontext.TT_Restricting_Period_Subject_ClassSectionDMO.Where(t => t.TTRPSU_Id.Equals(result123)).ToList();
                                    if (lorg.Any())
                                    {
                                        for (int i = 0; lorg.Count > i; i++)
                                        {
                                            lorg.ElementAt(i).TTRPSUCC_ActiveFlag = false;
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
                else if (_category.TTRPSU_SUbSelcFlag == true)
                {
                    if (objpge.TTRPSU_Id > 0)
                    {

                        var result0 = _ttcontext.TT_Restricting_Period_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id) && t.TTRPSU_Id != objpge.TTRPSU_Id);
                        if (result0.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                            { 
                            var result = _ttcontext.TT_Restricting_Period_SubjectDMO.Single(t => t.TTRPSU_Id.Equals(objpge.TTRPSU_Id) && t.MI_Id.Equals(objpge.MI_Id));
                            result.ASMAY_Id = objpge.ASMAY_Id;
                            result.ISMS_Id = objpge.ISMS_Id;
                            result.TTMP_Id = objpge.TTMP_Id;
                            result.UpdatedDate = DateTime.Now;
                            result.TTRPSU_AllotedFlag = "No";
                            result.TTRPSU_ActiveFlag = true;
                            result.TTRPSU_SUbSelcFlag = true;
                            _ttcontext.Update(result);
                            var contactExists = _ttcontext.SaveChanges();
                            if (contactExists == 1)
                            {
                                deletepagesRightgrid5(objpge.TTRPSU_Id);
                                for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                {
                                    TTRestrictionDTO ttclass = new TTRestrictionDTO();
                                    TT_Restricting_Period_Subject_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Restricting_Period_Subject_ClassSectionDMO>(ttclass);
                                    tt_dis_det.TTRPSU_Id = objpge.TTRPSU_Id;
                                    tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                    tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                    tt_dis_det.HRME_Id = Convert.ToInt64(_category.TempararyArrayList[i].HRME_Id);
                                    tt_dis_det.TTRPSUCC_Days = Convert.ToInt32(_category.TempararyArrayList[i].NOD);
                                    tt_dis_det.TTRPSUCC_ActiveFlag = true;
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
                        var result = _ttcontext.TT_Restricting_Period_SubjectDMO.Where(t => t.ISMS_Id.Equals(objpge.ISMS_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTMP_Id.Equals(objpge.TTMP_Id));
                        if (result.Count() > 0)
                        {
                            _category.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            objpge.CreatedDate = DateTime.Now;
                            objpge.UpdatedDate = DateTime.Now;
                            objpge.TTRPSU_AllotedFlag = "No";
                            objpge.TTRPSU_ActiveFlag = true;
                            objpge.TTRPSU_SUbSelcFlag = true;
                            _ttcontext.Add(objpge);
                            var contactExists = _ttcontext.SaveChanges();

                            var result123 = _ttcontext.TT_Restricting_Period_SubjectDMO.Max(t => t.TTRPSU_Id);
                            if (contactExists == 1)
                            {
                                for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                                {
                                    TTRestrictionDTO ttclass = new TTRestrictionDTO();
                                    TT_Restricting_Period_Subject_ClassSectionDMO tt_dis_det = Mapper.Map<TT_Restricting_Period_Subject_ClassSectionDMO>(ttclass);
                                    tt_dis_det.TTRPSU_Id = result123;
                                    tt_dis_det.ASMCL_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMCL_Id);
                                    tt_dis_det.ASMS_Id = Convert.ToInt64(_category.TempararyArrayList[i].ASMS_Id);
                                    tt_dis_det.HRME_Id = Convert.ToInt64(_category.TempararyArrayList[i].HRME_Id);
                                    tt_dis_det.TTRPSUCC_Days = Convert.ToInt32(_category.TempararyArrayList[i].NOD);
                                    tt_dis_det.TTRPSUCC_ActiveFlag = true;
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
                                        _ttcontext.Remove(_ttcontext.TT_Restricting_Period_Subject_ClassSectionDMO.Where(t => t.TTRPSU_Id.Equals(result123)));
                                        _ttcontext.Remove(_ttcontext.TT_Restricting_Period_SubjectDMO.Where(t => t.TTRPSU_Id.Equals(result123)));
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
                    _category.returnfixstatus = "Fixed";
                    return _category;
                }
                _category.all_restrict_period_subject_list = (from a in _ttcontext.AcademicYear
                                                       from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                                       from g in _ttcontext.TT_Restricting_Period_SubjectDMO
                                                       from b in _ttcontext.TT_Master_PeriodDMO
                                                       where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == _category.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                       select new TTRestrictionDTO
                                                       {
                                                           TTRPSU_Id = g.TTRPSU_Id,
                                                           ASMAY_Year = a.ASMAY_Year,
                                                           ISMS_SubjectName=e.ISMS_SubjectName,
                                                           TTMP_Id = g.TTMP_Id,
                                                           TTMP_PeriodName = b.TTMP_PeriodName,
                                                           TTRPSU_SUbSelcFlag = g.TTRPSU_SUbSelcFlag,
                                                           TTRPSU_ActiveFlag = g.TTRPSU_ActiveFlag,
                                                       }
     ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }

        public TTRestrictionDTO savedetail(TTRestrictionDTO _category)
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
                                TTRestrictionDTO ttclass = new TTRestrictionDTO();
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
                                TTRestrictionDTO ttclass = new TTRestrictionDTO();
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
         public TTRestrictionDTO deletepagesRightgrid2(long id)
        {
            TTRestrictionDTO pagert = new TTRestrictionDTO();
            try
            {
                
                List<TT_Restricting_Day_Staff_ClassSectionDMO> lorg = new List<TT_Restricting_Day_Staff_ClassSectionDMO>();
                lorg = _ttcontext.TT_Restricting_Day_Staff_ClassSectionDMO.Where(t => t.TTRDS_Id.Equals(id)).ToList();
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

        public TTRestrictionDTO deletepagesRightgrid3(long id)
        {
            TTRestrictionDTO pagert = new TTRestrictionDTO();
            try
            {

                List<TT_Restricting_Day_Subject_ClassSectionDMO> lorg = new List<TT_Restricting_Day_Subject_ClassSectionDMO>();
                lorg = _ttcontext.TT_Restricting_Day_Subject_ClassSectionDMO.Where(t => t.TTRDSU_Id.Equals(id)).ToList();
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

        public TTRestrictionDTO deletepagesRightgrid4(long id)
        {
            TTRestrictionDTO pagert = new TTRestrictionDTO();
            try
            {

                List<TT_Restricting_Period_Staff_ClassSectionDMO> lorg = new List<TT_Restricting_Period_Staff_ClassSectionDMO>();
                lorg = _ttcontext.TT_Restricting_Period_Staff_ClassSectionDMO.Where(t => t.TTRPS_Id.Equals(id)).ToList();
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

        public TTRestrictionDTO deletepagesRightgrid5(long id)
        {
            TTRestrictionDTO pagert = new TTRestrictionDTO();
            try
            {

                List<TT_Restricting_Period_Subject_ClassSectionDMO> lorg = new List<TT_Restricting_Period_Subject_ClassSectionDMO>();
                lorg = _ttcontext.TT_Restricting_Period_Subject_ClassSectionDMO.Where(t => t.TTRPSU_Id.Equals(id)).ToList();
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

        public TTRestrictionDTO getalldetailsviewrecords(int id)
        {
            TTRestrictionDTO TTMC = new TTRestrictionDTO();
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
                                          select new TTRestrictionDTO
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

        public TTRestrictionDTO getdetails(TTRestrictionDTO data)
       {
            try
            {
                List<AcademicYear> year = new List<AcademicYear>();
                year = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(c=>c.ASMAY_Order).ToList();
                data.acayear = year.Distinct().ToArray();

                List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                mcat = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList();
                data.categorylist = mcat.Distinct().ToArray();

                data.classlist = (from a in _ttcontext.TTMasterCategoryDMO
                                  from b in _ttcontext.TT_Category_Class_DMO
                                  from c in _ttcontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && b.TTCC_ActiveFlag == true )
                                  select new TTRestrictionDTO
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
                                  where (e.MI_Id.Equals(data.MI_Id) && e.HRME_ActiveFlag.Equals(true) && staffAbbr.HRME_Id == e.HRME_Id && staffAbbr.MI_Id==data.MI_Id)
                                  select new TTRestrictionDTO
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
                data.restrict_day_period_list = (from a in _ttcontext.AcademicYear
                                            from b in _ttcontext.TTMasterCategoryDMO
                                            from c in _ttcontext.School_M_Class
                                            from d in _ttcontext.School_M_Section
                                            from e in _ttcontext.HR_Master_Employee_DMO                                          
                                            from f in _ttcontext.IVRM_School_Master_SubjectsDMO                                            
                                            from g in _ttcontext.TT_Master_PeriodDMO
                                            from h in _ttcontext.TT_Restricting_Day_PeriodDMO
                                            from i in _ttcontext.TT_Master_DayDMO
                                            where (a.MI_Id == h.MI_Id && a.ASMAY_Id == h.ASMAY_Id && b.MI_Id == h.MI_Id && b.TTMC_Id == h.TTMC_Id && c.MI_Id == h.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == h.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == h.MI_Id && e.HRME_Id == h.HRME_Id && f.MI_Id == h.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == h.MI_Id && g.TTMP_Id == h.TTMP_Id && i.MI_Id == h.MI_Id && i.TTMD_Id == h.TTMD_Id && h.MI_Id == data.MI_Id)
                                            select new TTRestrictionDTO
                                            {
                                                TTRDP_Id = h.TTRDP_Id,
                                                ASMAY_Year = a.ASMAY_Year,
                                                TTMC_CategoryName = b.TTMC_CategoryName,
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                ISMS_SubjectName = f.ISMS_SubjectName,
                                                TTMP_PeriodName = g.TTMP_PeriodName,
                                                TTMD_DayName = i.TTMD_DayName,
                                                TTRDP_ActiveFlag = h.TTRDP_ActiveFlag
                                            }
    ).Distinct().OrderBy(x => x.ASMCL_ClassName).ToArray();

                data.all_restrict_day_staff_list = (from a in _ttcontext.AcademicYear
                                                    from e in _ttcontext.HR_Master_Employee_DMO
                                                    from g in _ttcontext.TT_Restricting_Day_StaffDMO                                                     
                                                        from i in _ttcontext.TT_Master_DayDMO
                                                    where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && i.MI_Id==g.MI_Id && i.TTMD_Id==g.TTMD_Id)
                                                    select new TTRestrictionDTO
                                                    {
                                                        TTRDS_Id = g.TTRDS_Id,
                                                        ASMAY_Year = a.ASMAY_Year,
                                                        staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                        TTMD_Id =g.TTMD_Id,
                                                        TTMD_DayName=i.TTMD_DayName,
                                                        TTRDS_SUbSelcFlag = g.TTRDS_SUbSelcFlag,
                                                        TTRDS_ActiveFlag = g.TTRDS_ActiveFlag,
                                                    }
     ).Distinct().OrderBy(x => x.staffName).ToArray();

                data.all_restrict_day_subject_list = (from a in _ttcontext.AcademicYear
                                                      from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                                      from g in _ttcontext.TT_Restricting_Day_SubjectDMO
                                                      from i in _ttcontext.TT_Master_DayDMO
                                                      where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && i.MI_Id == g.MI_Id && i.TTMD_Id == g.TTMD_Id)
                                                      select new TTRestrictionDTO
                                                      {
                                                          TTRDSU_Id = g.TTRDSU_Id,
                                                          ASMAY_Year = a.ASMAY_Year,
                                                          ISMS_SubjectName = e.ISMS_SubjectName,
                                                          TTMD_Id = g.TTMD_Id,
                                                          TTMD_DayName = i.TTMD_DayName,
                                                          TTRDSU_SUbSelcFlag = g.TTRDSU_SUbSelcFlag,
                                                          TTRDSU_ActiveFlag = g.TTRDSU_ActiveFlag,
                                                      }
     ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();

                data.all_restrict_period_staff_list = (from a in _ttcontext.AcademicYear
                                                       from e in _ttcontext.HR_Master_Employee_DMO
                                                       from g in _ttcontext.TT_Restricting_Period_StaffDMO
                                                       from b in _ttcontext.TT_Master_PeriodDMO
                                                       where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                       select new TTRestrictionDTO
                                                       {
                                                           TTRPS_Id = g.TTRPS_Id,
                                                           ASMAY_Year = a.ASMAY_Year,
                                                           staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                           TTMP_Id = g.TTMP_Id,
                                                           TTMP_PeriodName = b.TTMP_PeriodName,
                                                           TTRPS_SUbSelcFlag = g.TTRPS_SUbSelcFlag,
                                                           TTRPS_ActiveFlag = g.TTRPS_ActiveFlag,
                                                       }
    ).Distinct().OrderBy(x => x.staffName).ToArray();

                data.all_restrict_period_subject_list = (from a in _ttcontext.AcademicYear
                                                         from e in _ttcontext.IVRM_School_Master_SubjectsDMO
                                                         from g in _ttcontext.TT_Restricting_Period_SubjectDMO
                                                         from b in _ttcontext.TT_Master_PeriodDMO
                                                         where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.ISMS_Id == g.ISMS_Id && g.MI_Id == data.MI_Id && b.MI_Id == g.MI_Id && b.TTMP_Id == g.TTMP_Id)
                                                         select new TTRestrictionDTO
                                                         {
                                                             TTRPSU_Id = g.TTRPSU_Id,
                                                             ASMAY_Year = a.ASMAY_Year,
                                                             ISMS_SubjectName = e.ISMS_SubjectName,
                                                             TTMP_Id = g.TTMP_Id,
                                                             TTMP_PeriodName = b.TTMP_PeriodName,
                                                             TTRPSU_SUbSelcFlag = g.TTRPSU_SUbSelcFlag,
                                                             TTRPSU_ActiveFlag = g.TTRPSU_ActiveFlag,
                                                         }
     ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public TTRestrictionDTO getpageedit1(int id)
        {
            TTRestrictionDTO page = new TTRestrictionDTO();
            try
            {
                List<TT_Restricting_Day_PeriodDMO> lorg = new List<TT_Restricting_Day_PeriodDMO>();
                lorg = _ttcontext.TT_Restricting_Day_PeriodDMO.AsNoTracking().Where(t => t.TTRDP_Id.Equals(id)).ToList();
                page.restrict_day_period_edit = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }


        public TTRestrictionDTO getpageedit2(int id)
        {
            TTRestrictionDTO page = new TTRestrictionDTO();
            try
            {
                List<TT_Restricting_Day_StaffDMO> lorg = new List<TT_Restricting_Day_StaffDMO>();
                lorg = _ttcontext.TT_Restricting_Day_StaffDMO.AsNoTracking().Where(t => t.TTRDS_Id.Equals(id)).ToList();
                page.restrict_day_staff_edit = lorg.ToArray();

                List<TT_Restricting_Day_Staff_ClassSectionDMO> lorgdetails = new List<TT_Restricting_Day_Staff_ClassSectionDMO>();
                lorgdetails = _ttcontext.TT_Restricting_Day_Staff_ClassSectionDMO.AsNoTracking().Where(t => t.TTRDS_Id.Equals(id) && t.TTRDSCC_ActiveFlag==true).ToList();
                page.restrict_day_staff__classecedit = lorgdetails.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public TTRestrictionDTO getpageedit3(int id)
        {
            TTRestrictionDTO page = new TTRestrictionDTO();
            try
            {
                List<TT_Restricting_Day_SubjectDMO> lorg = new List<TT_Restricting_Day_SubjectDMO>();
                lorg = _ttcontext.TT_Restricting_Day_SubjectDMO.AsNoTracking().Where(t => t.TTRDSU_Id.Equals(id)).ToList();
                page.restrict_day_subject_edit = lorg.ToArray();

                List<TT_Restricting_Day_Subject_ClassSectionDMO> lorgdetails = new List<TT_Restricting_Day_Subject_ClassSectionDMO>();
                lorgdetails = _ttcontext.TT_Restricting_Day_Subject_ClassSectionDMO.AsNoTracking().Where(t => t.TTRDSU_Id.Equals(id) && t.TTRDSUCC_ActiveFlag == true).ToList();
                page.restrict_day_subject__classecedit = lorgdetails.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public TTRestrictionDTO getpageedit4(int id)
        {
            TTRestrictionDTO page = new TTRestrictionDTO();
            try
            {
                List<TT_Restricting_Period_StaffDMO> lorg = new List<TT_Restricting_Period_StaffDMO>();
                lorg = _ttcontext.TT_Restricting_Period_StaffDMO.AsNoTracking().Where(t => t.TTRPS_Id.Equals(id)).ToList();
                page.restrict_period_staff_edit = lorg.ToArray();

                List<TT_Restricting_Period_Staff_ClassSectionDMO> lorgdetails = new List<TT_Restricting_Period_Staff_ClassSectionDMO>();
                lorgdetails = _ttcontext.TT_Restricting_Period_Staff_ClassSectionDMO.AsNoTracking().Where(t => t.TTRPS_Id.Equals(id) && t.TTRPSCC_ActiveFlag == true).ToList();
                page.restrict_period_staff__classecedit = lorgdetails.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public TTRestrictionDTO getpageedit5(int id)
        {
            TTRestrictionDTO page = new TTRestrictionDTO();
            try
            {
                List<TT_Restricting_Period_SubjectDMO> lorg = new List<TT_Restricting_Period_SubjectDMO>();
                lorg = _ttcontext.TT_Restricting_Period_SubjectDMO.AsNoTracking().Where(t => t.TTRPSU_Id.Equals(id)).ToList();
                page.restrict_period_subject_edit = lorg.ToArray();

                List<TT_Restricting_Period_Subject_ClassSectionDMO> lorgdetails = new List<TT_Restricting_Period_Subject_ClassSectionDMO>();
                lorgdetails = _ttcontext.TT_Restricting_Period_Subject_ClassSectionDMO.AsNoTracking().Where(t => t.TTRPSU_Id.Equals(id) && t.TTRPSUCC_ActiveFlag == true).ToList();
                page.restrict_period_subject__classecedit = lorgdetails.ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public TTRestrictionDTO getcategories(TTRestrictionDTO data)
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


        public TTRestrictionDTO getclasses(TTRestrictionDTO data)
        {

            try
            {
                data.classbycategory = (from a in _ttcontext.TTMasterCategoryDMO
                                        from c in _ttcontext.School_M_Class
                                        from d in _ttcontext.TT_Final_Period_DistributionDMO
                                        from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                        where (e.TTMC_Id==a.TTMC_Id && d.ASMAY_Id==data.ASMAY_Id && c.ASMCL_Id==e.ASMCL_Id && e.TTFPD_Id==d.TTFPD_Id && a.MI_Id == c.MI_Id && d.MI_Id==a.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == data.TTMC_Id &&     d.TTFPD_ActiveFlag==true && e.TTFPDD_ActiveFlag==true)
                                        select new TTRestrictionDTO
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

        public TTRestrictionDTO getperiods(TTRestrictionDTO data)
        {

            try
            {

                data.sectionbyclass = (from a in _ttcontext.TTMasterCategoryDMO
                                        from b in _ttcontext.School_M_Section                                      
                                        from d in _ttcontext.TT_Final_Period_DistributionDMO
                                        from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                        where (e.TTMC_Id == a.TTMC_Id && d.ASMAY_Id == data.ASMAY_Id && b.ASMS_Id == e.ASMS_Id && e.TTFPD_Id == d.TTFPD_Id && a.MI_Id == b.MI_Id && d.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == data.TTMC_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true && e.ASMCL_Id==data.ASMCL_Id)
                                        select new TTRestrictionDTO
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
                                      select new TTRestrictionDTO
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


        public TTRestrictionDTO getstaff(TTRestrictionDTO data)
        {
            try
            {
                data.staffbyall = (from a in _ttcontext.TTMasterCategoryDMO                              
                                        from c in _ttcontext.HR_Master_Employee_DMO
                                        from d in _ttcontext.TT_Final_Period_DistributionDMO
                                        from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                        where (e.TTMC_Id == a.TTMC_Id && d.ASMAY_Id == data.ASMAY_Id && c.HRME_Id == d.HRME_Id  && e.ASMCL_Id==data.ASMCL_Id && e.TTMC_Id == data.TTMC_Id &&  e.ASMS_Id==data.ASMS_Id && e.TTFPD_Id == d.TTFPD_Id && a.MI_Id == c.MI_Id && d.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id &&  d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                        select new TTRestrictionDTO
                                        {
                                            HRME_Id = d.HRME_Id,
                                            staffName = c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == " " || c.HRME_EmployeeMiddleName == "0" ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == " " || c.HRME_EmployeeLastName == "0" ? " " : c.HRME_EmployeeLastName),
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

        public TTRestrictionDTO getsubjects(TTRestrictionDTO data)
        {
            try
            {
                data.subjectbystaff = (
                                   from c in _ttcontext.IVRM_School_Master_SubjectsDMO
                                   from d in _ttcontext.TT_Final_Period_DistributionDMO
                                   from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                   where (  d.ASMAY_Id == data.ASMAY_Id && c.ISMS_Id == e.ISMS_Id && d.HRME_Id==data.HRME_Id && e.ASMCL_Id == data.ASMCL_Id && e.TTMC_Id == data.TTMC_Id && e.ASMS_Id == data.ASMS_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == c.MI_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                   select new TTRestrictionDTO
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
        public TTRestrictionDTO get_cls_sec_subs(TTRestrictionDTO data)
        {
            try
            {
                data.clssbystaff = (from c in _ttcontext.School_M_Class
                                        from d in _ttcontext.TT_Final_Period_DistributionDMO
                                        from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                        where (d.HRME_Id==data.HRME_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id == d.MI_Id && c.ASMCL_Id == e.ASMCL_Id && e.TTFPD_Id == d.TTFPD_Id &&  d.MI_Id == data.MI_Id &&  d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                        select new TTRestrictionDTO
                                        {
                                            ASMCL_Id = e.ASMCL_Id,
                                            ASMCL_ClassName = c.ASMCL_ClassName,
                                           
                                        }
                                      ).Distinct().ToArray();
                data.secsbystaff = (from c in _ttcontext.School_M_Section
                                     from d in _ttcontext.TT_Final_Period_DistributionDMO
                                     from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                     where (d.HRME_Id == data.HRME_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id == d.MI_Id && c.ASMS_Id == e.ASMS_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                     select new TTRestrictionDTO
                                     {
                                         ASMS_Id=e.ASMS_Id,
                                         ASMC_SectionName=c.ASMC_SectionName,

                                     }
                                      ).Distinct().ToArray();
                data.subsbystaff = (from c in _ttcontext.IVRM_School_Master_SubjectsDMO
                                     from d in _ttcontext.TT_Final_Period_DistributionDMO
                                     from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                     where (d.HRME_Id == data.HRME_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id == d.MI_Id && c.ISMS_Id == e.ISMS_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                     select new TTRestrictionDTO
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

        public TTRestrictionDTO get_cls_sec_staffs(TTRestrictionDTO data)
        {
            try
            {
                data.clssbysub = (from c in _ttcontext.School_M_Class
                                    from d in _ttcontext.TT_Final_Period_DistributionDMO
                                    from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                    where (e.ISMS_Id == data.ISMS_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id==d.MI_Id && c.ASMCL_Id == e.ASMCL_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                    select new TTRestrictionDTO
                                    {
                                        ASMCL_Id = e.ASMCL_Id,
                                        ASMCL_ClassName = c.ASMCL_ClassName,

                                    }
                                      ).Distinct().ToArray();
                data.secsbysub = (from c in _ttcontext.School_M_Section
                                    from d in _ttcontext.TT_Final_Period_DistributionDMO
                                    from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                    where (e.ISMS_Id == data.ISMS_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id==d.MI_Id && c.ASMS_Id == e.ASMS_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                    select new TTRestrictionDTO
                                    {
                                        ASMS_Id = e.ASMS_Id,
                                        ASMC_SectionName = c.ASMC_SectionName,

                                    }
                                      ).Distinct().ToArray();
                data.staffbysub = (from c in _ttcontext.HR_Master_Employee_DMO
                                    from d in _ttcontext.TT_Final_Period_DistributionDMO
                                    from e in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                    where (e.ISMS_Id == data.ISMS_Id && d.ASMAY_Id == data.ASMAY_Id && c.MI_Id==d.MI_Id && c.HRME_Id == d.HRME_Id && e.TTFPD_Id == d.TTFPD_Id && d.MI_Id == data.MI_Id && d.TTFPD_ActiveFlag == true && e.TTFPDD_ActiveFlag == true)
                                    select new TTRestrictionDTO
                                    {
                                        HRME_Id = d.HRME_Id,
                                        staffName =c.HRME_EmployeeFirstName + " " + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == " " || c.HRME_EmployeeMiddleName == "0" ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == " " || c.HRME_EmployeeLastName == "0" ? " " : c.HRME_EmployeeLastName),

                                    }
                                      ).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return data;

        }

        public TTRestrictionDTO getalldetailsviewrecords2(int id)
        {
            TTRestrictionDTO TTMC = new TTRestrictionDTO();
            try
            {
                TTMC.detailspopuparray2 = (from a in _ttcontext.AcademicYear
                                          from c in _ttcontext.School_M_Class
                                          from d in _ttcontext.School_M_Section
                                          from e in _ttcontext.HR_Master_Employee_DMO
                                          from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                          from g in _ttcontext.TT_Restricting_Day_StaffDMO
                                          from h in _ttcontext.TT_Restricting_Day_Staff_ClassSectionDMO
                                              from i in _ttcontext.TT_Master_DayDMO
                                          where (g.TTRDS_Id == h.TTRDS_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id &&  c.MI_Id == g.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == g.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && f.MI_Id == g.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == g.MI_Id && h.TTRDS_Id == id  && i.MI_Id==g.MI_Id && g.TTMD_Id==i.TTMD_Id)
                                          select new TTRestrictionDTO
                                          {
                                              TTRDSCC_Id = h.TTRDSCC_Id,
                                              ASMAY_Year = a.ASMAY_Year,                                            
                                              ASMCL_ClassName = c.ASMCL_ClassName,
                                              ASMC_SectionName = d.ASMC_SectionName,
                                              staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                              TTMD_DayName =i.TTMD_DayName,
                                              ISMS_SubjectName = f.ISMS_SubjectName,
                                              TTRDSCC_Periods = h.TTRDSCC_Periods,
                                              TTRDSCC_ActiveFlag = h.TTRDSCC_ActiveFlag,
                                          }
                ).Distinct().OrderBy(x => x.staffName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TTRestrictionDTO getalldetailsviewrecords3(int id)
        {
            TTRestrictionDTO TTMC = new TTRestrictionDTO();
            try
            {
                TTMC.detailspopuparray3 = (from a in _ttcontext.AcademicYear
                                           from c in _ttcontext.School_M_Class
                                           from d in _ttcontext.School_M_Section
                                           from e in _ttcontext.HR_Master_Employee_DMO
                                           from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                           from g in _ttcontext.TT_Restricting_Day_SubjectDMO
                                           from h in _ttcontext.TT_Restricting_Day_Subject_ClassSectionDMO
                                           from i in _ttcontext.TT_Master_DayDMO
                                           where (g.TTRDSU_Id == h.TTRDSU_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && c.MI_Id == g.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == g.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == g.MI_Id && e.HRME_Id == h.HRME_Id && f.MI_Id == g.MI_Id && f.ISMS_Id == g.ISMS_Id && g.MI_Id == g.MI_Id && h.TTRDSU_Id == id && i.MI_Id == g.MI_Id && g.TTMD_Id == i.TTMD_Id)
                                           select new TTRestrictionDTO
                                           {
                                               TTRDSUCC_Id = h.TTRDSUCC_Id,
                                               ASMAY_Year = a.ASMAY_Year,
                                               ASMCL_ClassName = c.ASMCL_ClassName,
                                               ASMC_SectionName = d.ASMC_SectionName,
                                               staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                               TTMD_DayName = i.TTMD_DayName,
                                               ISMS_SubjectName = f.ISMS_SubjectName,
                                               TTRDSUCC_Periods = h.TTRDSUCC_Periods,
                                               TTRDSUCC_ActiveFlag = h.TTRDSUCC_ActiveFlag,
                                           }
                ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TTRestrictionDTO getalldetailsviewrecords4(int id)
        {
            TTRestrictionDTO TTMC = new TTRestrictionDTO();
            try
            {
                TTMC.detailspopuparray4 = (from a in _ttcontext.AcademicYear                                         
                                           from c in _ttcontext.School_M_Class
                                           from d in _ttcontext.School_M_Section
                                           from e in _ttcontext.HR_Master_Employee_DMO
                                           from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                           from g in _ttcontext.TT_Restricting_Period_StaffDMO
                                           from h in _ttcontext.TT_Restricting_Period_Staff_ClassSectionDMO
                                               from b in _ttcontext.TT_Master_PeriodDMO
                                           where (g.TTRPS_Id == h.TTRPS_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && c.MI_Id == g.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == g.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && f.MI_Id == g.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == g.MI_Id && h.TTRPS_Id == id && b.MI_Id == g.MI_Id && g.TTMP_Id == b.TTMP_Id)
                                           select new TTRestrictionDTO
                                           {
                                               TTRPSCC_Id = h.TTRPSCC_Id,
                                               ASMAY_Year = a.ASMAY_Year,
                                               ASMCL_ClassName = c.ASMCL_ClassName,
                                               ASMC_SectionName = d.ASMC_SectionName,
                                               staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                               TTMP_PeriodName =b.TTMP_PeriodName,
                                               ISMS_SubjectName = f.ISMS_SubjectName,
                                               TTRPSCC_Days = h.TTRPSCC_Days,
                                               TTRPSCC_ActiveFlag = h.TTRPSCC_ActiveFlag,
                                           }
                ).Distinct().OrderBy(x => x.staffName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TTRestrictionDTO getalldetailsviewrecords5(int id)
        {
            TTRestrictionDTO TTMC = new TTRestrictionDTO();
            try
            {
                TTMC.detailspopuparray5 = (from a in _ttcontext.AcademicYear
                                           from c in _ttcontext.School_M_Class
                                           from d in _ttcontext.School_M_Section
                                           from e in _ttcontext.HR_Master_Employee_DMO
                                           from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                           from g in _ttcontext.TT_Restricting_Period_SubjectDMO
                                           from h in _ttcontext.TT_Restricting_Period_Subject_ClassSectionDMO
                                           from b in _ttcontext.TT_Master_PeriodDMO
                                           where (g.TTRPSU_Id == h.TTRPSU_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && c.MI_Id == g.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == g.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == g.MI_Id && e.HRME_Id == h.HRME_Id && f.MI_Id == g.MI_Id && f.ISMS_Id == g.ISMS_Id && g.MI_Id == g.MI_Id && h.TTRPSU_Id == id && b.MI_Id == g.MI_Id && g.TTMP_Id == b.TTMP_Id)
                                           select new TTRestrictionDTO
                                           {
                                               TTRPSUCC_Id = h.TTRPSUCC_Id,
                                               ASMAY_Year = a.ASMAY_Year,
                                               ASMCL_ClassName = c.ASMCL_ClassName,
                                               ASMC_SectionName = d.ASMC_SectionName,
                                               staffName = e.HRME_EmployeeFirstName + " " + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                               TTMP_PeriodName = b.TTMP_PeriodName,
                                               ISMS_SubjectName = f.ISMS_SubjectName,
                                               TTRPSUCC_Days = h.TTRPSUCC_Days,
                                               TTRPSUCC_ActiveFlag = h.TTRPSUCC_ActiveFlag,
                                           }
                ).Distinct().OrderBy(x => x.ISMS_SubjectName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }



        public TTRestrictionDTO deactivate1(TTRestrictionDTO data)
        {
            TT_Restricting_Day_PeriodDMO pge = Mapper.Map<TT_Restricting_Day_PeriodDMO>(data);
            if (pge.TTRDP_Id > 0)
            {
                var result = _ttcontext.TT_Restricting_Day_PeriodDMO.AsNoTracking().Single(t => t.TTRDP_Id == pge.TTRDP_Id);
                if (result.TTRDP_ActiveFlag == true)
                {
                    result.TTRDP_ActiveFlag = false;
                }
                else
                {
                    result.TTRDP_ActiveFlag = true;
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


        public TTRestrictionDTO deactivate2(TTRestrictionDTO data)
        {
            TT_Restricting_Day_StaffDMO pge = Mapper.Map<TT_Restricting_Day_StaffDMO>(data);
            if (pge.TTRDS_Id > 0)
            {
                var result = _ttcontext.TT_Restricting_Day_StaffDMO.AsNoTracking().Single(t => t.TTRDS_Id == pge.TTRDS_Id);
                if (result.TTRDS_ActiveFlag == true)
                {
                    result.TTRDS_ActiveFlag = false;
                }
                else
                {
                    result.TTRDS_ActiveFlag = true;
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
        public TTRestrictionDTO deactivate3(TTRestrictionDTO data)
        {
            TT_Restricting_Day_SubjectDMO pge = Mapper.Map<TT_Restricting_Day_SubjectDMO>(data);
            if (pge.TTRDSU_Id > 0)
            {
                var result = _ttcontext.TT_Restricting_Day_SubjectDMO.AsNoTracking().Single(t => t.TTRDSU_Id == pge.TTRDSU_Id);
                if (result.TTRDSU_ActiveFlag == true)
                {
                    result.TTRDSU_ActiveFlag = false;
                }
                else
                {
                    result.TTRDSU_ActiveFlag = true;
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

        public TTRestrictionDTO deactivate4(TTRestrictionDTO data)
        {
            TT_Restricting_Period_StaffDMO pge = Mapper.Map<TT_Restricting_Period_StaffDMO>(data);
            if (pge.TTRPS_Id > 0)
            {
                var result = _ttcontext.TT_Restricting_Period_StaffDMO.AsNoTracking().Single(t => t.TTRPS_Id == pge.TTRPS_Id);
                if (result.TTRPS_ActiveFlag == true)
                {
                    result.TTRPS_ActiveFlag = false;
                }
                else
                {
                    result.TTRPS_ActiveFlag = true;
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

        public TTRestrictionDTO deactivate5(TTRestrictionDTO data)
        {
            TT_Restricting_Period_SubjectDMO pge = Mapper.Map<TT_Restricting_Period_SubjectDMO>(data);
            if (pge.TTRPSU_Id > 0)
            {
                var result = _ttcontext.TT_Restricting_Period_SubjectDMO.AsNoTracking().Single(t => t.TTRPSU_Id == pge.TTRPSU_Id);
                if (result.TTRPSU_ActiveFlag == true)
                {
                    result.TTRPSU_ActiveFlag = false;
                }
                else
                {
                    result.TTRPSU_ActiveFlag = true;
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

        public TTRestrictionDTO deactivate(TTRestrictionDTO data)
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
        public TTRestrictionDTO getpageedit(int id)
        {
            TTRestrictionDTO page = new TTRestrictionDTO();
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
        public TTRestrictionDTO deleterec(int id)
        {
    
            TTRestrictionDTO period = new TTRestrictionDTO();
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
