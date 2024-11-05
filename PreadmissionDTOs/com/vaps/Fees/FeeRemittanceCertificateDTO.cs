using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeRemittanceCertificateDTO
    {
        
        public long ASMAY_Id { get; set; }
        public long Adm_no_name { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }

        public long FMCC_Id { get; set; }

        public long ASMCL_Id { get; set; } // class
        public long ASMS_Id { get; set; }
        public long Stud_Id { get; set; }
        public long ASMC_Id { get; set; } //section
        public long ASMCC_Id { get; set; } //category
        public string Stud_Sel { get; set; }

        public Array DateList { get; set; }


        //  public int AMST_AdmNo { get; set; }
        public string AMST_AdmNo { get; set; }

        public string StudentName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public int? AMAY_RollNo { get; set; }
        public Array YearList { get; set; }
        public Array name_List { get; set; }
        public Array Fee_Group_List { get; set; }
        public Array Class_List { get; set; }
        public Array Section_List { get; set; }
        public Array Student_Name_List { get; set; }
        public string Student_Name { get; set; }
        public Array Class_Category_List { get; set; }
        public DateTime FYP_Date { get; set; }

        public string FHWR_ClassCategoryName { get; set; }
        public Array Fee_rem_cer_list { get; set; }
        public string Fee_Group { get; set; }
        public string Fee_Head { get; set; }
        public bool Active_Flag { get; set; }
        public string  Fine_Applicable{ get; set; }
        public string Installment { get; set; }

        public decimal Fine_Amount { get; set; }
        public decimal? To_Be_Paid_Amount { get; set; }
        public long fmG_Id { get; set; }

        public string radio_selected { get; set; }

        public string Father_Name { get; set; }
        public string Mother_Name { get; set; }

        public DateTime? Receipt_Date { get; set; }

        public string Receipt_Number { get; set; }
        public decimal Tuition_Fee_Amount { get; set; }
        public Array feehead_Name_list { get; set; }
    }
}
