using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Exam_Questions_Options_MF")]
    public class LP_Master_OE_Exam_Questions_Options_MFDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMOEEXQNSOPTMF_Id { get; set; }
        public long LPMOEEXQNSOPT_Id { get; set; }
        public string LPMOEEXQNSOPTMF_MatchtheFollowing { get; set; }
        public long? LPMOEEXQNSOPTMF_Answer_LPMOEEXQNSOPT { get; set; }  
        public bool? LPMOEEXQNSOPTMF_ActiveFlg { get; set; }  
        public long? LPMOEEXQNSOPTMF_CreatedBy { get; set; }
        public DateTime? LPMOEEXQNSOPTMF_CreatedDate { get; set; }
        public long? LPMOEEXQNSOPTMF_UpdatedBy { get; set; }
        public DateTime? LPMOEEXQNSOPTMF_UpdatedDate { get; set; }
        public bool? LPMOEEXQNSOPTMF_Answer_Flg { get; set; }
        public int? LPMOEEXQNSOPTMF_Order { get; set; }
    }
}
