using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_343_TechnologyTransferred")]
    public  class MC_343_TechnologyTransferredDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCTT343_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCTT343_Year { get; set; }
        public string NCMCTT343_PatenterName { get; set; }
        public string NCMCTT343_Patent { get; set; }
        public string NCMCTT343_Title { get; set; }
        public long NCMCTT343_CreatedBy { get; set; }
        public long NCMCTT343_UpdatedBy { get; set; }
        public DateTime? NCMCTT343_CreatedDate { get; set; }
        public DateTime? NCMCTT343_UpdatedDate { get; set; }
        public bool NCMCTT343_ActiveFlag { get; set; }
        public List<MC_343_TechnologyTransferred_FilesDMO> MC_343_TechnologyTransferred_FilesDMO { get; set; }
    }
}
