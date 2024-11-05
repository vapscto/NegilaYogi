using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("LMS_Live_Meeting_Class")]
    public class LMS_Live_Meeting_ClassDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         
     public long LMSLMEETCLS_Id { get; set; }
       public long LMSLMEET_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public bool LMSLMEETCLS_ActiveFlg { get; set; }
        public DateTime LMSLMEETCLS_CreatedDate { get; set; }
        public DateTime LMSLMEETCLS_UpdatedDate { get; set; }
        public long LMSLMEETCLS_CreatedBy { get; set; }
        public long LMSLMEETCLS_UpdatedBy { get; set; }

    }
}
