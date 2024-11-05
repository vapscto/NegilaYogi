using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_414_Budget_Comments")]
   public class NAAC_AC_414_Budget_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

      public long NCAC414BDC_Id { get; set; }
        public string NCAC414BDC_Remarks { get; set; }
        public long? NCAC414BDC_RemarksBy { get; set; }
        public string NCAC414BDC_StatusFlg { get; set; }
        public bool? NCAC414BDC_ActiveFlag { get; set; }
        public long? NCAC414BDC_CreatedBy { get; set; }
        public DateTime? NCAC414BDC_CreatedDate { get; set; }
        public long? NCAC414BDC_UpdatedBy { get; set; }
        public DateTime? NCAC414BDC_UpdatedDate { get; set; }
        public long NCAC414BD_Id { get; set; }
    }
}
