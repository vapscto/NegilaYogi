using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Training_Question")]
    public class HR_Training_Question_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRTRQNS_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRTCR_Id { get; set; }
        public long HRMFQNS_Id { get; set; }
        public bool HRTRQNS_ActiveFlg { get; set; }
        public long? HRTRQNS_CreatedBy { get; set; }
        public long? HRTRQNS_UpdatedBy { get; set; }
        public DateTime? HRTRQNS_CreatedDate { get; set; }
        public DateTime? HRTRQNS_UpdatedDate { get; set; }
    }
}
