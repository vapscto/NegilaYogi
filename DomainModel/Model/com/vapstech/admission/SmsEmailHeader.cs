using DomainModel.Model.com.vapstech.admission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vaps.admission
{
    [Table("IVRM_Header")]
    public class SmsEmailHeader:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      public long  IVRMHE_Id { get; set; }
      public long  MI_Id { get; set; }
      public long  IVRMIM_Id { get; set; }
      public long  IVRMIMP_Id { get; set; }
      public string  IVRMHE_Name { get; set; }
      
        public List<SmsEmailParameterMappingDMO> SmsEmailParameterMappingDMO { get; set; }
    }
}
