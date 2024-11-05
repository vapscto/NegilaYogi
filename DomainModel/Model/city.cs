using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    //[Table("CV_M_City")]
    [Table("IVRM_Master_City")]
    public class City : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public long CMC_Id { get; set; }
        public long IVRMMCT_Id { get; set; }
        public string IVRMMCT_Name { get; set; }

        public long IVRMMS_Id { get; set; }
        public long IVRMMC_Id { get; set; }

        //public int CMC_Radius { get; set; }
        //public string CMC_Colour { get; set; }
        //public string CMC_Flag { get; set; }
        //public int CMC_level { get; set; }
        //public long CMC_Parent_Id { get; set; }
        //public int CMC_X { get; set; }
        //public int CMC_Y { get; set; }
        //public string CMC_Alias_Name { get; set; }

    }
}
