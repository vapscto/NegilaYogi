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
    public class OnlineExamCollegeImpl : Interfaces.OnlineExamCollegeInterface
    {
        private static ConcurrentDictionary<string, OnlineExamDTO> _login =
             new ConcurrentDictionary<string, OnlineExamDTO>();

        ILogger<OnlineExamCollegeImpl> _log;
        public DomainModelMsSqlServerContext _dbContext;
        public OnlineExamCollegeImpl(DomainModelMsSqlServerContext dbcontext, ILogger<OnlineExamCollegeImpl> log)
        {
            _dbContext = dbcontext;
            _log = log;
        }
        public OnlineExamDTO getloaddata(OnlineExamDTO data)
        {
            try
            {
               // data.getclass = _dbContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id).ToArray();

               var PASR_ID = _dbContext.PA_College_Application.Where(a => a.MI_Id == data.MI_Id && a.ID==data.userid).FirstOrDefault().PACA_Id;

                var AMSE_Id = _dbContext.PA_College_Application.Where(a => a.MI_Id == data.MI_Id && a.ID == data.userid).FirstOrDefault().AMSE_Id;
                var AMB_ID = _dbContext.PA_College_Application.Where(a => a.MI_Id == data.MI_Id && a.ID == data.userid).FirstOrDefault().AMB_Id;
                var AMCO_Id = _dbContext.PA_College_Application.Where(a => a.MI_Id == data.MI_Id && a.ID == data.userid).FirstOrDefault().AMCO_Id;
                data.AMCST_ID = PASR_ID;
                data.AMSE_Id = AMSE_Id;
                data.AMB_ID = AMB_ID;
                data.AMCO_Id = AMCO_Id;

                data.getSubjects = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                    from b in _dbContext.LMS_Master_OE_Questions_BranchDMO
                                    from c in _dbContext.MasterSubjectList
                                    where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && a.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && b.AMSE_Id == data.AMSE_Id && b.AMB_Id==data.AMB_ID && b.AMCO_Id==data.AMCO_Id && c.ISMS_PreadmFlag == 1)
                                    select new OnlineExamDTO
                                    {
                                        ISMS_Id = c.ISMS_Id,
                                        ISMS_SubjectName = c.ISMS_SubjectName,
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
                data.getSubjects = (from a in _dbContext.LMS_Master_OE_QuestionsDMO
                                    from b in _dbContext.LMS_Master_OE_Questions_BranchDMO
                                    from c in _dbContext.MasterSubjectList
                                    where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && a.ISMS_Id==c.ISMS_Id && a.MI_Id == data.MI_Id && b.AMSE_Id == data.AMSE_Id && b.AMB_Id == data.AMB_ID && b.AMCO_Id == data.AMCO_Id && c.ISMS_PreadmFlag == 1)
                                    select new OnlineExamDTO
                                    {
                                        ISMS_Id = c.ISMS_Id,
                                        ISMS_SubjectName = c.ISMS_SubjectName,
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
                                    from b in _dbContext.LMS_Master_OE_Questions_BranchDMO
                                    from c in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                                    where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && b.LMSMOEQ_Id == c.LMSMOEQ_Id && a.ISMS_Id==data.ISMS_Id)
                                    select new OnlineExamDTO
                                    {
                                        LMSMOEQ_Id = a.LMSMOEQ_Id,
                                        LMSMOEQOA_Id=c.LMSMOEQOA_Id,
                                        LMSMOEQ_Question = a.LMSMOEQ_Question,
                                        LMSMOEQOA_OptionCode = c.LMSMOEQOA_OptionCode,
                                        LMSMOEQOA_Option = c.LMSMOEQOA_Option,
                                        LMSMOEQOA_AnswerFlag=c.LMSMOEQOA_AnswerFlag
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
                                    from b in _dbContext.LMS_Master_OE_Questions_BranchDMO
                                    from c in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                                    from d in _dbContext.LMS_Students_Exam_CollegeDMO
                                    from e in _dbContext.LMS_Students_Exam_Answer_CollegeDMO
                                    where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && b.LMSMOEQ_Id == c.LMSMOEQ_Id && a.MI_Id == data.MI_Id && e.LMSMOEQOA_Id == c.LMSMOEQOA_Id && d.LMSSTECO_Id == e.LMSSTECO_Id && d.AMCST_ID==data.AMCST_ID)
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
                data.AMCST_ID = 877;
                var res = _dbContext.LMS_Students_Exam_CollegeDMO.Where(t => t.AMCST_ID==data.AMCST_ID).ToList();
                if (res.Count > 0)
                {
                    try
                    {
                       

                        var check = (from a in _dbContext.LMS_Students_Exam_CollegeDMO
                                     from b in _dbContext.LMS_Students_Exam_Answer_CollegeDMO
                                     where (a.LMSSTECO_Id == b.LMSSTECO_Id && a.AMCST_ID == data.AMCST_ID && a.MI_Id == data.MI_Id && a.LMSSTECO_Id == res.FirstOrDefault().LMSSTECO_Id && b.LMSMOEQ_Id== data.saveanswerlst[0].LMSMOEQ_Id)
                                     select new OnlineExamDTO
                                     {
                                         LMSSTECO_Id = b.LMSSTECO_Id,
                                         LMSMOEQOA_Id = b.LMSMOEQOA_Id,
                                     }).ToList();

                        if (check.Count > 0)
                        {
                            var update1 = _dbContext.LMS_Students_Exam_Answer_CollegeDMO.Where(t => t.LMSSTECO_Id == check.FirstOrDefault().LMSSTE_Id && t.LMSMOEQ_Id== data.saveanswerlst[0].LMSMOEQ_Id).SingleOrDefault();


                            update1.LMSMOEQOA_Id = data.saveanswerlst[0].QuizeQuastions;
                            _dbContext.Update(update1);
                            _dbContext.SaveChanges();
                        }

                        else
                        {
                            LMS_Students_Exam_Answer_CollegeDMO oe = new LMS_Students_Exam_Answer_CollegeDMO();
                            oe.LMSSTECO_Id = res.FirstOrDefault().LMSSTECO_Id;
                            oe.LMSMOEQOA_Id = data.saveanswerlst[0].QuizeQuastions;
                            oe.LMSMOEQ_Id = data.saveanswerlst[0].LMSMOEQ_Id;
                            //oe.CreatedDate = DateTime.Now;
                            //oe.UpdatedDate = DateTime.Now;
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
                        LMS_Students_Exam_CollegeDMO oe = new LMS_Students_Exam_CollegeDMO();
                        
                        oe.MI_Id = data.MI_Id;
                        oe.AMCST_ID = data.AMCST_ID;
                        oe.LMSSTECO_Date = DateTime.Now;
                        oe.LMSSTECO_TotalTime =null;
                        oe.LMSSTECO_TotalQnsAnswered = null;
                        oe.LMSSTECO_TotalCorrectAns = null;
                        oe.LMSSTECO_TotalMaxMarks = null;
                        oe.LMSSTECO_TotalMarks = null;
                        oe.LMSSTECO_Percentage = null;
                       
                        _dbContext.Add(oe);


                      
                        foreach (var e in data.saveanswerlst)
                        {
                            LMS_Students_Exam_Answer_CollegeDMO oe1 = new LMS_Students_Exam_Answer_CollegeDMO();
                            oe1.LMSSTECO_Id = oe.LMSSTECO_Id;
                            oe1.LMSMOEQOA_Id = e.QuizeQuastions;
                            oe1.LMSMOEQ_Id = e.LMSMOEQ_Id;
                            oe1.LMSSTEACO_CorrectAnsFlg = true;
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
                                    from b in _dbContext.LMS_Master_OE_Questions_BranchDMO
                                    from c in _dbContext.LMS_Master_OE_QNS_OptionsDMO
                                    from d in _dbContext.LMS_Students_Exam_CollegeDMO
                                    from e in _dbContext.LMS_Students_Exam_Answer_CollegeDMO
                                    where (a.LMSMOEQ_Id == b.LMSMOEQ_Id && b.LMSMOEQ_Id == c.LMSMOEQ_Id && a.MI_Id == data.MI_Id && e.LMSMOEQOA_Id == c.LMSMOEQOA_Id && d.LMSSTECO_Id == e.LMSSTECO_Id && d.AMCST_ID == data.AMCST_ID)
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
                    cmd.CommandText = "Online_Exam_College";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;
                    cmd.Parameters.Add(new SqlParameter("@examtime",
                        SqlDbType.VarChar)
                    {
                        Value = data.LMSSTE_TotalTime
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_ID",
                       SqlDbType.BigInt)
                    {
                        Value = data.AMCST_ID
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
                 //   cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                 //SqlDbType.BigInt)
                 //   {
                 //       Value = data.ASMCL_Id
                 //   });

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
