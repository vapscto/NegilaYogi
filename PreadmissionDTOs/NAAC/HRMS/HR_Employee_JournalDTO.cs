using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_JournalDTO
    {
        public long HREJORNL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREJORNL_ArticleTitle { get; set; }
        public string HREJORNL_RefOrNonRefFlg { get; set; }
        public string HREJORNL_NatOrIntNatFlg { get; set; }
        public string HREJORNL_JournalName { get; set; }
        public string HREJORNL_Authors { get; set; }
        public string HREJORNL_ISSNISBN { get; set; }
        public string HREJORNL_ImpactFactor { get; set; }
        public string HREJORNL_Volume { get; set; }
        public string HREJORNL_IssueNo { get; set; }
        public long HREJORNL_PageNo { get; set; }
        public string HREJORNL_Country { get; set; }
        public string HREJORNL_PublisherName { get; set; }
        public string HREJORNL_Place { get; set; }
        public long HREJORNL_Year { get; set; }
        public string HREJORNL_Document { get; set; }
        public bool HREJORNL_ActiveFlg { get; set; }
        public long HREJORNL_CreatedBy { get; set; }
        public long HREJORNL_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public string HREJORNL_DocumentPath { get; set; }
        public string HREJORNL_Link { get; set; }
    }
}
