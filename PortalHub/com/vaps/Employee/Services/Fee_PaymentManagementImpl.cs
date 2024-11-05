using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class Fee_PaymentManagementImpl:Interfaces.Fee_PaymentManagementInterface
    {
        public FeeGroupContext _feecontext;
        public Fee_PaymentManagementImpl(FeeGroupContext feecontext)
        {
            _feecontext = feecontext;
        }
        public Fee_Payment_ManagementDTO getFee_PaymentManagement(Fee_Payment_ManagementDTO dto)
        {
            try
            {
                using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_ClientWiseFeesCollection";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.VarChar)
                    {
                        Value = dto.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate",
                   SqlDbType.VarChar)
                    {
                        Value = dto.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate",
                    SqlDbType.VarChar)
                    {
                        Value = dto.todate
                    });

                    cmd.Parameters.Add(new SqlParameter("@Type",
                    SqlDbType.BigInt)
                    {
                        Value = dto.termswise
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
                        dto.getreportdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
