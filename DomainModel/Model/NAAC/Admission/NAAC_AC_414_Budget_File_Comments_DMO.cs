using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_414_Budget_File_Comments")]
   public class NAAC_AC_414_Budget_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

       public long NCAC414BDFC_Id { get;set; }
        public string NCAC414BDFC_Remarks { get; set; }
        public long? NCAC414BDFC_RemarksBy { get; set; }
        public bool? NCAC414BDFC_ActiveFlag { get; set; }
        public long? NCAC414BDFC_CreatedBy { get; set; }
        public DateTime? NCAC414BDFC_CreatedDate { get; set; }
        public long? NCAC414BDFC_UpdatedBy { get; set; }
        public DateTime? NCAC414BDFC_UpdatedDate { get; set; }
        public string NCAC414BDFC_StatusFlg { get; set; }
        public long NCAC414BDF_Id { get; set; }
    }
}
