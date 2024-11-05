using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Inward_data", Schema = "VM")]
    public class InwardDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IW_Id { get; set; }
        public long MI_Id { get; set; }
        public string IW_Discription { get; set; }
        public string IW_From { get; set; }
        public string IW_To { get; set; }
        public int IW_No { get; set; }
        public DateTime? IW_Date { get; set; }
        public string IW_Remarks { get; set; }
        public string IW_Action_By { get; set; }
        public string Ass_To { get; set; }
        public bool IW_ActiveFlag { get; set; }
    }
}
