using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Master_College_OtherStudents", Schema = "CLG")]
    public class Fee_Master_College_OtherStudents
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMCOST_Id { get; set; }
        public long MI_Id { get; set; }
        public string FMCOST_StudentName { get; set; }
        public long FMCOST_StudentMobileNo { get; set; }
        public string FMCOST_StudentEmailId { get; set; }
        public bool FMCOST_ActiveFlag { get; set; }

        public DateTime FMCOST_CreatedDate { get; set; }
        public DateTime FMCOST_UpdatedDate { get; set; }
        public long FMCOST_CreatedBy { get; set; }
        public long FMCOST_UpdatedBy { get; set; }
    }
}
