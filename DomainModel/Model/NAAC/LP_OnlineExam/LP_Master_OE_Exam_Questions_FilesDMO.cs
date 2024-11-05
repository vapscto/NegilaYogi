using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Exam_Questions_Files")]
    public class LP_Master_OE_Exam_Questions_FilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMOEEXQNSF_Id { get; set; }
        public long LPMOEEXQNS_Id { get; set; }
        public string LPMOEEXQNSF_FileName { get; set; }
        public string LPMOEEXQNSF_FilePath { get; set; }
        public bool? LPMOEEXQNSF_ActiveFlag { get; set; }
        public DateTime? LPMOEEXQNSF_CreatedDate { get; set; }
        public DateTime? LPMOEEXQNSF_UpdatedDate { get; set; }
        public long? LPMOEEXQNSF_CreatedBy { get; set; }
        public long? LPMOEEXQNSF_UpdatedBy { get; set; }
    }
}
