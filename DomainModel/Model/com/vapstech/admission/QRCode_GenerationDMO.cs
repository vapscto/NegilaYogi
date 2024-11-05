using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.admission
{
  public  class QRCode_GenerationDMO
    {
        [Table("IVRM_QRCode_Generation_Details")]
        public class QR_Code_GenerateDMO
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public long IQRGD_Id { get; set; }
            public long MI_Id { get; set; }
            public long Amst_Id { get; set; }
            public string IQRGD_URL { get; set; }
            public string IQRGD_QRCode { get; set; }

            public DateTime IQRGD_CreatedDate { get; set; }
            public DateTime IQRGD_UpdatedDate { get; set; }
            public long IQRGD_CreatedBy { get; set; }
            public long IQRGD_UpdatedBy { get; set; }

        }
    }
}
