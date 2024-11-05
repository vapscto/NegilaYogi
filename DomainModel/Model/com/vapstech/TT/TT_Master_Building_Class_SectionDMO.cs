using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.TT
{
    [Table("TT_Master_Building_Class_Section")]
    public class TT_Master_Building_Class_SectionDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TTMBCS_Id {get;set;}
        public long TTMB_Id { get;set;}
        public long ASMCL_Id { get;set;}
        public long ASMS_Id { get;set;}
        public bool TTMBCS_ActiveFlag { get; set; }

    }
}
