using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.OnlineExam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.PAOnlineExam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.PAOnlineExam.Services
{
    public class PAOnlineExamImpl : Interface.PAOnlineExamInterface
    {
        ILogger<PAOnlineExamImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public PAOnlineExamImpl(DomainModelMsSqlServerContext dbcontext, ILogger<PAOnlineExamImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public PAOnlineExamDTO getloaddata(PAOnlineExamDTO data)
        {
            try
            {
                var PASR_ID = _dbContext.StudentApplication.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userid).FirstOrDefault().pasr_id;

                var ASMCL_ID = _dbContext.StudentApplication.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userid).FirstOrDefault().ASMCL_Id;

                data.Amst_ID = PASR_ID;
                data.ASMCL_Id = ASMCL_ID;


                List<long> subjid = new List<long>();

                var checksubjid = (from a in _dbContext.PA_Students_ExamDMO
                                   from b in _dbContext.PA_Students_Exam_AnswerDMO
                                   from c in _dbContext.PA_Master_OE_QuestionsDMO
                                   where (a.PASTE_Id == b.PASTE_Id && b.PAMOEQ_Id == c.PAMOEQ_Id && a.MI_Id == data.MI_Id && a.PASR_Id == data.Amst_ID
                                   && a.PASTE_TotalTime != null)
                                   select new PAOnlineExamDTO
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


                data.getSubjects = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                    from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.MasterSubjectList
                                    where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id && c.ISMS_PreadmFlag == 1
                                    && !subjid.Contains(a.ISMS_Id))
                                    select new PAOnlineExamDTO
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
        public PAOnlineExamDTO getSubjects(PAOnlineExamDTO data)
        {
            try
            {
                List<long> subjid = new List<long>();

                var checksubjid = (from a in _dbContext.PA_Students_ExamDMO
                                   from b in _dbContext.PA_Students_Exam_AnswerDMO
                                   from c in _dbContext.PA_Master_OE_QuestionsDMO
                                   where (a.PASTE_Id == b.PASTE_Id && b.PAMOEQ_Id == c.PAMOEQ_Id && a.MI_Id == data.MI_Id && a.PASR_Id == data.Amst_ID
                                   && a.PASTE_TotalTime != null)
                                   select new PAOnlineExamDTO
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

                data.getSubjects = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                    from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.MasterSubjectList
                                    where (a.PAMOEQ_Id == b.PAMOEQ_Id && a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id
                                    && c.ISMS_PreadmFlag == 1 && c.ISMS_ActiveFlag == 1 && !subjid.Contains(a.ISMS_Id))
                                    select new PAOnlineExamDTO
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
        public PAOnlineExamDTO getQuestion(PAOnlineExamDTO data)
        {
            try
            {
                data.getQuestion = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                    from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                    where (a.PAMOEQ_Id == b.PAMOEQ_Id && b.PAMOEQ_Id == c.PAMOEQ_Id && a.MI_Id == data.MI_Id && b.ASMCL_Id == data.ASMCL_Id
                                    && a.ISMS_Id == data.ISMS_Id)
                                    select new PAOnlineExamDTO
                                    {
                                        PAMOEQ_Id = a.PAMOEQ_Id,
                                        PAMOEQOA_Id = c.PAMOEQOA_Id,
                                        PAMOEQ_Question = a.PAMOEQ_Question,
                                        ASMCL_Id = b.ASMCL_Id,
                                        PAMOEQOA_OptionCode = c.PAMOEQOA_OptionCode,
                                        PAMOEQOA_Option = c.PAMOEQOA_Option,
                                        PAMOEQOA_AnswerFlag = c.PAMOEQOA_AnswerFlag
                                    }).Distinct().ToArray();

                data.getQuestiondocuments = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                             from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                             from c in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                             from d in _dbContext.PA_Master_OE_Questions_FilesDMO
                                             where (a.PAMOEQ_Id == b.PAMOEQ_Id && b.PAMOEQ_Id == c.PAMOEQ_Id && a.PAMOEQ_Id == d.PAMOEQ_Id && a.MI_Id == data.MI_Id
                                             && b.ASMCL_Id == data.ASMCL_Id && a.ISMS_Id == data.ISMS_Id && d.PAMOEQF_ActiveFlag == true)
                                             select new PAOnlineExamDTO
                                             {
                                                 PAMOEQ_Id = a.PAMOEQ_Id,
                                                 PAMOEQF_Id = d.PAMOEQF_Id,
                                                 PAMOEQF_FileName = d.PAMOEQF_FileName,
                                                 PAMOEQF_FilePath = d.PAMOEQF_FilePath
                                             }).Distinct().ToArray();

                data.getconnfig = _dbContext.PA_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public PAOnlineExamDTO savedanswers(PAOnlineExamDTO data)
        {
            try
            {
                data.savedanswer = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                    from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                    from d in _dbContext.PA_Students_ExamDMO
                                    from e in _dbContext.PA_Students_Exam_AnswerDMO
                                    where (a.PAMOEQ_Id == b.PAMOEQ_Id && b.PAMOEQ_Id == c.PAMOEQ_Id && a.MI_Id == data.MI_Id && e.PAMOEQOA_Id == c.PAMOEQOA_Id
                                    && d.PASTE_Id == e.PASTE_Id && d.PASR_Id == data.Amst_ID)
                                    select new PAOnlineExamDTO
                                    {
                                        PAMOEQ_Id = e.PAMOEQ_Id,
                                        PAMOEQOA_Id = e.PAMOEQOA_Id,
                                        PAMOEQ_Question = a.PAMOEQ_Question,
                                        PAMOEQOA_OptionCode = c.PAMOEQOA_OptionCode,
                                        PAMOEQOA_Option = c.PAMOEQOA_Option,
                                        PAMOEQOA_AnswerFlag = c.PAMOEQOA_AnswerFlag
                                    }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public PAOnlineExamDTO Saveanswer(PAOnlineExamDTO data)
        {
            try
            {
                data.Amst_ID = _dbContext.StudentApplication.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userid).FirstOrDefault().pasr_id;

                var res = _dbContext.PA_Students_ExamDMO.Where(t => t.PASR_Id == data.Amst_ID).ToList();
                if (res.Count > 0)
                {
                    try
                    {
                        var check = (from a in _dbContext.PA_Students_ExamDMO
                                     from b in _dbContext.PA_Students_Exam_AnswerDMO
                                     where (a.PASTE_Id == b.PASTE_Id && a.PASR_Id == data.Amst_ID && a.MI_Id == data.MI_Id && a.PASTE_Id == res.FirstOrDefault().PASTE_Id && b.PAMOEQ_Id == data.saveanswerlst[0].PAMOEQ_Id)
                                     select new PAOnlineExamDTO
                                     {
                                         PASTE_Id = b.PASTE_Id,
                                         PAMOEQOA_Id = b.PAMOEQOA_Id,
                                     }).ToList();

                        if (check.Count > 0)
                        {
                            var update1 = _dbContext.PA_Students_Exam_AnswerDMO.Where(t => t.PASTE_Id == check.FirstOrDefault().PASTE_Id
                            && t.PAMOEQ_Id == data.saveanswerlst[0].PAMOEQ_Id).SingleOrDefault();

                            update1.PAMOEQOA_Id = data.saveanswerlst[0].QuizeQuastions;
                            update1.UpdatedDate = DateTime.Now;
                            _dbContext.Update(update1);
                            _dbContext.SaveChanges();
                        }

                        else
                        {
                            PA_Students_Exam_AnswerDMO oe = new PA_Students_Exam_AnswerDMO();
                            oe.PASTE_Id = res.FirstOrDefault().PASTE_Id;
                            oe.PAMOEQOA_Id = data.saveanswerlst[0].QuizeQuastions;
                            oe.PAMOEQ_Id = data.saveanswerlst[0].PAMOEQ_Id;
                            oe.CreatedDate = DateTime.Now;
                            oe.UpdatedDate = DateTime.Now;
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
                        PA_Students_ExamDMO oe = new PA_Students_ExamDMO();

                        oe.MI_Id = data.MI_Id;
                        oe.PASR_Id = data.Amst_ID;
                        oe.PASTE_Date = DateTime.Now;
                        oe.PASTE_TotalTime = null;
                        oe.PASTE_TotalQnsAnswered = null;
                        oe.PASTE__TotalCorrectAns = null;
                        oe.PASTE_TotalMaxMarks = null;
                        oe.PASTE_TotalMarks = null;
                        oe.PASTE_Percentage = null;
                        oe.CreatedDate = DateTime.Now;
                        oe.UpdatedDate = DateTime.Now;
                        _dbContext.Add(oe);

                        foreach (var e in data.saveanswerlst)
                        {
                            PA_Students_Exam_AnswerDMO oe1 = new PA_Students_Exam_AnswerDMO();
                            oe1.PASTE_Id = oe.PASTE_Id;
                            oe1.PAMOEQOA_Id = e.QuizeQuastions;
                            oe1.PAMOEQ_Id = e.PAMOEQ_Id;
                            oe1.PAMOEQOA_CorrectAnsFlg = true;
                            oe1.CreatedDate = DateTime.Now;
                            oe1.UpdatedDate = DateTime.Now;
                            _dbContext.Add(oe1);
                        }

                        var contactExists1 = _dbContext.SaveChanges();

                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }

                data.savedanswer = (from a in _dbContext.PA_Master_OE_QuestionsDMO
                                    from b in _dbContext.PA_Master_OE_Questions_ClassDMO
                                    from c in _dbContext.PA_Master_OE_QNS_OptionsDMO
                                    from d in _dbContext.PA_Students_ExamDMO
                                    from e in _dbContext.PA_Students_Exam_AnswerDMO
                                    where (a.PAMOEQ_Id == b.PAMOEQ_Id && b.PAMOEQ_Id == c.PAMOEQ_Id && a.MI_Id == data.MI_Id && e.PAMOEQOA_Id == c.PAMOEQOA_Id
                                    && d.PASTE_Id == e.PASTE_Id && d.PASR_Id == data.Amst_ID)
                                    select new PAOnlineExamDTO
                                    {
                                        PAMOEQ_Id = e.PAMOEQ_Id,
                                        PAMOEQOA_Id = e.PAMOEQOA_Id,
                                        PAMOEQ_Question = a.PAMOEQ_Question,
                                        PAMOEQOA_OptionCode = c.PAMOEQOA_OptionCode,
                                        PAMOEQOA_Option = c.PAMOEQOA_Option,
                                        PAMOEQOA_AnswerFlag = c.PAMOEQOA_AnswerFlag
                                    }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public async Task<PAOnlineExamDTO> submitexam(PAOnlineExamDTO data)
        {
            try
            {
                data.Amst_ID = _dbContext.StudentApplication.Where(a => a.MI_Id == data.MI_Id && a.Id == data.userid).FirstOrDefault().pasr_id;

                using (var cmd = _dbContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PA_Online_Exam";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@examtime",
                        SqlDbType.VarChar)
                    {
                        Value = data.PASTE_TotalTime
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
