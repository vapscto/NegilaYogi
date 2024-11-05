using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.IVRS
{
    [Table("IVRS_Call_Status")]
    public class IVRS_Call_StatusDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMCS_Id { get; set; }
        public string IMCS_VirtualNo { get; set; }
        public long IMCS_MI_Id { get; set; }
        public string IMCS_SchoolName { get; set; }
        public string IMCS_URL { get; set; }
        public long IMCS_Year { get; set; }
        public string IMCS_Month { get; set; }
        public long IMCS_AssignedCall { get; set; }
        public long IMCS_InboundCalls { get; set; }
        public long IMCS_OutboundCalls { get; set; }
        public long IMCS_AvailableCalls { get; set; }
        public bool IMCS_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
