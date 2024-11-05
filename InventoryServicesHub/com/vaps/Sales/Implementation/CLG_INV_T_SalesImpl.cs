using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class CLG_INV_T_SalesImpl : Interface.CLG_INV_T_SalesInterface
    {
        public InventoryContext _INVContext;
        ILogger<CLG_INV_T_SalesImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public CLG_INV_T_SalesImpl(InventoryContext InvContext, ILogger<CLG_INV_T_SalesImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public INV_T_SalesDTO getloaddata(INV_T_SalesDTO data)
        {
            try
            {
                data.get_Sale = _INVContext.INV_M_SalesDMO.Where(m => m.MI_Id == data.MI_Id).OrderBy(m => m.INVMSL_Id).ToArray();
                data.get_employee = (from a in _INVContext.MasterEmployee
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true)
                                     select new INV_T_SalesDTO
                                     {
                                         employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeOrder = a.HRME_EmployeeOrder,

                                     }).Distinct().OrderBy(h => h.HRME_EmployeeOrder).ToArray();

                data.course_list = (from a in _INVContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _INVContext.AcademicYear
                                    from d in _INVContext.MasterCourseDMO
                                    where (a.MI_Id == c.MI_Id && a.AMCO_Id == d.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMCO_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select new INV_T_SalesDTO
                                    {
                                        AMCO_Id = d.AMCO_Id,
                                        AMCO_CourseName = d.AMCO_CourseName,
                                        AMCO_CourseCode = d.AMCO_CourseCode,
                                        AMCO_CourseFlag = d.AMCO_CourseFlag,
                                        AMCO_ActiveFlag = d.AMCO_ActiveFlag,
                                        AMCO_Order = d.AMCO_Order
                                    }
                          ).Distinct().OrderBy(c => c.AMCO_Order).ToArray();

                data.get_customer = _INVContext.INV_Master_CustomerDMO.Where(h => h.MI_Id == data.MI_Id && h.INVMC_ActiveFlg == true).OrderBy(h => h.INVMC_Id).Distinct().ToArray();
                data.get_Product = _INVContext.INV_Master_ProductDMO.Where(h => h.MI_Id == data.MI_Id && h.INVMP_ActiveFlg == true).OrderBy(h => h.INVMP_Id).Distinct().ToArray();
                data.get_Store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMST_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sales load Page:" + ex.Message);
            }
            return data;
        }

        public INV_T_SalesDTO getbranchlist(INV_T_SalesDTO data)
        {
            try
            {
                data.branch_list = (from a in _INVContext.CLG_Adm_College_AY_CourseDMO
                                    from b in _INVContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from c in _INVContext.AcademicYear
                                    from d in _INVContext.ClgMasterBranchDMO
                                    where (a.ACAYC_Id == b.ACAYC_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMB_Id == d.AMB_Id && d.AMB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id)
                                    select new INV_T_SalesDTO
                                    {
                                        AMB_Id = d.AMB_Id,
                                        AMB_BranchName = d.AMB_BranchName,
                                        AMB_BranchCode = d.AMB_BranchCode,
                                        AMB_ActiveFlag = d.AMB_ActiveFlag,
                                        AMB_Order = d.AMB_Order
                                    }
                      ).Distinct().OrderBy(c => c.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("getbranchlist :" + ex.Message);
            }
            return data;
        }

        public INV_T_SalesDTO getsemesterlist(INV_T_SalesDTO data)
        {
            try
            {
                data.sem_list = (from a in _INVContext.CLG_Adm_College_AY_CourseDMO
                                 from b in _INVContext.CLG_Adm_College_AY_Course_BranchDMO
                                 from c in _INVContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                 from d in _INVContext.AcademicYear
                                 from e in _INVContext.CLG_Adm_Master_SemesterDMO
                                 where (a.ACAYC_Id == b.ACAYC_Id && b.ACAYCB_Id == c.ACAYCB_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == d.ASMAY_Id && c.AMSE_Id == e.AMSE_Id && e.AMSE_ActiveFlg == true && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMB_Id == data.AMB_Id)
                                 select new INV_T_SalesDTO
                                 {
                                     AMSE_Id = e.AMSE_Id,
                                     AMSE_SEMName = e.AMSE_SEMName,
                                     AMSE_SEMCode = e.AMSE_SEMCode,
                                     AMSE_SEMOrder = e.AMSE_SEMOrder
                                 }
                            ).Distinct().OrderBy(c => c.AMSE_SEMOrder).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("semester List:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_T_SalesDTO> getStudentlist(INV_T_SalesDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_CLG_STUDENTLIST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
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
                        data.get_Studentlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("get_StudentList :" + ex.Message);
            }
            return data;
        }

        public async Task<INV_T_SalesDTO> getitem(INV_T_SalesDTO data)
        {
            try
            {
                var config = _INVContext.INV_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVC_ProcessApplFlg == true).ToList();
                data.INVC_LIFOFIFOFlg = config.FirstOrDefault().INVC_LIFOFIFOFlg;

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_LIFO_FIFO_ITEM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVC_LIFOFIFOFlg",
                  SqlDbType.VarChar)
                    {
                        Value = data.INVC_LIFOFIFOFlg
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.INVMST_Id
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
                        data.get_item = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Inventory Sales get items:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_T_SalesDTO> getitemDetail(INV_T_SalesDTO data)
        {
            try
            {

                var config = _INVContext.INV_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVC_ProcessApplFlg == true).ToList();
                var lifofifo = config.FirstOrDefault().INVC_LIFOFIFOFlg;
                if (lifofifo == "LIFO")
                {
                    var itemDetail = (from a in _INVContext.INV_StockDMO
                                      from b in _INVContext.INV_Master_ItemDMO
                                      from c in _INVContext.INV_Master_UOMDMO
                                      from d in _INVContext.INV_StockDMO
                                      where (a.INVMI_Id == b.INVMI_Id && b.INVMUOM_Id == c.INVMUOM_Id && a.INVSTO_Id == d.INVSTO_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMST_Id == data.INVMST_Id && a.INVMI_Id == data.INVMI_Id && a.INVSTO_SalesRate == data.INVSTO_SalesRate && b.INVMI_ActiveFlg == true)
                                      select new INV_T_SalesDTO
                                      {
                                          INVMST_Id = a.INVMST_Id,
                                          INVMI_Id = a.INVMI_Id,
                                          INVMUOM_Id = c.INVMUOM_Id,
                                          INVSTO_Id = a.INVSTO_Id,
                                          INVMUOM_UOMName = c.INVMUOM_UOMName,
                                          INVMUOM_UOMAliasName = c.INVMUOM_UOMAliasName,
                                          INVSTO_BatchNo = a.INVSTO_BatchNo,
                                          INVSTO_SalesRate = a.INVSTO_SalesRate,
                                          INVSTO_PurchaseDate = a.INVSTO_PurchaseDate
                                      }).Distinct().OrderByDescending(d => d.INVSTO_PurchaseDate).ToArray();

                    data.get_itemDetail = itemDetail.ToArray();
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_AvaiableStock";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.INVMI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVSTO_SalesRate",
                      SqlDbType.VarChar)
                        {
                            Value = data.INVSTO_SalesRate
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.INVMST_Id
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
                            data.availablestock = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    var itemDetail = (from a in _INVContext.INV_StockDMO
                                      from b in _INVContext.INV_Master_ItemDMO
                                      from c in _INVContext.INV_Master_UOMDMO
                                      from d in _INVContext.INV_StockDMO
                                      where (a.INVMI_Id == b.INVMI_Id && b.INVMUOM_Id == c.INVMUOM_Id && a.INVSTO_Id == d.INVSTO_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMST_Id == data.INVMST_Id && a.INVMI_Id == data.INVMI_Id && a.INVSTO_SalesRate == data.INVSTO_SalesRate && b.INVMI_ActiveFlg == true)
                                      select new INV_T_SalesDTO
                                      {
                                          INVMST_Id = a.INVMST_Id,
                                          INVMI_Id = a.INVMI_Id,
                                          INVMUOM_Id = c.INVMUOM_Id,
                                          INVSTO_Id = a.INVSTO_Id,
                                          INVMUOM_UOMName = c.INVMUOM_UOMName,
                                          INVMUOM_UOMAliasName = c.INVMUOM_UOMAliasName,
                                          INVSTO_BatchNo = a.INVSTO_BatchNo,
                                          INVSTO_SalesRate = a.INVSTO_SalesRate,
                                          INVSTO_PurchaseDate = a.INVSTO_PurchaseDate
                                      }).Distinct().OrderBy(d => d.INVSTO_PurchaseDate).ToList();
                    data.get_itemDetail = itemDetail.ToArray();

                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_AvaiableStock";
                        cmd.CommandType = CommandType.StoredProcedure;


                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.INVMI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVSTO_SalesRate",
                      SqlDbType.VarChar)
                        {
                            Value = data.INVSTO_SalesRate
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
                    SqlDbType.BigInt)
                        {
                            Value = data.INVMST_Id
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
                            data.availablestock = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                data.get_itemTax = (from a in _INVContext.INV_Master_ItemDMO
                                    from b in _INVContext.INV_Master_Item_TaxDMO
                                    from c in _INVContext.INV_Master_TaxDMO
                                    from d in _INVContext.INV_StockDMO
                                    where (a.INVMI_Id == b.INVMI_Id && b.INVMT_Id == c.INVMT_Id && a.INVMI_Id == d.INVMI_Id && a.MI_Id == c.MI_Id && a.INVMI_Id == data.INVMI_Id && d.INVSTO_SalesRate == data.INVSTO_SalesRate && a.INVMI_ActiveFlg == true && b.INVMIT_ActiveFlg == true && c.INVMT_ActiveFlg == true)
                                    select new INV_T_SalesDTO
                                    {
                                        INVMI_Id = a.INVMI_Id,
                                        INVMI_ItemName = a.INVMI_ItemName,
                                        INVMT_Id = b.INVMT_Id,
                                        INVMT_TaxName = c.INVMT_TaxName,
                                        INVMT_TaxAliasName = c.INVMT_TaxAliasName,
                                        INVMIT_TaxValue = b.INVMIT_TaxValue,


                                    }).Distinct().OrderBy(m => m.INVMT_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sales Item Details:" + ex.Message);
            }
            return data;
        }

        public INV_T_SalesDTO savedetails(INV_T_SalesDTO data)
        {
            try
            {
                if (data.INVMSL_Id > 0)
                {
                    var res = _INVContext.INV_M_SalesDMO.Where(t => t.INVMSL_SalesNo == data.trans_id && t.INVMSL_SalesDate == data.INVMSL_SalesDate && t.INVMSL_SalesValue == data.INVMSL_SalesValue && t.INVMSL_TotalAmount == data.INVMSL_TotalAmount && t.INVMSL_TotDiscount == data.INVMSL_TotDiscount && t.INVMSL_TotTaxAmt == data.INVMSL_TotTaxAmt && t.INVMST_Id == data.INVMST_Id && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _INVContext.INV_M_SalesDMO.Single(t => t.MI_Id == data.MI_Id && t.INVMSL_Id == data.INVMSL_Id);
                        result.MI_Id = data.MI_Id;
                        result.INVMSL_StuOtherFlg = data.INVMSL_StuOtherFlg;
                        result.INVMSL_SalesDate = data.INVMSL_SalesDate;
                        result.INVMSL_SalesValue = data.INVMSL_SalesValue;
                        result.INVMSL_TotDiscount = data.INVMSL_TotDiscount;
                        result.INVMSL_TotTaxAmt = data.INVMSL_TotTaxAmt;
                        result.INVMSL_TotalAmount = data.INVMSL_TotalAmount;
                        result.INVMSL_Remarks = data.INVMSL_Remarks;
                        result.INVMSL_CreditFlg = data.INVMSL_CreditFlg;
                        result.INVMSL_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _INVContext.Update(result);

                        //===================================Sales Transcation Data
                        foreach (var i in data.SaleItem)
                        {
                            var resulttrans = _INVContext.INV_T_SalesDMO.Single(t => t.INVTSL_Id == i.INVTSL_Id);
                            resulttrans.INVMSL_Id = data.INVMSL_Id;
                            resulttrans.INVMI_Id = i.INVMI_Id;
                            resulttrans.INVMUOM_Id = i.INVMUOM_Id;
                            resulttrans.INVTSL_BatchNo = i.INVTSL_BatchNo;
                            resulttrans.INVTSL_SalesQty = i.INVTSL_SalesQty;
                            resulttrans.INVTSL_TaxAmt = i.INVTSL_TaxAmt;
                            resulttrans.INVTSL_SalesPrice = i.INVTSL_SalesPrice;
                            resulttrans.INVTSL_DiscountAmt = i.INVTSL_DiscountAmt;
                            resulttrans.INVTSL_Amount = i.INVTSL_Amount;
                            resulttrans.INVTSL_Naration = i.INVTSL_Naration;
                            resulttrans.INVTSL_ActiveFlg = true;
                            resulttrans.CreatedDate = DateTime.Now;
                            resulttrans.UpdatedDate = DateTime.Now;
                            _INVContext.Update(resulttrans);
                            //===================================Sales Transcation Tax
                            foreach (var t in i.saleItemTax)
                            {
                                var resultlist = _INVContext.INV_T_Sales_TaxDMO.Where(a => a.INVTSL_Id == t.INVTSL_Id);
                                foreach (var tx in resultlist)
                                {
                                    var resultsaletax = _INVContext.INV_T_Sales_TaxDMO.Single(x => x.INVTSLT_Id == tx.INVTSLT_Id);
                                    resultsaletax.INVTSL_Id = resulttrans.INVTSL_Id;
                                    resultsaletax.INVMT_Id = t.INVMT_Id;
                                    resultsaletax.INVTSLT_TaxPer = t.INVTSLT_TaxPer;
                                    resultsaletax.INVTSLT_TaxAmt = t.INVTSLT_TaxAmt;
                                    resultsaletax.INVTSLT_ActiveFlg = true;

                                    resultsaletax.CreatedDate = DateTime.Now;
                                    resultsaletax.UpdatedDate = DateTime.Now;
                                    _INVContext.Update(resultsaletax);
                                }
                            }
                            using (var dbSaleTxn = _INVContext.Database.BeginTransaction())
                            {
                                var contexttrans = _INVContext.SaveChanges();
                                dbSaleTxn.Commit();
                                if (contexttrans > 0)
                                {
                                    try
                                    {
                                        var contactupdate = _INVContext.Database.ExecuteSqlCommand("INV_UpdateSales @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.INVMST_Id, data.INVMSL_Id, resulttrans.INVMI_Id, resulttrans.INVTSL_SalesPrice);
                                        if (contactupdate > 0)
                                        {
                                            data.returnduplicatestatus = "Updated";
                                        }
                                        else
                                        {
                                            data.returnduplicatestatus = "notUpdated";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                    data.returnval = true;
                                }
                                else
                                {
                                    dbSaleTxn.Rollback();
                                    data.returnval = false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                        data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                        data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                        data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                    }
                    INV_M_SalesDMO sale = new INV_M_SalesDMO();
                    sale.MI_Id = data.MI_Id;
                    sale.INVMST_Id = data.INVMST_Id;
                    sale.INVMP_Id = data.INVMP_Id;
                    sale.INVMSL_StuOtherFlg = data.INVMSL_StuOtherFlg;
                    sale.INVMSL_SalesNo = data.trans_id;
                    sale.INVMSL_SalesDate = data.INVMSL_SalesDate;
                    sale.INVMSL_SalesValue = data.INVMSL_SalesValue;
                    sale.INVMSL_TotDiscount = data.INVMSL_TotDiscount;
                    sale.INVMSL_TotTaxAmt = data.INVMSL_TotTaxAmt;
                    sale.INVMSL_TotalAmount = data.INVMSL_TotalAmount;
                    sale.INVMSL_Remarks = data.INVMSL_Remarks;
                    sale.INVMSL_CreditFlg = data.INVMSL_CreditFlg;
                    sale.INVMSL_ActiveFlg = true;
                    sale.CreatedDate = DateTime.Now;
                    sale.UpdatedDate = DateTime.Now;
                    _INVContext.Add(sale);

                    if (data.INVMSL_StuOtherFlg == "Student")
                    {
                        foreach (var c in data.clgStudentList)
                        {
                            INV_M_Sales_College_StudentDMO Stulist = new INV_M_Sales_College_StudentDMO();
                            Stulist.INVMSL_Id = sale.INVMSL_Id;
                            Stulist.AMCST_Id = c.AMCST_Id;
                            Stulist.AMCO_Id = c.AMCO_Id;
                            Stulist.AMB_Id = c.AMB_Id;
                            Stulist.AMSE_Id = c.AMSE_Id;
                            Stulist.ASMAY_Id = data.ASMAY_Id;
                            Stulist.INVMSLCS_ActiveFlg = true;

                            Stulist.CreatedDate = DateTime.Now;
                            Stulist.UpdatedDate = DateTime.Now;
                            _INVContext.Add(Stulist);
                        }
                    }
                    else if (data.INVMSL_StuOtherFlg == "Staff")
                    {
                        INV_M_Sales_StaffDMO staff = new INV_M_Sales_StaffDMO();

                        staff.INVMSL_Id = sale.INVMSL_Id;
                        staff.HRME_Id = data.HRME_Id;
                        staff.INVMSLST_ActiveFlg = true;

                        staff.CreatedDate = DateTime.Now;
                        staff.UpdatedDate = DateTime.Now;
                        _INVContext.Add(staff);
                    }
                    else if (data.INVMSL_StuOtherFlg == "Customer")
                    {
                        INV_M_Sales_CustomerDMO customer = new INV_M_Sales_CustomerDMO();
                        customer.INVMSL_Id = sale.INVMSL_Id;
                        customer.INVMC_Id = data.INVMC_Id;
                        customer.INVMSLC_ActiveFlg = true;

                        customer.CreatedDate = DateTime.Now;
                        customer.UpdatedDate = DateTime.Now;
                        _INVContext.Add(customer);
                    }
                    //===================================Sales Transcation Data
                    foreach (var i in data.SaleItem)
                    {
                        INV_T_SalesDMO saleitem = new INV_T_SalesDMO();
                        saleitem.INVMSL_Id = sale.INVMSL_Id;
                        saleitem.INVMI_Id = i.INVMI_Id;
                        saleitem.INVMUOM_Id = i.INVMUOM_Id;
                        saleitem.INVTSL_BatchNo = i.INVTSL_BatchNo;
                        saleitem.INVTSL_SalesQty = i.INVTSL_SalesQty;
                        saleitem.INVTSL_TaxAmt = i.INVTSL_TaxAmt;
                        saleitem.INVTSL_SalesPrice = i.INVTSL_SalesPrice;
                        saleitem.INVTSL_DiscountAmt = i.INVTSL_DiscountAmt;
                        saleitem.INVTSL_Amount = i.INVTSL_Amount;
                        saleitem.INVTSL_Naration = i.INVTSL_Naration;
                        saleitem.INVTSL_ActiveFlg = true;
                        saleitem.CreatedDate = DateTime.Now;
                        saleitem.UpdatedDate = DateTime.Now;
                        _INVContext.Add(saleitem);
                        //===================================Sales Transcation Tax
                        foreach (var t in i.saleItemTax)
                        {
                            INV_T_Sales_TaxDMO saletax = new INV_T_Sales_TaxDMO();
                            saletax.INVTSL_Id = saleitem.INVTSL_Id;
                            saletax.INVMT_Id = t.INVMT_Id;
                            saletax.INVTSLT_TaxPer = t.INVTSLT_TaxPer;
                            saletax.INVTSLT_TaxAmt = t.INVTSLT_TaxAmt;
                            saletax.INVTSLT_ActiveFlg = true;

                            saletax.CreatedDate = DateTime.Now;
                            saletax.UpdatedDate = DateTime.Now;
                            _INVContext.Add(saletax);
                        }
                        using (var dbSaleTxn = _INVContext.Database.BeginTransaction())
                        {
                            var contexttrans = _INVContext.SaveChanges();
                            dbSaleTxn.Commit();

                            if (contexttrans > 0)
                            {
                                try
                                {
                                    var contactExistsP = _INVContext.Database.ExecuteSqlCommand("INV_InsertSales @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.INVMST_Id, sale.INVMSL_Id, saleitem.INVMI_Id, saleitem.INVTSL_SalesPrice);
                                    if (contactExistsP > 0)
                                    {
                                        data.returnduplicatestatus = "Updated";
                                    }
                                    else
                                    {
                                        data.returnduplicatestatus = "notUpdated";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                data.returnval = true;
                            }
                            else
                            {
                                dbSaleTxn.Rollback();
                                data.returnval = false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logInv.LogInformation("Sales savedata :" + ex.Message);
            }

            return data;
        }

        //====================================Grid Model Data
        public async Task<INV_T_SalesDTO> getSaletypes(INV_T_SalesDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_SALE_TYPES_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMSL_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.INVMSL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@saletype",
                    SqlDbType.VarChar)
                    {
                        Value = data.saletype
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
                        data.get_Saletypes = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sale Types :" + ex.Message);
            }
            return data;
        }
        public async Task<INV_T_SalesDTO> getSaleItemDetails(INV_T_SalesDTO data)
        {
            try
            {
                data.get_SaleItemDetails = (from a in _INVContext.INV_T_SalesDMO
                                            from b in _INVContext.INV_M_SalesDMO
                                            from c in _INVContext.INV_Master_ItemDMO
                                            from d in _INVContext.INV_Master_UOMDMO
                                            where (a.INVMSL_Id == b.INVMSL_Id && a.INVMI_Id == c.INVMI_Id && a.INVMUOM_Id == d.INVMUOM_Id && b.MI_Id == c.MI_Id && b.MI_Id == data.MI_Id && a.INVMSL_Id == data.INVMSL_Id)
                                            select new INV_T_SalesDTO
                                            {
                                                INVTSL_Id = a.INVTSL_Id,
                                                INVMSL_Id = a.INVMSL_Id,
                                                INVMI_Id = a.INVMI_Id,
                                                INVMUOM_Id = a.INVMUOM_Id,
                                                INVMSL_SalesNo = b.INVMSL_SalesNo,
                                                INVMI_ItemName = c.INVMI_ItemName,
                                                INVMUOM_UOMName = d.INVMUOM_UOMName,
                                                INVTSL_BatchNo = a.INVTSL_BatchNo,
                                                INVTSL_SalesQty = a.INVTSL_SalesQty,
                                                INVTSL_SalesPrice = a.INVTSL_SalesPrice,
                                                INVTSL_DiscountAmt = a.INVTSL_DiscountAmt,
                                                INVTSL_TaxAmt = a.INVTSL_TaxAmt,
                                                INVTSL_Amount = a.INVTSL_Amount,
                                                INVTSL_Naration = a.INVTSL_Naration,
                                                INVTSL_ActiveFlg = a.INVTSL_ActiveFlg,


                                            }).Distinct().OrderBy(m => m.INVTSL_Id).ToArray();

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_SALE_TYPES_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMSL_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.INVMSL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@saletype",
                    SqlDbType.VarChar)
                    {
                        Value = data.saletype
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
                        data.get_Saletypes = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("sale Model:" + ex.Message);
            }
            return data;
        }

        public INV_T_SalesDTO getSaleItemTax(INV_T_SalesDTO data)
        {
            try
            {
                data.get_SaleItemTax = (from a in _INVContext.INV_T_Sales_TaxDMO
                                        from b in _INVContext.INV_T_SalesDMO
                                        from c in _INVContext.INV_Master_TaxDMO
                                        where (a.INVTSL_Id == b.INVTSL_Id && a.INVMT_Id == c.INVMT_Id && c.MI_Id == data.MI_Id && a.INVTSL_Id == data.INVTSL_Id)
                                        select new INV_T_SalesDTO
                                        {
                                            INVTSLT_Id = a.INVTSLT_Id,
                                            INVTSL_Id = a.INVTSL_Id,
                                            INVMT_Id = a.INVMT_Id,
                                            INVMT_TaxName = c.INVMT_TaxName,
                                            INVTSLT_TaxPer = a.INVTSLT_TaxPer,
                                            INVTSLT_TaxAmt = a.INVTSLT_TaxAmt,
                                            INVTSLT_ActiveFlg = a.INVTSLT_ActiveFlg

                                        }).Distinct().OrderBy(m => m.INVTSLT_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("sale Tax Model:" + ex.Message);
            }
            return data;
        }
        //====================================Grid Activate and Deactivate
        public INV_T_SalesDTO deactive(INV_T_SalesDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_SalesDMO.Single(t => t.INVMSL_Id == data.INVMSL_Id && t.MI_Id == data.MI_Id);

                if (result.INVMSL_ActiveFlg == true)
                {
                    result.INVMSL_ActiveFlg = false;
                }
                else if (result.INVMSL_ActiveFlg == false)
                {
                    result.INVMSL_ActiveFlg = true;
                }
                var resultt = _INVContext.INV_T_SalesDMO.Where(t => t.INVMSL_Id == data.INVMSL_Id).ToList();
                foreach (var r in resultt)
                {
                    if (result.INVMSL_ActiveFlg == true)
                    {
                        r.INVTSL_ActiveFlg = true;
                    }
                    else if (result.INVMSL_ActiveFlg == false)
                    {
                        r.INVTSL_ActiveFlg = false;
                    }
                    r.UpdatedDate = DateTime.Now;
                    _INVContext.Update(r);
                }
                var saletrans = (from a in _INVContext.INV_M_SalesDMO
                                 from b in _INVContext.INV_T_SalesDMO
                                 where (a.INVMSL_Id == b.INVMSL_Id && a.INVMSL_Id == data.INVMSL_Id)
                                 select new INV_T_SalesDTO
                                 {
                                     INVMSL_Id = a.INVMSL_Id,
                                     INVTSL_Id = b.INVTSL_Id,
                                     INVMSL_StuOtherFlg = a.INVMSL_StuOtherFlg,
                                 }).Distinct().OrderBy(m => m.INVMSL_Id).ToList();
                foreach (var sid in saletrans)
                {
                    var resulttax = _INVContext.INV_T_Sales_TaxDMO.Where(t => t.INVTSL_Id == sid.INVTSL_Id).ToList();
                    foreach (var rtax in resulttax)
                    {
                        if (result.INVMSL_ActiveFlg == true)
                        {
                            rtax.INVTSLT_ActiveFlg = true;
                        }
                        else if (result.INVMSL_ActiveFlg == false)
                        {
                            rtax.INVTSLT_ActiveFlg = false;
                        }
                        rtax.UpdatedDate = DateTime.Now;
                        _INVContext.Update(rtax);
                    }
                }
                var type = _INVContext.INV_M_SalesDMO.Where(t => t.INVMSL_Id == data.INVMSL_Id).ToList();
                foreach (var typ in type)
                {
                    if (typ.INVMSL_StuOtherFlg == "Staff")
                    {
                        var salestaff = (from a in _INVContext.INV_M_SalesDMO
                                         from b in _INVContext.INV_M_Sales_StaffDMO
                                         where (a.INVMSL_Id == b.INVMSL_Id && a.INVMSL_Id == data.INVMSL_Id)
                                         select new INV_T_SalesDTO
                                         {
                                             INVMSL_Id = a.INVMSL_Id,
                                             INVMSLST_Id = b.INVMSLST_Id,
                                         }).Distinct().OrderBy(m => m.INVMSLST_Id).ToList();

                        foreach (var salestf in salestaff)
                        {
                            var resultstaff = _INVContext.INV_M_Sales_StaffDMO.Where(t => t.INVMSLST_Id == salestf.INVMSLST_Id).ToList();
                            foreach (var rstaff in resultstaff)
                            {
                                if (result.INVMSL_ActiveFlg == true)
                                {
                                    rstaff.INVMSLST_ActiveFlg = true;
                                }
                                else if (result.INVMSL_ActiveFlg == false)
                                {
                                    rstaff.INVMSLST_ActiveFlg = false;
                                }
                                rstaff.UpdatedDate = DateTime.Now;
                                _INVContext.Update(rstaff);
                            }
                        }
                    }

                    else if (typ.INVMSL_StuOtherFlg == "Student")
                    {
                        var salestudent = (from a in _INVContext.INV_M_SalesDMO
                                           from b in _INVContext.INV_M_Sales_College_StudentDMO
                                           where (a.INVMSL_Id == b.INVMSL_Id && a.INVMSL_Id == data.INVMSL_Id)
                                           select new INV_T_SalesDTO
                                           {
                                               INVMSL_Id = a.INVMSL_Id,
                                               INVMSLCS_Id = b.INVMSLCS_Id,
                                           }).Distinct().OrderBy(m => m.INVMSLCS_Id).ToList();

                        foreach (var salestu in salestudent)
                        {
                            var resultstu = _INVContext.INV_M_Sales_College_StudentDMO.Where(t => t.INVMSLCS_Id == salestu.INVMSLCS_Id).ToList();
                            foreach (var rstu in resultstu)
                            {
                                if (result.INVMSL_ActiveFlg == true)
                                {
                                    rstu.INVMSLCS_ActiveFlg = true;
                                }
                                else if (result.INVMSL_ActiveFlg == false)
                                {
                                    rstu.INVMSLCS_ActiveFlg = false;
                                }
                                rstu.UpdatedDate = DateTime.Now;
                                _INVContext.Update(rstu);
                            }
                        }
                    }
                }

                result.UpdatedDate = DateTime.Now;
                _INVContext.Update(result);
                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    try
                    {
                        //  data.IMFY_Id = 5;
                        var contextupdate = _INVContext.Database.ExecuteSqlCommand("INV_UpdateStockforDeactiveSales @p0,@p1,@p2,@p3", data.MI_Id, data.IMFY_Id, data.INVMSL_Id, data.INVMST_Id);
                        if (contextupdate > 0)
                        {
                            data.returnduplicatestatus = "Updated";
                        }
                        else
                        {
                            data.returnduplicatestatus = "notUpdated";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
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
        public INV_T_SalesDTO deactiveS(INV_T_SalesDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_SalesDMO.Single(t => t.INVTSL_Id == data.INVTSL_Id);

                if (result.INVTSL_ActiveFlg == true)
                {
                    result.INVTSL_ActiveFlg = false;
                }
                else if (result.INVTSL_ActiveFlg == false)
                {
                    result.INVTSL_ActiveFlg = true;
                }

                var resulttx = _INVContext.INV_T_Sales_TaxDMO.Where(t => t.INVTSL_Id == data.INVTSL_Id).ToList();
                foreach (var tx in resulttx)
                {
                    if (result.INVTSL_ActiveFlg == true)
                    {
                        tx.INVTSLT_ActiveFlg = true;
                    }
                    else if (result.INVTSL_ActiveFlg == false)
                    {
                        tx.INVTSLT_ActiveFlg = false;
                    }

                    tx.UpdatedDate = DateTime.Now;
                    _INVContext.Update(tx);
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
        public INV_T_SalesDTO deactivetax(INV_T_SalesDTO data)
        {
            try
            {
                var result = _INVContext.INV_T_Sales_TaxDMO.Single(t => t.INVTSLT_Id == data.INVTSLT_Id);

                if (result.INVTSLT_ActiveFlg == true)
                {
                    result.INVTSLT_ActiveFlg = false;
                }
                else if (result.INVTSLT_ActiveFlg == false)
                {
                    result.INVTSLT_ActiveFlg = true;
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


    }
}
