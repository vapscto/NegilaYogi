using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Master_Institution_SubscriptionValidity")]
    public class Master_Institution_SubscriptionValidity : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MISV_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime? MISV_FromDate { get; set; }
        public DateTime? MISV_ToDate { get; set; }
        public string MISV_SubscriptionNo { get; set; }
        public string MISV_SubscriptionType { get; set; }
        public bool MISV_ActiveFlag { get; set; }


        [ForeignKey("MI_Id")]
        public virtual Institution Institution { get; set; }

    }
}
