using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.OnlineExam
{
    public class OnlineExamDTO
    {
        public long MI_Id { get; set; }

        public long userid { get; set; }
        public long ASMCL_Id { get; set; }
        public long LMSMOEQ_Id { get; set; }
        public string LMSMOEQ_Question { get; set; }
        public string LMSMOEQOA_OptionCode { get; set; }
        public string LMSMOEQOA_Option { get; set; }
        public Array getQuestion { get; set; }
        public Array savedanswer { get; set; }
        public Array getclass { get; set; }
        public Array getSubjects { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public bool LMSMOEQOA_AnswerFlag { get; set; }
        public SelectOptionDTO[] saveanswerlst { get; set; }
        public Array getQdetails { get; set; }
        public long LMSSTE_Id { get; set; }
        public long Amst_ID { get; set; }
        public string LMSSTE_TotalTime { get; set; }
        public Array result { get; set; }
        public class SelectOptionDTO
        {
           // public long LMSSTE_Id { get; set; }
            public long LMSMOEQ_Id { get; set; }
            public long QuizeQuastions { get; set; }
            public string q_name { get; set; }
           // public test[] test1 { get; set; }
        }
            public class test
            {
                public long? ISMS_Id { get; set; }
                public string ISMS_SubjectName { get; set; }
                public bool LMSMOEQOA_AnswerFlag { get; set; }
                public long? Amst_ID { get; set; }
                public string LMSSTE_TotalTime { get; set; }
                public long? LMSSTE_TotalQnsAnswered { get; set; }
                public long? LMSSTE_TotalCorrectAns { get; set; }
                public long? LMSSTE_TotalMaxMarks { get; set; }
                public long? LMSSTE_TotalMarks { get; set; }
                public decimal? LMSSTE_Percentage { get; set; }
                public long? LMSSTE_Id { get; set; }
                public long LMSMOEQOA_Id { get; set; }
                public long? MI_Id { get; set; }
                public long? userid { get; set; }
                public long ASMCL_Id { get; set; }
                public long LMSMOEQ_Id { get; set; }
                public Array getQuestion { get; set; }
                public Array getclass { get; set; }
                public Array getSubjects { get; set; }
                public string LMSMOEQ_Question { get; set; }
                public string LMSMOEQOA_Option { get; set; }
                public string LMSMOEQOA_OptionCode { get; set; }
                public SelectOptionDTO[] saveanswerlst { get; set; }

            }
         
        public long LMSMOEQOA_Id { get; set; }
        public long LMSSTECO_Id { get; set; }
        public long LMSSTEACO_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMB_ID { get; set; }
        public long AMCO_Id { get; set; }
        public long AMCST_ID { get; set; }
    }
}