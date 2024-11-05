using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class YearlyAnalysisReportImpl : Interfaces.YearlyAnalysisReportInterface
    {
        public AdmissionFormContext _context;

        ILogger<YearlyAnalysisReportImpl> _logger;

        public YearlyAnalysisReportImpl(AdmissionFormContext context, ILogger<YearlyAnalysisReportImpl> logger)
        {
            _context = context;
            _logger = logger;
        }

        public YearlyAnalysisReportDTO loaddata(YearlyAnalysisReportDTO data)
        {
            try
            {
                data.getyearlist = _context.year.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("Yearly Analysi Report Load : " + ex.Message);
            }
            return data;
        }

        public YearlyAnalysisReportDTO report(YearlyAnalysisReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_School_Yearly_Analaysis_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Noofyears", SqlDbType.VarChar)
                    {
                        Value = data.Noofyears
                    });
                    cmd.Parameters.Add(new SqlParameter("@TCflag", SqlDbType.Int)
                    {
                        Value = data.tcflag
                    });

                    cmd.Parameters.Add(new SqlParameter("@Deactiveflag", SqlDbType.Int)
                    {
                        Value = data.deactiveflag
                    });

                    cmd.Parameters.Add(new SqlParameter("@Reporttype", SqlDbType.VarChar)
                    {
                        Value = data.reporttype
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
                        data.getreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                List<long> yearid = new List<long>();

                var getselectedyearorder = _context.year.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true).ToList();

                var getyearlist = _context.year.Where(a => a.MI_Id == data.MI_Id
                && a.ASMAY_Order <= getselectedyearorder.FirstOrDefault().ASMAY_Order).OrderByDescending(a => a.ASMAY_Order).Take(data.Noofyears).ToList();

                foreach (var c in getyearlist)
                {
                    yearid.Add(c.ASMAY_Id);
                }

                data.getreportacademicyearlist = _context.year.Where(a => a.MI_Id == data.MI_Id && yearid.Contains(a.ASMAY_Id)).OrderBy(a => a.ASMAY_Order).ToArray();

                data.getclasslist = _context.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("Yearly Analysi Report report : " + ex.Message);
            }
            return data;
        }

    }
}
