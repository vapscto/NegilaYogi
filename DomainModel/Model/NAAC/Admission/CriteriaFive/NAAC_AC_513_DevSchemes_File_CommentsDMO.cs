using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_513_DevSchemes_File_Comments")]
    public class NAAC_AC_513_DevSchemes_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long NCAC513INSCHFC_Id { get; set; }
        public string NCAC513INSCHFC_Remarks { get; set; }
        public long NCAC513INSCHFC_RemarksBy { get; set; }
        public bool NCAC513INSCHFC_ActiveFlag { get; set; }
        public long NCAC513INSCHFC_CreatedBy { get; set; }
        public DateTime NCAC513INSCHFC_CreatedDate { get; set; }
        public long NCAC513INSCHFC_UpdatedBy { get; set; }
        public DateTime NCAC513INSCHFC_UpdatedDate { get; set; }
        public string NCAC513INSCHFC_StatusFlg { get; set; }
        public long NCAC513INSCHF_Id { get; set; }

    }
}
