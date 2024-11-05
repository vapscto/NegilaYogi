using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class PhyStock_UpdationImpl : Interface.PhyStock_UpdationInterface
    {
        public InventoryContext _INVContext;
        ILogger<PhyStock_UpdationImpl> _logInv;
        public PhyStock_UpdationImpl(InventoryContext InvContext, ILogger<PhyStock_UpdationImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_PhyStock_UpdationDTO getloaddata(INV_PhyStock_UpdationDTO data)
        {
            try
            {
                data.get_Store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMST_Id).ToArray();

                data.UOM = _INVContext.INV_Master_UOMDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMUOM_ActiveFlg == true).OrderBy(m => m.INVMUOM_Id).ToArray();
                


                data.get_phyStockdata = (from a in _INVContext.DCS_PhysicalStock_UpdationDMO
                                         from b in _INVContext.INV_Master_StoreDMO
                                         from c in _INVContext.INV_Master_ProductDMO
                                         from d in _INVContext.DCS_StockDMO
                                         from e in _INVContext.INV_Master_UOMDMO
                                         where (a.INVMST_Id == b.INVMST_Id && a.INVMP_Id == c.INVMP_Id && b.INVMST_Id == d.INVMST_Id &&  e.INVMUOM_Id==a.INVMUOM_Id && c.INVMP_Id == d.INVMP_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                         select new INV_PhyStock_UpdationDTO
                                         {
                                             DCSPSU_Id = a.DCSPSU_Id,
                                             INVMST_Id = a.INVMST_Id,
                                             INVMI_Id = a.INVMP_Id,
                                             INVMUOM_Id=a.INVMUOM_Id,
                                             INVMUOM_UOMName=e.INVMUOM_UOMName,
                                             INVMS_StoreName = b.INVMS_StoreName,
                                             INVMP_ProductName = c.INVMP_ProductName,
                                             INVPSU_StockPlus = a.INVPSU_StockPlus,
                                             INVPSU_StockMinus = a.INVPSU_StockMinus,
                                             INVSTO_AvaiableStock = d.INVSTO_AvaiableStock,
                                             INVMP_ProductPrice = d.INVSTO_SalesRate,
                                             INVPSU_Remarks = a.INVPSU_Remarks,
                                             INVPSU_ActiveFlg = a.INVPSU_ActiveFlg
                                         }).Distinct().OrderBy(p => p.DCSPSU_Id).ToArray();
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
                if (data.DCSPSU_Id != 0)
                {
                    foreach (var pu in data.DCSphyStock)
                    {
                        var PSU = _INVContext.DCS_PhysicalStock_UpdationDMO.Single(b => b.MI_Id == data.MI_Id && b.DCSPSU_Id == data.DCSPSU_Id);
                        PSU.MI_Id = data.MI_Id;
                        PSU.INVMST_Id = data.INVMST_Id;
                        PSU.INVMP_Id = pu.INVMP_Id;           // product assigned to item
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
                                var contactExistsO = _INVContext.Database.ExecuteSqlCommand("DCS_InsertPhysicalStock @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, data.INVMST_Id, pu.INVMP_Id, pu.INVMP_ProductPrice, pu.INVPSU_StockPlus, pu.INVPSU_StockMinus);
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
                    foreach (var p in data.DCSphyStock)
                    {
                        DCS_PhysicalStock_UpdationDMO PS = new DCS_PhysicalStock_UpdationDMO();
                        PS.MI_Id = data.MI_Id;
                        PS.INVMST_Id = data.INVMST_Id;
                        PS.INVMP_Id = p.INVMP_Id;
                        PS.INVPSU_StockPlus = p.INVPSU_StockPlus;
                        PS.INVPSU_StockMinus = p.INVPSU_StockMinus;
                        PS.INVPSU_Remarks = p.INVPSU_Remarks;
                        PS.INVPSU_ActiveFlg = true;
                        PS.INVPSU_CreatedBy = data.UserId;
                        PS.INVPSU_UpdatedBy = data.UserId;
                        PS.INVMUOM_Id = data.INVMUOM_Id;
                        PS.UpdatedDate = DateTime.Now;
                        PS.CreatedDate = DateTime.Now;
                        _INVContext.Add(PS);
                        var contextPSU = _INVContext.SaveChanges();
                        if (contextPSU > 0)
                        {
                            try
                            {
                                var contactExistsO = _INVContext.Database.ExecuteSqlCommand("DCS_InsertPhysicalStock @p0,@p1,@p2,@p3,@p4,@p5", data.MI_Id, data.INVMST_Id, p.INVMP_Id, p.INVMP_ProductPrice, p.INVPSU_StockPlus, p.INVPSU_StockMinus);
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
                var result = _INVContext.DCS_PhysicalStock_UpdationDMO.Single(t => t.DCSPSU_Id == data.DCSPSU_Id);
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

        public INV_PhyStock_UpdationDTO getitem(INV_PhyStock_UpdationDTO data)
        {
            try
            {
                var config = _INVContext.INV_Master_ProductDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMP_ActiveFlg == true).ToList();
                data.get_item = config.ToArray();

               
            }
                
            
            catch (Exception ex)
            {
                _logInv.LogInformation("Inventory Sales get Product:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_PhyStock_UpdationDTO> getitemDetail(INV_PhyStock_UpdationDTO data)
        {
            try
            {

                var config = _INVContext.INV_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVC_ProcessApplFlg == true).ToList();
                var lifofifo = config.FirstOrDefault().INVC_LIFOFIFOFlg;
              
                    var itemDetail = (from a in _INVContext.DCS_StockDMO
                                      from b in _INVContext.INV_Master_ProductDMO
                                      where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMST_Id == data.INVMST_Id && a.INVMP_Id == data.INVMP_Id && a.INVSTO_SalesRate == data.INVSTO_SalesRate && b.INVMP_ActiveFlg == true)
                                      select new INV_T_SalesDTO
                                      {
                                          INVMST_Id = a.INVMST_Id,
                                          INVMP_Id = a.INVMP_Id,                                                                INVSTO_BatchNo = a.INVSTO_BatchNo,
                                          INVSTO_SalesRate = a.INVSTO_SalesRate,
                                          INVSTO_PurchaseDate = a.INVSTO_PurchaseDate
                                      }).Distinct().OrderByDescending(d => d.INVSTO_PurchaseDate).ToArray();

                    data.get_itemDetail = itemDetail.ToArray();
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "DCS_AvaiableStock";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMP_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.INVMP_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMP_ProductPrice",
                      SqlDbType.VarChar)
                        {
                            Value = data.INVMP_ProductPrice
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
                    SqlDbType.BigInt)
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
                            data.availablestock = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                
              
                data.get_itemTax = (from a in _INVContext.INV_Master_ItemDMO
                                    from b in _INVContext.INV_Master_Item_TaxDMO
                                    from c in _INVContext.INV_Master_TaxDMO
                                    from d in _INVContext.INV_StockDMO
                                    where (a.INVMI_Id == b.INVMI_Id && b.INVMT_Id == c.INVMT_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == c.MI_Id && a.INVMI_Id == data.INVMI_Id && d.INVSTO_SalesRate == data.INVSTO_SalesRate && a.INVMI_ActiveFlg == true && b.INVMIT_ActiveFlg == true && c.INVMT_ActiveFlg == true)
                                    select new INV_T_SalesDTO
                                    {
                                        INVMI_Id = a.INVMI_Id,
                                        INVMI_ItemName = a.INVMI_ItemName,
                                        INVMT_Id = b.INVMT_Id,
                                        INVMT_TaxName = c.INVMT_TaxName,
                                        INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                        INVMIT_TaxValue = b.INVMIT_TaxValue,
                                    }).Distinct().OrderBy(m => m.INVMT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Product Details:" + ex.Message);
            }
            return data;
        }






    }
}
