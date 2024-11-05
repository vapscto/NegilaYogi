using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_316_Dept_Awards")]
    public class NAAC_HSU_316_Dept_AwardsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NMC316DA_Id { get; set; }
        public long MI_Id { get;set;}
        public long HRMD_Id { get;set;}
        public long NMC316DA_Year { get;set;}
        public string NMC316DA_Scheme { get;set;}
        public string NMC316DA_Agency { get;set;}
        public string NMC316DA_FoundProvided { get;set;}
        public string NMC316DA_Duration { get;set;}
        public DateTime? NMC316DA_CreatedDate { get;set;}
        public DateTime? NMC316DA_UpdatedDate { get;set;}
        public bool NMC316DA_ActiveFlag { get; set; }
        public List<NAAC_HSU_316_Dept_Awards_FilesDMO> NAAC_HSU_316_Dept_Awards_FilesDMO { get; set; }
    }
}
