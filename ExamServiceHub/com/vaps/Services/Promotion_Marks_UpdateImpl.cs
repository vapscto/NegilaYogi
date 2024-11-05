using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.TT;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class Promotion_Marks_UpdateImpl : Promotion_Marks_UpdateInterface
    {
        private readonly ExamContext _examcontext;
        ILogger<Baldwin_Final_P_ReportImpl> _acdimpl;
        public Promotion_Marks_UpdateImpl(ExamContext exm)
        {
            _examcontext = exm;
        }
        public Promotion_Marks_UpdateDTO Getdetails(Promotion_Marks_UpdateDTO data)
        {
            try
            {
                data.yearlist = _examcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.categorylist = _examcontext.Exm_Master_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.EMCA_ActiveFlag == true).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }

        public Promotion_Marks_UpdateDTO get_categories(Promotion_Marks_UpdateDTO data)
        {
            try
            {
                //data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                //                     from b in _examcontext.Exm_Yearly_CategoryDMO
                //                     where (a.MI_Id == data.MI_Id && a.EMCA_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == a.EMCA_Id && b.EYC_ActiveFlg == true)
                //                     select a).Distinct().ToArray();
                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     from c in _examcontext.Exm_M_PromotionDMO
                                     where (a.MI_Id == data.MI_Id && a.EMCA_ActiveFlag == true && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == a.EMCA_Id && b.EYC_ActiveFlg == true && c.MI_Id == data.MI_Id && c.EMP_ActiveFlag == true && (c.EMP_MarksPerFlg == "P") && c.EYC_Id == b.EYC_Id)//|| c.EMP_MarksPerFlg == "M"
                                     select new Exm_Master_CategoryDMO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName
                                     }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public Promotion_Marks_UpdateDTO get_classes(Promotion_Marks_UpdateDTO data)
        {
            try
            {
                data.classlist = (from a in _examcontext.AdmissionClass
                                  from b in _examcontext.Exm_Category_ClassDMO
                                  where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag == true && a.MI_Id == b.MI_Id && a.ASMCL_ActiveFlag == true && a.ASMCL_Id == b.ASMCL_Id)
                                  select a).Distinct().OrderBy(t=>t.ASMCL_Order).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public Promotion_Marks_UpdateDTO get_sections(Promotion_Marks_UpdateDTO data)
        {
            try
            {
                data.sectionlist = (from a in _examcontext.School_M_Section
                                    from b in _examcontext.Exm_Category_ClassDMO
                                    where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.ECAC_ActiveFlag == true && a.MI_Id == b.MI_Id && a.ASMC_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMCL_Id == data.ASMCL_Id)
                                    select a).Distinct().OrderBy(t=>t.ASMC_Order).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
        public Promotion_Marks_UpdateDTO get_subjects(Promotion_Marks_UpdateDTO data)
        {
            try
            {
                var promo_subjects = (from a in _examcontext.Exm_Yearly_CategoryDMO
                                      from b in _examcontext.Exm_M_PromotionDMO
                                      from c in _examcontext.Exm_M_Promotion_SubjectsDMO
                                      where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && a.EYC_ActiveFlg == true && b.MI_Id == a.MI_Id && b.EMP_ActiveFlag == true && b.EYC_Id == a.EYC_Id && c.EMP_Id == b.EMP_Id && c.EMPS_ActiveFlag == true)
                                      select c.ISMS_Id).Distinct().ToList();


                data.subjectlist = (from a in _examcontext.IVRM_School_Master_SubjectsDMO
                                    from b in _examcontext.Exm_Yearly_CategoryDMO
                                    from c in _examcontext.Exm_Yearly_Category_GroupDMO
                                    from d in _examcontext.Exm_Yearly_Category_Group_SubjectsDMO
                                    from e in _examcontext.StudentMappingDMO
                                    where (a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1 && a.ISMS_ExamFlag == 1 && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.EMCA_Id == data.EMCA_Id && b.EYC_ActiveFlg == true && c.EYCG_ActiveFlg == true && c.EYC_Id == b.EYC_Id && d.EYCG_Id == c.EYCG_Id && d.EYCGS_ActiveFlg == true && d.ISMS_Id == a.ISMS_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id && e.ASMS_Id == data.ASMS_Id && e.ESTSU_ActiveFlg == true && e.ISMS_Id == a.ISMS_Id && promo_subjects.Contains(e.ISMS_Id))
                                    select new IVRM_School_Master_SubjectsDMO
                                    {
                                        ISMS_Id = a.ISMS_Id,
                                        ISMS_SubjectName = a.ISMS_SubjectName,
                                        ISMS_SubjectCode = a.ISMS_SubjectCode
                                    }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }

        public Promotion_Marks_UpdateDTO get_prommarks(Promotion_Marks_UpdateDTO data)
        {
            try
            {
                var prom_subj_groups = (from a in _examcontext.Exm_Yearly_CategoryDMO
                                        from b in _examcontext.Exm_M_PromotionDMO
                                        from c in _examcontext.Exm_M_Promotion_SubjectsDMO
                                        from d in _examcontext.Exm_M_Prom_Subj_GroupDMO
                                        where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && a.EYC_ActiveFlg == true && b.MI_Id == a.MI_Id && b.EMP_ActiveFlag == true && b.EYC_Id == a.EYC_Id && c.EMP_Id == b.EMP_Id && c.EMPS_ActiveFlag == true && d.EMPSG_ActiveFlag == true && d.EMPS_Id == c.EMPS_Id && c.ISMS_Id == data.ISMS_Id)
                                        select d).Distinct().ToList();

                data.prom_subj_groups = prom_subj_groups.ToArray();

                var prom_students = (from a in _examcontext.Adm_M_Student
                                     from b in _examcontext.School_Adm_Y_StudentDMO
                                     from c in _examcontext.Exm_Stu_MP_Promo_SubjectwiseDMO
                                     where (a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMST_Id == a.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && data.ASMS_Id == data.ASMS_Id &&  c.AMST_Id == a.AMST_Id && c.ISMS_Id==data.ISMS_Id)
                                     select new Baldwin_Subj_G_F_ReportDTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                         AMST_AdmNo = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                         AMAY_RollNo = b.AMAY_RollNo,
                                         AMST_DOB = a.AMST_DOB,
                                         AMST_Photoname = a.AMST_Photoname
                                     }).Distinct().OrderBy(t => t.AMAY_RollNo).ToList();
                data.prom_students = prom_students.ToArray();

                var prom_subjmarks = _examcontext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ISMS_Id == data.ISMS_Id).Distinct().ToList();
                data.prom_subjmarks = prom_subjmarks.ToArray();

                var prom_subj_groupmarks = (from a in _examcontext.Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO
                                            from b in prom_subjmarks
                                            where (a.ESTMPPS_Id == b.ESTMPPS_Id)
                                            select a).Distinct().ToList();
                data.prom_subj_groupmarks = prom_subj_groupmarks.ToArray();


                //var promo_subjects = (from a in _examcontext.Exm_Yearly_CategoryDMO
                //                      from b in _examcontext.Exm_M_PromotionDMO
                //                      from c in _examcontext.Exm_M_Promotion_SubjectsDMO
                //                      where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.EMCA_Id == data.EMCA_Id && a.EYC_ActiveFlg == true && b.MI_Id == a.MI_Id && b.EMP_ActiveFlag == true && b.EYC_Id == a.EYC_Id && c.EMP_Id == b.EMP_Id && c.EMPS_ActiveFlag == true && c.ISMS_Id==data.ISMS_Id)
                //                      select c).Distinct().ToList();               

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return data;
        }
    }
}
