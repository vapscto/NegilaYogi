using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vaps.Fee
{
    [Table("FEE_ECS_DETAILS")]
    public class FEE_ECS_DETAILSDMO: CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FECD_Id { get; set; }
        public long AMST_Id { get; set; }
        public string Receipt_No { get; set; }
        public long ASMAY_Id { get; set; }
        public string Transaction_Id { get; set; }
        public string Guardian_Name { get; set; }
       
    }
}
