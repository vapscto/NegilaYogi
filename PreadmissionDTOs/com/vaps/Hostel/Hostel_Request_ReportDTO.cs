using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
  public  class Hostel_Request_ReportDTO
    {
       

        public long HLHSREQ_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime HLHSREQ_RequestDate { get; set; }
        public long HLMH_Id { get; set; }
        public long HLMRCA_Id { get; set; }
        public long AMST_Id { get; set; }
        public bool HLHSREQ_ACRoomFlg { get; set; }
        public bool HLHSREQ_SingleRoomFlg { get; set; }
        public bool HLHSREQ_VegMessFlg { get; set; }
        public bool HLHSREQ_NonVegMessFlg { get; set; }
        public string HLHSREQ_Remarks { get; set; }
        public string HLHSREQ_BookingStatus { get; set; }
        public bool HLHSREQ_ActiveFlag { get; set; }
        public DateTime? HLHSREQ_CreatedDate { get; set; }
        public DateTime? HLHSREQ_UpdatedDate { get; set; }
        public long HLHSREQ_CreatedBy { get; set; }
        public long HLHSREQ_UpdatedBy { get; set; }


        public long HLHSTREQ_Id { get; set; }
        //public long MI_Id { get; set; }
        public DateTime HLHSTREQ_RequestDate { get; set; }
        //public long HLMH_Id { get; set; }
        //public long HLMRCA_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool HLHSTREQ_ACRoomFlg { get; set; }
        public bool HLHSTREQ_SingleRoomFlg { get; set; }
        public bool HLHSTREQ_VegMessFlg { get; set; }
        public bool HLHSTREQ_NonVegMessFlg { get; set; }
        public string HLHSTREQ_Remarks { get; set; }
        public string HLHSTREQ_BookingStatus { get; set; }
        public bool HLHSTREQ_ActiveFlag { get; set; }
        public DateTime? HLHSTREQ_CreatedDate { get; set; }
        public DateTime? HLHSTREQ_UpdatedDate { get; set; }
        public long? HLHSTREQ_CreatedBy { get; set; }
        public long? HLHSTREQ_UpdatedBy { get; set; }

        public Array griddata { get; set; }
        public string issuertype1 { get; set; }
        public string Type { get; set; }
        public string frmdate { get; set; }
        public string todate { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_MiddleName { get; set; }
        public string AMST_LastName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string HLMH_Name { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string HRME_EmployeeLastName { get; set; }
        public string HRME_EmployeeMiddleName { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string HRMDES_DesignationName { get; set; }

        //================Confirm 
        public long HLHSREQC_Id { get; set; }
        public string HLHSREQC_Remarks { get; set; }
        public string HLHSREQC_BookingStatus { get; set; }
        public DateTime HLHSREQC_Date { get; set; }
        public long HLHSTREQC_Id { get; set; }
        public string HLHSTREQC_Remarks { get; set; }
        public string HLHSTREQC_BookingStatus { get; set; }
        public DateTime HLHSTREQC_RequestDate { get; set; }

        public string ctype { get; set; }
    }
}
