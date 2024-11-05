using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Student_Route_College", Schema = "TRN")]

    public class CLGStudentRouteMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   
        public long TRSRCO_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCST_Id { get; set; }
        public DateTime TRSRCO_Date { get; set; }
        public long TRSRCO_PickUpRoute { get; set; }
        public long TRSRCO_PickupSession { get; set; }
        public long TRRSCO_PickUpLocation { get; set; }
        public long? TRRSCO_PickUpMobileNo { get; set; }
        public long TRSRCO_DropRoute { get; set; }
        public long TRSRCO_DropSession { get; set; }
        public long TRRSCO_DropLocation { get; set; }
        public long? TRRSCO_DropMobileNo { get; set; }
        public long? ASTACO_Id { get; set; }
        public string TRRSCO_ApplicationNo { get; set; }
        public bool TRRSCO_ActiveFlg { get; set; }
        public List<CLGStudentRouteFeeGroupDMO> CLGStudentRouteFeeGroupDMO { get; set; }
    }
}
