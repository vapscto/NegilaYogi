using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_Feedback_Option")]
    public class HR_Master_Feedback_Option_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMFOPT_Id { get; set; }
        public long MI_Id { get; set; }
        public string HRMFOPT_OptionName { get; set; }
        public int HRMFOPT_OptionOrder { get; set; }
        public bool HRMFOPT_ActiveFlg { get; set; }
        public long? HRMFOPT_CreatedBy { get; set; }
        public long? HRMFOPT_UpdatedBy { get; set; }
        public DateTime? HRMFOPT_CreatedDate { get; set; }
        public DateTime? HRMFOPT_UpdatedDate { get; set; }
        public string HRMFOPT_OptionFor { get; set; }
    }
}
