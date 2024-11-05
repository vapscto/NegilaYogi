using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Services
{
    public class InOutCallsReportImpl : Interfaces.InOutCallsReportInterface
    {
        public DomainModelMsSqlServerContext _db;
        public PortalContext _ivrs;
        public InOutCallsReportImpl(DomainModelMsSqlServerContext db, PortalContext ivrs)
        {
            _db = db;
            _ivrs = ivrs;
        }
        public async Task<IVRSInOutCallsReportDTO> getreport(IVRSInOutCallsReportDTO data)
        {
            List<IVRSInOutCallsReportDTO> AllInOne = new List<IVRSInOutCallsReportDTO>();
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IVRS_IN_OUT_CALLS_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.DateTime) { Value = data.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.DateTime) { Value = data.todate });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@typeofrpt", SqlDbType.VarChar) { Value = data.typeofrpt });
                    cmd.Parameters.Add(new SqlParameter("@consol", SqlDbType.VarChar) { Value = data.conso });
                   
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
                        data.reportdatelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                return data;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
