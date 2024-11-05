using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking.AssetTag;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Implementation
{
    public class AssetTagCheckInImpl : Interface.AssetTagCheckInInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetTagCheckInImpl> _logAT;
        public AssetTagCheckInImpl(AssetTrackingContext ATContext, ILogger<AssetTagCheckInImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public AssetTagCheckInDTO getloaddata(AssetTagCheckInDTO data)
        {
            try
            {
                data.get_locations = (from a in _ATContext.INV_Master_LocationDMO
                                      from b in _ATContext.INV_AssetTag_CheckOutDMO
                                      where (a.INVMLO_Id == b.INVMLO_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.INVATCO_ActiveFlg == true)
                                      select new AssetTagCheckInDTO
                                      {
                                          INVMLO_Id = a.INVMLO_Id,
                                          INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                      }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();

                data.get_employee = (from a in _ATContext.MasterEmployee
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                     select new AssetTagCheckInDTO
                                     {
                                         employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeOrder = a.HRME_EmployeeOrder,

                                     }).Distinct().OrderBy(h => h.HRME_EmployeeOrder).ToArray();

                data.get_ATcheckin = (from a in _ATContext.INV_AssetTag_CheckInDMO
                                      from b in _ATContext.INV_Asset_AssetTagDMO
                                      from c in _ATContext.INV_Master_StoreDMO
                                      from d in _ATContext.INV_Master_ItemDMO
                                      from e in _ATContext.INV_Master_LocationDMO
                                      where (a.INVAAT_Id == b.INVAAT_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && a.INVMLO_Id == e.INVMLO_Id
                                      && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.INVAAT_DisposedFlg == false)
                                      select new AssetTagCheckInDTO
                                      {
                                          INVATCI_Id = a.INVATCI_Id,
                                          INVAAT_Id = a.INVAAT_Id,
                                          INVMST_Id = a.INVMST_Id,
                                          INVMI_Id = a.INVMI_Id,
                                          INVMLO_Id = a.INVMLO_Id,
                                          INVMS_StoreName = c.INVMS_StoreName,
                                          INVMI_ItemName = d.INVMI_ItemName,
                                          INVAAT_AssetId = b.INVAAT_AssetId,
                                          INVMLO_LocationRoomName = e.INVMLO_LocationRoomName,
                                          INVATCI_CheckInDate = a.INVATCI_CheckInDate,
                                          INVATCI_ReceivedBy = a.INVATCI_ReceivedBy,
                                          INVATCI_CheckInRemarks = a.INVATCI_CheckInRemarks,
                                          INVAAT_AssetDescription = b.INVAAT_AssetDescription,
                                          INVAAT_ModelNo = b.INVAAT_ModelNo,
                                          INVAAT_SerialNo = b.INVAAT_SerialNo,
                                          INVAAT_ManufacturedDate = b.INVAAT_ManufacturedDate,
                                          INVAAT_PurchaseDate = b.INVAAT_PurchaseDate,
                                          INVATCI_ActiveFlg = a.INVATCI_ActiveFlg
                                      }).Distinct().OrderBy(m => m.INVATCI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Check In load Page:" + ex.Message);
            }
            return data;
        }
        public AssetTagCheckInDTO getstore(AssetTagCheckInDTO data)
        {
            try
            {
                data.get_store = (from a in _ATContext.INV_Master_StoreDMO
                                  from b in _ATContext.INV_AssetTag_CheckOutDMO
                                  where (a.INVMST_Id == b.INVMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.INVATCO_ActiveFlg == true && b.INVMLO_Id == data.INVMLO_Id)
                                  select new AssetTagCheckInDTO
                                  {
                                      INVMST_Id = a.INVMST_Id,
                                      INVMS_StoreName = a.INVMS_StoreName,
                                  }).Distinct().OrderBy(m => m.INVMST_Id).ToArray();

                var contactid = _ATContext.INV_Master_LocationDMO.Single(m => m.MI_Id == data.MI_Id && m.INVMLO_Id == data.INVMLO_Id).HRME_Id;

                if (contactid > 0)
                {
                    data.contactflag = "E";
                    data.get_contactperson = (from a in _ATContext.INV_Master_LocationDMO
                                              from b in _ATContext.INV_Master_SiteDMO
                                              from c in _ATContext.MasterEmployee
                                              where (a.INVMSI_Id == b.INVMSI_Id && a.HRME_Id == c.HRME_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id)
                                              select new CheckOutAssetsDTO
                                              {
                                                  INVMLO_Id = a.INVMLO_Id,
                                                  INVMSI_Id = b.INVMSI_Id,
                                                  INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                                  INVMLO_LocationRemarks = a.INVMLO_LocationRemarks,
                                                  INVMLO_InchargeName = a.INVMLO_InchargeName,
                                                  HRME_Id = a.HRME_Id,
                                                  employeename = ((c.HRME_EmployeeFirstName == null || c.HRME_EmployeeFirstName == "" ? "" : " " + c.HRME_EmployeeFirstName) + (c.HRME_EmployeeMiddleName == null || c.HRME_EmployeeMiddleName == "" || c.HRME_EmployeeMiddleName == "0" ? "" : " " + c.HRME_EmployeeMiddleName) + (c.HRME_EmployeeLastName == null || c.HRME_EmployeeLastName == "" || c.HRME_EmployeeLastName == "0" ? "" : " " + c.HRME_EmployeeLastName)).Trim(),

                                              }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();
                }
                else
                {
                    data.contactflag = "O";
                    data.get_contactperson = (from a in _ATContext.INV_Master_LocationDMO
                                              from b in _ATContext.INV_Master_SiteDMO

                                              where (a.INVMSI_Id == b.INVMSI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id)
                                              select new CheckOutAssetsDTO
                                              {
                                                  INVMLO_Id = a.INVMLO_Id,
                                                  INVMSI_Id = b.INVMSI_Id,
                                                  INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                                  INVMLO_LocationRemarks = a.INVMLO_LocationRemarks,
                                                  INVMLO_InchargeName = a.INVMLO_InchargeName,
                                              }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Check In get items:" + ex.Message);
            }
            return data;
        }
        public AssetTagCheckInDTO getitems(AssetTagCheckInDTO data)
        {
            try
            {
                data.get_items = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                                  from b in _ATContext.INV_Master_StoreDMO
                                  from c in _ATContext.INV_Master_ItemDMO
                                  from d in _ATContext.INV_Asset_AssetTagDMO
                                  where (a.INVMST_Id == b.INVMST_Id && a.INVAAT_Id == d.INVAAT_Id && a.INVMI_Id == c.INVMI_Id && a.MI_Id == b.MI_Id && c.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMST_Id == data.INVMST_Id)
                                  select new CheckOutAssetsDTO
                                  {
                                      INVMST_Id = a.INVMST_Id,
                                      INVMI_Id = a.INVMI_Id,
                                      INVMI_ItemName = c.INVMI_ItemName,
                                  }).Distinct().OrderByDescending(m => m.INVMI_ItemName).ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Checkout get items:" + ex.Message);
            }
            return data;
        }

        public AssetTagCheckInDTO getitemtagdata(AssetTagCheckInDTO data)
        {
            try
            {
                data.get_itemtagdata = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                                        from b in _ATContext.INV_Asset_AssetTagDMO
                                        from c in _ATContext.INV_Master_StoreDMO
                                        from d in _ATContext.INV_Master_ItemDMO
                                        from e in _ATContext.INV_Master_LocationDMO
                                        where (a.INVAAT_Id == b.INVAAT_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && a.INVMLO_Id == e.INVMLO_Id &&
                                        a.MI_Id == b.MI_Id && a.INVATCO_ActiveFlg == true && a.INVATCO_CheckInFlg == false && b.INVAAT_DisposedFlg == false && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMST_Id == data.INVMST_Id && a.INVMI_Id == data.INVMI_Id)
                                        select new AssetTagCheckInDTO
                                        {
                                            INVATCO_Id = a.INVATCO_Id,
                                            INVAAT_Id = a.INVAAT_Id,
                                            INVMST_Id = a.INVMST_Id,
                                            INVMI_Id = a.INVMI_Id,
                                            INVMLO_Id = a.INVMLO_Id,
                                            INVMLO_LocationRoomName = e.INVMLO_LocationRoomName,
                                            INVMS_StoreName = c.INVMS_StoreName,
                                            INVMI_ItemName = d.INVMI_ItemName,
                                            INVAAT_AssetId = b.INVAAT_AssetId,
                                            INVAAT_AssetDescription = b.INVAAT_AssetDescription,
                                            INVAAT_ModelNo = b.INVAAT_ModelNo,
                                            INVAAT_SerialNo = b.INVAAT_SerialNo
                                        }).Distinct().OrderByDescending(m => m.INVAAT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag check in get Data:" + ex.Message);
            }
            return data;
        }

        public AssetTagCheckInDTO savedata(AssetTagCheckInDTO data)
        {
            try
            {
                if (data.INVATCI_Id != 0)
                {
                    foreach (var t in data.tagckInArray)
                    {
                        DateTime? ckindate = null;

                        if (data.INVATCI_CheckInDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            ckindate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.INVATCI_CheckInDate), INDIAN_ZONE);
                        }

                        var result = _ATContext.INV_AssetTag_CheckInDMO.Single(a => a.MI_Id == data.MI_Id && a.INVATCI_Id == data.INVATCI_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVATCI_CheckInDate = ckindate;
                        result.HRME_Id = data.HRME_Id;
                        result.INVATCI_ReceivedBy = data.INVATCI_ReceivedBy;
                        result.INVATCI_ReceivedBy = t.INVATCI_CheckInRemarks;
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
                    foreach (var t in data.tagckInArray)
                    {
                        DateTime? ckindate = null;

                        if (data.INVATCI_CheckInDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            ckindate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.INVATCI_CheckInDate), INDIAN_ZONE);
                        }

                        INV_AssetTag_CheckInDMO itag = new INV_AssetTag_CheckInDMO();
                        itag.MI_Id = data.MI_Id;
                        itag.INVMST_Id = data.INVMST_Id;
                        itag.INVMI_Id = data.INVMI_Id;
                        itag.INVAAT_Id = t.INVAAT_Id;
                        itag.INVMLO_Id = data.INVMLO_Id;
                        itag.INVATCI_CheckInDate = ckindate;
                        itag.INVATCI_CheckInQty = data.INVATCI_CheckInQty;
                        itag.HRME_Id = data.HRME_Id;
                        itag.INVATCI_ReceivedBy = data.INVATCI_ReceivedBy;
                        itag.INVATCI_CheckInRemarks = t.INVATCI_CheckInRemarks;
                        itag.INVATCI_ActiveFlg = true;
                        itag.UpdatedDate = DateTime.Now;
                        itag.CreatedDate = DateTime.Now;
                        _ATContext.Add(itag);

                        var cotag = _ATContext.INV_AssetTag_CheckOutDMO.Single(a => a.MI_Id == data.MI_Id && a.INVAAT_Id == itag.INVAAT_Id);
                        cotag.INVATCO_CheckInFlg = true;
                        _ATContext.Update(cotag);

                        var tag = _ATContext.INV_Asset_AssetTagDMO.Single(a => a.MI_Id == data.MI_Id && a.INVAAT_Id == itag.INVAAT_Id);
                        tag.INVAAT_CheckOutFlg = false;
                        _ATContext.Update(tag);
                    }
                }
                var contextOB = _ATContext.SaveChanges();
                if (contextOB > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Asset tag check out savedata :" + ex.Message);
            }
            return data;
        }

        public AssetTagCheckInDTO deactive(AssetTagCheckInDTO data)
        {
            try
            {
                var result = _ATContext.INV_AssetTag_CheckInDMO.Single(t => t.INVATCI_Id == data.INVATCI_Id);

                if (result.INVATCI_ActiveFlg == true)
                {
                    result.INVATCI_ActiveFlg = false;
                }
                else if (result.INVATCI_ActiveFlg == false)
                {
                    result.INVATCI_ActiveFlg = true;
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
