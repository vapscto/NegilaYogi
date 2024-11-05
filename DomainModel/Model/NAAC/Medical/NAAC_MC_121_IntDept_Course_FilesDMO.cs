using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_121_IntDept_Course_Files")]
    public class NAAC_MC_121_IntDept_Course_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NMC121IDCF_Id { get; set; }
        public long NMC121IDC_Id { get; set; }
        public string NMC121IDCF_FileName { get; set; }
        public string NMC121IDCF_FilePath { get; set; }
        public string NMC121IDCF_FileDesc { get; set; }
        public DateTime? NMC121IDCF_CreatedDate { get; set; }
        public DateTime? NMC121IDCF_UpdatedDate { get; set; }
        public long? NMC121IDCF_CreatedBy { get; set; }
        public long? NMC121IDCF_UpdatedBy { get; set; }
        public string NMC121IDCF_StatusFlg { get; set; }
        public bool? NMC121IDCF_ActiveFlg { get; set; }
        public bool? NMC121IDCF_ApprovedFlg { get; set; }
        public string NMC121IDCF_Remarks { get; set; }
    }
}
