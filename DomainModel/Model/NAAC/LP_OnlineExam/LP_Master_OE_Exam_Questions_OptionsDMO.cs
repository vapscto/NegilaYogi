using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Exam_Questions_Options")]
    public class LP_Master_OE_Exam_Questions_OptionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long LPMOEEXQNSOPT_Id { get; set; }
        public long LPMOEEXQNS_Id { get; set; }
        public string LPMOEEXQNSOPT_Option { get; set; }
        public string LPMOEEXQNSOPT_OptionCode { get; set; }
        public string LPMOEEXQNSOPT_OptionImage { get; set; }
        public bool? LPMOEEXQNSOPT_AnswerFlag { get; set; }
        public string LPMOEEXQNSOPT_AnswerDesc { get; set; }
        public bool? LPMOEEXQNSOPT_ActiveFlg { get; set; }
        public long? LPMOEEXQNSOPT_CreatedBy { get; set; }
        public DateTime? LPMOEEXQNSOPT_CreatedDate { get; set; }
        public long? LPMOEEXQNSOPT_UpdatedBy { get; set; }
        public DateTime? LPMOEEXQNSOPT_UpdatedDate { get; set; }
        public decimal? LPMOEEXQNSOPT_Marks { get; set; }
        public List<LP_Master_OE_Exam_Questions_Options_MFDMO> LP_Master_OE_Exam_Questions_Options_MFDMO { get; set; }
        public List<LP_Master_OE_Exam_Questions_Options_FilesDMO> LP_Master_OE_Exam_Questions_Options_FilesDMO { get; set; }
    }
}