

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
    public class FEESTodayCollectionImpl : Interfaces.FEESTodayCollectionInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public FEESTodayCollectionImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public FEESTodayCollectionDTO Getdetails(FEESTodayCollectionDTO data)//int IVRMM_Id
        {
           
            try
            {
                

                List<FEESTodayCollectionDTO> result = new List<FEESTodayCollectionDTO>();
                List<FEESTodayCollectionDTO> result1 = new List<FEESTodayCollectionDTO>();

                if (data.eventName=="C")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Get_DatewiseFeecollection_Portal";
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
                        cmd.Parameters.Add(new SqlParameter("@fromdate",
                         SqlDbType.DateTime)
                        {
                            Value = data.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                         SqlDbType.DateTime)
                        {
                            Value = data.todate
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
                                    result1.Add(new FEESTodayCollectionDTO
                                    {
                                        paid = Convert.ToDecimal(dataReader["amount"].ToString()),
                                        recpt_count = Convert.ToInt32(dataReader["recept"].ToString()),
                                        Class_Name = dataReader["classname"].ToString(),
                                        classid = Convert.ToInt32(dataReader["classid"].ToString())


                                    });
                                    data.fillgroupfee = result1.ToArray();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Get_DatewiseFeecollection_Portal_date";
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
                        cmd.Parameters.Add(new SqlParameter("@fromdate",
                         SqlDbType.DateTime)
                        {
                            Value = data.fromdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                         SqlDbType.DateTime)
                        {
                            Value = data.todate
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
                            data.fillgroupfee = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }

                       
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



        public FEESTodayCollectionDTO Getsectionpop(FEESTodayCollectionDTO data)
        {
            List<FEESTodayCollectionDTO> result1 = new List<FEESTodayCollectionDTO>();
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_Section_DatewiseFeecollection_Portal";
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
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                     SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                     SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.classid
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
                                result1.Add(new FEESTodayCollectionDTO
                                {
                                    paid = Convert.ToDecimal(dataReader["amount"].ToString()),
                                    recpt_count = Convert.ToInt32(dataReader["recept"].ToString()),
                                    sectionname = dataReader["sectionname"].ToString(),
                                    sectionid = Convert.ToInt32(dataReader["sectionid"].ToString())


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
