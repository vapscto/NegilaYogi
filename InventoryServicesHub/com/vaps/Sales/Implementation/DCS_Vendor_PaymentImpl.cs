using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Purchase.Inventory;
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
    public class DCS_Vendor_PaymentImpl : Interface.DCS_Vendor_PaymentInterface
    {
        public InventoryContext _INVContext;
        ILogger<DCS_Vendor_PaymentImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public DCS_Vendor_PaymentImpl(InventoryContext InvContext, ILogger<DCS_Vendor_PaymentImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public async Task<INV_T_SalesDTO> getloaddata(INV_T_SalesDTO data)
        {
            try
            {
                data.get_item = _INVContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id).OrderByDescending(m => m.INVMI_Id).ToArray();
             

                data.get_customer = _INVContext.INV_Master_SupplierDMO.Where(h => h.MI_Id == data.MI_Id && h.INVMS_ActiveFlg == true).OrderBy(h => h.INVMS_Id).Distinct().ToArray();


             


                data.get_Product = (from a in _INVContext.INV_Master_SupplierDMO
                                    from b in _INVContext.DCS_Supplier_PaymentDMO
                                  where (a.INVMS_Id==b.INVMS_Id && b.MI_Id==data.MI_Id)
                                  select new INV_T_SalesDTO
                                  {
                                      DCSSPT_Id = b.DCSSPT_Id,
                                      INVMS_SupplierName = a.INVMS_SupplierName,
                                      INVSPT_PaymentDate=b.INVSPT_PaymentDate,
                                      INVSPT_ModeOfPayment = b.INVSPT_ModeOfPayment,
                                      INVSPT_Amount = b.INVSPT_Amount,
                                      INVSPT_Remarks = b.INVSPT_Remarks,
                                  }).Distinct().OrderByDescending(d => d.INVSPT_PaymentDate).ToArray();


            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sales load Page:" + ex.Message);
            }
            return data;
        }


        public INV_T_SalesDTO getitem(INV_T_SalesDTO data)
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

        public async Task<INV_T_SalesDTO> getitemDetail(INV_T_SalesDTO data)
        {
            try
            {

                var config = _INVContext.INV_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVC_ProcessApplFlg == true).ToList();
                var lifofifo = config.FirstOrDefault().INVC_LIFOFIFOFlg;

                var itemDetail = (from a in _INVContext.DCS_StockDMO
                                  from b in _INVContext.INV_Master_ProductDMO
                                  from c in _INVContext.INV_Master_UOMDMO
                                  from d in _INVContext.DCS_PhysicalStock_UpdationDMO
                                  where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMST_Id == data.INVMST_Id && a.INVMP_Id == data.INVMP_Id && a.INVSTO_SalesRate == data.INVMP_ProductPrice && b.INVMP_ActiveFlg == true && d.INVMP_Id==b.INVMP_Id && d.INVMUOM_Id==c.INVMUOM_Id)
                                  select new INV_T_SalesDTO
                                  {
                                      INVMST_Id = a.INVMST_Id,
                                      INVMP_Id = a.INVMP_Id,
                                      INVSTO_BatchNo = a.INVSTO_BatchNo,
                                      INVMUOM_Id=c.INVMUOM_Id,
                                      INVMUOM_UOMName=c.INVMUOM_UOMName,
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

        public INV_T_SalesDTO savedetails(INV_T_SalesDTO data)
        {

            try
            {
                
                var res = _INVContext.DCS_Supplier_PaymentDMO.Where(t => t.DCSSPT_Id==data.DCSSPT_Id && t.MI_Id == data.MI_Id).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {

                    DCS_Supplier_PaymentDMO sale = new DCS_Supplier_PaymentDMO();
                    sale.MI_Id = data.MI_Id;
                    sale.INVMS_Id = data.INVMST_Id;
                    sale.INVSPT_PaymentDate = data.INVSPT_PaymentDate;
                    sale.INVSPT_ModeOfPayment = data.INVSPT_ModeOfPayment;
                    sale.INVSPT_PaymentReference = data.INVSPT_PaymentReference;
                    sale.INVSPT_ChequeDDNo = data.INVSPT_ChequeDDNo;
                    sale.INVSPT_BankName = data.INVSPT_BankName;
                    sale.INVSPT_ChequeDDDate = data.INVSPT_ChequeDDDate;
                    sale.INVSPT_Amount = data.INVSPT_Amount;
                    sale.INVSPT_Remarks = data.INVMSL_Remarks;
                    sale.INVSPT_ActiveFlg = true;
                    sale.INVSPT_CreatedBy = data.userid;
                    sale.INVSPT_UpdatedBy = data.userid;
                    sale.CreatedDate = DateTime.Now;
                    sale.UpdatedDate = DateTime.Now;
                    _INVContext.Add(sale);

                  
                    //===================================Sales Transcation Data
                    foreach (var i in data.paymentdto)
                    {
                        DCS_Supplier_Payment_DetailsDMO saleitem = new DCS_Supplier_Payment_DetailsDMO();
                        saleitem.DCSSPT_Id = sale.DCSSPT_Id;
                        saleitem.INVMI_Id = i.INVMI_Id;
                        saleitem.INVSPTGRN_Amount = i.INVSPTGRN_Amount;
                        saleitem.INVSPTGRN_Remarks = i.INVSPTGRN_Remarks;
                        saleitem.INVSPTGRN_ActiveFlg = true;
                        saleitem.CreatedDate = DateTime.Now;
                        saleitem.UpdatedDate = DateTime.Now;
                        _INVContext.Add(saleitem);
                      
                        using (var dbSaleTxn = _INVContext.Database.BeginTransaction())
                        {
                            var contexttrans = _INVContext.SaveChanges();
                            dbSaleTxn.Commit();

                            if (contexttrans > 0)
                            {
                                
                              data.returnduplicatestatus = "saved";
                                data.returnval = true;
                            }
                            else
                            {
                                dbSaleTxn.Rollback();
                                data.returnval = false;
                            }
                        }
                    }
                  
                }

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Sales savedata :" + ex.Message);
            }

            return data;
        }

        //====================================Grid Model Data
        public async Task<INV_T_SalesDTO> getSaletypes(INV_T_SalesDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "DCS_SALE_TYPES_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@DCSMSL_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.DCSMSL_Id
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
                        data.get_Saletypes = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sale Types :" + ex.Message);
            }
            return data;
        }
        public async Task<INV_T_SalesDTO> getbilldetails(INV_T_SalesDTO data)
        {
            try
            {
                data.get_SaleItemDetails = (from a in _INVContext.DCS_Supplier_PaymentDMO
                                            from b in _INVContext.DCS_Supplier_Payment_DetailsDMO
                                            from c in _INVContext.INV_Master_ItemDMO
                                            where (a.DCSSPT_Id==b.DCSSPT_Id && a.MI_Id==data.MI_Id && a.DCSSPT_Id==data.DCSSPT_Id && c.INVMI_Id==b.INVMI_Id)
                                            select new INV_T_SalesDTO
                                            {
                                                DCSSPT_Id = a.DCSSPT_Id,
                                                DCSSPTD_Id = b.DCSSPTD_Id,
                                                INVMI_Id = b.INVMI_Id,
                                                INVSPT_PaymentDate=a.INVSPT_PaymentDate,
                                                INVMI_ItemName=c.INVMI_ItemName,
                                                INVMP_ProductPrice = b.INVSPTGRN_Amount,
                                                INVSPT_Remarks = b.INVSPTGRN_Remarks,
                                            }).Distinct().OrderBy(m => m.DCSSPT_Id).ToArray();


            }
            catch (Exception ex)
            {
                _logInv.LogInformation("sale Model:" + ex.Message);
            }
            return data;
        }

        public INV_T_SalesDTO getSaleItemTax(INV_T_SalesDTO data)
        {
            try
            {
                data.get_SaleItemTax = (from a in _INVContext.INV_T_Sales_TaxDMO
                                        from b in _INVContext.DCS_T_SalesDMO
                                        from c in _INVContext.INV_Master_TaxDMO
                                        where (a.INVTSL_Id == b.DCSTSL_Id && a.INVMT_Id == c.INVMT_Id && c.MI_Id == data.MI_Id && a.INVTSL_Id == data.DCSTSL_Id)
                                        select new INV_T_SalesDTO
                                        {
                                            INVTSLT_Id = a.INVTSLT_Id,
                                            DCSTSL_Id = a.INVTSL_Id,
                                            INVMT_Id = a.INVMT_Id,
                                            INVMT_TaxName = c.INVMT_TaxName,
                                            INVTSLT_TaxPer = a.INVTSLT_TaxPer,
                                            INVTSLT_TaxAmt = a.INVTSLT_TaxAmt,
                                            INVTSLT_ActiveFlg = a.INVTSLT_ActiveFlg

                                        }).Distinct().OrderBy(m => m.INVTSLT_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("sale Tax Model:" + ex.Message);
            }
            return data;
        }
        //====================================Grid Activate and Deactivate
        public INV_T_SalesDTO deactive(INV_T_SalesDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_SalesDMO.Single(t => t.INVMSL_Id == data.INVMSL_Id && t.MI_Id == data.MI_Id);

                if (result.INVMSL_ActiveFlg == true)
                {
                    result.INVMSL_ActiveFlg = false;
                }
                else if (result.INVMSL_ActiveFlg == false)
                {
                    result.INVMSL_ActiveFlg = true;
                }
                var resultt = _INVContext.DCS_T_SalesDMO.Where(t => t.DCSMSL_Id == data.INVMSL_Id).ToList();
                foreach (var r in resultt)
                {
                    if (result.INVMSL_ActiveFlg == true)
                    {
                        r.INVTSL_ActiveFlg = true;
                    }
                    else if (result.INVMSL_ActiveFlg == false)
                    {
                        r.INVTSL_ActiveFlg = false;
                    }

                    // if (r.INVTSL_ActiveFlg == true)
                    //{
                    // r.INVTSL_ActiveFlg = false;
                    //}
                    //else if (r.INVTSL_ActiveFlg == false)
                    //{
                    //    r.INVTSL_ActiveFlg = true;
                    //}
                    r.UpdatedDate = DateTime.Now;
                    _INVContext.Update(r);
                }
                var saletrans = (from a in _INVContext.INV_M_SalesDMO
                                 from b in _INVContext.DCS_T_SalesDMO
                                 where (a.INVMSL_Id == b.DCSMSL_Id && a.INVMSL_Id == data.INVMSL_Id)
                                 select new INV_T_SalesDTO
                                 {
                                     INVMSL_Id = a.INVMSL_Id,
                                     DCSTSL_Id = b.DCSTSL_Id,
                                     INVMSL_StuOtherFlg = a.INVMSL_StuOtherFlg,
                                 }).Distinct().OrderBy(m => m.INVMSL_Id).ToList();
                foreach (var sid in saletrans)
                {
                    var resulttax = _INVContext.INV_T_Sales_TaxDMO.Where(t => t.INVTSL_Id == sid.DCSTSL_Id).ToList();
                    foreach (var rtax in resulttax)
                    {
                        if (result.INVMSL_ActiveFlg == true)
                        {
                            rtax.INVTSLT_ActiveFlg = true;
                        }
                        else if (result.INVMSL_ActiveFlg == false)
                        {
                            rtax.INVTSLT_ActiveFlg = false;
                        }

                        //if (rtax.INVTSLT_ActiveFlg == true)
                        //{
                        //    rtax.INVTSLT_ActiveFlg = false;
                        //}
                        //else if (rtax.INVTSLT_ActiveFlg == false)
                        //{
                        //    rtax.INVTSLT_ActiveFlg = true;
                        //}
                        rtax.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rtax);
                    }
                }
                var type = _INVContext.INV_M_SalesDMO.Where(t => t.INVMSL_Id == data.INVMSL_Id).ToList();
                foreach (var typ in type)
                {
                    if (typ.INVMSL_StuOtherFlg == "Staff")
                    {
                        var salestaff = (from a in _INVContext.INV_M_SalesDMO
                                         from b in _INVContext.INV_M_Sales_StaffDMO
                                         where (a.INVMSL_Id == b.INVMSL_Id && a.INVMSL_Id == data.INVMSL_Id)
                                         select new INV_T_SalesDTO
                                         {
                                             INVMSL_Id = a.INVMSL_Id,
                                             INVMSLST_Id = b.INVMSLST_Id,
                                         }).Distinct().OrderBy(m => m.INVMSLST_Id).ToList();

                        foreach (var salestf in salestaff)
                        {
                            var resultstaff = _INVContext.INV_M_Sales_StaffDMO.Where(t => t.INVMSLST_Id == salestf.INVMSLST_Id).ToList();
                            foreach (var rstaff in resultstaff)
                            {
                                if (result.INVMSL_ActiveFlg == true)
                                {
                                    rstaff.INVMSLST_ActiveFlg = true;
                                }
                                else if (result.INVMSL_ActiveFlg == false)
                                {
                                    rstaff.INVMSLST_ActiveFlg = false;
                                }
                                //if (rstaff.INVMSLST_ActiveFlg == true)
                                //{
                                //    rstaff.INVMSLST_ActiveFlg = false;
                                //}
                                //else if (rstaff.INVMSLST_ActiveFlg == false)
                                //{
                                //    rstaff.INVMSLST_ActiveFlg = true;
                                //}
                                rstaff.UpdatedDate = DateTime.Now;
                                _INVContext.Update(rstaff);
                            }
                        }
                    }

                    else if (typ.INVMSL_StuOtherFlg == "Student")
                    {
                        var salestudent = (from a in _INVContext.INV_M_SalesDMO
                                           from b in _INVContext.INV_M_Sales_StudentDMO
                                           where (a.INVMSL_Id == b.INVMSL_Id && a.INVMSL_Id == data.INVMSL_Id)
                                           select new INV_T_SalesDTO
                                           {
                                               INVMSL_Id = a.INVMSL_Id,
                                               INVMSLS_Id = b.INVMSLS_Id,
                                           }).Distinct().OrderBy(m => m.INVMSLS_Id).ToList();

                        foreach (var salestu in salestudent)
                        {
                            var resultstu = _INVContext.INV_M_Sales_StudentDMO.Where(t => t.INVMSLS_Id == salestu.INVMSLS_Id).ToList();
                            foreach (var rstu in resultstu)
                            {
                                if (result.INVMSL_ActiveFlg == true)
                                {
                                    rstu.INVMSLS_ActiveFlg = true;
                                }
                                else if (result.INVMSL_ActiveFlg == false)
                                {
                                    rstu.INVMSLS_ActiveFlg = false;
                                }
                                //if (rstu.INVMSLS_ActiveFlg == true)
                                //{
                                //    rstu.INVMSLS_ActiveFlg = false;
                                //}
                                //else if (rstu.INVMSLS_ActiveFlg == false)
                                //{
                                //    rstu.INVMSLS_ActiveFlg = true;
                                //}
                                rstu.UpdatedDate = DateTime.Now;
                                _INVContext.Update(rstu);
                            }
                        }
                    }
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    try
                    {
                        var contextupdate = _INVContext.Database.ExecuteSqlCommand("INV_UpdateStockforDeactiveSales @p0,@p1,@p2,@p3", data.MI_Id, data.IMFY_Id, data.INVMSL_Id, data.INVMST_Id);
                        if (contextupdate > 0)
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public INV_T_SalesDTO deactiveS(INV_T_SalesDTO data)
        {
            try
            {
                var result = _INVContext.DCS_T_SalesDMO.Single(t => t.DCSTSL_Id == data.DCSTSL_Id);

                if (result.INVTSL_ActiveFlg == true)
                {
                    result.INVTSL_ActiveFlg = false;
                }
                else if (result.INVTSL_ActiveFlg == false)
                {
                    result.INVTSL_ActiveFlg = true;
                }

                var resulttx = _INVContext.INV_T_Sales_TaxDMO.Where(t => t.INVTSL_Id == data.DCSTSL_Id).ToList();
                foreach (var tx in resulttx)
                {
                    if (result.INVTSL_ActiveFlg == true)
                    {
                        tx.INVTSLT_ActiveFlg = true;
                    }
                    else if (result.INVTSL_ActiveFlg == false)
                    {
                        tx.INVTSLT_ActiveFlg = false;
                    }

                    tx.UpdatedDate = DateTime.Now;
                    _INVContext.Update(tx);
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
        public INV_T_SalesDTO deactivetax(INV_T_SalesDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_Sales_TaxDMO.Single(t => t.INVTSLT_Id == data.INVTSLT_Id);

                if (result.INVTSLT_ActiveFlg == true)
                {
                    result.INVTSLT_ActiveFlg = false;
                }
                else if (result.INVTSLT_ActiveFlg == false)
                {
                    result.INVTSLT_ActiveFlg = true;
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



    }
}
