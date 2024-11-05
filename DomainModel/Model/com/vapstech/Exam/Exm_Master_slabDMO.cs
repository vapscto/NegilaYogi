using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Master_PointsSlab", Schema = "Exm")]
    public class Exm_Master_slabDMO 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long EMPTSL_Id { get; set; }
        public long MI_Id { get; set; }
        public decimal EMPTSL_PercentFrom { get; set; }
        public decimal EMPTSL_PercentTo { get;set;}
        public string EMPTSL_Points { get; set; }
        public bool EMPTSL_ActiveFlg { get; set; }
        public long EMPTSL_CreatedBy { get; set; }
        public long EMPTSL_UpdatedBy { get; set; }
        public DateTime? EMPTSL_CreatedDate { get; set; }
        public DateTime? EMPTSL_UpdatedDate { get; set; }

        //public List<Exm_Master_Grade_DetailsDMO> EXM_M_Grade_Details { get; set; }
    }
}
