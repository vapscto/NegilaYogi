using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_College_Student_SubjectMarks", Schema = "CLG")]
    public class PA_College_Student_SubjectMarks : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PACSTSUM_Id { get; set; }
        public long PACA_Id { get; set; }
        public string PACSTSUM_SubjectName { get; set; }
        public Decimal PACSTSUM_MaxMarks { get; set; }
        public Decimal? PACSTSUM_MinMarks { get; set; }
        public Decimal PACSTSUM_SubjectMarks { get; set; }
        public Decimal PACSTSUM_Percentage { get; set; }
        public string PACSTSUM_LangFlg { get; set; }

    }
}
