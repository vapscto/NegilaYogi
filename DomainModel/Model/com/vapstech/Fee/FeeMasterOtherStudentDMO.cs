using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{
    [Table("Fee_Master_OtherStudents")]
    public class FeeMasterOtherStudentDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMOST_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMOST_StudentName { get; set; }
        public long FMOST_StudentMobileNo { get; set; }
        public string FMOST_StudentEmailId { get; set; }
        public bool FMOST_ActiveFlag { get; set; }
    }
}
