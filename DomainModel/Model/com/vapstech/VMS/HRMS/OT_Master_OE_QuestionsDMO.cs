using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VMS.HRMS
{
    [Table("OT_Master_OE_Questions")]
    public class OT_Master_OE_QuestionsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public long OTMOEQ_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMP_Id { get; set; }
        public long OTQPTYP_Id { get; set; }
        public string OTMOEQ_Question { get; set; }
        public bool OTMOEQ_SubjectiveFlg { get; set; }
        public string OTMOEQ_Answer { get; set; }
        public decimal OTMOEQ_Marks { get; set; }
        public string OTMOEQ_QuestionDesc { get; set; }
        public bool OTMOEQ_ActiveFlg { get; set; }
        public long OTMOEQ_CreatedBy { get; set; }
        public DateTime OTMOEQ_CreatedDate { get; set; }
        public long OTMOEQ_UpdatedBy { get; set; }
        public DateTime OTMOEQ_UpdatedDate { get; set; }
        public List<OT_Master_OE_QNS_OptionsDMO> OT_Master_OE_QNS_OptionsDMO { get; set; }
    }
}