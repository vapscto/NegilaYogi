using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class BookCirculationReportDTO:CommonParamDTO
    {
        public long LMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string LMB_BookType { get; set; }
        public string LMB_BookTitle { get; set; }
        public string IssueFromDate { get; set; }
        public string IssueToDate { get; set; }
        public string DueFromdate { get; set; }
        public string DueTodate { get; set; }
        public Array booklist { get; set; }
        public Array parentsubjectlist { get; set; }
        public Array subsubjectlist { get; set; }
        public Array booktype { get; set; }
        public Array booktitle { get; set; }
        public Array alldata { get; set; }
        public long Book_Trans_Id { get; set; }
        public long AMST_Id { get; set; }
        public long Guest_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string Book_Trans_Status { get; set; }
        public string LMBANO_AccessionNo { get; set; }
        public string Type { get; set; }
        public string AGType { get; set; }
        public string TrnType { get; set; }
        public Array reportlist { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public long ASMCL_Id { get; set; }
        public string className { get; set; }
        public long ASMS_Id { get; set; }
        public string sectionName { get; set; }
        public BookCirculationReportDTO[] selectedClasslist { get; set; }
        public BookCirculationReportDTO[] selectedSectionlist { get; set; }

        public long LMAL_Id { get; set; }
        public Array lib_list { get; set; }
        public long IVRMUL_Id { get; set; }
        public BookSummaryCircular[] BookSummaryCircular { get; set; }
        public bool BookSummary { get; set; }
    }
     public class BookSummaryCircular
    {
        public long LMS_Id { get; set; }
        public string LMS_SubjectName { get; set; }
    }
}
