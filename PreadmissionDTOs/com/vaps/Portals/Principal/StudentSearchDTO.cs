using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
    public class StudentSearchDTO : CommonParamDTO
    {
        public Array studentlistsearch { get; set; }
        public string searchfiltervalue { get; set; }
        public string searchbyflag { get; set; }
        public Array salarylist { get; set; }
        public Array salaryDetailslist { get; set; }
        public Array salaryEarningDlist { get; set; }
        public Array allperiods { get; set; }
        public Array periods { get; set; }
        public Array class_sectons { get; set; }
        public Array TT_final_generationDetails { get; set; }
        public Array TT_final_generation { get; set; }
        public Array getfeedetails { get; set; }
        public Array subwiseexmlist { get; set; }
        public Array termwisefeelist { get; set; }
        public string monthName { get; set; }
        public decimal? salary { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long EME_Id { get; set; }
        public string DayName { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string SubjectName { get; set; }
        public int PeriodCount { get; set; }

        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }
        public string Period { get; set; }
        public string P_Days { get; set; }
        public Array datalst { get; set; }
        public Array attendencelist { get; set; }
        public int dayCount { get; set; }
        public long TTMD_Id { get; set; }
        public long userid { get; set; }
        public string ASMAY_Year { get; set; }
        public Array fillstudent { get; set; }
        public Array fillstudentalldetails { get; set; }
        public long Amst_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string amst_FirstName { get; set; }
        public string amst_MiddleName { get; set; }
        public string amst_LastName { get; set; }

        public string amst_RegistrationNo { get; set; }
        public string amst_AdmNo { get; set; }
        public string amst_sex { get; set; }
        public DateTime amst_dob { get; set; }
        public string amst_emailid { get; set; }
        public long amay_RollNo { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }

        public long rollno { get; set; }
        public string admno { get; set; }
        public long amst_mobile { get; set; }
        public string fathername { get; set; }
        public string mothername { get; set; }
        public string bloodgroup { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public DateTime studentdob { get; set; }
        public long? fathermobileno { get; set; }
        public string asma_year { get; set; }
        public Array examlist { get; set; }
        public Array studentdivlist { get; set; }
        public Array feeDetails { get; set; }
        public string exam_name { get; set; }
        public decimal? totalmarks { get; set; }
        public decimal? obtainmarks { get; set; }
        public decimal? persentage { get; set; }
        public string result { get; set; }
        public long user_id { get; set; }
        public Array yearlist { get; set; }
        public Array currentYear { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public int studentCount { get; set; }
        public string AMST_FatherOccupation { get; set; }
        public string AMST_MotherOccupation { get; set; }
        public string amsT_Photoname { get; set; }
    }
}


