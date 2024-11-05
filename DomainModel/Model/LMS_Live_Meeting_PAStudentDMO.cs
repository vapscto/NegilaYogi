using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model
{
    [Table("LMS_Live_Meeting_PAStudent")]
    public class LMS_Live_Meeting_PAStudentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSLMEETPASTD_Id { get; set; }
        public long LMSLMEET_Id { get; set; }
        public long PASR_Id { get; set; }
        public string LMSLMEETPASTD_LoginTime { get; set; }
        public string LMSLMEETPASTD_MACAddress { get; set; }
        public string LMSLMEETPASTD_IPAddress { get; set; }
        public string LMSLMEETPASTD_LogoutTime { get; set; }
        public bool LMSLMEETPASTD_ActiveFlg { get; set; }
        public DateTime? LMSLMEETPASTD_CreatedDate { get; set; }
        public DateTime? LMSLMEETPASTD_UpdatedDate { get; set; }
        public long LMSLMEETPASTD_CreatedBy { get; set; }
        public long LMSLMEETPASTD_UpdatedBy { get; set; }
    }
}
