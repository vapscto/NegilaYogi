using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
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
    public class PeriodTimeSettingImpl : Interfaces.PeriodTimeSettingInterface
    {

        public TTContext _ttcontext;

        public PeriodTimeSettingImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public TT_Master_Day_Period_TimeDTO savedetail(TT_Master_Day_Period_TimeDTO data)
        {
            
            try
            {


                if (data.datidss.Length>0)
                {
                    if (data.TTMDPT_Id>0)
                    {

                        foreach (var item in data.datidss)
                        {
                            var res = _ttcontext.TT_Master_Day_Period_TimeDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTMC_Id == data.TTMC_Id && t.TTMP_Id == data.TTMP_Id && t.TTMD_Id == item.TTMD_Id && t.TTMDPT_Id!=data.TTMDPT_Id
                            ).ToList();

                            if (res.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                                data.dupcnt += 1;
                            }
                            else
                            {
                                var result = _ttcontext.TT_Master_Day_Period_TimeDMO.Single(t => t.TTMDPT_Id == data.TTMDPT_Id && t.MI_Id == data.MI_Id);
                                result.MI_Id = data.MI_Id;
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.TTMC_Id = data.TTMC_Id;
                                result.TTMD_Id = item.TTMD_Id;
                                result.TTMP_Id = data.TTMP_Id;
                                result.TTMDPT_StartTime = data.TTMDPT_StartTime;
                                result.TTMDPT_EndTime = data.TTMDPT_EndTime;
                                result.TTMDPT_ActiveFlag = true;
                                result.UpdatedDate = DateTime.Now;
                                _ttcontext.Update(result);
                            }

                        }
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                            data.sucnt += 1;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        foreach (var item in data.datidss)
                        {
                            var res = _ttcontext.TT_Master_Day_Period_TimeDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id && t.TTMC_Id == data.TTMC_Id && t.TTMP_Id == data.TTMP_Id && t.TTMD_Id == item.TTMD_Id 
                            //&& t.TTMDPT_StartTime.Equals(data.TTMDPT_StartTime) && t.TTMDPT_EndTime.Equals(data.TTMDPT_EndTime)
                            ).ToList();

                            if (res.Count() > 0)
                            {
                                data.returnduplicatestatus = "Duplicate";
                                data.dupcnt += 1;
                            }
                            else
                            {
                                TT_Master_Day_Period_TimeDMO obj = new TT_Master_Day_Period_TimeDMO();

                                obj.MI_Id = data.MI_Id;
                                obj.ASMAY_Id = data.ASMAY_Id;
                                obj.TTMC_Id = data.TTMC_Id;
                                obj.TTMD_Id = item.TTMD_Id;
                                obj.TTMP_Id = data.TTMP_Id;
                                obj.TTMDPT_StartTime = data.TTMDPT_StartTime;
                                obj.TTMDPT_EndTime = data.TTMDPT_EndTime;
                                obj.TTMDPT_ActiveFlag = true;
                                obj.CreatedDate = DateTime.Now ;
                                obj.UpdatedDate = DateTime.Now ;
                                _ttcontext.Add(obj);
                            }

                        }
                        var contactExists = _ttcontext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                            data.sucnt += 1;
                        }
                        else
                        {
                            data.returnval = false;
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

        public TT_Master_Day_Period_TimeDTO getdetails(int id)
        {
            TT_Master_Day_Period_TimeDTO TTMC = new TT_Master_Day_Period_TimeDTO();
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _ttcontext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active == true).OrderBy(t => t.ASMAY_Id).OrderByDescending(rr=>rr.ASMAY_Order).ToList();
                TTMC.academicdrp = allyear.ToArray();

                List<TTMasterCategoryDMO> category = new List<TTMasterCategoryDMO>();
                category = _ttcontext.TTMasterCategoryDMO.Where(t => t.MI_Id == id && t.TTMC_ActiveFlag == true).OrderBy(t => t.TTMC_Id).ToList();
                TTMC.categorylist = category.ToArray();

                List<TT_Master_DayDMO> day = new List<TT_Master_DayDMO>();
                day = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id == id && t.TTMD_ActiveFlag == true).ToList();
                TTMC.daydrp = day.ToArray();

                List<TT_Master_PeriodDMO> period = new List<TT_Master_PeriodDMO>();
                period = _ttcontext.TT_Master_PeriodDMO.Where(t => t.MI_Id == id && t.TTMP_ActiveFlag == true).OrderBy(t => t.TTMP_PeriodName).ToList();
                TTMC.perioddrp = period.ToArray();
                TTMC.gridview = (from a in _ttcontext.AcademicYear
                                 from b in _ttcontext.TTMasterCategoryDMO
                                 from c in _ttcontext.TT_Master_DayDMO
                                 from d in _ttcontext.TT_Master_PeriodDMO
                                 from e in _ttcontext.TT_Master_Day_Period_TimeDMO
                                 where (a.MI_Id == e.MI_Id && b.MI_Id == e.MI_Id && c.MI_Id == e.MI_Id && d.MI_Id == e.MI_Id && e.MI_Id == id && a.ASMAY_Id == e.ASMAY_Id && b.TTMC_Id == e.TTMC_Id && c.TTMD_Id == e.TTMD_Id && d.TTMP_Id == e.TTMP_Id)
                                 select new TT_Master_Day_Period_TimeDTO
                                 {
                                     TTMDPT_Id = e.TTMDPT_Id,
                                     ASMAY_Year = a.ASMAY_Year,
                                     TTMC_CategoryName = b.TTMC_CategoryName,
                                     TTMD_DayName = c.TTMD_DayName,
                                     TTMP_PeriodName = d.TTMP_PeriodName,
                                     TTMDPT_StartTime = e.TTMDPT_StartTime,
                                     TTMDPT_EndTime = e.TTMDPT_EndTime,
                                     TTMDPT_ActiveFlag = e.TTMDPT_ActiveFlag
                                 }
                    ).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TT_Master_Day_Period_TimeDTO getpageedit(int id)
        {
            TT_Master_Day_Period_TimeDTO page = new TT_Master_Day_Period_TimeDTO();
            try
            {
                List<TT_Master_Day_Period_TimeDMO> lorg = new List<TT_Master_Day_Period_TimeDMO>();
                lorg = _ttcontext.TT_Master_Day_Period_TimeDMO.AsNoTracking().Where(t => t.TTMDPT_Id.Equals(id)).ToList();
                page.periodlistedit = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TT_Master_Day_Period_TimeDTO deleterec(TT_Master_Day_Period_TimeDTO acd)
        {
            try
            {
                if (acd.TTMDPT_Id > 0)
                {
                    var result = _ttcontext.TT_Master_Day_Period_TimeDMO.Single(t => t.TTMDPT_Id.Equals(acd.TTMDPT_Id));
                    if (result.TTMDPT_ActiveFlag.Equals(false))
                    {
                        result.TTMDPT_ActiveFlag = true;
                    }
                    else
                    {
                        result.TTMDPT_ActiveFlag = false;
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


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return acd;
        }

    }
}
