using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_531_SportsCA_Students_Comments")]
    public class NAAC_AC_531_SportsCA_Students_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
  
      
        public long NCAC531SPCASC_Id { get; set; }
        public string NCAC531SPCASC_Remarks { get; set; }
        public long NCAC531SPCASC_RemarksBy { get; set; }
        public string NCAC531SPCASC_StatusFlg { get; set; }
        public bool NCAC531SPCASC_ActiveFlag { get; set; }
        public long NCAC531SPCASC_CreatedBy { get; set; }
        public DateTime NCAC531SPCASC_CreatedDate { get; set; }
        public long NCAC531SPCASC_UpdatedBy { get; set; }
        public DateTime NCAC531SPCASC_UpdatedDate { get; set; }
        public long NCAC531SPCAS_Id { get; set; }
        
    }
}
