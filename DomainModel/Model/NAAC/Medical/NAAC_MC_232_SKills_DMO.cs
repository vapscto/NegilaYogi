using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_232_SKills")]
    public class NAAC_MC_232_SKills_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCS232_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCMCS232_Year { get; set; }
        public bool NCMCS232_InstClinicalSkillsFlag { get; set; }
        public bool NCMCS232_InstAdvsimulationBasedTrainingFlag { get; set; }
        public bool NCMCS232_StuProgTrAsstofStudentsFlag { get; set; }
        public bool NCMCS232_StuProgTrAsstClORSimulationLrnFlag { get; set; }
        public long NCMCS232_CreatedBy { get; set; }
        public long NCMCS232_UpdatedBy { get; set; }
        public DateTime NCMCS232_CreateDate { get; set; }
        public DateTime NCMCS232_UpdatedDate { get; set; }
    }
}
