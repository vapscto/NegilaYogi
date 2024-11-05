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
    public class NonBookReportImpl:Interfaces.NonBookReportInterface
    {

        private LibraryContext _LibraryContext;
        public NonBookReportImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }
        public NonBookReport_DTO getdetails(NonBookReport_DTO id)
        {

            try
            {
                
                id.deptlist = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == id.MI_Id && t.LMD_ActiveFlg == true).Distinct().ToArray();

                id.lib_list = (from a in _LibraryContext.LIB_Master_Library_DMO
                               //from c in _LibraryContext.LIB_Master_NonBook_Library_DMO
                               from b in _LibraryContext.LIB_User_Library_DMO                               
                                  
                               where a.MI_Id == b.MI_Id && a.MI_Id == id.MI_Id /*&& a.LMAL_Id==c.LMAL_Id*/ && a.LMAL_Id == b.LMAL_Id && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true /*&& c.LMNBKL_ActiveFlg==true*/ && b.IVRMUL_Id == id.IVRMUL_Id
                               select a).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return id;
        }
        public async Task<NonBookReport_DTO> get_report(NonBookReport_DTO data)
        {
            try
            {
                if (data.LMD_Id == "0")
                {
                    data.LMD_Id = "ALL";
                }
                List<NonBookReport_DTO> result1 = new List<NonBookReport_DTO>();

                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lib_Non_Book_Report";

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
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                  SqlDbType.BigInt)
                    {
                        Value = data.LMAL_Id
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
