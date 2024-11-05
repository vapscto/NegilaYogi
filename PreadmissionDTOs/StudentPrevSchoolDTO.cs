using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs
{
    public class StudentPrevSchoolDTO : CommonParamDTO
    {
        public long PASRPS_Id { get; set; }
        public long PASR_Id { get; set; }
        public string PASRPS_PrvSchoolName { get; set; }
        public string PASRPS_PreviousClass { get; set; }
        public string PASRPS_PreviousMarks { get; set; }
        public string PASRPS_PreviousPer { get; set; }
        public string PASRPS_PreviousGrade { get; set; }
        public string PASRPS_LeftYear { get; set; }
        public DateTime? PASRPS_LeftDate { get; set; }
        public string PASRPS_LeftReason { get; set; }
        public string PASRPS_Board { get; set; }
        public string PASRPS_Address { get; set; }
        public string PASRPS_ConcOrScholarshipFlg { get; set; }
        public DateTime? PASRPS_ConcOrScholarshipDate { get; set; }
        public string PASRPS_TcNo { get; set; }
        public DateTime? PASRPS_TcIssueDate { get; set; }
        public DateTime? PASRPS_TCProducingDate { get; set; }
    }
}
