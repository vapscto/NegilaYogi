using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_Student_TermAchievements", Schema = "Exm")]
    public class Exm_Student_TermAchievementsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESTTA_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public int ECT_Id { get; set; }
        public long AMST_Id { get; set; }
        public string ESTTA_Remarks { get; set; }
        public bool ESTTA_ActiveFlag { get; set; }
        public long? ESTTA_Createdby { get; set; }
        public DateTime? ESTTA_CreatedDate { get; set; }
        public long? ESTTA_Updatedby { get; set; }
        public DateTime? ESTTA_UpdatedDate { get; set; }
    }
}
