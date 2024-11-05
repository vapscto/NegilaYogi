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

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTPeriodAllocationImpl : Interfaces.PeriodAllocationInterface
    {
        private static ConcurrentDictionary<string, TTMasterCategoryDTO> _login =
         new ConcurrentDictionary<string, TTMasterCategoryDTO>();

        public TTContext _ttcontext;
        readonly ILogger<TTPeriodAllocationImpl> _logger;
        public TTPeriodAllocationImpl(TTContext ttcntx, ILogger<TTPeriodAllocationImpl> acdimpl)
        {
            _ttcontext = ttcntx;
            _logger = acdimpl;
        }



        public TTPeriodAllocationDTO savedetail(TTPeriodAllocationDTO _period)
        {
            TT_Master_Period_ClasswiseDMO objpgee = Mapper.Map<TT_Master_Period_ClasswiseDMO>(_period);
            try
            {
                for (var y = 0; y < _period.Temp_class_Array.Count(); y++)
                {
                    //var a = 0;

                    if (_period.temp_period_Array.Length>0)
                    {
                        var lorg = _ttcontext.TT_Master_Period_ClasswiseDMO.Where(t => t.ASMCL_Id.Equals(_period.Temp_class_Array[y].ASMCL_Id) && t.ASMAY_Id.Equals(_period.ASMAY_Id) && t.MI_Id.Equals(_period.MI_Id)).ToList();
                        if (lorg.Count>0)
                        {

                            foreach (var item in lorg)
                            {
                                _ttcontext.Remove(item);
                            }

                            //_ttcontext.SaveChanges();
                        }
                    }


                    for (var z = 0; z < _period.temp_period_Array.Count(); z++)
                    {
                        //if (a == 0)
                        //{
                        //    List<TT_Master_Period_ClasswiseDMO> lorg = new List<TT_Master_Period_ClasswiseDMO>();
                        //    lorg = _ttcontext.TT_Master_Period_ClasswiseDMO.Where(t => t.ASMCL_Id.Equals(_period.Temp_class_Array[y].ASMCL_Id) && t.ASMAY_Id.Equals(_period.ASMAY_Id) && t.MI_Id.Equals(_period.MI_Id)).ToList();
                        //    if (lorg.Any())
                        //    {
                        //        for (int i = 0; i < lorg.Count; i++)
                        //        {
                        //            _ttcontext.Remove(lorg.ElementAt(i));
                        //            _ttcontext.SaveChanges();
                        //        }
                        //        a = 1;
                        //    }
                        //}
                        TT_Master_Period_ClasswiseDMO result = new TT_Master_Period_ClasswiseDMO();
                        result.MI_Id = _period.MI_Id;
                        result.ASMAY_Id = _period.ASMAY_Id;
                        result.ASMCL_Id = _period.Temp_class_Array[y].ASMCL_Id;
                        result.TTMP_Id = _period.temp_period_Array[z].TTMP_Id;
                        result.TTMPC_ActiveFlag = true;
                        result.CreatedDate = DateTime.Now;
                        result.UpdatedDate = DateTime.Now;
                        _ttcontext.Add(result);
                       
                    }


                    var contactExists = _ttcontext.SaveChanges();
                    if (contactExists >0)
                    {
                        _period.returnval = true;
                    }
                    else
                    {
                        _period.returnval = false;
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _period;
        }


        public TTPeriodAllocationDTO saveperiod(TTPeriodAllocationDTO _period)
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
                            var result2 = _ttcontext.TT_Master_Period_ClasswiseDMO.Where(t => t.TTMP_Id.Equals(result1.FirstOrDefault()) && t.MI_Id.Equals(_period.MI_Id) && t.TTMPC_ActiveFlag == true).Select(g => g.TTMPC_Id).ToList();
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

        public TTPeriodAllocationDTO getdetails(TTPeriodAllocationDTO data)
        {
            try
            {
                List<TT_Master_PeriodDMO> periods = new List<TT_Master_PeriodDMO>();
                periods = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id).ToList();
                data.periodlist = periods.Distinct().ToArray();
                data.count = _ttcontext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id).Count();
                List<AcademicYear> year = new List<AcademicYear>();
                year = _ttcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(ee=>ee.ASMAY_Order).ToList();
                data.acayear = year.Distinct().ToArray();

                List<TTMasterCategoryDMO> mcat = new List<TTMasterCategoryDMO>();
                mcat = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList();
                data.catelist = mcat.Distinct().ToArray();

                List<TT_Master_DayDMO> day = new List<TT_Master_DayDMO>();
                day = _ttcontext.TT_Master_DayDMO.Where(d => d.MI_Id == data.MI_Id && d.TTMD_ActiveFlag == true).ToList();
                data.day_list = day.Distinct().ToArray();

                data.TempararyArray = (from a in _ttcontext.TTMasterCategoryDMO
                                       from b in _ttcontext.TT_Category_Class_DMO
                                       from c in _ttcontext.School_M_Class
                                       where (a.MI_Id == b.MI_Id && c.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && a.TTMC_Id == b.TTMC_Id && c.ASMCL_Id == b.ASMCL_Id && a.TTMC_ActiveFlag==true)
                                       select new TTPeriodAllocationDTO
                                       {
                                           ASMCL_Id = c.ASMCL_Id,
                                           ASMCL_ClassName = c.ASMCL_ClassName,
                                           TTMC_Id = a.TTMC_Id,
                                           TTMC_CategoryName = a.TTMC_CategoryName,
                                       }
        ).Distinct().GroupBy(c => c.ASMCL_Id).Select(c => c.First()).ToArray();

                data.All_list = (from a in _ttcontext.AcademicYear
                                 from b in _ttcontext.TT_Master_PeriodDMO
                                 from c in _ttcontext.School_M_Class
                                 from d in _ttcontext.TT_Master_Period_ClasswiseDMO
                                 where (d.MI_Id == data.MI_Id && d.TTMP_Id == b.TTMP_Id && d.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == d.ASMAY_Id)
                                 select new TTPeriodAllocationDTO
                                 {
                                     TTMPC_Id = d.TTMPC_Id,
                                     TTMP_PeriodName = b.TTMP_PeriodName,
                                     ASMAY_Year = a.ASMAY_Year,
                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                     ASMCL_Id = d.ASMCL_Id,
                                     TTMPC_ActiveFlag = d.TTMPC_ActiveFlag
                                 }
        ).Distinct().OrderBy(x => x.ASMCL_ClassName).ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return data;

        }

        public TTPeriodAllocationDTO getclasses(TTPeriodAllocationDTO data)
        {
            try
            {

                data.TempararyArray = (from a in _ttcontext.TTMasterCategoryDMO
                                       from b in _ttcontext.TT_Category_Class_DMO
                                       from c in _ttcontext.School_M_Class
                                       where (a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.TTMC_Id == b.TTMC_Id && b.ASMCL_Id == c.ASMCL_Id && a.TTMC_Id == data.TTMC_Id) //&& b.TTCC_ActiveFlag==true
                                       select new TTPeriodAllocationDTO
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

        public TTPeriodAllocationDTO getcategories(TTPeriodAllocationDTO data)
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
        public TTPeriodAllocationDTO getperiod_class(TTPeriodAllocationDTO data)
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
                                       select new TTPeriodAllocationDTO
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

        public TTPeriodAllocationDTO getpageedit(int id)
        {
            TTPeriodAllocationDTO page = new TTPeriodAllocationDTO();
            try
            {
                page.periodlistedit = (from TT_Master_Period_Classwise in _ttcontext.TT_Master_Period_ClasswiseDMO
                                       from TTMasterCategoryDMO in _ttcontext.TTMasterCategoryDMO
                                       from TT_Category_Class_DMO in _ttcontext.TT_Category_Class_DMO
                                       where (TTMasterCategoryDMO.TTMC_Id == TT_Category_Class_DMO.TTMC_Id && TT_Master_Period_Classwise.ASMCL_Id == TT_Category_Class_DMO.ASMCL_Id && TT_Master_Period_Classwise.TTMPC_Id == id)
                                       select new TTPeriodAllocationDTO
                                       {

                                           TTMPC_Id = TT_Master_Period_Classwise.TTMPC_Id,
                                           ASMAY_Id = TT_Master_Period_Classwise.ASMAY_Id,
                                           ASMCL_Id = TT_Master_Period_Classwise.ASMCL_Id,
                                           TTMP_Id = TT_Master_Period_Classwise.TTMP_Id,
                                           TTMC_Id = TTMasterCategoryDMO.TTMC_Id
                                       }
                                  ).ToArray();

                var lorg = _ttcontext.TT_Master_Period_ClasswiseDMO.Where(t => t.TTMPC_Id.Equals(id)).Select(l => l.TTMP_Id).FirstOrDefault();



                var g = (from a in _ttcontext.TT_Master_Period_ClasswiseDMO
                         from b in _ttcontext.TT_Master_PeriodDMO
                         where (a.TTMP_Id == b.TTMP_Id && a.TTMPC_ActiveFlag == true && a.TTMP_Id == lorg)
                         select new TTPeriodAllocationDTO
                         {
                             TTMP_PeriodName = b.TTMP_PeriodName
                         }
                                    ).Select(l => l.TTMP_PeriodName).FirstOrDefault();


                page.period_count = Convert.ToInt32(g);

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return page;
        }

        public TTPeriodAllocationDTO deactivate(TTPeriodAllocationDTO acd)
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
                            var result_1 = _ttcontext.TT_Master_Period_ClasswiseDMO.Where(t => t.TTMP_Id.Equals(periods[i].TTMP_Id)&&t.TTMPC_ActiveFlag==true).ToList();
                            if(result_1.Count>0)
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

        public TTPeriodAllocationDTO deactivate1(TTPeriodAllocationDTO acd)
        {
            try
            {
                TT_Master_Period_ClasswiseDMO pge = Mapper.Map<TT_Master_Period_ClasswiseDMO>(acd);
                if (pge.TTMPC_Id > 0)
                {

                    var result = _ttcontext.TT_Master_Period_ClasswiseDMO.Single(t => t.TTMPC_Id.Equals(pge.TTMPC_Id));
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

        public TTPeriodAllocationDTO deleterec(int id)
        {
            TTPeriodAllocationDTO period = new TTPeriodAllocationDTO();
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
                _logger.LogError(ee.Message);
            }
            return period;
        }


    }
}
