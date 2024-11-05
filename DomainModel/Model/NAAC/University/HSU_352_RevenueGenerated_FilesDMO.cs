using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_352_RevenueGenerated_Files")]
    public  class HSU_352_RevenueGenerated_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCRG352F_Id { get; set; }
        public long NCMCRG352_Id { get; set; }
        public string NCMCRG352F_FileName { get; set; }
        public string NCMCRG352F_Filedesc { get; set; }
        public string NCMCRG352F_FilePath { get; set; }
    }
}
