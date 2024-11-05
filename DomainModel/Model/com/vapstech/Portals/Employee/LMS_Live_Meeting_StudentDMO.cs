using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("LMS_Live_Meeting_Student")]
    public class LMS_Live_Meeting_StudentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSLMEETSTD_Id { get; set; }
        public long LMSLMEET_Id { get; set; }
        public long AMST_Id { get; set; }
        public string LMSLMEETSTD_LoginTime { get; set; }
        public string LMSLMEETSTD_MACAddress { get; set; }
        public string LMSLMEETSTD_IPAddress { get; set; }
        public bool LMSLMEETSTD_ActiveFlg { get; set; }
        public DateTime LMSLMEETSTD_CreatedDate { get; set; }
        public DateTime LMSLMEETSTD_UpdatedDate { get; set; }
        public long LMSLMEETSTD_CreatedBy { get; set; }
        public long LMSLMEETSTD_UpdatedBy { get; set; }
        public string LMSLMEETSTD_LogoutTime { get; set; }

    }
}
