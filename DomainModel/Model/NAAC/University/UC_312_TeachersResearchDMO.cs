using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_312_TeachersResearch")]
    public  class UC_312_TeachersResearchDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCTR312_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public long NCMCTR312_Year { get; set; }
        public string NCMCTR312_ProjectName { get; set; }
        public string NCMCTR312_Duration { get; set; }
        public decimal NCMCTR312_ProjReceivingSeedMoney { get; set; }
        public decimal NCMCTR312_amountOfSeedMoneyProvided { get; set; }
        public long NCMCTR312_CreatedBy { get; set; }
        public long NCMCTR312_UpdatedBy { get; set; }
        public bool NCMCTR312_ActiveFlag { get; set; }
        public DateTime? NCMCTR312_CreatedDate { get; set; }
        public DateTime? NCMCTR312_UpdatedDate { get; set; }
        public List<UC_312_TeachersResearchFilesDMO> UC_312_TeachersResearchFilesDMO { get; set; }
    }
}
