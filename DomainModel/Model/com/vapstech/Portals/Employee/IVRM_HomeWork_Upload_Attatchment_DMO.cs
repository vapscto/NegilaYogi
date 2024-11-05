using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel.Model.com.vapstech.Portals.Employee
{
    [Table("IVRM_HomeWork_Upload_Attatchment")]
    public class IVRM_HomeWork_Upload_Attatchment_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long IHWUPLATT_Id { get; set; }
        public long IHWUPL_Id { get; set; }
        public string IHWUPLATT_FileName { get; set; }
        public string IHWUPLATT_Attachment { get; set; }
        public string IHWUPLATT_StaffUpload { get; set; }
        public string IHWUPLATT_StaffRemarks { get; set; }
        public bool IHWUPLATT_ActiveFlag { get; set; }
        public DateTime? IHWUPLATT_CreatedDate { get; set; }
        public DateTime? IHWUPLATT_UpdatedDate { get; set; }
    }
}
