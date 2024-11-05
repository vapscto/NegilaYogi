using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Alumni
{
    [Table("Alumni_Master_MembershipCategory", Schema = "ALU")]
    public class Alumni_Master_MembershipCategoryDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ALMMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string ALMMC_MembershipCategory { get; set; }
        public string ALMMC_MembershipCategoryDesc { get; set; }
        public bool ALMMC_ActiveFlg { get; set; }
        public long ALMMC_CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ALMMC_UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
