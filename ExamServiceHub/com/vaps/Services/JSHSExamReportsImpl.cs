using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam;
using System.Runtime.Serialization;


namespace ExamServiceHub.com.vaps.Services
{
    public class JSHSExamReportsImpl : Interfaces.JSHSExamReportsInterface
    {
        public ExamContext _context;
        public JSHSExamReportsImpl(ExamContext _cont)
        {
            _context = _cont;
        }
        public JSHSExamReportsDTO Getdetails(JSHSExamReportsDTO data)
        {
            try
            {
                data.getyearlist = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO get_classes(JSHSExamReportsDTO data)
        {
            try
            {
                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.Roleid)
                                      select new JSHSExamReportsDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.Userid))
                                     select new JSHSExamReportsDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _context.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {

                        data.getclasslist = (from a in _context.ClassTeacherMappingDMO
                                             from b in _context.AdmissionClass
                                             from c in _context.AcademicYear
                                             where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                             && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                             select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                    }
                }
                else
                {
                    data.getclasslist = (from a in _context.Exm_Category_ClassDMO
                                         from b in _context.AdmissionClass
                                         from c in _context.AcademicYear
                                         where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                         && a.ECAC_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                         select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO get_sections(JSHSExamReportsDTO data)
        {
            try
            {
                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.Roleid)
                                      select new JSHSExamReportsDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.Userid))
                                     select new JSHSExamReportsDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _context.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {

                        data.getsectionlist = (from a in _context.ClassTeacherMappingDMO
                                               from b in _context.School_M_Section
                                               from c in _context.AcademicYear
                                               from d in _context.AdmissionClass
                                               where (a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                               && a.ASMCL_Id == data.ASMCL_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true
                                               && b.ASMC_ActiveFlag == 1)
                                               select b).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                    }
                }
                else
                {
                    data.getsectionlist = (from a in _context.Exm_Category_ClassDMO
                                           from b in _context.School_M_Section
                                           from c in _context.AcademicYear
                                           from d in _context.AdmissionClass
                                           where (a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ASMCL_Id == data.ASMCL_Id && a.ECAC_ActiveFlag == true && b.ASMC_ActiveFlag == 1 && a.ASMCL_Id == data.ASMCL_Id)
                                           select b).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO get_students_category_grade(JSHSExamReportsDTO data)
        {
            try
            {
                List<int?> ids = new List<int?>();
                ids.Add(0);
                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");
                sol.Add("L");
                sol.Add("D");

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                data.gettermlist = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().OrderBy(a => a.ECT_TermName).ToArray();


                data.getgradelist = (from a in _context.Exm_Yearly_CategoryDMO
                                     from b in _context.Exm_Yearly_Category_ExamsDMO
                                     from c in _context.Exm_Master_GradeDMO
                                     where (a.EYC_Id == b.EYC_Id && b.EMGR_Id == c.EMGR_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                                     && a.ASMAY_Id == data.ASMAY_Id)
                                     select c).Distinct().ToArray();

                data.getgradetermlist = (from a in _context.CCE_Exam_M_TermsDMO
                                         from c in _context.Exm_Master_GradeDMO
                                         where (a.EMGR_Id == c.EMGR_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                                         && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true)
                                         select c).Distinct().ToArray();


                data.getstudentlist = (from a in _context.School_Adm_Y_StudentDMO
                                       from b in _context.Adm_M_Student
                                       from c in _context.AcademicYear
                                       from d in _context.AdmissionClass
                                       from e in _context.School_M_Section
                                       where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                       && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && ids.Contains(a.AMAY_ActiveFlag)
                                       && sol.Contains(b.AMST_SOL) && ids.Contains(b.AMST_ActiveFlag) && b.MI_Id == data.MI_Id)
                                       select new JSHSExamReportsDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           studentname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName)
                                           + (b.AMST_MiddleName == null || b.AMST_MiddleName == "" || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName)
                                           + (b.AMST_LastName == null || b.AMST_LastName == "" || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)
                                           + (b.AMST_AdmNo == null ? "" : " :" + b.AMST_AdmNo)).Trim()
                                       }).Distinct().OrderBy(a => a.studentname).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO get_Exam_grade(JSHSExamReportsDTO data)
        {
            try
            {
                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var eycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                && a.EYC_ActiveFlg == true).ToList();

                var getexam = (from a in _context.Exm_Yearly_CategoryDMO
                               from b in _context.Exm_Yearly_Category_ExamsDMO
                               from c in _context.masterexam
                               where (a.EYC_Id == b.EYC_Id && b.EME_Id == c.EME_Id && a.EYC_ActiveFlg == true && b.EYCE_ActiveFlg == true
                               && c.EME_ActiveFlag == true && c.MI_Id == data.MI_Id && a.EYC_Id == eycid.FirstOrDefault().EYC_Id
                               && b.EYC_Id == eycid.FirstOrDefault().EYC_Id && a.ASMAY_Id == data.ASMAY_Id)
                               select c).Distinct().OrderBy(a => a.EME_ExamOrder).ToList();

                data.getexam = getexam.ToArray();

                data.getgradelist = (from a in _context.Exm_Yearly_CategoryDMO
                                     from b in _context.Exm_Yearly_Category_ExamsDMO
                                     from c in _context.Exm_Master_GradeDMO
                                     where (a.EYC_Id == b.EYC_Id && b.EMGR_Id == c.EMGR_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                                     && a.ASMAY_Id == data.ASMAY_Id)
                                     select c).Distinct().ToArray();

                data.getallgrade = _context.Exm_Master_GradeDMO.Where(a => a.MI_Id == data.MI_Id && a.EMGR_ActiveFlag == true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO get_Exam_group(JSHSExamReportsDTO data)
        {
            try
            {
                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
               && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var eycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                && a.EYC_ActiveFlg == true).ToList();


                if (data.reporttype == "indi")
                {
                    var getexam = (from a in _context.Exm_Yearly_CategoryDMO
                                   from b in _context.Exm_Yearly_Category_ExamsDMO
                                   from c in _context.masterexam
                                   where (a.EYC_Id == b.EYC_Id && b.EME_Id == c.EME_Id && a.EYC_ActiveFlg == true && b.EYCE_ActiveFlg == true
                                   && c.EME_ActiveFlag == true && c.MI_Id == data.MI_Id && a.EYC_Id == eycid.FirstOrDefault().EYC_Id
                                   && b.EYC_Id == eycid.FirstOrDefault().EYC_Id && a.ASMAY_Id == data.ASMAY_Id)
                                   select c).Distinct().OrderBy(a => a.EME_ExamOrder).ToList();

                    data.getexam = getexam.ToArray();
                }
                else if (data.reporttype == "groupwise")
                {
                    data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == eycid.FirstOrDefault().EYC_Id
                                            && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,

                                            }).Distinct().OrderBy(a => a.EMPG_GroupName).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO get_exam(JSHSExamReportsDTO data)
        {
            try
            {
                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var eycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                && a.EYC_ActiveFlg == true).ToList();

                var getexam = (from a in _context.Exm_Yearly_CategoryDMO
                               from b in _context.Exm_Yearly_Category_ExamsDMO
                               from c in _context.masterexam
                               where (a.EYC_Id == b.EYC_Id && b.EME_Id == c.EME_Id && a.EYC_ActiveFlg == true && b.EYCE_ActiveFlg == true && c.EME_ActiveFlag == true
                               && c.MI_Id == data.MI_Id && a.EYC_Id == eycid.FirstOrDefault().EYC_Id && b.EYC_Id == eycid.FirstOrDefault().EYC_Id
                               && a.ASMAY_Id == data.ASMAY_Id)
                               select c).Distinct().OrderBy(a => a.EME_ExamOrder).ToList();

                data.getexam = getexam.ToArray();

                data.getsubjects = (from a in _context.Exm_Yearly_CategoryDMO
                                    from b in _context.Exm_Yearly_Category_GroupDMO
                                    from c in _context.Exm_Yearly_Category_Group_SubjectsDMO
                                    from d in _context.IVRM_School_Master_SubjectsDMO
                                    where (a.EYC_Id == b.EYC_Id && b.EYCG_Id == c.EYCG_Id && c.ISMS_Id == d.ISMS_Id && a.EYC_ActiveFlg == true
                                    && b.EYCG_ActiveFlg == true && c.EYCGS_ActiveFlg == true && d.ISMS_ActiveFlag == 1 && a.EYC_Id == eycid.FirstOrDefault().EYC_Id
                                    && d.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select d).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

                data.getgradelist = (from a in _context.Exm_Yearly_CategoryDMO
                                     from b in _context.Exm_Yearly_Category_ExamsDMO
                                     from c in _context.Exm_Master_GradeDMO
                                     where (a.EYC_Id == b.EYC_Id && b.EMGR_Id == c.EMGR_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                                     && a.ASMAY_Id == data.ASMAY_Id)
                                     select c).Distinct().ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 3
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = 0
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesportsdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //added New 

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BBHS_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '5' });
                    // cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO GetStudentDetails(JSHSExamReportsDTO data)
        {
            try
            {
                List<int?> ids = new List<int?>();
                ids.Add(0);
                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");
                sol.Add("L");
                sol.Add("D");


                data.getstudentlist = (from a in _context.School_Adm_Y_StudentDMO
                                       from b in _context.Adm_M_Student
                                       from c in _context.AcademicYear
                                       from d in _context.AdmissionClass
                                       from e in _context.School_M_Section
                                       where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                       && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && ids.Contains(a.AMAY_ActiveFlag)
                                       && sol.Contains(b.AMST_SOL) && ids.Contains(b.AMST_ActiveFlag) && b.MI_Id == data.MI_Id)
                                       select new JSHSExamReportsDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           studentname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName)
                                           + (b.AMST_MiddleName == null || b.AMST_MiddleName == "" || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName)
                                           + (b.AMST_LastName == null || b.AMST_LastName == "" || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)
                                           + (b.AMST_AdmNo == null ? "" : " :" + b.AMST_AdmNo)).Trim()
                                       }).Distinct().OrderBy(a => a.studentname).ToArray();


                if (data.flagtype == "NDSJrKGSrKg" || data.flagtype == "Exam")
                {
                    var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                    var eycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                    && a.EYC_ActiveFlg == true).ToList();

                    var getexam = (from a in _context.Exm_Yearly_CategoryDMO
                                   from b in _context.Exm_Yearly_Category_ExamsDMO
                                   from c in _context.masterexam
                                   where (a.EYC_Id == b.EYC_Id && b.EME_Id == c.EME_Id && a.EYC_ActiveFlg == true && b.EYCE_ActiveFlg == true
                                   && c.EME_ActiveFlag == true && c.MI_Id == data.MI_Id && a.EYC_Id == eycid.FirstOrDefault().EYC_Id
                                   && b.EYC_Id == eycid.FirstOrDefault().EYC_Id && a.ASMAY_Id == data.ASMAY_Id)
                                   select c).Distinct().OrderBy(a => a.EME_ExamOrder).ToList();

                    data.getexam = getexam.ToArray();

