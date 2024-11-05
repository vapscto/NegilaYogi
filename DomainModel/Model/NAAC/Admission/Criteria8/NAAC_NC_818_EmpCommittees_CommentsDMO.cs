using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_NC_818_EmpCommittees_Comments")] 
   public class NAAC_NC_818_EmpCommittees_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCNC8111ECC_Id { get; set; }
        public long NCNC8111ECC_RemarksBy { get; set; }
        public long NCNC8111ECC_CreatedBy { get; set; }
        public long NCNC8111ECC_UpdatedBy { get; set; }
        public long NCNC8111EC_Id { get; set; }
        public string NCNC8111ECC_Remarks { get; set; }
        public string NCNC8111ECC_StatusFlg { get; set; }
        public bool NCNC8111ECC_ActiveFlag { get; set; }
        public DateTime? NCNC8111ECC_CreatedDate { get; set; }
        public DateTime? NCNC8111ECC_UpdatedDate { get; set; }
    }
}
