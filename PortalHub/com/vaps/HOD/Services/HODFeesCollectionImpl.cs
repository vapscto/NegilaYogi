using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace PortalHub.com.vaps.HOD.Services
{
    public class HODFeesCollectionImpl: Interfaces.HODFeesCollectionInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
       // ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public HODFeesCollectionImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        public FEESOverAllStatusSchoolDTO Getdetails(FEESOverAllStatusSchoolDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {
                List<FEESOverAllStatusSchoolDTO> result = new List<FEESOverAllStatusSchoolDTO>();
                List<FEESOverAllStatusSchoolDTO> result1 = new List<FEESOverAllStatusSchoolDTO>();
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag==1).Distinct().OrderBy(t=>t.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();

                data.selectedyear = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOD_Classwise_fee_collaction";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.ASMAY_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@User_Id",
                     SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.user_id)
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
                                result1.Add(new FEESOverAllStatusSchoolDTO
                                {
                                    paid = Convert.ToDecimal(dataReader["callected"].ToString()),
                                    ballance = Convert.ToDecimal(dataReader["ballance"].ToString()),
                                    recived = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    concession = Convert.ToDecimal(dataReader["concession"].ToString()),
                                    feeclass = dataReader["class"].ToString(),
                                    classid = Convert.ToInt32(dataReader["classid"].ToString())


                                });
                                data.fillfee = result1.ToArray();
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
                //_acdimpl.LogError(ex.Message);
                //_acdimpl.LogDebug(ex.Message);
            }
            return data;

        }

        public FEESOverAllStatusSchoolDTO Getsectioncount(FEESOverAllStatusSchoolDTO data)
        {
            List<FEESOverAllStatusSchoolDTO> result1 = new List<FEESOverAllStatusSchoolDTO>();
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_Sectionwise_fee_collaction_Portal";
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
                                result1.Add(new FEESOverAllStatusSchoolDTO
                                {
                                    paid = Convert.ToDecimal(dataReader["callected"].ToString()),
                                    ballance = Convert.ToDecimal(dataReader["ballance"].ToString()),
                                    recived = Convert.ToDecimal(dataReader["receivable"].ToString()),
                                    concession = Convert.ToDecimal(dataReader["concession"].ToString()),
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
