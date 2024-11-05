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
    public class INV_MasterUOMImpl : Interface.INV_MasterUOMInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_MasterUOMImpl> _logInv;
        public INV_MasterUOMImpl(InventoryContext InvContext, ILogger<INV_MasterUOMImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_Master_UOMDTO getloaddata(INV_Master_UOMDTO data)
        {
            try
            {
                data.get_uom = _INVContext.INV_Master_UOMDMO.Where(m => m.MI_Id == data.MI_Id).OrderBy(m => m.INVMUOM_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("UOM load Page:" + ex.Message);
            }
            return data;
        }

        //--------------------------SAVE
        public INV_Master_UOMDTO savedetails(INV_Master_UOMDTO data)
        {
            try
            {
                if (data.INVMUOM_Id != 0)
                {
                    var res = _INVContext.INV_Master_UOMDMO.Where(t => t.INVMUOM_UOMName == data.INVMUOM_UOMName && t.INVMUOM_Qty == data.INVMUOM_Qty && t.MI_Id == data.MI_Id &&
                    t.INVMUOM_Id != data.INVMUOM_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_UOMDMO.Single(t => t.INVMUOM_Id == data.INVMUOM_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMUOM_UOMName = data.INVMUOM_UOMName;
                        result.INVMUOM_UOMAliasName = data.INVMUOM_UOMAliasName;
                        result.INVMUOM_Qty = data.INVMUOM_Qty;
                        result.INVMUOM_ActiveFlg = true;
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
                    var res = _INVContext.INV_Master_UOMDMO.Where(t => (t.INVMUOM_UOMName == data.INVMUOM_UOMName && t.INVMUOM_Qty == data.INVMUOM_Qty) && t.MI_Id == data.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_UOMDMO uom = new INV_Master_UOMDMO();
                        uom.MI_Id = data.MI_Id;
                        uom.INVMUOM_UOMName = data.INVMUOM_UOMName;
                        uom.INVMUOM_UOMAliasName = data.INVMUOM_UOMAliasName;
                        uom.INVMUOM_Qty = data.INVMUOM_Qty;
                        uom.INVMUOM_ActiveFlg = true;
                        uom.CreatedDate = DateTime.Now;
                        uom.UpdatedDate = DateTime.Now;
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
                _logInv.LogInformation("Uom savedata :" + ex.Message);
            }
            return data;
        }

        public INV_Master_UOMDTO deactive(INV_Master_UOMDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_UOMDMO.Single(t => t.INVMUOM_Id == data.INVMUOM_Id);

                if (result.INVMUOM_ActiveFlg == true)
                {
                    result.INVMUOM_ActiveFlg = false;
                }
                else if (result.INVMUOM_ActiveFlg == false)
                {
                    result.INVMUOM_ActiveFlg = true;
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
