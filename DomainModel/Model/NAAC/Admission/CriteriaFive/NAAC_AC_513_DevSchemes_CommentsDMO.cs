using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_513_DevSchemes_Comments")]
    public class NAAC_AC_513_DevSchemes_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long NCAC513INSCHC_Id { get; set; }
        public string NCAC513INSCHC_Remarks { get; set; }
        public long NCAC513INSCHC_RemarksBy { get; set; }
        public string NCAC513INSCHC_StatusFlg { get; set; }
        public bool NCAC513INSCHC_ActiveFlag { get; set; }
        public long NCAC513INSCHC_CreatedBy { get; set; }
        public DateTime NCAC513INSCHC_CreatedDate { get; set; }
        public long NCAC513INSCHC_UpdatedBy { get; set; }
        public DateTime NCAC513INSCHC_UpdatedDate { get; set; }
        public long NCAC513INSCH_Id { get; set; }

    }
}
