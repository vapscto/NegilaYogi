using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_633_AdmTraining_File_Comments")]
    public class NAAC_AC_633_AdmTraining_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC633ADMTRGFC_Id { get; set; }
        public string NCAC633ADMTRGFC_Remarks { get; set; }
        public long? NCAC633ADMTRGFC_RemarksBy { get; set; }
        public bool? NCAC633ADMTRGFC_ActiveFlag { get; set; }
        public long? NCAC633ADMTRGFC_CreatedBy { get; set; }
        public DateTime? NCAC633ADMTRGFC_CreatedDate { get; set; }
        public long? NCAC633ADMTRGFC_UpdatedBy { get; set; }
        public DateTime? NCAC633ADMTRGFC_UpdatedDate { get; set; }
        public string NCAC633ADMTRGFC_StatusFlg { get; set; }
        public long NCAC633ADMTRGF_Id { get; set; }

    }
}
