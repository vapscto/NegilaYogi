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
    public class INV_R_StockImpl : Interface.INV_R_StockInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_R_StockImpl> _logInv;
        public INV_R_StockImpl(InventoryContext InvContext, ILogger<INV_R_StockImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_StockDTO> getloaddata(INV_StockDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_Stock_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@optionflag",
           SqlDbType.VarChar)
                    {
                        Value = data.optionflag
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
                        data.get_stockdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Stock Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<INV_StockDTO> onreport(INV_StockDTO data)
        {
            try
            {               
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_FY_StockReport";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_FromDate",
                  SqlDbType.VarChar)
                    {
                        Value = data.startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.enddate
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
                   SqlDbType.VarChar)
                    {
                        Value = data.INVMI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Ids",
                SqlDbType.VarChar)
                    {
                        Value = data.INVMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMG_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.INVMG_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@optionflag",
                  SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });
                       cmd.Parameters.Add(new SqlParameter("@overallflag",
                  SqlDbType.VarChar)
                    {
                        Value = data.overallflag
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
                        data.get_StockReport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Stock Report:" + ex.Message);
            }
            return data;
        }


    }
}
