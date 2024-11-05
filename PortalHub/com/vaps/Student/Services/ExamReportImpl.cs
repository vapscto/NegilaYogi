using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;

namespace PortalHub.com.vaps.Student.Services
{
    public class ExamReportImpl : Interfaces.ExamReportInterface
    {
        private static ConcurrentDictionary<string, ExamDTO> _login =
           new ConcurrentDictionary<string, ExamDTO>();
        private PortalContext _Portalcontext;
        private ExamContext _Examcontext;
        public ExamReportImpl(PortalContext Feecontext, ExamContext Democontext)
        {
            _Examcontext = Democontext;
            _Portalcontext = Feecontext;
        }
        public ExamDTO getloaddata(ExamDTO data)
        {
            try
            {
                data.studetiallist = (from d in _Portalcontext.AcademicYearDMO
                                      from a in _Portalcontext.School_M_Class
                                      from b in _Portalcontext.School_M_Section
                                      from c in _Portalcontext.School_Adm_Y_StudentDMO
                                      where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
                                      && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                      select new ExamDTO
                                      {
                                          ASMCL_Id = c.ASMCL_Id,
                                          ASMCL_ClassName = a.ASMCL_ClassName,
                                          ASMS_Id = c.ASMS_Id,
                                          ASMC_SectionName = b.ASMC_SectionName,
                                          ASMAY_Id = c.ASMAY_Id,
                                          ASMAY_Year = d.ASMAY_Year,
                                          ASMAY_Order=d.ASMAY_Order
                                      }).Distinct().OrderByDescending(t=>t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ExamDTO> getexamdata(ExamDTO data)
        {
            try
            {
                long asmclid = 0;
                long asmsid = 0;
                var classSectionData = (from d in _Portalcontext.AcademicYearDMO
                                        from a in _Portalcontext.School_M_Class
                                        from b in _Portalcontext.School_M_Section
                                        from c in _Portalcontext.School_Adm_Y_StudentDMO
                                        where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                        select new ExamDTO
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

                if (data.Type == "Overall")
                {
                    await StudentExamDetails(data);
                }
                else if (data.Type == "SWAE")
                {
                     data.subjectlist = (from ECC in _Examcontext.Exm_Category_ClassDMO
                                        from EYC in _Examcontext.Exm_Yearly_CategoryDMO
                                        from EYCE in _Examcontext.Exm_Yearly_Category_ExamsDMO
                                        from EYCES in _Examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                        from n in _Examcontext.StudentMappingDMO
                                        from i in _Examcontext.IVRM_School_Master_SubjectsDMO
                                        //from ef in _Examcontext.ExmStudentMarksProcessDMO
                                        where (EYC.MI_Id == data.MI_Id && EYC.ASMAY_Id == data.ASMAY_Id && ECC.EMCA_Id == EYC.EMCA_Id && EYCE.EYC_Id == EYC.EYC_Id
                                        && EYCES.EYCE_Id == EYCE.EYCE_Id && n.ISMS_Id == EYCES.ISMS_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id
                                        && n.ASMCL_Id == asmclid && n.ASMS_Id == asmsid && EYC.MI_Id == data.MI_Id && EYC.ASMAY_Id == data.ASMAY_Id
                                        && ECC.ASMCL_Id == asmclid && ECC.ASMS_Id == asmsid 
                                        //&& EYCE.EME_Id == ef.EME_Id && ef.AMST_Id == data.AMST_Id && ef.ESTMP_PublishToStudentFlg == true
                                        && i.ISMS_Id == EYCES.ISMS_Id && i.MI_Id == data.MI_Id && n.AMST_Id == data.AMST_Id && n.ESTSU_ActiveFlg == true)
                                        select new ProgressCardReportDTO
                                        {
                                            ISMS_Id = EYCES.ISMS_Id,
                                            ISMS_SubjectName = i.ISMS_SubjectName,
                                            ISMS_SubjectCode = i.ISMS_SubjectCode,
                                           // EYCES_SubjectOrder = EYCES.EYCES_SubjectOrder
                                        }).Distinct().OrderBy(a => a.ISMS_SubjectName).ToArray();
                }
                else
                {
                    var EQuery = _Examcontext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == asmclid && t.ASMS_Id == asmsid && t.AMST_Id == data.AMST_Id && t.ESTMP_PublishToStudentFlg == true).Select(d => d.EME_Id).Distinct().ToList();

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
                    esmp = _Portalcontext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeidnew.Contains(t.EME_Id)).ToList();
                    data.examlist = esmp.OrderBy(a => a.EME_ExamOrder).Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamDTO getSubjects(ExamDTO data)
        {
            try
            {
                long asmclid = 0;
                long asmsid = 0;
                var classSectionData = (from d in _Portalcontext.AcademicYearDMO
                                        from a in _Portalcontext.School_M_Class
                                        from b in _Portalcontext.School_M_Section
                                        from c in _Portalcontext.School_Adm_Y_StudentDMO
                                        where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                        select new ExamDTO
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMCL_ClassName = a.ASMCL_ClassName,
                                            ASMS_Id = c.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMAY_Id = c.ASMAY_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }
                         ).Distinct().ToList();
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

                data.subjectlist = (from ECC in _Examcontext.Exm_Category_ClassDMO
                                    from EYC in _Examcontext.Exm_Yearly_CategoryDMO
                                    from EYCE in _Examcontext.Exm_Yearly_Category_ExamsDMO
                                    from EYCES in _Examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                    from n in _Examcontext.StudentMappingDMO
                                    from i in _Examcontext.IVRM_School_Master_SubjectsDMO
                                    from ef in _Examcontext.ExmStudentMarksProcessDMO
                                    where (EYC.MI_Id == data.MI_Id && EYC.ASMAY_Id == data.ASMAY_Id && ECC.EMCA_Id == EYC.EMCA_Id && EYCE.EYC_Id == EYC.EYC_Id
                                    && EYCES.EYCE_Id == EYCE.EYCE_Id && n.ISMS_Id == EYCES.ISMS_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id
                                    && n.ASMCL_Id == asmclid && n.ASMS_Id == asmsid && EYC.MI_Id == data.MI_Id && EYC.ASMAY_Id == data.ASMAY_Id
                                    && EYCE.EME_Id == ef.EME_Id && ef.AMST_Id == data.AMST_Id && ef.ESTMP_PublishToStudentFlg == true
                                    && ECC.ASMCL_Id == asmclid && ECC.ASMS_Id == asmsid && EYCE.EME_Id == data.EME_Id
                                    && i.ISMS_Id == EYCES.ISMS_Id && i.MI_Id == data.MI_Id && n.AMST_Id == data.AMST_Id && n.ESTSU_ActiveFlg == true)
                                    select new ProgressCardReportDTO
                                    {
                                        ISMS_Id = EYCES.ISMS_Id,
                                        ISMS_SubjectName = i.ISMS_SubjectName,
                                        ISMS_SubjectCode = i.ISMS_SubjectCode,
                                        //EYCES_SubjectOrder = EYCES.EYCES_SubjectOrder
                                    }
                    ).Distinct().OrderBy(a => a.ISMS_SubjectName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ExamDTO> StudentExamDetails(ExamDTO data)
        {
            try
            {
                long asmclid = 0;
                long asmsid = 0;
                var classSectionData = (from d in _Portalcontext.AcademicYearDMO
                                        from a in _Portalcontext.School_M_Class
                                        from b in _Portalcontext.School_M_Section
                                        from c in _Portalcontext.School_Adm_Y_StudentDMO
                                        where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                        select new ExamDTO
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

                data.getexamconfig = _Examcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                using (var cmd = _Examcontext.Database.GetDbConnection().CreateCommand())
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
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                    }
                }

                if(data.Type== "EWAS")
                {
                    var get_emca_id = _Examcontext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == asmclid
                    && a.ASMS_Id == asmsid && a.ECAC_ActiveFlag == true).Distinct().Select(a => a.EMCA_Id).FirstOrDefault();


                    var get_eyc_id = _Examcontext.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == get_emca_id 
                    && a.EYC_ActiveFlg == true).Distinct().Select(a => a.EYC_Id).FirstOrDefault();

                    var get_eme_id_details = _Examcontext.Exm_Yearly_Category_ExamsDMO.Where(a => a.EYC_Id == get_eyc_id && a.EME_Id == data.EME_Id).Distinct().ToList();

                    data.get_eme_id_details = get_eme_id_details.ToArray();

                    if (get_eme_id_details.FirstOrDefault().EYCE_SubExamFlg==true || get_eme_id_details.FirstOrDefault().EYCE_SubSubjectFlg)
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
                                using (var dataReader = await cmd.ExecuteReaderAsync())
                                {
                                    while (await dataReader.ReadAsync())
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
                            }
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
    }
}
