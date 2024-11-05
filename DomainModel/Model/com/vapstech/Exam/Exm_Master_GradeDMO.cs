using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Master_Grade", Schema = "Exm")]
    public class Exm_Master_GradeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EMGR_Id { get; set; }
        public long MI_Id { get; set; }
        public string EMGR_GradeName { get; set; }
        public string EMGR_MarksPerFlag { get;set;}
        public bool EMGR_ActiveFlag { get; set; }

        public List<Exm_Master_Grade_DetailsDMO> EXM_M_Grade_Details { get; set; }
    }
}
