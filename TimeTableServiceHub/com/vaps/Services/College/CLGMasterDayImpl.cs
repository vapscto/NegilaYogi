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
    public class CLGMasterDayImpl : Interfaces.CLGMasterDayInterface
    {
        private static ConcurrentDictionary<string, CLGMasterDayDTO> _login =
             new ConcurrentDictionary<string, CLGMasterDayDTO>();

        public TTContext _TTContext;
        ILogger<CLGMasterDayImpl> _dataimpl;
        public DomainModelMsSqlServerContext _db;
        public CLGMasterDayImpl(TTContext academiccontext, ILogger<CLGMasterDayImpl> dataimpl, DomainModelMsSqlServerContext db)
        {
            _TTContext = academiccontext;
            _dataimpl = dataimpl;
            _db = db;
        }
        #region LOAD ALL DATA
        public CLGMasterDayDTO getalldetails(CLGMasterDayDTO data)
        {
            try
            {
                //FILL DROPDOWNS
                data.categorylist = _TTContext.TTMasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().ToArray();
                data.yearlist = _TTContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(yy => yy.ASMAY_Order).ToList().ToArray();

                data.courselist = _TTContext.MasterCourseDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.AMCO_ActiveFlag == true).ToList().ToArray();
                data.daydropdown = _TTContext.TT_Master_DayDMO.Where(u => u.MI_Id == data.MI_Id && u.TTMD_ActiveFlag == true).Distinct().ToArray();


                //MAPPED LIST  
                data.Daylist = (from TT_Master_Day in _TTContext.TT_Master_DayDMO
                                where (TT_Master_Day.MI_Id.Equals(data.MI_Id))
                                select new CLGMasterDayDTO
                                {
                                    TTMD_Id = TT_Master_Day.TTMD_Id,
                                    TTMD_DayName = TT_Master_Day.TTMD_DayName,
                                    TTMD_DayCode = TT_Master_Day.TTMD_DayCode,
                                    TTMD_ActiveFlag = TT_Master_Day.TTMD_ActiveFlag
                                }
                                      ).Distinct().ToArray();

                data.daymappedlist = (from a in _TTContext.CLGTT_Master_Day_CourseBranchDMO
                                      from b in _TTContext.TT_Master_DayDMO
                                      from c in _TTContext.ClgMasterBranchDMO
                                      from d in _TTContext.MasterCourseDMO
                                      from e in _TTContext.CLG_Adm_Master_SemesterDMO
                                      from f in _TTContext.AcademicYear
                                      where b.MI_Id == c.MI_Id && b.MI_Id == d.MI_Id && b.MI_Id == e.MI_Id && b.MI_Id == f.MI_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == f.ASMAY_Id && a.AMCO_Id == d.AMCO_Id && a.AMB_Id == c.AMB_Id && a.TTMD_Id == b.TTMD_Id && a.AMSE_Id == e.AMSE_Id
                                      select new CLGMasterDayDTO
                                      {
                                          TTMDC_Id = a.TTMDC_Id,
                                          ASMAY_Year = f.ASMAY_Year,
                                          TTMD_DayName = b.TTMD_DayName,
                                          AMB_BranchName = c.AMB_BranchName,
                                          AMCO_CourseName = d.AMCO_CourseName,
                                          AMSE_SEMName = e.AMSE_SEMName,
                                          TTMDC_ActiveFlag = a.TTMDC_ActiveFlag,


                                      }).ToArray();
        
                


            }
            catch (Exception ee)
            {
                // Console.WriteLine(ee.Message);
            }
            return data;

        }
        #endregion

        #region SAVE DAY
        public CLGMasterDayDTO saveday(CLGMasterDayDTO data)
        {
            
            try
            {
                if (data.TTMD_Id > 0)
                {
                    var res = _TTContext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && (t.TTMD_DayName.Trim().ToLower() == data.TTMD_DayName.Trim().ToLower() || t.TTMD_DayCode.Trim().ToLower() == data.TTMD_DayCode.Trim().ToLower()) && t.TTMD_Id != data.TTMD_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _TTContext.TT_Master_DayDMO.Single(t => t.MI_Id == data.MI_Id && t.TTMD_Id == data.TTMD_Id);
                        result.TTMD_DayCode = data.TTMD_DayCode.ToUpper();
                        result.TTMD_DayName = data.TTMD_DayName.ToUpper();
                        result.UpdatedDate = DateTime.Now;
                        result.TTMD_ActiveFlag = true;
                        _TTContext.Update(result);
                        var contactExists = _TTContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }
                }
                else
                {
                    var res = _TTContext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && (t.TTMD_DayName.Trim().ToLower() == data.TTMD_DayName.Trim().ToLower() || t.TTMD_DayCode.Trim().ToLower() == data.TTMD_DayCode.Trim().ToLower())).ToList();
                    if (res.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        long OID = 0;
                        var orderId = _TTContext.TT_Master_DayDMO.Where(f => f.MI_Id == data.MI_Id).ToList();
                        if (orderId.Count==0)
                        {
                            OID = 1;
                        }
                        else
                        {
                         long ooid= orderId.Select(r => r.Order_Id).Max();
                            OID = ooid + 1;
                        }

                        TT_Master_DayDMO obj = new TT_Master_DayDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.TTMD_DayName = data.TTMD_DayName.Trim().ToUpper();
                        obj.TTMD_DayCode = data.TTMD_DayCode.Trim().ToUpper(); ;
                        obj.Order_Id = OID;
                        obj.TTMD_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _TTContext.Add(obj);
                        var contactExists = _TTContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
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
        #endregion

        #region EDIT DAY
        public CLGMasterDayDTO editDay(CLGMasterDayDTO data)
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

        #region ACTIVE/DEACTIVE DAY
        public CLGMasterDayDTO daydeactive(CLGMasterDayDTO data)
        {
            try
            {

                if (data.TTMD_Id > 0)
                {
                    var result = _TTContext.TT_Master_DayDMO.Single(t => t.TTMD_Id.Equals(data.TTMD_Id));

                    var exist = _TTContext.CLGTT_Master_Day_CourseBranchDMO.Where(t => t.TTMD_Id == result.TTMD_Id && t.TTMDC_ActiveFlag == true).ToList();
                    if (exist.Count>0)
                    {
                        data.returnMsg = "E";
                    }
                    else
                    {
                        data.returnMsg = " ";
                        if (result.TTMD_ActiveFlag.Equals(true))
                        {
                            result.TTMD_ActiveFlag = false;
                        }
                        else
                        {
                            result.TTMD_ActiveFlag = true;
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        #endregion

        #region GET BRANCH
        public CLGMasterDayDTO getBranch(CLGMasterDayDTO data)
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
        public CLGMasterDayDTO savesemday(CLGMasterDayDTO data)
        {
            try
            {
                foreach (var item in data.semids)
                {

                    foreach (var dd in data.dayids)
                    {
                        var mappeddaylist = _TTContext.CLGTT_Master_Day_CourseBranchDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id == item.AMSE_Id && t.TTMD_Id== dd.TTMD_Id).ToList().ToList();

                        if (mappeddaylist.Count > 0)
                        {
                            foreach (var item1 in mappeddaylist)
                            {
                                _TTContext.Remove(item1);

                                var flag = _TTContext.SaveChanges();

                                if (flag == 0)
                                {
                                    data.returnMsg = "";
                                    return data;
                                }

                            }

                        }
                    }
                    //var mappeddaylist = _TTContext.CLGTT_Master_Day_CourseBranchDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.AMSE_Id == item.AMSE_Id).ToList().ToList();

                    //if (mappeddaylist.Count > 0)
                    //{
                    //    foreach (var item1 in mappeddaylist)
                    //    {
                    //        _TTContext.Remove(item1);

                    //        var flag = _TTContext.SaveChanges();

                    //        if (flag == 0)
                    //        {
                    //            data.returnMsg = "";
                    //            return data;
                    //        }

                    //    }

                    //}

                    foreach (var day in data.dayids)
                    {
                        CLGTT_Master_Day_CourseBranchDMO dayobj = new CLGTT_Master_Day_CourseBranchDMO();
                        dayobj.TTMD_Id = day.TTMD_Id;
                        dayobj.ASMAY_Id = data.ASMAY_Id;
                        dayobj.AMCO_Id = data.AMCO_Id;
                        dayobj.AMB_Id = data.AMB_Id;
                        dayobj.AMSE_Id = item.AMSE_Id;
                        dayobj.TTMDC_ActiveFlag = true;
                        dayobj.CreatedDate = DateTime.Now;
                        dayobj.UpdatedDate = DateTime.Now;
                        _TTContext.Add(dayobj);

                        var flag = _TTContext.SaveChanges();
                        if (flag == 1)
                        {
                            data.returnval =true;

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
                _dataimpl.LogError(ee.Message);
                _dataimpl.LogDebug(ee.Message);
            }

            return data;
        }
        #endregion

        #region ACTIVE/DEACTIVATE COURSE  DAY
        public CLGMasterDayDTO deactivecrsday(CLGMasterDayDTO data)
        {
            try
            {
               var result = _TTContext.CLGTT_Master_Day_CourseBranchDMO.Single(t => t.TTMDC_Id == data.TTMDC_Id);

                if (result.TTMDC_ActiveFlag.Equals(true))
                {
                    result.TTMDC_ActiveFlag = false;
                }
                else
                {
                    result.TTMDC_ActiveFlag = true;
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
            catch (Exception ee)
            {
                //data.message = "Sorry...You Can't delete this record because it is used in other operation.";
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        #endregion


        public CLGMasterDayDTO getorder(CLGMasterDayDTO data)
        {
            try
            {
                data.dayorderlist = _TTContext.TT_Master_DayDMO.Where(i => i.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }
        public CLGMasterDayDTO saveorder(CLGMasterDayDTO data)
        {
            try
            {
                int id = 0;
                for (int i = 0; i < data.ordeidss.Count(); i++)
                {
                    var reult = _TTContext.TT_Master_DayDMO.Single(t => t.MI_Id == data.MI_Id && t.TTMD_Id == data.ordeidss[i].TTMD_Id);
                    id = id + 1;

                    if (i == 0)
                    {
                        reult.Order_Id = id;
                    }
                    else
                    {
                        reult.Order_Id = id;
                    }
                    _TTContext.Update(reult);
                    var flag = _TTContext.SaveChanges();
                    if (flag > 0)
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

    }
}
