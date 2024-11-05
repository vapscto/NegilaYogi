using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_7116_StatutoryBodies")]
    public class NAAC_AC_7116_StatutoryBodiesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7116STABOD_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC7116STABOD_Year { get; set; }
        public string NCAC7116STABOD_URL { get; set; }
        public bool NCAC7116STABOD_ActiveFlg { get; set; }
        public long NCAC7116STABOD_CreatedBy { get; set; }
        public long NCAC7116STABOD_UpdatedBy { get; set; }
        public DateTime NCAC7116STABOD_CreatedDate { get; set; }
        public DateTime NCAC7116STABOD_UpdatedDate { get; set; }
        public string NCAC7116STABOD_StatusFlg { get; set; }
    }
}
