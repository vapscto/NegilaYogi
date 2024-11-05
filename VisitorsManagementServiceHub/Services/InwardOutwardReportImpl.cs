using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using VisitorsManagementServiceHub.Interfaces;

namespace VisitorsManagementServiceHub.Services
{
    public class InwardOutwardReportImpl : InwardOutwardReportInterface
    {
        public VisitorsManagementContext visctxt;
        public InwardOutwardReportImpl(VisitorsManagementContext context)
        {
            visctxt = context;
        }
        public async Task<InwardOutwardReportDTO> report(InwardOutwardReportDTO data)
        {

            try
            {

                #region

                //if (data.radiotype == "inward")
                //{
                //    var listdata = (from a in visctxt.FO_Inward_DMO
                //                    from b in visctxt.MasterEmployee
                //                    where (a.MI_Id == b.MI_Id && a.FOIN_To == b.HRME_Id && a.MI_Id == data.MI_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false)
                //                    select new InwardDTO
                //                    {
                //                        FOIN_Id = a.FOIN_Id,
                //                        MI_Id = a.MI_Id,
                //                        FOIN_InwardNo = a.FOIN_InwardNo,
                //                        FOIN_DateTime = a.FOIN_DateTime,
                //                        FOIN_From = a.FOIN_From,
                //                        FOIN_Adddress = a.FOIN_Adddress,
                //                        FOIN_ContactPerson = a.FOIN_ContactPerson,
                //                        FOIN_PhoneNo = a.FOIN_PhoneNo,
                //                        FOIN_EmailId = a.FOIN_EmailId,
                //                        FOIN_Discription = a.FOIN_Discription,
                //                        FOIN_To = a.FOIN_To,
                //                        FOIN_ReceivedBy = a.FOIN_ReceivedBy,
                //                        FOIN_HandedOverTo = a.FOIN_HandedOverTo,
                //                        FOIN_ActiveFlag = a.FOIN_ActiveFlag,
                //                        HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) ? "" : ' ' + b.HRME_EmployeeLastName),
                //                        HRME_Id = b.HRME_Id,

                //                        firstName1 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_ReceivedBy).FirstOrDefault().HRME_EmployeeFirstName),
                //                        firstName2 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_ReceivedBy).FirstOrDefault().HRME_EmployeeMiddleName),
                //                        firstName3 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_ReceivedBy).FirstOrDefault().HRME_EmployeeLastName),

                //                        secnam1 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_HandedOverTo).FirstOrDefault().HRME_EmployeeFirstName),
                //                        secnam2 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_HandedOverTo).FirstOrDefault().HRME_EmployeeMiddleName),
                //                        secnam3 = (visctxt.MasterEmployee.Where(t => t.MI_Id == data.MI_Id && t.HRME_Id == a.FOIN_HandedOverTo).FirstOrDefault().HRME_EmployeeLastName),


                //                    }).Distinct().OrderBy(t => t.FOIN_Id).ToList();

                //    data.viewlist = listdata.ToArray();
                //    //data.viewlist = visctxt.FO_Inward_DMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                //}
                //else if (data.radiotype == "outward")
                //{
                //    var listdata= (from a in visctxt.FO_Outward_DMO
                //                    from t in visctxt.MasterEmployee
                //                    where (a.MI_Id == t.MI_Id && a.FOOUT_DispatachedBy == t.HRME_Id && a.MI_Id == data.MI_Id)
                //                    select new OutwardDTO
                //                    {

                //                        HRME_EmployeeFirstName = t.HRME_EmployeeFirstName + (string.IsNullOrEmpty(t.HRME_EmployeeMiddleName) ? "" : ' ' + t.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(t.HRME_EmployeeLastName) ? "" : ' ' + t.HRME_EmployeeLastName),
                //                        HRME_Id = t.HRME_Id,
                //                        FOOUT_Id = a.FOOUT_Id,
                //                        FOOUT_OutwardNo = a.FOOUT_OutwardNo,
                //                        FOOUT_DateTime = a.FOOUT_DateTime,
                //                        FOOUT_Discription = a.FOOUT_Discription,
                //                        FOOUT_From = a.FOOUT_From,
                //                        FOOUT_To = a.FOOUT_To,
                //                        FOOUT_Address = a.FOOUT_Address,
                //                        FOOUT_PhoneNo = a.FOOUT_PhoneNo,
                //                        FOOUT_EmailId = a.FOOUT_EmailId,
                //                        FOOUT_DispatachedBy = a.FOOUT_DispatachedBy,
                //                        FOOUT_DispatchedThrough = a.FOOUT_DispatchedThrough,
                //                        FOOUT_DispatchedDeatils = a.FOOUT_DispatchedDeatils,
                //                        FOOUT_DispatchedPhNo = a.FOOUT_DispatchedPhNo,
                //                        FOOUT_ActiveFlag = a.FOOUT_ActiveFlag,

                //                    }).Distinct().OrderBy(t => t.FOOUT_Id).ToList();

                //        data.viewlist = listdata.ToArray();



                //    //data.viewlist = visctxt.FO_Outward_DMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                //}
                #endregion



                if (data.all1 == "1")
                {
                    data.month_id = "";
                }
                else
                {
                    data.fromdate = "";
                    data.todate="";
                }


                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Inward_Outward_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@radiotype",
                   SqlDbType.VarChar)
                    {
                        Value = data.radiotype
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@months",
                 SqlDbType.VarChar)
                    {
                        Value = data.month_id
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
                        data.viewlist = retObject.ToArray();

                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        #region old Code
        //public InwardOutwardReportDTO report(InwardOutwardReportDTO data)
        //{

        //    try
        //    {
        //        if (data.radiotype == "inward")
        //        {
        //            data.viewlist = visctxt.InwardDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
        //        }
        //        else if (data.radiotype == "outward")
        //        {
        //            data.viewlist = visctxt.OutwardDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
        //        }

        //    }

        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }
        //    return data;
        //}
        #endregion
    }
}
