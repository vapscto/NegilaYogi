using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class INV_PhyStock_UpdationImpl : Interface.INV_PhyStock_UpdationInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PhyStock_UpdationImpl> _logInv;
        public INV_PhyStock_UpdationImpl(InventoryContext InvContext, ILogger<INV_PhyStock_UpdationImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_PhyStock_UpdationDTO getloaddata(INV_PhyStock_UpdationDTO data)
        {
            try
            {
                data.get_Store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMST_Id).ToArray();
                data.get_phyStockdata = (from a in _INVContext.INV_PhysicalStock_UpdationDMO
                                         from b in _INVContext.INV_Master_StoreDMO
                                         from c in _INVContext.INV_Master_ItemDMO
                                         from d in _INVContext.INV_StockDMO
                                         from e in _INVContext.INV_Master_UOMDMO
                                         where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && c.INVMUOM_Id == e.INVMUOM_Id && b.INVMST_Id == d.INVMST_Id && c.INVMI_Id == d.INVMI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                         select new INV_PhyStock_UpdationDTO
                                         {
                                             INVPSU_Id = a.INVPSU_Id,
                                             INVMST_Id = a.INVMST_Id,
                                             INVMI_Id = a.INVMI_Id,
                                             INVMUOM_Id = c.INVMUOM_Id,
                                             INVMUOM_UOMName = e.INVMUOM_UOMName,
                                             INVMS_StoreName = b.INVMS_StoreName,
                                             INVMI_ItemName = c.INVMI_ItemName,
                                             INVPSU_StockPlus = a.INVPSU_StockPlus,
                                             INVPSU_StockMinus = a.INVPSU_StockMinus,
                                             INVSTO_AvaiableStock = d.INVSTO_AvaiableStock,
                                             INVSTO_SalesRate = d.INVSTO_SalesRate,
                                             INVPSU_Remarks = a.INVPSU_Remarks,
                                             INVPSU_ActiveFlg = a.INVPSU_ActiveFlg
                                         }).Distinct().OrderBy(p => p.INVPSU_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Phy Stock load Page:" + ex.Message);
            }
            return data;
        }

        public INV_PhyStock_UpdationDTO savedetails(INV_PhyStock_UpdationDTO data)
        {
            try
            {
                if (data.INVPSU_Id != 0)
                {
                    foreach (var pu in data.phyStock)
                    {
                        var PSU = _INVContext.INV_PhysicalStock_UpdationDMO.Single(b => b.MI_Id == data.MI_Id && b.INVPSU_Id == data.INVPSU_Id);
                        PSU.MI_Id = data.MI_Id;
                        PSU.INVMST_Id = data.INVMST_Id;
                        PSU.INVMI_Id = pu.INVMI_Id;
                        PSU.INVPSU_StockPlus = pu.INVPSU_StockPlus;
                        PSU.INVPSU_StockMinus = pu.INVPSU_StockMinus;
                        PSU.INVPSU_Remarks = pu.INVPSU_Remarks;
                        PSU.INVPSU_ActiveFlg = true;
                        PSU.INVPSU_UpdatedBy = data.UserId;
                        PSU.UpdatedDate = DateTime.Now;
                        _INVContext.Update(PSU);
                        var contextPSU = _INVContext.SaveChanges();
                        if (contextPSU > 0)
                        {
                            try
                            {
                                var contactExistsO = _INVContext.Database.ExecuteSqlCommand("INV_InsertPhysicalStock @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, data.INVMST_Id, pu.INVMI_Id, pu.INVSTO_SalesRate, pu.INVPSU_StockPlus, pu.INVPSU_StockMinus);
                                if (contactExistsO > 0)
                                {
                                    data.returnduplicatestatus = "Updated";
                                }
                                else
                                {
                                    data.returnduplicatestatus = "notUpdated";
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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
                    foreach (var p in data.phyStock)
                    {
                        INV_PhysicalStock_UpdationDMO PS = new INV_PhysicalStock_UpdationDMO();
                        PS.MI_Id = data.MI_Id;
                        PS.INVMST_Id = data.INVMST_Id;
                        PS.INVMI_Id = p.INVMI_Id;
                        PS.INVPSU_StockPlus = p.INVPSU_StockPlus;
                        PS.INVPSU_StockMinus = p.INVPSU_StockMinus;
                        PS.INVPSU_Remarks = p.INVPSU_Remarks;
                        PS.INVPSU_ActiveFlg = true;
                        PS.INVPSU_CreatedBy = data.UserId;
                        PS.INVPSU_UpdatedBy = data.UserId;
                        PS.UpdatedDate = DateTime.Now;
                        PS.CreatedDate = DateTime.Now;
                        _INVContext.Add(PS);
                        var contextPSU = _INVContext.SaveChanges();
                        if (contextPSU > 0)
                        {
                            try
                            {
                                var contactExistsO = _INVContext.Database.ExecuteSqlCommand("INV_InsertPhysicalStock @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, data.INVMST_Id, p.INVMI_Id, p.INVSTO_SalesRate, p.INVPSU_StockPlus, p.INVPSU_StockMinus);
                                if (contactExistsO > 0)
                                {
                                    data.returnduplicatestatus = "Updated";
                                }
                                else
                                {
                                    data.returnduplicatestatus = "notUpdated";
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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
                _logInv.LogInformation("Phy Stock Update savedata :" + ex.Message);
            }
            return data;
        }

        public INV_PhyStock_UpdationDTO deactive(INV_PhyStock_UpdationDTO data)
        {
            try
            {
                var result = _INVContext.INV_PhysicalStock_UpdationDMO.Single(t => t.INVPSU_Id == data.INVPSU_Id);
                if (result.INVPSU_ActiveFlg == true)
                {
                    result.INVPSU_ActiveFlg = false;
                }
                else if (result.INVPSU_ActiveFlg == false)
                {
                    result.INVPSU_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int returnval = _INVContext.SaveChanges();
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

        public INV_PhyStock_UpdationDTO getobdetails(INV_PhyStock_UpdationDTO data)
        {
            //try
            //{

            //    data.get_obdetails = (from a in _INVContext.INV_PhyStock_UpdationDMO
            //                          from b in _INVContext.INV_Master_StoreDMO
            //                          from c in _INVContext.INV_Master_ItemDMO
            //                          from d in _INVContext.INV_Master_UOMDMO
            //                          where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && a.MI_Id == b.MI_Id && c.MI_Id == d.MI_Id && a.MI_Id == data.MI_Id && a.INVOB_Id == data.INVOB_Id)
            //                          select new INV_PhyStock_UpdationDTO
            //                          {
            //                              INVMS_StoreName = b.INVMS_StoreName,
            //                              INVMI_ItemName = c.INVMI_ItemName,
            //                              INVMUOM_UOMName = d.INVMUOM_UOMName,
            //                              INVMUOM_UOMAliasName = d.INVMUOM_UOMAliasName,
            //                              INVOB_Id = a.INVOB_Id,
            //                              INVMST_Id = a.INVMST_Id,
            //                              INVMI_Id = a.INVMI_Id,
            //                              INVMUOM_Id = a.INVMUOM_Id,
            //                              INVOB_BatchNo = a.INVOB_BatchNo,
            //                              INVOB_PurchaseDate = a.INVOB_PurchaseDate,
            //                              INVOB_PurchaseRate = a.INVOB_PurchaseRate,
            //                              INVOB_SaleRate = a.INVOB_SaleRate,
            //                              INVOB_Qty = a.INVOB_Qty,
            //                              INVOB_Naration = a.INVOB_Naration,
            //                              INVOB_MfgDate = a.INVOB_MfgDate,
            //                              INVOB_ExpDate = a.INVOB_ExpDate,
            //                              INVOB_ActiveFlg = a.INVOB_ActiveFlg,

            //                          }).Distinct().OrderBy(m => m.INVOB_Id).ToArray();
            //}
            //catch (Exception ex)
            //{
            //    _logInv.LogInformation("OB load Page:" + ex.Message);
            //}
            return data;
        }


    }
}
