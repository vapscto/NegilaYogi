using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_343_TechnologyTransferred_Files")]
    public  class MC_343_TechnologyTransferred_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCTT343F_Id { get; set; }
        public long NCMCTT343_Id { get; set; }
        public string NCMCTT343F_FileName { get; set; }
        public string NCMCTT343F_Filedesc { get; set; }
        public string NCMCTT343F_FilePath { get; set; }
    }
}
