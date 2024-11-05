using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Services

{
    public class ClgPeriodAllocationImpl:Interfaces.ClgPeriodAllocationInterface
    {





        private static ConcurrentDictionary<string, ClgPeriodAllocation_DTO> _login =
        new ConcurrentDictionary<string, ClgPeriodAllocation_DTO>();

        public TTContext _ttcontext;
        readonly ILogger<ClgPeriodAllocationImpl> _logger;
        public ClgPeriodAllocationImpl(TTContext ttcntx, ILogger<ClgPeriodAllocationImpl> acdimpl)
        {
            _ttcontext = ttcntx;
            _logger = acdimpl;
        }


        public ClgPeriodAllocation_DTO save_period(ClgPeriodAllocation_DTO _period)
        {
            try
            {
                var result_DATA = _ttcontext.TT_Master_PeriodDMO.Where(t => t.MI_Id.Equals(_period.MI_Id) && t.TTMP_ActiveFlag == true).ToList();
                for (var i = 0; i < _period.tempperiods.Count(); i++)
                {
                    var result1 = _ttcontext.TT_Master_PeriodDMO.Where(t => t.TTMP_PeriodName.Equals(_period.tempperiods[i].TTMP_PeriodName) && t.MI_Id.Equals(_period.MI_Id));
                    if (result1.Count() > 0)
                    {
                        var result = _ttcontext.TT_Master_PeriodDMO.Single(t => t.TTMP_PeriodName.Equals(_period.tempperiods[i].TTMP_PeriodName) && t.MI_Id.Equals(_period.MI_Id));
                        result.TTMP_ActiveFlag = true;
                        _ttcontext.Update(result);
                        var flag = _ttcontext.SaveChanges();
                        if (flag.Equals(1))
                        {
                            _period.returnval = true;
                        }
                        else
                        {
                            _period.returnval = false;
                        }
                    }
                    else
                    {
                        TT_Master_PeriodDMO objpge = new TT_Master_PeriodDMO();
                        objpge.TTMP_PeriodName = _period.tempperiods[i].TTMP_PeriodName;
                        objpge.MI_Id = _period.MI_Id;
                        objpge.TTMP_ActiveFlag = true;
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        _ttcontext.Add(objpge);
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            _period.returnval = true;
                        }
                        else
                        {
                            _period.returnval = false;
                        }
                    }
                }

                if (result_DATA.Count() > _period.tempperiods.Count())
                {
                    for (var k = _period.tempperiods.Count(); k < result_DATA.Count(); k++)
                    {
                        var result1 = _ttcontext.TT_Master_PeriodDMO.Where(t => t.TTMP_PeriodName.Equals(result_DATA[k].TTMP_PeriodName) && t.MI_Id.Equals(_period.MI_Id) && t.TTMP_ActiveFlag == true).Distinct().Select(f => f.TTMP_Id).ToList();
                        if (result1.Count() > 0)
                        {
                            var result2 = _ttcontext.ClgPeriodAllocation_Course_DMO.Where(t => t.TTMP_Id.Equals(result1.FirstOrDefault())/* && t.MI_Id.Equals(_period.MI_Id)*/ && t.TTMPC_ActiveFlag == true).Select(g => g.TTMPC_Id).ToList();
                            if (result2.Count() > 0)
                            {
                                _period.cannot = true;
                                break;
                            }
                            else
                            {
                                var result = _ttcontext.TT_Master_PeriodDMO.Single(t => t.TTMP_PeriodName.Equals(result_DATA[k].TTMP_PeriodName) && t.MI_Id.Equals(_period.MI_Id));
                                result.TTMP_ActiveFlag = false;
                                _ttcontext.Update(result);
                                var flag = _ttcontext.SaveChanges();
                                if (flag.Equals(1))
                                {
                                    _period.returnval = true;
                                }
                                else
                                {
                                    _period.returnval = false;
                                }
                            }
                        }
                    }
                }


                _period.count = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == _period.MI_Id && t.TTMP_ActiveFlag == true).Count();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }

            return _period;
        }

        //public ClgPeriodAllocation_DTO getcategories(ClgPeriodAllocation_DTO data)
        //{
        //    try
        //    {
        //        List<TT_Master_PeriodDMO> periods = new List<TT_Master_PeriodDMO>();
        //        periods = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id).ToList();
        //        data.periodlist = periods.Distinct().ToArray();
        //        data.count = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id).Count();
        //        List<AcademicYear> year = new List<AcademicYear>();
        //        year = _ttcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(ee => ee.ASMAY_Order).ToList();
        //        data.acayear = year.Distinct().ToArray();

        //        List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
        //        mcat = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList();
        //        data.catelist = mcat.Distinct().ToArray();

        //        List<TT_Master_DayDMO> day = new List<TT_Master_DayDMO>();
        //        day = _ttcontext.TT_Master_DayDMO.Where(d => d.MI_Id == data.MI_Id && d.TTMD_ActiveFlag == true).ToList();
        //        data.day_list = day.Distinct().ToArray();

        //        data.TempararyArray = (from a in _ttcontext.TTMasterCategoryDMO
        //                               from b in _ttcontext.TT_Category_Class_DMO
        //                               from c in _ttcontext.School_M_Class
        //                               where (a.MI_Id == b.MI_Id && c.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && a.TTMC_Id == b.TTMC_Id && c.ASMCL_Id == b.ASMCL_Id && a.TTMC_ActiveFlag == true)
        //                               select new TTPeriodAllocationDTO
        //                               {
        //                                   ASMCL_Id = c.ASMCL_Id,
        //                                   ASMCL_ClassName = c.ASMCL_ClassName,
        //                                   TTMC_Id = a.TTMC_Id,
        //                                   TTMC_CategoryName = a.TTMC_CategoryName,
        //                               }
        //).Distinct().GroupBy(c => c.ASMCL_Id).Select(c => c.First()).ToArray();

        //        data.All_list = (from a in _ttcontext.AcademicYear
        //                         from b in _ttcontext.TT_Master_PeriodDMO
        //                         from c in _ttcontext.School_M_Class
        //                         from d in _ttcontext.ClgPeriodAllocation_Course_DMO
        //                         where (/*d.MI_Id == data.MI_Id &&*/ d.TTMP_Id == b.TTMP_Id && d.TTMPC_Id == c.ASMCL_Id && a.ASMAY_Id == d.ASMAY_Id)
        //                         select new TTPeriodAllocationDTO
        //                         {
        //                             TTMPC_Id = d.TTMPC_Id,
        //                             TTMP_PeriodName = b.TTMP_PeriodName,
        //                             ASMAY_Year = a.ASMAY_Year,
        //                             ASMCL_ClassName = c.ASMCL_ClassName,
        //                             ASMCL_Id = d.TTMPC_Id,
        //                             TTMPC_ActiveFlag = d.TTMPC_ActiveFlag
        //                         }
        //).Distinct().OrderBy(x => x.ASMCL_ClassName).ToArray();

        //    }
        //    catch (Exception ee)
        //    {
        //        _logger.LogError(ee.Message);
        //    }
        //    return data;

        //}

        public ClgPeriodAllocation_DTO getdetails(ClgPeriodAllocation_DTO data)
        {
            try
            {
                List<TT_Master_PeriodDMO> periods = new List<TT_Master_PeriodDMO>();
                periods = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id).ToList();
                data.periodlist = periods.Distinct().ToArray();
                data.count = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id).Count();
                List<AcademicYear> year = new List<AcademicYear>();
                year = _ttcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(ee => ee.ASMAY_Order).ToList();
                data.acayear = year.Distinct().ToArray();

                List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                mcat = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList();
                data.catelist = mcat.Distinct().ToArray();

                
                data.All_list = (from a in _ttcontext.MasterCourseDMO
                                 from b in _ttcontext.ClgMasterBranchDMO
                                 from c in _ttcontext.CLG_Adm_Master_SemesterDMO
                                 from d in _ttcontext.AcademicYear
                                 from e in _ttcontext.TT_Master_PeriodDMO
                                 from f in _ttcontext.ClgPeriodAllocation_Course_DMO
                                 where (a.AMCO_Id == f.AMCO_Id && b.AMB_Id == f.AMB_Id && c.AMSE_Id == f.AMSE_Id && e.TTMP_Id == f.TTMP_Id && d.ASMAY_Id == f.ASMAY_Id && e.MI_Id==data.MI_Id && a.MI_Id==e.MI_Id && a.MI_Id==c.MI_Id  )
                                 select new ClgPeriodAllocation_DTO
                                 {
                                     ASMAY_Year = d.ASMAY_Year,
                                     BRANCH_Name=b.AMB_BranchName,
                                     SEMISTER_Name=c.AMSE_SEMName,
                                     COURSE_Name=a.AMCO_CourseName,
                                     TTMP_PeriodName=e.TTMP_PeriodName,
                                     TTMPC_Id=f.TTMPC_Id,
                                     TTMPC_ActiveFlag=f.TTMPC_ActiveFlag
                                 }).Distinct().ToArray();
        //        data.All_list = (from a in _ttcontext.AcademicYear
        //                         from b in _ttcontext.TT_Master_PeriodDMO
        //                         from c in _ttcontext.MasterCourseDMO
        //                         from e in _ttcontext.ClgMasterBranchDMO
        //                         from f in _ttcontext.CLG_Adm_Master_SemesterDMO
        //                             //from c in _ttcontext.School_M_Class
        //                         from d in _ttcontext.ClgPeriodAllocation_Course_DMO
        //                         where (/*d.MI_Id == data.MI_Id && */d.TTMP_Id == b.TTMP_Id && /*d.ASMCL_Id == c.ASMCL_Id &&*/ a.ASMAY_Id == d.ASMAY_Id)
        //                         select new ClgPeriodAllocation_DTO
        //                         {
        //                             TTMPC_Id = d.TTMPC_Id,
        //                             TTMP_PeriodName = b.TTMP_PeriodName,
        //                             ASMAY_Year = a.ASMAY_Year,
        //                             TTMPC_ActiveFlag = d.TTMPC_ActiveFlag,
        //                             //ASMCL_ClassName = c.ASMCL_ClassName,
        //                             TTMP_ActiveFlag=d.TTMPC_ActiveFlag,

                                //                         }
                                //).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return data;

        }


        public ClgPeriodAllocation_DTO getcategories(ClgPeriodAllocation_DTO data)
        {
            try
            {
                List<TTMasterCategoryDMO> mcats = new List<TTMasterCategoryDMO>();
                mcats = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList();
                data.TempararyArray_categ = mcats.Distinct().ToArray();



            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return data;

        }
        public ClgPeriodAllocation_DTO getperiod_class(ClgPeriodAllocation_DTO data)
        {
            try
            {
                List<TT_Master_PeriodDMO> periods = new List<TT_Master_PeriodDMO>();
                periods = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(p => Convert.ToInt32(p.TTMP_PeriodName) <= data.period_count && p.TTMP_ActiveFlag == true && p.MI_Id == data.MI_Id).ToList();
                data.periodlist_class = periods.Distinct().ToArray();


                data.TempararyArray = (from a in _ttcontext.TTMasterCategoryDMO
                                       from b in _ttcontext.TT_Category_Class_DMO
                                       from c in _ttcontext.School_M_Class

                                       where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag==true
                                       select new ClgPeriodAllocation_DTO
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
                _logger.LogError(ee.Message);
            }
            return data;

        }

        public ClgPeriodAllocation_DTO savedetail(ClgPeriodAllocation_DTO _period)
        {
            try
            {

           


            var existing = _ttcontext.ClgPeriodAllocation_Course_DMO.Where(t => t.ASMAY_Id == _period.ASMAY_Id && t.AMB_Id == _period.AMB_Id && t.AMCO_Id == _period.AMCO_Id && t.AMSE_Id == _period.AMSE_Id).ToList();
            if(existing.Count>0)
            {
                foreach (var item in existing)
                {
                    _ttcontext.Remove(item);
                    _ttcontext.SaveChanges();
                }
            }
            foreach (var item1 in _period.temp_period_Array)
            {
                ClgPeriodAllocation_Course_DMO objpgee = new ClgPeriodAllocation_Course_DMO();
                objpgee.ASMAY_Id = _period.ASMAY_Id;              
                objpgee.AMCO_Id = _period.AMCO_Id;
                objpgee.AMB_Id = _period.AMB_Id;
                objpgee.AMSE_Id = _period.AMSE_Id;
                objpgee.CreatedDate = DateTime.Now;
                objpgee.UpdatedDate = DateTime.Now;
                    objpgee.TTMPC_ActiveFlag = true;
                objpgee.TTMP_Id = item1.TTMP_Id;
                    
                _ttcontext.Add(objpgee);
                var contactExists = _ttcontext.SaveChanges();
                if (contactExists>0)
                {
                    _period.returnval = true;
                }
                else
                {
                    _period.returnval = false;
                }
            }
            }
            catch (Exception d)
            {

                Console.WriteLine(d.Message);
            }

            return _period;
        }







        public ClgPeriodAllocation_DTO deactivate(ClgPeriodAllocation_DTO acd)
        {
            try
            {
                TT_Master_PeriodDMO pge = Mapper.Map<TT_Master_PeriodDMO>(acd);
                var result1 = _ttcontext.TT_Master_PeriodDMO.Single(t => t.TTMP_Id.Equals(pge.TTMP_Id));
                if (pge.TTMP_Id > 0)
                {
                    List<TT_Master_PeriodDMO> periods = new List<TT_Master_PeriodDMO>();
                    periods = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(p => p.MI_Id == acd.MI_Id).ToList();
                    if (result1.TTMP_ActiveFlag.Equals(false))
                    {
                        for (int i = 0; i <= periods.Count(); i++)
                        {
                            var result = _ttcontext.TT_Master_PeriodDMO.Single(t => t.TTMP_Id.Equals(periods[i].TTMP_Id));
                            if (periods[i].TTMP_Id <= pge.TTMP_Id)
                            {
                                result.TTMP_ActiveFlag = true;
                            }
                            if (periods[i].TTMP_Id > pge.TTMP_Id)
                            {
                                i = periods.Count() + 1;
                            }
                            _ttcontext.Update(result);
                            var flag = _ttcontext.SaveChanges();
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
                    else
                    {
                        for (int i = 0; i <= periods.Count(); i++)
                        {
                            var result_1 = _ttcontext.ClgPeriodAllocation_Course_DMO.Where(t => t.TTMP_Id.Equals(periods[i].TTMP_Id) && t.TTMPC_ActiveFlag == true).ToList();
                            if (result_1.Count > 0)
                            {
                                acd.returnduplicatestatus = "active";
                            }
                            else
                            {
                                var result = _ttcontext.TT_Master_PeriodDMO.Single(t => t.TTMP_Id.Equals(periods[i].TTMP_Id));
                                if (periods[i].TTMP_Id >= pge.TTMP_Id)
                                {
                                    result.TTMP_ActiveFlag = false;
                                }
                                _ttcontext.Update(result);
                                var flag = _ttcontext.SaveChanges();
                                if (flag.Equals(1))
                                {
                                    acd.returnval = true;
                                    acd.returnduplicatestatus = "deactive";
                                }
                                else
                                {
                                    acd.returnval = false;
                                }
                            }
                        }
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

        public ClgPeriodAllocation_DTO deactivate1(ClgPeriodAllocation_DTO acd)
        {
            try
            {
                ClgPeriodAllocation_Course_DMO pge = Mapper.Map<ClgPeriodAllocation_Course_DMO>(acd);
                if (pge.TTMPC_Id > 0)
                {

                    var result = _ttcontext.ClgPeriodAllocation_Course_DMO.Single(t => t.TTMPC_Id.Equals(pge.TTMPC_Id));
                    var result_1 = _ttcontext.TT_Master_PeriodDMO.Single(t => t.TTMP_Id.Equals(result.TTMP_Id));
                    if (result.TTMPC_ActiveFlag.Equals(true) && result_1.TTMP_ActiveFlag.Equals(true))
                    {
                        result.TTMPC_ActiveFlag = false;
                        _ttcontext.Update(result);
                        var flag = _ttcontext.SaveChanges();
                        if (flag.Equals(1))
                        {
                            acd.returnval = true;
                            acd.returnduplicatestatus = "active";
                        }
                        else
                        {
                            acd.returnval = false;
                        }

                    }
                    else if (result.TTMPC_ActiveFlag.Equals(false) && result_1.TTMP_ActiveFlag.Equals(false))
                    {
                        acd.returnduplicatestatus = "deactive";
                    }
                    else if (result.TTMPC_ActiveFlag.Equals(false) && result_1.TTMP_ActiveFlag.Equals(true))
                    {
                        result.TTMPC_ActiveFlag = true;
                        _ttcontext.Update(result);
                        var flag = _ttcontext.SaveChanges();
                        if (flag.Equals(1))
                        {
                            acd.returnval = true;
                            acd.returnduplicatestatus = "active";
                        }
                        else
                        {
                            acd.returnval = false;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }









    }
}
