using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_House", Schema = "SPC")]
    public class SportMasterHouseDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMH_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMH_HouseName { get; set; }
        public string SPCCMH_HouseDescription { get; set; }
        public bool SPCCMH_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string SPCCMH_Flag { get; set; }

    }
}
