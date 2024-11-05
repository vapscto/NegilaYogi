using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Medical
{
    [Table("NAAC_MC_819_ClinicalLaboratory")]
  public  class MC_819_Accredition_ClinicallabDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCMCCL819_Id { get; set; }
        public long MI_Id { get; set; }
        public bool NCMCCL819_NABHAccnTechHoslFlg { get; set; }
        public bool NCMCCL819_NABHAccnTechlabslFlg { get; set; }
        public bool NCMCCL819_CertificationDeptlFlg { get; set; }
        public bool NCMCCL819_OtherRecAccCertificationFlg { get; set; }
        public bool NCMCCL819_ActiveFlag { get; set; }
        public long NCMCCL819_CreatedBy { get; set; }
        public long NCMCCL819_UpdatedBy { get; set; }
        public string NCMCCL819_StatusFlg { get; set; }
        public DateTime? NCMCCL819_CreatedDate { get; set; }
        public DateTime? NCMCCL819_UpdatedDate { get; set; }
        public long NCMCCL819_Year { get; set; }
    }
}
