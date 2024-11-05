using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("PA_Student_Sibblings_Details")]
    public class PA_Student_Sibblings_Details : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASSD_Id { get; set; }
        public long PASS_Id { get; set; }
        public long PASSD_SibblingAMST_Id { get; set; }
        public int? PASSD_SibblingOrder { get; set; }
        public bool PASSD_ActiveFlag { get; set; }

    }
}
