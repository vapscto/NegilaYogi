using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class HHSTCCustomReportDTO
    {
        public Array accyear { get; set; }
        public Array accclass{ get;set;}
        public Array accsection { get; set; }
        public Array studentlist { get; set; }
        public Array classsecregno { get; set; }
        public Array studentTCList { get; set; }
        public Array MasterCompany { get; set; }
        public Array academicList1 { get; set; }
        public Array previousschool { get; set; }
        public Array getnextclass { get; set; }
        public Array classnamejoin { get; set; }
        public long AMST_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long? AMST_AadharNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public long? classid { get; set; }
        public long? joinclassid { get; set; }
        public decimal? Fee_Due_Amnt { get; set; }
        public decimal? Library_Due_Amnt { get; set; }
        public decimal? Store_Canteen_Due { get; set; }
        public decimal? PDA_Due { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string admnoorname { get; set; }
        public string tctemporper { get; set; }
        public string studentname { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_FatherName { get; set; }
        public string AMST_MotherName { get; set; }
        public string Nationality { get; set; }
        public string AMST_BirthPlace { get; set; }
        public string AMST_DOB_Words { get; set; }
        public string AMST_Sex { get; set; }
        public string astC_TCNO { get; set; }
        public string AMST_emailId { get; set; }
        public string astC_LeavingReason { get; set; }
        public string Last_Class_Studied { get; set; }
        public string AMST_PerStreet { get; set; }
        public string AMST_PerArea { get; set; }
        public string AMST_PerCity { get; set; }
        public string AMST_ConStreet { get; set; }
        public string AMST_ConArea { get; set; }
        public string AMST_ConCity { get; set; }
        public string ASTC_Remarks { get; set; }
        public string Religion { get; set; }
        public string caste { get; set; }
        public string classname { get; set; }
        public string qualificlass { get; set; }
        public string classjoinname { get; set; }
        public string ASTC_Qual_PromotionFlag { get; set; }
        public string ASTC_Conduct { get; set; }
        public string companyname { get; set; }
        public DateTime AMST_DOB { get; set; }
        public DateTime AMST_Date { get; set; }
        public DateTime ASTC_LastAttendedDate { get; set; }      
        public DateTime astC_TCIssueDate { get; set; }
        public DateTime tcdatess { get; set; }
        public string bpl_id { get; set; }
        public DateTime ? leftDate { get; set; }
        public string mothertounge { get; set; }
        public string category { get; set; }
        public string feepaid { get; set; }
        public DateTime ASTC_TCDate { get; set; }
        public Array studenttcdetails { get; set; }
        public Array getadm_m_student_details { get; set; }
    }
}
