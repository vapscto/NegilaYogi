using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7117_UniversalValues_File_Comments")]
   public class NAAC_AC_7117_UniversalValues_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7117UNIVALFC_Id { get; set; }
        public string NCAC7117UNIVALFC_Remarks { get; set; }
        public long? NCAC7117UNIVALFC_RemarksBy { get; set; }
        public bool? NCAC7117UNIVALFC_ActiveFlag { get; set; }
        public long? NCAC7117UNIVALFC_CreatedBy { get; set; }
        public DateTime? NCAC7117UNIVALFC_CreatedDate { get; set; }
        public long? NCAC7117UNIVALFC_UpdatedBy { get; set; }
        public DateTime? NCAC7117UNIVALFC_UpdatedDate { get; set; }
        public string NCAC7117UNIVALFC_StatusFlg { get; set; }
        public long NCAC7117UNIVALF_Id { get; set; } 
    }
}
