using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_521_Placement_Files")]
    public class NAAC_AC_521_PlacementFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC521PLAF_Id { get; set; }
        public long NCAC521PLA_Id { get; set; }
    
        public string NCAC521PLAF_FileName { get; set; }
        public string NCAC521PLAF_Filedesc { get; set; }
        public string NCAC521PLAF_FilePath { get; set; }
        public string NCAC521PLAF_StatusFlg { get; set; }
        public bool? NCAC521PLAF_ActiveFlg { get; set; }

        




    }
}
