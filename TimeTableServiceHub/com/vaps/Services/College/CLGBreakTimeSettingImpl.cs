using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeTableServiceHub.Services
{
    public class CLGBreakTimeSettingImpl : Interfaces.CLGBreakTimeSettingInterface
    {
        private static ConcurrentDictionary<string, CLGBreakTimeSettingDTO> _login =
             new ConcurrentDictionary<string, CLGBreakTimeSettingDTO>();

        public TTContext _TTContext;
        ILogger<CLGBreakTimeSettingImpl> _dataimpl;
        public DomainModelMsSqlServerContext _db;
        public CLGBreakTimeSettingImpl(TTContext academiccontext, ILogger<CLGBreakTimeSettingImpl> dataimpl, DomainModelMsSqlServerContext db)
        {
            _TTContext = academiccontext;
            _dataimpl = dataimpl;
            _db = db;
        }
        #region LOAD ALL DATA
        public CLGBreakTimeSettingDTO getalldetails(CLGBreakTimeSettingDTO data)
        {
            try
            {
                //FILL DROPDOWNS
                data.categorylist = _TTContext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().ToArray();
                data.academiclist = _TTContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(yy => yy.ASMAY_Order).ToList().ToArray();
                data.daydropdown = _TTContext.TT_Master_DayDMO.Where(u => u.MI_Id == data.MI_Id && u.TTMD_ActiveFlag == true).Distinct().ToArray();

                data.breaktimelist = (from a in _TTContext.CLGTT_Master_BreakDMO
                                      from b in _TTContext.AcademicYear
                                      from c in _TTContext.MasterCourseDMO
                                      from d in _TTContext.ClgMasterBranchDMO
                                      from e in _TTContext.CLG_Adm_Master_SemesterDMO
                                      from f in _TTContext.TTMasterCategoryDMO
                                      from g in _TTContext.TT_Master_DayDMO
                                      where a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == e.MI_Id && a.MI_Id == f.MI_Id && a.MI_Id == g.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == d.AMB_Id && a.AMSE_Id == e.AMSE_Id && a.TTMC_Id == f.TTMC_Id && a.TTMD_Id == g.TTMD_Id && a.MI_Id==data.MI_Id
                                      select new CLGBreakTimeSettingDTO
                                      {
                                          TTMBC_Id = a.TTMBC_Id,
                                          AMCO_CourseName=c.AMCO_CourseName,
                                          AMB_BranchName=d.AMB_BranchName,
                                          AMSE_SEMName=e.AMSE_SEMName,
                                          ASMAY_Year=b.ASMAY_Year,
                                          ASMAY_Order=b.ASMAY_Order,
                                          TTMC_CategoryName=f.TTMC_CategoryName,
                                          TTMD_DayName=g.TTMD_DayName,
                                         TTMBC_AfterPeriod=a.TTMBC_AfterPeriod,
                                          TTMBC_BreakName=a.TTMBC_BreakName,
                                           TTMBC_BreakStartTime = a.TTMBC_BreakStartTime,
                                          TTMBC_BreakEndTime = a.TTMBC_BreakEndTime,
                                          TTMBC_ActiveFlag = a.TTMBC_ActiveFlag,
                                      }
                                    ).Distinct().OrderByDescending(r=>r.ASMAY_Order).ToArray();



            }
            catch (Exception ee)
            {
                // Console.WriteLine(ee.Message);
            }
            return data;

        }
        #endregion

        #region SAVE BREAK TIME
        public CLGBreakTimeSettingDTO savetimedetail(CLGBreakTimeSettingDTO data)
        {
            
            try
            {
                if (data.TTMBC_Id>0)
                {
                    foreach (var sem in data.ArrayClassList)
                    {
                        foreach (var day in data.ArrayDayList)
                        {

                            var dupcheck = _TTContext.CLGTT_Master_BreakDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.TTMC_Id == data.TTMC_Id && t.AMB_Id == data.TTMB_Id && t.AMSE_Id == sem.AMSE_Id && t.TTMD_Id == day.TTMD_Id && t.TTMBC_BreakStartTime == data.TTMBC_BreakStartTime && t.TTMBC_BreakEndTime == data.TTMBC_BreakEndTime && t.TTMBC_BreakName == data.TTMBC_BreakName && t.TTMBC_AfterPeriod == data.TTMBC_AfterPeriod && t.TTMBC_Id != data.TTMBC_Id).ToList();


                            if (dupcheck.Count == 0)
                            {

                                var result = _TTContext.CLGTT_Master_BreakDMO.Single(f => f.TTMBC_Id == data.TTMBC_Id);

                                result.TTMBC_AfterPeriod = data.TTMBC_AfterPeriod;
                                result.TTMBC_BreakName = data.TTMBC_BreakName.Trim();
                                result.TTMBC_BreakStartTime = data.TTMBC_BreakStartTime;
                                result.TTMBC_BreakEndTime = data.TTMBC_BreakEndTime;
                                result.TTMBC_ActiveFlag = true;
                                result.UpdatedDate = DateTime.Now;
                                _TTContext.Update(result);

                                var beforeperiod = _TTContext.CLGTT_Master_Break_BefPeriodsDMO.Where(t => t.TTMBC_Id == data.TTMBC_Id).ToList();
                                if (beforeperiod.Count>0)
                                {
                                    foreach (var bbfp in beforeperiod)
                                    {
                                        _TTContext.Remove(bbfp);
                                    }
                                }

                                var afterperiod = _TTContext.CLGTT_Master_Break_AftPeriodsDMO.Where(t => t.TTMBC_Id == data.TTMBC_Id).ToList();
                                if (afterperiod.Count > 0)
                                {
                                    foreach (var aafp in afterperiod)
                                    {
                                        _TTContext.Remove(aafp);
                                    }
                                }

                                foreach (var aft in data.ArrayafterperiodsList)
                                {
                                    CLGTT_Master_Break_AftPeriodsDMO aftobj = new CLGTT_Master_Break_AftPeriodsDMO();
                                    aftobj.TTMBC_Id = data.TTMBC_Id;
                                    aftobj.TTMP_Id = _TTContext.TT_Master_PeriodDMO.Single(t => t.MI_Id == data.MI_Id && t.TTMP_PeriodName == aft.TTPeriodnameA).TTMP_Id;
                                    _TTContext.Add(aftobj);

                                }

                                foreach (var beff in data.ArraybeforeperiodsList)
                                {
                                    CLGTT_Master_Break_BefPeriodsDMO befobj = new CLGTT_Master_Break_BefPeriodsDMO();
                                    befobj.TTMBC_Id = data.TTMBC_Id;
                                    befobj.TTMP_Id = _TTContext.TT_Master_PeriodDMO.Single(t => t.MI_Id == data.MI_Id && t.TTMP_PeriodName == beff.TTPeriodnameB).TTMP_Id;
                                    _TTContext.Add(befobj);

                                }

                            }

                        }

                    }

                    var exist = _TTContext.SaveChanges();
                    if (exist > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {
                    foreach (var sem in data.ArrayClassList)
                    {
                        foreach (var day in data.ArrayDayList)
                        {
                            var dupcheck = _TTContext.CLGTT_Master_BreakDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.TTMC_Id == data.TTMC_Id && t.AMB_Id == data.TTMB_Id && t.AMSE_Id == sem.AMSE_Id && t.TTMD_Id == day.TTMD_Id && t.TTMBC_BreakStartTime == data.TTMBC_BreakStartTime && t.TTMBC_BreakEndTime == data.TTMBC_BreakEndTime && t.TTMBC_BreakName == data.TTMBC_BreakName && t.TTMBC_AfterPeriod == data.TTMBC_AfterPeriod).ToList();
                            if (dupcheck.Count==0)
                            {
                                CLGTT_Master_BreakDMO obj = new CLGTT_Master_BreakDMO();
                                obj.MI_Id = data.MI_Id;
                                obj.ASMAY_Id = data.ASMAY_Id;
                                obj.TTMC_Id = data.TTMC_Id;
                                obj.AMCO_Id = data.AMCO_Id;
                                obj.AMB_Id = data.AMB_Id;
                                obj.AMSE_Id = sem.AMSE_Id;
                                obj.TTMD_Id = day.TTMD_Id;
                                obj.TTMBC_AfterPeriod = data.TTMBC_AfterPeriod;
                                obj.TTMBC_BreakName = data.TTMBC_BreakName.Trim();
                                obj.TTMBC_BreakStartTime = data.TTMBC_BreakStartTime;
                                obj.TTMBC_BreakEndTime = data.TTMBC_BreakEndTime;
                                obj.TTMBC_ActiveFlag = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                _TTContext.Add(obj);
                                

                                foreach (var aft in data.ArrayafterperiodsList)
                                {
                                    CLGTT_Master_Break_AftPeriodsDMO aftobj = new CLGTT_Master_Break_AftPeriodsDMO();
                                    aftobj.TTMBC_Id = obj.TTMBC_Id;
                                    aftobj.TTMP_Id = _TTContext.TT_Master_PeriodDMO.Single(t => t.MI_Id == data.MI_Id && t.TTMP_PeriodName == aft.TTPeriodnameA).TTMP_Id;
                                    _TTContext.Add(aftobj);

                                }

                                foreach (var beff in data.ArraybeforeperiodsList)
                                {
                                    CLGTT_Master_Break_BefPeriodsDMO befobj = new CLGTT_Master_Break_BefPeriodsDMO();
                                    befobj.TTMBC_Id = obj.TTMBC_Id;
                                    befobj.TTMP_Id = _TTContext.TT_Master_PeriodDMO.Single(t => t.MI_Id == data.MI_Id && t.TTMP_PeriodName == beff.TTPeriodnameB).TTMP_Id;
                                    _TTContext.Add(befobj);

                                }
                                
                            }

                        }

                    }

                    var exist = _TTContext.SaveChanges();
                    if (exist>0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                }



                //if (data.TTMD_Id > 0)
                //{
                //    var res = _TTContext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && (t.TTMD_DayName.Trim().ToLower() == data.TTMD_DayName.Trim().ToLower() || t.TTMD_DayCode.Trim().ToLower() == data.TTMD_DayCode.Trim().ToLower()) && t.TTMD_Id != data.TTMD_Id).ToList();
                //    if (res.Count > 0)
                //    {
                //        data.returnduplicatestatus = "Duplicate";
                //    }
                //    else
                //    {
                //        var result = _TTContext.TT_Master_DayDMO.Single(t => t.MI_Id == data.MI_Id && t.TTMD_Id == data.TTMD_Id);
                //        result.TTMD_DayCode = data.TTMD_DayCode.ToUpper();
                //        result.TTMD_DayName = data.TTMD_DayName.ToUpper();
                //        result.UpdatedDate = DateTime.Now;
                //        result.TTMD_ActiveFlag = true;
                //        _TTContext.Update(result);
                //        var contactExists = _TTContext.SaveChanges();
                //        if (contactExists == 1)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }

                //    }
                //}
                //else
                //{
                //    var res = _TTContext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && (t.TTMD_DayName.Trim().ToLower() == data.TTMD_DayName.Trim().ToLower() || t.TTMD_DayCode.Trim().ToLower() == data.TTMD_DayCode.Trim().ToLower())).ToList();
                //    if (res.Count() > 0)
                //    {
                //        data.returnduplicatestatus = "Duplicate";
                //    }
                //    else
                //    {
                //        long OID = 0;
                //        var orderId = _TTContext.TT_Master_DayDMO.Where(f => f.MI_Id == data.MI_Id).ToList();
                //        if (orderId.Count==0)
                //        {
                //            OID = 1;
                //        }
                //        else
                //        {
                //         long ooid= orderId.Select(r => r.Order_Id).Max();
                //            OID = ooid + 1;
                //        }

                //        TT_Master_DayDMO obj = new TT_Master_DayDMO();
                //        obj.MI_Id = data.MI_Id;
                //        obj.TTMD_DayName = data.TTMD_DayName.Trim().ToUpper();
                //        obj.TTMD_DayCode = data.TTMD_DayCode.Trim().ToUpper(); ;
                //        obj.Order_Id = OID;
                //        obj.TTMD_ActiveFlag = true;
                //        obj.CreatedDate = DateTime.Now;
                //        obj.UpdatedDate = DateTime.Now;
                //        _TTContext.Add(obj);
                //        var contactExists = _TTContext.SaveChanges();
                //        if (contactExists == 1)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }
                //    }
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        #endregion

        #region EDIT DAY
        public CLGBreakTimeSettingDTO editDay(CLGBreakTimeSettingDTO data)
        {

            try
            {
                data.Daylistedit = _TTContext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMD_Id == data.TTMD_Id).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        #endregion

        #region ACTIVE/DEACTIVE BREAKTIME
        public CLGBreakTimeSettingDTO deactivate(CLGBreakTimeSettingDTO data)
        {
            try
            {

                if (data.TTMBC_Id > 0)
                {
                    var result = _TTContext.CLGTT_Master_BreakDMO.Single(t => t.TTMBC_Id==data.TTMBC_Id);
                    
                        if (result.TTMBC_ActiveFlag==true)
                        {
                            result.TTMBC_ActiveFlag = false;
                        }
                        else
                        {
                            result.TTMBC_ActiveFlag = true;
                        }
                        _TTContext.Update(result);
                        var flag = _TTContext.SaveChanges();
                        if (flag.Equals(1))
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                   

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion

        #region GET BRANCH
        public CLGBreakTimeSettingDTO getBranch(CLGBreakTimeSettingDTO data)
        {
            try
            {


                data.branchlist = (from a in _TTContext.CLG_Adm_College_AY_CourseDMO
                                   from b in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from c in _TTContext.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == c.AMB_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true
                                   select c
                                 ).Distinct().ToArray();
            

              

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion

        #region SAVE THE SEMISTER WISE  DAY
        public CLGBreakTimeSettingDTO getmaximumperiodscount(CLGBreakTimeSettingDTO data)
        {
            try
            {
                try
                {
                    if (data.classidscount > 1)
                    {
                        var lorg = _TTContext.TT_Master_PeriodDMO.Count(t => t.MI_Id == data.MI_Id && t.TTMP_ActiveFlag == true);
                        data.classidscountreturn = lorg;
                    }
                    else
                    {
                        var lorg = _TTContext.TT_Master_PeriodDMO.Count(t => t.MI_Id==data.MI_Id && t.TTMP_ActiveFlag==true);
                        data.classidscountreturn = lorg;
                    }
                }
                catch (Exception ee)
                {
                    data.returnval = false;

                }
                return data;
            }
            catch (Exception ee)
            {
                _dataimpl.LogError(ee.Message);
                _dataimpl.LogDebug(ee.Message);
            }

            return data;
        }
        #endregion

        #region EDIT DETAILS FOR BREAK TIME
        public CLGBreakTimeSettingDTO geteditdetails(CLGBreakTimeSettingDTO data)
        {
            try
            {
                if (data.TTMBC_Id>0)
                {
                    data.breaktimelistedit = _TTContext.CLGTT_Master_BreakDMO.Where(jj => jj.MI_Id == data.MI_Id && jj.TTMBC_Id == data.TTMBC_Id).ToArray();
                }

            }
            catch (Exception ee)
            {
               
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion
    }
}
