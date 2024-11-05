using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.COE
{
  

    [Table("COE_Events_Classes", Schema = "COE")]

    public class COE_Events_ClassesDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int COEEC_Id { get; set; }
        [ForeignKey("COEE_Id")]
        public int COEE_Id { get; set; }
        public long ASMCL_Id { get; set; }
    }
}
