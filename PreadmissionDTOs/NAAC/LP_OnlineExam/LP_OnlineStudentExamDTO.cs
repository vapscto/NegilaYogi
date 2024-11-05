using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.LP_OnlineExam
{
    public class LP_OnlineStudentExamDTO
    {
        public long LPSTUEX_Id { get; set; }
        public long LPMOEEXQNS_Id { get; set; }
        public long viewmarkscount { get; set; }
        public long viewmarkscountnew { get; set; }
        public long LPMOEEX_Id { get; set; }
        public long? LPMOEQ_Id { get; set; }
        public string LPMOEQ_Question { get; set; }
        public string LPMOEQ_Answer { get; set; }
        public decimal? LPMOEQ_Marks { get; set; }
        public string LPMOEEX_ExamName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string LPMOEEX_ExamDuration { get; set; }
        public DateTime? ExamStartDateTime { get; set; }
        public DateTime? ExamEndDateTime { get; set; }
        public DateTime? AMST_Date { get; set; }
        public long UserId { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long? ISMS_Id { get; set; }
        public int? EME_Id { get; set; }
        public DateTime? LPSTUEX_Date { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public string LPSTUEX_TotalTime { get; set; }
        public string LPSTUEX_AnswerSheetPath { get; set; }
        public string LPSTUEX_AnswerSheetFile { get; set; }
        public long LPSTUEX_TotalQnsAnswered { get; set; }
        public long LPSTUEX_TotalCorrectAns { get; set; }
        public decimal? LPSTUEX_TotalMaxMarks { get; set; }
        public decimal? LPMOEEX_TotalMarks { get; set; }
        public decimal? LPSTUEX_TotalMarks { get; set; }
        public decimal? LPSTUEX_Percentage { get; set; }
        public Array getsubjectlist { get; set; }
        public Array getsectionlist { get; set; }
        public Array getexamlist { get; set; }
        public Array getsubjectdetails { get; set; }
        public Array getexamquestionlist { get; set; }
        public Array getquestionoptionlist { get; set; }
        public Array getquestiondoclist { get; set; }
        public Array getconnfig { get; set; }
        public Array getexamdetails { get; set; }
        public Array getsavedanswer { get; set; }
        public Array getyearlist { get; set; }
        public Array getclasslist { get; set; }
        public Array getstudentquesansdetails { get; set; }
        public Array getstudentquesansstaffdetails { get; set; }
        public Array getstudentquesansstaffdetailsview { get; set; }
        public Array getquestionmfoptionlist { get; set; }
        public Array result { get; set; }
        public Array getlpexamdetails { get; set; }
        public Array getmasterexamdetails { get; set; }
        public string message { get; set; }
        public long? LPMOEQOA_Id { get; set; }
        public long? LPMOEEXLVL_Id { get; set; }
        public long? LPMOEEXQNS_QnsOrder { get; set; }
        public string LPMOEQOA_Option { get; set; }
        public string LPMOEQOA_OptionCode { get; set; }
        public bool? LPMOEQOA_AnswerFlag { get; set; }
        public bool? LPMOEQ_SubjectiveFlg { get; set; }
        public bool? LPMOEQ_MatchTheFollowingFlg { get; set; }
        public bool LPMOEEX_UploadExamPaperFlg { get; set; }
        public string LPMOEEX_QuestionPaper { get; set; }
        public string LPMOEEX_QuestionPapeFileName { get; set; }
        public string LPMOEEX_AnswerSheet { get; set; }
        public string LPMOEEX_AnswerPapeFileName { get; set; }
        public string LPMOEQ_StructuralFlg { get; set; }
        public long LPMOEQF_Id { get; set; }
        public string LPMOEQF_FileName { get; set; }
        public string LPMOEQF_FilePath { get; set; }
        public string LPSTUEXSANS_Answer { get; set; }
        public string Flag { get; set; }
        public saveanswerlsttemp[] saveanswerlsttemp { get; set; }
        public savemarks[] savemarks { get; set; }
        public savedetails[] savedetails { get; set; }
        public savedetails_MCQ_MF[] savedetails_MCQ_MF { get; set; }
        public SaveAnswerSheet[] SaveAnswerSheet { get; set; }
        public Array getallexamdetails { get; set; }
        public Array gettodaysexamdetails { get; set; }
        public Array getexamcompleteddetails { get; set; }
        public Array getexamsubmitteddetails { get; set; }
        public Array getmarksdetails { get; set; }
        public Array getallmarksdetails { get; set; }
        public Array getstudentdetails { get; set; }
        public Array getstudentlist { get; set; }
        public LP_OnlineFinalDetails[] LP_OnlineFinalDetails { get; set; }
        public Array getexamleveldetails { get; set; }
        public Array getoptionwisefiles { get; set; }
        public string LPMOEQOAF_FileName { get; set; }
        public string LPMOEQOAF_FilePath { get; set; }
        public string studentname { get; set; }
        public string admno { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string yearname { get; set; }
        public Array configuration { get; set; }
        public Array studentdetails { get; set; }
        public Array get_lpoe_studentmarks { get; set; }
        public Array get_exam_studentmarks { get; set; }
        public Array get_yearly_examdetails { get; set; }
        public Array get_yearly_exam_subject_details { get; set; }
        public Array grade_details { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string IP4 { get; set; }
        public long AMAY_RollNo { get; set; }
        public bool? LPSTUEX_PublishToStudent { get; set; }
        public main_save_list[] main_save_list { get; set; }
        public missingfiles[] missingfiles { get; set; }
        public correctedanswerfiles[] correctedanswerfiles { get; set; }
        public selectedstudetntspublish[] selectedstudetntspublish { get; set; }
        public Array getstudentlistpublish { get; set; }
        public Array getexamwise_mfquestions { get; set; }
        public Array getexamwise_mfques_options { get; set; }
        public Array getexamwise_ques_options_mf { get; set; }
        public Array getdetails { get; set; }
        public Array getexamwise_ques_options_mf_marks { get; set; }
        public bool? LPMOEEX_NotLinkedToQnsBankFlg { get; set; }
        public bool? LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg { get; set; }
        public bool LPSTUEXANS_CorrectAnsFlg { get; set; }
        public string LPMOEEX_Duration { get; set; }
        public string LPMOEEX_DurationFlag { get; set; }
        public long? LPMOEQOAMF_Id { get; set; }
        public string LPMOEQOAMF_MatchtheFollowing { get; set; }
        public bool? LPMOEQOAMF_AnswerFlag { get; set; }
        public int? LPMOEQOAMF_Order { get; set; }
        public string LPSTUEXAS_AnswerSheetFile { get; set; }
        public string LPSTUEXAS_AnswerSheetPath { get; set; }
        public string LPSTUEXAS_StaffOrStudentUploadFlag { get; set; }
        public long LPSTUEXAS_Id { get; set; }
        public long LPSTUEXANS_Id { get; set; }
        public long? LPMOEEXQNSOPTMF_Id { get; set; }
        public decimal? LPMOEQOA_Marks { get; set; }
        public decimal? LPSTUEXANS_Marks { get; set; }
        public string LPMOEQOA_AnswerDesc { get; set; }
        public string LPSTUEXANS_AttemptFlag { get; set; }
        public MergeFilesDTO[] MergeFilesDTO { get; set; }
        public string LPSTUEXSANS_FilePath { get; set; }
        public string LPSTUEXSANS_FileName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long LPSTUEXSANS_Id { get; set; }
        public Array getquestionsubjective_fileslist { get; set; }
        public Array getquestionsubjective_staff_fileslist { get; set; }
        public Array get_examwise_ques_option_marks { get; set; }
        public Array get_examwise_ques_subjective_marks { get; set; }
        public Array get_student_exam_details { get; set; }
        public Array get_stutdent_view_subjectfiles_list { get; set; }
    }
    public class saveanswerlsttemp
    {
        public long LPMOEQ_Id { get; set; }
        public long LPMOEQOA_Id { get; set; }
        public long QuizeQuastions { get; set; }
        public string LPMOEQ_Question { get; set; }
        public bool LPMOEQOA_AnswerFlag { get; set; }
    }

    public class savemarks
    {
        public long AMST_Id { get; set; }
        public decimal? marks { get; set; }
        public long LPSTUEX_Id { get; set; }
        public string LPSTUEX_CorrectedAnswerSheetPath { get; set; }
        public string LPSTUEX_CorrectedAnswerSheetFile { get; set; }
       
    }

    public class savedetails
    {
        public long AMST_Id { get; set; }
        public decimal? LPSTUEXSANS_Marks { get; set; }
        public decimal? LPMOEEXQNS_Marks { get; set; }
        public decimal? LPSTUEX_TotalMaxMarks { get; set; }
        public decimal? LPSTUEX_TotalMarks { get; set; }
        public long LPSTUEX_Id { get; set; }
        public long LPSTUEXSANS_Id { get; set; }
        public long LPMOEQ_Id { get; set; }
        public Temp_Staff_Ques_Subjective_Files[] Temp_Staff_Ques_Subjective_Files { get; set; }
    }

    public class savedetails_MCQ_MF
    {
        public long AMST_Id { get; set; }
        public decimal? LPSTUEXANS_Marks { get; set; }
        public decimal? LPMOEEXQNS_Marks { get; set; }
        public decimal? LPSTUEX_TotalMaxMarks { get; set; }
        public decimal? LPSTUEX_TotalMarks { get; set; }
        public long LPSTUEX_Id { get; set; }
        public long LPSTUEXANS_Id { get; set; }
        public long LPMOEQ_Id { get; set; }       
    }
    public class SaveAnswerSheet
    {
        public long LPSTUEXAS_Id { get; set; }
        public long LPSTUEX_Id { get; set; }
        public string LPSTUEXAS_AnswerSheetFile { get; set; }
        public string LPSTUEXAS_AnswerSheetPath { get; set; }
    }
    public class LP_OnlineFinalDetails
    {
        public long? LPMOEQ_Id { get; set; }
        public long? QuizeQuastions { get; set; }
        public string LPMOEQ_Question { get; set; }
        public bool LPMOEQOA_AnswerFlag { get; set; }
        public bool LPMOEQ_SubjectiveFlg { get; set; }
        public bool? LPMOEQ_MatchTheFollowingFlg { get; set; }
        public string answer { get; set; }
        public string LPSTUEXANS_AttemptFlag { get; set; }
        public string LPSTUEXSANS_FilePath { get; set; }
        public string LPSTUEXSANS_FileName { get; set; }
        public LP_OnlineFinalDetails_MF[] LP_OnlineFinalDetails_MF { get; set; }
        public Temp_Ques_Subjective_Files[] Temp_Ques_Subjective_Files { get; set; }
    }
    public class LP_OnlineFinalDetails_MF
    {
        public long? LPMOEQ_Id { get; set; }
        public long? LPMOEQOA_Id { get; set; }
        public long? QuizeQuastions { get; set; }
        public string LPMOEQ_Question { get; set; }
        public bool LPMOEQOA_AnswerFlag { get; set; }
        public bool LPMOEQ_SubjectiveFlg { get; set; }
        public bool? LPMOEQ_MatchTheFollowingFlg { get; set; }
        public string answer { get; set; }
        public string LPSTUEXANS_AttemptFlag { get; set; }
    }
    public class Temp_Ques_Subjective_Files
    {
        public long? LPMOEQ_Id { get; set; }
        public long LPSTUEXSANSFL_Id { get; set; }
        public long LPSTUEXSANS_Id { get; set; }
        public string LPSTUEXSANSFL_FileName { get; set; }
        public string LPSTUEXSANSFNFL_FilePath { get; set; }
    }
    public class main_save_list
    {
        public long AMST_Id { set; get; }         
        public long ISMS_Id { set; get; }         
        public decimal ESTM_Marks { get; set; }
        public string ESTM_Grade { get; set; }
        public string ESTM_Flg { get; set; }
        public string ESTM_MarksGradeFlg { get; set; }
    }
    public class missingfiles
    {
        public long LPSTUEXAS_Id { get; set; }
        public long LPSTUEX_Id { get; set; }
        public string LPSTUEXAS_AnswerSheetFile { get; set; }
        public string LPSTUEXAS_AnswerSheetPath { get; set; }
    }
    public class correctedanswerfiles
    {
        public long LPSTUEXASTF_Id { get; set; }
        public long LPSTUEX_Id { get; set; }
        public string LPSTUEXASTF_AnswerSheetFile { get; set; }
        public string LPSTUEXASTF_AnswerSheetPath { get; set; }
    }
    public class selectedstudetntspublish
    {
        public long AMST_Id { get; set; }
        public long LPSTUEX_Id { get; set; }
        public bool? LPSTUEX_PublishToStudent { get; set; }
    }
    public class MergeFilesDTO
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileOrder { get; set; }
        public string FileType { get; set; }
    }

    public class Temp_Staff_Ques_Subjective_Files
    {
        public long LPSTUEXSANSSFL_Id { get; set; }
        public long LPSTUEXSANS_Id { get; set; }
        public string LPSTUEXSANSSFL_FileName { get; set; }
        public string LPSTUEXSANSSFL_FilePath { get; set; }
    }
}