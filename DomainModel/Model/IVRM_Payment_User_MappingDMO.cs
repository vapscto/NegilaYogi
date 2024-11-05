using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Payment_User_Mapping")]
    public class IVRM_Payment_User_MappingDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMPUM_Id { get; set; }
        public long MI_Id { get; set; }
        public long? User_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool IVRMPUM_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
