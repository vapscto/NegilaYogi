using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
 [Table("LIB_Master_Vendor", Schema ="LIB")]
   public class MasterVanderDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMV_Id { get; set; }
        public long MI_Id { get; set; }
        public long? LMV_MobileNo { get; set; }
        public string LMV_VendorName { get; set; }
        public string LMV_Address { get; set; }
        public string LMV_EMailId { get; set; }
        public string LMV_PhoneNo { get; set; }
        public bool LMV_ActiveFlg { get; set; }
     

    }
}
