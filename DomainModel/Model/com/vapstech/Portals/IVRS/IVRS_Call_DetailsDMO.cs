using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Portals.IVRS
{
    [Table("IVRS_Call_Details")]
    public class IVRS_Call_DetailsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IMCD_Id { get; set; }
        public string IMCD_VirtualNo { get; set; }
        public long IMCD_MI_Id { get; set; }
        public string IMCD_SchoolName { get; set; }
        public string IMCD_URL { get; set; }
        public long IMCD_MobileNo { get; set; }
        public string IMCD_InOutFlg { get; set; }
        public DateTime? IMCD_DateTime { get; set; }
        public string IMCD_CallStatus { get; set; }
        public string IMCD_CallDuration { get; set; }
        public long IMCD_PulseCount { get; set; }
        public string IMCD_IVRSText { get; set; }
        public string IMCD_IVRSVoiceURL { get; set; }
        public long IMCD_CreatedBy { get; set; }
        public long IMCD_UpdatedBy { get; set; }
        public bool IMCD_ActiveFlg { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
