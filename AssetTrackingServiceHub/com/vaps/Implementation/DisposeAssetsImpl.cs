using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class DisposeAssetsImpl : Interface.DisposeAssetsInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<DisposeAssetsImpl> _logAT;
        public DisposeAssetsImpl(AssetTrackingContext ATContext, ILogger<DisposeAssetsImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public DisposeAssetsDTO getloaddata(DisposeAssetsDTO data)
        {
            try
            {
                // data.get_store = _ATContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMST_Id).ToArray();
                data.get_store = (from a in _ATContext.INV_Asset_CheckOutDMO
                                  from b in _ATContext.INV_Master_LocationDMO
                                  from c in _ATContext.INV_Master_StoreDMO
                                  where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                  select new CheckInAssetsDTO
                                  {
                                      INVMST_Id = a.INVMST_Id,
                                      INVMS_StoreName = c.INVMS_StoreName
                                  }).Distinct().OrderBy(m => m.INVACO_Id).ToArray();


                data.get_dispose = (from a in _ATContext.INV_Asset_DisposeDMO
                                    from b in _ATContext.INV_Master_StoreDMO
                                    from c in _ATContext.INV_Master_ItemDMO
                                    from d in _ATContext.INV_Master_LocationDMO
                                    where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id)
                                    select new DisposeAssetsDTO
                                    {
                                        INVADI_Id = a.INVADI_Id,
                                        INVMST_Id = a.INVMST_Id,
                                        INVMS_StoreName = b.INVMS_StoreName,
                                        INVMI_Id = a.INVMI_Id,
                                        INVMI_ItemName = c.INVMI_ItemName,
                                        INVSTO_SalesRate = a.INVSTO_SalesRate,
                                        INVMLO_Id = a.INVMLO_Id,
                                        INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                        INVADI_DisposedDate = a.INVADI_DisposedDate,
                                        INVADI_DisposedQty = a.INVADI_DisposedQty,
                                        INVADI_DisposedRemarks = a.INVADI_DisposedRemarks,
                                        INVADI_ActiveFlg = a.INVADI_ActiveFlg

                                    }).Distinct().OrderBy(m => m.INVADI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Dispose load Page:" + ex.Message);
            }
            return data;
        }

        public DisposeAssetsDTO getlocations(DisposeAssetsDTO data)
        {
            try
            {
                data.get_locations = (from a in _ATContext.INV_Asset_CheckOutDMO
                                      from b in _ATContext.INV_Master_LocationDMO
                                      from c in _ATContext.INV_Master_StoreDMO
                                      where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMST_Id == data.INVMST_Id)
                                      select new DisposeAssetsDTO
                                      {
                                          INVMLO_Id = a.INVMLO_Id,
                                          INVMST_Id = a.INVMST_Id,
                                          INVMLO_LocationRoomName = b.INVMLO_LocationRoomName
                                      }).Distinct().OrderBy(m => m.INVACO_Id).ToArray();


            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Dispose get Loaction:" + ex.Message);
            }
            return data;
        }

        public DisposeAssetsDTO getitems(DisposeAssetsDTO data)
        {
            try
            {
                data.get_items = (from a in _ATContext.INV_Asset_CheckOutDMO
                                  from b in _ATContext.INV_Master_LocationDMO
                                  from c in _ATContext.INV_Master_StoreDMO
                                  from d in _ATContext.INV_Master_ItemDMO
                                  from e in _ATContext.INV_StockDMO
                                  where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && e.INVMST_Id == c.INVMST_Id && e.INVMI_Id == d.INVMI_Id
                                  && a.INVSTO_SalesRate == e.INVSTO_SalesRate && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMST_Id == data.INVMST_Id && a.INVACO_ActiveFlg == true && d.INVMI_ActiveFlg == true)
                                  select new CheckInAssetsDTO
                                  {
                                      // INVACO_Id = a.INVACO_Id,
                                      INVMLO_Id = a.INVMLO_Id,
                                      INVMST_Id = a.INVMST_Id,
                                      INVMI_Id = a.INVMI_Id,
                                      // INVSTO_PurchaseDate = e.INVSTO_PurchaseDate,
                                      INVSTO_SalesRate = e.INVSTO_SalesRate,
                                      //INVSTO_AvaiableStock = e.INVSTO_AvaiableStock,
                                      // INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                                      INVMI_ItemName = d.INVMI_ItemName
                                  }).Distinct().OrderByDescending(m => m.INVMI_ItemName).ToArray();

                //var config = _ATContext.INV_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVC_ProcessApplFlg == true).ToList();
                //data.INVC_LIFOFIFOFlg = config.FirstOrDefault().INVC_LIFOFIFOFlg;

                //if (data.INVC_LIFOFIFOFlg == "LIFO")
                //{
                //    data.get_items = (from a in _ATContext.INV_Asset_CheckOutDMO
                //                      from b in _ATContext.INV_Master_LocationDMO
                //                      from c in _ATContext.INV_Master_StoreDMO
                //                      from d in _ATContext.INV_Master_ItemDMO
                //                      from e in _ATContext.INV_StockDMO
                //                      where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && e.INVMST_Id == c.INVMST_Id && e.INVMI_Id == d.INVMI_Id
                //                      && a.INVSTO_SalesRate == e.INVSTO_SalesRate && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMST_Id == data.INVMST_Id && a.INVACO_ActiveFlg == true && d.INVMI_ActiveFlg == true)
                //                      select new CheckInAssetsDTO
                //                      {
                //                         // INVACO_Id = a.INVACO_Id,
                //                          INVMLO_Id = a.INVMLO_Id,
                //                          INVMST_Id = a.INVMST_Id,
                //                          INVMI_Id = a.INVMI_Id,
                //                          INVSTO_PurchaseDate = e.INVSTO_PurchaseDate,
                //                          INVSTO_SalesRate = e.INVSTO_SalesRate,
                //                          INVSTO_AvaiableStock = e.INVSTO_AvaiableStock,
                //                          INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                //                          INVMI_ItemName = d.INVMI_ItemName
                //                      }).Distinct().OrderByDescending(m => m.INVSTO_PurchaseDate).ToArray();
                //}
                //else
                //{
                //    data.get_items = (from a in _ATContext.INV_Asset_CheckOutDMO
                //                      from b in _ATContext.INV_Master_LocationDMO
                //                      from c in _ATContext.INV_Master_StoreDMO
                //                      from d in _ATContext.INV_Master_ItemDMO
                //                      from e in _ATContext.INV_StockDMO
                //                      where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && e.INVMST_Id == c.INVMST_Id && e.INVMI_Id == d.INVMI_Id && a.INVSTO_SalesRate == e.INVSTO_SalesRate && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMST_Id == data.INVMST_Id && a.INVACO_ActiveFlg == true && d.INVMI_ActiveFlg == true)
                //                      select new CheckInAssetsDTO
                //                      {
                //                          INVACO_Id = a.INVACO_Id,
                //                          INVMLO_Id = a.INVMLO_Id,
                //                          INVMST_Id = a.INVMST_Id,
                //                          INVMI_Id = a.INVMI_Id,
                //                          INVSTO_PurchaseDate = e.INVSTO_PurchaseDate,
                //                          INVSTO_SalesRate = e.INVSTO_SalesRate,
                //                          INVSTO_AvaiableStock = e.INVSTO_AvaiableStock,
                //                          INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                //                          INVMI_ItemName = d.INVMI_ItemName
                //                      }).Distinct().OrderBy(m => m.INVSTO_PurchaseDate).ToArray();
                //}

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("DISPOSE get item:" + ex.Message);
            }
            return data;
        }
        public DisposeAssetsDTO getdetails(DisposeAssetsDTO data)
        {
            try
            {
                data.get_details = (from a in _ATContext.INV_Asset_CheckOutDMO
                                    from b in _ATContext.INV_Master_LocationDMO
                                    from c in _ATContext.INV_Master_StoreDMO
                                    from d in _ATContext.INV_Master_ItemDMO
                                    from e in _ATContext.INV_StockDMO
                                    where (a.INVMLO_Id == b.INVMLO_Id && a.INVMST_Id == c.INVMST_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == b.MI_Id
                                    && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVSTO_SalesRate == e.INVSTO_SalesRate && a.MI_Id == data.MI_Id && a.INVMLO_Id == data.INVMLO_Id && a.INVMST_Id == data.INVMST_Id && a.INVMI_Id == data.INVMI_Id && a.INVSTO_SalesRate == data.INVSTO_SalesRate && a.INVACO_ActiveFlg == true && d.INVMI_ActiveFlg == true)
                                    select new CheckInAssetsDTO
                                    {
                                        INVACO_Id = a.INVACO_Id,
                                        INVMLO_Id = a.INVMLO_Id,
                                        INVMST_Id = a.INVMST_Id,
                                        INVMI_Id = a.INVMI_Id,
                                        INVSTO_SalesRate = e.INVSTO_SalesRate,
                                        INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                                        INVMI_ItemName = d.INVMI_ItemName
                                    }).Distinct().OrderBy(m => m.INVACO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Dispose Item Details:" + ex.Message);
            }
            return data;
        }
        public DisposeAssetsDTO savedetails(DisposeAssetsDTO data)
        {
            try
            {
                INV_Asset_DisposeDMO dispose = new INV_Asset_DisposeDMO();
                dispose.MI_Id = data.MI_Id;
                dispose.INVMST_Id = data.INVMST_Id;
                dispose.INVMI_Id = data.INVMI_Id;
                dispose.INVSTO_SalesRate = data.INVSTO_SalesRate;
                dispose.INVMLO_Id = data.INVMLO_Id;
                dispose.INVADI_DisposedDate = data.INVADI_DisposedDate;
                dispose.INVADI_DisposedQty = data.INVADI_DisposedQty;
                dispose.INVADI_DisposedRemarks = data.INVADI_DisposedRemarks;
                dispose.INVADI_ActiveFlg = true;
                dispose.CreatedDate = DateTime.Now;
                dispose.UpdatedDate = DateTime.Now;
                _ATContext.Add(dispose);

                var contactExists = _ATContext.SaveChanges();
                if (contactExists > 0)
                {
                    try
                    {
                        var contactExistsP = _ATContext.Database.ExecuteSqlCommand("INV_StockupdatesDispose @p0, @p1,@p2, @p3,@p4,@p5", data.MI_Id, data.INVMST_Id, data.INVMLO_Id, data.INVMI_Id, data.INVSTO_SalesRate, dispose.INVADI_Id);
                        if (contactExistsP > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        data.message = "Error";
                        _logAT.LogInformation("Dispose Procedure :" + ex.Message);
                    }                   
                }
                else
                {
                    data.returnval = false;
                }


            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Asset Dispose savedata :" + ex.Message);
            }
            return data;
        }

        public DisposeAssetsDTO deactive(DisposeAssetsDTO data)
        {
            try
            {
                var result = _ATContext.INV_Asset_DisposeDMO.Single(t => t.INVADI_Id == data.INVADI_Id);

                if (result.INVADI_ActiveFlg == true)
                {
                    result.INVADI_ActiveFlg = false;
                }
                else if (result.INVADI_ActiveFlg == false)
                {
                    result.INVADI_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ATContext.Update(result);
                int returnval = _ATContext.SaveChanges();
                if (returnval > 0)
                {
                    var contactExistsP = _ATContext.Database.ExecuteSqlCommand("INV_StockupdatesDispose @p0, @p1,@p2, @p3,@p4,@p5", result.MI_Id, result.INVMST_Id, result.INVMLO_Id, result.INVMI_Id, result.INVSTO_SalesRate, data.INVADI_Id);
                    if (contactExistsP > 0)
                    {
                        data.returnval = false;
                    }
                    else
                    {
                        data.returnval = true;
                    }
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
