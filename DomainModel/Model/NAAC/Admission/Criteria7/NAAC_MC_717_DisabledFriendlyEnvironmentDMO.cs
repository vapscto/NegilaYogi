using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_MC_717_DisabledFriendlyEnvironment")]
    public class NAAC_MC_717_DisabledFriendlyEnvironmentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMC717DFE_Id { get; set; }
        public long MI_Id { get; set; }
        public string NCMC717DFE_BuiltEnvwithRampsORLiftsFlag { get; set; }
        public string NCMC717DFE_DisabledFriendlyWashroomsFlag { get; set; }
        public string NCMC717DFE_SignageIncTactilePathssignpostsFlag { get; set; }
        public string NCMC717DFE_AssistiveTechnologyFacfacMEFlag { get; set; }
        public string NCMC717DFE_ProvisionForEnquiryScreenReadingFlag { get; set; }
        public long NCMC717DFE_CreatedBy { get; set; }
        public long NCMC717DFE_UpdatedBy { get; set; }
        public DateTime NCMC717DFE_CreatedDate { get; set; }
        public DateTime NCMC717DFE_UpdatedDate { get; set; }
        public long NCMC717DFE_Year { get; set; }
        public bool NCMC717DFE_ActiveFlg { get; set; }
    }
}
