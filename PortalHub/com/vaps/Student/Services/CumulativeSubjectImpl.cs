using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using System.Collections.Generic;

namespace PortalHub.com.vaps.Student.Services
{
    public class CumulativeSubjectImpl : Interfaces.CumulativeSubjectInterface
    {
        private static ConcurrentDictionary<string, ExamDTO> _login =
           new ConcurrentDictionary<string, ExamDTO>();
        private PortalContext _Examcontext;
        public CumulativeSubjectImpl(PortalContext Feecontext)
        {
            _Examcontext = Feecontext;
        }
        public ExamDTO getloaddata(ExamDTO data)
        {
            try
            {
                data.stuyearlist = (from d in _Examcontext.AcademicYearDMO
                                    from a in _Examcontext.School_M_Class
                                    from b in _Examcontext.School_M_Section
                                    from c in _Examcontext.School_Adm_Y_StudentDMO
                                    where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                    a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id)
                                    select new ExamDTO
                                    {
                                        ASMCL_Id = c.ASMCL_Id,
                                        ASMCL_ClassName = a.ASMCL_ClassName,
                                        ASMS_Id = c.ASMS_Id,
                                        ASMC_SectionName = b.ASMC_SectionName,
                                        ASMAY_Id = c.ASMAY_Id,
                                        ASMAY_Year = d.ASMAY_Year,
                                        ASMAY_Order=d.ASMAY_Order
                                    }
                             ).OrderByDescending(t=>t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamDTO getSubjectsdata(ExamDTO data)
        {
            try
            {
                var classSectionData = (from d in _Examcontext.AcademicYearDMO
                                        from a in _Examcontext.School_M_Class
                                        from b in _Examcontext.School_M_Section
                                        from c in _Examcontext.School_Adm_Y_StudentDMO
                                        where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                        a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                                        select new ExamDTO
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMCL_ClassName = a.ASMCL_ClassName,
                                            ASMS_Id = c.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMAY_Id = c.ASMAY_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }
                             ).ToList();
                data.stuyearlist = classSectionData.ToArray();

                data.subjectlist = (from a in _Examcontext.StudentMappingDMO
                                    from b in _Examcontext.IVRM_Master_SubjectsDMO
                                    where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == classSectionData.FirstOrDefault().ASMCL_Id && a.ASMS_Id == classSectionData.FirstOrDefault().ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                    && a.AMST_Id == data.AMST_Id && b.MI_Id == data.MI_Id)
                                    select new ExamDTO
                                    {
                                        ISMS_Id = b.ISMS_Id,
                                        ISMS_SubjectName = b.ISMS_SubjectName
                                    }
                             ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamDTO getexamdetails(ExamDTO data)
        {
            try
            {
                var classSectionData = (from d in _Examcontext.AcademicYearDMO
                                        from a in _Examcontext.School_M_Class
                                        from b in _Examcontext.School_M_Section
                                        from c in _Examcontext.School_Adm_Y_StudentDMO
                                        where (c.AMST_Id == data.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id &&
                                        a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                                        select new ExamDTO
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            ASMCL_ClassName = a.ASMCL_ClassName,
                                            ASMS_Id = c.ASMS_Id,
                                            ASMC_SectionName = b.ASMC_SectionName,
                                            ASMAY_Id = c.ASMAY_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }).ToList();

                data.stuyearlist = classSectionData.ToArray();

                var subjectData = (from a in _Examcontext.StudentMappingDMO
                                   from b in _Examcontext.IVRM_Master_SubjectsDMO
                                   where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == classSectionData.FirstOrDefault().ASMCL_Id && a.ASMS_Id == classSectionData.FirstOrDefault().ASMS_Id && a.ASMAY_Id == data.ASMAY_Id
                                     && a.AMST_Id == data.AMST_Id && b.MI_Id == data.MI_Id)
                                   select new ExamDTO
                                   {
                                       ISMS_Id = b.ISMS_Id,
                                       ISMS_SubjectName = b.ISMS_SubjectName
                                   }).ToList();

                data.subjectlist = subjectData.ToArray();
                //Exm_Calculation_LogDMO
                var Examlist = _Examcontext.ExmStudentMarksProcessDMO.Where(R => R.MI_Id == data.MI_Id && R.ASMAY_Id == data.ASMAY_Id && R.ASMCL_Id == classSectionData.FirstOrDefault().ASMCL_Id && R.ASMS_Id == classSectionData.FirstOrDefault().ASMS_Id && R.AMST_Id == data.AMST_Id && R.ESTMP_PublishToStudentFlg==true).ToList();

                List<long> EME_Id = new List<long>();
                if (Examlist.Count >0)
                {
                   foreach(var i in Examlist)
                    {
                        EME_Id.Add(i.EME_Id);
                    }
                }
                data.examgradelist = (from a in _Examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                      from b in _Examcontext.exammasterDMO
                                      from c in _Examcontext.AcademicYearDMO
                                      where (a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id 
                                      && a.ASMCL_Id == classSectionData.FirstOrDefault().ASMCL_Id && a.ASMS_Id == classSectionData.FirstOrDefault().ASMS_Id
                                      && a.AMST_Id == data.AMST_Id && a.ISMS_Id == data.ISMS_Id && b.EME_Id == a.EME_Id && EME_Id.Contains(a.EME_Id))                                     
                                      select new ExamDTO
                                      {
                                          EME_Id = a.EME_Id,
                                          EME_ExamName = b.EME_ExamName,
                                          ESTMPS_ObtainedGrade = a.ESTMPS_ObtainedGrade,
                                          ESTMPS_ObtainedMarks = a.ESTMPS_ObtainedMarks,
                                          ESTMPS_MaxMarks = a.ESTMPS_MaxMarks,
                                          ESTMPS_PassFailFlg = a.ESTMPS_PassFailFlg,
                                         // percentage =  Math.Round(a.ESTMPS_MaxMarks(a.ESTMPS_ObtainedMarks), 2),
                                          ASMAY_Year = c.ASMAY_Year
                                      }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}