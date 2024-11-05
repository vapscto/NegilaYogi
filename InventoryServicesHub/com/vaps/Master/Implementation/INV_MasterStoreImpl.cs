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
    public class INV_MasterStoreImpl : Interface.INV_MasterStoreInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_MasterStoreImpl> _logInv;
        public INV_MasterStoreImpl(InventoryContext InvContext, ILogger<INV_MasterStoreImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_Master_StoreDTO getloaddata(INV_Master_StoreDTO data)
        {
            try
            {
                data.empname_list= (from a in _INVContext.MasterEmployee

                                    where (a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.MI_Id == data.MI_Id)
                                    select new INV_Master_StoreDTO
                                    {
                                        employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : a.HRME_EmployeeFirstName)
                                        + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" ? "" : " " + a.HRME_EmployeeMiddleName)
                                        + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                        HRME_Id=a.HRME_Id

                                    }).Distinct().ToList().ToArray();

                data.get_store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id).OrderBy(m => m.INVMST_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Store load Page:" + ex.Message);
            }
            return data;
        }

        //--------------------------SAVE
        public INV_Master_StoreDTO savedetails(INV_Master_StoreDTO data)
        {
            try
            {
                if (data.INVMST_Id != 0)
                {
                    var res = _INVContext.INV_Master_StoreDMO.Where(t => t.INVMS_StoreName == data.INVMS_StoreName && t.INVMS_StoreLocation == data.INVMS_StoreLocation && t.INVMS_ContactPerson == data.INVMS_ContactPerson && t.MI_Id == data.MI_Id && t.INVMST_Id != data.INVMST_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_Master_StoreDMO.Single(t => t.INVMST_Id == data.INVMST_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMS_StoreName = data.INVMS_StoreName;
                        result.INVMS_StoreLocation = data.INVMS_StoreLocation;
                        result.INVMS_ContactPerson = data.INVMS_ContactPerson;
                        result.INVMS_ContactNo = data.INVMS_ContactNo;
                        result.HRME_Id = data.HRME_Id;
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
                    var res = _INVContext.INV_Master_StoreDMO.Where(t => t.INVMS_StoreName == data.INVMS_StoreName && t.INVMS_StoreLocation == data.INVMS_StoreLocation && t.INVMS_ContactPerson == data.INVMS_ContactPerson && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        INV_Master_StoreDMO store = new INV_Master_StoreDMO();
                        store.MI_Id = data.MI_Id;
                        store.INVMS_StoreName = data.INVMS_StoreName;
                        store.INVMS_StoreLocation = data.INVMS_StoreLocation;
                        store.INVMS_ContactPerson = data.INVMS_ContactPerson;
                        store.INVMS_ContactNo = data.INVMS_ContactNo;
                        store.HRME_Id = data.HRME_Id;
                        store.INVMS_ActiveFlg = true;
                        store.CreatedDate = DateTime.Now;
                        store.UpdatedDate = DateTime.Now;
                        _INVContext.Add(store);

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
                _logInv.LogInformation("Store savedata :" + ex.Message);
            }
            return data;
        }

        public INV_Master_StoreDTO deactive(INV_Master_StoreDTO data)
        {
            try
            {
                var result = _INVContext.INV_Master_StoreDMO.Single(t => t.INVMST_Id == data.INVMST_Id);

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
