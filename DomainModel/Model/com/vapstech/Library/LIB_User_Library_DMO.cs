using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_User_Library", Schema = "LIB")]
    public class LIB_User_Library_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LUL_Id { get; set; }
        public long MI_Id { get; set; }
        public long LMAL_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public bool LUL_ActiveFlg { get; set; }


    }
}
