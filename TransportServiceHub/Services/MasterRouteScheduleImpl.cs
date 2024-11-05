using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class MasterRouteScheduleImpl : Interfaces.MasterRouteScheduleInterface
    {
        public TransportContext _context;
        ILogger<MasterRouteScheduleImpl> _log;

        public MasterRouteScheduleImpl(TransportContext context, ILogger<MasterRouteScheduleImpl> log)
        {
            _context = context;
            _log = log;
        }

        public MasterRouteScheduleDTO getdata(int id)
        {
            MasterRouteScheduleDTO data = new MasterRouteScheduleDTO();
            try
            {
                //data.getroute = _context.MasterRouteDMO.Where(a => a.MI_Id == id && a.TRMR_ActiveFlg == true).ToArray();
                //data.getdata = (from a in _context.TR_Route_ScheduleDMO
                //                from b in _context.MasterRouteDMO
                //                where (a.TRMR_Id == b.TRMR_Id && a.MI_Id == id && b.MI_Id == id)
                //                select new MasterRouteScheduleDTO
                //                {
                //                    TRRSC_Id = a.TRRSC_Id,
                //                    TRRSC_ScheduleName = a.TRRSC_ScheduleName,
                //                    TRRSC_Date = a.TRRSC_Date,
                //                    TRMR_RouteName = b.TRMR_RouteName,
                //                    TRRSC_ActiveFlag = a.TRRSC_ActiveFlag
                //                }).OrderByDescending(a => a.TRRSC_Id).ToArray();



                data.getdata = (from a in _context.TR_Route_ScheduleDMO
                               
                                where ( a.MI_Id == id )
                                select new MasterRouteScheduleDTO
                                {    TRRSC_Id = a.TRRSC_Id,
                                    TRRSC_ScheduleName = a.TRRSC_ScheduleName,
                                    TRRSC_ActiveFlag = a.TRRSC_ActiveFlag
                                }).OrderByDescending(a => a.TRRSC_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in transport route schedule getdata " + ex.Message);
            }

            return data;
        }

        public MasterRouteScheduleDTO savedata(MasterRouteScheduleDTO data)
        {
            try
            {
                if (data.TRRSC_Id > 0)
                {
                    var check_schedulename = _context.TR_Route_ScheduleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRRSC_ScheduleName.Equals(data.TRRSC_ScheduleName) && a.TRRSC_Id != data.TRRSC_Id).ToList();
                    if (check_schedulename.Count == 0)
                    {
                        var result = _context.TR_Route_ScheduleDMO.Single(a => a.MI_Id == data.MI_Id && a.TRRSC_Id == data.TRRSC_Id);
                        result.TRRSC_ScheduleName = data.TRRSC_ScheduleName;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
                        int v = _context.SaveChanges();
                        if (v > 0)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var check_schedulename = _context.TR_Route_ScheduleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRRSC_ScheduleName.Equals(data.TRRSC_ScheduleName)).ToList();
                    if (check_schedulename.Count() == 0)
                    {
                        TR_Route_ScheduleDMO obj = new TR_Route_ScheduleDMO();
                        obj.TRRSC_ScheduleName = data.TRRSC_ScheduleName;
                        obj.MI_Id = data.MI_Id;
                        obj.TRRSC_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _context.Add(obj);
                        int n = _context.SaveChanges();
                        if (n > 0)
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
                    else
                    {
                        data.message = "Duplicate";
                    }
                }








                //if (data.TRRSC_Id > 0)
                //{
                //    var check_schedulename = _context.TR_Route_ScheduleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRRSC_ScheduleName.Equals(data.TRRSC_ScheduleName) &&
                //    a.TRMR_Id == data.TRMR_Id && a.TRRSC_Id != data.TRRSC_Id).ToList();
                //    if (check_schedulename.Count == 0)
                //    {
                //        var result = _context.TR_Route_ScheduleDMO.Single(a => a.MI_Id == data.MI_Id && a.TRRSC_Id == data.TRRSC_Id);
                //        result.TRRSC_ScheduleName = data.TRRSC_ScheduleName;
                //        result.UpdatedDate = DateTime.Now;
                //        result.TRRSC_Date = data.TRRSC_Date;
                //        _context.Update(result);
                //        int v = _context.SaveChanges();
                //        if (v > 0)
                //        {
                //            data.message = "Update";
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.message = "Update";
                //            data.returnval = false;
                //        }
                //    }
                //    else
                //    {
                //        data.message = "Duplicate";
                //    }
                //}
                //else
                //{
                //    var check_schedulename = _context.TR_Route_ScheduleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRRSC_ScheduleName.Equals(data.TRRSC_ScheduleName) &&
                //    a.TRMR_Id == data.TRMR_Id).ToList();
                //    if (check_schedulename.Count() == 0)
                //    {
                //        TR_Route_ScheduleDMO obj = new TR_Route_ScheduleDMO();
                //        obj.TRMR_Id = data.TRMR_Id;
                //        obj.TRRSC_ScheduleName = data.TRRSC_ScheduleName;
                //        obj.TRRSC_Date = data.TRRSC_Date;
                //        obj.MI_Id = data.MI_Id;
                //        obj.TRRSC_ActiveFlag = true;
                //        obj.CreatedDate = DateTime.Now;
                //        obj.UpdatedDate = DateTime.Now;
                //        _context.Add(obj);
                //        int n = _context.SaveChanges();
                //        if (n > 0)
                //        {
                //            data.returnval = true;
                //            data.message = "Save";
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //            data.message = "Save";
                //        }
                //    }
                //    else
                //    {
                //        data.message = "Duplicate";
                //    }
                //}
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in transport route schedule savedata " + ex.Message);
            }
            return data;
        }

        public MasterRouteScheduleDTO edit(MasterRouteScheduleDTO data)
        {
            try
            {
                data.geteditdata = _context.TR_Route_ScheduleDMO.Where(a => a.MI_Id == data.MI_Id && a.TRRSC_Id == data.TRRSC_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in transport route schedule edit " + ex.Message);
            }
            return data;
        }
        public MasterRouteScheduleDTO activedeactive(MasterRouteScheduleDTO data)
        {
            try
            {
                var checkidused = (from a in _context.TR_Student_RouteDMO
                                   from b in _context.TR_Route_ScheduleDMO
                                   where (a.TRSR_PickupSchedule == b.TRRSC_Id && a.MI_Id == data.MI_Id && a.TRSR_PickupSchedule == data.TRRSC_Id && b.TRRSC_ActiveFlag==true)
                                   select new MasterRouteScheduleDTO
                                   {
                                       TRRSC_Id = a.TRSR_PickupSchedule
                                   }).ToList();

                var checkidused1 = (from a in _context.TR_Student_RouteDMO
                                    from b in _context.TR_Route_ScheduleDMO
                                    where (a.TRSR_DropSchedule == b.TRRSC_Id && a.MI_Id == data.MI_Id && a.TRSR_DropSchedule == data.TRRSC_Id && b.TRRSC_ActiveFlag == true)
                                    select new MasterRouteScheduleDTO
                                    {
                                        TRRSC_Id = a.TRSR_PickupSchedule
                                    }).ToList();

                var checkidused2 = (from a in _context.TR_Route_Sch_SessionDMO
                                    from b in _context.TR_Route_ScheduleDMO
                                    where (a.TRRSC_Id == b.TRRSC_Id && a.TRRSC_Id == data.TRRSC_Id && b.TRRSC_ActiveFlag == true)
                                    select new MasterRouteScheduleDTO
                                    {
                                        TRRSC_Id = a.TRRSC_Id
                                    }).ToList();


                if (checkidused.Count == 0 && checkidused1.Count == 0 && checkidused2.Count == 0)
                {
                    var result = _context.TR_Route_ScheduleDMO.Single(a => a.MI_Id == data.MI_Id && a.TRRSC_Id == data.TRRSC_Id);
                    if (result.TRRSC_ActiveFlag == true)
                    {
                        result.TRRSC_ActiveFlag = false;
                    }
                    else
                    {
                        result.TRRSC_ActiveFlag = true;
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
                else
                {
                    data.message = "Mapped";
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Error in transport route schedule activedeactive " + ex.Message);
            }
            return data;
        }
    }
}
