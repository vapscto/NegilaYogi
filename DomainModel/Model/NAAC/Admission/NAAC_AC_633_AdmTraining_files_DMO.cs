using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_633_AdmTraining_files")]
    public class NAAC_AC_633_AdmTraining_files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC633ADMTRGF_Id { get; set; }
        [ForeignKey("NCAC633ADMTRG_Id")]
        public long NCAC633ADMTRG_Id { get; set; }
        public string NCAC633ADMTRGF_FilePath { get; set; }
        public string NCAC633ADMTRGF_FileName { get; set; }
        public string NCAC633ADMTRGF_Filedesc { get; set; }
        public string NCAC633ADMTRGF_StatusFlg { get; set; }
        public bool? NCAC633ADMTRGF_ActiveFlg { get; set; }
        public bool? NCAC633ADMTRGF_ApprovedFlg { get; set; }
        public string NCAC633ADMTRGF_Remarks { get; set; }


    }
}
