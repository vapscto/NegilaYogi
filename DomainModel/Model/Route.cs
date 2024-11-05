using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("CV_M_Route")]
    public class Route : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMR_Id { get; set; }
        public string CMR_Name { get; set; }
        //public string CMR_Number { get; set; }

        public long MI_Id { get; set; }

    }
}
