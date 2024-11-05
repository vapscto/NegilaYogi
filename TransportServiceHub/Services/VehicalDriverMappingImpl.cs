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
    public class VehicalDriverMappingImpl : Interfaces.VehicalDriverMappingInterface
    {
        public TransportContext _context;
        public ILogger<VehicalDriverMappingDTO> _log;

        public VehicalDriverMappingImpl(ILogger<VehicalDriverMappingDTO> log, TransportContext context)
        {
            _log = log;
            _context = context;
        }

        public VehicalDriverMappingDTO getdata(int id)
        {
            VehicalDriverMappingDTO data = new VehicalDriverMappingDTO();
            data.MI_Id = id;
            try
            {
                data.sessiondata = _context.MsterSessionDMO.Where(s => s.MI_Id == data.MI_Id && s.TRMS_ActiveFlg == true).ToArray();
                data.vehicaldata = _context.Master_VehicleDMO.Where(b => b.MI_Id == data.MI_Id && b.TRMV_ActiveFlag == true).ToArray();
                data.driverdata = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_ActiveFlg == true).ToArray();

                data.savedata = (from a in _context.Master_VehicleDMO
                                 from b in _context.MasterDriverDMO
                                 from c in _context.VehicleDriver
                                 from d in _context.MsterSessionDMO
                                 from e in _context.VehicleDriverSessionDMO
                                 where (a.TRMV_Id == c.TRMV_Id && c.TRMD_Id == b.TRMD_Id  && c.TRVD_Id==e.TRVD_Id && e.TRMS_Id==d.TRMS_Id && c.MI_Id == data.MI_Id)
                                 select new VehicalDriverMappingDTO
                                 {
                                     TRVD_Id = c.TRVD_Id,
                                     TRMV_VehicleName =a.TRMV_VehicleName,
                                     TRMD_DriverName = b.TRMD_DriverName,
                                     TRMV_VehicleNo = a.TRMV_VehicleNo, 
                                     TRVD_Date=c.TRVD_Date,                                     
                                     TRMS_Id=e.TRMS_Id,
                                     TRMS_Flag=d.TRMS_Flag,
                                     TRMS_SessionName = d.TRMS_SessionName,
                                     TRVD_ActiveFlg =c.TRVD_ActiveFlg


                                 }).ToArray();



            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }


        public VehicalDriverMappingDTO savedata(VehicalDriverMappingDTO data)
        {
            try
            {
                if (data.TRVD_Id > 0)
                {
                    var check_duplicate_vehical_update = (from a in _context.VehicleDriver
                                                          from b in _context.VehicleDriverSessionDMO
                                                          where a.TRVD_Id == b.TRVD_Id && a.TRMD_Id == data.TRMD_Id && a.TRMV_Id == data.TRMV_Id && a.TRVD_Date.Date == data.TRVD_Date.Date && b.TRMS_Id == data.TRMS_Id && a.TRVD_Id != data.TRVD_Id select a).Distinct().ToList();


                    //var check_duplicate_driver_update = _context.VehicleDriver.Where(b => b.TRMD_Id.Equals(data.TRMD_Id) && b.TRVD_Id != data.TRVD_Id).ToList();
                    //var check_duplicate_session_update = _context.VehicleDriverSessionDMO.Where(c => c.TRMS_Id.Equals(data.TRMS_Id) && c.TRVD_Id != data.TRVD_Id).ToList();

                    if (check_duplicate_vehical_update.Count == 0 )
                    {
                        var result = _context.VehicleDriver.Single(a => a.TRVD_Id == data.TRVD_Id);
                        var result1 = _context.VehicleDriverSessionDMO.Single(a => a.TRVD_Id == data.TRVD_Id);
                        result.TRMV_Id = data.TRMV_Id;
                        result.TRMD_Id = data.TRMD_Id;
                        result.TRVD_Date = data.TRVD_Date;
                        result1.TRMS_Id = data.TRMS_Id;
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
                    var check_duplicate_vehical_update1 = (from a in _context.VehicleDriver
                                                          from b in _context.VehicleDriverSessionDMO
                                                          where a.TRVD_Id == b.TRVD_Id && a.TRMD_Id == data.TRMD_Id && a.TRMV_Id == data.TRMV_Id && a.TRVD_Date.Date == data.TRVD_Date.Date && b.TRMS_Id == data.TRMS_Id 
                                                           select a).Distinct().ToList();

                    if (check_duplicate_vehical_update1.Count == 0 )
                    {
                        VehicleDriver vehidriveDMO = new VehicleDriver();
                        VehicleDriverSessionDMO sess = new VehicleDriverSessionDMO();
                        vehidriveDMO.MI_Id = data.MI_Id;
                        vehidriveDMO.TRMV_Id = data.TRMV_Id;
                        vehidriveDMO.TRMD_Id = data.TRMD_Id;
                        vehidriveDMO.TRVD_Date = data.TRVD_Date;
                        vehidriveDMO.TRVD_ActiveFlg =true;
                        vehidriveDMO.CreatedDate = DateTime.Now;
                        vehidriveDMO.UpdatedDate = DateTime.Now;
                        _context.Add(vehidriveDMO);

                        sess.TRVD_Id = vehidriveDMO.TRVD_Id;
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


        public VehicalDriverMappingDTO activedeactive(VehicalDriverMappingDTO data)
        {
            try
            {
                //var check_Vehical_driver = (from a in _context.VehicleDriver
                //                            from b in _context.VehicleDriverSessionDMO

                //                            where (a.TRVD_Id == b.TRVD_Id && a.TRVD_ActiveFlg == true)
                //                            select new VehicalDriverMappingDTO
                //                            {
                //                                TRVD_Id = a.TRVD_Id
                //                            }).ToList();


                //if (check_Vehical_driver.Count > 0)
                //{
                //    data.message = "You Can Not Deactivate This Record Its Already Mapped";
                //}
                //else
                //{
                    var result = _context.VehicleDriver.Single(a => a.MI_Id == data.MI_Id && a.TRVD_Id == data.TRVD_Id);
                   
                    if (result.TRVD_ActiveFlg == false)
                    {
                        result.TRVD_ActiveFlg = true;
                        
                    }
                    else
                    {
                        result.TRVD_ActiveFlg = false;
                        
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



        public VehicalDriverMappingDTO editdata(VehicalDriverMappingDTO data)
        {
            try
            {
                data.editdata = (from a in _context.VehicleDriver
                                 from b in _context.VehicleDriverSessionDMO
                                 where (a.TRVD_Id == b.TRVD_Id && a.TRVD_Id == data.TRVD_Id)
                                 select new VehicalDriverMappingDTO
                                 {
                     TRVD_Id = a.TRVD_Id,
                     TRMD_Id = a.TRMD_Id,
                     TRMV_Id = a.TRMV_Id,
                     TRVD_Date = a.TRVD_Date,
                     TRMS_Id = b.TRMS_Id
                 }).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Master Driver editdata" + ex.Message);
            }
            return data;
        }

    }
}
