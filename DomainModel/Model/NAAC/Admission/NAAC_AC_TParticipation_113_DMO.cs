using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_TParticipation_113")]
    public class NAAC_AC_TParticipation_113_DMO
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCACTP113_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string NCACTP113_BodyName { get; set; }
        public long NCACTP113_ParticipatedYear { get; set; }
        public bool NCACTP113_ActiveFlg { get; set; }
        public long NCACTP113_CreatedBy { get; set; }
        public long NCACTP113_UpdatedBy { get; set; }
        public DateTime? NCACTP113_CreatedDate { get; set; }
        public DateTime? NCACTP113_UpdatedDate { get; set; }
        public DateTime? NCACTP113_PDate { get; set; }
        public string NCACTP113_StatusFlg { get; set; }
        public string NCACTP113_Remarks { get; set; }
        public bool? NCACTP113_ApprovedFlg { get; set; }
        //added by sanjeev
        public bool? NCACTP113_FromExelImportFlag { get; set; }
        public bool? NCACTP113_FreezeFlag { get; set; }
        //adddd close
        public List<NAAC_AC_TParticipation_113_FilesDMO> NAAC_AC_TParticipation_113_FilesDMO { get; set; }

    }
}
