using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_523_QualExams_Comments")]
    public class NAAC_AC_523_QualExams_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
        public long  NCAC523QEC_Id { get; set; }
        public string NCAC523QEC_Remarks { get; set; }
        public long NCAC523QEC_RemarksBy { get; set; }
        public string NCAC523QEC_StatusFlg { get; set; }
        public bool NCAC523QEC_ActiveFlag { get; set; }
        public long NCAC523QEC_CreatedBy { get; set; }
        public DateTime NCAC523QEC_CreatedDate { get; set; }
        public long NCAC523QEC_UpdatedBy { get; set; }
        public DateTime NCAC523QEC_UpdatedDate { get; set; }
        public long NCAC523QE_Id { get; set; }

    }
}
