using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Library_Class", Schema = "LIB")]
    public class LIB_Library_Class_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LLC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool LLC_ActiveFlg { get; set; }
        public long LMAL_Id { get; set; }
        
       
    }
}
