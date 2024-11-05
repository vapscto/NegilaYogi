using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.OnlineExam
{
    public class MasterQuestionDTO
    {
        public long MI_Id { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool already_cnt { get; set; }
        public string message { get; set; }
        public string ISMS_SubjectName { get; set; }

        //---------------------------------------------------
        public long ASMCL_Id { get; set; }
        public string ASMC_SectionName { get; set; }
        public long ASMS_Id { get; set; }
        public long LMSMOEQ_Id { get; set; }
        public long countview { get; set; }
        public long ISMS_Id { get; set; }
        public string LMSMOEQ_Question { get; set; }
        public decimal LMSMOEQ_Marks { get; set; }
        public string LMSMOEQ_QuestionDesc { get; set; }
        public long LMSMOEQOA_Id { get; set; } 
        public string LMSMOEQOA_Option { get; set; }
        public string LMSMOEQOA_OptionCode { get; set; }
        public bool LMSMOEQOA_AnswerFlag { get; set; }
        public string OptionType { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public DateTime? orderdate { get; set; }
        public string fromdate1 { get; set; }
        public string todate1 { get; set; }
        public Array getQuestiondetails { get; set; }
        public Array getsection { get; set; }
        public Array getQdetails { get; set; }
        public Array getclass { get; set; }
        public Array getSubjects { get; set; }
        public Array getAnsOptions { get; set; }
        public Array editQus { get; set; }
        public Array getFQuestiondetails { get; set; }
        public Array getFQOptiondetails { get; set; }
        public Array getoptiondetails { get; set; }
        public Array result { get; set; }
        public Array onlinereport { get; set; }
        public string ASMCL_ClassName { get; set; }
        public long seleted_Qus { get; set; }
        public SelectOptionDTO[] seleted_Ans { get; set; }
        public class SelectOptionDTO
        {
           // public long LMSMOEQOA_Id { get; set; }
            public long LMSMOEQ_Id { get; set; }
            public string pamoeaO_Options { get; set; }
            public string LMSMOEQOA_Option { get; set; }
            public string LMSMOEQOA_OptionCode { get; set; }
            public bool LMSMOEQOA_AnswerFlag { get; set; }
        }

        public long LMSMOES_Id { get; set; }
        public long Noofques { get; set; }
        public long totmrks { get; set; }
        public string totdur { get; set; }
        public long echmrkques { get; set; }
        public string echquesdur { get; set; }
        public long noopt { get; set; }
        public long LMSMOEQC_Id { get; set; }
        public long[] AMCO_Ids { get; set; }
        public long[] AMB_Ids { get; set; }
        public long[] AMSE_Ids { get; set; }
        public long ASMAY_Id { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array courselist { get; set; }
        public long AMSE_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long LMSMOEQB_Id { get; set; }
        public long LMSSTEACO_Id { get; set; }
        public long LMSSTECO_Id { get; set;}
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMSE_SEMName { get; set; }
        public uploadquestionfilestemp[] uploadquestionfiles { get; set; }
        public Array viewdocarray { get; set; }
        public Array geteditdocs { get; set; }
        public long LMSMOEQF_Id { get; set; }       
        public long Roleid { get; set; }
    }

    public class uploadquestionfilestemp
    {
        public long LMSMOEQF_Id { get; set; }
        public string LMSMOEQF_FileName { get; set; }
        public string LMSMOEQF_FilePath { get; set; }
    }
}