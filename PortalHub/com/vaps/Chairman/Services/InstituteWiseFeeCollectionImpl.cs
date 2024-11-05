

using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
namespace PortalHub.com.vaps.Chairman.Services
{
    public class InstituteWiseFeeCollectionImpl : Interfaces.InstituteWiseFeeCollectionInterface
    {

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<InstituteWiseFeeCollectionImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public InstituteWiseFeeCollectionImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }

        public InstituteWiseFeeCollectionDTO Getdetails(InstituteWiseFeeCollectionDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {


                List<InstituteWiseFeeCollectionDTO> result = new List<InstituteWiseFeeCollectionDTO>();
                List<InstituteWiseFeeCollectionDTO> result1 = new List<InstituteWiseFeeCollectionDTO>();
                

              //  data.monthname = _db.month.Where(t => t.Is_Active == true).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Rolewise_Institution";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERId",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
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
                        data.institutename = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }


                }



            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }



        public InstituteWiseFeeCollectionDTO Getsectioncount(InstituteWiseFeeCollectionDTO data)
        {
            List<InstituteWiseFeeCollectionDTO> result1 = new List<InstituteWiseFeeCollectionDTO>();
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
                //monthid = monthid.Substring(0, (monthid.Length - 1));

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "InstituwiseFeeCollection";
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
                        Value = data.COEE_EStartDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                     SqlDbType.VarChar)
                    {
                        Value = data.COEE_EEndDate
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
                                result1.Add(new InstituteWiseFeeCollectionDTO
                                {
                                    paid = Convert.ToDecimal(dataReader["callected"].ToString()),
                                    ballance = Convert.ToDecimal(dataReader["ballance"].ToString()),
                                    recived = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    concession = Convert.ToDecimal(dataReader["concession"].ToString()),
                                  

                                    waived = Convert.ToDecimal(dataReader["waived"].ToString()),
                                    rebate = Convert.ToDecimal(dataReader["rebate"].ToString()),
                                    fine = Convert.ToDecimal(dataReader["fine"].ToString()),
                                    receivable = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    IVRM_Month_Name = dataReader["IVRM_Month_Name"].ToString(),
                                    MI_Name = dataReader["MI_Name"].ToString(),
                                    MI_Id = Convert.ToInt64(dataReader["MI_Id"].ToString())

                                });
                                data.sectionarray = result1.ToArray();
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
