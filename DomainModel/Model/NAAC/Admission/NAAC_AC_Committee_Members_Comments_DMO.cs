using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Committee_Members_Comments")]
   public class NAAC_AC_Committee_Members_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACCOMMMC_Id { get; set; }
        public string NCACCOMMMC_Remarks { get; set; }
        public long? NCACCOMMMC_RemarksBy { get; set; }
        public string NCACCOMMMC_StatusFlg { get; set; }
        public bool? NCACCOMMMC_ActiveFlag { get; set; }
        public long? NCACCOMMMC_CreatedBy { get; set; }
        public DateTime? NCACCOMMMC_CreatedDate { get; set; }
        public long? NCACCOMMMC_UpdatedBy { get; set; }
        public DateTime? NCACCOMMMC_UpdatedDate { get; set; }
        public long NCACCOMMM_Id { get; set; }
    }
}
