using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Outward_data", Schema = "VM")]
    public class OutwardDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long OW_Id { get; set; }
        public long MI_Id { get; set; }
        public string OW_Discription { get; set; }
        public string OW_From { get; set; }
        public string OW_To { get; set; }
        public string OW_add { get; set; }
        public string OW_Date { get; set; }
        public string OW_Remarks { get; set; }
        public string OW_Action_By { get; set; }
        public bool OW_ActiveFlag { get; set; }
    }
}
