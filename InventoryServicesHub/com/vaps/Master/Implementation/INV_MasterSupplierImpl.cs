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
    public class INV_MasterSupplierImpl : Interface.INV_MasterSupplierInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_MasterSupplierImpl> _logInv;
        public INV_MasterSupplierImpl(InventoryContext InvContext, ILogger<INV_MasterSupplierImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_Master_SupplierDTO getloaddata(INV_Master_SupplierDTO data)
        {
            try
            {
                data.get_supplier = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id).OrderBy(m => m.INVMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Supplier load Page:" + ex.Message);
            }
            return data;
        }

        //--------------------------SAVE
        public INV_Master_SupplierDTO savedetails(INV_Master_SupplierDTO data)
        {
            try
            {
                if (data.INVMS_Id != 0)
                {
                    var res = _INVContext.INV_Master_SupplierDMO.Where(t => t.INVMS_SupplierName == data.INVMS_SupplierName && t.INVMS_SupplierConatctNo == data.INVMS_SupplierConatctNo && t.INVMS_EmailId == data.INVMS_EmailId && t.INVMS_SupplierAddress == data.INVMS_SupplierAddress && t.MI_Id == data.MI_Id && t.INVMS_Id != data.INVMS_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_SupplierDMO.Single(t => t.INVMS_Id == data.INVMS_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMS_SupplierName = data.INVMS_SupplierName;
                        result.INVMS_SupplierCode = data.INVMS_SupplierCode;
                        result.INVMS_SupplierConatctPerson = data.INVMS_SupplierConatctPerson;
                        result.INVMS_SupplierConatctNo = data.INVMS_SupplierConatctNo;
                        result.INVMS_EmailId = data.INVMS_EmailId;
                        result.INVMS_GSTNo = data.INVMS_GSTNo;
                        result.INVMS_SupplierAddress = data.INVMS_SupplierAddress;
                        result.INVMS_ActiveFlg = true;
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
                    var res = _INVContext.INV_Master_SupplierDMO.Where(t => t.INVMS_SupplierName == data.INVMS_SupplierName && t.INVMS_SupplierConatctNo == data.INVMS_SupplierConatctNo && t.INVMS_EmailId == data.INVMS_EmailId && t.INVMS_SupplierAddress == data.INVMS_SupplierAddress && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_SupplierDMO supplier = new INV_Master_SupplierDMO();
                        supplier.MI_Id = data.MI_Id;
                        supplier.INVMS_SupplierName = data.INVMS_SupplierName;
                        supplier.INVMS_SupplierCode = data.INVMS_SupplierCode;
                        supplier.INVMS_SupplierConatctPerson = data.INVMS_SupplierConatctPerson;
                        supplier.INVMS_SupplierConatctNo = data.INVMS_SupplierConatctNo;
                        supplier.INVMS_EmailId = data.INVMS_EmailId;
                        supplier.INVMS_GSTNo = data.INVMS_GSTNo;
                        supplier.INVMS_SupplierAddress = data.INVMS_SupplierAddress;
                        supplier.INVMS_ActiveFlg = true;
                        supplier.CreatedDate = DateTime.Now;
                        supplier.UpdatedDate = DateTime.Now;
                        _INVContext.Add(supplier);

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
                _logInv.LogInformation("Supplier savedata :" + ex.Message);
            }
            return data;
        }

        public INV_Master_SupplierDTO deactive(INV_Master_SupplierDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_SupplierDMO.Single(t => t.INVMS_Id == data.INVMS_Id);

                if (result.INVMS_ActiveFlg == true)
                {
                    result.INVMS_ActiveFlg = false;
                }
                else if (result.INVMS_ActiveFlg == false)
                {
                    result.INVMS_ActiveFlg = true;
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
