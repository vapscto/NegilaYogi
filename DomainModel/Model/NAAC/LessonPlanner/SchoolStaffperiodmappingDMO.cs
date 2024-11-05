using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.Exam.LessonPlanner
{
    [Table("LP_LessonPlanner")]
    public class SchoolStaffperiodmappingDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LPLP_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }       
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long LPMT_Id { get; set; }
        public DateTime LPLP_LPDate { get; set; }
        public DateTime LPLP_CTDate { get; set; }
        public bool LPLP_ClassTakenFlg { get; set; }
        public bool LPLP_ActiveFlag { get; set; }
        public long LPMT_CreatedBy { get; set; }
        public long LPMT_UpdatedBy { get; set; }

    }
}
