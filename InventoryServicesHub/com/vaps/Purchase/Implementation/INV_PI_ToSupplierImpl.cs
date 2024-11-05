using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using CommonLibrary;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using DomainModel.Model.com.vapstech.Purchase.Inventory;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_PI_ToSupplierImpl : Interface.INV_PI_ToSupplierInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PI_ToSupplierImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_PI_ToSupplierImpl(InventoryContext InvContext, ILogger<INV_PI_ToSupplierImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_PI_ToSupplierDTO getloaddata(INV_PI_ToSupplierDTO data)
        {
            try
            {
                data.get_piNo = _INVContext.INV_M_PurchaseIndentDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMPI_ActiveFlg == true).OrderByDescending(m => m.INVMPI_Id).ToArray();
                data.get_supplier = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI TO Supplier load Page:" + ex.Message);
            }
            return data;
        }

        public INV_PI_ToSupplierDTO getpiDetail(INV_PI_ToSupplierDTO data)
        {
            try
            {
                data.get_pidetails = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                      from b in _INVContext.INV_T_PurchaseIndentDMO
                                      from c in _INVContext.INV_Master_ItemDMO
                                      from d in _INVContext.INV_Master_UOMDMO
                                      where (a.INVMPI_Id == b.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                               && a.INVMPI_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMPI_Id == data.INVMPI_Id)
                                      select new INV_PurchaseIndentDTO
                                      {
                                          INVMPI_Id = a.INVMPI_Id,
                                          INVMPI_PINo = a.INVMPI_PINo,
                                          INVMI_Id = b.INVMI_Id,
                                          INVMUOM_Id = b.INVMUOM_Id,
                                          INVMPR_Id = b.INVMPR_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVMUOM_UOMName = d.INVMUOM_UOMName,
                                          INVMPI_PIDate = a.INVMPI_PIDate,
                                          INVMPI_ReferenceNo = a.INVMPI_ReferenceNo,
                                          INVTPI_PRQty = b.INVTPI_PRQty,
                                          INVTPI_PIQty = b.INVTPI_PIQty,
                                          INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                                          INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,
                                          INVMPI_Remarks = a.INVMPI_Remarks,
                                          INVMPI_ApproxTotAmount = a.INVMPI_ApproxTotAmount,
                                          INVMPI_POCreatedFlg = a.INVMPI_POCreatedFlg,
                                          INVMPI_ActiveFlg = a.INVMPI_ActiveFlg,
                                          INVTPI_Id = b.INVTPI_Id,
                                          INVTPI_Remarks = b.INVTPI_Remarks,
                                          INVTPI_ActiveFlg = b.INVTPI_ActiveFlg
                                      }).Distinct().OrderBy(i => i.INVMPI_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Pi To Supplier Pi change:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_PI_ToSupplierDTO> savedetails(INV_PI_ToSupplierDTO data)
        {
            try
            {
                string s = "";
                string m = "";

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
               
                foreach (var sp in data.supplierArray)
                {
                    INV_PurchaseIndent_ToSupplierDMO PITOSP = new INV_PurchaseIndent_ToSupplierDMO();
                    PITOSP.MI_Id = data.MI_Id;
                    PITOSP.INVMPI_Id = data.INVMPI_Id;
                    PITOSP.INVPITS_SupplierName = sp.INVPITS_SupplierName;
                    PITOSP.INVPITS_ContactNo = sp.INVPITS_ContactNo;
                    PITOSP.INVPITS_EmailId = sp.INVPITS_EmailId;
                    PITOSP.INVPITS_SMSSentDate = indianTime;
                    PITOSP.INVPITS_MailSentDate = indianTime;
                    PITOSP.INVPITS_ActiveFlg = true;
                    PITOSP.INVPITS_CreatedBy = data.UserId;
                    PITOSP.INVPITS_UpdatedBy = data.UserId;
                    PITOSP.CreatedDate = indianTime;
                    PITOSP.UpdatedDate = indianTime;
                    _INVContext.Add(PITOSP);

                    var contactExists = _INVContext.SaveChanges();
                    if (contactExists > 0)
                    {
                        var id = _INVContext.INV_PurchaseIndent_ToSupplierDMO.Where(e => e.INVPITS_Id == PITOSP.INVPITS_Id).FirstOrDefault().INVPITS_Id;

                        if (data.sms == "true")
                        {
                            if (sp.INVPITS_ContactNo != 0)
                            {
                                //sp.INVPITS_ContactNo = 9591081840;
                                SMS sms = new SMS(_db);
                                s = await sms.sendPItoSupplierSms(data.MI_Id, sp.INVPITS_ContactNo, "PINotification", data.UserId, id, data.INVMPI_Id);
                            }
                        }

                        if (data.email == "true")
                        {
                            if (sp.INVPITS_EmailId != "")
                            {

                                var indentno = _INVContext.INV_M_PurchaseIndentDMO.Single(w => w.INVMPI_Id == data.INVMPI_Id).INVMPI_PINo;

                               // sp.INVPITS_EmailId = "praveenishwar@vapstech.com";
                                Email Email = new Email(_db);
                                m = Email.sendPItoSupplierEmail_withAtch(data.MI_Id, sp.INVPITS_EmailId, "PINotification", data.UserId, id, data.INVMPI_Id, indentno,data.atchtempl);
                            }
                        }
                        

                        if (s == "success" && m == "success")
                        {
                            data.returnval = true;
                        }
                        else if (s == "success")
                        {
                            data.message = "SMS";
                            data.returnval = true;
                        }
                        else if (m == "success")
                        {
                            data.message = "Email";
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }



                   
                   
                }

                

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("PITOSP savedata :" + ex.Message);
            }

            return data;
        }

        public INV_PI_ToSupplierDTO deactive(INV_PI_ToSupplierDTO data)
        {

            return data;
        }


    }
}
