using DataAccessMsSqlServerProvider.com.vapstech.Inventory;
using DomainModel.Model.com.vapstech.Inventory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Implementation
{
    public class INV_MonthEndReportImpl : Interface.INV_MonthEndReportInterface
    {
        public InventoryContext _INVContext;
        ILogger<INV_MonthEndReportImpl> _logInv;
        public INV_MonthEndReportImpl(InventoryContext InvContext, ILogger<INV_MonthEndReportImpl> log)
        {
            _INVContext = InvContext;
            _logInv = log;
        }

        public INV_MonthEndReportDTO getloaddata(INV_MonthEndReportDTO data)
        {
            try
            {
                var list = _INVContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.acayear = list.ToArray();

                data.Month_array = _INVContext.mnth.Where(a => a.Is_Active == true).ToArray();
            }
            catch (Exception ex)
            {
                _logInv.LogInformation("Month-End Report Page load:" + ex.Message);
            }
            return data;
        }

        public async Task<INV_MonthEndReportDTO> getmonthreport(INV_MonthEndReportDTO data)
        {
            try
            {              
                using (var cmd = _INVContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_MonthEndReport";
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

                    cmd.Parameters.Add(new SqlParameter("@month",
                  SqlDbType.VarChar)
                    {
                        Value = data.month
                    });

                    cmd.Parameters.Add(new SqlParameter("@year",
                  SqlDbType.VarChar)
                    {
                        Value = data.year
                    });                
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
                        data.get_monthendreport = retObject.ToArray();
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
