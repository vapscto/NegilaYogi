using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
  public  class NAAC_HSU_EvaluationRelated_253_DTO
    {
        public long NCHSU253ER_Id { get; set; }
        public long MI_Id { get; set; }    
        public long NCHSU253ER_Year { get; set; }
        public long NCHSU253ER_TotalNoOfStsAppreadFinalExm { get; set; }
        public long NCHSU253ER_NoOfCasesSingleValuationAppProcRevaluation { get; set; }
        public long NCHSU253ER_NoOfCasesDoubleAppProcRetotallingOnly { get; set; }
        public long NCHSU253ER_NoOfCasesDoubleAppProcRevaluationOnly { get; set; }
        public long NCHSU253ER_NoOfCasesDoubleAppProcRetotalORRevlAccToAnsScript { get; set; }
        public bool NCHSU253ER_ActiveFlag { get; set; }
        public DateTime NCHSU253ER_CreatedDate { get; set; }
        public DateTime NCHSU253ER_UpdatedDate { get; set; }
        public long NCHSU253ER_CreatedBy { get; set; }
        public long NCHSU253ER_UpdatedBy { get; set; }

        public Array list { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public string ASMAY_Year { get; set; }
        public string msg { get; set; }
        public long UserId { get; set; }
        public long NCHSU253ERF_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public bool ret { get; set; }
        public Array editlist { get; set; }
        public NAAC_HSU_EvaluationRelated_253_DTO[] filelist { get; set; }
        public string cfilename { get; set; }
        public string cfilepath { get; set; }
        public string cfiledesc { get; set; }
        public Array editFileslist { get; set; }
        public Array viewuploadflies { get; set; }
        public Array institutionlist { get; set; }

    }
}
