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
    public class INV_MasterProductStagesImpl : Interface.INV_MasterProductStagesInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_MasterProductStagesImpl> _logInv;
        public INV_MasterProductStagesImpl(InventoryContext InvContext, ILogger<INV_MasterProductStagesImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_Master_ProductDTO getloaddata(INV_Master_ProductDTO data)
        {
            try
            {
               
                data.get_product = _INVContext.INV_Master_ProductDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMP_ActiveFlg == true).OrderBy(m => m.INVMP_Id).ToArray();

                data.gridproductTax = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id).OrderBy(m => m.INVMST_Id).ToArray(); 

                data.get_productlist = (from a in _INVContext.INV_Master_Product_StagesDMO
                                            from b in _INVContext.INV_Master_ProductDMO
                                            where (a.INVMP_Id == b.INVMP_Id && b.MI_Id == data.MI_Id)
                                            select new INV_Master_ProductDTO
                                            {
                                                INVMPS_Id = a.INVMPS_Id,
                                                INVMP_Id = a.INVMP_Id,
                                                INVMPS_ActiveFlg = a.INVMPS_ActiveFlg,
                                                INVMP_ProductName = b.INVMP_ProductName,
                                                INVMPS_Stages=a.INVMPS_Stages

                                            }).Distinct().OrderBy(m => m.INVMP_Id).ToArray();

                data.get_store_product = (from a in _INVContext.INV_Master_StoreDMO
                                          from b in _INVContext.INV_Master_ProductDMO
                                          from c in _INVContext.DCS_Store_ProductDMO
                                          where (a.INVMST_Id == c.INVMST_Id && b.INVMP_Id==c.INVMP_Id && b.MI_Id == data.MI_Id)
                                        select new INV_Master_ProductDTO
                                        {
                                            DCSSP_Id = c.DCSSP_Id,
                                            INVMP_Id = c.INVMP_Id,
                                            store_name = a.INVMS_StoreName,
                                            INVMP_ProductName = b.INVMP_ProductName,
                                            DCSSP_Activeflag=c.DCSSP_Activeflag,
                                        }).Distinct().OrderBy(m => m.DCSSP_Id).ToArray();





                data.get_item = (from b in _INVContext.INV_Master_ProductDMO
                                 from c in _INVContext.INV_Master_Product_StagesDMO
                                 where (b.INVMP_Id == c.INVMP_Id && b.MI_Id == data.MI_Id)
                                 select new INV_Master_ProductDTO
                                 {
                                     INVMP_Id = b.INVMP_Id,
                                     INVMP_ProductName = b.INVMP_ProductName,

                                 }).Distinct().OrderBy(m => m.INVMPI_Id).ToArray();


                data.get_productItemlist = (from a in _INVContext.INV_Master_Product_StagesDMO
                                        from b in _INVContext.INV_Master_ProductDMO
                                        from c in _INVContext.INV_Master_Product_Stages_StatusDMO
                                            where (a.INVMP_Id == b.INVMP_Id && a.INVMPS_Id==c.INVMPS_Id && b.INVMP_Id==c.INVMP_Id && b.MI_Id == data.MI_Id)
                                        select new INV_Master_ProductDTO
                                        {
                                            INVMPSS_Id=c.INVMPSS_Id,
                                            INVMPS_Id = a.INVMPS_Id,
                                            INVMP_Id = a.INVMP_Id,
                                            INVMPSS_ActiveFlg = c.INVMPSS_ActiveFlg,
                                            INVMP_ProductName = b.INVMP_ProductName,
                                            INVMPS_Stages = a.INVMPS_Stages,
                                            INVMPSS_Status=c.INVMPSS_Status,

                                        }).Distinct().OrderBy(m => m.INVMPSS_Id).ToArray();

                
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
                if (data.INVMPS_Id != 0)
                {
                    var res = _INVContext.INV_Master_Product_StagesDMO.Where(t => t.INVMPS_Stages == data.INVMPS_Stages && t.MI_Id == data.MI_Id && t.INVMPS_Id != data.INVMPS_Id ).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_Product_StagesDMO.Single(t => t.INVMPS_Id == data.INVMPS_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMP_Id = data.INVMP_Id;
                        result.INVMPS_Stages = data.INVMPS_Stages;
                        result.INVMPS_ActiveFlg = true;
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
                    var res = _INVContext.INV_Master_Product_StagesDMO.Where(t => t.INVMPS_Stages == data.INVMPS_Stages && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_Product_StagesDMO product = new INV_Master_Product_StagesDMO();
                        product.MI_Id = data.MI_Id;
                        product.INVMP_Id = data.INVMP_Id;
                        product.INVMPS_Stages = data.INVMPS_Stages;
                        product.INVMPS_ActiveFlg = true;
                        product.CreatedDate = DateTime.Now;
                        product.UpdatedDate = DateTime.Now;
                        _INVContext.Add(product);
                        
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

        public INV_Master_ProductDTO savestoreproduct(INV_Master_ProductDTO data)
        {
            try
            {
                if (data.DCSSP_Id != 0)
                {
                    var res = _INVContext.DCS_Store_ProductDMO.Where(t => t.MI_Id == data.MI_Id && t.DCSSP_Id != data.DCSSP_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.DCS_Store_ProductDMO.Single(t => t.DCSSP_Id == data.DCSSP_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMST_Id = data.INVMST_Id;
                        result.INVMP_Id = data.INVMP_Id;
                        result.DCSSP_Activeflag = data.DCSSP_Activeflag;
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
                    var res = _INVContext.DCS_Store_ProductDMO.Where(t => t.DCSSP_Id == data.DCSSP_Id && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        DCS_Store_ProductDMO product = new DCS_Store_ProductDMO();
                        product.MI_Id = data.MI_Id;
                        product.INVMP_Id = data.INVMP_Id;
                        product.INVMST_Id = data.INVMST_Id;
                        product.DCSSP_Activeflag = true;
                        product.CreatedDate = DateTime.Now;
                        product.UpdatedDate = DateTime.Now;
                        _INVContext.Add(product);

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
                int contactExists = 0;
                foreach (var x in data.product_stage)
                {
                   
                    if (x.INVMPSS_Id != 0)
                    {

                        var res = _INVContext.INV_Master_Product_Stages_StatusDMO.Where(t => t.INVMPSS_Id == x.INVMPSS_Id && t.INVMPS_Id == x.INVMPS_Id && t.INVMPSS_Status == x.INVMPSS_Status && t.INVMP_Id==data.INVMP_Id && t.MI_Id == data.MI_Id && t.INVMPSS_Id != x.INVMPSS_Id).ToList();
                        if (res.Count > 0)
                        {
                            data.returnduplicatestatus = "Duplicate";
                        }


                        var res1 = _INVContext.INV_Master_Product_Stages_StatusDMO.Where(a => a.INVMPSS_Id == x.INVMPSS_Id).ToList();
                        if (res1.Count > 0)
                        {
                            var res11 = _INVContext.INV_Master_Product_Stages_StatusDMO.Single(a => a.INVMPSS_Id == x.INVMPSS_Id);
                            res11.INVMPSS_Status = x.INVMPSS_Status;
                            res11.UpdatedDate = DateTime.Now;
                            _INVContext.Update(res11);
                        }
                        else
                        {
                            INV_Master_Product_Stages_StatusDMO taxper = new INV_Master_Product_Stages_StatusDMO();
                            taxper.MI_Id = data.MI_Id;
                            taxper.INVMPS_Id = x.INVMPS_Id;
                            taxper.INVMPSS_Status = x.INVMPSS_Status;
                            taxper.INVMPSS_ActiveFlg = true;
                            taxper.INVMP_Id = data.INVMP_Id;
                            taxper.CreatedDate = DateTime.Now;
                            taxper.UpdatedDate = DateTime.Now;
                            _INVContext.Add(taxper);
                        }
                         contactExists = _INVContext.SaveChanges();
                    }

                    else
                    {
                       
                                INV_Master_Product_Stages_StatusDMO taxper = new INV_Master_Product_Stages_StatusDMO();
                                taxper.MI_Id = data.MI_Id;
                                taxper.INVMPS_Id = x.INVMPS_Id;
                                taxper.INVMPSS_Status = x.INVMPSS_Status;
                                taxper.INVMPSS_ActiveFlg = true;
                                taxper.INVMP_Id = data.INVMP_Id;
                                taxper.CreatedDate = DateTime.Now;
                                taxper.UpdatedDate = DateTime.Now;

                                _INVContext.Add(taxper);

                             contactExists = _INVContext.SaveChanges();
                    }
                }
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
                _logInv.LogInformation("Product savedata :" + ex.Message);
            }
            return data;
        }


        public INV_Master_ProductDTO deactive(INV_Master_ProductDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_Product_StagesDMO.Single(t => t.INVMPS_Id == data.INVMPS_Id);

                if (result.INVMPS_ActiveFlg == true)
                {
                    result.INVMPS_ActiveFlg = false;
                }
                else if (result.INVMPS_ActiveFlg == false)
                {
                    result.INVMPS_ActiveFlg = true;
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
                var result = _INVContext.INV_Master_Product_Stages_StatusDMO.Single(t => t.INVMPSS_Id == data.INVMPSS_Id);

                if (result.INVMPSS_ActiveFlg == true)
                {
                    result.INVMPSS_ActiveFlg = false;
                }
                else if (result.INVMPSS_ActiveFlg == false)
                {
                    result.INVMPSS_ActiveFlg = true;
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
                data.get_productTax = (from a in _INVContext.INV_Master_Product_StagesDMO
                                        from b in _INVContext.INV_Master_Product_Stages_StatusDMO
                                        where (a.INVMPS_Id == b.INVMPS_Id && b.MI_Id == data.MI_Id && b.INVMPSS_Id == data.INVMPSS_Id)
                                        select new INV_Master_ProductDTO
                                        {
                                            INVMPS_Id = a.INVMPS_Id,
                                            INVMPSS_Id = b.INVMPSS_Id,
                                            INVMPSS_Status = b.INVMPSS_Status,
                                            INVMPS_Stages = a.INVMPS_Stages

                                        }).Distinct().OrderBy(m => m.INVMPSS_Id).ToArray();
                
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Product Tax :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }

        public INV_Master_ProductDTO getstages(INV_Master_ProductDTO data)
        {
            try
            {
                data.get_product = _INVContext.INV_Master_Product_StagesDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMPS_ActiveFlg == true).OrderBy(m => m.INVMPS_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Product Stage :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }
      





    }
}
