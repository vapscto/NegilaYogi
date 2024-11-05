using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_ClassWork_Upload_Attatchment")]
    public class IVRM_ClassWork_Upload_Attatchment_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ICWUPLATT_Id { get; set; }
        public long ICWUPL_Id { get; set; }
        public string ICWUPLATT_FileName { get; set; }
        public string ICWUPLATT_Attachment { get; set; }
        public string ICWUPLATT_StaffUpload { get; set; }
        public string ICWUPLATT_StaffRemarks { get; set; }
        public bool ICWUPLATT_ActiveFlag { get; set; }
        public DateTime? ICWUPLATT_CreatedDate { get; set; }
        public DateTime? ICWUPLATT_UpdatedDate { get; set; }
    }
}
