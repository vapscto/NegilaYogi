using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Studentwise_Subjects", Schema = "Exm")]
    public class StudentMappingDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ESTSU_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMG_Id { get; set; }
        public bool ESTSU_ElecetiveFlag { get; set; }
        public bool ESTSU_ActiveFlg { get; set; }
        public long? ESTSU_CreatedBy { get; set; }
        public long? ESTSU_UpdatedBy { get; set; }
        public int? EME_Id { get; set; }
    }
}
