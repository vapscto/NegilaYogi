using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Category", Schema = "LIB")]
    public class MasterCategoryDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMC_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMC_CategoryName { get; set; }
        public string LMC_BNBFlg { get; set; }
        public bool LMC_ActiveFlag { get; set; }
        public string LMC_CategoryCode { get; set; }

        public string AMC_Logo { get; set; }
        public string AMC_FilePath { get; set; }

    }
}
