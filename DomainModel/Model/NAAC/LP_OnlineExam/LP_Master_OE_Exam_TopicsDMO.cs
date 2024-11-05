using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.NAAC.LP_OnlineExam
{
    [Table("LP_Master_OE_Exam_Topics")]
    public class LP_Master_OE_Exam_TopicsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long  LPMOEEXTOP_Id { get; set; }
        public long LPMOEEX_Id { get; set; }
        public long LPMT_Id { get; set; }
        public bool LPMOEEXTOP_ActiveFlg { get; set; }
        public long LPMOEEXQNS_CreatedBy { get; set; }
        public DateTime LPMOEEXQNS_CreatedDate { get; set; }
        public long LPMOEEXQNS_UpdatedBy { get; set; }
        public DateTime LPMOEEXQNS_UpdatedDate { get; set; }
    }
}
