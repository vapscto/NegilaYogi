using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_514_CompExams_File_Comments")]
    public class NAAC_AC_514_CompExams_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long  NCAC514CPEXFC_Id { get; set; }
        public string NCAC514CPEXFC_Remarks { get; set; }
        public long NCAC514CPEXFC_RemarksBy { get; set; }
        public bool NCAC514CPEXFC_ActiveFlag { get; set; }
        public long NCAC514CPEXFC_CreatedBy { get; set; }
        public DateTime NCAC514CPEXFC_CreatedDate { get; set; }
        public long NCAC514CPEXFC_UpdatedBy { get; set; }
        public DateTime NCAC514CPEXFC_UpdatedDate { get; set; }
        public string NCAC514CPEXFC_StatusFlg { get; set; }
        public long NCAC514CPEXF_Id { get; set; }

    }
}
