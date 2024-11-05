

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
    public class ExamToppersListImpl : Interfaces.ExamToppersListInterface
    {
        private static ConcurrentDictionary<string, ExamToppersListDTO> _login =
         new ConcurrentDictionary<string, ExamToppersListDTO>();

        private readonly PortalContext _ChairmanDashboardContext;
        ILogger<HomeSchoolAdmImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        //public ExamContext _exm;
        public ExamToppersListImpl(PortalContext cpContext, DomainModelMsSqlServerContext db)
        {
            _ChairmanDashboardContext = cpContext;
            _db = db;
            //_exm = exm;
        }

        public ExamToppersListDTO Getdetails(ExamToppersListDTO data)//int IVRMM_Id
        {



            List<MasterAcademic> list = new List<MasterAcademic>();
            list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
            data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();


            return data;

        }




        public ExamToppersListDTO getclassexam(ExamToppersListDTO data)//int IVRMM_Id
        {


            try
            {



                data.classlist = (from a in _ChairmanDashboardContext.School_M_Class
                                  from b in _ChairmanDashboardContext.Exm_Category_ClassDMO
                                  where (a.MI_Id == b.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag.Equals(true) && a.ASMCL_ActiveFlag.Equals(true))
                                  select new ExamToppersListDTO
                                  {
                                      ASMCL_ClassName = a.ASMCL_ClassName,
                                      ASMCL_Id = a.ASMCL_Id
                                  }).Distinct().ToArray();



                data.exmstdlist = (from a in _ChairmanDashboardContext.Exm_Yearly_CategoryDMO
                                   from b in _ChairmanDashboardContext.Exm_Yearly_Category_ExamsDMO
                                   from c in _ChairmanDashboardContext.exammasterDMO

                                   where (a.MI_Id == c.MI_Id && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && a.EYC_Id == b.EYC_Id && b.EME_Id == c.EME_Id && c.EME_ActiveFlag.Equals(true))
                                   select new ExamToppersListDTO
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
        public ExamToppersListDTO getcategory(ExamToppersListDTO data)//int IVRMM_Id
        {


            try
            {

                data.fillcategory = (from a in _ChairmanDashboardContext.Exm_Master_CategoryDMO
                                     from b in _ChairmanDashboardContext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == b.EMCA_Id && b.EYC_ActiveFlg.Equals(true) && a.EMCA_ActiveFlag.Equals(true))


                                     select new ExamToppersListDTO
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


        public ExamToppersListDTO showreport(ExamToppersListDTO data)
        {


            try
            {
                List<ExamToppersListDTO> result1 = new List<ExamToppersListDTO>();
                List<ExamToppersListDTO> result2 = new List<ExamToppersListDTO>();

                if (data.Logintype == "KIOSK" && data.Logintype != "")
                {

                   // var mo_id = _db.Organisation.Where(t => t.MO_Id == data.MO_Id).Select(d => d.MO_Id).FirstOrDefault();

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Chairman_Classwise_Exm_Rank_Kiosk";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MO_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MO_Id
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
                                    result1.Add(new ExamToppersListDTO
                                    {
                                        ASMAY_Id = Convert.ToInt32(dataReader["ASMAY_Id"].ToString()),
                                        ASMCL_Id = Convert.ToInt32(dataReader["ASMCL_Id"].ToString()),
                                        ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString()),
                                        ASMS_Id = Convert.ToInt32(dataReader["ASMS_Id"].ToString()),
                                        ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString()),
                                        AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                        AMST_FirstName = (dataReader["name"].ToString()),
                                        AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString()),
                                        ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                        ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString()),
                                        ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString()),
                                        Class_Rnk = Convert.ToInt32(dataReader["Class_Rnk"].ToString()),
                                        ELP_Flg = dataReader["AMST_Photoname"].ToString(),
                                        MI_Name = dataReader["MI_Name"].ToString()

                                    });
                                    data.classranklist = result1.ToArray();
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
                        cmd.CommandText = "Chairman_Sectionwise_Exm_Rank_Kiosk";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MO_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.MO_Id
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
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.ASMS_Id
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
                                    result2.Add(new ExamToppersListDTO
                                    {
                                        ASMAY_Id = Convert.ToInt32(dataReader["ASMAY_Id"].ToString()),
                                        ASMCL_Id = Convert.ToInt32(dataReader["ASMCL_Id"].ToString()),
                                        ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString()),
                                        ASMS_Id = Convert.ToInt32(dataReader["ASMS_Id"].ToString()),
                                        ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString()),
                                        AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                        AMST_FirstName = (dataReader["name"].ToString()),
                                        AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString()),
                                        ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                        ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString()),
                                        ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString()),
                                        sectionrank = Convert.ToInt32(dataReader["Section_Rnk"].ToString()),
                                        MI_Name = dataReader["MI_Name"].ToString()


                                    });
                                    data.sectionranklist = result2.ToArray();
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
                        cmd.CommandText = "Chairman_Classwise_Exm_Rank";
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
                                    result1.Add(new ExamToppersListDTO
                                    {
                                        ASMAY_Id = Convert.ToInt32(dataReader["ASMAY_Id"].ToString()),
                                        ASMCL_Id = Convert.ToInt32(dataReader["ASMCL_Id"].ToString()),
                                        ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString()),
                                        ASMS_Id = Convert.ToInt32(dataReader["ASMS_Id"].ToString()),
                                        ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString()),
                                        AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                        AMST_FirstName = (dataReader["name"].ToString()),
                                        AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString()),
                                        ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                        ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString()),
                                        ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString()),
                                        Class_Rnk = Convert.ToInt32(dataReader["Class_Rnk"].ToString()),
                                        ELP_Flg = dataReader["AMST_Photoname"].ToString()

                                    });
                                    data.classranklist = result1.ToArray();
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
                        cmd.CommandText = "Chairman_Sectionwise_Exm_Rank";
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
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.ASMS_Id
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
                                    result2.Add(new ExamToppersListDTO
                                    {
                                        ASMAY_Id = Convert.ToInt32(dataReader["ASMAY_Id"].ToString()),
                                        ASMCL_Id = Convert.ToInt32(dataReader["ASMCL_Id"].ToString()),
                                        ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString()),
                                        ASMS_Id = Convert.ToInt32(dataReader["ASMS_Id"].ToString()),
                                        ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString()),
                                        AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                        AMST_FirstName = (dataReader["name"].ToString()),
                                        AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString()),
                                        ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                        ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString()),
                                        ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString()),
                                        sectionrank = Convert.ToInt32(dataReader["Section_Rnk"].ToString()),


                                    });
                                    data.sectionranklist = result2.ToArray();
                                }
                            }
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



        public ExamToppersListDTO getsection(ExamToppersListDTO data)
        {
            try
            {
                data.seclist = (from a in _ChairmanDashboardContext.School_M_Class
                                from b in _ChairmanDashboardContext.Exm_Category_ClassDMO
                                  from c in _ChairmanDashboardContext.School_M_Section 
                                  where (a.MI_Id == b.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag.Equals(true) && a.ASMCL_ActiveFlag.Equals(true) && b.ASMS_Id==c.ASMS_Id && a.ASMCL_Id==data.ASMCL_Id )
                                  select new ExamToppersListDTO
                                  {
                                      ASMC_SectionName = c.ASMC_SectionName,
                                      ASMS_Id = c.ASMS_Id
                                  }).Distinct().ToArray();

            }
            catch (Exception)
            {

                throw;
            }
            return data;
        }

    }
}
