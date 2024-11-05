using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
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
    public class MasterQuestionImpl : Interfaces.MasterQuestionInterface
    {
        private static ConcurrentDictionary<string, MasterQuestionDTO> _login =new ConcurrentDictionary<string, MasterQuestionDTO>();

        ILogger<MasterQuestionImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public MasterQuestionImpl(DomainModelMsSqlServerContext dbcontext, ILogger<MasterQuestionImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public MasterQuestionDTO getloaddata(MasterQuestionDTO data)
        {
            try
            {
                data.getQuestiondetails = _dbContext.LMS_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getQuestiondetails = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                           from b in _dbContext.MasterSubjectList
                                           where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id)
                                           select new MasterQuestionDTO
                                           {
                                               LMSMOEQ_Id = a.LMSMOEQ_Id,
                                               LMSMOEQ_Question = a.LMSMOEQ_Question,
                                               LMSMOEQ_QuestionDesc = a.LMSMOEQ_QuestionDesc,
                                               LMSMOEQ_Marks = a.LMSMOEQ_Marks,
                                               ISMS_Id = b.ISMS_Id,
                                               ISMS_SubjectName = b.ISMS_SubjectName,
                                               orderdate = a.CreatedDate,
                                               countview = _dbContext.LMS_Master_OE_Questions_FilesDMO.Where(b => b.LMSMOEQ_Id == a.LMSMOEQ_Id).Count()
                                           }).Distinct().OrderByDescending(a => a.orderdate).ToArray();

                data.getclass = _dbContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getSubjects = _dbContext.MasterSubjectList.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1 && a.ISMS_ExamFlag == 1).ToArray();

                //--------------------2nd Tab


                //data.getAnsOptions = _dbContext.PA_Master_OE_ANS_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.PAMOEAO_OptionsFlag == "A").ToArray();

                //data.getFQOptiondetails = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                //                           from b in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                //                           where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && a.MI_Id == data.MI_Id)
                //                           select new MasterQuestionDTO
                //                           {
                //                               LMSMOEQ_Id = a.LMSMOEQ_Id,
                //                               LMSMOEQ_Question = a.LMSMOEQ_Question,
                //                               LMSMOEQ_Marks = a.LMSMOEQ_Marks,
                //                           }).Distinct().ToArray();

                List<long> questionid = new List<long>();

                var getquesoptionsaved = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                          from b in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                                          where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && a.MI_Id == data.MI_Id)
                                          select new MasterQuestionDTO
                                          {
                                              LMSMOEQ_Id = a.LMSMOEQ_Id
                                          }).Distinct().ToList();

                if (getquesoptionsaved.Count() > 0)
                {
                    foreach (var item in getquesoptionsaved)
                    {
                        questionid.Add(item.LMSMOEQ_Id);
                    }
                }
                else
                {
                    questionid.Add(0);
                }

                data.getFQOptiondetails = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                           from c in _dbContext.MasterSubjectList
                                           where (a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && questionid.Contains(a.LMSMOEQ_Id))
                                           select new MasterQuestionDTO
                                           {
                                               LMSMOEQ_Id = a.LMSMOEQ_Id,
                                               LMSMOEQ_Question = a.LMSMOEQ_Question,
                                               LMSMOEQ_Marks = a.LMSMOEQ_Marks,
                                               ISMS_SubjectName = c.ISMS_SubjectName,
                                               orderdate = a.CreatedDate
                                           }).Distinct().OrderByDescending(a => a.orderdate).ToArray();


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
                                            from c in _dbContext.MasterSubjectList
                                            where (a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && !PAMOEQ_Ids.Contains(a.LMSMOEQ_Id))
                                            select new MasterQuestionDTO
                                            {
                                                LMSMOEQ_Id = a.LMSMOEQ_Id,
                                                LMSMOEQ_Question = a.LMSMOEQ_Question,
                                                LMSMOEQ_Marks = a.LMSMOEQ_Marks,
                                                ISMS_SubjectName = c.ISMS_SubjectName
                                            }).Distinct().ToArray();


                data.result = (from a in _dbContext.School_M_Class
                               from b in _dbContext.LMS_Master_OE_QuestionsDMO
                               from c in _dbContext.LMS_Master_OE_Questions_ClassDMO
                               from d in _dbContext.MasterSubjectList
                               where (b.LMSMOEQ_Id == c.LMSMOEQ_Id && b.ISMS_Id == d.ISMS_Id && a.ASMCL_Id == c.ASMCL_Id && a.MI_Id == data.MI_Id)
                               select new MasterQuestionDTO
                               {
                                   LMSMOEQ_Id = c.LMSMOEQC_Id,
                                   LMSMOEQ_Question = b.LMSMOEQ_Question,
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
        public MasterQuestionDTO savedetails(MasterQuestionDTO data)
        {
            try
            {
                if (data.LMSMOEQ_Id != 0)
                {
                    var res = _dbContext.LMS_Master_OE_QuestionsDMO.Where(t => t.LMSMOEQ_Question.Contains(data.LMSMOEQ_Question) && t.LMSMOEQ_Id != data.LMSMOEQ_Id
                    && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        List<long> filesid = new List<long>();

                        foreach (var c in data.uploadquestionfiles)
                        {
                            filesid.Add(c.LMSMOEQF_Id);
                        }

                        Array previous_Noresultremove = _dbContext.LMS_Master_OE_Questions_FilesDMO.Where(t => !filesid.Contains(t.LMSMOEQF_Id)
                        && t.LMSMOEQ_Id == data.LMSMOEQ_Id).ToArray();

                        foreach (LMS_Master_OE_Questions_FilesDMO ph1 in previous_Noresultremove)
                        {
                            _dbContext.Remove(ph1);
                        }

                        var result = _dbContext.LMS_Master_OE_QuestionsDMO.Single(t => t.LMSMOEQ_Id == data.LMSMOEQ_Id && t.MI_Id == data.MI_Id);
                        result.MI_Id = data.MI_Id;
                        result.ISMS_Id = data.ISMS_Id;
                        result.LMSMOEQ_Question = data.LMSMOEQ_Question;
                        result.LMSMOEQ_Marks = data.LMSMOEQ_Marks;
                        result.LMSMOEQ_QuestionDesc = data.LMSMOEQ_QuestionDesc;
                        result.UpdatedDate = DateTime.Now;
                        _dbContext.Update(result);

                        if (data.uploadquestionfiles.Length > 0)
                        {
                            foreach (var c in data.uploadquestionfiles)
                            {
                                if (c.LMSMOEQF_Id > 0)
                                {
                                    if (c.LMSMOEQF_FilePath != null && c.LMSMOEQF_FilePath != "")
                                    {
                                        var resultfiles = _dbContext.LMS_Master_OE_Questions_FilesDMO.Single(a => a.LMSMOEQF_Id == c.LMSMOEQF_Id
                                        && a.LMSMOEQ_Id == data.LMSMOEQ_Id);
                                        resultfiles.LMSMOEQF_FileName = c.LMSMOEQF_FileName;
                                        resultfiles.LMSMOEQF_FilePath = c.LMSMOEQF_FilePath;
                                        resultfiles.UpdatedDate = DateTime.Now;
                                        _dbContext.Update(resultfiles);
                                    }
                                }
                                else
                                {
                                    if (c.LMSMOEQF_FilePath != null && c.LMSMOEQF_FilePath != "")
                                    {
                                        LMS_Master_OE_Questions_FilesDMO lMS_Master_OE_Questions_FilesDMO = new LMS_Master_OE_Questions_FilesDMO();
                                        lMS_Master_OE_Questions_FilesDMO.LMSMOEQ_Id = data.LMSMOEQ_Id;
                                        lMS_Master_OE_Questions_FilesDMO.LMSMOEQF_FileName = c.LMSMOEQF_FileName;
                                        lMS_Master_OE_Questions_FilesDMO.LMSMOEQF_FilePath = c.LMSMOEQF_FilePath;
                                        lMS_Master_OE_Questions_FilesDMO.LMSMOEQF_ActiveFlag = true;
                                        lMS_Master_OE_Questions_FilesDMO.CreatedDate = DateTime.Now;
                                        lMS_Master_OE_Questions_FilesDMO.UpdatedDate = DateTime.Now;
                                        _dbContext.Add(lMS_Master_OE_Questions_FilesDMO);
                                    }
                                }
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
                }
                else
                {
                    var res = _dbContext.LMS_Master_OE_QuestionsDMO.Where(t => t.LMSMOEQ_Question == data.LMSMOEQ_Question && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _dbContext.LMS_Master_OE_QuestionsDMO.Where(t => t.MI_Id == data.MI_Id).ToList().Count;
                        LMS_Master_OE_QuestionsDMO oe = new LMS_Master_OE_QuestionsDMO();
                        oe.MI_Id = data.MI_Id;
                        oe.ISMS_Id = data.ISMS_Id;
                        oe.LMSMOEQ_Question = data.LMSMOEQ_Question;
                        oe.LMSMOEQ_Marks = data.LMSMOEQ_Marks;
                        oe.LMSMOEQ_QuestionDesc = data.LMSMOEQ_QuestionDesc;
                        oe.CreatedDate = DateTime.Now;
                        oe.UpdatedDate = DateTime.Now;
                        _dbContext.Add(oe);

                        if (data.uploadquestionfiles.Length > 0)
                        {
                            foreach (var c in data.uploadquestionfiles)
                            {
                                if (c.LMSMOEQF_FilePath != null && c.LMSMOEQF_FilePath != "")
                                {
                                    LMS_Master_OE_Questions_FilesDMO lMS_Master_OE_Questions_FilesDMO = new LMS_Master_OE_Questions_FilesDMO();
                                    lMS_Master_OE_Questions_FilesDMO.LMSMOEQ_Id = oe.LMSMOEQ_Id;
                                    lMS_Master_OE_Questions_FilesDMO.LMSMOEQF_FileName = c.LMSMOEQF_FileName;
                                    lMS_Master_OE_Questions_FilesDMO.LMSMOEQF_FilePath = c.LMSMOEQF_FilePath;
                                    lMS_Master_OE_Questions_FilesDMO.LMSMOEQF_ActiveFlag = true;
                                    lMS_Master_OE_Questions_FilesDMO.CreatedDate = DateTime.Now;
                                    lMS_Master_OE_Questions_FilesDMO.UpdatedDate = DateTime.Now;
                                    _dbContext.Add(lMS_Master_OE_Questions_FilesDMO);
                                }
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
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Quota save data :" + ex.Message);
            }
            return data;
        }
        public MasterQuestionDTO viewdocumetns(MasterQuestionDTO data)
        {
            try
            {
                data.viewdocarray = _dbContext.LMS_Master_OE_Questions_FilesDMO.Where(a => a.LMSMOEQ_Id == data.LMSMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterQuestionDTO deactiveparticulars(MasterQuestionDTO data)
        {
            try
            {
                var checkresult = _dbContext.LMS_Master_OE_Questions_FilesDMO.Where(a => a.LMSMOEQF_Id == data.LMSMOEQF_Id).ToList();

                if (checkresult.Count > 0)
                {
                    var result = _dbContext.LMS_Master_OE_Questions_FilesDMO.Single(a => a.LMSMOEQF_Id == data.LMSMOEQF_Id);

                    if (result.LMSMOEQF_ActiveFlag == true)
                    {
                        result.LMSMOEQF_ActiveFlag = false;
                    }
                    else
                    {
                        result.LMSMOEQF_ActiveFlag = true;
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

                data.viewdocarray = _dbContext.LMS_Master_OE_Questions_FilesDMO.Where(a => a.LMSMOEQ_Id == data.LMSMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Question viewdocarray :" + ex.Message);
            }
            return data;
        }
        public MasterQuestionDTO editQuestion(MasterQuestionDTO data)
        {
            try
            {
                data.editQus = _dbContext.LMS_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.LMSMOEQ_Id == data.LMSMOEQ_Id).ToArray();
                data.geteditdocs = _dbContext.LMS_Master_OE_Questions_FilesDMO.Where(a => a.LMSMOEQ_Id == data.LMSMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                _log.LogInformation("Master Question editQuestion :" + ex.Message);
            }
            return data;
        }
        public MasterQuestionDTO savedataclass(MasterQuestionDTO data)
        {
            try
            {
                if (data.LMSMOEQC_Id > 0)
                {
                    var res = _dbContext.LMS_Master_OE_Questions_ClassDMO.Where(t => t.LMSMOEQ_Id == data.LMSMOEQ_Id && t.ASMCL_Id == data.ASMCL_Id
                    && t.LMSMOEQC_Id != data.LMSMOEQC_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _dbContext.LMS_Master_OE_Questions_ClassDMO.Single(t => t.LMSMOEQC_Id == data.LMSMOEQC_Id);
                        result.LMSMOEQ_Id = data.LMSMOEQ_Id;
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
                    var res = _dbContext.LMS_Master_OE_Questions_ClassDMO.Where(t => t.LMSMOEQ_Id == data.LMSMOEQ_Id && t.ASMCL_Id == data.ASMCL_Id).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _dbContext.LMS_Master_OE_Questions_ClassDMO.Where(t => t.LMSMOEQ_Id == data.LMSMOEQ_Id).ToList().Count;
                        LMS_Master_OE_Questions_ClassDMO oe = new LMS_Master_OE_Questions_ClassDMO();

                        oe.LMSMOEQ_Id = data.LMSMOEQ_Id;
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
    }
}