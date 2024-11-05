using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_533_SportsCA_Activities_Comments")]
    public class NAAC_AC_533_SportsCA_Activities_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        
        public long NCAC533SPCAAC_Id { get; set; }
        public string  NCAC533SPCAAC_Remarks { get; set; }
        public long  NCAC533SPCAAC_RemarksBy { get; set; }
        public string NCAC533SPCAAC_StatusFlg { get; set; }
        public bool  NCAC533SPCAAC_ActiveFlag { get; set; }
        public long  NCAC533SPCAAC_CreatedBy { get; set; }
        public DateTime  NCAC533SPCAAC_CreatedDate { get; set; }
        public long  NCAC533SPCAAC_UpdatedBy { get; set; }
        public DateTime NCAC533SPCAAC_UpdatedDate { get; set; }
        public long  NCAC533SPCAA_Id { get; set; }

    }
}
