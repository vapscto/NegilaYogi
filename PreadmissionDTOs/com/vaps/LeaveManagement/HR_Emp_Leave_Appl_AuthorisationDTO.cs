using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.LeaveManagement
{
    public class HR_Emp_Leave_Appl_AuthorisationDTO
    {
        public long HRELAPA_Id { get; set; }
        public long HRELAP_Id { get; set; }
        public long HRME_Id { get; set; }
        public string HRELAPA_SanctioningLevel { get; set; }
        public string HRELAPA_Remarks { get; set; }
        public bool HRELAPA_FinalFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long IVRMUL_Id { get; set; }
        public long HRML_Id { get; set; }
        public DateTime HRELAPA_FromDate { get; set; }
        public DateTime HRELAPA_ToDate { get; set; }
        public int HRELAPA_TotalDays { get; set; }
        public string HRELAPA_LeaveStatus { get; set; }
    }
}
