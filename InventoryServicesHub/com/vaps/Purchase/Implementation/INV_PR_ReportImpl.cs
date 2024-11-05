using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_PR_ReportImpl : Interface.INV_PR_ReportInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PR_ReportImpl> _logInv;
        public INV_PR_ReportImpl(InventoryContext InvContext, ILogger<INV_PR_ReportImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_PurchaseRequisitionDTO> getloaddata(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_PR_Details";
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
                        data.get_PRdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PR Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<INV_PurchaseRequisitionDTO> onreport(INV_PurchaseRequisitionDTO data)
        {
            try
            {
                string prids = "0";
                string invmids = "0";           
                if (data.optionflag == "PRno")
                {
                    if (data.prArray != null)
                    {
                        foreach (var g in data.prArray)
                        {
                            prids = prids + "," + g.INVMPR_Id;
                        }
                    }
                }
                else if (data.optionflag == "Item")
                {
                    if (data.itemArray != null)
                    {
                        foreach (var i in data.itemArray)
                        {
                            invmids = invmids + "," + i.INVMI_Id;
                        }
                    }
                }
              
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_PR_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@startdate",
                  SqlDbType.VarChar)
                    {
                        Value = data.startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@enddate",
                    SqlDbType.VarChar)
                    {
                        Value = data.enddate
                    });
                    cmd.Parameters.Add(new SqlParameter("@PR_Ids",
                   SqlDbType.VarChar)
                    {
                        Value = prids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
                  SqlDbType.VarChar)
                    {
                        Value = invmids
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
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
                        data.get_PRreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PR Report:" + ex.Message);
            }
            return data;
        }


    }
}
