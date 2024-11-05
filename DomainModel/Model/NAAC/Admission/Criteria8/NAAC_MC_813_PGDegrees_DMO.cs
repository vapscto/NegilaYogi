using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_813_PGDegrees")]
    public class NAAC_MC_813_PGDegrees_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC813PGDE_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC813PGDE_JoinYear { get; set; }
        public long NCMC813PGDE_CompletedYear { get; set; }
        public string NCMC813PGDE_DegreeFromInst { get; set; }
        public string NCMC813PGDE_StatusFlg { get; set; }
        public long NCMC813PGDE_NoOfTeachers { get; set; }
        public bool NCMC813PGDE_ActiveFlg { get; set; }
        public long NCMC813PGDE_CreatedBy { get; set; }
        public long NCMC813PGDE_UpdatedBy { get; set; }
        public DateTime? NCMC813PGDE_CreatedDate { get; set; }
        public DateTime? NCMC813PGDE_UpdatedDate { get; set; }
        public List<NAAC_MC_813_PGDegrees_Files_DMO> NAAC_MC_813_PGDegrees_Files_DMO { get; set; }

    }
}
