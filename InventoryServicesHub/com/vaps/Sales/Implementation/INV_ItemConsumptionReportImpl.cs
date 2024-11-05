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
    public class INV_ItemConsumptionReportImpl : Interface.INV_ItemConsumptionReportInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_ItemConsumptionReportImpl> _logInv;
        public INV_ItemConsumptionReportImpl(InventoryContext InvContext, ILogger<INV_ItemConsumptionReportImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_ItemConsumptionDTO> getloaddata(INV_ItemConsumptionDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_IC_Report_Details";
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
                        data.get_ICreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("IC Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<INV_ItemConsumptionDTO> onreport(INV_ItemConsumptionDTO data)
        {
            try
            {
                string hrmeids = "0";
                if (data.optionflag == "Staff")
                {
                    if (data.staffarray != null)
                    {
                        foreach (var e in data.staffarray)
                        {
                            hrmeids = hrmeids + "," + e.HRME_Id;
                        }
                    }
                }
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_ItemConsumption_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@typeflag",
                    SqlDbType.VarChar)
                    {
                        Value = data.typeflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@optionflag",
                    SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.INVMI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                   SqlDbType.VarChar)
                    {
                        Value = hrmeids
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRMD_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.HRMD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
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
                        data.get_ICReport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("IC Report:" + ex.Message);
            }
            return data;
        }


    }
}
