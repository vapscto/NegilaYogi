using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Implementation
{
    public class INV_MasterProductImpl : Interface.INV_MasterProductInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_MasterProductImpl> _logInv;
        public INV_MasterProductImpl(InventoryContext InvContext, ILogger<INV_MasterProductImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_Master_ProductDTO getloaddata(INV_Master_ProductDTO data)
        {
            try
            {
                data.get_tax = _INVContext.INV_Master_TaxDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMT_ActiveFlg == true).OrderBy(m => m.INVMT_Id).ToArray();
                data.get_product = _INVContext.INV_Master_ProductDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMP_ActiveFlg == true).OrderBy(m => m.INVMP_Id).ToArray();
                data.get_item = _INVContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMI_ActiveFlg == true).OrderBy(m => m.INVMI_Id).ToArray();
                data.get_productlist = _INVContext.INV_Master_ProductDMO.Where(m => m.MI_Id == data.MI_Id).OrderBy(m => m.INVMP_Id).ToArray();

                data.get_productItemlist = (from a in _INVContext.INV_Master_Product_ItemDMO
                                            from b in _INVContext.INV_Master_ProductDMO
                                            from c in _INVContext.INV_Master_ItemDMO
                                            where (a.INVMP_Id == b.INVMP_Id && a.INVMI_Id == c.INVMI_Id && b.MI_Id == data.MI_Id)
                                            select new INV_Master_ProductDTO
                                            {
                                                INVMI_Id = a.INVMI_Id,
                                                INVMP_Id = a.INVMP_Id,
                                                INVMPI_ActiveFlg = a.INVMPI_ActiveFlg,
                                                INVMPI_Id = a.INVMPI_Id,
                                                INVMP_ProductName = b.INVMP_ProductName,
                                                INVMP_ProductCode = b.INVMP_ProductCode,
                                                INVMI_ItemName = c.INVMI_ItemName,
                                                INVMPI_ItemQty = a.INVMPI_ItemQty,
                                                INVMP_ProductPrice = b.INVMP_ProductPrice,

                                            }).Distinct().OrderBy(m => m.INVMPI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Product load Page:" + ex.Message);
            }
            return data;
        }

        //--------------------------SAVE
        public INV_Master_ProductDTO savedetails(INV_Master_ProductDTO data)
        {
            try
            {
                if (data.INVMP_Id != 0)
                {
                    var res = _INVContext.INV_Master_ProductDMO.Where(t => t.INVMP_ProductName == data.INVMP_ProductName && t.INVMP_ProductCode == data.INVMP_ProductCode && t.MI_Id == data.MI_Id && t.INVMP_Id != data.INVMP_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_ProductDMO.Single(t => t.INVMP_Id == data.INVMP_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMP_ProductName = data.INVMP_ProductName;
                        result.INVMP_ProductCode = data.INVMP_ProductCode;
                        result.INVMP_ProductPrice = data.INVMP_ProductPrice;
                        result.INVMP_TaxApplFlg = data.INVMP_TaxApplFlg;
                        result.INVMP_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _INVContext.Update(result);

                        if (data.INVMP_TaxApplFlg == true)
                        {
                            foreach (var x in data.product_tax)
                            {
                                var res1 = _INVContext.INV_Master_Product_TaxDMO.Where(a => a.INVMPT_Id == x.INVMPT_Id).ToList();
                                if (res1.Count > 0)
                                {
                                    var res11 = _INVContext.INV_Master_Product_TaxDMO.Single(a => a.INVMPT_Id == x.INVMPT_Id);
                                    res11.INVMPT_TaxValue = x.INVMPT_TaxValue;
                                    res11.UpdatedDate = DateTime.Now;
                                    _INVContext.Update(res11);
                                }
                                else
                                {
                                    INV_Master_Product_TaxDMO taxper = new INV_Master_Product_TaxDMO();
                                    taxper.INVMP_Id = result.INVMP_Id;
                                    taxper.INVMT_Id = x.INVMT_Id;
                                    taxper.INVMPT_TaxValue = x.INVMPT_TaxValue;
                                    taxper.INVMPT_ActiveFlg = true;
                                    taxper.CreatedDate = DateTime.Now;
                                    taxper.UpdatedDate = DateTime.Now;
                                    _INVContext.Add(taxper);
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
                }
                else
                {
                    var res = _INVContext.INV_Master_ProductDMO.Where(t => t.INVMP_ProductName == data.INVMP_ProductName && t.INVMP_ProductCode == data.INVMP_ProductCode && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_ProductDMO product = new INV_Master_ProductDMO();
                        product.MI_Id = data.MI_Id;
                        product.INVMP_ProductName = data.INVMP_ProductName;
                        product.INVMP_ProductCode = data.INVMP_ProductCode;
                        product.INVMP_ProductPrice = data.INVMP_ProductPrice;
                        product.INVMP_TaxApplFlg = data.INVMP_TaxApplFlg;
                        product.INVMP_ActiveFlg = true;
                        product.CreatedDate = DateTime.Now;
                        product.UpdatedDate = DateTime.Now;
                        _INVContext.Add(product);

                        if (data.INVMP_TaxApplFlg == true)
                        {
                            foreach (var x in data.product_tax)
                            {
                                INV_Master_Product_TaxDMO taxper = new INV_Master_Product_TaxDMO();
                                taxper.INVMP_Id = product.INVMP_Id;
                                taxper.INVMT_Id = x.INVMT_Id;
                                taxper.INVMPT_TaxValue = x.INVMPT_TaxValue;
                                taxper.INVMPT_ActiveFlg = true;
                                taxper.CreatedDate = DateTime.Now;
                                taxper.UpdatedDate = DateTime.Now;

                                _INVContext.Add(taxper); ;
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
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Product savedata :" + ex.Message);
            }
            return data;
        }
        public INV_Master_ProductDTO savedetailQty(INV_Master_ProductDTO data)
        {
            try
            {
                if (data.INVMPI_Id != 0)
                {
                    var res = _INVContext.INV_Master_Product_ItemDMO.Where(t => t.INVMP_Id == data.INVMP_Id && t.INVMI_Id == data.INVMI_Id && t.INVMPI_ItemQty == data.INVMPI_ItemQty && t.INVMPI_Id != data.INVMPI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_Product_ItemDMO.Single(t => t.INVMPI_Id == data.INVMPI_Id);

                        result.INVMP_Id = data.INVMP_Id;
                        result.INVMI_Id = data.INVMI_Id;
                        result.INVMPI_ItemQty = data.INVMPI_ItemQty;
                        result.INVMPI_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _INVContext.Update(result);

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
                }
                else
                {
                    var res = _INVContext.INV_Master_Product_ItemDMO.Where(t => t.INVMP_Id == data.INVMP_Id && t.INVMI_Id == data.INVMI_Id && t.INVMPI_ItemQty == data.INVMPI_ItemQty).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_Product_ItemDMO productitem = new INV_Master_Product_ItemDMO();
                        productitem.INVMP_Id = data.INVMP_Id;
                        productitem.INVMI_Id = data.INVMI_Id;
                        productitem.INVMPI_ItemQty = data.INVMPI_ItemQty;
                        productitem.INVMPI_ActiveFlg = true;
                        productitem.CreatedDate = DateTime.Now;
                        productitem.UpdatedDate = DateTime.Now;
                        _INVContext.Add(productitem);

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
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Product savedetailQty :" + ex.Message);
            }
            return data;
        }

        public INV_Master_ProductDTO deactive(INV_Master_ProductDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_ProductDMO.Single(t => t.INVMP_Id == data.INVMP_Id);

                if (result.INVMP_ActiveFlg == true)
                {
                    result.INVMP_ActiveFlg = false;
                }
                else if (result.INVMP_ActiveFlg == false)
                {
                    result.INVMP_ActiveFlg = true;
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
        public INV_Master_ProductDTO deactiveQty(INV_Master_ProductDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_Product_ItemDMO.Single(t => t.INVMPI_Id == data.INVMPI_Id);

                if (result.INVMPI_ActiveFlg == true)
                {
                    result.INVMPI_ActiveFlg = false;
                }
                else if (result.INVMPI_ActiveFlg == false)
                {
                    result.INVMPI_ActiveFlg = true;
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
        public INV_Master_ProductDTO deactiveptax(INV_Master_ProductDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_Product_TaxDMO.Single(t => t.INVMPT_Id == data.INVMPT_Id);

                if (result.INVMPT_ActiveFlg == true)
                {
                    result.INVMPT_ActiveFlg = false;
                }
                else if (result.INVMPT_ActiveFlg == false)
                {
                    result.INVMPT_ActiveFlg = true;
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

        public INV_Master_ProductDTO productTax(INV_Master_ProductDTO data)
        {
            try
            {
                data.get_tax = _INVContext.INV_Master_TaxDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMT_ActiveFlg == true).OrderBy(m => m.INVMT_Id).ToArray();
                data.gridproductTax = (from a in _INVContext.INV_Master_Product_TaxDMO
                                       from b in _INVContext.INV_Master_ProductDMO
                                       from c in _INVContext.INV_Master_TaxDMO

                                       where (a.INVMP_Id == b.INVMP_Id && a.INVMT_Id == c.INVMT_Id && b.MI_Id == c.MI_Id && b.MI_Id == data.MI_Id && a.INVMP_Id == data.INVMP_Id)
                                       select new INV_Master_ProductDTO
                                       {
                                           INVMP_Id = a.INVMP_Id,
                                           INVMPT_Id = a.INVMPT_Id,
                                           INVMP_ProductName = b.INVMP_ProductName,
                                           INVMPT_TaxValue = a.INVMPT_TaxValue,
                                           INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                           INVMT_TaxName = c.INVMT_TaxName,
                                           INVMT_Id = a.INVMT_Id,
                                           INVMP_ActiveFlg = b.INVMP_ActiveFlg,
                                           INVMPT_ActiveFlg = a.INVMPT_ActiveFlg

                                       }).Distinct().OrderBy(t => t.INVMPT_Id).ToArray();
                data.get_productTax = (from a in _INVContext.INV_Master_Product_TaxDMO
                                       from b in _INVContext.INV_Master_ProductDMO
                                       from c in _INVContext.INV_Master_TaxDMO

                                       where (a.INVMP_Id == b.INVMP_Id && a.INVMT_Id == c.INVMT_Id && b.MI_Id == c.MI_Id && a.INVMPT_ActiveFlg == true && b.MI_Id == data.MI_Id && a.INVMP_Id == data.INVMP_Id)
                                       select new INV_Master_ProductDTO
                                       {
                                           INVMP_Id = a.INVMP_Id,
                                           INVMPT_Id = a.INVMPT_Id,
                                           INVMP_ProductName = b.INVMP_ProductName,
                                           INVMPT_TaxValue = a.INVMPT_TaxValue,
                                           INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                           INVMT_TaxName = c.INVMT_TaxName,
                                           INVMT_Id = a.INVMT_Id,
                                           INVMP_ActiveFlg = b.INVMP_ActiveFlg,
                                           INVMPT_ActiveFlg = a.INVMPT_ActiveFlg

                                       }).Distinct().OrderBy(t => t.INVMPT_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Product Tax :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }




    }
}
