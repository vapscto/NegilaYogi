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
    public class TTClassMasterImpl : Interfaces.ClassMasterInterface
    {
        private static ConcurrentDictionary<string, TTMasterCategoryDTO> _login =
         new ConcurrentDictionary<string, TTMasterCategoryDTO>();


        public TTContext _ttcontext;
        readonly ILogger<TTClassMasterImpl> _logger;
        public TTClassMasterImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public TTClassMasterDTO savedetail(TTClassMasterDTO _category)
        {
            TT_Final_Period_DistributionDMO objpge = Mapper.Map<TT_Final_Period_DistributionDMO>(_category);
            try
            {
                if (objpge.TTFPD_Id > 0)
                {
                    var result0 = _ttcontext.TT_Final_Period_DistributionDMO.Where(t => t.HRME_Id.Equals(objpge.HRME_Id) && t.MI_Id.Equals(objpge.MI_Id) && t.ASMAY_Id.Equals(objpge.ASMAY_Id) && t.TTFPD_Id != objpge.TTFPD_Id);
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
                        result.TTFPD_StaffConsecutive = objpge.TTFPD_StaffConsecutive; 
                        result.UpdatedDate = DateTime.Now;
                        _ttcontext.Update(result);
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            deletepagesRightgrid(objpge.TTFPD_Id);
                            for (int i = 0; i < _category.TempararyArrayList.Length; i++)
                            {
                                TTClassMasterDTO ttclass = new TTClassMasterDTO();
                                TT_Final_Period_Distribution_DetailedDMO tt_dis_det = Mapper.Map<TT_Final_Period_Distribution_DetailedDMO>(ttclass);

                                tt_dis_det.TTFPD_Id = objpge.TTFPD_Id;
                                tt_dis_det.TTMC_Id = Convert.ToInt64(_category.TempararyArrayList[i].TTMC_Id);
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
                                TTClassMasterDTO ttclass = new TTClassMasterDTO();
                                TT_Final_Period_Distribution_DetailedDMO tt_dis_det = Mapper.Map<TT_Final_Period_Distribution_DetailedDMO>(ttclass);
                                tt_dis_det.TTFPD_Id = result123;
                                tt_dis_det.TTMC_Id = Convert.ToInt64(_category.TempararyArrayList[i].TTMC_Id);
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
        public TTClassMasterDTO deletepagesRightgrid(long id)
        {
            TTClassMasterDTO pagert = new TTClassMasterDTO();

            try
            {

                List<TT_Final_Period_Distribution_DetailedDMO> lorg = new List<TT_Final_Period_Distribution_DetailedDMO>();
                lorg = _ttcontext.TT_Final_Period_Distribution_DetailedDMO.Where(t => t.TTFPD_Id.Equals(id)).ToList();
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

        public TTClassMasterDTO getalldetailsviewrecords(int id)
        {
            TTClassMasterDTO TTMC = new TTClassMasterDTO();
            try
            {
                TTMC.detailspopuparray = (from a in _ttcontext.AcademicYear
                                          from b in _ttcontext.TTMasterCategoryDMO
                                          from c in _ttcontext.School_M_Class
                                          from d in _ttcontext.School_M_Section
                                          from e in _ttcontext.HR_Master_Employee_DMO
                                          from f in _ttcontext.IVRM_School_Master_SubjectsDMO
                                          from g in _ttcontext.TT_Final_Period_DistributionDMO
                                          from h in _ttcontext.TT_Final_Period_Distribution_DetailedDMO
                                          where (g.TTFPD_Id == h.TTFPD_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && b.MI_Id == g.MI_Id && b.TTMC_Id == h.TTMC_Id && c.MI_Id == g.MI_Id && c.ASMCL_Id == h.ASMCL_Id && d.MI_Id == g.MI_Id && d.ASMS_Id == h.ASMS_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && f.MI_Id == g.MI_Id && f.ISMS_Id == h.ISMS_Id && g.MI_Id == g.MI_Id && h.TTFPD_Id == id)
                                          select new TTClassMasterDTO
                                          {
                                              TTFPDD_Id = h.TTFPDD_Id,
                                              ASMAY_Year = a.ASMAY_Year,
                                              TTMC_CategoryName = b.TTMC_CategoryName,
                                              ASMCL_ClassName = c.ASMCL_ClassName,
                                              ASMC_SectionName = d.ASMC_SectionName,
                                              staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
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

        public TTClassMasterDTO getdetails(TTClassMasterDTO data)
        {
            try
            {
                List<AcademicYear> year = new List<AcademicYear>();
                year = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.acayear = year.Distinct().OrderByDescending(t=>t.ASMAY_Order).ToArray();

                List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                mcat = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList();
                data.categorylist = mcat.Distinct().ToArray();

                data.classlist = (from a in _ttcontext.TTMasterCategoryDMO
                                  from b in _ttcontext.TT_Category_Class_DMO
                                  from c in _ttcontext.School_M_Class
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id) //&& b.TTCC_ActiveFlag == true 
                                  select new TTClassMasterDTO
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

                data.stafflist = (from a in _ttcontext.HR_Master_Employee_DMO
                                  from b in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                  where (a.MI_Id.Equals(data.MI_Id) && a.HRME_ActiveFlag.Equals(true) && b.HRME_Id == a.HRME_Id && b.TTMSAB_ActiveFlag==true)
                                  select new TTClassMasterDTO
                                  {
                                      HRME_Id = a.HRME_Id,
                                      FirstName = a.HRME_EmployeeFirstName,
                                      MiddleName = a.HRME_EmployeeMiddleName,
                                      LastName = a.HRME_EmployeeLastName,
                                 staffName = a.HRME_EmployeeFirstName + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == " " || a.HRME_EmployeeMiddleName == "0" ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == " " || a.HRME_EmployeeLastName == "0" ? " " : a.HRME_EmployeeLastName),
                                      
                                  }
                                 ).Distinct().OrderBy(r=>r.staffName).ToArray();


                List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                subjects = _ttcontext.IVRM_School_Master_SubjectsDMO.AsNoTracking().Where(s => s.MI_Id == data.MI_Id && s.ISMS_ActiveFlag == 1).ToList();
                data.subjectlist = subjects.Distinct().ToArray();



                List<TT_Master_PeriodDMO> periods = new List<TT_Master_PeriodDMO>();
                periods = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(p => p.MI_Id == data.MI_Id && p.TTMP_ActiveFlag == true).ToList();
                data.periodlist = periods.Distinct().ToArray();


                data.all_period_distri_list = (from a in _ttcontext.AcademicYear
                                               from e in _ttcontext.HR_Master_Employee_DMO
                                               from g in _ttcontext.TT_Final_Period_DistributionDMO
                                               where (a.MI_Id == g.MI_Id && a.ASMAY_Id == g.ASMAY_Id && e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == data.MI_Id)
                                               select new TTClassMasterDTO
                                               {
                                                   TTFPD_Id = g.TTFPD_Id,
                                                   ASMAY_Year = a.ASMAY_Year,
                                                   staffName = e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName),
                                                   TTFPD_TotWeekPeriods = g.TTFPD_TotWeekPeriods,
                                                   TTFPD_ActiveFlag = g.TTFPD_ActiveFlag,

                                               }
     ).Distinct().OrderBy(x => x.staffName).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public TTClassMasterDTO getcategories(TTClassMasterDTO data)
        {

            try
            {
                List<TTMasterCategoryDMO> mcats = new List<TTMasterCategoryDMO>();
                mcats = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList();
                data.categorybyyear = mcats.Distinct().ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }


        public TTClassMasterDTO getclasses(TTClassMasterDTO data)
        {

            try
            {
                data.classbycategory = (from a in _ttcontext.TTMasterCategoryDMO
                                        from b in _ttcontext.TT_Category_Class_DMO
                                        from c in _ttcontext.School_M_Class
                                        where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag == true
                                        select new TTClassMasterDTO
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
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public TTClassMasterDTO deactivate(TTClassMasterDTO data)
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
                result.UpdatedDate = DateTime.Now;
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
        public TTClassMasterDTO getpageedit(int id)
        {
            TTClassMasterDTO page = new TTClassMasterDTO();
            try
            {
                //List<TT_Final_Period_DistributionDMO> lorg = new List<TT_Final_Period_DistributionDMO>();
              var  lorg = (from a in _ttcontext.TT_Final_Period_DistributionDMO
                        from b in _ttcontext.HR_Master_Employee_DMO
                        where a.TTFPD_Id == id && a.MI_Id == b.MI_Id && a.HRME_Id == b.HRME_Id
                        select new TTClassMasterDTO
                        {
                            TTFPD_Id = a.TTFPD_Id,
                            HRME_Id = a.HRME_Id,
                            ASMAY_Id = a.ASMAY_Id,
                            TTFPD_TotWeekPeriods = a.TTFPD_TotWeekPeriods,
                            TTFPD_StaffConsecutive = a.TTFPD_StaffConsecutive,
                            staffName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == " " || b.HRME_EmployeeMiddleName == "0" ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == " " || b.HRME_EmployeeLastName == "0" ? " " : b.HRME_EmployeeLastName),
                        }

                       ).Distinct().ToList();
                      
                page.period_distri_edit = lorg.ToArray();

                List<TT_Final_Period_Distribution_DetailedDMO> lorgdetails = new List<TT_Final_Period_Distribution_DetailedDMO>();
                lorgdetails = _ttcontext.TT_Final_Period_Distribution_DetailedDMO.AsNoTracking().Where(t => t.TTFPD_Id.Equals(id)).ToList();
                page.period_distri_detail_edit = lorgdetails.ToArray();

                var count = _ttcontext.TT_Final_Period_Distribution_DetailedDMO.Where(x => x.TTFPD_Id.Equals(id) && x.TTFPDD_ActiveFlag == true).Sum(t => t.TTFPD_TotalPeriods);
                page.edit_count = count;



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TTClassMasterDTO deleterec(int id)
        {
            TTClassMasterDTO period = new TTClassMasterDTO();
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
