using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.HRMS;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransportServiceHub.Interfaces;


namespace TransportServiceHub.Services
{
    public class VehicalRouteMappingImpl : Interfaces.VehicalRouteMappingInterface
    {
        public TransportContext _context;
        public ILogger<VehicalRouteMappingDTO> _log;

        public VehicalRouteMappingImpl(ILogger<VehicalRouteMappingDTO> log, TransportContext context)
        {
            _log = log;
            _context = context;
        }

        public VehicalRouteMappingDTO getdata(int id)
        {
            VehicalRouteMappingDTO data = new VehicalRouteMappingDTO();
            data.MI_Id = id;
            try
            {

                List<Master_VehicleDMO> lorg = new List<Master_VehicleDMO>();
                lorg = _context.Master_VehicleDMO.Where(t => t.MI_Id.Equals(data.MI_Id) && t.TRMV_ActiveFlag == true).ToList();
                data.vehicaldata = lorg.ToArray();
                data.routedata = _context.MasterRouteDMO.Where(r => r.MI_Id == data.MI_Id && r.TRMR_ActiveFlg == true).ToArray();
                data.sessiondata = _context.MsterSessionDMO.Where(s => s.MI_Id == data.MI_Id && s.TRMS_ActiveFlg == true).ToArray();

                data.savedata = (from a in _context.VehicleRouteDMo
                                 from b in _context.VehicleRouteSessionDMO
                                 from c in _context.Master_VehicleDMO
                                 from d in _context.MasterRouteDMO
                                 from e in _context.MsterSessionDMO
                                 where (a.TRVR_Id == b.TRVR_Id && a.TRMV_Id == c.TRMV_Id && a.TRMR_Id == d.TRMR_Id
                                  && b.TRMS_Id == e.TRMS_Id && a.MI_Id == data.MI_Id)
                                 select new VehicalRouteMappingDTO
                                 {
                                     TRVR_Id = a.TRVR_Id,
                                     TRMV_VehicleName = c.TRMV_VehicleName,
                                     TRMV_VehicleNo = c.TRMV_VehicleNo,
                                     TRMR_RouteName = d.TRMR_RouteName,
                                     TRMS_SessionName = e.TRMS_SessionName,
                                     TRMS_Flag = e.TRMS_Flag,
                                     TRVR_Date = a.TRVR_Date,
                                     TRVR_ActiveFlg = a.TRVR_ActiveFlg
                                 }).ToArray();


            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }



        public VehicalRouteMappingDTO savedata(VehicalRouteMappingDTO data)
        {
            try
            {
                if (data.TRVR_Id > 0)
                {
                    var check_duplicate_vehical_update = _context.VehicleRouteDMo.Where(a => a.TRMV_Id.Equals(data.TRMV_Id) && a.TRVR_Id != data.TRVR_Id).ToList();
                    var check_duplicate_route_update = _context.VehicleRouteDMo.Where(b => b.TRMR_Id.Equals(data.TRMR_Id) && b.TRVR_Id != data.TRVR_Id).ToList();
                    var check_duplicate_session_update = _context.VehicleRouteSessionDMO.Where(c => c.TRMS_Id.Equals(data.TRMS_Id) && c.TRVR_Id != data.TRVR_Id).ToList();

                    if (check_duplicate_vehical_update.Count == 0 || check_duplicate_route_update.Count == 0 || check_duplicate_session_update.Count == 0)
                    {
                        var result = _context.VehicleRouteDMo.Single(a => a.TRVR_Id == data.TRVR_Id);
                        var result1 = _context.VehicleRouteSessionDMO.Single(b => b.TRVR_Id == data.TRVR_Id);
                        result.TRMV_Id = data.TRMV_Id;
                        result.TRMR_Id = data.TRMR_Id;
                        result1.TRMS_Id = data.TRMS_Id;
                        result.TRVR_Date = data.TRVR_Date;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Update";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrunval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var check_duplicate_vehical = _context.VehicleRouteDMo.Where(a => a.TRMV_Id.Equals(data.TRMV_Id)).ToList();
                    var check_duplicate_route = _context.VehicleRouteDMo.Where(b => b.TRMR_Id.Equals(data.TRMR_Id)).ToList();
                    var check_duplicate_session = _context.VehicleRouteSessionDMO.Where(c => c.TRMS_Id.Equals(data.TRMS_Id)).ToList();

                    if (check_duplicate_vehical.Count == 0 || check_duplicate_route.Count == 0 || check_duplicate_session.Count == 0)
                    {
                        VehicleRouteDMo vehiroute = new VehicleRouteDMo();
                        VehicleRouteSessionDMO sess = new VehicleRouteSessionDMO();
                        vehiroute.MI_Id = data.MI_Id;
                        vehiroute.TRMV_Id = data.TRMV_Id;
                        vehiroute.TRMR_Id = data.TRMR_Id;
                        vehiroute.TRVR_Date = data.TRVR_Date;
                        vehiroute.TRVR_ActiveFlg = true;
                        vehiroute.CreatedDate = DateTime.Now;
                        vehiroute.UpdatedDate = DateTime.Now;
                        vehiroute.TRVR_ActiveFlg = true;
                        _context.Add(vehiroute);

                        sess.TRVR_Id = vehiroute.TRVR_Id;
                        sess.TRMS_Id = data.TRMS_Id;                        
                        sess.CreatedDate = DateTime.Now;
                        sess.UpdatedDate = DateTime.Now;

                        _context.Add(sess);

                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Add";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.retrunval = false;
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
                _log.LogInformation("Transport Error Master Vehicle savedata" + ex.Message);
            }
            return data;
        }



        public VehicalRouteMappingDTO editdata(VehicalRouteMappingDTO data)
        {
            try
            {
                data.editdata = (from a in _context.VehicleRouteDMo
                                 from b in _context.VehicleRouteSessionDMO
                                 where (a.TRVR_Id == b.TRVR_Id && a.TRVR_Id == data.TRVR_Id)
                                 select new VehicalRouteMappingDTO
                                 {
                                     TRVR_Id = a.TRVR_Id,
                                     TRMR_Id = a.TRMR_Id,
                                     TRMV_Id = a.TRMV_Id,
                                     TRVR_Date = a.TRVR_Date,
                                     TRMS_Id = b.TRMS_Id
                                 }).ToArray();

                //   data.child_edit_data = _context.VehicleRouteSessionDMO.Where(a => a.TRVR_Id == data.TRVR_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Driver editdata" + ex.Message);
            }
            return data;
        }




        public VehicalRouteMappingDTO activedeactive(VehicalRouteMappingDTO data)
        {
            try
            {
                //var check_Vehical_Route = (from a in _context.VehicleRouteDMo
                //                           from b in _context.VehicleRouteSessionDMO

                //                           where (a.TRVR_Id == b.TRVR_Id && a.TRVR_Id == data.TRVR_Id && a.TRVR_ActiveFlg == true)
                //                           select new VehicalRouteMappingDTO
                //                           {
                //                               TRVR_Id = a.TRVR_Id
                //                           }).ToList();




                //if (check_Vehical_Route.Count > 0)
                //{
                //    data.message = "You Can Not Deactivate This Record Its Already Mapped";
                //}
                //else
                //{
                    var result = _context.VehicleRouteDMo.Single(a => a.MI_Id == data.MI_Id && a.TRVR_Id == data.TRVR_Id);

                    if (result.TRVR_ActiveFlg == false)
                    {
                        result.TRVR_ActiveFlg = true;

                    }
                    else
                    {
                        result.TRVR_ActiveFlg = false;

                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.retrunval = true;
                    }
                    else
                    {
                        data.retrunval = false;
                    }
                //}
            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _log.LogInformation("Transport Error Master Area activedeactive" + ex.Message);
            }
            return data;
        }




    }
}
