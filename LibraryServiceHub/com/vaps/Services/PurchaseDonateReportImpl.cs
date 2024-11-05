using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.COE;
using DomainModel.Model.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;


namespace LibraryServiceHub.com.vaps.Services
{
    public class PurchaseDonateReportImpl : Interfaces.PurchaseDonateReportInterface
    {

        public LibraryContext _LibraryContext;

        public PurchaseDonateReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }






        public async Task<CirculationParameterDTO> getdata(CirculationParameterDTO data)
        {
                try
                {
                    var retObject1 = new List<dynamic>();
                    using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Purchase_Donate_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.VarChar)
                        {
                            Value = Convert.ToInt64(data.MI_Id)
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                           SqlDbType.VarChar)
                        {
                            Value = data.LBCPA_Flg
                         });
                       cmd.Parameters.Add(new SqlParameter("@booktype",
                         SqlDbType.VarChar)
                       {
                             Value = data.Circ_Flag
                       });
                      if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
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
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] 
                                        );
                                    }

                                    retObject1.Add((ExpandoObject)dataRow);
                                }

                            }

                            data.alldata = retObject1.ToArray();
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
