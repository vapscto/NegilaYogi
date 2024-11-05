using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Master_InstallmentType", Schema = "CMS")]
    public class CMS_Master_InstallmentTypeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSMINSTTY_Id { get; set; }
        public long MI_Id { get; set; }
        public string CMSMINSTTY_InstallmentType { get; set; }
        public string CMSMINSTTY_InstallmentTypeFlg { get; set; }
        public long CMSMINSTTY_Duration { get; set; }
        public string CMSMINSTTY_DurationFlg { get; set; }
        public bool CMSMINSTTY_ActiveFlag { get; set; }
        public DateTime? CMSMINSTTY_CreatedDate { get; set; }
        public long CMSMINSTTY_CreatedBy { get; set; }
        public DateTime? CMSMINSTTY_UpdatedDate { get; set; }
        public long CMSMINSTTY_UpdatedBy { get; set; }
    }
    //IVRM_Month
}
