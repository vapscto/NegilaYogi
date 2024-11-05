using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class StudentTcReportDTO
    {
        public long ASYST_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMC_Id { get; set; }
        public long ASMAY_Id { get; set; }


        public Array fillyear { get; set; }

        public Array fillclass { get; set; }

        public Array fillsection { get; set; }

        public StudentTcReportDTO[] TempararyArrayheadList { get; set; }

        public string tcperortemp { get; set; }
        public string tcallorindi { get; set; }


        public Array alldatagridreport { get; set; }

        public int mid { get; set; }
        //reports columns

        public string AMST_FirstName { get; set; }

        public string AMST_RegistrationNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASTC_LeavingReason { get; set; }
        public string ASTC_Remarks { get; set; }
        //public DateTime ASTC_TCIssueDate { get; set; }
        //public DateTime ASTC_TCDate { get; set; }
        public string AMST_FatherName { get; set; }
        public string AMST_MotherName { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        //public DateTime AMST_DOB { get; set; }
        public string AMST_PerCity { get; set; }
        public string AMST_PerAdd3 { get; set; }
        public string AMST_BPLCardNo { get; set; }
        public string ASTC_TCNO { get; set; }
       // public DateTime AMST_Date { get; set; }
        public string AMST_AdmNo { get; set; }

        public string columnName { get; set; }
        public string columnID { get; set; }


        public Array SectionList { get; set; }
        public Array academicList { get; set; }
        public Array classlist { get; set; }
        public Array studentList { get; set; }
        public Array monthList { get; set; }
       
        public long AMM_ID { get; set; }
        public int radiotype { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public Array studentAttendanceList { get; set; }
     
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
       
        public decimal ASA_ClassHeld { get; set; }
        public decimal ASA_Class_Attended { get; set; }
        public decimal percentage { get; set; }
    
        public int type { get; set; }
 
        public decimal classheld { get; set; }
        public decimal attendance { get; set; }
        public string ASMCL_className { get; set; }
    
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
      
        public string Nationality { get; set; }
        public string AMST_BirthPlace { get; set; }
        public string AMST_DOB_Words { get; set; }
        public DateTime AMST_DOB { get; set; }
        public DateTime AMST_Date { get; set; }
        public string ASTC_Conduct { get; set; }
      
        public string Religion { get; set; }
        public String caste { get; set; }
        public Array MasterCompany { get; set; }
        public string companyname { get; set; }
        public string type1 { get; set; }
        public string name { get; set; }



        //new


        public Array fillsec { get; set; }
        public Array studentlist { get; set; }

        public string regornamedetails { get; set; }

        public Array category_list { get; set; }
        public long AMC_Id { get; set; }
        public Array AMC_logo { get; set; }
        public bool categoryflag { get; set; }
        public categorylistarray1[] categorylistarray { get; set; }

    }
 
}
