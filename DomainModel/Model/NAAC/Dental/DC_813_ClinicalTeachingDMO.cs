using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Dental
{
    [Table("NAAC_DC_813_ClinicalTeaching")]
   public class DC_813_ClinicalTeachingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCDCCL813_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCDCCL813_Year { get; set; }
        public bool NCDCCL813_CentralSterileSuppliesDepartmentFlag { get; set; }
        public bool NCDCCL813_ProvidesPersonalProtectiveEquipmentFlag { get; set; }
        public bool NCDCCL813_PatientSafetyCurriculumFlag { get; set; }
        public bool NCDCCL813_PeriodicFumigationClinicalAreasFlag { get; set; }
        public bool NCDCCL813_ImmunizationOfAllTheCaregiversFlag { get; set; }
        public bool NCDCCL813_NeedleStickInjuryRegisterFlag { get; set; }
        public bool NCDCCL813_ActiveFlag { get; set; }
        public long NCDCCL813_CreatedBy { get; set; }
        public long NCDCCL813_UpdatedBy { get; set; }
        public DateTime? NCDCCL813_CreatedDate { get; set; }
        public DateTime? NCDCCL813_UpdatedDate { get; set; }
    }
}
