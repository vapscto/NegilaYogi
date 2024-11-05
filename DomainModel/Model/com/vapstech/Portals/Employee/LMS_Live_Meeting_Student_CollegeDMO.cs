using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("LMS_Live_Meeting_Student_College")]
    public class LMS_Live_Meeting_Student_CollegeDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSLMEETSTDCOL_Id { get; set; }
        public long LMSLMEET_Id { get; set; }
        public long AMCST_Id { get; set; }
        public string LMSLMEETSTDCOL_LoginTime { get; set; }
        public string LMSLMEETSTDCOL_MACAddress { get; set; }
        public string LMSLMEETSTDCOL_ISPAddress { get; set; }
        public string LMSLMEETSTDCOL_LogoutTime { get; set; }
        public bool LMSLMEETSTDCOL_ActiveFlg { get; set; }
        public DateTime LMSLMEETSTDCOL_CreatedDate { get; set; }
        public DateTime LMSLMEETSTDCOL_UpdatedDate { get; set; }

    }
}
