using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.College.Admission
{
    [Table("Adm_Master_College_Category", Schema = "CLG")]
    public class ClgMasterCategoryDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AMCOC_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMCOC_Name { get; set; }
        public string AMCOC_Address { get; set; }
        public string AMCOC_Details { get; set; }
        public string AMCOC_Type { get; set; }
        public string ACMC_RegNoPrefix { get; set; }
        public bool ACMC_ActiveFlag { get; set; }
    }
}
