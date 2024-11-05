using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Book_Library", Schema = "LIB")]
    public class LIB_Master_Book_Library_DMO:CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMBL_Id { get; set; }
        public long LMB_Id { get; set; }
        public long LMAL_Id { get; set; }
        public bool LMBL_ActiveFlg { get; set; }
        

    }
}
