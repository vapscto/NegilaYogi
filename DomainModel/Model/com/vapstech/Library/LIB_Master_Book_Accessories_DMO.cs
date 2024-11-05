using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
     [Table("LIB_Master_Book_Accessories", Schema = "LIB")]

    public class LIB_Master_Book_Accessories_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMBAC_Id { get; set; }
        public long LMB_Id { get; set; }
        public long? LMAC_Id { get; set; }
        public bool LMBAC_ActiveFlg { get; set; }

    }
}
