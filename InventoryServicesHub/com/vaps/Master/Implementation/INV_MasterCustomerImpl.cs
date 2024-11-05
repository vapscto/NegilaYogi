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
    public class INV_MasterCustomerImpl : Interface.INV_MasterCustomerInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_MasterCustomerImpl> _logInv;
        public INV_MasterCustomerImpl(InventoryContext InvContext, ILogger<INV_MasterCustomerImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_Master_CustomerDTO getloaddata(INV_Master_CustomerDTO data)
        {
            try
            {
                data.get_customer = _INVContext.INV_Master_CustomerDMO.Where(m => m.MI_Id == data.MI_Id).OrderBy(m => m.INVMC_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Customer load Page:" + ex.Message);
            }
            return data;
        }

        //--------------------------SAVE
        public INV_Master_CustomerDTO savedetails(INV_Master_CustomerDTO data)
        {
            try
            {
                if (data.INVMC_Id != 0)
                {
                    var res = _INVContext.INV_Master_CustomerDMO.Where(t => t.INVMC_CustomerName == data.INVMC_CustomerName && t.INVMC_CustomerContactNo == data.INVMC_CustomerContactNo && t.INVMC_CustomerContactPerson == data.INVMC_CustomerContactPerson && t.INVMC_CustomerAddress == data.INVMC_CustomerAddress && t.MI_Id == data.MI_Id && t.INVMC_Id != data.INVMC_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_CustomerDMO.Single(t => t.INVMC_Id == data.INVMC_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMC_CustomerName = data.INVMC_CustomerName;
                        result.INVMC_CustomerContactPerson = data.INVMC_CustomerContactPerson;
                        result.INVMC_CustomerContactNo = data.INVMC_CustomerContactNo;
                        result.INVMC_CustomerAddress = data.INVMC_CustomerAddress;
                        result.INVMC_ActiveFlg = true;
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
                    var res = _INVContext.INV_Master_CustomerDMO.Where(t => t.INVMC_CustomerName == data.INVMC_CustomerName && t.INVMC_CustomerContactNo == data.INVMC_CustomerContactNo && t.INVMC_CustomerContactPerson == data.INVMC_CustomerContactPerson && t.INVMC_CustomerAddress == data.INVMC_CustomerAddress && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_CustomerDMO customer = new INV_Master_CustomerDMO();
                        customer.MI_Id = data.MI_Id;
                        customer.INVMC_CustomerName = data.INVMC_CustomerName;
                        customer.INVMC_CustomerContactPerson = data.INVMC_CustomerContactPerson;
                        customer.INVMC_CustomerContactNo = data.INVMC_CustomerContactNo;
                        customer.INVMC_CustomerAddress = data.INVMC_CustomerAddress;
                        customer.INVMC_ActiveFlg = true;
                        customer.CreatedDate = DateTime.Now;
                        customer.UpdatedDate = DateTime.Now;
                        _INVContext.Add(customer);

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
                _logInv.LogInformation("Customer savedata :" + ex.Message);
            }
            return data;
        }

        public INV_Master_CustomerDTO deactive(INV_Master_CustomerDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_CustomerDMO.Single(t => t.INVMC_Id == data.INVMC_Id);

                if (result.INVMC_ActiveFlg == true)
                {
                    result.INVMC_ActiveFlg = false;
                }
                else if (result.INVMC_ActiveFlg == false)
                {
                    result.INVMC_ActiveFlg = true;
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
