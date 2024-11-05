using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("IVRM_Module_Page")]
    public class MasterPageModuleMapping : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMP_Id { get; set; }
        public long IVRMM_Id { get; set; }
        public long IVRMP_Id { get; set; }
        public MasterPage masterPage { get; set; }
        public MasterModule mastermodule { get; set; }

       // public MasterPage[] masterPagemultiple { get; set; }

        //public List<MasterPage> masterPageMany { get; set; }
    }

    //public class pagemodulemapping
    //{
    //    public long IVRMP_ID { get; set; }
    //    public virtual MasterPage page { get; set; }

    //    public long IVRMM_ID { get; set; }
    //    public virtual MasterModule question { get; set; }
    //}
}
