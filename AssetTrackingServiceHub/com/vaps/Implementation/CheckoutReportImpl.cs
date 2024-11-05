using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using DomainModel.Model.com.vapstech.AssetTracking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.AssetTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class CheckoutReportImpl : Interface.CheckoutReportInterface
    {
        public AssetTrackingContext _ATContext;
        ILogger<CheckoutReportImpl> _logAT;
        public CheckoutReportImpl(AssetTrackingContext ATContext, ILogger<CheckoutReportImpl> log)
        {
            _ATContext = ATContext;
            _logAT = log;
        }

        public CheckOutAssetsDTO getloaddata(CheckOutAssetsDTO data)
        {
            try
            {
                data.get_items = (from a in _ATContext.INV_Master_ItemDMO
                                  from b in _ATContext.INV_Asset_CheckOutDMO
                                  where (a.INVMI_Id == b.INVMI_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMI_ActiveFlg == true)
                                  select new CheckOutAssetsDTO
                                  {
                                      INVMI_Id = a.INVMI_Id,
                                      INVMI_ItemName = a.INVMI_ItemName,
                                  }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();

                data.get_locations = (from a in _ATContext.INV_Master_LocationDMO
                                      from b in _ATContext.INV_Asset_CheckOutDMO
                                      where (a.INVMLO_Id == b.INVMLO_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.INVMLO_ActiveFlg == true)
                                      select new CheckOutAssetsDTO
                                      {
                                          INVMLO_Id = a.INVMLO_Id,
                                          INVMLO_LocationRoomName = a.INVMLO_LocationRoomName,
                                      }).Distinct().OrderBy(m => m.INVMI_Id).ToArray();

                data.get_Financialyear = (from a in _ATContext.IVRM_Master_FinancialYear
                                         // from b in _ATContext.INV_StockDMO
                                         // where (
                                         // //a.IMFY_Id == b.IMFY_Id && 
                                         //// a.MI_Id == data.MI_Id
                                         // )
                                          select new CheckOutAssetsDTO
                                          {
                                              IMFY_Id = a.IMFY_Id,
                                              IMFY_FinancialYear = a.IMFY_FinancialYear,
                                              IMFY_AssessmentYear = a.IMFY_AssessmentYear,
                                              IMFY_FromDate = a.IMFY_FromDate,
                                              IMFY_ToDate = a.IMFY_ToDate,
                                              IMFY_OrderBy = a.IMFY_OrderBy
                                          }).Distinct().OrderByDescending(o => o.IMFY_OrderBy).ToArray();

                data.academicyearlist = _ATContext.AcademicYear.Where(e => e.MI_Id == data.MI_Id && e.Is_Active == true).OrderByDescending(e => e.ASMAY_Order).Distinct().ToArray();

                data.employeedropdown = (from a in _ATContext.MasterEmployee

                                         where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false)
                                         select new CheckOutAssetsDTO
                                         {
                                             HRME_Id = a.HRME_Id,
                                            
                                             employeename = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                         }).Distinct().OrderBy(m => m.HRME_Id).ToArray();

            }
            catch (Exception ex)
            {
                _logAT.LogInformation("Check-out Report load Page:" + ex.Message);
            }
            return data;
        }

        public async Task<CheckOutAssetsDTO> getreport(CheckOutAssetsDTO data)
        {
            try
            {
                string itemids = "0";
                string locationids = "0";
                string HRME_Ids = "0";
                if (data.selectionflag == "Item")
                {
                    if (data.itemarray != null)
                    {
                        foreach (var i in data.itemarray)
                        {
                            itemids = itemids + "," + i.INVMI_Id;
                        }
                    }
                }
                else if (data.selectionflag == "Location")
                {
                    if (data.locationarray != null)
                    {
                        foreach (var l in data.locationarray)
                        {
                            locationids = locationids + "," + l.INVMLO_Id;
                        }
                    }
                }
                else if (data.selectionflag == "Employee")
                {
                    if (data.emparray != null)
                    {
                        foreach (var l in data.emparray)
                        {
                            HRME_Ids = HRME_Ids + "," + l.HRME_Id;
                        }
                    }
                }
                using (var cmd = _ATContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "AT_CHECKOUT_REPORT_NEW";
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
                    cmd.Parameters.Add(new SqlParameter("@IMFY_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.IMFY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@selectionflag",
                  SqlDbType.VarChar)
                    {
                        Value = data.selectionflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMI_Id",
                SqlDbType.VarChar)
                    {
                        Value = itemids
                    });
                    cmd.Parameters.Add(new SqlParameter("@INVMLO_Id",
                                    SqlDbType.VarChar)
                    {
                        Value = locationids
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                    SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                                                     SqlDbType.VarChar)
                    {
                        Value = HRME_Ids
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
                        data.get_checkoutreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logAT.LogInformation("Check-out Report :" + ex.Message);
            }
            return data;
        }



    }
}
