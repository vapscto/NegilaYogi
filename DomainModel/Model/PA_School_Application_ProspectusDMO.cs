using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model
{
    [Table("PA_School_Application_Prospectus")]
    public class PA_School_Application_ProspectusDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PASAP_Id { get; set; }
        public long PASR_Id { get; set; }
        public long PASP_Id { get; set; }
     

    }
}
