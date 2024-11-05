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
    public class INV_Quotation_ReportImpl : Interface.INV_Quotation_ReportInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_Quotation_ReportImpl> _logInv;
        public INV_Quotation_ReportImpl(InventoryContext InvContext, ILogger<INV_Quotation_ReportImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_QuotationDTO> getloaddata(INV_QuotationDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_Quotation_Details";
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
                        data.get_Quotedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<INV_QuotationDTO> onreport(INV_QuotationDTO data)
        {
            try
            {
                string qtids = "0";
                string piids = "0";
                string invmids = "0";
                string invmsids = "0";
                if (data.optionflag == "QuoteNo")
                {
                    if (data.quoteArray != null)
                    {
                        foreach (var g in data.quoteArray)
                        {
                            qtids = qtids + "," + g.INVMSQ_Id;
                        }
                    }
                }
                else if (data.optionflag == "PINo")
                {
                    if (data.pIArray != null)
                    {
                        foreach (var g in data.pIArray)
                        {
                            piids = piids + "," + g.INVMPI_Id;
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
                else if (data.optionflag == "Supplier")
                {
                    if (data.suplierArray != null)
                    {
                        foreach (var i in data.suplierArray)
                        {
                            qtids = qtids + "," + i.INVMSQ_Id;
                        }
                    }
                }

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_Quotation_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@INVMSQ_Ids",
                  SqlDbType.VarChar)
                    {
                        Value = qtids
                    });
                    cmd.Parameters.Add(new SqlParameter("@PI_Ids",
                   SqlDbType.VarChar)
                    {
                        Value = piids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
                  SqlDbType.VarChar)
                    {
                        Value = invmids
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
                        data.get_Quotationreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PI Report:" + ex.Message);
            }
            return data;
        }


    }
}
