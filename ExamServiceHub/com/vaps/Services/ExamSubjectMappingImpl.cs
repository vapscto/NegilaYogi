
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
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.Extensions.Logging;
using DomainModel.Model.com.vapstech.Exam;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamMarksprocessconditionImpl : Interfaces.ExamSubjectMappingInterface
    {
        private static ConcurrentDictionary<string, ExamSubjectMappingDTO> _login =
         new ConcurrentDictionary<string, ExamSubjectMappingDTO>();

        private ExamContext _examcontext;
        ILogger<ExamMarksprocessconditionImpl> _acdimpl;
        public ExamMarksprocessconditionImpl(ExamContext masterexamContext, ILogger<ExamMarksprocessconditionImpl> _acdimp)
        {
            _examcontext = masterexamContext;
            _acdimpl = _acdimp;
        }
        public ExamSubjectMappingDTO Getdetails(ExamSubjectMappingDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _examcontext.AcademicYear.Where(y => y.MI_Id == data.MI_Id && y.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = year.Distinct().ToArray();

                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.EYC_ActiveFlg == true)
                                     select new ExamSubjectMappingDTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName,
                                         EYC_Id = b.EYC_Id,

                                     }).Distinct().ToArray();



                List<Exm_Master_GradeDMO> grades = new List<Exm_Master_GradeDMO>();
                grades = _examcontext.Exm_Master_GradeDMO.Where(g => g.MI_Id == data.MI_Id && g.EMGR_ActiveFlag == true).ToList();
                data.gradelist = grades.Distinct().ToArray();

                List<exammasterDMO> exams = new List<exammasterDMO>();
                exams = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).ToList();
                data.examlist = exams.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                List<mastersubexamDMO> subexams = new List<mastersubexamDMO>();
                subexams = _examcontext.mastersubexam.Where(s => s.MI_Id == data.MI_Id && s.EMSE_ActiveFlag == true).ToList();
                data.subexamlist = subexams.Distinct().OrderBy(t => t.EMSE_SubExamOrder).ToArray();

                List<IVRM_School_Master_SubjectsDMO> subjects = new List<IVRM_School_Master_SubjectsDMO>();
                subjects = _examcontext.IVRM_School_Master_SubjectsDMO.Where(c => c.MI_Id == data.MI_Id && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1).ToList();
                data.subjectlist = subjects.Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                List<mastersubsubjectDMO> subsubjects = new List<mastersubsubjectDMO>();
                subsubjects = _examcontext.mastersubsubject.Where(s => s.MI_Id == data.MI_Id && s.EMSS_ActiveFlag == true).ToList();
                data.subsubjectlist = subsubjects.Distinct().OrderBy(t => t.EMSS_Order).ToArray();

                data.category_exams = (from a in _examcontext.AcademicYear
                                       from b in _examcontext.Exm_Master_CategoryDMO
                                       from c in _examcontext.exammasterDMO
                                       from d in _examcontext.Exm_Yearly_CategoryDMO
                                       from e in _examcontext.Exm_Yearly_Category_ExamsDMO
                                       from f in _examcontext.Exm_Master_GradeDMO
                                       where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == c.MI_Id && f.MI_Id == a.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.EMCA_Id == b.EMCA_Id && d.EYC_Id == e.EYC_Id && c.EME_Id == e.EME_Id && e.EMGR_Id == f.EMGR_Id)
                                       select new ExamSubjectMappingDTO
                                       {
                                           EYCE_Id = e.EYCE_Id,
                                           EYC_Id = e.EYC_Id,
                                           EMCA_Id = d.EMCA_Id,
                                           ASMAY_Id = d.ASMAY_Id,
                                           EME_Id = e.EME_Id,
                                           ASMAY_Year = a.ASMAY_Year,
                                           EMCA_CategoryName = b.EMCA_CategoryName,
                                           EME_ExamName = c.EME_ExamName,
                                           EME_ExamCode = c.EME_ExamCode,
                                           EMGR_GradeName = f.EMGR_GradeName,
                                           EYCEAttendanceFromDate = e.EYCE_AttendanceFromDate.Value.Date.ToString("dd/MM/yyyy"),
                                           EYCEAttendanceToDate = e.EYCE_AttendanceToDate.Value.Date.ToString("dd/MM/yyyy"),
                                           EYCE_SubExamFlg = e.EYCE_SubExamFlg,
                                           EYCE_SubSubjectFlg = e.EYCE_SubSubjectFlg,
                                           EYCE_ActiveFlg = e.EYCE_ActiveFlg,
                                           ASMAY_Order = a.ASMAY_Order,
                                           EYCE_BestOf = e.EYCE_BestOf,
                                           EYCE_BestOfApplicableFlg = e.EYCE_BestOfApplicableFlg
                                       }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.subsubjectsubexamlist = (from a in _examcontext.mastersubsubject
                                              from b in _examcontext.mastersubexam
                                              where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.EMSS_ActiveFlag == true
                                              && b.EMSE_ActiveFlag == true)
                                              select new ExamSubjectMappingDTO
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
        public DateTime? CreatedDate = DateTime.Now;
        public ExamSubjectMappingDTO savedetails(ExamSubjectMappingDTO _category)
        {
            Exm_Yearly_Category_ExamsDMO objpge = Mapper.Map<Exm_Yearly_Category_ExamsDMO>(_category);
            try
             {
                if (objpge.EYCE_Id > 0)
                {
                    ExamSubjectMappingDTO deleted_result = delete_records_of_cat_exm(_category.EYCE_Id);
                    if (deleted_result.returnval == true)
                    {
                        _category.EYCE_Id = 0;

                        for (int i = 0; i < _category.exams_list.Length; i++)
                        {
                            Exm_Yearly_Category_ExamsDMO objpge1 = Mapper.Map<Exm_Yearly_Category_ExamsDMO>(_category);
                            var result = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == objpge1.EYC_Id
                            && t.EME_Id == _category.exams_list[i].EME_Id).ToList();

                            if (result.Count() == 0)
                            {
                                objpge1.EME_Id = _category.exams_list[i].EME_Id;
                                objpge1.EYCE_ActiveFlg = true;
                                objpge1.CreatedDate = DateTime.Now;
                                objpge1.UpdatedDate = DateTime.Now;
                                objpge1.EYCE_BestOfApplicableFlg = _category.EYCE_BestOfApplicableFlg;
                                objpge1.EYCE_BestOf = _category.EYCE_BestOf;
                                _examcontext.Add(objpge1);

                                for (int j = 0; j < _category.exm_subjects_list.Length; j++)
                                {
                                    Exm_Yrly_Cat_Exams_SubwiseDMO obj_subs = Mapper.Map<Exm_Yrly_Cat_Exams_SubwiseDMO>(_category.exm_subjects_list[j]);
                                    obj_subs.EYCE_Id = objpge1.EYCE_Id;
                                    obj_subs.EYCES_ActiveFlg = true;
                                    obj_subs.CreatedDate = DateTime.Now;
                                    obj_subs.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj_subs);

                                    if (_category.exm_subjects_list[j].Exam_Subject_PT_GradeList != null
                                        && _category.exm_subjects_list[j].Exam_Subject_PT_GradeList.Length > 0)
                                    {
                                        foreach (var c in _category.exm_subjects_list[j].Exam_Subject_PT_GradeList)
                                        {
                                            Exm_Yrly_Cat_Exams_Subwise_PTDMO exm_Yrly_Cat_Exams_Subwise_PTDMO = new Exm_Yrly_Cat_Exams_Subwise_PTDMO
                                            {
                                                EYCES_Id = obj_subs.EYCES_Id,
                                                EMPATY_Id = c.EMPATY_Id,
                                                EMGR_Id = c.EMGR_Id,
                                                EYCESPT_ActiveFlg = true,
                                                EYCESPT_CreatedBy = _category.UserId,
                                                EYCESPT_CreatedDate = DateTime.Now,
                                                EYCESPT_UpdatedBy = _category.UserId,
                                                EYCESPT_UpdatedDate = DateTime.Now
                                            };
                                            _examcontext.Add(exm_Yrly_Cat_Exams_Subwise_PTDMO);
                                        }
                                    }

                                    if (obj_subs.EYCES_SubExamFlg == true && obj_subs.EYCES_SubSubjectFlg == false)
                                    {
                                        for (int x = 0; x < _category.exm_subject_subexams_list.Length; x++)
                                        {
                                            if (obj_subs.ISMS_Id == _category.exm_subject_subexams_list[x].ISMS_Id)
                                            {
                                                for (int a = 0; a < _category.exm_subject_subexams_list[x].sub_exam_list.Length; a++)
                                                {
                                                    Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO obj_sub_subexms = new Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO();
                                                    obj_sub_subexms.EMSE_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMSE_Id;
                                                    obj_sub_subexms.EMGR_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMGR_Id;
                                                    obj_sub_subexms.EYCESSS_ExemptedFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_ExemptedFlg;
                                                    obj_sub_subexms.EYCESSS_ExemptedPer = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_ExemptedPer;
                                                    obj_sub_subexms.EYCESSS_MaxMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_MaxMarks;
                                                    obj_sub_subexms.EYCESSS_MinMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_MinMarks;
                                                    obj_sub_subexms.EYCESSS_MarksEntryMax = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_MarksEntryMax;
                                                    obj_sub_subexms.EYCESSS_MarksFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_MarksFlg;
                                                    obj_sub_subexms.EYCESSS_GradesFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_GradesFlg;
                                                    obj_sub_subexms.EYCESSS_AplResultFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_AplResultFlg;
                                                    obj_sub_subexms.EYCESSS_SubSubjectOrder = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_SubExamOrder;
                                                    obj_sub_subexms.EMSS_Id = 0;
                                                    obj_sub_subexms.EYCES_Id = obj_subs.EYCES_Id;
                                                    obj_sub_subexms.EYCESSS_ActiveFlg = true;
                                                    obj_sub_subexms.CreatedDate = DateTime.Now;
                                                    obj_sub_subexms.UpdatedDate = DateTime.Now;

                                                    _examcontext.Add(obj_sub_subexms);
                                                }
                                            }
                                        }
                                    }

                                    else if (obj_subs.EYCES_SubExamFlg == false && obj_subs.EYCES_SubSubjectFlg == true)
                                    {
                                        for (int y = 0; y < _category.exm_subject_subsubjects_list.Length; y++)
                                        {
                                            if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_list[y].ISMS_Id)
                                            {
                                                for (int b = 0; b < _category.exm_subject_subsubjects_list[y].sub_subjs_list.Length; b++)
                                                {
                                                    Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO obj_sub_subsubjs = Mapper.Map<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>(_category.exm_subject_subsubjects_list[y].sub_subjs_list[b]);
                                                    obj_sub_subsubjs.EYCES_Id = obj_subs.EYCES_Id;
                                                    obj_sub_subsubjs.EYCESSS_ActiveFlg = true;
                                                    obj_sub_subsubjs.CreatedDate = DateTime.Now;
                                                    obj_sub_subsubjs.UpdatedDate = DateTime.Now;
                                                    _examcontext.Add(obj_sub_subsubjs);
                                                }
                                            }
                                        }
                                    }

                                    else
                                    {
                                        for (int y = 0; y < _category.exm_subject_subsubjects_subexam.Length; y++)
                                        {
                                            if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_subexam[y].ISMS_Id)
                                            {
                                                for (int b = 0; b < _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list.Length; b++)
                                                {
                                                    Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO obj_sub_subsubjs = Mapper.Map<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>(_category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b]);
                                                    obj_sub_subsubjs.EYCES_Id = obj_subs.EYCES_Id;
                                                    obj_sub_subsubjs.EYCESSS_ActiveFlg = true;
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
                                var result_new = _examcontext.Exm_Yearly_Category_ExamsDMO.Single(t => t.EYC_Id == objpge1.EYC_Id && t.EME_Id == _category.exams_list[i].EME_Id && t.EYCE_Id == objpge.EYCE_Id);

                                result_new.EME_Id = _category.exams_list[i].EME_Id;
                                result_new.UpdatedDate = DateTime.Now;
                                result_new.EMGR_Id = _category.EMGR_Id;
                                result_new.EYCE_SubExamFlg = _category.EYCE_SubExamFlg;
                                result_new.EYCE_SubSubjectFlg = _category.EYCE_SubSubjectFlg;
                                result_new.EYCE_AttendanceFromDate = _category.EYCE_AttendanceFromDate;
                                result_new.EYCE_AttendanceToDate = _category.EYCE_AttendanceToDate;
                                result_new.EYCE_BestOfApplicableFlg = _category.EYCE_BestOfApplicableFlg;
                                result_new.EYCE_BestOf = _category.EYCE_BestOf;
                                _examcontext.Update(result_new);

                                for (int j = 0; j < _category.exm_subjects_list.Length; j++)
                                {
                                    Exm_Yrly_Cat_Exams_SubwiseDMO obj_subs = Mapper.Map<Exm_Yrly_Cat_Exams_SubwiseDMO>(_category.exm_subjects_list[j]);
                                    obj_subs.EYCE_Id = result_new.EYCE_Id;
                                    obj_subs.EYCES_ActiveFlg = true;
                                    obj_subs.CreatedDate = DateTime.Now;
                                    obj_subs.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj_subs);

                                    if (_category.exm_subjects_list[j].Exam_Subject_PT_GradeList != null
                                        && _category.exm_subjects_list[j].Exam_Subject_PT_GradeList.Length > 0)
                                    {
                                        foreach (var c in _category.exm_subjects_list[j].Exam_Subject_PT_GradeList)
                                        {
                                            Exm_Yrly_Cat_Exams_Subwise_PTDMO exm_Yrly_Cat_Exams_Subwise_PTDMO = new Exm_Yrly_Cat_Exams_Subwise_PTDMO
                                            {
                                                EYCES_Id = obj_subs.EYCES_Id,
                                                EMPATY_Id = c.EMPATY_Id,
                                                EMGR_Id = c.EMGR_Id,
                                                EYCESPT_ActiveFlg = true,
                                                EYCESPT_CreatedBy = _category.UserId,
                                                EYCESPT_CreatedDate = DateTime.Now,
                                                EYCESPT_UpdatedBy = _category.UserId,
                                                EYCESPT_UpdatedDate = DateTime.Now
                                            };
                                            _examcontext.Add(exm_Yrly_Cat_Exams_Subwise_PTDMO);
                                        }
                                    }


                                    if (obj_subs.EYCES_SubExamFlg == true && obj_subs.EYCES_SubSubjectFlg == false)
                                    {
                                        for (int x = 0; x < _category.exm_subject_subexams_list.Length; x++)
                                        {
                                            if (obj_subs.ISMS_Id == _category.exm_subject_subexams_list[x].ISMS_Id)
                                            {
                                                for (int a = 0; a < _category.exm_subject_subexams_list[x].sub_exam_list.Length; a++)
                                                {
                                                    Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO obj_sub_subexms = new Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO();
                                                    obj_sub_subexms.EMSE_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMSE_Id;
                                                    obj_sub_subexms.EMGR_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMGR_Id;
                                                    obj_sub_subexms.EYCESSS_ExemptedFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_ExemptedFlg;
                                                    obj_sub_subexms.EYCESSS_ExemptedPer = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_ExemptedPer;
                                                    obj_sub_subexms.EYCESSS_MaxMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_MaxMarks;
                                                    obj_sub_subexms.EYCESSS_MinMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_MinMarks;
                                                    obj_sub_subexms.EYCESSS_MarksEntryMax = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_MarksEntryMax;
                                                    obj_sub_subexms.EYCESSS_MarksFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_MarksFlg;
                                                    obj_sub_subexms.EYCESSS_GradesFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_GradesFlg;
                                                    obj_sub_subexms.EYCESSS_AplResultFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_AplResultFlg;
                                                    obj_sub_subexms.EYCESSS_SubSubjectOrder = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_SubExamOrder;
                                                    obj_sub_subexms.EMSS_Id = 0;
                                                    obj_sub_subexms.EYCES_Id = obj_subs.EYCES_Id;
                                                    obj_sub_subexms.EYCESSS_ActiveFlg = true;
                                                    obj_sub_subexms.CreatedDate = DateTime.Now;
                                                    obj_sub_subexms.UpdatedDate = DateTime.Now;
                                                    _examcontext.Add(obj_sub_subexms);
                                                }
                                            }
                                        }
                                    }

                                    else if (obj_subs.EYCES_SubExamFlg == false && obj_subs.EYCES_SubSubjectFlg == true)
                                    {
                                        for (int y = 0; y < _category.exm_subject_subsubjects_list.Length; y++)
                                        {
                                            if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_list[y].ISMS_Id)
                                            {
                                                for (int b = 0; b < _category.exm_subject_subsubjects_list[y].sub_subjs_list.Length; b++)
                                                {
                                                    Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO obj_sub_subsubjs = Mapper.Map<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>(_category.exm_subject_subsubjects_list[y].sub_subjs_list[b]);

                                                    obj_sub_subsubjs.EYCES_Id = obj_subs.EYCES_Id;
                                                    obj_sub_subsubjs.EYCESSS_ActiveFlg = true;
                                                    obj_sub_subsubjs.CreatedDate = DateTime.Now;
                                                    obj_sub_subsubjs.UpdatedDate = DateTime.Now;
                                                    _examcontext.Add(obj_sub_subsubjs);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int y = 0; y < _category.exm_subject_subsubjects_subexam.Length; y++)
                                        {
                                            if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_subexam[y].ISMS_Id)
                                            {
                                                for (int b = 0; b < _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list.Length; b++)
                                                {
                                                    Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO obj_sub_subsubjs = Mapper.Map<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>(_category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b]);
                                                    obj_sub_subsubjs.EYCES_Id = obj_subs.EYCES_Id;
                                                    obj_sub_subsubjs.EYCESSS_ActiveFlg = true;
                                                    obj_sub_subsubjs.CreatedDate = DateTime.Now;
                                                    obj_sub_subsubjs.UpdatedDate = DateTime.Now;
                                                    _examcontext.Add(obj_sub_subsubjs);
                                                }
                                            }
                                        }
                                    }
                                }
                                //_category.returnduplicatestatus = "Duplicate";
                            }
                        }

                        var contactExists = _examcontext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                    _category.EYCE_Id = objpge.EYCE_Id;
                }

                else
                {
                    for (int i = 0; i < _category.exams_list.Length; i++)
                    {
                        Exm_Yearly_Category_ExamsDMO objpge1 = Mapper.Map<Exm_Yearly_Category_ExamsDMO>(_category);
                        var result = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == objpge1.EYC_Id && t.EME_Id == _category.exams_list[i].EME_Id).ToList();//&& t.EMGR_Id == objpge1.EMGR_Id
                        if (result.Count() == 0)
                        {
                            objpge1.EME_Id = _category.exams_list[i].EME_Id;
                            objpge1.EYCE_ActiveFlg = true;
                            objpge1.CreatedDate = DateTime.Now;
                            objpge1.UpdatedDate = DateTime.Now;
                            objpge1.EYCE_BestOfApplicableFlg = _category.EYCE_BestOfApplicableFlg;
                            objpge1.EYCE_BestOf = _category.EYCE_BestOf;
                            _examcontext.Add(objpge1);

                            for (int j = 0; j < _category.exm_subjects_list.Length; j++)
                            {
                                Exm_Yrly_Cat_Exams_SubwiseDMO obj_subs = Mapper.Map<Exm_Yrly_Cat_Exams_SubwiseDMO>(_category.exm_subjects_list[j]);
                                obj_subs.EYCE_Id = objpge1.EYCE_Id;
                                obj_subs.EYCES_ActiveFlg = true;
                                obj_subs.CreatedDate = DateTime.Now;
                                obj_subs.UpdatedDate = DateTime.Now;
                                _examcontext.Add(obj_subs);

                                if (_category.exm_subjects_list[j].Exam_Subject_PT_GradeList != null
                                    && _category.exm_subjects_list[j].Exam_Subject_PT_GradeList.Length > 0)
                                {
                                    foreach (var c in _category.exm_subjects_list[j].Exam_Subject_PT_GradeList)
                                    {
                                        Exm_Yrly_Cat_Exams_Subwise_PTDMO exm_Yrly_Cat_Exams_Subwise_PTDMO = new Exm_Yrly_Cat_Exams_Subwise_PTDMO
                                        {
                                            EYCES_Id = obj_subs.EYCES_Id,
                                            EMPATY_Id = c.EMPATY_Id,
                                            EMGR_Id = c.EMGR_Id,
                                            EYCESPT_ActiveFlg = true,
                                            EYCESPT_CreatedBy = _category.UserId,
                                            EYCESPT_CreatedDate = DateTime.Now,
                                            EYCESPT_UpdatedBy = _category.UserId,
                                            EYCESPT_UpdatedDate = DateTime.Now
                                        };
                                        _examcontext.Add(exm_Yrly_Cat_Exams_Subwise_PTDMO);
                                    }
                                }

                                if (obj_subs.EYCES_SubExamFlg == true && obj_subs.EYCES_SubSubjectFlg == false)
                                {
                                    for (int x = 0; x < _category.exm_subject_subexams_list.Length; x++)
                                    {
                                        if (obj_subs.ISMS_Id == _category.exm_subject_subexams_list[x].ISMS_Id)
                                        {
                                            for (int a = 0; a < _category.exm_subject_subexams_list[x].sub_exam_list.Length; a++)
                                            {
                                                Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO obj_sub_subexms = new Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO();

                                                obj_sub_subexms.EMSE_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMSE_Id;
                                                obj_sub_subexms.EMGR_Id = _category.exm_subject_subexams_list[x].sub_exam_list[a].EMGR_Id;
                                                obj_sub_subexms.EYCESSS_ExemptedFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_ExemptedFlg;
                                                obj_sub_subexms.EYCESSS_ExemptedPer = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_ExemptedPer;
                                                obj_sub_subexms.EYCESSS_MaxMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_MaxMarks;
                                                obj_sub_subexms.EYCESSS_MinMarks = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_MinMarks;
                                                obj_sub_subexms.EYCESSS_MarksEntryMax = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_MarksEntryMax;
                                                obj_sub_subexms.EYCESSS_MarksFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_MarksFlg;
                                                obj_sub_subexms.EYCESSS_GradesFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_GradesFlg;
                                                obj_sub_subexms.EYCESSS_AplResultFlg = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSS_AplResultFlg;
                                                obj_sub_subexms.EYCESSS_SubSubjectOrder = _category.exm_subject_subexams_list[x].sub_exam_list[a].EYCESSE_SubExamOrder;
                                                obj_sub_subexms.EMSS_Id = 0;
                                                obj_sub_subexms.EYCES_Id = obj_subs.EYCES_Id;
                                                obj_sub_subexms.EYCESSS_ActiveFlg = true;
                                                obj_sub_subexms.CreatedDate = DateTime.Now;
                                                obj_sub_subexms.UpdatedDate = DateTime.Now;
                                                _examcontext.Add(obj_sub_subexms);
                                            }
                                        }
                                    }
                                }

                                else if (obj_subs.EYCES_SubExamFlg == false && obj_subs.EYCES_SubSubjectFlg == true)
                                {
                                    for (int y = 0; y < _category.exm_subject_subsubjects_list.Length; y++)
                                    {
                                        if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_list[y].ISMS_Id)
                                        {
                                            for (int b = 0; b < _category.exm_subject_subsubjects_list[y].sub_subjs_list.Length; b++)
                                            {
                                                Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO obj_sub_subsubjs = Mapper.Map<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>(_category.exm_subject_subsubjects_list[y].sub_subjs_list[b]);
                                                obj_sub_subsubjs.EYCES_Id = obj_subs.EYCES_Id;
                                                obj_sub_subsubjs.EYCESSS_ActiveFlg = true;
                                                obj_sub_subsubjs.CreatedDate = DateTime.Now;
                                                obj_sub_subsubjs.UpdatedDate = DateTime.Now;
                                                _examcontext.Add(obj_sub_subsubjs);
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    for (int y = 0; y < _category.exm_subject_subsubjects_subexam.Length; y++)
                                    {
                                        if (obj_subs.ISMS_Id == _category.exm_subject_subsubjects_subexam[y].ISMS_Id)
                                        {
                                            for (int b = 0; b < _category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list.Length; b++)
                                            {
                                                Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO obj_sub_subsubjs = Mapper.Map<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>(_category.exm_subject_subsubjects_subexam[y].sub_subject_sub_exam_list[b]);
                                                obj_sub_subsubjs.EYCES_Id = obj_subs.EYCES_Id;
                                                obj_sub_subsubjs.EYCESSS_ActiveFlg = true;
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

                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        _category.returnval = true;
                    }
                    else
                    {
                        _category.returnval = false;
                    }


                }

                _category.category_exams = (from a in _examcontext.AcademicYear
                                            from b in _examcontext.Exm_Master_CategoryDMO
                                            from c in _examcontext.exammasterDMO
                                            from d in _examcontext.Exm_Yearly_CategoryDMO
                                            from e in _examcontext.Exm_Yearly_Category_ExamsDMO
                                            from f in _examcontext.Exm_Master_GradeDMO
                                            where (a.MI_Id == _category.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == c.MI_Id && f.MI_Id == a.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.EMCA_Id == b.EMCA_Id && d.EYC_Id == e.EYC_Id && c.EME_Id == e.EME_Id && e.EMGR_Id == f.EMGR_Id)
                                            select new ExamSubjectMappingDTO
                                            {
                                                EYCE_Id = e.EYCE_Id,
                                                EYC_Id = e.EYC_Id,
                                                EMCA_Id = d.EMCA_Id,
                                                ASMAY_Id = d.ASMAY_Id,
                                                EME_Id = e.EME_Id,
                                                ASMAY_Year = a.ASMAY_Year,
                                                EMCA_CategoryName = b.EMCA_CategoryName,
                                                EME_ExamName = c.EME_ExamName,
                                                EME_ExamCode = c.EME_ExamCode,
                                                EMGR_GradeName = f.EMGR_GradeName,
                                                EYCE_AttendanceFromDate = e.EYCE_AttendanceFromDate,
                                                EYCE_AttendanceToDate = e.EYCE_AttendanceToDate,
                                                EYCE_SubExamFlg = e.EYCE_SubExamFlg,
                                                EYCE_SubSubjectFlg = e.EYCE_SubSubjectFlg,
                                                EYCE_ActiveFlg = e.EYCE_ActiveFlg,
                                                EYCE_BestOf = e.EYCE_BestOf,
                                                EYCE_BestOfApplicableFlg = e.EYCE_BestOfApplicableFlg
                                            }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return _category;
        }
        public ExamSubjectMappingDTO editdetails(int id)
        {
            ExamSubjectMappingDTO page = new ExamSubjectMappingDTO();
            try
            {
                page.edit_cat_exm = (from a in _examcontext.Exm_Yearly_Category_ExamsDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.EYCE_Id == id && a.EYC_Id == b.EYC_Id)
                                     select new ExamSubjectMappingDTO
                                     {
                                         ASMAY_Id = b.ASMAY_Id,
                                         EYC_Id = a.EYC_Id,
                                         EME_Id = a.EME_Id,
                                         EMGR_Id = a.EMGR_Id,
                                         EYCE_AttendanceFromDate = a.EYCE_AttendanceFromDate,
                                         EYCE_AttendanceToDate = a.EYCE_AttendanceToDate,
                                         EYCE_SubExamFlg = a.EYCE_SubExamFlg,
                                         EYCE_SubSubjectFlg = a.EYCE_SubSubjectFlg,
                                         EYCE_Id = a.EYCE_Id,
                                         EYCE_ActiveFlg = a.EYCE_ActiveFlg,
                                         EYCE_BestOf = a.EYCE_BestOf,
                                         EYCE_BestOfApplicableFlg = a.EYCE_BestOfApplicableFlg
                                     }).Distinct().ToArray();

                page.edit_cat_exm_subs = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(ce => ce.EYCE_Id == id).Distinct().ToArray();

                List<int> subs = new List<int>();
                foreach (Exm_Yrly_Cat_Exams_SubwiseDMO x in page.edit_cat_exm_subs)
                {
                    subs.Add(x.EYCES_Id);
                }

                page.edit_cat_exm_subs_sub_subjs = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Where(e => subs.Contains(e.EYCES_Id)).Distinct().ToArray();

                page.edit_cat_exm_subs_grade_list = _examcontext.Exm_Yrly_Cat_Exams_Subwise_PTDMO.Where(e => subs.Contains(e.EYCES_Id)).Distinct().ToArray();

                var GetEYCId = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(a => a.EYCE_Id == id).ToList();

                if (GetEYCId.Count > 0)
                {
                    var eycdetails = _examcontext.Exm_Yearly_CategoryDMO.Where(a => a.EYC_Id == GetEYCId.FirstOrDefault().EYC_Id).ToList();

                    if (eycdetails.Count > 0 && eycdetails.FirstOrDefault().EYC_BasedOnPaperTypeFlg == true)
                    {
                        page.Get_Master_PT = _examcontext.Exm_Master_PaperTypeDMO.Where(a => a.MI_Id == eycdetails.FirstOrDefault().MI_Id
                        && a.EMPATY_ActiveFlag == true).ToArray();
                    }
                }

                var geteycid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(a => a.EYCE_Id == id).ToList();

                var getemcid = _examcontext.Exm_Yearly_CategoryDMO.Where(a => a.EYC_Id == geteycid.FirstOrDefault().EYC_Id).ToList();

                var check_marks_enetered_not = (from a in _examcontext.ExamMarksDMO
                                                from b in _examcontext.Exm_Category_ClassDMO
                                                where (a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id
                                                && a.ASMAY_Id == getemcid.FirstOrDefault().ASMAY_Id && b.ASMAY_Id == getemcid.FirstOrDefault().ASMAY_Id
                                                && b.EMCA_Id == getemcid.FirstOrDefault().EMCA_Id
                                                && a.EME_Id == geteycid.FirstOrDefault().EME_Id && b.ECAC_ActiveFlag == true)
                                                select a).ToList();

                if (check_marks_enetered_not.Count > 0)
                {
                    page.edit_exam_flag = false;
                }
                else
                {
                    page.edit_exam_flag = true;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return page;
        }
        public ExamSubjectMappingDTO get_category(ExamSubjectMappingDTO data)
        {
            // ExamSubjectMappingDTO editlt = new ExamSubjectMappingDTO();
            try
            {
                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_ActiveFlg == true)
                                     select new ExamSubjectMappingDTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName,
                                         EYC_Id = b.EYC_Id,

                                     }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public ExamSubjectMappingDTO get_subjects(ExamSubjectMappingDTO data)
        {
            try
            {
                data.subjectlist = (from a in _examcontext.Exm_Yearly_Category_GroupDMO
                                    from b in _examcontext.Exm_Yearly_Category_Group_SubjectsDMO
                                    from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                    where (c.MI_Id == data.MI_Id && a.EYC_Id == data.EYC_Id && a.EYCG_Id == b.EYCG_Id && b.ISMS_Id == c.ISMS_Id
                                    && b.EYCGS_ActiveFlg == true && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1 && a.EYCG_ActiveFlg == true)
                                    select new ExamSubjectMappingDTO
                                    {
                                        ISMS_Id = c.ISMS_Id,
                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                        ISMS_SubjectCode = c.ISMS_SubjectCode,
                                        ISMS_Max_Marks = c.ISMS_Max_Marks,
                                        ISMS_Min_Marks = c.ISMS_Min_Marks,
                                        ISMS_OrderFlag = c.ISMS_OrderFlag,
                                    }).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                List<int> exams = new List<int>();
                exams = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id).Select(t => t.EME_Id).ToList();

                data.examlist = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true
                && !exams.Contains(t.EME_Id)).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                var check_pt_examType = _examcontext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_Id == data.EYC_Id && a.EYC_ActiveFlg == true && a.EYC_BasedOnPaperTypeFlg == true).ToList();

                if (check_pt_examType.Count > 0 && check_pt_examType.FirstOrDefault().EYC_BasedOnPaperTypeFlg == true)
                {
                    data.Get_Master_PT = _examcontext.Exm_Master_PaperTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.EMPATY_ActiveFlag == true).ToArray();
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public ExamSubjectMappingDTO deactivate(ExamSubjectMappingDTO data)
        {
            data.already_cnt = false;
            Exm_Yearly_Category_ExamsDMO pge = Mapper.Map<Exm_Yearly_Category_ExamsDMO>(data);
            if (pge.EYCE_Id > 0)
            {
                var result = _examcontext.Exm_Yearly_Category_ExamsDMO.Single(t => t.EYCE_Id == pge.EYCE_Id);

                if (result.EYCE_ActiveFlg == true)
                {
                    //  var Exm_Yrly_Cat_Exams_SubwiseDMO_cnt = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => t.EYCE_Id == result.EYCE_Id).ToList();
                    // data.MI_Id = _examcontext.Exm_Yearly_CategoryDMO.Where(t => t.EYC_Id == result.EYC_Id).Select(t => t.MI_Id).FirstOrDefault();
                    var category_y = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == result.EYC_Id);
                    var class_sec = _examcontext.Exm_Category_ClassDMO.Where(t => t.MI_Id == category_y.MI_Id && t.ASMAY_Id == category_y.ASMAY_Id && t.EMCA_Id == category_y.EMCA_Id).Distinct().ToList();
                    var ExamMarksDMO_cnt = 0;
                    for (int i = 0; i < class_sec.Count; i++)
                    {
                        ExamMarksDMO_cnt = ExamMarksDMO_cnt + _examcontext.ExamMarksDMO.Where(t => t.MI_Id == category_y.MI_Id && t.ASMAY_Id == class_sec[i].ASMAY_Id && t.ASMCL_Id == class_sec[i].ASMCL_Id && t.ASMS_Id == class_sec[i].ASMS_Id && t.EME_Id == result.EME_Id).ToList().Count;

                    }
                    var Exm_M_PromotionDMO_cnt = (from a in _examcontext.Exm_M_PromotionDMO
                                                  from b in _examcontext.Exm_M_Promotion_SubjectsDMO
                                                  from c in _examcontext.Exm_M_Prom_Subj_GroupDMO
                                                  from d in _examcontext.Exm_M_Prom_Subj_Group_ExamsDMO
                                                  where (a.MI_Id == category_y.MI_Id && a.EYC_Id == result.EYC_Id && a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == result.EME_Id)
                                                  select new ExamSubjectMappingDTO
                                                  {
                                                      EYC_Id = a.EYC_Id,
                                                      EME_Id = d.EME_Id,
                                                  }).Distinct().ToList();
                    if (Exm_M_PromotionDMO_cnt.Count == 0 && ExamMarksDMO_cnt == 0) //Exm_Yrly_Cat_Exams_SubwiseDMO_cnt.Count == 0 &&
                    {
                        result.EYCE_ActiveFlg = false;
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }
                    // result.EYCE_ActiveFlg = false;
                }
                else
                {
                    result.EYCE_ActiveFlg = true;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                }
                // result.UpdatedDate = DateTime.Now;
                //_examcontext.Update(result);
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
        public ExamSubjectMappingDTO deactivate_sub(ExamSubjectMappingDTO data)
        {
            data.already_cnt = false;
            Exm_Yrly_Cat_Exams_SubwiseDMO pge = Mapper.Map<Exm_Yrly_Cat_Exams_SubwiseDMO>(data);
            if (pge.EYCES_Id > 0)
            {
                var result = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Single(t => t.EYCES_Id == pge.EYCES_Id);
                if (result.EYCES_ActiveFlg == true)
                {
                    var exam_y = _examcontext.Exm_Yearly_Category_ExamsDMO.Single(t => t.EYCE_Id == result.EYCE_Id);
                    var category_y = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == exam_y.EYC_Id);
                    var class_sec = _examcontext.Exm_Category_ClassDMO.Where(t => t.MI_Id == category_y.MI_Id && t.ASMAY_Id == category_y.ASMAY_Id && t.EMCA_Id == category_y.EMCA_Id).Distinct().ToList();
                    var ExamMarksDMO_cnt = 0;
                    for (int i = 0; i < class_sec.Count; i++)
                    {
                        ExamMarksDMO_cnt = ExamMarksDMO_cnt + _examcontext.ExamMarksDMO.Where(t => t.MI_Id == category_y.MI_Id && t.ASMCL_Id == class_sec[i].ASMCL_Id && t.ASMS_Id == class_sec[i].ASMS_Id && t.EME_Id == exam_y.EME_Id && t.ISMS_Id == result.ISMS_Id).ToList().Count;

                    }
                    var Exm_M_PromotionDMO_cnt = (from a in _examcontext.Exm_M_PromotionDMO
                                                  from b in _examcontext.Exm_M_Promotion_SubjectsDMO
                                                  from c in _examcontext.Exm_M_Prom_Subj_GroupDMO
                                                  from d in _examcontext.Exm_M_Prom_Subj_Group_ExamsDMO
                                                  where (a.MI_Id == category_y.MI_Id && a.EYC_Id == exam_y.EYC_Id && a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id && c.EMPSG_Id == d.EMPSG_Id && d.EME_Id == exam_y.EME_Id && b.ISMS_Id == result.ISMS_Id)
                                                  select new ExamSubjectMappingDTO
                                                  {
                                                      EYC_Id = a.EYC_Id,
                                                      EME_Id = d.EME_Id,
                                                      ISMS_Id = b.ISMS_Id,
                                                  }).Distinct().ToList();
                    if (Exm_M_PromotionDMO_cnt.Count == 0 && ExamMarksDMO_cnt == 0) //Exm_Yrly_Cat_Exams_SubwiseDMO_cnt.Count == 0 &&
                    {
                        result.EYCES_ActiveFlg = false;
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                    }
                    else
                    {
                        data.already_cnt = true;
                    }
                    // result.EYCES_ActiveFlg = false;
                }
                else
                {
                    result.EYCES_ActiveFlg = true;
                    result.UpdatedDate = DateTime.Now;
                    _examcontext.Update(result);
                }
                // result.UpdatedDate = DateTime.Now;
                //_examcontext.Update(result);
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
        public ExamSubjectMappingDTO deactive_sub_exm(ExamSubjectMappingDTO data)
        {
            // Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO pge = Mapper.Map<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>(data);
            if (data.EYCESSE_Id > 0)
            {
                var result = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Single(t => t.EYCESSS_Id == data.EYCESSE_Id);
                if (result.EYCESSS_ActiveFlg == true)
                {
                    result.EYCESSS_ActiveFlg = false;
                }
                else
                {
                    result.EYCESSS_ActiveFlg = true;
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

            return data;
        }
        public ExamSubjectMappingDTO deactive_sub_subj(ExamSubjectMappingDTO data)
        {
            Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO pge = Mapper.Map<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>(data);
            if (pge.EYCESSS_Id > 0)
            {
                var result = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Single(t => t.EYCESSS_Id == pge.EYCESSS_Id);
                if (result.EYCESSS_ActiveFlg == true)
                {
                    result.EYCESSS_ActiveFlg = false;
                }
                else
                {
                    result.EYCESSS_ActiveFlg = true;
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


            return data;
        }
        public ExamSubjectMappingDTO deactive_sub_subj_subexam(ExamSubjectMappingDTO data)
        {
            Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO pge = Mapper.Map<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>(data);
            if (pge.EYCESSS_Id > 0)
            {
                var result = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Single(t => t.EYCESSS_Id == pge.EYCESSS_Id);
                if (result.EYCESSS_ActiveFlg == true)
                {
                    result.EYCESSS_ActiveFlg = false;
                }
                else
                {
                    result.EYCESSS_ActiveFlg = true;
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


            return data;
        }
        public ExamSubjectMappingDTO getalldetailsviewrecords(int id)
        {
            ExamSubjectMappingDTO page = new ExamSubjectMappingDTO();
            try
            {
                page.view_exam_subjects = (from a in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                           from b in _examcontext.Exm_Master_CategoryDMO
                                           from c in _examcontext.exammasterDMO
                                           from d in _examcontext.Exm_Yearly_CategoryDMO
                                           from e in _examcontext.Exm_Yearly_Category_ExamsDMO
                                           from f in _examcontext.Exm_Master_GradeDMO
                                           from g in _examcontext.IVRM_School_Master_SubjectsDMO
                                           where (a.EYCE_Id == e.EYCE_Id && e.EYC_Id == d.EYC_Id && d.EMCA_Id == b.EMCA_Id && b.MI_Id == d.MI_Id && b.MI_Id == f.MI_Id && c.MI_Id == b.MI_Id && a.ISMS_Id == g.ISMS_Id && a.EMGR_Id == f.EMGR_Id && a.EYCE_Id == id && e.EME_Id == c.EME_Id)
                                           select new ExamSubjectMappingDTO
                                           {
                                               EYCES_Id = a.EYCES_Id,
                                               EYCE_Id = a.EYCE_Id,
                                               ISMS_Id = a.ISMS_Id,
                                               EMCA_CategoryName = b.EMCA_CategoryName,
                                               EME_ExamName = c.EME_ExamName,
                                               EMGR_GradeName = f.EMGR_GradeName,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               EYCES_MaxMarks = a.EYCES_MaxMarks,
                                               EYCES_MinMarks = a.EYCES_MinMarks,
                                               EYCES_MarksEntryMax = a.EYCES_MarksEntryMax,
                                               EYCES_SubExamFlg = a.EYCES_SubExamFlg,
                                               EYCES_SubSubjectFlg = a.EYCES_SubSubjectFlg,
                                               EYCES_MarksGradeEntryFlg = a.EYCES_MarksGradeEntryFlg,
                                               EYCES_MarksDisplayFlg = a.EYCES_MarksDisplayFlg,
                                               EYCES_GradeDisplayFlg = a.EYCES_GradeDisplayFlg,
                                               EYCES_AplResultFlg = a.EYCES_AplResultFlg,
                                               EYCES_SubjectOrder = a.EYCES_SubjectOrder,
                                               EYCES_ActiveFlg = a.EYCES_ActiveFlg,
                                               count = _examcontext.Exm_Yrly_Cat_Exams_Subwise_PTDMO.Where(z => z.EYCES_Id == a.EYCES_Id).Count()
                                           }).Distinct().OrderBy(x => x.EYCES_SubjectOrder).ToArray();

                List<Exm_Yrly_Cat_Exams_SubwiseDMO> lorg2 = new List<Exm_Yrly_Cat_Exams_SubwiseDMO>();
                lorg2 = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => t.EYCE_Id == id).ToList();

                List<int> subs = new List<int>();
                foreach (Exm_Yrly_Cat_Exams_SubwiseDMO x in lorg2)
                {
                    subs.Add(x.EYCES_Id);
                }

                page.view_exam_subjects_grade_list = (from a in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                      from b in _examcontext.Exm_Yrly_Cat_Exams_Subwise_PTDMO
                                                      from c in _examcontext.Exm_Master_PaperTypeDMO
                                                      from d in _examcontext.Exm_Master_GradeDMO
                                                      where (a.EYCES_Id == b.EYCES_Id && b.EMPATY_Id == c.EMPATY_Id && b.EMGR_Id == d.EMGR_Id
                                                      && subs.Contains(a.EYCES_Id) && subs.Contains(b.EYCES_Id))
                                                      select new ExamSubjectMappingDTO
                                                      {
                                                          EYCES_Id = b.EYCES_Id,
                                                          ISMS_Id = a.ISMS_Id,
                                                          EMGR_Id = b.EMGR_Id,
                                                          EYCESPT_Id = b.EYCESPT_Id,
                                                          EMPATY_Id = b.EMPATY_Id,
                                                          EYCESPT_ActiveFlg = b.EYCESPT_ActiveFlg,
                                                          EMPATY_PaperTypeName = c.EMPATY_PaperTypeName,
                                                          EMGR_GradeName = d.EMGR_GradeName,
                                                      }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public ExamSubjectMappingDTO getalldetailsviewrecords_subexms(int id)
        {
            ExamSubjectMappingDTO page = new ExamSubjectMappingDTO();
            try
            {
                page.view_exam_subjects_subexams = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                    from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                    from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                                    from d in _examcontext.Exm_Master_GradeDMO
                                                    from e in _examcontext.mastersubexam
                                                    where (a.EYCES_Id == b.EYCES_Id && b.EYCES_Id == id && b.ISMS_Id == c.ISMS_Id && c.MI_Id == d.MI_Id && a.EMGR_Id == d.EMGR_Id && c.MI_Id == e.MI_Id && e.EMSE_Id == a.EMSE_Id)
                                                    select new ExamSubjectMappingDTO
                                                    {
                                                        EYCESSE_Id = a.EYCESSS_Id,
                                                        EYCES_Id = a.EYCES_Id,
                                                        ISMS_Id = b.ISMS_Id,
                                                        EMSE_Id = a.EMSE_Id,
                                                        EMGR_GradeName = d.EMGR_GradeName,
                                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                                        ISMS_SubjectCode = c.ISMS_SubjectCode,
                                                        EMSE_SubExamName = e.EMSE_SubExamName,
                                                        EMSE_SubExamCode = e.EMSE_SubExamCode,
                                                        EYCESSE_MaxMarks = a.EYCESSS_MaxMarks,
                                                        EYCESSE_MinMarks = a.EYCESSS_MinMarks,
                                                        EYCESSE_ExemptedFlg = a.EYCESSS_ExemptedFlg,
                                                        EYCESSE_ExemptedPer = a.EYCESSS_ExemptedPer,
                                                        EYCESSS_MarksEntryMax = a.EYCESSS_MarksEntryMax,
                                                        EYCESSE_SubExamOrder = a.EYCESSS_SubSubjectOrder,
                                                        EYCESSE_ActiveFlg = a.EYCESSS_ActiveFlg,
                                                        EYCESSS_GradesFlg = a.EYCESSS_GradesFlg,
                                                        EYCESSS_AplResultFlg = a.EYCESSS_AplResultFlg,
                                                        EYCESSS_MarksFlg = a.EYCESSS_MarksFlg

                                                    }).Distinct().OrderBy(x => x.EYCESSE_SubExamOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public ExamSubjectMappingDTO getalldetailsviewrecords_subsubjs(int id)
        {
            ExamSubjectMappingDTO page = new ExamSubjectMappingDTO();
            try
            {
                page.view_exam_subjects_subsubjects = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                       from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                       from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                                       from d in _examcontext.Exm_Master_GradeDMO
                                                       from e in _examcontext.mastersubsubject
                                                       where (a.EYCES_Id == b.EYCES_Id && b.EYCES_Id == id && b.ISMS_Id == c.ISMS_Id
                                                       && c.MI_Id == d.MI_Id && a.EMGR_Id == d.EMGR_Id && c.MI_Id == e.MI_Id && e.EMSS_Id == a.EMSS_Id)
                                                       select new ExamSubjectMappingDTO
                                                       {
                                                           EYCESSS_Id = a.EYCESSS_Id,
                                                           EYCES_Id = a.EYCES_Id,
                                                           ISMS_Id = b.ISMS_Id,
                                                           EMSS_Id = a.EMSS_Id,
                                                           EMGR_GradeName = d.EMGR_GradeName,
                                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                                           ISMS_SubjectCode = c.ISMS_SubjectCode,
                                                           EMSS_SubSubjectName = e.EMSS_SubSubjectName,
                                                           EMSS_SubSubjectCode = e.EMSS_SubSubjectCode,
                                                           EYCESSS_MaxMarks = a.EYCESSS_MaxMarks,
                                                           EYCESSS_MinMarks = a.EYCESSS_MinMarks,
                                                           EYCESSS_MarksEntryMax = a.EYCESSS_MarksEntryMax,
                                                           EYCESSS_ExemptedFlg = a.EYCESSS_ExemptedFlg,
                                                           EYCESSS_ExemptedPer = a.EYCESSS_ExemptedPer,
                                                           EYCESSS_SubSubjectOrder = a.EYCESSS_SubSubjectOrder,
                                                           EYCESSS_ActiveFlg = a.EYCESSS_ActiveFlg,
                                                           EYCESSS_GradesFlg = a.EYCESSS_GradesFlg,
                                                           EYCESSS_AplResultFlg = a.EYCESSS_AplResultFlg,
                                                           EYCESSS_MarksFlg = a.EYCESSS_MarksFlg

                                                       }).Distinct().OrderBy(x => x.EYCESSS_SubSubjectOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public ExamSubjectMappingDTO getalldetailsviewrecords_subsubjssunexam(int id)
        {
            ExamSubjectMappingDTO page = new ExamSubjectMappingDTO();
            try
            {
                page.view_exam_subjects_subsubjects = (from a in _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO
                                                       from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                       from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                                       from d in _examcontext.Exm_Master_GradeDMO
                                                       from e in _examcontext.mastersubsubject
                                                       from f in _examcontext.mastersubexam
                                                       where (a.EYCES_Id == b.EYCES_Id && b.ISMS_Id == c.ISMS_Id
                                                       && c.MI_Id == d.MI_Id && a.EMGR_Id == d.EMGR_Id && c.MI_Id == e.MI_Id
                                                       && e.EMSS_Id == a.EMSS_Id && f.EMSE_Id == a.EMSE_Id && b.EYCES_Id == id)
                                                       select new ExamSubjectMappingDTO
                                                       {
                                                           EYCESSS_Id = a.EYCESSS_Id,
                                                           EYCES_Id = a.EYCES_Id,
                                                           ISMS_Id = b.ISMS_Id,
                                                           EMSS_Id = a.EMSS_Id,
                                                           EMGR_GradeName = d.EMGR_GradeName,
                                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                                           ISMS_SubjectCode = c.ISMS_SubjectCode,
                                                           EMSS_SubSubjectName = e.EMSS_SubSubjectName,
                                                           EMSS_SubSubjectCode = e.EMSS_SubSubjectCode,
                                                           EMSE_SubExamName = f.EMSE_SubExamName,
                                                           EMSE_SubExamCode = f.EMSE_SubExamCode,
                                                           EYCESSS_MaxMarks = a.EYCESSS_MaxMarks,
                                                           EYCESSS_MinMarks = a.EYCESSS_MinMarks,
                                                           EYCESSS_MarksEntryMax = a.EYCESSS_MarksEntryMax,
                                                           EYCESSS_ExemptedFlg = a.EYCESSS_ExemptedFlg,
                                                           EYCESSS_ExemptedPer = a.EYCESSS_ExemptedPer,
                                                           EYCESSS_SubSubjectOrder = a.EYCESSS_SubSubjectOrder,
                                                           EYCESSS_ActiveFlg = a.EYCESSS_ActiveFlg,
                                                           EYCESSS_GradesFlg = a.EYCESSS_GradesFlg,
                                                           EYCESSS_AplResultFlg = a.EYCESSS_AplResultFlg,
                                                           EYCESSS_MarksFlg = a.EYCESSS_MarksFlg
                                                       }).Distinct().OrderBy(x => x.EYCESSS_SubSubjectOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public ExamSubjectMappingDTO delete_records_of_cat_exm(int id)
        {
            ExamSubjectMappingDTO pagert = new ExamSubjectMappingDTO();
            try
            {
                List<Exm_Yearly_Category_ExamsDMO> lorg1 = new List<Exm_Yearly_Category_ExamsDMO>();
                lorg1 = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYCE_Id == id).ToList();
                CreatedDate = lorg1[0].CreatedDate;

                List<Exm_Yrly_Cat_Exams_SubwiseDMO> lorg2 = new List<Exm_Yrly_Cat_Exams_SubwiseDMO>();
                lorg2 = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(t => t.EYCE_Id == id).ToList();

                List<int> subs = new List<int>();
                foreach (Exm_Yrly_Cat_Exams_SubwiseDMO x in lorg2)
                {
                    subs.Add(x.EYCES_Id);
                }

                List<Exm_Yrly_Cat_Exams_Subwise_PTDMO> lorg3_sub_grade = new List<Exm_Yrly_Cat_Exams_Subwise_PTDMO>();
                lorg3_sub_grade = _examcontext.Exm_Yrly_Cat_Exams_Subwise_PTDMO.Where(t => subs.Contains(t.EYCES_Id)).ToList();

                List<Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO> lorg3 = new List<Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO>();
                lorg3 = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubExamsDMO.Where(t => subs.Contains(t.EYCES_Id)).ToList();

                List<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO> lorg4 = new List<Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO>();
                lorg4 = _examcontext.Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDMO.Where(t => subs.Contains(t.EYCES_Id)).ToList();

                if (lorg4.Any())
                {
                    for (int i = 0; lorg4.Count > i; i++)
                    {
                        _examcontext.Remove(lorg4.ElementAt(i));
                    }
                }

                if (lorg3.Any())
                {
                    for (int i = 0; lorg3.Count > i; i++)
                    {
                        _examcontext.Remove(lorg3.ElementAt(i));
                    }
                }

                if (lorg3_sub_grade.Any())
                {
                    for (int i = 0; lorg3_sub_grade.Count > i; i++)
                    {
                        _examcontext.Remove(lorg3_sub_grade.ElementAt(i));
                    }
                }

                if (lorg2.Any())
                {
                    for (int i = 0; lorg2.Count > i; i++)
                    {
                        _examcontext.Remove(lorg2.ElementAt(i));
                    }
                }

                //if (lorg1.Any())
                //{
                //    for (int i = 0; lorg1.Count > i; i++)
                //    {
                //        _examcontext.Remove(lorg1.ElementAt(i));
                //    }
                //}

                if (lorg1.Count > 0 || lorg2.Count > 0 || lorg3_sub_grade.Count > 0 || lorg3.Count > 0 || lorg4.Count > 0)
                {
                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists > 0)
                    {
                        pagert.returnval = true;
                    }
                    else
                    {
                        pagert.returnval = false;
                    }
                }
                else
                {
                    pagert.returnval = true;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }
        public ExamSubjectMappingDTO SetSubjectOrder(ExamSubjectMappingDTO data)
        {
            try
            {
                data.view_exam_subjects = (from a in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                           from b in _examcontext.Exm_Master_CategoryDMO
                                           from c in _examcontext.exammasterDMO
                                           from d in _examcontext.Exm_Yearly_CategoryDMO
                                           from e in _examcontext.Exm_Yearly_Category_ExamsDMO
                                           from f in _examcontext.Exm_Master_GradeDMO
                                           from g in _examcontext.IVRM_School_Master_SubjectsDMO
                                           where (a.EYCE_Id == e.EYCE_Id && e.EYC_Id == d.EYC_Id && d.EMCA_Id == b.EMCA_Id && b.MI_Id == d.MI_Id
                                           && b.MI_Id == f.MI_Id && c.MI_Id == b.MI_Id && a.ISMS_Id == g.ISMS_Id && a.EMGR_Id == f.EMGR_Id && a.EYCE_Id == data.EYCE_Id
                                           && e.EME_Id == c.EME_Id)
                                           select new ExamSubjectMappingDTO
                                           {
                                               EYCES_Id = a.EYCES_Id,
                                               EYCE_Id = a.EYCE_Id,
                                               ISMS_Id = a.ISMS_Id,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               EYCES_SubjectOrder = a.EYCES_SubjectOrder
                                           }).Distinct().OrderBy(x => x.EYCES_SubjectOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamSubjectMappingDTO SaveSubjectOrder(ExamSubjectMappingDTO data)
        {
            try
            {
                if (data.Temp_Subject_Order.Length > 0)
                {
                    var id = 0;
                    foreach (var c in data.Temp_Subject_Order)
                    {
                        id = id + 1;
                        var result = _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO.Single(a => a.EYCES_Id == c.EYCES_Id);
                        result.EYCES_SubjectOrder = id;
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                    }

                    var i = _examcontext.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                data.view_exam_subjects = (from a in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                           from b in _examcontext.Exm_Master_CategoryDMO
                                           from c in _examcontext.exammasterDMO
                                           from d in _examcontext.Exm_Yearly_CategoryDMO
                                           from e in _examcontext.Exm_Yearly_Category_ExamsDMO
                                           from f in _examcontext.Exm_Master_GradeDMO
                                           from g in _examcontext.IVRM_School_Master_SubjectsDMO
                                           where (a.EYCE_Id == e.EYCE_Id && e.EYC_Id == d.EYC_Id && d.EMCA_Id == b.EMCA_Id && b.MI_Id == d.MI_Id
                                           && b.MI_Id == f.MI_Id && c.MI_Id == b.MI_Id && a.ISMS_Id == g.ISMS_Id && a.EMGR_Id == f.EMGR_Id && a.EYCE_Id == data.EYCE_Id
                                           && e.EME_Id == c.EME_Id)
                                           select new ExamSubjectMappingDTO
                                           {
                                               EYCES_Id = a.EYCES_Id,
                                               EYCE_Id = a.EYCE_Id,
                                               ISMS_Id = a.ISMS_Id,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               EYCES_SubjectOrder = a.EYCES_SubjectOrder
                                           }).Distinct().OrderBy(x => x.EYCES_SubjectOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamSubjectMappingDTO deactive_subj_GradeList(ExamSubjectMappingDTO data)
        {
            try
            {
                data.returnval = false;
                var checkresult = _examcontext.Exm_Yrly_Cat_Exams_Subwise_PTDMO.Single(a => a.EYCESPT_Id == data.EYCESPT_Id);
                checkresult.EYCESPT_ActiveFlg = checkresult.EYCESPT_ActiveFlg == true ? false : true;
                checkresult.EYCESPT_UpdatedBy = data.UserId;
                checkresult.EYCESPT_UpdatedDate = DateTime.Now;
                _examcontext.Update(checkresult);
                var i = _examcontext.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }

                data.view_exam_subjects_grade_list = (from a in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                      from b in _examcontext.Exm_Yrly_Cat_Exams_Subwise_PTDMO
                                                      from c in _examcontext.Exm_Master_PaperTypeDMO
                                                      from d in _examcontext.Exm_Master_GradeDMO
                                                      where (a.EYCES_Id == b.EYCES_Id && b.EMPATY_Id == c.EMPATY_Id && b.EMGR_Id == d.EMGR_Id
                                                      && a.EYCES_Id == data.EYCES_Id && b.EYCES_Id == data.EYCES_Id)
                                                      select new ExamSubjectMappingDTO
                                                      {
                                                          EYCES_Id = b.EYCES_Id,
                                                          ISMS_Id = a.ISMS_Id,
                                                          EMGR_Id = b.EMGR_Id,
                                                          EYCESPT_Id = b.EYCESPT_Id,
                                                          EMPATY_Id = b.EMPATY_Id,
                                                          EYCESPT_ActiveFlg = b.EYCESPT_ActiveFlg,
                                                          EMPATY_PaperTypeName = c.EMPATY_PaperTypeName,
                                                          EMGR_GradeName = d.EMGR_GradeName,
                                                      }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}