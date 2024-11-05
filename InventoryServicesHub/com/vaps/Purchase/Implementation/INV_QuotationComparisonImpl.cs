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

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_QuotationComparisonImpl : Interface.INV_QuotationComparisonInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_QuotationComparisonImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_QuotationComparisonImpl(InventoryContext InvContext, ILogger<INV_QuotationComparisonImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_QuotationDTO getloaddata(INV_QuotationDTO data)
        {
            try
            {
                //  data.get_piNo = _INVContext.INV_M_PurchaseIndentDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMPI_ActiveFlg == true).OrderBy(m => m.INVMPI_Id).ToArray();

                data.get_piNo = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                 from b in _INVContext.INV_M_SupplierQuotationDMO
                                 where (a.INVMPI_Id == b.INVMPI_Id && a.INVMPI_ActiveFlg == true && b.INVMSQ_ActiveFlg == true && a.MI_Id == data.MI_Id)
                                 select new INV_QuotationDTO
                                 {
                                     INVMPI_Id = a.INVMPI_Id,
                                     INVMPI_PINo = a.INVMPI_PINo,
                                     INVMPI_PIDate = a.INVMPI_PIDate,
                                     INVMPI_Remarks = a.INVMPI_Remarks,
                                 }).Distinct().OrderBy(m => m.INVMPI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Q Comparison load Page:" + ex.Message);
            }
            return data;
        }

        public INV_QuotationDTO getqtdetails(INV_QuotationDTO data)
        {
            try
            {
                data.get_qtdetails = (from a in _INVContext.INV_M_SupplierQuotationDMO
                                      from b in _INVContext.INV_T_SupplierQuotationDMO
                                      from c in _INVContext.INV_M_PurchaseIndentDMO
                                      from d in _INVContext.INV_Master_ItemDMO
                                      from e in _INVContext.INV_Master_UOMDMO

                                      where (a.INVMSQ_Id == b.INVMSQ_Id && a.INVMPI_Id == c.INVMPI_Id && b.INVMI_Id == d.INVMI_Id && b.INVMUOM_Id == e.INVMUOM_Id
                                                && a.INVMSQ_ActiveFlg == true && c.INVMPI_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMPI_Id == data.INVMPI_Id && a.INVMSQ_Id == data.INVMSQ_Id)
                                      select new INV_QuotationDTO
                                      {
                                          INVMSQ_Id = a.INVMSQ_Id,
                                          INVTSQ_Id = b.INVTSQ_Id,
                                          INVMPI_Id = a.INVMPI_Id,
                                          INVMSQ_QuotationNo = a.INVMSQ_QuotationNo,
                                          INVMI_Id = b.INVMI_Id,
                                          INVMSQ_SupplierName = a.INVMSQ_SupplierName,
                                          INVMSQ_SupplierEmailId = a.INVMSQ_SupplierEmailId,
                                          INVMSQ_SupplierContactNo = a.INVMSQ_SupplierContactNo,
                                          INVMI_ItemName = d.INVMI_ItemName,
                                          INVMUOM_UOMName = e.INVMUOM_UOMName,
                                          INVMSQ_TotalQuotedRate = a.INVMSQ_TotalQuotedRate,
                                          INVMSQ_NegotiatedRate = a.INVMSQ_NegotiatedRate,
                                          INVTSQ_QuotedRate = b.INVTSQ_QuotedRate,
                                          INVTSQ_NegotiatedRate = b.INVTSQ_NegotiatedRate,
                                          INVTSQ_FinaliseFlg = b.INVTSQ_FinaliseFlg

                                      }).Distinct().OrderBy(m => m.INVMSQ_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Quotation Comparison :" + ex.Message);
            }
            return data;
        }

        public INV_QuotationDTO getpisupplier(INV_QuotationDTO data)
        {
            try
            {
                data.get_pisupplier = (from a in _INVContext.INV_M_SupplierQuotationDMO
                                       from b in _INVContext.INV_T_SupplierQuotationDMO
                                       from c in _INVContext.INV_M_PurchaseIndentDMO
                                       where (a.INVMSQ_Id == b.INVMSQ_Id && a.INVMPI_Id == c.INVMPI_Id && a.INVMSQ_ActiveFlg == true && b.INVTSQ_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMPI_Id == data.INVMPI_Id)
                                       select new INV_QuotationDTO
                                       {
                                           INVMSQ_Id = a.INVMSQ_Id,
                                           INVMPI_Id = a.INVMPI_Id,
                                           INVMPI_PINo = c.INVMPI_PINo,
                                           INVMSQ_QuotationNo = a.INVMSQ_QuotationNo,
                                           INVMSQ_SupplierName = a.INVMSQ_SupplierName,
                                           INVMSQ_SupplierEmailId = a.INVMSQ_SupplierEmailId,
                                           INVMSQ_SupplierContactNo = a.INVMSQ_SupplierContactNo,
                                           INVMSQ_TotalQuotedRate = a.INVMSQ_TotalQuotedRate,
                                           INVMSQ_NegotiatedRate = a.INVMSQ_NegotiatedRate

                                       }).Distinct().OrderBy(m => m.INVMSQ_SupplierName).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Quotation Comparison :" + ex.Message);
            }
            return data;
        }

        public INV_QuotationDTO get_Comparison(INV_QuotationDTO data)
        {
            //try
            //{
            //    List<long> INVMSQ_Ids = new List<long>();
            //    if (data.arrayQSupplier != null)
            //    {
            //        foreach (var qs in data.arrayQSupplier)
            //        {
            //            INVMSQ_Ids.Add(qs.INVMSQ_Id);
            //        }
            //    }

            //    data.get_Comparison = (from a in _INVContext.INV_M_SupplierQuotationDMO
            //                           from b in _INVContext.INV_T_SupplierQuotationDMO
            //                           from c in _INVContext.INV_Master_ItemDMO
            //                           from d in _INVContext.INV_M_PurchaseIndentDMO
            //                           where (a.INVMSQ_Id == b.INVMSQ_Id && a.INVMPI_Id == d.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && a.INVMSQ_ActiveFlg == true && b.INVTSQ_ActiveFlg == true && a.MI_Id == data.MI_Id && INVMSQ_Ids.Contains(a.INVMSQ_Id))
            //                           select new INV_QuotationDTO
            //                           {
            //                               INVMSQ_Id = a.INVMSQ_Id,
            //                               INVMPI_Id = a.INVMPI_Id,
            //                               INVMPI_PINo = d.INVMPI_PINo,
            //                               INVMSQ_QuotationNo = a.INVMSQ_QuotationNo,
            //                               INVMSQ_Quotation = a.INVMSQ_Quotation,
            //                               INVMSQ_SupplierName = a.INVMSQ_SupplierName,
            //                               INVMSQ_SupplierEmailId = a.INVMSQ_SupplierEmailId,
            //                               INVMSQ_SupplierContactNo = a.INVMSQ_SupplierContactNo,
            //                               INVMSQ_TotalQuotedRate = a.INVMSQ_TotalQuotedRate,
            //                               INVMSQ_NegotiatedRate = a.INVMSQ_NegotiatedRate,
            //                               INVMSQ_FinaliseFlg = a.INVMSQ_FinaliseFlg,
            //                               INVMSQ_Remarks = a.INVMSQ_Remarks

            //                           }).Distinct().OrderBy(m => m.INVMSQ_Id).ToArray();

            //}
            //catch (Exception ex)
            //{
            //    _logInv.LogInformation("Quotation Comparison :" + ex.Message);
            //}
            return data;
        }

        public INV_QuotationDTO savedata(INV_QuotationDTO data)
        {
            try
            {
                foreach (var qc in data.arrayQcompare)
                {
                    var result = _INVContext.INV_M_SupplierQuotationDMO.Single(t => t.MI_Id == data.MI_Id && t.INVMSQ_Id == qc.INVMSQ_Id);
                    result.MI_Id = data.MI_Id;                  
                    result.INVMSQ_NegotiatedRate = qc.INVMSQ_NegotiatedRate;
                    result.INVMSQ_FinaliseFlg = true;
                    result.INVMSQ_ActiveFlg = true;
                    result.INVMSQ_UpdatedBy = data.UserId;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(result);


                    var res1 = _INVContext.INV_T_SupplierQuotationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVTSQ_Id == qc.INVTSQ_Id).ToList();
                    if (res1.Count > 0)
                    {
                        var res11 = _INVContext.INV_T_SupplierQuotationDMO.Single(a => a.MI_Id == data.MI_Id && a.INVTSQ_Id == qc.INVTSQ_Id);
                        res11.MI_Id = data.MI_Id;
                        res11.INVMSQ_Id = qc.INVMSQ_Id;
                        res11.INVMI_Id = qc.INVMI_Id;                                          
                        res11.INVTSQ_NegotiatedRate = qc.INVTSQ_NegotiatedRate;
                        res11.INVTSQ_FinaliseFlg = true;
                        res11.INVTSQ_ActiveFlg = true;
                        res11.INVTSQ_UpdatedBy = data.UserId;
                        res11.UpdatedDate = DateTime.Now;
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

            catch (Exception ex)
            {
                _logInv.LogInformation("Quotation Comparison :" + ex.Message);
            }
            return data;
        }


    }
}
