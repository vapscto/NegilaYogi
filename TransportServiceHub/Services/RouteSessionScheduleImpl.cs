using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class RouteSessionScheduleImpl : Interfaces.RouteSessionScheduleInterface
    {
        public TransportContext _context;
        ILogger<RouteSessionScheduleImpl> _log;

        public RouteSessionScheduleImpl(TransportContext context, ILogger<RouteSessionScheduleImpl> log)
        {
            _context = context;
            _log = log;
        }

        public RouteSessionScheduleDTO getdata(int id)
        {
            RouteSessionScheduleDTO data = new RouteSessionScheduleDTO();
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id && y.Is_Active == true ).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.getroute = _context.MasterRouteDMO.Where(a => a.MI_Id == id && a.TRMR_ActiveFlg == true).ToArray();
                data.sessionlist = _context.MsterSessionDMO.Where(a => a.MI_Id == id && a.TRMS_ActiveFlg == true).ToArray();

                data.schdulelist = _context.TR_Route_ScheduleDMO.Where(a => a.MI_Id == id && a.TRRSC_ActiveFlag == true).ToArray();



                data.getdata = (from a in _context.TR_Route_Sch_SessionDMO
                               // from b in _context.TR_Route_Sch_Sess_LocationDMO
                                from c in _context.TR_Route_ScheduleDMO
                                from  d in _context.MsterSessionDMO
                                from e in _context.MasterRouteDMO
                                from f in _context.AcademicYear
                               // from g in _context.MasterLocationDMO
                                where (f.MI_Id==id && f.MI_Id==d.MI_Id && f.MI_Id==c.MI_Id && f.MI_Id==e.MI_Id  && f.MI_Id==d.MI_Id && a.ASMAY_Id==f.ASMAY_Id && a.TRMR_Id==e.TRMR_Id && a.TRMS_Id==d.TRMS_Id && d.TRMS_ActiveFlg==true &&  c.TRRSC_Id==a.TRRSC_Id && c.TRRSC_ActiveFlag==true  && e.TRMR_ActiveFlg==true  
                                )
                                select new RouteSessionScheduleDTO
                                {
                                    TRRSCS_Id = a.TRRSCS_Id,
                                    TRRSC_ScheduleName = c.TRRSC_ScheduleName,
                                    TRMS_SessionName=d.TRMS_SessionName,
                                    TRMR_RouteName=e.TRMR_RouteName,
                                    ASMAY_Year=f.ASMAY_Year,
                                    TRRSCS_Day= a.TRRSCS_Day,
                                    TRRSC_Date=a.TRRSCS_Date,
                                    TRRSCS_FromTime=a.TRRSCS_FromTime,
                                    TRRSCS_ToTime=a.TRRSCS_ToTime,
                                    TRRSCS_ActiveFlg=a.TRRSCS_ActiveFlg
                                }).OrderByDescending(a => a.TRRSCS_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in transport route schedule getdata " + ex.Message);
            }

            return data;
        }

        public RouteSessionScheduleDTO savedata(RouteSessionScheduleDTO data)
        {
            try
           {
                if (data.TRRSCS_Id > 0)
                {
                    var check_schedulename = _context.TR_Route_Sch_SessionDMO.Where(a => a.TRMR_Id == data.TRMR_Id && a.TRMS_Id == data.TRMS_Id && a.TRRSCS_Day == data.TRRSCS_Day && a.TRRSCS_Date == data.TRRSC_Date && a.TRRSC_Id == data.TRRSCS_Id && a.TRRSCS_Id != data.TRRSCS_Id).ToList();
                    if (check_schedulename.Count == 0)
                    {

                        var rmres = _context.TR_Route_Sch_Sess_LocationDMO.Where(gg => gg.TRRSCS_Id == data.TRRSCS_Id).ToList();

                        if (rmres.Count>0)
                        {
                            foreach (var item in rmres)
                            {
                                _context.Remove(item);
                            }
                        }
                        
                        if (data.weekdays.Length > 0)
                        {
                            foreach (var item in data.weekdays)
                            {

                                var result = _context.TR_Route_Sch_SessionDMO.Single(a => a.TRRSCS_Id == data.TRRSCS_Id);

                                result.TRRSC_Id = data.TRRSC_Id;
                                result.TRMS_Id = data.TRMS_Id;
                                result.TRRSCS_Day = item.type;
                                result.TRRSCS_FromTime = data.TRRSCS_FromTime;
                                result.TRRSCS_ToTime = data.TRRSCS_ToTime;
                                result.TRMR_Id = data.TRMR_Id;
                                result.TRRSCS_Date = data.TRRSC_Date;
                                result.ASMAY_Id = data.ASMAY_Id;
                                result.UpdatedDate = DateTime.Now;
                                result.TRRSCS_ActiveFlg = true;
                                _context.Update(result);
                                int n2 = _context.SaveChanges();
                                if (data.loclixt.Length > 0)
                                {
                                    var row_cnt = _context.TR_Route_Sch_Sess_LocationDMO.Where(t => t.TRRSCS_Id == data.TRRSCS_Id).ToList().Count;
                                    var nrow_cnt = 0;
                                    foreach (var item1 in data.loclixt)
                                    {
                                        if (nrow_cnt == 0)
                                        {
                                            nrow_cnt = row_cnt + 1;
                                        }
                                        else
                                        {
                                            nrow_cnt = nrow_cnt + 1;
                                        }
                                        TR_Route_Sch_Sess_LocationDMO obj1 = new TR_Route_Sch_Sess_LocationDMO();
                                        obj1.TRRSCS_Id = data.TRRSCS_Id;
                                        obj1.TRML_Id = item1.TRML_Id;
                                        obj1.TRRSCSL_ArrivalTime = item1.TRRSCSL_ArrivalTime;
                                        obj1.TRRSCSL_DepartureTime = item1.TRRSCSL_DepartureTime;
                                        obj1.TRRSCSL_Order = nrow_cnt;
                                        obj1.TRRSCSL_ActiveFlg = true;
                                        obj1.CreatedDate = DateTime.Now;
                                        obj1.UpdatedDate = DateTime.Now;
                                        _context.Add(obj1);

                                    }
                                    int n24 = _context.SaveChanges();
                                    if (n24 > 0)
                                    {
                                        data.returnval = true;
                                        data.message = "Update";
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                        data.message = "Update";
                                    }
                                }

                            }

                        }
                       
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var check_schedulename = _context.TR_Route_Sch_SessionDMO.Where(a => a.TRMR_Id == data.TRMR_Id && a.TRMS_Id == data.TRMS_Id && a.TRRSCS_Day == data.TRRSCS_Day && a.TRRSCS_Date == data.TRRSC_Date && a.TRRSC_Id == data.TRRSCS_Id).ToList();
                    if (check_schedulename.Count() == 0)
                    {
                        if (data.weekdays.Length > 0)
                        {
                            foreach (var item in data.weekdays)
                            {

                                TR_Route_Sch_SessionDMO obj = new TR_Route_Sch_SessionDMO();

                                obj.TRRSC_Id = data.TRRSC_Id;
                                obj.TRMS_Id = data.TRMS_Id;
                                obj.TRRSCS_Day = item.type;
                                obj.TRRSCS_FromTime = data.TRRSCS_FromTime;
                                obj.TRRSCS_ToTime = data.TRRSCS_ToTime;
                                obj.TRMR_Id = data.TRMR_Id;
                                obj.TRRSCS_Date = data.TRRSC_Date;
                                obj.ASMAY_Id = data.ASMAY_Id;
                                obj.TRRSCS_ActiveFlg = true;
                                obj.CreatedDate = DateTime.Now;
                                obj.UpdatedDate = DateTime.Now;
                                _context.Add(obj);
                                int n2 = _context.SaveChanges();
                                if (data.loclixt.Length > 0)
                                {
                                    var row_cnt = _context.TR_Route_Sch_Sess_LocationDMO.Where(t => t.TRRSCS_Id == obj.TRRSCS_Id).ToList().Count;
                                    var nrow_cnt = 0;
                                    foreach (var item1 in data.loclixt)
                                    {
                                        if (nrow_cnt == 0)
                                        {
                                            nrow_cnt = row_cnt + 1;
                                        }
                                        else
                                        {
                                            nrow_cnt = nrow_cnt + 1;
                                        }
                                        TR_Route_Sch_Sess_LocationDMO obj1 = new TR_Route_Sch_Sess_LocationDMO();
                                        obj1.TRRSCS_Id = obj.TRRSCS_Id;
                                        obj1.TRML_Id = item1.TRML_Id;
                                        obj1.TRRSCSL_ArrivalTime = item1.TRRSCSL_ArrivalTime;
                                        obj1.TRRSCSL_DepartureTime = item1.TRRSCSL_DepartureTime;
                                        obj1.TRRSCSL_Order = nrow_cnt;
                                        obj1.TRRSCSL_ActiveFlg = true;
                                        obj1.CreatedDate = DateTime.Now;
                                        obj1.UpdatedDate = DateTime.Now;
                                        _context.Add(obj1);

                                    }
                                    int n24 = _context.SaveChanges();
                                    if (n24 > 0)
                                    {
                                        data.returnval = true;
                                        data.message = "Save";
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                        data.message = "Save";
                                    }
                                }

                            }

                        }


                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }





            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in transport route schedule savedata " + ex.Message);
            }
            return data;
        }




        public RouteSessionScheduleDTO edit(RouteSessionScheduleDTO data)
        {
            try
            {
                data.geteditdata = (from a in _context.TR_Route_Sch_SessionDMO
                                    from b in _context.TR_Route_Sch_Sess_LocationDMO
                                    where a.TRRSCS_Id == data.TRRSCS_Id && a.TRRSCS_Id==b.TRRSCS_Id
                                    select new RouteSessionScheduleDTO
                                    {
                                        TRRSCS_Id = a.TRRSCS_Id,
                                        TRRSC_Id = a.TRRSC_Id,
                                        TRMS_Id = a.TRMS_Id,
                                        TRMR_Id = a.TRMR_Id,
                                        TRRSCS_Day = a.TRRSCS_Day,
                                        TRRSC_Date = a.TRRSCS_Date,
                                        TRRSCS_FromTime = a.TRRSCS_FromTime,
                                        TRRSCS_ToTime = a.TRRSCS_ToTime,
                                        TRML_Id = b.TRML_Id,
                                        TRRSCSL_ArrivalTime = b.TRRSCSL_ArrivalTime,
                                        TRRSCSL_DepartureTime = b.TRRSCSL_DepartureTime,
                                        ASMAY_Id=a.ASMAY_Id

                                    }).Distinct().OrderBy(t => t.TRRSCSL_Order).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in transport route schedule edit " + ex.Message);
            }
            return data;
        }
        public RouteSessionScheduleDTO showlocationGrid(RouteSessionScheduleDTO data)
        {
            try
            {
                data.getpopupdata = (from a in _context.TR_Route_Sch_Sess_LocationDMO
                                     from b in _context.MasterLocationDMO
                                     where a.TRRSCS_Id == data.TRRSCS_Id && a.TRML_Id == b.TRML_Id
                                     select new RouteSessionScheduleDTO
                                     {
                                         TRML_LocationName = b.TRML_LocationName,
                                         TRRSCSL_Id = a.TRRSCSL_Id,
                                         TRRSCSL_ArrivalTime = a.TRRSCSL_ArrivalTime,
                                         TRRSCSL_DepartureTime = a.TRRSCSL_DepartureTime,
                                         TRRSCSL_ActiveFlg = a.TRRSCSL_ActiveFlg,
                                         TRRSCS_Id=a.TRRSCS_Id

                                     }).Distinct().OrderBy(t=>t.TRRSCSL_Order).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in transport route schedule edit " + ex.Message);
            }
            return data;
        }
        public RouteSessionScheduleDTO routechange(RouteSessionScheduleDTO data)
        {
            try
            {
                data.getlocationlist = (from a in _context.Route_Location
                                        from b in _context.MasterLocationDMO
                                        where a.MI_Id == data.MI_Id && a.TRML_Id == b.TRML_Id && a.TRMR_Id == data.TRMR_Id && b.TRML_ActiveFlg == true
                                        select new RouteSessionScheduleDTO
                                        {
                                            TRML_Id = a.TRML_Id,
                                            TRML_LocationName = b.TRML_LocationName,
                                            TRMRL_Order = a.TRMRL_Order
                                        }).Distinct().OrderBy(x => x.TRMRL_Order).ToArray();


            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in transport route schedule edit " + ex.Message);
            }
            return data;
        }
        public RouteSessionScheduleDTO activedeactive(RouteSessionScheduleDTO data)
        {
            try
            {

                //if (checkidused.Count == 0 && checkidused1.Count == 0 && checkidused2.Count == 0)
                //{
                if (data.TRRSCS_Id>0)
                {
                    var result = _context.TR_Route_Sch_SessionDMO.Single(a => a.TRRSCS_Id == data.TRRSCS_Id);
                    if (result.TRRSCS_ActiveFlg == true)
                    {
                        result.TRRSCS_ActiveFlg = false;
                    }
                    else
                    {
                        result.TRRSCS_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int k = _context.SaveChanges();
                    if (k > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else if (data.TRRSCSL_Id>0)
                {
                    var result = _context.TR_Route_Sch_Sess_LocationDMO.Single(a => a.TRRSCSL_Id == data.TRRSCSL_Id);
                    if (result.TRRSCSL_ActiveFlg == true)
                    {
                        result.TRRSCSL_ActiveFlg = false;
                    }
                    else
                    {
                        result.TRRSCSL_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int k = _context.SaveChanges();
                    if (k > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
               
                   
                //}
                //else
                //{
                //    data.message = "Mapped";
                //}
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in transport route schedule activedeactive " + ex.Message);
            }
            return data;
        }
    }
}
