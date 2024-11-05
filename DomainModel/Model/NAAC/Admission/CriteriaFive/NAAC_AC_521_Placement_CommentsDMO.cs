using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_521_Placement_Comments")]
    public class NAAC_AC_521_Placement_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long  NCAC521PLAC_Id { get; set; }
        public string NCAC521PLAC_Remarks { get; set; }
        public long NCAC521PLAC_RemarksBy { get; set; }
        public string NCAC521PLAC_StatusFlg { get; set; }
        public bool NCAC521PLAC_ActiveFlag { get; set; }
        public long NCAC521PLAC_CreatedBy { get; set; }
        public DateTime NCAC521PLAC_CreatedDate { get; set; }
        public long NCAC521PLAC_UpdatedBy { get; set; }
        public DateTime NCAC521PLAC_UpdatedDate { get; set; }
        public long NCAC521PLA_Id { get; set; }
    }
}
