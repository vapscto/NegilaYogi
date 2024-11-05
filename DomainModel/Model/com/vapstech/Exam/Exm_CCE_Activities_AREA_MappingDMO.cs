using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Exam
{
    [Table("Exm_CCE_Activities_AREA_Mapping", Schema = "Exm")]
    public class Exm_CCE_Activities_AREA_MappingDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ECACTAM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int ECACT_Id { get; set; }
        public int ECACTA_Id { get; set; }
        public string ECACTAM_IndicatorDescription { get; set; }     
        public int EMGR_Id { get; set; }
        public bool ECACTAM_ActiveFlag { get; set; }
    }
}
