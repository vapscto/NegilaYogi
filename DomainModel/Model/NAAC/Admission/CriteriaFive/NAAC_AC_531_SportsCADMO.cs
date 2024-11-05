using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_531_SportsCA")]
    public class NAAC_AC_531_SportsCADMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC531SPCA_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC531SPCA_Year { get; set; }
        public long NCAC531SPCA_NoOfStudents { get; set; }
        public bool NCAC531SPCA_ActiveFlg { get; set; }
        public long NCAC531SPCA_CreatedBy { get; set; }
        public long NCAC531SPCA_UpdatedBy { get; set; }
        public string NCAC531SPCA_StatusFlg { get; set; }
        public DateTime NCAC531SPCA_CreatedDate { get; set; }
        public DateTime NCAC531SPCA_UpdatedDate { get; set; }
        public List<NAAC_AC_531_SportsCA_StudentsDMO> NAAC_AC_531_SportsCA_StudentsDMO { get; set; }
        public List<NAAC_AC_531_SportsCAFilesDMO> NAAC_AC_531_SportsCAFilesDMO { get; set; }

    }
}
