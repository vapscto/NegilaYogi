using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_531_SportsCA_Activities")]
    public class NAAC_AC_531_SportsCA_ActivitiesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC533SPCAA_Id { get; set; }
        public long  MI_Id { get; set; }
        public long  NCAC533SPCAA_Year { get; set; }
        public long  NCAC533SPCAA_NoOfActivities { get; set; }
        public string  NCAC533SPCAA_FileDesc { get; set; }
        public string  NCAC533SPCAA_FileName { get; set; }
        public string  NCAC533SPCAA_FilePath { get; set; }
        public bool  NCAC533SPCAA_ActiveFlg { get; set; }
        public long  NCAC533SPCAA_CreatedBy { get; set; }
        public long  NCAC533SPCAA_UpdatedBy { get; set; }
        public DateTime  NCAC533SPCAA_CreatedDate { get; set; }
        public DateTime NCAC533SPCAA_UpdatedDate { get; set; }

    }
}
