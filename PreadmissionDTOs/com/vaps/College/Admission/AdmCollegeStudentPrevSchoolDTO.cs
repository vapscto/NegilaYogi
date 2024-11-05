﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class AdmCollegeStudentPrevSchoolDTO:CommonParamDTO
    {
        public long ACSTPS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string ACSTPS_PrvSchoolName { get; set; }
        public string ACSTPS_PreSchoolState { get; set; }
        public string ACSTPS_Address { get; set; }
        public string ACSTPS_PreSchoolCountry { get; set; }
        public string ACSTPS_PreSchoolBoard { get; set; }
        public string ACSTPS_PreSchoolType { get; set; }
        public string ACSTPS_MediumOfInst { get; set; }
        public string ACSTPS_PreviousClass { get; set; }
        public string ACSTPS_PreviousMarks { get; set; }
        public string ACSTPS_PreviousMarksObtained { get; set; }
        public string ACSTPS_PreviousPer { get; set; }
        public string ACSTPS_PreviousGrade { get; set; }
        public long ACSTPS_LeftYear { get; set; }
        public string ACSTPS_LeftReason { get; set; }
        public string ACSTPS_PreviousTCNo { get; set; }
        public DateTime? ACSTPS_PreviousTCDate { get; set; }
        public string ACSTPS_PreviousBranch { get; set; }
        public string ACSTPS_PreviousExamPassed { get; set; }
        public string ACSTPS_PreviousRegNo { get; set; }
        public string ACSTPS_PasssedMonthYear { get; set; }
        public string ACSTPS_LanguagesTaken { get; set; }
    }
}
