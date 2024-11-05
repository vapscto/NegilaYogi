using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Master_College_Staff_GroupHead", Schema = "CLG")]
    public class Fee_Master_College_Staff_GroupHeadDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMCSTGH_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool FMCSTGH_ActiveFlag { get; set; }
        public DateTime FMCSTGH_CreatedDate { get; set; }
        public DateTime FMCSTGH_UpdatedDate { get; set; }
        public long FMCSTGH_CreatedBy { get; set; }
        public long FMCSTGH_UpdatedBy { get; set; }
        public List<Fee_Master_College_Staff_GroupHead_InstallmentsDMO> Fee_Master_College_Staff_GroupHead_InstallmentsDMO { get; set; }
    }
}
