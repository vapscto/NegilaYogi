using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_423_StuLearningResource")]
   public class NAAC_MC_423_StuLearningResourceDMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCI423SLR_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCI423SLR_Year { get; set; }
        public string NCMCI423SLR_ResourcesName { get; set; }
        public long NCMCI423SLR_NoOfUGStudentsExposed { get; set; }
        public long NCMCI423SLR_NoOfPGStudentsExposed { get; set; }
        public long NCMCI423SLR_CreatedBy { get; set; }
        public long NCMCI423SLR_UpdatedBy { get; set; }
        public DateTime? NCMCI423SLR_CreateDate { get; set; }
        public DateTime? NCMCI423SLR_UpdatedDate { get; set; }
        public List<NAAC_MC_423_StuLearningResource_FilesDMO> NAAC_MC_423_StuLearningResource_FilesDMO { get; set; }
    }
}
