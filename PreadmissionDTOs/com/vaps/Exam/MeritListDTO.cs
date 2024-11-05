using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MeritListDTO
    {
        public long MI_Id { get; set; }
        public int ESTMP_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EME_Id { get; set; }
        public decimal? ESTMP_TotalMaxMarks { get; set; }
        public decimal? ESTMP_TotalObtMarks { get; set; }
        public decimal? ESTMP_Percentage { get; set; }
        public string ESTMP_TotalGrade { get; set; }
        public int? ESTMP_ClassRank { get; set; }
        public int? ESTMP_SectionRank { get; set; }
        public string ESTMP_Result { get; set; }
        public long ASYST_Id { get; set; }
        public long AMAY_RollNo { get; set; }
        public int? AMAY_PassFailFlag { get; set; }
        public long? LoginId { get; set; }
        public DateTime? AMAY_DateTime { get; set; }
        public int AMAY_ActiveFlag { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public DateTime AMST_Date { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public long? AMC_Id { get; set; }
        public string AMST_Sex { get; set; }
        public DateTime AMST_DOB { get; set; }
        public string AMST_DOB_Words { get; set; }
        public int? PASR_Age { get; set; }
        public string AMST_BloodGroup { get; set; }
        public string AMST_MotherTongue { get; set; }
        public string AMST_BirthCertNO { get; set; }
        public long? IVRMMR_Id { get; set; }
        public long? IMCC_Id { get; set; }
        public long? IC_Id { get; set; }
        public Array yearlist { get; set; }
        public Array classname { get; set; }
        public Array secname { get; set; }
        public Array categoryname { get; set; }
        public Array subjectname { get; set; }
        public Array examname { get; set; }
        public Array studentAttendanceList { get; set; }
        public Array masterinstitution { get; set; }
        public Array getsubjecttopers { get; set; }
        public Array getgradewisetotal { get; set; }
        public string username { get; set; }
        public string flag { get; set; }
        public long userid { get; set; }
        public long roleid { get; set; }
    }
}
