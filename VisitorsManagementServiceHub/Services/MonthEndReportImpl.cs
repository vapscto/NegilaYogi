using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Services
{
    public class MonthEndReportImpl:Interfaces.MonthEndReportInterface
    {
        private VisitorsManagementContext _visctxt;
        private DomainModelMsSqlServerContext _db;
        public MonthEndReportImpl(VisitorsManagementContext para1, DomainModelMsSqlServerContext para2)
        {
            _visctxt = para1;
            _db = para2;
        }


        public VisitorsMonthEndReport_DTO getdeatils(VisitorsMonthEndReport_DTO data)
        {
            try
            {
                var q = (from a in _db.month
                         where (a.Is_Active == true)
                         select new
                         {
                             monthid = a.IVRM_Month_Id,
                             monthname = a.IVRM_Month_Name,
                         }).Distinct().ToArray();

                var query = q.Distinct().ToArray();
                data.fillmonth = (from a in query
                                  select new VisitorsMonthEndReport_DTO
                                  {
                                      month = Convert.ToInt32(a.monthid),
                                      monthname = a.monthname
                                  }).Distinct().OrderBy(t => t.month).ToArray();


                data.fillyear = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }

        public async Task<VisitorsMonthEndReport_DTO> GetReport(VisitorsMonthEndReport_DTO data)
        {
            try
            {
               
                using (var cmd = _visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "VM_MONTH_END_REPORT";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

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

                    cmd.Parameters.Add(new SqlParameter("@frmdate",
                                SqlDbType.VarChar)
                    {
                        Value = data.year
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                                SqlDbType.VarChar)
                    {
                        Value = data.IVRM_Month_Id
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
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            return data;
        }
    }
}
