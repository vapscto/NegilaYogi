using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_MC_8110_Immunisation_Comments")] 
  public  class NAAC_MC_8110_Immunisation_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC8110IMMC_Id { get; set; }
        public string NCMC8110IMMC_Remarks { get; set; }
        public string NCMC8110IMMC_StatusFlg { get; set; }
        public long NCMC8110IMMC_RemarksBy { get; set; }
        public long NCMC8110IMMC_CreatedBy { get; set; }
        public long NCMC8110IMMC_UpdatedBy { get; set; }
        public bool NCMC8110IMMC_ActiveFlag { get; set; }
        public DateTime? NCMC8110IMMC_CreatedDate { get; set; }
        public DateTime? NCMC8110IMMC_UpdatedDate { get; set; }
        public long NCMC8110IMM_Id { get; set; }
    }
}
