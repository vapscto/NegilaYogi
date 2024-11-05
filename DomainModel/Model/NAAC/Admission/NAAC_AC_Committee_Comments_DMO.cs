using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_Committee_Comments")]
   public class NAAC_AC_Committee_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCACCOMMC_Id { get; set; }
        public string NCACCOMMC_Remarks { get; set; }
        public long? NCACCOMMC_RemarksBy { get; set; }
        public string NCACCOMMC_StatusFlg { get; set; }
        public bool? NCACCOMMC_ActiveFlag { get; set; }
        public long? NCACCOMMC_CreatedBy { get; set; }
        public DateTime? NCACCOMMC_CreatedDate { get; set; }
        public long? NCACCOMMC_UpdatedBy { get; set; }
        public DateTime? NCACCOMMC_UpdatedDate { get; set; }
        public long NCACCOMM_Id { get; set; }
    }
}
