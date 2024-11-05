using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class INV_StockImpl : Interface.INV_StockInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_StockImpl> _logInv;
        public INV_StockImpl(InventoryContext InvContext, ILogger<INV_StockImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_StockDTO getloaddata(INV_StockDTO data)
        {
            try
            {
                data.get_store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMST_Id).ToArray();
                data.get_item = _INVContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMI_ActiveFlg == true).OrderBy(m => m.INVMI_Id).ToArray();
                data.get_stock = (from a in _INVContext.INV_StockDMO
                                  from b in _INVContext.INV_Master_StoreDMO
                                  from c in _INVContext.INV_Master_ItemDMO
                                  where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                  select new INV_StockDTO
                                  {
                                      INVSTO_Id = a.INVSTO_Id,
                                      INVMS_StoreName = b.INVMS_StoreName,
                                      INVMI_ItemName = c.INVMI_ItemName,
                                      INVMST_Id = b.INVMST_Id,
                                      INVMI_Id = c.INVMI_Id,
                                      INVSTO_PurchaseDate = a.INVSTO_PurchaseDate,
                                      INVSTO_PurchaseRate = a.INVSTO_PurchaseRate,
                                      INVSTO_BatchNo = a.INVSTO_BatchNo,
                                      INVSTO_SalesRate = a.INVSTO_SalesRate,
                                      INVSTO_PurOBQty = a.INVSTO_PurOBQty,
                                      INVSTO_PurRetQty = a.INVSTO_PurRetQty,
                                      INVSTO_SalesQty = a.INVSTO_SalesQty,
                                      INVSTO_SalesRetQty = a.INVSTO_SalesRetQty,
                                      INVSTO_ItemConQty = a.INVSTO_ItemConQty,
                                      INVSTO_MatIssPlusQty = a.INVSTO_MatIssPlusQty,
                                      INVSTO_MatIssMinusQty = a.INVSTO_MatIssMinusQty,
                                      INVSTO_PhyPlusQty = a.INVSTO_PhyPlusQty,
                                      INVSTO_PhyMinQty = a.INVSTO_PhyMinQty,
                                      INVSTO_AvaiableStock = a.INVSTO_AvaiableStock,

                                  }).Distinct().OrderBy(m => m.INVSTO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Stock load Page:" + ex.Message);
            }
            return data;
        }

        //--------------------------SAVE
        public INV_StockDTO savedetails(INV_StockDTO data)
        {
            try
            {
                foreach (var s in data.stocklist)
                {

                    if (s.INVSTO_Id != 0)
                    {
                        var res = _INVContext.INV_StockDMO.Where(t => t.INVMST_Id == data.INVMST_Id && t.INVMI_Id == data.INVMI_Id && t.INVSTO_PurchaseDate == data.INVSTO_PurchaseDate && t.INVSTO_BatchNo == data.INVSTO_BatchNo && t.MI_Id == data.MI_Id && t.INVSTO_Id != data.INVSTO_Id).ToList();
                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result = _INVContext.INV_StockDMO.Single(t => t.INVSTO_Id == s.INVSTO_Id);
                            result.MI_Id = data.MI_Id;
                            result.INVSTO_Id = s.INVSTO_Id;
                            result.INVMST_Id = s.INVMST_Id;
                            result.INVMI_Id = s.INVMI_Id;
                            result.INVSTO_BatchNo = s.INVSTO_BatchNo;
                            result.INVSTO_PurchaseDate = s.INVSTO_PurchaseDate;
                            result.INVSTO_PurchaseRate = s.INVSTO_PurchaseRate;
                            result.INVSTO_SalesRate = s.INVSTO_SalesRate;
                            result.INVSTO_PurOBQty = s.INVSTO_PurOBQty;
                            result.INVSTO_PurRetQty = s.INVSTO_PurRetQty;
                            result.INVSTO_SalesQty = s.INVSTO_SalesQty;
                            result.INVSTO_SalesRetQty = s.INVSTO_SalesRetQty;
                            result.INVSTO_ItemConQty = s.INVSTO_ItemConQty;
                            result.INVSTO_MatIssPlusQty = s.INVSTO_MatIssPlusQty;
                            result.INVSTO_MatIssMinusQty = s.INVSTO_MatIssMinusQty;
                            result.INVSTO_PhyPlusQty = s.INVSTO_PhyPlusQty;
                            result.INVSTO_PhyMinQty = s.INVSTO_PhyMinQty;
                            result.INVSTO_AvaiableStock = s.INVSTO_AvaiableStock;
                            result.UpdatedDate = DateTime.Now;
                            _INVContext.Update(result);
                        }
                    }
                    else
                    {
                        var res = _INVContext.INV_StockDMO.Where(t => t.INVMST_Id == data.INVMST_Id && t.INVMI_Id == data.INVMI_Id && t.INVSTO_PurchaseDate == data.INVSTO_PurchaseDate && t.INVSTO_BatchNo == data.INVSTO_BatchNo && t.MI_Id == data.MI_Id).ToList();
                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            INV_StockDMO stok = new INV_StockDMO();
                            stok.MI_Id = data.MI_Id;
                            stok.INVMST_Id = s.INVMST_Id;
                            stok.INVMI_Id = s.INVMI_Id;
                            stok.INVSTO_PurchaseDate = s.INVSTO_PurchaseDate;
                            stok.INVSTO_PurchaseRate = s.INVSTO_PurchaseRate;
                            stok.INVSTO_BatchNo = s.INVSTO_BatchNo;
                            stok.INVSTO_SalesRate = s.INVSTO_SalesRate;
                            stok.INVSTO_PurOBQty = s.INVSTO_PurOBQty;
                            stok.INVSTO_PurRetQty = s.INVSTO_PurRetQty;
                            stok.INVSTO_SalesQty = s.INVSTO_SalesQty;
                            stok.INVSTO_SalesRetQty = s.INVSTO_SalesRetQty;
                            stok.INVSTO_ItemConQty = s.INVSTO_ItemConQty;
                            stok.INVSTO_MatIssPlusQty = s.INVSTO_MatIssPlusQty;
                            stok.INVSTO_MatIssMinusQty = s.INVSTO_MatIssMinusQty;
                            stok.INVSTO_PhyPlusQty = s.INVSTO_PhyPlusQty;
                            stok.INVSTO_PhyMinQty = s.INVSTO_PhyMinQty;
                            stok.INVSTO_AvaiableStock = s.INVSTO_AvaiableStock;
                            stok.CreatedDate = DateTime.Now;
                            stok.UpdatedDate = DateTime.Now;
                            _INVContext.Add(stok);
                        }
                    }
                }
                var contactExists = _INVContext.SaveChanges();
                if (contactExists > 0)
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
                _logInv.LogInformation("Stock savedata :" + ex.Message);
            }
            return data;
        }


        public INV_StockDTO editStock(INV_StockDTO data)
        {
            try
            {
                data.get_editstock = (from a in _INVContext.INV_StockDMO
                                      from b in _INVContext.INV_Master_StoreDMO
                                      from c in _INVContext.INV_Master_ItemDMO
                                      where (a.INVMST_Id == b.INVMST_Id && a.INVMI_Id == c.INVMI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVSTO_Id == data.INVSTO_Id)
                                      select new INV_StockDTO
                                      {
                                          INVSTO_Id = a.INVSTO_Id,
                                          INVMS_StoreName = b.INVMS_StoreName,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVMST_Id = b.INVMST_Id,
                                          INVMI_Id = c.INVMI_Id,
                                          INVSTO_PurchaseDate = a.INVSTO_PurchaseDate,
                                          INVSTO_PurchaseRate = a.INVSTO_PurchaseRate,
                                          INVSTO_BatchNo = a.INVSTO_BatchNo,
                                          INVSTO_SalesRate = a.INVSTO_SalesRate,
                                          INVSTO_PurOBQty = a.INVSTO_PurOBQty,
                                          INVSTO_PurRetQty = a.INVSTO_PurRetQty,
                                          INVSTO_SalesQty = a.INVSTO_SalesQty,
                                          INVSTO_SalesRetQty = a.INVSTO_SalesRetQty,
                                          INVSTO_ItemConQty = a.INVSTO_ItemConQty,
                                          INVSTO_MatIssPlusQty = a.INVSTO_MatIssPlusQty,
                                          INVSTO_MatIssMinusQty = a.INVSTO_MatIssMinusQty,
                                          INVSTO_PhyPlusQty = a.INVSTO_PhyPlusQty,
                                          INVSTO_PhyMinQty = a.INVSTO_PhyMinQty,
                                          INVSTO_AvaiableStock = a.INVSTO_AvaiableStock,

                                      }).Distinct().OrderBy(m => m.INVSTO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Stock load Page:" + ex.Message);
            }

            return data;
        }




    }
}
