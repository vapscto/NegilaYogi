using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("IVRM_Master_Subjects")]
    public class subjectmasterDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public bool ISMS_ExamFlag { get; set; }
        public bool ISMS_PreadmFlag { get; set; }
        public bool ISMS_SubjectFlag { get; set; }
        public bool ISMS_BatchAppl { get; set; }
        public bool ISMS_ActiveFlag { get; set; }
        public string AMSU_Flag { get; set; }
        public int? ISMS_OrderFlag { get; set; }

    }
}
