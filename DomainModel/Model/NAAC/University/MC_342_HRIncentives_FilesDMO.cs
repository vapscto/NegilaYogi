using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_MC_342_HRIncentives_Files")]
    public  class MC_342_HRIncentives_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCHRI342F_Id { get; set; }
        public long NCMCHRI342_Id { get; set; }
        public string NCMCHRI342F_FileName { get; set; }
        public string NCMCHRI342F_Filedesc { get; set; }
        public string NCMCHRI342F_FilePath { get; set; }
    }
}
