using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_NoticeBoard_Class_Sec")]
    public class IVRM_NoticeBoard_Class_Section_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTBCS_Id { get; set; }
        public long INTBC_Id { get; set; }
        public long ASMS_Id { get; set; }
        public bool INTBC_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long INTBC_CreatedBy { get; set; }
        public long INTBC_UpdatedBy { get; set; }


        
    }
}
