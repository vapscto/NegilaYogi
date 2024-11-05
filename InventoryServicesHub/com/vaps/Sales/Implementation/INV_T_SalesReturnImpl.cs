using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
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
    public class INV_T_SalesReturnImpl : Interface.INV_T_SalesReturnInterface
    {
        public InventoryContext _INVContext;

        public DomainModelMsSqlServerContext _db;
        public INV_T_SalesReturnImpl(InventoryContext InvContext, DomainModelMsSqlServerContext db)
        {
            _INVContext = InvContext;

            _db = db;
        }

        public async Task<INV_T_SalesReturnDTO> getloaddata(INV_T_SalesReturnDTO data)
        {
            try
            {
                // data.MI_Id = 10;
                long Id = 0;

                var rolelist = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleId).ToList();
                if (rolelist[0].IVRMRT_Role == "Student")
                {
                    Id = data.AMST_Id;
                }
                else if (rolelist[0].IVRMRT_Role == "Staff")
                {
                    
                    var HRME_Id = _db.Staff_User_Login.Where(R => R.Id == data.UserId).Select(R => R.Emp_Code).FirstOrDefault();
                    Id = HRME_Id;
                }
               
                //var HRME_Id = _db.Staff_User_Login.Where(R => R.Id == data.UserId).Select(R => R.Emp_Code).FirstOrDefault();

               // if ()
                    //data.get_salesno = _INVContext.INV_M_SalesDMO.Where(m => m.MI_Id == data.MI_Id && m.INVMSL_ActiveFlg == true).OrderByDescending(m => m.INVMSL_Id).ToArray();

                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_get_SaleReturnList";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@User_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.UserId
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
              SqlDbType.VarChar)
                        {
                            Value = rolelist[0].IVRMRT_Role
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
                            data.get_saleReturn = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                //
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_get_SalesList";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
            SqlDbType.BigInt)
                    {
                        Value = Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
          SqlDbType.VarChar)
                    {
                        Value = rolelist[0].IVRMRT_Role
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
                        data.get_salesno = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<INV_T_SalesReturnDTO> getitem(INV_T_SalesReturnDTO data)
        {
            try
            {
                //data.MI_Id = 10;
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GetSalesReturnItem";
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

                    cmd.Parameters.Add(new SqlParameter("@Type",
            SqlDbType.VarChar)
                    {
                        Value = "Item"
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
                        data.get_item = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                data.get_Store = (from a in _INVContext.INV_M_SalesDMO
                                  from b in _INVContext.INV_Master_StoreDMO

                                  where (a.INVMST_Id == b.INVMST_Id && a.MI_Id == data.MI_Id && a.INVMSL_Id == data.INVMSL_Id)
                                  select new INV_T_SalesReturnDTO
                                  {
                                      INVMST_Id = Convert.ToInt32(b.INVMST_Id),
                                      INVMS_StoreName = b.INVMS_StoreName,
                                  }).Distinct().OrderBy(m => m.INVMS_StoreName).ToArray();

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GetSalesTaxDetails";
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
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.INVMI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@Type",
            SqlDbType.VarChar)
                    {
                        Value = "Tax"
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
                        data.get_itemTax = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<INV_T_SalesReturnDTO> getitemDetail(INV_T_SalesReturnDTO data)
        {
            try
            {
                //data.MI_Id = 10;
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GetSalesReturnItemDetails";
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
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.INVMI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.INVMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
            SqlDbType.VarChar)
                    {
                        Value = "ItemDetails"
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
                        data.get_itemDetail = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GetSalesTaxDetails";
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
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.INVMI_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@Type",
            SqlDbType.VarChar)
                    {
                        Value = "Tax"
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
                        data.get_itemTax = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<INV_T_SalesReturnDTO> savedetails(INV_T_SalesReturnDTO data)
        {
            try
            {
                long sale_id = 0;
                string s = "";
                var res = _INVContext.INV_M_Sales_Return_DMO_con.Where(t => t.INVMSLRET_SalesReturnNo == data.trans_id && t.INVMST_Id == data.INVMST_Id && t.MI_Id == data.MI_Id).ToList();
                if (res.Count > 0)
                {
                    data.returnduplicatestatus = "Duplicate";
                }
                if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                    data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);



                }
                else
                {
                    INV_M_Sales_Return_DMO sale = new INV_M_Sales_Return_DMO();
                    sale.MI_Id = data.MI_Id;
                    sale.INVMST_Id = data.INVMST_Id;
                    sale.INVMSL_Id = data.INVMSL_Id;
                    sale.INVMSLRET_SalesReturnNo = data.trans_id;
                    sale.INVMSLRET_SalesReturnDate = data.INVMSLRET_SalesReturnDate;
                    sale.INVMSLRET_TotalReturnAmount = data.INVMSLRET_TotalReturnAmount;
                    sale.INVMSLRET_ReturnRemarks = data.INVMSLRET_ReturnRemarks;
                    sale.INVMSLRET_CreditNoteDate = data.INVMSLRET_CreditNoteDate;
                    sale.INVMSLRET_CreditNoteNo = data.INVMSLRET_CreditNoteNo;
                    sale.INVMSLRET_EWayRefNo = data.INVMSLRET_EWayRefNo;
                    sale.INVMSLRET_ActiveFlg = true;
                    sale.INVMSLRET_CreatedDate = DateTime.Today;
                    sale.INVMSLRET_UpdatedDate = DateTime.Today;
                    sale.INVMSLRET_CreatedBy = data.UserId;
                    sale.INVMSLRET_UpdatedBy = data.UserId;
                    _INVContext.Add(sale);

                    //===================================Sales Transcation Data
                    foreach (var i in data.SaleItem)
                    {
                        INV_T_Sales_Return_DMO saleitem = new INV_T_Sales_Return_DMO();
                        saleitem.INVMSLRET_Id = sale.INVMSLRET_Id;
                        saleitem.INVMI_Id = i.INVMI_Id;
                        saleitem.INVMUOM_Id = i.INVMUOM_Id;
                        saleitem.INVMP_Id = i.INVMP_Id;
                        saleitem.INVTSLRET_BatchNo = i.INVTSLRET_BatchNo;
                        saleitem.INVTSLRET_SalesReturnQty = i.INVTSLRET_SalesReturnQty;
                        saleitem.INVTSLRET_SalesReturnAmount = i.INVTSLRET_SalesReturnAmount;
                        saleitem.INVTSLRET_SalesReturnNaration = i.INVTSLRET_SalesReturnNaration;
                        saleitem.INVTSLRET_ReturnDate = data.INVMSLRET_SalesReturnDate;
                        saleitem.INVTSLRET_ActiveFlg = true;
                        saleitem.INVTSLRET_CreatedDate = DateTime.Today;
                        saleitem.INVTSLRET_UpdatedDate = DateTime.Today;
                        saleitem.INVTSLRET_CreatedBy = data.UserId;
                        saleitem.INVTSLRET_UpdatedBy = data.UserId;
                        _INVContext.Add(saleitem);
                        //===================================Sales Transcation Tax
                        if (i.saleItemTax != null)
                        {
                            foreach (var t in i.saleItemTax)
                            {
                                INV_T_Sales_Tax_Return_DMO saletax = new INV_T_Sales_Tax_Return_DMO();
                                saletax.INVTSLRET_Id = saleitem.INVTSLRET_Id;
                                saletax.INVMT_Id = t.INVMT_Id;
                                saletax.INVTSLTRET_TaxPer = t.INVTSLT_TaxPer;
                                saletax.INVTSLTRET_TaxAmt = t.INVTSLT_TaxAmt;
                                saletax.INVTSLTRET_ActiveFlg = true;
                                saletax.INVTSLTRET_CreatedDate = DateTime.Today;
                                saletax.INVTSLTRET_UpdatedDate = DateTime.Today;
                                saletax.INVTSLTRET_CreatedBy = data.UserId;
                                saletax.INVTSLTRET_UpdatedBy = data.UserId;
                                _INVContext.Add(saletax);
                            }
                        }

                        using (var dbSaleTxn = _INVContext.Database.BeginTransaction())
                        {
                            sale_id = sale.INVMSL_Id;
                            var contexttrans = _INVContext.SaveChanges();
                            dbSaleTxn.Commit();

                            if (contexttrans > 0)
                            {
                                //try
                                //{
                                //    var contactExistsP = _INVContext.Database.ExecuteSqlCommand("INV_InsertSales @p0,@p1,@p2,@p3,@p4", data.MI_Id, data.INVMST_Id, sale.INVMSL_Id, saleitem.INVMI_Id, saleitem.INVTSL_SalesPrice);
                                //    if (contactExistsP > 0)
                                //    {
                                //        data.returnduplicatestatus = "Updated";
                                //    }
                                //    else
                                //    {
                                //        data.returnduplicatestatus = "notUpdated";
                                //    }
                                //}
                                //catch (Exception ex)
                                //{
                                //    Console.WriteLine(ex.Message);
                                //}
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

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public async Task<INV_T_SalesReturnDTO> deactive(INV_T_SalesReturnDTO data)
        {
            try
            {
                var result = _INVContext.INV_M_Sales_Return_DMO_con.Single(t => t.INVMSLRET_Id == data.INVMSLRET_Id && t.MI_Id == data.MI_Id);

                if (result.INVMSLRET_ActiveFlg == true)
                {
                    result.INVMSLRET_ActiveFlg = false;
                }
                else if (result.INVMSLRET_ActiveFlg == false)
                {
                    result.INVMSLRET_ActiveFlg = true;
                }
                result.INVMSLRET_UpdatedDate = DateTime.Now;
                _INVContext.Update(result);

                var resultt = _INVContext.INV_T_Sales_Return_DMO_con.Where(t => t.INVMSLRET_Id == data.INVMSLRET_Id).ToList();
                foreach (var r in resultt)
                {
                    var slt = _INVContext.INV_T_Sales_Return_DMO_con.Single(t => t.INVMSLRET_Id == data.INVMSLRET_Id && t.INVTSLRET_Id == r.INVTSLRET_Id);

                    if (slt.INVTSLRET_ActiveFlg == true)
                    {
                        slt.INVTSLRET_ActiveFlg = false;
                    }
                    else if (slt.INVTSLRET_ActiveFlg == false)
                    {
                        slt.INVTSLRET_ActiveFlg = true;
                    }
                    slt.INVTSLRET_UpdatedDate = DateTime.Now;
                    _INVContext.Update(slt);

                    var tax = _INVContext.INV_T_Sales_Tax_Return_DMO_con.Where(t => t.INVTSLRET_Id == slt.INVTSLRET_Id).ToList();
                    foreach (var tt in tax)
                    {
                        var tax1 = _INVContext.INV_T_Sales_Tax_Return_DMO_con.Single(t => t.INVTSLRET_Id == slt.INVTSLRET_Id && t.INVTSLTRET_Id == tt.INVTSLTRET_Id);
                        if (tax1.INVTSLTRET_ActiveFlg == true)
                        {
                            tax1.INVTSLTRET_ActiveFlg = false;
                        }
                        else if (tax1.INVTSLTRET_ActiveFlg == false)
                        {
                            tax1.INVTSLTRET_ActiveFlg = true;
                        }
                        tax1.INVTSLTRET_UpdatedDate = DateTime.Now;
                        _INVContext.Update(tax1);
                    }
                }


                int returnval = _INVContext.SaveChanges();
                if (returnval > 0)
                {
                    try
                    {
                        //var contextupdate = _INVContext.Database.ExecuteSqlCommand("INV_UpdateStockforDeactiveSales @p0,@p1,@p2,@p3", data.MI_Id, data.IMFY_Id, data.INVMSL_Id, data.INVMST_Id);
                        //if (contextupdate > 0)
                        //{
                        //    data.returnduplicatestatus = "Updated";
                        //}
                        //else
                        //{
                        //    data.returnduplicatestatus = "notUpdated";
                        //}
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
        public async Task<INV_T_SalesReturnDTO> viewitem(INV_T_SalesReturnDTO data)
        {
            try
            {

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_get_SaleReturnItemView";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMSLRET_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.INVMSLRET_Id
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
                        data.get_salereturnitemview = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
