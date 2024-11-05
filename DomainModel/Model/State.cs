using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Master_State")]
    public class State : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMS_Id { get; set; }
        public string IVRMMS_Name { get; set; }
       
        public string IVRMMS_Code { get; set; }
        public long IVRMMC_Id { get; set; }
        public bool? IVRMMS_ActiveFlag { get; set; }
        public long? IVRMMS_CreatedBy { get; set; }
        public long? IVRMMS_UpdatedBy { get; set; }
        public int? IVRMMS_Default { get; set; }

    }
}
