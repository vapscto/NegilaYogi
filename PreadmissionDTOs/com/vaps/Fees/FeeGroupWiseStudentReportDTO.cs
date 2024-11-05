using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeGroupWiseStudentReportDTO
    {
        
        public long ASMAY_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long FMCC_Id { get; set; }
        public long ASMCL_Id { get; set; } // class
        public long ASMS_Id { get; set; }
        public long Stud_Id { get; set; }
        public string Stud_Sel { get; set; }
        public long FMG_Id { get; set; }
        public int AMST_AdmNo { get; set; }

        public string StudentName { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public long? AMAY_RollNo { get; set; }
        public Array YearList { get; set; }
        public Array Fee_Group_List { get; set; }
        public Array Class_List { get; set; }
        public Array Section_List { get; set; }
        public Array Student_Name_List { get; set; }
        public string Student_Name { get; set; }
        public Array Class_Category_List { get; set; }


        public string FHWR_ClassCategoryName { get; set; }
        public Array FHWR_searchdatalist { get; set; }
        public string Fee_Group { get; set; }
        public string Fee_Head { get; set; }
        public bool Active_Flag { get; set; }
        public string  Fine_Applicable{ get; set; }
        public string Installment { get; set; }

        public decimal Fine_Amount { get; set; }
        public decimal? To_Be_Paid_Amount { get; set; }
       

        public string radio_selected { get; set; }

       public long user_id { get; set; }
        
        public Listfeegroup[] selectfeegroup { get; set; }


        public Array paiddatelist { get; set; }

    }
    
    public class Listfeegroup
    {

        public long fmG_Id { get; set; }
    }
    
}
