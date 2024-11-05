using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.SeatingArrangment
{
    [Table("Exam_SA_ETT")]
    public class Exam_SA_ETTDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ESAETT_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public int EME_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ESAUE_Id { get; set; }
        public DateTime? ESAETT_FromDate { get; set; }
        public DateTime? ESAETT_ToDate { get; set; }
        public bool ESAETT_ActiveFlg { get; set; }
        public DateTime? ESAETT_CreatedDate { get; set; }
        public DateTime? ESAETT_UpdatedDate { get; set; }
        public long ESAETT_CreatedBy { get; set; }
        public long ESAETT_UpdatedBy { get; set; }
        public List<Exam_SA_ETT_DetailsDMO> Exam_SA_ETT_DetailsDMO { get; set; }



    }
}
