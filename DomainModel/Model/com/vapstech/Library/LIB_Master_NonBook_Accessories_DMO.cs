using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_NonBook_Accessories", Schema = "LIB")]
    public class LIB_Master_NonBook_Accessories_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMNBKAC_Id { get; set; }
        public long LMNBK_Id { get; set; }
        public long LMAC_Id { get; set; }
        public bool LMNBKAC_ActiveFlg { get; set; }
        public long LMNBKAC_CreatedBy { get; set; }
        public long LMNBKAC_UpdatedBy { get; set; }


    }
}
