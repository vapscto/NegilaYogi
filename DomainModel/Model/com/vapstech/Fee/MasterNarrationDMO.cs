using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Fee
{

    [Table("Fee_Master_Narration")]
    public class MasterNarrationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FMNAR_Id { get; set; }
        public long MI_ID { get; set; }
    
        public string FMNAR_Narration { get; set; }
        public string FMNAR_NarrationDesc { get; set; }
        public bool FMNAR_ActiveFlag { get; set; }

    }
}
