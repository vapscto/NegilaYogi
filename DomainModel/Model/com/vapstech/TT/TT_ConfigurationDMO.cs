using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Configuration")]
    public class TT_ConfigurationDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTC_Id { get; set; }

        public long MI_Id { get; set; }
        public bool TTC_StaffwiseContiniousPeriods { get; set; }
        public bool TTC_CTConstraintFlg { get; set; }

        public int TTC_CTConstraintNoOfDays { get; set; }

        public bool TTC_MaxMinPeriodCheckingFlg { get; set; }
    }
}
