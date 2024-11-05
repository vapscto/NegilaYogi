using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class SportMasterHouseDTO :CommonParamDTO
    {
        public long SPCCSH_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public DateTime? SPCCSH_AsOnDate { get; set; }
        public long ASMS_Id { get; set; }       
        public long SPCCMH_Id { get; set; }
        public long AMST_Id { get; set; }
        public int? SPCCSH_Age { get; set; }
        public decimal? SPCCSH_Height { get; set; }
        public decimal? SPCCSH_Weight { get; set; }
        public bool SPCCMH_ActiveFlag { get; set; }

        public decimal? SPCCSH_BMI { get; set; }
        public string SPCCSH_BMIRemarks { get; set; }   
        public string SPCCMH_HouseName { get; set; }
        public string SPCCMH_HouseDescription { get; set; }        
        public long SPCCMD_Id { get; set; }       
        public string studentname { get; set; }
        public Array yearlist { get; set; }      
        public long SPCCMHC_Id { get; set; }
        public string SPCCMD_DivisionName { get; set; }
        public long ASMST_Id { get; set; }
        public long SPCCMHC_ContactNo { get; set; }
        public string SPCCMHC_EmailId { get; set; }
        public bool SPCCMHC_ActiveFlag { get; set; }
        public long SPCCMHD_Id { get; set; }
        public string SPCCMHD_DesignationName { get; set; }
        public string SPCCMHD_DesignationDescription { get; set; }
        public bool SPCCMHD_ActiveFlag { get; set; }
      
        public Array masterhousename { get; set; }
        public Array GridviewDetails { get; set; }
        public int count { get; set; }
        public string msg { get; set; }
        public bool returnVal { get; set; }
        public bool returnVal_update { get; set; }
        public bool duplicate_caste_name_bool { get; set; }

        public Array houseList { get; set; }
        public Array DesignationList { get; set; }
        public Array ClassList { get; set; }
        public Array SectionList { get; set; }
        public Array StudentList { get; set; }
        public SportMasterHouseDTO[] StudList { get; set; }
        public Array std1 { get; set; }
        public string ASMAY_Year { get; set; }

        public Array classList { get; set; }

        public decimal? SPCCMHD_BMI { get; set; }
        public string SPCCMHD_BMI_Remark { get; set; }

    }
}
