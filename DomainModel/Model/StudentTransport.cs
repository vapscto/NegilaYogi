using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Preadmission_School_Registration_TransportDetails")]
    public class StudentTransport : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASRT_Id { get; set; }
        public long PASR_Id { get; set; }
        public string PASRT_ivrmmcT_Id { get; set; }
        public long PASRT_cmR_Id { get; set; }
        public long PASRT_cmL_Id { get; set; }
        public long PASRT_consession_type_Id { get; set; }
        public int? PASRT_Daughter { get; set; }
        public int? PASRT_Son { get; set; }
        public int? PASRT_Heared_Friend_Colleague { get; set; }
        public int? PASRT_Internet { get; set; }
        public int? PASRT_Media { get; set; }
        public int? PASRT_Other { get; set; }
    }
}
