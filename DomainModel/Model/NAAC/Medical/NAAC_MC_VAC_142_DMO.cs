using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_VAC_142")]
    public class NAAC_MC_VAC_142_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCVAC142_Id { get; set; }
        public long MI_Id { get; set; }
        public bool NCMCVAC142_FKCollAnlInstWebsite { get; set; }
        public bool NCMCVAC142_FKCollAnlFk { get; set; }
        public bool NCMCVAC142_FKCollanalysed { get; set; }
        public bool NCMCVAC142_FKcollected { get; set; }
        public bool NCMCVAC142_FKNotcollected { get; set; }
        public DateTime NCMCVAC142_CreatedDate { get; set; }
        public DateTime NCMCVAC142_UpdatedDate { get; set; }
        public long NCMCVAC142_CreatedBy { get; set; }
        public long NCMCVAC142_UpdatedBy { get; set; }
        public long NCMCVAC142_year { get; set; }
    }
}
