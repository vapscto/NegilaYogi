using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("master_role")]
    public class MasterRole : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long IVRMR_Id { get; set; }
        public string IVRMR_Role { get; set; }
        public string IVRMR_Role_desc { get; set; }
        public int IVRMR_Order { get; set; }
        public int IVRMR_ActiveFlag { get; set; }

    }
}
