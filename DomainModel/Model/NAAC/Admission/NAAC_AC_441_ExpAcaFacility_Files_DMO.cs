using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_441_ExpAcaFacility_Files")]
    public class NAAC_AC_441_ExpAcaFacility_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC441ExAcFcF_Id { get; set; }
        public string NCAC441ExAcFcF_FileName { get; set; }
        public string NCAC441ExAcFcF_Filedesc { get; set; }
        public string NCAC441ExAcFcF_FilePath { get; set; }
        public long NCAC441ExAcFc_Id { get; set; }
        public string NCAC441ExAcFcF_StatusFlg { get; set; }
        public bool NCAC441ExAcFcF_ActiveFlg { get; set; }
    }
}
