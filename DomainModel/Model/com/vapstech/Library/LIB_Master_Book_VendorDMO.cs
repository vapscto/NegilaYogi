using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Book_Vendor", Schema = "LIB")]
    public class LIB_Master_Book_VendorDMO : CommonParamDMO
    {
        [Key]
        public long LMBV_Id { get; set; }
        public long LMB_Id { get; set; }
        public long LMV_Id { get; set; }
        public bool LMBV_ActiveFlg { get; set; }
       
    }
}
