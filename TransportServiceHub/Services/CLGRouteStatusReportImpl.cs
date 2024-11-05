using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class CLGRouteStatusReportImpl : Interfaces.CLGRouteStatusReportInterface
    {
        private static ConcurrentDictionary<string, RouteStatusReportDTO> _login =
        new ConcurrentDictionary<string, RouteStatusReportDTO>();

        public TransportContext _context;
        ILogger<CLGRouteStatusReportImpl> _areaimpl;
        public CLGRouteStatusReportImpl(ILogger<CLGRouteStatusReportImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public RouteStatusReportDTO getdata(int id)
        {
            RouteStatusReportDTO data = new RouteStatusReportDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id ).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.routename = _context.MasterRouteDMO.Where(a => a.MI_Id == id && a.TRMR_ActiveFlg==true).OrderBy(d=>d.TRMR_order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }
        public RouteStatusReportDTO Getreportdetails(RouteStatusReportDTO data)
        {

            try
            {
              
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_RouteWiseStatus_Collage";
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
                    cmd.Parameters.Add(new SqlParameter("@TRMR_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.TRMR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AppStatus",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASTA_ApplStatus
                    });
                    cmd.Parameters.Add(new SqlParameter("@StudentType",
                   SqlDbType.VarChar)
                    {
                        Value = data.regorname_map
                    });
                    cmd.Parameters.Add(new SqlParameter("@paystatus",
                   SqlDbType.VarChar)
                    {
                        Value = data.Paidnotpaid
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
                        data.messagelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }



            return data;
        }

    }
}
