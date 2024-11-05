using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.FrontOffice
{
    [Table("FO_Master_HolidayWorkingDay", Schema = "FO")]
    public class MasterHolidayDMO : CommonParamDMO
    {
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int FOMHWD_Id { get; set; }
        public long MI_Id { get; set; }
       // [ForeignKey("FOHWDT_Id")]
        public int FOHWDT_Id { get; set; }
        public string FOMHWD_HolidayWDName { get; set; }
     //   public string FOMHWD_CalenderYear { get; set; }
        public bool FOMHWD_ActiveFlg { get; set; }
      
    }
}
