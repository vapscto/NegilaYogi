using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
    public class BMICalculationDTO :CommonParamDTO
    {      
        public long SPCCSHW_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime SPCCSHW_AsOnDate { get; set; }
        public decimal? SPCCSHW_Height { get; set; }
        public decimal? SPCCSHW_Weight { get; set; }
        public decimal? SPCCSHW_BMI { get; set; }
        public string SPCCSHW_BMIRemark { get; set; }
        public bool SPCCMHW_ActiveFlag { get; set; }
        public BMICalculationDTO[] student { get; set; }
        public string returnVal { get; set; }
        public Array academicYear { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public string studentName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string stringmobileorportal { get; set; }
        public string mobileprivileges { get; set; }
        public Array studentList { get; set; }
        public Array viewlist { get; set; }
        public Array Staffmobileappprivileges { get; set; }
        public long SPCCMH_Id { get; set; }
        public string Type { get; set; }
        public Array houseList { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public Array studentrecord { get; set; }
        public bool returnVal2 { get; set; }
        public Array editlist { get; set; }
        public string ASMAY_Year { get; set; }
        public long HRME_Id { get; set; }
        public Array classlist { get; set; }
        public long UserId { get; set; }
        public Array editchecklist { get; set; }
        public Array viewdetails { get; set; }
        public BMICalculationDTO[] selectedSectionlist { get; set; }
        public string Pagename { get; set; }
        public string Pageicon { get; set; }
        public string Pageurl { get; set; }
        public long? IVRMRMAP_Id { get; set; }
        public bool? IVRMMAP_AddFlg { get; set; }
        public bool? IVRMMAP_UpdateFlg { get; set; }
        public bool? IVRMMAP_DeleteFlg { get; set; }
        public int roleid { get; set; }
        public string asondate { get; set; }

    }
}
