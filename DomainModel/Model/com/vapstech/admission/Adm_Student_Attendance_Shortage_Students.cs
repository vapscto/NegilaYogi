using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_Attendance_Shortage_Students")]
    public class Adm_Student_Attendance_Shortage_Students
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASASHORTD_Id { get; set; }
        public long ASASHORT_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
    }
}
