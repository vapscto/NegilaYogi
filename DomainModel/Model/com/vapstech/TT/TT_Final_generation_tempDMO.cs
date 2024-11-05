using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Final_generation_temp")]
    public class TT_Final_generation_tempDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ASMCL_Id { get; set; }
        public int ASMS_Id { get; set; }
        public int HRME_Id { get; set; }
        public int ISMS_ID { get; set; }
        public int TTMD_ID { get; set; }
        public int TTMP_ID { get; set; }
        public int MI_Id { get; set; }
        public int ASMAY_Id { get; set; }
        public int TypeofRemove { get; set; }
    }
}
