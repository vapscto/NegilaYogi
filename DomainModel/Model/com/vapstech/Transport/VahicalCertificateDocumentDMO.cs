using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Vehicle_Certificates_Documents", Schema = "TRN")]
    public class VahicalCertificateDocumentDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRVCTD_Id { get; set; }
        public long TRVCT_Id { get; set; }
        public string TRVCTD_FileName { get; set; }
        public string TRVCTD_FileLocation { get; set; }
        public bool TRVCTD_ActiveFlg { get; set; }
        public DateTime? TRVCTD_CreatedDate { get; set; }
        public DateTime? TRVCTD_UpdatedDate { get; set; }
        public long TRVCTD_CreatedBy { get; set; }
        public long TRVCTD_UpdatedBy { get; set; }



    }
}

