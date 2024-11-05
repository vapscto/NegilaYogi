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
    public class INV_PO_ReportImpl : Interface.INV_PO_ReportInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_PO_ReportImpl> _logInv;
        public INV_PO_ReportImpl(InventoryContext InvContext, ILogger<INV_PO_ReportImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_PurchaseOrderDTO> getloaddata(INV_PurchaseOrderDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_PO_Details";
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
                        data.get_POdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("PO Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<INV_PurchaseOrderDTO> onreport(INV_PurchaseOrderDTO data)
        {
            try
            {
                if (data.optionflag == "PO"|| data.optionflag == "Itm")
                {
                    string poids = "0";
                    string invmids = "0";
                    string invmsids = "0";
                    if (data.optionflag == "PO")
                    {
                        if (data.poArray != null)
                        {
                            foreach (var g in data.poArray)
                            {
                                poids = poids + "," + g.INVMPO_Id;
                            }
                        }
                    }
                    else if (data.optionflag == "Itm")
                    {
                        if (data.poitemArray != null)
                        {
                            foreach (var i in data.poitemArray)
                            {
                                invmids = invmids + "," + i.INVMI_Id;
                            }
                        }
                    }
                    else if (data.optionflag == "Sup")
                    {
                        if (data.posuplierArray != null)
                        {
                            foreach (var i in data.posuplierArray)
                            {
                                invmsids = invmsids + "," + i.INVMS_Id;
                            }
                        }
                    }
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_PO_Report_mob";
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
                        cmd.Parameters.Add(new SqlParameter("@PO_Ids",
                       SqlDbType.VarChar)
                        {
                            Value = poids
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
                      SqlDbType.VarChar)
                        {
                            Value = invmids
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMS_Ids",
                    SqlDbType.VarChar)
                        {
                            Value = invmsids
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
                            using (var dataReader =  cmd.ExecuteReader())
                            {
                                while ( dataReader.Read())
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
                            data.get_POreport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    
                }
                else
                {
                    string poids = "0";
                    string invmids = "0";
                    string invmsids = "0";
                    if (data.optionflag == "PONo")
                    {
                        if (data.poArray != null)
                        {
                            foreach (var g in data.poArray)
                            {
                                poids = poids + "," + g.INVMPO_Id;
                            }
                        }
                    }
                    else if (data.optionflag == "Item")
                    {
                        if (data.poitemArray != null)
                        {
                            foreach (var i in data.poitemArray)
                            {
                                invmids = invmids + "," + i.INVMI_Id;
                            }
                        }
                    }
                    else if (data.optionflag == "Supplier")
                    {
                        if (data.posuplierArray != null)
                        {
                            foreach (var i in data.posuplierArray)
                            {
                                invmsids = invmsids + "," + i.INVMS_Id;
                            }
                        }
                    }

                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_PO_Report";
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
                        cmd.Parameters.Add(new SqlParameter("@PO_Ids",
                       SqlDbType.VarChar)
                        {
                            Value = poids
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
                      SqlDbType.VarChar)
                        {
                            Value = invmids
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMS_Ids",
                    SqlDbType.VarChar)
                        {
                            Value = invmsids
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
                            data.get_POreport = retObject.ToArray();
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
                _logInv.LogInformation("PI Report:" + ex.Message);
            }
            return data;
        }


    }
}
