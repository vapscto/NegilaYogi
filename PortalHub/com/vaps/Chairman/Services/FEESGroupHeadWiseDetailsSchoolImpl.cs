

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
    public class FEESGroupHeadWiseDetailsSchoolImpl : Interfaces.FEESGroupHeadWiseDetailsSchoolInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public FEESGroupHeadWiseDetailsSchoolImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public FEESGroupHeadWiseDetailsSchoolDTO Getdetails(FEESGroupHeadWiseDetailsSchoolDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {
                

                List<FEESGroupHeadWiseDetailsSchoolDTO> result = new List<FEESGroupHeadWiseDetailsSchoolDTO>();
                List<FEESGroupHeadWiseDetailsSchoolDTO> result1 = new List<FEESGroupHeadWiseDetailsSchoolDTO>();
               


                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.selectedyear= _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id==data.ASMAY_Id).ToArray();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_Groupwise_fee_collection_portal";
                    cmd.CommandType = CommandType.StoredProcedure;
                   cmd.CommandTimeout = 800000;

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






                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();


                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result1.Add(new FEESGroupHeadWiseDetailsSchoolDTO
                                {
                                    paid = Convert.ToDecimal(dataReader["callected"].ToString()),
                                    ballance = Convert.ToDecimal(dataReader["ballance"].ToString()),
                                    recived = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    concession = Convert.ToDecimal(dataReader["concession"].ToString()),
                                    groupname = dataReader["groupname"].ToString(),
                                    groupid = Convert.ToInt32(dataReader["groupid"].ToString())


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


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_feeheadwise_fee_collaction";
                    cmd.CommandType = CommandType.StoredProcedure;
                     cmd.CommandTimeout = 800000;

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






                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();


                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FEESGroupHeadWiseDetailsSchoolDTO
                                {
                                    paid = Convert.ToDecimal(dataReader["callected"].ToString()),
                                    ballance = Convert.ToDecimal(dataReader["ballance"].ToString()),
                                    recived = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    concession = Convert.ToDecimal(dataReader["concession"].ToString()),
                                    headname = dataReader["headname"].ToString(),
                                    headid = Convert.ToInt32(dataReader["headid"].ToString())


                                });
                                data.fillhead = result.ToArray();
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
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }



        public FEESGroupHeadWiseDetailsSchoolDTO Getsectioncount(FEESGroupHeadWiseDetailsSchoolDTO data)
        {
            List<FEESGroupHeadWiseDetailsSchoolDTO> result1 = new List<FEESGroupHeadWiseDetailsSchoolDTO>();
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_feeheadwise_groupbyclass_collaction_portal";
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
                    cmd.Parameters.Add(new SqlParameter("@FMH_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.headid
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
                                result1.Add(new FEESGroupHeadWiseDetailsSchoolDTO
                                {
                                    paid = Convert.ToDecimal(dataReader["callected"].ToString()),
                                    ballance = Convert.ToDecimal(dataReader["ballance"].ToString()),
                                    recived = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    concession = Convert.ToDecimal(dataReader["concession"].ToString()),
                                    Class_Name = dataReader["classname"].ToString(),
                                    classid = Convert.ToInt32(dataReader["classid"].ToString())


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




        


             public FEESGroupHeadWiseDetailsSchoolDTO Getgroupclasscount(FEESGroupHeadWiseDetailsSchoolDTO data)
        {
            List<FEESGroupHeadWiseDetailsSchoolDTO> result1 = new List<FEESGroupHeadWiseDetailsSchoolDTO>();
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_feegroupwise_groupbyclass_collaction_portal";
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
                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.groupid
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
                                result1.Add(new FEESGroupHeadWiseDetailsSchoolDTO
                                {
                                    paid = Convert.ToDecimal(dataReader["callected"].ToString()),
                                    ballance = Convert.ToDecimal(dataReader["ballance"].ToString()),
                                    recived = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    concession = Convert.ToDecimal(dataReader["concession"].ToString()),
                                    Class_Name = dataReader["classname"].ToString(),
                                    classid = Convert.ToInt32(dataReader["classid"].ToString())


                                });
                                data.groupclass = result1.ToArray();
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
