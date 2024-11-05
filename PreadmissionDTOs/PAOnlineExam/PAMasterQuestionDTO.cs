using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.PAOnlineExam
{
    public class PAMasterQuestionDTO
    {
        public long PAMOEQ_Id { get; set; }
        public long PAMOEQF_Id { get; set; }
        public long MI_Id { get; set; }
        public long ISMS_Id { get; set; }        
        public string PAMOEQ_Question { get; set; }
        public decimal? PAMOEQ_Marks { get; set; }
        public string PAMOEQ_QuestionDesc { get; set; }
        public string ASMCL_ClassName { get; set; }
        public Array getQuestiondetails { get; set; }
        public Array getclass { get; set; }
        public Array getSubjects { get; set; }
        public Array getFQOptiondetails { get; set; }
        public Array getFQuestiondetails { get; set; }
        public Array getoptiondetails { get; set; }
        public Array editQus { get; set; }
        public Array result { get; set; }
        public Array viewdocarray { get; set; }
        public bool returnval { get; set; }
        public DateTime? orderdate { get; set; }
        public string returnduplicatestatus { get; set;}
        public string ISMS_SubjectName { get; set;}
        public string message { get; set;}
        public uploadquestionfiles[] uploadquestionfiles { get; set; }
        public long countview { get; set; }
        public Array geteditdocs { get; set; }
        // CLASS QUESTION DTO
        public long PAMOEQC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long noopt { get; set; }
        public PASelectOptionDTO[] seleted_Ans { get; set; }

        // OUESTION OPTION DTO
        public long PAMOEQOA_Id { get; set; }        
        public string PAMOEQOA_Option { get; set; }
        public string PAMOEQOA_OptionCode { get; set; }
        public bool PAMOEQOA_AnswerFlag { get; set; }

        
    }
    public class PASelectOptionDTO
    {
        // public long LMSMOEQOA_Id { get; set; }
        public long LMSMOEQ_Id { get; set; }
        public string pamoeaO_Options { get; set; }
        public string PAMOEQOA_Option { get; set; }
        public string PAMOEQOA_OptionCode { get; set; }
        public bool PAMOEQOA_AnswerFlag { get; set; }
    }
    public class uploadquestionfiles
    {
        public long PAMOEQF_Id { get; set; }        
        public string PAMOEQF_FileName { get; set; }
        public string PAMOEQF_FilePath { get; set; }
    }
}
