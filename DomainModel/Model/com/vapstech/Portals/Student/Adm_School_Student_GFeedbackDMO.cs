using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("Adm_School_Student_GFeedback")]
    public class Adm_School_Student_GFeedbackDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public long ASGFE_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASGFE_FeedBack { get; set; }
        public DateTime? ASGFE_FeedbackDate { get; set; }
        public bool ASGFE_ActiveFlag { get; set; }
        public long ASGFE_CreatedBy { get; set; }
        public long ASGFE_UpdatedBy { get; set; }


    }
}

