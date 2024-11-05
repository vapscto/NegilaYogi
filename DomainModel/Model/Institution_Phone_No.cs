using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Master_Institution_Phone_No")]
    public class Institution_Phone_No : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long MIPN_Id { get; set; }
        public long MI_Id { get; set; }
        public long MIPN_PhoneNo { get; set; }
    }
}
