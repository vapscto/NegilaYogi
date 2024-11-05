using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("SMS_Sent_Details_LoginUser")]
    public class SMS_Sent_Details_LoginUserDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SSDLU_Id { get; set; }
        public long SSD_Id { get; set; }
        public long IVRMUL_Id { get; set; }
    }
}
