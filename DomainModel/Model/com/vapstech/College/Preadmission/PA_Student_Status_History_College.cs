using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.College.Preadmission
{
    [Table("PA_Student_Status_History_College", Schema = "CLG")]
    public class PA_Student_Status_History_College
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASSHC_Id { get; set; }
        public long PACA_Id { get; set; }
        public string PASSHC_Status { get; set; }
        public DateTime PASSHC_Date { get; set; }
        //public Decimal? PACSTSUM_MinMarks { get; set; }
        //public Decimal PACSTSUM_SubjectMarks { get; set; }
        //public Decimal PACSTSUM_Percentage { get; set; }
        //public string PACSTSUM_LangFlg { get; set; }
    }
}
