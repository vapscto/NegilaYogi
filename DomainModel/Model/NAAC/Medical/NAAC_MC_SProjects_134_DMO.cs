using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_SProjects_134")]
    public class NAAC_MC_SProjects_134_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCSP134_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCSP134_Year { get; set; }
        public long NCMCSP134_NoOfStudentsUndertakingFieldVisits { get; set; }
        public long NCMCSP134_NoOfStudentsUndertakingClinical { get; set; }
        public long NCMCSP134_NoOfStudentsUndertakingResearchProjects { get; set; }
        public long NCMCSP134_NoOfStudentsUndertakingIndustryVisits { get; set; }
        public long NCMCSP134_NoOfStudentsUndertakingCommunityPostings { get; set; }
        public long NCMCSP134_CreatedBy { get; set; }
        public long NCMCSP134_UpdatedBy { get; set; }
        public DateTime NCMCSP134_CreateDate { get; set; }
        public DateTime NCMCSP134_UpdatedDate { get; set; }
     
        public string NCMCSP134_StatusFlg { get; set; }
        public string NCMCSP134_Remarks { get; set; }
        public bool? NCMCSP134_ApprovedFlg { get; set; }
        public bool? NCMCSP134_ActiveFlag { get; set; }

        public List<NAAC_MC_SProjects_134_Files_DMO> NAAC_MC_SProjects_134_Files_DMO { get; set;}

    }
}
