using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
namespace TransportServiceHub.Services
{
    public class ConsolidatedBusRouteListImpl : Interfaces.ConsolidatedBusRouteListInterface
    {
        public TransportContext _context;
        ILogger<ConsolidatedBusRouteListImpl> _areaimpl;
        public ConsolidatedBusRouteListImpl(ILogger<ConsolidatedBusRouteListImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public ConsolidatedBusRouteDTO getdata(int id)
        {
            ConsolidatedBusRouteDTO data = new ConsolidatedBusRouteDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id && y.Is_Active==true ).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }


        public ConsolidatedBusRouteDTO Getreportdetails(ConsolidatedBusRouteDTO data)

        {
            try
            {


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ConsolidatedListForBusRoute_new";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
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
