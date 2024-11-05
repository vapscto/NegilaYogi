using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Master_College_OthStudents_GH_Instl", Schema = "CLG")]
    public class Fee_Master_College_OthStudents_GH_InstlDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMCOSTGHI_Id { get; set; }
        [ForeignKey("FMCOSTGH_Id")]
        public long FMCOSTGH_Id { get; set; }
        public long FMH_Id { get; set; }
        public long FTI_Id { get; set; }

        public DateTime FMCOSTGHI_CreatedDate { get; set; }

        public DateTime  FMCOSTGHI_UpdatedDate { get; set; }

        public long FMCOSTGHI_CreatedBy { get; set; }

        public long FMCOSTGHI_UpdatedBy { get; set; }

    }
}
