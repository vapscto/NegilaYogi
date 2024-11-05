using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model
{
    [Table("PN_Sent_Details_Devicewise")]
    public class PN_Sent_Details_Devicewise_DMO
    {
       [Key]
       [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PNSDDE_Id { get; set; }
        public long PNSD_Id { get; set; }
        public long? PNSDDE_MobileNo { get; set; }
        public string PNSDDE_DeviceId { get; set; }
        public string PNSDDE_ReadStatus { get; set; }
        public DateTime PNSDDE_DeliveryDate { get; set; }
        public string PNSDDE_DeliveryTime { get; set; }
        public bool PNSDDE_MakeUnreadFlg { get; set; }
        public string PNSDDE_ApprovalLevel { get; set; }
    }
}
