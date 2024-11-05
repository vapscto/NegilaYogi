using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.University
{
    [Table("NAAC_HSU_InterdisciplinaryProgrammes_123")]
    public class NAAC_HSU_InterdisciplinaryProgrammes_123_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

      public long NCHSUIP123_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSUIP123_Year { get; set; }
        public long NCHSUIP123_TotalNoOfProg { get; set; }
        public long NCHSUIP123_TotalNoOfCoursesAcrossProgs { get; set; }
        public long NCHSUIP123_NoOfInterdisciplinaryCoursesAcsProg { get; set; }
        public bool NCHSUIP123_ActiveFlag { get; set; }
        public long NCHSUIP123_CreatedBy { get; set; }
        public long NCHSUIP123_UpdatedBy { get; set; }
        public DateTime NCHSUIP123_CreatedDate { get; set; }
        public DateTime NCHSUIP123_UpdatedDate { get; set; }
        public List<NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO> NAAC_HSU_InterdisciplinaryProgrammes_123_Files_DMO { get; set; }
    }
}
