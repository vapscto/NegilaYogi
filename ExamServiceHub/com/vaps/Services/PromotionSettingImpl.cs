
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
using Microsoft.EntityFrameworkCore;

namespace ExamServiceHub.com.vaps.Services
{
    public class PromotionSettingImpl : Interfaces.PromotionSettingInterface
    {
        private static ConcurrentDictionary<string, PromotionSettingDTO> _login =
         new ConcurrentDictionary<string, PromotionSettingDTO>();

        private ExamContext _examcontext;
        ILogger<PromotionSettingImpl> _acdimpl;
        public PromotionSettingImpl(ExamContext masterexamContext, ILogger<PromotionSettingImpl> _acd)
        {
            _examcontext = masterexamContext;
            _acdimpl = _acd;
        }
        public PromotionSettingDTO Getdetails(PromotionSettingDTO data)//int IVRMM_Id
        {
            // PromotionSettingDTO TTMC = new PromotionSettingDTO();
            try
            {

                data.exm_prom_groups = (from a in _examcontext.Exm_M_PromotionDMO
                                        from b in _examcontext.Exm_M_Promotion_SubjectsDMO
                                        from c in _examcontext.Exm_M_Prom_Subj_GroupDMO
                                        where (a.MI_Id == data.MI_Id && a.EMP_Id == b.EMP_Id && b.EMPS_Id == c.EMPS_Id)
                                        select c).Distinct().ToArray();



                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _examcontext.AcademicYear.Where(y => y.MI_Id == data.MI_Id && y.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = year.Distinct().ToArray();

                //List<Exm_Master_CategoryDMO> categories = new List<Exm_Master_CategoryDMO>();
                //categories = _examcontext.Exm_Master_CategoryDMO.Where(c => c.MI_Id == data.MI_Id && c.EMCA_ActiveFlag == true).ToList();
                //data.categorylist = categories.Distinct().ToArray();

                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.EYC_ActiveFlg == true)
                                     select new PromotionSettingDTO
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


                data.promotion_details = (from a in _examcontext.AcademicYear
                                          from b in _examcontext.Exm_Master_CategoryDMO
                                          from d in _examcontext.Exm_Yearly_CategoryDMO
                                          from e in _examcontext.Exm_M_PromotionDMO
                                          from f in _examcontext.Exm_Master_GradeDMO
                                          where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && f.MI_Id == a.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.EMCA_Id == b.EMCA_Id && d.EYC_Id == e.EYC_Id && e.EMGR_Id == f.EMGR_Id)
                                          select new PromotionSettingDTO
                                          {
                                              EMP_Id = e.EMP_Id,
                                              EYC_Id = e.EYC_Id,
                                              EMCA_Id = d.EMCA_Id,
                                              ASMAY_Id = d.ASMAY_Id,
                                              EMGR_Id = e.EMGR_Id,
                                              ASMAY_Year = a.ASMAY_Year,
                                              EMCA_CategoryName = b.EMCA_CategoryName,
                                              EMGR_GradeName = f.EMGR_GradeName,
                                              EMP_PassToIndSubjectFlg = e.EMP_PassToIndSubjectFlg,
                                              EMP_PassToOverallFlag = e.EMP_PassToOverallFlag,
                                              EMP_MarksPerFlg = e.EMP_MarksPerFlg,
                                              EMP_ActiveFlag = e.EMP_ActiveFlag,
                                              ASMAY_Order = a.ASMAY_Order,
                                              EMP_BestOf = e.EMP_BestOf,
                                              EMP_BestOfApplicableFlg = e.EMP_BestOfApplicableFlg
                                          }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public PromotionSettingDTO savedetails(PromotionSettingDTO _category)
        {
            Exm_M_PromotionDMO objpge = Mapper.Map<Exm_M_PromotionDMO>(_category);
            try
            {
                PromotionSettingDTO req = new PromotionSettingDTO();
                req.returnval = true;

                if (objpge.EMP_Id > 0)
                {
                    req = delete_records_of_pro_cat(objpge.EMP_Id);
                }

                if (req.returnval == true)
                {
                    //Exm_M_PromotionDMO objpge1 = Mapper.Map<Exm_M_PromotionDMO>(_category);
                    var result = _examcontext.Exm_M_PromotionDMO.Where(t => t.EYC_Id == _category.EYC_Id && t.MI_Id == _category.MI_Id).ToList();
                    if (result.Count() == 0)
                    {
                        Exm_M_PromotionDMO objpge11 = new Exm_M_PromotionDMO();
                        objpge11.EYC_Id = _category.EYC_Id;
                        objpge11.MI_Id = _category.MI_Id;
                        objpge11.EMGR_Id = _category.EMGR_Id;
                        objpge11.EMP_ActiveFlag = true;                       
                        objpge11.EMP_BestOfApplicableFlg = _category.EMP_BestOfApplicableFlg;
                        objpge11.EMP_BestOf = _category.EMP_BestOf;
                        objpge11.EMP_PassToIndSubjectFlg = _category.EMP_PassToIndSubjectFlg;
                        objpge11.EMP_PassToOverallFlag = _category.EMP_PassToOverallFlag;
                        objpge11.EMP_MarksPerFlg = _category.EMP_MarksPerFlg;
                        objpge11.CreatedDate = DateTime.Now;
                        objpge11.UpdatedDate = DateTime.Now;
                        _examcontext.Add(objpge11);
                        _category.EMP_Id = objpge11.EMP_Id;

                        if (objpge11.EMP_MarksPerFlg != "F")
                        {
                            for (int j = 0; j < _category.pro_subjects_list.Length; j++)
                            {
                                //Exm_M_Promotion_SubjectsDMO obj_subs = Mapper.Map<Exm_M_Promotion_SubjectsDMO>(_category.pro_subjects_list[j]);
                                Exm_M_Promotion_SubjectsDMO obj_subss = new Exm_M_Promotion_SubjectsDMO();
                                obj_subss.EMP_Id = objpge11.EMP_Id;
                                obj_subss.ISMS_Id = _category.pro_subjects_list[j].ISMS_Id;
                                obj_subss.EMGR_Id = _category.pro_subjects_list[j].EMGR_Id;
                                obj_subss.EMPS_MaxMarks = _category.pro_subjects_list[j].EMPS_MaxMarks;
                                obj_subss.EMPS_MinMarks = _category.pro_subjects_list[j].EMPS_MinMarks;
                                obj_subss.EMPS_ConvertForMarks = _category.pro_subjects_list[j].EMPS_ConvertForMarks;
                                obj_subss.EMPS_AppToResultFlg = _category.pro_subjects_list[j].EMPS_AppToResultFlg;
                                obj_subss.EMPS_SubjOrder = _category.pro_subjects_list[j].EMPS_SubjOrder;
                                obj_subss.EMPS_ActiveFlag = true;
                                obj_subss.CreatedDate = DateTime.Now;
                                obj_subss.UpdatedDate = DateTime.Now;
                                _examcontext.Add(obj_subss);
                                _category.EMPS_Id = obj_subss.EMPS_Id;
                                if (objpge11.EMP_MarksPerFlg != "T")
                                {
                                    for (int x = 0; x < _category.pro_subjects_list[j].pro_exams_group_list.Length; x++)
                                    {
                                        //Exm_M_Prom_Subj_GroupDMO obj_sub_grps = Mapper.Map<Exm_M_Prom_Subj_GroupDMO>(_category.pro_subjects_list[j].pro_exams_group_list[x]);

                                        Exm_M_Prom_Subj_GroupDMO obj_sub_grpss = new Exm_M_Prom_Subj_GroupDMO();
                                        obj_sub_grpss.EMPS_Id = obj_subss.EMPS_Id;
                                        obj_sub_grpss.EMPSG_GroupName = _category.pro_subjects_list[j].pro_exams_group_list[x].EMPSG_GroupName;
                                        obj_sub_grpss.EMPSG_DisplayName = _category.pro_subjects_list[j].pro_exams_group_list[x].EMPSG_DisplayName;
                                        obj_sub_grpss.EMPSG_PercentValue = _category.pro_subjects_list[j].pro_exams_group_list[x].EMPSG_PercentValue;
                                        obj_sub_grpss.EMPSG_MarksValue = _category.pro_subjects_list[j].pro_exams_group_list[x].EMPSG_MarksValue;
                                        obj_sub_grpss.EMPSG_MaxOff = _category.pro_subjects_list[j].pro_exams_group_list[x].EMPSG_MaxOff;
                                        obj_sub_grpss.EMPSG_BestOff = _category.pro_subjects_list[j].pro_exams_group_list[x].EMPSG_BestOff;
                                        obj_sub_grpss.EMPSG_Order = _category.pro_subjects_list[j].pro_exams_group_list[x].EMPSG_Order;
                                        obj_sub_grpss.EMPSG_RoundOffFlag = _category.pro_subjects_list[j].pro_exams_group_list[x].EMPSG_RoundOffFlag;
                                        obj_sub_grpss.EMPSG_ActiveFlag = true;
                                        obj_sub_grpss.CreatedDate = DateTime.Now;
                                        obj_sub_grpss.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_sub_grpss);
                                        _category.EMPSG_Id = obj_sub_grpss.EMPSG_Id;

                                        for (int y = 0; y < _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master.Length; y++)
                                        {
                                            Exm_M_Prom_Subj_Group_ExamsDMO obj_sub_grps_exms = new Exm_M_Prom_Subj_Group_ExamsDMO();
                                            obj_sub_grps_exms.EMPSG_Id = obj_sub_grpss.EMPSG_Id;
                                            obj_sub_grps_exms.EME_Id = _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EME_Id;
                                            obj_sub_grps_exms.EMPSGE_ForMaxMarkrs = _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EMPSGE_ForMaxMarkrs;
                                            obj_sub_grps_exms.EMPSGE_ConvertionReqOrNot = _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EMPSGE_ConvertionReqOrNot;
                                            obj_sub_grps_exms.EMPSGE_ActiveFlg = true;
                                            obj_sub_grps_exms.CreatedDate = DateTime.Now;
                                            obj_sub_grps_exms.UpdatedDate = DateTime.Now;

                                            _examcontext.Add(obj_sub_grps_exms);
                                        }
                                    }
                                }
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
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                }
                else
                {
                    _category.returnval = false;
                }

                _category.promotion_details = (from a in _examcontext.AcademicYear
                                               from b in _examcontext.Exm_Master_CategoryDMO
                                               from d in _examcontext.Exm_Yearly_CategoryDMO
                                               from e in _examcontext.Exm_M_PromotionDMO
                                               from f in _examcontext.Exm_Master_GradeDMO
                                               where (a.MI_Id == _category.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && f.MI_Id == a.MI_Id
                                               && a.ASMAY_Id == d.ASMAY_Id && d.EMCA_Id == b.EMCA_Id && d.EYC_Id == e.EYC_Id && e.EMGR_Id == f.EMGR_Id)
                                               select new PromotionSettingDTO
                                               {
                                                   EMP_Id = e.EMP_Id,
                                                   EYC_Id = e.EYC_Id,
                                                   EMCA_Id = d.EMCA_Id,
                                                   ASMAY_Id = d.ASMAY_Id,
                                                   EMGR_Id = e.EMGR_Id,
                                                   ASMAY_Year = a.ASMAY_Year,
                                                   EMCA_CategoryName = b.EMCA_CategoryName,
                                                   EMGR_GradeName = f.EMGR_GradeName,
                                                   EMP_PassToIndSubjectFlg = e.EMP_PassToIndSubjectFlg,
                                                   EMP_PassToOverallFlag = e.EMP_PassToOverallFlag,
                                                   EMP_MarksPerFlg = e.EMP_MarksPerFlg,
                                                   EMP_ActiveFlag = e.EMP_ActiveFlag,
                                                   ASMAY_Order = a.ASMAY_Order,
                                                   EMP_BestOf = e.EMP_BestOf,
                                                   EMP_BestOfApplicableFlg = e.EMP_BestOfApplicableFlg
                                               }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return _category;
        }
        public PromotionSettingDTO editdetails(int id)
        {
            PromotionSettingDTO page = new PromotionSettingDTO();
            try
            {
                page.edit_m_pro = (from a in _examcontext.Exm_M_PromotionDMO
                                   from b in _examcontext.Exm_Yearly_CategoryDMO
                                   where (a.EMP_Id == id && a.EYC_Id == b.EYC_Id)
                                   select new PromotionSettingDTO
                                   {
                                       ASMAY_Id = b.ASMAY_Id,
                                       EYC_Id = a.EYC_Id,
                                       //EMCA_Id = b.EMCA_Id,
                                       //  EME_Id = a.EME_Id,
                                       EMGR_Id = a.EMGR_Id,
                                       EMP_PassToIndSubjectFlg = a.EMP_PassToIndSubjectFlg,
                                       EMP_PassToOverallFlag = a.EMP_PassToOverallFlag,
                                       EMP_MarksPerFlg = a.EMP_MarksPerFlg,
                                       EMP_Id = a.EMP_Id,
                                       EMP_ActiveFlag = a.EMP_ActiveFlag,
                                       EMP_BestOfApplicableFlg = a.EMP_BestOfApplicableFlg,
                                       EMP_BestOf = a.EMP_BestOf

                                   }).Distinct().ToArray();


                page.edit_m_pro_subs = _examcontext.Exm_M_Promotion_SubjectsDMO.Where(ce => ce.EMP_Id == id).Distinct().ToArray();

                List<int> subs = new List<int>();
                foreach (Exm_M_Promotion_SubjectsDMO x in page.edit_m_pro_subs)
                {
                    subs.Add(x.EMPS_Id);
                }

                page.edit_m_pro_subs_grps = _examcontext.Exm_M_Prom_Subj_GroupDMO.Where(e => subs.Contains(e.EMPS_Id)).Distinct().ToArray();
                List<int> subs_grps = new List<int>();
                foreach (Exm_M_Prom_Subj_GroupDMO x in page.edit_m_pro_subs_grps)
                {
                    subs_grps.Add(x.EMPSG_Id);
                }
                page.edit_m_pro_subs_grps_exms = (from a in _examcontext.Exm_M_Prom_Subj_Group_ExamsDMO
                                                  from b in _examcontext.masterexam
                                                  where (a.EME_Id == b.EME_Id && subs_grps.Contains(a.EMPSG_Id))
                                                  select new PromotionSettingDTO
                                                  {
                                                      EMPSGE_Id = a.EMPSGE_Id,
                                                      EMPSG_Id = a.EMPSG_Id,
                                                      EME_Id = a.EME_Id,
                                                      EMPSGE_ActiveFlg = a.EMPSGE_ActiveFlg,
                                                      EMPSGE_ForMaxMarkrs = a.EMPSGE_ForMaxMarkrs,
                                                      EMPSGE_ConvertionReqOrNot = a.EMPSGE_ConvertionReqOrNot,
                                                      EME_ExamName = b.EME_ExamName

                                                  }).Distinct().ToArray();


                //check for promotion calcution marks
                var EYC_Id = _examcontext.Exm_M_PromotionDMO.Single(t => t.EMP_Id == id).EYC_Id;
                var EMCA_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == EYC_Id).EMCA_Id;
                var ASMAY_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == EYC_Id).ASMAY_Id;
                var MI_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == EYC_Id).MI_Id;

                var count = (from a in _examcontext.Exm_Category_ClassDMO
                             from b in _examcontext.Exm_Stu_MP_Promo_SubjectwiseDMO
                             where (a.MI_Id == MI_Id && a.ASMAY_Id == ASMAY_Id && a.EMCA_Id == EMCA_Id && b.MI_Id == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id && b.ASMS_Id == a.ASMS_Id)
                             select b.AMST_Id).Distinct().ToList().Count();
                if (count == 0)
                {
                    page.Calculated_Flag = false;
                }
                else if (count > 0)
                {
                    page.Calculated_Flag = true;
                }
                //
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return page;
        }
        public PromotionSettingDTO get_category(PromotionSettingDTO data)
        {
            // PromotionSettingDTO editlt = new PromotionSettingDTO();
            try
            {
                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_ActiveFlg == true)
                                     select new PromotionSettingDTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName,
                                         EYC_Id = b.EYC_Id,

                                     }).Distinct().ToArray();


                var pro_cats = _examcontext.Exm_M_PromotionDMO.Where(t => t.MI_Id == data.MI_Id).Select(t => t.EYC_Id).Distinct().ToList();
                if (pro_cats.Count > 0)
                {

                    if (data.EMP_Id > 0)
                    {
                        var eyc_id = _examcontext.Exm_M_PromotionDMO.Single(t => t.EMP_Id == data.EMP_Id && t.MI_Id == data.MI_Id).EYC_Id;
                        pro_cats.Remove(eyc_id);
                    }
                    data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                         from b in _examcontext.Exm_Yearly_CategoryDMO
                                         where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_ActiveFlg == true && !pro_cats.Contains(b.EYC_Id))
                                         select new PromotionSettingDTO
                                         {
                                             EMCA_Id = a.EMCA_Id,
                                             EMCA_CategoryName = a.EMCA_CategoryName,
                                             EYC_Id = b.EYC_Id,

                                         }).Distinct().ToArray();
                }


                //var pro_cats = _examcontext.Exm_M_PromotionDMO.Where(t => t.MI_Id == data.MI_Id).Select(t => t.EYC_Id).Distinct().ToList();
                //if(pro_cats.Count>0)
                //{
                //    data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                //                         from b in _examcontext.Exm_Yearly_CategoryDMO
                //                         where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_ActiveFlg == true && !pro_cats.Contains(b.EYC_Id))
                //                         select new PromotionSettingDTO
                //                         {
                //                             EMCA_Id = a.EMCA_Id,
                //                             EMCA_CategoryName = a.EMCA_CategoryName,
                //                             EYC_Id = b.EYC_Id,

                //                         }).Distinct().ToArray();
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public PromotionSettingDTO get_subjects(PromotionSettingDTO data)
        {
            // PromotionSettingDTO editlt = new PromotionSettingDTO();
            try
            {
                data.subjectlist = (from a in _examcontext.Exm_Yearly_Category_GroupDMO
                                    from b in _examcontext.Exm_Yearly_Category_Group_SubjectsDMO
                                    from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                    where (c.MI_Id == data.MI_Id && a.EYC_Id == data.EYC_Id && a.EYCG_Id == b.EYCG_Id && b.ISMS_Id == c.ISMS_Id && b.EYCGS_ActiveFlg == true && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1 && a.EYCG_ActiveFlg == true)
                                    select new PromotionSettingDTO
                                    {

                                        ISMS_Id = c.ISMS_Id,
                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                        ISMS_SubjectCode = c.ISMS_SubjectCode,
                                        ISMS_Max_Marks = c.ISMS_Max_Marks,
                                        ISMS_Min_Marks = c.ISMS_Min_Marks,
                                        ISMS_OrderFlag = c.ISMS_OrderFlag,
                                        //EYCES_MaxMarks = c.ISMS_Max_Marks,
                                        //EYCES_MinMarks =c.ISMS_Min_Marks,
                                    }).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                //data.examlist = (from a in _examcontext.Exm_Yearly_Category_ExamsDMO
                //                 from b in _examcontext.masterexam
                //                 where (b.MI_Id == data.MI_Id && a.EME_Id == b.EME_Id && a.EYC_Id == data.EYC_Id && b.EME_ActiveFlag == true)
                //                 select new exammasterDMO
                //                 {
                //                     EME_Id=b.EME_Id,
                //                     MI_Id=b.MI_Id,
                //                     EME_ExamName=b.EME_ExamName,
                //                     EME_ExamCode=b.EME_ExamCode,
                //                     EME_ExamOrder=b.EME_ExamOrder,
                //                     EME_FinalExamFlag=b.EME_FinalExamFlag,
                //                     EME_ActiveFlag=b.EME_ActiveFlag,
                //                     CreatedDate=b.CreatedDate,
                //                     UpdatedDate=b.UpdatedDate,
                //                 }).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();
                data.examlist = (from a in _examcontext.Exm_Yearly_Category_ExamsDMO
                                 from b in _examcontext.masterexam
                                 where (b.MI_Id == data.MI_Id && a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && a.EYC_Id == data.EYC_Id && b.EME_ActiveFlag == true)
                                 select b).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public PromotionSettingDTO deactivate(PromotionSettingDTO data)
        {
            Exm_M_PromotionDMO pge = Mapper.Map<Exm_M_PromotionDMO>(data);
            if (pge.EMP_Id > 0)
            {
                var result = _examcontext.Exm_M_PromotionDMO.Single(t => t.EMP_Id == pge.EMP_Id);
                if (result.EMP_ActiveFlag == true)
                {
                    result.EMP_ActiveFlag = false;
                }
                else
                {
                    result.EMP_ActiveFlag = true;
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
        public PromotionSettingDTO deactivate_sub(PromotionSettingDTO data)
        {
            Exm_M_Promotion_SubjectsDMO pge = Mapper.Map<Exm_M_Promotion_SubjectsDMO>(data);
            if (pge.EMPS_Id > 0)
            {
                var result = _examcontext.Exm_M_Promotion_SubjectsDMO.Single(t => t.EMPS_Id == pge.EMPS_Id);
                if (result.EMPS_ActiveFlag == true)
                {
                    result.EMPS_ActiveFlag = false;
                }
                else
                {
                    result.EMPS_ActiveFlag = true;
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
        public PromotionSettingDTO deactive_sub_grp_exm(PromotionSettingDTO data)
        {
            Exm_M_Prom_Subj_Group_ExamsDMO pge = Mapper.Map<Exm_M_Prom_Subj_Group_ExamsDMO>(data);
            if (pge.EMPSGE_Id > 0)
            {
                var result = _examcontext.Exm_M_Prom_Subj_Group_ExamsDMO.Single(t => t.EMPSGE_Id == pge.EMPSGE_Id);
                if (result.EMPSGE_ActiveFlg == true)
                {
                    result.EMPSGE_ActiveFlg = false;
                }
                else
                {
                    result.EMPSGE_ActiveFlg = true;
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
        public PromotionSettingDTO deactive_sub_grp(PromotionSettingDTO data)
        {
            Exm_M_Prom_Subj_GroupDMO pge = Mapper.Map<Exm_M_Prom_Subj_GroupDMO>(data);
            if (pge.EMPSG_Id > 0)
            {
                var result = _examcontext.Exm_M_Prom_Subj_GroupDMO.Single(t => t.EMPSG_Id == pge.EMPSG_Id);
                if (result.EMPSG_ActiveFlag == true)
                {
                    result.EMPSG_ActiveFlag = false;
                }
                else
                {
                    result.EMPSG_ActiveFlag = true;
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
        public PromotionSettingDTO getalldetailsviewrecords(int id)
        {
            PromotionSettingDTO page = new PromotionSettingDTO();
            try
            {
                page.view_prom_subjects = (from a in _examcontext.Exm_M_Promotion_SubjectsDMO
                                           from b in _examcontext.Exm_Master_CategoryDMO
                                               //  from c in _examcontext.exammasterDMO
                                           from d in _examcontext.Exm_Yearly_CategoryDMO
                                           from e in _examcontext.Exm_M_PromotionDMO
                                           from f in _examcontext.Exm_Master_GradeDMO
                                           from g in _examcontext.IVRM_School_Master_SubjectsDMO
                                           where (a.EMP_Id == e.EMP_Id && e.EYC_Id == d.EYC_Id && d.EMCA_Id == b.EMCA_Id && b.MI_Id == d.MI_Id && b.MI_Id == f.MI_Id && a.ISMS_Id == g.ISMS_Id && a.EMGR_Id == f.EMGR_Id && a.EMP_Id == id)
                                           select new PromotionSettingDTO
                                           {
                                               EMPS_Id = a.EMPS_Id,
                                               EMP_Id = a.EMP_Id,
                                               ISMS_Id = a.ISMS_Id,
                                               EMCA_CategoryName = b.EMCA_CategoryName,
                                               //  EME_ExamName = c.EME_ExamName,
                                               EMGR_GradeName = f.EMGR_GradeName,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               EMPS_MaxMarks = a.EMPS_MaxMarks,
                                               EMPS_MinMarks = a.EMPS_MinMarks,
                                               EMPS_ConvertForMarks = a.EMPS_ConvertForMarks,
                                               EMPS_AppToResultFlg = a.EMPS_AppToResultFlg,
                                               EMPS_ActiveFlag = a.EMPS_ActiveFlag,
                                               EMPS_SubjOrder = a.EMPS_SubjOrder
                                           }).Distinct().OrderBy(a => a.EMPS_SubjOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public PromotionSettingDTO getalldetailsviewrecords_sub_grp_exms(int id)
        {
            PromotionSettingDTO page = new PromotionSettingDTO();
            try
            {
                page.view_exam_subjects_subgroup_exms = (from a in _examcontext.Exm_M_Prom_Subj_Group_ExamsDMO
                                                         from b in _examcontext.Exm_M_Prom_Subj_GroupDMO
                                                         from c in _examcontext.masterexam
                                                             // from d in _examcontext.Exm_Master_GradeDMO
                                                             //  from e in _examcontext.mastersubexam
                                                         where (a.EMPSG_Id == b.EMPSG_Id && b.EMPSG_Id == id && a.EME_Id == c.EME_Id)
                                                         select new PromotionSettingDTO
                                                         {
                                                             EMPSGE_Id = a.EMPSGE_Id,
                                                             EMPSG_Id = a.EMPSG_Id,
                                                             EME_Id = a.EME_Id,
                                                             EME_ExamName = c.EME_ExamName,
                                                             EMPSG_GroupName = b.EMPSG_GroupName,
                                                             EMPSG_DisplayName = b.EMPSG_DisplayName,
                                                             EME_ExamCode = c.EME_ExamCode,
                                                             EMPSGE_ActiveFlg = a.EMPSGE_ActiveFlg,
                                                             EMPSGE_ForMaxMarkrs = a.EMPSGE_ForMaxMarkrs,
                                                             EMPSGE_ConvertionReqOrNot = a.EMPSGE_ConvertionReqOrNot,

                                                         }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public PromotionSettingDTO getalldetailsviewrecords_subgrps(int id)
        {
            PromotionSettingDTO page = new PromotionSettingDTO();
            try
            {
                page.view_exam_subjects_subgroups = (from a in _examcontext.Exm_M_Prom_Subj_GroupDMO
                                                     from b in _examcontext.Exm_M_Promotion_SubjectsDMO
                                                     from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                                         //  from d in _examcontext.Exm_Master_GradeDMO
                                                         // from e in _examcontext.mastersubsubject
                                                     where (a.EMPS_Id == b.EMPS_Id && b.EMPS_Id == id && b.ISMS_Id == c.ISMS_Id)
                                                     select new PromotionSettingDTO
                                                     {
                                                         EMPSG_Id = a.EMPSG_Id,
                                                         EMPS_Id = a.EMPS_Id,
                                                         ISMS_Id = b.ISMS_Id,
                                                         ISMS_SubjectName = c.ISMS_SubjectName,
                                                         ISMS_SubjectCode = c.ISMS_SubjectCode,
                                                         EMPSG_GroupName = a.EMPSG_GroupName,
                                                         EMPSG_DisplayName = a.EMPSG_DisplayName,
                                                         EMPSG_PercentValue = a.EMPSG_PercentValue,
                                                         EMPSG_MarksValue = a.EMPSG_MarksValue,
                                                         EMPSG_MaxOff = a.EMPSG_MaxOff,
                                                         EMPSG_BestOff = a.EMPSG_BestOff,
                                                         EMPSG_ActiveFlag = a.EMPSG_ActiveFlag,
                                                         EMPSG_RoundOffFlag = a.EMPSG_RoundOffFlag,
                                                         EMPSG_Order = a.EMPSG_Order
                                                     }).Distinct().OrderBy(a => a.EMPSG_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public PromotionSettingDTO delete_records_of_pro_cat(int id)
        {
            PromotionSettingDTO pagert = new PromotionSettingDTO();
            try
            {
                var EYC_Id = _examcontext.Exm_M_PromotionDMO.Single(t => t.EMP_Id == id).EYC_Id;
                var EMCA_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == EYC_Id).EMCA_Id;
                var ASMAY_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == EYC_Id).ASMAY_Id;
                var MI_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == EYC_Id).MI_Id;

                var outputval1 = _examcontext.Database.ExecuteSqlCommand("Exm_Promotion_Deletion_Modify  @p0,@p1,@p2", MI_Id, ASMAY_Id, EYC_Id);

                if (outputval1 >= 1)
                {
                    pagert.returnval = true;
                }
                else
                {
                    pagert.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }
        public PromotionSettingDTO GetSubjectExamMaks(PromotionSettingDTO data)
        {
            try
            {

                data.GetYearlyExamSubjectMarks = (from a in _examcontext.Exm_Yearly_Category_ExamsDMO
                                                  from b in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                  from c in _examcontext.Exm_Yearly_CategoryDMO
                                                  where (a.EYCE_Id == b.EYCE_Id && c.EYC_Id == a.EYC_Id && b.ISMS_Id == data.ISMS_Id && a.EYC_Id == data.EYC_Id
                                                  && a.EYCE_ActiveFlg == true && b.EYCES_ActiveFlg == true)
                                                  select new PromotionSettingDTO
                                                  {
                                                      EME_Id = a.EME_Id,
                                                      EYCES_MaxMarks = b.EYCES_MaxMarks
                                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PromotionSettingDTO SetSubjectOrder(PromotionSettingDTO data)
        {
            try
            {
                data.view_prom_subjects = (from a in _examcontext.Exm_M_Promotion_SubjectsDMO
                                           from b in _examcontext.Exm_Master_CategoryDMO
                                           from d in _examcontext.Exm_Yearly_CategoryDMO
                                           from e in _examcontext.Exm_M_PromotionDMO
                                           from f in _examcontext.Exm_Master_GradeDMO
                                           from g in _examcontext.IVRM_School_Master_SubjectsDMO
                                           where (a.EMP_Id == e.EMP_Id && e.EYC_Id == d.EYC_Id && d.EMCA_Id == b.EMCA_Id && b.MI_Id == d.MI_Id && b.MI_Id == f.MI_Id
                                           && a.ISMS_Id == g.ISMS_Id && a.EMGR_Id == f.EMGR_Id && a.EMP_Id == data.EMP_Id)
                                           select new PromotionSettingDTO
                                           {
                                               EMPS_Id = a.EMPS_Id,
                                               EMP_Id = a.EMP_Id,
                                               ISMS_Id = a.ISMS_Id,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               EMPS_SubjOrder = a.EMPS_SubjOrder
                                           }).Distinct().OrderBy(a => a.EMPS_SubjOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PromotionSettingDTO SaveSubjectOrder(PromotionSettingDTO data)
        {
            try
            {

                int id = 0;
                for (int i = 0; i < data.subject_list_temp.Length; i++)
                {
                    var reult = _examcontext.Exm_M_Promotion_SubjectsDMO.Single(t => t.EMPS_Id == data.subject_list_temp[i].EMPS_Id);
                    id = id + 1;
                    reult.EMPS_SubjOrder = id;
                    reult.UpdatedDate = DateTime.UtcNow;
                    _examcontext.Update(reult);
                }

                var flag = _examcontext.SaveChanges();
                if (flag > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

                data.view_prom_subjects = (from a in _examcontext.Exm_M_Promotion_SubjectsDMO
                                           from b in _examcontext.Exm_Master_CategoryDMO
                                           from d in _examcontext.Exm_Yearly_CategoryDMO
                                           from e in _examcontext.Exm_M_PromotionDMO
                                           from f in _examcontext.Exm_Master_GradeDMO
                                           from g in _examcontext.IVRM_School_Master_SubjectsDMO
                                           where (a.EMP_Id == e.EMP_Id && e.EYC_Id == d.EYC_Id && d.EMCA_Id == b.EMCA_Id && b.MI_Id == d.MI_Id && b.MI_Id == f.MI_Id
                                           && a.ISMS_Id == g.ISMS_Id && a.EMGR_Id == f.EMGR_Id && a.EMP_Id == data.EMP_Id)
                                           select new PromotionSettingDTO
                                           {
                                               EMPS_Id = a.EMPS_Id,
                                               EMP_Id = a.EMP_Id,
                                               ISMS_Id = a.ISMS_Id,
                                               ISMS_SubjectName = g.ISMS_SubjectName,
                                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                                               EMPS_SubjOrder = a.EMPS_SubjOrder
                                           }).Distinct().OrderBy(a => a.EMPS_SubjOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Dont Use
        public PromotionSettingDTO savedetailsworking(PromotionSettingDTO _category)
        {
            Exm_M_PromotionDMO objpge = Mapper.Map<Exm_M_PromotionDMO>(_category);
            try
            {
                if (objpge.EMP_Id > 0)
                {
                    Exm_M_PromotionDMO objpge1 = Mapper.Map<Exm_M_PromotionDMO>(_category);
                    var result = _examcontext.Exm_M_PromotionDMO.Where(t => t.EYC_Id == objpge1.EYC_Id && t.MI_Id == objpge1.MI_Id && t.EMP_Id != objpge1.EMP_Id).ToList();

                    if (result.Count() == 0)
                    {
                        PromotionSettingDTO req = new PromotionSettingDTO();
                        var resulted = _examcontext.Exm_M_PromotionDMO.Single(t => t.EMP_Id == _category.EMP_Id && t.MI_Id == _category.MI_Id);
                        var total_flag = resulted.EMP_MarksPerFlg;
                        resulted.MI_Id = _category.MI_Id;
                        resulted.EYC_Id = _category.EYC_Id;
                        resulted.EMGR_Id = _category.EMGR_Id;
                        resulted.EMP_PassToIndSubjectFlg = _category.EMP_PassToIndSubjectFlg;
                        resulted.EMP_PassToOverallFlag = _category.EMP_PassToOverallFlag;
                        resulted.EMP_MarksPerFlg = _category.EMP_MarksPerFlg;
                        resulted.EMP_ActiveFlag = true;
                        resulted.UpdatedDate = DateTime.Now;
                        resulted.EMP_BestOfApplicableFlg = _category.EMP_BestOfApplicableFlg;
                        resulted.EMP_BestOf = _category.EMP_BestOf;

                        if (total_flag == "F")
                        {
                            req.returnval = true;
                        }
                        else if (total_flag != "F")
                        {
                            req = delete_records_of_pro_cat(objpge.EMP_Id);
                        }

                        if (req.returnval == true)
                        {
                            _examcontext.Update(resulted);
                            if (_category.EMP_MarksPerFlg != "F")
                            {
                                for (int j = 0; j < _category.pro_subjects_list.Length; j++)
                                {
                                    Exm_M_Promotion_SubjectsDMO obj_subs = Mapper.Map<Exm_M_Promotion_SubjectsDMO>(_category.pro_subjects_list[j]);
                                    obj_subs.EMP_Id = objpge1.EMP_Id;
                                    obj_subs.EMPS_ActiveFlag = true;
                                    // obj_subs.CreatedDate = DateTime.Now;
                                    obj_subs.CreatedDate = resulted.CreatedDate;
                                    obj_subs.UpdatedDate = DateTime.Now;

                                    _examcontext.Add(obj_subs);
                                    _category.EMPS_Id = obj_subs.EMPS_Id;
                                    if (_category.EMP_MarksPerFlg != "T")
                                    {
                                        for (int x = 0; x < _category.pro_exams_group_list.Length; x++)
                                        {
                                            Exm_M_Prom_Subj_GroupDMO obj_sub_grps = Mapper.Map<Exm_M_Prom_Subj_GroupDMO>(_category.pro_exams_group_list[x]);
                                            obj_sub_grps.EMPS_Id = obj_subs.EMPS_Id;
                                            obj_sub_grps.EMPSG_ActiveFlag = true;
                                            // obj_sub_grps.CreatedDate = DateTime.Now;
                                            obj_sub_grps.CreatedDate = resulted.CreatedDate;
                                            obj_sub_grps.UpdatedDate = DateTime.Now;

                                            _examcontext.Add(obj_sub_grps);
                                            _category.EMPSG_Id = obj_sub_grps.EMPSG_Id;

                                            for (int y = 0; y < _category.pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master.Length; y++)
                                            {
                                                Exm_M_Prom_Subj_Group_ExamsDMO obj_sub_grps_exms = new Exm_M_Prom_Subj_Group_ExamsDMO();
                                                obj_sub_grps_exms.EMPSG_Id = obj_sub_grps.EMPSG_Id;
                                                obj_sub_grps_exms.EME_Id = _category.pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EME_Id;
                                                obj_sub_grps_exms.EMPSGE_ForMaxMarkrs = _category.pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EMPSGE_ForMaxMarkrs;
                                                obj_sub_grps_exms.EMPSGE_ActiveFlg = true;
                                                // obj_sub_grps_exms.CreatedDate = DateTime.Now;
                                                obj_sub_grps_exms.CreatedDate = resulted.CreatedDate;
                                                obj_sub_grps_exms.UpdatedDate = DateTime.Now;

                                                _examcontext.Add(obj_sub_grps_exms);
                                            }
                                        }
                                    }
                                }
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
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                }
                else
                {
                    Exm_M_PromotionDMO objpge1 = Mapper.Map<Exm_M_PromotionDMO>(_category);
                    var result = _examcontext.Exm_M_PromotionDMO.Where(t => t.EYC_Id == objpge1.EYC_Id && t.MI_Id == objpge1.MI_Id).ToList();
                    if (result.Count() == 0)
                    {
                        objpge1.EMP_ActiveFlag = true;
                        objpge1.CreatedDate = DateTime.Now;
                        objpge1.UpdatedDate = DateTime.Now;
                        objpge1.EMP_BestOfApplicableFlg = _category.EMP_BestOfApplicableFlg;
                        objpge1.EMP_BestOf = _category.EMP_BestOf;
                        _examcontext.Add(objpge1);
                        _category.EMP_Id = objpge1.EMP_Id;
                        if (objpge1.EMP_MarksPerFlg != "F")
                        {
                            for (int j = 0; j < _category.pro_subjects_list.Length; j++)
                            {
                                Exm_M_Promotion_SubjectsDMO obj_subs = Mapper.Map<Exm_M_Promotion_SubjectsDMO>(_category.pro_subjects_list[j]);
                                obj_subs.EMP_Id = objpge1.EMP_Id;
                                obj_subs.EMPS_ActiveFlag = true;
                                obj_subs.CreatedDate = DateTime.Now;
                                obj_subs.UpdatedDate = DateTime.Now;

                                _examcontext.Add(obj_subs);
                                _category.EMPS_Id = obj_subs.EMPS_Id;

                                if (objpge1.EMP_MarksPerFlg != "T")
                                {
                                    for (int x = 0; x < _category.pro_exams_group_list.Length; x++)
                                    {
                                        Exm_M_Prom_Subj_GroupDMO obj_sub_grps = Mapper.Map<Exm_M_Prom_Subj_GroupDMO>(_category.pro_exams_group_list[x]);
                                        obj_sub_grps.EMPS_Id = obj_subs.EMPS_Id;
                                        obj_sub_grps.EMPSG_ActiveFlag = true;
                                        obj_sub_grps.CreatedDate = DateTime.Now;
                                        obj_sub_grps.UpdatedDate = DateTime.Now;

                                        _examcontext.Add(obj_sub_grps);

                                        _category.EMPSG_Id = obj_sub_grps.EMPSG_Id;

                                        for (int y = 0; y < _category.pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master.Length; y++)
                                        {
                                            Exm_M_Prom_Subj_Group_ExamsDMO obj_sub_grps_exms = new Exm_M_Prom_Subj_Group_ExamsDMO();
                                            obj_sub_grps_exms.EMPSG_Id = obj_sub_grps.EMPSG_Id;
                                            obj_sub_grps_exms.EME_Id = _category.pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EME_Id;
                                            obj_sub_grps_exms.EMPSGE_ForMaxMarkrs = _category.pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EMPSGE_ForMaxMarkrs;
                                            obj_sub_grps_exms.EMPSGE_ActiveFlg = true;
                                            obj_sub_grps_exms.CreatedDate = DateTime.Now;
                                            obj_sub_grps_exms.UpdatedDate = DateTime.Now;

                                            _examcontext.Add(obj_sub_grps_exms);
                                        }
                                    }
                                }
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
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                }

                _category.promotion_details = (from a in _examcontext.AcademicYear
                                               from b in _examcontext.Exm_Master_CategoryDMO
                                               from d in _examcontext.Exm_Yearly_CategoryDMO
                                               from e in _examcontext.Exm_M_PromotionDMO
                                               from f in _examcontext.Exm_Master_GradeDMO
                                               where (a.MI_Id == _category.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && f.MI_Id == a.MI_Id
                                               && a.ASMAY_Id == d.ASMAY_Id && d.EMCA_Id == b.EMCA_Id && d.EYC_Id == e.EYC_Id && e.EMGR_Id == f.EMGR_Id)
                                               select new PromotionSettingDTO
                                               {
                                                   EMP_Id = e.EMP_Id,
                                                   EYC_Id = e.EYC_Id,
                                                   EMCA_Id = d.EMCA_Id,
                                                   ASMAY_Id = d.ASMAY_Id,
                                                   EMGR_Id = e.EMGR_Id,
                                                   ASMAY_Year = a.ASMAY_Year,
                                                   EMCA_CategoryName = b.EMCA_CategoryName,
                                                   EMGR_GradeName = f.EMGR_GradeName,
                                                   EMP_PassToIndSubjectFlg = e.EMP_PassToIndSubjectFlg,
                                                   EMP_PassToOverallFlag = e.EMP_PassToOverallFlag,
                                                   EMP_MarksPerFlg = e.EMP_MarksPerFlg,
                                                   EMP_ActiveFlag = e.EMP_ActiveFlag,
                                                   ASMAY_Order = a.ASMAY_Order,
                                                   EMP_BestOf = e.EMP_BestOf,
                                                   EMP_BestOfApplicableFlg = e.EMP_BestOfApplicableFlg
                                               }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return _category;
        }
        public PromotionSettingDTO delete_records_of_pro_cat_Working(int id)
        {
            PromotionSettingDTO pagert = new PromotionSettingDTO();

            try
            {
                //for delete marks of promotion
                var EYC_Id = _examcontext.Exm_M_PromotionDMO.Single(t => t.EMP_Id == id).EYC_Id;
                var EMCA_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == EYC_Id).EMCA_Id;
                var ASMAY_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == EYC_Id).ASMAY_Id;
                var MI_Id = _examcontext.Exm_Yearly_CategoryDMO.Single(t => t.EYC_Id == EYC_Id).MI_Id;

                var class_sections = _examcontext.Exm_Category_ClassDMO.Where(t => t.MI_Id == MI_Id && t.ASMAY_Id == ASMAY_Id && t.EMCA_Id == EMCA_Id).Select(a => new { ASMCL_Id = a.ASMCL_Id, ASMS_Id = a.ASMS_Id }).Distinct().ToList();

                foreach (var c in class_sections)
                {
                    var lorg12 = _examcontext.Exm_Student_MP_PromotionDMO.Where(t => t.MI_Id == MI_Id && t.ASMAY_Id == ASMAY_Id && t.ASMCL_Id == c.ASMCL_Id && t.ASMS_Id == c.ASMS_Id).ToList();

                    var lorg22 = _examcontext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == MI_Id && t.ASMAY_Id == ASMAY_Id && t.ASMCL_Id == c.ASMCL_Id && t.ASMS_Id == c.ASMS_Id).ToList();

                    List<int> subs2 = new List<int>();
                    foreach (Exm_Stu_MP_Promo_SubjectwiseDMO x in lorg22)
                    {
                        subs2.Add(x.ESTMPPS_Id);
                    }

                    var lorg32 = _examcontext.Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO.Where(t => subs2.Contains(t.ESTMPPS_Id)).ToList();
                    List<int> subs23 = new List<int>();
                    foreach (Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO x in lorg32)
                    {
                        subs23.Add(x.ESTMPPSG_Id);
                    }

                    var lorg323 = _examcontext.Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO.Where(t => subs23.Contains(t.ESTMPPSG_Id)).ToList();

                    if (lorg323.Any())
                    {
                        for (int i = 0; lorg323.Count > i; i++)
                        {
                            _examcontext.Remove(lorg323.ElementAt(i));
                        }
                    }

                    if (lorg32.Any())
                    {
                        for (int i = 0; lorg32.Count > i; i++)
                        {
                            _examcontext.Remove(lorg32.ElementAt(i));
                        }
                    }
                    if (lorg22.Any())
                    {
                        for (int i = 0; lorg22.Count > i; i++)
                        {
                            _examcontext.Remove(lorg22.ElementAt(i));
                        }
                    }
                    if (lorg12.Any())
                    {
                        for (int i = 0; lorg12.Count > i; i++)
                        {
                            _examcontext.Remove(lorg12.ElementAt(i));
                        }
                    }
                }

                List<Exm_M_Promotion_SubjectsDMO> lorg2 = new List<Exm_M_Promotion_SubjectsDMO>();
                lorg2 = _examcontext.Exm_M_Promotion_SubjectsDMO.Where(t => t.EMP_Id == id).ToList();

                List<int> subs = new List<int>();
                foreach (Exm_M_Promotion_SubjectsDMO x in lorg2)
                {
                    subs.Add(x.EMPS_Id);
                }

                List<Exm_M_Prom_Subj_GroupDMO> lorg3 = new List<Exm_M_Prom_Subj_GroupDMO>();
                lorg3 = _examcontext.Exm_M_Prom_Subj_GroupDMO.Where(t => subs.Contains(t.EMPS_Id)).ToList();

                List<int> subs_grps = new List<int>();
                foreach (Exm_M_Prom_Subj_GroupDMO x in lorg3)
                {
                    subs_grps.Add(x.EMPSG_Id);
                }

                List<Exm_M_Prom_Subj_Group_ExamsDMO> lorg4 = new List<Exm_M_Prom_Subj_Group_ExamsDMO>();
                lorg4 = _examcontext.Exm_M_Prom_Subj_Group_ExamsDMO.Where(t => subs_grps.Contains(t.EMPSG_Id)).ToList();

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
                if (lorg2.Any())
                {
                    for (int i = 0; lorg2.Count > i; i++)
                    {
                        _examcontext.Remove(lorg2.ElementAt(i));
                    }
                }

                var outputval1 = _examcontext.SaveChanges();

                if (outputval1 >= 1)
                {
                    pagert.returnval = true;
                }
                else
                {
                    pagert.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pagert;
        }
        public PromotionSettingDTO savedetails_backup(PromotionSettingDTO _category)
        {
            Exm_M_PromotionDMO objpge = Mapper.Map<Exm_M_PromotionDMO>(_category);
            try
            {
                if (objpge.EMP_Id > 0)
                {
                    Exm_M_PromotionDMO objpge1 = Mapper.Map<Exm_M_PromotionDMO>(_category);
                    var result = _examcontext.Exm_M_PromotionDMO.Where(t => t.EYC_Id == objpge1.EYC_Id && t.MI_Id == objpge1.MI_Id && t.EMP_Id != objpge1.EMP_Id).ToList();

                    if (result.Count() == 0)
                    {
                        PromotionSettingDTO req = new PromotionSettingDTO();
                        var resulted = _examcontext.Exm_M_PromotionDMO.Single(t => t.EMP_Id == _category.EMP_Id && t.MI_Id == _category.MI_Id);
                        var total_flag = resulted.EMP_MarksPerFlg;
                        resulted.MI_Id = _category.MI_Id;
                        resulted.EYC_Id = _category.EYC_Id;
                        resulted.EMGR_Id = _category.EMGR_Id;
                        resulted.EMP_PassToIndSubjectFlg = _category.EMP_PassToIndSubjectFlg;
                        resulted.EMP_PassToOverallFlag = _category.EMP_PassToOverallFlag;
                        resulted.EMP_MarksPerFlg = _category.EMP_MarksPerFlg;
                        resulted.EMP_ActiveFlag = true;
                        resulted.UpdatedDate = DateTime.Now;
                        resulted.EMP_BestOfApplicableFlg = _category.EMP_BestOfApplicableFlg;
                        resulted.EMP_BestOf = _category.EMP_BestOf;

                        if (total_flag == "F")
                        {
                            req.returnval = true;
                        }
                        else if (total_flag != "F")
                        {
                            req = delete_records_of_pro_cat(objpge.EMP_Id);
                        }

                        if (req.returnval == true)
                        {
                            _examcontext.Update(resulted);
                            if (_category.EMP_MarksPerFlg != "F")
                            {
                                for (int j = 0; j < _category.pro_subjects_list.Length; j++)
                                {
                                    Exm_M_Promotion_SubjectsDMO obj_subs = Mapper.Map<Exm_M_Promotion_SubjectsDMO>(_category.pro_subjects_list[j]);
                                    obj_subs.EMP_Id = objpge1.EMP_Id;
                                    obj_subs.EMPS_ActiveFlag = true;
                                    obj_subs.CreatedDate = resulted.CreatedDate;
                                    obj_subs.UpdatedDate = DateTime.Now;

                                    _examcontext.Add(obj_subs);
                                    _category.EMPS_Id = obj_subs.EMPS_Id;
                                    if (_category.EMP_MarksPerFlg != "T")
                                    {
                                        for (int x = 0; x < _category.pro_subjects_list[j].pro_exams_group_list.Length; x++)
                                        {
                                            Exm_M_Prom_Subj_GroupDMO obj_sub_grps = Mapper.Map<Exm_M_Prom_Subj_GroupDMO>(_category.pro_subjects_list[j].pro_exams_group_list[x]);
                                            obj_sub_grps.EMPS_Id = obj_subs.EMPS_Id;
                                            obj_sub_grps.EMPSG_ActiveFlag = true;
                                            obj_sub_grps.CreatedDate = resulted.CreatedDate;
                                            obj_sub_grps.UpdatedDate = DateTime.Now;
                                            _examcontext.Add(obj_sub_grps);
                                            _category.EMPSG_Id = obj_sub_grps.EMPSG_Id;

                                            for (int y = 0; y < _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master.Length; y++)
                                            {
                                                Exm_M_Prom_Subj_Group_ExamsDMO obj_sub_grps_exms = new Exm_M_Prom_Subj_Group_ExamsDMO();
                                                obj_sub_grps_exms.EMPSG_Id = obj_sub_grps.EMPSG_Id;
                                                obj_sub_grps_exms.EME_Id = _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EME_Id;
                                                obj_sub_grps_exms.EMPSGE_ForMaxMarkrs = _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EMPSGE_ForMaxMarkrs;
                                                obj_sub_grps_exms.EMPSGE_ConvertionReqOrNot = _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EMPSGE_ConvertionReqOrNot;
                                                obj_sub_grps_exms.EMPSGE_ActiveFlg = true;
                                                obj_sub_grps_exms.CreatedDate = resulted.CreatedDate;
                                                obj_sub_grps_exms.UpdatedDate = DateTime.Now;

                                                _examcontext.Add(obj_sub_grps_exms);
                                            }
                                        }
                                    }
                                }
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
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                }

                else
                {
                    Exm_M_PromotionDMO objpge1 = Mapper.Map<Exm_M_PromotionDMO>(_category);
                    var result = _examcontext.Exm_M_PromotionDMO.Where(t => t.EYC_Id == objpge1.EYC_Id && t.MI_Id == objpge1.MI_Id).ToList();
                    if (result.Count() == 0)
                    {
                        objpge1.EMP_ActiveFlag = true;
                        objpge1.CreatedDate = DateTime.Now;
                        objpge1.UpdatedDate = DateTime.Now;
                        objpge1.EMP_BestOfApplicableFlg = _category.EMP_BestOfApplicableFlg;
                        objpge1.EMP_BestOf = _category.EMP_BestOf;
                        _examcontext.Add(objpge1);
                        _category.EMP_Id = objpge1.EMP_Id;

                        if (objpge1.EMP_MarksPerFlg != "F")
                        {
                            for (int j = 0; j < _category.pro_subjects_list.Length; j++)
                            {
                                Exm_M_Promotion_SubjectsDMO obj_subs = Mapper.Map<Exm_M_Promotion_SubjectsDMO>(_category.pro_subjects_list[j]);
                                obj_subs.EMP_Id = objpge1.EMP_Id;
                                obj_subs.EMPS_ActiveFlag = true;
                                obj_subs.CreatedDate = DateTime.Now;
                                obj_subs.UpdatedDate = DateTime.Now;

                                _examcontext.Add(obj_subs);
                                _category.EMPS_Id = obj_subs.EMPS_Id;

                                if (objpge1.EMP_MarksPerFlg != "T")
                                {
                                    for (int x = 0; x < _category.pro_subjects_list[j].pro_exams_group_list.Length; x++)
                                    {
                                        Exm_M_Prom_Subj_GroupDMO obj_sub_grps = Mapper.Map<Exm_M_Prom_Subj_GroupDMO>(_category.pro_subjects_list[j].pro_exams_group_list[x]);
                                        obj_sub_grps.EMPS_Id = obj_subs.EMPS_Id;
                                        obj_sub_grps.EMPSG_ActiveFlag = true;
                                        obj_sub_grps.CreatedDate = DateTime.Now;
                                        obj_sub_grps.UpdatedDate = DateTime.Now;

                                        _examcontext.Add(obj_sub_grps);

                                        _category.EMPSG_Id = obj_sub_grps.EMPSG_Id;

                                        for (int y = 0; y < _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master.Length; y++)
                                        {
                                            Exm_M_Prom_Subj_Group_ExamsDMO obj_sub_grps_exms = new Exm_M_Prom_Subj_Group_ExamsDMO();
                                            obj_sub_grps_exms.EMPSG_Id = obj_sub_grps.EMPSG_Id;
                                            obj_sub_grps_exms.EME_Id = _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EME_Id;
                                            obj_sub_grps_exms.EMPSGE_ForMaxMarkrs = _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EMPSGE_ForMaxMarkrs;
                                            obj_sub_grps_exms.EMPSGE_ConvertionReqOrNot = _category.pro_subjects_list[j].pro_exams_group_list[x].Exm_M_Prom_Subj_Group_Exams_master[y].EMPSGE_ConvertionReqOrNot;
                                            obj_sub_grps_exms.EMPSGE_ActiveFlg = true;
                                            obj_sub_grps_exms.CreatedDate = DateTime.Now;
                                            obj_sub_grps_exms.UpdatedDate = DateTime.Now;

                                            _examcontext.Add(obj_sub_grps_exms);
                                        }
                                    }
                                }
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
                    else
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                }

                _category.promotion_details = (from a in _examcontext.AcademicYear
                                               from b in _examcontext.Exm_Master_CategoryDMO
                                               from d in _examcontext.Exm_Yearly_CategoryDMO
                                               from e in _examcontext.Exm_M_PromotionDMO
                                               from f in _examcontext.Exm_Master_GradeDMO
                                               where (a.MI_Id == _category.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && f.MI_Id == a.MI_Id
                                               && a.ASMAY_Id == d.ASMAY_Id && d.EMCA_Id == b.EMCA_Id && d.EYC_Id == e.EYC_Id && e.EMGR_Id == f.EMGR_Id)
                                               select new PromotionSettingDTO
                                               {
                                                   EMP_Id = e.EMP_Id,
                                                   EYC_Id = e.EYC_Id,
                                                   EMCA_Id = d.EMCA_Id,
                                                   ASMAY_Id = d.ASMAY_Id,
                                                   EMGR_Id = e.EMGR_Id,
                                                   ASMAY_Year = a.ASMAY_Year,
                                                   EMCA_CategoryName = b.EMCA_CategoryName,
                                                   EMGR_GradeName = f.EMGR_GradeName,
                                                   EMP_PassToIndSubjectFlg = e.EMP_PassToIndSubjectFlg,
                                                   EMP_PassToOverallFlag = e.EMP_PassToOverallFlag,
                                                   EMP_MarksPerFlg = e.EMP_MarksPerFlg,
                                                   EMP_ActiveFlag = e.EMP_ActiveFlag,
                                                   ASMAY_Order = a.ASMAY_Order,
                                                   EMP_BestOf = e.EMP_BestOf,
                                                   EMP_BestOfApplicableFlg = e.EMP_BestOfApplicableFlg
                                               }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();

            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return _category;
        }
    }
}