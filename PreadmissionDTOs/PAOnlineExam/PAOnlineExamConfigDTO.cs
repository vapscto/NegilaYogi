using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.PAOnlineExam
{
    public class PAOnlineExamConfigDTO
    {
        public long PAMOES_Id { get; set; }
        public long MI_Id { get; set; }
        public long PAMOES_NoofQns { get; set; }
        public decimal PAMOES_TotalMarks { get; set; }
        public string PAMOES_TotalDuration { get; set; }
        public decimal PAMOES_EachQnsMarks { get; set; }
        public string PAMOES_EachQnsDuration { get; set; }
        public long PAMOES_NoOfOptions { get; set; }
        public long userid { get; set; }
        public long roleid { get; set; }
        public Array getQdetails { get; set; }
        public Array editQus { get; set; }
        public Array result { get; set; }
        public long Noofques { get; set; }
        public long totmrks { get; set; }
        public string totdur { get; set; }
        public long echmrkques { get; set; }
        public string echquesdur { get; set; }
        public long noopt { get; set; }
        public bool returnval { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
    }
}
