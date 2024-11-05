using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_Room_ClassSubject_School")]
    public class School_Exam_SA_Room_ClassSubject_SchoolDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESARMCSSCH_Id { get; set; }
        public long ESARMSCH_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool ESARMCSSCH_ActiveFlg { get; set; }
        public DateTime? ESARMCSSCH_CreatedDate { get; set; }
        public DateTime? ESARMCSSCH_UpdatedDate { get; set; }
        public long ESARMCSSCH_CreatedBy { get; set; }
        public long ESARMCSSCH_UpdatedBy { get; set; }
    }
}
