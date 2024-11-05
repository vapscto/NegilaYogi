using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class AT_MasterLocationImpl : Interface.AT_MasterLocationInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AT_MasterLocationImpl> _logAT;
        public AT_MasterLocationImpl(AssetTrackingContext ATContext, ILogger<AT_MasterLocationImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public AT_MasterLocationDTO getloaddata(AT_MasterLocationDTO data)
        {
            try
            {
                data.get_sites = _ATContext.INV_Master_SiteDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMSI_ActiveFlg == true).OrderBy(m => m.INVMSI_Id).ToArray();

                data.get_locations = (from a in _ATContext.INV_Master_LocationDMO
                                      from b in _ATContext.INV_Master_SiteDMO
                                      from c in _ATContext.MasterEmployee
                                      where (a.INVMSI_Id == b.INVMSI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                      select new AT_MasterLocationDTO
                                      {
                                          INVMLO_Id = a.INVMLO_Id,
                                          INVMSI_Id = b.INVMSI_Id,
                                          HRME_Id = a.HRME_Id,
                                          INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                          INVMLO_LocationRemarks = a.INVMLO_LocationRemarks,
                                          INVMLO_ActiveFlg = a.INVMLO_ActiveFlg,
                                          INVMSI_SiteBuildingName = b.INVMSI_SiteBuildingName,
                                          INVMLO_InchargeName = a.INVMLO_InchargeName,
                                          INVMSI_SiteRemarks = b.INVMSI_SiteRemarks,
                                      }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();

                data.get_employee = (from a in _ATContext.MasterEmployee
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                     select new AT_MasterLocationDTO
                                     {
                                         employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeOrder = a.HRME_EmployeeOrder,

                                     }).Distinct().OrderBy(h => h.HRME_EmployeeOrder).ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Master Location load Page:" + ex.Message);
            }
            return data;
        }

        public AT_MasterLocationDTO savedetails(AT_MasterLocationDTO data)
        {
            try
            {
                if (data.INVMLO_Id != 0)
                {
                    var res = _ATContext.INV_Master_LocationDMO.Where(t => t.INVMSI_Id == data.INVMSI_Id && t.INVMLO_LocationRoomName == data.INVMLO_LocationRoomName && t.MI_Id == data.MI_Id && t.INVMLO_Id != data.INVMLO_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ATContext.INV_Master_LocationDMO.Single(t => t.INVMLO_Id == data.INVMLO_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMSI_Id = data.INVMSI_Id;
                        result.INVMLO_LocationRoomName = data.INVMLO_LocationRoomName;
                        result.INVMLO_LocationRemarks = data.INVMLO_LocationRemarks;
                        result.HRME_Id = data.HRME_Id;
                        result.INVMLO_InchargeName = data.INVMLO_InchargeName;
                        result.INVMLO_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _ATContext.Update(result);

                        var contactExists = _ATContext.SaveChanges();
                        if (contactExists > 0)
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
                    var res = _ATContext.INV_Master_LocationDMO.Where(t => t.INVMSI_Id == data.INVMSI_Id && t.INVMLO_LocationRoomName == data.INVMLO_LocationRoomName && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_LocationDMO location = new INV_Master_LocationDMO();
                        location.MI_Id = data.MI_Id;
                        location.INVMSI_Id = data.INVMSI_Id;
                        location.INVMLO_LocationRoomName = data.INVMLO_LocationRoomName;
                        location.INVMLO_LocationRemarks = data.INVMLO_LocationRemarks;
                        location.HRME_Id = data.HRME_Id;
                        location.INVMLO_InchargeName = data.INVMLO_InchargeName;
                        location.INVMLO_ActiveFlg = true;
                        location.CreatedDate = DateTime.Now;
                        location.UpdatedDate = DateTime.Now;
                        _ATContext.Add(location);

                        var contactExists = _ATContext.SaveChanges();
                        if (contactExists > 0)
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
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Master Location savedata :" + ex.Message);
            }
            return data;
        }

        public AT_MasterLocationDTO deactive(AT_MasterLocationDTO data)
        {
            try
            {
                var result = _ATContext.INV_Master_LocationDMO.Single(t => t.INVMLO_Id == data.INVMLO_Id);

                if (result.INVMLO_ActiveFlg == true)
                {
                    result.INVMLO_ActiveFlg = false;
                }
                else if (result.INVMLO_ActiveFlg == false)
                {
                    result.INVMLO_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ATContext.Update(result);
                int returnval = _ATContext.SaveChanges();
                if (returnval > 0)
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
                Console.WriteLine(ee.Message);
            }

            return data;
        }



    }
}
