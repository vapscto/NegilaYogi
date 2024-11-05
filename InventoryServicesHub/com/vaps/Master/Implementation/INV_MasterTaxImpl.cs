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
    public class INV_MasterTaxImpl : Interface.INV_MasterTaxInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_MasterTaxImpl> _logInv;
        public INV_MasterTaxImpl(InventoryContext InvContext, ILogger<INV_MasterTaxImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_Master_TaxDTO getloaddata(INV_Master_TaxDTO data)
        {
            try
            {
                data.get_tax = _INVContext.INV_Master_TaxDMO.Where(m => m.MI_Id == data.MI_Id).OrderBy(m => m.INVMT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Tax load Page:" + ex.Message);
            }
            return data;
        }

        //--------------------------SAVE
        public INV_Master_TaxDTO savedetails(INV_Master_TaxDTO data)
        {
            try
            {
                if (data.INVMT_Id != 0)
                {
                    var res = _INVContext.INV_Master_TaxDMO.Where(t => t.INVMT_TaxName == data.INVMT_TaxName && t.MI_Id == data.MI_Id &&
                    t.INVMT_Id != data.INVMT_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_TaxDMO.Single(t => t.INVMT_Id == data.INVMT_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMT_TaxName = data.INVMT_TaxName;
                        result.INVMT_TaxAliasName = data.INVMT_TaxAliasName;                      
                        result.INVMT_ActiveFlg = true;
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
                    var res = _INVContext.INV_Master_TaxDMO.Where(t => (t.INVMT_TaxName == data.INVMT_TaxName) && t.MI_Id == data.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_TaxDMO tax = new INV_Master_TaxDMO();
                        tax.MI_Id = data.MI_Id;
                        tax.INVMT_TaxName = data.INVMT_TaxName;
                        tax.INVMT_TaxAliasName = data.INVMT_TaxAliasName;

                        tax.INVMT_ActiveFlg = true;
                        tax.CreatedDate = DateTime.Now;
                        tax.UpdatedDate = DateTime.Now;
                        _INVContext.Add(tax);

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
                _logInv.LogInformation("Tax savedata :" + ex.Message);
            }
            return data;
        }

        public INV_Master_TaxDTO deactive(INV_Master_TaxDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_TaxDMO.Single(t => t.INVMT_Id == data.INVMT_Id);

                if (result.INVMT_ActiveFlg == true)
                {
                    result.INVMT_ActiveFlg = false;
                }
                else if (result.INVMT_ActiveFlg == false)
                {
                    result.INVMT_ActiveFlg = true;
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
