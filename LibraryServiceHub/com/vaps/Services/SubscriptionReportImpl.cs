using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class SubscriptionReportImpl : Interfaces.SubscriptionReportInterface
    {

        public LibraryContext _LibContext;
        public DomainModelMsSqlServerContext _context;
       

        public SubscriptionReportImpl(LibraryContext paar, DomainModelMsSqlServerContext paar2)
        {
            _LibContext = paar;
            _context = paar2;
        }
        public NonBookReport_DTO getdetails(NonBookReport_DTO data)
        {
            try
            {
                data.deptlist = _LibContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_ActiveFlg == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<NonBookReport_DTO> get_report(NonBookReport_DTO data)
        {
            try
            {
                if(data.LMD_Id=="0")
                {
                    data.LMD_Id = "ALL";
                }
                List<NonBookReport_DTO> result1 = new List<NonBookReport_DTO>();

                using (var cmd = _LibContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lib_Subscription_Report";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;


                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMD_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.LMD_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = data.Fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                 SqlDbType.VarChar)
                    {
                        Value = data.ToDate
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
                        data.reportlist = retObject.ToArray();
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
