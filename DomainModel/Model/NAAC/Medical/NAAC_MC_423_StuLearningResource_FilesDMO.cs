using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_423_StuLearningResource_Files")]
   public class NAAC_MC_423_StuLearningResource_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCMCI423SLRF_Id { get; set; }
        public string NCMCI423SLRF_FileName { get; set; }
        public string NCMCI423SLRF_Filedesc { get; set; }
        public string NCMCI423SLRF_FilePath { get; set; }
        public long NCMCI423SLR_Id { get; set; }
    }
}
