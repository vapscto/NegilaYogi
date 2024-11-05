using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_UOM", Schema = "SPC")]
    public class SportMasterUOMDMO
    {
        public long SPCCMUOM_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMUOM_UOMName { get; set; }
        public bool SPCCMUOM_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
