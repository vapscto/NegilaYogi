using DomainModel.Model.com.vapstech.Exam;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Student_Examwise_PT", Schema = "Exm")]
    public class Exm_Student_Examwise_PTDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ESEWPT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMPATY_Id { get; set; }
        public bool? ESEWPT_ActiveFlg { get; set; }
        public long? ESEWPT_CreatedBy { get; set; }
        public DateTime? ESEWPT_CreatedDate { get; set; }
        public long? ESEWPT_UpdatedBy { get; set; }
        public DateTime? ESEWPT_UpdatedDate { get; set; }
    }
}