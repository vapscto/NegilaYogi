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
    public class INV_VendorPaymentImpl : Interface.INV_VendorPaymentInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_VendorPaymentImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_VendorPaymentImpl(InventoryContext InvContext, ILogger<INV_VendorPaymentImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_VendorPaymentDTO getloaddata(INV_VendorPaymentDTO data)
        {
            try
            {
                data.get_supplier = _INVContext.INV_Master_SupplierDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).Distinct().OrderBy(m => m.INVMS_Id).ToArray();
                data.get_paymentMode = _INVContext.IVRM_ModeOfPaymentDMO.Where(m => m.MI_Id == data.MI_Id && m.IVRMMOD_ActiveFlag == true).Distinct().OrderBy(m => m.IVRMMOD_Id).ToArray();
                data.get_vendorpayment = (from a in _INVContext.INV_Supplier_PaymentDMO
                                          from b in _INVContext.INV_Master_SupplierDMO
                                          where (a.INVMS_Id == b.INVMS_Id && a.MI_Id == data.MI_Id)
                                          select new INV_VendorPaymentDTO
                                          {
                                              INVSPT_Id = a.INVSPT_Id,
                                              INVMS_Id = a.INVMS_Id,
                                              INVMS_SupplierName = b.INVMS_SupplierName,
                                              INVSPT_PaymentDate = a.INVSPT_PaymentDate,
                                              INVSPT_ModeOfPayment = a.INVSPT_ModeOfPayment,
                                              INVSPT_PaymentReference = a.INVSPT_PaymentReference,
                                              INVSPT_ChequeDDNo = a.INVSPT_ChequeDDNo,
                                              INVSPT_BankName = a.INVSPT_BankName,
                                              INVSPT_ChequeDDDate = a.INVSPT_ChequeDDDate,
                                              INVSPT_Amount = a.INVSPT_Amount,
                                              INVSPT_Remarks = a.INVSPT_Remarks,
                                              INVSPT_ActiveFlg = a.INVSPT_ActiveFlg
                                          }).Distinct().OrderBy(s => s.INVSPT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Vendor Payment load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_VendorPaymentDTO> getGRNdetail(INV_VendorPaymentDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_VendorGRN";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.INVMS_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }

                        data.get_SuplierGRNno = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.get_GRNpayment = (from a in _INVContext.INV_Supplier_PaymentDMO
                                       from b in _INVContext.INV_Supplier_Payment_GRNDMO
                                       from c in _INVContext.INV_Master_SupplierDMO
                                       from d in _INVContext.INV_M_GRNDMO
                                       where (a.INVSPT_Id == b.INVSPT_Id && b.INVSPTGRN_ActiveFlg == true && a.INVMS_Id == c.INVMS_Id && a.MI_Id == c.MI_Id && a.INVMS_Id == d.INVMS_Id && b.INVMGRN_Id == d.INVMGRN_Id && a.INVSPT_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMS_Id == data.INVMS_Id && b.INVMGRN_Id == data.INVMGRN_Id)
                                       select new INV_VendorPaymentDTO
                                       {
                                           INVSPT_Id = a.INVSPT_Id,
                                           INVSPTGRN_Id = b.INVSPTGRN_Id,
                                           INVMGRN_Id = b.INVMGRN_Id,
                                           INVSPT_ModeOfPayment = a.INVSPT_ModeOfPayment,
                                           INVSPT_PaymentDate = a.INVSPT_PaymentDate,
                                           INVMGRN_GRNNo = d.INVMGRN_GRNNo,
                                           INVMGRN_PurchaseValue = d.INVMGRN_PurchaseValue,
                                           INVSPTGRN_Amount = b.INVSPTGRN_Amount,
                                           INVMGRN_TotalPaid = d.INVMGRN_TotalPaid,
                                           INVMGRN_TotalBalance = d.INVMGRN_TotalBalance
                                       }).Distinct().OrderBy(p => p.INVSPT_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Ventor Payment Get Grn no.:" + ex.Message);
            }
            return data;
        }

        public INV_VendorPaymentDTO savedetails(INV_VendorPaymentDTO data)
        {
            try
            {
                if (data.INVSPT_Id != 0)
                {

                    var result = _INVContext.INV_Supplier_PaymentDMO.Single(t => t.INVSPT_Id == data.INVSPT_Id);
                    result.MI_Id = data.MI_Id;
                    result.INVMS_Id = data.INVMS_Id;
                    result.INVSPT_PaymentDate = data.INVSPT_PaymentDate;
                    result.INVSPT_ModeOfPayment = data.INVSPT_ModeOfPayment;
                    result.INVSPT_PaymentReference = data.INVSPT_PaymentReference;
                    result.INVSPT_ChequeDDNo = data.INVSPT_ChequeDDNo;
                    result.INVSPT_BankName = data.INVSPT_BankName;
                    result.INVSPT_ChequeDDDate = data.INVSPT_ChequeDDDate;
                    result.INVSPT_Amount = data.INVSPT_Amount;
                    result.INVSPT_Remarks = data.INVSPT_Remarks;
                    result.INVSPT_CreatedBy = data.UserId;
                    result.INVSPT_UpdatedBy = data.UserId;
                    result.INVSPT_ActiveFlg = true;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(result);

                    foreach (var pu in data.paymentArray)
                    {
                        var res1 = _INVContext.INV_Supplier_Payment_GRNDMO.Where(a => a.INVSPTGRN_Id == pu.INVSPTGRN_Id).ToList();
                        if (res1.Count > 0)
                        {
                            var res11 = _INVContext.INV_Supplier_Payment_GRNDMO.Single(a => a.INVSPTGRN_Id == pu.INVSPTGRN_Id);
                            res11.INVMGRN_Id = pu.INVMGRN_Id;
                            res11.INVSPTGRN_Amount = pu.INVSPTGRN_Amount;
                            res11.INVSPTGRN_Remarks = pu.INVSPTGRN_Remarks;
                            res11.INVSPTGRN_UpdatedBy = data.UserId;
                            res11.UpdatedDate = DateTime.Now;
                            _INVContext.Update(res11);
                        }
                        else
                        {
                            INV_Supplier_Payment_GRNDMO pymu = new INV_Supplier_Payment_GRNDMO();
                            pymu.INVSPT_Id = result.INVSPT_Id;
                            pymu.INVMGRN_Id = pu.INVMGRN_Id;
                            pymu.INVSPTGRN_Amount = pu.INVSPTGRN_Amount;
                            pymu.INVSPTGRN_Remarks = pu.INVSPTGRN_Remarks;
                            pymu.INVSPTGRN_CreatedBy = data.UserId;
                            pymu.INVSPTGRN_UpdatedBy = data.UserId;
                            pymu.CreatedDate = DateTime.Now;
                            pymu.UpdatedDate = DateTime.Now;
                            _INVContext.Add(pymu);
                        }
                        var grnupdate = _INVContext.INV_M_GRNDMO.Single(g => g.MI_Id == data.MI_Id && g.INVMGRN_Id == pu.INVMGRN_Id);
                        grnupdate.INVMGRN_TotalPaid = pu.INVMGRN_TotalPaid;
                        grnupdate.INVMGRN_TotalBalance = pu.INVMGRN_TotalBalance;
                        grnupdate.UpdatedDate = DateTime.Now;
                        _INVContext.Update(grnupdate);
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
                    INV_Supplier_PaymentDMO pay = new INV_Supplier_PaymentDMO();
                    pay.MI_Id = data.MI_Id;
                    pay.INVMS_Id = data.INVMS_Id;
                    pay.INVSPT_PaymentDate = data.INVSPT_PaymentDate;
                    pay.INVSPT_ModeOfPayment = data.INVSPT_ModeOfPayment;
                    pay.INVSPT_PaymentReference = data.INVSPT_PaymentReference;

                    pay.INVSPT_ChequeDDNo = data.INVSPT_ChequeDDNo;
                    pay.INVSPT_BankName = data.INVSPT_BankName;
                    pay.INVSPT_ChequeDDDate = data.INVSPT_ChequeDDDate;

                    pay.INVSPT_Amount = data.INVSPT_Amount;
                    pay.INVSPT_Remarks = data.INVSPT_Remarks;
                    pay.INVSPT_CreatedBy = data.UserId;
                    pay.INVSPT_UpdatedBy = data.UserId;
                    pay.INVSPT_ActiveFlg = true;
                    pay.CreatedDate = DateTime.Now;
                    pay.UpdatedDate = DateTime.Now;
                    _INVContext.Add(pay);

                    foreach (var p in data.paymentArray)
                    {
                        INV_Supplier_Payment_GRNDMO paygrn = new INV_Supplier_Payment_GRNDMO();
                        paygrn.INVSPT_Id = pay.INVSPT_Id;
                        paygrn.INVMGRN_Id = p.INVMGRN_Id;
                        paygrn.INVSPTGRN_Amount = p.INVSPTGRN_Amount;
                        paygrn.INVSPTGRN_Remarks = p.INVSPTGRN_Remarks;
                        paygrn.INVSPTGRN_ActiveFlg = true;
                        paygrn.INVSPTGRN_CreatedBy = data.UserId;
                        paygrn.INVSPTGRN_UpdatedBy = data.UserId;
                        paygrn.CreatedDate = DateTime.Now;
                        paygrn.UpdatedDate = DateTime.Now;
                        _INVContext.Add(paygrn);

                        var grnupdate = _INVContext.INV_M_GRNDMO.Single(g => g.MI_Id == data.MI_Id && g.INVMGRN_Id == p.INVMGRN_Id);
                        grnupdate.INVMGRN_TotalPaid = p.INVMGRN_TotalPaid;
                        grnupdate.INVMGRN_TotalBalance = p.INVMGRN_TotalBalance;
                        grnupdate.UpdatedDate = DateTime.Now;
                        _INVContext.Update(grnupdate);
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
                _logInv.LogInformation("Vendor Payment savedata :" + ex.Message);
            }
            return data;
        }

        public INV_VendorPaymentDTO deactive(INV_VendorPaymentDTO data)
        {
            try
            {
                //var result = _INVContext.INV_Supplier_PaymentDMO.Single(t => t.INVSPT_Id == data.INVSPT_Id && t.MI_Id == data.MI_Id);

                //if (result.INVSPT_ActiveFlg == true)
                //{
                //    result.INVSPT_ActiveFlg = false;
                //}
                //else if (result.INVSPT_ActiveFlg == false)
                //{
                //    result.INVSPT_ActiveFlg = true;
                //}
                //var resultt = _INVContext.INV_Supplier_Payment_GRNDMO.Where(t => t.INVSPT_Id == data.INVSPT_Id).ToList();
                //foreach (var r in resultt)
                //{
                //    if (result.INVSPT_ActiveFlg == true)
                //    {
                //        r.INVSPTGRN_ActiveFlg = true;
                //    }
                //    else if (result.INVSPT_ActiveFlg == false)
                //    {
                //        r.INVSPTGRN_ActiveFlg = false;
                //    }
                //    r.UpdatedDate = DateTime.Now;
                //    _INVContext.Update(r);
                //}
                //result.UpdatedDate = DateTime.Now;
                //_INVContext.Update(result);
                //int returnval = _INVContext.SaveChanges();
                //if (returnval > 0)
                //{
                //    try
                //    {
                //        if (contextupdate > 0)
                //        {
                //            data.returnduplicatestatus = "Updated";
                //        }
                //        else
                //        {
                //            data.returnduplicatestatus = "notUpdated";
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public INV_VendorPaymentDTO deactiveGRN(INV_VendorPaymentDTO data)
        {
            try
            {
                //var result = _INVContext.INV_T_SalesDMO.Single(t => t.INVTSL_Id == data.INVTSL_Id);

                //if (result.INVTSL_ActiveFlg == true)
                //{
                //    result.INVTSL_ActiveFlg = false;
                //}
                //else if (result.INVTSL_ActiveFlg == false)
                //{
                //    result.INVTSL_ActiveFlg = true;
                //}

                //var resulttx = _INVContext.INV_T_Sales_TaxDMO.Where(t => t.INVTSL_Id == data.INVTSL_Id).ToList();
                //foreach (var tx in resulttx)
                //{
                //    if (result.INVTSL_ActiveFlg == true)
                //    {
                //        tx.INVTSLT_ActiveFlg = true;
                //    }
                //    else if (result.INVTSL_ActiveFlg == false)
                //    {
                //        tx.INVTSLT_ActiveFlg = false;
                //    }

                //    tx.UpdatedDate = DateTime.Now;
                //    _INVContext.Update(tx);
                //}

                //result.UpdatedDate = DateTime.Now;
                //_INVContext.Update(result);
                //int returnval = _INVContext.SaveChanges();
                //if (returnval > 0)
                //{
                //    data.returnval = true;
                //}
                //else
                //{
                //    data.returnval = false;
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public INV_VendorPaymentDTO getmodeldetail(INV_VendorPaymentDTO data)
        {
            data.get_modeldetails = (from a in _INVContext.INV_Supplier_PaymentDMO
                                     from b in _INVContext.INV_Supplier_Payment_GRNDMO
                                     from c in _INVContext.INV_M_GRNDMO
                                     from d in _INVContext.INV_Master_SupplierDMO
                                     where (a.INVSPT_Id == b.INVSPT_Id && a.INVMS_Id == c.INVMS_Id && b.INVMGRN_Id == c.INVMGRN_Id && a.INVMS_Id == d.INVMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.INVSPT_Id == data.INVSPT_Id)
                                     select new INV_VendorPaymentDTO
                                     {
                                         INVSPT_Id = a.INVSPT_Id,
                                         INVSPTGRN_Id = b.INVSPTGRN_Id,
                                         INVMGRN_Id = b.INVMGRN_Id,
                                         INVMGRN_GRNNo = c.INVMGRN_GRNNo,
                                         INVMGRN_PurchaseValue = c.INVMGRN_PurchaseValue,
                                         INVMGRN_TotalPaid = c.INVMGRN_TotalPaid,
                                         INVMGRN_TotalBalance = c.INVMGRN_TotalBalance,
                                         INVSPTGRN_Amount = b.INVSPTGRN_Amount,
                                         INVSPTGRN_Remarks = b.INVSPTGRN_Remarks,
                                         INVSPTGRN_ActiveFlg = b.INVSPTGRN_ActiveFlg
                                     }).Distinct().OrderBy(s => s.INVMGRN_Id).ToArray();
            return data;
        }


    }
}
