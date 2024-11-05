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
    public class AssetTagDisposeImpl : Interface.AssetTagDisposeInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetTagDisposeImpl> _logAT;
        public AssetTagDisposeImpl(AssetTrackingContext ATContext, ILogger<AssetTagDisposeImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public AssetTagDisposeDTO getloaddata(AssetTagDisposeDTO data)
        {
            try
            {
                data.get_store = (from a in _ATContext.INV_Master_StoreDMO
                                  from b in _ATContext.INV_AssetTag_CheckOutDMO
                                  where (a.INVMST_Id == b.INVMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.INVATCO_ActiveFlg == true)
                                  select new AssetTagDisposeDTO
                                  {
                                      INVMST_Id = a.INVMST_Id,
                                      INVMS_StoreName = a.INVMS_StoreName,
                                  }).Distinct().OrderBy(m => m.INVMST_Id).ToArray();


                //data.get_ATcheckin = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                //                      from b in _ATContext.INV_Asset_AssetTagDMO
                //                      from c in _ATContext.INV_Master_StoreDMO
                //                      from d in _ATContext.INV_Master_ItemDMO
                //                      from e in _ATContext.INV_Master_LocationDMO
                //                      where (a.INVAAT_Id == b.INVAAT_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && a.INVMLO_Id == e.INVMLO_Id
                //                      && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                //                      select new AssetTagDisposeDTO
                //                      {
                //                          INVATCO_Id = a.INVATCO_Id,
                //                          INVAAT_Id = a.INVAAT_Id,
                //                          INVMST_Id = a.INVMST_Id,
                //                          INVMI_Id = a.INVMI_Id,
                //                          INVMLO_Id = a.INVMLO_Id,
                //                          INVMS_StoreName = c.INVMS_StoreName,
                //                          INVMI_ItemName = d.INVMI_ItemName,
                //                          INVAAT_AssetId = b.INVAAT_AssetId,
                //                          INVMLO_LocationRoomName = e.INVMLO_LocationRoomName,
                //                          INVATCO_CheckoutDate = a.INVATCO_CheckoutDate,
                //                          INVATCO_ReceivedBy = a.INVATCO_ReceivedBy,
                //                          INVATCO_CheckOutRemarks = a.INVATCO_CheckOutRemarks,
                //                          INVAAT_AssetDescription = b.INVAAT_AssetDescription,
                //                          INVAAT_ModelNo = b.INVAAT_ModelNo,
                //                          INVAAT_SerialNo = b.INVAAT_SerialNo,
                //                          INVAAT_ManufacturedDate = b.INVAAT_ManufacturedDate,
                //                          INVAAT_PurchaseDate = b.INVAAT_PurchaseDate,
                //                          INVATCO_ActiveFlg = a.INVATCO_ActiveFlg
                //                      }).Distinct().OrderBy(m => m.INVATCO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Dispose load Page:" + ex.Message);
            }
            return data;
        }
        public AssetTagDisposeDTO getlocation(AssetTagDisposeDTO data)
        {
            try
            {
                data.get_locations = (from a in _ATContext.INV_Master_LocationDMO
                                      from b in _ATContext.INV_AssetTag_CheckOutDMO
                                      where (a.INVMLO_Id == b.INVMLO_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.INVATCO_ActiveFlg == true && b.INVMST_Id == data.INVMST_Id)
                                      select new AssetTagDisposeDTO
                                      {
                                          INVMLO_Id = a.INVMLO_Id,
                                          INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                      }).Distinct().OrderBy(m => m.INVMLO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Dispose get items:" + ex.Message);
            }
            return data;
        }
        public AssetTagDisposeDTO getitems(AssetTagDisposeDTO data)
        {
            try
            {
                data.get_items = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                                  from b in _ATContext.INV_Master_StoreDMO
                                  from c in _ATContext.INV_Master_ItemDMO
                                  where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.MI_Id == b.MI_Id && c.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMST_Id == data.INVMST_Id)
                                  select new CheckOutAssetsDTO
                                  {
                                      INVMST_Id = a.INVMST_Id,
                                      INVMI_Id = a.INVMI_Id,
                                      INVMI_ItemName = c.INVMI_ItemName,
                                  }).Distinct().OrderByDescending(m => m.INVMI_ItemName).ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Dispose get items:" + ex.Message);
            }
            return data;
        }

        public AssetTagDisposeDTO getitemtagdata(AssetTagDisposeDTO data)
        {
            try
            {
                data.get_itemtagdata = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                                        from b in _ATContext.INV_Asset_AssetTagDMO
                                        from c in _ATContext.INV_Master_StoreDMO
                                        from d in _ATContext.INV_Master_ItemDMO
                                        from e in _ATContext.INV_Master_LocationDMO
                                        where (a.INVAAT_Id == b.INVAAT_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && a.INVMLO_Id == e.INVMLO_Id && a.INVATCO_CheckInFlg == false && a.MI_Id == b.MI_Id && a.INVATCO_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMST_Id == data.INVMST_Id && a.INVMI_Id == data.INVMI_Id)
                                        select new AssetTagDisposeDTO
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
                _logAT.LogInformation("Assets Tag Dispose get Data:" + ex.Message);
            }
            return data;
        }

        public AssetTagDisposeDTO savedata(AssetTagDisposeDTO data)
        {
            try
            {
                if (data.INVATDI_Id != 0)
                {
                    foreach (var d in data.tagDisposeArray)
                    {
                        DateTime? disposedate = null;

                        if (data.INVATDI_DisposedDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            disposedate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.INVATDI_DisposedDate), INDIAN_ZONE);
                        }

                        var result = _ATContext.INV_AssetTag_DisposeDMO.Single(a => a.MI_Id == data.MI_Id && a.INVATDI_Id == data.INVATDI_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVATDI_DisposedDate = disposedate;
                        result.INVATDI_DisposedRemarks = d.INVATDI_DisposedRemarks;
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
                    foreach (var d in data.tagDisposeArray)
                    {
                        DateTime? disposedate = null;

                        if (data.INVATDI_DisposedDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            disposedate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.INVATDI_DisposedDate), INDIAN_ZONE);
                        }

                        INV_AssetTag_DisposeDMO dtag = new INV_AssetTag_DisposeDMO();
                        dtag.MI_Id = data.MI_Id;
                        dtag.INVMST_Id = data.INVMST_Id;
                        dtag.INVMI_Id = data.INVMI_Id;
                        dtag.INVAAT_Id = d.INVAAT_Id;
                        dtag.INVMLO_Id = data.INVMLO_Id;
                        dtag.INVATDI_DisposedDate = disposedate;
                        dtag.INVATDI_DisposedQty = data.INVATDI_DisposedQty;
                        dtag.INVATDI_DisposedRemarks = d.INVATDI_DisposedRemarks;
                        dtag.INVATDI_ActiveFlg = true;
                        dtag.UpdatedDate = DateTime.Now;
                        dtag.CreatedDate = DateTime.Now;
                        _ATContext.Add(dtag);

                        var tag = _ATContext.INV_Asset_AssetTagDMO.Single(a => a.MI_Id == data.MI_Id && a.INVAAT_Id == dtag.INVAAT_Id);
                        tag.INVAAT_DisposedFlg = true;
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

        public AssetTagDisposeDTO deactive(AssetTagDisposeDTO data)
        {
            try
            {
                var result = _ATContext.INV_AssetTag_DisposeDMO.Single(t => t.INVATDI_Id == data.INVATDI_Id);
                var dtag = _ATContext.INV_AssetTag_DisposeDMO.Single(t => t.INVATDI_Id == data.INVATDI_Id).INVAAT_Id;
                if (result.INVATDI_ActiveFlg == true)
                {
                    result.INVATDI_ActiveFlg = false;

                    var tag = _ATContext.INV_Asset_AssetTagDMO.Single(a => a.MI_Id == data.MI_Id && a.INVAAT_Id == dtag);
                    if (tag.INVAAT_DisposedFlg == true)
                    {
                        tag.INVAAT_DisposedFlg = false;
                        _ATContext.Update(tag);
                    }

                }
                else if (result.INVATDI_ActiveFlg == false)
                {
                    result.INVATDI_ActiveFlg = true;
                    var tag = _ATContext.INV_Asset_AssetTagDMO.Single(a => a.MI_Id == data.MI_Id && a.INVAAT_Id == dtag);
                    if (tag.INVAAT_DisposedFlg == false)
                    {
                        tag.INVAAT_DisposedFlg = true;
                        _ATContext.Update(tag);
                    }
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
