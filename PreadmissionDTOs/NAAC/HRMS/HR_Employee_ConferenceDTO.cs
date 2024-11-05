using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_ConferenceDTO
    {
        public long HRECONF_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRECONF_PaperTitle { get; set; }
        public string HRECONF_ConferenceName { get; set; }
        public string HRECONF_ConferenceVenue { get; set; }
        public string HRECONF_Duration { get; set; }
        public string HRECONF_Authors { get; set; }
        public string HRECONF_Volume { get; set; }
        public string HRECONF_IssueNo { get; set; }
        public long HRECONF_PageNo { get; set; }
        public string HRECONF_ProceddingsName { get; set; }
        public string HRECONF_Country { get; set; }
        public string HRECONF_PublisherName { get; set; }
        public string HRECONF_Place { get; set; }
        public long HRECONF_Year { get; set; }
        public string HRECONF_Document { get; set; }
        public bool HRECONF_ActiveFlg { get; set; }
        public long HRECONF_CreatedBy { get; set; }
        public long HRECONF_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string retrunMsg { get; set; }
        public string HRECONF_Journal { get; set; }
        public string HRECONF_NatIntFlg { get; set; }
        public string HRECONF_DocumentPath { get; set; }
        public string HRECONF_ProceddingsISBN { get; set; }
        public string HRECONF_AffiliatedInstitute { get; set; }
        public string HRECONF_Link { get; set; }
        public DateTime HRECONF_Fromdate { get; set; }
        public DateTime HRECONF_Todate { get; set; }
    }
}
