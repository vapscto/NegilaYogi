using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_121_IntDept_Course")]
    public class NAAC_MC_121_IntDept_CourseDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NMC121IDC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HRMD_Id { get; set; }
        public long AMCO_Id { get; set; }
        public bool NMC121IDC_ActiveFlag { get; set; }
        public long NMC121IDC_NoOfCourse { get; set; }
        public DateTime? NMC121IDC_CreatedDate { get; set; }
        public DateTime? NMC121IDC_UpdatedDate { get; set; }
        public long NMC121IDC_CreatedBy { get; set; }
        public long NMC121IDC_UpdatedBy { get; set; }
        public string NMC121IDC_StatusFlg { get; set; }
        public bool? NMC121IDC_ApprovedFlg { get; set; }
        public string NMC121IDC_Remarks { get; set; }
        public List<NAAC_MC_121_IntDept_Course_FilesDMO> NAAC_MC_121_IntDept_Course_FilesDMO { get; set; }
        public List<NAAC_MC_121_IntDept_Course_Comments_DMO> NAAC_MC_121_IntDept_Course_Comments_DMO { get; set; }
    }
}
