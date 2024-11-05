using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Library
{
    [Table("LIB_Master_Rack", Schema ="LIB")]
   public class RackDetailsDMO:CommonParamDMO
    {
        [Key]
        public long LMRA_Id { get; set; }
        public long MI_Id { get; set; }
        public string LMRA_RackName { get; set; }
        public string LMRA_DisplayColour { get; set; }
        public string LMRA_BuildingName { get; set; }
        public string LMRA_FloorName { get; set; }            
        public string LMRA_Location { get; set; }
        public bool LMRA_ActiveFlag { get; set; }

        public List<Lib_Rack_SubjectDMO> Lib_Rack_SubjectDMO { get; set; }
    }
}
