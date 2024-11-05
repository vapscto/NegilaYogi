using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_314_ResearchAssociates_Files")]
    public class MC_314_ResearchAssociates_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCRA314F_Id { get; set; }
        public long NCMCRA314_Id { get; set; }
        public string NCMCRA314F_FileName { get; set; }
        public string NCMCRA314F_Filedesc { get; set; }
        public string NCMCRA314F_FilePath { get; set; }
    }
}
