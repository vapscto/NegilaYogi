using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_441_ExpAcaFacility")]
    public class NAAC_AC_441_ExpAcaFacility_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC441ExAcFc_Id { get; set; }
        public long MI_Id { get; set; }
        public Nullable<decimal> NCAC441EXACFC_ExpAccFacility { get; set; }
        public Nullable<decimal> NCAC441EXACFC_ExpPhyFacility { get; set; }
        public long NCAC441EXACFC_Year { get; set; }
        //public string NCAC441EXACFC_FileName { get; set; }
        //public string NCAC441EXACFC_FilePath { get; set; }
        public Nullable<bool> NCAC441EXACFC_ActiveFlg { get; set; }
        public Nullable<long> NCAC441EXACFC_CreatedBy { get; set; }
        public Nullable<long> NCAC441EXACFC_UpdatedBy { get; set; }
        public DateTime NCAC441EXACFC_CreatedDate { get; set; }
        public DateTime NCAC441EXACFC_UpdatedDate { get; set; }
        public string NCAC441ExAcFc_StatusFlg { get; set; }
        public List<NAAC_AC_441_ExpAcaFacility_Files_DMO> NAAC_AC_441_ExpAcaFacility_Files_DMO { get; set; }
    }
}
