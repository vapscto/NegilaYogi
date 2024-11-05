using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
   public class House_Committe_Report_DTO:CommonParamDTO
    {


        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array houseList { get; set; }
        public Array viewlist { get; set; }
        public long SPCCMH_Id { get; set; }

        public long SPCCMHC_Id { get; set; }
        public string SPCCMD_DivisionName { get; set; }
        public long ASMST_Id { get; set; }
        public long SPCCMHC_ContactNo { get; set; }
        public string SPCCMHC_EmailId { get; set; }
        public string SPCCMHD_DesignationName { get; set; }
        public string SPCCMH_HouseName { get; set; }
        public string studentname { get; set; }

        public string AMST_AdmNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ASMAY_Year { get; set; }
        public Array asmay_list { get; set; }

        public House_Committe_Report_DTO[] selectedhouselist { get; set; }

    }
}
