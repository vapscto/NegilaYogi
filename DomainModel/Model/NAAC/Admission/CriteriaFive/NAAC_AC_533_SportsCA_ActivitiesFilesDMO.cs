using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_533_SportsCA_Activities_Files")]
    public class NAAC_AC_533_SportsCA_ActivitiesFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC533SPCAAF_Id { get; set; }
        public long NCAC533SPCAA_Id { get; set; }
       
        public string NCAC533SPCAAF_FileName { get; set; }
        public string NCAC533SPCAAF_Filedesc { get; set; }
        public string NCAC533SPCAAF_FilePath { get; set; }
        public string NCAC533SPCAAF_StatusFlg { get; set; }
        public bool? NCAC533SPCAAF_ActiveFlg { get; set; }

        

    }
}
