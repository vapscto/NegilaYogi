using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.OnlineExam
{
    [Table("PA_Master_OE_Questions_Files")]
    public class PA_Master_OE_Questions_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PAMOEQF_Id { get; set; }
        public long PAMOEQ_Id { get; set; }
        public string PAMOEQF_FileName { get; set; }
        public string PAMOEQF_FilePath { get; set; }
        public bool PAMOEQF_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
