
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
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model.com.vapstech.Exam;

namespace ExamServiceHub.com.vaps.Services
{
    public class PromotionCalculationImpl : Interfaces.PromotionCalculationInterface
    {
        private static ConcurrentDictionary<string, PromotionCalculationDTO> _login =
         new ConcurrentDictionary<string, PromotionCalculationDTO>();

        private readonly ExamContext _CumulativeReportContext;
        ILogger<PromotionCalculationImpl> _acdimpl;
        public PromotionCalculationImpl(ExamContext cpContext)
        {
            _CumulativeReportContext = cpContext;
        }
        public PromotionCalculationDTO Getdetails(PromotionCalculationDTO data)
        {
            PromotionCalculationDTO getdata = new PromotionCalculationDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _CumulativeReportContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                getdata.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _CumulativeReportContext.School_M_Section.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToList();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = _CumulativeReportContext.AdmissionClass.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToList();
                getdata.classlist = admlist.ToArray();

                //List<exammasterDMO> esmp = new List<exammasterDMO>();
                //esmp = _CumulativeReportContext.exammasterDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag== true).ToList();
                //getdata.exmstdlist = esmp.ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public PromotionCalculationDTO get_cls_sections(PromotionCalculationDTO id)
        {

            try
            {
                //var Cat_Id = _CumulativeReportContext.Exm_Category_ClassDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMCL_Id == id.ASMCL_Id && t.ASMAY_Id == id.ASMAY_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).First();
                //var year_cat_id = _CumulativeReportContext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == id.MI_Id && t.ASMAY_Id == id.ASMAY_Id && t.EYC_ActiveFlg == true && t.EMCA_Id == Cat_Id).Select(t => t.EYC_Id).First();
                //id.seclist = (from m in _CumulativeReportContext.Exm_Category_ClassDMO
                //                 // from n in _CumulativeReportContext.Adm_School_Master_Class_Cat_SecDMO
                //                  from o in _CumulativeReportContext.School_M_Section
                //                  where (m.ASMCL_Id == id.ASMCL_Id && m.ASMS_Id==o.ASMS_Id 
                //                  && o.ASMC_ActiveFlag == 1 && o.MI_Id == id.MI_Id && m.MI_Id==id.MI_Id
                //                  && m.ASMAY_Id == id.ASMAY_Id && m.ECAC_ActiveFlag==true)
                //                  select new School_M_Section
                //                  {
                //                      ASMS_Id = o.ASMS_Id,
                //                      ASMC_SectionName = o.ASMC_SectionName,
                //                      ASMC_SectionCode = o.ASMC_SectionCode,
                //                      ASMC_Order = o.ASMC_Order,
                //                      ASMC_MaxCapacity = o.ASMC_MaxCapacity,
                //                      ASMC_ActiveFlag = o.ASMC_ActiveFlag,

                //                  }).ToArray();

                ////List<exammasterDMO> esmp = new List<exammasterDMO>();
                ////esmp = _CumulativeReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).ToList();
                //id.exmstdlist = (from a in _CumulativeReportContext.masterexam
                //                 from b in _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO
                //                 where (a.MI_Id == id.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == year_cat_id)
                //                 select a).Distinct().ToArray();

                id.seclist = (from c in _CumulativeReportContext.School_M_Section
                              from d in _CumulativeReportContext.Exm_Category_ClassDMO
                              from e in _CumulativeReportContext.AdmissionClass
                              where (c.MI_Id == id.MI_Id && c.ASMS_Id == d.ASMS_Id && c.ASMC_ActiveFlag == 1 && d.MI_Id == id.MI_Id
                              && d.ASMAY_Id == id.ASMAY_Id && d.ASMCL_Id == id.ASMCL_Id && d.ASMCL_Id == e.ASMCL_Id && d.ECAC_ActiveFlag == true
                              && c.ASMS_Id == d.ASMS_Id)
                              select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public PromotionCalculationDTO Calculation(PromotionCalculationDTO exm)
        {
            try
            {
                exm.returnval = Promotion_Calculation(exm);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                exm.returnval = false;
            }

            return exm;
        }
        public PromotionCalculationDTO get_classes(PromotionCalculationDTO data)
        {

            try
            {
                data.classlist = (from c in _CumulativeReportContext.AdmissionClass
                                  from d in _CumulativeReportContext.Exm_Category_ClassDMO
                                  where (c.MI_Id == data.MI_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMCL_ActiveFlag == true && d.MI_Id == data.MI_Id
                                  && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true)
                                  select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public PromotionCalculationDTO publishtostudentportal(PromotionCalculationDTO data)
        {
            try
            {
                try
                {
                    var outputval = _CumulativeReportContext.Database.ExecuteSqlCommand("Exam_Promotion_Marks_Approval_Process_To_Publis_Student_Portal @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                    if (outputval >= 1)
                    {
                        data.returnval = true;

                        TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                        Exm_Calculation_LogDMO dmo = new Exm_Calculation_LogDMO();

                        dmo.MI_Id = data.MI_Id;
                        dmo.ASMAY_Id = data.ASMAY_Id;
                        dmo.ASMCL_Id = data.ASMCL_Id;
                        dmo.ASMS_Id = data.ASMS_Id;
                        dmo.EME_Id = 0;
                        dmo.IVRMUL_Id = data.userid;
                        dmo.ECL_PublishFlag = 1;
                        dmo.CreatedDate = indiantime0;
                        dmo.UpdatedDate = indiantime0;
                        _CumulativeReportContext.Add(dmo);

                        try
                        {
                            var i = _CumulativeReportContext.SaveChanges();
                            if (i > 0)
                            {
                                _acdimpl.LogInformation("Exam Calculation New Insert Publish Into Log Success");
                                data.returnval = true;
                            }
                            else
                            {
                                _acdimpl.LogInformation("Exam Calculation New Insert Publish Into Log Failed");
                                data.returnval = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            _acdimpl.LogInformation("Exam Calculation New Insert :" + data.MI_Id + " " + ex.Message + "");
                            Console.WriteLine(ex.Message);
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                catch (Exception ee)
                {
                    _acdimpl.LogInformation("Exam Calculation New 2:" + data.MI_Id + " " + ee.Message + "");
                    Console.WriteLine(ee.Message);
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public PromotionCalculationDTO onchangesection(PromotionCalculationDTO data)
        {
            try
            {
                var checkmarkscalculated = _CumulativeReportContext.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).Distinct().ToList();

                if (checkmarkscalculated.Count() > 0)
                {
                    data.countcalculated = checkmarkscalculated.Count();
                }
                else
                {
                    data.countcalculated = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public bool Promotion_Calculation(PromotionCalculationDTO data)
        {
            var go_stop = false;
            try
            {
                var getconfing = _CumulativeReportContext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();


                var EMCA_Id = _CumulativeReportContext.Exm_Category_ClassDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;
                var EYC_Id = _CumulativeReportContext.Exm_Yearly_CategoryDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id
                && t.EYC_ActiveFlg == true).EYC_Id;

                var promotion_type = _CumulativeReportContext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id
                && t.EMP_ActiveFlag == true).EMP_MarksPerFlg;
                data.EMP_MarksPerFlg = promotion_type;
                if (promotion_type == "T" || promotion_type == "F")
                {
                    if (promotion_type == "F")
                    {
                        var EME_Id_Final = _CumulativeReportContext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && t.EME_FinalExamFlag == true).FirstOrDefault().EME_Id;

                        var Cat_exms = _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == EYC_Id && t.EYCE_ActiveFlg == true).Select(t => t.EME_Id).Distinct().ToList();
                        if (Cat_exms.Contains(EME_Id_Final))
                        {
                            var stu_process_marks = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == EME_Id_Final).OrderBy(t => t.AMST_Id).Distinct().ToList();

                            var stu_subj_process_marks = _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == EME_Id_Final).OrderBy(t => t.AMST_Id).Distinct().ToList();

                            if (stu_process_marks.Count > 0 && stu_subj_process_marks.Count > 0)
                            {
                                var EMGR_Id = _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO.Single(t => t.EYC_Id == EYC_Id && t.EYCE_ActiveFlg == true && t.EME_Id == EME_Id_Final).EMGR_Id;

                                var gradedetails = _CumulativeReportContext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == EMGR_Id && t.EMGD_ActiveFlag == true).Distinct().ToList();

                                //var already_details = _CumulativeReportContext.Exm_Student_MP_PromotionDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToList();

                                //var already_details1 = _CumulativeReportContext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToList();

                                //var already_details2 = (from a in _CumulativeReportContext.Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO
                                //                        from b in already_details1
                                //                        where (a.ESTMPPS_Id == b.ESTMPPS_Id)
                                //                        select a).Distinct().ToList();

                                //if (already_details2.Any())
                                //{
                                //    for (int i = 0; already_details2.Count > i; i++)
                                //    {
                                //        _CumulativeReportContext.Remove(already_details2.ElementAt(i));
                                //    }
                                //}

                                //if (already_details1.Any())
                                //{
                                //    for (int i = 0; already_details1.Count > i; i++)
                                //    {
                                //        _CumulativeReportContext.Remove(already_details1.ElementAt(i));
                                //    }
                                //}

                                //if (already_details.Any())
                                //{
                                //    for (int i = 0; already_details.Count > i; i++)
                                //    {
                                //        _CumulativeReportContext.Remove(already_details.ElementAt(i));
                                //    }
                                //}

                                var outputval = _CumulativeReportContext.Database.ExecuteSqlCommand("Exm_PromotionDetails_Delete @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                                foreach (var subj_mks in stu_subj_process_marks)
                                {
                                    Exm_Stu_MP_Promo_SubjectwiseDMO obj_s = new Exm_Stu_MP_Promo_SubjectwiseDMO();
                                    obj_s.MI_Id = data.MI_Id;
                                    obj_s.ASMAY_Id = data.ASMAY_Id;
                                    obj_s.ASMCL_Id = data.ASMCL_Id;
                                    obj_s.ASMS_Id = data.ASMS_Id;
                                    obj_s.AMST_Id = subj_mks.AMST_Id;
                                    obj_s.ISMS_Id = subj_mks.ISMS_Id;
                                    obj_s.ESTMPPS_MaxMarks = subj_mks.ESTMPS_Medical_MaxMarks;
                                    obj_s.ESTMPPS_ObtainedMarks = subj_mks.ESTMPS_ObtainedMarks;
                                    obj_s.ESTMPPS_ObtainedGrade = subj_mks.ESTMPS_ObtainedGrade;
                                    obj_s.ESTMPPS_GradePoints = subj_mks.ESTMPS_GradePoints;
                                    obj_s.ESTMPPS_ClassAverage = subj_mks.ESTMPS_ClassAverage;
                                    obj_s.ESTMPPS_SectionAverage = subj_mks.ESTMPS_SectionAverage;
                                    obj_s.ESTMPPS_ClassHighest = subj_mks.ESTMPS_ClassHighest;
                                    obj_s.ESTMPPS_SectionHighest = subj_mks.ESTMPS_SectionHighest;
                                    obj_s.ESTMPPS_PassFailFlg = subj_mks.ESTMPS_PassFailFlg;

                                    obj_s.EMGD_Id = (obj_s.ESTMPPS_ObtainedGrade != null && obj_s.ESTMPPS_ObtainedGrade != "") ?
                                        gradedetails.Where(t => t.EMGD_Name == obj_s.ESTMPPS_ObtainedGrade).FirstOrDefault().EMGD_Id : obj_s.EMGD_Id;

                                    obj_s.ESTMPPS_Remarks = (obj_s.ESTMPPS_ObtainedGrade != null && obj_s.ESTMPPS_ObtainedGrade != "") ?
                                        gradedetails.Where(t => t.EMGD_Name == obj_s.ESTMPPS_ObtainedGrade).FirstOrDefault().EMGD_Remarks : obj_s.ESTMPPS_ObtainedGrade;

                                    obj_s.CreatedDate = DateTime.Now;
                                    obj_s.UpdatedDate = DateTime.Now;
                                    _CumulativeReportContext.Add(obj_s);
                                }

                                foreach (var mks in stu_process_marks)
                                {
                                    Exm_Student_MP_PromotionDMO obj_m = new Exm_Student_MP_PromotionDMO();
                                    obj_m.MI_Id = data.MI_Id;
                                    obj_m.ASMAY_Id = data.ASMAY_Id;
                                    obj_m.ASMCL_Id = data.ASMCL_Id;
                                    obj_m.ASMS_Id = data.ASMS_Id;
                                    obj_m.AMST_Id = mks.AMST_Id;
                                    obj_m.ESTMPP_TotalMaxMarks = mks.ESTMP_TotalMaxMarks;
                                    obj_m.ESTMPP_TotalObtMarks = mks.ESTMP_TotalObtMarks;
                                    obj_m.ESTMPP_GraceMarks = 0;
                                    obj_m.ESTMPP_BonusMarks = 0;
                                    obj_m.ESTMPP_TotalMarks = (obj_m.ESTMPP_TotalObtMarks + obj_m.ESTMPP_GraceMarks + obj_m.ESTMPP_BonusMarks);
                                    obj_m.ESTMPP_Percentage = mks.ESTMP_Percentage;
                                    obj_m.ESTMPP_TotalGrade = mks.ESTMP_TotalGrade;
                                    obj_m.ESTMPP_ClassRank = mks.ESTMP_ClassRank;
                                    obj_m.ESTMPP_SectionRank = mks.ESTMP_SectionRank;
                                    obj_m.ESTMPP_Result = mks.ESTMP_Result;
                                    obj_m.EMGD_Id = mks.EMGD_Id;
                                    obj_m.CreatedDate = DateTime.Now;
                                    obj_m.UpdatedDate = DateTime.Now;
                                    _CumulativeReportContext.Add(obj_m);
                                }

                                var contactExists = _CumulativeReportContext.SaveChanges();
                                if (contactExists >= 1)
                                {
                                    go_stop = true;
                                }
                                else
                                {
                                    go_stop = false;
                                }
                            }
                        }

                    }

                    else if (promotion_type == "T")
                    {                     
                        var already_details1 = _CumulativeReportContext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToList();

                        //var already_details2 = (from a in _CumulativeReportContext.Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO
                        //                        from b in already_details1
                        //                        where (a.ESTMPPS_Id == b.ESTMPPS_Id)
                        //                        select a).Distinct().ToList();
                        //if (already_details2.Any())
                        //{
                        //    for (int i = 0; already_details2.Count > i; i++)
                        //    {
                        //        _CumulativeReportContext.Remove(already_details2.ElementAt(i));
                        //    }
                        //}

                        //if (already_details1.Any())
                        //{
                        //    for (int i = 0; already_details1.Count > i; i++)
                        //    {
                        //        _CumulativeReportContext.Remove(already_details1.ElementAt(i));
                        //    }
                        //}

                        var outputval = _CumulativeReportContext.Database.ExecuteSqlCommand("Exm_PromotionDetails_Delete @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                        var student_list = (from a in _CumulativeReportContext.ExmStudentMarksProcessDMO
                                            from b in _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                            where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id 
                                            && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                            select a).Select(t => t.AMST_Id).Distinct().ToList();

                        var EMGR_Id = _CumulativeReportContext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id
                        && t.EMP_ActiveFlag == true).EMGR_Id;

                        var EMGR_MarksPerFlag = _CumulativeReportContext.Exm_Master_GradeDMO.Single(t => t.MI_Id == data.MI_Id && t.EMGR_Id == EMGR_Id
                        && t.EMGR_ActiveFlag == true).EMGR_MarksPerFlag;

                        var Grade_Details = _CumulativeReportContext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == EMGR_Id 
                        && t.EMGD_ActiveFlag == true).Distinct().ToList();

                        foreach (var stu_id in student_list)
                        {
                            data.AMST_Id = stu_id;

                            var student_subjects = (from a in _CumulativeReportContext.StudentMappingDMO
                                                    from b in _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                                    where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                                    && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id && a.ESTSU_ActiveFlg == true && b.MI_Id == a.MI_Id
                                                    && b.ASMAY_Id == a.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id && b.ASMS_Id == a.ASMS_Id && b.AMST_Id == a.AMST_Id
                                                    && b.ISMS_Id == a.ISMS_Id)
                                                    select a.ISMS_Id).Distinct().ToList();

                            foreach (var subj_id in student_subjects)
                            {
                                var stu_subj_process_marks = _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id 
                                && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id 
                                && t.ISMS_Id == subj_id).OrderBy(t => t.EME_Id).Distinct().ToList();

                                decimal? ESTMPPS_MaxMarks = 0;
                                decimal? ESTMPPS_ObtainedMarks = 0;
                                var flag = "";

                                foreach (var marks in stu_subj_process_marks)
                                {
                                    var subj_flg_exm_wise = marks.ESTMPS_PassFailFlg;
                                    if (subj_flg_exm_wise == "AB" || subj_flg_exm_wise == "M" || subj_flg_exm_wise == "OD" || subj_flg_exm_wise == "L")
                                    {
                                        flag = subj_flg_exm_wise;
                                    }
                                    ESTMPPS_MaxMarks += marks.ESTMPS_Medical_MaxMarks;
                                    ESTMPPS_ObtainedMarks += marks.ESTMPS_ObtainedMarks;
                                }
                                var result_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                if (EMGR_MarksPerFlag == "M")
                                {
                                    result_grade = Grade_Details.Where(t => (ESTMPPS_ObtainedMarks >= t.EMGD_From && ESTMPPS_ObtainedMarks <= t.EMGD_To) || (ESTMPPS_ObtainedMarks <= t.EMGD_From && ESTMPPS_ObtainedMarks >= t.EMGD_To)).Distinct().ToList();
                                }

                                else if (EMGR_MarksPerFlag == "P")
                                {
                                    decimal? per = 0;
                                    per = (ESTMPPS_ObtainedMarks / ESTMPPS_MaxMarks) * 100;
                                    result_grade = Grade_Details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To) || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                }

                                decimal? ESTMPPS_MinMarks = 0;
                                ESTMPPS_MinMarks = (from a in _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO
                                                    from b in _CumulativeReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                                    where (a.EYC_Id == EYC_Id && a.EYCE_ActiveFlg == true && b.EYCE_Id == a.EYCE_Id && b.EYCES_ActiveFlg == true
                                                    && b.ISMS_Id == subj_id)
                                                    select b.EYCES_MinMarks).Sum();

                                Exm_Stu_MP_Promo_SubjectwiseDMO obj_p = new Exm_Stu_MP_Promo_SubjectwiseDMO();
                                obj_p.MI_Id = data.MI_Id;
                                obj_p.ASMAY_Id = data.ASMAY_Id;
                                obj_p.ASMCL_Id = data.ASMCL_Id;
                                obj_p.ASMS_Id = data.ASMS_Id;
                                obj_p.AMST_Id = data.AMST_Id;
                                obj_p.ISMS_Id = subj_id;
                                obj_p.ESTMPPS_MaxMarks = ESTMPPS_MaxMarks;
                                obj_p.ESTMPPS_ObtainedMarks = ESTMPPS_ObtainedMarks;
                                obj_p.ESTMPPS_ObtainedGrade = result_grade.Count > 0 ? result_grade[0].EMGD_Name : null;
                                obj_p.ESTMPPS_GradePoints = result_grade.Count > 0 ? result_grade[0].EMGD_GradePoints : null;
                                if (result_grade.Count > 0)
                                {
                                    obj_p.EMGD_Id = result_grade[0].EMGD_Id;
                                }

                                obj_p.ESTMPPS_PassFailFlg = flag == "" ? ((ESTMPPS_ObtainedMarks) < ESTMPPS_MinMarks ? "Fail" : "Pass") : flag;
                                obj_p.ESTMPPS_Remarks = result_grade.Count > 0 ? result_grade[0].EMGD_Remarks : null;
                                obj_p.CreatedDate = DateTime.Now;
                                obj_p.UpdatedDate = DateTime.Now;
                                _CumulativeReportContext.Add(obj_p);
                            }
                        }

                        var contactExists = _CumulativeReportContext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            go_stop = true;

                            using (var cmd = _CumulativeReportContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Promotion_StudentCS_CACH_Total";
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

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                try
                                {
                                    var a = cmd.ExecuteNonQuery();
                                    if (a >= 1)
                                    {
                                        go_stop = true;
                                    }
                                    else
                                    {
                                        go_stop = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    go_stop = false;
                                    _acdimpl.LogError(ex.Message);
                                    _acdimpl.LogDebug(ex.Message);
                                }
                            }

                        }
                        else
                        {
                            go_stop = false;
                        }
                    }
                }

                else if (promotion_type != "T" && promotion_type != "F")
                {
                    if (promotion_type == "P")
                    {
                        //var already_details1 = _CumulativeReportContext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToList();

                        //var already_details2 = (from a in _CumulativeReportContext.Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO
                        //                        from b in already_details1
                        //                        where (a.ESTMPPS_Id == b.ESTMPPS_Id)
                        //                        select a).Distinct().ToList();

                        //var already_details3 = (from a in _CumulativeReportContext.Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO
                        //                        from b in already_details2
                        //                        where (a.ESTMPPSG_Id == b.ESTMPPSG_Id)
                        //                        select a).Distinct().ToList();


                        //if (already_details3.Any())
                        //{
                        //    for (int i = 0; already_details3.Count > i; i++)
                        //    {
                        //        _CumulativeReportContext.Remove(already_details3.ElementAt(i));
                        //    }
                        //}

                        //if (already_details2.Any())
                        //{
                        //    for (int i = 0; already_details2.Count > i; i++)
                        //    {
                        //        _CumulativeReportContext.Remove(already_details2.ElementAt(i));
                        //    }
                        //}

                        //if (already_details1.Any())
                        //{
                        //    for (int i = 0; already_details1.Count > i; i++)
                        //    {
                        //        _CumulativeReportContext.Remove(already_details1.ElementAt(i));
                        //    }
                        //}

                        var outputval = _CumulativeReportContext.Database.ExecuteSqlCommand("Exm_PromotionDetails_Delete @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                        var EMP_Id = _CumulativeReportContext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id && t.EMP_ActiveFlag == true).EMP_Id;

                       //var promotion_subectdetails_new = _CumulativeReportContext.Exm_M_Promotion_SubjectsDMO.Where(t => t.EMP_Id == EMP_Id).Distinct().ToList();

                        var student_list = (from a in _CumulativeReportContext.ExmStudentMarksProcessDMO
                                            from b in _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                            where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id 
                                            && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                            select a).Select(t => t.AMST_Id).Distinct().ToList();

                        foreach (var stu_id in student_list)
                        {
                            data.AMST_Id = stu_id;

                            var promotion_subectdetails = (from a in _CumulativeReportContext.Exm_M_Promotion_SubjectsDMO
                                                           from b in _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                                           where (a.ISMS_Id == b.ISMS_Id && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id
                                                           && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.EMP_Id == EMP_Id
                                                           && a.EMPS_ActiveFlag == true && b.MI_Id==data.MI_Id)
                                                           select a).Distinct().ToList();

                            List<TEmp_GroupWise_Exam_Marks> TEmp_GroupWise_Exam_Marks = new List<TEmp_GroupWise_Exam_Marks>();
                            List<TEmp_GroupWise_Marks> Groupwise_Details = new List<TEmp_GroupWise_Marks>();

                            foreach (var s in promotion_subectdetails)
                            {
                                var EMGR_MarksPerFlag = _CumulativeReportContext.Exm_Master_GradeDMO.Single(t => t.MI_Id == data.MI_Id && t.EMGR_Id == s.EMGR_Id && t.EMGR_ActiveFlag == true).EMGR_MarksPerFlag;

                                var grade_details = _CumulativeReportContext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == s.EMGR_Id && t.EMGD_ActiveFlag == true).Distinct().ToList();

                                var stu_marks_details = _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id
                                && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id
                                && t.ISMS_Id == s.ISMS_Id).Distinct().ToList();

                                if (stu_marks_details.Count > 0)
                                {
                                    var prom_subj_groupdetails = _CumulativeReportContext.Exm_M_Prom_Subj_GroupDMO.Where(t => t.EMPS_Id == s.EMPS_Id).Distinct().ToList();
                                    var flag = "";
                                    //data.prom_subj_groupdetails = prom_subj_groupdetails.ToArray();
                                    foreach (var w in prom_subj_groupdetails)
                                    {
                                        decimal? group_marks = 0;
                                        decimal? ESTMPPSG_GroupMaxMarks = 0;

                                        var prom_subj_grp_exms = _CumulativeReportContext.Exm_M_Prom_Subj_Group_ExamsDMO.Where(t => t.EMPSGE_ActiveFlg == true
                                        && t.EMPSG_Id == w.EMPSG_Id).Distinct().ToList();

                                        List<decimal?> exms_marks_grpwise = new List<decimal?>();
                                        List<decimal?> exms_max_marks_grpwise = new List<decimal?>();

                                        List<decimal?> exms_marks_grpwise_Temp = new List<decimal?>();
                                        List<decimal?> exms_max_marks_grpwise_Temp = new List<decimal?>();

                                        int subexamgrp = 0;

                                        //foreach (var z in prom_subj_grp_exms)
                                        //{
                                        //    var result = stu_marks_details.Where(t => t.EME_Id == z.EME_Id && t.ESTMPS_PassFailFlg != "OD").Distinct().ToList();
                                        //    if (result.Count > 0)
                                        //    {
                                        //        var subj_max_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_Medical_MaxMarks;
                                        //        var subj_flg_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_PassFailFlg;
                                        //        var subj_obt_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_ObtainedMarks;

                                        //        var ratio = s.EMPS_MaxMarks / subj_max_exm_wise;
                                        //        decimal? groupwise_marks = 0;

                                        //        if (subj_flg_exm_wise != "AB" && subj_flg_exm_wise != "OD" && subj_flg_exm_wise != "M" && subj_flg_exm_wise != "L")
                                        //        {
                                        //            groupwise_marks = subj_obt_exm_wise * ratio;
                                        //        }
                                        //        else
                                        //        {
                                        //            flag = subj_flg_exm_wise;
                                        //        }
                                        //        exms_marks_grpwise.Add(groupwise_marks);
                                        //    }
                                        //}

                                        foreach (var z in prom_subj_grp_exms)
                                        {
                                            decimal? raitoexammarks = 0.00m;
                                            decimal? convertedexammarks = 0.00m;
                                            decimal? convertedexam_max_marks = 0.00m;

                                            decimal? raitoexammarks_temp = 0.00m;
                                            decimal? convertedexammarks_temp = 0.00m;
                                            decimal? convertedexam_max_marks_temp = 0.00m;

                                            var examForMaxMarkrs = z.EMPSGE_ForMaxMarkrs;

                                            var result = stu_marks_details.Where(t => t.EME_Id == z.EME_Id && t.ESTMPS_PassFailFlg != "OD").Distinct().ToList();
                                            if (result.Count > 0)
                                            {
                                                subexamgrp = 1;
                                                var subj_max_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_Medical_MaxMarks;
                                                var subj_flg_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_PassFailFlg;
                                                var subj_obt_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_ObtainedMarks;
                                                var subj_max_exm_wise_temp = subj_max_exm_wise;

                                                decimal? groupwise_marks = 0;
                                                decimal? groupwise_maxmarks = 0;

                                                if (subj_flg_exm_wise != "OD")
                                                {
                                                    groupwise_marks = subj_obt_exm_wise;
                                                }
                                                flag = subj_flg_exm_wise;

                                                if (z.EMPSGE_ConvertionReqOrNot == true)
                                                {
                                                    // 30 > 10
                                                    if (subj_max_exm_wise > z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        //raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                        raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                        convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                        convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;

                                                    }
                                                    //10 < 30
                                                    else if (subj_max_exm_wise < z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                        convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                        convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;
                                                    }
                                                    // 10 = 10
                                                    else if (subj_max_exm_wise == z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                        convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                        convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;
                                                    }
                                                    subj_max_exm_wise_temp = convertedexam_max_marks;
                                                }

                                                else
                                                {
                                                    convertedexammarks = subj_obt_exm_wise;
                                                }


                                                // Convertion To Group Marks For Best Marks 

                                                if (subj_max_exm_wise == 100)
                                                {
                                                    convertedexammarks_temp = subj_obt_exm_wise;
                                                    convertedexam_max_marks_temp = subj_max_exm_wise;
                                                }

                                                else if (subj_max_exm_wise > 100)
                                                {
                                                    raitoexammarks_temp = 100 / subj_max_exm_wise;
                                                    convertedexammarks_temp = subj_obt_exm_wise * raitoexammarks_temp;
                                                    convertedexam_max_marks_temp = subj_max_exm_wise * raitoexammarks_temp;
                                                }
                                                else if (subj_max_exm_wise < 100)
                                                {
                                                    raitoexammarks_temp = subj_max_exm_wise / 100;
                                                    convertedexammarks_temp = subj_obt_exm_wise * raitoexammarks_temp;
                                                    convertedexam_max_marks_temp = subj_max_exm_wise * raitoexammarks_temp;
                                                }

                                                groupwise_maxmarks = z.EMPSGE_ForMaxMarkrs;
                                                exms_marks_grpwise.Add(convertedexammarks);
                                                exms_max_marks_grpwise.Add(subj_max_exm_wise_temp);


                                                exms_marks_grpwise_Temp.Add(convertedexammarks_temp);
                                                exms_max_marks_grpwise_Temp.Add(convertedexam_max_marks_temp);

                                                var resultexam_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                                if (EMGR_MarksPerFlag == "M")
                                                {
                                                    resultexam_grade = grade_details.Where(t => (convertedexammarks >= t.EMGD_From
                                                    && convertedexammarks <= t.EMGD_To) || (convertedexammarks <= t.EMGD_From
                                                    && convertedexammarks >= t.EMGD_To)).Distinct().ToList();
                                                }
                                                else if (EMGR_MarksPerFlag == "P")
                                                {
                                                    decimal? per = 0;
                                                    per = (convertedexammarks / examForMaxMarkrs) * 100;
                                                    resultexam_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                    || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                                }

                                                int? EMGD_Id = null;
                                                if (resultexam_grade.Count > 0)
                                                {
                                                    EMGD_Id = resultexam_grade[0].EMGD_Id;
                                                }

                                                TEmp_GroupWise_Exam_Marks.Add(new TEmp_GroupWise_Exam_Marks
                                                {
                                                    MI_Id = data.MI_Id,
                                                    ASMAY_Id = data.ASMAY_Id,
                                                    ASMCL_Id = data.ASMCL_Id,
                                                    ASMS_Id = data.ASMS_Id,
                                                    AMST_Id = data.AMST_Id,
                                                    ISMS_Id = s.ISMS_Id,
                                                    EMPSG_Id = w.EMPSG_Id,
                                                    EME_Id = z.EME_Id,
                                                    ESTMPPSGE_ExamActualMarks = subj_obt_exm_wise,
                                                    ESTMPPSGE_ExamActualMaxMarks = subj_max_exm_wise,
                                                    ESTMPPSGE_ExamConvertedMarks = convertedexammarks,
                                                    ESTMPPSGE_ExamConvertedMaxMarks = subj_max_exm_wise_temp,
                                                    ESTMPPSGE_ExamPassFailFlag = subj_flg_exm_wise,
                                                    ESTMPPSGE_ExamConvertedGrade = resultexam_grade.Count > 0 ?
                                                    resultexam_grade[0].EMGD_Name : null,
                                                    ESTMPPSGE_ExamConvertedPoints = resultexam_grade.Count > 0 ?
                                                    resultexam_grade[0].EMGD_GradePoints : null,
                                                    EMGD_Id = EMGD_Id

                                                });
                                            }
                                        }

                                        var Best_off = w.EMPSG_BestOff;

                                        var best_marks_GroupTotal = exms_marks_grpwise.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var best_max_marks_GroupTotal = exms_max_marks_grpwise.OrderByDescending(t => t).Take(Best_off).ToList();

                                        var best_marks = exms_marks_grpwise_Temp.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var best_max_marks = exms_max_marks_grpwise_Temp.OrderByDescending(t => t).Take(Best_off).ToList();


                                        //var avg_marks = best_marks.Average();
                                        var avg_marks = best_marks.Sum();
                                        var avg_Max_marks = best_max_marks.Sum();

                                        var avg_marks_GroupTotal = best_marks_GroupTotal.Sum();
                                        var avg_Max_marks_GroupTotal = best_max_marks_GroupTotal.Sum();
                                        var avg_percentage_GroupTotal = (avg_marks_GroupTotal * s.EMPS_MaxMarks / avg_Max_marks_GroupTotal);

                                        if (promotion_type == "P")
                                        {
                                            if (avg_Max_marks > 0)
                                            {
                                                group_marks = (avg_marks * 100 / avg_Max_marks) * (w.EMPSG_PercentValue) / 100;
                                            }
                                            else
                                            {
                                                group_marks = 0;
                                            }

                                            ESTMPPSG_GroupMaxMarks = ((w.EMPSG_PercentValue) / (s.EMPS_MaxMarks)) * 100;
                                        }

                                        if (getconfing.FirstOrDefault().ExmConfig_RoundoffFlag == true)
                                        {
                                            group_marks = Math.Round(Convert.ToDecimal(group_marks), 0, MidpointRounding.AwayFromZero);
                                        }

                                        if (w.EMPSG_RoundOffFlag == true)
                                        {
                                            group_marks = Math.Round(Convert.ToDecimal(group_marks), 0, MidpointRounding.AwayFromZero);
                                        }

                                        Groupwise_Details.Add(new TEmp_GroupWise_Marks
                                        {
                                            MI_Id = data.MI_Id,
                                            ASMAY_Id = data.ASMAY_Id,
                                            ASMCL_Id = data.ASMCL_Id,
                                            ASMS_Id = data.ASMS_Id,
                                            AMST_Id = data.AMST_Id,
                                            ISMS_Id = s.ISMS_Id,
                                            EMPSG_Id = w.EMPSG_Id,
                                            ESTMPPSG_GroupMaxMarks = ESTMPPSG_GroupMaxMarks,
                                            ESTMPPSG_GroupObtMarks = group_marks,
                                            ESTMPPSG_GroupTotalMarks = avg_Max_marks_GroupTotal,
                                            ESTMPPSG_GroupObtMarksOutOfGroupTotal = avg_marks_GroupTotal,
                                            ESTMPPSG_ObtMarksOutOfSubjectMaxMarks = avg_percentage_GroupTotal,
                                        });
                                    }

                                    var sub_marks_total = Groupwise_Details.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id && t.ISMS_Id == s.ISMS_Id).GroupBy(t => t.ISMS_Id).Select(a => new { Sum_obtain = a.Sum(t => t.ESTMPPSG_GroupObtMarks), Sum_max = a.Sum(t => t.ESTMPPSG_GroupMaxMarks) }).ToList();

                                    //var EMGR_MarksPerFlag = _CumulativeReportContext.Exm_Master_GradeDMO.Single(t => t.MI_Id == data.MI_Id && t.EMGR_Id == s.EMGR_Id && t.EMGR_ActiveFlag == true).EMGR_MarksPerFlag;

                                    //var grade_details = _CumulativeReportContext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == s.EMGR_Id && t.EMGD_ActiveFlag == true).Distinct().ToList();

                                    if (sub_marks_total.Count > 0)
                                    {
                                        Exm_Stu_MP_Promo_SubjectwiseDMO obj_p = new Exm_Stu_MP_Promo_SubjectwiseDMO();
                                        obj_p.MI_Id = data.MI_Id;
                                        obj_p.ASMAY_Id = data.ASMAY_Id;
                                        obj_p.ASMCL_Id = data.ASMCL_Id;
                                        obj_p.ASMS_Id = data.ASMS_Id;
                                        obj_p.AMST_Id = data.AMST_Id;
                                        obj_p.ISMS_Id = s.ISMS_Id;

                                        if (s.EMPS_MaxMarks == s.EMPS_ConvertForMarks)
                                        {
                                            var result_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                            if (EMGR_MarksPerFlag == "M")
                                            {
                                                result_grade = grade_details.Where(t => (sub_marks_total[0].Sum_obtain >= t.EMGD_From
                                                && sub_marks_total[0].Sum_obtain <= t.EMGD_To) || (sub_marks_total[0].Sum_obtain <= t.EMGD_From
                                                && sub_marks_total[0].Sum_obtain >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            else if (EMGR_MarksPerFlag == "P")
                                            {
                                                decimal? per = 0;
                                                per = (sub_marks_total[0].Sum_obtain / sub_marks_total[0].Sum_max) * 100;
                                                result_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            obj_p.ESTMPPS_MaxMarks = sub_marks_total[0].Sum_max;
                                            obj_p.ESTMPPS_ObtainedMarks = sub_marks_total[0].Sum_obtain;
                                            obj_p.ESTMPPS_ObtainedGrade = result_grade.Count > 0 ? result_grade[0].EMGD_Name : null;
                                            obj_p.ESTMPPS_PassFailFlg = flag == "" ? ((s.EMPS_MinMarks) > obj_p.ESTMPPS_ObtainedMarks ? "Fail" : "Pass") : flag;
                                            obj_p.ESTMPPS_Remarks = result_grade.Count > 0 ? result_grade[0].EMGD_Remarks : null;
                                            obj_p.ESTMPPS_GradePoints = result_grade.Count > 0 ? result_grade[0].EMGD_GradePoints : null;
                                            if (result_grade.Count > 0)
                                            {
                                                obj_p.EMGD_Id = result_grade[0].EMGD_Id;
                                            }
                                        }
                                        else
                                        {
                                            var convert_ratio = s.EMPS_ConvertForMarks / s.EMPS_MaxMarks;
                                            obj_p.ESTMPPS_MaxMarks = (sub_marks_total[0].Sum_max * convert_ratio);
                                            obj_p.ESTMPPS_ObtainedMarks = (sub_marks_total[0].Sum_obtain * convert_ratio);
                                            if (getconfing.FirstOrDefault().ExmConfig_RoundoffFlag == true)
                                            {
                                                obj_p.ESTMPPS_ObtainedMarks = Math.Round(Convert.ToDecimal(obj_p.ESTMPPS_ObtainedMarks), 0,
                                                    MidpointRounding.AwayFromZero);
                                            }

                                            var result_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                            if (EMGR_MarksPerFlag == "M")
                                            {
                                                result_grade = grade_details.Where(t => (obj_p.ESTMPPS_ObtainedMarks >= t.EMGD_From
                                                && obj_p.ESTMPPS_ObtainedMarks <= t.EMGD_To) || (obj_p.ESTMPPS_ObtainedMarks <= t.EMGD_From
                                                && obj_p.ESTMPPS_ObtainedMarks >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            else if (EMGR_MarksPerFlag == "P")
                                            {
                                                decimal? per = 0.00m;
                                                per = (obj_p.ESTMPPS_ObtainedMarks / obj_p.ESTMPPS_MaxMarks) * 100;
                                                result_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            obj_p.ESTMPPS_ObtainedGrade = result_grade.Count > 0 ? result_grade[0].EMGD_Name : null;
                                            obj_p.ESTMPPS_PassFailFlg = flag == "" ? ((s.EMPS_MinMarks) > obj_p.ESTMPPS_ObtainedMarks ? "Fail" : "Pass") : flag;
                                            obj_p.ESTMPPS_Remarks = result_grade.Count > 0 ? result_grade[0].EMGD_Remarks : null;
                                            obj_p.ESTMPPS_GradePoints = result_grade.Count > 0 ? result_grade[0].EMGD_GradePoints : null;
                                            if (result_grade.Count > 0)
                                            {
                                                obj_p.EMGD_Id = result_grade[0].EMGD_Id;
                                            }
                                        }

                                        obj_p.CreatedDate = DateTime.Now;
                                        obj_p.UpdatedDate = DateTime.Now;
                                        _CumulativeReportContext.Add(obj_p);

                                        foreach (var q in Groupwise_Details)
                                        {
                                            decimal? per1 = 0;

                                            if (q.MI_Id == obj_p.MI_Id && q.ASMAY_Id == obj_p.ASMAY_Id && q.ASMCL_Id == obj_p.ASMCL_Id && q.ASMS_Id == obj_p.ASMS_Id
                                                && q.AMST_Id == obj_p.AMST_Id && q.ISMS_Id == obj_p.ISMS_Id)
                                            {
                                                var result1_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                                var result2_grade = new List<Exm_Master_Grade_DetailsDMO>();

                                                if (EMGR_MarksPerFlag == "M")
                                                {
                                                    result1_grade = grade_details.Where(t => (q.ESTMPPSG_GroupObtMarks >= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarks <= t.EMGD_To) || (q.ESTMPPSG_GroupObtMarks <= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarks >= t.EMGD_To)).Distinct().ToList();

                                                    result2_grade = grade_details.Where(t => (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal >= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarksOutOfGroupTotal <= t.EMGD_To) || (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal <= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarksOutOfGroupTotal >= t.EMGD_To)).Distinct().ToList();
                                                }
                                                else if (EMGR_MarksPerFlag == "P")
                                                {
                                                    decimal? per = 0;
                                                    per = (q.ESTMPPSG_GroupObtMarks / q.ESTMPPSG_GroupMaxMarks) * 100;
                                                    result1_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                    || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();

                                                    per1 = (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal / q.ESTMPPSG_GroupTotalMarks) * 100;

                                                    result2_grade = grade_details.Where(t => (per1 >= t.EMGD_From && per1 <= t.EMGD_To)
                                                    || (per1 <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                                }


                                                Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO obj_c = new Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO();
                                                obj_c.ESTMPPS_Id = obj_p.ESTMPPS_Id;
                                                obj_c.EMPSG_Id = q.EMPSG_Id;
                                                obj_c.ESTMPPSG_GroupMaxMarks = q.ESTMPPSG_GroupMaxMarks;
                                                obj_c.ESTMPPSG_GroupObtMarks = q.ESTMPPSG_GroupObtMarks;
                                                obj_c.ESTMPPSG_GroupObtGrade = result1_grade.Count > 0 ? result1_grade[0].EMGD_Name : null;
                                                obj_c.ESTMPPSG_GradePoints = result1_grade.Count > 0 ? result1_grade[0].EMGD_GradePoints : null;
                                                if (result1_grade.Count > 0)
                                                {
                                                    obj_c.EMGD_Id = result1_grade[0].EMGD_Id;
                                                }

                                                obj_c.ESTMPPSG_GroupTotalMarks = q.ESTMPPSG_GroupTotalMarks;
                                                obj_c.ESTMPPSG_GroupObtMarksOutOfGroupTotal = q.ESTMPPSG_GroupObtMarksOutOfGroupTotal;
                                                obj_c.ESTMPPSG_ObtMarksOutOfSubjectMaxMarks = q.ESTMPPSG_ObtMarksOutOfSubjectMaxMarks;
                                                obj_c.ESTMPPSG_GroupMarksGrade = result1_grade.Count > 0 ? result1_grade[0].EMGD_Name : null;
                                                obj_c.ESTMPPSG_GroupPercentage = per1;

                                                if (result2_grade.Count > 0)
                                                {
                                                    obj_c.EMGD_Id_GroupTotalMarks = result2_grade[0].EMGD_Id;
                                                }

                                                obj_c.CreatedDate = DateTime.Now;
                                                obj_c.UpdatedDate = DateTime.Now;
                                                _CumulativeReportContext.Add(obj_c);

                                                foreach (var r in TEmp_GroupWise_Exam_Marks)
                                                {
                                                    if (r.MI_Id == obj_p.MI_Id && r.ASMAY_Id == obj_p.ASMAY_Id && r.ASMCL_Id == obj_p.ASMCL_Id
                                                        && r.ASMS_Id == obj_p.ASMS_Id && r.AMST_Id == obj_p.AMST_Id && r.ISMS_Id == obj_p.ISMS_Id
                                                        && q.EMPSG_Id == r.EMPSG_Id)
                                                    {
                                                        Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO obj_groupexam = new Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO();

                                                        obj_groupexam.ESTMPPSG_Id = obj_c.ESTMPPSG_Id;
                                                        obj_groupexam.EME_Id = r.EME_Id;
                                                        obj_groupexam.ESTMPPSGE_ExamActualMarks = r.ESTMPPSGE_ExamActualMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamActualMaxMarks = r.ESTMPPSGE_ExamActualMaxMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedMarks = r.ESTMPPSGE_ExamConvertedMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedMaxMarks = r.ESTMPPSGE_ExamConvertedMaxMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedGrade = r.ESTMPPSGE_ExamConvertedGrade;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedPoints = r.ESTMPPSGE_ExamConvertedPoints;
                                                        obj_groupexam.ESTMPPSGE_ActiveFlg = true;
                                                        obj_groupexam.ESTMPPSGE_ExamPassFailFlag = r.ESTMPPSGE_ExamPassFailFlag;
                                                        obj_groupexam.EMGD_Id = r.EMGD_Id;
                                                        obj_groupexam.CreatedDate = DateTime.Now;
                                                        obj_groupexam.UpdatedDate = DateTime.Now;
                                                        _CumulativeReportContext.Add(obj_groupexam);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var contactExists = _CumulativeReportContext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            go_stop = true;

                            using (var cmd = _CumulativeReportContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Promotion_StudentCS_CACH";
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                try
                                {
                                    var a = cmd.ExecuteNonQuery();
                                    if (a >= 1)
                                    {
                                        go_stop = true;
                                    }
                                    else
                                    {
                                        go_stop = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    go_stop = false;
                                    _acdimpl.LogError(ex.Message);
                                    _acdimpl.LogDebug(ex.Message);
                                }
                            }
                        }
                        else
                        {
                            go_stop = false;
                        }
                    }

                    else if (promotion_type == "M")
                    {
                        //var already_details1 = _CumulativeReportContext.Exm_Stu_MP_Promo_SubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).Distinct().ToList();//&& t.AMST_Id == data.AMST_Id

                        //var already_details2 = (from a in _CumulativeReportContext.Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO
                        //                        from b in already_details1
                        //                        where (a.ESTMPPS_Id == b.ESTMPPS_Id)
                        //                        select a).Distinct().ToList();

                        //var already_details3 = (from a in _CumulativeReportContext.Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO
                        //                        from b in already_details2
                        //                        where (a.ESTMPPSG_Id == b.ESTMPPSG_Id)
                        //                        select a).Distinct().ToList();


                        //if (already_details3.Any())
                        //{
                        //    for (int i = 0; already_details3.Count > i; i++)
                        //    {
                        //        _CumulativeReportContext.Remove(already_details3.ElementAt(i));
                        //    }
                        //}

                        //if (already_details2.Any())
                        //{
                        //    for (int i = 0; already_details2.Count > i; i++)
                        //    {
                        //        _CumulativeReportContext.Remove(already_details2.ElementAt(i));
                        //    }
                        //}

                        //if (already_details1.Any())
                        //{
                        //    for (int i = 0; already_details1.Count > i; i++)
                        //    {
                        //        _CumulativeReportContext.Remove(already_details1.ElementAt(i));
                        //    }
                        //}

                        var outputval = _CumulativeReportContext.Database.ExecuteSqlCommand("Exm_PromotionDetails_Delete @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                        var EMP_Id = _CumulativeReportContext.Exm_M_PromotionDMO.Single(t => t.MI_Id == data.MI_Id && t.EYC_Id == EYC_Id && t.EMP_ActiveFlag == true).EMP_Id;

                        //var promotion_subectdetails_new = _CumulativeReportContext.Exm_M_Promotion_SubjectsDMO.Where(t => t.EMP_Id == EMP_Id).Distinct().ToList();

                        var student_list = (from a in _CumulativeReportContext.ExmStudentMarksProcessDMO
                                            from b in _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                            where (a.MI_Id == b.MI_Id && a.ASMAY_Id == b.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id
                                            && a.ASMS_Id == b.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                            && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                            select a).Select(t => t.AMST_Id).Distinct().ToList();

                        foreach (var stu_id in student_list)
                        {
                            data.AMST_Id = stu_id;
                                                      

                            List<TEmp_GroupWise_Exam_Marks> TEmp_GroupWise_Exam_Marks = new List<TEmp_GroupWise_Exam_Marks>();

                            List<TEmp_GroupWise_Marks> Groupwise_Details = new List<TEmp_GroupWise_Marks>();

                            var promotion_subectdetails = (from a in _CumulativeReportContext.Exm_M_Promotion_SubjectsDMO
                                                           from b in _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                                           where (a.ISMS_Id == b.ISMS_Id && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id
                                                           && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.EMP_Id == EMP_Id
                                                           && a.EMPS_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                                           select a).Distinct().ToList();

                            foreach (var s in promotion_subectdetails)
                            {
                                var EMGR_MarksPerFlag = _CumulativeReportContext.Exm_Master_GradeDMO.Single(t => t.MI_Id == data.MI_Id && t.EMGR_Id == s.EMGR_Id && t.EMGR_ActiveFlag == true).EMGR_MarksPerFlag;

                                var grade_details = _CumulativeReportContext.Exm_Master_Grade_DetailsDMO.Where(t => t.EMGR_Id == s.EMGR_Id && t.EMGD_ActiveFlag == true).Distinct().ToList();

                                var stu_marks_details = _CumulativeReportContext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.MI_Id == data.MI_Id
                                && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id
                                && t.ISMS_Id == s.ISMS_Id).Distinct().ToList();

                                if (stu_marks_details.Count > 0)
                                {
                                    var prom_subj_groupdetails = _CumulativeReportContext.Exm_M_Prom_Subj_GroupDMO.Where(t => t.EMPS_Id == s.EMPS_Id).Distinct().ToList();
                                    var flag = "";

                                    foreach (var w in prom_subj_groupdetails)
                                    {
                                        decimal? group_marks = 0;
                                        decimal? ESTMPPSG_GroupMaxMarks = 0;

                                        var prom_subj_grp_exms = _CumulativeReportContext.Exm_M_Prom_Subj_Group_ExamsDMO.Where(t => t.EMPSGE_ActiveFlg == true && t.EMPSG_Id == w.EMPSG_Id).Distinct().ToList();

                                        List<decimal?> exms_marks_grpwise = new List<decimal?>();
                                        List<decimal?> exms_max_marks_grpwise = new List<decimal?>();

                                        List<decimal?> exms_marks_grpwise_Temp = new List<decimal?>();
                                        List<decimal?> exms_max_marks_grpwise_Temp = new List<decimal?>();


                                        int subexamgrp = 0;

                                        foreach (var z in prom_subj_grp_exms)
                                        {
                                            decimal? raitoexammarks = 0.00m;
                                            decimal? convertedexammarks = 0.00m;
                                            decimal? convertedexam_max_marks = 0.00m;

                                            decimal? raitoexammarks_temp = 0.00m;
                                            decimal? convertedexammarks_temp = 0.00m;
                                            decimal? convertedexam_max_marks_temp = 0.00m;

                                            var examForMaxMarkrs = z.EMPSGE_ForMaxMarkrs;

                                            var result = stu_marks_details.Where(t => t.EME_Id == z.EME_Id && t.ESTMPS_PassFailFlg != "OD").Distinct().ToList();
                                            if (result.Count > 0)
                                            {
                                                subexamgrp = 1;
                                             
                                                var subj_max_exm_wise = stu_marks_details.Where(t => t.EME_Id == z.EME_Id).FirstOrDefault().ESTMPS_Medical_MaxMarks;
                                                var subj_flg_exm_wise = stu_marks_details.Where(t => t.EME_Id == z.EME_Id).FirstOrDefault().ESTMPS_PassFailFlg;
                                                var subj_obt_exm_wise = stu_marks_details.Where(t => t.EME_Id == z.EME_Id).FirstOrDefault().ESTMPS_ObtainedMarks;
                                                //var subj_max_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_Medical_MaxMarks;
                                                //var subj_flg_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_PassFailFlg;
                                                //var subj_obt_exm_wise = stu_marks_details.Single(t => t.EME_Id == z.EME_Id).ESTMPS_ObtainedMarks;
                                                var subj_max_exm_wise_temp = subj_max_exm_wise;

                                                decimal? groupwise_marks = 0;
                                                decimal? groupwise_maxmarks = 0;

                                                if (subj_flg_exm_wise != "OD")
                                                {
                                                    groupwise_marks = subj_obt_exm_wise;
                                                }
                                                flag = subj_flg_exm_wise;

                                                //Convertion To Exam Wise
                                                if (z.EMPSGE_ConvertionReqOrNot == true)
                                                {
                                                    // 30 > 10
                                                    if (subj_max_exm_wise > z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        if (subj_max_exm_wise > 0)
                                                        {
                                                            raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                            convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                            convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;
                                                        }
                                                    }
                                                    //10 < 30
                                                    else if (subj_max_exm_wise < z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        if (subj_max_exm_wise > 0)
                                                        {
                                                            raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                            convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                            convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;
                                                        }
                                                    }
                                                    // 10 = 10
                                                    else if (subj_max_exm_wise == z.EMPSGE_ForMaxMarkrs)
                                                    {
                                                        if (subj_max_exm_wise > 0)
                                                        {
                                                            raitoexammarks = z.EMPSGE_ForMaxMarkrs / subj_max_exm_wise;
                                                            convertedexammarks = subj_obt_exm_wise * raitoexammarks;
                                                            convertedexam_max_marks = subj_max_exm_wise * raitoexammarks;
                                                        }                                                        
                                                    }
                                                    subj_max_exm_wise_temp = convertedexam_max_marks;
                                                }
                                                else
                                                {
                                                    convertedexammarks = subj_obt_exm_wise;
                                                }

                                                // Convertion To Group Marks For Best Marks 

                                                if (w.EMPSG_MarksValue == subj_max_exm_wise)
                                                {
                                                    convertedexammarks_temp = subj_obt_exm_wise;
                                                    convertedexam_max_marks_temp = subj_max_exm_wise;
                                                }

                                                else if (w.EMPSG_MarksValue > subj_max_exm_wise)
                                                {
                                                    if (subj_max_exm_wise > 0)
                                                    {
                                                        raitoexammarks_temp = w.EMPSG_MarksValue / subj_max_exm_wise;
                                                        convertedexammarks_temp = subj_obt_exm_wise * raitoexammarks_temp;
                                                        convertedexam_max_marks_temp = subj_max_exm_wise * raitoexammarks_temp;
                                                    }
                                                }

                                                else if (w.EMPSG_MarksValue < subj_max_exm_wise)
                                                {
                                                    if (w.EMPSG_MarksValue > 0)
                                                    {
                                                        raitoexammarks_temp = subj_max_exm_wise / w.EMPSG_MarksValue;
                                                        convertedexammarks_temp = subj_obt_exm_wise * raitoexammarks_temp;
                                                        convertedexam_max_marks_temp = subj_max_exm_wise * raitoexammarks_temp;
                                                    }                                                   
                                                }


                                                if (getconfing.FirstOrDefault().ExmConfig_RoundoffFlag == true)
                                                {
                                                    convertedexammarks = Math.Round(Convert.ToDecimal(convertedexammarks), 0, MidpointRounding.AwayFromZero);
                                                }

                                                if (w.EMPSG_RoundOffFlag == true)
                                                {
                                                    convertedexammarks = Math.Round(Convert.ToDecimal(convertedexammarks), 0, MidpointRounding.AwayFromZero);
                                                }

                                                groupwise_maxmarks = z.EMPSGE_ForMaxMarkrs;

                                                exms_marks_grpwise.Add(convertedexammarks);
                                                exms_max_marks_grpwise.Add(subj_max_exm_wise_temp);


                                                exms_marks_grpwise_Temp.Add(convertedexammarks_temp);
                                                exms_max_marks_grpwise_Temp.Add(convertedexam_max_marks_temp);


                                                var resultexam_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                                if (EMGR_MarksPerFlag == "M")
                                                {
                                                    resultexam_grade = grade_details.Where(t => (convertedexammarks >= t.EMGD_From
                                                    && convertedexammarks <= t.EMGD_To) || (convertedexammarks <= t.EMGD_From
                                                    && convertedexammarks >= t.EMGD_To)).Distinct().ToList();
                                                }
                                                else if (EMGR_MarksPerFlag == "P")
                                                {
                                                    decimal? per = 0;
                                                    if (examForMaxMarkrs > 0)
                                                    {
                                                        per = (convertedexammarks / examForMaxMarkrs) * 100;
                                                    }                                                   
                                                    resultexam_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                    || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                                }
                                                int? EMGD_Id = null;
                                                if (resultexam_grade.Count > 0)
                                                {
                                                    EMGD_Id = resultexam_grade[0].EMGD_Id;
                                                }

                                                TEmp_GroupWise_Exam_Marks.Add(new TEmp_GroupWise_Exam_Marks
                                                {
                                                    MI_Id = data.MI_Id,
                                                    ASMAY_Id = data.ASMAY_Id,
                                                    ASMCL_Id = data.ASMCL_Id,
                                                    ASMS_Id = data.ASMS_Id,
                                                    AMST_Id = data.AMST_Id,
                                                    ISMS_Id = s.ISMS_Id,
                                                    EMPSG_Id = w.EMPSG_Id,
                                                    EME_Id = z.EME_Id,
                                                    ESTMPPSGE_ExamActualMarks = subj_obt_exm_wise,
                                                    ESTMPPSGE_ExamActualMaxMarks = subj_max_exm_wise,
                                                    ESTMPPSGE_ExamConvertedMarks = convertedexammarks,
                                                    ESTMPPSGE_ExamConvertedMaxMarks = subj_max_exm_wise_temp,
                                                    ESTMPPSGE_ExamPassFailFlag = subj_flg_exm_wise,
                                                    ESTMPPSGE_ExamConvertedGrade = resultexam_grade.Count > 0 ?
                                                    resultexam_grade[0].EMGD_Name : null,
                                                    ESTMPPSGE_ExamConvertedPoints = resultexam_grade.Count > 0 ?
                                                    resultexam_grade[0].EMGD_GradePoints : null,
                                                    EMGD_Id = EMGD_Id
                                                });
                                            }
                                        }

                                        var Best_off = w.EMPSG_BestOff;

                                        var best_marks_GroupTotal = exms_marks_grpwise.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var best_maxmarks_GroupTotal = exms_max_marks_grpwise.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var avg_marks_GroupTotal = best_marks_GroupTotal.Sum();
                                        var avg_maxmarks_GroupTotal = best_maxmarks_GroupTotal.Sum();

                                        decimal? avg_percentage_GroupTotal = 0.00m;
                                        if (avg_maxmarks_GroupTotal > 0)
                                        {
                                            avg_percentage_GroupTotal = (avg_marks_GroupTotal * s.EMPS_MaxMarks / avg_maxmarks_GroupTotal);
                                        }

                                        var best_marks = exms_marks_grpwise_Temp.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var best_maxmarks = exms_max_marks_grpwise_Temp.OrderByDescending(t => t).Take(Best_off).ToList();
                                        var avg_marks = best_marks.Average();
                                        var avg_maxmarks = best_maxmarks.Average();

                                        if (promotion_type == "P")
                                        {
                                            group_marks = avg_marks * (w.EMPSG_PercentValue) / 100;
                                            ESTMPPSG_GroupMaxMarks = ((w.EMPSG_PercentValue) / (s.EMPS_MaxMarks)) * 100;
                                        }
                                        else
                                        {
                                            if (avg_maxmarks > 0)
                                            {
                                                var ratio = w.EMPSG_MarksValue / avg_maxmarks;
                                                group_marks = avg_marks * ratio;
                                                ESTMPPSG_GroupMaxMarks = w.EMPSG_MarksValue;
                                            }                                           
                                        }

                                        if (getconfing.FirstOrDefault().ExmConfig_RoundoffFlag == true)
                                        {
                                            group_marks = Math.Round(Convert.ToDecimal(group_marks), 0, MidpointRounding.AwayFromZero);
                                        }

                                        if (w.EMPSG_RoundOffFlag == true)
                                        {
                                            group_marks = Math.Round(Convert.ToDecimal(group_marks), 0, MidpointRounding.AwayFromZero);
                                        }

                                        if (subexamgrp == 1)
                                        {
                                            Groupwise_Details.Add(new TEmp_GroupWise_Marks
                                            {
                                                MI_Id = data.MI_Id,
                                                ASMAY_Id = data.ASMAY_Id,
                                                ASMCL_Id = data.ASMCL_Id,
                                                ASMS_Id = data.ASMS_Id,
                                                AMST_Id = data.AMST_Id,
                                                ISMS_Id = s.ISMS_Id,
                                                EMPSG_Id = w.EMPSG_Id,
                                                ESTMPPSG_GroupMaxMarks = ESTMPPSG_GroupMaxMarks,
                                                ESTMPPSG_GroupObtMarks = group_marks,
                                                ESTMPPSG_GroupObtMarksOutOfGroupTotal = avg_marks_GroupTotal,
                                                ESTMPPSG_GroupTotalMarks = avg_marks_GroupTotal,
                                                ESTMPPSG_ObtMarksOutOfSubjectMaxMarks = avg_percentage_GroupTotal,
                                            });
                                        }
                                    }

                                    var sub_marks_total = Groupwise_Details.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.AMST_Id == data.AMST_Id && t.ISMS_Id == s.ISMS_Id).GroupBy(t => t.ISMS_Id).Select(a => new { Sum_obtain = a.Sum(t => t.ESTMPPSG_GroupObtMarks), Sum_max = a.Sum(t => t.ESTMPPSG_GroupMaxMarks) }).ToList();

                                    if (sub_marks_total.Count > 0)
                                    {
                                        Exm_Stu_MP_Promo_SubjectwiseDMO obj_p = new Exm_Stu_MP_Promo_SubjectwiseDMO();
                                        obj_p.MI_Id = data.MI_Id;
                                        obj_p.ASMAY_Id = data.ASMAY_Id;
                                        obj_p.ASMCL_Id = data.ASMCL_Id;
                                        obj_p.ASMS_Id = data.ASMS_Id;
                                        obj_p.AMST_Id = data.AMST_Id;
                                        obj_p.ISMS_Id = s.ISMS_Id;
                                        //for conversion of marks ........
                                        if (s.EMPS_MaxMarks == s.EMPS_ConvertForMarks)
                                        {
                                            var result_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                            if (EMGR_MarksPerFlag == "M")
                                            {
                                                result_grade = grade_details.Where(t => (sub_marks_total[0].Sum_obtain >= t.EMGD_From
                                                && sub_marks_total[0].Sum_obtain <= t.EMGD_To) || (sub_marks_total[0].Sum_obtain <= t.EMGD_From
                                                && sub_marks_total[0].Sum_obtain >= t.EMGD_To)).Distinct().ToList();
                                            }
                                            else if (EMGR_MarksPerFlag == "P")
                                            {
                                                decimal? per = 0;
                                                if (sub_marks_total[0].Sum_max > 0)
                                                {
                                                    per = (sub_marks_total[0].Sum_obtain / sub_marks_total[0].Sum_max) * 100;
                                                }
                                                
                                                result_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            obj_p.ESTMPPS_MaxMarks = sub_marks_total[0].Sum_max;
                                            obj_p.ESTMPPS_ObtainedMarks = sub_marks_total[0].Sum_obtain;
                                            obj_p.ESTMPPS_ObtainedGrade = result_grade.Count > 0 ? result_grade[0].EMGD_Name : null;
                                            obj_p.ESTMPPS_PassFailFlg = flag == "" ? ((s.EMPS_MinMarks) > obj_p.ESTMPPS_ObtainedMarks ? "Fail" : "Pass") : flag;
                                            obj_p.ESTMPPS_Remarks = result_grade.Count > 0 ? result_grade[0].EMGD_Remarks : null;
                                            obj_p.ESTMPPS_GradePoints = result_grade.Count > 0 ? result_grade[0].EMGD_GradePoints : null;
                                            if (result_grade.Count > 0)
                                            {
                                                obj_p.EMGD_Id = result_grade[0].EMGD_Id;
                                            }
                                        }
                                        else
                                        {
                                            decimal? convert_ratio = 0.00m;
                                            if (s.EMPS_MaxMarks > 0)
                                            {
                                                convert_ratio = s.EMPS_ConvertForMarks / s.EMPS_MaxMarks;
                                            }
                                            obj_p.ESTMPPS_MaxMarks = (sub_marks_total[0].Sum_max * convert_ratio);
                                            obj_p.ESTMPPS_ObtainedMarks = (sub_marks_total[0].Sum_obtain * convert_ratio);
                                            if (getconfing.FirstOrDefault().ExmConfig_RoundoffFlag == true)
                                            {
                                                obj_p.ESTMPPS_ObtainedMarks = Math.Round(Convert.ToDecimal(obj_p.ESTMPPS_ObtainedMarks),
                                                    0, MidpointRounding.AwayFromZero);
                                            }

                                            var result_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                            if (EMGR_MarksPerFlag == "M")

                                            {
                                                result_grade = grade_details.Where(t => (obj_p.ESTMPPS_ObtainedMarks >= t.EMGD_From
                                                && obj_p.ESTMPPS_ObtainedMarks <= t.EMGD_To) || (obj_p.ESTMPPS_ObtainedMarks <= t.EMGD_From
                                                && obj_p.ESTMPPS_ObtainedMarks >= t.EMGD_To)).Distinct().ToList();
                                            }
                                            else if (EMGR_MarksPerFlag == "P")
                                            {
                                                decimal? per = 0;
                                                if (obj_p.ESTMPPS_MaxMarks > 0)
                                                {
                                                    per = (obj_p.ESTMPPS_ObtainedMarks / obj_p.ESTMPPS_MaxMarks) * 100;
                                                }                                                
                                                result_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                            }

                                            obj_p.ESTMPPS_ObtainedGrade = result_grade.Count > 0 ? result_grade[0].EMGD_Name : null;
                                            obj_p.ESTMPPS_PassFailFlg = flag == "" ? ((s.EMPS_MinMarks) > obj_p.ESTMPPS_ObtainedMarks ? "Fail" : "Pass") : flag;
                                            obj_p.ESTMPPS_Remarks = result_grade.Count > 0 ? result_grade[0].EMGD_Remarks : null;
                                            obj_p.ESTMPPS_GradePoints = result_grade.Count > 0 ? result_grade[0].EMGD_GradePoints : null;
                                            if (result_grade.Count > 0)
                                            {
                                                obj_p.EMGD_Id = result_grade[0].EMGD_Id;
                                            }
                                        }
                                        obj_p.CreatedDate = DateTime.Now;
                                        obj_p.UpdatedDate = DateTime.Now;
                                        _CumulativeReportContext.Add(obj_p);

                                        foreach (var q in Groupwise_Details)
                                        {
                                            decimal? per1 = 0;

                                            if (q.MI_Id == obj_p.MI_Id && q.ASMAY_Id == obj_p.ASMAY_Id && q.ASMCL_Id == obj_p.ASMCL_Id && q.ASMS_Id == obj_p.ASMS_Id
                                                && q.AMST_Id == obj_p.AMST_Id && q.ISMS_Id == obj_p.ISMS_Id)
                                            {
                                                var result1_grade = new List<Exm_Master_Grade_DetailsDMO>();
                                                var result2_grade = new List<Exm_Master_Grade_DetailsDMO>();

                                                if (EMGR_MarksPerFlag == "M")
                                                {
                                                    result1_grade = grade_details.Where(t => (q.ESTMPPSG_GroupObtMarks >= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarks <= t.EMGD_To) || (q.ESTMPPSG_GroupObtMarks <= t.EMGD_From
                                                    && q.ESTMPPSG_GroupObtMarks >= t.EMGD_To)).Distinct().ToList();

                                                    result2_grade = grade_details.Where(t => (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal >= t.EMGD_From
                                                 && q.ESTMPPSG_GroupObtMarksOutOfGroupTotal <= t.EMGD_To) || (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal <= t.EMGD_From
                                                 && q.ESTMPPSG_GroupObtMarksOutOfGroupTotal >= t.EMGD_To)).Distinct().ToList();
                                                }
                                                else if (EMGR_MarksPerFlag == "P")
                                                {
                                                    decimal? per = 0;
                                                    if (q.ESTMPPSG_GroupMaxMarks > 0)
                                                    {
                                                        per = (q.ESTMPPSG_GroupObtMarks / q.ESTMPPSG_GroupMaxMarks) * 100;
                                                    }
                                                    result1_grade = grade_details.Where(t => (per >= t.EMGD_From && per <= t.EMGD_To)
                                                    || (per <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();

                                                    if (q.ESTMPPSG_GroupTotalMarks > 0)
                                                    {
                                                        per1 = (q.ESTMPPSG_GroupObtMarksOutOfGroupTotal / q.ESTMPPSG_GroupTotalMarks) * 100;
                                                    }
                                                    else
                                                    {
                                                        per1 = 0;
                                                    }
                                                    
                                                    result2_grade = grade_details.Where(t => (per1 >= t.EMGD_From && per1 <= t.EMGD_To)
                                                    || (per1 <= t.EMGD_From && per >= t.EMGD_To)).Distinct().ToList();
                                                }


                                                Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO obj_c = new Exm_Stu_MP_Promo_Subjectwise_GroupwiseDMO();
                                                obj_c.ESTMPPS_Id = obj_p.ESTMPPS_Id;
                                                obj_c.EMPSG_Id = q.EMPSG_Id;
                                                obj_c.ESTMPPSG_GroupMaxMarks = q.ESTMPPSG_GroupMaxMarks;
                                                obj_c.ESTMPPSG_GroupObtMarks = q.ESTMPPSG_GroupObtMarks;
                                                obj_c.ESTMPPSG_GroupObtGrade = result1_grade.Count > 0 ? result1_grade[0].EMGD_Name : null;
                                                obj_c.ESTMPPSG_GradePoints = result1_grade.Count > 0 ? result1_grade[0].EMGD_GradePoints : null;
                                                if (result1_grade.Count > 0)
                                                {
                                                    obj_c.EMGD_Id = result1_grade[0].EMGD_Id;
                                                }

                                                obj_c.ESTMPPSG_GroupMarksGrade = result2_grade.Count > 0 ? result2_grade[0].EMGD_Name : null;
                                                obj_c.ESTMPPSG_GroupTotalMarks = q.ESTMPPSG_GroupTotalMarks;
                                                obj_c.ESTMPPSG_GroupObtMarksOutOfGroupTotal = q.ESTMPPSG_GroupObtMarksOutOfGroupTotal;
                                                obj_c.ESTMPPSG_ObtMarksOutOfSubjectMaxMarks = q.ESTMPPSG_ObtMarksOutOfSubjectMaxMarks;
                                                obj_c.ESTMPPSG_GroupPercentage = per1;

                                                if (result2_grade.Count > 0)
                                                {
                                                    obj_c.EMGD_Id_GroupTotalMarks = result2_grade[0].EMGD_Id;
                                                }
                                                obj_c.CreatedDate = DateTime.Now;
                                                obj_c.UpdatedDate = DateTime.Now;
                                                _CumulativeReportContext.Add(obj_c);

                                                foreach (var r in TEmp_GroupWise_Exam_Marks)
                                                {
                                                    if (r.MI_Id == obj_p.MI_Id && r.ASMAY_Id == obj_p.ASMAY_Id && r.ASMCL_Id == obj_p.ASMCL_Id
                                                        && r.ASMS_Id == obj_p.ASMS_Id && r.AMST_Id == obj_p.AMST_Id && r.ISMS_Id == obj_p.ISMS_Id
                                                        && q.EMPSG_Id == r.EMPSG_Id)
                                                    {
                                                        Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO obj_groupexam = new Exm_Stu_MP_Promo_Subject_Groupwise_ExamDMO();

                                                        obj_groupexam.ESTMPPSG_Id = obj_c.ESTMPPSG_Id;
                                                        obj_groupexam.EME_Id = r.EME_Id;
                                                        obj_groupexam.ESTMPPSGE_ExamActualMarks = r.ESTMPPSGE_ExamActualMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamActualMaxMarks = r.ESTMPPSGE_ExamActualMaxMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedMarks = r.ESTMPPSGE_ExamConvertedMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedMaxMarks = r.ESTMPPSGE_ExamConvertedMaxMarks;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedGrade = r.ESTMPPSGE_ExamConvertedGrade;
                                                        obj_groupexam.ESTMPPSGE_ExamConvertedPoints = r.ESTMPPSGE_ExamConvertedPoints;
                                                        obj_groupexam.ESTMPPSGE_ActiveFlg = true;
                                                        obj_groupexam.ESTMPPSGE_ExamPassFailFlag = r.ESTMPPSGE_ExamPassFailFlag;
                                                        obj_groupexam.EMGD_Id = r.EMGD_Id;
                                                        obj_groupexam.CreatedDate = DateTime.Now;
                                                        obj_groupexam.UpdatedDate = DateTime.Now;
                                                        _CumulativeReportContext.Add(obj_groupexam);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        var contactExists = _CumulativeReportContext.SaveChanges();
                        if (contactExists >= 1)
                        {
                            go_stop = true;

                            using (var cmd = _CumulativeReportContext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "Promotion_StudentCS_CACH";
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

                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();
                                try
                                {
                                    var a = cmd.ExecuteNonQuery();
                                    if (a >= 1)
                                    {
                                        go_stop = true;
                                    }
                                    else
                                    {
                                        go_stop = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    go_stop = false;
                                    _acdimpl.LogError(ex.Message);
                                    _acdimpl.LogDebug(ex.Message);
                                }
                            }

                        }
                        else
                        {
                            go_stop = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return go_stop;
        }
    }
}