using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Exam
{
    [Table("Exm_Student_Marks_Pro_Sub_SubSubject", Schema = "Exm")]
    public class Exm_Student_Marks_Pro_Sub_SubSubjectDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ESTMPSSS_Id { get; set; }
        public int ESTMPS_Id { get; set; }
        public int EMSS_Id { get; set; }
        public int EMSE_Id { get; set; }
        public decimal? ESTMPSSS_MaxMarks { get; set; }
        public decimal? ESTMPSSS_ObtainedMarks { get; set; }
        public string ESTMPSSS_ObtainedGrade { get; set; }
        public decimal? ESTMPSSS_ClassAverage { get; set; }
        public decimal? ESTMPSSS_SectionAverage { get; set; }
        public decimal? ESTMPSSS_ClassHighest { get; set; }
        public decimal? ESTMPSSS_SectionHighest { get; set; }
        public string ESTMPSSS_PassFailFlg { get; set; }
        public int? EMGD_Id { get; set; }

    }
}
