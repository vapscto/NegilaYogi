using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.VMS.HRMS
{
    public class OnlineTestDTO
    {
        public long OTMOEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long roleid { get; set; }
        public long Userid { get; set; }
        public int countoption { get; set; }
        public long HRMP_Id { get; set; }
        public long OTQPTYP_Id { get; set; }
        public string OTMOEQ_Question { get; set; }
        public string HRMP_Position { get; set; }
        public string OTQPTYP_QuestionPaperName { get; set; }
        public bool OTMOEQ_SubjectiveFlg { get; set; }
        public bool OTMOEQ_ActiveFlg { get; set; }
        public bool returnval { get; set; }
        public string OTMOEQ_Answer { get; set; }
        public decimal OTMOEQ_Marks { get; set; }
        public string OTMOEQ_QuestionDesc { get; set; }
        public Array papertypelist { get; set; }
        public Array editlist { get; set; }
        public Array geteditmasterquestion { get; set; }
        public Array getMasterQuestiondetails { get; set; }
        public Array positionlist { get; set; }
        public Array getviedocarray { get; set; }
        public Array getSavedOptions { get; set; }
        public Array getViewSavedOptions { get; set; }
        public long OTMOEQOA_Id { get; set; }
      
        public string message { get; set; }
        public string OTMOEQOA_Option { get; set; }
        public string OTMOEQOA_OptionCode { get; set; }
        public bool OTMOEQOA_AnswerFlag { get; set; }
        public string OTMOEQOA_AnswerDesc { get; set; }
        public DateTime OTMOEQ_CreatedDate { get; set; }
        public tempoptionsDTO[] tempoptionsDTO { get; set; }

    }

    public class tempoptionsDTO
    {
        public long OTMOEQOA_Id { get; set; }
        public string OTMOEQOA_Option { get; set; }
        public string OTMOEQOA_OptionCode { get; set; }
        public bool OTMOEQOA_AnswerFlag { get; set; }
        public string OTMOEQOA_AnswerDesc { get; set; }
    }

}