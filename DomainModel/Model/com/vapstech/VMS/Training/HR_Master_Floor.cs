using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.VMS.Training
{
    [Table("HR_Master_Floor")]
    public class HR_Master_Floor : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HRMF_Id { get; set; }
        public long MI_Id { get; set; }
        public long HRMB_Id { get; set; }
        public string HRMF_FloorName { get; set; }
        public bool HRMF_ActiveFlag { get; set; }
        public long HRMF_CreatedBy { get; set; }
        public long HRMF_UpdatedBy { get; set; }
    }
}