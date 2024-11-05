using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Inventory;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class ClientInvoiceImpl : Interface.ClientInvoiceInterface
    {
        public InventoryContext _INVContext;
        public DomainModelMsSqlServerContext _db;
        public ClientInvoiceImpl(InventoryContext hh, DomainModelMsSqlServerContext db)
        {
            _INVContext = hh;
            _db = db;
            //
        }
        public ClientInvoiceDTO loaddata(ClientInvoiceDTO data)
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
        public ClientInvoiceDTO companychange(ClientInvoiceDTO data)
        {
            try
            {
                data.clientlist = (from a in _INVContext.clientTable
                                   from b in _INVContext.ISM_Master_Client_ProjectDMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                   select new ClientInvoiceDTO
                                   {
                                       ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                       ISMMCLT_Id = b.ISMMCLT_Id,



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
                    cmd.CommandText = "LOAD_CLIENT_INVIOCE";
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





            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public ClientInvoiceDTO getProject(ClientInvoiceDTO data)
        {
            try
            {

                data.projectlist = (from a in _INVContext.clientTable
                                    from b in _INVContext.MastersProject_DMO
                                    from c in _INVContext.ISM_Master_Client_ProjectDMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == data.ISMMCLT_Id)
                                    select new ClientInvoiceDTO
                                    {
                                        ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                        ISMMPR_Id = c.ISMMPR_Id,
                                        ISMINC_MOURefNo = c.ISMMCLTPR_MOURefNo,
                                        ISMINC_MOUDate = c.ISMMCLTPR_MOUDate,
                                        //ISMINC_WorkOrder = c.ISMMCLTPR_WorkOrder,
                                    }).Distinct().ToArray();


                data.clientlist = _INVContext.clientTable.Where(w => w.ISMMCLT_Id == data.ISMMCLT_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientInvoiceDTO getbom(ClientInvoiceDTO data)
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
                                    select new ClientInvoiceDTO
                                    {
                                        ISMCLTC_Id = d.ISMCLTC_Id,
                                        ISMCLTC_Name = d.ISMCLTC_Name,
                                        ISMCLTPRBOM_Qty = e.ISMCLTPRBOM_Qty,
                                        ISMINC_MOURefNo = c.ISMMCLTPR_MOURefNo,
                                        ISMINC_MOUDate = c.ISMMCLTPR_MOUDate,
                                        //ISMINC_WorkOrder = c.ISMMCLTPR_WorkOrder

                                    }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientInvoiceDTO savedata(ClientInvoiceDTO data)
        {
            try
            {
                if (data.ISMINC_Id == 0)
                {
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_INVOICE_UPDATE";
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

                    ISM_InvoiceDMO obj = new ISM_InvoiceDMO();
                    obj.ISMMCLT_Id = data.ISMMCLT_Id;
                    obj.MI_Id = data.MI_Id;
                    obj.ISMMPR_Id = data.ISMMPR_Id;
                    obj.ISMINC_WorkOrder = data.ISMINC_WorkOrder;
                    obj.ISMINC_PrInviceNo = data.trans_id;
                    obj.ISMINC_Date = Convert.ToDateTime(data.ISMCLTPRP_PaymentDate);
                    obj.ISMINC_TotalTaxAmount = data.ISMINC_TotalTaxAmount;
                    obj.ISMINC_TotalAmount = data.ISMINC_TotalAmount;
                    obj.ISMINC_Remarks = data.ISMINC_Remarks;
                    obj.ISMINC_MOURefNo = data.ISMINC_MOURefNo;
                    //obj.ISMINC_InstallmentName = data.ISMINC_InstallmentName;
                    //obj.ISMINC_TotalBasicAmount = data.ISMINC_TotalBasicAmount;
                    //obj.ISMINC_TotalPercentage = data.ISMINC_TotalPercentage;

                    if (data.ISMINC_MOURefNo == "" || data.ISMINC_MOURefNo == null)
                    {
                        obj.ISMINC_MOUDate = null;
                    }
                    else
                    {
                        obj.ISMINC_MOUDate = data.ISMINC_MOUDate;
                    }

                    obj.HRMBD_Id = data.HRMBD_Id;
                    obj.ISMINC_ModeOfPayment = data.ISMINC_ModeOfPayment;
                    obj.ISMINC_ActiveFlag = true;
                    obj.ISMINC_CreatedDate = DateTime.Now;
                    obj.ISMINC_CreatedBy = data.UserId;
                    obj.ISMINC_UpdatedDate = DateTime.Now;
                    obj.ISMINC_UpdatedBy = data.UserId;
                    _INVContext.Add(obj);
                    if (data.itemsdto != null)
                    {
                        foreach (var item in data.itemsdto)
                        {
                            ISM_Invoice_DetailsDMO obj1 = new ISM_Invoice_DetailsDMO();
                            obj1.ISMINC_Id = obj.ISMINC_Id;
                            obj1.ISMCLTC_Id = item.ISMCLTC_Id;
                            obj1.ISMCLTPRMP_Id = item.ISMCLTPRMP_Id;
                            obj1.ISMINCD_Qty = item.ISMINCD_Qty;
                            obj1.ISMINCD_UnitRate = item.ISMINCD_UnitRate;
                            obj1.ISMINCD_Amount = item.ISMINCD_Amount;
                            obj1.ISMINCD_ItemDesc = item.ISMINCD_ItemDesc;
                            obj1.ISMINCD_Remarks = item.ISMINCD_Remarks;
                            obj1.ISMINCD_ActiveFlag = true;
                            obj1.ISMINCD_CreatedDate = DateTime.Now;
                            obj1.ISMINCD_CreatedBy = data.UserId;
                            obj1.ISMINCD_UpdatedDate = DateTime.Now;
                            obj1.ISMINCD_UpdatedBy = data.UserId;
                            //obj1.ISMINCD_HSNCode = item.ISMINCD_HSNCode;
                            //obj1.ISMINCD_SACCode = item.ISMINCD_SACCode;
                            _INVContext.Add(obj1);
                        }
                    }
                    if (data.taxdto != null)
                    {
                        foreach (var item in data.taxdto)
                        {
                            ISM_Invoice_TaxDMO obj2 = new ISM_Invoice_TaxDMO();
                            obj2.ISMINC_Id = obj.ISMINC_Id;
                            obj2.INVMT_Id = item.INVMT_Id;
                            obj2.ISMINTX_TaxPercent = item.INVMIT_TaxValue;
                            obj2.ISMINTX_TaxAmount = item.taxamount;
                            obj2.ISMINTX_ActiveFlag = true;
                            obj2.ISMINTX_CreatedDate = DateTime.Now;
                            obj2.ISMINTX_CreatedBy = data.UserId;
                            obj2.ISMINTX_UpdatedDate = DateTime.Now;
                            obj2.ISMINTX_UpdatedBy = data.UserId;
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
                    var update = _INVContext.ISM_InvoiceDMO.Single(e => e.ISMINC_Id == data.ISMINC_Id);
                    update.ISMINC_WorkOrder = data.ISMINC_WorkOrder;
                    update.ISMINC_Date = Convert.ToDateTime(data.ISMCLTPRP_PaymentDate);
                    update.ISMINC_TotalTaxAmount = data.ISMINC_TotalTaxAmount;
                    update.ISMINC_TotalAmount = data.ISMINC_TotalAmount;
                    update.ISMINC_Remarks = data.ISMINC_Remarks;
                    update.ISMINC_MOURefNo = data.ISMINC_MOURefNo;
                    //update.ISMINC_InstallmentName = data.ISMINC_InstallmentName;
                    if (data.ISMINC_MOURefNo == "" || data.ISMINC_MOURefNo == null)
                    {
                        update.ISMINC_MOUDate = null;
                    }
                    else
                    {
                        update.ISMINC_MOUDate = data.ISMINC_MOUDate;
                    }
                    update.HRMBD_Id = data.HRMBD_Id;
                    update.ISMINC_ModeOfPayment = data.ISMINC_ModeOfPayment;
                    update.ISMINC_ActiveFlag = true;
                    update.ISMINC_UpdatedDate = DateTime.Now;
                    update.ISMINC_UpdatedBy = data.UserId;
                    //update.ISMINC_TotalBasicAmount = data.ISMINC_TotalBasicAmount;
                    //update.ISMINC_TotalPercentage = data.ISMINC_TotalPercentage;
                    _INVContext.Update(update);

                    var detailslist = _INVContext.ISM_Invoice_DetailsDMO.Where(e => e.ISMINC_Id == data.ISMINC_Id).ToList();
                    if (detailslist.Count > 0)
                    {
                        foreach (var item in detailslist)
                        {
                            _INVContext.Remove(item);
                        }
                    }

                    var taxlist = _INVContext.ISM_Invoice_TaxDMO.Where(e => e.ISMINC_Id == data.ISMINC_Id).ToList();
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
                            ISM_Invoice_DetailsDMO obj1 = new ISM_Invoice_DetailsDMO();
                            obj1.ISMINC_Id = data.ISMINC_Id;
                            obj1.ISMCLTC_Id = item.ISMCLTC_Id;
                            obj1.ISMCLTPRMP_Id = item.ISMCLTPRMP_Id;
                            obj1.ISMINCD_Qty = item.ISMINCD_Qty;
                            obj1.ISMINCD_UnitRate = item.ISMINCD_UnitRate;
                            obj1.ISMINCD_Amount = item.ISMINCD_Amount;
                            obj1.ISMINCD_ItemDesc = item.ISMINCD_ItemDesc;
                            obj1.ISMINCD_Remarks = item.ISMINCD_Remarks;
                            obj1.ISMINCD_ActiveFlag = true;
                            obj1.ISMINCD_CreatedDate = DateTime.Now;
                            obj1.ISMINCD_CreatedBy = data.UserId;
                            obj1.ISMINCD_UpdatedDate = DateTime.Now;
                            obj1.ISMINCD_UpdatedBy = data.UserId;
                            //obj1.ISMINCD_HSNCode = item.ISMINCD_HSNCode;
                            //obj1.ISMINCD_SACCode = item.ISMINCD_SACCode;

                            _INVContext.Add(obj1);

                        }
                    }

                    if (data.taxdto != null)
                    {
                        foreach (var item in data.taxdto)
                        {
                            ISM_Invoice_TaxDMO obj2 = new ISM_Invoice_TaxDMO();
                            obj2.ISMINC_Id = data.ISMINC_Id;
                            obj2.INVMT_Id = item.INVMT_Id;
                            obj2.ISMINTX_TaxPercent = item.INVMIT_TaxValue;
                            obj2.ISMINTX_TaxAmount = item.taxamount;
                            obj2.ISMINTX_ActiveFlag = true;
                            obj2.ISMINTX_CreatedDate = DateTime.Now;
                            obj2.ISMINTX_CreatedBy = data.UserId;
                            obj2.ISMINTX_UpdatedDate = DateTime.Now;
                            obj2.ISMINTX_UpdatedBy = data.UserId;
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


        public ClientInvoiceDTO getinvoiceno(ClientInvoiceDTO data)
        {
            try
            {

                //getinvoiceno
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public ClientInvoiceDTO Editdata(ClientInvoiceDTO data)
        {
            try
            {

                var editlist = _INVContext.ISM_InvoiceDMO.Where(w => w.ISMINC_Id == data.ISMINC_Id).Distinct().ToList();

                data.editlist = editlist.ToArray();


                data.clientlist = _INVContext.clientTable.Where(d => d.ISMMCLT_Id == editlist.FirstOrDefault().ISMMCLT_Id).ToArray();

                data.geteditclient = (from a in _INVContext.clientTable
                                      from b in _INVContext.ISM_Master_Client_ProjectDMO
                                      where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id
                                      && a.ISMMCLT_Id == editlist.FirstOrDefault().ISMMCLT_Id)
                                      select new ClientInvoiceDTO
                                      {
                                          ISMMCLT_ClientName = a.ISMMCLT_ClientName,
                                          ISMMCLT_Id = b.ISMMCLT_Id
                                      }).Distinct().OrderBy(a => a.ISMMCLT_ClientName).ToArray();


                data.projectlist = (from a in _INVContext.clientTable
                                    from b in _INVContext.MastersProject_DMO
                                    from c in _INVContext.ISM_Master_Client_ProjectDMO
                                    where (a.ISMMCLT_Id == c.ISMMCLT_Id && b.ISMMPR_Id == c.ISMMPR_Id && a.MI_Id == b.MI_Id && b.ISMMPR_ActiveFlg == true
                                    && c.ISMMCLT_Id == editlist.FirstOrDefault().ISMMCLT_Id)
                                    select new ClientInvoiceDTO
                                    {
                                        ISMMPR_ProjectName = b.ISMMPR_ProjectName,
                                        ISMMPR_Id = c.ISMMPR_Id,
                                    }).Distinct().ToArray();

                data.editlistdetails = _INVContext.ISM_Invoice_DetailsDMO.Where(w => w.ISMINC_Id == data.ISMINC_Id).ToArray();
                data.editlisttax = _INVContext.ISM_Invoice_TaxDMO.Where(w => w.ISMINC_Id == data.ISMINC_Id).ToArray();


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public ClientInvoiceDTO clientDecative(ClientInvoiceDTO data)
        {
            try
            {
                var u = _INVContext.ISM_InvoiceDMO.Where(t => t.ISMINC_Id == data.ISMINC_Id).SingleOrDefault();
                if (u.ISMINC_ActiveFlag == true)
                {
                    u.ISMINC_ActiveFlag = false;
                }
                else if (u.ISMINC_ActiveFlag == false)
                {
                    u.ISMINC_ActiveFlag = true;
                }

                u.ISMINC_UpdatedBy = data.UserId;
                u.ISMINC_UpdatedDate = DateTime.Now;
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
        public ClientInvoiceDTO viewdetails(ClientInvoiceDTO data)
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
        public ClientInvoiceDTO savepaymentdetailsrecord(ClientInvoiceDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Payment Configuration
        public ClientInvoiceDTO loaddataconfig(ClientInvoiceDTO data)
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
        public ClientInvoiceDTO savedataconfig(ClientInvoiceDTO data)
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
        public ClientInvoiceDTO loaddatareport(ClientInvoiceDTO data)
        {
            try
            {
                data.clientlist = (from a in _INVContext.clientTable
                                   from b in _INVContext.ISM_Master_Client_ProjectDMO
                                   where (a.ISMMCLT_Id == b.ISMMCLT_Id && a.MI_Id == data.MI_Id && a.ISMMCLT_ActiveFlag == true)
                                   select new ClientInvoiceDTO
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
        public ClientInvoiceDTO getreport(ClientInvoiceDTO data)
        {
            try
            {
                //ISMMPR_Id
                var update = _db.INV_M_PurchaseOrderDMO.Where(R => R.INVMPO_Id == data.ISMMPR_Id).ToList();
                if (update.Count > 0)
                {
                    foreach (var d in update)
                    {
                        d.INVMPO_POTemplate = data.msg;
                        _db.Update(d);
                    }
                    int order = _db.SaveChanges();
                    if (order > 0)
                    {
                        if (data.ISMINC_WorkOrder == "SAVE")
                        {
                            return data;
                        }
                    }

                }
                Dictionary<string, string> val = new Dictionary<string, string>();//CleintInvoiceMail
                var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "Potemplate" && e.ISES_MailActiveFlag == true).ToList();
                // var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "CleintInvoiceMail" && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return data;
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = data.msg;
                string Mailmsg = data.msg;
                Mailcontent = "<br />";
                Mailmsg = "<br />";
                Mailcontent = Mailcontent + data.msg;
                Mailmsg = Mailmsg + data.msg;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    var message = new SendGridMessage();
                    if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                    {
                        string[] ccmail = template[0].ISES_MailBCCId.Split(',');
                        foreach (var c in ccmail)
                        {
                            if (c != "")
                            {
                                message.AddBcc(c);

                            }
                        }

                    }

                    //if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    //{
                    //    string[] ccmail = template[0].ISES_MailCCId.Split(',');
                    //    foreach (var c in ccmail)
                    //    {
                    //        if (c != "")
                    //        {
                    //            message.AddCc(c);

                    //        }
                    //    }

                    //}
                    //FHEAD
                    if (data.FHEAD != null && data.FHEAD != "")
                    {
                        string[] ccmail = data.FHEAD.Split(',');
                        foreach (var c in ccmail)
                        {
                            if (c != "")
                            {
                                message.AddCc(c);

                            }
                        }

                    }
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    if (data.esubject != null && data.esubject != "")
                    {
                        message.Subject = data.esubject;
                    }
                    else
                    {
                        message.Subject = Subject;
                    }
                    // string emailid = "sanjeev@globalqtytrainig.com";
                    string emailid = data.ClientMail;
                    message.AddTo(emailid);


                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "Potemplate" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = emailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {


                            }
                        }
                        catch (Exception ex)
                        {

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
        public ClientInvoiceDTO paymentnotification(ClientInvoiceDTO data)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "CleintInvoiceMail" && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return data;
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == data.MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == data.MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailcontent = data.msg;
                string Mailmsg = data.msg;
                Mailcontent = "<br />";
                Mailmsg = "<br />";
                Mailcontent = Mailcontent + data.msg;
                Mailmsg = Mailmsg + data.msg;
                if (data.FHEAD != null && data.FHEAD != "")
                {
                    Mailcontent = Mailcontent + "<br />" + data.FHEAD + "<br />";
                    Mailmsg = Mailmsg + "<br />" + data.FHEAD + "<br />";
                }
                //if (data.Footer != null && data.Footer != "")
                //{
                //    Mailcontent = Mailcontent + "<br />" + data.Footer;
                //    Mailmsg = Mailmsg + "<br />" + data.Footer;
                //}
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.MI_Id)).ToList();
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    var message = new SendGridMessage();
                    if (template[0].ISES_MailBCCId != null && template[0].ISES_MailBCCId != "")
                    {
                        string[] ccmail = template[0].ISES_MailBCCId.Split(',');
                        foreach (var c in ccmail)
                        {
                            if (c != "")
                            {
                                message.AddBcc(c);

                            }
                        }

                    }

                    if (template[0].ISES_MailCCId != null && template[0].ISES_MailCCId != "")
                    {
                        string[] ccmail = template[0].ISES_MailCCId.Split(',');
                        foreach (var c in ccmail)
                        {
                            if (c != "")
                            {
                                message.AddCc(c);

                            }
                        }

                    }
                    //ISES_MailBCCId

                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    if (data.esubject != null && data.esubject != "")
                    {
                        message.Subject = data.esubject;
                    }
                    else
                    {
                        message.Subject = Subject;
                    }

                    string emailid = data.ClientMail;
                    //  string emailid = "sanjeev@globalqtytrainig.com";
                    message.AddTo(emailid);


                    //StringBuilder sb = new StringBuilder(data.ISMMCLT_ClientName);
                    //StringReader sr = new StringReader(sb.ToString());

                    //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    //HtmlWorker htmlparser = new HtmlWorker(pdfDoc);
                    //using (MemoryStream memoryStream = new MemoryStream())
                    //{
                    //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    //    pdfDoc.Open();
                    //    htmlparser.Parse(sr);
                    //    pdfDoc.Close();
                    //    byte[] bytess = memoryStream.ToArray();
                    //    memoryStream.Close();
                    //    var file = Convert.ToBase64String(bytess);
                    //    string emp;
                    //    emp = Convert.ToString(sr);
                    //    string c = "";
                    //    string v = emp.Replace("System.IO.StringReader", "Invoice.Pdf");
                    //    message.AddAttachment(v, file);


                    //}
                    //
                    if (data.taxdto != null && data.taxdto.Length > 0)
                    {
                        foreach (var item in data.taxdto)
                        {
                            if (item.cfilepath != null && item.cfilepath != "")
                            {
                                var webClient = new WebClient();
                                byte[] imageBytes = webClient.DownloadData(item.cfilepath);
                                string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                message.AddAttachment(item.cfilepath, fileContentsAsBase64, null, null, null);
                            }
                        }
                    }


                    //
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.ISES_Template_Name == "CleintInvoiceMail" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();
                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();
                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId", SqlDbType.NVarChar)
                        {
                            Value = emailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {


                            }
                        }
                        catch (Exception ex)
                        {

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
    }
}
