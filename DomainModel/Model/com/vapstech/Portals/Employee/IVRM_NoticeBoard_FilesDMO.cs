using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_NoticeBoard_Files")]
    public class IVRM_NoticeBoard_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long INTBFL_Id { get; set; }
        public long MI_Id { get; set; }
        public long INTB_Id { get; set; }
        public string INTBFL_FileName { get; set; }
        public string INTBFL_FilePath { get; set; }
        public bool INTBFL_ActiveFlag { get; set; }
        public DateTime? INTBFL_CreatedDate { get; set; }
        public DateTime? INTBFL_UpdatedDate { get; set; }
        public long INTBFL_CreatedBy { get; set; }
        public long INTBFL_UpdatedBy { get; set; }
    }
}
