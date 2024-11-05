using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class ShiftSettingMasterImpl:Interfaces.ShiftSettingMasterInterface
    {
        public FOContext _FOContext;


        public ShiftSettingMasterImpl(FOContext fOContext)
        {
            _FOContext = fOContext;
        }
        public MasterShiftsTimingsDTO getdata(MasterShiftsTimingsDTO data)
        {
            try
            {
                List<HolidayWorkingDayTypeDMO> alltype = new List<HolidayWorkingDayTypeDMO>();
                alltype = _FOContext.holidayWorkingDayType.Where(y => y.FOHWDT_ActiveFlg == true && y.MI_Id == data.MI_Id).ToList();
                data.filltype = alltype.Distinct().ToArray();

                loadingdata(data, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public MasterShiftsTimingsDTO savedatadelegate(MasterShiftsTimingsDTO data)
        {
            try
            {
                MasterShiftsTimingsDMO enq = Mapper.Map<MasterShiftsTimingsDMO>(data);
                if (enq.FOMST_Id > 0)
                {

                    var resultnew = _FOContext.masterShifts.Where(t => t.FOMS_ShiftName.ToLower().Equals(data.FOMS_ShiftName.ToLower()) && t.MI_Id == data.MI_Id).ToList();
                    //if (resultnew.Count() > 0)
                    //{
                    //    data.returnvalue = "Duplicate";
                    //}
                    //else
                    //{
                    var result = _FOContext.masterShiftsTimings.Single(t => t.FOMST_Id.Equals(data.FOMST_Id));
                    // result.FOHWDT_Id = enq.FOHWDT_Id;
                    result.FOMST_FDWHrMin = enq.FOMST_FDWHrMin;
                    result.FOMST_HDWHrMin = enq.FOMST_HDWHrMin;
                    result.FOMST_IHalfLoginTime = enq.FOMST_IHalfLoginTime;
                    result.FOMST_IHalfLogoutTime = enq.FOMST_IHalfLogoutTime;
                    result.FOMST_IIHalfLoginTime = enq.FOMST_IIHalfLoginTime;
                    result.FOMST_IIHalfLogoutTime = enq.FOMST_IIHalfLogoutTime;
                    result.FOMST_DelayPerShiftHrMin = enq.FOMST_DelayPerShiftHrMin;
                    result.FOMST_EarlyPerShiftHrMin = enq.FOMST_EarlyPerShiftHrMin;
                    result.FOMST_LunchHoursDuration = enq.FOMST_LunchHoursDuration;
                    result.FOMST_FixTimings = enq.FOMST_FixTimings;
                    result.UpdatedDate = DateTime.Now;
                    _FOContext.Update(result);
                    MasterShiftsDMO enq1 = Mapper.Map<MasterShiftsDMO>(data);
                    var result1 = _FOContext.masterShifts.Single(t => t.FOMS_Id.Equals(result.FOMS_Id));
                    result1.FOMS_ShiftName = enq1.FOMS_ShiftName;
                    result1.MI_Id = enq1.MI_Id;
                    result1.UpdatedDate = DateTime.Now;
                    _FOContext.Update(result1);
                    var flag1 = _FOContext.SaveChanges();
                    if (flag1 == 2)
                        data.returnvalue = "success";
                    else
                        data.returnvalue = "fail";
                    // }
                }
                else
                {
                    var resultnew = _FOContext.masterShifts.Where(t => t.FOMS_ShiftName.ToLower().Equals(data.FOMS_ShiftName.ToLower()) && t.MI_Id == data.MI_Id).ToList();
                    if (resultnew.Count() > 0)
                    {
                        data.returnvalue = "Duplicate";
                    }
                    else
                    {
                        MasterShiftsDMO enqnew = Mapper.Map<MasterShiftsDMO>(data);
                        enqnew.FOMS_ActiveFlg = true;
                        enqnew.CreatedDate = DateTime.Now;
                        enqnew.UpdatedDate = DateTime.Now;
                        _FOContext.Add(enqnew);

                        MasterShiftsTimingsDMO enqnew1 = Mapper.Map<MasterShiftsTimingsDMO>(data);
                        enqnew1.FOMS_Id = enqnew.FOMS_Id;
                        enqnew1.CreatedDate = DateTime.Now;
                        enqnew1.UpdatedDate = DateTime.Now;
                        _FOContext.Add(enqnew1);
                        var flag1 = _FOContext.SaveChanges();
                        if (flag1 == 2)
                        {
                            data.returnvalue = "success";
                        }
                        else
                        {
                            data.returnvalue = "fail";
                        }
                    }
                }
                loadingdata(data, 0);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public MasterShiftsTimingsDTO getpageedit(int id)
        {
            MasterShiftsTimingsDTO data = new MasterShiftsTimingsDTO();
            try
            {
                loadingdata(data, id);

            }
            catch (Exception ee)
            {
                //_logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public MasterShiftsTimingsDTO deactivate(MasterShiftsTimingsDTO acd)
        {
            try
            {
                MasterShiftsDMO feepge = Mapper.Map<MasterShiftsDMO>(acd);
                if (feepge.FOMS_Id > 0)
                {
                    var result = _FOContext.masterShifts.Single(t => t.FOMS_Id == feepge.FOMS_Id);
                    if (result.FOMS_ActiveFlg == true)
                    {
                        result.FOMS_ActiveFlg = false;
                    }
                    else
                    {
                        result.FOMS_ActiveFlg = true;
                    }
                    _FOContext.Update(result);
                    var flag = _FOContext.SaveChanges();
                    if (flag == 1)
                    {
                        acd.returnvalue = "success";
                    }
                    else
                    {
                        acd.returnvalue = "fail";
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        public MasterShiftsTimingsDTO getalldetailsviewrecords1(int id)
        {
            MasterShiftsTimingsDTO data = new MasterShiftsTimingsDTO();
            try
            {
                loadingdata(data, id);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        private void loadingdata(MasterShiftsTimingsDTO data, int id)
        {
            if (id > 0)
            {
                //data.filldata = (from a in _FOContext.masterShifts
                //                 from b in _FOContext.masterShiftsTimings
                //                 from c in _FOContext.holidayWorkingDayType
                //                 where (a.FOMS_Id == b.FOMS_Id && b.FOHWDT_Id == c.FOHWDT_Id && b.FOMST_Id == id)
                //                 select new MasterShiftsTimingsDTO
                //                 {
                //                     FOMS_Id = a.FOMS_Id,
                //                     FOMS_ShiftName = a.FOMS_ShiftName,
                //                     FOMST_Id = b.FOMST_Id,
                //                     FOHWDT_Id = b.FOHWDT_Id,
                //                     FOHTWD_HolidayWDType = c.FOHTWD_HolidayWDType,
                //                     FOMS_ActiveFlg = a.FOMS_ActiveFlg,
                //                     FOMST_FDWHrMin = b.FOMST_FDWHrMin,
                //                     FOMST_HDWHrMin = b.FOMST_HDWHrMin,
                //                     FOMST_IHalfLoginTime = b.FOMST_IHalfLoginTime,
                //                     FOMST_IHalfLogoutTime = b.FOMST_IHalfLogoutTime,
                //                     FOMST_IIHalfLoginTime = b.FOMST_IIHalfLoginTime,
                //                     FOMST_IIHalfLogoutTime = b.FOMST_IIHalfLogoutTime,
                //                     FOMST_DelayPerShiftHrMin = b.FOMST_DelayPerShiftHrMin,
                //                     FOMST_EarlyPerShiftHrMin = b.FOMST_EarlyPerShiftHrMin,
                //                     FOMST_LunchHoursDuration = b.FOMST_LunchHoursDuration,
                //                     FOMST_BlockAttendance = b.FOMST_BlockAttendance,
                //                     FOMST_FixTimings = b.FOMST_FixTimings

                //                 }).OrderByDescending(t=>t.FOMST_Id).ToArray();  

                data.filldata = (from a in _FOContext.masterShifts
                                 from b in _FOContext.masterShiftsTimings
                                 where (a.FOMS_Id == b.FOMS_Id && b.FOMST_Id == id)
                                 select new MasterShiftsTimingsDTO
                                 {
                                     FOMS_Id = a.FOMS_Id,
                                     FOMS_ShiftName = a.FOMS_ShiftName,
                                     FOMST_Id = b.FOMST_Id,
                                     // FOHWDT_Id = b.FOHWDT_Id,
                                     // FOHTWD_HolidayWDType = c.FOHTWD_HolidayWDType,
                                     FOMS_ActiveFlg = a.FOMS_ActiveFlg,
                                     FOMST_FDWHrMin = b.FOMST_FDWHrMin,
                                     FOMST_HDWHrMin = b.FOMST_HDWHrMin,
                                     FOMST_IHalfLoginTime = b.FOMST_IHalfLoginTime,
                                     FOMST_IHalfLogoutTime = b.FOMST_IHalfLogoutTime,
                                     FOMST_IIHalfLoginTime = b.FOMST_IIHalfLoginTime,
                                     FOMST_IIHalfLogoutTime = b.FOMST_IIHalfLogoutTime,
                                     FOMST_DelayPerShiftHrMin = b.FOMST_DelayPerShiftHrMin,
                                     FOMST_EarlyPerShiftHrMin = b.FOMST_EarlyPerShiftHrMin,
                                     FOMST_LunchHoursDuration = b.FOMST_LunchHoursDuration,
                                     FOMST_BlockAttendance = b.FOMST_BlockAttendance,
                                     FOMST_FixTimings = b.FOMST_FixTimings

                                 }).OrderByDescending(t => t.FOMST_Id).ToArray();
            }
            else
            {
                data.filldata = (from a in _FOContext.masterShifts
                                 from b in _FOContext.masterShiftsTimings
                                 where (a.FOMS_Id == b.FOMS_Id && a.MI_Id == data.MI_Id)
                                 select new MasterShiftsTimingsDTO
                                 {
                                     FOMS_Id = a.FOMS_Id,
                                     FOMS_ShiftName = a.FOMS_ShiftName,
                                     FOMST_Id = b.FOMST_Id,
                                     //  FOHWDT_Id = b.FOHWDT_Id,
                                     //   FOHTWD_HolidayWDType = c.FOHTWD_HolidayWDType,
                                     FOMS_ActiveFlg = a.FOMS_ActiveFlg,
                                     FOMST_FDWHrMin = b.FOMST_FDWHrMin,
                                     FOMST_HDWHrMin = b.FOMST_HDWHrMin,
                                     FOMST_IHalfLoginTime = b.FOMST_IHalfLoginTime,
                                     FOMST_IHalfLogoutTime = b.FOMST_IHalfLogoutTime,
                                     FOMST_IIHalfLoginTime = b.FOMST_IIHalfLoginTime,
                                     FOMST_IIHalfLogoutTime = b.FOMST_IIHalfLogoutTime,
                                     FOMST_DelayPerShiftHrMin = b.FOMST_DelayPerShiftHrMin,
                                     FOMST_EarlyPerShiftHrMin = b.FOMST_EarlyPerShiftHrMin,
                                     FOMST_LunchHoursDuration = b.FOMST_LunchHoursDuration,
                                     FOMST_BlockAttendance = b.FOMST_BlockAttendance,
                                     FOMST_FixTimings = b.FOMST_FixTimings
                                 }).OrderByDescending(t => t.FOMST_Id).ToArray();
            }

        }
    }
}
