using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.NAAC.LP_OnlineExam;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using PreadmissionDTOs.NAAC.LP_OnlineExam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NaacServiceHub.LP_OnlineExam.Services
{
    public class LP_OnlineStudentExamImpl : Interface.LP_OnlineStudentExamInterface
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public LessonplannerContext _context;
        public LP_OnlineStudentExamImpl(LessonplannerContext _cont, IHostingEnvironment _hosting)
        {
            _context = _cont;
            _hostingEnvironment = _hosting;
        }
        public LP_OnlineStudentExamDTO getloaddata(LP_OnlineStudentExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getamstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.UserId).ToList();
                if (getamstid.Count > 0)
                {
                    data.AMST_Id = getamstid.FirstOrDefault().AMST_ID;

                    var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                             from b in _context.Adm_M_Student
                                             from c in _context.AcademicYear
                                             from d in _context.AdmissionClass
                                             from e in _context.School_M_Section
                                             where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                             && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                             && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                             select new LP_OnlineStudentExamDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                                 ASMAY_Id = a.ASMAY_Id,
                                                 AMST_Date = b.AMST_Date
                                             }).Distinct().ToList();

                    if (getstudentdetails.Count > 0)
                    {
                        data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                        data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                        data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;
                        data.AMST_Date = getstudentdetails.FirstOrDefault().AMST_Date;

                        var checkexamsubjects = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ASMCL_Id == data.ASMCL_Id && a.LPMOEEX_ActiveFlg == true).ToList();

                        List<long?> subjidd = new List<long?>();

                        foreach (var c in checkexamsubjects)
                        {
                            subjidd.Add(c.ISMS_Id);
                        }

                        var checksubjects = _context.StudentMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ESTSU_ActiveFlg == true && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && subjidd.Contains(a.ISMS_Id)).ToList();

                        if (checksubjects.Count > 0)
                        {
                            List<long?> subjid = new List<long?>();

                            foreach (var c in checksubjects)
                            {
                                subjid.Add(c.ISMS_Id);
                            }

                            List<long> ids = new List<long>();

                            var getstudentexamids = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                            && a.AMST_Id == data.AMST_Id).Distinct().Select(a => a.LPMOEEX_Id);


                            var getstudentexamidsupload = (from a in _context.LP_Students_ExamDMO
                                                           from c in _context.LP_Master_OE_ExamDMO
                                                           where (a.LPMOEEX_Id == c.LPMOEEX_Id && a.MI_Id == data.MI_Id
                                                           && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && c.LPMOEEX_UploadExamPaperFlg == true)
                                                           select a).Distinct().Select(a => a.LPMOEEX_Id);

                            foreach (var c in getstudentexamidsupload)
                            {
                                ids.Add(c);
                            }

                            foreach (var c in getstudentexamids)
                            {
                                ids.Add(c);
                            }

                            //***************** Getting Today's Exam Details *********************//

                            var getexamdetails = (from a in _context.LP_Master_OE_ExamDMO
                                                  from b in _context.IVRM_School_Master_SubjectsDMO
                                                  where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                                  && a.ASMS_Id == data.ASMS_Id && a.LPMOEEX_ActiveFlg == true && subjid.Contains(a.ISMS_Id)
                                                  && !ids.Contains(a.LPMOEEX_Id) && (a.LPMOEEX_FromDateTime <= indiantime0 && a.LPMOEEX_ToDateTime >= indiantime0)
                                                  && a.LPMOEEX_FromDateTime.Value.Date > data.AMST_Date.Value.Date)
                                                  select new LP_OnlineStudentExamDTO
                                                  {
                                                      ISMS_Id = a.ISMS_Id,
                                                      LPMOEEX_Id = a.LPMOEEX_Id,
                                                      LPMOEEX_ExamName = a.LPMOEEX_ExamName,
                                                      ISMS_SubjectName = b.ISMS_SubjectName,
                                                      ExamStartDateTime = a.LPMOEEX_FromDateTime,
                                                      ExamEndDateTime = a.LPMOEEX_ToDateTime,
                                                      LPMOEEX_ExamDuration = a.LPMOEEX_ExamDuration,
                                                      LPMOEEX_TotalMarks = a.LPMOEEX_TotalMarks,
                                                      LPMOEEX_UploadExamPaperFlg = a.LPMOEEX_UploadExamPaperFlg,
                                                      LPMOEEX_QuestionPaper = a.LPMOEEX_QuestionPaper,
                                                      LPMOEEX_QuestionPapeFileName = a.LPMOEEX_QuestionPapeFileName,
                                                      LPMOEEX_AnswerSheet = a.LPMOEEX_AnswerSheet,
                                                      LPMOEEX_AnswerPapeFileName = a.LPMOEEX_AnswerPapeFileName,
                                                      LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg = a.LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg,
                                                      LPMOEEX_Duration = a.LPMOEEX_Duration,
                                                      LPMOEEX_DurationFlag = a.LPMOEEX_DurationFlag,
                                                  }).Distinct().OrderBy(a => a.ExamStartDateTime).ThenBy(a => a.ExamEndDateTime).ToArray();

                            //***************** Getting All Exam Details *********************//

                            var getexamalldetails = (from a in _context.LP_Master_OE_ExamDMO
                                                     from b in _context.IVRM_School_Master_SubjectsDMO
                                                     where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                                     && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.LPMOEEX_ActiveFlg == true
                                                     && subjid.Contains(a.ISMS_Id) && a.LPMOEEX_FromDateTime.Value.Date > data.AMST_Date.Value.Date)
                                                     select new LP_OnlineStudentExamDTO
                                                     {
                                                         ISMS_Id = a.ISMS_Id,
                                                         LPMOEEX_Id = a.LPMOEEX_Id,
                                                         LPMOEEX_ExamName = a.LPMOEEX_ExamName,
                                                         ISMS_SubjectName = b.ISMS_SubjectName,
                                                         ExamStartDateTime = a.LPMOEEX_FromDateTime,
                                                         ExamEndDateTime = a.LPMOEEX_ToDateTime,
                                                         LPMOEEX_ExamDuration = a.LPMOEEX_ExamDuration,
                                                         LPMOEEX_TotalMarks = a.LPMOEEX_TotalMarks,
                                                         LPMOEEX_UploadExamPaperFlg = a.LPMOEEX_UploadExamPaperFlg,
                                                         LPMOEEX_QuestionPaper = a.LPMOEEX_QuestionPaper,
                                                         LPMOEEX_QuestionPapeFileName = a.LPMOEEX_QuestionPapeFileName,
                                                         LPMOEEX_AnswerSheet = a.LPMOEEX_AnswerSheet,
                                                         LPMOEEX_AnswerPapeFileName = a.LPMOEEX_AnswerPapeFileName,
                                                         LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg = a.LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg,
                                                         LPMOEEX_Duration = a.LPMOEEX_Duration,
                                                         LPMOEEX_DurationFlag = a.LPMOEEX_DurationFlag,
                                                     }).Distinct().OrderByDescending(a => a.ExamStartDateTime).ThenByDescending(a => a.ExamEndDateTime).ToArray();

                            //***************** Getting All Exam Details Which Are Completed But Not Submitted *********************//

                            var getexamcompleteddetails = (from a in _context.LP_Master_OE_ExamDMO
                                                           from b in _context.IVRM_School_Master_SubjectsDMO
                                                           where (a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                                           && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.LPMOEEX_ActiveFlg == true
                                                           && subjid.Contains(a.ISMS_Id) && a.LPMOEEX_ToDateTime < indiantime0 && !ids.Contains(a.LPMOEEX_Id)
                                                           && a.LPMOEEX_FromDateTime.Value.Date > data.AMST_Date.Value.Date)
                                                           select new LP_OnlineStudentExamDTO
                                                           {
                                                               ISMS_Id = a.ISMS_Id,
                                                               LPMOEEX_Id = a.LPMOEEX_Id,
                                                               LPMOEEX_ExamName = a.LPMOEEX_ExamName,
                                                               ISMS_SubjectName = b.ISMS_SubjectName,
                                                               ExamStartDateTime = a.LPMOEEX_FromDateTime,
                                                               ExamEndDateTime = a.LPMOEEX_ToDateTime,
                                                               LPMOEEX_ExamDuration = a.LPMOEEX_ExamDuration,
                                                               LPMOEEX_TotalMarks = a.LPMOEEX_TotalMarks,
                                                               LPMOEEX_UploadExamPaperFlg = a.LPMOEEX_UploadExamPaperFlg,
                                                               LPMOEEX_QuestionPaper = a.LPMOEEX_QuestionPaper,
                                                               LPMOEEX_QuestionPapeFileName = a.LPMOEEX_QuestionPapeFileName,
                                                               LPMOEEX_AnswerSheet = a.LPMOEEX_AnswerSheet,
                                                               LPMOEEX_AnswerPapeFileName = a.LPMOEEX_AnswerPapeFileName,
                                                               LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg = a.LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg,
                                                               LPMOEEX_Duration = a.LPMOEEX_Duration,
                                                               LPMOEEX_DurationFlag = a.LPMOEEX_DurationFlag,
                                                           }).Distinct().OrderByDescending(a => a.ExamStartDateTime).ThenByDescending(a => a.ExamEndDateTime).ToArray();


                            //***************** Getting All Exam Details Which Are Completed But Submitted *********************//

                            var getexamsubmitteddetails = (from a in _context.LP_Master_OE_ExamDMO
                                                           from b in _context.IVRM_School_Master_SubjectsDMO
                                                           from c in _context.LP_Students_ExamDMO
                                                           where (a.LPMOEEX_Id == c.LPMOEEX_Id && a.ISMS_Id == b.ISMS_Id && a.MI_Id == data.MI_Id
                                                           && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                                           && a.LPMOEEX_ActiveFlg == true && subjid.Contains(a.ISMS_Id) && ids.Contains(a.LPMOEEX_Id)
                                                           && a.LPMOEEX_FromDateTime.Value.Date > data.AMST_Date.Value.Date)
                                                           select new LP_OnlineStudentExamDTO
                                                           {
                                                               ISMS_Id = a.ISMS_Id,
                                                               LPMOEEX_Id = a.LPMOEEX_Id,
                                                               LPMOEEX_ExamName = a.LPMOEEX_ExamName,
                                                               ISMS_SubjectName = b.ISMS_SubjectName,
                                                               ExamStartDateTime = a.LPMOEEX_FromDateTime,
                                                               ExamEndDateTime = a.LPMOEEX_ToDateTime,
                                                               LPMOEEX_ExamDuration = a.LPMOEEX_ExamDuration,
                                                               LPMOEEX_TotalMarks = a.LPMOEEX_TotalMarks,
                                                               LPMOEEX_UploadExamPaperFlg = a.LPMOEEX_UploadExamPaperFlg,
                                                               viewmarkscount = a.LPMOEEX_ToDateTime < indiantime0 ? 1 : 0,
                                                               viewmarkscountnew = c.LPSTUEX_PublishToStudent == true ? 1 : 0,
                                                               LPMOEEX_QuestionPaper = a.LPMOEEX_QuestionPaper,
                                                               LPMOEEX_QuestionPapeFileName = a.LPMOEEX_QuestionPapeFileName,
                                                               LPMOEEX_AnswerSheet = a.LPMOEEX_AnswerSheet,
                                                               LPMOEEX_AnswerPapeFileName = a.LPMOEEX_AnswerPapeFileName,
                                                               LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg = a.LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg,
                                                               LPMOEEX_Duration = a.LPMOEEX_Duration,
                                                               LPMOEEX_DurationFlag = a.LPMOEEX_DurationFlag,
                                                           }).Distinct().OrderByDescending(a => a.ExamStartDateTime).ThenByDescending(a => a.ExamEndDateTime).ToArray();



                            data.getsubjectdetails = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && subjid.Contains(a.ISMS_Id)
                            && a.ISMS_ActiveFlag == 1).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                            data.gettodaysexamdetails = getexamdetails;
                            data.getallexamdetails = getexamalldetails;
                            data.getexamcompleteddetails = getexamcompleteddetails;
                            data.getexamsubmitteddetails = getexamsubmitteddetails;
                        }
                    }
                }
                else
                {
                    data.message = "Student Details Not Found";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO onselectsubject(LP_OnlineStudentExamDTO data)
        {
            try
            {
                var getamstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.UserId).ToList();
                if (getamstid.Count > 0)
                {
                    data.AMST_Id = getamstid.FirstOrDefault().AMST_ID;

                    var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                             from b in _context.Adm_M_Student
                                             from c in _context.AcademicYear
                                             from d in _context.AdmissionClass
                                             from e in _context.School_M_Section
                                             where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                             && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                             && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                             select new LP_OnlineStudentExamDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                                 ASMAY_Id = a.ASMAY_Id,
                                             }).Distinct().ToList();

                    if (getstudentdetails.Count > 0)
                    {
                        data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                        data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                        data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;

                        var getsavedexamlist = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.LPSTUEX_ActiveFlg == true && a.LPSTUEX_TotalMarks != null && a.AMST_Id == data.AMST_Id).ToList();

                        List<long> ids = new List<long>();
                        foreach (var c in getsavedexamlist)
                        {
                            ids.Add(c.LPMOEEX_Id);
                        }


                        var getexamlist = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ISMS_Id == data.ISMS_Id && a.ASMS_Id == data.ASMS_Id && a.LPMOEEX_ActiveFlg == true && !ids.Contains(a.LPMOEEX_Id)).ToArray();

                        data.getexamlist = getexamlist;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO getQuestion(LP_OnlineStudentExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getamstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.UserId).ToList();

                data.AMST_Id = getamstid.FirstOrDefault().AMST_ID;

                var getexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                var datad = indiantime0.Subtract(Convert.ToDateTime(getexamdetails.FirstOrDefault().LPMOEEX_FromDateTime)).TotalMinutes;

                int i = (int)datad;

                if (i > 15)
                {
                    data.message = "Time Crossed";
                }

                data.getconnfig = _context.LP_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.getexamdetails = getexamdetails.ToArray();

                if (getexamdetails.Count > 0)
                {
                    var getrandomflag = getexamdetails.FirstOrDefault().LPMOEEX_RandomFlg;
                    var getnoofquestion = getexamdetails.FirstOrDefault().LPMOEEX_NoOfQuestion;
                    var getuploadflag = getexamdetails.FirstOrDefault().LPMOEEX_UploadExamPaperFlg;
                    var LPMOEEX_NotLinkedToQnsBankFlg = getexamdetails.FirstOrDefault().LPMOEEX_NotLinkedToQnsBankFlg;

                    if (getuploadflag == false)
                    {
                        data.getexamleveldetails = _context.LP_Master_OE_Exam_LevelsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id).OrderBy(a => a.LPMOEEXLVL_LevelOrder).ToArray();

                        if (LPMOEEX_NotLinkedToQnsBankFlg == true)
                        {
                            var getexamquestionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                       from b in _context.LP_Master_OE_ExamDMO
                                                       from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                       where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                       && a.LPMOEEXQNS_ActiveFlg == true && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQ_Question = a.LPMOEEXQNS_Question,
                                                           LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQ_SubjectiveFlg = a.LPMOEEXQNS_SubjectiveFlg,
                                                           LPMOEQ_MatchTheFollowingFlg = a.LPMOEEXQNS_MatchTheFollowingFlg,
                                                           LPMOEEXQNS_QnsOrder = a.LPMOEEXQNS_QnsOrder,
                                                           LPMOEEXLVL_Id = a.LPMOEEXLVL_Id,
                                                           LPMOEQ_StructuralFlg = a.LPMOEEXQNS_QuestionType
                                                       }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToList();

                            List<long?> questionids = new List<long?>();

                            foreach (var c in getexamquestionlist)
                            {
                                questionids.Add(c.LPMOEEXQNS_Id);
                            }

                            data.getexamquestionlist = getexamquestionlist.ToArray();

                            data.getquestionoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                          from b in _context.LP_Master_OE_ExamDMO
                                                          from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                          from e in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                          where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                          && a.LPMOEEXQNS_Id == e.LPMOEEXQNS_Id && a.LPMOEEXQNS_ActiveFlg == true
                                                          && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                          && questionids.Contains(e.LPMOEEXQNS_Id))
                                                          select new LP_OnlineStudentExamDTO
                                                          {
                                                              LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                              LPMOEQOA_Id = e.LPMOEEXQNSOPT_Id,
                                                              LPMOEQOA_Option = e.LPMOEEXQNSOPT_Option,
                                                              LPMOEQOA_OptionCode = e.LPMOEEXQNSOPT_OptionCode,
                                                              LPMOEQOA_AnswerFlag = e.LPMOEEXQNSOPT_AnswerFlag,
                                                          }).Distinct().ToArray();

                            data.getquestiondoclist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                       from b in _context.LP_Master_OE_ExamDMO
                                                       from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                       from e in _context.LP_Master_OE_Exam_Questions_FilesDMO
                                                       where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                       && a.LPMOEEXQNS_Id == e.LPMOEEXQNS_Id && a.LPMOEEXQNS_ActiveFlg == true
                                                       && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id && e.LPMOEEXQNSF_ActiveFlag == true
                                                       && questionids.Contains(e.LPMOEEXQNS_Id))
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQF_FileName = e.LPMOEEXQNSF_FileName,
                                                           LPMOEQF_FilePath = e.LPMOEEXQNSF_FilePath
                                                       }).Distinct().ToArray();


                            data.getoptionwisefiles = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                       from b in _context.LP_Master_OE_ExamDMO
                                                       from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                       from e in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                       from f in _context.LP_Master_OE_Exam_Questions_Options_FilesDMO
                                                       where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                       && a.LPMOEEXQNS_Id == e.LPMOEEXQNS_Id && e.LPMOEEXQNSOPT_Id == f.LPMOEEXQNSOPT_Id
                                                       && a.LPMOEEXQNS_ActiveFlg == true && f.LPMOEEXQNSOPTF_ActiveFlag == true && e.LPMOEEXQNSOPT_ActiveFlg == true
                                                       && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                       && questionids.Contains(e.LPMOEEXQNS_Id))
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQOA_Id = e.LPMOEEXQNSOPT_Id,
                                                           LPMOEQOAF_FileName = f.LPMOEEXQNSOPTF_FileName,
                                                           LPMOEQOAF_FilePath = f.LPMOEEXQNSOPTF_FilePath
                                                       }).Distinct().ToArray();

                            data.getquestionmfoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                            from b in _context.LP_Master_OE_ExamDMO
                                                            from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                            from e in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                            from f in _context.LP_Master_OE_Exam_Questions_Options_MFDMO
                                                            where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                            && a.LPMOEEXQNS_Id == e.LPMOEEXQNS_Id && a.LPMOEEXQNS_ActiveFlg == true
                                                            && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                            && e.LPMOEEXQNSOPT_Id == f.LPMOEEXQNSOPT_Id && a.LPMOEEXQNS_MatchTheFollowingFlg == true
                                                            && questionids.Contains(e.LPMOEEXQNS_Id))
                                                            select new LP_OnlineStudentExamDTO
                                                            {
                                                                LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                                LPMOEQOA_Id = e.LPMOEEXQNSOPT_Id,
                                                                LPMOEQOAMF_Id = f.LPMOEEXQNSOPTMF_Id,
                                                                LPMOEQOAMF_MatchtheFollowing = f.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                                                LPMOEQOAMF_AnswerFlag = f.LPMOEEXQNSOPTMF_Answer_Flg,
                                                                LPMOEQOAMF_Order = f.LPMOEEXQNSOPTMF_Order,
                                                            }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();
                        }
                        else
                        {
                            var getexamquestionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                       from b in _context.LP_Master_OE_ExamDMO
                                                       from c in _context.LP_Master_OE_QuestionsDMO
                                                       from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                       where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id && a.LPMOEQ_Id == c.LPMOEQ_Id
                                                       && a.LPMOEEXQNS_ActiveFlg == true && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEQ_Id,
                                                           LPMOEQ_Question = c.LPMOEQ_Question,
                                                           LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQ_SubjectiveFlg = c.LPMOEQ_SubjectiveFlg,
                                                           LPMOEQ_MatchTheFollowingFlg = c.LPMOEQ_MatchTheFollowingFlg,
                                                           LPMOEEXQNS_QnsOrder = a.LPMOEEXQNS_QnsOrder,
                                                           LPMOEEXLVL_Id = a.LPMOEEXLVL_Id,
                                                           LPMOEQ_StructuralFlg = c.LPMOEQ_StructuralFlg
                                                       }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToList();

                            List<long?> questionids = new List<long?>();

                            foreach (var c in getexamquestionlist)
                            {
                                questionids.Add(c.LPMOEQ_Id);
                            }

                            data.getexamquestionlist = getexamquestionlist.ToArray();

                            data.getquestionoptionlist = (from a in _context.LP_Master_OE_QuestionsDMO
                                                          from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                          where (a.LPMOEQ_Id == b.LPMOEQ_Id && a.LPMOEQ_ActiveFlg == true && b.LPMOEQOA_ActiveFlg == true
                                                           && questionids.Contains(b.LPMOEQ_Id) && a.MI_Id == data.MI_Id)
                                                          select new LP_OnlineStudentExamDTO
                                                          {
                                                              LPMOEQ_Id = a.LPMOEQ_Id,
                                                              LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                              LPMOEQOA_Option = b.LPMOEQOA_Option,
                                                              LPMOEQOA_OptionCode = b.LPMOEQOA_OptionCode,
                                                              LPMOEQOA_AnswerFlag = b.LPMOEQOA_AnswerFlag,
                                                          }).Distinct().OrderBy(a => a.LPMOEQ_Id).OrderBy(a => a.LPMOEQOA_Option).ToArray();


                            data.getquestionmfoptionlist = (from a in _context.LP_Master_OE_QuestionsDMO
                                                            from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                            from c in _context.LP_Master_OE_QNS_Options_MFDMO
                                                            where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id
                                                            && a.LPMOEQ_ActiveFlg == true && b.LPMOEQOA_ActiveFlg == true
                                                            && questionids.Contains(a.LPMOEQ_Id) && a.MI_Id == data.MI_Id
                                                            && a.LPMOEQ_MatchTheFollowingFlg == true)
                                                            select new LP_OnlineStudentExamDTO
                                                            {
                                                                LPMOEQ_Id = a.LPMOEQ_Id,
                                                                LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                                LPMOEQOAMF_Id = c.LPMOEQOAMF_Id,
                                                                LPMOEQOAMF_MatchtheFollowing = c.LPMOEQOAMF_MatchtheFollowing,
                                                                LPMOEQOAMF_AnswerFlag = c.LPMOEQOAMF_AnswerFlag,
                                                                LPMOEQOAMF_Order = c.LPMOEQOAMF_Order
                                                            }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();

                            data.getquestiondoclist = (from a in _context.LP_Master_OE_QuestionsDMO
                                                       from b in _context.LP_Master_OE_Questions_FilesDMO
                                                       where (a.LPMOEQ_Id == b.LPMOEQ_Id && a.LPMOEQ_ActiveFlg == true && b.LPMOEQF_ActiveFlag == true
                                                        && questionids.Contains(b.LPMOEQ_Id) && a.MI_Id == data.MI_Id)
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEQ_Id,
                                                           LPMOEQF_FileName = b.LPMOEQF_FileName,
                                                           LPMOEQF_FilePath = b.LPMOEQF_FilePath
                                                       }).Distinct().ToArray();


                            data.getoptionwisefiles = (from a in _context.LP_Master_OE_QuestionsDMO
                                                       from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                       from c in _context.LP_Master_OE_QNS_Options_FilesDMO
                                                       where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id && a.LPMOEQ_ActiveFlg == true
                                                       && b.LPMOEQOA_ActiveFlg == true && c.LPMOEQOAF_ActiveFlag == true
                                                       && questionids.Contains(b.LPMOEQ_Id) && a.MI_Id == data.MI_Id)
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                           LPMOEQOAF_FileName = c.LPMOEQOAF_FileName,
                                                           LPMOEQOAF_FilePath = c.LPMOEQOAF_FilePath
                                                       }).Distinct().ToArray();
                        }
                    }
                }

                data.getstudentlist = (from a in _context.Adm_M_Student
                                       from b in _context.School_Adm_Y_StudentDMO
                                       from c in _context.AdmissionClass
                                       from d in _context.School_M_Section
                                       from e in _context.AcademicYear
                                       where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.ASMAY_Id == e.ASMAY_Id
                                       && b.AMST_Id == data.AMST_Id && b.ASMAY_Id == data.ASMAY_Id)
                                       select new LP_OnlineStudentExamDTO
                                       {
                                           studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                           (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                           (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                                           admno = a.AMST_AdmNo,
                                           ASMCL_ClassName = c.ASMCL_ClassName,
                                           ASMC_SectionName = d.ASMC_SectionName,
                                           yearname = e.ASMAY_Year

                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO Saveanswer(LP_OnlineStudentExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getamstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.UserId).ToList();
                if (getamstid.Count > 0)
                {
                    data.AMST_Id = getamstid.FirstOrDefault().AMST_ID;

                    var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                             from b in _context.Adm_M_Student
                                             from c in _context.AcademicYear
                                             from d in _context.AdmissionClass
                                             from e in _context.School_M_Section
                                             where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                             && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                             && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                             select new LP_OnlineStudentExamDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                                 ASMAY_Id = a.ASMAY_Id,
                                             }).Distinct().ToList();

                    if (getstudentdetails.Count > 0)
                    {
                        data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                        data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                        data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;

                        var checkexamresult = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.LPMOEEX_Id == data.LPMOEEX_Id && a.AMST_Id == data.AMST_Id).ToList();

                        if (checkexamresult.Count > 0)
                        {
                            var checkresult = _context.LP_Students_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                            && a.LPMOEEX_Id == data.LPMOEEX_Id && a.AMST_Id == data.AMST_Id && a.LPSTUEX_Id == checkexamresult.FirstOrDefault().LPSTUEX_Id);

                            checkresult.LPSTUEX_TotalTime = data.LPSTUEX_TotalTime;
                            checkresult.LPSTUEX_UpdatedBy = data.UserId;
                            checkresult.UpdatedDate = indiantime0;
                            _context.Update(checkresult);

                            if (data.LPMOEQ_SubjectiveFlg == false)
                            {

                                var checkquestionresult = _context.LP_Students_Exam_AnswerDMO.Where(a => a.LPSTUEX_Id == checkexamresult.FirstOrDefault().LPSTUEX_Id
                                && a.LPMOEQ_Id == data.saveanswerlsttemp[0].LPMOEQ_Id).ToList();

                                if (checkquestionresult.Count > 0)
                                {
                                    var checkquestionansresult = _context.LP_Students_Exam_AnswerDMO.Single(a => a.LPSTUEX_Id == checkexamresult.FirstOrDefault().LPSTUEX_Id
                                        && a.LPMOEQ_Id == data.saveanswerlsttemp[0].LPMOEQ_Id);

                                    checkquestionansresult.LPMOEQOA_Id = data.saveanswerlsttemp[0].LPMOEQOA_Id;
                                    checkquestionansresult.LPSTUEXANS_CorrectAnsFlg = data.saveanswerlsttemp[0].LPMOEQOA_AnswerFlag;
                                    checkquestionansresult.LPSTUEXANS_UpdatedBy = data.UserId;
                                    checkquestionansresult.UpdatedDate = indiantime0;
                                    _context.Update(checkquestionansresult);

                                }
                                else
                                {
                                    foreach (var c in data.saveanswerlsttemp)
                                    {
                                        LP_Students_Exam_AnswerDMO lP_Students_Exam_AnswerDMO = new LP_Students_Exam_AnswerDMO();

                                        lP_Students_Exam_AnswerDMO.LPSTUEX_Id = checkexamresult.FirstOrDefault().LPSTUEX_Id;
                                        lP_Students_Exam_AnswerDMO.LPMOEQ_Id = c.LPMOEQ_Id;
                                        lP_Students_Exam_AnswerDMO.LPMOEQOA_Id = c.LPMOEQOA_Id;
                                        lP_Students_Exam_AnswerDMO.LPSTUEXANS_CorrectAnsFlg = c.LPMOEQOA_AnswerFlag;
                                        lP_Students_Exam_AnswerDMO.LPSTUEXANS_ActiveFlg = true;
                                        lP_Students_Exam_AnswerDMO.LPSTUEXANS_CreatedBy = data.UserId;
                                        lP_Students_Exam_AnswerDMO.LPSTUEXANS_UpdatedBy = data.UserId;
                                        lP_Students_Exam_AnswerDMO.CreatedDate = indiantime0;
                                        lP_Students_Exam_AnswerDMO.UpdatedDate = indiantime0;
                                        _context.Add(lP_Students_Exam_AnswerDMO);
                                    }
                                }
                            }
                            else
                            {
                                var checkquestionsubjectiveresult = _context.LP_Students_Exam_SubjectiveAnswerDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id
                                && a.LPSTUEX_Id == checkexamresult.FirstOrDefault().LPSTUEX_Id).ToList();

                                if (checkquestionsubjectiveresult.Count > 0)
                                {
                                    var quessubjectiveresult = _context.LP_Students_Exam_SubjectiveAnswerDMO.Single(a => a.LPMOEQ_Id == data.LPMOEQ_Id
                                    && a.LPSTUEX_Id == checkexamresult.FirstOrDefault().LPSTUEX_Id);

                                    quessubjectiveresult.LPSTUEXSANS_Answer = data.LPSTUEXSANS_Answer;
                                    quessubjectiveresult.LPSTUEXANS_UpdatedDate = indiantime0;
                                    quessubjectiveresult.LPSTUEXANS_UpdatedBy = data.UserId;
                                    _context.Update(quessubjectiveresult);

                                }
                                else
                                {
                                    LP_Students_Exam_SubjectiveAnswerDMO lP_Students_Exam_SubjectiveAnswerDMO = new LP_Students_Exam_SubjectiveAnswerDMO();
                                    lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEX_Id = checkexamresult.FirstOrDefault().LPSTUEX_Id;
                                    lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXSANS_Answer = data.LPSTUEXSANS_Answer;
                                    lP_Students_Exam_SubjectiveAnswerDMO.LPMOEQ_Id = data.LPMOEQ_Id;
                                    lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXANS_ActiveFlg = true;
                                    lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXANS_CreatedDate = indiantime0;
                                    lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXANS_CreatedBy = data.UserId;
                                    lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXANS_UpdatedDate = indiantime0;
                                    lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXANS_UpdatedBy = data.UserId;
                                    _context.Add(lP_Students_Exam_SubjectiveAnswerDMO);
                                }
                            }
                            var i = _context.SaveChanges();
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
                            LP_Students_ExamDMO lP_Students_ExamDMO = new LP_Students_ExamDMO();

                            lP_Students_ExamDMO.MI_Id = data.MI_Id;
                            lP_Students_ExamDMO.AMST_Id = data.AMST_Id;
                            lP_Students_ExamDMO.ASMAY_Id = data.ASMAY_Id;
                            lP_Students_ExamDMO.LPMOEEX_Id = data.LPMOEEX_Id;
                            lP_Students_ExamDMO.LPSTUEX_TotalTime = data.LPSTUEX_TotalTime;
                            lP_Students_ExamDMO.LPSTUEX_Date = indiantime0;
                            lP_Students_ExamDMO.LPSTUEX_ActiveFlg = true;
                            lP_Students_ExamDMO.LPSTUEX_CreatedBy = data.UserId;
                            lP_Students_ExamDMO.LPSTUEX_UpdatedBy = data.UserId;
                            lP_Students_ExamDMO.CreatedDate = indiantime0;
                            lP_Students_ExamDMO.UpdatedDate = indiantime0;
                            lP_Students_ExamDMO.LPSTUEX_StaffOrStudentUploadFlag = "Student";

                            _context.Add(lP_Students_ExamDMO);

                            if (data.LPMOEQ_SubjectiveFlg == false)
                            {
                                foreach (var c in data.saveanswerlsttemp)
                                {
                                    LP_Students_Exam_AnswerDMO lP_Students_Exam_AnswerDMO = new LP_Students_Exam_AnswerDMO();

                                    lP_Students_Exam_AnswerDMO.LPSTUEX_Id = lP_Students_ExamDMO.LPSTUEX_Id;
                                    lP_Students_Exam_AnswerDMO.LPMOEQ_Id = c.LPMOEQ_Id;
                                    lP_Students_Exam_AnswerDMO.LPMOEQOA_Id = c.LPMOEQOA_Id;
                                    lP_Students_Exam_AnswerDMO.LPSTUEXANS_CorrectAnsFlg = c.LPMOEQOA_AnswerFlag;
                                    lP_Students_Exam_AnswerDMO.LPSTUEXANS_ActiveFlg = true;
                                    lP_Students_Exam_AnswerDMO.LPSTUEXANS_CreatedBy = data.UserId;
                                    lP_Students_Exam_AnswerDMO.LPSTUEXANS_UpdatedBy = data.UserId;
                                    lP_Students_Exam_AnswerDMO.CreatedDate = indiantime0;
                                    lP_Students_Exam_AnswerDMO.UpdatedDate = indiantime0;
                                    _context.Add(lP_Students_Exam_AnswerDMO);
                                }
                            }

                            else
                            {
                                LP_Students_Exam_SubjectiveAnswerDMO lP_Students_Exam_SubjectiveAnswerDMO = new LP_Students_Exam_SubjectiveAnswerDMO();
                                lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEX_Id = lP_Students_ExamDMO.LPSTUEX_Id;
                                lP_Students_Exam_SubjectiveAnswerDMO.LPMOEQ_Id = data.LPMOEQ_Id;
                                lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXSANS_Answer = data.LPSTUEXSANS_Answer;
                                lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXANS_ActiveFlg = true;
                                lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXANS_CreatedDate = indiantime0;
                                lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXANS_CreatedBy = data.UserId;
                                lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXANS_UpdatedDate = indiantime0;
                                lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXANS_UpdatedBy = data.UserId;
                                _context.Add(lP_Students_Exam_SubjectiveAnswerDMO);
                            }
                            var i = _context.SaveChanges();
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

                    List<LP_OnlineStudentExamDTO> objectivequestions = new List<LP_OnlineStudentExamDTO>();
                    List<LP_OnlineStudentExamDTO> subjectivequestions = new List<LP_OnlineStudentExamDTO>();
                    List<LP_OnlineStudentExamDTO> allquestion = new List<LP_OnlineStudentExamDTO>();

                    //data.getsavedanswer = (from a in _context.LP_Master_OE_QuestionsDMO
                    //                       from c in _context.LP_Master_OE_QNS_OptionsDMO
                    //                       from d in _context.LP_Students_ExamDMO
                    //                       from e in _context.LP_Students_Exam_AnswerDMO
                    //                       where (a.LPMOEQ_Id == c.LPMOEQ_Id && a.MI_Id == data.MI_Id && e.LPMOEQOA_Id == c.LPMOEQOA_Id
                    //                       && d.LPSTUEX_Id == e.LPSTUEX_Id && d.AMST_Id == data.AMST_Id && a.ASMCL_Id == data.ASMCL_Id
                    //                       && d.LPMOEEX_Id == data.LPMOEEX_Id && d.ASMAY_Id == data.ASMAY_Id)
                    //                       select new LP_OnlineStudentExamDTO
                    //                       {
                    //                           LPMOEQ_Id = e.LPMOEQ_Id,
                    //                           LPMOEQOA_Id = e.LPMOEQOA_Id,
                    //                           LPMOEQ_Question = a.LPMOEQ_Question,
                    //                           LPMOEQOA_OptionCode = c.LPMOEQOA_OptionCode,
                    //                           LPMOEQOA_Option = c.LPMOEQOA_Option,
                    //                           LPMOEQOA_AnswerFlag = c.LPMOEQOA_AnswerFlag,
                    //                           LPMOEQ_SubjectiveFlg = a.LPMOEQ_SubjectiveFlg
                    //                       }).Distinct().ToArray();

                    objectivequestions = (from a in _context.LP_Master_OE_QuestionsDMO
                                          from c in _context.LP_Master_OE_QNS_OptionsDMO
                                          from d in _context.LP_Students_ExamDMO
                                          from e in _context.LP_Students_Exam_AnswerDMO
                                          where (a.LPMOEQ_Id == c.LPMOEQ_Id && a.MI_Id == data.MI_Id && e.LPMOEQOA_Id == c.LPMOEQOA_Id
                                          && d.LPSTUEX_Id == e.LPSTUEX_Id && d.AMST_Id == data.AMST_Id && a.ASMCL_Id == data.ASMCL_Id
                                          && d.LPMOEEX_Id == data.LPMOEEX_Id && d.ASMAY_Id == data.ASMAY_Id && a.LPMOEQ_SubjectiveFlg == false)
                                          select new LP_OnlineStudentExamDTO
                                          {
                                              LPMOEQ_Id = e.LPMOEQ_Id,
                                              LPMOEQOA_Id = e.LPMOEQOA_Id,
                                              LPMOEQ_Question = a.LPMOEQ_Question,
                                              LPMOEQOA_OptionCode = c.LPMOEQOA_OptionCode,
                                              LPMOEQOA_Option = c.LPMOEQOA_Option,
                                              LPMOEQOA_AnswerFlag = c.LPMOEQOA_AnswerFlag,
                                              LPMOEQ_SubjectiveFlg = a.LPMOEQ_SubjectiveFlg,
                                              LPMOEQ_MatchTheFollowingFlg = a.LPMOEQ_MatchTheFollowingFlg,
                                          }).Distinct().ToList();

                    subjectivequestions = (from a in _context.LP_Master_OE_QuestionsDMO
                                           from d in _context.LP_Students_ExamDMO
                                           from e in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                           where (a.LPMOEQ_Id == e.LPMOEQ_Id && a.MI_Id == data.MI_Id
                                           && d.LPSTUEX_Id == e.LPSTUEX_Id && d.AMST_Id == data.AMST_Id && a.ASMCL_Id == data.ASMCL_Id
                                           && d.LPMOEEX_Id == data.LPMOEEX_Id && d.ASMAY_Id == data.ASMAY_Id && a.LPMOEQ_SubjectiveFlg == true)
                                           select new LP_OnlineStudentExamDTO
                                           {
                                               LPMOEQ_Id = e.LPMOEQ_Id,
                                               LPMOEQ_Question = a.LPMOEQ_Question,
                                               LPMOEQ_SubjectiveFlg = a.LPMOEQ_SubjectiveFlg,
                                               LPMOEQ_MatchTheFollowingFlg = a.LPMOEQ_MatchTheFollowingFlg,
                                               LPSTUEXSANS_Answer = e.LPSTUEXSANS_Answer
                                           }).Distinct().ToList();


                    foreach (var c in objectivequestions)
                    {
                        allquestion.Add(c);
                    }

                    foreach (var c in subjectivequestions)
                    {
                        allquestion.Add(c);
                    }

                    data.getsavedanswer = allquestion.ToArray();

                }
            }
            catch (Exception ex)
            {
                data.message = "Failed";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO savedanswers(LP_OnlineStudentExamDTO data)
        {
            try
            {
                var getamstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.UserId).ToList();
                if (getamstid.Count > 0)
                {
                    data.AMST_Id = getamstid.FirstOrDefault().AMST_ID;

                    var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                             from b in _context.Adm_M_Student
                                             from c in _context.AcademicYear
                                             from d in _context.AdmissionClass
                                             from e in _context.School_M_Section
                                             where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                             && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                             && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                             select new LP_OnlineStudentExamDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                                 ASMAY_Id = a.ASMAY_Id,
                                             }).Distinct().ToList();

                    if (getstudentdetails.Count > 0)
                    {
                        data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                        data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                        data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;

                        data.getsavedanswer = (from a in _context.LP_Master_OE_QuestionsDMO
                                               from c in _context.LP_Master_OE_QNS_OptionsDMO
                                               from d in _context.LP_Students_ExamDMO
                                               from e in _context.LP_Students_Exam_AnswerDMO
                                               where (a.LPMOEQ_Id == c.LPMOEQ_Id && a.MI_Id == data.MI_Id && e.LPMOEQOA_Id == c.LPMOEQOA_Id
                                               && d.LPSTUEX_Id == e.LPSTUEX_Id && d.AMST_Id == data.AMST_Id && a.ASMCL_Id == data.ASMCL_Id
                                               && d.LPMOEEX_Id == data.LPMOEEX_Id && d.ASMAY_Id == data.ASMAY_Id)
                                               select new LP_OnlineStudentExamDTO
                                               {
                                                   LPMOEQ_Id = e.LPMOEQ_Id,
                                                   LPMOEQOA_Id = e.LPMOEQOA_Id,
                                                   LPMOEQ_Question = a.LPMOEQ_Question,
                                                   LPMOEQOA_OptionCode = c.LPMOEQOA_OptionCode,
                                                   LPMOEQOA_Option = c.LPMOEQOA_Option,
                                                   LPMOEQOA_AnswerFlag = c.LPMOEQOA_AnswerFlag
                                               }).Distinct().ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO submitexam(LP_OnlineStudentExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                data.AMST_Id = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.UserId).FirstOrDefault().AMST_ID;

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         from c in _context.AcademicYear
                                         from d in _context.AdmissionClass
                                         from e in _context.School_M_Section
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                         && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                         select new LP_OnlineStudentExamDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id,
                                         }).Distinct().ToList();

                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;

                var examdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id
                && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToList();

                var LPMOEEX_NotLinkedToQnsBankFlg = examdetails.FirstOrDefault().LPMOEEX_NotLinkedToQnsBankFlg;
                long LPSTUEX_Id = 0;

                if (data.LP_OnlineFinalDetails != null && data.LP_OnlineFinalDetails.Length > 0)
                {
                    var checkexamresult = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                     && a.LPMOEEX_Id == data.LPMOEEX_Id && a.AMST_Id == data.AMST_Id).ToList();

                    if (checkexamresult.Count > 0)
                    {
                        LPSTUEX_Id = checkexamresult.FirstOrDefault().LPSTUEX_Id;
                        var checkresult = _context.LP_Students_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.LPMOEEX_Id == data.LPMOEEX_Id && a.AMST_Id == data.AMST_Id && a.LPSTUEX_Id == LPSTUEX_Id);
                        checkresult.LPSTUEX_TotalTime = data.LPSTUEX_TotalTime;
                        checkresult.LPSTUEX_UpdatedBy = data.UserId;
                        checkresult.UpdatedDate = indiantime0;
                        _context.Update(checkresult);
                    }
                    else
                    {
                        LP_Students_ExamDMO lP_Students_ExamDMO = new LP_Students_ExamDMO
                        {
                            MI_Id = data.MI_Id,
                            AMST_Id = data.AMST_Id,
                            ASMAY_Id = data.ASMAY_Id,
                            LPMOEEX_Id = data.LPMOEEX_Id,
                            LPSTUEX_TotalTime = data.LPSTUEX_TotalTime,
                            LPSTUEX_Date = indiantime0,
                            LPSTUEX_ActiveFlg = true,
                            LPSTUEX_CreatedBy = data.UserId,
                            LPSTUEX_UpdatedBy = data.UserId,
                            CreatedDate = indiantime0,
                            UpdatedDate = indiantime0,
                            LPSTUEX_StaffOrStudentUploadFlag = "Student"
                        };
                        _context.Add(lP_Students_ExamDMO);
                        LPSTUEX_Id = lP_Students_ExamDMO.LPSTUEX_Id;
                    }
                    if (data.LP_OnlineFinalDetails != null && data.LP_OnlineFinalDetails.Length > 0)
                    {
                        foreach (var c in data.LP_OnlineFinalDetails)
                        {
                            //Choose The Correct Answer Saving Or Update
                            if (c.LPMOEQ_SubjectiveFlg == false && c.LPMOEQ_MatchTheFollowingFlg == false)
                            {
                                var QuizeQuastions = c.QuizeQuastions > 0 ? c.QuizeQuastions : null;

                                var checkquestionresult = _context.LP_Students_Exam_AnswerDMO.Where(a => a.LPSTUEX_Id == LPSTUEX_Id).ToList();

                                if (LPMOEEX_NotLinkedToQnsBankFlg == true)
                                {
                                    checkquestionresult = checkquestionresult.Where(a => a.LPMOEEXQNS_Id == c.LPMOEQ_Id).ToList();
                                }
                                else
                                {
                                    checkquestionresult = checkquestionresult.Where(a => a.LPMOEQ_Id == c.LPMOEQ_Id).ToList();
                                }

                                if (checkquestionresult.Count > 0)
                                {
                                    var checkquestionansresult = _context.LP_Students_Exam_AnswerDMO.Single(a => a.LPSTUEX_Id == LPSTUEX_Id
                                    && a.LPSTUEXANS_Id == checkquestionresult.FirstOrDefault().LPSTUEXANS_Id);

                                    checkquestionansresult.LPMOEEXQNSOPT_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? QuizeQuastions : null;
                                    checkquestionansresult.LPMOEQOA_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? null : QuizeQuastions;
                                    checkquestionansresult.LPSTUEXANS_CorrectAnsFlg = c.LPMOEQOA_AnswerFlag;
                                    checkquestionansresult.LPSTUEXANS_AttemptFlag = c.LPSTUEXANS_AttemptFlag;
                                    checkquestionansresult.LPSTUEXANS_UpdatedBy = data.UserId;
                                    checkquestionansresult.UpdatedDate = indiantime0;
                                    _context.Update(checkquestionansresult);
                                }
                                else
                                {
                                    LP_Students_Exam_AnswerDMO lP_Students_Exam_AnswerDMO = new LP_Students_Exam_AnswerDMO
                                    {
                                        LPSTUEX_Id = LPSTUEX_Id,
                                        LPMOEQ_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? null : c.LPMOEQ_Id,
                                        LPMOEQOA_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? null : QuizeQuastions,
                                        LPMOEEXQNS_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? c.LPMOEQ_Id : null,
                                        LPMOEEXQNSOPT_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? QuizeQuastions : null,
                                        LPSTUEXANS_CorrectAnsFlg = c.LPMOEQOA_AnswerFlag,
                                        LPSTUEXANS_AttemptFlag = c.LPSTUEXANS_AttemptFlag,
                                        LPSTUEXANS_ActiveFlg = true,
                                        LPSTUEXANS_CreatedBy = data.UserId,
                                        LPSTUEXANS_UpdatedBy = data.UserId,
                                        CreatedDate = indiantime0,
                                        UpdatedDate = indiantime0
                                    };
                                    _context.Add(lP_Students_Exam_AnswerDMO);
                                }
                            }

                            // Subject Type Saving Or Update
                            else if (c.LPMOEQ_SubjectiveFlg == true && c.LPMOEQ_MatchTheFollowingFlg == false)
                            {
                                var checkquestionsubjectiveresult = _context.LP_Students_Exam_SubjectiveAnswerDMO.Where(a => a.LPSTUEX_Id == LPSTUEX_Id).ToList();

                                if (LPMOEEX_NotLinkedToQnsBankFlg == true)
                                {
                                    checkquestionsubjectiveresult = checkquestionsubjectiveresult.Where(a => a.LPMOEEXQNS_Id == c.LPMOEQ_Id
                                     && a.LPSTUEX_Id == LPSTUEX_Id).ToList();
                                }
                                else
                                {
                                    checkquestionsubjectiveresult = checkquestionsubjectiveresult.Where(a => a.LPMOEQ_Id == c.LPMOEQ_Id
                                    && a.LPSTUEX_Id == LPSTUEX_Id).ToList();
                                }

                                if (checkquestionsubjectiveresult.Count > 0)
                                {
                                    var quessubjectiveresult = _context.LP_Students_Exam_SubjectiveAnswerDMO.Single(a => a.LPSTUEXSANS_Id ==
                                    checkquestionsubjectiveresult.FirstOrDefault().LPSTUEXSANS_Id && a.LPSTUEX_Id == LPSTUEX_Id);
                                    quessubjectiveresult.LPSTUEXSANS_Answer = c.answer;
                                    quessubjectiveresult.LPSTUEXANS_AttemptFlag = c.LPSTUEXANS_AttemptFlag;
                                    quessubjectiveresult.LPSTUEXANS_UpdatedDate = indiantime0;
                                    quessubjectiveresult.LPSTUEXANS_UpdatedBy = data.UserId;
                                    quessubjectiveresult.LPSTUEXSANS_FileName = c.LPSTUEXSANS_FileName;
                                    quessubjectiveresult.LPSTUEXSANS_FilePath = c.LPSTUEXSANS_FilePath;
                                    _context.Update(quessubjectiveresult);
                                }
                                else
                                {
                                    LP_Students_Exam_SubjectiveAnswerDMO lP_Students_Exam_SubjectiveAnswerDMO = new LP_Students_Exam_SubjectiveAnswerDMO
                                    {
                                        LPSTUEX_Id = LPSTUEX_Id,
                                        LPSTUEXSANS_Answer = c.answer,
                                        LPSTUEXANS_AttemptFlag = c.LPSTUEXANS_AttemptFlag,
                                        LPMOEQ_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? null : c.LPMOEQ_Id,
                                        LPMOEEXQNS_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? c.LPMOEQ_Id : null,
                                        LPSTUEXANS_ActiveFlg = true,
                                        LPSTUEXANS_CreatedDate = indiantime0,
                                        LPSTUEXANS_CreatedBy = data.UserId,
                                        LPSTUEXANS_UpdatedDate = indiantime0,
                                        LPSTUEXANS_UpdatedBy = data.UserId,
                                        LPSTUEXSANS_FileName = c.LPSTUEXSANS_FileName,
                                        LPSTUEXSANS_FilePath = c.LPSTUEXSANS_FilePath,
                                    };
                                    _context.Add(lP_Students_Exam_SubjectiveAnswerDMO);

                                    if (c.Temp_Ques_Subjective_Files != null && c.Temp_Ques_Subjective_Files.Length > 0)
                                    {
                                        foreach (var c_files in c.Temp_Ques_Subjective_Files)
                                        {
                                            LP_Students_Exam_SubjectiveAnswer_FilesDMO lP_Students_Exam_SubjectiveAnswer_FilesDMO = new LP_Students_Exam_SubjectiveAnswer_FilesDMO
                                            {
                                                LPSTUEXSANS_Id = lP_Students_Exam_SubjectiveAnswerDMO.LPSTUEXSANS_Id,
                                                LPSTUEXSANSFL_FileName = c_files.LPSTUEXSANSFL_FileName,
                                                LPSTUEXSANSFNFL_FilePath = c_files.LPSTUEXSANSFNFL_FilePath,
                                                LPSTUEXSANSFL_ActiveFlg = true,
                                                LPSTUEXSANSFL_CreatedBy = data.UserId,
                                                LPSTUEXSANSFL_CreatedDate = indiantime0,
                                                LPSTUEXSANSFL_UpdatedBy = data.UserId,
                                                LPSTUEXSANSFL_UpdatedDate = indiantime0,
                                            };
                                            _context.Add(lP_Students_Exam_SubjectiveAnswer_FilesDMO);
                                        }
                                    }
                                }
                            }

                            // Match The Following Type Saving Or Update
                            else if (c.LPMOEQ_SubjectiveFlg == false && c.LPMOEQ_MatchTheFollowingFlg == true)
                            {
                                if (c.LP_OnlineFinalDetails_MF != null && c.LP_OnlineFinalDetails_MF.Length > 0)
                                {
                                    foreach (var mf in c.LP_OnlineFinalDetails_MF)
                                    {
                                        var QuizeQuastions_MF = mf.QuizeQuastions > 0 ? mf.QuizeQuastions : null;

                                        var checkquestionresult_Mf = _context.LP_Students_Exam_AnswerDMO.Where(a => a.LPSTUEX_Id == LPSTUEX_Id
                                        && a.LPMOEQ_Id == c.LPMOEQ_Id && a.LPMOEQOA_Id == mf.LPMOEQOA_Id).ToList();

                                        if (LPMOEEX_NotLinkedToQnsBankFlg == true)
                                        {
                                            checkquestionresult_Mf = checkquestionresult_Mf.Where(a => a.LPMOEQ_Id == c.LPMOEQ_Id
                                            && a.LPMOEQOA_Id == mf.LPMOEQOA_Id).ToList();
                                        }
                                        else
                                        {
                                            checkquestionresult_Mf = checkquestionresult_Mf.Where(a => a.LPMOEEXQNS_Id == c.LPMOEQ_Id
                                            && a.LPMOEEXQNSOPT_Id == mf.LPMOEQOA_Id).ToList();
                                        }

                                        if (checkquestionresult_Mf.Count > 0)
                                        {
                                            var checkquestionansresult_Mf = _context.LP_Students_Exam_AnswerDMO.Single(a => a.LPSTUEX_Id == LPSTUEX_Id
                                            && a.LPSTUEXANS_Id == checkquestionresult_Mf.FirstOrDefault().LPSTUEXANS_Id);

                                            checkquestionansresult_Mf.LPMOEQOA_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? null : mf.LPMOEQOA_Id;
                                            checkquestionansresult_Mf.LPMOEQOAMF_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? null : QuizeQuastions_MF;

                                            checkquestionansresult_Mf.LPSTUEXANS_CorrectAnsFlg = mf.LPMOEQOA_AnswerFlag;
                                            checkquestionansresult_Mf.LPSTUEXANS_AttemptFlag = mf.LPSTUEXANS_AttemptFlag;
                                            checkquestionansresult_Mf.LPSTUEXANS_UpdatedBy = data.UserId;
                                            checkquestionansresult_Mf.UpdatedDate = indiantime0;

                                            checkquestionansresult_Mf.LPMOEEXQNSOPT_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? mf.LPMOEQOA_Id : null;
                                            checkquestionansresult_Mf.LPMOEEXQNSOPTMF_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? QuizeQuastions_MF : null;
                                            _context.Update(checkquestionansresult_Mf);
                                        }
                                        else
                                        {
                                            LP_Students_Exam_AnswerDMO lP_Students_Exam_AnswerDMO_mf = new LP_Students_Exam_AnswerDMO
                                            {
                                                LPSTUEX_Id = LPSTUEX_Id,
                                                LPMOEQ_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? null : mf.LPMOEQ_Id,
                                                LPMOEQOA_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? null : mf.LPMOEQOA_Id,
                                                LPMOEQOAMF_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? null : QuizeQuastions_MF,
                                                LPMOEEXQNS_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? mf.LPMOEQ_Id : null,
                                                LPMOEEXQNSOPT_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? mf.LPMOEQOA_Id : null,
                                                LPMOEEXQNSOPTMF_Id = LPMOEEX_NotLinkedToQnsBankFlg == true ? QuizeQuastions_MF : null,
                                                LPSTUEXANS_CorrectAnsFlg = mf.LPMOEQOA_AnswerFlag,
                                                LPSTUEXANS_AttemptFlag = mf.LPSTUEXANS_AttemptFlag,
                                                LPSTUEXANS_ActiveFlg = true,
                                                LPSTUEXANS_CreatedBy = data.UserId,
                                                LPSTUEXANS_UpdatedBy = data.UserId,
                                                CreatedDate = indiantime0,
                                                UpdatedDate = indiantime0
                                            };
                                            _context.Add(lP_Students_Exam_AnswerDMO_mf);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Saved";
                    }
                    else
                    {
                        data.message = "Failed";
                    }

                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "LP_Online_Exam_Modify";
                    cmd.CommandText = "LP_Online_Exam_MarksCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@LPMOEEX_Id", SqlDbType.VarChar) { Value = data.LPMOEEX_Id });
                    cmd.Parameters.Add(new SqlParameter("@examtime", SqlDbType.VarChar) { Value = data.LPSTUEX_TotalTime });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO GetViewMarks(LP_OnlineStudentExamDTO data)
        {
            try
            {
                data.AMST_Id = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.UserId).FirstOrDefault().AMST_ID;

                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         from c in _context.AcademicYear
                                         from d in _context.AdmissionClass
                                         from e in _context.School_M_Section
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                         && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                         select new LP_OnlineStudentExamDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id,
                                         }).Distinct().ToList();

                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;

                data.getmarksdetails = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                && a.LPMOEEX_Id == data.LPMOEEX_Id).Distinct().ToArray();

                var getexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                data.getexamdetails = getexamdetails.ToArray();

                var LPMOEEX_NotLinkedToQnsBankFlg = getexamdetails.FirstOrDefault().LPMOEEX_NotLinkedToQnsBankFlg;

                if (getexamdetails.FirstOrDefault().LPMOEEX_UploadExamPaperFlg == false)
                {
                    data.getexamleveldetails = _context.LP_Master_OE_Exam_LevelsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id).OrderBy(a => a.LPMOEEXLVL_LevelOrder).ToArray();

                    if (getexamdetails.FirstOrDefault().LPMOEEX_NotLinkedToQnsBankFlg == true)
                    {
                        // Get Question List 
                        var getexamquestionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                   from b in _context.LP_Master_OE_ExamDMO
                                                   from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                   where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                   && a.LPMOEEXQNS_ActiveFlg == true && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                   select new LP_OnlineStudentExamDTO
                                                   {
                                                       LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                       LPMOEQ_Question = a.LPMOEEXQNS_Question,
                                                       LPMOEQ_Answer = a.LPMOEEXQNS_Answer,
                                                       LPMOEQ_Marks = a.LPMOEEXQNS_Marks,
                                                       LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                       LPMOEQ_SubjectiveFlg = a.LPMOEEXQNS_SubjectiveFlg,
                                                       LPMOEQ_MatchTheFollowingFlg = a.LPMOEEXQNS_MatchTheFollowingFlg,
                                                       LPMOEEXQNS_QnsOrder = a.LPMOEEXQNS_QnsOrder,
                                                       LPMOEEXLVL_Id = a.LPMOEEXLVL_Id,
                                                       LPMOEQ_StructuralFlg = a.LPMOEEXQNS_QuestionType
                                                   }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToList();

                        List<long?> questionids = new List<long?>();

                        foreach (var c in getexamquestionlist)
                        {
                            questionids.Add(c.LPMOEQ_Id);
                        }

                        data.getexamquestionlist = getexamquestionlist.ToArray();

                        // Get Question Wise Option List 
                        data.getquestionoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                      from b in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                      from c in _context.LP_Master_OE_Exam_LevelsDMO
                                                      from d in _context.LP_Master_OE_ExamDMO
                                                      where (a.LPMOEEXQNS_Id == b.LPMOEEXQNS_Id && a.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                      && c.LPMOEEX_Id == d.LPMOEEX_Id && a.LPMOEEXQNS_ActiveFlg == true && b.LPMOEEXQNSOPT_ActiveFlg == true
                                                      && questionids.Contains(b.LPMOEEXQNS_Id) && d.MI_Id == data.MI_Id && d.LPMOEEX_Id == data.LPMOEEX_Id)
                                                      select new LP_OnlineStudentExamDTO
                                                      {
                                                          LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                          LPMOEQOA_Id = b.LPMOEEXQNSOPT_Id,
                                                          LPMOEQOA_Option = b.LPMOEEXQNSOPT_Option,
                                                          LPMOEQOA_OptionCode = b.LPMOEEXQNSOPT_OptionCode,
                                                          LPMOEQOA_AnswerFlag = b.LPMOEEXQNSOPT_AnswerFlag,
                                                          LPMOEQOA_Marks = b.LPMOEEXQNSOPT_Marks,
                                                      }).Distinct().OrderBy(a => a.LPMOEQ_Id).OrderBy(a => a.LPMOEQOA_Option).ToArray();

                        // Get Question Wise Option MF List 
                        data.getquestionmfoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                        from b in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                        from c in _context.LP_Master_OE_Exam_Questions_Options_MFDMO
                                                        from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                        from e in _context.LP_Master_OE_ExamDMO
                                                        where (a.LPMOEEXQNS_Id == b.LPMOEEXQNS_Id && b.LPMOEEXQNSOPT_Id == c.LPMOEEXQNSOPT_Id
                                                        && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id && d.LPMOEEX_Id == e.LPMOEEX_Id
                                                        && a.LPMOEEXQNS_ActiveFlg == true && b.LPMOEEXQNSOPT_ActiveFlg == true
                                                        && questionids.Contains(a.LPMOEEXQNS_Id) && e.MI_Id == data.MI_Id && e.LPMOEEX_Id == data.LPMOEEX_Id
                                                        && a.LPMOEEXQNS_MatchTheFollowingFlg == true)
                                                        select new LP_OnlineStudentExamDTO
                                                        {
                                                            LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                            LPMOEQOA_Id = b.LPMOEEXQNSOPT_Id,
                                                            LPMOEQOAMF_Id = c.LPMOEEXQNSOPTMF_Id,
                                                            LPMOEQOAMF_MatchtheFollowing = c.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                                            LPMOEQOAMF_AnswerFlag = c.LPMOEEXQNSOPTMF_Answer_Flg,
                                                            LPMOEQOAMF_Order = c.LPMOEEXQNSOPTMF_Order
                                                        }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();

                        // Get Subjective Uploaded File List By Student
                        data.getquestionsubjective_fileslist = (from a in _context.LP_Students_Exam_SubjectiveAnswer_FilesDMO
                                                                from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                from c in _context.LP_Students_ExamDMO
                                                                where (a.LPSTUEXSANS_Id == b.LPSTUEXSANS_Id && b.LPSTUEX_Id == c.LPSTUEX_Id
                                                                && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id
                                                                && c.LPMOEEX_Id == data.LPMOEEX_Id && questionids.Contains(b.LPMOEEXQNS_Id))
                                                                select new LP_OnlineStudentExamDTO
                                                                {
                                                                    LPMOEQ_Id = b.LPMOEEXQNS_Id,
                                                                    FileName = a.LPSTUEXSANSFL_FileName,
                                                                    FilePath = a.LPSTUEXSANSFNFL_FilePath,
                                                                    LPSTUEXSANS_Id = a.LPSTUEXSANS_Id,
                                                                }).Distinct().ToArray();

                        data.getquestionsubjective_staff_fileslist = (from a in _context.LP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO
                                                                      from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                      from c in _context.LP_Students_ExamDMO
                                                                      where (a.LPSTUEXSANS_Id == b.LPSTUEXSANS_Id && b.LPSTUEX_Id == c.LPSTUEX_Id
                                                                      && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id
                                                                      && c.LPMOEEX_Id == data.LPMOEEX_Id && questionids.Contains(b.LPMOEEXQNS_Id))
                                                                      select new LP_OnlineStudentExamDTO
                                                                      {
                                                                          LPMOEQ_Id = b.LPMOEEXQNS_Id,
                                                                          FileName = a.LPSTUEXSANSSFL_FileName,
                                                                          FilePath = a.LPSTUEXSANSSFL_FilePath,
                                                                          LPSTUEXSANS_Id = a.LPSTUEXSANS_Id,
                                                                      }).Distinct().ToArray();


                        //Get Student Wise Question Choose The Correct Answer Marks
                        data.get_examwise_ques_option_marks = (from a in _context.LP_Students_ExamDMO
                                                               from b in _context.LP_Students_Exam_AnswerDMO
                                                               from c in _context.LP_Master_OE_ExamDMO
                                                               where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.LPMOEEX_Id == c.LPMOEEX_Id && a.ASMAY_Id == data.ASMAY_Id
                                                               && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && c.LPMOEEX_Id == data.LPMOEEX_Id
                                                               && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id
                                                               && c.ISMS_Id == data.ISMS_Id && questionids.Contains(b.LPMOEEXQNS_Id))
                                                               select new LP_OnlineStudentExamDTO
                                                               {
                                                                   LPMOEQ_Id = b.LPMOEEXQNS_Id,
                                                                   LPSTUEXANS_Id = b.LPSTUEXANS_Id,
                                                                   LPMOEQOA_Id = b.LPMOEEXQNSOPT_Id,
                                                                   LPMOEQOAMF_Id = b.LPMOEEXQNSOPTMF_Id,
                                                                   LPSTUEXANS_AttemptFlag = b.LPSTUEXANS_AttemptFlag,
                                                                   LPSTUEXANS_Marks = b.LPSTUEXANS_Marks,
                                                                   LPSTUEXANS_CorrectAnsFlg = b.LPSTUEXANS_CorrectAnsFlg,
                                                               }).Distinct().ToArray();

                        //Get Student Wise Question Subjective Marks
                        data.get_examwise_ques_subjective_marks = (from a in _context.LP_Students_ExamDMO
                                                                   from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                   from c in _context.LP_Master_OE_ExamDMO
                                                                   where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.LPMOEEX_Id == c.LPMOEEX_Id && a.ASMAY_Id == data.ASMAY_Id
                                                                   && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && c.LPMOEEX_Id == data.LPMOEEX_Id
                                                                   && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id
                                                                   && c.ISMS_Id == data.ISMS_Id && questionids.Contains(b.LPMOEEXQNS_Id))
                                                                   select new LP_OnlineStudentExamDTO
                                                                   {
                                                                       LPMOEQ_Id = b.LPMOEEXQNS_Id,
                                                                       LPSTUEXSANS_Id = b.LPSTUEXSANS_Id,
                                                                       LPSTUEXANS_AttemptFlag = b.LPSTUEXANS_AttemptFlag,
                                                                       LPSTUEXSANS_Answer = b.LPSTUEXSANS_Answer,
                                                                       LPSTUEXANS_Marks = b.LPSTUEXSANS_Marks
                                                                   }).Distinct().ToArray();
                    }
                    else
                    {
                        // Get Question List 
                        var getexamquestionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                   from b in _context.LP_Master_OE_ExamDMO
                                                   from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                   from e in _context.LP_Master_OE_QuestionsDMO
                                                   where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id && a.LPMOEQ_Id == e.LPMOEQ_Id
                                                   && a.LPMOEEXQNS_ActiveFlg == true && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                   select new LP_OnlineStudentExamDTO
                                                   {
                                                       LPMOEQ_Id = a.LPMOEQ_Id,
                                                       LPMOEQ_Question = e.LPMOEQ_Question,
                                                       LPMOEQ_Answer = e.LPMOEQ_Answer,
                                                       LPMOEQ_Marks = a.LPMOEEXQNS_Marks,
                                                       LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                       LPMOEQ_SubjectiveFlg = e.LPMOEQ_SubjectiveFlg,
                                                       LPMOEQ_MatchTheFollowingFlg = e.LPMOEQ_MatchTheFollowingFlg,
                                                       LPMOEEXQNS_QnsOrder = a.LPMOEEXQNS_QnsOrder,
                                                       LPMOEEXLVL_Id = a.LPMOEEXLVL_Id,
                                                       LPMOEQ_StructuralFlg = e.LPMOEQ_StructuralFlg
                                                   }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToList();

                        List<long?> questionids = new List<long?>();

                        foreach (var c in getexamquestionlist)
                        {
                            questionids.Add(c.LPMOEQ_Id);
                        }

                        data.getexamquestionlist = getexamquestionlist.ToArray();

                        // Get Question Wise Option List 
                        data.getquestionoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                      from c in _context.LP_Master_OE_Exam_LevelsDMO
                                                      from d in _context.LP_Master_OE_ExamDMO
                                                      from e in _context.LP_Master_OE_QuestionsDMO
                                                      from f in _context.LP_Master_OE_QNS_OptionsDMO
                                                      where (a.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id && c.LPMOEEX_Id == d.LPMOEEX_Id && a.LPMOEQ_Id == a.LPMOEQ_Id
                                                      && a.LPMOEQ_Id == f.LPMOEQ_Id && e.LPMOEQ_Id == f.LPMOEQ_Id && a.LPMOEEXQNS_ActiveFlg == true &&
                                                      f.LPMOEQOA_ActiveFlg == true && questionids.Contains(a.LPMOEQ_Id) && d.MI_Id == data.MI_Id
                                                      && d.LPMOEEX_Id == data.LPMOEEX_Id)
                                                      select new LP_OnlineStudentExamDTO
                                                      {
                                                          LPMOEQ_Id = a.LPMOEQ_Id,
                                                          LPMOEQOA_Id = f.LPMOEQOA_Id,
                                                          LPMOEQOA_Option = f.LPMOEQOA_Option,
                                                          LPMOEQOA_OptionCode = f.LPMOEQOA_OptionCode,
                                                          LPMOEQOA_AnswerFlag = f.LPMOEQOA_AnswerFlag,
                                                          LPMOEQOA_Marks = f.LPMOEQOA_Marks,
                                                      }).Distinct().OrderBy(a => a.LPMOEQ_Id).OrderBy(a => a.LPMOEQOA_Option).ToArray();


                        // Get Question Wise Option MF List 
                        data.getquestionmfoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                        from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                        from c in _context.LP_Master_OE_QNS_Options_MFDMO
                                                        from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                        from e in _context.LP_Master_OE_ExamDMO
                                                        from f in _context.LP_Master_OE_QuestionsDMO
                                                        where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id && f.LPMOEQ_Id == a.LPMOEQ_Id
                                                        && f.LPMOEQ_Id == b.LPMOEQ_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id && d.LPMOEEX_Id == e.LPMOEEX_Id
                                                        && a.LPMOEEXQNS_ActiveFlg == true && b.LPMOEQOA_ActiveFlg == true
                                                        && questionids.Contains(a.LPMOEQ_Id) && e.MI_Id == data.MI_Id && e.LPMOEEX_Id == data.LPMOEEX_Id
                                                        && f.LPMOEQ_MatchTheFollowingFlg == true)
                                                        select new LP_OnlineStudentExamDTO
                                                        {
                                                            LPMOEQ_Id = a.LPMOEQ_Id,
                                                            LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                            LPMOEQOAMF_Id = c.LPMOEQOAMF_Id,
                                                            LPMOEQOAMF_MatchtheFollowing = c.LPMOEQOAMF_MatchtheFollowing,
                                                            LPMOEQOAMF_AnswerFlag = c.LPMOEQOAMF_AnswerFlag,
                                                            LPMOEQOAMF_Order = c.LPMOEQOAMF_Order
                                                        }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();


                        // Get Subjective Uploaded File List By Student
                        data.getquestionsubjective_fileslist = (from a in _context.LP_Students_Exam_SubjectiveAnswer_FilesDMO
                                                                from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                from c in _context.LP_Students_ExamDMO
                                                                where (a.LPSTUEXSANS_Id == b.LPSTUEXSANS_Id && b.LPSTUEX_Id == c.LPSTUEX_Id
                                                                && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id
                                                                && c.LPMOEEX_Id == data.LPMOEEX_Id && questionids.Contains(b.LPMOEQ_Id))
                                                                select new LP_OnlineStudentExamDTO
                                                                {
                                                                    LPMOEQ_Id = b.LPMOEQ_Id,
                                                                    FileName = a.LPSTUEXSANSFL_FileName,
                                                                    FilePath = a.LPSTUEXSANSFNFL_FilePath,
                                                                    LPSTUEXSANS_Id = a.LPSTUEXSANS_Id,
                                                                }).Distinct().ToArray();

                        data.getquestionsubjective_staff_fileslist = (from a in _context.LP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO
                                                                      from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                      from c in _context.LP_Students_ExamDMO
                                                                      where (a.LPSTUEXSANS_Id == b.LPSTUEXSANS_Id && b.LPSTUEX_Id == c.LPSTUEX_Id
                                                                      && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id
                                                                      && c.LPMOEEX_Id == data.LPMOEEX_Id && questionids.Contains(b.LPMOEQ_Id))
                                                                      select new LP_OnlineStudentExamDTO
                                                                      {
                                                                          LPMOEQ_Id = b.LPMOEQ_Id,
                                                                          FileName = a.LPSTUEXSANSSFL_FileName,
                                                                          FilePath = a.LPSTUEXSANSSFL_FilePath,
                                                                          LPSTUEXSANS_Id = a.LPSTUEXSANS_Id,
                                                                      }).Distinct().ToArray();

                        //Get Student Wise Question Choose The Correct Answer Marks
                        data.get_examwise_ques_option_marks = (from a in _context.LP_Students_ExamDMO
                                                               from b in _context.LP_Students_Exam_AnswerDMO
                                                               from c in _context.LP_Master_OE_ExamDMO
                                                               where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.LPMOEEX_Id == c.LPMOEEX_Id && a.ASMAY_Id == data.ASMAY_Id
                                                               && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && c.LPMOEEX_Id == data.LPMOEEX_Id
                                                               && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id
                                                               && c.ISMS_Id == data.ISMS_Id && questionids.Contains(b.LPMOEQ_Id))
                                                               select new LP_OnlineStudentExamDTO
                                                               {
                                                                   LPMOEQ_Id = b.LPMOEQ_Id,
                                                                   LPSTUEXANS_Id = b.LPSTUEXANS_Id,
                                                                   LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                                   LPMOEQOAMF_Id = b.LPMOEQOAMF_Id,
                                                                   LPSTUEXANS_AttemptFlag = b.LPSTUEXANS_AttemptFlag,
                                                                   LPSTUEXANS_Marks = b.LPSTUEXANS_Marks,
                                                                   LPSTUEXANS_CorrectAnsFlg = b.LPSTUEXANS_CorrectAnsFlg,
                                                               }).Distinct().ToArray();

                        //Get Student Wise Question Subjective Marks
                        data.get_examwise_ques_subjective_marks = (from a in _context.LP_Students_ExamDMO
                                                                   from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                   from c in _context.LP_Master_OE_ExamDMO
                                                                   where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.LPMOEEX_Id == c.LPMOEEX_Id && a.ASMAY_Id == data.ASMAY_Id
                                                                   && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && c.LPMOEEX_Id == data.LPMOEEX_Id
                                                                   && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id
                                                                   && c.ISMS_Id == data.ISMS_Id && questionids.Contains(b.LPMOEQ_Id))
                                                                   select new LP_OnlineStudentExamDTO
                                                                   {
                                                                       LPMOEQ_Id = b.LPMOEQ_Id,
                                                                       LPSTUEXSANS_Id = b.LPSTUEXSANS_Id,
                                                                       LPSTUEXANS_AttemptFlag = b.LPSTUEXANS_AttemptFlag,
                                                                       LPSTUEXSANS_Answer = b.LPSTUEXSANS_Answer,
                                                                       LPSTUEXANS_Marks = b.LPSTUEXSANS_Marks
                                                                   }).Distinct().ToArray();
                    }
                }
                else
                {
                    data.getallmarksdetails = (from a in _context.LP_Students_ExamDMO
                                               from b in _context.LP_Students_Exam_AnswersheetDMO
                                               where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                               && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id)
                                               select b).Distinct().ToArray();
                }

                data.getstudentquesansstaffdetailsview = (from a in _context.LP_Students_ExamDMO
                                                          from b in _context.LP_Students_Exam_Answersheet_StaffDMO
                                                          where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                                          && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id)
                                                          select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO SaveAnswerSheet(LP_OnlineStudentExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var getamstid = _context.StudentAppUserLoginDMO.Where(a => a.STD_APP_ID == data.UserId).ToList();
                if (getamstid.Count > 0)
                {
                    data.AMST_Id = getamstid.FirstOrDefault().AMST_ID;

                    var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                             from b in _context.Adm_M_Student
                                             from c in _context.AcademicYear
                                             from d in _context.AdmissionClass
                                             from e in _context.School_M_Section
                                             where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                             && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                             && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                             select new LP_OnlineStudentExamDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 ASMCL_Id = a.ASMCL_Id,
                                                 ASMS_Id = a.ASMS_Id,
                                                 ASMAY_Id = a.ASMAY_Id,
                                             }).Distinct().ToList();

                    if (getstudentdetails.Count > 0)
                    {
                        data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                        data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                        data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;

                        if (data.SaveAnswerSheet.Length > 0)
                        {
                            var checkexamresult = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                            && a.LPMOEEX_Id == data.LPMOEEX_Id && a.AMST_Id == data.AMST_Id).ToList();

                            if (checkexamresult.Count > 0)
                            {

                            }
                            else
                            {
                                LP_Students_ExamDMO lP_Students_ExamDMO = new LP_Students_ExamDMO();

                                lP_Students_ExamDMO.MI_Id = data.MI_Id;
                                lP_Students_ExamDMO.AMST_Id = data.AMST_Id;
                                lP_Students_ExamDMO.ASMAY_Id = data.ASMAY_Id;
                                lP_Students_ExamDMO.LPMOEEX_Id = data.LPMOEEX_Id;
                                lP_Students_ExamDMO.LPSTUEX_TotalTime = data.LPSTUEX_TotalTime;
                                lP_Students_ExamDMO.LPSTUEX_Date = indiantime0;
                                lP_Students_ExamDMO.LPSTUEX_ActiveFlg = true;
                                lP_Students_ExamDMO.LPSTUEX_CreatedBy = data.UserId;
                                lP_Students_ExamDMO.LPSTUEX_UpdatedBy = data.UserId;
                                lP_Students_ExamDMO.CreatedDate = indiantime0;
                                lP_Students_ExamDMO.UpdatedDate = indiantime0;
                                lP_Students_ExamDMO.LPSTUEX_StaffOrStudentUploadFlag = "Student";
                                _context.Add(lP_Students_ExamDMO);

                                foreach (var c in data.SaveAnswerSheet)
                                {
                                    LP_Students_Exam_AnswersheetDMO lP_Students_Exam_AnswersheetDMO = new LP_Students_Exam_AnswersheetDMO();
                                    lP_Students_Exam_AnswersheetDMO.LPSTUEX_Id = lP_Students_ExamDMO.LPSTUEX_Id;
                                    lP_Students_Exam_AnswersheetDMO.LPSTUEXAS_ActiveFlg = true;
                                    lP_Students_Exam_AnswersheetDMO.LPSTUEXAS_AnswerSheetFile = c.LPSTUEXAS_AnswerSheetFile;
                                    lP_Students_Exam_AnswersheetDMO.LPSTUEXAS_AnswerSheetPath = c.LPSTUEXAS_AnswerSheetPath;
                                    lP_Students_Exam_AnswersheetDMO.LPSTUEXAS_CreatedBy = data.UserId;
                                    lP_Students_Exam_AnswersheetDMO.LPSTUEXAS_UpdatedBy = data.UserId;
                                    lP_Students_Exam_AnswersheetDMO.LPSTUEXAS_CreatedDate = indiantime0;
                                    lP_Students_Exam_AnswersheetDMO.LPSTUEXAS_UpdatedDate = indiantime0;
                                    lP_Students_Exam_AnswersheetDMO.LPSTUEXAS_StaffOrStudentUploadFlag = "Student";
                                    _context.Add(lP_Students_Exam_AnswersheetDMO);
                                }

                                var i = _context.SaveChanges();
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Report
        public LP_OnlineStudentExamDTO getloaddatareport(LP_OnlineStudentExamDTO data)
        {
            try
            {
                data.getyearlist = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO onchangeyear(LP_OnlineStudentExamDTO data)
        {
            try
            {
                List<bool> flags = new List<bool>();

                flags.Add(true);
                flags.Add(false);

                var getuserdetails = _context.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                if (getuserdetails.Count() > 0)
                {
                    data.getclasslist = (from a in _context.LP_Master_OE_ExamDMO
                                         from b in _context.AdmissionClass
                                         from c in _context.Exm_Login_PrivilegeDMO
                                         from d in _context.Exm_Login_Privilege_SubjectsDMO
                                         from e in _context.Staff_User_Login
                                         where (a.ASMCL_Id == b.ASMCL_Id && e.IVRMSTAUL_Id == c.Login_Id && c.ELP_Id == d.ELP_Id
                                         && a.LPMOEEX_ActiveFlg == true && d.ASMCL_Id == b.ASMCL_Id && b.ASMCL_Id == d.ASMCL_Id
                                         && a.ASMAY_Id == data.ASMAY_Id && c.ASMAY_Id == data.ASMAY_Id
                                         && e.Id == data.UserId && flags.Contains(a.LPMOEEX_UploadExamPaperFlg))
                                         select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
                else
                {
                    data.getclasslist = (from a in _context.LP_Master_OE_ExamDMO
                                         from b in _context.AdmissionClass
                                         where (a.ASMCL_Id == b.ASMCL_Id && a.LPMOEEX_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id
                                         && flags.Contains(a.LPMOEEX_UploadExamPaperFlg))
                                         select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO onchangeclass(LP_OnlineStudentExamDTO data)
        {
            try
            {
                List<bool> flags = new List<bool>();

                flags.Add(true);
                flags.Add(false);


                var getuserdetails = _context.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                if (getuserdetails.Count() > 0)
                {
                    data.getsectionlist = (from a in _context.LP_Master_OE_ExamDMO
                                           from b in _context.AdmissionClass
                                           from c in _context.IVRM_School_Master_SubjectsDMO
                                           from c1 in _context.Exm_Login_PrivilegeDMO
                                           from d1 in _context.Exm_Login_Privilege_SubjectsDMO
                                           from e1 in _context.Staff_User_Login
                                           from f1 in _context.School_M_Section
                                           where (a.ASMCL_Id == b.ASMCL_Id && d1.ASMS_Id == f1.ASMS_Id && a.ISMS_Id == c.ISMS_Id
                                           && c1.ELP_Id == d1.ELP_Id && c1.Login_Id == e1.IVRMSTAUL_Id && b.ASMCL_Id == d1.ASMCL_Id
                                           && d1.ISMS_Id == c.ISMS_Id && a.LPMOEEX_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ASMCL_Id == data.ASMCL_Id && c1.ASMAY_Id == data.ASMAY_Id && d1.ASMCL_Id == data.ASMCL_Id
                                           && e1.Id == data.UserId && flags.Contains(a.LPMOEEX_UploadExamPaperFlg))
                                           select f1).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
                else
                {
                    data.getsectionlist = (from b in _context.Exm_Category_ClassDMO
                                           from c in _context.AdmissionClass
                                           from d in _context.School_M_Section
                                           where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.ECAC_ActiveFlag == true
                                           && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id)
                                           select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO OnchangeSection(LP_OnlineStudentExamDTO data)
        {
            try
            {
                List<bool> flags = new List<bool>();


                flags.Add(true);
                flags.Add(false);


                var getuserdetails = _context.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();

                if (getuserdetails.Count() > 0)
                {
                    data.getsubjectlist = (from a in _context.LP_Master_OE_ExamDMO
                                           from b in _context.AdmissionClass
                                           from c in _context.IVRM_School_Master_SubjectsDMO
                                           from c1 in _context.Exm_Login_PrivilegeDMO
                                           from d1 in _context.Exm_Login_Privilege_SubjectsDMO
                                           from e1 in _context.Staff_User_Login
                                           from f1 in _context.School_M_Section
                                           where (a.ASMCL_Id == b.ASMCL_Id && f1.ASMS_Id == d1.ASMS_Id && a.ISMS_Id == c.ISMS_Id && c1.ELP_Id == d1.ELP_Id
                                           && c1.Login_Id == e1.IVRMSTAUL_Id && b.ASMCL_Id == d1.ASMCL_Id && d1.ISMS_Id == c.ISMS_Id && a.LPMOEEX_ActiveFlg == true
                                           && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && c1.ASMAY_Id == data.ASMAY_Id
                                           && d1.ASMS_Id == data.ASMS_Id && d1.ASMCL_Id == data.ASMCL_Id && e1.Id == data.UserId
                                           && flags.Contains(a.LPMOEEX_UploadExamPaperFlg))
                                           select c).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }
                else
                {
                    data.getsubjectlist = (from a in _context.LP_Master_OE_ExamDMO
                                           from b in _context.AdmissionClass
                                           from c in _context.IVRM_School_Master_SubjectsDMO
                                           where (a.ASMCL_Id == b.ASMCL_Id && a.ISMS_Id == c.ISMS_Id && a.LPMOEEX_ActiveFlg == true && a.ASMAY_Id == data.ASMAY_Id
                                           && a.ASMCL_Id == data.ASMCL_Id && flags.Contains(a.LPMOEEX_UploadExamPaperFlg))
                                           select c).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO onchangesubject(LP_OnlineStudentExamDTO data)
        {
            try
            {
                List<bool> flags = new List<bool>();

                flags.Add(true);
                flags.Add(false);

                data.getexamlist = (from a in _context.LP_Master_OE_ExamDMO
                                    from b in _context.AdmissionClass
                                    from d in _context.School_M_Section
                                    from c in _context.IVRM_School_Master_SubjectsDMO
                                    where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ISMS_Id == c.ISMS_Id && a.LPMOEEX_ActiveFlg == true
                                    && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ISMS_Id == data.ISMS_Id
                                    && flags.Contains(a.LPMOEEX_UploadExamPaperFlg))
                                    select a).Distinct().OrderByDescending(a => a.LPMOEEX_FromDateTime).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO onchangesubject_studentmarks(LP_OnlineStudentExamDTO data)
        {
            try
            {
                List<bool> flags = new List<bool>();

                flags.Add(true);
                flags.Add(false);

                data.getexamlist = (from a in _context.LP_Master_OE_ExamDMO
                                    from b in _context.AdmissionClass
                                    from d in _context.School_M_Section
                                    from c in _context.IVRM_School_Master_SubjectsDMO
                                    where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.ISMS_Id == c.ISMS_Id && a.LPMOEEX_ActiveFlg == true
                                    && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ISMS_Id == data.ISMS_Id
                                    && flags.Contains(a.LPMOEEX_UploadExamPaperFlg) && a.EME_Id > 0)
                                    select a).Distinct().OrderByDescending(a => a.LPMOEEX_FromDateTime).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO OnChangeExam(LP_OnlineStudentExamDTO data)
        {
            try
            {
                List<bool> flags = new List<bool>();

                flags.Add(true);
                flags.Add(false);


                var getlpexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ISMS_Id == data.ISMS_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).Distinct().ToList();

                data.getlpexamdetails = getlpexamdetails.ToArray();

                if (getlpexamdetails.FirstOrDefault().EME_Id > 0)
                {
                    data.getmasterexamdetails = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.EME_Id == getlpexamdetails.FirstOrDefault().EME_Id).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO getreport(LP_OnlineStudentExamDTO data)
        {
            try
            {
                DateTime fromdate = new DateTime();
                string confromdate = "";

                fromdate = Convert.ToDateTime(data.fromdate.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdate.ToString("yyyy-MM-dd");

                DateTime todate = new DateTime();
                string contodate = "";

                todate = Convert.ToDateTime(data.todate.Value.Date.ToString("yyyy-MM-dd"));
                contodate = todate.ToString("yyyy-MM-dd");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "LP_OnlineExamStudentsMarks_New_Modify";
                    cmd.CommandText = "LP_OnlineExam_StudentsMarks_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar) { Value = confromdate });
                    cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = contodate });
                    cmd.Parameters.Add(new SqlParameter("@LPMOEEX_Id", SqlDbType.VarChar) { Value = data.LPMOEEX_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO ViewStudentWiseMarks(LP_OnlineStudentExamDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         from c in _context.AcademicYear
                                         from d in _context.AdmissionClass
                                         from e in _context.School_M_Section
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                         && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && b.MI_Id == data.MI_Id
                                         && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                         select new LP_OnlineStudentExamDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id,
                                             ASMAY_Id = a.ASMAY_Id,
                                         }).Distinct().ToList();

                data.getstudentdetails = getstudentdetails.ToArray();

                data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                data.ASMAY_Id = getstudentdetails.FirstOrDefault().ASMAY_Id;

                data.getmarksdetails = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id
                && a.LPMOEEX_Id == data.LPMOEEX_Id).Distinct().ToArray();

                var getexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                data.getexamdetails = getexamdetails.ToArray();

                if (getexamdetails.FirstOrDefault().LPMOEEX_UploadExamPaperFlg == false)
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LP_Online_Exam_StudentWiseMarks_Details_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300000;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = data.AMST_Id });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@LPMOEEX_Id", SqlDbType.VarChar) { Value = data.LPMOEEX_Id });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.getallmarksdetails = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                if (getexamdetails.Count > 0)
                {
                    var getrandomflag = getexamdetails.FirstOrDefault().LPMOEEX_RandomFlg;
                    var getnoofquestion = getexamdetails.FirstOrDefault().LPMOEEX_NoOfQuestion;
                    var getuploadflag = getexamdetails.FirstOrDefault().LPMOEEX_UploadExamPaperFlg;
                    var LPMOEEX_NotLinkedToQnsBankFlg = getexamdetails.FirstOrDefault().LPMOEEX_NotLinkedToQnsBankFlg;

                    if (getuploadflag == false)
                    {
                        data.getexamleveldetails = _context.LP_Master_OE_Exam_LevelsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id).OrderBy(a => a.LPMOEEXLVL_LevelOrder).ToArray();

                        if (LPMOEEX_NotLinkedToQnsBankFlg == true)
                        {
                            var getexamquestionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                       from b in _context.LP_Master_OE_ExamDMO
                                                       from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                       where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                       && a.LPMOEEXQNS_ActiveFlg == true && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQ_Question = a.LPMOEEXQNS_Question,
                                                           LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQ_SubjectiveFlg = a.LPMOEEXQNS_SubjectiveFlg,
                                                           LPMOEQ_MatchTheFollowingFlg = a.LPMOEEXQNS_MatchTheFollowingFlg,
                                                           LPMOEEXQNS_QnsOrder = a.LPMOEEXQNS_QnsOrder,
                                                           LPMOEEXLVL_Id = a.LPMOEEXLVL_Id,
                                                           LPMOEQ_StructuralFlg = a.LPMOEEXQNS_QuestionType
                                                       }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToList();

                            List<long?> questionids = new List<long?>();

                            foreach (var c in getexamquestionlist)
                            {
                                questionids.Add(c.LPMOEQ_Id);
                            }

                            data.getexamquestionlist = getexamquestionlist.ToArray();

                            data.getquestionoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                          from b in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                          from c in _context.LP_Master_OE_Exam_LevelsDMO
                                                          from d in _context.LP_Master_OE_ExamDMO
                                                          where (a.LPMOEEXQNS_Id == b.LPMOEEXQNS_Id && a.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                          && c.LPMOEEX_Id == d.LPMOEEX_Id && a.LPMOEEXQNS_ActiveFlg == true && b.LPMOEEXQNSOPT_ActiveFlg == true
                                                          && questionids.Contains(b.LPMOEEXQNS_Id) && d.MI_Id == data.MI_Id && d.LPMOEEX_Id == data.LPMOEEX_Id)
                                                          select new LP_OnlineStudentExamDTO
                                                          {
                                                              LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                              LPMOEQOA_Id = b.LPMOEEXQNSOPT_Id,
                                                              LPMOEQOA_Option = b.LPMOEEXQNSOPT_Option,
                                                              LPMOEQOA_OptionCode = b.LPMOEEXQNSOPT_OptionCode,
                                                              LPMOEQOA_AnswerFlag = b.LPMOEEXQNSOPT_AnswerFlag,
                                                              LPMOEQOA_Marks = b.LPMOEEXQNSOPT_Marks,
                                                          }).Distinct().OrderBy(a => a.LPMOEQ_Id).OrderBy(a => a.LPMOEQOA_Option).ToArray();


                            data.getquestionmfoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                            from b in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                            from c in _context.LP_Master_OE_Exam_Questions_Options_MFDMO
                                                            from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                            from e in _context.LP_Master_OE_ExamDMO
                                                            where (a.LPMOEEXQNS_Id == b.LPMOEEXQNS_Id && b.LPMOEEXQNSOPT_Id == c.LPMOEEXQNSOPT_Id
                                                            && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id && d.LPMOEEX_Id == e.LPMOEEX_Id
                                                            && a.LPMOEEXQNS_ActiveFlg == true && b.LPMOEEXQNSOPT_ActiveFlg == true
                                                            && questionids.Contains(a.LPMOEEXQNS_Id) && e.MI_Id == data.MI_Id && e.LPMOEEX_Id == data.LPMOEEX_Id
                                                            && a.LPMOEEXQNS_MatchTheFollowingFlg == true)
                                                            select new LP_OnlineStudentExamDTO
                                                            {
                                                                LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                                LPMOEQOA_Id = b.LPMOEEXQNSOPT_Id,
                                                                LPMOEQOAMF_Id = c.LPMOEEXQNSOPTMF_Id,
                                                                LPMOEQOAMF_MatchtheFollowing = c.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                                                LPMOEQOAMF_AnswerFlag = c.LPMOEEXQNSOPTMF_Answer_Flg,
                                                                LPMOEQOAMF_Order = c.LPMOEEXQNSOPTMF_Order
                                                            }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();


                            var getexamwise_mfquestions = (from a in _context.LP_Master_OE_ExamDMO
                                                           from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                           from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                           where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                           && c.LPMOEEXQNS_ActiveFlg == true && c.LPMOEEXQNS_MatchTheFollowingFlg == true)
                                                           select new LP_OnlineStudentExamDTO
                                                           {
                                                               LPMOEQ_Id = c.LPMOEEXQNS_Id,
                                                               LPMOEQ_Question = c.LPMOEEXQNS_Question,
                                                               LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                               LPMOEQ_SubjectiveFlg = c.LPMOEEXQNS_SubjectiveFlg,
                                                               LPMOEQ_MatchTheFollowingFlg = c.LPMOEEXQNS_MatchTheFollowingFlg,
                                                               LPMOEEXQNS_QnsOrder = c.LPMOEEXQNS_QnsOrder,
                                                               LPMOEQ_StructuralFlg = c.LPMOEEXQNS_QuestionType
                                                           }).Distinct().ToList();

                            data.getexamwise_mfquestions = getexamwise_mfquestions.ToArray();

                            List<long?> ques_ids = new List<long?>();

                            foreach (var c in getexamwise_mfquestions)
                            {
                                ques_ids.Add(c.LPMOEQ_Id);
                            }

                            data.getexamwise_ques_options_mf_marks = (from a in _context.LP_Students_ExamDMO
                                                                      from b in _context.LP_Students_Exam_AnswerDMO
                                                                      where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                                                      && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id
                                                                      && ques_ids.Contains(b.LPMOEEXQNS_Id))
                                                                      select new LP_OnlineStudentExamDTO
                                                                      {
                                                                          LPSTUEXANS_Id = b.LPSTUEXANS_Id,
                                                                          LPSTUEX_Id = b.LPSTUEX_Id,
                                                                          LPMOEQ_Id = b.LPMOEEXQNS_Id,
                                                                          LPMOEQOA_Id = b.LPMOEEXQNSOPT_Id,
                                                                          LPMOEQOAMF_Id = b.LPMOEEXQNSOPTMF_Id,
                                                                          LPSTUEXANS_Marks = b.LPSTUEXANS_Marks,
                                                                          LPSTUEXANS_AttemptFlag = b.LPSTUEXANS_AttemptFlag,
                                                                          LPSTUEXANS_CorrectAnsFlg = b.LPSTUEXANS_CorrectAnsFlg
                                                                      }).Distinct().ToArray();

                            var getexamwise_ques_options_mf = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                               from b in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                               from c in _context.LP_Master_OE_Exam_Questions_Options_MFDMO
                                                               where (a.LPMOEEXQNS_Id == b.LPMOEEXQNS_Id && b.LPMOEEXQNSOPT_Id == c.LPMOEEXQNSOPT_Id
                                                               && ques_ids.Contains(a.LPMOEEXQNS_Id) && b.LPMOEEXQNSOPT_ActiveFlg == true
                                                               && a.LPMOEEXQNS_MatchTheFollowingFlg == true)
                                                               select new LP_OnlineStudentExamDTO
                                                               {
                                                                   LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                                   LPMOEQOA_Id = b.LPMOEEXQNSOPT_Id,
                                                                   LPMOEQOAMF_Id = c.LPMOEEXQNSOPTMF_Id,
                                                                   LPMOEQOAMF_MatchtheFollowing = c.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                                                   LPMOEQOAMF_AnswerFlag = c.LPMOEEXQNSOPTMF_Answer_Flg,
                                                                   LPMOEQOAMF_Order = c.LPMOEEXQNSOPTMF_Order,
                                                               }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();

                            data.getexamwise_ques_options_mf = getexamwise_ques_options_mf;
                        }
                        else
                        {
                            var getexamquestionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                       from b in _context.LP_Master_OE_ExamDMO
                                                       from c in _context.LP_Master_OE_QuestionsDMO
                                                       from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                       where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id && a.LPMOEQ_Id == c.LPMOEQ_Id
                                                       && a.LPMOEEXQNS_ActiveFlg == true && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEQ_Id,
                                                           LPMOEQ_Question = c.LPMOEQ_Question,
                                                           LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQ_SubjectiveFlg = c.LPMOEQ_SubjectiveFlg,
                                                           LPMOEQ_MatchTheFollowingFlg = c.LPMOEQ_MatchTheFollowingFlg,
                                                           LPMOEEXQNS_QnsOrder = a.LPMOEEXQNS_QnsOrder,
                                                           LPMOEEXLVL_Id = a.LPMOEEXLVL_Id,
                                                           LPMOEQ_StructuralFlg = c.LPMOEQ_StructuralFlg
                                                       }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToList();

                            List<long?> questionids = new List<long?>();

                            foreach (var c in getexamquestionlist)
                            {
                                questionids.Add(c.LPMOEQ_Id);
                            }

                            data.getexamquestionlist = getexamquestionlist.ToArray();

                            data.getquestionoptionlist = (from a in _context.LP_Master_OE_QuestionsDMO
                                                          from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                          where (a.LPMOEQ_Id == b.LPMOEQ_Id && a.LPMOEQ_ActiveFlg == true && b.LPMOEQOA_ActiveFlg == true
                                                           && questionids.Contains(b.LPMOEQ_Id) && a.MI_Id == data.MI_Id)
                                                          select new LP_OnlineStudentExamDTO
                                                          {
                                                              LPMOEQ_Id = a.LPMOEQ_Id,
                                                              LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                              LPMOEQOA_Option = b.LPMOEQOA_Option,
                                                              LPMOEQOA_OptionCode = b.LPMOEQOA_OptionCode,
                                                              LPMOEQOA_AnswerFlag = b.LPMOEQOA_AnswerFlag,
                                                              LPMOEQOA_Marks = b.LPMOEQOA_Marks,
                                                          }).Distinct().OrderBy(a => a.LPMOEQ_Id).OrderBy(a => a.LPMOEQOA_Option).ToArray();


                            data.getquestionmfoptionlist = (from a in _context.LP_Master_OE_QuestionsDMO
                                                            from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                            from c in _context.LP_Master_OE_QNS_Options_MFDMO
                                                            where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id
                                                            && a.LPMOEQ_ActiveFlg == true && b.LPMOEQOA_ActiveFlg == true
                                                            && questionids.Contains(a.LPMOEQ_Id) && a.MI_Id == data.MI_Id
                                                            && a.LPMOEQ_MatchTheFollowingFlg == true)
                                                            select new LP_OnlineStudentExamDTO
                                                            {
                                                                LPMOEQ_Id = a.LPMOEQ_Id,
                                                                LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                                LPMOEQOAMF_Id = c.LPMOEQOAMF_Id,
                                                                LPMOEQOAMF_MatchtheFollowing = c.LPMOEQOAMF_MatchtheFollowing,
                                                                LPMOEQOAMF_AnswerFlag = c.LPMOEQOAMF_AnswerFlag,
                                                                LPMOEQOAMF_Order = c.LPMOEQOAMF_Order
                                                            }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();


                            var getexamwise_mfquestions = (from a in _context.LP_Master_OE_ExamDMO
                                                           from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                           from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                           from d in _context.LP_Master_OE_QuestionsDMO
                                                           where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id && c.LPMOEQ_Id == d.LPMOEQ_Id
                                                           && c.LPMOEEXQNS_ActiveFlg == true && d.LPMOEQ_MatchTheFollowingFlg == true)
                                                           select d).Distinct().ToList();

                            data.getexamwise_mfquestions = getexamwise_mfquestions.ToArray();

                            List<long?> ques_ids = new List<long?>();

                            foreach (var c in getexamwise_mfquestions)
                            {
                                ques_ids.Add(c.LPMOEQ_Id);
                            }


                            data.getexamwise_ques_options_mf_marks = (from a in _context.LP_Students_ExamDMO
                                                                      from b in _context.LP_Students_Exam_AnswerDMO
                                                                      where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                                                      && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && ques_ids.Contains(b.LPMOEQ_Id))
                                                                      select b).Distinct().ToArray();

                            var getexamwise_ques_options_mf = (from a in _context.LP_Master_OE_QuestionsDMO
                                                               from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                               from c in _context.LP_Master_OE_QNS_Options_MFDMO
                                                               where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id
                                                               && ques_ids.Contains(a.LPMOEQ_Id) && b.LPMOEQOA_ActiveFlg == true
                                                               && a.LPMOEQ_MatchTheFollowingFlg == true)
                                                               select new LP_OnlineStudentExamDTO
                                                               {
                                                                   LPMOEQ_Id = a.LPMOEQ_Id,
                                                                   LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                                   LPMOEQOAMF_Id = c.LPMOEQOAMF_Id,
                                                                   LPMOEQOAMF_MatchtheFollowing = c.LPMOEQOAMF_MatchtheFollowing,
                                                                   LPMOEQOAMF_AnswerFlag = c.LPMOEQOAMF_AnswerFlag,
                                                                   LPMOEQOAMF_Order = c.LPMOEQOAMF_Order
                                                               }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();

                            data.getexamwise_ques_options_mf = getexamwise_ques_options_mf;


                            data.getquestiondoclist = (from a in _context.LP_Master_OE_QuestionsDMO
                                                       from b in _context.LP_Master_OE_Questions_FilesDMO
                                                       where (a.LPMOEQ_Id == b.LPMOEQ_Id && a.LPMOEQ_ActiveFlg == true && b.LPMOEQF_ActiveFlag == true
                                                        && questionids.Contains(b.LPMOEQ_Id) && a.MI_Id == data.MI_Id)
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEQ_Id,
                                                           LPMOEQF_FileName = b.LPMOEQF_FileName,
                                                           LPMOEQF_FilePath = b.LPMOEQF_FilePath
                                                       }).Distinct().ToArray();


                            data.getoptionwisefiles = (from a in _context.LP_Master_OE_QuestionsDMO
                                                       from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                       from c in _context.LP_Master_OE_QNS_Options_FilesDMO
                                                       where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id && a.LPMOEQ_ActiveFlg == true
                                                       && b.LPMOEQOA_ActiveFlg == true && c.LPMOEQOAF_ActiveFlag == true
                                                        && questionids.Contains(b.LPMOEQ_Id) && a.MI_Id == data.MI_Id)
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                           LPMOEQOAF_FileName = c.LPMOEQOAF_FileName,
                                                           LPMOEQOAF_FilePath = c.LPMOEQOAF_FilePath
                                                       }).Distinct().ToArray();
                        }
                    }

                    else
                    {
                        data.getallmarksdetails = (from a in _context.LP_Students_ExamDMO
                                                   from b in _context.LP_Students_Exam_AnswersheetDMO
                                                   where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                                   && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id)
                                                   select b).Distinct().ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // CORRECTION
        public LP_OnlineStudentExamDTO GetSearchDetails(LP_OnlineStudentExamDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    //cmd.CommandText = "LP_OnlineExam_Get_StudentAnswerPaper_List_modfiy";
                    cmd.CommandText = "LP_OnlineExam_Get_StudentAnswerPaper_List_Modfiy_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@LPMOEEX_Id", SqlDbType.VarChar) { Value = data.LPMOEEX_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getstudentdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                var examdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.ISMS_Id == data.ISMS_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                if (examdetails.FirstOrDefault().LPMOEEX_UploadExamPaperFlg == true)
                {
                    data.getstudentquesansdetails = (from a in _context.LP_Students_ExamDMO
                                                     from b in _context.LP_Students_Exam_AnswersheetDMO
                                                     where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                                     && a.LPMOEEX_Id == data.LPMOEEX_Id)
                                                     select new LP_OnlineStudentExamDTO
                                                     {
                                                         AMST_Id = a.AMST_Id,
                                                         LPSTUEX_Id = b.LPSTUEX_Id,
                                                         LPSTUEXAS_Id = b.LPSTUEXAS_Id,
                                                         LPSTUEXAS_AnswerSheetFile = b.LPSTUEXAS_AnswerSheetFile,
                                                         LPSTUEXAS_AnswerSheetPath = b.LPSTUEXAS_AnswerSheetPath,
                                                         LPSTUEXAS_StaffOrStudentUploadFlag = b.LPSTUEXAS_StaffOrStudentUploadFlag,
                                                     }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Save Upload Marks
        public LP_OnlineStudentExamDTO SaveMarks(LP_OnlineStudentExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.savemarks != null && data.savemarks.Length > 0)
                {
                    // var getexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                    var gettotalmarks = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                    data.LPSTUEX_TotalMaxMarks = gettotalmarks.FirstOrDefault().LPMOEEX_TotalMarks;


                    foreach (var c in data.savemarks)
                    {
                        data.AMST_Id = c.AMST_Id;
                        data.LPSTUEX_Id = c.LPSTUEX_Id;
                        string filepath = c.LPSTUEX_CorrectedAnswerSheetPath;
                        string filename = c.LPSTUEX_CorrectedAnswerSheetFile;
                        data.LPSTUEX_TotalMarks = c.marks;
                        decimal? percentage = (data.LPSTUEX_TotalMarks / data.LPSTUEX_TotalMaxMarks) * 100;
                        percentage = Math.Round(Convert.ToDecimal(percentage), 0, MidpointRounding.AwayFromZero);


                        var getresult = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPSTUEX_Id == data.LPSTUEX_Id
                        && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                        if (getresult.Count > 0)
                        {
                            var result = _context.LP_Students_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.LPSTUEX_Id == data.LPSTUEX_Id
                            && a.LPMOEEX_Id == data.LPMOEEX_Id);

                            result.LPSTUEX_TotalMarks = data.LPSTUEX_TotalMarks;
                            result.LPSTUEX_TotalMaxMarks = data.LPSTUEX_TotalMaxMarks;
                            result.LPSTUEX_Percentage = percentage;
                            result.UpdatedDate = indiantime0;
                            result.LPSTUEX_UpdatedBy = data.UserId;
                            //result.LPSTUEX_PublishToStudent = gettotalmarks.FirstOrDefault().LPMOEEX_AutoPublishFlg;
                            _context.Update(result);
                        }
                    }

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Save";
                    }
                    else
                    {
                        data.message = "Fail";
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Fail";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO SaveStudentAnswerFileByStaff(LP_OnlineStudentExamDTO data)
        {
            try
            {
                data.message = "Failed";

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);
                LP_Students_ExamDMO lP_Students_ExamDMO = new LP_Students_ExamDMO();

                long LPSTUEX_Id = 0;
                var checkstudentid = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.LPMOEEX_Id == data.LPMOEEX_Id
                 && a.MI_Id == data.MI_Id && a.LPSTUEX_ActiveFlg == true && a.AMST_Id == data.AMST_Id).ToList();

                if (checkstudentid.Count > 0)
                {
                    LPSTUEX_Id = checkstudentid.FirstOrDefault().LPSTUEX_Id;
                    data.LPSTUEX_Id = LPSTUEX_Id;

                    var result = _context.LP_Students_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id
                    && a.LPSTUEX_Id == LPSTUEX_Id);
                    result.LPSTUEX_UpdatedBy = data.UserId;
                    result.UpdatedDate = indiantime0;
                    result.LPSTUEX_MergedFile = "";
                    _context.Update(result);
                }
                else
                {
                    lP_Students_ExamDMO.MI_Id = data.MI_Id;
                    lP_Students_ExamDMO.AMST_Id = data.AMST_Id;
                    lP_Students_ExamDMO.ASMAY_Id = data.ASMAY_Id;
                    lP_Students_ExamDMO.LPSTUEX_Date = indiantime0;
                    lP_Students_ExamDMO.LPSTUEX_TotalTime = "00:10:00";
                    lP_Students_ExamDMO.LPSTUEX_TotalQnsAnswered = 0;
                    lP_Students_ExamDMO.LPSTUEX_TotalCorrectAns = 0;
                    lP_Students_ExamDMO.LPSTUEX_TotalMaxMarks = null;
                    lP_Students_ExamDMO.LPSTUEX_TotalMarks = null;
                    lP_Students_ExamDMO.LPSTUEX_Percentage = null;
                    lP_Students_ExamDMO.LPSTUEX_ActiveFlg = true;
                    lP_Students_ExamDMO.LPSTUEX_CreatedBy = data.UserId;
                    lP_Students_ExamDMO.LPSTUEX_UpdatedBy = data.UserId;
                    lP_Students_ExamDMO.CreatedDate = indiantime0;
                    lP_Students_ExamDMO.UpdatedDate = indiantime0;
                    lP_Students_ExamDMO.LPMOEEX_Id = data.LPMOEEX_Id;
                    lP_Students_ExamDMO.LPSTUEX_StaffOrStudentUploadFlag = "Staff";
                    lP_Students_ExamDMO.LPSTUEX_MergedFile = "";
                    _context.Add(lP_Students_ExamDMO);
                    data.LPSTUEX_Id = lP_Students_ExamDMO.LPSTUEX_Id;
                }

                if (data.missingfiles != null && data.missingfiles.Length > 0)
                {
                    foreach (var c in data.missingfiles)
                    {
                        LP_Students_Exam_AnswersheetDMO lP_Students_Exam_AnswersheetDMO = new LP_Students_Exam_AnswersheetDMO
                        {
                            LPSTUEX_Id = data.LPSTUEX_Id,
                            LPSTUEXAS_AnswerSheetFile = c.LPSTUEXAS_AnswerSheetFile,
                            LPSTUEXAS_AnswerSheetPath = c.LPSTUEXAS_AnswerSheetPath,
                            LPSTUEXAS_ActiveFlg = true,
                            LPSTUEXAS_CreatedBy = data.UserId,
                            LPSTUEXAS_CreatedDate = indiantime0,
                            LPSTUEXAS_UpdatedBy = data.UserId,
                            LPSTUEXAS_UpdatedDate = indiantime0,
                            LPSTUEXAS_StaffOrStudentUploadFlag = "Staff"
                        };

                        _context.Add(lP_Students_Exam_AnswersheetDMO);
                    }
                }

                if (data.correctedanswerfiles != null && data.correctedanswerfiles.Length > 0)
                {
                    foreach (var c in data.correctedanswerfiles)
                    {
                        if (c.LPSTUEXASTF_Id > 0)
                        {
                            var result = _context.LP_Students_Exam_Answersheet_StaffDMO.Single(a => a.LPSTUEXASTF_Id == c.LPSTUEXASTF_Id);
                            result.LPSTUEXASTF_AnswerSheetFile = c.LPSTUEXASTF_AnswerSheetFile;
                            result.LPSTUEXASTF_AnswerSheetPath = c.LPSTUEXASTF_AnswerSheetPath;
                            result.LPSTUEXASTF_UpdatedBy = data.UserId;
                            result.LPSTUEXASTF_UpdatedDate = indiantime0;
                            _context.Update(result);
                        }
                        else
                        {
                            LP_Students_Exam_Answersheet_StaffDMO lP_Students_Exam_Answersheet_StaffDMO = new LP_Students_Exam_Answersheet_StaffDMO
                            {
                                LPSTUEX_Id = data.LPSTUEX_Id,
                                LPSTUEXASTF_AnswerSheetFile = c.LPSTUEXASTF_AnswerSheetFile,
                                LPSTUEXASTF_AnswerSheetPath = c.LPSTUEXASTF_AnswerSheetPath,
                                LPSTUEXASTF_ActiveFlg = true,
                                LPSTUEXASTF_CreatedBy = data.UserId,
                                LPSTUEXASTF_CreatedDate = indiantime0,
                                LPSTUEXASTF_UpdatedBy = data.UserId,
                                LPSTUEXASTF_UpdatedDate = indiantime0,
                            };
                            _context.Add(lP_Students_Exam_Answersheet_StaffDMO);
                        }
                    }
                }

                var i = _context.SaveChanges();

                if (i > 0)
                {
                    data.message = "Save";
                    data.LPSTUEX_Id = lP_Students_ExamDMO.LPSTUEX_Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO ViewQuestion(LP_OnlineStudentExamDTO data)
        {
            try
            {
                var getexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                data.get_student_exam_details = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.LPMOEEX_Id == data.LPMOEEX_Id && a.AMST_Id == data.AMST_Id).Distinct().ToArray();

                if (getexamdetails.FirstOrDefault().LPMOEEX_UploadExamPaperFlg == false)
                {
                    data.getexamleveldetails = _context.LP_Master_OE_Exam_LevelsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id).OrderBy(a => a.LPMOEEXLVL_LevelOrder).ToArray();

                    if (getexamdetails.FirstOrDefault().LPMOEEX_NotLinkedToQnsBankFlg == true)
                    {
                        // Get Question List 
                        var getexamquestionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                   from b in _context.LP_Master_OE_ExamDMO
                                                   from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                   where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                   && a.LPMOEEXQNS_ActiveFlg == true && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                   select new LP_OnlineStudentExamDTO
                                                   {
                                                       LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                       LPMOEQ_Question = a.LPMOEEXQNS_Question,
                                                       LPMOEQ_Marks = a.LPMOEEXQNS_Marks,
                                                       LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                       LPMOEQ_SubjectiveFlg = a.LPMOEEXQNS_SubjectiveFlg,
                                                       LPMOEQ_MatchTheFollowingFlg = a.LPMOEEXQNS_MatchTheFollowingFlg,
                                                       LPMOEEXQNS_QnsOrder = a.LPMOEEXQNS_QnsOrder,
                                                       LPMOEEXLVL_Id = a.LPMOEEXLVL_Id,
                                                       LPMOEQ_StructuralFlg = a.LPMOEEXQNS_QuestionType
                                                   }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToList();

                        List<long?> questionids = new List<long?>();

                        foreach (var c in getexamquestionlist)
                        {
                            questionids.Add(c.LPMOEQ_Id);
                        }

                        data.getexamquestionlist = getexamquestionlist.ToArray();

                        // Get Question Wise Option List 
                        data.getquestionoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                      from b in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                      from c in _context.LP_Master_OE_Exam_LevelsDMO
                                                      from d in _context.LP_Master_OE_ExamDMO
                                                      where (a.LPMOEEXQNS_Id == b.LPMOEEXQNS_Id && a.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                      && c.LPMOEEX_Id == d.LPMOEEX_Id && a.LPMOEEXQNS_ActiveFlg == true && b.LPMOEEXQNSOPT_ActiveFlg == true
                                                      && questionids.Contains(b.LPMOEEXQNS_Id) && d.MI_Id == data.MI_Id && d.LPMOEEX_Id == data.LPMOEEX_Id)
                                                      select new LP_OnlineStudentExamDTO
                                                      {
                                                          LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                          LPMOEQOA_Id = b.LPMOEEXQNSOPT_Id,
                                                          LPMOEQOA_Option = b.LPMOEEXQNSOPT_Option,
                                                          LPMOEQOA_OptionCode = b.LPMOEEXQNSOPT_OptionCode,
                                                          LPMOEQOA_AnswerFlag = b.LPMOEEXQNSOPT_AnswerFlag,
                                                          LPMOEQOA_Marks = b.LPMOEEXQNSOPT_Marks,
                                                      }).Distinct().OrderBy(a => a.LPMOEQ_Id).OrderBy(a => a.LPMOEQOA_Option).ToArray();

                        // Get Question Wise Option MF List 
                        data.getquestionmfoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                        from b in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                        from c in _context.LP_Master_OE_Exam_Questions_Options_MFDMO
                                                        from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                        from e in _context.LP_Master_OE_ExamDMO
                                                        where (a.LPMOEEXQNS_Id == b.LPMOEEXQNS_Id && b.LPMOEEXQNSOPT_Id == c.LPMOEEXQNSOPT_Id
                                                        && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id && d.LPMOEEX_Id == e.LPMOEEX_Id
                                                        && a.LPMOEEXQNS_ActiveFlg == true && b.LPMOEEXQNSOPT_ActiveFlg == true
                                                        && questionids.Contains(a.LPMOEEXQNS_Id) && e.MI_Id == data.MI_Id && e.LPMOEEX_Id == data.LPMOEEX_Id
                                                        && a.LPMOEEXQNS_MatchTheFollowingFlg == true)
                                                        select new LP_OnlineStudentExamDTO
                                                        {
                                                            LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                            LPMOEQOA_Id = b.LPMOEEXQNSOPT_Id,
                                                            LPMOEQOAMF_Id = c.LPMOEEXQNSOPTMF_Id,
                                                            LPMOEQOAMF_MatchtheFollowing = c.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                                            LPMOEQOAMF_AnswerFlag = c.LPMOEEXQNSOPTMF_Answer_Flg,
                                                            LPMOEQOAMF_Order = c.LPMOEEXQNSOPTMF_Order
                                                        }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();

                        // Get Subjective Uploaded File List By Student
                        data.getquestionsubjective_fileslist = (from a in _context.LP_Students_Exam_SubjectiveAnswer_FilesDMO
                                                                from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                from c in _context.LP_Students_ExamDMO
                                                                where (a.LPSTUEXSANS_Id == b.LPSTUEXSANS_Id && b.LPSTUEX_Id == c.LPSTUEX_Id
                                                                && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id
                                                                && c.LPMOEEX_Id == data.LPMOEEX_Id && questionids.Contains(b.LPMOEEXQNS_Id))
                                                                select new LP_OnlineStudentExamDTO
                                                                {
                                                                    LPMOEQ_Id = b.LPMOEEXQNS_Id,
                                                                    FileName = a.LPSTUEXSANSFL_FileName,
                                                                    FilePath = a.LPSTUEXSANSFNFL_FilePath,
                                                                    LPSTUEXSANS_Id = a.LPSTUEXSANS_Id,
                                                                }).Distinct().ToArray();

                        data.getquestionsubjective_staff_fileslist = (from a in _context.LP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO
                                                                      from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                      from c in _context.LP_Students_ExamDMO
                                                                      where (a.LPSTUEXSANS_Id == b.LPSTUEXSANS_Id && b.LPSTUEX_Id == c.LPSTUEX_Id
                                                                      && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id
                                                                      && c.LPMOEEX_Id == data.LPMOEEX_Id && questionids.Contains(b.LPMOEEXQNS_Id))
                                                                      select new LP_OnlineStudentExamDTO
                                                                      {
                                                                          LPMOEQ_Id = b.LPMOEEXQNS_Id,
                                                                          FileName = a.LPSTUEXSANSSFL_FileName,
                                                                          FilePath = a.LPSTUEXSANSSFL_FilePath,
                                                                          LPSTUEXSANS_Id = a.LPSTUEXSANS_Id,
                                                                      }).Distinct().ToArray();

                        //Get Student Wise Question Choose The Correct Answer Marks
                        data.get_examwise_ques_option_marks = (from a in _context.LP_Students_ExamDMO
                                                               from b in _context.LP_Students_Exam_AnswerDMO
                                                               from c in _context.LP_Master_OE_ExamDMO
                                                               where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.LPMOEEX_Id == c.LPMOEEX_Id && a.ASMAY_Id == data.ASMAY_Id
                                                               && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && c.LPMOEEX_Id == data.LPMOEEX_Id
                                                               && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id
                                                               && c.ISMS_Id == data.ISMS_Id && questionids.Contains(b.LPMOEEXQNS_Id))
                                                               select new LP_OnlineStudentExamDTO
                                                               {
                                                                   LPMOEQ_Id = b.LPMOEEXQNS_Id,
                                                                   LPSTUEXANS_Id = b.LPSTUEXANS_Id,
                                                                   LPMOEQOA_Id = b.LPMOEEXQNSOPT_Id,
                                                                   LPMOEQOAMF_Id = b.LPMOEEXQNSOPTMF_Id,
                                                                   LPSTUEXANS_AttemptFlag = b.LPSTUEXANS_AttemptFlag,
                                                                   LPSTUEXANS_Marks = b.LPSTUEXANS_Marks,
                                                                   LPSTUEXANS_CorrectAnsFlg = b.LPSTUEXANS_CorrectAnsFlg,
                                                               }).Distinct().ToArray();

                        //Get Student Wise Question Subjective Marks
                        data.get_examwise_ques_subjective_marks = (from a in _context.LP_Students_ExamDMO
                                                                   from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                   from c in _context.LP_Master_OE_ExamDMO
                                                                   where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.LPMOEEX_Id == c.LPMOEEX_Id && a.ASMAY_Id == data.ASMAY_Id
                                                                   && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && c.LPMOEEX_Id == data.LPMOEEX_Id
                                                                   && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id
                                                                   && c.ISMS_Id == data.ISMS_Id && questionids.Contains(b.LPMOEEXQNS_Id))
                                                                   select new LP_OnlineStudentExamDTO
                                                                   {
                                                                       LPMOEQ_Id = b.LPMOEEXQNS_Id,
                                                                       LPSTUEXSANS_Id = b.LPSTUEXSANS_Id,
                                                                       LPSTUEXANS_AttemptFlag = b.LPSTUEXANS_AttemptFlag,
                                                                       LPSTUEXSANS_Answer = b.LPSTUEXSANS_Answer,
                                                                       LPSTUEXANS_Marks = b.LPSTUEXSANS_Marks
                                                                   }).Distinct().ToArray();
                    }
                    else
                    {
                        // Get Question List 
                        var getexamquestionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                   from b in _context.LP_Master_OE_ExamDMO
                                                   from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                   from e in _context.LP_Master_OE_QuestionsDMO
                                                   where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id && a.LPMOEQ_Id == e.LPMOEQ_Id
                                                   && a.LPMOEEXQNS_ActiveFlg == true && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                   select new LP_OnlineStudentExamDTO
                                                   {
                                                       LPMOEQ_Id = a.LPMOEQ_Id,
                                                       LPMOEQ_Question = e.LPMOEQ_Question,
                                                       LPMOEQ_Marks = a.LPMOEEXQNS_Marks,
                                                       LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                       LPMOEQ_SubjectiveFlg = e.LPMOEQ_SubjectiveFlg,
                                                       LPMOEQ_MatchTheFollowingFlg = e.LPMOEQ_MatchTheFollowingFlg,
                                                       LPMOEEXQNS_QnsOrder = a.LPMOEEXQNS_QnsOrder,
                                                       LPMOEEXLVL_Id = a.LPMOEEXLVL_Id,
                                                       LPMOEQ_StructuralFlg = e.LPMOEQ_StructuralFlg
                                                   }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToList();

                        List<long?> questionids = new List<long?>();

                        foreach (var c in getexamquestionlist)
                        {
                            questionids.Add(c.LPMOEQ_Id);
                        }

                        data.getexamquestionlist = getexamquestionlist.ToArray();

                        // Get Question Wise Option List 
                        data.getquestionoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                      from c in _context.LP_Master_OE_Exam_LevelsDMO
                                                      from d in _context.LP_Master_OE_ExamDMO
                                                      from e in _context.LP_Master_OE_QuestionsDMO
                                                      from f in _context.LP_Master_OE_QNS_OptionsDMO
                                                      where (a.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id && c.LPMOEEX_Id == d.LPMOEEX_Id && a.LPMOEQ_Id == a.LPMOEQ_Id
                                                      && a.LPMOEQ_Id == f.LPMOEQ_Id && e.LPMOEQ_Id == f.LPMOEQ_Id && a.LPMOEEXQNS_ActiveFlg == true &&
                                                      f.LPMOEQOA_ActiveFlg == true && questionids.Contains(a.LPMOEQ_Id) && d.MI_Id == data.MI_Id
                                                      && d.LPMOEEX_Id == data.LPMOEEX_Id)
                                                      select new LP_OnlineStudentExamDTO
                                                      {
                                                          LPMOEQ_Id = a.LPMOEQ_Id,
                                                          LPMOEQOA_Id = f.LPMOEQOA_Id,
                                                          LPMOEQOA_Option = f.LPMOEQOA_Option,
                                                          LPMOEQOA_OptionCode = f.LPMOEQOA_OptionCode,
                                                          LPMOEQOA_AnswerFlag = f.LPMOEQOA_AnswerFlag,
                                                          LPMOEQOA_Marks = f.LPMOEQOA_Marks,
                                                      }).Distinct().OrderBy(a => a.LPMOEQ_Id).OrderBy(a => a.LPMOEQOA_Option).ToArray();


                        // Get Question Wise Option MF List 
                        data.getquestionmfoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                        from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                        from c in _context.LP_Master_OE_QNS_Options_MFDMO
                                                        from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                        from e in _context.LP_Master_OE_ExamDMO
                                                        from f in _context.LP_Master_OE_QuestionsDMO
                                                        where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id && f.LPMOEQ_Id == a.LPMOEQ_Id
                                                        && f.LPMOEQ_Id == b.LPMOEQ_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id && d.LPMOEEX_Id == e.LPMOEEX_Id
                                                        && a.LPMOEEXQNS_ActiveFlg == true && b.LPMOEQOA_ActiveFlg == true
                                                        && questionids.Contains(a.LPMOEQ_Id) && e.MI_Id == data.MI_Id && e.LPMOEEX_Id == data.LPMOEEX_Id
                                                        && f.LPMOEQ_MatchTheFollowingFlg == true)
                                                        select new LP_OnlineStudentExamDTO
                                                        {
                                                            LPMOEQ_Id = a.LPMOEQ_Id,
                                                            LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                            LPMOEQOAMF_Id = c.LPMOEQOAMF_Id,
                                                            LPMOEQOAMF_MatchtheFollowing = c.LPMOEQOAMF_MatchtheFollowing,
                                                            LPMOEQOAMF_AnswerFlag = c.LPMOEQOAMF_AnswerFlag,
                                                            LPMOEQOAMF_Order = c.LPMOEQOAMF_Order
                                                        }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();


                        // Get Subjective Uploaded File List By Student
                        data.getquestionsubjective_fileslist = (from a in _context.LP_Students_Exam_SubjectiveAnswer_FilesDMO
                                                                from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                from c in _context.LP_Students_ExamDMO
                                                                where (a.LPSTUEXSANS_Id == b.LPSTUEXSANS_Id && b.LPSTUEX_Id == c.LPSTUEX_Id
                                                                && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id
                                                                && c.LPMOEEX_Id == data.LPMOEEX_Id && questionids.Contains(b.LPMOEQ_Id))
                                                                select new LP_OnlineStudentExamDTO
                                                                {
                                                                    LPMOEQ_Id = b.LPMOEQ_Id,
                                                                    FileName = a.LPSTUEXSANSFL_FileName,
                                                                    FilePath = a.LPSTUEXSANSFNFL_FilePath,
                                                                    LPSTUEXSANS_Id = a.LPSTUEXSANS_Id,
                                                                }).Distinct().ToArray();

                        data.getquestionsubjective_staff_fileslist = (from a in _context.LP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO
                                                                      from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                      from c in _context.LP_Students_ExamDMO
                                                                      where (a.LPSTUEXSANS_Id == b.LPSTUEXSANS_Id && b.LPSTUEX_Id == c.LPSTUEX_Id
                                                                      && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id
                                                                      && c.LPMOEEX_Id == data.LPMOEEX_Id && questionids.Contains(b.LPMOEQ_Id))
                                                                      select new LP_OnlineStudentExamDTO
                                                                      {
                                                                          LPMOEQ_Id = b.LPMOEQ_Id,
                                                                          FileName = a.LPSTUEXSANSSFL_FileName,
                                                                          FilePath = a.LPSTUEXSANSSFL_FilePath,
                                                                          LPSTUEXSANS_Id = a.LPSTUEXSANS_Id,
                                                                      }).Distinct().ToArray();

                        //Get Student Wise Question Choose The Correct Answer Marks
                        data.get_examwise_ques_option_marks = (from a in _context.LP_Students_ExamDMO
                                                               from b in _context.LP_Students_Exam_AnswerDMO
                                                               from c in _context.LP_Master_OE_ExamDMO
                                                               where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.LPMOEEX_Id == c.LPMOEEX_Id && a.ASMAY_Id == data.ASMAY_Id
                                                               && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && c.LPMOEEX_Id == data.LPMOEEX_Id
                                                               && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id
                                                               && c.ISMS_Id == data.ISMS_Id && questionids.Contains(b.LPMOEQ_Id))
                                                               select new LP_OnlineStudentExamDTO
                                                               {
                                                                   LPMOEQ_Id = b.LPMOEQ_Id,
                                                                   LPSTUEXANS_Id = b.LPSTUEXANS_Id,
                                                                   LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                                   LPMOEQOAMF_Id = b.LPMOEQOAMF_Id,
                                                                   LPSTUEXANS_AttemptFlag = b.LPSTUEXANS_AttemptFlag,
                                                                   LPSTUEXANS_Marks = b.LPSTUEXANS_Marks,
                                                                   LPSTUEXANS_CorrectAnsFlg = b.LPSTUEXANS_CorrectAnsFlg,
                                                               }).Distinct().ToArray();

                        //Get Student Wise Question Subjective Marks
                        data.get_examwise_ques_subjective_marks = (from a in _context.LP_Students_ExamDMO
                                                                   from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                                   from c in _context.LP_Master_OE_ExamDMO
                                                                   where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.LPMOEEX_Id == c.LPMOEEX_Id && a.ASMAY_Id == data.ASMAY_Id
                                                                   && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && c.LPMOEEX_Id == data.LPMOEEX_Id
                                                                   && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id
                                                                   && c.ISMS_Id == data.ISMS_Id && questionids.Contains(b.LPMOEQ_Id))
                                                                   select new LP_OnlineStudentExamDTO
                                                                   {
                                                                       LPMOEQ_Id = b.LPMOEQ_Id,
                                                                       LPSTUEXSANS_Id = b.LPSTUEXSANS_Id,
                                                                       LPSTUEXANS_AttemptFlag = b.LPSTUEXANS_AttemptFlag,
                                                                       LPSTUEXSANS_Answer = b.LPSTUEXSANS_Answer,
                                                                       LPSTUEXANS_Marks = b.LPSTUEXSANS_Marks
                                                                   }).Distinct().ToArray();
                    }
                }
                else
                {
                    data.getstudentquesansdetails = (from a in _context.LP_Students_ExamDMO
                                                     from b in _context.LP_Students_Exam_AnswersheetDMO
                                                     where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                                     && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id)
                                                     select b).Distinct().ToArray();
                }

                data.getstudentquesansstaffdetails = (from a in _context.LP_Students_ExamDMO
                                                      from b in _context.LP_Students_Exam_Answersheet_StaffDMO
                                                      where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id
                                                      && a.AMST_Id == data.AMST_Id && a.LPMOEEX_Id == data.LPMOEEX_Id)
                                                      select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO SaveSubjectiveMarks(LP_OnlineStudentExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                decimal? totalsubjectivemarks = 0.00m;
                decimal? totalmaxsubjectivemarks = 0.00m;

                var getexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                var checkhrmeid = _context.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();
                long HRME_Id = 0;
                if (checkhrmeid.Count > 0)
                {
                    HRME_Id = checkhrmeid.FirstOrDefault().Emp_Code;
                }
                if (HRME_Id > 0)
                {
                    if ((data.savedetails != null && data.savedetails.Length > 0) || (data.savedetails_MCQ_MF != null && data.savedetails_MCQ_MF.Length > 0))
                    {
                        var getresultstudent = _context.LP_Students_ExamDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id
                        //&& a.AMST_Id == data.savedetails[0].AMST_Id
                        && a.ASMAY_Id == data.ASMAY_Id && a.LPSTUEX_Id == data.LPSTUEX_Id).ToList();

                        //var getsubjectivemarks = _context.LP_Students_Exam_SubjectiveAnswerDMO.Where(a => a.LPSTUEX_Id == getresultstudent.FirstOrDefault().LPSTUEX_Id).ToList();

                        //var getmcqmfmarks = _context.LP_Students_Exam_AnswerDMO.Where(a => a.LPSTUEX_Id == getresultstudent.FirstOrDefault().LPSTUEX_Id).ToList();

                        //decimal? perviousmarks = 0.00m;
                        //foreach (var c in getsubjectivemarks)
                        //{
                        //    perviousmarks += c.LPSTUEXSANS_Marks == null ? 0 : c.LPSTUEXSANS_Marks;
                        //}

                        //foreach (var c in getmcqmfmarks)
                        //{
                        //    perviousmarks += c.LPSTUEXANS_Marks == null ? 0 : c.LPSTUEXANS_Marks;
                        //}

                        //Subjective Marks
                        if (data.savedetails != null && data.savedetails.Length > 0)
                        {
                            foreach (var c in data.savedetails)
                            {
                                decimal? marks = c.LPSTUEXSANS_Marks;
                                decimal? LPMOEEXQNS_Marks = c.LPMOEEXQNS_Marks;
                                long amstid = c.AMST_Id;
                                long LPSTUEXSANS_Id = c.LPSTUEXSANS_Id;
                                long LPMOEQ_Id = c.LPMOEQ_Id;
                                long LPSTUEX_Id = c.LPSTUEX_Id;

                                totalsubjectivemarks += marks;
                                totalmaxsubjectivemarks += LPMOEEXQNS_Marks;

                                var checkresult = _context.LP_Students_Exam_SubjectiveAnswerDMO.Where(a => a.LPSTUEXSANS_Id == LPSTUEXSANS_Id).ToList();

                                if (checkresult.Count > 0)
                                {
                                    var result = _context.LP_Students_Exam_SubjectiveAnswerDMO.Single(a => a.LPSTUEXSANS_Id == LPSTUEXSANS_Id);
                                    result.LPSTUEXSANS_Marks = marks;
                                    result.LPSTUEXANS_UpdatedBy = data.UserId;
                                    result.LPSTUEXANS_UpdatedDate = indiantime0;
                                    result.HRME_Id = HRME_Id;
                                    _context.Update(result);
                                }

                                //Files Uploaded By Staff 
                                if (c.Temp_Staff_Ques_Subjective_Files != null && c.Temp_Staff_Ques_Subjective_Files.Length > 0)
                                {
                                    var staff_subjectfiles = _context.LP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO.Where(a => a.LPSTUEXSANS_Id == LPSTUEXSANS_Id).ToList();

                                    if (staff_subjectfiles != null && staff_subjectfiles.Count > 0)
                                    {
                                        foreach (var df in staff_subjectfiles)
                                        {
                                            _context.Remove(df);
                                        }
                                    }

                                    foreach (var sf in c.Temp_Staff_Ques_Subjective_Files)
                                    {
                                        LP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO lP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO = new LP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO
                                        {
                                            LPSTUEXSANS_Id = LPSTUEXSANS_Id,
                                            LPSTUEXSANSSFL_FileName = sf.LPSTUEXSANSSFL_FileName,
                                            LPSTUEXSANSSFL_FilePath = sf.LPSTUEXSANSSFL_FilePath,
                                            LPSTUEXSANSSFL_ActiveFlg = true,
                                            LPSTUEXSANSSFL_CreatedBy = data.UserId,
                                            LPSTUEXSANSSFL_CreatedDate = indiantime0,
                                            LPSTUEXSANSSFL_UpdatedBy = data.UserId,
                                            LPSTUEXSANSSFL_UpdatedDate = indiantime0
                                        };
                                        _context.Add(lP_Students_Exam_SubjectiveAnswer_Staff_FilesDMO);
                                    }
                                }
                            }
                        }

                        // MCQ OR MF Marks
                        if (data.savedetails_MCQ_MF != null && data.savedetails_MCQ_MF.Length > 0)
                        {
                            foreach (var c in data.savedetails_MCQ_MF)
                            {
                                decimal? marks = c.LPSTUEXANS_Marks;
                                decimal? LPMOEEXQNS_Marks = c.LPMOEEXQNS_Marks;
                                long amstid = c.AMST_Id;
                                long LPSTUEXANS_Id = c.LPSTUEXANS_Id;
                                long LPMOEQ_Id = c.LPMOEQ_Id;
                                long LPSTUEX_Id = c.LPSTUEX_Id;

                                totalsubjectivemarks += marks;
                                totalmaxsubjectivemarks += LPMOEEXQNS_Marks;

                                var checkresult = _context.LP_Students_Exam_AnswerDMO.Where(a => a.LPSTUEXANS_Id == LPSTUEXANS_Id).ToList();

                                if (checkresult.Count > 0)
                                {
                                    var result = _context.LP_Students_Exam_AnswerDMO.Single(a => a.LPSTUEXANS_Id == LPSTUEXANS_Id);
                                    result.LPSTUEXANS_Marks = marks;
                                    result.LPSTUEXANS_UpdatedBy = data.UserId;
                                    result.UpdatedDate = indiantime0;
                                    _context.Update(result);
                                }
                            }
                        }


                        var getobjectivecount = (from a in _context.LP_Students_ExamDMO
                                                 from b in _context.LP_Students_Exam_AnswerDMO
                                                 where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && a.ASMAY_Id == data.ASMAY_Id
                                                 && a.LPSTUEX_Id == data.LPSTUEX_Id)
                                                 select b).Distinct().ToList();

                        var getsubjectivecount = (from a in _context.LP_Students_ExamDMO
                                                  from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                                  where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && a.ASMAY_Id == data.ASMAY_Id
                                                  && a.LPSTUEX_Id == data.LPSTUEX_Id)
                                                  select b).Distinct().ToList();

                        var resultstudent = _context.LP_Students_ExamDMO.Single(a => a.LPMOEEX_Id == data.LPMOEEX_Id && a.LPSTUEX_Id == data.LPSTUEX_Id
                        && a.ASMAY_Id == data.ASMAY_Id);

                        decimal? totalmarks = 0.00m;
                        decimal? totalmaxmarks = 0.00m;
                        decimal? totalpercentage = 0.00m;

                        //totalmarks = (resultstudent.LPSTUEX_TotalMarks - perviousmarks) + totalsubjectivemarks;
                        totalmarks = totalsubjectivemarks;

                        totalmaxmarks = resultstudent.LPSTUEX_TotalMaxMarks;

                        totalpercentage = (totalmarks / totalmaxmarks) * 100;
                        totalpercentage = Math.Round(Convert.ToDecimal(totalpercentage), 0, MidpointRounding.AwayFromZero);

                        resultstudent.LPSTUEX_TotalMarks = totalmarks;
                        resultstudent.LPSTUEX_TotalMaxMarks = totalmaxmarks;
                        resultstudent.LPSTUEX_Percentage = totalpercentage;
                        resultstudent.LPSTUEX_UpdatedBy = data.UserId;
                        resultstudent.UpdatedDate = indiantime0;
                        resultstudent.LPSTUEX_TotalQnsAnswered = getobjectivecount.Count() + getsubjectivecount.Count();

                        _context.Update(resultstudent);

                        var i = _context.SaveChanges();

                        if (i > 0)
                        {
                            data.message = "Add";
                        }
                        else
                        {
                            data.message = "Error";
                        }
                    }
                }
                else
                {
                    data.message = "Emp";
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO GetStudentListForPublish(LP_OnlineStudentExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                string order = "AMST_FirstName";
                var get_configuration = _context.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration != null && get_configuration.Count > 0)
                {
                    if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                    {
                        order = "AMST_FirstName";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                    {
                        order = "AMST_AdmNo";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                    {
                        order = "AMAY_RollNo";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                    {
                        order = "AMST_RegistrationNo";
                    }
                    else
                    {
                        order = "AMST_FirstName";
                    }
                }

                var studentList = (from a in _context.LP_Students_ExamDMO
                                   from b in _context.LP_Master_OE_ExamDMO
                                   from c in _context.School_Adm_Y_StudentDMO
                                   from d in _context.Adm_M_Student
                                   from e in _context.AcademicYear
                                   from f in _context.AdmissionClass
                                   from g in _context.School_M_Section
                                   where (a.LPMOEEX_Id == b.LPMOEEX_Id && a.AMST_Id == c.AMST_Id && c.AMST_Id == d.AMST_Id
                                   && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.ASMS_Id == g.ASMS_Id
                                   && c.ASMAY_Id == e.ASMAY_Id && c.ASMCL_Id == f.ASMCL_Id && c.ASMS_Id == g.ASMS_Id
                                   && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id
                                   && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id && c.ASMS_Id == data.ASMS_Id
                                   && a.ASMAY_Id == data.ASMAY_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && b.ISMS_Id == data.ISMS_Id)
                                   select new LP_OnlineStudentExamDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       AMST_FirstName = ((d.AMST_FirstName == null ? "" : d.AMST_FirstName) +
                                       (d.AMST_MiddleName == null || d.AMST_MiddleName == "" ? "" : " " + d.AMST_MiddleName) +
                                       (d.AMST_LastName == null || d.AMST_LastName == "" ? "" : " " + d.AMST_LastName)).Trim(),
                                       AMST_AdmNo = d.AMST_AdmNo == null || d.AMST_AdmNo == "" ? "" : d.AMST_AdmNo,
                                       AMST_RegistrationNo = d.AMST_RegistrationNo == null || d.AMST_RegistrationNo == "" ? "" : d.AMST_RegistrationNo,
                                       AMAY_RollNo = c.AMAY_RollNo,
                                       LPSTUEX_Id = a.LPSTUEX_Id,
                                       LPSTUEX_PublishToStudent = a.LPSTUEX_PublishToStudent,
                                       LPSTUEX_TotalMarks = a.LPSTUEX_TotalMarks,
                                   }).Distinct().OrderBy(a => order).ToList();

                var propertyInfo = typeof(LP_OnlineStudentExamDTO).GetProperty(order);
                data.getstudentlistpublish = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToArray();

            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO CheckStudentMarksEntered(LP_OnlineStudentExamDTO data)
        {
            try
            {
                var getexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                var checkexamflag = getexamdetails.FirstOrDefault().LPMOEEX_UploadExamPaperFlg;

                long countpublish = 0;
                if (checkexamflag == false)
                {
                    var checkcountmarks = (from a in _context.LP_Students_ExamDMO
                                           from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                                           where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                           && a.LPSTUEX_Id == data.LPSTUEX_Id && b.LPSTUEXSANS_Marks == null && a.AMST_Id == data.AMST_Id)
                                           select b).Distinct().ToList();

                    if (checkcountmarks.Count > 0)
                    {
                        countpublish = 1;
                    }
                }
                else
                {
                    var checkcountmarks = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.LPMOEEX_Id == data.LPMOEEX_Id && a.LPSTUEX_TotalMarks == null && a.AMST_Id == data.AMST_Id).ToList();

                    if (checkcountmarks.Count() > 0)
                    {
                        countpublish = 1;
                    }
                }

                if (countpublish == 0)
                {
                    data.message = "MarksCalculated";
                }
                else
                {
                    data.message = "MarksNotCalculated";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO PublishToStudent(LP_OnlineStudentExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkhrmeid = _context.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();
                long HRME_Id = 0;
                if (checkhrmeid.Count > 0)
                {
                    HRME_Id = checkhrmeid.FirstOrDefault().Emp_Code;
                }

                var getexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                var checkexamflag = getexamdetails.FirstOrDefault().LPMOEEX_UploadExamPaperFlg;

                if (data.selectedstudetntspublish != null && data.selectedstudetntspublish.Length > 0)
                {
                    foreach (var c in data.selectedstudetntspublish)
                    {
                        var result = _context.LP_Students_ExamDMO.Single(a => a.LPSTUEX_Id == c.LPSTUEX_Id);
                        result.LPSTUEX_PublishToStudent = result.LPSTUEX_PublishToStudent == true ? false : true;
                        result.UpdatedDate = indiantime0;
                        result.LPSTUEX_UpdatedBy = data.UserId;
                        _context.Update(result);
                    }
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.message = "Update";
                    }
                    else
                    {
                        data.message = "Failed";
                    }
                }

                // long countpublish = 0;
                //if (checkexamflag == false)
                //{
                //    var checksubjectivequestions = (from a in _context.LP_Master_OE_ExamDMO
                //                                    from b in _context.LP_Master_OE_Exam_QuestionsDMO
                //                                    from c in _context.LP_Master_OE_QuestionsDMO
                //                                    from d in _context.LP_Master_OE_Exam_LevelsDMO
                //                                    where (a.LPMOEEX_Id == d.LPMOEEX_Id && d.LPMOEEXLVL_Id == b.LPMOEEXLVL_Id && b.LPMOEQ_Id == c.LPMOEQ_Id
                //                                    && a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && d.LPMOEEX_Id == data.LPMOEEX_Id
                //                                    && b.LPMOEEXQNS_ActiveFlg == true && c.LPMOEQ_SubjectiveFlg == true && a.ASMAY_Id == data.ASMAY_Id)
                //                                    select c).Distinct().ToList();


                //    if (checksubjectivequestions.Count > 0)
                //    {
                //        var checkcountmarks = (from a in _context.LP_Students_ExamDMO
                //                               from b in _context.LP_Students_Exam_SubjectiveAnswerDMO
                //                               where (a.LPSTUEX_Id == b.LPSTUEX_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                //                               && a.LPSTUEX_Id == data.LPSTUEX_Id && b.LPSTUEXSANS_Marks == null)
                //                               select b).Distinct().ToList();

                //        if (checkcountmarks.Count > 0)
                //        {
                //            countpublish = 1;
                //        }
                //    }
                //}
                //else
                //{
                //    var checkcountmarks = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                //    && a.LPMOEEX_Id == data.LPMOEEX_Id && a.LPSTUEX_TotalMarks == null).ToList();

                //    if (checkcountmarks.Count() > 0)
                //    {
                //        countpublish = 1;
                //    }
                //}

                //if (countpublish == 0)
                //{
                //    var outputval = _context.Database.ExecuteSqlCommand("LP_OnlineExam_PublishToStudent @p0,@p1,@p2,@p3,@p4,@p5,@p6",
                //        data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id, data.ISMS_Id, data.LPMOEEX_Id, data.UserId);

                //    if (outputval > 0)
                //    {
                //        data.message = "Update";
                //    }
                //    else
                //    {
                //        data.message = "Failed";
                //    }
                //}
                //else
                //{
                //    data.message = "MarksNotCalculated";
                //}
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Pushing Online Exam Marks To Master Exam Marks      
        public LP_OnlineStudentExamDTO GetExam_OE_StudentList(LP_OnlineStudentExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkhrmeid = _context.Staff_User_Login.Where(a => a.Id == data.UserId).ToList();
                long HRME_Id = 0;
                if (checkhrmeid.Count > 0)
                {
                    HRME_Id = checkhrmeid.FirstOrDefault().Emp_Code;
                }

                var catid = _context.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id
                && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _context.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EYC_ActiveFlg == true
                && catid.Contains(t.EMCA_Id)).Select(t => t.EYC_Id).ToArray();

                var eyceid = _context.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true
                && t.EME_Id == data.EME_Id).Select(t => t.EYCE_Id).ToArray();

                var get_yearly_exam_subject_details = _context.Exm_Yrly_Cat_Exams_SubwiseDMO.Where(a => a.ISMS_Id == data.ISMS_Id && eyceid.Contains(a.EYCE_Id)
                && a.EYCES_ActiveFlg == true).Distinct().ToList();

                data.get_yearly_exam_subject_details = get_yearly_exam_subject_details.ToArray();


                string order = "";
                var get_configuration = _context.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                data.configuration = get_configuration.ToArray();

                if (get_configuration.Count > 0)
                {
                    if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                    {
                        order = "AMST_FirstName";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                    {
                        order = "AMST_AdmNo";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                    {
                        order = "AMAY_RollNo";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                    {
                        order = "AMST_RegistrationNo";
                    }
                    else
                    {
                        order = "AMST_FirstName";
                    }
                }
                else
                {
                    order = "AMST_FirstName";
                }

                var studentList = (from a in _context.Adm_M_Student
                                   from b in _context.School_Adm_Y_StudentDMO
                                   where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id
                                   && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && a.AMST_ActiveFlag == 1 && a.MI_Id == data.MI_Id)
                                   select new LP_OnlineStudentExamDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                       (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                       (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName)).Trim(),
                                       AMST_AdmNo = a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : a.AMST_AdmNo,
                                       AMST_RegistrationNo = a.AMST_RegistrationNo == null || a.AMST_RegistrationNo == "" ? "" : a.AMST_RegistrationNo,
                                       AMAY_RollNo = b.AMAY_RollNo,
                                   }).Distinct().OrderBy(a => order).ToList();

                var propertyInfo = typeof(LP_OnlineStudentExamDTO).GetProperty(order);
                data.studentdetails = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToArray();

                data.get_lpoe_studentmarks = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.LPMOEEX_Id == data.LPMOEEX_Id).Distinct().ToArray();

                data.get_exam_studentmarks = _context.ExamMarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id).Distinct().ToArray();

                var EMGR_Id = get_yearly_exam_subject_details.FirstOrDefault().EMGR_Id;

                if (get_yearly_exam_subject_details.FirstOrDefault().EYCES_MarksGradeEntryFlg == "G")
                {
                    data.grade_details = (from a in _context.Exm_Master_GradeDMO
                                          from b in _context.Exm_Master_Grade_DetailsDMO
                                          where (a.MI_Id == data.MI_Id && a.EMGR_Id == EMGR_Id && b.EMGR_Id == EMGR_Id && b.EMGD_ActiveFlag == true)
                                          select b).Select(t => t.EMGD_Name).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineStudentExamDTO SaveOE_Marks_ME(LP_OnlineStudentExamDTO data)
        {
            try
            {

                //string Str = "";
                //Str = System.Net.Dns.GetHostName();
                //IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(Str);
                //IPAddress[] addr = ipEntry.AddressList;
                //data.IP4 = addr[addr.Length - 1].ToString();

                // data.IP4 = Convert.ToString(ipEntry.AddressList.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork));


                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                foreach (var d in data.main_save_list)
                {
                    var checkresult = _context.ExamMarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == d.AMST_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id).ToList();

                    if (checkresult.Count > 0)
                    {
                        var result = _context.ExamMarksDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                     && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == d.AMST_Id && a.EME_Id == data.EME_Id && a.ISMS_Id == data.ISMS_Id
                     && a.ESTM_Id == checkresult.FirstOrDefault().ESTM_Id);
                        result.ESTM_Grade = d.ESTM_Grade;
                        result.ESTM_Marks = d.ESTM_Marks;
                        result.ESTM_Flg = d.ESTM_Flg;
                        result.ESTM_MarksGradeFlg = d.ESTM_MarksGradeFlg;
                        result.UpdatedDate = indiantime0;
                        result.ESTM_UpdatedBy = data.UserId;
                        _context.Update(result);
                    }
                    else
                    {
                        ExamMarksDMO obj_M = new ExamMarksDMO();
                        obj_M.MI_Id = data.MI_Id;
                        obj_M.ASMAY_Id = data.ASMAY_Id;
                        obj_M.ASMCL_Id = data.ASMCL_Id;
                        obj_M.ASMS_Id = data.ASMS_Id;
                        obj_M.EME_Id = Convert.ToInt32(data.EME_Id);
                        obj_M.ISMS_Id = Convert.ToInt64(data.ISMS_Id);
                        obj_M.AMST_Id = d.AMST_Id;
                        obj_M.ESTM_Marks = d.ESTM_Marks;
                        obj_M.ESTM_MarksGradeFlg = d.ESTM_MarksGradeFlg;
                        obj_M.Id = data.UserId;
                        obj_M.LoginDateTime = indiantime0;
                        obj_M.IP4 = data.IP4;
                        obj_M.CreatedDate = indiantime0;
                        obj_M.UpdatedDate = indiantime0;
                        obj_M.ESTM_ActiveFlg = true;
                        obj_M.ESTM_Grade = d.ESTM_Grade;
                        obj_M.ESTM_Flg = d.ESTM_Flg;
                        obj_M.ESTM_OnlineExamFlag = true;
                        obj_M.ESTM_CreatedBy = data.UserId;
                        obj_M.ESTM_UpdatedBy = data.UserId;
                        _context.Add(obj_M);
                    }
                }

                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.message = "Save";
                }
                else
                {
                    data.message = "Failed";
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //online exam not submitted list report
        public LP_OnlineStudentExamDTO getNonSubmittedreport(LP_OnlineStudentExamDTO data)
        {
            try
            {
                DateTime fromdate = new DateTime();
                string confromdate = "";

                fromdate = Convert.ToDateTime(data.fromdate.Value.Date.ToString("yyyy-MM-dd"));
                confromdate = fromdate.ToString("yyyy-MM-dd");

                DateTime todate = new DateTime();
                string contodate = "";

                todate = Convert.ToDateTime(data.todate.Value.Date.ToString("yyyy-MM-dd"));
                contodate = todate.ToString("yyyy-MM-dd");

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LP_OnlineExamStudentsNotSubmittedList";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@LPMOEEX_Id", SqlDbType.VarChar) { Value = data.LPMOEEX_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LP_OnlineStudentExamDTO MergeFiles(LP_OnlineStudentExamDTO data)
        {
            try
            {
                if (data.MergeFilesDTO != null && data.MergeFilesDTO.Length > 0)
                {
                    string wwwPath = _hostingEnvironment.WebRootPath;
                    string contentPath = _hostingEnvironment.ContentRootPath;

                    string pathname = "UploadImages/" + data.AMST_Id;

                    string path = Path.Combine(_hostingEnvironment.WebRootPath, pathname);
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }

                    Directory.CreateDirectory(path);

                    string accountname = "";
                    string accesskey = "";

                    var dataq = _context.IVRM_Storage_path_Details.ToList();
                    if (dataq.Count() > 0)
                    {
                        accountname = dataq.FirstOrDefault().IVRM_SD_Access_Name;
                        accesskey = dataq.FirstOrDefault().IVRM_SD_Access_Key;
                    }
                    string target = path;

                    foreach (var d in data.MergeFilesDTO)
                    {
                        //var c = DownloadAsync(d.FilePath, d.FileName, target, accountname, accesskey);
                        var c = DownloadFile(d.FilePath, d.FileName);
                        //CloudBlockBlob blockBlob;
                        //using (MemoryStream memoryStream = new MemoryStream())
                        //{
                        //    var storageCredentials = new StorageCredentials(accountname, accesskey);
                        //    CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
                        //    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                        //    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("files");
                        //    blockBlob = cloudBlobContainer.GetBlockBlobReference(d.FilePath);
                        //    await blockBlob.DownloadToStreamAsync(memoryStream);
                        //    var s = blockBlob.DownloadToFileAsync(target + "/" + d.FileName, FileMode.OpenOrCreate);
                        //}



                        //var storageCredentials = new StorageCredentials(accountname, accesskey);
                        //var csa = new CloudStorageAccount(storageCredentials, true);
                        //CloudBlobClient blobClient = csa.CreateCloudBlobClient();
                        //CloudBlobContainer container = blobClient.GetContainerReference("files");
                        //CloudBlockBlob blockBlob = container.GetBlockBlobReference(d.FilePath);
                        //string pathd = (target + "/" + d.FileName);
                        ////var s = blockBlob.DownloadToStreamAsync(pathd, FileMode.OpenOrCreate);
                        ////var s = blockBlob.DownloadToStreamAsync(System.IO.File.OpenWrite(pathd));

                        //using (var fileStream = System.IO.File.OpenWrite(pathd))
                        //{
                        //    await blockBlob.DownloadToStreamAsync(fileStream);
                        //}

                        //string fileExt = "";
                        //if (d.FileType == "jpeg")
                        //{
                        //    fileExt = "image/jpeg";
                        //}
                        //if (d.FileType  == "jpg")
                        //{
                        //    fileExt = "image/jpg";
                        //}
                        //if (d.FileType  == "pdf")
                        //{
                        //    fileExt = "application/pdf";
                        //}                        
                        //else if (d.FileType  == "png")
                        //{
                        //    fileExt = "image/png";
                        //}
                        //else if (d.FileType  == "mp4")
                        //{
                        //    fileExt = "video/mp4";
                        //}
                        //else if (d.FileType  == "mp3")
                        //{
                        //    fileExt = "audio/mp3";
                        //}                       
                        //else if (d.FileType  == "wmv")
                        //{
                        //    fileExt = "video/x-ms-wmv";
                        //}
                        //else if (d.FileType  == "xls")
                        //{
                        //    fileExt = "application/vnd.ms-excel";
                        //}
                        //else if (d.FileType  == "xlsx")
                        //{
                        //    fileExt = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        //}
                        //else if (d.FileType  == "doc")
                        //{
                        //    fileExt = "application/msword";
                        //}
                        //else if (d.FileType  == "docx")
                        //{
                        //    fileExt = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        //}
                        //else if (d.FileType  == "ppt")
                        //{
                        //    fileExt = "application/vnd.ms-powerpoint";
                        //}
                        //else if (d.FileType  == "pptx")
                        //{
                        //    fileExt = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                        //}
                        //else if (d.FileType  == "ppsx")
                        //{
                        //    fileExt = "application/vnd.openxmlformats-officedocument.presentationml.slideshow";
                        //}

                        //CloudBlockBlob blockBlob;
                        //using (MemoryStream memoryStream = new MemoryStream())
                        //{
                        //    var storageCredentials = new StorageCredentials(accountname, accesskey);
                        //    CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials,true);
                        //    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                        //    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("files");
                        //    blockBlob = cloudBlobContainer.GetBlockBlobReference(d.FilePath);
                        //    await blockBlob.DownloadToStreamAsync(memoryStream);
                        //}

                        //Stream blobStream = blockBlob.OpenReadAsync().Result;
                        //return File(blobStream, fileExt, d.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task DownloadAsync(string filepath, string filename, string target, string accountname, string accesskey)
        {
            var storageCredentials = new StorageCredentials(accountname, accesskey);
            var csa = new CloudStorageAccount(storageCredentials, true);

            //CloudBlobClient blobClient = csa.CreateCloudBlobClient();
            //CloudBlobContainer container = blobClient.GetContainerReference("files");
            //CloudBlockBlob blockBlob = container.GetBlockBlobReference(d.FilePath);
            //string pathd = (target + "/" + d.FileName);
            ////var s = blockBlob.DownloadToStreamAsync(pathd, FileMode.OpenOrCreate);
            ////var s = blockBlob.DownloadToStreamAsync(System.IO.File.OpenWrite(pathd));

            //using (var fileStream = System.IO.File.OpenWrite(pathd))
            //{
            //    await blockBlob.DownloadToStreamAsync(fileStream);
            //}

            //CloudBlobClient myBlob = csa.CreateCloudBlobClient();

            string[] filepatharray = filepath.Split("/");

            filepath = filepatharray[filepatharray.Length - 1];

            //CloudBlobContainer mycontainer = myBlob.GetContainerReference("files");
            //CloudBlockBlob myBlockBlob = mycontainer.GetBlockBlobReference(filepath);
            //Stream fileupd = File.OpenWrite(target+"/"+ filepath);
            //// var c = myBlockBlob.DownloadToStreamAsync(fileupd);

            //using (var fileStream = System.IO.File.OpenWrite(target + "/" + filepath))
            //{
            //    await myBlockBlob.DownloadToStreamAsync(fileStream);
            //}

            CloudBlobClient blobClient = csa.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("files");
            CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filepath);

            // provide the file download location below            
            Stream file = File.OpenWrite(target + "/" + filepath);
            cloudBlockBlob.DownloadToStreamAsync(file);
        }
        public async Task DownloadFile(string filepath, string filenamed)
        {
            string accountname = "";
            string accesskey = "";

            var fileName = string.Empty;
            MemoryStream ms = new MemoryStream();
            var data = _context.IVRM_Storage_path_Details.ToList();
            if (data.Count() > 0)
            {
                accountname = data.FirstOrDefault().IVRM_SD_Access_Name;
                accesskey = data.FirstOrDefault().IVRM_SD_Access_Key;
            }

            string[] filepatharray = filepath.Split("/");

            filepath = "7/LessonPlanner/" + filepatharray[filepatharray.Length - 1];

            StorageCredentials cre = new StorageCredentials(accountname, accesskey);
            CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
            CloudBlobClient blobClient = acc.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("files");
            if (await container.ExistsAsync())
            {
                CloudBlob file = container.GetBlobReference(filepath);
                if (await file.ExistsAsync())
                {
                    var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages");
                    fileName = filenamed;
                    fileName = Path.Combine(_hostingEnvironment.WebRootPath, "UploadImages") + $@"\{fileName}";
                    file.DownloadToFileAsync(fileName, FileMode.CreateNew);
                }
            }
        }
        public void imgtopdfd()
        {
            var exePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);

            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            //appRoot = appRoot.Replace("WebApplication1", "IVRMUX");
            var uploads = Path.Combine(appRoot, "wwwroot\\UploadImages");
            string ORIG = "google.PNG";
            string OUTPUT_FOLDER = "wwwroot\\UploadImages\\";
            OUTPUT_FOLDER = Path.Combine(appRoot, OUTPUT_FOLDER);

            ORIG = Path.Combine(appRoot, uploads) + $@"\{ORIG}";

            PdfDocument pdfDocument = new PdfDocument(new PdfWriter(OUTPUT_FOLDER + "ImageToPdf.pdf"));
            Document document = new Document(pdfDocument);

            ImageData imageData = ImageDataFactory.Create(ORIG);
            Image image = new Image(imageData);
            image.SetWidth(pdfDocument.GetDefaultPageSize().GetWidth() - 50);
            image.SetAutoScaleHeight(true);

            document.Add(image);
            pdfDocument.Close();
        }
    }
}