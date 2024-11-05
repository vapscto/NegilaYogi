using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("LMS_Live_Meeting_StaffOthers")]
    public class LMS_Live_Meeting_StaffOthersDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSLMEETSTFOTH_Id { get; set; }
        public long LMSLMEET_Id { get; set; }
        public long User_Id { get; set; }
        public long HRME_Id { get; set; }
        public string LMSLMEETSTFOTH_LoginTime { get; set; }
        public string LMSLMEETSTFOTH_LogoutTime { get; set; }
        public string LMSLMEETSTFOTH_MACAddress { get; set; }
        public string LMSLMEETSTFOTH_IPAddress { get; set; }
        public string LMSLMEETSTFOTH_Remarks { get; set; }
        public string LMSLMEETSTFOTH_Grade { get; set; }
        public bool LMSLMEETSTFOTH_ActiveFlg { get; set; }
        public DateTime LMSLMEETSTFOTH_CreatedDate { get; set; }
        public DateTime LMSLMEETSTFOTH_UpdatedDate { get; set; }
        public long LMSLMEETSTFOTH_CreatedBy { get; set; }
        public long LMSLMEETSTFOTH_UpdatedBy { get; set; }

    }
}
