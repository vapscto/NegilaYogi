using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_514_CompExams_Comments")]
    public class NAAC_AC_514_CompExams_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public long NCAC514CPEXC_Id { get; set; }
        public string NCAC514CPEXC_Remarks { get; set; }
        public long NCAC514CPEXC_RemarksBy { get; set; }
        public string NCAC514CPEXC_StatusFlg { get; set; }
        public bool NCAC514CPEXC_ActiveFlag { get; set; }
        public long NCAC514CPEXC_CreatedBy { get; set; }
        public DateTime NCAC514CPEXC_CreatedDate { get; set; }
        public long NCAC514CPEXC_UpdatedBy { get; set; }
        public DateTime NCAC514CPEXC_UpdatedDate { get; set; }
        public long NCAC514CPEX_Id { get; set; }

    }
}
