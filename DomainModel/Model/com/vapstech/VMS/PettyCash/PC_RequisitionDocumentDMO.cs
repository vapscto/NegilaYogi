using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.com.vapstech.VMS.PettyCash
{
    [Table("PC_Requistion_Upload")]
    public class PC_RequisitionDocumentDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PCREQTNUP_Id { get; set; }
        public long PCREQTN_Id { get; set; }
        public string PCREQTNUP_FileName { get; set; }
        public string PCREQTNUP_FileLocation { get; set; }
        public bool PCREQTNUP_ActiveFlg { get; set; }
        public DateTime PCREQTNUP_CreatedDate { get; set; }
        public DateTime PCREQTNUP_UpdatedDate { get; set; }
        public long PCREQTNUP_CreatedBy { get; set; }
        public long PCREQTNUP_UpdatedBy { get; set; }

    }
}
