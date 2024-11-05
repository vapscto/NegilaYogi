using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Master_College_Staff_GroupHead_Installments", Schema = "CLG")]
    public class Fee_Master_College_Staff_GroupHead_InstallmentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMCSTGHI_Id { get; set; }
        [ForeignKey("FMCSTGH_Id")]
        public long FMCSTGH_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }
        public DateTime FMCSTGHI_CreatedDate { get; set; }
        public DateTime FMCSTGHI_UpdatedDate { get; set; }
        public long FMCSTGHI_CreatedBy { get; set; }
        public long FMCSTGHI_UpdatedBy { get; set; }
    }

}
