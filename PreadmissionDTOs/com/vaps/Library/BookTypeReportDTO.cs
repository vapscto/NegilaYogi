using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class BookTypeReportDTO:CommonParamDTO
    {
        public long LMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long LMBANO_Id { get; set; }
        public string LMB_BookType { get; set; }
        public string LMB_BookTitle { get; set; }
        public string LMBANO_AccessionNo { get; set; }
        public string LMB_ISBNNo { get; set; }
        public decimal LMB_Price { get;set; }
        public DateTime Issue_Date { get; set; }
        public DateTime IssueToDate { get; set; }
        public string Book_Trans_Status { get; set; }
        public Array getdata { get; set; }
        public Array alldata { get; set; }
        public string Type { get; set; }
        
    }
}
