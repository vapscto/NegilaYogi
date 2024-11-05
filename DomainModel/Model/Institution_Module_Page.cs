using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Institution_Module_Page")]
    public class Institution_Module_Page : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMIMP_Id { get; set; }
        public long IVRMIM_Id { get; set; }
        public long IVRMP_Id { get; set; }
        public int IVRMIMP_Flag { get; set; }
        public int IVRMIMP_PageOrder { get; set; }
        public string IVRMIMP_DisplayContent { get; set; }
       

    }
}
