using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{   
    [Table("CMS_Master_Installments", Schema = "CMS")]
    public class CMS_Master_InstallmentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSMINST_Id { get; set; }
        public long CMSMINSTTY_Id { get; set; }
        public string CMSMINST_InstallmentName { get; set; }
        public DateTime? CMSMINST_FromDate { get; set; }
        public string CMSMINST_FromMonth { get; set; }
        public DateTime? CMSMINST_ToDate { get; set; }
        public string CMSMINST_ToMonth { get; set; }
        public DateTime? CMSMINST_ApplicableDate { get; set; }
        public string CMSMINST_ApplMonth { get; set; }
        public bool CMSMINST_ActiveFlag { get; set; }
        public DateTime? CMSMINST_CreatedDate { get; set; }
        public long CMSMINST_CreatedBy { get; set; }
        public DateTime? CMSMINST_UpdatedDate { get; set; }
        public long CMSMINST_UpdatedBy { get; set; }

    }
}
