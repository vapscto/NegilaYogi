using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_423_Memberships_Comments")]
   public class NAAC_AC_423_Memberships_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC423MEMC_Id { get; set; }
        public string NCAC423MEMC_Remarks { get; set; }
        public long? NCAC423MEMC_RemarksBy { get; set; }
        public string NCAC423MEMC_StatusFlg { get; set; }
        public bool? NCAC423MEMC_ActiveFlag { get; set; }
        public long? NCAC423MEMC_CreatedBy { get; set; }
        public DateTime? NCAC423MEMC_CreatedDate { get; set; }
        public long? NCAC423MEMC_UpdatedBy { get; set; }
        public DateTime? NCAC423MEMC_UpdatedDate { get; set; }
        public long NCAC423MEM_Id { get; set; }
    }
}
