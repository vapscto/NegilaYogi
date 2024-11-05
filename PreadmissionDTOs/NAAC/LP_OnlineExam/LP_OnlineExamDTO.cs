using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.LP_OnlineExam
{
    public class LP_OnlineExamDTO
    {
        // LP ONLINE EXAM CONFIG
        public long LPMOES_Id { get; set; }
        public long countclass { get; set; }
        public long MI_Id { get; set; }
        public long Userid { get; set; }
        public long roleid { get; set; }
        public long LPMCOMP_Id { get; set; }
        public long LPMOES_NoofQns { get; set; }
        public decimal? LPMOES_TotalMarks { get; set; }
        public string LPMOES_TotalDuration { get; set; }
        public decimal? LPMOES_EachQnsMarks { get; set; }
        public string LPMOES_EachQnsDuration { get; set; }
        public long LPMOES_NoOfOptions { get; set; }
        public Array getconfigloaddataarray { get; set; }
        public bool returnval { get; set; }
        public string message { get; set; }
        public string duplicatemessage { get; set; }
        public string LPMCOMP_ComplexityName { get; set; }
        public string LPMCOMP_ComplexityDesc { get; set; }
        public bool? LPMCOMP_DefaultFlg { get; set; }
        public long? subjective_question_mapped_count { get; set; }
        public bool? objective_question_mapped_count { get; set; }

        // LP MASTER QUESTION DTO
        public long? LPMOEQ_Id { get; set; }
        public long LPMOEQF_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMT_Id { get; set; }
        public string LPMOEQ_Question { get; set; }
        public decimal? LPMOEQ_Marks { get; set; }
        public string LPMOEQ_QuestionDesc { get; set; }
        public string ASMAY_Year { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string LPMT_TopicName { get; set; }
        public string examname { get; set; }
        public long countdoc { get; set; }
        public long countoption { get; set; }
        public long editcount { get; set; }
        public long counttopics { get; set; }
        public long getexamhappenedcount { get; set; }
        public int order { get; set; }
        public bool LPMOEQ_ActiveFlg { get; set; }
        public DateTime CreatedDate { get; set; }
        public Array getsubjectlist { get; set; }
        public Array getyearlist { get; set; }
        public Array getarratcomplexities { get; set; }
        public Array getclasslist { get; set; }
        public Array getmasterexamdetails { get; set; }
        public Array getsectionlist { get; set; }
        public Array gettopiclist { get; set; }
        public Array getedittopiclist { get; set; }
        public Array geteditmasterquestion { get; set; }
        public Array geteditdocuments { get; set; }
        public Array geteditclasslist { get; set; }
        public Array geteditsubjectlist { get; set; }
        public Array geteditexamtopiclist { get; set; }
        public Array getviedocarray { get; set; }
        public Array getMasterQuestiondetails { get; set; }
        public Array getConfigurationSettings { get; set; }
        public Array getSavedOptions { get; set; }
        public Array getViewSavedOptions { get; set; }
        public Array getviewsavedmfoptions { get; set; }
        public Array getViewSavedOptionsFiles { get; set; }
        public tempfilesDTO[] tempfilesDTO { get; set; }
        public tempoptionsDTO[] tempoptionsDTO { get; set; }
        public Temp_MF_OptionsDTO[] Temp_MF_OptionsDTO { get; set; }
        public long LPMOEQOA_Id { get; set; }
        public bool? LPMOEQ_SubjectiveFlg { get; set; }
        public bool? LPMOEQ_MatchTheFollowingFlg { get; set; }
        public string LPMOEQ_StructuralFlg { get; set; }
        public string LPMOEQ_Answer { get; set; }
        public long LPMOEQ_NoOfOptions { get; set; }
        public long countoptionfiles { get; set; }
        public long? examconducted_count { get; set; }
        public string LPMOEQOA_Option { get; set; }
        public string LPMOEQOA_OptionCode { get; set; }
        public bool? LPMOEQOA_AnswerFlag { get; set; }
        public bool LPMOEQOA_ActiveFlg { get; set; }
        public long LPMOEQOAF_Id { get; set; }
        public long? LPMOEQ_MFCount { get; set; }
        public long LPMOEQOAMF_Id { get; set; }
        public string LPMOEQOAMF_MatchtheFollowing { get; set; }
        public bool? LPMOEQOAMF_AnswerFlag { get; set; }
        public int? LPMOEQOAMF_Order { get; set; }
        public string LPMOEQOAF_FileName { get; set; }
        public string LPMOEQOAF_FilePath { get; set; }
        public string LPMOEQF_FileName { get; set; }
        public string LPMOEQF_FilePath { get; set; }
        public int? LPMOEQ_MFRowCount { get; set; }
        public int? LPMOEQ_MFColumnCount { get; set; }

        //********** LP SCHOOL ONLINE EXAM MASTER EXAM  *****************//
        public Array getquestionlist { get; set; }
        public Array getsavequestionlist { get; set; }
        public Array getMasterExamQuestiondetails { get; set; }
        public Array getviewexamquestiondetails { get; set; }
        public Array getviewexamquestiontopicdetails { get; set; }
        public string LPMOEEX_ExamName { get; set; }
        public string LPMOEEX_ExamDuration { get; set; }
        public long LPMOEEX_Id { get; set; }
        public long LPMOEEX_NoOfQuestion { get; set; }
        public long LPMOEEXQNS_Id { get; set; }
        public bool LPMOEEX_RandomFlg { get; set; }
        public bool LPMOEEX_UploadExamPaperFlg { get; set; }
        public string LPMOEEX_QuestionPaperDesc { get; set; }
        public string LPMOEEX_QuestionPaper { get; set; }
        public string LPMOEEX_AnswerSheet { get; set; }
        public bool? LPMOEEX_AutoPublishFlg { get; set; }
        public string LPMOEEX_QuestionPapeFileName { get; set; }
        public string LPMOEEX_AnswerPapeFileName { get; set; }
        public bool LPMOEEXQNS_ActiveFlg { get; set; }
        public bool LPMOEEX_ActiveFlg { get; set; }
        public decimal? LPMOEEX_TotalMarks { get; set; }
        public decimal? LPMOEEX_MarksPerQns { get; set; }
        public decimal? LPMOEEXQNS_Marks { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public tempquestiondto[] tempquestiondto { get; set; }
        public tempquestiondto[] temporderquestiondto { get; set; }
        public ExamLevelDetails[] ExamLevelDetails { get; set; }
        public ExamLevelDetails[] ExamOrderLevelDetails { get; set; }
        public sectiondetailslist[] sectiondetailslist { get; set; }
        public tempcomplexitites[] tempcomplexitites { get; set; }
        public temptopics[] temptopics { get; set; }
        public temptopicDTO[] temptopicDTO { get; set; }
        public DateTime? LPMOEEX_FromDateTime { get; set; }
        public DateTime? LPMOEEX_ToDateTime { get; set; }
        public int fhrors { get; set; }
        public int fminutes { get; set; }
        public int fsec { get; set; }
        public int thrors { get; set; }
        public int tminutes { get; set; }
        public int tsec { get; set; }
        public int? EME_Id { get; set; }
        public Array getarrayofleveldetails { get; set; }
        public Array getarrayoflevelquestiondetails { get; set; }
        public Array getarrayoflevelquestiondetailsfiles { get; set; }
        public Array getarrayoflevelquestionoptiondetails { get; set; }
        public Array getarrayoflevelquestionoptiondetailsfiles { get; set; }
        public Array getarrayoflevelquestionoptionmfdetails { get; set; }
        public Array getexamleveldetails { get; set; }
        public Array getexamquestionlist { get; set; }
        public Array getquestionoptionlist { get; set; }
        public Array getquestionmfoptionlist { get; set; }
        public Array getquestiondoclist { get; set; }
        public Array getoptionwisefiles { get; set; }
        public Array getexamdetails { get; set; }
        public Array getediteleveldetails { get; set; }
        public Array geteditelevelquestions { get; set; }
        public Array geteditelevelquestionsfiles { get; set; }
        public Array geteditmasteroeexam { get; set; }
        public long? LPMOEEXLVL_Id { get; set; }
        public long? LPMOEEXQNS_QnsOrder { get; set; }
        public bool? LPMOEEX_NotLinkedToQnsBankFlg { get; set; }
        public bool? LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg { get; set; }
        public string LPMOEEX_Duration { get; set; }
        public string LPMOEEX_DurationFlag { get; set; }
        public string LPMOEEXQNS_Question { get; set; }
        public bool? LPMOEEXQNS_SubjectiveFlg { get; set; }
        public bool? LPMOEEXQNS_MatchTheFollowingFlg { get; set; }
        public long? LPMOEEXQNS_NoOfOptions { get; set; }
        public long? LPMOEEXQNS_NoOfRows { get; set; }
        public long? LPMOEEXQNS_NoOfColumns { get; set; }
        public string LPMOEEXQNS_QuestionType { get; set; }
        public Array geteditelevelquestionsoptions { get; set; }
        public Array geteditelevelquestionsoptionsfiles { get; set; }
        public long LPMOEEXQNSOPT_Id { get; set; }       
        public string LPMOEEXQNSOPT_Option { get; set; }
        public string LPMOEEXQNSOPT_OptionCode { get; set; }
        public string LPMOEEXQNSOPT_OptionImage { get; set; }
        public bool? LPMOEEXQNSOPT_AnswerFlag { get; set; }
        public string LPMOEEXQNSOPT_AnswerDesc { get; set; }
        public decimal? LPMOEEXQNSOPT_Marks { get; set; }
        public bool? LPMOEEXQNSOPT_ActiveFlg { get; set; }
        public bool? LPMOEEXQNSOPTMF_Answer_Flg { get; set; }
        public Array geteditelevelquestionsoptionsmf { get; set; }

        public long LPMOEEXQNSOPTMF_Id { get; set; }        
        public string LPMOEEXQNSOPTMF_MatchtheFollowing { get; set; }
        public long? LPMOEEXQNSOPTMF_Answer_LPMOEEXQNSOPT { get; set; }
        public bool? LPMOEEXQNSOPTMF_ActiveFlg { get; set; }
        public int? LPMOEEXQNSOPTMF_Order { get; set; }

        //********** LP SCHOOL ONLINE EXAM MASTER EXAM TOPIC  *****************//  
        public long LPMOEEXTOP_Id { get; set; }
        public bool LPMOEEXTOP_ActiveFlg { get; set; }
        public bool? subjectiveflag { get; set; }

        // Master Complexities
        public Array getMasterComplexitiesdetails { get; set; }

        // Report      
        public string Report_Type { get; set; }
        public Array GetReport { get; set; }
        public ClasswisestudentdetailsDTO[] classlistarray { get; set; }
        public string editflag { get; set; }
        public string Order_Flag { get; set; }
        public string LPMOEEXQNS_Answer { get; set; }
        public Array get_stutdent_view_subjectfiles_list { get; set; }

        // Manual Questions Files
        public long LPMOEEXQNSF_Id { get; set; }       
        public string LPMOEEXQNSF_FileName { get; set; }
        public string LPMOEEXQNSF_FilePath { get; set; }
        public bool? LPMOEEXQNSF_ActiveFlag { get; set; }

        // Manual Questions Options Files
        public long LPMOEEXQNSOPTF_Id { get; set; }        
        public string LPMOEEXQNSOPTF_FileName { get; set; }
        public string LPMOEEXQNSOPTF_FilePath { get; set; }
        public bool? LPMOEEXQNSOPTF_ActiveFlag { get; set; }
    }

    public class tempfilesDTO
    {
        public long LPMOEQF_Id { get; set; }
        public string LPMOEQF_FileName { get; set; }
        public string LPMOEQF_FilePath { get; set; }
    }
    public class temptopicDTO
    {
        public long LPMT_Id { get; set; }
        public long LPMOEEXTOP_Id { get; set; }
        public long LPMOEEX_Id { get; set; }
    }
    public class tempoptionsDTO
    {
        public long LPMOEQOA_Id { get; set; }
        public string LPMOEQOA_Option { get; set; }
        public string LPMOEQOA_OptionCode { get; set; }
        public bool LPMOEQOA_AnswerFlag { get; set; }
        public decimal? LPMOEQOA_Marks { get; set; }
        public tempoptionsfiles[] tempoptionsfiles { get; set; }
        public Temp_MF_OptionsDTO[] Temp_MF_OptionsDTO { get; set; }

    }
    public class Temp_MF_OptionsDTO
    {
        public long LPMOEQOAMF_Id { get; set; }
        public long LPMOEQOA_Id { get; set; }
        public string LPMOEQOAMF_MatchtheFollowing { get; set; }
        public long LPMOEQOAMF_Answer_LPMOEQOA_Id { get; set; }
        public bool? LPMOEQOAMF_ActiveFlg { get; set; }
        public bool? LPMOEQOAMF_AnswerFlag { get; set; }
        public int? LPMOEQOAMF_Order { get; set; }
    }
    public class tempquestiondto
    {
        public long LPMOEEXQNS_Id { get; set; }
        public decimal? LPMOEEXQNS_Marks { get; set; }
        public long? LPMOEQ_Id { get; set; }
        public long? LPMOEEXQNS_QnsOrder { get; set; }
        public string LPMOEEXQNS_Question { get; set; }
        public string LPMOEEXQNS_Answer { get; set; }
        public bool? LPMOEEXQNS_SubjectiveFlg { get; set; }
        public bool? LPMOEEXQNS_MatchTheFollowingFlg { get; set; }
        public long? LPMOEEXQNS_NoOfOptions { get; set; }
        public long? LPMOEEXQNS_NoOfRows { get; set; }
        public long? LPMOEEXQNS_NoOfColumns { get; set; }
        public string LPMOEEXQNS_QuestionType { get; set; }
        public Temp_Manual_Ques_Options[] Temp_Manual_Ques_Options { get; set; }
        public Temp_Manual_Ques_Files[] Temp_Manual_Ques_Files { get; set; }
    }
    public class temptopics
    {
        public long LPMT_Id { get; set; }
    }
    public class tempoptionsfiles
    {
        public long LPMOEQOAF_Id { get; set; }
        public string LPMOEQOAF_FileName { get; set; }
        public string LPMOEQOAF_FilePath { get; set; }
        public bool? LPMOEQOAF_ActiveFlag { get; set; }
    }
    public class ExamLevelDetails
    {
        public long LPMOEEXLVL_Id { get; set; }
        public string LPMOEEXLVL_LevelDesc { get; set; }
        public long? LPMOEEXLVL_TotalNoOfQns { get; set; }
        public long? LPMOEEXLVL_MaxQns { get; set; }
        public decimal? LPMOEEXLVL_LevelTotalMarks { get; set; }
        public decimal? LPMOEEXLVL_MarksPerQns { get; set; }
        public bool LPMOEEXLVL_ActiveFlg { get; set; }
        public long? LPMOEEXLVL_LevelOrder { get; set; }
        public tempquestiondto[] questionlist { get; set; }
        public classlsttwo[] classlsttwo { get; set; }
      
    }
    public class sectiondetailslist
    {
        public long ASMS_Id { get; set; }
    }
    public class tempcomplexitites
    {
        public long LPMCOMP_Id { get; set; }
    }
    public class classlsttwo
    {
        public long ASMCL_Id { get; set; }
    }
    public class Temp_Manual_Ques_Options
    {
        public long LPMOEEXQNSOPT_Id { get; set; }
        public long LPMOEEXQNS_Id { get; set; }
        public string LPMOEEXQNSOPT_Option { get; set; }
        public string LPMOEEXQNSOPT_OptionCode { get; set; }
        public decimal? LPMOEEXQNSOPT_Marks { get; set; }
        public bool? LPMOEEXQNSOPT_AnswerFlag { get; set; }
        public Temp_Manual_Ques_Options_Mf[] Temp_Manual_Ques_Options_Mf { get; set; }
        public Temp_Manual_Ques_Opts_Files[] Temp_Manual_Ques_Opts_Files { get; set; }
    }
    public class Temp_Manual_Ques_Options_Mf
    {
        public long LPMOEEXQNSOPTMF_Id { get; set; }
        public long LPMOEEXQNSOPT_Id { get; set; }
        public string LPMOEEXQNSOPTMF_MatchtheFollowing { get; set; }
        public long? LPMOEEXQNSOPTMF_Answer_LPMOEEXQNSOPT { get; set; }
        public bool? LPMOEEXQNSOPTMF_Answer_Flg { get; set; }
        public int? LPMOEEXQNSOPTMF_Order { get; set; }
    }
    public class Temp_Manual_Ques_Files
    {
        public long LPMOEEXQNSF_Id { get; set; }
        public long LPMOEEXQNS_Id { get; set; }
        public string LPMOEEXQNSF_FileName { get; set; }
        public string LPMOEEXQNSF_FilePath { get; set; }
        public bool? LPMOEEXQNSF_ActiveFlag { get; set; }
    }
    public class Temp_Manual_Ques_Opts_Files
    {
        public long LPMOEEXQNSOPTF_Id { get; set; }
        public long LPMOEEXQNSOPT_Id { get; set; }
        public string LPMOEEXQNSOPTF_FileName { get; set; }
        public string LPMOEEXQNSOPTF_FilePath { get; set; }
        public bool? LPMOEEXQNSOPTF_ActiveFlag { get; set; }
    }
}
