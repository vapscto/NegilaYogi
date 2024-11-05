using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DomainModel.Model.NAAC.Admission
{
    [Table("NAAC_AC_632_FinanceSupport_File_Comments")]
    public class NAAC_AC_632_FinanceSupport_File_Comments_DMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long NCAC632FINSUPFC_Id { get; set; }
        public string NCAC632FINSUPFC_Remarks { get; set; }
        public long? NCAC632FINSUPFC_RemarksBy { get; set; }
        public bool? NCAC632FINSUPFC_ActiveFlag { get; set; }
        public long? NCAC632FINSUPFC_CreatedBy { get; set; }
        public DateTime? NCAC632FINSUPFC_CreatedDate { get; set; }
        public long? NCAC632FINSUPFC_UpdatedBy { get; set; }
        public DateTime? NCAC632FINSUPFC_UpdatedDate { get; set; }
        public string NCAC632FINSUPFC_StatusFlg { get; set; }
        public long NCAC632FINSUPF_Id { get; set; }
    }
}
