using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_714_LEDBulbs_File_Comments")]
   public class NAAC_AC_714_LEDBulbs_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCAC714LEDBUFC_Id { get; set; }
        public string NCAC714LEDBUFC_Remarks { get; set; }
        public long? NCAC714LEDBUFC_RemarksBy { get; set; }
        public bool? NCAC714LEDBUFC_ActiveFlag { get; set; }
        public long? NCAC714LEDBUFC_CreatedBy { get; set; }
        public DateTime? NCAC714LEDBUFC_CreatedDate { get; set; }
        public long? NCAC714LEDBUFC_UpdatedBy { get; set; }
        public DateTime? NCAC714LEDBUFC_UpdatedDate { get; set; }
        public string NCAC714LEDBUFC_StatusFlg { get; set; }
        public long NCAC714LEDBUF_Id { get; set; }
    }
}
