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
    public class TriphiresmsemailreportImpl : Interfaces.TriphiresmsemailreportInterface
    {
        private static ConcurrentDictionary<string, TriphiresmsemailreportDTO> _login =
        new ConcurrentDictionary<string, TriphiresmsemailreportDTO>();
        public DomainModelMsSqlServerContext _db;
        public TransportContext _context;
        ILogger<TRGroupConsoleReportImpl> _areaimpl;
        public TriphiresmsemailreportImpl(ILogger<TRGroupConsoleReportImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db)
        {

            _areaimpl = areaimpl;
            _context = context;
            _db = db;
        }

        public TriphiresmsemailreportDTO getdata(int id)
        {
            TriphiresmsemailreportDTO data = new TriphiresmsemailreportDTO();
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


        public TriphiresmsemailreportDTO Getreportdetails(TriphiresmsemailreportDTO data)

        {
            try
            {
                List<TriphiresmsemailreportDTO> result1 = new List<TriphiresmsemailreportDTO>();
                
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TRN_SMS_EMAIL_DETAILS_BUS_HIRE";

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

                    cmd.Parameters.Add(new SqlParameter("@type",
                                SqlDbType.VarChar)
                    {
                        Value = data.type
                    });
                    cmd.Parameters.Add(new SqlParameter("@template",
                                SqlDbType.VarChar)
                    {
                        Value = data.templete
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
