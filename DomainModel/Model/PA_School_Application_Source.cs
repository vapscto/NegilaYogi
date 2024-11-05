using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model
{
    [Table("PA_School_Application_Source")]
    public class PA_School_Application_Source
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASAS_Id { get; set; }
        public long PAMS_Id { get; set; }
        
        public long PASR_Id { get; set; }
        public bool PASAS_ActiveFlg { get; set; }
        public DateTime PASAS_CreatedDate { get; set; }
        public DateTime PASAS_UpdatedDate { get; set; }
        public long? PASAS_CreatedBy { get; set; }
        public long? PASAS_UpdatedBy { get; set; }


        //public List<MasterSource> MasterSource { get; set; }
    }
}
