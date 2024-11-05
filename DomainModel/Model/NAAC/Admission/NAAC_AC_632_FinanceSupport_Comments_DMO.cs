using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_632_FinanceSupport_Comments")]
    public class NAAC_AC_632_FinanceSupport_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCAC632FINSUPC_Id { get; set; }
        public string NCAC632FINSUPC_Remarks { get; set; }
        public long? NCAC632FINSUPC_RemarksBy { get; set; }
        public string NCAC632FINSUPC_StatusFlg { get; set; }
        public bool? NCAC632FINSUPC_ActiveFlag { get; set; }
        public long? NCAC632FINSUPC_CreatedBy { get; set; }
        public DateTime? NCAC632FINSUPC_CreatedDate { get; set; }
        public long? NCAC632FINSUPC_UpdatedBy { get; set; }
        public DateTime? NCAC632FINSUPC_UpdatedDate { get; set; }
        public long NCAC632FINSUP_Id { get; set; }
    }
}
