using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.HRMS
{
    public class HR_Employee_IntConferenceDTO
    {
        public long HREINTCONF_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HREINTCONF_PaperTitle { get; set; }
        public string HREINTCONF_ConferenceName { get; set; }
        public string HREINTCONF_ConferenceVenue { get; set; }
        public string HREINTCONF_Duration { get; set; }
        public string HREINTCONF_Journal { get; set; }
        public string HREINTCONF_Authors { get; set; }
        public string HREINTCONF_Volume { get; set; }
        public string HREINTCONF_IssueNo { get; set; }
        public long HREINTCONF_PageNo { get; set; }
        public string HREINTCONF_ProceddingsName { get; set; }
        public string HREINTCONF_Country { get; set; }
        public string HREINTCONF_PublisherName { get; set; }
        public string HREINTCONF_Place { get; set; }
        public long HREINTCONF_Year { get; set; }
        public string HREINTCONF_Document { get; set; }
        public bool HREINTCONF_ActiveFlg { get; set; }
        public long HREINTCONF_CreatedBy { get; set; }
        public long HREINTCONF_UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
