using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_QNS_Options_MF")]
    public class LP_Master_OE_QNS_Options_MFDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPMOEQOAMF_Id { get; set; }
        public long LPMOEQOA_Id { get; set; }
        public string LPMOEQOAMF_MatchtheFollowing { get; set; }
        public long? LPMOEQOAMF_Answer_LPMOEQOA_Id { get; set; }
        public bool? LPMOEQOAMF_ActiveFlg { get; set; }
        public bool? LPMOEQOAMF_AnswerFlag { get; set; }
        public long? LPMOEQOAMF_CreatedBy { get; set; }
        public DateTime? LPMOEQOAMF_CreatedDate { get; set; }
        public long? LPMOEQOAMF_UpdatedBy { get; set; }
        public DateTime? LPMOEQOAMF_UpdatedDate { get; set; }
        public int? LPMOEQOAMF_Order { get; set; }
    }
}