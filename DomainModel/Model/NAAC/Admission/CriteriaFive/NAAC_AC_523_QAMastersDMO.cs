using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
  [Table("NAAC_AC_523_QAMasters")]
    public class NAAC_AC_523_QAMastersDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC523QAMA_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCAC523QAMA_ExamName { get; set; }
        public string NCAC523QAMA_ExamDes { get; set; }
        public bool NCAC523QAMA_ActiveFlg { get; set; }
        public long NCAC523QAMA_CreatedBy { get; set; }
        public long NCAC523QAMA_UpdatedBy { get; set; }
        public DateTime NCAC523QAMA_CreatedDate { get; set; }
        public DateTime NCAC523QAMA_UpdatedDate { get; set; }

    }
}
