using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Preadmission
{
    public class CollegeStudentPrevSchoolDTO : CommonParamDTO
    {
        public long PACSTPS_Id { get; set; }
        public long PACA_Id { get; set; }
        public string PACSTPS_PrvSchoolName { get; set; }
        public string PACSTPS_PreSchoolState { get; set; }
        public string PACSTPS_Address { get; set; }
        public string PACSTPS_PreSchoolCountry { get; set; }
        public string PACSTPS_PreSchoolBoard  { get; set; }
        public string PACSTPS_PreSchoolType { get; set; }
        public string PACSTPS_MediumOfInst { get; set; }
        public string PACSTPS_PreviousClass { get; set; }
        public string PACSTPS_PreviousBranch { get; set; }
        public string PACSTPS_PreviousMarks { get; set; }

        public string PACSTPS_PreviousMarksObtained { get; set; }
        public string PACSTPS_PreviousPer { get; set; }
        public string PACSTPS_PreviousGrade { get; set; }
        public string PACSTPS_LeftYear { get; set; }
        public string PACSTPS_LeftReason { get; set; }
        public string PACSTPS_PreviousRegNo { get; set; }
        public string PACSTPS_PreviousTCNo { get; set; }
        public string PACSTPS_PreSchoolPlace { get; set; }
        public string PACSTPS_PreSchoolDistrict { get; set; }
        public DateTime PACSTPS_TCDate { get; set; }
        public string PACSTPS_PasssedMonthYear { get; set; }
        public string PACSTPS_LanguagesTaken { get; set; }

        public string PACSTPS_Urbanrural { get; set; }

        public string PACSTPS_Attempts { get; set; }

        public string PACSTPS_PreviousExamPassed { get; set; }
        public string PACSTPS_Result { get; set; }
    }
}
