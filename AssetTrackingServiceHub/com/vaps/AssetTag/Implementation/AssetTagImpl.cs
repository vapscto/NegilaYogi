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
    public class AssetTagImpl : Interface.AssetTagInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetTagImpl> _logAT;
        public AssetTagImpl(AssetTrackingContext ATContext, ILogger<AssetTagImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public AssetTagDTO getloaddata(AssetTagDTO data)
        {
            try
            {
                data.get_store = (from a in _ATContext.INV_StockDMO
                                  from b in _ATContext.INV_Master_StoreDMO
                                  where (a.INVMST_Id == b.INVMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                  select new AssetTagDTO
                                  {
                                      INVMST_Id = a.INVMST_Id,
                                      INVMS_StoreName = b.INVMS_StoreName,
                                  }).Distinct().OrderBy(m => m.INVMST_Id).ToArray();


                data.get_Assetstag = (from a in _ATContext.INV_Asset_AssetTagDMO
                                      from b in _ATContext.INV_Master_StoreDMO
                                      from c in _ATContext.INV_Master_ItemDMO
                                      where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                      select new AssetTagDTO
                                      {
                                          INVAAT_Id = a.INVAAT_Id,
                                          INVMST_Id = a.INVMST_Id,
                                          INVMS_StoreName = b.INVMS_StoreName,
                                          INVMI_Id = a.INVMI_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVAAT_AssetId = a.INVAAT_AssetId,
                                          INVAAT_AssetDescription = a.INVAAT_AssetDescription,
                                          INVAAT_ManufacturedDate = a.INVAAT_ManufacturedDate,
                                          INVAAT_SKU = a.INVAAT_SKU,
                                          INVAAT_ModelNo = a.INVAAT_ModelNo,
                                          INVAAT_SerialNo = a.INVAAT_SerialNo,
                                          INVAAT_PurchaseDate = a.INVAAT_PurchaseDate,
                                          INVAAT_WarantyPeriod = a.INVAAT_WarantyPeriod,
                                          INVAAT_WarantyExpiryDate = a.INVAAT_WarantyExpiryDate,
                                          INVAAT_UnderAMCFlg = a.INVAAT_UnderAMCFlg,
                                          INVAAT_AMCExpiryDate = a.INVAAT_AMCExpiryDate,
                                          INVAAT_CheckOutFlg = a.INVAAT_CheckOutFlg,
                                          INVAAT_DisposedFlg = a.INVAAT_DisposedFlg,
                                          INVAAT_ActiveFlg = a.INVAAT_ActiveFlg,
                                           

                                      }).Distinct().OrderBy(m => m.INVAAT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Tag load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<AssetTagDTO> getdata(AssetTagDTO data)
        {
            try
            {
                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTagData";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.INVMST_Id
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
                        data.get_tagdata = retObject.ToArray();
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

        public AssetTagDTO savedata(AssetTagDTO data)
        {
            try
            {
                if (data.INVAAT_Id != 0)
                {
                    foreach (var t in data.tagckdArray)
                    {
                        DateTime? mafdate = null;
                        DateTime? purchasedate = null;
                        DateTime? wartydate = null;
                        DateTime? amcdate = null;
                        if (t.INVAAT_ManufacturedDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            mafdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_ManufacturedDate), INDIAN_ZONE);
                        }

                        if (t.INVAAT_PurchaseDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            purchasedate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_PurchaseDate), INDIAN_ZONE1);
                        }

                        if (t.INVAAT_WarantyExpiryDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE2 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            wartydate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_WarantyExpiryDate), INDIAN_ZONE2);
                        }

                        if (t.INVAAT_UnderAMCFlg == true)
                        {
                            if (t.INVAAT_AMCExpiryDate != null)
                            {
                                TimeZoneInfo INDIAN_ZONE3 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                                amcdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_AMCExpiryDate), INDIAN_ZONE3);
                            }
                        }

                        var result = _ATContext.INV_Asset_AssetTagDMO.Single(a => a.MI_Id == data.MI_Id && a.INVAAT_Id == data.INVAAT_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVAAT_AssetId = t.INVAAT_AssetId;
                        result.INVAAT_AssetDescription = t.INVAAT_AssetDescription;
                        result.INVAAT_ManufacturerName = t.INVAAT_ManufacturerName;
                        result.INVAAT_ManufacturedDate = mafdate;
                        result.INVAAT_SKU = t.INVAAT_SKU;
                        result.INVAAT_ModelNo = t.INVAAT_ModelNo;
                        result.INVAAT_SerialNo = t.INVAAT_SerialNo;
                        result.INVAAT_PurchaseDate = purchasedate;
                        result.INVAAT_WarantyPeriod = t.INVAAT_WarantyPeriod;
                        result.INVAAT_WarantyExpiryDate = wartydate;
                        result.INVAAT_UnderAMCFlg = t.INVAAT_UnderAMCFlg;
                        result.INVAAT_AMCExpiryDate = amcdate;
                        result.INVAAT_ActiveFlg = true;
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
                    foreach (var t in data.tagckdArray)
                    {
                        DateTime? mafdate = null;
                        DateTime? purchasedate = null;
                        DateTime? wartydate = null;
                        DateTime? amcdate = null;
                        if (t.INVAAT_ManufacturedDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            mafdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_ManufacturedDate), INDIAN_ZONE);
                        }


                        if (t.INVAAT_PurchaseDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE1 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            purchasedate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_PurchaseDate), INDIAN_ZONE1);
                        }


                        if (t.INVAAT_WarantyExpiryDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE2 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            wartydate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_WarantyExpiryDate), INDIAN_ZONE2);
                        }


                        if (t.INVAAT_AMCExpiryDate != null)
                        {
                            TimeZoneInfo INDIAN_ZONE3 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                            amcdate = TimeZoneInfo.ConvertTime(Convert.ToDateTime(t.INVAAT_AMCExpiryDate), INDIAN_ZONE3);
                        }

                        var res = _ATContext.INV_Asset_AssetTagDMO.Where(i => i.INVMST_Id == t.INVMST_Id && i.INVMI_Id == t.INVMI_Id && i.MI_Id == data.MI_Id && i.INVAAT_AssetId == t.INVAAT_AssetId).ToList();
                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            INV_Asset_AssetTagDMO tag = new INV_Asset_AssetTagDMO();
                            tag.MI_Id = data.MI_Id;
                            tag.INVMST_Id = t.INVMST_Id;
                            tag.INVMI_Id = t.INVMI_Id;
                            tag.INVAAT_AssetId = t.INVAAT_AssetId;
                            tag.INVAAT_AssetDescription = t.INVAAT_AssetDescription;
                            tag.INVAAT_ManufacturerName = t.INVAAT_ManufacturerName;
                            tag.INVAAT_ManufacturedDate = mafdate;
                            tag.INVAAT_SKU = t.INVAAT_SKU;
                            tag.INVAAT_ModelNo = t.INVAAT_ModelNo;
                            tag.INVAAT_SerialNo = t.INVAAT_SerialNo;
                            tag.INVAAT_PurchaseDate = purchasedate;
                            tag.INVAAT_WarantyPeriod = t.INVAAT_WarantyPeriod;
                            tag.INVAAT_WarantyExpiryDate = wartydate;
                            tag.INVAAT_UnderAMCFlg = t.INVAAT_UnderAMCFlg;
                            tag.INVAAT_AMCExpiryDate = amcdate;
                            tag.INVAAT_CheckOutFlg = false;
                            tag.INVAAT_DisposedFlg = false;
                            tag.INVAAT_ActiveFlg = true;
                            tag.UpdatedDate = DateTime.Now;
                            tag.CreatedDate = DateTime.Now;
                            _ATContext.Add(tag);
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
            }
            catch (Exception ex)
            {

                _logAT.LogInformation("Asset tag savedata :" + ex.Message);
            }
            return data;
        }


        public AssetTagDTO deactive(AssetTagDTO data)
        {
            try
            {
                var result = _ATContext.INV_Asset_AssetTagDMO.Single(t => t.INVAAT_Id == data.INVAAT_Id);

                if (result.INVAAT_ActiveFlg == true)
                {
                    result.INVAAT_ActiveFlg = false;
                }
                else if (result.INVAAT_ActiveFlg == false)
                {
                    result.INVAAT_ActiveFlg = true;
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
