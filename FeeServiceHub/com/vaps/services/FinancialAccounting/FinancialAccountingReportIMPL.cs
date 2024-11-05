using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee.FinancialAccounting;
using FeeServiceHub.com.vaps.interfaces.FinancialAccounting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services.FinancialAccounting
{
    public class FinancialAccountingReportIMPL : interfaces.FinancialAccounting.FinancialAccountingReportInterface

    {
        public FeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _db;


        readonly ILogger<FAMasterCompanyImpl> _logger;
        public FinancialAccountingReportIMPL(FeeGroupContext frgContext, DomainModelMsSqlServerContext db)
        {
            //_logger = log;
            _FeeGroupContext = frgContext;
            _db = db;
        }

        public FinancialAccountingReportDTO GetInitialData(FinancialAccountingReportDTO data)
        {

            try
            {


                data.fillcompany = _FeeGroupContext.FACompanyMasterDMO.Where(t => t.MI_Id == data.MI_Id && t.FAMCOMP_ActiveFlg == true).Distinct().ToArray();

                data.fillfinacialyear = _FeeGroupContext.IVRM_Master_FinancialYear.Distinct().ToArray();

            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return data;
        }
        public FinancialAccountingReportDTO getReport(FinancialAccountingReportDTO data)
        {

            try
            {
                if (data.type=="Balance sheet")
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FA_BalanceSheetNew";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt)
                        {
                            Value = data.IMFY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FAMCOMP_Id", SqlDbType.BigInt)
                        {
                            Value = data.FAMCOMP_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@sdate", SqlDbType.VarChar)
                        {
                            Value = data.Fromdate.Date.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@edate", SqlDbType.VarChar)
                        {
                            Value = data.Todate.Date.ToString("yyyy-MM-dd")
                        });                     

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader1.GetName(iFiled),
                                           dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.reportdetails = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.type == "Profit And Loss")
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FA_ProfitAndLoss";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt)
                        {
                            Value = data.IMFY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FAMCOMP_Id", SqlDbType.BigInt)
                        {
                            Value = data.FAMCOMP_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@sdate", SqlDbType.VarChar)
                        {
                            Value = data.Fromdate.Date.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@edate", SqlDbType.VarChar)
                        {
                            Value = data.Todate.Date.ToString("yyyy-MM-dd")
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader1.GetName(iFiled),
                                           dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.reportdetails = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.type == "Trail Balance")
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FA_TrailBalance";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt)
                        {
                            Value = data.IMFY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FAMCOMP_Id", SqlDbType.BigInt)
                        {
                            Value = data.FAMCOMP_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@sdate", SqlDbType.VarChar)
                        {
                            Value = data.Fromdate.Date.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@edate", SqlDbType.VarChar)
                        {
                            Value = data.Todate.Date.ToString("yyyy-MM-dd")
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader1.GetName(iFiled),
                                           dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.reportdetails = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FinancialAccountingReport";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FAMCOMP_Id", SqlDbType.BigInt)
                        {
                            Value = data.FAMCOMP_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime)
                        {
                            Value = data.Fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime)
                        {
                            Value = data.Todate
                        });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                        {
                            Value = data.type
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader1.GetName(iFiled),
                                           dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.reportdetails = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
               


            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return data;
        }
        public FinancialAccountingReportDTO subreport(FinancialAccountingReportDTO data)
                                                                                                                                                                        {

            try
            {

                if (data.reporttype == "daybookreport")
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FA_DAYBOOK";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FAMVOU_Id", SqlDbType.BigInt)
                        {
                            Value = data.FAMVOU_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader1.GetName(iFiled),
                                           dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.subreportdetails = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.reporttype == "groupwiseledgerreport")
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FA_LedgerDetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FAMGRP_Id", SqlDbType.BigInt)
                        {
                            Value = data.FAMGRP_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt)
                        {
                            Value = data.IMFY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime)
                        {
                            Value = data.Fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime)
                        {
                            Value = data.Todate
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader1.GetName(iFiled),
                                           dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.subreportdetails = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }                
                else if (data.reporttype == "Monthwisebalancereport")
                {
                    //Monthwisebalancedetails
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FA_Monthwisebalancedetails";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FAMLED_Id", SqlDbType.BigInt)
                        {
                            Value = data.FAMLED_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@IMFY_Id", SqlDbType.BigInt)
                        {
                            Value = data.IMFY_Id
                        });
                        //cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.DateTime)
                        //{
                        //    Value = data.Fromdate
                        //});
                        //cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.DateTime)
                        //{
                        //    Value = data.Todate
                        //});

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader1.GetName(iFiled),
                                           dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.subreportdetails = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else if (data.reporttype == "Ledgerwisereport")
                {
                    using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Fa_Ledgerwisedaybook";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FAMLED_Id", SqlDbType.BigInt)
                        {
                            Value = data.FAMLED_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Monthid", SqlDbType.BigInt)
                        {
                            Value = data.Monthid
                        });                     

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = cmd.ExecuteReader())
                            {
                                while (dataReader1.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader1.GetName(iFiled),
                                           dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.subreportdetails = retObject.ToArray();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }


            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return data;
        }



    }
}
