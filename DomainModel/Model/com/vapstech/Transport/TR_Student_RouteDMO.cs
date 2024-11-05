using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Student_Route", Schema = "TRN")]
    public class TR_Student_RouteDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRSR_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public DateTime TRSR_Date { get; set; }
        public long FMG_Id { get; set; }
        public long TRMR_Id { get; set; }
        public long TRSR_PickupSchedule { get; set; }
        public long TRSR_PickUpLocation { get; set; }
        public long TRSR_PickUpMobileNo { get; set; }
        public long TRSR_DropSchedule { get; set; }
        public long TRSR_DropLocation { get; set; }
        public long TRSR_DropMobileNo { get; set; }
        public long TRSR_ApplicationNo { get; set; }
        public bool TRSR_ActiveFlg { get; set; }
        public long? ASTA_Id { get; set; }
        public long? TRMR_Drop_Route { get; set; }
        public long TRSR_PickupSession { get; set; }
        public long TRSR_DropSession { get; set; }
        public List<TR_Student_Route_FeeGroupDMO> TR_Student_Route_FeeGroupDMO { get; set; }
    }
}
