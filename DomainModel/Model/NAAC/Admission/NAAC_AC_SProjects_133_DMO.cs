using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_SProjects_133")]
    public class NAAC_AC_SProjects_133_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACSPR133_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string NCACSPR133_ProjectName { get; set; }
        public bool NCACSPR133_ActiveFlg { get; set; }
        public long NCACSPR133_CreatedBy { get; set; }
        public long NCACSPR133_UpdatedBy { get; set; }
        public DateTime NCACSPR133_CreatedDate { get; set; }
        public DateTime NCACSPR133_UpdatedDate { get; set; }
        public long NCACSPR133_Year { get; set; }
        public string NCACSPR133_StatusFlg { get; set; }
        public bool? NCACSPR133_ApprovedFlg { get; set; }
        public string NCACSPR133_Remarks { get; set; }
        public bool? NCACSPR133_FromExelImportFlag { get; set; }
        public bool? NCACSPR133_FreezeFlag { get; set; }
        public List<NAAC_AC_SProjects_133_FilesDMO> NAAC_AC_SProjects_133_FilesDMO { get; set; }
    }
}
