using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_513_DevSchemes")]
    public class NAAC_AC_513_DevSchemesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC513INSCH_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC513INSCH_ImpYear { get; set; }
        public string NCAC513INSCH_DevSchemeName { get; set; }
        public string NCAC513INSCH_StatusFlg { get; set; }
        public long NCAC513INSCH_NoOfStudents { get; set; }
        public string NCAC513INSCH_AgencyDetails { get; set; }
        public bool NCAC513INSCH_ActiveFlg { get; set; }
        public long NCAC513INSCH_CreatedBy { get; set; }
        public long NCAC513INSCH_UpdatedBy { get; set; }
        public DateTime NCAC513INSCH_CreatedDate { get; set; }
        public DateTime NCAC513INSCH_UpdatedDate { get; set; }

        public List<NAAC_AC_513_DevSchemeFilesDMO> NAAC_AC_513_DevSchemeFilesDMO { get; set; }

    }
}
