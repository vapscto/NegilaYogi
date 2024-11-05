using DataAccessMsSqlServerProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using PreadmissionDTOs.com.vaps.MobileApp;
using DomainModel.Model.com.vaps.Exam;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;

namespace PortalHub.com.vaps.MobileApp.Services
{

    public class ExamImpl : Interfaces.ExamInterface
    {
        public DomainModelMsSqlServerContext _db;
        private readonly PortalContext _Portalcontext;
        private ExamContext _Examcontext;
        public ExamImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, ExamContext Exam)
        {
            _Portalcontext = cpContext;
            _Examcontext = Exam;
            _db = db;
        }
        public ExamDTO.getStudent getStudent(ExamDTO.getStudent data)
        {
            try
            {
                data.status = true;
                data.studetiallist = (from d in _Portalcontext.AcademicYearDMO
                                      from a in _Portalcontext.School_M_Class
                                      from b in _Portalcontext.School_M_Section
                                      from c in _Portalcontext.School_Adm_Y_StudentDMO
                                      where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
                                      && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                      select new ExamDTO.getStudent
                                      {
                                          ASMCL_Id = c.ASMCL_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,
                                          ASMS_Id = c.ASMS_Id,
                                          ASMC_SectionName = b.ASMC_SectionName,
                                          ASMAY_Id = c.ASMAY_Id,
                                          ASMAY_Year = d.ASMAY_Year,
                                          ASMAY_Order = d.ASMAY_Order
                                      }).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }
        public ExamDTO.getExamdata getExamdata(ExamDTO.getExamdata data)
        {
            try
            {
                data.status = true;
                long asmclid = 0;
                long asmsid = 0;
                var classSectionData = (from d in _Portalcontext.AcademicYearDMO
                                        from a in _Portalcontext.School_M_Class
                                        from b in _Portalcontext.School_M_Section
                                        from c in _Portalcontext.School_Adm_Y_StudentDMO
                                        where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.mI_ID && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                        select new ExamDTO.getExamdata
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMCL_ClassName = a.ASMCL_ClassName,
                                            ASMS_Id = c.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMAY_Id = c.ASMAY_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }).ToList();
                if (classSectionData.Count > 0)
                {
                    asmclid = classSectionData.FirstOrDefault().ASMCL_Id;
                    asmsid = classSectionData.FirstOrDefault().ASMS_Id;
                }
                else
                {
                    asmclid = 0;
                    asmsid = 0;
                }

                if (data.Type == "SWAE")
                {
                    data.subjectlist = (from ECC in _Examcontext.Exm_Category_ClassDMO
                                        from EYC in _Examcontext.Exm_Yearly_CategoryDMO
                                        from EYCE in _Examcontext.Exm_Yearly_Category_ExamsDMO
                                        from EYCES in _Examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                        from n in _Examcontext.StudentMappingDMO
                                        from i in _Examcontext.IVRM_School_Master_SubjectsDMO
                                        where (EYC.MI_Id == data.mI_ID && EYC.ASMAY_Id == data.ASMAY_Id && ECC.EMCA_Id == EYC.EMCA_Id && EYCE.EYC_Id == EYC.EYC_Id
                                        && EYCES.EYCE_Id == EYCE.EYCE_Id && n.ISMS_Id == EYCES.ISMS_Id && n.MI_Id == data.mI_ID && n.ASMAY_Id == data.ASMAY_Id
                                        && n.ASMCL_Id == asmclid && n.ASMS_Id == asmsid && EYC.MI_Id == data.mI_ID && EYC.ASMAY_Id == data.ASMAY_Id
                                        && ECC.ASMCL_Id == asmclid && ECC.ASMS_Id == asmsid
                                        && i.ISMS_Id == EYCES.ISMS_Id && i.MI_Id == data.mI_ID && n.AMST_Id == data.AMST_Id && n.ESTSU_ActiveFlg == true)
                                        select new ExamDTO.getExamdata
                                        {
                                            ISMS_Id = EYCES.ISMS_Id,
                                            ISMS_SubjectName = i.ISMS_SubjectName,
                                            ISMS_SubjectCode = i.ISMS_SubjectCode,
                                        }).Distinct().OrderBy(a => a.ISMS_SubjectName).ToArray();
                }
                else
                {
                    var EQuery = _Examcontext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.mI_ID && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == asmclid && t.ASMS_Id == asmsid && t.AMST_Id == data.AMST_Id && t.ESTMP_PublishToStudentFlg == true).Select(d => d.EME_Id).Distinct().ToList();

                    List<long> emeidnew = new List<long>();

                    var getexamlist = (from a in _Examcontext.Exm_Category_ClassDMO
                                       from b in _Examcontext.Exm_Master_CategoryDMO
                                       from c in _Examcontext.Exm_Yearly_CategoryDMO
                                       from d in _Examcontext.Exm_Yearly_Category_ExamsDMO
                                       from e in _Examcontext.ExmStudentMarksProcessDMO
                                       where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id && d.EME_Id == e.EME_Id && a.ECAC_ActiveFlag == true
                                       && b.EMCA_ActiveFlag == true && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true && a.ASMCL_Id == asmclid && a.ASMS_Id == asmsid
                                       && a.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == asmclid && e.ASMS_Id == asmsid && e.ASMAY_Id == data.ASMAY_Id
                                       && e.AMST_Id == data.AMST_Id &&
                                       ((d.EYCE_MarksPublishDate == null && e.ESTMP_PublishToStudentFlg == true)
                                       || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == false
                                       && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))
                                       || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == true && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))))
                                       select new exammasterDMO
                                       {
                                           EME_Id = d.EME_Id
                                       }).Distinct().ToList();

                    foreach (var e in getexamlist)
                    {
                        emeidnew.Add(e.EME_Id);
                    }

                    List<exammasterDMO> esmp = new List<exammasterDMO>();
                    esmp = _Portalcontext.exammasterDMO.Where(t => t.MI_Id == data.mI_ID && t.EME_ActiveFlag == true && emeidnew.Contains(t.EME_Id)).ToList();
                    data.examlist = esmp.OrderBy(a => a.EME_ExamOrder).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }

            return data;
        }
        //studentExamDetails
        public ExamDTO.studentExamDetails studentExamDetails(ExamDTO.studentExamDetails data)
        {
            try
            {
                data.status = true;

                long asmclid = 0;
                long asmsid = 0;
                var classSectionData = (from d in _Portalcontext.AcademicYearDMO
                                        from a in _Portalcontext.School_M_Class
                                        from b in _Portalcontext.School_M_Section
                                        from c in _Portalcontext.School_Adm_Y_StudentDMO
                                        where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                        select new ExamDTO.getExamdata
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMS_Id = c.ASMS_Id,
                                        }).ToList();

                if (classSectionData.Count > 0)
                {
                    asmclid = classSectionData.FirstOrDefault().ASMCL_Id;
                    asmsid = classSectionData.FirstOrDefault().ASMS_Id;
                }
                else
                {
                    asmclid = 0;
                    asmsid = 0;
                }

                data.getexamconfig = _Portalcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                using (var cmd = _Portalcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_EXAMREPORT_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                    {
                        Value = asmclid
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                    {
                        Value = asmsid
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.BigInt)
                    {
                        Value = data.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt)
                    {
                        Value = data.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = data.Type
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
                        data.examReportList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        data.status = false;
                    }
                }

                if (data.Type == "EWAS")
                {
                    var get_emca_id = _Examcontext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == asmclid
                    && a.ASMS_Id == asmsid && a.ECAC_ActiveFlag == true).Distinct().Select(a => a.EMCA_Id).FirstOrDefault();


                    var get_eyc_id = _Examcontext.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == get_emca_id
                    && a.EYC_ActiveFlg == true).Distinct().Select(a => a.EYC_Id).FirstOrDefault();

                    var get_eme_id_details = _Examcontext.Exm_Yearly_Category_ExamsDMO.Where(a => a.EYC_Id == get_eyc_id && a.EME_Id == data.EME_Id).Distinct().ToList();

                    data.get_eme_id_details = get_eme_id_details.ToArray();

                    if (get_eme_id_details.FirstOrDefault().EYCE_SubExamFlg == true || get_eme_id_details.FirstOrDefault().EYCE_SubSubjectFlg)
                    {
                        using (var cmd = _Examcontext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "PORTAL_EXAMREPORT_SUBSUBJECT_SUBEXAM_MODIFY";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                            {
                                Value = asmclid
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                            {
                                Value = asmsid
                            });

                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                            {
                                Value = data.AMST_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.BigInt)
                            {
                                Value = data.EME_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt)
                            {
                                Value = data.ISMS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                            {
                                Value = data.Type
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
                                data.subexamreportexamReportList = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                data.status = false;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }

        //Getdetails_IT
        public ExamDTO.getdetails_IT Getdetails_IT(ExamDTO.getdetails_IT data)
        {
            try
            {
                data.status = true;
                data.getyearlist = _Portalcontext.AcademicYearDMO.Where(a => a.MI_Id == data.mI_ID && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }
        //get_Exam_grade_pc
        public ExamDTO.getdetails_IT get_Exam_grade_pc(ExamDTO.getdetails_IT data)
        {
            try
            {
                data.status = true;
                var clssec1 = (from a in _Portalcontext.Adm_M_Student
                               from b in _Portalcontext.School_Adm_Y_StudentDMO
                               from c in _Portalcontext.School_M_Class
                               from s in _Portalcontext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.mI_ID
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new ExamDTO.getdetails_IT
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();

                data.ASMCL_Id = clssec1.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = clssec1.FirstOrDefault().ASMS_Id;



                var getemcaid = _Examcontext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.mI_ID && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
               && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToList();

                var eycid = _Examcontext.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                && a.EYC_ActiveFlg == true).ToList();


                List<long> emeidnew = new List<long>();

                var getexamlist = (from a in _Examcontext.Exm_Category_ClassDMO
                                   from b in _Examcontext.Exm_Master_CategoryDMO
                                   from c in _Examcontext.Exm_Yearly_CategoryDMO
                                   from d in _Examcontext.Exm_Yearly_Category_ExamsDMO
                                   from e in _Examcontext.ExmStudentMarksProcessDMO
                                   where (a.EMCA_Id == b.EMCA_Id && b.EMCA_Id == c.EMCA_Id && c.EYC_Id == d.EYC_Id
                                   && d.EME_Id == e.EME_Id && a.ECAC_ActiveFlag == true
                                   && b.EMCA_ActiveFlag == true && c.EYC_ActiveFlg == true
                                   && d.EYCE_ActiveFlg == true && a.ASMCL_Id == data.ASMCL_Id
                                   && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                   && e.ASMCL_Id == data.ASMCL_Id
                                   && e.ASMS_Id == data.ASMS_Id && e.ASMAY_Id == data.ASMAY_Id
                                   && e.AMST_Id == data.AMST_Id &&
                                   ((d.EYCE_MarksPublishDate == null && e.ESTMP_PublishToStudentFlg == true)
                                   || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == false
                                   && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))
                                   || (d.EYCE_MarksPublishDate != null && e.ESTMP_PublishToStudentFlg == true && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(d.EYCE_MarksPublishDate))))
                                   select new exammasterDMO
                                   {
                                       EME_Id = d.EME_Id
                                   }).Distinct().ToList();

                foreach (var e in getexamlist)
                {
                    emeidnew.Add(e.EME_Id);
                }


                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _Examcontext.exammasterDMO.Where(t => t.MI_Id == data.mI_ID && t.EME_ActiveFlag == true && emeidnew.Contains(t.EME_Id)).ToList();

                data.getexam = esmp.OrderBy(a => a.EME_ExamOrder).Distinct().ToArray();



                data.getgradelist = (from a in _Examcontext.Exm_Yearly_CategoryDMO
                                     from b in _Examcontext.Exm_Yearly_Category_ExamsDMO
                                     from c in _Examcontext.Exm_Master_GradeDMO
                                     where (a.EYC_Id == b.EYC_Id && b.EMGR_Id == c.EMGR_Id && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id
                                     && a.ASMAY_Id == data.ASMAY_Id)
                                     select c).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;

        }
        //saveddata_pc
        public ExamDTO.getdetails_IT saveddata_pc(ExamDTO.getdetails_IT data)
        {
            try
            {
                data.status = true;
                data.instname = _Portalcontext.Institution_master.Where(t => t.MI_Id == data.mI_ID).ToArray();

                var clssec1 = (from a in _Portalcontext.Adm_M_Student
                               from b in _Portalcontext.School_Adm_Y_StudentDMO
                               from c in _Portalcontext.School_M_Class
                               from s in _Portalcontext.School_M_Section
                               where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.mI_ID
                               && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.AMST_Id == data.AMST_Id)
                               select new ExamDTO.getdetails_IT
                               {
                                   ASMCL_Id = c.ASMCL_Id,
                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                   ASMS_Id = s.ASMS_Id,
                                   ASMC_SectionName = s.ASMC_SectionName
                               }).Distinct().ToList();
                data.ASMCL_Id = clssec1.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = clssec1.FirstOrDefault().ASMS_Id;

                data.clstchname = (from a in _Portalcontext.ClassTeacherMappingDMO
                                   from b in _Portalcontext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.mI_ID && a.ASMAY_Id == data.ASMAY_Id
                                   && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.IMCT_ActiveFlag == true)
                                   select new ExamDTO.getdetails_IT
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                   }).Distinct().ToArray();

                List<ExamDTO.getsProgressReport> result = new List<ExamDTO.getsProgressReport>();

                using (var cmd = _Portalcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 450000000;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.ASMAY_Id)
                    });


                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.ASMS_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.mI_ID)
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                        SqlDbType.TinyInt)
                    {
                        Value = Convert.ToInt32(data.EME_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                                SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt32(data.AMST_Id)
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
                                result.Add(new ExamDTO.getsProgressReport
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

                var from_date = (from a in _Examcontext.Exm_Category_ClassDMO
                                 from b in _Examcontext.Exm_Yearly_CategoryDMO
                                 from c in _Examcontext.Exm_Yearly_Category_ExamsDMO
                                 where (a.MI_Id == data.mI_ID && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id && b.ASMAY_Id == data.ASMAY_Id)
                                 select c.EYCE_AttendanceFromDate).FirstOrDefault();
                var to_date = (from a in _Examcontext.Exm_Category_ClassDMO
                               from b in _Examcontext.Exm_Yearly_CategoryDMO
                               from c in _Examcontext.Exm_Yearly_Category_ExamsDMO
                               where (a.MI_Id == data.mI_ID && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && b.EYC_Id == c.EYC_Id && c.EME_Id == data.EME_Id && b.ASMAY_Id == data.ASMAY_Id)
                               select c.EYCE_AttendanceToDate).Max();

                using (var cmd = _Examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "adm_exam_student_attendance_details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.mI_ID
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
                            //data.savelist = retObject.ToArray();

                        }
                        data.Present_attendence = retObject1.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.savelisttot = _Examcontext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.mI_ID && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id).Distinct().ToArray();

                data.subjlist = data.savelist.Distinct<ExamDTO.getsProgressReport>(new progressEqualityComparerjhs()).OrderBy(t => t.EYCES_SubjectOrder).ToArray();


                List<int> grade = new List<int>();
                foreach (ExamDTO.getdetails_IT x in data.subjlist)
                {
                    grade.Add(x.EMGR_Id);
                }

                data.grade_details = (from a in _Examcontext.Exm_Master_GradeDMO
                                      from b in _Examcontext.Exm_Master_Grade_DetailsDMO
                                      where (a.MI_Id == data.mI_ID && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                      select b
                                     ).Distinct().ToArray();

                data.examwiseremarks = _Examcontext.Exm_ProgressCard_RemarksDMO.Where(a => a.MI_Id == data.mI_ID && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_ID == data.EME_Id && a.EMER_ActiveFlag == true).Distinct().ToArray();

                data.getstudentdetails = (from a in _Portalcontext.School_Adm_Y_StudentDMO
                                          from b in _Portalcontext.Adm_M_Student
                                          from c in _Portalcontext.School_M_Class
                                          from d in _Portalcontext.School_M_Section
                                          from e in _Portalcontext.AcademicYearDMO
                                          where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                          && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id
                                          && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.mI_ID && a.ASMS_Id == data.ASMS_Id)
                                          select new ExamDTO.getdetails_IT
                                          {
                                              AMST_Id = a.AMST_Id,
                                              photoname = b.AMST_Photoname

                                          }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.status = false;
            }
            return data;
        }
        class progressEqualityComparerjhs : IEqualityComparer<ExamDTO.getsProgressReport>
        {
            public bool Equals(ExamDTO.getsProgressReport b1, ExamDTO.getsProgressReport b2)
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
            public int GetHashCode(ExamDTO.getsProgressReport bx)
            {
                int hCode = Convert.ToInt32(bx.ISMS_Id);
                return hCode.GetHashCode();
            }
        }
    }
}
