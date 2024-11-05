using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Sports
{
   public class SPCC_Master_House_Staff_DTO:CommonParamDTO
    {
        public long SPCCMHS_Id { get; set; }
        public long MI_Id { get; set; }
        public long SPCCMH_Id { get; set; }
        public long HRME_Id { get; set; }
      
        public string SPCCMHS_Description { get; set; }
        public bool SPCCMHS_ActiveFlag { get; set; }
        public long ASMAY_Id { get; set; }

        public long? HRMD_Id { get; set; }
        public long? HRMDES_Id { get; set; }

        public Array yearlist { get; set; }
        public Array filldesignation { get; set; }
        public Array houseList { get; set; }
        public Array emplist { get; set; }
        public bool returnval { get; set; }
        public bool dulicate { get; set; }
        public string empname { get; set; }
        public int? HRME_EmployeeOrder { get; set; }
        public Array alldata { get; set; }
        public string ASMAY_Year { get; set; }
        public string SPCCMH_HouseName { get; set; }   
        public string empcode { get; set; }
        public Array editdata { get; set; }
        public Array filldepartment { get; set; }
        //public long HRMEMNO_MobileNo { get; set; }
        public string mobileNo { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }


        public SPCC_Master_House_Staff_DTO[] emplstdata { get; set; }

    }
}
