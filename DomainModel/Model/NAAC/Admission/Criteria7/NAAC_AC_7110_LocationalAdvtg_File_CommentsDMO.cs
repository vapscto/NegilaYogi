using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_AC_7110_LocationalAdvtg_File_Comments")]
   public class NAAC_AC_7110_LocationalAdvtg_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC7110LOCADVTGFC_Id { get; set; }
        public long NCAC7110LOCADVTGF_Id { get; set; }
        public long NCAC7110LOCADVTGFC_RemarksBy { get; set; }
        public string NCAC7110LOCADVTGFC_Remarks { get; set; }
        public bool NCAC7110LOCADVTGFC_ActiveFlag { get; set; }
        public long NCAC7110LOCADVTGFC_CreatedBy { get; set; }
        public long NCAC7110LOCADVTGFC_UpdatedBy { get; set; }
        public DateTime? NCAC7110LOCADVTGFC_CreatedDate { get; set; }
        public DateTime? NCAC7110LOCADVTGFC_UpdatedDate { get; set; }
        public string NCAC7110LOCADVTGFC_StatusFlg { get; set; }
       
    }
}







