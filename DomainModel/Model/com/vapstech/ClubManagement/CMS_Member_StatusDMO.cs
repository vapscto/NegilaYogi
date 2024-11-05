using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.ClubManagement
{
    [Table("CMS_Member_Status", Schema = "CMS")]
    public class CMS_Member_StatusDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CMSMEMSTS_Id { get; set; }
        public long MI_Id { get; set; }
        public long CMSMMEM_Id { get; set; }
        public long IMFY_Id { get; set; }
        public decimal CMSMEMSTS_OpeningBalance { get; set; }
        public string CMSMEMSTS_OBCRDRFlg { get; set; }
        public decimal CMSMEMSTS_TotalDR { get; set; }
        public decimal CMSMEMSTS_TotalDRTrans { get; set; }
        public decimal CMSMEMSTS_TotalCRTrans { get; set; }
        public decimal CMSMEMSTS_ClosingBalance { get; set; }
        public string CMSMEMSTS_CBDRDRFlg { get; set; }
        public bool CMSMEMSTS_ActiveFlg { get; set; }
        public DateTime? CMSMEMSTS_CreatedDate { get; set; }
        public long CMSMEMSTS_CreatedBy { get; set; }
        public DateTime? CMSMEMSTS_UpdatedDate { get; set; }
        public long CMSMEMSTS_UpdatedBy { get; set; }



    }
}
