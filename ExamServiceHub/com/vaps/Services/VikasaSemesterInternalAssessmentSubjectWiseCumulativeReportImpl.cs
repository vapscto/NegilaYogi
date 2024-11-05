using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class VikasaSemesterInternalAssessmentSubjectWiseCumulativeReportImpl:Interfaces.VikasaSemesterInternalAssessmentSubjectWiseCumulativeReportInterface
    {
        private static ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO> _login =
      new ConcurrentDictionary<string, VikasaSubjectwiseCumulativeReportDTO>();

        private readonly ExamContext _ReportContext;
        public StudentAttendanceReportContext _db;
        ILogger<VikasaSemesterInternalAssessmentSubjectWiseCumulativeReportImpl> _acdimpl;
        public VikasaSemesterInternalAssessmentSubjectWiseCumulativeReportImpl(ExamContext cpContext, StudentAttendanceReportContext db)
        {
            _ReportContext = cpContext;
            _db = db;
        }
        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)//int IVRMM_Id
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _ReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = list.ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public VikasaSubjectwiseCumulativeReportDTO showdetails(VikasaSubjectwiseCumulativeReportDTO data)
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

                data.BasicListYear = (from a in _ReportContext.AcademicYear
                                      where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          yearname = a.ASMAY_Year
                                      }).Distinct().ToArray();


                data.BasiListclass = (from a in _db.admissionyearstudent
                                      from b in _db.admissionClass
                                      where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == data.MI_Id 
                                      && b.ASMCL_Id == data.ASMCL_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          ClassName = b.ASMCL_ClassName
                                      }).Distinct().ToArray();

                data.BasiListsectiont = (from a in _db.admissionyearstudent
                                         from b in _db.masterSection

                                         where (a.ASMAY_Id == data.ASMAY_Id  && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == data.MI_Id 
                                         && a.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                         select new VikasaSubjectwiseCumulativeReportDTO
                                         {
                                             sectionname = b.ASMC_SectionName
                                         }).Distinct().ToArray();

                data.BasiListsubject = (from a in _ReportContext.Exm_Category_ClassDMO
                                        from b in _ReportContext.Exm_Yearly_CategoryDMO
                                        from c in _ReportContext.Exm_Yearly_Category_ExamsDMO
                                        from d in _ReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                        from e in _ReportContext.IVRM_School_Master_SubjectsDMO
                                        where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EYCE_Id == d.EYCE_Id && d.ISMS_Id == e.ISMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && b.EMCA_Id == data.EMCA_Id && d.ISMS_Id == data.ISMS_Id)
                                        select new VikasaSubjectwiseCumulativeReportDTO
                                        {
                                            ISMS_Id = d.ISMS_Id,
                                            SubjectName = e.ISMS_SubjectName
                                        }).Distinct().ToArray();
                data.BasiListcategory = _ReportContext.Exm_Master_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.EMCA_Id == data.EMCA_Id).Distinct().ToArray();
                //for subject wise grade
                data.gradelist = _ReportContext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == data.ASMCL_Id && t.ISMS_Id == data.ISMS_Id && t.ASMS_Id == data.ASMS_Id && t.ASMAY_Id == data.ASMAY_Id).Distinct().ToArray();

                //for student details
                data.studentList = (from a in _db.admissionyearstudent
                                    from b in _db.admissionStduent
                                    where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && ids.Contains(a.AMAY_ActiveFlag) && a.ASMS_Id == data.ASMS_Id
                                    && b.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && ids.Contains(b.AMST_ActiveFlag)
                                    && sol.Contains(b.AMST_SOL))
                                    select b).Distinct().ToArray();


                //for student Exam Group head
                var dt = (from a in _ReportContext.Exm_Yearly_CategoryDMO
                          from b in _ReportContext.Exm_Category_ClassDMO
                          from c in _ReportContext.Exm_Master_CategoryDMO
                          where a.EMCA_Id == b.EMCA_Id && a.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == c.EMCA_Id && b.ECAC_ActiveFlag == true && b.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id
                          select new VikasaSubjectwiseCumulativeReportDTO
                          {
                              EYC_Id = a.EYC_Id
                          }).Distinct().ToList();
                var EQuery = dt.Select(d => d.EYC_Id).ToList();

                data.ExamGroupname = (from a in _ReportContext.Exm_M_PromotionDMO
                                      from b in _ReportContext.Exm_M_Promotion_SubjectsDMO
                                      from c in _ReportContext.Exm_M_Prom_Subj_GroupDMO
                                      where (a.EMP_Id == b.EMP_Id && c.EMPS_Id == b.EMPS_Id && a.MI_Id == data.MI_Id && EQuery.Contains(a.EYC_Id) && c.EMPSG_ActiveFlag == true && b.ISMS_Id == data.ISMS_Id)
                                      select new VikasaSubjectwiseCumulativeReportDTO
                                      {
                                          EMPSG_GroupName = c.EMPSG_GroupName,
                                          EMPSG_PercentValue = c.EMPSG_PercentValue,
                                          EMPSG_MarksValue = c.EMPSG_MarksValue,
                                          EMPSG_Id = c.EMPSG_Id
                                      }).Distinct().ToArray();

                //for student Exam Group Marks

                var dt1 = (from a in _ReportContext.Exm_M_PromotionDMO
                           from b in _ReportContext.Exm_M_Promotion_SubjectsDMO
                           from c in _ReportContext.Exm_M_Prom_Subj_GroupDMO
                           where (a.EMP_Id == b.EMP_Id && c.EMPS_Id == b.EMPS_Id && a.MI_Id == data.MI_Id && EQuery.Contains(a.EYC_Id) && c.EMPSG_ActiveFlag == true)
                           select new VikasaSubjectwiseCumulativeReportDTO
                           {
                               EMPSG_Id = c.EMPSG_Id
                           }).Distinct().ToList();
                var EQuery1 = dt1.Select(d => d.EMPSG_Id).ToList();

                data.examgroupmarks = (from a in _ReportContext.Exm_M_Prom_Subj_Group_ExamsDMO
                                       from b in _ReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                       where a.EME_Id == b.EME_Id && EQuery1.Contains(a.EMPSG_Id) && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ISMS_Id == data.ISMS_Id
                                       group new { a, b }
                                   by new { b.AMST_Id, a.EMPSG_Id } into g
                                       select new VikasaSubjectwiseCumulativeReportDTO
                                       {
                                           AMST_Id = g.FirstOrDefault().b.AMST_Id,
                                           EMPSG_Id = g.FirstOrDefault().a.EMPSG_Id,
                                           ESTMPS_MaxMarks = g.Sum(d => d.b.ESTMPS_MaxMarks),
                                           ESTMPS_ObtainedMarks = g.Sum(d => d.b.ESTMPS_ObtainedMarks)
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }
        public VikasaSubjectwiseCumulativeReportDTO get_class(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                dto.classlist = (from c in _db.admissionClass
                                 from d in _db.Exm_Category_ClassDMO
                                 where (d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true && d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id)
                                 select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public VikasaSubjectwiseCumulativeReportDTO get_section(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                dto.sectionList = (from b in _db.admissionClass
                                   from c in _db.masterSection
                                   from d in _db.Exm_Category_ClassDMO
                                   where (b.ASMCL_Id == d.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && d.ECAC_ActiveFlag == true && d.ASMCL_Id == dto.ASMCL_Id
                                   && c.MI_Id == dto.MI_Id && d.MI_Id == dto.MI_Id && d.ASMAY_Id == dto.ASMAY_Id)
                                   select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public VikasaSubjectwiseCumulativeReportDTO get_subject(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                dto.subjectList = (from a in _ReportContext.Exm_Category_ClassDMO
                                   from b in _ReportContext.Exm_Yearly_CategoryDMO
                                   from c in _ReportContext.Exm_Yearly_Category_ExamsDMO
                                   from d in _ReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                   from e in _ReportContext.IVRM_School_Master_SubjectsDMO
                                   where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EYCE_Id == d.EYCE_Id && d.ISMS_Id == e.ISMS_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMS_Id == dto.ASMS_Id && a.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id && b.EMCA_Id == dto.EMCA_Id)
                                   select new VikasaSubjectwiseCumulativeReportDTO
                                   {
                                       ISMS_Id = d.ISMS_Id,
                                       SubjectName = e.ISMS_SubjectName
                                   }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
        public VikasaSubjectwiseCumulativeReportDTO get_category(VikasaSubjectwiseCumulativeReportDTO dto)
        {
            try
            {
                dto.categoryList = _ReportContext.Exm_Master_CategoryDMO.Where(t => t.MI_Id == dto.MI_Id).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
    }
}
