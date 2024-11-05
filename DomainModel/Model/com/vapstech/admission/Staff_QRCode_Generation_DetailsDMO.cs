using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Staff_QRCode_Generation_Details")]
    public class Staff_QRCode_Generation_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SQRGD_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRME_Id { get; set; }
        public string SQRGD_URL { get; set; }
        public string SQRGD_QRCode { get; set; }

        public DateTime SQRGD_CreatedDate { get; set; }
        public DateTime SQRGD_UpdatedDate { get; set; }
        public long SQRGD_CreatedBy { get; set; }
        public long SQRGD_UpdatedBy { get; set; }

    }
}
