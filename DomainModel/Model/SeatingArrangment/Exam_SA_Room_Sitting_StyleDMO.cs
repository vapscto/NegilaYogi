using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Room_Sitting_Style")]
    public class Exam_SA_Room_Sitting_StyleDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESARSITSTY_Id { get; set; }
        public long MI_Id { get; set; }
        public bool? ESARSITSTY_SameBranchSameSemFlg { get; set; }
        public bool? ESARSITSTY_DiffBranchSameSemFlg { get; set; }
        public bool? ESARSITSTY_SameBranchDiffSemFlg { get; set; }
        public bool? ESARSITSTY_DiffBranchDiffSemFlg { get; set; }
        public bool? ESARSITSTY_AnyBranchAnySemFlg { get; set; }
        public bool? ESARSITSTY_ActiveFlg { get; set; }
        public DateTime? ESARSITSTY_CreatedDate { get; set; }
        public DateTime? ESARSITSTY_UpdatedDate { get; set; }
        public long? ESARSITSTY_CreatedBy { get; set; }
        public long? ESARSITSTY_UpdatedBy { get; set; }
    }
}
