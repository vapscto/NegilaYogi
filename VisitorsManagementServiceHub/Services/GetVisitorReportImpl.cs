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
    public class GetVisitorReportImpl : GetVisitorReportInterface
    {
        public VisitorsManagementContext visctxt;

        public GetVisitorReportImpl(VisitorsManagementContext context)
        {
            visctxt = context;
        }
        public async Task<GetVisitorReportDTO> report(GetVisitorReportDTO data)
        {

            try
            {
                if ((data.all1=="1") || (data.all1 == "0"))
                {

                    if (data.all1 == "1")
                    {
                        data.month_id = "";
                    }
                    else
                    {
                        data.fromdate = "";
                        data.todate = "";
                    }


                    using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Vistors_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
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
                        cmd.Parameters.Add(new SqlParameter("@column",
                    SqlDbType.VarChar)
                        {
                            Value = data.searchby
                        });
                        cmd.Parameters.Add(new SqlParameter("@value",
                    SqlDbType.VarChar)
                        {
                            Value = data.txtbox
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
                else
                {
                    List<GetVisitorReportDTO> result = new List<GetVisitorReportDTO>();

                    using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "visitor_details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@column", SqlDbType.VarChar) { Value = data.searchby });
                        cmd.Parameters.Add(new SqlParameter("@value", SqlDbType.VarChar) { Value = data.txtbox });
                        //  cmd.Parameters.Add(new SqlParameter("@datevisit", SqlDbType.DateTime) { Value = data.Date_vist });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new GetVisitorReportDTO
                                    {
                                        AMVM_Name = dataReader["VMMV_VisitorName"].ToString(),
                                        AMVM_Contact_No = dataReader["VMMV_VisitorContactNo"].ToString(),
                                        AMVM_Emailid = dataReader["VMMV_VisitorEmailid"].ToString(),
                                        Date_Visit = Convert.ToDateTime(dataReader["VMMV_MeetingDateTime"].ToString()),
                                        Time_Visit = dataReader["VMMV_EntryDateTime"].ToString(),
                                        AMVM_Out_Time = dataReader["VMMV_ExitDateTime"].ToString(),
                                        AMVM_Status = dataReader["VMMV_CkeckedInOutStatus"].ToString(),
                                        AMVM_Purpose = dataReader["VMMV_MeetingPurpose"].ToString(),
                                    });
                                    data.viewlist = result.ToArray();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

             
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        #region Old Code
        //public GetVisitorReportDTO report(GetVisitorReportDTO data)
        //{

        //    try
        //    {
        //        // data.viewlist = visctxt.AddVisitorsDMO.Where(d => d.MI_Id == data.MI_Id).ToArray();

        //        List<GetVisitorReportDTO> result = new List<GetVisitorReportDTO>();

        //        using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
        //        {
        //            cmd.CommandText = "visitor_details";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.Int) { Value = data.MI_Id });
        //            cmd.Parameters.Add(new SqlParameter("@column", SqlDbType.VarChar) { Value = data.searchby });
        //            cmd.Parameters.Add(new SqlParameter("@value", SqlDbType.VarChar) { Value = data.txtbox });
        //            //  cmd.Parameters.Add(new SqlParameter("@datevisit", SqlDbType.DateTime) { Value = data.Date_vist });
        //            if (cmd.Connection.State != ConnectionState.Open)
        //                cmd.Connection.Open();

        //            var retObject = new List<dynamic>();
        //            try
        //            {
        //                using (var dataReader = cmd.ExecuteReader())
        //                {
        //                    while (dataReader.Read())
        //                    {
        //                        result.Add(new GetVisitorReportDTO
        //                        {
        //                            AMVM_Name = dataReader["AMVM_Name"].ToString(),
        //                            AMVM_Contact_No = dataReader["AMVM_Contact_No"].ToString(),
        //                            AMVM_Emailid = dataReader["AMVM_Emailid"].ToString(),
        //                            Date_Visit = Convert.ToDateTime(dataReader["Date_Visit"].ToString()),
        //                            Time_Visit = dataReader["Time_Visit"].ToString(),
        //                            AMVM_Out_Time = dataReader["AMVM_Out_Time"].ToString(),
        //                            AMVM_Status = dataReader["AMVM_Status"].ToString(),
        //                            AMVM_Purpose = dataReader["AMVM_Purpose"].ToString(),
        //                        });
        //                        data.viewlist = result.ToArray();
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.Write(ex.Message);
        //            }
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
