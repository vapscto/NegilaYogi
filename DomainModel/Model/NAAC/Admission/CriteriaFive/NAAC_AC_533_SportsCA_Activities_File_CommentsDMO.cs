using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_533_SportsCA_Activities_File_Comments")]
    public class NAAC_AC_533_SportsCA_Activities_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long NCAC533SPCAAFC_Id { get; set; }
        public string NCAC533SPCAAFC_Remarks { get; set; }
        public long NCAC533SPCAAFC_RemarksBy { get; set; }
        public bool NCAC533SPCAAFC_ActiveFlag { get; set; }
        public long NCAC533SPCAAFC_CreatedBy { get; set; }
        public DateTime NCAC533SPCAAFC_CreatedDate { get; set; }
        public long NCAC533SPCAAFC_UpdatedBy { get; set; }
        public DateTime NCAC533SPCAAFC_UpdatedDate { get; set; }
        public string NCAC533SPCAAFC_StatusFlg { get; set; }
        public long NCAC533SPCAAF_Id { get; set; }

    }
}
