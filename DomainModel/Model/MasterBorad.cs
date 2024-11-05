using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_Board")]
    public class MasterBorad : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMB_Id { get; set; }
        public long MI_Id { get; set; }
        public string IVRMMB_Name { get; set; }
        public string IVRMMB_Description { get; set; }
        public bool Is_Active { get; set; }
    }
}
