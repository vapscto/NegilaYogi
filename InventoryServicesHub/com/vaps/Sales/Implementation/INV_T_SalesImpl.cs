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
    public class INV_T_SalesImpl : Interface.INV_T_SalesInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_T_SalesImpl> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_T_SalesImpl(InventoryContext InvContext, ILogger<INV_T_SalesImpl> log, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = db;
        }

        public async Task<INV_T_SalesDTO> getloaddata(INV_T_SalesDTO data)
        {
            try
            {
                data.get_Sale = _INVContext.INV_M_SalesDMO.Where(m => m.MI_Id == data.MI_Id).OrderByDescending(m => m.INVMSL_Id).ToArray();
                data.get_employee = (from a in _INVContext.MasterEmployee
                                     from b in _INVContext.HR_Master_Department
                                     where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRMD_Id == b.HRMD_Id && b.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id)
                                     select new INV_T_SalesDTO
                                     {
                                         //employeename = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "" ? "" : " " + a.HRME_EmployeeFirstName) + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "" || a.HRME_EmployeeMiddleName == "0" ? "" : " " + a.HRME_EmployeeMiddleName) + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "" || a.HRME_EmployeeLastName == "0" ? "" : " " + a.HRME_EmployeeLastName)).Trim(),
                                         employeename = ((a.HRME_EmployeeFirstName == null ? " " : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null ? " " : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null ? " " : a.HRME_EmployeeLastName) + " " + ":" + " " + (b.HRMD_DepartmentName == null ? " " : b.HRMD_DepartmentName)).Trim(),
                                         HRME_EmployeeCode = a.HRME_EmployeeCode,
                                         HRME_Id = a.HRME_Id,
                                         HRME_EmployeeOrder = a.HRME_EmployeeOrder,

                                     }).Distinct().OrderBy(h => h.HRME_EmployeeOrder).ToArray();

                data.get_customer = _INVContext.INV_Master_CustomerDMO.Where(h => h.MI_Id == data.MI_Id && h.INVMC_ActiveFlg == true).OrderBy(h => h.INVMC_Id).Distinct().ToArray();
                data.get_Product = _INVContext.INV_Master_ProductDMO.Where(h => h.MI_Id == data.MI_Id && h.INVMP_ActiveFlg == true).OrderBy(h => h.INVMP_Id).Distinct().ToArray();
                data.get_Store = _INVContext.INV_Master_StoreDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMS_ActiveFlg == true).OrderBy(m => m.INVMS_StoreName).ToArray();
                //try
                //{
                //    var config = _INVContext.INV_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.INVC_ProcessApplFlg == true).ToList();
                //    data.INVC_LIFOFIFOFlg = config.FirstOrDefault().INVC_LIFOFIFOFlg;

                //    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandText = "INV_LIFO_FIFO_ITEM";
                //        cmd.CommandType = CommandType.StoredProcedure;

                //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                // SqlDbType.BigInt)
                //        {
                //            Value = data.MI_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@INVC_LIFOFIFOFlg",
                //      SqlDbType.VarChar)
                //        {
                //            Value = data.INVC_LIFOFIFOFlg
                //        });

                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        var retObject = new List<dynamic>();
                //        try
                //        {
                //            using (var dataReader = await cmd.ExecuteReaderAsync())
                //            {
                //                while (await dataReader.ReadAsync())
                //                {
                //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(
                //                            dataReader.GetName(iFiled),
                //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                        );
                //                    }
                //                    retObject.Add((ExpandoObject)dataRow);
                //                }
                //            }
                //            data.get_item = retObject.ToArray();
                //        }
                //        catch (Exception ex)
                //        {
                //            Console.WriteLine(ex.Message);
                //        }
                //    }                
                //}
                //catch (Exception ex)
                //{
                //    _logInv.LogInformation("Sales Item :" + ex.Message);
                //}

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sales load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_T_SalesDTO> getStudentClsSec(INV_T_SalesDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_AMSTID_CLASS_SECTION";
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
                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = data.type
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
                        data.get_Student_Cls_Sec = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("get_StudentClsSec :" + ex.Message);
            }
            return data;
        }


        public INV_T_SalesDTO getsectionlist(INV_T_SalesDTO data)
        {
            try
            {
                data.get_sectionlist = (from a in _INVContext.Adm_M_Student
                                        from b in _INVContext.School_Adm_Y_StudentDMO
                                        from c in _INVContext.AcademicYear
                                        from d in _INVContext.School_M_Class
                                        from e in _INVContext.School_M_Section
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && b.ASMS_Id == e.ASMS_Id
                                        && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id)
                                        select new INV_T_SalesDTO
                                        {
                                            ASMS_Id = e.ASMS_Id,
                                            ASMC_SectionName = e.ASMC_SectionName
                                        }).Distinct().OrderBy(m => m.ASMS_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Section List:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_T_SalesDTO> getStudentlist(INV_T_SalesDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_STUDENTLIST";
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
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = data.type
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
                _logInv.LogInformation("get_StudentClsSec :" + ex.Message);
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
                                      where (a.INVMI_Id == b.INVMI_Id && b.INVMUOM_Id == c.INVMUOM_Id && a.INVSTO_Id == d.INVSTO_Id
                                      && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMST_Id == data.INVMST_Id
                                      && a.INVMI_Id == data.INVMI_Id && a.INVSTO_SalesRate == data.INVSTO_SalesRate
                                      && b.INVMI_ActiveFlg == true && a.INVSTO_BatchNo == data.INVSTO_BatchNo)
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
                        cmd.Parameters.Add(new SqlParameter("@INVSTO_BatchNo",
                SqlDbType.VarChar)
                        {
                            Value = itemDetail[0].INVSTO_BatchNo
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
                                      where (a.INVMI_Id == b.INVMI_Id && b.INVMUOM_Id == c.INVMUOM_Id && a.INVSTO_Id == d.INVSTO_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMST_Id == data.INVMST_Id
                                      && a.INVMI_Id == data.INVMI_Id && a.INVSTO_SalesRate == data.INVSTO_SalesRate && b.INVMI_ActiveFlg == true && a.INVSTO_BatchNo == data.INVSTO_BatchNo)
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
                        cmd.Parameters.Add(new SqlParameter("@INVSTO_BatchNo",
                SqlDbType.VarChar)
                        {
                            Value = itemDetail[0].INVSTO_BatchNo
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

        public async Task<INV_T_SalesDTO> savedetails(INV_T_SalesDTO data)
        {
            try
            {
                long sale_id = 0;
                string s = "";
                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                }
                var res = _INVContext.INV_M_SalesDMO.Where(t => t.INVMSL_SalesNo == data.trans_id && t.INVMSL_SalesDate == data.INVMSL_SalesDate && t.INVMSL_SalesValue == data.INVMSL_SalesValue && t.INVMST_Id == data.INVMST_Id && t.MI_Id == data.MI_Id).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                else
                {
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
                        if (data.Student_flag == "I")
                        {
                            var clsSec = (from a in _INVContext.Adm_M_Student
                                          from b in _INVContext.School_Adm_Y_StudentDMO
                                          from c in _INVContext.AcademicYear
                                          from d in _INVContext.School_M_Class
                                          from e in _INVContext.School_M_Section
                                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && b.ASMS_Id == e.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id)
                                          select new INV_T_SalesDTO
                                          {
                                              ASMCL_Id = d.ASMCL_Id,
                                              ASMS_Id = e.ASMS_Id
                                          }).Distinct().ToArray();

                            INV_M_Sales_StudentDMO salestudent = new INV_M_Sales_StudentDMO();
                            salestudent.INVMSL_Id = sale.INVMSL_Id;
                            salestudent.AMST_Id = data.AMST_Id;
                            salestudent.ASMCL_Id = clsSec.FirstOrDefault().ASMCL_Id;
                            salestudent.ASMS_Id = clsSec.FirstOrDefault().ASMS_Id;
                            salestudent.ASMAY_Id = data.ASMAY_Id;
                            salestudent.INVMSLS_ActiveFlg = true;

                            salestudent.CreatedDate = DateTime.Now;
                            salestudent.UpdatedDate = DateTime.Now;
                            _INVContext.Add(salestudent);
                            //==================SMS===================
                            var mob = _INVContext.Adm_M_Student.Single(a => a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id);
                            var itm = _INVContext.INV_Master_ItemDMO.Where(a => a.INVMI_Id == data.SaleItem[0].INVMI_Id && a.MI_Id == data.MI_Id).ToList();
                            var tmp = "INVSales";
                            var template = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == tmp && e.ISES_SMSActiveFlag == true).ToList();
                            if (template.Count > 0)
                            {
                                string templatedetails1 = template.FirstOrDefault().ISES_SMSMessage;
                                templatedetails1 = templatedetails1.Replace("[item]", itm[0].INVMI_ItemName);
                                templatedetails1 = templatedetails1.Replace("[SalesPrice]", Convert.ToInt32(data.INVMSL_SalesValue).ToString());
                                templatedetails1 = templatedetails1.Replace("[SaleDate]", data.INVMSL_SalesDate.ToString());



                                SMS sms = new SMS(_db);
                                s = await sms.sendSalesSms(data.MI_Id, mob.AMST_FatherMobleNo, templatedetails1, Convert.ToInt32(data.INVMSL_SalesValue), data.INVMSL_SalesDate.ToString());
                            }

                            //====================end sms===================
                        }
                        else if (data.Student_flag == "C" || data.Student_flag == "CS")
                        {
                            foreach (var c in data.studentlist)
                            {
                                INV_M_Sales_StudentDMO Stulist = new INV_M_Sales_StudentDMO();
                                Stulist.INVMSL_Id = sale.INVMSL_Id;
                                Stulist.AMST_Id = c.AMST_Id;
                                Stulist.ASMCL_Id = c.ASMCL_Id;
                                Stulist.ASMS_Id = c.ASMS_Id;
                                Stulist.ASMAY_Id = data.ASMAY_Id;
                                Stulist.INVMSLS_ActiveFlg = true;

                                Stulist.CreatedDate = DateTime.Now;
                                Stulist.UpdatedDate = DateTime.Now;
                                _INVContext.Add(Stulist);

                                //==================SMS===================
                                var mob = _INVContext.Adm_M_Student.Single(a => a.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id);
                                var itm = _INVContext.INV_Master_ItemDMO.Where(a => a.INVMI_Id == data.SaleItem[0].INVMI_Id && a.MI_Id == data.MI_Id).ToList();
                                var tmp = "INVSales";
                                var template = _db.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == tmp && e.ISES_SMSActiveFlag == true).ToList();
                                if (template.Count > 0)
                                {
                                    string templatedetails1 = template.FirstOrDefault().ISES_SMSMessage;
                                    templatedetails1 = templatedetails1.Replace("[item]", itm[0].INVMI_ItemName);
                                    templatedetails1 = templatedetails1.Replace("[SalesPrice]", Convert.ToInt32(data.INVMSL_SalesValue).ToString());
                                    templatedetails1 = templatedetails1.Replace("[SaleDate]", data.INVMSL_SalesDate.ToString());

                                    SMS sms = new SMS(_db);
                                    s = await sms.sendSalesSms(data.MI_Id, mob.AMST_FatherMobleNo, templatedetails1, Convert.ToInt32(data.INVMSL_SalesValue), data.INVMSL_SalesDate.ToString());
                                }

                                //====================end sms===================
                            }
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
                        //saleitem.INVSTO_Id = i.INVSTO_Id;
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
                            sale_id = sale.INVMSL_Id;
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
                    //********************************* SALES SMS ********************************//
                    //long amst_mobileno = 0;
                    //long SalePrice = 0;
                    //string SaleDate = "";
                    //List<long> amstids = new List<long>();
                    //if (data.INVMSL_StuOtherFlg == "Student")
                    //{
                    //    if (data.Student_flag == "I")
                    //    {
                    //        var amstid = _INVContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && a.AMST_Id == data.AMST_Id).Distinct().ToList();
                    //        if (amstid.Count > 0)
                    //        {
                    //            var saledetails = _INVContext.INV_M_SalesDMO.Where(t => t.MI_Id == data.MI_Id && t.INVMSL_Id == sale_id && t.INVMSL_ActiveFlg == true).Distinct().ToList();
                    //            if (saledetails.Count > 0)
                    //            {
                    //                SalePrice = Convert.ToInt64(saledetails.FirstOrDefault().INVMSL_SalesValue);
                    //                SaleDate = saledetails.FirstOrDefault().INVMSL_SalesDate.ToString();

                    //            }
                    //            // amst_mobileno = amstid.FirstOrDefault().AMST_MobileNo;
                    //            amst_mobileno = 9980115560;
                    //            SMS sms = new SMS(_db);
                    //            s = await sms.sendSalesSms(data.MI_Id, amst_mobileno, "INVSales", SalePrice, SaleDate);
                    //        }
                    //    }
                    //    else if (data.Student_flag == "C" || data.Student_flag == "CS")
                    //    {
                    //        long nofstudent = data.studentlist.Length;
                    //        long salerate = 0;
                    //        foreach (var c in data.studentlist)
                    //        {
                    //            amstids.Add(c.AMST_Id);
                    //        }
                    //        if (amstids.Count > 0)
                    //        {
                    //            var amstmobileno = _INVContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S" && amstids.Contains(a.AMST_Id)).Distinct().ToList();

                    //            if (amstmobileno.Count > 0)
                    //            {
                    //                var saledetails = _INVContext.INV_M_SalesDMO.Where(t => t.MI_Id == data.MI_Id && t.INVMSL_Id == sale_id && t.INVMSL_ActiveFlg == true).Distinct().ToList();
                    //                if (saledetails.Count > 0)
                    //                {
                    //                    salerate = Convert.ToInt64(saledetails.FirstOrDefault().INVMSL_SalesValue);
                    //                    SaleDate = saledetails.FirstOrDefault().INVMSL_SalesDate.ToString();
                    //                    SalePrice = salerate / nofstudent;
                    //                }
                    //                foreach (var m in amstmobileno)
                    //                {
                    //                    amst_mobileno = m.AMST_MobileNo;
                    //                    SMS sms = new SMS(_db);
                    //                    s = await sms.sendSalesSms(data.MI_Id, amst_mobileno, "INVSales", SalePrice, SaleDate);
                    //                }
                    //            }
                    //        }

                    //    }
                    //}



                    //var contactExists = _INVContext.SaveChanges();
                    //if (contactExists > 0)
                    //{
                    //    data.returnval = true;
                    //}
                    //else
                    //{
                    //    data.returnval = false;
                    //}

                    //else
                    //{
                    //    dbSaleTxn.Rollback();
                    //}
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

                    // if (r.INVTSL_ActiveFlg == true)
                    //{
                    // r.INVTSL_ActiveFlg = false;
                    //}
                    //else if (r.INVTSL_ActiveFlg == false)
                    //{
                    //    r.INVTSL_ActiveFlg = true;
                    //}
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

                        //if (rtax.INVTSLT_ActiveFlg == true)
                        //{
                        //    rtax.INVTSLT_ActiveFlg = false;
                        //}
                        //else if (rtax.INVTSLT_ActiveFlg == false)
                        //{
                        //    rtax.INVTSLT_ActiveFlg = true;
                        //}
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
                                //if (rstaff.INVMSLST_ActiveFlg == true)
                                //{
                                //    rstaff.INVMSLST_ActiveFlg = false;
                                //}
                                //else if (rstaff.INVMSLST_ActiveFlg == false)
                                //{
                                //    rstaff.INVMSLST_ActiveFlg = true;
                                //}
                                rstaff.UpdatedDate = DateTime.Now;
                                _INVContext.Update(rstaff);
                            }
                        }
                    }

                    else if (typ.INVMSL_StuOtherFlg == "Student")
                    {
                        var salestudent = (from a in _INVContext.INV_M_SalesDMO
                                           from b in _INVContext.INV_M_Sales_StudentDMO
                                           where (a.INVMSL_Id == b.INVMSL_Id && a.INVMSL_Id == data.INVMSL_Id)
                                           select new INV_T_SalesDTO
                                           {
                                               INVMSL_Id = a.INVMSL_Id,
                                               INVMSLS_Id = b.INVMSLS_Id,
                                           }).Distinct().OrderBy(m => m.INVMSLS_Id).ToList();

                        foreach (var salestu in salestudent)
                        {
                            var resultstu = _INVContext.INV_M_Sales_StudentDMO.Where(t => t.INVMSLS_Id == salestu.INVMSLS_Id).ToList();
                            foreach (var rstu in resultstu)
                            {
                                if (result.INVMSL_ActiveFlg == true)
                                {
                                    rstu.INVMSLS_ActiveFlg = true;
                                }
                                else if (result.INVMSL_ActiveFlg == false)
                                {
                                    rstu.INVMSLS_ActiveFlg = false;
                                }
                                //if (rstu.INVMSLS_ActiveFlg == true)
                                //{
                                //    rstu.INVMSLS_ActiveFlg = false;
                                //}
                                //else if (rstu.INVMSLS_ActiveFlg == false)
                                //{
                                //    rstu.INVMSLS_ActiveFlg = true;
                                //}
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
