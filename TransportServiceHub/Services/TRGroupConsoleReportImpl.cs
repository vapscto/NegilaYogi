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
    public class TRGroupConsoleReportImpl : Interfaces.TRGroupConsoleReportInterface
    {
        private static ConcurrentDictionary<string, TripReportDTO> _login =
        new ConcurrentDictionary<string, TripReportDTO>();
        public DomainModelMsSqlServerContext _db;
        public TransportContext _context;
        ILogger<TRGroupConsoleReportImpl> _areaimpl;
        public TRGroupConsoleReportImpl(ILogger<TRGroupConsoleReportImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db)
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
                //List<AcademicYear> allyear = new List<AcademicYear>();
                //allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id).ToList();
                //data.YearList = allyear.Distinct().ToArray();



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
                List<TripReportDTO> result1 = new List<TripReportDTO>();
                
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TRN_CONSOLIDATED_BUSHIRE_CHARGES_REPORT";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    
                    cmd.Parameters.Add(new SqlParameter("@frmdate",
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
