using DataAccessMsSqlServerProvider.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class DeputationReportIMPL : Interfaces.DeputationReportInterface
    {
        public TTContext _Context;
        public DeputationReportIMPL(TTContext ttContext)
        {
            _Context = ttContext;
        }

        public async Task<TTDeputationReportDTO> getreport(TTDeputationReportDTO data)
        {
            List<TTDeputationReportDTO> AllInOne = new List<TTDeputationReportDTO>();
            try
            {
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_Deputation_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.Date) { Value = data.fromdate });
                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.Date) { Value = data.todate });
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_ID });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt) { Value = data.ASMAY_ID });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.Char) { Value = data.flag });
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
