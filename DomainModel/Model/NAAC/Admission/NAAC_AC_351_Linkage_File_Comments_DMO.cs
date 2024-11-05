using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_351_Linkage_File_Comments")]
   public class NAAC_AC_351_Linkage_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

       public long NCAC351LINFC_Id { get; set; }
        public string NCAC351LINFC_Remarks { get; set; }
        public long? NCAC351LINFC_RemarksBy { get; set; }
        public bool? NCAC351LINFC_ActiveFlag { get; set; }
        public long? NCAC351LINFC_CreatedBy { get; set; }
        public DateTime? NCAC351LINFC_CreatedDate { get; set; }
        public long? NCAC351LINFC_UpdatedBy { get; set; }
        public DateTime? NCAC351LINFC_UpdatedDate { get; set; }
        public string NCAC351LINFC_StatusFlg { get; set; }
        public long NCAC351LINF_Id { get; set; }
    }
}
