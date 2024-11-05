using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("TR_Master_Location", Schema = "TRN")]
    public class MasterLocationDMO :CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TRML_Id { get; set; }
        public long MI_Id { get; set; }
        //public long TRMA_Id { get; set; }
        public string TRML_Latitude { get; set; }
        public string TRML_Longitude { get; set; }
        public bool TRML_ActiveFlg { get; set; }
        public string TRML_LocationName { get; set; }
    }
}
