using PreadmissionDTOs.com.vaps.College.Portals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;

namespace CollegePortals.com.Student.Services
{
    public class ClgFeeDetailsImpl : Interfaces.ClgFeeDetailsInterface
    {
        private static ConcurrentDictionary<string, ClgPortalFeeDTO> _login =
           new ConcurrentDictionary<string, ClgPortalFeeDTO>();
        private CollegeportalContext _ClgPortalContext;
        public ClgFeeDetailsImpl(CollegeportalContext ClgPortalContext)
        {
            _ClgPortalContext = ClgPortalContext;
        }

        public ClgPortalFeeDTO getloaddata(ClgPortalFeeDTO data)
        {
            try
            {
                data.yearlist = (from a in _ClgPortalContext.Adm_Master_College_StudentDMO
                                 from b in _ClgPortalContext.Adm_College_Yearly_StudentDMO
                                 from c in _ClgPortalContext.academicYearDMO
                                 where (a.AMCST_Id == b.AMCST_Id && b.ASMAY_Id == c.ASMAY_Id && b.AMCST_Id == data.AMCST_Id
                                 && a.MI_Id == data.MI_Id && b.ACYST_ActiveFlag == 1 && a.AMCST_SOL == "S" && a.AMCST_ActiveFlag == true)

                                 select new ClgPortalFeeDTO
                                 {
                                     ASMAY_Id = c.ASMAY_Id,
                                     ASMAY_Year = c.ASMAY_Year,
                                     ASMAY_Order = c.ASMAY_Order
                                 }).Distinct().OrderBy(a => a.ASMAY_Order).ToArray();

                data.currentyear = _ClgPortalContext.academicYearDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ClgPortalFeeDTO> Getdetails(ClgPortalFeeDTO data)
        {
            try
            {
                using (var cmd = _ClgPortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_PORTAL_FEE_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;

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

                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@type",
                    SqlDbType.VarChar)
                    {
                        Value = data.type
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
                        data.getfeedetails = retObject.ToArray();
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
