using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_HSU_StudentLearningLevels_221")]
    public class NAAC_HSU_StudentLearningLevels_221_DMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCHSUSLL221_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSUSLL221_Year { get; set; }
        public bool NCHSUSLL221_MsCrRegSlowPerformersFlag { get; set; }
        public bool NCHSUSLL221_MsCrRegAdLearnersFlag { get; set; }
        public bool NCHSUSLL221_SplProgSlowAdLearnersFlag { get; set; }
        public bool NCHSUSLL221_ProtocolsmesAchievementsFlag { get; set; }
        public long NCHSUSLL221_CreatedBy { get; set; }
        public long NCHSUSLL221_UpdatedBy { get; set; }
        public DateTime NCHSUSLL221_CreatedDate { get; set; }
        public DateTime NCHSUSLL221_UpdatedDate { get; set; }
    }
}
