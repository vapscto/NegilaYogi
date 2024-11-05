using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model
{
    [Table("IVRM_Prospectus_Path")]
    public class ProspectusFilePath
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IPPC_Id { get; set; }
        public string IPPC_Path { get; set; }
        public string IPPC_FileName { get; set; }
        public long MI_ID { get; set; }

    }
}
