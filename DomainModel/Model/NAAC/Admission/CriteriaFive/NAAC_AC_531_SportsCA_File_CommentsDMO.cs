using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_531_SportsCA_File_Comments")]
    public class NAAC_AC_531_SportsCA_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
  
        public long  NCAC531SPCAFC_Id { get; set; }
        public string NCAC531SPCAFC_Remarks { get; set; }
        public long NCAC531SPCAFC_RemarksBy { get; set; }
        public bool NCAC531SPCAFC_ActiveFlag { get; set; }
        public long NCAC531SPCAFC_CreatedBy { get; set; }
        public DateTime NCAC531SPCAFC_CreatedDate { get; set; }
        public long NCAC531SPCAFC_UpdatedBy { get; set; }
        public DateTime NCAC531SPCAFC_UpdatedDate { get; set; }
        public string NCAC531SPCAFC_StatusFlg { get; set; }
        public long NCAC531SPCAF_Id { get; set; }
        

    }
}
