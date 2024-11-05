using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_714_LEDBulbs")]
    public class NAAC_AC_714_LEDBulbsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC714LEDBU_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC714LEDBU_Year { get; set; }
        public string NCAC714LEDBU_LightingsRequirements { get; set; }
        public string NCAC714LEDBU_LughtingLED { get; set; }
        public string NCAC714LEDBU_OtherSource { get; set; }
        public bool NCAC714LEDBU_ActiveFlg { get; set; }
        public long NCAC714LEDBU_CreatedBy { get; set; }
        public long NCAC714LEDBU_UpdatedBy { get; set; }
        public DateTime? NCAC714LEDBU_CreatedDate { get; set; }
        public DateTime? NCAC714LEDBU_UpdatedDate { get; set; }
        public string NCAC714LEDBU_StatusFlg { get; set; }
    }
}
