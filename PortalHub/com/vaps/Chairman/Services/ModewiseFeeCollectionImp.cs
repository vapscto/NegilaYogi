

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
    public class ModewiseFeeCollectionImpl : Interfaces.ModewiseFeeCollectionInterface
    {
        private static ConcurrentDictionary<string, ChairmanDashboardDTO> _login =
         new ConcurrentDictionary<string, ChairmanDashboardDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public ModewiseFeeCollectionImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
        }
        
        public ModewiseFeeCollectionDTO Getdetails(ModewiseFeeCollectionDTO data)//int IVRMM_Id
        {
            //ChairmanDashboardDTO getdata = new ChairmanDashboardDTO();
            try
            {
                List<ModewiseFeeCollectionDTO> result = new List<ModewiseFeeCollectionDTO>();
                List<ModewiseFeeCollectionDTO> result1 = new List<ModewiseFeeCollectionDTO>();
               
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();




             var selectedyear= _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id==data.ASMAY_Id).ToList();

                data.selectedyear= selectedyear.ToArray();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chairman_ClientWiseFeesCollection_Monthly";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@USERID",
                      SqlDbType.BigInt)
                    {
                        Value = data.user_id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Year",
                      SqlDbType.Char)
                    {
                        Value = selectedyear.FirstOrDefault().ASMAY_Year
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
                        data.sectionwisestrenth = retObject.ToArray();
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



        public ModewiseFeeCollectionDTO Getsectioncount(ModewiseFeeCollectionDTO data)
        {
            List<ModewiseFeeCollectionDTO> result1 = new List<ModewiseFeeCollectionDTO>();
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
                                result1.Add(new ModewiseFeeCollectionDTO
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
