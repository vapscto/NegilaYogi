using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Master_TimeSettings",Schema ="FO")]
    public class MasterTimeSettingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FOMTS_Id { get; set; }
        public long MI_Id { get; set; }
        public string FOMTS_FDWHrMin { get; set; }
        public string FOMTS_HDWHrMin { get; set; }
        public string FOMTS_IHalfLoginTime { get; set; }
        public string FOMTS_IhalfLogoutTime { get; set; }
        public string FOMTS_IIHalfLoginTime { get; set; }
        public string FOMTS_IIHalfLogoutTime { get; set; }
        public string FOMTS_DelayPerShiftHrMin { get; set; }
        public string FOMTS_EarlyPerShiftHrMin { get; set; }
        public string FOMTS_LunchHoursDuration { get; set; }
        public string FOMTS_BlockAttendance { get; set; }
        public string FOMTS_FixTimings { get; set; }
        public bool FOMHWD_ActiveFlg { get; set; }
    }
}     
     
















       
