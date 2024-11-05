using DataAccessMsSqlServerProvider.FeedBack;
using DomainModel.Model.FeedBack;
using DomainModel.Model.NAAC.FeedBack;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.FeedBack.Services
{
    public class FeedbackTypeQuestionMappingImpl : Interface.FeedbackTypeQuestionMappingInterface
    {
        public FeedBackContext _context;

        public FeedbackTypeQuestionMappingImpl(FeedBackContext context)
        {
            _context = context;
        }
        public FeedbackTypeQuestionMappingDTO getdetails(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                data.feedbacktype = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.getdetails = (from a in _context.Feedback_Type_QuestionsDMO
                                   from b in _context.FeedBackMasterTypeDMO
                                   from c in _context.Feedback_Master_QuestionsDMO
                                   where (a.FMTY_Id == b.FMTY_Id && c.FMQE_Id == a.FMQE_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                   && b.FMTY_ActiveFlag == true && c.FMQE_ActiveFlag == true)
                                   select new FeedbackTypeQuestionMappingDTO
                                   {
                                       FMTY_Id = a.FMTY_Id,
                                       FMQE_Id = a.FMQE_Id,
                                       FMQE_FeedbackQuestions = c.FMQE_FeedbackQuestions,
                                       FMTY_FeedbackTypeName = b.FMTY_FeedbackTypeName,
                                       FMTQ_TQOrder = a.FMTQ_TQOrder,
                                       FMTQ_Id = a.FMTQ_Id,
                                       FMTQ_ActiveFlag = a.FMTQ_ActiveFlag,
                                       FMTY_QuestionwiseOptionFlg = b.FMTY_QuestionwiseOptionFlg
                                   }).Distinct().OrderBy(a => a.FMTQ_TQOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO onchnagetype(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                var checkquestionwiseflag = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id).ToList();

                var getmappedids = (from a in _context.FeedBackMasterTypeDMO
                                    from b in _context.Feedback_Master_QuestionsDMO
                                    from c in _context.Feedback_Type_QuestionsDMO
                                    where (a.FMTY_Id == c.FMTY_Id && b.FMQE_Id == c.FMQE_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                    && c.FMTY_Id == data.FMTY_Id && b.FMQE_ActiveFlag == true)
                                    select new FeedbackTypeQuestionMappingDTO
                                    {
                                        FMQE_Id = c.FMQE_Id

                                    }).Distinct().ToList();

                for (int k = 0; k < getmappedids.Count(); k++)
                {
                    ids.Add(getmappedids[k].FMQE_Id);
                }

                data.feedbackquestions = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id
                 && a.FMQE_ActiveFlag == true && !ids.Contains(a.FMQE_Id)).Distinct().OrderBy(a => a.FMQE_FQOrder).ToArray();

                var checkinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (checkinstitute.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    var checkstudentschool = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    //   var checkalumischool =_context.Feedback_Alumni_TransactionDMO

                    var checkstudentostaffschool = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffschool = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentschool.Count == 0 && checkstudentostaffschool.Count == 0 && checkstaffschool.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                else
                {
                    var checkstudentclg = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkaluminclg = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstudenttostaffclg = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffclg = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentclg.Count == 0 && checkaluminclg.Count == 0 && checkstudenttostaffclg.Count == 0 && checkstaffclg.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // When Feedback Type Not A Question Wise Options
        public FeedbackTypeQuestionMappingDTO savedata(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                if (data.FeedbackTypeQuestionMappingTempDTO.Count() > 0)
                {
                    int rowcount = _context.Feedback_Type_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                    for (int k = 0; k < data.FeedbackTypeQuestionMappingTempDTO.Count(); k++)
                    {
                        rowcount = rowcount + 1;

                        Feedback_Type_QuestionsDMO dmo = new Feedback_Type_QuestionsDMO();

                        dmo.MI_Id = data.MI_Id;
                        dmo.FMTY_Id = data.FMTY_Id;
                        dmo.FMQE_Id = data.FeedbackTypeQuestionMappingTempDTO[k].FMQE_Id;
                        dmo.FMTQ_TQOrder = rowcount;
                        dmo.FMTQ_ActiveFlag = true;
                        dmo.FMTQ_CreatedBy = data.userid;
                        dmo.FMTQ_UpdatedBy = data.userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;
                        _context.Add(dmo);
                    }
                    int i = _context.SaveChanges();
                    if (i > 0)
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
                data.returnval = false;
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO activedeactive(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                var checkinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (checkinstitute.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    var checkstudentschool = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    //   var checkalumischool =_context.Feedback_Alumni_TransactionDMO

                    var checkstudentostaffschool = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffschool = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentschool.Count == 0 && checkstudentostaffschool.Count == 0 && checkstaffschool.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                else
                {
                    var checkstudentclg = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkaluminclg = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstudenttostaffclg = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffclg = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentclg.Count == 0 && checkaluminclg.Count == 0 && checkstudenttostaffclg.Count == 0 && checkstaffclg.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }


                if (data.FMTQ_Id > 0 && data.mappeddetailscount == "continue")
                {
                    var result = _context.Feedback_Type_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTQ_Id == data.FMTQ_Id);
                    if (result.FMTQ_ActiveFlag == false)
                    {
                        result.FMTQ_ActiveFlag = true;
                    }
                    else
                    {
                        result.FMTQ_ActiveFlag = false;
                    }
                    result.FMTQ_UpdatedBy = data.userid;
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int i = _context.SaveChanges();
                    if (i > 0)
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
                data.returnval = false;
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO getorder(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                if (data.FeedbackTypeQuestionMappingTemporderDTO.Count() > 0)
                {
                    int rowcount = 0;
                    for (int k = 0; k < data.FeedbackTypeQuestionMappingTemporderDTO.Count(); k++)
                    {
                        rowcount = rowcount + 1;
                        var result = _context.Feedback_Type_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTQ_Id == data.FeedbackTypeQuestionMappingTemporderDTO[k].FMTQ_Id);
                        result.FMTQ_TQOrder = rowcount;
                        result.UpdatedDate = DateTime.Now;
                        result.FMTQ_UpdatedBy = data.userid;
                        _context.Update(result);
                    }

                    int i = _context.SaveChanges();
                    if (i > 0)
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

        // When Feedback Type Is A Question Wise Options
        public FeedbackTypeQuestionMappingDTO onchangequestion(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                List<long> optionsid = new List<long>();

                var getoptions = (from a in _context.Feedback_Type_QuestionsDMO
                                  from b in _context.Feedback_Master_QuestionsDMO
                                  from c in _context.FeedBackMasterTypeDMO
                                  from d in _context.Feedback_Type_Questions_OptionsDMO
                                  from e in _context.Feedback_Master_OptionsDMO
                                  where (a.FMQE_Id == b.FMQE_Id && a.FMTY_Id == c.FMTY_Id && a.FMTQ_Id == d.FMTQ_Id && d.FMOP_Id == e.FMOP_Id
                                  && a.FMQE_Id == data.FMQE_Id && a.FMTY_Id == data.FMTY_Id && a.MI_Id == data.MI_Id)
                                  select d.FMOP_Id).Distinct().ToArray();


                foreach (var c in getoptions)
                {
                    optionsid.Add(c);
                }

                data.getoptions = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && !optionsid.Contains(a.FMOP_Id)
                && a.FMOP_ActiveFlag == true).OrderBy(a => a.FMOP_FOOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO getquestionwiseoption(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                data.getquestionsoptions = (from a in _context.Feedback_Type_QuestionsDMO
                                            from b in _context.Feedback_Master_QuestionsDMO
                                            from c in _context.FeedBackMasterTypeDMO
                                            from d in _context.Feedback_Type_Questions_OptionsDMO
                                            from e in _context.Feedback_Master_OptionsDMO
                                            where (a.FMQE_Id == b.FMQE_Id && a.FMTY_Id == c.FMTY_Id && a.FMTQ_Id == d.FMTQ_Id && d.FMOP_Id == e.FMOP_Id
                                             && a.FMQE_Id == data.FMQE_Id && a.FMTY_Id == data.FMTY_Id && a.MI_Id == data.MI_Id)
                                            select new FeedbackTypeQuestionMappingDTO
                                            {
                                                FMOP_Id = d.FMOP_Id,
                                                FMOP_OptionName = e.FMOP_FeedbackOptions,
                                                FMTQO_Id = d.FMTQO_Id,
                                                FMTQO_TQOOrder = d.FMTQO_TQOOrder,
                                                FMOP_FeedbackORemarks = e.FMOP_FeedbackORemarks,
                                                FMTQO_ActiveFlag = d.FMTQO_ActiveFlag
                                            }).Distinct().OrderBy(a => a.FMTQO_TQOOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO savedatanew(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                if (data.temp_question.Length > 0)
                {
                    int rowcount = _context.Feedback_Type_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                    int rowcountnew = (from b in _context.Feedback_Type_Questions_OptionsDMO
                                       from cd in _context.Feedback_Type_QuestionsDMO
                                       where (b.FMTQ_Id == cd.FMTQ_Id && cd.FMTY_Id == data.FMTY_Id && cd.MI_Id == data.MI_Id)
                                       select b).Count();

                    foreach (var c in data.temp_question)
                    {
                        rowcount = rowcount + 1;

                        Feedback_Type_QuestionsDMO dmo = new Feedback_Type_QuestionsDMO();

                        dmo.MI_Id = data.MI_Id;
                        dmo.FMTY_Id = c.FMTY_Id;
                        dmo.FMQE_Id = c.FMQE_Id;
                        dmo.FMTQ_TQOrder = rowcount;
                        dmo.FMTQ_ActiveFlag = true;
                        dmo.FMTQ_CreatedBy = data.userid;
                        dmo.FMTQ_UpdatedBy = data.userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;
                        _context.Add(dmo);

                        if (!c.FMQE_ManualEntryFlg)
                        {
                            foreach (var d in c.optiondetails)
                            {
                                rowcountnew += 1;

                                Feedback_Type_Questions_OptionsDMO dmooptions = new Feedback_Type_Questions_OptionsDMO();

                                dmooptions.FMTQ_Id = dmo.FMTQ_Id;
                                dmooptions.FMOP_Id = d.FMOP_Id;
                                dmooptions.FMTQO_TQOOrder = rowcountnew;
                                dmooptions.FMTQO_ActiveFlag = true;
                                dmooptions.FMTQO_CreatedBy = data.userid;
                                dmooptions.FMTQO_UpdatedBy = data.userid;
                                dmooptions.CreatedDate = DateTime.UtcNow;
                                dmooptions.UpdatedDate = DateTime.UtcNow;
                                _context.Add(dmooptions);
                            }
                        }
                    }

                    var k = _context.SaveChanges();
                    if (k > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeQuestionMappingDTO deactiveoption(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                var checkinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (checkinstitute.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    var checkstudentschool = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    //   var checkalumischool =_context.Feedback_Alumni_TransactionDMO

                    var checkstudentostaffschool = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffschool = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentschool.Count == 0 && checkstudentostaffschool.Count == 0 && checkstaffschool.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                else
                {
                    var checkstudentclg = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkaluminclg = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstudenttostaffclg = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffclg = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentclg.Count == 0 && checkaluminclg.Count == 0 && checkstudenttostaffclg.Count == 0 && checkstaffclg.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                if (data.FMTQO_Id > 0 && data.mappeddetailscount == "continue")
                {
                    var result = _context.Feedback_Type_Questions_OptionsDMO.Single(a => a.FMTQO_Id == data.FMTQO_Id);
                    if (result.FMTQO_ActiveFlag == true)
                    {
                        result.FMTQO_ActiveFlag = false;
                    }
                    else
                    {
                        result.FMTQO_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.UtcNow;
                    result.FMTQO_UpdatedBy = data.userid;
                    _context.Update(result);

                    var k = _context.SaveChanges();
                    if (k > 0)
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
        public FeedbackTypeQuestionMappingDTO getordernew(FeedbackTypeQuestionMappingDTO data)
        {
            try
            {
                if (data.temp_question_order_TemporderDTO.Count() > 0)
                {
                    int rowcount = 0;

                    foreach (var c in data.temp_question_order_TemporderDTO)
                    {
                        rowcount = rowcount + 1;
                        var result = _context.Feedback_Type_Questions_OptionsDMO.Single(a => a.FMTQO_Id == c.FMTQO_Id);
                        result.FMTQO_TQOOrder = rowcount;
                        result.UpdatedDate = DateTime.Now;
                        result.FMTQO_UpdatedBy = data.userid;
                        _context.Update(result);
                    }

                    int i = _context.SaveChanges();
                    if (i > 0)
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

        // Type Option Mapping 
        public FeedbackTypeOptionMappingDTO optiongetdetails(FeedbackTypeOptionMappingDTO data)
        {
            try
            {
                data.feedbacktype = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_ActiveFlag == true
                && a.FMTY_QuestionwiseOptionFlg == false).Distinct().OrderBy(a => a.FMTY_FTOrder).ToArray();

                data.getdetails = (from a in _context.Feedback_Type_OptionsDMO
                                   from b in _context.FeedBackMasterTypeDMO
                                   from c in _context.Feedback_Master_OptionsDMO
                                   where (a.FMTY_Id == b.FMTY_Id && c.FMOP_Id == a.FMOP_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                   && b.FMTY_ActiveFlag == true && c.FMOP_ActiveFlag == true && b.FMTY_QuestionwiseOptionFlg == false)
                                   select new FeedbackTypeOptionMappingDTO
                                   {
                                       FMTY_Id = a.FMTY_Id,
                                       FMOP_Id = a.FMOP_Id,
                                       FMOP_FeedbackOptions = c.FMOP_FeedbackOptions,
                                       FMTY_FeedbackTypeName = b.FMTY_FeedbackTypeName,
                                       FMTO_TQOrder = a.FMTO_TQOrder,
                                       FMTO_Id = a.FMTO_Id,
                                       FMTO_ActiveFlag = a.FMTO_ActiveFlag
                                   }).Distinct().OrderBy(a => a.FMTO_TQOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeOptionMappingDTO optiononchnagetype(FeedbackTypeOptionMappingDTO data)
        {
            try
            {
                List<long> ids = new List<long>();

                var getmappedids = (from a in _context.FeedBackMasterTypeDMO
                                    from b in _context.Feedback_Master_OptionsDMO
                                    from c in _context.Feedback_Type_OptionsDMO
                                    where (a.FMTY_Id == c.FMTY_Id && b.FMOP_Id == c.FMOP_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                    && c.FMTY_Id == data.FMTY_Id && b.FMOP_ActiveFlag == true)
                                    select new FeedbackTypeOptionMappingDTO
                                    {
                                        FMOP_Id = c.FMOP_Id

                                    }).Distinct().ToList();

                for (int k = 0; k < getmappedids.Count(); k++)
                {
                    ids.Add(getmappedids[k].FMOP_Id);
                }

                data.feedbackoptions = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id
                 && a.FMOP_ActiveFlag == true && !ids.Contains(a.FMOP_Id)).Distinct().OrderBy(a => a.FMOP_FOOrder).ToArray();

                var checkinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (checkinstitute.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    var checkstudentschool = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    //   var checkalumischool =_context.Feedback_Alumni_TransactionDMO

                    var checkstudentostaffschool = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffschool = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentschool.Count == 0 && checkstudentostaffschool.Count == 0 && checkstaffschool.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                else
                {
                    var checkstudentclg = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkaluminclg = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstudenttostaffclg = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffclg = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentclg.Count == 0 && checkaluminclg.Count == 0 && checkstudenttostaffclg.Count == 0 && checkstaffclg.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedbackTypeOptionMappingDTO optionsavedata(FeedbackTypeOptionMappingDTO data)
        {
            try
            {
                if (data.FeedbackTypeOptionMappingTempDTO.Count() > 0)
                {
                    int rowcount = _context.Feedback_Type_OptionsDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                    for (int k = 0; k < data.FeedbackTypeOptionMappingTempDTO.Count(); k++)
                    {
                        rowcount = rowcount + 1;

                        Feedback_Type_OptionsDMO dmo = new Feedback_Type_OptionsDMO();

                        dmo.MI_Id = data.MI_Id;
                        dmo.FMTY_Id = data.FMTY_Id;
                        dmo.FMOP_Id = data.FeedbackTypeOptionMappingTempDTO[k].FMOP_Id;
                        dmo.FMTO_TQOrder = rowcount;
                        dmo.FMTO_ActiveFlag = true;
                        dmo.FMTO_CreatedBy = data.userid;
                        dmo.FMTO_UpdatedBy = data.userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.UpdatedDate = DateTime.Now;
                        _context.Add(dmo);
                    }
                    int i = _context.SaveChanges();
                    if (i > 0)
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
                data.returnval = false;
            }
            return data;
        }
        public FeedbackTypeOptionMappingDTO optionactivedeactive(FeedbackTypeOptionMappingDTO data)
        {
            try
            {
                var checkinstitute = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (checkinstitute.FirstOrDefault().MI_SchoolCollegeFlag == "S")
                {
                    var checkstudentschool = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    //   var checkalumischool =_context.Feedback_Alumni_TransactionDMO

                    var checkstudentostaffschool = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffschool = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentschool.Count == 0 && checkstudentostaffschool.Count == 0 && checkstaffschool.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                else
                {
                    var checkstudentclg = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkaluminclg = _context.Feedback_College_Alumni_Transaction.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstudenttostaffclg = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    var checkstaffclg = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();

                    if (checkstudentclg.Count == 0 && checkaluminclg.Count == 0 && checkstudenttostaffclg.Count == 0 && checkstaffclg.Count == 0)
                    {
                        data.mappeddetailscount = "continue";
                    }
                    else
                    {
                        data.mappeddetailscount = "mapped";
                    }
                }
                if (data.FMTO_Id > 0 && data.mappeddetailscount == "continue")
                {
                    var result = _context.Feedback_Type_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTO_Id == data.FMTO_Id);
                    if (result.FMTO_ActiveFlag == false)
                    {
                        result.FMTO_ActiveFlag = true;
                    }
                    else
                    {
                        result.FMTO_ActiveFlag = false;
                    }
                    result.FMTO_UpdatedBy = data.userid;
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int i = _context.SaveChanges();
                    if (i > 0)
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
                data.returnval = false;
            }
            return data;
        }
        public FeedbackTypeOptionMappingDTO optiongetorder(FeedbackTypeOptionMappingDTO data)
        {
            try
            {
                if (data.FeedbackTypeOptionMappingTemporderDTO.Count() > 0)
                {
                    int rowcount = 0;
                    for (int k = 0; k < data.FeedbackTypeOptionMappingTemporderDTO.Count(); k++)
                    {
                        rowcount = rowcount + 1;
                        var result = _context.Feedback_Type_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTO_Id == data.FeedbackTypeOptionMappingTemporderDTO[k].FMTO_Id);
                        result.FMTO_TQOrder = rowcount;
                        result.UpdatedDate = DateTime.Now;
                        result.FMTO_UpdatedBy = data.userid;
                        _context.Update(result);
                    }

                    int i = _context.SaveChanges();
                    if (i > 0)
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
    }
}
