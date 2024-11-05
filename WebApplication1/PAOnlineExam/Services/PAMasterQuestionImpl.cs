using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.OnlineExam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.PAOnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.PAOnlineExam.Services
{
    public class PAMasterQuestionImpl : Interface.PAMasterQuestionInterface
    {
        ILogger<PAMasterQuestionImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public PAMasterQuestionImpl(DomainModelMsSqlServerContext dbcontext, ILogger<PAMasterQuestionImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public PAMasterQuestionDTO getloaddata(PAMasterQuestionDTO data)
        {
            try
            {
                data.getQuestiondetails = _dbContext.PA_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getQuestiondetails = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                           from b in _dbContext.MasterSubjectList
                                           where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id)
                                           select new PAMasterQuestionDTO
                                           {
                                               PAMOEQ_Id = a.PAMOEQ_Id,
                                               PAMOEQ_Question = a.PAMOEQ_Question,
                                               PAMOEQ_QuestionDesc = a.PAMOEQ_QuestionDesc,
                                               PAMOEQ_Marks = a.PAMOEQ_Marks,
                                               ISMS_Id = b.ISMS_Id,
                                               ISMS_SubjectName = b.ISMS_SubjectName,
                                               orderdate = a.CreatedDate,
                                               countview = _dbContext.PA_Master_OE_Questions_FilesDMO.Where(b => b.PAMOEQ_Id == a.PAMOEQ_Id).Count()
                                           }).Distinct().OrderByDescending(a => a.orderdate).ToArray();

                data.getclass = _dbContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getSubjects = _dbContext.MasterSubjectList.Where(a => a.MI_Id == data.MI_Id && a.ISMS_PreadmFlag == 1 && a.ISMS_ActiveFlag == 1).ToArray();

                //--------------------2nd Tab

                List<long> questionid = new List<long>();

                var getquesoptionsaved = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                          from b in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                          where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.MI_Id == data.MI_Id)
                                          select new PAMasterQuestionDTO
                                          {
                                              PAMOEQ_Id = a.PAMOEQ_Id
                                          }).Distinct().ToList();

                if (getquesoptionsaved.Count() > 0)
                {
                    foreach (var item in getquesoptionsaved)
                    {
                        questionid.Add(item.PAMOEQ_Id);
                    }
                }
                else
                {
                    questionid.Add(0);
                }

                data.getFQOptiondetails = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                           from c in _dbContext.MasterSubjectList
                                           where (a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && questionid.Contains(a.PAMOEQ_Id))
                                           select new PAMasterQuestionDTO
                                           {
                                               PAMOEQ_Id = a.PAMOEQ_Id,
                                               PAMOEQ_Question = a.PAMOEQ_Question,
                                               PAMOEQ_Marks = a.PAMOEQ_Marks,
                                               ISMS_SubjectName = c.ISMS_SubjectName,
                                               orderdate = a.CreatedDate
                                           }).Distinct().OrderByDescending(a => a.orderdate).ToArray();

                var questions = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                 from b in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                 where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.MI_Id == data.MI_Id)
                                 select new PAMasterQuestionDTO
                                 {
                                     PAMOEQ_Id = a.PAMOEQ_Id,
                                 }).Distinct().ToArray().ToList();



                List<long> PAMOEQ_Ids = new List<long>();

                foreach (var item in questions)
                {
                    PAMOEQ_Ids.Add(item.PAMOEQ_Id);
                }


                data.getFQuestiondetails = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                            from c in _dbContext.MasterSubjectList
                                            where (a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && !PAMOEQ_Ids.Contains(a.PAMOEQ_Id))
                                            select new PAMasterQuestionDTO
                                            {
                                                PAMOEQ_Id = a.PAMOEQ_Id,
                                                PAMOEQ_Question = a.PAMOEQ_Question,
                                                PAMOEQ_Marks = a.PAMOEQ_Marks,
                                                ISMS_SubjectName = c.ISMS_SubjectName
                                            }).Distinct().ToArray();


                data.result = (from a in _dbContext.School_M_Class
                               from b in _dbContext.PA_Master_OE_QuestionsDMO
                               from c in _dbContext.PA_Master_OE_Questions_ClassDMO
                               from d in _dbContext.MasterSubjectList
                               where (b.PAMOEQ_Id == c.PAMOEQ_Id && b.ISMS_Id == d.ISMS_Id && a.ASMCL_Id == c.ASMCL_Id && a.MI_Id == data.MI_Id)
                               select new PAMasterQuestionDTO
                               {
                                   PAMOEQC_Id = c.PAMOEQC_Id,
                                   PAMOEQ_Question = b.PAMOEQ_Question,
                                   ASMCL_ClassName = a.ASMCL_ClassName,
                                   ASMCL_Id = a.ASMCL_Id,
                                   ISMS_SubjectName = d.ISMS_SubjectName,
                                   orderdate = c.CreatedDate
                               }).Distinct().OrderByDescending(a => a.orderdate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public PAMasterQuestionDTO savedetails(PAMasterQuestionDTO data)
        {
            try
            {
                if (data.PAMOEQ_Id != 0)
                {
                    var res = _dbContext.PA_Master_OE_QuestionsDMO.Where(t => t.PAMOEQ_Question.Contains(data.PAMOEQ_Question)
                     && t.PAMOEQ_Id != data.PAMOEQ_Id && t.MI_Id == data.MI_Id).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        List<long> filesid = new List<long>();

                        foreach (var c in data.uploadquestionfiles)
                        {
                            filesid.Add(c.PAMOEQF_Id);
                        }

                        Array previous_Noresultremove = _dbContext.PA_Master_OE_Questions_FilesDMO.Where(t => !filesid.Contains(t.PAMOEQF_Id)
                        && t.PAMOEQ_Id == data.PAMOEQ_Id).ToArray();

                        foreach (PA_Master_OE_Questions_FilesDMO ph1 in previous_Noresultremove)
                        {
                            _dbContext.Remove(ph1);
                        }

                        var result = _dbContext.PA_Master_OE_QuestionsDMO.Single(t => t.PAMOEQ_Id == data.PAMOEQ_Id && t.MI_Id == data.MI_Id);
                        result.MI_Id = data.MI_Id;
                        result.ISMS_Id = data.ISMS_Id;
                        result.PAMOEQ_Question = data.PAMOEQ_Question;
                        result.PAMOEQ_Marks = data.PAMOEQ_Marks;
                        result.PAMOEQ_QuestionDesc = data.PAMOEQ_QuestionDesc;
                        result.PAMOEQ_QuestionDesc = data.PAMOEQ_QuestionDesc;
                        result.UpdatedDate = DateTime.Now;

                        _dbContext.Update(result);

                        if (data.uploadquestionfiles.Length > 0)
                        {
                            foreach (var c in data.uploadquestionfiles)
                            {
                                if (c.PAMOEQF_Id > 0)
                                {
                                    if (c.PAMOEQF_FilePath != null && c.PAMOEQF_FilePath != "")
                                    {
                                        var resultfiles = _dbContext.PA_Master_OE_Questions_FilesDMO.Single(a => a.PAMOEQF_Id == c.PAMOEQF_Id && a.PAMOEQ_Id == data.PAMOEQ_Id);
                                        resultfiles.PAMOEQF_FileName = c.PAMOEQF_FileName;
                                        resultfiles.PAMOEQF_FilePath = c.PAMOEQF_FilePath;
                                        resultfiles.UpdatedDate = DateTime.Now;
                                        _dbContext.Update(resultfiles);
                                    }
                                }
                                else
                                {
                                    if (c.PAMOEQF_FilePath != null && c.PAMOEQF_FilePath != "")
                                    {
                                        PA_Master_OE_Questions_FilesDMO pA_Master_OE_Questions_FilesDMO = new PA_Master_OE_Questions_FilesDMO();
                                        pA_Master_OE_Questions_FilesDMO.PAMOEQ_Id = data.PAMOEQ_Id;
                                        pA_Master_OE_Questions_FilesDMO.PAMOEQF_FileName = c.PAMOEQF_FileName;
                                        pA_Master_OE_Questions_FilesDMO.PAMOEQF_FilePath = c.PAMOEQF_FilePath;
                                        pA_Master_OE_Questions_FilesDMO.PAMOEQF_ActiveFlag = true;
                                        pA_Master_OE_Questions_FilesDMO.CreatedDate = DateTime.Now;
                                        pA_Master_OE_Questions_FilesDMO.UpdatedDate = DateTime.Now;
                                        _dbContext.Add(pA_Master_OE_Questions_FilesDMO);
                                    }
                                }
                            }
                        }

                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists > 1)
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
                    var res = _dbContext.PA_Master_OE_QuestionsDMO.Where(t => t.PAMOEQ_Question == data.PAMOEQ_Question && t.PAMOEQ_Id != data.PAMOEQ_Id && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _dbContext.PA_Master_OE_QuestionsDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id && t.MI_Id == data.MI_Id).ToList().Count;
                        PA_Master_OE_QuestionsDMO oe = new PA_Master_OE_QuestionsDMO();
                        oe.MI_Id = data.MI_Id;
                        oe.ISMS_Id = data.ISMS_Id;
                        oe.PAMOEQ_Question = data.PAMOEQ_Question;
                        oe.PAMOEQ_Marks = data.PAMOEQ_Marks;
                        oe.PAMOEQ_QuestionDesc = data.PAMOEQ_QuestionDesc;
                        oe.CreatedDate = DateTime.Now;
                        oe.UpdatedDate = DateTime.Now;

                        _dbContext.Add(oe);

                        if (data.uploadquestionfiles.Length > 0)
                        {
                            foreach (var c in data.uploadquestionfiles)
                            {
                                if (c.PAMOEQF_FilePath != null && c.PAMOEQF_FilePath != "")
                                {
                                    PA_Master_OE_Questions_FilesDMO pA_Master_OE_Questions_FilesDMO = new PA_Master_OE_Questions_FilesDMO();
                                    pA_Master_OE_Questions_FilesDMO.PAMOEQ_Id = oe.PAMOEQ_Id;
                                    pA_Master_OE_Questions_FilesDMO.PAMOEQF_FileName = c.PAMOEQF_FileName;
                                    pA_Master_OE_Questions_FilesDMO.PAMOEQF_FilePath = c.PAMOEQF_FilePath;
                                    pA_Master_OE_Questions_FilesDMO.PAMOEQF_ActiveFlag = true;
                                    pA_Master_OE_Questions_FilesDMO.CreatedDate = DateTime.Now;
                                    pA_Master_OE_Questions_FilesDMO.UpdatedDate = DateTime.Now;
                                    _dbContext.Add(pA_Master_OE_Questions_FilesDMO);
                                }
                            }
                        }

                        var contactExists = _dbContext.SaveChanges();
                        if (contactExists > 0)
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
                _log.LogInformation("Master Quota save data :" + ex.Message);
            }
            return data;
        }
        public PAMasterQuestionDTO savedataclass(PAMasterQuestionDTO data)
        {
            try
            {
                if (data.PAMOEQC_Id > 0)
                {
                    var res = _dbContext.PA_Master_OE_Questions_ClassDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id && t.ASMCL_Id == data.ASMCL_Id
                    && t.PAMOEQC_Id != data.PAMOEQC_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _dbContext.PA_Master_OE_Questions_ClassDMO.Single(t => t.PAMOEQC_Id == data.PAMOEQC_Id);
                        result.PAMOEQ_Id = data.PAMOEQ_Id;
                        result.ASMCL_Id = data.ASMCL_Id;
                        result.UpdatedDate = DateTime.Now;

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
                    var res = _dbContext.PA_Master_OE_Questions_ClassDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id && t.ASMCL_Id == data.ASMCL_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _dbContext.PA_Master_OE_Questions_ClassDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id).ToList().Count;
                        PA_Master_OE_Questions_ClassDMO oe = new PA_Master_OE_Questions_ClassDMO();

                        oe.PAMOEQ_Id = data.PAMOEQ_Id;
                        oe.ASMCL_Id = data.ASMCL_Id;
                        oe.CreatedDate = DateTime.Now;
                        oe.UpdatedDate = DateTime.Now;

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
        public PAMasterQuestionDTO viewdocumetns(PAMasterQuestionDTO data)
        {
            try
            {
                data.viewdocarray = _dbContext.PA_Master_OE_Questions_FilesDMO.Where(a => a.PAMOEQ_Id == data.PAMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Question viewdocarray :" + ex.Message);
            }
            return data;
        }
        public PAMasterQuestionDTO deactiveparticulars(PAMasterQuestionDTO data)
        {
            try
            {
                var checkresult = _dbContext.PA_Master_OE_Questions_FilesDMO.Where(a => a.PAMOEQF_Id == data.PAMOEQF_Id).ToList();

                if (checkresult.Count > 0)
                {
                    var result = _dbContext.PA_Master_OE_Questions_FilesDMO.Single(a => a.PAMOEQF_Id == data.PAMOEQF_Id);

                    if (result.PAMOEQF_ActiveFlag == true)
                    {
                        result.PAMOEQF_ActiveFlag = false;
                    }
                    else
                    {
                        result.PAMOEQF_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _dbContext.Update(result);

                    var s = _dbContext.SaveChanges();

                    if (s > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                data.viewdocarray = _dbContext.PA_Master_OE_Questions_FilesDMO.Where(a => a.PAMOEQ_Id == data.PAMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Question viewdocarray :" + ex.Message);
            }
            return data;
        }
        public PAMasterQuestionDTO editQuestion(PAMasterQuestionDTO data)
        {
            try
            {
                data.editQus = _dbContext.PA_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.PAMOEQ_Id == data.PAMOEQ_Id).ToArray();
                data.geteditdocs = _dbContext.PA_Master_OE_Questions_FilesDMO.Where(a => a.PAMOEQ_Id == data.PAMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Question editQuestion :" + ex.Message);
            }
            return data;
        }


        //--------------------------------------2nd TAB
        public PAMasterQuestionDTO optionChange(PAMasterQuestionDTO data)
        {
            try
            {
                var logo = _dbContext.PA_Master_OE_SettingDMO.Where(d => d.MI_Id == data.MI_Id).ToList();
                data.noopt = logo.FirstOrDefault().PAMOES_NoOfOptions;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public PAMasterQuestionDTO savedetails1(PAMasterQuestionDTO data)
        {
            try
            {

                foreach (var a in data.seleted_Ans)
                {
                    var res = _dbContext.PA_Master_OE_QNS_OptionsDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id && t.PAMOEQOA_Option == a.PAMOEQOA_Option
                    && t.PAMOEQOA_OptionCode == a.PAMOEQOA_OptionCode).ToList();

                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        PA_Master_OE_QNS_OptionsDMO option = new PA_Master_OE_QNS_OptionsDMO();

                        option.PAMOEQ_Id = data.PAMOEQ_Id;
                        option.PAMOEQOA_Option = a.PAMOEQOA_Option;
                        option.PAMOEQOA_OptionCode = a.PAMOEQOA_OptionCode;
                        option.PAMOEQOA_AnswerFlag = a.PAMOEQOA_AnswerFlag;
                        option.UpdatedDate = DateTime.Now;
                        option.CreatedDate = DateTime.Now;
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
        public PAMasterQuestionDTO optiondetails(PAMasterQuestionDTO data)
        {
            try
            {
                data.getoptiondetails = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                         from b in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                         where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.PAMOEQ_Id == data.PAMOEQ_Id && a.MI_Id == data.MI_Id)
                                         select new PAMasterQuestionDTO
                                         {
                                             PAMOEQOA_Id = b.PAMOEQOA_Id,
                                             PAMOEQ_Id = a.PAMOEQ_Id,
                                             PAMOEQ_Question = a.PAMOEQ_Question,
                                             PAMOEQ_Marks = a.PAMOEQ_Marks,
                                             PAMOEQOA_OptionCode = b.PAMOEQOA_OptionCode,
                                             PAMOEQOA_Option = b.PAMOEQOA_Option,
                                             PAMOEQOA_AnswerFlag = b.PAMOEQOA_AnswerFlag
                                         }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public PAMasterQuestionDTO Deletedetails(PAMasterQuestionDTO data)
        {
            try
            {
                var lorg1 = _dbContext.PA_Master_OE_QNS_OptionsDMO.Where(t => t.PAMOEQ_Id == data.PAMOEQ_Id).ToList();

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
    }
}
