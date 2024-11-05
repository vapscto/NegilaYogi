using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Master_Shifts_Timings", Schema = "FO")]
    public class MasterShiftsTimingsDMO : CommonParamDMO
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int FOMST_Id { get; set; }
        public int FOMS_Id { get; set; }
        public int FOHWDT_Id { get; set; }
        public string FOMST_FDWHrMin { get; set; }
        public string FOMST_HDWHrMin { get; set; }
        public string FOMST_IHalfLoginTime { get; set; }
        public string FOMST_IHalfLogoutTime { get; set; }
        public string FOMST_IIHalfLoginTime { get; set; }
        public string FOMST_IIHalfLogoutTime { get; set; }
        public string FOMST_DelayPerShiftHrMin { get; set; }
        public string FOMST_EarlyPerShiftHrMin { get; set; }
        public string FOMST_LunchHoursDuration { get; set; }
        public string FOMST_BlockAttendance { get; set; }
        public string FOMST_FixTimings { get; set; }
    }
}     
     
















       
