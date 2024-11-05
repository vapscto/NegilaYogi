using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Language", Schema ="LIB")]
    public class MasterLanguageDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long LML_Id { get; set; }
       public long MI_Id { get; set; }
       public string LML_LanguageName { get; set; }
       public bool LML_ActiveFlg { get; set; }
       
    }
}
