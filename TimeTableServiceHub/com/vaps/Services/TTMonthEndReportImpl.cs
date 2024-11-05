using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
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
    public class TTMonthEndReportImpl:Interfaces.TTMonthEndReportInterface
    {
        public TTContext _Context;
        public TTMonthEndReportImpl(TTContext ttContext)
        {
            _Context = ttContext;
        }

        public TTMonthEndReportDTO getdata123(TTMonthEndReportDTO data)
        {

            try
            {
                data.acdlist = _Context.AcademicYear.AsNoTracking().Where(t => t.MI_Id.Equals(data.MI_ID) && t.Is_Active == true).Distinct().OrderByDescending(f=>f.ASMAY_Order).ToList().ToArray();

                data.monthlist = _Context.month.Where(t => t.Is_Active == true).ToList().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<TTMonthEndReportDTO> getreport(TTMonthEndReportDTO data)
        {
            List<TTMonthEndReportDTO> AllInOne = new List<TTMonthEndReportDTO>();
            try
            {             
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_Monthend_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@month", SqlDbType.BigInt) {Value = data.month});
                    cmd.Parameters.Add(new SqlParameter("@year",SqlDbType.BigInt) {Value = data.year});
                    cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) {Value = data.MI_ID});
                    cmd.Parameters.Add(new SqlParameter("@asmay_id", SqlDbType.BigInt){Value = data.ASMAY_ID});
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
