using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_436_EContent")]
    public class NAAC_MC_436_EContentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCMCMEC436_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string NCMCMEC436_ModuleName { get; set; }
        public string NCMCMEC436_PlatformModuleUsed { get; set; }
        public DateTime? NCMCMEC436_Date { get; set; }
        public string NCMCMEC436_WebLink { get; set; }
        public long NCMCMEC436_CreatedBy { get; set; }
        public long NCMCMEC436_UpdatedBy { get; set; }
        public DateTime? NCMCMEC436_CreatedDate { get; set; }
        public DateTime? NCMCMEC436_UpdatedDate { get; set; }
        public long NCMCMEC436_Year { get; set; }
        public bool NCMCMEC436_ActiveFlag { get; set; }
        public List<NAAC_MC_436_EContent_FilesDMO> NAAC_MC_436_EContent_FilesDMO { get; set; }       
    }
}
