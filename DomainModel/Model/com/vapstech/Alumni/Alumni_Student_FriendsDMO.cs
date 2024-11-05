using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Student_Friends", Schema = "ALU")]
    public class Alumni_Student_FriendsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]


        public long ALSFRND_Id { get; set; }
        public long ALMST_Id { get; set; }
        public long ALSFRND_FriendsId { get; set; }
        public DateTime? ALSFRND_RequestDate { get; set; }
        public DateTime? ALSFRND_AcceptedDate { get; set; }
        public bool ALSFRND_ActiveFlg { get; set; }
        public long ALSFRND_CreatedBy { get; set; }
        public string ALSFRND_AcceptFlag { get; set; }
        public DateTime? ALSFRND_CreatedDate { get; set; }
        public long ALSFRND_UpdatedBy { get; set; }
        public DateTime? ALSFRND_UpdatedDate { get; set; }
    }
}
