using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class BookArrivalReportDTO:CommonParamDTO
    {
        public long LMB_Id { get; set; }
        public long Donor_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string LMB_BookTitle { get; set; }
        public string LMB_BookType { get; set; }
        public string LMB_ISBNNo { get; set; }
        public string Donor_Name { get; set; }
        public string Donor_Address { get; set; }
        public DateTime Purchase_Date { get; set; }
        public DateTime purch_FrmDate { get; set; }
        public DateTime purch_ToDate { get; set; }
        public long LMAL_Id { get; set; }
        public string Type { get; set; }
        public string Type2 { get; set; }
        public Array donorlist { get; set; }
        public Array reportlist { get; set; }
        public Array deptlist { get; set; }
        public Array lib_list { get; set; }
        public string LMD_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string Fromdate { get; set; }
        public string ToDate { get; set; }
        public bool BookArrival { get; set; }
        public BookSubjeclistSummary[] BookSummary { get; set; }
        public Array griddata { get; set; }

    }
    public class BookSubjeclistSummary
    {
        public long LMS_Id { get; set; }
        public string LMS_SubjectName { get; set; }
    }
}
