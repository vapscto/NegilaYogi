using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("Adm_Certificates_Apply")]
    public class Adm_Certificates_Apply_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ACERTAPP_Id { get; set; }
        public long MI_Id { get; set; }
        public string ACERTAPP_CertificateName { get; set; }
        public string ACERTAPP_CertificateCode { get; set; }
        public bool ACERTAPP_ApprovaReqlFlg { get; set; }
        public bool ACERTAPP_OnlineDownloadFlg { get; set; }
        public bool ACERTAPP_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long ACERTAPP_CreatedBy { get; set; }
        public long ACERTAPP_UpdatedBy { get; set; }
    }
}
