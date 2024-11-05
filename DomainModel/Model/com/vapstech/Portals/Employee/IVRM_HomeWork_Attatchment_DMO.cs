using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_HomeWork_Attatchment")]
    public class IVRM_HomeWork_Attatchment_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IHWATT_Id { get; set; }
        public long IHW_Id { get; set; }
        public string IHWATT_FileName { get; set; }
        public string IHWATT_Attachment { get; set; }
        public bool IHWATT_ActiveFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        


    }
}
