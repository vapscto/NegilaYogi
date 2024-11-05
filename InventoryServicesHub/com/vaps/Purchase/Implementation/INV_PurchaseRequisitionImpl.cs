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
    public class INV_PurchaseRequisitionImpl : Interface.INV_PurchaseRequisitionInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PurchaseRequisitionImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_PurchaseRequisitionImpl(InventoryContext InvContext, ILogger<INV_PurchaseRequisitionImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_PurchaseRequisitionDTO getloaddata(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                data.get_pidata = _INVContext.INV_T_PurchaseIndentDMO.Where(p => p.MI_Id == data.MI_Id && p.INVTPI_ActiveFlg == true).ToArray();

                data.get_item = (from a in _INVContext.INV_Master_ItemDMO
                                 from c in _INVContext.INV_Master_UOMDMO
                                 where (c.INVMUOM_Id == a.INVMUOM_Id && a.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id)
                                 select new INV_PurchaseRequisitionDTO
                                 {
                                     INVMI_Id = a.INVMI_Id,
                                     INVMUOM_Id = a.INVMUOM_Id,
                                     INVMI_ItemName = a.INVMI_ItemName,
                                     INVMI_ItemCode = a.INVMI_ItemCode,
                                 }).Distinct().OrderBy(i => i.INVMI_ItemName).ToArray();



                data.get_purchaserequisition = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                                from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                                where (a.INVMPR_Id == b.INVMPR_Id && a.MI_Id == data.MI_Id && a.INVMPR_CreatedBy == data.UserId)
                                                select new INV_PurchaseRequisitionDTO
                                                {
                                                    INVMPR_Id = a.INVMPR_Id,
                                                    INVMPR_PRNo = a.INVMPR_PRNo,
                                                    INVMPR_PRDate = a.INVMPR_PRDate,
                                                    INVMPR_Remarks = a.INVMPR_Remarks,
                                                    INVMPR_ApproxTotAmount = a.INVMPR_ApproxTotAmount,
                                                    INVMPR_PICreatedFlg = a.INVMPR_PICreatedFlg,
                                                    INVMPR_ActiveFlg = a.INVMPR_ActiveFlg,
                                                    //  INVMI_Id=b.INVMI_Id
                                                }).Distinct().OrderByDescending(i => i.INVMPR_Id).ToArray();


            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition load Page:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseRequisitionDTO get_prdetails(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                data.get_prDetail = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                                     from b in _INVContext.INV_T_PurchaseRequisitionDMO
                                     from c in _INVContext.INV_Master_ItemDMO
                                     from d in _INVContext.INV_Master_UOMDMO
                                     where (a.INVMPR_Id == b.INVMPR_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id && a.MI_Id == data.MI_Id && a.INVMPR_Id == data.INVMPR_Id)
                                     select new INV_PurchaseRequisitionDTO
                                     {
                                         INVTPR_Id = b.INVTPR_Id,
                                         INVMPR_Id = a.INVMPR_Id,
                                         INVMI_Id = b.INVMI_Id,
                                         INVMUOM_Id = b.INVMUOM_Id,
                                         INVMPR_PRNo = a.INVMPR_PRNo,
                                         INVMI_ItemName = c.INVMI_ItemName,
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

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "INV_Purchase_ExipireDate";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@INVMPR_Id", SqlDbType.BigInt) { Value = data.INVMPR_Id });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.get_pidata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition Item details:" + ex.Message);
            }
            return data;
        }
        public INV_PurchaseRequisitionDTO getitemDetail(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                data.get_itemDetail = (from a in _INVContext.INV_Master_ItemDMO
                                       from c in _INVContext.INV_Master_UOMDMO
                                       where (c.INVMUOM_Id == a.INVMUOM_Id && a.INVMI_ActiveFlg == true && a.MI_Id == data.MI_Id && a.INVMI_Id == data.INVMI_Id)
                                       select new INV_PurchaseRequisitionDTO
                                       {
                                           INVMI_Id = a.INVMI_Id,
                                           INVMI_ItemName = a.INVMI_ItemName,
                                           INVMI_ItemCode = a.INVMI_ItemCode,
                                           INVMUOM_Id = a.INVMUOM_Id,
                                           INVMUOM_UOMName = c.INVMUOM_UOMName,
                                       }).Distinct().ToArray();




                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "INV_BalanceStock";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id", SqlDbType.BigInt) { Value = data.INVMI_Id });

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
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.balancestock = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Purchase Requisition Item details:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseRequisitionDTO savedetails(INV_PurchaseRequisitionDTO data)
        {

            try
            {

                var hrmeid = _db.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).ToList();

                if (hrmeid.Count > 0)
                {
                    data.HRME_Id = hrmeid.FirstOrDefault().Emp_Code;
                }
                else
                {
                    data.HRME_Id = 0;
                }

                var amsts_id = 0;
                if (data.AMST_Id > 0)
                {
                    amsts_id = Convert.ToInt32(data.AMST_Id);
                }
                else
                {
                    amsts_id = 0;
                }

                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }

                if (data.INVMPR_Id != 0)
                {
                    var result = _INVContext.INV_M_PurchaseRequisitionDMO.Single(t => t.MI_Id == data.MI_Id && t.INVMPR_Id == data.INVMPR_Id);
                    result.MI_Id = data.MI_Id;
                    result.INVMPR_PRDate = data.INVMPR_PRDate;
                    result.INVMPR_Remarks = data.INVMPR_Remarks;
                    result.INVMPR_ApproxTotAmount = data.INVMPR_ApproxTotAmount;
                    result.INVMPR_PICreatedFlg = false;
                    result.INVMPR_ActiveFlg = true;
                    result.INVMPR_UpdatedBy = data.UserId;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(result);

                    foreach (var r in data.arrayPR)
                    {
                        var res1 = _INVContext.INV_T_PurchaseRequisitionDMO.Where(a => a.INVTPR_Id == r.INVTPR_Id).ToList();
                        if (res1.Count > 0)
                        {
                            var res11 = _INVContext.INV_T_PurchaseRequisitionDMO.Single(a => a.INVTPR_Id == r.INVTPR_Id);
                            res11.INVMPR_Id = result.INVMPR_Id;
                            res11.INVTPR_PRQty = r.INVTPR_PRQty;
                            res11.INVTPR_PRUnitRate = r.INVTPR_PRUnitRate;
                            res11.INVTPR_ApproxAmount = r.INVTPR_ApproxAmount;
                            res11.INVTPR_ApprovedQty = r.INVTPR_ApprovedQty;
                            res11.INVTPR_Remarks = r.INVTPR_Remarks;
                            res11.INVTPR_ActiveFlg = true;
                            res11.INVTPR_UpdatedBy = data.UserId;
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
                else
                {
                    INV_M_PurchaseRequisitionDMO pr = new INV_M_PurchaseRequisitionDMO();
                    pr.MI_Id = data.MI_Id;
                    pr.HRME_Id = data.HRME_Id;
                    pr.INVMPR_PRNo = data.trans_id;
                    pr.INVMPR_PRDate = data.INVMPR_PRDate;
                    pr.INVMPR_Remarks = data.INVMPR_Remarks;
                    pr.INVMPR_ApproxTotAmount = data.INVMPR_ApproxTotAmount;
                    pr.INVMPR_PICreatedFlg = false;
                    pr.INVMPR_ActiveFlg = true;
                    pr.INVMPR_CreatedBy = data.UserId;
                    pr.INVMPR_UpdatedBy = data.UserId;
                    pr.CreatedDate = DateTime.Now;
                    pr.UpdatedDate = DateTime.Now;
                    pr.INVMPR_Flag = data.Roleflag;
                    pr.AMST_Id = data.AMST_Id;
                    _INVContext.Add(pr);

                    foreach (var r in data.arrayPR)
                    {
                        INV_T_PurchaseRequisitionDMO tpr = new INV_T_PurchaseRequisitionDMO();
                        tpr.INVMPR_Id = pr.INVMPR_Id;
                        tpr.INVMI_Id = r.INVMI_Id;
                        tpr.INVMUOM_Id = r.INVMUOM_Id;
                        tpr.INVTPR_PRQty = r.INVTPR_PRQty;
                        tpr.INVTPR_PRUnitRate = r.INVTPR_PRUnitRate;
                        tpr.INVTPR_ApproxAmount = r.INVTPR_ApproxAmount;
                        tpr.INVTPR_ApprovedQty = r.INVTPR_ApprovedQty;
                        tpr.INVTPR_Remarks = r.INVTPR_Remarks;
                        tpr.INVTPR_ActiveFlg = true;
                        tpr.INVTPR_CreatedBy = data.UserId;
                        tpr.INVTPR_UpdatedBy = data.UserId;
                        tpr.CreatedDate = DateTime.Now;
                        tpr.UpdatedDate = DateTime.Now;
                        _INVContext.Add(tpr);
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
                _logInv.LogInformation("Purchase Requisition savedata :" + ex.Message);
            }

            return data;
        }
        public INV_PurchaseRequisitionDTO edit(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                data.editPR = (from a in _INVContext.INV_M_PurchaseRequisitionDMO
                               from b in _INVContext.INV_T_PurchaseRequisitionDMO
                               from c in _INVContext.INV_Master_ItemDMO
                               from d in _INVContext.INV_Master_UOMDMO

                               where (a.INVMPR_Id == b.INVMPR_Id && b.INVMI_Id == c.INVMI_Id && b.INVMUOM_Id == d.INVMUOM_Id
                                           && a.MI_Id == data.MI_Id && b.INVMPR_Id == data.INVMPR_Id)
                               select new INV_PurchaseRequisitionDTO
                               {
                                   INVTPR_Id = b.INVTPR_Id,
                                   INVMPR_Id = a.INVMPR_Id,
                                   INVMI_Id = b.INVMI_Id,
                                   INVMUOM_Id = b.INVMUOM_Id,
                                   INVMPR_PRNo = a.INVMPR_PRNo,
                                   INVMI_ItemName = c.INVMI_ItemName,
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
                _logInv.LogInformation("Purchase Requisition edit:" + ex.Message);
            }
            return data;
        }

        public INV_PurchaseRequisitionDTO deactiveM(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                var check = _INVContext.INV_T_PurchaseIndentDMO.Where(a => a.INVMPR_Id == data.INVMPR_Id && a.MI_Id == data.MI_Id).ToList();
                if (check.Count == 0)
                {
                    var result = _INVContext.INV_M_PurchaseRequisitionDMO.Single(t => t.INVMPR_Id == data.INVMPR_Id && t.MI_Id == data.MI_Id);

                    if (result.INVMPR_ActiveFlg == true)
                    {
                        result.INVMPR_ActiveFlg = false;
                    }
                    else if (result.INVMPR_ActiveFlg == false)
                    {
                        result.INVMPR_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(result);

                    var resultt = _INVContext.INV_T_PurchaseRequisitionDMO.Where(t => t.INVMPR_Id == data.INVMPR_Id).ToList();
                    foreach (var rt in resultt)
                    {
                        var resultA = _INVContext.INV_T_PurchaseRequisitionDMO.Single(t => t.INVMPR_Id == rt.INVMPR_Id && t.INVTPR_Id == rt.INVTPR_Id);
                        if (result.INVMPR_ActiveFlg == true)
                        {
                            resultA.INVTPR_ActiveFlg = true;
                            resultA.UpdatedDate = DateTime.Now;
                            _INVContext.Update(resultA);
                        }
                        if (result.INVMPR_ActiveFlg == false)
                        {
                            resultA.INVTPR_ActiveFlg = false;
                            resultA.UpdatedDate = DateTime.Now;
                            _INVContext.Update(resultA);
                        }
                    }

                    int returnval = _INVContext.SaveChanges();
                    if (returnval > 0)
                    {
                        data.returnval_1 = "Success";
                    }
                    else
                    {
                        data.returnval_1 = "Fail";
                    }
                }
                else
                {
                    data.returnval_1 = "Process";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public INV_PurchaseRequisitionDTO deactive(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_PurchaseRequisitionDMO.Single(t => t.INVTPR_Id == data.INVTPR_Id);

                if (result.INVTPR_ActiveFlg == true)
                {
                    result.INVTPR_ActiveFlg = false;
                }
                else if (result.INVTPR_ActiveFlg == false)
                {
                    result.INVTPR_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                int countactiveT = 0;
                int countactiveF = 0;
                var resultt = _INVContext.INV_T_PurchaseRequisitionDMO.Where(t => t.INVMPR_Id == data.INVMPR_Id).ToList();
                foreach (var rt in resultt)
                {
                    if (rt.INVTPR_ActiveFlg == false)
                    {
                        countactiveF += 1;
                    }
                    else if (rt.INVTPR_ActiveFlg == true)
                    {
                        countactiveT += 1;
                    }
                }
                var resultmflg = _INVContext.INV_M_PurchaseRequisitionDMO.Single(t => t.INVMPR_Id == data.INVMPR_Id);
                if (countactiveF > 0 && countactiveT == 0)
                {
                    resultmflg.INVMPR_ActiveFlg = false;
                    result.UpdatedDate = DateTime.Now;
                    _INVContext.Update(resultmflg);
                }
                else if (countactiveT > 0 && countactiveF == 0)
                {
                    resultmflg.INVMPR_ActiveFlg = true;
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
