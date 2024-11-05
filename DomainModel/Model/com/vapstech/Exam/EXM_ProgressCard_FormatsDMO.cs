using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("EXM_ProgressCard_Formats", Schema = "Exm")]
    public class EXM_ProgressCard_FormatsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EPCFT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public string EPCFT_ProgressCardFormat { get; set; }
        public bool? EPCFT_ActiveFlg { get; set; }
        public long? EPCFT_CreatedBy { get; set; }
        public DateTime? EPCFT_CreateDate { get; set; }
        public long? EPCFT_UpdatedBy { get; set; }
        public DateTime? EPCFT_UpdateDate { get; set; }
        public string EPCFT_SPFlag { get; set; }
        public string EPCFT_ExamFlag { get; set; }
        public int? EPCFT_ExamwiseFlg { get; set; }
    }
}
