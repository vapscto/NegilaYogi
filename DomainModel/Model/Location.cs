using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("CV_M_Location")]
    public class Location : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CML_Id { get; set; }
        //public long CMC_Id { get; set; }
        public string CML_Name { get; set; }
        //public string CML_Default { get; set; }
        //public string CML_Alias_Name { get; set; }

        public long MI_Id { get; set; }
    }
}
