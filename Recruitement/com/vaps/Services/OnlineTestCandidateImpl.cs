using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recruitment.com.vaps.Services
{
    public class OnlineTestCandidateImpl : Interfaces.OnlineTestCandidateInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public OnlineTestCandidateImpl(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        #region  MASTER EXAM
        //MASTER QUESTION
        #region LOAD QUESTION PAPER
        public OnlineTestCandidateDTO loadExamdata(OnlineTestCandidateDTO data)
        {
            try
            {
                data.HRMP_Id = 31;
                data.positionlist = _VMSContext.HR_Master_PositionDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMP_ActiveFlg == true).Distinct().OrderBy(e => e.HRMP_Position).ToArray();

                var exampaperlist = (from a in _VMSContext.OT_QuestionPaperTypeDMO
                                     from b in _VMSContext.HR_Master_PositionDMO
                                     where a.MI_Id == b.MI_Id && a.OTQPTYP_ActiveFlg == true && b.HRMP_ActiveFlg == true && a.MI_Id == b.MI_Id && a.HRMP_Id == b.HRMP_Id && a.HRMP_Id == data.HRMP_Id

                                     select new OnlineTestCandidateDTO
                                     {
                                         OTQPTYP_Id=a.OTQPTYP_Id,
                                         HRMP_Id = a.HRMP_Id,
                                         OTQPTYP_QuestionPaperName = a.OTQPTYP_QuestionPaperName,
                                         OTQPTYP_QuestionPaperDesc = a.OTQPTYP_QuestionPaperDesc,
                                    //  totalquestion = _VMSContext.OT_Master_OE_QuestionsDMO.Where(w => w.MI_Id == a.MI_Id && w.OTQPTYP_Id == a.OTQPTYP_Id && w.HRMP_Id == data.HRMP_Id && w.OTMOEQ_ActiveFlg == true).ToList().Count(), 
                             //            maxmarks= _VMSContext.OT_Master_OE_QuestionsDMO.Where(w => w.MI_Id == a.MI_Id && w.OTQPTYP_Id == a.OTQPTYP_Id && w.HRMP_Id == data.HRMP_Id && w.OTMOEQ_ActiveFlg == true).Distinct().ToList().Sum(e=>e.OTMOEQ_Marks)
                                     }).Distinct().ToList();


                data.getallexamdetails = exampaperlist.ToArray();
                data.gettodaysexamdetails = exampaperlist.ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region GET  QUESTIONS AND OPTIONS
        public OnlineTestCandidateDTO getQuestion(OnlineTestCandidateDTO data)
        {
            try
            {
                var getexamquestionlist = (from a in _VMSContext.OT_QuestionPaperTypeDMO
                                           from b in _VMSContext.OT_Master_OE_QuestionsDMO
                                           where (a.OTQPTYP_Id == b.OTQPTYP_Id && a.HRMP_Id == b.HRMP_Id && a.OTQPTYP_ActiveFlg == true
                                           && a.HRMP_Id == data.HRMP_Id && b.OTQPTYP_Id == data.OTQPTYP_Id && b.OTMOEQ_ActiveFlg==true)
                                           select new OnlineTestCandidateDTO
                                           {
                                               OTQPTYP_Id = a.OTQPTYP_Id,
                                               OTQPTYP_QuestionPaperName = a.OTQPTYP_QuestionPaperName,
                                               OTQPTYP_QuestionPaperDesc = a.OTQPTYP_QuestionPaperDesc,
                                               OTMOEQ_Id = b.OTMOEQ_Id,
                                               OTMOEQ_Question = b.OTMOEQ_Question,
                                               OTMOEQ_SubjectiveFlg = b.OTMOEQ_SubjectiveFlg
                                           }).Distinct().ToList();

                List<long> questionids = new List<long>();

                foreach (var c in getexamquestionlist)
                {
                    questionids.Add(c.OTMOEQ_Id);
                }

                data.getexamquestionlist = getexamquestionlist.ToArray();

                data.getquestionoptionlist = (from a in _VMSContext.OT_Master_OE_QuestionsDMO
                                              from b in _VMSContext.OT_Master_OE_QNS_OptionsDMO
                                              where (a.OTMOEQ_Id == b.OTMOEQ_Id && a.OTMOEQ_ActiveFlg == true && b.OTMOEQOA_ActiveFlg == true
                                               && questionids.Contains(b.OTMOEQ_Id) && a.MI_Id == data.MI_Id && a.HRMP_Id==data.HRMP_Id)
                                              select new OnlineTestCandidateDTO
                                              {
                                                  OTMOEQ_Id = a.OTMOEQ_Id,
                                                  OTMOEQOA_Id = b.OTMOEQOA_Id,
                                                  OTMOEQOA_Option = b.OTMOEQOA_Option,
                                                  OTMOEQOA_OptionCode = b.OTMOEQOA_OptionCode,
                                                  OTMOEQOA_AnswerFlag = b.OTMOEQOA_AnswerFlag,
                                              }).Distinct().OrderBy(a => a.OTMOEQ_Id).OrderBy(a => a.OTMOEQOA_OptionCode).ToArray();

               
            }
          
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        #endregion
        
        #region Save Or Update ANSWER
        public OnlineTestCandidateDTO Saveanswer(OnlineTestCandidateDTO data)
        {
            try
            {
                data.HRCD_Id = 164;
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkexamresult = _VMSContext.OT_Candidates_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.HRCD_Id == data.HRCD_Id  && a.OTCANDEX_Id == data.OTCANDEX_Id ).ToList();

                if (checkexamresult.Count > 0)
                {
                    var checkresult = _VMSContext.OT_Candidates_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.HRCD_Id == data.HRCD_Id && a.OTCANDEX_Id == data.OTCANDEX_Id);

                    checkresult.OTCANDEX_TotalTime = data.OTCANDEX_TotalTime;
                    checkresult.OTCANDEX_UpdatedBy = data.Userid;
                    checkresult.OTCANDEX_UpdatedDate = indiantime0;
                    _VMSContext.Update(checkresult);

                    if (data.OTMOEQ_SubjectiveFlg == false)
                    {

                        var checkquestionresult = _VMSContext.OT_Candidates_Exam_AnswerDMO.Where(a => a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id
                        && a.OTMOEQ_Id == data.saveanswerlsttemp[0].OTMOEQ_Id).ToList();

                        if (checkquestionresult.Count > 0)
                        {
                            var checkquestionansresult = _VMSContext.OT_Candidates_Exam_AnswerDMO.Single(a => a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id
                                && a.OTMOEQ_Id == data.saveanswerlsttemp[0].OTMOEQ_Id);

                            checkquestionansresult.OTMOEQOA_Id = data.saveanswerlsttemp[0].OTMOEQOA_Id;
                            checkquestionansresult.OTCANDEXANS_CorrectAnsFlg = data.saveanswerlsttemp[0].OTMOEQOA_AnswerFlag;
                            checkquestionansresult.OTCANDEXANS_UpdatedBy = data.Userid;
                            checkquestionansresult.OTCANDEXANS_UpdatedDate = indiantime0;
                            _VMSContext.Update(checkquestionansresult);

                        }
                        else
                        {
                            foreach (var c in data.saveanswerlsttemp)
                            {
                                OT_Candidates_Exam_AnswerDMO obj1 = new OT_Candidates_Exam_AnswerDMO();

                                obj1.OTCANDEX_Id = checkexamresult.FirstOrDefault().OTCANDEX_Id;
                                obj1.OTMOEQ_Id = c.OTMOEQ_Id;
                                obj1.OTMOEQOA_Id = c.OTMOEQOA_Id;
                                obj1.OTCANDEXANS_CorrectAnsFlg = c.OTMOEQOA_AnswerFlag;
                                obj1.OTCANDEXANS_ActiveFlg = true;
                                obj1.OTCANDEXANS_CreatedBy = data.Userid;
                                obj1.OTCANDEXANS_UpdatedBy = data.Userid;
                                obj1.OTCANDEXANS_CreatedDate = indiantime0;
                                obj1.OTCANDEXANS_UpdatedDate = indiantime0;
                                _VMSContext.Add(obj1);
                            }
                        }
                    }
                    else
                    {
                        var checkquestionsubjectiveresult = _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id
                        && a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id).ToList();

                        if (checkquestionsubjectiveresult.Count > 0)
                        {
                            var quessubjectiveresult = _VMSContext.OT_Candidates_Exam_SubjectiveAnswerDMO.Single(a => a.OTMOEQ_Id == data.OTMOEQ_Id
                            && a.OTCANDEX_Id == checkexamresult.FirstOrDefault().OTCANDEX_Id);

                            quessubjectiveresult.OTCANDEXSANS_Answer = data.OTCANDEXSANS_Answer; ;
                            quessubjectiveresult.OTCANDEXSANS_UpdatedDate = indiantime0;
                            quessubjectiveresult.OTCANDEXSANS_UpdatedBy = data.Userid;
                            _VMSContext.Update(quessubjectiveresult);

                        }
                        else
                        {
                            OT_Candidates_Exam_SubjectiveAnswerDMO obj2 = new OT_Candidates_Exam_SubjectiveAnswerDMO();
                            obj2.OTCANDEX_Id = checkexamresult.FirstOrDefault().OTCANDEX_Id;
                            obj2.OTCANDEXSANS_Answer = data.OTCANDEXSANS_Answer;
                            obj2.OTMOEQ_Id = data.OTMOEQ_Id;
                            obj2.OTCANDEXSANS_ActiveFlg = true;
                            obj2.OTCANDEXSANS_CreatedDate = indiantime0;
                            obj2.OTCANDEXSANS_CreatedBy = data.Userid;
                            obj2.OTCANDEXSANS_UpdatedDate = indiantime0;
                            obj2.OTCANDEXSANS_UpdatedBy = data.Userid;
                            _VMSContext.Add(obj2);
                        }
                    }
                    var i = _VMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Update";
                    }
                    else
                    {
                        data.message = "Failed";
                    }
                }
                else
                {
                    OT_Candidates_ExamDMO cobj = new OT_Candidates_ExamDMO();

                    cobj.MI_Id = data.MI_Id;
                    cobj.HRCD_Id = data.HRCD_Id;
                    cobj.OTCANDEX_TotalTime = data.OTCANDEX_TotalTime;
                    cobj.OTCANDEX_Date = indiantime0;
                    cobj.OTCANDEX_ActiveFlg = true;
                    cobj.OTCANDEX_CreatedBy = data.Userid;
                    cobj.OTCANDEX_UpdatedBy = data.Userid;
                    cobj.OTCANDEX_CreatedDate = indiantime0;
                    cobj.OTCANDEX_UpdatedDate = indiantime0;

                    _VMSContext.Add(cobj);

                    if (data.OTMOEQ_SubjectiveFlg == false)
                    {
                        foreach (var c in data.saveanswerlsttemp)
                        {
                            OT_Candidates_Exam_AnswerDMO obobj = new OT_Candidates_Exam_AnswerDMO();

                            obobj.OTCANDEX_Id = cobj.OTCANDEX_Id;
                            obobj.OTMOEQ_Id = c.OTMOEQ_Id;
                            obobj.OTMOEQOA_Id = c.OTMOEQOA_Id;
                            obobj.OTCANDEXANS_CorrectAnsFlg = c.OTMOEQOA_AnswerFlag;
                            obobj.OTCANDEXANS_ActiveFlg = true;
                            obobj.OTCANDEXANS_CreatedBy = data.Userid;
                            obobj.OTCANDEXANS_UpdatedBy = data.Userid;
                            obobj.OTCANDEXANS_CreatedDate = indiantime0;
                            obobj.OTCANDEXANS_UpdatedDate = indiantime0;
                            _VMSContext.Add(obobj);
                        }
                    }

                    else
                    {
                        OT_Candidates_Exam_SubjectiveAnswerDMO subobj = new OT_Candidates_Exam_SubjectiveAnswerDMO();
                        subobj.OTCANDEX_Id = cobj.OTCANDEX_Id;
                        subobj.OTMOEQ_Id = data.OTMOEQ_Id;
                        subobj.OTCANDEXSANS_Answer = data.OTCANDEXSANS_Answer;
                        subobj.OTCANDEXSANS_ActiveFlg = true;
                        subobj.OTCANDEXSANS_CreatedDate = indiantime0;
                        subobj.OTCANDEXSANS_CreatedBy = data.Userid;
                        subobj.OTCANDEXSANS_UpdatedDate = indiantime0;
                        subobj.OTCANDEXSANS_UpdatedBy = data.Userid;
                        _VMSContext.Add(subobj);
                    }
                    var i = _VMSContext.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Saved";
                    }
                    else
                    {
                        data.message = "Failed";
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region Edit Master Question
        public OnlineTestCandidateDTO EditMasterQuestion(OnlineTestCandidateDTO data)
        {
            try
            {
                var getquestiondetails = _VMSContext.OT_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.OTMOEQ_Id == data.OTMOEQ_Id).ToList();
                data.geteditmasterquestion = getquestiondetails.ToArray();

                data.HRMP_Id = getquestiondetails.FirstOrDefault().HRMP_Id;
                data.OTQPTYP_Id = getquestiondetails.FirstOrDefault().OTQPTYP_Id;

                data.getSavedOptions = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();

                data.papertypelist = (from a in _VMSContext.OT_QuestionPaperTypeDMO
                                      from b in _VMSContext.HR_Master_PositionDMO
                                      where a.MI_Id == b.MI_Id && a.OTQPTYP_ActiveFlg == true && b.HRMP_ActiveFlg == true && a.MI_Id == b.MI_Id && a.HRMP_Id == b.HRMP_Id && a.HRMP_Id == data.HRMP_Id

                                      select a).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region View Master Question Document
        public OnlineTestCandidateDTO ViewMasterQuesDoc(OnlineTestCandidateDTO data)
        {
            try
            {
                //data.getviedocarray = _VMSContext.LP_Master_OE_Questions_FilesDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

       

        #region Deactivate Document
        public OnlineTestCandidateDTO DeactivateActivateDocument(OnlineTestCandidateDTO data)
        {
            try
            {
                //    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                //    var checkresult = _VMSContext.LP_Master_OE_Questions_FilesDMO.Where(a => a.LPMOEQF_Id == data.LPMOEQF_Id).ToList();

                //    if (checkresult.Count > 0)
                //    {
                //        var result = _VMSContext.LP_Master_OE_Questions_FilesDMO.Single(a => a.LPMOEQF_Id == data.LPMOEQF_Id);

                //        if (result.LPMOEQF_ActiveFlag == true)
                //        {
                //            result.LPMOEQF_ActiveFlag = false;
                //        }
                //        else
                //        {
                //            result.LPMOEQF_ActiveFlag = true;
                //        }
                //        result.UpdatedDate = indiantime0;
                //        _VMSContext.Update(result);

                //        var s = _VMSContext.SaveChanges();

                //        if (s > 0)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }
                //    }

                //    data.getviedocarray = _VMSContext.LP_Master_OE_Questions_FilesDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region View Option
        public OnlineTestCandidateDTO ViewMasterQuesOptions(OnlineTestCandidateDTO data)
        {
            try
            {
                data.getViewSavedOptions = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region Deactivate Activate
        public OnlineTestCandidateDTO DeactivateActivateQuesOption(OnlineTestCandidateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(a => a.OTMOEQOA_Id == data.OTMOEQOA_Id).ToList();

                if (checkresult.Count > 0)
                {
                    var result = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Single(a => a.OTMOEQOA_Id == data.OTMOEQOA_Id);

                    if (result.OTMOEQOA_ActiveFlg == true)
                    {
                        result.OTMOEQOA_ActiveFlg = false;
                    }
                    else
                    {
                        result.OTMOEQOA_ActiveFlg = true;
                    }
                    result.OTMOEQOA_UpdatedDate = indiantime0;
                    result.OTMOEQOA_UpdatedBy = data.Userid;
                    _VMSContext.Update(result);

                    var s = _VMSContext.SaveChanges();

                    if (s > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                data.getViewSavedOptions = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion 
        
        #region Deactivate Activate Questions
        public OnlineTestCandidateDTO DeactivateActivateQues(OnlineTestCandidateDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _VMSContext.OT_Master_OE_QuestionsDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToList();

                if (checkresult.Count > 0)
                {
                    var result = _VMSContext.OT_Master_OE_QuestionsDMO.Single(a => a.OTMOEQ_Id == data.OTMOEQ_Id);

                    if (result.OTMOEQ_ActiveFlg == true)
                    {
                        result.OTMOEQ_ActiveFlg = false;
                    }
                    else
                    {
                        result.OTMOEQ_ActiveFlg = true;
                    }
                    result.OTMOEQ_UpdatedDate = indiantime0;
                    result.OTMOEQ_UpdatedBy = data.Userid;
                    _VMSContext.Update(result);

                    var s = _VMSContext.SaveChanges();

                    if (s > 0)
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
        #endregion


        #region ADD MORE OPTION
        public OnlineTestCandidateDTO AddMoreOptions(OnlineTestCandidateDTO data)
        {
            TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
            try
            {
                if (data.OTMOEQ_Id > 0)
                {
                    if (data.tempoptionsDTO.Length > 0)
                    {
                        var cnt = 0;
                        foreach (var item in data.tempoptionsDTO)
                        {
                            if (item.OTMOEQOA_AnswerFlag==true)
                            {
                                cnt += 1;
                            }
                        }


                        foreach (var item1 in data.tempoptionsDTO)
                        {
                            if (item1.OTMOEQOA_Option != null && item1.OTMOEQOA_Option != "")
                            {
                                OT_Master_OE_QNS_OptionsDMO obj1 = new OT_Master_OE_QNS_OptionsDMO();
                                obj1.OTMOEQ_Id = data.OTMOEQ_Id;
                                obj1.OTMOEQOA_Option = item1.OTMOEQOA_Option;
                                obj1.OTMOEQOA_OptionCode = item1.OTMOEQOA_OptionCode;
                                obj1.OTMOEQOA_AnswerFlag = item1.OTMOEQOA_AnswerFlag;
                                obj1.OTMOEQOA_ActiveFlg = true;
                                obj1.OTMOEQOA_CreatedDate = indiantime0;
                                obj1.OTMOEQOA_UpdatedDate = indiantime0;
                                obj1.OTMOEQOA_UpdatedBy = data.Userid;
                                obj1.OTMOEQOA_CreatedBy = data.Userid;
                                _VMSContext.Add(obj1);
                            }
                        }

                        if (cnt>0)
                        {
                            var checkresultoptions = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToList();

                            foreach (var item2 in checkresultoptions)
                            {
                                item2.OTMOEQOA_AnswerFlag = false;
                                item2.OTMOEQOA_UpdatedDate = indiantime0;
                                item2.OTMOEQOA_UpdatedBy = data.Userid;
                                _VMSContext.Update(item2);
                            }

                        }


                        int cnx = _VMSContext.SaveChanges();
                        if (cnx>0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                        
                    }


                    data.getViewSavedOptions = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(a => a.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }
        #endregion


        //MASTER QUESTION
        #endregion

    }
}
