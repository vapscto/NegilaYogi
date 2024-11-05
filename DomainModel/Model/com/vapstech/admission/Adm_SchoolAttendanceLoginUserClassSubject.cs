using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("Adm_School_Attendance_Login_User_Class_Subjects")]
    public class Adm_SchoolAttendanceLoginUserClassSubject:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASALUCS_Id { get; set; }
        public long ASALUC_Id { get; set; }
        public long ISMS_Id { get; set; }
    }
}
