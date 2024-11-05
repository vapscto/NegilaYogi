using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Master_Driver", Schema = "TRN")]
    public class MasterDriverDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRMD_DriverName { get; set; }
        public string TRMD_DriverCode { get; set; }
        public string TRMD_DLNo { get; set; }
        public string TRMD_RTOName { get; set; }
        public DateTime TRMD_DLExpiryDate { get; set; }
        public DateTime? TRMD_MTExpiryDate { get; set; }
        public DateTime? TRMD_SDExpiryDate { get; set; }
        public string TRMD_DriverBadgeNo { get; set; }
        public string TRMD_LicenseType { get; set; }
        public bool TRMD_SpareDriverFlg { get; set; }
        public bool TRMD_ActiveFlg { get; set; }
        public long? TRMD_MobileNo { get; set; }
        public string TRMD_EmailId { get; set; }

    }
}
