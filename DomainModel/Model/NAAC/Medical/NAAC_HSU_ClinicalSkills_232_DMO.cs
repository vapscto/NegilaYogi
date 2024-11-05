using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_HSU_ClinicalSkills_232")]
    public class NAAC_HSU_ClinicalSkills_232_DMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCHSUCS232_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCHSUCS232_Year { get; set; }
        public bool NCHSUCS232_CsTrclinicalskillsRelevantFlag { get; set; }
        public bool NCHSUCS232_PatientSimulatorsSimulationbasedFlag { get; set; }
        public bool NCHSUCS232_StProgConductedSssessmentStudentsFlag { get; set; }
        public bool NCHSUCS232_TrProgConForCsSblearningFlag { get; set; }
        public long NCHSUCS232_CreatedBy { get; set; }
        public long NCHSUCS232_UpdatedBy { get; set; }
        public DateTime NCHSUCS232_CreatedDate { get; set; }
        public DateTime NCHSUCS232_UpdatedDate { get; set; }
    }
}
