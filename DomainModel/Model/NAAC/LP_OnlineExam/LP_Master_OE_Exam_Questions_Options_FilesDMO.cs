using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Exam_Questions_Options_Files")]
    public class LP_Master_OE_Exam_Questions_Options_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMOEEXQNSOPTF_Id { get; set; }
        public long LPMOEEXQNSOPT_Id { get; set; }
        public string LPMOEEXQNSOPTF_FileName { get; set; }
        public string LPMOEEXQNSOPTF_FilePath { get; set; }
        public bool? LPMOEEXQNSOPTF_ActiveFlag { get; set; }
        public DateTime? LPMOEEXQNSOPTF_CreatedDate { get; set; }
        public DateTime? LPMOEEXQNSOPTF_UpdatedDate { get; set; }
        public long? LPMOEEXQNSOPTF_CreatedBy { get; set; }
        public long? LPMOEEXQNSOPTF_UpdatedBy { get; set; }
    }
}
