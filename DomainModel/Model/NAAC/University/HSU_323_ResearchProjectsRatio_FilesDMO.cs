using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_323_ResearchProjectsRatio_Files")]
    public class HSU_323_ResearchProjectsRatio_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NC323RPRF_Id { get; set; }
        public long NC323RPR_Id { get; set; }
        public string NC323RPRF_FileName { get; set; }
        public string NC323RPRF_Filedesc { get; set; }
        public string NC323RPRF_FilePath { get; set; }
    }
}
