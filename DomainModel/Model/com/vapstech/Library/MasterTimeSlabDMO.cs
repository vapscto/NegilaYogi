using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Fine_Setting", Schema = "LIB")]
    public class MasterTimeSlabDMO :CommonParamDMO
    {
        [Key]
        public long LFSE_Id { get; set; }
        public long MI_Id { get; set; }
       // public long LMC_Id { get; set; }
        public string LFSE_UserType { get; set; }
        public string LFSE_SlabTypeFlg { get; set; }
        public bool LFSE_PerDayFlg { get; set; }
        public int LFSE_FromDay { get; set; }
        public int? LFSE_ToDay { get; set; }
        public decimal LFSE_Amount { get; set; }
        public bool LFSE_ActiveFlg { get; set; }

    }
}
