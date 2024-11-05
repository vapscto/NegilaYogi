

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
using DataAccessMsSqlServerProvider.com.vapstech.Exam;

namespace PortalHub.com.vaps.Chairman.Services
{
    public class ExamHomeImpl : Interfaces.ExamHomeInterface
    {
        private static ConcurrentDictionary<string, ExamHomeDTO> _login =
         new ConcurrentDictionary<string, ExamHomeDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        //public ExamContext _exm;
        public ExamHomeImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
            //_exm = exm;
        }

        public ExamHomeDTO Getdetails(ExamHomeDTO data)//int IVRMM_Id
        {



            List<MasterAcademic> list = new List<MasterAcademic>();
            list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
            data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();


            return data;

        }




        public ExamHomeDTO getclassexam(ExamHomeDTO data)//int IVRMM_Id
        {


            try
            {



                data.classlist = (from a in _ChairmanDashboardContext.School_M_Class
                                  from b in _ChairmanDashboardContext.Exm_Category_ClassDMO
                                  where (a.MI_Id == b.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag.Equals(true) && a.ASMCL_ActiveFlag.Equals(true))
                                  select new ExamHomeDTO
                                  {
                                      ASMCL_ClassName = a.ASMCL_ClassName,
                                      ASMCL_Id = a.ASMCL_Id
                                  }).Distinct().ToArray();



                data.exmstdlist = (from a in _ChairmanDashboardContext.Exm_Yearly_CategoryDMO
                                   from b in _ChairmanDashboardContext.Exm_Yearly_Category_ExamsDMO
                                   from c in _ChairmanDashboardContext.exammasterDMO

                                   where (a.MI_Id == c.MI_Id && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && a.EYC_Id == b.EYC_Id && b.EME_Id == c.EME_Id && c.EME_ActiveFlag.Equals(true))
                                   select new ExamHomeDTO
                                   {
                                       EME_ExamName = c.EME_ExamName,
                                       EME_Id = c.EME_Id
                                   }).Distinct().OrderBy(t => t.EME_Id).ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }
        public ExamHomeDTO getcategory(ExamHomeDTO data)//int IVRMM_Id
        {


            try
            {

                data.fillcategory = (from a in _ChairmanDashboardContext.Exm_Master_CategoryDMO
                                     from b in _ChairmanDashboardContext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == b.EMCA_Id && b.EYC_ActiveFlg.Equals(true) && a.EMCA_ActiveFlag.Equals(true))


                                     select new ExamHomeDTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName
                                     }).Distinct().OrderBy(t => t.EMCA_Id).ToArray();




            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;

        }


        public ExamHomeDTO showreport(ExamHomeDTO data)
        {


            try
            {
                List<ExamHomeDTO> result1 = new List<ExamHomeDTO>();
                List<ExamHomeDTO> result2 = new List<ExamHomeDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chairman_Exm_Classwise_passfail_count";
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
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.EME_Id
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
                                result1.Add(new ExamHomeDTO
                                {
                                    Total_Count = Convert.ToInt32(dataReader["Total_Count"].ToString()),
                                    Pass_Count = Convert.ToInt32(dataReader["Pass_Count"].ToString()),
                                    Fail_Count = Convert.ToInt32(dataReader["Fail_Count"].ToString()),


                                });
                                data.studlist = result1.ToArray();
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
                    cmd.CommandText = "Chairman_Exm_passfail_count_allclass";
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
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.EME_Id
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
                                result2.Add(new ExamHomeDTO
                                {
                                    ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString()),
                                    Pass_Count = Convert.ToInt32(dataReader["Pass"].ToString()),
                                    Fail_Count = Convert.ToInt32(dataReader["Fail"].ToString()),
                                    
                                });
                                data.graphstudlist = result2.ToArray();
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



        public ExamHomeDTO showsectioncount(ExamHomeDTO data)
        {
            try
            {
                List<ExamHomeDTO> result2 = new List<ExamHomeDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Chairman_Exm_passfail_count_allsection";
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
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
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
                                result2.Add(new ExamHomeDTO
                                {
                                    ASMC_SectionName = (dataReader["section"].ToString()),
                                    Pass_Count = Convert.ToInt32(dataReader["Pass"].ToString()),
                                    Fail_Count = Convert.ToInt32(dataReader["Fail"].ToString()),

                                });
                                data.seclist = result2.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }

    }
}
