using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model.com.vapstech.Transport
{
    [Table("Adm_Student_Trans_Appl_Update_College")]
    public class CLGAdm_Student_Trans_Appl_update_CollegeDMO : CommonParamDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ASTACOU_Id { get; set; }
        public long ASTACO_Id { get; set; }
        public DateTime ASTACO_ApplicationDate { get; set; }
        public long TRMA_Id { get; set; }
        public long ASTACO_PickUp_TRMR_Id { get; set; }
        public long ASTACO_PickUp_TRML_Id { get; set; }
        public long ASTACO_Drop_TRMR_Id { get; set; }
        public long ASTACO_Drop_TRML_Id { get; set; }
        public string ASTACO_Landmark { get; set; }
        public DateTime ASTAUCO_UpdateDate { get; set; }
        public long TRMAU_Id { get; set; }
        public long ASTACOU_PickUp_TRMR_Id { get; set; }
        public long ASTACOU_PickUp_TRML_Id { get; set; }
        public long ASTACOU_Drop_TRMR_Id { get; set; }
        public long ASTACOU_Drop_TRML_Id { get; set; }
        public string ASTACOU_Landmark { get; set; }
        public string ASTACOU_Type { get; set; }
    }
}
