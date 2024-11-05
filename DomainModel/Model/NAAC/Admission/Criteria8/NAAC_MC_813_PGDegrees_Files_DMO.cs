using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_813_PGDegrees_Files")]
    public class NAAC_MC_813_PGDegrees_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC813PGDEF_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC813PGDE_Id { get; set; }
        public string NCMC813PGDEF_FileDesc { get; set; }
        public string NCMC813PGDEF_FileName { get; set; }
        public string NCMC813PGDEF_FilePath { get; set; }
        public string NCMC813PGDEF_StatusFlg { get; set; }
        public bool NCMC813PGDEF_ActiveFlg { get; set; }
        public long NCMC813PGDEF_CreatedBy { get; set; }
        public long NCMC813PGDEF_UpdatedBy { get; set; }
        public DateTime? NCMC813PGDEF_CreatedDate { get; set; }
        public DateTime? NCMC813PGDEF_UpdatedDate { get; set; }
    }
}
