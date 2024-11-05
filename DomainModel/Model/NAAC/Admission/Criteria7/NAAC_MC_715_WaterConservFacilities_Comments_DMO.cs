using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria7
{
    [Table("NAAC_MC_715_WaterConservFacilities_Comments")]
   public class NAAC_MC_715_WaterConservFacilities_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long NCMC715WCFC_Id { get; set; }
        public string NCMC715WCFC_Remarks { get; set; }
        public long? NCMC715WCFC_RemarksBy { get; set; }
        public string NCMC715WCFC_StatusFlg { get; set; }
        public bool? NCMC715WCFC_ActiveFlag { get; set; }
        public long? NCMC715WCFC_CreatedBy { get; set; }
        public DateTime? NCMC715WCFC_CreatedDate { get; set; }
        public long? NCMC715WCFC_UpdatedBy { get; set; }
        public DateTime? NCMC715WCFC_UpdatedDate { get; set; }
        public long NCMC715WCF_Id { get; set; }

    }
}
