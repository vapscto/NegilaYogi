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
    public class INV_PurchaseIndentImpl : Interface.INV_PurchaseIndentInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PurchaseIndentImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_PurchaseIndentImpl(InventoryContext InvContext, ILogger<INV_PurchaseIndentImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_PurchaseIndentDTO getloaddata(INV_PurchaseIndentDTO data)
        {
            try
            {
                var prflg = _INVContext.INV_ConfigurationDMO.Where(c => c.MI_Id == data.MI_Id && c.INVC_ProcessApplFlg == true).ToList();
                data.prflag = prflg.FirstOrDefault().INVC_PRApplicableFlg;


                //  data.get_prNo = _INVContext.INV_M_PurchaseRequisitionDMO.Where(r => r.MI_Id == data.MI_Id && r.INVMPR_ActiveFlg == true).Distinct().ToArray();

                data.get_prNo = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                 from b in _INVContext.MasterEmployee

                                 where (a.HRME_Id == b.HRME_Id && a.MI_Id == b.MI_Id && a.INVMPR_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMPR_PICreatedFlg == false)
                                 select new INV_PurchaseIndentDTO
                                 {
                                     INVMPR_Id = a.INVMPR_Id,
                                     HRME_Id = a.HRME_Id,
                                     employeename = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "" ? "" : " " + b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" || b.HRME_EmployeeMiddleName == "0" ? "" : " " + b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" || b.HRME_EmployeeLastName == "0" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                     INVMPR_PRNo = a.INVMPR_PRNo,
                                     INVMPR_PRDate = a.INVMPR_PRDate,
                                     INVMPR_Remarks = a.INVMPR_Remarks,
                                     INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                                     INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                                 }).Distinct().OrderByDescending(i => i.INVMPR_PRDate).ToArray();

                data.get_item = _INVContext.INV_Master_ItemDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMI_ActiveFlg == true).OrderBy(m => m.INVMI_ItemName).ToArray();

                //data.get_item = (from a in _INVContext.INV_Master_ItemDMO
                //                 from b in _INVContext.INV_StockDMO
                //                 from c in _INVContext.INV_Master_UOMDMO
                //                 where (a.INVMI_Id == b.INVMI_Id && c.INVMUOM_Id == a.INVMUOM_Id && a.MI_Id == b.MI_Id && b.INVSTO_AvaiableStock != 0 && a.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id)
                //                 select new INV_PurchaseIndentDTO
                //                 {
                //                     INVMI_Id = a.INVMI_Id,
                //                     INVMUOM_Id = a.INVMUOM_Id,
                //                     INVMI_ItemName = a.INVMI_ItemName,
                //                     INVMI_ItemCode = a.INVMI_ItemCode,
                //                 }).Distinct().OrderBy(i => i.INVMI_ItemName).ToArray();

               // data.get_purchaseindent = _INVContext.INV_M_PurchaseIndentDMO.Where(i => i.MI_Id == data.MI_Id).OrderByDescending(a=>a.INVMPI_Id).ToArray();
                data.get_purchaseindent = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                          from b in _INVContext.INV_T_PurchaseIndentDMO
                                        //   from c in _INVContext.INV_Master_ItemDMO
                                         //  from d in _INVContext.INV_Master_UOMDMO
                                           where
                                         a.INVMPI_Id == b.INVMPI_Id &&
                                          //  a.INVMPI_ActiveFlg == true 
                                            //&& 
                                           b.INVTPI_ActiveFlg == true &&
                                            a.MI_Id == data.MI_Id &&
                                            a.MI_Id == b.MI_Id
                                           //&& 
                                           //   c.INVMI_Id == b.INVMI_Id && 
                                           //   d.INVMUOM_Id == b.INVMUOM_Id

                                           select new INV_PurchaseIndentDTO
                                           {
                                               INVMPI_Id = a.INVMPI_Id,
                                               INVMPI_PINo = a.INVMPI_PINo,
                                               INVMPI_PIDate = a.INVMPI_PIDate,
                                               INVMPI_ReferenceNo = a.INVMPI_ReferenceNo,
                                            //   INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,
                                               INVMPI_ApproxTotAmount = a.INVMPI_ApproxTotAmount,
                                            //   INVTPI_PIQty = b.INVTPI_PIQty,
                                               INVMPI_ActiveFlg = a.INVMPI_ActiveFlg,
                                             //  INVMI_ItemName = c.INVMI_ItemName,
                                              // INVMUOM_UOMName = d.INVMUOM_UOMName,
                                              // INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                                             //  INVTPI_Remarks = b.INVTPI_Remarks
                                           }).Distinct().OrderByDescending(a => a.INVMPI_Id).ToArray();



                
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_PurchaseIndentDTO> getpidetails(INV_PurchaseIndentDTO data)
        {
            try
            {
                data.get_pimodel = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                    from b in _INVContext.INV_T_PurchaseIndentDMO
                                    from c in _INVContext.INV_Master_ItemDMO
                                    from d in _INVContext.INV_Master_UOMDMO
                                    where (a.INVMPI_Id == b.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                             && a.MI_Id == data.MI_Id && a.INVMPI_Id == data.INVMPI_Id)
                                    select new INV_PurchaseIndentDTO
                                    {
                                        INVMPI_Id = a.INVMPI_Id,
                                        INVTPI_Id = b.INVTPI_Id,
                                        INVMPI_PINo = a.INVMPI_PINo,
                                        INVMI_Id = b.INVMI_Id,
                                        INVMUOM_Id = b.INVMUOM_Id,
                                        INVMPR_Id = b.INVMPR_Id,
                                        INVMI_ItemName = c.INVMI_ItemName,
                                        INVMUOM_UOMName = d.INVMUOM_UOMName,
                                        INVTPI_PRQty = b.INVTPI_PRQty,
                                        INVMPI_PIDate = a.INVMPI_PIDate,
                                        INVMPI_ReferenceNo = a.INVMPI_ReferenceNo,
                                        INVTPI_PIQty = b.INVTPI_PIQty,
                                        INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                                        INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,
                                        INVMPI_Remarks = a.INVMPI_Remarks,
                                        INVMPI_ApproxTotAmount = a.INVMPI_ApproxTotAmount
                                    }).Distinct().OrderBy(i => i.INVMPI_Id).ToArray();


                if (data.checkArray.Length > 0)
                {
                    string ckdids = "0";
                    if (data.checkArray != null)
                    {
                        foreach (var c in data.checkArray)
                        {
                            ckdids = ckdids + "," + c.INVMPI_Id;
                        }
                    }
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_PI_ReceiptPrint";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ckd_Ids",
                        SqlDbType.VarChar)
                        {
                            Value = ckdids
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
                            data.get_PIReceipt = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent Item details:" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseIndentDTO getprDetail(INV_PurchaseIndentDTO data)
        {
            try
            {
                data.get_indentDetail = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                         from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                         from c in _INVContext.INV_Master_ItemDMO
                                         from d in _INVContext.INV_Master_UOMDMO
                                         from e in _INVContext.MasterEmployee
                                         where (a.INVMPR_Id == b.INVMPR_Id && b.INVMI_Id == c.INVMI_Id && a.HRME_Id == e.HRME_Id && b.INVMUOM_Id == d.INVMUOM_Id && a.INVMPR_ActiveFlg == true && c.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id && b.INVMPR_Id == data.INVMPR_Id)
                                         select new INV_PurchaseRequisitionDTO
                                         {
                                             INVMPR_Id = a.INVMPR_Id,
                                             INVTPR_Id = b.INVTPR_Id,
                                             INVMI_Id = b.INVMI_Id,
                                             INVMUOM_Id = b.INVMUOM_Id,
                                             HRME_Id = a.HRME_Id,
                                             INVMPR_PRNo = a.INVMPR_PRNo,
                                             INVMI_ItemName = c.INVMI_ItemName,
                                             INVMI_ItemCode = c.INVMI_ItemCode,
                                             INVMUOM_UOMName = d.INVMUOM_UOMName,
                                             employeename = ((e.HRME_EmployeeFirstName == null || e.HRME_EmployeeFirstName == "" ? "" : " " + e.HRME_EmployeeFirstName) + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" || e.HRME_EmployeeMiddleName == "0" ? "" : " " + e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" || e.HRME_EmployeeLastName == "0" ? "" : " " + e.HRME_EmployeeLastName)).Trim(),
                                             INVTPR_PRQty = b.INVTPR_PRQty,
                                             INVTPR_PRUnitRate = b.INVTPR_PRUnitRate,
                                             INVTPR_ApproxAmount = b.INVTPR_ApproxAmount,
                                             INVTPR_ApprovedQty = b.INVTPR_ApprovedQty,
                                             INVMPR_PRDate = a.INVMPR_PRDate,
                                             INVMPR_Remarks = a.INVMPR_Remarks,
                                             INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                                             INVTPR_Remarks = b.INVTPR_Remarks,
                                             INVTPR_ActiveFlg = b.INVTPR_ActiveFlg,
                                             INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                                             INVMPR_ActiveFlg = a.INVMPR_ActiveFlg
                                         }).Distinct().OrderBy(i => i.INVTPR_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent PR Change:" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseIndentDTO get_details(INV_PurchaseIndentDTO data)
        {
            try
            {
                data.get_PIdetails = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                      from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                      from c in _INVContext.INV_Master_ItemDMO
                                      from d in _INVContext.INV_Master_UOMDMO
                                      where (a.INVMPR_Id == b.INVMPR_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && a.INVMPR_ActiveFlg == true && c.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id && b.INVMPR_Id == data.INVMPR_Id)
                                      select new INV_PurchaseRequisitionDTO
                                      {
                                          INVMPR_Id = a.INVMPR_Id,
                                          INVTPR_Id = b.INVTPR_Id,
                                          INVMI_Id = b.INVMI_Id,
                                          INVMUOM_Id = b.INVMUOM_Id,
                                          HRME_Id = a.HRME_Id,
                                          INVMI_ItemName = c.INVMI_ItemName,
                                          INVMI_ItemCode = c.INVMI_ItemCode,
                                          INVMUOM_UOMName = d.INVMUOM_UOMName,
                                          INVTPR_PRQty = b.INVTPR_PRQty,
                                          INVTPR_PRUnitRate = b.INVTPR_PRUnitRate,
                                          INVTPR_ApproxAmount = b.INVTPR_ApproxAmount,
                                          INVTPR_ApprovedQty = b.INVTPR_ApprovedQty,
                                          INVMPR_PRDate = a.INVMPR_PRDate,
                                          INVMPR_Remarks = a.INVMPR_Remarks,
                                          INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                                          INVTPR_Remarks = b.INVTPR_Remarks,
                                          INVTPR_ActiveFlg = b.INVTPR_ActiveFlg,
                                          INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                                          INVMPR_ActiveFlg = a.INVMPR_ActiveFlg
                                      }).Distinct().OrderBy(i => i.INVTPR_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent PR Change:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseIndentDTO savedetails(INV_PurchaseIndentDTO data)
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

                if (data.INVMPI_Id != 0)
                {
                    var result = _INVContext.INV_M_PurchaseIndentDMO.Single(t => t.MI_Id == data.MI_Id && t.INVMPI_Id == data.INVMPI_Id);
                    result.MI_Id = data.MI_Id;
                    result.INVMPI_PIDate = data.INVMPI_PIDate;
                    result.INVMPI_Remarks = data.INVMPI_Remarks;
                    result.INVMPI_ReferenceNo = data.INVMPI_ReferenceNo;
                    result.INVMPI_ApproxTotAmount = data.INVMPI_ApproxTotAmount;
                    result.INVMPI_POCreatedFlg = false;
                    result.INVMPI_ActiveFlg = true;

                    result.INVMPI_UpdatedBy = data.UserId;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(result);

                    foreach (var r in data.arrayPI)
                    {
                        var res1 = _INVContext.INV_T_PurchaseIndentDMO.Where(a => a.INVTPI_Id == r.INVTPI_Id).ToList();
                        if (res1.Count > 0)
                        {
                            var res11 = _INVContext.INV_T_PurchaseIndentDMO.Single(a => a.INVTPI_Id == r.INVTPI_Id);
                            res11.INVTPI_PRQty = r.INVTPI_PRQty;
                            res11.INVTPI_PIQty = r.INVTPI_PIQty;
                            res11.INVTPI_PIUnitRate = r.INVTPI_PIUnitRate;
                            res11.INVTPI_ApproxAmount = r.INVTPI_ApproxAmount;
                            res11.INVTPI_Remarks = r.INVTPI_Remarks;
                            res11.INVTPI_ActiveFlg = true;
                            res11.INVTPI_UpdatedBy = data.UserId;
                            res11.UpdatedDate = indianTime;
                            _INVContext.Update(res11);
                        }
                        else
                        {
                            INV_T_PurchaseIndentDMO piu = new INV_T_PurchaseIndentDMO();
                            piu.INVTPI_PRQty = r.INVTPI_PRQty;
                            piu.INVTPI_PIQty = r.INVTPI_PIQty;
                            piu.INVTPI_PIUnitRate = r.INVTPI_PIUnitRate;
                            piu.INVTPI_ApproxAmount = r.INVTPI_ApproxAmount;
                            piu.INVTPI_Remarks = r.INVTPI_Remarks;
                            piu.INVTPI_ActiveFlg = true;
                            piu.INVTPI_UpdatedBy = data.UserId;
                            piu.UpdatedDate = indianTime;
                            _INVContext.Add(piu);
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


                    INV_M_PurchaseIndentDMO pi = new INV_M_PurchaseIndentDMO();
                    pi.MI_Id = data.MI_Id;

                    pi.INVMPI_PINo = data.trans_id;
                    pi.INVMPI_PIDate = data.INVMPI_PIDate;
                    pi.INVMPI_Remarks = data.INVMPI_Remarks;
                    pi.INVMPI_ReferenceNo = data.INVMPI_ReferenceNo;
                    pi.INVMPI_ApproxTotAmount = data.INVMPI_ApproxTotAmount;
                    pi.INVMPI_POCreatedFlg = false;
                    pi.INVMPI_ActiveFlg = true;
                    pi.INVMPI_CreatedBy = data.UserId;
                    pi.INVMPI_UpdatedBy = data.UserId;
                    pi.CreatedDate = indianTime;
                    pi.UpdatedDate = indianTime;
                    _INVContext.Add(pi);

                    foreach (var r in data.arrayPI)
                    {
                        INV_T_PurchaseIndentDMO tpi = new INV_T_PurchaseIndentDMO();
                        tpi.MI_Id = data.MI_Id;
                        tpi.INVMPI_Id = pi.INVMPI_Id;
                        tpi.INVMI_Id = r.INVMI_Id;
                        tpi.INVMUOM_Id = r.INVMUOM_Id;
                        tpi.INVMPR_Id = r.INVMPR_Id;
                        tpi.INVTPI_PRQty = r.INVTPI_PRQty;
                        tpi.INVTPI_PIUnitRate = r.INVTPI_PIUnitRate;
                        tpi.INVTPI_ApproxAmount = r.INVTPI_ApproxAmount;
                        tpi.INVTPI_PIQty = r.INVTPI_PIQty;
                        tpi.INVTPI_Remarks = r.INVTPI_Remarks;
                        tpi.INVTPI_ActiveFlg = true;

                        tpi.INVTPI_CreatedBy = data.UserId;
                        tpi.INVTPI_UpdatedBy = data.UserId;

                        tpi.CreatedDate = indianTime;
                        tpi.UpdatedDate = indianTime;
                        _INVContext.Add(tpi);

                        if (data.prflag == true)
                        {

                            var resUpdate = _INVContext.INV_M_PurchaseRequisitionDMO.Single(a => a.MI_Id == data.MI_Id && a.INVMPR_Id == r.INVMPR_Id);
                            if (resUpdate.INVMPR_PICreatedFlg == false)
                            {
                                resUpdate.INVMPR_PICreatedFlg = true;
                            }
                            _INVContext.Update(resUpdate);

                            var resUpdateqty = _INVContext.INV_T_PurchaseRequisitionDMO.Single(a => a.INVMPR_Id == r.INVMPR_Id && a.INVMI_Id == r.INVMI_Id && a.INVTPR_PRQty == r.INVTPI_PRQty);
                            resUpdateqty.INVTPR_ApprovedQty = r.INVTPI_PIQty;
                            _INVContext.Update(resUpdateqty);
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

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Purchase Indent savedata :" + ex.Message);
            }

            return data;
        }

        public INV_PurchaseIndentDTO edit(INV_PurchaseIndentDTO data)
        {
            try
            {
                data.get_editPI = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                   from b in _INVContext.INV_T_PurchaseIndentDMO
                                   from c in _INVContext.INV_Master_ItemDMO
                                   from d in _INVContext.INV_Master_UOMDMO
                                   where (a.INVMPI_Id == b.INVMPI_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                            && a.INVMPI_ActiveFlg == true && a.MI_Id == data.MI_Id && b.INVTPI_Id == data.INVTPI_Id)
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
                _logInv.LogInformation("Purchase Indent edit:" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseIndentDTO genrateReceipt(INV_PurchaseIndentDTO data)
        {
            try
            {
                List<long> piids = new List<long>();
                if (data.piArray != null)
                {
                    foreach (var item in data.piArray)
                    {
                        piids.Add(item.INVMPI_Id);
                    }
                }


                data.get_purchaseindent = (from a in _INVContext.INV_M_PurchaseIndentDMO
                                           from b in _INVContext.INV_T_PurchaseIndentDMO
                                           from c in _INVContext.INV_Master_ItemDMO
                                           from d in _INVContext.INV_Master_UOMDMO
                                           where
                                             a.INVMPI_Id == b.INVMPI_Id &&
                                             piids.Contains(a.INVMPI_Id)

                                            // a.INVMPI_ActiveFlg == true
                                            &&
                                               b.INVTPI_ActiveFlg == true &&
                                            a.MI_Id == data.MI_Id
                                           &&
                                              c.INVMI_Id == b.INVMI_Id &&
                                              d.INVMUOM_Id == b.INVMUOM_Id

                                           select new INV_PurchaseIndentDTO
                                           {
                                               INVMPI_Id = a.INVMPI_Id,
                                               INVMPI_PINo = a.INVMPI_PINo,
                                               INVMPI_PIDate = a.INVMPI_PIDate,
                                               INVMPI_ReferenceNo = a.INVMPI_ReferenceNo,
                                               INVTPI_ApproxAmount = b.INVTPI_ApproxAmount,
                                               INVMPI_ApproxTotAmount = a.INVMPI_ApproxTotAmount,
                                               INVTPI_PIQty = b.INVTPI_PIQty,
                                               INVMPI_ActiveFlg = a.INVMPI_ActiveFlg,
                                               INVMI_ItemName = c.INVMI_ItemName,
                                               INVMUOM_UOMName = d.INVMUOM_UOMName,
                                               INVTPI_PIUnitRate = b.INVTPI_PIUnitRate,
                                               INVTPI_Remarks = b.INVTPI_Remarks
                                           }).Distinct().OrderByDescending(a => a.INVMPI_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Indent edit:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseIndentDTO deactiveM(INV_PurchaseIndentDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_PurchaseIndentDMO.Single(t => t.INVMPI_Id == data.INVMPI_Id);

                if (result.INVMPI_ActiveFlg == true)
                {
                    result.INVMPI_ActiveFlg = false;
                }
                else if (result.INVMPI_ActiveFlg == false)
                {
                    result.INVMPI_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                var resultt = _INVContext.INV_T_PurchaseIndentDMO.Where(t => t.INVMPI_Id == data.INVMPI_Id).ToList();
                if (resultt.Count > 0)
                {
                    foreach (var rt in resultt)
                    {
                        // var rest = _INVContext.INV_T_PurchaseIndentDMO.Single(t => t.INVMPI_Id == rt.INVMPI_Id);
                        if (result.INVMPI_ActiveFlg == true)
                        {
                            rt.INVTPI_ActiveFlg = true;
                        }
                        if (result.INVMPI_ActiveFlg == false)
                        {
                            rt.INVTPI_ActiveFlg = false;
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

        public INV_PurchaseIndentDTO deactive(INV_PurchaseIndentDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_PurchaseIndentDMO.Single(t => t.INVTPI_Id == data.INVTPI_Id);

                if (result.INVTPI_ActiveFlg == true)
                {
                    result.INVTPI_ActiveFlg = false;
                }
                else if (result.INVTPI_ActiveFlg == false)
                {
                    result.INVTPI_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                int countactiveT = 0;
                int countactiveF = 0;
                var resultt = _INVContext.INV_T_PurchaseIndentDMO.Where(t => t.INVMPI_Id == data.INVMPI_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (rt.INVTPI_ActiveFlg == false)
                    {
                        countactiveF += 1;
                    }
                    else if (rt.INVTPI_ActiveFlg == true)
                    {
                        countactiveT += 1;
                    }
                }
                var resultmflg = _INVContext.INV_M_PurchaseIndentDMO.Single(t => t.INVMPI_Id == data.INVMPI_Id);
                if (countactiveF > 0 && countactiveT == 0)
                {
                    resultmflg.INVMPI_ActiveFlg = false;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }
                else if (countactiveT > 0 && countactiveF == 0)
                {
                    resultmflg.INVMPI_ActiveFlg = true;
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
