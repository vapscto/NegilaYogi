using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_BookChapterDTO
    {
        public long HREBKCP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREBKCP_BookName { get; set; }
        public string HREBKCP_Author { get; set; }
        public string HREBKCP_Title { get; set; }
        public string HREBKCP_PublisherName { get; set; }
        public string HREBKCP_Volume { get; set; }
        public string HREBKCP_IssueNo { get; set; }
        public long HREBKCP_PageNo { get; set; }
        public string HREBKCP_Editon { get; set; }
        public string HREBKCP_Publisher { get; set; }
        public string HREBKCP_Place { get; set; }
        public long HREBKCP_Year { get; set; }
        public string HREBKCP_Document { get; set; }
        public bool HREBKCP_ActiveFlg { get; set; }
        public long HREBKCP_CreatedBy { get; set; }
        public long HREBKCP_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public string HREBKCP_DocumentPath { get; set; }
    }
}
