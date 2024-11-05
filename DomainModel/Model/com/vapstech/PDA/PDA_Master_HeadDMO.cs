using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.PDA
{
    [Table("PDA_Master_Head")]
    public class PDA_Master_HeadDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PDAMH_Id { get; set; }
        public long MI_Id { get; set; }
        public string PDAMH_HeadName { get; set; }
        public bool PDAMH_ActiveFlag { get; set; }
        public long FMH_ID { get; set; }
       // public DateTime CreatedDate { get; set; }
     //   public DateTime UpdatedDate { get; set; }
    }
}
