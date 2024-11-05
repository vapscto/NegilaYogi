using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.OnlineExam;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineExam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApplication1.Services
{
    public class MasterQuestionCollegeImpl : Interfaces.MasterQuestionCollegeInterface
    {
        private static ConcurrentDictionary<string, MasterQuestionDTO> _login =
             new ConcurrentDictionary<string, MasterQuestionDTO>();

        ILogger<MasterQuestionCollegeImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public MasterQuestionCollegeImpl(DomainModelMsSqlServerContext dbcontext, ILogger<MasterQuestionCollegeImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public MasterQuestionDTO getloaddata(MasterQuestionDTO data)
        {
            try
            {
                data.getQuestiondetails = _dbContext.LMS_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
                data.getclass = _dbContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id).ToArray();
                data.getSubjects = _dbContext.MasterSubjectList.Where(a => a.MI_Id == data.MI_Id && a.ISMS_PreadmFlag==1).ToArray();
                //--------------------2nd Tab


             data.courselist = (from a in _dbContext.MasterCourseDMO
                                from b in _dbContext.CLG_Adm_College_AY_CourseDMO
                                where (a.MI_Id.Equals(data.MI_Id) && a.AMCO_ActiveFlag && b.MI_Id.Equals(data.MI_Id) && b.ASMAY_Id.Equals(data.ASMAY_Id) && b.ACAYC_ActiveFlag && b.AMCO_Id.Equals(a.AMCO_Id))
                                                   select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();

                data.getFQOptiondetails = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                           from b in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                                           where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && a.MI_Id == data.MI_Id)
                                           select new MasterQuestionDTO
                                           {
                                               LMSMOEQ_Id = a.LMSMOEQ_Id,
                                               LMSMOEQ_Question = a.LMSMOEQ_Question,
                                               LMSMOEQ_Marks = a.LMSMOEQ_Marks,
                                           }).Distinct().ToArray();

                
                //data.getFQuestiondetails = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                //                            from b in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                //                            where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && a.MI_Id == data.MI_Id)
                //                            select new MasterQuestionDTO
                //                            {
                //                                LMSMOEQ_Id = a.LMSMOEQ_Id,
                //                                LMSMOEQ_Question = a.LMSMOEQ_Question,
                //                                LMSMOEQ_Marks = a.LMSMOEQ_Marks,
                //                                LMSMOEQOA_Option = b.LMSMOEQOA_Option,
                //                                LMSMOEQOA_OptionCode = b.LMSMOEQOA_OptionCode,
                //                                LMSMOEQOA_AnswerFlag = b.LMSMOEQOA_AnswerFlag
                //                            }).Distinct().ToArray();



              //  var questions = _dbContext.LMS_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).ToArray().ToList();



                var questions = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                           from b in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                                           where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && a.MI_Id == data.MI_Id)
                                           select new MasterQuestionDTO
                                           {
                                               LMSMOEQ_Id = a.LMSMOEQ_Id,
                                           }).Distinct().ToArray().ToList();



                List<long> PAMOEQ_Ids = new List<long>();
               
                foreach (var item in questions)
                {
                    PAMOEQ_Ids.Add(item.LMSMOEQ_Id);
                }


                data.getFQuestiondetails = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                          where (a.MI_Id == data.MI_Id && ! PAMOEQ_Ids.Contains(a.LMSMOEQ_Id))
                                            select new MasterQuestionDTO
                                            {
                                                LMSMOEQ_Id = a.LMSMOEQ_Id,
                                                LMSMOEQ_Question = a.LMSMOEQ_Question,
                                                LMSMOEQ_Marks = a.LMSMOEQ_Marks,
                                            }).Distinct().ToArray();

                
                data.result = (from a in _dbContext.ClgMasterBranchDMO
                               from b in _dbContext.MasterCourseDMO
                               from c in _dbContext.CLG_Adm_Master_SemesterDMO
                               from d in _dbContext.LMS_Master_OE_Questions_BranchDMO
                               from e in _dbContext.LMS_Master_OE_QuestionsDMO
                               where (a.AMB_Id==d.AMB_Id && b.AMCO_Id==d.AMCO_Id && c.AMSE_Id==d.AMSE_Id && a.MI_Id == data.MI_Id && e.LMSMOEQ_Id==d.LMSMOEQ_Id)
                               select new MasterQuestionDTO
                               {
                                   LMSMOEQB_Id=d.LMSMOEQB_Id,
                                   LMSMOEQ_Id = e.LMSMOEQ_Id,
                                   LMSMOEQ_Question = e.LMSMOEQ_Question,
                                   AMB_BranchName = a.AMB_BranchName,
                                   AMCO_CourseName=b.AMCO_CourseName,
                                   AMSE_SEMName=c.AMSE_SEMName
                               }).Distinct().ToArray();
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public MasterQuestionDTO savedetails(MasterQuestionDTO data)
        {
            try
            {
                if (data.LMSMOEQ_Id != 0)
                {
                    var res = _dbContext.LMS_Master_OE_QuestionsDMO.Where(t =>  t.LMSMOEQ_Question.Contains(data.LMSMOEQ_Question) && t.LMSMOEQ_Marks == data.LMSMOEQ_Marks && t.LMSMOEQ_Id != data.LMSMOEQ_Id && t.MI_Id==data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _dbContext.LMS_Master_OE_QuestionsDMO.Single(t => t.LMSMOEQ_Id == data.LMSMOEQ_Id && t.MI_Id==data.MI_Id);
                        result.MI_Id = data.MI_Id;
                        result.ISMS_Id = data.ISMS_Id;
                        result.LMSMOEQ_Question = data.LMSMOEQ_Question;
                        result.LMSMOEQ_Marks = data.LMSMOEQ_Marks;
                        result.LMSMOEQ_QuestionDesc = data.LMSMOEQ_QuestionDesc;
                      
                        _dbContext.Update(result);
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }
                }
                else
                {
                    var res = _dbContext.LMS_Master_OE_QuestionsDMO.Where(t => t.LMSMOEQ_Question == data.LMSMOEQ_Question && t.LMSMOEQ_Marks == data.LMSMOEQ_Marks && t.LMSMOEQ_Id != data.LMSMOEQ_Id && t.MI_Id==data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _dbContext.LMS_Master_OE_QuestionsDMO.Where(t => t.LMSMOEQ_Id == data.LMSMOEQ_Id && t.MI_Id==data.MI_Id).ToList().Count;
                        LMS_Master_OE_QuestionsDMO oe = new LMS_Master_OE_QuestionsDMO();
                        oe.MI_Id = data.MI_Id;
                        oe.ISMS_Id = data.ISMS_Id;
                        oe.LMSMOEQ_Question = data.LMSMOEQ_Question;
                        oe.LMSMOEQ_Marks = data.LMSMOEQ_Marks;
                        oe.LMSMOEQ_QuestionDesc = data.LMSMOEQ_QuestionDesc;
                   
                        _dbContext.Add(oe);
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
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
                _log.LogInformation("Master Quota save data :" + ex.Message);
            }
            return data;
        }
        
        public MasterQuestionDTO savedataclass(MasterQuestionDTO data)
        {
            try
            {
                if (data.LMSMOEQB_Id != 0)
                {

                    var res = _dbContext.LMS_Master_OE_Questions_BranchDMO.Where(t => t.LMSMOEQ_Id==data.LMSMOEQ_Id && t.AMCO_Id == data.AMCO_Ids[0] && t.AMB_Id==data.AMB_Ids[0] && t.AMSE_Id==data.AMSE_Ids[0]).ToList();
                    
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _dbContext.LMS_Master_OE_Questions_BranchDMO.Single(t => t.LMSMOEQB_Id == data.LMSMOEQB_Id);
                        result.LMSMOEQ_Id = data.LMSMOEQ_Id;
                        result.AMCO_Id = data.AMCO_Ids[0];
                        result.AMB_Id = data.AMB_Ids[0];
                        result.AMSE_Id = data.AMSE_Ids[0];

                        _dbContext.Update(result);
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                    }
                }
                else
                {
                  
                    var res = _dbContext.LMS_Master_OE_Questions_BranchDMO.Where(t => t.LMSMOEQ_Id == data.LMSMOEQ_Id && t.AMCO_Id == data.AMCO_Ids[0] && t.AMB_Id == data.AMB_Ids[0] && t.AMSE_Id == data.AMSE_Ids[0] && t.LMSMOEQB_Id != data.LMSMOEQB_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {


                        foreach (var x in data.AMCO_Ids)
                        {
                            foreach (var y in data.AMB_Ids)
                            {
                                foreach (var z in data.AMSE_Ids)
                                {
                                    LMS_Master_OE_Questions_BranchDMO obj1 = new LMS_Master_OE_Questions_BranchDMO();
                                    obj1.LMSMOEQ_Id = data.LMSMOEQ_Id;
                                    obj1.AMCO_Id = x;
                                    obj1.AMB_Id = y;
                                    obj1.AMSE_Id = z;
                                    _dbContext.Add(obj1);
                                }
                            }
                        }
                        
                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists == 1)
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
                _log.LogInformation("Master Quota save data :" + ex.Message);
            }
            return data;
        }

        public MasterQuestionDTO editQuestion(MasterQuestionDTO data)
        {
            try
            {
                data.editQus = _dbContext.LMS_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.LMSMOEQ_Id == data.LMSMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Question editQuestion :" + ex.Message);
            }
            return data;
        }
        
        //--------------------------------------2nd TAB
        public MasterQuestionDTO optionChange(MasterQuestionDTO data)
        {
            try
            {

                var logo = _dbContext.LMS_Master_OE_SettingDMO.Where(d => d.MI_Id == data.MI_Id).ToList();
                data.noopt = logo.FirstOrDefault().LMSMOES_NoOfOptions;

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public MasterQuestionDTO savedetails1(MasterQuestionDTO data)
        {
            try
            {

                foreach (var a in data.seleted_Ans)
                {
                    var res = _dbContext.LMS_Master_OE_QNS_OptionsDMO.Where(t => t.LMSMOEQ_Id == data.LMSMOEQ_Id && t.LMSMOEQOA_Option == a.LMSMOEQOA_Option && t.LMSMOEQOA_OptionCode == a.LMSMOEQOA_OptionCode).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        LMS_Master_OE_QNS_OptionsDMO option = new LMS_Master_OE_QNS_OptionsDMO();

                        option.LMSMOEQ_Id = data.LMSMOEQ_Id;
                        option.LMSMOEQOA_Option = a.LMSMOEQOA_Option;
                        option.LMSMOEQOA_OptionCode = a.LMSMOEQOA_OptionCode;
                        option.LMSMOEQOA_AnswerFlag = a.LMSMOEQOA_AnswerFlag;
                        //option.UpdatedDate = DateTime.Now;
                        //option.CreatedDate = DateTime.Now;
                        _dbContext.Add(option);
                    }
                }

                var contactExists = _dbContext.SaveChanges();
                if (contactExists > 0)
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
                _log.LogInformation("Answer Options savedetails1 :" + ex.Message);
            }
            return data;
        }

        public MasterQuestionDTO optiondetails(MasterQuestionDTO data)
        {
            try
            {
                data.getoptiondetails = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                         from b in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                                         where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && a.LMSMOEQ_Id == data.LMSMOEQ_Id && a.MI_Id == data.MI_Id)
                                         select new MasterQuestionDTO
                                         {
                                             LMSMOEQOA_Id = b.LMSMOEQOA_Id,
                                             LMSMOEQ_Id = a.LMSMOEQ_Id,
                                             LMSMOEQ_Question = a.LMSMOEQ_Question,
                                             LMSMOEQ_Marks = a.LMSMOEQ_Marks,
                                             LMSMOEQOA_OptionCode = b.LMSMOEQOA_OptionCode,
                                             LMSMOEQOA_Option = b.LMSMOEQOA_Option,
                                             LMSMOEQOA_AnswerFlag = b.LMSMOEQOA_AnswerFlag
                                         }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }


        public MasterQuestionDTO Deletedetails(MasterQuestionDTO data)
        {
            try
            {
                var lorg1 = _dbContext.LMS_Master_OE_QNS_OptionsDMO.Where(t => t.LMSMOEQ_Id == data.LMSMOEQ_Id).ToList();
                if (lorg1.Any())
                {
                    for (int i = 0; lorg1.Count > i; i++)
                    {
                        _dbContext.Remove(lorg1.ElementAt(i));
                    }
                }
                
                var contactexisttransaction = 0;
                using (var dbCtxTxn = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _dbContext.SaveChanges();
                        dbCtxTxn.Commit();
                        data.returnval = true;
                    }
                    catch (Exception ex)
                    {
                        dbCtxTxn.Rollback();
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                data.returnval = false;
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        


        public MasterQuestionDTO selectcourse(MasterQuestionDTO data)
        {
            try
            {
                var branchlist = (from a in _dbContext.ClgMasterBranchDMO
                                  from b in _dbContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _dbContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag && data.AMCO_Ids.Contains(b.AMCO_Id))
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToList();
                data.branchlist = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public MasterQuestionDTO selectbran(MasterQuestionDTO data)
        {
            try
            {
                var semisterlist = (from a in _dbContext.CLG_Adm_Master_SemesterDMO
                                    from b in _dbContext.CLG_Adm_College_AY_CourseDMO
                                    from c in _dbContext.CLG_Adm_College_AY_Course_BranchDMO
                                    from d in _dbContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                    where (a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && data.AMCO_Ids.Contains(b.AMCO_Id) && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && data.AMB_Ids.Contains(c.AMB_Id) && c.ACAYCB_ActiveFlag && d.MI_Id == data.MI_Id && d.ACAYCB_Id == c.ACAYCB_Id && d.AMSE_Id == a.AMSE_Id && d.ACAYCBS_ActiveFlag)
                                    select new CLG_Adm_Master_SemesterDMO
                                    {
                                        AMSE_Id = a.AMSE_Id,
                                        AMSE_SEMName = a.AMSE_SEMName,
                                        AMSE_SEMInfo = a.AMSE_SEMInfo,
                                        AMSE_SEMCode = a.AMSE_SEMCode,
                                        AMSE_SEMTypeFlag = a.AMSE_SEMTypeFlag,
                                        AMSE_SEMOrder = a.AMSE_SEMOrder,
                                        AMSE_Year = a.AMSE_Year,
                                        AMSE_EvenOdd = a.AMSE_EvenOdd
                                    }).Distinct().ToList();
                data.semisterlist = semisterlist.OrderBy(t => t.AMSE_SEMOrder).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public MasterQuestionDTO editbranchquestion(MasterQuestionDTO data)
        {
            try
            {
                var result = _dbContext.LMS_Master_OE_Questions_BranchDMO.Where(t => t.LMSMOEQB_Id == data.LMSMOEQB_Id).ToList();

                long AMCO_id, AMB_id, AMSE_id;

                for (int i = 1; result.Count == i; i++)
                {
                    AMCO_id = result[0].AMCO_Id;
                    AMB_id = result[0].AMB_Id;
                    AMSE_id = result[0].AMSE_Id;

                    data.courselist = _dbContext.MasterCourseDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_Id == AMCO_id).ToArray();

                    data.branchlist = _dbContext.ClgMasterBranchDMO.Where(t => t.MI_Id == data.MI_Id && t.AMB_Id == AMB_id).ToArray();

                    data.semisterlist = _dbContext.CLG_Adm_Master_SemesterDMO.Where(t => t.MI_Id == data.MI_Id && t.AMSE_Id == AMSE_id).ToArray();
                }


                data.result = ( from d in _dbContext.LMS_Master_OE_Questions_BranchDMO
                               from e in _dbContext.LMS_Master_OE_QuestionsDMO
                               where (e.LMSMOEQ_Id == d.LMSMOEQ_Id && d.LMSMOEQB_Id==data.LMSMOEQB_Id)
                               select new MasterQuestionDTO
                               {
                                   LMSMOEQB_Id = d.LMSMOEQB_Id,
                                   LMSMOEQ_Id = e.LMSMOEQ_Id,
                                   LMSMOEQ_Question = e.LMSMOEQ_Question
                               }).Distinct().ToArray();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
