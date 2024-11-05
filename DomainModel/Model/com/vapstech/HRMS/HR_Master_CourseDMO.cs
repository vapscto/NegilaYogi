using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Master_Course")]
    public class HR_Master_CourseDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMC_QulaificationName { get; set; }
        public string HRMC_QualificationDesc { get; set; }
        public bool HRMC_DefaultQualFag { get; set; }
        public bool HRMC_SpecialisationFlag { get; set; }
        public int HRMC_Order { get; set; }
        public bool HRMC_ActiveFlag { get; set; }
    }
}
