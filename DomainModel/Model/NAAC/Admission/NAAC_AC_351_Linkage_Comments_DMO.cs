using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_351_Linkage_Comments")]
   public class NAAC_AC_351_Linkage_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

       public long NCAC351LINC_Id { get; set; }
       public string NCAC351LINC_Remarks { get; set; }
        public long? NCAC351LINC_RemarksBy { get; set; }
        public string NCAC351LINC_StatusFlg { get; set; }
        public bool? NCAC351LINC_ActiveFlag { get; set; }
        public long? NCAC351LINC_CreatedBy { get; set; }
        public DateTime? NCAC351LINC_CreatedDate { get; set; }
        public long? NCAC351LINC_UpdatedBy { get; set; }
        public DateTime? NCAC351LINC_UpdatedDate { get; set; }
        public long NCAC351LIN_Id { get; set; }
    }
}
