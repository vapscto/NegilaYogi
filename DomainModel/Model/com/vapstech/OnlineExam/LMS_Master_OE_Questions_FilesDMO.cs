using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("LMS_Master_OE_Questions_Files")]
    public class LMS_Master_OE_Questions_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMSMOEQF_Id { get; set; }
        public long LMSMOEQ_Id { get; set; }
        public string LMSMOEQF_FileName { get; set; }
        public string LMSMOEQF_FilePath { get; set; }
        public bool LMSMOEQF_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}