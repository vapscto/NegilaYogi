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
    public class INV_QuotationImpl : Interface.INV_QuotationInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_QuotationImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_QuotationImpl(InventoryContext InvContext, ILogger<INV_QuotationImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_QuotationDTO getloaddata(INV_QuotationDTO data)
        {
            try
            {
                data.get_piNo = _INVContext.INV_M_PurchaseIndentDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMPI_ActiveFlg == true).OrderByDescending(m => m.INVMPI_Id).Distinct().ToArray();
                data.get_supplier = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMS_Id).Distinct().ToArray();

                data.get_Quotation = _INVContext.INV_M_SupplierQuotationDMO.Where(q => q.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Quatation load Page:" + ex.Message);
            }
            return data;
        }

        public INV_QuotationDTO getquotationdetails(INV_QuotationDTO data)
        {
            try
            {
                data.get_quotationdetails = (from a in _INVContext.INV_M_SupplierQuotationDMO
                                             from b in _INVContext.INV_T_SupplierQuotationDMO
                                             from c in _INVContext.INV_M_PurchaseIndentDMO
                                             from d in _INVContext.INV_Master_ItemDMO
                                             from e in _INVContext.INV_Master_UOMDMO
                                             where (a.INVMSQ_Id == b.INVMSQ_Id && a.INVMPI_Id == c.INVMPI_Id && b.INVMI_Id == d.INVMI_Id && b.INVMUOM_Id == e.INVMUOM_Id && a.MI_Id == data.MI_Id && a.INVMSQ_Id == data.INVMSQ_Id)
                                             select new INV_QuotationDTO
                                             {
                                                 INVMSQ_Id = a.INVMSQ_Id,
                                                 INVTSQ_Id = b.INVTSQ_Id,
                                                 INVMI_Id = b.INVMI_Id,
                                                 INVMUOM_Id = b.INVMUOM_Id,
                                                 INVMSQ_QuotationNo = a.INVMSQ_QuotationNo,
                                                 INVMSQ_SupplierName = a.INVMSQ_SupplierName,
                                                 INVMSQ_SupplierContactNo = a.INVMSQ_SupplierContactNo,
                                                 INVMSQ_SupplierEmailId = a.INVMSQ_SupplierEmailId,
                                                 INVMSQ_Quotation = a.INVMSQ_Quotation,
                                                 INVMI_ItemName = d.INVMI_ItemName,
                                                 INVMUOM_UOMName = e.INVMUOM_UOMName,
                                                 INVTSQ_QuotedRate = b.INVTSQ_QuotedRate,
                                                 INVTSQ_NegotiatedRate = b.INVTSQ_NegotiatedRate,
                                                 INVMSQ_TotalQuotedRate = a.INVMSQ_TotalQuotedRate,
                                                 INVMSQ_NegotiatedRate = a.INVMSQ_NegotiatedRate,
                                                 INVMSQ_ActiveFlg = a.INVMSQ_ActiveFlg,
                                                 INVTSQ_ActiveFlg = b.INVTSQ_ActiveFlg

                                             }).Distinct().OrderBy(i => i.INVMSQ_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Quotation details:" + ex.Message);
            }
            return data;
        }

        public INV_QuotationDTO getpiDetail(INV_QuotationDTO data)
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
                                          INVTPI_Id = b.INVTPI_Id,
                                          INVMI_Id = b.INVMI_Id,
                                          INVMUOM_Id = b.INVMUOM_Id,
                                          INVMPR_Id = b.INVMPR_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVMUOM_UOMName = d.INVMUOM_UOMName,
                                          INVTPI_PRQty = b.INVTPI_PRQty,
                                          INVTPI_PIQty = b.INVTPI_PIQty,
                                          INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                                          INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,

                                      }).Distinct().OrderBy(i => i.INVMPI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI Details load Page:" + ex.Message);
            }
            return data;
        }

        public INV_QuotationDTO savedetails(INV_QuotationDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                if (data.INVMSQ_Id != 0)
                {
                    var result = _INVContext.INV_M_SupplierQuotationDMO.Single(t => t.MI_Id == data.MI_Id && t.INVMSQ_Id == data.INVMSQ_Id);
                    result.MI_Id = data.MI_Id;
                    result.INVMPI_Id = data.INVMPI_Id;
                    result.INVMSQ_QuotationNo = data.trans_id;
                    result.INVMSQ_SupplierName = data.INVMSQ_SupplierName;
                    result.INVMSQ_SupplierContactNo = data.INVMSQ_SupplierContactNo;
                    result.INVMSQ_SupplierEmailId = data.INVMSQ_SupplierEmailId;
                    result.INVMSQ_Quotation = data.INVMSQ_Quotation;
                    result.INVMSQ_TotalQuotedRate = data.INVMSQ_TotalQuotedRate;
                    result.INVMSQ_NegotiatedRate = data.INVMSQ_NegotiatedRate;
                    result.INVMSQ_Remarks = data.INVMSQ_Remarks;
                    result.INVMSQ_FinaliseFlg = false;
                    result.INVMSQ_ActiveFlg = true;
                    result.INVMSQ_UpdatedBy = data.UserId;
                    result.UpdatedDate = indianTime;
                    _INVContext.Update(result);

                    foreach (var q in data.arrayQuatation)
                    {
                        var res1 = _INVContext.INV_T_SupplierQuotationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVTSQ_Id == q.INVTSQ_Id).ToList();
                        if (res1.Count > 0)
                        {
                            var res11 = _INVContext.INV_T_SupplierQuotationDMO.Single(a => a.MI_Id == data.MI_Id && a.INVTSQ_Id == q.INVTSQ_Id);
                            res11.MI_Id = data.MI_Id;
                            res11.INVMSQ_Id = result.INVMSQ_Id;
                            res11.INVMI_Id = q.INVMI_Id;
                            res11.INVMUOM_Id = q.INVMUOM_Id;
                            res11.INVTSQ_QuotedRate = q.INVTSQ_QuotedRate;
                            res11.INVTSQ_NegotiatedRate = q.INVTSQ_NegotiatedRate;
                            res11.INVTSQ_FinaliseFlg = false;
                            res11.INVTSQ_ActiveFlg = true;
                            res11.INVTSQ_UpdatedBy = data.UserId;
                            res11.UpdatedDate = indianTime;
                            _INVContext.Update(res11);
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
                else
                {
                    INV_M_SupplierQuotationDMO quot = new INV_M_SupplierQuotationDMO();
                    quot.MI_Id = data.MI_Id;
                    quot.INVMPI_Id = data.INVMPI_Id;
                    quot.INVMSQ_QuotationNo = data.trans_id;
                    quot.INVMSQ_SupplierName = data.INVMSQ_SupplierName;
                    quot.INVMSQ_SupplierContactNo = data.INVMSQ_SupplierContactNo;
                    quot.INVMSQ_SupplierEmailId = data.INVMSQ_SupplierEmailId;
                    quot.INVMSQ_Quotation = data.INVMSQ_Quotation;
                    quot.INVMSQ_TotalQuotedRate = data.INVMSQ_TotalQuotedRate;
                    quot.INVMSQ_NegotiatedRate = data.INVMSQ_NegotiatedRate;
                    quot.INVMSQ_Remarks = data.INVMSQ_Remarks;
                    quot.INVMSQ_FinaliseFlg = false;
                    quot.INVMSQ_ActiveFlg = true;
                    quot.INVMSQ_CreatedBy = data.UserId;
                    quot.INVMSQ_UpdatedBy = data.UserId;
                    quot.UpdatedDate = indianTime;
                    quot.CreatedDate = indianTime;
                    _INVContext.Add(quot);

                    foreach (var q in data.arrayQuatation)
                    {
                        INV_T_SupplierQuotationDMO tq = new INV_T_SupplierQuotationDMO();

                        tq.MI_Id = data.MI_Id;
                        tq.INVMSQ_Id = quot.INVMSQ_Id;
                        tq.INVMI_Id = q.INVMI_Id;
                        tq.INVMUOM_Id = q.INVMUOM_Id;
                        tq.INVTSQ_QuotedRate = q.INVTSQ_QuotedRate;
                        tq.INVTSQ_NegotiatedRate = q.INVTSQ_NegotiatedRate;
                        tq.INVTSQ_FinaliseFlg = false;
                        tq.INVTSQ_ActiveFlg = true;
                        tq.INVTSQ_CreatedBy = data.UserId;
                        tq.INVTSQ_UpdatedBy = data.UserId;
                        tq.CreatedDate = indianTime;
                        tq.UpdatedDate = indianTime;
                        _INVContext.Add(tq);
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
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Quotation savedata :" + ex.Message);
            }
            return data;
        }

        public INV_QuotationDTO deactiveM(INV_QuotationDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_SupplierQuotationDMO.Single(t => t.INVMSQ_Id == data.INVMSQ_Id);

                if (result.INVMSQ_ActiveFlg == true)
                {
                    result.INVMSQ_ActiveFlg = false;
                }
                else if (result.INVMSQ_ActiveFlg == false)
                {
                    result.INVMSQ_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                var resultt = _INVContext.INV_T_SupplierQuotationDMO.Where(t => t.INVMSQ_Id == data.INVMSQ_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (result.INVMSQ_ActiveFlg == true)
                    {
                        rt.INVTSQ_ActiveFlg = true;
                        rt.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rt);
                    }
                    if (result.INVMSQ_ActiveFlg == false)
                    {
                        rt.INVTSQ_ActiveFlg = false;
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

        public INV_QuotationDTO deactive(INV_QuotationDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_SupplierQuotationDMO.Single(t => t.INVTSQ_Id == data.INVTSQ_Id);

                if (result.INVTSQ_ActiveFlg == true)
                {
                    result.INVTSQ_ActiveFlg = false;
                }
                else if (result.INVTSQ_ActiveFlg == false)
                {
                    result.INVTSQ_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                int countactiveT = 0;
                int countactiveF = 0;
                var resultt = _INVContext.INV_T_SupplierQuotationDMO.Where(t => t.INVMSQ_Id == data.INVMSQ_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (rt.INVTSQ_ActiveFlg == false)
                    {
                        countactiveF += 1;
                    }
                    else if (rt.INVTSQ_ActiveFlg == true)
                    {
                        countactiveT += 1;
                    }
                }
                var resultmflg = _INVContext.INV_M_SupplierQuotationDMO.Single(t => t.INVMSQ_Id == data.INVMSQ_Id);
                if (countactiveF > 0 && countactiveT == 0)
                {
                    resultmflg.INVMSQ_ActiveFlg = false;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }
                else if (countactiveT > 0 && countactiveF == 0)
                {
                    resultmflg.INVMSQ_ActiveFlg = true;
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



    }
}
