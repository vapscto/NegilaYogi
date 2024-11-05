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
    public class INV_PurchaseOrderImpl : Interface.INV_PurchaseOrderInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PurchaseOrderImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_PurchaseOrderImpl(InventoryContext InvContext, ILogger<INV_PurchaseOrderImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_PurchaseOrderDTO getloaddata(INV_PurchaseOrderDTO data)
        {
            try
            {
                data.get_quotationno = _INVContext.INV_M_SupplierQuotationDMO.Where(s => s.MI_Id == data.MI_Id && s.INVMSQ_ActiveFlg == true).Distinct().OrderBy(o => o.INVMSQ_Id).ToArray();

                data.get_comparequotationno = _INVContext.INV_M_SupplierQuotationDMO.Where(s => s.MI_Id == data.MI_Id && s.INVMSQ_ActiveFlg == true && s.INVMSQ_FinaliseFlg == true).Distinct().OrderBy(o => o.INVMSQ_Id).ToArray();

                data.get_supplier = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMS_Id).Distinct().ToArray();
                data.get_PI = _INVContext.INV_M_PurchaseIndentDMO.Where(p => p.MI_Id == data.MI_Id && p.INVMPI_ActiveFlg == true).OrderBy(o => o.INVMPI_Id).Distinct().ToArray();
                data.get_purchaseorder = _INVContext.INV_M_PurchaseOrderDMO.Where(s => s.MI_Id == data.MI_Id).Distinct().OrderBy(o => o.INVMPO_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Order load Page:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseOrderDTO getqtDetail(INV_PurchaseOrderDTO data)
        {
            try
            {
                List<INV_PurchaseOrderDTO> qtdetails = new List<INV_PurchaseOrderDTO>();

                if (data.Selectionflag == "P")
                {
                    data.pidetails = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                      from b in _INVContext.INV_T_PurchaseIndentDMO
                                      from c in _INVContext.INV_Master_ItemDMO
                                      from d in _INVContext.INV_Master_UOMDMO
                                      where (a.INVMPI_Id == b.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                               && a.INVMPI_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMPI_Id == data.INVMPI_Id)
                                      select new INV_PurchaseOrderDTO
                                      {
                                          INVMPI_Id = a.INVMPI_Id,
                                          INVTPI_Id = b.INVTPI_Id,
                                          INVMI_Id = b.INVMI_Id,
                                          INVMUOM_Id = b.INVMUOM_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVMUOM_UOMName = d.INVMUOM_UOMName,
                                          INVTPI_PIQty = b.INVTPI_PIQty,
                                          INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                                          INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,
                                      }).Distinct().ToArray();
                }
                else
                {
                    if (data.quotationflag == "0")
                    {
                        qtdetails = (from a in _INVContext.INV_M_SupplierQuotationDMO
                                     from b in _INVContext.INV_T_SupplierQuotationDMO
                                     from c in _INVContext.INV_Master_ItemDMO
                                     from d in _INVContext.INV_Master_UOMDMO
                                     where (a.INVMSQ_Id == b.INVMSQ_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                              && a.INVMSQ_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMSQ_Id == data.INVMSQ_Id)
                                     select new INV_PurchaseOrderDTO
                                     {
                                         INVMPI_Id = a.INVMPI_Id,
                                         INVTSQ_Id = b.INVTSQ_Id,
                                         INVMI_Id = b.INVMI_Id,
                                         INVMUOM_Id = b.INVMUOM_Id,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVMUOM_UOMName = d.INVMUOM_UOMName,
                                         INVTSQ_QuotedRate = b.INVTSQ_QuotedRate,
                                         INVTSQ_NegotiatedRate = b.INVTSQ_NegotiatedRate,
                                     }).Distinct().ToList();
                    }
                    else
                    {
                        qtdetails = (from a in _INVContext.INV_M_SupplierQuotationDMO
                                     from b in _INVContext.INV_T_SupplierQuotationDMO
                                     from c in _INVContext.INV_Master_ItemDMO
                                     from d in _INVContext.INV_Master_UOMDMO
                                     where (a.INVMSQ_Id == b.INVMSQ_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                              && a.INVMSQ_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMSQ_Id == data.INVMSQ_Id && a.INVMSQ_FinaliseFlg == true && b.INVTSQ_FinaliseFlg == true)
                                     select new INV_PurchaseOrderDTO
                                     {
                                         INVMPI_Id = a.INVMPI_Id,
                                         INVTSQ_Id = b.INVTSQ_Id,
                                         INVMI_Id = b.INVMI_Id,
                                         INVMUOM_Id = b.INVMUOM_Id,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVMUOM_UOMName = d.INVMUOM_UOMName,
                                         INVTSQ_QuotedRate = b.INVTSQ_QuotedRate,
                                         INVTSQ_NegotiatedRate = b.INVTSQ_NegotiatedRate,
                                     }).Distinct().ToList();
                    }
                }
                data.get_qtdetails = qtdetails.ToArray();
                if (qtdetails.Count > 0)
                {
                    foreach (var pi in qtdetails)
                    {
                        var itemtax = (from a in _INVContext.INV_Master_ItemDMO
                                       from b in _INVContext.INV_Master_Item_TaxDMO
                                       from c in _INVContext.INV_Master_TaxDMO
                                       from d in _INVContext.INV_T_SupplierQuotationDMO
                                       where (a.INVMI_Id == b.INVMI_Id && b.INVMT_Id == c.INVMT_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == c.MI_Id && a.INVMI_ActiveFlg == true && b.INVMIT_ActiveFlg == true && c.INVMT_ActiveFlg == true && a.INVMI_Id == pi.INVMI_Id)
                                       select new INV_PurchaseOrderDTO
                                       {
                                           INVMI_Id = a.INVMI_Id,
                                           INVMI_ItemName = a.INVMI_ItemName,
                                           INVMT_Id = b.INVMT_Id,
                                           INVMT_TaxName = c.INVMT_TaxName,
                                           INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                           INVMIT_TaxValue = b.INVMIT_TaxValue,
                                       }).Distinct().OrderBy(m => m.INVMT_Id).ToList();

                        data.get_itemTax = itemtax.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI Details :" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseOrderDTO getitemtax(INV_PurchaseOrderDTO data)
        {
            try
            {
                List<INV_PurchaseOrderDTO> itemtax = new List<INV_PurchaseOrderDTO>();
                if (data.Selectionflag == "P")
                {
                    itemtax = (from a in _INVContext.INV_Master_ItemDMO
                               from b in _INVContext.INV_Master_Item_TaxDMO
                               from c in _INVContext.INV_Master_TaxDMO
                               from d in _INVContext.INV_T_PurchaseIndentDMO
                               where (a.INVMI_Id == b.INVMI_Id && b.INVMT_Id == c.INVMT_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == c.MI_Id && a.INVMI_ActiveFlg == true && b.INVMIT_ActiveFlg == true && c.INVMT_ActiveFlg == true && a.INVMI_Id == data.INVMI_Id && d.INVTPI_PIUnitRate == data.INVTPI_PIUnitRate)
                               select new INV_PurchaseOrderDTO
                               {
                                   INVMI_Id = a.INVMI_Id,
                                   INVMIT_Id = b.INVMIT_Id,
                                   INVMI_ItemName = a.INVMI_ItemName,
                                   INVMT_Id = b.INVMT_Id,
                                   INVMT_TaxName = c.INVMT_TaxName,
                                   INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                   INVMIT_TaxValue = b.INVMIT_TaxValue,
                                   INVTPI_PIUnitRate = d.INVTPI_PIUnitRate
                               }).Distinct().OrderBy(m => m.INVMT_Id).ToList();
                }
                else
                {
                    itemtax = (from a in _INVContext.INV_Master_ItemDMO
                               from b in _INVContext.INV_Master_Item_TaxDMO
                               from c in _INVContext.INV_Master_TaxDMO
                               from d in _INVContext.INV_T_SupplierQuotationDMO
                               where (a.INVMI_Id == b.INVMI_Id && b.INVMT_Id == c.INVMT_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == c.MI_Id && a.INVMI_ActiveFlg == true && b.INVMIT_ActiveFlg == true && c.INVMT_ActiveFlg == true && a.INVMI_Id == data.INVMI_Id && d.INVTSQ_NegotiatedRate == data.INVTSQ_NegotiatedRate)
                               select new INV_PurchaseOrderDTO
                               {
                                   INVMI_Id = a.INVMI_Id,
                                   INVMIT_Id = b.INVMIT_Id,
                                   INVMI_ItemName = a.INVMI_ItemName,
                                   INVMT_Id = b.INVMT_Id,
                                   INVMT_TaxName = c.INVMT_TaxName,
                                   INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                   INVMIT_TaxValue = b.INVMIT_TaxValue,
                                   INVTSQ_NegotiatedRate = d.INVTSQ_NegotiatedRate
                               }).Distinct().OrderBy(m => m.INVMT_Id).ToList();
                }
                data.get_itemTax = itemtax.ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI Details :" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseOrderDTO savedetails(INV_PurchaseOrderDTO data)
        {
            try
            {
                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                if (data.INVMPO_Id != 0)
                {
                    INV_M_PurchaseOrderDMO po = new INV_M_PurchaseOrderDMO();
                    po.MI_Id = data.MI_Id;
                    po.INVMPO_PODate = data.INVMPO_PODate;
                    po.INVMPO_Remarks = data.INVMPO_Remarks;
                    po.INVMPO_TotRate = data.INVMPO_TotRate;
                    po.INVMPO_TotTax = data.INVMPO_TotTax;
                    po.INVMPO_TotAmount = data.INVMPO_TotAmount;
                    po.INVMPO_UpdatedBy = data.UserId;
                    po.INVMPO_ActiveFlg = true;
                    po.UpdatedDate = DateTime.Now;
                    _INVContext.Update(po);
                    //===================================PO Transcation Data
                    foreach (var i in data.arrayPO)
                    {
                        INV_T_PurchaseOrderDMO potrans = new INV_T_PurchaseOrderDMO();
                        potrans.MI_Id = data.MI_Id;
                        potrans.INVTPO_POQty = i.INVTPO_POQty;
                        potrans.INVTPO_RatePerUnit = i.INVTPO_RatePerUnit;
                        potrans.INVTPO_TaxAmount = i.INVTPO_TaxAmount;
                        potrans.INVTPO_Amount = i.INVTPO_Amount;
                        potrans.INVTPO_Remarks = i.INVTPO_Remarks;
                        potrans.INVTPO_ActiveFlg = true;
                        potrans.INVTPO_UpdatedBy = data.UserId;
                        potrans.UpdatedDate = DateTime.Now;
                        potrans.INVTPO_ExpectedDeliveryDate = i.INVTPO_ExpectedDeliveryDate;
                        _INVContext.Update(potrans);
                        //===================================PO Transcation Tax
                        foreach (var t in i.arrayPOtax)
                        {
                            INV_T_PurchaseOrder_TaxDMO potax = new INV_T_PurchaseOrder_TaxDMO();
                            potax.MI_Id = data.MI_Id;
                            potax.INVTPO_Id = potrans.INVTPO_Id;
                            potax.INVMIT_Id = t.INVMIT_Id;
                            potax.INVTPOT_TaxPercent = t.INVTPOT_TaxPercent;
                            potax.INVTPOT_TaxAmount = t.INVTPOT_TaxAmount;
                            potax.INVTPOT_ActiveFlg = true;

                            potax.INVTPOT_UpdatedBy = data.UserId;
                            potax.UpdatedDate = DateTime.Now;
                            _INVContext.Update(potax);
                        }
                        var contexttrans = _INVContext.SaveChanges();
                        if (contexttrans > 0)
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
                    INV_M_PurchaseOrderDMO po = new INV_M_PurchaseOrderDMO();
                    po.MI_Id = data.MI_Id;
                    po.INVMS_Id = data.INVMS_Id;
                    po.INVMPO_PONo = data.trans_id;
                    po.INVMSQ_Id = data.INVMSQ_Id;
                    po.INVMPO_PODate = data.INVMPO_PODate;
                    po.INVMPO_Remarks = data.INVMPO_Remarks;
                    po.INVMPO_TotRate = data.INVMPO_TotRate;
                    po.INVMPO_ReferenceNo = data.INVMPO_ReferenceNo;
                    po.INVMPO_TotTax = data.INVMPO_TotTax;
                    po.INVMPO_TotAmount = data.INVMPO_TotAmount;
                    po.INVMPO_CreatedBy = data.UserId;
                    po.INVMPO_UpdatedBy = data.UserId;
                    po.INVMPO_ActiveFlg = true;
                    po.CreatedDate = DateTime.Now;
                    po.UpdatedDate = DateTime.Now;
                    _INVContext.Add(po);
                    //===================================PO Transcation Data
                    foreach (var i in data.arrayPO)
                    {
                        INV_T_PurchaseOrderDMO potrans = new INV_T_PurchaseOrderDMO();
                        potrans.MI_Id = data.MI_Id;
                        potrans.INVMPO_Id = po.INVMPO_Id;
                        potrans.INVMPI_Id = i.INVMPI_Id;
                        potrans.INVMI_Id = i.INVMI_Id;
                        potrans.INVMUOM_Id = i.INVMUOM_Id;
                        potrans.INVTPO_POQty = i.INVTPO_POQty;
                        potrans.INVTPO_RatePerUnit = i.INVTPO_RatePerUnit;
                        potrans.INVTPO_TaxAmount = i.INVTPO_TaxAmount;
                        potrans.INVTPO_Amount = i.INVTPO_Amount;
                        potrans.INVTPO_Remarks = i.INVTPO_Remarks;
                        potrans.INVTPO_ActiveFlg = true;
                        potrans.INVTPO_CreatedBy = data.UserId;
                        potrans.INVTPO_UpdatedBy = data.UserId;
                        potrans.CreatedDate = DateTime.Now;
                        potrans.UpdatedDate = DateTime.Now;
                        potrans.INVTPO_ExpectedDeliveryDate = i.INVTPO_ExpectedDeliveryDate;
                        _INVContext.Add(potrans);
                        //===================================PO Transcation Tax
                        if (i.arrayPOtax.Length > 0)
                        {
                            foreach (var t in i.arrayPOtax)
                            {
                                INV_T_PurchaseOrder_TaxDMO potax = new INV_T_PurchaseOrder_TaxDMO();
                                potax.MI_Id = data.MI_Id;
                                potax.INVTPO_Id = potrans.INVTPO_Id;
                                potax.INVMIT_Id = t.INVMIT_Id;
                                potax.INVTPOT_TaxPercent = t.INVTPOT_TaxPercent;
                                potax.INVTPOT_TaxAmount = t.INVTPOT_TaxAmount;
                                potax.INVTPOT_ActiveFlg = true;
                                potax.INVTPOT_CreatedBy = data.UserId;
                                potax.INVTPOT_UpdatedBy = data.UserId;
                                potax.CreatedDate = DateTime.Now;
                                potax.UpdatedDate = DateTime.Now;
                                _INVContext.Add(potax);
                            }
                        }
                    }
                    using (var dbPOTxn = _INVContext.Database.BeginTransaction())
                    {
                        var contexttrans = _INVContext.SaveChanges();
                        dbPOTxn.Commit();
                        if (contexttrans > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            dbPOTxn.Rollback();
                            data.returnval = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("PO savedata :" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseOrderDTO deactiveM(INV_PurchaseOrderDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_PurchaseOrderDMO.Single(t => t.INVMPO_Id == data.INVMPO_Id);

                if (result.INVMPO_ActiveFlg == true)
                {
                    result.INVMPO_ActiveFlg = false;
                }
                else if (result.INVMPO_ActiveFlg == false)
                {
                    result.INVMPO_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                var resultt = _INVContext.INV_T_PurchaseOrderDMO.Where(t => t.INVMPO_Id == data.INVMPO_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (result.INVMPO_ActiveFlg == true)
                    {
                        rt.INVTPO_ActiveFlg = true;

                        var resulttax = _INVContext.INV_T_PurchaseOrder_TaxDMO.Where(t => t.INVTPO_Id == rt.INVTPO_Id).ToList();
                        foreach (var rtx in resulttax)
                        {
                            rtx.INVTPOT_ActiveFlg = true;
                            rtx.UpdatedDate = DateTime.Now;
                            _INVContext.Update(rtx);
                        }

                        rt.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rt);
                    }
                    if (result.INVMPO_ActiveFlg == false)
                    {
                        rt.INVTPO_ActiveFlg = false;
                        var resulttax = _INVContext.INV_T_PurchaseOrder_TaxDMO.Where(t => t.INVTPO_Id == rt.INVTPO_Id).ToList();
                        foreach (var rtx in resulttax)
                        {
                            rtx.INVTPOT_ActiveFlg = false;
                            rtx.UpdatedDate = DateTime.Now;
                            _INVContext.Update(rtx);
                        }
                        rt.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rt);
                    }

                }

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

        public INV_PurchaseOrderDTO deactive(INV_PurchaseOrderDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_PurchaseOrderDMO.Single(t => t.INVTPO_Id == data.INVTPO_Id);

                if (result.INVTPO_ActiveFlg == true)
                {
                    result.INVTPO_ActiveFlg = false;
                }
                else if (result.INVTPO_ActiveFlg == false)
                {
                    result.INVTPO_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                int countactiveT = 0;
                int countactiveF = 0;
                var resultt = _INVContext.INV_T_PurchaseOrderDMO.Where(t => t.INVMPO_Id == data.INVMPO_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (rt.INVTPO_ActiveFlg == false)
                    {
                        countactiveF += 1;
                    }
                    else if (rt.INVTPO_ActiveFlg == true)
                    {
                        countactiveT += 1;
                    }
                }
                var resultmflg = _INVContext.INV_M_PurchaseOrderDMO.Single(t => t.INVMPO_Id == data.INVMPO_Id);
                if (countactiveF > 0 && countactiveT == 0)
                {
                    resultmflg.INVMPO_ActiveFlg = false;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }
                else if (countactiveT > 0 && countactiveF == 0)
                {
                    resultmflg.INVMPO_ActiveFlg = true;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }

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
        public INV_PurchaseOrderDTO deactiveTx(INV_PurchaseOrderDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_PurchaseOrder_TaxDMO.Single(t => t.INVTPOT_Id == data.INVTPOT_Id);

                if (result.INVTPOT_ActiveFlg == true)
                {
                    result.INVTPOT_ActiveFlg = false;
                }
                else if (result.INVTPOT_ActiveFlg == false)
                {
                    result.INVTPOT_ActiveFlg = true;
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
        public INV_PurchaseOrderDTO get_modeldetails(INV_PurchaseOrderDTO data)
        {
            try
            {
                data.get_poDetail = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                     from b in _INVContext.INV_T_PurchaseOrderDMO
                                     from c in _INVContext.INV_Master_ItemDMO
                                     from d in _INVContext.INV_Master_UOMDMO                                    
                                     where (a.INVMPO_Id == b.INVMPO_Id &&  b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && a.MI_Id == data.MI_Id && b.INVMPO_Id == data.INVMPO_Id)
                                     select new INV_PurchaseOrderDTO
                                     {
                                         INVMPO_Id = a.INVMPO_Id,
                                         INVMPO_PONo = a.INVMPO_PONo,
                                         INVMS_Id = a.INVMS_Id,
                                         INVTPO_Id = b.INVTPO_Id,
                                         INVMI_Id = b.INVMI_Id,
                                         INVMUOM_Id = b.INVMUOM_Id,
                                         INVMSQ_Id = a.INVMSQ_Id,
                                         INVMPO_ReferenceNo = a.INVMPO_ReferenceNo,
                                         INVMPO_PODate = a.INVMPO_PODate,
                                         INVMPO_TotRate = a.INVMPO_TotAmount,
                                         INVMPO_TotTax = a.INVMPO_TotTax,
                                         INVMPO_TotAmount = a.INVMPO_TotAmount,
                                         INVMI_ItemName = c.INVMI_ItemName,
                                         INVMI_ItemCode = c.INVMI_ItemCode,
                                         INVMUOM_UOMName = d.INVMUOM_UOMName,
                                         INVTPO_POQty = b.INVTPO_POQty,
                                         INVTPO_RatePerUnit = b.INVTPO_RatePerUnit,
                                         INVTSQ_NegotiatedRate = b.INVTPO_RatePerUnit,
                                         INVTPI_PIUnitRate = b.INVTPO_RatePerUnit,
                                         INVTPO_TaxAmount = b.INVTPO_TaxAmount,
                                         INVTPO_Amount = b.INVTPO_Amount,
                                         INVTPO_Remarks = b.INVTPO_Remarks,
                                         INVTPO_ActiveFlg = b.INVTPO_ActiveFlg,

                                     }).Distinct().OrderBy(i => i.INVTPO_Id).ToArray();


                data.get_supdata = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true && m.INVMS_Id == data.INVMS_Id).OrderBy(m => m.INVMS_Id).Distinct().ToArray();

                data.get_potax = (from a in _INVContext.INV_M_PurchaseOrderDMO
                                  from b in _INVContext.INV_T_PurchaseOrderDMO
                                  from c in _INVContext.INV_T_PurchaseOrder_TaxDMO
                                  from d in _INVContext.INV_Master_Item_TaxDMO
                                  from e in _INVContext.INV_Master_TaxDMO
                                  from f in _INVContext.INV_Master_ItemDMO

                                  where (a.INVMPO_Id == b.INVMPO_Id && b.INVTPO_Id == c.INVTPO_Id && c.INVMIT_Id == d.INVMIT_Id && d.INVMT_Id == e.INVMT_Id && b.INVMI_Id == f.INVMI_Id
                                  && a.MI_Id == data.MI_Id && b.INVTPO_Id == data.INVTPO_Id)
                                  select new INV_PurchaseOrderDTO
                                  {
                                      INVTPOT_Id = c.INVTPOT_Id,
                                      INVMPO_Id = a.INVMPO_Id,
                                      INVMPO_PONo = a.INVMPO_PONo,
                                      INVTPO_Id = b.INVTPO_Id,
                                      INVMI_Id = b.INVMI_Id,
                                      INVMI_ItemName = f.INVMI_ItemName,
                                      INVMI_ItemCode = f.INVMI_ItemCode,
                                      INVMT_TaxName = e.INVMT_TaxName,
                                      INVTPOT_TaxPercent = c.INVTPOT_TaxPercent,
                                      INVTPOT_TaxAmount = c.INVTPOT_TaxAmount,
                                      INVTPOT_ActiveFlg = c.INVTPOT_ActiveFlg,

                                  }).Distinct().OrderBy(i => i.INVTPO_Id).ToArray();

                //data.get_editDetail = (from a in _INVContext.INV_M_PurchaseOrderDMO
                //                       from b in _INVContext.INV_T_PurchaseOrderDMO
                //                       from c in _INVContext.INV_Master_ItemDMO
                //                       from d in _INVContext.INV_Master_UOMDMO
                //                       from e in _INVContext.INV_T_PurchaseOrder_TaxDMO
                //                       from f in _INVContext.INV_M_SupplierQuotationDMO
                //                       where (a.INVMPO_Id == b.INVMPO_Id && b.INVTPO_Id == e.INVTPO_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                //                       && a.MI_Id == data.MI_Id && b.INVMPO_Id == data.INVMPO_Id)
                //                       select new INV_PurchaseOrderDTO
                //                       {
                //                           INVMPO_Id = a.INVMPO_Id,
                //                           INVMPO_PONo = a.INVMPO_PONo,
                //                           INVTPO_Id = b.INVTPO_Id,
                //                           INVMI_Id = b.INVMI_Id,
                //                           INVMUOM_Id = b.INVMUOM_Id,
                //                           INVMPO_ReferenceNo = a.INVMPO_ReferenceNo,
                //                           INVMPO_PODate = a.INVMPO_PODate,
                //                           INVMI_ItemName = c.INVMI_ItemName,
                //                           INVMI_ItemCode = c.INVMI_ItemCode,
                //                           INVMUOM_UOMName = d.INVMUOM_UOMName,
                //                           INVTPO_POQty = b.INVTPO_POQty,
                //                           INVTPO_RatePerUnit = b.INVTPO_RatePerUnit,
                //                           INVTPO_TaxAmount = b.INVTPO_TaxAmount,
                //                           INVTPO_Amount = b.INVTPO_Amount,
                //                           INVTPO_Remarks = b.INVTPO_Remarks,
                //                           INVTPO_ActiveFlg = b.INVTPO_ActiveFlg,

                //                       }).Distinct().OrderBy(i => i.INVTPO_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }



    }
}
