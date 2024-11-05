using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Student_FriendRequest", Schema = "ALU")]
    public class Alumni_Student_FriendRequestDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ALSFRNDREQ_Id { get; set; }
        public long ALMST_Id { get; set; }
        public long ALSFRNDREQ_FriendsReqId { get; set; }
        public DateTime ALSFRNDREQ_RequestDate { get; set; }
        public bool ALSFRNDREQ_AcceptFlg { get; set; }
        public DateTime ALSFRNDREQ_AcceptedDate { get; set; }
        public bool ALSFRNDREQ_ActiveFlg { get; set; }
        public DateTime? ALSFRNDREQ_CreatedDate { get; set; }
        public long ALSFRNDREQ_CreatedBy { get; set; }
        public DateTime? ALSFRNDREQ_UpdatredDate { get; set; }
        public long ALSFRNDREQ_UpdatedBy { get; set; }
    }
}
