using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.Exam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamMarksProcessConditionsImpl : Interfaces.ExamMarksprocessconditionInterface
    {
        private static ConcurrentDictionary<string, ExamMarksProcess_DTO> _login =
         new ConcurrentDictionary<string, ExamMarksProcess_DTO>();

        private ExamContext _examcontext;
        ILogger<ExamMarksProcessConditionsImpl> _acdimpl;
        public ExamMarksProcessConditionsImpl(ExamContext masterexamContext, ILogger<ExamMarksProcessConditionsImpl> _acdim)
        {
            _examcontext = masterexamContext;
            _acdimpl = _acdim;
        }
        public ExamMarksProcess_DTO Getdetails(ExamMarksProcess_DTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _examcontext.AcademicYear.Where(y => y.MI_Id == data.MI_Id && y.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.yearlist = year.Distinct().ToArray();


                data.category_exams = (from a in _examcontext.AcademicYear
                                       from b in _examcontext.Exm_Master_CategoryDMO
                                       from c in _examcontext.exammasterDMO
                                       from d in _examcontext.Exm_Yearly_CategoryDMO
                                       from e in _examcontext.Exm_Yearly_Category_ExamsDMO
                                       from f in _examcontext.Exm_Master_GradeDMO
                                       where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == d.MI_Id && a.MI_Id == c.MI_Id && f.MI_Id == a.MI_Id && a.ASMAY_Id == d.ASMAY_Id && d.EMCA_Id == b.EMCA_Id && d.EYC_Id == e.EYC_Id && c.EME_Id == e.EME_Id && e.EMGR_Id == f.EMGR_Id)
                                       select new ExamMarksProcess_DTO
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
                                           EYCE_ExamStartDate = e.EYCE_ExamStartDate,
                                           EYCE_ExamEndDate = e.EYCE_ExamEndDate,
                                           EYCE_MarksEntryLastDate = e.EYCE_MarksEntryLastDate,
                                           EYCE_MarksProcessLastDate = e.EYCE_MarksProcessLastDate,
                                           EYCE_MarksPublishDate = e.EYCE_MarksPublishDate,
                                       }).Distinct().OrderByDescending(t => t.EYCE_Id).ToArray();


                data.userlist = (from a in _examcontext.Staff_User_Login
                                 from b in _examcontext.HR_Master_Employee_DMO
                                 where (a.Emp_Code == b.HRME_Id && a.MI_Id == data.MI_Id)
                                 select new ExamMarksProcess_DTO
                                 {
                                     IVRMULF_Id = a.Id,
                                     HRME_EmployeeFirstName = ((a.IVRMSTAUL_UserName == null ? " " : a.IVRMSTAUL_UserName)),
                                     Emp_Code = a.Emp_Code

                                 }
                                 ).Distinct().ToArray();



                data.userPromotionlist = (from a in _examcontext.UserPromotion_DMO
                                          from b in _examcontext.Exm_Yearly_Category_ExamsDMO
                                          from c in _examcontext.exammasterDMO
                                          from d in _examcontext.Exm_Yearly_CategoryDMO
                                          from e in _examcontext.HR_Master_Employee_DMO
                                          from f in _examcontext.Exm_Master_CategoryDMO
                                          from h in _examcontext.Staff_User_Login
                                          from y in _examcontext.AcademicYear

                                          where (a.EYCE_Id == b.EYCE_Id && b.EME_Id == c.EME_Id && b.EYC_Id == d.EYC_Id && d.MI_Id == e.MI_Id && d.EMCA_Id == f.EMCA_Id && a.IVRMUL_Id == h.Id && e.HRME_Id == h.Emp_Code && c.MI_Id == d.MI_Id && d.ASMAY_Id == y.ASMAY_Id && d.MI_Id == data.MI_Id)
                                          select new ExamMarksProcess_DTO
                                          {
                                              EMCA_Id = d.EMCA_Id,
                                              ASMAY_Id = d.ASMAY_Id,
                                              ASMAY_Year = y.ASMAY_Year,
                                              EYCESU_Id = a.EYCESU_Id,
                                              EMCA_CategoryName = f.EMCA_CategoryName,
                                              EME_ExamName = c.EME_ExamName,
                                              EME_ExamCode = c.EME_ExamCode,
                                              EYC_Id = d.EYC_Id,
                                              EME_Id = c.EME_Id,
                                              EYCESU_MarksEntryFromDate = a.EYCESU_MarksEntryFromDate,
                                              EYCESU_MarksEntryToDate = a.EYCESU_MarksEntryToDate,
                                              EYCESU_MarksProcessFromDate = a.EYCESU_MarksProcessFromDate,
                                              EYCESU_MarksProcessToDate = a.EYCESU_MarksProcessToDate,
                                              EYCESU_MarksPublishDate = a.EYCESU_MarksPublishDate,
                                              //HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),
                                              HRME_EmployeeFirstName = ((h.IVRMSTAUL_UserName == null ? " " : h.IVRMSTAUL_UserName)),
                                              IVRMULF_Id = Convert.ToInt32(a.IVRMUL_Id),
                                              EYCESU_ActiveFlg = a.EYCESU_ActiveFlg,
                                          }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public ExamMarksProcess_DTO get_category(ExamMarksProcess_DTO data)
        {
            try
            {
                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_ActiveFlg == true)
                                     select new ExamMarksProcess_DTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName,
                                         EYC_Id = b.EYC_Id,

                                     }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public ExamMarksProcess_DTO get_subjects(ExamMarksProcess_DTO data)
        {
            try
            {
                data.subjectlist = (from a in _examcontext.Exm_Yearly_Category_GroupDMO
                                    from b in _examcontext.Exm_Yearly_Category_Group_SubjectsDMO
                                    from c in _examcontext.IVRM_School_Master_SubjectsDMO
                                    where (c.MI_Id == data.MI_Id && a.EYC_Id == data.EYC_Id && a.EYCG_Id == b.EYCG_Id && b.ISMS_Id == c.ISMS_Id && b.EYCGS_ActiveFlg == true && c.ISMS_ActiveFlag == 1 && c.ISMS_ExamFlag == 1 && a.EYCG_ActiveFlg == true)
                                    select new ExamMarksProcess_DTO
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

                List<int> exams = new List<int>();
                exams = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id).Select(t => t.EME_Id).ToList();

                data.examlist = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && exams.Contains(t.EME_Id)).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee);
            }
            return data;
        }
        public ExamMarksProcess_DTO savedetails(ExamMarksProcess_DTO data)
        {
            try
            {

                var geteycid = _examcontext.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == data.EMCA_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true).ToList();

                var geteyceid = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(a => a.EME_Id == data.EME_Id && a.EYC_Id == data.EYC_Id
                && a.EYCE_ActiveFlg == true).ToList();

                data.EYCE_Id = geteyceid.FirstOrDefault().EYCE_Id;

                if (data.EYCE_Id == 0)
                {

                }
                else if (data.EYCE_Id > 0)
                {
                    var duplicate = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id && t.EYCE_Id != data.EYCE_Id && t.EME_Id == data.EME_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var examCatg = _examcontext.Exm_Yearly_Category_ExamsDMO.Single(t => t.EYCE_Id == data.EYCE_Id);
                        examCatg.EYCE_Id = data.EYCE_Id;

                        examCatg.EYC_Id = data.EYC_Id;
                        examCatg.EME_Id = data.EME_Id;
                        examCatg.UpdatedDate = DateTime.Now;
                        examCatg.EYCE_ExamStartDate = data.EYCE_ExamStartDate;
                        examCatg.EYCE_ExamEndDate = data.EYCE_ExamEndDate;
                        examCatg.EYCE_MarksEntryLastDate = data.EYCE_MarksEntryLastDate;
                        examCatg.EYCE_MarksProcessLastDate = data.EYCE_MarksProcessLastDate;
                        examCatg.EYCE_MarksPublishDate = data.EYCE_MarksPublishDate;

                        _examcontext.Update(examCatg);
                        int row = _examcontext.SaveChanges();
                        if (row > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

        public ExamMarksProcess_DTO deactivate(ExamMarksProcess_DTO data)
        {
            try
            {


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public ExamMarksProcess_DTO editdetails(ExamMarksProcess_DTO data)
        {
            try
            {

                //   var asmay_ids = _examcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_From_Date < DateTime.Now
                //&& t.ASMAY_To_Date > DateTime.Now).Distinct().Select(i => i.ASMAY_Id).ToList();
                //   data.ASMAY_Id = asmay_ids.FirstOrDefault();

                data.ASMAY_Id = data.ASMAY_Id;

                data.edit_cat_exm = (from a in _examcontext.Exm_Yearly_Category_ExamsDMO
                                         //from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.EYCE_Id == data.EYCE_Id)
                                     select new ExamMarksProcess_DTO
                                     {
                                         //ASMAY_Id = b.ASMAY_Id,
                                         EYC_Id = a.EYC_Id,
                                         // EMCA_Id = b.EMCA_Id,
                                         EME_Id = a.EME_Id,
                                         EMGR_Id = a.EMGR_Id,
                                         EYCE_AttendanceFromDate = a.EYCE_AttendanceFromDate,
                                         EYCE_AttendanceToDate = a.EYCE_AttendanceToDate,
                                         EYCE_SubExamFlg = a.EYCE_SubExamFlg,
                                         EYCE_SubSubjectFlg = a.EYCE_SubSubjectFlg,
                                         EYCE_Id = a.EYCE_Id,
                                         EYCE_ActiveFlg = a.EYCE_ActiveFlg,
                                         EYCE_ExamStartDate = a.EYCE_ExamStartDate,
                                         EYCE_ExamEndDate = a.EYCE_ExamEndDate,
                                         EYCE_MarksEntryLastDate = a.EYCE_MarksEntryLastDate,
                                         EYCE_MarksProcessLastDate = a.EYCE_MarksProcessLastDate,
                                         EYCE_MarksPublishDate = a.EYCE_MarksPublishDate,

                                     }).Distinct().ToArray();



                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id
                                     && b.EYC_ActiveFlg == true)
                                     select new ExamMarksProcess_DTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName,
                                         EYC_Id = b.EYC_Id,

                                     }).Distinct().ToArray();



                List<int> exams = new List<int>();
                exams = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id).Select(t => t.EME_Id).ToList();

                data.examlist = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && exams.Contains(t.EME_Id)).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public ExamMarksProcess_DTO get_exm_details(ExamMarksProcess_DTO data)
        {
            try
            {

                var examlist = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id && t.EME_Id == data.EME_Id).ToList();


                List<Exm_Yearly_Category_ExamsDMO> examlist1 = new List<Exm_Yearly_Category_ExamsDMO>();


                List<int> exams = new List<int>();
                exams = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id).Select(t => t.EME_Id).ToList();

                var examlistd = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && exams.Contains(t.EME_Id)).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();

                if (examlist1.FirstOrDefault().EYCE_ExamStartDate == null)
                {
                    var getselectedexamorder = _examcontext.masterexam.Where(a => a.MI_Id == data.MI_Id && a.EME_Id == data.EME_Id).ToList();

                    int selectedexamorder = getselectedexamorder.FirstOrDefault().EME_ExamOrder;

                    int getpreviousorder = selectedexamorder - 1;

                    if (getpreviousorder > 0)
                    {
                        var getpreviousexam = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true
                    && exams.Contains(t.EME_Id) && t.EME_ExamOrder == getpreviousorder).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();

                        while (getpreviousexam.Count() == 0)
                        {
                            getpreviousorder = getpreviousorder - 1;

                            getpreviousexam = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true
                            && exams.Contains(t.EME_Id) && t.EME_ExamOrder == getpreviousorder).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();
                        }

                        var getmindate = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id
                        && t.EME_Id == getpreviousexam.FirstOrDefault().EME_Id).ToList();

                        data.EYCE_MindateDate = getmindate.FirstOrDefault().EYCE_MarksPublishDate;
                    }
                }

                data.examlistdetails = examlist.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public ExamMarksProcess_DTO getalldetailsviewrecords(ExamMarksProcess_DTO page)
        {
            //ExamMarksProcess_DTO page = new ExamMarksProcess_DTO();
            try
            {
                page.view_exam_subjects = (from a in _examcontext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                           from b in _examcontext.Exm_Master_CategoryDMO
                                           from c in _examcontext.exammasterDMO
                                           from d in _examcontext.Exm_Yearly_CategoryDMO
                                           from e in _examcontext.Exm_Yearly_Category_ExamsDMO
                                           from f in _examcontext.Exm_Master_GradeDMO
                                           from g in _examcontext.IVRM_School_Master_SubjectsDMO
                                           where (a.EYCE_Id == e.EYCE_Id && e.EYC_Id == d.EYC_Id && d.EMCA_Id == b.EMCA_Id && b.MI_Id == d.MI_Id && b.MI_Id == f.MI_Id && c.MI_Id == b.MI_Id && a.ISMS_Id == g.ISMS_Id && a.EMGR_Id == f.EMGR_Id && a.EYCE_Id == page.EYCE_Id && e.EME_Id == c.EME_Id)
                                           select new ExamMarksProcess_DTO
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
                                               EYCES_ActiveFlg = a.EYCES_ActiveFlg


                                           }).Distinct().OrderBy(x => x.EYCES_SubjectOrder).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        //saveUserPromotionDataNew
        public ExamMarksProcess_DTO saveUserPromotionDataNew(ExamMarksProcess_DTO data)
        {
            try
            {
                int dulicateCount = 0;
                int savedcount = 0;
                if (data.ivrmulF_IdList != null)
                {
                    if (data.EYCESU_Id == 0)
                    {
                        foreach (var d in data.ivrmulF_IdList)
                        {
                            int EYCE_Ids = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id && t.EME_Id == data.EME_Id).Select(t => t.EYCE_Id).SingleOrDefault();
                            var Duplicate = _examcontext.UserPromotion_DMO.Where(t => t.IVRMUL_Id == d.IVRMULF_Id && t.EYCE_Id == EYCE_Ids).ToList();
                            if (Duplicate.Count > 0)
                            {
                                data.duplicate = true;
                                dulicateCount += 1;
                            }
                            else
                            {
                                if (EYCE_Ids > 0)
                                {
                                    UserPromotion_DMO obj = new UserPromotion_DMO();
                                    obj.EYCESU_Id = data.EYCESU_Id;
                                    obj.EYCE_Id = EYCE_Ids;
                                    obj.IVRMUL_Id = d.IVRMULF_Id;
                                    obj.EYCESU_MarksEntryFromDate = data.EYCESU_MarksEntryFromDate;
                                    obj.EYCESU_MarksEntryToDate = data.EYCESU_MarksEntryToDate;
                                    obj.EYCESU_MarksProcessFromDate = data.EYCESU_MarksProcessFromDate;
                                    obj.EYCESU_MarksProcessToDate = data.EYCESU_MarksProcessToDate;
                                    obj.EYCESU_MarksPublishDate = data.EYCESU_MarksPublishDate;
                                    obj.EYCESU_MarksPublishApproverFlg = data.EYCESU_MarksPublishApproverFlg;
                                    obj.EYCESU_ActiveFlg = true;
                                    obj.CreatedDate = DateTime.Now;
                                    obj.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj);

                                }


                            }

                        }
                        data.dulicateCount = dulicateCount;
                        int rowAffected = _examcontext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            rowAffected = savedcount;
                            data.savedcount = savedcount;
                            data.returnval = true;
                            data.duplicate = false;

                        }
                        else
                        {
                            data.returnval = false;
                            rowAffected = savedcount;
                            data.savedcount = savedcount;

                        }

                    }
                    else if (data.EYCESU_Id > 0)
                    {
                        int EYCE_Ids = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id && t.EME_Id == data.EME_Id).Select(t => t.EYCE_Id).SingleOrDefault();

                        var Duplicate = _examcontext.UserPromotion_DMO.Where(t => t.IVRMUL_Id == data.IVRMULF_Id && t.EYCESU_Id != data.EYCESU_Id && t.EYCE_Id == EYCE_Ids).ToList();
                        if (Duplicate.Count > 0)
                        {
                            data.duplicate = true;
                        }
                        else
                        {
                            var result = _examcontext.UserPromotion_DMO.Where(t => t.EYCESU_Id == data.EYCESU_Id).Single();


                            //result.EYCE_Id = data.EYCE_Id;
                            result.IVRMUL_Id = data.IVRMULF_Id;
                            result.EYCESU_MarksEntryFromDate = data.EYCESU_MarksEntryFromDate;
                            result.EYCESU_MarksEntryToDate = data.EYCESU_MarksEntryToDate;
                            result.EYCESU_MarksProcessFromDate = data.EYCESU_MarksProcessFromDate;
                            result.EYCESU_MarksProcessToDate = data.EYCESU_MarksProcessToDate;
                            result.EYCESU_MarksPublishDate = data.EYCESU_MarksPublishDate;
                            result.EYCESU_MarksPublishApproverFlg = data.EYCESU_MarksPublishApproverFlg;
                            result.UpdatedDate = DateTime.Now;
                            _examcontext.Update(result);
                            int rowAffected = _examcontext.SaveChanges();

                            if (rowAffected > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
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
        //saveUserPromotionDataNew
        public ExamMarksProcess_DTO saveUserPromotionData(ExamMarksProcess_DTO data)
        {
            try
            {


                if (data.EYCESU_Id == 0)
                {
                    int EYCE_Ids = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id && t.EME_Id == data.EME_Id).Select(t => t.EYCE_Id).SingleOrDefault();
                    var Duplicate = _examcontext.UserPromotion_DMO.Where(t => t.IVRMUL_Id == data.IVRMULF_Id && t.EYCE_Id == EYCE_Ids).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        if (EYCE_Ids > 0)
                        {
                            UserPromotion_DMO obj = new UserPromotion_DMO();
                            obj.EYCESU_Id = data.EYCESU_Id;
                            obj.EYCE_Id = EYCE_Ids;
                            obj.IVRMUL_Id = data.IVRMULF_Id;
                            obj.EYCESU_MarksEntryFromDate = data.EYCESU_MarksEntryFromDate;
                            obj.EYCESU_MarksEntryToDate = data.EYCESU_MarksEntryToDate;
                            obj.EYCESU_MarksProcessFromDate = data.EYCESU_MarksProcessFromDate;
                            obj.EYCESU_MarksProcessToDate = data.EYCESU_MarksProcessToDate;
                            obj.EYCESU_MarksPublishDate = data.EYCESU_MarksPublishDate;
                            obj.EYCESU_MarksPublishApproverFlg = data.EYCESU_MarksPublishApproverFlg;
                            obj.EYCESU_ActiveFlg = true;
                            obj.CreatedDate = DateTime.Now;
                            obj.UpdatedDate = DateTime.Now;
                            _examcontext.Add(obj);
                            int rowAffected = _examcontext.SaveChanges();

                            if (rowAffected > 0)
                            {
                                data.returnval = true;
                            }
                            else
                            {
                                data.returnval = false;
                            }

                        }


                    }
                }
                else if (data.EYCESU_Id > 0)
                {
                    int EYCE_Ids = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id && t.EME_Id == data.EME_Id).Select(t => t.EYCE_Id).SingleOrDefault();

                    var Duplicate = _examcontext.UserPromotion_DMO.Where(t => t.IVRMUL_Id == data.IVRMULF_Id && t.EYCESU_Id != data.EYCESU_Id && t.EYCE_Id == EYCE_Ids).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var result = _examcontext.UserPromotion_DMO.Where(t => t.EYCESU_Id == data.EYCESU_Id).Single();
                        //result.EYCE_Id = data.EYCE_Id;
                        result.IVRMUL_Id = data.IVRMULF_Id;
                        result.EYCESU_MarksEntryFromDate = data.EYCESU_MarksEntryFromDate;
                        result.EYCESU_MarksEntryToDate = data.EYCESU_MarksEntryToDate;
                        result.EYCESU_MarksProcessFromDate = data.EYCESU_MarksProcessFromDate;
                        result.EYCESU_MarksProcessToDate = data.EYCESU_MarksProcessToDate;
                        result.EYCESU_MarksPublishDate = data.EYCESU_MarksPublishDate;
                        result.EYCESU_MarksPublishApproverFlg = data.EYCESU_MarksPublishApproverFlg;
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                        int rowAffected = _examcontext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
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

        public ExamMarksProcess_DTO deActiveUserPromotion(ExamMarksProcess_DTO data)
        {
            try
            {
                var result = _examcontext.UserPromotion_DMO.Single(t => t.EYCESU_Id == data.EYCESU_Id);

                if (result.EYCESU_ActiveFlg == true)
                {
                    result.EYCESU_ActiveFlg = false;
                }
                else if (result.EYCESU_ActiveFlg == false)
                {
                    result.EYCESU_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _examcontext.Update(result);
                int rowAffected = _examcontext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ExamMarksProcess_DTO editUserPromotion(ExamMarksProcess_DTO data)
        {
            try
            {
                var result = _examcontext.UserPromotion_DMO.Where(t => t.EYCESU_Id == data.EYCESU_Id).SingleOrDefault();

                data.editPromotionUserlist = (from a in _examcontext.UserPromotion_DMO
                                              from b in _examcontext.Exm_Yearly_Category_ExamsDMO
                                              from c in _examcontext.exammasterDMO
                                              from d in _examcontext.Exm_Yearly_CategoryDMO
                                              from e in _examcontext.HR_Master_Employee_DMO
                                              from f in _examcontext.Exm_Master_CategoryDMO
                                              from h in _examcontext.Staff_User_Login
                                              from y in _examcontext.AcademicYear

                                              where (a.EYCE_Id == b.EYCE_Id && b.EME_Id == c.EME_Id && b.EYC_Id == d.EYC_Id && d.MI_Id == e.MI_Id && d.EMCA_Id == f.EMCA_Id && c.MI_Id == d.MI_Id && a.IVRMUL_Id == h.Id && e.HRME_Id == h.Emp_Code && d.ASMAY_Id == y.ASMAY_Id && d.MI_Id == data.MI_Id && a.EYCESU_Id == data.EYCESU_Id)
                                              select new ExamMarksProcess_DTO
                                              {
                                                  EMCA_Id = d.EMCA_Id,
                                                  ASMAY_Id = d.ASMAY_Id,
                                                  ASMAY_Year = y.ASMAY_Year,
                                                  EYCESU_Id = a.EYCESU_Id,
                                                  EMCA_CategoryName = f.EMCA_CategoryName,
                                                  EME_ExamName = c.EME_ExamName,
                                                  EME_ExamCode = c.EME_ExamCode,
                                                  EYC_Id = d.EYC_Id,
                                                  EME_Id = c.EME_Id,
                                                  EYCESU_MarksEntryFromDate = a.EYCESU_MarksEntryFromDate,
                                                  EYCESU_MarksEntryToDate = a.EYCESU_MarksEntryToDate,
                                                  EYCESU_MarksProcessFromDate = a.EYCESU_MarksProcessFromDate,
                                                  EYCESU_MarksProcessToDate = a.EYCESU_MarksProcessToDate,
                                                  EYCESU_MarksPublishDate = a.EYCESU_MarksPublishDate,
                                                  //HRME_EmployeeFirstName = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) + " " + (e.HRME_EmployeeMiddleName == null ? " " : e.HRME_EmployeeMiddleName) + " " + (e.HRME_EmployeeLastName == null ? " " : e.HRME_EmployeeLastName)).Trim(),

                                                  HRME_EmployeeFirstName = ((h.IVRMSTAUL_UserName == null ? " " : h.IVRMSTAUL_UserName)),
                                                  IVRMULF_Id = Convert.ToInt32(a.IVRMUL_Id),
                                                  EYCESU_ActiveFlg = a.EYCESU_ActiveFlg,
                                                  EYCESU_MarksPublishApproverFlg = a.EYCESU_MarksPublishApproverFlg
                                              }).Distinct().ToArray();

                data.categorylist = (from a in _examcontext.Exm_Master_CategoryDMO
                                     from b in _examcontext.Exm_Yearly_CategoryDMO
                                     where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && b.EYC_ActiveFlg == true)
                                     select new ExamMarksProcess_DTO
                                     {
                                         EMCA_Id = a.EMCA_Id,
                                         EMCA_CategoryName = a.EMCA_CategoryName,
                                         EYC_Id = b.EYC_Id,

                                     }).Distinct().ToArray();

                List<int> exams = new List<int>();
                exams = _examcontext.Exm_Yearly_Category_ExamsDMO.Where(t => t.EYC_Id == data.EYC_Id).Select(t => t.EME_Id).ToList();

                data.examlist = _examcontext.masterexam.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && exams.Contains(t.EME_Id)).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


    }
}
