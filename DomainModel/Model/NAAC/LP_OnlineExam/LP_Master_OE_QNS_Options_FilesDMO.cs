using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_QNS_Options_Files")]
    public class LP_Master_OE_QNS_Options_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMOEQOAF_Id { get; set; }
        public long LPMOEQOA_Id { get; set; }
        public string LPMOEQOAF_FileName { get; set; }
        public string LPMOEQOAF_FilePath { get; set; }
        public bool? LPMOEQOAF_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
