using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_NC_818_EmpCommittees_File_Comments")] 
   public class NAAC_NC_818_EmpCommittees_File_CommentsDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCNC8111ECFC_Id { get; set; }
        public long NCNC8111ECFC_RemarksBy { get; set; }
        public long NCNC8111ECFC_CreatedBy { get; set; }
        public long NCNC8111ECFC_UpdatedBy { get; set; }
        public long NCNC8111ECF_Id { get; set; }
        public string NCNC8111ECFC_Remarks { get; set; }
        public string NCNC8111ECFC_StatusFlg { get; set; }
        public bool NCNC8111ECFC_ActiveFlag { get; set; }
        public DateTime? NCNC8111ECFC_CreatedDate { get; set; }
        public DateTime? NCNC8111ECFC_UpdatedDate { get; set; }
    }
}
