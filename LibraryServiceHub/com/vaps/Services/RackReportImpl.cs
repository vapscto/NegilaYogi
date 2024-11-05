using DataAccessMsSqlServerProvider.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class RackReportImpl:Interfaces.RackReportInterface
    {
        public LibraryContext _LibraryContext;
        public RackReportImpl(LibraryContext context)
        {
            _LibraryContext = context;
        }

        public RackReportDTO getdetails(RackReportDTO data)
        {
            
            try
            {             

                data.racklist = (from a in _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_ActiveFlag == true) select new RackReportDTO { LMRA_RackName = a.LMRA_RackName, LMRA_Id=a.LMRA_Id }).Distinct().OrderBy(t => t.LMRA_Id).ToArray();

                data.floorlist = (from a in _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_ActiveFlag == true) select new RackReportDTO { LMRA_FloorName =a.LMRA_FloorName ,}).Distinct().OrderBy(t => t.LMRA_FloorName).ToArray();

                data.lib_list = (from a in _LibraryContext.LIB_Master_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO
                                   // from c in _LibraryContext.LIB_Library_Class_DMO
                               where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true && b.IVRMUL_Id == data.IVRMUL_Id
                               select a).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<RackReportDTO> get_report(RackReportDTO data)
        {
            try
            {
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lib_RackReport_NewOne";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.LMAL_Id
                    });


                    //cmd.Parameters.Add(new SqlParameter("@LMRA_FloorName",
                    //SqlDbType.VarChar)
                    //{
                    //    Value = data.LMRA_FloorName
                    //});

                    cmd.Parameters.Add(new SqlParameter("@LMRA_RackName",
                    SqlDbType.VarChar)
                    {
                        Value = data.LMRA_RackName
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
