using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Sports
{
    [Table("SPCC_Master_House_Designation", Schema = "SPC")]
    public class SportMasterHouseDessignationDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SPCCMHD_Id { get; set; }
        public long MI_Id { get; set; }
        public string SPCCMHD_DesignationName { get; set; }
        public string SPCCMHD_DesignationDescription { get; set; }
        public bool SPCCMHD_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
