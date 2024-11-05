using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_ClassWork_Attatchment")]
   public class IVRM_ClassWork_Attatchment_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ICWATT_Id { get; set; }
        public long ICW_Id { get; set; }
        public string ICWATT_FileName { get; set; }
        public string ICWATT_Attachment { get; set; }
        public bool ICWATT_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
