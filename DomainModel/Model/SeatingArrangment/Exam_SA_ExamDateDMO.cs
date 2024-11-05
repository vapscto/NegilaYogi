using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_ExamDate")]
    public class Exam_SA_ExamDateDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ESAEDATE_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EME_Id { get; set; }
        public long ESAUE_Id { get; set; }
        public long ESAESLOT_Id { get; set; }        
        public DateTime? ESAEDATE_ExamDate { get; set; }
        public bool? ESAEDATE_ActiveFlg { get; set; }
        public DateTime? ESAEDATE_CreatedDate { get; set; }
        public DateTime? ESAEDATE_UpdatedDate { get; set; }
        public long ESAEDATE_CreatedBy { get; set; }
        public long ESAEDATE_UpdatedBy { get; set; }
        public List<Exam_SA_ExamDate_RoomDMO> Exam_SA_ExamDate_RoomDMO { get; set; }
    }
}
