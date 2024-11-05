using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_ExamDate_Room")]
    public class Exam_SA_ExamDate_RoomDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAEDATED_Id { get; set; }
        public long ESAEDATE_Id { get; set; }
        public long ESAROOM_Id { get; set; }        
        public bool? ESAEDATED_ActiveFlg { get; set; }
        public DateTime? ESAEDATED_CreatedDate { get; set; }
        public DateTime? ESAEDATED_UpdatedDate { get; set; }
        public long ESAEDATED_CreatedBy { get; set; }
        public long ESAEDATED_UpdatedBy { get; set; }
    }
}
