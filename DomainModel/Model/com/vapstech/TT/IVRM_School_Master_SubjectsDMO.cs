using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("IVRM_Master_Subjects")]

    public class IVRM_School_Master_SubjectsDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ISMS_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public string ISMS_IVRSSubjectName { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long ISMS_ExamFlag { get; set; }
        public long ISMS_PreadmFlag { get; set; }
        public long ISMS_SubjectFlag { get; set; }
        public long ISMS_BatchAppl { get; set; }
        public long ISMS_ActiveFlag { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public bool ISMS_TTFlag { get; set; }
        public bool ISMS_AttendanceFlag { get; set; }
        public int ISMS_LanguageFlg { get; set; }
        public int ISMS_AtExtraFeeFlg { get; set; }
        public long? ISMS_IntroYear { get; set; }
        public string ISMS_FileName { get; set; }
        public string ISMS_FilePath { get; set; }
        public bool? ISMS_DiscontinuedFlg { get; set; }
        public long? ISMS_DiscontinuedYear { get; set; }
        public string ISMS_DiscontinuedReason { get; set; }
        public string ISMS_SubjectNameNew { get; set; }
        public List<WIrttenTestSubjectWiseMarksDMO> WIrttenTestSubjectWiseMarksDMO { get; set; }
    }
}
