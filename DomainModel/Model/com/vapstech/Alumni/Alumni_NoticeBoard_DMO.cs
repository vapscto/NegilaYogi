using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_NoticeBoard", Schema = "ALU")]
    public class Alumni_NoticeBoard_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALNTB_Id { get; set; }
        public long MI_Id { get; set; }
        public string ALNTB_Title { get; set; }
        public string ALNTB_Description { get; set; }
        public string ALNTB_FilePath { get; set; }
        public string ALNTB_Attachment { get; set; }
        public DateTime? ALNTB_DisplayDate { get; set; }
        public DateTime? ALNTB_StartDate { get; set; }
        public DateTime? ALNTB_EndDate { get; set; }
        public string ALNTB_TTSylabusFlg { get; set; }
        public bool ALNTB_ActiveFlag { get; set; }
        public DateTime? ALNTB_CreatedDate { get; set; }
        public DateTime? ALNTB_UpdatedDate { get; set; }
       
        public List<Alumni_NoticeBoard_Files_DMO> Alumni_NoticeBoard_Files_DMO { get; set; }
        
    }
}

