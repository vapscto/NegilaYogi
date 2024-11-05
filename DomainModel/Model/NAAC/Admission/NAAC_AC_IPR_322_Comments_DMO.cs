using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_IPR_322_Comments")]
   public class NAAC_AC_IPR_322_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACIPR322C_Id { get; set; }
        public string NCACIPR322C_Remarks { get; set; }
        public long? NCACIPR322C_RemarksBy { get; set; }
        public string NCACIPR322C_StatusFlg { get; set; }
        public bool? NCACIPR322C_ActiveFlag { get; set; }
        public long? NCACIPR322C_CreatedBy { get; set; }
        public DateTime? NCACIPR322C_CreatedDate { get; set; }
        public long? NCACIPR322C_UpdatedBy { get; set; }
        public DateTime? NCACIPR322C_UpdatedDate { get; set; }
        public long NCACIPR322_Id { get; set; }
    }
}
