using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Accessories", Schema = "LIB")]
    public class LIB_Master_Accessories_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long  LMAC_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMAC_AccessoriesName { get; set; }
        public string LMAC_AccessoriesDesc { get; set; }
        public bool LMAC_ActiveFlg { get; set; }
   


    }
}
