using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_User_ClassCategory", Schema = "LIB")]
    public class LIB_User_ClassCategory_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LUCC_Id { get; set; }        
        public long MI_Id { get; set; }
        public long LMCC_Id { get; set; }
        public long IVRMUL_Id { get; set; }        
        public bool LUCC_ActiveFlg { get; set; }


    }
}
