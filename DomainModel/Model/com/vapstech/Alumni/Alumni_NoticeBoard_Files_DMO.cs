using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_NoticeBoard_Files", Schema = "ALU")]
    public class Alumni_NoticeBoard_Files_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALNTBFL_Id { get; set; }
        public long ALNTB_Id { get; set; }
        public long MI_Id { get; set; }
        public string ALNTBFL_FileName { get; set; }
        public string ALNTBFL_FilePath { get; set; }
        public bool ALNTBFL_ActiveFlag { get; set; }
        public DateTime? ALNTBFL_CreatedDate { get; set; }
        public DateTime? ALNTBFL_UpdatedDate { get; set; }
        public long ALNTBFL_CreatedBy { get; set; }
        public long ALNTBFL_UpdatedBy { get; set; }
    }
}
