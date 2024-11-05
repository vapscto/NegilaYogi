using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
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

namespace InventoryServicesHub.com.vaps.Master.Implementation
{
    public class INV_StockSummaryIMPL : Interface.INV_StockSummaryInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_StockSummaryIMPL> _logInv;
        public DomainModelMsSqlServerContext _db;
        public INV_StockSummaryIMPL(InventoryContext InvContext, ILogger<INV_StockSummaryIMPL> log, DomainModelMsSqlServerContext MsSqlServerContext)
        {
            _INVContext = InvContext;
            _logInv = log;
            _db = MsSqlServerContext;
        }

        public async Task<INV_StockSummaryDTO> getloaddata(INV_StockSummaryDTO data)
        {
            try
            {
                if(data.optionflag !="")
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
                            data.get_report = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
               
                data.Select_list = (from a in _db.School_M_Class
                                    from b in _db.Masterclasscategory
                                    where (a.MI_Id == data.MI_Id && b.Is_Active == true && a.ASMCL_Id == b.ASMCL_Id)
                                    select a).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                data.yearlist = _db.AcademicYear.Where(y => y.MI_Id == data.MI_Id).OrderByDescending(d => d.ASMAY_Order).ToArray();



            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Item Report Page load:" + ex.Message);
            }
            return data;
        }
        //getstudent
        public async Task<INV_StockSummaryDTO> getstudent(INV_StockSummaryDTO data)
        {
            try
            {
                List<long> ASMS_Ids = new List<long>();
                if (data.sections != null)
                {
                    foreach(var i in data.sections)
                    {
                        ASMS_Ids.Add(i.ASMS_Id);
                    }
                   
                }
                data.getstudent = (from a in _db.School_Adm_Y_StudentDMO
                                   from b in _db.Adm_M_Student
                                   where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && ASMS_Ids.Contains(a.ASMS_Id)
                                   && b.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.MI_Id == data.MI_Id)
                                   select new INV_StockSummaryDTO
                                   {
                                       AMST_Id=a.AMST_Id,
                                       studentname = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) ? "" : ' ' + b.AMST_LastName)
                                   }
                                 ).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Bill wise Report Page load:" + ex.Message);
            }
            return data;
        }
        public async Task<INV_StockSummaryDTO> load_onchange(INV_StockSummaryDTO dto)
        {
            try
            {
                dto.Section_list = (from a in _db.AdmSchoolMasterClassCatSec
                                    from b in _db.Masterclasscategory
                                    from c in _db.School_M_Section
                                    where (a.ASMCC_Id == b.ASMCC_Id && a.ASMS_Id == c.ASMS_Id
                                    && b.ASMCL_Id == dto.ASMCL_Id
                                    && a.ASMCCS_ActiveFlg == true)
                                    select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

            return dto;
        }
        public async Task<INV_StockSummaryDTO> onreport(INV_StockSummaryDTO data)
        {
            try
            {
                string startdate = "";
                string enddate = "";
                data.logopath = _db.Institution.Where(R => R.MI_Id == data.MI_Id && R.MI_ActiveFlag == 1).Select(p => p.MI_Logo).FirstOrDefault();
                if (data.startdate != null && data.enddate != null)
                {
                    startdate = data.startdate.Value.ToString("yyyy-MM-dd");
                    enddate = data.enddate.Value.Date.ToString("yyyy-MM-dd");
                }
                string itemids = "0";
                if (data.optionflag == "Item")
                {
                    if (data.itemsArray != null)
                    {
                        foreach (var i in data.itemsArray)
                        {
                            itemids = itemids + "," + i.INVMI_Id;
                        }
                    }
                }

                string INVMST_Ids = "0";
                if (data.optionflag == "Store")
                {
                    if (data.itemsArray != null)
                    {
                        foreach (var i in data.itemsArray)
                        {
                            INVMST_Ids = INVMST_Ids + "," + i.INVMST_Id;
                        }
                    }
                }
                string INVMG_Id = "0";
                if (data.optionflag == "Group")
                {
                    if (data.itemsArray != null)
                    {
                        foreach (var i in data.itemsArray)
                        {
                            INVMG_Id = INVMG_Id + "," + i.INVMG_Id;
                        }
                    }
                }

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_StockSummary_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_FromDate",
                  SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@IMFY_ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = enddate
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
                  SqlDbType.VarChar)
                    {
                        Value = itemids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMST_Ids",
                SqlDbType.VarChar)
                    {
                        Value = INVMST_Ids
                    });
                    //INVMST_Ids
                    cmd.Parameters.Add(new SqlParameter("@optionflag",
                  SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMG_Id",
            SqlDbType.VarChar)
                    {
                        Value = INVMG_Id
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
                        data.stock_summaryreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Item Report:" + ex.Message);
            }
            return data;
        }
        //onreporttwo
        public async Task<INV_StockSummaryDTO> onreporttwo(INV_StockSummaryDTO data)
        {
            try
            {
                string startdate = "";
                string enddate = "";
                data.logopath = _db.Institution.Where(R => R.MI_Id == data.MI_Id && R.MI_ActiveFlag == 1).Select(p => p.MI_Logo).FirstOrDefault();
                if (data.startdate != null && data.enddate != null)
                {
                    startdate = data.startdate.Value.Date.ToString("yyyy-MM-dd");
                    enddate = data.enddate.Value.Date.ToString("yyyy-MM-dd");
                }
                string itemids = "0";
                if (data.optionflag == "Item")
                {
                    if (data.itemsArray != null)
                    {
                        foreach (var i in data.itemsArray)
                        {
                            itemids = itemids + "," + i.INVMI_Id;
                        }
                    }
                }



                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_StockRegister_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@startdate",
                  SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@enddate",
                    SqlDbType.VarChar)
                    {
                        Value = enddate
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Ids",
                  SqlDbType.VarChar)
                    {
                        Value = itemids
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
                        data.stock_summaryreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Stock Register Report:" + ex.Message);
            }
            return data;
        }

        //onreportthree
        public async Task<INV_StockSummaryDTO> onreportthree(INV_StockSummaryDTO data)
        {
            try
            {
                string startdate = "";
                string enddate = "";
                data.logopath = _db.Institution.Where(R => R.MI_Id == data.MI_Id && R.MI_ActiveFlag == 1).Select(p => p.MI_Logo).FirstOrDefault();
                if (data.startdate != null && data.enddate != null)
                {
                    startdate = data.startdate.Value.ToString("yyyy-MM-dd");
                    enddate = data.enddate.Value.Date.ToString("yyyy-MM-dd");
                }
                string AMST_Ids = "0";
                
                if (data.itemsArray != null)
                {
                    foreach (var i in data.itemsArray)
                    {
                        AMST_Ids = AMST_Ids + "," + i.AMST_Id;
                    }
                }
             
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PDA_BillForMonth_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
               SqlDbType.VarChar)
                    {
                        Value = enddate
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Ids",
               SqlDbType.VarChar)
                    {
                        Value = AMST_Ids
                    });

                    cmd.Parameters.Add(new SqlParameter("@optionflag",
                  SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });
                    //

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
                        data.stock_summaryreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Stock Register Report:" + ex.Message);
            }
            return data;
        }

        //INV Bill 
        public async Task<INV_StockSummaryDTO> onreportstock(INV_StockSummaryDTO data)
        {
            try
            {
                string startdate = "";
                string enddate = "";
                if (data.startdate != null && data.enddate != null)
                {
                    startdate = data.startdate.Value.Date.ToString("yyyy-MM-dd");
                    enddate = data.enddate.Value.Date.ToString("yyyy-MM-dd");
                }

                string AMST_Ids = "0";
                if (data.itemsArray != null)
                {
                    foreach (var i in data.itemsArray)
                    {
                        AMST_Ids = AMST_Ids + "," + i.AMST_Id;
                    }
                }

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_BillForMonth_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });                    
                    cmd.Parameters.Add(new SqlParameter("@optionflag",
                  SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });
               //     cmd.Parameters.Add(new SqlParameter("@AMST_Id",
               //SqlDbType.VarChar)
               //     {
               //         Value = AMST_Ids
               //     });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
               SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Startdate",
                  SqlDbType.VarChar)
                    {
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@Enddate",
                    SqlDbType.VarChar)
                    {
                        Value = enddate
                    });

                    //
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
                        data.invstockreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Stock Register Report:" + ex.Message);
            }
            return data;
        }
    }
}
