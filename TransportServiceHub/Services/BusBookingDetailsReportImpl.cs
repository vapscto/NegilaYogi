using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
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

namespace TransportServiceHub.Services
{
    public class BusBookingDetailsReportImpl : Interfaces.BusBookingDetailsReportInterface
    {
        private static ConcurrentDictionary<string, TripReportDTO> _login =
        new ConcurrentDictionary<string, TripReportDTO>();
        public DomainModelMsSqlServerContext _db;
        public TransportContext _context;
        ILogger<BusBookingDetailsReportImpl> _areaimpl;
        public BusBookingDetailsReportImpl(ILogger<BusBookingDetailsReportImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db)
        {

            _areaimpl = areaimpl;
            _context = context;
            _db = db;
        }

        public TripReportDTO getdata(int id)
        {
            TripReportDTO data = new TripReportDTO();
            data.MI_Id = id;
            try
            {
                data.hiregrouplist = (from a in _db.MasterHirerGroupDMO
                                      from b in _db.TripOnlineBookingDMO
                                      where a.MI_Id == b.MI_Id && a.TRHG_ActiveFlg == true && a.TRHG_Id == b.TRHG_Id && a.MI_Id == data.MI_Id
                                      select a).Distinct().ToArray();

                data.fillvahicletype = _context.MasterVehicleTypeDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMVT_ActiveFlg == true).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }


        public TripReportDTO Getreportdetails(TripReportDTO data)

        {
            try
            {

               // data.griddata = _context.TripOnlineBookingDMO.Where(t => t.MI_Id == data.MI_Id && t.TRHG_Id == data.TRHG_Id && t.TRTOB_BookingDate >= data.frmdate && t.TRTOB_BookingDate <= data.todate).ToArray();




                List<TripReportDTO> result1 = new List<TripReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_TRIP_BOOKING_DETAIL_REPORT";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMVT_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.TRMVT_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRHG_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.TRHG_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                                SqlDbType.Date)
                    {
                        Value = data.frmdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                                SqlDbType.Date)
                    {
                        Value = data.todate
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
                        data.griddata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        }
}
