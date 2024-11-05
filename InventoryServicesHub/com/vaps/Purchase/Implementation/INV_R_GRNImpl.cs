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

namespace InventoryServicesHub.com.vaps.Purchase.Implementation
{
    public class INV_R_GRNImpl : Interface.INV_R_GRNInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_R_GRNImpl> _logInv;
        public INV_R_GRNImpl(InventoryContext InvContext, ILogger<INV_R_GRNImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public async Task<INV_T_GRNDTO> getloaddata(INV_T_GRNDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_GRNno_ITEM_SUPPLIER_LIST";
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
                        data.get_grn_item_supplier = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("GRN Report Page load:" + ex.Message);
            }
            return data;
        }


        public async Task<INV_T_GRNDTO> onreport(INV_T_GRNDTO data)
        {
            try
            {
                if (data.optionflag == "Item" || data.optionflag == "Individual")
                {
                    string grnids = "0";
                    string invmids = "0";
                    string invmsids = "0";
                    if (data.optionflag == "Individual")
                    {
                        if (data.grnArray != null)
                        {
                            foreach (var g in data.grnArray)
                            {
                                grnids = grnids + "," + g.INVMGRN_Id;
                            }
                        }
                    }
                    else if (data.optionflag == "Item" )
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
                        cmd.CommandText = "INV_GRNReport_mob";
                        cmd.CommandType = CommandType.StoredProcedure;
                       
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        
                      
                        cmd.Parameters.Add(new SqlParameter("@GRN_Ids",
                       SqlDbType.VarChar)
                        {
                            Value = grnids
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
                        cmd.Parameters.Add(new SqlParameter("@typeflag",
                    SqlDbType.VarChar)
                        {
                            Value = data.typeflag
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
                            data.get_grnreport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
            }
                else
                {
                    
                        string grnids = "0";
                        string invmids = "0";
                        string invmsids = "0";
                        if (data.optionflag == "Individual_1")
                        {
                            if (data.grnArray != null)
                            {
                                foreach (var g in data.grnArray)
                                {
                                    grnids = grnids + "," + g.INVMGRN_Id;
                                }
                            }
                        }
                        else if (data.optionflag == "Item_1" || data.optionflag == "I")
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
                            if (data.supplierArray != null)
                            {
                                foreach (var s in data.supplierArray)
                                {
                                    invmsids = invmsids + "," + s.INVMS_Id;
                                }
                            }
                        }


                        using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "INV_GRNReport";
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
                            cmd.Parameters.Add(new SqlParameter("@GRN_Ids",
                           SqlDbType.VarChar)
                            {
                                Value = grnids
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
                            cmd.Parameters.Add(new SqlParameter("@typeflag",
                        SqlDbType.VarChar)
                            {
                                Value = data.typeflag
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
                                data.get_grnreport = retObject.ToArray();
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
                _logInv.LogInformation("Grn Report:" + ex.Message);
            }
            return data;
        }

        public INV_OpeningBalanceDTO getdata_ob(INV_OpeningBalanceDTO data)
        {
            try
            {
                data.year_list_ob = _INVContext.IVRM_Master_FinancialYear.ToArray();
                data.store_list_ob = _INVContext.INV_Master_StoreDMO.Where(a => a.MI_Id == data.MI_Id && a.INVMS_ActiveFlg==true).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public INV_OpeningBalanceDTO GetReport_ob(INV_OpeningBalanceDTO dto)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_OB_Report_proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt)
                    {
                        Value = dto.IMFY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@INVMST_Id", SqlDbType.BigInt)
                     {
                         Value = dto.INVMST_Id
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
                        dto.ob_report_list = retObject.ToArray();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

    }
}