                    //data.getgradelist = (from a in _context.Exm_Yearly_CategoryDMO
                    //                     from b in _context.Exm_Yearly_Category_ExamsDMO
                    //                     from c in _context.Exm_Master_GradeDMO
                    //                     where (a.EYC_Id == b.EYC_Id && b.EMGR_Id == c.EMGR_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                    //                     && a.ASMAY_Id == data.ASMAY_Id)
                    //                     select c).Distinct().ToArray();
                    data.getgradelist = _context.Exm_Master_GradeDMO.Where(R => R.MI_Id == data.MI_Id && R.EMGR_ActiveFlag == true).Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //StudentDetailspramotion
        public JSHSExamReportsDTO StudentDetailspramotion(JSHSExamReportsDTO data)
        {
            try
            {
                List<int?> ids = new List<int?>();
                ids.Add(0);
                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");
                sol.Add("L");
                sol.Add("D");

                data.getstudentlist = (from a in _context.School_Adm_Y_StudentDMO
                                       from b in _context.Adm_M_Student
                                       from c in _context.AcademicYear
                                       from d in _context.AdmissionClass
                                       from e in _context.School_M_Section
                                       from f in _context.Exm_Student_MP_PromotionDMO
                                       where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                       && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && ids.Contains(a.AMAY_ActiveFlag)
                                       && sol.Contains(b.AMST_SOL) && ids.Contains(b.AMST_ActiveFlag) && b.MI_Id == data.MI_Id && a.AMST_Id == f.AMST_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id)
                                       select new JSHSExamReportsDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           studentname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName)
                                           + (b.AMST_MiddleName == null || b.AMST_MiddleName == "" || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName)
                                           + (b.AMST_LastName == null || b.AMST_LastName == "" || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)
                                           + (b.AMST_AdmNo == null ? "" : " :" + b.AMST_AdmNo)).Trim()
                                       }).Distinct().OrderBy(a => a.studentname).ToArray();
                // .OrderByDescending(a => b.AMST_Gender == "M") // Order by gender (males first, then females).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        // Individual  Term Wise Report JSHS
        public JSHSExamReportsDTO get_individualtermreport(JSHSExamReportsDTO data)
        {
            try
            {
                data.getgradedetails = (from a in _context.Exm_Master_GradeDMO
                                        from b in _context.Exm_Master_Grade_DetailsDMO
                                        where (a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.EMGR_Id == data.EMGR_Id)
                                        select b).Distinct().OrderByDescending(a => a.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ECT_Id == data.ECT_Id).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true && b.ECT_Id == data.ECT_Id)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue

                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 2 });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SPORTS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 3
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesportsdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE TERM REMARKS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 5
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisetermwisedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // GET STUDENT WISE EXAM MARKS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_CCE_Term_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Multiple Term Report JSHS
        public JSHSExamReportsDTO get_reportdetails(JSHSExamReportsDTO data)
        {
            try
            {
                data.getgradedetails = (from a in _context.Exm_Master_GradeDMO
                                        from b in _context.Exm_Master_Grade_DetailsDMO
                                        where (a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.EMGR_Id == data.EMGR_Id)
                                        select b).Distinct().OrderByDescending(a => a.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();


                List<long> termid = new List<long>();

                foreach (var c in data.termlist)
                {
                    termid.Add(c.ECT_Id);
                }

                var termidnew = "";

                for (int k = 0; k < data.termlist.Length; k++)
                {
                    if (k == 0)
                    {
                        termidnew = data.termlist[k].ECT_Id.ToString();
                    }
                    else
                    {
                        termidnew = termidnew + ',' + data.termlist[k].ECT_Id.ToString();
                    }
                }

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && termid.Contains(a.ECT_Id)).ToArray();


                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true && termid.Contains(b.ECT_Id))
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue

                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = termidnew
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = termidnew
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SPORTS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 3
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = termidnew
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesportsdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = termidnew
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE TREM WISE REMARKS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 5
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = termidnew
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisetermwisedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // GET STUDENT WISE EXAM MARKS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_CCE_Term_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                        SqlDbType.VarChar)
                    {
                        Value = termidnew
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Sneha sagar Term Wise Report JSHS
        public JSHSExamReportsDTO getss_reportdetails(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                List<long> termid = new List<long>();

                foreach (var c in data.termlist)
                {
                    termid.Add(c.ECT_Id);
                }

                var termidnew = "";

                for (int k = 0; k < data.termlist.Length; k++)
                {
                    if (k == 0)
                    {
                        termidnew = data.termlist[k].ECT_Id.ToString();
                    }
                    else
                    {
                        termidnew = termidnew + ',' + data.termlist[k].ECT_Id.ToString();
                    }
                }

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && termid.Contains(a.ECT_Id)).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true && termid.Contains(b.ECT_Id))
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamDescription = c.EME_ExamCode,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue,
                                               ECTEX_NotApplToTotalFlg = b.ECTEX_NotApplToTotalFlg
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getgradedetails = (from a in _context.Exm_Master_GradeDMO
                                        from b in _context.Exm_Master_Grade_DetailsDMO
                                        where (a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.EMGR_Id == data.EMGR_Id)
                                        select b).Distinct().OrderByDescending(a => a.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();


                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SS_Exam_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SS_Exam_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SS_Exam_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 3
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // GET STUDENT WISE EXAM MARKS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SS_Exam_CCE_Term_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                        SqlDbType.VarChar)
                    {
                        Value = termidnew
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Subject With Multiple Exam Report JSHS
        public JSHSExamReportsDTO get_cumulativereportdetails(JSHSExamReportsDTO data)
        {
            try
            {
                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var eycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                && a.EYC_ActiveFlg == true).ToList();

                List<long> emeid = new List<long>();

                foreach (var c in data.examlist)
                {
                    emeid.Add(c.EME_Id);
                }

                var examidnew = "";

                for (int k = 0; k < data.examlist.Length; k++)
                {
                    if (k == 0)
                    {
                        examidnew = data.examlist[k].EME_Id.ToString();
                    }
                    else
                    {
                        examidnew = examidnew + ',' + data.examlist[k].EME_Id.ToString();
                    }
                }

                data.getselectedexamlist = _context.masterexam.Where(a => a.MI_Id == data.MI_Id && emeid.Contains(a.EME_Id)).OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getgradedetails = (from a in _context.Exm_Master_GradeDMO
                                        from b in _context.Exm_Master_Grade_DetailsDMO
                                        where (a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.EMGR_Id == data.EMGR_Id)
                                        select b).Distinct().OrderByDescending(a => a.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();

                data.getexamwisesubsubjectlist = (from a in _context.Exm_Yearly_CategoryDMO
                                                  from b in _context.Exm_Yearly_Category_ExamsDMO
                                                  from c in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                  from d in _context.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                  from e in _context.mastersubsubject
                                                  from f in _context.masterexam
                                                  where (a.EYC_Id == b.EYC_Id && b.EYCE_Id == c.EYCE_Id && c.EYCES_Id == d.EYCES_Id && d.EMSS_Id == e.EMSS_Id
                                                  && b.EME_Id == f.EME_Id && a.EYC_ActiveFlg == true && b.EYCE_ActiveFlg == true && c.EYCES_ActiveFlg == true
                                                  && d.EYCESSS_ActiveFlg == true && e.EMSS_ActiveFlag == true && a.EYC_Id == eycid.FirstOrDefault().EYC_Id
                                                  && a.ASMAY_Id == data.ASMAY_Id && c.ISMS_Id == data.ISMS_Id && emeid.Contains(b.EME_Id))
                                                  select new JSHSExamReportsDTO
                                                  {
                                                      EME_Id = b.EME_Id,
                                                      EMSS_Id = d.EMSS_Id,
                                                      EMSS_SubSubjectName = e.EMSS_SubSubjectName,
                                                      EMSS_Order = e.EMSS_Order,
                                                      EME_ExamOrder = f.EME_ExamOrder,
                                                      EYCESSS_Grade = "G",
                                                      EYCESSS_MaxMarks = d.EYCESSS_MaxMarks
                                                  }).Distinct().OrderBy(a => a.EME_ExamOrder).ThenBy(a => a.EMSS_Order).ToArray();


                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Cumulative_Report_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = examidnew
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 1
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getexamsubjectwisereport = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Cumulative_Report_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = examidnew
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
                    SqlDbType.VarChar)
                    {
                        Value = 2
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getgradereport = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Term Wise Cumulative Report JSHS
        public JSHSExamReportsDTO get_termcumulative_reportdetails(JSHSExamReportsDTO data)
        {
            try
            {

                data.getgradedetails = (from a in _context.Exm_Master_GradeDMO
                                        from b in _context.Exm_Master_Grade_DetailsDMO
                                        where (a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.EMGR_Id == data.EMGR_Id)
                                        select b).Distinct().OrderByDescending(a => a.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();


                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();


                List<long> termid = new List<long>();

                foreach (var c in data.termlist)
                {
                    termid.Add(c.ECT_Id);
                }

                var termidnew = "";

                for (int k = 0; k < data.termlist.Length; k++)
                {
                    if (k == 0)
                    {
                        termidnew = data.termlist[k].ECT_Id.ToString();
                    }
                    else
                    {
                        termidnew = termidnew + ',' + data.termlist[k].ECT_Id.ToString();
                    }
                }

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && termid.Contains(a.ECT_Id)).ToArray();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_CCE_Term_Cumulative_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                   SqlDbType.VarChar)
                    {
                        Value = termidnew
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id",
                 SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@checkoruncheckflag",
                 SqlDbType.VarChar)
                    {
                        Value = data.checkoruncheckflag
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getexamsubjectwisereport = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Multiple Exam Cumulative Report JSHS
        public JSHSExamReportsDTO get_multipleexam_reportdetails(JSHSExamReportsDTO data)
        {
            try
            {
                data.getgradedetails = (from a in _context.Exm_Master_GradeDMO
                                        from b in _context.Exm_Master_Grade_DetailsDMO
                                        where (a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.EMGR_Id == data.EMGR_Id)
                                        select b).Distinct().OrderByDescending(a => a.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();


                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();


                List<long> examid = new List<long>();

                foreach (var c in data.examlist)
                {
                    examid.Add(c.EME_Id);
                }

                var examidnew = "";

                for (int k = 0; k < data.examlist.Length; k++)
                {
                    if (k == 0)
                    {
                        examidnew = data.examlist[k].EME_Id.ToString();
                    }
                    else
                    {
                        examidnew = examidnew + ',' + data.examlist[k].EME_Id.ToString();
                    }
                }

                data.getexamdetails = _context.masterexam.Where(a => a.MI_Id == data.MI_Id && examid.Contains(a.EME_Id)).OrderBy(a => a.EME_ExamOrder).ToArray();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Multiple_Exam_Cumulative_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getmultipleexamcumulativereport = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Individual Exam Wise Progress card report JSHS
        public async Task<JSHSProgressCardReportDTO> saveddata(JSHSProgressCardReportDTO data)
        {
            try
            {
                List<int?> ids = new List<int?>();
                ids.Add(0);
                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");
                sol.Add("L");
                sol.Add("D");

                data.instname = _context.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSProgressCardReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                List<JSHSProgressCardReportDTO> result = new List<JSHSProgressCardReportDTO>();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_get_BB_Exam_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.ASMS_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.EME_Id)
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
                                result.Add(new JSHSProgressCardReportDTO
                                {
                                    ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                    ESTMPS_ObtainedGrade = (dataReader["ESTMPS_ObtainedGrade"].ToString() == null || dataReader["ESTMPS_ObtainedGrade"].ToString() == "" ? "" : dataReader["ESTMPS_ObtainedGrade"].ToString()),
                                    ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                    EME_ExamName = (dataReader["EME_ExamName"].ToString() == null || dataReader["EME_ExamName"].ToString() == "" ? "" : dataReader["EME_ExamName"].ToString()),
                                    ASMCL_ClassName = (dataReader["ASMCL_ClassName"].ToString() == null || dataReader["ASMCL_ClassName"].ToString() == "" ? "" : dataReader["ASMCL_ClassName"].ToString()),
                                    ASMC_SectionName = (dataReader["ASMC_SectionName"].ToString() == null || dataReader["ASMC_SectionName"].ToString() == "" ? "" : dataReader["ASMC_SectionName"].ToString()),

                                    AMST_FatherName = (dataReader["AMST_FatherName"].ToString() == null || dataReader["AMST_FatherName"].ToString() == "" ? "" : dataReader["AMST_FatherName"].ToString()),
                                    AMST_MotherName = (dataReader["AMST_MotherName"].ToString() == null || dataReader["AMST_MotherName"].ToString() == "" ? "" : dataReader["AMST_MotherName"].ToString()),

                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = ((dataReader["AMST_FirstName"].ToString() == null ? " " : dataReader["AMST_FirstName"].ToString()) + " " + (dataReader["AMST_MiddleName"].ToString() == null ? " " : dataReader["AMST_MiddleName"].ToString()) + " " + (dataReader["AMST_LastName"].ToString() == null ? " " : dataReader["AMST_LastName"].ToString())).Trim(),
                                    AMST_DOB = Convert.ToDateTime(dataReader["AMST_DOB"].ToString() == null || dataReader["AMST_DOB"].ToString() == "" ? "" : dataReader["AMST_DOB"].ToString()),
                                    AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),
                                    ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                                    ESTMPS_ClassAverage = Convert.ToDecimal(dataReader["ESTMPS_ClassAverage"].ToString() == null || dataReader["ESTMPS_ClassAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassAverage"].ToString()),
                                    ESTMPS_SectionAverage = Convert.ToDecimal(dataReader["ESTMPS_SectionAverage"].ToString() == null || dataReader["ESTMPS_SectionAverage"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionAverage"].ToString()),
                                    ESTMPS_ClassHighest = Convert.ToDecimal(dataReader["ESTMPS_ClassHighest"].ToString() == null || dataReader["ESTMPS_ClassHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_ClassHighest"].ToString()),
                                    ESTMPS_SectionHighest = Convert.ToDecimal(dataReader["ESTMPS_SectionHighest"].ToString() == null || dataReader["ESTMPS_SectionHighest"].ToString() == "" ? "0" : dataReader["ESTMPS_SectionHighest"].ToString()),
                                    ISMS_SubjectCode = (dataReader["ISMS_SubjectCode"].ToString() == null || dataReader["ISMS_SubjectCode"].ToString() == "" ? "" : dataReader["ISMS_SubjectCode"].ToString()),
                                    EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),
                                    EYCES_MaxMarks = Convert.ToDecimal(dataReader["EYCES_MaxMarks"].ToString() == null || dataReader["EYCES_MaxMarks"].ToString() == "" ? "0" : dataReader["EYCES_MaxMarks"].ToString()),
                                    EYCES_MinMarks = Convert.ToDecimal(dataReader["EYCES_MinMarks"].ToString() == null || dataReader["EYCES_MinMarks"].ToString() == "" ? "0" : dataReader["EYCES_MinMarks"].ToString()),
                                    EMGR_Id = Convert.ToInt32(dataReader["EMGR_Id"].ToString()),
                                    classheld = Convert.ToDecimal(dataReader["ASA_ClassHeld"].ToString() == null || dataReader["ASA_ClassHeld"].ToString() == "" ? "0" : dataReader["ASA_ClassHeld"].ToString()),
                                    classatt = Convert.ToDecimal(dataReader["ASA_Class_Attended"].ToString() == null || dataReader["ASA_Class_Attended"].ToString() == "" ? "0" : dataReader["ASA_Class_Attended"].ToString()),
                                    graderemark = (dataReader["EMGD_Remarks"].ToString() == null || dataReader["EMGD_Remarks"].ToString() == "" ? "0" : dataReader["EMGD_Remarks"].ToString()),

                                    ESTMP_TotalObtMarks = Convert.ToDecimal(dataReader["ESTMP_TotalObtMarks"].ToString() == null || dataReader["ESTMP_TotalObtMarks"].ToString() == "" ? "0" : dataReader["ESTMP_TotalObtMarks"].ToString()),
                                    ESTMP_Percentage = Convert.ToDecimal(dataReader["ESTMP_Percentage"].ToString() == null || dataReader["ESTMP_Percentage"].ToString() == "" ? "0" : dataReader["ESTMP_Percentage"].ToString()),
                                    ESTMP_TotalGrade = (dataReader["ESTMP_TotalGrade"].ToString() == null || dataReader["ESTMP_TotalGrade"].ToString() == "" ? "" : dataReader["ESTMP_TotalGrade"].ToString()),
                                    ESTMP_ClassRank = Convert.ToInt16(dataReader["ESTMP_ClassRank"].ToString() == null || dataReader["ESTMP_ClassRank"].ToString() == "" ? "" : dataReader["ESTMP_ClassRank"].ToString()),
                                    ESTMP_SectionRank = Convert.ToInt16(dataReader["ESTMP_SectionRank"].ToString() == null || dataReader["ESTMP_SectionRank"].ToString() == "" ? "" : dataReader["ESTMP_SectionRank"].ToString()),
                                    ESTMP_TotalGradeRemark = (dataReader["ESTMP_TotalGradeRemark"].ToString() == null || dataReader["ESTMP_TotalGradeRemark"].ToString() == "" ? "" : dataReader["ESTMP_TotalGradeRemark"].ToString()),
                                    ESTMP_TotalMaxMarks = Convert.ToDecimal(dataReader["ESTMP_TotalMaxMarks"].ToString() == null || dataReader["ESTMP_TotalMaxMarks"].ToString() == "0" ? "" : dataReader["ESTMP_TotalMaxMarks"].ToString()),
                                    EYCES_SubjectOrder = Convert.ToInt16(dataReader["EYCES_SubjectOrder"].ToString() == null || dataReader["EYCES_SubjectOrder"].ToString() == "" ? "" : dataReader["EYCES_SubjectOrder"].ToString()),
                                    EYCES_MarksDisplayFlg = Convert.ToBoolean(dataReader["EYCES_MarksDisplayFlg"].ToString()),
                                    EYCES_GradeDisplayFlg = Convert.ToBoolean(dataReader["EYCES_GradeDisplayFlg"].ToString()),
                                    ESTMP_Result = (dataReader["ESTMP_Result"].ToString() == null || dataReader["ESTMP_Result"].ToString() == "" ? "" : dataReader["ESTMP_Result"].ToString())
                                });

                                data.savelist = result.OrderBy(t => t.EYCES_SubjectOrder).ToList();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }

                var from_date = (from a in _context.Exm_Category_ClassDMO
                                 from b in _context.Exm_Yearly_CategoryDMO
                                 from c in _context.Exm_Yearly_Category_ExamsDMO
                                 where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id && b.ASMAY_Id == data.ASMAY_Id)
                                 select c.EYCE_AttendanceFromDate).FirstOrDefault();
                var to_date = (from a in _context.Exm_Category_ClassDMO
                               from b in _context.Exm_Yearly_CategoryDMO
                               from c in _context.Exm_Yearly_Category_ExamsDMO
                               where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id && b.ASMAY_Id == data.ASMAY_Id)
                               select c.EYCE_AttendanceToDate).Max();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "adm_exam_student_attendance_details";
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

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@from",
                        SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to",
                        SqlDbType.Date)
                    {
                        Value = to_date
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                            //data.savelist = retObject.ToArray();

                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.savelisttot = _context.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();

                data.subjlist = data.savelist.Distinct<JSHSProgressCardReportDTO>(new progressEqualityComparerjhs()).OrderBy(t => t.EYCES_SubjectOrder).ToArray();


                List<int> grade = new List<int>();
                foreach (JSHSProgressCardReportDTO x in data.subjlist)
                {
                    grade.Add(x.EMGR_Id);
                }

                data.grade_details = (from a in _context.Exm_Master_GradeDMO
                                      from b in _context.Exm_Master_Grade_DetailsDMO
                                      where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                      select b
                                     ).Distinct().ToArray();

                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true).Distinct().ToArray();

                data.getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                          from b in _context.Adm_M_Student
                                          from c in _context.AdmissionClass
                                          from d in _context.School_M_Section
                                          from e in _context.AcademicYear
                                          where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                          && ids.Contains(a.AMAY_ActiveFlag) && sol.Contains(b.AMST_SOL) && ids.Contains(b.AMST_ActiveFlag)
                                          && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMS_Id == data.ASMS_Id)
                                          select new JSHSProgressCardReportDTO
                                          {
                                              AMST_Id = a.AMST_Id,
                                              photoname = b.AMST_Photoname
                                          }).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        // IX Term Report Promotion Setting WIse JSHS
        public JSHSExamReportsDTO getixtermreport(JSHSExamReportsDTO data)
        {
            try
            {
                data.getgradedetails = (from a in _context.Exm_Master_GradeDMO
                                        from b in _context.Exm_Master_Grade_DetailsDMO
                                        where (a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.EMGR_Id == data.EMGR_Id)
                                        select b).Distinct().OrderByDescending(a => a.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ECT_Id == data.ECT_Id).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true && b.ECT_Id == data.ECT_Id)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                // promotion Details


                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToList();

                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid.FirstOrDefault().EYC_Id && a.MI_Id == data.MI_Id
                                        && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPG_DistplayName = c.EMPSG_DisplayName,
                                            EMPSG_PercentValue = a.EMP_MarksPerFlg == "M" ? c.EMPSG_MarksValue : c.EMPSG_PercentValue,
                                        }).Distinct().OrderBy(a => a.EMPG_GroupName).ToArray();

                data.getpromotionmarksdetails = _context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToArray();



                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SPORTS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 3
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesportsdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE TERM REMARKS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 5
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisetermwisedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // GET STUDENT WISE EXAM MARKS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_CCE_Exam_IX_Term_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Individual X Term Wise Report JSHS
        public JSHSExamReportsDTO get_individualtermxreport(JSHSExamReportsDTO data)
        {
            try
            {
                data.getgradedetails = (from a in _context.Exm_Master_GradeDMO
                                        from b in _context.Exm_Master_Grade_DetailsDMO
                                        where (a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.EMGR_Id == data.EMGR_Id)
                                        select b).Distinct().OrderByDescending(a => a.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ECT_Id == data.ECT_Id).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true && b.ECT_Id == data.ECT_Id)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue

                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SPORTS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 3
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesportsdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 4
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE TERM REMARKS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_Term_Skills_Activites_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 5
                    });
                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisetermwisedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // GET STUDENT WISE EXAM MARKS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "JSHS_Exam_Individual_CCE10_Term_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ECT_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ECT_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.EMGR_Id
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //  getmultiple_exam_cumulative_Calcutta
        public JSHSExamReportsDTO getmultiple_exam_cumulative_Calcutta(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).Distinct().ToArray();
                var examidnew = "";
                for (int k = 0; k < data.examlist.Length; k++)
                {
                    if (k == 0)
                    {
                        examidnew = data.examlist[k].EME_Id.ToString();
                    }
                    else
                    {
                        examidnew = examidnew + ',' + data.examlist[k].EME_Id.ToString();
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Mark_cumalative_report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 100000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getcumulativereportdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Multiple_Exam_Cumulative_Calcutta";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 100000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getsubjectslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Multiple_Exam_Cumulative_Calcutta";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 100000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "3" });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //difrren
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Multiple_Exam_Cumulative_Calcutta";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 100000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.gettermdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // getmultiple_exam_cumulative_report  JSHS
        public JSHSExamReportsDTO getmultiple_exam_cumulative_report(JSHSExamReportsDTO data)
        {
            try
            {
                if (data.ECTEX_NotApplToTotalFlg == true)
                {
                    List<long> examid = new List<long>();

                    foreach (var c in data.examlist)
                    {
                        examid.Add(c.EME_Id);
                    }

                    var examidnew = "";

                    for (int k = 0; k < data.examlist.Length; k++)
                    {
                        if (k == 0)
                        {
                            examidnew = data.examlist[k].EME_Id.ToString();
                        }
                        else
                        {
                            examidnew = examidnew + ',' + data.examlist[k].EME_Id.ToString();
                        }
                    }

                    data.getexamdetails = _context.masterexam.Where(a => a.MI_Id == data.MI_Id && examid.Contains(a.EME_Id)).OrderBy(a => a.EME_ExamOrder).ToArray();
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exm_Multiple_Exam_Cumulative_Report_SMS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                        if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getcumulativereportdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }
                else
                {


                    data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                    List<long> examid = new List<long>();

                    foreach (var c in data.examlist)
                    {
                        examid.Add(c.EME_Id);
                    }

                    var examidnew = "";

                    for (int k = 0; k < data.examlist.Length; k++)
                    {
                        if (k == 0)
                        {
                            examidnew = data.examlist[k].EME_Id.ToString();
                        }
                        else
                        {
                            examidnew = examidnew + ',' + data.examlist[k].EME_Id.ToString();
                        }
                    }

                    data.getexamdetails = _context.masterexam.Where(a => a.MI_Id == data.MI_Id && examid.Contains(a.EME_Id)).OrderBy(a => a.EME_ExamOrder).ToArray();

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exm_Multiple_Exam_Cumulative_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 100000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                        if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getcumulativereportdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exm_Multiple_Exam_Cumulative_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                        if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getsubjectslist = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // BGHS/BBHS Multiple Exam Progress Report 
        public JSHSExamReportsDTO getmultiple_exam_progress_report(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                List<long> examid = new List<long>();

                foreach (var c in data.examlist)
                {
                    examid.Add(c.EME_Id);
                }

                var examidnew = "0";

                for (int k = 0; k < data.examlist.Length; k++)
                {
                    examidnew = examidnew + ',' + data.examlist[k].EME_Id.ToString();
                }

                var get_latest_examid = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && examid.Contains(a.EME_Id)).OrderByDescending(a => a.EME_ExamOrder).Take(1).ToList();

                data.getexamwisetotaldetails = _context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && examid.Contains(a.EME_Id)).Distinct().ToArray();

                data.getexamsubjectwisemarksdetails = _context.ExmStudentMarksProcessSubjectwiseDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && examid.Contains(a.EME_Id)).Distinct().ToArray();

                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                var get_latest_examidtemp = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && examid.Contains(a.EME_Id)).OrderBy(a => a.EME_ExamOrder).ToList();
                long fromEme_id = 0; long ToEme_id = 0;

                if (get_latest_examidtemp != null && get_latest_examidtemp.Count > 0)
                {
                    for (int i = 0; i < get_latest_examidtemp.Count; i++)
                    {
                        if (i == 0)
                        {
                            fromEme_id = get_latest_examidtemp[i].EME_Id;
                        }
                        if (i == get_latest_examidtemp.Count - 1)
                        {
                            ToEme_id = get_latest_examidtemp[i].EME_Id;
                        }
                    }
                }

                // This Condition Will Execute only for app.MultipleExamProgressCardReport /app.MultipleExamProgressCardReport1
                if (data.flagtype != "BBHS")
                {
                    data.getexamdetails = (from a in _context.Exm_Category_ClassDMO
                                           from b in _context.Exm_Master_CategoryDMO
                                           from c in _context.Exm_Yearly_CategoryDMO
                                           from d in _context.Exm_Yearly_Category_ExamsDMO
                                           from e in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                           from f in _context.exammasterDMO
                                           from g in _context.IVRM_School_Master_SubjectsDMO
                                           where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id && d.EYCE_Id == e.EYCE_Id && d.EME_Id == f.EME_Id
                                           && e.ISMS_Id == g.ISMS_Id && a.ECAC_ActiveFlag == true && b.EMCA_ActiveFlag == true && c.EYC_ActiveFlg == true
                                           && d.EYCE_ActiveFlg == true && e.EYCES_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                           && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id
                                           && examid.Contains(d.EME_Id) && e.EYCES_AplResultFlg == true)
                                           select new JSHSExamReportsDTO
                                           {
                                               EME_Id = d.EME_Id,
                                               EME_ExamName = f.EME_ExamName,
                                               EME_ExamOrder = f.EME_ExamOrder,
                                               EYCES_MaxMarks = e.EYCES_MaxMarks
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                    // STUDENT WISE SUBJECT DETAILS //
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_Student_SubjectWise_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 2 });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwisesubjectlist = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    // EXAM MARKS DETAILS //
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_Student_SubjectWise_Marks_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentmarksdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_Student_SubjectWise_Marks_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 2 });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentmarksindidetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }

                // This Condition Will Execute only for app.BBHSMultipleExamReport
                if (data.flagtype == "BBHS")
                {
                    data.getexamdetails = (from a in _context.Exm_Category_ClassDMO
                                           from b in _context.Exm_Master_CategoryDMO
                                           from c in _context.Exm_Yearly_CategoryDMO
                                           from d in _context.Exm_Yearly_Category_ExamsDMO
                                           from e in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                           from f in _context.exammasterDMO
                                           from g in _context.IVRM_School_Master_SubjectsDMO
                                           where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id && d.EYCE_Id == e.EYCE_Id && d.EME_Id == f.EME_Id
                                           && e.ISMS_Id == g.ISMS_Id && a.ECAC_ActiveFlag == true && b.EMCA_ActiveFlag == true && c.EYC_ActiveFlg == true
                                           && d.EYCE_ActiveFlg == true && e.EYCES_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                           && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id
                                           && examid.Contains(d.EME_Id) && e.EYCES_AplResultFlg == true)
                                           select new JSHSExamReportsDTO
                                           {
                                               EME_Id = d.EME_Id,
                                               EME_ExamName = f.EME_ExamName,
                                               EME_ExamOrder = f.EME_ExamOrder,
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                    // STUDENT WISE SUBJECT DETAILS //
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_Student_SubjectWise_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 3 });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwisesubjectlist = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    //Student Wise Grand Total
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_Student_SubjectWise_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 4 });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = examidnew });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? null : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentmarksindidetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                    var from_date = (from a in _context.Exm_Category_ClassDMO
                                     from b in _context.Exm_Yearly_CategoryDMO
                                     from c in _context.Exm_Yearly_Category_ExamsDMO
                                     where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id
                                     && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == fromEme_id
                                     && b.ASMAY_Id == data.ASMAY_Id)
                                     select c.EYCE_AttendanceFromDate).FirstOrDefault();
                    var to_date = (from a in _context.Exm_Category_ClassDMO
                                   from b in _context.Exm_Yearly_CategoryDMO
                                   from c in _context.Exm_Yearly_Category_ExamsDMO
                                   where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id
                                   && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == ToEme_id
                                   && b.ASMAY_Id == data.ASMAY_Id)
                                   select c.EYCE_AttendanceToDate).Max();
                    //var from_date = (from a in _context.Exm_Category_ClassDMO
                    //                 from b in _context.Exm_Yearly_CategoryDMO
                    //                 from c in _context.Exm_Yearly_Category_ExamsDMO
                    //                 where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id
                    //                 && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == get_latest_examid.FirstOrDefault().EME_Id
                    //                 && b.ASMAY_Id == data.ASMAY_Id)
                    //                 select c.EYCE_AttendanceFromDate).FirstOrDefault();

                    //var to_date = (from a in _context.Exm_Category_ClassDMO
                    //               from b in _context.Exm_Yearly_CategoryDMO
                    //               from c in _context.Exm_Yearly_Category_ExamsDMO
                    //               where (a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id
                    //               && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == get_latest_examid.FirstOrDefault().EME_Id
                    //               && b.ASMAY_Id == data.ASMAY_Id)
                    //               select c.EYCE_AttendanceToDate).Max();

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "StudentAttendance_W";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                        cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }

                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.Work_attendence = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "StudentAttendance_P";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                        cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject1 = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject1.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.Present_attendence = retObject1.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Stjames Exam Progress Reports
        public JSHSExamReportsDTO stjamesexamreport(JSHSExamReportsDTO data)
        {
            try
            {
                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var eycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                && a.EYC_ActiveFlg == true).ToList();

                var examidnew = "";
                if (data.reporttype == "indi")
                {
                    for (int k = 0; k < data.examlist.Length; k++)
                    {
                        if (k == 0)
                        {
                            examidnew = data.examlist[k].EME_Id.ToString();
                        }
                        else
                        {
                            examidnew = examidnew + ',' + data.examlist[k].EME_Id.ToString();
                        }
                    }
                }

                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Stjames_Exam_Individual_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.VarChar)
                    {
                        Value = examidnew
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                if (data.reporttype == "indi")
                {
                    List<long> examid = new List<long>();
                    foreach (var c in data.examlist)
                    {
                        examid.Add(c.EME_Id);
                    }

                    data.getexamdetails = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true
                    && examid.Contains(a.EME_Id)).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                    data.getexamwisetotaldetails = _context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && examid.Contains(a.EME_Id)).Distinct().ToArray();

                    // STUDENT MARKS DETAILS //
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Stjames_Get_Exam_Subject_SubSubj_Marks_List";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                     SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@EME_Id",
                            SqlDbType.VarChar)
                        {
                            Value = examidnew
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                            SqlDbType.VarChar)
                        {
                            Value = 1
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
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentmarksdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    // STUDENT WISE SUBJECT DETAILS
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Stjames_Exam_Individual_Exam_SubjectList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                     SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@EME_Id",
                            SqlDbType.VarChar)
                        {
                            Value = examidnew
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                            SqlDbType.VarChar)
                        {
                            Value = 2
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
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwisesubjectlistnew = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    // STUDENT ATTENDANCE DETAILS //
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Stjames_Exam_Individual_Term_Report_Details";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                     SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@EME_Id",
                            SqlDbType.VarChar)
                        {
                            Value = examidnew
                        });

                        cmd.Parameters.Add(new SqlParameter("@FLAG",
                        SqlDbType.VarChar)
                        {
                            Value = 3
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
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwiseattendancedetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }

                else if (data.reporttype == "promotion")
                {
                    data.getexamdetails = (from a in _context.Exm_M_PromotionDMO
                                           from b in _context.Exm_M_Promotion_SubjectsDMO
                                           from c in _context.Exm_M_Prom_Subj_GroupDMO
                                           where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == eycid.FirstOrDefault().EYC_Id
                                           && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true)
                                           select new JSHSExamReportsDTO
                                           {
                                               EMPG_GroupName = c.EMPSG_GroupName,
                                               EMPSG_Order = c.EMPSG_Order,
                                               EMPG_DistplayName = c.EMPSG_DisplayName,
                                               
                                           }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                    data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                                from b in _context.Exm_M_Promotion_SubjectsDMO
                                                from c in _context.Exm_M_Prom_Subj_GroupDMO
                                                from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                                from e in _context.exammasterDMO
                                                where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == e.EME_Id
                                                && a.EYC_Id == eycid.FirstOrDefault().EYC_Id && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                                && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                                select new JSHSExamReportsDTO
                                                {
                                                    EMPG_GroupName = c.EMPSG_GroupName,
                                                    EME_Id = d.EME_Id,
                                                    EME_ExamName = e.EME_ExamName,
                                                    EMPG_DistplayName = c.EMPSG_DisplayName,
                                                    
                                                }).Distinct().ToArray();

                    // STUDENT MARKS DETAILS //
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Stjames_Exam_Promotion_Report_New";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentmarksdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    // STUDENT WISE SUBJECT DETAILS
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Stjames_Exam_Promotion_SubjectList";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });

                        if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwisesubjectlistnew = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    // STUDENT WISE TOTAL MARKS
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "stjames_promotion_exam_total_details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });

                        if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getexamwisetotaldetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    // STUDENT WISE ATTENDANCE DETAILS
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "stjames_promotion_exam_total_details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });

                        if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwiseattendancedetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Stjames Exam Consolidated Reports
        public JSHSExamReportsDTO stjamesexamconsolidatedreport(JSHSExamReportsDTO data)
        {
            try
            {
                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var eycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                && a.EYC_ActiveFlg == true).ToList();

                data.getexamdetails = (from a in _context.Exm_M_PromotionDMO
                                       from b in _context.Exm_M_Promotion_SubjectsDMO
                                       from c in _context.Exm_M_Prom_Subj_GroupDMO
                                       where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == eycid.FirstOrDefault().EYC_Id
                                       && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true)
                                       select new JSHSExamReportsDTO
                                       {
                                           EMPG_GroupName = c.EMPSG_GroupName,
                                           EMPSG_Order = c.EMPSG_Order,
                                           EMPG_DistplayName = c.EMPSG_DisplayName
                                       }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();


                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == e.EME_Id
                                            && a.EYC_Id == eycid.FirstOrDefault().EYC_Id && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EMPG_DistplayName = c.EMPSG_DisplayName
                                            }).Distinct().ToArray();

                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Stjames_Exam_Get_Overall_Cumulative_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT MARKS DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Stjames_Exam_Get_Overall_Cumulative_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 2 });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Stjames_Exam_PromotionCumulative_SubjectList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });

                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlistnew = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE TOTAL MARKS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "stjames_promotion_cumulative_exam_total_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });

                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getexamwisetotaldetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "stjames_promotion_cumulative_exam_total_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });

                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Stjames Exam Nursery Reports GetStjamesNurReport
        public JSHSExamReportsDTO GetStjamesNurReport(JSHSExamReportsDTO data)
        {
            try
            {
                //STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Get_Student_Subject_Attendance_Details_ProgressCard_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = "1"
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Get_Student_Subject_Attendance_Details_ProgressCard_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = "2"
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Get_Student_Subject_Attendance_Details_ProgressCard_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = "3"
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // EXAM WISE SUBSUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Get_Student_Subject_Attendance_Details_ProgressCard_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar)
                    {
                        Value = "4"
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getexamwisesubsubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE MARKS DETAILS               
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Stjames_Exam_SubSubject_ProgressCard_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSProgressCardReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true).Distinct().ToArray();

                data.studentwisemarks = _context.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // BGHS PROMOTION REPORT NURSERY AND JUNIOR I FORMAT 
        public JSHSExamReportsDTO promotionreportnurjr(JSHSExamReportsDTO data)
        {
            try
            {
                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = ""
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details_Promotion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                       SqlDbType.VarChar)
                    {
                        Value = "1"
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Studentwise_Marks_Details_Promotion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                data.getexamdetails = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                       from b in _context.masterexam
                                       where (a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && a.EYC_Id == geteycid)
                                       select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getclassteacher = (from a in _context.ClassTeacherMappingDMO
                                        from b in _context.HR_Master_Employee_DMO
                                        where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            classteachername = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                            (b.HRME_EmployeeMiddleName == null ? "" : " " + b.HRME_EmployeeMiddleName) +
                                            (b.HRME_EmployeeLastName == null ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // BGHS PROMOTION REPORT CLASS I TO X (EXCEPT IX) FORMAT
        public JSHSExamReportsDTO promotionreportstdiiv(JSHSExamReportsDTO data)
        {
            try
            {
                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = "" });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details_Promotion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                        SqlDbType.VarChar)
                    {
                        Value = "3"
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details_Promotion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                        SqlDbType.VarChar)
                    {
                        Value = "2"
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Studentwise_Marks_Details_Promotion1";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                data.getexamdetails = (from a in _context.Exm_M_PromotionDMO
                                       from b in _context.Exm_M_Promotion_SubjectsDMO
                                       from c in _context.Exm_M_Prom_Subj_GroupDMO
                                       where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                       && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true)
                                       select new JSHSExamReportsDTO
                                       {
                                           EMPG_GroupName = c.EMPSG_GroupName,
                                           EMPSG_Order = c.EMPSG_Order,
                                           EMPG_DistplayName = c.EMPSG_DisplayName,
                                           EMPSG_MarksValue = c.EMPSG_MarksValue

                                       }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == e.EME_Id
                                            && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                List<long> emeid = new List<long>();

                var getemeids = (from a in _context.Exm_M_PromotionDMO
                                 from b in _context.Exm_M_Promotion_SubjectsDMO
                                 from c in _context.Exm_M_Prom_Subj_GroupDMO
                                 from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                 from e in _context.exammasterDMO
                                 where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == e.EME_Id
                                 && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                 && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                 select new JSHSExamReportsDTO
                                 {
                                     EME_Id = d.EME_Id,
                                 }).Distinct().ToList();

                foreach (var c in getemeids)
                {
                    emeid.Add(c.EME_Id);
                }

                data.getclassteacher = (from a in _context.ClassTeacherMappingDMO
                                        from b in _context.HR_Master_Employee_DMO
                                        where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            classteachername = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                            (b.HRME_EmployeeMiddleName == null ? "" : " " + b.HRME_EmployeeMiddleName) +
                                            (b.HRME_EmployeeLastName == null ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                        }).Distinct().ToArray();


                data.getexammaxmarks = (from a in _context.Exm_Category_ClassDMO
                                        from b in _context.Exm_Master_CategoryDMO
                                        from c in _context.Exm_Yearly_CategoryDMO
                                        from d in _context.Exm_Yearly_Category_ExamsDMO
                                        from e in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                        from f in _context.exammasterDMO
                                        from g in _context.IVRM_School_Master_SubjectsDMO
                                        where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id && d.EYCE_Id == e.EYCE_Id
                                        && d.EME_Id == f.EME_Id && e.ISMS_Id == g.ISMS_Id && a.ECAC_ActiveFlag == true && b.EMCA_ActiveFlag == true
                                        && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true && e.EYCES_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id
                                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id
                                        && c.MI_Id == data.MI_Id && emeid.Contains(d.EME_Id) && e.EYCES_AplResultFlg == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EME_Id = d.EME_Id,
                                            EME_ExamName = f.EME_ExamName,
                                            EME_ExamOrder = f.EME_ExamOrder,
                                            EYCES_MaxMarks = e.EYCES_MaxMarks
                                        }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //BGHS PROMOTION REPORT CLASS IX 
        public JSHSExamReportsDTO bghspromotionreportix(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                 && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid
                && a.ECT_ActiveFlag == true).OrderBy(a => a.ECT_TermName).Distinct().ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BGHS_Student_SubjectWise_Marks_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT SUBJECT WISE DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BGHS_Student_SubjectWise_Marks_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 2 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BGHS_Student_SubjectWise_Marks_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 3 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE MARKS DETAILS 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BGHS_Student_SubjectWise_Marks_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 4 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //BCEHS PROMOTION REPORT CLASS IX 
        public JSHSExamReportsDTO bcehspromotionreportix(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                 && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid
                && a.ECT_ActiveFlag == true).OrderBy(a => a.ECT_TermName).Distinct().ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BCEHS_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT SUBJECT WISE DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BCEHS_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 2 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BCEHS_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 3 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE MARKS DETAILS 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BCEHS_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 4 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        //TBSReportSchool
        //PromotionReportTBSchool
        public JSHSExamReportsDTO PromotionReportTBSchool(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "TBS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = 0 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_Exam_6_8_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EMPG_GroupName", SqlDbType.VarChar) { Value = data.EMPG_GroupName });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = 0 });
                    //EMPG_GroupName
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_EXAM_GET_6_8_STUDENT_GrandTotal";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = "0" });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }

                        data.getstudentwisetermwisedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                           && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && ids.Contains(a.AMST_Id)).ToArray();

                var from_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_From_Date).FirstOrDefault();
                var to_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_To_Date).FirstOrDefault();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "stjames_promotion_exam_total_details";
                    cmd.CommandText = "TBSchool_promotion_exam_total_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });

                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getexamwisetotaldetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Promotion_Exam_Attendnce";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EYC_Id", SqlDbType.VarChar) { Value = geteycid });
                    //@EYC_Id
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                               );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_Student_Exam_Remarks";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.examwiseremarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //added

                //added
                //    //added
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_Student_SubjectWise_Details_Ind";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    // cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudent_examwisemarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //added  

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;


        }

        //TBSReportSchool
        public JSHSExamReportsDTO TBSReportSchool(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }
                string emeids = "";
                List<long> EME_Ids = new List<long>();
                if (data.examlist != null && data.examlist.Length > 0)
                {
                    foreach (var c in data.examlist)
                    {
                        if (emeids == "")
                        {
                            emeids = c.EME_Id.ToString();
                        }
                        else
                        {
                            emeids = emeids + "," + c.EME_Id.ToString();
                        }
                        EME_Ids.Add(c.EME_Id);
                    }

                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "TBS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });
                    cmd.Parameters.Add(new SqlParameter("@EMGD_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_Student_Exam_Remarks";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.examwiseremarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && ids.Contains(a.AMST_Id)).Distinct().ToArray();


                //examlist
                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true && EME_Ids.Contains(e.EME_Id))
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).Distinct().ToArray();
                //added
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_Exam_6_8_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EMPG_GroupName", SqlDbType.VarChar) { Value = data.EMPG_GroupName });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "6" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });
                    cmd.Parameters.Add(new SqlParameter("@EMGD_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                var from_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_From_Date).FirstOrDefault();
                var to_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_To_Date).FirstOrDefault();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "stjames_promotion_exam_total_details";
                    cmd.CommandText = "TBSchool_promotion_exam_total_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });

                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getexamwisetotaldetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_Student_SubjectWise_Details_Ind";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    // cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudent_examwisemarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Promotion_Exam_Attendnce";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EYC_Id", SqlDbType.VarChar) { Value = geteycid });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    //@EYC_Id
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                               );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_EXAM_GET_6_8_STUDENT_GrandTotal";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "4" });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }

                        data.getstudentwisetermwisedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //getsubjects
                data.getsubjects = _context.ExamsubjectGroupMappingDMO.Where(R => R.MI_Id == data.MI_Id && R.ESG_ActiveFlag == true && R.ESG_ExamPromotionFlag == "PE" && R.EMCA_Id == getemcaid
                && R.ASMAY_Id == data.ASMAY_Id).Distinct().ToArray();
                //getgrade
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TBS_EXAM_GET_6_8_STUDENT_GrandTotal";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "3" });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }

                        data.YearlySkillAreaStudentWise = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;


        }

        public JSHSExamReportsDTO Sarvodaya_Report(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }
                string emeids = "";
                List<long> EME_Ids = new List<long>();
                if (data.examlist != null && data.examlist.Length > 0)
                {
                    foreach (var c in data.examlist)
                    {
                        if (emeids == "")
                        {
                            emeids = c.EME_Id.ToString();
                        }
                        else
                        {
                            emeids = emeids + "," + c.EME_Id.ToString();
                        }
                        EME_Ids.Add(c.EME_Id);
                    }

                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "SARVODAYA_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                //examlist
                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO                                            from b in _context.Exm_M_Promotion_SubjectsDMO                                            from c in _context.Exm_M_Prom_Subj_GroupDMO                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO                                            from e in _context.exammasterDMO                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)                                            select new JSHSExamReportsDTO                                            {                                                EMPG_GroupName = c.EMPSG_GroupName,                                                EME_Id = d.EME_Id,                                                EME_ExamName = e.EME_ExamName,                                                EME_ExamCode = e.EME_ExamCode,                                                EME_ExamOrder = e.EME_ExamOrder,                                                EMPG_DistplayName = c.EMPSG_DisplayName,                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).Distinct().ToArray();
                //added
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SARVODAYA_Exam_6_8_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SARVODAYA_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SARVODAYA_Examwise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudent_examwisemarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                var from_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_From_Date).FirstOrDefault();
                var to_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_To_Date).FirstOrDefault();
                //getsubjects
                //constant
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SARVODAYA_CCE_Activities_Transaction";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });//@Flag
                    cmd.Parameters.Add(new SqlParameter("@Flag", SqlDbType.VarChar) { Value = "1" });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getallgrade = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Promotion_Exam_Attendnce";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EYC_Id", SqlDbType.VarChar) { Value = geteycid });
                    //@EYC_Id
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                               );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;


        }
        //Sarvodaya_ReportSenior
        public JSHSExamReportsDTO Sarvodaya_ReportSenior(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }
                string emeids = "";
                List<long> EME_Ids = new List<long>();
                if (data.examlist != null && data.examlist.Length > 0)
                {
                    foreach (var c in data.examlist)
                    {
                        if (emeids == "")
                        {
                            emeids = c.EME_Id.ToString();
                        }
                        else
                        {
                            emeids = emeids + "," + c.EME_Id.ToString();
                        }
                        EME_Ids.Add(c.EME_Id);
                    }

                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "SARVODAYA_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                //examlist
                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO                                            from b in _context.Exm_M_Promotion_SubjectsDMO                                            from c in _context.Exm_M_Prom_Subj_GroupDMO                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO                                            from e in _context.exammasterDMO                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)                                            select new JSHSExamReportsDTO                                            {                                                EMPG_GroupName = c.EMPSG_GroupName,                                                EME_Id = d.EME_Id,                                                EME_ExamName = e.EME_ExamName,                                                EME_ExamCode = e.EME_ExamCode,                                                EME_ExamOrder = e.EME_ExamOrder,                                                EMPG_DistplayName = c.EMPSG_DisplayName,                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).Distinct().ToArray();
                //added
                string EME_Id = "0";
                var getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                           from b in _context.Exm_M_Promotion_SubjectsDMO
                                           from c in _context.Exm_M_Prom_Subj_GroupDMO
                                           from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                           from e in _context.exammasterDMO
                                           where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                           && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                           && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                           select new JSHSExamReportsDTO
                                           {
                                               EMPG_GroupName = c.EMPSG_GroupName,
                                               EME_Id = d.EME_Id,
                                               EME_ExamName = e.EME_ExamName,
                                               EME_ExamCode = e.EME_ExamCode,
                                               EME_ExamOrder = e.EME_ExamOrder,
                                               EMPG_DistplayName = c.EMPSG_DisplayName,
                                               EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).Distinct().ToList();
                if (getgroupexamdetails != null && getgroupexamdetails.Count > 0)
                {
                    foreach (var d in getgroupexamdetails)
                    {
                        EME_Id = EME_Id + ',' + d.EME_Id;
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Mark_cumalative_Sarvodaya";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    cmd.Parameters.Add(new SqlParameter("@Percentage", SqlDbType.VarChar) { Value = data.ECTEX_MarksPercentValue });
                    cmd.Parameters.Add(new SqlParameter("@Percentage2", SqlDbType.VarChar) { Value = data.EYCES_MaxMarks });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //Exam_Mark_cumalative_Sarvodaya_List
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Mark_cumalative_Sarvodaya_List";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = EME_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getexam = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //Exam List 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SARVODAYA_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SARVODAYA_Examwise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudent_examwisemarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                var from_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_From_Date).FirstOrDefault();
                var to_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_To_Date).FirstOrDefault();
                //getsubjects
                //constant

                //getstudentwiseattendancedetails
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_W";
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

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@from",
                        SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to",
                        SqlDbType.Date)
                    {
                        Value = to_date
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Work_attendence = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_P";
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

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@from",
                        SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to",
                        SqlDbType.Date)
                    {
                        Value = to_date
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //co-colsticks
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SARVODAYA_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 90000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    //cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '5' });
                    //5'
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //termDetails
                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                       && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                           && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                //overalltotal
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "SARVODAYA_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "6" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getsubjectwisetotaldetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;


        }
        //Sarvodaya_ReportSenior
        public JSHSExamReportsDTO PromotionReportI_IV(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                      
                    }
                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CBS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                //STUDENT WISE MARKS DETAILS
                data.examwiseremarks = _context.ExamTermWiseRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
              && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECTERE_Indi_OverAllFlag == "IE" && ids.Contains(a.AMST_Id)).ToArray();
                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CBS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "CBS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 0;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                //    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();

                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(
                //                        dataReader.GetName(iFiled1),
                //                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.getstudentwiseactiviteslist = retObject.ToArray();
                //    }
                //    catch (Exception ee)
                //    {
                //        Console.WriteLine(ee.Message);
                //    }
                //}

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_6_8_STUDENT_GrandTotal";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }

                        data.getstudentwisetermwisedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CBS_Exam_6_8_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EMPG_GroupName", SqlDbType.VarChar) { Value = data.EMPG_GroupName });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //
                if (data.flag == "cumulative")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CBS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "7" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwisesubjectlist = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CBS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwisesubjectlist = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }

                //Grandtatoat


                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                           && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
                if (data.EMPG_GroupName != "")
                {
                    data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                            && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                            && c.EMPSG_ActiveFlag == true && c.EMPSG_GroupName == data.EMPG_GroupName)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EMPSG_Order = c.EMPSG_Order,
                                                EMPG_DistplayName = c.EMPSG_DisplayName
                                            }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                    data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                                from b in _context.Exm_M_Promotion_SubjectsDMO
                                                from c in _context.Exm_M_Prom_Subj_GroupDMO
                                                from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                                from e in _context.exammasterDMO
                                                where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                                && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                                && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true && c.EMPSG_GroupName == data.EMPG_GroupName)
                                                select new JSHSExamReportsDTO
                                                {
                                                    EMPG_GroupName = c.EMPSG_GroupName,
                                                    EME_Id = d.EME_Id,
                                                    EME_ExamName = e.EME_ExamName,
                                                    EME_ExamCode = e.EME_ExamCode,
                                                    EME_ExamOrder = e.EME_ExamOrder,
                                                    EMPG_DistplayName = c.EMPSG_DisplayName,
                                                    EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                                }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                }
                else
                {
                    data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                            && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                            && c.EMPSG_ActiveFlag == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EMPSG_Order = c.EMPSG_Order,
                                                EMPG_DistplayName = c.EMPSG_DisplayName
                                            }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                    data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                                from b in _context.Exm_M_Promotion_SubjectsDMO
                                                from c in _context.Exm_M_Prom_Subj_GroupDMO
                                                from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                                from e in _context.exammasterDMO
                                                where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                                && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                                && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                                select new JSHSExamReportsDTO
                                                {
                                                    EMPG_GroupName = c.EMPSG_GroupName,
                                                    EME_Id = d.EME_Id,
                                                    EME_ExamName = e.EME_ExamName,
                                                    EME_ExamCode = e.EME_ExamCode,
                                                    EME_ExamOrder = e.EME_ExamOrder,
                                                    EMPG_DistplayName = c.EMPSG_DisplayName,
                                                    EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                                }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                }

                var getgroupExam = (from a in _context.Exm_M_PromotionDMO
                                    from b in _context.Exm_M_Promotion_SubjectsDMO
                                    from c in _context.Exm_M_Prom_Subj_GroupDMO
                                    from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                    from e in _context.exammasterDMO
                                    where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                    && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                    && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                    select new JSHSExamReportsDTO
                                    {
                                        EMPG_GroupName = c.EMPSG_GroupName,
                                        EME_Id = d.EME_Id,
                                        EME_ExamName = e.EME_ExamName,
                                        EME_ExamCode = e.EME_ExamCode,
                                        EME_ExamOrder = e.EME_ExamOrder,
                                        EMPG_DistplayName = c.EMPSG_DisplayName,
                                        EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                    }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && ids.Contains(a.AMST_Id)).ToArray();

                var from_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_From_Date).FirstOrDefault();
                var to_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_To_Date).FirstOrDefault();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_CCE_Activities_Transaction";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getallgrade = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

             
                //added By sanjeev              
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Calkatta_exam_Garph_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EMPG_GroupName", SqlDbType.VarChar) { Value = data.EMPG_GroupName });

                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getexamwisetotaldetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //Calkatta_exam_Garph_details
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Calkatta_exam_Garph_details";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 0;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                //    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                //    cmd.Parameters.Add(new SqlParameter("@EMPG_GroupName", SqlDbType.VarChar) { Value = data.EMPG_GroupName });

                //    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(
                //                        dataReader.GetName(iFiled1),
                //                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.ExamWise_PaperType = retObject.ToArray();
                //    }
                //    catch (Exception ee)
                //    {
                //        Console.WriteLine(ee.Message);
                //    }
                //}
                ////
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Calkatta_exam_Garph_details";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 0;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                //    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "3" });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                //    cmd.Parameters.Add(new SqlParameter("@EMPG_GroupName", SqlDbType.VarChar) { Value = data.EMPG_GroupName });

                //    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                //    var retObject = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(
                //                        dataReader.GetName(iFiled1),
                //                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.getparticipatedetails = retObject.ToArray();
                //    }
                //    catch (Exception ee)
                //    {
                //        Console.WriteLine(ee.Message);
                //    }
                //}


                //added
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BBHS_Student_SubjectWise_Details_Ind";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    // cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudent_examwisemarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //attendence
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "CBS_GET_ATTENENDE_STUDENT_DETAILS";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 0;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                //    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = data.flag });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                //    cmd.Parameters.Add(new SqlParameter("@FROM_DATE", SqlDbType.Date)
                //    {
                //        Value = from_date
                //    });
                //    cmd.Parameters.Add(new SqlParameter("@TO_DATE",
                //        SqlDbType.Date)
                //    {
                //        Value = to_date
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();

                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                //                }
                //                retObject.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.getstudentwiseattendancedetails = retObject.ToArray();
                //    }
                //    catch (Exception ee)
                //    {
                //        Console.WriteLine(ee.Message);
                //    }
                //}

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Promotion_Exam_Attendnce";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EYC_Id", SqlDbType.VarChar) { Value = geteycid });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    //@EYC_Id
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                               );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_promotion_groupwise_remarks";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                               );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.exam_promotion_groupwise_remarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CBS_EXAM_Yearly_Total_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getgradelist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Stthomos_III_V_Report
        //NOTREDAME
        public JSHSExamReportsDTO nds_6_8_report(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_Exam_6_8_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE TOTAL MARKS GROUPWISE AND EXAMWISE DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BBHS_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "4" });
                    // cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getsubjectwisetotaldetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_6_8_STUDENT_GrandTotal";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }

                        data.getstudentwisetermwisedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                           && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && ids.Contains(a.AMST_Id)).ToArray();

                var from_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_From_Date).FirstOrDefault();
                var to_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_To_Date).FirstOrDefault();
                //Exm_ProgressCard_RemarksDMO
                //NDS_Exam_6_8_ProgressCard_Total
                //getallgrade
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_CCE_Activities_Transaction";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getallgrade = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //added By sanjeev              
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "stjames_promotion_exam_total_details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });

                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getexamwisetotaldetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
               && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMER_ActiveFlag == true
               && ids.Contains(a.AMST_Id)).Distinct().ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EXAM_GET_ATTENENDE_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "4" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@FROM_DATE",
                      SqlDbType.Date)
                    {
                        Value = from_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@TO_DATE",
                        SqlDbType.Date)
                    {
                        Value = to_date
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Promotion_Exam_Attendnce";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@EYC_Id", SqlDbType.VarChar) { Value = geteycid });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    //@EYC_Id
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                                               );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Present_attendence = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //added
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BBHS_Student_SubjectWise_Details_Ind";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    // cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudent_examwisemarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Pramotion_report
        public JSHSExamReportsDTO Pramotion_report(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";

                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                if (data.reporttype == "superaverage")
                {
                    //STUDENT WISE SUBJECT DETAILS
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "St_ThomosCumualtive_AverageReport";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentmarksdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "St_ThomosCumualtive_AverageReport";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }

                            data.getstudentwisesubjectlist = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "St_ThomosCumualtive_AverageReport";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "3" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }

                            data.St_ThomosTotal = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                }
                else
                {


                    // STUDENT DETAILS
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    //STUDENT WISE SUBJECT DETAILS
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwisesubjectlist = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    //STUDENT WISE MARKS DETAILS
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "NDS_Exam_6_8_ProgressCard_Report_Details_Modify_PIOT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                        cmd.Parameters.Add(new SqlParameter("@EMPG_GroupName", SqlDbType.VarChar) { Value = data.EMPG_GroupName });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentmarksdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    // STUDENT WISE SKILLS LIST
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwiseskillslist = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    // STUDENT WISE ACTIVITES LIST
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentwiseactiviteslist = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STCSP_EXAM_GET_6_8_STUDENT_GrandTotal";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }

                            data.getstudentwisetermwisedetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                    data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();



                    data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                            && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                            && c.EMPSG_ActiveFlag == true && c.EMPSG_GroupName == data.EMPG_GroupName)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EMPSG_Order = c.EMPSG_Order,
                                                EMPG_DistplayName = c.EMPSG_DisplayName
                                            }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                    var getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                               from b in _context.Exm_M_Promotion_SubjectsDMO
                                               from c in _context.Exm_M_Prom_Subj_GroupDMO
                                               from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                               from e in _context.exammasterDMO
                                               where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                               && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                               && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true && c.EMPSG_GroupName == data.EMPG_GroupName)
                                               select new JSHSExamReportsDTO
                                               {
                                                   EMPG_GroupName = c.EMPSG_GroupName,
                                                   EME_Id = d.EME_Id,
                                                   EME_ExamName = e.EME_ExamName,
                                                   EME_ExamCode = e.EME_ExamCode,
                                                   EME_ExamOrder = e.EME_ExamOrder,
                                                   EMPG_DistplayName = c.EMPSG_DisplayName,
                                                   EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                               }).Distinct().OrderBy(a => a.EME_ExamOrder).ToList();
                    data.getgroupexamdetails = getgroupexamdetails.Distinct().ToArray();
                    string EME_Id = "0";
                    List<long> EME_Ids = new List<long>();
                    if (getgroupexamdetails.Count > 0)
                    {
                        foreach (var d in getgroupexamdetails)
                        {
                            EME_Ids.Add(d.EME_Id);
                            EME_Id = EME_Id + ',' + d.EME_Id;
                        }

                    }

                    data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                               from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                               from c in _context.masterexam
                                               where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                               && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                               && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid && EME_Ids.Contains(c.EME_Id) && EME_Ids.Contains(b.EME_Id))
                                               select new JSHSExamReportsDTO
                                               {
                                                   ECT_Id = b.ECT_Id,
                                                   EME_Id = b.EME_Id,
                                                   EME_ExamName = c.EME_ExamName,
                                                   EME_ExamOrder = c.EME_ExamOrder,
                                                   ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                               }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exm_CCE_Activities_Transaction_Tabular";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@EMPG_GroupName", SqlDbType.VarChar) { Value = data.EMPG_GroupName });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(
                                            dataReader.GetName(iFiled1),
                                            dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getallgrade = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }


                    //added
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_BBHS_Student_SubjectWise__StTabular";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                        cmd.Parameters.Add(new SqlParameter("@EMPG_GroupName", SqlDbType.VarChar) { Value = data.EMPG_GroupName });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudent_examwisemarks = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                    //added
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STCS_EXAM_GET_9_STUDENT_DETAILS_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "6" });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.savelisttot = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO nds_9_report(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_9_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_9_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_Exam_9_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_9_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_9_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                           && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName,
                                            EMPSG_MarksValue = c.EMPSG_MarksValue
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && ids.Contains(a.AMST_Id)).ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //nds_9_Newreport
        public JSHSExamReportsDTO nds_9_Newreport(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //Exam Group Deatils
                var getgroupdetailsE = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToList();
                var bestoffemeid1 = 0; var bestoffemeid2 = 0;
                var list = _context.masterexam.Where(R => R.EME_ExamName == "PERIODIC TEST 1" && R.MI_Id == data.MI_Id).ToList();
                var listtwo = _context.masterexam.Where(R => R.EME_ExamName == "PERIODIC TEST 2" && R.MI_Id == data.MI_Id).ToList();
                if (list.Count > 0 && listtwo.Count > 0)
                {
                    bestoffemeid1 = list.FirstOrDefault().EME_Id;
                    bestoffemeid2 = listtwo.FirstOrDefault().EME_Id;
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "NDS_Exam_6_8_ProgressCard_Report_Details_Modify_bkp";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                        cmd.Parameters.Add(new SqlParameter("@bestoffemeid1", SqlDbType.VarChar) { Value = bestoffemeid1 });
                        cmd.Parameters.Add(new SqlParameter("@bestoffemeid2", SqlDbType.VarChar) { Value = bestoffemeid2 });
                        //, 
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentmarksdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "NDS_Exam_6_8_ProgressCard_Report_Details_Modify";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.getstudentmarksdetails = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                           && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().ToArray();
                //NDS_Exam_6_8_getgroupexamdetails
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "NDS_Exam_6_8_getgroupexamdetails";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 0;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@EYC_Id", SqlDbType.VarChar) { Value = geteycid });
                //    cmd.Parameters.Add(new SqlParameter("@EMPSG_ID", SqlDbType.VarChar) { Value = 1890 });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();
                //    var retObject = new List<dynamic>();

                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(
                //                        dataReader.GetName(iFiled1),
                //                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.getgroupexamdetails = retObject.ToArray();
                //    }
                //    catch (Exception ee)
                //    {
                //        Console.WriteLine(ee.Message);
                //    }
                //}

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && ids.Contains(a.AMST_Id)).ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO nds_1_5_report(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_1_5_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_1_5_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_Exam_1_5_ProgressCard_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_1_5_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_1_5_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_1_5_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "6" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SPORTS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_1_5_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "7" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesportsdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.getsubjectwisetotaldetails = _context.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id).ToArray();

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                           && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && a.AMST_Id == data.AMST_Id).ToArray();

                data.getparticipatedetails = _context.Exm_Student_TermAchievementsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id).ToArray();

                data.examwiseremarks = _context.ExamTermWiseRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ECTERE_Indi_OverAllFlag == "IE" && a.AMST_Id == data.AMST_Id).ToArray();

                data.getsudentwisehousedetails = (from a in _context.SportStudentHouseDivisionDMO
                                                  from b in _context.SportMasterHouseDMO
                                                  where (a.SPCCMH_Id == b.SPCCMH_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                                  && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id
                                                  && a.SPCCMH_ActiveFlag == true)
                                                  select b).Distinct().ToArray();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO nds_JrSrKG_report(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                string emeids = "";
                List<long> EME_Ids = new List<long>();
                foreach (var c in data.examlist)
                {
                    if (emeids == "")
                    {
                        emeids = c.EME_Id.ToString();
                    }
                    else
                    {
                        emeids = emeids + "," + c.EME_Id.ToString();
                    }
                    EME_Ids.Add(c.EME_Id);
                }


                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_Jr_Sr_KG_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_Jr_Sr_KG_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT SUBSUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_Jr_Sr_KG_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "3" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectsubsubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE ATTENDANCE
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_Jr_Sr_KG_STUDENT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "4" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "NDS_EXAM_GET_Jr_Sr_KG_ProgressCard_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && a.AMST_Id == data.AMST_Id).ToArray();

                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                  && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMER_ActiveFlag == true && EME_Ids.Contains(a.EME_ID) && a.AMST_Id == data.AMST_Id
                  ).Distinct().ToArray();
                data.getexamwisetotaldetails = _context.ExmStudentMarksProcessDMO.Where(R => R.MI_Id == data.MI_Id && R.ASMCL_Id == data.ASMCL_Id && R.ASMS_Id == data.ASMS_Id && R.AMST_Id == data.AMST_Id && R.ASMAY_Id == data.ASMAY_Id && EME_Ids.Contains(R.EME_Id)).Distinct().ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //BGHS 2020-2021 promotion Progress Card Report I TO X (EXCEPT IX)
        public JSHSExamReportsDTO BGHS_IIV_20202021(JSHSExamReportsDTO data)
        {
            try
            {
                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = "" });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details_Promotion";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                        SqlDbType.VarChar)
                    {
                        Value = "3"
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ATTENDANCE DETAILS
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Exam_Student_SubjectWise_Details_Promotion";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 0;

                //    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //      SqlDbType.VarChar)
                //    {
                //        Value = data.MI_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //                 SqlDbType.VarChar)
                //    {
                //        Value = data.ASMAY_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                //        SqlDbType.VarChar)
                //    {
                //        Value = data.ASMCL_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                //        SqlDbType.VarChar)
                //    {
                //        Value = data.ASMS_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@FLAG",
                //        SqlDbType.VarChar)
                //    {
                //        Value = "2"
                //    });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();

                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(
                //                        dataReader.GetName(iFiled1),
                //                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                //                    );
                //                }
                //                retObject.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.getstudentwiseattendancedetails = retObject.ToArray();
                //    }
                //    catch (Exception ee)
                //    {
                //        Console.WriteLine(ee.Message);
                //    }
                //}

                // STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Studentwise_Marks_Details_Promotion_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                data.getexamdetails = (from a in _context.Exm_M_PromotionDMO
                                       from b in _context.Exm_M_Promotion_SubjectsDMO
                                       from c in _context.Exm_M_Prom_Subj_GroupDMO
                                       where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                       && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true)
                                       select new JSHSExamReportsDTO
                                       {
                                           EMPG_GroupName = c.EMPSG_GroupName,
                                           EMPSG_Order = c.EMPSG_Order,
                                           EMPG_DistplayName = c.EMPSG_DisplayName,
                                           EMPSG_MarksValue = c.EMPSG_MarksValue

                                       }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == e.EME_Id
                                            && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                List<long> emeid = new List<long>();

                var getemeids = (from a in _context.Exm_M_PromotionDMO
                                 from b in _context.Exm_M_Promotion_SubjectsDMO
                                 from c in _context.Exm_M_Prom_Subj_GroupDMO
                                 from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                 from e in _context.exammasterDMO
                                 where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == e.EME_Id
                                 && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                 && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                 select new JSHSExamReportsDTO
                                 {
                                     EME_Id = d.EME_Id,
                                 }).Distinct().ToList();

                foreach (var c in getemeids)
                {
                    emeid.Add(c.EME_Id);
                }

                data.getclassteacher = (from a in _context.ClassTeacherMappingDMO
                                        from b in _context.HR_Master_Employee_DMO
                                        where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            classteachername = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                            (b.HRME_EmployeeMiddleName == null ? "" : " " + b.HRME_EmployeeMiddleName) +
                                            (b.HRME_EmployeeLastName == null ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                        }).Distinct().ToArray();


                data.getexammaxmarks = (from a in _context.Exm_Category_ClassDMO
                                        from b in _context.Exm_Master_CategoryDMO
                                        from c in _context.Exm_Yearly_CategoryDMO
                                        from d in _context.Exm_Yearly_Category_ExamsDMO
                                        from e in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                        from f in _context.exammasterDMO
                                        from g in _context.IVRM_School_Master_SubjectsDMO
                                        where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id && d.EYCE_Id == e.EYCE_Id
                                        && d.EME_Id == f.EME_Id && e.ISMS_Id == g.ISMS_Id && a.ECAC_ActiveFlag == true && b.EMCA_ActiveFlag == true
                                        && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true && e.EYCES_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id
                                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id
                                        && c.MI_Id == data.MI_Id && emeid.Contains(d.EME_Id) && e.EYCES_AplResultFlg == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EME_Id = d.EME_Id,
                                            EME_ExamName = f.EME_ExamName,
                                            EME_ExamOrder = f.EME_ExamOrder,
                                            EYCES_MaxMarks = e.EYCES_MaxMarks
                                        }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // BGHS 2020-2021 PROMOTION REPORT NURSERY AND JUNIOR I FORMAT
        public JSHSExamReportsDTO BGHS_Nurjr_20202021(JSHSExamReportsDTO data)
        {
            try
            {
                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = ""
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details_Promotion";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                       SqlDbType.VarChar)
                    {
                        Value = "1"
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Studentwise_Marks_Details_Promotion_New1";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                data.getexamdetails = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                       from b in _context.masterexam
                                       where (a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && a.EYC_Id == geteycid)
                                       select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getclassteacher = (from a in _context.ClassTeacherMappingDMO
                                        from b in _context.HR_Master_Employee_DMO
                                        where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            classteachername = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                            (b.HRME_EmployeeMiddleName == null ? "" : " " + b.HRME_EmployeeMiddleName) +
                                            (b.HRME_EmployeeLastName == null ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // BGHS 2020-2021 Multiple Exam Progress Report  
        public JSHSExamReportsDTO BGHS_GetMultiple_Exam_Progress_Report(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                List<long> examid = new List<long>();

                foreach (var c in data.examlist)
                {
                    examid.Add(c.EME_Id);
                }

                var examidnew = "";

                for (int k = 0; k < data.examlist.Length; k++)
                {
                    if (k == 0)
                    {
                        examidnew = data.examlist[k].EME_Id.ToString();
                    }
                    else
                    {
                        examidnew = examidnew + ',' + data.examlist[k].EME_Id.ToString();
                    }
                }

                data.getexamdetails = (from a in _context.Exm_Category_ClassDMO
                                       from b in _context.Exm_Master_CategoryDMO
                                       from c in _context.Exm_Yearly_CategoryDMO
                                       from d in _context.Exm_Yearly_Category_ExamsDMO
                                       from e in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                       from f in _context.exammasterDMO
                                       from g in _context.IVRM_School_Master_SubjectsDMO
                                       where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id && d.EYCE_Id == e.EYCE_Id && d.EME_Id == f.EME_Id
                                       && e.ISMS_Id == g.ISMS_Id && a.ECAC_ActiveFlag == true && b.EMCA_ActiveFlag == true && c.EYC_ActiveFlg == true
                                       && d.EYCE_ActiveFlg == true && e.EYCES_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                       && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id
                                       && examid.Contains(d.EME_Id) && e.EYCES_AplResultFlg == true)
                                       select new JSHSExamReportsDTO
                                       {
                                           EME_Id = d.EME_Id,
                                           EME_ExamName = f.EME_ExamName,
                                           EME_ExamOrder = f.EME_ExamOrder,
                                           EYCES_MaxMarks = e.EYCES_MaxMarks
                                       }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 1
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = examidnew
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SUBJECT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 2
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                  SqlDbType.VarChar)
                    {
                        Value = examidnew
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // EXAM MARKS DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Student_SubjectWise_Marks_Details_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = examidnew
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                  SqlDbType.VarChar)
                    {
                        Value = 1
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //added
                var from_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_From_Date).FirstOrDefault();
                var to_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_To_Date).FirstOrDefault();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "StudentAttendance_W";                    cmd.CommandTimeout = 800000;                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);                                }                                retObject.Add((ExpandoObject)dataRow1);                            }                        }                        data.Work_attendence = retObject.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {                    cmd.CommandText = "StudentAttendance_P";
                    cmd.CommandTimeout = 800000;                    cmd.CommandType = CommandType.StoredProcedure;                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject1 = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);                                }                                retObject1.Add((ExpandoObject)dataRow1);                            }                        }                        data.Present_attendence = retObject1.ToArray();                    }                    catch (Exception ex)                    {                        Console.WriteLine(ex.Message);                    }                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //BGHS PROMOTION REPORT CLASS IX  2020-2021
        public JSHSExamReportsDTO BGHS_IX_20202021(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                 && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid
                && a.ECT_ActiveFlag == true).OrderBy(a => a.ECT_TermName).Distinct().ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                // STUDENT DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BGHS_Student_SubjectWise_Marks_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 1 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT SUBJECT WISE DETAILS //
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BGHS_Student_SubjectWise_Marks_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 2 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //// STUDENT WISE ATTENDANCE DETAILS //
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Exam_BGHS_Student_SubjectWise_Marks_Details";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                //    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 3 });

                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();

                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                //                }
                //                retObject.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.getstudentwiseattendancedetails = retObject.ToArray();
                //    }
                //    catch (Exception ee)
                //    {
                //        Console.WriteLine(ee.Message);
                //    }
                //}

                // STUDENT WISE MARKS DETAILS 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BGHS_Student_SubjectWise_Marks_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = 4 });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //BIS Progress Card Report
        public JSHSExamReportsDTO BISProgressCardReport(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_Exam_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                           && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE").ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO BISPromotionCardReport(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_Exam_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName,
                                            EMPSG_PercentValue = a.EMP_MarksPerFlg == "P" ? c.EMPSG_PercentValue : c.EMPSG_MarksValue
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE").ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO BISFianlPromotionCardReport(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                var Emp_id = _context.Exm_M_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.EYC_Id == geteycid && a.EMP_ActiveFlag == true).Select(a => a.EMP_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_Exam_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                var from_date = _context.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true).ASMAY_From_Date;

                var to_date = _context.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true).ASMAY_To_Date;

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_W";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Work_attendence = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_P";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var getpromotionnonapplicablesubjects = _context.Exm_M_Promotion_SubjectsDMO.Where(a => a.EMP_Id == Emp_id && a.EMPS_ActiveFlag == true
                && a.EMPS_AppToResultFlg == false).Select(a => a.ISMS_Id).ToList();

                var nonapplicablesubject_examwisemarks = _context.ExmStudentMarksProcessSubjectwiseDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && getpromotionnonapplicablesubjects.Contains(a.ISMS_Id)).ToArray();

                data.nonapplicablesubject_examwisemarks = nonapplicablesubject_examwisemarks.ToArray();


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName,
                                            EMPSG_PercentValue = a.EMP_MarksPerFlg == "P" ? c.EMPSG_PercentValue : c.EMPSG_MarksValue
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE").ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public JSHSExamReportsDTO GetBISExamWiseProgressCardReport(JSHSExamReportsDTO data)
        {
            try
            {
                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                var geteyceid = _context.Exm_Yearly_Category_ExamsDMO.Where(a => a.EYC_Id == geteycid && a.EME_Id == data.EME_Id
                && a.EYCE_ActiveFlg == true).Select(a => a.EYCE_Id).FirstOrDefault();


                var getgradeid = _context.Exm_Yearly_Category_ExamsDMO.Where(a => a.EYCE_Id == geteyceid && a.EME_Id == data.EME_Id
                && a.EYCE_ActiveFlg == true).Select(a => a.EMGR_Id).FirstOrDefault();

                data.getgradedetails = (from a in _context.Exm_Master_GradeDMO
                                        from b in _context.Exm_Master_Grade_DetailsDMO
                                        where (a.EMGR_Id == b.EMGR_Id && a.EMGR_ActiveFlag == true && b.EMGD_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && b.EMGR_Id == getgradeid)
                                        select b).Distinct().OrderByDescending(a => a.EMGD_From).ThenByDescending(a => a.EMGD_To).ToArray();

                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_Wise_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_Wise_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE ATTENDANCE DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIS_EXAM_GET_STUDENT_Wise_DETAILS_MODIFY";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "3" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true
                && ids.Contains(a.AMST_Id)).Distinct().ToArray();

                data.ExamWise_PaperType = (from a in _context.Exm_Student_Examwise_PTDMO
                                           from b in _context.Exm_Master_PaperTypeDMO
                                           where (a.EMPATY_Id == b.EMPATY_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                           && a.ASMS_Id == data.ASMS_Id && ids.Contains(a.AMST_Id) && a.ESEWPT_ActiveFlg == true && a.EME_Id == data.EME_Id
                                           && a.MI_Id == data.MI_Id)
                                           select new JSHSExamReportsDTO
                                           {
                                               ISMS_Id = a.ISMS_Id,
                                               EME_Id = a.EME_Id,
                                               AMST_Id = a.AMST_Id,
                                               EMPATY_Id = a.EMPATY_Id,
                                               EMPATY_PaperTypeName = b.EMPATY_PaperTypeName,
                                               EMPATY_Color = b.EMPATY_Color == null || b.EMPATY_Color == "" ? "" : b.EMPATY_Color,
                                           }).Distinct().ToArray();

                if (data.flag == "Preparatory")
                {
                    data.YearlySkillArea = (from a in _context.CCE_Master_Life_Skill_Areas_MappingDMO
                                            from b in _context.CCE_Master_Life_Skill_AreasDMO
                                            from c in _context.CCE_Master_Life_SkillsDMO
                                            from d in _context.AcademicYear
                                            where (a.ECSA_Id == b.ECSA_Id && a.ECS_Id == c.ECS_Id && a.ASMAY_Id == d.ASMAY_Id
                                            && a.ASMAY_Id == data.ASMAY_Id && a.ECSAM_ActiveFlag == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                ECS_Id = a.ECS_Id,
                                                ECS_SkillName = c.ECS_SkillName,
                                                ECSA_Id = a.ECSA_Id,
                                                ECSA_SkillArea = b.ECSA_SkillArea,
                                                ECSA_SkillOrder = b.ECSA_SkillOrder
                                            }).Distinct().OrderBy(a => a.ECSA_SkillOrder).ToArray();

                    data.YearlySkillAreaStudentWise = _context.Exm_CCE_SKILLS_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                     && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && ids.Contains(a.AMST_Id)).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //BBHS IXth Class Promotion Progress Card  2020-2021
        public JSHSExamReportsDTO BBHS_IX_20202021(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BBHS_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    //cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BBHS_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    // cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE TOTAL MARKS GROUPWISE AND EXAMWISE DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BBHS_Student_SubjectWise_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "4" });
                    // cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudent_examwisemarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_BBHS_Studentwise_Marks_Details_Promotion_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    //cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                data.getpromotionmarksdetails = _context.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
            && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToArray();

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_Stthomos";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject1 = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }

                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                var from_date = _context.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true).ASMAY_From_Date;
                var to_date = _context.AcademicYear.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true).ASMAY_To_Date;
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_W";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }

                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Work_attendence = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_P";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject1 = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject1.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true && e.EME_ActiveFlag == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE").ToArray();

                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMER_ActiveFlag == true
                    ).Distinct().ToArray();
                if (data.ECTEX_NotApplToTotalFlg == true)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_EXAM_Wise_Remarks";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 900000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        // cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                        //cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                    {
                                        dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.examwiseremarks = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                }



            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //
        public JSHSExamReportsDTO St_Thomos_Ix2023(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StThomos_Exam_Promotion_Report_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });

                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }


                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "7" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //addednewprocedure
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "8" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.savelisttot = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "NDS_Exam_6_8_ProgressCard_Report_Details_Modify";
                    cmd.CommandText = "STThomos_Exam_6_8_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }



                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_BBHS_Student_SubjectWise_Details_Ind";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    // cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.examwiseremarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                           && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && ids.Contains(a.AMST_Id)).ToArray();

                var from_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_From_Date).FirstOrDefault();
                var to_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_To_Date).FirstOrDefault();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "StudentAttendance_W";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }

                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Work_attendence = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                //attendence added by adarsh final flag 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "3" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Present_attendence = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "StudentAttendance_P";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                //    cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.Date) { Value = from_date });
                //    cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.Date) { Value = to_date });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();
                //    var retObject1 = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                //                }
                //                retObject1.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.Present_attendence = retObject1.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
                //NDS_Exam_6_8_ProgressCard_Total
                //getallgrade
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_CCE_Activities_Transaction";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getallgrade = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "10" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getexamwisetotaldetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //graphs
                //formax_marks
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())                {
                    //cmd.CommandText = "NDS_Exam_6_8_ProgressCard_Report_Details_Modify";
                    cmd.CommandText = "STThomos_Max_marks";                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1000000;                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });                    if (cmd.Connection.State != ConnectionState.Open)                        cmd.Connection.Open();                    var retObject = new List<dynamic>();                    try                    {                        using (var dataReader = cmd.ExecuteReader())                        {                            while (dataReader.Read())                            {                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)                                {                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);                                }                                retObject.Add((ExpandoObject)dataRow1);                            }                        }                        data.gettermlist = retObject.ToArray();                    }                    catch (Exception ee)                    {                        Console.WriteLine(ee.Message);                    }                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        //stthomo
        public JSHSExamReportsDTO Stthomos_III_V_Report(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE SUBJECT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "2" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //STUDENT WISE MARKS DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_Exam_6_8_ProgressCard_Report_Details_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentmarksdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE SKILLS LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = '4' });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseskillslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                // STUDENT WISE ACTIVITES LIST
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "5" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseactiviteslist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_GrandTotal";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }

                        data.getstudentwisetermwisedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                data.gettermdetails = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EMCA_Id == getemcaid && a.ECT_ActiveFlag == true).ToArray();

                data.gettermexamdetails = (from a in _context.CCE_Exam_M_TermsDMO
                                           from b in _context.Exm_CCE_TERMS_EXAMSDMO
                                           from c in _context.masterexam
                                           where (a.ECT_Id == b.ECT_Id && b.EME_Id == c.EME_Id && a.MI_Id == data.MI_Id
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ECT_ActiveFlag == true && b.ECTEX_ActiveFlag == true
                                           && c.EME_ActiveFlag == true && a.EMCA_Id == getemcaid)
                                           select new JSHSExamReportsDTO
                                           {
                                               ECT_Id = b.ECT_Id,
                                               EME_Id = b.EME_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EME_ExamOrder = c.EME_ExamOrder,
                                               ECTEX_MarksPercentValue = b.ECTEX_MarksPercentValue
                                           }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getgroupdetails = (from a in _context.Exm_M_PromotionDMO
                                        from b in _context.Exm_M_Promotion_SubjectsDMO
                                        from c in _context.Exm_M_Prom_Subj_GroupDMO
                                        where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && a.EYC_Id == geteycid
                                        && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true && b.EMPS_ActiveFlag == true
                                        && c.EMPSG_ActiveFlag == true)
                                        select new JSHSExamReportsDTO
                                        {
                                            EMPG_GroupName = c.EMPSG_GroupName,
                                            EMPSG_Order = c.EMPSG_Order,
                                            EMPG_DistplayName = c.EMPSG_DisplayName
                                        }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

                data.getgroupexamdetails = (from a in _context.Exm_M_PromotionDMO
                                            from b in _context.Exm_M_Promotion_SubjectsDMO
                                            from c in _context.Exm_M_Prom_Subj_GroupDMO
                                            from d in _context.Exm_M_Prom_Subj_Group_ExamsDMO
                                            from e in _context.exammasterDMO
                                            where (a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id
                                            && d.EME_Id == e.EME_Id && a.EYC_Id == geteycid && a.MI_Id == data.MI_Id && a.EMP_ActiveFlag == true
                                            && b.EMPS_ActiveFlag == true && c.EMPSG_ActiveFlag == true && d.EMPSGE_ActiveFlg == true)
                                            select new JSHSExamReportsDTO
                                            {
                                                EMPG_GroupName = c.EMPSG_GroupName,
                                                EME_Id = d.EME_Id,
                                                EME_ExamName = e.EME_ExamName,
                                                EME_ExamCode = e.EME_ExamCode,
                                                EME_ExamOrder = e.EME_ExamOrder,
                                                EMPG_DistplayName = c.EMPSG_DisplayName,
                                                EMPSGE_ForMaxMarkrs = d.EMPSGE_ForMaxMarkrs
                                            }).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                data.getpromotionremarksdetails = _context.ExamPromotionRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                 && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.EPRD_Promotionflag == "PE" && ids.Contains(a.AMST_Id)).ToArray();

                var from_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_From_Date).FirstOrDefault();
                var to_date = _context.AcademicYear.Where(R => R.ASMAY_Id == data.ASMAY_Id).Select(p => p.ASMAY_To_Date).FirstOrDefault();
                //Exm_ProgressCard_RemarksDMO
                //NDS_Exam_6_8_ProgressCard_Total
                //getallgrade
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_CCE_Activities_Transaction";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getallgrade = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
               && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMER_ActiveFlag == true
               && ids.Contains(a.AMST_Id)).Distinct().ToArray();


                //added
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_BBHS_Student_SubjectWise__StThird";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    // cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudent_examwisemarks = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //added
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "9" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.savelisttot = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //NDS_EXAM_GET_6_8_STUDENT_DETAILS_Modify
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "11" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.St_ThomosTotal = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "STCS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 1000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "3" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    if (cmd.Connection.State != ConnectionState.Open) cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.Present_attendence = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                //using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "StudentAttendance_Stthomos";
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.CommandTimeout = 900000000;
                //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();
                //    var retObject1 = new List<dynamic>();
                //    try
                //    {
                //        using (var dataReader = cmd.ExecuteReader())
                //        {
                //            while (dataReader.Read())
                //            {
                //                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                //                {
                //                    dataRow1.Add(
                //                        dataReader.GetName(iFiled1),
                //                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                //                    );
                //                }

                //                retObject1.Add((ExpandoObject)dataRow1);
                //            }
                //        }
                //        data.Present_attendence = retObject1.ToArray();
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}

                //added New Things  
                if (data.reporttype == "superaverage")
                {

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //TBS_PRIMARY
        public JSHSExamReportsDTO TBSPrimarySchool(JSHSExamReportsDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> ids = new List<long>();
                if (data.Temp_AmstIds != null && data.Temp_AmstIds.Length > 0)
                {
                    foreach (var c in data.Temp_AmstIds)
                    {
                        ids.Add(c.AMST_Id);
                        amstids = amstids + "," + c.AMST_Id;
                    }
                }
                string emeids = "";
                List<long> EME_Ids = new List<long>();
                if (data.examlist != null && data.examlist.Length > 0)
                {
                    foreach (var c in data.examlist)
                    {
                        if (emeids == "")
                        {
                            emeids = c.EME_Id.ToString();
                        }
                        else
                        {
                            emeids = emeids + "," + c.EME_Id.ToString();
                        }
                        EME_Ids.Add(c.EME_Id);
                    }

                }

                data.getinstitution = _context.Institution_master.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToArray();

                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).FirstOrDefault();

                var geteycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == getemcaid && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).Select(a => a.EYC_Id).FirstOrDefault();

                // CLASS TEACHER NAME
                data.clstchname = (from a in _context.ClassTeacherMappingDMO
                                   from b in _context.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new JSHSExamReportsDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? "" : b.HRME_EmployeeFirstName) +
                                       (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "" ? "" : " " + b.HRME_EmployeeMiddleName) +
                                       (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "" ? "" : " " + b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                // STUDENT DETAILS
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "TBS_EXAM_GET_6_8_STUDENT_DETAILS_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });
                    cmd.Parameters.Add(new SqlParameter("@EMGD_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "TBS_Mark__report_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 9000000;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = "1" });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeids });
                    cmd.Parameters.Add(new SqlParameter("@EMGR_Id", SqlDbType.VarChar) { Value = data.EMGR_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwisesubjectlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                //aaded Marks 
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Stjames_Exam_Individual_Term_Report_Details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.VarChar)
                    {
                        Value = emeids
                    });

                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                    SqlDbType.VarChar)
                    {
                        Value = 3
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
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(
                                        dataReader.GetName(iFiled1),
                                        dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.getstudentwiseattendancedetails = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.examwiseremarks = _context.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true).Distinct().ToArray();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;


        }





    }
    class progressEqualityComparerjhs : IEqualityComparer<JSHSProgressCardReportDTO>
    {
        public bool Equals(JSHSProgressCardReportDTO b1, JSHSProgressCardReportDTO b2)
        {
            if (b2 == null && b1 == null)
                return true;
            else if (b1 == null | b2 == null)
                return false;
            else if (b1.ISMS_Id == b2.ISMS_Id)
                return true;
            else
                return false;
        }
        public int GetHashCode(JSHSProgressCardReportDTO bx)
        {
            int hCode = Convert.ToInt32(bx.ISMS_Id);
            return hCode.GetHashCode();
        }
    }
}
