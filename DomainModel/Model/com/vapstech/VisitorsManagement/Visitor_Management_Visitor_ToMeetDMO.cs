using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.VisitorsManagement
{
    [Table("Visitor_Management_Visitor_ToMeet", Schema = "VM")]
    public class Visitor_Management_Visitor_ToMeetDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long VMVTMT_Id { get; set; }
        public long MI_Id { get; set; }
        public long VMMV_Id { get; set; }
        public long VMVTMT_ToMeet_HRME_Id { get; set; }
        public long HRME_Id { get; set; }     
        public bool VMVTMT_MetFlg { get; set; }
        public string VMVTMT_Remarks { get; set; }
        public string VMVTMT_Location { get; set; }
        public DateTime? VMVTMT_DateTime { get; set; }
        public bool VMVTMT_Flg { get; set; }
        public long VMVTMT_CreatedBy { get; set; }
        public DateTime? VMVTMT_CreatedDate { get; set; }
        public long VMVTMT_UpdatedBy { get; set; }
        public DateTime? VMVTMT_UpdatedDate { get; set; }
    }
}