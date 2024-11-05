using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("Lib_M_Floor", Schema ="LIB")]
    public class MasterFloorDMO:CommonParamDMO
    {
        [Key]
        public long Floor_Id { get; set; }
        public long MI_Id { get; set; }
        public string FloorName { get; set; }
        public bool Floor_ActiveFlag { get; set; }

    }
}
