using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_312_TeachersResearch_Files")]
    public  class UC_312_TeachersResearchFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCTR312F_Id { get; set; }
        public long NCMCTR312_Id { get; set; }
        public string NCMCTR312F_FileName { get; set; }
        public string NCMCTR312F_Filedesc { get; set; }
        public string NCMCTR312F_FilePath { get; set; }
    }
}
