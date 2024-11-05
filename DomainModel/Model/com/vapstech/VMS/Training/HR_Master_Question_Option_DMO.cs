using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_Question_Option")]
    public class HR_Master_Question_Option_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMQNOP_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMFQNS_Id { get; set; }
        public long HRMFOPT_Id { get; set; }
        public bool HRMQNOP_ActiveFlg { get; set; }
        public long? HRMQNOP_CreatedBy { get; set; }
        public long? HRMQNOP_UpdatedBy { get; set; }
        public DateTime? HRMQNOP_CreatedDate { get; set; }
        public DateTime? HRMQNOP_UpdatedDate { get; set; }
    }
}
