using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class ClientProformaInvoiceImpl : Interface.ClientProformaInvoiceInterface
    {
        //public IssueManagerContext _INVContext;
        public InventoryContext _INVContext;
        public DomainModelMsSqlServerContext _db;
        public ClientProformaInvoiceImpl(InventoryContext hh, DomainModelMsSqlServerContext db)
        {
            _INVContext = hh;
            _db = db;
            //
        }
        public ClientProformaInvoiceDTO loaddata(ClientProformaInvoiceDTO data)
        {
            try
            {
                data.allcompany = _INVContext.Institution.Where(a => a.MI_ActiveFlag == 1).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public ClientProformaInvoiceDTO companychange(ClientProformaInvoiceDTO data)
        {
            try
            {
                data.clientlist = (from a in _INVContext.clientTable
                                   from b in _INVContext.ISM_Master_Client_ProjectDMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                   select new ClientProformaInvoiceDTO
                                   {
                                       ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                       ISMMCLT_Id = b.ISMMCLT_Id
                                   }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();


                List<AcademicYear> list = new List<AcademicYear>();
                list = _INVContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                data.taxlist = _INVContext.INV_Master_TaxDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMT_ActiveFlg == true).Distinct().ToArray();

                data.instlist = _INVContext.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).Distinct().ToArray();


                data.instlistmobile = _INVContext.Institution_Phone_No.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();

                data.instlistemail = _INVContext.Institution_EmailId.Where(a => a.MI_Id == data.MI_Id).Distinct().ToArray();


                data.modeofpaymentlist = _db.IVRM_ModeOfPaymentDMO.Where(a => a.MI_Id == data.MI_Id && a.IVRMMOD_ActiveFlag == true).Distinct().ToArray();

                data.banklist = _db.HR_Master_BankDeatils.Where(a => a.MI_Id == data.MI_Id && a.HRMBD_ActiveFlag == true).Distinct().ToArray();
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LOAD_CLIENT_PROPORMAINVIOCE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.alldata = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


                //data.getinstallment = (from a in _INVContext.FeeInstallmentDMO
                //                       from b in _INVContext.FeeInstallmentsyearlyDMO
                //                       where (a.FMI_Id == b.FMI_Id && a.FMI_ActiceFlag == true && a.MI_Id == data.MI_Id && b.MI_ID == data.MI_Id)
                //                       select b).Distinct().OrderBy(a => a.FTI_Name).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ClientProformaInvoiceDTO getProject(ClientProformaInvoiceDTO data)
        {
            try
            {

                data.projectlist = (from a in _INVContext.clientTable
                                    from b in _INVContext.MastersProject_DMO
                                    from c in _INVContext.ISM_Master_Client_ProjectDMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == data.ISMMCLT_Id)
                                    select new ClientProformaInvoiceDTO
                                    {
                                        ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                        ISMMPR_Id = c.ISMMPR_Id,
                                    }).Distinct().ToArray();


                data.clientlist = _INVContext.clientTable.Where(w => w.ISMMCLT_Id == data.ISMMCLT_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientProformaInvoiceDTO getbom(ClientProformaInvoiceDTO data)
        {
            try
            {
                data.projectlist = (from a in _INVContext.clientTable
                                    from b in _INVContext.MastersProject_DMO
                                    from c in _INVContext.ISM_Master_Client_ProjectDMO
                                    from d in _INVContext.ISM_Client_Master_Components_DMO
                                    from e in _INVContext.ISM_Client_Project_BOM_DMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == data.ISMMCLT_Id && c.ISMMCLTPR_Id == e.ISMMCLTPR_Id && d.ISMCLTC_Id == e.ISMCLTC_Id && e.ISMCLTPRBOM_ActiveFlag == true && d.ISMCLTC_ActiveFlag == true && a.MI_Id == d.MI_Id)
                                    select new ClientProformaInvoiceDTO
                                    {
                                        ISMCLTC_Id = d.ISMCLTC_Id,
                                        ISMCLTC_Name = d.ISMCLTC_Name,
                                        ISMCLTPRBOM_Qty = e.ISMCLTPRBOM_Qty,
                                    }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientProformaInvoiceDTO savedata(ClientProformaInvoiceDTO data)
        {
            try
            {
                if (data.ISMPRINC_Id == 0)
                {
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_PEFORMINVOICE_UPDATE";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                        { Value = data.asmaY_Id });
                        cmd.Parameters.Add(new SqlParameter("@StudentAdmnoOP", SqlDbType.VarChar, Int32.MaxValue)
                        { Direction = ParameterDirection.Output });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        data.trans_id = cmd.Parameters["@StudentAdmnoOP"].Value.ToString();
                    }
                    //if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    //{
                    //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    //    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    //    data.transnumbconfigurationsettingsss.ASMAY_Id = data.asmaY_Id;
                    //    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                    //}
                    ISM_Proforma_InvoiceDMO obj = new ISM_Proforma_InvoiceDMO();
                    obj.ISMMCLT_Id = data.ISMMCLT_Id;
                    obj.MI_Id = data.MI_Id;
                    obj.ISMMPR_Id = data.ISMMPR_Id;
                    obj.ISMPRINC_WorkOrder = data.ISMPRINC_WorkOrder;
                    obj.ISMPRINC_PrInviceNo = data.trans_id;
                    obj.ISMPRINC_Date = Convert.ToDateTime(data.ISMCLTPRP_PaymentDate);
                    obj.ISMPRINC_TotalTaxAmount = data.ISMPRINC_TotalTaxAmount;
                    obj.ISMPRINC_TotalAmount = data.ISMPRINC_TotalAmount;
                    obj.ISMPRINC_Remarks = data.ISMPRINC_Remarks;
                    obj.ISMPRINC_ActiveFlag = true;
                    obj.ISMPRINC_CreatedDate = DateTime.Now;
                    obj.ISMPRINC_CreatedBy = data.UserId;
                    obj.ISMPRINC_UpdatedDate = DateTime.Now;
                    obj.ISMPRINC_UpdatedBy = data.UserId;
                    obj.ISMPRINC_AdvPer = data.ISMPRINC_AdvPer;
                    obj.ISMPRINC_AdvanceAmount = data.ISMPRINC_AdvanceAmount;
                    obj.ISMPRINC_MOURefNo = data.ISMPRINC_MOURefNo;
                    //obj.ISMPRINC_InstallmentName = data.ISMPRINC_InstallmentName;

                    if (data.ISMPRINC_MOURefNo == "" || data.ISMPRINC_MOURefNo == null)
                    {
                        obj.ISMPRINC_MOUDate = null;
                    }
                    else
                    {
                        obj.ISMPRINC_MOUDate = data.ISMPRINC_MOUDate;
                    }

                    obj.HRMBD_Id = data.HRMBD_Id;
                    obj.ISMPRINC_ModeOfPayment = data.ISMPRINC_ModeOfPayment;

                    _INVContext.Add(obj);
                    if (data.itemsdto != null)
                    {
                        foreach (var item in data.itemsdto)
                        {
                            ISM_Proforma_Invoice_DetailsDMO obj1 = new ISM_Proforma_Invoice_DetailsDMO();
                            obj1.ISMPRINC_Id = obj.ISMPRINC_Id;
                            obj1.ISMCLTC_Id = item.ISMCLTC_Id;
                            obj1.ISMCLTPRMP_Id = item.ISMCLTPRMP_Id;
                            obj1.ISMPRINCD_Qty = item.ISMPRINCD_Qty;
                            obj1.ISMPRINCD_UnitRate = item.ISMPRINCD_UnitRate;
                            obj1.ISMPRINCD_Amount = item.ISMPRINCD_Amount;
                            obj1.ISMPRINCD_ItemDesc = item.ISMPRINCD_ItemDesc;
                            obj1.ISMPRINCD_Remarks = item.ISMPRINCD_Remarks;
                            obj1.ISMPRINCD_ActiveFlag = true;
                            obj1.ISMPRINCD_CreatedDate = DateTime.Now;
                            obj1.ISMPRINCD_CreatedBy = data.UserId;
                            obj1.ISMPRINCD_UpdatedDate = DateTime.Now;
                            obj1.ISMPRINCD_UpdatedBy = data.UserId;
                            ///obj1.ISMPRINCD_HSNCode = item.ISMPRINCD_HSNCode;
                            //obj1.ISMPRINCD_SACCode = item.ISMPRINCD_SACCode;
                            _INVContext.Add(obj1);
                        }
                    }

                    if (data.taxdto != null)
                    {
                        foreach (var item in data.taxdto)
                        {
                            ISM_Proforma_Invoice_TaxDMO obj2 = new ISM_Proforma_Invoice_TaxDMO();
                            obj2.ISMPRINC_Id = obj.ISMPRINC_Id;
                            obj2.INVMT_Id = item.INVMT_Id;
                            obj2.ISMMTTX_TaxPercent = item.INVMIT_TaxValue;
                            obj2.ISMMTTX_TaxAmount = item.taxamount;
                            obj2.ISMMTTX_ActiveFlag = true;
                            obj2.ISMMTTX_CreatedDate = DateTime.Now;
                            obj2.ISMMTTX_CreatedBy = data.UserId;
                            obj2.ISMMTTX_UpdatedDate = DateTime.Now;
                            obj2.ISMMTTX_UpdatedBy = data.UserId;
                            _INVContext.Add(obj2);
                        }
                    }

                    int cxt = _INVContext.SaveChanges();
                    if (cxt > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }

                }
                else
                {

                    var update = _INVContext.ISM_Proforma_InvoiceDMO.Single(e => e.ISMPRINC_Id == data.ISMPRINC_Id);
                    update.ISMPRINC_WorkOrder = data.ISMPRINC_WorkOrder;
                    update.ISMPRINC_Date = Convert.ToDateTime(data.ISMCLTPRP_PaymentDate);
                    update.ISMPRINC_TotalTaxAmount = data.ISMPRINC_TotalTaxAmount;
                    update.ISMPRINC_TotalAmount = data.ISMPRINC_TotalAmount;
                    update.ISMPRINC_Remarks = data.ISMPRINC_Remarks;
                    update.ISMPRINC_ActiveFlag = true;
                    update.ISMPRINC_UpdatedDate = DateTime.Now;
                    update.ISMPRINC_UpdatedBy = data.UserId;
                    update.ISMPRINC_AdvPer = data.ISMPRINC_AdvPer;
                    update.ISMPRINC_AdvanceAmount = data.ISMPRINC_AdvanceAmount;
                    update.ISMPRINC_MOURefNo = data.ISMPRINC_MOURefNo;
                    //update.ISMPRINC_InstallmentName = data.ISMPRINC_InstallmentName;

                    if (data.ISMPRINC_MOURefNo == "" || data.ISMPRINC_MOURefNo == null)
                    {
                        update.ISMPRINC_MOUDate = null;
                    }
                    else
                    {
                        update.ISMPRINC_MOUDate = data.ISMPRINC_MOUDate;
                    }

                    update.HRMBD_Id = data.HRMBD_Id;
                    update.ISMPRINC_ModeOfPayment = data.ISMPRINC_ModeOfPayment;
                    _INVContext.Update(update);

                    var detailslist = _INVContext.ISM_Proforma_Invoice_DetailsDMO.Where(e => e.ISMPRINC_Id == data.ISMPRINC_Id).ToList();
                    if (detailslist.Count > 0)
                    {
                        foreach (var item in detailslist)
                        {
                            _INVContext.Remove(item);
                        }
                    }

                    var taxlist = _INVContext.ISM_Proforma_Invoice_TaxDMO.Where(e => e.ISMPRINC_Id == data.ISMPRINC_Id).ToList();
                    if (taxlist.Count > 0)
                    {
                        foreach (var item in taxlist)
                        {
                            _INVContext.Remove(item);
                        }
                    }

                    if (data.itemsdto != null)
                    {
                        foreach (var item in data.itemsdto)
                        {
                            ISM_Proforma_Invoice_DetailsDMO obj1 = new ISM_Proforma_Invoice_DetailsDMO();
                            obj1.ISMPRINC_Id = data.ISMPRINC_Id;
                            obj1.ISMCLTC_Id = item.ISMCLTC_Id;
                            obj1.ISMCLTPRMP_Id = item.ISMCLTPRMP_Id;
                            obj1.ISMPRINCD_Qty = item.ISMPRINCD_Qty;
                            obj1.ISMPRINCD_UnitRate = item.ISMPRINCD_UnitRate;
                            obj1.ISMPRINCD_Amount = item.ISMPRINCD_Amount;
                            obj1.ISMPRINCD_ItemDesc = item.ISMPRINCD_ItemDesc;
                            obj1.ISMPRINCD_Remarks = item.ISMPRINCD_Remarks;
                            obj1.ISMPRINCD_ActiveFlag = true;
                            obj1.ISMPRINCD_CreatedDate = DateTime.Now;
                            obj1.ISMPRINCD_CreatedBy = data.UserId;
                            obj1.ISMPRINCD_UpdatedDate = DateTime.Now;
                            obj1.ISMPRINCD_UpdatedBy = data.UserId;
                            //obj1.ISMPRINCD_HSNCode = item.ISMPRINCD_HSNCode;
                            //obj1.ISMPRINCD_SACCode = item.ISMPRINCD_SACCode;
                            _INVContext.Add(obj1);

                        }
                    }

                    if (data.taxdto != null)
                    {
                        foreach (var item in data.taxdto)
                        {
                            ISM_Proforma_Invoice_TaxDMO obj2 = new ISM_Proforma_Invoice_TaxDMO();
                            obj2.ISMPRINC_Id = data.ISMPRINC_Id;
                            obj2.INVMT_Id = item.INVMT_Id;
                            obj2.ISMMTTX_TaxPercent = item.INVMIT_TaxValue;
                            obj2.ISMMTTX_TaxAmount = item.taxamount;
                            obj2.ISMMTTX_ActiveFlag = true;
                            obj2.ISMMTTX_CreatedDate = DateTime.Now;
                            obj2.ISMMTTX_CreatedBy = data.UserId;
                            obj2.ISMMTTX_UpdatedDate = DateTime.Now;
                            obj2.ISMMTTX_UpdatedBy = data.UserId;
                            _INVContext.Add(obj2);
                        }
                    }

                    int cxt = _INVContext.SaveChanges();
                    if (cxt > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public ClientProformaInvoiceDTO getinvoiceno(ClientProformaInvoiceDTO data)
        {
            try
            {
                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.asmaY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public ClientProformaInvoiceDTO Editdata(ClientProformaInvoiceDTO data)
        {
            try
            {

                var editlist = _INVContext.ISM_Proforma_InvoiceDMO.Where(w => w.ISMPRINC_Id == data.ISMPRINC_Id).Distinct().ToList();

                data.editlist = editlist.ToArray();


                data.clientlist = _INVContext.clientTable.Where(d => d.ISMMCLT_Id == editlist.FirstOrDefault().ISMMCLT_Id).ToArray();

                data.geteditclient = (from a in _INVContext.clientTable
                                      from b in _INVContext.ISM_Master_Client_ProjectDMO
                                      where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id
                                      && a.ISMMCLT_Id == editlist.FirstOrDefault().ISMMCLT_Id)
                                      select new ClientProformaInvoiceDTO
                                      {
                                          ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                          ISMMCLT_Id = b.ISMMCLT_Id
                                      }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();


                data.projectlist = (from a in _INVContext.clientTable
                                    from b in _INVContext.MastersProject_DMO
                                    from c in _INVContext.ISM_Master_Client_ProjectDMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == editlist.FirstOrDefault().ISMMCLT_Id)
                                    select new ClientProformaInvoiceDTO
                                    {
                                        ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                        ISMMPR_Id = c.ISMMPR_Id,
                                    }).Distinct().ToArray();

                data.editlistdetails = _INVContext.ISM_Proforma_Invoice_DetailsDMO.Where(w => w.ISMPRINC_Id == data.ISMPRINC_Id).ToArray();
                data.editlisttax = _INVContext.ISM_Proforma_Invoice_TaxDMO.Where(w => w.ISMPRINC_Id == data.ISMPRINC_Id).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientProformaInvoiceDTO clientDecative(ClientProformaInvoiceDTO data)
        {
            try
            {
                var u = _INVContext.ISM_Proforma_InvoiceDMO.Where(t => t.ISMPRINC_Id == data.ISMPRINC_Id).SingleOrDefault();
                if (u.ISMPRINC_ActiveFlag == true)
                {
                    u.ISMPRINC_ActiveFlag = false;
                }
                else if (u.ISMPRINC_ActiveFlag == false)
                {
                    u.ISMPRINC_ActiveFlag = true;
                }

                u.ISMPRINC_UpdatedBy = data.UserId;
                u.ISMPRINC_UpdatedDate = DateTime.Now;
                _INVContext.Update(u);
                int o = _INVContext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
        public ClientProformaInvoiceDTO viewdetails(ClientProformaInvoiceDTO data)
        {
            try
            {
                data.paymentmodedetails = _INVContext.IVRM_ModeOfPaymentDMO.Where(a => a.MI_Id == data.MI_Id
                && a.IVRMMOD_ActiveFlag == true).Distinct().ToArray();

                data.getpaymentdetails = _INVContext.ISM_Client_Project_Payment_DetailsDMO.Where(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClientProformaInvoiceDTO savepaymentdetailsrecord(ClientProformaInvoiceDTO data)
        {
            //try
            //{
            //    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

            //    string paymentstatus = "";                 

            //    var getinstallmentamt = _INVContext.ISM_Master_ClientProject_PaymentDMO.Where(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id).ToList();
            //    decimal? installmentamt = 0;
            //    decimal? balanceamt = 0;
            //    decimal? excessamt = 0;
            //    installmentamt = getinstallmentamt.FirstOrDefault().ISMCLTPRP_InstallmentAmt;

            //    decimal? installmentpaidamt = 0;

            //    if (data.ISM_Client_Project_Payment_Details.Length > 0)
            //    {
            //        foreach (var c in data.ISM_Client_Project_Payment_Details)
            //        {
            //            installmentpaidamt = installmentpaidamt + c.ISMCPPD_ReceivedAmount;

            //            if (c.ISMCPPD_Id > 0)
            //            {
            //                var checkresult = _INVContext.ISM_Client_Project_Payment_DetailsDMO.Single(a => a.ISMCPPD_Id == c.ISMCPPD_Id);
            //                checkresult.ISMCPPD_ReceivedAmount = c.ISMCPPD_ReceivedAmount;
            //                checkresult.ISMCPPD_ReceivedDate = c.ISMCPPD_ReceivedDate;
            //                checkresult.IVRMMOD_Id = c.IVRMMOD_Id;
            //                checkresult.ISMCPPD_PaymentRefNo = c.ISMCPPD_PaymentRefNo;
            //                checkresult.ISMCPPD_Remarks = c.ISMCPPD_Remarks;
            //                if (c.flag == 1)
            //                {
            //                    checkresult.ISMCPPD_ChequeDate = c.ISMCPPD_ChequeDate;
            //                }
            //                else
            //                {
            //                    checkresult.ISMCPPD_ChequeDate = null;
            //                }
            //                checkresult.ISMCPPD_UpdatedBy = data.UserId;
            //                checkresult.Updateddate = indiantime0;
            //                _INVContext.Update(checkresult);
            //            }

            //            else
            //            {
            //                ISM_Client_Project_Payment_DetailsDMO iSM_Client_Project_Payment_DetailsDMO = new ISM_Client_Project_Payment_DetailsDMO();
            //                iSM_Client_Project_Payment_DetailsDMO.ISMCLTPRP_Id = data.ISMCLTPRP_Id;
            //                iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_ReceivedAmount = c.ISMCPPD_ReceivedAmount;
            //                iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_ReceivedDate = c.ISMCPPD_ReceivedDate;
            //                iSM_Client_Project_Payment_DetailsDMO.IVRMMOD_Id = c.IVRMMOD_Id;
            //                iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_PaymentRefNo = c.ISMCPPD_PaymentRefNo;
            //                iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_Remarks = c.ISMCPPD_Remarks;
            //                if (c.flag == 1)
            //                {
            //                    iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_ChequeDate = c.ISMCPPD_ChequeDate;
            //                }
            //                else
            //                {
            //                    iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_ChequeDate = null;
            //                }
            //                iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_CreatedBy = data.UserId;
            //                iSM_Client_Project_Payment_DetailsDMO.ISMCPPD_UpdatedBy = data.UserId;
            //                iSM_Client_Project_Payment_DetailsDMO.Createddate = indiantime0;
            //                iSM_Client_Project_Payment_DetailsDMO.Updateddate = indiantime0;
            //                _INVContext.Add(iSM_Client_Project_Payment_DetailsDMO);
            //            }
            //        }

            //        if (installmentamt == installmentpaidamt)
            //        {
            //            paymentstatus = "Full Payment";
            //            balanceamt = 0;
            //            excessamt = 0;
            //        }

            //        else if (installmentpaidamt < installmentamt)
            //        {
            //            paymentstatus = "Partial Payment";
            //            balanceamt = installmentamt - installmentpaidamt;
            //            excessamt = 0;
            //        }
            //        else if (installmentpaidamt > installmentamt)
            //        {
            //            paymentstatus = "Full Payment";
            //            excessamt = installmentpaidamt - installmentamt;
            //            balanceamt = 0;
            //        }

            //        var checkresultpayment = _INVContext.ISM_Master_ClientProject_PaymentDMO.Single(a => a.ISMCLTPRP_Id == data.ISMCLTPRP_Id);
            //        checkresultpayment.ISMCLTPRP_PaymentStatus = paymentstatus;
            //        checkresultpayment.ISMCLTPRP_BalanceAmt = balanceamt;
            //        checkresultpayment.ISMCLTPRP_ExcessAmt = excessamt;
            //        checkresultpayment.UpdatedDate = indiantime0;
            //        checkresultpayment.ISMCLTPRP_UpdatedBy = data.UserId;
            //        _INVContext.Update(checkresultpayment);

            //        var i = _INVContext.SaveChanges();

            //        if (i > 0)
            //        {
            //            data.returnval = true;
            //        }
            //        else
            //        {
            //            data.returnval = false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    data.returnval = false;
            //    Console.WriteLine(ex.Message);
            //}
            return data;
        }

        // Payment Configuration
        public ClientProformaInvoiceDTO loaddataconfig(ClientProformaInvoiceDTO data)
        {
            try
            {
                data.getconfigloaddata = _INVContext.ISM_Client_Payment_ConfigurationDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClientProformaInvoiceDTO savedataconfig(ClientProformaInvoiceDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.ISMCPC_Id > 0)
                {
                    var checkresult = _INVContext.ISM_Client_Payment_ConfigurationDMO.Single(a => a.ISMCPC_Id == data.ISMCPC_Id);
                    checkresult.ISMCPC_RemainderDays = data.ISMCPC_RemainderDays;
                    checkresult.ISMCPC_FullORPartialPayment = data.ISMCPC_FullORPartialPayment;
                    checkresult.UpdatedDate = indiantime0;
                    _INVContext.Update(checkresult);
                    var i = _INVContext.SaveChanges();
                    if (i > 0)
                    {
                        data.msg = "updated";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }
                else
                {
                    ISM_Client_Payment_ConfigurationDMO iSM_Client_Payment_ConfigurationDMO = new ISM_Client_Payment_ConfigurationDMO();
                    iSM_Client_Payment_ConfigurationDMO.ISMCPC_RemainderDays = data.ISMCPC_RemainderDays;
                    iSM_Client_Payment_ConfigurationDMO.ISMCPC_FullORPartialPayment = data.ISMCPC_FullORPartialPayment;
                    iSM_Client_Payment_ConfigurationDMO.UpdatedDate = indiantime0;
                    iSM_Client_Payment_ConfigurationDMO.CreatedDate = indiantime0;
                    _INVContext.Add(iSM_Client_Payment_ConfigurationDMO);

                    var i = _INVContext.SaveChanges();
                    if (i > 0)
                    {
                        data.msg = "saved";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                data.msg = "Failed";
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Payment subscription audit report
        public ClientProformaInvoiceDTO loaddatareport(ClientProformaInvoiceDTO data)
        {
            try
            {
                data.clientlist = (from a in _INVContext.clientTable
                                   from b in _INVContext.ISM_Master_Client_ProjectDMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                   select new ClientProformaInvoiceDTO
                                   {
                                       ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                       ISMMCLT_Id = b.ISMMCLT_Id
                                   }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClientProformaInvoiceDTO getreport(ClientProformaInvoiceDTO data)
        {
            try
            {
                var geturl = _INVContext.clientTable.Where(a => a.ISMMCLT_Id == data.ISMMCLT_Id).ToList();

                if (geturl.Count > 0)
                {
                    string url = geturl.FirstOrDefault().ISMMCLT_IVRM_URL;

                    ClientProformaInvoiceDTO dtonew = new ClientProformaInvoiceDTO();

                    dtonew.MI_Id = Convert.ToInt64(geturl.FirstOrDefault().IVRM_MI_Id);
                    dtonew.fromdate = data.fromdate;
                    dtonew.todate = data.todate;
                    // url = "http://localhost:51263/";
                    if (url != null && url != "")
                    {
                        HttpClient client1 = new HttpClient();
                        client1.BaseAddress = new Uri(url);
                        client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client1.PostAsJsonAsync("api/EmployeePtalFacade/getvmspaymentsubsctiptionreport", dtonew).Result;

                        string description = string.Empty;
                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            description = result;

                            dtonew = JsonConvert.DeserializeObject<ClientProformaInvoiceDTO>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                            data.getreportdetails = dtonew.getreportdetails;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //payment notification
        public ClientProformaInvoiceDTO paymentnotification(ClientProformaInvoiceDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ISM_GET_PAYMENT_NOTIFICATION_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@IVRM_MI_Id", SqlDbType.VarChar) { Value = data.IVRM_MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMMCLT_ClientCode", SqlDbType.VarChar) { Value = data.ISMMCLT_ClientCode });

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

                        data.getpaymentnotificationdetails = retObject.ToArray();
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

    }
}
