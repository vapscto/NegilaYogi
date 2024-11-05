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
    public class AssetTagDispose_ReportImpl : Interface.AssetTagDispose_ReportInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<AssetTagDispose_ReportImpl> _ATINV;
        public AssetTagDispose_ReportImpl(AssetTrackingContext ATContext, ILogger<AssetTagDispose_ReportImpl> log)
        {
            _ATContext = ATContext;
            _ATINV = log;
        }

        public async Task<AssetTagDisposeDTO> getloaddata(AssetTagDisposeDTO data)
        {
            try
            {
                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTagDispose_Details";
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
                        data.get_disposedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _ATINV.LogInformation("Asset tag Dispose Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<AssetTagDisposeDTO> onreport(AssetTagDisposeDTO data)
        {
            try
            {
                string invmids = "0";
                string invmstids = "0";
                string invmloids = "0";
                string invaatids = "0";
                if (data.optionflag == "Item")
                {
                    if (data.disposeItemArray != null)
                    {
                        foreach (var i in data.disposeItemArray)
                        {
                            invmids = invmids + "," + i.INVMI_Id;
                        }
                    }
                }
                else if (data.optionflag == "Store")
                {
                    if (data.disposeStoreArray != null)
                    {
                        foreach (var i in data.disposeStoreArray)
                        {
                            invmstids = invmstids + "," + i.INVMST_Id;
                        }
                    }
                }
                else if (data.optionflag == "Location")
                {
                    if (data.disposeLocationArray != null)
                    {
                        foreach (var i in data.disposeLocationArray)
                        {
                            invmloids = invmloids + "," + i.INVMLO_Id;
                        }
                    }
                }
                else if (data.optionflag == "Tag")
                {
                    if (data.disposeTagArray != null)
                    {
                        foreach (var i in data.disposeTagArray)
                        {
                            invaatids = invaatids + "," + i.INVAAT_Id;
                        }
                    }
                }

                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AssetTagDispose_Report";
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
                        data.get_disposereport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _ATINV.LogInformation("Asset Tag Dispose Report:" + ex.Message);
            }
            return data;
        }


    }
}
