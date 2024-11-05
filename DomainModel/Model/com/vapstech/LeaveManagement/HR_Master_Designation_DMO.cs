using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.LeaveManagement
{
    [Table("HR_Master_Designation")]
    public class HR_Master_Designation_DMO :CommonParamDMO
    {     
        public long HRMDES_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public decimal HRMDES_BasicAmount { get; set; }
        public int HRMDES_SanctionedSeats { get; set; }
        public bool HRMDES_DisplaySanctionedSeatsFlag { get; set; }
        public int HRMDES_Order { get; set; }
        public bool HRMDES_ActiveFlag { get; set; }
    }
}
