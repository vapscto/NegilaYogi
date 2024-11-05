using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Student_Attendance_Shortage")]
    public class Adm_Student_Attendance_Shortage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASASHORT_Id { get; set; }
        public long MI_Id { get; set; }
        public DateTime ASASHORT_AlertDate { get; set; }
        
    }
}
