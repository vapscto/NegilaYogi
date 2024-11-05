using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
   public class SPCC_Master_House_Committe_DTO:CommonParamDTO
    {

        public long SPCCMHC_Id { get; set; }
        public long MI_Id { get; set; }
        public long SPCCMHD_Id { get; set; }
        public long SPCCMH_Id { get; set; }
        public long AMST_Id { get; set; }
        public long? SPCCMHC_ContactNo { get; set; }
        public string SPCCMHC_EmailId { get; set; }
        public bool SPCCMHD_ActiveFlag { get; set; }


        public Array masterhousename { get; set; }
        public Array yearlist { get; set; }
        public Array DesignationList { get; set; }
        public string SPCCMHD_DesignationName { get; set; }
        public Array houseList { get; set; }
        public string SPCCMH_HouseName { get; set; }
        public Array ClassList { get; set; }
        public long ASMAY_Id { get; set; }
        public Array GridviewDetails { get; set; }
        public string studentname { get; set; }
        public int count { get; set; }
        public string msg { get; set; }
        public bool returnVal { get; set; }
        public bool returnVal_update { get; set; }
        public bool duplicate_caste_name_bool { get; set; }
        public Array SectionList { get; set; }
        public Array StudentList { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public Array studentList { get; set; }
        public string studentName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public int ASMCL_Order { get; set; }
        public int ASMC_Order { get; set; }


    }
}
