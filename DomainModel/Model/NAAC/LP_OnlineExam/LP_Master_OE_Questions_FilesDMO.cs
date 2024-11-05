using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Questions_Files")]
    public class LP_Master_OE_Questions_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMOEQF_Id { get; set; }
        public long? LPMOEQ_Id { get; set; }
        public string LPMOEQF_FileName { get; set; }
        public string LPMOEQF_FilePath { get; set; }
        public bool LPMOEQF_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
