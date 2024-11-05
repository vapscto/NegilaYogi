using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_M_Category")]

    
    public class MasterCategory : CommonParamDMO

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMC_Id { get; set; }
        [ForeignKey("MI_Id")]
        public long MI_Id { get; set; }
        public string AMC_Name { get; set; }
        public string AMC_Address { get; set; }
        public string AMC_Details { get; set; }
        public string AMC_Type { get; set; }
        public string AMC_RegNoPrefix { get; set; }
        public int AMC_ActiveFlag { get; set; }
        public string AMC_PAApplicationName { get; set; }
        public string AMC_PAViewPath { get; set; }
        public string AMC_Logo { get; set; }
        public string AMC_FilePath { get; set; }

    }
}
