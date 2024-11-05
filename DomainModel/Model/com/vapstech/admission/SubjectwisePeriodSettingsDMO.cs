using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_School_Attendance_Subject_MaxPeriod")]
    public class SubjectwisePeriodSettingsDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASASMP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int ASASMP_MaxPeriod { get; set; }

       // public ICollection<pagemodulemapping> modulepagemapping { get; set; }
    }
}
