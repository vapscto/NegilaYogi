using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_121_IntDept_Course_File_Comments")]
    public class NAAC_MC_121_IntDept_Course_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NMC121IDCFC_Id { get; set; }
        public string NMC121IDCFC_Remarks { get; set; }
        public long NMC121IDCFC_RemarksBy { get; set; }
        public bool? NMC121IDCFC_ActiveFlag { get; set; }
        public long? NMC121IDCFC_CreatedBy { get; set; }
        public DateTime? NMC121IDCFC_CreatedDate { get; set; }
        public long? NMC121IDCFC_UpdatedBy { get; set; }
        public DateTime? NMC121IDCFC_UpdatedDate { get; set; }
        public string NMC121IDCFC_StatusFlg { get; set; }
        public long NMC121IDCF_Id { get; set; }

    }
}
