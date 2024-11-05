using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class AdmittedStudentPrevSchoolDTO
    {
        public long AMSTPS_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMSTPS_PrvSchoolName { get; set; }
        public string AMSTPS_PreSchoolState { get; set; }
        public string AMSTPS_Address { get; set; }
        public string AMSTPS_PreSchoolCountry { get; set; }
        public string AMSTPS_PreSchoolBoard { get; set; }
        public string AMSTPS_PreSchoolType { get; set; }
        public string AMSTPS_MediumOfInst { get; set; }
        public string AMSTPS_PreviousClass { get; set; }
        public string AMSTPS_PreviousMarks { get; set; }
        public string AMSTPS_PreviousPer { get; set; }
        public string AMSTPS_PreviousGrade { get; set; }
        public string AMSTPS_LeftYear { get; set; }
        public string AMSTPS_LeftReason { get; set; }
        public string AMSTPS_ConcOrScholarshipFlg { get; set; }
        public DateTime? AMSTPS_ConcOrScholarshipDate { get; set; }
        public string AMSTPS_PrvTCNO { get; set; }
        public DateTime? AMSTPS_PrvTCDate { get; set; }
    }
}
