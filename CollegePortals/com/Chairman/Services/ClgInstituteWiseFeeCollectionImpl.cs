using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePortals.com.Chairman.Services
{
    public class ClgInstituteWiseFeeCollectionImpl : Interfaces.ClgInstituteWiseFeeCollectionInterface
    {
        private static ConcurrentDictionary<string, ClgInstituteWiseFeeCollectionDTO> _login =
          new ConcurrentDictionary<string, ClgInstituteWiseFeeCollectionDTO>();
        private CollegeportalContext _ClgPortalContext;
        ILogger<ClgInstituteWiseFeeCollectionImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public ClgInstituteWiseFeeCollectionImpl(CollegeportalContext ClgPortalContext, DomainModelMsSqlServerContext db)
        {
            _ClgPortalContext = ClgPortalContext;
            _db = db;
        }
        
        public ClgInstituteWiseFeeCollectionDTO Getdetails(ClgInstituteWiseFeeCollectionDTO data)//int IVRMM_Id
        {
         
            try
            {
                List<ClgInstituteWiseFeeCollectionDTO> result = new List<ClgInstituteWiseFeeCollectionDTO>();
                List<ClgInstituteWiseFeeCollectionDTO> result1 = new List<ClgInstituteWiseFeeCollectionDTO>();
                data.institutename = _db.Institute.Where(t => t.MI_ActiveFlag == 1).ToArray();
                data.monthname = _db.month.Where(t => t.Is_Active == true).ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }
        public ClgInstituteWiseFeeCollectionDTO Getsectioncount(ClgInstituteWiseFeeCollectionDTO data)
        {
            List<ClgInstituteWiseFeeCollectionDTO> result1 = new List<ClgInstituteWiseFeeCollectionDTO>();
            try
            {
                var miid = "";
                foreach (var x in data.mi_ids)
                {
                    miid += x + ",";
                }
                miid = miid.Substring(0, (miid.Length - 1));
                //var monthid = "";
                //foreach (var x in data.monthids)
                //{
                //    monthid += x + ",";
                //}
               // monthid = monthid.Substring(0, (monthid.Length - 1));

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "InstituwiseFeeCollectionNew";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = miid
                    });
                    //cmd.Parameters.Add(new SqlParameter("@monthid",
                    //  SqlDbType.VarChar)
                    //{
                    //    Value = monthid
                    //});
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                      SqlDbType.VarChar)
                    {
                        Value = data.StartDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                     SqlDbType.VarChar)
                    {
                        Value = data.EndDate
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
                                result1.Add(new ClgInstituteWiseFeeCollectionDTO
                                {
                                    paid = Convert.ToDecimal(dataReader["collected"].ToString()),
                                    ballance = Convert.ToDecimal(dataReader["ballance"].ToString()),
                                    recived = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    concession = Convert.ToDecimal(dataReader["concession"].ToString()),


                                    waived = Convert.ToDecimal(dataReader["waived"].ToString()),
                                    rebate = Convert.ToDecimal(dataReader["rebate"].ToString()),
                                    fine = Convert.ToDecimal(dataReader["fine"].ToString()),
                                    receivable = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    FYP_Date= dataReader["FYP_DOE"].ToString(),
                                    MI_Name = dataReader["MI_Name"].ToString(),
                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"].ToString())

                                });
                                data.institutedata = result1.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

    }
}
