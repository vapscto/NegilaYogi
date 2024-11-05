using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DomainModel.Model.NAAC.Admission.Criteria8
{
    [Table("NAAC_NC_818_EmpCommittees_Files")]
    public class NC_818_EmpCommitteesFilesDMO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NCNC8111ECF_Id { get; set; }
        public long NCNC8111EC_Id { get; set; }
        public string NCNC8111ECF_FileDesc { get; set; }
        public string NCNC8111ECF_FileName { get; set; }
        public string NCNC8111ECF_FilePath { get; set; }
        public string NCNC8111ECF_StatusFlg { get; set; }
        public bool NCNC8111ECF_ActiveFlg { get; set; }
        public DateTime? NCNC8111ECF_CreatedDate { get; set; }
        public DateTime? NCNC8111ECF_UpdatedDate { get; set; }
        public long NCNC8111ECF_CreatedBy { get; set; }
        public long NCNC8111ECF_UpdatedBy { get; set; }
    }
}
