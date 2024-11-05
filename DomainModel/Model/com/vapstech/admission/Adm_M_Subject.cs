using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_M_Subject")]
    public class Adm_M_Subject : CommonParamDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMSU_Id { get; set; }

        public long MI_Id { get; set; }
        public string AMSU_Name { get; set; }

        public string AMSU_Code { get; set; }

        public string AMSU_Flag { get; set; }

        public int AMSU_Order { get; set; }

        public string BatchFlag { get; set; }

        public int amsu_activeflag { get; set; }

        public string TimeTable_flag { get; set; }
    }
}
