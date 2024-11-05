using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Gate_Pass_Staff", Schema = "VM")]
    public class Gate_Pass_Staff_DMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long GPHST_Id { get; set; }
        public long MI_Id { get; set; } 
        public long HRME_Id { get; set; }
        public string GPHST_GatePassNo { get; set; }
        public string GPHST_IDCardNo { get; set; }
        public DateTime? GPHST_DateTime { get; set; }
        public string GPHST_Remarks { get; set; }
        public bool GPHST_ActiveFlg { get; set; }
        public long? GPHST_CreatedBy { get; set; }
        public long? GPHST_UpdatedBy { get; set; }
        public string GPHST_InTime { get; set; }
        public string GPHST_OutTime { get; set; }
    }
}
