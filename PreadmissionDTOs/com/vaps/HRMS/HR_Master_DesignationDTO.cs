using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.HRMS
{
    public class HR_Master_DesignationDTO :CommonParamDTO
    {
        public long HRMDES_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public decimal HRMDES_BasicAmount { get; set; }
        public Int32? HRMDES_SanctionedSeats { get; set; }
        public bool? HRMDES_DisplaySanctionedSeatsFlag { get; set; }
        public Int32? HRMDES_Order { get; set; }
        public bool HRMDES_ActiveFlag { get; set; }
        public Array designationlList { get; set; }
        public string retrunMsg { get; set; }

        public long roleId { get; set; }
        public HR_Master_DesignationDTO[] DesignationDTO { get; set; }
    }
}
