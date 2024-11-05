using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_NoticeBoard_Class")]
    public class IVRM_NoticeBoard_Class_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTBC_Id { get; set; }
        public long INTB_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool INTBC_ActiveFlag { get; set; }
        public List<IVRM_NoticeBoard_Class_Section_DMO> IVRM_NoticeBoard_Class_Section_DMO { get; set; }
    }
}
