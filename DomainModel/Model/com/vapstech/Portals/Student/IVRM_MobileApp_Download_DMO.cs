using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Student
{
    [Table("IVRM_MobileApp_Download")]
    public class IVRM_MobileApp_Download_DMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IVRMMAD_Id { get; set; }
        public long MI_Id {get;set;}
        public long IVRMUL_Id { get; set; }
        public string  IVRMMAD_MobileModel{get;set;}
        public DateTime IVRMMAD_DownlaodDateTime {get;set;}

    }
}
