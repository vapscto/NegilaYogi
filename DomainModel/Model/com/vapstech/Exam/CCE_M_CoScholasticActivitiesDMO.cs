using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("CCE_M_CoScholasticActivities")]
    public class CCE_M_CoScholasticActivitiesDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long CCE_M_CoA_Id { get; set; }
        public long MI_Id { get; set; }
        public string CCE_M_CoA_Name { get; set; }
        public string CCE_M_CoA_Code { get; set; }
        public int CCE_M_CoA_Order { get; set; }
        public bool Active_flag { get; set; }

    }
}
