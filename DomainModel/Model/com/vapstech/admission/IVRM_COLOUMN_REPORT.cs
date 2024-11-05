using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("IVRM_COLOUMN_REPORT")]
    public class IVRM_COLOUMN_REPORT
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRM_CLM_ID { get; set; }

    
        public string IVRM_CLM_NAME { get; set; }
        public string IVRM_CLM_PAR { get; set; }
      
    }
}
