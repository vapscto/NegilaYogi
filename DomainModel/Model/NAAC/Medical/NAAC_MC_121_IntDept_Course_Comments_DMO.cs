using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_121_IntDept_Course_Comments")]
    public class NAAC_MC_121_IntDept_Course_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NMC121IDCC_Id { get; set; }
        public string NMC121IDCC_Remarks { get; set; }
        public long? NMC121IDCC_RemarksBy { get; set; }
        public string NMC121IDCC_StatusFlg { get; set; }
        public bool? NMC121IDCC_ActiveFlag { get; set; }
        public long? NMC121IDCC_CreatedBy { get; set; }
        public DateTime? NMC121IDCC_CreatedDate { get; set; }
        public long? NMC121IDCC_UpdatedBy { get; set; }
        public DateTime? NMC121IDCC_UpdatedDate { get; set; }
        public long NMC121IDC_Id { get; set; }


    }
}
