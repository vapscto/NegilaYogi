using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("Adm_Student_Transport_Application_Update")]
    public class Adm_Student_Transport_Application_UpdateDMO  
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ASTAU_Id { get; set; }
        public long ASTA_Id { get; set; }
       
        public long TRMA_Id  { get; set; }
        public DateTime ASTA_ApplicationDate { get; set; }
     
        public long ASTA_PickUp_TRMR_Id { get; set; }
        public long ASTA_PickUp_TRML_Id { get; set; }
        public long ASTA_Drop_TRMR_Id { get; set; }
        public long ASTA_Drop_TRML_Id { get; set; }
        public string ASTA_Landmark { get; set; }

        public long TRMAU_Id { get; set; }
        public DateTime ASTAU_UpdateDate  { get; set; }

        public long ASTAU_PickUp_TRMR_Id { get; set; }
        public long ASTAU_PickUp_TRML_Id { get; set; }
        public long ASTAU_Drop_TRMR_Id { get; set; }
        public long ASTAU_Drop_TRML_Id { get; set; }
        public string ASTAU_Landmark { get; set; }

        public string ASTAU_Type  { get; set; }

    }
}
