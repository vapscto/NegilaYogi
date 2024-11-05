using DataAccessMsSqlServerProvider.com.vapstech.Canteen;
using DomainModel.Model.com.vapstech.Canteen;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenHub.com.vaps.Services
{

    public class FoodtransactionImpl : Interfaces.FoodtransactionInterface

    {
        public Canteencontext _fmtContext;
        public FoodtransactionImpl(Canteencontext fmtContext)
        {
            _fmtContext = fmtContext;
        }
        private readonly Random _random;

        public FoodtransactionImpl()
        {
            _random = new Random();
        }
        public FoodtransactionDTO loaddata(FoodtransactionDTO data)
        {
            try
            {

                data.Foodcategeory = _fmtContext.FoodMasterCategoryDMO.Where(R => R.MI_Id == data.MI_Id && R.CMMCA_ActiveFlag == true).ToArray();

                data.modeofpayment = _fmtContext.IVRM_ModeOfPayment.Where(t => t.MI_Id == data.MI_Id).ToArray();
                // data.foodtax = _fmtContext.FooditemtaxDMO.ToArray();
                data.Fooditeam = _fmtContext.FooditeamDMO.Where(T => T.CMMFI_ActiveFlg == true).ToArray();
                data.invmaster = _fmtContext.INV_Master_TaxDMO.Where(T => T.INVMT_ActiveFlg == true).ToArray();

                data.foodtax = (from a in _fmtContext.FooditemtaxDMO
                                from b in _fmtContext.FooditeamDMO
                                from c in _fmtContext.INV_Master_TaxDMO
                                where (a.CMMFI_Id == b.CMMFI_Id && a.INVMT_Id == c.INVMT_Id)
                                select new FoodtransactionDTO
                                {
                                    CMMFI_FoodItemName = b.CMMFI_FoodItemName,
                                    taxpercent = a.CMMFIT_TaxPercent,
                                    CMMFIT_ActiveFlg = a.CMMFIT_ActiveFlg,
                                    CMMFIT_Id = a.CMMFIT_Id,
                                    INVMT_TaxName = c.INVMT_TaxName

                                }).ToArray();

                using (var cmd = _fmtContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Transactiondeatils";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ACMST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ACMST_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        data.Transactiondeatils = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }




        public FoodtransactionDTO FoodItem(FoodtransactionDTO data)
        {
            try
            {

                data.get_foodDetail = (from a in _fmtContext.FooditeamDMO
                                       from b in _fmtContext.FoodMasterCategoryDMO
                                       from c in _fmtContext.FooditemimageDMO

                                       where (a.CMMCA_Id == b.CMMCA_Id && a.CMMFI_Id == c.CMMFI_Id)
                                       select new FooditeamDTO
                                       {
                                           CMMFI_FoodItemName = a.CMMFI_FoodItemName,
                                           CMMFI_FoodItemDescription = a.CMMFI_FoodItemDescription,
                                           CMMFI_UnitRate = a.CMMFI_UnitRate,
                                           CMMFI_OutofStockFlg = a.CMMFI_OutofStockFlg,
                                           CMMFI_PathURL = a.CMMFI_PathURL,
                                           CMMFI_Id = a.CMMFI_Id,
                                           CMMCA_CategoryName = b.CMMCA_CategoryName,
                                           CMMCA_Id = b.CMMCA_Id,
                                           CMMFI_ActiveFlg = a.CMMFI_ActiveFlg,
                                           ICAI_Attachment = c.ICAI_Attachment,
                                           MI_Id = b.MI_Id

                                       }).ToArray();




                //data.get_foodDetail = (from a in _fmtContext.FoodMasterCategoryDMO
                //                       from b in _fmtContext.FooditeamDMO

                //                       where (a.CMMCA_Id == b.CMMCA_Id && a.MI_Id == data.MI_Id && b.CMMFI_ActiveFlg == true && a.CMMCA_Id==data.CMMCA_Id)
                //                       select new FoodtransactionDTO
                //                       {
                //                           CMMCA_Id = a.CMMCA_Id,
                //                           CMMCA_CategoryName = a.CMMCA_CategoryName,
                //                           CMMFI_Id = b.CMMFI_Id,
                //                           CMMFI_FoodItemName = b.CMMFI_FoodItemName,
                //                           CMMFI_UnitRate = b.CMMFI_UnitRate,



                //                       }).Distinct().OrderBy(m => m.CMMFI_Id).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return data;
        }


        public FoodtransactionDTO FoodItemtax(FoodtransactionDTO data)
        {
            try
            {
                data.get_foodtaxDetail = (from a in _fmtContext.FooditeamDMO
                                          from b in _fmtContext.FooditemtaxDMO

                                          where (a.CMMFI_Id == b.CMMFI_Id && b.CMMFIT_ActiveFlg == true && a.CMMFI_Id == data.CMMFI_Id)
                                          select new FoodtransactionDTO
                                          {
                                              CMMFIT_Id = b.CMMFIT_Id,
                                              CMMFIT_TaxPercent = b.CMMFIT_TaxPercent,
                                              CMMFI_UnitRate = a.CMMFI_UnitRate,
                                              INVMT_Id = b.INVMT_Id
                                          }).Distinct().OrderBy(m => m.CMMFI_Id).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return data;
        }

        public FoodtransactionDTO savedata(FoodtransactionDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.CMTRANS_Id != 0)
                {
                    //var result = _fmtContext.FoodtransactionDMO.Single(t => t.MI_Id == data.MI_Id && t.CMTRANS_Id == data.CMTRANS_Id);
                    //result.CMTRANS_MemberFlg = "Other";
                    //result.CMTRANS_Amount = data.CMTRANS_Amount;
                    //result.CMTRANS_TaxAmount = data.CMTRANS_TaxAmount;
                    //result.CMTRANS_TotalAmount = data.CMTRANS_TotalAmount;
                    //result.CMTRANS_Remarks = data.CMTRANS_Remarks;
                    //result.CMTRANS_PaidAmount = data.CMTRANS_PaidAmount;
                    //result.CMTRANS_PendingAmount = data.CMTRANS_PendingAmount;
                    //result.CMTRANS_KOTPrintedFlg = data.CMTRANS_KOTPrintedFlg;
                    //result.CMTRANS_NoofKOTPrints = data.CMTRANS_NoofKOTPrints;
                    //result.CMTRANS_VoidKotFlg = data.CMTRANS_VoidKotFlg;
                    //result.CMTRANS_VoidReasons = data.CMTRANS_VoidReasons;
                    //result.CMTRANS_SelfCheckInFlg = data.CMTRANS_SelfCheckInFlg;
                    //result.CMTRANS_SecurityCode = data.CMTRANS_SecurityCode;
                    //result.CMTRANS_ActiveFlg = true;
                    //result.CMTRANS_UpdatedBy = data.UserId;
                    //result.CMTRANS_Updateddate = DateTime.Now;

                    //_fmtContext.Update(result);

                    ////added by kavitha

                    //for (int i = 0; i < data.CMTransactionTax.Length; i++)
                    //{
                    //    var resultupdate = _fmtContext.CM_Transaction_TaxDMO.Where(t => t.CMTRANST_Id == data.CMTransactionTax[i].CMTRANST_Id).FirstOrDefault();

                    //    resultupdate.CMTRANS_Id = data.CMTransactionTax[i].CMTRANS_Id;


                    //    resultupdate.INVMT_Id = data.CMTransactionTax[i].INVMT_Id;
                    //    resultupdate.CMTRANST_TaxAmount = data.CMTransactionTax[i].CMTRANST_TaxAmount;
                    //    resultupdate.CMTRANST_UpdatedBy = data.UserId;
                    //    resultupdate.CMTRANST_Updateddate = DateTime.Now;

                    //    _fmtContext.CM_Transaction_TaxDMO.Update(resultupdate);
                    //}
                    //for (int i = 0; i < data.CMTransactionPaymentMode.Length; i++)
                    //{
                    //    var resultupdate = _fmtContext.CM_Transaction_PaymentModeDMO.Where(t => t.CMTRANSPM_Id == data.CMTransactionPaymentMode[i].CMTRANSPM_Id).FirstOrDefault();

                    //    resultupdate.CMTRANS_Id = data.CMTransactionPaymentMode[i].CMTRANS_Id;


                    //    resultupdate.IVRMMOD_Id = data.CMTransactionPaymentMode[i].IVRMMOD_Id;
                    //    resultupdate.CMTRANSPM_Amount = data.CMTransactionPaymentMode[i].CMTRANSPM_Amount;
                    //    resultupdate.CMTRANSPM_UpdatedBy = data.UserId;
                    //    resultupdate.CMTRANSPM_Updateddate = DateTime.Now;

                    //    _fmtContext.CM_Transaction_PaymentModeDMO.Update(resultupdate);
                    //}
                    //for (int i = 0; i < data.CMTransactionItems.Length; i++)
                    //{



                    //    var resultupdate = _fmtContext.CM_Transaction_ItemsDMO.Where(t => t.CMTRANSI_Id == data.CMTransactionItems[i].CMTRANSI_Id).FirstOrDefault();

                    //    resultupdate.CMTRANS_Id = data.CMTransactionItems[i].CMTRANS_Id;


                    //    resultupdate.CMMFI_Id = data.CMTransactionItems[i].CMMFI_Id;

                    //    resultupdate.CMTRANS_Qty = data.CMTransactionItems[i].CMTRANS_Qty;
                    //    resultupdate.CMTRANSI_UnitRate = data.CMTransactionItems[i].CMTRANSI_UnitRate;
                    //    resultupdate.CMTRANSI_Amount = data.CMTransactionItems[i].CMTRANSI_Amount;
                    //    resultupdate.CMTRANSI_TaxApplicableFlg = data.CMTransactionItems[i].CMTRANSI_TaxApplicableFlg;
                    //    resultupdate.CMTRANSI_UpdatedBy = data.UserId;
                    //    resultupdate.CMTRANSI_Updateddate = DateTime.Now;

                    //    _fmtContext.CM_Transaction_ItemsDMO.Update(resultupdate);
                    //}


                    //var contactexisttransaction = 0;
                    //using (var dbCtxTxn = _fmtContext.Database.BeginTransaction())
                    //{
                    //    try
                    //    {
                    //        contactexisttransaction = _fmtContext.SaveChanges();
                    //        dbCtxTxn.Commit();
                    //        data.returnval = "true";
                    //        data.displaymessage = "Updated";
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine(ex.Message);
                    //        dbCtxTxn.Rollback();
                    //        data.returnval = "false";
                    //        data.displaymessage = "Not Updated";
                    //    }
                    //}
                }
                else
                {

                    var pda = _fmtContext.PDA_StatusDMO.Where(t => t.PDAS_CBExcessPaid < data.CMTRANS_Amount && t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id && t.ASMAY_Id == data.ASMAY_Id).ToList();


                    if (pda.Count > 0)
                    {
                        data.returnval = "insufficient balance";
                    }
                    else
                    {

                        var result = _fmtContext.PDA_StatusDMO.Single(t => t.AMST_Id == data.AMST_Id && t.ASMAY_Id == data.ASMAY_Id);
                        result.PDAS_CBExcessPaid = result.PDAS_CBExcessPaid - data.CMTRANS_Amount;
                        result.PDAS_CYExpenses = result.PDAS_CYExpenses + data.CMTRANS_Amount;
                        result.UpdatedDate = DateTime.Now;
                        _fmtContext.Update(result);
                        var contactExists = _fmtContext.SaveChanges();
                        if (contactExists > 0)
                        {

                            data.Transcationnum = "";
                            data.Transcationnum = data.MI_Id + indianTime.ToString("yyyyMMddHHmmss") + string.Format("{0:d5}", (indianTime.Millisecond)).Trim();


                            Random rnd = new Random();
                            data.CM_orderID = rnd.Next();

                            FoodtransactionDMO finalresult = new FoodtransactionDMO();
                            finalresult.MI_Id = data.MI_Id;
                            finalresult.ACMST_Id = data.AMST_Id;
                            finalresult.HRME_Id = data.HRME_Id;
                            finalresult.CM_Transactionnum = data.Transcationnum;
                            finalresult.CM_orderID = data.CM_orderID;
                            finalresult.CMTRANS_TotalAmount = data.CMTRANS_Amount;
                            finalresult.CMTRANS_ActiveFlg = true;
                            finalresult.CMTRANS_UpdatedBy = data.AMST_Id;
                            finalresult.CMTRANS_Updateddate = DateTime.Now;
                            _fmtContext.FoodtransactionDMO.Add(finalresult);


                            for (int i = 0; i < data.CMTransactionItems.Length; i++)
                            {
                                CM_Transaction_ItemsDMO resultupdate = new CM_Transaction_ItemsDMO();
                                resultupdate.CMTRANS_Id = finalresult.CMTRANS_Id;
                                resultupdate.CMMFI_Id = data.CMTransactionItems[i].cmmfI_Id;
                                resultupdate.CMTRANS_Qty = data.CMTransactionItems[i].itemCount;
                                resultupdate.CMTRANSI_UnitRate = data.CMTransactionItems[i].unitRate;
                                resultupdate.CMTRANSI_name = data.CMTransactionItems[i].CMTRANSI_name;
                                resultupdate.CMTRANSI_UpdatedBy = data.UserId;
                                resultupdate.CMTRANSI_Updateddate = DateTime.Now;

                                _fmtContext.CM_Transaction_ItemsDMO.Add(resultupdate);
                            }

                            for (int i = 0; i < data.CMTransactionPaymentMode.Length; i++)
                            {
                                CM_Transaction_PaymentModeDMO resultupdate = new CM_Transaction_PaymentModeDMO();
                                resultupdate.CMTRANS_Id = finalresult.CMTRANS_Id;
                                resultupdate.CMTRANSPM_PaymentModeId = data.CMTransactionPaymentMode[i].CMTRANSPM_PaymentModeId;
                                resultupdate.CMTRANSPM_PaymentMode = data.CMTransactionPaymentMode[i].CMTRANSPM_PaymentMode;
                                resultupdate.CMTRANSPM_UpdatedBy = data.UserId;
                                resultupdate.CMTRANSPM_Updateddate = DateTime.Now;

                                _fmtContext.CM_Transaction_PaymentModeDMO.Add(resultupdate);
                            }


                            var contactExistss = _fmtContext.SaveChanges();

                            if (contactExistss > 0)
                            {

                                data.returnval = "save";
                                data.CMTRANS_Id = _fmtContext.FoodtransactionDMO.OrderByDescending(R => R.CMTRANS_Id).FirstOrDefault().CMTRANS_Id;
                                data.paymenttrans = _fmtContext.FoodtransactionDMO.ToArray();


                            }
                            else
                            {
                                data.returnval = "Notsave";
                            }

                        }
                        else
                        {
                            data.returnval = "not sucessfully";
                        }

                    }


                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }


        public FoodtransactionDTO paymenthistory(FoodtransactionDTO data)
        {
            try
            {

                using (var cmd = _fmtContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Payment_deatils";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@CMTRANS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.CMTRANS_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        data.Payment_deatils = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return data;
        }



        public FoodtransactionDTO deactivate(FoodtransactionDTO acd)
        {
            try
            {



                //   var ismapped = _fmtContext.FooditeamDMO.Single(t => t.CMMFI_Id == acd.CMMFI_Id);

                if (acd.CMMFIT_Id > 0)
                {
                    var result = _fmtContext.FooditemtaxDMO.Single(t => t.CMMFIT_Id == acd.CMMFIT_Id);

                    if (acd.CMMFIT_ActiveFlg == true)
                    {
                        result.CMMFIT_ActiveFlg = false;
                    }
                    else if (acd.CMMFIT_ActiveFlg == false)
                    {
                        result.CMMFIT_ActiveFlg = true;
                    }

                    result.CMMFIT_Updateddate = DateTime.Now;

                    _fmtContext.Update(result);
                    var flag = _fmtContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.CMMFIT_ActiveFlg == true)
                        {

                            acd.returnval = "Foodtax Activated Successfully.";
                        }
                        else
                        {
                            acd.returnval = "Foodtax Deactivated Successfully.";
                        }
                    }
                    else
                    {
                        acd.returnval = "Foodtax Not Activated/Deactivated";
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

       

        public FoodtransactionDTO getstudent(FoodtransactionDTO data)        {            try            {                using (var cmd = _fmtContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "ADM_RF_CARDS_Studentdetails";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_id",                      SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });                    cmd.Parameters.Add(new SqlParameter("@AMCTST_IP",                      SqlDbType.BigInt)                    {                        Value = data.AMCTST_IP                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                    dataReader.GetName(iFiled),                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.getstudentdetails = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.Write(ex.Message);                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.InnerException);            }            return data;        }


        public FoodtransactionDTO savesmartData(FoodtransactionDTO data)
        {
            try
            {
                data.School_Flag = "";
                //AMCTST_IP
                //var contactExistsP = _fmtContext.Database.ExecuteSqlCommand("ADM_Canteen_CardInsert @p0,@p1,@p2", data.MI_Id, data.AMST_Id, data.AMCTST_IP);
                var contactExistsP = _fmtContext.Database.ExecuteSqlCommand("ADM_Canteen_CardInsert @p0,@p1,@p2,@p3", data.MI_Id, data.AMST_Id, data.AMCTST_IP, data.School_Flag);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return data;
        }

        public FoodtransactionDTO trns_cancel(FoodtransactionDTO data)        {            try            {                using (var cmd = _fmtContext.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "ADM_RF_CARDS_Studentdetails_Cancel";                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_id",                      SqlDbType.BigInt)                    {                        Value = data.MI_Id                    });                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",                      SqlDbType.BigInt)                    {                        Value = data.AMST_Id                    });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)                                {                                    dataRow.Add(                                    dataReader.GetName(iFiled),                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}                                    );                                }                                retObject.Add((ExpandoObject)dataRow);                            }                        }                        data.getstudentdetails_cancel = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.Write(ex.Message);                    }                }            }            catch (Exception ex)            {                Console.WriteLine(ex.InnerException);            }            return data;        }

        public FoodtransactionDTO orderdeatils(FoodtransactionDTO acd)
        {
            try
            {
                using (var cmd = _fmtContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CM_Canteen_OrderDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        acd.order_deatils = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }


        public FoodtransactionDTO foodreport(FoodtransactionDTO acd)
        {
            try
            {
                using (var cmd = _fmtContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CM_Item_Date_Categorywise_OrderDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Flag",  SqlDbType.VarChar)                    {                        Value = acd.Flag                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate",SqlDbType.VarChar)                    {                        Value = acd.Fromdate                    });

                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar)                    {                        Value = acd.Todate                    });
                    cmd.Parameters.Add(new SqlParameter("@ItemName", SqlDbType.VarChar)                    {                        Value = acd.ItemName                    });
                    cmd.Parameters.Add(new SqlParameter("@Category", SqlDbType.VarChar)                    {                        Value = acd.Category                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        acd.food_OrderDetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        

        public FoodtransactionDTO Month_Daywise_graph(FoodtransactionDTO acd)
        {
            try
            {
                using (var cmd = _fmtContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CM_Month_Daywise_TotalCollection";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_id", SqlDbType.BigInt)                    {                        Value = acd.MI_Id                    });
                    
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        acd.Month_Daywise_deatils = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }


        public FoodtransactionDTO YearWise_graph(FoodtransactionDTO acd)
        {
            try
            {
                using (var cmd = _fmtContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CM_YearWise_TotalCollection";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_id",SqlDbType.BigInt)                    {                        Value = acd.MI_Id                    });
                    
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        acd.YearWise_deatils = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }


        public FoodtransactionDTO paymenthistory_print(FoodtransactionDTO data)
        {
            try
            {

                using (var cmd = _fmtContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Payment_deatils_print";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@CM_orderID",
                      SqlDbType.BigInt)
                    {
                        Value = data.CM_orderID
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        data.Payment_deatils_print = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return data;
        }


        public FoodtransactionDTO paymenthistory_print_onetime(FoodtransactionDTO data)
        {
            try
            {

                using (var cmd = _fmtContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Payment_deatils_print_onetime";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@CM_orderID",
                      SqlDbType.BigInt)
                    {
                        Value = data.CM_orderID
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
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
                        data.Payment_deatils_print = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException);
            }
            return data;
        }


    }
}
