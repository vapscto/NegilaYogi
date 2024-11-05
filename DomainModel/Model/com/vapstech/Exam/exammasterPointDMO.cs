using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_M_Progresscard_Point", Schema = "Exm")]
    public class exammasterPointDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]     
        public long Point_Id { get; set; }
        public long MI_Id { get; set; }
        public string Point_Name { get; set; }
        public int Point_Order { get; set; }
        public bool Active_flag { get; set; }

    }
}
