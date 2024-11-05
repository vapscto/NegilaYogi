using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentAttendanceReportDTO
    {
        public Array SectionList { get; set; }
        public Array subjectlist { get; set; }
        public Array studenttcdetails { get; set; }
        public Array getadm_m_student_details { get; set; }
        public Array academicList { get; set; }
        public Array classlist { get; set; }
        public Array studentList { get; set; }
        public Array monthList { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMC_Id { get; set; }
        public long AMM_ID { get; set; }
        public int radiotype { get; set; }
        public int monthid { get; set; }
        public int yearid { get; set; }

        public int ASMC_Order { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public Array studentAttendanceList { get; set; }
        public Array studentAttendanceListnew { get; set; }
        public Array totalpercentageatt { get; set; }
        public Array getstudentlist { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string columnname { get; set; }
        public string sectionids { get; set; }
        public long AMAY_Roll_No { get; set; }
        public decimal ASA_ClassHeld { get; set; }
        public decimal ASA_Class_Attended { get; set; }
        public decimal percentage { get; set; }
        public long AMST_Id { get; set; }
        public int type { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public decimal classheld { get; set; }
        public decimal attendance { get; set; }
      //  public string ASMCL_className { get; set; }
        public string ASMC_SectionName { get; set; }
        public string asmcL_ClassName { get; set; }
        public long absent { get; set; }
        public long present { get; set; }
        public long strength { get; set; }
        public string AMC_Name { get; set; }
        public long totalPresent { get; set; }
        public long totalAbsent { get; set; }
        public long Premale { get; set; }
        public long PreFemale { get; set; }
        public long absmale { get; set; }
        public long absFemale { get; set; }
        public string MONTH_NAME { get; set; }
        public long YEAR_NAME { get; set; }
        public long TOTAL_PRESENT { get; set; }
        public long TOTAL_classheld { get; set; }
        public Array studentTCList { get; set; }
        public string AMST_FatherName { get; set; }
        public string AMST_MotherName { get; set; }
        public string Nationality { get; set; }
        public string AMST_BirthPlace { get; set; }
        public string AMST_DOB_Words { get; set; }
        public DateTime AMST_DOB { get; set; }
        public DateTime AMST_Date { get; set; }
        public string ASTC_Conduct { get; set; }
        public string ASTC_Remarks { get; set; }
        public string Religion { get; set; }
        public String caste { get; set; }
        public String castecat { get; set; }
        public Array MasterCompany { get; set; }
        public Array institutiondetails { get; set; }
        public string companyname { get; set; }
        public string type1 { get; set; }
        public string name { get; set; }
        public Array fillclass { get; set; }
        public Array fillsec { get; set; }
        public Array classdetails { get; set; }
        public Array studentlist { get; set; }
        public string regornamedetails { get; set; }
        public long miid { get; set; }
        public string astC_TCNO { get; set; }
        public string AMST_emailId { get; set; }
        public long? AMST_AadharNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public string astC_LeavingReason { get; set; }
        public string Last_Class_Studied { get; set; }
        public DateTime astC_TCIssueDate { get; set; }
        public string AMST_PerStreet { get; set; }
        public string AMST_PerArea { get; set; }
        public string AMST_PerCity { get; set; }
        public string AMST_ConStreet { get; set; }
        public string AMST_ConArea { get; set; }
        public string AMST_ConCity { get; set; }
        public DateTime ASTC_LastAttendedDate { get; set; }
        public string ASTC_Qual_PromotionFlag { get; set; }
        public decimal? Fee_Due_Amnt { get; set; }
        public decimal? Library_Due_Amnt { get; set; }
        public decimal? Store_Canteen_Due { get; set; }
        public decimal? PDA_Due { get; set; }
        public string allorindiflag { get; set; }
        public Array student_teacherList { get; set; }
        public string AMST_Sex { get; set; }
        public long? joinclassid { get; set; }
        public string classjoinname { get; set; }
        public string classname { get; set; }
        public string qualificlass { get; set; }
        public Array academicList1 { get; set; }
        public Array previousschool { get; set; }
        public long? classid { get; set; }
        public Array getnextclass { get; set; }
        public Array classnamejoin { get; set; }
        public DateTime tcdatess { get; set; }     
        public string username { get; set; }
        public long? Emp_Code { get; set; }
        public long? ASALU_Id { get; set; }
        public long? userId { get; set; }
        public long? roleId { get; set; }
        public string rolename { get; set; }
        public string flag { get; set; }
        
        public long? ASMS_Id { get; set; }   
        public long? ISMS_Id { get; set; }       
        public Array academicListdefault { get; set; }
        public Array photopath { get; set; }
        public string rolenameid { get; set; }
        public string photopathname { get; set; }
        public int datewise { get; set; }
        public decimal countclass { get; set; }
        public Array newarray { get; set; }
        public Array newarray_total { get; set; }
        public Array newarray_date { get; set; }
        public Array getbtwn_monthsname { get; set; }
        public Array getsubjectlist { get; set; }
        public int classorder { get; set; }
        public int secorder { get; set; }
        public Array classsecdetails { get; set; }
        public Array stafflist { get; set; }
        public string employeename { get; set; }
        public long hrmE_Id { get; set; }
        public string govtno { get; set; }
        public string reportflag { get; set; }
        public string statename { get; set; }
        public string district { get; set; }
        public string mediumOfInstruction { get; set; }
        public string subjectstudied { get; set; }
        public string electivestudied { get; set; }

        public long noof_daysattended { get; set; }
        public long noof_schooldays{ get; set; }
        public string fee_concesion { get; set; }
        public string promotionflag { get; set; }
        public string medicallyexamined { get; set; }
        public string ASTC_FeePaid { get; set; }
        
        public Array daysOfMonth { get; set; }
        public AbsentSMS1[] AbsentSMS { get; set; }
        public string return_msg { get; set; }
        public Get_Selected_Student_List[] Get_Selected_Student_List { get; set; }
        public Get_Selected_Subject_List[] Get_Selected_Subject_List { get; set; }

        public sectionlistarray1[] sectionlistarray { get; set; }

        public subjectlistarray1[] subjectlistarray { get; set; }
        public classlstarray[] classlsttwo { get; set; }
        public categorylistarray1[] categorylistarray { get; set; }
        

        public string ISMS_SubjectName { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public bool categoryflag { get; set; }

        public string logo_path { get; set; }

        public Array category_list { get; set; }
        public long AMC_Id { get; set; }
        public Array AMC_logo { get; set; }

        public string AMST_SubCasteIMC_Id { get; set; }

        public bool? IVRMGC_AttendanceShortageAlertFlg { get; set; }

        public string message { get; set; }

   public studentAttendanceList_shoratgae1[] studentAttendanceList_shoratgae { get; set; }


    }

    public class studentAttendanceList_shoratgae1
    {
        public long AMST_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string AMST_Name { get; set; }
        public string ASMC_SectionName { get; set; }
        public long AMST_MobileNo { get; set; }
        public string Percentage { get; set; }
    }

    public class Get_Selected_Student_List
    {
        public long AMST_Id { get; set; }
    }
    public class classlstarray
    {
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
    }

    public class categorylistarray1
    {
        public long AMC_Id { get; set; }
      
    }
    
    public class sectionlistarray1
    {
        public long ASMS_Id { get; set; }

        public long ASMCL_Id { get; set; }
    }

    public class subjectlistarray1
    {
        //public long ASMS_Id { get; set; }

        //public long ASMCL_Id { get; set; }

        public long ISMS_Id { get; set; }
    }


    public class AbsentSMS1
    {
        public long AMST_Id { get; set; }
    }

    public class Get_Selected_Subject_List
    {
        public long ISMS_Id { get; set; }
    }
}
