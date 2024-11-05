using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Hirer_Group", Schema = "TRN")]
    public class MasterHirerGroupDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRHG_Id { get; set; }
        public long MI_Id { get; set; }
        public string TRHG_HirerGroup { get; set; }
        public string TRHG_HirerDec { get; set; }
        public bool TRHG_ActiveFlg { get; set; }
    }
}
