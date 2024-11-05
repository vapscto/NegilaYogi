using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_331_Ethics")]
    public class HSU_341_EthicsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC331ES_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMC331ES_Year { get; set; }
        public bool NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag { get; set; }
        public bool NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag { get; set; }
        public bool NCMC331ES_InstPlagiarismSoftInstPolicyFlag { get; set; }
        public bool NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag { get; set; }
        public long NCMC331ES_CreatedBy { get; set; }
        public long NCMC331ES_UpdatedBy { get; set; }
        public DateTime? NCMC331ES_CreatedDate { get; set; }
        public DateTime? NCMC331ES_UpdatedDate { get; set; }
        public bool NCMC331ES_ActiveFlag { get; set; }
    }
}
