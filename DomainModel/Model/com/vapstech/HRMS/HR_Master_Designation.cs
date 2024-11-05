using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Designation")]
    public class HR_Master_Designation:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMDES_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public decimal HRMDES_BasicAmount { get; set; }
        public Int32? HRMDES_SanctionedSeats { get; set; }
        public bool? HRMDES_DisplaySanctionedSeatsFlag { get; set; }
        public Int32? HRMDES_Order { get; set; }
        public bool HRMDES_ActiveFlag { get; set; }              
        public long? HRMDC_ID { get; set; }              

    }
}
