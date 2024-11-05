using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.AssetTag.Implementation
{
    public class AssetTagCheckIn_ReportImpl : Interface.AssetTagCheckIn_ReportInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetTagCheckIn_ReportImpl> _ATINV;
        public AssetTagCheckIn_ReportImpl(AssetTrackingContext ATContext, ILogger<AssetTagCheckIn_ReportImpl> log)
        {
            _ATContext = ATContext;
            _ATINV = log;
        }

        public async Task<AssetTagCheckInDTO> getloaddata(AssetTagCheckInDTO data)
        {
            try
            {
                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTagCheckIn_Details";
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
                        data.get_ckIndetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _ATINV.LogInformation("Asset tag Check IN Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<AssetTagCheckInDTO> onreport(AssetTagCheckInDTO data)
        {
            try
            {
                string invmids = "0";
                string invmstids = "0";
                string invmloids = "0";
                string invaatids = "0";
                if (data.optionflag == "Item")
                {
                    if (data.ciItemArray != null)
                    {
                        foreach (var i in data.ciItemArray)
                        {
                            invmids = invmids + "," + i.INVMI_Id;
                        }
                    }
                }
                else if (data.optionflag == "Store")
                {
                    if (data.ciStoreArray != null)
                    {
                        foreach (var i in data.ciStoreArray)
                        {
                            invmstids = invmstids + "," + i.INVMST_Id;
                        }
                    }
                }
                else if (data.optionflag == "Location")
                {
                    if (data.ciLocationArray != null)
                    {
                        foreach (var i in data.ciLocationArray)
                        {
                            invmloids = invmloids + "," + i.INVMLO_Id;
                        }
                    }
                }
                else if (data.optionflag == "Tag")
                {
                    if (data.ciTagArray != null)
                    {
                        foreach (var i in data.ciTagArray)
                        {
                            invaatids = invaatids + "," + i.INVAAT_Id;
                        }
                    }
                }

                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTagCheckIn_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@INVAAT_Ids",
                  SqlDbType.VarChar)
                    {
                        Value = invaatids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
                  SqlDbType.VarChar)
                    {
                        Value = invmids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Ids",
                SqlDbType.VarChar)
                    {
                        Value = invmstids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMLO_Ids",
                SqlDbType.VarChar)
                    {
                        Value = invmloids
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
                        data.get_ckInreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _ATINV.LogInformation("Asset Tag Check-In Report:" + ex.Message);
            }
            return data;
        }


    }
}
