using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_314_ResearchAssociates")]
   public class MC_314_ResearchAssociatesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCRA314_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long NCMCRA314_Year { get; set; }
        public string NCMCRA314_NameOfResearch { get; set; }
        public string NCMCRA314_Type { get; set; }
        public string NCMCRA314_GrantingAgency { get; set; }
        public string NCMCRA314_QualExamName { get; set; }
        public string NCMCRA314_Duration { get; set; }
        public bool NCMCRA314_ActiveFlag { get; set; }
        public long NCMCRA314_CreatedBy { get; set; }
        public long NCMCRA314_UpdatedBy { get; set; }
        public DateTime? NCMCRA314_CreatedDate { get; set; }
        public DateTime? NCMCRA314_UpdatedDate { get; set; }
        public List<MC_314_ResearchAssociates_FilesDMO> MC_314_ResearchAssociates_FilesDMO { get; set; }
    }
}
