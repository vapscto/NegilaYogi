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
    public class CheckOutAssetsImpl : Interface.CheckOutAssetsInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<CheckOutAssetsImpl> _logAT;
        public CheckOutAssetsImpl(AssetTrackingContext ATContext, ILogger<CheckOutAssetsImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public CheckOutAssetsDTO getloaddata(CheckOutAssetsDTO data)
        {
            try
            {
                data.get_store = _ATContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).Distinct().OrderBy(R => R.INVMST_Id).ToArray();
                // data.get_items = _ATContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMI_ActiveFlg == true).OrderBy(m => m.INVMI_Id).ToArray();
                data.get_locations = _ATContext.INV_Master_LocationDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMLO_ActiveFlg == true).Distinct().OrderBy(R => R.INVMLO_Id).ToArray();

                //data.get_items = (from a in _ATContext.INV_Master_ItemDMO
                //                  from b in _ATContext.INV_StockDMO
                //                  from c in _ATContext.INV_Master_StoreDMO
                //                  where (a.INVMI_Id == b.INVMI_Id && b.INVMST_Id==c.INVMST_Id && a.MI_Id == data.MI_Id && b.INVSTO_AvaiableStock != 0)
                //                  select new CheckOutAssetsDTO
                //                  {
                //                      INVMI_Id = a.INVMI_Id,
                //                      INVMI_ItemName = a.INVMI_ItemName,
                //                  }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();

                data.get_checkout = (from a in _ATContext.INV_Asset_CheckOutDMO
                                     from b in _ATContext.INV_Master_StoreDMO
                                     from c in _ATContext.INV_Master_ItemDMO
                                     from d in _ATContext.INV_Master_LocationDMO
                                     from e in _ATContext.INV_StockDMO
                                     where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMST_Id == e.INVMST_Id && a.INVMI_Id == e.INVMI_Id && a.INVMLO_Id == d.INVMLO_Id && a.MI_Id == data.MI_Id)
                                     select new CheckOutAssetsDTO
                                     {
                                         INVACO_Id = a.INVACO_Id,
                                         INVMST_Id = a.INVMST_Id,
                                         INVMS_StoreName = b.INVMS_StoreName,
                                         INVMI_Id = a.INVMI_Id,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVSTO_SalesRate = a.INVSTO_SalesRate,
                                         INVMLO_Id = a.INVMLO_Id,
                                         HRME_Id = a.HRME_Id,
                                         INVMLO_LocationRoomName = d.INVMLO_LocationRoomName,
                                         INVACO_ReceivedBy = a.INVACO_ReceivedBy,
                                         INVACO_CheckoutDate = a.INVACO_CheckoutDate,
                                         INVACO_CheckOutQty = a.INVACO_CheckOutQty,
                                         INVACO_CheckOutRemarks = a.INVACO_CheckOutRemarks,
                                         INVACO_ActiveFlg = a.INVACO_ActiveFlg

                                     }).Distinct().OrderBy(m => m.INVACO_Id).ToArray();

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
                _logAT.LogInformation("Assets Checkout load Page:" + ex.Message);
            }
            return data;
        }

        public CheckOutAssetsDTO getitems(CheckOutAssetsDTO data)
        {
            try
            {
                var config = _ATContext.INV_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVC_ProcessApplFlg == true).ToList();
                data.INVC_LIFOFIFOFlg = config.FirstOrDefault().INVC_LIFOFIFOFlg;

                if (data.INVC_LIFOFIFOFlg == "LIFO")
                {
                    data.get_items = (from a in _ATContext.INV_Master_ItemDMO
                                      from b in _ATContext.INV_StockDMO
                                      from c in _ATContext.INV_Master_StoreDMO
                                      where (a.INVMI_Id == b.INVMI_Id && b.INVMST_Id == c.INVMST_Id && a.MI_Id == data.MI_Id && b.INVSTO_AvaiableStock != 0 && b.INVSTO_PurchaseDate != null && b.INVMST_Id == data.INVMST_Id && a.INVMI_ActiveFlg == true)
                                      select new CheckOutAssetsDTO
                                      {
                                          INVMI_Id = a.INVMI_Id,
                                          INVSTO_PurchaseDate = b.INVSTO_PurchaseDate,
                                          INVMI_ItemName = a.INVMI_ItemName,
                                          INVSTO_SalesRate = b.INVSTO_SalesRate,
                                          INVSTO_AvaiableStock = b.INVSTO_AvaiableStock,
                                      }).Distinct().OrderByDescending(m => m.INVSTO_PurchaseDate).ToArray();
                }
                else
                {
                    data.get_items = (from a in _ATContext.INV_Master_ItemDMO
                                      from b in _ATContext.INV_StockDMO
                                      from c in _ATContext.INV_Master_StoreDMO
                                      where (a.INVMI_Id == b.INVMI_Id && b.INVMST_Id == c.INVMST_Id && a.MI_Id == data.MI_Id && b.INVSTO_AvaiableStock != 0 && b.INVSTO_PurchaseDate != null && b.INVMST_Id == data.INVMST_Id && a.INVMI_ActiveFlg == true)
                                      select new CheckOutAssetsDTO
                                      {
                                          INVMI_Id = a.INVMI_Id,
                                          INVSTO_PurchaseDate = b.INVSTO_PurchaseDate,
                                          INVMI_ItemName = a.INVMI_ItemName,
                                          INVSTO_SalesRate = b.INVSTO_SalesRate,
                                          INVSTO_AvaiableStock = b.INVSTO_AvaiableStock,
                                      }).Distinct().OrderBy(m => m.INVSTO_PurchaseDate).ToArray();
                }

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Checkout get items:" + ex.Message);
            }
            return data;
        }

        public CheckOutAssetsDTO savedetails(CheckOutAssetsDTO data)
        {
            try
            {
                if (data.INVACO_Id != 0)
                {
                    var result = _ATContext.INV_Asset_CheckOutDMO.Single(t => t.INVACO_Id == data.INVACO_Id);
                    result.MI_Id = data.MI_Id;
                    result.INVMST_Id = data.INVMST_Id;
                    result.INVMI_Id = data.INVMI_Id;
                    result.INVMLO_Id = data.INVMLO_Id;
                    result.INVACO_CheckoutDate = data.INVACO_CheckoutDate;
                    result.INVACO_CheckOutRemarks = data.INVACO_CheckOutRemarks;
                    result.INVACO_ActiveFlg = true;
                    result.UpdatedDate = DateTime.Now;

                    result.INVACO_ReceivedBy = data.INVACO_ReceivedBy;
                    result.HRME_Id = data.HRME_Id;

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
                else
                {
                    INV_Asset_CheckOutDMO checkout = new INV_Asset_CheckOutDMO();
                    checkout.MI_Id = data.MI_Id;
                    checkout.INVMST_Id = data.INVMST_Id;
                    checkout.INVMI_Id = data.INVMI_Id;
                    checkout.INVSTO_SalesRate = data.INVSTO_SalesRate;
                    checkout.INVMLO_Id = data.INVMLO_Id;
                    checkout.INVACO_CheckoutDate = data.INVACO_CheckoutDate;
                    checkout.INVACO_CheckOutQty = data.INVACO_CheckOutQty;
                    checkout.INVACO_ReceivedBy = data.INVACO_ReceivedBy;
                    checkout.INVACO_CheckOutRemarks = data.INVACO_CheckOutRemarks;
                    checkout.HRME_Id = data.HRME_Id;
                    checkout.INVACO_ActiveFlg = true;
                    checkout.CreatedDate = DateTime.Now;
                    checkout.UpdatedDate = DateTime.Now;
                    _ATContext.Add(checkout);

                    var contactExists = _ATContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        try
                        {
                            var contactExistsP = _ATContext.Database.ExecuteSqlCommand("INV_StockupdatesCheckOut @p0, @p1,@p2, @p3,@p4,@p5", data.MI_Id, data.INVMST_Id, data.INVMLO_Id, data.INVMI_Id, data.INVSTO_SalesRate, checkout.INVACO_Id);
                            if (contactExistsP > 0)
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
                            data.message = "Error";
                            _logAT.LogInformation("Asset Checkout Procedure :" + ex.Message);
                        }

                    }
                    else
                    {
                        data.returnval = false;
                    }

                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Asset Check out savedata :" + ex.Message);
            }
            return data;
        }

        public CheckOutAssetsDTO deactive(CheckOutAssetsDTO data)
        {
            try
            {
                var result = _ATContext.INV_Asset_CheckOutDMO.Single(t => t.INVACO_Id == data.INVACO_Id);

                if (result.INVACO_ActiveFlg == true)
                {
                    result.INVACO_ActiveFlg = false;
                }
                else if (result.INVACO_ActiveFlg == false)
                {
                    result.INVACO_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _ATContext.Update(result);
                int returnval = _ATContext.SaveChanges();
                if (returnval > 0)
                {
                    try
                    {
                        var contactExistsP = _ATContext.Database.ExecuteSqlCommand("INV_StockupdatesCheckOut @p0, @p1,@p2, @p3,@p4,@p5", data.MI_Id, data.INVMST_Id, data.INVMLO_Id, data.INVMI_Id, data.INVSTO_SalesRate, data.INVACO_Id);
                        if (contactExistsP > 0)
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
                        data.message = "Error";
                        _logAT.LogInformation("Asset Checkout Procedure :" + ex.Message);
                    }
                    //  data.returnval = true;
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
        public CheckOutAssetsDTO getcontactperson(CheckOutAssetsDTO data)
        {
            try
            {
                var contactid = _ATContext.INV_Master_LocationDMO.Single(m => m.MI_Id == data.MI_Id && m.INVMLO_Id == data.INVMLO_Id).HRME_Id;

                data.get_employee = (from a in _ATContext.MasterEmployee
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                     select new AT_MasterLocationDTO
                                     {
                                         employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeOrder = a.HRME_EmployeeOrder,

                                     }).Distinct().OrderBy(h => h.HRME_EmployeeOrder).ToArray();


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
        public CheckOutAssetsDTO get_avaiablestock(CheckOutAssetsDTO data)
        {
            try
            {
                //data.availablestock = (from a in _ATContext.INV_StockDMO
                //                       where (a.MI_Id == data.MI_Id && a.INVMI_Id == data.INVMI_Id && a.INVMST_Id == data.INVMST_Id)
                //                       group new { a }
                //                  by new { a.INVSTO_AvaiableStock } into p
                //                       select new CheckOutAssetsDTO
                //                       {
                //                           INVSTO_AvaiableStock = p.Sum(s => s.a.INVSTO_AvaiableStock)
                //                       }).Distinct().ToArray();

                data.availablestock = (from a in _ATContext.INV_Master_ItemDMO
                                       from b in _ATContext.INV_StockDMO
                                       from c in _ATContext.INV_Master_StoreDMO
                                       where (a.INVMI_Id == b.INVMI_Id && b.INVMST_Id == c.INVMST_Id && a.MI_Id == data.MI_Id && b.INVSTO_AvaiableStock != 0 && b.INVSTO_PurchaseDate != null && b.INVMI_Id == data.INVMI_Id && b.INVMST_Id == data.INVMST_Id && b.INVSTO_SalesRate == data.INVSTO_SalesRate)
                                       select new CheckOutAssetsDTO
                                       {
                                           INVMI_Id = a.INVMI_Id,
                                           INVSTO_PurchaseDate = b.INVSTO_PurchaseDate,
                                           INVSTO_SalesRate = b.INVSTO_SalesRate,
                                           INVMI_ItemName = a.INVMI_ItemName,
                                           INVSTO_AvaiableStock = b.INVSTO_AvaiableStock,

                                       }).Distinct().OrderByDescending(m => m.INVSTO_PurchaseDate).ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Assets Check-out avaiablestock:" + ex.Message);
            }
            return data;
        }


    }
}
