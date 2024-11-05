using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace DomainModel.Model.com.vapstech.College.Fees
{
    [Table("Fee_Master_College_OthStudents_GH", Schema = "CLG")]
    public class Fee_Master_College_OthStudents_GHDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMCOSTGH_Id { get; set; }
        public long MI_Id { get; set; }
        public long FMCOST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long FMG_Id { get; set; }
        public bool FMCOSTGH_ActiveFlag { get; set; }
        public DateTime FMCOSTGH_CreatedDate { get; set; }
        public DateTime FMCOSTGH_UpdatedDate { get; set; }
        public long FMCOSTGH_CreatedBy { get; set; }
        public long FMCOSTGH_UpdatedBy { get; set; }
        public List<Fee_Master_College_OthStudents_GH_InstlDMO> Fee_Master_College_OthStudents_GH_InstlDMO { get; set; }
       

    }
}
