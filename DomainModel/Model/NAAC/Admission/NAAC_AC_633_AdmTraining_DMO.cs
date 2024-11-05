using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_633_AdmTraining")]
    public class NAAC_AC_633_AdmTraining_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC633ADMTRG_Id { get; set; }
        public long MI_Id { get; set; }
        public long NCAC633ADMTRG_Year { get; set; }
        public string NCAC633ADMTRG_Title { get; set; }
        public DateTime NCAC633ADMTRG_FromDate { get; set; }
        public DateTime NCAC633ADMTRG_ToDate { get; set; }
        public string NCAC633ADMTRG_ProfDevAdmTrgFlg { get; set; }
        public long NCAC633ADMTRG_NoOfParticipants { get; set; }
        public bool NCAC633ADMTRG_ActiveFlg { get; set; }
        public long NCAC633ADMTRG_CreatedBy { get; set; }
        public long NCAC633ADMTRG_UpdatedBy { get; set; }
        public DateTime NCAC633ADMTRG_CreatedDate { get; set; }
        public string NCAC633ADMTRG_StatusFlg { get; set; }
        public bool? NCAC633ADMTRG_ApprovedFlg { get; set; }
        public string NCAC633ADMTRG_Remarks { get; set; }
        public DateTime NCAC633ADMTRG_UpdatedDate { get; set; }

        public List<NAAC_AC_633_AdmTraining_files_DMO> NAAC_AC_633_AdmTraining_files_DMO { get; set; }
    }
}
