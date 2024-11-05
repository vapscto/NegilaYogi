using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_254_CourseImprovement")]
    public class NAAC_MC_254_CourseImprovement_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCCI254_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCCI254_Year { get; set; }
        public bool NCMCCI254_TimelyAdministrationCIEFlag { get; set; }
        public bool NCMCCI254_OnTimeAssessmentFeedbackFlag { get; set; }
        public bool NCMCCI254_MakeupAssignmentsFlag { get; set; }
        public bool NCMCCI254_RemedialTeachingFlag { get; set; }
        public long NCMCCI254_CreatedBy { get; set; }
        public long NCMCCI254_UpdatedBy { get; set; }
        public DateTime NCMCCI254_CreateDate { get; set; }
        public DateTime NCMCCI254_UpdatedDate { get; set; }

    }
}
