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
    public class CLG_INV_SalesReportImpl : Interface.CLG_INV_SalesReportInterface
    {
        public InventoryContext _INVContext;
        ILogger<CLG_INV_SalesReportImpl> _logInv;
        public CLG_INV_SalesReportImpl(InventoryContext InvContext, ILogger<CLG_INV_SalesReportImpl> log)
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
        public INV_T_SalesDTO getbranchlist(INV_T_SalesDTO data)
        {
            try
            {
                data.branch_list = (from a in _INVContext.CLG_Adm_College_AY_CourseDMO
                                    from b in _INVContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from c in _INVContext.AcademicYear
                                    from d in _INVContext.ClgMasterBranchDMO
                                    from e in _INVContext.INV_M_Sales_College_StudentDMO
                                    where (a.ACAYC_Id == b.ACAYC_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMB_Id == d.AMB_Id && a.AMCO_Id == e.AMCO_Id && b.AMB_Id == e.AMB_Id
                                    && d.AMB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id)
                                    select new INV_T_SalesDTO
                                    {
                                        AMB_Id = d.AMB_Id,
                                        AMB_BranchName = d.AMB_BranchName,
                                        AMB_BranchCode = d.AMB_BranchCode,
                                        AMB_ActiveFlag = d.AMB_ActiveFlag,
                                        AMB_Order = d.AMB_Order
                                    }
                      ).Distinct().OrderBy(c => c.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("getbranchlist :" + ex.Message);
            }
            return data;
        }

        public INV_T_SalesDTO getsemesterlist(INV_T_SalesDTO data)
        {
            try
            {
                data.sem_list = (from a in _INVContext.CLG_Adm_College_AY_CourseDMO
                                 from b in _INVContext.CLG_Adm_College_AY_Course_BranchDMO
                                 from c in _INVContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                 from d in _INVContext.AcademicYear
                                 from e in _INVContext.CLG_Adm_Master_SemesterDMO
                                 from f in _INVContext.INV_M_Sales_College_StudentDMO
                                 where (a.ACAYC_Id == b.ACAYC_Id && b.ACAYCB_Id == c.ACAYCB_Id && a.MI_Id == c.MI_Id && a.ASMAY_Id == d.ASMAY_Id && c.AMSE_Id == e.AMSE_Id && a.AMCO_Id == f.AMCO_Id && b.AMB_Id == f.AMB_Id && c.AMSE_Id == f.AMSE_Id
                                 && e.AMSE_ActiveFlg == true && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.AMB_Id == data.AMB_Id)
                                 select new INV_T_SalesDTO
                                 {
                                     AMSE_Id = e.AMSE_Id,
                                     AMSE_SEMName = e.AMSE_SEMName,
                                     AMSE_SEMCode = e.AMSE_SEMCode,
                                     AMSE_SEMOrder = e.AMSE_SEMOrder
                                 }
                            ).Distinct().OrderBy(c => c.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("semester List:" + ex.Message);
            }
            return data;
        }
        public async Task<INV_T_SalesDTO> getStudentlist(INV_T_SalesDTO data)
        {
            try
            {
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_CLG_STUDENTLIST";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
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
                string amcstids = "0";
                string invmids = "0";
                string invmslids = "0";
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
                    if (data.clgstuarray != null)
                    {
                        foreach (var cs in data.clgstuarray)
                        {
                            amcstids = amcstids + "," + cs.AMCST_Id;
                        }
                    }
                }

                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_CLG_SALESReport";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                   SqlDbType.VarChar)
                    {
                        Value = amcstids
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
