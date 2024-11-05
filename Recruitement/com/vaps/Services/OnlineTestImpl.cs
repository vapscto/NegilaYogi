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
    public class OnlineTestImpl : Interfaces.OnlineTestInterface
    {
        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _Context;
        public OnlineTestImpl(VMSContext VMSContext, DomainModelMsSqlServerContext OrganisationContext)
        {
            _VMSContext = VMSContext;
            _Context = OrganisationContext;
        }

        #region  MASTER QUESTION
        //MASTER QUESTION
        #region Load Master Question
        public OnlineTestDTO getmasterquestionloaddata(OnlineTestDTO data)
        {
            try
            {
                data.getMasterQuestiondetails = (from a in _VMSContext.OT_Master_OE_QuestionsDMO
                                                 from b in _VMSContext.HR_Master_PositionDMO
                                                 from c in _VMSContext.OT_QuestionPaperTypeDMO
                                                 where (a.HRMP_Id == b.HRMP_Id && a.OTQPTYP_Id == c.OTQPTYP_Id &&
                                                  a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id)
                                                 select new OnlineTestDTO
                                                 {
                                                     OTMOEQ_Id = a.OTMOEQ_Id,
                                                     OTMOEQ_Question = a.OTMOEQ_Question,
                                                     OTMOEQ_QuestionDesc = a.OTMOEQ_QuestionDesc,
                                                     OTMOEQ_ActiveFlg = a.OTMOEQ_ActiveFlg,
                                                     HRMP_Id = a.HRMP_Id,
                                                     OTQPTYP_Id = a.OTQPTYP_Id,
                                                     OTQPTYP_QuestionPaperName = c.OTQPTYP_QuestionPaperName,
                                                     HRMP_Position = b.HRMP_Position,
                                                     countoption = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(b => b.OTMOEQ_Id == a.OTMOEQ_Id).Count(),
                                                     OTMOEQ_CreatedDate = a.OTMOEQ_CreatedDate,
                                                     OTMOEQ_SubjectiveFlg = a.OTMOEQ_SubjectiveFlg,
                                                     OTMOEQ_Answer = a.OTMOEQ_Answer
                                                 }).Distinct().OrderByDescending(a => a.OTMOEQ_CreatedDate).ToArray();


                data.positionlist = _VMSContext.HR_Master_PositionDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMP_ActiveFlg == true).Distinct().OrderBy(e => e.HRMP_Position).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        #endregion

        #region Save Or Update Question
        public OnlineTestDTO SaveMasterQuestionDetails(OnlineTestDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.OTMOEQ_Id > 0)
                {
                    var resultduplicate = _VMSContext.OT_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMP_Id == data.HRMP_Id
                    && a.OTQPTYP_Id == data.OTQPTYP_Id && a.OTMOEQ_Id != data.OTMOEQ_Id && a.OTMOEQ_Question.ToLower().Trim().Equals(data.OTMOEQ_Question.ToLower().Trim())).ToList();

                    if (resultduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        //COMMENTED FOR FILES
                        //List<long> filesid = new List<long>();

                        //foreach (var c in data.tempfilesDTO)
                        //{
                        //    filesid.Add(c.LPMOEQF_Id);
                        //}

                        //Array previous_Noresultremove = _VMSContext.LP_Master_OE_Questions_FilesDMO.Where(t => !filesid.Contains(t.LPMOEQF_Id)
                        //&& t.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();

                        //foreach (LP_Master_OE_Questions_FilesDMO ph1 in previous_Noresultremove)
                        //{
                        //    _VMSContext.Remove(ph1);
                        //}
                        //COMMENTED FOR FILES

                        var result = _VMSContext.OT_Master_OE_QuestionsDMO.Single(t => t.OTMOEQ_Id == data.OTMOEQ_Id && t.MI_Id == data.MI_Id);
                        result.OTMOEQ_Question = data.OTMOEQ_Question;
                        result.OTMOEQ_QuestionDesc = data.OTMOEQ_QuestionDesc;
                        result.OTMOEQ_SubjectiveFlg = data.OTMOEQ_SubjectiveFlg;
                        result.OTMOEQ_Answer = data.OTMOEQ_Answer;
                        result.OTMOEQ_Marks = data.OTMOEQ_Marks;
                        result.OTMOEQ_UpdatedBy = data.Userid;
                        result.OTMOEQ_UpdatedDate = indiantime0;

                        _VMSContext.Update(result);
                        //COMMENTED FOR FILES
                        //if (data.tempfilesDTO.Length > 0)
                        //{
                        //    foreach (var c in data.tempfilesDTO)
                        //    {
                        //        if (c.LPMOEQF_Id > 0)
                        //        {
                        //            if (c.LPMOEQF_FilePath != null && c.LPMOEQF_FilePath != "")
                        //            {
                        //                var resultfiles = _VMSContext.LP_Master_OE_Questions_FilesDMO.Single(a => a.LPMOEQF_Id == c.LPMOEQF_Id
                        //                && a.OTMOEQ_Id == data.OTMOEQ_Id);
                        //                resultfiles.LPMOEQF_FilePath = c.LPMOEQF_FilePath;
                        //                resultfiles.LPMOEQF_FileName = c.LPMOEQF_FileName;
                        //                resultfiles.UpdatedDate = indiantime0;
                        //                _VMSContext.Update(resultfiles);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            if (c.LPMOEQF_FilePath != null && c.LPMOEQF_FilePath != "")
                        //            {
                        //                LP_Master_OE_Questions_FilesDMO lP_Master_OE_Questions_FilesDMO = new LP_Master_OE_Questions_FilesDMO();
                        //                lP_Master_OE_Questions_FilesDMO.OTMOEQ_Id = data.OTMOEQ_Id;
                        //                lP_Master_OE_Questions_FilesDMO.LPMOEQF_FileName = c.LPMOEQF_FileName;
                        //                lP_Master_OE_Questions_FilesDMO.LPMOEQF_FilePath = c.LPMOEQF_FilePath;
                        //                lP_Master_OE_Questions_FilesDMO.LPMOEQF_ActiveFlag = true;
                        //                lP_Master_OE_Questions_FilesDMO.CreatedDate = indiantime0;
                        //                lP_Master_OE_Questions_FilesDMO.UpdatedDate = indiantime0;

                        //                _VMSContext.Add(lP_Master_OE_Questions_FilesDMO);
                        //            }
                        //        }
                        //    }
                        //}
                        //COMMENTED FOR FILES

                        if (data.tempoptionsDTO.Length > 0 && data.OTMOEQ_SubjectiveFlg == false)
                        {
                            foreach (var c in data.tempoptionsDTO)
                            {
                                var checkresultoptions = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(a => a.OTMOEQOA_Id == c.OTMOEQOA_Id
                                && a.OTMOEQ_Id == data.OTMOEQ_Id).ToList();

                                if (checkresultoptions.Count > 0)
                                {
                                    var resultoptions = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Single(a => a.OTMOEQOA_Id == c.OTMOEQOA_Id
                                    && a.OTMOEQ_Id == data.OTMOEQ_Id);
                                    resultoptions.OTMOEQOA_Option = c.OTMOEQOA_Option;
                                    resultoptions.OTMOEQOA_OptionCode = c.OTMOEQOA_OptionCode;
                                    resultoptions.OTMOEQOA_AnswerFlag = c.OTMOEQOA_AnswerFlag;
                                    resultoptions.OTMOEQOA_AnswerDesc = c.OTMOEQOA_AnswerDesc;
                                    resultoptions.OTMOEQOA_UpdatedDate = indiantime0;
                                    resultoptions.OTMOEQOA_UpdatedBy = data.Userid;
                                    _VMSContext.Update(resultoptions);
                                }
                                else
                                {
                                    if (c.OTMOEQOA_Option != null && c.OTMOEQOA_Option != "")
                                    {
                                        OT_Master_OE_QNS_OptionsDMO obj1 = new OT_Master_OE_QNS_OptionsDMO();
                                        obj1.OTMOEQ_Id = data.OTMOEQ_Id;
                                        obj1.OTMOEQOA_Option = c.OTMOEQOA_Option;
                                        obj1.OTMOEQOA_OptionCode = c.OTMOEQOA_OptionCode;
                                        obj1.OTMOEQOA_AnswerFlag = c.OTMOEQOA_AnswerFlag;
                                        obj1.OTMOEQOA_ActiveFlg = true;
                                        obj1.OTMOEQOA_CreatedDate = indiantime0;
                                        obj1.OTMOEQOA_UpdatedDate = indiantime0;
                                        obj1.OTMOEQOA_UpdatedBy = data.Userid;
                                        obj1.OTMOEQOA_CreatedBy = data.Userid;
                                        _VMSContext.Add(obj1);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Array previous_options = _VMSContext.OT_Master_OE_QNS_OptionsDMO.Where(t => t.OTMOEQ_Id == data.OTMOEQ_Id).ToArray();

                            foreach (OT_Master_OE_QNS_OptionsDMO ph2 in previous_options)
                            {
                                _VMSContext.Remove(ph2);
                            }
                        }

                        var contactExists = _VMSContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = true;
                            data.message = "Update";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Update";
                        }
                    }
                }
                else
                {
                    OT_Master_OE_QuestionsDMO obj = new OT_Master_OE_QuestionsDMO();

                    var resultduplicate = _VMSContext.OT_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.HRMP_Id == data.HRMP_Id
                    && a.OTQPTYP_Id == data.OTQPTYP_Id && a.OTMOEQ_Question.ToLower().Trim().Equals(data.OTMOEQ_Question.ToLower().Trim())).ToList();

                    if (resultduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        obj.MI_Id = data.MI_Id;
                        obj.HRMP_Id = data.HRMP_Id;
                        obj.OTQPTYP_Id = data.OTQPTYP_Id;
                        obj.OTMOEQ_Question = data.OTMOEQ_Question;
                        obj.OTMOEQ_QuestionDesc = data.OTMOEQ_QuestionDesc;
                        obj.OTMOEQ_SubjectiveFlg = data.OTMOEQ_SubjectiveFlg;
                        obj.OTMOEQ_Answer = data.OTMOEQ_Answer;
                        obj.OTMOEQ_Marks = data.OTMOEQ_Marks;
                        obj.OTMOEQ_ActiveFlg = true;
                        obj.OTMOEQ_CreatedBy = data.Userid;
                        obj.OTMOEQ_UpdatedBy = data.Userid;
                        obj.OTMOEQ_CreatedDate = indiantime0;
                        obj.OTMOEQ_UpdatedDate = indiantime0;

                        _VMSContext.Add(obj);
                        //COMMENTED FOR FILES
                        //if (data.tempfilesDTO.Length > 0)
                        //{
                        //    foreach (var c in data.tempfilesDTO)
                        //    {
                        //        if (c.LPMOEQF_FilePath != null && c.LPMOEQF_FilePath != "")
                        //        {
                        //            LP_Master_OE_Questions_FilesDMO lP_Master_OE_Questions_FilesDMO = new LP_Master_OE_Questions_FilesDMO();
                        //            lP_Master_OE_Questions_FilesDMO.OTMOEQ_Id = OT_Master_OE_QuestionsDMO.OTMOEQ_Id;
                        //            lP_Master_OE_Questions_FilesDMO.LPMOEQF_FileName = c.LPMOEQF_FileName;
                        //            lP_Master_OE_Questions_FilesDMO.LPMOEQF_FilePath = c.LPMOEQF_FilePath;
                        //            lP_Master_OE_Questions_FilesDMO.LPMOEQF_ActiveFlag = true;
                        //            lP_Master_OE_Questions_FilesDMO.CreatedDate = indiantime0;
                        //            lP_Master_OE_Questions_FilesDMO.UpdatedDate = indiantime0;

                        //            _VMSContext.Add(lP_Master_OE_Questions_FilesDMO);
                        //        }
                        //    }
                        //}
                        //COMMENTED FOR FILES


                        if (data.tempoptionsDTO.Length > 0 && data.OTMOEQ_SubjectiveFlg == false)
                        {
                            foreach (var c in data.tempoptionsDTO)
                            {
                                if (c.OTMOEQOA_Option != null && c.OTMOEQOA_Option != "")
                                {
                                    OT_Master_OE_QNS_OptionsDMO robj = new OT_Master_OE_QNS_OptionsDMO();
                                    robj.OTMOEQ_Id = obj.OTMOEQ_Id;
                                    robj.OTMOEQOA_Option = c.OTMOEQOA_Option;
                                    robj.OTMOEQOA_OptionCode = c.OTMOEQOA_OptionCode;
                                    robj.OTMOEQOA_AnswerDesc = c.OTMOEQOA_AnswerDesc;
                                    robj.OTMOEQOA_AnswerFlag = c.OTMOEQOA_AnswerFlag;
                                    robj.OTMOEQOA_ActiveFlg = true;
                                    robj.OTMOEQOA_CreatedDate = indiantime0;
                                    robj.OTMOEQOA_UpdatedDate = indiantime0;

                                    _VMSContext.Add(robj);
                                }
                            }
                        }

                        var i = _VMSContext.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                            data.message = "Add";
                        }
                        else
                        {
                            data.returnval = false;
                            data.message = "Add";
                        }
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
        public OnlineTestDTO EditMasterQuestion(OnlineTestDTO data)
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
        public OnlineTestDTO ViewMasterQuesDoc(OnlineTestDTO data)
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

        #region Get the Question Paper type
        public OnlineTestDTO getqnspapertype(OnlineTestDTO data)
        {
            try
            {
                data.papertypelist = (from a in _VMSContext.OT_QuestionPaperTypeDMO
                                      from b in _VMSContext.HR_Master_PositionDMO
                                      where a.MI_Id == b.MI_Id && a.OTQPTYP_ActiveFlg == true && b.HRMP_ActiveFlg == true && a.MI_Id == b.MI_Id && a.HRMP_Id == b.HRMP_Id && a.HRMP_Id == data.HRMP_Id

                                      select a).Distinct().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        #endregion

        #region Deactivate Document
        public OnlineTestDTO DeactivateActivateDocument(OnlineTestDTO data)
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
        public OnlineTestDTO ViewMasterQuesOptions(OnlineTestDTO data)
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
        public OnlineTestDTO DeactivateActivateQuesOption(OnlineTestDTO data)
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
        public OnlineTestDTO DeactivateActivateQues(OnlineTestDTO data)
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
        public OnlineTestDTO AddMoreOptions(OnlineTestDTO data)
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
