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
    public class INV_R_SalesImpl : Interface.INV_R_SalesInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_R_SalesImpl> _logInv;
        public INV_R_SalesImpl(InventoryContext InvContext, ILogger<INV_R_SalesImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_T_SalesDTO getloaddata(INV_T_SalesDTO data)
        {
            try
            {
                //  data.get_salesno = _INVContext.INV_M_SalesDMO.Where(a => a.INVMSL_ActiveFlg == true && a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sales Report Page load:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_T_SalesDTO> mainradiochange(INV_T_SalesDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_SaleReport_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = data.type
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
                        data.get_salesdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sale Report get_StudentClsSec :" + ex.Message);
            }
            return data;
        }
        public INV_T_SalesDTO radiochange(INV_T_SalesDTO data)
        {
            try
            {
                data.get_class = (from a in _INVContext.Adm_M_Student
                                  from b in _INVContext.INV_M_SalesDMO
                                  from c in _INVContext.INV_M_Sales_StudentDMO
                                  from d in _INVContext.School_Adm_Y_StudentDMO
                                  from e in _INVContext.School_M_Class
                                  where (a.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && b.INVMSL_Id == c.INVMSL_Id && d.ASMCL_Id == e.ASMCL_Id && b.MI_Id == data.MI_Id)
                                  select new INV_T_SalesDTO
                                  {
                                      ASMCL_Id = d.ASMCL_Id,
                                      ASMCL_ClassName = e.ASMCL_ClassName,
                                      ASMCL_Order = e.ASMCL_Order
                                  }).Distinct().OrderBy(e => e.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sale Report get_StudentClsSec :" + ex.Message);
            }
            return data;
        }
        public async Task<INV_T_SalesDTO> getStudentlist(INV_T_SalesDTO data)
        {
            try
            {
                if (data.type == "C")
                {
                    using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_SalesReport_Studentlist";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                      SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                        {
                            Value = data.type
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
                            data.get_Studentlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                else
                {
                    data.get_Section = (from a in _INVContext.Adm_M_Student
                                        from b in _INVContext.INV_M_SalesDMO
                                        from c in _INVContext.INV_M_Sales_StudentDMO
                                        from d in _INVContext.School_Adm_Y_StudentDMO
                                        from e in _INVContext.School_M_Class
                                        from f in _INVContext.School_M_Section
                                        where (a.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id && b.INVMSL_Id == c.INVMSL_Id && d.ASMCL_Id == e.ASMCL_Id && d.ASMS_Id == f.ASMS_Id && b.MI_Id == data.MI_Id && d.ASMCL_Id == data.ASMCL_Id)
                                        select new INV_T_SalesDTO
                                        {
                                            ASMS_Id = d.ASMS_Id,
                                            ASMC_SectionName = f.ASMC_SectionName,
                                            ASMC_Order = f.ASMC_Order
                                        }).Distinct().OrderBy(e => e.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sales Report get_Studentlist :" + ex.Message);
            }
            return data;
        }
        public async Task<INV_T_SalesDTO> onreport(INV_T_SalesDTO data)
        {
            try
            {
                string amstids = "0";
                string invmids = "0";
                string invmslids = "0";
                string invmst_ids = "0";
                //if (data.optionflag == "Store")
                //{
                //    if (data.storenoarray != null)
                //    {
                //        foreach (var str in data.storenoarray)
                //        {
                //            invmst_ids = invmst_ids + "," + str.invmst_id;
                //        }
                //    }
                //}
                if (data.optionflag == "Item")
                {
                    if (data.itemarray != null) 
                    {
                        foreach (var i in data.itemarray)
                        {
                            invmids = invmids + "," + i.invmi_id;
                        }
                    }
                }
                else if (data.optionflag == "Saleno")
                {

                    if (data.salenoarray != null)
                    {
                        foreach (var s in data.salenoarray)
                        {
                            invmslids = invmslids + "," + s.invmsl_id;
                        }
                    }
                }
                else if (data.optionflag == "Student")
                {
                    if (data.selectionflag == "C")
                    {
                        if (data.clsarray != null)
                        {
                            foreach (var c in data.clsarray)
                            {
                                amstids = amstids + "," + c.amst_id;
                            }
                        }
                    }
                    else if (data.selectionflag == "CS")
                    {
                        if (data.secarray != null)
                        {
                            foreach (var cs in data.secarray)
                            {
                                amstids = amstids + "," + cs.amst_id;
                            }
                        }
                    }
                    else
                    {
                        amstids = data.studentid;
                    }
                }

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_SALESReport_OD";
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

                    cmd.Parameters.Add(new SqlParameter("@INVMSL_Id",
                         SqlDbType.VarChar)
                    {
                        Value = invmslids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = invmids
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                   SqlDbType.VarChar)
                    {
                        Value = amstids
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.hrme_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMC_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.invmc_id
                    });
                    //cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    //  SqlDbType.BigInt)
                    //{
                    //    Value = data.ASMAY_Id
                    //});
                    //ASMAY_Id
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
                        data.get_SalesReport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Sales Report:" + ex.Message);
            }
            return data;
        }


    }
}
