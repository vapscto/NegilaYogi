using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.HRMS
{
    [Table("HR_PROCESS_PRIVILEGE")]
    public class HR_PROCESS_PRIVILEGE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HR_PR_ID { get; set; }

        public string HR_PR_NAME { get; set; }

        public string HR_PR_DESC { get; set; }
    }
}
