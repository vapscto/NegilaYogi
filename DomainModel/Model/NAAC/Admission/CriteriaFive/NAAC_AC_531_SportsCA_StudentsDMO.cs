using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_531_SportsCA_Students")]
    public class NAAC_AC_531_SportsCA_StudentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long  NCAC531SPCAS_Id { get; set; }
        public long  MI_Id { get; set; }
        public long  NCAC531SPCA_Id { get; set; }
        public long  AMCST_Id { get; set; }
        public string  NCAC531SPCAS_AwardName { get; set; }
        public string NCAC531SPCAS_NatOrInterNatFlg { get; set; }
        public string NCAC531SPCAS_SportsCAIEEEFlg { get; set; }
        public string NCAC531SPCAS_StatusFlg { get; set; }
        public bool NCAC531SPCAS_ActiveFlg { get; set; }
        public long NCAC531SPCAS_CreatedBy { get; set; }
        public long NCAC531SPCAS_UpdatedBy { get; set; }
        public DateTime NCAC531SPCAS_CreatedDate { get; set; }
        public DateTime NCAC531SPCAS_UpdatedDate { get; set; }

    }
}
