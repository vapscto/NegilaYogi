using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.HOD;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Services
{
    public class HODExamTopperImpl : Interfaces.HODExamTopperInterface
    {

        private static ConcurrentDictionary<string, HODExamTopper_DTO> _login = new ConcurrentDictionary<string, HODExamTopper_DTO>();
        private PortalContext _portalContext;

        public DomainModelMsSqlServerContext _db;
        public HODExamTopperImpl(PortalContext para1, DomainModelMsSqlServerContext para2)
        {
            _portalContext = para1;
            _db = para2;
        }

        public HODExamTopper_DTO Getdetails(HODExamTopper_DTO data)//int IVRMM_Id
        {

            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.yearlist = list.OrderByDescending(t => t.ASMAY_Order).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return data;

        }

        public HODExamTopper_DTO getclassexam(HODExamTopper_DTO data)//int IVRMM_Id
        {
            try
            {
                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();

                data.classlist = (from a in _portalContext.School_M_Class
                                  from b in _portalContext.Exm_Category_ClassDMO                                  
                                  from h in _portalContext.HOD_DMO
                                  from hc in _portalContext.IVRM_HOD_Class_DMO
                                  where (a.MI_Id == b.MI_Id && a.ASMCL_Id == b.ASMCL_Id && h.IHOD_Id == hc.IHOD_Id && hc.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && h.HRME_Id == loginData.SingleOrDefault().Emp_Code && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag.Equals(true) && a.ASMCL_ActiveFlag.Equals(true)  && h.IHOD_Flg == "HOD" && h.IHOD_ActiveFlag == true)
                                  select new HODExamTopper_DTO
                                  {
                                      ASMCL_ClassName = a.ASMCL_ClassName,
                                      ASMCL_Id = a.ASMCL_Id
                                  }).Distinct().ToArray();

                data.exmstdlist = (from a in _portalContext.Exm_Yearly_CategoryDMO
                                   from b in _portalContext.Exm_Yearly_Category_ExamsDMO
                                   from c in _portalContext.exammasterDMO

                                   where (a.MI_Id == c.MI_Id && a.EYC_Id == b.EYC_Id && b.EME_Id == c.EME_Id && c.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && c.EME_ActiveFlag.Equals(true))
                                   select new HODExamTopper_DTO
                                   {
                                       EME_ExamName = c.EME_ExamName,
                                       EME_Id = c.EME_Id
                                   }).Distinct().OrderBy(t => t.EME_Id).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public HODExamTopper_DTO getcategory(HODExamTopper_DTO data)//int IVRMM_Id
        {
            try
            {
                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();

                List<long> assignEmpclass = new List<long>();
                List<long> emca_idss = new List<long>();
                var assignclass = (from a in _portalContext.HOD_DMO
                                   from b in _portalContext.IVRM_HOD_Class_DMO
                                   where (a.IHOD_Id == b.IHOD_Id && a.HRME_Id == loginData.FirstOrDefault().Emp_Code && a.MI_Id == data.MI_Id && a.IHOD_ActiveFlag == true)
                                   select new HODExamTopper_DTO
                                   {
                                       ASMCL_Id=b.ASMCL_Id,
                                   }).Distinct().ToList();

                if (assignclass.Count > 0)
                {
                    foreach (var classid in assignclass)
                    {
                        assignEmpclass.Add(classid.ASMCL_Id);
                    }
                }

                var categid = (from t in _portalContext.Exm_Category_ClassDMO
                               where (t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && assignEmpclass.Contains(t.ASMCL_Id))
                               select new HODExamTopper_DTO
                               {
                                   EMCA_Id = t.EMCA_Id,
                               }).Distinct().ToList();

                if (categid.Count > 0)
                {
                    foreach (var emca in categid)
                    {
                        emca_idss.Add(emca.EMCA_Id);
                    }
                }
                

                data.fillcategory = (from a in _portalContext.Exm_Master_CategoryDMO
                                     from b in _portalContext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == b.EMCA_Id && emca_idss.Contains(b.EMCA_Id) && b.EYC_ActiveFlg.Equals(true) && a.EMCA_ActiveFlag.Equals(true))
                                     select new HODExamTopper_DTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName
                                     }).Distinct().OrderBy(t => t.EMCA_Id).ToArray();




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public HODExamTopper_DTO showreport(HODExamTopper_DTO data)
        {


            try
            {
                List<HODExamTopper_DTO> result1 = new List<HODExamTopper_DTO>();
                List<HODExamTopper_DTO> result2 = new List<HODExamTopper_DTO>();
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
                                result1.Add(new HODExamTopper_DTO
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
                                result2.Add(new HODExamTopper_DTO
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public HODExamTopper_DTO getsection(HODExamTopper_DTO data)
        {
            try
            {
                data.seclist = (from a in _portalContext.School_M_Class
                                from b in _portalContext.Exm_Category_ClassDMO
                                from c in _portalContext.School_M_Section
                                where (a.MI_Id == b.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag.Equals(true) && a.ASMCL_ActiveFlag.Equals(true) && b.ASMS_Id == c.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id)
                                select new HODExamTopper_DTO
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
