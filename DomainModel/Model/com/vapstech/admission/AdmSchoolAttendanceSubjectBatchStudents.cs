using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_School_Attendance_Subject_Batch_Students")]
    public class AdmSchoolAttendanceSubjectBatchStudents:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASASBS_Id { get; set; }
        public long ASASB_Id { get; set; }
        public long AMST_Id { get; set; }
    }
}
