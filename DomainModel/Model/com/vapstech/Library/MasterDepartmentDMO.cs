using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Department", Schema ="LIB")]
    public class MasterDepartmentDMO:CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LMD_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMD_DepartmentName { get; set; }
        public bool LMD_ActiveFlg { get; set; }
        public string LMD_DepartmentCode { get; set; }


    }
}
