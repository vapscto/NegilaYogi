using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModel.Model
{
    [Table("PA_School_Application_ElectiveSujects")]
    public class PA_School_Application_ElectiveSujects 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long PASAES_Id { get; set; }
        public long PASR_Id  { get; set; }
        public long? ISMS_Id { get; set; }
        public bool PASAES_ActiveFlg { get; set; } 
        public bool PASAES_TransferredToExmFlg { get; set; }
        public long PASAES_CreatedBy { get; set; }
        public long PASAES_UpdatedBy { get; set; }
        public int EMG_Id { get; set; }
        public DateTime PASAES_CreatedDate { get; set; }
        public DateTime PASAES_UpdatedDate { get; set; }

    }
}
