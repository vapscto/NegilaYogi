using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("CCE_M_Scholastic_Areas")]
    public class CCE_M_Scholastic_AreasDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     


        public long CCE_M_Sch_Area_Id { get; set; }
        public long MI_Id { get; set; }
        public string CCE_M_Sch_Area_Name { get; set; }
        public int CCE_M_Sch_Area_Order { get; set; }
        public bool Active_flag { get; set; }


    }
}
