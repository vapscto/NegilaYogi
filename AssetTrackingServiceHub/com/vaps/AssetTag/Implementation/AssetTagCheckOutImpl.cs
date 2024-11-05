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
    public class AssetTagCheckOutImpl : Interface.AssetTagCheckOutInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetTagCheckOutImpl> _logAT;
        public AssetTagCheckOutImpl(AssetTrackingContext ATContext, ILogger<AssetTagCheckOutImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public AssetTagCheckOutDTO getloaddata(AssetTagCheckOutDTO data)
        {
            try
            {
                data.get_store = (from a in _ATContext.INV_StockDMO
                                  from b in _ATContext.INV_Master_StoreDMO
                                  from c in _ATContext.INV_Asset_AssetTagDMO
                                  where (a.INVMST_Id == b.INVMST_Id && a.INVMST_Id == c.INVMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                  select new AssetTagCheckOutDTO
                                  {
                                      INVMST_Id = a.INVMST_Id,
                                      INVMS_StoreName = b.INVMS_StoreName,
                                  }).Distinct().OrderBy(m => m.INVMST_Id).ToArray();

                data.get_locations = _ATContext.INV_Master_LocationDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMLO_ActiveFlg == true).OrderBy(m => m.INVMLO_Id).ToArray();

                data.get_employee = (from a in _ATContext.MasterEmployee
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                     select new AssetTagCheckOutDTO
                                     {
                                         employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeOrder = a.HRME_EmployeeOrder,

                                     }).Distinct().OrderBy(h => h.HRME_EmployeeOrder).ToArray();

                data.get_ATcheckout = (from a in _ATContext.INV_AssetTag_CheckOutDMO
                                       from b in _ATContext.INV_Asset_AssetTagDMO
                                       from c in _ATContext.INV_Master_StoreDMO
                                       from d in _ATContext.INV_Master_ItemDMO
                                       from e in _ATContext.INV_Master_LocationDMO
                                       where (a.INVAAT_Id == b.INVAAT_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && a.INVMLO_Id == e.INVMLO_Id
                                       && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.INVAAT_DisposedFlg== false)
                                       select new AssetTagCheckOutDTO
                                       {
                                           INVATCO_Id = a.INVATCO_Id,
                                           INVAAT_Id = a.INVAAT_Id,
                                           INVMST_Id = a.INVMST_Id,
                                           INVMI_Id = a.INVMI_Id,
                                           INVMLO_Id = a.INVMLO_Id,
                                           INVMS_StoreName = c.INVMS_StoreName,
                                           INVMI_ItemName = d.INVMI_ItemName,
                                           INVAAT_AssetId = b.INVAAT_AssetId,
                                           INVMLO_LocationRoomName = e.INVMLO_LocationRoomName,
                                           INVATCO_CheckoutDate = a.INVATCO_CheckoutDate,
                                           INVATCO_ReceivedBy = a.INVATCO_ReceivedBy,
                                           INVATCO_CheckInFlg=a.INVATCO_CheckInFlg,
                                           INVATCO_CheckOutRemarks = a.INVATCO_CheckOutRemarks,
                                           INVAAT_AssetDescription = b.INVAAT_AssetDescription,
                                           INVAAT_ModelNo = b.INVAAT_ModelNo,
                                           INVAAT_SerialNo = b.INVAAT_SerialNo,
                                           INVAAT_ManufacturedDate = b.INVAAT_ManufacturedDate,
                                           INVAAT_PurchaseDate = b.INVAAT_PurchaseDate,
                                           INVATCO_ActiveFlg = a.INVATCO_ActiveFlg
                                       }).Distinct().OrderBy(m => m.INVATCO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag Check Out load Page:" + ex.Message);
            }
            return data;
        }
        public AssetTagCheckOutDTO getitems(AssetTagCheckOutDTO data)
        {
            try
            {
                data.get_items = (from a in _ATContext.INV_Asset_AssetTagDMO
                                  from b in _ATContext.INV_Master_StoreDMO
                                  from c in _ATContext.INV_Master_ItemDMO
                                  where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMST_Id == data.INVMST_Id && a.INVAAT_ActiveFlg == true)
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

        public async Task<AssetTagCheckOutDTO> getitemtagdata(AssetTagCheckOutDTO data)
        {
            try
            {
                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTagItemData";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.INVMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.INVMI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_itemtagdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag get Data:" + ex.Message);
            }
            return data;
        }

        public AssetTagCheckOutDTO savedata(AssetTagCheckOutDTO data)
        {
            try
            {
                if (data.INVATCO_Id != 0)
                {
                    foreach (var t in data.tagckoutArray)
                    {
                        DateTime? ckoutdate = null;

                        if (data.INVATCO_CheckoutDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            ckoutdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.INVATCO_CheckoutDate), INDIAN_ZONE);
                        }

                        var result = _ATContext.INV_AssetTag_CheckOutDMO.Single(a => a.MI_Id == data.MI_Id && a.INVATCO_Id == data.INVATCO_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVATCO_CheckoutDate = ckoutdate;
                        result.HRME_Id = data.HRME_Id;
                        result.INVATCO_ReceivedBy = data.INVATCO_ReceivedBy;
                        result.INVATCO_CheckOutRemarks = t.INVATCO_CheckOutRemarks;
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
                    foreach (var t in data.tagckoutArray)
                    {
                        DateTime? ckoutdate = null;

                        if (data.INVATCO_CheckoutDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            ckoutdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(data.INVATCO_CheckoutDate), INDIAN_ZONE);
                        }

                        INV_AssetTag_CheckOutDMO itag = new INV_AssetTag_CheckOutDMO();
                        itag.MI_Id = data.MI_Id;
                        itag.INVMST_Id = data.INVMST_Id;
                        itag.INVMI_Id = data.INVMI_Id;
                        itag.INVAAT_Id = t.INVAAT_Id;
                        itag.INVMLO_Id = data.INVMLO_Id;
                        itag.INVATCO_CheckoutDate = ckoutdate;
                        itag.INVATCO_CheckOutQty = data.INVATCO_CheckOutQty;
                        itag.HRME_Id = data.HRME_Id;
                        itag.INVATCO_ReceivedBy = data.INVATCO_ReceivedBy;
                        itag.INVATCO_CheckOutRemarks = t.INVATCO_CheckOutRemarks;
                        itag.INVATCO_CheckInFlg = false;
                        itag.INVATCO_ActiveFlg = true;
                        itag.UpdatedDate = DateTime.Now;
                        itag.CreatedDate = DateTime.Now;
                        _ATContext.Add(itag);

                        var tag = _ATContext.INV_Asset_AssetTagDMO.Single(a => a.MI_Id == data.MI_Id && a.INVAAT_Id == itag.INVAAT_Id);
                        tag.INVAAT_CheckOutFlg = true;
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


        public AssetTagCheckOutDTO deactive(AssetTagCheckOutDTO data)
        {
            try
            {
                var result = _ATContext.INV_AssetTag_CheckOutDMO.Single(t => t.INVATCO_Id == data.INVATCO_Id);

                if (result.INVATCO_ActiveFlg == true)
                {
                    result.INVATCO_ActiveFlg = false;
                }
                else if (result.INVATCO_ActiveFlg == false)
                {
                    result.INVATCO_ActiveFlg = true;
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
