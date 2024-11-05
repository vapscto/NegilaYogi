using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_ETT_Details")]
    public class Exam_SA_ETT_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAETTD_Id { get; set; }
        public long ESAETT_Id { get; set; }
        public long ESAESLOT_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ESAETTD_ActiveFlg { get; set; }
        public DateTime? ESAETTD_CreatedDate { get; set; }
        public DateTime? ESAETTD_UpdatedDate { get; set; }
        public long ESAETTD_CreatedBy { get; set; }
        public long ESAETTD_UpdatedBy { get; set; }
        public DateTime ESAETT_ExamDate { get; set; }
    }
}
