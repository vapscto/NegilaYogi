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
using TimeTableServiceHub.com.vaps.Interfaces.College;

namespace TimeTableServiceHub.com.vaps.Services.College
{
    public class CLGTTConstraintReportImpl: CLGTTConstraintReportInterface
    {
        public TTContext _ttcontext;
        public CLGTTConstraintReportImpl(TTContext u)
        {
            _ttcontext = u;
        }

        public CLGTTConstraintReportDTO getalldetails(int id)
        {
            CLGTTConstraintReportDTO data = new CLGTTConstraintReportDTO();
            try
            {
                data.Acayear = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == Convert.ToInt64(id) && t.Is_Active == true).ToList().Distinct().ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public CLGTTConstraintReportDTO getpagedetails(CLGTTConstraintReportDTO data)
        {
            try
            {
                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "[TT_CLG_SCHOOL_CONSTRAINT_REPORT]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_ID",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_ID
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });


                    cmd.Parameters.Add(new SqlParameter("@Ctype",
                    SqlDbType.NVarChar)
                    {
                        Value = data.constype
                    });
                    cmd.Parameters.Add(new SqlParameter("@Vtype",
                    SqlDbType.Char)
                    {
                        Value = data.periodtype
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
                        data.getreportdata = retObject.ToArray();
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

    }
}
