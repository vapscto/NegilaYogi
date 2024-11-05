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
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApplication1.Services
{
    public class OnlineExamImpl : Interfaces.OnlineExamInterface
    {
        private static ConcurrentDictionary<string, OnlineExamDTO> _login = new ConcurrentDictionary<string, OnlineExamDTO>();

        ILogger<OnlineExamImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public OnlineExamImpl(DomainModelMsSqlServerContext dbcontext, ILogger<OnlineExamImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public OnlineExamDTO getloaddata(OnlineExamDTO data)
        {
            try
            {
                // data.getclass = _dbContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id).ToArray();

                var PASR_ID = _dbContext.StudentApplication.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userid).FirstOrDefault().pasr_id;

                var ASMCL_ID = _dbContext.StudentApplication.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userid).FirstOrDefault().ASMCL_Id;

                data.Amst_ID = PASR_ID;
                data.ASMCL_Id = ASMCL_ID;

                List<long> subjid = new List<long>();

                var checksubjid = (from a in _dbContext.LMS_Students_ExamDMO
                                   from b in _dbContext.LMS_Students_Exam_AnswerDMO
                                   from c in _dbContext.LMS_Master_OE_QuestionsDMO
                                   where (a.LMSSTE_Id == b.LMSSTE_Id && b.LMSMOEQ_Id == c.LMSMOEQ_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_ID
                                   && a.LMSSTE_TotalTime != null)
                                   select new OnlineExamDTO
                                   {
                                       ISMS_Id = c.ISMS_Id
                                   }).Distinct().ToList();

                if (checksubjid.Count > 0)
                {
                    foreach (var c in checksubjid)
                    {
                        subjid.Add(c.ISMS_Id);
                    }
                }
                else
                {
                    subjid.Add(0);
                }


                data.getSubjects = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                    from b in _dbContext.LMS_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.MasterSubjectList
                                    where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && c.ISMS_PreadmFlag == 1 && !subjid.Contains(a.ISMS_Id))
                                    select new OnlineExamDTO
                                    {
                                        ISMS_Id = c.ISMS_Id,
                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                        ASMCL_Id = b.ASMCL_Id,
                                    }).Distinct().ToArray();



                data.getQdetails = _dbContext.LMS_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public OnlineExamDTO getSubjects(OnlineExamDTO data)
        {
            try
            {
                List<long> subjid = new List<long>();

                var checksubjid = (from a in _dbContext.LMS_Students_ExamDMO
                                   from b in _dbContext.LMS_Students_Exam_AnswerDMO
                                   from c in _dbContext.LMS_Master_OE_QuestionsDMO
                                   where (a.LMSSTE_Id == b.LMSSTE_Id && b.LMSMOEQ_Id == c.LMSMOEQ_Id && a.MI_Id == data.MI_Id && a.AMST_Id == data.Amst_ID
                                   && a.LMSSTE_TotalTime != null)
                                   select new OnlineExamDTO
                                   {
                                       ISMS_Id = c.ISMS_Id
                                   }).Distinct().ToList();

                if (checksubjid.Count > 0)
                {
                    foreach (var c in checksubjid)
                    {
                        subjid.Add(c.ISMS_Id);
                    }
                }
                else
                {
                    subjid.Add(0);
                }

                data.getSubjects = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                    from b in _dbContext.LMS_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.MasterSubjectList
                                    where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id
                                    && c.ISMS_PreadmFlag == 1 && c.ISMS_ActiveFlag == 1 && !subjid.Contains(a.ISMS_Id))
                                    select new OnlineExamDTO
                                    {
                                        ISMS_Id = c.ISMS_Id,
                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                        ASMCL_Id = b.ASMCL_Id,
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public OnlineExamDTO getQuestion(OnlineExamDTO data)
        {
            try
            {
                data.getQuestion = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                    from b in _dbContext.LMS_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                                    where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && b.LMSMOEQ_Id == c.LMSMOEQ_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && a.ISMS_Id == data.ISMS_Id)
                                    select new OnlineExamDTO
                                    {
                                        LMSMOEQ_Id = a.LMSMOEQ_Id,
                                        LMSMOEQOA_Id = c.LMSMOEQOA_Id,
                                        LMSMOEQ_Question = a.LMSMOEQ_Question,
                                        ASMCL_Id = b.ASMCL_Id,
                                        LMSMOEQOA_OptionCode = c.LMSMOEQOA_OptionCode,
                                        LMSMOEQOA_Option = c.LMSMOEQOA_Option,
                                        LMSMOEQOA_AnswerFlag = c.LMSMOEQOA_AnswerFlag
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public OnlineExamDTO savedanswers(OnlineExamDTO data)
        {
            try
            {
                data.savedanswer = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                    from b in _dbContext.LMS_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                                    from d in _dbContext.LMS_Students_ExamDMO
                                    from e in _dbContext.LMS_Students_Exam_AnswerDMO
                                    where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && b.LMSMOEQ_Id == c.LMSMOEQ_Id && a.MI_Id == data.MI_Id && e.LMSMOEQOA_Id == c.LMSMOEQOA_Id && d.LMSSTE_Id == e.LMSSTE_Id && d.AMST_Id == data.Amst_ID)
                                    select new OnlineExamDTO
                                    {
                                        LMSMOEQ_Id = e.LMSMOEQ_Id,
                                        LMSMOEQOA_Id = e.LMSMOEQOA_Id,
                                        LMSMOEQ_Question = a.LMSMOEQ_Question,
                                        LMSMOEQOA_OptionCode = c.LMSMOEQOA_OptionCode,
                                        LMSMOEQOA_Option = c.LMSMOEQOA_Option,
                                        LMSMOEQOA_AnswerFlag = c.LMSMOEQOA_AnswerFlag
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public OnlineExamDTO Saveanswer(OnlineExamDTO data)
        {
            try
            {

                var res = _dbContext.LMS_Students_ExamDMO.Where(t => t.AMST_Id == data.Amst_ID).ToList();
                if (res.Count > 0)
                {
                    try
                    {
                        var check = (from a in _dbContext.LMS_Students_ExamDMO
                                     from b in _dbContext.LMS_Students_Exam_AnswerDMO
                                     where (a.LMSSTE_Id == b.LMSSTE_Id && a.AMST_Id == data.Amst_ID && a.MI_Id == data.MI_Id && a.LMSSTE_Id == res.FirstOrDefault().LMSSTE_Id && b.LMSMOEQ_Id == data.saveanswerlst[0].LMSMOEQ_Id)
                                     select new OnlineExamDTO
                                     {
                                         LMSSTE_Id = b.LMSSTE_Id,
                                         LMSMOEQOA_Id = b.LMSMOEQOA_Id,
                                     }).ToList();

                        if (check.Count > 0)
                        {
                            var update1 = _dbContext.LMS_Students_Exam_AnswerDMO.Where(t => t.LMSSTE_Id == check.FirstOrDefault().LMSSTE_Id && t.LMSMOEQ_Id == data.saveanswerlst[0].LMSMOEQ_Id).SingleOrDefault();
                            update1.LMSMOEQOA_Id = data.saveanswerlst[0].QuizeQuastions;
                            _dbContext.Update(update1);
                            _dbContext.SaveChanges();
                        }

                        else
                        {
                            LMS_Students_Exam_AnswerDMO oe = new LMS_Students_Exam_AnswerDMO();
                            oe.LMSSTE_Id = res.FirstOrDefault().LMSSTE_Id;
                            oe.LMSMOEQOA_Id = data.saveanswerlst[0].QuizeQuastions;
                            oe.LMSMOEQ_Id = data.saveanswerlst[0].LMSMOEQ_Id;
                            _dbContext.Add(oe);
                            var contactExists = _dbContext.SaveChanges();
                        }
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
                else
                {
                    try
                    {
                        LMS_Students_ExamDMO oe = new LMS_Students_ExamDMO();

                        oe.MI_Id = data.MI_Id;
                        oe.AMST_Id = data.Amst_ID;
                        oe.LMSSTE_Date = DateTime.Now;
                        oe.LMSSTE_TotalTime = null;
                        oe.LMSSTE_TotalQnsAnswered = null;
                        oe.LMSSTE_TotalCorrectAns = null;
                        oe.LMSSTE_TotalMaxMarks = null;
                        oe.LMSSTE_TotalMarks = null;
                        oe.LMSSTE_Percentage = null;
                        _dbContext.Add(oe);

                        foreach (var e in data.saveanswerlst)
                        {
                            LMS_Students_Exam_AnswerDMO oe1 = new LMS_Students_Exam_AnswerDMO();
                            oe1.LMSSTE_Id = oe.LMSSTE_Id;
                            oe1.LMSMOEQOA_Id = e.QuizeQuastions;
                            oe1.LMSMOEQ_Id = e.LMSMOEQ_Id;
                            oe1.LMSSTEA_CorrectAnsFlg = true;
                            _dbContext.Add(oe1);
                        }

                        var contactExists1 = _dbContext.SaveChanges();

                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.savedanswer = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                    from b in _dbContext.LMS_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                                    from d in _dbContext.LMS_Students_ExamDMO
                                    from e in _dbContext.LMS_Students_Exam_AnswerDMO
                                    where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && b.LMSMOEQ_Id == c.LMSMOEQ_Id && a.MI_Id == data.MI_Id && e.LMSMOEQOA_Id == c.LMSMOEQOA_Id && d.LMSSTE_Id == e.LMSSTE_Id && d.AMST_Id == data.Amst_ID)
                                    select new OnlineExamDTO
                                    {
                                        LMSMOEQ_Id = e.LMSMOEQ_Id,
                                        LMSMOEQOA_Id = e.LMSMOEQOA_Id,
                                        LMSMOEQ_Question = a.LMSMOEQ_Question,
                                        LMSMOEQOA_OptionCode = c.LMSMOEQOA_OptionCode,
                                        LMSMOEQOA_Option = c.LMSMOEQOA_Option,
                                        LMSMOEQOA_AnswerFlag = c.LMSMOEQOA_AnswerFlag
                                    }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public async Task<OnlineExamDTO> submitexam(OnlineExamDTO data)
        {
            try
            {
                using (var cmd = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Online_Exam";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@examtime",
                        SqlDbType.VarChar)
                    {
                        Value = data.LMSSTE_TotalTime
                    });
                    cmd.Parameters.Add(new SqlParameter("@Amst_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.Amst_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                 SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.result = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}