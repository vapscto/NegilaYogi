using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_BookDTO
    {
        public long HREBK_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREBK_BookName { get; set; }
        public string HREBK_Author { get; set; }
        public string HREBK_Title { get; set; }
        public string HREBK_PublisherName { get; set; }
        public string HREBK_Volume { get; set; }
        public string HREBK_IssueNo { get; set; }
        public long HREBK_PageNo { get; set; }
        public string HREBK_Editon { get; set; }
        public string HREBK_Publisher { get; set; }
        public string HREBK_Place { get; set; }
        public long HREBK_Year { get; set; }
        public string HREBK_Document { get; set; }
        public bool HREBK_ActiveFlg { get; set; }
        public long HREBK_CreatedBy { get; set; }
        public long HREBK_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public string HREBK_DocumentPath { get; set; }
    }
}
