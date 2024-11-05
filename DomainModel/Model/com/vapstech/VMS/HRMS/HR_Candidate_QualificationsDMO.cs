using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_Candidate_Qualifications")]
    public class HR_Candidate_QualificationsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long HRCQUAL_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRCD_Id { get; set; }
        public string HRCQUAL_Course { get; set; }
        public string HRCQUAL_Board { get; set; }
        public long HRCQUAL_PassingYear { get; set; }
        public bool HRCQUAL_ActiveFlag { get; set; }
        public long HRCQUAL_CreatedBy { get; set; }
        public long HRCQUAL_UpdatedBy { get; set; }
    }
}
