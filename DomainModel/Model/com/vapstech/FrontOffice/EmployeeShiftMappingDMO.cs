using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Emp_Shifts_Timings", Schema ="FO")]
    public class EmployeeShiftMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public int FOEST_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public int FOHWDT_Id { get; set; }
        public int FOMS_Id { get; set; }
        public string FOEST_FDWHrMin { get; set; }
        public string FOEST_HDWHrMin { get; set; }
        public string FOEST_IHalfLoginTime { get; set; }
        public string FOEST_IHalfLogoutTime { get; set; }
        public string FOEST_IIHalfLoginTime { get; set; }
        public string FOEST_IIHalfLogoutTime { get; set; }
        public string FOEST_DelayPerShiftHrMin { get; set; }
        public string FOEST_EarlyPerShiftHrMin { get; set; }
        public string FOEST_LunchHoursDuration { get; set; }
        public string FOEST_BlockAttendance { get; set; }
        public string FOEST_FixTimings { get; set; }
        public DateTime? FOEST_Date { get; set; }
        public long? FOEST_CreatedBy { get; set; }
        public long? FOEST_UpdatedBy { get; set; }


    }
}     
     
















       
