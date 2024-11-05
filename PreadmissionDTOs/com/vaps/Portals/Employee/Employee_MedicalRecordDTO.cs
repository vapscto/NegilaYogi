using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class Employee_MedicalRecordDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public string flag { get; set; }
        public long IVRMRT_Id { get; set; }
        public long AMST_Id { get; set; }
        public string roletype { get; set; }
        public string flag_Type { get; set; }
        public Array staffarray { get; set; }
        public Array appliedgrid { get; set; }
        public Medicallist[] Medicallisttwo { get; set; }
        public string returnval { get; set; }
        public int count { get; set; }
        public long HREMR_Id { get; set; }
        public bool returnVal { get; set; }
        public string msg { get; set; }
        public Array attachementlist { get; set; }
        public long HRME_Id { get; set; }
        public string HREMRF_FileName { get; set; }
        public string HREMRF_FilePath { get; set; }
        public filelistMedical[] filelistMedical { get; set; }
    }
    public class Medicallist
    {
        public long HREMR_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime HREMR_TestDate { get; set; }
        public string HREMR_TestName { get; set; }
        public string HREMR_Remarks { get; set; }
        public filelistMedical[] filelistMedical { get; set; }

    }
    public class filelistMedical
    {
        public long HREMR_Id { get; set; }
        public string HREMRF_FileName { get; set; }
        public string HREMRF_FilePath { get; set; }
        public bool HREMRF_ActiveFlag { get; set; }
    }

  
}
