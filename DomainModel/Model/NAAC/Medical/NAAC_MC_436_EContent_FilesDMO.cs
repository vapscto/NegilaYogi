using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_436_EContent_Files")]
    public class NAAC_MC_436_EContent_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCMCMEC436F_Id { get; set; }
        public long NCMCMEC436_Id { get; set; }
        public string NCAC434ECTF_FileName { get; set; }
        public string NCAC434ECTF_Filedesc { get; set; }
        public string NCAC434ECTF_FilePath { get; set; }
    }
}
