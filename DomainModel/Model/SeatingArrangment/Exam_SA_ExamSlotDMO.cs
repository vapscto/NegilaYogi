using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_ExamSlot")]
    public class Exam_SA_ExamSlotDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAESLOT_Id { get; set; }
        public long MI_Id { get; set; }
        public string ESAESLOT_SlotName { get; set; }
        public string ESAESLOT_StartTime { get; set; }
        public string ESAESLOT_EndTime { get; set; }
        public bool ESAESLOT_ActiveFlg { get; set; }
        public DateTime ESAESLOT_CreatedDate { get; set; }
        public DateTime ESAESLOT_UpdatedDate { get; set; }
        public long ESAESLOT_CreatedBy { get; set; }
        public long ESAESLOT_UpdatedBy { get; set; }
    }
}
