using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_QNS_Options")]
    public class LP_Master_OE_QNS_OptionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LPMOEQOA_Id { get; set; }
        public long? LPMOEQ_Id { get; set; }
        public string LPMOEQOA_Option { get; set; }
        public string LPMOEQOA_OptionCode { get; set; }
        public bool LPMOEQOA_AnswerFlag { get; set; }
        public bool LPMOEQOA_ActiveFlg { get; set; }
        public long LPMOEQOA_CreatedBy { get; set; }
        public long LPMOEQOA_UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public decimal? LPMOEQOA_Marks { get; set; }
        public List<LP_Master_OE_QNS_Options_FilesDMO> LP_Master_OE_QNS_Options_FilesDMO { get; set; }
        public List<LP_Master_OE_QNS_Options_MFDMO> LP_Master_OE_QNS_Options_MFDMO { get; set; }
    }
}