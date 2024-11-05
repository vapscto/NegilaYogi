using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_521_Placement_File_Comments")]
    public class NAAC_AC_521_Placement_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        
        public long NCAC521PLAFC_Id { get; set; }
        public string  NCAC521PLAFC_Remarks { get; set; }
        public long NCAC521PLAFC_RemarksBy { get; set; }
        public bool NCAC521PLAFC_ActiveFlag { get; set; }
        public long NCAC521PLAFC_CreatedBy { get; set; }
        public DateTime NCAC521PLAFC_CreatedDate { get; set; }
        public long NCAC521PLAFC_UpdatedBy { get; set; }
        public DateTime NCAC521PLAFC_UpdatedDate { get; set; }
        public string NCAC521PLAFC_StatusFlg { get; set; }
        public long NCAC521PLAF_Id { get; set; }
    }
}
