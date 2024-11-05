using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.Fee
{
    [Table("Fee_Group_Login_Previledge")]
    public class FEeGroupLoginPreviledgeDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long FGL_Id { get; set; }
        public long FMG_ID { get; set; }
        public long User_Id { get; set; }
        public long FMH_Id { get; set; }
        public long MI_ID { get; set; }
      
    }
}
