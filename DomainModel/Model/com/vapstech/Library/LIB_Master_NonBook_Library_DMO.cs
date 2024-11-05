using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_NonBook_Library", Schema = "LIB")]
    public class LIB_Master_NonBook_Library_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMNBKL_Id { get; set; }
        public long LMNBK_Id { get; set; }
        public long LMAL_Id { get; set; }
        public bool LMNBKL_ActiveFlg { get; set; }
        public long LMNBKL_CreatedBy { get; set; }
        public long LMNBKL_UpdatedBy { get; set; }


    }
}
