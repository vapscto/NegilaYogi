using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Exam
{
  
    [Table("Exm_YCExams_SplUsers", Schema = "Exm")]
    public class UserPromotion_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long EYCESU_Id { get; set; }
        public int EYCE_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public DateTime? EYCESU_MarksEntryFromDate { get; set; }
        public DateTime? EYCESU_MarksEntryToDate { get; set; }
        public DateTime? EYCESU_MarksProcessFromDate { get; set; }
        public DateTime? EYCESU_MarksProcessToDate { get; set; }
        public DateTime? EYCESU_MarksPublishDate { get; set; }
        public bool EYCESU_ActiveFlg { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public bool? EYCESU_MarksPublishApproverFlg { get; set; }
    }
}
