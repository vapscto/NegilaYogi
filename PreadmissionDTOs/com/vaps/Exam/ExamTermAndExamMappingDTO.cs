using Microsoft.AspNetCore.Http;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamTermAndExamMappingDTO
    {

        public int ECTMP_Id { get; set; }
        public int EMCA_Id { get; set; }
        public int EYC_Id { get; set; }
        public int ECT_Id { get; set; }
        public long exmid { get; set; }
        public string ECT_TermName { get; set; }
        public DateTime? ECT_TermStartDate { get; set; }
        public DateTime? ECT_TermEndDate { get; set; }
        public DateTime? ECT_PublishDate { get; set; }
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMAY_Id { get; set; }
        public decimal? ECT_MinMarks { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public bool ECT_ActiveFlag { get; set; }
        public Array exammastername { get; set; }
        public Array editlist { get; set; }
        public Array termlist { get; set; }
        public Array examnamelist { get; set; }
        public bool already_cnt { get; set; }
        public int? EME_Id { get; set; }
        public int?[] EME_Ids { get; set; }
        public string EME_ExamName { get; set; }
        public Array termmapplist { get; set; }
        public bool? Active_flag { get; set; }
        public Array categorylist { get; set; }
        public string EMCA_CategoryName { get; set; }
        public Array termgridlist { get; set; }
        public Array examlist { get; set; }
        public string ECTMP_MarksPerFlag { get; set; }
        public decimal ECT_Marks { get; set; }
        public Array mapgridlist { get; set; }
        public string ASMAY_Year { get; set; }
        public bool? ECTMP_ActiveFlag { get; set; }
        public Array exampopup { get; set; }
        public bool? ECTMPE_ActiveFlag { get; set; }
        public int? EME_ID { get; set; }
        public int ECTMPE_Id { get; set; }
        public Array editexmlist { get; set; }
        public string ECTMP_Name { get; set; }
        public Array getyear { get; set; }
        public int EME_ExamOrder { get; set; }
        public string message { get; set; }
        public Exm_CCE_Term_Save_Details[] saveddetails { get; set; }
        public decimal? ECTEX_MarksPercentValue { get; set; }
        public string ECTEX_MarksPerFlag { get; set; }
        public bool? ECTEX_ActiveFlag { get; set; }
        public Array viewdetails { get; set; }
        public long ECTEX_Id { get; set; }
        public int EMGR_Id { get; set; }
        public Array getexamlist { get; set; }
        public Array getgradelist { get; set; }
        public bool ECTEX_RoundOffReqFlg { get; set; }
        public bool ECTEX_ConversionReqFlg { get; set; }
        public string EMGR_GradeName { get; set; }
        public bool? ECTEX_NotApplToTotalFlg { get; set; }
    }
    public class Exm_CCE_Term_Save_Details
    {
        public string examname { get; set; }
        public int examid { get; set; }
        public decimal? marksorpercentage { get; set; }
        public string marksorpercentageflag { get; set; }
        public bool roundofflag { get; set; }
        public bool converstionreqflag { get; set; }
        public long ECTEX_Id { get; set; }
        public bool? ECTEX_NotApplToTotalFlg { get; set; }
    }
}
