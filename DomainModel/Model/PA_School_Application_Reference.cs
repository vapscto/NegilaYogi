using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model
{
    [Table("PA_School_Application_Reference")]
    public class PA_School_Application_Reference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PASAR_Id { get; set; }

        public long PAMR_Id { get; set; }  
       
        public long PASR_Id { get; set; }
        public bool PASAR_ActiveFlg { get; set; }
        public DateTime PASAR_CreatedDate { get; set; }
        public DateTime PASAR_UpdatedDate { get; set; }
        public long PASAR_CreatedBy { get; set; }
        public long PASAR_UpdatedBy { get; set; }


        //public List<MasterReference> MasterReference { get; set; }
    }
}
