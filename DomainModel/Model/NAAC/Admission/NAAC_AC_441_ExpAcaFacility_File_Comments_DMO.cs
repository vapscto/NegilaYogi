using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_441_ExpAcaFacility_File_Comments")]
   public class NAAC_AC_441_ExpAcaFacility_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public long NCAC441EXACFCFC_Id { get; set; }
      public string NCAC441EXACFCFC_Remarks { get; set; }
        public long? NCAC441EXACFCFC_RemarksBy { get; set; }
        public bool? NCAC441EXACFCFC_ActiveFlag { get; set; }
        public long? NCAC441EXACFCFC_CreatedBy { get; set; }
        public DateTime? NCAC441EXACFCFC_CreatedDate { get; set; }
        public long? NCAC441EXACFCFC_UpdatedBy { get; set; }
        public DateTime? NCAC441EXACFCFC_UpdatedDate { get; set; }
        public string NCAC441EXACFCFC_StatusFlg { get; set; }
        public long NCAC441EXACFCF_Id { get; set; }
    }
}
