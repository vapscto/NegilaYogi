using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.admission
{
    [Table("Adm_Master_Batch")]
    public class Adm_Master_Batch :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMBA_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMAY_Id { get; set; }
        public string AMBA_Name { get; set; }
        public string AMBA_Description { get; set; }
    }
}
