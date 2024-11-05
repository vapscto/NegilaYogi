using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Emp_Punch", Schema ="FO")]
    public class FO_Emp_PunchDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FOEP_Id { get; set; }

        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public DateTime? FOEP_PunchDate { get; set; }
        public bool FOEP_HolidayPunchFlg { get; set; }
        public bool FOEP_Flag { get; set; }
        public List<FO_Emp_Punch_DetailsDMO> FO_Emp_Punch_DetailsDMO { get; set; }


    }
}     
     
















       
