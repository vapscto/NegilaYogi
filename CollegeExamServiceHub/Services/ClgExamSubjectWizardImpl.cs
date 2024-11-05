using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.College.Exam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgExamSubjectWizardImpl : Interfaces.ClgExamSubjectWizardInterface
    {
        private static ConcurrentDictionary<string, ClgSubjectWizardDTO> _login =
        new ConcurrentDictionary<string, ClgSubjectWizardDTO>();

        private ClgExamContext _examcontext;
        public ClgExamSubjectWizardImpl(ClgExamContext masterexamContext)
        {
            _examcontext = masterexamContext;
        }
        public ClgSubjectWizardDTO Getdetails(ClgSubjectWizardDTO data)
        {
            try
            {
                data.courseslist = _examcontext.MasterCourseDMO.Where(c => c.MI_Id == data.MI_Id && c.AMCO_ActiveFlag == true).ToList().Distinct().ToArray();

                data.subjectshemalist = _examcontext.AdmCollegeSubjectSchemeDMO.Where(c => c.MI_Id == data.MI_Id && c.ACST_ActiveFlg == true).ToList().Distinct().ToArray();

                data.subjectgrplist = _examcontext.col_Exm_Master_GroupDMO.Where(c => c.MI_Id == data.MI_Id && c.EMG_ActiveFlag == true).ToList().Distinct().ToArray();

                data.branchlist = _examcontext.ClgMasterBranchDMO.Where(c => c.MI_Id == data.MI_Id && c.AMB_ActiveFlag == true).ToList().Distinct().ToArray();

                data.schmetypelist = _examcontext.AdmCollegeSchemeTypeDMO.Where(c => c.MI_Id == data.MI_Id && c.ACST_ActiveFlg == true).ToList().Distinct().ToArray();

                data.semisters = _examcontext.CLG_Adm_Master_SemesterDMO.Where(c => c.MI_Id == data.MI_Id && c.AMSE_ActiveFlg == true).ToList().Distinct().ToArray();

                data.gradelist = _examcontext.col_Exm_Master_GradeDMO.Where(g => g.MI_Id == data.MI_Id && g.EMGR_ActiveFlag == true).ToList().Distinct().ToArray();

                data.examlist = _examcontext.col_exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).ToList().Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                data.subexamlist = _examcontext.clg_mastersubexam.Where(s => s.MI_Id == data.MI_Id && s.EMSE_ActiveFlag == true).ToList().Distinct().OrderBy(t => t.EMSE_SubExamOrder).ToArray();

                data.subjectlist = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == data.MI_Id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1).ToList().Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                data.subsubjectlist = _examcontext.clg_mastersubsubject.Where(s => s.MI_Id == data.MI_Id && s.EMSS_ActiveFlag == true).ToList().Distinct().OrderBy(t => t.EMSS_Order).ToArray();

                data.Scheme_exams = (from a in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                     from b in _examcontext.Exm_Col_Yearly_SchemeDMO
                                     from c in _examcontext.MasterCourseDMO
                                     from d in _examcontext.ClgMasterBranchDMO
                                     from e in _examcontext.CLG_Adm_Master_SemesterDMO
                                     from f in _examcontext.col_exammasterDMO
                                     from g in _examcontext.AdmCollegeSubjectSchemeDMO
                                     from h in _examcontext.AdmCollegeSchemeTypeDMO
                                         //from i in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                         //from k in _examcontext.Exm_Master_GroupDMO
                                     from l in _examcontext.col_Exm_Master_GradeDMO
                                     where (a.ECYS_Id == b.ECYS_Id && b.AMCO_Id == c.AMCO_Id && b.AMB_Id == d.AMB_Id && b.AMSE_Id == e.AMSE_Id
                                     && a.EME_Id == f.EME_Id && a.ACSS_Id == g.ACSS_Id && a.ACST_Id == h.ACST_Id
                                     //&& b.ECYS_Id == i.ECYS_Id && i.EMG_Id == k.EMG_Id
                                     && a.EMGR_Id == l.EMGR_Id && b.MI_Id == data.MI_Id)
                                     select new ClgSubjectWizardDTO
                                     {
                                         ECYSE_Id = a.ECYSE_Id,
                                         EME_Id = a.EME_Id,
                                         EME_ExamName = f.EME_ExamName,
                                         EME_ExamCode = f.EME_ExamCode,
                                         //subjectgrpname = k.EMG_GroupName,
                                         EMGR_GradeName = l.EMGR_GradeName,
                                         ECYSE_AttendanceFromDate = a.ECYSE_AttendanceFromDate,
                                         ECYSE_AttendanceToDate = a.ECYSE_AttendanceToDate,
                                         ECYSE_SubExamFlg = a.ECYSE_SubExamFlg,
                                         ECYSE_SubSubjectFlg = a.ECYSE_SubSubjectFlg,
                                         ECYSE_ActiveFlg = a.ECYSE_ActiveFlg,
                                         AMCO_CourseName = c.AMCO_CourseName,
                                         AMB_BranchName = d.AMB_BranchName,
                                         AMSE_SEMName = e.AMSE_SEMName,
                                         schemetype = h.ACST_SchmeType,
                                         subjectscheme = g.ACSS_SchmeName
                                     }).Distinct().ToArray();

                data.subsubjectsubexamlist = (from b in _examcontext.clg_mastersubexam
                                              from a in _examcontext.clg_mastersubsubject
                                              where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                              && a.EMSS_ActiveFlag == true && b.EMSE_ActiveFlag == true)
                                              select new ClgSubjectWizardDTO
                                              {
                                                  EMSS_Id = a.EMSS_Id,
                                                  EMSS_SubSubjectName = a.EMSS_SubSubjectName,
                                                  EMSS_Order = a.EMSS_Order,
                                                  EMSS_SubSubjectCode = a.EMSS_SubSubjectCode,
                                                  EMSE_Id = b.EMSE_Id,
                                                  EMSE_SubExamName = b.EMSE_SubExamName,
                                                  EMSE_SubExamOrder = b.EMSE_SubExamOrder,
                                                  EMSE_SubExamCode = b.EMSE_SubExamCode

                                              }).Distinct().OrderBy(a => a.EMSS_Order).ThenBy(a => a.EMSE_SubExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public ClgSubjectWizardDTO getbranch(ClgSubjectWizardDTO data)
        {
            try
            {
                data.branchlist = (from a in _examcontext.MasterCourseDMO
                                   from b in _examcontext.ClgMasterCourseBranchMap
                                   from d in _examcontext.ClgMasterBranchDMO
                                   where (a.AMCO_Id == b.AMCO_Id && b.AMB_Id == d.AMB_Id && a.AMCO_ActiveFlag == true && b.AMCOBM_ActiveFlg == true && d.AMB_ActiveFlag == true && a.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id)
                                   select d).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgSubjectWizardDTO getsemester(ClgSubjectWizardDTO data)
        {
            try
            {
                data.semisters = (from a in _examcontext.MasterCourseDMO
                                  from b in _examcontext.ClgMasterCourseBranchMap
                                  from d in _examcontext.ClgMasterBranchDMO
                                  from e in _examcontext.AdmCourseBranchSemesterMappingDMO
                                  from f in _examcontext.CLG_Adm_Master_SemesterDMO
                                  where (a.AMCO_Id == b.AMCO_Id && b.AMCOBM_Id == e.AMCOBM_Id && e.AMSE_Id == f.AMSE_Id && b.AMB_Id == d.AMB_Id
                                  && a.AMCO_ActiveFlag == true && b.AMCOBM_ActiveFlg == true && d.AMB_ActiveFlag == true && a.MI_Id == data.MI_Id
                                  && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && e.AMCOBMS_ActiveFlg == true && f.AMSE_ActiveFlg == true)
                                  select f).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgSubjectWizardDTO getsubjectscheme(ClgSubjectWizardDTO data)
        {
            try
            {
                data.subjectshemalist = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                         from b in _examcontext.AdmCollegeSubjectSchemeDMO
                                         where (a.ACSS_Id == b.ACSS_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id
                                         && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ECYS_ActiveFlag == true && b.ACST_ActiveFlg == true)
                                         select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgSubjectWizardDTO getsubjectschemetype(ClgSubjectWizardDTO data)
        {
            try
            {

                data.schmetypelist = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                      from b in _examcontext.AdmCollegeSubjectSchemeDMO
                                      from c in _examcontext.AdmCollegeSchemeTypeDMO
                                      where (a.ACSS_Id == b.ACSS_Id && a.ACST_Id == c.ACST_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id
                                      && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACSS_Id == data.ACSS_Id)
                                      select c).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgSubjectWizardDTO getsubjectgroup(ClgSubjectWizardDTO data)
        {
            try
            {
                data.subjectgrplist = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                       from b in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                       from c in _examcontext.col_Exm_Master_GroupDMO
                                       where (a.ECYS_Id == b.ECYS_Id && b.EMG_Id == c.EMG_Id && a.ECYS_ActiveFlag == true && b.ECYSG_ActiveFlag == true
                                       && c.EMG_ActiveFlag == true && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id
                                       && a.MI_Id == data.MI_Id)
                                       select c).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgSubjectWizardDTO savedetails(ClgSubjectWizardDTO _category)
        {
            Exm_Col_Yearly_Scheme_ExamsDMO objpge = Mapper.Map<Exm_Col_Yearly_Scheme_ExamsDMO>(_category);
            try
            {
                if (objpge.ECYSE_Id > 0)
                {
                    List<Exm_Col_Yearly_Scheme_ExamsDMO> lorg1 = new List<Exm_Col_Yearly_Scheme_ExamsDMO>();
                    lorg1 = _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(t => t.ECYSE_Id == objpge.ECYSE_Id).ToList();

                    List<Exm_Col_Yrly_Sch_Exams_SubwiseDMO> lorg2 = new List<Exm_Col_Yrly_Sch_Exams_SubwiseDMO>();
                    lorg2 = _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Where(t => t.ECYSE_Id == objpge.ECYSE_Id).ToList();

                    List<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO> lorg3 = new List<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO>();
                    List<long> ids = new List<long>();

                    foreach (var c in lorg2)
                    {
                        ids.Add(c.ECYSES_Id);
                    }

                    lorg3 = _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Where(t => ids.Contains(t.ECYSES_Id)).ToList();
                    if (lorg3.Any())
                    {
                        for (int ii = 0; lorg3.Count > ii; ii++)
                        {
                            _examcontext.Remove(lorg3.ElementAt(ii));
                        }
                    }

                    if (lorg2.Any())
                    {
                        for (int i = 0; lorg2.Count > i; i++)
                        {
                            _examcontext.Remove(lorg2.ElementAt(i));
                        }
                    }

                    if (lorg1.Any())
                    {
                        for (int i = 0; lorg1.Count > i; i++)
                        {
                            _examcontext.Remove(lorg1.ElementAt(i));
                        }
                    }
                }

                for (int i = 0; i < _category.exams_list.Length; i++)
                {
                    var rst = (from t in _examcontext.Exm_Col_Yearly_SchemeDMO
                               from g in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                               where (t.AMCO_Id == _category.AMCO_Id && t.AMB_Id == _category.AMB_Id && t.AMSE_Id == _category.AMSE_Id
                               && t.ACSS_Id == _category.ACSS_Id && t.ACST_Id == _category.ACST_Id && t.MI_Id == _category.MI_Id && t.ECYS_ActiveFlag == true
                               && g.ECYS_Id == t.ECYS_Id)
                               select new ClgSubjectWizardDTO { ECYS_Id = t.ECYS_Id });

                    Exm_Col_Yearly_Scheme_ExamsDMO objpge1 = new Exm_Col_Yearly_Scheme_ExamsDMO();
                    objpge1.ECYS_Id = rst.Select(g => g.ECYS_Id).FirstOrDefault();
                    objpge1.AMCO_Id = _category.AMCO_Id;
                    objpge1.AMB_Id = _category.AMB_Id;
                    objpge1.AMSE_Id = _category.AMSE_Id;
                    objpge1.ACSS_Id = _category.ACSS_Id;
                    objpge1.ACST_Id = _category.ACST_Id;
                    objpge1.EME_Id = _category.exams_list[i].EME_Id;
                    objpge1.EMGR_Id = _category.EMGR_Id;
                    objpge1.ECYSE_AttendanceFromDate = _category.ECYSE_AttendanceFromDate;
                    objpge1.ECYSE_AttendanceToDate = _category.ECYSE_AttendanceToDate;
                    objpge1.ECYSE_SubExamFlg = _category.ECYSE_SubExamFlg;
                    objpge1.ECYSE_SubSubjectFlg = _category.ECYSE_SubSubjectFlg;
                    objpge1.ECYSE_ActiveFlg = true;
                    objpge1.CreatedDate = DateTime.Now;
                    objpge1.UpdatedDate = DateTime.Now;
                    _examcontext.Add(objpge1);

                    for (int j = 0; j < _category.exm_subjects_list.Length; j++)
                    {
                        Exm_Col_Yrly_Sch_Exams_SubwiseDMO obj_subs = Mapper.Map<Exm_Col_Yrly_Sch_Exams_SubwiseDMO>(_category.exm_subjects_list[j]);
                        obj_subs.ECYSE_Id = objpge1.ECYSE_Id;
                        obj_subs.ECYSES_ActiveFlg = true;
                        obj_subs.CreatedDate = DateTime.Now;
                        obj_subs.UpdatedDate = DateTime.Now;
                        _examcontext.Add(obj_subs);

                        if (obj_subs.ECYSES_SubExamFlg == true && obj_subs.ECYSES_SubSubjectFlg == false)
                        {
                            for (int x = 0; x < _category.exm_subject_subexams_list.Length; x++)
                            {
                                if (obj_subs.ISMS_Id == _category.exm_subject_subexams_list[x].ISMS_Id)
                                {
                                    for (int a = 0; a < _category.exm_subject_subexams_list[x].sub_exam_list.Length; a++)
                                    {
                                        Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO obj_sub_subexms = new Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO();

                                        obj_sub_subexms.EMSE_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMSE_Id;
                                        obj_sub_subexms.EMGR_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMGR_Id;
                                        obj_sub_subexms.ECYSESSS_ExemptedFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_ExemptedFlg;
                                        obj_sub_subexms.ECYSESSS_ExemptedPer = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_ExemptedPer;
                                        obj_sub_subexms.ECYSESSS_MaxMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_MaxMarks;
                                        obj_sub_subexms.ECYSESSS_MinMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_MinMarks;
                                        obj_sub_subexms.ECYSESSS_SubSubjectOrder = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_SubSubjectOrder;
                                        obj_sub_subexms.EMSS_Id = 0;
                                        obj_sub_subexms.ECYSES_Id = obj_subs.ECYSES_Id;
                                        obj_sub_subexms.ECYSESSS_ActiveFlg = true;
                                        obj_sub_subexms.CreatedDate = DateTime.Now;
                                        obj_sub_subexms.UpdatedDate = DateTime.Now;

                                        obj_sub_subexms.ECYSESSS_ProgressCardFlag = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_ProgressCardFlag;

                                        obj_sub_subexms.ECYSESSS_SubjectDisplayName = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_SubjectDisplayName;

                                        obj_sub_subexms.ECYSESSS_SubjectDisplayCode = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_SubjectDisplayCode;

                                        _examcontext.Add(obj_sub_subexms);
                                    }
                                }
                            }
                        }

                        else if (obj_subs.ECYSES_SubExamFlg == false && obj_subs.ECYSES_SubSubjectFlg == true)
                        {
                            for (int y = 0; y < _category.exm_subject_subsubjects_list.Length; y++)
                            {
                                if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_list[y].ISMS_Id)
                                {
                                    for (int b = 0; b < _category.exm_subject_subsubjects_list[y].sub_subjs_list.Length; b++)
                                    {
                                        Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO obj_sub_subsubjs = Mapper.Map<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO>(_category.exm_subject_subsubjects_list[y].sub_subjs_list[b]);
                                        obj_sub_subsubjs.ECYSES_Id = obj_subs.ECYSES_Id;
                                        obj_sub_subsubjs.ECYSESSS_ActiveFlg = true;
                                        obj_sub_subsubjs.CreatedDate = DateTime.Now;
                                        obj_sub_subsubjs.UpdatedDate = DateTime.Now;

                                        obj_sub_subsubjs.ECYSESSS_ProgressCardFlag = _category.exm_subject_subsubjects_list[y].sub_subjs_list[b].ECYSESSS_ProgressCardFlag;

                                        obj_sub_subsubjs.ECYSESSS_SubjectDisplayName = _category.exm_subject_subsubjects_list[y].sub_subjs_list[b].ECYSESSS_SubjectDisplayName;

                                        obj_sub_subsubjs.ECYSESSS_SubjectDisplayCode = _category.exm_subject_subsubjects_list[y].sub_subjs_list[b].ECYSESSS_SubjectDisplayCode;

                                        _examcontext.Add(obj_sub_subsubjs);
                                    }
                                }
                            }
                        }

                        else if (obj_subs.ECYSES_SubExamFlg == true && obj_subs.ECYSES_SubSubjectFlg == true)
                        {
                            for (int y = 0; y < _category.exm_subject_subsubjects_subexam.Length; y++)
                            {
                                if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_subexam[y].ISMS_Id)
                                {
                                    for (int b = 0; b < _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list.Length; b++)
                                    {
                                        Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO obj_sub_subsubjs = Mapper.Map<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO>(_category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b]);
                                        obj_sub_subsubjs.ECYSES_Id = obj_subs.ECYSES_Id;
                                        obj_sub_subsubjs.ECYSESSS_ActiveFlg = true;

                                        obj_sub_subsubjs.ECYSESSS_ProgressCardFlag = _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b].ECYSESSS_ProgressCardFlag;
                                        obj_sub_subsubjs.ECYSESSS_SubjectDisplayName = _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b].ECYSESSS_SubjectDisplayName;
                                        obj_sub_subsubjs.ECYSESSS_SubjectDisplayCode = _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b].ECYSESSS_SubjectDisplayCode;

                                        obj_sub_subsubjs.CreatedDate = DateTime.Now;
                                        obj_sub_subsubjs.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_sub_subsubjs);
                                    }
                                }
                            }
                        }
                    }
                }

                var ik = _examcontext.SaveChanges();

                if (ik > 0)
                {
                    _category.returnval = true;
                }
                else
                {
                    _category.returnval = false;
                }

                _category.Scheme_exams = (from a in _examcontext.col_exammasterDMO
                                          from b in _examcontext.Exm_Master_GradeDMO
                                          from c in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                          from d in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                          where (a.MI_Id == _category.MI_Id && a.EME_Id == c.EME_Id && c.ECYSE_Id == d.ECYSE_Id && b.EMGR_Id == c.EMGR_Id)
                                          select new ClgSubjectWizardDTO
                                          {
                                              ECYSE_Id = d.ECYSE_Id,
                                              EME_Id = a.EME_Id,
                                              EME_ExamName = a.EME_ExamName,
                                              EME_ExamCode = a.EME_ExamCode,
                                              EMGR_GradeName = b.EMGR_GradeName,
                                              ECYSE_AttendanceFromDate = c.ECYSE_AttendanceFromDate,
                                              ECYSE_AttendanceToDate = c.ECYSE_AttendanceToDate,
                                              ECYSE_SubExamFlg = c.ECYSE_SubExamFlg,
                                              ECYSE_SubSubjectFlg = c.ECYSE_SubSubjectFlg,
                                              ECYSE_ActiveFlg = c.ECYSE_ActiveFlg,
                                          }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }
        public ClgSubjectWizardDTO delete_records_of_cat_exm(long id)
        {
            ClgSubjectWizardDTO pagert = new ClgSubjectWizardDTO();

            try
            {
                List<Exm_Col_Yearly_Scheme_ExamsDMO> lorg1 = new List<Exm_Col_Yearly_Scheme_ExamsDMO>();
                lorg1 = _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(t => t.ECYSE_Id == id).ToList();

                List<Exm_Col_Yrly_Sch_Exams_SubwiseDMO> lorg2 = new List<Exm_Col_Yrly_Sch_Exams_SubwiseDMO>();
                lorg2 = _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Where(t => t.ECYSE_Id == id).ToList();

                List<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO> lorg3 = new List<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO>();

                List<long> ids = new List<long>();

                foreach (var c in lorg2)
                {
                    ids.Add(c.ECYSES_Id);
                }

                lorg3 = _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Where(t => ids.Contains(t.ECYSES_Id)).ToList();
                if (lorg3.Any())
                {
                    for (int ii = 0; lorg3.Count > ii; ii++)
                    {
                        _examcontext.Remove(lorg3.ElementAt(ii));
                    }
                }

                if (lorg2.Any())
                {
                    for (int i = 0; lorg2.Count > i; i++)
                    {
                        _examcontext.Remove(lorg2.ElementAt(i));
                    }
                }

                if (lorg1.Any())
                {
                    for (int i = 0; lorg1.Count > i; i++)
                    {
                        _examcontext.Remove(lorg1.ElementAt(i));
                    }
                }

                if (lorg1.Count > 0 || lorg2.Count > 0 || lorg3.Count > 0)
                {
                    var contactExists1 = _examcontext.SaveChanges();
                    if (contactExists1 > 0)
                    {
                        pagert.returnval = true;
                    }
                    else
                    {
                        pagert.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }
        public ClgSubjectWizardDTO getalldetailsviewrecords(int id)
        {
            ClgSubjectWizardDTO page = new ClgSubjectWizardDTO();
            try
            {
                page.view_exam_subjects = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                           from c in _examcontext.col_exammasterDMO
                                           from e in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                           from f in _examcontext.Exm_Master_GradeDMO
                                           from g in _examcontext.IVRM_School_Master_SubjectsDMO
                                           where (a.ECYSE_Id == e.ECYSE_Id && a.ISMS_Id == g.ISMS_Id && a.EMGR_Id == f.EMGR_Id && a.ECYSE_Id == id && e.EME_Id == c.EME_Id)
                                           select new ClgSubjectWizardDTO
                                           {
                                               ECYSES_Id = a.ECYSES_Id,
                                               ECYSE_Id = a.ECYSE_Id,
                                               ISMS_Id = a.ISMS_Id,
                                               EME_ExamName = c.EME_ExamName,
                                               EMGR_GradeName = f.EMGR_GradeName,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               ECYSES_MaxMarks = a.ECYSES_MaxMarks,
                                               ECYSES_MinMarks = a.ECYSES_MinMarks,
                                               ECYSES_MarksEntryMax = a.ECYSES_MarksEntryMax,
                                               ECYSES_SubExamFlg = a.ECYSES_SubExamFlg,
                                               ECYSES_SubSubjectFlg = a.ECYSES_SubSubjectFlg,
                                               ECYSES_MarksGradeEntryFlg = a.ECYSES_MarksGradeEntryFlg,
                                               ECYSES_MarksDisplayFlg = a.ECYSES_MarksDisplayFlg,
                                               ECYSES_GradeDisplayFlg = a.ECYSES_GradeDisplayFlg,
                                               ECYSES_AplResultFlg = a.ECYSES_AplResultFlg,
                                               ECYSES_SubjectOrder = a.ECYSES_SubjectOrder,
                                               ECYSES_ActiveFlg = a.ECYSES_ActiveFlg
                                           }).Distinct().OrderBy(x => x.ECYSES_SubjectOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public ClgSubjectWizardDTO getalldetailsviewrecords_subexms(int id)
        {
            ClgSubjectWizardDTO page = new ClgSubjectWizardDTO();
            try
            {
                page.view_exam_subjects_subexams = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                    from b in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                                    from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                                    from d in _examcontext.Exm_Master_GradeDMO
                                                    from e in _examcontext.clg_mastersubexam
                                                    where (a.ECYSES_Id == b.ECYSES_Id && b.ECYSES_Id == id && b.ISMS_Id == c.ISMS_Id && c.MI_Id == d.MI_Id
                                                    && a.EMGR_Id == d.EMGR_Id && c.MI_Id == e.MI_Id && e.EMSE_Id == a.EMSE_Id && a.ECYSES_Id == id)
                                                    select new ClgSubjectWizardDTO
                                                    {
                                                        ECYSESSS_Id = a.ECYSESSS_Id,
                                                        ECYSES_Id = a.ECYSES_Id,
                                                        ISMS_Id = b.ISMS_Id,
                                                        EMSE_Id = a.EMSE_Id,
                                                        EMGR_GradeName = d.EMGR_GradeName,
                                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                                        ISMS_SubjectCode = c.ISMS_SubjectCode,
                                                        EMSE_SubExamName = e.EMSE_SubExamName,
                                                        EMSE_SubExamCode = e.EMSE_SubExamCode,
                                                        ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                        ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                        ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                        ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                        ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                        ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                        ECYSES_SubExamFlg = b.ECYSES_SubExamFlg,
                                                        ECYSES_SubSubjectFlg = b.ECYSES_SubSubjectFlg

                                                    }).Distinct().OrderBy(x => x.ECYSESSS_SubSubjectOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public ClgSubjectWizardDTO getalldetailsviewrecords_subsubjs(int id)
        {
            ClgSubjectWizardDTO page = new ClgSubjectWizardDTO();
            try
            {
                page.view_exam_subjects_subsubjects = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                       from b in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                                       from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                                       from d in _examcontext.Exm_Master_GradeDMO
                                                       from e in _examcontext.clg_mastersubsubject
                                                       where (a.ECYSES_Id == b.ECYSES_Id && b.ECYSES_Id == id && b.ISMS_Id == c.ISMS_Id && c.MI_Id == d.MI_Id
                                                       && a.EMGR_Id == d.EMGR_Id && c.MI_Id == e.MI_Id && e.EMSS_Id == a.EMSS_Id && a.ECYSES_Id == id)
                                                       select new ClgSubjectWizardDTO
                                                       {
                                                           ECYSESSS_Id = a.ECYSESSS_Id,
                                                           ECYSES_Id = a.ECYSES_Id,
                                                           ISMS_Id = b.ISMS_Id,
                                                           EMSS_Id = a.EMSS_Id,
                                                           EMGR_GradeName = d.EMGR_GradeName,
                                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                                           ISMS_SubjectCode = c.ISMS_SubjectCode,
                                                           EMSS_SubSubjectName = e.EMSS_SubSubjectName,
                                                           EMSS_SubSubjectCode = e.EMSS_SubSubjectCode,
                                                           ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                           ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                           ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                           ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                           ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                           ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                           ECYSES_SubSubjectFlg = b.ECYSES_SubSubjectFlg,
                                                           ECYSES_SubExamFlg = b.ECYSES_SubExamFlg
                                                       }).Distinct().OrderBy(x => x.ECYSESSS_SubSubjectOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public ClgSubjectWizardDTO deactivate_sub(ClgSubjectWizardDTO data)
        {
            data.already_cnt = false;
            Exm_Col_Yrly_Sch_Exams_SubwiseDMO pge = Mapper.Map<Exm_Col_Yrly_Sch_Exams_SubwiseDMO>(data);
            if (pge.ECYSES_Id > 0)
            {
                var result = _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Single(t => t.ECYSES_Id == pge.ECYSES_Id);
                if (result.ECYSES_ActiveFlg == true)
                {
                    result.ECYSES_ActiveFlg = false;
                }
                else
                {
                    result.ECYSES_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _examcontext.Update(result);
                var flag = _examcontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }


            return data;
        }
        public ClgSubjectWizardDTO deactive_sub_exm(ClgSubjectWizardDTO data)
        {
            Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO pge = Mapper.Map<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO>(data);
            if (pge.ECYSESSS_Id > 0)
            {
                var result = _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Single(t => t.ECYSESSS_Id == pge.ECYSESSS_Id);
                if (result.ECYSESSS_ActiveFlg == true)
                {
                    result.ECYSESSS_ActiveFlg = false;
                }
                else
                {
                    result.ECYSESSS_ActiveFlg = true;
                }
                _examcontext.Update(result);
                var flag = _examcontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            return data;
        }
        public ClgSubjectWizardDTO deactive_sub_subj(ClgSubjectWizardDTO data)
        {
            Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO pge = Mapper.Map<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO>(data);
            if (pge.ECYSESSS_Id > 0)
            {
                var result = _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Single(t => t.ECYSESSS_Id == pge.ECYSESSS_Id);
                if (result.ECYSESSS_ActiveFlg == true)
                {
                    result.ECYSESSS_ActiveFlg = false;
                }
                else
                {
                    result.ECYSESSS_ActiveFlg = true;
                }
                _examcontext.Update(result);
                var flag = _examcontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            return data;
        }
        public ClgSubjectWizardDTO editdetails(int id)
        {
            ClgSubjectWizardDTO page = new ClgSubjectWizardDTO();
            try
            {
                page.edit_cat_exm = _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(a => a.ECYSE_Id == id).Distinct().ToArray();
                page.edit_cat_exm_subs = _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Where(ce => ce.ECYSE_Id == id).Distinct().ToArray();
                var idNew = _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(a => a.ECYSE_Id == id).Distinct().Select(g => g.ECYS_Id).FirstOrDefault();

                //page.EMG_ID = (from i in _examcontext.Exm_Col_Yearly_SchemeDMO
                //               from j in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                //               from k in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                //               where (i.ECYS_Id == j.ECYS_Id && i.ECYS_Id == k.ECYS_Id && i.ECYS_Id == idNew)
                //               select new ClgSubjectWizardDTO
                //               {
                //                   EMG_ID = j.EMG_Id
                //               }).Distinct().Select(e => e.EMG_ID).FirstOrDefault();

                List<int> subs = new List<int>();
                foreach (Exm_Col_Yrly_Sch_Exams_SubwiseDMO x in page.edit_cat_exm_subs)
                {
                    subs.Add(Convert.ToInt32(x.ECYSES_Id));
                }

                page.edit_cat_exm_subs_sub_exms = _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Where(e => subs.Contains(Convert.ToInt32(e.ECYSES_Id))).Distinct().ToArray();
                page.edit_cat_exm_subs_sub_subjs = _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Where(e => subs.Contains(Convert.ToInt32(e.ECYSES_Id))).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return page;
        }
        public ClgSubjectWizardDTO deactivate(ClgSubjectWizardDTO data)
        {
            data.already_cnt = false;
            if (data.ECYSE_Id > 0)
            {
                var result = _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO.Single(t => t.ECYSE_Id == data.ECYSE_Id);
                if (result.ECYSE_ActiveFlg == true)
                {
                    result.ECYSE_ActiveFlg = false;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                }
                else
                {
                    result.ECYSE_ActiveFlg = true;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                }
                var flag = _examcontext.SaveChanges();
                if (flag == 1)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            return data;
        }
        public ClgSubjectWizardDTO getalldetailsviewrecords_subsubjssunexam(ClgSubjectWizardDTO data)
        {
            try
            {
                data.view_exam_subjects_subsubjects = (from a in _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO
                                                       from b in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                                       from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                                       from d in _examcontext.Exm_Master_GradeDMO
                                                       from e in _examcontext.clg_mastersubsubject
                                                       from f in _examcontext.clg_mastersubexam
                                                       where (a.ECYSES_Id == b.ECYSES_Id && b.ISMS_Id == c.ISMS_Id
                                                       && c.MI_Id == d.MI_Id && a.EMGR_Id == d.EMGR_Id && c.MI_Id == e.MI_Id
                                                       && e.EMSS_Id == a.EMSS_Id && f.EMSE_Id == a.EMSE_Id && b.ECYSES_Id == data.ECYSES_Id)
                                                       select new ClgSubjectWizardDTO
                                                       {
                                                           ECYSESSS_Id = a.ECYSESSS_Id,
                                                           ECYSES_Id = a.ECYSES_Id,
                                                           ISMS_Id = b.ISMS_Id,
                                                           EMSS_Id = a.EMSS_Id,
                                                           EMGR_GradeName = d.EMGR_GradeName,
                                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                                           ISMS_SubjectCode = c.ISMS_SubjectCode,
                                                           EMSS_SubSubjectName = e.EMSS_SubSubjectName,
                                                           EMSS_SubSubjectCode = e.EMSS_SubSubjectCode,

                                                           ECYSESSS_ProgressCardFlag = a.ECYSESSS_ProgressCardFlag,
                                                           ECYSESSS_SubjectDisplayName = a.ECYSESSS_SubjectDisplayName,
                                                           ECYSESSS_SubjectDisplayCode = a.ECYSESSS_SubjectDisplayCode,

                                                           EMSE_SubExamName = f.EMSE_SubExamName,
                                                           EMSE_SubExamCode = f.EMSE_SubExamCode,
                                                           ECYSESSS_MaxMarks = a.ECYSESSS_MaxMarks,
                                                           ECYSESSS_MinMarks = a.ECYSESSS_MinMarks,
                                                           ECYSESSS_ExemptedFlg = a.ECYSESSS_ExemptedFlg,
                                                           ECYSESSS_ExemptedPer = a.ECYSESSS_ExemptedPer,
                                                           ECYSESSS_SubSubjectOrder = a.ECYSESSS_SubSubjectOrder,
                                                           ECYSESSS_ActiveFlg = a.ECYSESSS_ActiveFlg,
                                                           ECYSES_SubExamFlg = b.ECYSES_SubExamFlg,
                                                           ECYSES_SubSubjectFlg = b.ECYSES_SubSubjectFlg
                                                       }).Distinct().OrderBy(x => x.ECYSESSS_SubSubjectOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgSubjectWizardDTO deactive_sub_subj_subexam(ClgSubjectWizardDTO data)
        {
            try
            {
                if (data.ECYSESSS_Id > 0)
                {
                    var result = _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Single(t => t.ECYSESSS_Id == data.ECYSESSS_Id);
                    if (result.ECYSESSS_ActiveFlg == true)
                    {
                        result.ECYSESSS_ActiveFlg = false;
                    }
                    else
                    {
                        result.ECYSESSS_ActiveFlg = true;
                    }
                    // result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                    var flag = _examcontext.SaveChanges();
                    if (flag == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgSubjectWizardDTO get_subjects(ClgSubjectWizardDTO data)
        {
            try
            {
                data.subjectgroups = (from s in _examcontext.Exm_Col_Yearly_SchemeDMO
                                      from k in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                      from l in _examcontext.Exm_Col_Yearly_Scheme_Group_SubjectsDMO
                                      from j in _examcontext.IVRM_Master_SubjectsDMO
                                      where (s.ECYS_Id == k.ECYS_Id && k.ECYSG_Id == l.ECYSG_Id && l.ISMS_Id == j.ISMS_Id
                                      && s.AMCO_Id == data.AMCO_Id && s.AMB_Id == data.AMB_Id && s.AMSE_Id == data.AMSE_Id && s.ACSS_Id == data.ACSS_Id
                                      && s.ACST_Id == data.ACST_Id && s.MI_Id == data.MI_Id && s.ECYS_ActiveFlag == true && k.ECYSG_ActiveFlag == true
                                      && l.ECYSGS_ActiveFlag == true)
                                      select j).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

                data.examlist = _examcontext.col_exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToList().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgSubjectWizardDTO SetOrder_SubSubject(ClgSubjectWizardDTO data)
        {
            try
            {
                data.returnval = true;
                if (data.Set_SubSubject_Order_DTO != null && data.Set_SubSubject_Order_DTO.Length > 0)
                {
                    int? order = 0;
                    foreach (var c in data.Set_SubSubject_Order_DTO)
                    {
                        order += 1;
                        var result = _examcontext.Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO.Single(a => a.ECYSESSS_Id == c.ECYSESSS_Id);
                        result.ECYSESSS_SubSubjectOrder = order;
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                    }

                    var i = _examcontext.SaveChanges();

                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgSubjectWizardDTO savedetails_backup(ClgSubjectWizardDTO _category)
        {
            Exm_Col_Yearly_Scheme_ExamsDMO objpge = Mapper.Map<Exm_Col_Yearly_Scheme_ExamsDMO>(_category);
            try
            {
                if (objpge.ECYSE_Id > 0)
                {
                    ClgSubjectWizardDTO deleted_result = delete_records_of_cat_exm(_category.ECYSE_Id);
                    if (deleted_result.returnval == true)
                    {
                        _category.ECYSE_Id = 0;
                        for (int i = 0; i < _category.exams_list.Length; i++)
                        {
                            var rst1 = (from t in _examcontext.Exm_Col_Yearly_SchemeDMO
                                        from g in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                        where (t.AMCO_Id == _category.AMCO_Id && t.AMB_Id == _category.AMB_Id && t.AMSE_Id == _category.AMSE_Id && t.ACSS_Id == _category.ACSS_Id && t.ACST_Id == _category.ACST_Id && t.MI_Id == _category.MI_Id && t.ECYS_ActiveFlag == true && g.ECYS_Id == t.ECYS_Id && g.EMG_Id == _category.EMG_ID)
                                        select new { }).Count();

                            var rst = (from t in _examcontext.Exm_Col_Yearly_SchemeDMO
                                       from g in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                       where (t.AMCO_Id == _category.AMCO_Id && t.AMB_Id == _category.AMB_Id && t.AMSE_Id == _category.AMSE_Id && t.ACSS_Id == _category.ACSS_Id && t.ACST_Id == _category.ACST_Id && t.MI_Id == _category.MI_Id && t.ECYS_ActiveFlag == true && g.ECYS_Id == t.ECYS_Id && g.EMG_Id == _category.EMG_ID)
                                       select new ClgSubjectWizardDTO { ECYS_Id = t.ECYS_Id });

                            if (rst1 > 0)
                            {
                                Exm_Col_Yearly_Scheme_ExamsDMO objpge1 = Mapper.Map<Exm_Col_Yearly_Scheme_ExamsDMO>(_category);
                                var result = _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(t => t.AMCO_Id == objpge1.AMCO_Id && t.AMB_Id == objpge1.AMB_Id && t.AMSE_Id == objpge1.AMSE_Id && t.ACSS_Id == objpge1.ACSS_Id && t.ACST_Id == objpge1.ACST_Id && t.EME_Id == _category.exams_list[i].EME_Id && t.ECYSE_ActiveFlg == true &&
                                t.EMGR_Id == _category.EMGR_Id && t.ECYS_Id == rst.Select(g => g.ECYS_Id).FirstOrDefault()).ToList();

                                if (result.Count() == 0)
                                {
                                    objpge1.ECYS_Id = rst.Select(g => g.ECYS_Id).FirstOrDefault();
                                    objpge1.EME_Id = _category.exams_list[i].EME_Id;
                                    objpge1.ECYSE_ActiveFlg = true;
                                    objpge1.CreatedDate = DateTime.Now;
                                    objpge1.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(objpge1);

                                    for (int j = 0; j < _category.exm_subjects_list.Length; j++)
                                    {
                                        Exm_Col_Yrly_Sch_Exams_SubwiseDMO obj_subs = Mapper.Map<Exm_Col_Yrly_Sch_Exams_SubwiseDMO>(_category.exm_subjects_list[j]);
                                        obj_subs.ECYSE_Id = objpge1.ECYSE_Id;
                                        obj_subs.ECYSES_ActiveFlg = true;
                                        obj_subs.CreatedDate = DateTime.Now;
                                        obj_subs.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_subs);

                                        if (obj_subs.ECYSES_SubExamFlg == true && obj_subs.ECYSES_SubSubjectFlg == false)
                                        {
                                            for (int x = 0; x < _category.exm_subject_subexams_list.Length; x++)
                                            {
                                                if (obj_subs.ISMS_Id == _category.exm_subject_subexams_list[x].ISMS_Id)
                                                {
                                                    for (int a = 0; a < _category.exm_subject_subexams_list[x].sub_exam_list.Length; a++)
                                                    {
                                                        Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO obj_sub_subexms = new Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO();
                                                        obj_sub_subexms.EMSE_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMSE_Id;
                                                        obj_sub_subexms.EMGR_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMGR_Id;
                                                        obj_sub_subexms.ECYSESSS_ExemptedFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_ExemptedFlg;
                                                        obj_sub_subexms.ECYSESSS_ExemptedPer = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_ExemptedPer;
                                                        obj_sub_subexms.ECYSESSS_MaxMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_MaxMarks;
                                                        obj_sub_subexms.ECYSESSS_MinMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_MinMarks;
                                                        obj_sub_subexms.ECYSESSS_SubSubjectOrder = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_SubSubjectOrder;
                                                        obj_sub_subexms.EMSS_Id = 0;
                                                        obj_sub_subexms.ECYSES_Id = obj_subs.ECYSES_Id;
                                                        obj_sub_subexms.ECYSESSS_ActiveFlg = true;
                                                        obj_sub_subexms.CreatedDate = DateTime.Now;
                                                        obj_sub_subexms.UpdatedDate = DateTime.Now;

                                                        obj_sub_subexms.ECYSESSS_ProgressCardFlag = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_ProgressCardFlag;

                                                        obj_sub_subexms.ECYSESSS_SubjectDisplayName = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_SubjectDisplayName;

                                                        obj_sub_subexms.ECYSESSS_SubjectDisplayCode = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_SubjectDisplayCode;

                                                        _examcontext.Add(obj_sub_subexms);
                                                    }
                                                }
                                            }
                                        }

                                        else if (obj_subs.ECYSES_SubExamFlg == false && obj_subs.ECYSES_SubSubjectFlg == true)
                                        {
                                            for (int y = 0; y < _category.exm_subject_subsubjects_list.Length; y++)
                                            {
                                                if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_list[y].ISMS_Id)
                                                {
                                                    for (int b = 0; b < _category.exm_subject_subsubjects_list[y].sub_subjs_list.Length; b++)
                                                    {
                                                        Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO obj_sub_subsubjs = Mapper.Map<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO>(_category.exm_subject_subsubjects_list[y].sub_subjs_list[b]);
                                                        obj_sub_subsubjs.ECYSES_Id = obj_subs.ECYSES_Id;
                                                        obj_sub_subsubjs.ECYSESSS_ActiveFlg = true;
                                                        obj_sub_subsubjs.CreatedDate = DateTime.Now;
                                                        obj_sub_subsubjs.UpdatedDate = DateTime.Now;

                                                        obj_sub_subsubjs.ECYSESSS_ProgressCardFlag = _category.exm_subject_subsubjects_list[y].sub_subjs_list[b].ECYSESSS_ProgressCardFlag;
                                                        obj_sub_subsubjs.ECYSESSS_SubjectDisplayName = _category.exm_subject_subsubjects_list[y].sub_subjs_list[b].ECYSESSS_SubjectDisplayName;
                                                        obj_sub_subsubjs.ECYSESSS_SubjectDisplayCode = _category.exm_subject_subsubjects_list[y].sub_subjs_list[b].ECYSESSS_SubjectDisplayCode;
                                                        _examcontext.Add(obj_sub_subsubjs);
                                                    }
                                                }
                                            }
                                        }

                                        else if (obj_subs.ECYSES_SubExamFlg == true && obj_subs.ECYSES_SubSubjectFlg == true)
                                        {
                                            for (int y = 0; y < _category.exm_subject_subsubjects_subexam.Length; y++)
                                            {
                                                if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_subexam[y].ISMS_Id)
                                                {

                                                    for (int b = 0; b < _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list.Length; b++)
                                                    {

                                                        Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO obj_sub_subsubjs = Mapper.Map<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO>(_category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b]);
                                                        obj_sub_subsubjs.ECYSES_Id = obj_subs.ECYSES_Id;
                                                        obj_sub_subsubjs.ECYSESSS_ActiveFlg = true;
                                                        obj_sub_subsubjs.CreatedDate = DateTime.Now;
                                                        obj_sub_subsubjs.UpdatedDate = DateTime.Now;

                                                        obj_sub_subsubjs.ECYSESSS_ProgressCardFlag = _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b].ECYSESSS_ProgressCardFlag;

                                                        obj_sub_subsubjs.ECYSESSS_SubjectDisplayName = _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b].ECYSESSS_SubjectDisplayName;

                                                        obj_sub_subsubjs.ECYSESSS_SubjectDisplayCode = _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b].ECYSESSS_SubjectDisplayCode;

                                                        _examcontext.Add(obj_sub_subsubjs);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    _category.returnduplicatestatus = "Duplicate";
                                }
                            }
                        }

                        var kk = _examcontext.SaveChanges();
                        if (kk > 0)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                    _category.ECYSE_Id = objpge.ECYSE_Id;
                }
                else
                {
                    _category.ECYSE_Id = 0;

                    for (int i = 0; i < _category.exams_list.Length; i++)
                    {
                        var rst1 = (from t in _examcontext.Exm_Col_Yearly_SchemeDMO
                                    from g in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                    where (t.AMCO_Id == _category.AMCO_Id && t.AMB_Id == _category.AMB_Id && t.AMSE_Id == _category.AMSE_Id
                                    && t.ACSS_Id == _category.ACSS_Id && t.ACST_Id == _category.ACST_Id && t.MI_Id == _category.MI_Id && t.ECYS_ActiveFlag == true
                                    && g.ECYS_Id == t.ECYS_Id && g.EMG_Id == _category.EMG_ID)
                                    select new { }).Count();

                        var rst = (from t in _examcontext.Exm_Col_Yearly_SchemeDMO
                                   from g in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                   where (t.AMCO_Id == _category.AMCO_Id && t.AMB_Id == _category.AMB_Id && t.AMSE_Id == _category.AMSE_Id
                                   && t.ACSS_Id == _category.ACSS_Id && t.ACST_Id == _category.ACST_Id && t.MI_Id == _category.MI_Id && t.ECYS_ActiveFlag == true
                                   && g.ECYS_Id == t.ECYS_Id && g.EMG_Id == _category.EMG_ID)
                                   select new ClgSubjectWizardDTO { ECYS_Id = t.ECYS_Id });


                        if (rst1 > 0)
                        {
                            Exm_Col_Yearly_Scheme_ExamsDMO objpge1 = Mapper.Map<Exm_Col_Yearly_Scheme_ExamsDMO>(_category);
                            var result = _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(t => t.AMCO_Id == objpge1.AMCO_Id && t.AMB_Id == objpge1.AMB_Id
                            && t.AMSE_Id == objpge1.AMSE_Id && t.ACSS_Id == objpge1.ACSS_Id && t.ACST_Id == objpge1.ACST_Id
                            && t.EME_Id == _category.exams_list[i].EME_Id && t.ECYSE_ActiveFlg == true && t.EMGR_Id == _category.EMGR_Id
                            && t.ECYS_Id == rst.Select(g => g.ECYS_Id).FirstOrDefault()).ToList();

                            if (result.Count() == 0)
                            {
                                objpge1.ECYS_Id = rst.Select(g => g.ECYS_Id).FirstOrDefault();
                                objpge1.EME_Id = _category.exams_list[i].EME_Id;
                                objpge1.ECYSE_ActiveFlg = true;
                                objpge1.CreatedDate = DateTime.Now;
                                objpge1.UpdatedDate = DateTime.Now;
                                _examcontext.Add(objpge1);

                                for (int j = 0; j < _category.exm_subjects_list.Length; j++)
                                {
                                    Exm_Col_Yrly_Sch_Exams_SubwiseDMO obj_subs = Mapper.Map<Exm_Col_Yrly_Sch_Exams_SubwiseDMO>(_category.exm_subjects_list[j]);
                                    obj_subs.ECYSE_Id = objpge1.ECYSE_Id;
                                    obj_subs.ECYSES_ActiveFlg = true;
                                    obj_subs.CreatedDate = DateTime.Now;
                                    obj_subs.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj_subs);

                                    if (obj_subs.ECYSES_SubExamFlg == true && obj_subs.ECYSES_SubSubjectFlg == false)
                                    {
                                        for (int x = 0; x < _category.exm_subject_subexams_list.Length; x++)
                                        {
                                            if (obj_subs.ISMS_Id == _category.exm_subject_subexams_list[x].ISMS_Id)
                                            {
                                                for (int a = 0; a < _category.exm_subject_subexams_list[x].sub_exam_list.Length; a++)
                                                {
                                                    Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO obj_sub_subexms = new Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO();

                                                    obj_sub_subexms.EMSE_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMSE_Id;
                                                    obj_sub_subexms.EMGR_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMGR_Id;
                                                    obj_sub_subexms.ECYSESSS_ExemptedFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_ExemptedFlg;
                                                    obj_sub_subexms.ECYSESSS_ExemptedPer = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_ExemptedPer;
                                                    obj_sub_subexms.ECYSESSS_MaxMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_MaxMarks;
                                                    obj_sub_subexms.ECYSESSS_MinMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_MinMarks;
                                                    obj_sub_subexms.ECYSESSS_SubSubjectOrder = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_SubSubjectOrder;
                                                    obj_sub_subexms.EMSS_Id = 0;
                                                    obj_sub_subexms.ECYSES_Id = obj_subs.ECYSES_Id;
                                                    obj_sub_subexms.ECYSESSS_ActiveFlg = true;
                                                    obj_sub_subexms.CreatedDate = DateTime.Now;
                                                    obj_sub_subexms.UpdatedDate = DateTime.Now;

                                                    obj_sub_subexms.ECYSESSS_ProgressCardFlag = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_ProgressCardFlag;

                                                    obj_sub_subexms.ECYSESSS_SubjectDisplayName = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_SubjectDisplayName;

                                                    obj_sub_subexms.ECYSESSS_SubjectDisplayCode = _category.exm_subject_subexams_list[x].sub_exam_list[a].ECYSESSS_SubjectDisplayCode;

                                                    _examcontext.Add(obj_sub_subexms);
                                                }
                                            }
                                        }
                                    }

                                    else if (obj_subs.ECYSES_SubExamFlg == false && obj_subs.ECYSES_SubSubjectFlg == true)
                                    {
                                        for (int y = 0; y < _category.exm_subject_subsubjects_list.Length; y++)
                                        {
                                            if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_list[y].ISMS_Id)
                                            {
                                                for (int b = 0; b < _category.exm_subject_subsubjects_list[y].sub_subjs_list.Length; b++)
                                                {
                                                    Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO obj_sub_subsubjs = Mapper.Map<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO>(_category.exm_subject_subsubjects_list[y].sub_subjs_list[b]);
                                                    obj_sub_subsubjs.ECYSES_Id = obj_subs.ECYSES_Id;
                                                    obj_sub_subsubjs.ECYSESSS_ActiveFlg = true;
                                                    obj_sub_subsubjs.CreatedDate = DateTime.Now;
                                                    obj_sub_subsubjs.UpdatedDate = DateTime.Now;

                                                    obj_sub_subsubjs.ECYSESSS_ProgressCardFlag = _category.exm_subject_subsubjects_list[y].sub_subjs_list[b].ECYSESSS_ProgressCardFlag;

                                                    obj_sub_subsubjs.ECYSESSS_SubjectDisplayName = _category.exm_subject_subsubjects_list[y].sub_subjs_list[b].ECYSESSS_SubjectDisplayName;

                                                    obj_sub_subsubjs.ECYSESSS_SubjectDisplayCode = _category.exm_subject_subsubjects_list[y].sub_subjs_list[b].ECYSESSS_SubjectDisplayCode;

                                                    _examcontext.Add(obj_sub_subsubjs);
                                                }
                                            }
                                        }
                                    }

                                    else if (obj_subs.ECYSES_SubExamFlg == true && obj_subs.ECYSES_SubSubjectFlg == true)
                                    {
                                        for (int y = 0; y < _category.exm_subject_subsubjects_subexam.Length; y++)
                                        {
                                            if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_subexam[y].ISMS_Id)
                                            {
                                                for (int b = 0; b < _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list.Length; b++)
                                                {
                                                    Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO obj_sub_subsubjs = Mapper.Map<Exm_Col_Yrly_Sch_Exams_Subwise_SubDMO>(_category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b]);
                                                    obj_sub_subsubjs.ECYSES_Id = obj_subs.ECYSES_Id;
                                                    obj_sub_subsubjs.ECYSESSS_ActiveFlg = true;

                                                    obj_sub_subsubjs.ECYSESSS_ProgressCardFlag = _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b].ECYSESSS_ProgressCardFlag;
                                                    obj_sub_subsubjs.ECYSESSS_SubjectDisplayName = _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b].ECYSESSS_SubjectDisplayName;
                                                    obj_sub_subsubjs.ECYSESSS_SubjectDisplayCode = _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b].ECYSESSS_SubjectDisplayCode;

                                                    obj_sub_subsubjs.CreatedDate = DateTime.Now;
                                                    obj_sub_subsubjs.UpdatedDate = DateTime.Now;
                                                    _examcontext.Add(obj_sub_subsubjs);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                _category.returnduplicatestatus = "Duplicate";
                            }
                        }
                    }

                    var ik = _examcontext.SaveChanges();

                    if (ik > 0)
                    {
                        _category.returnval = true;
                    }
                    else
                    {
                        _category.returnval = false;
                    }
                }
                _category.Scheme_exams = (from a in _examcontext.col_exammasterDMO
                                          from b in _examcontext.Exm_Master_GradeDMO
                                          from c in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                          from d in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                          where (a.MI_Id == _category.MI_Id && a.EME_Id == c.EME_Id && c.ECYSE_Id == d.ECYSE_Id && b.EMGR_Id == c.EMGR_Id)
                                          select new ClgSubjectWizardDTO
                                          {
                                              ECYSE_Id = d.ECYSE_Id,
                                              EME_Id = a.EME_Id,
                                              EME_ExamName = a.EME_ExamName,
                                              EME_ExamCode = a.EME_ExamCode,
                                              EMGR_GradeName = b.EMGR_GradeName,
                                              ECYSE_AttendanceFromDate = c.ECYSE_AttendanceFromDate,
                                              ECYSE_AttendanceToDate = c.ECYSE_AttendanceToDate,
                                              ECYSE_SubExamFlg = c.ECYSE_SubExamFlg,
                                              ECYSE_SubSubjectFlg = c.ECYSE_SubSubjectFlg,
                                              ECYSE_ActiveFlg = c.ECYSE_ActiveFlg,
                                          }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return _category;
        }
    }
}