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
    public class VehicalDriverSubstituteImpl : Interfaces.VehicalDriverSubstituteInterface
    {
        public TransportContext _context;
        public ILogger<VehicalDriverSubstituteDTO> _log;

        public VehicalDriverSubstituteImpl(ILogger<VehicalDriverSubstituteDTO> log, TransportContext context)
        {
            _log = log;
            _context = context;
        }
        public VehicalDriverSubstituteDTO getdata(int id)
        {
            VehicalDriverSubstituteDTO data = new VehicalDriverSubstituteDTO();
            data.MI_Id = id;
            try
            {

                data.vehicaldata = _context.Master_VehicleDMO.Where(b => b.MI_Id == data.MI_Id && b.TRMV_ActiveFlag == true).ToArray();
                data.driverdata = _context.MasterDriverDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMD_ActiveFlg == true).ToArray();

                //data.vehicaldriverdata=(from a in _context.TR_Vehicle_Driver_SubstituteDMO
                //                        from b in _context.VehicleDriver

                //                        where(a.MI_Id==b.MI_Id)

                data.savedata = (from a in _context.TR_Vehicle_Driver_SubstituteDMO
                                     //from b in _context.MasterDriverDMO
                                     //from c in _context.VehicleDriver
                                 where (a.MI_Id == data.MI_Id)
                                 select new VehicalDriverSubstituteDTO
                                 {
                                     TRVDST_Id = a.TRVDST_Id,
                                     TRMV_Id = a.TRMV_Id,
                                     TRVDS_AbsentDriverId = a.TRVDST_AbsentDriverId,
                                     TRVDS_SubstituteDriverId = a.TRVDST_SubstituteDriverId,
                                     Absent_Driver = _context.MasterDriverDMO.Single(e => e.TRMD_Id == a.TRVDST_AbsentDriverId).TRMD_DriverName,
                                     Substitute_Driver = _context.MasterDriverDMO.Single(e => e.TRMD_Id == a.TRVDST_SubstituteDriverId).TRMD_DriverName,
                                     TRVDS_FromDate = a.TRVDST_FromDate,
                                     TRVDS_ToDate=a.TRVDST_ToDate,
                                     TRMV_VehicleName=_context.Master_VehicleDMO.Single(f=>f.TRMV_Id== a.TRMV_Id).TRMV_VehicleName
                                 }).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }
        public VehicalDriverSubstituteDTO get_driver_data(VehicalDriverSubstituteDTO data)
        {
            
            try {
                data.driverdata = (from a in _context.MasterDriverDMO
                                   from b in _context.VehicleDriver
                                   where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.TRMD_Id == b.TRMD_Id && b.TRMV_Id == data.TRMV_Id && a.TRMD_ActiveFlg == true && b.TRVD_ActiveFlg == true)
                                   select a).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }


        public VehicalDriverSubstituteDTO savedata(VehicalDriverSubstituteDTO data)
        {
            try
            {
                if (data.TRVDST_Id > 0)
                {
                    var check_duplicate_vehical_update = _context.TR_Vehicle_Driver_SubstituteDMO.Where(a => a.TRMV_Id.Equals(data.TRMV_Id) && a.TRMV_Id != data.TRMV_Id).ToList();
                    var check_duplicate_driver_update = _context.TR_Vehicle_Driver_SubstituteDMO.Where(b => b.TRVDST_AbsentDriverId.Equals(data.TRVDS_AbsentDriverId) && b.TRVDST_AbsentDriverId != data.TRVDS_AbsentDriverId).ToList();

                    if (check_duplicate_vehical_update.Count == 0 && check_duplicate_driver_update.Count == 0)
                    {
                        var result = _context.TR_Vehicle_Driver_SubstituteDMO.Single(a => a.TRVDST_Id == data.TRVDST_Id);
                        result.TRMV_Id = data.TRMV_Id;
                        result.TRVDST_AbsentDriverId = data.TRVDS_AbsentDriverId;
                        result.TRVDST_SubstituteDriverId = data.TRVDS_SubstituteDriverId;
                        result.TRVDST_FromDate = data.TRVDS_FromDate;
                        result.TRVDST_ToDate = data.TRVDS_ToDate;
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
                    var check_duplicate_vehical = _context.TR_Vehicle_Driver_SubstituteDMO.Where(a => a.TRMV_Id.Equals(data.TRMV_Id)).ToList();
                    var check_duplicate_driver = _context.TR_Vehicle_Driver_SubstituteDMO.Where(b => b.TRVDST_AbsentDriverId.Equals(data.TRVDS_AbsentDriverId)).ToList();



                    if (check_duplicate_vehical.Count == 0 && check_duplicate_driver.Count == 0)
                    {

                        VehicalDriverSubstituteDMO vehidrivesubDMO = new VehicalDriverSubstituteDMO();
                        vehidrivesubDMO.MI_Id = data.MI_Id;
                        vehidrivesubDMO.TRMV_Id = data.TRMV_Id;
                        vehidrivesubDMO.TRVDST_AbsentDriverId = data.TRVDS_AbsentDriverId;
                        vehidrivesubDMO.TRVDST_SubstituteDriverId = data.TRVDS_SubstituteDriverId;
                        vehidrivesubDMO.TRVDST_FromDate = data.TRVDS_FromDate;
                        vehidrivesubDMO.TRVDST_ToDate = data.TRVDS_ToDate;
                        vehidrivesubDMO.CreatedDate = DateTime.Now;
                        vehidrivesubDMO.UpdatedDate = DateTime.Now;
                        _context.Add(vehidrivesubDMO);

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

        public VehicalDriverSubstituteDTO editdata(VehicalDriverSubstituteDTO data)
        {
            try
            {
                data.editdata = (from a in _context.TR_Vehicle_Driver_SubstituteDMO
                                     //from b in _context.MasterDriverDMO
                                     //from c in _context.VehicleDriver
                                 where (a.MI_Id == data.MI_Id && a.TRVDST_Id==data.TRVDST_Id)
                                 select new VehicalDriverSubstituteDTO
                                 {
                                     TRVDST_Id = a.TRVDST_Id,
                                     TRMV_Id = a.TRMV_Id,
                                     TRVDS_AbsentDriverId = a.TRVDST_AbsentDriverId,
                                     TRVDS_SubstituteDriverId = a.TRVDST_SubstituteDriverId,
                                     Absent_Driver = _context.MasterDriverDMO.Single(e => e.TRMD_Id == a.TRVDST_AbsentDriverId).TRMD_DriverName,
                                     Substitute_Driver = _context.MasterDriverDMO.Single(e => e.TRMD_Id == a.TRVDST_SubstituteDriverId).TRMD_DriverName,
                                     TRVDS_FromDate = a.TRVDST_FromDate,
                                     TRVDS_ToDate=a.TRVDST_ToDate,
                                     TRMV_VehicleName = _context.Master_VehicleDMO.Single(f => f.TRMV_Id == a.TRMV_Id).TRMV_VehicleName
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
