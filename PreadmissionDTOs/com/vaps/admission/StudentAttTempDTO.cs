using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentAttTempDTO
    {
        public long? amaY_RollNo { get; set; }
        public string amsT_AdmNo { get; set; }
        public long? amsT_Id { get; set; }
        public string studentname { get; set; }
        public decimal? pdays { get; set; }
        public bool? selected { get; set; }
        public long? ASAS_Id { get; set; }
        //public long AMST_Id { get; set; }
        //public string AMST_AdmNo { get; set; }
        //public long AMAY_RollNo { get; set; }
        public bool? FirstHalfflag { get; set; }
        public bool? SecondHalfflag { get; set; }
        public string asA_Dailytwice_Flag { get; set; }
        public long asA_Id { get; set; }
        public int? TTMP_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long asasB_Id { get; set; }
        public string amsT_RegistrationNo { get; set; }
    }

    public class Absent_Student_List
    {
        public string Studentname { get; set; }
        public long MobileNo { get; set; }
        public DateTime date { get; set; }
        public long Amst_Id { get; set; }
        public string attType { get; set; }
        public bool? FirstHalf { get; set; }
        public bool? SecondHalf { get; set; }

        //  public long Amst_Id { get; set; }

    }

    public class Absent_Student_AbsentList
    {
        public string Studentname { get; set; }
        public long MobileNo { get; set; }
        public DateTime date { get; set; }
        public long Amst_Id { get; set; }
         public long ISMS_Id { get; set; }

    }

    public class Studentattsmartcardabsent
    {
        public long AMST_Id { get; set; }
    }

    public class Studentattsmartcardabsenttimings
    {
        public long ASSCT_Id { get; set; }
        public TimeSpan? ASSCT_FH_TimeTo { get; set; }
        public TimeSpan? ASSCT_FH_TimeFrom { get; set; }
    }

    public class Studentattsmartcardabsent_class_sectionlist
    {
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
    }
    public class class_section_list
    {
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public long roleId { get; set; }
        public long MI_Id { get; set; }
        public string username { get; set; }
        public long ASMAY_Id { get; set; }
        public string message { get; set; }
        public string rolename { get; set; }
        public long Emp_Code { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string asmcL_ClassName { get; set; }
        public string asmC_SectionName { get; set; }
        public long AMST_Id { get; set; }
        public Array get_cls_section { get; set; }
        public DateTime? ASA_FromDate { get; set; }
        public TimeSpan? ASSC_PunchTime { get; set; }
        public string ASA_Network_IP { get; set; }
        public long ASALU_Id { get; set; }
        public string studentname { get; set; }
        public string AMST_AdmNo { get; set; }
        public Array studentlist { get; set; }
        public Studentattsmartcardabsent[] Studentattsmartcardabsent { get; set; }
        public string total_punch { get; set; }
        public string not_punch { get; set; }
        public string offline { get; set; }
    }
}
