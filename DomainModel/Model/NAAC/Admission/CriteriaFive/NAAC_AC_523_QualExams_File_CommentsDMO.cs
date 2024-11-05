using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_523_QualExams_File_Comments")]
    public class NAAC_AC_523_QualExams_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       
       
        public long NCAC523QEFC_Id { get; set; }
        public string  NCAC523QEFC_Remarks { get; set; }
        public long  NCAC523QEFC_RemarksBy { get; set; }
        public bool  NCAC523QEFC_ActiveFlag { get; set; }
        public long  NCAC523QEFC_CreatedBy { get; set; }
        public DateTime  NCAC523QEFC_CreatedDate { get; set; }
        public long  NCAC523QEFC_UpdatedBy { get; set; }
        public DateTime NCAC523QEFC_UpdatedDate { get; set; }
        public string NCAC523QEFC_StatusFlg { get; set; }
        public long  NCAC523QEF_Id { get; set; }

    }
}
