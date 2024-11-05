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

namespace InventoryServicesHub.com.vaps.Implementation
{
    public class INV_MasterItemImpl : Interface.INV_MasterItemInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_MasterItemImpl> _logInv;
        public INV_MasterItemImpl(InventoryContext InvContext, ILogger<INV_MasterItemImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_Master_ItemDTO getloaddata(INV_Master_ItemDTO data)
        {
            try
            {
                data.get_itemgroup = _INVContext.INV_Master_GroupDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMG_ActiveFlg == true).OrderBy(m => m.INVMG_Id).ToArray();
                data.get_tax = _INVContext.INV_Master_TaxDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMT_ActiveFlg == true).OrderBy(m => m.INVMT_Id).ToArray();
                data.get_UOM = _INVContext.INV_Master_UOMDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMUOM_ActiveFlg == true).OrderBy(m => m.INVMUOM_Id).ToArray();

                data.get_item = (from a in _INVContext.INV_Master_ItemDMO
                                 from b in _INVContext.INV_Master_GroupDMO
                                 from c in _INVContext.INV_Master_UOMDMO
                                 where (a.INVMG_Id == b.INVMG_Id && a.INVMUOM_Id == c.INVMUOM_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id)
                                 select new INV_Master_ItemDTO
                                 {
                                     INVMI_Id = a.INVMI_Id,
                                     INVMG_Id = a.INVMG_Id,
                                     INVMUOM_Id = a.INVMUOM_Id,
                                     INVMG_GroupName = b.INVMG_GroupName,
                                     INVMUOM_UOMName = c.INVMUOM_UOMName,
                                     INVMI_ItemName = a.INVMI_ItemName,
                                     INVMI_ItemCode = a.INVMI_ItemCode,
                                     INVMI_MaxStock = a.INVMI_MaxStock,
                                     INVMI_ReorderStock = a.INVMI_ReorderStock,
                                     INVMI_HSNCode = a.INVMI_HSNCode,
                                     INVMI_ForSaleFlg = a.INVMI_ForSaleFlg,
                                     INVMI_RawMatFlg = a.INVMI_RawMatFlg,
                                     INVMI_MaintenanceAplFlg = a.INVMI_MaintenanceAplFlg,
                                     INVMI_TaxAplFlg = a.INVMI_TaxAplFlg,
                                     INVMI_ActiveFlg = a.INVMI_ActiveFlg,
                                     INVMI_GroupItemNo = a.INVMI_GroupItemNo
                                 }).Distinct().OrderByDescending(m => m.INVMI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Item load Page:" + ex.Message);
            }
            return data;
        }

        //--------------------------SAVE
        public INV_Master_ItemDTO savedetails(INV_Master_ItemDTO data)
        {
            try
            {
                string autono = "";
                if (data.INVMI_Id != 0)
                {
                    var res = _INVContext.INV_Master_ItemDMO.Where(t => t.INVMI_ItemName == data.INVMI_ItemName && t.INVMG_Id == data.INVMG_Id && t.INVMI_ItemCode == data.INVMI_ItemCode && t.INVMUOM_Id == data.INVMUOM_Id && t.MI_Id == data.MI_Id && t.INVMI_Id != data.INVMI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_ItemDMO.Single(t => t.INVMI_Id == data.INVMI_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMG_Id = data.INVMG_Id;
                        result.INVMUOM_Id = data.INVMUOM_Id;
                        result.INVMI_ItemName = data.INVMI_ItemName;
                        result.INVMI_MaxStock = data.INVMI_MaxStock;

                        result.INVMI_TaxAplFlg = data.INVMI_TaxAplFlg;
                        result.INVMI_ItemCode = data.INVMI_ItemCode;
                        result.INVMI_ReorderStock = data.INVMI_ReorderStock;

                        result.INVMI_RawMatFlg = data.INVMI_RawMatFlg;
                        result.INVMI_ForSaleFlg = data.INVMI_ForSaleFlg;
                        result.INVMI_MaintenanceAplFlg = data.INVMI_MaintenanceAplFlg;
                        result.INVMI_HSNCode = data.INVMI_HSNCode;

                        result.INVMI_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _INVContext.Update(result);

                        if (data.INVMI_TaxAplFlg == true)
                        {
                            foreach (var x in data.tax_Applicable)
                            {
                                var res1 = _INVContext.INV_Master_Item_TaxDMO.Where(a => a.INVMIT_Id == x.INVMIT_Id).ToList();
                                if (res1.Count > 0)
                                {
                                    var res11 = _INVContext.INV_Master_Item_TaxDMO.Single(a => a.INVMIT_Id == x.INVMIT_Id);
                                    res11.INVMIT_TaxValue = x.INVMIT_TaxValue;
                                    res11.UpdatedDate = DateTime.Now;
                                    _INVContext.Update(res11);
                                }
                                else
                                {
                                    INV_Master_Item_TaxDMO taxper = new INV_Master_Item_TaxDMO();
                                    taxper.INVMI_Id = result.INVMI_Id;
                                    taxper.INVMT_Id = x.INVMT_Id;
                                    taxper.INVMIT_TaxValue = x.INVMIT_TaxValue;
                                    taxper.INVMIT_ActiveFlg = true;
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
                    var res = _INVContext.INV_Master_ItemDMO.Where(t => t.INVMI_ItemName == data.INVMI_ItemName && t.INVMG_Id == data.INVMG_Id && t.INVMI_ItemCode == data.INVMI_ItemCode && t.INVMUOM_Id == data.INVMUOM_Id && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                         autono = GenerateReferenceNo(data.MI_Id, data.INVMG_Id);

                        INV_Master_ItemDMO item = new INV_Master_ItemDMO();
                        item.MI_Id = data.MI_Id;
                        item.INVMG_Id = data.INVMG_Id;
                        item.INVMUOM_Id = data.INVMUOM_Id;
                        item.INVMI_ItemName = data.INVMI_ItemName;
                        item.INVMI_MaxStock = data.INVMI_MaxStock;

                        item.INVMI_TaxAplFlg = data.INVMI_TaxAplFlg;
                        item.INVMI_ItemCode = data.INVMI_ItemCode;
                        item.INVMI_ReorderStock = data.INVMI_ReorderStock;

                        item.INVMI_RawMatFlg = data.INVMI_RawMatFlg;
                        item.INVMI_ForSaleFlg = data.INVMI_ForSaleFlg;
                        item.INVMI_MaintenanceAplFlg = data.INVMI_MaintenanceAplFlg;
                        item.INVMI_HSNCode = data.INVMI_HSNCode;
                        item.INVMI_GroupItemNo = autono;

                        item.INVMI_ActiveFlg = true;
                        item.CreatedDate = DateTime.Now;
                        item.UpdatedDate = DateTime.Now;
                        _INVContext.Add(item);

                        if (data.INVMI_TaxAplFlg == true)
                        {
                            foreach (var x in data.tax_Applicable)
                            {
                                INV_Master_Item_TaxDMO taxper = new INV_Master_Item_TaxDMO();
                                taxper.INVMI_Id = item.INVMI_Id;
                                taxper.INVMT_Id = x.INVMT_Id;
                                taxper.INVMIT_TaxValue = x.INVMIT_TaxValue;
                                taxper.INVMIT_ActiveFlg = true;
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
                _logInv.LogInformation("Item savedata :" + ex.Message);
            }
            return data;
        }

        public INV_Master_ItemDTO deactive(INV_Master_ItemDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_ItemDMO.Single(t => t.INVMI_Id == data.INVMI_Id);

                if (result.INVMI_ActiveFlg == true)
                {
                    result.INVMI_ActiveFlg = false;
                }
                else if (result.INVMI_ActiveFlg == false)
                {
                    result.INVMI_ActiveFlg = true;
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

        public INV_Master_ItemDTO deactiveitax(INV_Master_ItemDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_Item_TaxDMO.Single(t => t.INVMIT_Id == data.INVMIT_Id);

                if (result.INVMIT_ActiveFlg == true)
                {
                    result.INVMIT_ActiveFlg = false;
                }
                else if (result.INVMIT_ActiveFlg == false)
                {
                    result.INVMIT_ActiveFlg = true;
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


        
        public INV_Master_ItemDTO itemTax(INV_Master_ItemDTO data)
        {
            try
            {
                data.get_tax = _INVContext.INV_Master_TaxDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMT_ActiveFlg == true).OrderBy(m => m.INVMT_Id).ToArray();
                data.griditemTax = (from a in _INVContext.INV_Master_Item_TaxDMO
                                   from b in _INVContext.INV_Master_ItemDMO
                                    from c in _INVContext.INV_Master_TaxDMO

                                    where (a.INVMI_Id == b.INVMI_Id && a.INVMT_Id == c.INVMT_Id && b.MI_Id == c.MI_Id && b.MI_Id == data.MI_Id && a.INVMI_Id == data.INVMI_Id)
                                    select new INV_Master_ItemDTO
                                    {
                                        INVMI_Id = a.INVMI_Id,
                                        INVMIT_Id = a.INVMIT_Id,
                                        INVMI_ItemName = b.INVMI_ItemName,
                                        INVMIT_TaxValue = a.INVMIT_TaxValue,
                                        INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                        INVMT_TaxName = c.INVMT_TaxName,
                                        INVMT_Id = a.INVMT_Id,
                                        INVMIT_ActiveFlg = a.INVMIT_ActiveFlg

                                    }).Distinct().OrderBy(t => t.INVMIT_Id).ToArray();

                data.get_itemTax = (from a in _INVContext.INV_Master_Item_TaxDMO
                                    from b in _INVContext.INV_Master_ItemDMO
                                    from c in _INVContext.INV_Master_TaxDMO

                                    where (a.INVMI_Id == b.INVMI_Id && a.INVMT_Id == c.INVMT_Id && b.MI_Id == c.MI_Id && a.INVMIT_ActiveFlg==true && b.MI_Id == data.MI_Id && a.INVMI_Id == data.INVMI_Id)
                                    select new INV_Master_ItemDTO
                                    {
                                        INVMI_Id = a.INVMI_Id,
                                        INVMIT_Id = a.INVMIT_Id,
                                        INVMI_ItemName = b.INVMI_ItemName,
                                        INVMIT_TaxValue = a.INVMIT_TaxValue,
                                        INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                        INVMT_TaxName = c.INVMT_TaxName,
                                        INVMT_Id = a.INVMT_Id,
                                        INVMIT_ActiveFlg = a.INVMIT_ActiveFlg

                                    }).Distinct().OrderBy(t => t.INVMIT_Id).ToArray();



            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Item Tax :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }

        public string GenerateReferenceNo(long MI_Id, long INVMG_Id)
        {
            string AutoGeneratedNo = "";
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GroupWise_AutogenerationNo";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@INVMG_Id", SqlDbType.VarChar) { Value = INVMG_Id });
                    cmd.Parameters.Add(new SqlParameter("@AutoGenerateNo", SqlDbType.VarChar, Int32.MaxValue) { Direction = ParameterDirection.Output });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var data1 = cmd.ExecuteNonQuery();

                    AutoGeneratedNo = cmd.Parameters["@AutoGenerateNo"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                AutoGeneratedNo = "Error";
                Console.WriteLine(ex.Message);
            }

            return AutoGeneratedNo;
        }


    }
}
