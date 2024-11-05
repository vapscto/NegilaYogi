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
    public class INVMasterCategoryImpl : Interface.INVMasterCategoryInterface
    {
        public InventoryContext _INVContext;
        ILogger<INVMasterCategoryImpl> _logInv;
        public INVMasterCategoryImpl(InventoryContext InvContext, ILogger<INVMasterCategoryImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INVMasterCategoryDTO getloaddata(INVMasterCategoryDTO data)
        {
            try
            {
                data.categorylist = _INVContext.INV_Master_CategoryDMO.Where(m => m.MI_Id == data.MI_Id).OrderByDescending(m => m.INVMC_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation(" load Page:" + ex.Message);
            }
            return data;
        }

        //--------------------------SAVE
        public INVMasterCategoryDTO savedetails(INVMasterCategoryDTO data)
        {
            try
            {
                if (data.INVMC_Id != 0)
                {
                    var res = _INVContext.INV_Master_CategoryDMO.Where(t => t.INVMC_CategoryName == data.INVMC_CategoryName  && t.MI_Id == data.MI_Id &&
                    t.INVMC_Id != data.INVMC_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_CategoryDMO.Single(t => t.INVMC_Id == data.INVMC_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMC_CategoryName = data.INVMC_CategoryName;
                        result.INVMC_AliasName = data.INVMC_AliasName;
                        result.INVMC_ParentId = data.INVMC_ParentId;
                        result.INVMC_ActiveFlg = true;
                        result.INVMC_UpdatedBy = data.User_Id;
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
                    var res = _INVContext.INV_Master_CategoryDMO.Where(t => (t.INVMC_CategoryName == data.INVMC_CategoryName && t.INVMC_ParentId == data.INVMC_ParentId) && t.MI_Id == data.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {

                        long level = 0;

                        var maxlevellist = _INVContext.INV_Master_CategoryDMO.Where(w => w.MI_Id == data.MI_Id).ToList();
                        if (maxlevellist.Count>0)
                        {
                            var maxlevel = _INVContext.INV_Master_CategoryDMO.Where(w => w.MI_Id == data.MI_Id).Max(w => w.INVMC_Level);
                            level = maxlevel;
                        }
                       

                        INV_Master_CategoryDMO uom = new INV_Master_CategoryDMO();
                        uom.MI_Id = data.MI_Id;
                        uom.INVMC_CategoryName = data.INVMC_CategoryName;
                        uom.INVMC_AliasName = data.INVMC_AliasName;
                        uom.INVMC_ParentId = data.INVMC_ParentId;
                        uom.INVMC_Level = level + 1;
                        uom.INVMC_ActiveFlg = true;
                        uom.INVMC_CreatedBy = data.User_Id;
                        uom.INVMC_UpdatedBy = data.User_Id;
                        _INVContext.Add(uom);

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
                _logInv.LogInformation(" savedata :" + ex.Message);
            }
            return data;
        }

        public INVMasterCategoryDTO deactive(INVMasterCategoryDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_CategoryDMO.Single(t => t.INVMC_Id == data.INVMC_Id);

                if (result.INVMC_ActiveFlg == true)
                {
                    result.INVMC_ActiveFlg = false;
                }
                else if (result.INVMC_ActiveFlg == false)
                {
                    result.INVMC_ActiveFlg = true;
                }
                result.INVMC_UpdatedBy = data.User_Id;
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
      

        public INVMasterCategoryDTO getorder(INVMasterCategoryDTO data)
        {
            try
            {
                data.categorylist = _INVContext.INV_Master_CategoryDMO.Where(m => m.MI_Id == data.MI_Id).OrderByDescending(m => m.INVMC_Level).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public INVMasterCategoryDTO saveorder(INVMasterCategoryDTO data)
        {
            try
            {
                int id = 0;
                for (int i = 0; i < data.ordeidss.Count(); i++)
                {
                    var reult = _INVContext.INV_Master_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.INVMC_Id == data.ordeidss[i].INVMC_Id);
                    id = id + 1;

                    if (i == 0)
                    {
                        reult.INVMC_Level = id;
                    }
                    else
                    {
                        reult.INVMC_Level = id;
                    }
                    _INVContext.Update(reult);
                    var flag = _INVContext.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }

    }
}
