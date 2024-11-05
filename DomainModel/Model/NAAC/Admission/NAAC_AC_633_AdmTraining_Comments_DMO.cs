using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_633_AdmTraining_Comments")]
    public class NAAC_AC_633_AdmTraining_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC633ADMTRGC_Id { get; set; }
        public string NCAC633ADMTRGC_Remarks { get; set; }
        public long? NCAC633ADMTRGC_RemarksBy { get; set; }
        public string NCAC633ADMTRGC_StatusFlg { get; set; }
        public bool? NCAC633ADMTRGC_ActiveFlag { get; set; }
        public long NCAC633ADMTRGC_CreatedBy { get; set; }
        public DateTime? NCAC633ADMTRGC_CreatedDate { get; set; }
        public long? NCAC633ADMTRGC_UpdatedBy { get; set; }
        public DateTime? NCAC633ADMTRGC_UpdatedDate { get; set; }
        public long NCAC633ADMTRG_Id { get; set; }

    }
}
