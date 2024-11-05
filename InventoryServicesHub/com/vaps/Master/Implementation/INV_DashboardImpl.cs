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

namespace InventoryServicesHub.com.vaps.Implementation
{
    public class INV_DashboardImpl : Interface.INV_DashboardInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_DashboardImpl> _logInv;
        public INV_DashboardImpl(InventoryContext InvContext, ILogger<INV_DashboardImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_DashboardDTO> getloaddata(INV_DashboardDTO data)
        {
            try
            {
                #region  Total Purchase,Sales,Stock,ChcekOut,Item,Low_Stock,Warranty    

                var totalp = (from a in _INVContext.INV_StockDMO where a.MI_Id == data.MI_Id select a.INVSTO_PurOBQty).Distinct().Sum();
                var totals = (from a in _INVContext.INV_StockDMO where a.MI_Id == data.MI_Id select a.INVSTO_SalesQty).Distinct().Sum();
                var totalco = (from a in _INVContext.INV_StockDMO where a.MI_Id == data.MI_Id select a.INVSTO_CheckedOutQty).Distinct().Sum();
                var totalas = (from a in _INVContext.INV_StockDMO where a.MI_Id == data.MI_Id select a.INVSTO_AvaiableStock).Distinct().Sum();
                var totalitm = (from a in _INVContext.INV_Master_ItemDMO where a.MI_Id == data.MI_Id && a.INVMI_ActiveFlg == true select a.INVMI_Id).Distinct().Sum();
                data.totPurchase = totalp;
                data.totSales = totals;
                data.totAvailableStock = totalas;
                data.totCheckout = totalco;
                data.totItem = totalitm;
                var totallwstk = (from a in _INVContext.INV_StockDMO
                                  from b in _INVContext.INV_Master_ItemDMO
                                  where (a.INVMI_Id == b.INVMI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVSTO_AvaiableStock < 10)
                                  select new INV_DashboardDTO
                                  {
                                      INVSTO_Id = a.INVSTO_Id,
                                      INVMI_Id = a.INVMI_Id,
                                      INVMI_ItemName = b.INVMI_ItemName,
                                      INVSTO_SalesRate = a.INVSTO_SalesRate,
                                      INVSTO_PurchaseDate = a.INVSTO_PurchaseDate,
                                      INVSTO_AvaiableStock = a.INVSTO_AvaiableStock
                                  }).Distinct().OrderBy(i => i.INVMI_ItemName).ToList();
                data.totalowStock = totallwstk.ToArray();
                //== Warranty Expire
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_TotalWarrantyExpire";
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
                        data.totalWexpire = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //===== Warranty Expired 
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_TotalWarrantyExpired";
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
                        data.totalWexpired = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                #endregion

                #region  Dashboard Grid             
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_DashboardGrid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@expiredays",
                   SqlDbType.BigInt)
                    {
                        Value = data.expiredays
                    });

                    cmd.Parameters.Add(new SqlParameter("@typeflg",
                     SqlDbType.VarChar)
                    {
                        Value = data.typeflg
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
                        data.dashboardgrid = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Inventory dashboard load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_DashboardDTO> getwarrantydetails(INV_DashboardDTO data)
        {
            try
            {                             
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_DashboardGrid";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@expiredays",
                   SqlDbType.BigInt)
                    {
                        Value = data.expiredays
                    });

                    cmd.Parameters.Add(new SqlParameter("@typeflg",
                     SqlDbType.VarChar)
                    {
                        Value = data.typeflg
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
                        data.warrantydetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
               
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Inventory Dashboard Warranty:" + ex.Message);
            }
            return data;
        }
    }
}
